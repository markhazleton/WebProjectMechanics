Public Class LocationPageNameCompare
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
            Return x.PageName.CompareTo(y.PageName)
        Else
            Return x.PageName.CompareTo(y.PageName) * -1
        End If
    End Function
End Class
