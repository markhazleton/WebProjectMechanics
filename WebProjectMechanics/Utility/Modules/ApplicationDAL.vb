Imports System.Text

Public Module ApplicationDAL
    Public Function GetCompanyData(ByVal CompanyID As String) As DataTable
        Return wpm_GetDataTable(((String.Format("SELECT Company.CompanyID,  Company.CompanyName,  Company.GalleryFolder,  Company.SiteURL,  Company.SiteTitle,  Company.SiteTemplate,  Company.DefaultSiteTemplate,  Company.HomePageID,  Company.DefaultArticleID,  Company.ActiveFL,  Company.UseBreadCrumbURL,  Company.DefaultPaymentTerms,  Company.DefaultInvoiceDescription,  Company.City,  Company.StateOrProvince,  Company.PostalCode,  Company.Country,  Company.FromEmail, Company.SMTP, Company.Component FROM Company WHERE Company.CompanyID={0} ", CompanyID))), "GetCompanyData")
    End Function
    Public Function GetSiteGroupList() As DataTable
        Return wpm_GetDataTable(("SELECT SiteCategoryGroup.[SiteCategoryGroupID], SiteCategoryGroup.[SiteCategoryGroupNM], SiteCategoryGroup.[SiteCategoryGroupDS], SiteCategoryGroup.[SiteCategoryGroupOrder]FROM SiteCategoryGroup;"), "SiteCategoryGroup")
    End Function
    Public Function GetSiteLinks(ByVal CompanyID As String) As DataTable
        Return wpm_GetDataTable((String.Format("SELECT [Link].[ID] AS [PartID],  [Link].[LinkTypeCD] AS [PartTypeCD], [LinkCategory].[Title] AS [PartCategoryTitle], [LinkCategory].[Id] AS [PartCategoryID], IIf([Page].[PageID] Is Null,Null,[Page].[PageID]) AS [LocationID], IIf([Page].[PageID] Is Null,Null,[Page].[PageName]) AS [LocationNM], [Link].[Title] AS [Title], [Link].[Description] AS [Description], [Link].[Url] AS [URL], [Link].[DateAdd] AS [ModifiedDT], [Link].[Ranks] AS [PartSortOrder], [Link].[Views] AS [View], [Link].[UserID] AS [UserID], [Link].[ASIN] AS [AmazonIndex], 'Link' AS [PartSource], [Company].[CompanyID] AS [CompanyID], [Company].[CompanyName] AS [CompanyNM] FROM (SiteCategoryType RIGHT JOIN Company ON SiteCategoryType.SiteCategoryTypeID = Company.SiteCategoryTypeID) LEFT JOIN (SiteCategoryGroup RIGHT JOIN ((Link LEFT JOIN Page ON Link.PageID = Page.PageID) LEFT JOIN LinkCategory ON Link.CategoryID = LinkCategory.ID) ON SiteCategoryGroup.SiteCategoryGroupID = Link.SiteCategoryGroupID) ON Company.CompanyID = Link.CompanyID WHERE (((Company.CompanyID)={0}) AND ((Link.CompanyID) Is Null Or (Link.CompanyID)={0})) and Link.ID is not null  ORDER BY 3, 11; ", CompanyID)), "ApplicationDAL.GetSiteCategoryLinks")
    End Function
    Public Function GetPartCategoryList() As DataTable
        Return wpm_GetDataTable("SELECT LinkCategory.ID,  LinkCategory.Title,  LinkCategory.ParentID,  LinkCategory.Description,  LinkCategory.PageID,  0 AS CountOfID FROM LinkCategory ", "mhDataCon.GetLinkCategoryList")
    End Function
    Public Function GetSiteParameterList(ByVal CompanyID As String) As DataTable
        Dim strSQL As String = String.Format("SELECT 'CompanySiteParameter' AS RecordSource, IIf(CompanySiteParameter.CompanySiteParameterID Is Null,0,10)+IIf(CompanySiteParameter.SiteCategoryGroupID Is Null,0,1)+IIf(CompanySiteParameter.PageID Is Null,0,1)+IIf(CompanySiteParameter.CompanyID Is Null,0,1) AS PrimarySort, CompanySiteParameter.CompanyID, CompanySiteParameter.SiteParameterTypeID, CompanySiteParameter.SortOrder, CompanySiteParameter.ParameterValue, CompanySiteParameter.PageID, CompanySiteParameter.SiteCategoryGroupID, '' AS SiteCategoryTypeID, SiteParameterType.SiteParameterTypeNM, SiteParameterType.SiteParameterTypeDS, CompanySiteParameter.CompanySiteParameterID, Page.PageName, Company.CompanyName, SiteCategoryGroup.SiteCategoryGroupNM FROM SiteCategoryGroup RIGHT JOIN ((Page RIGHT JOIN (SiteParameterType INNER JOIN CompanySiteParameter ON SiteParameterType.SiteParameterTypeID = CompanySiteParameter.SiteParameterTypeID) ON Page.PageID = CompanySiteParameter.PageID) LEFT JOIN Company ON CompanySiteParameter.CompanyID = Company.CompanyID) ON SiteCategoryGroup.SiteCategoryGroupID = CompanySiteParameter.SiteCategoryGroupID WHERE (((CompanySiteParameter.CompanyID)={0} Or (CompanySiteParameter.CompanyID) Is Null)) ORDER BY 2 DESC , 3 DESC , 7 DESC; ", CompanyID)
        Return wpm_GetDataTable(strSQL, "GetSiteParameterList")
    End Function

    Public Function GetParameterTypeList() As DataTable
        Return wpm_GetDataTable("SELECT 'SiteParameterType' AS RecordSource, SiteParameterType.SiteParameterTypeOrder AS PrimarySort, Null AS CompanyID, SiteParameterType.SiteParameterTypeID, SiteParameterType.SiteParameterTypeOrder, SiteParameterType.SiteParameterTemplate, Null AS SiteCategoryID, Null AS SiteCategoryGroupID, Null AS SiteCategoryTypeID, SiteParameterType.SiteParameterTypeNM, SiteParameterType.SiteParameterTypeDS,Null as CompanySiteParameterID FROM SiteParameterType ORDER BY 2 desc,3 DESC, 7 DESC ", "GetParameterTypeList")
    End Function
    Public Function GetPageAliasList(ByVal CompanyID As String) As DataTable
        Dim strSQL As String = (String.Format("SELECT PageAliasID, PageURL, TargetURL, AliasType from PageAlias where [CompanyID]={0} ", CompanyID))
        Return wpm_GetDataTable(strSQL, "GetPageAliasList")
    End Function
    Public Function GetImageByID(ByVal ImageID As String) As DataTable
        Dim strSQL As String = String.Format("SELECT [Image].[ImageID], [Image].[ImageName], [Image].[ImageFileName], [Image].[ImageThumbFileName], [Image].[ImageDescription], [Image].[ImageComment], [Image].[ImageDate], [Image].[Active], [Image].[ModifiedDT], [Image].[VersionNo], [Image].[ContactID], [Image].[CompanyID], [Image].[Title], [Image].[Medium], [Image].[Size], [Image].[Price], [Image].[Color], [Image].[Subject], [Image].[Sold], [PageImage].[PageImagePosition], [Page].[PageID], [Page].[PageName] FROM [Image] LEFT JOIN ([Page] RIGHT JOIN [PageImage] ON [Page].[PageID] = [PageImage].[PageID]) ON [Image].[ImageID] = [PageImage].[ImageID] where [Image].[ImageID]={0} ", ImageID)
        Return wpm_GetDataTable(strSQL, "GetImageByID")
    End Function
    Public Function GetImageList(ByVal CompanyID As String) As DataTable
        Dim strSQL As String = String.Format("SELECT [Image].[ImageID], [Image].[ImageName], [Image].[ImageFileName], [Image].[ImageThumbFileName], [Image].[ImageDescription], [Image].[ImageComment], [Image].[ImageDate], [Image].[Active], [Image].[ModifiedDT], [Image].[VersionNo], [Image].[ContactID], [Image].[CompanyID], [Image].[Title], [Image].[Medium], [Image].[Size], [Image].[Price], [Image].[Color], [Image].[Subject], [Image].[Sold],[Image].[ModifiedDT], [PageImage].[PageImagePosition], [Page].[PageID], [Page].[PageName] FROM [Image] LEFT JOIN ([Page] RIGHT JOIN [PageImage] ON [Page].[PageID] = [PageImage].[PageID]) ON [Image].[ImageID] = [PageImage].[ImageID] where [Image].[CompanyID]={0} Order By [PageImage].[PageID],[PageImage].[PageImagePosition],[Image].[ImageName] ", CompanyID)
        Return wpm_GetDataTable(strSQL, "GetImageList")
    End Function
    Public Function GetPageImageByCompany(ByVal CompanyID As String) As DataTable
        Dim strSQL As String = String.Format("SELECT [Page].[PageID], [Page].[PageName], [Page].[PageDescription], [Page].[PageKeywords], [Page].[ImagesPerRow], [Page].[RowsPerPage], [PageImage].[PageImagePosition], Image.[ImageID], Image.[ImageName], Image.[ImageFileName], Image.[ImageThumbFileName], Image.[ImageDescription], Image.[ImageComment], Image.[ImageDate], Image.[Active], Image.[ModifiedDT], Image.[VersionNo], Image.[ContactID], Image.[CompanyID], Image.[Title], Image.[Medium], Image.[Size], Image.[Price], Image.[Color], Image.[Subject], Image.[Sold] FROM [Page],[PageImage],[Image] WHERE [Page].[PageID] = [PageImage].[PageID] AND [Image].[ImageID] = [PageImage].[ImageID] AND [Page].[CompanyID] = {0} ORDER BY [PageImage].[PageImagePosition] ", CompanyID)
        Return wpm_GetDataTable(strSQL, "GetPageImageByCompany")
    End Function
    Public Function GetPageImage(ByVal PageID As String, ByVal CompanyID As String, ByVal GroupID As String) As DataTable
        If wpm_GetDBInteger(PageID, 0) > 0 AndAlso wpm_GetDBInteger(CompanyID, 0) > 0 AndAlso wpm_GetDBInteger(GroupID, 0) > 0 Then
            Return wpm_GetDataTable(String.Format("SELECT [Page].[PageID], [Page].[PageName], [Page].[PageDescription], [Page].[PageKeywords], [Page].[ImagesPerRow], [Page].[RowsPerPage], [Page].[PageFileName] FROM [Page] WHERE [Page].[CompanyID] = {0} AND [Page].[PageID] = {1} AND [Page].[GroupID] >= {2} AND [Page].[Active] = TRUE ",
                                          CompanyID,
                                          PageID,
                                          GroupID), "GetPageImage")
        Else
            Return New DataTable
        End If
    End Function
    Public Function GetSiteMap(ByVal sSortBy As String, ByVal CompanyID As String, ByVal GroupID As String) As DataTable
        Dim strSQL As New StringBuilder(String.Empty)
        ' SiteMap only shows Active Pages and Articles for Guests
        '  1 - PageID
        '  2 - PageName
        '  3 - PageTitle
        '  4 - PageDescription
        '  5 - ParentPageID
        '  6 - PageSource - Source of Record "Article","Page","Image", "Category"
        '  7 - PageKeywords
        '  8 - TransferURL (only if not equal to "NO FILE NAME")
        '  9 - PageFileName (use for TransferURL if TransferURL =  "NO FILE NAME")
        ' 10 - ModifiedDate (Use StartDT for articles)
        ' 11 - ArticleID
        ' 12 - Active Flag
        ' 13 - PageOrder
        ' 14 - SiteCategoryID
        ' 15 - SiteCategoryName
        ' 16 - SiteCategoryGroupName
        ' 17 - PageTypeCD
        ' 18 - PageTypeID
        ' 19 - GroupID
        ' 20 - SiteCategoryTypeID
        ' 21 - ImageFileName
        ' 22 - Summary
        ' 23 - Body
        ' 24 - UseInNavigation
        ' 25 - ImageID
        ' 26 - LocationAuthor
        ' 27 - RowsPerPage
        ' 28 - ImagesPerRow

        strSQL.Append(String.Format("SELECT Page.PageID, Article.Title AS PageName, Article.Title AS PageTitle, Article.Description as Description, Page.ParentPageID, 'Article' AS RecordSource, Page.PageKeywords, IIf(pagetype.PageFileName Is Null,'/default.aspx',pagetype.PageFileName) AS TransferURL, Page.PageFileName, Article.ModifiedDT , Article.ArticleID, Article.Active, Page.PageOrder, Page.SiteCategoryID, ' ' AS SiteCategoryNM, ' ' AS SiteCategoryGroupNM, ' ' AS SiteCategoryGroupID, IIf(pagetype.PageTypeCD Is Null,'Article',pagetype.PageTypeCD) AS PageTypeCD, Page.PageTypeID AS PageTypeID, Page.GroupID,  Null AS SiteCategoryTypeID, '' AS ImageFileName, Article.ArticleSummary AS Summary, Article.ArticleBody AS Body, False AS UseInNavigation, 0 AS ImageID, Article.Author as LocationAuthor, Page.RowsPerPage, Page.ImagesPerRow FROM (Article LEFT JOIN Page ON Article.PageID = Page.PageID) LEFT JOIN pagetype ON Page.PageTypeID = pagetype.PageTypeID Where ( Article.Title <> Page.PageName OR Page.PageName is null) and Article.CompanyID ={0} and (Page.CompanyID = {0} or Page.CompanyID is null) and (Page.PageName <> Article.Title or Page.PageName is null)  UNION ALL  SELECT Page.PageID, Page.PageName, Page.PageTitle, Page.PageDescription, Page.ParentPageID, 'Page' AS RecordSource, Page.PageKeywords, PageType.PageFileName AS TransferURL, Page.PageFileName, Page.ModifiedDT, Article.ArticleID, Page.Active, IIf([Page].[PageOrder] Is Null,0,[Page].[PageOrder]) AS PageOrder, Page.SiteCategoryID, SiteCategory.CategoryName, IIf(SiteCategoryGroup.SiteCategoryGroupNM Is Null,SiteCategoryGroup_Page.SiteCategoryGroupNM,SiteCategoryGroup.SiteCategoryGroupNM) AS SiteCategoryGroupNM, IIf(SiteCategoryGroup.SiteCategoryGroupID Is Null,SiteCategoryGroup_Page.SiteCategoryGroupID,SiteCategoryGroup.SiteCategoryGroupID) AS SiteCategoryGroupID, IIf(pagetype.PageTypeCD Is Null,'Article',pagetype.PageTypeCD) AS PageTypeCD, PageType.PageTypeID AS PageTypeID, Page.GroupID, Null AS SiteCategoryTypeID, '' AS ImageFileName, Article.ArticleSummary AS Summary, Article.ArticleBody AS Body, False AS UseInNavigation, 0 as ImageID, '' as LocationAuthor, Page.RowsPerPage, Page.ImagesPerRow  FROM ((((Page LEFT JOIN PageType ON Page.PageTypeID = PageType.PageTypeID) LEFT JOIN SiteCategory ON Page.SiteCategoryID = SiteCategory.SiteCategoryID) LEFT JOIN SiteCategoryGroup ON SiteCategory.SiteCategoryGroupID = SiteCategoryGroup.SiteCategoryGroupID) LEFT JOIN SiteCategoryGroup AS SiteCategoryGroup_Page ON Page.SiteCategoryGroupID = SiteCategoryGroup_Page.SiteCategoryGroupID) LEFT JOIN Article ON (Page.PageName = Article.Title) AND (Page.PageID = Article.PageID)  WHERE Page.CompanyID={0} UNION ALL  SELECT Page.PageID,Page.PageName & '-' & Image.ImageName AS PageName, Page.PageName & '-' & Image.ImageName AS PageTitle, Image.ImageDescription, Page.ParentPageID, 'Image' AS RecordSource, Page.PageKeywords, IIf([pagetype].[PageFileName] Is Null,'/default.aspx',[pagetype].[PageFileName]) AS TransferURL, Page.PageFileName, Page.ModifiedDT, null as ArticleID, Image.Active,Page.PageOrder,  Page.SiteCategoryID, ' ' as CategoryName, ' ' as SiteCategoryGroupNM, ' ' as SiteCategoryGroupID, IIf(pagetype.PageTypeCD Is Null,'Image',pagetype.PageTypeCD) AS PageTypeCD, pagetype.PageTypeID as PageTypeID, Page.GroupID as GroupID, null as SiteCategoryTypeID, '' as ImageFileName, '' as Summary, '' as Body, FALSE as UseInNavigation, Image.ImageID , '' as LocationAuthor, Page.RowsPerPage, Page.ImagesPerRow   FROM [Image] INNER JOIN ((Page LEFT JOIN pagetype ON Page.PageTypeID = pagetype.PageTypeID) INNER JOIN PageImage ON Page.PageID = PageImage.PageID) ON Image.ImageID = PageImage.ImageID WHERE (((Image.CompanyID)={0}) AND ((Page.PageName)<>[Image].[ImageName]) AND ((Page.CompanyID)={0})) OR (((Image.CompanyID)={0}) AND ((Page.PageName) Is Null) AND ((Page.GroupID) Is Null) AND ((Page.CompanyID) Is Null))  UNION ALL  SELECT SiteCategory.SiteCategoryID AS PageID, SiteCategory.CategoryName AS PageName, SiteCategory.CategoryTitle AS PageTitle, SiteCategory.CategoryDescription AS Description, SiteCategory.ParentCategoryID AS ParentPageID, 'Category' AS RecordSource, SiteCategory.CategoryKeywords AS PageKeywords,  IIf(SiteCategory.CategoryFileName Is Null, IIf(SiteCategoryType.SiteCategoryFileName Is Null,'/default.aspx',SiteCategoryType.SiteCategoryFileName),SiteCategory.CategoryFileName ) AS TransferURL, SiteCategory.CategoryFileName AS PageFileName,   Now() AS ModifiedDT, Null AS ArticleID, True AS Active, SiteCategory.GroupOrder AS PageOrder, SiteCategory.SiteCategoryID, SiteCategory.CategoryName AS CategoryName, SiteCategoryGroup.SiteCategoryGroupNM AS SiteCategoryGroupNM, SiteCategoryGroup.SiteCategoryGroupID AS SiteCategoryGroupID, 'SITECAT' AS PageTypeCD, Null AS PageTypeID, 4 AS GroupID, SiteCategoryType.SiteCategoryTypeID, '' AS ImageFileName, '' AS Summary, '' AS Body, False AS UseInNavigation, 0 AS ImageID, '' AS LocationAuthor, 0 AS RowsPerPage, 0 AS ImagesPerRow  FROM SiteCategoryGroup RIGHT JOIN ((Company LEFT JOIN SiteCategoryType ON Company.SiteCategoryTypeID = SiteCategoryType.SiteCategoryTypeID) LEFT JOIN SiteCategory ON SiteCategoryType.SiteCategoryTypeID = SiteCategory.SiteCategoryTypeID) ON SiteCategoryGroup.SiteCategoryGroupID = SiteCategory.SiteCategoryGroupID WHERE (((Company.CompanyID)={0})) and SiteCategory.SiteCategoryID is not null  ", CompanyID))
        ' Order results by PageOrder
        Select Case sSortBy
            Case "ORDER"
                strSQL.Append(" ORDER BY 13 ASC,5 ASC,2 ASC,10 ASC ")
            Case "MODIFIED"
                strSQL.Append("ORDER BY 10 DESC ")
            Case "NAME"
                strSQL.Append("ORDER BY 2 ASC ")
            Case "ARTICLE"
                strSQL.Append("ORDER BY 2 DESC ")
            Case Else
                strSQL.Append("ORDER BY 10 DESC ")
        End Select
        Return wpm_GetDataTable(strSQL.ToString, "SiteMap")
    End Function

    Public Function UpdateLocationSort(ByRef myLocation As Location) As Boolean
        Dim sSQL As String
        Dim iRowsUpdated As Integer = 0
        If myLocation.RecordSource = "Page" Then
            If Trim(myLocation.LocationID) <> String.Empty Then
                sSQL = String.Format("UPDATE Page SET Page.PageOrder={1} WHERE Page.PageID={0};", myLocation.LocationID, myLocation.DefaultOrder)
                iRowsUpdated = wpm_RunUpdateSQL(sSQL, "ApplicationDAL.UpdatePageDate")
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
    Public Function UpdatePageDate(ByVal pageID As String) As Boolean
        Dim sSQL As String
        Dim iRowsUpdated As Integer = 0
        If Trim(pageID) <> String.Empty Then
            sSQL = String.Format("UPDATE Page SET Page.ModifiedDT = system.datetime.now() WHERE (((Page.PageID)={0}));", pageID)
            iRowsUpdated = wpm_RunUpdateSQL(sSQL, "ApplicationDAL.UpdatePageDate")
        End If
        If iRowsUpdated > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function GetDropDownList(ByVal ListTable As String, ByVal ListID As String, ByVal ListDisplay As String, ByVal ListWhere As String, ByVal CurrentID As String) As String
        Dim sbList As New StringBuilder(String.Format("<SELECT name='cb{0}'><OPTION value=''>Please Select</OPTION>", ListID))
        Dim mySQL As String
        Dim myID As String
        Dim myValue As String
        If Trim(ListWhere) <> String.Empty Then
            mySQL = String.Format("{0} where {1}", ("Select " & ListID & ", " & ListDisplay & " from " & ListTable), ListWhere)
        Else
            mySQL = (String.Format("Select {0}, {1} from {2}", ListID, ListDisplay, ListTable))
        End If
        mySQL = String.Format("{0} order by {1}", mySQL, ListDisplay)
        Try
            Using RecConn As New System.Data.OleDb.OleDbConnection(wpm_SQLDBConnString)
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
            ApplicationLogging.SQLSelectError("DB ERROR - ApplicationDAL.GetDropDownList", mySQL)
        End Try
        sbList.Append("</SELECT>")
        Return sbList.ToString
    End Function
    Public Function GetCompanyLookupList() As List(Of LookupItem)
        Dim myList As New List(Of LookupItem)
        Dim sqlwrk As String = ("SELECT [Company].[CompanyID], [Company].[CompanyName] FROM [Company] ORDER BY [Company].[CompanyName] ASC")
        For Each myRow As DataRow In wpm_GetDataTable(sqlwrk, "CompanyList").Rows
            myList.Add(New LookupItem With {.Value = wpm_GetDBString(myRow.Item(0)), .Name = wpm_GetDBString(myRow.Item(1))})
        Next
        Return myList
    End Function

    Public Function GetRoleLookupList() As List(Of LookupItem)
        Dim myList As New List(Of LookupItem)
        Dim sqlwrk As String = ("SELECT [role].[roleID], [role].[RoleName] FROM [Role] ORDER BY [Role].[RoleName] ASC")
        For Each myRow As DataRow In wpm_GetDataTable(sqlwrk, "RoleList").Rows
            myList.Add(New LookupItem With {.Value = wpm_GetDBString(myRow.Item(0)), .Name = wpm_GetDBString(myRow.Item(1))})
        Next
        Return myList
    End Function

    Public Function GetCompanyContactList(ByVal sContactID As String, ByVal sFieldName As String, ByVal bRequired As Boolean, ByVal CompanyID As String) As String
        Dim sqlwrk As String = String.Format("SELECT ContactID, PrimaryContact & ' (' & LogonName & ')' FROM Contact where Contact.CompanyID={0} ORDER BY PrimaryContact ", CompanyID)
        Using mydt As DataTable = wpm_GetDataTable(sqlwrk, "ApplicationDAL.GetCompanyContactList")
            Dim x_ContactIDList As String
            If (bRequired) Then
                x_ContactIDList = String.Format("<SELECT name='{0}'>", sFieldName)
            Else
                x_ContactIDList = String.Format("<SELECT name='{0}'><OPTION value=''>Please Select</OPTION>", sFieldName)
            End If
            For Each row As DataRow In mydt.Rows
                If wpm_GetDBString(row(0)) = sContactID Then
                    x_ContactIDList = String.Format("{0}<OPTION value='{1}' selected", x_ContactIDList, wpm_GetDBString(row(0)))
                Else
                    x_ContactIDList = String.Format("{0}<OPTION value='{1}'", x_ContactIDList, wpm_GetDBString(row(0)))
                End If
                x_ContactIDList = String.Format("{0}>{1}</option>", x_ContactIDList, wpm_GetDBString(row(1)))
            Next
            x_ContactIDList = x_ContactIDList & "</SELECT>"
            Return x_ContactIDList
        End Using
    End Function
    Public Function GetTemplateList(ByVal sTemplatePrefix As String, ByVal bRequired As Boolean) As String
        Dim stList As String = String.Empty
        Dim sqlwrk As String = String.Empty
        Dim myDT As DataTable
        If (bRequired) Then
            stList = "<SELECT name='st'>"
        Else
            stList = "<SELECT name='st'><OPTION value=''>Please Select</OPTION>"
        End If
        sqlwrk = "SELECT [SiteTemplate].[TemplatePrefix], [SiteTemplate].[Name] FROM [SiteTemplate] ORDER BY [SiteTemplate].[Name] ASC"
        myDT = wpm_GetDataTable(sqlwrk, "ApplicationDAL.GetTemplateList")
        For Each row As DataRow In myDT.Rows
            If wpm_GetDBString(row(0)) = sTemplatePrefix Then
                stList = String.Format("{0}<OPTION value='{1}' selected", stList, wpm_GetDBString(row(0)))
            Else
                stList = String.Format("{0}<OPTION value='{1}'", stList, wpm_GetDBString(row(0)))
            End If
            stList = String.Format("{0}>{1}</option>", stList, wpm_GetDBString(row(1)))
        Next
        stList = stList & "</SELECT>"
        Return stList
    End Function

End Module

