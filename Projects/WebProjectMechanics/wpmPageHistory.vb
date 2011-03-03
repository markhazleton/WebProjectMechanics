Public Class wpmPageHistory
    Private _PageSource As String
    Public Property PageSource() As String
        Get
            Return _PageSource
        End Get
        Set(ByVal value As String)
            _PageSource = value
        End Set
    End Property
    Private _PageName As String
    Public Property PageName() As String
        Get
            Return _PageName
        End Get
        Set(ByVal value As String)
            _PageName = value
        End Set
    End Property
    Private _ViewTime As Date
    Public Property ViewTime() As Date
        Get
            Return _ViewTime
        End Get
        Set(ByVal value As Date)
            _ViewTime = value
        End Set
    End Property
    Private _RequestURL As String
    Public Property RequestURL() As String
        Get
            Return _RequestURL
        End Get
        Set(ByVal value As String)
            _RequestURL = value
        End Set
    End Property

End Class
