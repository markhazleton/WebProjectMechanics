<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="SiteTemplate_delete.aspx.vb" Inherits="SiteTemplate_delete" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var SiteTemplate_delete = new ew_Page("SiteTemplate_delete");
// page properties
SiteTemplate_delete.PageID = "delete"; // page ID
var EW_PAGE_ID = SiteTemplate_delete.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
SiteTemplate_delete.ValidateRequired = true; // uses JavaScript validation
<% Else %>
SiteTemplate_delete.ValidateRequired = false; // no JavaScript validation
<% End If %>
//-->
</script>
<script language="JavaScript" type="text/javascript">
<!--
// Write your client script here, no need to add script tags.
// To include another .js script, use:
// ew_ClientScriptInclude("my_javascript.js"); 
//-->
</script>
<%

' Load records for display
Rs = SiteTemplate_delete.LoadRecordset()
If SiteTemplate_delete.lTotalRecs <= 0 Then ' No record found, exit
	Rs.Close()
	Rs.Dispose()
	SiteTemplate_delete.Page_Terminate("SiteTemplate_list.aspx") ' Return to list
End If
%>
<p><span class="aspnetmaker">Delete From TABLE: Presentation Template (skin)<br /><br />
<a href="<%= SiteTemplate.ReturnUrl %>">Go Back</a></span></p>
<% SiteTemplate_delete.ShowMessage() %>
<form method="post">
<p />
<input type="hidden" name="t" id="t" value="SiteTemplate" />
<input type="hidden" name="a_delete" id="a_delete" value="D" />
<% For i As Integer = 0 to SiteTemplate_delete.arRecKeys.GetUpperBound(0) %>
<input type="hidden" name="key_m" id="key_m" value="<%= Server.HtmlEncode(Convert.ToString(SiteTemplate_delete.arRecKeys(i))) %>" />
<% Next %>
<table class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable ewTableSeparate">
<%= SiteTemplate.TableCustomInnerHTML %>
	<thead>
	<tr class="ewTableHeader">
		<td valign="top">Template Prefix</td>
		<td valign="top">Name</td>
	</tr>
	</thead>
	<tbody>
<%
SiteTemplate_delete.lRecCnt = 0
Do While Rs.Read()
	SiteTemplate_delete.lRecCnt = SiteTemplate_delete.lRecCnt + 1

	' Set row properties
	SiteTemplate.CssClass = ""
	SiteTemplate.CssStyle = ""
	SiteTemplate.RowType = EW_ROWTYPE_VIEW ' view

	' Get the field contents
	SiteTemplate_delete.LoadRowValues(Rs)

	' Render row
	SiteTemplate_delete.RenderRow()
%>
	<tr<%= SiteTemplate.RowAttributes %>>
		<td<%= SiteTemplate.TemplatePrefix.CellAttributes %>>
<div<%= SiteTemplate.TemplatePrefix.ViewAttributes %>><%= SiteTemplate.TemplatePrefix.ListViewValue %></div>
</td>
		<td<%= SiteTemplate.zName.CellAttributes %>>
<div<%= SiteTemplate.zName.ViewAttributes %>><%= SiteTemplate.zName.ListViewValue %></div>
</td>
	</tr>
<%
Loop
Rs.Close()
Rs.Dispose()
%>
	</tbody>
</table>
</div>
</td></tr></table>
<p />
<input type="submit" name="Action" id="Action" value="Confirm Delete" />
</form>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
</asp:Content>
