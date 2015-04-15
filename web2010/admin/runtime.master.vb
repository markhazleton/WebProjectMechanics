Imports WebProjectMechanics

Partial Class RuntimeMaster
    Inherits ApplicationMasterPage
    Public sTemp As String = "<h1>Default from RuntimeMaster.master</h1>~~CurrentPageName~~<br/>~~MainContent~~<br/>"
    Public isExport As Boolean = False

    ' *************************
    ' *  Handler for Export
    ' *************************
    Public Sub SetExport(ByVal value As Boolean)
        isExport = value
    End Sub

    ' *********************************
    ' *  Handler for LoginStatus LoggingOut
    ' *********************************
    Protected Sub mhweb1LoginStatus_LoggingOut(ByVal sender As Object, ByVal e As LoginCancelEventArgs)
        Response.Redirect("login/logout.aspx")
    End Sub

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

