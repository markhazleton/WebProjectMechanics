<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="SiteCategory_srch.aspx.vb" Inherits="SiteCategory_srch" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var SiteCategory_search = new ew_Page("SiteCategory_search");
// page properties
SiteCategory_search.PageID = "search"; // page ID
var EW_PAGE_ID = SiteCategory_search.PageID; // for backward compatibility
// extend page with validate function for search
SiteCategory_search.ValidateSearch = function(fobj) {
	if (!this.ValidateRequired)
		return true; // ignore validation
	var infix = "";
	elm = fobj.elements["x" + infix + "_GroupOrder"];
	if (elm && !ew_CheckNumber(elm.value))
		return ew_OnError(this, elm, "Incorrect floating point number - Order");
	for (var i=0;i<fobj.elements.length;i++) {
		var elem = fobj.elements[i];
		if (elem.name.substring(0,2) == "s_" || elem.name.substring(0,3) == "sv_")
			elem.value = "";
	}
	return true;
}
<% If EW_CLIENT_VALIDATE Then %>
SiteCategory_search.ValidateRequired = true; // uses JavaScript validation
<% Else %>
SiteCategory_search.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">Search TABLE: Site Type Location<br /><br />
<a href="<%= SiteCategory.ReturnUrl %>">Back to List</a></span></p>
<% SiteCategory_search.ShowMessage() %>
<form name="fSiteCategorysearch" id="fSiteCategorysearch" method="post" onsubmit="this.action=location.pathname;return SiteCategory_search.ValidateSearch(this);">
<p />
<input type="hidden" name="t" id="t" value="SiteCategory" />
<input type="hidden" name="a_search" id="a_search" value="S" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
	<tr<%= SiteCategory.SiteCategoryTypeID.RowAttributes %>>
		<td class="ewTableHeader">Site Type</td>
		<td<%= SiteCategory.SiteCategoryTypeID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_SiteCategoryTypeID" id="z_SiteCategoryTypeID" value="=" /></span></td>
		<td<%= SiteCategory.SiteCategoryTypeID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_SiteCategoryTypeID" name="x_SiteCategoryTypeID" onchange="ew_UpdateOpt('x_ParentCategoryID','x_SiteCategoryTypeID',SiteCategory_search.ar_x_ParentCategoryID);"<%= SiteCategory.SiteCategoryTypeID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(SiteCategory.SiteCategoryTypeID.EditValue) Then
	arwrk = SiteCategory.SiteCategoryTypeID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), SiteCategory.SiteCategoryTypeID.AdvancedSearch.SearchValue) Then
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
	<tr<%= SiteCategory.GroupOrder.RowAttributes %>>
		<td class="ewTableHeader">Order</td>
		<td<%= SiteCategory.GroupOrder.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_GroupOrder" id="z_GroupOrder" value="=" /></span></td>
		<td<%= SiteCategory.GroupOrder.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_GroupOrder" id="x_GroupOrder" size="30" value="<%= SiteCategory.GroupOrder.EditValue %>"<%= SiteCategory.GroupOrder.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteCategory.CategoryName.RowAttributes %>>
		<td class="ewTableHeader">Name</td>
		<td<%= SiteCategory.CategoryName.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_CategoryName" id="z_CategoryName" value="LIKE" /></span></td>
		<td<%= SiteCategory.CategoryName.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_CategoryName" id="x_CategoryName" size="50" maxlength="255" value="<%= SiteCategory.CategoryName.EditValue %>"<%= SiteCategory.CategoryName.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteCategory.CategoryTitle.RowAttributes %>>
		<td class="ewTableHeader">Title</td>
		<td<%= SiteCategory.CategoryTitle.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_CategoryTitle" id="z_CategoryTitle" value="LIKE" /></span></td>
		<td<%= SiteCategory.CategoryTitle.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<textarea name="x_CategoryTitle" id="x_CategoryTitle" cols="50" rows="5"<%= SiteCategory.CategoryTitle.EditAttributes %>><%= SiteCategory.CategoryTitle.EditValue %></textarea>
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteCategory.CategoryDescription.RowAttributes %>>
		<td class="ewTableHeader">Description</td>
		<td<%= SiteCategory.CategoryDescription.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_CategoryDescription" id="z_CategoryDescription" value="LIKE" /></span></td>
		<td<%= SiteCategory.CategoryDescription.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<textarea name="x_CategoryDescription" id="x_CategoryDescription" cols="50" rows="5"<%= SiteCategory.CategoryDescription.EditAttributes %>><%= SiteCategory.CategoryDescription.EditValue %></textarea>
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteCategory.CategoryKeywords.RowAttributes %>>
		<td class="ewTableHeader">Keywords</td>
		<td<%= SiteCategory.CategoryKeywords.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_CategoryKeywords" id="z_CategoryKeywords" value="LIKE" /></span></td>
		<td<%= SiteCategory.CategoryKeywords.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<textarea name="x_CategoryKeywords" id="x_CategoryKeywords" cols="50" rows="5"<%= SiteCategory.CategoryKeywords.EditAttributes %>><%= SiteCategory.CategoryKeywords.EditValue %></textarea>
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteCategory.ParentCategoryID.RowAttributes %>>
		<td class="ewTableHeader">Parent Category</td>
		<td<%= SiteCategory.ParentCategoryID.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_ParentCategoryID" id="z_ParentCategoryID" value="LIKE" /></span></td>
		<td<%= SiteCategory.ParentCategoryID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_ParentCategoryID" name="x_ParentCategoryID"<%= SiteCategory.ParentCategoryID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(SiteCategory.ParentCategoryID.EditValue) Then
	arwrk = SiteCategory.ParentCategoryID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), SiteCategory.ParentCategoryID.AdvancedSearch.SearchValue) Then
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
If ew_IsArrayList(SiteCategory.ParentCategoryID.EditValue) Then
	arwrk = SiteCategory.ParentCategoryID.EditValue
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
SiteCategory_search.ar_x_ParentCategoryID = [<%= jswrk %>];
//-->
</script>
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteCategory.CategoryFileName.RowAttributes %>>
		<td class="ewTableHeader">Category File Name</td>
		<td<%= SiteCategory.CategoryFileName.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_CategoryFileName" id="z_CategoryFileName" value="LIKE" /></span></td>
		<td<%= SiteCategory.CategoryFileName.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_CategoryFileName" id="x_CategoryFileName" size="30" maxlength="255" value="<%= SiteCategory.CategoryFileName.EditValue %>"<%= SiteCategory.CategoryFileName.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteCategory.SiteCategoryGroupID.RowAttributes %>>
		<td class="ewTableHeader">Site Group</td>
		<td<%= SiteCategory.SiteCategoryGroupID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_SiteCategoryGroupID" id="z_SiteCategoryGroupID" value="=" /></span></td>
		<td<%= SiteCategory.SiteCategoryGroupID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_SiteCategoryGroupID" name="x_SiteCategoryGroupID"<%= SiteCategory.SiteCategoryGroupID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(SiteCategory.SiteCategoryGroupID.EditValue) Then
	arwrk = SiteCategory.SiteCategoryGroupID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), SiteCategory.SiteCategoryGroupID.AdvancedSearch.SearchValue) Then
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
ew_UpdateOpts([['x_ParentCategoryID','x_SiteCategoryTypeID',SiteCategory_search.ar_x_ParentCategoryID]]);
//-->
</script>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
</asp:Content>
