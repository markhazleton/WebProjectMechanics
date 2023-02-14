Imports WebProjectMechanics
Imports wpmMineralCollection

Public Class MineralCollection_Default
    Inherits AdminPage

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        SetPageName("Mineral Collection")
        Response.Redirect("/MineralCollection/admin.aspx")
    End Sub
End Class
