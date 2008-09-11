
Partial Class mhweb_admin_admin
    Inherits mhPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim myAdmin As New mhAdmin(Session)
        myContent.Text = myAdmin.BuildAdmin()
    End Sub
End Class
