Imports System.Text
Imports System.Web

Public Class ActiveCompany
#Region "properties"
    Private CurCompany As New Company
    Public CurLocation As New Location
    Public Property UseDefaultTemplate() As Boolean
#End Region
#Region "consturctors"
    Public Sub New()
        MyBase.New()
        wpm_CheckCommandParameters()
        wpm_SetPageHistory()
        If Not LoadSiteProfile("ORDER") Then
            wpm_SetGenericError("ActiveCompany.New - Site Profile Did not load")
            HttpContext.Current.Response.Redirect("/GenericErrorPage.aspx")
        End If
        With CurLocation
            .LocationTypeCD = "404"
            .LocationID = ""
            .ArticleID = 0
            .TransferURL = ""
            .MainMenuLocationID = ""
            .LocationName = wpm_FileNotFound
            .LocationTitle = wpm_FileNotFound
        End With
    End Sub
    Public Sub New(ByVal OrderBy As String)
        MyBase.New()
        wpm_CheckCommandParameters()
        wpm_SetPageHistory()
        If Not LoadSiteProfile(OrderBy) Then
            wpm_SetGenericError("ActiveCompany.New - Site Profile Did not load")
            HttpContext.Current.Response.Redirect("/GenericErrorPage.aspx")
        End If
        With CurLocation
            .LocationTypeCD = "404"
            .LocationID = ""
            .ArticleID = 0
            .TransferURL = ""
            .MainMenuLocationID = ""
            .LocationName = wpm_FileNotFound
            .LocationTitle = wpm_FileNotFound
        End With

    End Sub
