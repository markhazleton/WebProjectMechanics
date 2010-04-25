<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="SiteLink_srch.aspx.vb" Inherits="SiteLink_srch" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var SiteLink_search = new ew_Page("SiteLink_search");
// page properties
SiteLink_search.PageID = "search"; // page ID
var EW_PAGE_ID = SiteLink_search.PageID; // for backward compatibility
// extend page with validate function for search
SiteLink_search.ValidateSearch = function(fobj) {
	if (!this.ValidateRequired)
		return true; // ignore validation
	var infix = "";
	elm = fobj.elements["x" + infix + "_Ranks"];
	if (elm && !ew_CheckInteger(elm.value))
		return ew_OnError(this, elm, "Incorrect integer - Ranks");
	for (var i=0;i<fobj.elements.length;i++) {
		var elem = fobj.elements[i];
		if (elem.name.substring(0,2) == "s_" || elem.name.substring(0,3) == "sv_")
			elem.value = "";
	}
	return true;
}
<% If EW_CLIENT_VALIDATE Then %>
SiteLink_search.ValidateRequired = true; // uses JavaScript validation
<% Else %>
SiteLink_search.ValidateRequired = false; // no JavaScript validation
<% End If %>
//-->
</script>
<script type="text/javascript">
<!--
var ew_DHTMLEditors = [];
//-->
</script>
<script language="JavaScript" type="text/javascript">
<!--
// Write your client script here, no need to add script tags.
// To include another .js script, use:
// ew_ClientScriptInclude("my_javascript.js"); 
//-->
</script>
<p><span class="aspnetmaker">Search TABLE: Site Type Parts<br /><br />
<a href="<%= SiteLink.ReturnUrl %>">Back to List</a></span></p>
<% SiteLink_search.ShowMessage() %>
<form name="fSiteLinksearch" id="fSiteLinksearch" method="post" onsubmit="this.action=location.pathname;return SiteLink_search.ValidateSearch(this);">
<p />
<input type="hidden" name="t" id="t" value="SiteLink" />
<input type="hidden" name="a_search" id="a_search" value="S" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
	<tr<%= SiteLink.SiteCategoryTypeID.RowAttributes %>>
		<td class="ewTableHeader">Site Type</td>
		<td<%= SiteLink.SiteCategoryTypeID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_SiteCategoryTypeID" id="z_SiteCategoryTypeID" value="=" /></span></td>
		<td<%= SiteLink.SiteCategoryTypeID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_SiteCategoryTypeID" name="x_SiteCategoryTypeID" onchange="ew_UpdateOpt('x_SiteCategoryID','x_SiteCategoryTypeID',SiteLink_search.ar_x_SiteCategoryID);"<%= SiteLink.SiteCategoryTypeID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(SiteLink.SiteCategoryTypeID.EditValue) Then
	arwrk = SiteLink.SiteCategoryTypeID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), SiteLink.SiteCategoryTypeID.AdvancedSearch.SearchValue) Then
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
	<tr<%= SiteLink.Title.RowAttributes %>>
		<td class="ewTableHeader">Title</td>
		<td<%= SiteLink.Title.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_Title" id="z_Title" value="LIKE" /></span></td>
		<td<%= SiteLink.Title.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_Title" id="x_Title" size="30" maxlength="255" value="<%= SiteLink.Title.EditValue %>"<%= SiteLink.Title.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteLink.LinkTypeCD.RowAttributes %>>
		<td class="ewTableHeader">Link Type</td>
		<td<%= SiteLink.LinkTypeCD.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_LinkTypeCD" id="z_LinkTypeCD" value="=" /></span></td>
		<td<%= SiteLink.LinkTypeCD.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_LinkTypeCD" name="x_LinkTypeCD"<%= SiteLink.LinkTypeCD.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(SiteLink.LinkTypeCD.EditValue) Then
	arwrk = SiteLink.LinkTypeCD.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), SiteLink.LinkTypeCD.AdvancedSearch.SearchValue) Then
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
	<tr<%= SiteLink.CategoryID.RowAttributes %>>
		<td class="ewTableHeader">Link Category</td>
		<td<%= SiteLink.CategoryID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_CategoryID" id="z_CategoryID" value="=" /></span></td>
		<td<%= SiteLink.CategoryID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_CategoryID" name="x_CategoryID"<%= SiteLink.CategoryID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(SiteLink.CategoryID.EditValue) Then
	arwrk = SiteLink.CategoryID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), SiteLink.CategoryID.AdvancedSearch.SearchValue) Then
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
	<tr<%= SiteLink.CompanyID.RowAttributes %>>
		<td class="ewTableHeader">Company</td>
		<td<%= SiteLink.CompanyID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_CompanyID" id="z_CompanyID" value="=" /></span></td>
		<td<%= SiteLink.CompanyID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_CompanyID" name="x_CompanyID"<%= SiteLink.CompanyID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(SiteLink.CompanyID.EditValue) Then
	arwrk = SiteLink.CompanyID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), SiteLink.CompanyID.AdvancedSearch.SearchValue) Then
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
	<tr<%= SiteLink.SiteCategoryID.RowAttributes %>>
		<td class="ewTableHeader">Site Category</td>
		<td<%= SiteLink.SiteCategoryID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_SiteCategoryID" id="z_SiteCategoryID" value="=" /></span></td>
		<td<%= SiteLink.SiteCategoryID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_SiteCategoryID" name="x_SiteCategoryID"<%= SiteLink.SiteCategoryID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(SiteLink.SiteCategoryID.EditValue) Then
	arwrk = SiteLink.SiteCategoryID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), SiteLink.SiteCategoryID.AdvancedSearch.SearchValue) Then
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
SiteLink_search.ar_x_SiteCategoryID = [<%= jswrk %>];
//-->
</script>
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteLink.SiteCategoryGroupID.RowAttributes %>>
		<td class="ewTableHeader">Category Group</td>
		<td<%= SiteLink.SiteCategoryGroupID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_SiteCategoryGroupID" id="z_SiteCategoryGroupID" value="=" /></span></td>
		<td<%= SiteLink.SiteCategoryGroupID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_SiteCategoryGroupID" name="x_SiteCategoryGroupID"<%= SiteLink.SiteCategoryGroupID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(SiteLink.SiteCategoryGroupID.EditValue) Then
	arwrk = SiteLink.SiteCategoryGroupID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), SiteLink.SiteCategoryGroupID.AdvancedSearch.SearchValue) Then
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
	<tr<%= SiteLink.Ranks.RowAttributes %>>
		<td class="ewTableHeader">Ranks</td>
		<td<%= SiteLink.Ranks.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_Ranks" id="z_Ranks" value="=" /></span></td>
		<td<%= SiteLink.Ranks.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_Ranks" id="x_Ranks" size="30" value="<%= SiteLink.Ranks.EditValue %>"<%= SiteLink.Ranks.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteLink.Views.RowAttributes %>>
		<td class="ewTableHeader">Active/Visible</td>
		<td<%= SiteLink.Views.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_Views" id="z_Views" value="=" /></span></td>
		<td<%= SiteLink.Views.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<%
If ew_SameStr(SiteLink.Views.AdvancedSearch.SearchValue, "1") Then
	selwrk = " checked=""checked"""
Else
	selwrk = ""
End If
%>
<input type="checkbox" name="x_Views" id="x_Views" value="1"<%= selwrk %><%= SiteLink.Views.EditAttributes %> />
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
ew_UpdateOpts([['x_SiteCategoryID','x_SiteCategoryTypeID',SiteLink_search.ar_x_SiteCategoryID]]);
//-->
</script>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
</asp:Content>
