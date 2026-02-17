namespace WPM.Domain.CMS.Models;

public class Location
{
    public int Id { get; set; }
    public int? ParentLocationId { get; set; }
    public required string Title { get; set; }
    public required string Slug { get; set; }
    public string? Description { get; set; }
    public string? Keywords { get; set; }
    public string? Body { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
    public bool ShowInNavigation { get; set; } = true;
    public string? TemplateName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public Location? Parent { get; set; }
    public List<Location> Children { get; set; } = [];
    public List<Article> Articles { get; set; } = [];
    public List<LocationImage> LocationImages { get; set; } = [];
    public List<LocationAlias> Aliases { get; set; } = [];
}