#End Region



    Public Function SetCurrentPageID(ByVal reqCurrentPageID As String) As Boolean
        If IsNothing(reqCurrentPageID) Or IsDBNull(reqCurrentPageID) Or reqCurrentPageID = "" Then
            wpm_CurrentPageID = CurCompany.HomeLocationID
            CurLocation.MainMenuLocationID = CurCompany.HomeLocationID
        Else
            wpm_CurrentPageID = reqCurrentPageID
            CurLocation.MainMenuLocationID = CurCompany.HomeLocationID
        End If

        'If IsNothing(wpm_CurrentPageID) Or IsDBNull(wpm_CurrentPageID) Or wpm_CurrentPageID = "" Then
        '    Return False
        'Else
        '    Return True
        'End If
        Return True
    End Function


    Public Function SelectCurrentPageRow(ByVal CurrentPageID As String, ByVal CurrentArticleID As Integer) As Boolean
        CurLocation = CurCompany.Locations.FindLocation(CurrentPageID, CurrentArticleID)
        For Each bcRow As LocationTrail In CurLocation.LocationTrailList
            If bcRow.MenuLevelNBR = 1 Then
                SetMainPage(bcRow.LocationID, bcRow.Name)
            End If
        Next
        Return True
    End Function
    Public Sub ProcessTag(ByRef sbContent As StringBuilder, ByVal sTag As String)
        If wpm_TagExists(sbContent, sTag) Then
            Select Case sTag
                Case "~~SiteTitle~~"
                    sbContent.Replace("~~SiteTitle~~", GetSiteTitle())
                Case "~~SiteName~~"
                    sbContent.Replace("~~SiteName~~", CurCompany.CompanyNM)
                Case "~~SiteTagLine~~"
                    sbContent.Replace("~~SiteTagLine~~", CurCompany.CompanyTitle)
                Case "~~CurrentURL~~"
                    sbContent.Replace("~~CurrentURL~~", CurrentDisplayURL)
                Case "~~PageAdmin~~"
                    sbContent.Replace("~~PageAdmin~~", GetPageAdmin())
                Case "~~SiteDirectory~~"
                    sbContent.Replace("~~SiteDirectory~~", CurCompany.SiteGallery)
                Case "~~UserOptions~~"
                    sbContent.Replace("~~UserOptions~~", wpm_GetUserOptions())
                Case "~~CurrentPageID~~"
                    sbContent.Replace("~~CurrentPageID~~", CurLocation.LocationID)
                Case "~~CurrentPageName~~"
                    sbContent.Replace("~~CurrentPageName~~", CurLocation.LocationName)
                Case "~~CurrentPageDesc~~"
                    sbContent.Replace("~~CurrentPageDesc~~", GetPageDescription())
                Case "~~CurrentPageKeywords~~"
                    sbContent.Replace("~~CurrentPageKeywords~~", GetPageKeywords())
                Case "~~CurrentPageURL~~"
                    sbContent.Replace("~~CurrentPageURL~~", CurLocation.LocationURL)
                Case "~~ParentPageName~~"
                    sbContent.Replace("~~ParentPageName~~", CurLocation.MainMenuLocationName)
                Case "~~MainMenuName~~"
                    sbContent.Replace("~~MainMenuName~~", CurLocation.MainMenuLocationName)
                Case "~~BreadCrumbs~~"
                    sbContent.Replace("~~BreadCrumbs~~", CurLocation.BreadCrumbHTML)
                Case "~~UserName~~"
                    sbContent.Replace("~~UserName~~", wpm_ContactName)
                Case "~~Year~~"
                    sbContent.Replace("~~Year~~", wpm_FormatDate(Now, "Y"))
                Case "~~Today~~"
                    sbContent.Replace("~~Today~~", wpm_GetCurrentDate())
                Case "~~KeywordSearch~~"
                    sbContent.Replace("~~KeywordSearch~~", ProcessKeywordSearch(wpm_SiteGallery))
                Case "~~KeywordResults~~"
                    sbContent.Replace("~~KeywordResults~~", ProcessKeywordResults(wpm_SiteGallery))
                Case "~~UserPreferences~~"
                    sbContent.Replace("~~UserPreferences~~", wpm_FormatLink("Preferences", "Preferences", String.Format("{0}login/contact_edit.aspx?key={1}", wpm_SiteConfig.ApplicationHome(), wpm_GetContactID())))
                Case "~~PageArticles~~"
                    sbContent.Replace("~~PageArticles~~", CurCompany.BuildPageArticle(CurLocation))
                Case "~~nav-bar~~"
                    sbContent.Replace("~~nav-bar~~", CurCompany.BuildMenuChild(CurLocation.MainMenuLocationID, String.Empty, String.Empty, "nav-bar", String.Empty))
                Case "~~side-nav~~"
                    sbContent.Replace("~~side-nav~~", CurCompany.Locations.BuildPageTree(CurLocation.MainMenuLocationID, 0, "side-nav"))

                Case "~~nav-stacked~~"
                    sbContent.Replace("~~nav-stacked~~", CurCompany.Locations.BuildPageTree(CurLocation.MainMenuLocationID, 0, "nav nav-stacked nav-pills"))

                Case "~~TopMenu~~"
                    sbContent.Replace("~~TopMenu~~", CurCompany.BuildMenuChild(CurLocation.MainMenuLocationID, String.Empty, String.Empty, String.Empty, String.Empty))
                Case "~~SubTree~~"
                    sbContent.Replace("~~SubTree~~", CurCompany.Locations.BuildPageTree(String.Empty, 0, "topmenu"))
                Case "~~MainSubTree~~"
                    sbContent.Replace("~~MainSubTree~~", CurCompany.Locations.BuildPageTree(CurLocation.MainMenuLocationID, 0, "topmenu"))
                Case "~~MainSubMenu~~"
                    sbContent.Replace("~~MainSubMenu~~", CurCompany.BuildMenuChild(CurLocation.LocationID, CurLocation.MainMenuLocationID, CurLocation.MainMenuLocationID, String.Empty, String.Empty))
                Case "~~SubMenu~~"
                    sbContent.Replace("~~SubMenu~~", CurCompany.BuildMenuChild(CurLocation.LocationID, CurLocation.MainMenuLocationID, CurLocation.MainMenuLocationID, String.Empty, String.Empty))
                Case "~~2ndMenu~~"
                    sbContent.Replace("~~2ndMenu~~", CurCompany.BuildMenuChild(CurLocation.LocationID, CurLocation.MainMenuLocationID, CurLocation.MainMenuLocationID, String.Empty, String.Empty))
                Case "~~3rdMenu~~"
                    sbContent.Replace("~~3rdMenu~~", CurCompany.BuildLinkMenu(3, CurLocation.LocationID, "~~LINKS~~", CurLocation))
                Case "~~FoundationMenuTree~~"
                    sbContent.Replace("~~FoundationMenuTree~~", CurCompany.BuildFoundationNavSection(String.Empty, 0))
                Case "~~TopMenuTree~~"
                    sbContent.Replace("~~TopMenuTree~~", CurCompany.BuildTopMenuTree(String.Empty, 0, String.Empty))
                Case "~~yuiMainTree~~"
                    sbContent.Replace("~~yuiMainTree~~", CurCompany.yuiBuildPageTree("", 0, "top"))
                Case "~~yuiSubMenu~~"
                    sbContent.Replace("~~yuiSubMenu~~", CurCompany.yuiBuildPageList(CurLocation.MainMenuLocationID, CurLocation.MainMenuLocationName, CurLocation.LocationID, 0))
                Case "~~yuiChildrenMenu~~"
                    sbContent.Replace("~~yuiChildrenMenu~~", CurCompany.yuiBuildMenuChild(CurLocation.LocationID, CurLocation.LocationID, "yuiChildrenMenu"))
                Case "~~yuiSiblingMenu~~"
                    sbContent.Replace("~~yuiSiblingMenu~~", CurCompany.yuiBuildMenuChild(CurLocation.LocationID, CurLocation.ParentLocationID, "yuiSiblingMenu"))
                Case "~~mooMenuTree~~"
                    sbContent.Replace("~~mooMenuTree~~", CurCompany.mooBuildPageList(String.Empty, CurLocation.LocationID, 0))
                Case "~~fndNavBar~~"
                    sbContent.Replace("~~fndNavBar~~", CurCompany.fndBuilNavBar(String.Empty, CurLocation.LocationID, 0))
                Case "~~bootNavBar~~"
                    sbContent.Replace("~~bootNavBar~~", CurCompany.bootBuilNavBar(String.Empty, CurLocation.LocationID, 0))
                Case "~~ParentMenu~~"
                    sbContent.Replace("~~ParentMenu~~", CurCompany.BuildLinkListByParent(CurLocation.ParentLocationID, CurLocation.ParentLocationID, CurLocation.SiteCategoryID))
                Case "~~ChildrenMenu~~"
                    sbContent.Replace("~~ChildrenMenu~~", CurCompany.BuildLinkListByParent(CurLocation.LocationID, CurLocation.LocationID, CurLocation.SiteCategoryID))
                Case "~~BreadCrumbChildren~~"
                    sbContent.Replace("~~BreadCrumbChildren~~", CurCompany.BreadCrumbWithChildren(CurLocation.BreadCrumbHTML, CurLocation.LocationID, CurLocation.LocationID, CurLocation.SiteCategoryID))
                Case "~~SiblingMenu~~"
                    sbContent.Replace("~~SiblingMenu~~", CurCompany.BuildLinkListBySibling(CurLocation.LocationID, CurLocation.ParentLocationID))
                Case Else
                    ' do nothing 
            End Select
        End If
    End Sub
    Public Function BuildTemplate(ByRef sbContent As StringBuilder) As Boolean
        ReplaceLinkTags(sbContent)
        ReplaceTags(CurLocation, sbContent)
        ProcessTag(sbContent, "~~SiteTitle~~")
        ProcessTag(sbContent, "~~SiteName~~")
        ProcessTag(sbContent, "~~SiteTagLine~~")
        If Not (CurCompany.Parameters.ReplaceTags(sbContent, CurLocation)) Then
            ApplicationLogging.ErrorLog("Error With ReplaceSiteParameterTags", "ActiveCompany.BuildTemplate(sbContent)")
        End If
        ProcessTag(sbContent, "~~CurrentURL~~")
        ProcessTag(sbContent, "~~PageAdmin~~")
        ProcessTag(sbContent, "~~SiteDirectory~~")
        ProcessTag(sbContent, "~~UserOptions~~")
        ProcessTag(sbContent, "~~CurrentPageID~~")
        ProcessTag(sbContent, "~~CurrentPageName~~")
        ProcessTag(sbContent, "~~CurrentPageDesc~~")
        ProcessTag(sbContent, "~~CurrentPageKeywords~~")
        ProcessTag(sbContent, "~~CurrentPageURL~~")
        ProcessTag(sbContent, "~~ParentPageName~~")
        ProcessTag(sbContent, "~~MainMenuName~~")
        ProcessTag(sbContent, "~~BreadCrumbs~~")
        ProcessTag(sbContent, "~~UserName~~")
        ProcessTag(sbContent, "~~Year~~")
        ProcessTag(sbContent, "~~Today~~")
        ProcessTag(sbContent, "~~UserPreferences~~")
        ProcessTag(sbContent, "~~PageArticles~~")
        ProcessTag(sbContent, "~~KeywordSearch~~")
        ProcessTag(sbContent, "~~KeywordResults~~")
        ' Menu Options 
        ProcessTag(sbContent, "~~nav-bar~~")
        ProcessTag(sbContent, "~~side-nav~~")
        ProcessTag(sbContent, "~~TopMenu~~")
        ProcessTag(sbContent, "~~SubTree~~")
        ProcessTag(sbContent, "~~MainSubTree~~")
        ProcessTag(sbContent, "~~MainSubMenu~~")
        ProcessTag(sbContent, "~~SubMenu~~")
        ProcessTag(sbContent, "~~2ndMenu~~")
        ProcessTag(sbContent, "~~3rdMenu~~")
        ProcessTag(sbContent, "~~TopMenuTree~~")
        ProcessTag(sbContent, "~~FoundationMenuTree~~")
        ProcessTag(sbContent, "~~nav-stacked~~")

        ' YUI Menu Options
        ProcessTag(sbContent, "~~yuiMainTree~~")
        ProcessTag(sbContent, "~~yuiSubMenu~~")
        ProcessTag(sbContent, "~~yuiChildrenMenu~~")
        ProcessTag(sbContent, "~~yuiSiblingMenu~~")
        ' MOO Tools Options
        ProcessTag(sbContent, "~~mooMenuTree~~")
        ' Zurb Foundation Options
        ProcessTag(sbContent, "~~fndNavBar~~")

        ' Bootstrap Options
        ProcessTag(sbContent, "~~bootNavBar~~")

        ' Alternate Menu Options
        ProcessTag(sbContent, "~~ParentMenu~~")
        ProcessTag(sbContent, "~~ChildrenMenu~~")
        ProcessTag(sbContent, "~~BreadCrumbChildren~~")
        ProcessTag(sbContent, "~~SiblingMenu~~")

        ' Replace Site Cateogry Tags
        ReplaceSiteCategoryTags(sbContent)
        ' Replace Company Tags
        CurCompany.ReplaceTags(sbContent)
        pm_AddPageHistory(CurLocation.LocationName)
        ReplaceTags(CurLocation, sbContent)
        Return True
    End Function
    Public Function WriteCurrentLocation() As Boolean
        Dim response As HttpResponse = HttpContext.Current.Response
        wpm_CurrentPageID = CurLocation.LocationID
        wpm_CurrentArticleID = CurLocation.ArticleID
        Select Case (CurLocation.LocationTypeCD.ToUpper)
            Case "PAGE"
                response.Write(GetArticlePageHTML())
            Case "SITECAT"
                response.Write(GetArticlePageHTML())
            Case "MAIN"
                response.Write(GetArticlePageHTML())
            Case "ARTICLE"
                response.Write(GetArticlePageHTML())
            Case "BLOG"
                response.Write(GetBlogPageHTML())
            Case "GAME"
                response.Write(GetGamePageHTML())
            Case "GALLERY"
                response.Write(GetCatalogPageHTML())
            Case "CATALOG"
                response.Write(GetCatalogPageHTML())
            Case "FORM"
                response.Write(GetFormPageHTML())
            Case "LINK LIST"
                response.Write(GetLinkPageHTML())
            Case "LINK DIR"
                response.Write(GetLinkDirectoryHTML())
            Case "MODULE"
                response.Write(GetModulePageHTML())
            Case "LINK"
                response.Write(GetLinkDirectoryHTML())
            Case "SITEMAP"
                CurLocation.LocationName = CurCompany.DomainName & " Sitemap"
                CurLocation.LocationTitle = CurCompany.DomainName & " Sitemap"
                response.Write(GetHTML(CurCompany.CompanyDescription & CurCompany.Locations.BuildPageTree("", 0, "sitemap"), False, wpm_SiteTemplatePrefix))
            Case "SITEMAP.XML"
                response.ContentType = "text/xml"
                Using gen As SiteMapXmlTextWriter = New SiteMapXmlTextWriter(response.Output)
                    gen.WriteSitemapDocument()
                End Using
            Case Else
                ProcessUnknownLocationTypeCD(response)
        End Select
        Return True
    End Function
    Public Function GetHTML(ByVal sMainContent As String) As String
        Dim myHTML As New StringBuilder(sMainContent)
        If myHTML.Length > 0 Then
            ' myHTML.Replace("~~RightContent~~", Session.RightContent)
            BuildTemplate(myHTML)
            myHTML.Replace("</head>", wpm_AddHTMLHead & "</head>")
            wpm_ResetSessionVariables()
        End If
        wpm_SetListPageURL(HttpContext.Current.Request.ServerVariables.Item("QUERY_STRING"), HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), HttpContext.Current.Request.ServerVariables.Item("URL"))
        'If App.Config.FullLoggingOn() Then
        'LocationBuildHTML.SaveHTML(CurLocation.GetLocationURL, myHTML.ToString)
        'End If
        Return myHTML.ToString
    End Function
    Public Function GetHTML(ByVal sMainContent As String, ByVal bUseDefault As Boolean, ByVal TemplatePrefix As String) As String
        If CurLocation.LocationID = "" And CurLocation.ArticleID < 1 And Trim(TemplatePrefix) = "" Then
            bUseDefault = True
        End If
        If UseDefaultTemplate Then
            bUseDefault = True
        End If
        Dim mySiteTheme As New SiteTemplate(bUseDefault, TemplatePrefix)
        Dim myHTML As New StringBuilder(mySiteTheme.sbSiteTemplate.ToString)
        If myHTML.Length > 0 Then
            If (InStr(1, myHTML.ToString, "~~MainContent~~") > 0) Then
                myHTML.Replace("~~MainContent~~", sMainContent)
            End If
            ' myHTML.Replace("~~RightContent~~", Session.RightContent)
            BuildTemplate(myHTML)
            myHTML.Replace("</head>", wpm_AddHTMLHead & "</head>")
            'If App.IsAdmin Then
            '    myHTML.Replace("</body>", SessionDebug & "</body>")
            'End If
            wpm_ResetSessionVariables()
        End If
        '  wpm_SetListPageURL(HttpContext.Current.Request.ServerVariables.Item("QUERY_STRING"), HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), HttpContext.Current.Request.ServerVariables.Item("URL"))
        ' If App.Config.FullLoggingOn() Then
        ' LocationBuildHTML.SaveHTML(CurLocation.LocationName, myHTML.ToString)
        ' End If
        Return myHTML.ToString
    End Function
    Public Function GetGamePageHTML() As String
        Dim sbGameTemplate As New StringBuilder
        If (wpm_GetProperty("GameName", String.Empty) = String.Empty) Then
            If Not FileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath(CurCompany.SiteGallery & "/GamesTemplate.txt"), sbGameTemplate) Then
                FileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath(String.Format("{0}game/GamesTemplate.txt", wpm_SiteConfig.ApplicationHome)), sbGameTemplate)
            End If
        Else
            If Not FileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath(String.Format("{0}/GameTemplate.txt", CurCompany.SiteGallery)), sbGameTemplate) Then
                FileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath(String.Format("{0}game/GameTemplate.txt", wpm_SiteConfig.ApplicationHome)), sbGameTemplate)
            End If
        End If
        Dim myGameList As New FlashGameList
        myGameList.LoadFlashList(CurLocation)

        Return GetHTML(myGameList.BuildGameList(sbGameTemplate, wpm_GetDBInteger(wpm_GetProperty("Page", "0"), 0), wpm_GetProperty("GameName", String.Empty)), False, wpm_SiteTemplatePrefix)
    End Function
    Public Function GetBlogPageHTML() As String
        Dim sbBlogTemplate As New StringBuilder
        If (wpm_CurrentArticleID < 1) Then
            If Not FileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath(CurCompany.SiteGallery & "/BlogPostsTemplate.txt"), sbBlogTemplate) Then
                FileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath(String.Format("{0}blog/BlogPostsTemplate.txt", wpm_SiteConfig.ApplicationHome)), sbBlogTemplate)
            End If
        Else
            If Not FileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath(String.Format("{0}/BlogPostTemplate.txt", CurCompany.SiteGallery)), sbBlogTemplate) Then
                FileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath(String.Format("{0}blog/BlogPostTemplate.txt", wpm_SiteConfig.ApplicationHome)), sbBlogTemplate)
            End If
        End If

        wpm_AddHTMLHead = String.Format("{0}<link rel=""alternate"" type=""application/rss+xml"" title=""RSS 2.0"" href=""http://{1}{3}blog/rss_blog.aspx?c={2}"" >{0}", _
                                     vbCrLf, _
                                     HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), _
                                     wpm_CurrentPageID, wpm_SiteConfig.ApplicationHome)
        Dim myBlog As New ArticleList(Me)
        Return GetHTML(myBlog.BuildBlogList(sbBlogTemplate, wpm_GetDBInteger(wpm_GetProperty("Page", "0"))), False, wpm_SiteTemplatePrefix)
    End Function
    Public Function GetModulePageHTML() As String
        Dim myArticle As New Article(CurLocation, CurCompany.DefaultArticleID)
        Return GetHTML(myArticle.ArticleBody)
    End Function
    Public Function GetArticlePageHTML() As String
        Dim myArticle As New Article(CurLocation, CurCompany.DefaultArticleID)
        Return GetHTML(myArticle.ArticleBody, False, wpm_SiteTemplatePrefix)
    End Function
    Public Function GetCatalogPageHTML() As String
        Dim myImageList As New LocationImageList(Me)
        Dim myArticle As New Article(CurLocation, 0)

        Return GetHTML(myImageList.ProcessPageRequest(CurLocation.ArticleID, myArticle), False, "")
    End Function

    Public Function GetFormPageHTML() As String
        Dim mycontents As New StringBuilder
        FileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath(String.Format("{0}form/AddToHTMLHead.txt", wpm_SiteConfig.ApplicationHome)), mycontents)
        wpm_AddHTMLHead = mycontents.ToString
        Dim myArticle As New Article(CurLocation.ArticleID, CurLocation.LocationID, CurCompany.DefaultArticleID)
        If InStr(myArticle.ArticleBody, "<FORM", CompareMethod.Text) > 0 Or InStr(myArticle.ArticleBody, "<form", CompareMethod.Text) > 0 Then
            Return GetHTML(vbCrLf & myArticle.ArticleBody & vbCrLf, False, wpm_SiteTemplatePrefix)
        Else
            Return GetHTML(String.Format("<form action=""{2}Form.aspx"" method=""post"">{0}{1}{0}</form>", _
                               vbCrLf, _
                               myArticle.ArticleBody, wpm_SiteConfig.ApplicationHome), False, wpm_SiteTemplatePrefix)
        End If
    End Function
    Public Function GetSiteLocationAdmin() As String
        wpm_ListPageURL = HttpContext.Current.Request.FilePath()
        LocationList.Sort()
        Return ""
    End Function
    Public Function GetSitePartAdmin() As String
        Dim myLinkDirectory As New PartDirectory(Me)
        wpm_ListPageURL = HttpContext.Current.Request.FilePath()
        myLinkDirectory.CreateAdminPartDirectory()
        Return myLinkDirectory.MyStringBuilder.ToString
    End Function
    Public Function GetLinkDirectoryHTML() As String
        Dim myLinkDirectory As New PartDirectory(Me)
        myLinkDirectory.CreateLinkDirectory()
        Return GetHTML(myLinkDirectory.MyStringBuilder.ToString, False, wpm_SiteTemplatePrefix)
    End Function
    Public Function GetLinkPageHTML() As String
        Dim myLinkDirectory As New PartDirectory(Me)
        myLinkDirectory.DrawYUILinks(Me)
        Return GetHTML(myLinkDirectory.MyStringBuilder.ToString, False, wpm_SiteTemplatePrefix)
    End Function
    Public Function FindCurrentRow(ByVal sPageID As String, ByVal sArticleID As Integer, ByVal sRecordSource As String) As Boolean
        Dim bReturn As Boolean = False
        For Each myrow As Location In CurCompany.Locations
            If myrow.LocationID = sPageID And myrow.ArticleID = sArticleID And myrow.RecordSource = sRecordSource Then
                CurLocation.CopyLocation(myrow)
                For Each bcRow As LocationTrail In CurLocation.LocationTrailList
                    If bcRow.MenuLevelNBR = 1 Then
                        SetMainPage(bcRow.LocationID, bcRow.Name)
                    End If
                Next
                bReturn = True
            End If
        Next
        Return bReturn
    End Function
    Public Function ReplaceTags(ByVal sValue As String, ByRef myLocation As Location) As String
        If sValue <> String.Empty And sValue.Contains("~~") Then
            Dim mySB As New StringBuilder(sValue)
            ReplaceSiteCategoryTags(mySB)
            CurCompany.Parameters.ReplaceTags(mySB, myLocation)
            CurCompany.ReplaceTags(mySB)
            ReplaceTags(myLocation, mySB)
            sValue = mySB.ToString
        End If
        Return sValue
    End Function
    Public Function GetLocationImages(ByVal LocationID As String) As List(Of LocationImage)
        Return CurCompany.GetLocationImages(LocationID)
    End Function

