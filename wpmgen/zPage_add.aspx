<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="zPage_add.aspx.vb" Inherits="zPage_add" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var zPage_add = new ew_Page("zPage_add");
// page properties
zPage_add.PageID = "add"; // page ID
var EW_PAGE_ID = zPage_add.PageID; // for backward compatibility
// extend page with ValidateForm function
zPage_add.ValidateForm = function(fobj) {
	if (!this.ValidateRequired)
		return true; // ignore validation
	if (fobj.a_confirm && fobj.a_confirm.value == "F")
		return true;
	var i, elm, aelm, infix;
	var rowcnt = (fobj.key_count) ? Number(fobj.key_count.value) : 1;
	for (i=0; i<rowcnt; i++) {
		infix = (fobj.key_count) ? String(i+1) : "";
		elm = fobj.elements["x" + infix + "_CompanyID"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field - Company");
		elm = fobj.elements["x" + infix + "_PageOrder"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field - Order");
		elm = fobj.elements["x" + infix + "_PageOrder"];
		if (elm && !ew_CheckInteger(elm.value))
			return ew_OnError(this, elm, "Incorrect integer - Order");
		elm = fobj.elements["x" + infix + "_GroupID"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field - Group");
		elm = fobj.elements["x" + infix + "_PageTypeID"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field - PageType");
		elm = fobj.elements["x" + infix + "_zPageName"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field - Page Name");
		elm = fobj.elements["x" + infix + "_ImagesPerRow"];
		if (elm && !ew_CheckInteger(elm.value))
			return ew_OnError(this, elm, "Incorrect integer - Images Per Row");
		elm = fobj.elements["x" + infix + "_RowsPerPage"];
		if (elm && !ew_CheckInteger(elm.value))
			return ew_OnError(this, elm, "Incorrect integer - Rows Per Page");
		elm = fobj.elements["x" + infix + "_VersionNo"];
		if (elm && !ew_CheckInteger(elm.value))
			return ew_OnError(this, elm, "Incorrect integer - Version No");
	}
	return true;
}
<% If EW_CLIENT_VALIDATE Then %>
zPage_add.ValidateRequired = true; // uses JavaScript validation
<% Else %>
zPage_add.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">Add to TABLE: Site Location<br /><br />
<a href="<%= zPage.ReturnUrl %>">Go Back</a></span></p>
<% zPage_add.ShowMessage() %>
<form name="fzPageadd" id="fzPageadd" method="post" onsubmit="this.action=location.pathname;return zPage_add.ValidateForm(this);">
<p />
<input type="hidden" name="t" id="t" value="zPage" />
<input type="hidden" name="a_add" id="a_add" value="A" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
<% If zPage.CompanyID.Visible Then ' CompanyID %>
	<tr<%= zPage.CompanyID.RowAttributes %>>
		<td class="ewTableHeader">Company<span class="ewRequired">&nbsp;*</span></td>
		<td<%= zPage.CompanyID.CellAttributes %>><span id="el_CompanyID">
<select id="x_CompanyID" name="x_CompanyID" onchange="ew_UpdateOpt('x_ParentPageID','x_CompanyID',zPage_add.ar_x_ParentPageID);"<%= zPage.CompanyID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(zPage.CompanyID.EditValue) Then
	arwrk = zPage.CompanyID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), zPage.CompanyID.CurrentValue) Then
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
</span><%= zPage.CompanyID.CustomMsg %></td>
	</tr>
<% End If %>
<% If zPage.PageOrder.Visible Then ' PageOrder %>
	<tr<%= zPage.PageOrder.RowAttributes %>>
		<td class="ewTableHeader">Order<span class="ewRequired">&nbsp;*</span></td>
		<td<%= zPage.PageOrder.CellAttributes %>><span id="el_PageOrder">
<input type="text" name="x_PageOrder" id="x_PageOrder" size="10" value="<%= zPage.PageOrder.EditValue %>"<%= zPage.PageOrder.EditAttributes %> />
</span><%= zPage.PageOrder.CustomMsg %></td>
	</tr>
<% End If %>
<% If zPage.GroupID.Visible Then ' GroupID %>
	<tr<%= zPage.GroupID.RowAttributes %>>
		<td class="ewTableHeader">Group<span class="ewRequired">&nbsp;*</span></td>
		<td<%= zPage.GroupID.CellAttributes %>><span id="el_GroupID">
<select id="x_GroupID" name="x_GroupID"<%= zPage.GroupID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(zPage.GroupID.EditValue) Then
	arwrk = zPage.GroupID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), zPage.GroupID.CurrentValue) Then
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
</span><%= zPage.GroupID.CustomMsg %></td>
	</tr>
<% End If %>
<% If zPage.ParentPageID.Visible Then ' ParentPageID %>
	<tr<%= zPage.ParentPageID.RowAttributes %>>
		<td class="ewTableHeader">Parent Page</td>
		<td<%= zPage.ParentPageID.CellAttributes %>><span id="el_ParentPageID">
<select id="x_ParentPageID" name="x_ParentPageID"<%= zPage.ParentPageID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(zPage.ParentPageID.EditValue) Then
	arwrk = zPage.ParentPageID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), zPage.ParentPageID.CurrentValue) Then
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
If ew_IsArrayList(zPage.ParentPageID.EditValue) Then
	arwrk = zPage.ParentPageID.EditValue
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
zPage_add.ar_x_ParentPageID = [<%= jswrk %>];
//-->
</script>
</span><%= zPage.ParentPageID.CustomMsg %></td>
	</tr>
