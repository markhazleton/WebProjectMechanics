Public Class mhDataCon
    Dim sSQL As String = String.Empty

    Private Function GetNavigationDataTable(ByVal sSortBy As String, ByVal CompanyID As String, ByVal GroupID As Integer) As DataTable
        Dim strSQL As String
        ' Default to Guest if Group does not exist
        strSQL = "SELECT Page.PageID, " & _
                       "Page.PageName, " & _
                        "Page.PageDescription, " & _
                       " Page.ParentPageID, " & _
                       "0," & _
                       "Page.PageKeywords, " & _
                       "pagetype.PageFileName, " & _
                       "Page.PageFileName, " & _
                       "Page.ModifiedDT, " & _
                       "Page.Active " & _
                  "FROM Page, pagetype  "

        If (mhSession.GetUserRoleFilterMenu()) Then
            strSQL = strSQL & " , PageRole where Page.PageID = PageRole.PageID and PageRole.RoleID =" & mhSession.GetUserRoleID() & " and Page.PageTypeID = pagetype.PageTypeID  AND Page.GroupID >= " & GroupID & " "
        Else
            strSQL = strSQL & " WHERE Page.PageTypeID = pagetype.PageTypeID  AND Page.GroupID >= " & GroupID & " "
        End If
        ' Hide inactive pages for everyone but Admin
        If Not (mhUser.IsAdmin()) Then
            strSQL = strSQL & " AND Page.Active <> 0 "
        End If
        ' Limit pages to the Company of the User
        If (CStr(CompanyID) <> "") Then
            strSQL = strSQL & " AND Page.CompanyID =" & CompanyID & " "
        Else
            If (CompanyID <> "") Then
                strSQL = strSQL & " AND ( Page.CompanyID =" & CompanyID & " ) "
            End If
        End If
        ' Order results by PageOrder
        Select Case sSortBy
            Case "ORDER"
                strSQL = strSQL & "ORDER BY [Page].[PageOrder], [Page].[PageName],'99' "
            Case "MODIFIED"
                strSQL = strSQL & "ORDER BY [Page].[ModifiedDT] DESC "
            Case "NAME"
                strSQL = strSQL & "ORDER BY [Page].[PageName] ASC "
            Case "PARENT"
                strSQL = strSQL & "ORDER BY [Page].[ParentPageID] ASC "
            Case Else
                strSQL = strSQL & "ORDER BY [Page].[PageOrder], [Page].[PageName],'99' "
        End Select
        Return mhDB.GetDataTable(strSQL, "GetNavigationDataTable")
    End Function
    '********************************************************************************
    Public Shared Function GetSiteGroupList(ByVal CompanyID As String) As DataTable
        Return mhDB.GetDataTable(("SELECT " & _
                 "SiteCategoryGroup.[SiteCategoryGroupID], " & _
                 "SiteCategoryGroup.[SiteCategoryGroupNM], " & _
                 "SiteCategoryGroup.[SiteCategoryGroupDS], " & _
                 "SiteCategoryGroup.[SiteCategoryGroupOrder]" & _
                 "FROM SiteCategoryGroup;"), "SiteCategoryGroup")
    End Function
    Public Shared Function GetSiteLinks(ByVal CompanyID As String) As DataTable
        Return mhDB.GetDataTable(("SELECT " & _
                 "Link.ID," & _
                 "Link.LinkTypeCD, " & _
                 "LinkCategory.Title as LinkCategoryTitle, " & _
                 "Link.CategoryID, " & _
                 "Link.PageID," & _
                 "Link.Title as LinkTitle, " & _
                 "Link.Description, " & _
                 "Link.URL, " & _
                 "Link.DateAdd, " & _
                 "Link.Ranks, " & _
                 "Link.Views, " & _
                 "Link.UserName, " & _
                 "Link.UserID, " & _
                 "Link.ASIN, " & _
                 "'Link' as LinkSource, " & _
                 "'' as SiteCategoryTypeID, " & _
                 "'' as SiteCategoryGroupID, " & _
                 "Link.CompanyID " & _
                 "FROM Link, LinkCategory " & _
                 "where Link.CategoryID = LinkCategory.ID " & _
                 "and ( Link.CompanyID =" & CompanyID & " or Link.CompanyID is null) " & _
                 "ORDER BY Link.Ranks"), "mhDataCon.GetSiteLinks")
    End Function
    Public Shared Function GetSiteCategoryLinks(ByVal CompanyID As String, ByVal SiteCategoryTypeID As String) As DataTable
        Return mhDB.GetDataTable(( _
         "SELECT " & _
                 "Link.ID," & _
                 "Link.LinkTypeCD, " & _
                 "LinkCategory.Title as LinkCategoryTitle, " & _
                 "Link.CategoryID, " & _
                 "Link.PageID," & _
                 "Link.Title as LinkTitle, " & _
                 "Link.Description, " & _
                 "Link.URL, " & _
                 "Link.DateAdd, " & _
                 "Link.Ranks, " & _
                 "Link.Views, " & _
                 "Link.UserName, " & _
                 "Link.UserID, " & _
                 "Link.ASIN, " & _
                 "Link.Ranks,  " & _
                 "'Link' as LinkSource, " & _
                 "'' as SiteCategoryTypeID, " & _
                 "'' as SiteCategoryGroupID, " & _
                 "Link.CompanyID " & _
                 "FROM Link, LinkCategory " & _
                 "where Link.CategoryID = LinkCategory.ID " & _
                 "and ( Link.CompanyID =" & CompanyID & " or Link.CompanyID is null) " & _
        "union all SELECT " & _
                      "SiteLink.ID," & _
                      "SiteLink.LinkTypeCD, " & _
                      "LinkCategory.Title as LinkCategoryTitle, " & _
                      "SiteLink.CategoryID, " & _
                      "'CAT-' & SiteLink.SiteCategoryID as PageID, " & _
                      "SiteLink.Title as LinkTitle, " & _
                      "SiteLink.Description, " & _
                      "SiteLink.URL, " & _
                      "SiteLink.DateAdd, " & _
                      "SiteLink.Ranks, " & _
                      "SiteLink.Views, " & _
                      "SiteLink.UserName, " & _
                      "SiteLink.UserID, " & _
                      "SiteLink.ASIN, " & _
                      "SiteLink.Ranks,  " & _
                      "'SiteLink' as LinkSource, " & _
                      "SiteLink.SiteCategoryTypeID, " & _
                      "SiteLink.SiteCategoryGroupID, " & _
                      "SiteLink.CompanyID " & _
                      "FROM SiteLink, LinkCategory " & _
                      "where SiteLink.CategoryID = LinkCategory.ID " & _
                      "and ( SiteLink.CompanyID =" & CompanyID & " or SiteLink.CompanyID is null) " & _
                      "and ( SiteLink.SiteCategoryTypeID =" & SiteCategoryTypeID & " or SiteLink.SiteCategoryTypeID is null) " & _
                 "ORDER BY 15 "), "mhDataCon.GetSiteLinks")
    End Function
    Public Shared Function GetLinkCategoryList(ByVal CompanyID As String) As DataTable
        'Return mhDB.GetDataTable( _
        '  "SELECT LinkCategory.ID,  " & _
        '                "LinkCategory.Title,  " & _
        '                "LinkCategory.ParentID,  " & _
        '                "LinkCategory.Description,  " & _
        '                "Page.PageID,  " & _
        '                "Count(Link.ID) AS CountOfID " & _
        '  "FROM Page RIGHT JOIN (LinkCategory LEFT JOIN Link ON LinkCategory.ID = Link.CategoryID) ON Page.PageID = LinkCategory.PageID " & _
        '  "WHERE((Page.CompanyID = " & CompanyID & ") Or (Page.CompanyID Is Null)) " & _
        '  "AND((Link.CompanyID = " & CompanyID & ") Or (Link.CompanyID Is Null)) " & _
        '  "AND ((Link.Views =TRUE) or (Link.Views is Null)) " & _
        '  "GROUP BY LinkCategory.ID, LinkCategory.Title, LinkCategory.ParentID, LinkCategory.Description, Page.PageID " & _
        '  "ORDER BY LinkCategory.Title;", "mhDataCon.GetLinkCategoryList")
        Return mhDB.GetDataTable( _
          "SELECT LinkCategory.ID,  " & _
                 "LinkCategory.Title,  " & _
                 "LinkCategory.ParentID,  " & _
                 "LinkCategory.Description,  " & _
                 "LinkCategory.PageID,  " & _
                 "0 AS CountOfID " & _
          "FROM LinkCategory ", "mhDataCon.GetLinkCategoryList")
    End Function
    Public Shared Function GetSiteParameterList(ByVal CompanyID As String) As DataTable
        Return mhDB.GetDataTable("SELECT CompanySiteParameter.CompanySiteParameterID,  " & _
                        "CompanySiteParameter.CompanyID,  " & _
                        "CompanySiteParameter.SiteParameterTypeID,  " & _
                        "CompanySiteParameter.SortOrder,  " & _
                        "CompanySiteParameter.ParameterValue,  " & _
                        "SiteParameterType.SiteParameterTypeNM,  " & _
                        "SiteParameterType.SiteParameterTypeDS,  " & _
                        "SiteParameterType.SiteParameterTypeOrder,  " & _
                        "SiteParameterType.SiteParameterTemplate  " & _
                        "FROM SiteParameterType  " & _
                        "LEFT JOIN CompanySiteParameter ON SiteParameterType.[SiteParameterTypeID] = CompanySiteParameter.[SiteParameterTypeID]  " & _
                        "WHERE  " & _
                        "(((CompanySiteParameter.CompanyID)=" & CompanyID & "))  " & _
                        "OR (((CompanySiteParameter.CompanyID) Is Null))  " & _
                        "union all " & _
                        "SELECT null,  " & _
                        "null,  " & _
                        "SiteParameterTypeID,  " & _
                        "SiteParameterType.SiteParameterTypeOrder,  " & _
                        "SiteParameterType.SiteParameterTemplate,  " & _
                        "SiteParameterType.SiteParameterTypeNM,  " & _
                        "SiteParameterType.SiteParameterTypeDS,  " & _
                        "SiteParameterType.SiteParameterTypeOrder,  " & _
                        "SiteParameterType.SiteParameterTemplate " & _
                        "        FROM(SiteParameterType) " & _
                        "order by 2 desc,4 asc", "mhDataCon.GetSiteParameterList")
    End Function

    'Public Shared Function GetPageLinkList(ByVal PageID As String) As DataTable
    '    Return mhDB.GetDataTable( _
    '      "SELECT LinkCategory.ID," & _
    '             "LinkCategory.Title, " & _
    '             "LinkCategory.ParentID, " & _
    '             "LinkCategory.Description, " & _
    '             "LinkCategory.PageID, " & _
    '             "Count(Link.ID) AS CountOfID " & _
    '             "FROM LinkCategory " & _
    '             "LEFT JOIN Link ON LinkCategory.ID=Link.CategoryID " & _
    '             "WHERE (Link.Views=True OR Link.Views Is Null) " & _
    '             "and LinkCategory.PageID=" & PageID & "  " & _
    '             "GROUP BY LinkCategory.Title, " & _
    '             "LinkCategory.ID,  " & _
    '             "LinkCategory.ParentID, " & _
    '             "LinkCategory.Description, " & _
    '             "LinkCategory.PageID " & _
    '             "ORDER BY LinkCategory.Title ;", "GetPageLinkList")

    'End Function
    Public Shared Function GetImageList(ByVal CompanyID As String) As DataTable
        Dim strSQL As String = "SELECT Image.[ImageID], " & _
                                      "Image.[ImageName], " & _
                                      "Image.[ImageFileName], " & _
                                      "Image.[ImageThumbFileName], " & _
                                      "Image.[ImageDescription], " & _
                                      "Image.[ImageComment], " & _
                                      "Image.[ImageDate], " & _
                                      "Image.[Active], " & _
                                      "Image.[ModifiedDT], " & _
                                      "Image.[VersionNo], " & _
                                      "Image.[ContactID], " & _
                                      "Image.[CompanyID], " & _
                                      "Image.[Title], " & _
                                      "Image.[Medium], " & _
                                      "Image.[Size], " & _
                                      "Image.[Price], " & _
                                      "Image.[Color], " & _
                                      "Image.[Subject], " & _
                                      "Image.[Sold] " & _
                                "FROM [Image] " & _
                               "where [CompanyID]=" & CompanyID & " "
        Return mhDB.GetDataTable(strSQL, "GetImageList")
    End Function
    Public Shared Function GetPageAliasList(ByVal CompanyID As String) As DataTable
        Dim strSQL As String = ("SELECT PageAliasID, PageURL, TargetURL, AliasType from PageAlias where [CompanyID]=" & CompanyID & " ")
        Return mhDB.GetDataTable(strSQL, "GetPageAliasList")
    End Function
    Public Shared Function GetUnlinkedImageList(ByVal CompanyID As String) As DataTable
        Dim strSQL As String = ("SELECT Image.[ImageID], " & _
                                      "Image.[ImageName], " & _
                                      "Image.[ImageFileName], " & _
                                      "Image.[ImageThumbFileName], " & _
                                      "Image.[ImageDescription], " & _
                                      "Image.[ImageComment], " & _
                                      "Image.[ImageDate], " & _
                                      "Image.[Active], " & _
                                      "Image.[ModifiedDT], " & _
                                      "Image.[VersionNo], " & _
                                      "Image.[ContactID], " & _
                                      "Image.[CompanyID], " & _
                                      "Image.[Title], " & _
                                      "Image.[Medium], " & _
                                      "Image.[Size], " & _
                                      "Image.[Price], " & _
                                      "Image.[Color], " & _
                                      "Image.[Subject], " & _
                                      "Image.[Sold] " & _
                                "FROM [Image] " & _
                                      " LEFT JOIN [PageImage] ON [Image].[ImageID] = [PageImage].[ImageID] " & _
                                      " WHERE [PageImage].[PageID] Is Null " & _
                                      "and [CompanyID]=" & CompanyID & " ")
        Return mhDB.GetDataTable(strSQL, "GetImageList")
    End Function
    Public Shared Function GetImageByID(ByVal ImageID As String) As DataTable
        Dim strSQL As String = ("SELECT Image.[ImageID], " & _
                                      "Image.[ImageName], " & _
                                      "Image.[ImageFileName], " & _
                                      "Image.[ImageThumbFileName], " & _
                                      "Image.[ImageDescription], " & _
                                      "Image.[ImageComment], " & _
                                      "Image.[ImageDate], " & _
                                      "Image.[Active], " & _
                                      "Image.[ModifiedDT], " & _
                                      "Image.[VersionNo], " & _
                                      "Image.[ContactID], " & _
                                      "Image.[CompanyID], " & _
                                      "Image.[Title], " & _
                                      "Image.[Medium], " & _
                                      "Image.[Size], " & _
                                      "Image.[Price], " & _
                                      "Image.[Color], " & _
                                      "Image.[Subject], " & _
                                      "Image.[Sold] " & _
                                "FROM [Image] " & _
                               "where [ImageID]=" & ImageID & " ")
        Return mhDB.GetDataTable(strSQL, "GetImageByID")
    End Function
    Public Shared Function GetPageImage(ByVal CompanyID As String) As DataTable
        Dim strSQL As String
        strSQL = "SELECT [Page].[PageID], [Page].[PageName], " & _
                        "[Page].[PageDescription], " & _
                        "[Page].[PageKeywords], " & _
                        "[Page].[ImagesPerRow], " & _
                        "[Page].[RowsPerPage], " & _
                        "[PageImage].[PageImagePosition], " & _
                        "[Image].[ImageID], " & _
                        "[Image].[ImageName], " & _
                        "[Image].[ImageFileName], " & _
                        "[Image].[ImageThumbFileName], " & _
                        "[Image].[ImageDescription], " & _
                        "[Image].[ImageComment], " & _
                        "[Image].[ImageDate], " & _
                        "[Image].[ModifiedDT], " & _
                        "[Image].[VersionNo], " & _
                        "[Image].[ContactID], " & _
                        "[Image].[title],  " & _
                        "[Image].[medium],  " & _
                        "[Image].[size] " & _
                        "FROM [Page],[PageImage],[Image] " & _
                        "WHERE [Page].[PageID] = [PageImage].[PageID] " & _
                        "AND [Image].[ImageID] = [PageImage].[ImageID] " & _
                        "AND [Page].[CompanyID] = " & CompanyID & " " & _
                        "ORDER BY [PageImage].[PageImagePosition] "

        Return mhDB.GetDataTable(strSQL, "SiteMap")

    End Function
    Public Shared Function GetPageImage(ByVal PageID As String, ByVal CompanyID As String, ByVal GroupID As String) As DataTable
        Dim strSQL As String
        strSQL = "SELECT [Page].[PageID], [Page].[PageName], " & _
                        "[Page].[PageDescription], " & _
                        "[Page].[PageKeywords], " & _
                        "[Page].[ImagesPerRow], " & _
                        "[Page].[RowsPerPage], " & _
                        "[PageImage].[PageImagePosition], " & _
                        "[Image].[ImageID], " & _
                        "[Image].[ImageName], " & _
                        "[Image].[ImageFileName], " & _
                        "[Image].[ImageThumbFileName], " & _
                        "[Image].[ImageDescription], " & _
                        "[Image].[ImageComment], " & _
                        "[Image].[ImageDate], " & _
                        "[Image].[ModifiedDT], " & _
                        "[Image].[VersionNo], " & _
                        "[Image].[ContactID], " & _
                        "[Image].[title],  " & _
                        "[Image].[medium],  " & _
                        "[Image].[size] " & _
                        "FROM [Page],[PageImage],[Image] " & _
                        "WHERE [Page].[PageID] = [PageImage].[PageID] " & _
                        "AND [Image].[ImageID] = [PageImage].[ImageID] " & _
                        "AND [Page].[CompanyID] = " & CompanyID & " " & _
                        "AND [Page].[PageID] = " & PageID & " " & _
                        "AND [Page].[GroupID] >= " & GroupID & " " & _
                        "AND [Page].[Active] = TRUE " & _
                        "AND [Image].[Active]= TRUE " & _
                        "ORDER BY [PageImage].[PageImagePosition] "

        Return mhDB.GetDataTable(strSQL, "SiteMap")

    End Function
    Public Shared Function GetSiteMap(ByVal sSortBy As String, ByVal CompanyID As String, ByVal GroupID As String) As DataTable
        Dim strSQL As String
        ' SiteMap only shows Active Pages and Articles for Guests
        ' 1 - PageID
        ' 2 - PageName
        ' 3 - PageTitle
        ' 4 - PageDescription
        ' 5 - ParentPageID
        ' 6 - PageSource - Source of Record "Article","Page","Image"
        ' 7 - PageKeywords
        ' 8 - TransferURL (only if not equal to "NO FILE NAME")
        ' 9 - PageFileName (use for TransferURL if TransferURL =  "NO FILE NAME")
        ' 10- ModifiedDate (Use StartDT for articles)
        ' 11 - ArticleID
        ' 12 - Active Flag
        ' 13 - PageOrder
        ' 14 - SiteCategoryID
        ' 15 - SiteCategoryName
        ' 16 - SiteCategoryGroupName
        ' 17 - PageTypeCD
        strSQL = _
        ("SELECT Page.PageID, " & _
            "Article.Title as PageName,  " & _
            "Article.Title as PageTitle,  " & _
            "Article.Description,  " & _
            "Page.ParentPageID,  " & _
            "'Article' AS PageSource,  " & _
            "Page.PageKeywords,  " & _
            "IIf(pagetype.PageFileName Is Null,""" & mhConfig.mhWebHome & "mhmain.aspx"",pagetype.PageFileName) AS TransferURL, " & _
            "Page.PageFileName, " & _
            "Article.StartDT as ModifiedDT, " & _
            "Article.ArticleID, " & _
            "Article.Active, " & _
            "Page.PageOrder,  " & _
            "Page.SiteCategoryID, " & _
            "' ' as SiteCategoryName, " & _
            "' ' as SiteCategoryGroupName, " & _
            "IIf(pagetype.PageTypeCD Is Null,'Article',pagetype.PageTypeCD) AS PageTypeCD " & _
          "FROM (Page RIGHT JOIN Article ON Page.PageID = Article.PageID)  " & _
            " LEFT JOIN pagetype ON Page.PageTypeID = pagetype.PageTypeID  " & _
          "WHERE (((Article.CompanyID)=" & CompanyID & " )  " & _
            "AND ((Article.Active=TRUE)) " & _
            "AND ((Page.PageName)<>[Article].[Title])  " & _
            "AND ((Page.GroupID)>= " & GroupID & " )  " & _
            "AND ((Page.CompanyID)=" & CompanyID & " )) OR (((Article.CompanyID)=" & CompanyID & " )  " & _
            "AND ((Page.PageName) Is Null)  " & _
            "AND ((Page.GroupID) Is Null) AND ((Page.CompanyID) Is Null)) " & _
            "union SELECT Page.PageID, " & _
            "Page.PageName, " & _
            "Page.PageTitle, " & _
            "Page.PageDescription, " & _
            "Page.ParentPageID, " & _
            "'Page' AS RecordSource, " & _
            "Page.PageKeywords, " & _
            "pagetype.PageFileName AS TransferURL, " & _
            "Page.PageFileName, " & _
            "Page.ModifiedDT, " & _
            "Null AS ArticleID, " & _
            "Page.Active, " & _
            "IIf([Page].[PageOrder] Is Null,0,[Page].[PageOrder]), " & _
            "Page.SiteCategoryID, " & _
            "SiteCategory.CategoryName, " & _
            "IIf(SiteCategoryGroup.SiteCategoryGroupNM Is Null,SiteCategoryGroup_Page.SiteCategoryGroupNM,SiteCategoryGroup.SiteCategoryGroupNM) AS SiteCategoryGroupNM, " & _
            "IIf(pagetype.PageTypeCD Is Null,'Article',pagetype.PageTypeCD) AS PageTypeCD " & _
            "FROM (((Page LEFT JOIN PageType ON Page.PageTypeID = PageType.PageTypeID) LEFT JOIN SiteCategory ON Page.SiteCategoryID = SiteCategory.SiteCategoryID) LEFT JOIN SiteCategoryGroup ON SiteCategory.SiteCategoryGroupID = SiteCategoryGroup.SiteCategoryGroupID) LEFT JOIN SiteCategoryGroup AS SiteCategoryGroup_Page ON Page.SiteCategoryGroupID = SiteCategoryGroup_Page.SiteCategoryGroupID " & _
            "WHERE (((Page.Active)=True) " & _
            "AND ((Page.GroupID)>=" & GroupID & ") " & _
            "AND ((Page.CompanyID)=" & CompanyID & ")) " & _
           " union SELECT Page.PageID," & _
            " Page.PageName & '-' & Image.ImageName AS PageName, " & _
            " Page.PageName & '-' & Image.ImageName AS PageTitle, " & _
            "Image.ImageDescription, " & _
            "Page.ParentPageID, " & _
            "'Image' AS PageSource, " & _
            "Page.PageKeywords, " & _
            "IIf([pagetype].[PageFileName] Is Null,""" & mhConfig.mhWebHome & "mhmain.aspx"",[pagetype].[PageFileName]) AS TransferURL, " & _
            "Page.PageFileName, " & _
            "Page.ModifiedDT, " & _
            "Image.ImageID, " & _
            "Image.Active," & _
            "Page.PageOrder,  " & _
            "Page.SiteCategoryID, " & _
            "' ' as SiteCategoryName, " & _
            "' ' as SiteCategoryGroupName, " & _
            "IIf(pagetype.PageTypeCD Is Null,'Article',pagetype.PageTypeCD) AS PageTypeCD " & _
            "FROM [Image] " & _
            "INNER JOIN ((Page LEFT JOIN pagetype ON Page.PageTypeID = pagetype.PageTypeID) " & _
            "INNER JOIN PageImage ON Page.PageID = PageImage.PageID) ON Image.ImageID = PageImage.ImageID " & _
            "WHERE (((Image.CompanyID)=" & CompanyID & ") " & _
            "AND ((Image.Active=TRUE)) " & _
            "AND ((Page.PageName)<>[Image].[ImageName]) " & _
            "AND ((Page.GroupID)>= " & GroupID & ") " & _
            "AND ((Page.CompanyID)=" & CompanyID & ")) " & _
            "OR (((Image.CompanyID)=" & CompanyID & ") " & _
            "AND ((Page.PageName) Is Null) " & _
            "AND ((Page.GroupID) Is Null) " & _
            "AND ((Page.CompanyID) Is Null)) " & _
            "union " & _
            "SELECT SiteCategory.SiteCategoryID as PageID, " & _
            "SiteCategory.CategoryName as PageName, " & _
            "SiteCategory.CategoryTitle as PageTitle, " & _
            "SiteCategory.CategoryDescription as Description, " & _
            "SiteCategory.ParentCategoryID as ParentPageID , " & _
            "'Category' as PageSource, " & _
            "SiteCategory.CategoryKeywords as PageKeywords, " & _
            "IIf(SiteCategoryType.SiteCategoryFileName Is Null,""" & mhConfig.mhWebHome & "mhmain.aspx"",SiteCategoryType.SiteCategoryFileName) AS TransferURL, " & _
            "SiteCategory.CategoryFileName as PageFileName, " & _
            "now() as ModifiedDT, " & _
            "SiteCategoryGroup.SiteCategoryGroupID as ArticleID, " & _
            "TRUE as Active, " & _
            "SiteCategory.GroupOrder as PageOrder, " & _
            "SiteCategory.SiteCategoryID, " & _
            "SiteCategory.CategoryName  as SiteCategoryName, " & _
            "SiteCategoryGroup.SiteCategoryGroupNM as SiteCategoryGroupName, " & _
            "'SITECAT' " & _
            "FROM SiteCategoryGroup, SiteCategoryType, SiteCategory,Company " & _
            "WHERE SiteCategoryType.SiteCategoryTypeID = SiteCategory.SiteCategoryTypeID " & _
            "AND SiteCategoryGroup.SiteCategoryGroupID = SiteCategory.SiteCategoryGroupID " & _
            "AND SiteCategoryType.SiteCategoryTypeID = Company.SiteCategoryTypeID " & _
            "AND Company.CompanyID =" & CompanyID & " ")

        If CInt(GroupID) < 3 Then
            strSQL = _
            ("SELECT Page.PageID, " & _
    "Article.Title AS PageName,  " & _
    "Article.Title AS PageTitle,  " & _
    "Article.Description,  " & _
    "Page.ParentPageID,  " & _
    "'Article' AS PageSource,  " & _
    "Page.PageKeywords,  " & _
    "IIf(pagetype.PageFileName Is Null,""" & mhConfig.mhWebHome & "mhmain.aspx"",pagetype.PageFileName) AS TransferURL, " & _
    "Page.PageFileName, " & _
    "Article.StartDT as ModifiedDT, " & _
    "Article.ArticleID, " & _
    "Article.Active, " & _
            "Page.PageOrder,  " & _
            "Page.SiteCategoryID, " & _
            "' ' as SiteCategoryName, " & _
            "' ' as SiteCategoryGroupName, " & _
            "IIf(pagetype.PageTypeCD Is Null,'Article',pagetype.PageTypeCD) AS PageTypeCD " & _
  "FROM (Page RIGHT JOIN Article ON Page.PageID = Article.PageID)  " & _
    " LEFT JOIN pagetype ON Page.PageTypeID = pagetype.PageTypeID  " & _
  "WHERE (((Article.CompanyID)=" & CompanyID & " )  " & _
    "AND ((Page.PageName)<>[Article].[Title])  " & _
    "AND ((Page.GroupID)>= " & GroupID & " )  " & _
    "AND ((Page.CompanyID)=" & CompanyID & " )) OR (((Article.CompanyID)=" & CompanyID & " )  " & _
    "AND ((Page.PageName) Is Null)  " & _
    "AND ((Page.GroupID) Is Null) AND ((Page.CompanyID) Is Null)) " & _
            "union SELECT " & _
            "Page.PageID, " & _
            "Page.PageName, " & _
            "Page.PageTitle, " & _
            "Page.PageDescription, " & _
            "Page.ParentPageID, " & _
            "'Page' AS RecordSource, " & _
            "Page.PageKeywords, " & _
            "pagetype.PageFileName AS TransferURL, " & _
            "Page.PageFileName, " & _
            "Page.ModifiedDT, " & _
            "Null AS ArticleID, " & _
            "Page.Active, " & _
            "IIf([Page].[PageOrder] Is Null,0,[Page].[PageOrder]), " & _
            "Page.SiteCategoryID, " & _
            "SiteCategory.CategoryName, " & _
            "IIf(SiteCategoryGroup.SiteCategoryGroupNM Is Null,SiteCategoryGroup_Page.SiteCategoryGroupNM,SiteCategoryGroup.SiteCategoryGroupNM) AS SiteCategoryGroupNM, " & _
            "IIf(pagetype.PageTypeCD Is Null,'Article',pagetype.PageTypeCD) AS PageTypeCD " & _
            "FROM (((Page LEFT JOIN PageType ON Page.PageTypeID = PageType.PageTypeID) LEFT JOIN SiteCategory ON Page.SiteCategoryID = SiteCategory.SiteCategoryID) LEFT JOIN SiteCategoryGroup ON SiteCategory.SiteCategoryGroupID = SiteCategoryGroup.SiteCategoryGroupID) LEFT JOIN SiteCategoryGroup AS SiteCategoryGroup_Page ON Page.SiteCategoryGroupID = SiteCategoryGroup_Page.SiteCategoryGroupID " & _
            "WHERE (((Page.GroupID)>=" & GroupID & ") " & _
            "AND ((Page.CompanyID)=" & CompanyID & ")) " & _
   " union SELECT Page.PageID," & _
    "Page.PageName & '-' & Image.ImageName AS PageName, " & _
    "Page.PageName & '-' & Image.ImageName AS PageTitle, " & _
    "Image.ImageDescription, " & _
    "Page.ParentPageID, " & _
    "'Image' AS PageSource, " & _
    "Page.PageKeywords, " & _
    "IIf([pagetype].[PageFileName] Is Null,""" & mhConfig.mhWebHome & "mhmain.aspx"",[pagetype].[PageFileName]) AS TransferURL, " & _
    "Page.PageFileName, " & _
    "Page.ModifiedDT, " & _
    "Image.ImageID, " & _
    "Image.Active," & _
            "Page.PageOrder,  " & _
            "Page.SiteCategoryID, " & _
            "' ' as SiteCategoryName, " & _
            "' ' as SiteCategoryGroupName, " & _
            "IIf(pagetype.PageTypeCD Is Null,'Image',pagetype.PageTypeCD) AS PageTypeCD " & _
    "FROM [Image] " & _
    "INNER JOIN ((Page LEFT JOIN pagetype ON Page.PageTypeID = pagetype.PageTypeID) " & _
    "INNER JOIN PageImage ON Page.PageID = PageImage.PageID) ON Image.ImageID = PageImage.ImageID " & _
    "WHERE (((Image.CompanyID)=" & CompanyID & ") " & _
    "AND ((Page.PageName)<>[Image].[ImageName]) " & _
    "AND ((Page.GroupID)>= " & GroupID & ") " & _
    "AND ((Page.CompanyID)=" & CompanyID & ")) " & _
    "OR (((Image.CompanyID)=" & CompanyID & ") " & _
    "AND ((Page.PageName) Is Null) " & _
    "AND ((Page.GroupID) Is Null) " & _
    "AND ((Page.CompanyID) Is Null)) " & _
                "union " & _
            "SELECT SiteCategory.SiteCategoryID as PageID, " & _
            "SiteCategory.CategoryName as PageName, " & _
            "SiteCategory.CategoryTitle as PageTitle, " & _
            "SiteCategory.CategoryDescription as Description, " & _
            "SiteCategory.ParentCategoryID as ParentPageID , " & _
            "'Category' as PageSource, " & _
            "SiteCategory.CategoryKeywords as PageKeywords, " & _
            "IIf(SiteCategoryType.SiteCategoryFileName Is Null,""" & mhConfig.mhWebHome & "mhmain.aspx"",SiteCategoryType.SiteCategoryFileName) AS TransferURL, " & _
            "SiteCategory.CategoryFileName as PageFileName, " & _
            "now() as ModifiedDT, " & _
            "SiteCategoryGroup.SiteCategoryGroupID as ArticleID, " & _
            "TRUE as Active, " & _
            "SiteCategory.GroupOrder as PageOrder, " & _
            "SiteCategory.SiteCategoryID, " & _
            "SiteCategory.CategoryName  as SiteCategoryName, " & _
            "SiteCategoryGroup.SiteCategoryGroupNM as SiteCategoryGroupName, " & _
            "'SITECAT' as PageTypeCD " & _
            "FROM SiteCategoryGroup, SiteCategoryType, SiteCategory,Company " & _
            "WHERE SiteCategoryType.SiteCategoryTypeID = SiteCategory.SiteCategoryTypeID " & _
            "AND SiteCategoryGroup.SiteCategoryGroupID = SiteCategory.SiteCategoryGroupID " & _
            "AND SiteCategoryType.SiteCategoryTypeID = Company.SiteCategoryTypeID " & _
            "AND Company.CompanyID =" & CompanyID & " ")


        End If
        ' Order results by PageOrder
        Select Case sSortBy
            Case "ORDER"
                strSQL = strSQL & "ORDER BY 13 ASC,5 ASC,2 ASC,10 ASC "
            Case "MODIFIED"
                strSQL = strSQL & "ORDER BY 10 DESC "
            Case "NAME"
                strSQL = strSQL & "ORDER BY 2 ASC "
            Case "ARTICLE"
                strSQL = strSQL & "ORDER BY 2 DESC "
            Case Else
                strSQL = strSQL & "ORDER BY 10 DESC "
        End Select
        Return mhDB.GetDataTable(strSQL, "SiteMap")

    End Function
    Public Shared Function UpdatePageDate(ByVal pageID As String, ByVal mySiteMap As mhSiteMap) As Boolean
        Dim sSQL As String
        Dim iRowsUpdated As Integer = 0
        If Trim(pageID) <> "" Then
            sSQL = "UPDATE Page SET Page.ModifiedDT = Now() WHERE (((Page.PageID)=" & pageID & "));"
            iRowsUpdated = mhDB.RunUpdateSQL(sSQL, "mhDataCon.UpdatePageDate")
        End If
        If iRowsUpdated > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Shared Function GetDropDownList(ByVal ListTable As String, ByVal ListID As String, ByVal ListDisplay As String, ByVal ListWhere As String, ByVal CurrentID As String) As String
        Dim sbList As New StringBuilder("<SELECT name='cb" & ListID & "'><OPTION value=''>Please Select</OPTION>")
        Dim mySQL As String = ("Select " & ListID & ", " & ListDisplay & " from " & ListTable)
        Dim myID As String
        Dim myValue As String
        If Trim(ListWhere) <> "" Then
            mySQL = mySQL & " where " & ListWhere
        End If
        mySQL = mySQL & " order by " & ListDisplay
        Try
            Dim RecConn As New System.Data.OleDb.OleDbConnection(mhConfig.GetSiteDB)
            RecConn.Open()
            Dim command As New System.Data.OleDb.OleDbCommand(mySQL, RecConn)
            Dim reader As System.Data.OleDb.OleDbDataReader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
            While reader.Read
                If reader.IsDBNull(0) Then
                    myID = "NULL"
                Else
                    myID = reader.GetValue(0).ToString
                End If
                If reader.IsDBNull(1) Then
                    myValue = "NULL"
                Else
                    myValue = reader.GetValue(1).ToString
                End If
                sbList.Append("<option value='" & myID & "'")
                If myID = CurrentID Then
                    sbList.Append("  selected ")
                End If
                sbList.Append(">" & myValue & "</option>")
            End While
        Catch
            mhUTIL.AuditLog("DB ERROR - mhwcm.GetDropDownList", mySQL)
        End Try
        sbList.Append("</SELECT>")
        Return sbList.ToString
    End Function
    Public Shared Function GetCompanyList(ByVal CompanyID As String, ByRef SiteDB As String) As String
        Dim sbList As New StringBuilder("<SELECT name='st'><OPTION value=''>Please Select</OPTION>")
        Dim sqlwrk As String = ("SELECT [Company].[CompanyID], [Company].[CompanyName],[Company].[DBString] FROM [Company] ORDER BY [Company].[CompanyName] ASC")
        For Each myRow As DataRow In mhDB.GetDataTable(sqlwrk, "CompanyList").Rows
            sbList.Append("<option value='" & mhUTIL.GetDBString(myRow.Item(0)) & "'")
            If mhUTIL.GetDBString(myRow.Item(0)) = CompanyID Then
                sbList.Append(" selected ")
            End If
            sbList.Append(">" & mhUTIL.GetDBString(myRow.Item(1)) & "</option>")
        Next
        sbList.Append("</SELECT>")
        Return sbList.ToString
    End Function
    Public Shared Function GetCompanyContactList(ByVal sContactID As String, ByVal sFieldName As String, ByVal bRequired As Boolean, ByVal CompanyID As String, ByVal mySiteMap As mhSiteMap) As String
        Dim sqlwrk As String
        Dim mydt As DataTable
        Dim x_ContactIDList As String
        If (bRequired) Then
            x_ContactIDList = "<SELECT name='" & sFieldName & "'>"
        Else
            x_ContactIDList = "<SELECT name='" & sFieldName & "'><OPTION value=''>Please Select</OPTION>"
        End If
        sqlwrk = "SELECT ContactID, PrimaryContact & ' (' & LogonName & ')' FROM Contact where Contact.CompanyID=" & CompanyID & " ORDER BY PrimaryContact "
        mydt = mhDB.GetDataTable(sqlwrk, "mhwcm.GetCompanyContactList")
        For Each row As DataRow In mydt.Rows
            x_ContactIDList = x_ContactIDList & "<OPTION value='" & mhUTIL.GetDBString(row(0)) & "'"
            If mhUTIL.GetDBString(row(0)) = sContactID Then
                x_ContactIDList = x_ContactIDList & " selected"
            End If
            x_ContactIDList = x_ContactIDList & ">" & mhUTIL.GetDBString(row(1)) & "</option>"
        Next
        x_ContactIDList = x_ContactIDList & "</SELECT>"
        Return x_ContactIDList
    End Function
    Public Shared Function GetTemplateList(ByVal sTemplatePrefix As String, ByVal bRequired As Boolean) As String
        Dim stList As String = ""
        Dim sqlwrk As String = ""
        Dim myDT As DataTable
        If (bRequired) Then
            stList = "<SELECT name='st'>"
        Else
            stList = "<SELECT name='st'><OPTION value=''>Please Select</OPTION>"
        End If
        sqlwrk = "SELECT [SiteTemplate].[TemplatePrefix], [SiteTemplate].[Name] FROM [SiteTemplate] ORDER BY [SiteTemplate].[Name] ASC"
        myDT = mhDB.GetDataTable(sqlwrk, "mhwcm.GetTemplateList")
        For Each row As DataRow In myDT.Rows
            stList = stList & "<OPTION value='" & mhUTIL.GetDBString(row(0)) & "'"
            If mhUTIL.GetDBString(row(0)) = sTemplatePrefix Then
                stList = stList & " selected"
            End If
            stList = stList & ">" & mhUTIL.GetDBString(row(1)) & "</option>"
        Next
        stList = stList & "</SELECT>"
        Return stList
    End Function

End Class

