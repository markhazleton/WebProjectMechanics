<%@ Page Language="VB" MasterPageFile="~/mhwcm.master" AutoEventWireup="false" title="Download Sample Chapter" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">

<SCRIPT TYPE="text/javascript" src=formval.js></SCRIPT> 
<!-- ====================================== -->

<NOSCRIPT>
<P> Javascript is not currently enabled on your browser. If you can enable it, your input will be checked as you enter it (on most browsers, at least). You may find this helpful. </P>
</NOSCRIPT>

<SCRIPT TYPE="text/javascript">
// Only script specific to this form goes here.
// General-purpose routines are in a separate file.
  function validateOnSubmit() {
    var elem;
    var errs=0;
    // execute all element validations in reverse order, so focus gets
    // set to the first one in error.
    if (!validateEmail  (document.forms.demo.email, 'inf_email', true)) errs += 1; 
    if (!validatePresent(document.forms.demo.from,  'inf_from'))        errs += 1; 

    if (errs>1)  alert('There are fields which need correction before sending');
    if (errs==1) alert('There is a field which needs correction before sending');

    return (errs==0);
  };
</SCRIPT>
<FORM  onsubmit="return validateOnSubmit()" >
<TABLE CLASS=formtab SUMMARY="Demonstration form">
  <TR>
    <TD STYLE="width: 10em">
        <LABEL FOR=from>Your name:</LABEL></TD>
    <TD><INPUT TYPE=text NAME="from" ID="from" SIZE="35" MAXLENGTH="50" 
         ONCHANGE="validatePresent(this, 'inf_from');"></TD>
    <TD id="inf_from">Required</TD>
  </TR>

  <TR>
    <TD><LABEL FOR=email>Your e-mail address:</LABEL></TD>
    <TD><INPUT TYPE=text NAME="email" ID="email" SIZE="35" MAXLENGTH="50" 
         ONCHANGE="validateEmail(this, 'inf_email', true);"></TD>
    <TD id="inf_email">Required</TD>
  </TR>

  <TR>
    <TD>&nbsp;</TD>
    <TD><INPUT TYPE="Submit" NAME="Submit" VALUE="Send"></TD>
    <TD>&nbsp;</TD>
  </TR>

</TABLE>
</FORM>

<!-- ====================================== -->
</asp:Content>
