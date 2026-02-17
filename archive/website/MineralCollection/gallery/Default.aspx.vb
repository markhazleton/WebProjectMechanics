
Partial Class MineralCollection_gallery_Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        context.Session("CollectionItemID")=0
    End Sub
End Class
