<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="CompanySiteTypeParameter_delete.aspx.vb" Inherits="CompanySiteTypeParameter_delete" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var CompanySiteTypeParameter_delete = new ew_Page("CompanySiteTypeParameter_delete");
// page properties
CompanySiteTypeParameter_delete.PageID = "delete"; // page ID
var EW_PAGE_ID = CompanySiteTypeParameter_delete.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
CompanySiteTypeParameter_delete.ValidateRequired = true; // uses JavaScript validation
<% Else %>
CompanySiteTypeParameter_delete.ValidateRequired = false; // no JavaScript validation
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
Rs = CompanySiteTypeParameter_delete.LoadRecordset()
If CompanySiteTypeParameter_delete.lTotalRecs <= 0 Then ' No record found, exit
	Rs.Close()
	Rs.Dispose()
	CompanySiteTypeParameter_delete.Page_Terminate("CompanySiteTypeParameter_list.aspx") ' Return to list
End If
%>
<p><span class="aspnetmaker">Delete From TABLE: Site Type Parameter<br /><br />
<a href="<%= CompanySiteTypeParameter.ReturnUrl %>">Go Back</a></span></p>
<% CompanySiteTypeParameter_delete.ShowMessage() %>
<form method="post">
<p />
<input type="hidden" name="t" id="t" value="CompanySiteTypeParameter" />
<input type="hidden" name="a_delete" id="a_delete" value="D" />
<% For i As Integer = 0 to CompanySiteTypeParameter_delete.arRecKeys.GetUpperBound(0) %>
<input type="hidden" name="key_m" id="key_m" value="<%= Server.HtmlEncode(Convert.ToString(CompanySiteTypeParameter_delete.arRecKeys(i))) %>" />
<% Next %>
<table class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable ewTableSeparate">
<%= CompanySiteTypeParameter.TableCustomInnerHTML %>
	<thead>
	<tr class="ewTableHeader">
		<td valign="top">Parameter</td>
		<td valign="top">Site</td>
		<td valign="top">Site Type</td>
		<td valign="top">Site Group</td>
		<td valign="top">Site Category</td>
		<td valign="top">Process Order</td>
	</tr>
	</thead>
	<tbody>
<%
CompanySiteTypeParameter_delete.lRecCnt = 0
Do While Rs.Read()
	CompanySiteTypeParameter_delete.lRecCnt = CompanySiteTypeParameter_delete.lRecCnt + 1

	' Set row properties
	CompanySiteTypeParameter.CssClass = ""
	CompanySiteTypeParameter.CssStyle = ""
	CompanySiteTypeParameter.RowType = EW_ROWTYPE_VIEW ' view

	' Get the field contents
	CompanySiteTypeParameter_delete.LoadRowValues(Rs)

	' Render row
	CompanySiteTypeParameter_delete.RenderRow()
%>
	<tr<%= CompanySiteTypeParameter.RowAttributes %>>
		<td<%= CompanySiteTypeParameter.SiteParameterTypeID.CellAttributes %>>
<div<%= CompanySiteTypeParameter.SiteParameterTypeID.ViewAttributes %>><%= CompanySiteTypeParameter.SiteParameterTypeID.ListViewValue %></div>
</td>
		<td<%= CompanySiteTypeParameter.CompanyID.CellAttributes %>>
<div<%= CompanySiteTypeParameter.CompanyID.ViewAttributes %>><%= CompanySiteTypeParameter.CompanyID.ListViewValue %></div>
</td>
		<td<%= CompanySiteTypeParameter.SiteCategoryTypeID.CellAttributes %>>
<div<%= CompanySiteTypeParameter.SiteCategoryTypeID.ViewAttributes %>><%= CompanySiteTypeParameter.SiteCategoryTypeID.ListViewValue %></div>
</td>
		<td<%= CompanySiteTypeParameter.SiteCategoryGroupID.CellAttributes %>>
<div<%= CompanySiteTypeParameter.SiteCategoryGroupID.ViewAttributes %>><%= CompanySiteTypeParameter.SiteCategoryGroupID.ListViewValue %></div>
</td>
		<td<%= CompanySiteTypeParameter.SiteCategoryID.CellAttributes %>>
<div<%= CompanySiteTypeParameter.SiteCategoryID.ViewAttributes %>><%= CompanySiteTypeParameter.SiteCategoryID.ListViewValue %></div>
</td>
		<td<%= CompanySiteTypeParameter.SortOrder.CellAttributes %>>
<div<%= CompanySiteTypeParameter.SortOrder.ViewAttributes %>><%= CompanySiteTypeParameter.SortOrder.ListViewValue %></div>
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
