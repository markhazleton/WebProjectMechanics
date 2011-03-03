<%@ Page ClassName="contact_add" Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="contact_add.aspx.vb" Inherits="contact_add" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var Contact_add = new ew_Page("Contact_add");
// page properties
Contact_add.PageID = "add"; // page ID
Contact_add.FormID = "fContactadd"; // form ID 
var EW_PAGE_ID = Contact_add.PageID; // for backward compatibility
// extend page with ValidateForm function
Contact_add.ValidateForm = function(fobj) {
	ew_PostAutoSuggest(fobj); 
	if (!this.ValidateRequired)
		return true; // ignore validation
	if (fobj.a_confirm && fobj.a_confirm.value == "F")
		return true;
	var i, elm, aelm, infix;
	var rowcnt = (fobj.key_count) ? Number(fobj.key_count.value) : 1;
	for (i=0; i<rowcnt; i++) {
		infix = (fobj.key_count) ? String(i+1) : "";
		elm = fobj.elements["x" + infix + "_LogonName"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, ewLanguage.Phrase("EnterRequiredField") + " - <%= ew_JsEncode2(Contact.LogonName.FldCaption) %>");
		// Form Custom Validate event
		if (!this.Form_CustomValidate(fobj)) return false;
	}
	return true;
}
// extend page with Form_CustomValidate function
Contact_add.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
Contact_add.ValidateRequired = true; // uses JavaScript validation
<% Else %>
Contact_add.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker"><%= Language.Phrase("Add") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= Contact.TableCaption %><br /><br />
<a href="<%= Contact.ReturnUrl %>"><%= Language.Phrase("GoBack") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(Contact_add.DebugMsg) %>
<% Contact_add.ShowMessage() %>
<form name="fContactadd" id="fContactadd" method="post" onsubmit="this.action=location.pathname;return Contact_add.ValidateForm(this);">
<p />
<input type="hidden" name="t" id="t" value="Contact" />
<input type="hidden" name="a_add" id="a_add" value="A" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
<% If Contact.LogonName.Visible Then ' LogonName %>
	<tr<%= Contact.RowAttributes %>>
		<td class="ewTableHeader"><%= Contact.LogonName.FldCaption %><%= Language.Phrase("FieldRequiredIndicator") %></td>
		<td<%= Contact.LogonName.CellAttributes %>><span id="el_LogonName">
<input type="text" name="x_LogonName" id="x_LogonName" title="<%= Contact.LogonName.FldTitle %>" size="30" maxlength="50" value="<%= Contact.LogonName.EditValue %>"<%= Contact.LogonName.EditAttributes %> />
</span><%= Contact.LogonName.CustomMsg %></td>
	</tr>
<% End If %>
<% If Contact.LogonPassword.Visible Then ' LogonPassword %>
	<tr<%= Contact.RowAttributes %>>
		<td class="ewTableHeader"><%= Contact.LogonPassword.FldCaption %></td>
		<td<%= Contact.LogonPassword.CellAttributes %>><span id="el_LogonPassword">
<input type="text" name="x_LogonPassword" id="x_LogonPassword" title="<%= Contact.LogonPassword.FldTitle %>" size="30" maxlength="50" value="<%= Contact.LogonPassword.EditValue %>"<%= Contact.LogonPassword.EditAttributes %> />
</span><%= Contact.LogonPassword.CustomMsg %></td>
	</tr>
<% End If %>
<% If Contact.GroupID.Visible Then ' GroupID %>
	<tr<%= Contact.RowAttributes %>>
		<td class="ewTableHeader"><%= Contact.GroupID.FldCaption %></td>
		<td<%= Contact.GroupID.CellAttributes %>><span id="el_GroupID">
<select id="x_GroupID" name="x_GroupID" title="<%= Contact.GroupID.FldTitle %>"<%= Contact.GroupID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Contact.GroupID.EditValue) Then
	arwrk = Contact.GroupID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Contact.GroupID.CurrentValue) Then
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
</span><%= Contact.GroupID.CustomMsg %></td>
	</tr>
