
'Public Class ArticleEditControllor
'    Private ReadOnly myArticleEdit As IArticleEdit
'    Private ReadOnly myArticleDisplay As IArticleDisplay
'    Private myArticle As IArticle

'    Public Sub New(myView As Object)
'        myArticleDisplay = TryCast(myView, IArticleDisplay)
'        myArticleEdit = TryCast(myView, IArticleEdit)
'        myArticle = TryCast(myView, IArticle)
'    End Sub
'    Public Sub New()
'        
'    End Sub

'    Public Sub SaveArticle()
'        Dim myArticleBL As ArticleBusinessLogic = New ArticleBusinessLogic(myArticleEdit)
'        Dim ErrorMessage As String = String.Empty
'        If myArticleBL.IsValid(ErrorMessage) Then
'            ErrorMessage = myArticleBL.UpdateArticle()
'            If ErrorMessage = String.Empty Then
'                myArticleEdit.UpdateComplete = True
'            Else
'                myArticleEdit.UpdateError = ErrorMessage
'            End If
'        Else
'            myArticleEdit.UpdateError = ErrorMessage
'        End If
'    End Sub

'    Public Sub GetArticle(ByVal theArticleID As String)
'        If Not myArticleDisplay Is Nothing Then
'        End If
'        If Not myArticle Is Nothing Then
'            myArticle = New ArticleBusinessLogic(theArticleID).SetArticleI(myArticle)
'        End If
'    End Sub

'    Public Sub GetArticles(ByRef myActiveSite As ActiveCompany)
'        myArticleDisplay.ArticleList = New ArticleList(myActiveSite)
'    End Sub

'End Class
