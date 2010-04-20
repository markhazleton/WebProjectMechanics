<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="SiteCategory_add.aspx.vb" Inherits="SiteCategory_add" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var SiteCategory_add = new ew_Page("SiteCategory_add");
// page properties
SiteCategory_add.PageID = "add"; // page ID
var EW_PAGE_ID = SiteCategory_add.PageID; // for backward compatibility
// extend page with ValidateForm function
SiteCategory_add.ValidateForm = function(fobj) {
	if (!this.ValidateRequired)
		return true; // ignore validation
	if (fobj.a_confirm && fobj.a_confirm.value == "F")
		return true;
	var i, elm, aelm, infix;
	var rowcnt = (fobj.key_count) ? Number(fobj.key_count.value) : 1;
	for (i=0; i<rowcnt; i++) {
		infix = (fobj.key_count) ? String(i+1) : "";
		elm = fobj.elements["x" + infix + "_SiteCategoryTypeID"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field - Site Type");
		elm = fobj.elements["x" + infix + "_GroupOrder"];
		if (elm && !ew_CheckNumber(elm.value))
			return ew_OnError(this, elm, "Incorrect floating point number - Order");
	}
	return true;
}
<% If EW_CLIENT_VALIDATE Then %>
SiteCategory_add.ValidateRequired = true; // uses JavaScript validation
<% Else %>
SiteCategory_add.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">Add to TABLE: Site Type Location<br /><br />
<a href="<%= SiteCategory.ReturnUrl %>">Go Back</a></span></p>
<% SiteCategory_add.ShowMessage() %>
<form name="fSiteCategoryadd" id="fSiteCategoryadd" method="post" onsubmit="this.action=location.pathname;return SiteCategory_add.ValidateForm(this);">
<p />
<input type="hidden" name="t" id="t" value="SiteCategory" />
<input type="hidden" name="a_add" id="a_add" value="A" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
<% If SiteCategory.SiteCategoryTypeID.Visible Then ' SiteCategoryTypeID %>
	<tr<%= SiteCategory.SiteCategoryTypeID.RowAttributes %>>
		<td class="ewTableHeader">Site Type<span class="ewRequired">&nbsp;*</span></td>
		<td<%= SiteCategory.SiteCategoryTypeID.CellAttributes %>><span id="el_SiteCategoryTypeID">
<select id="x_SiteCategoryTypeID" name="x_SiteCategoryTypeID" onchange="ew_UpdateOpt('x_ParentCategoryID','x_SiteCategoryTypeID',SiteCategory_add.ar_x_ParentCategoryID);"<%= SiteCategory.SiteCategoryTypeID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(SiteCategory.SiteCategoryTypeID.EditValue) Then
	arwrk = SiteCategory.SiteCategoryTypeID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), SiteCategory.SiteCategoryTypeID.CurrentValue) Then
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
</span><%= SiteCategory.SiteCategoryTypeID.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteCategory.GroupOrder.Visible Then ' GroupOrder %>
	<tr<%= SiteCategory.GroupOrder.RowAttributes %>>
		<td class="ewTableHeader">Order</td>
		<td<%= SiteCategory.GroupOrder.CellAttributes %>><span id="el_GroupOrder">
<input type="text" name="x_GroupOrder" id="x_GroupOrder" size="30" value="<%= SiteCategory.GroupOrder.EditValue %>"<%= SiteCategory.GroupOrder.EditAttributes %> />
</span><%= SiteCategory.GroupOrder.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteCategory.CategoryName.Visible Then ' CategoryName %>
	<tr<%= SiteCategory.CategoryName.RowAttributes %>>
		<td class="ewTableHeader">Name</td>
		<td<%= SiteCategory.CategoryName.CellAttributes %>><span id="el_CategoryName">
<input type="text" name="x_CategoryName" id="x_CategoryName" size="50" maxlength="255" value="<%= SiteCategory.CategoryName.EditValue %>"<%= SiteCategory.CategoryName.EditAttributes %> />
</span><%= SiteCategory.CategoryName.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteCategory.CategoryTitle.Visible Then ' CategoryTitle %>
	<tr<%= SiteCategory.CategoryTitle.RowAttributes %>>
		<td class="ewTableHeader">Title</td>
		<td<%= SiteCategory.CategoryTitle.CellAttributes %>><span id="el_CategoryTitle">
