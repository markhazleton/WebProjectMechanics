Imports System.Net.Mail
Imports WebProjectMechanics

Partial Class wpm_wpmForm
    Inherits wpmPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim landing_page As String = String.Empty
        Dim myForm As New wpmForm

        ' If IsPostBack Then
        Dim sFileName As String = String.Empty
        Dim req_method As String = Request.ServerVariables("REQUEST_METHOD")
        Dim value As String = String.Empty
        Dim key As String
        Dim bErr As Boolean = False
        Dim errStr As String = String.Empty
        Dim bEmpty As Boolean = False
        Dim sOutFile As String = String.Empty
        Dim iFieldCount As Integer = 0
        Dim subject As New String(String.Empty)
        Dim sMyHeaderRow As New String("<tr>")
        Dim sMyDataRow As New String("<tr>")
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

            sOutFile = String.Format("{0}{1}", sOutFile, wpmForm.FormatVariableLine("Date", System.DateTime.Now().ToString("U")))
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
                        sOutFile = sOutFile & wpmForm.FormatVariableLine(key, value)
                        iFieldCount = iFieldCount + 1
                        sMyHeaderRow = sMyHeaderRow & wpmForm.FormatTableCell(key)
                        sMyDataRow = sMyDataRow & wpmForm.FormatTableCell(value)
                    Case Else
                        sOutFile = sOutFile & wpmForm.FormatVariableLine(key, value)
                        iFieldCount = iFieldCount + 1
                        sMyHeaderRow = sMyHeaderRow & wpmForm.FormatTableCell(key)
                        sMyDataRow = sMyDataRow & wpmForm.FormatTableCell(value)
                End Select

            Next loop1
            sMyHeaderRow = sMyHeaderRow & "</tr>"
            sMyDataRow = sMyDataRow & "</tr>"
            sOutFile = String.Format("{0}<br/><br/><table border=""1"">{1}{2}</table>{3}", sOutFile, sMyHeaderRow, sMyDataRow, wpmForm.GetPageHistory())

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
                    If pageActiveSite.FromEmail <> String.Empty Then
                        mail.From = New MailAddress(pageActiveSite.FromEmail)
                        mail.To.Add(pageActiveSite.FromEmail)
                        mail.Bcc.Add("mark.hazleton@projectmechanics.com")
                    Else
                        mail.From = New MailAddress("mark.hazleton@projectmechanics.com")
                        mail.To.Add("mark.hazleton@projectmechanics.com")
                    End If
                    'set the content
                    mail.Subject = String.Format("Website Form from {0} :{1}", Request.ServerVariables("HTTP_HOST"), subject)
                    mail.Body = sOutFile
                    mail.IsBodyHtml = True
                    ' Save Copy of Email
                    wpmForm.SaveMailToFile(pageActiveSite, sOutFile)
                    'send the message
                    Try
                        Dim smtp As New SmtpClient("relay-hosting.secureserver.net")
                        smtp.Send(mail)

                    Catch ex As Exception
                        wpmLogging.ErrorLog(String.Format("wpmForm-Error Sending Email -({0}) {1}", sFileName, ex), "wpmForm.aspx - PageLoad")
                    End Try
                End Using
            Else
                wpmLogging.AuditLog("wpmForm-Error - " & errStr, "wpmForm.aspx - PageLoad")
            End If

            If (landing_page <> "" And landing_page <> "/" And landing_page <> "\") Then
                Response.Redirect(String.Format("http://{0}/{1}", Request.ServerVariables("HTTP_HOST"), landing_page))
            Else
                Response.Redirect("http://" & Request.ServerVariables("HTTP_HOST"))
            End If
        Else
            Response.Write(pageActiveSite.GetFormPageHTML())
        End If
    End Sub


End Class
