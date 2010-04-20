Imports System.ComponentModel

Public Class wpmSiteParameter
    Implements IEquatable(Of wpmSiteParameter)
    Private _RecordSource As String
    Public Property RecordSource() As String
        Get
            Return _RecordSource
        End Get
        Set(ByVal Value As String)
            _RecordSource = Value
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
    Private _siteCategoryTypeID As String
    Public Property SiteCategoryTypeID() As String
        Get
            Return _siteCategoryTypeID
        End Get
        Set(ByVal Value As String)
            _siteCategoryTypeID = Value
        End Set
    End Property
    Private _pageID As String
    Public Property PageID() As String
        Get
            Return _pageID
        End Get
        Set(ByVal Value As String)
            _pageID = Value
        End Set
    End Property
    Private _SiteCategoryGroupID As String
    Public Property SiteCategoryGroupID() As String
        Get
            Return _SiteCategoryGroupID
        End Get
        Set(ByVal Value As String)
            _SiteCategoryGroupID = Value
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


    Private Sub PopulateDefault()
        SortOrder = 999
    End Sub
    Public Sub New()
        PopulateDefault()
    End Sub
    Public Sub New(ByVal SiteTypeParameterID As Integer)
        Dim mydt As DataTable = wpmDataCon.GetSiteTypeParameter(SiteTypeParameterID)
        If mydt.Rows.Count = 1 Then
            For Each myrow As DataRow In mydt.Rows
                Me._CompanyID = wpmUTIL.GetDBString(myrow("CompanyID"))
                Me._pageID = wpmUTIL.GetDBString(myrow("PageID"))
                Me._ParameterValue = wpmUTIL.GetDBString(myrow("ParameterValue"))
                Me._SiteCategoryGroupID = wpmUTIL.GetDBString(myrow("SiteCategoryGroupID"))
                Me._siteCategoryTypeID = wpmUTIL.GetDBString(myrow("SiteCategoryTypeID"))
                Me._SiteParameterTemplate = wpmUTIL.GetDBString(myrow("SiteParameterTemplate"))
                Me._SiteParameterTypeID = wpmUTIL.GetDBString(myrow("SiteParameterTypeID"))
                Me._SiteParameterTypeDS = wpmUTIL.GetDBString(myrow("SiteParameterTypeDS"))
                Me._SiteParameterTypeNM = wpmUTIL.GetDBString(myrow("SiteParameterTypeNM"))
                Me._SiteParameterTypeOrder = wpmUTIL.GetDBInteger(myrow("SiteParameterTypeOrder"))
                Me._SortOrder = wpmUTIL.GetDBInteger(myrow("SortOrder"))
            Next
        Else
            PopulateDefault()
        End If
        mydt.Clear()
        mydt = Nothing
    End Sub

    Public Function createSiteParameter() As Boolean
        Dim bReturn As Boolean = True
        Dim strSQL As String = String.Empty
        Try
            'strSQL = "insert into [Image] ([ImageName],[ImageFileName],[CompanyID],[Active]) values(""" & Me.ImageName & """,""" & Me.ImageFileName & """," & Me.CompanyID & ",TRUE);"
            'mhDB.RunInsertSQL(strSQL, "createSiteTypeParameter-" & Me.ImageFileName)
        Catch ex As Exception
            bReturn = False
            wpmUTIL.SQLAudit(strSQL, ex.ToString)
        End Try
        Return bReturn
    End Function
    Public Function updateSiteParameter() As Boolean
        Dim result As Boolean = False
        Return result
    End Function

    Public Function Equals1(ByVal other As wpmSiteParameter) As Boolean Implements System.IEquatable(Of wpmSiteParameter).Equals
        Return Me.CompanySiteParameterID.Equals(other.CompanySiteParameterID)
    End Function
End Class

