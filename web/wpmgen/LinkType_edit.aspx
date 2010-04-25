<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="LinkType_edit.aspx.vb" Inherits="LinkType_edit" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var LinkType_edit = new ew_Page("LinkType_edit");
// page properties
LinkType_edit.PageID = "edit"; // page ID
var EW_PAGE_ID = LinkType_edit.PageID; // for backward compatibility
// extend page with ValidateForm function
LinkType_edit.ValidateForm = function(fobj) {
	if (!this.ValidateRequired)
		return true; // ignore validation
	if (fobj.a_confirm && fobj.a_confirm.value == "F")
		return true;
	var i, elm, aelm, infix;
	var rowcnt = (fobj.key_count) ? Number(fobj.key_count.value) : 1;
	for (i=0; i<rowcnt; i++) {
		infix = (fobj.key_count) ? String(i+1) : "";
	}
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
// To include another .js script, use:
// ew_ClientScriptInclude("my_javascript.js"); 
//-->
</script>
<p><span class="aspnetmaker">Edit TABLE: Part Type<br /><br />
<a href="<%= LinkType.ReturnUrl %>">Go Back</a></span></p>
<% LinkType_edit.ShowMessage() %>
<form name="fLinkTypeedit" id="fLinkTypeedit" method="post" onsubmit="this.action=location.pathname;return LinkType_edit.ValidateForm(this);">
<p />
<input type="hidden" name="a_table" id="a_table" value="LinkType" />
<input type="hidden" name="a_edit" id="a_edit" value="U" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
<% If LinkType.LinkTypeCD.Visible Then ' LinkTypeCD %>
	<tr<%= LinkType.LinkTypeCD.RowAttributes %>>
		<td class="ewTableHeader">Link Type CD</td>
		<td<%= LinkType.LinkTypeCD.CellAttributes %>><span id="el_LinkTypeCD">
<div<%= LinkType.LinkTypeCD.ViewAttributes %>><%= LinkType.LinkTypeCD.EditValue %></div>
<input type="hidden" name="x_LinkTypeCD" id="x_LinkTypeCD" value="<%= ew_HTMLEncode(LinkType.LinkTypeCD.CurrentValue) %>" />
</span><%= LinkType.LinkTypeCD.CustomMsg %></td>
	</tr>
<% End If %>
<% If LinkType.LinkTypeDesc.Visible Then ' LinkTypeDesc %>
	<tr<%= LinkType.LinkTypeDesc.RowAttributes %>>
		<td class="ewTableHeader">Link Type Desc</td>
		<td<%= LinkType.LinkTypeDesc.CellAttributes %>><span id="el_LinkTypeDesc">
<input type="text" name="x_LinkTypeDesc" id="x_LinkTypeDesc" size="30" maxlength="255" value="<%= LinkType.LinkTypeDesc.EditValue %>"<%= LinkType.LinkTypeDesc.EditAttributes %> />
</span><%= LinkType.LinkTypeDesc.CustomMsg %></td>
	</tr>
<% End If %>
<% If LinkType.LinkTypeComment.Visible Then ' LinkTypeComment %>
	<tr<%= LinkType.LinkTypeComment.RowAttributes %>>
		<td class="ewTableHeader">Link Type Comment</td>
		<td<%= LinkType.LinkTypeComment.CellAttributes %>><span id="el_LinkTypeComment">
<textarea name="x_LinkTypeComment" id="x_LinkTypeComment" cols="35" rows="4"<%= LinkType.LinkTypeComment.EditAttributes %>><%= LinkType.LinkTypeComment.EditValue %></textarea>
</span><%= LinkType.LinkTypeComment.CustomMsg %></td>
	</tr>
<% End If %>
<% If LinkType.LinkTypeTarget.Visible Then ' LinkTypeTarget %>
	<tr<%= LinkType.LinkTypeTarget.RowAttributes %>>
		<td class="ewTableHeader">Link Type Target</td>
		<td<%= LinkType.LinkTypeTarget.CellAttributes %>><span id="el_LinkTypeTarget">
<input type="text" name="x_LinkTypeTarget" id="x_LinkTypeTarget" size="30" maxlength="50" value="<%= LinkType.LinkTypeTarget.EditValue %>"<%= LinkType.LinkTypeTarget.EditAttributes %> />
</span><%= LinkType.LinkTypeTarget.CustomMsg %></td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<p />
<input type="submit" name="btnAction" id="btnAction" value="Save Changes" />
</form>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
</asp:Content>
