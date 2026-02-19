using Microsoft.EntityFrameworkCore;
using WPM.Core.Models;
using WPM.Domain.CMS.Data;
using WPM.Domain.CMS.Models;
using WPM.Infrastructure.Data;
using WPM.Infrastructure.Services;

namespace WPM.Migration;

/// <summary>
/// Migrates a single site (one CompanyID from one MDB) into core.db + per-site cms.db.
/// </summary>
static class SiteMigrator
{
    public static async Task<MigrationResult> MigrateAsync(
        MdbReader reader,
        LegacySiteConfig config,
        CoreDbContext coreDb,
        WpmPaths paths,
        CancellationToken ct = default)
    {
        var result = new MigrationResult(config.Domain);
        var domain = config.Domain.ToLowerInvariant();

        Console.WriteLine($"  Migrating {domain} (CompanyID {config.CompanyId})...");

        // 1. Read legacy Company
        var company = reader.ReadCompany(config.CompanyId);
        if (company is null)
        {
            result.AddWarning($"Company row not found for CompanyID {config.CompanyId}");
            return result;
        }

        // 2. Check if site already exists in core.db
        var existingSite = await coreDb.Sites
            .Include(s => s.Domains)
            .FirstOrDefaultAsync(s => s.FolderName == domain, ct);

        Site site;
        if (existingSite is not null)
        {
            site = existingSite;
            Console.WriteLine($"    Site already exists (Id={site.Id}), updating...");
            site.SiteName = company.CompanyName;
            site.ThemeName = company.SiteTemplate ?? company.DefaultSiteTemplate;
            site.ContactEmail = company.FromEmail;
            site.IsActive = company.ActiveFl;
            site.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            site = new Site
            {
                SiteName = company.CompanyName,
                FolderName = domain,
                ThemeName = company.SiteTemplate ?? company.DefaultSiteTemplate,
                ContactEmail = company.FromEmail,
                IsActive = company.ActiveFl
            };
            coreDb.Sites.Add(site);
        }

        await coreDb.SaveChangesAsync(ct);

        // 3. Ensure SiteDomain exists
        var existingDomain = await coreDb.SiteDomains
            .FirstOrDefaultAsync(d => d.Domain == domain, ct);
        if (existingDomain is null)
        {
            coreDb.SiteDomains.Add(new SiteDomain
            {
                SiteId = site.Id,
                Domain = domain,
                IsPrimary = true
            });
            await coreDb.SaveChangesAsync(ct);
        }

        // 4. Create site data folder
        var siteDataFolder = Path.Combine(paths.DataRoot, domain);
        Directory.CreateDirectory(siteDataFolder);
        Directory.CreateDirectory(Path.Combine(siteDataFolder, "media"));

        // 5. Create cms.db via factory
        using var cmsDb = CmsDbContextFactory.Create(siteDataFolder);

        // 6. Read legacy Pages
        var legacyPages = reader.ReadPages(config.CompanyId);
        result.LegacyPageCount = legacyPages.Count;

        // 7. First pass: insert Locations without ParentLocationId
        var pageIdMap = new Dictionary<int, int>(); // legacyPageId → newLocationId
        var slugCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        var locationBatch = new List<(int legacyPageId, Location location)>();

        foreach (var page in legacyPages)
        {
            var slug = SanitizeSlug(page.PageName);

            // Ensure slug uniqueness within this site
            if (slugCounts.TryGetValue(slug, out var count))
            {
                slugCounts[slug] = count + 1;
                slug = $"{slug}-{count + 1}";
            }
            else
            {
                slugCounts[slug] = 1;
            }

            var location = new Location
            {
                Title = page.PageTitle ?? page.PageName,
                Slug = slug,
                Description = page.PageDescription,
                Keywords = page.PageKeywords,
                TemplateName = page.PageFileName,
                SortOrder = page.PageOrder,
                IsActive = page.Active,
                ShowInNavigation = true,
                CreatedAt = page.ModifiedDt ?? DateTime.UtcNow,
                UpdatedAt = page.ModifiedDt ?? DateTime.UtcNow
            };
            locationBatch.Add((page.PageId, location));
            cmsDb.Locations.Add(location);
        }
        await cmsDb.SaveChangesAsync(ct);

        // Build ID map
        foreach (var (legacyPageId, location) in locationBatch)
        {
            pageIdMap[legacyPageId] = location.Id;
        }

        // 8. Second pass: set ParentLocationId
        foreach (var (legacyPageId, location) in locationBatch)
        {
            var legacyPage = legacyPages.First(p => p.PageId == legacyPageId);
            if (legacyPage.ParentPageId.HasValue &&
                pageIdMap.TryGetValue(legacyPage.ParentPageId.Value, out var newParentId))
            {
                location.ParentLocationId = newParentId;
            }
        }
        await cmsDb.SaveChangesAsync(ct);
        result.MigratedLocationCount = locationBatch.Count;

        // 9. Resolve HomePageSlug
        if (company.HomePageId.HasValue &&
            pageIdMap.TryGetValue(company.HomePageId.Value, out var homeLocId))
        {
            var homeLoc = locationBatch.FirstOrDefault(b => b.location.Id == homeLocId).location;
            if (homeLoc is not null)
            {
                site.HomePageSlug = homeLoc.Slug;
                await coreDb.SaveChangesAsync(ct);
            }
        }

        // 10. Read and insert Articles
        // When an article references an unmapped PageID, auto-create a Location from
        // the article title so no content is lost. Articles sharing the same orphan
        // PageID are grouped under one auto-created Location.
        var legacyArticles = reader.ReadArticles(config.CompanyId);
        result.LegacyArticleCount = legacyArticles.Count;
        var migratedArticles = 0;
        var autoCreatedLocations = 0;

        foreach (var art in legacyArticles)
        {
            int locationId;
            if (art.PageId.HasValue && pageIdMap.TryGetValue(art.PageId.Value, out var mappedId))
            {
                locationId = mappedId;
            }
            else
            {
                // Auto-create a Location for this orphan article.
                // If multiple articles share the same orphan PageID, reuse one Location.
                var orphanKey = art.PageId ?? -(art.ArticleId); // negative to avoid collision with real IDs
                if (!pageIdMap.TryGetValue(orphanKey, out locationId))
                {
                    var slug = SanitizeSlug(art.Title);
                    if (string.IsNullOrWhiteSpace(slug)) slug = $"article-{art.ArticleId}";

                    if (slugCounts.TryGetValue(slug, out var count))
                    {
                        slugCounts[slug] = count + 1;
                        slug = $"{slug}-{count + 1}";
                    }
                    else
                    {
                        slugCounts[slug] = 1;
                    }

                    var location = new Location
                    {
                        Title = art.Title,
                        Slug = slug,
                        IsActive = art.Active,
                        ShowInNavigation = false,
                        CreatedAt = art.ModifiedDt ?? DateTime.UtcNow,
                        UpdatedAt = art.ModifiedDt ?? DateTime.UtcNow
                    };
                    cmsDb.Locations.Add(location);
                    await cmsDb.SaveChangesAsync(ct);
                    locationId = location.Id;
                    pageIdMap[orphanKey] = locationId;
                    locationBatch.Add((orphanKey, location));
                    autoCreatedLocations++;
                }
            }

            cmsDb.Articles.Add(new Article
            {
                LocationId = locationId,
                Title = art.Title,
                Body = art.ArticleBody,
                Summary = art.ArticleSummary,
                SortOrder = art.SortOrder,
                IsActive = art.Active,
                CreatedAt = art.ModifiedDt ?? DateTime.UtcNow,
                UpdatedAt = art.ModifiedDt ?? DateTime.UtcNow
            });
            migratedArticles++;
        }
        await cmsDb.SaveChangesAsync(ct);
        result.MigratedArticleCount = migratedArticles;
        if (autoCreatedLocations > 0)
            result.MigratedLocationCount += autoCreatedLocations;

        // 11. Read and insert Images
        var imageIdMap = new Dictionary<int, int>(); // legacyImageId → newImageId
        try
        {
            var legacyImages = reader.ReadImages(config.CompanyId);
            result.LegacyImageCount = legacyImages.Count;

            // Track legacy→new mapping by inserting one at a time to capture assigned IDs
            foreach (var img in legacyImages)
            {
                if (string.IsNullOrWhiteSpace(img.ImageFileName)) continue;

                var image = new Image
                {
                    FileName = img.ImageFileName,
                    AltText = img.ImageName,
                    Caption = img.ImageDescription,
                    CreatedAt = img.ImageDate ?? img.ModifiedDt ?? DateTime.UtcNow
                };
                cmsDb.Images.Add(image);
                await cmsDb.SaveChangesAsync(ct);
                imageIdMap[img.ImageId] = image.Id;
            }
            result.MigratedImageCount = imageIdMap.Count;
        }
        catch (Exception ex)
        {
            result.AddWarning($"Image migration failed: {ex.Message}");
        }

        // 12. Read and insert PageImages → LocationImages
        try
        {
            var legacyPageIds = new HashSet<int>(legacyPages.Select(p => p.PageId));
            var legacyPageImages = reader.ReadPageImages(legacyPageIds);
            result.LegacyPageImageCount = legacyPageImages.Count;
            var migratedPageImages = 0;

            foreach (var pi in legacyPageImages)
            {
                if (!pi.PageId.HasValue || !pi.ImageId.HasValue) continue;
                if (!pageIdMap.TryGetValue(pi.PageId.Value, out var locId)) continue;
                if (!imageIdMap.TryGetValue(pi.ImageId.Value, out var imgId)) continue;

                cmsDb.LocationImages.Add(new LocationImage
                {
                    LocationId = locId,
                    ImageId = imgId,
                    Position = pi.PageImagePosition
                });
                migratedPageImages++;
            }
            await cmsDb.SaveChangesAsync(ct);
            result.MigratedLocationImageCount = migratedPageImages;
        }
        catch (Exception ex)
        {
            result.AddWarning($"PageImage migration failed: {ex.Message}");
        }

        // 13. Read and insert PageAliases → LocationAliases
        try
        {
            var legacyAliases = reader.ReadPageAliases(config.CompanyId);
            result.LegacyAliasCount = legacyAliases.Count;
            var migratedAliases = 0;

            foreach (var alias in legacyAliases)
            {
                if (string.IsNullOrWhiteSpace(alias.PageUrl)) continue;

                // Try to resolve TargetURL to a LocationId
                int? resolvedLocationId = null;
                string? targetUrl = alias.TargetUrl;

                if (!string.IsNullOrWhiteSpace(alias.TargetUrl))
                {
                    // Strategy 1: Try matching TargetURL as a page name/slug
                    var targetSlug = SanitizeSlug(alias.TargetUrl.TrimStart('/'));
                    var matchBySlug = locationBatch.FirstOrDefault(b =>
                        string.Equals(b.location.Slug, targetSlug, StringComparison.OrdinalIgnoreCase));
                    if (matchBySlug.location is not null)
                    {
                        resolvedLocationId = matchBySlug.location.Id;
                        targetUrl = null;
                    }
                    else
                    {
                        // Strategy 2: Try extracting path from URL
                        if (Uri.TryCreate(alias.TargetUrl, UriKind.Absolute, out var uri))
                        {
                            var pathSlug = SanitizeSlug(uri.AbsolutePath.TrimStart('/'));
                            var matchByPath = locationBatch.FirstOrDefault(b =>
                                string.Equals(b.location.Slug, pathSlug, StringComparison.OrdinalIgnoreCase));
                            if (matchByPath.location is not null)
                            {
                                resolvedLocationId = matchByPath.location.Id;
                                targetUrl = null;
                            }
                        }
                    }
                }

                cmsDb.LocationAliases.Add(new LocationAlias
                {
                    LocationId = resolvedLocationId,
                    AliasPath = alias.PageUrl,
                    TargetUrl = targetUrl
                });
                migratedAliases++;
            }
            await cmsDb.SaveChangesAsync(ct);
            result.MigratedAliasCount = migratedAliases;
        }
        catch (Exception ex)
        {
            result.AddWarning($"PageAlias migration failed: {ex.Message}");
        }

        // 14. Generate site.json (always runs, even if image/alias steps failed)
        SiteJsonGenerator.Generate(siteDataFolder, company, config, site.HomePageSlug);

        Console.WriteLine($"    Done: {result.MigratedLocationCount} locations, {result.MigratedArticleCount} articles, " +
                          $"{result.MigratedImageCount} images, {result.MigratedLocationImageCount} location-images, " +
                          $"{result.MigratedAliasCount} aliases");

        return result;
    }

    private static string SanitizeSlug(string input)
    {
        return input.Trim().ToLowerInvariant().Replace(" ", "-");
    }
}

class MigrationResult
{
    public string Domain { get; }
    public int LegacyPageCount { get; set; }
    public int MigratedLocationCount { get; set; }
    public int LegacyArticleCount { get; set; }
    public int MigratedArticleCount { get; set; }
    public int LegacyImageCount { get; set; }
    public int MigratedImageCount { get; set; }
    public int LegacyPageImageCount { get; set; }
    public int MigratedLocationImageCount { get; set; }
    public int LegacyAliasCount { get; set; }
    public int MigratedAliasCount { get; set; }
    public List<string> Warnings { get; } = [];

    public MigrationResult(string domain) => Domain = domain;

    public void AddWarning(string message)
    {
        Warnings.Add(message);
        Console.WriteLine($"    WARNING: {message}");
    }
}
