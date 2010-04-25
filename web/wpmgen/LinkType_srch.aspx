<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="LinkType_srch.aspx.vb" Inherits="LinkType_srch" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var LinkType_search = new ew_Page("LinkType_search");
// page properties
LinkType_search.PageID = "search"; // page ID
var EW_PAGE_ID = LinkType_search.PageID; // for backward compatibility
// extend page with validate function for search
LinkType_search.ValidateSearch = function(fobj) {
	if (!this.ValidateRequired)
		return true; // ignore validation
	var infix = "";
	for (var i=0;i<fobj.elements.length;i++) {
		var elem = fobj.elements[i];
		if (elem.name.substring(0,2) == "s_" || elem.name.substring(0,3) == "sv_")
			elem.value = "";
	}
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
// To include another .js script, use:
// ew_ClientScriptInclude("my_javascript.js"); 
//-->
</script>
<p><span class="aspnetmaker">Search TABLE: Part Type<br /><br />
<a href="<%= LinkType.ReturnUrl %>">Back to List</a></span></p>
<% LinkType_search.ShowMessage() %>
<form name="fLinkTypesearch" id="fLinkTypesearch" method="post" onsubmit="this.action=location.pathname;return LinkType_search.ValidateSearch(this);">
<p />
<input type="hidden" name="t" id="t" value="LinkType" />
<input type="hidden" name="a_search" id="a_search" value="S" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
	<tr<%= LinkType.LinkTypeCD.RowAttributes %>>
		<td class="ewTableHeader">Link Type CD</td>
		<td<%= LinkType.LinkTypeCD.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_LinkTypeCD" id="z_LinkTypeCD" value="LIKE" /></span></td>
		<td<%= LinkType.LinkTypeCD.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_LinkTypeCD" id="x_LinkTypeCD" size="30" maxlength="50" value="<%= LinkType.LinkTypeCD.EditValue %>"<%= LinkType.LinkTypeCD.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= LinkType.LinkTypeDesc.RowAttributes %>>
		<td class="ewTableHeader">Link Type Desc</td>
		<td<%= LinkType.LinkTypeDesc.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_LinkTypeDesc" id="z_LinkTypeDesc" value="LIKE" /></span></td>
		<td<%= LinkType.LinkTypeDesc.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_LinkTypeDesc" id="x_LinkTypeDesc" size="30" maxlength="255" value="<%= LinkType.LinkTypeDesc.EditValue %>"<%= LinkType.LinkTypeDesc.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= LinkType.LinkTypeComment.RowAttributes %>>
		<td class="ewTableHeader">Link Type Comment</td>
		<td<%= LinkType.LinkTypeComment.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_LinkTypeComment" id="z_LinkTypeComment" value="LIKE" /></span></td>
		<td<%= LinkType.LinkTypeComment.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<textarea name="x_LinkTypeComment" id="x_LinkTypeComment" cols="35" rows="4"<%= LinkType.LinkTypeComment.EditAttributes %>><%= LinkType.LinkTypeComment.EditValue %></textarea>
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= LinkType.LinkTypeTarget.RowAttributes %>>
		<td class="ewTableHeader">Link Type Target</td>
		<td<%= LinkType.LinkTypeTarget.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_LinkTypeTarget" id="z_LinkTypeTarget" value="LIKE" /></span></td>
		<td<%= LinkType.LinkTypeTarget.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_LinkTypeTarget" id="x_LinkTypeTarget" size="30" maxlength="50" value="<%= LinkType.LinkTypeTarget.EditValue %>"<%= LinkType.LinkTypeTarget.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
</table>
</div>
</td></tr></table>
<p />
<input type="submit" name="Action" id="Action" value="  Search  " />
<input type="button" name="Reset" id="Reset" value="   Reset   " onclick="ew_ClearForm(this.form);" />
</form>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
</asp:Content>
