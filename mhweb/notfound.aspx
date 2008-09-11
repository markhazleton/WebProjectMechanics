<%@ Page Language="vb"  %>
<script language="VB" runat="Server">
     '*********************
     '  Page Load Handler
     '*********************
    Private Sub Page_Load(ByVal s As System.Object, ByVal e As System.EventArgs)
        Dim mySiteMap As New mhSiteMap(Session)
        Response.Status = "404 Not Found"
        Response.StatusCode = "404"
        mySiteMap.CurrentMapRow.PageName = "404 - File Not Found"
        mySiteMap.CurrentMapRow.PageTitle = "404 - File Not Found"
        Response.Write(mySiteMap.GetHTML("<blockquote>The page you were looking for can not be found</blockquote>", False, mySiteMap.mySession.SiteTemplatePrefix))
    End Sub
    </script>
