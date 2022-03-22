Imports System.Linq

Public Class HttpPutAttribute
    Inherits HttpVerbAttribute
    Public Overrides ReadOnly Property HttpVerb() As String
        Get
            Return "PUT"
        End Get
    End Property
End Class

