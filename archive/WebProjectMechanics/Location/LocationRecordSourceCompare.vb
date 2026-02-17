Public Class LocationRecordSourceCompare
    Implements IComparer(Of Location)
    Public Property Direction() As UtilitySortDirection = UtilitySortDirection.ASC
    Public Function Compare(ByVal x As Location, ByVal y As Location) As Integer Implements System.Collections.Generic.IComparer(Of Location).Compare
        If Direction = UtilitySortDirection.ASC Then
            Return x.RecordSource.CompareTo(y.RecordSource)
        Else
            Return x.RecordSource.CompareTo(y.RecordSource) * -1
        End If
    End Function
End Class
