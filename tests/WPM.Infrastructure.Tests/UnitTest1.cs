using Microsoft.EntityFrameworkCore;
using WPM.Core.Models;
using WPM.Infrastructure.Data;
using WPM.Infrastructure.Services;

namespace WPM.Infrastructure.Tests;

public class CoreDbContextTests : IDisposable
{
    private readonly CoreDbContext _db;

    public CoreDbContextTests()
    {
        var options = new DbContextOptionsBuilder<CoreDbContext>()
            .UseSqlite("Data Source=:memory:")
            .Options;
        _db = new CoreDbContext(options);
        _db.Database.OpenConnection();
        _db.Database.EnsureCreated();
    }

    [Fact]
    public async Task CanCreateAndQuerySite()
    {
        var site = new Site
        {
            SiteName = "Test Site",
            FolderName = "test.com",
            Domains = [new SiteDomain { Domain = "test.com", IsPrimary = true }]
        };
        _db.Sites.Add(site);
        await _db.SaveChangesAsync();

        var loaded = await _db.Sites
            .Include(s => s.Domains)
            .FirstOrDefaultAsync(s => s.FolderName == "test.com");

        Assert.NotNull(loaded);
        Assert.Equal("Test Site", loaded.SiteName);
        Assert.Single(loaded.Domains);
        Assert.Equal("test.com", loaded.Domains[0].Domain);
        Assert.True(loaded.Domains[0].IsPrimary);
    }

    [Fact]
    public async Task DomainUniqueConstraint()
    {
        _db.Sites.Add(new Site
        {
            SiteName = "A",
            FolderName = "a.com",
            Domains = [new SiteDomain { Domain = "shared.com", IsPrimary = true }]
        });
        await _db.SaveChangesAsync();

        _db.Sites.Add(new Site
        {
            SiteName = "B",
            FolderName = "b.com",
            Domains = [new SiteDomain { Domain = "shared.com", IsPrimary = true }]
        });

        await Assert.ThrowsAsync<DbUpdateException>(() => _db.SaveChangesAsync());
    }

    [Fact]
    public void WpmPaths_FromBaseDirectory_SetsCorrectPaths()
    {
        var baseDir = Path.Combine("var", "wpm");
        var paths = WpmPaths.FromBaseDirectory(baseDir);

        Assert.Equal(Path.Combine(baseDir, "data"), paths.DataRoot);
        Assert.Equal(Path.Combine(baseDir, "sites"), paths.SitesRoot);
        Assert.Equal(Path.Combine(baseDir, "core.db"), paths.CoreDbPath);
    }

    public void Dispose()
    {
        _db.Database.CloseConnection();
        _db.Dispose();
    }
}
