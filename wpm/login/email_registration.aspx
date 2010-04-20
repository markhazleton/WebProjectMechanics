<%@ Page explicit="true" %>
<script language="VB" runat="Server">
    Private Function SendEmail(ByVal mTo As String, ByVal mSubject As String, ByVal mBody As String, ByVal mFormat As String) As Boolean
        Dim setComponent As Object
        Dim mailObj As Object
        Dim setFromMail As New String("mark.hazleton@projectmechanics.com")
        Dim setSMTP As Object
        Dim setFromEmail As Object

        setComponent = "CDONTS"
        setFromEmail = "website@frogsfolly.com"
        setSMTP = "mail.frogsfolly.com"

        Try
            Select Case setComponent
                Case "CDONTS"
                    mailObj = Server.CreateObject("CDONTS.NewMail")
                    If (mFormat = "Text") Then
                        mailObj.BodyFormat = 1
                        mailObj.MailFormat = 1
                    Else
                        mailObj.BodyFormat = 0
                        mailObj.MailFormat = 0
                    End If
                    mailObj.From = setFromEmail
                    mailObj.to = mTo
                    mailObj.Subject = mSubject
                    mailObj.Body = mBody
                    mailObj.Send()
                Case "ASPMail"
                    mailObj = Server.CreateObject("SMTPsvg.Mailer")
                    If (mFormat = "Text") Then
                        mailObj.CharSet = 2
                    Else
                        mailObj.ContentType = "text/html"
                    End If
                    mailObj.FromName = setFromEmail
                    mailObj.FromAddress = setFromEmail
                    mailObj.RemoteHost = setSMTP
                    mailObj.Subject = mSubject
                    mailObj.BodyText = mBody
                    mailObj.AddRecipient(mTo, mTo)
                    mailObj.SendMail()
                Case "ASPEmail"
                    mailObj = Server.CreateObject("Persits.MailSender")
                    mailObj.Host = setSMTP
                    mailObj.From = setFromMail
                    mailObj.AddAddress(mTo)
                    mailObj.Subject = mSubject
                    mailObj.Body = mBody
                    If (mFormat = "Text") Then
                        mailObj.IsHTML = False
                    Else
                        mailObj.IsHTML = True
                    End If
                    mailObj.Send()
            End Select
            SendEmail = True
        Catch exp As Exception
            Response.Write("There was and error Sending the mail: " & exp.Message)
            SendEmail = False
        End Try

    End Function

    Private Function ConfirmSubscribe(ByVal sUserName As String, ByVal sUserEmail As String) As Boolean
        Dim sFormat As String
        Dim sBody As String
        Dim sSubject As String
        sSubject = "Thank you " & sUserName & ", for joining " & Session("siteURL")
        sBody = "This email is to confirm that you have registered at " & Session("siteURL") & ".  Your user id is " & sUserName & ".  "
        sFormat = "Text"
        Return SendEmail(sUserEmail, sSubject, sBody, sFormat)
    End Function

</script>

<script language="JavaScript" type="text/JavaScript">
<!--
function MM_openBrWindow(theURL,winName,features) { //v2.0
  window.open(theURL,winName,features);
}

function MM_findObj(n, d) { //v4.01
  var p,i,x;  if(!d) d=document; if((p=n.indexOf("?"))>0&&parent.frames.length) {
    d=parent.frames[n.substring(p+1)].document; n=n.substring(0,p);}
  if(!(x=d[n])&&d.all) x=d.all[n]; for (i=0;!x&&i<d.forms.length;i++) x=d.forms[i][n];
  for(i=0;!x&&d.layers&&i<d.layers.length;i++) x=MM_findObj(n,d.layers[i].document);
  if(!x && d.getElementById) x=d.getElementById(n); return x;
}

 function MM_validateForm() { //v4.0
  var i,p,q,nm,test,num,min,max,errors='',args=MM_validateForm.arguments;
  for (i=0; i<(args.length-2); i+=3) { test=args[i+2]; val=MM_findObj(args[i]);
    if (val) { nm=val.name; if ((val=val.value)!="") {
      if (test.indexOf('isEmail')!=-1) { p=val.indexOf('@');
        if (p<1 || p==(val.length-1)) errors+='- '+nm+' must contain an e-mail address.\n';
      } else if (test!='R') { num = parseFloat(val);
        if (isNaN(val)) errors+='- '+nm+' must contain a number.\n';
        if (test.indexOf('inRange') != -1) { p=test.indexOf(':');
          min=test.substring(8,p); max=test.substring(p+1);
          if (num<min || max<num) errors+='- '+nm+' must contain a number between '+min+' and '+max+'.\n';
    } } } else if (test.charAt(0) == 'R') errors += '- '+nm+' is required.\n'; }
  } if (errors) alert('The following error(s) occurred:\n'+errors);
  document.MM_returnValue = (errors == '');
}
//-->
</script>
<%
    Dim sBody As New String("")
    If Request.Form.Item("REGISTRATION") = "yes" Then
        sBody = sBody & "Online Registration<br /><br /><hr/>"
        sBody = sBody & "E-mail -------- " & Request.Form.GetValues("eMail1")(0) & "<br />"
        sBody = sBody & "First Name ---- " & Request.Form.GetValues("FirstName")(0) & "<br />"
        sBody = sBody & "Last Name ----- " & Request.Form.GetValues("LastName")(0) & "<br />"
        sBody = sBody & "Area Code ----- " & Request.Form.GetValues("areacode")(0) & "<br />"
        sBody = sBody & "Phone Number ---" & Request.Form.GetValues("phonenumber")(0) & "<br />"
        sBody = sBody & "City ---------- " & Request.Form.GetValues("City")(0) & "<br />"
        sBody = sBody & "State Country - " & Request.Form.GetValues("StateCountry")(0) & "<br />"
        sBody = sBody & "Zip Code ------ " & Request.Form.GetValues("postalcode")(0) & "<br />"
    End If

    Response.Write(sBody)

    If SendEmail("info@denovaco.com", "Web Site Registration for " & Request.Form.GetValues("eMail1")(0), sBody, "HTML") Then
        Response.Redirect("index.html")
    Else
        Response.Write("<br /><hr/>ERROR --> Message Not Sent<br /><hr/>")
    End If
%>

