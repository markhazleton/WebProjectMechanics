
Public Interface IArticle
    Property ArticleID() As Integer
    Property ArticleURL() As String
    Property ArticleName() As String
    Property ArticleSummary() As String
    Property ArticleDescription() As String
    Property ArticleBody() As String
    Property ArticleModDate() As DateTime
    Property ArticlePageID() As String
    Property IsArticleDefault() As Boolean
    Property PageName() As String
    Property IsArticleActive() As Boolean
    Property RowsPerPage() As Integer
    Property ArticleAuthor() As String
    Property ArticleAdmin() As String
    Property ContactID() As String
    Property CompanyID As String
End Interface
