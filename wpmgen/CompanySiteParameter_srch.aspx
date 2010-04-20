<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="CompanySiteParameter_srch.aspx.vb" Inherits="CompanySiteParameter_srch" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var CompanySiteParameter_search = new ew_Page("CompanySiteParameter_search");
// page properties
CompanySiteParameter_search.PageID = "search"; // page ID
var EW_PAGE_ID = CompanySiteParameter_search.PageID; // for backward compatibility
// extend page with validate function for search
CompanySiteParameter_search.ValidateSearch = function(fobj) {
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
CompanySiteParameter_search.ValidateRequired = true; // uses JavaScript validation
<% Else %>
CompanySiteParameter_search.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">Search TABLE: Site Parameter<br /><br />
<a href="<%= CompanySiteParameter.ReturnUrl %>">Back to List</a></span></p>
<% CompanySiteParameter_search.ShowMessage() %>
<form name="fCompanySiteParametersearch" id="fCompanySiteParametersearch" method="post" onsubmit="this.action=location.pathname;return CompanySiteParameter_search.ValidateSearch(this);">
<p />
<input type="hidden" name="t" id="t" value="CompanySiteParameter" />
<input type="hidden" name="a_search" id="a_search" value="S" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
	<tr<%= CompanySiteParameter.CompanyID.RowAttributes %>>
		<td class="ewTableHeader">Site</td>
		<td<%= CompanySiteParameter.CompanyID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_CompanyID" id="z_CompanyID" value="=" /></span></td>
		<td<%= CompanySiteParameter.CompanyID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_CompanyID" name="x_CompanyID" onchange="ew_UpdateOpt('x_zPageID','x_CompanyID',CompanySiteParameter_search.ar_x_zPageID);"<%= CompanySiteParameter.CompanyID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(CompanySiteParameter.CompanyID.EditValue) Then
	arwrk = CompanySiteParameter.CompanyID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), CompanySiteParameter.CompanyID.AdvancedSearch.SearchValue) Then
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
	<tr<%= CompanySiteParameter.zPageID.RowAttributes %>>
		<td class="ewTableHeader">Page</td>
		<td<%= CompanySiteParameter.zPageID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_zPageID" id="z_zPageID" value="=" /></span></td>
		<td<%= CompanySiteParameter.zPageID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_zPageID" name="x_zPageID"<%= CompanySiteParameter.zPageID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(CompanySiteParameter.zPageID.EditValue) Then
	arwrk = CompanySiteParameter.zPageID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), CompanySiteParameter.zPageID.AdvancedSearch.SearchValue) Then
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
If ew_IsArrayList(CompanySiteParameter.zPageID.EditValue) Then
	arwrk = CompanySiteParameter.zPageID.EditValue
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
CompanySiteParameter_search.ar_x_zPageID = [<%= jswrk %>];
//-->
</script>
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= CompanySiteParameter.SiteCategoryGroupID.RowAttributes %>>
		<td class="ewTableHeader">Category Group</td>
		<td<%= CompanySiteParameter.SiteCategoryGroupID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_SiteCategoryGroupID" id="z_SiteCategoryGroupID" value="=" /></span></td>
		<td<%= CompanySiteParameter.SiteCategoryGroupID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_SiteCategoryGroupID" name="x_SiteCategoryGroupID"<%= CompanySiteParameter.SiteCategoryGroupID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(CompanySiteParameter.SiteCategoryGroupID.EditValue) Then
	arwrk = CompanySiteParameter.SiteCategoryGroupID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), CompanySiteParameter.SiteCategoryGroupID.AdvancedSearch.SearchValue) Then
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
	<tr<%= CompanySiteParameter.SiteParameterTypeID.RowAttributes %>>
		<td class="ewTableHeader"> Parameter</td>
		<td<%= CompanySiteParameter.SiteParameterTypeID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_SiteParameterTypeID" id="z_SiteParameterTypeID" value="=" /></span></td>
		<td<%= CompanySiteParameter.SiteParameterTypeID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_SiteParameterTypeID" name="x_SiteParameterTypeID"<%= CompanySiteParameter.SiteParameterTypeID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(CompanySiteParameter.SiteParameterTypeID.EditValue) Then
	arwrk = CompanySiteParameter.SiteParameterTypeID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), CompanySiteParameter.SiteParameterTypeID.AdvancedSearch.SearchValue) Then
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
ew_UpdateOpts([['x_zPageID','x_CompanyID',CompanySiteParameter_search.ar_x_zPageID]]);
//-->
</script>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
</asp:Content>
