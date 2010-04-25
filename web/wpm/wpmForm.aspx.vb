Imports System.Net.Mail

Partial Class wpm_wpmForm
    Inherits wpmPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim landing_page As String = String.Empty
        ' If IsPostBack Then
        Dim filename As String
        Dim req_method As String
        Dim value As String = String.Empty
        Dim key As String
        Dim bErr As Boolean = False
        Dim errStr As String = String.Empty
        Dim bEmpty As Boolean = False
        Dim dtNow As Date = System.DateTime.Now()
        Dim sOutFile As String = String.Empty
        Dim iFieldCount As Integer = 0
        Dim subject As New String(String.Empty)
        Dim sMyHeaderRow As New String("<tr>")
        Dim sMyDataRow As New String("<tr>")
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
                        value = Request.Form(key)
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
            sOutFile = sOutFile & "<br/><br/><table border=""1"">" & sMyHeaderRow & sMyDataRow & "</table>" & GetPageHistory()
            
            If (iFieldCount = 0) Then
                bErr = True
                errStr = errStr & "<br>No variables sent to form! Unable to process request."
            Else
                bErr = False
            End If

            If Not (bErr) Then

                'create the mail message
                Dim mail As New MailMessage()

                'set the addresses
                If pageActiveSite.FromEmail <> String.Empty Then
                    mail.From = New MailAddress(pageActiveSite.FromEmail)
                    mail.To.Add(pageActiveSite.FromEmail)
                    mail.Bcc.Add("mark.hazleton@projectmechanics.com")
                Else
                    mail.From = New MailAddress("mark.hazleton@projectmechanics.com")
                    mail.To.Add("mark.hazleton@projectmechanics.com")
                End If

                'set the content
                mail.Subject = "Website Form from " & Request.ServerVariables("HTTP_HOST") & " :" & subject
                mail.Body = sOutFile
                mail.IsBodyHtml = True

                ' Save Copy of Email 
                filename = App.Config.ConfigFolderPath & "form\" & Replace(Replace(Replace(pageActiveSite.CompanyName & "-" & Format(dtNow, "yyyy:MM:dd:HH:mm:ss"), " ", "-"), ",", ""), ":", "-") & ".html"
                wpmFileIO.CreateFile(filename, sOutFile & "<br/><br/><hr/>Sent to:" & pageActiveSite.FromEmail & "<br/>")
                Response.Write(sOutFile)

                'send the message
                Try
                    Dim smtp As New SmtpClient("relay-hosting.secureserver.net")
                    smtp.Send(mail)
                Catch ex As Exception
                    wpmLog.AuditLog("wpmForm-Error Sending Email -(" & filename & ") " & ex.ToString, "wpmForm.aspx - PageLoad")
                End Try
            Else
                wpmLog.AuditLog("wpmForm-Error - " & errStr, "wpmForm.aspx - PageLoad")
            End If

            If (landing_page <> "") Then
                Response.Redirect("http://" & Request.ServerVariables("HTTP_HOST") & "/" & landing_page)
            Else
                Response.Redirect("http://" & Request.ServerVariables("HTTP_HOST"))
            End If
        Else
            Response.Write(pageActiveSite.GetFormPageHTML())
        End If
    End Sub

    Private Function FormatTableCell(ByVal cell_value As String) As String
        Return "<td>" & cell_value & "</td>"
    End Function
    Private Function FormatVariableLine(ByVal var_name As String, ByVal var_value As String) As String
        Dim tmpStr As New String("")
        If var_name <> "Submit" Then
            tmpStr = tmpStr & "<b>" & var_name.ToUpper() & "</b>:<br/>" & vbCrLf
            tmpStr = tmpStr & var_value & vbCrLf
            tmpStr = tmpStr & "<br/>" & vbCrLf
        End If
        Return tmpStr
    End Function
    Private Function GetSessionPageHistory() As wpmPageHistoryList
        Dim myPageHistory As wpmPageHistoryList
        Try
            myPageHistory = CType(HttpContext.Current.Session("PageHistory"), wpmPageHistoryList)
            If myPageHistory Is Nothing Then
                myPageHistory = New wpmPageHistoryList
            End If
        Catch ex As Exception
            wpmLog.AuditLog("Error when reading Session variable (PageHisotry) - " & ex.ToString, "wpmForm.New")
            myPageHistory = New wpmPageHistoryList
        End Try
        Return myPageHistory
    End Function

    Private Function GetPageHistory() As String
        Dim mysb As New StringBuilder("<hr/><h2>Session History</h2><br/><table border=""1"">")
        mysb.Append("<thead><tr><td>Timestamp</td><td>Page Name</td><td>Previous Page</td></tr></thead>")
        For Each ph As wpmPageHistory In GetSessionPageHistory()
            mysb.Append("<tr><td>" & ph.ViewTime.ToString() & "</td><td><a href=""" & ph.RequestURL & """>" & ph.PageName & "</a></td><td>" & ph.PageSource & "</td></tr>")
        Next
        mysb.Append("</table></br><hr/>")
        Return mysb.ToString
    End Function

End Class
