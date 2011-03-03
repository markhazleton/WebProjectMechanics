
Public Class wpmPartGroupList
    Inherits List(Of wpmPartGroup)
    Public Sub New()

    End Sub
    Public Sub New(ByVal capacity As Integer)
        MyBase.New(capacity)

    End Sub
    Public Sub New(ByVal collection As IEnumerable(Of wpmPartGroup))
        MyBase.New(collection)

    End Sub

    Public Sub PopulateLinkCategoryList(ByVal CompanyID As String)
        Try
            For Each myrow As DataRow In wpmDataCon.GetLinkCategoryList().Rows
                Add(New wpmPartGroup() With {.ID = wpmUtil.GetDBString(myrow("ID")),
                                             .Name = wpmUtil.GetDBString(myrow("Title")),
                                             .Description = wpmUtil.GetDBString(myrow("Description")),
                                             .ParentID = wpmUtil.GetDBString(myrow("ParentID")),
                                             .PageID = wpmUtil.GetDBString(myrow("PageID")),
                                             .LinkCount = wpmUtil.GetDBInteger(myrow("CountOfID"))})
            Next
        Catch ex As Exception
            wpmLogging.AuditLog("Error on mhLinkCategoryList.New(ByVal CompanyID As String)", ex.ToString)
        End Try
    End Sub

End Class
