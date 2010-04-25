
Partial Class wpm_admin_admin
    Inherits AspNetMaker7_WPMGen

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim myAdmin As New wpmAdmin(Session)
        myContent.Text = myAdmin.BuildAdmin()
    End Sub
End Class
