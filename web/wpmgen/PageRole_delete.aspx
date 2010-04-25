<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="PageRole_delete.aspx.vb" Inherits="PageRole_delete" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var PageRole_delete = new ew_Page("PageRole_delete");
// page properties
PageRole_delete.PageID = "delete"; // page ID
var EW_PAGE_ID = PageRole_delete.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
PageRole_delete.ValidateRequired = true; // uses JavaScript validation
<% Else %>
PageRole_delete.ValidateRequired = false; // no JavaScript validation
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
Rs = PageRole_delete.LoadRecordset()
If PageRole_delete.lTotalRecs <= 0 Then ' No record found, exit
	Rs.Close()
	Rs.Dispose()
	PageRole_delete.Page_Terminate("PageRole_list.aspx") ' Return to list
End If
%>
<p><span class="aspnetmaker">Delete From TABLE: Location Role<br /><br />
<a href="<%= PageRole.ReturnUrl %>">Go Back</a></span></p>
<% PageRole_delete.ShowMessage() %>
<form method="post">
<p />
<input type="hidden" name="t" id="t" value="PageRole" />
<input type="hidden" name="a_delete" id="a_delete" value="D" />
<% For i As Integer = 0 to PageRole_delete.arRecKeys.GetUpperBound(0) %>
<input type="hidden" name="key_m" id="key_m" value="<%= Server.HtmlEncode(Convert.ToString(PageRole_delete.arRecKeys(i))) %>" />
<% Next %>
<table class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable ewTableSeparate">
<%= PageRole.TableCustomInnerHTML %>
	<thead>
	<tr class="ewTableHeader">
		<td valign="top">Page Role ID</td>
		<td valign="top">Role ID</td>
		<td valign="top">Page ID</td>
		<td valign="top">Company ID</td>
	</tr>
	</thead>
	<tbody>
<%
PageRole_delete.lRecCnt = 0
Do While Rs.Read()
	PageRole_delete.lRecCnt = PageRole_delete.lRecCnt + 1

	' Set row properties
	PageRole.CssClass = ""
	PageRole.CssStyle = ""
	PageRole.RowType = EW_ROWTYPE_VIEW ' view

	' Get the field contents
	PageRole_delete.LoadRowValues(Rs)

	' Render row
	PageRole_delete.RenderRow()
%>
	<tr<%= PageRole.RowAttributes %>>
		<td<%= PageRole.PageRoleID.CellAttributes %>>
<div<%= PageRole.PageRoleID.ViewAttributes %>><%= PageRole.PageRoleID.ListViewValue %></div>
</td>
		<td<%= PageRole.RoleID.CellAttributes %>>
<div<%= PageRole.RoleID.ViewAttributes %>><%= PageRole.RoleID.ListViewValue %></div>
</td>
		<td<%= PageRole.zPageID.CellAttributes %>>
<div<%= PageRole.zPageID.ViewAttributes %>><%= PageRole.zPageID.ListViewValue %></div>
</td>
		<td<%= PageRole.CompanyID.CellAttributes %>>
<div<%= PageRole.CompanyID.ViewAttributes %>><%= PageRole.CompanyID.ListViewValue %></div>
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
