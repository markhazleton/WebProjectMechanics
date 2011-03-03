<%@ Page ClassName="siteparametertype_edit" Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="siteparametertype_edit.aspx.vb" Inherits="siteparametertype_edit" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var SiteParameterType_edit = new ew_Page("SiteParameterType_edit");
// page properties
SiteParameterType_edit.PageID = "edit"; // page ID
SiteParameterType_edit.FormID = "fSiteParameterTypeedit"; // form ID 
var EW_PAGE_ID = SiteParameterType_edit.PageID; // for backward compatibility
// extend page with ValidateForm function
SiteParameterType_edit.ValidateForm = function(fobj) {
	ew_PostAutoSuggest(fobj); 
	if (!this.ValidateRequired)
		return true; // ignore validation
	if (fobj.a_confirm && fobj.a_confirm.value == "F")
		return true;
	var i, elm, aelm, infix;
	var rowcnt = (fobj.key_count) ? Number(fobj.key_count.value) : 1;
	for (i=0; i<rowcnt; i++) {
		infix = (fobj.key_count) ? String(i+1) : "";
		elm = fobj.elements["x" + infix + "_SiteParameterTypeOrder"];
		if (elm && elm.type != "hidden" && !ew_CheckInteger(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(SiteParameterType.SiteParameterTypeOrder.FldErrMsg) %>");
		// Form Custom Validate event
		if (!this.Form_CustomValidate(fobj)) return false;
	}
	return true;
}
// extend page with Form_CustomValidate function
SiteParameterType_edit.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
SiteParameterType_edit.ValidateRequired = true; // uses JavaScript validation
<% Else %>
SiteParameterType_edit.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker"><%= Language.Phrase("Edit") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= SiteParameterType.TableCaption %><br /><br />
<a href="<%= SiteParameterType.ReturnUrl %>"><%= Language.Phrase("GoBack") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(SiteParameterType_edit.DebugMsg) %>
<% SiteParameterType_edit.ShowMessage() %>
<form name="fSiteParameterTypeedit" id="fSiteParameterTypeedit" method="post" onsubmit="this.action=location.pathname;return SiteParameterType_edit.ValidateForm(this);">
<p />
<input type="hidden" name="a_table" id="a_table" value="SiteParameterType" />
<input type="hidden" name="a_edit" id="a_edit" value="U" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
<% If SiteParameterType.SiteParameterTypeNM.Visible Then ' SiteParameterTypeNM %>
	<tr<%= SiteParameterType.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteParameterType.SiteParameterTypeNM.FldCaption %></td>
		<td<%= SiteParameterType.SiteParameterTypeNM.CellAttributes %>><span id="el_SiteParameterTypeNM">
<input type="text" name="x_SiteParameterTypeNM" id="x_SiteParameterTypeNM" title="<%= SiteParameterType.SiteParameterTypeNM.FldTitle %>" size="30" maxlength="255" value="<%= SiteParameterType.SiteParameterTypeNM.EditValue %>"<%= SiteParameterType.SiteParameterTypeNM.EditAttributes %> />
</span><%= SiteParameterType.SiteParameterTypeNM.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteParameterType.SiteParameterTypeDS.Visible Then ' SiteParameterTypeDS %>
	<tr<%= SiteParameterType.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteParameterType.SiteParameterTypeDS.FldCaption %></td>
		<td<%= SiteParameterType.SiteParameterTypeDS.CellAttributes %>><span id="el_SiteParameterTypeDS">
<input type="text" name="x_SiteParameterTypeDS" id="x_SiteParameterTypeDS" title="<%= SiteParameterType.SiteParameterTypeDS.FldTitle %>" size="30" maxlength="255" value="<%= SiteParameterType.SiteParameterTypeDS.EditValue %>"<%= SiteParameterType.SiteParameterTypeDS.EditAttributes %> />
</span><%= SiteParameterType.SiteParameterTypeDS.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteParameterType.SiteParameterTypeOrder.Visible Then ' SiteParameterTypeOrder %>
	<tr<%= SiteParameterType.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteParameterType.SiteParameterTypeOrder.FldCaption %></td>
		<td<%= SiteParameterType.SiteParameterTypeOrder.CellAttributes %>><span id="el_SiteParameterTypeOrder">
<input type="text" name="x_SiteParameterTypeOrder" id="x_SiteParameterTypeOrder" title="<%= SiteParameterType.SiteParameterTypeOrder.FldTitle %>" size="30" value="<%= SiteParameterType.SiteParameterTypeOrder.EditValue %>"<%= SiteParameterType.SiteParameterTypeOrder.EditAttributes %> />
</span><%= SiteParameterType.SiteParameterTypeOrder.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteParameterType.SiteParameterTemplate.Visible Then ' SiteParameterTemplate %>
	<tr<%= SiteParameterType.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteParameterType.SiteParameterTemplate.FldCaption %></td>
		<td<%= SiteParameterType.SiteParameterTemplate.CellAttributes %>><span id="el_SiteParameterTemplate">
<textarea name="x_SiteParameterTemplate" id="x_SiteParameterTemplate" title="<%= SiteParameterType.SiteParameterTemplate.FldTitle %>" cols="70" rows="10"<%= SiteParameterType.SiteParameterTemplate.EditAttributes %>><%= SiteParameterType.SiteParameterTemplate.EditValue %></textarea>
</span><%= SiteParameterType.SiteParameterTemplate.CustomMsg %></td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<input type="hidden" name="x_SiteParameterTypeID" id="x_SiteParameterTypeID" value="<%= ew_HTMLEncode(SiteParameterType.SiteParameterTypeID.CurrentValue) %>" />
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
