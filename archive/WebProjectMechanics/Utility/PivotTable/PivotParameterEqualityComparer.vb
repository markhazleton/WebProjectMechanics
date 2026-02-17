
Public Class PivotParameterEqualityComparer
    Inherits EqualityComparer(Of PivotParameter)

    Public Overloads Overrides Function Equals(ByVal x As PivotParameter, _
           ByVal y As PivotParameter) As Boolean
        If x Is Nothing Or y Is Nothing Then
            Return False
        End If
        Return x.CSVFile = y.CSVFile
    End Function

    Public Overloads Overrides Function _
           GetHashCode(ByVal obj As PivotParameter) As Integer
        Return obj.ToString.GetHashCode
    End Function

End Class
