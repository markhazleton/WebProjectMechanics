Imports System.Text

Public Class wpmDataCon
    Public Shared Function GetCompanyData(ByVal CompanyID As String) As DataTable
        Return wpmDB.GetDataTable(((String.Format("SELECT Company.CompanyID,  Company.CompanyName,  Company.GalleryFolder,  Company.SiteURL,  Company.SiteTitle,  Company.SiteTemplate,  Company.DefaultSiteTemplate,  Company.HomePageID,  Company.DefaultArticleID,  Company.ActiveFL,  Company.UseBreadCrumbURL,  Company.SiteCategoryTypeID,  Company.DefaultPaymentTerms,  Company.DefaultInvoiceDescription,  Company.City,  Company.StateOrProvince,  Company.PostalCode,  Company.Country,  Company.FromEmail, Company.SMTP, Company.Component, SiteCategoryType.SiteCategoryTypeNM,  SiteCategoryType.SiteCategoryTypeDS,  SiteCategoryType.DefaultSiteCategoryID   FROM SiteCategoryType RIGHT JOIN Company ON SiteCategoryType.[SiteCategoryTypeID] = Company.[SiteCategoryTypeID] WHERE Company.CompanyID={0} ", CompanyID))), "GetCompanyData")
    End Function
    Public Shared Function GetSiteGroupList() As DataTable
        Return wpmDB.GetDataTable(("SELECT SiteCategoryGroup.[SiteCategoryGroupID], SiteCategoryGroup.[SiteCategoryGroupNM], SiteCategoryGroup.[SiteCategoryGroupDS], SiteCategoryGroup.[SiteCategoryGroupOrder]FROM SiteCategoryGroup;"), "SiteCategoryGroup")
    End Function
    Public Shared Function GetSiteLinks(ByVal CompanyID As String) As DataTable
        Return wpmDB.GetDataTable((String.Format("SELECT Link.ID,Link.LinkTypeCD, LinkCategory.Title as LinkCategoryTitle, Link.CategoryID, Link.PageID,Link.Title as LinkTitle, Link.Description, Link.URL, Link.DateAdd, Link.Ranks, Link.Views, Link.UserName, Link.UserID, Link.ASIN, 'Link' as LinkSource, '' as SiteCategoryTypeID, Link.SiteCategoryGroupID, Link.CompanyID FROM Link, LinkCategory where Link.CategoryID = LinkCategory.ID and ( Link.CompanyID ={0} or Link.CompanyID is null) ORDER BY Link.Ranks ", CompanyID)), "mhDataCon.GetSiteLinks")
    End Function
    Public Shared Function GetSiteCategoryLinks(ByVal CompanyID As String, ByVal SiteCategoryTypeID As String) As DataTable
        Return wpmDB.GetDataTable((String.Format("SELECT Link.ID,Link.LinkTypeCD, LinkCategory.Title as LinkCategoryTitle, Link.CategoryID, Link.PageID,Link.Title as LinkTitle, Link.Description, Link.URL, Link.DateAdd, Link.Ranks, Link.Views, Link.UserName, Link.UserID, Link.ASIN, Link.Ranks,  'Link' as LinkSource, '' as SiteCategoryTypeID, Link.SiteCategoryGroupID, Link.CompanyID FROM Link, LinkCategory where Link.CategoryID = LinkCategory.ID and ( Link.CompanyID ={0} or Link.CompanyID is null) union all SELECT SiteLink.ID,SiteLink.LinkTypeCD, LinkCategory.Title as LinkCategoryTitle, SiteLink.CategoryID, 'CAT-' & SiteLink.SiteCategoryID as PageID, SiteLink.Title as LinkTitle, SiteLink.Description, SiteLink.URL, SiteLink.DateAdd, SiteLink.Ranks, SiteLink.Views, SiteLink.UserName, SiteLink.UserID, SiteLink.ASIN, SiteLink.Ranks,  'SiteLink' as LinkSource, SiteLink.SiteCategoryTypeID, SiteLink.SiteCategoryGroupID, SiteLink.CompanyID FROM SiteLink, LinkCategory where SiteLink.CategoryID = LinkCategory.ID and ( SiteLink.CompanyID ={0} or SiteLink.CompanyID is null) and ( SiteLink.SiteCategoryTypeID ={1} or SiteLink.SiteCategoryTypeID is null) ORDER BY 15 ", CompanyID, SiteCategoryTypeID)), "mhDataCon.GetSiteLinks")
    End Function
    Public Shared Function GetLinkCategoryList() As DataTable
        Return wpmDB.GetDataTable("SELECT LinkCategory.ID,  LinkCategory.Title,  LinkCategory.ParentID,  LinkCategory.Description,  LinkCategory.PageID,  0 AS CountOfID FROM LinkCategory ", "mhDataCon.GetLinkCategoryList")
    End Function

    Public Shared Function GetCompanySiteTypeParameterList(ByVal CompanyID As String, ByVal SiteCategoryTypeID As String) As DataTable
        If SiteCategoryTypeID = String.Empty Then
            SiteCategoryTypeID = "0"
        End If
        Dim strSQL As String = String.Format("SELECT 'CompanySiteTypeParameter' AS RecordSource, IIf(CompanySiteTypeParameter.CompanySiteTypeParameterID Is Null,0,10)+IIf(CompanySiteTypeParameter.SiteCategoryGroupID Is Null,0,1)+IIf(CompanySiteTypeParameter.SiteCategoryID Is Null,0,1)+IIf(CompanySiteTypeParameter.CompanyID Is Null,0,1) AS PrimarySort, CompanySiteTypeParameter.CompanyID, CompanySiteTypeParameter.SiteParameterTypeID, CompanySiteTypeParameter.SortOrder, CompanySiteTypeParameter.ParameterValue, iif(CompanySiteTypeParameter.SiteCategoryID is null,null,'CAT-' & CompanySiteTypeParameter.SiteCategoryID) as PageID, CompanySiteTypeParameter.SiteCategoryGroupID, CompanySiteTypeParameter.SiteCategoryTypeID, SiteParameterType.SiteParameterTypeNM, SiteParameterType.SiteParameterTypeDS,CompanySiteTypeParameter.CompanySiteTypeParameterID FROM SiteParameterType INNER JOIN CompanySiteTypeParameter ON SiteParameterType.[SiteParameterTypeID] = CompanySiteTypeParameter.[SiteParameterTypeID] WHERE (((CompanySiteTypeParameter.CompanyID)={0}) AND ((CompanySiteTypeParameter.SiteCategoryTypeID)={1})) OR (((CompanySiteTypeParameter.CompanyID) Is Null) AND ((CompanySiteTypeParameter.SiteCategoryTypeID)={1})) ORDER BY 2 desc,3 DESC, 7 DESC ", CompanyID, SiteCategoryTypeID)
        Return wpmDB.GetDataTable(strSQL, "GetSiteTypeParameter")
    End Function
    Public Shared Function GetSiteParameterList(ByVal CompanyID As String, ByVal SiteCategoryTypeID As String) As DataTable
        If SiteCategoryTypeID = String.Empty Then
            SiteCategoryTypeID = "0"
        End If
        Dim strSQL As String = String.Format("SELECT 'CompanySiteParameter' AS RecordSource, iif(CompanySiteParameter.CompanySiteParameterID is null,0,10)+iif(CompanySiteParameter.SiteCategoryGroupID is null,0,1)+iif(CompanySiteParameter.PageID is null,0,1)+iif(CompanySiteParameter.CompanyID is null,0,1) as PrimarySort, CompanySiteParameter.CompanyID, CompanySiteParameter.SiteParameterTypeID, CompanySiteParameter.SortOrder, CompanySiteParameter.ParameterValue, CompanySiteParameter.PageID, CompanySiteParameter.SiteCategoryGroupID, '' AS SiteCategoryTypeID, SiteParameterType.SiteParameterTypeNM, SiteParameterType.SiteParameterTypeDS,CompanySiteParameter.CompanySiteParameterID FROM SiteParameterType INNER JOIN CompanySiteParameter ON SiteParameterType.SiteParameterTypeID = CompanySiteParameter.SiteParameterTypeID WHERE(((CompanySiteParameter.CompanyID) = {0} Or (CompanySiteParameter.CompanyID) Is Null)) ORDER BY 2 desc,3 DESC, 7 DESC ", CompanyID)
        Return wpmDB.GetDataTable(strSQL, "GetSiteTypeParameter")
    End Function

    Public Shared Function GetParameterTypeList(ByVal SiteCategoryTypeID As String) As DataTable
        If SiteCategoryTypeID = String.Empty Then
            SiteCategoryTypeID = "0"
        End If
        Return wpmDB.GetDataTable("SELECT 'SiteParameterType' AS RecordSource, 0 AS PrimarySort, Null AS CompanyID, Null AS SiteParameterTypeID, SiteParameterType.SiteParameterTypeOrder, SiteParameterType.SiteParameterTemplate, Null AS SiteCategoryID, Null AS SiteCategoryGroupID, Null AS SiteCategoryTypeID, SiteParameterType.SiteParameterTypeNM, SiteParameterType.SiteParameterTypeDS,0 as CompanySiteParameterID FROM(SiteParameterType) ORDER BY 2 desc,3 DESC, 7 DESC ", "GetSiteTypeParameter")
    End Function
    Public Shared Function GetSiteTypeParameter(ByVal CompanySiteTypeParameterID As Integer) As DataTable
        Return wpmDB.GetDataTable(String.Format("SELECT CompanySiteTypeParameter.CompanySiteTypeParameterID,  CompanySiteTypeParameter.CompanyID,  CompanySiteTypeParameter.SiteParameterTypeID,  CompanySiteTypeParameter.SortOrder,  CompanySiteTypeParameter.ParameterValue,  CompanySiteTypeParameter.SiteCategoryGroupID,  CompanySiteTypeParameter.SiteCategoryID,  CompanySiteTypeParameter.ParameterValue,  CompanySiteTypeParameter.SiteCategoryTypeID, SiteParameterType.SiteParameterTypeID,  SiteParameterType.SiteParameterTypeNM,  SiteParameterType.SiteParameterTypeDS,  SiteParameterType.SiteParameterTypeOrder,  SiteParameterType.SiteParameterTemplate  FROM SiteParameterType  LEFT JOIN CompanySiteTypeParameter ON SiteParameterType.[SiteParameterTypeID] = CompanySiteTypeParameter.[SiteParameterTypeID]  WHERE  CompanySiteTypeParameter.CompanySiteTypeParameterID={0} ", CompanySiteTypeParameterID), "mhDataCon.GetSiteTypeParameterList")
    End Function
    Public Shared Function GetImageList(ByVal CompanyID As String) As DataTable
        Dim strSQL As String = String.Format("SELECT Image.[ImageID], Image.[ImageName], Image.[ImageFileName], Image.[ImageThumbFileName], Image.[ImageDescription], Image.[ImageComment], Image.[ImageDate], Image.[Active], Image.[ModifiedDT], Image.[VersionNo], Image.[ContactID], Image.[CompanyID], Image.[Title], Image.[Medium], Image.[Size], Image.[Price], Image.[Color], Image.[Subject], Image.[Sold] FROM [Image] where [CompanyID]={0} ", CompanyID)
        Return wpmDB.GetDataTable(strSQL, "GetImageList")
    End Function
    Public Shared Function GetPageAliasList(ByVal CompanyID As String) As DataTable
        Dim strSQL As String = (String.Format("SELECT PageAliasID, PageURL, TargetURL, AliasType from PageAlias where [CompanyID]={0} ", CompanyID))
        Return wpmDB.GetDataTable(strSQL, "GetPageAliasList")
    End Function
    Public Shared Function GetUnlinkedImageList(ByVal CompanyID As String) As DataTable
        Dim strSQL As String = (String.Format("SELECT Image.[ImageID], Image.[ImageName], Image.[ImageFileName], Image.[ImageThumbFileName], Image.[ImageDescription], Image.[ImageComment], Image.[ImageDate], Image.[Active], Image.[ModifiedDT], Image.[VersionNo], Image.[ContactID], Image.[CompanyID], Image.[Title], Image.[Medium], Image.[Size], Image.[Price], Image.[Color], Image.[Subject], Image.[Sold] FROM [Image]  LEFT JOIN [PageImage] ON [Image].[ImageID] = [PageImage].[ImageID]  WHERE [PageImage].[PageID] Is Null and [CompanyID]={0} ", CompanyID))
        Return wpmDB.GetDataTable(strSQL, "GetImageList")
    End Function
    Public Shared Function GetImageByID(ByVal ImageID As String) As DataTable
        Dim strSQL As String = (String.Format("SELECT Image.[ImageID], Image.[ImageName], Image.[ImageFileName], Image.[ImageThumbFileName], Image.[ImageDescription], Image.[ImageComment], Image.[ImageDate], Image.[Active], Image.[ModifiedDT], Image.[VersionNo], Image.[ContactID], Image.[CompanyID], Image.[Title], Image.[Medium], Image.[Size], Image.[Price], Image.[Color], Image.[Subject], Image.[Sold] FROM [Image] where [ImageID]={0} ", ImageID))
        Return wpmDB.GetDataTable(strSQL, "GetImageByID")
    End Function
    Public Shared Function GetPageImage(ByVal CompanyID As String) As DataTable
        Dim strSQL As String = String.Format("SELECT [Page].[PageID], [Page].[PageName], [Page].[PageDescription], [Page].[PageKeywords], [Page].[ImagesPerRow], [Page].[RowsPerPage], [PageImage].[PageImagePosition], [Image].[ImageID], [Image].[ImageName], [Image].[ImageFileName], [Image].[ImageThumbFileName], [Image].[ImageDescription], [Image].[ImageComment], [Image].[ImageDate], [Image].[ModifiedDT], [Image].[VersionNo], [Image].[ContactID], [Image].[title],  [Image].[medium],  [Image].[size] FROM [Page],[PageImage],[Image] WHERE [Page].[PageID] = [PageImage].[PageID] AND [Image].[ImageID] = [PageImage].[ImageID] AND [Page].[CompanyID] = {0} ORDER BY [PageImage].[PageImagePosition] ", CompanyID)
        Return wpmDB.GetDataTable(strSQL, "SiteMap")
    End Function
    Public Shared Function GetPageImage(ByVal PageID As String, ByVal CompanyID As String, ByVal GroupID As String) As DataTable
        Return wpmDB.GetDataTable(String.Format("SELECT [Page].[PageID], [Page].[PageName], [Page].[PageDescription], [Page].[PageKeywords], [Page].[ImagesPerRow], [Page].[RowsPerPage], [Page].[PageFileName], [PageImage].[PageImagePosition], [Image].[ImageID], [Image].[ImageName], [Image].[ImageFileName], [Image].[ImageThumbFileName], [Image].[ImageDescription], [Image].[ImageComment], [Image].[ImageDate], [Image].[ModifiedDT], [Image].[VersionNo], [Image].[ContactID], [Image].[title],  [Image].[Price], [Image].[Color], [Image].[Subject], [Image].[Sold], [Image].[medium],  [Image].[size] FROM [Page],[PageImage],[Image] WHERE [Page].[PageID] = [PageImage].[PageID] AND [Image].[ImageID] = [PageImage].[ImageID] AND [Page].[CompanyID] = {0} AND [Page].[PageID] = {1} AND [Page].[GroupID] >= {2} AND [Page].[Active] = TRUE AND [Image].[Active]= TRUE ORDER BY [PageImage].[PageImagePosition] ", _
                                      CompanyID, _
                                      PageID, _
                                      GroupID), "SiteMap")

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

        If CInt(GroupID) < 3 Then
            strSQL = (String.Format("SELECT Page.PageID, Article.Title AS PageName,  Article.Title AS PageTitle,  Article.Description,  Page.ParentPageID,  'Article' AS PageSource,  Page.PageKeywords,  IIf(pagetype.PageFileName Is Null,'/default.aspx',pagetype.PageFileName) AS TransferURL, Page.PageFileName, Article.StartDT as ModifiedDT, Article.ArticleID, Article.Active, Page.PageOrder,  Page.SiteCategoryID, ' ' as SiteCategoryName, ' ' as SiteCategoryGroupName, ' ' as SiteCategoryGroupID, IIf(pagetype.PageTypeCD Is Null,'Article',pagetype.PageTypeCD) AS PageTypeCD FROM (Page RIGHT JOIN Article ON Page.PageID = Article.PageID)   LEFT JOIN pagetype ON Page.PageTypeID = pagetype.PageTypeID  WHERE (((Article.CompanyID)={0} )  AND ((Page.PageName)<>[Article].[Title])  AND ((Page.GroupID)>= {1} )  AND ((Page.CompanyID)={0} )) OR (((Article.CompanyID)={0} )  AND ((Page.PageName) Is Null)  AND ((Page.GroupID) Is Null) AND ((Page.CompanyID) Is Null)) union SELECT Page.PageID, Page.PageName, Page.PageTitle, Page.PageDescription, Page.ParentPageID, 'Page' AS RecordSource, Page.PageKeywords, pagetype.PageFileName AS TransferURL, Page.PageFileName, Page.ModifiedDT, Null AS ArticleID, Page.Active, IIf([Page].[PageOrder] Is Null,0,[Page].[PageOrder]), Page.SiteCategoryID, SiteCategory.CategoryName, IIf(SiteCategoryGroup.SiteCategoryGroupNM Is Null,SiteCategoryGroup_Page.SiteCategoryGroupNM,SiteCategoryGroup.SiteCategoryGroupNM) AS SiteCategoryGroupNM, IIf(SiteCategoryGroup.SiteCategoryGroupID Is Null,SiteCategoryGroup_Page.SiteCategoryGroupID,SiteCategoryGroup.SiteCategoryGroupID) AS SiteCategoryGroupID, IIf(pagetype.PageTypeCD Is Null,'Article',pagetype.PageTypeCD) AS PageTypeCD FROM (((Page LEFT JOIN PageType ON Page.PageTypeID = PageType.PageTypeID) LEFT JOIN SiteCategory ON Page.SiteCategoryID = SiteCategory.SiteCategoryID) LEFT JOIN SiteCategoryGroup ON SiteCategory.SiteCategoryGroupID = SiteCategoryGroup.SiteCategoryGroupID) LEFT JOIN SiteCategoryGroup AS SiteCategoryGroup_Page ON Page.SiteCategoryGroupID = SiteCategoryGroup_Page.SiteCategoryGroupID WHERE (((Page.GroupID)>={1}) AND ((Page.CompanyID)={0}))  union SELECT Page.PageID,Page.PageName & '-' & Image.ImageName AS PageName, Page.PageName & '-' & Image.ImageName AS PageTitle, Image.ImageDescription, Page.ParentPageID, 'Image' AS PageSource, Page.PageKeywords, IIf([pagetype].[PageFileName] Is Null,'/default.aspx',[pagetype].[PageFileName]) AS TransferURL, Page.PageFileName, Page.ModifiedDT, Image.ImageID, Image.Active,Page.PageOrder,  Page.SiteCategoryID, ' ' as SiteCategoryName, ' ' as SiteCategoryGroupName, ' ' as SiteCategoryGroupID, IIf(pagetype.PageTypeCD Is Null,'Image',pagetype.PageTypeCD) AS PageTypeCD FROM [Image] INNER JOIN ((Page LEFT JOIN pagetype ON Page.PageTypeID = pagetype.PageTypeID) INNER JOIN PageImage ON Page.PageID = PageImage.PageID) ON Image.ImageID = PageImage.ImageID WHERE (((Image.CompanyID)={0}) AND ((Page.PageName)<>[Image].[ImageName]) AND ((Page.GroupID)>= {1}) AND ((Page.CompanyID)={0})) OR (((Image.CompanyID)={0}) AND ((Page.PageName) Is Null) AND ((Page.GroupID) Is Null) AND ((Page.CompanyID) Is Null)) union SELECT SiteCategory.SiteCategoryID as PageID, SiteCategory.CategoryName as PageName, SiteCategory.CategoryTitle as PageTitle, SiteCategory.CategoryDescription as Description, SiteCategory.ParentCategoryID as ParentPageID , 'Category' as PageSource, SiteCategory.CategoryKeywords as PageKeywords, IIf(SiteCategoryType.SiteCategoryFileName Is Null,'/default.aspx',SiteCategoryType.SiteCategoryFileName) AS TransferURL, SiteCategory.CategoryFileName as PageFileName, now() as ModifiedDT, NULL as ArticleID, TRUE as Active, SiteCategory.GroupOrder as PageOrder, SiteCategory.SiteCategoryID, SiteCategory.CategoryName  as SiteCategoryName, SiteCategoryGroup.SiteCategoryGroupNM as SiteCategoryGroupName, SiteCategoryGroup.SiteCategoryGroupID as SiteCategoryGroupID, 'SITECAT' as PageTypeCD FROM SiteCategoryGroup, SiteCategoryType, SiteCategory,Company WHERE SiteCategoryType.SiteCategoryTypeID = SiteCategory.SiteCategoryTypeID AND SiteCategoryGroup.SiteCategoryGroupID = SiteCategory.SiteCategoryGroupID AND SiteCategoryType.SiteCategoryTypeID = Company.SiteCategoryTypeID AND Company.CompanyID ={0} ", CompanyID, GroupID))
        Else
            strSQL = (String.Format("SELECT Page.PageID, Article.Title as PageName,  Article.Title as PageTitle,  Article.Description,  Page.ParentPageID,  'Article' AS PageSource,  Page.PageKeywords,  IIf(pagetype.PageFileName Is Null,'/default.aspx',pagetype.PageFileName) AS TransferURL, Page.PageFileName, Article.StartDT as ModifiedDT, Article.ArticleID, Article.Active, Page.PageOrder,  Page.SiteCategoryID, ' ' as SiteCategoryName, ' ' as SiteCategoryGroupName, ' ' as SiteCategoryGroupID, IIf(pagetype.PageTypeCD Is Null,'Article',pagetype.PageTypeCD) AS PageTypeCD FROM (Page RIGHT JOIN Article ON Page.PageID = Article.PageID)   LEFT JOIN pagetype ON Page.PageTypeID = pagetype.PageTypeID  WHERE (((Article.CompanyID)={0} )  AND ((Article.Active=TRUE)) AND ((Page.PageName)<>[Article].[Title])  AND ((Page.GroupID)>= {1} )  AND ((Page.CompanyID)={0} )) OR (((Article.CompanyID)={0} )  AND ((Page.PageName) Is Null)  AND ((Page.GroupID) Is Null) AND ((Page.CompanyID) Is Null)) union SELECT Page.PageID, Page.PageName, Page.PageTitle, Page.PageDescription, Page.ParentPageID, 'Page' AS RecordSource, Page.PageKeywords, pagetype.PageFileName AS TransferURL, Page.PageFileName, Page.ModifiedDT, Null AS ArticleID, Page.Active, IIf([Page].[PageOrder] Is Null,0,[Page].[PageOrder]), Page.SiteCategoryID, SiteCategory.CategoryName, IIf(SiteCategoryGroup.SiteCategoryGroupNM Is Null,SiteCategoryGroup_Page.SiteCategoryGroupNM,SiteCategoryGroup.SiteCategoryGroupNM) AS SiteCategoryGroupNM, IIf(SiteCategoryGroup.SiteCategoryGroupID Is Null,SiteCategoryGroup_Page.SiteCategoryGroupID,SiteCategoryGroup.SiteCategoryGroupID) AS SiteCategoryGroupID, IIf(pagetype.PageTypeCD Is Null,'Article',pagetype.PageTypeCD) AS PageTypeCD FROM (((Page LEFT JOIN PageType ON Page.PageTypeID = PageType.PageTypeID) LEFT JOIN SiteCategory ON Page.SiteCategoryID = SiteCategory.SiteCategoryID) LEFT JOIN SiteCategoryGroup ON SiteCategory.SiteCategoryGroupID = SiteCategoryGroup.SiteCategoryGroupID) LEFT JOIN SiteCategoryGroup AS SiteCategoryGroup_Page ON Page.SiteCategoryGroupID = SiteCategoryGroup_Page.SiteCategoryGroupID WHERE (((Page.Active)=True) AND ((Page.GroupID)>={1}) AND ((Page.CompanyID)={0}))  union SELECT Page.PageID, Page.PageName & '-' & Image.ImageName AS PageName,  Page.PageName & '-' & Image.ImageName AS PageTitle, Image.ImageDescription, Page.ParentPageID, 'Image' AS PageSource, Page.PageKeywords, IIf([pagetype].[PageFileName] Is Null,'/default.aspx',[pagetype].[PageFileName]) AS TransferURL, Page.PageFileName, Page.ModifiedDT, Image.ImageID, Image.Active,Page.PageOrder,  Page.SiteCategoryID, ' ' as SiteCategoryName, ' ' as SiteCategoryGroupName, ' ' as SiteCategoryGroupID, IIf(pagetype.PageTypeCD Is Null,'Article',pagetype.PageTypeCD) AS PageTypeCD FROM [Image] INNER JOIN ((Page LEFT JOIN pagetype ON Page.PageTypeID = pagetype.PageTypeID) INNER JOIN PageImage ON Page.PageID = PageImage.PageID) ON Image.ImageID = PageImage.ImageID WHERE (((Image.CompanyID)={0}) AND ((Image.Active=TRUE)) AND ((Page.PageName)<>[Image].[ImageName]) AND ((Page.GroupID)>= {1}) AND ((Page.CompanyID)={0})) OR (((Image.CompanyID)={0}) AND ((Page.PageName) Is Null) AND ((Page.GroupID) Is Null) AND ((Page.CompanyID) Is Null)) union SELECT SiteCategory.SiteCategoryID as PageID, SiteCategory.CategoryName as PageName, SiteCategory.CategoryTitle as PageTitle, SiteCategory.CategoryDescription as Description, SiteCategory.ParentCategoryID as ParentPageID , 'Category' as PageSource, SiteCategory.CategoryKeywords as PageKeywords, IIf(SiteCategoryType.SiteCategoryFileName Is Null,'/default.aspx',SiteCategoryType.SiteCategoryFileName) AS TransferURL, SiteCategory.CategoryFileName as PageFileName, now() as ModifiedDT, NULL as ArticleID, TRUE as Active, SiteCategory.GroupOrder as PageOrder, SiteCategory.SiteCategoryID, SiteCategory.CategoryName  as SiteCategoryName, SiteCategoryGroup.SiteCategoryGroupNM as SiteCategoryGroupName, SiteCategoryGroup.SiteCategoryGroupID as SiteCategoryGroupID, 'SITECAT' FROM SiteCategoryGroup, SiteCategoryType, SiteCategory,Company WHERE SiteCategoryType.SiteCategoryTypeID = SiteCategory.SiteCategoryTypeID AND SiteCategoryGroup.SiteCategoryGroupID = SiteCategory.SiteCategoryGroupID AND SiteCategoryType.SiteCategoryTypeID = Company.SiteCategoryTypeID AND Company.CompanyID ={0} ", CompanyID, GroupID))
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

        Return wpmDB.GetDataTable(strSQL, "SiteMap")

    End Function

    Public Shared Function UpdateLocationSort(ByRef myLocation As wpmLocation) As Boolean
        Dim sSQL As String
        Dim iRowsUpdated As Integer = 0
        If myLocation.RecordSource = "Page" Then
            If Trim(myLocation.PageID) <> "" Then
                sSQL = String.Format("UPDATE Page SET Page.PageOrder={1} WHERE Page.PageID={0};", myLocation.PageID, myLocation.DefaultOrder)
                iRowsUpdated = wpmDB.RunUpdateSQL(sSQL, "wpmDataCon.UpdatePageDate")
            End If
        Else
            ' Add Code for other Location Types
        End If
        If iRowsUpdated > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Shared Function UpdatePageDate(ByVal pageID As String) As Boolean
        Dim sSQL As String
        Dim iRowsUpdated As Integer = 0
        If Trim(pageID) <> "" Then
            sSQL = String.Format("UPDATE Page SET Page.ModifiedDT = system.datetime.now() WHERE (((Page.PageID)={0}));", pageID)
            iRowsUpdated = wpmDB.RunUpdateSQL(sSQL, "wpmDataCon.UpdatePageDate")
        End If
        If iRowsUpdated > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Shared Function GetDropDownList(ByVal ListTable As String, ByVal ListID As String, ByVal ListDisplay As String, ByVal ListWhere As String, ByVal CurrentID As String) As String
        Dim sbList As New StringBuilder(String.Format("<SELECT name='cb{0}'><OPTION value=''>Please Select</OPTION>", ListID))
        Dim mySQL As String
        Dim myID As String
        Dim myValue As String
        If Trim(ListWhere) <> "" Then
            mySQL = String.Format("{0} where {1}", ("Select " & ListID & ", " & ListDisplay & " from " & ListTable), ListWhere)
        Else
            mySQL = (String.Format("Select {0}, {1} from {2}", ListID, ListDisplay, ListTable))
        End If
        mySQL = String.Format("{0} order by {1}", mySQL, ListDisplay)
        Try
            Using RecConn As New System.Data.OleDb.OleDbConnection(wpmApp.ConnStr)
                RecConn.Open()
                Using command As New System.Data.OleDb.OleDbCommand(mySQL, RecConn)
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
                        sbList.Append(String.Format("<option value='{0}'", myID))
                        If myID = CurrentID Then
                            sbList.Append("  selected ")
                        End If
                        sbList.Append(String.Format(">{0}</option>", myValue))
                    End While
                End Using
            End Using
        Catch
            wpmLogging.AuditLog("DB ERROR - mhwcm.GetDropDownList", mySQL)
        End Try
        sbList.Append("</SELECT>")
        Return sbList.ToString
    End Function
    Public Shared Function GetCompanyList(ByVal CompanyID As String) As String
        Dim sbList As New StringBuilder("<SELECT name='st'><OPTION value=''>Please Select</OPTION>")
        Dim sqlwrk As String = ("SELECT [Company].[CompanyID], [Company].[CompanyName],[Company].[DBString] FROM [Company] ORDER BY [Company].[CompanyName] ASC")
        For Each myRow As DataRow In wpmDB.GetDataTable(sqlwrk, "CompanyList").Rows
            sbList.Append(String.Format("<option value='{0}'", wpmUtil.GetDBString(myRow.Item(0))))
            If wpmUtil.GetDBString(myRow.Item(0)) = CompanyID Then
                sbList.Append(" selected ")
            End If
            sbList.Append(String.Format(">{0}</option>", wpmUtil.GetDBString(myRow.Item(1))))
        Next
        sbList.Append("</SELECT>")
        Return sbList.ToString
    End Function
    Public Shared Function GetCompanyContactList(ByVal sContactID As String, ByVal sFieldName As String, ByVal bRequired As Boolean, ByVal CompanyID As String) As String
        Dim sqlwrk As String = String.Format("SELECT ContactID, PrimaryContact & ' (' & LogonName & ')' FROM Contact where Contact.CompanyID={0} ORDER BY PrimaryContact ", CompanyID)
        Dim mydt As DataTable = wpmDB.GetDataTable(sqlwrk, "mhwcm.GetCompanyContactList")
        Dim x_ContactIDList As String
        If (bRequired) Then
            x_ContactIDList = String.Format("<SELECT name='{0}'>", sFieldName)
        Else
            x_ContactIDList = String.Format("<SELECT name='{0}'><OPTION value=''>Please Select</OPTION>", sFieldName)
        End If
        For Each row As DataRow In mydt.Rows
            If wpmUtil.GetDBString(row(0)) = sContactID Then
                x_ContactIDList = String.Format("{0}<OPTION value='{1}' selected", x_ContactIDList, wpmUtil.GetDBString(row(0)))
            Else
                x_ContactIDList = String.Format("{0}<OPTION value='{1}'", x_ContactIDList, wpmUtil.GetDBString(row(0)))
            End If
            x_ContactIDList = String.Format("{0}>{1}</option>", x_ContactIDList, wpmUtil.GetDBString(row(1)))
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
        myDT = wpmDB.GetDataTable(sqlwrk, "mhwcm.GetTemplateList")
        For Each row As DataRow In myDT.Rows
            If wpmUtil.GetDBString(row(0)) = sTemplatePrefix Then
                stList = String.Format("{0}<OPTION value='{1}' selected", stList, wpmUtil.GetDBString(row(0)))
            Else
                stList = String.Format("{0}<OPTION value='{1}'", stList, wpmUtil.GetDBString(row(0)))
            End If
            stList = String.Format("{0}>{1}</option>", stList, wpmUtil.GetDBString(row(1)))
        Next
        stList = stList & "</SELECT>"
        Return stList
    End Function

End Class

