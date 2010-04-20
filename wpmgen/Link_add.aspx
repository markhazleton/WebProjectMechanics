<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="Link_add.aspx.vb" Inherits="Link_add" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var Link_add = new ew_Page("Link_add");
// page properties
Link_add.PageID = "add"; // page ID
var EW_PAGE_ID = Link_add.PageID; // for backward compatibility
// extend page with ValidateForm function
Link_add.ValidateForm = function(fobj) {
	if (!this.ValidateRequired)
		return true; // ignore validation
	if (fobj.a_confirm && fobj.a_confirm.value == "F")
		return true;
	var i, elm, aelm, infix;
	var rowcnt = (fobj.key_count) ? Number(fobj.key_count.value) : 1;
	for (i=0; i<rowcnt; i++) {
		infix = (fobj.key_count) ? String(i+1) : "";
		elm = fobj.elements["x" + infix + "_Title"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field - Title");
		elm = fobj.elements["x" + infix + "_LinkTypeCD"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field - Part Type");
		elm = fobj.elements["x" + infix + "_CategoryID"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field - Part Category");
		elm = fobj.elements["x" + infix + "_CompanyID"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field - Company");
		elm = fobj.elements["x" + infix + "_Ranks"];
		if (elm && !ew_CheckInteger(elm.value))
			return ew_OnError(this, elm, "Incorrect integer - Ranks");
		elm = fobj.elements["x" + infix + "_UserID"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field - User");
		elm = fobj.elements["x" + infix + "_DateAdd"];
		if (elm && !ew_CheckUSDate(elm.value))
			return ew_OnError(this, elm, "Incorrect date, format = mm/dd/yyyy - Date Add");
	}
	return true;
}
<% If EW_CLIENT_VALIDATE Then %>
Link_add.ValidateRequired = true; // uses JavaScript validation
<% Else %>
Link_add.ValidateRequired = false; // no JavaScript validation
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
<link rel="stylesheet" type="text/css" media="all" href="calendar/calendar-win2k-1.css" title="win2k-1" />
<script type="text/javascript" src="calendar/calendar.js"></script>
<script type="text/javascript" src="calendar/lang/calendar-en.js"></script>
<script type="text/javascript" src="calendar/calendar-setup.js"></script>
<script language="JavaScript" type="text/javascript">
<!--
// Write your client script here, no need to add script tags.
// To include another .js script, use:
// ew_ClientScriptInclude("my_javascript.js"); 
//-->
</script>
<p><span class="aspnetmaker">Add to TABLE: Site Parts<br /><br />
<a href="<%= Link.ReturnUrl %>">Go Back</a></span></p>
<% Link_add.ShowMessage() %>
<form name="fLinkadd" id="fLinkadd" method="post">
<p />
<input type="hidden" name="t" id="t" value="Link" />
<input type="hidden" name="a_add" id="a_add" value="A" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
<% If Link.Title.Visible Then ' Title %>
	<tr<%= Link.Title.RowAttributes %>>
		<td class="ewTableHeader">Title<span class="ewRequired">&nbsp;*</span></td>
		<td<%= Link.Title.CellAttributes %>><span id="el_Title">
<input type="text" name="x_Title" id="x_Title" size="30" maxlength="255" value="<%= Link.Title.EditValue %>"<%= Link.Title.EditAttributes %> />
</span><%= Link.Title.CustomMsg %></td>
	</tr>
<% End If %>
<% If Link.LinkTypeCD.Visible Then ' LinkTypeCD %>
	<tr<%= Link.LinkTypeCD.RowAttributes %>>
		<td class="ewTableHeader">Part Type<span class="ewRequired">&nbsp;*</span></td>
		<td<%= Link.LinkTypeCD.CellAttributes %>><span id="el_LinkTypeCD">
<select id="x_LinkTypeCD" name="x_LinkTypeCD"<%= Link.LinkTypeCD.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Link.LinkTypeCD.EditValue) Then
	arwrk = Link.LinkTypeCD.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Link.LinkTypeCD.CurrentValue) Then
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
</span><%= Link.LinkTypeCD.CustomMsg %></td>
	</tr>
<% End If %>
<% If Link.CategoryID.Visible Then ' CategoryID %>
	<tr<%= Link.CategoryID.RowAttributes %>>
		<td class="ewTableHeader">Part Category<span class="ewRequired">&nbsp;*</span></td>
		<td<%= Link.CategoryID.CellAttributes %>><span id="el_CategoryID">
<select id="x_CategoryID" name="x_CategoryID"<%= Link.CategoryID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Link.CategoryID.EditValue) Then
	arwrk = Link.CategoryID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Link.CategoryID.CurrentValue) Then
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
</span><%= Link.CategoryID.CustomMsg %></td>
	</tr>
<% End If %>
<% If Link.CompanyID.Visible Then ' CompanyID %>
	<tr<%= Link.CompanyID.RowAttributes %>>
		<td class="ewTableHeader">Company<span class="ewRequired">&nbsp;*</span></td>
		<td<%= Link.CompanyID.CellAttributes %>><span id="el_CompanyID">
<select id="x_CompanyID" name="x_CompanyID" onchange="ew_UpdateOpt('x_zPageID','x_CompanyID',Link_add.ar_x_zPageID);ew_UpdateOpt('x_UserID','x_CompanyID',Link_add.ar_x_UserID);"<%= Link.CompanyID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Link.CompanyID.EditValue) Then
	arwrk = Link.CompanyID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Link.CompanyID.CurrentValue) Then
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
</span><%= Link.CompanyID.CustomMsg %></td>
	</tr>
