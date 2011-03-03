<%@ Page ClassName="link_srch" Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="link_srch.aspx.vb" Inherits="link_srch" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var Link_search = new ew_Page("Link_search");
// page properties
Link_search.PageID = "search"; // page ID
Link_search.FormID = "fLinksearch"; // form ID 
var EW_PAGE_ID = Link_search.PageID; // for backward compatibility
// extend page with validate function for search
Link_search.ValidateSearch = function(fobj) {
	ew_PostAutoSuggest(fobj); 
	if (this.ValidateRequired) { 
		var infix = "";
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
Link_search.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
Link_search.ValidateRequired = true; // uses JavaScript validation
<% Else %>
Link_search.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker"><%= Language.Phrase("Search") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= Link.TableCaption %><br /><br />
<a href="<%= Link.ReturnUrl %>"><%= Language.Phrase("BackToList") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(Link_search.DebugMsg) %>
<% Link_search.ShowMessage() %>
<form name="fLinksearch" id="fLinksearch" method="post" onsubmit="this.action=location.pathname;return Link_search.ValidateSearch(this);">
<p />
<input type="hidden" name="t" id="t" value="Link" />
<input type="hidden" name="a_search" id="a_search" value="S" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
	<tr<%= Link.RowAttributes %>>
		<td class="ewTableHeader"><%= Link.Title.FldCaption %></td>
		<td<%= Link.Title.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_Title" id="z_Title" value="LIKE" /></span></td>
		<td<%= Link.Title.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_Title" id="x_Title" title="<%= Link.Title.FldTitle %>" size="30" maxlength="255" value="<%= Link.Title.EditValue %>"<%= Link.Title.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Link.RowAttributes %>>
		<td class="ewTableHeader"><%= Link.LinkTypeCD.FldCaption %></td>
		<td<%= Link.LinkTypeCD.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_LinkTypeCD" id="z_LinkTypeCD" value="LIKE" /></span></td>
		<td<%= Link.LinkTypeCD.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_LinkTypeCD" name="x_LinkTypeCD" title="<%= Link.LinkTypeCD.FldTitle %>"<%= Link.LinkTypeCD.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Link.LinkTypeCD.EditValue) Then
	arwrk = Link.LinkTypeCD.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Link.LinkTypeCD.AdvancedSearch.SearchValue) Then
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
	<tr<%= Link.RowAttributes %>>
		<td class="ewTableHeader"><%= Link.CategoryID.FldCaption %></td>
		<td<%= Link.CategoryID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_CategoryID" id="z_CategoryID" value="=" /></span></td>
		<td<%= Link.CategoryID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_CategoryID" name="x_CategoryID" title="<%= Link.CategoryID.FldTitle %>"<%= Link.CategoryID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Link.CategoryID.EditValue) Then
	arwrk = Link.CategoryID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Link.CategoryID.AdvancedSearch.SearchValue) Then
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
	<tr<%= Link.RowAttributes %>>
		<td class="ewTableHeader"><%= Link.CompanyID.FldCaption %></td>
		<td<%= Link.CompanyID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_CompanyID" id="z_CompanyID" value="=" /></span></td>
		<td<%= Link.CompanyID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<% Link.CompanyID.EditAttrs("onchange") = "ew_UpdateOpt('x_zPageID','x_CompanyID',Link_search.ar_x_zPageID);" %>
<select id="x_CompanyID" name="x_CompanyID" title="<%= Link.CompanyID.FldTitle %>"<%= Link.CompanyID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Link.CompanyID.EditValue) Then
	arwrk = Link.CompanyID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Link.CompanyID.AdvancedSearch.SearchValue) Then
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
	<tr<%= Link.RowAttributes %>>
		<td class="ewTableHeader"><%= Link.SiteCategoryGroupID.FldCaption %></td>
		<td<%= Link.SiteCategoryGroupID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_SiteCategoryGroupID" id="z_SiteCategoryGroupID" value="=" /></span></td>
		<td<%= Link.SiteCategoryGroupID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_SiteCategoryGroupID" name="x_SiteCategoryGroupID" title="<%= Link.SiteCategoryGroupID.FldTitle %>"<%= Link.SiteCategoryGroupID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Link.SiteCategoryGroupID.EditValue) Then
	arwrk = Link.SiteCategoryGroupID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Link.SiteCategoryGroupID.AdvancedSearch.SearchValue) Then
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
	<tr<%= Link.RowAttributes %>>
		<td class="ewTableHeader"><%= Link.zPageID.FldCaption %></td>
		<td<%= Link.zPageID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_zPageID" id="z_zPageID" value="=" /></span></td>
		<td<%= Link.zPageID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_zPageID" name="x_zPageID" title="<%= Link.zPageID.FldTitle %>"<%= Link.zPageID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Link.zPageID.EditValue) Then
	arwrk = Link.zPageID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Link.zPageID.AdvancedSearch.SearchValue) Then
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
If ew_IsArrayList(Link.zPageID.EditValue) Then
	arwrk = Link.zPageID.EditValue
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
Link_search.ar_x_zPageID = [<%= jswrk %>];
//-->
</script>
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Link.RowAttributes %>>
		<td class="ewTableHeader"><%= Link.Views.FldCaption %></td>
		<td<%= Link.Views.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_Views" id="z_Views" value="=" /></span></td>
		<td<%= Link.Views.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<%
If ew_SameStr(Link.Views.AdvancedSearch.SearchValue, "1") Then
	selwrk = " checked=""checked"""
Else
	selwrk = ""
End If
%>
<input type="checkbox" name="x_Views" id="x_Views" title="<%= Link.Views.FldTitle %>" value="1"<%= selwrk %><%= Link.Views.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Link.RowAttributes %>>
		<td class="ewTableHeader"><%= Link.Description.FldCaption %></td>
		<td<%= Link.Description.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_Description" id="z_Description" value="LIKE" /></span></td>
		<td<%= Link.Description.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<textarea name="x_Description" id="x_Description" title="<%= Link.Description.FldTitle %>" cols="70" rows="5"<%= Link.Description.EditAttributes %>><%= Link.Description.EditValue %></textarea>
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Link.RowAttributes %>>
		<td class="ewTableHeader"><%= Link.ASIN.FldCaption %></td>
		<td<%= Link.ASIN.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_ASIN" id="z_ASIN" value="LIKE" /></span></td>
		<td<%= Link.ASIN.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_ASIN" id="x_ASIN" title="<%= Link.ASIN.FldTitle %>" size="30" maxlength="50" value="<%= Link.ASIN.EditValue %>"<%= Link.ASIN.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Link.RowAttributes %>>
		<td class="ewTableHeader"><%= Link.DateAdd.FldCaption %></td>
		<td<%= Link.DateAdd.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_DateAdd" id="z_DateAdd" value="=" /></span></td>
		<td<%= Link.DateAdd.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_DateAdd" id="x_DateAdd" title="<%= Link.DateAdd.FldTitle %>" value="<%= Link.DateAdd.EditValue %>"<%= Link.DateAdd.EditAttributes %> />
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
ew_UpdateOpts([['x_zPageID','x_CompanyID',Link_search.ar_x_zPageID]]);
//-->
</script>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
</asp:Content>
