<%@ Page Language="VB" MasterPageFile="~/wpm/wpm.master" CodeFile="login.aspx.vb" Inherits="wpm_login_login"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
<asp:literal runat="server" text="" id="label1" />
  <input type="hidden" name="LOGIN" value="yes" />
  <script language="JavaScript" type="text/JavaScript">
<!--
function MM_openBrWindow(theURL,winName,features) {
  window.open(theURL,winName,features);
}
//-->
</script>
<div style="width:400px;margin-left:auto;margin-right:auto;">
<br /><br /><br />
<fieldset style="margin:5px 5px 5px 5px;padding:5px 5px 5px 5px;">
<legend>Welcome: Sign in now:</legend>
<label for="Username">User Name:</label><br />
<input value="" name="Username" type="text" class="inputFieldIE" size="40" /><br />
<label for="Password">Password:</label><br />
<input value="" name="Password" type="password" class="inputFieldIE" size="15" /><br /><br />
<input type="submit" value="  Sign In!" />
</fieldset>
<br /><br />
</div>
<asp:literal runat="server" text="" id="myMessage" />
</asp:Content>

