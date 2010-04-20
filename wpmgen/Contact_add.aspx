<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="Contact_add.aspx.vb" Inherits="Contact_add" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var Contact_add = new ew_Page("Contact_add");
// page properties
Contact_add.PageID = "add"; // page ID
var EW_PAGE_ID = Contact_add.PageID; // for backward compatibility
// extend page with ValidateForm function
Contact_add.ValidateForm = function(fobj) {
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
			return ew_OnError(this, elm, "Please enter required field - Logon Name");
		elm = fobj.elements["x" + infix + "_LogonPassword"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field - Logon Password");
		elm = fobj.elements["x" + infix + "_zEMail"];
		if (elm && !ew_CheckEmail(elm.value))
			return ew_OnError(this, elm, "Incorrect email - EMail");
		elm = fobj.elements["x" + infix + "_TemplatePrefix"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field - Name");
	}
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
// To include another .js script, use:
// ew_ClientScriptInclude("my_javascript.js"); 
//-->
</script>
<p><span class="aspnetmaker">Add to TABLE: Site Contact<br /><br />
<a href="<%= Contact.ReturnUrl %>">Go Back</a></span></p>
<% Contact_add.ShowMessage() %>
<form name="fContactadd" id="fContactadd" method="post" onsubmit="this.action=location.pathname;return Contact_add.ValidateForm(this);">
<p />
<input type="hidden" name="t" id="t" value="Contact" />
<input type="hidden" name="a_add" id="a_add" value="A" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
<% If Contact.LogonName.Visible Then ' LogonName %>
	<tr<%= Contact.LogonName.RowAttributes %>>
		<td class="ewTableHeader">Logon Name<span class="ewRequired">&nbsp;*</span></td>
		<td<%= Contact.LogonName.CellAttributes %>><span id="el_LogonName">
<input type="text" name="x_LogonName" id="x_LogonName" size="30" maxlength="50" value="<%= Contact.LogonName.EditValue %>"<%= Contact.LogonName.EditAttributes %> />
</span><%= Contact.LogonName.CustomMsg %></td>
	</tr>
<% End If %>
<% If Contact.LogonPassword.Visible Then ' LogonPassword %>
	<tr<%= Contact.LogonPassword.RowAttributes %>>
		<td class="ewTableHeader">Logon Password<span class="ewRequired">&nbsp;*</span></td>
		<td<%= Contact.LogonPassword.CellAttributes %>><span id="el_LogonPassword">
<input type="password" name="x_LogonPassword" id="x_LogonPassword" size="30" maxlength="50"<%= Contact.LogonPassword.EditAttributes %> />
</span><%= Contact.LogonPassword.CustomMsg %></td>
	</tr>
<% End If %>
<% If Contact.PrimaryContact.Visible Then ' PrimaryContact %>
	<tr<%= Contact.PrimaryContact.RowAttributes %>>
		<td class="ewTableHeader">Full Name</td>
		<td<%= Contact.PrimaryContact.CellAttributes %>><span id="el_PrimaryContact">
<input type="text" name="x_PrimaryContact" id="x_PrimaryContact" size="30" maxlength="255" value="<%= Contact.PrimaryContact.EditValue %>"<%= Contact.PrimaryContact.EditAttributes %> />
</span><%= Contact.PrimaryContact.CustomMsg %></td>
	</tr>
<% End If %>
<% If Contact.zEMail.Visible Then ' EMail %>
	<tr<%= Contact.zEMail.RowAttributes %>>
		<td class="ewTableHeader">EMail</td>
		<td<%= Contact.zEMail.CellAttributes %>><span id="el_zEMail">
<input type="text" name="x_zEMail" id="x_zEMail" size="30" maxlength="50" value="<%= Contact.zEMail.EditValue %>"<%= Contact.zEMail.EditAttributes %> />
</span><%= Contact.zEMail.CustomMsg %></td>
	</tr>
<% End If %>
<% If Contact.Active.Visible Then ' Active %>
	<tr<%= Contact.Active.RowAttributes %>>
		<td class="ewTableHeader">Active</td>
		<td<%= Contact.Active.CellAttributes %>><span id="el_Active">
<%
If ew_SameStr(Contact.Active.CurrentValue, "1") Then
	selwrk = " checked=""checked"""
Else
	selwrk = ""
End If
%>
<input type="checkbox" name="x_Active" id="x_Active" value="1"<%= selwrk %><%= Contact.Active.EditAttributes %> />
</span><%= Contact.Active.CustomMsg %></td>
	</tr>
<% End If %>
<% If Contact.CompanyID.Visible Then ' CompanyID %>
	<tr<%= Contact.CompanyID.RowAttributes %>>
		<td class="ewTableHeader">Company</td>
		<td<%= Contact.CompanyID.CellAttributes %>><span id="el_CompanyID">
<select id="x_CompanyID" name="x_CompanyID"<%= Contact.CompanyID.EditAttributes %>>
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
<% If Contact.GroupID.Visible Then ' GroupID %>
	<tr<%= Contact.GroupID.RowAttributes %>>
		<td class="ewTableHeader">Group</td>
		<td<%= Contact.GroupID.CellAttributes %>><span id="el_GroupID">
<select id="x_GroupID" name="x_GroupID"<%= Contact.GroupID.EditAttributes %>>
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
<% If Contact.TemplatePrefix.Visible Then ' TemplatePrefix %>
	<tr<%= Contact.TemplatePrefix.RowAttributes %>>
		<td class="ewTableHeader">Name<span class="ewRequired">&nbsp;*</span></td>
		<td<%= Contact.TemplatePrefix.CellAttributes %>><span id="el_TemplatePrefix">
<select id="x_TemplatePrefix" name="x_TemplatePrefix"<%= Contact.TemplatePrefix.EditAttributes %>>
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
<% If Contact.RoleID.Visible Then ' RoleID %>
	<tr<%= Contact.RoleID.RowAttributes %>>
		<td class="ewTableHeader">Role ID</td>
		<td<%= Contact.RoleID.CellAttributes %>><span id="el_RoleID">
<select id="x_RoleID" name="x_RoleID"<%= Contact.RoleID.EditAttributes %>>
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
<input type="submit" name="btnAction" id="btnAction" value=" Save New " />
</form>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
</asp:Content>