<% End If %>
<% If Link.SiteCategoryGroupID.Visible Then ' SiteCategoryGroupID %>
	<tr<%= Link.SiteCategoryGroupID.RowAttributes %>>
		<td class="ewTableHeader">Location Group</td>
		<td<%= Link.SiteCategoryGroupID.CellAttributes %>><span id="el_SiteCategoryGroupID">
<select id="x_SiteCategoryGroupID" name="x_SiteCategoryGroupID"<%= Link.SiteCategoryGroupID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Link.SiteCategoryGroupID.EditValue) Then
	arwrk = Link.SiteCategoryGroupID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Link.SiteCategoryGroupID.CurrentValue) Then
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
</span><%= Link.SiteCategoryGroupID.CustomMsg %></td>
	</tr>
<% End If %>
<% If Link.zPageID.Visible Then ' PageID %>
	<tr<%= Link.zPageID.RowAttributes %>>
		<td class="ewTableHeader">Location</td>
		<td<%= Link.zPageID.CellAttributes %>><span id="el_zPageID">
<select id="x_zPageID" name="x_zPageID"<%= Link.zPageID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Link.zPageID.EditValue) Then
	arwrk = Link.zPageID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Link.zPageID.CurrentValue) Then
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
If ew_IsArrayList(Link.zPageID.EditValue) Then
	arwrk = Link.zPageID.EditValue
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
Link_add.ar_x_zPageID = [<%= jswrk %>];
//-->
</script>
</span><%= Link.zPageID.CustomMsg %></td>
	</tr>
<% End If %>
<% If Link.Views.Visible Then ' Views %>
	<tr<%= Link.Views.RowAttributes %>>
		<td class="ewTableHeader">Visible/Active<span class="ewRequired">&nbsp;*</span></td>
		<td<%= Link.Views.CellAttributes %>><span id="el_Views">
<%
If ew_SameStr(Link.Views.CurrentValue, "1") Then
	selwrk = " checked=""checked"""
Else
	selwrk = ""
End If
%>
<input type="checkbox" name="x_Views" id="x_Views" value="1"<%= selwrk %><%= Link.Views.EditAttributes %> />
</span><%= Link.Views.CustomMsg %></td>
	</tr>
<% End If %>
<% If Link.Description.Visible Then ' Description %>
	<tr<%= Link.Description.RowAttributes %>>
		<td class="ewTableHeader">Description</td>
		<td<%= Link.Description.CellAttributes %>><span id="el_Description">
