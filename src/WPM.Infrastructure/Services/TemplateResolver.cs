namespace WPM.Infrastructure.Services;

public class TemplateResolver(string fallbackTemplatesRoot)
{
    public string ResolveTemplatePath(string siteDataFolder, string templateName)
    {
        // Check site-specific templates first
        var sitePath = Path.Combine(siteDataFolder, "templates", templateName);
        if (File.Exists(sitePath)) return sitePath;

        // Fall back to default templates
        var fallbackPath = Path.Combine(fallbackTemplatesRoot, templateName);
        if (File.Exists(fallbackPath)) return fallbackPath;

        throw new FileNotFoundException(
            $"Template not found: {templateName} (checked {sitePath} and {fallbackPath})");
    }
}
