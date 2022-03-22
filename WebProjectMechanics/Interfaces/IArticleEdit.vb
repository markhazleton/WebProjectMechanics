
Public Interface IArticleEdit
    Event ArticleEditCancel()
    Event ArticleEditSave()
    ReadOnly Property ArticleURL As String
    ReadOnly Property ArticleID As Integer
    ReadOnly Property ArticleName() As String
    ReadOnly Property ArticleSummary() As String
    ReadOnly Property ArticleDescription() As String
    ReadOnly Property ArticleBody() As String
    ReadOnly Property ArticleModDate() As DateTime
    ReadOnly Property ArticlePageID() As String
    ReadOnly Property IsArticleDefault() As Boolean
    ReadOnly Property PageName() As String
    ReadOnly Property IsArticleActive() As Boolean
    ReadOnly Property RowsPerPage() As Integer
    ReadOnly Property ArticleAuthor() As String
    ReadOnly Property ArticleAdmin() As String
    ReadOnly Property ContactID() As String
    ReadOnly Property CompanyID As String
    WriteOnly Property UpdateComplete As Boolean
    WriteOnly Property UpdateError As String
    Sub SetArticle(ByVal ArticleID As String)

End Interface
