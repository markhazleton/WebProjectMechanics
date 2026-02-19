namespace WPM.Migration;

// Legacy record types — verbatim field mapping from Access .mdb schemas

record LegacySiteConfig(string XmlFileName, string Domain, string DbPath, int CompanyId);

record LegacyCompany(
    int CompanyId,
    string CompanyName,
    string? SiteUrl,
    string? GalleryFolder,
    string? SiteTemplate,
    string? DefaultSiteTemplate,
    string? FromEmail,
    int? HomePageId,
    bool ActiveFl,
    string? Address,
    string? City,
    string? StateOrProvince,
    string? PostalCode,
    string? PhoneNumber);

record LegacyPage(
    int PageId,
    int CompanyId,
    string PageName,
    string? PageTitle,
    string? PageDescription,
    string? PageKeywords,
    string? PageFileName,
    int? ParentPageId,
    int PageOrder,
    bool Active,
    DateTime? ModifiedDt);

record LegacyArticle(
    int ArticleId,
    int CompanyId,
    int? PageId,
    string Title,
    string? ArticleBody,
    string? ArticleSummary,
    bool Active,
    int SortOrder,
    DateTime? ModifiedDt);

record LegacyImage(
    int ImageId,
    int? CompanyId,
    string ImageFileName,
    string? ImageName,
    string? ImageDescription,
    bool Active,
    DateTime? ImageDate,
    DateTime? ModifiedDt);

record LegacyPageImage(
    int PageImageId,
    int? PageId,
    int? ImageId,
    int PageImagePosition);

record LegacyPageAlias(
    int PageAliasId,
    int? CompanyId,
    string? PageUrl,
    string? TargetUrl);
