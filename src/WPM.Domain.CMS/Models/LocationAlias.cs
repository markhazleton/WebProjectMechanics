namespace WPM.Domain.CMS.Models;

public class LocationAlias
{
    public int Id { get; set; }
    public int LocationId { get; set; }
    public required string AliasPath { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Location Location { get; set; } = null!;
}
