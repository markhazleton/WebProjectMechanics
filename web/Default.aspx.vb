Partial Class _Default
    Inherits wpmPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("CurrentArticleID") = ""
        Session("CurrentPageID") = ""

        Dim mySearchKeyword As String = GetProperty("searchfield", String.Empty)
        If mySearchKeyword <> String.Empty Then
            Dim sRedirectURL As String
            sRedirectURL = pageActiveSite.LocationList.GetLocationURLByKeyword(mySearchKeyword)
            If sRedirectURL <> String.Empty Then
                Response.Redirect(sRedirectURL)
            End If
        End If
        If App.Config.FullLoggingOn Then
            wpmLog.AccessLog("Default Page", Request.ServerVariables.Item("QUERY_STRING"))
        End If
        pageActiveSite.UseDefaultTemplate = True
        pageActiveSite.WriteCurrentLocation()


    End Sub
End Class
