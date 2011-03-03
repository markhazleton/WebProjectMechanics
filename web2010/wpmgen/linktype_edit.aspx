<%@ Page ClassName="linktype_edit" Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="linktype_edit.aspx.vb" Inherits="linktype_edit" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var LinkType_edit = new ew_Page("LinkType_edit");
// page properties
LinkType_edit.PageID = "edit"; // page ID
LinkType_edit.FormID = "fLinkTypeedit"; // form ID 
var EW_PAGE_ID = LinkType_edit.PageID; // for backward compatibility
// extend page with ValidateForm function
LinkType_edit.ValidateForm = function(fobj) {
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
LinkType_edit.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
LinkType_edit.ValidateRequired = true; // uses JavaScript validation
<% Else %>
LinkType_edit.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker"><%= Language.Phrase("Edit") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= LinkType.TableCaption %><br /><br />
<a href="<%= LinkType.ReturnUrl %>"><%= Language.Phrase("GoBack") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(LinkType_edit.DebugMsg) %>
<% LinkType_edit.ShowMessage() %>
<form name="fLinkTypeedit" id="fLinkTypeedit" method="post" onsubmit="this.action=location.pathname;return LinkType_edit.ValidateForm(this);">
<p />
<input type="hidden" name="a_table" id="a_table" value="LinkType" />
<input type="hidden" name="a_edit" id="a_edit" value="U" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
<% If LinkType.LinkTypeCD.Visible Then ' LinkTypeCD %>
	<tr<%= LinkType.RowAttributes %>>
		<td class="ewTableHeader"><%= LinkType.LinkTypeCD.FldCaption %></td>
		<td<%= LinkType.LinkTypeCD.CellAttributes %>><span id="el_LinkTypeCD">
<div<%= LinkType.LinkTypeCD.ViewAttributes %>><%= LinkType.LinkTypeCD.EditValue %></div>
<input type="hidden" name="x_LinkTypeCD" id="x_LinkTypeCD" value="<%= ew_HTMLEncode(LinkType.LinkTypeCD.CurrentValue) %>" />
</span><%= LinkType.LinkTypeCD.CustomMsg %></td>
	</tr>
<% End If %>
<% If LinkType.LinkTypeDesc.Visible Then ' LinkTypeDesc %>
	<tr<%= LinkType.RowAttributes %>>
		<td class="ewTableHeader"><%= LinkType.LinkTypeDesc.FldCaption %></td>
		<td<%= LinkType.LinkTypeDesc.CellAttributes %>><span id="el_LinkTypeDesc">
<input type="text" name="x_LinkTypeDesc" id="x_LinkTypeDesc" title="<%= LinkType.LinkTypeDesc.FldTitle %>" size="30" maxlength="255" value="<%= LinkType.LinkTypeDesc.EditValue %>"<%= LinkType.LinkTypeDesc.EditAttributes %> />
</span><%= LinkType.LinkTypeDesc.CustomMsg %></td>
	</tr>
<% End If %>
<% If LinkType.LinkTypeComment.Visible Then ' LinkTypeComment %>
	<tr<%= LinkType.RowAttributes %>>
		<td class="ewTableHeader"><%= LinkType.LinkTypeComment.FldCaption %></td>
		<td<%= LinkType.LinkTypeComment.CellAttributes %>><span id="el_LinkTypeComment">
<textarea name="x_LinkTypeComment" id="x_LinkTypeComment" title="<%= LinkType.LinkTypeComment.FldTitle %>" cols="35" rows="4"<%= LinkType.LinkTypeComment.EditAttributes %>><%= LinkType.LinkTypeComment.EditValue %></textarea>
</span><%= LinkType.LinkTypeComment.CustomMsg %></td>
	</tr>
<% End If %>
<% If LinkType.LinkTypeTarget.Visible Then ' LinkTypeTarget %>
	<tr<%= LinkType.RowAttributes %>>
		<td class="ewTableHeader"><%= LinkType.LinkTypeTarget.FldCaption %></td>
		<td<%= LinkType.LinkTypeTarget.CellAttributes %>><span id="el_LinkTypeTarget">
<input type="text" name="x_LinkTypeTarget" id="x_LinkTypeTarget" title="<%= LinkType.LinkTypeTarget.FldTitle %>" size="30" maxlength="50" value="<%= LinkType.LinkTypeTarget.EditValue %>"<%= LinkType.LinkTypeTarget.EditAttributes %> />
</span><%= LinkType.LinkTypeTarget.CustomMsg %></td>
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
