Imports System.Xml.Serialization
Imports System.Text
Imports System.Linq
Imports System.Web


<XmlRootAttribute("Company", _
 Namespace:="http://projectmechanics.com", IsNullable:=False)> _
Public Class Company
    Private Const STR_App_Datasites As String = "/App_Data/sites"
    Public Property CompanyID() As String
    Public Property CompanyNM() As String
    Public Property DomainName() As String
    Public Property CompanyTitle() As String
    Public Property CompanyKeywords() As String
    Public Property CompanyDescription() As String
    Public Property HomeLocationID() As String
    Public Property DefaultArticleID() As Integer
    Public Property SitePrefix() As String
    Public Property DefaultSitePrefix() As String
    Public Property SiteGallery() As String
    Public Property SiteConfig() As String
    Public Property SiteCity As String
    Public Property SiteState As String
    Public Property SiteCountry As String
    Public Property PostalCode As String
    Public Property SiteType As new SiteType
    Public Property SiteCategoryTypeID() As String
    Public Property SiteCategoryTypeNM() As String 
    Public Property DefaultSiteCategoryID() As String
    Public Property UseBreadCrumbURL() As Boolean
    Public Property FromEmail() As String
    Public Property SMTP() As String
    Public Property Component() As String
    Public ReadOnly Property CompanyURL As String
        Get
            Return String.Format("http://{0}", DomainName)
        End Get
    End Property
    Public ReadOnly Property Locations() As LocationList
        Get
            Return _Locations
        End Get
    End Property
    Public ReadOnly Property Parts() As PartList
        Get
            Return _Parts
        End Get
    End Property
    Public ReadOnly Property Parameters() As ParameterList
        Get
            Return _Parameters
        End Get
    End Property
    Public ReadOnly Property Articles() As ArticleList
        Get
            Return _Articles
        End Get
    End Property
    Public ReadOnly Property PartCategories() As PartCategoryList
        Get
            Return _PartCategories
        End Get
    End Property
    Public ReadOnly Property LocationGroups() As LocationGroupList
        Get
            Return _LocationGroups
        End Get
    End Property
    Public ReadOnly Property Images() As CompanyImageList
        Get
            Return _Images
        End Get
    End Property
    Public ReadOnly Property LocationAliases() As LocationAliasList
        Get
            Return _LocationAliases
        End Get
    End Property
    Public ReadOnly Property IsValidCompany() As Boolean
        Get
            If IsNumeric(CompanyID) AndAlso Not (DomainName = String.Empty) Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property

    Public Sub New()
        CompanyID = String.Empty
        DomainName = String.Empty
    End Sub


#Region "Class Members"
    Private ReadOnly _Locations As New LocationList()
    Private ReadOnly _Parts As New PartList()
    Private ReadOnly _Parameters As New ParameterList()
    Private ReadOnly _PartCategories As New PartCategoryList()
    Private ReadOnly _LocationGroups As New LocationGroupList()
    Private ReadOnly _Images As New CompanyImageList()
    Private ReadOnly _LocationAliases As New LocationAliasList()
    Private ReadOnly _Articles As New ArticleList()

#End Region

