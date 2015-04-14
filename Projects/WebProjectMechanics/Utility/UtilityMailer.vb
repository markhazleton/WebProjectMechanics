Imports System.Net.Mail
Public Class UtilityMailer
    Implements IUtilityMailer
    Public Property FromEmail() As String Implements IUtilityMailer.FromEmail
    Public Property ToEmail() As String Implements IUtilityMailer.ToEmail
    Public Property Subject() As String Implements IUtilityMailer.Subject
    Public Property SendUsing() As String Implements IUtilityMailer.SendUsing
    Public Property smtpserver() As String Implements IUtilityMailer.smtpserver
    Public Property smtpserverport() As String Implements IUtilityMailer.smtpserverport
    Private Property smtpauthenticate() As String
    Private Property sendusername() As String
    Private Property sendpassword() As String
    Public Property HTMLBody() As String Implements IUtilityMailer.HTMLBody

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
End Class

