<%@ Page ClassName="sitecategorygroup_srch" Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="sitecategorygroup_srch.aspx.vb" Inherits="sitecategorygroup_srch" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var SiteCategoryGroup_search = new ew_Page("SiteCategoryGroup_search");
// page properties
SiteCategoryGroup_search.PageID = "search"; // page ID
SiteCategoryGroup_search.FormID = "fSiteCategoryGroupsearch"; // form ID 
var EW_PAGE_ID = SiteCategoryGroup_search.PageID; // for backward compatibility
// extend page with validate function for search
SiteCategoryGroup_search.ValidateSearch = function(fobj) {
	ew_PostAutoSuggest(fobj); 
	if (this.ValidateRequired) { 
		var infix = "";
		elm = fobj.elements["x" + infix + "_SiteCategoryGroupID"];
		if (elm && elm.type != "hidden" && !ew_CheckInteger(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(SiteCategoryGroup.SiteCategoryGroupID.FldErrMsg) %>");
		elm = fobj.elements["x" + infix + "_SiteCategoryGroupOrder"];
		if (elm && elm.type != "hidden" && !ew_CheckInteger(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(SiteCategoryGroup.SiteCategoryGroupOrder.FldErrMsg) %>");
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
SiteCategoryGroup_search.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
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
// To include another .js script, you can use, e.g.
// ew_ClientScriptInclude("my_javascript.js"); or
// ew_ClientScriptInclude("my_javascript.js", {onSuccess: function(o) { // your code here }});
//-->
</script>
<p><span class="aspnetmaker"><%= Language.Phrase("Search") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= SiteCategoryGroup.TableCaption %><br /><br />
<a href="<%= SiteCategoryGroup.ReturnUrl %>"><%= Language.Phrase("BackToList") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(SiteCategoryGroup_search.DebugMsg) %>
<% SiteCategoryGroup_search.ShowMessage() %>
<form name="fSiteCategoryGroupsearch" id="fSiteCategoryGroupsearch" method="post" onsubmit="this.action=location.pathname;return SiteCategoryGroup_search.ValidateSearch(this);">
<p />
<input type="hidden" name="t" id="t" value="SiteCategoryGroup" />
<input type="hidden" name="a_search" id="a_search" value="S" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
	<tr<%= SiteCategoryGroup.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteCategoryGroup.SiteCategoryGroupID.FldCaption %></td>
		<td<%= SiteCategoryGroup.SiteCategoryGroupID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_SiteCategoryGroupID" id="z_SiteCategoryGroupID" value="=" /></span></td>
		<td<%= SiteCategoryGroup.SiteCategoryGroupID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_SiteCategoryGroupID" id="x_SiteCategoryGroupID" title="<%= SiteCategoryGroup.SiteCategoryGroupID.FldTitle %>" value="<%= SiteCategoryGroup.SiteCategoryGroupID.EditValue %>"<%= SiteCategoryGroup.SiteCategoryGroupID.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteCategoryGroup.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteCategoryGroup.SiteCategoryGroupNM.FldCaption %></td>
		<td<%= SiteCategoryGroup.SiteCategoryGroupNM.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_SiteCategoryGroupNM" id="z_SiteCategoryGroupNM" value="LIKE" /></span></td>
		<td<%= SiteCategoryGroup.SiteCategoryGroupNM.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_SiteCategoryGroupNM" id="x_SiteCategoryGroupNM" title="<%= SiteCategoryGroup.SiteCategoryGroupNM.FldTitle %>" size="30" maxlength="255" value="<%= SiteCategoryGroup.SiteCategoryGroupNM.EditValue %>"<%= SiteCategoryGroup.SiteCategoryGroupNM.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteCategoryGroup.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteCategoryGroup.SiteCategoryGroupDS.FldCaption %></td>
		<td<%= SiteCategoryGroup.SiteCategoryGroupDS.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_SiteCategoryGroupDS" id="z_SiteCategoryGroupDS" value="LIKE" /></span></td>
		<td<%= SiteCategoryGroup.SiteCategoryGroupDS.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_SiteCategoryGroupDS" id="x_SiteCategoryGroupDS" title="<%= SiteCategoryGroup.SiteCategoryGroupDS.FldTitle %>" size="30" maxlength="255" value="<%= SiteCategoryGroup.SiteCategoryGroupDS.EditValue %>"<%= SiteCategoryGroup.SiteCategoryGroupDS.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteCategoryGroup.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteCategoryGroup.SiteCategoryGroupOrder.FldCaption %></td>
		<td<%= SiteCategoryGroup.SiteCategoryGroupOrder.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_SiteCategoryGroupOrder" id="z_SiteCategoryGroupOrder" value="=" /></span></td>
		<td<%= SiteCategoryGroup.SiteCategoryGroupOrder.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_SiteCategoryGroupOrder" id="x_SiteCategoryGroupOrder" title="<%= SiteCategoryGroup.SiteCategoryGroupOrder.FldTitle %>" size="30" value="<%= SiteCategoryGroup.SiteCategoryGroupOrder.EditValue %>"<%= SiteCategoryGroup.SiteCategoryGroupOrder.EditAttributes %> />
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
