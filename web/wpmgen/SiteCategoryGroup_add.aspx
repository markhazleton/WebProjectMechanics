<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="SiteCategoryGroup_add.aspx.vb" Inherits="SiteCategoryGroup_add" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var SiteCategoryGroup_add = new ew_Page("SiteCategoryGroup_add");
// page properties
SiteCategoryGroup_add.PageID = "add"; // page ID
var EW_PAGE_ID = SiteCategoryGroup_add.PageID; // for backward compatibility
// extend page with ValidateForm function
SiteCategoryGroup_add.ValidateForm = function(fobj) {
	if (!this.ValidateRequired)
		return true; // ignore validation
	if (fobj.a_confirm && fobj.a_confirm.value == "F")
		return true;
	var i, elm, aelm, infix;
	var rowcnt = (fobj.key_count) ? Number(fobj.key_count.value) : 1;
	for (i=0; i<rowcnt; i++) {
		infix = (fobj.key_count) ? String(i+1) : "";
		elm = fobj.elements["x" + infix + "_SiteCategoryGroupOrder"];
		if (elm && !ew_CheckInteger(elm.value))
			return ew_OnError(this, elm, "Incorrect integer - Site Category Group Order");
	}
	return true;
}
<% If EW_CLIENT_VALIDATE Then %>
SiteCategoryGroup_add.ValidateRequired = true; // uses JavaScript validation
<% Else %>
SiteCategoryGroup_add.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">Add to TABLE: Location Group<br /><br />
<a href="<%= SiteCategoryGroup.ReturnUrl %>">Go Back</a></span></p>
<% SiteCategoryGroup_add.ShowMessage() %>
<form name="fSiteCategoryGroupadd" id="fSiteCategoryGroupadd" method="post" onsubmit="this.action=location.pathname;return SiteCategoryGroup_add.ValidateForm(this);">
<p />
<input type="hidden" name="t" id="t" value="SiteCategoryGroup" />
<input type="hidden" name="a_add" id="a_add" value="A" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
<% If SiteCategoryGroup.SiteCategoryGroupNM.Visible Then ' SiteCategoryGroupNM %>
	<tr<%= SiteCategoryGroup.SiteCategoryGroupNM.RowAttributes %>>
		<td class="ewTableHeader">Site Category Group NM</td>
		<td<%= SiteCategoryGroup.SiteCategoryGroupNM.CellAttributes %>><span id="el_SiteCategoryGroupNM">
<input type="text" name="x_SiteCategoryGroupNM" id="x_SiteCategoryGroupNM" size="30" maxlength="255" value="<%= SiteCategoryGroup.SiteCategoryGroupNM.EditValue %>"<%= SiteCategoryGroup.SiteCategoryGroupNM.EditAttributes %> />
</span><%= SiteCategoryGroup.SiteCategoryGroupNM.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteCategoryGroup.SiteCategoryGroupDS.Visible Then ' SiteCategoryGroupDS %>
	<tr<%= SiteCategoryGroup.SiteCategoryGroupDS.RowAttributes %>>
		<td class="ewTableHeader">Site Category Group DS</td>
		<td<%= SiteCategoryGroup.SiteCategoryGroupDS.CellAttributes %>><span id="el_SiteCategoryGroupDS">
<input type="text" name="x_SiteCategoryGroupDS" id="x_SiteCategoryGroupDS" size="30" maxlength="255" value="<%= SiteCategoryGroup.SiteCategoryGroupDS.EditValue %>"<%= SiteCategoryGroup.SiteCategoryGroupDS.EditAttributes %> />
</span><%= SiteCategoryGroup.SiteCategoryGroupDS.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteCategoryGroup.SiteCategoryGroupOrder.Visible Then ' SiteCategoryGroupOrder %>
	<tr<%= SiteCategoryGroup.SiteCategoryGroupOrder.RowAttributes %>>
		<td class="ewTableHeader">Site Category Group Order</td>
		<td<%= SiteCategoryGroup.SiteCategoryGroupOrder.CellAttributes %>><span id="el_SiteCategoryGroupOrder">
<input type="text" name="x_SiteCategoryGroupOrder" id="x_SiteCategoryGroupOrder" size="30" value="<%= SiteCategoryGroup.SiteCategoryGroupOrder.EditValue %>"<%= SiteCategoryGroup.SiteCategoryGroupOrder.EditAttributes %> />
</span><%= SiteCategoryGroup.SiteCategoryGroupOrder.CustomMsg %></td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<p />
<input type="submit" name="btnAction" id="btnAction" value=" Save New " />
</form>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
</asp:Content>
