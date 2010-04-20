Imports System.Web

Public Class wpmAdmin
    Private ActiveSite As wpmActiveSite
    Private ArticleList As wpmArticleList
    Private ImageList As wpmSiteImageList
    Sub New(ByVal thisSession As System.Web.SessionState.HttpSessionState)
        ActiveSite = New wpmActiveSite(thisSession)
        ArticleList = New wpmArticleList(ActiveSite.CompanyName)
        ImageList = New wpmSiteImageList(ActiveSite.CompanyID)
    End Sub
    Public Function SiteImageReport() As String
        Dim sbReturn As New StringBuilder(String.Empty)
        sbReturn.Append("<table class=""sortable autostripe "">")
        sbReturn.Append("<thead><tr><th>Image</th><th>Details</th></tr></thead>")
        For Each myRow As wpmLocation In ActiveSite.LocationList
            If myRow.RecordSource = "Image" Then
                sbReturn.Append("<tr>")
                sbReturn.Append("<td style=""whitespace:nobreak;""><a href=""/wpmgen/image_edit.aspx?ImageID=" & myRow.ArticleID & """>" & myRow.PageName & "</a></td>")
                sbReturn.Append("<td>" & GetImageDetail(myRow.ArticleID) & "</td>")
                sbReturn.Append("</tr>")
                sbReturn.Append("<tr><td ""colspan=""2""><div style=""page-break-after: always;""><span style=""display: none;"">&nbsp;</span></div></td></tr>")
            End If
        Next
        sbReturn.Append("</table>")
        Return sbReturn.ToString
    End Function
    Public Function SiteReport() As String
        Dim sbReturn As New StringBuilder(String.Empty)
        sbReturn.Append("<table class=""sortable autostripe "">")
        sbReturn.Append("<thead><tr><th>Page</th><th>Content</th></tr></thead>")
        For Each myRow As wpmLocation In ActiveSite.LocationList
            If myRow.RecordSource = "Page" Or myRow.RecordSource = "Category" Then
                sbReturn.Append("<tr>")
                sbReturn.Append("<td style=""whitespace:nobreak;"">" & myRow.PageName & "</td>")
                sbReturn.Append("<td>" & GetArticleBody(myRow.PageID) & "</td>")
                sbReturn.Append("</tr>")
                sbReturn.Append("<tr><td ""colspan=""2""><div style=""page-break-after: always;""><span style=""display: none;"">&nbsp;</span></div></td></tr>")
            End If
        Next
        sbReturn.Append("</table>")
        Return sbReturn.ToString
    End Function
    Private Function GetImageDetail(ByVal ImageID As String) As String
        Dim sbReturn As New StringBuilder(String.Empty)
        For Each myImage As wpmImage In ImageList
            If myImage.ImageID = ImageID Then
                sbReturn.Append("<strong>" & myImage.ImageName & " (" & myImage.ImageID & ")</strong><br/>")
                sbReturn.Append("" & myImage.ImageDescription & "<br/>")
                sbReturn.Append("<img src=""/wpm/catalog/ImageResize.aspx?img=" & ActiveSite.SiteGallery & myImage.ImageFileName & "&w=200"" />")
            End If
        Next
        Return sbReturn.ToString
    End Function

    Private Function GetArticleBody(ByVal PageID As String) As String
        Dim sbReturn As New StringBuilder(String.Empty)

        For Each myArticle As wpmArticle In ArticleList
            If myArticle.ArticlePageID = PageID Then
                sbReturn.Append("<strong>" & myArticle.ArticleName & " (" & myArticle.ArticlePageID & " - " & myArticle.ArticleID & ")</strong")
                sbReturn.Append("" & myArticle.ArticleBody & "")
            End If
        Next
        Return sbReturn.ToString
    End Function
    Public Function BuildAdmin() As String
        Dim RowCount As Integer = 0
        Dim myStringBuilder As StringBuilder = New StringBuilder()
        ' Set the ListPageURL so the edit,delete, add, pages know where to come back to
        HttpContext.Current.Session("ListPageURL") = HttpContext.Current.Request.FilePath()
        myStringBuilder.Append(BuildAdminHeader())
        myStringBuilder.Append(BuildPageStructure("", 0, RowCount))
        myStringBuilder.Append("<tr><td colspan=""2"" class=""ewTableHeader""><h2>Articles without Pages</h2></td></tr>")
        myStringBuilder.Append(BuildArticleList())
        myStringBuilder.Append("<tr><td colspan=""2"" class=""ewTableHeader""><h2>Site Category</h2></td></tr>")
        myStringBuilder.Append(BuildCategoryStructure("", 0, RowCount))
        myStringBuilder.Append("</table>")
        Dim sbAdminDHTML As New StringBuilder
        wpmFileIO.ReadTextFile(HttpContext.Current.Server.MapPath("/wpm/admin/adminDHTML.html"), sbAdminDHTML)
        myStringBuilder.Append(sbAdminDHTML.ToString)
        Return (myStringBuilder.ToString)
    End Function

    Private Function AdminFormatLink(ByVal LinkID As String, ByVal LinkName As String, ByVal LinkType As String, ByVal LinkURL As String) As String
        Dim sReturn As String = ""
        If LinkID <> "" Then
            Select Case LinkType
                Case "Page"
                    sReturn = "<a class=""linkmenu"" href="""" onclick=""createPopUp(200,110,'" & LinkID & "','Page',this);return false"" id=Page" & LinkID & " >" & LinkName & "</a>"
                Case "Article"
                    sReturn = "<a class=""linkmenu"" href="""" onclick=""createPopUp(200,90,'" & LinkID & "','Article',this);return false"" id=Article" & LinkID & " >" & LinkName & "</a>"
                Case "Company"
                    sReturn = "<a class=""linkmenu"" href="""" onclick=""createPopUp(200,110,'" & LinkID & "','Company',this);return false"" id=Company" & LinkID & " >" & LinkName & "</a>"
                Case "Category"
                    sReturn = LinkName
            End Select
        End If
        '        Return wpmUTIL.FormatLink(LinkID, LinkName, LinkType, LinkURL)
        Return sReturn
    End Function

    Private Function FormatLink(ByVal LinkID As Integer, ByVal LinkName As String, ByVal LinkType As String, ByVal LinkURL As String) As String
        Dim sReturn As String
        Select Case LinkType
            Case "Contact"
                sReturn = LinkName
            Case Else
                sReturn = "<a href=""" & LinkURL & """>" & LinkName & "</a>"
        End Select
        FormatLink = sReturn
    End Function
    Private Function BuildPageStructure(ByVal ParentID As String, ByVal intLevel As Integer, ByRef iRowCount As Integer) As String
        Dim myRow As wpmLocation
        Dim j As Integer
        Dim sReturn As New StringBuilder
        Dim sRowClass As String
        For j = 0 To ActiveSite.LocationList.Count - 1
            Dim sCellContent As New StringBuilder
            myRow = ActiveSite.LocationList.Item(j)
            If ParentID = myRow.ParentPageID And myRow.RecordSource = "Page" Then
                If CBool(iRowCount Mod 2) Then
                    sRowClass = "ewTableAltRow"
                Else
                    sRowClass = "ewTableRow"
                End If
                iRowCount = iRowCount + 1
                sCellContent.Append("<img src=""/wpm/images/spacer.gif"" alt="""" width=""" & intLevel * 15 & """ height=""1"" />")
                If Not myRow.ActiveFL Then
                    sCellContent.Append("(i) ")
                End If
                sCellContent.Append(AdminFormatLink(myRow.PageID, myRow.PageName, "Page", myRow.TransferURL) & "&nbsp;(")
                sCellContent.Append(wpmUTIL.FormatLink(myRow.PageID, "Edit", "Page", App.Config.AspMakerGen & "zpage_edit.aspx?zPageID=" & myRow.PageID, "edit") & "&nbsp;")
                sCellContent.Append(wpmUTIL.FormatLink(myRow.PageID, "Delete", "Page", App.Config.AspMakerGen & "zpage_delete.aspx?zPageID=" & myRow.PageID, "delete") & "&nbsp;")
                sCellContent.Append(wpmUTIL.FormatLink(myRow.PageID, "Copy", "Page", App.Config.AspMakerGen & "zpage_add.aspx?zPageID=" & myRow.PageID, "add") & "&nbsp;)")
                sReturn.Append("<tr class=""" & sRowClass & """>" & vbCrLf & "<td NOWRAP class=""" & sRowClass & """ valign=""top"">" & sCellContent.ToString & vbCrLf & "</td>" & vbCrLf & "<td NOWRAP class=""" & sRowClass & """ valign=""top"">" & vbCrLf)
                sReturn.Append(BuildPageArticleList(myRow.PageID, myRow.TransferURL))
                sReturn.Append("</td>" & vbCrLf & "</tr>" & vbCrLf)
                sReturn.Append(BuildPageStructure(myRow.PageID, intLevel + 1, iRowCount))
            End If
        Next
        Return sReturn.ToString
    End Function

    Private Function BuildCategoryStructure(ByVal ParentID As String, ByVal intLevel As Integer, ByRef iRowCount As Integer) As String
        Dim myRow As wpmLocation
        Dim j As Integer
        Dim sReturn As New StringBuilder
        Dim sRowClass As String
        Dim sSiteCategoryID As String
        For j = 0 To ActiveSite.LocationList.Count - 1
            Dim sCellContent As New StringBuilder
            myRow = ActiveSite.LocationList.Item(j)
            If ParentID = myRow.ParentPageID And myRow.RecordSource = "Category" Then
                If CBool(iRowCount Mod 2) Then
                    sRowClass = "ewTableAltRow"
                Else
                    sRowClass = "ewTableRow"
                End If
                iRowCount = iRowCount + 1
                sCellContent.Append("<img src=""/wpm/images/spacer.gif"" alt="""" width=""" & intLevel * 15 & """ height=""2"" />")
                If Not myRow.ActiveFL Then
                    sCellContent.Append("(i) ")
                End If
                sSiteCategoryID = Replace(myRow.PageID, "CAT-", "")
                sCellContent.Append(AdminFormatLink(myRow.PageID, myRow.PageName, "Category", myRow.TransferURL) & "&nbsp;(")
                sCellContent.Append(wpmUTIL.FormatLink(sSiteCategoryID, "Edit", "Category", App.Config.AspMakerGen & "sitecategory_edit.aspx?SiteCategoryID=" & sSiteCategoryID, "edit") & "&nbsp;")
                sCellContent.Append(wpmUTIL.FormatLink(sSiteCategoryID, "Delete", "Category", App.Config.AspMakerGen & "sitecategory_delete.aspx?SiteCategoryID=" & sSiteCategoryID, "delete") & "&nbsp;")
                sCellContent.Append(wpmUTIL.FormatLink(sSiteCategoryID, "Copy", "Category", App.Config.AspMakerGen & "sitecategory_add.aspx?SiteCategoryID=" & sSiteCategoryID, "add") & "&nbsp;)")
                sReturn.Append("<tr class=""" & sRowClass & """>" & vbCrLf & "<td NOWRAP class=""" & sRowClass & """ valign=""top"">" & sCellContent.ToString & vbCrLf & "</td>" & vbCrLf & "<td NOWRAP class=""" & sRowClass & """ valign=""top"">" & vbCrLf)
                sReturn.Append(BuildCategoryPageList(myRow.SiteCategoryID) & "&nbsp;</td>" & vbCrLf & "</tr>" & vbCrLf)
                sReturn.Append(BuildCategoryStructure(myRow.PageID, intLevel + 1, iRowCount))
            End If
        Next
        Return sReturn.ToString
    End Function
    Private Function BuildCategoryPageList(ByVal SiteCategoryID As String) As String
        Dim myRow As wpmLocation
        Dim j As Integer
        Dim sReturn As New StringBuilder
        For j = 0 To ActiveSite.LocationList.Count - 1
            Dim sCellContent As New StringBuilder
            myRow = ActiveSite.LocationList.Item(j)
            If SiteCategoryID = myRow.SiteCategoryID And myRow.RecordSource = "Page" Then
                sCellContent.Append(AdminFormatLink(myRow.PageID, myRow.PageName, "Page", myRow.TransferURL) & "&nbsp;(")
                sCellContent.Append(wpmUTIL.FormatLink(myRow.PageID, "Edit", "Page", App.Config.AspMakerGen & "zpage_edit.aspx?zPageID=" & myRow.PageID, "edit") & "&nbsp;")
                sCellContent.Append(wpmUTIL.FormatLink(myRow.PageID, "Delete", "Page", App.Config.AspMakerGen & "zpage_delete.aspx?zPageID=" & myRow.PageID, "delete") & "&nbsp;")
                sCellContent.Append(wpmUTIL.FormatLink(myRow.PageID, "Copy", "Page", App.Config.AspMakerGen & "zpage_add.aspx?zPageID=" & myRow.PageID, "add") & "&nbsp;)")
                If Not myRow.ActiveFL Then
                    sCellContent.Append("(i) ")
                End If
                sReturn.Append(sCellContent.ToString & "<br/>" & vbCrLf)
            End If
        Next
        Return sReturn.ToString
    End Function

    Private Function BuildPageList() As String
        Dim myrow As wpmLocation
        Dim sbCellContent As New StringBuilder("")
        Dim j As Integer = 0
        Dim sReturn As String = ""
        Dim sRowClass As String = ""
        For j = 0 To ActiveSite.LocationList.Count - 1
            myrow = ActiveSite.LocationList.Item(j)
            If myrow.RecordSource = "Page" Then
                sbCellContent.Append("<img src=""/wpm/images/spacer.gif"" alt="""" width=""15"" height=""1"" />")
                sbCellContent.Append(wpmUTIL.FormatLink(myrow.PageID, myrow.PageName, "Page", myrow.TransferURL) & "&nbsp;(")
                sbCellContent.Append(wpmUTIL.FormatLink(myrow.PageID, "Edit", "Page", App.Config.AspMakerGen & "zpage_edit.aspx?zPageID=" & myrow.PageID, "edit") & "&nbsp;")
                sbCellContent.Append(wpmUTIL.FormatLink(myrow.PageID, "Delete", "Page", App.Config.AspMakerGen & "zpage_delete.aspx?zPageID=" & myrow.PageID, "delete") & "&nbsp;")
                sbCellContent.Append(wpmUTIL.FormatLink(myrow.PageID, "Copy", "Page", App.Config.AspMakerGen & "zpage_add.aspx?zPageID=" & myrow.PageID, "add") & "&nbsp;)")
                '
                sReturn = sReturn & ("<tr class=""" & sRowClass & """>" & vbCrLf)
                sReturn = sReturn & (vbTab & "<td NOWRAP class=""" & sRowClass & """ valign=""top"">" & sbCellContent.ToString & "&nbsp;</td>" & vbCrLf)
                sReturn = sReturn & (vbTab & "<td NOWRAP class=""" & sRowClass & """ valign=""top"">")
                sReturn = sReturn & myrow.BreadCrumbHTML
                sReturn = sReturn & ("</td>" & vbCrLf & "</tr>" & vbCrLf)
            End If
        Next
        Return sReturn
    End Function

    Private Function BuildPageArticleList(ByVal pageID As String, ByVal LinkURL As String) As String
        Dim myrow As wpmArticle
        Dim sReturn As New StringBuilder
        Dim j As Integer
        For j = 0 To ArticleList.Count - 1
            myrow = ArticleList.Item(j)
            If (myrow.ArticlePageID = pageID) Then
                If Not (myrow.IsArticleActive) Then
                    sReturn.Append("(i) ")
                End If
                sReturn.Append(AdminFormatLink(myrow.ArticleID, myrow.PageName, "Article", myrow.ArticleName) & _
                    " (" & wpmUTIL.FormatLink(myrow.ArticleID, " Edit ", "Article", App.Config.AspMakerGen & "article_edit.aspx?ArticleID=" & myrow.ArticleID, "edit") & _
                    wpmUTIL.FormatLink(myrow.ArticleID, " Delete ", "Article", App.Config.AspMakerGen & "article_delete.aspx?ArticleID=" & myrow.ArticleID, "delete") & _
                    wpmUTIL.FormatLink(myrow.ArticleID, "Copy", "Article", App.Config.AspMakerGen & "article_add.aspx?ArticleID=" & myrow.ArticleID, "add") & ")<br />")
            End If
        Next
        If (sReturn.ToString = "") Then
            sReturn.Append("&nbsp;")
        End If
        BuildPageArticleList = sReturn.ToString
    End Function

    Private Function BuildArticleList() As String
        Dim sReturn As New StringBuilder("")
        Dim myrow As wpmLocation
        Dim j As Integer
        For j = 0 To ActiveSite.LocationList.Count - 1
            myrow = ActiveSite.LocationList.Item(j)
            If myrow.RecordSource = "Article" Then
                If (myrow.PageID = "") Or IsDBNull(myrow.PageID) Then
                    sReturn.Append("<tr><td nowrap=""nowrap"">")
                    If Not (myrow.ActiveFL) Then
                        sReturn.Append("(i) ")
                    End If
                    sReturn.Append(AdminFormatLink(myrow.ArticleID, myrow.PageName, "Article", myrow.TransferURL) & _
                        " (" & wpmUTIL.FormatLink(myrow.ArticleID, " Edit ", "Article", App.Config.AspMakerGen & "article_edit.aspx?ArticleID=" & myrow.ArticleID, "edit") & _
                        wpmUTIL.FormatLink(myrow.ArticleID, " Delete ", "Article", App.Config.AspMakerGen & "article_delete.aspx?ArticleID=" & myrow.ArticleID, "delete") & _
                        wpmUTIL.FormatLink(myrow.ArticleID, "Copy", "Article", App.Config.AspMakerGen & "article_add.aspx?ArticleID=" & myrow.ArticleID, "add") & ")<br />")
                End If
            End If
        Next
        If (sReturn.ToString = "") Then
            sReturn.Append("&nbsp;")
        End If
        BuildArticleList = sReturn.ToString
    End Function
    Private Function BuildAdminHeader() As String
        Dim sbReturn As New StringBuilder
        sbReturn.Append("<table border=""2"" width=""300"" class=""ewTable"" Summary=""Admin Header"">" & vbCrLf)
        sbReturn.Append("<tr>" & vbCrLf)
        sbReturn.Append("<td nowrap=""nowrap"" class=""ewTableHeader""><strong>Site Template</strong> (" & vbCrLf)
        sbReturn.Append("<a href=""" & App.Config.AspMakerGen & "sitetemplate_edit.aspx?TemplatePrefix=" & ActiveSite.SiteTemplatePrefix & """>Edit </a>," & vbCrLf)
        sbReturn.Append("<a href=""" & App.Config.AspMakerGen & "sitetemplate_list.aspx"">List </a>," & vbCrLf)
        sbReturn.Append("<a href=""/wpm/admin/skin_switch.aspx"">Switch </a> )<br />" & vbCrLf)
        sbReturn.Append("<strong>Company</strong>  (" & vbCrLf)
        sbReturn.Append("<a href=""" & App.Config.AspMakerGen & "company_edit.aspx?CompanyID=" & ActiveSite.CompanyID & """>Edit </a>," & vbCrLf)
        sbReturn.Append("<a href=""" & App.Config.AspMakerGen & "company_list.aspx"">List </a>, " & vbCrLf)
        sbReturn.Append("<a href=""/wpm/admin/site_switch.aspx"">Change</a> )<br />" & vbCrLf)
        sbReturn.Append("<strong>Contact</strong> " & wpmSession.GetUserName & " (" & vbCrLf)
        sbReturn.Append("<a href=""" & App.Config.AspMakerGen & "contact_edit.aspx?ContactID=" & ActiveSite.ContactID & """>Edit</a>," & vbCrLf)
        sbReturn.Append("<a href=""" & App.Config.AspMakerGen & "contact_add.aspx"">Add</a>," & vbCrLf)
        sbReturn.Append("<a href=""" & App.Config.AspMakerGen & "contact_list.aspx"">List</a> )" & vbCrLf)
        sbReturn.Append("</td><td nowrap=""nowrap"" class=""ewTableHeader"">" & vbCrLf)
        sbReturn.Append("<strong>Parts</strong> ( " & vbCrLf)
        sbReturn.Append("<a href=""" & "/wpm/admin/AdminLink.aspx"">All Parts</a> )" & vbCrLf)
        sbReturn.Append("<br />" & vbCrLf)
        sbReturn.Append("<strong>Articles</strong> ( " & vbCrLf)
        sbReturn.Append("<a href=""" & App.Config.AspMakerGen & "article_list.aspx"">List</a>," & vbCrLf)
        sbReturn.Append("<a href=""" & App.Config.AspMakerGen & "article_add.aspx"">Add</a> ) " & vbCrLf)
        sbReturn.Append("<br />" & vbCrLf)
        sbReturn.Append("<strong>Pages</strong> ( " & vbCrLf)
        sbReturn.Append("<a href=""" & App.Config.AspMakerGen & "zpage_list.aspx"">List</a>," & vbCrLf)
        sbReturn.Append("<a href=""" & App.Config.AspMakerGen & "zpage_add.aspx"">Add</a>," & vbCrLf)
        sbReturn.Append("<a href=""/wpm/admin/wpmPageOrder.aspx"">Order</a> )&nbsp;&nbsp;&nbsp;" & vbCrLf)

        '  sbReturn.Append("<strong>Page Role</strong>(" & vbCrLf)
        '  sbReturn.Append("<a href=""" & App.Config.AspMakerGen & "pagerole_list.aspx"">List</a> )<br /><br/>" & vbCrLf)
        '  sbReturn.Append("<br /><hr /><br/>" & vbCrLf)
        '  sbReturn.Append("<a href=""" & App.Config.AspMakerGen & "link_list.aspx"">List</a>," & vbCrLf)
        '  sbReturn.Append("<a href=""" & App.Config.AspMakerGen & "link_add.aspx"">Add</a>) " & vbCrLf)
        '  sbReturn.Append("<br /><br/>" & vbCrLf)
        '  sbReturn.Append("<strong>Images</strong> ( " & vbCrLf)
        '  sbReturn.Append("<a href=""" & App.Config.AspMakerGen & "image_list.aspx"">List</a>," & vbCrLf)
        '  sbReturn.Append("<a href=""" & App.Config.AspMakerGen & "image_add.aspx"">Add</a>) " & vbCrLf)
        '  sbReturn.Append("<br /><hr /><br/>" & vbCrLf)
        ' sbReturn.Append("<a href=""" & App.Config.AspMakerGen & "sitetemplate_add.aspx?TemplatePrefix=" & ActiveSite.SiteTemplatePrefix & """>Copy </a>," & vbCrLf)
        ' sbReturn.Append("<a href=""" & App.Config.AspMakerGen & "sitetemplate_add.aspx"">Add</a>," & vbCrLf)
        ' sbReturn.Append("<a href=""" & App.Config.AspMakerGen & "company_add.aspx?CompanyID=" & ActiveSite.CompanyID & """>Copy </a>," & vbCrLf)
        ' sbReturn.Append("<a href=""" & App.Config.AspMakerGen & "company_add.aspx"">Add </a>," & vbCrLf)
        ' sbReturn.Append("<a href=""" & App.Config.AspMakerGen & "contact_add.aspx?ContactID=" & ActiveSite.ContactID & " "">Copy</a>," & vbCrLf)

        sbReturn.Append("</td></tr><tr><td colspan=""2"" class=""ewTableHeader""><h2>Pages &amp; Articles Outline</h2></td></tr>" & vbCrLf)
        Return sbReturn.ToString
    End Function

    Public Shared Function BuildAdminHeaderLinks(ByRef ActiveSite As wpmActiveSite) As String
        Return ("<div class=""topbanner""><div class=""rightnav""><a href=""/wpm/admin/default.aspx""> Admin Home </a> | <a href=""/""> Back To Site </a> |" & _
ActiveSite.GetUserOptions & "</div><h1>" & _
ActiveSite.CompanyName & " - Administration</h1></div>")
    End Function


End Class