#Region "privatefunctions"
    Public Function LoadSiteProfile(ByVal OrderBy As String) As Boolean
        Dim result As Boolean = True
        If wpm_CurrentSiteID = String.Empty Then
            wpm_CurrentSiteID = wpm_DomainConfig.CompanyID
        End If

        If wpm_CurrentSiteID = String.Empty Then
            result = False
        Else
            If wpm_IsAdmin Then
                HttpContext.Current.Application(wpm_SiteMapName) = Nothing
            End If
            If wpm_SiteConfig.CachingEnabled AndAlso OrderBy = "ORDER" AndAlso wpm_CurrentUserGroupID = "4" Then
                If HttpContext.Current.Application(wpm_SiteMapName) Is Nothing Then
                    If CurCompany.GetCompanyFromDB(wpm_CurrentSiteID, wpm_CurrentUserGroupID, OrderBy) Then
                        Try
                            HttpContext.Current.Application(wpm_SiteMapName) = CurCompany
                        Catch ex As Exception
                            ApplicationLogging.ErrorLog(String.Format("Error When updating Application variable ({0}) - {1}", wpm_SiteMapName, ex), "ActiveCompany.LoadSiteProfile")
                        End Try
                    Else
                        ApplicationLogging.ErrorLog(String.Format("Failed to GetCompanyFromDB ({0})", wpm_CurrentSiteID), "ActiveCompany.LoadSiteProfile")
                        result = False
                    End If
                Else
                    Try
                        CurCompany = TryCast(HttpContext.Current.Application(wpm_SiteMapName), Company)
                        If CurCompany.DomainName Is Nothing Then
                            result = CurCompany.GetCompanyFromDB(wpm_CurrentSiteID, wpm_CurrentUserGroupID, OrderBy)
                        End If
                    Catch ex As Exception
                        ApplicationLogging.ErrorLog(String.Format("Error when reading Application variable ({0}) - {1}", wpm_SiteMapName, ex), "ActiveCompany.LoadSiteProfile")
                        result = False
                    End Try
                End If
            Else
                result = CurCompany.GetCompanyFromDB(wpm_CurrentSiteID, wpm_CurrentUserGroupID, OrderBy)
            End If

            If result Then
                wpm_SiteGallery = CurCompany.SiteGallery
                If wpm_ContactID = String.Empty Then
                    wpm_CurrentUserGroupID = "4"
                    wpm_SetContactID(String.Empty)
                    wpm_ContactName = String.Empty
                    wpm_ContactEmail = String.Empty
                    'Session.RightContent = String.Empty
                    If wpm_SiteTemplatePrefix = String.Empty Then
                        wpm_SiteTemplatePrefix = CurCompany.SitePrefix
                        wpm_DefaultSitePrefix = CurCompany.DefaultSitePrefix
                    End If
                End If
                wpm_CurrentSiteCategoryTypeID = CurCompany.SiteCategoryTypeID
                wpm_CurrentSiteName = CurCompany.CompanyNM
                If wpm_CheckCurrentSettings() Then
                    result = SetCurrentPageID(wpm_CurrentPageID)
                    SelectCurrentPageRow((wpm_CurrentPageID), (wpm_CurrentArticleID))
                End If
            End If
        End If
        Return result
    End Function
    Private Function SetMainPage(ByVal sMainPageID As String, ByVal sMainPageName As String) As Boolean
        CurLocation.MainMenuLocationID = sMainPageID
        CurLocation.MainMenuLocationName = sMainPageName
        Return True
    End Function
    Private Function ReplaceSiteCategoryTags(ByRef sbContent As StringBuilder) As Boolean
        For Each myGroup As LocationGroup In CurCompany.LocationGroups
            If (InStr(1, (myGroup.LocationGroupNM.ToUpper), "TOP") > 0) Then
                sbContent.Replace(String.Format("~~yui{0}~~", myGroup.LocationGroupNM), CurCompany.yuiBuildSiteCategoryGroupBar(String.Empty, myGroup.LocationGroupNM, CurLocation.LocationID, 1))
                sbContent.Replace(String.Format("~~{0}~~", myGroup.LocationGroupNM), CurCompany.BuildSiteCategoryGroupList(String.Empty, myGroup.LocationGroupNM, CurLocation.LocationID, 1, myGroup.LocationGroupDS))
            Else
                sbContent.Replace(String.Format("~~yui{0}~~", myGroup.LocationGroupNM), CurCompany.yuiBuildSiteCategoryGroupList(String.Empty, myGroup.LocationGroupNM, CurLocation.LocationID, 1))
                sbContent.Replace(String.Format("~~{0}~~", myGroup.LocationGroupNM), CurCompany.BuildSiteCategoryGroupList(String.Empty, myGroup.LocationGroupNM, CurLocation.LocationID, 1, myGroup.LocationGroupDS))
            End If
        Next
        Return True
    End Function
    Private Sub ProcessFileLinkTypeCD(ByRef sThisLink As String, ByVal myrow As Part)
        ' If the linkPageID is null or 0 apply it to all pages
        ' If the linkPageID is equal to the current page, apply it
        ' If the linkPageID is not null and not the current page, ignore it
        If IsDBNull(myrow.LocationID) Then
            sThisLink = String.Format("<a href=""{0}"">{1}</a><br />", myrow.URL, myrow.Title)
        ElseIf (myrow.LocationID = CurLocation.LocationID) Then
            sThisLink = String.Format("<a href=""{0}"">{1}</a><br />", myrow.URL, myrow.Title)
        ElseIf (CInt(myrow.LocationID) = 0) Then
            sThisLink = String.Format("<a href=""{0}"">{1}</a><br />", myrow.URL, myrow.Title)
        End If
    End Sub
    Private Function ProcessXMLLinkTypeCD(ByVal myrow As Part) As String
        If PartIsUsed(myrow) Then
            Dim mySB As New StringBuilder(myrow.URL)
            If mySB.ToString.Contains("~~") Then
                If Not (CurCompany.Parameters.ReplaceTags(mySB, CurLocation)) Then
                    ApplicationLogging.ErrorLog("Error With ReplaceSiteParameterTags", "wpmSiteMap.ReplaceLinks-XML")
                End If
                mySB.Replace("~~PageKeywords~~", GetPageKeywords.Replace(" ", "+"))

                If Not CurLocation.LocationName Is Nothing Then
                    mySB.Replace("~~PageName~~", CurLocation.LocationName.Replace(" ", "+"))
                    mySB.Replace("~~CurrentPageName~~", CurLocation.LocationName.Replace(" ", "+"))
                End If

                CurCompany.ReplaceTags(mySB)
            End If
            Dim myXML As New UtilityXMLDocument(myrow.Title, mySB.ToString, myrow.Description, wpm_SiteGallery)
            Return myXML.getXMLTransform()
        Else
            Return String.Empty
        End If

    End Function
    Private Sub ProcessRSSLinkTypeCD(ByRef sThisLink As String, ByVal myrow As Part)
        If myrow.LocationID = CurLocation.LocationID Or myrow.LocationID = "" Then
            Dim myRSS As New UtilityRSSTools.RSSFeed(myrow.URL)
            sThisLink = myRSS.getRSSFeed(wpm_SiteGallery)
        End If
    End Sub
    Private Function ProcessSubMenuLinkTypeCD(ByVal myrow As Part) As String
        Dim sThisLink As String
        myrow.URL = Replace(myrow.URL, "~~MainMenuName~~", CurLocation.MainMenuLocationName)
        sThisLink = CurCompany.BuildLinkMenu(2, myrow.LocationID, myrow.URL, CurLocation)
        Return sThisLink
    End Function
    Private Function ProcessSiteCategoryLinkTypeCD(ByVal myrow As Part) As String
        Dim sThisLink As String
        Dim sSiteCategory As String = myrow.URL
        For Each myGroup As LocationGroup In CurCompany.LocationGroups
            If (InStr(1, (myGroup.LocationGroupNM.ToUpper), "TOP") > 0) Then
                sSiteCategory = Replace(sSiteCategory, String.Format("~~{0}~~", myGroup.LocationGroupNM), CurCompany.yuiBuildSiteCategoryGroupBar(myrow.LocationID, myGroup.LocationGroupNM, myrow.LocationID, 1))
            Else
                sSiteCategory = Replace(sSiteCategory,
                                    String.Format("~~{0}~~", myGroup.LocationGroupNM),
                                    CurCompany.yuiBuildSiteCategoryGroupList(myrow.LocationID, myGroup.LocationGroupNM, myrow.LocationID, 1))
            End If
        Next
        sThisLink = sSiteCategory
        Return sThisLink
    End Function
    Private Sub ProcessUnknownLinkTypeCD(ByRef sThisLink As String, ByVal myrow As Part)
        If PartIsUsed(myrow) Then
            sThisLink = myrow.URL
        End If
    End Sub
    Private Function PartIsUsed(ByVal myrow As Part) As Boolean
        Dim bReturn As Boolean
        If CurLocation.LocationGroupID <> String.Empty AndAlso myrow.SiteCategoryGroupID = CurLocation.LocationGroupID Then
            bReturn = True
        Else
            bReturn = False
        End If
        If (myrow.LocationID = CurLocation.LocationID) Then
            bReturn = True
        End If
        If myrow.LocationID = String.Empty Then
            bReturn = True
        End If
        If (CurLocation.RecordSource = "Article" AndAlso myrow.LocationID = CurLocation.ParentLocationID) Then
            bReturn = True
        End If
        Return bReturn
    End Function
    Private Shared Sub AddAdminLinksToModule(ByRef sThisLink As String, ByVal myrow As Part)
        If sThisLink <> "" Then
            If wpm_IsAdmin Then
                If myrow.PartSource = "SiteLink" Then
                    sThisLink = String.Format("{0}<div class=""admin"">(<a href=""{1}maint/default.aspx?type=Part&PartID={2}"">edit {3}</a>)</div>",
                                    sThisLink,
                                    wpm_SiteConfig.AdminFolder,
                                    myrow.PartID,
                                    myrow.Title)
                Else
                    sThisLink = String.Format("{0}<div class=""admin"">(<a href=""{1}maint/default.aspx?type=Part&PartID={2}"">edit {3}</a>)</div>",
                                    sThisLink,
                                    wpm_SiteConfig.AdminFolder,
                                    myrow.PartID,
                                    myrow.Title)
                End If
            End If
        End If
    End Sub
    Private Sub UpdateLinkHTML(ByRef sThisLink As String, ByVal myrow As Part)
        If PartIsUsed(myrow) Then
            Select Case myrow.PartTypeCD
                Case "MENU5"
                    sThisLink = CurCompany.BuildLinkMenu(5, myrow.LocationID, myrow.URL, CurLocation)
                Case "MENU4"
                    sThisLink = CurCompany.BuildLinkMenu(4, myrow.LocationID, myrow.URL, CurLocation)
                Case "MENU3"
                    sThisLink = CurCompany.BuildLinkMenu(3, myrow.LocationID, myrow.URL, CurLocation)
                Case "MENU2"
                    sThisLink = CurCompany.BuildLinkMenu(2, myrow.LocationID, myrow.URL, CurLocation)
                Case "MENU1"
                    sThisLink = CurCompany.BuildLinkMenu(1, myrow.LocationID, myrow.URL, CurLocation)
                Case "SubMenu"
                    sThisLink = ProcessSubMenuLinkTypeCD(myrow)
                Case "FILE"
                    ProcessFileLinkTypeCD(sThisLink, myrow)
                Case "XML"
                    sThisLink = ProcessXMLLinkTypeCD(myrow)
                Case "RSS"
                    ProcessRSSLinkTypeCD(sThisLink, myrow)
                Case "SiteCategory"
                    sThisLink = ProcessSiteCategoryLinkTypeCD(myrow)
                Case "KeywordSearch"
                    sThisLink = ProcessKeywordSearch(wpm_SiteGallery)
                Case Else
                    ProcessUnknownLinkTypeCD(sThisLink, myrow)
            End Select
        End If
    End Sub
    Private Sub ProcessActiveParts(ByRef sLeftLinks As String, ByRef sRightLinks As String, ByRef sCenterLinks As String, ByVal sThisLink As String)
        For Each myrow As Part In CurCompany.Parts.GetActiveParts
            UpdateLinkHTML(sThisLink, myrow)
            AddAdminLinksToModule(sThisLink, myrow)
            Select Case myrow.PartCategoryTitle
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
        Next
    End Sub
    Private Function ReplaceLinkTags(ByRef sbContent As StringBuilder) As Boolean
        Dim sLeftLinks As String = ""
        Dim sRightLinks As String = ""
        Dim sCenterLinks As String = ""
        Dim sThisLink As String = ("")
        If (InStr(1, sbContent.ToString, "~~LeftColumnLinks~~") > 0) _
          Or (InStr(1, sbContent.ToString, "~~RightColumnLinks~~") > 0) _
          Or (InStr(1, sbContent.ToString, "~~CenterColumnLinks~~") > 0) Then
            If CurCompany.Parts.Count = 0 Then
                wpm_SetMissingParts(sRightLinks, sLeftLinks, sCenterLinks)
            Else
                ProcessActiveParts(sLeftLinks, sRightLinks, sCenterLinks, sThisLink)
                If wpm_IsAdmin Then
                    If (sRightLinks = "") Then
                        sRightLinks = String.Format("<a href=""{0}maint/default.aspx?type=Part"">NO RIGHT LINKS</a>", wpm_SiteConfig.AdminFolder)
                    End If
                    If (sLeftLinks = "") Then
                        sLeftLinks = String.Format("<a href=""{0}maint/default.aspx?type=Part"">NO LEFT LINKS</a>", wpm_SiteConfig.AdminFolder)
                    End If
                    If (sCenterLinks = "") Then
                        sCenterLinks = String.Format("<a href=""{0}maint/default.aspx?type=Part"">NO CENTER LINKS</a>", wpm_SiteConfig.AdminFolder)
                    End If
                End If
            End If
            sbContent.Replace("~~LeftColumnLinks~~", sLeftLinks)
            sbContent.Replace("~~RightColumnLinks~~", sRightLinks)
            sbContent.Replace("~~CenterColumnLinks~~", sCenterLinks)
        End If
        Return True
    End Function
    Private Function ProcessKeywordResults(ByVal SiteGallery As String) As String
        Dim myReturn As New StringBuilder(String.Empty)
        FileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath(String.Format("{0}search/searchfield.html", SiteGallery)), myReturn)
        Dim mySearchKeyword As String = wpm_GetProperty("searchfield", String.Empty)
        If mySearchKeyword <> String.Empty Then
            ApplicationLogging.SearchLog(mySearchKeyword, "KeywordSearch")
            myReturn.Replace("~~SearchReturn~~", LocationList.FindLocationsByKeyword(mySearchKeyword))
        Else
            myReturn.Replace("~~SearchReturn~~", String.Empty)
        End If
        Return myReturn.ToString
    End Function
    Private Function ProcessKeywordSearch(ByVal SiteGallery As String) As String
        Dim myReturn As New StringBuilder(String.Empty)
        For Each myString As LocationKeyword In LocationList.KeywordList
            myReturn.Append(myString.Code & ",")
        Next
        Dim myContents As New StringBuilder
        FileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath(String.Format("{0}/search/searchfield.js", SiteGallery)), myContents)
        myContents.Replace("<keywordlist>", myReturn.ToString)

        wpm_AddHTMLHead = (String.Format("<script type=""text/javascript"">{0}</script>", myContents))
        wpm_AddHTMLHead = (String.Format("<link rel=""stylesheet"" type=""text/css"" href=""{0}search/searchfield.css"">", SiteGallery))

        myReturn.Length = 0
        FileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath(String.Format("{0}search/searchfield.html", SiteGallery)), myReturn)

        Dim mySearchKeyword As String = wpm_GetProperty("searchfield", String.Empty)

        If mySearchKeyword <> String.Empty Then
            ApplicationLogging.SearchLog(mySearchKeyword, "KeywordSearch")
            myReturn.Replace("~~SearchReturn~~", LocationList.FindLocationsByKeyword(mySearchKeyword))
        Else
            myReturn.Replace("~~SearchReturn~~", String.Empty)
        End If
        Return myReturn.ToString
    End Function
    Private Function GetSiteTitle() As String
        Dim mySiteTitle As New StringBuilder
        If CurLocation.LocationTitle = "" Then
            mySiteTitle.Append(CurCompany.CompanyTitle)
        Else
            mySiteTitle.Append(CurLocation.LocationTitle)
        End If
        ReplaceTags(CurLocation, mySiteTitle)
        CurCompany.ReplaceTags(mySiteTitle)
        Return mySiteTitle.ToString
    End Function
    Private Function GetPageKeywords() As String
        Dim mySiteKeywords As New StringBuilder
        If CurLocation.LocationKeywords = "" Then
            mySiteKeywords.Append(CurCompany.CompanyKeywords)
        Else
            mySiteKeywords.Append(CurLocation.LocationKeywords)
        End If
        ReplaceTags(CurLocation, mySiteKeywords)
        CurCompany.ReplaceTags(mySiteKeywords)
        Return mySiteKeywords.ToString
    End Function
    Private Function GetPageDescription() As String
        Dim mySiteDescription As New StringBuilder
        If CurLocation.LocationKeywords = "" Then
            mySiteDescription.Append(CurCompany.CompanyDescription)
        Else
            mySiteDescription.Append(CurLocation.LocationDescription)
        End If
        ReplaceTags(CurLocation, mySiteDescription)
        CurCompany.ReplaceTags(mySiteDescription)
        Return mySiteDescription.ToString
    End Function
    Private Function GetPageAdmin() As String
        Dim sReturn As String = ("")
        If wpm_IsAdmin Then
            Select Case CurLocation.RecordSource
                Case "Page"
                    sReturn = (String.Format("<div class=""wpmADMIN""><a href=""{0}maint/default.aspx?type=Location&LocationID={1}"">Location Properties</a> | <a href=""{2}AdminLink.aspx"">All Parts</a> | <a href=""{2}default.aspx"">Admin Home</a> </div>", wpm_SiteConfig.AdminFolder, CurLocation.LocationID, wpm_SiteConfig.AdminFolder()))
                Case "Article"
                    sReturn = (String.Format("<div class=""wpmADMIN""><a href=""{0}maint/default.aspx?type=Location&LocationID={1}"">Location Properties</a> | <a href=""{2}AdminLink.aspx"">All Parts</a> | <a href=""{2}default.aspx"">Admin Home</a> </div>", wpm_SiteConfig.AdminFolder, CurLocation.LocationID, wpm_SiteConfig.AdminFolder()))
                Case "Category"
                    sReturn = (String.Format("<div class=""wpmADMIN""><a href=""{0}maint/default.aspx?type=Location&LocationID=CAT-{1}"">Location Properties</a> | <a href=""{2}AdminLink.aspx"">All Parts</a> | <a href=""{2}default.aspx"">Admin Home</a> </div>", wpm_SiteConfig.AdminFolder, Replace(CurLocation.LocationID, "CAT-", ""), wpm_SiteConfig.AdminFolder()))
                Case "Module"
                    sReturn = (String.Format("<div class=""wpmADMIN""><a href=""{0}maint/default.aspx?type=Location&LocationID={1}"">Location Properties</a> | <a href=""{2}AdminLink.aspx"">All Parts</a> | <a href=""{2}default.aspx"">Admin Home</a> </div>", wpm_SiteConfig.AdminFolder, CurLocation.LocationID, wpm_SiteConfig.AdminFolder()))
                Case Else
                    sReturn = (String.Format("<div class=""wpmADMIN""><a href=""{0}maint/default.aspx?type=Location&LocationID={1}"">Location List</a> | <a href=""{0}maint/default.aspx?type=Article"">Article List</a> | <a href=""{2}AdminLink.aspx"">All Parts</a> | <a href=""{2}default.aspx"">Admin Home</a> </div>", wpm_SiteConfig.AdminFolder, CurLocation.LocationID, wpm_SiteConfig.AdminFolder()))
            End Select
        End If
        Return sReturn
    End Function
    Private Sub ProcessUnknownLocationTypeCD(ByVal response As HttpResponse)
        If CurLocation.TransferURL Is Nothing Then
            ApplicationLogging.ErrorLog("Current Location Empty", "Redirect to ADMIN")
            If DefaultArticleID < 1 And SiteHomePageID = String.Empty And CurCompany.SiteCategoryTypeID = String.Empty Then
                If HttpContext.Current.Request.RawUrl <> CurCompany.CompanyURL Then
                    If CurCompany.CompanyURL Is Nothing Then
                        ApplicationLogging.ErrorLog("SiteURL is nothing", "Redirect to debug")
                        response.Redirect(String.Format("~{0}debug.aspx?Error=NoSiteURL", wpm_SiteConfig.ApplicationHome), True)
                    Else
                        If wpm_SiteConfig.FullLoggingOn Then
                            ApplicationLogging.ErrorLog("301-Redirect", CurCompany.CompanyURL)
                        End If
                        wpm_Build301Redirect(CurCompany.CompanyURL)
                    End If
                Else
                    ApplicationLogging.ErrorLog("Missing Key Site Components", "Redirect to debug")
                    response.Redirect(String.Format("~{0}debug.aspx?Error=NoKeyComponents", wpm_SiteConfig.ApplicationHome), True)
                End If
            Else
                If SiteHomePageID = String.Empty Then
                    ApplicationLogging.ErrorLog("SiteHomePageID Missing", "Redirect to debug")
                    response.Redirect(String.Format("~{0}debug.aspx?Error=NoSiteHomePageID", wpm_SiteConfig.ApplicationHome), True)
                Else
                    SetCurrentPageID(SiteHomePageID)
                    SelectCurrentPageRow(SiteHomePageID, 0)
                    response.Write(GetArticlePageHTML())
                End If
            End If
        Else
            If Left(CurLocation.TransferURL, 4) = "http" Then
                HttpContext.Current.Response.Redirect(CurLocation.TransferURL, True)
            Else
                If String.IsNullOrEmpty(CurLocation.TransferURL) Then
                    ' Need to check for global Page Alias
                    CurLocation.TransferURL = LocationAliasList.LookupTargetURL(String.Empty)
                    If String.IsNullOrEmpty(CurLocation.TransferURL) Then
                        response.Write(GetArticlePageHTML())
                    Else
                        HttpContext.Current.Response.Redirect(CurLocation.TransferURL, True)
                    End If
                Else
                    HttpContext.Current.Server.Transfer(CurLocation.TransferURL, True)
                End If
            End If
        End If
    End Sub
    Private Sub ResetCurrentRow()
        With CurLocation
            .LocationTypeCD = "404"
            .LocationID = ""
            .ArticleID = 0
            .TransferURL = ""
            .MainMenuLocationID = ""
            .LocationName = wpm_FileNotFound
            .LocationTitle = wpm_FileNotFound
        End With
    End Sub
    Public Function SetCurrentLocation(ByVal urlName As String, ByVal bStrict As Boolean) As String
        Dim LocationURL As String = String.Empty
        Dim bMatch As Boolean = False
        Dim pageURL As String = String.Empty
        Dim indexc As Integer = 0

        For Each myLocation As Location In CurCompany.Locations
            bMatch = False
            If CurCompany.UseBreadCrumbURL Then
                pageURL = myLocation.BreadCrumbURL
            Else
                pageURL = myLocation.LocationURL
            End If

            If (bStrict) Then
                If (urlName = pageURL) Then
                    bMatch = True
                End If
            Else
                indexc = InStrRev(urlName, "/")
                If indexc = Len(urlName) Then
                    urlName = Left(urlName, Len(urlName) - 1)
                Else
                    If (indexc > 1) Then
                        urlName = Right(urlName, Len(urlName) - indexc + 1)
                    End If
                End If
                bMatch = wpm_CheckForMatch(urlName, myLocation.LocationURL)
            End If
            If (bMatch) Then
                If (myLocation.ActiveFL Or wpm_IsAdmin) Then
                    If (bStrict) Then
                        LocationURL = myLocation.TransferURL
                        CurLocation.CopyLocation(myLocation)
                        For Each bcRow As LocationTrail In CurLocation.LocationTrailList
                            If bcRow.MenuLevelNBR = 1 Then
                                SetMainPage(bcRow.LocationID, bcRow.Name)
                            End If
                        Next
                    Else
                        LocationURL = pageURL
                    End If
                Else
                    If (bStrict) Then
                        LocationURL = String.Empty
                    Else
                        LocationURL = pageURL
                    End If
                End If
                Exit For
            End If
        Next
        Return LocationURL
    End Function

    Public Sub ReplaceTags(ByVal myLoc As Location, ByRef mySB As Text.StringBuilder)
        mySB.Replace("~~CurrentPageName~~", myLoc.LocationName)
        mySB.Replace("~~CurrentPageDescription~~", myLoc.LocationDescription)
        mySB.Replace("~~CurrentPageKeywords~~", myLoc.LocationKeywords)
        mySB.Replace("~~CurrentPageTitle~~", myLoc.LocationTitle)
        mySB.Replace("~~CurrentPageDate~~", myLoc.ModifiedDT.ToShortDateString())
        mySB.Replace("~~PageName~~", myLoc.LocationName)
        mySB.Replace("~~PageDescription~~", myLoc.LocationDescription)
        mySB.Replace("~~PageKeywords~~", myLoc.LocationKeywords)
        mySB.Replace("~~PageTitle~~", myLoc.LocationTitle)
        mySB.Replace("<PageName>", myLoc.LocationName)
        mySB.Replace("<PageDescription>", myLoc.LocationDescription)
        mySB.Replace("<PageKeywords>", myLoc.LocationKeywords)
        mySB.Replace("<PageTitle>", myLoc.LocationTitle)
    End Sub

