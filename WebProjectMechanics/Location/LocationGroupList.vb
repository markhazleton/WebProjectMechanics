
Public Class LocationGroupList
    Inherits List(Of LocationGroup)
    Public Sub New()

    End Sub
    Public Sub New(ByVal capacity As Integer)
        MyBase.New(capacity)

    End Sub
    Public Sub New(ByVal collection As IEnumerable(Of LocationGroup))
        MyBase.New(collection)

    End Sub
    Public Function PopulateSiteGroupList(ByVal CompanyID As String) As Boolean
        Dim bReturn As Boolean = True
        Try
            For Each myrow As DataRow In ApplicationDAL.GetSiteGroupList().Rows
                Me.Add(New LocationGroup() With {.LocationGroupID = wpm_GetDBString(myrow("SiteCategoryGroupID")), .LocationGroupNM = wpm_GetDBString(myrow("SiteCategoryGroupNM")), .LocationGroupDS = wpm_GetDBString(myrow("SiteCategoryGroupDS")), .LocationGroupOrder = wpm_GetDBInteger(myrow("SiteCategoryGroupOrder"))})
            Next
        Catch ex As Exception
            bReturn = False
            ApplicationLogging.ErrorLog("ERROR ON LocationGroupList.PopulateSiteGroupList", ex.ToString)
        End Try
        Return bReturn
    End Function
End Class
