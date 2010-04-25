<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="SiteParameterType_edit.aspx.vb" Inherits="SiteParameterType_edit" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var SiteParameterType_edit = new ew_Page("SiteParameterType_edit");
// page properties
SiteParameterType_edit.PageID = "edit"; // page ID
var EW_PAGE_ID = SiteParameterType_edit.PageID; // for backward compatibility
// extend page with ValidateForm function
SiteParameterType_edit.ValidateForm = function(fobj) {
	if (!this.ValidateRequired)
		return true; // ignore validation
	if (fobj.a_confirm && fobj.a_confirm.value == "F")
		return true;
	var i, elm, aelm, infix;
	var rowcnt = (fobj.key_count) ? Number(fobj.key_count.value) : 1;
	for (i=0; i<rowcnt; i++) {
		infix = (fobj.key_count) ? String(i+1) : "";
		elm = fobj.elements["x" + infix + "_SiteParameterTypeOrder"];
		if (elm && !ew_CheckInteger(elm.value))
			return ew_OnError(this, elm, "Incorrect integer - Parameter Order");
	}
	return true;
}
<% If EW_CLIENT_VALIDATE Then %>
SiteParameterType_edit.ValidateRequired = true; // uses JavaScript validation
<% Else %>
SiteParameterType_edit.ValidateRequired = false; // no JavaScript validation
<% End If %>
//-->
</script>
<script type="text/javascript" src="fckeditor/fckeditor.js"></script>
<script type="text/javascript">
<!--
_width_multiplier = 16;
_height_multiplier = 60;
var ew_DHTMLEditors = [];
// update value from editor to textarea

function ew_UpdateTextArea() {
	if (typeof ew_DHTMLEditors != 'undefined' && typeof FCKeditorAPI != 'undefined') {			
			var inst;			
			for (inst in FCKeditorAPI.__Instances)
				FCKeditorAPI.__Instances[inst].UpdateLinkedField();
	}
}
// update value from textarea to editor

function ew_UpdateDHTMLEditor(name) {
	if (typeof ew_DHTMLEditors != 'undefined' && typeof FCKeditorAPI != 'undefined') {
		var inst = FCKeditorAPI.GetInstance(name);		
		if (inst)
			inst.SetHTML(inst.LinkedField.value)
	}
}
// focus editor

function ew_FocusDHTMLEditor(name) {
	if (typeof ew_DHTMLEditors != 'undefined' && typeof FCKeditorAPI != 'undefined') {
		var inst = FCKeditorAPI.GetInstance(name);	
		if (inst && inst.EditorWindow) {
			inst.EditorWindow.focus();
		}
	}
}
//-->
</script>
<script language="JavaScript" type="text/javascript">
<!--
// Write your client script here, no need to add script tags.
// To include another .js script, use:
// ew_ClientScriptInclude("my_javascript.js"); 
//-->
</script>
<p><span class="aspnetmaker">Edit TABLE: Parameter Type<br /><br />
<a href="<%= SiteParameterType.ReturnUrl %>">Go Back</a></span></p>
<% SiteParameterType_edit.ShowMessage() %>
<form name="fSiteParameterTypeedit" id="fSiteParameterTypeedit" method="post">
<p />
<input type="hidden" name="a_table" id="a_table" value="SiteParameterType" />
<input type="hidden" name="a_edit" id="a_edit" value="U" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
<% If SiteParameterType.SiteParameterTypeNM.Visible Then ' SiteParameterTypeNM %>
	<tr<%= SiteParameterType.SiteParameterTypeNM.RowAttributes %>>
		<td class="ewTableHeader">Parameter Name</td>
		<td<%= SiteParameterType.SiteParameterTypeNM.CellAttributes %>><span id="el_SiteParameterTypeNM">
<input type="text" name="x_SiteParameterTypeNM" id="x_SiteParameterTypeNM" size="50" maxlength="255" value="<%= SiteParameterType.SiteParameterTypeNM.EditValue %>"<%= SiteParameterType.SiteParameterTypeNM.EditAttributes %> />
</span><%= SiteParameterType.SiteParameterTypeNM.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteParameterType.SiteParameterTypeDS.Visible Then ' SiteParameterTypeDS %>
	<tr<%= SiteParameterType.SiteParameterTypeDS.RowAttributes %>>
		<td class="ewTableHeader">Parameter Description</td>
		<td<%= SiteParameterType.SiteParameterTypeDS.CellAttributes %>><span id="el_SiteParameterTypeDS">
<input type="text" name="x_SiteParameterTypeDS" id="x_SiteParameterTypeDS" size="50" maxlength="255" value="<%= SiteParameterType.SiteParameterTypeDS.EditValue %>"<%= SiteParameterType.SiteParameterTypeDS.EditAttributes %> />
</span><%= SiteParameterType.SiteParameterTypeDS.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteParameterType.SiteParameterTypeOrder.Visible Then ' SiteParameterTypeOrder %>
	<tr<%= SiteParameterType.SiteParameterTypeOrder.RowAttributes %>>
		<td class="ewTableHeader">Parameter Order</td>
		<td<%= SiteParameterType.SiteParameterTypeOrder.CellAttributes %>><span id="el_SiteParameterTypeOrder">
<input type="text" name="x_SiteParameterTypeOrder" id="x_SiteParameterTypeOrder" size="30" value="<%= SiteParameterType.SiteParameterTypeOrder.EditValue %>"<%= SiteParameterType.SiteParameterTypeOrder.EditAttributes %> />
</span><%= SiteParameterType.SiteParameterTypeOrder.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteParameterType.SiteParameterTemplate.Visible Then ' SiteParameterTemplate %>
	<tr<%= SiteParameterType.SiteParameterTemplate.RowAttributes %>>
		<td class="ewTableHeader">Parameter Template</td>
		<td<%= SiteParameterType.SiteParameterTemplate.CellAttributes %>><span id="el_SiteParameterTemplate">
<textarea name="x_SiteParameterTemplate" id="x_SiteParameterTemplate" cols="50" rows="10"<%= SiteParameterType.SiteParameterTemplate.EditAttributes %>><%= SiteParameterType.SiteParameterTemplate.EditValue %></textarea>
<script type="text/javascript">
<!--
ew_DHTMLEditors.push(new ew_DHTMLEditor("x_SiteParameterTemplate", function() {
	var sBasePath = 'fckeditor/';
	var oFCKeditor = new FCKeditor('x_SiteParameterTemplate', 50*_width_multiplier, 10*_height_multiplier);
	oFCKeditor.BasePath = sBasePath;
	oFCKeditor.ReplaceTextarea();
	this.active = true;
}));
-->
</script>
</span><%= SiteParameterType.SiteParameterTemplate.CustomMsg %></td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<input type="hidden" name="x_SiteParameterTypeID" id="x_SiteParameterTypeID" value="<%= ew_HTMLEncode(SiteParameterType.SiteParameterTypeID.CurrentValue) %>" />
<p />
<input type="button" name="btnAction" id="btnAction" value="Save Changes" onclick="ew_SubmitForm(SiteParameterType_edit, this.form);" />
</form>
<script type="text/javascript">
<!--
ew_CreateEditor();  // Create DHTML editor(s)
//-->
</script>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
</asp:Content>
