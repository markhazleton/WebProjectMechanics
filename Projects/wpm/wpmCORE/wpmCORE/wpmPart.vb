Public Class wpmPart
#Region "properties"
    Private _LinkTitle As String
    Public Property LinkTitle() As String
        Get
            Return _LinkTitle
        End Get
        Set(ByVal value As String)
            _LinkTitle = value
        End Set
    End Property
    Private _LinkURL As String

    Public Property LinkURL() As String
        Get
            Return _LinkURL
        End Get
        Set(ByVal value As String)
            _LinkURL = value
        End Set
    End Property
    Private _LinkDescription As String
    Public Property LinkDescription() As String
        Get
            Return _LinkDescription
        End Get
        Set(ByVal value As String)
            _LinkDescription = value
        End Set
    End Property
    Private _LinkASIN As String
    Public Property LinkASIN() As String
        Get
            Return _LinkASIN
        End Get
        Set(ByVal value As String)
            _LinkASIN = value
        End Set
    End Property

    Private _LinkCategoryID As String
    Public Property LinkCategoryID() As String
        Get
            Return _LinkCategoryID
        End Get
        Set(ByVal value As String)
            _LinkCategoryID = value
        End Set
    End Property
    Private _LinkTypeCD As String
    Public Property LinkTypeCD() As String
        Get
            Return _LinkTypeCD
        End Get
        Set(ByVal value As String)
            _LinkTypeCD = value
        End Set
    End Property
    Private _LinkCategoryTitle As String
    Public Property LinkCategoryTitle() As String
        Get
            Return _LinkCategoryTitle
        End Get
        Set(ByVal value As String)
            _LinkCategoryTitle = value
        End Set
    End Property
    Private _LinkID As String

    Public Property LinkID() As String
        Get
            Return _LinkID
        End Get
        Set(ByVal value As String)
            _LinkID = value
        End Set
    End Property
    Private _SiteCategoryTypeID As String
    Public Property SiteCategoryTypeID() As String
        Get
            Return _SiteCategoryTypeID
        End Get
        Set(ByVal value As String)
            _SiteCategoryTypeID = value
        End Set
    End Property
    Private _SiteCategoryGroupID As String
    Public Property SiteCategoryGroupID() As String
        Get
            Return _SiteCategoryGroupID
        End Get
        Set(ByVal value As String)
            _SiteCategoryGroupID = value
        End Set
    End Property
    Private _PageID As String
    Public Property PageID() As String
        Get
            Return _PageID
        End Get
        Set(ByVal value As String)
            If value = "CAT-" Then
                value = ""
            End If
            _PageID = value
        End Set
    End Property
    Private _Views As Boolean
    Public Property View() As Boolean
        Get
            Return _Views
        End Get
        Set(ByVal value As Boolean)
            _Views = value
        End Set
    End Property
    Private _modifiedDT As Date
    Public Property ModifiedDT() As Date
        Get
            Return _modifiedDT
        End Get
        Set(ByVal Value As Date)
            _modifiedDT = Value
        End Set
    End Property
    Private _linkRank As Integer
    Public Property LinkRank() As Integer
        Get
            Return _linkRank
        End Get
        Set(ByVal Value As Integer)
            _linkRank = Value
        End Set
    End Property
    Private _userName As String
    Public Property UserName() As String
        Get
            Return _userName
        End Get
        Set(ByVal Value As String)
            _userName = Value
        End Set
    End Property
    Private _userID As String
    Public Property UserID() As String
        Get
            Return _userID
        End Get
        Set(ByVal Value As String)
            _userID = Value
        End Set
    End Property
    Private _amazonIndex As String
    Public Property AmazonIndex() As String
        Get
            Return _amazonIndex
        End Get
        Set(ByVal Value As String)
            _amazonIndex = Value
        End Set
    End Property
    Private _LinkSource As String
    Public Property LinkSource() As String
        Get
            Return _LinkSource
        End Get
        Set(ByVal value As String)
            _LinkSource = value
        End Set
    End Property
    Private _LinkCompanyID As String
    Public Property LinkCompanyID() As String
        Get
            Return _LinkCompanyID
        End Get
        Set(ByVal value As String)
            _LinkCompanyID = value
        End Set
    End Property
