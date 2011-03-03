<%@ Import Namespace="WebProjectMechanics" %>
<script language="VB" runat="Server">
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim ActiveSite As New wpmActiveSite(Session)
        Page.Response.ContentType = "text/xml"
        Dim mhrss As wpmRssFeed = New wpmRssFeed(Page.Response.Output, ActiveSite)
        mhrss.WriteRSSMenuDocument()
        mhrss.Close()
        Page.Response.End
    End Sub
</script>
