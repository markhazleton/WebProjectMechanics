'
' ASP.NET code-behind class (Master Page)
'
Partial Class MasterPage
    Inherits wpmMasterPage
    '
    ' Master page Page_Load event
    '
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
        If Not wpmUser.IsAdmin() Then
            Session("Login_Link") = Request.RawUrl
            Response.Redirect("/wpm/login/login.aspx")
        End If
        If TypeOf Me.Page Is AspNetMaker7_WPMGen Then
            ParentPage = CType(Me.Page, AspNetMaker7_WPMGen)
        End If
        sExport = Convert.ToString(Request.QueryString("export")) ' Load Export Request
        Dim EmptyOrPrint As Boolean = (sExport = "" OrElse sExport = "print")
        ProjectCss.Visible = EmptyOrPrint
        ClientScript.Visible = EmptyOrPrint
        StartupScript.Visible = EmptyOrPrint
        If sExport <> "" Then
            Links.Visible = False
            Header1.Visible = False
            Menu.Visible = False
            Header2.Visible = False
            Footer.Visible = False
        Else
            TopBanner.Text = wpmAdmin.BuildAdminHeaderLinks(ParentPage.pageActiveSite)
        End If
    End Sub
End Class
