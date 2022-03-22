Imports System.Linq

''' <summary>
''' Use this attribute to decorate the handler methods that need explicit authentication configuration.
''' If this attibute is set to true it will validate if the user is authenticated.
''' </summary>
Public Class RequireAuthenticationAttribute
    Inherits Attribute
    Public ReadOnly RequireAuthentication As Boolean = False

    Public Sub New(value As Boolean)
        RequireAuthentication = value
    End Sub

End Class
