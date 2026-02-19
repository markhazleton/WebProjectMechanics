using System.Data.OleDb;

namespace WPM.Migration;

/// <summary>
/// Reads legacy data from an Access .mdb file via OleDb.
/// </summary>
sealed class MdbReader : IDisposable
{
    private readonly OleDbConnection _conn;

    public MdbReader(string mdbPath)
    {
        _conn = new OleDbConnection($"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={mdbPath};");
        _conn.Open();
    }

    public LegacyCompany? ReadCompany(int companyId)
    {
        using var cmd = new OleDbCommand(
            "SELECT CompanyID, CompanyName, SiteURL, GalleryFolder, SiteTemplate, DefaultSiteTemplate, " +
            "FromEmail, HomePageID, ActiveFL, Address, City, StateOrProvince, PostalCode, PhoneNumber " +
            "FROM [Company] WHERE CompanyID = @cid", _conn);
        cmd.Parameters.AddWithValue("@cid", companyId);
        using var r = cmd.ExecuteReader();
        if (!r.Read()) return null;

        return new LegacyCompany(
            CompanyId: GetInt(r, "CompanyID"),
            CompanyName: GetString(r, "CompanyName") ?? $"Company {companyId}",
            SiteUrl: GetString(r, "SiteURL"),
            GalleryFolder: GetString(r, "GalleryFolder"),
            SiteTemplate: GetString(r, "SiteTemplate"),
            DefaultSiteTemplate: GetString(r, "DefaultSiteTemplate"),
            FromEmail: GetString(r, "FromEmail"),
            HomePageId: GetNullableInt(r, "HomePageID"),
            ActiveFl: GetBool(r, "ActiveFL"),
            Address: GetString(r, "Address"),
            City: GetString(r, "City"),
            StateOrProvince: GetString(r, "StateOrProvince"),
            PostalCode: GetString(r, "PostalCode"),
            PhoneNumber: GetString(r, "PhoneNumber"));
    }

