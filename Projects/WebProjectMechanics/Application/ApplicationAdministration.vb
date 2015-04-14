'Imports System.Text
'Imports System.Web.SessionState

'Public Class ApplicationAdministration
'    Public ReadOnly myActiveCompany As ActiveCompany
'    Private ReadOnly ArticleList As ArticleList
'    Private ReadOnly ImageList As CompanyImageList
'    Sub New(ByVal thisSession As HttpSessionState, ByRef thisActiveCompany As ActiveCompany)
'        myActiveCompany = thisActiveCompany
'        ArticleList = New ArticleList()
'        ImageList = New CompanyImageList(wpm_CurrentSiteID)
'    End Sub
'    Public Function SiteImageReport() As String
'        Dim sbReturn As New StringBuilder(String.Empty)
'        sbReturn.Append("<table class=""sortable autostripe "">")
'        sbReturn.Append("<thead><tr><th>Image</th><th>Details</th></tr></thead>")
'        For Each myRow As Location In myActiveCompany.LocationList
'            If myRow.RecordSource = "Image" Then
'                sbReturn.Append("<tr>")
'                sbReturn.Append(String.Format("<td style=""whitespace:nobreak;""><a href=""{2}maint/default.aspx?type=Image&ImageID={0}"">{1}</a></td>", myRow.ArticleID, myRow.LocationName, wpm_SiteConfig.AdminFolder))
'                sbReturn.Append(String.Format("<td>{0}</td>", GetImageDetail(myRow.ArticleID)))
'                sbReturn.Append("</tr>")
'                sbReturn.Append("<tr><td ""colspan=""2""><div style=""page-break-after: always;""><span style=""display: none;"">&nbsp;</span></div></td></tr>")
'            End If
'        Next
'        sbReturn.Append("</table>")
'        Return sbReturn.ToString
'    End Function
'    Public Function SiteReport() As String
'        Dim sbReturn As New StringBuilder(String.Empty)
'        sbReturn.Append("<table class=""sortable autostripe "">")
'        sbReturn.Append("<thead><tr><th>Page</th><th>Content</th></tr></thead>")
'        For Each myRow As Location In myActiveCompany.LocationList
'            If myRow.RecordSource = "Page" Or myRow.RecordSource = "Category" Then
'                sbReturn.Append("<tr>")
'                sbReturn.Append(String.Format("<td style=""whitespace:nobreak;"">{0}</td>", myRow.LocationName))
'                sbReturn.Append(String.Format("<td>{0}</td>", GetArticleBody(myRow.LocationID)))
'                sbReturn.Append("</tr>")
'                sbReturn.Append("<tr><td ""colspan=""2""><div style=""page-break-after: always;""><span style=""display: none;"">&nbsp;</span></div></td></tr>")
'            End If
'        Next
'        sbReturn.Append("</table>")
'        Return sbReturn.ToString
'    End Function
'    Private Function GetImageDetail(ByVal ImageID As String) As String
'        Dim sbReturn As New StringBuilder(String.Empty)
'        For Each myImage As LocationImage In ImageList
'            If myImage.ImageID = ImageID Then
'                sbReturn.Append(String.Format("<strong>{0} ({1})</strong><br/>", myImage.ImageName, myImage.ImageID))
'                sbReturn.Append(String.Format("{0}<br/>", myImage.ImageDescription))
'                sbReturn.Append(String.Format("<img src=""/runtime/catalog/FindImage.ashx?img={0}{1}&w=200"" />", wpm_SiteGallery, myImage.ImageFileName, wpm_SiteConfig.ApplicationHome))
'            End If
'        Next
'        Return sbReturn.ToString
'    End Function

'    Private Function GetArticleBody(ByVal PageID As String) As String
'        Dim sbReturn As New StringBuilder(String.Empty)

