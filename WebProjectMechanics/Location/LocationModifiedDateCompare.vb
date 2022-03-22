Public Class LocationModifiedDateCompare
    Implements IComparer(Of Location)
    Public Property Direction() As UtilitySortDirection = UtilitySortDirection.ASC
    Public Function Compare(ByVal x As Location, ByVal y As Location) As Integer Implements System.Collections.Generic.IComparer(Of Location).Compare
        If Direction = UtilitySortDirection.ASC Then
            Return x.ModifiedDT.CompareTo(y.ModifiedDT)
        Else
            Return x.ModifiedDT.CompareTo(y.ModifiedDT) * -1
        End If
    End Function
End Class
