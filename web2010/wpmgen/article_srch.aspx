<%@ Page ClassName="article_srch" Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="article_srch.aspx.vb" Inherits="article_srch" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var Article_search = new ew_Page("Article_search");
// page properties
Article_search.PageID = "search"; // page ID
Article_search.FormID = "fArticlesearch"; // form ID 
var EW_PAGE_ID = Article_search.PageID; // for backward compatibility
// extend page with validate function for search
Article_search.ValidateSearch = function(fobj) {
	ew_PostAutoSuggest(fobj); 
	if (this.ValidateRequired) { 
		var infix = "";
		elm = fobj.elements["x" + infix + "_StartDT"];
		if (elm && elm.type != "hidden" && !ew_CheckUSDate(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(Article.StartDT.FldErrMsg) %>");
		elm = fobj.elements["x" + infix + "_EndDT"];
		if (elm && elm.type != "hidden" && !ew_CheckUSDate(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(Article.EndDT.FldErrMsg) %>");
		elm = fobj.elements["x" + infix + "_ExpireDT"];
		if (elm && elm.type != "hidden" && !ew_CheckUSDate(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(Article.ExpireDT.FldErrMsg) %>");
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
Article_search.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
Article_search.ValidateRequired = true; // uses JavaScript validation
<% Else %>
Article_search.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker"><%= Language.Phrase("Search") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= Article.TableCaption %><br /><br />
<a href="<%= Article.ReturnUrl %>"><%= Language.Phrase("BackToList") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(Article_search.DebugMsg) %>
<% Article_search.ShowMessage() %>
<form name="fArticlesearch" id="fArticlesearch" method="post" onsubmit="this.action=location.pathname;return Article_search.ValidateSearch(this);">
<p />
<input type="hidden" name="t" id="t" value="Article" />
<input type="hidden" name="a_search" id="a_search" value="S" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
	<tr<%= Article.RowAttributes %>>
		<td class="ewTableHeader"><%= Article.ArticleID.FldCaption %></td>
		<td<%= Article.ArticleID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_ArticleID" id="z_ArticleID" value="=" /></span></td>
		<td<%= Article.ArticleID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_ArticleID" id="x_ArticleID" title="<%= Article.ArticleID.FldTitle %>" value="<%= Article.ArticleID.EditValue %>"<%= Article.ArticleID.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Article.RowAttributes %>>
		<td class="ewTableHeader"><%= Article.Active.FldCaption %></td>
		<td<%= Article.Active.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_Active" id="z_Active" value="=" /></span></td>
		<td<%= Article.Active.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<%
If ew_SameStr(Article.Active.AdvancedSearch.SearchValue, "1") Then
	selwrk = " checked=""checked"""
Else
	selwrk = ""
End If
%>
<input type="checkbox" name="x_Active" id="x_Active" title="<%= Article.Active.FldTitle %>" value="1"<%= selwrk %><%= Article.Active.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Article.RowAttributes %>>
		<td class="ewTableHeader"><%= Article.StartDT.FldCaption %></td>
		<td<%= Article.StartDT.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_StartDT" id="z_StartDT" value="=" /></span></td>
		<td<%= Article.StartDT.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_StartDT" id="x_StartDT" title="<%= Article.StartDT.FldTitle %>" value="<%= Article.StartDT.EditValue %>"<%= Article.StartDT.EditAttributes %> />
&nbsp;<img src="images/calendar.png" id="cal_x_StartDT" name="cal_x_StartDT" alt="<%= Language.Phrase("PickDate") %>" title="<%= Language.Phrase("PickDate") %>" style="cursor:pointer;cursor:hand;" />
<script type="text/javascript">
Calendar.setup({
	inputField: "x_StartDT", // input field id
	ifFormat: "%m/%d/%Y", // date format
	button: "cal_x_StartDT" // button id
});
</script>
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Article.RowAttributes %>>
		<td class="ewTableHeader"><%= Article.Title.FldCaption %></td>
		<td<%= Article.Title.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_Title" id="z_Title" value="LIKE" /></span></td>
		<td<%= Article.Title.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_Title" id="x_Title" title="<%= Article.Title.FldTitle %>" size="70" maxlength="255" value="<%= Article.Title.EditValue %>"<%= Article.Title.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Article.RowAttributes %>>
		<td class="ewTableHeader"><%= Article.CompanyID.FldCaption %></td>
		<td<%= Article.CompanyID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_CompanyID" id="z_CompanyID" value="=" /></span></td>
		<td<%= Article.CompanyID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<% Article.CompanyID.EditAttrs("onchange") = "ew_UpdateOpt('x_zPageID','x_CompanyID',Article_search.ar_x_zPageID);ew_UpdateOpt('x_userID','x_CompanyID',Article_search.ar_x_userID);ew_UpdateOpt('x_Author','x_CompanyID',Article_search.ar_x_Author);" %>
<select id="x_CompanyID" name="x_CompanyID" title="<%= Article.CompanyID.FldTitle %>"<%= Article.CompanyID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Article.CompanyID.EditValue) Then
	arwrk = Article.CompanyID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Article.CompanyID.AdvancedSearch.SearchValue) Then
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
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Article.RowAttributes %>>
		<td class="ewTableHeader"><%= Article.zPageID.FldCaption %></td>
		<td<%= Article.zPageID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_zPageID" id="z_zPageID" value="=" /></span></td>
		<td<%= Article.zPageID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_zPageID" name="x_zPageID" title="<%= Article.zPageID.FldTitle %>"<%= Article.zPageID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Article.zPageID.EditValue) Then
	arwrk = Article.zPageID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Article.zPageID.AdvancedSearch.SearchValue) Then
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
Article_search.ar_x_zPageID = [<%= jswrk %>];
//-->
</script>
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Article.RowAttributes %>>
		<td class="ewTableHeader"><%= Article.userID.FldCaption %></td>
		<td<%= Article.userID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_userID" id="z_userID" value="=" /></span></td>
		<td<%= Article.userID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_userID" name="x_userID" title="<%= Article.userID.FldTitle %>"<%= Article.userID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Article.userID.EditValue) Then
	arwrk = Article.userID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Article.userID.AdvancedSearch.SearchValue) Then
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
Article_search.ar_x_userID = [<%= jswrk %>];
//-->
</script>
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Article.RowAttributes %>>
		<td class="ewTableHeader"><%= Article.Description.FldCaption %></td>
		<td<%= Article.Description.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_Description" id="z_Description" value="LIKE" /></span></td>
		<td<%= Article.Description.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<textarea name="x_Description" id="x_Description" title="<%= Article.Description.FldTitle %>" cols="70" rows="3"<%= Article.Description.EditAttributes %>><%= Article.Description.EditValue %></textarea>
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Article.RowAttributes %>>
		<td class="ewTableHeader"><%= Article.ArticleSummary.FldCaption %></td>
		<td<%= Article.ArticleSummary.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_ArticleSummary" id="z_ArticleSummary" value="LIKE" /></span></td>
		<td<%= Article.ArticleSummary.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
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
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Article.RowAttributes %>>
		<td class="ewTableHeader"><%= Article.ArticleBody.FldCaption %></td>
		<td<%= Article.ArticleBody.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_ArticleBody" id="z_ArticleBody" value="LIKE" /></span></td>
		<td<%= Article.ArticleBody.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
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
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Article.RowAttributes %>>
		<td class="ewTableHeader"><%= Article.EndDT.FldCaption %></td>
		<td<%= Article.EndDT.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_EndDT" id="z_EndDT" value="=" /></span></td>
		<td<%= Article.EndDT.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_EndDT" id="x_EndDT" title="<%= Article.EndDT.FldTitle %>" value="<%= Article.EndDT.EditValue %>"<%= Article.EndDT.EditAttributes %> />
&nbsp;<img src="images/calendar.png" id="cal_x_EndDT" name="cal_x_EndDT" alt="<%= Language.Phrase("PickDate") %>" title="<%= Language.Phrase("PickDate") %>" style="cursor:pointer;cursor:hand;" />
<script type="text/javascript">
Calendar.setup({
	inputField: "x_EndDT", // input field id
	ifFormat: "%m/%d/%Y", // date format
	button: "cal_x_EndDT" // button id
});
</script>
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Article.RowAttributes %>>
		<td class="ewTableHeader"><%= Article.ExpireDT.FldCaption %></td>
		<td<%= Article.ExpireDT.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_ExpireDT" id="z_ExpireDT" value="=" /></span></td>
		<td<%= Article.ExpireDT.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_ExpireDT" id="x_ExpireDT" title="<%= Article.ExpireDT.FldTitle %>" value="<%= Article.ExpireDT.EditValue %>"<%= Article.ExpireDT.EditAttributes %> />
&nbsp;<img src="images/calendar.png" id="cal_x_ExpireDT" name="cal_x_ExpireDT" alt="<%= Language.Phrase("PickDate") %>" title="<%= Language.Phrase("PickDate") %>" style="cursor:pointer;cursor:hand;" />
<script type="text/javascript">
Calendar.setup({
	inputField: "x_ExpireDT", // input field id
	ifFormat: "%m/%d/%Y", // date format
	button: "cal_x_ExpireDT" // button id
});
</script>
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Article.RowAttributes %>>
		<td class="ewTableHeader"><%= Article.Author.FldCaption %></td>
		<td<%= Article.Author.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_Author" id="z_Author" value="LIKE" /></span></td>
		<td<%= Article.Author.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_Author" name="x_Author" title="<%= Article.Author.FldTitle %>"<%= Article.Author.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Article.Author.EditValue) Then
	arwrk = Article.Author.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Article.Author.AdvancedSearch.SearchValue) Then
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
Article_search.ar_x_Author = [<%= jswrk %>];
//-->
</script>
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Article.RowAttributes %>>
		<td class="ewTableHeader"><%= Article.Counter.FldCaption %></td>
		<td<%= Article.Counter.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_Counter" id="z_Counter" value="=" /></span></td>
		<td<%= Article.Counter.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_Counter" id="x_Counter" title="<%= Article.Counter.FldTitle %>" size="30" value="<%= Article.Counter.EditValue %>"<%= Article.Counter.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Article.RowAttributes %>>
		<td class="ewTableHeader"><%= Article.VersionNo.FldCaption %></td>
		<td<%= Article.VersionNo.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_VersionNo" id="z_VersionNo" value="=" /></span></td>
		<td<%= Article.VersionNo.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_VersionNo" id="x_VersionNo" title="<%= Article.VersionNo.FldTitle %>" size="30" value="<%= Article.VersionNo.EditValue %>"<%= Article.VersionNo.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Article.RowAttributes %>>
		<td class="ewTableHeader"><%= Article.ContactID.FldCaption %></td>
		<td<%= Article.ContactID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_ContactID" id="z_ContactID" value="=" /></span></td>
		<td<%= Article.ContactID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_ContactID" id="x_ContactID" title="<%= Article.ContactID.FldTitle %>" size="30" value="<%= Article.ContactID.EditValue %>"<%= Article.ContactID.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Article.RowAttributes %>>
		<td class="ewTableHeader"><%= Article.ModifiedDT.FldCaption %></td>
		<td<%= Article.ModifiedDT.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_ModifiedDT" id="z_ModifiedDT" value="=" /></span></td>
		<td<%= Article.ModifiedDT.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_ModifiedDT" id="x_ModifiedDT" title="<%= Article.ModifiedDT.FldTitle %>" value="<%= Article.ModifiedDT.EditValue %>"<%= Article.ModifiedDT.EditAttributes %> />
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
ew_UpdateOpts([['x_zPageID','x_CompanyID',Article_search.ar_x_zPageID],['x_userID','x_CompanyID',Article_search.ar_x_userID],['x_Author','x_CompanyID',Article_search.ar_x_Author]]);
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
