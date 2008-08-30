Public Class mhArticleRow
    Public ArticleURL As String
    Public ArticleID As String
    Public ArticleName As String
    Public ArticleDescription As String
    Public ArticleBody As String
    Public ArticleModDate As Date
    Public ArticlePageID As String
    Public IsArticleDefault As Boolean
    Public PageName As String
    Public IsArticleActive As Boolean
    Public RowsPerPage As Integer
    Public ArticleAuthor As String
    Public ArticleAdmin As String
    Public ContactID As String
End Class
Public Class mhArticleRows
    Inherits List(Of mhArticleRow)

    Public Sub New(ByVal PageID As String, ByVal ListName As String)
        Dim sFileName As String = ("PageList-" & PageID)
        If Not LoadArticleListFile(sFileName) Or mhUser.IsAdmin() Then
            PopulatePageArticleList(PageID)
            SaveArticleListFile(sFileName)
        End If
    End Sub
    Public Sub New(ByVal ListName As String)
        Dim sFileName As String = ("CompanyList-" & mhSession.GetCompanyID)
        If Not LoadArticleListFile(sFileName) Or mhUser.IsAdmin() Then
            PopulateCompanyArticleList()
            SaveArticleListFile(sFileName)
        End If
    End Sub
    Private Function SaveArticleListFile(ByVal ListDescription As String) As Boolean
        Dim sFileName As String = mhConfig.mhWebConfig & "\" & ListDescription & ".xml"
        'Try
        '    mhfio.DeleteFile(sFileName)
        '    Dim ViewFile As New System.IO.StreamWriter(sFileName)
        '    Dim ViewWriter As New System.Xml.Serialization.XmlSerializer(GetType(mhArticleRows))
        '    ViewWriter.Serialize(ViewFile, myArticleRows)
        '    ViewFile.Close()
        '    ViewFile.Dispose()
        '    Return True
        'Catch ex As Exception
        '    Return False
        'End Try
        Return False
    End Function

    Private Function LoadArticleListFile(ByVal ListDescription As String) As Boolean
        Dim bReturn As Boolean = False
        'Try
        '    Dim sr As New StreamReader(mhConfig.ApplicationIndexPath & "\" & ListDescription & ".xml")
        '    Dim xs As New XmlSerializer(GetType(mhArticleRows))
        '    myArticleRows = DirectCast(xs.Deserialize(sr), mhArticleRows)
        '    sr.Close()
        '    bReturn = True
        'Catch
        '    bReturn = False
        'Finally
        'End Try
        Return bReturn
    End Function

    Private Function PopulateCompanyArticleList() As Boolean
        Dim sSQL As String = ""
        sSQL = ("SELECT [Article].[ArticleID]," & _
                       "[Article].[title]," & _
                       "[Article].[Description]," & _
                       "[Article].[StartDT]," & _
                       "[Article].[ContactID]," & _
                       "[Article].[ArticleBody]," & _
                       "[Article].[active]," & _
                       "[Article].[PageID], " & _
                       "[Company].[CompanyName] " & _
                       "FROM [Article],[Company] " & _
                       "where [Article].[CompanyID]=[Company].[CompanyID] " & _
                       "and [Company].[CompanyID]=" & mhSession.GetCompanyID & " ")
        If Not mhUser.IsAdmin() Then
            sSQL = sSQL & " and [Article].[Active]= TRUE "
        End If
        Me.Clear()
        sSQL = sSQL & "order by [Article].[StartDT] desc "
        For Each row As DataRow In mhDB.GetDataTable(sSQL, "PopulateArticleList").Rows
            Dim myArticleRow As New mhArticleRow
            myArticleRow.ArticleID = mhUTIL.GetDBString(row.Item("ArticleID"))
            myArticleRow.ArticleBody = mhUTIL.GetDBString(row.Item("ArticleBody"))
            myArticleRow.ArticleModDate = mhUTIL.GetDBDate(row.Item("StartDT"))
            myArticleRow.ArticleDescription = mhUTIL.GetDBString(row.Item("Description"))
            myArticleRow.ArticleName = mhUTIL.GetDBString(row.Item("title"))
            myArticleRow.IsArticleActive = mhUTIL.GetDBBoolean(row.Item("active"))
            myArticleRow.ArticlePageID = mhUTIL.GetDBString(row.Item("PageID"))
            myArticleRow.IsArticleDefault = False
            myArticleRow.ArticleURL = mhUTIL.FormatNameForURL(myArticleRow.PageName & "/" & myArticleRow.ArticleName)
            myArticleRow.PageName = myArticleRow.ArticleName
            myArticleRow.ArticleAdmin = GetArticleAdmin(myArticleRow.ArticleID)
            Me.Add(myArticleRow)
        Next
    End Function

    Private Function PopulatePageArticleList(ByVal PageID As String) As Boolean
        Dim sSQL As String = ("SELECT [Article].[ArticleID]," & _
                       "[Article].[title]," & _
                       "[Article].[Description]," & _
                       "[Article].[StartDT]," & _
                       "[Contact].[PrimaryContact]," & _
                       "[Article].[ArticleBody]," & _
                       "[Article].[active]," & _
                       "[Page].[PageID], " & _
                       "[Page].[PageName], " & _
                       "[Page].[PageDescription], " & _
                       "[Page].[PageKeywords], " & _
                       "[Page].[RowsPerPage] " & _
                       "FROM [Article],[Contact],[Page] " & _
                       "where [Article].[ContactID]=[Contact].[ContactID] " & _
                       "and [Article].[PageID]=[Page].[PageID] " & _
                       "and [Page].[PageID]=" & PageID & " ")
        If Not mhUser.IsAdmin() Then
            sSQL = sSQL & " and [Article].[Active]= TRUE "
        End If
        sSQL = sSQL & "order by [Article].[StartDT] desc "
        Me.Clear()
        For Each row As DataRow In mhDB.GetDataTable(sSQL, "PopulateArticleList").Rows
            Dim myArticleRow As New mhArticleRow
            myArticleRow.ArticleID = mhUTIL.GetDBString(row.Item("ArticleID"))
            myArticleRow.ArticleBody = mhUTIL.GetDBString(row.Item("ArticleBody"))
            myArticleRow.ArticleModDate = mhUTIL.GetDBDate(row.Item("StartDT"))
            myArticleRow.ArticleDescription = mhUTIL.GetDBString(row.Item("Description"))
            myArticleRow.ArticleName = mhUTIL.GetDBString(row.Item("title"))
            myArticleRow.IsArticleActive = mhUTIL.GetDBBoolean(row.Item("active"))
            myArticleRow.ArticlePageID = mhUTIL.GetDBString(row.Item("PageID"))
            myArticleRow.IsArticleDefault = False
            myArticleRow.PageName = mhUTIL.GetDBString(row.Item("PageName"))
            myArticleRow.RowsPerPage = mhUTIL.GetDBInteger(row.Item("RowsPerPage"))
            myArticleRow.ArticleAuthor = mhUTIL.GetDBString(row.Item("PrimaryContact"))
            myArticleRow.ArticleAdmin = GetArticleAdmin(myArticleRow.ArticleID)
            Me.Add(myArticleRow)
        Next
    End Function
    '********************************************************************************
    'Public Function BuildArticleList(ByRef colSelect As System.Array, ByRef ReqArticleID As String, ByVal ServerURL As String) As String
    '    Dim alttext As String
    '    Dim title As Object
    '    Dim text1 As String
    '    Dim lenght As Integer
    '    Dim j As Integer
    '    Dim strReturn As String
    '    Dim ID As Object
    '    strReturn = ""
    '    For j = 0 To UBound(colSelect, 2)
    '        ID = colSelect(0, j)
    '        title = colSelect(1, j)
    '        title = LCase(title)
    '        text1 = Left(title, 100)
    '        lenght = Len(CStr(title))
    '        title = Left(title, 20)
    '        alttext = text1 & "(" & ID & ")"
    '        strReturn = strReturn & "<a title=""" & alttext & """ href=""" & ServerURL & "?a=" & ID & """>"
    '        If CShort(ID) = CShort(ReqArticleID) Then
    '            strReturn = strReturn & "<b>"
    '        End If
    '        strReturn = strReturn & title
    '        If lenght > 20 Then
    '            strReturn = strReturn & "..."
    '        End If
    '        If CShort(ID) = CShort(ReqArticleID) Then
    '            strReturn = strReturn & "</b>"
    '        End If
    '        strReturn = strReturn & "</a> | "
    '    Next
    '    strReturn = strReturn & "<hr/>"
    '    BuildArticleList = strReturn
    'End Function
    Private Function GetArticleAdmin(ByVal ArticleID As String) As String
        If mhUser.IsAdmin() Then
            Return ("<br /><br /><hr /><a href=""" & mhConfig.mhASPMakerGen & "article_edit.aspx?ArticleID=" & ArticleID & """> Edit Article </a>")
        Else
            Return ""
        End If
    End Function

End Class

Public Class mhArticle
    Public ReadOnly Property ArticleID() As String
        Get
            Return myArt.ArticleID
        End Get
    End Property

    Public Property ArticleName() As String
        Get
            Return myArt.ArticleName
        End Get
        Set(ByVal value As String)
            myArt.ArticleName = value
        End Set
    End Property
    Public Property ArticleBody() As String
        Get
            Return myArt.ArticleBody
        End Get
        Set(ByVal value As String)
            myArt.ArticleBody = value
        End Set
    End Property
    Public ReadOnly Property ArticleDescription() As String
        Get
            Return myArt.ArticleDescription
        End Get
    End Property
    Public Property ArticleModDate() As Date
        Get
            Return myArt.ArticleModDate
        End Get
        Set(ByVal value As Date)
            myArt.ArticleModDate = value
        End Set
    End Property
    Public ReadOnly Property ArticleAuthor() As String
        Get
            Return myArt.ArticleAuthor
        End Get
    End Property
    Public ReadOnly Property ArticlePageID() As String
        Get
            Return myArt.ArticlePageID
        End Get
    End Property
    Public ReadOnly Property IsArticleDefault() As Boolean
        Get
            Return myArt.IsArticleDefault
        End Get
    End Property
    Public ReadOnly Property PageName() As String
        Get
            Return myArt.PageName
        End Get
    End Property
    Public ReadOnly Property IsArticleActive() As Boolean
        Get
            Return myArt.IsArticleActive
        End Get
    End Property
    Public ReadOnly Property RowsPerPage() As Integer
        Get
            Return myArt.RowsPerPage
        End Get
    End Property
    Public Property ContactID() As String
        Get
            Return myArt.ContactID
        End Get
        Set(ByVal value As String)
            myArt.ContactID = value
        End Set
    End Property

    Private myArt As New mhArticleRow

    Public Sub New(ByRef CurrentMapRow As mhSiteMapRow, ByVal DefaultArticleID As String)
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
                        myArt.ArticleBody = " "
                    End If
                Else
                    myArt.ArticleBody = " "
                End If
        End Select
    End Sub
    Public Sub New(ByVal reqArticleID As String, ByVal reqPageID As String, ByVal DefaultArticleID As String)
        MyBase.New()
        myArt.IsArticleDefault = False

        If reqPageID = "" Then
            If reqArticleID = "" Then
                GetDefaultArticle(DefaultArticleID)
            Else
                myArt.ArticleID = reqArticleID
                If Not SetArticleByArticleID(reqArticleID) Then
                    GetDefaultArticle(DefaultArticleID)
                End If
            End If
        Else
            If Not SetArticleByPageID(reqPageID, reqArticleID, DefaultArticleID) Then
                If IsNothing(DefaultArticleID) Then
                    myArt.ArticleBody = "<h1>NO DEFAULT ARTICLE</h1>"
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
        myArt.ArticleID = SiteArticleID
        myArt.IsArticleDefault = True
        If myArt.ArticleID = "" Then
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
                For Each row As DataRow In mhDB.GetDataTable(artSQL, "mhArticle.SetArticleByArticleID").Rows
                    bFunctionStatus = True
                    myArt.ArticleID = mhUTIL.GetDBString(row.Item("ArticleID"))
                    myArt.ArticleBody = mhUTIL.GetDBString(row.Item("ArticleBody"))
                    myArt.ArticleName = mhUTIL.GetDBString(row.Item("Title"))
                    myArt.ArticleDescription = mhUTIL.GetDBString(row.Item("Description"))
                    myArt.ArticleModDate = mhUTIL.GetDBDate(row("StartDT"))
                    myArt.ArticlePageID = mhUTIL.GetDBString(row("pageid"))
                    myArt.PageName = mhUTIL.GetDBString(row("PageName"))
                    myArt.IsArticleActive = mhUTIL.GetDBBoolean(row("active"))
                    myArt.ContactID = mhUTIL.GetDBString(row("ContactID"))
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
                For Each row As DataRow In mhDB.GetDataTable(artSQL, "mhArticle.SetArticleByArticleID").Rows
                    bFunctionStatus = True
                    myArt.ArticleID = mhUTIL.GetDBString(row.Item("ArticleID"))
                    myArt.ArticleBody = FormatArticleBody(mhUTIL.GetDBString(row.Item("ArticleBody")))
                    myArt.ArticleName = mhUTIL.GetDBString(row.Item("Title"))
                    myArt.ArticleDescription = mhUTIL.GetDBString(row.Item("Description"))
                    myArt.ArticleModDate = mhUTIL.GetDBDate(row("StartDT"))
                    myArt.ArticlePageID = mhUTIL.GetDBString(row("pageid"))
                    myArt.PageName = mhUTIL.GetDBString(row("PageName"))
                    myArt.IsArticleActive = mhUTIL.GetDBBoolean(row("active"))
                    myArt.ArticleAdmin = GetArticleAdmin(myArt.ArticleID)
                    myArt.ContactID = mhUTIL.GetDBString(row("ContactID"))
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
            Dim myDT As DataTable = mhDB.GetDataTable(artSQL, "mhArticle.SetArticleByPageID")
            myArt.ArticleBody = ""
            For Each row As DataRow In myDT.Rows
                If myArt.ArticleID = reqArticleID Then
                    bFunctionStatus = True
                    myArt.ArticleID = mhUTIL.GetDBString(row.Item("ArticleID"))
                    myArt.ArticleBody = FormatArticleBody(mhUTIL.GetDBString(row.Item("ArticleBody")))
                    myArt.ArticleDescription = mhUTIL.GetDBString(row.Item("Description"))
                    myArt.ArticleName = mhUTIL.GetDBString(row.Item("Title"))
                    myArt.ArticleModDate = mhUTIL.GetDBDate(row("StartDT"))
                    myArt.ArticlePageID = mhUTIL.GetDBString(row("pageid"))
                    myArt.PageName = mhUTIL.GetDBString(row("PageName"))
                    myArt.IsArticleActive = mhUTIL.GetDBBoolean(row("active"))
                    myArt.ArticleAdmin = GetArticleAdmin(myArt.ArticleID)
                    myArt.ContactID = mhUTIL.GetDBString(row("ContactID"))
                    Exit For
                End If
            Next
            If Not (bFunctionStatus) Then
                For Each row As DataRow In myDT.Rows
                    bFunctionStatus = True
                    myArt.ArticleID = mhUTIL.GetDBString(row.Item("ArticleID"))
                    myArt.ArticleBody = FormatArticleBody(mhUTIL.GetDBString(row.Item("ArticleBody")))
                    myArt.ArticleDescription = mhUTIL.GetDBString(row.Item("Description"))
                    myArt.ArticleName = mhUTIL.GetDBString(row.Item("Title"))
                    myArt.ArticleModDate = mhUTIL.GetDBDate(row("StartDT"))
                    myArt.ArticlePageID = mhUTIL.GetDBString(row("pageid"))
                    myArt.PageName = mhUTIL.GetDBString(row("PageName"))
                    myArt.IsArticleActive = mhUTIL.GetDBBoolean(row("active"))
                    myArt.ArticleAdmin = GetArticleAdmin(myArt.ArticleID)
                    myArt.ContactID = mhUTIL.GetDBString(row("ContactID"))
                    Exit For
                Next
            End If
        End If
        If Not bFunctionStatus Then
            bFunctionStatus = SetArticleByArticleID(DefaultArticleID)
        End If
        Return bFunctionStatus
    End Function
    '********************************************************************************
    Private Function GetArticleAdmin(ByVal ArticleID As String) As String
        If mhUser.IsAdmin() Then
            If (Me.IsArticleDefault) Then
                Return ("<br /><br /><hr /><font color=""red"">No Article For Current Page -> </font><a href=""" & mhConfig.mhASPMakerGen & "article_add.aspx""> Add Article </a>")
            Else
                Return ("<br/><br /><hr /><a href=""" & mhConfig.mhASPMakerGen & "article_edit.aspx?ArticleID=" & ArticleID & """> Edit Article </a>")
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
    '********************************************************************************
    Private Function FormatArticleBody(ByRef ArticleBody As String) As String
        Dim sbArticleBody As New StringBuilder(ArticleBody)
        sbArticleBody.Append(GetArticleAdmin(Me.ArticleID))
        Return sbArticleBody.ToString
    End Function

    Public Function updateArticleBody() As Boolean
        Dim result As Boolean = False
        If Me.ArticleID = "" Then
            Throw New Exception("Missing ArticleID can not Update Article")
        Else
            Dim connection As New OleDbConnection(mhConfig.ConnStr)
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
                mhUTIL.AuditLog("mhArticle.updateArticleBody", ex.ToString)
            Finally
                connection.Close()
                connection = Nothing
            End Try
            Return result
        End If
    End Function




End Class

