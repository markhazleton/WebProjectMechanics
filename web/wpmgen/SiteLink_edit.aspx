<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="SiteLink_edit.aspx.vb" Inherits="SiteLink_edit" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var SiteLink_edit = new ew_Page("SiteLink_edit");
// page properties
SiteLink_edit.PageID = "edit"; // page ID
var EW_PAGE_ID = SiteLink_edit.PageID; // for backward compatibility
// extend page with ValidateForm function
SiteLink_edit.ValidateForm = function(fobj) {
	if (!this.ValidateRequired)
		return true; // ignore validation
	if (fobj.a_confirm && fobj.a_confirm.value == "F")
		return true;
	var i, elm, aelm, infix;
	var rowcnt = (fobj.key_count) ? Number(fobj.key_count.value) : 1;
	for (i=0; i<rowcnt; i++) {
		infix = (fobj.key_count) ? String(i+1) : "";
		elm = fobj.elements["x" + infix + "_SiteCategoryTypeID"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field - Site Type");
		elm = fobj.elements["x" + infix + "_Title"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field - Title");
		elm = fobj.elements["x" + infix + "_LinkTypeCD"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field - Link Type");
		elm = fobj.elements["x" + infix + "_CategoryID"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field - Link Category");
		elm = fobj.elements["x" + infix + "_Ranks"];
		if (elm && !ew_CheckInteger(elm.value))
			return ew_OnError(this, elm, "Incorrect integer - Ranks");
		elm = fobj.elements["x" + infix + "_Views"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field - Active/Visible");
	}
	return true;
}
<% If EW_CLIENT_VALIDATE Then %>
SiteLink_edit.ValidateRequired = true; // uses JavaScript validation
<% Else %>
SiteLink_edit.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">Edit TABLE: Site Type Parts<br /><br />
<a href="<%= SiteLink.ReturnUrl %>">Go Back</a></span></p>
<% SiteLink_edit.ShowMessage() %>
<form name="fSiteLinkedit" id="fSiteLinkedit" method="post">
<p />
<input type="hidden" name="a_table" id="a_table" value="SiteLink" />
<input type="hidden" name="a_edit" id="a_edit" value="U" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
<% If SiteLink.SiteCategoryTypeID.Visible Then ' SiteCategoryTypeID %>
	<tr<%= SiteLink.SiteCategoryTypeID.RowAttributes %>>
		<td class="ewTableHeader">Site Type<span class="ewRequired">&nbsp;*</span></td>
		<td<%= SiteLink.SiteCategoryTypeID.CellAttributes %>><span id="el_SiteCategoryTypeID">
<select id="x_SiteCategoryTypeID" name="x_SiteCategoryTypeID" onchange="ew_UpdateOpt('x_SiteCategoryID','x_SiteCategoryTypeID',SiteLink_edit.ar_x_SiteCategoryID);"<%= SiteLink.SiteCategoryTypeID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(SiteLink.SiteCategoryTypeID.EditValue) Then
	arwrk = SiteLink.SiteCategoryTypeID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), SiteLink.SiteCategoryTypeID.CurrentValue) Then
			selwrk = " selected=""selected"""
			emptywrk = False
		Else
			selwrk = ""
		End If
%>
<option value="<%= ew_HtmlEncode(arwrk(rowcntwrk)(0)) %>"<%= selwrk %>>
<%= arwrk(rowcntwrk)(1) %>
</option>
<%
	Next
End If
%>
</select>
</span><%= SiteLink.SiteCategoryTypeID.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteLink.Title.Visible Then ' Title %>
	<tr<%= SiteLink.Title.RowAttributes %>>
		<td class="ewTableHeader">Title<span class="ewRequired">&nbsp;*</span></td>
		<td<%= SiteLink.Title.CellAttributes %>><span id="el_Title">
<input type="text" name="x_Title" id="x_Title" size="30" maxlength="255" value="<%= SiteLink.Title.EditValue %>"<%= SiteLink.Title.EditAttributes %> />
</span><%= SiteLink.Title.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteLink.LinkTypeCD.Visible Then ' LinkTypeCD %>
	<tr<%= SiteLink.LinkTypeCD.RowAttributes %>>
		<td class="ewTableHeader">Link Type<span class="ewRequired">&nbsp;*</span></td>
		<td<%= SiteLink.LinkTypeCD.CellAttributes %>><span id="el_LinkTypeCD">
<select id="x_LinkTypeCD" name="x_LinkTypeCD"<%= SiteLink.LinkTypeCD.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(SiteLink.LinkTypeCD.EditValue) Then
	arwrk = SiteLink.LinkTypeCD.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), SiteLink.LinkTypeCD.CurrentValue) Then
			selwrk = " selected=""selected"""
			emptywrk = False
		Else
			selwrk = ""
		End If
%>
<option value="<%= ew_HtmlEncode(arwrk(rowcntwrk)(0)) %>"<%= selwrk %>>
<%= arwrk(rowcntwrk)(1) %>
</option>
<%
	Next
End If
%>
</select>
</span><%= SiteLink.LinkTypeCD.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteLink.CategoryID.Visible Then ' CategoryID %>
	<tr<%= SiteLink.CategoryID.RowAttributes %>>
		<td class="ewTableHeader">Link Category<span class="ewRequired">&nbsp;*</span></td>
		<td<%= SiteLink.CategoryID.CellAttributes %>><span id="el_CategoryID">
<select id="x_CategoryID" name="x_CategoryID"<%= SiteLink.CategoryID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(SiteLink.CategoryID.EditValue) Then
	arwrk = SiteLink.CategoryID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), SiteLink.CategoryID.CurrentValue) Then
			selwrk = " selected=""selected"""
			emptywrk = False
		Else
			selwrk = ""
		End If
