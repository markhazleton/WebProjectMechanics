
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
    Public Property SiteCategoryTypeID As String
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
    Public Sub New(ByVal SiteTypeParameterID As Integer)
        Using mydt As DataTable = ApplicationDAL.GetSiteTypeParameter(SiteTypeParameterID)
            If mydt.Rows.Count = 1 Then
                For Each myrow As DataRow In mydt.Rows
                    CompanySiteParameterID = wpm_GetDBInteger(myrow("CompanySiteParameterID"))
                    CompanyID = wpm_GetDBString(myrow("CompanyID"))
                    LocationID = wpm_GetDBString(myrow("PageID"))
                    ParameterValue = wpm_GetDBString(myrow("ParameterValue"))
                    LocationGroupID = wpm_GetDBString(myrow("SiteCategoryGroupID"))
                    SiteCategoryTypeID = wpm_GetDBString(myrow("SiteCategoryTypeID"))
                    ParameterTemplate = wpm_GetDBString(myrow("SiteParameterTemplate"))
                    ParameterTypeID = wpm_GetDBInteger(myrow("SiteParameterTypeID"),0)
                    ParameterTypeDS = wpm_GetDBString(myrow("SiteParameterTypeDS"))
                    ParameterTypeNM = wpm_GetDBString(myrow("SiteParameterTypeNM"))
                    ParameterTypeOrder = wpm_GetDBInteger(myrow("SiteParameterTypeOrder"))
                    SortOrder = wpm_GetDBInteger(myrow("SortOrder"))
                Next
            Else
                PopulateDefault()
            End If
        End Using
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