    public List<LegacyPage> ReadPages(int companyId)
    {
        var pages = new List<LegacyPage>();
        using var cmd = new OleDbCommand(
            "SELECT PageID, CompanyID, PageName, PageTitle, PageDescription, PageKeywords, " +
            "PageFileName, ParentPageID, PageOrder, Active, ModifiedDT " +
            "FROM [Page] WHERE CompanyID = @cid ORDER BY PageOrder", _conn);
        cmd.Parameters.AddWithValue("@cid", companyId);
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            pages.Add(new LegacyPage(
                PageId: GetInt(r, "PageID"),
                CompanyId: GetInt(r, "CompanyID"),
                PageName: GetString(r, "PageName") ?? "",
                PageTitle: GetString(r, "PageTitle"),
                PageDescription: GetString(r, "PageDescription"),
                PageKeywords: GetString(r, "PageKeywords"),
                PageFileName: GetString(r, "PageFileName"),
                ParentPageId: GetNullableInt(r, "ParentPageID"),
                PageOrder: GetNullableInt(r, "PageOrder") ?? 0,
                Active: GetBool(r, "Active"),
                ModifiedDt: GetNullableDateTime(r, "ModifiedDT")));
        }
        return pages;
    }

    public List<LegacyArticle> ReadArticles(int companyId)
    {
        var articles = new List<LegacyArticle>();
        using var cmd = new OleDbCommand(
            "SELECT ArticleID, CompanyID, PageID, Title, ArticleBody, ArticleSummary, " +
            "Active, ModifiedDT " +
            "FROM [Article] WHERE CompanyID = @cid ORDER BY PageID, ArticleID", _conn);
        cmd.Parameters.AddWithValue("@cid", companyId);
        using var r = cmd.ExecuteReader();
        var sortIndex = 0;
        int? lastPageId = null;
        while (r.Read())
        {
            var pageId = GetNullableInt(r, "PageID");
            if (pageId != lastPageId) { sortIndex = 0; lastPageId = pageId; }
            articles.Add(new LegacyArticle(
                ArticleId: GetInt(r, "ArticleID"),
                CompanyId: GetInt(r, "CompanyID"),
                PageId: pageId,
                Title: GetString(r, "Title") ?? "Untitled",
                ArticleBody: GetString(r, "ArticleBody"),
                ArticleSummary: GetString(r, "ArticleSummary"),
                Active: GetBool(r, "Active"),
                SortOrder: sortIndex++,
                ModifiedDt: GetNullableDateTime(r, "ModifiedDT")));
        }
        return articles;
    }

    public List<LegacyImage> ReadImages(int companyId)
    {
        var images = new List<LegacyImage>();
        using var cmd = new OleDbCommand(
            "SELECT ImageID, CompanyID, ImageFileName, ImageName, ImageDescription, " +
            "Active, ImageDate, ModifiedDT " +
            "FROM [Image] WHERE CompanyID = @cid ORDER BY ImageID", _conn);
        cmd.Parameters.AddWithValue("@cid", companyId);
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            images.Add(new LegacyImage(
                ImageId: GetInt(r, "ImageID"),
                CompanyId: GetNullableInt(r, "CompanyID"),
                ImageFileName: GetString(r, "ImageFileName") ?? "",
                ImageName: GetString(r, "ImageName"),
                ImageDescription: GetString(r, "ImageDescription"),
                Active: GetBool(r, "Active"),
                ImageDate: GetNullableDateTime(r, "ImageDate"),
                ModifiedDt: GetNullableDateTime(r, "ModifiedDT")));
        }
        return images;
    }

    public List<LegacyPageImage> ReadPageImages(IReadOnlySet<int> pageIds)
    {
        if (pageIds.Count == 0) return [];
        var results = new List<LegacyPageImage>();
        // OleDb doesn't support large IN lists well, so query all and filter in memory
        using var cmd = new OleDbCommand(
            "SELECT PageImageID, PageID, ImageID, PageImagePosition FROM [PageImage] ORDER BY PageID, PageImagePosition", _conn);
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            var pageId = GetNullableInt(r, "PageID");
            if (pageId.HasValue && pageIds.Contains(pageId.Value))
            {
                results.Add(new LegacyPageImage(
                    PageImageId: GetInt(r, "PageImageID"),
                    PageId: pageId,
                    ImageId: GetNullableInt(r, "ImageID"),
                    PageImagePosition: GetNullableInt(r, "PageImagePosition") ?? 0));
            }
        }
        return results;
    }

    public List<LegacyPageAlias> ReadPageAliases(int companyId)
    {
        var aliases = new List<LegacyPageAlias>();
        using var cmd = new OleDbCommand(
            "SELECT PageAliasID, CompanyID, PageURL, TargetURL FROM [PageAlias] WHERE CompanyID = @cid", _conn);
        cmd.Parameters.AddWithValue("@cid", companyId);
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            aliases.Add(new LegacyPageAlias(
                PageAliasId: GetInt(r, "PageAliasID"),
                CompanyId: GetNullableInt(r, "CompanyID"),
                PageUrl: GetString(r, "PageURL"),
                TargetUrl: GetString(r, "TargetURL")));
        }
        return aliases;
    }

    public void Dispose() => _conn.Dispose();

    // Helper methods for safe reading from OleDbDataReader
    private static int GetInt(OleDbDataReader r, string col)
    {
        var ordinal = r.GetOrdinal(col);
        return r.IsDBNull(ordinal) ? 0 : Convert.ToInt32(r.GetValue(ordinal));
    }

    private static int? GetNullableInt(OleDbDataReader r, string col)
    {
        var ordinal = r.GetOrdinal(col);
        return r.IsDBNull(ordinal) ? null : Convert.ToInt32(r.GetValue(ordinal));
    }

    private static string? GetString(OleDbDataReader r, string col)
    {
        var ordinal = r.GetOrdinal(col);
        return r.IsDBNull(ordinal) ? null : r.GetValue(ordinal)?.ToString()?.Trim();
    }

    private static bool GetBool(OleDbDataReader r, string col)
    {
        var ordinal = r.GetOrdinal(col);
        if (r.IsDBNull(ordinal)) return false;
        var val = r.GetValue(ordinal);
        return val is bool b ? b : Convert.ToInt32(val) != 0;
    }

    private static DateTime? GetNullableDateTime(OleDbDataReader r, string col)
    {
        var ordinal = r.GetOrdinal(col);
        return r.IsDBNull(ordinal) ? null : Convert.ToDateTime(r.GetValue(ordinal));
    }
}
