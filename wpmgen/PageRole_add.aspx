<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="PageRole_add.aspx.vb" Inherits="PageRole_add" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var PageRole_add = new ew_Page("PageRole_add");
// page properties
PageRole_add.PageID = "add"; // page ID
var EW_PAGE_ID = PageRole_add.PageID; // for backward compatibility
// extend page with ValidateForm function
PageRole_add.ValidateForm = function(fobj) {
	if (!this.ValidateRequired)
		return true; // ignore validation
	if (fobj.a_confirm && fobj.a_confirm.value == "F")
		return true;
	var i, elm, aelm, infix;
	var rowcnt = (fobj.key_count) ? Number(fobj.key_count.value) : 1;
	for (i=0; i<rowcnt; i++) {
		infix = (fobj.key_count) ? String(i+1) : "";
		elm = fobj.elements["x" + infix + "_RoleID"];
		if (elm && !ew_CheckInteger(elm.value))
			return ew_OnError(this, elm, "Incorrect integer - Role ID");
		elm = fobj.elements["x" + infix + "_zPageID"];
		if (elm && !ew_CheckInteger(elm.value))
			return ew_OnError(this, elm, "Incorrect integer - Page ID");
		elm = fobj.elements["x" + infix + "_CompanyID"];
		if (elm && !ew_CheckInteger(elm.value))
			return ew_OnError(this, elm, "Incorrect integer - Company ID");
	}
	return true;
}
<% If EW_CLIENT_VALIDATE Then %>
PageRole_add.ValidateRequired = true; // uses JavaScript validation
<% Else %>
PageRole_add.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">Add to TABLE: Location Role<br /><br />
<a href="<%= PageRole.ReturnUrl %>">Go Back</a></span></p>
<% PageRole_add.ShowMessage() %>
<form name="fPageRoleadd" id="fPageRoleadd" method="post" onsubmit="this.action=location.pathname;return PageRole_add.ValidateForm(this);">
<p />
<input type="hidden" name="t" id="t" value="PageRole" />
<input type="hidden" name="a_add" id="a_add" value="A" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
<% If PageRole.RoleID.Visible Then ' RoleID %>
	<tr<%= PageRole.RoleID.RowAttributes %>>
		<td class="ewTableHeader">Role ID</td>
		<td<%= PageRole.RoleID.CellAttributes %>><span id="el_RoleID">
<input type="text" name="x_RoleID" id="x_RoleID" size="30" value="<%= PageRole.RoleID.EditValue %>"<%= PageRole.RoleID.EditAttributes %> />
</span><%= PageRole.RoleID.CustomMsg %></td>
	</tr>
<% End If %>
<% If PageRole.zPageID.Visible Then ' PageID %>
	<tr<%= PageRole.zPageID.RowAttributes %>>
		<td class="ewTableHeader">Page ID</td>
		<td<%= PageRole.zPageID.CellAttributes %>><span id="el_zPageID">
<input type="text" name="x_zPageID" id="x_zPageID" size="30" value="<%= PageRole.zPageID.EditValue %>"<%= PageRole.zPageID.EditAttributes %> />
</span><%= PageRole.zPageID.CustomMsg %></td>
	</tr>
<% End If %>
<% If PageRole.CompanyID.Visible Then ' CompanyID %>
	<tr<%= PageRole.CompanyID.RowAttributes %>>
		<td class="ewTableHeader">Company ID</td>
		<td<%= PageRole.CompanyID.CellAttributes %>><span id="el_CompanyID">
<input type="text" name="x_CompanyID" id="x_CompanyID" size="30" value="<%= PageRole.CompanyID.EditValue %>"<%= PageRole.CompanyID.EditAttributes %> />
</span><%= PageRole.CompanyID.CustomMsg %></td>
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
