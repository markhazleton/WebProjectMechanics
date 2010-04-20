<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="PageRole_srch.aspx.vb" Inherits="PageRole_srch" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var PageRole_search = new ew_Page("PageRole_search");
// page properties
PageRole_search.PageID = "search"; // page ID
var EW_PAGE_ID = PageRole_search.PageID; // for backward compatibility
// extend page with validate function for search
PageRole_search.ValidateSearch = function(fobj) {
	if (!this.ValidateRequired)
		return true; // ignore validation
	var infix = "";
	elm = fobj.elements["x" + infix + "_PageRoleID"];
	if (elm && !ew_CheckInteger(elm.value))
		return ew_OnError(this, elm, "Incorrect integer - Page Role ID");
	elm = fobj.elements["x" + infix + "_RoleID"];
	if (elm && !ew_CheckInteger(elm.value))
		return ew_OnError(this, elm, "Incorrect integer - Role ID");
	elm = fobj.elements["x" + infix + "_zPageID"];
	if (elm && !ew_CheckInteger(elm.value))
		return ew_OnError(this, elm, "Incorrect integer - Page ID");
	elm = fobj.elements["x" + infix + "_CompanyID"];
	if (elm && !ew_CheckInteger(elm.value))
		return ew_OnError(this, elm, "Incorrect integer - Company ID");
	for (var i=0;i<fobj.elements.length;i++) {
		var elem = fobj.elements[i];
		if (elem.name.substring(0,2) == "s_" || elem.name.substring(0,3) == "sv_")
			elem.value = "";
	}
	return true;
}
<% If EW_CLIENT_VALIDATE Then %>
PageRole_search.ValidateRequired = true; // uses JavaScript validation
<% Else %>
PageRole_search.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">Search TABLE: Location Role<br /><br />
<a href="<%= PageRole.ReturnUrl %>">Back to List</a></span></p>
<% PageRole_search.ShowMessage() %>
<form name="fPageRolesearch" id="fPageRolesearch" method="post" onsubmit="this.action=location.pathname;return PageRole_search.ValidateSearch(this);">
<p />
<input type="hidden" name="t" id="t" value="PageRole" />
<input type="hidden" name="a_search" id="a_search" value="S" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
	<tr<%= PageRole.PageRoleID.RowAttributes %>>
		<td class="ewTableHeader">Page Role ID</td>
		<td<%= PageRole.PageRoleID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_PageRoleID" id="z_PageRoleID" value="=" /></span></td>
		<td<%= PageRole.PageRoleID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_PageRoleID" id="x_PageRoleID" value="<%= PageRole.PageRoleID.EditValue %>"<%= PageRole.PageRoleID.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= PageRole.RoleID.RowAttributes %>>
		<td class="ewTableHeader">Role ID</td>
		<td<%= PageRole.RoleID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_RoleID" id="z_RoleID" value="=" /></span></td>
		<td<%= PageRole.RoleID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_RoleID" id="x_RoleID" size="30" value="<%= PageRole.RoleID.EditValue %>"<%= PageRole.RoleID.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= PageRole.zPageID.RowAttributes %>>
		<td class="ewTableHeader">Page ID</td>
		<td<%= PageRole.zPageID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_zPageID" id="z_zPageID" value="=" /></span></td>
		<td<%= PageRole.zPageID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_zPageID" id="x_zPageID" size="30" value="<%= PageRole.zPageID.EditValue %>"<%= PageRole.zPageID.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= PageRole.CompanyID.RowAttributes %>>
		<td class="ewTableHeader">Company ID</td>
		<td<%= PageRole.CompanyID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_CompanyID" id="z_CompanyID" value="=" /></span></td>
		<td<%= PageRole.CompanyID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_CompanyID" id="x_CompanyID" size="30" value="<%= PageRole.CompanyID.EditValue %>"<%= PageRole.CompanyID.EditAttributes %> />
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
