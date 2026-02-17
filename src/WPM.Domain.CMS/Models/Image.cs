namespace WPM.Domain.CMS.Models;

public class Image
{
    public int Id { get; set; }
    public required string FileName { get; set; }
    public string? AltText { get; set; }
    public string? Caption { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<LocationImage> LocationImages { get; set; } = [];
}

public class LocationImage
{
    public int Id { get; set; }
    public int LocationId { get; set; }
    public int ImageId { get; set; }
    public int Position { get; set; }

    public Location Location { get; set; } = null!;
    public Image Image { get; set; } = null!;
}
