Imports WebProjectMechanics

Partial Class wpm_wpm404
    Inherits wpmPage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        pageActiveSite.Process404(Request.RawUrl(), Request.ServerVariables.Item("QUERY_STRING"))
        If wpmApp.Config.FullLoggingOn Then
            wpmLogging.FileNotFoundLog(pageActiveSite.GetCurrentPageName, Request.ServerVariables.Item("QUERY_STRING").Replace("404;", ""))
        End If
    End Sub
End Class
