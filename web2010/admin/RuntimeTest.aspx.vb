Imports WebProjectMechanics

Partial Class admin_RuntimeTest
    Inherits ApplicationPage

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        masterPage.myCompany.CurLocation.LocationTitle = "My New Title"

    End Sub
End Class
