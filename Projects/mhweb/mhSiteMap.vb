Public Class mhSiteMap
    Public mySiteFile As New mhSiteFile
    Public mySession As mhSession
    Public CurrentMapRow As New mhSiteMapRow
    Public Sub New(ByVal thisSession As System.Web.SessionState.HttpSessionState)
        MyBase.New()
        mySession = New mhSession(thisSession)
        LoadSiteFile("ORDER", thisSession)
    End Sub
    Public Sub New(ByVal OrderBy As String, ByVal thisSession As System.Web.SessionState.HttpSessionState)
        MyBase.New()
        mySession = New mhSession(thisSession)
        LoadSiteFile(OrderBy, thisSession)
    End Sub
    Private Function LoadSiteFile(ByVal OrderBy As String, ByRef thisSession As System.Web.SessionState.HttpSessionState) As Boolean
        If mySession.SiteDB = "" Then
            GetConfig()
        End If
        mySiteFile = mhSiteFile.GetSiteFile(mySession.CompanyID, mySession.GroupID, mySession.SiteDB, OrderBy)
        mySession.SiteGallery = mySiteFile.SiteGallery
        If mySession.ContactID = "" Then
            mySession.GroupID = "4"
            mySession.ContactID = ""
            mySession.ContactName = ""
            mySession.ContactEmail = ""
            mySession.ContactRoleTitle = "GUEST"
            mySession.ContactRoleFilterMenu = "FALSE"
            mySession.ContactRoleID = ""
            mySession.RightContent = ""
            mySession.CurrentArticleID = ""
            If mySession.CurrentPageID Is Nothing Then
                mySession.CurrentPageID = mySiteFile.SiteHomePageID
            End If
            mySession.SiteTemplatePrefix = mySiteFile.SitePrefix
        End If

        mySession.SiteCategoryTypeID = mySiteFile.SiteCategoryTypeID

        If CheckCurrentSettings() Then
            SetCurrentPageID(mySession.CurrentPageID)
            SelectCurrentPageRow((mySession.CurrentPageID), (mySession.CurrentArticleID))
        End If
        Return True
    End Function
    Private Function SetMainPage(ByVal sMainPageID As String, ByVal sMainPageName As String) As Boolean
        CurrentMapRow.MainMenuPageID = sMainPageID
        CurrentMapRow.MainMenuPageName = sMainPageName
        Return True
    End Function
    Public Function SetListPage(ByVal sQueryString As String, ByVal sServerName As String, ByVal sURL As String) As Boolean
        If (Left(sQueryString, 4) = "404;") Then
            mySession.ListPageURL = (Right(sQueryString, Len(CStr(sQueryString)) - 4))
        Else
            mySession.ListPageURL = ("http://" & sServerName & sURL & "?" & sQueryString)
        End If
        Return True
    End Function
    Public Function CheckCurrentSettings() As Boolean
        Dim sbError As New StringBuilder
        If (mySession.CompanyID = "") Then
            sbError.Append("&NoCompanyID=Failed")
        End If
        If Trim(mySession.SiteDB) = "" Then
            sbError.Append("&NoSiteDB=Failed")
        End If
        If sbError.Length > 0 Then
            HttpContext.Current.Response.Redirect("~" & mhConfig.mhWebHome & "debug.aspx?Error=TRUE" & sbError.ToString)
            Return False
        Else
            Return True
        End If
    End Function
    Public Function SetCurrentPageID(ByVal CurrentPageID As String) As Boolean
        If IsNothing(CurrentPageID) Then
            mySession.CurrentPageID = mySiteFile.SiteHomePageID
            CurrentMapRow.MainMenuPageID = mySiteFile.SiteHomePageID
        ElseIf IsDBNull(CurrentPageID) Then
            mySession.CurrentPageID = mySiteFile.SiteHomePageID
            CurrentMapRow.MainMenuPageID = mySiteFile.SiteHomePageID
        End If
        If CurrentPageID = "" Then
            mySession.CurrentPageID = mySiteFile.SiteHomePageID
            CurrentMapRow.MainMenuPageID = mySiteFile.SiteHomePageID
        End If
        Return True
    End Function
    Public Function SelectCurrentPageRow(ByVal CurrentPageID As String, ByVal CurrentArticleID As String) As Boolean
        CurrentMapRow = Me.mySiteFile.SiteMapRows.GetPageRow(CurrentPageID, CurrentArticleID)
        For Each bcRow As mhBreadCrumbRow In CurrentMapRow.BreadCrumbRows
            If bcRow.MenuLevelNBR = 1 Then
                SetMainPage(bcRow.PageID, bcRow.PageName)
            End If
        Next
        Return True
    End Function
    Private Sub ReadSiteFile()
        Try
            Dim sr As New StreamReader(mySession.SiteMapFilePath)
            Dim xs As New XmlSerializer(GetType(mhSiteFile))
            mySiteFile = DirectCast(xs.Deserialize(sr), mhSiteFile)
            sr.Close()
        Catch
        Finally
        End Try
    End Sub
    Public Function BuildTemplate(ByRef sbContent As StringBuilder) As Boolean
        Me.ReplaceLinkTags(sbContent)
        sbContent.Replace("~~PageAdmin~~", GetPageAdmin())
        sbContent.Replace("~~UserOptions~~", Me.GetUserOptions())
        sbContent.Replace("~~SiteTitle~~", Me.GetSiteTitle())
        sbContent.Replace("~~CurrentPageName~~", Me.CurrentMapRow.PageName)
        sbContent.Replace("~~CurrentPageDesc~~", Me.GetPageDescription())
        sbContent.Replace("~~CurrentPageKeywords~~", Me.GetPageKeywords())
        sbContent.Replace("~~ParentPageName~~", Me.CurrentMapRow.MainMenuPageName)
        sbContent.Replace("~~MainMenuName~~", Me.CurrentMapRow.MainMenuPageName)
        sbContent.Replace("~~BreadCrumbs~~", Me.CurrentMapRow.BreadCrumbHTML)
        sbContent.Replace("~~UserName~~", Me.mySession.ContactName)
        sbContent.Replace("~~Year~~", mhUTIL.FormatDate(Now, "Y"))
        sbContent.Replace("~~Today~~", mhUTIL.GetCurrentDate())
        sbContent.Replace("~~UserRole~~", Me.mySession.ContactRoleTitle)
        sbContent.Replace("~~UserPreferences~~", mhutil.FormatLink(mhSession.GetContactID(), "Preferences", "Preferences", "" & mhConfig.mhWebHome & "login/contact_edit.aspx?key=" & mhSession.GetContactID()))
        sbContent.Replace("~~PageArticles~~", mySiteFile.BuildPageArticle(Me.CurrentMapRow.PageID, Me.CurrentMapRow.ArticleID))
        sbContent.Replace("~~CurrentPageURL~~", Me.CurrentMapRow.DisplayURL)
        ' Menu Options 
        sbContent.Replace("~~TopMenu~~", Me.mySiteFile.BuildMenuChild(Me.CurrentMapRow.MainMenuPageID, "", ""))
        sbContent.Replace("~~SubTree~~", Me.mySiteFile.BuildPageTree(Me.CurrentMapRow.PageID, 0, "topmenu"))
        sbContent.Replace("~~MainSubTree~~", Me.mySiteFile.BuildPageTree(Me.CurrentMapRow.MainMenuPageID, 0, "topmenu"))
        sbContent.Replace("~~MainSubMenu~~", Me.mySiteFile.BuildMenuChild(Me.CurrentMapRow.PageID, Me.CurrentMapRow.MainMenuPageID, Me.CurrentMapRow.MainMenuPageID))
        sbContent.Replace("~~2ndMenu~~", Me.mySiteFile.BuildMenuChild(Me.CurrentMapRow.PageID, Me.CurrentMapRow.MainMenuPageID, Me.CurrentMapRow.MainMenuPageID))
        sbContent.Replace("~~3rdMenu~~", Me.mySiteFile.BuildLinkMenu(3, Me.CurrentMapRow.PageID, "~~LINKS~~", Me.CurrentMapRow))

        ' YUI Menu Options
        'sbContent.Replace("~~yuiTopMenu~~", Me.mySiteFile.yuiBuildMenuChild(Me.CurrentMapRow.MainMenuPageID, String.Empty, String.Empty, "yuiTopMenu"))
        'sbContent.Replace("~~yuiMainSubMenu~~", Me.mySiteFile.yuiBuildMenuChild(Me.CurrentMapRow.PageID, Me.CurrentMapRow.MainMenuPageID, Me.CurrentMapRow.MainMenuPageID, "yuiMainSubMenu"))
        sbContent.Replace("~~yuiMainTree~~", Me.mySiteFile.yuiBuildPageTree("", 0, "top"))
        sbContent.Replace("~~yuiSubMenu~~", Me.mySiteFile.yuiBuildPageList(Me.CurrentMapRow.MainMenuPageID, Me.CurrentMapRow.MainMenuPageName, Me.CurrentMapRow.PageID, 0))
        sbContent.Replace("~~yuiChildrenMenu~~", Me.mySiteFile.yuiBuildMenuChild(Me.CurrentMapRow.PageID, Me.CurrentMapRow.PageID, Me.CurrentMapRow.PageID, "yuiChildrenMenu"))
        sbContent.Replace("~~yuiSiblingMenu~~", Me.mySiteFile.yuiBuildMenuChild(Me.CurrentMapRow.PageID, Me.CurrentMapRow.ParentPageID, Me.CurrentMapRow.ParentPageID, "yuiSiblingMenu"))

        ' Alternate Menu Options
        sbContent.Replace("~~ParentMenu~~", Me.mySiteFile.BuildLinkListByParent(Me.CurrentMapRow.ParentPageID, Me.CurrentMapRow.ParentPageID, Me.CurrentMapRow.SiteCategoryID))
        sbContent.Replace("~~ChildrenMenu~~", Me.mySiteFile.BuildLinkListByParent(Me.CurrentMapRow.PageID, Me.CurrentMapRow.PageID, Me.CurrentMapRow.SiteCategoryID))
        sbContent.Replace("~~SiblingMenu~~", Me.mySiteFile.BuildLinkListBySibling(Me.CurrentMapRow.PageID, Me.CurrentMapRow.ParentPageID, Me.CurrentMapRow.SiteCategoryID))

        ' Replace Site Cateogry Tags
        For Each myGroup As mhSiteGroup In Me.mySiteFile.SiteGroupRows
            If (InStr(1, UCase(myGroup.SiteCategoryGroupNM), "TOP") > 0) Then
                sbContent.Replace("~~yui" & myGroup.SiteCategoryGroupNM & "~~", Me.mySiteFile.yuiBuildSiteCategoryGroupBar(String.Empty, myGroup.SiteCategoryGroupNM, Me.CurrentMapRow.PageID, 1))
                sbContent.Replace("~~" & myGroup.SiteCategoryGroupNM & "~~", Me.mySiteFile.BuildSiteCategoryGroupList(String.Empty, myGroup.SiteCategoryGroupNM, Me.CurrentMapRow.PageID, 1, myGroup.SiteCategoryGroupDS))
            Else
                sbContent.Replace("~~yui" & myGroup.SiteCategoryGroupNM & "~~", Me.mySiteFile.yuiBuildSiteCategoryGroupList(String.Empty, myGroup.SiteCategoryGroupNM, Me.CurrentMapRow.PageID, 1))
                sbContent.Replace("~~" & myGroup.SiteCategoryGroupNM & "~~", Me.mySiteFile.BuildSiteCategoryGroupList(String.Empty, myGroup.SiteCategoryGroupNM, Me.CurrentMapRow.PageID, 1, myGroup.SiteCategoryGroupDS))
            End If
        Next
        ' Replace Site Parameter Tags
        For Each mySiteParameter As mhSiteParameter In Me.mySiteFile.SiteParameterList
            sbContent.Replace("~~" & mySiteParameter.SiteParameterTypeNM & "~~", mySiteParameter.ParameterValue)
        Next
        ' Replace Company Tags
        Me.mySiteFile.ReplaceTags(sbContent)
        sbContent.Replace("~~debug~~", Me.mySession.GetSessionDebug())

        Return True
    End Function

    Private Function ReplaceLinkTags(ByRef sbContent As StringBuilder) As Boolean
        Dim sLeftLinks As String = ""
        Dim sRightLinks As String = ""
        Dim sCenterLinks As String = ""
        Dim sThisLink As String = ("")
        If (InStr(1, sbContent.ToString, "~~LeftColumnLinks~~") > 0) _
          Or (InStr(1, sbContent.ToString, "~~RightColumnLinks~~") > 0) _
         Or (InStr(1, sbContent.ToString, "~~CenterColumnLinks~~") > 0) Then
            If mySiteFile.SiteLinkRows.Count = 0 Then
                NoLinkRows(sRightLinks, sLeftLinks, sCenterLinks)
            Else
                For Each myrow As mhSiteLinkRow In mySiteFile.SiteLinkRows
                    If (myrow.LinkCategoryTitle.ToString = "LeftColumnLinks") _
                      Or (myrow.LinkCategoryTitle = "RightColumnLinks") _
                       Or (myrow.LinkCategoryTitle = "CenterColumnLinks") Then
                        Select Case myrow.LinkTypeCD
                            Case "MENU4"
                                sThisLink = mySiteFile.BuildLinkMenu(4, myrow.PageID, myrow.LinkURL, Me.CurrentMapRow)
                            Case "MENU3"
                                sThisLink = mySiteFile.BuildLinkMenu(3, myrow.PageID, myrow.LinkURL, Me.CurrentMapRow)
                            Case "MENU2"
                                sThisLink = mySiteFile.BuildLinkMenu(2, myrow.PageID, myrow.LinkURL, Me.CurrentMapRow)
                            Case "MENU1"
                                sThisLink = mySiteFile.BuildLinkMenu(1, myrow.PageID, myrow.LinkURL, Me.CurrentMapRow)
                            Case "SubMenu"
                                myrow.LinkURL = Replace(myrow.LinkURL, "~~MainMenuName~~", Me.CurrentMapRow.MainMenuPageName)
                                sThisLink = mySiteFile.BuildLinkMenu(2, myrow.PageID, myrow.LinkURL, Me.CurrentMapRow)
                            Case "FILE"
                                ' If the linkPageID is null or 0 apply it to all pages
                                ' If the linkPageID is equal to the current page, apply it
                                ' If the linkPageID is not null and not the current page, ignore it
                                If IsDBNull(myrow.PageID) Then
                                    sThisLink = "<a href=""" & myrow.LinkURL & """>" & myrow.LinkTitle & "</a><br />"
                                ElseIf (myrow.PageID = Me.CurrentMapRow.PageID) Then
                                    sThisLink = "<a href=""" & myrow.LinkURL & """>" & myrow.LinkTitle & "</a><br />"
                                ElseIf (CInt(myrow.PageID) = 0) Then
                                    sThisLink = "<a href=""" & myrow.LinkURL & """>" & myrow.LinkTitle & "</a><br />"
                                End If
                            Case "XML"
                                If myrow.PageID = Me.CurrentMapRow.PageID Or myrow.PageID = "" Then
                                    sThisLink = getXMLTransform(myrow.LinkTitle, myrow.LinkURL, myrow.LinkDescription)
                                End If
                            Case "RSS"
                                If myrow.PageID = Me.CurrentMapRow.PageID Or myrow.PageID = "" Then
                                    sThisLink = Me.getRSSFeed(myrow.LinkTitle, myrow.LinkURL, myrow.LinkDescription)
                                End If
                            Case "SiteCategory"
                                Dim sSiteCategory As String = myrow.LinkURL
                                For Each myGroup As mhSiteGroup In Me.mySiteFile.SiteGroupRows
                                    If (InStr(1, UCase(myGroup.SiteCategoryGroupNM), "TOP") > 0) Then
                                        sSiteCategory = Replace(sSiteCategory, "~~" & myGroup.SiteCategoryGroupNM & "~~", Me.mySiteFile.yuiBuildSiteCategoryGroupBar(myrow.PageID, myGroup.SiteCategoryGroupNM, myrow.PageID, 1))
                                    Else
                                        sSiteCategory = Replace(sSiteCategory, "~~" & myGroup.SiteCategoryGroupNM & "~~", Me.mySiteFile.yuiBuildSiteCategoryGroupList(myrow.PageID, myGroup.SiteCategoryGroupNM, myrow.PageID, 1))
                                    End If
                                Next
                                sThisLink = sSiteCategory
                            Case Else
                                ' If the linkPageID is null or 0 apply it to all pages
                                ' If the linkPageID is equal to the current page, apply it
                                ' If the linkPageID is not null and not the current page, ignore it
                                If IsDBNull(myrow.PageID) Then
                                    sThisLink = myrow.LinkURL
                                ElseIf (myrow.PageID = Me.CurrentMapRow.PageID) Then
                                    sThisLink = myrow.LinkURL
                                ElseIf myrow.PageID = "" Then
                                    sThisLink = myrow.LinkURL
                                End If
                        End Select
                        If sThisLink <> "" Then
                            If mhUser.IsAdmin() Then
                                If myrow.LinkSource = "SiteLink" Then
                                    sThisLink = sThisLink & "<div class=""admin"">(<a href=""" & mhConfig.mhASPMakerGen & "SiteLink_edit.aspx?ID=" & myrow.LinkID & """>edit " & myrow.LinkTitle & "</a>)</div>"
                                Else
                                    sThisLink = sThisLink & "<div class=""admin"">(<a href=""" & mhConfig.mhASPMakerGen & "link_edit.aspx?ID=" & myrow.LinkID & """>edit " & myrow.LinkTitle & "</a>)</div>"
                                End If
                            End If
                        End If
                        Select Case myrow.LinkCategoryTitle
                            Case "LeftColumnLinks"
                                sLeftLinks = sLeftLinks & sThisLink
                            Case "RightColumnLinks"
                                sRightLinks = sRightLinks & sThisLink
                            Case "CenterColumnLinks"
                                sCenterLinks = sCenterLinks & sThisLink
                            Case Else
                                ' do nothing
                        End Select
                        sThisLink = ""
                    End If
                Next
                If mhUser.IsAdmin() Then
                    If (sRightLinks = "") Then
                        sRightLinks = "<a href=""" & mhConfig.mhASPMakerGen & "link_list.aspx"">NO RIGHT LINKS</a>"
                    End If
                    If (sLeftLinks = "") Then
                        sLeftLinks = "<a href=""" & mhConfig.mhASPMakerGen & "link_list.aspx"">NO LEFT LINKS</a>"
                    End If
                    If (sCenterLinks = "") Then
                        sCenterLinks = "<a href=""" & mhConfig.mhASPMakerGen & "link_list.aspx"">NO CENTER LINKS</a>"
                    End If
                End If
            End If
            sbContent.Replace("~~LeftColumnLinks~~", sLeftLinks)
            sbContent.Replace("~~RightColumnLinks~~", sRightLinks)
            sbContent.Replace("~~CenterColumnLinks~~", sCenterLinks)
        End If
        Return True
    End Function
    Private Function GetUserOptions() As String
        Dim sReturn As String
        sReturn = ""
        If mhSession.GetContactID() <> "" Then
            sReturn = "<a href=""" & mhConfig.mhWebHome & "login/logout.aspx"" >Sign Out</a>"
        Else
            sReturn = "<a href=""" & mhConfig.mhWebHome & "login/login.aspx"">Sign On</a>"
        End If
        Return sReturn
    End Function
    Private Function GetSiteTitle() As String
        Dim mySiteTitle As New StringBuilder
        If Me.CurrentMapRow.PageTitle = "" Then
            mySiteTitle.Append(Me.mySiteFile.SiteTitle)
        Else
            mySiteTitle.Append(Me.CurrentMapRow.PageTitle)
        End If
        mySiteTitle.Replace("<PageName>", Me.CurrentMapRow.PageName)
        mySiteTitle.Replace("<PageTitle>", Me.CurrentMapRow.PageTitle)
        mySiteTitle.Replace("<PageDescription>", Me.CurrentMapRow.PageDescription)
        mySiteTitle.Replace("<PageKeywords>", Me.CurrentMapRow.PageKeywords)
        Return mySiteTitle.ToString
    End Function
    Private Function GetPageKeywords() As String
        Dim mySiteKeywords As New StringBuilder
        If Me.CurrentMapRow.PageKeywords = "" Then
            mySiteKeywords.Append(Me.mySiteFile.SiteKeywords)
        Else
            mySiteKeywords.Append(Me.CurrentMapRow.PageKeywords)
        End If
        mySiteKeywords.Replace("<PageName>", Me.CurrentMapRow.PageName)
        mySiteKeywords.Replace("<PageTitle>", Me.CurrentMapRow.PageTitle)
        mySiteKeywords.Replace("<PageDescription>", Me.CurrentMapRow.PageDescription)
        mySiteKeywords.Replace("<PageKeywords>", Me.CurrentMapRow.PageKeywords)
        mySiteKeywords.Replace("~~SiteCity~~", Me.mySiteFile.SiteCity)
        mySiteKeywords.Replace("~~SiteState~~", Me.mySiteFile.SiteState)
        Return mySiteKeywords.ToString
    End Function
    Private Function GetPageDescription() As String
        Dim mySiteDescription As New StringBuilder
        If Me.CurrentMapRow.PageKeywords = "" Then
            mySiteDescription.Append(Me.mySiteFile.SiteDescription)
        Else
            mySiteDescription.Append(Me.CurrentMapRow.PageDescription)
        End If
        mySiteDescription.Replace("<PageName>", Me.CurrentMapRow.PageName)
        mySiteDescription.Replace("<PageTitle>", Me.CurrentMapRow.PageTitle)
        mySiteDescription.Replace("<PageDescription>", Me.CurrentMapRow.PageDescription)
        mySiteDescription.Replace("<PageKeywords>", Me.CurrentMapRow.PageKeywords)
        mySiteDescription.Replace("~~SiteCity~~", Me.mySiteFile.SiteCity)
        mySiteDescription.Replace("~~SiteState~~", Me.mySiteFile.SiteState)
        Return mySiteDescription.ToString
    End Function
    Private Function NoLinkRows(ByRef sRightLinks As String, ByRef sLeftLinks As String, ByRef sCenterLinks As String) As Boolean
        If mhUser.IsAdmin() Then
            sRightLinks = "<a href=""" & mhConfig.mhASPMakerGen & "link_list.aspx"">NO RIGHT LINKS</a>"
            sLeftLinks = "<a href=""" & mhConfig.mhASPMakerGen & "link_list.aspx"">NO LEFT LINKS</a>"
            sCenterLinks = "<a href=""" & mhConfig.mhASPMakerGen & "link_list.aspx"">NO CENTER LINKS</a>"
        Else
            sRightLinks = ""
            sLeftLinks = ""
            sCenterLinks = ""
        End If
    End Function
    Private Function getXMLFileName(ByVal Company As String, ByVal Site As String, ByVal XMLType As String) As String
        Return mhConfig.mhWebConfigFolder & "xml\" & XMLType & "-" & Company & "-" & Site & ".xml"
    End Function
    Private Function getRSSFeed(ByVal sFeedName As String, ByVal sURL As String, ByVal sXSLTPath As String) As String
        Dim sItem As New StringBuilder
        Dim myXmlDoc As XmlDocument = New XmlDocument()
        Dim Path As String = getXMLFileName(mySession.CompanyID, "XML", sFeedName)
        Try
            If mhfio.IsValidPath(Path) Then
                If (mhfio.CompareFileAge(Path) > 60) Then
                    myXmlDoc.Load(sURL)
                    myXmlDoc.Save(Path)
                End If
            End If
        Catch ex As Exception
            mhUTIL.AuditLog("Problem getting External RSS Feed - (" & Path & ") - " & ex.ToString, "mhSiteMap.getXMLTransform")
        End Try
        Dim myrss As New mhweb.HGBRSSTools.HGBRSS(Path)
        Dim myOutput As New StringBuilder
        Dim sbBlogTemplate As New StringBuilder
        If Not mhfio.ReadTextFile(HttpContext.Current.Server.MapPath(Me.mySiteFile.SiteGallery & "/BlogPostsTemplate.txt"), sbBlogTemplate) Then
            mhfio.ReadTextFile(HttpContext.Current.Server.MapPath("/mhweb/blog/BlogPostsTemplate.txt"), sbBlogTemplate)
        End If
        If myrss.FeedIsGood Then
            For Each myitem As mhweb.HGBRSSTools.RSSItem In myrss.Items
                sItem.Append(sbBlogTemplate.ToString)
                sItem.Replace("~~PostID~~", "")
                sItem.Replace("~~PostDate~~", myitem.pubDate)
                sItem.Replace("~~PostURL~~", myitem.Link)
                sItem.Replace("~~PostName~~", myitem.Title)
                sItem.Replace("~~PostTitle~~", myitem.Title)
                sItem.Replace("~~PostBody~~", myitem.content)
                sItem.Replace("~~PostDescription~~", myitem.Description)
                sItem.Replace("~~BlogPageURL~~", myitem.Link)
                sItem.Replace("~~BlogPageName~~", myitem.Title)
                sItem.Replace("~~PostAuthor~~", myitem.author)
                sItem.Replace("~~ArticleAdmin~~", "")
            Next
        End If
        Return sItem.ToString
    End Function
    Private Function ConvertICStoXML() As Boolean


        Return True
    End Function

    Private Function getXMLTransform(ByVal sFeedName As String, ByVal sURL As String, ByVal sXSLTPath As String) As String
        Dim path As String
        Dim myXmlDoc As XmlDocument = New XmlDocument()
        Dim strXslFile As String = ("")
        Dim myXslDoc As XslCompiledTransform = New XslCompiledTransform()
        Dim myStringBuilder As StringBuilder = New StringBuilder()
        Dim myStringWriter As StringWriter = New StringWriter(myStringBuilder)
        path = getXMLFileName(mySession.CompanyID, "XML", sFeedName)
        sURL = mhUTIL.RemoveHtml(sURL)
        sURL = sURL.Replace("&amp;", "&")
        Try
            If mhfio.IsValidPath(path) Then
                If (mhfio.CompareFileAge(path) > 300) Then
                    Dim request As System.Net.WebRequest = System.Net.WebRequest.Create(sURL)
                    Dim response As System.Net.WebResponse = request.GetResponse()
                    Dim mystream As Stream = response.GetResponseStream()
                    Dim xmlreader As New XmlTextReader(mystream)
                    myXmlDoc.Load(xmlreader)
                    myXmlDoc.Save(path)
                Else
                    myXmlDoc.Load(path)
                End If
            End If
        Catch url_ex As Exception
            Try
                myXmlDoc.Load(path)
            Catch ex As Exception
                mhUTIL.AuditLog("URL Exception", url_ex.Message & " - on " & sURL)
                mhUTIL.AuditLog("Problem getting XML Feed - (" & path & ") - " & ex.ToString, "mhSiteMap.getXMLTransform")
            End Try
        End Try
        Try
            If sXSLTPath = "" Then
                strXslFile = (mhConfig.mhWebHome & "style\rss_title.xsl")
            Else
                strXslFile = mhConfig.mhWebHome & "style\" & sXSLTPath
            End If
            myXslDoc.Load(strXslFile)
            myXslDoc.Transform(myXmlDoc, Nothing, myStringWriter)
        Catch ex As Exception
            mhUTIL.AuditLog("Problem processing XSL/XML - (" & strXslFile & ") - " & ex.ToString, "mhSiteMap.getXMLTransform")
        End Try
        Return myStringBuilder.ToString()
    End Function
    Private Function GetPageAdmin() As String
        Dim sReturn As String = ("")
        If mhUser.IsAdmin() Then
            Select Case Me.CurrentMapRow.RecordSource
                Case "Page"
                    sReturn = ("<div class=""mhAdmin""><a href=""" & mhConfig.mhASPMakerGen & "page_edit.aspx?PageID=" & Me.CurrentMapRow.PageID & """>Page Properties</a> | <a href=""" & mhConfig.mhASPMakerGen & "page_add.aspx"">Add Page</a> | <a href=""" & mhConfig.mhASPMakerGen & "article_add.aspx"">Add Article</a> | <a href=""" & mhConfig.mhWebHome & "admin/admin.aspx"">Admin</a></div>")
                Case "Article"
                    sReturn = ("<div class=""mhAdmin""><a href=""" & mhConfig.mhASPMakerGen & "page_edit.aspx?PageID=" & Me.CurrentMapRow.PageID & """>Page Properties</a> | <a href=""" & mhConfig.mhASPMakerGen & "page_add.aspx"">Add Page</a> | <a href=""" & mhConfig.mhASPMakerGen & "article_add.aspx"">Add Article</a> | <a href=""" & mhConfig.mhWebHome & "admin/admin.aspx"">Admin</a></div>")
                Case "Category"
                    sReturn = ("<div class=""mhAdmin""><a href=""" & mhConfig.mhASPMakerGen & "SiteCategory_edit.aspx?SiteCategoryID=" & Replace(Me.CurrentMapRow.PageID, "CAT-", "") & """>Page Properties</a> | <a href=""" & mhConfig.mhWebHome & "admin/admin.aspx"">Admin</a></div>")
                Case Else
                    sReturn = ("<div class=""mhAdmin""><a href=""" & mhConfig.mhASPMakerGen & "page_add.aspx"">Add Page</a> | <a href=""" & mhConfig.mhASPMakerGen & "article_add.aspx"">Add Article</a> | <a href=""" & mhConfig.mhWebHome & "admin/admin.aspx"">Admin</a></div>")
            End Select
        End If
        Return sReturn
    End Function

    Public Function GetHTML(ByVal sMainContent As String, ByVal bUseDefault As Boolean, ByVal TemplatePrefix As String) As String
        If Me.CurrentMapRow.PageID = "" And Me.CurrentMapRow.ArticleID = "" And Trim(TemplatePrefix) = "" Then
            bUseDefault = True
        End If
        Dim mySiteTheme As New mhSiteTheme(Me, bUseDefault, TemplatePrefix)
        Dim myHTML As New StringBuilder(mySiteTheme.sbSiteTemplate.ToString)
        If myHTML.Length > 0 Then
            If (InStr(1, myHTML.ToString, "~~MainContent~~") > 0) Then
                myHTML.Replace("~~MainContent~~", sMainContent)
            End If
            myHTML.Replace("~~RightContent~~", Me.mySession.RightContent)
            myHTML.Replace("</head>", Me.mySession.AddHTMLHead & "</head>")
            Me.BuildTemplate(myHTML)
            ResetSessionVariables()
        End If
        Me.SetListPage(HttpContext.Current.Request.ServerVariables.Item("QUERY_STRING"), HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), HttpContext.Current.Request.ServerVariables.Item("URL"))
        '  mhUTIL.AccessLog(Me.CurrentMapRow.DisplayURL, Me.CurrentMapRow.TransferURL)
        '  mhfio.SaveHTML(Me.CurrentMapRow.DisplayURL, myHTML.ToString)

        Return myHTML.ToString
    End Function
    Public Function GetBlogPageHTML() As String
        Dim sbBlogTemplate As New StringBuilder
        If (Me.mySession.CurrentArticleID = "") Or (Me.mySession.CurrentArticleID = Me.mySiteFile.DefaultArticleID) Then
            If Not mhfio.ReadTextFile(HttpContext.Current.Server.MapPath(Me.mySiteFile.SiteGallery & "/BlogPostsTemplate.txt"), sbBlogTemplate) Then
                mhfio.ReadTextFile(HttpContext.Current.Server.MapPath("/mhweb/blog/BlogPostsTemplate.txt"), sbBlogTemplate)
            End If
        Else
            If Not mhfio.ReadTextFile(HttpContext.Current.Server.MapPath(Me.mySiteFile.SiteGallery & "/BlogPostTemplate.txt"), sbBlogTemplate) Then
                mhfio.ReadTextFile(HttpContext.Current.Server.MapPath("/mhweb/blog/BlogPostTemplate.txt"), sbBlogTemplate)
            End If
        End If
        Me.mySession.AddHTMLHead = vbCrLf & _
          "<link rel=""alternate"" type=""application/rss+xml"" title=""RSS 2.0"" href=""http://" & _
          HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME") & _
          "/mhweb/blog/rss_blog.aspx?c=" & Me.mySession.CurrentPageID & """ >" & vbCrLf
        Dim myBlog As New mhBlog(Me)
        Return GetHTML(myBlog.BuildBlogList(sbBlogTemplate, mhUTIL.GetDBInteger(GetProperty("Page", "0"))), False, Me.mySession.SiteTemplatePrefix)
    End Function

    Public Function GetArticlePageHTML() As String
        Dim myArticle As New mhArticle(Me.CurrentMapRow, Me.mySiteFile.DefaultArticleID)
        Return GetHTML(myArticle.ArticleBody, False, Me.mySession.SiteTemplatePrefix)
    End Function
    Public Function GetFormPageHTML() As String
        Dim mycontents As New StringBuilder
        mhfio.ReadTextFile(HttpContext.Current.Server.MapPath("/util/forms/AddToHTMLHead.txt"), mycontents)
        Me.mySession.AddHTMLHead = mycontents.ToString
        Dim myArticle As New mhArticle(Me.CurrentMapRow.ArticleID, Me.CurrentMapRow.PageID, Me.mySiteFile.DefaultArticleID)
        Return GetHTML("<form action=""/mhweb/mhForm.aspx"" method=""post"">" & vbCrLf & myArticle.ArticleBody & vbCrLf & "</form>", False, Me.mySession.SiteTemplatePrefix)
    End Function
    Public Function GetCatalogPageHTML() As String
        Dim myCat As New mhcatalog(Me)
        Return GetHTML(myCat.ProcessPageRequest(HttpContext.Current.Request.Item("Page"), Me.CurrentMapRow.ArticleID, HttpContext.Current.Request.Item("Template")), False, "")
    End Function

    Public Function GetLinkAdmin() As String
        Dim myLinkDirectory As New mhLinkDirectory(Me)
        myLinkDirectory.CreateAdminLinkDirectory(Me)
        Return myLinkDirectory.MyStringBuilder.ToString
    End Function
    Public Function GetLinkDirectoryHTML() As String
        Dim myLinkDirectory As New mhLinkDirectory(Me)
        myLinkDirectory.CreateLinkDirectory(Me)
        Return GetHTML(myLinkDirectory.MyStringBuilder.ToString, False, Me.mySession.SiteTemplatePrefix)
    End Function
    Public Function GetLinkPageHTML() As String
        Dim myLinkDirectory As New mhLinkDirectory(Me)
        myLinkDirectory.DrawYUILinks(Me)
        Return GetHTML(myLinkDirectory.MyStringBuilder.ToString, False, Me.mySession.SiteTemplatePrefix)
    End Function

    Public Sub ResetSessionVariables()
        Me.mySession.CurrentArticleID = ""
        Me.mySession.CurrentPageID = ""
        Me.mySession.RightContent = ""
        Me.mySession.AddHTMLHead = ""
    End Sub
    Public Function GetProperty(ByVal myProperty As String, ByVal curValue As String) As String
        Dim myValue As String = ""
        If Len(HttpContext.Current.Request.QueryString(myProperty)) > 0 Then
            myValue = HttpContext.Current.Request.QueryString(myProperty).ToString
        ElseIf Len(HttpContext.Current.Request.Form.Item(myProperty)) > 0 Then
            myValue = HttpContext.Current.Request.Form.Item(myProperty).ToString
        ElseIf Len(HttpContext.Current.Request(myProperty)) > 0 Then
            myValue = HttpContext.Current.Request(myProperty).ToString
        Else
            myValue = curValue
        End If
        Return myValue
    End Function
    Private Function GetConfig() As Boolean
        Dim SiteConfig As MHSiteSettings = MHSiteSettings.Load(mhConfig.mhWebConfig)
        mySession.CompanyID = (SiteConfig.mhSite.CompanyID.ToString)
        mySession.SiteDB = (SiteConfig.mhSite.SQLDBConnString.ToString)
        Return True
    End Function
    Private Sub ResetCurrentRow()
        Me.CurrentMapRow.PageTypeCD = "404"
        Me.CurrentMapRow.PageID = ""
        Me.CurrentMapRow.ArticleID = ""
        Me.CurrentMapRow.TransferURL = ""
        Me.CurrentMapRow.MainMenuPageID = ""
        Me.CurrentMapRow.PageName = "404 - File Not Found"
        Me.CurrentMapRow.PageTitle = "404 - File Not Found"
    End Sub
    Public Function Process404(ByVal RawURL As String, ByVal QueryString As String) As Boolean
        Dim bReturn As Boolean = True
        Dim sTransferURL As String = ""
        Dim sRedirectURL As String = ""
        ResetCurrentRow()
        RawURL = Replace(RawURL, "/mhweb/404.aspx?404;", "")
        QueryString = Replace(QueryString, ":80", "")
        sTransferURL = GetTransferURL(QueryString)
        If sTransferURL = "" Then
            sRedirectURL = GetRedirectURL(QueryString)
            If (sRedirectURL = "") Then
                If (UCase(Right(QueryString, 4)) = "HTML" Or _
                    UCase(Right(QueryString, 4)) = ".HTM") Then
                    If (CStr(sTransferURL) = "") Then
                        ' Build a 404 Page
                        bReturn = False
                    Else
                        TransferToURL(sTransferURL)
                    End If
                Else
                    If (Right(QueryString, 11) = "sitemap.xml") Then
                        Me.CurrentMapRow.PageTypeCD = "sitemap.xml"
                        sTransferURL = "" & mhConfig.mhWebHome & "sitemap.aspx"
                    End If
                    If (Right(QueryString, 12) = "sitemap_view") Then
                        Me.CurrentMapRow.PageTypeCD = "sitemap_view"
                        sTransferURL = "" & mhConfig.mhWebHome & "sitemap_view.aspx"
                    End If
                    If (Right(QueryString, 7) = "sitemap") Then
                        Me.CurrentMapRow.PageTypeCD = "sitemap"
                        sTransferURL = "" & mhConfig.mhWebHome & "site.aspx"
                    End If
                    If (Right(QueryString, 12) = "rss_menu.xml") Then
                        Me.CurrentMapRow.PageTypeCD = "rss_menu.xml"
                        sTransferURL = "" & mhConfig.mhWebHome & "rss_menu.aspx"
                    End If
                    If sTransferURL = "" Then
                        ' Log the error and return FALSE
                        Me.CurrentMapRow.PageTypeCD = "404"
                        bReturn = False
                    Else
                        TransferToURL(sTransferURL)
                    End If
                End If
            Else
                Build301Redirect(sRedirectURL)
                bReturn = False
            End If
        Else
            TransferToURL(sTransferURL)
        End If
        Return bReturn
    End Function
    Private Sub TransferToURL(ByVal sTransferURL As String)
        sTransferURL = Replace("~/" & sTransferURL, "//", "/")
        If CurrentMapRow.TransferURL <> sTransferURL Then
            CurrentMapRow.TransferURL = sTransferURL
        End If
    End Sub
    Private Function Build301Redirect(ByVal sNewURL As String) As Boolean
        HttpContext.Current.Response.Status = "301 Moved Permanently"
        HttpContext.Current.Response.AddHeader("Location", sNewURL)
        Return True
    End Function
    Private Function BuildErrorMessage() As String
        Dim item As Object
        Dim strReturn As String = ""
        strReturn = Replace(HttpContext.Current.Request.ServerVariables.Item("QUERY_STRING"), "404;", "") & "<br/><br/>Sorry, the file you were looking for can not be found. It may have moved to a new location.<br /><br />You can go to our <A href=""/"">homepage</A> or look in our sitemap:<br /><br />"
        If mhUser.IsAdmin() Then
            strReturn = strReturn & ("<table border=1>")
            For Each item In HttpContext.Current.Session.Contents
                strReturn = strReturn & ("<tr><td>")
                strReturn = strReturn & (item).ToString
                strReturn = strReturn & ("</td><td>&nbsp;")
                strReturn = strReturn & (HttpContext.Current.Session.Contents.Item(CInt(item))).ToString
                strReturn = strReturn & ("</td></tr>")
            Next item
            strReturn = strReturn & ("</table><br/><br/>")
        End If
        Return strReturn
    End Function
    Private Function GetTransferURL(ByVal refer As String) As String
        Dim indexc As Integer = 0
        Dim realpage As String = ""
        Dim iQuestionMark As Integer = 0
        ' Get Full relative path of request
        Dim strPath As String = GetPath(refer)
        ' Remove the QueryString for purposes of finding the right page
        iQuestionMark = InStr(strPath, "?")
        If (iQuestionMark > 0) Then
            strPath = Left(strPath, iQuestionMark - 1)
        End If
        Return GetPageURL(strPath, True)
    End Function
    Private Function GetPath(ByVal refer As String) As String
        Dim sPath As String = ("")
        Dim indexDomain As Integer = 0
        Dim sBasePath As String = ("http://" & HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"))
        indexDomain = InStr(refer, sBasePath)
        If Not IsNothing(refer) Or Not (refer = "") Then
            If ((refer.Length - sBasePath.Length) - indexDomain > 0) Then
                sPath = Right(refer, (refer.Length - sBasePath.Length) - indexDomain)
            End If
        End If
        Return sPath
    End Function
    Private Function GetRedirectURL(ByVal refer As String) As String
        Dim strpage As String = ""
        Dim indexc As Integer = 0
        Dim realpage As String = ""
        Dim iQuestionMark As Integer = 0
        indexc = InStrRev(refer, "/")
        If (indexc > 0) Then
            strpage = Right(refer, Len(refer) - indexc)
            refer = Left(refer, indexc - 1)
        End If
        iQuestionMark = InStr(strpage, "?")
        If (iQuestionMark > 0) Then
            strpage = Left(strpage, iQuestionMark - 1)
        End If
        Return GetPageURL(strpage, False)
    End Function
    Public Function FindCurrentRow(ByVal sPageID As String, ByVal sArticleID As String, ByVal sRecordSource As String) As Boolean
        Dim bReturn As Boolean = False
        For Each myrow As mhSiteMapRow In mySiteFile.SiteMapRows
            If myrow.PageID = sPageID And myrow.ArticleID = sArticleID And myrow.RecordSource = sRecordSource Then
                Me.CurrentMapRow.UpdatePageRow(myrow)
                For Each bcRow As mhBreadCrumbRow In CurrentMapRow.BreadCrumbRows
                    If bcRow.MenuLevelNBR = 1 Then
                        SetMainPage(bcRow.PageID, bcRow.PageName)
                    End If
                Next
                bReturn = True
            End If
        Next
        Return bReturn
    End Function
    Private Function GetPageURL(ByVal urlName As String, ByVal bStrict As Boolean) As String
        Dim LinkURL As String = ""
        Dim bMatch As Boolean = False
        Dim pageURL As String = ("")
        Dim indexc As Integer = 0
        For Each myrow As mhSiteMapRow In mySiteFile.SiteMapRows
            bMatch = False
            If mySiteFile.UseBreadCrumbURL Then
                pageURL = myrow.BreadCrumbURL
            Else
                pageURL = myrow.DisplayURL
            End If

            If (bStrict) Then
                If ("/" & urlName = pageURL) Then
                    bMatch = True
                End If
            Else
                If Not mySiteFile.UseBreadCrumbURL Then
                    indexc = InStrRev(urlName, "/")
                    If (indexc > 0) Then
                        urlName = Right(urlName, Len(urlName) - indexc)
                        urlName = Left(urlName, indexc - 1)
                    End If
                End If
                bMatch = CheckForMatch(urlName, myrow.DisplayURL)
            End If
            If (bMatch) Then
                If (myrow.ActiveFL Or mhUser.IsAdmin()) Then
                    If (bStrict) Then
                        LinkURL = myrow.TransferURL
                        Me.CurrentMapRow.UpdatePageRow(myrow)

                        For Each bcRow As mhBreadCrumbRow In CurrentMapRow.BreadCrumbRows
                            If bcRow.MenuLevelNBR = 1 Then
                                SetMainPage(bcRow.PageID, bcRow.PageName)
                            End If
                        Next
                    Else
                        LinkURL = pageURL
                    End If
                Else
                    If (bStrict) Then
                        LinkURL = ""
                    Else
                        LinkURL = pageURL
                    End If
                End If
                Exit For
            End If
        Next
        If Not bStrict And Not bMatch Then
            LinkURL = GetPageAliasURL(urlName)
        End If
        Return LinkURL
    End Function
    Private Function GetPageAliasURL(ByVal urlName As String) As String
        Dim LinkURL As String = String.Empty
        For Each myPageAlias As mhPageAlias In mySiteFile.PageAliasRows
            If (CheckForMatch(urlName, myPageAlias.PageURL)) Then
                LinkURL = myPageAlias.TransferURL
                Exit For
            End If
        Next
        Return LinkURL
    End Function

    Private Function CheckForMatch(ByVal pageName As String, ByVal urlName As String) As Boolean
        Dim bMatch As Boolean = False
        ' To Make this Easier, let's ignore case and spaces and apmersands and dashes
        urlName = LCase(urlName)
        urlName = Replace(urlName, "/", "")
        urlName = Replace(urlName, ".html", "")
        urlName = Replace(urlName, ".htm", "")
        urlName = Replace(urlName, "&amp;", "&")
        urlName = Replace(urlName, "%20", "")
        urlName = Replace(urlName, "-", "")
        urlName = Replace(urlName, " ", "")

        urlName = Replace(urlName, mhConfig.DefaultExtension, "")
        pageName = LCase(pageName)
        pageName = Replace(pageName, "/", "")
        pageName = Replace(pageName, ".html", "")
        pageName = Replace(pageName, ".htm", "")
        pageName = Replace(pageName, "%20", "")
        pageName = Replace(pageName, " ", "")
        pageName = Replace(pageName, "&amp;", "&")
        pageName = Replace(pageName, "-", "")
        pageName = Replace(pageName, mhConfig.DefaultExtension, "")
        If (urlName = pageName) Then
            bMatch = True
        Else
            bMatch = False
        End If
        Return bMatch
    End Function
    Public Function getXMLTransform(ByVal XMLFilePath As String, ByVal XSLFilePath As String) As String
        Dim myXmlDoc As XmlDocument = New XmlDocument()
        Dim myXslDoc As XslCompiledTransform = New XslCompiledTransform()
        Dim myStringBuilder As StringBuilder = New StringBuilder()
        Dim myStringWriter As StringWriter = New StringWriter(myStringBuilder)

        Try
            myXmlDoc.Load(XMLFilePath)
        Catch ex As Exception
            mhUTIL.AuditLog("URL Exception", ex.Message & " - on " & XMLFilePath)
            mhUTIL.AuditLog("Problem getting XML Feed - (" & XMLFilePath & ") - " & ex.ToString, "mhSiteMap.getXMLTransform")
        End Try

        Try
            myXslDoc.Load(XSLFilePath)
            myXslDoc.Transform(myXmlDoc, Nothing, myStringWriter)
        Catch ex As Exception
            mhUTIL.AuditLog("Problem processing XSL/XML - (" & XSLFilePath & ") - " & ex.ToString, "mhSiteMap.getXMLTransform")
        End Try
        Return myStringBuilder.ToString()
    End Function

End Class