#End Region
End Class
Public Class wpmPartList
    Inherits List(Of wpmPart)

    Public Function PopulateSiteCategoryLinkRows(ByVal CompanyID As String, ByVal SiteCategoryTypeID As String) As Boolean
        Dim bReturn As Boolean = True
        Try
            For Each myrow As DataRow In wpmDataCon.GetSiteCategoryLinks(CompanyID, SiteCategoryTypeID).Rows
                Dim myLinkRow As New wpmPart
                myLinkRow.LinkID = wpmUTIL.GetDBString(myrow("ID"))
                myLinkRow.LinkTypeCD = wpmUTIL.GetDBString(myrow("LinkTypeCD"))
                myLinkRow.LinkCategoryTitle = wpmUTIL.GetDBString(myrow("LinkCategoryTitle"))
                myLinkRow.LinkCategoryID = wpmUTIL.GetDBString(myrow("CategoryID"))
                myLinkRow.PageID = wpmUTIL.GetDBString(myrow("PageID"))
                myLinkRow.LinkTitle = wpmUTIL.GetDBString(myrow("LinkTitle"))
                myLinkRow.LinkDescription = wpmUTIL.GetDBString(myrow("Description"))
                myLinkRow.LinkURL = wpmUTIL.GetDBString(myrow("URL"))
                myLinkRow.ModifiedDT = wpmUTIL.GetDBDate(myrow("DateAdd"))
                myLinkRow.LinkRank = wpmUTIL.GetDBInteger(myrow("Ranks"))
                myLinkRow.View = wpmUTIL.GetDBBoolean(myrow("Views"))
                myLinkRow.UserName = wpmUTIL.GetDBString(myrow("UserName"))
                myLinkRow.UserID = wpmUTIL.GetDBString(myrow("UserID"))
                myLinkRow.AmazonIndex = wpmUTIL.GetDBString(myrow("ASIN"))
                myLinkRow.LinkSource = wpmUTIL.GetDBString(myrow("LinkSource"))
                myLinkRow.LinkCompanyID = wpmUTIL.GetDBString(myrow("CompanyID"))
                myLinkRow.SiteCategoryGroupID = wpmUTIL.GetDBString(myrow("SiteCategoryGroupID"))
                myLinkRow.SiteCategoryTypeID = wpmUTIL.GetDBString(myrow("SiteCategoryTypeID"))
                Me.Add(myLinkRow)
            Next
        Catch ex As Exception
            bReturn = False
            wpmUTIL.AuditLog("ERROR ON wpmSiteLinkRows.PopulateSiteLinkRows-Cateogry()", ex.ToString)
        End Try
        Return bReturn
    End Function
    Public Function PopulateSiteLinkRows(ByVal CompanyID As String) As Boolean
        Dim bReturn As Boolean = True
        Try
            For Each myrow As DataRow In wpmDataCon.GetSiteLinks(CompanyID).Rows
                Dim myLinkRow As New wpmPart
                myLinkRow.LinkID = wpmUTIL.GetDBString(myrow("ID"))
                myLinkRow.LinkTypeCD = wpmUTIL.GetDBString(myrow("LinkTypeCD"))
                myLinkRow.LinkCategoryTitle = wpmUTIL.GetDBString(myrow("LinkCategoryTitle"))
                myLinkRow.LinkCategoryID = wpmUTIL.GetDBString(myrow("CategoryID"))
                myLinkRow.PageID = wpmUTIL.GetDBString(myrow("PageID"))
                myLinkRow.LinkTitle = wpmUTIL.GetDBString(myrow("LinkTitle"))
                myLinkRow.LinkDescription = wpmUTIL.GetDBString(myrow("Description"))
                myLinkRow.LinkURL = wpmUTIL.GetDBString(myrow("URL"))
                myLinkRow.ModifiedDT = wpmUTIL.GetDBDate(myrow("DateAdd"))
                myLinkRow.LinkRank = wpmUTIL.GetDBInteger(myrow("Ranks"))
                myLinkRow.View = wpmUTIL.GetDBBoolean(myrow("Views"))
                myLinkRow.UserName = wpmUTIL.GetDBString(myrow("UserName"))
                myLinkRow.UserID = wpmUTIL.GetDBString(myrow("UserID"))
                myLinkRow.AmazonIndex = wpmUTIL.GetDBString(myrow("ASIN"))
                myLinkRow.LinkSource = "Link"
                myLinkRow.LinkCompanyID = wpmUTIL.GetDBString(myrow("CompanyID"))
                myLinkRow.SiteCategoryGroupID = wpmUTIL.GetDBString(myrow("SiteCategoryGroupID"))
                myLinkRow.SiteCategoryTypeID = wpmUTIL.GetDBString(myrow("SiteCategoryTypeID"))
                Me.Add(myLinkRow)
            Next
        Catch innerex As Exception
            bReturn = False
            wpmUTIL.AuditLog("ERROR ON wpmSiteLinkRows.PopulateSiteLinkRows()", innerex.ToString)
        End Try
        Return bReturn
    End Function
    '
    '  Find 
    '
    Public Function GetActiveParts() As List(Of wpmPart)
        Return Me.FindAll(AddressOf FindActiveParts)
    End Function
    Private Function FindActiveParts(ByVal Part As wpmPart) As Boolean
        Dim bReturn As Boolean = False
        If Part.View Then
            If ((Part.LinkCategoryTitle = "LeftColumnLinks") Or _
                (Part.LinkCategoryTitle = "RightColumnLinks") Or _
                (Part.LinkCategoryTitle = "CenterColumnLinks")) Then
                bReturn = True
            End If
        End If
        Return bReturn
    End Function
End Class
