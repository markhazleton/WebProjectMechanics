<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="Article_add.aspx.vb" Inherits="Article_add" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var Article_add = new ew_Page("Article_add");
// page properties
Article_add.PageID = "add"; // page ID
var EW_PAGE_ID = Article_add.PageID; // for backward compatibility
// extend page with ValidateForm function
Article_add.ValidateForm = function(fobj) {
	if (!this.ValidateRequired)
		return true; // ignore validation
	if (fobj.a_confirm && fobj.a_confirm.value == "F")
		return true;
	var i, elm, aelm, infix;
	var rowcnt = (fobj.key_count) ? Number(fobj.key_count.value) : 1;
	for (i=0; i<rowcnt; i++) {
		infix = (fobj.key_count) ? String(i+1) : "";
		elm = fobj.elements["x" + infix + "_StartDT"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field - Start DT");
		elm = fobj.elements["x" + infix + "_StartDT"];
		if (elm && !ew_CheckUSDate(elm.value))
			return ew_OnError(this, elm, "Incorrect date, format = mm/dd/yyyy - Start DT");
		elm = fobj.elements["x" + infix + "_CompanyID"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field - Site");
		elm = fobj.elements["x" + infix + "_EndDT"];
		if (elm && !ew_CheckUSDate(elm.value))
			return ew_OnError(this, elm, "Incorrect date, format = mm/dd/yyyy - End DT");
		elm = fobj.elements["x" + infix + "_ExpireDT"];
		if (elm && !ew_CheckUSDate(elm.value))
			return ew_OnError(this, elm, "Incorrect date, format = mm/dd/yyyy - Expire DT");
	}
	return true;
}
Article_add.SelectAllKey = function(elem) {
	ew_SelectAll(elem);
}
<% If EW_CLIENT_VALIDATE Then %>
Article_add.ValidateRequired = true; // uses JavaScript validation
<% Else %>
Article_add.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">Add to TABLE: Site Article<br /><br />
<a href="<%= Article.ReturnUrl %>">Go Back</a></span></p>
<% Article_add.ShowMessage() %>
<form name="fArticleadd" id="fArticleadd" method="post">
<p />
<input type="hidden" name="t" id="t" value="Article" />
<input type="hidden" name="a_add" id="a_add" value="A" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
<% If Article.Active.Visible Then ' Active %>
	<tr<%= Article.Active.RowAttributes %>>
		<td class="ewTableHeader">Active</td>
		<td<%= Article.Active.CellAttributes %>><span id="el_Active">
<%
If ew_SameStr(Article.Active.CurrentValue, "1") Then
	selwrk = " checked=""checked"""
Else
	selwrk = ""
End If
%>
<input type="checkbox" name="x_Active" id="x_Active" value="1"<%= selwrk %><%= Article.Active.EditAttributes %> />
</span><%= Article.Active.CustomMsg %></td>
	</tr>
<% End If %>
<% If Article.StartDT.Visible Then ' StartDT %>
	<tr<%= Article.StartDT.RowAttributes %>>
		<td class="ewTableHeader">Start DT<span class="ewRequired">&nbsp;*</span></td>
		<td<%= Article.StartDT.CellAttributes %>><span id="el_StartDT">
<input type="text" name="x_StartDT" id="x_StartDT" value="<%= Article.StartDT.EditValue %>"<%= Article.StartDT.EditAttributes %> />
&nbsp;<img src="images/calendar.png" id="cal_x_StartDT" name="cal_x_StartDT" alt="Pick a date" style="cursor:pointer;cursor:hand;" />
<script type="text/javascript">
Calendar.setup({
	inputField : "x_StartDT", // ID of the input field
	ifFormat : "%m/%d/%Y", // the date format
	button : "cal_x_StartDT" // ID of the button
});
</script>
</span><%= Article.StartDT.CustomMsg %></td>
	</tr>
<% End If %>
<% If Article.Title.Visible Then ' Title %>
	<tr<%= Article.Title.RowAttributes %>>
		<td class="ewTableHeader">Title</td>
		<td<%= Article.Title.CellAttributes %>><span id="el_Title">
<input type="text" name="x_Title" id="x_Title" size="80" maxlength="255" value="<%= Article.Title.EditValue %>"<%= Article.Title.EditAttributes %> />
</span><%= Article.Title.CustomMsg %></td>
	</tr>
<% End If %>
<% If Article.CompanyID.Visible Then ' CompanyID %>
	<tr<%= Article.CompanyID.RowAttributes %>>
		<td class="ewTableHeader">Site<span class="ewRequired">&nbsp;*</span></td>
		<td<%= Article.CompanyID.CellAttributes %>><span id="el_CompanyID">
<select id="x_CompanyID" name="x_CompanyID" onchange="ew_UpdateOpt('x_zPageID','x_CompanyID',Article_add.ar_x_zPageID);ew_UpdateOpt('x_ContactID','x_CompanyID',Article_add.ar_x_ContactID);"<%= Article.CompanyID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Article.CompanyID.EditValue) Then
	arwrk = Article.CompanyID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Article.CompanyID.CurrentValue) Then
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
</span><%= Article.CompanyID.CustomMsg %></td>
	</tr>
