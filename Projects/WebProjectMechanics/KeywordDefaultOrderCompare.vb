Public Class KeywordDefaultOrderCompare
    Implements IComparer(Of wpmKeyword)
    Protected _direction As wpmSortDirection = wpmSortDirection.DESC
    Public Property Direction() As wpmSortDirection
        Get
            Return _direction
        End Get
        Set(ByVal value As wpmSortDirection)
            _direction = value
        End Set
    End Property

    Public Function Compare(ByVal x As wpmKeyword, ByVal y As wpmKeyword) As Integer Implements System.Collections.Generic.IComparer(Of wpmKeyword).Compare
        If _direction = wpmSortDirection.ASC Then
            Return x.Code.CompareTo(y.Code)
        Else
            Return x.Code.CompareTo(y.Code) * -1
        End If

    End Function
End Class
