Imports WebProjectMechanics

Partial Class ProjectMechanicsMain
    Inherits ApplicationPage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        masterPage.myCompany.WriteCurrentLocation()
    End Sub
End Class
