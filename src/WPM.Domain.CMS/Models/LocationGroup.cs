namespace WPM.Domain.CMS.Models;

public class LocationGroup
{
    public int Id { get; set; }
    public required string GroupName { get; set; }
    public string? Description { get; set; }
    public int SortOrder { get; set; }
}
