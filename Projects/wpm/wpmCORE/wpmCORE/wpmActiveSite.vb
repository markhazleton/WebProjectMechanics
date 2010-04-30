Imports System.Web
Imports System.Xml.Serialization

Public Class wpmActiveSite
#Region "properties"
    Private SiteProfile As New wpmSiteProfile
    Public Session As wpmSession
    Private CurrentLocation As New wpmLocation
    Private _UseDefaultTemplate As Boolean = False
    Public Property UseDefaultTemplate() As Boolean
        Get
            Return _UseDefaultTemplate
        End Get
        Set(ByVal value As Boolean)
            _UseDefaultTemplate = value
        End Set
    End Property
#End Region
#Region "consturctors"
    Public Sub New(ByVal thisSession As System.Web.SessionState.HttpSessionState)
        MyBase.New()
        Session = New wpmSession(thisSession)
        Session.AddHTMLHead = "RESET"
        LoadSiteProfile("ORDER", thisSession)
    End Sub
    Public Sub New(ByVal OrderBy As String, ByVal thisSession As System.Web.SessionState.HttpSessionState)
        MyBase.New()
        Session = New wpmSession(thisSession)
        Session.AddHTMLHead = "RESET"
        LoadSiteProfile(OrderBy, thisSession)
    End Sub
#End Region

#Region "publicfunctions"

#End Region

