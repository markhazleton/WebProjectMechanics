Imports WebProjectMechanics

Partial Class wpm_admin_ApplicationAdmin
    Inherits wpmPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            cbCachingEnabled.Checked = wpmApp.Config.CachingEnabled()
            cbUse404Processing.Checked = wpmApp.Config.Use404Processing()
            cbFullLoggingOn.Checked = wpmApp.Config.FullLoggingOn()
            cbRemoveWWW.Checked = wpmApp.Config.RemoveWWW()
            ShowDebug()

            Dim myString As New StringBuilder("<table cellspacing=""0"" rowhighlightclass=""ewTableHighlightRow"" rowselectclass=""ewTableSelectRow"" roweditclass=""ewTableEditRow"" class=""ewTable ewTableSeparate"">")
            myString.Append("<thead><tr class=""ewTableHeader""><th>Domain</th><th>CompanyID</th><th>Database</th><th>Path</th></tr></thead><tbody>")
            For Each Site As wpmSite In wpmApp.SiteList.GetSiteList
                myString.Append(String.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>", Site.DomainName, Site.CompanyID, Site.SQLDBConnString, Site.AccessDatabasePath))
            Next
            myString.Append("</tbody></table><br/>")
            myString.Append(String.Format("<strong>ConnStr:</strong> {0}<br/>", wpmApp.ConnStr))
            myString.Append(String.Format("<strong>Config Path:</strong> {0}<br/>", wpmApp.wpmConfigFile))

            Me.SiteList.Text = myString.ToString()
        End If
    End Sub

    Protected Sub cbCachingEnabled_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbCachingEnabled.CheckedChanged
        wpmApp.Config.CachingEnabled = cbCachingEnabled.Checked
        ShowDebug()
    End Sub
    Protected Sub cbUse404Processing_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbUse404Processing.CheckedChanged
        wpmApp.Config.Use404Processing = cbUse404Processing.Checked
        ShowDebug()
    End Sub
    Protected Sub cbFullLoggingOn_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbFullLoggingOn.CheckedChanged
        wpmApp.Config.FullLoggingOn = cbFullLoggingOn.Checked
        ShowDebug()
    End Sub
    Private Function ShowDebug() As Boolean
        Dim mySessions As New wpmSession(Session)
        debug.Text = mySessions.GetSessionDebug
        Return True
    End Function
    Protected Sub cbRemoveWWW_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbRemoveWWW.CheckedChanged
        wpmApp.Config.RemoveWWW = cbRemoveWWW.Checked
        ShowDebug()
    End Sub
End Class
