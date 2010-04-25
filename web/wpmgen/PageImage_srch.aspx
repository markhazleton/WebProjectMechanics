<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="PageImage_srch.aspx.vb" Inherits="PageImage_srch" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var PageImage_search = new ew_Page("PageImage_search");
// page properties
PageImage_search.PageID = "search"; // page ID
var EW_PAGE_ID = PageImage_search.PageID; // for backward compatibility
// extend page with validate function for search
PageImage_search.ValidateSearch = function(fobj) {
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
PageImage_search.ValidateRequired = true; // uses JavaScript validation
<% Else %>
PageImage_search.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">Search TABLE: Location Image<br /><br />
<a href="<%= PageImage.ReturnUrl %>">Back to List</a></span></p>
<% PageImage_search.ShowMessage() %>
<form name="fPageImagesearch" id="fPageImagesearch" method="post" onsubmit="this.action=location.pathname;return PageImage_search.ValidateSearch(this);">
<p />
<input type="hidden" name="t" id="t" value="PageImage" />
<input type="hidden" name="a_search" id="a_search" value="S" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
	<tr<%= PageImage.zPageID.RowAttributes %>>
		<td class="ewTableHeader">PageName</td>
		<td<%= PageImage.zPageID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_zPageID" id="z_zPageID" value="=" /></span></td>
		<td<%= PageImage.zPageID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_zPageID" name="x_zPageID"<%= PageImage.zPageID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(PageImage.zPageID.EditValue) Then
	arwrk = PageImage.zPageID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), PageImage.zPageID.AdvancedSearch.SearchValue) Then
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
	<tr<%= PageImage.ImageID.RowAttributes %>>
		<td class="ewTableHeader">ImageName</td>
		<td<%= PageImage.ImageID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_ImageID" id="z_ImageID" value="=" /></span></td>
		<td<%= PageImage.ImageID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_ImageID" name="x_ImageID"<%= PageImage.ImageID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(PageImage.ImageID.EditValue) Then
	arwrk = PageImage.ImageID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), PageImage.ImageID.AdvancedSearch.SearchValue) Then
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
