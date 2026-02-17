Imports WebProjectMechanics

Public Class wpm_Admin_AdminLink
    Inherits ApplicationPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        litMain.Text = masterPage.myCompany.GetSitePartAdmin()
    End Sub
End Class
