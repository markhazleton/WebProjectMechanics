<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="SiteCategory_delete.aspx.vb" Inherits="SiteCategory_delete" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var SiteCategory_delete = new ew_Page("SiteCategory_delete");
// page properties
SiteCategory_delete.PageID = "delete"; // page ID
var EW_PAGE_ID = SiteCategory_delete.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
SiteCategory_delete.ValidateRequired = true; // uses JavaScript validation
<% Else %>
SiteCategory_delete.ValidateRequired = false; // no JavaScript validation
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
Rs = SiteCategory_delete.LoadRecordset()
If SiteCategory_delete.lTotalRecs <= 0 Then ' No record found, exit
	Rs.Close()
	Rs.Dispose()
	SiteCategory_delete.Page_Terminate("SiteCategory_list.aspx") ' Return to list
End If
%>
<p><span class="aspnetmaker">Delete From TABLE: Site Type Location<br /><br />
<a href="<%= SiteCategory.ReturnUrl %>">Go Back</a></span></p>
<% SiteCategory_delete.ShowMessage() %>
<form method="post">
<p />
<input type="hidden" name="t" id="t" value="SiteCategory" />
<input type="hidden" name="a_delete" id="a_delete" value="D" />
<% For i As Integer = 0 to SiteCategory_delete.arRecKeys.GetUpperBound(0) %>
<input type="hidden" name="key_m" id="key_m" value="<%= Server.HtmlEncode(Convert.ToString(SiteCategory_delete.arRecKeys(i))) %>" />
<% Next %>
<table class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable ewTableSeparate">
<%= SiteCategory.TableCustomInnerHTML %>
	<thead>
	<tr class="ewTableHeader">
		<td valign="top">Site Type</td>
		<td valign="top">Order</td>
		<td valign="top">Name</td>
		<td valign="top">Parent Category</td>
		<td valign="top">Category File Name</td>
		<td valign="top">Site Group</td>
	</tr>
	</thead>
	<tbody>
<%
SiteCategory_delete.lRecCnt = 0
Do While Rs.Read()
	SiteCategory_delete.lRecCnt = SiteCategory_delete.lRecCnt + 1

	' Set row properties
	SiteCategory.CssClass = ""
	SiteCategory.CssStyle = ""
	SiteCategory.RowType = EW_ROWTYPE_VIEW ' view

	' Get the field contents
	SiteCategory_delete.LoadRowValues(Rs)

	' Render row
	SiteCategory_delete.RenderRow()
%>
	<tr<%= SiteCategory.RowAttributes %>>
		<td<%= SiteCategory.SiteCategoryTypeID.CellAttributes %>>
<div<%= SiteCategory.SiteCategoryTypeID.ViewAttributes %>><%= SiteCategory.SiteCategoryTypeID.ListViewValue %></div>
</td>
		<td<%= SiteCategory.GroupOrder.CellAttributes %>>
<div<%= SiteCategory.GroupOrder.ViewAttributes %>><%= SiteCategory.GroupOrder.ListViewValue %></div>
</td>
		<td<%= SiteCategory.CategoryName.CellAttributes %>>
<div<%= SiteCategory.CategoryName.ViewAttributes %>><%= SiteCategory.CategoryName.ListViewValue %></div>
</td>
		<td<%= SiteCategory.ParentCategoryID.CellAttributes %>>
<div<%= SiteCategory.ParentCategoryID.ViewAttributes %>><%= SiteCategory.ParentCategoryID.ListViewValue %></div>
</td>
		<td<%= SiteCategory.CategoryFileName.CellAttributes %>>
<div<%= SiteCategory.CategoryFileName.ViewAttributes %>><%= SiteCategory.CategoryFileName.ListViewValue %></div>
</td>
		<td<%= SiteCategory.SiteCategoryGroupID.CellAttributes %>>
<div<%= SiteCategory.SiteCategoryGroupID.ViewAttributes %>><%= SiteCategory.SiteCategoryGroupID.ListViewValue %></div>
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