<% End If %>
<% If zPage.PageTypeID.Visible Then ' PageTypeID %>
	<tr<%= zPage.PageTypeID.RowAttributes %>>
		<td class="ewTableHeader">PageType<span class="ewRequired">&nbsp;*</span></td>
		<td<%= zPage.PageTypeID.CellAttributes %>><span id="el_PageTypeID">
<select id="x_PageTypeID" name="x_PageTypeID"<%= zPage.PageTypeID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(zPage.PageTypeID.EditValue) Then
	arwrk = zPage.PageTypeID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), zPage.PageTypeID.CurrentValue) Then
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
</span><%= zPage.PageTypeID.CustomMsg %></td>
	</tr>
<% End If %>
<% If zPage.Active.Visible Then ' Active %>
	<tr<%= zPage.Active.RowAttributes %>>
		<td class="ewTableHeader">Active</td>
		<td<%= zPage.Active.CellAttributes %>><span id="el_Active">
<%
If ew_SameStr(zPage.Active.CurrentValue, "1") Then
	selwrk = " checked=""checked"""
Else
	selwrk = ""
End If
%>
<input type="checkbox" name="x_Active" id="x_Active" value="1"<%= selwrk %><%= zPage.Active.EditAttributes %> />
</span><%= zPage.Active.CustomMsg %></td>
	</tr>
<% End If %>
<% If zPage.zPageName.Visible Then ' PageName %>
	<tr<%= zPage.zPageName.RowAttributes %>>
		<td class="ewTableHeader">Page Name<span class="ewRequired">&nbsp;*</span></td>
		<td<%= zPage.zPageName.CellAttributes %>><span id="el_zPageName">
<input type="text" name="x_zPageName" id="x_zPageName" size="60" maxlength="60" value="<%= zPage.zPageName.EditValue %>"<%= zPage.zPageName.EditAttributes %> />
</span><%= zPage.zPageName.CustomMsg %></td>
	</tr>
<% End If %>
<% If zPage.PageTitle.Visible Then ' PageTitle %>
	<tr<%= zPage.PageTitle.RowAttributes %>>
		<td class="ewTableHeader">Page Title</td>
		<td<%= zPage.PageTitle.CellAttributes %>><span id="el_PageTitle">
<input type="text" name="x_PageTitle" id="x_PageTitle" size="60" maxlength="255" value="<%= zPage.PageTitle.EditValue %>"<%= zPage.PageTitle.EditAttributes %> />
</span><%= zPage.PageTitle.CustomMsg %></td>
	</tr>
<% End If %>
<% If zPage.PageDescription.Visible Then ' PageDescription %>
	<tr<%= zPage.PageDescription.RowAttributes %>>
		<td class="ewTableHeader">Page Description</td>
		<td<%= zPage.PageDescription.CellAttributes %>><span id="el_PageDescription">
<input type="text" name="x_PageDescription" id="x_PageDescription" size="60" maxlength="255" value="<%= zPage.PageDescription.EditValue %>"<%= zPage.PageDescription.EditAttributes %> />
</span><%= zPage.PageDescription.CustomMsg %></td>
	</tr>
<% End If %>
<% If zPage.PageKeywords.Visible Then ' PageKeywords %>
	<tr<%= zPage.PageKeywords.RowAttributes %>>
		<td class="ewTableHeader">Page Keywords</td>
		<td<%= zPage.PageKeywords.CellAttributes %>><span id="el_PageKeywords">
