Imports WebProjectMechanics

Public Class ProjectMechanics_Default
    Inherits ApplicationPage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        wpm_CurrentArticleID = wpm_GetProperty("a", masterPage.myCompany.DefaultArticleID)
        wpm_CurrentPageID = wpm_GetProperty("c", masterPage.myCompany.SiteHomePageID)
        Dim myRedirect = masterPage.myCompany.LocationAliasList.LookupTargetURL("*")
        If myRedirect <> String.Empty Then
            wpm_Build301Redirect(myRedirect)
        Else
            Dim mySearchKeyword As String = wpm_GetProperty("searchfield", String.Empty)
            If mySearchKeyword <> String.Empty Then
                Dim sRedirectURL As String = masterPage.myCompany.LocationList.GetLocationURLByKeyword(mySearchKeyword)
                If sRedirectURL <> String.Empty Then
                    Response.Redirect(sRedirectURL)
                End If
            End If
            masterPage.myCompany.SetCurrentPageID(wpm_CurrentPageID)
            masterPage.myCompany.SelectCurrentPageRow(wpm_CurrentPageID, wpm_CurrentArticleID)
            masterPage.myCompany.UseDefaultTemplate = True

            Dim myImageList As New LocationImageList(masterPage.myCompany)
            Dim myArticle As New Article(masterPage.myCompany.CurLocation, masterPage.myCompany.DefaultArticleID)
            litMain.Text = myImageList.ProcessPageRequest(masterPage.myCompany.CurLocation.ArticleID, myArticle)
        End If
    End Sub

End Class
