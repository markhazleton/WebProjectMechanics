<%@ Page ClassName="zpage_edit" Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="zpage_edit.aspx.vb" Inherits="zpage_edit" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var zPage_edit = new ew_Page("zPage_edit");
// page properties
zPage_edit.PageID = "edit"; // page ID
zPage_edit.FormID = "fzPageedit"; // form ID 
var EW_PAGE_ID = zPage_edit.PageID; // for backward compatibility
// extend page with ValidateForm function
zPage_edit.ValidateForm = function(fobj) {
	ew_PostAutoSuggest(fobj); 
	if (!this.ValidateRequired)
		return true; // ignore validation
	if (fobj.a_confirm && fobj.a_confirm.value == "F")
		return true;
	var i, elm, aelm, infix;
	var rowcnt = (fobj.key_count) ? Number(fobj.key_count.value) : 1;
	for (i=0; i<rowcnt; i++) {
		infix = (fobj.key_count) ? String(i+1) : "";
		elm = fobj.elements["x" + infix + "_zPageName"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, ewLanguage.Phrase("EnterRequiredField") + " - <%= ew_JsEncode2(zPage.zPageName.FldCaption) %>");
		elm = fobj.elements["x" + infix + "_PageOrder"];
		if (elm && elm.type != "hidden" && !ew_CheckInteger(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(zPage.PageOrder.FldErrMsg) %>");
		elm = fobj.elements["x" + infix + "_ImagesPerRow"];
		if (elm && elm.type != "hidden" && !ew_CheckInteger(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(zPage.ImagesPerRow.FldErrMsg) %>");
		elm = fobj.elements["x" + infix + "_RowsPerPage"];
		if (elm && elm.type != "hidden" && !ew_CheckInteger(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(zPage.RowsPerPage.FldErrMsg) %>");
		elm = fobj.elements["x" + infix + "_SiteCategoryID"];
		if (elm && elm.type != "hidden" && !ew_CheckInteger(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(zPage.SiteCategoryID.FldErrMsg) %>");
		elm = fobj.elements["x" + infix + "_SiteCategoryGroupID"];
		if (elm && elm.type != "hidden" && !ew_CheckInteger(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(zPage.SiteCategoryGroupID.FldErrMsg) %>");
		elm = fobj.elements["x" + infix + "_VersionNo"];
		if (elm && elm.type != "hidden" && !ew_CheckInteger(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(zPage.VersionNo.FldErrMsg) %>");
		// Form Custom Validate event
		if (!this.Form_CustomValidate(fobj)) return false;
	}
	return true;
}
// extend page with Form_CustomValidate function
zPage_edit.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
zPage_edit.ValidateRequired = true; // uses JavaScript validation
<% Else %>
zPage_edit.ValidateRequired = false; // no JavaScript validation
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
// To include another .js script, you can use, e.g.
// ew_ClientScriptInclude("my_javascript.js"); or
// ew_ClientScriptInclude("my_javascript.js", {onSuccess: function(o) { // your code here }});
//-->
</script>
<p><span class="aspnetmaker"><%= Language.Phrase("Edit") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= zPage.TableCaption %><br /><br />
<a href="<%= zPage.ReturnUrl %>"><%= Language.Phrase("GoBack") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(zPage_edit.DebugMsg) %>
<% zPage_edit.ShowMessage() %>
<form name="fzPageedit" id="fzPageedit" method="post" onsubmit="this.action=location.pathname;return zPage_edit.ValidateForm(this);">
<p />
<input type="hidden" name="a_table" id="a_table" value="zPage" />
<input type="hidden" name="a_edit" id="a_edit" value="U" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
<% If zPage.ParentPageID.Visible Then ' ParentPageID %>
	<tr<%= zPage.RowAttributes %>>
		<td class="ewTableHeader"><%= zPage.ParentPageID.FldCaption %></td>
		<td<%= zPage.ParentPageID.CellAttributes %>><span id="el_ParentPageID">
<select id="x_ParentPageID" name="x_ParentPageID" title="<%= zPage.ParentPageID.FldTitle %>"<%= zPage.ParentPageID.EditAttributes %>>
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
zPage_edit.ar_x_ParentPageID = [<%= jswrk %>];
//-->
</script>
</span><%= zPage.ParentPageID.CustomMsg %></td>
	</tr>
<% End If %>
<% If zPage.zPageName.Visible Then ' PageName %>
	<tr<%= zPage.RowAttributes %>>
		<td class="ewTableHeader"><%= zPage.zPageName.FldCaption %><%= Language.Phrase("FieldRequiredIndicator") %></td>
		<td<%= zPage.zPageName.CellAttributes %>><span id="el_zPageName">
<input type="text" name="x_zPageName" id="x_zPageName" title="<%= zPage.zPageName.FldTitle %>" size="30" maxlength="50" value="<%= zPage.zPageName.EditValue %>"<%= zPage.zPageName.EditAttributes %> />
</span><%= zPage.zPageName.CustomMsg %></td>
	</tr>
<% End If %>
<% If zPage.PageTitle.Visible Then ' PageTitle %>
	<tr<%= zPage.RowAttributes %>>
		<td class="ewTableHeader"><%= zPage.PageTitle.FldCaption %></td>
		<td<%= zPage.PageTitle.CellAttributes %>><span id="el_PageTitle">
<input type="text" name="x_PageTitle" id="x_PageTitle" title="<%= zPage.PageTitle.FldTitle %>" size="30" maxlength="255" value="<%= zPage.PageTitle.EditValue %>"<%= zPage.PageTitle.EditAttributes %> />
</span><%= zPage.PageTitle.CustomMsg %></td>
	</tr>
<% End If %>
<% If zPage.PageTypeID.Visible Then ' PageTypeID %>
	<tr<%= zPage.RowAttributes %>>
		<td class="ewTableHeader"><%= zPage.PageTypeID.FldCaption %></td>
		<td<%= zPage.PageTypeID.CellAttributes %>><span id="el_PageTypeID">
<select id="x_PageTypeID" name="x_PageTypeID" title="<%= zPage.PageTypeID.FldTitle %>"<%= zPage.PageTypeID.EditAttributes %>>
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
<% If zPage.GroupID.Visible Then ' GroupID %>
	<tr<%= zPage.RowAttributes %>>
		<td class="ewTableHeader"><%= zPage.GroupID.FldCaption %></td>
		<td<%= zPage.GroupID.CellAttributes %>><span id="el_GroupID">
<select id="x_GroupID" name="x_GroupID" title="<%= zPage.GroupID.FldTitle %>"<%= zPage.GroupID.EditAttributes %>>
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
<% If zPage.Active.Visible Then ' Active %>
	<tr<%= zPage.RowAttributes %>>
		<td class="ewTableHeader"><%= zPage.Active.FldCaption %></td>
		<td<%= zPage.Active.CellAttributes %>><span id="el_Active">
<%
If ew_SameStr(zPage.Active.CurrentValue, "1") Then
	selwrk = " checked=""checked"""
Else
	selwrk = ""
End If
%>
<input type="checkbox" name="x_Active" id="x_Active" title="<%= zPage.Active.FldTitle %>" value="1"<%= selwrk %><%= zPage.Active.EditAttributes %> />
</span><%= zPage.Active.CustomMsg %></td>
	</tr>
<% End If %>
<% If zPage.PageOrder.Visible Then ' PageOrder %>
	<tr<%= zPage.RowAttributes %>>
		<td class="ewTableHeader"><%= zPage.PageOrder.FldCaption %><%= Language.Phrase("FieldRequiredIndicator") %></td>
		<td<%= zPage.PageOrder.CellAttributes %>><span id="el_PageOrder">
<input type="text" name="x_PageOrder" id="x_PageOrder" title="<%= zPage.PageOrder.FldTitle %>" size="30" value="<%= zPage.PageOrder.EditValue %>"<%= zPage.PageOrder.EditAttributes %> />
</span><%= zPage.PageOrder.CustomMsg %></td>
	</tr>
<% End If %>
<% If zPage.CompanyID.Visible Then ' CompanyID %>
	<tr<%= zPage.RowAttributes %>>
		<td class="ewTableHeader"><%= zPage.CompanyID.FldCaption %></td>
		<td<%= zPage.CompanyID.CellAttributes %>><span id="el_CompanyID">
<% If zPage.CompanyID.SessionValue <> "" Then %>
<div<%= zPage.CompanyID.ViewAttributes %>><%= zPage.CompanyID.ViewValue %></div>
<input type="hidden" id="x_CompanyID" name="x_CompanyID" value="<%= ew_HtmlEncode(zPage.CompanyID.CurrentValue) %>">
<% Else %>
<% zPage.CompanyID.EditAttrs("onchange") = "ew_UpdateOpt('x_ParentPageID','x_CompanyID',zPage_edit.ar_x_ParentPageID);" %>
<select id="x_CompanyID" name="x_CompanyID" title="<%= zPage.CompanyID.FldTitle %>"<%= zPage.CompanyID.EditAttributes %>>
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
<% End If %>
</span><%= zPage.CompanyID.CustomMsg %></td>
	</tr>
<% End If %>
<% If zPage.PageDescription.Visible Then ' PageDescription %>
	<tr<%= zPage.RowAttributes %>>
		<td class="ewTableHeader"><%= zPage.PageDescription.FldCaption %></td>
		<td<%= zPage.PageDescription.CellAttributes %>><span id="el_PageDescription">
<input type="text" name="x_PageDescription" id="x_PageDescription" title="<%= zPage.PageDescription.FldTitle %>" size="30" maxlength="255" value="<%= zPage.PageDescription.EditValue %>"<%= zPage.PageDescription.EditAttributes %> />
</span><%= zPage.PageDescription.CustomMsg %></td>
	</tr>
<% End If %>
<% If zPage.PageKeywords.Visible Then ' PageKeywords %>
	<tr<%= zPage.RowAttributes %>>
		<td class="ewTableHeader"><%= zPage.PageKeywords.FldCaption %></td>
		<td<%= zPage.PageKeywords.CellAttributes %>><span id="el_PageKeywords">
<input type="text" name="x_PageKeywords" id="x_PageKeywords" title="<%= zPage.PageKeywords.FldTitle %>" size="30" maxlength="255" value="<%= zPage.PageKeywords.EditValue %>"<%= zPage.PageKeywords.EditAttributes %> />
</span><%= zPage.PageKeywords.CustomMsg %></td>
	</tr>
<% End If %>
<% If zPage.ImagesPerRow.Visible Then ' ImagesPerRow %>
	<tr<%= zPage.RowAttributes %>>
		<td class="ewTableHeader"><%= zPage.ImagesPerRow.FldCaption %></td>
		<td<%= zPage.ImagesPerRow.CellAttributes %>><span id="el_ImagesPerRow">
<input type="text" name="x_ImagesPerRow" id="x_ImagesPerRow" title="<%= zPage.ImagesPerRow.FldTitle %>" size="30" value="<%= zPage.ImagesPerRow.EditValue %>"<%= zPage.ImagesPerRow.EditAttributes %> />
</span><%= zPage.ImagesPerRow.CustomMsg %></td>
	</tr>
<% End If %>
<% If zPage.RowsPerPage.Visible Then ' RowsPerPage %>
	<tr<%= zPage.RowAttributes %>>
		<td class="ewTableHeader"><%= zPage.RowsPerPage.FldCaption %></td>
		<td<%= zPage.RowsPerPage.CellAttributes %>><span id="el_RowsPerPage">
<input type="text" name="x_RowsPerPage" id="x_RowsPerPage" title="<%= zPage.RowsPerPage.FldTitle %>" size="30" value="<%= zPage.RowsPerPage.EditValue %>"<%= zPage.RowsPerPage.EditAttributes %> />
</span><%= zPage.RowsPerPage.CustomMsg %></td>
	</tr>
<% End If %>
<% If zPage.PageFileName.Visible Then ' PageFileName %>
	<tr<%= zPage.RowAttributes %>>
		<td class="ewTableHeader"><%= zPage.PageFileName.FldCaption %></td>
		<td<%= zPage.PageFileName.CellAttributes %>><span id="el_PageFileName">
<input type="text" name="x_PageFileName" id="x_PageFileName" title="<%= zPage.PageFileName.FldTitle %>" size="30" maxlength="50" value="<%= zPage.PageFileName.EditValue %>"<%= zPage.PageFileName.EditAttributes %> />
</span><%= zPage.PageFileName.CustomMsg %></td>
	</tr>
<% End If %>
<% If zPage.AllowMessage.Visible Then ' AllowMessage %>
	<tr<%= zPage.RowAttributes %>>
		<td class="ewTableHeader"><%= zPage.AllowMessage.FldCaption %></td>
		<td<%= zPage.AllowMessage.CellAttributes %>><span id="el_AllowMessage">
<%
If ew_SameStr(zPage.AllowMessage.CurrentValue, "1") Then
	selwrk = " checked=""checked"""
Else
	selwrk = ""
End If
%>
<input type="checkbox" name="x_AllowMessage" id="x_AllowMessage" title="<%= zPage.AllowMessage.FldTitle %>" value="1"<%= selwrk %><%= zPage.AllowMessage.EditAttributes %> />
</span><%= zPage.AllowMessage.CustomMsg %></td>
	</tr>
<% End If %>
<% If zPage.SiteCategoryID.Visible Then ' SiteCategoryID %>
	<tr<%= zPage.RowAttributes %>>
		<td class="ewTableHeader"><%= zPage.SiteCategoryID.FldCaption %></td>
		<td<%= zPage.SiteCategoryID.CellAttributes %>><span id="el_SiteCategoryID">
<input type="text" name="x_SiteCategoryID" id="x_SiteCategoryID" title="<%= zPage.SiteCategoryID.FldTitle %>" size="30" value="<%= zPage.SiteCategoryID.EditValue %>"<%= zPage.SiteCategoryID.EditAttributes %> />
</span><%= zPage.SiteCategoryID.CustomMsg %></td>
	</tr>
<% End If %>
<% If zPage.SiteCategoryGroupID.Visible Then ' SiteCategoryGroupID %>
	<tr<%= zPage.RowAttributes %>>
		<td class="ewTableHeader"><%= zPage.SiteCategoryGroupID.FldCaption %></td>
		<td<%= zPage.SiteCategoryGroupID.CellAttributes %>><span id="el_SiteCategoryGroupID">
<input type="text" name="x_SiteCategoryGroupID" id="x_SiteCategoryGroupID" title="<%= zPage.SiteCategoryGroupID.FldTitle %>" size="30" value="<%= zPage.SiteCategoryGroupID.EditValue %>"<%= zPage.SiteCategoryGroupID.EditAttributes %> />
</span><%= zPage.SiteCategoryGroupID.CustomMsg %></td>
	</tr>
<% End If %>
<% If zPage.VersionNo.Visible Then ' VersionNo %>
	<tr<%= zPage.RowAttributes %>>
		<td class="ewTableHeader"><%= zPage.VersionNo.FldCaption %></td>
		<td<%= zPage.VersionNo.CellAttributes %>><span id="el_VersionNo">
<input type="text" name="x_VersionNo" id="x_VersionNo" title="<%= zPage.VersionNo.FldTitle %>" size="30" value="<%= zPage.VersionNo.EditValue %>"<%= zPage.VersionNo.EditAttributes %> />
</span><%= zPage.VersionNo.CustomMsg %></td>
	</tr>
<% End If %>
<% If zPage.ModifiedDT.Visible Then ' ModifiedDT %>
	<tr<%= zPage.RowAttributes %>>
		<td class="ewTableHeader"><%= zPage.ModifiedDT.FldCaption %></td>
		<td<%= zPage.ModifiedDT.CellAttributes %>><span id="el_ModifiedDT">
<input type="text" name="x_ModifiedDT" id="x_ModifiedDT" title="<%= zPage.ModifiedDT.FldTitle %>" value="<%= zPage.ModifiedDT.EditValue %>"<%= zPage.ModifiedDT.EditAttributes %> />
</span><%= zPage.ModifiedDT.CustomMsg %></td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<input type="hidden" name="x_zPageID" id="x_zPageID" value="<%= ew_HTMLEncode(zPage.zPageID.CurrentValue) %>" />
<p />
<input type="submit" name="btnAction" id="btnAction" value="<%= ew_BtnCaption(Language.Phrase("EditBtn")) %>" />
</form>
<script language="JavaScript" type="text/javascript">
<!--
ew_UpdateOpts([['x_ParentPageID','x_CompanyID',zPage_edit.ar_x_ParentPageID]]);
//-->
</script>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
</asp:Content>
