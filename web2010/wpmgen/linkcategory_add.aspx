<%@ Page ClassName="linkcategory_add" Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="linkcategory_add.aspx.vb" Inherits="linkcategory_add" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var LinkCategory_add = new ew_Page("LinkCategory_add");
// page properties
LinkCategory_add.PageID = "add"; // page ID
LinkCategory_add.FormID = "fLinkCategoryadd"; // form ID 
var EW_PAGE_ID = LinkCategory_add.PageID; // for backward compatibility
// extend page with ValidateForm function
LinkCategory_add.ValidateForm = function(fobj) {
	ew_PostAutoSuggest(fobj); 
	if (!this.ValidateRequired)
		return true; // ignore validation
	if (fobj.a_confirm && fobj.a_confirm.value == "F")
		return true;
	var i, elm, aelm, infix;
	var rowcnt = (fobj.key_count) ? Number(fobj.key_count.value) : 1;
	for (i=0; i<rowcnt; i++) {
		infix = (fobj.key_count) ? String(i+1) : "";
		elm = fobj.elements["x" + infix + "_ParentID"];
		if (elm && elm.type != "hidden" && !ew_CheckInteger(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(LinkCategory.ParentID.FldErrMsg) %>");
		elm = fobj.elements["x" + infix + "_zPageID"];
		if (elm && elm.type != "hidden" && !ew_CheckInteger(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(LinkCategory.zPageID.FldErrMsg) %>");
		// Form Custom Validate event
		if (!this.Form_CustomValidate(fobj)) return false;
	}
	return true;
}
// extend page with Form_CustomValidate function
LinkCategory_add.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
LinkCategory_add.ValidateRequired = true; // uses JavaScript validation
<% Else %>
LinkCategory_add.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker"><%= Language.Phrase("Add") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= LinkCategory.TableCaption %><br /><br />
<a href="<%= LinkCategory.ReturnUrl %>"><%= Language.Phrase("GoBack") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(LinkCategory_add.DebugMsg) %>
<% LinkCategory_add.ShowMessage() %>
<form name="fLinkCategoryadd" id="fLinkCategoryadd" method="post" onsubmit="this.action=location.pathname;return LinkCategory_add.ValidateForm(this);">
<p />
<input type="hidden" name="t" id="t" value="LinkCategory" />
<input type="hidden" name="a_add" id="a_add" value="A" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
<% If LinkCategory.Title.Visible Then ' Title %>
	<tr<%= LinkCategory.RowAttributes %>>
		<td class="ewTableHeader"><%= LinkCategory.Title.FldCaption %></td>
		<td<%= LinkCategory.Title.CellAttributes %>><span id="el_Title">
<input type="text" name="x_Title" id="x_Title" title="<%= LinkCategory.Title.FldTitle %>" size="30" maxlength="50" value="<%= LinkCategory.Title.EditValue %>"<%= LinkCategory.Title.EditAttributes %> />
</span><%= LinkCategory.Title.CustomMsg %></td>
	</tr>
<% End If %>
<% If LinkCategory.Description.Visible Then ' Description %>
	<tr<%= LinkCategory.RowAttributes %>>
		<td class="ewTableHeader"><%= LinkCategory.Description.FldCaption %></td>
		<td<%= LinkCategory.Description.CellAttributes %>><span id="el_Description">
<textarea name="x_Description" id="x_Description" title="<%= LinkCategory.Description.FldTitle %>" cols="35" rows="4"<%= LinkCategory.Description.EditAttributes %>><%= LinkCategory.Description.EditValue %></textarea>
</span><%= LinkCategory.Description.CustomMsg %></td>
	</tr>
<% End If %>
<% If LinkCategory.ParentID.Visible Then ' ParentID %>
	<tr<%= LinkCategory.RowAttributes %>>
		<td class="ewTableHeader"><%= LinkCategory.ParentID.FldCaption %></td>
		<td<%= LinkCategory.ParentID.CellAttributes %>><span id="el_ParentID">
<input type="text" name="x_ParentID" id="x_ParentID" title="<%= LinkCategory.ParentID.FldTitle %>" size="30" value="<%= LinkCategory.ParentID.EditValue %>"<%= LinkCategory.ParentID.EditAttributes %> />
</span><%= LinkCategory.ParentID.CustomMsg %></td>
	</tr>
<% End If %>
<% If LinkCategory.zPageID.Visible Then ' PageID %>
	<tr<%= LinkCategory.RowAttributes %>>
		<td class="ewTableHeader"><%= LinkCategory.zPageID.FldCaption %></td>
		<td<%= LinkCategory.zPageID.CellAttributes %>><span id="el_zPageID">
<input type="text" name="x_zPageID" id="x_zPageID" title="<%= LinkCategory.zPageID.FldTitle %>" size="30" value="<%= LinkCategory.zPageID.EditValue %>"<%= LinkCategory.zPageID.EditAttributes %> />
</span><%= LinkCategory.zPageID.CustomMsg %></td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<p />
<input type="submit" name="btnAction" id="btnAction" value="<%= ew_BtnCaption(Language.Phrase("AddBtn")) %>" />
</form>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
</asp:Content>
