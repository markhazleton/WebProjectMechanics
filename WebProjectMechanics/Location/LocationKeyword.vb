Public Class LocationKeyword
    Private ReadOnly _Code As String
    Public ReadOnly Property Code() As String
        Get
            Return _Code
        End Get
    End Property
    Public Sub New(ByVal value As String)
        _Code = value
    End Sub
    Public Sub New()

    End Sub

End Class
