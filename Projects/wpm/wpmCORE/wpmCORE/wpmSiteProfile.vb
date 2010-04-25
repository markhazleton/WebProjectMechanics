Public Class wpmSiteProfile
    Public SiteImageList As wpmSiteImageList
    Public ArticleList As wpmArticleList
    Public PageAliasList As wpmPageAliasList
#Region "Class Members"
    Private _LocationList As New wpmLocationList
    Public ReadOnly Property LocationList() As wpmLocationList
        Get
            Return _LocationList
        End Get
    End Property
    Private _LinkCategoryList As New wpmPartGroupList
    Public ReadOnly Property LinkCategoryList() As wpmPartGroupList
        Get
            Return _LinkCategoryList
        End Get
    End Property
    Private _PartList As New wpmPartList
    Public ReadOnly Property PartList() As wpmPartList
        Get
            Return _PartList
        End Get
    End Property
    Private _SiteGroupList As New wpmSiteGroupList
    Public ReadOnly Property SiteGroupList() As wpmSiteGroupList
        Get
            Return _SiteGroupList
        End Get
    End Property
    Private _SiteParameterList As New wpmSiteParameterList
    Public ReadOnly Property SiteParameterList() As wpmSiteParameterList
        Get
            Return _SiteParameterList
        End Get
    End Property
    Public ReadOnly Property TreeHTML() As String
        Get
            Return BuildPageTree("", 0, "sitemap")
        End Get
    End Property
    Private _CompanyName As String
    Public Property CompanyName() As String
        Get
            Return _CompanyName
        End Get
        Set(ByVal value As String)
            _CompanyName = value
        End Set
    End Property
    Private _SiteURL As String
    Public Property SiteURL() As String
        Get
            Return _SiteURL
        End Get
        Set(ByVal value As String)
            _SiteURL = value
        End Set
    End Property
    Private _SiteTitle As String
    Public Property SiteTitle() As String
        Get
            Return _SiteTitle
        End Get
        Set(ByVal value As String)
            _SiteTitle = value
        End Set
    End Property
    Private _SiteKeywords As String
    Public Property SiteKeywords() As String
        Get
            Return _SiteKeywords
        End Get
        Set(ByVal value As String)
            _SiteKeywords = value
        End Set
    End Property
    Private _SiteDescription As String
    Public Property SiteDescription() As String
        Get
            Return _SiteDescription
        End Get
        Set(ByVal value As String)
            _SiteDescription = value
        End Set
    End Property
    Private _SiteHomePageID As String
    Public Property SiteHomePageID() As String
        Get
            Return _SiteHomePageID
        End Get
        Set(ByVal value As String)
            _SiteHomePageID = value
        End Set
    End Property
    Private _DefaultArticleID As String
    Public Property DefaultArticleID() As String
        Get
            Return _DefaultArticleID
        End Get
        Set(ByVal value As String)
            _DefaultArticleID = value
        End Set
    End Property
    Private _SitePrefix As String
    Public Property SitePrefix() As String
        Get
            Return _SitePrefix
        End Get
        Set(ByVal value As String)
            _SitePrefix = value
        End Set
    End Property
    Private _DefaultSitePrefix As String
    Public Property DefaultSitePrefix() As String
        Get
            Return _DefaultSitePrefix
        End Get
        Set(ByVal value As String)
            _DefaultSitePrefix = value
        End Set
    End Property
    Private _SiteGallery As String
    Public Property SiteGallery() As String
        Get
            Return _SiteGallery
        End Get
        Set(ByVal value As String)
            _SiteGallery = value
        End Set
    End Property
    Private _SiteConfig As String
    Public Property SiteConfig() As String
        Get
            Return _SiteConfig
        End Get
        Set(ByVal value As String)
            _SiteConfig = value
        End Set
    End Property
    Private _siteState As String
    Public Property SiteState() As String
        Get
            Return _siteState
        End Get
        Set(ByVal Value As String)
            _siteState = Value
        End Set
    End Property
    Private _siteCountry As String
    Public Property SiteCountry() As String
        Get
            Return _siteCountry
        End Get
        Set(ByVal Value As String)
            _siteCountry = Value
        End Set
    End Property
    Private _siteCity As String
    Public Property SiteCity() As String
        Get
            Return _siteCity
        End Get
        Set(ByVal Value As String)
            _siteCity = Value
        End Set
    End Property
    Private _siteCategoryTypeID As String
    Public Property SiteCategoryTypeID() As String
        Get
            Return _siteCategoryTypeID
        End Get
        Set(ByVal Value As String)
            _siteCategoryTypeID = Value
        End Set
    End Property
    Private _DefaultSiteCategoryID As String
    Public Property DefaultSiteCategoryID() As String
        Get
            Return _DefaultSiteCategoryID
        End Get
        Set(ByVal value As String)
            _DefaultSiteCategoryID = value
        End Set
    End Property
    Private _PageStructure As String
    Public ReadOnly Property PageStructure() As String
        Get
            Return "<div class=""sitemap""><ul class=""sitemap"">" & vbCrLf & TreeHTML & "</ul></div>" & vbCrLf
        End Get
    End Property
    Private _UseBreadCrumbURL As Boolean
    Public Property UseBreadCrumbURL() As Boolean
        Get
            Return _UseBreadCrumbURL
        End Get
        Set(ByVal value As Boolean)
            _UseBreadCrumbURL = value
        End Set
    End Property
    Private _FromEmail As String
    Public Property FromEmail() As String
        Get
            Return _FromEmail
        End Get
        Set(ByVal value As String)
            _FromEmail = value
        End Set
    End Property
    Private _SMTP As String
    Public Property SMTP() As String
        Get
            Return _SMTP
        End Get
        Set(ByVal value As String)
            _SMTP = value
        End Set
    End Property
    Private _Component As String
    Public Property Component() As String
        Get
            Return _Component
        End Get
        Set(ByVal value As String)
            _Component = value
        End Set
    End Property
