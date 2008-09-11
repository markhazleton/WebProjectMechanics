<%@ Page Language="VB"    %>
<script runat="server" type="text/VB" >
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim mySiteMap As New mhSiteMap(Session)
        Response.Write(mySiteMap.GetBlogPageHTML())
    End Sub
</script>
