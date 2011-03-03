<%@ Page ClassName="link_add" Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="link_add.aspx.vb" Inherits="link_add" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var Link_add = new ew_Page("Link_add");
// page properties
Link_add.PageID = "add"; // page ID
Link_add.FormID = "fLinkadd"; // form ID 
var EW_PAGE_ID = Link_add.PageID; // for backward compatibility
// extend page with ValidateForm function
Link_add.ValidateForm = function(fobj) {
	ew_PostAutoSuggest(fobj); 
	if (!this.ValidateRequired)
		return true; // ignore validation
	if (fobj.a_confirm && fobj.a_confirm.value == "F")
		return true;
	var i, elm, aelm, infix;
	var rowcnt = (fobj.key_count) ? Number(fobj.key_count.value) : 1;
	for (i=0; i<rowcnt; i++) {
		infix = (fobj.key_count) ? String(i+1) : "";
		elm = fobj.elements["x" + infix + "_Ranks"];
		if (elm && elm.type != "hidden" && !ew_CheckInteger(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(Link.Ranks.FldErrMsg) %>");
		// Form Custom Validate event
		if (!this.Form_CustomValidate(fobj)) return false;
	}
	return true;
}
// extend page with Form_CustomValidate function
Link_add.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
Link_add.ValidateRequired = true; // uses JavaScript validation
<% Else %>
Link_add.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker"><%= Language.Phrase("Add") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= Link.TableCaption %><br /><br />
<a href="<%= Link.ReturnUrl %>"><%= Language.Phrase("GoBack") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(Link_add.DebugMsg) %>
<% Link_add.ShowMessage() %>
<form name="fLinkadd" id="fLinkadd" method="post" onsubmit="this.action=location.pathname;return Link_add.ValidateForm(this);">
<p />
<input type="hidden" name="t" id="t" value="Link" />
<input type="hidden" name="a_add" id="a_add" value="A" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
<% If Link.Title.Visible Then ' Title %>
	<tr<%= Link.RowAttributes %>>
		<td class="ewTableHeader"><%= Link.Title.FldCaption %></td>
		<td<%= Link.Title.CellAttributes %>><span id="el_Title">
<input type="text" name="x_Title" id="x_Title" title="<%= Link.Title.FldTitle %>" size="30" maxlength="255" value="<%= Link.Title.EditValue %>"<%= Link.Title.EditAttributes %> />
</span><%= Link.Title.CustomMsg %></td>
	</tr>
<% End If %>
<% If Link.LinkTypeCD.Visible Then ' LinkTypeCD %>
	<tr<%= Link.RowAttributes %>>
		<td class="ewTableHeader"><%= Link.LinkTypeCD.FldCaption %></td>
		<td<%= Link.LinkTypeCD.CellAttributes %>><span id="el_LinkTypeCD">
<% If Link.LinkTypeCD.SessionValue <> "" Then %>
<div<%= Link.LinkTypeCD.ViewAttributes %>><%= Link.LinkTypeCD.ViewValue %></div>
<input type="hidden" id="x_LinkTypeCD" name="x_LinkTypeCD" value="<%= ew_HtmlEncode(Link.LinkTypeCD.CurrentValue) %>">
<% Else %>
<select id="x_LinkTypeCD" name="x_LinkTypeCD" title="<%= Link.LinkTypeCD.FldTitle %>"<%= Link.LinkTypeCD.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Link.LinkTypeCD.EditValue) Then
	arwrk = Link.LinkTypeCD.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Link.LinkTypeCD.CurrentValue) Then
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
</span><%= Link.LinkTypeCD.CustomMsg %></td>
	</tr>
<% End If %>
<% If Link.CategoryID.Visible Then ' CategoryID %>
	<tr<%= Link.RowAttributes %>>
		<td class="ewTableHeader"><%= Link.CategoryID.FldCaption %></td>
		<td<%= Link.CategoryID.CellAttributes %>><span id="el_CategoryID">
