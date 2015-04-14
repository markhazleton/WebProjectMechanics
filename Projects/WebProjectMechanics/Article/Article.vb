Imports System.Data.OleDb
Imports System.Text

Public Class Article
    Implements IArticle

    Public Property ArticleID As Integer Implements IArticle.ArticleID
    Public Property ArticleName As String Implements IArticle.ArticleName
    Public Property ArticlePageID As String Implements IArticle.ArticlePageID
    Public Property ArticleSummary As String Implements IArticle.ArticleSummary

    Public Property ArticleAdmin As String Implements IArticle.ArticleAdmin
    Public Property ArticleAuthor As String Implements IArticle.ArticleAuthor
    Public Property ArticleBody As String Implements IArticle.ArticleBody
    Public Property ArticleDescription As String Implements IArticle.ArticleDescription
    Public Property ArticleModDate As Date Implements IArticle.ArticleModDate
    Public Property ArticleURL As String Implements IArticle.ArticleURL
    Public Property CompanyID As String Implements IArticle.CompanyID
    Public Property ContactID As String Implements IArticle.ContactID
    Public Property IsArticleActive As Boolean Implements IArticle.IsArticleActive
    Public Property IsArticleDefault As Boolean Implements IArticle.IsArticleDefault
    Public Property PageName As String Implements IArticle.PageName
    Public Property RowsPerPage As Integer Implements IArticle.RowsPerPage

    Public Sub New()
        ' Blank Article
        ArticleBody = String.Empty
        ArticleName = String.Empty
    End Sub
    Public Sub New(ByRef CurrentMapRow As Location, ByVal DefaultArticleID As Integer)
        ArticleBody = String.Empty
        ArticleName = String.Empty
        Select Case CurrentMapRow.RecordSource
            Case "Page"
                SetArticleByPageID(CurrentMapRow.LocationID, CurrentMapRow.ArticleID, DefaultArticleID)
            Case "Article"
                GetArticleByArticleID(CurrentMapRow.ArticleID)
            Case "Module"
                SetArticleByPageID(CurrentMapRow.LocationID, CurrentMapRow.ArticleID, DefaultArticleID)
            Case Else
                If CurrentMapRow.ArticleID < 1 Then
                    If DefaultArticleID > 0 Then
                        GetArticleByArticleID(DefaultArticleID)
                    End If
                End If
        End Select
    End Sub
    'Public Sub New(ByVal reqArticleID As String, ByVal reqPageID As String, ByVal DefaultArticleID As String)
    '    MyBase.New()
    '    ArticleBody = String.Empty
    '    ArticleName = String.Empty
    '    IsArticleDefault = False
    '    If wpm_GetDBInteger(reqPageID) > 0 Then
    '        If wpm_GetDBInteger(reqArticleID) > 0 Then
    '            GetDefaultArticle(wpm_GetDBInteger(DefaultArticleID))
    '        Else
    '            ArticleID = wpm_GetDBInteger(reqArticleID)
    '            If Not GetArticleByArticleID(wpm_GetDBInteger(reqArticleID)) Then
    '                GetDefaultArticle(wpm_GetDBInteger(DefaultArticleID))
    '            End If
    '        End If
    '    Else
    '        If Not SetArticleByPageID(reqPageID, wpm_GetDBInteger(reqArticleID), wpm_GetDBInteger(DefaultArticleID)) Then
    '            If IsNothing(DefaultArticleID) Then
    '                ArticleBody = "<h1>NO DEFAULT ARTICLE</h1>"
    '            Else
    '                GetDefaultArticle(wpm_GetDBInteger(DefaultArticleID))
    '            End If
    '        End If
    '    End If
    'End Sub
    Public Sub New(ByVal reqArticleID As Integer, ByVal reqPageID As String, ByVal DefaultArticleID As Integer)
        MyBase.New()
        ArticleBody = String.Empty
        ArticleName = String.Empty

        IsArticleDefault = False

        If wpm_GetDBInteger(reqPageID) > 0 Then
            If reqArticleID < 1 Then
                GetDefaultArticle(DefaultArticleID)
            Else
                ArticleID = reqArticleID
                If Not GetArticleByArticleID(reqArticleID) Then
                    GetDefaultArticle(DefaultArticleID)
                End If
            End If
        Else
            If Not SetArticleByPageID(reqPageID, reqArticleID, DefaultArticleID) Then
                If IsNothing(DefaultArticleID) Then
                    ArticleBody = "<h1>NO DEFAULT ARTICLE</h1>"
                Else
                    GetDefaultArticle(DefaultArticleID)
                End If
            End If
        End If
    End Sub
    Public Sub New(ByVal reqArticleID As Integer)
        MyBase.New()
        GetArticleByArticleID(reqArticleID)

    End Sub
    Private Function GetDefaultArticle(ByVal SiteArticleID As Integer) As Boolean
        ArticleID = SiteArticleID
        IsArticleDefault = True
        If ArticleID > 0 Then
            Return GetArticleByArticleID(ArticleID)
        Else
            Return False
        End If
    End Function
    Private Function GetArticleByArticleID(ByVal reqArticleID As Integer) As Boolean
        Dim bFunctionStatus As Boolean = False
        Dim artSQL As String = ("SELECT article.author, article.ArticleID, article.Title, article.ArticleSummary, article.Description, article.ArticleBody, article.StartDT, page.pageid, page.pagename, article.active, article.contactID FROM article LEFT JOIN page ON article.PageID = page.PageID WHERE [article].[ArticleID]=" & reqArticleID)
        If reqArticleID < 1 Then
            bFunctionStatus = False
        Else
            Try
                For Each row As DataRow In wpm_GetDataTable(artSQL, "Article.SetArticleByArticleID").Rows
                    bFunctionStatus = True
                    ArticleID = wpm_GetDBInteger(row.Item("ArticleID"))
                    ArticleBody = wpm_GetDBString(row.Item("ArticleBody"))
                    ArticleName = wpm_GetDBString(row.Item("Title"))
                    ArticleDescription = wpm_GetDBString(row.Item("Description"))
                    ArticleSummary = wpm_GetDBString(row.Item("ArticleSummary"))
                    ArticleModDate = Now()
                    ArticlePageID = wpm_GetDBString(row.Item("PageID"))
                    PageName = wpm_GetDBString(row.Item("PageName"))
                    IsArticleActive = wpm_GetDBBoolean(row.Item("active"))
                    ContactID = wpm_GetDBString(row.Item("ContactID"))
                    ArticleAuthor = wpm_GetDBString(row.Item("Author"))
                Next
            Catch ex As Exception
                bFunctionStatus = False
            End Try
            bFunctionStatus = True
        End If
        Return bFunctionStatus
    End Function
    Private Function SetArticleByPageID(ByVal reqPageID As String, ByVal reqArticleID As Integer, ByVal DefaultArticleID As Integer) As Boolean
        Dim bFunctionStatus As Boolean = False
        If IsNumeric(reqPageID) Then
            Dim artSQL As String = (String.Format("SELECT [article].[ArticleID],[article].[Title],[article].[Description],[article].[ArticleBody],[article].[ModifiedDT],[page].[pageid],[page].[pagename],[article].[active],[article].[contactid],[article].[Author]  FROM [article],[page] WHERE [page].[pageid]=[article].[pageid] and [article].[PageID]={0} ORDER BY [article].[StartDT] ", reqPageID))
            Using myDT As DataTable = wpm_GetDataTable(artSQL, "mhArticle.SetArticleByPageID")
                ArticleBody = ""
                For Each row As DataRow In myDT.Rows
                    If ArticleID = reqArticleID Then
                        bFunctionStatus = True
                        ArticleID = wpm_GetDBInteger(row.Item("ArticleID"))
                        ArticleBody = FormatArticleBody(wpm_GetDBString(row.Item("ArticleBody")))
                        ArticleDescription = wpm_GetDBString(row.Item("Description"))
                        ArticleName = wpm_GetDBString(row.Item("Title"))
                        ArticleModDate = wpm_GetDBDate(row("ModifiedDT"))
                        ArticlePageID = wpm_GetDBString(row("pageid"))
                        PageName = wpm_GetDBString(row("PageName"))
                        IsArticleActive = wpm_GetDBBoolean(row("active"))
                        ArticleAdmin = GetArticleAdmin(ArticleID)
                        ContactID = wpm_GetDBString(row("ContactID"))
                        Exit For
                    End If
                Next
                If Not (bFunctionStatus) Then
                    For Each row As DataRow In myDT.Rows
                        bFunctionStatus = True
                        ArticleID = wpm_GetDBInteger(row.Item("ArticleID"))
                        ArticleBody = FormatArticleBody(wpm_GetDBString(row.Item("ArticleBody")))
                        ArticleDescription = wpm_GetDBString(row.Item("Description"))
                        ArticleName = wpm_GetDBString(row.Item("Title"))
                        ArticleModDate = wpm_GetDBDate(row("ModifiedDT"))
                        ArticlePageID = wpm_GetDBString(row("pageid"))
                        PageName = wpm_GetDBString(row("PageName"))
                        IsArticleActive = wpm_GetDBBoolean(row("active"))
                        ArticleAdmin = GetArticleAdmin(ArticleID)
                        ContactID = wpm_GetDBString(row("ContactID"))
                        Exit For
                    Next
                End If
            End Using
        End If
        If Not bFunctionStatus Then
            bFunctionStatus = GetArticleByArticleID(DefaultArticleID)
        End If
        Return bFunctionStatus
    End Function
    Public Function DeleteArticle() As Boolean
        If wpm_RunDeleteSQL(String.Format("Delete [Article] where [Article].[ArticleID]={0}", ArticleID), "Article") = 1 Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function UpdateArticle() As String
        Dim result As String = String.Empty
        If ArticleID > 0 Then
            Using connection As New OleDbConnection(wpm_SQLDBConnString)
                Try
                    connection.Open()
                    Using myCommand As New OleDbCommand("UPDATE [Article] set " & _
                                "[Article].[ArticleBody]=@ArticleBody, " & _
                                "[Article].[title]=@ArticleName, " & _
                                "[Article].[Description]=@ArticleDescription, " & _
                                "[Article].[ArticleSummary]=@ArticleSummary, " & _
                                "[Article].[Author]=@Author, " & _
                                "[Article].[UserID]=@UserID, " & _
                                "[Article].[ContactID]=@ContactID, " & _
                                "[Article].[CompanyID]=@CompanyID, " & _
                                "[Article].[PageID]=@PageID, " & _
                                "[Article].[ModifiedDT]=now() " & _
                                "WHERE [Article].[ArticleID]=@ArticleID ", connection)
                        wpm_AddParameterStringValue("@ArticleBody", ArticleBody, myCommand)
                        wpm_AddParameterStringValue("@ArticleName", ArticleName, myCommand)
                        wpm_AddParameterStringValue("@ArticleDescription", ArticleDescription, myCommand)
                        wpm_AddParameterStringValue("@ArticleSummary", ArticleSummary, myCommand)
                        wpm_AddParameterStringValue("@Author", ArticleAuthor, myCommand)
                        wpm_AddParameterStringValue("@UserID", ContactID, myCommand)
                        wpm_AddParameterStringValue("@ContactID", ContactID, myCommand)
                        wpm_AddParameterStringValue("@CompanyID", CompanyID, myCommand)
                        wpm_AddParameterStringValue("@PageID", ArticlePageID, myCommand)
                        wpm_AddParameterValue("@ArticleID", ArticleID, SqlDbType.Int, myCommand)

                        If myCommand.ExecuteNonQuery() > 0 Then
                            result = String.Empty
                        End If
                    End Using
                Catch ex As Exception
                    ApplicationLogging.SQLExceptionLog("Article.updateArticleBody", ex)
                    result = ex.ToString
                End Try
            End Using
        Else
            Using connection As New OleDbConnection(wpm_SQLDBConnString)
                Try
                    connection.Open()
                    Using myCommand As New OleDbCommand("INSERT into [Article] (" & _
                                "[ArticleBody], " & _
                                "[title], " & _
                                "[Description], " & _
                                "[ArticleSummary], " & _
                                "[Author], " & _
                                "[UserID], " & _
                                "[ContactID], " & _
                                "[CompanyID], " & _
                                "[PageID], " & _
                                "[ModifiedDT]) values( " & _
                                "@ArticleBody, " & _
                                "@ArticleName, " & _
                                "@ArticleDescription, " & _
                                "@ArticleSummary, " & _
                                "@Author, " & _
                                "@UserID, " & _
                                "@ContactID, " & _
                                "@CompanyID, " & _
                                "@PageID, " & _
                                "now() )", connection)
                        wpm_AddParameterStringValue("@ArticleBody", ArticleBody, myCommand)
                        wpm_AddParameterStringValue("@ArticleName", ArticleName, myCommand)
                        wpm_AddParameterStringValue("@ArticleDescription", ArticleDescription, myCommand)
                        wpm_AddParameterStringValue("@ArticleSummary", ArticleSummary, myCommand)
                        wpm_AddParameterStringValue("@Author", ArticleAuthor, myCommand)
                        wpm_AddParameterStringValue("@UserID", ContactID, myCommand)
                        wpm_AddParameterStringValue("@ContactID", ContactID, myCommand)
                        wpm_AddParameterStringValue("@CompanyID", CompanyID, myCommand)
                        wpm_AddParameterStringValue("@PageID", ArticlePageID, myCommand)
                        If myCommand.ExecuteNonQuery() > 0 Then
                            result = String.Empty
                        End If
                    End Using
                Catch ex As Exception
                    ApplicationLogging.SQLExceptionLog("Article.updateArticleBody", ex)
                    result = ex.ToString
                End Try
            End Using
        End If
        Return result
    End Function
    Private Function GetArticleAdmin(ByVal ArticleID As Integer) As String
        If wpm_IsAdmin Then
            If (IsArticleDefault) Then
                Return (String.Format("<br /><br /><hr /><font color=""red"">No Article For Current Page -> </font><a href=""{0}maint/default.aspx?type=Article""> Article List </a>", wpm_SiteConfig.AdminFolder))
            Else
                Return (String.Format("<br/><br /><hr /><a href=""{0}/maint/default.aspx?type=Article&ArticleID={1}""> Edit Article </a>", wpm_SiteConfig.AdminFolder, ArticleID))
            End If
        Else
            Return ""
        End If
    End Function
    Private Function FormatArticleBody(ByRef ArticleBody As String) As String
        Dim sbArticleBody As New StringBuilder(ArticleBody)
        sbArticleBody.Append(GetArticleAdmin(ArticleID))
        Return sbArticleBody.ToString
    End Function
    Public Function CopyArticle(ByRef SourceArticle As Article) As Article
        With SourceArticle
            ArticleAdmin = .ArticleAdmin
            ArticleAuthor = .ArticleAuthor
            ArticleBody = .ArticleBody
            ArticleDescription = .ArticleDescription
            ArticleID = .ArticleID
            ArticleModDate = .ArticleModDate
            ArticleName = .ArticleName
            ArticlePageID = .ArticlePageID
            ArticleSummary = .ArticleSummary
            ArticleURL = .ArticleURL
            CompanyID = .CompanyID
            ContactID = .ContactID
            IsArticleActive = .IsArticleActive
            IsArticleDefault = .IsArticleDefault
            PageName = .PageName
            RowsPerPage = .RowsPerPage
        End With
        Return Me
    End Function



End Class
