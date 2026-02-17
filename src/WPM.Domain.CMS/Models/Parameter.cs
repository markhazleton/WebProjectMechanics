namespace WPM.Domain.CMS.Models;

public class Parameter
{
    public int Id { get; set; }
    public required string Key { get; set; }
    public string? Value { get; set; }
    public string? Description { get; set; }
}
