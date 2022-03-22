Public Interface IUtilityMailer
    Property FromEmail() As String
    Property ToEmail() As String
    Property Subject() As String
    Property SendUsing() As String
    Property smtpserver() As String
    Property smtpserverport() As String
    Property HTMLBody() As String
End Interface
