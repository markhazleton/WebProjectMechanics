<script language="VB" runat="Server">
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Page.Response.ContentType = "text/xml"
        Dim mySiteMap As New mhSiteMap(Session)
        Dim mhrss As New mhRssFeed(Page.Response.Output, mySiteMap)
        mhrss.WriteRSSBlogDocument(mySiteMap.CurrentMapRow.PageID, mySiteMap.mySession.CompanyID, mySiteMap.CurrentMapRow.PageName, mySiteMap.CurrentMapRow.PageDescription, mySiteMap.mySiteFile.CompanyName)
        mhrss.Close()
    End Sub
</script>
