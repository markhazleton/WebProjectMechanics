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
