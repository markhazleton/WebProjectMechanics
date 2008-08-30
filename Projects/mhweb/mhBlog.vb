Public Class mhBlog
    Private myArticleRows As mhArticleRows
    Private curPageID As String
    Private curArticleID As String
    Private curRecordsPerPage As Integer = 3
    Private siteDefaultArticleID As String

    Sub New(ByRef mySiteMap As mhSiteMap)
        myArticleRows = New mhArticleRows(mySiteMap.CurrentMapRow.PageID, "mhBlog-" & mySiteMap.CurrentMapRow.PageID)
        curPageID = mySiteMap.CurrentMapRow.PageID
        curArticleID = mySiteMap.CurrentMapRow.ArticleID
        siteDefaultArticleID = mySiteMap.mySiteFile.DefaultArticleID
        If myArticleRows.Count > 0 Then
            curRecordsPerPage = myArticleRows.Item(0).RowsPerPage
        Else
            curRecordsPerPage = 999
        End If
    End Sub
    Sub New(ByVal PageID As String, ByVal ArticleID As String, ByVal DefaultArticleID As String)
        myArticleRows = New mhArticleRows(PageID, "mhBlog-" & PageID)
        curPageID = PageID
        curArticleID = ArticleID
        siteDefaultArticleID = DefaultArticleID
    End Sub

    Public Function GetBlogPosts(ByRef sbBlogTemplate As StringBuilder) As String
        Dim sItem As New StringBuilder
        Dim sWeblogRSS As String = ("http://" & HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME") & "" & mhConfig.mhWebHome & "blog/blog_rss.aspx?c=" & curPageID)
        If (curArticleID = "") Or (curArticleID = siteDefaultArticleID) Then
            sItem.Append("<div class=""blog"">")
            sItem.Append("<div class=""blog-posts"">")
            For Each myArticle As mhArticleRow In myArticleRows
                ' Build The Blog Entry
                sItem.Append(sbBlogTemplate.ToString)
                sItem.Replace("~~PostID~~", myArticle.ArticleID)
                sItem.Replace("~~PostDate~~", myArticle.ArticleModDate.ToString)
                sItem.Replace("~~PostShortDate~~", myArticle.ArticleModDate.ToShortDateString)
                sItem.Replace("~~PostURL~~", mhUTIL.FormatPageNameURL(myArticle.ArticleName))
                sItem.Replace("~~PostName~~", myArticle.ArticleName)
                sItem.Replace("~~PostTitle~~", myArticle.ArticleName)
                sItem.Replace("~~PostBody~~", myArticle.ArticleBody)
                sItem.Replace("~~PostDescription~~", myArticle.ArticleDescription)
                sItem.Replace("~~BlogPageURL~~", mhUTIL.FormatPageNameURL(myArticle.PageName))
                sItem.Replace("~~BlogPageName~~", myArticle.PageName)
                sItem.Replace("~~PostAuthor~~", myArticle.ArticleAuthor)
                sItem.Replace("~~ArticleAdmin~~", myArticle.ArticleAdmin)
            Next
            sItem.Append("</div>")
            sItem.Append("</div>")
        Else
            For Each myArticle As mhArticleRow In myArticleRows
                If myArticle.ArticleID = curArticleID Then
                    sItem.Append(sbBlogTemplate.ToString)
                    sItem.Replace("~~PostID~~", myArticle.ArticleID)
                    sItem.Replace("~~PostDate~~", myArticle.ArticleModDate.ToString)
                    sItem.Replace("~~PostShortDate~~", myArticle.ArticleModDate.ToShortDateString)
                    sItem.Replace("~~PostURL~~", mhUTIL.FormatPageNameURL(myArticle.ArticleName))
                    sItem.Replace("~~PostTitle~~", myArticle.ArticleName)
                    sItem.Replace("~~PostName~~", myArticle.ArticleName)
                    sItem.Replace("~~PostBody~~", myArticle.ArticleBody)
                    sItem.Replace("~~PostDescription~~", myArticle.ArticleDescription)
                    sItem.Replace("~~BlogPageURL~~", mhUTIL.FormatPageNameURL(myArticle.PageName))
                    sItem.Replace("~~BlogPageName~~", myArticle.PageName)
                    sItem.Replace("~~PostAuthor~~", myArticle.ArticleAuthor)
                    sItem.Replace("~~ArticleAdmin~~", myArticle.ArticleAdmin)
                    Exit For
                End If
            Next
        End If
        Return sItem.ToString
    End Function

    Public Function BuildBlogList(ByRef sbBlogTemplate As StringBuilder, ByVal iCurrentPageNumber As Integer) As String
        Dim sItem As New StringBuilder
        Dim sURL As String = ("/mhweb/blog/mhBlog.aspx?c=" & curPageID & "&amp;a=" & curArticleID)
        Dim sSubPageNav As String = ("")
        Dim iPostCount As Integer = 0
        Dim iPageCount As Integer = 0
        Dim iFirstDisplay As Integer = 0
        Dim iLastDisplay As Integer = 0
        Dim iPageNumber As Integer = 0

        Dim iRow As Integer = 0
        If (curArticleID = "") Or (curArticleID = siteDefaultArticleID) Then

            If myArticleRows.Count = 0 Then
                ' No Rows
            Else
                ' Count number of Images for this PageID, SETUP FOR REMAINDER OF TASKS
                ' determine page break
                If (curRecordsPerPage < 1) Then
                    curRecordsPerPage = 1
                End If
                iPageCount = CInt(Math.Round(Val(myArticleRows.Count / curRecordsPerPage), 0))
                ' If last image is on a page break, then subtract one page to avoid an empty page
                If (iPageCount * curRecordsPerPage) > myArticleRows.Count Then
                    ' Do nothing we have the right number of pages
                Else
                    If (myArticleRows.Count Mod curRecordsPerPage) = 0 Then
                    Else
                        iPageCount = iPageCount + 1
                    End If
                End If
                ' Determine First and Last record to display
                If myArticleRows.Count > 0 Then
                    If iCurrentPageNumber <= 1 Then
                        ' We are on the first page
                        iPageNumber = 1
                        iFirstDisplay = 0
                        iLastDisplay = curRecordsPerPage - 1
                    Else
                        iPageNumber = iCurrentPageNumber
                        iFirstDisplay = ((iPageNumber * curRecordsPerPage) - curRecordsPerPage)
                        iLastDisplay = iFirstDisplay + curRecordsPerPage - 1
                    End If
                End If
                ' Build Sub Page Navigation
                If iPageNumber > 1 Then
                    sSubPageNav = sSubPageNav & "<a title=""PREVIOUS"" href=""" & sURL & "&amp;Page=" & iPageNumber - 1 & """><::</a>"
                End If
                If (iPageCount > 1) Then
                    sSubPageNav = sSubPageNav & "<b>Page " & iPageNumber & " of " & iPageCount & "</b>"
                End If
                If iPageNumber < iPageCount Then
                    sSubPageNav = sSubPageNav & "<a title=""NEXT"" href=""" & sURL & "&amp;Page=" & iPageNumber + 1 & """>::></a>"
                End If
                If sSubPageNav <> "" Then
                    sItem.Append("<center>" & sSubPageNav & "</center>" & vbCrLf & vbCrLf)
                End If
                ' Draw The current page

                For Each myArticle As mhArticleRow In myArticleRows
                    ' Determine if this record is displayed
                    If (iRow >= iFirstDisplay) Then
                        If (iRow <= iLastDisplay) Then
                            ' Build The Blog Entry
                            sItem.Append(sbBlogTemplate.ToString)
                            sItem.Replace("~~PostID~~", myArticle.ArticleID)
                            sItem.Replace("~~PostDate~~", myArticle.ArticleModDate.ToString)
                            sItem.Replace("~~PostShortDate~~", myArticle.ArticleModDate.ToString("m"))
                            sItem.Replace("~~PostURL~~", mhUTIL.FormatPageNameURL(myArticle.ArticleName))
                            sItem.Replace("~~PostName~~", myArticle.ArticleName)
                            sItem.Replace("~~PostTitle~~", myArticle.ArticleName)
                            sItem.Replace("~~PostBody~~", myArticle.ArticleBody)
                            sItem.Replace("~~PostDescription~~", myArticle.ArticleDescription)
                            sItem.Replace("~~BlogPageURL~~", mhUTIL.FormatPageNameURL(myArticle.PageName))
                            sItem.Replace("~~BlogPageName~~", myArticle.PageName)
                            sItem.Replace("~~PostAuthor~~", myArticle.ArticleAuthor)
                            sItem.Replace("~~ArticleAdmin~~", myArticle.ArticleAdmin)
                        End If
                    End If
                    iRow = iRow + 1
                Next
            End If
            sItem.Append("<center>" & sSubPageNav & "</center>" & vbCrLf & vbCrLf)
            sItem.Append(vbCrLf)
        Else
            For Each myArticle As mhArticleRow In myArticleRows
                If myArticle.ArticleID = curArticleID Then
                    sItem.Append(sbBlogTemplate.ToString)
                    sItem.Replace("~~PostID~~", myArticle.ArticleID)
                    sItem.Replace("~~PostDate~~", myArticle.ArticleModDate.ToString)
                    sItem.Replace("~~PostShortDate~~", myArticle.ArticleModDate.ToString("m"))
                    sItem.Replace("~~PostURL~~", mhUTIL.FormatPageNameURL(myArticle.ArticleName))
                    sItem.Replace("~~PostName~~", myArticle.ArticleName)
                    sItem.Replace("~~PostTitle~~", myArticle.ArticleName)
                    sItem.Replace("~~PostBody~~", myArticle.ArticleBody)
                    sItem.Replace("~~PostDescription~~", myArticle.ArticleDescription)
                    sItem.Replace("~~BlogPageURL~~", mhUTIL.FormatPageNameURL(myArticle.PageName))
                    sItem.Replace("~~BlogPageName~~", myArticle.PageName)
                    sItem.Replace("~~PostAuthor~~", myArticle.ArticleAuthor)
                    sItem.Replace("~~ArticleAdmin~~", myArticle.ArticleAdmin)
                    Exit For
                End If
            Next
        End If
        Return sItem.ToString
    End Function

End Class