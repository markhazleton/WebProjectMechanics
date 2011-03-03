<%@ Page ClassName="role_delete" Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="role_delete.aspx.vb" Inherits="role_delete" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var role_delete = new ew_Page("role_delete");
// page properties
role_delete.PageID = "delete"; // page ID
role_delete.FormID = "froledelete"; // form ID 
var EW_PAGE_ID = role_delete.PageID; // for backward compatibility
// extend page with Form_CustomValidate function
role_delete.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
role_delete.ValidateRequired = true; // uses JavaScript validation
<% Else %>
role_delete.ValidateRequired = false; // no JavaScript validation
<% End If %>
//-->
</script>
<script language="JavaScript" type="text/javascript">
<!--
// Write your client script here, no need to add script tags.
// To include another .js script, you can use, e.g.
// ew_ClientScriptInclude("my_javascript.js"); or
// ew_ClientScriptInclude("my_javascript.js", {onSuccess: function(o) { // your code here }});
//-->
</script>
<%

' Load records for display
Rs = role_delete.LoadRecordset()
If role_delete.lTotalRecs <= 0 Then ' No record found, exit
	Rs.Close()
	Rs.Dispose()
	role_delete.Page_Terminate("role_list.aspx") ' Return to list
End If
%>
<p><span class="aspnetmaker"><%= Language.Phrase("Delete") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= role.TableCaption %><br /><br />
<a href="<%= role.ReturnUrl %>"><%= Language.Phrase("GoBack") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(role_delete.DebugMsg) %>
<% role_delete.ShowMessage() %>
<form method="post">
<p />
<input type="hidden" name="t" id="t" value="role" />
<input type="hidden" name="a_delete" id="a_delete" value="D" />
<% For i As Integer = 0 to role_delete.arRecKeys.GetUpperBound(0) %>
<input type="hidden" name="key_m" id="key_m" value="<%= Server.HtmlEncode(Convert.ToString(role_delete.arRecKeys(i))) %>" />
<% Next %>
<table class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable ewTableSeparate">
<%= role.TableCustomInnerHTML %>
	<thead>
	<tr class="ewTableHeader">
		<td valign="top"><%= role.RoleID.FldCaption %></td>
		<td valign="top"><%= role.RoleName.FldCaption %></td>
		<td valign="top"><%= role.RoleTitle.FldCaption %></td>
		<td valign="top"><%= role.RoleComment.FldCaption %></td>
		<td valign="top"><%= role.FilterMenu.FldCaption %></td>
	</tr>
	</thead>
	<tbody>
<%
role_delete.lRecCnt = 0
Do While Rs.Read()
	role_delete.lRecCnt = role_delete.lRecCnt + 1

	' Set row properties
	role.CssClass = ""
	role.CssStyle = ""
	role.RowType = EW_ROWTYPE_VIEW ' view

	' Get the field contents
	role_delete.LoadRowValues(Rs)

	' Render row
	role_delete.RenderRow()
%>
	<tr<%= role.RowAttributes %>>
		<td<%= role.RoleID.CellAttributes %>>
<div<%= role.RoleID.ViewAttributes %>><%= role.RoleID.ListViewValue %></div>
</td>
		<td<%= role.RoleName.CellAttributes %>>
<div<%= role.RoleName.ViewAttributes %>><%= role.RoleName.ListViewValue %></div>
</td>
		<td<%= role.RoleTitle.CellAttributes %>>
<div<%= role.RoleTitle.ViewAttributes %>><%= role.RoleTitle.ListViewValue %></div>
</td>
		<td<%= role.RoleComment.CellAttributes %>>
<div<%= role.RoleComment.ViewAttributes %>><%= role.RoleComment.ListViewValue %></div>
</td>
		<td<%= role.FilterMenu.CellAttributes %>>
<% If Convert.ToString(role.FilterMenu.CurrentValue) = "1" Then %>
<input type="checkbox" value="<%= role.FilterMenu.ListViewValue %>" checked onclick="this.form.reset();" disabled="disabled" />
<% Else %>
<input type="checkbox" value="<%= role.FilterMenu.ListViewValue %>" onclick="this.form.reset();" disabled="disabled" />
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
<input type="submit" name="Action" id="Action" value="<%= ew_BtnCaption(Language.Phrase("DeleteBtn")) %>" />
</form>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
</asp:Content>