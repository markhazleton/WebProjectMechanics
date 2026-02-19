using Microsoft.Extensions.Caching.Memory;
using WPM.Infrastructure.Services;

namespace WPM.Infrastructure.Tests;

public class ScribanTemplateEngineTests
{
    private readonly ScribanTemplateEngine _engine = new(new MemoryCache(new MemoryCacheOptions()));

    [Fact]
    public async Task RenderStringAsync_SubstitutesVariables()
    {
        var result = await _engine.RenderStringAsync(
            "Hello {{ Name }}!",
            new { Name = "World" });
        Assert.Equal("Hello World!", result);
    }

    [Fact]
    public async Task RenderStringAsync_HandlesNestedObjects()
    {
        var result = await _engine.RenderStringAsync(
            "{{ site.SiteName }} - {{ location.Title }}",
            new { site = new { SiteName = "TestSite" }, location = new { Title = "Home" } });
        Assert.Equal("TestSite - Home", result);
    }

    [Fact]
    public async Task RenderStringAsync_HandlesForLoops()
    {
        var result = await _engine.RenderStringAsync(
            "{{ for a in articles }}{{ a.Title }},{{ end }}",
            new { articles = new[] { new { Title = "A" }, new { Title = "B" } } });
        Assert.Equal("A,B,", result);
    }

    [Fact]
    public async Task RenderStringAsync_HandlesConditionals()
    {
        var result = await _engine.RenderStringAsync(
            "{{ if show }}Visible{{ else }}Hidden{{ end }}",
            new { show = true });
        Assert.Equal("Visible", result);
    }

    [Fact]
    public async Task RenderStringAsync_HandlesNullProperties()
    {
        var result = await _engine.RenderStringAsync(
            "Value: {{ value }}",
            new { value = (string?)null });
        Assert.Equal("Value: ", result);
    }

    [Fact]
    public async Task RenderStringAsync_ThrowsOnInvalidTemplate()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _engine.RenderStringAsync("{{ if }}", new { }));
    }

    [Fact]
    public async Task RenderAsync_LoadsFromFile()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), $"wpm-test-{Guid.NewGuid():N}");
        Directory.CreateDirectory(tempDir);
        try
        {
            var templatePath = Path.Combine(tempDir, "test.scriban");
            await File.WriteAllTextAsync(templatePath, "Hello {{ Name }}!");

            var result = await _engine.RenderAsync(templatePath, new { Name = "File" });
            Assert.Equal("Hello File!", result);
        }
        finally
        {
            Directory.Delete(tempDir, true);
        }
    }
}
