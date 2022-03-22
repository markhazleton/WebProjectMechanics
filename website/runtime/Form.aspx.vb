Imports System.Net.Mail
Imports WebProjectMechanics
Imports System

Partial Class ProjectMechanicsForm
    Inherits ApplicationPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim landing_page As String = String.Empty
        Dim sFileName As String = String.Empty
        Dim req_method As String = Request.ServerVariables("REQUEST_METHOD")
        Dim value As String = String.Empty
        Dim key As String = String.Empty
        Dim bErr As Boolean = False
        Dim errStr As String = String.Empty
        Dim bEmpty As Boolean = False
        Dim sOutFile As String = String.Empty
        Dim iFieldCount As Integer = 0
        Dim subject As String = String.Empty
        Dim sMyHeaderRow As String = "<tr>"
        Dim sMyDataRow As String = "<tr>"

        If (req_method = "POST") Then
            Dim loop1 As Integer
            Dim loop2 As Integer
            Dim arr1() As String
            Dim arr2() As String
            Dim coll As NameValueCollection = Request.Form
            ' Load Header collection into NameValueCollection object.
            ' Put the names of all keys into a string array.
            arr1 = coll.AllKeys
            value = ""

            sOutFile = String.Format("{0}{1}", sOutFile, FormatVariableLine("Date", DateTime.Now().ToString("U")))
            For loop1 = 0 To arr1.GetUpperBound(0)
                bEmpty = False
                key = arr1(loop1)
                arr2 = coll.GetValues(loop1)
                ' Get all values under this key.
                value = ""
                For loop2 = 0 To arr2.GetUpperBound(0)
                    If loop2 > 0 Then
                        value = String.Format("{0},{1}", value, Server.HtmlEncode(arr2(loop2)))
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
            sOutFile = String.Format("{0}<br/><br/><table border=""1"">{1}{2}</table>{3}", sOutFile, sMyHeaderRow, sMyDataRow, GetPageHistory())

            If (iFieldCount = 0) Then
                bErr = True
                errStr = errStr & "<br>No variables sent to form! Unable to process request."
            Else
                bErr = False
            End If

            If Not (bErr) Then

                'create the mail message
                Using mail As New MailMessage()
                    'set the addresses
                    If masterPage.myCompany.FromEmail <> String.Empty Then
                        mail.From = New MailAddress(masterPage.myCompany.FromEmail)
                        mail.To.Add(masterPage.myCompany.FromEmail)
                        mail.Bcc.Add("mark.hazleton@projectmechanics.com")
                    Else
                        mail.From = New MailAddress("website@projectmechanics.com")
                        mail.To.Add("mark.hazleton@projectmechanics.com")
                    End If
                    'set the content
                    mail.Subject = String.Format("Website Form from {0} :{1}", Request.ServerVariables("HTTP_HOST"), subject)
                    mail.Body = sOutFile
                    mail.IsBodyHtml = True
                    ' Save Copy of Email
                    SaveMailToFile(masterPage.myCompany, sOutFile)
                    'send the message
                    Try
                        Dim smtp As New SmtpClient("smtpout.secureserver.net", 25) With {.Credentials = New Net.NetworkCredential("website@projectmechanics.com", "justdoit"), .UseDefaultCredentials = False, .EnableSsl = False}
                        smtp.Send(mail)

                    Catch ex As Exception
                        ApplicationLogging.ErrorLog(String.Format("Form-Error Sending Email -({0}) {1}", sFileName, ex), "Form.aspx - PageLoad")
                    End Try
                End Using
            Else
                ApplicationLogging.AuditLog("Form-Error - " & errStr, "Form.aspx - PageLoad")
            End If

            If (landing_page <> "" And landing_page <> "/" And landing_page <> "\") Then
                Response.Redirect(String.Format("http://{0}/{1}", Request.ServerVariables("HTTP_HOST"), landing_page))
            Else
                Response.Redirect("http://" & Request.ServerVariables("HTTP_HOST"))
            End If
        Else
            Response.Write(masterPage.myCompany.GetFormPageHTML())
        End If
    End Sub

End Class