#Region "Public Methods"
    Public Function GetCompanyFromDB(ByVal reqCompanyID As String, ByVal GroupID As String, ByVal OrderBy As String) As Boolean
        CompanyID = reqCompanyID
        Dim bReturn As Boolean = GetCompanyValues(CompanyID)
        If Trim(OrderBy) = "" Then
            OrderBy = "ORDER"
        End If
        If bReturn Then
            bReturn = Locations.GetLocationListFromDb(OrderBy, CompanyID, GroupID)
            Locations.BuildBreadCrumbRows()
            If SiteCategoryTypeID = "" Then
                Parts.GetCompanyParts(CompanyID)
            Else
                Parts.GetCompanyCategoryParts(CompanyID, SiteCategoryTypeID)
            End If
            PartCategories.PopulateLinkCategoryList(CompanyID)
            updatePartCategories()
            LocationGroups.PopulateSiteGroupList(CompanyID)
            Parameters.PopulateParameterTypeList(CompanyID, SiteCategoryTypeID)
            Images.GetCompanyImages(CompanyID)
            LocationAliases.GetCompanyLocationAliases(CompanyID)
            Articles.PopulateCompanyArticleList(CompanyID)
            If wpm_SiteConfig.FullLoggingOn() Or wpm_IsAdmin Then
                SaveCompanyProfileXML()
            End If
        End If
        Return bReturn
    End Function
    Public Function ProcessCategories() As Boolean
        For Each myRow As Location In Locations
            ReplaceTags(myRow.LocationDescription)
            ReplaceTags(myRow.LocationName)
            ReplaceTags(myRow.LocationKeywords)
            ReplaceTags(myRow.LocationTitle)
            ReplaceTags(myRow.LocationURL)
            ReplaceTags(myRow.BreadCrumbURL)
        Next
        Return True
    End Function
    Public Function BuildPageArticle(ByRef myLocation As Location) As String
        Dim sReturn As New StringBuilder("")
        For Each myrow As Location In Locations
            If myLocation.RecordSource = "Article" Then
                If myrow.RecordSource = "Article" Then
                    If myrow.ParentLocationID = myLocation.ParentLocationID Then
                        If myrow.ArticleID = myLocation.ArticleID Then
                            sReturn.Append("<li class=""selected"">")
                            sReturn.Append(BuildNaviagtionLink(myrow, True))
                        Else
                            sReturn.Append("<li>")
                            sReturn.Append(BuildNaviagtionLink(myrow, False))
                        End If
                        sReturn.Append("</li>" & vbCrLf)
                    End If
                End If
            Else
                If myrow.RecordSource = "Article" Then
                    If myrow.ParentLocationID = myLocation.LocationID Then
                        If myrow.ArticleID = myLocation.ArticleID Then
                            sReturn.Append("<li class=""selected"">")
                            sReturn.Append(BuildNaviagtionLink(myrow, True))
                        Else
                            sReturn.Append("<li>")
                            sReturn.Append(BuildNaviagtionLink(myrow, False))
                        End If
                        sReturn.Append("</li>" & vbCrLf)
                    End If
                End If

            End If

        Next
        If sReturn.Length > 0 Then
            sReturn.Insert(0, "<ul>" & vbCrLf)
            sReturn.Append("</ul>" & vbCrLf)
        End If
        Return sReturn.ToString
    End Function
    Public Function BuildLinkListBySibling(ByVal LocationID As String, ByVal ParentLocationID As String) As String
        Dim sReturn As New StringBuilder(String.Empty)
        Dim sParentPageName As String = String.Empty
        For Each myrow As Location In Locations
            If (myrow.RecordSource = "Page") Or myrow.RecordSource = "Category" Then
                If (myrow.ParentLocationID = ParentLocationID And ParentLocationID <> String.Empty) Then
                    If myrow.LocationID = LocationID Then
                        sReturn.Append("<li class=""menuitem selected"">")
                        sReturn.Append(BuildNaviagtionLink(myrow, True))
                    Else
                        sReturn.Append("<li class=""menuitem"">")
                        sReturn.Append(BuildNaviagtionLink(myrow, False))
                    End If
                    sReturn.Append("</li>" & vbCrLf)
                End If
                If (myrow.LocationID = ParentLocationID) Then
                    sParentPageName = BuildNaviagtionLink(myrow, False)
                End If
            End If
        Next
        If sReturn.Length > 0 Then
            If String.IsNullOrWhiteSpace(sParentPageName) Then
                sReturn.Insert(0, String.Format("<div class=""menugroup""><ul class=""menu"">{0}", vbCrLf))
            Else
                sReturn.Insert(0, String.Format("<div class=""menugroup""><div class=""menugroupname"">{0}</div><ul class=""menu"">{1}", sParentPageName, vbCrLf))
            End If
            sReturn.Append("</ul></div>" & vbCrLf)
        End If
        Return sReturn.ToString
    End Function
    Public Function BuildLinkListByParent(ByVal LocationID As String, ByVal ParentLocationID As String, ByVal SiteCategoryID As String) As String
        Dim sReturn As New StringBuilder("")
        Dim sParentPageName As String = String.Empty

        If ParentLocationID = String.Empty And SiteCategoryID = String.Empty Then
            ' do nothing
        Else
            For Each myrow As Location In Locations
                If (myrow.RecordSource = "Page") Or myrow.RecordSource = "Category" Then
                    If (myrow.ParentLocationID = ParentLocationID) Then
                        If myrow.LocationID = LocationID Then
                            sReturn.Append("<li class=""menuitem selected"">")
                            sReturn.Append(BuildNaviagtionLink(myrow, True))
                        Else
                            sReturn.Append("<li class=""menuitem"">")
                            sReturn.Append(BuildNaviagtionLink(myrow, False))
                        End If
                        sReturn.Append("</li>" & vbCrLf)
                    End If
                    If (myrow.LocationID = LocationID) Then
                        ' sParentPageName = myrow.PageName
                        sParentPageName = BuildNaviagtionLink(myrow, False)
                    End If
                End If
            Next
            If sReturn.Length > 0 Then
                If String.IsNullOrWhiteSpace(sParentPageName) Then
                    sReturn.Insert(0, String.Format("<div class=""menugroup""><ul class=""menu"">{0}", vbCrLf))
                Else
                    sReturn.Insert(0, String.Format("<div class=""menugroup""><div class=""menugroupname"">{0}</div><ul class=""menu"">{1}", sParentPageName, vbCrLf))
                End If
                sReturn.Append("</ul></div>" & vbCrLf)
            End If
        End If

        Return sReturn.ToString
    End Function
    'This is a combination of a bread crumb menu with a children menu
    '<TODO> I imagine I should create subfunctions rather than copy/paste code
    Public Function BreadCrumbWithChildren(ByVal BreadCrumb As String, ByVal LocationID As String, ByVal ParentLocationID As String, ByVal SiteCategoryID As String) As String
        Dim sReturn As New StringBuilder("")
        Dim sParentPageName As String = String.Empty

        If ParentLocationID = String.Empty And SiteCategoryID = String.Empty Then
            sReturn.Append("<li><strong>NO Parent, No Category</strong></li>")
        Else
            For Each myrow As Location In Locations
                If (myrow.RecordSource = "Page") Or myrow.RecordSource = "Category" Then
                    If (myrow.ParentLocationID = ParentLocationID) Then
                        If myrow.LocationID = LocationID Then
                            sReturn.Append("<li class=""menuitem selected"">")
                            sReturn.Append(BuildNaviagtionLink(myrow, True))
                        Else
                            sReturn.Append("<li class=""menuitem"">")
                            sReturn.Append(BuildNaviagtionLink(myrow, False))
                        End If
                        sReturn.Append("</li>" & vbCrLf)
                    End If
                    If (myrow.LocationID = LocationID) Then
                        sParentPageName = BuildNaviagtionLink(myrow, False)
                    End If
                End If
            Next
        End If

        If BreadCrumb Is Nothing Then
            Return sReturn.ToString
        Else
            Return String.Format("{0}<ul>{1}</ul></ul>", BreadCrumb.Substring(0, BreadCrumb.LastIndexOf("</ul>")), sReturn)
        End If

    End Function

