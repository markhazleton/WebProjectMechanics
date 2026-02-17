namespace WPM.Core.Models;

public record SiteContext(
    string SiteDomain,
    string DataFolder,
    string OutputFolder,
    string MediaFolder,
    SiteConfig Config);

public record SiteConfig(
    int SiteId,
    string Domain,
    string SiteName,
    string? ThemeName,
    string? HomePageSlug,
    string? GoogleAnalyticsId,
    string? ContactEmail,
    bool IsActive);
