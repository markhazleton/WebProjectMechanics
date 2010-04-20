<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="PageAlias_srch.aspx.vb" Inherits="PageAlias_srch" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var PageAlias_search = new ew_Page("PageAlias_search");
// page properties
PageAlias_search.PageID = "search"; // page ID
var EW_PAGE_ID = PageAlias_search.PageID; // for backward compatibility
// extend page with validate function for search
PageAlias_search.ValidateSearch = function(fobj) {
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
PageAlias_search.ValidateRequired = true; // uses JavaScript validation
<% Else %>
PageAlias_search.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">Search TABLE: Location Alias<br /><br />
<a href="<%= PageAlias.ReturnUrl %>">Back to List</a></span></p>
<% PageAlias_search.ShowMessage() %>
<form name="fPageAliassearch" id="fPageAliassearch" method="post" onsubmit="this.action=location.pathname;return PageAlias_search.ValidateSearch(this);">
<p />
<input type="hidden" name="t" id="t" value="PageAlias" />
<input type="hidden" name="a_search" id="a_search" value="S" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
	<tr<%= PageAlias.zPageURL.RowAttributes %>>
		<td class="ewTableHeader">Page URL</td>
		<td<%= PageAlias.zPageURL.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_zPageURL" id="z_zPageURL" value="LIKE" /></span></td>
		<td<%= PageAlias.zPageURL.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_zPageURL" id="x_zPageURL" size="30" maxlength="255" value="<%= PageAlias.zPageURL.EditValue %>"<%= PageAlias.zPageURL.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= PageAlias.TargetURL.RowAttributes %>>
		<td class="ewTableHeader">Target URL</td>
		<td<%= PageAlias.TargetURL.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_TargetURL" id="z_TargetURL" value="LIKE" /></span></td>
		<td<%= PageAlias.TargetURL.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_TargetURL" id="x_TargetURL" size="30" maxlength="255" value="<%= PageAlias.TargetURL.EditValue %>"<%= PageAlias.TargetURL.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= PageAlias.AliasType.RowAttributes %>>
		<td class="ewTableHeader">Alias Type</td>
		<td<%= PageAlias.AliasType.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_AliasType" id="z_AliasType" value="LIKE" /></span></td>
		<td<%= PageAlias.AliasType.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_AliasType" id="x_AliasType" size="30" maxlength="10" value="<%= PageAlias.AliasType.EditValue %>"<%= PageAlias.AliasType.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= PageAlias.CompanyID.RowAttributes %>>
		<td class="ewTableHeader">Company</td>
		<td<%= PageAlias.CompanyID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_CompanyID" id="z_CompanyID" value="=" /></span></td>
		<td<%= PageAlias.CompanyID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_CompanyID" name="x_CompanyID"<%= PageAlias.CompanyID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(PageAlias.CompanyID.EditValue) Then
	arwrk = PageAlias.CompanyID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), PageAlias.CompanyID.AdvancedSearch.SearchValue) Then
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
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
</asp:Content>