<textarea name="x_CategoryTitle" id="x_CategoryTitle" cols="50" rows="5"<%= SiteCategory.CategoryTitle.EditAttributes %>><%= SiteCategory.CategoryTitle.EditValue %></textarea>
</span><%= SiteCategory.CategoryTitle.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteCategory.CategoryDescription.Visible Then ' CategoryDescription %>
	<tr<%= SiteCategory.CategoryDescription.RowAttributes %>>
		<td class="ewTableHeader">Description</td>
		<td<%= SiteCategory.CategoryDescription.CellAttributes %>><span id="el_CategoryDescription">
<textarea name="x_CategoryDescription" id="x_CategoryDescription" cols="50" rows="5"<%= SiteCategory.CategoryDescription.EditAttributes %>><%= SiteCategory.CategoryDescription.EditValue %></textarea>
</span><%= SiteCategory.CategoryDescription.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteCategory.CategoryKeywords.Visible Then ' CategoryKeywords %>
	<tr<%= SiteCategory.CategoryKeywords.RowAttributes %>>
		<td class="ewTableHeader">Keywords</td>
		<td<%= SiteCategory.CategoryKeywords.CellAttributes %>><span id="el_CategoryKeywords">
<textarea name="x_CategoryKeywords" id="x_CategoryKeywords" cols="50" rows="5"<%= SiteCategory.CategoryKeywords.EditAttributes %>><%= SiteCategory.CategoryKeywords.EditValue %></textarea>
</span><%= SiteCategory.CategoryKeywords.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteCategory.ParentCategoryID.Visible Then ' ParentCategoryID %>
	<tr<%= SiteCategory.ParentCategoryID.RowAttributes %>>
		<td class="ewTableHeader">Parent Category</td>
		<td<%= SiteCategory.ParentCategoryID.CellAttributes %>><span id="el_ParentCategoryID">
<select id="x_ParentCategoryID" name="x_ParentCategoryID"<%= SiteCategory.ParentCategoryID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(SiteCategory.ParentCategoryID.EditValue) Then
	arwrk = SiteCategory.ParentCategoryID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), SiteCategory.ParentCategoryID.CurrentValue) Then
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
If ew_IsArrayList(SiteCategory.ParentCategoryID.EditValue) Then
	arwrk = SiteCategory.ParentCategoryID.EditValue
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
SiteCategory_add.ar_x_ParentCategoryID = [<%= jswrk %>];
//-->
</script>
</span><%= SiteCategory.ParentCategoryID.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteCategory.CategoryFileName.Visible Then ' CategoryFileName %>
	<tr<%= SiteCategory.CategoryFileName.RowAttributes %>>
		<td class="ewTableHeader">Category File Name</td>
		<td<%= SiteCategory.CategoryFileName.CellAttributes %>><span id="el_CategoryFileName">
<input type="text" name="x_CategoryFileName" id="x_CategoryFileName" size="30" maxlength="255" value="<%= SiteCategory.CategoryFileName.EditValue %>"<%= SiteCategory.CategoryFileName.EditAttributes %> />
</span><%= SiteCategory.CategoryFileName.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteCategory.SiteCategoryGroupID.Visible Then ' SiteCategoryGroupID %>
	<tr<%= SiteCategory.SiteCategoryGroupID.RowAttributes %>>
		<td class="ewTableHeader">Site Group</td>
		<td<%= SiteCategory.SiteCategoryGroupID.CellAttributes %>><span id="el_SiteCategoryGroupID">
<select id="x_SiteCategoryGroupID" name="x_SiteCategoryGroupID"<%= SiteCategory.SiteCategoryGroupID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(SiteCategory.SiteCategoryGroupID.EditValue) Then
	arwrk = SiteCategory.SiteCategoryGroupID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), SiteCategory.SiteCategoryGroupID.CurrentValue) Then
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
</span><%= SiteCategory.SiteCategoryGroupID.CustomMsg %></td>
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
ew_UpdateOpts([['x_ParentCategoryID','x_SiteCategoryTypeID',SiteCategory_add.ar_x_ParentCategoryID]]);
//-->
</script>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
</asp:Content>
