Imports WebProjectMechanics

Partial Class minerals
    Inherits AdminPage

    Private Function GetMyMineral(ByVal mycon As wpmMineralCollection.DataController) As wpmMineralCollection.Mineral
        Return (From i In mycon.Minerals Where i.MineralID = ListBox1.SelectedValue).Single()
    End Function
    Protected Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        Using mycon As New wpmMineralCollection.DataController()
            litImage.Text = wpmMineralCollection.Display.DisplayCollectionItems(GetMyMineral(mycon))
        End Using
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        SetPageName("Minerals")
    End Sub
End Class