%>
<option value="<%= ew_HtmlEncode(arwrk(rowcntwrk)(0)) %>"<%= selwrk %>>
<%= arwrk(rowcntwrk)(1) %>
</option>
<%
	Next
End If
%>
</select>
</span><%= SiteLink.CategoryID.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteLink.CompanyID.Visible Then ' CompanyID %>
	<tr<%= SiteLink.CompanyID.RowAttributes %>>
		<td class="ewTableHeader">Company</td>
		<td<%= SiteLink.CompanyID.CellAttributes %>><span id="el_CompanyID">
<select id="x_CompanyID" name="x_CompanyID"<%= SiteLink.CompanyID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(SiteLink.CompanyID.EditValue) Then
	arwrk = SiteLink.CompanyID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), SiteLink.CompanyID.CurrentValue) Then
			selwrk = " selected=""selected"""
			emptywrk = False
		Else
			selwrk = ""
		End If
%>
<option value="<%= ew_HtmlEncode(arwrk(rowcntwrk)(0)) %>"<%= selwrk %>>
<%= arwrk(rowcntwrk)(1) %>
</option>
<%
	Next
End If
%>
</select>
</span><%= SiteLink.CompanyID.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteLink.SiteCategoryID.Visible Then ' SiteCategoryID %>
	<tr<%= SiteLink.SiteCategoryID.RowAttributes %>>
		<td class="ewTableHeader">Site Category</td>
		<td<%= SiteLink.SiteCategoryID.CellAttributes %>><span id="el_SiteCategoryID">
<select id="x_SiteCategoryID" name="x_SiteCategoryID"<%= SiteLink.SiteCategoryID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(SiteLink.SiteCategoryID.EditValue) Then
	arwrk = SiteLink.SiteCategoryID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), SiteLink.SiteCategoryID.CurrentValue) Then
			selwrk = " selected=""selected"""
			emptywrk = False
		Else
			selwrk = ""
		End If
%>
<option value="<%= ew_HtmlEncode(arwrk(rowcntwrk)(0)) %>"<%= selwrk %>>
<%= arwrk(rowcntwrk)(1) %>
</option>
<%
	Next
End If
%>
</select>
<%
jswrk = "" ' Initialise
If ew_IsArrayList(SiteLink.SiteCategoryID.EditValue) Then
	arwrk = SiteLink.SiteCategoryID.EditValue
	For rowcntwrk As Integer = 1 To arwrk.Count - 1
		If jswrk <> "" Then jswrk = jswrk & ","
		jswrk = jswrk & "['" & ew_JsEncode(arwrk(rowcntwrk)(0)) & "'," ' Value
		jswrk = jswrk & "'" & ew_JsEncode(arwrk(rowcntwrk)(1)) & "'," ' Display field 1
		jswrk = jswrk & "'" & ew_JsEncode(arwrk(rowcntwrk)(2)) & "'," ' Display field 2
		jswrk = jswrk & "'" & ew_JsEncode(arwrk(rowcntwrk)(3)) & "']" ' Filter field
	Next
