<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="LinkCategory_delete.aspx.vb" Inherits="LinkCategory_delete" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var LinkCategory_delete = new ew_Page("LinkCategory_delete");
// page properties
LinkCategory_delete.PageID = "delete"; // page ID
var EW_PAGE_ID = LinkCategory_delete.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
LinkCategory_delete.ValidateRequired = true; // uses JavaScript validation
<% Else %>
LinkCategory_delete.ValidateRequired = false; // no JavaScript validation
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
Rs = LinkCategory_delete.LoadRecordset()
If LinkCategory_delete.lTotalRecs <= 0 Then ' No record found, exit
	Rs.Close()
	Rs.Dispose()
	LinkCategory_delete.Page_Terminate("LinkCategory_list.aspx") ' Return to list
End If
%>
<p><span class="aspnetmaker">Delete From TABLE: Part Category<br /><br />
<a href="<%= LinkCategory.ReturnUrl %>">Go Back</a></span></p>
<% LinkCategory_delete.ShowMessage() %>
<form method="post">
<p />
<input type="hidden" name="t" id="t" value="LinkCategory" />
<input type="hidden" name="a_delete" id="a_delete" value="D" />
<% For i As Integer = 0 to LinkCategory_delete.arRecKeys.GetUpperBound(0) %>
<input type="hidden" name="key_m" id="key_m" value="<%= Server.HtmlEncode(Convert.ToString(LinkCategory_delete.arRecKeys(i))) %>" />
<% Next %>
<table class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable ewTableSeparate">
<%= LinkCategory.TableCustomInnerHTML %>
	<thead>
	<tr class="ewTableHeader">
		<td valign="top">Title</td>
		<td valign="top">Parent</td>
		<td valign="top">Page</td>
	</tr>
	</thead>
	<tbody>
<%
LinkCategory_delete.lRecCnt = 0
Do While Rs.Read()
	LinkCategory_delete.lRecCnt = LinkCategory_delete.lRecCnt + 1

	' Set row properties
	LinkCategory.CssClass = ""
	LinkCategory.CssStyle = ""
	LinkCategory.RowType = EW_ROWTYPE_VIEW ' view

	' Get the field contents
	LinkCategory_delete.LoadRowValues(Rs)

	' Render row
	LinkCategory_delete.RenderRow()
%>
	<tr<%= LinkCategory.RowAttributes %>>
		<td<%= LinkCategory.Title.CellAttributes %>>
<div<%= LinkCategory.Title.ViewAttributes %>><%= LinkCategory.Title.ListViewValue %></div>
</td>
		<td<%= LinkCategory.ParentID.CellAttributes %>>
<div<%= LinkCategory.ParentID.ViewAttributes %>><%= LinkCategory.ParentID.ListViewValue %></div>
</td>
		<td<%= LinkCategory.zPageID.CellAttributes %>>
<div<%= LinkCategory.zPageID.ViewAttributes %>><%= LinkCategory.zPageID.ListViewValue %></div>
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
