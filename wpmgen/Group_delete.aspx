<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="Group_delete.aspx.vb" Inherits="Group_delete" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var Group_delete = new ew_Page("Group_delete");
// page properties
Group_delete.PageID = "delete"; // page ID
var EW_PAGE_ID = Group_delete.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
Group_delete.ValidateRequired = true; // uses JavaScript validation
<% Else %>
Group_delete.ValidateRequired = false; // no JavaScript validation
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
Rs = Group_delete.LoadRecordset()
If Group_delete.lTotalRecs <= 0 Then ' No record found, exit
	Rs.Close()
	Rs.Dispose()
	Group_delete.Page_Terminate("Group_list.aspx") ' Return to list
End If
%>
<p><span class="aspnetmaker">Delete From TABLE: Contact Group<br /><br />
<a href="<%= Group.ReturnUrl %>">Go Back</a></span></p>
<% Group_delete.ShowMessage() %>
<form method="post">
<p />
<input type="hidden" name="t" id="t" value="Group" />
<input type="hidden" name="a_delete" id="a_delete" value="D" />
<% For i As Integer = 0 to Group_delete.arRecKeys.GetUpperBound(0) %>
<input type="hidden" name="key_m" id="key_m" value="<%= Server.HtmlEncode(Convert.ToString(Group_delete.arRecKeys(i))) %>" />
<% Next %>
<table class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable ewTableSeparate">
<%= Group.TableCustomInnerHTML %>
	<thead>
	<tr class="ewTableHeader">
		<td valign="top">Group Name</td>
		<td valign="top">Group Comment</td>
	</tr>
	</thead>
	<tbody>
<%
Group_delete.lRecCnt = 0
Do While Rs.Read()
	Group_delete.lRecCnt = Group_delete.lRecCnt + 1

	' Set row properties
	Group.CssClass = ""
	Group.CssStyle = ""
	Group.RowType = EW_ROWTYPE_VIEW ' view

	' Get the field contents
	Group_delete.LoadRowValues(Rs)

	' Render row
	Group_delete.RenderRow()
%>
	<tr<%= Group.RowAttributes %>>
		<td<%= Group.GroupName.CellAttributes %>>
<div<%= Group.GroupName.ViewAttributes %>><%= Group.GroupName.ListViewValue %></div>
</td>
		<td<%= Group.GroupComment.CellAttributes %>>
<div<%= Group.GroupComment.ViewAttributes %>><%= Group.GroupComment.ListViewValue %></div>
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
