Public Class wpmSiteGroup
    Private _SiteCategoryGroupID As String
    Public Property SiteCategoryGroupID() As String
        Get
            Return _SiteCategoryGroupID
        End Get
        Set(ByVal value As String)
            _SiteCategoryGroupID = value
        End Set
    End Property
    Private _SiteCategoryGroupNM As String
    Public Property SiteCategoryGroupNM() As String
        Get
            Return _SiteCategoryGroupNM
        End Get
        Set(ByVal value As String)
            _SiteCategoryGroupNM = value
        End Set
    End Property
    Private _SiteCategoryGroupDS As String
    Public Property SiteCategoryGroupDS() As String
        Get
            Return _SiteCategoryGroupDS
        End Get
        Set(ByVal value As String)
            _SiteCategoryGroupDS = value
        End Set
    End Property
    Private _SiteCategoryGroupOrder As Integer
    Public Property SiteCategoryGroupOrder() As Integer
        Get
            Return _SiteCategoryGroupOrder
        End Get
        Set(ByVal value As Integer)
            _SiteCategoryGroupOrder = value
        End Set
    End Property
End Class

Public Class wpmSiteGroupList
    Inherits List(Of wpmSiteGroup)
    Public Function PopulateSiteGroupList(ByVal CompanyID As String) As Boolean
        Dim bReturn As Boolean = True
        Try
            For Each myrow As DataRow In wpmDataCon.GetSiteGroupList(CompanyID).Rows
                Dim myGroupRow As New wpmSiteGroup
                myGroupRow.SiteCategoryGroupID = wpmUTIL.GetDBString(myrow("SiteCategoryGroupID"))
                myGroupRow.SiteCategoryGroupNM = wpmUTIL.GetDBString(myrow("SiteCategoryGroupNM"))
                myGroupRow.SiteCategoryGroupDS = wpmUTIL.GetDBString(myrow("SiteCategoryGroupDS"))
                myGroupRow.SiteCategoryGroupOrder = wpmUTIL.GetDBInteger(myrow("SiteCategoryGroupOrder"))
                Me.Add(myGroupRow)
            Next
        Catch ex As Exception
            bReturn = False
            wpmUTIL.AuditLog("ERROR ON wpmSiteGroupList.PopulateSiteGroupList", ex.ToString)
        End Try
        Return bReturn
    End Function
End Class

