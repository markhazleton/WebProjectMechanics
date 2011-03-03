<%@ Page ClassName="article_add" Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="article_add.aspx.vb" Inherits="article_add" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var Article_add = new ew_Page("Article_add");
// page properties
Article_add.PageID = "add"; // page ID
Article_add.FormID = "fArticleadd"; // form ID 
var EW_PAGE_ID = Article_add.PageID; // for backward compatibility
// extend page with ValidateForm function
Article_add.ValidateForm = function(fobj) {
	ew_PostAutoSuggest(fobj); 
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
			return ew_OnError(this, elm, ewLanguage.Phrase("EnterRequiredField") + " - <%= ew_JsEncode2(Article.Title.FldCaption) %>");
		elm = fobj.elements["x" + infix + "_EndDT"];
		if (elm && elm.type != "hidden" && !ew_CheckUSDate(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(Article.EndDT.FldErrMsg) %>");
		elm = fobj.elements["x" + infix + "_ExpireDT"];
		if (elm && elm.type != "hidden" && !ew_CheckUSDate(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(Article.ExpireDT.FldErrMsg) %>");
		// Form Custom Validate event
		if (!this.Form_CustomValidate(fobj)) return false;
	}
	return true;
}
// extend page with Form_CustomValidate function
Article_add.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
Article_add.ValidateRequired = true; // uses JavaScript validation
<% Else %>
Article_add.ValidateRequired = false; // no JavaScript validation
<% End If %>
//-->
</script>
<link rel="stylesheet" type="text/css" media="all" href="calendar/calendar-win2k-cold-1.css" title="win2k-1" />
<script type="text/javascript" src="calendar/calendar.js"></script>
<script type="text/javascript" src="calendar/lang/calendar-en.js"></script>
<script type="text/javascript" src="calendar/calendar-setup.js"></script>
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
// To include another .js script, you can use, e.g.
// ew_ClientScriptInclude("my_javascript.js"); or
// ew_ClientScriptInclude("my_javascript.js", {onSuccess: function(o) { // your code here }});
//-->
</script>
<p><span class="aspnetmaker"><%= Language.Phrase("Add") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= Article.TableCaption %><br /><br />
<a href="<%= Article.ReturnUrl %>"><%= Language.Phrase("GoBack") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(Article_add.DebugMsg) %>
<% Article_add.ShowMessage() %>
<form name="fArticleadd" id="fArticleadd" method="post" onsubmit="this.action=location.pathname;return Article_add.ValidateForm(this);">
<p />
<input type="hidden" name="t" id="t" value="Article" />
<input type="hidden" name="a_add" id="a_add" value="A" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
<% If Article.Active.Visible Then ' Active %>
	<tr<%= Article.RowAttributes %>>
		<td class="ewTableHeader"><%= Article.Active.FldCaption %></td>
		<td<%= Article.Active.CellAttributes %>><span id="el_Active">
<%
If ew_SameStr(Article.Active.CurrentValue, "1") Then
	selwrk = " checked=""checked"""
Else
	selwrk = ""
End If
%>
<input type="checkbox" name="x_Active" id="x_Active" title="<%= Article.Active.FldTitle %>" value="1"<%= selwrk %><%= Article.Active.EditAttributes %> />
</span><%= Article.Active.CustomMsg %></td>
	</tr>
<% End If %>
<% If Article.Title.Visible Then ' Title %>
	<tr<%= Article.RowAttributes %>>
		<td class="ewTableHeader"><%= Article.Title.FldCaption %><%= Language.Phrase("FieldRequiredIndicator") %></td>
		<td<%= Article.Title.CellAttributes %>><span id="el_Title">
<input type="text" name="x_Title" id="x_Title" title="<%= Article.Title.FldTitle %>" size="70" maxlength="255" value="<%= Article.Title.EditValue %>"<%= Article.Title.EditAttributes %> />
</span><%= Article.Title.CustomMsg %></td>
	</tr>
<% End If %>
<% If Article.CompanyID.Visible Then ' CompanyID %>
	<tr<%= Article.RowAttributes %>>
		<td class="ewTableHeader"><%= Article.CompanyID.FldCaption %></td>
		<td<%= Article.CompanyID.CellAttributes %>><span id="el_CompanyID">
<% If Article.CompanyID.SessionValue <> "" Then %>
<div<%= Article.CompanyID.ViewAttributes %>><%= Article.CompanyID.ViewValue %></div>
<input type="hidden" id="x_CompanyID" name="x_CompanyID" value="<%= ew_HtmlEncode(Article.CompanyID.CurrentValue) %>">
<% Else %>
<% Article.CompanyID.EditAttrs("onchange") = "ew_UpdateOpt('x_zPageID','x_CompanyID',Article_add.ar_x_zPageID);ew_UpdateOpt('x_userID','x_CompanyID',Article_add.ar_x_userID);ew_UpdateOpt('x_Author','x_CompanyID',Article_add.ar_x_Author);" %>
<select id="x_CompanyID" name="x_CompanyID" title="<%= Article.CompanyID.FldTitle %>"<%= Article.CompanyID.EditAttributes %>>
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
<% End If %>
</span><%= Article.CompanyID.CustomMsg %></td>
	</tr>
<% End If %>
<% If Article.zPageID.Visible Then ' PageID %>
	<tr<%= Article.RowAttributes %>>
		<td class="ewTableHeader"><%= Article.zPageID.FldCaption %></td>
		<td<%= Article.zPageID.CellAttributes %>><span id="el_zPageID">
<% If Article.zPageID.SessionValue <> "" Then %>
<div<%= Article.zPageID.ViewAttributes %>><%= Article.zPageID.ViewValue %></div>
<input type="hidden" id="x_zPageID" name="x_zPageID" value="<%= ew_HtmlEncode(Article.zPageID.CurrentValue) %>">
<% Else %>
<select id="x_zPageID" name="x_zPageID" title="<%= Article.zPageID.FldTitle %>"<%= Article.zPageID.EditAttributes %>>
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
<% End If %>
</span><%= Article.zPageID.CustomMsg %></td>
	</tr>
<% End If %>
<% If Article.userID.Visible Then ' userID %>
	<tr<%= Article.RowAttributes %>>
		<td class="ewTableHeader"><%= Article.userID.FldCaption %></td>
		<td<%= Article.userID.CellAttributes %>><span id="el_userID">
<select id="x_userID" name="x_userID" title="<%= Article.userID.FldTitle %>"<%= Article.userID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Article.userID.EditValue) Then
	arwrk = Article.userID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Article.userID.CurrentValue) Then
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
If ew_IsArrayList(Article.userID.EditValue) Then
	arwrk = Article.userID.EditValue
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
Article_add.ar_x_userID = [<%= jswrk %>];
//-->
</script>
</span><%= Article.userID.CustomMsg %></td>
	</tr>
<% End If %>
<% If Article.Description.Visible Then ' Description %>
	<tr<%= Article.RowAttributes %>>
		<td class="ewTableHeader"><%= Article.Description.FldCaption %></td>
		<td<%= Article.Description.CellAttributes %>><span id="el_Description">
<textarea name="x_Description" id="x_Description" title="<%= Article.Description.FldTitle %>" cols="70" rows="3"<%= Article.Description.EditAttributes %>><%= Article.Description.EditValue %></textarea>
</span><%= Article.Description.CustomMsg %></td>
	</tr>
<% End If %>
<% If Article.ArticleSummary.Visible Then ' ArticleSummary %>
	<tr<%= Article.RowAttributes %>>
		<td class="ewTableHeader"><%= Article.ArticleSummary.FldCaption %></td>
		<td<%= Article.ArticleSummary.CellAttributes %>><span id="el_ArticleSummary">
<textarea name="x_ArticleSummary" id="x_ArticleSummary" title="<%= Article.ArticleSummary.FldTitle %>" cols="50" rows="3"<%= Article.ArticleSummary.EditAttributes %>><%= Article.ArticleSummary.EditValue %></textarea>
<script type="text/javascript">
<!--
ew_DHTMLEditors.push(new ew_DHTMLEditor("x_ArticleSummary", function() {
	var sBasePath = 'fckeditor/';
	var oFCKeditor = new FCKeditor('x_ArticleSummary', 50*_width_multiplier, 3*_height_multiplier);
	oFCKeditor.BasePath = sBasePath;
	oFCKeditor.ReplaceTextarea();
	this.active = true;
}));
-->
</script>
</span><%= Article.ArticleSummary.CustomMsg %></td>
	</tr>
<% End If %>
<% If Article.ArticleBody.Visible Then ' ArticleBody %>
	<tr<%= Article.RowAttributes %>>
		<td class="ewTableHeader"><%= Article.ArticleBody.FldCaption %></td>
		<td<%= Article.ArticleBody.CellAttributes %>><span id="el_ArticleBody">
<textarea name="x_ArticleBody" id="x_ArticleBody" title="<%= Article.ArticleBody.FldTitle %>" cols="50" rows="20"<%= Article.ArticleBody.EditAttributes %>><%= Article.ArticleBody.EditValue %></textarea>
<script type="text/javascript">
<!--
ew_DHTMLEditors.push(new ew_DHTMLEditor("x_ArticleBody", function() {
	var sBasePath = 'fckeditor/';
	var oFCKeditor = new FCKeditor('x_ArticleBody', 50*_width_multiplier, 20*_height_multiplier);
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
	<tr<%= Article.RowAttributes %>>
		<td class="ewTableHeader"><%= Article.EndDT.FldCaption %></td>
		<td<%= Article.EndDT.CellAttributes %>><span id="el_EndDT">
<input type="text" name="x_EndDT" id="x_EndDT" title="<%= Article.EndDT.FldTitle %>" value="<%= Article.EndDT.EditValue %>"<%= Article.EndDT.EditAttributes %> />
&nbsp;<img src="images/calendar.png" id="cal_x_EndDT" name="cal_x_EndDT" alt="<%= Language.Phrase("PickDate") %>" title="<%= Language.Phrase("PickDate") %>" style="cursor:pointer;cursor:hand;" />
<script type="text/javascript">
Calendar.setup({
	inputField: "x_EndDT", // input field id
	ifFormat: "%m/%d/%Y", // date format
	button: "cal_x_EndDT" // button id
});
</script>
</span><%= Article.EndDT.CustomMsg %></td>
	</tr>
<% End If %>
<% If Article.ExpireDT.Visible Then ' ExpireDT %>
	<tr<%= Article.RowAttributes %>>
		<td class="ewTableHeader"><%= Article.ExpireDT.FldCaption %></td>
		<td<%= Article.ExpireDT.CellAttributes %>><span id="el_ExpireDT">
<input type="text" name="x_ExpireDT" id="x_ExpireDT" title="<%= Article.ExpireDT.FldTitle %>" value="<%= Article.ExpireDT.EditValue %>"<%= Article.ExpireDT.EditAttributes %> />
&nbsp;<img src="images/calendar.png" id="cal_x_ExpireDT" name="cal_x_ExpireDT" alt="<%= Language.Phrase("PickDate") %>" title="<%= Language.Phrase("PickDate") %>" style="cursor:pointer;cursor:hand;" />
<script type="text/javascript">
Calendar.setup({
	inputField: "x_ExpireDT", // input field id
	ifFormat: "%m/%d/%Y", // date format
	button: "cal_x_ExpireDT" // button id
});
</script>
</span><%= Article.ExpireDT.CustomMsg %></td>
	</tr>
<% End If %>
<% If Article.Author.Visible Then ' Author %>
	<tr<%= Article.RowAttributes %>>
		<td class="ewTableHeader"><%= Article.Author.FldCaption %></td>
		<td<%= Article.Author.CellAttributes %>><span id="el_Author">
<select id="x_Author" name="x_Author" title="<%= Article.Author.FldTitle %>"<%= Article.Author.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Article.Author.EditValue) Then
	arwrk = Article.Author.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Article.Author.CurrentValue) Then
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
If ew_IsArrayList(Article.Author.EditValue) Then
	arwrk = Article.Author.EditValue
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
Article_add.ar_x_Author = [<%= jswrk %>];
//-->
</script>
</span><%= Article.Author.CustomMsg %></td>
	</tr>
<% End If %>
<input type="hidden" name="x_Counter" id="x_Counter" value="<%= ew_HTMLEncode(Article.Counter.CurrentValue) %>" />
<input type="hidden" name="x_VersionNo" id="x_VersionNo" value="<%= ew_HTMLEncode(Article.VersionNo.CurrentValue) %>" />
<input type="hidden" name="x_ContactID" id="x_ContactID" value="<%= ew_HTMLEncode(Article.ContactID.CurrentValue) %>" />
</table>
</div>
</td></tr></table>
<p />
<input type="submit" name="btnAction" id="btnAction" value="<%= ew_BtnCaption(Language.Phrase("AddBtn")) %>" />
</form>
<script language="JavaScript" type="text/javascript">
<!--
ew_UpdateOpts([['x_zPageID','x_CompanyID',Article_add.ar_x_zPageID],['x_userID','x_CompanyID',Article_add.ar_x_userID],['x_Author','x_CompanyID',Article_add.ar_x_Author]]);
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
