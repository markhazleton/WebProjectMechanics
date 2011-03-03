<%@ Import Namespace="WebProjectMechanics" %>
<script language="VB" runat="Server">
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Page.Response.ContentType = "text/xml"
        Dim gen As wpmXMLSitemap = New wpmXMLSitemap(Page.Response.Output)
        gen.WriteSitemapDocument()
    End Sub
</script>
