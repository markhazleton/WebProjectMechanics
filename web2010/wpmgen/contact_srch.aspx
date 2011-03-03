<%@ Page ClassName="contact_srch" Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="contact_srch.aspx.vb" Inherits="contact_srch" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var Contact_search = new ew_Page("Contact_search");
// page properties
Contact_search.PageID = "search"; // page ID
Contact_search.FormID = "fContactsearch"; // form ID 
var EW_PAGE_ID = Contact_search.PageID; // for backward compatibility
// extend page with validate function for search
Contact_search.ValidateSearch = function(fobj) {
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
Contact_search.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
Contact_search.ValidateRequired = true; // uses JavaScript validation
<% Else %>
Contact_search.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker"><%= Language.Phrase("Search") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= Contact.TableCaption %><br /><br />
<a href="<%= Contact.ReturnUrl %>"><%= Language.Phrase("BackToList") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(Contact_search.DebugMsg) %>
<% Contact_search.ShowMessage() %>
<form name="fContactsearch" id="fContactsearch" method="post" onsubmit="this.action=location.pathname;return Contact_search.ValidateSearch(this);">
<p />
<input type="hidden" name="t" id="t" value="Contact" />
<input type="hidden" name="a_search" id="a_search" value="S" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
	<tr<%= Contact.RowAttributes %>>
		<td class="ewTableHeader"><%= Contact.LogonName.FldCaption %></td>
		<td<%= Contact.LogonName.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_LogonName" id="z_LogonName" value="LIKE" /></span></td>
		<td<%= Contact.LogonName.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_LogonName" id="x_LogonName" title="<%= Contact.LogonName.FldTitle %>" size="30" maxlength="50" value="<%= Contact.LogonName.EditValue %>"<%= Contact.LogonName.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Contact.RowAttributes %>>
		<td class="ewTableHeader"><%= Contact.GroupID.FldCaption %></td>
		<td<%= Contact.GroupID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_GroupID" id="z_GroupID" value="=" /></span></td>
		<td<%= Contact.GroupID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_GroupID" name="x_GroupID" title="<%= Contact.GroupID.FldTitle %>"<%= Contact.GroupID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Contact.GroupID.EditValue) Then
	arwrk = Contact.GroupID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Contact.GroupID.AdvancedSearch.SearchValue) Then
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
	<tr<%= Contact.RowAttributes %>>
		<td class="ewTableHeader"><%= Contact.CompanyID.FldCaption %></td>
		<td<%= Contact.CompanyID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_CompanyID" id="z_CompanyID" value="=" /></span></td>
		<td<%= Contact.CompanyID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_CompanyID" name="x_CompanyID" title="<%= Contact.CompanyID.FldTitle %>"<%= Contact.CompanyID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Contact.CompanyID.EditValue) Then
	arwrk = Contact.CompanyID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Contact.CompanyID.AdvancedSearch.SearchValue) Then
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
	<tr<%= Contact.RowAttributes %>>
		<td class="ewTableHeader"><%= Contact.TemplatePrefix.FldCaption %></td>
		<td<%= Contact.TemplatePrefix.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_TemplatePrefix" id="z_TemplatePrefix" value="=" /></span></td>
		<td<%= Contact.TemplatePrefix.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_TemplatePrefix" name="x_TemplatePrefix" title="<%= Contact.TemplatePrefix.FldTitle %>"<%= Contact.TemplatePrefix.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Contact.TemplatePrefix.EditValue) Then
	arwrk = Contact.TemplatePrefix.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Contact.TemplatePrefix.AdvancedSearch.SearchValue) Then
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
<input type="submit" name="Action" id="Action" value="<%= ew_BtnCaption(Language.Phrase("Search")) %>" />
<input type="button" name="Reset" id="Reset" value="<%= ew_BtnCaption(Language.Phrase("Reset")) %>" onclick="ew_ClearForm(this.form);" />
</form>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
</asp:Content>
