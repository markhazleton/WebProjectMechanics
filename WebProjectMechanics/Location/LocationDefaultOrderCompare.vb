Public Class LocationDefaultOrderCompare
    Implements IComparer(Of Location)
    Public Property Direction() As UtilitySortDirection = UtilitySortDirection.ASC
    Public Function Compare(ByVal x As Location, ByVal y As Location) As Integer Implements System.Collections.Generic.IComparer(Of Location).Compare
        If Direction = UtilitySortDirection.ASC Then
            Return x.DefaultOrder.CompareTo(y.DefaultOrder)
        Else
            Return x.DefaultOrder.CompareTo(y.DefaultOrder) * -1
        End If
    End Function
End Class

Public Class LocationNameCompare
    Implements IComparer(Of Location)
    Public Property Direction() As UtilitySortDirection = UtilitySortDirection.ASC
    Public Function Compare(ByVal x As Location, ByVal y As Location) As Integer Implements System.Collections.Generic.IComparer(Of Location).Compare
        If Direction = UtilitySortDirection.ASC Then
            Return x.LocationName.CompareTo(y.LocationName)
        Else
            Return x.LocationName.CompareTo(y.LocationName) * -1
        End If
    End Function
End Class
