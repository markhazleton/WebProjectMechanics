using WPM.Core.Models;

namespace WPM.Core.Tests;

public class SiteContextTests
{
    [Fact]
    public void SiteContext_RecordEquality_Works()
    {
        var config = new SiteConfig(1, "test.com", "Test Site", null, "home", null, null, true);
        var a = new SiteContext("test.com", "/data/test.com", "/sites/test.com", "/data/test.com/media", config);
        var b = new SiteContext("test.com", "/data/test.com", "/sites/test.com", "/data/test.com/media", config);

        Assert.Equal(a, b);
    }

    [Fact]
    public void SiteConfig_StoresAllProperties()
    {
        var config = new SiteConfig(
            SiteId: 42,
            Domain: "frogsfolly.com",
            SiteName: "Frog's Folly",
            ThemeName: "nature",
            HomePageSlug: "home",
            GoogleAnalyticsId: "G-12345",
            ContactEmail: "test@frogsfolly.com",
            IsActive: true);

        Assert.Equal(42, config.SiteId);
        Assert.Equal("frogsfolly.com", config.Domain);
        Assert.Equal("Frog's Folly", config.SiteName);
        Assert.Equal("nature", config.ThemeName);
        Assert.Equal("home", config.HomePageSlug);
        Assert.Equal("G-12345", config.GoogleAnalyticsId);
        Assert.Equal("test@frogsfolly.com", config.ContactEmail);
        Assert.True(config.IsActive);
    }

    [Fact]
    public void PublishableFile_StoresContentAndPath()
    {
        var file = new PublishableFile("index.html", "<h1>Hello</h1>", "text/html");

        Assert.Equal("index.html", file.RelativePath);
        Assert.Equal("<h1>Hello</h1>", file.Content);
        Assert.Equal("text/html", file.ContentType);
    }

    [Fact]
    public void Site_DefaultValues()
    {
        var site = new Site { SiteName = "Test", FolderName = "test.com" };

        Assert.True(site.IsActive);
        Assert.Empty(site.Domains);
        Assert.NotEqual(default, site.CreatedAt);
    }
}
