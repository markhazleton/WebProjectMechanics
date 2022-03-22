Imports WebProjectMechanics

Partial Class RuntimeMaster
    Inherits ApplicationMasterPage
    Public sTemp As New String("<h1>Default from RuntimeMaster.master</h1>~~CurrentPageName~~<br/>~~MainContent~~<br/>")
    Public isExport As Boolean = False

    Protected Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Dim mySiteTheme As New SiteTemplate(myCompany, False)
        If mySiteTheme.sbSiteTemplate.Length > 0 Then
            myCompany.BuildTemplate(mySiteTheme.sbSiteTemplate)
            mySiteTheme.sbSiteTemplate.Replace("</head>", wpm_AddHTMLHead & "</head>")
            sTemp = mySiteTheme.sbSiteTemplate.ToString
            wpm_AddHTMLHead = "RESET"
        End If
        litTop.Text = sTemp.Substring(0, sTemp.IndexOf("~~MainContent~~"))
        litBottom.Text = sTemp.Substring(sTemp.IndexOf("~~MainContent~~") + 15, sTemp.Length - (sTemp.IndexOf("~~MainContent~~") + 15))
    End Sub
End Class

