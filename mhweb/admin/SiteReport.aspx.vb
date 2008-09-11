
Partial Class mhweb_admin_SiteReport
    Inherits mhPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim myAdmin As New mhAdmin(Session)
        myContent.Text = myAdmin.SiteImageReport()
    End Sub
End Class
