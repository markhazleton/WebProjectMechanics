<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="register.aspx.vb" Inherits="_register" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var register = new ew_Page("register");
// page properties
register.PageID = "register"; // page ID
var EW_PAGE_ID = register.PageID; // for backward compatibility
// extend page with ValidateForm function
register.ValidateForm = function(fobj) {
	if (!this.ValidateRequired)
		return true; // ignore validation
	if (fobj.a_confirm && fobj.a_confirm.value == "F")
		return true;
	var i, elm, aelm, infix;
	var rowcnt = (fobj.key_count) ? Number(fobj.key_count.value) : 1;
	for (i=0; i<rowcnt; i++) {
		infix = (fobj.key_count) ? String(i+1) : "";
		elm = fobj.elements["x" + infix + "_LogonPassword"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field - Logon Password");
		if (fobj.x_LogonPassword && !ew_HasValue(fobj.x_LogonPassword))
			return ew_OnError(this, fobj.x_LogonPassword, "Please enter password");
		if (fobj.c_LogonPassword.value != fobj.x_LogonPassword.value)
			return ew_OnError(this, fobj.c_LogonPassword, "Mismatch Password");
		elm = fobj.elements["x" + infix + "_zEMail"];
		if (elm && !ew_CheckEmail(elm.value))
			return ew_OnError(this, elm, "Incorrect email - EMail");
	}
	return true;
}
<% If EW_CLIENT_VALIDATE Then %>
register.ValidateRequired = true; // uses JavaScript validation
<% Else %>
register.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">Registration Page<br /><br />
<a href="login.aspx">Back to login page</a></span></p>
<% register.ShowMessage() %>
<form name="fContactregister" id="fContactregister" method="post" onsubmit="this.action=location.pathname;return register.ValidateForm(this);">
<p />
<input type="hidden" name="t" id="t" value="Contact" />
<input type="hidden" name="a_register" id="a_register" value="A" />
<% If Contact.CurrentAction = "F" Then ' Confirm page %>
<input type="hidden" name="a_confirm" id="a_confirm" value="F" />
<% End If %>
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table class="ewTable">
	<tr<%= Contact.LogonPassword.RowAttributes %>>
		<td class="ewTableHeader">Logon Password<span class="ewRequired">&nbsp;*</span></td>
		<td<%= Contact.LogonPassword.CellAttributes %>><span id="el_LogonPassword">
<% If Contact.CurrentAction <> "F" Then %>
<input type="password" name="x_LogonPassword" id="x_LogonPassword" size="30" maxlength="50"<%= Contact.LogonPassword.EditAttributes %> />
<% Else %>
<div<%= Contact.LogonPassword.ViewAttributes %>><%= Contact.LogonPassword.ViewValue %></div>
<input type="hidden" name="x_LogonPassword" id="x_LogonPassword" value="<%= ew_HTMLEncode(Contact.LogonPassword.CurrentValue) %>" />
<% End If %>
</span><%= Contact.LogonPassword.CustomMsg %></td>
	</tr>
	<tr<%= Contact.LogonPassword.RowAttributes %>>
		<td class="ewTableHeader">Confirm Logon Password</td>
		<td<%= Contact.LogonPassword.CellAttributes %>>
<% If Contact.CurrentAction <> "F" Then %>
<input type="password" name="c_LogonPassword" id="c_LogonPassword" size="30" maxlength="50"<%= Contact.LogonPassword.EditAttributes %> />
<% Else %>
<div<%= Contact.LogonPassword.ViewAttributes %>><%= Contact.LogonPassword.ViewValue %></div>
<input type="hidden" name="c_LogonPassword" id="c_LogonPassword" value="<%= ew_HTMLEncode(Contact.LogonPassword.CurrentValue) %>" />
<% End If %>
</td>
	</tr>
	<tr<%= Contact.PrimaryContact.RowAttributes %>>
		<td class="ewTableHeader">Full Name</td>
		<td<%= Contact.PrimaryContact.CellAttributes %>><span id="el_PrimaryContact">
<% If Contact.CurrentAction <> "F" Then %>
<input type="text" name="x_PrimaryContact" id="x_PrimaryContact" size="30" maxlength="255" value="<%= Contact.PrimaryContact.EditValue %>"<%= Contact.PrimaryContact.EditAttributes %> />
<% Else %>
<div<%= Contact.PrimaryContact.ViewAttributes %>><%= Contact.PrimaryContact.ViewValue %></div>
<input type="hidden" name="x_PrimaryContact" id="x_PrimaryContact" value="<%= ew_HTMLEncode(Contact.PrimaryContact.CurrentValue) %>" />
<% End If %>
</span><%= Contact.PrimaryContact.CustomMsg %></td>
	</tr>
	<tr<%= Contact.zEMail.RowAttributes %>>
		<td class="ewTableHeader">EMail</td>
		<td<%= Contact.zEMail.CellAttributes %>><span id="el_zEMail">
<% If Contact.CurrentAction <> "F" Then %>
<input type="text" name="x_zEMail" id="x_zEMail" size="30" maxlength="50" value="<%= Contact.zEMail.EditValue %>"<%= Contact.zEMail.EditAttributes %> />
<% Else %>
<div<%= Contact.zEMail.ViewAttributes %>><%= Contact.zEMail.ViewValue %></div>
<input type="hidden" name="x_zEMail" id="x_zEMail" value="<%= ew_HTMLEncode(Contact.zEMail.CurrentValue) %>" />
<% End If %>
</span><%= Contact.zEMail.CustomMsg %></td>
	</tr>
</table>
</div>
</td></tr></table>
<p />
<% If Contact.CurrentAction <> "F" Then ' Confirm page %>
<input type="submit" name="btnAction" id="btnAction" value=" Register " onclick="this.form.a_register.value='F';" />
<% Else %>
<input type="submit" name="btnCancel" id="btnCancel" value="  Cancel  " onclick="this.form.a_register.value='X';" />
<input type="submit" name="btnAction" id="btnAction" value="  Confirm  " />
<% End If %>
</form>
<% If Contact.CurrentAction <> "F" Then %>
<% End If %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your startup script here
// document.write("page loaded");
//-->
</script>
</asp:Content>
