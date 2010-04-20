<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="Link_srch.aspx.vb" Inherits="Link_srch" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var Link_search = new ew_Page("Link_search");
// page properties
Link_search.PageID = "search"; // page ID
var EW_PAGE_ID = Link_search.PageID; // for backward compatibility
// extend page with validate function for search
Link_search.ValidateSearch = function(fobj) {
	if (!this.ValidateRequired)
		return true; // ignore validation
	var infix = "";
	elm = fobj.elements["x" + infix + "_Ranks"];
	if (elm && !ew_CheckInteger(elm.value))
		return ew_OnError(this, elm, "Incorrect integer - Ranks");
	elm = fobj.elements["x" + infix + "_DateAdd"];
	if (elm && !ew_CheckUSDate(elm.value))
		return ew_OnError(this, elm, "Incorrect date, format = mm/dd/yyyy - Date Add");
	for (var i=0;i<fobj.elements.length;i++) {
		var elem = fobj.elements[i];
		if (elem.name.substring(0,2) == "s_" || elem.name.substring(0,3) == "sv_")
			elem.value = "";
	}
	return true;
}
<% If EW_CLIENT_VALIDATE Then %>
Link_search.ValidateRequired = true; // uses JavaScript validation
<% Else %>
Link_search.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">Search TABLE: Site Parts<br /><br />
<a href="<%= Link.ReturnUrl %>">Back to List</a></span></p>
<% Link_search.ShowMessage() %>
<form name="fLinksearch" id="fLinksearch" method="post">
<p />
<input type="hidden" name="t" id="t" value="Link" />
<input type="hidden" name="a_search" id="a_search" value="S" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
	<tr<%= Link.Title.RowAttributes %>>
		<td class="ewTableHeader">Title</td>
		<td<%= Link.Title.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_Title" id="z_Title" value="LIKE" /></span></td>
		<td<%= Link.Title.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_Title" id="x_Title" size="30" maxlength="255" value="<%= Link.Title.EditValue %>"<%= Link.Title.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Link.LinkTypeCD.RowAttributes %>>
		<td class="ewTableHeader">Part Type</td>
		<td<%= Link.LinkTypeCD.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_LinkTypeCD" id="z_LinkTypeCD" value="LIKE" /></span></td>
		<td<%= Link.LinkTypeCD.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_LinkTypeCD" name="x_LinkTypeCD"<%= Link.LinkTypeCD.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Link.LinkTypeCD.EditValue) Then
	arwrk = Link.LinkTypeCD.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Link.LinkTypeCD.AdvancedSearch.SearchValue) Then
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
	<tr<%= Link.CategoryID.RowAttributes %>>
		<td class="ewTableHeader">Part Category</td>
		<td<%= Link.CategoryID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_CategoryID" id="z_CategoryID" value="=" /></span></td>
		<td<%= Link.CategoryID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_CategoryID" name="x_CategoryID"<%= Link.CategoryID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Link.CategoryID.EditValue) Then
	arwrk = Link.CategoryID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Link.CategoryID.AdvancedSearch.SearchValue) Then
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
	<tr<%= Link.CompanyID.RowAttributes %>>
		<td class="ewTableHeader">Company</td>
		<td<%= Link.CompanyID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_CompanyID" id="z_CompanyID" value="=" /></span></td>
		<td<%= Link.CompanyID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_CompanyID" name="x_CompanyID" onchange="ew_UpdateOpt('x_zPageID','x_CompanyID',Link_search.ar_x_zPageID);ew_UpdateOpt('x_UserID','x_CompanyID',Link_search.ar_x_UserID);"<%= Link.CompanyID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Link.CompanyID.EditValue) Then
	arwrk = Link.CompanyID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Link.CompanyID.AdvancedSearch.SearchValue) Then
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
	<tr<%= Link.SiteCategoryGroupID.RowAttributes %>>
		<td class="ewTableHeader">Location Group</td>
		<td<%= Link.SiteCategoryGroupID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_SiteCategoryGroupID" id="z_SiteCategoryGroupID" value="=" /></span></td>
		<td<%= Link.SiteCategoryGroupID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_SiteCategoryGroupID" name="x_SiteCategoryGroupID"<%= Link.SiteCategoryGroupID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Link.SiteCategoryGroupID.EditValue) Then
	arwrk = Link.SiteCategoryGroupID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Link.SiteCategoryGroupID.AdvancedSearch.SearchValue) Then
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
	<tr<%= Link.zPageID.RowAttributes %>>
		<td class="ewTableHeader">Location</td>
		<td<%= Link.zPageID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_zPageID" id="z_zPageID" value="=" /></span></td>
		<td<%= Link.zPageID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_zPageID" name="x_zPageID"<%= Link.zPageID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Link.zPageID.EditValue) Then
	arwrk = Link.zPageID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Link.zPageID.AdvancedSearch.SearchValue) Then
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
Link_search.ar_x_zPageID = [<%= jswrk %>];
//-->
</script>
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Link.Views.RowAttributes %>>
		<td class="ewTableHeader">Visible/Active</td>
		<td<%= Link.Views.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_Views" id="z_Views" value="=" /></span></td>
		<td<%= Link.Views.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<%
