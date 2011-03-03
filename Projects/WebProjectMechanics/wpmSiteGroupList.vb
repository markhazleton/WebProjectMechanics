
Public Class wpmSiteGroupList
    Inherits List(Of wpmSiteGroup)
    Public Function PopulateSiteGroupList(ByVal CompanyID As String) As Boolean
        Dim bReturn As Boolean = True
        Try
            For Each myrow As DataRow In wpmDataCon.GetSiteGroupList().Rows
                Me.Add(New wpmSiteGroup() With {.SiteCategoryGroupID = wpmUtil.GetDBString(myrow("SiteCategoryGroupID")), .SiteCategoryGroupNM = wpmUtil.GetDBString(myrow("SiteCategoryGroupNM")), .SiteCategoryGroupDS = wpmUtil.GetDBString(myrow("SiteCategoryGroupDS")), .SiteCategoryGroupOrder = wpmUtil.GetDBInteger(myrow("SiteCategoryGroupOrder"))})
            Next
        Catch ex As Exception
            bReturn = False
            wpmLogging.AuditLog("ERROR ON wpmSiteGroupList.PopulateSiteGroupList", ex.ToString)
        End Try
        Return bReturn
    End Function
End Class