<% If Link.CategoryID.SessionValue <> "" Then %>
<div<%= Link.CategoryID.ViewAttributes %>><%= Link.CategoryID.ViewValue %></div>
<input type="hidden" id="x_CategoryID" name="x_CategoryID" value="<%= ew_HtmlEncode(Link.CategoryID.CurrentValue) %>">
<% Else %>
<select id="x_CategoryID" name="x_CategoryID" title="<%= Link.CategoryID.FldTitle %>"<%= Link.CategoryID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Link.CategoryID.EditValue) Then
	arwrk = Link.CategoryID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Link.CategoryID.CurrentValue) Then
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
</span><%= Link.CategoryID.CustomMsg %></td>
	</tr>
<% End If %>
<% If Link.CompanyID.Visible Then ' CompanyID %>
	<tr<%= Link.RowAttributes %>>
		<td class="ewTableHeader"><%= Link.CompanyID.FldCaption %><%= Language.Phrase("FieldRequiredIndicator") %></td>
		<td<%= Link.CompanyID.CellAttributes %>><span id="el_CompanyID">
<% If Link.CompanyID.SessionValue <> "" Then %>
<div<%= Link.CompanyID.ViewAttributes %>><%= Link.CompanyID.ViewValue %></div>
<input type="hidden" id="x_CompanyID" name="x_CompanyID" value="<%= ew_HtmlEncode(Link.CompanyID.CurrentValue) %>">
<% Else %>
<% Link.CompanyID.EditAttrs("onchange") = "ew_UpdateOpt('x_zPageID','x_CompanyID',Link_add.ar_x_zPageID);" %>
<select id="x_CompanyID" name="x_CompanyID" title="<%= Link.CompanyID.FldTitle %>"<%= Link.CompanyID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Link.CompanyID.EditValue) Then
	arwrk = Link.CompanyID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Link.CompanyID.CurrentValue) Then
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
</span><%= Link.CompanyID.CustomMsg %></td>
	</tr>
<% End If %>
<% If Link.SiteCategoryGroupID.Visible Then ' SiteCategoryGroupID %>
	<tr<%= Link.RowAttributes %>>
		<td class="ewTableHeader"><%= Link.SiteCategoryGroupID.FldCaption %></td>
		<td<%= Link.SiteCategoryGroupID.CellAttributes %>><span id="el_SiteCategoryGroupID">
<select id="x_SiteCategoryGroupID" name="x_SiteCategoryGroupID" title="<%= Link.SiteCategoryGroupID.FldTitle %>"<%= Link.SiteCategoryGroupID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Link.SiteCategoryGroupID.EditValue) Then
	arwrk = Link.SiteCategoryGroupID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Link.SiteCategoryGroupID.CurrentValue) Then
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
</span><%= Link.SiteCategoryGroupID.CustomMsg %></td>
	</tr>
<% End If %>
<% If Link.zPageID.Visible Then ' PageID %>
	<tr<%= Link.RowAttributes %>>
		<td class="ewTableHeader"><%= Link.zPageID.FldCaption %></td>
		<td<%= Link.zPageID.CellAttributes %>><span id="el_zPageID">
<select id="x_zPageID" name="x_zPageID" title="<%= Link.zPageID.FldTitle %>"<%= Link.zPageID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Link.zPageID.EditValue) Then
	arwrk = Link.zPageID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Link.zPageID.CurrentValue) Then
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
Link_add.ar_x_zPageID = [<%= jswrk %>];
//-->
</script>
</span><%= Link.zPageID.CustomMsg %></td>
	</tr>
<% End If %>
<% If Link.Views.Visible Then ' Views %>
	<tr<%= Link.RowAttributes %>>
		<td class="ewTableHeader"><%= Link.Views.FldCaption %><%= Language.Phrase("FieldRequiredIndicator") %></td>
		<td<%= Link.Views.CellAttributes %>><span id="el_Views">
<%
If ew_SameStr(Link.Views.CurrentValue, "1") Then
	selwrk = " checked=""checked"""
Else
	selwrk = ""
End If
%>
<input type="checkbox" name="x_Views" id="x_Views" title="<%= Link.Views.FldTitle %>" value="1"<%= selwrk %><%= Link.Views.EditAttributes %> />
</span><%= Link.Views.CustomMsg %></td>
	</tr>
