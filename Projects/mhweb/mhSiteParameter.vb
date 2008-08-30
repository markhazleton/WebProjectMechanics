Public Class mhSiteParameter
    Private _SiteParameterTypeID As String
    Public Property SiteParameterTypeID() As String
        Get
            Return _SiteParameterTypeID
        End Get
        Set(ByVal value As String)
            _SiteParameterTypeID = value
        End Set
    End Property
    Private _SiteParameterTypeNM As String
    Public Property SiteParameterTypeNM() As String
        Get
            Return _SiteParameterTypeNM
        End Get
        Set(ByVal value As String)
            _SiteParameterTypeNM = value
        End Set
    End Property
    Private _SiteParameterTypeDS As String
    Public Property SiteParameterTypeDS() As String
        Get
            Return _SiteParameterTypeDS
        End Get
        Set(ByVal value As String)
            _SiteParameterTypeDS = value
        End Set
    End Property
    Private _SiteParameterTypeOrder As Integer
    Public Property SiteParameterTypeOrder() As Integer
        Get
            Return _SiteParameterTypeOrder
        End Get
        Set(ByVal value As Integer)
            _SiteParameterTypeOrder = value
        End Set
    End Property
    Private _SiteParameterTemplate As String
    Public Property SiteParameterTemplate() As String
        Get
            Return _SiteParameterTemplate
        End Get
        Set(ByVal value As String)
            _SiteParameterTemplate = value
        End Set
    End Property
    Private _CompanySiteParameterID As String
    Public Property CompanySiteParameterID() As String
        Get
            Return _CompanySiteParameterID
        End Get
        Set(ByVal value As String)
            _CompanySiteParameterID = value
        End Set
    End Property
    Private _CompanyID As String
    Public Property CompanyID() As String
        Get
            Return _CompanyID
        End Get
        Set(ByVal value As String)
            _CompanyID = value
        End Set
    End Property
    Private _SortOrder As Integer
    Public Property SortOrder() As Integer
        Get
            Return _SortOrder
        End Get
        Set(ByVal value As Integer)
            _SortOrder = value
        End Set
    End Property
    Private _ParameterValue As String
    Public Property ParameterValue() As String
        Get
            Return _ParameterValue
        End Get
        Set(ByVal value As String)
            _ParameterValue = value
        End Set
    End Property
End Class

Public Class mhSiteParameterList
    Inherits List(Of mhSiteParameter)

    Public Function PopulateParameterTypeList(ByVal CompanyID As String) As Boolean
        Try
            For Each myrow As DataRow In mhDataCon.GetSiteParameterList(CompanyID).Rows
                Dim mySiteParameter As New mhSiteParameter
                mySiteParameter.CompanySiteParameterID = mhUTIL.GetDBString(myrow("CompanySiteParameterID"))
                mySiteParameter.CompanyID = mhUTIL.GetDBString(myrow("CompanyID"))
                mySiteParameter.SiteParameterTypeID = mhUTIL.GetDBString(myrow("SiteParameterTypeID"))
                mySiteParameter.SortOrder = mhUTIL.GetDBInteger(myrow("SortOrder"))
                mySiteParameter.ParameterValue = mhUTIL.GetDBString(myrow("ParameterValue"))
                mySiteParameter.SiteParameterTypeNM = mhUTIL.GetDBString(myrow("SiteParameterTypeNM"))
                mySiteParameter.SiteParameterTypeDS = mhUTIL.GetDBString(myrow("SiteParameterTypeDS"))
                mySiteParameter.SiteParameterTypeOrder = mhUTIL.GetDBInteger(myrow("SiteParameterTypeOrder"))
                mySiteParameter.SiteParameterTemplate = mhUTIL.GetDBString(myrow("SiteParameterTemplate"))
                Me.Add(mySiteParameter)
            Next
        Catch ex As Exception
            mhUTIL.AuditLog("Error on mhSiteParameterList.PopulateParameterTypeList", ex.ToString)
        End Try

    End Function


End Class

