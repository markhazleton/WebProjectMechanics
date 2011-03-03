<%@ Page ClassName="sitecategorytype_srch" Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="sitecategorytype_srch.aspx.vb" Inherits="sitecategorytype_srch" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var SiteCategoryType_search = new ew_Page("SiteCategoryType_search");
// page properties
SiteCategoryType_search.PageID = "search"; // page ID
SiteCategoryType_search.FormID = "fSiteCategoryTypesearch"; // form ID 
var EW_PAGE_ID = SiteCategoryType_search.PageID; // for backward compatibility
// extend page with validate function for search
SiteCategoryType_search.ValidateSearch = function(fobj) {
	ew_PostAutoSuggest(fobj); 
	if (this.ValidateRequired) { 
		var infix = "";
		elm = fobj.elements["x" + infix + "_SiteCategoryTypeID"];
		if (elm && elm.type != "hidden" && !ew_CheckInteger(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(SiteCategoryType.SiteCategoryTypeID.FldErrMsg) %>");
		elm = fobj.elements["x" + infix + "_DefaultSiteCategoryID"];
		if (elm && elm.type != "hidden" && !ew_CheckInteger(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(SiteCategoryType.DefaultSiteCategoryID.FldErrMsg) %>");
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
SiteCategoryType_search.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
SiteCategoryType_search.ValidateRequired = true; // uses JavaScript validation
<% Else %>
SiteCategoryType_search.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker"><%= Language.Phrase("Search") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= SiteCategoryType.TableCaption %><br /><br />
<a href="<%= SiteCategoryType.ReturnUrl %>"><%= Language.Phrase("BackToList") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(SiteCategoryType_search.DebugMsg) %>
<% SiteCategoryType_search.ShowMessage() %>
<form name="fSiteCategoryTypesearch" id="fSiteCategoryTypesearch" method="post" onsubmit="this.action=location.pathname;return SiteCategoryType_search.ValidateSearch(this);">
<p />
<input type="hidden" name="t" id="t" value="SiteCategoryType" />
<input type="hidden" name="a_search" id="a_search" value="S" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
	<tr<%= SiteCategoryType.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteCategoryType.SiteCategoryTypeID.FldCaption %></td>
		<td<%= SiteCategoryType.SiteCategoryTypeID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_SiteCategoryTypeID" id="z_SiteCategoryTypeID" value="=" /></span></td>
		<td<%= SiteCategoryType.SiteCategoryTypeID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_SiteCategoryTypeID" id="x_SiteCategoryTypeID" title="<%= SiteCategoryType.SiteCategoryTypeID.FldTitle %>" value="<%= SiteCategoryType.SiteCategoryTypeID.EditValue %>"<%= SiteCategoryType.SiteCategoryTypeID.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteCategoryType.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteCategoryType.SiteCategoryTypeNM.FldCaption %></td>
		<td<%= SiteCategoryType.SiteCategoryTypeNM.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_SiteCategoryTypeNM" id="z_SiteCategoryTypeNM" value="LIKE" /></span></td>
		<td<%= SiteCategoryType.SiteCategoryTypeNM.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_SiteCategoryTypeNM" id="x_SiteCategoryTypeNM" title="<%= SiteCategoryType.SiteCategoryTypeNM.FldTitle %>" size="30" maxlength="255" value="<%= SiteCategoryType.SiteCategoryTypeNM.EditValue %>"<%= SiteCategoryType.SiteCategoryTypeNM.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteCategoryType.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteCategoryType.SiteCategoryTypeDS.FldCaption %></td>
		<td<%= SiteCategoryType.SiteCategoryTypeDS.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_SiteCategoryTypeDS" id="z_SiteCategoryTypeDS" value="LIKE" /></span></td>
		<td<%= SiteCategoryType.SiteCategoryTypeDS.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_SiteCategoryTypeDS" id="x_SiteCategoryTypeDS" title="<%= SiteCategoryType.SiteCategoryTypeDS.FldTitle %>" size="30" maxlength="255" value="<%= SiteCategoryType.SiteCategoryTypeDS.EditValue %>"<%= SiteCategoryType.SiteCategoryTypeDS.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteCategoryType.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteCategoryType.SiteCategoryComment.FldCaption %></td>
		<td<%= SiteCategoryType.SiteCategoryComment.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_SiteCategoryComment" id="z_SiteCategoryComment" value="LIKE" /></span></td>
		<td<%= SiteCategoryType.SiteCategoryComment.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_SiteCategoryComment" id="x_SiteCategoryComment" title="<%= SiteCategoryType.SiteCategoryComment.FldTitle %>" size="30" maxlength="255" value="<%= SiteCategoryType.SiteCategoryComment.EditValue %>"<%= SiteCategoryType.SiteCategoryComment.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteCategoryType.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteCategoryType.SiteCategoryFileName.FldCaption %></td>
		<td<%= SiteCategoryType.SiteCategoryFileName.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_SiteCategoryFileName" id="z_SiteCategoryFileName" value="LIKE" /></span></td>
		<td<%= SiteCategoryType.SiteCategoryFileName.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_SiteCategoryFileName" id="x_SiteCategoryFileName" title="<%= SiteCategoryType.SiteCategoryFileName.FldTitle %>" size="30" maxlength="255" value="<%= SiteCategoryType.SiteCategoryFileName.EditValue %>"<%= SiteCategoryType.SiteCategoryFileName.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteCategoryType.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteCategoryType.SiteCategoryTransferURL.FldCaption %></td>
		<td<%= SiteCategoryType.SiteCategoryTransferURL.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_SiteCategoryTransferURL" id="z_SiteCategoryTransferURL" value="LIKE" /></span></td>
		<td<%= SiteCategoryType.SiteCategoryTransferURL.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_SiteCategoryTransferURL" id="x_SiteCategoryTransferURL" title="<%= SiteCategoryType.SiteCategoryTransferURL.FldTitle %>" size="30" maxlength="255" value="<%= SiteCategoryType.SiteCategoryTransferURL.EditValue %>"<%= SiteCategoryType.SiteCategoryTransferURL.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteCategoryType.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteCategoryType.DefaultSiteCategoryID.FldCaption %></td>
		<td<%= SiteCategoryType.DefaultSiteCategoryID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_DefaultSiteCategoryID" id="z_DefaultSiteCategoryID" value="=" /></span></td>
		<td<%= SiteCategoryType.DefaultSiteCategoryID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_DefaultSiteCategoryID" id="x_DefaultSiteCategoryID" title="<%= SiteCategoryType.DefaultSiteCategoryID.FldTitle %>" size="30" value="<%= SiteCategoryType.DefaultSiteCategoryID.EditValue %>"<%= SiteCategoryType.DefaultSiteCategoryID.EditAttributes %> />
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
