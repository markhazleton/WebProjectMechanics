Partial Class wpm_site_history
    Inherits wpmPage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sbMainContent As StringBuilder = New StringBuilder()
        Dim sbPageHistory As StringBuilder = New StringBuilder
        Dim sbArticleHistory As StringBuilder = New StringBuilder
        Dim sbImageHistory As StringBuilder = New StringBuilder
        Dim dFrom As Date
        Dim dTo As Date
        'Set the Display Dates
        Dim dRightNow As DateTime = DateTime.Now
        dFrom = dRightNow.AddDays(-900)
        dTo = dRightNow
        Dim myDateComp As LocationModifiedDateCompare = New LocationModifiedDateCompare()
        myDateComp.Direction = wpmSortDirection.DESC
        pageActiveSite.LocationList.Sort(myDateComp)

        For Each myrow As wpmLocation In pageActiveSite.LocationList
            Select Case myrow.RecordSource
                Case "Page"
                    If myrow.ModifiedDate > dFrom Then
                        sbPageHistory.Append("<li><a href=""" & wpmUTIL.FormatPageNameURL(myrow.PageName) & """>" & myrow.PageName & "</a>" & "(" & myrow.DefaultOrder & ") - Modified " & _
                myrow.ModifiedDate.ToLongDateString & "</li>")
                    End If
                Case "Article"
                    If myrow.ModifiedDate > dFrom Then
                        sbArticleHistory.Append("<li><a href=""" & wpmUTIL.FormatPageNameURL(myrow.PageName) & """>" & myrow.PageName & "</a> - Modified " & _
                myrow.ModifiedDate.ToLongDateString & "</li>")
                    End If
                Case "Image"
                    If myrow.ModifiedDate > dFrom Then
                        sbImageHistory.Append("<li><a href=""" & wpmUTIL.FormatPageNameURL(myrow.PageName) & """>" & myrow.PageName & "</a> - Modified " & _
                myrow.ModifiedDate.ToLongDateString & "</li>")
                    End If
            End Select
        Next
        pageActiveSite.LocationList.DefaultSort()

        sbMainContent.Append("Changes to " & pageActiveSite.SiteTitle & "<br />From " & dFrom.ToShortDateString & " to " & dTo.ToShortDateString & "<br />")
        sbMainContent.Append("<h1>Pages</h1><ul>" & sbPageHistory.ToString & "</ul>")
        sbMainContent.Append("<h1>Articles</h1><ul>" & sbArticleHistory.ToString & "</ul>")
        sbMainContent.Append("<h1>Images</h1><ul>" & sbImageHistory.ToString & "</ul>")
        Label1.Text = sbMainContent.ToString



    End Sub
End Class