<% End If %>
<% If Article.zPageID.Visible Then ' PageID %>
	<tr<%= Article.zPageID.RowAttributes %>>
		<td class="ewTableHeader">Location</td>
		<td<%= Article.zPageID.CellAttributes %>><span id="el_zPageID">
<select id="x_zPageID" name="x_zPageID"<%= Article.zPageID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Article.zPageID.EditValue) Then
	arwrk = Article.zPageID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Article.zPageID.CurrentValue) Then
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
If ew_IsArrayList(Article.zPageID.EditValue) Then
	arwrk = Article.zPageID.EditValue
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
Article_add.ar_x_zPageID = [<%= jswrk %>];
//-->
</script>
</span><%= Article.zPageID.CustomMsg %></td>
	</tr>
<% End If %>
<% If Article.ContactID.Visible Then ' ContactID %>
	<tr<%= Article.ContactID.RowAttributes %>>
		<td class="ewTableHeader">Contact</td>
		<td<%= Article.ContactID.CellAttributes %>><span id="el_ContactID">
<select id="x_ContactID" name="x_ContactID"<%= Article.ContactID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Article.ContactID.EditValue) Then
	arwrk = Article.ContactID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Article.ContactID.CurrentValue) Then
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
If ew_IsArrayList(Article.ContactID.EditValue) Then
	arwrk = Article.ContactID.EditValue
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
Article_add.ar_x_ContactID = [<%= jswrk %>];
//-->
</script>
</span><%= Article.ContactID.CustomMsg %></td>
	</tr>
<% End If %>
<% If Article.Description.Visible Then ' Description %>
	<tr<%= Article.Description.RowAttributes %>>
		<td class="ewTableHeader">Description</td>
		<td<%= Article.Description.CellAttributes %>><span id="el_Description">
<textarea name="x_Description" id="x_Description" cols="80" rows="2"<%= Article.Description.EditAttributes %>><%= Article.Description.EditValue %></textarea>
</span><%= Article.Description.CustomMsg %></td>
	</tr>
<% End If %>
<% If Article.ArticleBody.Visible Then ' ArticleBody %>
	<tr<%= Article.ArticleBody.RowAttributes %>>
		<td class="ewTableHeader">Article Body</td>
		<td<%= Article.ArticleBody.CellAttributes %>><span id="el_ArticleBody">
