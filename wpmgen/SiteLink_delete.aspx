<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="SiteLink_delete.aspx.vb" Inherits="SiteLink_delete" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var SiteLink_delete = new ew_Page("SiteLink_delete");
// page properties
SiteLink_delete.PageID = "delete"; // page ID
var EW_PAGE_ID = SiteLink_delete.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
SiteLink_delete.ValidateRequired = true; // uses JavaScript validation
<% Else %>
SiteLink_delete.ValidateRequired = false; // no JavaScript validation
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
Rs = SiteLink_delete.LoadRecordset()
If SiteLink_delete.lTotalRecs <= 0 Then ' No record found, exit
	Rs.Close()
	Rs.Dispose()
	SiteLink_delete.Page_Terminate("SiteLink_list.aspx") ' Return to list
End If
%>
<p><span class="aspnetmaker">Delete From TABLE: Site Type Parts<br /><br />
<a href="<%= SiteLink.ReturnUrl %>">Go Back</a></span></p>
<% SiteLink_delete.ShowMessage() %>
<form method="post">
<p />
<input type="hidden" name="t" id="t" value="SiteLink" />
<input type="hidden" name="a_delete" id="a_delete" value="D" />
<% For i As Integer = 0 to SiteLink_delete.arRecKeys.GetUpperBound(0) %>
<input type="hidden" name="key_m" id="key_m" value="<%= Server.HtmlEncode(Convert.ToString(SiteLink_delete.arRecKeys(i))) %>" />
<% Next %>
<table class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable ewTableSeparate">
<%= SiteLink.TableCustomInnerHTML %>
	<thead>
	<tr class="ewTableHeader">
		<td valign="top">Site Type</td>
		<td valign="top">Title</td>
		<td valign="top">Link Type</td>
		<td valign="top">Link Category</td>
		<td valign="top">Company</td>
		<td valign="top">Site Category</td>
		<td valign="top">Category Group</td>
		<td valign="top">Ranks</td>
		<td valign="top">Active/Visible</td>
	</tr>
	</thead>
	<tbody>
<%
SiteLink_delete.lRecCnt = 0
Do While Rs.Read()
	SiteLink_delete.lRecCnt = SiteLink_delete.lRecCnt + 1

	' Set row properties
	SiteLink.CssClass = ""
	SiteLink.CssStyle = ""
	SiteLink.RowType = EW_ROWTYPE_VIEW ' view

	' Get the field contents
	SiteLink_delete.LoadRowValues(Rs)

	' Render row
	SiteLink_delete.RenderRow()
%>
	<tr<%= SiteLink.RowAttributes %>>
		<td<%= SiteLink.SiteCategoryTypeID.CellAttributes %>>
<div<%= SiteLink.SiteCategoryTypeID.ViewAttributes %>><%= SiteLink.SiteCategoryTypeID.ListViewValue %></div>
</td>
		<td<%= SiteLink.Title.CellAttributes %>>
<div<%= SiteLink.Title.ViewAttributes %>><%= SiteLink.Title.ListViewValue %></div>
</td>
		<td<%= SiteLink.LinkTypeCD.CellAttributes %>>
<div<%= SiteLink.LinkTypeCD.ViewAttributes %>><%= SiteLink.LinkTypeCD.ListViewValue %></div>
</td>
		<td<%= SiteLink.CategoryID.CellAttributes %>>
<div<%= SiteLink.CategoryID.ViewAttributes %>><%= SiteLink.CategoryID.ListViewValue %></div>
</td>
		<td<%= SiteLink.CompanyID.CellAttributes %>>
<div<%= SiteLink.CompanyID.ViewAttributes %>><%= SiteLink.CompanyID.ListViewValue %></div>
</td>
		<td<%= SiteLink.SiteCategoryID.CellAttributes %>>
<div<%= SiteLink.SiteCategoryID.ViewAttributes %>><%= SiteLink.SiteCategoryID.ListViewValue %></div>
</td>
		<td<%= SiteLink.SiteCategoryGroupID.CellAttributes %>>
<div<%= SiteLink.SiteCategoryGroupID.ViewAttributes %>><%= SiteLink.SiteCategoryGroupID.ListViewValue %></div>
</td>
		<td<%= SiteLink.Ranks.CellAttributes %>>
<div<%= SiteLink.Ranks.ViewAttributes %>><%= SiteLink.Ranks.ListViewValue %></div>
</td>
		<td<%= SiteLink.Views.CellAttributes %>>
<% If Convert.ToString(SiteLink.Views.CurrentValue) = "1" Then %>
<input type="checkbox" value="<%= SiteLink.Views.ListViewValue %>" checked onclick="this.form.reset();" disabled="disabled" />
<% Else %>
<input type="checkbox" value="<%= SiteLink.Views.ListViewValue %>" onclick="this.form.reset();" disabled="disabled" />
<% End If %></td>
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
