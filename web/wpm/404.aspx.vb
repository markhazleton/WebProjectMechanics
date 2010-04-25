
Partial Class wpm_wpm404
    Inherits wpmPage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        pageActiveSite.Process404(Request.RawUrl(), Request.ServerVariables.Item("QUERY_STRING"))
        If App.Config.FullLoggingOn Then
            wpmUTIL.WriteLog(pageActiveSite.GetCurrentPageName, Request.ServerVariables.Item("QUERY_STRING").Replace("404;", ""), App.Config.ConfigFolderPath & "\log\404-log.csv")
        End If
    End Sub
End Class
