Public Class wpmLocation
    Inherits ItemForCollection
    Implements IComparable(Of wpmLocation)
    
    Public Property PageID() As String
    Public Property PageName() As String
    Public Property PageTitle() As String
    Public Property PageDescription() As String
    Public Property PageKeywords() As String
    Public Property ParentPageID() As String
    Public Property DefaultOrder() As Integer
    Public Property LocationTrailList() As New wpmLocationTrailList
    Public Property RecordSource() As String
    Public Property TransferURL() As String
    Public Property BreadCrumbURL() As String
    Public Property LevelNBR() As Integer
    Public Property PageFileName() As String
    Public Property ModifiedDate() As Date
    Public Property ArticleID() As String
    Public Property ActiveFL() As Boolean
    Public Property BreadCrumbHTML() As String
    Public Property MainMenuURL() As String
    Public Property MainMenuPageID() As String
    Public Property MainMenuPageName() As String
    Public Property SiteCategoryName() As String
    Public Property SiteCategoryID() As String
    Public Property SiteCategoryGroupName() As String
    Public Property SiteCategoryGroupID() As String

    Private _PageTypeCD As String
    Public Property PageTypeCD() As String
        Get
            If _PageTypeCD Is Nothing Then
                Return String.Empty
            Else
                Return _PageTypeCD
            End If
        End Get
        Set(ByVal value As String)
            _PageTypeCD = value
        End Set
    End Property
    Private _DisplayURL As String
    Public Property DisplayURL() As String
        Get
            If wpmApp.Config.Use404Processing Then
                Return _DisplayURL
            Else
                Return TransferURL
            End If
        End Get
        Set(ByVal value As String)
            _DisplayURL = (value.ToLower)
        End Set
    End Property

    Public Function UpdatePageRow(ByRef FoundPageRow As wpmLocation) As Boolean
        ModifiedDate = FoundPageRow.ModifiedDate
        ActiveFL = FoundPageRow.ActiveFL
        RecordSource = FoundPageRow.RecordSource
        PageID = FoundPageRow.PageID
        ParentPageID = FoundPageRow.ParentPageID
        ArticleID = FoundPageRow.ArticleID
        PageName = FoundPageRow.PageName
        PageTitle = FoundPageRow.PageTitle
        PageKeywords = FoundPageRow.PageKeywords
        PageDescription = FoundPageRow.PageDescription
        BreadCrumbHTML = FoundPageRow.BreadCrumbHTML
        LevelNBR = FoundPageRow.LevelNBR
        LocationTrailList = FoundPageRow.LocationTrailList
        PageTypeCD = FoundPageRow.PageTypeCD
        DisplayURL = FoundPageRow.DisplayURL
        TransferURL = FoundPageRow.TransferURL
        SiteCategoryID = FoundPageRow.SiteCategoryID
        SiteCategoryName = FoundPageRow.SiteCategoryName
        SiteCategoryGroupName = FoundPageRow.SiteCategoryGroupName
        SiteCategoryGroupID = FoundPageRow.SiteCategoryGroupID
        DefaultOrder = FoundPageRow.DefaultOrder
        Return True
    End Function

    Public Function BuildClassLink(ByVal LinkClass As String, ByRef UseBreadcrumbURL As Boolean) As String
        If UseBreadcrumbURL Then
            Return (String.Format("<a class=""{0}"" href=""{1}"">{2}</a>", LinkClass, BreadCrumbURL, PageName))
        Else
            Return (String.Format("<a class=""{0}"" href=""{1}"">{2}</a>", LinkClass, DisplayURL, PageName))
        End If
    End Function

    Public Function GetSiteMapRowURL(ByRef UseBreadCrumbURL As Boolean) As String
        If StrConv(Left(TransferURL, 4), VbStrConv.Lowercase) = "http" Then
            Return TransferURL
        Else
            If wpmApp.Config.Use404Processing Then
                If UseBreadCrumbURL Then
                    Return BreadCrumbURL
                Else
                    Return DisplayURL
                End If
            Else
                Return TransferURL
            End If
        End If

    End Function

    Public Function CompareTo(ByVal other As wpmLocation) As Integer Implements System.IComparable(Of wpmLocation).CompareTo
        Return DefaultOrder.CompareTo(other.DefaultOrder)
    End Function


    Public Function UpdateLocation() As Boolean


        Return True
    End Function



End Class