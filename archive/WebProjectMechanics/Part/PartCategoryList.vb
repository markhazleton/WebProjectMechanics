
Public Class PartCategoryList
    Inherits List(Of PartCategory)
    Public Sub New()

    End Sub
    Public Sub New(ByVal capacity As Integer)
        MyBase.New(capacity)

    End Sub
    Public Sub New(ByVal collection As IEnumerable(Of PartCategory))
        MyBase.New(collection)

    End Sub

    Public Sub PopulateLinkCategoryList(ByVal CompanyID As String)
        Try
            For Each myrow As DataRow In ApplicationDAL.GetPartCategoryList().Rows
                Add(New PartCategory() With {.ID = wpm_GetDBString(myrow("ID")),
                                             .Name = wpm_GetDBString(myrow("Title")),
                                             .Description = wpm_GetDBString(myrow("Description")),
                                             .ParentID = wpm_GetDBString(myrow("ParentID")),
                                             .LocationID = wpm_GetDBString(myrow("PageID")),
                                             .PartCount = wpm_GetDBInteger(myrow("CountOfID"))})
            Next
        Catch ex As Exception
            ApplicationLogging.AuditLog("Error on mhLinkCategoryList.New(ByVal CompanyID As String)", ex.ToString)
        End Try
    End Sub

End Class
