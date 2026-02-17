using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using WPM.Domain.CMS.Data;
using WPM.Domain.CMS.Models;

namespace WPM.Domain.CMS.Endpoints;

public static class LocationEndpoints
{
    public static RouteGroupBuilder MapLocationEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/cms/locations");

        group.MapGet("/", async (HttpContext http) =>
        {
            using var db = GetCmsDb(http);
            var locations = await db.Locations
                .Where(l => l.IsActive)
                .OrderBy(l => l.SortOrder)
                .Select(l => new LocationListDto(l.Id, l.Title, l.Slug, l.ParentLocationId, l.SortOrder, l.ShowInNavigation))
                .ToListAsync(http.RequestAborted);
            return Results.Ok(locations);
        });

        group.MapGet("/{id:int}", async (int id, HttpContext http) =>
        {
            using var db = GetCmsDb(http);
            var location = await db.Locations
                .Include(l => l.Articles.Where(a => a.IsActive).OrderBy(a => a.SortOrder))
                .Include(l => l.Children.Where(c => c.IsActive).OrderBy(c => c.SortOrder))
                .FirstOrDefaultAsync(l => l.Id == id, http.RequestAborted);

            if (location is null)
                return Results.NotFound();

            return Results.Ok(new LocationDetailDto(
                location.Id,
                location.Title,
                location.Slug,
                location.Description,
                location.Keywords,
                location.Body,
                location.ParentLocationId,
                location.SortOrder,
                location.IsActive,
                location.ShowInNavigation,
                location.TemplateName,
                location.Articles.Select(a => new ArticleDto(a.Id, a.Title, a.Body, a.Summary, a.SortOrder)).ToList(),
                location.Children.Select(c => new LocationListDto(c.Id, c.Title, c.Slug, c.ParentLocationId, c.SortOrder, c.ShowInNavigation)).ToList()));
        });

        group.MapGet("/by-slug/{slug}", async (string slug, HttpContext http) =>
        {
            using var db = GetCmsDb(http);
            var location = await db.Locations
                .Include(l => l.Articles.Where(a => a.IsActive).OrderBy(a => a.SortOrder))
                .FirstOrDefaultAsync(l => l.Slug == slug && l.IsActive, http.RequestAborted);

            if (location is null)
                return Results.NotFound();

            return Results.Ok(new LocationDetailDto(
                location.Id,
                location.Title,
                location.Slug,
                location.Description,
                location.Keywords,
                location.Body,
                location.ParentLocationId,
                location.SortOrder,
                location.IsActive,
                location.ShowInNavigation,
                location.TemplateName,
                location.Articles.Select(a => new ArticleDto(a.Id, a.Title, a.Body, a.Summary, a.SortOrder)).ToList(),
                []));
        });

        group.MapPost("/", async (CreateLocationDto dto, HttpContext http) =>
        {
            using var db = GetCmsDb(http);
            var location = new Location
            {
                Title = dto.Title,
                Slug = dto.Slug,
                Description = dto.Description,
                Keywords = dto.Keywords,
                Body = dto.Body,
                ParentLocationId = dto.ParentLocationId,
                SortOrder = dto.SortOrder,
                IsActive = dto.IsActive,
                ShowInNavigation = dto.ShowInNavigation,
                TemplateName = dto.TemplateName
            };
            db.Locations.Add(location);
            await db.SaveChangesAsync(http.RequestAborted);
            return Results.Created($"/api/cms/locations/{location.Id}", new { location.Id });
        });

        group.MapPut("/{id:int}", async (int id, UpdateLocationDto dto, HttpContext http) =>
        {
            using var db = GetCmsDb(http);
            var location = await db.Locations.FindAsync([id], http.RequestAborted);
            if (location is null)
                return Results.NotFound();

            location.Title = dto.Title;
            location.Slug = dto.Slug;
            location.Description = dto.Description;
            location.Keywords = dto.Keywords;
            location.Body = dto.Body;
            location.ParentLocationId = dto.ParentLocationId;
            location.SortOrder = dto.SortOrder;
            location.IsActive = dto.IsActive;
            location.ShowInNavigation = dto.ShowInNavigation;
            location.TemplateName = dto.TemplateName;
            location.UpdatedAt = DateTime.UtcNow;

            await db.SaveChangesAsync(http.RequestAborted);
            return Results.NoContent();
        });

        group.MapDelete("/{id:int}", async (int id, HttpContext http) =>
        {
            using var db = GetCmsDb(http);
            var location = await db.Locations.FindAsync([id], http.RequestAborted);
            if (location is null)
                return Results.NotFound();

            db.Locations.Remove(location);
            await db.SaveChangesAsync(http.RequestAborted);
            return Results.NoContent();
        });

        return group;
    }

    private static CmsDbContext GetCmsDb(HttpContext http)
    {
        var siteContext = http.Items["SiteContext"] as WPM.Core.Models.SiteContext
            ?? throw new InvalidOperationException("SiteContext not available");
        return CmsDbContextFactory.Create(siteContext.DataFolder);
    }
}

// DTOs
public record LocationListDto(int Id, string Title, string Slug, int? ParentLocationId, int SortOrder, bool ShowInNavigation);
public record LocationDetailDto(int Id, string Title, string Slug, string? Description, string? Keywords, string? Body,
    int? ParentLocationId, int SortOrder, bool IsActive, bool ShowInNavigation, string? TemplateName,
    List<ArticleDto> Articles, List<LocationListDto> Children);
public record ArticleDto(int Id, string Title, string? Body, string? Summary, int SortOrder);
public record CreateLocationDto(string Title, string Slug, string? Description, string? Keywords, string? Body,
    int? ParentLocationId, int SortOrder, bool IsActive, bool ShowInNavigation, string? TemplateName);
public record UpdateLocationDto(string Title, string Slug, string? Description, string? Keywords, string? Body,
    int? ParentLocationId, int SortOrder, bool IsActive, bool ShowInNavigation, string? TemplateName);
