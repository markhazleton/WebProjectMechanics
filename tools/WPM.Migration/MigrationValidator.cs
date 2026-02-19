using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using WPM.Domain.CMS.Data;
using WPM.Infrastructure.Data;
using WPM.Infrastructure.Services;

namespace WPM.Migration;

/// <summary>
/// Validates migration output: row counts, FK integrity, structure checks.
/// </summary>
static class MigrationValidator
{
    public static async Task ValidateAsync(
        CoreDbContext coreDb,
        WpmPaths paths,
        string reportPath,
        CancellationToken ct = default)
    {
        Console.WriteLine("WPM Migration Validator");
        Console.WriteLine("=======================");

        var report = new StringBuilder();
        report.AppendLine("# Migration Validation Report");
        report.AppendLine();
        report.AppendLine($"**Generated:** {DateTime.Now:yyyy-MM-dd HH:mm}");
        report.AppendLine();

        var sites = await coreDb.Sites
            .Include(s => s.Domains)
            .OrderBy(s => s.SiteName)
            .ToListAsync(ct);

        report.AppendLine($"**Total sites in core.db:** {sites.Count}");
        report.AppendLine();

        var totalIssues = 0;

        // Per-site validation
        report.AppendLine("## Site Validation Summary");
        report.AppendLine();
        report.AppendLine("| Domain | Locations | Articles | Images | LocImages | Aliases | Issues |");
        report.AppendLine("|--------|-----------|----------|--------|-----------|---------|--------|");

        foreach (var site in sites)
        {
            var domain = site.Domains.FirstOrDefault(d => d.IsPrimary)?.Domain ?? site.FolderName;
            var siteDataFolder = Path.Combine(paths.DataRoot, site.FolderName);
            var issues = new List<string>();

            // Structure checks
            if (!Directory.Exists(siteDataFolder))
            {
                issues.Add("Data folder missing");
                report.AppendLine($"| {domain} | - | - | - | - | - | **Folder missing** |");
                totalIssues++;
                continue;
            }

            var cmsDbPath = Path.Combine(siteDataFolder, "cms.db");
            if (!File.Exists(cmsDbPath))
            {
                issues.Add("cms.db missing");
                report.AppendLine($"| {domain} | - | - | - | - | - | **cms.db missing** |");
                totalIssues++;
                continue;
            }

            var siteJsonPath = Path.Combine(siteDataFolder, "site.json");
            if (!File.Exists(siteJsonPath))
                issues.Add("site.json missing");

            // Open cms.db and count
            using var cmsDb = CmsDbContextFactory.Create(siteDataFolder);

            var locationCount = await cmsDb.Locations.CountAsync(ct);
            var articleCount = await cmsDb.Articles.CountAsync(ct);
            var imageCount = await cmsDb.Images.CountAsync(ct);
            var locImageCount = await cmsDb.LocationImages.CountAsync(ct);
            var aliasCount = await cmsDb.LocationAliases.CountAsync(ct);

            // FK integrity: Articles → Locations
            var orphanArticles = await cmsDb.Articles
                .Where(a => !cmsDb.Locations.Any(l => l.Id == a.LocationId))
                .CountAsync(ct);
            if (orphanArticles > 0)
                issues.Add($"{orphanArticles} orphan articles");

            // FK integrity: LocationImages → Locations
            var orphanLocImages = await cmsDb.LocationImages
                .Where(li => !cmsDb.Locations.Any(l => l.Id == li.LocationId))
                .CountAsync(ct);
            if (orphanLocImages > 0)
                issues.Add($"{orphanLocImages} orphan location-images (loc)");

            // FK integrity: LocationImages → Images
            var orphanImgImages = await cmsDb.LocationImages
                .Where(li => !cmsDb.Images.Any(i => i.Id == li.ImageId))
                .CountAsync(ct);
            if (orphanImgImages > 0)
                issues.Add($"{orphanImgImages} orphan location-images (img)");

            // FK integrity: LocationAliases → Locations (only for non-null LocationId)
            var orphanAliases = await cmsDb.LocationAliases
                .Where(a => a.LocationId.HasValue && !cmsDb.Locations.Any(l => l.Id == a.LocationId))
                .CountAsync(ct);
            if (orphanAliases > 0)
                issues.Add($"{orphanAliases} orphan aliases");

            // FK integrity: Location ParentLocationId
            var orphanParents = await cmsDb.Locations
                .Where(l => l.ParentLocationId.HasValue && !cmsDb.Locations.Any(p => p.Id == l.ParentLocationId))
                .CountAsync(ct);
            if (orphanParents > 0)
                issues.Add($"{orphanParents} orphan parent refs");

            // HomePageSlug check
            if (!string.IsNullOrWhiteSpace(site.HomePageSlug))
            {
                var homeExists = await cmsDb.Locations.AnyAsync(l => l.Slug == site.HomePageSlug, ct);
                if (!homeExists)
                    issues.Add($"HomePageSlug '{site.HomePageSlug}' not found");
            }

            // site.json validity
            if (File.Exists(siteJsonPath))
            {
                try
                {
                    var json = await File.ReadAllTextAsync(siteJsonPath, ct);
                    JsonDocument.Parse(json);
                }
                catch
                {
                    issues.Add("site.json invalid JSON");
                }
            }

            totalIssues += issues.Count;
            var issueText = issues.Count == 0 ? "OK" : string.Join("; ", issues);
            report.AppendLine($"| {domain} | {locationCount} | {articleCount} | {imageCount} | {locImageCount} | {aliasCount} | {issueText} |");

            Console.WriteLine($"  {domain}: {locationCount} locs, {articleCount} arts, {imageCount} imgs — {(issues.Count == 0 ? "OK" : $"{issues.Count} issues")}");
        }

        report.AppendLine();
        report.AppendLine($"**Total issues found:** {totalIssues}");
        report.AppendLine();

        // Summary
        report.AppendLine("## Validation Result");
        report.AppendLine();
        if (totalIssues == 0)
            report.AppendLine("All sites passed validation. Migration data is consistent.");
        else
            report.AppendLine($"Found {totalIssues} issue(s) across {sites.Count} sites. Review details above.");

        File.WriteAllText(reportPath, report.ToString());
        Console.WriteLine();
        Console.WriteLine($"Validation report written to: {reportPath}");
        Console.WriteLine($"Result: {totalIssues} issues found across {sites.Count} sites.");
    }
}