#Region "privatefunctions"
    Private Function LoadSiteProfile(ByVal OrderBy As String, ByRef thisSession As System.Web.SessionState.HttpSessionState) As Boolean

        If Session.SiteDB = "" Then
            GetConfig()
        End If
        Dim SiteMapName As String = GetSiteMapName(Session.CompanyID, Session.GroupID, OrderBy)
        If wpmUser.IsAdmin() Then
            HttpContext.Current.Application((Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", "") & " - " & Session.CompanyID & " - " & "4" & " - " & OrderBy)) = Nothing
        End If
        If App.Config.CachingEnabled AndAlso OrderBy = "ORDER" AndAlso Session.GroupID = "4" Then
            If HttpContext.Current.Application(SiteMapName) Is Nothing Then
                SiteProfile.GetSiteFileFromDB(Session.CompanyID, Session.GroupID, OrderBy)
                Try
                    HttpContext.Current.Application(SiteMapName) = SiteProfile
                Catch ex As Exception
                    wpmLog.AuditLog("Error When updating Application variable (" & SiteMapName & ") - " & ex.ToString, "wpmActiveSite.LoadSiteProfile")
                End Try
            Else
                Try
                    SiteProfile = CType(HttpContext.Current.Application(SiteMapName), wpmSiteProfile)
                Catch ex As Exception
                    wpmLog.AuditLog("Error when reading Application variable (" & SiteMapName & ") - " & ex.ToString, "wpmSiteFile.LoadSiteProfile")
                End Try
            End If
        Else
            SiteProfile.GetSiteFileFromDB(Session.CompanyID, Session.GroupID, OrderBy)
        End If
        Session.SiteGallery = SiteProfile.SiteGallery
        If Session.ContactID = "" Then
            Session.GroupID = "4"
            Session.ContactID = ""
            Session.ContactName = ""
            Session.ContactEmail = ""
            Session.ContactRoleTitle = "GUEST"
            Session.ContactRoleFilterMenu = "FALSE"
            Session.ContactRoleID = ""
            Session.RightContent = ""
            Session.SiteTemplatePrefix = SiteProfile.SitePrefix
        End If
        Session.SiteCategoryTypeID = SiteProfile.SiteCategoryTypeID
        If CheckCurrentSettings() Then
            SetCurrentPageID(Session.CurrentPageID)
            SelectCurrentPageRow((Session.CurrentPageID), (Session.CurrentArticleID))
        End If
        Return True
    End Function

#End Region


    Public Function GetSiteMapName(ByVal CompanyID As String, ByVal GroupID As String, ByVal OrderBy As String) As String
        Return (Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", "") & " - " & CompanyID & " - " & GroupID & " - " & OrderBy)
    End Function

    Private Function SetMainPage(ByVal sMainPageID As String, ByVal sMainPageName As String) As Boolean
        CurrentLocation.MainMenuPageID = sMainPageID
        CurrentLocation.MainMenuPageName = sMainPageName
        Return True
    End Function
    Public Function SetListPage(ByVal sQueryString As String, ByVal sServerName As String, ByVal sURL As String) As Boolean
        If (Left(sQueryString, 4) = "404;") Then
            Session.ListPageURL = (Right(sQueryString, Len(CStr(sQueryString)) - 4))
        Else
            Session.ListPageURL = ("http://" & sServerName & sURL & "?" & sQueryString)
        End If
        Return True
    End Function
    Public Function CheckCurrentSettings() As Boolean
        Dim sbError As New StringBuilder
        If (Session.CompanyID = "") Then
            sbError.Append("&NoCompanyID=Failed")
        End If
        If Trim(Session.SiteDB) = "" Then
            sbError.Append("&NoSiteDB=Failed")
        End If
        If sbError.Length > 0 Then
            HttpContext.Current.Response.Redirect("~" & App.Config.wpmWebHome() & "debug.aspx?Error=TRUE" & sbError.ToString)
            Return False
        Else
            Return True
        End If
    End Function
    Public Function SetCurrentPageID(ByVal CurrentPageID As String) As Boolean
        If IsNothing(CurrentPageID) Or IsDBNull(CurrentPageID) Or CurrentPageID = "" Then
            Session.CurrentPageID = SiteProfile.SiteHomePageID
            CurrentLocation.MainMenuPageID = SiteProfile.SiteHomePageID
        Else
            Session.CurrentPageID = CurrentPageID
            CurrentLocation.MainMenuPageID = SiteProfile.SiteHomePageID
        End If
        Return True
    End Function
    Public Function SelectCurrentPageRow(ByVal CurrentPageID As String, ByVal CurrentArticleID As String) As Boolean
        CurrentLocation = Me.SiteProfile.LocationList.FindLocation(CurrentPageID, CurrentArticleID)
        For Each bcRow As wpmLocationTrail In CurrentLocation.LocationTrailList
            If bcRow.MenuLevelNBR = 1 Then
                SetMainPage(bcRow.PageID, bcRow.PageName)
            End If
        Next
        Return True
    End Function
    Private Sub ReadSiteFile()
        Try
            Dim sr As New StreamReader(Session.SiteMapFilePath)
            Dim xs As New XmlSerializer(GetType(wpmSiteProfile))
            SiteProfile = DirectCast(xs.Deserialize(sr), wpmSiteProfile)
            sr.Close()
        Catch
        Finally
        End Try
    End Sub
    Public Function BuildTemplate(ByRef sbContent As StringBuilder) As Boolean
        Me.ReplaceLinkTags(sbContent)

        sbContent.Replace("~~SiteTitle~~", Me.GetSiteTitle())

        If Not (Me.SiteProfile.SiteParameterList.ReplaceSiteParameterTags(sbContent, Me.CurrentLocation)) Then
            wpmLog.AuditLog("Error With ReplaceSiteParameterTags", "wpmSiteMap.BuildTemplate(sbContent)")
        End If

        sbContent.Replace("~~PageAdmin~~", GetPageAdmin())
        sbContent.Replace("~~UserOptions~~", Me.GetUserOptions())
        sbContent.Replace("~~CurrentPageID~~", Me.CurrentLocation.PageID)
        sbContent.Replace("~~CurrentPageName~~", Me.CurrentLocation.PageName)
        sbContent.Replace("~~CurrentPageDesc~~", Me.GetPageDescription())
        sbContent.Replace("~~CurrentPageKeywords~~", Me.GetPageKeywords())
        sbContent.Replace("~~ParentPageName~~", Me.CurrentLocation.MainMenuPageName)
        sbContent.Replace("~~MainMenuName~~", Me.CurrentLocation.MainMenuPageName)
        sbContent.Replace("~~BreadCrumbs~~", Me.CurrentLocation.BreadCrumbHTML)
        sbContent.Replace("~~UserName~~", Me.Session.ContactName)
        sbContent.Replace("~~Year~~", wpmUTIL.FormatDate(Now, "Y"))
        sbContent.Replace("~~Today~~", wpmUTIL.GetCurrentDate())
        sbContent.Replace("~~UserRole~~", Me.Session.ContactRoleTitle)
        sbContent.Replace("~~UserPreferences~~", wpmUTIL.FormatLink(wpmSession.GetContactID(), "Preferences", "Preferences", "" & App.Config.wpmWebHome() & "login/contact_edit.aspx?key=" & wpmSession.GetContactID()))
        sbContent.Replace("~~PageArticles~~", SiteProfile.BuildPageArticle(Me.CurrentLocation.PageID, Me.CurrentLocation.ArticleID))
        sbContent.Replace("~~CurrentPageURL~~", Me.CurrentLocation.DisplayURL)
        ' Menu Options 
        sbContent.Replace("~~TopMenu~~", Me.SiteProfile.BuildMenuChild(Me.CurrentLocation.MainMenuPageID, "", ""))
        sbContent.Replace("~~SubTree~~", Me.SiteProfile.BuildPageTree(Me.CurrentLocation.PageID, 0, "topmenu"))
        sbContent.Replace("~~MainSubTree~~", Me.SiteProfile.BuildPageTree(Me.CurrentLocation.MainMenuPageID, 0, "topmenu"))
        sbContent.Replace("~~MainSubMenu~~", Me.SiteProfile.BuildMenuChild(Me.CurrentLocation.PageID, Me.CurrentLocation.MainMenuPageID, Me.CurrentLocation.MainMenuPageID))
        sbContent.Replace("~~2ndMenu~~", Me.SiteProfile.BuildMenuChild(Me.CurrentLocation.PageID, Me.CurrentLocation.MainMenuPageID, Me.CurrentLocation.MainMenuPageID))
        sbContent.Replace("~~3rdMenu~~", Me.SiteProfile.BuildLinkMenu(3, Me.CurrentLocation.PageID, "~~LINKS~~", Me.CurrentLocation))

        ' YUI Menu Options
        'sbContent.Replace("~~yuiTopMenu~~", Me.mySiteFile.yuiBuildMenuChild(Me.CurrentMapRow.MainMenuPageID, String.Empty, String.Empty, "yuiTopMenu"))
        'sbContent.Replace("~~yuiMainSubMenu~~", Me.mySiteFile.yuiBuildMenuChild(Me.CurrentMapRow.PageID, Me.CurrentMapRow.MainMenuPageID, Me.CurrentMapRow.MainMenuPageID, "yuiMainSubMenu"))
        sbContent.Replace("~~yuiMainTree~~", Me.SiteProfile.yuiBuildPageTree("", 0, "top"))
        sbContent.Replace("~~yuiSubMenu~~", Me.SiteProfile.yuiBuildPageList(Me.CurrentLocation.MainMenuPageID, Me.CurrentLocation.MainMenuPageName, Me.CurrentLocation.PageID, 0))
        sbContent.Replace("~~yuiChildrenMenu~~", Me.SiteProfile.yuiBuildMenuChild(Me.CurrentLocation.PageID, Me.CurrentLocation.PageID, Me.CurrentLocation.PageID, "yuiChildrenMenu"))
        sbContent.Replace("~~yuiSiblingMenu~~", Me.SiteProfile.yuiBuildMenuChild(Me.CurrentLocation.PageID, Me.CurrentLocation.ParentPageID, Me.CurrentLocation.ParentPageID, "yuiSiblingMenu"))
        ' MOO Tools Options
        sbContent.Replace("~~mooMenuTree~~", Me.SiteProfile.mooBuildPageList(String.Empty, String.Empty, Me.CurrentLocation.PageID, 0))
        ' Alternate Menu Options
        sbContent.Replace("~~ParentMenu~~", Me.SiteProfile.BuildLinkListByParent(Me.CurrentLocation.ParentPageID, Me.CurrentLocation.ParentPageID, Me.CurrentLocation.SiteCategoryID))
        sbContent.Replace("~~ChildrenMenu~~", Me.SiteProfile.BuildLinkListByParent(Me.CurrentLocation.PageID, Me.CurrentLocation.PageID, Me.CurrentLocation.SiteCategoryID))
        '<Jonathan's Changes>
        'This is a new tag I wanted to combine the breadcrumb menu with the children menu
        sbContent.Replace("~~BreadCrumbChildren~~", Me.SiteProfile.BreadCrumbWithChildren(Me.CurrentLocation.BreadCrumbHTML, Me.CurrentLocation.PageID, Me.CurrentLocation.PageID, Me.CurrentLocation.SiteCategoryID))
        '</Jonathan's Changes>
        sbContent.Replace("~~SiblingMenu~~", Me.SiteProfile.BuildLinkListBySibling(Me.CurrentLocation.PageID, Me.CurrentLocation.ParentPageID, Me.CurrentLocation.SiteCategoryID))
        ' Replace Site Cateogry Tags
        ReplaceSiteCategoryTags(sbContent)
        ' Replace Company Tags
        Me.SiteProfile.ReplaceTags(sbContent)
        Me.Session.AddPageHistory(Me.CurrentLocation.PageName)
        sbContent.Replace("~~debug~~", Me.Session.GetSessionDebug())
        Return True
    End Function
    Private Function ReplaceSiteCategoryTags(ByRef sbContent As StringBuilder) As Boolean
        For Each myGroup As wpmSiteGroup In Me.SiteProfile.SiteGroupList
            If (InStr(1, (myGroup.SiteCategoryGroupNM.ToUpper), "TOP") > 0) Then
                sbContent.Replace("~~yui" & myGroup.SiteCategoryGroupNM & "~~", Me.SiteProfile.yuiBuildSiteCategoryGroupBar(String.Empty, myGroup.SiteCategoryGroupNM, Me.CurrentLocation.PageID, 1))
                sbContent.Replace("~~" & myGroup.SiteCategoryGroupNM & "~~", Me.SiteProfile.BuildSiteCategoryGroupList(String.Empty, myGroup.SiteCategoryGroupNM, Me.CurrentLocation.PageID, 1, myGroup.SiteCategoryGroupDS))
            Else
                sbContent.Replace("~~yui" & myGroup.SiteCategoryGroupNM & "~~", Me.SiteProfile.yuiBuildSiteCategoryGroupList(String.Empty, myGroup.SiteCategoryGroupNM, Me.CurrentLocation.PageID, 1))
                sbContent.Replace("~~" & myGroup.SiteCategoryGroupNM & "~~", Me.SiteProfile.BuildSiteCategoryGroupList(String.Empty, myGroup.SiteCategoryGroupNM, Me.CurrentLocation.PageID, 1, myGroup.SiteCategoryGroupDS))
            End If
        Next
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
            If SiteProfile.PartList.Count = 0 Then
                NoLinkRows(sRightLinks, sLeftLinks, sCenterLinks)
            Else
                For Each myrow As wpmPart In SiteProfile.PartList.GetActiveParts
                    If ((myrow.SiteCategoryGroupID = Me.CurrentLocation.SiteCategoryGroupID Or _
                         myrow.SiteCategoryGroupID = String.Empty) _
                        Or (myrow.PageID = Me.CurrentLocation.PageID Or IsDBNull(myrow.PageID))) Then
                        Select Case myrow.LinkTypeCD
                            Case "MENU5"
                                sThisLink = SiteProfile.BuildLinkMenu(5, myrow.PageID, myrow.LinkURL, Me.CurrentLocation)
                            Case "MENU4"
                                sThisLink = SiteProfile.BuildLinkMenu(4, myrow.PageID, myrow.LinkURL, Me.CurrentLocation)
                            Case "MENU3"
                                sThisLink = SiteProfile.BuildLinkMenu(3, myrow.PageID, myrow.LinkURL, Me.CurrentLocation)
                            Case "MENU2"
                                sThisLink = SiteProfile.BuildLinkMenu(2, myrow.PageID, myrow.LinkURL, Me.CurrentLocation)
                            Case "MENU1"
                                sThisLink = SiteProfile.BuildLinkMenu(1, myrow.PageID, myrow.LinkURL, Me.CurrentLocation)
                            Case "SubMenu"
                                myrow.LinkURL = Replace(myrow.LinkURL, "~~MainMenuName~~", Me.CurrentLocation.MainMenuPageName)
                                sThisLink = SiteProfile.BuildLinkMenu(2, myrow.PageID, myrow.LinkURL, Me.CurrentLocation)
                            Case "FILE"
                                ' If the linkPageID is null or 0 apply it to all pages
                                ' If the linkPageID is equal to the current page, apply it
                                ' If the linkPageID is not null and not the current page, ignore it
                                If IsDBNull(myrow.PageID) Then
                                    sThisLink = "<a href=""" & myrow.LinkURL & """>" & myrow.LinkTitle & "</a><br />"
                                ElseIf (myrow.PageID = Me.CurrentLocation.PageID) Then
                                    sThisLink = "<a href=""" & myrow.LinkURL & """>" & myrow.LinkTitle & "</a><br />"
                                ElseIf (CInt(myrow.PageID) = 0) Then
                                    sThisLink = "<a href=""" & myrow.LinkURL & """>" & myrow.LinkTitle & "</a><br />"
                                End If
                            Case "XML"
                                If myrow.PageID = Me.CurrentLocation.PageID Or myrow.PageID = "" Then
                                    Dim mySB As New StringBuilder(myrow.LinkURL)
                                    If mySB.ToString.Contains("~~") Then
                                        If Not (Me.SiteProfile.SiteParameterList.ReplaceSiteParameterTags(mySB, Me.CurrentLocation)) Then
                                            wpmLog.AuditLog("Error With ReplaceSiteParameterTags", "wpmSiteMap.ReplaceLinks-XML")
                                        End If
                                        mySB.Replace("~~PageKeywords~~", Me.GetPageKeywords.Replace(" ", "+"))

                                        If Not Me.CurrentLocation.PageName Is Nothing Then
                                            mySB.Replace("~~PageName~~", Me.CurrentLocation.PageName.Replace(" ", "+"))
                                        End If

                                        Me.SiteProfile.ReplaceTags(mySB)
                                    End If
                                    Dim myXML As New wpmXML(myrow.LinkTitle, mySB.ToString, myrow.LinkDescription, Session.CompanyID, Me.SiteGallery)
                                    sThisLink = myXML.getXMLTransform()
                                End If
                            Case "RSS"
                                If myrow.PageID = Me.CurrentLocation.PageID Or myrow.PageID = "" Then
                                    Dim myRSS As New wpmRssTools.wpmRSS(myrow.LinkURL)
                                    sThisLink = myRSS.getRSSFeed(Me.SiteGallery)
                                End If
                            Case "SiteCategory"
                                Dim sSiteCategory As String = myrow.LinkURL
                                For Each myGroup As wpmSiteGroup In Me.SiteProfile.SiteGroupList
                                    If (InStr(1, (myGroup.SiteCategoryGroupNM.ToUpper), "TOP") > 0) Then
                                        sSiteCategory = Replace(sSiteCategory, "~~" & myGroup.SiteCategoryGroupNM & "~~", Me.SiteProfile.yuiBuildSiteCategoryGroupBar(myrow.PageID, myGroup.SiteCategoryGroupNM, myrow.PageID, 1))
                                    Else
                                        sSiteCategory = Replace(sSiteCategory, "~~" & myGroup.SiteCategoryGroupNM & "~~", Me.SiteProfile.yuiBuildSiteCategoryGroupList(myrow.PageID, myGroup.SiteCategoryGroupNM, myrow.PageID, 1))
                                    End If
                                Next
                                sThisLink = sSiteCategory
                            Case "KeywordSearch"
                                sThisLink = ProcessKeywordSearch()
                            Case Else
                                ' If the linkPageID is null or 0 apply it to all pages
                                ' If the linkPageID is equal to the current page, apply it
                                ' If the linkPageID is not null and not the current page, ignore it
                                If IsDBNull(myrow.PageID) Then
                                    sThisLink = myrow.LinkURL
                                ElseIf (myrow.PageID = Me.CurrentLocation.PageID) Then
                                    sThisLink = myrow.LinkURL
                                ElseIf myrow.PageID = "" Then
                                    sThisLink = myrow.LinkURL
                                End If
                        End Select
                    End If
                    If sThisLink <> "" Then
                        If wpmUser.IsAdmin() Then
                            If myrow.LinkSource = "SiteLink" Then
                                sThisLink = sThisLink & "<div class=""admin"">(<a href=""" & App.Config.AspMakerGen & "SiteLink_edit.aspx?ID=" & myrow.LinkID & """>edit " & myrow.LinkTitle & "</a>)</div>"
                            Else
                                sThisLink = sThisLink & "<div class=""admin"">(<a href=""" & App.Config.AspMakerGen & "link_edit.aspx?ID=" & myrow.LinkID & """>edit " & myrow.LinkTitle & "</a>)</div>"
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

                Next
                If wpmUser.IsAdmin() Then
                    If (sRightLinks = "") Then
                        sRightLinks = "<a href=""" & App.Config.AspMakerGen & "link_list.aspx"">NO RIGHT LINKS</a>"
                    End If
                    If (sLeftLinks = "") Then
                        sLeftLinks = "<a href=""" & App.Config.AspMakerGen & "link_list.aspx"">NO LEFT LINKS</a>"
                    End If
                    If (sCenterLinks = "") Then
                        sCenterLinks = "<a href=""" & App.Config.AspMakerGen & "link_list.aspx"">NO CENTER LINKS</a>"
                    End If
                End If
            End If
            sbContent.Replace("~~LeftColumnLinks~~", sLeftLinks)
            sbContent.Replace("~~RightColumnLinks~~", sRightLinks)
            sbContent.Replace("~~CenterColumnLinks~~", sCenterLinks)
        End If
        Return True
    End Function
    Public Function GetUserOptions() As String
        Dim sReturn As String
        sReturn = ""
        If wpmSession.GetContactID() <> "" Then
            sReturn = "<a href=""" & App.Config.wpmWebHome() & "login/logout.aspx"" >Sign Out</a>"
        Else
            sReturn = "<a href=""" & App.Config.wpmWebHome() & "login/login.aspx"">Sign On</a>"
        End If
        Return sReturn
    End Function
    Private Function ProcessKeywordSearch() As String
        Dim myReturn As New StringBuilder(String.Empty)
        For Each myString As wpmKeyword In Me.LocationList.KeywordList
            myReturn.Append(myString.Code & ",")
        Next
        Dim myContents As New StringBuilder
        wpmFileIO.ReadTextFile(HttpContext.Current.Server.MapPath("/wpm/search/searchfield.js"), myContents)
        myContents.Replace("<keywordlist>", myReturn.ToString)

        Me.Session.AddHTMLHead = ("<script type=""text/javascript"">" & myContents.ToString & "</script>")
        Me.Session.AddHTMLHead = ("<link rel=""stylesheet"" type=""text/css"" href=""/wpm/search/searchfield.css"">")

        myReturn.Length = 0
        wpmFileIO.ReadTextFile(HttpContext.Current.Server.MapPath("/wpm/search/searchfield.html"), myReturn)

        Dim mySearchKeyword As String = GetProperty("searchfield", String.Empty)

        If mySearchKeyword <> String.Empty Then
            wpmLog.SearchLog(mySearchKeyword, "KeywordSearch")
            myReturn.Replace("~~SearchReturn~~", Me.LocationList.FindLocationsByKeyword(mySearchKeyword))
        Else
            myReturn.Replace("~~SearchReturn~~", String.Empty)
        End If
        Return myReturn.ToString
    End Function


    Private Function GetSiteTitle() As String
        Dim mySiteTitle As New StringBuilder
        If Me.CurrentLocation.PageTitle = "" Then
            mySiteTitle.Append(Me.SiteProfile.SiteTitle)
        Else
            mySiteTitle.Append(Me.CurrentLocation.PageTitle)
        End If
        mySiteTitle.Replace("<PageName>", Me.CurrentLocation.PageName)
        mySiteTitle.Replace("<PageTitle>", Me.CurrentLocation.PageTitle)
        mySiteTitle.Replace("<PageDescription>", Me.CurrentLocation.PageDescription)
        mySiteTitle.Replace("<PageKeywords>", Me.CurrentLocation.PageKeywords)
        Return mySiteTitle.ToString
    End Function
    Private Function GetPageKeywords() As String
        Dim mySiteKeywords As New StringBuilder
        If Me.CurrentLocation.PageKeywords = "" Then
            mySiteKeywords.Append(Me.SiteProfile.SiteKeywords)
        Else
            mySiteKeywords.Append(Me.CurrentLocation.PageKeywords)
        End If
        mySiteKeywords.Replace("<PageName>", Me.CurrentLocation.PageName)
        mySiteKeywords.Replace("<PageTitle>", Me.CurrentLocation.PageTitle)
        mySiteKeywords.Replace("<PageDescription>", Me.CurrentLocation.PageDescription)
        mySiteKeywords.Replace("<PageKeywords>", Me.CurrentLocation.PageKeywords)
        mySiteKeywords.Replace("~~SiteCity~~", Me.SiteProfile.SiteCity)
        mySiteKeywords.Replace("~~SiteState~~", Me.SiteProfile.SiteState)
        Return mySiteKeywords.ToString
    End Function
    Private Function GetPageDescription() As String
        Dim mySiteDescription As New StringBuilder
        If Me.CurrentLocation.PageKeywords = "" Then
            mySiteDescription.Append(Me.SiteProfile.SiteDescription)
        Else
            mySiteDescription.Append(Me.CurrentLocation.PageDescription)
        End If
        mySiteDescription.Replace("<PageName>", Me.CurrentLocation.PageName)
        mySiteDescription.Replace("<PageTitle>", Me.CurrentLocation.PageTitle)
        mySiteDescription.Replace("<PageDescription>", Me.CurrentLocation.PageDescription)
        mySiteDescription.Replace("<PageKeywords>", Me.CurrentLocation.PageKeywords)
        mySiteDescription.Replace("~~SiteCity~~", Me.SiteProfile.SiteCity)
        mySiteDescription.Replace("~~SiteState~~", Me.SiteProfile.SiteState)
        Return mySiteDescription.ToString
    End Function
    Private Function NoLinkRows(ByRef sRightLinks As String, ByRef sLeftLinks As String, ByRef sCenterLinks As String) As Boolean
        If wpmUser.IsAdmin() Then
            sRightLinks = "<a href=""" & App.Config.AspMakerGen & "link_list.aspx"">NO RIGHT LINKS</a>"
            sLeftLinks = "<a href=""" & App.Config.AspMakerGen & "link_list.aspx"">NO LEFT LINKS</a>"
            sCenterLinks = "<a href=""" & App.Config.AspMakerGen & "link_list.aspx"">NO CENTER LINKS</a>"
        Else
            sRightLinks = ""
            sLeftLinks = ""
            sCenterLinks = ""
        End If
    End Function

    Private Function GetPageAdmin() As String
        Dim sReturn As String = ("")
        If wpmUser.IsAdmin() Then
            Select Case Me.CurrentLocation.RecordSource
                Case "Page"
                    sReturn = ("<div class=""wpmADMIN""><a href=""" & App.Config.AspMakerGen & "zpage_edit.aspx?zPageID=" & Me.CurrentLocation.PageID & """>Page Properties</a> | <a href=""" & App.Config.AspMakerGen & "zpage_add.aspx?zPageID=" & Me.CurrentLocation.PageID & """>Add Page</a> | <a href=""" & App.Config.AspMakerGen & "article_add.aspx"">Add Article</a> | <a href=""" & App.Config.wpmWebHome() & "admin/AdminLink.aspx"">All Parts</a> | <a href=""" & App.Config.wpmWebHome() & "admin/default.aspx"">Admin Home</a> </div>")
                Case "Article"
                    sReturn = ("<div class=""wpmADMIN""><a href=""" & App.Config.AspMakerGen & "zpage_edit.aspx?zPageID=" & Me.CurrentLocation.PageID & """>Page Properties</a> | <a href=""" & App.Config.AspMakerGen & "zpage_add.aspx?zPageID=" & Me.CurrentLocation.PageID & """>Add Page</a> | <a href=""" & App.Config.AspMakerGen & "article_add.aspx"">Add Article</a> | <a href=""" & App.Config.wpmWebHome() & "admin/AdminLink.aspx"">All Parts</a> | <a href=""" & App.Config.wpmWebHome() & "admin/default.aspx"">Admin Home</a> </div>")
                Case "Category"
                    sReturn = ("<div class=""wpmADMIN""><a href=""" & App.Config.AspMakerGen & "SiteCategory_edit.aspx?SiteCategoryID=" & Replace(Me.CurrentLocation.PageID, "CAT-", "") & """>Page Properties</a> | <a href=""" & App.Config.wpmWebHome() & "admin/AdminLink.aspx"">All Parts</a> | <a href=""" & App.Config.wpmWebHome() & "admin/default.aspx"">Admin Home</a> </div>")
                Case Else
                    sReturn = ("<div class=""wpmADMIN""><a href=""" & App.Config.AspMakerGen & "zpage_add.aspx?zPageID=" & Me.CurrentLocation.PageID & """>Add Page</a> | <a href=""" & App.Config.AspMakerGen & "article_add.aspx"">Add Article</a> | <a href=""" & App.Config.wpmWebHome() & "admin/AdminLink.aspx"">All Parts</a> | <a href=""" & App.Config.wpmWebHome() & "admin/default.aspx"">Admin Home</a> </div>")
            End Select
        End If
        Return sReturn
    End Function

    Public Function WriteCurrentLocation() As Boolean
        Dim response As System.Web.HttpResponse = HttpContext.Current.Response
        Session.CurrentPageID = CurrentLocation.PageID
        Session.CurrentArticleID = CurrentLocation.ArticleID
        Select Case (CurrentLocation.PageTypeCD.ToUpper)
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
            Case "LINK"
                response.Write(GetLinkDirectoryHTML())
            Case "SITEMAP"
                CurrentLocation.PageName = SiteProfile.CompanyName & " Sitemap"
                CurrentLocation.PageTitle = SiteProfile.CompanyName & " Sitemap"
                response.Write(GetHTML(SiteProfile.SiteDescription & SiteProfile.TreeHTML.ToString, False, Session.SiteTemplatePrefix))
            Case "SITEMAP.XML"
                response.ContentType = "text/xml"
                Dim gen As wpmXmlSiteMap = New wpmXmlSiteMap(response.Output)
                gen.WriteSitemapDocument()
                gen.Close()
            Case Else
                If CurrentLocation.TransferURL Is Nothing Then
                    wpmLog.AuditLog("Current Location Empty", "Redirect to ADMIN")
                    If Me.DefaultArticleID = String.Empty And Me.SiteHomePageID = String.Empty And SiteProfile.SiteCategoryTypeID = String.Empty Then
                        If HttpContext.Current.Request.RawUrl <> Me.SiteProfile.SiteURL Then
                            If Me.SiteProfile.SiteURL Is Nothing Then
                                wpmLog.AuditLog("SiteURL is nothing", "Redirect to debug")
                                response.Redirect("~/wpm/debug.aspx", True)
                            Else
                                If App.Config.FullLoggingOn Then
                                    wpmLog.AuditLog("301-Redirect", Me.SiteProfile.SiteURL)
                                End If
                                wpmUTIL.Build301Redirect(Me.SiteProfile.SiteURL)
                            End If
                        Else
                            wpmLog.AuditLog("Missing Key Site Components", "Redirect to debug")
                            response.Redirect("~/wpm/debug.aspx", True)
                        End If
                    Else
                        If Me.SiteHomePageID = String.Empty Then
                            wpmLog.AuditLog("SiteHomePageID Missing", "Redirect to debug")
                            response.Redirect("~/wpm/debug.aspx", True)
                        Else
                            SetCurrentPageID(Me.SiteHomePageID)
                            SelectCurrentPageRow(Me.SiteHomePageID, String.Empty)
                            response.Write(GetArticlePageHTML())
                        End If
                    End If
                Else
                    If Left(CurrentLocation.TransferURL, 4) = "http" Then
                        HttpContext.Current.Response.Redirect(CurrentLocation.TransferURL, True)
                    Else
                        HttpContext.Current.Server.Transfer(CurrentLocation.TransferURL, True)
                    End If
                End If
        End Select
    End Function


    Public Function GetHTML(ByVal sMainContent As String, ByVal bUseDefault As Boolean, ByVal TemplatePrefix As String) As String
        If Me.CurrentLocation.PageID = "" And Me.CurrentLocation.ArticleID = "" And Trim(TemplatePrefix) = "" Then
            bUseDefault = True
        End If
        If UseDefaultTemplate Then
            bUseDefault = True
        End If
        Dim mySiteTheme As New wpmSiteTheme(Me, bUseDefault, TemplatePrefix)
        Dim myHTML As New StringBuilder(mySiteTheme.sbSiteTemplate.ToString)
        If myHTML.Length > 0 Then
            If (InStr(1, myHTML.ToString, "~~MainContent~~") > 0) Then
                myHTML.Replace("~~MainContent~~", sMainContent)
            End If
            myHTML.Replace("~~RightContent~~", Me.Session.RightContent)
            Me.BuildTemplate(myHTML)
            myHTML.Replace("</head>", Me.Session.AddHTMLHead & "</head>")
            ResetSessionVariables()
        End If
        Me.SetListPage(HttpContext.Current.Request.ServerVariables.Item("QUERY_STRING"), HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), HttpContext.Current.Request.ServerVariables.Item("URL"))
        If App.Config.FullLoggingOn() Then
            wpmLog.AccessLog(Me.CurrentLocation.DisplayURL, Me.CurrentLocation.TransferURL)
            wpmBuildHTML.SaveHTML(Me.CurrentLocation.DisplayURL, myHTML.ToString)
        End If
        Return myHTML.ToString
    End Function


    Public Function GetBlogPageHTML() As String
        Dim sbBlogTemplate As New StringBuilder
        If (Me.CurrentArticleID = String.Empty) Then
            If Not wpmFileIO.ReadTextFile(HttpContext.Current.Server.MapPath(Me.SiteProfile.SiteGallery & "/BlogPostsTemplate.txt"), sbBlogTemplate) Then
                wpmFileIO.ReadTextFile(HttpContext.Current.Server.MapPath("/wpm/blog/BlogPostsTemplate.txt"), sbBlogTemplate)
            End If
        Else
            If Not wpmFileIO.ReadTextFile(HttpContext.Current.Server.MapPath(Me.SiteProfile.SiteGallery & "/BlogPostTemplate.txt"), sbBlogTemplate) Then
                wpmFileIO.ReadTextFile(HttpContext.Current.Server.MapPath("/wpm/blog/BlogPostTemplate.txt"), sbBlogTemplate)
            End If
        End If

        Me.Session.AddHTMLHead = vbCrLf & _
          "<link rel=""alternate"" type=""application/rss+xml"" title=""RSS 2.0"" href=""http://" & _
          HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME") & _
          "/wpm/blog/rss_blog.aspx?c=" & Me.Session.CurrentPageID & """ >" & vbCrLf
        Dim myBlog As New wpmArticleList(Me)
        Return GetHTML(myBlog.BuildBlogList(sbBlogTemplate, wpmUTIL.GetDBInteger(GetProperty("Page", "0"))), False, Me.Session.SiteTemplatePrefix)
    End Function

    Public Function GetArticlePageHTML() As String
        Dim myArticle As New wpmArticle(Me.CurrentLocation, Me.SiteProfile.DefaultArticleID)
        Return GetHTML(myArticle.ArticleBody, False, Me.Session.SiteTemplatePrefix)
    End Function
    Public Function GetFormPageHTML() As String
        Dim mycontents As New StringBuilder
        wpmFileIO.ReadTextFile(HttpContext.Current.Server.MapPath("/wpm/form/AddToHTMLHead.txt"), mycontents)
        Me.Session.AddHTMLHead = mycontents.ToString
        Dim myArticle As New wpmArticle(Me.CurrentLocation.ArticleID, Me.CurrentLocation.PageID, Me.SiteProfile.DefaultArticleID)
        If InStr(myArticle.ArticleBody, "<FORM", CompareMethod.Text) > 0 Or InStr(myArticle.ArticleBody, "<form", CompareMethod.Text) > 0 Then
            Return GetHTML(vbCrLf & myArticle.ArticleBody & vbCrLf, False, Me.Session.SiteTemplatePrefix)
        Else
            Return GetHTML("<form action=""/wpm/wpmForm.aspx"" method=""post"">" & vbCrLf & myArticle.ArticleBody & vbCrLf & "</form>", False, Me.Session.SiteTemplatePrefix)
        End If
    End Function
    Public Function GetCatalogPageHTML() As String
        Dim myImageList As New wpmPageImageList(Me)
        Return GetHTML(myImageList.ProcessPageRequest(HttpContext.Current.Request.Item("Page"), Me.CurrentLocation.ArticleID), False, "")
    End Function

    Public Function GetSiteLocationAdmin() As String
        HttpContext.Current.Session("ListPageURL") = HttpContext.Current.Request.FilePath()
        Me.LocationList.Sort()

        Return ""
    End Function

    Public Function GetSitePartAdmin() As String
        Dim myLinkDirectory As New wpmLinkDirectory(Me)
        HttpContext.Current.Session("ListPageURL") = HttpContext.Current.Request.FilePath()
        myLinkDirectory.CreateAdminLinkDirectory(Me)
        Return myLinkDirectory.MyStringBuilder.ToString
    End Function
    Public Function GetLinkDirectoryHTML() As String
        Dim myLinkDirectory As New wpmLinkDirectory(Me)
        myLinkDirectory.CreateLinkDirectory(Me)
        Return GetHTML(myLinkDirectory.MyStringBuilder.ToString, False, Me.Session.SiteTemplatePrefix)
    End Function
    Public Function GetLinkPageHTML() As String
        Dim myLinkDirectory As New wpmLinkDirectory(Me)
        myLinkDirectory.DrawYUILinks(Me)
        Return GetHTML(myLinkDirectory.MyStringBuilder.ToString, False, Me.Session.SiteTemplatePrefix)
    End Function

    Public Sub ResetSessionVariables()
        Me.Session.CurrentArticleID = ""
        Me.Session.CurrentPageID = ""
        Me.Session.RightContent = ""
        Me.Session.AddHTMLHead = "RESET"
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
        Dim SiteConfig As wpmSiteSettings = wpmSiteSettings.Load(wpmConfig.wpmConfigFile)
        Session.CompanyID = (SiteConfig.wpmSite.CompanyID.ToString)
        Session.SiteDB = (SiteConfig.wpmSite.SQLDBConnString.ToString)
        Return True
    End Function
    Private Sub ResetCurrentRow()
        Me.CurrentLocation.PageTypeCD = "404"
        Me.CurrentLocation.PageID = ""
        Me.CurrentLocation.ArticleID = ""
        Me.CurrentLocation.TransferURL = ""
        Me.CurrentLocation.MainMenuPageID = ""
        Me.CurrentLocation.PageName = "404 - File Not Found"
        Me.CurrentLocation.PageTitle = "404 - File Not Found"
    End Sub
    Public Function Process404(ByVal RawURL As String, ByVal QueryString As String) As Boolean
        Dim bReturn As Boolean = True
        Dim sTransferURL As String = String.Empty
        Dim sRedirectURL As String = String.Empty
        Dim response As System.Web.HttpResponse = HttpContext.Current.Response

        ResetCurrentRow()

        QueryString = Replace(QueryString, ":80", String.Empty)
        sTransferURL = GetTransferURL(QueryString)
        If sTransferURL = String.Empty Then
            sRedirectURL = GetRedirectURL(QueryString)
            If (sRedirectURL = String.Empty) Then
                sRedirectURL = SiteProfile.PageAliasList.LookupTargetURL(RawURL)
            End If

            If (sRedirectURL = String.Empty) Then
                If (Not QueryString Is Nothing) AndAlso ((Right(QueryString.ToUpper, 4)) = "HTML" Or _
                    (Right(QueryString.ToUpper, 4)) = ".HTM") Then
                    If (CStr(sTransferURL) = "") Then
                        ' Build a 404 Page
                        bReturn = False
                    Else
                        TransferToURL(sTransferURL)
                    End If
                Else
                    If (Right(QueryString, 11) = "sitemap.xml") Then
                        Me.CurrentLocation.PageTypeCD = "sitemap.xml"
                        sTransferURL = "" & App.Config.wpmWebHome() & "sitemap.aspx"
                    End If
                    If (Right(QueryString, 12) = "sitemap_view") Then
                        Me.CurrentLocation.PageTypeCD = "sitemap_view"
                        sTransferURL = "" & App.Config.wpmWebHome() & "sitemap_view.aspx"
                    End If
                    If (Right(QueryString, 7) = "sitemap") Then
                        Me.CurrentLocation.PageTypeCD = "sitemap"
                        sTransferURL = "" & App.Config.wpmWebHome() & "site.aspx"
                    End If
                    If (Right(QueryString, 12) = "rss_menu.xml") Then
                        Me.CurrentLocation.PageTypeCD = "rss_menu.xml"
                        sTransferURL = "" & App.Config.wpmWebHome() & "rss_menu.aspx"
                    End If
                    If sTransferURL = "" Then
                        ' Log the error and return FALSE
                        Me.CurrentLocation.PageTypeCD = "404"
                        bReturn = False
                    Else
                        TransferToURL(sTransferURL)
                    End If
                End If
            Else
                wpmUTIL.Build301Redirect(sRedirectURL)
                bReturn = False
            End If
        Else
            TransferToURL(sTransferURL)
        End If

        If bReturn Then
            WriteCurrentLocation()
        Else
            If response.Status = "301 Moved Permanently" Then
                ' do nothing
            Else
                response.Status = "404 Not Found"
                response.StatusCode = 404
                Dim myContents As New StringBuilder
                Dim sFileName As New StringBuilder
                If Not (HttpContext.Current.Request.QueryString.Count = 0) Then
                    sFileName.Append(Replace(HttpContext.Current.Request.QueryString.Item(0), "404;", ""))
                Else
                    sFileName.Append("?")
                End If
                wpmFileIO.ReadTextFile(HttpContext.Current.Server.MapPath("/wpm/404-Text.html"), myContents)
                myContents.Replace("<RequestURL>", sFileName.ToString)
                myContents.Replace("<UserHostAddress>", HttpContext.Current.Request.UserHostAddress)
                myContents.Replace("<UserLanguages>", HttpContext.Current.Request.UserAgent)
                myContents.Replace("<RequestBrowser>", HttpContext.Current.Request.Browser.Browser)
                Session.AddHTMLHead = myContents.ToString
                response.Write(GetHTML("<blockquote>The page you were looking for can not be found</blockquote><br/><strong>" & sFileName.ToString & "</strong><br/><br/><form><div align=""center""><textarea rows=8 cols=60 wrap=soft></textarea></div></form><br/><br/><hr><sitemap>", False, Session.SiteTemplatePrefix))
            End If
        End If

        Return bReturn
    End Function
    Private Sub TransferToURL(ByVal sTransferURL As String)
        sTransferURL = Replace("~/" & sTransferURL, "//", "/")
        If CurrentLocation.TransferURL <> sTransferURL Then
            CurrentLocation.TransferURL = sTransferURL
        End If
    End Sub
    Private Function BuildErrorMessage() As String
        Dim item As Object
        Dim strReturn As String = String.Empty
        strReturn = Replace(HttpContext.Current.Request.ServerVariables.Item("QUERY_STRING"), "404;", String.Empty) & "<br/><br/>Sorry, the file you were looking for can not be found. It may have moved to a new location.<br /><br />You can go to our <A href=""/"">homepage</A> or look in our sitemap:<br /><br />"
        If wpmUser.IsAdmin() Then
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
        Dim realpage As String = String.Empty
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
        Dim sPath As String = String.Empty
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
        Dim strpage As String = String.Empty
        Dim indexc As Integer = 0
        Dim realpage As String = String.Empty
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
        For Each myrow As wpmLocation In SiteProfile.LocationList
            If myrow.PageID = sPageID And myrow.ArticleID = sArticleID And myrow.RecordSource = sRecordSource Then
                Me.CurrentLocation.UpdatePageRow(myrow)
                For Each bcRow As wpmLocationTrail In CurrentLocation.LocationTrailList
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
        Dim LinkURL As String = String.Empty
        Dim bMatch As Boolean = False
        Dim pageURL As String = String.Empty
        Dim indexc As Integer = 0
        For Each myrow As wpmLocation In SiteProfile.LocationList
            bMatch = False
            If SiteProfile.UseBreadCrumbURL Then
                pageURL = myrow.BreadCrumbURL
            Else
                pageURL = myrow.DisplayURL
            End If

            If (bStrict) Then
                If ("/" & urlName = pageURL) Then
                    bMatch = True
                End If
            Else
                If Not SiteProfile.UseBreadCrumbURL Then
                    indexc = InStrRev(urlName, "/")
                    If (indexc > 0) Then
                        urlName = Right(urlName, Len(urlName) - indexc)
                        urlName = Left(urlName, indexc - 1)
                    End If
                End If
                bMatch = wpmUTIL.CheckForMatch(urlName, myrow.DisplayURL)
            End If
            If (bMatch) Then
                If (myrow.ActiveFL Or wpmUser.IsAdmin()) Then
                    If (bStrict) Then
                        LinkURL = myrow.TransferURL
                        Me.CurrentLocation.UpdatePageRow(myrow)

                        For Each bcRow As wpmLocationTrail In CurrentLocation.LocationTrailList
                            If bcRow.MenuLevelNBR = 1 Then
                                SetMainPage(bcRow.PageID, bcRow.PageName)
                            End If
                        Next
                    Else
                        LinkURL = pageURL
                    End If
                Else
                    If (bStrict) Then
                        LinkURL = String.Empty
                    Else
                        LinkURL = pageURL
                    End If
                End If
                Exit For
            End If
        Next
        Return LinkURL
    End Function
    '
    ' Public Function to call private function in SiteProfile
    '

    Public Function ReplaceTags(ByVal sValue As String) As String
        If sValue <> String.Empty And sValue.Contains("~~") Then
            Dim mySB As New StringBuilder(sValue)
            SiteProfile.ReplaceTags(mySB)
            sValue = mySB.ToString
        End If
        Return sValue
    End Function
#Region "Public Properties to Access Active Site Properties"
    '
    ' Public Properties to Access Active Site Properties
    '
    Public ReadOnly Property LocationList() As wpmLocationList
        Get
            Return Me.SiteProfile.LocationList
        End Get
    End Property
    Public ReadOnly Property LinkCategoryList() As wpmPartGroupList
        Get
            Return Me.SiteProfile.LinkCategoryList
        End Get
    End Property
    Public ReadOnly Property SiteImageList() As wpmSiteImageList
        Get
            Return Me.SiteProfile.SiteImageList
        End Get
    End Property
    Public ReadOnly Property PartList() As wpmPartList
        Get
            Return Me.SiteProfile.PartList
        End Get
    End Property
    Public ReadOnly Property SiteParameterList() As wpmSiteParameterList
        Get
            Return Me.SiteProfile.SiteParameterList
        End Get
    End Property
    Public ReadOnly Property CompanyID() As String
        Get
            Return Me.Session.CompanyID
        End Get
    End Property
    Public ReadOnly Property CompanyName() As String
        Get
            Return Me.SiteProfile.CompanyName
        End Get
    End Property
    Public ReadOnly Property DefaultArticleID() As String
        Get
            Return Me.SiteProfile.DefaultArticleID
        End Get
    End Property
    Public ReadOnly Property SiteHomePageID() As String
        Get
            Return Me.SiteProfile.SiteHomePageID
        End Get
    End Property
    Public ReadOnly Property FromEmail() As String
        Get
            Return Me.SiteProfile.FromEmail
        End Get
    End Property
    Public ReadOnly Property GroupID() As String
        Get
            Return Me.Session.GroupID
        End Get
    End Property
    Public ReadOnly Property ContactID() As String
        Get
            Return Me.Session.ContactID
        End Get
    End Property
    Public ReadOnly Property ContactName() As String
        Get
            Return Me.Session.ContactName
        End Get
    End Property
    Public ReadOnly Property ContactEmail() As String
        Get
            Return Me.Session.ContactEmail
        End Get
    End Property
    Public ReadOnly Property SiteGallery() As String
        Get
            Return Me.Session.SiteGallery
        End Get
    End Property
    Public ReadOnly Property SiteTemplatePrefix() As String
        Get
            Return Me.Session.SiteTemplatePrefix
        End Get
    End Property
    Public ReadOnly Property SitePrefix() As String
        Get
            Return Me.SiteProfile.SitePrefix
        End Get
    End Property
    Public ReadOnly Property DefaultSitePrefix() As String
        Get
            Return Me.SiteProfile.DefaultSitePrefix
        End Get
    End Property
    Public ReadOnly Property SiteURL() As String
        Get
            Return Me.SiteProfile.SiteURL
        End Get
    End Property
    Public ReadOnly Property UseBreadCrumbURL() As Boolean
        Get
            Return Me.SiteProfile.UseBreadCrumbURL
        End Get
    End Property
    Public ReadOnly Property sitedb() As String
        Get
            Return Me.Session.SiteDB
        End Get
    End Property
    Public ReadOnly Property ListPageURL() As String
        Get
            Return Session.ListPageURL
        End Get
    End Property
    Public ReadOnly Property CurrentPageID() As String
        Get
            Return Me.CurrentLocation.PageID
        End Get
    End Property
    Public ReadOnly Property CurrentArticleID() As String
        Get
            Return Me.CurrentLocation.ArticleID
        End Get
    End Property
    Public ReadOnly Property GetCurrentPageName() As String
        Get
            Return Me.CurrentLocation.PageName
        End Get
    End Property
    Public ReadOnly Property CurrentPageDescription() As String
        Get
            Return Me.CurrentLocation.PageDescription
        End Get
    End Property
    Public ReadOnly Property CurrentPageTypeCD() As String
        Get
            Return Me.CurrentLocation.PageTypeCD
        End Get
    End Property
    Public ReadOnly Property CurrentDisplayURL() As String
        Get
            Return Me.CurrentLocation.DisplayURL
        End Get
    End Property
    Public ReadOnly Property CurrentPageFileName() As String
        Get
            Return Me.CurrentLocation.PageFileName
        End Get
    End Property
    Public ReadOnly Property SiteTitle() As String
        Get
            Return Me.SiteProfile.SiteTitle
        End Get
    End Property
#End Region
#Region "Updateable Properties"
    '
    '  Updateable Properties
    '
    Public Property RightContent() As String
        Get
            Return Me.Session.RightContent
        End Get
        Set(ByVal value As String)
            Me.Session.RightContent = value
        End Set
    End Property
    Public Property AddHTMLHead() As String
        Get
            Return Me.Session.AddHTMLHead
        End Get
        Set(ByVal value As String)
            Me.Session.AddHTMLHead = value
        End Set
    End Property
#End Region
End Class
