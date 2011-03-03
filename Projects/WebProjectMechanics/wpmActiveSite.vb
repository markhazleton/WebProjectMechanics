Imports System.Web
Imports System.Xml.Serialization
Imports System.Text

Public Class wpmActiveSite
#Region "properties"
    Private SiteProfile As New WebsiteProfile
    Public Session As wpmSession
    Private CurrentLocation As New wpmLocation
    Public Property UseDefaultTemplate() As Boolean
#End Region
#Region "consturctors"
    Public Sub New(ByVal thisSession As System.Web.SessionState.HttpSessionState)
        MyBase.New()
        Session = New wpmSession(thisSession)
        Session.AddHTMLHead = "RESET"
        LoadSiteProfile("ORDER")
    End Sub
    Public Sub New(ByVal OrderBy As String, ByVal thisSession As System.Web.SessionState.HttpSessionState)
        MyBase.New()
        Session = New wpmSession(thisSession)
        Session.AddHTMLHead = "RESET"
        LoadSiteProfile(OrderBy)
    End Sub
#End Region

#Region "publicfunctions"

#End Region

#Region "privatefunctions"
    Private Function LoadSiteProfile(ByVal OrderBy As String) As Boolean
        If Session.CompanyID = "" Then
            GetConfig()
        End If
        Dim SiteMapName As String = GetSiteMapName(Session.CompanyID, Session.GroupID, OrderBy)
        If wpmUser.IsAdmin() Then
            HttpContext.Current.Application((String.Format("{0} - {1} - 4 - {2}", _
                                                 Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", ""), _
                                                 Session.CompanyID, _
                                                 OrderBy))) = Nothing
        End If

        If wpmApp.Config.CachingEnabled AndAlso OrderBy = "ORDER" AndAlso Session.GroupID = "4" Then
            If HttpContext.Current.Application(SiteMapName) Is Nothing Then
                SiteProfile.GetSiteFileFromDB(Session.CompanyID, Session.GroupID, OrderBy)
                Try
                    HttpContext.Current.Application(SiteMapName) = SiteProfile
                Catch ex As Exception
                    wpmLogging.AuditLog(String.Format("Error When updating Application variable ({0}) - {1}", SiteMapName, ex), "wpmActiveSite.LoadSiteProfile")
                End Try
            Else
                Try
                    SiteProfile = CType(HttpContext.Current.Application(SiteMapName), WebsiteProfile)
                    If SiteProfile.CompanyName Is Nothing Then
                        SiteProfile.GetSiteFileFromDB(Session.CompanyID, Session.GroupID, OrderBy)
                    End If
                Catch ex As Exception
                    wpmLogging.AuditLog(String.Format("Error when reading Application variable ({0}) - {1}", SiteMapName, ex), "wpmSiteFile.LoadSiteProfile")
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


    Public Shared Function GetSiteMapName(ByVal CompanyID As String, ByVal GroupID As String, ByVal OrderBy As String) As String
        Return (String.Format("{0} - {1} - {2} - {3}", Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", ""), CompanyID, GroupID, OrderBy))
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
            Session.ListPageURL = (String.Format("http://{0}{1}?{2}", sServerName, sURL, sQueryString))
        End If
        Return True
    End Function
    Public Function CheckCurrentSettings() As Boolean
        Dim sbError As New StringBuilder
        If Session.CompanyID = String.Empty Then
            sbError.Append("&NoCompanyID=Failed")
        End If
        If wpmApp.ConnStr = String.Empty Then
            sbError.Append("&NoSiteDB=Failed")
        End If
        If sbError.Length > 0 Then
            HttpContext.Current.Response.Redirect(String.Format("~{0}debug.aspx?Error=TRUE{1}", wpmApp.Config.wpmWebHome(), sbError))
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
        CurrentLocation = SiteProfile.LocationList.FindLocation(CurrentPageID, CurrentArticleID)
        For Each bcRow As wpmLocationTrail In CurrentLocation.LocationTrailList
            If bcRow.MenuLevelNBR = 1 Then
                SetMainPage(bcRow.PageID, bcRow.PageName)
            End If
        Next
        Return True
    End Function

    'Private Sub ReadSiteFile()
    '    ' Create an instance of the XmlSerializer class;
    '    ' specify the type of object to be deserialized.
    '    Dim serializer As New XmlSerializer(GetType(WebsiteProfile))
    '    ' If the XML document has been altered with unknown
    '    ' nodes or attributes, handle them with the
    '    ' UnknownNode and UnknownAttribute events.
    '    AddHandler serializer.UnknownNode, AddressOf serializer_UnknownNode
    '    AddHandler serializer.UnknownAttribute, AddressOf serializer_UnknownAttribute
    '    ' A FileStream is needed to read the XML document.
    '    Using fs As New FileStream(Session.SiteMapFilePath, FileMode.Open)
    '        ' Use the Deserialize method to restore the object's state with
    '        ' data from the XML document. 
    '        SiteProfile = CType(serializer.Deserialize(fs), WebsiteProfile)
    '    End Using
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Shared Sub serializer_UnknownNode(ByVal sender As Object, ByVal e As XmlNodeEventArgs)
        WebProjectMechanics.wpmLogging.ErrorLog((String.Format("Unknown Node:{0}{1}{2}", e.Name, ControlChars.Tab, e.Text)), "serializer_UnknownNode")
    End Sub 'serializer_UnknownNode


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Shared Sub serializer_UnknownAttribute(ByVal sender As Object, ByVal e As XmlAttributeEventArgs)
        Dim attr As System.Xml.XmlAttribute = e.Attr
        WebProjectMechanics.wpmLogging.ErrorLog((String.Format("Unknown attribute {0}='{1}'", attr.Name, attr.Value)), "serializer_UnknownAttribute")
    End Sub 'serializer_UnknownAttribute
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sbContent"></param>
    ''' <returns></returns>
    Public Function BuildTemplate(ByRef sbContent As StringBuilder) As Boolean
        ReplaceLinkTags(sbContent)

        sbContent.Replace("~~SiteTitle~~", GetSiteTitle())

        If Not (SiteProfile.SiteParameterList.ReplaceSiteParameterTags(sbContent, CurrentLocation)) Then
            wpmLogging.AuditLog("Error With ReplaceSiteParameterTags", "wpmSiteMap.BuildTemplate(sbContent)")
        End If

        sbContent.Replace("~~PageAdmin~~", GetPageAdmin())
        sbContent.Replace("~~UserOptions~~", wpmActiveSite.GetUserOptions())
        sbContent.Replace("~~CurrentPageID~~", CurrentLocation.PageID)
        sbContent.Replace("~~CurrentPageName~~", CurrentLocation.PageName)
        sbContent.Replace("~~CurrentPageDesc~~", GetPageDescription())
        sbContent.Replace("~~CurrentPageKeywords~~", GetPageKeywords())
        sbContent.Replace("~~ParentPageName~~", CurrentLocation.MainMenuPageName)
        sbContent.Replace("~~MainMenuName~~", CurrentLocation.MainMenuPageName)
        sbContent.Replace("~~BreadCrumbs~~", CurrentLocation.BreadCrumbHTML)
        sbContent.Replace("~~UserName~~", Session.ContactName)
        sbContent.Replace("~~Year~~", wpmUtil.FormatDate(Now, "Y"))
        sbContent.Replace("~~Today~~", wpmUtil.GetCurrentDate())
        sbContent.Replace("~~UserRole~~", Session.ContactRoleTitle)
        sbContent.Replace("~~UserPreferences~~", wpmUtil.FormatLink("Preferences", "Preferences", String.Format("{0}login/contact_edit.aspx?key={1}", wpmApp.Config.wpmWebHome(), wpmSession.GetContactID())))
        sbContent.Replace("~~PageArticles~~", SiteProfile.BuildPageArticle(CurrentLocation.PageID, CurrentLocation.ArticleID))
        sbContent.Replace("~~CurrentPageURL~~", CurrentLocation.DisplayURL)
        ' Menu Options 
        sbContent.Replace("~~TopMenu~~", SiteProfile.BuildMenuChild(CurrentLocation.MainMenuPageID, "", ""))
        sbContent.Replace("~~SubTree~~", SiteProfile.BuildPageTree(CurrentLocation.PageID, 0, "topmenu"))
        sbContent.Replace("~~MainSubTree~~", SiteProfile.BuildPageTree(CurrentLocation.MainMenuPageID, 0, "topmenu"))
        sbContent.Replace("~~MainSubMenu~~", SiteProfile.BuildMenuChild(CurrentLocation.PageID, CurrentLocation.MainMenuPageID, CurrentLocation.MainMenuPageID))
        sbContent.Replace("~~2ndMenu~~", SiteProfile.BuildMenuChild(CurrentLocation.PageID, CurrentLocation.MainMenuPageID, CurrentLocation.MainMenuPageID))
        sbContent.Replace("~~3rdMenu~~", SiteProfile.BuildLinkMenu(3, CurrentLocation.PageID, "~~LINKS~~", CurrentLocation))

        ' YUI Menu Options
        'sbContent.Replace("~~yuiTopMenu~~", mySiteFile.yuiBuildMenuChild(CurrentMapRow.MainMenuPageID, String.Empty, String.Empty, "yuiTopMenu"))
        'sbContent.Replace("~~yuiMainSubMenu~~", mySiteFile.yuiBuildMenuChild(CurrentMapRow.PageID, CurrentMapRow.MainMenuPageID, CurrentMapRow.MainMenuPageID, "yuiMainSubMenu"))
        sbContent.Replace("~~yuiMainTree~~", SiteProfile.yuiBuildPageTree("", 0, "top"))
        sbContent.Replace("~~yuiSubMenu~~", SiteProfile.yuiBuildPageList(CurrentLocation.MainMenuPageID, CurrentLocation.MainMenuPageName, CurrentLocation.PageID, 0))
        sbContent.Replace("~~yuiChildrenMenu~~", SiteProfile.yuiBuildMenuChild(CurrentLocation.PageID, CurrentLocation.PageID, CurrentLocation.PageID, "yuiChildrenMenu"))
        sbContent.Replace("~~yuiSiblingMenu~~", SiteProfile.yuiBuildMenuChild(CurrentLocation.PageID, CurrentLocation.ParentPageID, CurrentLocation.ParentPageID, "yuiSiblingMenu"))
        ' MOO Tools Options
        sbContent.Replace("~~mooMenuTree~~", SiteProfile.mooBuildPageList(String.Empty, String.Empty, CurrentLocation.PageID, 0))
        ' Alternate Menu Options
        sbContent.Replace("~~ParentMenu~~", SiteProfile.BuildLinkListByParent(CurrentLocation.ParentPageID, CurrentLocation.ParentPageID, CurrentLocation.SiteCategoryID))
        sbContent.Replace("~~ChildrenMenu~~", SiteProfile.BuildLinkListByParent(CurrentLocation.PageID, CurrentLocation.PageID, CurrentLocation.SiteCategoryID))
        '<Jonathan's Changes>
        'This is a new tag I wanted to combine the breadcrumb menu with the children menu
        sbContent.Replace("~~BreadCrumbChildren~~", SiteProfile.BreadCrumbWithChildren(CurrentLocation.BreadCrumbHTML, CurrentLocation.PageID, CurrentLocation.PageID, CurrentLocation.SiteCategoryID))
        '</Jonathan's Changes>
        sbContent.Replace("~~SiblingMenu~~", SiteProfile.BuildLinkListBySibling(CurrentLocation.PageID, CurrentLocation.ParentPageID, CurrentLocation.SiteCategoryID))
        ' Replace Site Cateogry Tags
        ReplaceSiteCategoryTags(sbContent)
        ' Replace Company Tags
        SiteProfile.ReplaceTags(sbContent)
        Session.AddPageHistory(CurrentLocation.PageName)
        sbContent.Replace("~~debug~~", Session.GetSessionDebug())
        Return True
    End Function
    Private Function ReplaceSiteCategoryTags(ByRef sbContent As StringBuilder) As Boolean
        For Each myGroup As wpmSiteGroup In SiteProfile.SiteGroupList
            If (InStr(1, (myGroup.SiteCategoryGroupNM.ToUpper), "TOP") > 0) Then
                sbContent.Replace(String.Format("~~yui{0}~~", myGroup.SiteCategoryGroupNM), SiteProfile.yuiBuildSiteCategoryGroupBar(String.Empty, myGroup.SiteCategoryGroupNM, CurrentLocation.PageID, 1))
                sbContent.Replace(String.Format("~~{0}~~", myGroup.SiteCategoryGroupNM), SiteProfile.BuildSiteCategoryGroupList(String.Empty, myGroup.SiteCategoryGroupNM, CurrentLocation.PageID, 1, myGroup.SiteCategoryGroupDS))
            Else
                sbContent.Replace(String.Format("~~yui{0}~~", myGroup.SiteCategoryGroupNM), SiteProfile.yuiBuildSiteCategoryGroupList(String.Empty, myGroup.SiteCategoryGroupNM, CurrentLocation.PageID, 1))
                sbContent.Replace(String.Format("~~{0}~~", myGroup.SiteCategoryGroupNM), SiteProfile.BuildSiteCategoryGroupList(String.Empty, myGroup.SiteCategoryGroupNM, CurrentLocation.PageID, 1, myGroup.SiteCategoryGroupDS))
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
                    If ((myrow.SiteCategoryGroupID = CurrentLocation.SiteCategoryGroupID Or _
                         myrow.SiteCategoryGroupID = String.Empty) _
                        Or (myrow.PageID = CurrentLocation.PageID Or IsDBNull(myrow.PageID))) Then
                        Select Case myrow.LinkTypeCD
                            Case "MENU5"
                                sThisLink = SiteProfile.BuildLinkMenu(5, myrow.PageID, myrow.LinkURL, CurrentLocation)
                            Case "MENU4"
                                sThisLink = SiteProfile.BuildLinkMenu(4, myrow.PageID, myrow.LinkURL, CurrentLocation)
                            Case "MENU3"
                                sThisLink = SiteProfile.BuildLinkMenu(3, myrow.PageID, myrow.LinkURL, CurrentLocation)
                            Case "MENU2"
                                sThisLink = SiteProfile.BuildLinkMenu(2, myrow.PageID, myrow.LinkURL, CurrentLocation)
                            Case "MENU1"
                                sThisLink = SiteProfile.BuildLinkMenu(1, myrow.PageID, myrow.LinkURL, CurrentLocation)
                            Case "SubMenu"
                                myrow.LinkURL = Replace(myrow.LinkURL, "~~MainMenuName~~", CurrentLocation.MainMenuPageName)
                                sThisLink = SiteProfile.BuildLinkMenu(2, myrow.PageID, myrow.LinkURL, CurrentLocation)
                            Case "FILE"
                                ' If the linkPageID is null or 0 apply it to all pages
                                ' If the linkPageID is equal to the current page, apply it
                                ' If the linkPageID is not null and not the current page, ignore it
                                If IsDBNull(myrow.PageID) Then
                                    sThisLink = String.Format("<a href=""{0}"">{1}</a><br />", myrow.LinkURL, myrow.LinkTitle)
                                ElseIf (myrow.PageID = CurrentLocation.PageID) Then
                                    sThisLink = String.Format("<a href=""{0}"">{1}</a><br />", myrow.LinkURL, myrow.LinkTitle)
                                ElseIf (CInt(myrow.PageID) = 0) Then
                                    sThisLink = String.Format("<a href=""{0}"">{1}</a><br />", myrow.LinkURL, myrow.LinkTitle)
                                End If
                            Case "XML"
                                If myrow.PageID = CurrentLocation.PageID Or myrow.PageID = "" Then
                                    Dim mySB As New StringBuilder(myrow.LinkURL)
                                    If mySB.ToString.Contains("~~") Then
                                        If Not (SiteProfile.SiteParameterList.ReplaceSiteParameterTags(mySB, CurrentLocation)) Then
                                            wpmLogging.AuditLog("Error With ReplaceSiteParameterTags", "wpmSiteMap.ReplaceLinks-XML")
                                        End If
                                        mySB.Replace("~~PageKeywords~~", GetPageKeywords.Replace(" ", "+"))

                                        If Not CurrentLocation.PageName Is Nothing Then
                                            mySB.Replace("~~PageName~~", CurrentLocation.PageName.Replace(" ", "+"))
                                        End If

                                        SiteProfile.ReplaceTags(mySB)
                                    End If
                                    Dim myXML As New wpmXML(myrow.LinkTitle, mySB.ToString, myrow.LinkDescription, Session.CompanyID, SiteGallery)
                                    sThisLink = myXML.getXMLTransform()
                                End If
                            Case "RSS"
                                If myrow.PageID = CurrentLocation.PageID Or myrow.PageID = "" Then
                                    Dim myRSS As New wpmRssTools.wpmRSS(myrow.LinkURL)
                                    sThisLink = myRSS.getRSSFeed(SiteGallery)
                                End If
                            Case "SiteCategory"
                                Dim sSiteCategory As String = myrow.LinkURL
                                For Each myGroup As wpmSiteGroup In SiteProfile.SiteGroupList
                                    If (InStr(1, (myGroup.SiteCategoryGroupNM.ToUpper), "TOP") > 0) Then
                                        sSiteCategory = Replace(sSiteCategory, String.Format("~~{0}~~", myGroup.SiteCategoryGroupNM), SiteProfile.yuiBuildSiteCategoryGroupBar(myrow.PageID, myGroup.SiteCategoryGroupNM, myrow.PageID, 1))
                                    Else
                                        sSiteCategory = Replace(sSiteCategory,
                                                            String.Format("~~{0}~~", myGroup.SiteCategoryGroupNM),
                                                            SiteProfile.yuiBuildSiteCategoryGroupList(myrow.PageID, myGroup.SiteCategoryGroupNM, myrow.PageID, 1))
                                    End If
                                Next
                                sThisLink = sSiteCategory
                            Case "KeywordSearch"
                                sThisLink = ProcessKeywordSearch(SiteGallery)
                            Case Else
                                ' If the linkPageID is null or 0 apply it to all pages
                                ' If the linkPageID is equal to the current page, apply it
                                ' If the linkPageID is not null and not the current page, ignore it
                                If IsDBNull(myrow.PageID) Then
                                    sThisLink = myrow.LinkURL
                                ElseIf (myrow.PageID = CurrentLocation.PageID) Then
                                    sThisLink = myrow.LinkURL
                                ElseIf myrow.PageID = "" Then
                                    sThisLink = myrow.LinkURL
                                End If
                        End Select
                    End If
                    If sThisLink <> "" Then
                        If wpmUser.IsAdmin() Then
                            If myrow.LinkSource = "SiteLink" Then
                                sThisLink = String.Format("{0}<div class=""admin"">(<a href=""{1}SiteLink_edit.aspx?ID={2}"">edit {3}</a>)</div>",
                                                sThisLink,
                                                wpmApp.Config.AspMakerGen,
                                                myrow.LinkID,
                                                myrow.LinkTitle)
                            Else
                                sThisLink = String.Format("{0}<div class=""admin"">(<a href=""{1}link_edit.aspx?ID={2}"">edit {3}</a>)</div>",
                                                sThisLink,
                                                wpmApp.Config.AspMakerGen,
                                                myrow.LinkID,
                                                myrow.LinkTitle)
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
                        sRightLinks = String.Format("<a href=""{0}link_list.aspx"">NO RIGHT LINKS</a>", wpmApp.Config.AspMakerGen)
                    End If
                    If (sLeftLinks = "") Then
                        sLeftLinks = String.Format("<a href=""{0}link_list.aspx"">NO LEFT LINKS</a>", wpmApp.Config.AspMakerGen)
                    End If
                    If (sCenterLinks = "") Then
                        sCenterLinks = String.Format("<a href=""{0}link_list.aspx"">NO CENTER LINKS</a>", wpmApp.Config.AspMakerGen)
                    End If
                End If
            End If
            sbContent.Replace("~~LeftColumnLinks~~", sLeftLinks)
            sbContent.Replace("~~RightColumnLinks~~", sRightLinks)
            sbContent.Replace("~~CenterColumnLinks~~", sCenterLinks)
        End If
        Return True
    End Function
    Public Shared Function GetUserOptions() As String
        Dim sReturn As String = ""
        If wpmSession.GetContactID() <> "" Then
            sReturn = String.Format("<a href=""{0}login/logout.aspx"" >Sign Out</a>", wpmApp.Config.wpmWebHome())
        Else
            sReturn = String.Format("<a href=""{0}login/login.aspx"">Sign On</a>", wpmApp.Config.wpmWebHome())
        End If
        Return sReturn
    End Function
    Private Function ProcessKeywordSearch(ByVal SiteGallery As String) As String
        Dim myReturn As New StringBuilder(String.Empty)
        For Each myString As wpmKeyword In LocationList.KeywordList
            myReturn.Append(myString.Code & ",")
        Next
        Dim myContents As New StringBuilder
        wpmFileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath(String.Format("{0}/search/searchfield.js", SiteGallery)), myContents)
        myContents.Replace("<keywordlist>", myReturn.ToString)

        Session.AddHTMLHead = (String.Format("<script type=""text/javascript"">{0}</script>", myContents))
        Session.AddHTMLHead = (String.Format("<link rel=""stylesheet"" type=""text/css"" href=""{0}search/searchfield.css"">", SiteGallery))

        myReturn.Length = 0
        wpmFileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath(String.Format("{0}search/searchfield.html", SiteGallery)), myReturn)

        Dim mySearchKeyword As String = GetProperty("searchfield", String.Empty)

        If mySearchKeyword <> String.Empty Then
            wpmLogging.SearchLog(mySearchKeyword, "KeywordSearch")
            myReturn.Replace("~~SearchReturn~~", LocationList.FindLocationsByKeyword(mySearchKeyword))
        Else
            myReturn.Replace("~~SearchReturn~~", String.Empty)
        End If
        Return myReturn.ToString
    End Function


    Private Function GetSiteTitle() As String
        Dim mySiteTitle As New StringBuilder
        If CurrentLocation.PageTitle = "" Then
            mySiteTitle.Append(SiteProfile.SiteTitle)
        Else
            mySiteTitle.Append(CurrentLocation.PageTitle)
        End If
        mySiteTitle.Replace("<PageName>", CurrentLocation.PageName)
        mySiteTitle.Replace("<PageTitle>", CurrentLocation.PageTitle)
        mySiteTitle.Replace("<PageDescription>", CurrentLocation.PageDescription)
        mySiteTitle.Replace("<PageKeywords>", CurrentLocation.PageKeywords)
        Return mySiteTitle.ToString
    End Function
    Private Function GetPageKeywords() As String
        Dim mySiteKeywords As New StringBuilder
        If CurrentLocation.PageKeywords = "" Then
            mySiteKeywords.Append(SiteProfile.SiteKeywords)
        Else
            mySiteKeywords.Append(CurrentLocation.PageKeywords)
        End If
        mySiteKeywords.Replace("<PageName>", CurrentLocation.PageName)
        mySiteKeywords.Replace("<PageTitle>", CurrentLocation.PageTitle)
        mySiteKeywords.Replace("<PageDescription>", CurrentLocation.PageDescription)
        mySiteKeywords.Replace("<PageKeywords>", CurrentLocation.PageKeywords)
        mySiteKeywords.Replace("~~SiteCity~~", SiteProfile.SiteCity)
        mySiteKeywords.Replace("~~SiteState~~", SiteProfile.SiteState)
        Return mySiteKeywords.ToString
    End Function
    Private Function GetPageDescription() As String
        Dim mySiteDescription As New StringBuilder
        If CurrentLocation.PageKeywords = "" Then
            mySiteDescription.Append(SiteProfile.SiteDescription)
        Else
            mySiteDescription.Append(CurrentLocation.PageDescription)
        End If
        mySiteDescription.Replace("<PageName>", CurrentLocation.PageName)
        mySiteDescription.Replace("<PageTitle>", CurrentLocation.PageTitle)
        mySiteDescription.Replace("<PageDescription>", CurrentLocation.PageDescription)
        mySiteDescription.Replace("<PageKeywords>", CurrentLocation.PageKeywords)
        mySiteDescription.Replace("~~SiteCity~~", SiteProfile.SiteCity)
        mySiteDescription.Replace("~~SiteState~~", SiteProfile.SiteState)
        Return mySiteDescription.ToString
    End Function
    Private Shared Function NoLinkRows(ByRef sRightLinks As String, ByRef sLeftLinks As String, ByRef sCenterLinks As String) As Boolean
        If wpmUser.IsAdmin() Then
            sRightLinks = String.Format("<a href=""{0}link_list.aspx"">NO RIGHT LINKS</a>", wpmApp.Config.AspMakerGen)
            sLeftLinks = String.Format("<a href=""{0}link_list.aspx"">NO LEFT LINKS</a>", wpmApp.Config.AspMakerGen)
            sCenterLinks = String.Format("<a href=""{0}link_list.aspx"">NO CENTER LINKS</a>", wpmApp.Config.AspMakerGen)
        Else
            sRightLinks = ""
            sLeftLinks = ""
            sCenterLinks = ""
        End If
        Return True
    End Function

    Private Function GetPageAdmin() As String
        Dim sReturn As String = ("")
        If wpmUser.IsAdmin() Then
            Select Case CurrentLocation.RecordSource
                Case "Page"
                    sReturn = (String.Format("<div class=""wpmADMIN""><a href=""{0}zpage_edit.aspx?zPageID={1}"">Page Properties</a> | <a href=""{0}zpage_add.aspx?zPageID={1}"">Add Page</a> | <a href=""{0}article_add.aspx"">Add Article</a> | <a href=""{2}admin/AdminLink.aspx"">All Parts</a> | <a href=""{2}admin/default.aspx"">Admin Home</a> </div>", wpmApp.Config.AspMakerGen, CurrentLocation.PageID, wpmApp.Config.wpmWebHome()))
                Case "Article"
                    sReturn = (String.Format("<div class=""wpmADMIN""><a href=""{0}zpage_edit.aspx?zPageID={1}"">Page Properties</a> | <a href=""{0}zpage_add.aspx?zPageID={1}"">Add Page</a> | <a href=""{0}article_add.aspx"">Add Article</a> | <a href=""{2}admin/AdminLink.aspx"">All Parts</a> | <a href=""{2}admin/default.aspx"">Admin Home</a> </div>", wpmApp.Config.AspMakerGen, CurrentLocation.PageID, wpmApp.Config.wpmWebHome()))
                Case "Category"
                    sReturn = (String.Format("<div class=""wpmADMIN""><a href=""{0}SiteCategory_edit.aspx?SiteCategoryID={1}"">Page Properties</a> | <a href=""{2}admin/AdminLink.aspx"">All Parts</a> | <a href=""{2}admin/default.aspx"">Admin Home</a> </div>", wpmApp.Config.AspMakerGen, Replace(CurrentLocation.PageID, "CAT-", ""), wpmApp.Config.wpmWebHome()))
                Case Else
                    sReturn = (String.Format("<div class=""wpmADMIN""><a href=""{0}zpage_add.aspx?zPageID={1}"">Add Page</a> | <a href=""{0}article_add.aspx"">Add Article</a> | <a href=""{2}admin/AdminLink.aspx"">All Parts</a> | <a href=""{2}admin/default.aspx"">Admin Home</a> </div>", wpmApp.Config.AspMakerGen, CurrentLocation.PageID, wpmApp.Config.wpmWebHome()))
            End Select
        End If
        Return sReturn
    End Function

    Public Function WriteCurrentLocation() As Boolean
        Dim response As HttpResponse = HttpContext.Current.Response
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
                'Case "SITEMAP.XML"
                '    response.ContentType = "text/xml"
                '    Dim gen As wpmXmlSiteMap = New wpmXmlSiteMap(response.Output)
                '    gen.WriteSitemapDocument()
                '    gen.Dispose()
            Case Else
                If CurrentLocation.TransferURL Is Nothing Then
                    wpmLogging.AuditLog("Current Location Empty", "Redirect to ADMIN")
                    If DefaultArticleID = String.Empty And SiteHomePageID = String.Empty And SiteProfile.SiteCategoryTypeID = String.Empty Then
                        If HttpContext.Current.Request.RawUrl <> SiteProfile.SiteURL Then
                            If SiteProfile.SiteURL Is Nothing Then
                                wpmLogging.AuditLog("SiteURL is nothing", "Redirect to debug")
                                response.Redirect("~/wpm/debug.aspx", True)
                            Else
                                If wpmApp.Config.FullLoggingOn Then
                                    wpmLogging.AuditLog("301-Redirect", SiteProfile.SiteURL)
                                End If
                                wpmUtil.Build301Redirect(SiteProfile.SiteURL)
                            End If
                        Else
                            wpmLogging.AuditLog("Missing Key Site Components", "Redirect to debug")
                            response.Redirect("~/wpm/debug.aspx", True)
                        End If
                    Else
                        If SiteHomePageID = String.Empty Then
                            wpmLogging.AuditLog("SiteHomePageID Missing", "Redirect to debug")
                            response.Redirect("~/wpm/debug.aspx", True)
                        Else
                            SetCurrentPageID(SiteHomePageID)
                            SelectCurrentPageRow(SiteHomePageID, String.Empty)
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
        Return True
    End Function


    Public Function GetHTML(ByVal sMainContent As String, ByVal bUseDefault As Boolean, ByVal TemplatePrefix As String) As String
        If CurrentLocation.PageID = "" And CurrentLocation.ArticleID = "" And Trim(TemplatePrefix) = "" Then
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
            myHTML.Replace("~~RightContent~~", Session.RightContent)
            BuildTemplate(myHTML)
            myHTML.Replace("</head>", Session.AddHTMLHead & "</head>")
            ResetSessionVariables()
        End If
        SetListPage(HttpContext.Current.Request.ServerVariables.Item("QUERY_STRING"), HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), HttpContext.Current.Request.ServerVariables.Item("URL"))
        If wpmApp.Config.FullLoggingOn() Then
            wpmLogging.AccessLog(CurrentLocation.DisplayURL, CurrentLocation.TransferURL)
            wpmBuildHTML.SaveHTML(CurrentLocation.DisplayURL, myHTML.ToString)
        End If
        Return myHTML.ToString
    End Function


    Public Function GetBlogPageHTML() As String
        Dim sbBlogTemplate As New StringBuilder
        If (CurrentArticleID = String.Empty) Then
            If Not wpmFileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath(SiteProfile.SiteGallery & "/BlogPostsTemplate.txt"), sbBlogTemplate) Then
                wpmFileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath("/wpm/blog/BlogPostsTemplate.txt"), sbBlogTemplate)
            End If
        Else
            If Not wpmFileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath(SiteProfile.SiteGallery & "/BlogPostTemplate.txt"), sbBlogTemplate) Then
                wpmFileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath("/wpm/blog/BlogPostTemplate.txt"), sbBlogTemplate)
            End If
        End If

        Session.AddHTMLHead = String.Format("{0}<link rel=""alternate"" type=""application/rss+xml"" title=""RSS 2.0"" href=""http://{1}/wpm/blog/rss_blog.aspx?c={2}"" >{0}", _
                                     vbCrLf, _
                                     HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), _
                                     Session.CurrentPageID)
        Dim myBlog As New wpmArticleList(Me)
        Return GetHTML(myBlog.BuildBlogList(sbBlogTemplate, wpmUtil.GetDBInteger(GetProperty("Page", "0"))), False, Session.SiteTemplatePrefix)
    End Function

    Public Function GetArticlePageHTML() As String
        Dim myArticle As New wpmArticle(CurrentLocation, SiteProfile.DefaultArticleID)
        Return GetHTML(myArticle.ArticleBody, False, Session.SiteTemplatePrefix)
    End Function
    Public Function GetFormPageHTML() As String
        Dim mycontents As New StringBuilder
        wpmFileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath("/wpm/form/AddToHTMLHead.txt"), mycontents)
        Session.AddHTMLHead = mycontents.ToString
        Dim myArticle As New wpmArticle(CurrentLocation.ArticleID, CurrentLocation.PageID, SiteProfile.DefaultArticleID)
        If InStr(myArticle.ArticleBody, "<FORM", CompareMethod.Text) > 0 Or InStr(myArticle.ArticleBody, "<form", CompareMethod.Text) > 0 Then
            Return GetHTML(vbCrLf & myArticle.ArticleBody & vbCrLf, False, Session.SiteTemplatePrefix)
        Else
            Return GetHTML(String.Format("<form action=""/wpm/wpmForm.aspx"" method=""post"">{0}{1}{0}</form>", _
                               vbCrLf, _
                               myArticle.ArticleBody), False, Session.SiteTemplatePrefix)
        End If
    End Function
    Public Function GetCatalogPageHTML() As String
        Dim myImageList As New wpmPageImageList(Me)
        Return GetHTML(myImageList.ProcessPageRequest(HttpContext.Current.Request.Item("Page"), CurrentLocation.ArticleID), False, "")
    End Function

    Public Function GetSiteLocationAdmin() As String
        HttpContext.Current.Session("ListPageURL") = HttpContext.Current.Request.FilePath()
        LocationList.Sort()

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
        Return GetHTML(myLinkDirectory.MyStringBuilder.ToString, False, Session.SiteTemplatePrefix)
    End Function
    Public Function GetLinkPageHTML() As String
        Dim myLinkDirectory As New wpmLinkDirectory(Me)
        myLinkDirectory.DrawYUILinks(Me)
        Return GetHTML(myLinkDirectory.MyStringBuilder.ToString, False, Session.SiteTemplatePrefix)
    End Function

    Public Sub ResetSessionVariables()
        Session.CurrentArticleID = ""
        Session.CurrentPageID = ""
        Session.RightContent = ""
        Session.AddHTMLHead = "RESET"
    End Sub
    Public Shared Function GetProperty(ByVal myProperty As String, ByVal curValue As String) As String
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
        Dim SiteConfig As wpmSiteSettings = wpmSiteSettings.Load(wpmApp.wpmConfigFile)
        Session.CompanyID = SiteConfig.mySite.CompanyID
        Return True
    End Function
    Private Sub ResetCurrentRow()
        CurrentLocation.PageTypeCD = "404"
        CurrentLocation.PageID = ""
        CurrentLocation.ArticleID = ""
        CurrentLocation.TransferURL = ""
        CurrentLocation.MainMenuPageID = ""
        CurrentLocation.PageName = "404 - File Not Found"
        CurrentLocation.PageTitle = "404 - File Not Found"
    End Sub
    Public Function Process404(ByVal RawURL As String, ByVal QueryString As String) As Boolean
        Dim bReturn As Boolean = True
        Dim sTransferURL As String = String.Empty
        Dim sRedirectURL As String = String.Empty
        Dim response As HttpResponse = HttpContext.Current.Response

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
                        CurrentLocation.PageTypeCD = "sitemap.xml"
                        sTransferURL = String.Format("{0}sitemap.aspx", wpmApp.Config.wpmWebHome())
                    End If
                    If (Right(QueryString, 12) = "sitemap_view") Then
                        CurrentLocation.PageTypeCD = "sitemap_view"
                        sTransferURL = String.Format("{0}sitemap_view.aspx", wpmApp.Config.wpmWebHome())
                    End If
                    If (Right(QueryString, 7) = "sitemap") Then
                        CurrentLocation.PageTypeCD = "sitemap"
                        sTransferURL = String.Format("{0}site.aspx", wpmApp.Config.wpmWebHome())
                    End If
                    If (Right(QueryString, 12) = "rss_menu.xml") Then
                        CurrentLocation.PageTypeCD = "rss_menu.xml"
                        sTransferURL = String.Format("{0}rss_menu.aspx", wpmApp.Config.wpmWebHome())
                    End If
                    If sTransferURL = "" Then
                        ' Log the error and return FALSE
                        CurrentLocation.PageTypeCD = "404"
                        bReturn = False
                    Else
                        TransferToURL(sTransferURL)
                    End If
                End If
            Else
                wpmUtil.Build301Redirect(sRedirectURL)
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
                wpmFileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath("/wpm/404-Text.html"), myContents)
                myContents.Replace("<RequestURL>", sFileName.ToString)
                myContents.Replace("<UserHostAddress>", HttpContext.Current.Request.UserHostAddress)
                myContents.Replace("<UserLanguages>", HttpContext.Current.Request.UserAgent)
                myContents.Replace("<RequestBrowser>", HttpContext.Current.Request.Browser.Browser)
                Session.AddHTMLHead = myContents.ToString
                response.Write(GetHTML(String.Format("<blockquote>The page you were looking for can not be found</blockquote><br/><strong>{0}</strong><br/><br/><form><div align=""center""><textarea rows=8 cols=60 wrap=soft></textarea></div></form><br/><br/><hr><sitemap>", sFileName), False, Session.SiteTemplatePrefix))
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
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Private Shared Function BuildErrorMessage() As String
        Dim item As Object
        Dim strReturn As String = String.Format("{0}<br/><br/>Sorry, the file you were looking for can not be found. It may have moved to a new location.<br /><br />You can go to our <A href=""/"">homepage</A> or look in our sitemap:<br /><br />", Replace(HttpContext.Current.Request.ServerVariables.Item("QUERY_STRING"), "404;", String.Empty))
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
    Private Shared Function GetPath(ByVal refer As String) As String
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
        Dim indexc As Integer = InStrRev(refer, "/")
        Dim realpage As String = String.Empty
        Dim iQuestionMark As Integer = 0
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
                CurrentLocation.UpdatePageRow(myrow)
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
                bMatch = wpmUtil.CheckForMatch(urlName, myrow.DisplayURL)
            End If
            If (bMatch) Then
                If (myrow.ActiveFL Or wpmUser.IsAdmin()) Then
                    If (bStrict) Then
                        LinkURL = myrow.TransferURL
                        CurrentLocation.UpdatePageRow(myrow)

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
            Return SiteProfile.LocationList
        End Get
    End Property
    Public ReadOnly Property LinkCategoryList() As wpmPartGroupList
        Get
            Return SiteProfile.LinkCategoryList
        End Get
    End Property
    Public ReadOnly Property SiteImageList() As wpmSiteImageList
        Get
            Return SiteProfile.SiteImageList
        End Get
    End Property
    Public ReadOnly Property PartList() As wpmPartList
        Get
            Return SiteProfile.PartList
        End Get
    End Property
    Public ReadOnly Property SiteParameterList() As wpmSiteParameterList
        Get
            Return SiteProfile.SiteParameterList
        End Get
    End Property
    Public ReadOnly Property CompanyID() As String
        Get
            Return Session.CompanyID
        End Get
    End Property
    Public ReadOnly Property CompanyName() As String
        Get
            Return SiteProfile.CompanyName
        End Get
    End Property
    Public ReadOnly Property DefaultArticleID() As String
        Get
            Return SiteProfile.DefaultArticleID
        End Get
    End Property
    Public ReadOnly Property SiteHomePageID() As String
        Get
            Return SiteProfile.SiteHomePageID
        End Get
    End Property
    Public ReadOnly Property FromEmail() As String
        Get
            Return SiteProfile.FromEmail
        End Get
    End Property
    Public ReadOnly Property GroupID() As String
        Get
            Return Session.GroupID
        End Get
    End Property
    Public ReadOnly Property ContactID() As String
        Get
            Return Session.ContactID
        End Get
    End Property
    Public ReadOnly Property ContactName() As String
        Get
            Return Session.ContactName
        End Get
    End Property
    Public ReadOnly Property ContactEmail() As String
        Get
            Return Session.ContactEmail
        End Get
    End Property
    Public ReadOnly Property SiteGallery() As String
        Get
            Return Session.SiteGallery
        End Get
    End Property
    Public ReadOnly Property SiteTemplatePrefix() As String
        Get
            Return Session.SiteTemplatePrefix
        End Get
    End Property
    Public ReadOnly Property SitePrefix() As String
        Get
            Return SiteProfile.SitePrefix
        End Get
    End Property
    Public ReadOnly Property DefaultSitePrefix() As String
        Get
            Return SiteProfile.DefaultSitePrefix
        End Get
    End Property
    Public ReadOnly Property SiteURL() As String
        Get
            Return SiteProfile.SiteURL
        End Get
    End Property
    Public ReadOnly Property UseBreadCrumbURL() As Boolean
        Get
            Return SiteProfile.UseBreadCrumbURL
        End Get
    End Property
    
    Public ReadOnly Property ListPageURL() As String
        Get
            Return Session.ListPageURL
        End Get
    End Property
    Public ReadOnly Property CurrentPageID() As String
        Get
            Return CurrentLocation.PageID
        End Get
    End Property
    Public ReadOnly Property CurrentArticleID() As String
        Get
            Return CurrentLocation.ArticleID
        End Get
    End Property
    Public ReadOnly Property GetCurrentPageName() As String
        Get
            Return CurrentLocation.PageName
        End Get
    End Property
    Public ReadOnly Property CurrentPageDescription() As String
        Get
            Return CurrentLocation.PageDescription
        End Get
    End Property
    Public ReadOnly Property CurrentPageTypeCD() As String
        Get
            Return CurrentLocation.PageTypeCD
        End Get
    End Property
    Public ReadOnly Property CurrentDisplayURL() As String
        Get
            Return CurrentLocation.DisplayURL
        End Get
    End Property
    Public ReadOnly Property CurrentPageFileName() As String
        Get
            Return CurrentLocation.PageFileName
        End Get
    End Property
    Public ReadOnly Property SiteTitle() As String
        Get
            Return SiteProfile.SiteTitle
        End Get
    End Property
#End Region
#Region "Updateable Properties"
    '
    '  Updateable Properties
    '
    Public Property RightContent() As String
        Get
            Return Session.RightContent
        End Get
        Set(ByVal value As String)
            Session.RightContent = value
        End Set
    End Property
    Public Property AddHTMLHead() As String
        Get
            Return Session.AddHTMLHead
        End Get
        Set(ByVal value As String)
            Session.AddHTMLHead = value
        End Set
    End Property
#End Region
End Class
