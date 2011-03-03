Public Interface IwpmLocation
    Property PageTypeCD() As String
    Property DisplayURL() As String
    Function UpdatePageRow(ByRef FoundPageRow As wpmLocation) As Boolean
    Function BuildClassLink(ByVal LinkClass As String, ByRef UseBreadcrumbURL As Boolean) As String
    Function GetSiteMapRowURL(ByRef UseBreadCrumbURL As Boolean) As String
End Interface