End If
%>
<script type="text/javascript">
<!--
SiteLink_edit.ar_x_SiteCategoryID = [<%= jswrk %>];
//-->
</script>
</span><%= SiteLink.SiteCategoryID.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteLink.SiteCategoryGroupID.Visible Then ' SiteCategoryGroupID %>
	<tr<%= SiteLink.SiteCategoryGroupID.RowAttributes %>>
		<td class="ewTableHeader">Category Group</td>
		<td<%= SiteLink.SiteCategoryGroupID.CellAttributes %>><span id="el_SiteCategoryGroupID">
<select id="x_SiteCategoryGroupID" name="x_SiteCategoryGroupID"<%= SiteLink.SiteCategoryGroupID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(SiteLink.SiteCategoryGroupID.EditValue) Then
	arwrk = SiteLink.SiteCategoryGroupID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), SiteLink.SiteCategoryGroupID.CurrentValue) Then
			selwrk = " selected=""selected"""
			emptywrk = False
		Else
			selwrk = ""
		End If
%>
<option value="<%= ew_HtmlEncode(arwrk(rowcntwrk)(0)) %>"<%= selwrk %>>
<%= arwrk(rowcntwrk)(1) %>
</option>
<%
	Next
End If
%>
</select>
</span><%= SiteLink.SiteCategoryGroupID.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteLink.Description.Visible Then ' Description %>
	<tr<%= SiteLink.Description.RowAttributes %>>
		<td class="ewTableHeader">Description</td>
		<td<%= SiteLink.Description.CellAttributes %>><span id="el_Description">
<textarea name="x_Description" id="x_Description" cols="100" rows="5"<%= SiteLink.Description.EditAttributes %>><%= SiteLink.Description.EditValue %></textarea>
</span><%= SiteLink.Description.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteLink.URL.Visible Then ' URL %>
	<tr<%= SiteLink.URL.RowAttributes %>>
		<td class="ewTableHeader">URL</td>
		<td<%= SiteLink.URL.CellAttributes %>><span id="el_URL">
<textarea name="x_URL" id="x_URL" cols="50" rows="10"<%= SiteLink.URL.EditAttributes %>><%= SiteLink.URL.EditValue %></textarea>
<script type="text/javascript">
<!--
ew_DHTMLEditors.push(new ew_DHTMLEditor("x_URL", function() {
	var sBasePath = 'fckeditor/';
	var oFCKeditor = new FCKeditor('x_URL', 50*_width_multiplier, 10*_height_multiplier);
	oFCKeditor.BasePath = sBasePath;
	oFCKeditor.ReplaceTextarea();
	this.active = true;
}));
-->
</script>
</span><%= SiteLink.URL.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteLink.Ranks.Visible Then ' Ranks %>
	<tr<%= SiteLink.Ranks.RowAttributes %>>
		<td class="ewTableHeader">Ranks</td>
		<td<%= SiteLink.Ranks.CellAttributes %>><span id="el_Ranks">
<input type="text" name="x_Ranks" id="x_Ranks" size="30" value="<%= SiteLink.Ranks.EditValue %>"<%= SiteLink.Ranks.EditAttributes %> />
</span><%= SiteLink.Ranks.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteLink.Views.Visible Then ' Views %>
	<tr<%= SiteLink.Views.RowAttributes %>>
		<td class="ewTableHeader">Active/Visible<span class="ewRequired">&nbsp;*</span></td>
		<td<%= SiteLink.Views.CellAttributes %>><span id="el_Views">
<%
If ew_SameStr(SiteLink.Views.CurrentValue, "1") Then
	selwrk = " checked=""checked"""
Else
	selwrk = ""
End If
%>
<input type="checkbox" name="x_Views" id="x_Views" value="1"<%= selwrk %><%= SiteLink.Views.EditAttributes %> />
</span><%= SiteLink.Views.CustomMsg %></td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<input type="hidden" name="x_ID" id="x_ID" value="<%= ew_HTMLEncode(SiteLink.ID.CurrentValue) %>" />
<p />
<input type="button" name="btnAction" id="btnAction" value="Save Changes" onclick="ew_SubmitForm(SiteLink_edit, this.form);" />
</form>
<script language="JavaScript">
<!--
ew_UpdateOpts([['x_SiteCategoryID','x_SiteCategoryTypeID',SiteLink_edit.ar_x_SiteCategoryID]]);
//-->
</script>
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
