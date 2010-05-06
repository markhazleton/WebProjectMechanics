<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="forgetpwd.aspx.vb" Inherits="_forgetpwd" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
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
var forgetpwd = new ew_Page("forgetpwd");
// extend page with ValidateForm function
forgetpwd.ValidateForm = function(fobj)
{
	if (!this.ValidateRequired)
		return true; // ignore validation
	if  (!ew_HasValue(fobj.email))
		return ew_OnError(this, fobj.email, "Please enter valid Email Address!");
	if  (!ew_CheckEmail(fobj.email.value))
		return ew_OnError(this, fobj.email, "Please enter valid Email Address!");
	return true;
}
// requires js validation
<% If EW_CLIENT_VALIDATE Then %>
forgetpwd.ValidateRequired = true;
<% Else %>
forgetpwd.ValidateRequired = false;
<% End If %>
//-->
</script>
<p><span class="aspnetmaker">Request Password Page<br /><br />
<a href="login.aspx">Back to login page</a></span></p>
<% forgetpwd.ShowMessage() %>
<form method="post" onsubmit="return forgetpwd.ValidateForm(this);">
<table border="0" cellspacing="0" cellpadding="4">
	<tr>
		<td><span class="aspnetmaker">User Email</span></td>
		<td><span class="aspnetmaker"><input type="text" name="email" id="email" value="<%= ew_HtmlEncode(forgetpwd.sEmail) %>" size="30" maxlength="50" /></span></td>
	</tr>
	<tr>
		<td>&nbsp;</td>
		<td><span class="aspnetmaker"><input type="submit" name="submit" id="submit" value="Send Password" /></span></td>
	</tr>
</table>
</form>
<br />
<script language="JavaScript" type="text/javascript">
<!--
// Write your startup script here
// document.write("page loaded");
//-->
</script>
</asp:Content>
