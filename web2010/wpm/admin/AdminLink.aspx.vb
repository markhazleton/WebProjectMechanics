Imports WebProjectMechanics

Partial Class wpm_admin_AdminLink
    Inherits wpmPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        myContent.Text = pageActiveSite.GetSitePartAdmin()
    End Sub
End Class
