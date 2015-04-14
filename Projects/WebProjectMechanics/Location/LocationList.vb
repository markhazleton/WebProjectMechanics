Imports System.Text

Public Class LocationList
    Inherits List(Of Location)

    Private _KeywordList As New LocationKeywordList
    Public Property KeywordList() As LocationKeywordList
        Get
            Return _KeywordList
        End Get
        Set(ByVal value As LocationKeywordList)
            _KeywordList = value
        End Set
    End Property
    Private SearchPageID As String
    Private SearchArticleID As Integer
    Private SearchKeyword As String
    Private SearchPageURL As String
    Public Sub New()

    End Sub
    Public Sub New(ByVal capacity As Integer)
        MyBase.New(capacity)

    End Sub
    Public Sub New(ByVal collection As IEnumerable(Of Location))
        MyBase.New(collection)

    End Sub
    Public Function GetLocationListFromDb(ByVal OrderBy As String, ByVal CompanyID As String, ByVal GroupID As String) As Boolean
        Dim bReturn As Boolean = True
        Dim iDefaultOrder As Integer = 0
        For Each myrow As DataRow In ApplicationDAL.GetSiteMap(OrderBy, CompanyID, GroupID).Rows
            Dim MySiteRow As New Location() With {.DefaultOrder = iDefaultOrder}
            iDefaultOrder = iDefaultOrder + 1
            MySiteRow.LocationID = wpm_GetDBString(myrow("PageID"))
            MySiteRow.LocationName = wpm_GetDBString(myrow("PageName"))
            MySiteRow.LocationTitle = wpm_GetDBString(myrow("PageTitle"))
            MySiteRow.ActiveFL = wpm_GetDBBoolean(myrow("Active"))
            MySiteRow.ArticleID = wpm_GetDBInteger(myrow("ArticleID"))
            MySiteRow.ModifiedDT = wpm_GetDBDate(myrow("ModifiedDT"))
            MySiteRow.LocationDescription = wpm_GetDBString(myrow("Description"))
            MySiteRow.LocationFileName = wpm_GetDBString(myrow("PageFileName"))
            MySiteRow.LocationKeywords = wpm_GetDBString(myrow("PageKeywords"))
            MySiteRow.ParentLocationID = wpm_GetDBString(myrow("ParentPageID"))
            MySiteRow.RecordSource = wpm_GetDBString(myrow("PageSource"))
            MySiteRow.SiteCategoryID = wpm_GetDBString(myrow("SiteCategoryID"))
            MySiteRow.SiteCategoryName = wpm_GetDBString(myrow("SitecategoryName"))
            MySiteRow.LocationGroupNM = wpm_GetDBString(myrow("SiteCategoryGroupName"))
            MySiteRow.LocationGroupID = wpm_GetDBString(myrow("SiteCategoryGroupID"))
            MySiteRow.LocationTypeCD = wpm_GetDBString(myrow("PageTypeCD"))
            MySiteRow.LocationTypeID = wpm_GetDBInteger(myrow("PageTypeID"))
            MySiteRow.GroupID = wpm_GetDBInteger(myrow("GroupID"))
            MySiteRow.SiteTypeID = wpm_GetDBString(myrow("SiteCategoryTypeID"))
            MySiteRow.TransferURL = wpm_GetDBString(myrow("TransferURL"))

            MySiteRow.LocationID = MySiteRow.SetLocationID()
            For Each myString As String In Split(MySiteRow.LocationKeywords, ",")
                myString = myString.Trim
                myString = myString.ToLower
                If (myString <> String.Empty) Then
                    KeywordList.AddToList(myString)
                End If
            Next
            Add(MySiteRow)
        Next
        KeywordList.Sort()
        If Count < 1 Then
            ' bReturn = False
        End If
        Return bReturn
    End Function
    Public Function BuildBreadCrumbRows() As Boolean
        Dim LevelCount As Integer = 1
        For Each myLocation As Location In Me
            If (myLocation.BreadCrumbURL = String.Empty Or myLocation.BreadCrumbURL = "") Then
                GenerateBreadcrumRow(myLocation, LevelCount)
            End If
            ' Reset Variables
            LevelCount = 1
        Next
        Return True
    End Function

    'The function to generate the breadcrumb for a row
    Private Function GenerateBreadcrumRow(ByVal myrow As Location, ByVal levelCount As Integer) As Boolean
        Dim sBreadCrumbHTML As String = String.Empty

        If (myrow.ParentLocationID.Trim) <> String.Empty Then
            BuildParentBreadcrumbs(myrow.LocationTrailList, myrow.ParentLocationID, levelCount)
        End If
        myrow.LocationTrailList.Add(GetBreadcrumbRow(myrow, levelCount))
        Dim myFullPathURL As New StringBuilder(String.Empty)
        For Each BCrow As LocationTrail In myrow.LocationTrailList
            If BCrow.LevelNBR < levelCount Then
                BCrow.MenuLevelNBR = (levelCount - BCrow.LevelNBR)
                If wpm_CheckForMatch(myrow.LocationTypeCD, "NO FILE NAME") Then
                    sBreadCrumbHTML = String.Format("<li><a href=""{0}"" title=""{1}"">{2}</a></li>{3}", myrow.LocationFileName, BCrow.Description, BCrow.Name, sBreadCrumbHTML)
                Else
                    sBreadCrumbHTML = String.Format("<li><a href=""{0}"" title=""{1}"">{2}</a></li>{3}", BCrow.DisplayURL, BCrow.Description, BCrow.Name, sBreadCrumbHTML)
                    myFullPathURL.Insert(0, "/" & wpm_FormatPageNameForURL(BCrow.Name))
                End If
            Else
                BCrow.MenuLevelNBR = levelCount
            End If
        Next
        myFullPathURL.Append(myrow.LocationURL)
        If (Left(myrow.LocationFileName.ToLower, 4)) <> "http" Then
            myrow.BreadCrumbURL = myFullPathURL.ToString
        Else
            myrow.BreadCrumbURL = myrow.LocationFileName
        End If
        sBreadCrumbHTML = String.Format("{0}<li><strong>{1}</strong></li>", sBreadCrumbHTML, myrow.LocationName)
        myrow.BreadCrumbHTML = String.Format("<ul>{0}</ul>", sBreadCrumbHTML)
        myrow.LevelNBR = levelCount

        Return True
    End Function
    Private Shared Function GetBreadcrumbRow(ByVal myrow As Location, ByVal LevelNBR As Integer) As LocationTrail
        Return New LocationTrail() With {.MenuLevelNBR = 0,
                                                  .LevelNBR = LevelNBR,
                                                  .LocationID = myrow.LocationID,
                                                  .Name = myrow.LocationName,
                                                  .ParentLocationID = myrow.ParentLocationID,
                                                  .DisplayURL = myrow.BreadCrumbURL,
                                                  .Description = myrow.LocationDescription}

    End Function
    Private Function BuildParentBreadcrumbs(ByRef BreadCrumbRows As List(Of LocationTrail), ByVal pageID As String, ByRef LevelCount As Integer) As Boolean
        For Each myrow As Location In Me
            If LevelCount < 10 Then
                If myrow.RecordSource = "Page" Or myrow.RecordSource = "Category" Then
                    If myrow.LocationID = pageID Then
                        'It turns out it's possible for a child's bread crumbs to be generated before it's parents.
                        'So, if I run across a parent that doesn't yet have a bread crumb, I generate it.
                        If (myrow.BreadCrumbURL = String.Empty Or myrow.BreadCrumbURL = "") Then
                            GenerateBreadcrumRow(myrow, LevelCount)
                        End If
                        BreadCrumbRows.Add(GetBreadcrumbRow(myrow, LevelCount))
                        LevelCount = LevelCount + 1
                        If (myrow.ParentLocationID.Trim) <> "" Then
                            BuildParentBreadcrumbs(BreadCrumbRows, myrow.ParentLocationID, LevelCount)
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
    Public Function SortByName() As Boolean
        Dim myDefaultComp As LocationNameCompare = New LocationNameCompare() With {.Direction = UtilitySortDirection.ASC}
        Sort(myDefaultComp)
        Return True
    End Function
    Public Function DefaultSort() As Boolean
        Dim myDefaultComp As LocationDefaultOrderCompare = New LocationDefaultOrderCompare() With {.Direction = UtilitySortDirection.ASC}
        Sort(myDefaultComp)
        Return True
    End Function
    Public Function DefaultSort(ByRef Direction As UtilitySortDirection) As Boolean
        Dim myDefaultComp As LocationDefaultOrderCompare = New LocationDefaultOrderCompare() With {.Direction = Direction}
        Sort(myDefaultComp)
        Return True
    End Function
    '
    '  Find A Location (Or List of Locations)
    '
    Public Function GetCatalogLocations() As List(Of Location)
        Return FindAll(AddressOf FindCatalogLocations)
    End Function

    Public Function FindLocationByKeyword(ByVal inSearchKeyword As String) As Location
        Dim FoundLocation As New Location
        SearchKeyword = inSearchKeyword
        For Each Location As Location In FindAll(AddressOf FindLocationByKeyword)
            FoundLocation.CopyLocation(Location)
        Next
        Return FoundLocation
    End Function
    Public Function GetLocationURLByKeyword(ByVal inSearchKeyword As String) As String
        Dim ReturnURL As String
        SearchKeyword = inSearchKeyword
        If FindAll(AddressOf FindLocationByKeyword).Count = 1 Then
            ReturnURL = Find(AddressOf FindLocationByKeyword).LocationURL
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
        For Each Location As Location In FindAll(AddressOf FindLocationByKeyword)
            FoundLocations.Append(String.Format("<li><a href=""{0}"">{1}</a></li>", Location.LocationURL, Location.LocationName))
        Next
        FoundLocations.Append("</ul><br/>")

        Return FoundLocations.ToString
    End Function


    Public Function FindLocation(ByVal PageURL As String) As Location
        Dim FoundLocation As New Location
        SearchPageURL = PageURL
        ' 
        '  Use the FindAll to get all Locations where the PageID is equal to search page id
        '
        If Count = 1 Then
            '
            '  TO DO:  Handle situation when company only has one record (i.e. article)
            '
        End If
        For Each thisLocation As Location In FindAll(AddressOf FindLocationByPageURL)
            FoundLocation.CopyLocation(thisLocation)
        Next

        Return FoundLocation
    End Function

    Public Function FindChildLocation(ByVal PageID As String) As List(Of Location)
        SearchArticleID = 0
        SearchPageID = PageID
        Return FindAll(AddressOf FindLocationByParentPageID)
    End Function

    Public Function FindTopMenuLocation() As List(Of Location)
        Return FindAll(AddressOf FindLocationForTopMenu)
    End Function

    Public Function FindLocation(ByVal PageID As String, ByVal ArticleID As Integer) As Location
        Dim FoundLocation As New Location
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

        For Each thisLocation As Location In FindAll(AddressOf FindLocationByPageID)
            FoundLocation.CopyLocation(thisLocation)
            Exit For
        Next

        If FoundLocation.ArticleID <> ArticleID Then
            SearchArticleID = 0
            For Each thisLocation As Location In FindAll(AddressOf FindLocationByPageID)
                FoundLocation.CopyLocation(thisLocation)
                Exit For
            Next
        End If



        Return FoundLocation
    End Function
    Private Function FindLocationByPageURL(ByVal Location As Location) As Boolean
        If Location.LocationURL = SearchPageURL Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function FindLocationByParentPageID(ByVal Location As Location) As Boolean
        If Location.RecordSource = "Category" Or Location.RecordSource = "Page" Then
            If Location.ParentLocationID = SearchPageID Then
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function
    Private Function FindLocationForTopMenu(ByVal checkLocation As Location) As Boolean
        If (checkLocation.RecordSource = "Page" Or checkLocation.RecordSource = "Category") And checkLocation.ParentLocationID = String.Empty Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function FindLocationByPageID(ByVal Location As Location) As Boolean
        If SearchArticleID < 1 Then
            If Location.LocationID = SearchPageID Then
                Return True
            Else
                Return False
            End If
        Else
            If Location.ParentLocationID = SearchPageID AndAlso Location.ArticleID = SearchArticleID Then
                Return True
            Else
                Return False
            End If
        End If

    End Function
    Private Function FindLocationByKeyword(ByVal Location As Location) As Boolean
        If Location.LocationKeywords.ToLower.Contains(SearchKeyword.ToLower) Then
            Return True
        Else
            Return False
        End If
    End Function
    Private Shared Function FindCatalogLocations(ByVal Location As Location) As Boolean
        If Location.RecordSource = "Page" And Location.LocationTypeCD = "CATALOG" Then
            Return True
        Else
            Return False
        End If
    End Function

