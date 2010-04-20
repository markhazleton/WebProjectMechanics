<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="role_srch.aspx.vb" Inherits="role_srch" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var role_search = new ew_Page("role_search");
// page properties
role_search.PageID = "search"; // page ID
var EW_PAGE_ID = role_search.PageID; // for backward compatibility
// extend page with validate function for search
role_search.ValidateSearch = function(fobj) {
	if (!this.ValidateRequired)
		return true; // ignore validation
	var infix = "";
	elm = fobj.elements["x" + infix + "_RoleID"];
	if (elm && !ew_CheckInteger(elm.value))
		return ew_OnError(this, elm, "Incorrect integer - Role ID");
	for (var i=0;i<fobj.elements.length;i++) {
		var elem = fobj.elements[i];
		if (elem.name.substring(0,2) == "s_" || elem.name.substring(0,3) == "sv_")
			elem.value = "";
	}
	return true;
}
<% If EW_CLIENT_VALIDATE Then %>
role_search.ValidateRequired = true; // uses JavaScript validation
<% Else %>
role_search.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">Search TABLE: Contact Role<br /><br />
<a href="<%= role.ReturnUrl %>">Back to List</a></span></p>
<% role_search.ShowMessage() %>
<form name="frolesearch" id="frolesearch" method="post" onsubmit="this.action=location.pathname;return role_search.ValidateSearch(this);">
<p />
<input type="hidden" name="t" id="t" value="role" />
<input type="hidden" name="a_search" id="a_search" value="S" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
	<tr<%= role.RoleID.RowAttributes %>>
		<td class="ewTableHeader">Role ID</td>
		<td<%= role.RoleID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_RoleID" id="z_RoleID" value="=" /></span></td>
		<td<%= role.RoleID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_RoleID" id="x_RoleID" value="<%= role.RoleID.EditValue %>"<%= role.RoleID.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= role.RoleName.RowAttributes %>>
		<td class="ewTableHeader">Role Name</td>
		<td<%= role.RoleName.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_RoleName" id="z_RoleName" value="LIKE" /></span></td>
		<td<%= role.RoleName.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_RoleName" id="x_RoleName" size="30" maxlength="50" value="<%= role.RoleName.EditValue %>"<%= role.RoleName.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= role.RoleTitle.RowAttributes %>>
		<td class="ewTableHeader">Role Title</td>
		<td<%= role.RoleTitle.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_RoleTitle" id="z_RoleTitle" value="LIKE" /></span></td>
		<td<%= role.RoleTitle.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_RoleTitle" id="x_RoleTitle" size="30" maxlength="50" value="<%= role.RoleTitle.EditValue %>"<%= role.RoleTitle.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= role.RoleComment.RowAttributes %>>
		<td class="ewTableHeader">Role Comment</td>
		<td<%= role.RoleComment.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_RoleComment" id="z_RoleComment" value="LIKE" /></span></td>
		<td<%= role.RoleComment.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_RoleComment" id="x_RoleComment" size="30" maxlength="50" value="<%= role.RoleComment.EditValue %>"<%= role.RoleComment.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= role.FilterMenu.RowAttributes %>>
		<td class="ewTableHeader">Filter Menu</td>
		<td<%= role.FilterMenu.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_FilterMenu" id="z_FilterMenu" value="=" /></span></td>
		<td<%= role.FilterMenu.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<%
If ew_SameStr(role.FilterMenu.AdvancedSearch.SearchValue, "1") Then
	selwrk = " checked=""checked"""
Else
	selwrk = ""
End If
%>
<input type="checkbox" name="x_FilterMenu" id="x_FilterMenu" value="1"<%= selwrk %><%= role.FilterMenu.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
</table>
</div>
</td></tr></table>
<p />
<input type="submit" name="Action" id="Action" value="  Search  " />
<input type="button" name="Reset" id="Reset" value="   Reset   " onclick="ew_ClearForm(this.form);" />
</form>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
</asp:Content>
