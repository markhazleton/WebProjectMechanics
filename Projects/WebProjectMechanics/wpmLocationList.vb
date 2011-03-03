Imports System.Text

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
    Private SearchPageURL As String
    ' Private SearchRecordSource As String
    Public Sub New()
        
    End Sub
    Public Sub New(ByVal capacity As Integer)
        MyBase.New(capacity)
        
    End Sub
    Public Sub New(ByVal collection As IEnumerable(Of wpmLocation))
        MyBase.New(collection)
        
    End Sub


    Public Function GetLocationListFromDb(ByVal OrderBy As String, ByVal CompanyID As String, ByVal GroupID As String) As Boolean
        Dim bReturn As Boolean = True
        Dim sTransferURL As String = ("")
        Dim sTransferParms As String = ("")
        Dim iDefaultOrder As Integer = 0
        For Each myrow As DataRow In wpmDataCon.GetSiteMap(OrderBy, CompanyID, GroupID).Rows
            Dim MySiteRow As New wpmLocation() With {.DefaultOrder = iDefaultOrder}
            iDefaultOrder = iDefaultOrder + 1
            MySiteRow.PageID = wpmUtil.GetDBString(myrow("PageID"))
            MySiteRow.PageName = wpmUtil.GetDBString(myrow("PageName"))
            MySiteRow.PageTitle = wpmUtil.GetDBString(myrow("PageTitle"))
            MySiteRow.ActiveFL = wpmUtil.GetDBBoolean(myrow("Active"))
            MySiteRow.ArticleID = wpmUtil.GetDBString(myrow("ArticleID"))
            MySiteRow.ModifiedDate = wpmUtil.GetDBDate(myrow("ModifiedDT"))
            MySiteRow.PageDescription = wpmUtil.GetDBString(myrow("Description"))
            MySiteRow.PageFileName = wpmUtil.GetDBString(myrow("PageFileName"))
            MySiteRow.PageKeywords = wpmUtil.GetDBString(myrow("PageKeywords"))
            MySiteRow.ParentPageID = wpmUtil.GetDBString(myrow("ParentPageID"))
            MySiteRow.RecordSource = wpmUtil.GetDBString(myrow("PageSource"))
            sTransferURL = wpmUtil.GetDBString(myrow("TransferURL"))
            MySiteRow.SiteCategoryID = wpmUtil.GetDBString(myrow("SiteCategoryID"))
            MySiteRow.SiteCategoryName = wpmUtil.GetDBString(myrow("SitecategoryName"))
            MySiteRow.SiteCategoryGroupName = wpmUtil.GetDBString(myrow("SiteCategoryGroupName"))
            MySiteRow.SiteCategoryGroupID = wpmUtil.GetDBString(myrow("SiteCategoryGroupID"))
            MySiteRow.PageTypeCD = wpmUtil.GetDBString(myrow("PageTypeCD"))
            If (sTransferURL.Trim) = String.Empty Then
                sTransferURL = MySiteRow.PageFileName
            End If

            ' Check for Recursive Parent
            If (MySiteRow.PageID = MySiteRow.ParentPageID And MySiteRow.ParentPageID <> "") Then
                MySiteRow.ParentPageID = ""
                wpmLogging.AuditLog(String.Format("PageID=ParentPageID ({0} - {1}) ", MySiteRow.PageName, MySiteRow.PageID), "wpmSiteMapRow.PopulateSiteMapCol")
            End If

            Select Case MySiteRow.RecordSource
                Case "Page"
                    If MySiteRow.ArticleID <> "" Then
                        sTransferParms = String.Format("{0}&a={1}", String.Format("c={0}", MySiteRow.PageID), MySiteRow.ArticleID)
                    Else
                        sTransferParms = String.Format("c={0}", MySiteRow.PageID)
                    End If
                Case "Article"
                    If MySiteRow.PageID <> "" Then
                        sTransferParms = String.Format("{0}&c={1}", String.Format("a={0}", MySiteRow.ArticleID), MySiteRow.PageID)
                    Else
                        sTransferParms = String.Format("a={0}", MySiteRow.ArticleID)
                    End If
                Case "Image"
                    If MySiteRow.PageID <> "" Then
                        sTransferParms = String.Format("{0}&c={1}", String.Format("i={0}", MySiteRow.ArticleID), MySiteRow.PageID)
                    Else
                        sTransferParms = String.Format("i={0}", MySiteRow.ArticleID)
                    End If
                Case "Category"
                    MySiteRow.PageID = "CAT-" & MySiteRow.PageID
                    If MySiteRow.ParentPageID <> "" Then
                        MySiteRow.ParentPageID = "CAT-" & MySiteRow.ParentPageID
                    End If
                    sTransferParms = "c=" & MySiteRow.PageID
                Case Else
                    sTransferParms = String.Format("c={0}&rc={1}", MySiteRow.PageID, MySiteRow.RecordSource)
            End Select

            If (MySiteRow.PageFileName.Trim) <> String.Empty Then
                sTransferURL = MySiteRow.PageFileName
                MySiteRow.PageTypeCD = "NOFILENAME"
            ElseIf sTransferURL.IndexOf("?") > 0 Then
                If InStr(sTransferURL, "c=") = 0 Then
                    sTransferURL = String.Format("{0}&{1}", sTransferURL, sTransferParms)
                End If
            Else
                sTransferURL = String.Format("{0}?{1}", sTransferURL, sTransferParms)
            End If

            If MySiteRow.PageTypeCD = "NO FILE NAME" Then
                If MySiteRow.PageFileName.IndexOf("?") > 0 Then
                    If InStr(MySiteRow.PageFileName, "c=") = 0 Then
                        MySiteRow.PageFileName = String.Format("{0}&{1}", MySiteRow.PageFileName, sTransferParms)
                    End If
                Else
                    MySiteRow.PageFileName = String.Format("{0}?{1}", MySiteRow.PageFileName, sTransferParms)
                End If
                MySiteRow.TransferURL = MySiteRow.PageFileName
                MySiteRow.DisplayURL = MySiteRow.PageFileName
                MySiteRow.BreadCrumbHTML = MySiteRow.PageFileName
            Else
                MySiteRow.TransferURL = sTransferURL
                sTransferURL = ""
                sTransferParms = ""
                MySiteRow.DisplayURL = wpmUtil.FormatPageNameURL(MySiteRow.PageName)
            End If
            If MySiteRow.RecordSource = "Page" And MySiteRow.ParentPageID = String.Empty And MySiteRow.SiteCategoryID <> String.Empty Then
                MySiteRow.ParentPageID = "CAT-" & MySiteRow.SiteCategoryID
            End If
            For Each myString As String In Split(MySiteRow.PageKeywords, ",")
                myString = myString.Trim
                myString = myString.ToLower
                If (myString <> String.Empty) Then
                    KeywordList.AddToList(myString)
                End If
            Next
            Add(MySiteRow)
        Next
        KeywordList.Sort()
        Return bReturn
    End Function
    Public Function BuildBreadCrumbRows() As Boolean
        Dim LevelCount As Integer = 1
        For Each myRow As wpmLocation In Me
            If (myRow.BreadCrumbURL = String.Empty Or myRow.BreadCrumbURL = "") Then
                GenerateBreadcrumRow(myRow, LevelCount)
            End If
            ' Reset Variables
            LevelCount = 1
        Next
        Return True
    End Function
    Public Function BuildBreadcrumbRows(ByVal CompanyName As String) As Boolean
        Return BuildBreadcrumbRows()
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
                sBreadCrumbHTML = String.Format("<li><a href=""{0}"" title=""{1}"">{2}</a></li>{3}", BCrow.DisplayURL, BCrow.PageDescription, BCrow.PageName, sBreadCrumbHTML)
                myFullPathURL.Insert(0, "/" & wpmUtil.FormatPageNameForURL(BCrow.PageName))
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
        sBreadCrumbHTML = String.Format("{0}<li><strong>{1}</strong></li>", sBreadCrumbHTML, myrow.PageName)
        myrow.BreadCrumbHTML = String.Format("<ul>{0}</ul>", sBreadCrumbHTML)
        myrow.LevelNBR = levelCount

        Return True
    End Function
    Private Shared Function GetBreadcrumbRow(ByVal myrow As wpmLocation, ByVal LevelNBR As Integer) As wpmLocationTrail
        Return New wpmLocationTrail() With {.MenuLevelNBR = 0,
                                                  .LevelNBR = LevelNBR,
                                                  .PageID = myrow.PageID,
                                                  .PageName = myrow.PageName,
                                                  .ParentPageID = myrow.ParentPageID,
                                                  .DisplayURL = myrow.BreadCrumbURL,
                                                  .PageDescription = myrow.PageDescription}

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
        Dim myDefaultComp As LocationDefaultOrderCompare = New LocationDefaultOrderCompare() With {.Direction = wpmSortDirection.ASC}
        Sort(myDefaultComp)
        Return True
    End Function
    Public Function DefaultSort(ByRef Direction As wpmSortDirection) As Boolean
        Dim myDefaultComp As LocationDefaultOrderCompare = New LocationDefaultOrderCompare() With {.Direction = Direction}
        Sort(myDefaultComp)
        Return True
    End Function
    '
    '  Find A Location (Or List of Locations)
    '
    Public Function GetCatalogLocations() As List(Of wpmLocation)
        Return FindAll(AddressOf FindCatalogLocations)
    End Function

    Public Function FindLocationByKeyword(ByVal inSearchKeyword As String) As wpmLocation
        Dim FoundLocation As New wpmLocation
        SearchKeyword = inSearchKeyword
        For Each Location As wpmLocation In FindAll(AddressOf FindLocationByKeyword)
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
        Dim ReturnURL As String
        SearchKeyword = inSearchKeyword
        If FindAll(AddressOf FindLocationByKeyword).Count = 1 Then
            ReturnURL = Find(AddressOf FindLocationByKeyword).DisplayURL
        Else
            ReturnURL = String.Empty
        End If
        Return ReturnURL
    End Function

    Public Function FindLocationsByKeyword(ByVal inSearchKeyword As String) As String
        Dim FoundLocations As New StringBuilder(String.Empty)
        SearchKeyword = inSearchKeyword
        FoundLocations.Append(String.Format("<br/>Search Results for <strong>{0} </strong><br/>", inSearchKeyword))
        FoundLocations.Append("<ul>")
        For Each Location As wpmLocation In FindAll(AddressOf FindLocationByKeyword)
            FoundLocations.Append(String.Format("<li><a href=""{0}"">{1}</a></li>", Location.DisplayURL, Location.PageName))
        Next
        FoundLocations.Append("</ul><br/>")

        Return FoundLocations.ToString
    End Function


    Public Function FindLocation(ByVal PageURL As String) As wpmLocation
        Dim FoundLocation As New wpmLocation
        SearchPageURL = PageURL
        ' 
        '  Use the FindAll to get all Locations where the PageID is equal to search page id
        '
        If Count = 1 Then
            '
            '  TO DO:  Handle situation when company only has one record (i.e. article)
            '
        End If

        For Each Location As wpmLocation In FindAll(AddressOf FindLocationByPageURL)
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

    Public Function FindLocation(ByVal PageID As String, ByVal ArticleID As String) As wpmLocation
        Dim FoundLocation As New wpmLocation
        SearchPageID = PageID
        SearchArticleID = ArticleID
        ' 
        '  Use the FindAll to get all Locations where the PageID is equal to search page id
        '
        If Count = 1 Then
            '
            '  TO DO:  Handle situation when company only has one record (i.e. article)
            '
        End If

        For Each Location As wpmLocation In FindAll(AddressOf FindLocationByPageID)
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
            Exit For
        Next
        Return FoundLocation
    End Function
    Private Function FindLocationByPageURL(ByVal Location As wpmLocation) As Boolean
        If Location.DisplayURL = SearchPageURL Then
            Return True
        Else
            Return False
        End If
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
    Private Shared Function FindCatalogLocations(ByVal Location As wpmLocation) As Boolean
        If Location.RecordSource = "Page" And Location.PageTypeCD = "CATALOG" Then
            Return True
        Else
            Return False
        End If
    End Function

