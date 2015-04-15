Imports WebProjectMechanics

Partial Class Admin2
    Inherits ApplicationMasterPage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not wpm_IsAdmin And Not Page.IsCallback Then
            wpm_ListPageURL = Request.Url.AbsoluteUri
            Response.Redirect(String.Format("{0}login/login.aspx", wpm_SiteConfig.AdminFolder))
        End If
        litSiteName.Text = wpm_CurrentSiteName
    End Sub
End Class

