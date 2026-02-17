namespace WPM.Domain.CMS.Models;

public class Category
{
    public int Id { get; set; }
    public required string CategoryName { get; set; }
    public string? Description { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
}