Public Class wpmSiteParameterList
    Inherits List(Of wpmSiteParameter)
    Public Function PopulateParameterTypeList(ByVal CompanyID As String, ByVal SiteCategoryTypeID As String) As Boolean
      
        Try
            For Each myrow As DataRow In wpmDataCon.GetSiteParameterList(CompanyID, SiteCategoryTypeID).Rows
                Dim mySiteParameter As New wpmSiteParameter
                mySiteParameter.RecordSource = wpmUTIL.GetDBString(myrow("RecordSource"))
                mySiteParameter.CompanySiteParameterID = wpmUTIL.GetDBString(myrow("CompanySiteParameterID"))
                mySiteParameter.SortOrder = wpmUTIL.GetDBInteger(myrow("PrimarySort"))
                mySiteParameter.CompanyID = wpmUTIL.GetDBString(myrow("CompanyID"))
                mySiteParameter.ParameterValue = wpmUTIL.GetDBString(myrow("ParameterValue"))
                mySiteParameter.SiteParameterTypeID = wpmUTIL.GetDBString(myrow("SiteParameterTypeID"))
                mySiteParameter.SiteParameterTypeNM = wpmUTIL.GetDBString(myrow("SiteParameterTypeNM"))
                mySiteParameter.SiteParameterTypeDS = wpmUTIL.GetDBString(myrow("SiteParameterTypeDS"))
                mySiteParameter.SiteCategoryTypeID = wpmUTIL.GetDBString(myrow("SiteCategoryTypeID"))
                mySiteParameter.PageID = wpmUTIL.GetDBString(myrow("PageID"))
                mySiteParameter.SiteCategoryGroupID = wpmUTIL.GetDBString(myrow("SiteCategoryGroupID"))
                Me.Add(mySiteParameter)
            Next
        Catch ex As Exception
            wpmUTIL.AuditLog("Error on PopulateParameterTypeList.GetSiteParameterList", ex.ToString)
        End Try
        Try
            For Each myrow As DataRow In wpmDataCon.GetCompanySiteTypeParameterList(CompanyID, SiteCategoryTypeID).Rows
                Dim mySiteParameter As New wpmSiteParameter
                mySiteParameter.RecordSource = wpmUTIL.GetDBString(myrow("RecordSource"))
                mySiteParameter.CompanySiteParameterID = wpmUTIL.GetDBString(myrow("CompanySiteTypeParameterID"))
                mySiteParameter.SortOrder = wpmUTIL.GetDBInteger(myrow("PrimarySort"))
                mySiteParameter.CompanyID = wpmUTIL.GetDBString(myrow("CompanyID"))
                mySiteParameter.ParameterValue = wpmUTIL.GetDBString(myrow("ParameterValue"))
                mySiteParameter.SiteParameterTypeID = wpmUTIL.GetDBString(myrow("SiteParameterTypeID"))
                mySiteParameter.SiteParameterTypeNM = wpmUTIL.GetDBString(myrow("SiteParameterTypeNM"))
                mySiteParameter.SiteParameterTypeDS = wpmUTIL.GetDBString(myrow("SiteParameterTypeDS"))
                mySiteParameter.SiteCategoryTypeID = wpmUTIL.GetDBString(myrow("SiteCategoryTypeID"))
                mySiteParameter.PageID = wpmUTIL.GetDBString(myrow("PageID"))
                mySiteParameter.SiteCategoryGroupID = wpmUTIL.GetDBString(myrow("SiteCategoryGroupID"))
                Me.Add(mySiteParameter)
            Next
        Catch ex As Exception
            wpmUTIL.AuditLog("Error on PopulateParameterTypeList.GetCompanySiteTypeParameterList", ex.ToString)
        End Try
        Try
            For Each myrow As DataRow In wpmDataCon.GetParameterTypeList(CompanyID, SiteCategoryTypeID).Rows
                Dim mySiteParameter As New wpmSiteParameter
                mySiteParameter.RecordSource = wpmUTIL.GetDBString(myrow("RecordSource"))
                mySiteParameter.CompanySiteParameterID = wpmUTIL.GetDBString(myrow("CompanySiteParameterID"))
                mySiteParameter.SortOrder = wpmUTIL.GetDBInteger(myrow("PrimarySort"))
                mySiteParameter.CompanyID = String.Empty
                mySiteParameter.ParameterValue = wpmUTIL.GetDBString(myrow("SiteParameterTemplate"))
                mySiteParameter.SiteParameterTypeID = wpmUTIL.GetDBString(myrow("SiteParameterTypeID"))
                mySiteParameter.SiteParameterTypeNM = wpmUTIL.GetDBString(myrow("SiteParameterTypeNM"))
                mySiteParameter.SiteParameterTypeDS = wpmUTIL.GetDBString(myrow("SiteParameterTypeDS"))
                mySiteParameter.SiteCategoryTypeID = String.Empty
                mySiteParameter.PageID = String.Empty
                mySiteParameter.SiteCategoryGroupID = String.Empty
                Me.Add(mySiteParameter)
            Next
        Catch ex As Exception
            wpmUTIL.AuditLog("Error on PopulateParameterTypeList.GetParameterTypeList", ex.ToString)
        End Try
    End Function

    Public Function ReplaceSiteParameterTags(ByRef sbContent As StringBuilder, ByRef mySiteMapRow As wpmLocation) As Boolean
        ' Find All Parameters that apply for a given SiteMapRow
        For Each mySiteParameter As wpmSiteParameter In Me
            If (mySiteMapRow.SiteCategoryGroupID = mySiteParameter.SiteCategoryGroupID Or mySiteParameter.SiteCategoryGroupID = String.Empty) Then
                If (mySiteMapRow.PageID = mySiteParameter.PageID) Then
                    sbContent.Replace("~~" & mySiteParameter.SiteParameterTypeNM & "~~", mySiteParameter.ParameterValue)
                End If
            End If
        Next
        For Each mySiteParameter As wpmSiteParameter In Me
            If (mySiteMapRow.SiteCategoryGroupID = mySiteParameter.SiteCategoryGroupID Or mySiteParameter.SiteCategoryGroupID = String.Empty) Then
                If (mySiteMapRow.PageID = mySiteParameter.PageID Or mySiteParameter.PageID = String.Empty) Then
                    sbContent.Replace("~~" & mySiteParameter.SiteParameterTypeNM & "~~", mySiteParameter.ParameterValue)
                End If
            End If
        Next
        Return True
    End Function
