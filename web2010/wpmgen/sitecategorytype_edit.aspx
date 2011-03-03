<%@ Page ClassName="sitecategorytype_edit" Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="sitecategorytype_edit.aspx.vb" Inherits="sitecategorytype_edit" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var SiteCategoryType_edit = new ew_Page("SiteCategoryType_edit");
// page properties
SiteCategoryType_edit.PageID = "edit"; // page ID
SiteCategoryType_edit.FormID = "fSiteCategoryTypeedit"; // form ID 
var EW_PAGE_ID = SiteCategoryType_edit.PageID; // for backward compatibility
// extend page with ValidateForm function
SiteCategoryType_edit.ValidateForm = function(fobj) {
	ew_PostAutoSuggest(fobj); 
	if (!this.ValidateRequired)
		return true; // ignore validation
	if (fobj.a_confirm && fobj.a_confirm.value == "F")
		return true;
	var i, elm, aelm, infix;
	var rowcnt = (fobj.key_count) ? Number(fobj.key_count.value) : 1;
	for (i=0; i<rowcnt; i++) {
		infix = (fobj.key_count) ? String(i+1) : "";
		elm = fobj.elements["x" + infix + "_DefaultSiteCategoryID"];
		if (elm && elm.type != "hidden" && !ew_CheckInteger(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(SiteCategoryType.DefaultSiteCategoryID.FldErrMsg) %>");
		// Form Custom Validate event
		if (!this.Form_CustomValidate(fobj)) return false;
	}
	return true;
}
// extend page with Form_CustomValidate function
SiteCategoryType_edit.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
SiteCategoryType_edit.ValidateRequired = true; // uses JavaScript validation
<% Else %>
SiteCategoryType_edit.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker"><%= Language.Phrase("Edit") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= SiteCategoryType.TableCaption %><br /><br />
<a href="<%= SiteCategoryType.ReturnUrl %>"><%= Language.Phrase("GoBack") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(SiteCategoryType_edit.DebugMsg) %>
<% SiteCategoryType_edit.ShowMessage() %>
<form name="fSiteCategoryTypeedit" id="fSiteCategoryTypeedit" method="post" onsubmit="this.action=location.pathname;return SiteCategoryType_edit.ValidateForm(this);">
<p />
<input type="hidden" name="a_table" id="a_table" value="SiteCategoryType" />
<input type="hidden" name="a_edit" id="a_edit" value="U" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
<% If SiteCategoryType.SiteCategoryTypeID.Visible Then ' SiteCategoryTypeID %>
	<tr<%= SiteCategoryType.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteCategoryType.SiteCategoryTypeID.FldCaption %></td>
		<td<%= SiteCategoryType.SiteCategoryTypeID.CellAttributes %>><span id="el_SiteCategoryTypeID">
<div<%= SiteCategoryType.SiteCategoryTypeID.ViewAttributes %>><%= SiteCategoryType.SiteCategoryTypeID.EditValue %></div>
<input type="hidden" name="x_SiteCategoryTypeID" id="x_SiteCategoryTypeID" value="<%= ew_HTMLEncode(SiteCategoryType.SiteCategoryTypeID.CurrentValue) %>" />
</span><%= SiteCategoryType.SiteCategoryTypeID.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteCategoryType.SiteCategoryTypeNM.Visible Then ' SiteCategoryTypeNM %>
	<tr<%= SiteCategoryType.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteCategoryType.SiteCategoryTypeNM.FldCaption %></td>
		<td<%= SiteCategoryType.SiteCategoryTypeNM.CellAttributes %>><span id="el_SiteCategoryTypeNM">
<input type="text" name="x_SiteCategoryTypeNM" id="x_SiteCategoryTypeNM" title="<%= SiteCategoryType.SiteCategoryTypeNM.FldTitle %>" size="30" maxlength="255" value="<%= SiteCategoryType.SiteCategoryTypeNM.EditValue %>"<%= SiteCategoryType.SiteCategoryTypeNM.EditAttributes %> />
</span><%= SiteCategoryType.SiteCategoryTypeNM.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteCategoryType.SiteCategoryTypeDS.Visible Then ' SiteCategoryTypeDS %>
	<tr<%= SiteCategoryType.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteCategoryType.SiteCategoryTypeDS.FldCaption %></td>
		<td<%= SiteCategoryType.SiteCategoryTypeDS.CellAttributes %>><span id="el_SiteCategoryTypeDS">
<input type="text" name="x_SiteCategoryTypeDS" id="x_SiteCategoryTypeDS" title="<%= SiteCategoryType.SiteCategoryTypeDS.FldTitle %>" size="30" maxlength="255" value="<%= SiteCategoryType.SiteCategoryTypeDS.EditValue %>"<%= SiteCategoryType.SiteCategoryTypeDS.EditAttributes %> />
</span><%= SiteCategoryType.SiteCategoryTypeDS.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteCategoryType.SiteCategoryComment.Visible Then ' SiteCategoryComment %>
	<tr<%= SiteCategoryType.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteCategoryType.SiteCategoryComment.FldCaption %></td>
		<td<%= SiteCategoryType.SiteCategoryComment.CellAttributes %>><span id="el_SiteCategoryComment">
<input type="text" name="x_SiteCategoryComment" id="x_SiteCategoryComment" title="<%= SiteCategoryType.SiteCategoryComment.FldTitle %>" size="30" maxlength="255" value="<%= SiteCategoryType.SiteCategoryComment.EditValue %>"<%= SiteCategoryType.SiteCategoryComment.EditAttributes %> />
</span><%= SiteCategoryType.SiteCategoryComment.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteCategoryType.SiteCategoryFileName.Visible Then ' SiteCategoryFileName %>
	<tr<%= SiteCategoryType.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteCategoryType.SiteCategoryFileName.FldCaption %></td>
		<td<%= SiteCategoryType.SiteCategoryFileName.CellAttributes %>><span id="el_SiteCategoryFileName">
<input type="text" name="x_SiteCategoryFileName" id="x_SiteCategoryFileName" title="<%= SiteCategoryType.SiteCategoryFileName.FldTitle %>" size="30" maxlength="255" value="<%= SiteCategoryType.SiteCategoryFileName.EditValue %>"<%= SiteCategoryType.SiteCategoryFileName.EditAttributes %> />
</span><%= SiteCategoryType.SiteCategoryFileName.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteCategoryType.SiteCategoryTransferURL.Visible Then ' SiteCategoryTransferURL %>
	<tr<%= SiteCategoryType.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteCategoryType.SiteCategoryTransferURL.FldCaption %></td>
		<td<%= SiteCategoryType.SiteCategoryTransferURL.CellAttributes %>><span id="el_SiteCategoryTransferURL">
<input type="text" name="x_SiteCategoryTransferURL" id="x_SiteCategoryTransferURL" title="<%= SiteCategoryType.SiteCategoryTransferURL.FldTitle %>" size="30" maxlength="255" value="<%= SiteCategoryType.SiteCategoryTransferURL.EditValue %>"<%= SiteCategoryType.SiteCategoryTransferURL.EditAttributes %> />
</span><%= SiteCategoryType.SiteCategoryTransferURL.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteCategoryType.DefaultSiteCategoryID.Visible Then ' DefaultSiteCategoryID %>
	<tr<%= SiteCategoryType.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteCategoryType.DefaultSiteCategoryID.FldCaption %></td>
		<td<%= SiteCategoryType.DefaultSiteCategoryID.CellAttributes %>><span id="el_DefaultSiteCategoryID">
<input type="text" name="x_DefaultSiteCategoryID" id="x_DefaultSiteCategoryID" title="<%= SiteCategoryType.DefaultSiteCategoryID.FldTitle %>" size="30" value="<%= SiteCategoryType.DefaultSiteCategoryID.EditValue %>"<%= SiteCategoryType.DefaultSiteCategoryID.EditAttributes %> />
</span><%= SiteCategoryType.DefaultSiteCategoryID.CustomMsg %></td>
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