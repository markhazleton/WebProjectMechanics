<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="CompanySiteTypeParameter_srch.aspx.vb" Inherits="CompanySiteTypeParameter_srch" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var CompanySiteTypeParameter_search = new ew_Page("CompanySiteTypeParameter_search");
// page properties
CompanySiteTypeParameter_search.PageID = "search"; // page ID
var EW_PAGE_ID = CompanySiteTypeParameter_search.PageID; // for backward compatibility
// extend page with validate function for search
CompanySiteTypeParameter_search.ValidateSearch = function(fobj) {
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
CompanySiteTypeParameter_search.ValidateRequired = true; // uses JavaScript validation
<% Else %>
CompanySiteTypeParameter_search.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">Search TABLE: Site Type Parameter<br /><br />
<a href="<%= CompanySiteTypeParameter.ReturnUrl %>">Back to List</a></span></p>
<% CompanySiteTypeParameter_search.ShowMessage() %>
<form name="fCompanySiteTypeParametersearch" id="fCompanySiteTypeParametersearch" method="post" onsubmit="this.action=location.pathname;return CompanySiteTypeParameter_search.ValidateSearch(this);">
<p />
<input type="hidden" name="t" id="t" value="CompanySiteTypeParameter" />
<input type="hidden" name="a_search" id="a_search" value="S" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
	<tr<%= CompanySiteTypeParameter.SiteParameterTypeID.RowAttributes %>>
		<td class="ewTableHeader">Parameter</td>
		<td<%= CompanySiteTypeParameter.SiteParameterTypeID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_SiteParameterTypeID" id="z_SiteParameterTypeID" value="=" /></span></td>
		<td<%= CompanySiteTypeParameter.SiteParameterTypeID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_SiteParameterTypeID" name="x_SiteParameterTypeID"<%= CompanySiteTypeParameter.SiteParameterTypeID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(CompanySiteTypeParameter.SiteParameterTypeID.EditValue) Then
	arwrk = CompanySiteTypeParameter.SiteParameterTypeID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), CompanySiteTypeParameter.SiteParameterTypeID.AdvancedSearch.SearchValue) Then
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
	<tr<%= CompanySiteTypeParameter.CompanyID.RowAttributes %>>
		<td class="ewTableHeader">Site</td>
		<td<%= CompanySiteTypeParameter.CompanyID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_CompanyID" id="z_CompanyID" value="=" /></span></td>
		<td<%= CompanySiteTypeParameter.CompanyID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_CompanyID" name="x_CompanyID"<%= CompanySiteTypeParameter.CompanyID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(CompanySiteTypeParameter.CompanyID.EditValue) Then
	arwrk = CompanySiteTypeParameter.CompanyID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), CompanySiteTypeParameter.CompanyID.AdvancedSearch.SearchValue) Then
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
	<tr<%= CompanySiteTypeParameter.SiteCategoryTypeID.RowAttributes %>>
		<td class="ewTableHeader">Site Type</td>
		<td<%= CompanySiteTypeParameter.SiteCategoryTypeID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_SiteCategoryTypeID" id="z_SiteCategoryTypeID" value="=" /></span></td>
		<td<%= CompanySiteTypeParameter.SiteCategoryTypeID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_SiteCategoryTypeID" name="x_SiteCategoryTypeID" onchange="ew_UpdateOpt('x_SiteCategoryID','x_SiteCategoryTypeID',CompanySiteTypeParameter_search.ar_x_SiteCategoryID);"<%= CompanySiteTypeParameter.SiteCategoryTypeID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(CompanySiteTypeParameter.SiteCategoryTypeID.EditValue) Then
	arwrk = CompanySiteTypeParameter.SiteCategoryTypeID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), CompanySiteTypeParameter.SiteCategoryTypeID.AdvancedSearch.SearchValue) Then
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
	<tr<%= CompanySiteTypeParameter.SiteCategoryGroupID.RowAttributes %>>
		<td class="ewTableHeader">Site Group</td>
		<td<%= CompanySiteTypeParameter.SiteCategoryGroupID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_SiteCategoryGroupID" id="z_SiteCategoryGroupID" value="=" /></span></td>
		<td<%= CompanySiteTypeParameter.SiteCategoryGroupID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_SiteCategoryGroupID" name="x_SiteCategoryGroupID"<%= CompanySiteTypeParameter.SiteCategoryGroupID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(CompanySiteTypeParameter.SiteCategoryGroupID.EditValue) Then
	arwrk = CompanySiteTypeParameter.SiteCategoryGroupID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), CompanySiteTypeParameter.SiteCategoryGroupID.AdvancedSearch.SearchValue) Then
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
	<tr<%= CompanySiteTypeParameter.SiteCategoryID.RowAttributes %>>
		<td class="ewTableHeader">Site Category</td>
		<td<%= CompanySiteTypeParameter.SiteCategoryID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_SiteCategoryID" id="z_SiteCategoryID" value="=" /></span></td>
		<td<%= CompanySiteTypeParameter.SiteCategoryID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_SiteCategoryID" name="x_SiteCategoryID"<%= CompanySiteTypeParameter.SiteCategoryID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(CompanySiteTypeParameter.SiteCategoryID.EditValue) Then
	arwrk = CompanySiteTypeParameter.SiteCategoryID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), CompanySiteTypeParameter.SiteCategoryID.AdvancedSearch.SearchValue) Then
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
CompanySiteTypeParameter_search.ar_x_SiteCategoryID = [<%= jswrk %>];
//-->
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
ew_UpdateOpts([['x_SiteCategoryID','x_SiteCategoryTypeID',CompanySiteTypeParameter_search.ar_x_SiteCategoryID]]);
//-->
</script>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
</asp:Content>
