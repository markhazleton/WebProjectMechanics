
'Public Class ArticleBusinessLogic
'    Private myArticle As Article = New Article
'    Public Sub New(ByRef TheArticle As Article)
'        myArticle = TheArticle
'    End Sub
'    Public Sub New(ByRef TheArticle As IArticleEdit)
'        CopyArticleEdit(TheArticle)
'    End Sub

'    Sub New(ByVal theArticleID As String)
'        myArticle = New Article(theArticleID)

'    End Sub
'    Public Sub New()
'        
'    End Sub

'    Public Function IsValid(ByRef ErrorMessage As String) As Boolean
'        Dim bReturn As Boolean
'        ' Validate Article
'        If CDbl(myArticle.ArticleID) > 0 Then
'            bReturn = True
'        Else
'            bReturn = New Boolean()
'        End If
'        ErrorMessage = String.Empty
'        bReturn = True
'        Return bReturn
'    End Function
'    Public Function UpdateArticle() As String
'        Return myArticle.UpdateArticle()
'    End Function
'    Public Function SetArticleI(ByRef returnArticle As IArticle) As IArticle
'        Return CopyArticle(myArticle, returnArticle)
'    End Function

'    Private Sub CopyArticleEdit(ByRef fromArticle As IArticleEdit)
'        myArticle.ArticleURL = fromArticle.ArticleURL
'        myArticle.ArticleID = CStr(fromArticle.ArticleID)
'        myArticle.ArticleName = fromArticle.ArticleName
'        myArticle.ArticleSummary = fromArticle.ArticleSummary
'        myArticle.ArticleDescription = fromArticle.ArticleDescription
'        myArticle.ArticleBody = fromArticle.ArticleBody
'        myArticle.ArticleModDate = fromArticle.ArticleModDate
'        myArticle.ArticlePageID = fromArticle.ArticlePageID
'        myArticle.IsArticleDefault = fromArticle.IsArticleDefault
'        myArticle.PageName = fromArticle.PageName
'        myArticle.IsArticleActive = fromArticle.IsArticleActive
'        myArticle.RowsPerPage = fromArticle.RowsPerPage
'        myArticle.ArticleAdmin = fromArticle.ArticleAdmin
'        myArticle.ArticleAuthor = fromArticle.ArticleAuthor
'        myArticle.ContactID = fromArticle.ContactID
'        myArticle.CompanyID = fromArticle.CompanyID
'    End Sub
'    Private Shared Function CopyArticle(ByRef fromArticle As Article, ByRef toArticle As IArticle) As IArticle
'        toArticle.ArticleURL = fromArticle.ArticleURL
'        toArticle.ArticleID = CStr(fromArticle.ArticleID)
'        toArticle.ArticleName = fromArticle.ArticleName
'        toArticle.ArticleSummary = fromArticle.ArticleSummary
'        toArticle.ArticleDescription = fromArticle.ArticleDescription
'        toArticle.ArticleBody = fromArticle.ArticleBody
'        toArticle.ArticleModDate = fromArticle.ArticleModDate
'        toArticle.ArticlePageID = fromArticle.ArticlePageID
'        toArticle.IsArticleDefault = fromArticle.IsArticleDefault
'        toArticle.PageName = fromArticle.PageName
'        toArticle.IsArticleActive = fromArticle.IsArticleActive
'        toArticle.RowsPerPage = fromArticle.RowsPerPage
'        toArticle.ArticleAdmin = fromArticle.ArticleAdmin
'        toArticle.ArticleAuthor = fromArticle.ArticleAuthor
'        toArticle.ContactID = fromArticle.ContactID
'        toArticle.CompanyID = fromArticle.CompanyID
'        Return toArticle
'    End Function

'End Class
