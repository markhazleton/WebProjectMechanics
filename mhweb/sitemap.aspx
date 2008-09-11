<script language="VB" runat="Server">
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ' Check to make sure the configuration is set
        Page.Response.ContentType = "text/xml"
        Dim gen As mhXMLSitemap = New mhXMLSitemap(Page.Response.Output)
        gen.WriteSitemapDocument()
        gen.Close()
    End Sub
</script>
