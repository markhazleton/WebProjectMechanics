<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="CompanySiteTypeParameter_add.aspx.vb" Inherits="CompanySiteTypeParameter_add" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var CompanySiteTypeParameter_add = new ew_Page("CompanySiteTypeParameter_add");
// page properties
CompanySiteTypeParameter_add.PageID = "add"; // page ID
var EW_PAGE_ID = CompanySiteTypeParameter_add.PageID; // for backward compatibility
// extend page with ValidateForm function
CompanySiteTypeParameter_add.ValidateForm = function(fobj) {
	if (!this.ValidateRequired)
		return true; // ignore validation
	if (fobj.a_confirm && fobj.a_confirm.value == "F")
		return true;
	var i, elm, aelm, infix;
	var rowcnt = (fobj.key_count) ? Number(fobj.key_count.value) : 1;
	for (i=0; i<rowcnt; i++) {
		infix = (fobj.key_count) ? String(i+1) : "";
		elm = fobj.elements["x" + infix + "_SiteParameterTypeID"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field - Parameter");
		elm = fobj.elements["x" + infix + "_SortOrder"];
		if (elm && !ew_CheckInteger(elm.value))
			return ew_OnError(this, elm, "Incorrect integer - Process Order");
	}
	return true;
}
<% If EW_CLIENT_VALIDATE Then %>
CompanySiteTypeParameter_add.ValidateRequired = true; // uses JavaScript validation
<% Else %>
CompanySiteTypeParameter_add.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">Add to TABLE: Site Type Parameter<br /><br />
<a href="<%= CompanySiteTypeParameter.ReturnUrl %>">Go Back</a></span></p>
<% CompanySiteTypeParameter_add.ShowMessage() %>
<form name="fCompanySiteTypeParameteradd" id="fCompanySiteTypeParameteradd" method="post">
<p />
<input type="hidden" name="t" id="t" value="CompanySiteTypeParameter" />
<input type="hidden" name="a_add" id="a_add" value="A" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
<% If CompanySiteTypeParameter.SiteParameterTypeID.Visible Then ' SiteParameterTypeID %>
	<tr<%= CompanySiteTypeParameter.SiteParameterTypeID.RowAttributes %>>
		<td class="ewTableHeader">Parameter<span class="ewRequired">&nbsp;*</span></td>
		<td<%= CompanySiteTypeParameter.SiteParameterTypeID.CellAttributes %>><span id="el_SiteParameterTypeID">
<select id="x_SiteParameterTypeID" name="x_SiteParameterTypeID"<%= CompanySiteTypeParameter.SiteParameterTypeID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(CompanySiteTypeParameter.SiteParameterTypeID.EditValue) Then
	arwrk = CompanySiteTypeParameter.SiteParameterTypeID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), CompanySiteTypeParameter.SiteParameterTypeID.CurrentValue) Then
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
</span><%= CompanySiteTypeParameter.SiteParameterTypeID.CustomMsg %></td>
	</tr>
<% End If %>
<% If CompanySiteTypeParameter.CompanyID.Visible Then ' CompanyID %>
	<tr<%= CompanySiteTypeParameter.CompanyID.RowAttributes %>>
		<td class="ewTableHeader">Site</td>
		<td<%= CompanySiteTypeParameter.CompanyID.CellAttributes %>><span id="el_CompanyID">
<select id="x_CompanyID" name="x_CompanyID"<%= CompanySiteTypeParameter.CompanyID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(CompanySiteTypeParameter.CompanyID.EditValue) Then
	arwrk = CompanySiteTypeParameter.CompanyID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), CompanySiteTypeParameter.CompanyID.CurrentValue) Then
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
</span><%= CompanySiteTypeParameter.CompanyID.CustomMsg %></td>
	</tr>
<% End If %>
<% If CompanySiteTypeParameter.SiteCategoryTypeID.Visible Then ' SiteCategoryTypeID %>
	<tr<%= CompanySiteTypeParameter.SiteCategoryTypeID.RowAttributes %>>
		<td class="ewTableHeader">Site Type</td>
		<td<%= CompanySiteTypeParameter.SiteCategoryTypeID.CellAttributes %>><span id="el_SiteCategoryTypeID">
