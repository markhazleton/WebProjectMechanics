<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="LinkCategory_srch.aspx.vb" Inherits="LinkCategory_srch" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var LinkCategory_search = new ew_Page("LinkCategory_search");
// page properties
LinkCategory_search.PageID = "search"; // page ID
var EW_PAGE_ID = LinkCategory_search.PageID; // for backward compatibility
// extend page with validate function for search
LinkCategory_search.ValidateSearch = function(fobj) {
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
LinkCategory_search.ValidateRequired = true; // uses JavaScript validation
<% Else %>
LinkCategory_search.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">Search TABLE: Part Category<br /><br />
<a href="<%= LinkCategory.ReturnUrl %>">Back to List</a></span></p>
<% LinkCategory_search.ShowMessage() %>
<form name="fLinkCategorysearch" id="fLinkCategorysearch" method="post" onsubmit="this.action=location.pathname;return LinkCategory_search.ValidateSearch(this);">
<p />
<input type="hidden" name="t" id="t" value="LinkCategory" />
<input type="hidden" name="a_search" id="a_search" value="S" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
	<tr<%= LinkCategory.Title.RowAttributes %>>
		<td class="ewTableHeader">Title</td>
		<td<%= LinkCategory.Title.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_Title" id="z_Title" value="LIKE" /></span></td>
		<td<%= LinkCategory.Title.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_Title" id="x_Title" size="30" maxlength="50" value="<%= LinkCategory.Title.EditValue %>"<%= LinkCategory.Title.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= LinkCategory.Description.RowAttributes %>>
		<td class="ewTableHeader">Description</td>
		<td<%= LinkCategory.Description.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_Description" id="z_Description" value="LIKE" /></span></td>
		<td<%= LinkCategory.Description.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<textarea name="x_Description" id="x_Description" cols="35" rows="4"<%= LinkCategory.Description.EditAttributes %>><%= LinkCategory.Description.EditValue %></textarea>
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= LinkCategory.ParentID.RowAttributes %>>
		<td class="ewTableHeader">Parent</td>
		<td<%= LinkCategory.ParentID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_ParentID" id="z_ParentID" value="=" /></span></td>
		<td<%= LinkCategory.ParentID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_ParentID" name="x_ParentID"<%= LinkCategory.ParentID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(LinkCategory.ParentID.EditValue) Then
	arwrk = LinkCategory.ParentID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), LinkCategory.ParentID.AdvancedSearch.SearchValue) Then
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
	<tr<%= LinkCategory.zPageID.RowAttributes %>>
		<td class="ewTableHeader">Page</td>
		<td<%= LinkCategory.zPageID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_zPageID" id="z_zPageID" value="=" /></span></td>
		<td<%= LinkCategory.zPageID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_zPageID" name="x_zPageID"<%= LinkCategory.zPageID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(LinkCategory.zPageID.EditValue) Then
	arwrk = LinkCategory.zPageID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), LinkCategory.zPageID.AdvancedSearch.SearchValue) Then
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