#Region "BootStrap Navigation"

    Public Function bootBuilNavBar(ByVal ParentLocationID As String, ByVal LocationID As String, ByVal Level As Integer) As String
        Dim sReturn As New StringBuilder(String.Empty)
        Dim ParentLocation As Location = Locations.FindLocation(ParentLocationID,0)

        Dim myMain = (From i In Locations.FindLocation(LocationID,0).LocationTrailList Where i.MenuLevelNBR=1 Select i).SingleOrDefault


        Dim ChildLocations As New List(Of Location)
        If ParentLocationID <> String.Empty Or Level = 0 Then
            For Each myrow As Location In Locations
                If (myrow.RecordSource.ToLower() = "category" Or (myrow.RecordSource.ToLower = "page")) Then
                    If (myrow.ParentLocationID = ParentLocationID) Then
                        ChildLocations = Locations.FindChildLocation(myrow.LocationID)
                        If ChildLocations.Count > 0 Then
                            If myrow.LocationID = myMain.LocationID Then
                                sReturn.Append("<li class=""dropdown-submenu active"" >")
                            ElseIf myrow.LocationID = LocationID Then
                                sReturn.Append("<li class=""dropdown-submenu"" >")
                            Else
                                sReturn.Append("<li class=""dropdown-submenu"" >")
                            End If
                            sReturn.Append(String.Format("<a href=""#"" class=""dropdown-toggle"" data-toggle=""dropdown"" role=""button"" aria-expanded=""false""  >{0}</a>", myrow.LocationName))
                            sReturn.Append(bootBuilNavBar(myrow.LocationID, LocationID, Level + 1))
                            sReturn.Append("</li>" & vbCrLf)
                        Else
                            If myrow.LocationID = myMain.LocationID Then
                                sReturn.Append("<li class=""active"" >")
                            ElseIf myrow.LocationID = LocationID Then
                                sReturn.Append("<li >")
                            Else
                                sReturn.Append("<li >")
                            End If
                            sReturn.Append(BuildClassLink(myrow, "", False))
                            sReturn.Append("</li>" & vbCrLf)
                        End If
                    End If
                End If
            Next
            If sReturn.Length > 0 Then
                If Level > 0 Then
                    sReturn.Append(vbCrLf)
                    sReturn.Insert(0, String.Format("<ul class=""dropdown-menu multi-level"" role=""menu""><li>{0}</li>", BuildClassLink(ParentLocation, "", False)))
                    sReturn.Append("</ul>")
                    sReturn.Append(vbCrLf)
                Else
                    sReturn.Insert(0, "<ul class=""nav navbar-nav"">")
                    sReturn.Append("</ul>")
                End If
            End If
        End If
        Return sReturn.ToString()
    End Function

#End Region


#Region "Foundation Navigation"

    Public Function fndBuilNavBar(ByVal ParentLocationID As String, ByVal LocationID As String, ByVal Level As Integer) As String
        Dim sReturn As New StringBuilder(String.Empty)
        Dim ChildLocations As New List(Of Location)
        If ParentLocationID <> String.Empty Or Level = 0 Then
            For Each myrow As Location In Locations
                If (myrow.RecordSource = "Category" Or (myrow.RecordSource = "Page")) Then
                    If (myrow.ParentLocationID = ParentLocationID) Then
                        ChildLocations = Locations.FindChildLocation(myrow.LocationID)
                        If ChildLocations.Count > 0 Then
                            If myrow.LocationID = HomeLocationID Then
                                sReturn.Append("<li class=""has-flyout active"" >")
                            ElseIf myrow.LocationID = LocationID Then
                                sReturn.Append("<li class=""has-flyout"" >")
                            Else
                                sReturn.Append("<li class=""has-flyout"" >")
                            End If
                            sReturn.Append(BuildClassLink(myrow, "", False))
                            sReturn.Append("<a href=""#"": class=""flyout-toggle""><span> </span></a>")
                            sReturn.Append(fndBuilNavBar(myrow.LocationID, LocationID, Level + 1))
                            sReturn.Append("</li>" & vbCrLf)
                        Else
                            If myrow.LocationID = HomeLocationID Then
                                sReturn.Append("<li class=""active"" >")
                            ElseIf myrow.LocationID = LocationID Then
                                sReturn.Append("<li >")
                            Else
                                sReturn.Append("<li >")
                            End If
                            sReturn.Append(BuildClassLink(myrow, "", False))
                            sReturn.Append("</li>" & vbCrLf)
                        End If
                    End If
                End If
            Next
            If sReturn.Length > 0 Then
                If Level > 0 Then
                    sReturn.Append(vbCrLf)
                    sReturn.Insert(0, "<ul class=""flyout"">")
                    sReturn.Append("</ul>")
                    sReturn.Append(vbCrLf)
                Else
                    sReturn.Insert(0, "<ul class=""nav-bar"">")
                    sReturn.Append("</ul>")
                End If
            End If
        End If
        Return sReturn.ToString()
    End Function
#End Region

