using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using WPM.Core.Interfaces;
using WPM.Core.Models;
using WPM.Infrastructure.Data;

namespace WPM.Infrastructure.Services;

public class SiteResolver(CoreDbContext coreDb, WpmPaths paths, IMemoryCache cache) : ISiteResolver
{
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

    public async Task<SiteContext?> ResolveAsync(string hostHeader, CancellationToken ct = default)
    {
        var host = NormalizeHost(hostHeader);

        if (cache.TryGetValue<SiteContext>($"site:{host}", out var cached))
            return cached;

        var siteDomain = await coreDb.SiteDomains
            .Include(d => d.Site)
            .FirstOrDefaultAsync(d => d.Domain == host, ct);

        if (siteDomain?.Site is not { IsActive: true } site)
            return null;

        var primaryDomain = await coreDb.SiteDomains
            .Where(d => d.SiteId == site.Id && d.IsPrimary)
            .Select(d => d.Domain)
            .FirstOrDefaultAsync(ct) ?? host;

        var config = new SiteConfig(
            SiteId: site.Id,
            Domain: primaryDomain,
            SiteName: site.SiteName,
            ThemeName: site.ThemeName,
            HomePageSlug: site.HomePageSlug,
            GoogleAnalyticsId: site.GoogleAnalyticsId,
            ContactEmail: site.ContactEmail,
            IsActive: site.IsActive);

        var dataFolder = Path.Combine(paths.DataRoot, site.FolderName);
        var outputFolder = Path.Combine(paths.SitesRoot, primaryDomain);
        var mediaFolder = Path.Combine(dataFolder, "media");

        var context = new SiteContext(primaryDomain, dataFolder, outputFolder, mediaFolder, config);

        cache.Set($"site:{host}", context, CacheDuration);
        return context;
    }

    private static string NormalizeHost(string host)
    {
        host = host.ToLowerInvariant();
        if (host.StartsWith("www."))
            host = host[4..];
        var colonIndex = host.IndexOf(':');
        if (colonIndex > 0)
            host = host[..colonIndex];
        return host;
    }
}
