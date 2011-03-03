<%@ Page ClassName="zpage_srch" Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="zpage_srch.aspx.vb" Inherits="zpage_srch" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var zPage_search = new ew_Page("zPage_search");
// page properties
zPage_search.PageID = "search"; // page ID
zPage_search.FormID = "fzPagesearch"; // form ID 
var EW_PAGE_ID = zPage_search.PageID; // for backward compatibility
// extend page with validate function for search
zPage_search.ValidateSearch = function(fobj) {
	ew_PostAutoSuggest(fobj); 
	if (this.ValidateRequired) { 
		var infix = "";
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
	for (var i=0;i<fobj.elements.length;i++) {
		var elem = fobj.elements[i];
		if (elem.name.substring(0,2) == "s_" || elem.name.substring(0,3) == "sv_")
			elem.value = "";
	}
	return true;
}
// extend page with Form_CustomValidate function
zPage_search.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
zPage_search.ValidateRequired = true; // uses JavaScript validation
<% Else %>
zPage_search.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker"><%= Language.Phrase("Search") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= zPage.TableCaption %><br /><br />
<a href="<%= zPage.ReturnUrl %>"><%= Language.Phrase("BackToList") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(zPage_search.DebugMsg) %>
<% zPage_search.ShowMessage() %>
<form name="fzPagesearch" id="fzPagesearch" method="post" onsubmit="this.action=location.pathname;return zPage_search.ValidateSearch(this);">
<p />
<input type="hidden" name="t" id="t" value="zPage" />
<input type="hidden" name="a_search" id="a_search" value="S" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
	<tr<%= zPage.RowAttributes %>>
		<td class="ewTableHeader"><%= zPage.ParentPageID.FldCaption %></td>
		<td<%= zPage.ParentPageID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_ParentPageID" id="z_ParentPageID" value="=" /></span></td>
		<td<%= zPage.ParentPageID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_ParentPageID" name="x_ParentPageID" title="<%= zPage.ParentPageID.FldTitle %>"<%= zPage.ParentPageID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(zPage.ParentPageID.EditValue) Then
	arwrk = zPage.ParentPageID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), zPage.ParentPageID.AdvancedSearch.SearchValue) Then
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
zPage_search.ar_x_ParentPageID = [<%= jswrk %>];
//-->
</script>
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= zPage.RowAttributes %>>
		<td class="ewTableHeader"><%= zPage.zPageName.FldCaption %></td>
		<td<%= zPage.zPageName.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_zPageName" id="z_zPageName" value="LIKE" /></span></td>
		<td<%= zPage.zPageName.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_zPageName" id="x_zPageName" title="<%= zPage.zPageName.FldTitle %>" size="30" maxlength="50" value="<%= zPage.zPageName.EditValue %>"<%= zPage.zPageName.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= zPage.RowAttributes %>>
		<td class="ewTableHeader"><%= zPage.PageTitle.FldCaption %></td>
		<td<%= zPage.PageTitle.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_PageTitle" id="z_PageTitle" value="LIKE" /></span></td>
		<td<%= zPage.PageTitle.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_PageTitle" id="x_PageTitle" title="<%= zPage.PageTitle.FldTitle %>" size="30" maxlength="255" value="<%= zPage.PageTitle.EditValue %>"<%= zPage.PageTitle.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= zPage.RowAttributes %>>
		<td class="ewTableHeader"><%= zPage.PageTypeID.FldCaption %></td>
		<td<%= zPage.PageTypeID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_PageTypeID" id="z_PageTypeID" value="=" /></span></td>
		<td<%= zPage.PageTypeID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_PageTypeID" name="x_PageTypeID" title="<%= zPage.PageTypeID.FldTitle %>"<%= zPage.PageTypeID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(zPage.PageTypeID.EditValue) Then
	arwrk = zPage.PageTypeID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), zPage.PageTypeID.AdvancedSearch.SearchValue) Then
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
	<tr<%= zPage.RowAttributes %>>
		<td class="ewTableHeader"><%= zPage.GroupID.FldCaption %></td>
		<td<%= zPage.GroupID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_GroupID" id="z_GroupID" value="=" /></span></td>
		<td<%= zPage.GroupID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_GroupID" name="x_GroupID" title="<%= zPage.GroupID.FldTitle %>"<%= zPage.GroupID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(zPage.GroupID.EditValue) Then
	arwrk = zPage.GroupID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), zPage.GroupID.AdvancedSearch.SearchValue) Then
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
	<tr<%= zPage.RowAttributes %>>
		<td class="ewTableHeader"><%= zPage.Active.FldCaption %></td>
		<td<%= zPage.Active.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_Active" id="z_Active" value="=" /></span></td>
		<td<%= zPage.Active.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<%