#Region "YUI Functions"
    Public Function yuiBuildMenuChild(ByVal LocationID As String, ByVal ParentLocationID As String, ByVal MenuID As String) As String
        Dim sReturn As New StringBuilder(String.Empty)
        If ParentLocationID <> String.Empty Then
            sReturn.Append(yuiBuildLineItemList(ParentLocationID, "first-of-type", LocationID))
            If sReturn.Length > 0 Then
                sReturn.Insert(0, String.Format("{0}<div id=""{1}{2}"" class=""yuimenu"">{3}<div class=""bd"">{3}",
                                      String.Format("<script type=""text/javascript"">{0}YAHOO.util.Event.onContentReady(""{1}{2}"", function () {{var oMenu = new YAHOO.widget.Menu(""{1}{2}"", {{position: ""static"", hidedelay: 750, lazyload: true }});oMenu.render();}});{0}</script>{0}",
                                                                                                                        vbCrLf,
                                                                                                                        MenuID,
                                                                                                                        LocationID),
                                      MenuID,
                                      LocationID,
                                      vbCrLf))
                sReturn.Append(String.Format("{0}</div>{0}</div>", vbCrLf))
            End If
        End If
        Return sReturn.ToString
    End Function
    Public Function yuiBuildPageList(ByVal ParentLocationID As String, ByVal GroupName As String, ByVal LocationID As String, ByVal Level As Integer) As String
        Dim sPageName As String = (String.Empty)
        Dim sReturn As New StringBuilder(String.Empty)
        If ParentLocationID <> String.Empty Then
            For Each myLocation As Location In Locations
                If (myLocation.DisplayInMenu) Then
                    If (myLocation.ParentLocationID = ParentLocationID) Then
                        If myLocation.LocationID = LocationID Then
                            sReturn.Append("<li class=""yuimenuitem selected"">")
                            sReturn.Append(BuildClassLink(myLocation, "yuimenuitemlabel selected", True))
                        Else
                            sReturn.Append("<li class=""yuimenuitem "">")
                            sReturn.Append(BuildClassLink(myLocation, "yuimenuitemlabel", True))
                        End If
                        sReturn.Append(yuiBuildPageList(myLocation.LocationID, myLocation.LocationName, LocationID, Level + 1))
                        sReturn.Append("</li>" & vbCrLf)
                    End If
                    If myLocation.LocationID = ParentLocationID Then
                        sPageName = myLocation.LocationName
                    End If
                End If
            Next
            If sReturn.Length > 0 Then
                If Level > 1 Then
                    sReturn.Append(vbCrLf)
                    sReturn.Insert(0, String.Format("<div id=""g_{0}-{1}"" class=""yuimenu""><div class=""bd""><ul class=""SiteCategoryGroup{2}"">", Replace(GroupName, " ", ""), Replace(sPageName, " ", ""), GroupName))
                    sReturn.Append("</ul></div></div>")
                    sReturn.Append(vbCrLf)
                Else
                    sReturn.Insert(0, String.Format("<ul id=""g_{0}"">", Replace(GroupName, " ", "")))
                    sReturn.Append("</ul>")
                End If
            End If
        End If
        Return sReturn.ToString
    End Function
    Public Function yuiBuildSiteCategoryGroupList(ByVal ParentLocationID As String, ByVal GroupName As String, ByVal LocationID As String, ByVal Level As Integer) As String
        Dim LocationName As String = ("")
        Dim sReturn As New StringBuilder("")
        For Each myLocation As Location In Locations
            If (myLocation.LocationGroupNM = GroupName) Then
                If (myLocation.ParentLocationID = ParentLocationID) Or (Level = 1 And myLocation.ParentLocationID = "") Then
                    If myLocation.LocationID = LocationID Then
                        sReturn.Append("<li class=""yuimenuitem selected"">")
                        sReturn.Append(BuildClassLink(myLocation, "yuimenuitemlabel selected", True))
                    Else
                        sReturn.Append("<li class=""yuimenuitem "">")
                        sReturn.Append(BuildClassLink(myLocation, "yuimenuitemlabel", True))
                    End If
                    sReturn.Append(yuiBuildSiteCategoryGroupList(myLocation.LocationID, GroupName, LocationID, Level + 1))
                    sReturn.Append("</li>" & vbCrLf)
                End If
                If myLocation.LocationID = ParentLocationID Then
                    LocationName = myLocation.LocationName
                End If
            End If
        Next
        If sReturn.Length > 0 Then
            If Level > 1 Then
                sReturn.Append(vbCrLf)
                sReturn.Insert(0, String.Format("<div id=""{0}-{1}"" class=""yuimenu""><div class=""bd""><ul class=""SiteCategoryGroup{0}"">", GroupName, LocationName))
                sReturn.Append("</ul></div></div>")
                sReturn.Append(vbCrLf)
            Else
                sReturn.Insert(0, String.Format("<ul id=""{0}"">", GroupName))
                sReturn.Append("</ul>")
            End If
        End If
        Return sReturn.ToString
    End Function
    Public Function yuiBuildSiteCategoryGroupBar(ByVal ParentLocationID As String, ByVal GroupName As String, ByVal LocationID As String, ByVal Level As Integer) As String
        Dim sPageName As String = ("")
        Dim sReturn As New StringBuilder("")
        For Each myrow As Location In Locations
            If (myrow.RecordSource = "Category" And myrow.LocationGroupNM = GroupName) Then
                If myrow.ParentLocationID = ParentLocationID Then
                    If myrow.LocationID = LocationID Then
                        sReturn.Append("<li class=""yuimenubaritem  selected"">")
                        sReturn.Append(BuildClassLink(myrow, "yuimenubaritemlabel selected", True))
                    Else
                        sReturn.Append("<li class=""yuimenubaritem  "">")
                        sReturn.Append(BuildClassLink(myrow, "yuimenubaritemlabel", True))
                    End If
                    sReturn.Append(yuiBuildSiteCategoryGroupList(myrow.LocationID, GroupName, LocationID, Level + 1))
                    sReturn.Append("</li>" & vbCrLf)
                End If
                If myrow.LocationID = ParentLocationID Then
                    sPageName = myrow.LocationName
                End If
            End If
            If "CAT-" & myrow.SiteCategoryID = ParentLocationID And myrow.RecordSource = "Page" Then
                sReturn.Append("<li class=""yuimenubaritem "">")
                sReturn.Append(BuildClassLink(myrow, "yuimenubaritemlabel", True))
                sReturn.Append("</li>" & vbCrLf)
            End If
        Next
        If sReturn.Length > 0 Then
            If Level > 1 Then
                sReturn.Append(vbCrLf)
                sReturn.Insert(0, String.Format("<div id=""{0}-{1}"" class=""yuimenu""><div class=""bd""><ul class=""SiteCategoryGroup{0}"">", GroupName, sPageName))
                sReturn.Append("</ul></div></div>")
                sReturn.Append(vbCrLf)
            Else
                sReturn.Insert(0, String.Format("<ul id=""{0}"">", GroupName))
                sReturn.Append("</ul>")
            End If
        End If
        Return sReturn.ToString
    End Function

    Public Function BuildFoundationNavSection(ByVal ParentID As String, ByVal intLevel As Double) As String
        Dim sReturn As New StringBuilder("")
        If intLevel = 0 Then
            sReturn.Append(String.Format("<nav class=""top-bar"" data-topbar>{0}", vbCrLf))
            sReturn.Append(String.Format("<ul class=""title-area"">{0}", vbCrLf))
            sReturn.Append(String.Format("<li class=""name"">{0}<h1><a href=""/"">{1}</a></h1>{0}</li>{0}", vbCrLf, CompanyNM))
            sReturn.Append(String.Format("<li class=""toggle-topbar menu-icon""><a href=""#""><span>Menu</span></a></li>{0}</ul>{0}", vbCrLf))
            sReturn.Append(String.Format("<section class=""top-bar-section"">{0}", vbCrLf))
            sReturn.Append(String.Format("<ul class=""right"">{0}", vbCrLf))
            For Each myTopLoc As Location In Locations.FindTopMenuLocation()
                If Locations.FindChildLocation(myTopLoc.LocationID).Count > 0 Then
                    sReturn.Append("<li class=""has-dropdown"">")
                    sReturn.Append(BuildClassLink(myTopLoc, String.Empty, False))
                    sReturn.Append(String.Format("{0}</li>{1}", BuildFoundationNavChild(myTopLoc.LocationID, intLevel + 1), vbCrLf))
                Else
                    sReturn.Append("<li class="""">")
                    sReturn.Append(BuildClassLink(myTopLoc, String.Empty, False))
                    sReturn.Append(String.Format("{0}</li>{1}", String.Empty, vbCrLf))
                End If
            Next
            sReturn.Append("</ul></section></nav>" & vbCrLf)
        Else
            BuildFoundationNavChild(ParentID, intLevel)
        End If
        Return sReturn.ToString
    End Function
    Private Function BuildFoundationNavChild(ByVal ParentID As String, ByVal intLevel As Double) As String
        Dim sReturn As New StringBuilder("")
        sReturn.Append(String.Format("<ul class=""dropdown"">{0}", vbCrLf))
        For Each myLoc As Location In Locations.FindChildLocation(ParentID)
            If Locations.FindChildLocation(myLoc.LocationID).Count > 0 Then
                sReturn.Append("<li class=""has-dropdown"">")
                sReturn.Append(BuildClassLink(myLoc, String.Empty, False))
                sReturn.Append(String.Format("{0}</li>{1}", BuildFoundationNavChild(myLoc.LocationID, intLevel + 1), vbCrLf))
            Else
                sReturn.Append("<li class="""">")
                sReturn.Append(BuildClassLink(myLoc, String.Empty, False))
                sReturn.Append(String.Format("{0}</li>{1}", String.Empty, vbCrLf))
            End If
        Next
        sReturn.Append("</ul>" & vbCrLf)
        Return sReturn.ToString
    End Function
    Public Function BuildTopMenuTree(ByVal ParentID As String, ByVal intLevel As Double, ByVal PageName As String) As String
        Dim sReturn As New StringBuilder("")
        For Each myRow As Location In Locations
            If (myRow.RecordSource = "Page") Then
                If (ParentID = myRow.ParentLocationID) Then
                    If intLevel = 0 Then
                        PageName = myRow.LocationName
                        sReturn.Append("<li>")
                        sReturn.Append(BuildClassLink(myRow, "toplevel", True))
                        sReturn.Append(String.Format("{0}</li>{1}", BuildTopMenuTree(myRow.LocationID, intLevel + 1, myRow.LocationName), vbCrLf))
                    ElseIf intLevel = 1 Then
                        sReturn.Append("<li>")
                        sReturn.Append(BuildClassLink(myRow, "sublevel", True))
                        sReturn.Append(String.Format("{0}</li>{1}", BuildTopMenuTree(myRow.LocationID, intLevel + 1, myRow.LocationName), vbCrLf))
                    Else
                        ' do nothing
                    End If
                End If
            End If
        Next
        If (sReturn.Length > 0) Then
            If intLevel = 0 Then
                sReturn.Insert(0, String.Format("<ul class=""topmenu"">{0}", vbCrLf))
            Else
                sReturn.Insert(0, String.Format("<ul>{0}", vbCrLf))
            End If
            sReturn.Append("</ul>" & vbCrLf)
        End If
        Return sReturn.ToString
    End Function

    Public Function yuiBuildPageTree(ByVal ParentID As String, ByVal intLevel As Double, ByVal PageName As String) As String
        Dim sReturn As New StringBuilder("")
        For Each myRow As Location In Locations
            If (myRow.RecordSource = "Page") Then
                If (ParentID = myRow.ParentLocationID) Then
                    If intLevel = 0 Then
                        PageName = myRow.LocationName
                        sReturn.Append("<li class=""yuimenubaritem"">")
                        sReturn.Append(BuildClassLink(myRow, "yuimenubaritemlabel", True))
                        sReturn.Append(String.Format("{0}</li>{1}", yuiBuildPageTree(myRow.LocationID, intLevel + 1, myRow.LocationName), vbCrLf))
                    Else
                        sReturn.Append("<li class=""yuimenuitem"">")
                        sReturn.Append(BuildClassLink(myRow, "yuimenuitemlabel", True))
                        sReturn.Append(String.Format("{0}</li>{1}", yuiBuildPageTree(myRow.LocationID, intLevel + 1, myRow.LocationName), vbCrLf))
                    End If
                End If
            End If
        Next
        If (sReturn.Length > 0) Then
            If intLevel = 0 Then
                sReturn.Insert(0, String.Format("<div id=""yuiMenuTree"" class=""yuimenubar yuimenubarnav"">{0}<div class=""bd"">{0}<ul class=""first-of-type"">{0}", vbCrLf))
            Else
                sReturn.Insert(0, String.Format("<div id=""p_{0}"" class=""yuimenu"">{1}<div class=""bd"">{1}<ul>{1}", Replace(PageName, " ", ""), vbCrLf))
            End If
            sReturn.Append("</ul></div></div>" & vbCrLf)
        End If
        Return sReturn.ToString
    End Function
