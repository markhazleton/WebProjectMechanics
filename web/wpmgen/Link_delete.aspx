<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="Link_delete.aspx.vb" Inherits="Link_delete" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var Link_delete = new ew_Page("Link_delete");
// page properties
Link_delete.PageID = "delete"; // page ID
var EW_PAGE_ID = Link_delete.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
Link_delete.ValidateRequired = true; // uses JavaScript validation
<% Else %>
Link_delete.ValidateRequired = false; // no JavaScript validation
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
Rs = Link_delete.LoadRecordset()
If Link_delete.lTotalRecs <= 0 Then ' No record found, exit
	Rs.Close()
	Rs.Dispose()
	Link_delete.Page_Terminate("Link_list.aspx") ' Return to list
End If
%>
<p><span class="aspnetmaker">Delete From TABLE: Site Parts<br /><br />
<a href="<%= Link.ReturnUrl %>">Go Back</a></span></p>
<% Link_delete.ShowMessage() %>
<form method="post">
<p />
<input type="hidden" name="t" id="t" value="Link" />
<input type="hidden" name="a_delete" id="a_delete" value="D" />
<% For i As Integer = 0 to Link_delete.arRecKeys.GetUpperBound(0) %>
<input type="hidden" name="key_m" id="key_m" value="<%= Server.HtmlEncode(Convert.ToString(Link_delete.arRecKeys(i))) %>" />
<% Next %>
<table class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable ewTableSeparate">
<%= Link.TableCustomInnerHTML %>
	<thead>
	<tr class="ewTableHeader">
		<td valign="top">Title</td>
		<td valign="top">Part Type</td>
		<td valign="top">Part Category</td>
		<td valign="top">Location Group</td>
		<td valign="top">Location</td>
		<td valign="top">Visible/Active</td>
		<td valign="top">Ranks</td>
		<td valign="top">Date Add</td>
	</tr>
	</thead>
	<tbody>
<%
Link_delete.lRecCnt = 0
Do While Rs.Read()
	Link_delete.lRecCnt = Link_delete.lRecCnt + 1

	' Set row properties
	Link.CssClass = ""
	Link.CssStyle = ""
	Link.RowType = EW_ROWTYPE_VIEW ' view

	' Get the field contents
	Link_delete.LoadRowValues(Rs)

	' Render row
	Link_delete.RenderRow()
%>
	<tr<%= Link.RowAttributes %>>
		<td<%= Link.Title.CellAttributes %>>
<div<%= Link.Title.ViewAttributes %>><%= Link.Title.ListViewValue %></div>
</td>
		<td<%= Link.LinkTypeCD.CellAttributes %>>
<div<%= Link.LinkTypeCD.ViewAttributes %>><%= Link.LinkTypeCD.ListViewValue %></div>
</td>
		<td<%= Link.CategoryID.CellAttributes %>>
<div<%= Link.CategoryID.ViewAttributes %>><%= Link.CategoryID.ListViewValue %></div>
</td>
		<td<%= Link.SiteCategoryGroupID.CellAttributes %>>
<div<%= Link.SiteCategoryGroupID.ViewAttributes %>><%= Link.SiteCategoryGroupID.ListViewValue %></div>
</td>
		<td<%= Link.zPageID.CellAttributes %>>
<div<%= Link.zPageID.ViewAttributes %>><%= Link.zPageID.ListViewValue %></div>
</td>
		<td<%= Link.Views.CellAttributes %>>
<% If Convert.ToString(Link.Views.CurrentValue) = "1" Then %>
<input type="checkbox" value="<%= Link.Views.ListViewValue %>" checked onclick="this.form.reset();" disabled="disabled" />
<% Else %>
<input type="checkbox" value="<%= Link.Views.ListViewValue %>" onclick="this.form.reset();" disabled="disabled" />
<% End If %></td>
		<td<%= Link.Ranks.CellAttributes %>>
<div<%= Link.Ranks.ViewAttributes %>><%= Link.Ranks.ListViewValue %></div>
</td>
		<td<%= Link.DateAdd.CellAttributes %>>
<div<%= Link.DateAdd.ViewAttributes %>><%= Link.DateAdd.ListViewValue %></div>
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
