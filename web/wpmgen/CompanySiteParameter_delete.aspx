<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="CompanySiteParameter_delete.aspx.vb" Inherits="CompanySiteParameter_delete" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var CompanySiteParameter_delete = new ew_Page("CompanySiteParameter_delete");
// page properties
CompanySiteParameter_delete.PageID = "delete"; // page ID
var EW_PAGE_ID = CompanySiteParameter_delete.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
CompanySiteParameter_delete.ValidateRequired = true; // uses JavaScript validation
<% Else %>
CompanySiteParameter_delete.ValidateRequired = false; // no JavaScript validation
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
Rs = CompanySiteParameter_delete.LoadRecordset()
If CompanySiteParameter_delete.lTotalRecs <= 0 Then ' No record found, exit
	Rs.Close()
	Rs.Dispose()
	CompanySiteParameter_delete.Page_Terminate("CompanySiteParameter_list.aspx") ' Return to list
End If
%>
<p><span class="aspnetmaker">Delete From TABLE: Site Parameter<br /><br />
<a href="<%= CompanySiteParameter.ReturnUrl %>">Go Back</a></span></p>
<% CompanySiteParameter_delete.ShowMessage() %>
<form method="post">
<p />
<input type="hidden" name="t" id="t" value="CompanySiteParameter" />
<input type="hidden" name="a_delete" id="a_delete" value="D" />
<% For i As Integer = 0 to CompanySiteParameter_delete.arRecKeys.GetUpperBound(0) %>
<input type="hidden" name="key_m" id="key_m" value="<%= Server.HtmlEncode(Convert.ToString(CompanySiteParameter_delete.arRecKeys(i))) %>" />
<% Next %>
<table class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable ewTableSeparate">
<%= CompanySiteParameter.TableCustomInnerHTML %>
	<thead>
	<tr class="ewTableHeader">
		<td valign="top">Site</td>
		<td valign="top">Page</td>
		<td valign="top">Category Group</td>
		<td valign="top"> Parameter</td>
		<td valign="top">Process Order</td>
	</tr>
	</thead>
	<tbody>
<%
CompanySiteParameter_delete.lRecCnt = 0
Do While Rs.Read()
	CompanySiteParameter_delete.lRecCnt = CompanySiteParameter_delete.lRecCnt + 1

	' Set row properties
	CompanySiteParameter.CssClass = ""
	CompanySiteParameter.CssStyle = ""
	CompanySiteParameter.RowType = EW_ROWTYPE_VIEW ' view

	' Get the field contents
	CompanySiteParameter_delete.LoadRowValues(Rs)

	' Render row
	CompanySiteParameter_delete.RenderRow()
%>
	<tr<%= CompanySiteParameter.RowAttributes %>>
		<td<%= CompanySiteParameter.CompanyID.CellAttributes %>>
<div<%= CompanySiteParameter.CompanyID.ViewAttributes %>><%= CompanySiteParameter.CompanyID.ListViewValue %></div>
</td>
		<td<%= CompanySiteParameter.zPageID.CellAttributes %>>
<div<%= CompanySiteParameter.zPageID.ViewAttributes %>><%= CompanySiteParameter.zPageID.ListViewValue %></div>
</td>
		<td<%= CompanySiteParameter.SiteCategoryGroupID.CellAttributes %>>
<div<%= CompanySiteParameter.SiteCategoryGroupID.ViewAttributes %>><%= CompanySiteParameter.SiteCategoryGroupID.ListViewValue %></div>
</td>
		<td<%= CompanySiteParameter.SiteParameterTypeID.CellAttributes %>>
<div<%= CompanySiteParameter.SiteParameterTypeID.ViewAttributes %>><%= CompanySiteParameter.SiteParameterTypeID.ListViewValue %></div>
</td>
		<td<%= CompanySiteParameter.SortOrder.CellAttributes %>>
<div<%= CompanySiteParameter.SortOrder.ViewAttributes %>><%= CompanySiteParameter.SortOrder.ListViewValue %></div>
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
