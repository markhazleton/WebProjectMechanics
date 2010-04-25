<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="CompanySiteParameter_add.aspx.vb" Inherits="CompanySiteParameter_add" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var CompanySiteParameter_add = new ew_Page("CompanySiteParameter_add");
// page properties
CompanySiteParameter_add.PageID = "add"; // page ID
var EW_PAGE_ID = CompanySiteParameter_add.PageID; // for backward compatibility
// extend page with ValidateForm function
CompanySiteParameter_add.ValidateForm = function(fobj) {
	if (!this.ValidateRequired)
		return true; // ignore validation
	if (fobj.a_confirm && fobj.a_confirm.value == "F")
		return true;
	var i, elm, aelm, infix;
	var rowcnt = (fobj.key_count) ? Number(fobj.key_count.value) : 1;
	for (i=0; i<rowcnt; i++) {
		infix = (fobj.key_count) ? String(i+1) : "";
		elm = fobj.elements["x" + infix + "_CompanyID"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field - Site");
		elm = fobj.elements["x" + infix + "_SiteParameterTypeID"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field -  Parameter");
		elm = fobj.elements["x" + infix + "_SortOrder"];
		if (elm && !ew_CheckInteger(elm.value))
			return ew_OnError(this, elm, "Incorrect integer - Process Order");
	}
	return true;
}
<% If EW_CLIENT_VALIDATE Then %>
CompanySiteParameter_add.ValidateRequired = true; // uses JavaScript validation
<% Else %>
CompanySiteParameter_add.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">Add to TABLE: Site Parameter<br /><br />
<a href="<%= CompanySiteParameter.ReturnUrl %>">Go Back</a></span></p>
<% CompanySiteParameter_add.ShowMessage() %>
<form name="fCompanySiteParameteradd" id="fCompanySiteParameteradd" method="post">
<p />
<input type="hidden" name="t" id="t" value="CompanySiteParameter" />
<input type="hidden" name="a_add" id="a_add" value="A" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
<% If CompanySiteParameter.CompanyID.Visible Then ' CompanyID %>
	<tr<%= CompanySiteParameter.CompanyID.RowAttributes %>>
		<td class="ewTableHeader">Site<span class="ewRequired">&nbsp;*</span></td>
		<td<%= CompanySiteParameter.CompanyID.CellAttributes %>><span id="el_CompanyID">
<select id="x_CompanyID" name="x_CompanyID" onchange="ew_UpdateOpt('x_zPageID','x_CompanyID',CompanySiteParameter_add.ar_x_zPageID);"<%= CompanySiteParameter.CompanyID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(CompanySiteParameter.CompanyID.EditValue) Then
	arwrk = CompanySiteParameter.CompanyID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), CompanySiteParameter.CompanyID.CurrentValue) Then
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
</span><%= CompanySiteParameter.CompanyID.CustomMsg %></td>
	</tr>
<% End If %>
<% If CompanySiteParameter.zPageID.Visible Then ' PageID %>
	<tr<%= CompanySiteParameter.zPageID.RowAttributes %>>
		<td class="ewTableHeader">Page</td>
		<td<%= CompanySiteParameter.zPageID.CellAttributes %>><span id="el_zPageID">
<select id="x_zPageID" name="x_zPageID"<%= CompanySiteParameter.zPageID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(CompanySiteParameter.zPageID.EditValue) Then
	arwrk = CompanySiteParameter.zPageID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), CompanySiteParameter.zPageID.CurrentValue) Then
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
If ew_IsArrayList(CompanySiteParameter.zPageID.EditValue) Then
	arwrk = CompanySiteParameter.zPageID.EditValue
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
CompanySiteParameter_add.ar_x_zPageID = [<%= jswrk %>];
//-->
</script>
</span><%= CompanySiteParameter.zPageID.CustomMsg %></td>
	</tr>
<% End If %>
<% If CompanySiteParameter.SiteCategoryGroupID.Visible Then ' SiteCategoryGroupID %>
	<tr<%= CompanySiteParameter.SiteCategoryGroupID.RowAttributes %>>
		<td class="ewTableHeader">Category Group</td>
		<td<%= CompanySiteParameter.SiteCategoryGroupID.CellAttributes %>><span id="el_SiteCategoryGroupID">
