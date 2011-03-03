Public Class SiteDefaultOrderCompare
    Implements IComparer(Of wpmSite)

    Protected _direction As SortDirection = SortDirection.ASC
    Public Property Direction() As SortDirection
        Get
            Return _direction
        End Get
        Set(ByVal value As SortDirection)
            _direction = value
        End Set
    End Property

    Public Function Compare(ByVal x As wpmSite, ByVal y As wpmSite) As Integer Implements System.Collections.Generic.IComparer(Of wpmSite).Compare
        If _direction = SortDirection.ASC Then
            Return x.DomainName.CompareTo(y.DomainName)
        Else
            Return x.DomainName.CompareTo(y.DomainName) * -1
        End If

    End Function
End Class
