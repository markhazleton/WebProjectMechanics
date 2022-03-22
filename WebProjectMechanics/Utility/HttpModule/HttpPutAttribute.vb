Imports System.Linq

Public Class HttpDeleteAttribute
    Inherits HttpVerbAttribute
    Public Overrides ReadOnly Property HttpVerb() As String
        Get
            Return "DELETE"
        End Get
    End Property
End Class
