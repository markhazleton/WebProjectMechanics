<%@ Page explicit="true" %>
<script runat="server" type="text/VB">
    Public mySiteMap As New mhSiteMap(Session)
    Private Sub Page_Load(ByVal s As System.Object, ByVal e As System.EventArgs)
        mySiteMap.SetListPage(Request.ServerVariables.Item("QUERY_STRING"), Request.ServerVariables.Item("SERVER_NAME"), Request.ServerVariables.Item("URL"))
        Dim myArticle As New mhArticle(mySiteMap.mySession.CurrentArticleID(), mySiteMap.CurrentMapRow.PageID(), mySiteMap.mySiteFile.DefaultArticleID)
        lblArticleBody.Text = myArticle.ArticleBody
    End Sub
</script>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">
<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head>
  <title><%=mySiteMap.mySiteFile.CompanyName%></title>
  <meta name="description" content="<%=mySiteMap.CurrentMapRow.PageDescription%>" />
  <meta name="keywords" content="<%=mySiteMap.CurrentMapRow.PageKeywords%>" />
  <meta http-equiv="Content-Language" content="en" />
  <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
  <meta http-equiv="PRAGMA" content="NO-CACHE" />
</head>
<body>
<asp:literal id="lblArticleBody" Visible="TRUE" runat="server" />
</body>
</html>
