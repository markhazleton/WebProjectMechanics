<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="Image_srch.aspx.vb" Inherits="Image_srch" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var Image_search = new ew_Page("Image_search");
// page properties
Image_search.PageID = "search"; // page ID
var EW_PAGE_ID = Image_search.PageID; // for backward compatibility
// extend page with validate function for search
Image_search.ValidateSearch = function(fobj) {
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
Image_search.ValidateRequired = true; // uses JavaScript validation
<% Else %>
Image_search.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">Search TABLE: Site Image<br /><br />
<a href="<%= Image.ReturnUrl %>">Back to List</a></span></p>
<% Image_search.ShowMessage() %>
<form name="fImagesearch" id="fImagesearch" method="post" onsubmit="this.action=location.pathname;return Image_search.ValidateSearch(this);">
<p />
<input type="hidden" name="t" id="t" value="Image" />
<input type="hidden" name="a_search" id="a_search" value="S" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
	<tr<%= Image.CompanyID.RowAttributes %>>
		<td class="ewTableHeader">Company</td>
		<td<%= Image.CompanyID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_CompanyID" id="z_CompanyID" value="=" /></span></td>
		<td<%= Image.CompanyID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_CompanyID" name="x_CompanyID"<%= Image.CompanyID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Image.CompanyID.EditValue) Then
	arwrk = Image.CompanyID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Image.CompanyID.AdvancedSearch.SearchValue) Then
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
	<tr<%= Image.title.RowAttributes %>>
		<td class="ewTableHeader">Title</td>
		<td<%= Image.title.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_title" id="z_title" value="LIKE" /></span></td>
		<td<%= Image.title.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_title" id="x_title" size="30" maxlength="50" value="<%= Image.title.EditValue %>"<%= Image.title.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Image.ImageName.RowAttributes %>>
		<td class="ewTableHeader">Name</td>
		<td<%= Image.ImageName.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_ImageName" id="z_ImageName" value="LIKE" /></span></td>
		<td<%= Image.ImageName.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_ImageName" id="x_ImageName" size="30" maxlength="50" value="<%= Image.ImageName.EditValue %>"<%= Image.ImageName.EditAttributes %> />
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
