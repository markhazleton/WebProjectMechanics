<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="SiteCategoryGroup_srch.aspx.vb" Inherits="SiteCategoryGroup_srch" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var SiteCategoryGroup_search = new ew_Page("SiteCategoryGroup_search");
// page properties
SiteCategoryGroup_search.PageID = "search"; // page ID
var EW_PAGE_ID = SiteCategoryGroup_search.PageID; // for backward compatibility
// extend page with validate function for search
SiteCategoryGroup_search.ValidateSearch = function(fobj) {
	if (!this.ValidateRequired)
		return true; // ignore validation
	var infix = "";
	elm = fobj.elements["x" + infix + "_SiteCategoryGroupID"];
	if (elm && !ew_CheckInteger(elm.value))
		return ew_OnError(this, elm, "Incorrect integer - Site Category Group ID");
	elm = fobj.elements["x" + infix + "_SiteCategoryGroupOrder"];
	if (elm && !ew_CheckInteger(elm.value))
		return ew_OnError(this, elm, "Incorrect integer - Site Category Group Order");
	for (var i=0;i<fobj.elements.length;i++) {
		var elem = fobj.elements[i];
		if (elem.name.substring(0,2) == "s_" || elem.name.substring(0,3) == "sv_")
			elem.value = "";
	}
	return true;
}
<% If EW_CLIENT_VALIDATE Then %>
SiteCategoryGroup_search.ValidateRequired = true; // uses JavaScript validation
<% Else %>
SiteCategoryGroup_search.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">Search TABLE: Location Group<br /><br />
<a href="<%= SiteCategoryGroup.ReturnUrl %>">Back to List</a></span></p>
<% SiteCategoryGroup_search.ShowMessage() %>
<form name="fSiteCategoryGroupsearch" id="fSiteCategoryGroupsearch" method="post" onsubmit="this.action=location.pathname;return SiteCategoryGroup_search.ValidateSearch(this);">
<p />
<input type="hidden" name="t" id="t" value="SiteCategoryGroup" />
<input type="hidden" name="a_search" id="a_search" value="S" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
	<tr<%= SiteCategoryGroup.SiteCategoryGroupID.RowAttributes %>>
		<td class="ewTableHeader">Site Category Group ID</td>
		<td<%= SiteCategoryGroup.SiteCategoryGroupID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_SiteCategoryGroupID" id="z_SiteCategoryGroupID" value="=" /></span></td>
		<td<%= SiteCategoryGroup.SiteCategoryGroupID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_SiteCategoryGroupID" id="x_SiteCategoryGroupID" value="<%= SiteCategoryGroup.SiteCategoryGroupID.EditValue %>"<%= SiteCategoryGroup.SiteCategoryGroupID.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteCategoryGroup.SiteCategoryGroupNM.RowAttributes %>>
		<td class="ewTableHeader">Site Category Group NM</td>
		<td<%= SiteCategoryGroup.SiteCategoryGroupNM.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_SiteCategoryGroupNM" id="z_SiteCategoryGroupNM" value="LIKE" /></span></td>
		<td<%= SiteCategoryGroup.SiteCategoryGroupNM.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_SiteCategoryGroupNM" id="x_SiteCategoryGroupNM" size="30" maxlength="255" value="<%= SiteCategoryGroup.SiteCategoryGroupNM.EditValue %>"<%= SiteCategoryGroup.SiteCategoryGroupNM.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteCategoryGroup.SiteCategoryGroupDS.RowAttributes %>>
		<td class="ewTableHeader">Site Category Group DS</td>
		<td<%= SiteCategoryGroup.SiteCategoryGroupDS.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_SiteCategoryGroupDS" id="z_SiteCategoryGroupDS" value="LIKE" /></span></td>
		<td<%= SiteCategoryGroup.SiteCategoryGroupDS.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_SiteCategoryGroupDS" id="x_SiteCategoryGroupDS" size="30" maxlength="255" value="<%= SiteCategoryGroup.SiteCategoryGroupDS.EditValue %>"<%= SiteCategoryGroup.SiteCategoryGroupDS.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteCategoryGroup.SiteCategoryGroupOrder.RowAttributes %>>
		<td class="ewTableHeader">Site Category Group Order</td>
		<td<%= SiteCategoryGroup.SiteCategoryGroupOrder.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_SiteCategoryGroupOrder" id="z_SiteCategoryGroupOrder" value="=" /></span></td>
		<td<%= SiteCategoryGroup.SiteCategoryGroupOrder.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_SiteCategoryGroupOrder" id="x_SiteCategoryGroupOrder" size="30" value="<%= SiteCategoryGroup.SiteCategoryGroupOrder.EditValue %>"<%= SiteCategoryGroup.SiteCategoryGroupOrder.EditAttributes %> />
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