<select id="x_SiteCategoryGroupID" name="x_SiteCategoryGroupID"<%= CompanySiteParameter.SiteCategoryGroupID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(CompanySiteParameter.SiteCategoryGroupID.EditValue) Then
	arwrk = CompanySiteParameter.SiteCategoryGroupID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), CompanySiteParameter.SiteCategoryGroupID.CurrentValue) Then
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
</span><%= CompanySiteParameter.SiteCategoryGroupID.CustomMsg %></td>
	</tr>
<% End If %>
<% If CompanySiteParameter.SiteParameterTypeID.Visible Then ' SiteParameterTypeID %>
	<tr<%= CompanySiteParameter.SiteParameterTypeID.RowAttributes %>>
		<td class="ewTableHeader"> Parameter<span class="ewRequired">&nbsp;*</span></td>
		<td<%= CompanySiteParameter.SiteParameterTypeID.CellAttributes %>><span id="el_SiteParameterTypeID">
<% If CompanySiteParameter.SiteParameterTypeID.SessionValue <> "" Then %>
<div<%= CompanySiteParameter.SiteParameterTypeID.ViewAttributes %>><%= CompanySiteParameter.SiteParameterTypeID.ViewValue %></div>
<input type="hidden" id="x_SiteParameterTypeID" name="x_SiteParameterTypeID" value="<%= ew_HtmlEncode(CompanySiteParameter.SiteParameterTypeID.CurrentValue) %>">
<% Else %>
<select id="x_SiteParameterTypeID" name="x_SiteParameterTypeID"<%= CompanySiteParameter.SiteParameterTypeID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(CompanySiteParameter.SiteParameterTypeID.EditValue) Then
	arwrk = CompanySiteParameter.SiteParameterTypeID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), CompanySiteParameter.SiteParameterTypeID.CurrentValue) Then
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
<% End If %>
</span><%= CompanySiteParameter.SiteParameterTypeID.CustomMsg %></td>
	</tr>
<% End If %>
<% If CompanySiteParameter.SortOrder.Visible Then ' SortOrder %>
	<tr<%= CompanySiteParameter.SortOrder.RowAttributes %>>
		<td class="ewTableHeader">Process Order</td>
		<td<%= CompanySiteParameter.SortOrder.CellAttributes %>><span id="el_SortOrder">
<input type="text" name="x_SortOrder" id="x_SortOrder" size="30" value="<%= CompanySiteParameter.SortOrder.EditValue %>"<%= CompanySiteParameter.SortOrder.EditAttributes %> />
</span><%= CompanySiteParameter.SortOrder.CustomMsg %></td>
	</tr>
<% End If %>
<% If CompanySiteParameter.ParameterValue.Visible Then ' ParameterValue %>
	<tr<%= CompanySiteParameter.ParameterValue.RowAttributes %>>
		<td class="ewTableHeader">Parameter Value</td>
		<td<%= CompanySiteParameter.ParameterValue.CellAttributes %>><span id="el_ParameterValue">
<textarea name="x_ParameterValue" id="x_ParameterValue" cols="50" rows="10"<%= CompanySiteParameter.ParameterValue.EditAttributes %>><%= CompanySiteParameter.ParameterValue.EditValue %></textarea>
<script type="text/javascript">
<!--
ew_DHTMLEditors.push(new ew_DHTMLEditor("x_ParameterValue", function() {
	var sBasePath = 'fckeditor/';
	var oFCKeditor = new FCKeditor('x_ParameterValue', 50*_width_multiplier, 10*_height_multiplier);
	oFCKeditor.BasePath = sBasePath;
	oFCKeditor.ReplaceTextarea();
	this.active = true;
}));
-->
</script>
</span><%= CompanySiteParameter.ParameterValue.CustomMsg %></td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<p />
<input type="button" name="btnAction" id="btnAction" value=" Save New " onclick="ew_SubmitForm(CompanySiteParameter_add, this.form);" />
</form>
<script language="JavaScript">
<!--
ew_UpdateOpts([['x_zPageID','x_CompanyID',CompanySiteParameter_add.ar_x_zPageID]]);
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