#Region "Menu"

    Public Function BuildMenu(ByVal PageID As String, ByVal ParentPageID As String, ByVal DefaultParentPageID As String) As String
        Dim sReturn As New StringBuilder("")
        For Each myLocation As Location In Me
            If myLocation.RecordSource = "Page" And myLocation.LocationTypeCD IsNot "MODULE" Then
                If myLocation.ParentLocationID = ParentPageID Then
                    If myLocation.LocationID = PageID Then
                        sReturn.Append("<li class=""selected"">")
                        sReturn.Append(wpm_BuildNaviagtionLink(myLocation, True))
                    Else
                        sReturn.Append("<li>")
                        sReturn.Append(wpm_BuildNaviagtionLink(myLocation, False))
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
        For Each myLocation As Location In Me
            If myLocation.ActiveFL Then
                If (myLocation.RecordSource = "Page" Or myLocation.RecordSource = "Article") And myLocation.LocationTypeCD IsNot "MODULE" Then
                    If ParentID = myLocation.ParentLocationID Then
                        sReturn.Append("<li>")
                        sReturn.Append(wpm_BuildNaviagtionLink(myLocation, False))
                        sReturn.Append(String.Format("{0}</li>{1}", BuildPageTree(myLocation.LocationID, intLevel + 1, sULClassName), vbCrLf))
                    End If
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



#End Region

    Public Function GetRSS() As XDocument
        Dim outputxml = <?xml version="1.0" encoding="UTF-8"?>
                        <rss version="2.0">
                            <channel>
                                <title>Recipe Library</title>
                                <description>Recipe Library</description>
                                <link></link>
                                <lastBuildDate><%= Now.ToLongDateString %></lastBuildDate>
                                <pubDate><%= Now.ToLongDateString %></pubDate>
                                <ttl>1800</ttl>
                                <%= From i In Me Select <item>
                                                            <title><%= i.LocationTitle %></title>
                                                            <description><%= i.LocationDescription %></description>
                                                            <link><%= i.LocationURL %></link>
                                                            <author></author>
                                                            <guid isPermaLink="false"><%= Guid.NewGuid.ToString %></guid>
                                                            <pubDate><%= Now.ToLongDateString %></pubDate>
                                                        </item> %>
                            </channel>
                        </rss>
        Return outputxml
    End Function


End Class