<% End If %>
<% If Link.Description.Visible Then ' Description %>
	<tr<%= Link.RowAttributes %>>
		<td class="ewTableHeader"><%= Link.Description.FldCaption %></td>
		<td<%= Link.Description.CellAttributes %>><span id="el_Description">
<textarea name="x_Description" id="x_Description" title="<%= Link.Description.FldTitle %>" cols="70" rows="5"<%= Link.Description.EditAttributes %>><%= Link.Description.EditValue %></textarea>
</span><%= Link.Description.CustomMsg %></td>
	</tr>
<% End If %>
<% If Link.URL.Visible Then ' URL %>
	<tr<%= Link.RowAttributes %>>
		<td class="ewTableHeader"><%= Link.URL.FldCaption %></td>
		<td<%= Link.URL.CellAttributes %>><span id="el_URL">
<textarea name="x_URL" id="x_URL" title="<%= Link.URL.FldTitle %>" cols="70" rows="5"<%= Link.URL.EditAttributes %>><%= Link.URL.EditValue %></textarea>
</span><%= Link.URL.CustomMsg %></td>
	</tr>
<% End If %>
<% If Link.Ranks.Visible Then ' Ranks %>
	<tr<%= Link.RowAttributes %>>
		<td class="ewTableHeader"><%= Link.Ranks.FldCaption %></td>
		<td<%= Link.Ranks.CellAttributes %>><span id="el_Ranks">
<input type="text" name="x_Ranks" id="x_Ranks" title="<%= Link.Ranks.FldTitle %>" size="30" value="<%= Link.Ranks.EditValue %>"<%= Link.Ranks.EditAttributes %> />
</span><%= Link.Ranks.CustomMsg %></td>
	</tr>
<% End If %>
<% If Link.UserID.Visible Then ' UserID %>
	<tr<%= Link.RowAttributes %>>
		<td class="ewTableHeader"><%= Link.UserID.FldCaption %></td>
		<td<%= Link.UserID.CellAttributes %>><span id="el_UserID">
<select id="x_UserID" name="x_UserID" title="<%= Link.UserID.FldTitle %>"<%= Link.UserID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Link.UserID.EditValue) Then
	arwrk = Link.UserID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Link.UserID.CurrentValue) Then
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
</span><%= Link.UserID.CustomMsg %></td>
	</tr>
<% End If %>
<% If Link.ASIN.Visible Then ' ASIN %>
	<tr<%= Link.RowAttributes %>>
		<td class="ewTableHeader"><%= Link.ASIN.FldCaption %></td>
		<td<%= Link.ASIN.CellAttributes %>><span id="el_ASIN">
<input type="text" name="x_ASIN" id="x_ASIN" title="<%= Link.ASIN.FldTitle %>" size="30" maxlength="50" value="<%= Link.ASIN.EditValue %>"<%= Link.ASIN.EditAttributes %> />
</span><%= Link.ASIN.CustomMsg %></td>
	</tr>
<% End If %>
<% If Link.DateAdd.Visible Then ' DateAdd %>
	<tr<%= Link.RowAttributes %>>
		<td class="ewTableHeader"><%= Link.DateAdd.FldCaption %></td>
		<td<%= Link.DateAdd.CellAttributes %>><span id="el_DateAdd">
<input type="text" name="x_DateAdd" id="x_DateAdd" title="<%= Link.DateAdd.FldTitle %>" value="<%= Link.DateAdd.EditValue %>"<%= Link.DateAdd.EditAttributes %> />
</span><%= Link.DateAdd.CustomMsg %></td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<p />
<input type="submit" name="btnAction" id="btnAction" value="<%= ew_BtnCaption(Language.Phrase("AddBtn")) %>" />
</form>
<script language="JavaScript" type="text/javascript">
<!--
ew_UpdateOpts([['x_zPageID','x_CompanyID',Link_add.ar_x_zPageID]]);
//-->
</script>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
</asp:Content>