#End Region
    Public Function mooBuildPageList(ByVal ParentLocationID As String, ByVal LocationID As String, ByVal Level As Integer) As String
        Dim sPageName As String = (String.Empty)
        Dim sReturn As New StringBuilder(String.Empty)
        If ParentLocationID <> String.Empty Or Level = 0 Then
            For Each myrow As Location In Locations
                If (myrow.RecordSource = "Category" Or (myrow.RecordSource = "Page")) Then
                    If (myrow.ParentLocationID = ParentLocationID) Then
                        If myrow.LocationID = LocationID Then
                            sReturn.Append("<li class=""active_over"" onmouseover=""this.className='active_over'"" onmouseout=""this.className='active'"">")
                            sReturn.Append(BuildClassLink(myrow, "selected", True))
                        Else
                            sReturn.Append("<li class=""active"" onmouseover=""this.className='active_over'"" onmouseout=""this.className='active'"">")
                            sReturn.Append(BuildClassLink(myrow, "", True))
                        End If
                        sReturn.Append(mooBuildPageList(myrow.LocationID, LocationID, Level + 1))
                        sReturn.Append("</li>" & vbCrLf)
                    End If
                    If myrow.LocationID = ParentLocationID Then
                        sPageName = myrow.LocationName
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


    Public Function BuildSiteCategoryGroupList(ByVal ParentLocationID As String, ByVal GroupName As String, ByVal LocationID As String, ByVal Level As Integer, ByVal GroupDescription As String) As String
        Dim sPageName As String = ("")
        Dim sReturn As New StringBuilder("")
        For Each myrow As Location In Locations
            If (myrow.LocationGroupNM = GroupName) Then
                If (myrow.ParentLocationID = ParentLocationID) Or (Level = 1 And myrow.ParentLocationID = "") Then
                    If myrow.LocationID = LocationID Then
                        sReturn.Append("<li class=""menuitem selected"">")
                        sReturn.Append(BuildClassLink(myrow, "menuitemlabel selected", True))
                    Else
                        sReturn.Append("<li class=""menuitem "">")
                        sReturn.Append(BuildClassLink(myrow, "menuitemlabel", True))
                    End If
                    sReturn.Append("</li>" & vbCrLf)
                End If
                If myrow.LocationID = ParentLocationID Then
                    sPageName = myrow.LocationName
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
                If String.IsNullOrWhiteSpace(GroupDescription) Then
                    sReturn.Insert(0, String.Format("<div class=""menugroup""><ul class=""menu"">{0}", vbCrLf))
                Else
                    sReturn.Insert(0, String.Format("<div class=""menugroup""><div class=""menugroupname"">{0}</div><ul class=""menu"">{1}", GroupDescription, vbCrLf))
                End If
                sReturn.Append("</ul></div>")
            End If
        End If
        Return sReturn.ToString
    End Function
    Public Function BuildMenuChild(ByVal LocationID As String, ByVal ParentLocationID As String, ByVal DefaultParentLocationID As String, ByVal ULClassName As String, ByVal LIClassName As String) As String

        Dim sReturn As New StringBuilder("")
        For Each myrow As Location In Locations
            If (myrow.RecordSource = "Page") Then
                If myrow.ParentLocationID = ParentLocationID Then
                    If myrow.LocationID = LocationID Then
                        sReturn.Append(String.Format("<li class="" {0} selected"">", LIClassName))
                        sReturn.Append(BuildNaviagtionLink(myrow, True))
                    Else
                        sReturn.Append(String.Format("<li class="" {0} "">", LIClassName))
                        sReturn.Append(BuildNaviagtionLink(myrow, False))
                    End If
                    sReturn.Append("</li>" & vbCrLf)
                End If
            End If
        Next
        If sReturn.Length > 0 Then
            sReturn.Insert(0, String.Format("<ul class=""{0}"">{1}", ULClassName, vbCrLf))
            sReturn.Append("</ul>" & vbCrLf)
        Else
            If ParentLocationID <> DefaultParentLocationID And DefaultParentLocationID <> "" Then
                sReturn.Append(BuildMenuChild(LocationID, DefaultParentLocationID, DefaultParentLocationID, ULClassName, LIClassName))
            End If
        End If
        Return sReturn.ToString
    End Function
    Public Function BuildPageTree(ByVal ParentID As String, ByVal intLevel As Double, ByVal sULClassName As String) As String
        Return Locations.BuildPageTree(ParentID, intLevel, sULClassName)
    End Function
    Public Function BuildLinkMenu(ByVal RequestLevel As Integer, ByVal DefaultParentLocationID As String, ByVal myReturnTemplate As String, ByRef CurrentMapRow As Location) As String
        Dim ReturnString As String = ("")
        Dim LocationID As String = CurrentMapRow.LocationID
        Dim ParentLocationID As String = CurrentMapRow.ParentLocationID
        Dim ParentPageName As String = ""
        If RequestLevel <= CurrentMapRow.LevelNBR Then
            For Each myrow As LocationTrail In CurrentMapRow.LocationTrailList
                If RequestLevel = myrow.MenuLevelNBR Then
                    LocationID = myrow.LocationID
                    ParentLocationID = myrow.ParentLocationID
                    Exit For
                End If
            Next
            For Each myrow As LocationTrail In CurrentMapRow.LocationTrailList
                If myrow.LocationID = ParentLocationID Then
                    ParentPageName = myrow.Name
                    Exit For
                End If
            Next
            If DefaultParentLocationID = "" Then
                DefaultParentLocationID = ParentLocationID
            End If
            ReturnString = BuildMenuChild(LocationID, ParentLocationID, DefaultParentLocationID, String.Empty, String.Empty)
        ElseIf RequestLevel = CurrentMapRow.LevelNBR + 1 Then
            ReturnString = BuildMenuChild(LocationID, LocationID, DefaultParentLocationID, String.Empty, String.Empty)
        End If
        If ParentPageName = "" Then
            ParentPageName = CurrentMapRow.LocationName
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
        sbContent.Replace("~~SiteURL~~", CompanyURL)
        sbContent.Replace("~~SiteCompanyName~~", CompanyNM)
        sbContent.Replace("~~SiteDomainName~~", DomainName)
        sbContent.Replace("~~SiteDescription~~", CompanyDescription)
        sbContent.Replace("~~SiteKeywords~~", CompanyKeywords)
        sbContent.Replace("~~SiteMap~~", String.Format("<div class=""sitemap""><ul class=""sitemap"">{0}{1}</ul></div>{0}", vbCrLf, Locations.BuildPageTree("", 0, "sitemap")))
        sbContent.Replace("~~SiteTree~~", Locations.BuildPageTree("", 0, "sitemap"))
        sbContent.Replace("~~SiteCity~~", SiteCity)
        sbContent.Replace("~~SiteCityDash~~", wpm_FormatPageNameForURL(SiteCity))
        sbContent.Replace("~~SiteCityNoSpace~~", Replace(SiteCity, " ", ""))
        sbContent.Replace("~~SiteState~~", SiteState)
        sbContent.Replace("~~SiteCountry~~", SiteCountry)
        Return True
    End Function
