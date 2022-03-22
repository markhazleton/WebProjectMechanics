Imports System.Linq

Public Class HttpGetAttribute
    Inherits HttpVerbAttribute
    Public Overrides ReadOnly Property HttpVerb() As String
        Get
            Return "GET"
        End Get
    End Property
End Class
