<%@ Page Language="vb" %>
<%@ Import Namespace="System.net.mail" %>

<script language="VB" runat="Server">
    '*********************
    '  Page Load Handler
    '*********************
    Private Sub Page_Load(ByVal s As System.Object, ByVal e As System.EventArgs)
        Dim mySiteMap As New mhSiteMap(Session)
        Dim landing_page As String = String.Empty
        Dim host_url As String
        Dim filename As String
        Dim req_method As String
        Dim value As String = String.Empty
        Dim key As String
        Dim bErr As Boolean = False
        Dim errStr As String = String.Empty
        Dim bEmpty As Boolean = False
        Dim dtNow As Date = Now()
        Dim sOutFile As String = String.Empty
        Dim iFieldCount As Integer = 0
        Dim subject As New String(String.Empty)
        Dim sMyHeaderRow As New String("<tr>")
        Dim sMyDataRow As New String("<tr>")
        On Error Resume Next
        req_method = Request.ServerVariables("REQUEST_METHOD")
        If (req_method = "POST") Then
            Dim loop1 As Integer
            Dim loop2 As Integer
            Dim arr1() As String
            Dim arr2() As String
            Dim coll As NameValueCollection
            ' Load Header collection into NameValueCollection object.
            coll = Request.Form
            ' Put the names of all keys into a string array.
            arr1 = coll.AllKeys
            value = ""
            sOutFile = sOutFile & FormatVariableLine("Date", dtNow.ToString("U"))
            For loop1 = 0 To arr1.GetUpperBound(0)
                bEmpty = False
                key = arr1(loop1)
                arr2 = coll.GetValues(loop1)
                ' Get all values under this key.
                value = ""
                For loop2 = 0 To arr2.GetUpperBound(0)
                    If loop2 > 0 Then
                        value = value & "," & Server.HtmlEncode(arr2(loop2))
                    Else
                        value = Server.HtmlEncode(arr2(loop2))
                    End If
                Next loop2
                Select Case LCase(key)
                    Case "redirect"
                        landing_page = value
                    Case "subject"
                        subject = value
                        sOutFile = sOutFile & FormatVariableLine(key, value)
                        iFieldCount = iFieldCount + 1
                        sMyHeaderRow = sMyHeaderRow & FormatTableCell(key)
                        sMyDataRow = sMyDataRow & FormatTableCell(value)
                    Case Else
                        sOutFile = sOutFile & FormatVariableLine(key, value)
                        iFieldCount = iFieldCount + 1
                        sMyHeaderRow = sMyHeaderRow & FormatTableCell(key)
                        sMyDataRow = sMyDataRow & FormatTableCell(value)
                End Select
                
            Next loop1
            sMyHeaderRow = sMyHeaderRow & "</tr>"
            sMyDataRow = sMyDataRow & "</tr>"
            sOutFile = sOutFile & "<br/><br/><table border=""1"">" & sMyHeaderRow & sMyDataRow & "</table>"
        Else
            bErr = True
        End If

        If (iFieldCount = 0) Then
            bErr = True
            errStr = errStr & "<br>No variables sent to form! Unable to process request."
        Else
            bErr = False
        End If

        If Not (bErr) Then
            host_url = Request.ServerVariables("HTTP_HOST")
            filename = mhConfig.mhWebConfigFolder & "form\mhForm_" & Replace(Replace(Replace(dtNow.ToString("U"), " ", "-"), ",", ""), ":", "-") & ".html"
            mhfio.CreateFile(filename, sOutFile)
            
            'create the mail message
            Dim mail As New MailMessage()

            'set the addresses
            mail.From = New MailAddress(mySiteMap.mySiteFile.FromEmail)
            mail.To.Add(mySiteMap.mySiteFile.FromEmail)

            'set the content
            mail.Subject = "Website Form from " & host_url & " :" & subject
            mail.Body = sOutFile
            mail.IsBodyHtml = True

            'send the message
            Dim smtp As New SmtpClient("relay-hosting.secureserver.net")
            smtp.Send(mail)
           
            Response.Write(sOutFile)

            If (landing_page <> "") Then
                Response.Redirect("http://" & host_url & "/" & landing_page)
            Else
                Response.Redirect("http://" & host_url)
            End If
        Else
        Response.Write(errStr)
        End If
    End Sub

    Function FormatTableCell(ByVal cell_value As String) As String
        Return "<td>" & cell_value & "</td>"
    End Function
    Function FormatVariableLine(ByVal var_name As String, ByVal var_value As String) As String
        Dim tmpStr As New String("")
        If var_name <> "Submit" Then
            tmpStr = tmpStr & "<b>" & var_name.ToUpper() & "</b>:<br/>" & vbCrLf
            tmpStr = tmpStr & var_value & vbCrLf
            tmpStr = tmpStr & "<br/>" & vbCrLf
        End If
        Return tmpStr
    End Function
</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title>MHForm</title></head>
<body>
</body></html>
