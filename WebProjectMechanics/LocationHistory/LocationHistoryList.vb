
<Serializable()> Public Class LocationHistoryList
    Inherits List(Of LocationHistory)
    Public Function AddPageHistory(ByVal PageName As String) As Boolean
        Dim myPH As New LocationHistory() With {.PageName = PageName, .RequestURL = HttpContext.Current.Request.Url.AbsoluteUri}
        If myPH.RequestURL.Contains("?404;") Then
            myPH.RequestURL = myPH.RequestURL.Substring(myPH.RequestURL.LastIndexOf("?404;") + 5)
            myPH.RequestURL = myPH.RequestURL.Replace(":80", String.Empty)
        End If
        If IsNothing(HttpContext.Current.Request.UrlReferrer) Then
            myPH.PageSource = "unknown"
        Else
            myPH.PageSource = HttpContext.Current.Request.UrlReferrer.AbsoluteUri
        End If
        myPH.ViewTime = Date.Now()
        If Count = 0 Then
            If Not IsNothing(HttpContext.Current.Request.UrlReferrer) Then
                If Not HttpContext.Current.Request.UrlReferrer.AbsoluteUri.Contains(HttpContext.Current.Request.Url.Host) Then
                    ApplicationLogging.SiteReferLog("Referrer", HttpContext.Current.Request.UrlReferrer.AbsoluteUri)
                End If
            End If
        End If
        Add(myPH)
        Return True
    End Function
End Class
