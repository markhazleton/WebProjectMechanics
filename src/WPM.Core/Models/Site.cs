namespace WPM.Core.Models;

public class Site
{
    public int Id { get; set; }
    public required string SiteName { get; set; }
    public required string FolderName { get; set; }
    public string? ThemeName { get; set; }
    public string? HomePageSlug { get; set; }
    public string? GoogleAnalyticsId { get; set; }
    public string? ContactEmail { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public List<SiteDomain> Domains { get; set; } = [];
}

public class SiteDomain
{
    public int Id { get; set; }
    public int SiteId { get; set; }
    public required string Domain { get; set; }
    public bool IsPrimary { get; set; }
    public Site Site { get; set; } = null!;
}
