<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="SiteParameterType_srch.aspx.vb" Inherits="SiteParameterType_srch" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var SiteParameterType_search = new ew_Page("SiteParameterType_search");
// page properties
SiteParameterType_search.PageID = "search"; // page ID
var EW_PAGE_ID = SiteParameterType_search.PageID; // for backward compatibility
// extend page with validate function for search
SiteParameterType_search.ValidateSearch = function(fobj) {
	if (!this.ValidateRequired)
		return true; // ignore validation
	var infix = "";
	elm = fobj.elements["x" + infix + "_SiteParameterTypeOrder"];
	if (elm && !ew_CheckInteger(elm.value))
		return ew_OnError(this, elm, "Incorrect integer - Parameter Order");
	for (var i=0;i<fobj.elements.length;i++) {
		var elem = fobj.elements[i];
		if (elem.name.substring(0,2) == "s_" || elem.name.substring(0,3) == "sv_")
			elem.value = "";
	}
	return true;
}
<% If EW_CLIENT_VALIDATE Then %>
SiteParameterType_search.ValidateRequired = true; // uses JavaScript validation
<% Else %>
SiteParameterType_search.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">Search TABLE: Parameter Type<br /><br />
<a href="<%= SiteParameterType.ReturnUrl %>">Back to List</a></span></p>
<% SiteParameterType_search.ShowMessage() %>
<form name="fSiteParameterTypesearch" id="fSiteParameterTypesearch" method="post" onsubmit="this.action=location.pathname;return SiteParameterType_search.ValidateSearch(this);">
<p />
<input type="hidden" name="t" id="t" value="SiteParameterType" />
<input type="hidden" name="a_search" id="a_search" value="S" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
	<tr<%= SiteParameterType.SiteParameterTypeNM.RowAttributes %>>
		<td class="ewTableHeader">Parameter Name</td>
		<td<%= SiteParameterType.SiteParameterTypeNM.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_SiteParameterTypeNM" id="z_SiteParameterTypeNM" value="LIKE" /></span></td>
		<td<%= SiteParameterType.SiteParameterTypeNM.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_SiteParameterTypeNM" id="x_SiteParameterTypeNM" size="50" maxlength="255" value="<%= SiteParameterType.SiteParameterTypeNM.EditValue %>"<%= SiteParameterType.SiteParameterTypeNM.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteParameterType.SiteParameterTypeDS.RowAttributes %>>
		<td class="ewTableHeader">Parameter Description</td>
		<td<%= SiteParameterType.SiteParameterTypeDS.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_SiteParameterTypeDS" id="z_SiteParameterTypeDS" value="LIKE" /></span></td>
		<td<%= SiteParameterType.SiteParameterTypeDS.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_SiteParameterTypeDS" id="x_SiteParameterTypeDS" size="50" maxlength="255" value="<%= SiteParameterType.SiteParameterTypeDS.EditValue %>"<%= SiteParameterType.SiteParameterTypeDS.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteParameterType.SiteParameterTypeOrder.RowAttributes %>>
		<td class="ewTableHeader">Parameter Order</td>
		<td<%= SiteParameterType.SiteParameterTypeOrder.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_SiteParameterTypeOrder" id="z_SiteParameterTypeOrder" value="=" /></span></td>
		<td<%= SiteParameterType.SiteParameterTypeOrder.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_SiteParameterTypeOrder" id="x_SiteParameterTypeOrder" size="30" value="<%= SiteParameterType.SiteParameterTypeOrder.EditValue %>"<%= SiteParameterType.SiteParameterTypeOrder.EditAttributes %> />
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
