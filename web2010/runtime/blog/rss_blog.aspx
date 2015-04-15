<%@ Import Namespace="WebProjectMechanics" %>
<script language="VB" runat="Server">
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim TimeScope As New String(Request.QueryString("TimeScope"))
        If TimeScope = String.Empty Then
            TimeScope = "ALL"
        End If
        Page.Response.ContentType = "text/xml"
        Dim ActiveSite As New ActiveCompany()
        Dim RSSFeed As New UtilityRSSFeed(Page.Response.Output, ActiveSite)
        RSSFeed.WriteRSSBlogDocument(TimeScope)
        RSSFeed.Close()
    End Sub
</script>
