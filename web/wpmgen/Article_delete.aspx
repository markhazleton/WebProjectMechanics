<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="Article_delete.aspx.vb" Inherits="Article_delete" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var Article_delete = new ew_Page("Article_delete");
// page properties
Article_delete.PageID = "delete"; // page ID
var EW_PAGE_ID = Article_delete.PageID; // for backward compatibility
Article_delete.SelectAllKey = function(elem) {
	ew_SelectAll(elem);
}
<% If EW_CLIENT_VALIDATE Then %>
Article_delete.ValidateRequired = true; // uses JavaScript validation
<% Else %>
Article_delete.ValidateRequired = false; // no JavaScript validation
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
Rs = Article_delete.LoadRecordset()
If Article_delete.lTotalRecs <= 0 Then ' No record found, exit
	Rs.Close()
	Rs.Dispose()
	Article_delete.Page_Terminate("Article_list.aspx") ' Return to list
End If
%>
<p><span class="aspnetmaker">Delete From TABLE: Site Article<br /><br />
<a href="<%= Article.ReturnUrl %>">Go Back</a></span></p>
<% Article_delete.ShowMessage() %>
<form method="post">
<p />
<input type="hidden" name="t" id="t" value="Article" />
<input type="hidden" name="a_delete" id="a_delete" value="D" />
<% For i As Integer = 0 to Article_delete.arRecKeys.GetUpperBound(0) %>
<input type="hidden" name="key_m" id="key_m" value="<%= Server.HtmlEncode(Convert.ToString(Article_delete.arRecKeys(i))) %>" />
<% Next %>
<table class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable ewTableSeparate">
<%= Article.TableCustomInnerHTML %>
	<thead>
	<tr class="ewTableHeader">
		<td valign="top">Active</td>
		<td valign="top">Start DT</td>
		<td valign="top">Title</td>
		<td valign="top">Location</td>
	</tr>
	</thead>
	<tbody>
<%
Article_delete.lRecCnt = 0
Do While Rs.Read()
	Article_delete.lRecCnt = Article_delete.lRecCnt + 1

	' Set row properties
	Article.CssClass = ""
	Article.CssStyle = ""
	Article.RowType = EW_ROWTYPE_VIEW ' view

	' Get the field contents
	Article_delete.LoadRowValues(Rs)

	' Render row
	Article_delete.RenderRow()
%>
	<tr<%= Article.RowAttributes %>>
		<td<%= Article.Active.CellAttributes %>>
<% If Convert.ToString(Article.Active.CurrentValue) = "1" Then %>
<input type="checkbox" value="<%= Article.Active.ListViewValue %>" checked onclick="this.form.reset();" disabled="disabled" />
<% Else %>
<input type="checkbox" value="<%= Article.Active.ListViewValue %>" onclick="this.form.reset();" disabled="disabled" />
<% End If %></td>
		<td<%= Article.StartDT.CellAttributes %>>
<div<%= Article.StartDT.ViewAttributes %>><%= Article.StartDT.ListViewValue %></div>
</td>
		<td<%= Article.Title.CellAttributes %>>
<div<%= Article.Title.ViewAttributes %>><%= Article.Title.ListViewValue %></div>
</td>
		<td<%= Article.zPageID.CellAttributes %>>
<div<%= Article.zPageID.ViewAttributes %>><%= Article.zPageID.ListViewValue %></div>
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
