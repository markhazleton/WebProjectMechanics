<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="SiteCategoryGroup_delete.aspx.vb" Inherits="SiteCategoryGroup_delete" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var SiteCategoryGroup_delete = new ew_Page("SiteCategoryGroup_delete");
// page properties
SiteCategoryGroup_delete.PageID = "delete"; // page ID
var EW_PAGE_ID = SiteCategoryGroup_delete.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
SiteCategoryGroup_delete.ValidateRequired = true; // uses JavaScript validation
<% Else %>
SiteCategoryGroup_delete.ValidateRequired = false; // no JavaScript validation
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
Rs = SiteCategoryGroup_delete.LoadRecordset()
If SiteCategoryGroup_delete.lTotalRecs <= 0 Then ' No record found, exit
	Rs.Close()
	Rs.Dispose()
	SiteCategoryGroup_delete.Page_Terminate("SiteCategoryGroup_list.aspx") ' Return to list
End If
%>
<p><span class="aspnetmaker">Delete From TABLE: Location Group<br /><br />
<a href="<%= SiteCategoryGroup.ReturnUrl %>">Go Back</a></span></p>
<% SiteCategoryGroup_delete.ShowMessage() %>
<form method="post">
<p />
<input type="hidden" name="t" id="t" value="SiteCategoryGroup" />
<input type="hidden" name="a_delete" id="a_delete" value="D" />
<% For i As Integer = 0 to SiteCategoryGroup_delete.arRecKeys.GetUpperBound(0) %>
<input type="hidden" name="key_m" id="key_m" value="<%= Server.HtmlEncode(Convert.ToString(SiteCategoryGroup_delete.arRecKeys(i))) %>" />
<% Next %>
<table class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable ewTableSeparate">
<%= SiteCategoryGroup.TableCustomInnerHTML %>
	<thead>
	<tr class="ewTableHeader">
		<td valign="top">Site Category Group ID</td>
		<td valign="top">Site Category Group NM</td>
		<td valign="top">Site Category Group DS</td>
		<td valign="top">Site Category Group Order</td>
	</tr>
	</thead>
	<tbody>
<%
SiteCategoryGroup_delete.lRecCnt = 0
Do While Rs.Read()
	SiteCategoryGroup_delete.lRecCnt = SiteCategoryGroup_delete.lRecCnt + 1

	' Set row properties
	SiteCategoryGroup.CssClass = ""
	SiteCategoryGroup.CssStyle = ""
	SiteCategoryGroup.RowType = EW_ROWTYPE_VIEW ' view

	' Get the field contents
	SiteCategoryGroup_delete.LoadRowValues(Rs)

	' Render row
	SiteCategoryGroup_delete.RenderRow()
%>
	<tr<%= SiteCategoryGroup.RowAttributes %>>
		<td<%= SiteCategoryGroup.SiteCategoryGroupID.CellAttributes %>>
<div<%= SiteCategoryGroup.SiteCategoryGroupID.ViewAttributes %>><%= SiteCategoryGroup.SiteCategoryGroupID.ListViewValue %></div>
</td>
		<td<%= SiteCategoryGroup.SiteCategoryGroupNM.CellAttributes %>>
<div<%= SiteCategoryGroup.SiteCategoryGroupNM.ViewAttributes %>><%= SiteCategoryGroup.SiteCategoryGroupNM.ListViewValue %></div>
</td>
		<td<%= SiteCategoryGroup.SiteCategoryGroupDS.CellAttributes %>>
<div<%= SiteCategoryGroup.SiteCategoryGroupDS.ViewAttributes %>><%= SiteCategoryGroup.SiteCategoryGroupDS.ListViewValue %></div>
</td>
		<td<%= SiteCategoryGroup.SiteCategoryGroupOrder.CellAttributes %>>
<div<%= SiteCategoryGroup.SiteCategoryGroupOrder.ViewAttributes %>><%= SiteCategoryGroup.SiteCategoryGroupOrder.ListViewValue %></div>
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