<input type="text" name="x_PageKeywords" id="x_PageKeywords" size="60" maxlength="255" value="<%= zPage.PageKeywords.EditValue %>"<%= zPage.PageKeywords.EditAttributes %> />
</span><%= zPage.PageKeywords.CustomMsg %></td>
	</tr>
<% End If %>
<% If zPage.ImagesPerRow.Visible Then ' ImagesPerRow %>
	<tr<%= zPage.ImagesPerRow.RowAttributes %>>
		<td class="ewTableHeader">Images Per Row</td>
		<td<%= zPage.ImagesPerRow.CellAttributes %>><span id="el_ImagesPerRow">
<input type="text" name="x_ImagesPerRow" id="x_ImagesPerRow" size="30" value="<%= zPage.ImagesPerRow.EditValue %>"<%= zPage.ImagesPerRow.EditAttributes %> />
</span><%= zPage.ImagesPerRow.CustomMsg %></td>
	</tr>
<% End If %>
<% If zPage.RowsPerPage.Visible Then ' RowsPerPage %>
	<tr<%= zPage.RowsPerPage.RowAttributes %>>
		<td class="ewTableHeader">Rows Per Page</td>
		<td<%= zPage.RowsPerPage.CellAttributes %>><span id="el_RowsPerPage">
<input type="text" name="x_RowsPerPage" id="x_RowsPerPage" size="30" value="<%= zPage.RowsPerPage.EditValue %>"<%= zPage.RowsPerPage.EditAttributes %> />
</span><%= zPage.RowsPerPage.CustomMsg %></td>
	</tr>
<% End If %>
<% If zPage.PageFileName.Visible Then ' PageFileName %>
	<tr<%= zPage.PageFileName.RowAttributes %>>
		<td class="ewTableHeader">Page File Name</td>
		<td<%= zPage.PageFileName.CellAttributes %>><span id="el_PageFileName">
<input type="text" name="x_PageFileName" id="x_PageFileName" size="30" maxlength="255" value="<%= zPage.PageFileName.EditValue %>"<%= zPage.PageFileName.EditAttributes %> />
</span><%= zPage.PageFileName.CustomMsg %></td>
	</tr>
<% End If %>
<% If zPage.VersionNo.Visible Then ' VersionNo %>
	<tr<%= zPage.VersionNo.RowAttributes %>>
		<td class="ewTableHeader">Version No</td>
		<td<%= zPage.VersionNo.CellAttributes %>><span id="el_VersionNo">
<input type="text" name="x_VersionNo" id="x_VersionNo" size="30" value="<%= zPage.VersionNo.EditValue %>"<%= zPage.VersionNo.EditAttributes %> />
</span><%= zPage.VersionNo.CustomMsg %></td>
	</tr>
<% End If %>
<% If zPage.SiteCategoryID.Visible Then ' SiteCategoryID %>
	<tr<%= zPage.SiteCategoryID.RowAttributes %>>
		<td class="ewTableHeader">Site Category</td>
		<td<%= zPage.SiteCategoryID.CellAttributes %>><span id="el_SiteCategoryID">
<select id="x_SiteCategoryID" name="x_SiteCategoryID"<%= zPage.SiteCategoryID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(zPage.SiteCategoryID.EditValue) Then
	arwrk = zPage.SiteCategoryID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), zPage.SiteCategoryID.CurrentValue) Then
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
</span><%= zPage.SiteCategoryID.CustomMsg %></td>
	</tr>
<% End If %>
<% If zPage.SiteCategoryGroupID.Visible Then ' SiteCategoryGroupID %>
	<tr<%= zPage.SiteCategoryGroupID.RowAttributes %>>
		<td class="ewTableHeader">Site Category Group</td>
		<td<%= zPage.SiteCategoryGroupID.CellAttributes %>><span id="el_SiteCategoryGroupID">
<select id="x_SiteCategoryGroupID" name="x_SiteCategoryGroupID"<%= zPage.SiteCategoryGroupID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(zPage.SiteCategoryGroupID.EditValue) Then
	arwrk = zPage.SiteCategoryGroupID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), zPage.SiteCategoryGroupID.CurrentValue) Then
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
</span><%= zPage.SiteCategoryGroupID.CustomMsg %></td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<p />
<input type="submit" name="btnAction" id="btnAction" value=" Save New " />
</form>
<script language="JavaScript">
<!--
ew_UpdateOpts([['x_ParentPageID','x_CompanyID',zPage_add.ar_x_ParentPageID]]);
//-->
</script>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
</asp:Content>
