Imports WebProjectMechanics

Partial Class wpmweb_wpmPopUp
    Inherits wpmPage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim myArticle As New wpmArticle(pageActiveSite.CurrentArticleID(), pageActiveSite.CurrentPageID(), pageActiveSite.DefaultArticleID)
        If myArticle.ArticleBody Is Nothing Then
            Page.Title = "Article without Template"
            myContent.Text = "Site Has No Articles"
        Else
            Page.Title = myArticle.ArticleName
            myContent.Text = pageActiveSite.ReplaceTags(myArticle.ArticleBody)
        End If
    End Sub
End Class
