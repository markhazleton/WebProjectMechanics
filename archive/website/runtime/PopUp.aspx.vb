Imports WebProjectMechanics

Partial Class ProjectMechanicsPopUp
    Inherits ApplicationPage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim myCompany As New ActiveCompany()
        Dim myArticle As New Article(wpm_CurrentArticleID(), myCompany.CurrentLocationID(), myCompany.DefaultArticleID)
        If myArticle.ArticleBody Is Nothing Then
            Page.Title = "Article without Template"
            myContent.Text = "Site Has No Articles"
        Else
            Page.Title = myArticle.ArticleName
            myContent.Text = myCompany.ReplaceTags(myArticle.ArticleBody, myCompany.CurLocation)
        End If
    End Sub
End Class
