Public Class LocationKeywordDefaultOrderCompare
    Implements IComparer(Of LocationKeyword)
    Public Property Direction() As UtilitySortDirection = UtilitySortDirection.DESC

    Public Function Compare(ByVal x As LocationKeyword, ByVal y As LocationKeyword) As Integer Implements System.Collections.Generic.IComparer(Of LocationKeyword).Compare
        If Direction = UtilitySortDirection.ASC Then
            Return x.Code.CompareTo(y.Code)
        Else
            Return x.Code.CompareTo(y.Code) * -1
        End If

    End Function
End Class
