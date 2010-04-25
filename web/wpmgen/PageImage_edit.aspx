<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="PageImage_edit.aspx.vb" Inherits="PageImage_edit" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var PageImage_edit = new ew_Page("PageImage_edit");
// page properties
PageImage_edit.PageID = "edit"; // page ID
var EW_PAGE_ID = PageImage_edit.PageID; // for backward compatibility
// extend page with ValidateForm function
PageImage_edit.ValidateForm = function(fobj) {
	if (!this.ValidateRequired)
		return true; // ignore validation
	if (fobj.a_confirm && fobj.a_confirm.value == "F")
		return true;
	var i, elm, aelm, infix;
	var rowcnt = (fobj.key_count) ? Number(fobj.key_count.value) : 1;
	for (i=0; i<rowcnt; i++) {
		infix = (fobj.key_count) ? String(i+1) : "";
		elm = fobj.elements["x" + infix + "_zPageID"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field - PageName");
		elm = fobj.elements["x" + infix + "_ImageID"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field - ImageName");
		elm = fobj.elements["x" + infix + "_PageImagePosition"];
		if (elm && !ew_CheckInteger(elm.value))
			return ew_OnError(this, elm, "Incorrect integer - Page Image Position");
	}
	return true;
}
<% If EW_CLIENT_VALIDATE Then %>
PageImage_edit.ValidateRequired = true; // uses JavaScript validation
<% Else %>
PageImage_edit.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">Edit TABLE: Location Image<br /><br />
<a href="<%= PageImage.ReturnUrl %>">Go Back</a></span></p>
<% PageImage_edit.ShowMessage() %>
<form name="fPageImageedit" id="fPageImageedit" method="post" onsubmit="this.action=location.pathname;return PageImage_edit.ValidateForm(this);">
<p />
<input type="hidden" name="a_table" id="a_table" value="PageImage" />
<input type="hidden" name="a_edit" id="a_edit" value="U" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
<% If PageImage.zPageID.Visible Then ' PageID %>
	<tr<%= PageImage.zPageID.RowAttributes %>>
		<td class="ewTableHeader">PageName<span class="ewRequired">&nbsp;*</span></td>
		<td<%= PageImage.zPageID.CellAttributes %>><span id="el_zPageID">
<select id="x_zPageID" name="x_zPageID"<%= PageImage.zPageID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(PageImage.zPageID.EditValue) Then
	arwrk = PageImage.zPageID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), PageImage.zPageID.CurrentValue) Then
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
</span><%= PageImage.zPageID.CustomMsg %></td>
	</tr>
<% End If %>
<% If PageImage.ImageID.Visible Then ' ImageID %>
	<tr<%= PageImage.ImageID.RowAttributes %>>
		<td class="ewTableHeader">ImageName<span class="ewRequired">&nbsp;*</span></td>
		<td<%= PageImage.ImageID.CellAttributes %>><span id="el_ImageID">
<select id="x_ImageID" name="x_ImageID"<%= PageImage.ImageID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(PageImage.ImageID.EditValue) Then
	arwrk = PageImage.ImageID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), PageImage.ImageID.CurrentValue) Then
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
</span><%= PageImage.ImageID.CustomMsg %></td>
	</tr>
<% End If %>
<% If PageImage.PageImagePosition.Visible Then ' PageImagePosition %>
	<tr<%= PageImage.PageImagePosition.RowAttributes %>>
		<td class="ewTableHeader">Page Image Position</td>
		<td<%= PageImage.PageImagePosition.CellAttributes %>><span id="el_PageImagePosition">
<input type="text" name="x_PageImagePosition" id="x_PageImagePosition" size="30" value="<%= PageImage.PageImagePosition.EditValue %>"<%= PageImage.PageImagePosition.EditAttributes %> />
</span><%= PageImage.PageImagePosition.CustomMsg %></td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<input type="hidden" name="x_PageImageID" id="x_PageImageID" value="<%= ew_HTMLEncode(PageImage.PageImageID.CurrentValue) %>" />
<p />
<input type="submit" name="btnAction" id="btnAction" value="Save Changes" />
</form>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
</asp:Content>
