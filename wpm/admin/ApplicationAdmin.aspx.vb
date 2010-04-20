
Partial Class wpm_admin_ApplicationAdmin
    Inherits AspNetMaker7_WPMGen

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            cbCachingEnabled.Checked = App.Config.CachingEnabled()
            cbUse404Processing.Checked = App.Config.Use404Processing()
            cbFullLoggingOn.Checked = App.Config.FullLoggingOn()
            cbRemoveWWW.Checked = App.Config.RemoveWWW()
            ShowDebug()

            Dim myString As New StringBuilder("<table cellspacing=""0"" rowhighlightclass=""ewTableHighlightRow"" rowselectclass=""ewTableSelectRow"" roweditclass=""ewTableEditRow"" class=""ewTable ewTableSeparate"">")
            myString.Append("<thead><tr class=""ewTableHeader""><th>Domain</th><th>CompanyID</th><th>Database</th></tr></thead><tbody>")
            For Each Site As wpmSite In App.SiteList
                myString.Append("<tr><td>" & Site.DomainName & "</td><td>" & Site.CompanyID & "</td><td>" & Site.SQLDBConnString & "</td></tr>")
            Next
            myString.Append("</tbody></table>")
            Me.SiteList.Text = myString.ToString()
        End If
    End Sub

    Protected Sub cbCachingEnabled_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbCachingEnabled.CheckedChanged
        App.Config.CachingEnabled = cbCachingEnabled.Checked
        ShowDebug()
    End Sub
    Protected Sub cbUse404Processing_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbUse404Processing.CheckedChanged
        App.Config.Use404Processing = cbUse404Processing.Checked
        ShowDebug()
    End Sub
    Protected Sub cbFullLoggingOn_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbFullLoggingOn.CheckedChanged
        App.Config.FullLoggingOn = cbFullLoggingOn.Checked
        ShowDebug()
    End Sub
    Private Function ShowDebug() As Boolean
        Dim mySessions As New wpmSession(Session)
        debug.Text = mySessions.GetSessionDebug
        Return True
    End Function
    Protected Sub cbRemoveWWW_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbRemoveWWW.CheckedChanged
        App.Config.RemoveWWW = cbRemoveWWW.Checked
        ShowDebug()
    End Sub
End Class
