Public Class AdminPage
    Inherits ApplicationPage
    Public Const STR_ImageFolder As String = "images/"

    Private Sub Page_Init1(sender As Object, e As EventArgs) Handles Me.Init
        CheckAdmin()
    End Sub

    Private _myControl As New ApplicationUserControl()
    Public Property myControl As ApplicationUserControl
        Get
            Return _myControl
        End Get
        Set(ByVal value As ApplicationUserControl)
            _myControl = value
        End Set
    End Property
    Public Sub RedirectToListPage(sender As Object)
        Response.Redirect(wpm_ListPageURL)
    End Sub
    Protected Sub SetListPageURL(ByVal KeyField As String, ByVal KeyType As String)
        If wpm_GetProperty(KeyField, String.Empty) = String.Empty Then
            wpm_ListPageURL = String.Format("{0}?Type={1}", wpm_CurrentPage(), KeyType)
        End If
    End Sub
    Public Function LoadMenu() As List(Of LookupItem)
        Dim myListItems As New List(Of LookupItem)
        myListItems.Add(New LookupItem With {.Value = "/admin/maint/default.aspx?type=Location", .Name = "Location"})
        myListItems.Add(New LookupItem With {.Value = "/admin/maint/default.aspx?type=Company", .Name = "Company"})
        myListItems.Add(New LookupItem With {.Value = "/admin/maint/default.aspx?type=SiteType", .Name = "Site Type"})
        myListItems.Add(New LookupItem With {.Value = "/admin/maint/default.aspx?type=Contact", .Name = "Contact"})
        myListItems.Add(New LookupItem With {.Value = "/admin/maint/default.aspx?type=SiteTemplate", .Name = "SiteTemplate"})
        myListItems.Add(New LookupItem With {.Value = "/admin/maint/default.aspx?type=Image", .Name = "Image"})
        myListItems.Add(New LookupItem With {.Value = "/admin/maint/default.aspx?type=Article", .Name = "Article"})
        myListItems.Add(New LookupItem With {.Value = "/admin/maint/default.aspx?type=Blog", .Name = "Blog"})
        myListItems.Add(New LookupItem With {.Value = "/admin/maint/default.aspx?type=Part", .Name = "Part"})
        myListItems.Add(New LookupItem With {.Value = "/admin/maint/default.aspx?type=Parameter", .Name = "Parameter"})
        myListItems.Add(New LookupItem With {.Value = "/admin/maint/default.aspx?type=ParameterType", .Name = "ParameterType"})
        myListItems.Add(New LookupItem With {.Value = "/admin/maint/default.aspx?type=LocationType", .Name = "LocationType"})
        myListItems.Add(New LookupItem With {.Value = "/admin/maint/default.aspx?type=PageAlias", .Name = "PageAlias"})
        Return myListItems
    End Function
End Class
