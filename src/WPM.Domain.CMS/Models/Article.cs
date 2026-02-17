namespace WPM.Domain.CMS.Models;

public class Article
{
    public int Id { get; set; }
    public int LocationId { get; set; }
    public required string Title { get; set; }
    public string? Body { get; set; }
    public string? Summary { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public Location Location { get; set; } = null!;
}
