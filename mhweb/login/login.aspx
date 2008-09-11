<%@ Page Language="VB" MasterPageFile="~/mhwcm.master" CodeFile="login.aspx.vb" Inherits="mhweb_login_login"  %>
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
<br /><br /><br /><br /><br />
<div style="width:300px;margin-left:auto;margin-right:auto;">
<fieldset>
<legend>Welcome: Sign in now:</legend>
<label for="Username">User Name:</label>
<input value="" name="Username" type="text" class="inputFieldIE" size="40" /><br />
<label for="Password">Password:</label>
<input value="" name="Password" type="password" class="inputFieldIE" size="15" /><br />
<input type="submit" value="  Sign In!" />

</fieldset>
</div>  
<asp:literal runat="server" text="" id="myMessage" />

</asp:Content>

