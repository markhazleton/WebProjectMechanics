Public Class LocationDefaultOrderCompare
    Implements IComparer(Of wpmLocation)
    Protected _direction As wpmSortDirection = wpmSortDirection.ASC
    Public Property Direction() As wpmSortDirection
        Get
            Return _direction
        End Get
        Set(ByVal value As wpmSortDirection)
            _direction = value
        End Set
    End Property
    Public Function Compare(ByVal x As wpmLocation, ByVal y As wpmLocation) As Integer Implements System.Collections.Generic.IComparer(Of wpmLocation).Compare
        If _direction = wpmSortDirection.ASC Then
            Return x.DefaultOrder.CompareTo(y.DefaultOrder)
        Else
            Return x.DefaultOrder.CompareTo(y.DefaultOrder) * -1
        End If
    End Function
End Class
