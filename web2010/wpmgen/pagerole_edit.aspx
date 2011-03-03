<%@ Page ClassName="pagerole_edit" Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="pagerole_edit.aspx.vb" Inherits="pagerole_edit" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var PageRole_edit = new ew_Page("PageRole_edit");
// page properties
PageRole_edit.PageID = "edit"; // page ID
PageRole_edit.FormID = "fPageRoleedit"; // form ID 
var EW_PAGE_ID = PageRole_edit.PageID; // for backward compatibility
// extend page with ValidateForm function
PageRole_edit.ValidateForm = function(fobj) {
	ew_PostAutoSuggest(fobj); 
	if (!this.ValidateRequired)
		return true; // ignore validation
	if (fobj.a_confirm && fobj.a_confirm.value == "F")
		return true;
	var i, elm, aelm, infix;
	var rowcnt = (fobj.key_count) ? Number(fobj.key_count.value) : 1;
	for (i=0; i<rowcnt; i++) {
		infix = (fobj.key_count) ? String(i+1) : "";
		elm = fobj.elements["x" + infix + "_RoleID"];
		if (elm && elm.type != "hidden" && !ew_CheckInteger(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(PageRole.RoleID.FldErrMsg) %>");
		elm = fobj.elements["x" + infix + "_zPageID"];
		if (elm && elm.type != "hidden" && !ew_CheckInteger(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(PageRole.zPageID.FldErrMsg) %>");
		elm = fobj.elements["x" + infix + "_CompanyID"];
		if (elm && elm.type != "hidden" && !ew_CheckInteger(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(PageRole.CompanyID.FldErrMsg) %>");
		// Form Custom Validate event
		if (!this.Form_CustomValidate(fobj)) return false;
	}
	return true;
}
// extend page with Form_CustomValidate function
PageRole_edit.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
PageRole_edit.ValidateRequired = true; // uses JavaScript validation
<% Else %>
PageRole_edit.ValidateRequired = false; // no JavaScript validation
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
// To include another .js script, you can use, e.g.
// ew_ClientScriptInclude("my_javascript.js"); or
// ew_ClientScriptInclude("my_javascript.js", {onSuccess: function(o) { // your code here }});
//-->
</script>
<p><span class="aspnetmaker"><%= Language.Phrase("Edit") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= PageRole.TableCaption %><br /><br />
<a href="<%= PageRole.ReturnUrl %>"><%= Language.Phrase("GoBack") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(PageRole_edit.DebugMsg) %>
<% PageRole_edit.ShowMessage() %>
<form name="fPageRoleedit" id="fPageRoleedit" method="post" onsubmit="this.action=location.pathname;return PageRole_edit.ValidateForm(this);">
<p />
<input type="hidden" name="a_table" id="a_table" value="PageRole" />
<input type="hidden" name="a_edit" id="a_edit" value="U" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
<% If PageRole.PageRoleID.Visible Then ' PageRoleID %>
	<tr<%= PageRole.RowAttributes %>>
		<td class="ewTableHeader"><%= PageRole.PageRoleID.FldCaption %></td>
		<td<%= PageRole.PageRoleID.CellAttributes %>><span id="el_PageRoleID">
<div<%= PageRole.PageRoleID.ViewAttributes %>><%= PageRole.PageRoleID.EditValue %></div>
<input type="hidden" name="x_PageRoleID" id="x_PageRoleID" value="<%= ew_HTMLEncode(PageRole.PageRoleID.CurrentValue) %>" />
</span><%= PageRole.PageRoleID.CustomMsg %></td>
	</tr>
<% End If %>
<% If PageRole.RoleID.Visible Then ' RoleID %>
	<tr<%= PageRole.RowAttributes %>>
		<td class="ewTableHeader"><%= PageRole.RoleID.FldCaption %></td>
		<td<%= PageRole.RoleID.CellAttributes %>><span id="el_RoleID">
<input type="text" name="x_RoleID" id="x_RoleID" title="<%= PageRole.RoleID.FldTitle %>" size="30" value="<%= PageRole.RoleID.EditValue %>"<%= PageRole.RoleID.EditAttributes %> />
</span><%= PageRole.RoleID.CustomMsg %></td>
	</tr>
<% End If %>
<% If PageRole.zPageID.Visible Then ' PageID %>
	<tr<%= PageRole.RowAttributes %>>
		<td class="ewTableHeader"><%= PageRole.zPageID.FldCaption %></td>
		<td<%= PageRole.zPageID.CellAttributes %>><span id="el_zPageID">
<input type="text" name="x_zPageID" id="x_zPageID" title="<%= PageRole.zPageID.FldTitle %>" size="30" value="<%= PageRole.zPageID.EditValue %>"<%= PageRole.zPageID.EditAttributes %> />
</span><%= PageRole.zPageID.CustomMsg %></td>
	</tr>
<% End If %>
<% If PageRole.CompanyID.Visible Then ' CompanyID %>
	<tr<%= PageRole.RowAttributes %>>
		<td class="ewTableHeader"><%= PageRole.CompanyID.FldCaption %></td>
		<td<%= PageRole.CompanyID.CellAttributes %>><span id="el_CompanyID">
<input type="text" name="x_CompanyID" id="x_CompanyID" title="<%= PageRole.CompanyID.FldTitle %>" size="30" value="<%= PageRole.CompanyID.EditValue %>"<%= PageRole.CompanyID.EditAttributes %> />
</span><%= PageRole.CompanyID.CustomMsg %></td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<p />
<input type="submit" name="btnAction" id="btnAction" value="<%= ew_BtnCaption(Language.Phrase("EditBtn")) %>" />
</form>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
</asp:Content>
