<%@ Page ClassName="sitetemplate_srch" Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="sitetemplate_srch.aspx.vb" Inherits="sitetemplate_srch" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var SiteTemplate_search = new ew_Page("SiteTemplate_search");
// page properties
SiteTemplate_search.PageID = "search"; // page ID
SiteTemplate_search.FormID = "fSiteTemplatesearch"; // form ID 
var EW_PAGE_ID = SiteTemplate_search.PageID; // for backward compatibility
// extend page with validate function for search
SiteTemplate_search.ValidateSearch = function(fobj) {
	ew_PostAutoSuggest(fobj); 
	if (this.ValidateRequired) { 
		var infix = "";
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
SiteTemplate_search.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
SiteTemplate_search.ValidateRequired = true; // uses JavaScript validation
<% Else %>
SiteTemplate_search.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker"><%= Language.Phrase("Search") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= SiteTemplate.TableCaption %><br /><br />
<a href="<%= SiteTemplate.ReturnUrl %>"><%= Language.Phrase("BackToList") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(SiteTemplate_search.DebugMsg) %>
<% SiteTemplate_search.ShowMessage() %>
<form name="fSiteTemplatesearch" id="fSiteTemplatesearch" method="post" onsubmit="this.action=location.pathname;return SiteTemplate_search.ValidateSearch(this);">
<p />
<input type="hidden" name="t" id="t" value="SiteTemplate" />
<input type="hidden" name="a_search" id="a_search" value="S" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
	<tr<%= SiteTemplate.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteTemplate.TemplatePrefix.FldCaption %></td>
		<td<%= SiteTemplate.TemplatePrefix.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_TemplatePrefix" id="z_TemplatePrefix" value="LIKE" /></span></td>
		<td<%= SiteTemplate.TemplatePrefix.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_TemplatePrefix" id="x_TemplatePrefix" title="<%= SiteTemplate.TemplatePrefix.FldTitle %>" size="70" maxlength="10" value="<%= SiteTemplate.TemplatePrefix.EditValue %>"<%= SiteTemplate.TemplatePrefix.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteTemplate.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteTemplate.zName.FldCaption %></td>
		<td<%= SiteTemplate.zName.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_zName" id="z_zName" value="LIKE" /></span></td>
		<td<%= SiteTemplate.zName.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_zName" id="x_zName" title="<%= SiteTemplate.zName.FldTitle %>" size="70" maxlength="50" value="<%= SiteTemplate.zName.EditValue %>"<%= SiteTemplate.zName.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteTemplate.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteTemplate.Top.FldCaption %></td>
		<td<%= SiteTemplate.Top.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_Top" id="z_Top" value="LIKE" /></span></td>
		<td<%= SiteTemplate.Top.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<textarea name="x_Top" id="x_Top" title="<%= SiteTemplate.Top.FldTitle %>" cols="70" rows="30"<%= SiteTemplate.Top.EditAttributes %>><%= SiteTemplate.Top.EditValue %></textarea>
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteTemplate.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteTemplate.Bottom.FldCaption %></td>
		<td<%= SiteTemplate.Bottom.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_Bottom" id="z_Bottom" value="LIKE" /></span></td>
		<td<%= SiteTemplate.Bottom.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<textarea name="x_Bottom" id="x_Bottom" title="<%= SiteTemplate.Bottom.FldTitle %>" cols="70" rows="30"<%= SiteTemplate.Bottom.EditAttributes %>><%= SiteTemplate.Bottom.EditValue %></textarea>
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
