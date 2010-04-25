<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="zMessage_delete.aspx.vb" Inherits="zMessage_delete" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var zMessage_delete = new ew_Page("zMessage_delete");
// page properties
zMessage_delete.PageID = "delete"; // page ID
var EW_PAGE_ID = zMessage_delete.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
zMessage_delete.ValidateRequired = true; // uses JavaScript validation
<% Else %>
zMessage_delete.ValidateRequired = false; // no JavaScript validation
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
Rs = zMessage_delete.LoadRecordset()
If zMessage_delete.lTotalRecs <= 0 Then ' No record found, exit
	Rs.Close()
	Rs.Dispose()
	zMessage_delete.Page_Terminate("zMessage_list.aspx") ' Return to list
End If
%>
<p><span class="aspnetmaker">Delete From TABLE: Message<br /><br />
<a href="<%= zMessage.ReturnUrl %>">Go Back</a></span></p>
<% zMessage_delete.ShowMessage() %>
<form method="post">
<p />
<input type="hidden" name="t" id="t" value="zMessage" />
<input type="hidden" name="a_delete" id="a_delete" value="D" />
<% For i As Integer = 0 to zMessage_delete.arRecKeys.GetUpperBound(0) %>
<input type="hidden" name="key_m" id="key_m" value="<%= Server.HtmlEncode(Convert.ToString(zMessage_delete.arRecKeys(i))) %>" />
<% Next %>
<table class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable ewTableSeparate">
<%= zMessage.TableCustomInnerHTML %>
	<thead>
	<tr class="ewTableHeader">
		<td valign="top">Message ID</td>
		<td valign="top">Page ID</td>
		<td valign="top">Parent Message ID</td>
		<td valign="top">Subject</td>
		<td valign="top">Author</td>
		<td valign="top">Email</td>
		<td valign="top">City</td>
		<td valign="top">URL</td>
		<td valign="top">Message Date</td>
	</tr>
	</thead>
	<tbody>
<%
zMessage_delete.lRecCnt = 0
Do While Rs.Read()
	zMessage_delete.lRecCnt = zMessage_delete.lRecCnt + 1

	' Set row properties
	zMessage.CssClass = ""
	zMessage.CssStyle = ""
	zMessage.RowType = EW_ROWTYPE_VIEW ' view

	' Get the field contents
	zMessage_delete.LoadRowValues(Rs)

	' Render row
	zMessage_delete.RenderRow()
%>
	<tr<%= zMessage.RowAttributes %>>
		<td<%= zMessage.MessageID.CellAttributes %>>
<div<%= zMessage.MessageID.ViewAttributes %>><%= zMessage.MessageID.ListViewValue %></div>
</td>
		<td<%= zMessage.zPageID.CellAttributes %>>
<div<%= zMessage.zPageID.ViewAttributes %>><%= zMessage.zPageID.ListViewValue %></div>
</td>
		<td<%= zMessage.ParentMessageID.CellAttributes %>>
<div<%= zMessage.ParentMessageID.ViewAttributes %>><%= zMessage.ParentMessageID.ListViewValue %></div>
</td>
		<td<%= zMessage.Subject.CellAttributes %>>
<div<%= zMessage.Subject.ViewAttributes %>><%= zMessage.Subject.ListViewValue %></div>
</td>
		<td<%= zMessage.Author.CellAttributes %>>
<div<%= zMessage.Author.ViewAttributes %>><%= zMessage.Author.ListViewValue %></div>
</td>
		<td<%= zMessage.zEmail.CellAttributes %>>
<div<%= zMessage.zEmail.ViewAttributes %>><%= zMessage.zEmail.ListViewValue %></div>
</td>
		<td<%= zMessage.City.CellAttributes %>>
<div<%= zMessage.City.ViewAttributes %>><%= zMessage.City.ListViewValue %></div>
</td>
		<td<%= zMessage.URL.CellAttributes %>>
<div<%= zMessage.URL.ViewAttributes %>><%= zMessage.URL.ListViewValue %></div>
</td>
		<td<%= zMessage.MessageDate.CellAttributes %>>
<div<%= zMessage.MessageDate.ViewAttributes %>><%= zMessage.MessageDate.ListViewValue %></div>
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
