<%@ Page ClassName="siteparametertype_srch" Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="siteparametertype_srch.aspx.vb" Inherits="siteparametertype_srch" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var SiteParameterType_search = new ew_Page("SiteParameterType_search");
// page properties
SiteParameterType_search.PageID = "search"; // page ID
SiteParameterType_search.FormID = "fSiteParameterTypesearch"; // form ID 
var EW_PAGE_ID = SiteParameterType_search.PageID; // for backward compatibility
// extend page with validate function for search
SiteParameterType_search.ValidateSearch = function(fobj) {
	ew_PostAutoSuggest(fobj); 
	if (this.ValidateRequired) { 
		var infix = "";
		elm = fobj.elements["x" + infix + "_SiteParameterTypeOrder"];
		if (elm && elm.type != "hidden" && !ew_CheckInteger(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(SiteParameterType.SiteParameterTypeOrder.FldErrMsg) %>");
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
SiteParameterType_search.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
SiteParameterType_search.ValidateRequired = true; // uses JavaScript validation
<% Else %>
SiteParameterType_search.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker"><%= Language.Phrase("Search") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= SiteParameterType.TableCaption %><br /><br />
<a href="<%= SiteParameterType.ReturnUrl %>"><%= Language.Phrase("BackToList") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(SiteParameterType_search.DebugMsg) %>
<% SiteParameterType_search.ShowMessage() %>
<form name="fSiteParameterTypesearch" id="fSiteParameterTypesearch" method="post" onsubmit="this.action=location.pathname;return SiteParameterType_search.ValidateSearch(this);">
<p />
<input type="hidden" name="t" id="t" value="SiteParameterType" />
<input type="hidden" name="a_search" id="a_search" value="S" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
	<tr<%= SiteParameterType.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteParameterType.SiteParameterTypeNM.FldCaption %></td>
		<td<%= SiteParameterType.SiteParameterTypeNM.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_SiteParameterTypeNM" id="z_SiteParameterTypeNM" value="LIKE" /></span></td>
		<td<%= SiteParameterType.SiteParameterTypeNM.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_SiteParameterTypeNM" id="x_SiteParameterTypeNM" title="<%= SiteParameterType.SiteParameterTypeNM.FldTitle %>" size="30" maxlength="255" value="<%= SiteParameterType.SiteParameterTypeNM.EditValue %>"<%= SiteParameterType.SiteParameterTypeNM.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteParameterType.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteParameterType.SiteParameterTypeDS.FldCaption %></td>
		<td<%= SiteParameterType.SiteParameterTypeDS.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_SiteParameterTypeDS" id="z_SiteParameterTypeDS" value="LIKE" /></span></td>
		<td<%= SiteParameterType.SiteParameterTypeDS.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_SiteParameterTypeDS" id="x_SiteParameterTypeDS" title="<%= SiteParameterType.SiteParameterTypeDS.FldTitle %>" size="30" maxlength="255" value="<%= SiteParameterType.SiteParameterTypeDS.EditValue %>"<%= SiteParameterType.SiteParameterTypeDS.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteParameterType.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteParameterType.SiteParameterTypeOrder.FldCaption %></td>
		<td<%= SiteParameterType.SiteParameterTypeOrder.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_SiteParameterTypeOrder" id="z_SiteParameterTypeOrder" value="=" /></span></td>
		<td<%= SiteParameterType.SiteParameterTypeOrder.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_SiteParameterTypeOrder" id="x_SiteParameterTypeOrder" title="<%= SiteParameterType.SiteParameterTypeOrder.FldTitle %>" size="30" value="<%= SiteParameterType.SiteParameterTypeOrder.EditValue %>"<%= SiteParameterType.SiteParameterTypeOrder.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteParameterType.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteParameterType.SiteParameterTemplate.FldCaption %></td>
		<td<%= SiteParameterType.SiteParameterTemplate.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_SiteParameterTemplate" id="z_SiteParameterTemplate" value="LIKE" /></span></td>
		<td<%= SiteParameterType.SiteParameterTemplate.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<textarea name="x_SiteParameterTemplate" id="x_SiteParameterTemplate" title="<%= SiteParameterType.SiteParameterTemplate.FldTitle %>" cols="70" rows="10"<%= SiteParameterType.SiteParameterTemplate.EditAttributes %>><%= SiteParameterType.SiteParameterTemplate.EditValue %></textarea>
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
