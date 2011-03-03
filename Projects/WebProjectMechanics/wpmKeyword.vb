Public Class wpmKeyword

    Private _Code As String
    Public ReadOnly Property Code() As String
        Get
            Return _Code
        End Get
    End Property
    Public Sub New(ByVal value As String)
        _Code = value
    End Sub
End Class
