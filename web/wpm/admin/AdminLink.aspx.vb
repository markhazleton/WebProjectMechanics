
Partial Class wpm_admin_AdminLink
    Inherits AspNetMaker7_WPMGen

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        myContent.Text = pageActiveSite.GetSitePartAdmin()
    End Sub
End Class
