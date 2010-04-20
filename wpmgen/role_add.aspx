<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="role_add.aspx.vb" Inherits="role_add" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var role_add = new ew_Page("role_add");
// page properties
role_add.PageID = "add"; // page ID
var EW_PAGE_ID = role_add.PageID; // for backward compatibility
// extend page with ValidateForm function
role_add.ValidateForm = function(fobj) {
	if (!this.ValidateRequired)
		return true; // ignore validation
	if (fobj.a_confirm && fobj.a_confirm.value == "F")
		return true;
	var i, elm, aelm, infix;
	var rowcnt = (fobj.key_count) ? Number(fobj.key_count.value) : 1;
	for (i=0; i<rowcnt; i++) {
		infix = (fobj.key_count) ? String(i+1) : "";
	}
	return true;
}
<% If EW_CLIENT_VALIDATE Then %>
role_add.ValidateRequired = true; // uses JavaScript validation
<% Else %>
role_add.ValidateRequired = false; // no JavaScript validation
<% End If %>
//-->
</script>
<script type="text/javascript">
<!--
var ew_DHTMLEditors = [];
//-->
</script>
<script language="JavaScript" type="text/javascript">
<!--
// Write your client script here, no need to add script tags.
// To include another .js script, use:
// ew_ClientScriptInclude("my_javascript.js"); 
//-->
</script>
<p><span class="aspnetmaker">Add to TABLE: Contact Role<br /><br />
<a href="<%= role.ReturnUrl %>">Go Back</a></span></p>
<% role_add.ShowMessage() %>
<form name="froleadd" id="froleadd" method="post" onsubmit="this.action=location.pathname;return role_add.ValidateForm(this);">
<p />
<input type="hidden" name="t" id="t" value="role" />
<input type="hidden" name="a_add" id="a_add" value="A" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
<% If role.RoleName.Visible Then ' RoleName %>
	<tr<%= role.RoleName.RowAttributes %>>
		<td class="ewTableHeader">Role Name</td>
		<td<%= role.RoleName.CellAttributes %>><span id="el_RoleName">
<input type="text" name="x_RoleName" id="x_RoleName" size="30" maxlength="50" value="<%= role.RoleName.EditValue %>"<%= role.RoleName.EditAttributes %> />
</span><%= role.RoleName.CustomMsg %></td>
	</tr>
<% End If %>
<% If role.RoleTitle.Visible Then ' RoleTitle %>
	<tr<%= role.RoleTitle.RowAttributes %>>
		<td class="ewTableHeader">Role Title</td>
		<td<%= role.RoleTitle.CellAttributes %>><span id="el_RoleTitle">
<input type="text" name="x_RoleTitle" id="x_RoleTitle" size="30" maxlength="50" value="<%= role.RoleTitle.EditValue %>"<%= role.RoleTitle.EditAttributes %> />
</span><%= role.RoleTitle.CustomMsg %></td>
	</tr>
<% End If %>
<% If role.RoleComment.Visible Then ' RoleComment %>
	<tr<%= role.RoleComment.RowAttributes %>>
		<td class="ewTableHeader">Role Comment</td>
		<td<%= role.RoleComment.CellAttributes %>><span id="el_RoleComment">
<input type="text" name="x_RoleComment" id="x_RoleComment" size="30" maxlength="50" value="<%= role.RoleComment.EditValue %>"<%= role.RoleComment.EditAttributes %> />
</span><%= role.RoleComment.CustomMsg %></td>
	</tr>
<% End If %>
<% If role.FilterMenu.Visible Then ' FilterMenu %>
	<tr<%= role.FilterMenu.RowAttributes %>>
		<td class="ewTableHeader">Filter Menu</td>
		<td<%= role.FilterMenu.CellAttributes %>><span id="el_FilterMenu">
<%
If ew_SameStr(role.FilterMenu.CurrentValue, "1") Then
	selwrk = " checked=""checked"""
Else
	selwrk = ""
End If
%>
<input type="checkbox" name="x_FilterMenu" id="x_FilterMenu" value="1"<%= selwrk %><%= role.FilterMenu.EditAttributes %> />
</span><%= role.FilterMenu.CustomMsg %></td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<p />
<input type="submit" name="btnAction" id="btnAction" value=" Save New " />
</form>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
</asp:Content>
