Public Class CompanyDefaultOrderCompare
    Implements IComparer(Of DomainConfiguration)

    Public Function Compare(ByVal x As DomainConfiguration, ByVal y As DomainConfiguration) As Integer Implements System.Collections.Generic.IComparer(Of DomainConfiguration).Compare
        If Direction = UtilitySortDirection.ASC Then
            Return x.DomainName.CompareTo(y.DomainName)
        Else
            Return x.DomainName.CompareTo(y.DomainName) * -1
        End If

    End Function

    Public Property Direction() As UtilitySortDirection = UtilitySortDirection.ASC
End Class