<textarea name="x_ArticleBody" id="x_ArticleBody" cols="50" rows="12"<%= Article.ArticleBody.EditAttributes %>><%= Article.ArticleBody.EditValue %></textarea>
<script type="text/javascript">
<!--
ew_DHTMLEditors.push(new ew_DHTMLEditor("x_ArticleBody", function() {
	var sBasePath = 'fckeditor/';
	var oFCKeditor = new FCKeditor('x_ArticleBody', 50*_width_multiplier, 12*_height_multiplier);
	oFCKeditor.BasePath = sBasePath;
	oFCKeditor.ReplaceTextarea();
	this.active = true;
}));
-->
</script>
</span><%= Article.ArticleBody.CustomMsg %></td>
	</tr>
<% End If %>
<% If Article.EndDT.Visible Then ' EndDT %>
	<tr<%= Article.EndDT.RowAttributes %>>
		<td class="ewTableHeader">End DT</td>
		<td<%= Article.EndDT.CellAttributes %>><span id="el_EndDT">
<input type="text" name="x_EndDT" id="x_EndDT" value="<%= Article.EndDT.EditValue %>"<%= Article.EndDT.EditAttributes %> />
&nbsp;<img src="images/calendar.png" id="cal_x_EndDT" name="cal_x_EndDT" alt="Pick a date" style="cursor:pointer;cursor:hand;" />
<script type="text/javascript">
Calendar.setup({
	inputField : "x_EndDT", // ID of the input field
	ifFormat : "%m/%d/%Y", // the date format
	button : "cal_x_EndDT" // ID of the button
});
</script>
</span><%= Article.EndDT.CustomMsg %></td>
	</tr>
<% End If %>
<% If Article.ExpireDT.Visible Then ' ExpireDT %>
	<tr<%= Article.ExpireDT.RowAttributes %>>
		<td class="ewTableHeader">Expire DT</td>
		<td<%= Article.ExpireDT.CellAttributes %>><span id="el_ExpireDT">
<input type="text" name="x_ExpireDT" id="x_ExpireDT" value="<%= Article.ExpireDT.EditValue %>"<%= Article.ExpireDT.EditAttributes %> />
&nbsp;<img src="images/calendar.png" id="cal_x_ExpireDT" name="cal_x_ExpireDT" alt="Pick a date" style="cursor:pointer;cursor:hand;" />
<script type="text/javascript">
Calendar.setup({
	inputField : "x_ExpireDT", // ID of the input field
	ifFormat : "%m/%d/%Y", // the date format
	button : "cal_x_ExpireDT" // ID of the button
});
</script>
</span><%= Article.ExpireDT.CustomMsg %></td>
	</tr>
<% End If %>
<% If Article.ArticleSummary.Visible Then ' ArticleSummary %>
	<tr<%= Article.ArticleSummary.RowAttributes %>>
		<td class="ewTableHeader">Article Summary</td>
		<td<%= Article.ArticleSummary.CellAttributes %>><span id="el_ArticleSummary">
<textarea name="x_ArticleSummary" id="x_ArticleSummary" cols="50" rows="5"<%= Article.ArticleSummary.EditAttributes %>><%= Article.ArticleSummary.EditValue %></textarea>
<script type="text/javascript">
<!--
ew_DHTMLEditors.push(new ew_DHTMLEditor("x_ArticleSummary", function() {
	var sBasePath = 'fckeditor/';
	var oFCKeditor = new FCKeditor('x_ArticleSummary', 50*_width_multiplier, 5*_height_multiplier);
	oFCKeditor.BasePath = sBasePath;
	oFCKeditor.ReplaceTextarea();
	this.active = true;
}));
-->
</script>
</span><%= Article.ArticleSummary.CustomMsg %></td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<p />
<input type="button" name="btnAction" id="btnAction" value=" Save New " onclick="ew_SubmitForm(Article_add, this.form);" />
</form>
<script language="JavaScript">
<!--
ew_UpdateOpts([['x_zPageID','x_CompanyID',Article_add.ar_x_zPageID],['x_ContactID','x_CompanyID',Article_add.ar_x_ContactID]]);
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
