<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="login.aspx.vb" Inherits="_login" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script language="JavaScript" type="text/javascript">
<!--
// Write your client script here, no need to add script tags.
// To include another .js script, use:
// ew_ClientScriptInclude("my_javascript.js"); 
//-->
</script>
<script type="text/javascript">
<!--
var login = new ew_Page("login");
// extend page with ValidateForm function
login.ValidateForm = function(fobj)
{
	if (!this.ValidateRequired)
		return true; // ignore validation
	if (!ew_HasValue(fobj.username))
		return ew_OnError(this, fobj.username, "Please enter user ID");
	if (!ew_HasValue(fobj.password))
		return ew_OnError(this, fobj.password, "Please enter password");
	return true;
}
// requires js validation
<% If EW_CLIENT_VALIDATE Then %>
login.ValidateRequired = true;
<% Else %>
login.ValidateRequired = false;
<% End If %>
//-->
</script>
<p><span class="aspnetmaker">Login Page</span></p>
<% login.ShowMessage() %>
<form method="post" onsubmit="return login.ValidateForm(this);">
<table border="0" cellspacing="0" cellpadding="4">
	<tr>
		<td><span class="aspnetmaker">User Name</span></td>
		<td><span class="aspnetmaker"><input type="text" name="username" id="username" size="20" value="<%= login.sUsername %>" /></span></td>
	</tr>
	<tr>
		<td><span class="aspnetmaker">Password</span></td>
		<td><span class="aspnetmaker"><input type="password" name="password" id="password" size="20" /></span></td>
	</tr>
	<tr>
		<td>&nbsp;</td>
		<td><span class="aspnetmaker">
		<label><input type="radio" name="rememberme" id="rememberme" value="a"<% If login.sLoginType = "a" Then %> checked="checked"<% End If %> />Auto login until I logout explicitly</label><br />
		<label><input type="radio" name="rememberme" id="rememberme" value="u"<% If login.sLoginType = "u" Then %>  checked="checked"<% End If %> />Save my user name</label><br />
		<label><input type="radio" name="rememberme" id="rememberme" value=""<% If login.sLoginType = "" Then %> checked="checked"<% End If %> />Always ask for my user name and password</label>
		</span></td>
	</tr>
	<tr>
		<td colspan="2" align="center"><span class="aspnetmaker"><input type="submit" name="submit" id="submit" value="   Login   " /></span></td>
	</tr>
</table>
</form>
<br />
<p><span class="aspnetmaker">
<a href="forgetpwd.aspx">Forgot Password</a>&nbsp;&nbsp;&nbsp;&nbsp;
<a href="register.aspx">Register</a>&nbsp;&nbsp;&nbsp;&nbsp;
</span></p>
<script language="JavaScript" type="text/javascript">
<!--
// Write your startup script here
// document.write("page loaded");
//-->
</script>
</asp:Content>
