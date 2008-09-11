<%@ Page Language="vb"  %>
<script language="VB" runat="Server">
     '*********************
     '  Page Load Handler
     '*********************
    Private Sub Page_Load(ByVal s As System.Object, ByVal e As System.EventArgs)  
        '
        ' Reset Session Varibles
        '              
        Dim my404 As New mhSiteMap(Session)
        If my404.Process404(Request.RawUrl(), Request.ServerVariables.Item("QUERY_STRING")) Then
            my404.mySession.CurrentPageID = my404.CurrentMapRow.PageID
            my404.mySession.CurrentArticleID = my404.CurrentMapRow.ArticleID
            Select Case UCase(my404.CurrentMapRow.PageTypeCD)
                Case "PAGE"
                    Response.Write(my404.GetArticlePageHTML())
                Case "SITECAT"
                    Response.Write(my404.GetArticlePageHTML())
                Case "MAIN"
                    Response.Write(my404.GetArticlePageHTML())
                Case "ARTICLE"
                    Response.Write(my404.GetArticlePageHTML())
                Case "BLOG"
                    Response.Write(my404.GetBlogPageHTML())
                Case "GALLERY"
                    Response.Write(my404.GetCatalogPageHTML())
                Case "CATALOG"
                    Response.Write(my404.GetCatalogPageHTML())
                Case "FORM"
                    Response.Write(my404.GetFormPageHTML())
                Case "LINK LIST"
                    Response.Write(my404.GetLinkPageHTML())
                Case "LINK DIR"
                    Response.Write(my404.GetLinkDirectoryHTML())
                Case "SITEMAP"
                    my404.CurrentMapRow.PageName = my404.mySiteFile.CompanyName & " Sitemap"
                    my404.CurrentMapRow.PageTitle = my404.mySiteFile.CompanyName & " Sitemap"
                    Response.Write(my404.GetHTML(my404.mySiteFile.SiteDescription & my404.mySiteFile.TreeHTML.ToString, False, my404.mySession.SiteTemplatePrefix))
                Case "SITEMAP.XML"
                    Page.Response.ContentType = "text/xml"
                    Dim gen As mhXMLSitemap = New mhXMLSitemap(Page.Response.Output)
                    gen.WriteSitemapDocument()
                    gen.Close()
                Case Else
                    '   Server.Transfer(my404.CurrentMapRow.TransferURL)
                    Response.Redirect(my404.CurrentMapRow.TransferURL)
            End Select
        Else
            If Response.Status = "301 Moved Permanently" Then
                ' do nothing
            Else
                Response.Status = "404 Not Found"
                Response.StatusCode = "404"
                my404.CurrentMapRow.PageName = "404 - File Not Found"
                my404.CurrentMapRow.PageTitle = "404 - File Not Found"
                Dim myContents As New StringBuilder
                Dim sFileName As New StringBuilder
                If Not (Request.QueryString.Count = 0) Then
                    sFileName.Append(Replace(Request.QueryString.Item(0), "404;", ""))
                Else
                    sFileName.Append("?")
                End If
                mhfio.ReadTextFile(HttpContext.Current.Server.MapPath("/mhweb/404-Text.html"), myContents)
                myContents.Replace("<RequestURL>", sFileName.ToString)
                myContents.Replace("<UserHostAddress>", Request.UserHostAddress)
                myContents.Replace("<UserLanguages>", Request.UserAgent)
                myContents.Replace("<RequestBrowser>", Request.Browser.Browser)
                my404.mySession.AddHTMLHead = myContents.ToString
                Response.Write(my404.GetHTML("<blockquote>The page you were looking for can not be found</blockquote><br/><strong>" & sFileName.ToString & "</strong><br/><br/><form><div align=""center""><textarea rows=8 cols=60 wrap=soft></textarea></div></form><br/><br/><hr><sitemap>", False, my404.mySession.SiteTemplatePrefix))
            End If
        End If
        
    End Sub
    </script>
