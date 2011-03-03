<%@ Page ClassName="linktype_srch" Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="linktype_srch.aspx.vb" Inherits="linktype_srch" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var LinkType_search = new ew_Page("LinkType_search");
// page properties
LinkType_search.PageID = "search"; // page ID
LinkType_search.FormID = "fLinkTypesearch"; // form ID 
var EW_PAGE_ID = LinkType_search.PageID; // for backward compatibility
// extend page with validate function for search
LinkType_search.ValidateSearch = function(fobj) {
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
LinkType_search.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
LinkType_search.ValidateRequired = true; // uses JavaScript validation
<% Else %>
LinkType_search.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker"><%= Language.Phrase("Search") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= LinkType.TableCaption %><br /><br />
<a href="<%= LinkType.ReturnUrl %>"><%= Language.Phrase("BackToList") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(LinkType_search.DebugMsg) %>
<% LinkType_search.ShowMessage() %>
<form name="fLinkTypesearch" id="fLinkTypesearch" method="post" onsubmit="this.action=location.pathname;return LinkType_search.ValidateSearch(this);">
<p />
<input type="hidden" name="t" id="t" value="LinkType" />
<input type="hidden" name="a_search" id="a_search" value="S" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
	<tr<%= LinkType.RowAttributes %>>
		<td class="ewTableHeader"><%= LinkType.LinkTypeCD.FldCaption %></td>
		<td<%= LinkType.LinkTypeCD.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_LinkTypeCD" id="z_LinkTypeCD" value="LIKE" /></span></td>
		<td<%= LinkType.LinkTypeCD.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_LinkTypeCD" id="x_LinkTypeCD" title="<%= LinkType.LinkTypeCD.FldTitle %>" size="30" maxlength="50" value="<%= LinkType.LinkTypeCD.EditValue %>"<%= LinkType.LinkTypeCD.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= LinkType.RowAttributes %>>
		<td class="ewTableHeader"><%= LinkType.LinkTypeDesc.FldCaption %></td>
		<td<%= LinkType.LinkTypeDesc.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_LinkTypeDesc" id="z_LinkTypeDesc" value="LIKE" /></span></td>
		<td<%= LinkType.LinkTypeDesc.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_LinkTypeDesc" id="x_LinkTypeDesc" title="<%= LinkType.LinkTypeDesc.FldTitle %>" size="30" maxlength="255" value="<%= LinkType.LinkTypeDesc.EditValue %>"<%= LinkType.LinkTypeDesc.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= LinkType.RowAttributes %>>
		<td class="ewTableHeader"><%= LinkType.LinkTypeComment.FldCaption %></td>
		<td<%= LinkType.LinkTypeComment.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_LinkTypeComment" id="z_LinkTypeComment" value="LIKE" /></span></td>
		<td<%= LinkType.LinkTypeComment.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<textarea name="x_LinkTypeComment" id="x_LinkTypeComment" title="<%= LinkType.LinkTypeComment.FldTitle %>" cols="35" rows="4"<%= LinkType.LinkTypeComment.EditAttributes %>><%= LinkType.LinkTypeComment.EditValue %></textarea>
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= LinkType.RowAttributes %>>
		<td class="ewTableHeader"><%= LinkType.LinkTypeTarget.FldCaption %></td>
		<td<%= LinkType.LinkTypeTarget.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_LinkTypeTarget" id="z_LinkTypeTarget" value="LIKE" /></span></td>
		<td<%= LinkType.LinkTypeTarget.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_LinkTypeTarget" id="x_LinkTypeTarget" title="<%= LinkType.LinkTypeTarget.FldTitle %>" size="30" maxlength="50" value="<%= LinkType.LinkTypeTarget.EditValue %>"<%= LinkType.LinkTypeTarget.EditAttributes %> />
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