#End Region
#Region "Private Methods"
    Private Function updatePartCategories() As Boolean
        For Each myPartCategory As PartCategory In PartCategories
            myPartCategory.PartCount = GetPartCountByCategory(myPartCategory.ID)
        Next
        Return True
    End Function
    Private Function GetPartCountByCategory(ByVal myPartCategoryID As String) As Integer
        Dim PartCount As Integer = 0
        For Each myPart As Part In Parts
            If myPart.PartCategoryID = myPartCategoryID Then
                PartCount = PartCount + 1
            End If
        Next
        Return PartCount
    End Function
    Private Function yuiBuildLineItemList(ByVal ParentLocationID As String, ByVal ListClass As String, ByVal LocationID As String) As String
        Dim LoacationName As String = (String.Empty)
        Dim sReturn As New StringBuilder(String.Empty)
        For Each myLocation As Location In Locations
            If (myLocation.RecordSource = "Page") Or myLocation.RecordSource = "Category" Then
                If myLocation.ParentLocationID = ParentLocationID Then
                    If myLocation.LocationID = LocationID Then
                        sReturn.Append("<li class=""yuimenuitem selected"">")
                        sReturn.Append(BuildClassLink(myLocation, "yuimenuitemlabel selected", True))
                        sReturn.Append("</li>" & vbCrLf)
                    Else
                        sReturn.Append("<li class=""yuimenuitem "">")
                        sReturn.Append(BuildClassLink(myLocation, "yuimenuitemlabel", True))
                        sReturn.Append("</li>" & vbCrLf)
                    End If
                End If
                If myLocation.LocationID = ParentLocationID Then
                    LoacationName = myLocation.LocationName
                End If
            End If
        Next
        If sReturn.Length > 0 Then
            sReturn.Insert(0, String.Format("<h6 class=""{0}"">{1}</h6><ul class=""{0}"">", ListClass, LoacationName))
            sReturn.Append("</ul>")
        End If
        Return sReturn.ToString
    End Function

    Private Function BuildClassLink(ByRef myRow As Location, ByVal LinkClass As String, ByVal bSpan As Boolean) As String
        If bSpan Then
            If LinkClass = String.Empty Then
                Return (String.Format("<a href=""{0}"" title=""{2}""><span>{1}</span></a>", wpm_GetSiteMapRowURL(myRow, UseBreadCrumbURL), myRow.LocationName, myRow.LocationDescription))
            Else
                Return (String.Format("<a class=""{0}"" href=""{1}"" title=""{2}"" ><span>{2}</span></a>", LinkClass, wpm_GetSiteMapRowURL(myRow, UseBreadCrumbURL), myRow.LocationName, myRow.LocationDescription))
            End If
        Else
            If LinkClass = String.Empty Then
                Return (String.Format("<a href=""{0}"" title=""{2}"">{1}</a>", wpm_GetSiteMapRowURL(myRow, UseBreadCrumbURL), myRow.LocationName, myRow.LocationDescription))
            Else
                Return (String.Format("<a class=""{0}"" href=""{1}"" title=""{2}"" >{2}</a>", LinkClass, wpm_GetSiteMapRowURL(myRow, UseBreadCrumbURL), myRow.LocationName, myRow.LocationDescription))
            End If

        End If
    End Function
    Private Function BuildNaviagtionLink(ByRef myRow As Location, ByVal IsSelected As Boolean) As String
        If (IsSelected) Then
            Return BuildClassLink(myRow, "selected", True)
        Else
            Return BuildClassLink(myRow, "", True)
        End If
    End Function
    Public Function SetCompanyValue(ByVal myrow As DataRow) As Company
        CompanyID = wpm_GetDBString(myrow("CompanyID"))
        CompanyNM = wpm_GetDBString(myrow("CompanyName"))
        DomainName = wpm_GetDBString(myrow("SiteURL"))
        SiteGallery = wpm_GetDBString(myrow("GalleryFolder"))
        DomainName = Replace(Replace(CompanyURL.ToLower, "http://", String.Empty), "www.", String.Empty)
        CompanyTitle = wpm_GetDBString(myrow("SiteTitle"))
        CompanyKeywords = wpm_GetDBString(myrow("DefaultPaymentTerms"))
        CompanyDescription = wpm_GetDBString(myrow("DefaultInvoiceDescription"))
        SitePrefix = wpm_GetDBString(myrow("SiteTemplate"))
        DefaultArticleID = wpm_GetDBInteger(myrow("DefaultArticleID"))
        HomeLocationID = wpm_GetDBString(myrow("HomePageID"))
        DefaultSitePrefix = wpm_GetDBString(myrow("DefaultSiteTemplate"))
        UseBreadCrumbURL = wpm_GetDBBoolean(myrow("UseBreadCrumbURL"))
        SiteCity = wpm_GetDBString(myrow("City"))
        SiteState = wpm_GetDBString(myrow("StateOrProvince"))
        SiteCountry = wpm_GetDBString(myrow("Country"))
        PostalCode = wpm_GetDBString(myrow("PostalCode"))
        FromEmail = wpm_GetDBString(myrow("FromEmail"))
        SMTP = wpm_GetDBString(myrow("SMTP"))
        Component = wpm_GetDBString(myrow("Component"))

        SiteType.SiteTypeID = wpm_GetDBString(myrow("SiteCategoryTypeID"))
        SiteType.SiteTypeNM = wpm_GetDBString(myrow("SiteCategoryTypeNM"))

        SiteCategoryTypeID = wpm_GetDBString(myrow("SiteCategoryTypeID"))
        SiteCategoryTypeNM = wpm_GetDBString(myrow("SiteCategoryTypeNM"))
        DefaultSiteCategoryID = wpm_GetDBString(myrow("DefaultSiteCategoryID"))
        If HomeLocationID = String.Empty And DefaultSiteCategoryID <> String.Empty Then
            HomeLocationID = "CAT-" & DefaultSiteCategoryID
        End If
        Return Me
    End Function
    Private Function GetCompanyValues(ByVal ReqCompanyID As String) As Boolean
        If IsNumeric(ReqCompanyID) AndAlso CInt(ReqCompanyID) > 0 Then
            For Each myrow As DataRow In ApplicationDAL.GetCompanyData(ReqCompanyID).Rows
                SetCompanyValue(myrow)
                Exit For
            Next
            Return IsValidCompany()
        Else
            ApplicationLogging.ConfigLog("Company.GetCompanyValues", String.Format("Failed to Load Company Information - {0}", ReqCompanyID))
            wpm_AddGenericError(String.Format("Company.GetCompanyValues <br/>Failed to Load Company Information - {0}", ReqCompanyID))

            Return False
        End If
    End Function
    Public Function GetIndexFilePath() As String
        Dim myFullPath As String = HttpContext.Current.Server.MapPath(STR_App_Datasites)


        If Not FileProcessing.VerifyFolderExists(myFullPath) Then
            FileProcessing.CreateFolder(myFullPath)
        End If
        If Not FileProcessing.VerifyFolderExists(String.Format("{0}\{1}", myFullPath, GetHostName())) Then
            FileProcessing.CreateFolder(String.Format("{0}\{1}", myFullPath, GetHostName()))
        End If

        myFullPath = Replace(String.Format("{0}\{1}\{1}-site-file.xml", myFullPath, GetHostName()), "\\", "\")

        Return myFullPath
    End Function

    Private Function GetHostName() As String
        Return Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", "")
    End Function

    Private Function SaveCompanyProfileXML() As Boolean
        Dim bReturn As Boolean = True
        HttpContext.Current.Application.Lock()
        Try
            Using sw As New System.IO.StreamWriter(GetIndexFilePath(), False)
                Try
                    Dim ViewWriter As New XmlSerializer(GetType(Company))
                    ViewWriter.Serialize(sw, Me)
                Catch ex As Exception
                    ApplicationLogging.ConfigLog("CompanyProfile.SaveCompanyProfileXML", String.Format("Error with Serialize - {0}", ex))
                    bReturn = False
                End Try
            End Using
        Catch ex As Exception
            ApplicationLogging.ConfigLog("CompanyProfile.SaveCompanyProfileXML", String.Format("Error with File - {0}", ex))
            bReturn = False
        End Try
        HttpContext.Current.Application.UnLock()
        Return bReturn
    End Function

    Public Function GetLocationImages(ByVal myLocationID As String) As List(Of LocationImage)
        Return (From f As LocationImage In Images Where f.ParentLocationID = myLocationID Order By f.DisplayOrder Ascending).ToList()
    End Function
#End Region

End Class
