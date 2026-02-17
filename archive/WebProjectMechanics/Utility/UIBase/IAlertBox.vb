
Public Interface IAlertBox
    Sub SetAlert()
    Property boldnote As String
    Property message As String
    Property alertType As AlertBoxType
    Property dismissable As Boolean
End Interface
