Imports WebProjectMechanics

Partial Class wpmAdminMaster
    Inherits wpmMasterPage

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim mySession As New wpmSession(Session)
        If Not wpmUser.IsAdmin() Then
            Response.Redirect("/")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim myAdmin As New wpmAdmin(Session)
        litAdminHeader.Text = myAdmin.BuildAdminHeaderLinks()
        litAdminCommon.Text = myAdmin.BuildAdminHeader()
        ' litAdminFileMenu.Text = GetFileMenu()
    End Sub
End Class

