<%@ Import Namespace="WebProjectMechanics" %>
<script language="VB" runat="Server">
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Page.Response.ContentType = "text/xml"
        Dim gen As SiteMapXmlTextWriter = New SiteMapXmlTextWriter(Page.Response.Output)
        gen.WriteSiteLocationDocument()
    End Sub
</script>
