
Public Class UtilityMailer
    Implements IUtilityMailer

    Public Sub New()
        PopulateDefault()
    End Sub

    Public Property FromEmail() As String Implements IUtilityMailer.FromEmail
    Public Property HTMLBody() As String Implements IUtilityMailer.HTMLBody
    Public Property SendUsing() As String Implements IUtilityMailer.SendUsing
    Public Property smtpserver() As String Implements IUtilityMailer.smtpserver
    Public Property smtpserverport() As String Implements IUtilityMailer.smtpserverport
    Public Property Subject() As String Implements IUtilityMailer.Subject
    Public Property ToEmail() As String Implements IUtilityMailer.ToEmail

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

    Private Property sendpassword() As String
    Private Property sendusername() As String
    Private Property smtpauthenticate() As String
End Class

