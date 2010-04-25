Imports System.Data.OleDb
Imports System.Xml.Serialization
Imports System.Web

Public Class wpmArticle
    Private _articleURL As String
    Public Property ArticleURL() As String
        Get
            Return _articleURL
        End Get
        Set(ByVal Value As String)
            _articleURL = value
        End Set
    End Property

    Private _articleID As String
    Public Property ArticleID() As String
        Get
            Return _articleID
        End Get
        Set(ByVal Value As String)
            _articleID = Value
        End Set
    End Property

    Private _articleName As String
    Public Property ArticleName() As String
        Get
            Return _articleName
        End Get
        Set(ByVal Value As String)
            _articleName = Value
        End Set
    End Property
    Private _articleSummary As String
    Public Property ArticleSummary() As String
        Get
            Return _articleSummary
        End Get
        Set(ByVal value As String)
            _articleSummary = value
        End Set
    End Property
    Private _articleDescription As String
    Public Property ArticleDescription() As String
        Get
            Return _articleDescription
        End Get
        Set(ByVal Value As String)
            _articleDescription = Value
        End Set
    End Property

    Private _articleBody As String
    Public Property ArticleBody() As String
        Get
            Return _articleBody
        End Get
        Set(ByVal Value As String)
            _articleBody = Value
        End Set
    End Property

    Private _articleModDate As Date
    Public Property ArticleModDate() As Date
        Get
            Return _articleModDate
        End Get
        Set(ByVal Value As Date)
            _articleModDate = Value
        End Set
    End Property

    Private _articlePageID As String
    Public Property ArticlePageID() As String
        Get
            Return _articlePageID
        End Get
        Set(ByVal Value As String)
            _articlePageID = Value
        End Set
    End Property
    Private _isArticleDefault As Boolean
    Public Property IsArticleDefault() As Boolean
        Get
            Return _isArticleDefault
        End Get
        Set(ByVal Value As Boolean)
            _isArticleDefault = Value
        End Set
    End Property
    Private _pageName As String
    Public Property PageName() As String
        Get
            Return _pageName
        End Get
        Set(ByVal Value As String)
            _pageName = Value
        End Set
    End Property

    Private _isArticleActive As Boolean
    Public Property IsArticleActive() As Boolean
        Get
            Return _isArticleActive
        End Get
        Set(ByVal Value As Boolean)
            _isArticleActive = Value
        End Set
    End Property
    Private _rowsPerPage As Integer
    Public Property RowsPerPage() As Integer
        Get
            Return _rowsPerPage
        End Get
        Set(ByVal Value As Integer)
            _rowsPerPage = Value
        End Set
    End Property

    Private _articleAuthor As String
    Public Property ArticleAuthor() As String
        Get
            Return _articleAuthor
        End Get
        Set(ByVal Value As String)
            _articleAuthor = Value
        End Set
    End Property
    Private _articleAdmin As String
    Public Property ArticleAdmin() As String
        Get
            Return _articleAdmin
        End Get
        Set(ByVal Value As String)
            _articleAdmin = Value
        End Set
    End Property

    Private _contactID As String
    Public Property ContactID() As String
        Get
            Return _contactID
        End Get
        Set(ByVal Value As String)
            _contactID = Value
        End Set
    End Property
    Public Sub New()
        ' Blank Article
    End Sub
    Public Sub New(ByRef CurrentMapRow As wpmLocation, ByVal DefaultArticleID As String)
        Select Case CurrentMapRow.RecordSource
            Case "Page"
                SetArticleByPageID(CurrentMapRow.PageID, CurrentMapRow.ArticleID, DefaultArticleID)
            Case "Article"
                SetArticleByArticleID(CurrentMapRow.ArticleID)
            Case Else
                If CurrentMapRow.ArticleID = "" Then
                    If DefaultArticleID <> "" Then
                        SetArticleByArticleID(DefaultArticleID)
                    Else
                        Me.ArticleBody = " "
                    End If
                Else
                    Me.ArticleBody = " "
                End If
        End Select
    End Sub
    Public Sub New(ByVal reqArticleID As String, ByVal reqPageID As String, ByVal DefaultArticleID As String)
        MyBase.New()
        Me.IsArticleDefault = False

        If reqPageID = "" Then
            If reqArticleID = "" Then
                GetDefaultArticle(DefaultArticleID)
            Else
                Me.ArticleID = reqArticleID
                If Not SetArticleByArticleID(reqArticleID) Then
                    GetDefaultArticle(DefaultArticleID)
                End If
            End If
        Else
            If Not SetArticleByPageID(reqPageID, reqArticleID, DefaultArticleID) Then
                If IsNothing(DefaultArticleID) Then
                    Me.ArticleBody = "<h1>NO DEFAULT ARTICLE</h1>"
                Else
                    GetDefaultArticle(DefaultArticleID)
                End If
            End If
        End If
    End Sub
    Public Sub New(ByVal reqArticleID As String)
        MyBase.New()
        GetArticleByArticleID(reqArticleID)

    End Sub

    Private Function GetDefaultArticle(ByVal SiteArticleID As String) As Boolean
        Me.ArticleID = SiteArticleID
        Me.IsArticleDefault = True
        If Me.ArticleID = "" Then
            Return False
        Else
            Return SetArticleByArticleID(ArticleID)
        End If
    End Function

    Private Function GetArticleByArticleID(ByVal reqArticleID As String) As Boolean
        Dim bFunctionStatus As Boolean = False
        Dim artSQL As String = ("SELECT article.ArticleID, article.Title, article.Description, article.ArticleBody, article.StartDT, page.pageid, page.pagename, article.active, article.contactID FROM article LEFT JOIN page ON article.PageID = page.PageID WHERE [article].[ArticleID]=" & reqArticleID)
        If Trim(reqArticleID) = "" Then
            bFunctionStatus = False
        Else
            Try
                For Each row As DataRow In wpmDB.GetDataTable(artSQL, "mhArticle.SetArticleByArticleID").Rows
                    bFunctionStatus = True
                    Me.ArticleID = wpmUTIL.GetDBString(row.Item("ArticleID"))
                    Me.ArticleBody = wpmUTIL.GetDBString(row.Item("ArticleBody"))
                    Me.ArticleName = wpmUTIL.GetDBString(row.Item("Title"))
                    Me.ArticleDescription = wpmUTIL.GetDBString(row.Item("Description"))
                    Me.ArticleModDate = wpmUTIL.GetDBDate(row("StartDT"))
                    Me.ArticlePageID = wpmUTIL.GetDBString(row("pageid"))
                    Me.PageName = wpmUTIL.GetDBString(row("PageName"))
                    Me.IsArticleActive = wpmUTIL.GetDBBoolean(row("active"))
                    Me.ContactID = wpmUTIL.GetDBString(row("ContactID"))
                Next
            Catch ex As Exception
                bFunctionStatus = False
            End Try
            bFunctionStatus = True
        End If
        Return bFunctionStatus
    End Function

    Private Function SetArticleByArticleID(ByVal reqArticleID As String) As Boolean
        Dim bFunctionStatus As Boolean = False
        Dim artSQL As String = ("SELECT article.ArticleID, article.Title, article.Description, article.ArticleBody, article.StartDT, page.pageid, page.pagename, article.active,article.contactid FROM article LEFT JOIN page ON article.PageID = page.PageID WHERE [article].[ArticleID]=" & reqArticleID)
        If Trim(reqArticleID) = "" Then
            bFunctionStatus = False
        Else
            Try
                For Each row As DataRow In wpmDB.GetDataTable(artSQL, "mhArticle.SetArticleByArticleID").Rows
                    bFunctionStatus = True
                    Me.ArticleID = wpmUTIL.GetDBString(row.Item("ArticleID"))
                    Me.ArticleBody = FormatArticleBody(wpmUTIL.GetDBString(row.Item("ArticleBody")))
                    Me.ArticleName = wpmUTIL.GetDBString(row.Item("Title"))
                    Me.ArticleDescription = wpmUTIL.GetDBString(row.Item("Description"))
                    Me.ArticleModDate = wpmUTIL.GetDBDate(row("StartDT"))
                    Me.ArticlePageID = wpmUTIL.GetDBString(row("pageid"))
                    Me.PageName = wpmUTIL.GetDBString(row("PageName"))
                    Me.IsArticleActive = wpmUTIL.GetDBBoolean(row("active"))
                    Me.ArticleAdmin = GetArticleAdmin(Me.ArticleID)
                    Me.ContactID = wpmUTIL.GetDBString(row("ContactID"))
                Next
            Catch ex As Exception
                bFunctionStatus = False
            End Try
            bFunctionStatus = True
        End If
        Return bFunctionStatus
    End Function
    Private Function SetArticleByPageID(ByVal reqPageID As String, ByVal reqArticleID As String, ByVal DefaultArticleID As String) As Boolean
        Dim bFunctionStatus As Boolean = False
        If IsNumeric(reqPageID) Then
            Dim artSQL As String = ("SELECT [article].[ArticleID],[article].[Title],[article].[Description],[article].[ArticleBody],[article].[StartDT],[page].[pageid],[page].[pagename],[article].[active],[article].[contactid]  FROM [article],[page] WHERE [page].[pageid]=[article].[pageid] and [article].[PageID]=" & reqPageID & " ORDER BY [article].[StartDT] ")
            Dim myDT As DataTable = wpmDB.GetDataTable(artSQL, "mhArticle.SetArticleByPageID")
            Me.ArticleBody = ""
            For Each row As DataRow In myDT.Rows
                If Me.ArticleID = reqArticleID Then
                    bFunctionStatus = True
                    Me.ArticleID = wpmUTIL.GetDBString(row.Item("ArticleID"))
                    Me.ArticleBody = FormatArticleBody(wpmUTIL.GetDBString(row.Item("ArticleBody")))
                    Me.ArticleDescription = wpmUTIL.GetDBString(row.Item("Description"))
                    Me.ArticleName = wpmUTIL.GetDBString(row.Item("Title"))
                    Me.ArticleModDate = wpmUTIL.GetDBDate(row("StartDT"))
                    Me.ArticlePageID = wpmUTIL.GetDBString(row("pageid"))
                    Me.PageName = wpmUTIL.GetDBString(row("PageName"))
                    Me.IsArticleActive = wpmUTIL.GetDBBoolean(row("active"))
                    Me.ArticleAdmin = GetArticleAdmin(Me.ArticleID)
                    Me.ContactID = wpmUTIL.GetDBString(row("ContactID"))
                    Exit For
                End If
            Next
            If Not (bFunctionStatus) Then
                For Each row As DataRow In myDT.Rows
                    bFunctionStatus = True
                    Me.ArticleID = wpmUTIL.GetDBString(row.Item("ArticleID"))
                    Me.ArticleBody = FormatArticleBody(wpmUTIL.GetDBString(row.Item("ArticleBody")))
                    Me.ArticleDescription = wpmUTIL.GetDBString(row.Item("Description"))
                    Me.ArticleName = wpmUTIL.GetDBString(row.Item("Title"))
                    Me.ArticleModDate = wpmUTIL.GetDBDate(row("StartDT"))
                    Me.ArticlePageID = wpmUTIL.GetDBString(row("pageid"))
                    Me.PageName = wpmUTIL.GetDBString(row("PageName"))
                    Me.IsArticleActive = wpmUTIL.GetDBBoolean(row("active"))
                    Me.ArticleAdmin = GetArticleAdmin(Me.ArticleID)
                    Me.ContactID = wpmUTIL.GetDBString(row("ContactID"))
                    Exit For
                Next
            End If
        End If
        If Not bFunctionStatus Then
            bFunctionStatus = SetArticleByArticleID(DefaultArticleID)
        End If
        Return bFunctionStatus
    End Function

    Public Function updateArticleBody() As Boolean
        Dim result As Boolean = False
        If Me.ArticleID = "" Then
            Throw New Exception("Missing ArticleID can not Update Article")
        Else
            Dim connection As New OleDbConnection(wpmConfig.ConnStr)
            Try
                connection.Open()
                Dim command As New OleDbCommand("UPDATE [Article] set " & _
                            "[Article].[ArticleBody]=@ArticleBody, " & _
                            "[Article].[title]=@ArticleName, " & _
                            "[Article].[ModifiedDT]=now() " & _
                            "WHERE [Article].[ArticleID]=@ArticleID ", connection)
                command.Parameters.AddWithValue("@ArticleBody", Me.ArticleBody)
                command.Parameters.AddWithValue("@ArticleName", Me.ArticleName)
                'command.Parameters.AddWithValue("@ArticleModDate", Today.Date())
                command.Parameters.AddWithValue("@ArticleID", Me.ArticleID)
                If command.ExecuteNonQuery() > 0 Then
                    result = True
                End If
            Catch ex As Exception
                wpmLog.AuditLog("mhArticle.updateArticleBody", ex.ToString)
            Finally
                connection.Close()
                connection = Nothing
            End Try
            Return result
        End If
    End Function
    Private Function GetArticleAdmin(ByVal ArticleID As String) As String
        If wpmUser.IsAdmin() Then
            If (Me.IsArticleDefault) Then
                Return ("<br /><br /><hr /><font color=""red"">No Article For Current Page -> </font><a href=""" & App.Config.AspMakerGen & "article_add.aspx""> Add Article </a>")
            Else
                Return ("<br/><br /><hr /><a href=""" & App.Config.AspMakerGen & "article_edit.aspx?ArticleID=" & ArticleID & """> Edit Article </a>")
            End If
        Else
            Return ""
        End If
    End Function
    Private Function GetCurrentArticleID() As String
        Dim session As System.Web.SessionState.HttpSessionState = System.Web.HttpContext.Current.Session
        GetCurrentArticleID = CStr(session.Item("CurrentArticleID"))
    End Function
    Private Function SetCurrentArticleID(ByVal sArticleID As String) As String
        Dim session As System.Web.SessionState.HttpSessionState = System.Web.HttpContext.Current.Session
        session.Item("CurrentArticleID") = sArticleID
        SetCurrentArticleID = session.Item("CurrentArticleID").ToString
    End Function
    Private Function FormatArticleBody(ByRef ArticleBody As String) As String
        Dim sbArticleBody As New StringBuilder(ArticleBody)
        sbArticleBody.Append(GetArticleAdmin(Me.ArticleID))
        Return sbArticleBody.ToString
    End Function
End Class

Public Class wpmArticleList
    Inherits List(Of wpmArticle)
    Private curPageID As String
    Private curArticleID As String
    Private curRecordsPerPage As Integer = 3
    Private siteDefaultArticleID As String

    Private Function GetFileName(ByVal FileName As String) As String
        Return App.Config.ConfigFolderPath & FileName & ".xml"
    End Function

    Public Sub New(ByVal ListName As String)
        PopulateCompanyArticleList()
    End Sub
    Sub New(ByRef myActiveSite As wpmActiveSite)
        curPageID = myActiveSite.CurrentPageID()
        curArticleID = myActiveSite.CurrentArticleID()
        siteDefaultArticleID = myActiveSite.DefaultArticleID
        PopulatePageArticleList(myActiveSite.CurrentPageID())
        If Me.Count > 0 Then
            curRecordsPerPage = Me.Item(0).RowsPerPage
        Else
            curRecordsPerPage = 999
        End If
    End Sub

    Private Function SaveArticleListFile(ByVal filename As String) As Boolean
        Try
            wpmFileIO.DeleteFile(GetFileName(filename))
            Dim ViewFile As New System.IO.StreamWriter(GetFileName(filename))
            Dim ViewWriter As New System.Xml.Serialization.XmlSerializer(GetType(wpmArticleList))
            ViewWriter.Serialize(ViewFile, Me)
            ViewFile.Close()
            ViewFile.Dispose()
            Return True
        Catch ex As Exception
            Return False
        End Try
        Return False
    End Function

    Private Function LoadArticleListFile(ByVal filename As String) As Boolean
        Dim bReturn As Boolean = False
        Dim mylist As wpmArticleList
        Try
            Dim sr As New StreamReader(GetFileName(filename))
            Dim xs As New XmlSerializer(GetType(wpmArticleList))
            mylist = DirectCast(xs.Deserialize(sr), wpmArticleList)
            sr.Close()
            bReturn = True
        Catch
            bReturn = False
        Finally
        End Try
        Return bReturn
    End Function

    Private Function PopulateCompanyArticleList() As Boolean
        Dim sSQL As String = ""
        sSQL = ("SELECT [Article].[ArticleID]," & _
                       "[Article].[title]," & _
                       "[Article].[Description]," & _
                       "[Article].[ArticleSummary]," & _
                       "[Article].[StartDT]," & _
                       "[Article].[ContactID]," & _
                       "[Article].[ArticleBody]," & _
                       "[Article].[active]," & _
                       "[Article].[PageID], " & _
                       "[Company].[CompanyName] " & _
                       "FROM [Article],[Company] " & _
                       "where [Article].[CompanyID]=[Company].[CompanyID] " & _
                       "and [Company].[CompanyID]=" & wpmSession.GetCompanyID & " ")
        If Not wpmUser.IsAdmin() Then
            sSQL = sSQL & " and [Article].[Active]= TRUE "
        End If
        Me.Clear()
        sSQL = sSQL & "order by [Article].[StartDT] desc "
        For Each row As DataRow In wpmDB.GetDataTable(sSQL, "PopulateArticleList").Rows
            Dim myArticleRow As New wpmArticle
            myArticleRow.ArticleID = wpmUTIL.GetDBString(row.Item("ArticleID"))
            myArticleRow.ArticleBody = wpmUTIL.GetDBString(row.Item("ArticleBody"))
            myArticleRow.ArticleModDate = wpmUTIL.GetDBDate(row.Item("StartDT"))
            myArticleRow.ArticleSummary = wpmUTIL.GetDBString(row.Item("ArticleSummary"))
            myArticleRow.ArticleDescription = wpmUTIL.GetDBString(row.Item("Description"))
            myArticleRow.ArticleName = wpmUTIL.GetDBString(row.Item("title"))
            myArticleRow.IsArticleActive = wpmUTIL.GetDBBoolean(row.Item("active"))
            myArticleRow.ArticlePageID = wpmUTIL.GetDBString(row.Item("PageID"))
            myArticleRow.IsArticleDefault = False
            myArticleRow.ArticleURL = GetArticleURL(myArticleRow)

            myArticleRow.PageName = myArticleRow.ArticleName
            myArticleRow.ArticleAdmin = GetArticleAdmin(myArticleRow.ArticleID)
            Me.Add(myArticleRow)
        Next
    End Function
    Private Function GetArticleURL(ByVal myarticle As wpmArticle) As String
        If App.Config.Use404Processing Then
            Return wpmUTIL.FormatNameForURL(myarticle.PageName & "/" & myarticle.ArticleName)
        Else
            Return "/default.aspx?c=" & curPageID & "&a=" & myarticle.ArticleID
        End If
    End Function

    Private Function PopulatePageArticleList(ByVal PageID As String) As Boolean
        Dim sSQL As String = ("SELECT [Article].[ArticleID]," & _
                       "[Article].[title]," & _
                       "[Article].[Description]," & _
                       "[Article].[ArticleSummary]," & _
                       "[Article].[StartDT]," & _
                       "[Contact].[PrimaryContact]," & _
                       "[Article].[ArticleBody]," & _
                       "[Article].[active]," & _
                       "[Page].[PageID], " & _
                       "[Page].[PageName], " & _
                       "[Page].[PageDescription], " & _
                       "[Page].[PageKeywords], " & _
                       "[Page].[RowsPerPage] " & _
                       "FROM (Article INNER JOIN Page ON Article.PageID = Page.PageID) LEFT JOIN Contact ON Article.ContactID = Contact.ContactID " & _
                       "where [Page].[PageID]=" & PageID & " ")
        If Not wpmUser.IsAdmin() Then
            sSQL = sSQL & " and [Article].[Active]= TRUE "
        End If
        sSQL = sSQL & "order by [Article].[StartDT] desc "
        Me.Clear()
        For Each row As DataRow In wpmDB.GetDataTable(sSQL, "PopulateArticleList").Rows
            Dim myArticleRow As New wpmArticle
            myArticleRow.ArticleID = wpmUTIL.GetDBString(row.Item("ArticleID"))
            myArticleRow.ArticleBody = wpmUTIL.GetDBString(row.Item("ArticleBody"))
            myArticleRow.ArticleSummary = wpmUTIL.GetDBString(row.Item("ArticleSummary"))
            myArticleRow.ArticleModDate = wpmUTIL.GetDBDate(row.Item("StartDT"))
            myArticleRow.ArticleDescription = wpmUTIL.GetDBString(row.Item("Description"))
            myArticleRow.ArticleName = wpmUTIL.GetDBString(row.Item("title"))
            myArticleRow.IsArticleActive = wpmUTIL.GetDBBoolean(row.Item("active"))
            myArticleRow.ArticlePageID = wpmUTIL.GetDBString(row.Item("PageID"))
            myArticleRow.IsArticleDefault = False
            myArticleRow.PageName = wpmUTIL.GetDBString(row.Item("PageName"))
            myArticleRow.RowsPerPage = wpmUTIL.GetDBInteger(row.Item("RowsPerPage"))
            myArticleRow.ArticleAuthor = wpmUTIL.GetDBString(row.Item("PrimaryContact"))
            myArticleRow.ArticleAdmin = GetArticleAdmin(myArticleRow.ArticleID)
            myArticleRow.ArticleURL = GetArticleURL(myArticleRow)
            Me.Add(myArticleRow)
        Next
    End Function

    Private Function GetArticleAdmin(ByVal ArticleID As String) As String
        If wpmUser.IsAdmin() Then
            Return ("<br /><br /><hr /><a href=""" & App.Config.AspMakerGen & "article_edit.aspx?ArticleID=" & ArticleID & """> Edit Article </a>")
        Else
            Return ""
        End If
    End Function

    Public Function GetBlogPosts(ByRef sbBlogTemplate As StringBuilder) As String
        Dim sItem As New StringBuilder
        Dim sWeblogRSS As String = ("http://" & HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME") & "" & App.Config.wpmWebHome() & "blog/blog_rss.aspx?c=" & curPageID)
        If (curArticleID = "") Or (curArticleID = siteDefaultArticleID) Then
            sItem.Append("<div class=""blog"">")
            sItem.Append("<div class=""blog-posts"">")
            For Each myArticle As wpmArticle In Me
                ' Build The Blog Entry
                AddArticleWithParm(myArticle, sItem, sbBlogTemplate)
            Next
            sItem.Append("</div>")
            sItem.Append("</div>")
        Else
            For Each myArticle As wpmArticle In Me
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
        Dim sURL As String = ("/default.aspx?c=" & curPageID & "&amp;a=" & curArticleID)
        Dim sSubPageNav As String = ("")
        Dim iPostCount As Integer = 0
        Dim iPageCount As Integer = 0
        Dim iFirstDisplay As Integer = 0
        Dim iLastDisplay As Integer = 0
        Dim iPageNumber As Integer = 0

        Dim iRow As Integer = 0
        If (curArticleID = "") Or (curArticleID = siteDefaultArticleID) Then

            If Me.Count = 0 Then
                ' No Rows
            Else
                ' Count number of Images for this PageID, SETUP FOR REMAINDER OF TASKS
                ' determine page break
                If (curRecordsPerPage < 1) Then
                    curRecordsPerPage = 1
                End If
                iPageCount = CInt(Math.Round(Val(Me.Count / curRecordsPerPage), 0))
                ' If last image is on a page break, then subtract one page to avoid an empty page
                If (iPageCount * curRecordsPerPage) > Me.Count Then
                    ' Do nothing we have the right number of pages
                Else
                    If (Me.Count Mod curRecordsPerPage) = 0 Then
                    Else
                        iPageCount = iPageCount + 1
                    End If
                End If
                ' Determine First and Last record to display
                If Me.Count > 0 Then
                    If iCurrentPageNumber <= 1 Then
                        ' We are on the first page
                        iPageNumber = 1
                        iFirstDisplay = 0
                        iLastDisplay = curRecordsPerPage - 1
                    Else
                        iPageNumber = iCurrentPageNumber
                        iFirstDisplay = ((iPageNumber * curRecordsPerPage) - curRecordsPerPage)
                        iLastDisplay = iFirstDisplay + curRecordsPerPage - 1
                    End If
                End If
                ' Build Sub Page Navigation
                If iPageNumber > 1 Then
                    sSubPageNav = sSubPageNav & "<a title=""PREVIOUS"" href=""" & sURL & "&amp;Page=" & iPageNumber - 1 & """><::</a>"
                End If
                If (iPageCount > 1) Then
                    sSubPageNav = sSubPageNav & "<b>Page " & iPageNumber & " of " & iPageCount & "</b>"
                End If
                If iPageNumber < iPageCount Then
                    sSubPageNav = sSubPageNav & "<a title=""NEXT"" href=""" & sURL & "&amp;Page=" & iPageNumber + 1 & """>::></a>"
                End If
                If sSubPageNav <> "" Then
                    sItem.Append("<center>" & sSubPageNav & "</center>" & vbCrLf & vbCrLf)
                End If
                ' Draw The current page

                For Each myArticle As wpmArticle In Me
                    ' Determine if this record is displayed
                    If (iRow >= iFirstDisplay) Then
                        If (iRow <= iLastDisplay) Then
                            AddArticleWithParm(myArticle, sItem, sbBlogTemplate)
                        End If
                    End If
                    iRow = iRow + 1
                Next
            End If
            sItem.Append("<center>" & sSubPageNav & "</center>" & vbCrLf & vbCrLf)
            sItem.Append(vbCrLf)
        Else
            For Each myArticle As wpmArticle In Me
                If myArticle.ArticleID = curArticleID Then
                    AddArticleWithParm(myArticle, sItem, sbBlogTemplate)
                    Exit For
                End If
            Next
        End If
        Return sItem.ToString
    End Function

    Private Function AddArticleWithParm(ByVal myArticle As wpmArticle, ByRef sItem As StringBuilder, ByRef sbBlogTemplate As StringBuilder) As Boolean
        sItem.Append(sbBlogTemplate.ToString)
        sItem.Replace("~~PostID~~", myArticle.ArticleID)
        sItem.Replace("~~PostDate~~", myArticle.ArticleModDate.ToString)
        sItem.Replace("~~PostShortDate~~", myArticle.ArticleModDate.ToString("m"))
        sItem.Replace("~~PostURL~~", myArticle.ArticleURL)
        sItem.Replace("~~PostName~~", myArticle.ArticleName)
        sItem.Replace("~~PostTitle~~", myArticle.ArticleName)
        sItem.Replace("~~PostBody~~", myArticle.ArticleBody)
        sItem.Replace("~~PostSummary~~", myArticle.ArticleSummary)
        sItem.Replace("~~PostDescription~~", myArticle.ArticleDescription)
        sItem.Replace("~~BlogPageURL~~", "/default.aspx?c=" & curPageID)
        sItem.Replace("~~BlogPageName~~", myArticle.PageName)
        sItem.Replace("~~PostAuthor~~", myArticle.ArticleAuthor)
        sItem.Replace("~~ArticleAdmin~~", myArticle.ArticleAdmin)
        Return True
    End Function


End Class


