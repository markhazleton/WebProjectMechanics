Imports System.Web
Imports System.Text

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
                sbReturn.Append(String.Format("<td style=""whitespace:nobreak;""><a href=""/wpmgen/image_edit.aspx?ImageID={0}"">{1}</a></td>", myRow.ArticleID, myRow.PageName))
                sbReturn.Append(String.Format("<td>{0}</td>", GetImageDetail(myRow.ArticleID)))
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
                sbReturn.Append(String.Format("<td style=""whitespace:nobreak;"">{0}</td>", myRow.PageName))
                sbReturn.Append(String.Format("<td>{0}</td>", GetArticleBody(myRow.PageID)))
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
                sbReturn.Append(String.Format("<strong>{0} ({1})</strong><br/>", myImage.ImageName, myImage.ImageID))
                sbReturn.Append(String.Format("{0}<br/>", myImage.ImageDescription))
                sbReturn.Append(String.Format("<img src=""/wpm/catalog/ImageResize.aspx?img={0}{1}&w=200"" />", ActiveSite.SiteGallery, myImage.ImageFileName))
            End If
        Next
        Return sbReturn.ToString
    End Function

    Private Function GetArticleBody(ByVal PageID As String) As String
        Dim sbReturn As New StringBuilder(String.Empty)

        For Each myArticle As wpmArticle In ArticleList
            If myArticle.ArticlePageID = PageID Then
                sbReturn.Append(String.Format("<strong>{0} ({1} - {2})</strong", myArticle.ArticleName, myArticle.ArticlePageID, myArticle.ArticleID))
                sbReturn.Append(String.Format("{0}", myArticle.ArticleBody))
            End If
        Next
        Return sbReturn.ToString
    End Function
    Public Function BuildAdmin() As String
        Dim RowCount As Integer = 0
        Dim myStringBuilder As StringBuilder = New StringBuilder()
        ' Set the ListPageURL so the edit,delete, add, pages know where to come back to
        HttpContext.Current.Session("ListPageURL") = HttpContext.Current.Request.FilePath()
        myStringBuilder.Append("<table><tr><td colspan=""2"" class=""ewTableHeader""><h2>Pages &amp; Articles Outline</h2></td></tr>")
        myStringBuilder.Append(BuildPageStructure("", 0, RowCount))
        myStringBuilder.Append("<tr><td colspan=""2"" class=""ewTableHeader""><h2>Articles without Pages</h2></td></tr>")
        myStringBuilder.Append(BuildArticleList())
        myStringBuilder.Append("<tr><td colspan=""2"" class=""ewTableHeader""><h2>Site Category</h2></td></tr>")
        myStringBuilder.Append(BuildCategoryStructure("", 0, RowCount))
        myStringBuilder.Append("</table>")
        Dim sbAdminDHTML As New StringBuilder
        wpmFileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath("/wpm/admin/adminDHTML.html"), sbAdminDHTML)
        myStringBuilder.Append(sbAdminDHTML.ToString)
        Return (myStringBuilder.ToString)
    End Function

    Private Shared Function AdminFormatLink(ByVal LinkID As String, ByVal LinkName As String, ByVal LinkType As String) As String
        Dim sReturn As String = ""
        If LinkID <> "" Then
            Select Case LinkType
                Case "Page"
                    sReturn = String.Format("<a class=""linkmenu"" href="""" onclick=""createPopUp(200,110,'{0}','Page',this);return false"" id=Page{0} >{1}</a>", LinkID, LinkName)
                Case "Article"
                    sReturn = String.Format("<a class=""linkmenu"" href="""" onclick=""createPopUp(200,90,'{0}','Article',this);return false"" id=Article{0} >{1}</a>", LinkID, LinkName)
                Case "Company"
                    sReturn = String.Format("<a class=""linkmenu"" href="""" onclick=""createPopUp(200,110,'{0}','Company',this);return false"" id=Company{0} >{1}</a>", LinkID, LinkName)
                Case "Category"
                    sReturn = LinkName
                Case Else
                    sReturn = LinkName
            End Select
        End If
        Return sReturn
    End Function

    Private Function FormatLink(ByVal LinkName As String, ByVal LinkType As String, ByVal LinkURL As String) As String
        Dim sReturn As String
        Select Case LinkType
            Case "Contact"
                sReturn = LinkName
            Case Else
                sReturn = String.Format("<a href=""{0}"">{1}</a>", LinkURL, LinkName)
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
                sCellContent.Append(String.Format("<img src=""/wpm/images/spacer.gif"" alt="""" width=""{0}"" height=""1"" />", intLevel * 15))
                If Not myRow.ActiveFL Then
                    sCellContent.Append("(i) ")
                End If
                sCellContent.Append(AdminFormatLink(myRow.PageID, myRow.PageName, "Page") & "&nbsp;(")
                sCellContent.Append(wpmUtil.FormatLink("Edit", "Page", String.Format("{0}zpage_edit.aspx?zPageID={1}", wpmApp.Config.AspMakerGen, myRow.PageID), "edit") & "&nbsp;")
                sCellContent.Append(wpmUtil.FormatLink("Delete", "Page", String.Format("{0}zpage_delete.aspx?zPageID={1}", wpmApp.Config.AspMakerGen, myRow.PageID), "delete") & "&nbsp;")
                sCellContent.Append(wpmUtil.FormatLink("Copy", "Page", String.Format("{0}zpage_add.aspx?zPageID={1}", wpmApp.Config.AspMakerGen, myRow.PageID), "add") & "&nbsp;)")
                sReturn.Append(String.Format("<tr class=""{0}"">{1}<td NOWRAP class=""{0}"" valign=""top"">{2}{1}</td>{1}<td NOWRAP class=""{0}"" valign=""top"">{1}", sRowClass, vbCrLf, sCellContent))
                sReturn.Append(BuildPageArticleList(myRow.PageID))
                sReturn.Append(String.Format("</td>{0}</tr>{0}", vbCrLf))
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
                sCellContent.Append(String.Format("<img src=""/wpm/images/spacer.gif"" alt="""" width=""{0}"" height=""2"" />", intLevel * 15))
                If Not myRow.ActiveFL Then
                    sCellContent.Append("(i) ")
                End If
                sSiteCategoryID = Replace(myRow.PageID, "CAT-", "")
                sCellContent.Append(AdminFormatLink(myRow.PageID, myRow.PageName, "Category") & "&nbsp;(")
                sCellContent.Append(wpmUtil.FormatLink("Edit", "Category", String.Format("{0}sitecategory_edit.aspx?SiteCategoryID={1}", wpmApp.Config.AspMakerGen, sSiteCategoryID), "edit") & "&nbsp;")
                sCellContent.Append(wpmUtil.FormatLink("Delete", "Category", String.Format("{0}sitecategory_delete.aspx?SiteCategoryID={1}", wpmApp.Config.AspMakerGen, sSiteCategoryID), "delete") & "&nbsp;")
                sCellContent.Append(wpmUtil.FormatLink("Copy", "Category", String.Format("{0}sitecategory_add.aspx?SiteCategoryID={1}", wpmApp.Config.AspMakerGen, sSiteCategoryID), "add") & "&nbsp;)")
                sReturn.Append(String.Format("<tr class=""{0}"">{1}<td NOWRAP class=""{0}"" valign=""top"">{2}{1}</td>{1}<td NOWRAP class=""{0}"" valign=""top"">{1}", sRowClass, vbCrLf, sCellContent))
                sReturn.Append(String.Format("{0}&nbsp;</td>{1}</tr>{1}", BuildCategoryPageList(myRow.SiteCategoryID), vbCrLf))
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
                sCellContent.Append(AdminFormatLink(myRow.PageID, myRow.PageName, "Page") & "&nbsp;(")
                sCellContent.Append(wpmUtil.FormatLink("Edit", "Page", String.Format("{0}zpage_edit.aspx?zPageID={1}", wpmApp.Config.AspMakerGen, myRow.PageID), "edit") & "&nbsp;")
                sCellContent.Append(wpmUtil.FormatLink("Delete", "Page", String.Format("{0}zpage_delete.aspx?zPageID={1}", wpmApp.Config.AspMakerGen, myRow.PageID), "delete") & "&nbsp;")
                sCellContent.Append(wpmUtil.FormatLink("Copy", "Page", String.Format("{0}zpage_add.aspx?zPageID={1}", wpmApp.Config.AspMakerGen, myRow.PageID), "add") & "&nbsp;)")
                If Not myRow.ActiveFL Then
                    sCellContent.Append("(i) ")
                End If
                sReturn.Append(String.Format("{0}<br/>{1}", sCellContent, vbCrLf))
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
                sbCellContent.Append(wpmUtil.FormatLink(myrow.PageName, "Page", myrow.TransferURL) & "&nbsp;(")
                sbCellContent.Append(wpmUtil.FormatLink("Edit", "Page", String.Format("{0}zpage_edit.aspx?zPageID={1}", wpmApp.Config.AspMakerGen, myrow.PageID), "edit") & "&nbsp;")
                sbCellContent.Append(wpmUtil.FormatLink("Delete", "Page", String.Format("{0}zpage_delete.aspx?zPageID={1}", wpmApp.Config.AspMakerGen, myrow.PageID), "delete") & "&nbsp;")
                sbCellContent.Append(wpmUtil.FormatLink("Copy", "Page", String.Format("{0}zpage_add.aspx?zPageID={1}", wpmApp.Config.AspMakerGen, myrow.PageID), "add") & "&nbsp;)")
                '
                sReturn = sReturn & (String.Format("<tr class=""{0}"">{1}", sRowClass, vbCrLf))
                sReturn = sReturn & (String.Format("{0}<td NOWRAP class=""{1}"" valign=""top"">{2}&nbsp;</td>{3}", vbTab, sRowClass, sbCellContent, vbCrLf))
                sReturn = sReturn & (String.Format("{0}<td NOWRAP class=""{1}"" valign=""top"">", vbTab, sRowClass))
                sReturn = sReturn & myrow.BreadCrumbHTML
                sReturn = sReturn & (String.Format("</td>{0}</tr>{0}", vbCrLf))
            End If
        Next
        Return sReturn
    End Function

    Private Function BuildPageArticleList(ByVal pageID As String) As String
        Dim myrow As wpmArticle
        Dim sReturn As New StringBuilder
        Dim j As Integer
        For j = 0 To ArticleList.Count - 1
            myrow = ArticleList.Item(j)
            If (myrow.ArticlePageID = pageID) Then
                If Not (myrow.IsArticleActive) Then
                    sReturn.Append("(i) ")
                End If
                sReturn.Append(String.Format("{0} ({1}{2}{3})<br />", AdminFormatLink(myrow.ArticleID, myrow.PageName, "Article"), wpmUtil.FormatLink(" Edit ", "Article", String.Format("{0}article_edit.aspx?ArticleID={1}", wpmApp.Config.AspMakerGen, myrow.ArticleID), "edit"), wpmUtil.FormatLink(" Delete ", "Article", String.Format("{0}article_delete.aspx?ArticleID={1}", wpmApp.Config.AspMakerGen, myrow.ArticleID), "delete"), wpmUtil.FormatLink("Copy", "Article", String.Format("{0}article_add.aspx?ArticleID={1}", wpmApp.Config.AspMakerGen, myrow.ArticleID), "add")))
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
                    sReturn.Append(String.Format("{0} ({1}{2}{3})<br />", AdminFormatLink(myrow.ArticleID, myrow.PageName, "Article"), wpmUtil.FormatLink(" Edit ", "Article", String.Format("{0}article_edit.aspx?ArticleID={1}", wpmApp.Config.AspMakerGen, myrow.ArticleID), "edit"), wpmUtil.FormatLink(" Delete ", "Article", String.Format("{0}article_delete.aspx?ArticleID={1}", wpmApp.Config.AspMakerGen, myrow.ArticleID), "delete"), wpmUtil.FormatLink("Copy", "Article", String.Format("{0}article_add.aspx?ArticleID={1}", wpmApp.Config.AspMakerGen, myrow.ArticleID), "add")))
                End If
            End If
        Next
        If (sReturn.ToString = "") Then
            sReturn.Append("&nbsp;")
        End If
        BuildArticleList = sReturn.ToString
    End Function
    Public Function BuildAdminHeader() As String
        Dim sbReturn As New StringBuilder

        wpmFileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath("/wpm/admin/wpmAdminHeader.html"), sbReturn)

        sbReturn.Replace("~~CompanyID~~", ActiveSite.CompanyID)
        sbReturn.Replace("~~Template~~", ActiveSite.SiteTemplatePrefix)
        sbReturn.Replace("~~ContactID~~", ActiveSite.ContactID)

        'sbReturn.Append("<table border=""2"" width=""300"" class=""ewTable"" Summary=""Admin Header"">" & vbCrLf)
        'sbReturn.Append("<tr>" & vbCrLf)
        'sbReturn.Append("<td nowrap=""nowrap"" class=""ewTableHeader""><strong>Site Template</strong> (" & vbCrLf)
        'sbReturn.Append(String.Format("<a href=""{0}sitetemplate_edit.aspx?TemplatePrefix={1}"">Edit </a>,{2}", wpmApp.Config.AspMakerGen, ActiveSite.SiteTemplatePrefix, vbCrLf))
        'sbReturn.Append(String.Format("<a href=""{0}sitetemplate_list.aspx"">List </a>,{1}", wpmApp.Config.AspMakerGen, vbCrLf))
        'sbReturn.Append("<a href=""/wpm/admin/skin_switch.aspx"">Switch </a> )<br />" & vbCrLf)
        'sbReturn.Append("<strong>Company</strong>  (" & vbCrLf)
        'sbReturn.Append(String.Format("<a href=""{0}company_edit.aspx?CompanyID={1}"">Edit </a>,{2}", wpmApp.Config.AspMakerGen, ActiveSite.CompanyID, vbCrLf))
        'sbReturn.Append(String.Format("<a href=""{0}company_list.aspx"">List </a>, {1}", wpmApp.Config.AspMakerGen, vbCrLf))
        'sbReturn.Append("<a href=""/wpm/admin/site_switch.aspx"">Change</a> )<br />" & vbCrLf)
        'sbReturn.Append(String.Format("<strong>Contact</strong> {0} ({1}", wpmSession.GetUserName, vbCrLf))
        'sbReturn.Append(String.Format("<a href=""{0}contact_edit.aspx?ContactID={1}"">Edit</a>,{2}", wpmApp.Config.AspMakerGen, ActiveSite.ContactID, vbCrLf))
        'sbReturn.Append(String.Format("<a href=""{0}contact_add.aspx"">Add</a>,{1}", wpmApp.Config.AspMakerGen, vbCrLf))
        'sbReturn.Append(String.Format("<a href=""{0}contact_list.aspx"">List</a> ){1}", wpmApp.Config.AspMakerGen, vbCrLf))
        'sbReturn.Append("</td><td nowrap=""nowrap"" class=""ewTableHeader"">" & vbCrLf)
        'sbReturn.Append("<strong>Parts</strong> ( " & vbCrLf)
        'sbReturn.Append(String.Format("<a href=""/wpm/admin/AdminLink.aspx"">All Parts</a> ){0}", vbCrLf))
        'sbReturn.Append("<br />" & vbCrLf)
        'sbReturn.Append("<strong>Articles</strong> ( " & vbCrLf)
        'sbReturn.Append(String.Format("<a href=""{0}article_list.aspx"">List</a>,{1}", wpmApp.Config.AspMakerGen, vbCrLf))
        'sbReturn.Append(String.Format("<a href=""{0}article_add.aspx"">Add</a> ) {1}", wpmApp.Config.AspMakerGen, vbCrLf))
        'sbReturn.Append("<br />" & vbCrLf)
        'sbReturn.Append("<strong>Pages</strong> ( " & vbCrLf)
        'sbReturn.Append(String.Format("<a href=""{0}zpage_list.aspx"">List</a>,{1}", wpmApp.Config.AspMakerGen, vbCrLf))
        'sbReturn.Append(String.Format("<a href=""{0}zpage_add.aspx"">Add</a>,{1}", wpmApp.Config.AspMakerGen, vbCrLf))
        'sbReturn.Append("<a href=""/wpm/admin/wpmPageOrder.aspx"">Order</a> )&nbsp;&nbsp;&nbsp;" & vbCrLf)
        'sbReturn.Append("<strong>Page Role</strong>(" & vbCrLf)
        'sbReturn.Append("<a href=""" & App.Config.AspMakerGen & "pagerole_list.aspx"">List</a> )<br /><br/>" & vbCrLf)
        'sbReturn.Append("<br /><hr /><br/>" & vbCrLf)
        'sbReturn.Append("<a href=""" & App.Config.AspMakerGen & "link_list.aspx"">List</a>," & vbCrLf)
        'sbReturn.Append("<a href=""" & App.Config.AspMakerGen & "link_add.aspx"">Add</a>) " & vbCrLf)
        'sbReturn.Append("<br /><br/>" & vbCrLf)
        'sbReturn.Append("<strong>Images</strong> ( " & vbCrLf)
        'sbReturn.Append("<a href=""" & App.Config.AspMakerGen & "image_list.aspx"">List</a>," & vbCrLf)
        'sbReturn.Append("<a href=""" & App.Config.AspMakerGen & "image_add.aspx"">Add</a>) " & vbCrLf)
        'sbReturn.Append("<br /><hr /><br/>" & vbCrLf)
        'sbReturn.Append("<a href=""" & App.Config.AspMakerGen & "sitetemplate_add.aspx?TemplatePrefix=" & ActiveSite.SiteTemplatePrefix & """>Copy </a>," & vbCrLf)
        'sbReturn.Append("<a href=""" & App.Config.AspMakerGen & "sitetemplate_add.aspx"">Add</a>," & vbCrLf)
        'sbReturn.Append("<a href=""" & App.Config.AspMakerGen & "company_add.aspx?CompanyID=" & ActiveSite.CompanyID & """>Copy </a>," & vbCrLf)
        'sbReturn.Append("<a href=""" & App.Config.AspMakerGen & "company_add.aspx"">Add </a>," & vbCrLf)
        'sbReturn.Append("<a href=""" & App.Config.AspMakerGen & "contact_add.aspx?ContactID=" & ActiveSite.ContactID & " "">Copy</a>," & vbCrLf)
        'sbReturn.Append("</td></tr></table>" & vbCrLf)
        Return sbReturn.ToString
    End Function

    Public Function BuildAdminHeaderLinks() As String
        Return (String.Format("<div class=""topbanner""><div class=""rightnav""><a href=""/wpm/admin/default.aspx""> Admin Home </a> | <a href=""/""> Back To Site </a> |{0}</div><h1>{1} - Administration</h1></div>", wpmActiveSite.GetUserOptions, ActiveSite.CompanyName))
    End Function


End Class
