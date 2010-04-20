<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="SiteCategoryType_delete.aspx.vb" Inherits="SiteCategoryType_delete" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var SiteCategoryType_delete = new ew_Page("SiteCategoryType_delete");
// page properties
SiteCategoryType_delete.PageID = "delete"; // page ID
var EW_PAGE_ID = SiteCategoryType_delete.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
SiteCategoryType_delete.ValidateRequired = true; // uses JavaScript validation
<% Else %>
SiteCategoryType_delete.ValidateRequired = false; // no JavaScript validation
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
Rs = SiteCategoryType_delete.LoadRecordset()
If SiteCategoryType_delete.lTotalRecs <= 0 Then ' No record found, exit
	Rs.Close()
	Rs.Dispose()
	SiteCategoryType_delete.Page_Terminate("SiteCategoryType_list.aspx") ' Return to list
End If
%>
<p><span class="aspnetmaker">Delete From TABLE: Site Type<br /><br />
<a href="<%= SiteCategoryType.ReturnUrl %>">Go Back</a></span></p>
<% SiteCategoryType_delete.ShowMessage() %>
<form method="post">
<p />
<input type="hidden" name="t" id="t" value="SiteCategoryType" />
<input type="hidden" name="a_delete" id="a_delete" value="D" />
<% For i As Integer = 0 to SiteCategoryType_delete.arRecKeys.GetUpperBound(0) %>
<input type="hidden" name="key_m" id="key_m" value="<%= Server.HtmlEncode(Convert.ToString(SiteCategoryType_delete.arRecKeys(i))) %>" />
<% Next %>
<table class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable ewTableSeparate">
<%= SiteCategoryType.TableCustomInnerHTML %>
	<thead>
	<tr class="ewTableHeader">
		<td valign="top">Site Type</td>
		<td valign="top">File Name</td>
		<td valign="top">Transfer URL</td>
		<td valign="top">Default Category</td>
	</tr>
	</thead>
	<tbody>
<%
SiteCategoryType_delete.lRecCnt = 0
Do While Rs.Read()
	SiteCategoryType_delete.lRecCnt = SiteCategoryType_delete.lRecCnt + 1

	' Set row properties
	SiteCategoryType.CssClass = ""
	SiteCategoryType.CssStyle = ""
	SiteCategoryType.RowType = EW_ROWTYPE_VIEW ' view

	' Get the field contents
	SiteCategoryType_delete.LoadRowValues(Rs)

	' Render row
	SiteCategoryType_delete.RenderRow()
%>
	<tr<%= SiteCategoryType.RowAttributes %>>
		<td<%= SiteCategoryType.SiteCategoryTypeNM.CellAttributes %>>
<div<%= SiteCategoryType.SiteCategoryTypeNM.ViewAttributes %>><%= SiteCategoryType.SiteCategoryTypeNM.ListViewValue %></div>
</td>
		<td<%= SiteCategoryType.SiteCategoryFileName.CellAttributes %>>
<div<%= SiteCategoryType.SiteCategoryFileName.ViewAttributes %>><%= SiteCategoryType.SiteCategoryFileName.ListViewValue %></div>
</td>
		<td<%= SiteCategoryType.SiteCategoryTransferURL.CellAttributes %>>
<div<%= SiteCategoryType.SiteCategoryTransferURL.ViewAttributes %>><%= SiteCategoryType.SiteCategoryTransferURL.ListViewValue %></div>
</td>
		<td<%= SiteCategoryType.DefaultSiteCategoryID.CellAttributes %>>
<div<%= SiteCategoryType.DefaultSiteCategoryID.ViewAttributes %>><%= SiteCategoryType.DefaultSiteCategoryID.ListViewValue %></div>
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
