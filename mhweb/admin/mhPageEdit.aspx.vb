
Partial Class mhweb_admin_mhPageEdit
    Inherits mhPage

    Protected Sub FormView1_DataBound(ByVal sender As Object, ByVal e As System.EventArgs)
        If FormView1.CurrentMode = FormViewMode.Edit Then
            Dim dv As System.Data.DataRowView = FormView1.DataItem
        End If
    End Sub

    Protected Sub FormView1_ItemUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.FormViewUpdateEventArgs)
    End Sub
End Class