If ew_SameStr(zPage.Active.AdvancedSearch.SearchValue, "1") Then
	selwrk = " checked=""checked"""
Else
	selwrk = ""
End If
%>
<input type="checkbox" name="x_Active" id="x_Active" title="<%= zPage.Active.FldTitle %>" value="1"<%= selwrk %><%= zPage.Active.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= zPage.RowAttributes %>>
		<td class="ewTableHeader"><%= zPage.PageOrder.FldCaption %></td>
		<td<%= zPage.PageOrder.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_PageOrder" id="z_PageOrder" value="=" /></span></td>
		<td<%= zPage.PageOrder.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_PageOrder" id="x_PageOrder" title="<%= zPage.PageOrder.FldTitle %>" size="30" value="<%= zPage.PageOrder.EditValue %>"<%= zPage.PageOrder.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= zPage.RowAttributes %>>
		<td class="ewTableHeader"><%= zPage.CompanyID.FldCaption %></td>
		<td<%= zPage.CompanyID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_CompanyID" id="z_CompanyID" value="=" /></span></td>
		<td<%= zPage.CompanyID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<% zPage.CompanyID.EditAttrs("onchange") = "ew_UpdateOpt('x_ParentPageID','x_CompanyID',zPage_search.ar_x_ParentPageID);" %>
<select id="x_CompanyID" name="x_CompanyID" title="<%= zPage.CompanyID.FldTitle %>"<%= zPage.CompanyID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(zPage.CompanyID.EditValue) Then
	arwrk = zPage.CompanyID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), zPage.CompanyID.AdvancedSearch.SearchValue) Then
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
	<tr<%= zPage.RowAttributes %>>
		<td class="ewTableHeader"><%= zPage.PageDescription.FldCaption %></td>
		<td<%= zPage.PageDescription.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_PageDescription" id="z_PageDescription" value="LIKE" /></span></td>
		<td<%= zPage.PageDescription.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_PageDescription" id="x_PageDescription" title="<%= zPage.PageDescription.FldTitle %>" size="30" maxlength="255" value="<%= zPage.PageDescription.EditValue %>"<%= zPage.PageDescription.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= zPage.RowAttributes %>>
		<td class="ewTableHeader"><%= zPage.PageKeywords.FldCaption %></td>
		<td<%= zPage.PageKeywords.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_PageKeywords" id="z_PageKeywords" value="LIKE" /></span></td>
		<td<%= zPage.PageKeywords.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_PageKeywords" id="x_PageKeywords" title="<%= zPage.PageKeywords.FldTitle %>" size="30" maxlength="255" value="<%= zPage.PageKeywords.EditValue %>"<%= zPage.PageKeywords.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= zPage.RowAttributes %>>
		<td class="ewTableHeader"><%= zPage.ImagesPerRow.FldCaption %></td>
		<td<%= zPage.ImagesPerRow.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_ImagesPerRow" id="z_ImagesPerRow" value="=" /></span></td>
		<td<%= zPage.ImagesPerRow.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_ImagesPerRow" id="x_ImagesPerRow" title="<%= zPage.ImagesPerRow.FldTitle %>" size="30" value="<%= zPage.ImagesPerRow.EditValue %>"<%= zPage.ImagesPerRow.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= zPage.RowAttributes %>>
		<td class="ewTableHeader"><%= zPage.RowsPerPage.FldCaption %></td>
		<td<%= zPage.RowsPerPage.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_RowsPerPage" id="z_RowsPerPage" value="=" /></span></td>
		<td<%= zPage.RowsPerPage.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_RowsPerPage" id="x_RowsPerPage" title="<%= zPage.RowsPerPage.FldTitle %>" size="30" value="<%= zPage.RowsPerPage.EditValue %>"<%= zPage.RowsPerPage.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= zPage.RowAttributes %>>
		<td class="ewTableHeader"><%= zPage.PageFileName.FldCaption %></td>
		<td<%= zPage.PageFileName.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_PageFileName" id="z_PageFileName" value="LIKE" /></span></td>
		<td<%= zPage.PageFileName.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_PageFileName" id="x_PageFileName" title="<%= zPage.PageFileName.FldTitle %>" size="30" maxlength="50" value="<%= zPage.PageFileName.EditValue %>"<%= zPage.PageFileName.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= zPage.RowAttributes %>>
		<td class="ewTableHeader"><%= zPage.AllowMessage.FldCaption %></td>
		<td<%= zPage.AllowMessage.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_AllowMessage" id="z_AllowMessage" value="=" /></span></td>
		<td<%= zPage.AllowMessage.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<%
