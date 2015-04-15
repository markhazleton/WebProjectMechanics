Imports WebProjectMechanics

Partial Class ProjectMechanicsModule
    Inherits ApplicationPage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        masterPage.myCompany.WriteCurrentLocation()
    End Sub

End Class


