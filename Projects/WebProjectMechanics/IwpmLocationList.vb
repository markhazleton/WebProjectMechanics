Public Interface IwpmLocationList
    Property KeywordList() As wpmKeywordList
    Function GetLocationListFromDb(ByVal OrderBy As String, ByVal CompanyID As String, ByVal GroupID As String) As Boolean
    Function BuildBreadcrumbRows(ByVal CompanyName As String) As Boolean
    Function DefaultSort() As Boolean
    Function DefaultSort(ByRef Direction As wpmSortDirection) As Boolean
    Function GetCatalogLocations() As List(Of wpmLocation)
    Function FindLocationByKeyword(ByVal inSearchKeyword As String) As wpmLocation
    Function GetLocationURLByKeyword(ByVal inSearchKeyword As String) As String
    Function FindLocationsByKeyword(ByVal inSearchKeyword As String) As String
    Function FindLocation(ByVal PageID As String, ByVal ArticleID As String) As wpmLocation
    Function FindLocation(ByVal PageURL As String) As wpmLocation
End Interface
