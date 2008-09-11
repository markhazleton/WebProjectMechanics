<%@ Page Language="VB" %>
<script language="vbscript" runat="Server">
    Dim mailBody As String
    Dim Mailer As Object
    Dim mail As Object

    Dim rsPassword__MMColParam As String

    Dim rsPassword As Object
    Dim strSQL As String
    Dim rsPassword_numRows As Byte

    Dim FF_RecConn As Object

    Dim RSBODY As Object
    Dim RSBODY_numRows As Byte

    Dim mailSubject As String
    Dim var_done As String
    Dim mailFrom As String
    Dim mailTo As String

    Dim mySiteMap As New mhSiteMap(httpcontext.Current.Session)
</script>
<%
    rsPassword__MMColParam = "1"
    If (Not IsNothing(Request.QueryString.GetValues("U_EMAIL"))) Then
        rsPassword__MMColParam = Request.QueryString.Item("U_EMAIL")
    End If
  
    FF_RecConn.Open(mhSession.GetSiteDB & ";")
    strSQL = "SELECT Contact.ContactID, " & "Contact.LogonName, " & "Contact.PrimaryContact, " & "Contact.GroupID, " & "Contact.LogonPassword, Contact.active, " & "Contact.Email, " & "Company.CompanyID, " & "Company.CompanyName, " & "Contact.TemplatePrefix " & "FROM Contact,Company " & "WHERE Contact.CompanyID=Company.CompanyID " & "and Contact.CompanyID=" & mhSession.GetCompanyID() & " " & "and Contact.Email = '" & Replace(rsPassword__MMColParam, "'", "''") & "' "
    With rsPassword
        .let_ActiveConnection(FF_RecConn)
        .let_Source(strSQL)
        .CursorType = 0
        .CursorLocation = 2
        .LockType = 1
        .Open()
    End With
    rsPassword_numRows = 0
    With RSBODY
        .let_ActiveConnection(FF_RecConn)
        .let_Source("SELECT * FROM Company where CompanyID=" & mhSession.GetCompanyID() & " ")
        .CursorType = 0
        .CursorLocation = 2
        .LockType = 1
        .Open()
    End With
    RSBODY_numRows = 0
    var_done = "0"

    If rsPassword.EOF Then
        Response.Redirect("accountnotfound.aspx")
    Else
        mailTo = IIf(IsDBNull(rsPassword.Fields.Item("Email").Value), Nothing, rsPassword.Fields.Item("Email").Value)
        mailFrom = IIf(IsDBNull(RSBODY.Fields.Item("FromEmail").Value), Nothing, RSBODY.Fields.Item("FromEmail").Value)
        mailSubject = "Your User Name and Password for " & mySiteMap.mySiteFile.SiteURL
        mailBody = "Your user name and password for " & mySiteMap.mySiteFile.SiteURL & " as requested:" & vbCrLf & "User Name : " & (IIf(IsDBNull(rsPassword.Fields.Item("LogonName").Value), Nothing, rsPassword.Fields.Item("LogonName").Value)) & vbCrLf & "Password  : " & (IIf(IsDBNull(rsPassword.Fields.Item("LogonPassword").Value), Nothing, rsPassword.Fields.Item("LogonPassword").Value)) & vbCrLf & vbCrLf & "You may now use these to log on to " & mySiteMap.mySiteFile.SiteURL & "."


        'create the mail message
        Dim mail As New System.Net.Mail.MailMessage()
        'set the addresses
        mail.From = New System.Net.Mail.MailAddress(mailFrom)
        mail.To.Add(mailTo)
        'set the content
        mail.Subject = mailSubject
        mail.Body = mailBody
        mail.IsBodyHtml = False
        'send the message
        Dim smtp As New System.Net.Mail.SmtpClient("relay-hosting.secureserver.net")
        smtp.Send(mail)
    End If
%>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title>Forgot Password</title>
</head>
<body>
<table width="80%" border="0" align="center" cellpadding="1" cellspacing="1" bgcolor="#003366">
  <tr>
    <td height="0" valign="top" bgcolor="#FFFFCC">&nbsp;</td>
  </tr>
  <tr>
    <td height="42" valign="top" bgcolor="#FFFFFF"><div align="center">
        <p><strong>You username and password have been sent.</strong></p>
      </div></td>
  </tr>
</table>
<p align="center">
<input name="button" type="button" class="Buttons" onclick="javascript:self.close();" value="Close Window" />
</p>
</body>
</html>

