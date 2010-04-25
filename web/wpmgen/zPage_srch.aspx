<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="zPage_srch.aspx.vb" Inherits="zPage_srch" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var zPage_search = new ew_Page("zPage_search");
// page properties
zPage_search.PageID = "search"; // page ID
var EW_PAGE_ID = zPage_search.PageID; // for backward compatibility
// extend page with validate function for search
zPage_search.ValidateSearch = function(fobj) {
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
<% If EW_CLIENT_VALIDATE Then %>
zPage_search.ValidateRequired = true; // uses JavaScript validation
<% Else %>
zPage_search.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">Search TABLE: Site Location<br /><br />
<a href="<%= zPage.ReturnUrl %>">Back to List</a></span></p>
<% zPage_search.ShowMessage() %>
<form name="fzPagesearch" id="fzPagesearch" method="post" onsubmit="this.action=location.pathname;return zPage_search.ValidateSearch(this);">
<p />
<input type="hidden" name="t" id="t" value="zPage" />
<input type="hidden" name="a_search" id="a_search" value="S" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
	<tr<%= zPage.CompanyID.RowAttributes %>>
		<td class="ewTableHeader">Company</td>
		<td<%= zPage.CompanyID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_CompanyID" id="z_CompanyID" value="=" /></span></td>
		<td<%= zPage.CompanyID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_CompanyID" name="x_CompanyID" onchange="ew_UpdateOpt('x_ParentPageID','x_CompanyID',zPage_search.ar_x_ParentPageID);"<%= zPage.CompanyID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(zPage.CompanyID.EditValue) Then
	arwrk = zPage.CompanyID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), zPage.CompanyID.AdvancedSearch.SearchValue) Then
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
	<tr<%= zPage.GroupID.RowAttributes %>>
		<td class="ewTableHeader">Group</td>
		<td<%= zPage.GroupID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_GroupID" id="z_GroupID" value="=" /></span></td>
		<td<%= zPage.GroupID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_GroupID" name="x_GroupID"<%= zPage.GroupID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(zPage.GroupID.EditValue) Then
	arwrk = zPage.GroupID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), zPage.GroupID.AdvancedSearch.SearchValue) Then
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
	<tr<%= zPage.ParentPageID.RowAttributes %>>
		<td class="ewTableHeader">Parent Page</td>
		<td<%= zPage.ParentPageID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_ParentPageID" id="z_ParentPageID" value="=" /></span></td>
		<td<%= zPage.ParentPageID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_ParentPageID" name="x_ParentPageID"<%= zPage.ParentPageID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(zPage.ParentPageID.EditValue) Then
	arwrk = zPage.ParentPageID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), zPage.ParentPageID.AdvancedSearch.SearchValue) Then
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
If ew_IsArrayList(zPage.ParentPageID.EditValue) Then
	arwrk = zPage.ParentPageID.EditValue
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
zPage_search.ar_x_ParentPageID = [<%= jswrk %>];
//-->
</script>
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= zPage.PageTypeID.RowAttributes %>>
		<td class="ewTableHeader">PageType</td>
		<td<%= zPage.PageTypeID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_PageTypeID" id="z_PageTypeID" value="=" /></span></td>
		<td<%= zPage.PageTypeID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_PageTypeID" name="x_PageTypeID"<%= zPage.PageTypeID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(zPage.PageTypeID.EditValue) Then
	arwrk = zPage.PageTypeID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), zPage.PageTypeID.AdvancedSearch.SearchValue) Then
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
	<tr<%= zPage.Active.RowAttributes %>>
		<td class="ewTableHeader">Active</td>
		<td<%= zPage.Active.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_Active" id="z_Active" value="=" /></span></td>
		<td<%= zPage.Active.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<%
If ew_SameStr(zPage.Active.AdvancedSearch.SearchValue, "1") Then
	selwrk = " checked=""checked"""
Else
	selwrk = ""
End If
%>
<input type="checkbox" name="x_Active" id="x_Active" value="1"<%= selwrk %><%= zPage.Active.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= zPage.zPageName.RowAttributes %>>
		<td class="ewTableHeader">Page Name</td>
		<td<%= zPage.zPageName.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_zPageName" id="z_zPageName" value="LIKE" /></span></td>
		<td<%= zPage.zPageName.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_zPageName" id="x_zPageName" size="60" maxlength="60" value="<%= zPage.zPageName.EditValue %>"<%= zPage.zPageName.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= zPage.PageTitle.RowAttributes %>>
		<td class="ewTableHeader">Page Title</td>
		<td<%= zPage.PageTitle.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_PageTitle" id="z_PageTitle" value="LIKE" /></span></td>
		<td<%= zPage.PageTitle.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_PageTitle" id="x_PageTitle" size="60" maxlength="255" value="<%= zPage.PageTitle.EditValue %>"<%= zPage.PageTitle.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= zPage.PageDescription.RowAttributes %>>
		<td class="ewTableHeader">Page Description</td>
		<td<%= zPage.PageDescription.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_PageDescription" id="z_PageDescription" value="LIKE" /></span></td>
		<td<%= zPage.PageDescription.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_PageDescription" id="x_PageDescription" size="60" maxlength="255" value="<%= zPage.PageDescription.EditValue %>"<%= zPage.PageDescription.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= zPage.PageKeywords.RowAttributes %>>
		<td class="ewTableHeader">Page Keywords</td>
		<td<%= zPage.PageKeywords.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_PageKeywords" id="z_PageKeywords" value="LIKE" /></span></td>
		<td<%= zPage.PageKeywords.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_PageKeywords" id="x_PageKeywords" size="60" maxlength="255" value="<%= zPage.PageKeywords.EditValue %>"<%= zPage.PageKeywords.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= zPage.SiteCategoryID.RowAttributes %>>
		<td class="ewTableHeader">Site Category</td>
		<td<%= zPage.SiteCategoryID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_SiteCategoryID" id="z_SiteCategoryID" value="=" /></span></td>
		<td<%= zPage.SiteCategoryID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_SiteCategoryID" name="x_SiteCategoryID"<%= zPage.SiteCategoryID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(zPage.SiteCategoryID.EditValue) Then
	arwrk = zPage.SiteCategoryID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), zPage.SiteCategoryID.AdvancedSearch.SearchValue) Then
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
	<tr<%= zPage.SiteCategoryGroupID.RowAttributes %>>
		<td class="ewTableHeader">Site Category Group</td>
		<td<%= zPage.SiteCategoryGroupID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_SiteCategoryGroupID" id="z_SiteCategoryGroupID" value="=" /></span></td>
		<td<%= zPage.SiteCategoryGroupID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_SiteCategoryGroupID" name="x_SiteCategoryGroupID"<%= zPage.SiteCategoryGroupID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(zPage.SiteCategoryGroupID.EditValue) Then
	arwrk = zPage.SiteCategoryGroupID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), zPage.SiteCategoryGroupID.AdvancedSearch.SearchValue) Then
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
</table>
</div>
</td></tr></table>
<p />
<input type="submit" name="Action" id="Action" value="  Search  " />
<input type="button" name="Reset" id="Reset" value="   Reset   " onclick="ew_ClearForm(this.form);" />
</form>
<script language="JavaScript" type="text/javascript">
<!--
ew_UpdateOpts([['x_ParentPageID','x_CompanyID',zPage_search.ar_x_ParentPageID]]);
//-->
</script>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
</asp:Content>