If ew_SameStr(Link.Views.AdvancedSearch.SearchValue, "1") Then
	selwrk = " checked=""checked"""
Else
	selwrk = ""
End If
%>
<input type="checkbox" name="x_Views" id="x_Views" value="1"<%= selwrk %><%= Link.Views.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Link.Description.RowAttributes %>>
		<td class="ewTableHeader">Description</td>
		<td<%= Link.Description.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_Description" id="z_Description" value="LIKE" /></span></td>
		<td<%= Link.Description.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<textarea name="x_Description" id="x_Description" cols="80" rows="5"<%= Link.Description.EditAttributes %>><%= Link.Description.EditValue %></textarea>
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Link.URL.RowAttributes %>>
		<td class="ewTableHeader">URL</td>
		<td<%= Link.URL.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_URL" id="z_URL" value="LIKE" /></span></td>
		<td<%= Link.URL.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
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
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Link.Ranks.RowAttributes %>>
		<td class="ewTableHeader">Ranks</td>
		<td<%= Link.Ranks.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_Ranks" id="z_Ranks" value="=" /></span></td>
		<td<%= Link.Ranks.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_Ranks" id="x_Ranks" size="30" value="<%= Link.Ranks.EditValue %>"<%= Link.Ranks.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Link.UserID.RowAttributes %>>
		<td class="ewTableHeader">User</td>
		<td<%= Link.UserID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_UserID" id="z_UserID" value="=" /></span></td>
		<td<%= Link.UserID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_UserID" name="x_UserID"<%= Link.UserID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Link.UserID.EditValue) Then
	arwrk = Link.UserID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Link.UserID.AdvancedSearch.SearchValue) Then
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
Link_search.ar_x_UserID = [<%= jswrk %>];
//-->
</script>
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Link.ASIN.RowAttributes %>>
		<td class="ewTableHeader">ASIN (Amazon)</td>
		<td<%= Link.ASIN.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_ASIN" id="z_ASIN" value="LIKE" /></span></td>
		<td<%= Link.ASIN.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_ASIN" id="x_ASIN" size="60" maxlength="50" value="<%= Link.ASIN.EditValue %>"<%= Link.ASIN.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Link.DateAdd.RowAttributes %>>
		<td class="ewTableHeader">Date Add</td>
		<td<%= Link.DateAdd.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_DateAdd" id="z_DateAdd" value="=" /></span></td>
		<td<%= Link.DateAdd.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_DateAdd" id="x_DateAdd" value="<%= Link.DateAdd.EditValue %>"<%= Link.DateAdd.EditAttributes %> />
&nbsp;<img src="images/calendar.png" id="cal_x_DateAdd" name="cal_x_DateAdd" alt="Pick a date" style="cursor:pointer;cursor:hand;" />
<script type="text/javascript">
Calendar.setup({
	inputField : "x_DateAdd", // ID of the input field
	ifFormat : "%m/%d/%Y", // the date format
	button : "cal_x_DateAdd" // ID of the button
});
</script>
</span></td>
			</tr></table>
		</td>
	</tr>
</table>
</div>
</td></tr></table>
<p />
<input type="button" name="Action" id="Action" value="  Search  " onclick="ew_SubmitSearch(Link_search, this.form);" />
<input type="button" name="Reset" id="Reset" value="   Reset   " onclick="ew_ClearForm(this.form);" />
</form>
<script language="JavaScript" type="text/javascript">
<!--
ew_UpdateOpts([['x_zPageID','x_CompanyID',Link_search.ar_x_zPageID],['x_UserID','x_CompanyID',Link_search.ar_x_UserID]]);
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
