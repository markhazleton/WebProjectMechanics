<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="PageAlias_delete.aspx.vb" Inherits="PageAlias_delete" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var PageAlias_delete = new ew_Page("PageAlias_delete");
// page properties
PageAlias_delete.PageID = "delete"; // page ID
var EW_PAGE_ID = PageAlias_delete.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
PageAlias_delete.ValidateRequired = true; // uses JavaScript validation
<% Else %>
PageAlias_delete.ValidateRequired = false; // no JavaScript validation
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
Rs = PageAlias_delete.LoadRecordset()
If PageAlias_delete.lTotalRecs <= 0 Then ' No record found, exit
	Rs.Close()
	Rs.Dispose()
	PageAlias_delete.Page_Terminate("PageAlias_list.aspx") ' Return to list
End If
%>
<p><span class="aspnetmaker">Delete From TABLE: Location Alias<br /><br />
<a href="<%= PageAlias.ReturnUrl %>">Go Back</a></span></p>
<% PageAlias_delete.ShowMessage() %>
<form method="post">
<p />
<input type="hidden" name="t" id="t" value="PageAlias" />
<input type="hidden" name="a_delete" id="a_delete" value="D" />
<% For i As Integer = 0 to PageAlias_delete.arRecKeys.GetUpperBound(0) %>
<input type="hidden" name="key_m" id="key_m" value="<%= Server.HtmlEncode(Convert.ToString(PageAlias_delete.arRecKeys(i))) %>" />
<% Next %>
<table class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable ewTableSeparate">
<%= PageAlias.TableCustomInnerHTML %>
	<thead>
	<tr class="ewTableHeader">
		<td valign="top">Page URL</td>
		<td valign="top">Target URL</td>
		<td valign="top">Alias Type</td>
		<td valign="top">Company</td>
	</tr>
	</thead>
	<tbody>
<%
PageAlias_delete.lRecCnt = 0
Do While Rs.Read()
	PageAlias_delete.lRecCnt = PageAlias_delete.lRecCnt + 1

	' Set row properties
	PageAlias.CssClass = ""
	PageAlias.CssStyle = ""
	PageAlias.RowType = EW_ROWTYPE_VIEW ' view

	' Get the field contents
	PageAlias_delete.LoadRowValues(Rs)

	' Render row
	PageAlias_delete.RenderRow()
%>
	<tr<%= PageAlias.RowAttributes %>>
		<td<%= PageAlias.zPageURL.CellAttributes %>>
<div<%= PageAlias.zPageURL.ViewAttributes %>><%= PageAlias.zPageURL.ListViewValue %></div>
</td>
		<td<%= PageAlias.TargetURL.CellAttributes %>>
<div<%= PageAlias.TargetURL.ViewAttributes %>><%= PageAlias.TargetURL.ListViewValue %></div>
</td>
		<td<%= PageAlias.AliasType.CellAttributes %>>
<div<%= PageAlias.AliasType.ViewAttributes %>><%= PageAlias.AliasType.ListViewValue %></div>
</td>
		<td<%= PageAlias.CompanyID.CellAttributes %>>
<div<%= PageAlias.CompanyID.ViewAttributes %>><%= PageAlias.CompanyID.ListViewValue %></div>
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