#End Region
#Region "Public Methods"
    Public Sub GetSiteFileFromDB(ByVal CompanyID As String, ByVal GroupID As String, ByVal OrderBy As String)
        If Trim(OrderBy) = "" Then
            OrderBy = "ORDER"
        End If
        GetCompanyValues(CompanyID)
        LocationList.GetLocationListFromDb(OrderBy, CompanyID, GroupID)
        LocationList.BuildBreadcrumbRows(CompanyName)
        If Me.SiteCategoryTypeID = "" Then
            PartList.PopulateSiteLinkRows(CompanyID)
        Else
            PartList.PopulateSiteCategoryLinkRows(CompanyID, Me.SiteCategoryTypeID)
        End If
        LinkCategoryList.PopulateLinkCategoryList(CompanyID)
        updateLinkCategoryList()
        SiteGroupList.PopulateSiteGroupList(CompanyID)
        SiteParameterList.PopulateParameterTypeList(CompanyID, Me.SiteCategoryTypeID)
        ArticleList = New wpmArticleList(CompanyID)
        SiteImageList = New wpmSiteImageList(CompanyID)
        PageAliasList = New wpmPageAliasList(CompanyID)
        If App.Config.FullLoggingOn() Or wpmUser.IsAdmin Then
            SaveSiteMapFile(App.Config.ConfigFolderPath & "\index\" & Me.CompanyName & "-site-file.xml")
        End If

    End Sub
    Public Function ProcessCategories() As Boolean
        For Each myRow As wpmLocation In Me.LocationList
            ReplaceTags(myRow.PageDescription)
            ReplaceTags(myRow.PageName)
            ReplaceTags(myRow.PageKeywords)
            ReplaceTags(myRow.PageTitle)
            ReplaceTags(myRow.DisplayURL)
            ReplaceTags(myRow.BreadCrumbURL)
        Next
        Return True
    End Function
    Public Function BuildPageArticle(ByVal PageID As String, ByVal ArticleID As String) As String
        Dim sReturn As New StringBuilder("")
        For Each myrow As wpmLocation In LocationList
            If myrow.RecordSource = "Article" Then
                If myrow.PageID = PageID Then
                    If myrow.ArticleID = ArticleID Then
                        sReturn.Append("<li class=""selected"">")
                        sReturn.Append(BuildNaviagtionLink(myrow, True))
                    Else
                        sReturn.Append("<li>")
                        sReturn.Append(BuildNaviagtionLink(myrow, False))
                    End If
                    sReturn.Append("</li>" & vbCrLf)
                End If
            End If
        Next
        If sReturn.Length > 0 Then
            sReturn.Insert(0, "<ul>" & vbCrLf)
            sReturn.Append("</ul>" & vbCrLf)
        End If
        Return sReturn.ToString
    End Function
    Public Function BuildLinkListBySibling(ByVal PageID As String, ByVal ParentPageID As String, ByVal SiteCategoryID As String) As String
        Dim sReturn As New StringBuilder(String.Empty)
        Dim sParentPageName As String = String.Empty
        For Each myrow As wpmLocation In LocationList
            If myrow.RecordSource = "Page" Or myrow.RecordSource = "Category" Then
                If (myrow.ParentPageID = ParentPageID And ParentPageID <> String.Empty) Then
                    If myrow.PageID = PageID Then
                        sReturn.Append("<li class=""menuitem selected"">")
                        sReturn.Append(BuildNaviagtionLink(myrow, True))
                    Else
                        sReturn.Append("<li class=""menuitem"">")
                        sReturn.Append(BuildNaviagtionLink(myrow, False))
                    End If
                    sReturn.Append("</li>" & vbCrLf)
                End If
                If (myrow.PageID = ParentPageID) Then
                    sParentPageName = BuildNaviagtionLink(myrow, False)
                End If
            End If
        Next
        If sReturn.Length > 0 Then
            sReturn.Insert(0, "<div class=""menugroup""><h2 class=""menugroupname"">" & sParentPageName & "</h2><ul class=""menu"">" & vbCrLf)
            sReturn.Append("</ul></div>" & vbCrLf)
        End If
        Return sReturn.ToString
    End Function
    Public Function BuildLinkListByParent(ByVal PageID As String, ByVal ParentPageID As String, ByVal SiteCategoryID As String) As String
        Dim sReturn As New StringBuilder("")
        Dim sParentPageName As String = String.Empty

        If ParentPageID = String.Empty And SiteCategoryID = String.Empty Then
            ' do nothing
        Else
            For Each myrow As wpmLocation In LocationList
                If myrow.RecordSource = "Page" Or myrow.RecordSource = "Category" Then
                    If (myrow.ParentPageID = ParentPageID) Then
                        If myrow.PageID = PageID Then
                            sReturn.Append("<li class=""menuitem selected"">")
                            sReturn.Append(BuildNaviagtionLink(myrow, True))
                        Else
                            sReturn.Append("<li class=""menuitem"">")
                            sReturn.Append(BuildNaviagtionLink(myrow, False))
                        End If
                        sReturn.Append("</li>" & vbCrLf)
                    End If
                    If (myrow.PageID = PageID) Then
                        ' sParentPageName = myrow.PageName
                        sParentPageName = BuildNaviagtionLink(myrow, False)
                    End If
                End If
            Next
            If sReturn.Length > 0 Then
                sReturn.Insert(0, "<div class=""menugroup""><h2 class=""menugroupname"">" & sParentPageName & "</h2><ul class=""menu"">" & vbCrLf)
                sReturn.Append("</ul></div>" & vbCrLf)
            End If
        End If

        Return sReturn.ToString
    End Function
    'This is a combination of a bread crumb menu with a children menu
    '<TODO> I imagine I should create subfunctions rather than copy/paste code
    Public Function BreadCrumbWithChildren(ByVal BreadCrumb As String, ByVal PageID As String, ByVal ParentPageID As String, ByVal SiteCategoryID As String) As String
        Dim sReturn As New StringBuilder("")
        Dim sParentPageName As String = String.Empty

        If ParentPageID = String.Empty And SiteCategoryID = String.Empty Then
            sReturn.Append("<li><strong>NO Parent, No Category</strong></li>")
        Else
            For Each myrow As wpmLocation In LocationList
                If myrow.RecordSource = "Page" Or myrow.RecordSource = "Category" Then
                    If (myrow.ParentPageID = ParentPageID) Then
                        If myrow.PageID = PageID Then
                            sReturn.Append("<li class=""menuitem selected"">")
                            sReturn.Append(BuildNaviagtionLink(myrow, True))
                        Else
                            sReturn.Append("<li class=""menuitem"">")
                            sReturn.Append(BuildNaviagtionLink(myrow, False))
                        End If
                        sReturn.Append("</li>" & vbCrLf)
                    End If
                    If (myrow.PageID = PageID) Then
                        sParentPageName = BuildNaviagtionLink(myrow, False)
                    End If
                End If
            Next
        End If

        If BreadCrumb Is Nothing Then
            Return sReturn.ToString
        Else
            Return BreadCrumb.Substring(0, BreadCrumb.LastIndexOf("</ul>")) & "<ul>" & sReturn.ToString & "</ul></ul>"
        End If

    End Function

#Region "YUI Functions"
    Public Function yuiBuildMenuChild(ByVal PageID As String, ByVal ParentPageID As String, ByVal DefaultParentPageID As String, ByVal MenuID As String) As String
        Dim sReturn As New StringBuilder(String.Empty)
        Dim myParentPageID As String = String.Empty
        If ParentPageID <> String.Empty Then
            sReturn.Append(yuiBuildLineItemList(ParentPageID, "first-of-type", PageID))
            If sReturn.Length > 0 Then
                Dim sScript As String
                sScript = "<script type=""text/javascript"">" & vbCrLf & "YAHOO.util.Event.onContentReady(""" & MenuID & PageID & """, function () {var oMenu = new YAHOO.widget.Menu(""" & MenuID & PageID & """, {position: ""static"", hidedelay: 750, lazyload: true });oMenu.render();});" & vbCrLf & "</script>" & vbCrLf
                sReturn.Insert(0, sScript & "<div id=""" & MenuID & PageID & """ class=""yuimenu"">" & vbCrLf & "<div class=""bd"">" & vbCrLf)
                sReturn.Append(vbCrLf & "</div>" & vbCrLf & "</div>")
            End If
        End If
        Return sReturn.ToString
    End Function
    Public Function yuiBuildPageList(ByVal ParentPageID As String, ByVal GroupName As String, ByVal PageID As String, ByVal Level As Integer) As String
        Dim sPageName As String = (String.Empty)
        Dim sReturn As New StringBuilder(String.Empty)
        If ParentPageID <> String.Empty Then
            For Each myrow As wpmLocation In LocationList
                If (myrow.RecordSource = "Category" Or myrow.RecordSource = "Page") Then
                    If (myrow.ParentPageID = ParentPageID) Then
                        If myrow.PageID = PageID Then
                            sReturn.Append("<li class=""yuimenuitem selected"">")
                            sReturn.Append(BuildClassLink(myrow, "yuimenuitemlabel selected"))
                        Else
                            sReturn.Append("<li class=""yuimenuitem "">")
                            sReturn.Append(BuildClassLink(myrow, "yuimenuitemlabel"))
                        End If
                        sReturn.Append(yuiBuildPageList(myrow.PageID, myrow.PageName, PageID, Level + 1))
                        sReturn.Append("</li>" & vbCrLf)
                    End If
                    If myrow.PageID = ParentPageID Then
                        sPageName = myrow.PageName
                    End If
                End If
            Next
            If sReturn.Length > 0 Then
                If Level > 1 Then
                    sReturn.Append(vbCrLf)
                    sReturn.Insert(0, "<div id=""g_" & Replace(GroupName, " ", "") & "-" & Replace(sPageName, " ", "") & """ class=""yuimenu""><div class=""bd""><ul class=""SiteCategoryGroup" & GroupName & """>")
                    sReturn.Append("</ul></div></div>")
                    sReturn.Append(vbCrLf)
                Else
                    sReturn.Insert(0, "<ul id=""g_" & Replace(GroupName, " ", "") & """>")
                    sReturn.Append("</ul>")
                End If
            End If
        End If
        Return sReturn.ToString
    End Function
    Public Function yuiBuildSiteCategoryGroupList(ByVal ParentPageID As String, ByVal GroupName As String, ByVal PageID As String, ByVal Level As Integer) As String
        Dim sPageName As String = ("")
        Dim sReturn As New StringBuilder("")
        For Each myrow As wpmLocation In LocationList
            If (myrow.SiteCategoryGroupName = GroupName) Then
                If (myrow.ParentPageID = ParentPageID) Or (Level = 1 And myrow.ParentPageID = "") Then
                    If myrow.PageID = PageID Then
                        sReturn.Append("<li class=""yuimenuitem selected"">")
                        sReturn.Append(BuildClassLink(myrow, "yuimenuitemlabel selected"))
                    Else
                        sReturn.Append("<li class=""yuimenuitem "">")
                        sReturn.Append(BuildClassLink(myrow, "yuimenuitemlabel"))
                    End If
                    sReturn.Append(yuiBuildSiteCategoryGroupList(myrow.PageID, GroupName, PageID, Level + 1))
                    sReturn.Append("</li>" & vbCrLf)
                End If
                If myrow.PageID = ParentPageID Then
                    sPageName = myrow.PageName
                End If
            End If
        Next
        If sReturn.Length > 0 Then
            If Level > 1 Then
                sReturn.Append(vbCrLf)
                sReturn.Insert(0, "<div id=""" & GroupName & "-" & sPageName & """ class=""yuimenu""><div class=""bd""><ul class=""SiteCategoryGroup" & GroupName & """>")
                sReturn.Append("</ul></div></div>")
                sReturn.Append(vbCrLf)
            Else
                sReturn.Insert(0, "<ul id=""" & GroupName & """>")
                sReturn.Append("</ul>")
            End If
        End If
        Return sReturn.ToString
    End Function
    Public Function yuiBuildSiteCategoryGroupBar(ByVal ParentPageID As String, ByVal GroupName As String, ByVal PageID As String, ByVal Level As Integer) As String
        Dim sPageName As String = ("")
        Dim sReturn As New StringBuilder("")
        For Each myrow As wpmLocation In LocationList
            If (myrow.RecordSource = "Category" And myrow.SiteCategoryGroupName = GroupName) Then
                If myrow.ParentPageID = ParentPageID Then
                    If myrow.PageID = PageID Then
                        sReturn.Append("<li class=""yuimenubaritem  selected"">")
                        sReturn.Append(BuildClassLink(myrow, "yuimenubaritemlabel selected"))
                    Else
                        sReturn.Append("<li class=""yuimenubaritem  "">")
                        sReturn.Append(BuildClassLink(myrow, "yuimenubaritemlabel"))
                    End If
                    sReturn.Append(yuiBuildSiteCategoryGroupList(myrow.PageID, GroupName, PageID, Level + 1))
                    sReturn.Append("</li>" & vbCrLf)
                End If
                If myrow.PageID = ParentPageID Then
                    sPageName = myrow.PageName
                End If
            End If
            If "CAT-" & myrow.SiteCategoryID = ParentPageID And myrow.RecordSource = "Page" Then
                sReturn.Append("<li class=""yuimenubaritem "">")
                sReturn.Append(BuildClassLink(myrow, "yuimenubaritemlabel"))
                sReturn.Append("</li>" & vbCrLf)
            End If
        Next
        If sReturn.Length > 0 Then
            If Level > 1 Then
                sReturn.Append(vbCrLf)
                sReturn.Insert(0, "<div id=""" & GroupName & "-" & sPageName & """ class=""yuimenu""><div class=""bd""><ul class=""SiteCategoryGroup" & GroupName & """>")
                sReturn.Append("</ul></div></div>")
                sReturn.Append(vbCrLf)
            Else
                sReturn.Insert(0, "<ul id=""" & GroupName & """>")
                sReturn.Append("</ul>")
            End If
        End If
        Return sReturn.ToString
    End Function
    Public Function yuiBuildPageTree(ByVal ParentID As String, ByVal intLevel As Double, ByVal PageName As String) As String
        Dim sReturn As New StringBuilder("")
        For Each myRow As wpmLocation In LocationList
            If myRow.RecordSource = "Page" Then
                If (ParentID = myRow.ParentPageID) Then
                    If intLevel = 0 Then
                        PageName = myRow.PageName
                        sReturn.Append("<li class=""yuimenubaritem"">")
                        sReturn.Append(BuildClassLink(myRow, "yuimenubaritemlabel"))
                        sReturn.Append(yuiBuildPageTree(myRow.PageID, intLevel + 1, myRow.PageName) & "</li>" & vbCrLf)
                    Else
                        sReturn.Append("<li class=""yuimenuitem"">")
                        sReturn.Append(BuildClassLink(myRow, "yuimenuitemlabel"))
                        sReturn.Append(yuiBuildPageTree(myRow.PageID, intLevel + 1, myRow.PageName) & "</li>" & vbCrLf)
                    End If
                End If
            End If
        Next
        If (sReturn.Length > 0) Then
            If intLevel = 0 Then
                sReturn.Insert(0, "<div id=""yuiMenuTree"" class=""yuimenubar yuimenubarnav"">" & vbCrLf & "<div class=""bd"">" & vbCrLf & "<ul class=""first-of-type"">" & vbCrLf)
            Else
                sReturn.Insert(0, "<div id=""p_" & Replace(PageName, " ", "") & """ class=""yuimenu"">" & vbCrLf & "<div class=""bd"">" & vbCrLf & "<ul>" & vbCrLf)
            End If
            sReturn.Append("</ul></div></div>" & vbCrLf)
        End If
        Return sReturn.ToString
    End Function
#End Region
    Public Function mooBuildPageList(ByVal ParentPageID As String, ByVal GroupName As String, ByVal PageID As String, ByVal Level As Integer) As String
        Dim sPageName As String = (String.Empty)
        Dim sReturn As New StringBuilder(String.Empty)
        If ParentPageID <> String.Empty Or Level = 0 Then
            For Each myrow As wpmLocation In LocationList
                If (myrow.RecordSource = "Category" Or myrow.RecordSource = "Page") Then
                    If (myrow.ParentPageID = ParentPageID) Then
                        If myrow.PageID = PageID Then
                            sReturn.Append("<li class=""active_over"" onmouseover=""this.className='active_over'"" onmouseout=""this.className='active'"">")
                            sReturn.Append(BuildClassLink(myrow, "selected"))
                        Else
                            sReturn.Append("<li class=""active"" onmouseover=""this.className='active_over'"" onmouseout=""this.className='active'"">")
                            sReturn.Append(BuildClassLink(myrow, ""))
                        End If
                        sReturn.Append(mooBuildPageList(myrow.PageID, myrow.PageName, PageID, Level + 1))
                        sReturn.Append("</li>" & vbCrLf)
                    End If
                    If myrow.PageID = ParentPageID Then
                        sPageName = myrow.PageName
                    End If
                End If
            Next
            If sReturn.Length > 0 Then
                If Level > 0 Then
                    sReturn.Append(vbCrLf)
                    sReturn.Insert(0, "<div class=""sub-menu""><ul>")
                    sReturn.Append("</ul></div>")
                    sReturn.Append(vbCrLf)
                Else
                    sReturn.Insert(0, "<ul>")
                    sReturn.Append("</ul>")
                End If
            End If
        End If
        Return sReturn.ToString
    End Function


    Public Function BuildSiteCategoryGroupList(ByVal ParentPageID As String, ByVal GroupName As String, ByVal PageID As String, ByVal Level As Integer, ByVal GroupDescription As String) As String
        Dim sPageName As String = ("")
        Dim sReturn As New StringBuilder("")
        For Each myrow As wpmLocation In LocationList
            If (myrow.SiteCategoryGroupName = GroupName) Then
                If (myrow.ParentPageID = ParentPageID) Or (Level = 1 And myrow.ParentPageID = "") Then
                    If myrow.PageID = PageID Then
                        sReturn.Append("<li class=""menuitem selected"">")
                        sReturn.Append(BuildClassLink(myrow, "menuitemlabel selected"))
                    Else
                        sReturn.Append("<li class=""menuitem "">")
                        sReturn.Append(BuildClassLink(myrow, "menuitemlabel"))
                    End If
                    sReturn.Append("</li>" & vbCrLf)
                End If
                If myrow.PageID = ParentPageID Then
                    sPageName = myrow.PageName
                End If
            End If
        Next
        If sReturn.Length > 0 Then
            If Level > 1 Then
                sReturn.Append(vbCrLf)
                sReturn.Insert(0, "<ul class=""submenu"">")
                sReturn.Append("</ul>")
                sReturn.Append(vbCrLf)
            Else
                sReturn.Insert(0, "<div class=""menugroup""><h2 class=""menugroupname"">" & GroupDescription & "</h2><ul class=""menu"">")
                sReturn.Append("</ul></div>")
            End If
        End If
        Return sReturn.ToString
    End Function
    Public Function BuildMenuChild(ByVal PageID As String, ByVal ParentPageID As String, ByVal DefaultParentPageID As String) As String
        Dim sReturn As New StringBuilder("")
        For Each myrow As wpmLocation In LocationList
            If myrow.RecordSource = "Page" Then
                If myrow.ParentPageID = ParentPageID Then
                    If myrow.PageID = PageID Then
                        sReturn.Append("<li class=""selected"">")
                        sReturn.Append(BuildNaviagtionLink(myrow, True))
                    Else
                        sReturn.Append("<li>")
                        sReturn.Append(BuildNaviagtionLink(myrow, False))
                    End If
                    sReturn.Append("</li>" & vbCrLf)
                End If
            End If
        Next
        If sReturn.Length > 0 Then
            sReturn.Insert(0, "<ul>" & vbCrLf)
            sReturn.Append("</ul>" & vbCrLf)
        Else
            If ParentPageID <> DefaultParentPageID And DefaultParentPageID <> "" Then
                sReturn.Append(BuildMenuChild(PageID, DefaultParentPageID, DefaultParentPageID))
            End If
        End If
        Return sReturn.ToString
    End Function
    Public Function BuildPageTree(ByVal ParentID As String, ByVal intLevel As Double, ByVal sULClassName As String) As String
        Dim sReturn As New StringBuilder("")
        For Each myRow As wpmLocation In LocationList
            If myRow.RecordSource = "Page" Then
                If ParentID = myRow.ParentPageID Then
                    sReturn.Append("<li>")
                    sReturn.Append(BuildNaviagtionLink(myRow, False))
                    sReturn.Append(BuildPageTree(myRow.PageID, intLevel + 1, sULClassName) & "</li>" & vbCrLf)
                End If
            End If
        Next
        If (sReturn.Length > 0) Then
            If intLevel = 0 Then
                sReturn.Insert(0, "<ul class=""" & sULClassName & """>" & vbCrLf)
            Else
                sReturn.Insert(0, "<ul>" & vbCrLf)
            End If
            sReturn.Append("</ul>" & vbCrLf)
        End If
        Return sReturn.ToString
    End Function
    Public Function BuildLinkMenu(ByVal RequestLevel As Integer, ByVal DefaultParentPageID As String, ByVal myReturnTemplate As String, ByRef CurrentMapRow As wpmLocation) As String
        Dim ReturnString As String = ("")
        Dim PageID As String = CurrentMapRow.PageID
        Dim ParentPageID As String = CurrentMapRow.ParentPageID
        Dim ParentPageName As String = ""
        If RequestLevel <= CurrentMapRow.LevelNBR Then
            For Each myrow As wpmLocationTrail In CurrentMapRow.LocationTrailList
                If RequestLevel = myrow.MenuLevelNBR Then
                    PageID = myrow.PageID
                    ParentPageID = myrow.ParentPageID
                    Exit For
                End If
            Next
            For Each myrow As wpmLocationTrail In CurrentMapRow.LocationTrailList
                If myrow.PageID = ParentPageID Then
                    ParentPageName = myrow.PageName
                    Exit For
                End If
            Next
            If DefaultParentPageID = "" Then
                DefaultParentPageID = ParentPageID
            End If
            ReturnString = BuildMenuChild(PageID, ParentPageID, DefaultParentPageID)
        ElseIf RequestLevel = CurrentMapRow.LevelNBR + 1 Then
            ReturnString = BuildMenuChild(PageID, PageID, DefaultParentPageID)
        End If
        If ParentPageName = "" Then
            ParentPageName = CurrentMapRow.PageName
        End If
        If Not (Trim(ReturnString) = "") Then
            ReturnString = Replace(myReturnTemplate, "~~LINKS~~", ReturnString)
            ReturnString = Replace(ReturnString, "~~ParentPageName~~", ParentPageName)
        End If
        Return ReturnString
    End Function
    Public Function ReplaceTags(ByVal sValue As String) As String
        If sValue <> String.Empty And sValue.Contains("~~") Then
            Dim mySB As New StringBuilder(sValue)
            ReplaceTags(mySB)
            sValue = mySB.ToString
        End If
        Return sValue
    End Function
    Public Function ReplaceTags(ByRef sbContent As StringBuilder) As Boolean
        sbContent.Replace("~~SiteURL~~", Me.SiteURL)
        sbContent.Replace("~~SiteCompanyName~~", Me.CompanyName)
        sbContent.Replace("~~SiteDescription~~", Me.SiteDescription)
        sbContent.Replace("~~SiteKeywords~~", Me.SiteKeywords)
        sbContent.Replace("~~SiteMap~~", Me.PageStructure)
        sbContent.Replace("~~SiteTree~~", Me.TreeHTML)
        sbContent.Replace("~~SiteCity~~", Me.SiteCity)
        sbContent.Replace("~~SiteCityDash~~", wpmUTIL.FormatPageNameForURL(Me.SiteCity))
        sbContent.Replace("~~SiteCityNoSpace~~", Replace(Me.SiteCity, " ", ""))
        sbContent.Replace("~~SiteState~~", Me.SiteState)
        sbContent.Replace("~~SiteCountry~~", Me.SiteCountry)
        Return True
    End Function

#End Region
#Region "Private Methods"
    Private Function updateLinkCategoryList() As Boolean
        For Each LinkCategory As wpmPartGroup In LinkCategoryList
            LinkCategory.LinkCount = GetLinkCountByCategory(LinkCategory.ID)
        Next
        Return True
    End Function
    Private Function GetLinkCountByCategory(ByVal myLinkCategoryID As String) As Integer
        Dim iLinkCount As Integer = 0
        For Each linkrow As wpmPart In PartList
            If linkrow.LinkCategoryID = myLinkCategoryID Then
                iLinkCount = iLinkCount + 1
            End If
        Next
        Return iLinkCount
    End Function
    Private Function yuiBuildLineItemList(ByVal ParentPageID As String, ByVal ListClass As String, ByVal PageID As String) As String
        Dim sPageName As String = (String.Empty)
        Dim sReturn As New StringBuilder(String.Empty)
        For Each myrow As wpmLocation In LocationList
            If myrow.RecordSource = "Page" Or myrow.RecordSource = "Category" Then
                If myrow.ParentPageID = ParentPageID Then
                    If myrow.PageID = PageID Then
                        sReturn.Append("<li class=""yuimenuitem selected"">")
                        sReturn.Append(BuildClassLink(myrow, "yuimenuitemlabel selected"))
                        sReturn.Append("</li>" & vbCrLf)
                    Else
                        sReturn.Append("<li class=""yuimenuitem "">")
                        sReturn.Append(BuildClassLink(myrow, "yuimenuitemlabel"))
                        sReturn.Append("</li>" & vbCrLf)
                    End If
                End If
                If myrow.PageID = ParentPageID Then
                    sPageName = myrow.PageName
                End If
            End If
        Next
        If sReturn.Length > 0 Then
            sReturn.Insert(0, "<h6 class=""" & ListClass & """>" & sPageName & "</h6><ul class=""" & ListClass & """>")
            sReturn.Append("</ul>")
        End If
        Return sReturn.ToString
    End Function
    Private Function isPageParent(ByVal PageID As String, ByVal ParentPageID As String, ByRef myParentPageID As String) As Boolean
        Dim bReturn As Boolean = True
        For Each myrow As wpmLocation In LocationList
            If myrow.PageID = PageID Then
                myParentPageID = myrow.ParentPageID
                If myrow.ParentPageID <> ParentPageID Then
                    bReturn = False
                End If
                Exit For
            End If
        Next
        Return bReturn
    End Function
    Private Function BuildClassLink(ByRef myRow As wpmLocation, ByVal LinkClass As String) As String
        If LinkClass = String.Empty Then
            Return ("<a href=""" & myRow.GetSiteMapRowURL(UseBreadCrumbURL) & """><span>" & myRow.PageName & "</span></a>")
        Else
            Return ("<a class=""" & LinkClass & """ href=""" & myRow.GetSiteMapRowURL(UseBreadCrumbURL) & """><span>" & myRow.PageName & "</span></a>")
        End If
    End Function
    Private Function BuildNaviagtionLink(ByRef myRow As wpmLocation, ByVal IsSelected As Boolean) As String
        If (IsSelected) Then
            Return BuildClassLink(myRow, "selected")
        Else
            Return BuildClassLink(myRow, "")
        End If
    End Function
    Private Function BuildMenuTree(ByRef arrPageID() As String, ByVal pageID As String, ByRef LevelCount As Integer) As Boolean
        Dim iLevel As Integer = 0
        iLevel = UBound(arrPageID)
        If LocationList.Count = 0 Then
            Return False
        Else
            For Each myrow As wpmLocation In LocationList
                If pageID = myrow.PageID Then
                    arrPageID(LevelCount) = myrow.PageID
                    LevelCount = LevelCount + 1
                    If (CInt(myrow.ParentPageID) = 0) Then
                        Call BuildMenuTree(arrPageID, myrow.ParentPageID, LevelCount)
                    End If
                    Exit For
                End If
            Next
            Return True
        End If
    End Function
    Private Function GetCompanyValues(ByVal ReqCompanyID As String) As Boolean
        If CInt(ReqCompanyID) > 0 Then
            For Each myrow As DataRow In wpmDataCon.GetCompanyData(ReqCompanyID).Rows
                Me.CompanyName = wpmUTIL.GetDBString(myrow("CompanyName"))
                Me.SiteGallery = wpmUTIL.GetDBString(myrow("GalleryFolder"))
                Me.SiteURL = wpmUTIL.GetDBString(myrow("SiteURL"))
                Me.SiteTitle = wpmUTIL.GetDBString(myrow("SiteTitle"))
                Me.SiteKeywords = wpmUTIL.GetDBString(myrow("DefaultPaymentTerms"))
                Me.SiteDescription = wpmUTIL.GetDBString(myrow("DefaultInvoiceDescription"))
                Me.SitePrefix = wpmUTIL.GetDBString(myrow("SiteTemplate"))
                Me.DefaultArticleID = wpmUTIL.GetDBString(myrow("DefaultArticleID"))
                Me.SiteHomePageID = wpmUTIL.GetDBString(myrow("HomePageID"))
                Me.DefaultSitePrefix = wpmUTIL.GetDBString(myrow("DefaultSiteTemplate"))
                Me.UseBreadCrumbURL = wpmUTIL.GetDBBoolean(myrow("UseBreadCrumbURL"))
                Me.SiteCity = wpmUTIL.GetDBString(myrow("City"))
                Me.SiteState = wpmUTIL.GetDBString(myrow("StateOrProvince"))
                Me.SiteCountry = wpmUTIL.GetDBString(myrow("Country"))
                Me.FromEmail = wpmUTIL.GetDBString(myrow("FromEmail"))
                Me.SMTP = wpmUTIL.GetDBString(myrow("SMTP"))
                Me.Component = wpmUTIL.GetDBString(myrow("Component"))
                Me.SiteCategoryTypeID = wpmUTIL.GetDBString(myrow("SiteCategoryTypeID"))
                Me.DefaultSiteCategoryID = wpmUTIL.GetDBString(myrow("DefaultSiteCategoryID"))
                If Me.SiteHomePageID = String.Empty And Me.DefaultSiteCategoryID <> String.Empty Then
                    Me.SiteHomePageID = "CAT-" & Me.DefaultSiteCategoryID
                End If

                Exit For
            Next
            Return True
        Else
            Return False
        End If
    End Function
    Private Function SaveSiteMapFile(ByRef SiteMapFilePath As String) As Boolean
        SiteMapFilePath = Replace(SiteMapFilePath, "\\", "\")
        Dim bReturn As Boolean = True
        Try
            Using sw As New System.IO.StreamWriter(SiteMapFilePath, False)
                Try
                    Dim ViewWriter As New System.Xml.Serialization.XmlSerializer(GetType(wpmSiteProfile))
                    ViewWriter.Serialize(sw, Me)
                Catch ex As Exception
                    wpmLog.AuditLog("Error Saving File (" & SiteMapFilePath & ") - " & ex.ToString, "wpmSiteFile.SaveSiteMapFile")
                    bReturn = False
                Finally
                    sw.Flush()
                    sw.Close()
                End Try
            End Using
        Catch ex As Exception
            wpmLog.AuditLog("Error Before Saving File (" & SiteMapFilePath & ") - " & ex.ToString, "wpmSiteFile.SaveSiteMapFile")
            bReturn = False
        End Try
        Return bReturn
    End Function

#End Region

End Class