#End Region
#Region "Public Properties to Access Active Company Profile Properties"
    Public ReadOnly Property LocationList() As LocationList
        Get
            Return CurCompany.Locations
        End Get
    End Property
    Public ReadOnly Property LocationGroupList As LocationGroupList
        Get
            Return CurCompany.LocationGroups
        End Get
    End Property
    Public ReadOnly Property LocationAliasList As LocationAliasList
        Get
            Return CurCompany.LocationAliases
        End Get
    End Property
    Public ReadOnly Property ArticleList As ArticleList
        Get
            Return CurCompany.Articles
        End Get
    End Property
    Public ReadOnly Property LinkCategoryList() As PartCategoryList
        Get
            Return CurCompany.PartCategories
        End Get
    End Property
    Public ReadOnly Property SiteImageList() As CompanyImageList
        Get
            Return CurCompany.Images
        End Get
    End Property
    Public ReadOnly Property PartList() As PartList
        Get
            Return CurCompany.Parts
        End Get
    End Property
    Public ReadOnly Property SiteParameterList() As ParameterList
        Get
            Return CurCompany.Parameters
        End Get
    End Property
    Public ReadOnly Property DefaultArticleID() As Integer
        Get
            Return CurCompany.DefaultArticleID
        End Get
    End Property
    Public ReadOnly Property SiteHomePageID() As String
        Get
            Return CurCompany.HomeLocationID
        End Get
    End Property
    Public ReadOnly Property FromEmail() As String
        Get
            Return CurCompany.FromEmail
        End Get
    End Property
    Public ReadOnly Property SitePrefix() As String
        Get
            Return CurCompany.SitePrefix
        End Get
    End Property
    Public ReadOnly Property SiteURL() As String
        Get
            Return CurCompany.CompanyURL
        End Get
    End Property
    Public ReadOnly Property UseBreadCrumbURL() As Boolean
        Get
            Return CurCompany.UseBreadCrumbURL
        End Get
    End Property
    Public ReadOnly Property CurrentLocationID() As String
        Get
            Select Case CurLocation.RecordSource
                Case "Page"
                    Return CurLocation.LocationID
                Case "Category"
                    Return CurLocation.LocationID
                Case "Article"
                    Return CurLocation.ParentLocationID
                Case "Image"
                    Return CurLocation.ParentLocationID
                Case Else
                    Return CurLocation.LocationID
            End Select
        End Get
    End Property
    Public ReadOnly Property CurrentLocationNM() As String
        Get
            Return CurLocation.LocationName
        End Get
    End Property
    Public ReadOnly Property CurrentLocationDS() As String
        Get
            Return CurLocation.LocationDescription
        End Get
    End Property
    Public ReadOnly Property CurrentDisplayURL() As String
        Get
            Return CurLocation.LocationURL
        End Get
    End Property
    Public ReadOnly Property CurrentPageFileName() As String
        Get
            Return CurLocation.LocationFileName
        End Get
    End Property
    Public ReadOnly Property SiteTitle() As String
        Get
            Return CurCompany.CompanyTitle
        End Get
    End Property
    Public ReadOnly Property SiteCategoryTypeID As String
        Get
            Return CurCompany.SiteCategoryTypeID
        End Get
    End Property
    Public ReadOnly Property SiteCategoryTypeNM As String
        Get
            Return CurCompany.SiteCategoryTypeNM
        End Get
    End Property
#End Region
End Class