<% End If %>
<% If Contact.CompanyID.Visible Then ' CompanyID %>
	<tr<%= Contact.RowAttributes %>>
		<td class="ewTableHeader"><%= Contact.CompanyID.FldCaption %></td>
		<td<%= Contact.CompanyID.CellAttributes %>><span id="el_CompanyID">
<select id="x_CompanyID" name="x_CompanyID" title="<%= Contact.CompanyID.FldTitle %>"<%= Contact.CompanyID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Contact.CompanyID.EditValue) Then
	arwrk = Contact.CompanyID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Contact.CompanyID.CurrentValue) Then
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
</span><%= Contact.CompanyID.CustomMsg %></td>
	</tr>
<% End If %>
<% If Contact.TemplatePrefix.Visible Then ' TemplatePrefix %>
	<tr<%= Contact.RowAttributes %>>
		<td class="ewTableHeader"><%= Contact.TemplatePrefix.FldCaption %></td>
		<td<%= Contact.TemplatePrefix.CellAttributes %>><span id="el_TemplatePrefix">
<select id="x_TemplatePrefix" name="x_TemplatePrefix" title="<%= Contact.TemplatePrefix.FldTitle %>"<%= Contact.TemplatePrefix.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Contact.TemplatePrefix.EditValue) Then
	arwrk = Contact.TemplatePrefix.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Contact.TemplatePrefix.CurrentValue) Then
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
</span><%= Contact.TemplatePrefix.CustomMsg %></td>
	</tr>
<% End If %>
<% If Contact.Active.Visible Then ' Active %>
	<tr<%= Contact.RowAttributes %>>
		<td class="ewTableHeader"><%= Contact.Active.FldCaption %></td>
		<td<%= Contact.Active.CellAttributes %>><span id="el_Active">
<%
If ew_SameStr(Contact.Active.CurrentValue, "1") Then
	selwrk = " checked=""checked"""
Else
	selwrk = ""
End If
%>
<input type="checkbox" name="x_Active" id="x_Active" title="<%= Contact.Active.FldTitle %>" value="1"<%= selwrk %><%= Contact.Active.EditAttributes %> />
</span><%= Contact.Active.CustomMsg %></td>
	</tr>
<% End If %>
<% If Contact.zEMail.Visible Then ' EMail %>
	<tr<%= Contact.RowAttributes %>>
		<td class="ewTableHeader"><%= Contact.zEMail.FldCaption %></td>
		<td<%= Contact.zEMail.CellAttributes %>><span id="el_zEMail">
<input type="text" name="x_zEMail" id="x_zEMail" title="<%= Contact.zEMail.FldTitle %>" size="30" maxlength="50" value="<%= Contact.zEMail.EditValue %>"<%= Contact.zEMail.EditAttributes %> />
</span><%= Contact.zEMail.CustomMsg %></td>
	</tr>
<% End If %>
<% If Contact.PrimaryContact.Visible Then ' PrimaryContact %>
	<tr<%= Contact.RowAttributes %>>
		<td class="ewTableHeader"><%= Contact.PrimaryContact.FldCaption %></td>
		<td<%= Contact.PrimaryContact.CellAttributes %>><span id="el_PrimaryContact">
<input type="text" name="x_PrimaryContact" id="x_PrimaryContact" title="<%= Contact.PrimaryContact.FldTitle %>" size="30" maxlength="255" value="<%= Contact.PrimaryContact.EditValue %>"<%= Contact.PrimaryContact.EditAttributes %> />
</span><%= Contact.PrimaryContact.CustomMsg %></td>
	</tr>
<% End If %>
<% If Contact.RoleID.Visible Then ' RoleID %>
	<tr<%= Contact.RowAttributes %>>
		<td class="ewTableHeader"><%= Contact.RoleID.FldCaption %></td>
		<td<%= Contact.RoleID.CellAttributes %>><span id="el_RoleID">
<select id="x_RoleID" name="x_RoleID" title="<%= Contact.RoleID.FldTitle %>"<%= Contact.RoleID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Contact.RoleID.EditValue) Then
	arwrk = Contact.RoleID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Contact.RoleID.CurrentValue) Then
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
</span><%= Contact.RoleID.CustomMsg %></td>
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
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
</asp:Content>
