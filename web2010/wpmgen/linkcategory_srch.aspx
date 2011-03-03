<%@ Page ClassName="linkcategory_srch" Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="linkcategory_srch.aspx.vb" Inherits="linkcategory_srch" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var LinkCategory_search = new ew_Page("LinkCategory_search");
// page properties
LinkCategory_search.PageID = "search"; // page ID
LinkCategory_search.FormID = "fLinkCategorysearch"; // form ID 
var EW_PAGE_ID = LinkCategory_search.PageID; // for backward compatibility
// extend page with validate function for search
LinkCategory_search.ValidateSearch = function(fobj) {
	ew_PostAutoSuggest(fobj); 
	if (this.ValidateRequired) { 
		var infix = "";
		elm = fobj.elements["x" + infix + "_ID"];
		if (elm && elm.type != "hidden" && !ew_CheckInteger(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(LinkCategory.ID.FldErrMsg) %>");
		elm = fobj.elements["x" + infix + "_ParentID"];
		if (elm && elm.type != "hidden" && !ew_CheckInteger(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(LinkCategory.ParentID.FldErrMsg) %>");
		elm = fobj.elements["x" + infix + "_zPageID"];
		if (elm && elm.type != "hidden" && !ew_CheckInteger(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(LinkCategory.zPageID.FldErrMsg) %>");
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
LinkCategory_search.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
LinkCategory_search.ValidateRequired = true; // uses JavaScript validation
<% Else %>
LinkCategory_search.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker"><%= Language.Phrase("Search") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= LinkCategory.TableCaption %><br /><br />
<a href="<%= LinkCategory.ReturnUrl %>"><%= Language.Phrase("BackToList") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(LinkCategory_search.DebugMsg) %>
<% LinkCategory_search.ShowMessage() %>
<form name="fLinkCategorysearch" id="fLinkCategorysearch" method="post" onsubmit="this.action=location.pathname;return LinkCategory_search.ValidateSearch(this);">
<p />
<input type="hidden" name="t" id="t" value="LinkCategory" />
<input type="hidden" name="a_search" id="a_search" value="S" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
	<tr<%= LinkCategory.RowAttributes %>>
		<td class="ewTableHeader"><%= LinkCategory.ID.FldCaption %></td>
		<td<%= LinkCategory.ID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_ID" id="z_ID" value="=" /></span></td>
		<td<%= LinkCategory.ID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_ID" id="x_ID" title="<%= LinkCategory.ID.FldTitle %>" value="<%= LinkCategory.ID.EditValue %>"<%= LinkCategory.ID.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= LinkCategory.RowAttributes %>>
		<td class="ewTableHeader"><%= LinkCategory.Title.FldCaption %></td>
		<td<%= LinkCategory.Title.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_Title" id="z_Title" value="LIKE" /></span></td>
		<td<%= LinkCategory.Title.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_Title" id="x_Title" title="<%= LinkCategory.Title.FldTitle %>" size="30" maxlength="50" value="<%= LinkCategory.Title.EditValue %>"<%= LinkCategory.Title.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= LinkCategory.RowAttributes %>>
		<td class="ewTableHeader"><%= LinkCategory.Description.FldCaption %></td>
		<td<%= LinkCategory.Description.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_Description" id="z_Description" value="LIKE" /></span></td>
		<td<%= LinkCategory.Description.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<textarea name="x_Description" id="x_Description" title="<%= LinkCategory.Description.FldTitle %>" cols="35" rows="4"<%= LinkCategory.Description.EditAttributes %>><%= LinkCategory.Description.EditValue %></textarea>
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= LinkCategory.RowAttributes %>>
		<td class="ewTableHeader"><%= LinkCategory.ParentID.FldCaption %></td>
		<td<%= LinkCategory.ParentID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_ParentID" id="z_ParentID" value="=" /></span></td>
		<td<%= LinkCategory.ParentID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_ParentID" id="x_ParentID" title="<%= LinkCategory.ParentID.FldTitle %>" size="30" value="<%= LinkCategory.ParentID.EditValue %>"<%= LinkCategory.ParentID.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= LinkCategory.RowAttributes %>>
		<td class="ewTableHeader"><%= LinkCategory.zPageID.FldCaption %></td>
		<td<%= LinkCategory.zPageID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_zPageID" id="z_zPageID" value="=" /></span></td>
		<td<%= LinkCategory.zPageID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_zPageID" id="x_zPageID" title="<%= LinkCategory.zPageID.FldTitle %>" size="30" value="<%= LinkCategory.zPageID.EditValue %>"<%= LinkCategory.zPageID.EditAttributes %> />
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
