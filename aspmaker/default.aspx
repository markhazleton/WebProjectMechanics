<%@ Page EnableViewState="true" Language="VB" %>
<%@ Import Namespace="PMGEN" %>
<%@ Import Namespace="EW.Web" %>
<%@ Register NameSpace="PMGEN" TagPrefix="PMGEN" %>
<script runat="server">

	' *************************
	' *  Handler for Page Load
	' *************************

	Protected Sub Page_Load(ByVal s As Object, ByVal e As System.EventArgs)
		Response.Redirect("company_list.aspx?cmd=resetall")
		Response.End()
	End Sub
</script>
<html>
    <head runat="server">
    <meta name="generator" content="ASP.NET Maker v3.2.0.1" />
</head>
    <body>
        <form id="form1" runat="server">
        </form>
    </body>
</html>
