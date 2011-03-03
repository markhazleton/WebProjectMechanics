
Public Class wpmSiteBL
    Private mySite As New wpmSite

    Public Sub New()

    End Sub
    Public Sub New(ByRef TheSite As wpmSite)
        mySite = TheSite
    End Sub

    Public Function IsValid(ByRef ErrorMessage As String) As Boolean
        Dim bReturn As Boolean
        If mySite.DomainName Is Nothing Or mySite.DomainName = String.Empty Then
            ErrorMessage = "Site Name Is Required"
            bReturn = False
        Else
            bReturn = True
        End If
        Return bReturn
    End Function

End Class