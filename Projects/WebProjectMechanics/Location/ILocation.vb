
Public Interface ILocation
    Property RecordSource As String
    Property LocationID() As String
    Property ArticleID As Integer
    Property LocationTypeCD As String
    Property LocationTypeID As Integer
    Property GroupID As Integer
    Property LocationName() As String
    Property LocationTitle() As String
    Property LocationDescription() As String
    Property LocationKeywords() As String
    Property LocationFileName() As String
    Property LocationBody As String
    Property LocationSummary As String
    Property IncludeInNavigation As Boolean
    Property ParentLocationID() As String
    Property DefaultOrder() As Integer
    Property SiteCategoryID() As String
    Property SiteCategoryName() As String
    Property LocationGroupID() As String
    Property LocationGroupNM() As String
    Property SiteTypeID As String
    Property ActiveFL() As Boolean
    Property RowsPerPage As Integer
    Property ImagesPerRow As Integer

    Property LevelNBR() As Integer
    Property TransferURL() As String
    Property BreadCrumbURL() As String
    Property BreadCrumbHTML() As String
    Property MainMenuURL() As String
    Property MainMenuLocationID() As String
    Property MainMenuLocationName() As String
    Property LocationTrailList() As List(Of LocationTrail)
    Property ModifiedDT As DateTime
    ReadOnly Property ParentCategoryID As String
    ReadOnly Property ParentPageID As String
    ReadOnly Property GetSiteCategoryID As String
    ReadOnly Property DisplayInMenu() As Boolean
    ReadOnly Property TransferParms As String
    ReadOnly Property LocationURL As String
End Interface
