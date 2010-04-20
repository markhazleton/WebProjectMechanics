<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="SiteCategoryType_srch.aspx.vb" Inherits="SiteCategoryType_srch" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var SiteCategoryType_search = new ew_Page("SiteCategoryType_search");
// page properties
SiteCategoryType_search.PageID = "search"; // page ID
var EW_PAGE_ID = SiteCategoryType_search.PageID; // for backward compatibility
// extend page with validate function for search
SiteCategoryType_search.ValidateSearch = function(fobj) {
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
SiteCategoryType_search.ValidateRequired = true; // uses JavaScript validation
<% Else %>
SiteCategoryType_search.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">Search TABLE: Site Type<br /><br />
<a href="<%= SiteCategoryType.ReturnUrl %>">Back to List</a></span></p>
<% SiteCategoryType_search.ShowMessage() %>
<form name="fSiteCategoryTypesearch" id="fSiteCategoryTypesearch" method="post" onsubmit="this.action=location.pathname;return SiteCategoryType_search.ValidateSearch(this);">
<p />
<input type="hidden" name="t" id="t" value="SiteCategoryType" />
<input type="hidden" name="a_search" id="a_search" value="S" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
	<tr<%= SiteCategoryType.SiteCategoryTypeNM.RowAttributes %>>
		<td class="ewTableHeader">Site Type</td>
		<td<%= SiteCategoryType.SiteCategoryTypeNM.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_SiteCategoryTypeNM" id="z_SiteCategoryTypeNM" value="LIKE" /></span></td>
		<td<%= SiteCategoryType.SiteCategoryTypeNM.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_SiteCategoryTypeNM" id="x_SiteCategoryTypeNM" size="30" maxlength="255" value="<%= SiteCategoryType.SiteCategoryTypeNM.EditValue %>"<%= SiteCategoryType.SiteCategoryTypeNM.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteCategoryType.SiteCategoryTypeDS.RowAttributes %>>
		<td class="ewTableHeader">Description</td>
		<td<%= SiteCategoryType.SiteCategoryTypeDS.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_SiteCategoryTypeDS" id="z_SiteCategoryTypeDS" value="LIKE" /></span></td>
		<td<%= SiteCategoryType.SiteCategoryTypeDS.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_SiteCategoryTypeDS" id="x_SiteCategoryTypeDS" size="30" maxlength="255" value="<%= SiteCategoryType.SiteCategoryTypeDS.EditValue %>"<%= SiteCategoryType.SiteCategoryTypeDS.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteCategoryType.SiteCategoryComment.RowAttributes %>>
		<td class="ewTableHeader">Comment</td>
		<td<%= SiteCategoryType.SiteCategoryComment.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_SiteCategoryComment" id="z_SiteCategoryComment" value="LIKE" /></span></td>
		<td<%= SiteCategoryType.SiteCategoryComment.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_SiteCategoryComment" id="x_SiteCategoryComment" size="30" maxlength="255" value="<%= SiteCategoryType.SiteCategoryComment.EditValue %>"<%= SiteCategoryType.SiteCategoryComment.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteCategoryType.SiteCategoryFileName.RowAttributes %>>
		<td class="ewTableHeader">File Name</td>
		<td<%= SiteCategoryType.SiteCategoryFileName.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_SiteCategoryFileName" id="z_SiteCategoryFileName" value="LIKE" /></span></td>
		<td<%= SiteCategoryType.SiteCategoryFileName.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_SiteCategoryFileName" id="x_SiteCategoryFileName" size="30" maxlength="255" value="<%= SiteCategoryType.SiteCategoryFileName.EditValue %>"<%= SiteCategoryType.SiteCategoryFileName.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteCategoryType.SiteCategoryTransferURL.RowAttributes %>>
		<td class="ewTableHeader">Transfer URL</td>
		<td<%= SiteCategoryType.SiteCategoryTransferURL.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_SiteCategoryTransferURL" id="z_SiteCategoryTransferURL" value="LIKE" /></span></td>
		<td<%= SiteCategoryType.SiteCategoryTransferURL.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_SiteCategoryTransferURL" id="x_SiteCategoryTransferURL" size="30" maxlength="255" value="<%= SiteCategoryType.SiteCategoryTransferURL.EditValue %>"<%= SiteCategoryType.SiteCategoryTransferURL.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteCategoryType.DefaultSiteCategoryID.RowAttributes %>>
		<td class="ewTableHeader">Default Category</td>
		<td<%= SiteCategoryType.DefaultSiteCategoryID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_DefaultSiteCategoryID" id="z_DefaultSiteCategoryID" value="=" /></span></td>
		<td<%= SiteCategoryType.DefaultSiteCategoryID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_DefaultSiteCategoryID" name="x_DefaultSiteCategoryID"<%= SiteCategoryType.DefaultSiteCategoryID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(SiteCategoryType.DefaultSiteCategoryID.EditValue) Then
	arwrk = SiteCategoryType.DefaultSiteCategoryID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), SiteCategoryType.DefaultSiteCategoryID.AdvancedSearch.SearchValue) Then
			selwrk = " selected=""selected"""
			emptywrk = False
		Else
			selwrk = ""
		End If
%>
<option value="<%= ew_HtmlEncode(arwrk(rowcntwrk)(0)) %>"<%= selwrk %>>
<%= arwrk(rowcntwrk)(1) %>
<% If ew_NotEmpty(arwrk(rowcntwrk)(2)) Then %>
<%= ew_ValueSeparator(rowcntwrk) %><%= arwrk(rowcntwrk)(2) %>
<% End If %>
</option>
<%
	Next
End If
%>
</select>
<%
sSqlWrk = "SELECT [SiteCategoryID], [CategoryName], [SiteCategoryTypeID] FROM [SiteCategory]"
sWhereWrk = ""
If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryTypeID] Asc"
sSqlWrk = cTEA.Encrypt(sSqlWrk, EW_RANDOM_KEY)
%>
<input type="hidden" name="s_x_DefaultSiteCategoryID" id="s_x_DefaultSiteCategoryID" value="<%= sSqlWrk %>">
<input type="hidden" name="lft_x_DefaultSiteCategoryID" id="lft_x_DefaultSiteCategoryID" value="">
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
ew_UpdateOpts([['x_DefaultSiteCategoryID','x_DefaultSiteCategoryID',false]]);
//-->
</script>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
</asp:Content>
