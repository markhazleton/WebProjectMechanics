

Public Interface IwpmSiteList
    Function AddSite(ByVal DomainName As String, ByVal CompanyID As String, ByVal SQLDBConnString As String) As Boolean
    Function GetSiteByDomain(ByVal inDomain As String) As wpmSite
End Interface
