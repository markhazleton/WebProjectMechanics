Imports WebProjectMechanics

Partial Class Admin_ApplicationAdmin
    Inherits ApplicationPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim mysb As StringBuilder = New StringBuilder("")
        If Not IsPostBack Then
            SetCheckBoxes()
            mysb.Append(String.Format("<p><strong>ConnStr:</strong><pre>{0}</pre></p><p><strong>Config Path:</strong> {1}</p>", wpm_SQLDBConnString, wpm_SiteConfig.ConfigFolder))
            mysb.Append(String.Format("<br><strong>Caching Enabled:</strong>{0}<br/>", wpm_SiteConfig.CachingEnabled))
            mysb.Append(String.Format("<br><strong>Use 404 Processing Enabled:</strong>{0}<br/>", wpm_SiteConfig.Use404Processing))
            SiteInformation.Text = mysb.ToString
        End If
    End Sub

    Private Sub SetCheckBoxes()
        cbCachingEnabled.Checked = wpm_SiteConfig.CachingEnabled()
        cbUse404Processing.Checked = wpm_SiteConfig.Use404Processing
        cbFullLoggingOn.Checked = wpm_SiteConfig.FullLoggingOn()
        cbRemoveWWW.Checked = wpm_SiteConfig.RemoveWWW()
    End Sub
    Protected Sub cbCachingEnabled_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbCachingEnabled.CheckedChanged
        wpm_SiteConfig.CachingEnabled = cbCachingEnabled.Checked
    End Sub
    Protected Sub cbUse404Processing_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbUse404Processing.CheckedChanged
        wpm_SiteConfig.Use404Processing = cbUse404Processing.Checked
    End Sub
    Protected Sub cbFullLoggingOn_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbFullLoggingOn.CheckedChanged
        wpm_SiteConfig.FullLoggingOn = cbFullLoggingOn.Checked
    End Sub
    Protected Sub cbRemoveWWW_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbRemoveWWW.CheckedChanged
        wpm_SiteConfig.RemoveWWW = cbRemoveWWW.Checked
    End Sub

    Protected Sub lbResetCache_Click(sender As Object, e As EventArgs)
        For Each myFolder In GetFolders("/app_data/sites/")
            RemoveFolder(myFolder)
        Next

        Dim sItems As New List(Of String)
        For Each item In HttpContext.Current.Application.Contents
            If TryCast(HttpContext.Current.Application(item.ToString), Company) Is Nothing Then
                ' do nothing
            Else
                sItems.Add(item.ToString)
            End If
        Next
        For Each item In sItems
            HttpContext.Current.Application.Contents(item) = Nothing
        Next
    End Sub
End Class
