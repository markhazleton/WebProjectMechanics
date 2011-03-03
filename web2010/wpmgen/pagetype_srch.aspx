<%@ Page ClassName="pagetype_srch" Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="pagetype_srch.aspx.vb" Inherits="pagetype_srch" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var PageType_search = new ew_Page("PageType_search");
// page properties
PageType_search.PageID = "search"; // page ID
PageType_search.FormID = "fPageTypesearch"; // form ID 
var EW_PAGE_ID = PageType_search.PageID; // for backward compatibility
// extend page with validate function for search
PageType_search.ValidateSearch = function(fobj) {
	ew_PostAutoSuggest(fobj); 
	if (this.ValidateRequired) { 
		var infix = "";
		elm = fobj.elements["x" + infix + "_PageTypeID"];
		if (elm && elm.type != "hidden" && !ew_CheckInteger(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(PageType.PageTypeID.FldErrMsg) %>");
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
PageType_search.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
PageType_search.ValidateRequired = true; // uses JavaScript validation
<% Else %>
PageType_search.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker"><%= Language.Phrase("Search") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= PageType.TableCaption %><br /><br />
<a href="<%= PageType.ReturnUrl %>"><%= Language.Phrase("BackToList") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(PageType_search.DebugMsg) %>
<% PageType_search.ShowMessage() %>
<form name="fPageTypesearch" id="fPageTypesearch" method="post" onsubmit="this.action=location.pathname;return PageType_search.ValidateSearch(this);">
<p />
<input type="hidden" name="t" id="t" value="PageType" />
<input type="hidden" name="a_search" id="a_search" value="S" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
	<tr<%= PageType.RowAttributes %>>
		<td class="ewTableHeader"><%= PageType.PageTypeID.FldCaption %></td>
		<td<%= PageType.PageTypeID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_PageTypeID" id="z_PageTypeID" value="=" /></span></td>
		<td<%= PageType.PageTypeID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_PageTypeID" id="x_PageTypeID" title="<%= PageType.PageTypeID.FldTitle %>" value="<%= PageType.PageTypeID.EditValue %>"<%= PageType.PageTypeID.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= PageType.RowAttributes %>>
		<td class="ewTableHeader"><%= PageType.PageTypeCD.FldCaption %></td>
		<td<%= PageType.PageTypeCD.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_PageTypeCD" id="z_PageTypeCD" value="LIKE" /></span></td>
		<td<%= PageType.PageTypeCD.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_PageTypeCD" id="x_PageTypeCD" title="<%= PageType.PageTypeCD.FldTitle %>" size="30" maxlength="50" value="<%= PageType.PageTypeCD.EditValue %>"<%= PageType.PageTypeCD.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= PageType.RowAttributes %>>
		<td class="ewTableHeader"><%= PageType.PageTypeDesc.FldCaption %></td>
		<td<%= PageType.PageTypeDesc.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_PageTypeDesc" id="z_PageTypeDesc" value="LIKE" /></span></td>
		<td<%= PageType.PageTypeDesc.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_PageTypeDesc" id="x_PageTypeDesc" title="<%= PageType.PageTypeDesc.FldTitle %>" size="30" maxlength="50" value="<%= PageType.PageTypeDesc.EditValue %>"<%= PageType.PageTypeDesc.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= PageType.RowAttributes %>>
		<td class="ewTableHeader"><%= PageType.PageFileName.FldCaption %></td>
		<td<%= PageType.PageFileName.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_PageFileName" id="z_PageFileName" value="LIKE" /></span></td>
		<td<%= PageType.PageFileName.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_PageFileName" id="x_PageFileName" title="<%= PageType.PageFileName.FldTitle %>" size="30" maxlength="50" value="<%= PageType.PageFileName.EditValue %>"<%= PageType.PageFileName.EditAttributes %> />
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
