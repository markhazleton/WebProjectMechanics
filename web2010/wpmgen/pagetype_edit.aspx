<%@ Page ClassName="pagetype_edit" Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="pagetype_edit.aspx.vb" Inherits="pagetype_edit" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var PageType_edit = new ew_Page("PageType_edit");
// page properties
PageType_edit.PageID = "edit"; // page ID
PageType_edit.FormID = "fPageTypeedit"; // form ID 
var EW_PAGE_ID = PageType_edit.PageID; // for backward compatibility
// extend page with ValidateForm function
PageType_edit.ValidateForm = function(fobj) {
	ew_PostAutoSuggest(fobj); 
	if (!this.ValidateRequired)
		return true; // ignore validation
	if (fobj.a_confirm && fobj.a_confirm.value == "F")
		return true;
	var i, elm, aelm, infix;
	var rowcnt = (fobj.key_count) ? Number(fobj.key_count.value) : 1;
	for (i=0; i<rowcnt; i++) {
		infix = (fobj.key_count) ? String(i+1) : "";
		// Form Custom Validate event
		if (!this.Form_CustomValidate(fobj)) return false;
	}
	return true;
}
// extend page with Form_CustomValidate function
PageType_edit.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
PageType_edit.ValidateRequired = true; // uses JavaScript validation
<% Else %>
PageType_edit.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker"><%= Language.Phrase("Edit") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= PageType.TableCaption %><br /><br />
<a href="<%= PageType.ReturnUrl %>"><%= Language.Phrase("GoBack") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(PageType_edit.DebugMsg) %>
<% PageType_edit.ShowMessage() %>
<form name="fPageTypeedit" id="fPageTypeedit" method="post" onsubmit="this.action=location.pathname;return PageType_edit.ValidateForm(this);">
<p />
<input type="hidden" name="a_table" id="a_table" value="PageType" />
<input type="hidden" name="a_edit" id="a_edit" value="U" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
<% If PageType.PageTypeID.Visible Then ' PageTypeID %>
	<tr<%= PageType.RowAttributes %>>
		<td class="ewTableHeader"><%= PageType.PageTypeID.FldCaption %></td>
		<td<%= PageType.PageTypeID.CellAttributes %>><span id="el_PageTypeID">
<div<%= PageType.PageTypeID.ViewAttributes %>><%= PageType.PageTypeID.EditValue %></div>
<input type="hidden" name="x_PageTypeID" id="x_PageTypeID" value="<%= ew_HTMLEncode(PageType.PageTypeID.CurrentValue) %>" />
</span><%= PageType.PageTypeID.CustomMsg %></td>
	</tr>
<% End If %>
<% If PageType.PageTypeCD.Visible Then ' PageTypeCD %>
	<tr<%= PageType.RowAttributes %>>
		<td class="ewTableHeader"><%= PageType.PageTypeCD.FldCaption %></td>
		<td<%= PageType.PageTypeCD.CellAttributes %>><span id="el_PageTypeCD">
<input type="text" name="x_PageTypeCD" id="x_PageTypeCD" title="<%= PageType.PageTypeCD.FldTitle %>" size="30" maxlength="50" value="<%= PageType.PageTypeCD.EditValue %>"<%= PageType.PageTypeCD.EditAttributes %> />
</span><%= PageType.PageTypeCD.CustomMsg %></td>
	</tr>
<% End If %>
<% If PageType.PageTypeDesc.Visible Then ' PageTypeDesc %>
	<tr<%= PageType.RowAttributes %>>
		<td class="ewTableHeader"><%= PageType.PageTypeDesc.FldCaption %></td>
		<td<%= PageType.PageTypeDesc.CellAttributes %>><span id="el_PageTypeDesc">
<input type="text" name="x_PageTypeDesc" id="x_PageTypeDesc" title="<%= PageType.PageTypeDesc.FldTitle %>" size="30" maxlength="50" value="<%= PageType.PageTypeDesc.EditValue %>"<%= PageType.PageTypeDesc.EditAttributes %> />
</span><%= PageType.PageTypeDesc.CustomMsg %></td>
	</tr>
<% End If %>
<% If PageType.PageFileName.Visible Then ' PageFileName %>
	<tr<%= PageType.RowAttributes %>>
		<td class="ewTableHeader"><%= PageType.PageFileName.FldCaption %></td>
		<td<%= PageType.PageFileName.CellAttributes %>><span id="el_PageFileName">
<input type="text" name="x_PageFileName" id="x_PageFileName" title="<%= PageType.PageFileName.FldTitle %>" size="30" maxlength="50" value="<%= PageType.PageFileName.EditValue %>"<%= PageType.PageFileName.EditAttributes %> />
</span><%= PageType.PageFileName.CustomMsg %></td>
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
