Imports WebProjectMechanics

Partial Class admin_gallery_Default
    Inherits AdminPage

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Context.Session("GalleryLocationID") = 0
    End Sub
End Class
