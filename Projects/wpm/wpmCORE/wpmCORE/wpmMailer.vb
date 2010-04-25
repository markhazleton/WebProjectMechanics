Imports System.Net.Mail
Public Class wpmMailer
    Private _FromEmail As String
    Public Property FromEmail() As String
        Get
            Return _FromEmail
        End Get
        Set(ByVal value As String)
            _FromEmail = value
        End Set
    End Property

    Private _ToEmail As String
    Public Property ToEmail() As String
        Get
            Return _ToEmail
        End Get
        Set(ByVal value As String)
            _ToEmail = value
        End Set
    End Property
    Private _Subject As String
    Public Property Subject() As String
        Get
            Return _Subject
        End Get
        Set(ByVal value As String)
            _Subject = value
        End Set
    End Property
    Private _SendUsing As String
    Public Property SendUsing() As String
        Get
            Return _SendUsing
        End Get
        Set(ByVal value As String)
            _SendUsing = value
        End Set
    End Property
    Private _smtpserver As String
    Public Property smtpserver() As String
        Get
            Return _smtpserver
        End Get
        Set(ByVal value As String)
            _smtpserver = value
        End Set
    End Property

    Private _smtpserverport As String
    Public Property smtpserverport() As String
        Get
            Return _smtpserverport
        End Get
        Set(ByVal value As String)
            _smtpserverport = value
        End Set
    End Property
    Private _smtpauthenticate As String
    Private Property smtpauthenticate() As String
        Get
            Return _smtpauthenticate
        End Get
        Set(ByVal value As String)
            _smtpauthenticate = value
        End Set
    End Property
    Private _sendusername As String
    Private Property sendusername() As String
        Get
            Return _sendusername
        End Get
        Set(ByVal value As String)
            _sendusername = value
        End Set
    End Property
    Private _sendpassword As String
    Private Property sendpassword() As String
        Get
            Return _sendpassword
        End Get
        Set(ByVal value As String)
            _sendpassword = value
        End Set
    End Property
    Private _HTMLBody As String
    Public Property HTMLBody() As String
        Get
            Return _HTMLBody
        End Get
        Set(ByVal value As String)
            _HTMLBody = value
        End Set
    End Property

    Private Sub PopulateDefault()
        _FromEmail = "website@projectmechanics.com"
        _ToEmail = "mark.hazleton@projectmechanics.com"
        _Subject = "Inquiries Form from website"
        _SendUsing = "2"
        _smtpserver = "smtp.gmail.com"
        _smtpserverport = "465"
        _smtpauthenticate = "1"
        _sendusername = "website@projectmechanics.com"
        _sendpassword = "goforit"
    End Sub

    Public Sub New()
        PopulateDefault()
    End Sub

    Public Function SendMail() As Boolean
        'Dim mymessage As New System.Net.Mail.MailMessage(Me.FromEmail, Me.ToEmail, Me.Subject, Me.HTMLBody)
        'mymessage.IsBodyHtml = True
        'Dim client As New System.Net.Mail.SmtpClient(Me.smtpserver, CInt(Me.smtpserverport))
        'Try
        '    With client
        '        .Credentials = New System.Net.NetworkCredential(Me.sendusername, Me.sendpassword)
        '        .Send(mymessage)
        '    End With
        'Catch ex As Exception
        '    wpmLog.AuditLog("mhMailer.SendMail Error", ex.ToString)
        'Finally

        'End Try

        'create the mail message
        Dim mail As New MailMessage()

        'set the addresses
        mail.From = New MailAddress("mark@frogsfolly.com")
        mail.To.Add("mark@frogsfolly.com")

        'set the content
        mail.Subject = "This is an email"
        mail.Body = "this is a sample body with html in it. <b>This is bold</b> <font color=#336699>This is blue</font>"
        mail.IsBodyHtml = True

        'send the message
        Dim smtp As New SmtpClient("relay-hosting.secureserver.net")
        smtp.Send(mail)


        Return True
    End Function

End Class