<textarea name="x_Description" id="x_Description" cols="80" rows="5"<%= Link.Description.EditAttributes %>><%= Link.Description.EditValue %></textarea>
</span><%= Link.Description.CustomMsg %></td>
	</tr>
<% End If %>
<% If Link.URL.Visible Then ' URL %>
	<tr<%= Link.URL.RowAttributes %>>
		<td class="ewTableHeader">URL</td>
		<td<%= Link.URL.CellAttributes %>><span id="el_URL">
<textarea name="x_URL" id="x_URL" cols="50" rows="10"<%= Link.URL.EditAttributes %>><%= Link.URL.EditValue %></textarea>
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
</span><%= Link.URL.CustomMsg %></td>
	</tr>
<% End If %>
<% If Link.Ranks.Visible Then ' Ranks %>
	<tr<%= Link.Ranks.RowAttributes %>>
		<td class="ewTableHeader">Ranks</td>
		<td<%= Link.Ranks.CellAttributes %>><span id="el_Ranks">
<input type="text" name="x_Ranks" id="x_Ranks" size="30" value="<%= Link.Ranks.EditValue %>"<%= Link.Ranks.EditAttributes %> />
</span><%= Link.Ranks.CustomMsg %></td>
	</tr>
<% End If %>
<% If Link.UserID.Visible Then ' UserID %>
	<tr<%= Link.UserID.RowAttributes %>>
		<td class="ewTableHeader">User<span class="ewRequired">&nbsp;*</span></td>
		<td<%= Link.UserID.CellAttributes %>><span id="el_UserID">
<select id="x_UserID" name="x_UserID"<%= Link.UserID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Link.UserID.EditValue) Then
	arwrk = Link.UserID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Link.UserID.CurrentValue) Then
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
If ew_IsArrayList(Link.UserID.EditValue) Then
	arwrk = Link.UserID.EditValue
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
Link_add.ar_x_UserID = [<%= jswrk %>];
//-->
</script>
</span><%= Link.UserID.CustomMsg %></td>
	</tr>
<% End If %>
<% If Link.ASIN.Visible Then ' ASIN %>
	<tr<%= Link.ASIN.RowAttributes %>>
		<td class="ewTableHeader">ASIN (Amazon)</td>
		<td<%= Link.ASIN.CellAttributes %>><span id="el_ASIN">
<input type="text" name="x_ASIN" id="x_ASIN" size="60" maxlength="50" value="<%= Link.ASIN.EditValue %>"<%= Link.ASIN.EditAttributes %> />
</span><%= Link.ASIN.CustomMsg %></td>
	</tr>
<% End If %>
<% If Link.DateAdd.Visible Then ' DateAdd %>
	<tr<%= Link.DateAdd.RowAttributes %>>
		<td class="ewTableHeader">Date Add</td>
		<td<%= Link.DateAdd.CellAttributes %>><span id="el_DateAdd">
<input type="text" name="x_DateAdd" id="x_DateAdd" value="<%= Link.DateAdd.EditValue %>"<%= Link.DateAdd.EditAttributes %> />
&nbsp;<img src="images/calendar.png" id="cal_x_DateAdd" name="cal_x_DateAdd" alt="Pick a date" style="cursor:pointer;cursor:hand;" />
<script type="text/javascript">
Calendar.setup({
	inputField : "x_DateAdd", // ID of the input field
	ifFormat : "%m/%d/%Y", // the date format
	button : "cal_x_DateAdd" // ID of the button
});
</script>
</span><%= Link.DateAdd.CustomMsg %></td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<p />
<input type="button" name="btnAction" id="btnAction" value=" Save New " onclick="ew_SubmitForm(Link_add, this.form);" />
</form>
<script language="JavaScript">
<!--
ew_UpdateOpts([['x_zPageID','x_CompanyID',Link_add.ar_x_zPageID],['x_UserID','x_CompanyID',Link_add.ar_x_UserID]]);
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
