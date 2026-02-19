using Microsoft.EntityFrameworkCore;
using WPM.Core.Interfaces;
using WPM.Domain.CMS.Endpoints;
using WPM.Infrastructure.Data;
using WPM.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure paths
var wpmBase = builder.Configuration.GetValue<string>("WPM:BaseDirectory")
    ?? Path.Combine(builder.Environment.ContentRootPath, "wpm-data");
var paths = WpmPaths.FromBaseDirectory(wpmBase);
builder.Services.AddSingleton(paths);

// Ensure data directories exist
Directory.CreateDirectory(paths.DataRoot);
Directory.CreateDirectory(paths.SitesRoot);

// Core database (shared: sites, users, auth)
builder.Services.AddDbContext<CoreDbContext>(options =>
    options.UseSqlite($"Data Source={paths.CoreDbPath}"));

// Services
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ISiteResolver, SiteResolver>();
builder.Services.AddSingleton<ITemplateEngine, ScribanTemplateEngine>();

var app = builder.Build();

// Ensure core database is created and WAL mode is set
using (var scope = app.Services.CreateScope())
{
    var coreDb = scope.ServiceProvider.GetRequiredService<CoreDbContext>();
    coreDb.Database.EnsureCreated();

    var connection = coreDb.Database.GetDbConnection() as Microsoft.Data.Sqlite.SqliteConnection;
    if (connection is not null)
    {
        if (connection.State != System.Data.ConnectionState.Open)
            connection.Open();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = "PRAGMA journal_mode=WAL; PRAGMA busy_timeout=5000;";
        cmd.ExecuteNonQuery();
    }
}

// Health check endpoint (no site context needed)
app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }));

// Site management endpoints (no site middleware - these manage sites across the platform)
app.MapGet("/api/sites", async (CoreDbContext db) =>
{
    var sites = await db.Sites
        .Include(s => s.Domains)
        .OrderBy(s => s.SiteName)
        .ToListAsync();
    return Results.Ok(sites.Select(s => new
    {
        s.Id,
        s.SiteName,
        s.FolderName,
        s.IsActive,
        Domains = s.Domains.Select(d => new { d.Domain, d.IsPrimary })
    }));
});

// Site-scoped API endpoints (require site resolution)
var siteApi = app.MapGroup("")
    .AddEndpointFilter(async (context, next) =>
    {
        var http = context.HttpContext;
        var resolver = http.RequestServices.GetRequiredService<ISiteResolver>();
        var host = http.Request.Host.Value;

        if (string.IsNullOrEmpty(host))
            return Results.BadRequest("Missing host header");

        var siteContext = await resolver.ResolveAsync(host, http.RequestAborted);
        if (siteContext is null)
            return Results.NotFound($"No site configured for host: {host}");

        http.Items["SiteContext"] = siteContext;
        return await next(context);
    });

siteApi.MapLocationEndpoints();

app.Run();

// Make Program accessible for integration tests
public partial class Program { }