End Class


'<DataObject(True)> _
'Public Class wpmSiteParameterAdapter
'    Private Property _data() As wpmSiteParameterList
'        Get
'            Dim myActiveSite As New wpmActiveSite(HttpContext.Current.Session)
'            Return myActiveSite.SiteParameterList
'        End Get
'        Set(ByVal value As wpmSiteParameterList)
'            HttpContext.Current.Session("_SiteParameterList") = value
'        End Set
'    End Property

'    <DataObjectMethod(DataObjectMethodType.[Select], True)> _
'    Public Function GetAll() As wpmSiteParameterList
'        Return _data
'    End Function

'    <DataObjectMethod(DataObjectMethodType.Delete, True)> _
'    Public Sub DeleteSiteParameter(ByVal SiteParameter As wpmSiteParameter)
'        Dim SiteParameterList As wpmSiteParameterList = _data
'        SiteParameterList.Remove(SiteParameter)
'        _data = SiteParameterList
'    End Sub

'    <DataObjectMethod(DataObjectMethodType.Update, True)> _
'    Public Sub UpdateSiteParameter(ByVal SiteParameter As wpmSiteParameter)
'        Dim SiteParameterList As wpmSiteParameterList = _data
'        SiteParameterList.Remove(SiteParameter)
'        SiteParameterList.Add(SiteParameter)
'        _data = SiteParameterList
'    End Sub

'    <DataObjectMethod(DataObjectMethodType.Insert, True)> _
'    Public Sub InsertSiteParameter(ByVal SiteParameter As wpmSiteParameter)
'        Dim SiteParameterList As wpmSiteParameterList = _data
'        SiteParameterList.Add(SiteParameter)
'        _data = SiteParameterList
'    End Sub
'End Class