<% If CompanySiteTypeParameter.SiteCategoryTypeID.SessionValue <> "" Then %>
<div<%= CompanySiteTypeParameter.SiteCategoryTypeID.ViewAttributes %>><%= CompanySiteTypeParameter.SiteCategoryTypeID.ViewValue %></div>
<input type="hidden" id="x_SiteCategoryTypeID" name="x_SiteCategoryTypeID" value="<%= ew_HtmlEncode(CompanySiteTypeParameter.SiteCategoryTypeID.CurrentValue) %>">
<% Else %>
<select id="x_SiteCategoryTypeID" name="x_SiteCategoryTypeID" onchange="ew_UpdateOpt('x_SiteCategoryID','x_SiteCategoryTypeID',CompanySiteTypeParameter_add.ar_x_SiteCategoryID);"<%= CompanySiteTypeParameter.SiteCategoryTypeID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(CompanySiteTypeParameter.SiteCategoryTypeID.EditValue) Then
	arwrk = CompanySiteTypeParameter.SiteCategoryTypeID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), CompanySiteTypeParameter.SiteCategoryTypeID.CurrentValue) Then
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
</span><%= CompanySiteTypeParameter.SiteCategoryTypeID.CustomMsg %></td>
	</tr>
<% End If %>
<% If CompanySiteTypeParameter.SiteCategoryGroupID.Visible Then ' SiteCategoryGroupID %>
	<tr<%= CompanySiteTypeParameter.SiteCategoryGroupID.RowAttributes %>>
		<td class="ewTableHeader">Site Group</td>
		<td<%= CompanySiteTypeParameter.SiteCategoryGroupID.CellAttributes %>><span id="el_SiteCategoryGroupID">
<select id="x_SiteCategoryGroupID" name="x_SiteCategoryGroupID"<%= CompanySiteTypeParameter.SiteCategoryGroupID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(CompanySiteTypeParameter.SiteCategoryGroupID.EditValue) Then
	arwrk = CompanySiteTypeParameter.SiteCategoryGroupID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), CompanySiteTypeParameter.SiteCategoryGroupID.CurrentValue) Then
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
</span><%= CompanySiteTypeParameter.SiteCategoryGroupID.CustomMsg %></td>
	</tr>
<% End If %>
<% If CompanySiteTypeParameter.SiteCategoryID.Visible Then ' SiteCategoryID %>
	<tr<%= CompanySiteTypeParameter.SiteCategoryID.RowAttributes %>>
		<td class="ewTableHeader">Site Category</td>
		<td<%= CompanySiteTypeParameter.SiteCategoryID.CellAttributes %>><span id="el_SiteCategoryID">
<select id="x_SiteCategoryID" name="x_SiteCategoryID"<%= CompanySiteTypeParameter.SiteCategoryID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(CompanySiteTypeParameter.SiteCategoryID.EditValue) Then
	arwrk = CompanySiteTypeParameter.SiteCategoryID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), CompanySiteTypeParameter.SiteCategoryID.CurrentValue) Then
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
If ew_IsArrayList(CompanySiteTypeParameter.SiteCategoryID.EditValue) Then
	arwrk = CompanySiteTypeParameter.SiteCategoryID.EditValue
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
CompanySiteTypeParameter_add.ar_x_SiteCategoryID = [<%= jswrk %>];
//-->
</script>
</span><%= CompanySiteTypeParameter.SiteCategoryID.CustomMsg %></td>
	</tr>
<% End If %>
<% If CompanySiteTypeParameter.SortOrder.Visible Then ' SortOrder %>
	<tr<%= CompanySiteTypeParameter.SortOrder.RowAttributes %>>
		<td class="ewTableHeader">Process Order</td>
		<td<%= CompanySiteTypeParameter.SortOrder.CellAttributes %>><span id="el_SortOrder">
<input type="text" name="x_SortOrder" id="x_SortOrder" size="30" value="<%= CompanySiteTypeParameter.SortOrder.EditValue %>"<%= CompanySiteTypeParameter.SortOrder.EditAttributes %> />
</span><%= CompanySiteTypeParameter.SortOrder.CustomMsg %></td>
	</tr>
<% End If %>
<% If CompanySiteTypeParameter.ParameterValue.Visible Then ' ParameterValue %>
	<tr<%= CompanySiteTypeParameter.ParameterValue.RowAttributes %>>
		<td class="ewTableHeader">Parameter Value</td>
		<td<%= CompanySiteTypeParameter.ParameterValue.CellAttributes %>><span id="el_ParameterValue">
<textarea name="x_ParameterValue" id="x_ParameterValue" cols="50" rows="10"<%= CompanySiteTypeParameter.ParameterValue.EditAttributes %>><%= CompanySiteTypeParameter.ParameterValue.EditValue %></textarea>
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
</span><%= CompanySiteTypeParameter.ParameterValue.CustomMsg %></td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<p />
<input type="button" name="btnAction" id="btnAction" value=" Save New " onclick="ew_SubmitForm(CompanySiteTypeParameter_add, this.form);" />
</form>
<script language="JavaScript">
<!--
ew_UpdateOpts([['x_SiteCategoryID','x_SiteCategoryTypeID',CompanySiteTypeParameter_add.ar_x_SiteCategoryID]]);
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
