Public Class wpmLocation
    Implements IComparable(Of wpmLocation)
    Private _DefaultOrder As Integer
    Public Property DefaultOrder() As Integer
        Get
            Return _DefaultOrder
        End Get
        Set(ByVal value As Integer)
            _DefaultOrder = value
        End Set
    End Property
    Private _PageID As String
    Public Property PageID() As String
        Get
            Return _PageID
        End Get
        Set(ByVal value As String)
            If value = "0" Then
                value = ""
            End If
            _PageID = value
        End Set
    End Property
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
    Private _LocationTrailList As New wpmLocationTrailList
    Public Property LocationTrailList() As wpmLocationTrailList
        Get
            Return _LocationTrailList
        End Get
        Set(ByVal value As wpmLocationTrailList)
            _LocationTrailList = value
        End Set
    End Property

    Private _PageName As String
    Public Property PageName() As String
        Get
            Return _PageName
        End Get
        Set(ByVal value As String)
            _PageName = value
        End Set
    End Property
    Private _PageTitle As String
    Public Property PageTitle() As String
        Get
            Return _PageTitle
        End Get
        Set(ByVal value As String)
            _PageTitle = value
        End Set
    End Property
    Private _PageDescription As String
    Public Property PageDescription() As String
        Get
            Return _PageDescription
        End Get
        Set(ByVal value As String)
            _PageDescription = value
        End Set
    End Property
    Private _ParentPageID As String
    Public Property ParentPageID() As String
        Get
            Return _ParentPageID
        End Get
        Set(ByVal value As String)
            If value = "0" Then
                value = ""
            End If
            _ParentPageID = value
        End Set
    End Property
    Private _RecordSource As String

    Public Property RecordSource() As String
        Get
            Return _RecordSource
        End Get
        Set(ByVal value As String)
            _RecordSource = value
        End Set
    End Property
    Private _PageKeywords As String

    Public Property PageKeywords() As String
        Get
            Return _PageKeywords
        End Get
        Set(ByVal value As String)
            _PageKeywords = value
        End Set
    End Property

    Private _TransferURL As String
    Public Property TransferURL() As String
        Get
            Return _TransferURL
        End Get
        Set(ByVal value As String)
            _TransferURL = value
        End Set
    End Property
    Private _DisplayURL As String
    Public Property DisplayURL() As String
        Get
            If App.Config.Use404Processing Then
                Return _DisplayURL
            Else
                Return _TransferURL
            End If
        End Get
        Set(ByVal value As String)
            _DisplayURL = (value.ToLower)
        End Set
    End Property
    Private _BreadCrumbURL As String
    Public Property BreadCrumbURL() As String
        Get
            Return _BreadCrumbURL
        End Get
        Set(ByVal value As String)
            _BreadCrumbURL = (value.ToLower)
        End Set
    End Property

    Private _LevelNBR As Integer
    Public Property LevelNBR() As Integer
        Get
            Return _LevelNBR
        End Get
        Set(ByVal value As Integer)
            _LevelNBR = value
        End Set
    End Property

    Private _PageFileName As String
    Public Property PageFileName() As String
        Get
            Return _PageFileName
        End Get
        Set(ByVal value As String)
            _PageFileName = value
        End Set
    End Property

    Private _ModifiedDate As Date

    Public Property ModifiedDate() As Date
        Get
            Return _ModifiedDate
        End Get
        Set(ByVal value As Date)
            _ModifiedDate = value
        End Set
    End Property

    Private _ArticleID As String
    Public Property ArticleID() As String
        Get
            Return _ArticleID
        End Get
        Set(ByVal value As String)
            If value = "0" Then
                value = ""
            End If
            _ArticleID = value
        End Set
    End Property
    Private _ActiveFL As Boolean

    Public Property ActiveFL() As Boolean
        Get
            Return _ActiveFL
        End Get
        Set(ByVal value As Boolean)
            _ActiveFL = value
        End Set
    End Property

    Private _BreadCrumbHTML As String
    Public Property BreadCrumbHTML() As String
        Get
            Return _BreadCrumbHTML
        End Get
        Set(ByVal value As String)
            _BreadCrumbHTML = value
        End Set
    End Property
    Private _MainMenuURL As String
    Public Property MainMenuURL() As String
        Get
            Return _MainMenuURL
        End Get
        Set(ByVal value As String)
            _MainMenuURL = value
        End Set
    End Property
    Private _MainMenuPageID As String
    Public Property MainMenuPageID() As String
        Get
            Return _MainMenuPageID
        End Get
        Set(ByVal value As String)
            If value = "0" Then
                value = ""
            End If

            _MainMenuPageID = value
        End Set
    End Property
    Private _MainMenuPageName As String
    Public Property MainMenuPageName() As String
        Get
            Return _MainMenuPageName
        End Get
        Set(ByVal value As String)
            _MainMenuPageName = value
        End Set
    End Property
    Private _TopMenu As String
    Public Property TopMenu() As String
        Get
            Return _TopMenu
        End Get
        Set(ByVal value As String)
            _TopMenu = value
        End Set
    End Property
    Private _SubMenu As String
    Public Property SubMenu() As String
        Get
            Return _SubMenu
        End Get
        Set(ByVal value As String)
            _SubMenu = value
        End Set
    End Property
    Private _siteCategoryName As String
    Public Property SiteCategoryName() As String
        Get
            Return _siteCategoryName
        End Get
        Set(ByVal Value As String)
            _siteCategoryName = Value
        End Set
    End Property

    Private _siteCategoryID As String
    Public Property SiteCategoryID() As String
        Get
            Return _siteCategoryID
        End Get
        Set(ByVal Value As String)
            If Value = "0" Then
                Value = ""
            End If
            _siteCategoryID = Value
        End Set
    End Property

    Private _siteCategoryGroupName As String
    Public Property SiteCategoryGroupName() As String
        Get
            Return _siteCategoryGroupName
        End Get
        Set(ByVal Value As String)
            _siteCategoryGroupName = Value
        End Set
    End Property
    Private _siteCategoryGroupID As String
    Public Property SiteCategoryGroupID() As String
        Get
            Return _siteCategoryGroupID
        End Get
        Set(ByVal Value As String)
            _siteCategoryGroupID = Value
        End Set
    End Property

    Public Function UpdatePageRow(ByRef FoundPageRow As wpmLocation) As Boolean
        Me.ModifiedDate = FoundPageRow.ModifiedDate
        Me.ActiveFL = FoundPageRow.ActiveFL
        Me.RecordSource = FoundPageRow.RecordSource
        Me.PageID = FoundPageRow.PageID
        Me.ParentPageID = FoundPageRow.ParentPageID
        Me.ArticleID = FoundPageRow.ArticleID
        Me.PageName = FoundPageRow.PageName
        Me.PageTitle = FoundPageRow.PageTitle
        Me.PageKeywords = FoundPageRow.PageKeywords
        Me.PageDescription = FoundPageRow.PageDescription
        Me.BreadCrumbHTML = FoundPageRow.BreadCrumbHTML
        Me.LevelNBR = FoundPageRow.LevelNBR
        Me.LocationTrailList = FoundPageRow.LocationTrailList
        Me.PageTypeCD = FoundPageRow.PageTypeCD
        Me.DisplayURL = FoundPageRow.DisplayURL
        Me.TransferURL = FoundPageRow.TransferURL
        Me.SiteCategoryID = FoundPageRow.SiteCategoryID
        Me.SiteCategoryName = FoundPageRow.SiteCategoryName
        Me.SiteCategoryGroupName = FoundPageRow.SiteCategoryGroupName
        Me.SiteCategoryGroupID = FoundPageRow.SiteCategoryGroupID
        Return True
    End Function

    Public Function BuildClassLink(ByVal LinkClass As String, ByRef UseBreadcrumbURL As Boolean) As String
        If UseBreadcrumbURL Then
            Return ("<a class=""" & LinkClass & """ href=""" & Me.BreadCrumbURL & """>" & Me.PageName & "</a>")
        Else
            Return ("<a class=""" & LinkClass & """ href=""" & Me.DisplayURL & """>" & Me.PageName & "</a>")
        End If
    End Function

    Public Function GetSiteMapRowURL(ByRef UseBreadCrumbURL As Boolean) As String
        If StrConv(Left(Me.TransferURL, 4), VbStrConv.Lowercase) = "http" Then
            Return Me.TransferURL
        Else
            If App.Config.Use404Processing Then
                If UseBreadCrumbURL Then
                    Return Me.BreadCrumbURL
                Else
                    Return Me.DisplayURL
                End If
            Else
                Return Me.TransferURL
            End If
        End If

    End Function

    Public Function CompareTo(ByVal other As wpmLocation) As Integer Implements System.IComparable(Of wpmLocation).CompareTo
        Return Me._DefaultOrder.CompareTo(other._DefaultOrder)
    End Function
End Class
Public Class LocationDefaultOrderCompare
    Implements IComparer(Of wpmLocation)
    Protected _direction As wpmSortDirection = wpmSortDirection.ASC
    Public Property Direction() As wpmSortDirection
        Get
            Return _direction
        End Get
        Set(ByVal value As wpmSortDirection)
            _direction = value
        End Set
    End Property
    Public Function Compare(ByVal x As wpmLocation, ByVal y As wpmLocation) As Integer Implements System.Collections.Generic.IComparer(Of wpmLocation).Compare
        If _direction = wpmSortDirection.ASC Then
            Return x.DefaultOrder.CompareTo(y.DefaultOrder)
        Else
            Return x.DefaultOrder.CompareTo(y.DefaultOrder) * -1
        End If
    End Function
End Class
Public Class LocationPageNameCompare
    Implements IComparer(Of wpmLocation)
    Protected _direction As wpmSortDirection = wpmSortDirection.ASC
    Public Property Direction() As wpmSortDirection
        Get
            Return _direction
        End Get
        Set(ByVal value As wpmSortDirection)
            _direction = value
        End Set
    End Property
    Public Function Compare(ByVal x As wpmLocation, ByVal y As wpmLocation) As Integer Implements System.Collections.Generic.IComparer(Of wpmLocation).Compare
        If _direction = wpmSortDirection.ASC Then
            Return x.PageName.CompareTo(y.PageName)
        Else
            Return x.PageName.CompareTo(y.PageName) * -1
        End If
    End Function
End Class
Public Class LocationRecordSourceCompare
    Implements IComparer(Of wpmLocation)
    Protected _direction As wpmSortDirection = wpmSortDirection.ASC
    Public Property Direction() As wpmSortDirection
        Get
            Return _direction
        End Get
        Set(ByVal value As wpmSortDirection)
            _direction = value
        End Set
    End Property
    Public Function Compare(ByVal x As wpmLocation, ByVal y As wpmLocation) As Integer Implements System.Collections.Generic.IComparer(Of wpmLocation).Compare
        If _direction = wpmSortDirection.ASC Then
            Return x.RecordSource.CompareTo(y.RecordSource)
        Else
            Return x.RecordSource.CompareTo(y.RecordSource) * -1
        End If
    End Function
End Class
Public Class LocationModifiedDateCompare
    Implements IComparer(Of wpmLocation)
    Protected _direction As wpmSortDirection = wpmSortDirection.ASC
    Public Property Direction() As wpmSortDirection
        Get
            Return _direction
        End Get
        Set(ByVal value As wpmSortDirection)
            _direction = value
        End Set
    End Property
    Public Function Compare(ByVal x As wpmLocation, ByVal y As wpmLocation) As Integer Implements System.Collections.Generic.IComparer(Of wpmLocation).Compare
        If _direction = wpmSortDirection.ASC Then
            Return x.ModifiedDate.CompareTo(y.ModifiedDate)
        Else
            Return x.ModifiedDate.CompareTo(y.ModifiedDate) * -1
        End If
    End Function
End Class
Public Class wpmKeyword
    Private _Code As String
    Public ReadOnly Property Code() As String
        Get
            Return _Code
        End Get
    End Property
    Public Sub New(ByVal value As String)
        _Code = value
    End Sub
End Class
Public Class KeywordPredicateClass
    Private code As String
    Public Sub New(ByVal code As String)
        Me.code = LCase(Trim(code))
    End Sub
    Public Function PredicateFunction(ByVal bo As wpmKeyword) As Boolean
        Return LCase(Trim(bo.Code)) = LCase(Trim(Me.code))
    End Function
End Class
Public Class KeywordDefaultOrderCompare
    Implements IComparer(Of wpmKeyword)
    Protected _direction As wpmSortDirection = wpmSortDirection.DESC
    Public Property Direction() As wpmSortDirection
        Get
            Return _direction
        End Get
        Set(ByVal value As wpmSortDirection)
            _direction = value
        End Set
    End Property

    Public Function Compare(ByVal x As wpmKeyword, ByVal y As wpmKeyword) As Integer Implements System.Collections.Generic.IComparer(Of wpmKeyword).Compare
        If _direction = wpmSortDirection.ASC Then
            Return x.Code.CompareTo(y.Code)
        Else
            Return x.Code.CompareTo(y.Code) * -1
        End If

    End Function
End Class
Public Class wpmKeywordList
    Inherits List(Of wpmKeyword)
    Public Function AddToList(ByVal ValueToAdd As String) As Boolean
        If (FindKeywordValue(wpmUTIL.RemoveTags(ValueToAdd)) Is Nothing) Then
            Dim myKeyword As New wpmKeyword(wpmUTIL.RemoveTags(ValueToAdd))
            Me.Add(myKeyword)
        End If
        Return True
    End Function
    Private Function FindKeyword(ByVal code As String) As wpmKeyword
        For Each p As wpmKeyword In Me
            If p.Code = code Then Return p
        Next
        Return Nothing
    End Function
    Function FindKeywordValue(ByVal code As String) As wpmKeyword
        Dim pred As New KeywordPredicateClass(code)
        Return Me.Find(AddressOf pred.PredicateFunction)
    End Function

    Public Overloads Function Sort() As Boolean
        Dim myDefaultComp As KeywordDefaultOrderCompare = New KeywordDefaultOrderCompare()
        myDefaultComp.Direction = wpmSortDirection.ASC
        Me.Sort(myDefaultComp)
    End Function

End Class
Public Class wpmLocationList
    Inherits List(Of wpmLocation)
    Private _KeywordList As New wpmKeywordList
    Public Property KeywordList() As wpmKeywordList
        Get
            Return _KeywordList
        End Get
        Set(ByVal value As wpmKeywordList)
            _KeywordList = value
        End Set
    End Property
    Private SearchPageID As String
    Private SearchArticleID As String
    Private SearchKeyword As String
    Private SearchRecordSource As String

    Public Function GetLocationListFromDb(ByVal OrderBy As String, ByVal CompanyID As String, ByVal GroupID As String) As Boolean
        Dim bReturn As Boolean = True
        Dim sTransferURL As String = ("")
        Dim sTransferParms As String = ("")
        Dim iDefaultOrder As Integer = 0
        For Each myrow As DataRow In wpmDataCon.GetSiteMap(OrderBy, CompanyID, GroupID).Rows
            Dim MySiteRow As New wpmLocation
            MySiteRow.DefaultOrder = iDefaultOrder
            iDefaultOrder = iDefaultOrder + 1
            MySiteRow.PageID = wpmUTIL.GetDBString(myrow("PageID"))
            MySiteRow.PageName = wpmUTIL.GetDBString(myrow("PageName"))
            MySiteRow.PageTitle = wpmUTIL.GetDBString(myrow("PageTitle"))
            MySiteRow.ActiveFL = wpmUTIL.GetDBBoolean(myrow("Active"))
            MySiteRow.ArticleID = wpmUTIL.GetDBString(myrow("ArticleID"))
            MySiteRow.ModifiedDate = wpmUTIL.GetDBDate(myrow("ModifiedDT"))
            MySiteRow.PageDescription = wpmUTIL.GetDBString(myrow("Description"))
            MySiteRow.PageFileName = wpmUTIL.GetDBString(myrow("PageFileName"))
            MySiteRow.PageKeywords = wpmUTIL.GetDBString(myrow("PageKeywords"))
            MySiteRow.ParentPageID = wpmUTIL.GetDBString(myrow("ParentPageID"))
            MySiteRow.RecordSource = wpmUTIL.GetDBString(myrow("PageSource"))
            sTransferURL = wpmUTIL.GetDBString(myrow("TransferURL"))
            MySiteRow.SiteCategoryID = wpmUTIL.GetDBString(myrow("SiteCategoryID"))
            MySiteRow.SiteCategoryName = wpmUTIL.GetDBString(myrow("SitecategoryName"))
            MySiteRow.SiteCategoryGroupName = wpmUTIL.GetDBString(myrow("SiteCategoryGroupName"))
            MySiteRow.SiteCategoryGroupID = wpmUTIL.GetDBString(myrow("SiteCategoryGroupID"))
            MySiteRow.PageTypeCD = wpmUTIL.GetDBString(myrow("PageTypeCD"))
            If (sTransferURL.Trim) = String.Empty Then
                sTransferURL = MySiteRow.PageFileName
            End If

            ' Check for Recursive Parent
            If (MySiteRow.PageID = MySiteRow.ParentPageID And MySiteRow.ParentPageID <> "") Then
                MySiteRow.ParentPageID = ""
                wpmLog.AuditLog("PageID=ParentPageID (" & MySiteRow.PageName & " - " & MySiteRow.PageID & ") ", "wpmSiteMapRow.PopulateSiteMapCol")
            End If

            Select Case MySiteRow.RecordSource
                Case "Page"
                    sTransferParms = "c=" & MySiteRow.PageID
                    If MySiteRow.ArticleID <> "" Then
                        sTransferParms = sTransferParms & "&a=" & MySiteRow.ArticleID
                    End If
                Case "Article"
                    sTransferParms = "a=" & MySiteRow.ArticleID
                    If MySiteRow.PageID <> "" Then
                        sTransferParms = sTransferParms & "&c=" & MySiteRow.PageID
                    End If
                Case "Image"
                    sTransferParms = "i=" & MySiteRow.ArticleID
                    If MySiteRow.PageID <> "" Then
                        sTransferParms = sTransferParms & "&c=" & MySiteRow.PageID
                    End If
                Case "Category"
                    MySiteRow.PageID = "CAT-" & MySiteRow.PageID
                    If MySiteRow.ParentPageID <> "" Then
                        MySiteRow.ParentPageID = "CAT-" & MySiteRow.ParentPageID
                    End If
                    sTransferParms = "c=" & MySiteRow.PageID
                Case Else
                    sTransferParms = "c=" & MySiteRow.PageID & "&rc=" & MySiteRow.RecordSource
            End Select

            If (MySiteRow.PageFileName.Trim) <> String.Empty Then
                sTransferURL = MySiteRow.PageFileName
                MySiteRow.PageTypeCD = "NOFILENAME"
            ElseIf sTransferURL.IndexOf("?") > 0 Then
                If InStr(sTransferURL, "c=") = 0 Then
                    sTransferURL = sTransferURL & "&" & sTransferParms
                End If
            Else
                sTransferURL = sTransferURL & "?" & sTransferParms
            End If

            If MySiteRow.PageTypeCD = "NO FILE NAME" Then
                If MySiteRow.PageFileName.IndexOf("?") > 0 Then
                    If InStr(MySiteRow.PageFileName, "c=") = 0 Then
                        MySiteRow.PageFileName = MySiteRow.PageFileName & "&" & sTransferParms
                    End If
                Else
                    MySiteRow.PageFileName = MySiteRow.PageFileName & "?" & sTransferParms
                End If
                MySiteRow.TransferURL = MySiteRow.PageFileName
                MySiteRow.DisplayURL = MySiteRow.PageFileName
                MySiteRow.BreadCrumbHTML = MySiteRow.PageFileName
            Else
                MySiteRow.TransferURL = sTransferURL
                sTransferURL = ""
                sTransferParms = ""
                MySiteRow.DisplayURL = wpmUTIL.FormatPageNameURL(MySiteRow.PageName)
            End If
            If MySiteRow.RecordSource = "Page" And MySiteRow.ParentPageID = String.Empty And MySiteRow.SiteCategoryID <> String.Empty Then
                MySiteRow.ParentPageID = "CAT-" & MySiteRow.SiteCategoryID
            End If
            For Each myString As String In Split(MySiteRow.PageKeywords, ",")
                myString = myString.Trim
                myString = myString.ToLower
                If (myString <> String.Empty) Then
                    Me.KeywordList.AddToList(myString)
                End If
            Next
            Me.Add(MySiteRow)
        Next
        Me.KeywordList.Sort()
        Return bReturn
    End Function
    Public Function BuildBreadcrumbRows(ByVal CompanyName As String) As Boolean
        Dim LevelCount As Integer = 1
        For Each myRow As wpmLocation In Me
            'I made a change so it's possible the BreadCrumURL might already be filled, so I check to make sure that is not the case
            'Then, I copied the code to generate the bread crumb to a function as I'll use it in another place
            If (myRow.BreadCrumbURL = String.Empty Or myRow.BreadCrumbURL = "") Then
                GenerateBreadcrumRow(myRow, LevelCount)
            End If
            ' Reset Variables
            LevelCount = 1
        Next
        Return True
    End Function

    'The function to generate the breadcrumb for a row
    Private Function GenerateBreadcrumRow(ByVal myrow As wpmLocation, ByVal levelCount As Integer) As Boolean
        Dim sBreadCrumbHTML As String = String.Empty
        If (myrow.ParentPageID.Trim) <> String.Empty Then
            BuildParentBreadcrumbs(myrow.LocationTrailList, myrow.ParentPageID, levelCount)
        End If
        myrow.LocationTrailList.Add(GetBreadcrumbRow(myrow, levelCount))
        Dim myFullPathURL As New StringBuilder(String.Empty)
        For Each BCrow As wpmLocationTrail In myrow.LocationTrailList
            If BCrow.LevelNBR < levelCount Then
                BCrow.MenuLevelNBR = (levelCount - BCrow.LevelNBR)
                sBreadCrumbHTML = "<li><a href=""" & BCrow.DisplayURL & """ title=""" & BCrow.PageDescription & """>" & BCrow.PageName & "</a></li>" & sBreadCrumbHTML
                myFullPathURL.Insert(0, "/" & wpmUTIL.FormatPageNameForURL(BCrow.PageName))
            Else
                BCrow.MenuLevelNBR = levelCount
            End If
        Next
        myFullPathURL.Append(myrow.DisplayURL)
        If (Left(myrow.PageFileName.ToLower, 4)) <> "http" Then
            myrow.BreadCrumbURL = myFullPathURL.ToString
        Else
            myrow.BreadCrumbURL = myrow.PageFileName
        End If
        sBreadCrumbHTML = sBreadCrumbHTML & "<li><strong>" & myrow.PageName & "</strong></li>"
        myrow.BreadCrumbHTML = "<ul>" & sBreadCrumbHTML & "</ul>"
        myrow.LevelNBR = levelCount

        Return True
    End Function
    Private Function GetBreadcrumbRow(ByVal myrow As wpmLocation, ByVal LevelNBR As Integer) As wpmLocationTrail
        Dim bcRow As New wpmLocationTrail
        bcRow.MenuLevelNBR = 0
        bcRow.LevelNBR = LevelNBR
        bcRow.PageID = myrow.PageID
        bcRow.PageName = myrow.PageName
        bcRow.ParentPageID = myrow.ParentPageID
        bcRow.DisplayURL = myrow.BreadCrumbURL
        bcRow.PageDescription = myrow.PageDescription
        Return bcRow
    End Function
    Private Function BuildParentBreadcrumbs(ByRef BreadCrumbRows As wpmLocationTrailList, ByVal pageID As String, ByRef LevelCount As Integer) As Boolean
        For Each myrow As wpmLocation In Me
            If LevelCount < 10 Then
                If myrow.RecordSource = "Page" Or myrow.RecordSource = "Category" Then
                    If myrow.PageID = pageID Then
                        'It turns out it's possible for a child's bread crumbs to be generated before it's parents.
                        'So, if I run across a parent that doesn't yet have a bread crumb, I generate it.
                        If (myrow.BreadCrumbURL = String.Empty Or myrow.BreadCrumbURL = "") Then
                            GenerateBreadcrumRow(myrow, LevelCount)
                        End If
                        BreadCrumbRows.Add(GetBreadcrumbRow(myrow, LevelCount))
                        LevelCount = LevelCount + 1
                        If (myrow.ParentPageID.Trim) <> "" Then
                            BuildParentBreadcrumbs(BreadCrumbRows, myrow.ParentPageID, LevelCount)
                        End If
                        Exit For
                    End If
                End If
            End If
        Next
        Return True
    End Function
    '
    '  Sort Location List
    '
    Public Function DefaultSort() As Boolean
        Dim myDefaultComp As LocationDefaultOrderCompare = New LocationDefaultOrderCompare()
        myDefaultComp.Direction = wpmSortDirection.ASC
        Me.Sort(myDefaultComp)
    End Function
    Public Function DefaultSort(ByRef Direction As wpmSortDirection) As Boolean
        Dim myDefaultComp As LocationDefaultOrderCompare = New LocationDefaultOrderCompare()
        myDefaultComp.Direction = Direction
        Me.Sort(myDefaultComp)
    End Function
    '
    '  Find A Location (Or List of Locations)
    '
    Public Function GetCatalogLocations() As List(Of wpmLocation)
        Return Me.FindAll(AddressOf FindCatalogLocations)
    End Function

    Public Function FindLocationByKeyword(ByVal inSearchKeyword As String) As wpmLocation
        Dim FoundLocation As New wpmLocation
        SearchKeyword = inSearchKeyword
        For Each Location As wpmLocation In Me.FindAll(AddressOf FindLocationByKeyword)
            FoundLocation.ModifiedDate = Location.ModifiedDate
            FoundLocation.ActiveFL = Location.ActiveFL
            FoundLocation.RecordSource = Location.RecordSource
            FoundLocation.PageID = Location.PageID
            FoundLocation.ParentPageID = Location.ParentPageID
            FoundLocation.ArticleID = Location.ArticleID
            FoundLocation.PageName = Location.PageName
            FoundLocation.PageFileName = Location.PageFileName
            FoundLocation.PageTitle = Location.PageTitle
            FoundLocation.PageKeywords = Location.PageKeywords
            FoundLocation.PageDescription = Location.PageDescription
            FoundLocation.PageFileName = Location.PageFileName
            FoundLocation.BreadCrumbHTML = Location.BreadCrumbHTML
            FoundLocation.LevelNBR = Location.LevelNBR
            FoundLocation.LocationTrailList = Location.LocationTrailList
            FoundLocation.PageTypeCD = Location.PageTypeCD
            FoundLocation.DisplayURL = Location.DisplayURL
            FoundLocation.TransferURL = Location.TransferURL
            FoundLocation.SiteCategoryID = Location.SiteCategoryID
            FoundLocation.SiteCategoryName = Location.SiteCategoryName
            FoundLocation.SiteCategoryGroupName = Location.SiteCategoryGroupName
            FoundLocation.SiteCategoryGroupID = Location.SiteCategoryGroupID
            If (Location.RecordSource = "Image" Or Location.RecordSource = "Article") Then
                FoundLocation.ArticleID = Location.ArticleID
                FoundLocation.PageName = Location.PageName
                FoundLocation.PageKeywords = Location.PageKeywords
                FoundLocation.PageDescription = Location.PageDescription
            End If
        Next
        Return FoundLocation
    End Function
    Public Function GetLocationURLByKeyword(ByVal inSearchKeyword As String) As String
        Dim ReturnURL As String = String.Empty
        SearchKeyword = inSearchKeyword
        If (Me.FindAll(AddressOf FindLocationByKeyword).Count = 1) Then
            ReturnURL = Me.Find(AddressOf FindLocationByKeyword).DisplayURL
        End If
        Return ReturnURL
    End Function

    Public Function FindLocationsByKeyword(ByVal inSearchKeyword As String) As String
        Dim FoundLocations As New StringBuilder(String.Empty)
        SearchKeyword = inSearchKeyword
        FoundLocations.Append("<br/>Search Results for <strong>" & inSearchKeyword & " </strong><br/>")
        FoundLocations.Append("<ul>")
        For Each Location As wpmLocation In Me.FindAll(AddressOf FindLocationByKeyword)
            FoundLocations.Append("<li><a href=""" & Location.DisplayURL & """>" & Location.PageName & "</a></li>")
        Next
        FoundLocations.Append("</ul><br/>")

        Return FoundLocations.ToString
    End Function

    Public Function FindLocation(ByVal PageID As String, ByVal ArticleID As String) As wpmLocation
        Dim FoundLocation As New wpmLocation
        SearchPageID = PageID
        SearchArticleID = ArticleID
        ' 
        '  Use the FindAll to get all Locations where the PageID is equal to search page id
        '
        If Me.Count = 1 Then
            '
            '  TO DO:  Handle situation when company only has one record (i.e. article)
            '
        End If

        For Each Location As wpmLocation In Me.FindAll(AddressOf FindLocationByPageID)
            FoundLocation.ModifiedDate = Location.ModifiedDate
            FoundLocation.ActiveFL = Location.ActiveFL
            FoundLocation.RecordSource = Location.RecordSource
            FoundLocation.PageID = Location.PageID
            FoundLocation.ParentPageID = Location.ParentPageID
            FoundLocation.ArticleID = Location.ArticleID
            FoundLocation.PageName = Location.PageName
            FoundLocation.PageFileName = Location.PageFileName
            FoundLocation.PageTitle = Location.PageTitle
            FoundLocation.PageKeywords = Location.PageKeywords
            FoundLocation.PageDescription = Location.PageDescription
            FoundLocation.PageFileName = Location.PageFileName
            FoundLocation.BreadCrumbHTML = Location.BreadCrumbHTML
            FoundLocation.LevelNBR = Location.LevelNBR
            FoundLocation.LocationTrailList = Location.LocationTrailList
            FoundLocation.PageTypeCD = Location.PageTypeCD
            FoundLocation.DisplayURL = Location.DisplayURL
            FoundLocation.TransferURL = Location.TransferURL
            FoundLocation.SiteCategoryID = Location.SiteCategoryID
            FoundLocation.SiteCategoryName = Location.SiteCategoryName
            FoundLocation.SiteCategoryGroupName = Location.SiteCategoryGroupName
            FoundLocation.SiteCategoryGroupID = Location.SiteCategoryGroupID
            If (Location.RecordSource = "Image" Or Location.RecordSource = "Article") Then
                FoundLocation.ArticleID = Location.ArticleID
                FoundLocation.PageName = Location.PageName
                FoundLocation.PageKeywords = Location.PageKeywords
                FoundLocation.PageDescription = Location.PageDescription
            End If
        Next

        Return FoundLocation
    End Function
    Private Function FindLocationByPageID(ByVal Location As wpmLocation) As Boolean
        If Location.PageID = SearchPageID AndAlso Location.ArticleID = SearchArticleID Then
            Return True
        Else
            Return False
        End If
    End Function
    Private Function FindLocationByKeyword(ByVal Location As wpmLocation) As Boolean
        If Location.PageKeywords.ToLower.Contains(SearchKeyword.ToLower) Then
            Return True
        Else
            Return False
        End If
    End Function
    Private Function FindCatalogLocations(ByVal Location As wpmLocation) As Boolean
        If Location.RecordSource = "Page" And Location.PageTypeCD = "CATALOG" Then
            Return True
        Else
            Return False
        End If
    End Function
End Class
Public Class wpmLocationTrail
    Private _PageID As String
    Public Property PageID() As String
        Get
            Return _PageID
        End Get
        Set(ByVal value As String)
            _PageID = value
        End Set
    End Property
    Private _PageName As String
    Public Property PageName() As String
        Get
            Return _PageName
        End Get
        Set(ByVal value As String)
            _PageName = value
        End Set
    End Property
    Private _PageDescription As String
    Public Property PageDescription() As String
        Get
            Return _PageDescription
        End Get
        Set(ByVal value As String)
            _PageDescription = value
        End Set
    End Property
    Private _ParentPageID As String
    Public Property ParentPageID() As String
        Get
            Return _ParentPageID
        End Get
        Set(ByVal value As String)
            _ParentPageID = value
        End Set
    End Property
    Private _DisplayURL As String
    Public Property DisplayURL() As String
        Get
            Return _DisplayURL
        End Get
        Set(ByVal value As String)
            _DisplayURL = value
        End Set
    End Property
    Private _LevelNBR As Integer
    Public Property LevelNBR() As Integer
        Get
            Return _LevelNBR
        End Get
        Set(ByVal value As Integer)
            _LevelNBR = value
        End Set
    End Property
    Private _MenuLevelNBR As Integer
    Public Property MenuLevelNBR() As Integer
        Get
            Return _MenuLevelNBR
        End Get
        Set(ByVal value As Integer)
            _MenuLevelNBR = value
        End Set
    End Property

End Class
Public Class wpmLocationTrailList
    Inherits List(Of wpmLocationTrail)
End Class
