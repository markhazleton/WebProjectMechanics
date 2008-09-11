<%@ Page Language="VB"  %>
<script runat="server" type="text/VB">
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
        session("CurrentArticleID")=""
        session("CurrentPageID")=""
        Dim mySiteMap As New mhSiteMap(Session)
        mySiteMap.FindCurrentRow(mySiteMap.mySiteFile.SiteHomePageID, "", "Page")
        If mySiteMap.CurrentMapRow.PageTypeCD = "CATALOG" Then
            Response.Write(mySiteMap.GetCatalogPageHTML())
        Else
            Response.Write(mySiteMap.GetArticlePageHTML())
        End If
        
    End Sub
</script>
