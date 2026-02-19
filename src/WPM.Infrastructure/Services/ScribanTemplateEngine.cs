using Microsoft.Extensions.Caching.Memory;
using Scriban;
using Scriban.Runtime;
using WPM.Core.Interfaces;

namespace WPM.Infrastructure.Services;

public class ScribanTemplateEngine(IMemoryCache cache) : ITemplateEngine
{
    private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(5);

    public async Task<string> RenderAsync(string templatePath, object model, CancellationToken ct = default)
    {
        var template = await GetOrCompileTemplateAsync(templatePath, ct);
        return await RenderTemplateAsync(template, model);
    }

    public async Task<string> RenderStringAsync(string templateContent, object model, CancellationToken ct = default)
    {
        var template = Template.Parse(templateContent);
        if (template.HasErrors)
            throw new InvalidOperationException($"Template parse errors: {string.Join("; ", template.Messages)}");
        return await RenderTemplateAsync(template, model);
    }

    private async Task<Template> GetOrCompileTemplateAsync(string path, CancellationToken ct)
    {
        var cacheKey = $"scriban:{path}";
        if (cache.TryGetValue<Template>(cacheKey, out var cached))
            return cached!;

        var content = await File.ReadAllTextAsync(path, ct);
        var template = Template.Parse(content);
        if (template.HasErrors)
            throw new InvalidOperationException($"Template parse errors in {path}: {string.Join("; ", template.Messages)}");

        cache.Set(cacheKey, template, CacheTtl);
        return template;
    }

    private static async Task<string> RenderTemplateAsync(Template template, object model)
    {
        var context = new TemplateContext { MemberRenamer = member => member.Name };
        var scriptObject = new ScriptObject();
        scriptObject.Import(model, renamer: member => member.Name);
        context.PushGlobal(scriptObject);
        return await template.RenderAsync(context);
    }
}
