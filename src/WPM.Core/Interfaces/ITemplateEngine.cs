namespace WPM.Core.Interfaces;

public interface ITemplateEngine
{
    Task<string> RenderAsync(string templatePath, object model, CancellationToken ct = default);
    Task<string> RenderStringAsync(string templateContent, object model, CancellationToken ct = default);
}