'        For Each myArticle As Article In ArticleList
'            If myArticle.ArticlePageID = PageID Then
'                sbReturn.Append(String.Format("<strong>{0} ({1} - {2})</strong", myArticle.ArticleName, myArticle.ArticlePageID, myArticle.ArticleID))
'                sbReturn.Append(String.Format("{0}", myArticle.ArticleBody))
'            End If
'        Next
'        Return sbReturn.ToString
'    End Function
'    Public Function BuildAdmin() As String
'        Dim iRowCount As Integer = 0
'        Dim myStringBuilder As StringBuilder = New StringBuilder()
'        ' Set the ListPageURL so the edit,delete, add, pages know where to come back to
'        wpm_ListPageURL = HttpContext.Current.Request.FilePath()
'        myStringBuilder.Append(String.Format("<table>{0}</table>", BuildPageTree("", 0, iRowCount)))
'        Return (myStringBuilder.ToString)
'    End Function

'    Private Shared Function AdminFormatLink(ByVal LinkID As String, ByVal LinkName As String, ByVal LinkType As String) As String
'        Dim sReturn As String = ""
'        If LinkID <> "" Then
'            Select Case LinkType
'                Case "Page"
'                    sReturn = String.Format("<a class=""linkmenu"" href="""" onclick=""createPopUp(200,110,'{0}','Page',this);return false"" id=Page{0} >{1}</a>", LinkID, LinkName)
'                Case "Article"
'                    sReturn = String.Format("<a class=""linkmenu"" href="""" onclick=""createPopUp(200,110,'{0}','Article',this);return false"" id=Article{0} >{1}</a>", LinkID, LinkName)
'                Case "Company"
'                    sReturn = String.Format("<a class=""linkmenu"" href="""" onclick=""createPopUp(200,110,'{0}','Company',this);return false"" id=Company{0} >{1}</a>", LinkID, LinkName)
'                Case "Category"
'                    sReturn = LinkName
'                Case Else
'                    sReturn = LinkName
'            End Select
'        End If
'        Return sReturn
'    End Function


'    Private Sub AddLocationStructure(ByVal intLevel As Integer, ByRef iRowCount As Integer, ByVal bSummary As Boolean, ByRef myRow As Location, ByRef strReturn As StringBuilder)
'        Dim sRowClass As String
'        If CBool(iRowCount Mod 2) Then
'            sRowClass = "ewTableAltRow"
'        Else
'            sRowClass = "ewTableRow"
'        End If
'        iRowCount = iRowCount + 1


'        Dim myLink As String = wpm_FormatLink(myRow.LocationName,
'                                    "Location",
'                                    String.Format("{0}maint/default.aspx?type=Location&LocationID={1}",
'                                                  wpm_SiteConfig.AdminFolder,
'                                                  myRow.LocationID),
'                                    myRow.LocationName)



'        strReturn.Append(String.Format("<tr class=""{2}""><td NOWRAP class=""{2}"" valign=""top""><img src=""/admin/images/spacer.gif"" alt="""" width=""{1}"" height=""1"" />{0}",
'                                       myLink,
'                                       intLevel * 15,
'                                       sRowClass))

'        If Not (myRow.RecordSource = "Article") Then
'            strReturn.Append(BuildPageArticleList(myRow.LocationID))
'        End If
'        If bSummary Then
'            strReturn.Append(String.Format("</td>{0}<td class=""{3}"" valign=""top"">{1}</td>{0}<td class=""{3}"" valign=""top"">{2}</td>{0}</tr>{0}",
'                                         vbCrLf,
'                                         myRow.LocationTitle,
'                                         myRow.LocationTitle,
'                                         sRowClass))
'        Else
'            strReturn.Append(String.Format("</td>{0}<td class=""{3}"" valign=""top"">{1}</td>{0}<td class=""{3}"" valign=""top"">{2}</td>{0}</tr>{0}",
'                                         vbCrLf,
'                                         myActiveCompany.ReplaceTags(myRow.LocationTitle, myRow),
'                                         myActiveCompany.ReplaceTags(myRow.LocationTitle, myRow),
'                                         sRowClass))
'        End If
'    End Sub

