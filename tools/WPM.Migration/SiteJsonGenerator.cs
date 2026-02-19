using System.Text.Json;
using System.Text.Json.Serialization;

namespace WPM.Migration;

/// <summary>
/// Generates site.json from legacy Company data + XML config.
/// </summary>
static class SiteJsonGenerator
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public static void Generate(
        string siteDataFolder,
        LegacyCompany company,
        LegacySiteConfig config,
        string? homePageSlug)
    {
        var siteJson = new
        {
            siteName = company.CompanyName,
            domain = config.Domain.ToLowerInvariant(),
            homePageSlug,
            contactEmail = company.FromEmail,
            galleryFolder = company.GalleryFolder,
            themeName = company.SiteTemplate ?? company.DefaultSiteTemplate,
            contact = new
            {
                address = company.Address,
                city = company.City,
                state = company.StateOrProvince,
                postalCode = company.PostalCode,
                phone = company.PhoneNumber
            },
            publishing = new
            {
                generateRss = true,
                generateSitemap = true
            },
            migration = new
            {
                legacyCompanyId = company.CompanyId,
                legacyMdbFile = Path.GetFileName(config.DbPath),
                migratedAt = DateTime.UtcNow.ToString("o")
            }
        };

        var json = JsonSerializer.Serialize(siteJson, JsonOptions);
        File.WriteAllText(Path.Combine(siteDataFolder, "site.json"), json);
    }
}
