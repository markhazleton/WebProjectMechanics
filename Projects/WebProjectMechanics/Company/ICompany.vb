Public Interface ICompany
    Property CompanyID() As String
    Property DomainName() As String
    Property CompanyTitle() As String
    Property CompanyKeywords() As String
    Property CompanyDescription() As String
    Property HomeLocationID() As String
    Property DefaultArticleID() As String
    Property SitePrefix() As String
    Property DefaultSitePrefix() As String
    Property SiteGallery() As String
    Property SiteConfig() As String
    Property SiteState() As String
    Property SiteCountry() As String
    Property SiteCity() As String
    Property SiteCategoryTypeID() As String
    Property DefaultSiteCategoryID() As String
    Property UseBreadCrumbURL() As Boolean
    Property FromEmail() As String
    Property SMTP() As String
    Property Component() As String
End Interface

Public Interface ISiteType
    Property SiteTypeID As String
    Property SiteTypeNM As String
    Property SiteTypeDS As String
    Property ModifiedID As Integer
    Property ModifiedDT As DateTime
    Property DefaultSiteTypeLocationID As String
    Property DefaultSiteTypeLocationNM As String
End Interface

Public Class SiteType
    Implements ISiteType
    Public Property SiteTypeID As String Implements ISiteType.SiteTypeID
    Public Property SiteTypeNM As String Implements ISiteType.SiteTypeNM
    Public Property SiteTypeDS As String Implements ISiteType.SiteTypeDS
    Public Property DefaultSiteTypeLocationID As String Implements ISiteType.DefaultSiteTypeLocationID
    Public Property DefaultSiteTypeLocationNM As String Implements ISiteType.DefaultSiteTypeLocationNM
    Public Property ModifiedDT As Date Implements ISiteType.ModifiedDT
    Public Property ModifiedID As Integer Implements ISiteType.ModifiedID
    Public Property SiteTypeLocationCount As Integer
End Class

Public Class SiteTypeList
    Inherits List(Of SiteType)
End Class