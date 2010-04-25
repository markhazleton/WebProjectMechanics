<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="Article_srch.aspx.vb" Inherits="Article_srch" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var Article_search = new ew_Page("Article_search");
// page properties
Article_search.PageID = "search"; // page ID
var EW_PAGE_ID = Article_search.PageID; // for backward compatibility
// extend page with validate function for search
Article_search.ValidateSearch = function(fobj) {
	if (!this.ValidateRequired)
		return true; // ignore validation
	var infix = "";
	for (var i=0;i<fobj.elements.length;i++) {
		var elem = fobj.elements[i];
		if (elem.name.substring(0,2) == "s_" || elem.name.substring(0,3) == "sv_")
			elem.value = "";
	}
	return true;
}
Article_search.SelectAllKey = function(elem) {
	ew_SelectAll(elem);
}
<% If EW_CLIENT_VALIDATE Then %>
Article_search.ValidateRequired = true; // uses JavaScript validation
<% Else %>
Article_search.ValidateRequired = false; // no JavaScript validation
<% End If %>
//-->
</script>
<script type="text/javascript">
<!--
var ew_DHTMLEditors = [];
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
<p><span class="aspnetmaker">Search TABLE: Site Article<br /><br />
<a href="<%= Article.ReturnUrl %>">Back to List</a></span></p>
<% Article_search.ShowMessage() %>
<form name="fArticlesearch" id="fArticlesearch" method="post" onsubmit="this.action=location.pathname;return Article_search.ValidateSearch(this);">
<p />
<input type="hidden" name="t" id="t" value="Article" />
<input type="hidden" name="a_search" id="a_search" value="S" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
	<tr<%= Article.Active.RowAttributes %>>
		<td class="ewTableHeader">Active</td>
		<td<%= Article.Active.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_Active" id="z_Active" value="=" /></span></td>
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
<input type="checkbox" name="x_Active" id="x_Active" value="1"<%= selwrk %><%= Article.Active.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Article.Title.RowAttributes %>>
		<td class="ewTableHeader">Title</td>
		<td<%= Article.Title.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_Title" id="z_Title" value="LIKE" /></span></td>
		<td<%= Article.Title.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_Title" id="x_Title" size="80" maxlength="255" value="<%= Article.Title.EditValue %>"<%= Article.Title.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Article.CompanyID.RowAttributes %>>
		<td class="ewTableHeader">Site</td>
		<td<%= Article.CompanyID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_CompanyID" id="z_CompanyID" value="=" /></span></td>
		<td<%= Article.CompanyID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_CompanyID" name="x_CompanyID" onchange="ew_UpdateOpt('x_zPageID','x_CompanyID',Article_search.ar_x_zPageID);ew_UpdateOpt('x_ContactID','x_CompanyID',Article_search.ar_x_ContactID);"<%= Article.CompanyID.EditAttributes %>>
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
	<tr<%= Article.zPageID.RowAttributes %>>
		<td class="ewTableHeader">Location</td>
		<td<%= Article.zPageID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_zPageID" id="z_zPageID" value="=" /></span></td>
		<td<%= Article.zPageID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_zPageID" name="x_zPageID"<%= Article.zPageID.EditAttributes %>>
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
	<tr<%= Article.ContactID.RowAttributes %>>
		<td class="ewTableHeader">Contact</td>
		<td<%= Article.ContactID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_ContactID" id="z_ContactID" value="=" /></span></td>
		<td<%= Article.ContactID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_ContactID" name="x_ContactID"<%= Article.ContactID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Article.ContactID.EditValue) Then
	arwrk = Article.ContactID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Article.ContactID.AdvancedSearch.SearchValue) Then
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
Article_search.ar_x_ContactID = [<%= jswrk %>];
//-->
</script>
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Article.Description.RowAttributes %>>
		<td class="ewTableHeader">Description</td>
		<td<%= Article.Description.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_Description" id="z_Description" value="LIKE" /></span></td>
		<td<%= Article.Description.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<textarea name="x_Description" id="x_Description" cols="80" rows="2"<%= Article.Description.EditAttributes %>><%= Article.Description.EditValue %></textarea>
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Article.ModifiedDT.RowAttributes %>>
		<td class="ewTableHeader">Modified DT</td>
		<td<%= Article.ModifiedDT.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_ModifiedDT" id="z_ModifiedDT" value="=" /></span></td>
		<td<%= Article.ModifiedDT.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_ModifiedDT" id="x_ModifiedDT" value="<%= Article.ModifiedDT.EditValue %>"<%= Article.ModifiedDT.EditAttributes %> />
&nbsp;<img src="images/calendar.png" id="cal_x_ModifiedDT" name="cal_x_ModifiedDT" alt="Pick a date" style="cursor:pointer;cursor:hand;" />
<script type="text/javascript">
Calendar.setup({
	inputField : "x_ModifiedDT", // ID of the input field
	ifFormat : "%m/%d/%Y", // the date format
	button : "cal_x_ModifiedDT" // ID of the button
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
<input type="submit" name="Action" id="Action" value="  Search  " />
<input type="button" name="Reset" id="Reset" value="   Reset   " onclick="ew_ClearForm(this.form);" />
</form>
<script language="JavaScript" type="text/javascript">
<!--
ew_UpdateOpts([['x_zPageID','x_CompanyID',Article_search.ar_x_zPageID],['x_ContactID','x_CompanyID',Article_search.ar_x_ContactID]]);
//-->
</script>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
</asp:Content>
