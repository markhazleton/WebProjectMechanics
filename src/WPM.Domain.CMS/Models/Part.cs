namespace WPM.Domain.CMS.Models;

public class Part
{
    public int Id { get; set; }
    public required string PartName { get; set; }
    public string? PartContent { get; set; }
    public string? PartType { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
