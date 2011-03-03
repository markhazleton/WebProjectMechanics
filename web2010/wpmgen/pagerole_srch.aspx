<%@ Page ClassName="pagerole_srch" Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="pagerole_srch.aspx.vb" Inherits="pagerole_srch" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var PageRole_search = new ew_Page("PageRole_search");
// page properties
PageRole_search.PageID = "search"; // page ID
PageRole_search.FormID = "fPageRolesearch"; // form ID 
var EW_PAGE_ID = PageRole_search.PageID; // for backward compatibility
// extend page with validate function for search
PageRole_search.ValidateSearch = function(fobj) {
	ew_PostAutoSuggest(fobj); 
	if (this.ValidateRequired) { 
		var infix = "";
		elm = fobj.elements["x" + infix + "_PageRoleID"];
		if (elm && elm.type != "hidden" && !ew_CheckInteger(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(PageRole.PageRoleID.FldErrMsg) %>");
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
	for (var i=0;i<fobj.elements.length;i++) {
		var elem = fobj.elements[i];
		if (elem.name.substring(0,2) == "s_" || elem.name.substring(0,3) == "sv_")
			elem.value = "";
	}
	return true;
}
// extend page with Form_CustomValidate function
PageRole_search.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
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
// To include another .js script, you can use, e.g.
// ew_ClientScriptInclude("my_javascript.js"); or
// ew_ClientScriptInclude("my_javascript.js", {onSuccess: function(o) { // your code here }});
//-->
</script>
<p><span class="aspnetmaker"><%= Language.Phrase("Search") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= PageRole.TableCaption %><br /><br />
<a href="<%= PageRole.ReturnUrl %>"><%= Language.Phrase("BackToList") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(PageRole_search.DebugMsg) %>
<% PageRole_search.ShowMessage() %>
<form name="fPageRolesearch" id="fPageRolesearch" method="post" onsubmit="this.action=location.pathname;return PageRole_search.ValidateSearch(this);">
<p />
<input type="hidden" name="t" id="t" value="PageRole" />
<input type="hidden" name="a_search" id="a_search" value="S" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
	<tr<%= PageRole.RowAttributes %>>
		<td class="ewTableHeader"><%= PageRole.PageRoleID.FldCaption %></td>
		<td<%= PageRole.PageRoleID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_PageRoleID" id="z_PageRoleID" value="=" /></span></td>
		<td<%= PageRole.PageRoleID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_PageRoleID" id="x_PageRoleID" title="<%= PageRole.PageRoleID.FldTitle %>" value="<%= PageRole.PageRoleID.EditValue %>"<%= PageRole.PageRoleID.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= PageRole.RowAttributes %>>
		<td class="ewTableHeader"><%= PageRole.RoleID.FldCaption %></td>
		<td<%= PageRole.RoleID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_RoleID" id="z_RoleID" value="=" /></span></td>
		<td<%= PageRole.RoleID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_RoleID" id="x_RoleID" title="<%= PageRole.RoleID.FldTitle %>" size="30" value="<%= PageRole.RoleID.EditValue %>"<%= PageRole.RoleID.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= PageRole.RowAttributes %>>
		<td class="ewTableHeader"><%= PageRole.zPageID.FldCaption %></td>
		<td<%= PageRole.zPageID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_zPageID" id="z_zPageID" value="=" /></span></td>
		<td<%= PageRole.zPageID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_zPageID" id="x_zPageID" title="<%= PageRole.zPageID.FldTitle %>" size="30" value="<%= PageRole.zPageID.EditValue %>"<%= PageRole.zPageID.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= PageRole.RowAttributes %>>
		<td class="ewTableHeader"><%= PageRole.CompanyID.FldCaption %></td>
		<td<%= PageRole.CompanyID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_CompanyID" id="z_CompanyID" value="=" /></span></td>
		<td<%= PageRole.CompanyID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_CompanyID" id="x_CompanyID" title="<%= PageRole.CompanyID.FldTitle %>" size="30" value="<%= PageRole.CompanyID.EditValue %>"<%= PageRole.CompanyID.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
</table>
</div>
</td></tr></table>
<p />
<input type="submit" name="Action" id="Action" value="<%= ew_BtnCaption(Language.Phrase("Search")) %>" />
<input type="button" name="Reset" id="Reset" value="<%= ew_BtnCaption(Language.Phrase("Reset")) %>" onclick="ew_ClearForm(this.form);" />
</form>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
</asp:Content>
