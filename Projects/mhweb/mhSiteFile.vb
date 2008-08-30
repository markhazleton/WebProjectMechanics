Public Class mhSiteFile
    Public ImageRows As mhImageRows
    Public ArticleRows As mhArticleRows
    Public PageAliasRows As mhPageAliasRows

    Private _SiteMapRows As New mhSiteMapRows
    Public Property SiteMapRows() As mhSiteMapRows
        Get
            Return _SiteMapRows
        End Get
        Set(ByVal value As mhSiteMapRows)
            _SiteMapRows = value
        End Set
    End Property
    Private _LinkCategoryList As New mhLinkCategoryList
    Public Property LinkCategoryList() As mhLinkCategoryList
        Get
            Return _LinkCategoryList
        End Get
        Set(ByVal value As mhLinkCategoryList)
            _LinkCategoryList = value
        End Set
    End Property
    Private _SiteLinkRows As New mhSiteLinkRows
    Public Property SiteLinkRows() As mhSiteLinkRows
        Get
            Return _SiteLinkRows
        End Get
        Set(ByVal value As mhSiteLinkRows)
            _SiteLinkRows = value
        End Set
    End Property
    Private _SiteGroupRows As New mhSiteGroupList
    Public Property SiteGroupRows() As mhSiteGroupList
        Get
            Return _SiteGroupRows
        End Get
        Set(ByVal value As mhSiteGroupList)
            _SiteGroupRows = value
        End Set
    End Property
    Private _SiteParameterList As New mhSiteParameterList
    Public Property SiteParameterList() As mhSiteParameterList
        Get
            Return _SiteParameterList
        End Get
        Set(ByVal value As mhSiteParameterList)
            _SiteParameterList = value
        End Set
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
    Private _SingleSiteGallery As Boolean
    Public Property SingleSiteGallery() As Boolean
        Get
            Return _SingleSiteGallery
        End Get
        Set(ByVal value As Boolean)
            _SingleSiteGallery = value
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


    Public Sub LoadMe(ByVal CompanyID As String, ByVal GroupID As String, ByVal SiteDB As String, ByVal OrderBy As String)
        If Trim(OrderBy) = "" Then
            OrderBy = "ORDER"
        End If
        GetCompanyValues(CompanyID, SiteDB)
        SiteMapRows.PopulateSiteMapCol(OrderBy, CompanyID, GroupID, Me)
        SiteMapRows.BuildBreadcrumbRows(CompanyName)
        If Me.SiteCategoryTypeID = "" Then
            SiteLinkRows.PopulateSiteLinkRows(CompanyID)
        Else
            SiteLinkRows.PopulateSiteCategoryLinkRows(CompanyID, Me.SiteCategoryTypeID)
        End If
        LinkCategoryList.PopulateLinkCategoryList(CompanyID)
        updateLinkCategoryList()
        SiteGroupRows.PopulateSiteGroupRows(CompanyID)
        SiteParameterList.PopulateParameterTypeList(CompanyID)
        If mhUser.IsAdmin Then
            ArticleRows = New mhArticleRows(CompanyID)
            ImageRows = New mhImageRows(CompanyID)
        End If
        PageAliasRows = New mhPageAliasRows(CompanyID)
        SaveSiteMapFile(mhConfig.mhWebConfigFolder & "\index\" & Me.CompanyName & "-site-file.xml")

    End Sub
    Private Function updateLinkCategoryList() As Boolean
        For Each LinkCategory As mhLinkCategory In LinkCategoryList
            LinkCategory.LinkCount = GetLinkCountByCategory(LinkCategory.ID)
        Next
        Return True
    End Function
    Private Function GetLinkCountByCategory(ByVal myLinkCategoryID As String) As Integer
        Dim iLinkCount As Integer = 0
        For Each linkrow As mhSiteLinkRow In SiteLinkRows
            If linkrow.LinkCategoryID = myLinkCategoryID Then
                iLinkCount = iLinkCount + 1
            End If
        Next
        Return iLinkCount
    End Function
    Public Function ProcessCategories() As Boolean
        For Each myRow As mhSiteMapRow In Me.SiteMapRows
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
        Dim sPageURL As String = ("")
        For Each myrow As mhSiteMapRow In SiteMapRows
            If UseBreadCrumbURL Then
                sPageURL = myrow.BreadCrumbURL
            Else
                sPageURL = myrow.DisplayURL
            End If
            If myrow.RecordSource = "Article" Then
                If myrow.PageID = PageID Then
                    If myrow.ArticleID = ArticleID Then
                        sReturn.Append("<li class=""selected"">")
                        sReturn.Append(BuildNaviagtionLink(sPageURL, myrow.PageName, myrow.PageDescription, True, myrow.PageID))
                    Else
                        sReturn.Append("<li>")
                        sReturn.Append(BuildNaviagtionLink(sPageURL, myrow.PageName, myrow.PageDescription, False, myrow.PageID))
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
        Dim sPageURL As String = (String.Empty)
        For Each myrow As mhSiteMapRow In SiteMapRows
            If myrow.RecordSource = "Page" Or myrow.RecordSource = "Category" Then
                If UseBreadCrumbURL Then
                    sPageURL = myrow.BreadCrumbURL
                Else
                    sPageURL = myrow.DisplayURL
                End If
                If (myrow.ParentPageID = ParentPageID And ParentPageID <> String.Empty) Then
                    If myrow.PageID = PageID Then
                        sReturn.Append("<li class=""menuitem selected"">")
                        sReturn.Append(BuildNaviagtionLink(sPageURL, myrow.PageName, myrow.PageDescription, True, myrow.PageID))
                    Else
                        sReturn.Append("<li class=""menuitem"">")
                        sReturn.Append(BuildNaviagtionLink(sPageURL, myrow.PageName, myrow.PageDescription, False, myrow.PageID))
                    End If
                    sReturn.Append("</li>" & vbCrLf)
                End If
                If (myrow.PageID = ParentPageID) Then
                    sParentPageName = myrow.PageName
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
        Dim sPageURL As String = String.Empty

        If ParentPageID = String.Empty And SiteCategoryID = String.Empty Then
            sReturn.Append("<li><strong>NO Parent, No Category</strong></li>")
        Else
            For Each myrow As mhSiteMapRow In SiteMapRows
                If myrow.RecordSource = "Page" Or myrow.RecordSource = "Category" Then
                    If UseBreadCrumbURL Then
                        sPageURL = myrow.BreadCrumbURL
                    Else
                        sPageURL = myrow.DisplayURL
                    End If
                    If (myrow.ParentPageID = ParentPageID) Then
                        If myrow.PageID = PageID Then
                            sReturn.Append("<li class=""menuitem selected"">")
                            sReturn.Append(BuildNaviagtionLink(sPageURL, myrow.PageName, myrow.PageDescription, True, myrow.PageID))
                        Else
                            sReturn.Append("<li class=""menuitem"">")
                            sReturn.Append(BuildNaviagtionLink(sPageURL, myrow.PageName, myrow.PageDescription, False, myrow.PageID))
                        End If
                        sReturn.Append("</li>" & vbCrLf)
                    End If
                    If (myrow.PageID = PageID) Then
                        sParentPageName = myrow.PageName
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

    Public Function BuildMenuChild(ByVal PageID As String, ByVal ParentPageID As String, ByVal DefaultParentPageID As String) As String
        Dim sReturn As New StringBuilder("")
        Dim sPageURL As String = ("")
        For Each myrow As mhSiteMapRow In SiteMapRows
            If UseBreadCrumbURL Then
                sPageURL = myrow.BreadCrumbURL
            Else
                sPageURL = myrow.DisplayURL
            End If
            If myrow.RecordSource = "Page" Then
                If myrow.ParentPageID = ParentPageID Then
                    If myrow.PageID = PageID Then
                        sReturn.Append("<li class=""selected"">")
                        sReturn.Append(BuildNaviagtionLink(sPageURL, myrow.PageName, myrow.PageDescription, True, myrow.PageID))
                    Else
                        sReturn.Append("<li>")
                        sReturn.Append(BuildNaviagtionLink(sPageURL, myrow.PageName, myrow.PageDescription, False, myrow.PageID))
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
    Private Function isPageParent(ByVal PageID As String, ByVal ParentPageID As String, ByRef myParentPageID As String) As Boolean
        Dim bReturn As Boolean = True
        For Each myrow As mhSiteMapRow In SiteMapRows
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
    Private Function yuiBuildLineItemList(ByVal ParentPageID As String, ByVal ListClass As String, ByVal PageID As String) As String
        Dim sPageURL As String = (String.Empty)
        Dim sPageName As String = (String.Empty)
        Dim sReturn As New StringBuilder(String.Empty)
        For Each myrow As mhSiteMapRow In SiteMapRows
            If UseBreadCrumbURL Then
                sPageURL = myrow.BreadCrumbURL
            Else
                sPageURL = myrow.DisplayURL
            End If
            If myrow.RecordSource = "Page" Or myrow.RecordSource = "Category" Then
                If myrow.ParentPageID = ParentPageID Then
                    If myrow.PageID = PageID Then
                        sReturn.Append("<li class=""yuimenuitem selected"">")
                        sReturn.Append(BuildClassLink(sPageURL, myrow.PageName, myrow.PageDescription, "yuimenuitemlabel selected"))
                        sReturn.Append("</li>" & vbCrLf)
                    Else
                        sReturn.Append("<li class=""yuimenuitem "">")
                        sReturn.Append(BuildClassLink(sPageURL, myrow.PageName, myrow.PageDescription, "yuimenuitemlabel"))
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
    Public Function yuiBuildPageList(ByVal ParentPageID As String, ByVal GroupName As String, ByVal PageID As String, ByVal Level As Integer) As String
        Dim sPageURL As String = (String.Empty)
        Dim sPageName As String = (String.Empty)
        Dim sReturn As New StringBuilder(String.Empty)
        If ParentPageID <> String.Empty Then
            For Each myrow As mhSiteMapRow In SiteMapRows
                If UseBreadCrumbURL Then
                    sPageURL = myrow.BreadCrumbURL
                Else
                    sPageURL = myrow.DisplayURL
                End If
                If (myrow.RecordSource = "Category" Or myrow.RecordSource = "Page") Then
                    If (myrow.ParentPageID = ParentPageID) Then
                        If myrow.PageID = PageID Then
                            sReturn.Append("<li class=""yuimenuitem selected"">")
                            sReturn.Append(BuildClassLink(sPageURL, myrow.PageName, myrow.PageDescription, "yuimenuitemlabel selected"))
                        Else
                            sReturn.Append("<li class=""yuimenuitem "">")
                            sReturn.Append(BuildClassLink(sPageURL, myrow.PageName, myrow.PageDescription, "yuimenuitemlabel"))
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
    Public Function BuildSiteCategoryGroupList(ByVal ParentPageID As String, ByVal GroupName As String, ByVal PageID As String, ByVal Level As Integer, ByVal GroupDescription As String) As String
        Dim sPageURL As String = ("")
        Dim sPageName As String = ("")
        Dim sReturn As New StringBuilder("")
        For Each myrow As mhSiteMapRow In SiteMapRows
            If UseBreadCrumbURL Then
                sPageURL = myrow.BreadCrumbURL
            Else
                sPageURL = myrow.DisplayURL
            End If
            If (myrow.SiteCategoryGroupName = GroupName) Then
                If (myrow.ParentPageID = ParentPageID) Or (Level = 1 And myrow.ParentPageID = "") Then
                    If myrow.PageID = PageID Then
                        sReturn.Append("<li class=""menuitem selected"">")
                        sReturn.Append(BuildClassLink(sPageURL, myrow.PageName, myrow.PageDescription, "menuitemlabel selected"))
                    Else
                        sReturn.Append("<li class=""menuitem "">")
                        sReturn.Append(BuildClassLink(sPageURL, myrow.PageName, myrow.PageDescription, "menuitemlabel"))
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


    Public Function yuiBuildSiteCategoryGroupList(ByVal ParentPageID As String, ByVal GroupName As String, ByVal PageID As String, ByVal Level As Integer) As String
        Dim sPageURL As String = ("")
        Dim sPageName As String = ("")
        Dim sReturn As New StringBuilder("")
        For Each myrow As mhSiteMapRow In SiteMapRows
            If UseBreadCrumbURL Then
                sPageURL = myrow.BreadCrumbURL
            Else
                sPageURL = myrow.DisplayURL
            End If
            If (myrow.SiteCategoryGroupName = GroupName) Then
                If (myrow.ParentPageID = ParentPageID) Or (Level = 1 And myrow.ParentPageID = "") Then
                    If myrow.PageID = PageID Then
                        sReturn.Append("<li class=""yuimenuitem selected"">")
                        sReturn.Append(BuildClassLink(sPageURL, myrow.PageName, myrow.PageDescription, "yuimenuitemlabel selected"))
                    Else
                        sReturn.Append("<li class=""yuimenuitem "">")
                        sReturn.Append(BuildClassLink(sPageURL, myrow.PageName, myrow.PageDescription, "yuimenuitemlabel"))
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
        Dim sPageURL As String = ("")
        Dim sPageName As String = ("")
        Dim sReturn As New StringBuilder("")
        For Each myrow As mhSiteMapRow In SiteMapRows
            If UseBreadCrumbURL Then
                sPageURL = myrow.BreadCrumbURL
            Else
                sPageURL = myrow.DisplayURL
            End If
            If (myrow.RecordSource = "Category" And myrow.SiteCategoryGroupName = GroupName) Then
                If myrow.ParentPageID = ParentPageID Then
                    If myrow.PageID = PageID Then
                        sReturn.Append("<li class=""yuimenubaritem  selected"">")
                        sReturn.Append(BuildClassLink(sPageURL, myrow.PageName, myrow.PageDescription, "yuimenubaritemlabel selected"))
                    Else
                        sReturn.Append("<li class=""yuimenubaritem  "">")
                        sReturn.Append(BuildClassLink(sPageURL, myrow.PageName, myrow.PageDescription, "yuimenubaritemlabel"))
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
                sReturn.Append(BuildClassLink(sPageURL, myrow.PageName, myrow.PageDescription, "yuimenubaritemlabel"))
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
    Private Function BuildClassLink(ByVal LinkURL As String, ByVal LinkText As String, ByVal LinkTitle As String, ByVal LinkClass As String) As String
        Dim sLinkText As String = ("")
        sLinkText = ("<a class=""" & LinkClass & """ href=""" & LinkURL & """>" & LinkText & "</a>")
        Return sLinkText
    End Function
    Public Function yuiBuildPageTree(ByVal ParentID As String, ByVal intLevel As Double, ByVal PageName As String) As String
        Dim sReturn As New StringBuilder("")
        For Each myRow As mhSiteMapRow In SiteMapRows
            If myRow.RecordSource = "Page" Then
                If (ParentID = myRow.ParentPageID) Then
                    If intLevel = 0 Then
                        PageName = myRow.PageName
                        sReturn.Append("<li class=""yuimenubaritem"">")
                        sReturn.Append(BuildClassLink(myRow.BreadCrumbURL, myRow.PageName, myRow.PageDescription, "yuimenubaritemlabel"))
                        sReturn.Append(yuiBuildPageTree(myRow.PageID, intLevel + 1, myRow.PageName) & "</li>" & vbCrLf)
                    Else
                        sReturn.Append("<li class=""yuimenuitem"">")
                        sReturn.Append(BuildClassLink(myRow.BreadCrumbURL, myRow.PageName, myRow.PageDescription, "yuimenuitemlabel"))
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
    Private Function BuildNaviagtionLink(ByVal LinkURL As String, ByVal LinkText As String, ByVal LinkTitle As String, ByVal IsSelected As Boolean, ByVal pageID As String) As String
        If (IsSelected) Then
            Return ("<a class=""selected"" href=""" & LinkURL & """><span class=""selected"">" & LinkText & "</span></a>")
        Else
            Return ("<a href=""" & LinkURL & """><span>" & LinkText & "</span></a>")
        End If
    End Function
    Public Function BuildPageTree(ByVal ParentID As String, ByVal intLevel As Double, ByVal sULClassName As String) As String
        Dim sReturn As New StringBuilder("")
        For Each myRow As mhSiteMapRow In SiteMapRows
            If myRow.RecordSource = "Page" Then
                If ParentID = myRow.ParentPageID Then
                    sReturn.Append("<li>")
                    sReturn.Append(BuildNaviagtionLink(myRow.BreadCrumbURL, myRow.PageName, myRow.PageDescription, False, myRow.PageID))
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
    Private Function BuildMenuTree(ByRef arrPageID() As String, ByVal pageID As String, ByRef LevelCount As Integer) As Boolean
        Dim iLevel As Integer = 0
        iLevel = UBound(arrPageID)
        If SiteMapRows.Count = 0 Then
            Return False
        Else
            For Each myrow As mhSiteMapRow In SiteMapRows
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
    Public Function BuildLinkMenu(ByVal RequestMenuLevel As Integer, ByVal DefaultParentPageID As String, ByVal myReturnTemplate As String, ByRef CurrentMapRow As mhSiteMapRow) As String
        Dim ReturnString As String = ("")
        Dim PageID As String = CurrentMapRow.PageID
        Dim ParentPageID As String = CurrentMapRow.PageID
        Dim ParentPageName As String = ""
        If RequestMenuLevel <= CurrentMapRow.LevelNBR Then
            For Each myrow As mhBreadCrumbRow In CurrentMapRow.BreadCrumbRows
                If RequestMenuLevel = (myrow.MenuLevelNBR - 1) Then
                    PageID = myrow.PageID
                    ParentPageID = myrow.ParentPageID
                    Exit For
                End If
                If RequestMenuLevel = myrow.MenuLevelNBR Then
                    PageID = myrow.PageID
                    ParentPageID = myrow.ParentPageID
                    Exit For
                End If
            Next
            For Each myrow As mhBreadCrumbRow In CurrentMapRow.BreadCrumbRows
                If myrow.PageID = ParentPageID Then
                    ParentPageName = myrow.PageName
                    Exit For
                End If
            Next
            If DefaultParentPageID = "" Then
                DefaultParentPageID = ParentPageID
            End If
            ReturnString = BuildMenuChild(PageID, ParentPageID, DefaultParentPageID)
        ElseIf RequestMenuLevel = CurrentMapRow.LevelNBR + 1 Then
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
    Public Function GetCompanyValues(ByVal ReqCompanyID As String, ByVal SiteDB As String) As Boolean
        Dim strSQL As String
        If CInt(ReqCompanyID) > 0 Then
            strSQL = ("SELECT Company.CompanyID,  " & _
                            "Company.CompanyName,  " & _
                            "Company.GalleryFolder,  " & _
                            "Company.SiteURL,  " & _
                            "Company.SiteTitle,  " & _
                            "Company.SiteTemplate,  " & _
                            "Company.DefaultSiteTemplate,  " & _
                            "Company.HomePageID,  " & _
                            "Company.DefaultArticleID,  " & _
                            "Company.ActiveFL,  " & _
                            "Company.UseBreadCrumbURL,  " & _
                            "Company.SiteCategoryTypeID,  " & _
                            "Company.SingleSiteGallery,  " & _
                            "Company.DefaultPaymentTerms,  " & _
                            "Company.DefaultInvoiceDescription,  " & _
                            "Company.City,  " & _
                            "Company.StateOrProvince,  " & _
                            "Company.PostalCode,  " & _
                            "Company.Country,  " & _
                            "Company.FromEmail, " & _
                            "Company.SMTP, " & _
                            "Company.Component, " & _
                            "SiteCategoryType.SiteCategoryTypeNM,  " & _
                            "SiteCategoryType.SiteCategoryTypeDS,  " & _
                            "SiteCategoryType.DefaultSiteCategoryID   " & _
                            "FROM SiteCategoryType " & _
                            "RIGHT JOIN Company ON SiteCategoryType.[SiteCategoryTypeID] = Company.[SiteCategoryTypeID] " & _
                           "WHERE Company.CompanyID=" & ReqCompanyID & " ")

            Dim mydt As System.Data.DataTable = mhDB.GetDataTable(strSQL, "GetCompanyValues")
            For Each myrow As DataRow In mydt.Rows
                Me.CompanyName = mhUTIL.GetDBString(myrow("CompanyName"))
                Me.SiteGallery = mhUTIL.GetDBString(myrow("GalleryFolder"))
                Me.SiteURL = mhUTIL.GetDBString(myrow("SiteURL"))
                Me.SiteTitle = mhUTIL.GetDBString(myrow("SiteTitle"))
                Me.SiteKeywords = mhUTIL.GetDBString(myrow("DefaultPaymentTerms"))
                Me.SiteDescription = mhUTIL.GetDBString(myrow("DefaultInvoiceDescription"))
                Me.SitePrefix = mhUTIL.GetDBString(myrow("SiteTemplate"))
                Me.DefaultArticleID = mhUTIL.GetDBString(myrow("DefaultArticleID"))
                Me.SiteHomePageID = mhUTIL.GetDBString(myrow("HomePageID"))
                Me.DefaultSitePrefix = mhUTIL.GetDBString(myrow("DefaultSiteTemplate"))
                Me.UseBreadCrumbURL = mhUTIL.GetDBBoolean(myrow("UseBreadCrumbURL"))
                Me.SingleSiteGallery = mhUTIL.GetDBBoolean(myrow("SingleSiteGallery"))
                Me.SiteCity = mhUTIL.GetDBString(myrow("City"))
                Me.SiteState = mhUTIL.GetDBString(myrow("StateOrProvince"))
                Me.SiteCountry = mhUTIL.GetDBString(myrow("Country"))
                Me.FromEmail = mhUTIL.GetDBString(myrow("FromEmail"))
                Me.SMTP = mhUTIL.GetDBString(myrow("SMTP"))
                Me.Component = mhUTIL.GetDBString(myrow("Component"))
                Me.SiteCategoryTypeID = mhUTIL.GetDBString(myrow("SiteCategoryTypeID"))
                Me.DefaultSiteCategoryID = mhUTIL.GetDBString(myrow("DefaultSiteCategoryID"))
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
        Dim bReturn As Boolean = True
        Try
            Using sw As New System.IO.StreamWriter(SiteMapFilePath, False)
                Try
                    Dim ViewWriter As New System.Xml.Serialization.XmlSerializer(GetType(mhSiteFile))
                    ViewWriter.Serialize(sw, Me)
                Catch ex As Exception
                    mhUTIL.AuditLog("Error Saving File (" & SiteMapFilePath & ") - " & ex.ToString, "mhSiteFile.SaveSiteMapFile")
                    bReturn = False
                Finally
                    sw.Flush()
                    sw.Close()
                End Try
            End Using
        Catch ex As Exception
            mhUTIL.AuditLog("Error Before Saving File (" & SiteMapFilePath & ") - " & ex.ToString, "mhSiteFile.SaveSiteMapFile")
            bReturn = False
        End Try
        Return bReturn
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
        sbContent.Replace("~~SiteTagLine~~", Me.SiteDescription)
        sbContent.Replace("<sitemap>", Me.PageStructure)
        sbContent.Replace("<sitetree>", Me.TreeHTML)
        sbContent.Replace("~~SiteCity~~", Me.SiteCity)
        sbContent.Replace("~~sitecity~~", Me.SiteCity)
        sbContent.Replace("~~SiteCityDash~~", mhUTIL.FormatPageNameForURL(Me.SiteCity))
        sbContent.Replace("~~SiteCityNoSpace~~", Replace(Me.SiteCity, " ", ""))
        sbContent.Replace("~~SiteState~~", Me.SiteState)
        sbContent.Replace("~~SiteCountry~~", Me.SiteCountry)
        Return True
    End Function
    Private Function GetSiteMapName(ByVal CompanyID As String, ByVal GroupID As String, ByVal OrderBy As String) As String
        Dim mystring As String = Me.CompanyName
        Return (Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", "") & " - " & CompanyID & " - " & GroupID & " - " & OrderBy)
    End Function
    Public Shared Function GetSiteFile(ByVal CompanyID As String, ByVal GroupID As String, ByVal SiteDB As String, ByVal OrderBy As String) As mhSiteFile
        Dim mySiteFile As New mhSiteFile
        Dim SiteMapName As String = mySiteFile.GetSiteMapName(CompanyID, GroupID, OrderBy)
        If mhUser.IsAdmin() Then
            HttpContext.Current.Application((Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", "") & " - " & CompanyID & " - " & "1" & " - " & OrderBy)) = Nothing
            HttpContext.Current.Application((Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", "") & " - " & CompanyID & " - " & "2" & " - " & OrderBy)) = Nothing
            HttpContext.Current.Application((Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", "") & " - " & CompanyID & " - " & "4" & " - " & OrderBy)) = Nothing
        End If
        If mhConfig.CacheState Then
            If HttpContext.Current.Application(SiteMapName) Is Nothing Then
                mySiteFile.LoadMe(CompanyID, GroupID, SiteDB, OrderBy)
                Try
                    HttpContext.Current.Application(SiteMapName) = mySiteFile
                Catch ex As Exception
                    mhUTIL.AuditLog("Error When updating Application variable (" & SiteMapName & ") - " & ex.ToString, "mhSiteFile.GetSiteFile")
                End Try
            Else
                Try
                    mySiteFile = CType(HttpContext.Current.Application(SiteMapName), mhSiteFile)
                Catch ex As Exception
                    mhUTIL.AuditLog("Error when reading Application variable (" & SiteMapName & ") - " & ex.ToString, "mhSiteFile.GetSiteFile")
                End Try
            End If
        Else
            mySiteFile.LoadMe(CompanyID, GroupID, SiteDB, OrderBy)
        End If
        Return mySiteFile
    End Function
End Class
