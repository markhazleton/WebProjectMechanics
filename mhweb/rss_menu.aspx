<script language="VB" runat="Server">
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ' Check to make sure the configuration is set
        Dim mySiteMap As New mhSiteMap("MODIFIED", Session)
        Page.Response.ContentType = "text/xml"
        Dim mhrss As mhrssFeed = New mhrssFeed(Page.Response.Output, mySiteMap)
        mhrss.WriteRSSMenuDocument()
        mhrss.Close()
    End Sub
</script>