'    Public Function BuildPageTree(ByVal ParentID As String, ByVal intLevel As Double, ByRef iRowcount As Integer) As String
'        Dim sReturn As New StringBuilder("")
'        For Each myLocation As Location In myActiveCompany.LocationList
'            If myLocation.ActiveFL Then
'                If (myLocation.RecordSource = "Page" Or myLocation.RecordSource = "Article" Or myLocation.RecordSource = "Category") And myLocation.LocationTypeCD IsNot "MODULE" Then
'                    If ParentID = myLocation.ParentLocationID Then
'                        AddLocationStructure(CInt(intLevel), iRowcount, False, myLocation, sReturn)
'                        sReturn.Append(BuildPageTree(myLocation.LocationID, intLevel + 1, iRowcount))
'                    End If
'                End If
'            End If
'        Next
'        Return sReturn.ToString
'    End Function

'    Private Function BuildCategoryPageList(ByVal SiteCategoryID As String) As String
'        Dim myRow As Location
'        Dim j As Integer
'        Dim sReturn As New StringBuilder
'        For j = 0 To myActiveCompany.LocationList.Count - 1
'            Dim sCellContent As New StringBuilder
'            myRow = myActiveCompany.LocationList.Item(j)
'            If SiteCategoryID = myRow.SiteCategoryID And myRow.RecordSource = "Page" Then
'                sCellContent.Append(AdminFormatLink(myRow.LocationID, myRow.LocationName, "Page") & "&nbsp;")
'                sCellContent.Append(wpm_FormatLink("Edit", "Page", String.Format("{0}maint/default.aspx?type=Location&LocationID={1}", wpm_SiteConfig.AdminFolder, myRow.LocationID), "edit") & "&nbsp;")
'                If Not myRow.ActiveFL Then
'                    sCellContent.Append("(i) ")
'                End If
'                sReturn.Append(String.Format("{0}<br/>{1}", sCellContent, vbCrLf))
'            End If
'        Next
'        Return sReturn.ToString
'    End Function

'    Private Function BuildPageArticleList(ByVal pageID As String) As String
'        Dim myrow As Article
'        Dim sReturn As New StringBuilder
'        Dim j As Integer
'        For j = 0 To ArticleList.Count - 1
'            myrow = ArticleList.Item(j)
'            If (myrow.ArticlePageID = pageID) Then
'                If Not (myrow.IsArticleActive) Then
'                    sReturn.Append("(i) ")
'                End If
'                sReturn.Append(String.Format("{0}<br />",
'                                   wpm_FormatLink(myrow.PageName,
'                                                              "Article",
'                                                              String.Format("{0}/maint/default.aspx?type=Article&ArticleID={1}", wpm_SiteConfig.AdminFolder, myrow.ArticleID),
'                                                              myrow.PageName)))
'            End If
'        Next
'        If (sReturn.ToString = "") Then
'            sReturn.Append("&nbsp;")
'        End If
'        BuildPageArticleList = sReturn.ToString
'    End Function
'    Public Function BuildAdminHeader() As String
'        Dim sbReturn As New StringBuilder
'        FileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath(wpm_SiteConfig.AdminFolder + "ApplicationAdminHeader.html"), sbReturn)
'        sbReturn.Replace("~~CompanyID~~", wpm_CurrentSiteID)
'        sbReturn.Replace("~~Template~~", wpm_SiteTemplatePrefix)
'        sbReturn.Replace("~~ContactID~~", wpm_ContactID)
'        Return sbReturn.ToString
'    End Function

'    Public Function BuildAdminHeaderLinks() As String
'        Return (String.Format("<div class=""topbanner""><div class=""rightnav""><a href=""{2}admin/default.aspx""> Admin Home </a> | <a href=""/""> Back To Site </a> |{0}</div><h1>{1} - Administration</h1></div>",
'                    wpm_GetUserOptions,
'                    wpm_HostName,
'                    wpm_SiteConfig.AdminFolder))
'    End Function

'End Class
