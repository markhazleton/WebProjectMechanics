Imports System.Text


Public Class ArticleList
    Inherits List(Of Article)
    Private Property SearchArticleID As Integer
    Public Property curLocationID As String
    Public Property curArticleID As Integer
    Public Property curRecordsPerLocation As Integer = 3
    Public Property CompanyDefaultArticleID As Integer

    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal capacity As Integer)
        MyBase.New(capacity)

    End Sub
    Public Sub New(ByVal collection As IEnumerable(Of Article))
        MyBase.New(collection)
    End Sub
    Sub New(ByRef myActiveCompany As ActiveCompany)
        curLocationID = myActiveCompany.CurrentLocationID()
        curArticleID = wpm_CurrentArticleID
        CompanyDefaultArticleID = myActiveCompany.DefaultArticleID
        PopulatePageArticleList(myActiveCompany.CurrentLocationID())
        If Count > 0 Then
            curRecordsPerLocation = Item(0).RowsPerPage
        Else
            curRecordsPerLocation = 999
        End If
    End Sub


    Public Function PopulateCompanyArticleList(ByVal CompanyID As String) As Boolean
        Dim sSQL As String = ""
        If Not wpm_IsAdmin Then
            sSQL = (String.Format("SELECT [Article].[ArticleID],[Article].[title],[Article].[Description],[Article].[ArticleSummary],[Article].[ModifiedDT],[Article].[ContactID],[Article].[Author],[Article].[ArticleBody],[Article].[active],[Article].[PageID], [Company].[CompanyName] FROM [Article],[Company] where [Article].[CompanyID]=[Company].[CompanyID] and [Company].[CompanyID]={0} ", CompanyID)) & " and [Article].[Active]= TRUE "
        Else
            sSQL = (String.Format("SELECT [Article].[ArticleID],[Article].[title],[Article].[Description],[Article].[ArticleSummary],[Article].[ModifiedDT],[Article].[ContactID],[Article].[Author],[Article].[ArticleBody],[Article].[active],[Article].[PageID], [Company].[CompanyName] FROM [Article],[Company] where [Article].[CompanyID]=[Company].[CompanyID] and [Company].[CompanyID]={0} ", CompanyID))
        End If
        Clear()
        sSQL = sSQL & "order by [Article].[StartDT] desc "
        For Each row As DataRow In wpm_GetDataTable(sSQL, "PopulateArticleList").Rows
            Dim myArticleRow As New Article
            myArticleRow.ArticleID = wpm_GetDBInteger(row.Item("ArticleID"))
            myArticleRow.ArticleBody = wpm_GetDBString(row.Item("ArticleBody"))
            myArticleRow.ArticleModDate = wpm_GetDBDate(row.Item("ModifiedDT"))
            myArticleRow.ArticleSummary = wpm_GetDBString(row.Item("ArticleSummary"))
            myArticleRow.ArticleDescription = wpm_GetDBString(row.Item("Description"))
            myArticleRow.ArticleName = wpm_GetDBString(row.Item("title"))
            myArticleRow.IsArticleActive = wpm_GetDBBoolean(row.Item("active"))
            myArticleRow.ArticlePageID = wpm_GetDBString(row.Item("PageID"))
            myArticleRow.ArticleAuthor = wpm_GetDBString(row.Item("Author"))
            myArticleRow.IsArticleDefault = False
            myArticleRow.ArticleURL = GetArticleURL(myArticleRow)

            myArticleRow.PageName = myArticleRow.ArticleName
            myArticleRow.ArticleAdmin = GetArticleAdmin(myArticleRow.ArticleID)
            Add(myArticleRow)
        Next
        Return True
    End Function
    Private Function GetArticleURL(ByVal myarticle As Article) As String
        If wpm_SiteConfig.Use404Processing Then
            Return wpm_FormatNameForURL(String.Format("{0}/{1}", myarticle.PageName, myarticle.ArticleName))
        Else
            Return String.Format("/default.aspx?c={0}&a={1}", curLocationID, myarticle.ArticleID)
        End If
    End Function

    Private Function PopulatePageArticleList(ByVal PageID As String) As Boolean
        Dim sSQL As String
        If Not wpm_IsAdmin Then
            sSQL = (String.Format("SELECT [Article].[ArticleID],[Article].[title],[Article].[Description],[Article].[ArticleSummary],[Article].[ModifiedDT],[Contact].[PrimaryContact],[Article].[ArticleBody],[Article].[active],[Article].[Author],[Page].[PageID], [Page].[PageName], [Page].[PageDescription], [Page].[PageKeywords], [Page].[RowsPerPage] FROM (Article INNER JOIN Page ON Article.PageID = Page.PageID) LEFT JOIN Contact ON Article.ContactID = Contact.ContactID where [Page].[PageID]={0} ", PageID)) & " and [Article].[Active]= TRUE "
        Else
            sSQL = (String.Format("SELECT [Article].[ArticleID],[Article].[title],[Article].[Description],[Article].[ArticleSummary],[Article].[ModifiedDT],[Contact].[PrimaryContact],[Article].[ArticleBody],[Article].[active],[Article].[Author],[Page].[PageID], [Page].[PageName], [Page].[PageDescription], [Page].[PageKeywords], [Page].[RowsPerPage] FROM (Article INNER JOIN Page ON Article.PageID = Page.PageID) LEFT JOIN Contact ON Article.ContactID = Contact.ContactID where [Page].[PageID]={0} ", PageID))
        End If
        sSQL = sSQL & "order by [Article].[StartDT] desc "
        Clear()
        For Each row As DataRow In wpm_GetDataTable(sSQL, "PopulateArticleList").Rows
            Dim myArticleRow As New Article
            myArticleRow.ArticleID = wpm_GetDBInteger(row.Item("ArticleID"))
            myArticleRow.ArticleBody = wpm_GetDBString(row.Item("ArticleBody"))
            myArticleRow.ArticleSummary = wpm_GetDBString(row.Item("ArticleSummary"))
            myArticleRow.ArticleModDate = wpm_GetDBDate(row.Item("ModifiedDT"))
            myArticleRow.ArticleDescription = wpm_GetDBString(row.Item("Description"))
            myArticleRow.ArticleName = wpm_GetDBString(row.Item("title"))
            myArticleRow.IsArticleActive = wpm_GetDBBoolean(row.Item("active"))
            myArticleRow.ArticlePageID = wpm_GetDBString(row.Item("PageID"))
            myArticleRow.IsArticleDefault = False
            myArticleRow.PageName = wpm_GetDBString(row.Item("PageName"))
            myArticleRow.RowsPerPage = wpm_GetDBInteger(row.Item("RowsPerPage"))
            myArticleRow.ArticleAuthor = wpm_GetDBString(row.Item("Author"))
            myArticleRow.ArticleAdmin = GetArticleAdmin(myArticleRow.ArticleID)
            myArticleRow.ArticleURL = GetArticleURL(myArticleRow)
            Add(myArticleRow)
        Next
        Return True
    End Function

    Private Shared Function GetArticleAdmin(ByVal ArticleID As Integer) As String
        If wpm_IsAdmin Then
            Return (String.Format("<br /><br /><hr /><a href=""{0}/maint/default.aspx?type=Article&ArticleID={1}""> Edit Article </a>", wpm_SiteConfig.AdminFolder, ArticleID))
        Else
            Return ""
        End If
    End Function

    Public Function GetBlogPosts(ByRef sbBlogTemplate As StringBuilder) As String
        Dim sItem As New StringBuilder
        If (curArticleID < 1) Or (curArticleID = CompanyDefaultArticleID) Then
            sItem.Append("<div class=""blog"">")
            sItem.Append("<div class=""blog-posts"">")
            For Each myArticle As Article In Me
                ' Build The Blog Entry
                AddArticleWithParm(myArticle, sItem, sbBlogTemplate)
            Next
            sItem.Append("</div>")
            sItem.Append("</div>")
        Else
            For Each myArticle As Article In Me
                If myArticle.ArticleID = curArticleID Then
                    AddArticleWithParm(myArticle, sItem, sbBlogTemplate)
                    Exit For
                End If
            Next
        End If
        Return sItem.ToString
    End Function

    Public Function BuildBlogList(ByRef sbBlogTemplate As StringBuilder, ByVal iCurrentPageNumber As Integer) As String
        Dim sItem As New StringBuilder
        Dim sURL As String = (String.Format("/default.aspx?c={0}&amp;a={1}", curLocationID, curArticleID))
        Dim sSubPageNav As String = ("")
        Dim iPageCount As Integer = 0
        Dim iFirstDisplay As Integer = 0
        Dim iLastDisplay As Integer = 0
        Dim iPageNumber As Integer = 0

        Dim iRow As Integer = 0
        If (curArticleID <1) Or (curArticleID = CompanyDefaultArticleID) Then

            If Count = 0 Then
                ' No Rows
            Else
                ' Count number of Images for this PageID, SETUP FOR REMAINDER OF TASKS
                ' determine page break
                If (curRecordsPerLocation < 1) Then
                    curRecordsPerLocation = 1
                End If
                iPageCount = CInt(Math.Round(Val(Count / curRecordsPerLocation), 0))
                ' If last image is on a page break, then subtract one page to avoid an empty page
                If (iPageCount * curRecordsPerLocation) > Count Then
                    ' Do nothing we have the right number of pages
                Else
                    If (Count Mod curRecordsPerLocation) = 0 Then
                    Else
                        iPageCount = iPageCount + 1
                    End If
                End If
                ' Determine First and Last record to display
                If Count > 0 Then
                    If iCurrentPageNumber <= 1 Then
                        ' We are on the first page
                        iPageNumber = 1
                        iFirstDisplay = 0
                        iLastDisplay = curRecordsPerLocation - 1
                    Else
                        iPageNumber = iCurrentPageNumber
                        iFirstDisplay = ((iPageNumber * curRecordsPerLocation) - curRecordsPerLocation)
                        iLastDisplay = iFirstDisplay + curRecordsPerLocation - 1
                    End If
                End If
                ' Build Sub Page Navigation
                If iPageNumber > 1 Then
                    sSubPageNav = String.Format("{0}<a title=""PREVIOUS"" href=""{1}&amp;Page={2}""><::</a>", sSubPageNav, sURL, iPageNumber - 1)
                End If
                If (iPageCount > 1) Then
                    sSubPageNav = String.Format("{0}<b>Page {1} of {2}</b>", sSubPageNav, iPageNumber, iPageCount)
                End If
                If iPageNumber < iPageCount Then
                    sSubPageNav = sSubPageNav & "<a title=""NEXT"" href=""" & sURL & "&amp;Page=" & iPageNumber + 1 & """>::></a>"
                End If
                If sSubPageNav <> "" Then
                    sItem.Append(String.Format("<center>{0}</center>{1}{1}", sSubPageNav, vbCrLf))
                End If
                ' Draw The current page

                For Each myArticle As Article In Me
                    ' Determine if this record is displayed
                    If (iRow >= iFirstDisplay) Then
                        If (iRow <= iLastDisplay) Then
                            AddArticleWithParm(myArticle, sItem, sbBlogTemplate)
                        End If
                    End If
                    iRow = iRow + 1
                Next
            End If
            sItem.Append(String.Format("<center>{0}</center>{1}{1}", sSubPageNav, vbCrLf))
            sItem.Append(vbCrLf)
        Else
            For Each myArticle As Article In Me
                If myArticle.ArticleID = curArticleID Then
                    AddArticleWithParm(myArticle, sItem, sbBlogTemplate)
                    Exit For
                End If
            Next
        End If
        Return sItem.ToString
    End Function

    Private Function AddArticleWithParm(ByVal myArticle As Article, ByRef sItem As StringBuilder, ByRef sbBlogTemplate As StringBuilder) As Boolean
        sItem.Append(sbBlogTemplate.ToString)
        sItem.Replace("~~PostID~~", myArticle.ArticleID.ToString)
        sItem.Replace("~~PostDate~~", myArticle.ArticleModDate.ToString("f"))
        sItem.Replace("~~PostShortDate~~", myArticle.ArticleModDate.ToString("m"))
        sItem.Replace("~~PostURL~~", myArticle.ArticleURL)
        sItem.Replace("~~PostName~~", myArticle.ArticleName)
        sItem.Replace("~~PostTitle~~", myArticle.ArticleName)
        sItem.Replace("~~PostBody~~", myArticle.ArticleBody)
        sItem.Replace("~~PostSummary~~", myArticle.ArticleSummary)
        sItem.Replace("~~PostDescription~~", myArticle.ArticleDescription)
        sItem.Replace("~~BlogPageURL~~", "/default.aspx?c=" & curLocationID)
        sItem.Replace("~~BlogPageName~~", myArticle.PageName)
        sItem.Replace("~~PostAuthor~~", myArticle.ArticleAuthor)
        sItem.Replace("~~ArticleAdmin~~", myArticle.ArticleAdmin)
        Return True
    End Function


    Public Function FindArticle(ByVal reqArticleID As Integer) As Article
        Dim FoundArticle As New Article
        SearchArticleID = reqArticleID
        ' 
        '  Use the FindAll to get all Locations where the PageID is equal to search page id
        '
        If Count = 1 Then
            '
            '  TO DO:  Handle situation when company only has one record (i.e. article)
            '
        End If
        For Each thisArticle As Article In FindAll(AddressOf FindArticleByArticleID)
            FoundArticle.CopyArticle(thisArticle)
        Next

        Return FoundArticle
    End Function
    Private Function FindArticleByArticleID(ByVal thisArticle As Article) As Boolean
        If thisArticle.ArticleID = SearchArticleID Then
            Return True
        Else
            Return False
        End If
    End Function

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
                                                            <title><%= i.ArticleName %></title>
                                                            <description><%= i.ArticleDescription %></description>
                                                            <link><%= i.ArticleURL %></link>
                                                            <author></author>
                                                            <guid isPermaLink="false"><%= Guid.NewGuid.ToString %></guid>
                                                            <pubDate><%= Now.ToLongDateString %></pubDate>
                                                        </item> %>
                            </channel>
                        </rss>
        Return outputxml
    End Function






End Class
