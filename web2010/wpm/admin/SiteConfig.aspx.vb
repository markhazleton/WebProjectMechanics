Imports WebProjectMechanics

Partial Class SiteConfiguration_Default
    Inherits wpmPage
    Dim WithEvents mycon As New wpmSiteListControler(SiteWebUserControl1)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        mycon = New wpmSiteListControler(SiteWebUserControl1)
        If Not IsPostBack Then
            ddPets.Items.Clear()
            For Each i As String In wpmSiteListControler.GetSiteList
                ddPets.Items.Add(i)
            Next
            PetEdit.Visible = False
            DisplayErrorMessage(String.Empty)
        End If
    End Sub

    Protected Sub mycon_CancelEdit() Handles mycon.CancelEdit
        PetEdit.Visible = False
        DisplayErrorMessage(String.Empty)
    End Sub

    Protected Sub mycon_InvalidSite(ByVal errormessage As String) Handles mycon.InvalidSite
        DisplayErrorMessage(errormessage)
    End Sub

    Private Sub DisplayErrorMessage(ByVal ErrorMessage As String)
        Label1.Text = ErrorMessage
        If ErrorMessage <> String.Empty Then
            Label1.Visible = True
            Label1.BackColor = Drawing.Color.Pink
        Else
            Label1.Visible = False
            Label1.BackColor = Drawing.Color.White
        End If
    End Sub
    Protected Sub mycon_ListChange() Handles mycon.ListChange
        ddPets.Items.Clear()
        For Each i As String In wpmSiteListControler.GetSiteList
            ddPets.Items.Add(i)
        Next
        PetEdit.Visible = False
        DisplayErrorMessage(String.Empty)
    End Sub

    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEdit.Click
        If ddPets.SelectedItem Is Nothing Then
            mycon.ShowASite(String.Empty)
        Else
            mycon.ShowASite(ddPets.SelectedItem.Value)
        End If
        PetEdit.Visible = True
        DisplayErrorMessage(String.Empty)
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not ddPets.SelectedItem Is Nothing Then
            mycon.RemoveASite(ddPets.SelectedItem.Value)
        End If
    End Sub

End Class
