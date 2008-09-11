Partial Class mhmaster
    Inherits System.Web.UI.MasterPage
    Public sTemp As New String("<h1>Default from mhmaster.vb</h1>~~CurrentPageName~~<br/>~~MainContent~~<br/>")
    Public isExport As Boolean = False
    ' *************************
    ' *  Handler for Export
    ' *************************
    Public Sub SetExport(ByVal value As Boolean)
        isExport = value
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
        '        Dim isLoginLinkVisible As Boolean = ((Request.ServerVariables("URL").Substring((Request.ServerVariables("URL").Length - "login.aspx".Length)) <> "login.aspx"))
        Dim isLoginLinkVisible As Boolean = False
    End Sub
    ' *********************************
    ' *  Handler for LoginStatus LoggingOut
    ' *********************************
    Protected Sub mhweb1LoginStatus_LoggingOut(ByVal sender As Object, ByVal e As LoginCancelEventArgs)
      Response.Redirect("aspmaker/logout.aspx")
    End Sub

End Class

