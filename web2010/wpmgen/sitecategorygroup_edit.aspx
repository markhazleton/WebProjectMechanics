<%@ Page ClassName="sitecategorygroup_edit" Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="sitecategorygroup_edit.aspx.vb" Inherits="sitecategorygroup_edit" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var SiteCategoryGroup_edit = new ew_Page("SiteCategoryGroup_edit");
// page properties
SiteCategoryGroup_edit.PageID = "edit"; // page ID
SiteCategoryGroup_edit.FormID = "fSiteCategoryGroupedit"; // form ID 
var EW_PAGE_ID = SiteCategoryGroup_edit.PageID; // for backward compatibility
// extend page with ValidateForm function
SiteCategoryGroup_edit.ValidateForm = function(fobj) {
	ew_PostAutoSuggest(fobj); 
	if (!this.ValidateRequired)
		return true; // ignore validation
	if (fobj.a_confirm && fobj.a_confirm.value == "F")
		return true;
	var i, elm, aelm, infix;
	var rowcnt = (fobj.key_count) ? Number(fobj.key_count.value) : 1;
	for (i=0; i<rowcnt; i++) {
		infix = (fobj.key_count) ? String(i+1) : "";
		elm = fobj.elements["x" + infix + "_SiteCategoryGroupOrder"];
		if (elm && elm.type != "hidden" && !ew_CheckInteger(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(SiteCategoryGroup.SiteCategoryGroupOrder.FldErrMsg) %>");
		// Form Custom Validate event
		if (!this.Form_CustomValidate(fobj)) return false;
	}
	return true;
}
// extend page with Form_CustomValidate function
SiteCategoryGroup_edit.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
SiteCategoryGroup_edit.ValidateRequired = true; // uses JavaScript validation
<% Else %>
SiteCategoryGroup_edit.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker"><%= Language.Phrase("Edit") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= SiteCategoryGroup.TableCaption %><br /><br />
<a href="<%= SiteCategoryGroup.ReturnUrl %>"><%= Language.Phrase("GoBack") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(SiteCategoryGroup_edit.DebugMsg) %>
<% SiteCategoryGroup_edit.ShowMessage() %>
<form name="fSiteCategoryGroupedit" id="fSiteCategoryGroupedit" method="post" onsubmit="this.action=location.pathname;return SiteCategoryGroup_edit.ValidateForm(this);">
<p />
<input type="hidden" name="a_table" id="a_table" value="SiteCategoryGroup" />
<input type="hidden" name="a_edit" id="a_edit" value="U" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
<% If SiteCategoryGroup.SiteCategoryGroupID.Visible Then ' SiteCategoryGroupID %>
	<tr<%= SiteCategoryGroup.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteCategoryGroup.SiteCategoryGroupID.FldCaption %></td>
		<td<%= SiteCategoryGroup.SiteCategoryGroupID.CellAttributes %>><span id="el_SiteCategoryGroupID">
<div<%= SiteCategoryGroup.SiteCategoryGroupID.ViewAttributes %>><%= SiteCategoryGroup.SiteCategoryGroupID.EditValue %></div>
<input type="hidden" name="x_SiteCategoryGroupID" id="x_SiteCategoryGroupID" value="<%= ew_HTMLEncode(SiteCategoryGroup.SiteCategoryGroupID.CurrentValue) %>" />
</span><%= SiteCategoryGroup.SiteCategoryGroupID.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteCategoryGroup.SiteCategoryGroupNM.Visible Then ' SiteCategoryGroupNM %>
	<tr<%= SiteCategoryGroup.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteCategoryGroup.SiteCategoryGroupNM.FldCaption %></td>
		<td<%= SiteCategoryGroup.SiteCategoryGroupNM.CellAttributes %>><span id="el_SiteCategoryGroupNM">
<input type="text" name="x_SiteCategoryGroupNM" id="x_SiteCategoryGroupNM" title="<%= SiteCategoryGroup.SiteCategoryGroupNM.FldTitle %>" size="30" maxlength="255" value="<%= SiteCategoryGroup.SiteCategoryGroupNM.EditValue %>"<%= SiteCategoryGroup.SiteCategoryGroupNM.EditAttributes %> />
</span><%= SiteCategoryGroup.SiteCategoryGroupNM.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteCategoryGroup.SiteCategoryGroupDS.Visible Then ' SiteCategoryGroupDS %>
	<tr<%= SiteCategoryGroup.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteCategoryGroup.SiteCategoryGroupDS.FldCaption %></td>
		<td<%= SiteCategoryGroup.SiteCategoryGroupDS.CellAttributes %>><span id="el_SiteCategoryGroupDS">
<input type="text" name="x_SiteCategoryGroupDS" id="x_SiteCategoryGroupDS" title="<%= SiteCategoryGroup.SiteCategoryGroupDS.FldTitle %>" size="30" maxlength="255" value="<%= SiteCategoryGroup.SiteCategoryGroupDS.EditValue %>"<%= SiteCategoryGroup.SiteCategoryGroupDS.EditAttributes %> />
</span><%= SiteCategoryGroup.SiteCategoryGroupDS.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteCategoryGroup.SiteCategoryGroupOrder.Visible Then ' SiteCategoryGroupOrder %>
	<tr<%= SiteCategoryGroup.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteCategoryGroup.SiteCategoryGroupOrder.FldCaption %></td>
		<td<%= SiteCategoryGroup.SiteCategoryGroupOrder.CellAttributes %>><span id="el_SiteCategoryGroupOrder">
<input type="text" name="x_SiteCategoryGroupOrder" id="x_SiteCategoryGroupOrder" title="<%= SiteCategoryGroup.SiteCategoryGroupOrder.FldTitle %>" size="30" value="<%= SiteCategoryGroup.SiteCategoryGroupOrder.EditValue %>"<%= SiteCategoryGroup.SiteCategoryGroupOrder.EditAttributes %> />
</span><%= SiteCategoryGroup.SiteCategoryGroupOrder.CustomMsg %></td>
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
