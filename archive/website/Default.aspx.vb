Imports WebProjectMechanics

Public Class ProjectMechanics_Default
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim curCompany As New ActiveCompany()

        wpm_CurrentArticleID = wpm_GetProperty("a", curCompany.DefaultArticleID)
        wpm_CurrentPageID = wpm_GetProperty("c", curCompany.SiteHomePageID)
        Dim mySearchKeyword As String = wpm_GetProperty("searchfield", String.Empty)
        If mySearchKeyword <> String.Empty Then
            Dim sRedirectURL As String = curCompany.LocationList.GetLocationURLByKeyword(mySearchKeyword)
            If sRedirectURL <> String.Empty Then
                Response.Redirect(sRedirectURL)
            End If
        End If
        If wpm_CurrentPageID = "" AndAlso wpm_CurrentArticleID < 1 Then
            Response.Write(curCompany.GetHTML(String.Empty, True, String.Empty))
        Else
            curCompany.SetCurrentPageID(wpm_CurrentPageID)
            curCompany.SelectCurrentPageRow(wpm_CurrentPageID, wpm_CurrentArticleID)
            curCompany.UseDefaultTemplate = True
            wpm_ListPageURL = "/"
            curCompany.WriteCurrentLocation()
        End If
        'If Not String.IsNullOrEmpty(myCompany.CurLocation.BreadCrumbURL) Then
        '     wpm_Build301Redirect(myCompany.CurLocation.BreadCrumbURL)
        'End If

    End Sub

End Class