If ew_SameStr(zPage.AllowMessage.AdvancedSearch.SearchValue, "1") Then
	selwrk = " checked=""checked"""
Else
	selwrk = ""
End If
%>
<input type="checkbox" name="x_AllowMessage" id="x_AllowMessage" title="<%= zPage.AllowMessage.FldTitle %>" value="1"<%= selwrk %><%= zPage.AllowMessage.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= zPage.RowAttributes %>>
		<td class="ewTableHeader"><%= zPage.SiteCategoryID.FldCaption %></td>
		<td<%= zPage.SiteCategoryID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_SiteCategoryID" id="z_SiteCategoryID" value="=" /></span></td>
		<td<%= zPage.SiteCategoryID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_SiteCategoryID" id="x_SiteCategoryID" title="<%= zPage.SiteCategoryID.FldTitle %>" size="30" value="<%= zPage.SiteCategoryID.EditValue %>"<%= zPage.SiteCategoryID.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= zPage.RowAttributes %>>
		<td class="ewTableHeader"><%= zPage.SiteCategoryGroupID.FldCaption %></td>
		<td<%= zPage.SiteCategoryGroupID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_SiteCategoryGroupID" id="z_SiteCategoryGroupID" value="=" /></span></td>
		<td<%= zPage.SiteCategoryGroupID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_SiteCategoryGroupID" id="x_SiteCategoryGroupID" title="<%= zPage.SiteCategoryGroupID.FldTitle %>" size="30" value="<%= zPage.SiteCategoryGroupID.EditValue %>"<%= zPage.SiteCategoryGroupID.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= zPage.RowAttributes %>>
		<td class="ewTableHeader"><%= zPage.VersionNo.FldCaption %></td>
		<td<%= zPage.VersionNo.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_VersionNo" id="z_VersionNo" value="=" /></span></td>
		<td<%= zPage.VersionNo.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_VersionNo" id="x_VersionNo" title="<%= zPage.VersionNo.FldTitle %>" size="30" value="<%= zPage.VersionNo.EditValue %>"<%= zPage.VersionNo.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= zPage.RowAttributes %>>
		<td class="ewTableHeader"><%= zPage.ModifiedDT.FldCaption %></td>
		<td<%= zPage.ModifiedDT.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_ModifiedDT" id="z_ModifiedDT" value="=" /></span></td>
		<td<%= zPage.ModifiedDT.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_ModifiedDT" id="x_ModifiedDT" title="<%= zPage.ModifiedDT.FldTitle %>" value="<%= zPage.ModifiedDT.EditValue %>"<%= zPage.ModifiedDT.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
</table>
</div>
</td></tr></table>
<p />
<input type="submit" name="Action" id="Action" value="<%= ew_BtnCaption(Language.Phrase("Search")) %>" />
<input type="button" name="Reset" id="Reset" value="<%= ew_BtnCaption(Language.Phrase("Reset")) %>" onclick="ew_ClearForm(this.form);" />
</form>
<script language="JavaScript" type="text/javascript">
<!--
ew_UpdateOpts([['x_ParentPageID','x_CompanyID',zPage_search.ar_x_ParentPageID]]);
//-->
</script>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
</asp:Content>
