<%@ Import Namespace="WebProjectMechanics" %>
<script language="VB" runat="Server">
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim ActiveSite As New ActiveCompany()
        Page.Response.ContentType = "text/xml"
        Dim mhrss As UtilityRSSFeed = New UtilityRSSFeed(Page.Response.Output, ActiveSite)
        mhrss.WriteRSSArticle()
        mhrss.Close()
        Page.Response.End
    End Sub
</script>
