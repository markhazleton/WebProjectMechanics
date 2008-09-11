
Partial Class mhweb_admin_AdminLink
    Inherits mhPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim mySiteMap As New mhSiteMap(Session)
        myContent.Text = mySiteMap.GetLinkAdmin()
    End Sub
End Class
