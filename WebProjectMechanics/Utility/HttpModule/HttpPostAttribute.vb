Imports System.Linq

Public Class HttpPostAttribute
    Inherits HttpVerbAttribute
    Public Overrides ReadOnly Property HttpVerb() As String
        Get
            Return "POST"
        End Get
    End Property
End Class
