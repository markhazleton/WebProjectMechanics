<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="Contact_delete.aspx.vb" Inherits="Contact_delete" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var Contact_delete = new ew_Page("Contact_delete");
// page properties
Contact_delete.PageID = "delete"; // page ID
var EW_PAGE_ID = Contact_delete.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
Contact_delete.ValidateRequired = true; // uses JavaScript validation
<% Else %>
Contact_delete.ValidateRequired = false; // no JavaScript validation
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
Rs = Contact_delete.LoadRecordset()
If Contact_delete.lTotalRecs <= 0 Then ' No record found, exit
	Rs.Close()
	Rs.Dispose()
	Contact_delete.Page_Terminate("Contact_list.aspx") ' Return to list
End If
%>
<p><span class="aspnetmaker">Delete From TABLE: Site Contact<br /><br />
<a href="<%= Contact.ReturnUrl %>">Go Back</a></span></p>
<% Contact_delete.ShowMessage() %>
<form method="post">
<p />
<input type="hidden" name="t" id="t" value="Contact" />
<input type="hidden" name="a_delete" id="a_delete" value="D" />
<% For i As Integer = 0 to Contact_delete.arRecKeys.GetUpperBound(0) %>
<input type="hidden" name="key_m" id="key_m" value="<%= Server.HtmlEncode(Convert.ToString(Contact_delete.arRecKeys(i))) %>" />
<% Next %>
<table class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable ewTableSeparate">
<%= Contact.TableCustomInnerHTML %>
	<thead>
	<tr class="ewTableHeader">
		<td valign="top">Logon Name</td>
		<td valign="top">Full Name</td>
		<td valign="top">Active</td>
		<td valign="top">Company</td>
		<td valign="top">Group</td>
	</tr>
	</thead>
	<tbody>
<%
Contact_delete.lRecCnt = 0
Do While Rs.Read()
	Contact_delete.lRecCnt = Contact_delete.lRecCnt + 1

	' Set row properties
	Contact.CssClass = ""
	Contact.CssStyle = ""
	Contact.RowType = EW_ROWTYPE_VIEW ' view

	' Get the field contents
	Contact_delete.LoadRowValues(Rs)

	' Render row
	Contact_delete.RenderRow()
%>
	<tr<%= Contact.RowAttributes %>>
		<td<%= Contact.LogonName.CellAttributes %>>
<div<%= Contact.LogonName.ViewAttributes %>><%= Contact.LogonName.ListViewValue %></div>
</td>
		<td<%= Contact.PrimaryContact.CellAttributes %>>
<div<%= Contact.PrimaryContact.ViewAttributes %>><%= Contact.PrimaryContact.ListViewValue %></div>
</td>
		<td<%= Contact.Active.CellAttributes %>>
<% If Convert.ToString(Contact.Active.CurrentValue) = "1" Then %>
<input type="checkbox" value="<%= Contact.Active.ListViewValue %>" checked onclick="this.form.reset();" disabled="disabled" />
<% Else %>
<input type="checkbox" value="<%= Contact.Active.ListViewValue %>" onclick="this.form.reset();" disabled="disabled" />
<% End If %></td>
		<td<%= Contact.CompanyID.CellAttributes %>>
<div<%= Contact.CompanyID.ViewAttributes %>><%= Contact.CompanyID.ListViewValue %></div>
</td>
		<td<%= Contact.GroupID.CellAttributes %>>
<div<%= Contact.GroupID.ViewAttributes %>><%= Contact.GroupID.ListViewValue %></div>
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
