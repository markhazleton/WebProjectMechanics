<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="LinkCategory_edit.aspx.vb" Inherits="LinkCategory_edit" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var LinkCategory_edit = new ew_Page("LinkCategory_edit");
// page properties
LinkCategory_edit.PageID = "edit"; // page ID
var EW_PAGE_ID = LinkCategory_edit.PageID; // for backward compatibility
// extend page with ValidateForm function
LinkCategory_edit.ValidateForm = function(fobj) {
	if (!this.ValidateRequired)
		return true; // ignore validation
	if (fobj.a_confirm && fobj.a_confirm.value == "F")
		return true;
	var i, elm, aelm, infix;
	var rowcnt = (fobj.key_count) ? Number(fobj.key_count.value) : 1;
	for (i=0; i<rowcnt; i++) {
		infix = (fobj.key_count) ? String(i+1) : "";
	}
	return true;
}
<% If EW_CLIENT_VALIDATE Then %>
LinkCategory_edit.ValidateRequired = true; // uses JavaScript validation
<% Else %>
LinkCategory_edit.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">Edit TABLE: Part Category<br /><br />
<a href="<%= LinkCategory.ReturnUrl %>">Go Back</a></span></p>
<% LinkCategory_edit.ShowMessage() %>
<form name="fLinkCategoryedit" id="fLinkCategoryedit" method="post" onsubmit="this.action=location.pathname;return LinkCategory_edit.ValidateForm(this);">
<p />
<input type="hidden" name="a_table" id="a_table" value="LinkCategory" />
<input type="hidden" name="a_edit" id="a_edit" value="U" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
<% If LinkCategory.Title.Visible Then ' Title %>
	<tr<%= LinkCategory.Title.RowAttributes %>>
		<td class="ewTableHeader">Title</td>
		<td<%= LinkCategory.Title.CellAttributes %>><span id="el_Title">
<input type="text" name="x_Title" id="x_Title" size="30" maxlength="50" value="<%= LinkCategory.Title.EditValue %>"<%= LinkCategory.Title.EditAttributes %> />
</span><%= LinkCategory.Title.CustomMsg %></td>
	</tr>
<% End If %>
<% If LinkCategory.Description.Visible Then ' Description %>
	<tr<%= LinkCategory.Description.RowAttributes %>>
		<td class="ewTableHeader">Description</td>
		<td<%= LinkCategory.Description.CellAttributes %>><span id="el_Description">
<textarea name="x_Description" id="x_Description" cols="35" rows="4"<%= LinkCategory.Description.EditAttributes %>><%= LinkCategory.Description.EditValue %></textarea>
</span><%= LinkCategory.Description.CustomMsg %></td>
	</tr>
<% End If %>
<% If LinkCategory.ParentID.Visible Then ' ParentID %>
	<tr<%= LinkCategory.ParentID.RowAttributes %>>
		<td class="ewTableHeader">Parent</td>
		<td<%= LinkCategory.ParentID.CellAttributes %>><span id="el_ParentID">
<select id="x_ParentID" name="x_ParentID"<%= LinkCategory.ParentID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(LinkCategory.ParentID.EditValue) Then
	arwrk = LinkCategory.ParentID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), LinkCategory.ParentID.CurrentValue) Then
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
</span><%= LinkCategory.ParentID.CustomMsg %></td>
	</tr>
<% End If %>
<% If LinkCategory.zPageID.Visible Then ' PageID %>
	<tr<%= LinkCategory.zPageID.RowAttributes %>>
		<td class="ewTableHeader">Page</td>
		<td<%= LinkCategory.zPageID.CellAttributes %>><span id="el_zPageID">
<select id="x_zPageID" name="x_zPageID"<%= LinkCategory.zPageID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(LinkCategory.zPageID.EditValue) Then
	arwrk = LinkCategory.zPageID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), LinkCategory.zPageID.CurrentValue) Then
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
</span><%= LinkCategory.zPageID.CustomMsg %></td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<input type="hidden" name="x_ID" id="x_ID" value="<%= ew_HTMLEncode(LinkCategory.ID.CurrentValue) %>" />
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
