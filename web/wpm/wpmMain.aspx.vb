
Partial Class wpm_wpmMain
    Inherits wpmPage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        pageActiveSite.WriteCurrentLocation()
    End Sub
End Class
