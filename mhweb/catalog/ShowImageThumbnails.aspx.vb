Partial Class mhweb_catalog_ShowImageThumbnails
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim mySB As New StringBuilder
        For Each baseFile As String In My.Computer.FileSystem.GetFiles(Server.MapPath(Session("SiteGallery") & "image/"))
            If Right(baseFile, 3) = "jpg" Then
                mySB.Append("<img src='/mhweb/catalog/ImageResize.aspx?img=\" & Replace(Replace(baseFile, Server.MapPath("/"), ""), "/", "\") & "&h=100' />")
            End If
        Next
        myMain.Text = mySB.ToString
    End Sub
End Class
