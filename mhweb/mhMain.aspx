<%@ Page Language="VB"    %>
<script runat="server" type="text/VB" >
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim mySiteMap As New mhSiteMap(Session)
        Dim sArticleID As String = ""
        Dim sPageID As String = ""
        Dim myArticle As mhArticle
        Try
            If Not IsNothing(HttpContext.Current.Request.QueryString.GetValues("c")) Then
                sPageID = HttpContext.Current.Request.QueryString.Item("c")
            End If
            If Not IsNothing(HttpContext.Current.Request.QueryString.Item("a")) Then
                sArticleID = HttpContext.Current.Request.QueryString.Item("a")
            End If
            myArticle = New mhArticle(sArticleID, sPageID, mySiteMap.mySiteFile.DefaultArticleID)
            Response.Write(mySiteMap.GetHTML(myArticle.ArticleBody, False, mySiteMap.mySession.SiteTemplatePrefix))
        Catch ex As Exception
            mhUTIL.AuditLog(ex.ToString, "mhMain.Page_Load()")
        End Try
    End Sub
</script>
