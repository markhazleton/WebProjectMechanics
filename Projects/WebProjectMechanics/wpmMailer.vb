Imports System.Net.Mail
Public Class wpmMailer
    Public Property FromEmail() As String
    Public Property ToEmail() As String
    Public Property Subject() As String
    Public Property SendUsing() As String
    Public Property smtpserver() As String
    Public Property smtpserverport() As String
    Private Property smtpauthenticate() As String
    Private Property sendusername() As String
    Private Property sendpassword() As String
    Public Property HTMLBody() As String

    Private Sub PopulateDefault()
        FromEmail = "website@projectmechanics.com"
        ToEmail = "mark.hazleton@projectmechanics.com"
        Subject = "Inquiries Form from website"
        SendUsing = "2"
        smtpserver = "smtp.gmail.com"
        smtpserverport = "465"
        smtpauthenticate = "1"
        sendusername = "website@projectmechanics.com"
        sendpassword = "goforit"
    End Sub

    Public Sub New()
        PopulateDefault()
    End Sub

    'Public Function SendMail() As Boolean
    '    'create the mail message
    '    Using mail As New MailMessage() With {.From = New MailAddress(FromEmail), .Subject = Subject, .IsBodyHtml = True, .Body = HTMLBody}
    '        mail.To.Add(ToEmail)
    '        Using smtp As New SmtpClient(smtpserver, smtpserverport)
    '            smtp.Send(mail)
    '        End Using
    '    End Using
    '    Return True
    'End Function
End Class

