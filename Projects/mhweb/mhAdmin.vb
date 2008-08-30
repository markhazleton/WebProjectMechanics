Public Class mhAdmin
    Private mySiteMap As mhSiteMap
    Private myArticleRows As mhArticleRows
    Private myImageRows As mhImageRows
    Sub New(ByVal thisSession As System.Web.SessionState.HttpSessionState)
        mySiteMap = New mhSiteMap(thisSession)
        myArticleRows = New mhArticleRows(mySiteMap.mySiteFile.CompanyName)
        myImageRows = New mhImageRows(mySiteMap.mySession.CompanyID)
    End Sub
    Public Function SiteImageReport() As String
        Dim sbReturn As New StringBuilder(String.Empty)
        sbReturn.Append("<table class=""sortable autostripe "">")
        sbReturn.Append("<thead><tr><th>Image</th><th>Details</th></tr></thead>")
        For Each myRow As mhSiteMapRow In mySiteMap.mySiteFile.SiteMapRows
            If myRow.RecordSource = "Image" Then
                sbReturn.Append("<tr>")
                sbReturn.Append("<td style=""whitespace:nobreak;""><a href=""/aspmaker/image_edit.aspx?ImageID=" & myRow.ArticleID & """>" & myRow.PageName & "</a></td>")
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
        For Each myRow As mhSiteMapRow In mySiteMap.mySiteFile.SiteMapRows
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
        For Each myImage As mhImageRow In myImageRows
            If myImage.ImageID = ImageID Then
                sbReturn.Append("<strong>" & myImage.ImageName & " (" & myImage.ImageID & ")</strong><br/>")
                sbReturn.Append("" & myImage.ImageDescription & "<br/>")
                sbReturn.Append("<img src=""/mhweb/catalog/ImageResize.aspx?img=" & mySiteMap.mySession.SiteGallery & myImage.ImageFileName & "&w=200"" />")
            End If
        Next
        Return sbReturn.ToString
    End Function

    Private Function GetArticleBody(ByVal PageID As String) As String
        Dim sbReturn As New StringBuilder(String.Empty)

        For Each myArticle As mhArticleRow In myArticleRows
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
        mySiteMap.mySession.ListPageURL = HttpContext.Current.Request.FilePath()
        myStringBuilder.Append(BuildAdminHeader())
        myStringBuilder.Append(BuildPageStructure("", 0, RowCount))
        myStringBuilder.Append("<tr><td colspan=""2"" class=""ewTableHeader""><h2>Articles without Pages</h2></td></tr>")
        myStringBuilder.Append(BuildArticleList())
        myStringBuilder.Append("<tr><td colspan=""2"" class=""ewTableHeader""><h2>Site Category</h2></td></tr>")
        myStringBuilder.Append(BuildCategoryStructure("", 0, RowCount))
        myStringBuilder.Append("</table>")
        Dim sbAdminDHTML As New StringBuilder
        mhfio.ReadTextFile(HttpContext.Current.Server.MapPath("/mhweb/admin/adminDHTML.html"), sbAdminDHTML)
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
        '        Return mhutil.FormatLink(LinkID, LinkName, LinkType, LinkURL)
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
        Dim myRow As mhSiteMapRow
        Dim j As Integer
        Dim sReturn As New StringBuilder
        Dim sRowClass As String
        For j = 0 To mySiteMap.mySiteFile.SiteMapRows.Count - 1
            Dim sCellContent As New StringBuilder
            myRow = mySiteMap.mySiteFile.SiteMapRows.Item(j)
            If ParentID = myRow.ParentPageID And myRow.RecordSource = "Page" Then
                If CBool(iRowCount Mod 2) Then
                    sRowClass = "ewTableAltRow"
                Else
                    sRowClass = "ewTableRow"
                End If
                iRowCount = iRowCount + 1
                sCellContent.Append("<img src=""/images/spacer.gif"" alt="""" width=""" & intLevel * 15 & """ height=""1"" />")
                If Not myRow.ActiveFL Then
                    sCellContent.Append("(i) ")
                End If
                sCellContent.Append(AdminFormatLink(myRow.PageID, myRow.PageName, "Page", myRow.TransferURL) & "&nbsp;(")
                sCellContent.Append(mhUTIL.FormatLink(myRow.PageID, "Edit", "Page", mhConfig.mhASPMakerGen & "page_edit.aspx?PageID=" & myRow.PageID, "edit") & "&nbsp;")
                sCellContent.Append(mhUTIL.FormatLink(myRow.PageID, "Delete", "Page", mhConfig.mhASPMakerGen & "page_delete.aspx?PageID=" & myRow.PageID, "delete") & "&nbsp;")
                sCellContent.Append(mhUTIL.FormatLink(myRow.PageID, "Copy", "Page", mhConfig.mhASPMakerGen & "page_add.aspx?PageID=" & myRow.PageID, "add") & "&nbsp;)")
                sReturn.Append("<tr class=""" & sRowClass & """>" & vbCrLf & "<td NOWRAP class=""" & sRowClass & """ valign=""top"">" & sCellContent.ToString & vbCrLf & "</td>" & vbCrLf & "<td NOWRAP class=""" & sRowClass & """ valign=""top"">" & vbCrLf)
                sReturn.Append(BuildPageArticleList(myRow.PageID, myRow.TransferURL))
                sReturn.Append("</td>" & vbCrLf & "</tr>" & vbCrLf)
                sReturn.Append(BuildPageStructure(myRow.PageID, intLevel + 1, iRowCount))
            End If
        Next
        Return sReturn.ToString
    End Function

    Private Function BuildCategoryStructure(ByVal ParentID As String, ByVal intLevel As Integer, ByRef iRowCount As Integer) As String
        Dim myRow As mhSiteMapRow
        Dim j As Integer
        Dim sReturn As New StringBuilder
        Dim sRowClass As String
        Dim sSiteCategoryID As String
        For j = 0 To mySiteMap.mySiteFile.SiteMapRows.Count - 1
            Dim sCellContent As New StringBuilder
            myRow = mySiteMap.mySiteFile.SiteMapRows.Item(j)
            If ParentID = myRow.ParentPageID And myRow.RecordSource = "Category" Then
                If CBool(iRowCount Mod 2) Then
                    sRowClass = "ewTableAltRow"
                Else
                    sRowClass = "ewTableRow"
                End If
                iRowCount = iRowCount + 1
                sCellContent.Append("<img src=""/images/spacer.gif"" alt="""" width=""" & intLevel * 15 & """ height=""2"" />")
                If Not myRow.ActiveFL Then
                    sCellContent.Append("(i) ")
                End If
                sSiteCategoryID = Replace(myRow.PageID, "CAT-", "")
                sCellContent.Append(AdminFormatLink(myRow.PageID, myRow.PageName, "Category", myRow.TransferURL) & "&nbsp;(")
                sCellContent.Append(mhUTIL.FormatLink(sSiteCategoryID, "Edit", "Category", mhConfig.mhASPMakerGen & "sitecategory_edit.aspx?SiteCategoryID=" & sSiteCategoryID, "edit") & "&nbsp;")
                sCellContent.Append(mhUTIL.FormatLink(sSiteCategoryID, "Delete", "Category", mhConfig.mhASPMakerGen & "sitecategory_delete.aspx?SiteCategoryID=" & sSiteCategoryID, "delete") & "&nbsp;")
                sCellContent.Append(mhUTIL.FormatLink(sSiteCategoryID, "Copy", "Category", mhConfig.mhASPMakerGen & "sitecategory_add.aspx?SiteCategoryID=" & sSiteCategoryID, "add") & "&nbsp;)")
                sReturn.Append("<tr class=""" & sRowClass & """>" & vbCrLf & "<td NOWRAP class=""" & sRowClass & """ valign=""top"">" & sCellContent.ToString & vbCrLf & "</td>" & vbCrLf & "<td NOWRAP class=""" & sRowClass & """ valign=""top"">" & vbCrLf)
                sReturn.Append(BuildCategoryPageList(myRow.SiteCategoryID) & "&nbsp;</td>" & vbCrLf & "</tr>" & vbCrLf)
                sReturn.Append(BuildCategoryStructure(myRow.PageID, intLevel + 1, iRowCount))
            End If
        Next
        Return sReturn.ToString
    End Function
    Private Function BuildCategoryPageList(ByVal SiteCategoryID As String) As String
        Dim myRow As mhSiteMapRow
        Dim j As Integer
        Dim sReturn As New StringBuilder
        For j = 0 To mySiteMap.mySiteFile.SiteMapRows.Count - 1
            Dim sCellContent As New StringBuilder
            myRow = mySiteMap.mySiteFile.SiteMapRows.Item(j)
            If SiteCategoryID = myRow.SiteCategoryID And myRow.RecordSource = "Page" Then
                sCellContent.Append(AdminFormatLink(myRow.PageID, myRow.PageName, "Page", myRow.TransferURL) & "&nbsp;(")
                sCellContent.Append(mhUTIL.FormatLink(myRow.PageID, "Edit", "Page", mhConfig.mhASPMakerGen & "page_edit.aspx?PageID=" & myRow.PageID, "edit") & "&nbsp;")
                sCellContent.Append(mhUTIL.FormatLink(myRow.PageID, "Delete", "Page", mhConfig.mhASPMakerGen & "page_delete.aspx?PageID=" & myRow.PageID, "delete") & "&nbsp;")
                sCellContent.Append(mhUTIL.FormatLink(myRow.PageID, "Copy", "Page", mhConfig.mhASPMakerGen & "page_add.aspx?PageID=" & myRow.PageID, "add") & "&nbsp;)")
                If Not myRow.ActiveFL Then
                    sCellContent.Append("(i) ")
                End If
                sReturn.Append(sCellContent.ToString & "<br/>" & vbCrLf)
            End If
        Next
        Return sReturn.ToString
    End Function

    Private Function BuildPageList() As String
        Dim myrow As mhSiteMapRow
        Dim sCellContent As String = ""
        Dim j As Integer = 0
        Dim sReturn As String = ""
        Dim sRowClass As String = ""
        For j = 0 To mySiteMap.mySiteFile.SiteMapRows.Count - 1
            myrow = mySiteMap.mySiteFile.SiteMapRows.Item(j)
            If myrow.RecordSource = "Page" Then
                sCellContent = "<img src=""/images/spacer.gif"" alt="""" width=""15"" height=""1"" />"
                sCellContent = sCellContent & mhUTIL.FormatLink(myrow.PageID, myrow.PageName, "Page", myrow.TransferURL) & "&nbsp;("
                sCellContent = sCellContent & mhUTIL.FormatLink(myrow.PageID, "Edit", "Page", mhConfig.mhASPMakerGen & "page_edit.aspx?PageID=" & myrow.PageID, "edit") & "&nbsp;"
                sCellContent = sCellContent & mhUTIL.FormatLink(myrow.PageID, "Delete", "Page", mhConfig.mhASPMakerGen & "page_delete.aspx?PageID=" & myrow.PageID, "delete") & "&nbsp;"
                sCellContent = sCellContent & mhUTIL.FormatLink(myrow.PageID, "Copy", "Page", mhConfig.mhASPMakerGen & "page_add.aspx?PageID=" & myrow.PageID, "add") & "&nbsp;)"
                sReturn = sReturn & ("<tr class=""" & sRowClass & """>" & vbCrLf)
                sReturn = sReturn & (vbTab & "<td NOWRAP class=""" & sRowClass & """ valign=""top"">" & sCellContent & "&nbsp;</td>" & vbCrLf)
                sReturn = sReturn & (vbTab & "<td NOWRAP class=""" & sRowClass & """ valign=""top"">")
                sReturn = sReturn & myrow.BreadCrumbHTML
                sReturn = sReturn & ("</td>" & vbCrLf & "</tr>" & vbCrLf)
            End If
        Next
        Return sReturn
    End Function

    Private Function BuildPageArticleList(ByVal pageID As String, ByVal LinkURL As String) As String
        Dim myrow As mhArticleRow
        Dim sReturn As New StringBuilder
        Dim j As Integer
        For j = 0 To myArticleRows.Count - 1
            myrow = myArticleRows.Item(j)
            If (myrow.ArticlePageID = pageID) Then
                If Not (myrow.IsArticleActive) Then
                    sReturn.Append("(i) ")
                End If
                sReturn.Append(AdminFormatLink(myrow.ArticleID, myrow.PageName, "Article", myrow.ArticleName) & _
                    " (" & mhUTIL.FormatLink(myrow.ArticleID, " Edit ", "Article", mhConfig.mhASPMakerGen & "article_edit.aspx?ArticleID=" & myrow.ArticleID, "edit") & _
                    mhUTIL.FormatLink(myrow.ArticleID, " Delete ", "Article", mhConfig.mhASPMakerGen & "article_delete.aspx?ArticleID=" & myrow.ArticleID, "delete") & _
                    mhUTIL.FormatLink(myrow.ArticleID, "Copy", "Article", mhConfig.mhASPMakerGen & "article_add.aspx?ArticleID=" & myrow.ArticleID, "add") & ")<br />")
            End If
        Next
        If (sReturn.ToString = "") Then
            sReturn.Append("&nbsp;")
        End If
        BuildPageArticleList = sReturn.ToString
    End Function

    Private Function BuildArticleList() As String
        Dim sReturn As New StringBuilder("")
        Dim myrow As mhSiteMapRow
        Dim j As Integer
        For j = 0 To mySiteMap.mySiteFile.SiteMapRows.Count - 1
            myrow = mySiteMap.mySiteFile.SiteMapRows.Item(j)
            If myrow.RecordSource = "Article" Then
                If (myrow.PageID = "") Or IsDBNull(myrow.PageID) Then
                    sReturn.Append("<tr><td nowrap=""nowrap"">")
                    If Not (myrow.ActiveFL) Then
                        sReturn.Append("(i) ")
                    End If
                    sReturn.Append(AdminFormatLink(myrow.ArticleID, myrow.PageName, "Article", myrow.TransferURL) & _
                        " (" & mhUTIL.FormatLink(myrow.ArticleID, " Edit ", "Article", mhConfig.mhASPMakerGen & "article_edit.aspx?ArticleID=" & myrow.ArticleID, "edit") & _
                        mhUTIL.FormatLink(myrow.ArticleID, " Delete ", "Article", mhConfig.mhASPMakerGen & "article_delete.aspx?ArticleID=" & myrow.ArticleID, "delete") & _
                        mhUTIL.FormatLink(myrow.ArticleID, "Copy", "Article", mhConfig.mhASPMakerGen & "article_add.aspx?ArticleID=" & myrow.ArticleID, "add") & ")<br />")
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
        sbReturn.Append("<td nowrap=""nowrap"" class=""ewTableHeader""><strong>Template</strong> (" & vbCrLf)
        sbReturn.Append("<a href=""" & mhConfig.mhASPMakerGen & "sitetemplate_edit.aspx?TemplatePrefix=" & mySiteMap.mySession.SiteTemplatePrefix & """>Edit </a>," & vbCrLf)
        sbReturn.Append("<a href=""" & mhConfig.mhASPMakerGen & "sitetemplate_add.aspx?TemplatePrefix=" & mySiteMap.mySession.SiteTemplatePrefix & """>Copy </a>," & vbCrLf)
        sbReturn.Append("<a href=""" & mhConfig.mhASPMakerGen & "sitetemplate_add.aspx"">Add</a>," & vbCrLf)
        sbReturn.Append("<a href=""" & mhConfig.mhASPMakerGen & "sitetemplate_list.aspx"">List </a>," & vbCrLf)
        sbReturn.Append("<a href=""/mhweb/admin/skin_switch.aspx"">Switch </a> )<br /><br/>" & vbCrLf)
        sbReturn.Append("<strong>Company</strong>  (" & vbCrLf)
        sbReturn.Append("<a href=""" & mhConfig.mhASPMakerGen & "company_edit.aspx?CompanyID=" & mySiteMap.mySession.CompanyID & """>Edit </a>," & vbCrLf)
        sbReturn.Append("<a href=""" & mhConfig.mhASPMakerGen & "company_add.aspx?CompanyID=" & mySiteMap.mySession.CompanyID & """>Copy </a>," & vbCrLf)
        sbReturn.Append("<a href=""" & mhConfig.mhASPMakerGen & "company_add.aspx"">Add </a>," & vbCrLf)
        sbReturn.Append("<a href=""" & mhConfig.mhASPMakerGen & "company_list.aspx"">List </a>, " & vbCrLf)
        sbReturn.Append("<a href=""/mhweb/admin/site_switch.aspx"">Change</a> )<br /><br/>" & vbCrLf)
        sbReturn.Append("<strong>Contact</strong> " & mhSession.GetUserName & " (" & vbCrLf)
        sbReturn.Append("<a href=""" & mhConfig.mhASPMakerGen & "contact_edit.aspx?ContactID=" & mySiteMap.mySession.ContactID & """>Edit</a>," & vbCrLf)
        sbReturn.Append("<a href=""" & mhConfig.mhASPMakerGen & "contact_add.aspx?ContactID=" & mySiteMap.mySession.ContactID & " "">Copy</a>," & vbCrLf)
        sbReturn.Append("<a href=""" & mhConfig.mhASPMakerGen & "contact_add.aspx"">Add</a>," & vbCrLf)
        sbReturn.Append("<a href=""" & mhConfig.mhASPMakerGen & "contact_list.aspx"">List</a>," & vbCrLf)
        sbReturn.Append("<a href=""/mhweb/login/logout.aspx"">Logout</a> ) <hr /><br/>" & vbCrLf)
        sbReturn.Append("<strong>Pages</strong> ( " & vbCrLf)
        sbReturn.Append("<a href=""" & mhConfig.mhASPMakerGen & "page_list.aspx"">List</a>," & vbCrLf)
        sbReturn.Append("<a href=""" & mhConfig.mhASPMakerGen & "page_add.aspx"">Add</a>," & vbCrLf)
        sbReturn.Append("<a href=""/mhweb/admin/mhPageOrder.aspx"">Order</a> )&nbsp;&nbsp;&nbsp;" & vbCrLf)
        sbReturn.Append("</td><td nowrap=""nowrap"" class=""ewTableHeader"">" & vbCrLf)
        sbReturn.Append("<strong>Page Role</strong>(" & vbCrLf)
        sbReturn.Append("<a href=""" & mhConfig.mhASPMakerGen & "pagerole_list.aspx"">List</a> )<br /><br/>" & vbCrLf)
        sbReturn.Append("<strong>Links</strong> ( " & vbCrLf)
        sbReturn.Append("<a href=""" & mhConfig.mhASPMakerGen & "link_list.aspx"">List</a>," & vbCrLf)
        sbReturn.Append("<a href=""" & mhConfig.mhASPMakerGen & "link_add.aspx"">Add</a>) " & vbCrLf)
        sbReturn.Append("<br /><br/>" & vbCrLf)
        sbReturn.Append("<strong>Images</strong> ( " & vbCrLf)
        sbReturn.Append("<a href=""" & mhConfig.mhASPMakerGen & "image_list.aspx"">List</a>," & vbCrLf)
        sbReturn.Append("<a href=""" & mhConfig.mhASPMakerGen & "image_add.aspx"">Add</a>) " & vbCrLf)
        sbReturn.Append("<br /><hr /><br/>" & vbCrLf)
        sbReturn.Append("<strong>Articles</strong> ( " & vbCrLf)
        sbReturn.Append("<a href=""" & mhConfig.mhASPMakerGen & "article_list.aspx"">List</a>," & vbCrLf)
        sbReturn.Append("<a href=""" & mhConfig.mhASPMakerGen & "article_add.aspx"">Add</a> ) " & vbCrLf)
        sbReturn.Append("</td></tr><tr><td colspan=""2"" class=""ewTableHeader""><h2>Pages &amp; Articles Outline</h2></td></tr>" & vbCrLf)
        Return sbReturn.ToString
    End Function
End Class
