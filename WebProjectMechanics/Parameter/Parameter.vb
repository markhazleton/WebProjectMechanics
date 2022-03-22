
Public Class Parameter
    Implements IEquatable(Of Parameter)
    Public Property RecordSource As String
    Public Property SortOrder As Integer
    Public Property ParameterTypeID As Integer
    Public Property ParameterTypeNM As String
    Public Property ParameterTypeDS As String
    Public Property ParameterTypeOrder As Integer
    Public Property ParameterTemplate As String
    Public Property CompanySiteParameterID As integer
    Public Property CompanyID As String
    Public Property CompanyNM As String
    Public Property LocationID As String
    Public Property LocationNM As String
    Public Property LocationGroupID As String
    Public Property LocationGroupNM As String
    Public Property ParameterValue As String
    Public Property ParameterID As String
    Public Property ParameterNM As String

    Private Sub PopulateDefault()
        SortOrder = 999
    End Sub
    Public Sub New()
        PopulateDefault()
    End Sub
    Public Shared Function createSiteParameter() As Boolean
        Dim bReturn As Boolean = True
        Dim strSQL As String = String.Empty
        Try
            ApplicationLogging.ErrorLog("createSiteParameter not implemented", "Parameter.vb")
        Catch ex As Exception
            bReturn = False
            ApplicationLogging.SQLAudit(strSQL, ex.ToString)
        End Try
        Return bReturn
    End Function
    Public Shared Function updateSiteParameter() As Boolean
        Return False
    End Function

    Public Function Equals1(ByVal other As Parameter) As Boolean Implements IEquatable(Of Parameter).Equals
        Return CompanySiteParameterID.Equals(other.CompanySiteParameterID)
    End Function
End Class
