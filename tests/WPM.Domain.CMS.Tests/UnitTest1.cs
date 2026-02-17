using Microsoft.EntityFrameworkCore;
using WPM.Domain.CMS.Data;
using WPM.Domain.CMS.Models;

namespace WPM.Domain.CMS.Tests;

public class CmsDbContextTests : IDisposable
{
    private readonly CmsDbContext _db;

    public CmsDbContextTests()
    {
        var options = new DbContextOptionsBuilder<CmsDbContext>()
            .UseSqlite("Data Source=:memory:")
            .Options;
        _db = new CmsDbContext(options);
        _db.Database.OpenConnection();
        _db.Database.EnsureCreated();
    }

    [Fact]
    public async Task CanCreateLocationHierarchy()
    {
        var parent = new Location { Title = "Home", Slug = "home" };
        _db.Locations.Add(parent);
        await _db.SaveChangesAsync();

        var child = new Location { Title = "About", Slug = "about", ParentLocationId = parent.Id };
        _db.Locations.Add(child);
        await _db.SaveChangesAsync();

        var loaded = await _db.Locations
            .Include(l => l.Children)
            .FirstOrDefaultAsync(l => l.Slug == "home");

        Assert.NotNull(loaded);
        Assert.Single(loaded.Children);
        Assert.Equal("About", loaded.Children[0].Title);
    }

    [Fact]
    public async Task CanCreateLocationWithArticles()
    {
        var location = new Location
        {
            Title = "Blog",
            Slug = "blog",
            Articles =
            [
                new Article { Title = "First Post", Body = "<p>Hello</p>", SortOrder = 1 },
                new Article { Title = "Second Post", Body = "<p>World</p>", SortOrder = 2 }
            ]
        };
        _db.Locations.Add(location);
        await _db.SaveChangesAsync();

        var loaded = await _db.Locations
            .Include(l => l.Articles.OrderBy(a => a.SortOrder))
            .FirstOrDefaultAsync(l => l.Slug == "blog");

        Assert.NotNull(loaded);
        Assert.Equal(2, loaded.Articles.Count);
        Assert.Equal("First Post", loaded.Articles[0].Title);
    }

    [Fact]
    public async Task CanCreateParts()
    {
        _db.Parts.Add(new Part { PartName = "header", PartContent = "<header>Logo</header>", PartType = "html" });
        _db.Parts.Add(new Part { PartName = "footer", PartContent = "<footer>Copyright</footer>", PartType = "html" });
        await _db.SaveChangesAsync();

        var parts = await _db.Parts.OrderBy(p => p.PartName).ToListAsync();
        Assert.Equal(2, parts.Count);
        Assert.Equal("footer", parts[0].PartName);
    }

    [Fact]
    public async Task CanCreateParametersAsKeyValue()
    {
        _db.Parameters.Add(new Parameter { Key = "theme", Value = "dark" });
        _db.Parameters.Add(new Parameter { Key = "analytics-id", Value = "G-12345" });
        await _db.SaveChangesAsync();

        var theme = await _db.Parameters.FirstOrDefaultAsync(p => p.Key == "theme");
        Assert.NotNull(theme);
        Assert.Equal("dark", theme.Value);
    }

    [Fact]
    public async Task CanCreateImageWithLocationJunction()
    {
        var location = new Location { Title = "Gallery", Slug = "gallery" };
        var image = new Image { FileName = "photo.jpg", AltText = "A photo" };
        _db.Locations.Add(location);
        _db.Images.Add(image);
        await _db.SaveChangesAsync();

        _db.LocationImages.Add(new LocationImage
        {
            LocationId = location.Id,
            ImageId = image.Id,
            Position = 1
        });
        await _db.SaveChangesAsync();

        var loaded = await _db.Locations
            .Include(l => l.LocationImages)
                .ThenInclude(li => li.Image)
            .FirstOrDefaultAsync(l => l.Slug == "gallery");

        Assert.NotNull(loaded);
        Assert.Single(loaded.LocationImages);
        Assert.Equal("photo.jpg", loaded.LocationImages[0].Image.FileName);
    }

    [Fact]
    public async Task CanCreateLocationAliases()
    {
        var location = new Location { Title = "About Us", Slug = "about" };
        _db.Locations.Add(location);
        await _db.SaveChangesAsync();

        _db.LocationAliases.Add(new LocationAlias { LocationId = location.Id, AliasPath = "/old-about-page" });
        _db.LocationAliases.Add(new LocationAlias { LocationId = location.Id, AliasPath = "/company/about" });
        await _db.SaveChangesAsync();

        var aliases = await _db.LocationAliases
            .Where(a => a.LocationId == location.Id)
            .ToListAsync();
        Assert.Equal(2, aliases.Count);
    }

    [Fact]
    public async Task DeletingLocationCascadesToArticles()
    {
        var location = new Location
        {
            Title = "Temp",
            Slug = "temp",
            Articles = [new Article { Title = "Article 1" }]
        };
        _db.Locations.Add(location);
        await _db.SaveChangesAsync();

        _db.Locations.Remove(location);
        await _db.SaveChangesAsync();

        Assert.Empty(await _db.Articles.ToListAsync());
    }

    public void Dispose()
    {
        _db.Database.CloseConnection();
        _db.Dispose();
    }
}
