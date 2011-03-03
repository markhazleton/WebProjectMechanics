<%@ Page ClassName="sitetemplate_add" Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="sitetemplate_add.aspx.vb" Inherits="sitetemplate_add" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var SiteTemplate_add = new ew_Page("SiteTemplate_add");
// page properties
SiteTemplate_add.PageID = "add"; // page ID
SiteTemplate_add.FormID = "fSiteTemplateadd"; // form ID 
var EW_PAGE_ID = SiteTemplate_add.PageID; // for backward compatibility
// extend page with ValidateForm function
SiteTemplate_add.ValidateForm = function(fobj) {
	ew_PostAutoSuggest(fobj); 
	if (!this.ValidateRequired)
		return true; // ignore validation
	if (fobj.a_confirm && fobj.a_confirm.value == "F")
		return true;
	var i, elm, aelm, infix;
	var rowcnt = (fobj.key_count) ? Number(fobj.key_count.value) : 1;
	for (i=0; i<rowcnt; i++) {
		infix = (fobj.key_count) ? String(i+1) : "";
		elm = fobj.elements["x" + infix + "_zName"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, ewLanguage.Phrase("EnterRequiredField") + " - <%= ew_JsEncode2(SiteTemplate.zName.FldCaption) %>");
		// Form Custom Validate event
		if (!this.Form_CustomValidate(fobj)) return false;
	}
	return true;
}
// extend page with Form_CustomValidate function
SiteTemplate_add.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
SiteTemplate_add.ValidateRequired = true; // uses JavaScript validation
<% Else %>
SiteTemplate_add.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker"><%= Language.Phrase("Add") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= SiteTemplate.TableCaption %><br /><br />
<a href="<%= SiteTemplate.ReturnUrl %>"><%= Language.Phrase("GoBack") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(SiteTemplate_add.DebugMsg) %>
<% SiteTemplate_add.ShowMessage() %>
<form name="fSiteTemplateadd" id="fSiteTemplateadd" method="post" onsubmit="this.action=location.pathname;return SiteTemplate_add.ValidateForm(this);">
<p />
<input type="hidden" name="t" id="t" value="SiteTemplate" />
<input type="hidden" name="a_add" id="a_add" value="A" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
<% If SiteTemplate.TemplatePrefix.Visible Then ' TemplatePrefix %>
	<tr<%= SiteTemplate.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteTemplate.TemplatePrefix.FldCaption %></td>
		<td<%= SiteTemplate.TemplatePrefix.CellAttributes %>><span id="el_TemplatePrefix">
<input type="text" name="x_TemplatePrefix" id="x_TemplatePrefix" title="<%= SiteTemplate.TemplatePrefix.FldTitle %>" size="70" maxlength="10" value="<%= SiteTemplate.TemplatePrefix.EditValue %>"<%= SiteTemplate.TemplatePrefix.EditAttributes %> />
</span><%= SiteTemplate.TemplatePrefix.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteTemplate.zName.Visible Then ' Name %>
	<tr<%= SiteTemplate.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteTemplate.zName.FldCaption %><%= Language.Phrase("FieldRequiredIndicator") %></td>
		<td<%= SiteTemplate.zName.CellAttributes %>><span id="el_zName">
<input type="text" name="x_zName" id="x_zName" title="<%= SiteTemplate.zName.FldTitle %>" size="70" maxlength="50" value="<%= SiteTemplate.zName.EditValue %>"<%= SiteTemplate.zName.EditAttributes %> />
</span><%= SiteTemplate.zName.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteTemplate.Top.Visible Then ' Top %>
	<tr<%= SiteTemplate.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteTemplate.Top.FldCaption %></td>
		<td<%= SiteTemplate.Top.CellAttributes %>><span id="el_Top">
<textarea name="x_Top" id="x_Top" title="<%= SiteTemplate.Top.FldTitle %>" cols="70" rows="30"<%= SiteTemplate.Top.EditAttributes %>><%= SiteTemplate.Top.EditValue %></textarea>
</span><%= SiteTemplate.Top.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteTemplate.Bottom.Visible Then ' Bottom %>
	<tr<%= SiteTemplate.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteTemplate.Bottom.FldCaption %></td>
		<td<%= SiteTemplate.Bottom.CellAttributes %>><span id="el_Bottom">
<textarea name="x_Bottom" id="x_Bottom" title="<%= SiteTemplate.Bottom.FldTitle %>" cols="70" rows="30"<%= SiteTemplate.Bottom.EditAttributes %>><%= SiteTemplate.Bottom.EditValue %></textarea>
</span><%= SiteTemplate.Bottom.CustomMsg %></td>
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