#Region "Menu"

    Public Function BuildMenu(ByVal PageID As String, ByVal ParentPageID As String, ByVal DefaultParentPageID As String) As String
        Dim sReturn As New StringBuilder("")
        For Each myrow As wpmLocation In Me
            If myrow.RecordSource = "Page" Then
                If myrow.ParentPageID = ParentPageID Then
                    If myrow.PageID = PageID Then
                        sReturn.Append("<li class=""selected"">")
                        sReturn.Append(BuildNaviagtionLink(myrow, True))
                    Else
                        sReturn.Append("<li>")
                        sReturn.Append(BuildNaviagtionLink(myrow, False))
                    End If
                    sReturn.Append("</li>" & vbCrLf)
                End If
            End If
        Next
        If sReturn.Length > 0 Then
            sReturn.Insert(0, String.Format("<ul class=""BuildMenu"">{0}", vbCrLf))
            sReturn.Append("</ul>" & vbCrLf)
        Else
            If ParentPageID <> DefaultParentPageID And DefaultParentPageID <> "" Then
                sReturn.Append(BuildMenu(PageID, DefaultParentPageID, DefaultParentPageID))
            End If
        End If
        Return sReturn.ToString
    End Function

    Public Function BuildPageTree(ByVal ParentID As String, ByVal intLevel As Double, ByVal sULClassName As String) As String
        Dim sReturn As New StringBuilder("")
        For Each myRow As wpmLocation In Me
            If myRow.RecordSource = "Page" Then
                If ParentID = myRow.ParentPageID Then
                    sReturn.Append("<li>")
                    sReturn.Append(BuildNaviagtionLink(myRow, False))
                    sReturn.Append(String.Format("{0}</li>{1}", BuildPageTree(myRow.PageID, intLevel + 1, sULClassName), vbCrLf))
                End If
            End If
        Next
        If (sReturn.Length > 0) Then
            If intLevel = 0 Then
                sReturn.Insert(0, String.Format("<ul class=""{0}"">{1}", sULClassName, vbCrLf))
            Else
                sReturn.Insert(0, "<ul>" & vbCrLf)
            End If
            sReturn.Append("</ul>" & vbCrLf)
        End If
        Return sReturn.ToString
    End Function



    Private Shared Function BuildClassLink(ByRef myRow As wpmLocation, ByVal LinkClass As String) As String
        If LinkClass = String.Empty Then
            Return (String.Format("<a href=""{0}""><span>{1}</span></a>", myRow.GetSiteMapRowURL(False), myRow.PageName))
        Else
            Return (String.Format("<a class=""{0}"" href=""{1}""><span>{2}</span></a>", LinkClass, myRow.GetSiteMapRowURL(False), myRow.PageName))
        End If
    End Function
    Private Shared Function BuildNaviagtionLink(ByRef myRow As wpmLocation, ByVal IsSelected As Boolean) As String
        If (IsSelected) Then
            Return BuildClassLink(myRow, "selected")
        Else
            Return BuildClassLink(myRow, "")
        End If
    End Function


#End Region

End Class
