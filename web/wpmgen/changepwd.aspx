<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="changepwd.aspx.vb" Inherits="_changepwd" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
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
var changepwd = new ew_Page("changepwd");
// extend page with ValidateForm function
changepwd.ValidateForm = function(fobj)
{
	if (!this.ValidateRequired)
		return true; // ignore validation
	if  (!ew_HasValue(fobj.opwd))
		return ew_OnError(this, fobj.opwd, "Please enter old password");
	if  (!ew_HasValue(fobj.npwd))
		return ew_OnError(this, fobj.npwd, "Please enter new password");
	if  (fobj.npwd.value != fobj.cpwd.value)
		return ew_OnError(this, fobj.cpwd, "Mismatch Password");
	return true;
}
// requires js validation
<% If EW_CLIENT_VALIDATE Then %>
changepwd.ValidateRequired = true;
<% Else %>
changepwd.ValidateRequired = false;
<% End If %>
//-->
</script>
<p><span class="aspnetmaker">Change Password Page</span></p>
<% changepwd.ShowMessage() %>
<form method="post" onsubmit="return changepwd.ValidateForm(this);">
<table border="0" cellspacing="0" cellpadding="4">
	<tr>
		<td><span class="aspnetmaker">Old Password</span></td>
		<td><span class="aspnetmaker"><input type="password" name="opwd" id="opwd" size="20" /></span></td>
	</tr>
	<tr>
		<td><span class="aspnetmaker">New Password</span></td>
		<td><span class="aspnetmaker"><input type="password" name="npwd" id="npwd" size="20" /></span></td>
	</tr>
	<tr>
		<td><span class="aspnetmaker">Confirm Password</span></td>
		<td><span class="aspnetmaker"><input type="password" name="cpwd" id="cpwd" size="20" /></span></td>
	</tr>
	<tr>
		<td>&nbsp;</td>
		<td><span class="aspnetmaker"><input type="submit" name="submit" id="submit" value="Change Password" /></span></td>
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
