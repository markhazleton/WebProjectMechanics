Public Class mhcatalog
    Public mySiteMap As mhSiteMap
    Public sImageWidth As String = "800"
    Public sThumbnailWidth As String = "200"
    Public Sub New(ByVal passedSiteMap As mhSiteMap)
        mySiteMap = passedSiteMap
    End Sub

    Public Function ProcessArticlePage(ByVal PageNumber As String, ByVal ImageID As String, ByVal sTemplate As String) As String
        Dim sRightContent As String = ("")
        Dim myStringBuilder As StringBuilder = New StringBuilder("")
        ' code for thumnail page
        myStringBuilder.Append(ProcesscatalogPage(sRightContent, mySiteMap.mySession.CurrentPageID, mySiteMap.mySession.CurrentArticleID, mySiteMap.mySiteFile.SiteGallery))
        Return myStringBuilder.ToString
    End Function

    Public Function ProcessPageRequest(ByVal PageNumber As String, ByVal ImageID As String, ByVal sTemplate As String) As String
        Dim sRightContent As String = ("")
        Dim myStringBuilder As StringBuilder = New StringBuilder("")
        If (ImageID = "") Then
            ' code for thumnail page
            myStringBuilder.Append(ProcesscatalogPage(sRightContent, mySiteMap.mySession.CurrentPageID, mySiteMap.mySession.CurrentArticleID, mySiteMap.mySiteFile.SiteGallery))

            If InStr(1, myStringBuilder.ToString, "~~cmotion~~") > 0 Then
                myStringBuilder.Replace("~~cmotion~~", BuildCMotionPageImage(mySiteMap.mySession.CurrentPageID, mySiteMap.mySession.CurrentArticleID, mySiteMap.mySiteFile.SiteGallery))
                Dim myContents As New StringBuilder
                mhfio.ReadTextFile(HttpContext.Current.Server.MapPath("/util/cmotion/AddToHTMLHead.txt"), myContents)
                mySiteMap.mySession.AddHTMLHead = myContents.ToString
            End If
            If InStr(1, myStringBuilder.ToString, "~~cmotionlb~~") > 0 Then
                myStringBuilder.Replace("~~cmotionlb~~", BuildCMotionLBPageImage(mySiteMap.mySession.CurrentPageID, mySiteMap.mySession.CurrentArticleID, mySiteMap.mySiteFile.SiteGallery))
                Dim myContents As New StringBuilder
                mhfio.ReadTextFile(HttpContext.Current.Server.MapPath("/util/cmotion/AddToHTMLHeadLB.txt"), myContents)
                mySiteMap.mySession.AddHTMLHead = myContents.ToString
            End If
            If InStr(1, myStringBuilder.ToString, "~~pan-lightbox~~") > 0 Then
                Dim myContents As New StringBuilder
                mhfio.ReadTextFile(HttpContext.Current.Server.MapPath("/util/slider/pan.js"), myContents)
                myContents.Replace("<PageImageArray>", BuildLightboxSliderPageThumbnail(mySiteMap.mySession.CurrentPageID, mySiteMap.mySession.CurrentArticleID, mySiteMap.mySiteFile.SiteGallery))
                myStringBuilder.Replace("~~pan-lightbox~~", myContents.ToString)

                'mhfio.ReadTextFile(HttpContext.Current.Server.MapPath("/util/lightbox/ADDToHTMLHead.txt"), myContents)
                'mySiteMap.mySession.AddHTMLHead = myContents.ToString
            End If
            If InStr(1, myStringBuilder.ToString, "~~lightbox~~") > 0 Then
                myStringBuilder.Replace("~~lightbox~~", BuildLightboxPageImage(mySiteMap.mySession.CurrentPageID, mySiteMap.mySession.CurrentArticleID, mySiteMap.mySiteFile.SiteGallery))
                Dim myContents As New StringBuilder
                mhfio.ReadTextFile(HttpContext.Current.Server.MapPath("/util/lightbox/ADDToHTMLHead.txt"), myContents)
                mySiteMap.mySession.AddHTMLHead = myContents.ToString
            End If
            If InStr(1, myStringBuilder.ToString, "photoalbum") > 0 Then
                myStringBuilder.Replace("~~photoalbum~~", BuildPhotoalbumPageImage(mySiteMap.mySession.CurrentPageID, mySiteMap.mySession.CurrentArticleID, mySiteMap.mySiteFile.SiteGallery))
                Dim myContents As New StringBuilder
                mhfio.ReadTextFile(HttpContext.Current.Server.MapPath("/util/photoalbum/ADDToHTMLHead.txt"), myContents)
                mySiteMap.mySession.AddHTMLHead = myContents.ToString
            End If
            If InStr(1, myStringBuilder.ToString, "~~thumbnail~~") > 0 Then
                myStringBuilder.Replace("~~thumbnail~~", BuildCMotionPageImage(mySiteMap.mySession.CurrentPageID, mySiteMap.mySession.CurrentArticleID, mySiteMap.mySiteFile.SiteGallery))
                Dim myContents As New StringBuilder
                mhfio.ReadTextFile(HttpContext.Current.Server.MapPath("/util/cmotion/AddToHTMLHead.txt"), myContents)
                mySiteMap.mySession.AddHTMLHead = myContents.ToString
            End If
            If InStr(1, myStringBuilder.ToString, "~~slider~~") > 0 Then
                Dim myContents As New StringBuilder
                mhfio.ReadTextFile(HttpContext.Current.Server.MapPath("/util/slider/slider.js"), myContents)
                myContents.Replace("<PageImageArray>", BuildSliderPageImage(mySiteMap.mySession.CurrentPageID, mySiteMap.mySession.CurrentArticleID, mySiteMap.mySiteFile.SiteGallery))
                myStringBuilder.Replace("~~slider~~", myContents.ToString)
            End If
            If InStr(1, myStringBuilder.ToString, "~~pan~~") > 0 Then
                Dim myContents As New StringBuilder
                mhfio.ReadTextFile(HttpContext.Current.Server.MapPath("/util/slider/pan.js"), myContents)
                myContents.Replace("<PageImageArray>", BuildSliderPageThumbnail(mySiteMap.mySession.CurrentPageID, mySiteMap.mySession.CurrentArticleID, mySiteMap.mySiteFile.SiteGallery))
                myStringBuilder.Replace("~~pan~~", myContents.ToString)
            End If
            If InStr(1, myStringBuilder.ToString, "~~drill-in~~") > 0 Then
                Dim myContents As New StringBuilder
                mhfio.ReadTextFile(HttpContext.Current.Server.MapPath("/util/slider/drill-in.js"), myContents)
                myContents.Replace("<PageImageArray>", BuildImageTags(mySiteMap.mySession.CurrentPageID, mySiteMap.mySession.CurrentArticleID, mySiteMap.mySiteFile.SiteGallery))
                myStringBuilder.Replace("~~drill-in~~", myContents.ToString)
            End If
            If InStr(1, myStringBuilder.ToString, "~~fade~~") > 0 Then
                Dim myContents As New StringBuilder
                mhfio.ReadTextFile(HttpContext.Current.Server.MapPath("/util/slider/fadeimages.html"), myContents)
                myContents.Replace("<PageImageArray>", BuildFadeImage(mySiteMap.mySession.CurrentPageID, mySiteMap.mySession.CurrentArticleID, mySiteMap.mySiteFile.SiteGallery))
                myStringBuilder.Replace("~~fade~~", myContents.ToString)
            End If

            If myStringBuilder.Length < 1 Then
                myStringBuilder.Append(BuildPhotoalbumPageImage(mySiteMap.mySession.CurrentPageID, mySiteMap.mySession.CurrentArticleID, mySiteMap.mySiteFile.SiteGallery))
                Dim myContents As New StringBuilder
                mhfio.ReadTextFile(HttpContext.Current.Server.MapPath("/util/photoalbum/ADDToHTMLHead.txt"), myContents)
                mySiteMap.mySession.AddHTMLHead = myContents.ToString
            End If
            If mhUser.IsAdmin() Then
                myStringBuilder.Append("<br /><strong><a href=""/mhweb/admin/browse_images.aspx"">Browse Images</a></strong><br />")
            End If
        Else
            ' Code for Detail Image
            If sTemplate = String.Empty Then
                mhfio.ReadTextFile(HttpContext.Current.Server.MapPath(mySiteMap.mySiteFile.SiteGallery & "/catalog_template.html"), myStringBuilder)
            Else
                mhfio.ReadTextFile(HttpContext.Current.Server.MapPath(mySiteMap.mySiteFile.SiteGallery & "/" & sTemplate & "_template.html"), myStringBuilder)
            End If
            ShowImageDetail(ImageID, mySiteMap.mySession.CurrentPageID, myStringBuilder, mySiteMap.mySiteFile.SiteGallery)

        End If
        Return myStringBuilder.ToString
    End Function
    ' -----------------------------------------------------------------------------
    ' Build a list of Thumbnails for a given PageID and currentPageNumber
    '' -----------------------------------------------------------------------------
    'Private Function BuildPageImage(ByVal EditPageID As Object, ByVal oCurPage As Object, ByVal CurrentArticleID As Integer, ByVal CurrentPageDescription As String, ByVal SiteGallery As String) As String
    '    Dim myStringBuilder As StringBuilder = New StringBuilder("")
    '    Dim sURL As New String("/mhweb/catalog/catalog.aspx?c=" & EditPageID & "&amp;a=" & CurrentArticleID)
    '    Dim sCellWidth As New String("100")
    '    Dim sSubPageNav As New String("")
    '    Dim iImageCount As Integer = 0
    '    Dim iPageCount As Integer = 0
    '    Dim iRecordsPerPage As Integer = 0
    '    Dim iFirstDisplay As Integer = 0
    '    Dim iLastDisplay As Integer = 0
    '    Dim iPageNumber As Integer = 0
    '    Dim iCellCount As Integer = 0
    '    Dim iCurPage As Integer = mhUTIL.GetDBInteger(oCurPage)

    '    Dim iRow As Integer = 0

    '    Dim dtPageImage As New DataTable

    '    If dtPageImage.Rows.Count = 0 Then
    '        ' No Rows
    '    Else
    '        ' Count number of Images for this PageID, SETUP FOR REMAINDER OF TASKS
    '        For Each myrow As DataRow In dtPageImage.Rows
    '            If myrow.Item("PageID") = EditPageID Then
    '                iImageCount = iImageCount + 1
    '                iRecordsPerPage = (Val(myrow.Item("ImagesPerRow"))) * Val(myrow.Item("RowsPerPage"))
    '                iCellCount = myrow.Item("ImagesPerRow")
    '                ' Calculate the the width for the TD
    '                If (Val(myrow.Item("ImagesPerRow")) > 0) Then
    '                    sCellWidth = System.Math.Round(1 / Val(myrow.Item("ImagesPerRow")) * 100, 0) & "%"
    '                End If
    '            End If
    '        Next
    '        iPageCount = 1
    '        ' determine page break
    '        If (iImageCount > 0 And iRecordsPerPage > 0) Then
    '            iPageCount = Math.Round(iImageCount / iRecordsPerPage, 0)
    '            ' If last image is on a page break, then subtract one page to avoid an empty page
    '            If (iPageCount * iRecordsPerPage) > iImageCount Then
    '                ' Do nothing we have the right number of pages
    '            Else
    '                If (iImageCount Mod iRecordsPerPage) = 0 Then
    '                Else
    '                    iPageCount = iPageCount + 1
    '                End If
    '            End If
    '        End If
    '        ' Page Description at the top
    '        If (iPageCount > 1) Then
    '            myStringBuilder.Append(CurrentPageDescription & "<br />")
    '        End If

    '        ' Determine First and Last record to display
    '        If iImageCount > 0 Then
    '            If iCurPage <= 1 Then
    '                ' We are on the first page
    '                iPageNumber = 1
    '                iFirstDisplay = 0
    '                iLastDisplay = iRecordsPerPage - 1
    '            Else
    '                iPageNumber = CShort(iCurPage)
    '                iFirstDisplay = ((iPageNumber * iRecordsPerPage) - iRecordsPerPage)
    '                iLastDisplay = iFirstDisplay + iRecordsPerPage - 1
    '            End If
    '        End If
    '        ' Build Sub Page Navigation
    '        If iPageNumber > 1 Then
    '            sSubPageNav = sSubPageNav & "<a title=""PREVIOUS"" href=""" & sURL & "&amp;Page=" & iPageNumber - 1 & """><::</a>"
    '        End If
    '        If (iPageCount > 1) Then
    '            sSubPageNav = sSubPageNav & "<b>Page " & iPageNumber & " of " & iPageCount & "</b>"
    '        End If
    '        If iPageNumber < iPageCount Then
    '            sSubPageNav = sSubPageNav & "<a title=""NEXT"" href=""" & sURL & "&amp;Page=" & iPageNumber + 1 & """>::></a>"
    '        End If
    '        If sSubPageNav <> "" Then
    '            myStringBuilder.Append("<center>" & sSubPageNav & "</center>" & vbCrLf & vbCrLf)
    '        End If
    '        ' Draw The current page
    '        myStringBuilder.Append("<table style=""border:0;"" >" & vbCrLf & "<tr>" & vbCrLf)
    '        For Each myrow As DataRow In dtPageImage.Rows
    '            If myrow.Item("PageID") = EditPageID Then
    '                If iRecordsPerPage = 0 Then
    '                    ' Show Image Detail for first record
    '                    mhfio.ReadFile(HttpContext.Current.Server.MapPath(SiteGallery & "/catalog_template.html"), myStringBuilder)
    '                    ShowImageDetail(myrow.Item("ImageID"), myrow.Item("PageID"), myStringBuilder, SiteGallery)
    '                    Exit For
    '                Else
    '                    ' Determine if this record is displayed
    '                    If (iRow >= iFirstDisplay) Then
    '                        If (iRow <= iLastDisplay) Then
    '                            If (Val(myrow.Item("ImagesPerRow")) = 1) Then
    '                                If (mhwcm.IsBreak(iRow, 2)) Then
    '                                    myStringBuilder.Append("<td align=""right"" width=""" & sCellWidth & _
    '                                          """><img align=""middle"" alt=""" & myrow.Item("ImageName") & _
    '                                          """ src=""" & GetImagePath(SiteGallery, myrow.Item("ImageThumbFileName"), Trim(myrow.Item("PageName"))) & """ " & _
    '                                          """ style=""border:0;"" /></td>" & _
    '                                          vbCrLf)
    '                                Else
    '                                    myStringBuilder.Append("<td align=""left"" width=""" & sCellWidth & _
    '                                           """><img align=""middle"" alt=""" & myrow.Item("ImageName") & """ style=""border:0;"" " & _
    '                                           "src=""" & GetImagePath(SiteGallery, myrow.Item("ImageThumbFileName"), Trim(myrow.Item("PageName"))) & """/></td>" & _
    '                                           vbCrLf)
    '                                End If
    '                            Else
    '                                myStringBuilder.Append("<td align=""center"" valign=""top"" width=""" & sCellWidth & _
    '                                           """><a href=""" & mhUTIL.FormatPageNameURL(myrow.Item("PageName") & "-" & myrow.Item("ImageName")) & """>" & _
    '                                           "<img align=""top"" style=""border:0;"" alt=""" & myrow.Item("ImageName") & _
    '                                           """ src=""" & GetImagePath(SiteGallery, myrow.Item("ImageThumbFileName"), Trim(myrow.Item("PageName"))) & """ /></a></td>" & _
    '                                           vbCrLf)
    '                            End If
    '                            iCellCount = (iCellCount - 1)
    '                            If (mhwcm.IsBreak(iRow, Val(myrow.Item("ImagesPerRow")))) Then
    '                                myStringBuilder.Append("</tr>" & vbCrLf & "<tr>" & vbCrLf)
    '                                iCellCount = Val(myrow.Item("ImagesPerRow"))
    '                            End If
    '                        End If
    '                    End If
    '                    iRow = iRow + 1
    '                End If
    '            End If
    '        Next
    '    End If
    '    ' Need to figure out how many cells to add to complete the row, mabye a colspan?
    '    If iCellCount > 0 Then
    '        myStringBuilder.Append("<td colspan=""" & iCellCount & """ >&nbsp;</td>" & vbCrLf)
    '    End If
    '    myStringBuilder.Append("</tr>" & vbCrLf & "</table>" & vbCrLf & vbCrLf)
    '    myStringBuilder.Append("<center>" & sSubPageNav & "</center>" & vbCrLf & vbCrLf)
    '    myStringBuilder.Append(vbCrLf)
    '    Return myStringBuilder.ToString
    'End Function

    Public Function BuildCMotionLBPageImage(ByVal EditPageID As String, ByVal CurrentArticleID As String, ByVal SiteGallery As String) As String
        Dim myStringBuilder As StringBuilder = New StringBuilder("")
        Dim dtPageImage As New DataTable
        Dim sURL As String = ("/mhweb/catalog/catalog.aspx?c=" & EditPageID & "&amp;a=" & CurrentArticleID)
        dtPageImage = mhDataCon.GetPageImage(EditPageID, mySiteMap.mySession.CompanyID, mySiteMap.mySession.GroupID)
        If dtPageImage.Rows.Count > 0 Then
            myStringBuilder.Append("<div id=""motioncontainer"" style=""position:relative;overflow:hidden;valign:top;"">" & vbCrLf)
            myStringBuilder.Append("<div id=""motiongallery"" style=""position:relative;left:0px;top:0px;white-space:nowrap;valign:top;"">" & vbCrLf & vbCrLf)
            myStringBuilder.Append("<nobr id=""trueContainer"">" & vbCrLf)
            For Each myrow As DataRow In dtPageImage.Rows
                If myrow.Item("PageID").ToString = EditPageID Then
                    myStringBuilder.Append("<a title=""" & myrow.Item("ImageDescription").ToString & _
                                           """ href=""" & GetImagePath(SiteGallery, myrow.Item("ImageFileName").ToString, Trim(myrow.Item("PageName").ToString), sImageWidth) & _
                                           """ rel=""lightbox[" & Trim(myrow.Item("PageName").ToString) & "]"" >" & _
                                           "<img style=""align:text-top;border:0;"" alt=""" & myrow.Item("ImageDescription").ToString & _
                                           """ src=""" & GetImagePathHeight(SiteGallery, myrow.Item("ImageFileName").ToString, Trim(myrow.Item("PageName").ToString), "150") & """  /></a> " & vbCrLf)
                End If
            Next
            myStringBuilder.Append("</nobr>" & vbCrLf)
            myStringBuilder.Append("</div>" & vbCrLf)
            myStringBuilder.Append("</div>" & vbCrLf)
        Else
            myStringBuilder.Append("<P STYLE=""font-size:10pt;"">" & vbCrLf)
            myStringBuilder.Append("No Image Results Found!<br/>" & vbCrLf)
            myStringBuilder.Append("BuildCMotionPageImage(EditPageID=" & EditPageID & ",CurrentArticleID=" & CurrentArticleID & ",SiteGallery=" & SiteGallery & ")<br/>" & vbCrLf)
            myStringBuilder.Append("</P>" & vbCrLf)
        End If
        Return myStringBuilder.ToString
    End Function

    Public Function BuildCMotionPageImage(ByVal EditPageID As String, ByVal CurrentArticleID As String, ByVal SiteGallery As String) As String
        Dim myStringBuilder As StringBuilder = New StringBuilder("")
        Dim dtPageImage As New DataTable
        Dim sURL As String = ("/mhweb/catalog/catalog.aspx?c=" & EditPageID & "&amp;a=" & CurrentArticleID)
        dtPageImage = mhDataCon.GetPageImage(EditPageID, mySiteMap.mySession.CompanyID, mySiteMap.mySession.GroupID)
        If dtPageImage.Rows.Count > 0 Then
            myStringBuilder.Append("<div id=""motioncontainer"" style=""position:relative;overflow:hidden;valign:top;"">" & vbCrLf)
            myStringBuilder.Append("<div id=""motiongallery"" style=""position:relative;left:0px;top:0px;white-space:nowrap;valign:top;"">" & vbCrLf & vbCrLf)
            myStringBuilder.Append("<nobr id=""trueContainer"">" & vbCrLf)
            For Each myrow As DataRow In dtPageImage.Rows
                If myrow.Item("PageID").ToString = EditPageID Then
                    myStringBuilder.Append("<a title=""" & myrow.Item("ImageDescription").ToString & _
                                           """ href=""" & mhUTIL.FormatPageNameURL(myrow.Item("PageName").ToString & "-" & myrow.Item("ImageName").ToString) & _
                                           """><img style=""align:text-top;border:0;"" alt=""" & myrow.Item("ImageDescription").ToString & _
                                           """ src=""" & GetImagePathHeight(SiteGallery, myrow.Item("ImageFileName").ToString, Trim(myrow.Item("PageName").ToString), "150") & """  /></a> " & vbCrLf)
                End If
            Next
            myStringBuilder.Append("</nobr>" & vbCrLf)
            myStringBuilder.Append("</div>" & vbCrLf)
            myStringBuilder.Append("</div>" & vbCrLf)
        Else
            myStringBuilder.Append("<P STYLE=""font-size:10pt;"">" & vbCrLf)
            myStringBuilder.Append("No Image Results Found!<br/>" & vbCrLf)
            myStringBuilder.Append("BuildCMotionPageImage(EditPageID=" & EditPageID & ",CurrentArticleID=" & CurrentArticleID & ",SiteGallery=" & SiteGallery & ")<br/>" & vbCrLf)
            myStringBuilder.Append("</P>" & vbCrLf)
        End If
        Return myStringBuilder.ToString
    End Function
    Public Function BuildSliderPageImage(ByVal EditPageID As String, ByVal CurrentArticleID As String, ByVal SiteGallery As String) As String
        Dim myStringBuilder As StringBuilder = New StringBuilder("")
        Dim dtPageImage As New DataTable
        Dim iRowCount As Integer = 0
        Dim sURL As String = ("/mhweb/catalog/catalog.aspx?c=" & EditPageID & "&amp;a=" & CurrentArticleID)
        dtPageImage = mhDataCon.GetPageImage(EditPageID, mySiteMap.mySession.CompanyID, mySiteMap.mySession.GroupID)
        If dtPageImage.Rows.Count > 0 Then
            For Each myrow As DataRow In dtPageImage.Rows
                If myrow.Item("PageID").ToString = EditPageID Then
                    myStringBuilder.Append("leftrightslide[" & iRowCount.ToString & "]='")
                    myStringBuilder.Append("<a title=""" & myrow.Item("ImageDescription").ToString & _
                                           """ href=""" & mhUTIL.FormatPageNameURL(myrow.Item("PageName").ToString & "-" & myrow.Item("ImageName").ToString) & _
                                           """><img style=""align:text-top;border:0;"" alt=""" & myrow.Item("ImageDescription").ToString & _
                                           """ src=""" & GetImagePathHeight(SiteGallery, myrow.Item("ImageFileName").ToString, Trim(myrow.Item("PageName").ToString), "410") & """  /></a> ")
                    myStringBuilder.Append("'" & vbCrLf)
                End If
                iRowCount = iRowCount + 1
            Next
        Else
            myStringBuilder.Append("<P STYLE=""font-size:10pt;"">" & vbCrLf)
            myStringBuilder.Append("No Image Results Found!<br/>" & vbCrLf)
            myStringBuilder.Append("BuildSliderPageImage(EditPageID=" & EditPageID & ",CurrentArticleID=" & CurrentArticleID & ",SiteGallery=" & SiteGallery & ")<br/>" & vbCrLf)
            myStringBuilder.Append("</P>" & vbCrLf)
        End If
        Return myStringBuilder.ToString
    End Function
    Public Function BuildImageTags(ByVal EditPageID As String, ByVal CurrentArticleID As String, ByVal SiteGallery As String) As String
        Dim myStringBuilder As StringBuilder = New StringBuilder("")
        Dim dtPageImage As New DataTable
        Dim iRowCount As Integer = 0
        Dim sURL As String = ("/mhweb/catalog/catalog.aspx?c=" & EditPageID & "&amp;a=" & CurrentArticleID)
        dtPageImage = mhDataCon.GetPageImage(EditPageID, mySiteMap.mySession.CompanyID, mySiteMap.mySession.GroupID)
        If dtPageImage.Rows.Count > 0 Then
            For Each myrow As DataRow In dtPageImage.Rows
                If myrow.Item("PageID").ToString = EditPageID Then
                    myStringBuilder.Append("<img title=""" & myrow.Item("Title").ToString & _
                                           """ src=""" & GetImagePath(SiteGallery, myrow.Item("ImageFileName").ToString, Trim(myrow.Item("PageName").ToString), sImageWidth) & """  />")
                    myStringBuilder.Append("'" & vbCrLf)
                End If
                iRowCount = iRowCount + 1
            Next
        Else
            myStringBuilder.Append("<P STYLE=""font-size:10pt;"">" & vbCrLf)
            myStringBuilder.Append("No Image Results Found!<br/>" & vbCrLf)
            myStringBuilder.Append("</P>" & vbCrLf)
        End If
        Return myStringBuilder.ToString
    End Function
    Public Function BuildSliderPageThumbnail(ByVal EditPageID As String, ByVal CurrentArticleID As String, ByVal SiteGallery As String) As String
        Dim myStringBuilder As StringBuilder = New StringBuilder("")
        Dim dtPageImage As New DataTable
        Dim iRowCount As Integer = 0
        Dim sImageThumbnailPath As String = ""
        Dim sURL As String = ("/mhweb/catalog/catalog.aspx?c=" & EditPageID & "&amp;a=" & CurrentArticleID)
        dtPageImage = mhDataCon.GetPageImage(EditPageID, mySiteMap.mySession.CompanyID, mySiteMap.mySession.GroupID)
        If dtPageImage.Rows.Count > 0 Then
            For Each myrow As DataRow In dtPageImage.Rows
                If myrow.Item("PageID").ToString = EditPageID Then
                    sImageThumbnailPath = GetImagePath(SiteGallery, Replace(myrow.Item("ImageFileName").ToString, "image", "thumbnail"), "")
                    myStringBuilder.Append("leftrightslide[" & iRowCount.ToString & "]='")
                    myStringBuilder.Append("<a title=""" & myrow.Item("ImageDescription").ToString & _
                                           """ href=""" & mhUTIL.FormatPageNameURL(myrow.Item("PageName").ToString & "-" & myrow.Item("ImageName").ToString) & _
                                           """><img style=""align:text-top;border:0;"" alt=""" & myrow.Item("ImageDescription").ToString & _
                                           """ src=""" & sImageThumbnailPath & """  /></a> ")
                    myStringBuilder.Append("'" & vbCrLf)
                End If
                iRowCount = iRowCount + 1
            Next
        Else
            myStringBuilder.Append("<P STYLE=""font-size:10pt;"">" & vbCrLf)
            myStringBuilder.Append("No Image Results Found!<br/>" & vbCrLf)
            myStringBuilder.Append("BuildSliderPageImage(EditPageID=" & EditPageID & ",CurrentArticleID=" & CurrentArticleID & ",SiteGallery=" & SiteGallery & ")<br/>" & vbCrLf)
            myStringBuilder.Append("</P>" & vbCrLf)
        End If
        Return myStringBuilder.ToString
    End Function
    Public Function BuildLightboxPageImage(ByVal EditPageID As String, ByVal CurrentArticleID As String, ByVal SiteGallery As String) As String
        Dim myStringBuilder As StringBuilder = New StringBuilder("")
        Dim dtPageImage As New DataTable
        Dim sURL As String = ("/mhweb/catalog/catalog.aspx?c=" & EditPageID & "&amp;a=" & CurrentArticleID)
        dtPageImage = mhDataCon.GetPageImage(EditPageID, mySiteMap.mySession.CompanyID, mySiteMap.mySession.GroupID)
        If dtPageImage.Rows.Count > 0 Then
            For Each myrow As DataRow In dtPageImage.Rows
                If myrow.Item("PageID").ToString = EditPageID Then
                    myStringBuilder.Append("<a title=""" & myrow.Item("ImageDescription").ToString & """ " & _
                                           "href=""" & GetImagePath(SiteGallery, myrow.Item("ImageFileName").ToString, Trim(myrow.Item("PageName").ToString), sImageWidth) & """ " & _
                                           "rel=""lightbox[" & Trim(myrow.Item("PageName").ToString) & "]"" >" & _
                                           "<img style=""align:text-top;border:0;"" alt=""" & myrow.Item("ImageDescription").ToString & _
                                           """ src=""" & GetImagePath(SiteGallery, myrow.Item("ImageFileName").ToString, Trim(myrow.Item("PageName").ToString), sThumbnailWidth) & """  /></a> " & vbCrLf)
                End If
            Next
        Else
            myStringBuilder.Append("<P STYLE=""font-size:10pt;"">" & vbCrLf)
            myStringBuilder.Append("No Image Results Found!<br/>" & vbCrLf)
            myStringBuilder.Append("BuildLightboxPageImage(EditPageID=" & EditPageID & ",CurrentArticleID=" & CurrentArticleID & ",SiteGallery=" & SiteGallery & ")<br/>" & vbCrLf)
            myStringBuilder.Append("</P>" & vbCrLf)
        End If
        Return myStringBuilder.ToString
    End Function
    Public Function BuildLightboxSliderPageThumbnail(ByVal EditPageID As String, ByVal CurrentArticleID As String, ByVal SiteGallery As String) As String
        Dim myStringBuilder As StringBuilder = New StringBuilder("")
        Dim dtPageImage As New DataTable
        Dim iRowCount As Integer = 0
        Dim sURL As String = ("/mhweb/catalog/catalog.aspx?c=" & EditPageID & "&amp;a=" & CurrentArticleID)
        dtPageImage = mhDataCon.GetPageImage(EditPageID, mySiteMap.mySession.CompanyID, mySiteMap.mySession.GroupID)
        If dtPageImage.Rows.Count > 0 Then
            For Each myrow As DataRow In dtPageImage.Rows
                If myrow.Item("PageID").ToString = EditPageID Then
                    myStringBuilder.Append("leftrightslide[" & iRowCount.ToString & "]='")
                    myStringBuilder.Append("<a title=""" & myrow.Item("ImageDescription").ToString & _
                                           """ href=""" & GetImagePath(SiteGallery, myrow.Item("ImageFileName").ToString, Trim(myrow.Item("PageName").ToString)) & _
                                           """ rel=""lightbox[" & Trim(myrow.Item("PageName").ToString) & "]"" >" & _
                                           "<img style=""align:text-top;border:0;"" alt=""" & myrow.Item("ImageDescription").ToString & _
                                           """ src=""" & GetImagePathHeight(SiteGallery, myrow.Item("ImageFileName").ToString, Trim(myrow.Item("PageName").ToString), "100") & """  /></a> ")
                    myStringBuilder.Append("'" & vbCrLf)
                End If
                iRowCount = iRowCount + 1
            Next
        Else
            myStringBuilder.Append("<P STYLE=""font-size:10pt;"">" & vbCrLf)
            myStringBuilder.Append("No Image Results Found!<br/>" & vbCrLf)
            myStringBuilder.Append("BuildSliderPageImage(EditPageID=" & EditPageID & ",CurrentArticleID=" & CurrentArticleID & ",SiteGallery=" & SiteGallery & ")<br/>" & vbCrLf)
            myStringBuilder.Append("</P>" & vbCrLf)
        End If
        Return myStringBuilder.ToString
    End Function
    Public Function BuildPhotoalbumPageImage(ByVal EditPageID As String, ByVal CurrentArticleID As String, ByVal SiteGallery As String) As String
        Dim myPageName As String = ("")
        Dim myPageRows As Integer = (3)
        Dim myPageCols As Integer = (3)
        Dim myStringBuilder As StringBuilder = New StringBuilder("")
        Dim dtPageImage As New DataTable
        Dim imgDescription As String = ("")
        Dim sURL As String = ("/mhweb/catalog/catalog.aspx?c=" & EditPageID & "&amp;a=" & CurrentArticleID)
        myStringBuilder.Append("<sc" & "ript type=""text/javascript"">" & vbCrLf)
        myStringBuilder.Append("var photoalbum=new Array() " & vbCrLf)
        Dim iImageNumber As Integer = 0
        dtPageImage = mhDataCon.GetPageImage(EditPageID, mySiteMap.mySession.CompanyID, mySiteMap.mySession.GroupID)
        If dtPageImage.Rows.Count > 0 Then
            For Each myrow As DataRow In dtPageImage.Rows
                If myrow.Item("PageID").ToString = EditPageID Then
                    mhUTIL.GetDBString(myrow.Item("ImageDescription"))
                    myStringBuilder.Append("photoalbum[" & iImageNumber & "]=[""" & GetImagePath(SiteGallery, myrow.Item("ImageFileName").ToString, Trim(myrow.Item("PageName").ToString), sThumbnailWidth) & """  " & _
                          ",""" & imgDescription & """,""" & _
                          mhUTIL.FixInvalidCharacters(myrow.Item("PageName").ToString & "-" & myrow.Item("ImageName").ToString & ".html") & """,""_self""] " & vbCrLf)
                    iImageNumber = iImageNumber + 1
                    myPageName = myrow.Item("PageName").ToString
                    myPageCols = mhUTIL.GetDBInteger(myrow.Item("ImagesPerRow"))
                    myPageRows = mhUTIL.GetDBInteger(myrow.Item("RowsPerPage"))
                    If myPageCols = 0 Then myPageCols = 3
                    If myPageRows = 0 Then myPageRows = 3
                End If
            Next
        Else
            myStringBuilder.Append("<P STYLE=""font-size:10pt;"">" & vbCrLf)
            myStringBuilder.Append("No Image Results Found!" & vbCrLf)
            myStringBuilder.Append("</P>" & vbCrLf)
        End If

        myStringBuilder.Append(vbCrLf & vbCrLf & "var thepics=new photogallery(photoalbum, " & myPageCols.ToString & ", " & myPageRows.ToString & ", '600px', '600px',['Browse " & myPageName & " Photo Gallery:','Page '])" & vbCrLf)
        ' This code is if you want a PopUP when an image is clicked on
        'myStringBuilder.Append("thepics.onselectphoto=function(img, link){ " & vbCrLf)
        'myStringBuilder.Append("if (link!=null) " & vbCrLf)
        'myStringBuilder.Append("    window.open(link.href, """", ""width=800, height=600, status=1, resizable=1"")" & vbCrLf)
        'myStringBuilder.Append("    return false " & vbCrLf)
        'myStringBuilder.Append("} " & vbCrLf)
        myStringBuilder.Append("</sc" & "ript>" & vbCrLf & vbCrLf)
        Return myStringBuilder.ToString
    End Function
    Public Function BuildFadeImage(ByVal EditPageID As String, ByVal CurrentArticleID As String, ByVal SiteGallery As String) As String
        Dim myPageName As String = ("")
        Dim myPageRows As String = ("")
        Dim myPageCols As String = ("")
        Dim myStringBuilder As StringBuilder = New StringBuilder("")
        Dim dtPageImage As New DataTable
        Dim imgDescription As String = ("")
        Dim sURL As String = ("/mhweb/catalog/catalog.aspx?c=" & EditPageID & "&amp;a=" & CurrentArticleID)
        myStringBuilder.Append("var photoalbum=new Array() " & vbCrLf)
        Dim iImageNumber As Integer = 0
        dtPageImage = mhDataCon.GetPageImage(EditPageID, mySiteMap.mySession.CompanyID, mySiteMap.mySession.GroupID)
        If dtPageImage.Rows.Count > 0 Then
            For Each myrow As DataRow In dtPageImage.Rows
                If myrow.Item("PageID").ToString = EditPageID Then
                    imgDescription = mhUTIL.GetDBString(myrow.Item("ImageDescription"))
                    myStringBuilder.Append("photoalbum[" & iImageNumber & "]=[""" & GetImagePath(SiteGallery, myrow.Item("ImageFileName").ToString, Trim(myrow.Item("PageName").ToString), sImageWidth) & """  " & _
                          ",""" & mhUTIL.FormatNameForURL(myrow.Item("PageName").ToString & "-" & myrow.Item("ImageName").ToString & ".html") & """,""" & _
                           """] " & vbCrLf)
                    iImageNumber = iImageNumber + 1
                    myPageName = myrow.Item("PageName").ToString
                    myPageCols = myrow.Item("ImagesPerRow").ToString
                    myPageRows = myrow.Item("RowsPerPage").ToString
                End If
            Next
        Else
            myStringBuilder.Append("<P STYLE=""font-size:10pt;"">" & vbCrLf)
            myStringBuilder.Append("No Image Results Found!" & vbCrLf)
            myStringBuilder.Append("</P>" & vbCrLf)
        End If
        Return myStringBuilder.ToString
    End Function

    Public Function GetImagePath(ByVal SiteGallery As String, ByVal ImagePath As String, ByVal PageName As String) As String
        Dim myReturn As String

        If mySiteMap.mySiteFile.SingleSiteGallery Then
            myReturn = SiteGallery & ImagePath
        Else
            myReturn = SiteGallery & "pages/" & mhUTIL.FixInvalidCharacters(PageName) & "/" & ImagePath
        End If
        myReturn = Replace(myReturn, "\", "/")
        Return Replace(myReturn, "//", "/")
    End Function
    Public Function GetImagePath(ByVal SiteGallery As String, ByVal ImagePath As String, ByVal PageName As String, ByVal ImageWidth As String) As String
        Dim myReturn As String
        If mySiteMap.mySiteFile.SingleSiteGallery Then
            myReturn = SiteGallery & ImagePath
        Else
            myReturn = SiteGallery & "pages/" & mhUTIL.FixInvalidCharacters(PageName) & "/" & ImagePath
        End If
        myReturn = Replace(myReturn, "\", "/")
        Return "/mhweb/catalog/ImageResize.aspx?w=" & ImageWidth & "&img=" & Replace(myReturn, "//", "/")
    End Function
    Public Function GetImagePathHeight(ByVal SiteGallery As String, ByVal ImagePath As String, ByVal PageName As String, ByVal ImageHeight As String) As String
        Dim myReturn As String
        If mySiteMap.mySiteFile.SingleSiteGallery Then
            myReturn = SiteGallery & ImagePath
        Else
            myReturn = SiteGallery & "pages/" & mhUTIL.FixInvalidCharacters(PageName) & "/" & ImagePath
        End If
        myReturn = Replace(myReturn, "\", "/")
        Return "/mhweb/catalog/ImageResize.aspx?h=" & ImageHeight & "&img=" & Replace(myReturn, "//", "/")
    End Function
    ' -----------------------------------------------------------------------------
    ' This Function is to merge and article and PageImageList
    ' -----------------------------------------------------------------------------
    Private Function GetImageTextMerge(ByVal ReqPageID As String, ByVal strText As String, ByVal bAddLink As Boolean, ByRef sImageDesc As String, ByVal CurrentPageID As String, ByVal SiteGallery As String) As String
        Dim iImageCount As Integer = 0
        Dim stemp As String
        Dim dtPageImage As New DataTable
        dtPageImage = mhDataCon.GetPageImage(ReqPageID, "1", "4")
        If dtPageImage.Rows.Count > 0 Then
            sImageDesc = "<div id=""imagedesc"">Photos left:<ul>"
            For Each myrow As DataRow In dtPageImage.Rows
                If myrow.Item("PageID").ToString = CurrentPageID Then
                    If (bAddLink) Then
                        ' Replace a number in brackets with the thumbnail and link to the larger image
                        stemp = "<div id=""imagedesc""><a href=""" & _
                           mhUTIL.FixInvalidCharacters(mhUTIL.GetDBString(myrow.Item("PageName")) & _
                           "-" & mhUTIL.GetDBString(myrow.Item("ImageName"))) & _
                           """><img align=""top"" alt='" & mhUTIL.GetDBString(myrow.Item("ImageDescription")) & _
                           "' src=""" & GetImagePath(SiteGallery, mhUTIL.GetDBString(myrow.Item("ImageFileName")), Trim(mhUTIL.GetDBString(myrow.Item("PageName"))), sThumbnailWidth) & _
                           """ /></a></div>"
                        strText = Replace(strText, "<" & iImageCount & ">", stemp)
                        strText = Replace(strText, "~" & iImageCount & "~", stemp)
                    Else
                        ' Replace a number in brackets with the thumbnail and link to the larger image
                        stemp = "<img align=""top"" src=""" & SiteGallery & GetImagePath(SiteGallery, mhUTIL.GetDBString(myrow.Item("ImageFileName")), Trim(mhUTIL.GetDBString(myrow.Item("PageName"))), sThumbnailWidth) & """ />"
                        strText = Replace(strText, "<" & iImageCount & ">", stemp)
                        strText = Replace(strText, "~" & iImageCount & "~", stemp)
                    End If
                    ' Replace custom page description tag with Page Description
                    strText = Replace(strText, "<$PageDescription$>", mhUTIL.GetDBString(myrow.Item("PageDescription")))
                    strText = Replace(strText, "~$PageDescription$~", mhUTIL.GetDBString(myrow.Item("PageDescription")))
                    ' Replace custom image description tag with Image Description
                    strText = Replace(strText, "<$imageDescription" & iImageCount & "$>", stemp & "<br />" & mhUTIL.GetDBString(myrow.Item("ImageDescription")))
                    strText = Replace(strText, "~$imageDescription" & iImageCount & "$~", stemp & "<br />" & mhUTIL.GetDBString(myrow.Item("ImageDescription")))
                    sImageDesc = sImageDesc & "<li>" & mhUTIL.GetDBString(myrow.Item("ImageDescription")) & "</li>"
                    iImageCount = iImageCount + 1
                End If
            Next
            sImageDesc = sImageDesc & "</ul></div>"
        End If
        Return strText
    End Function
    Private Function ProcesscatalogPage(ByRef sImageDesc As String, ByVal CurrentPageID As String, ByVal DefaultArticleID As String, ByVal SiteGallery As String) As String
        Dim strArticleText As String = ""
        Dim myArticle As New mhArticle("", CurrentPageID, DefaultArticleID)
        Return GetImageTextMerge(CurrentPageID, myArticle.ArticleBody, True, sImageDesc, CurrentPageID, SiteGallery)
    End Function
    Private Function GetPageNumber(ByVal iItemNumber As Integer, ByVal iItemsPerPage As Integer) As Integer
        Dim iPageNumber As Integer
        If (iItemsPerPage > 0) Then
            If (iItemsPerPage >= iItemNumber) Then
                iPageNumber = 1
            Else
                If (iItemNumber Mod iItemsPerPage) = 0 Then
                    iPageNumber = CInt(iItemNumber / iItemsPerPage)
                Else
                    If (iItemNumber Mod iItemsPerPage) > 0.5 Then
                        iPageNumber = CInt(System.Math.Round(iItemNumber / iItemsPerPage, 4)) + 1
                    Else
                        iPageNumber = CInt(System.Math.Round(iItemNumber / iItemsPerPage, 4))
                    End If
                End If
            End If
        Else
            iPageNumber = 1
        End If
        Return iPageNumber
    End Function
    Private Function BuildImageAdmin(ByVal ImageID As String, ByVal PageID As String) As String
        If mhUser.IsEditor() Then
            Return "<hr><a href=""" & mhConfig.mhWebHome & "admin/mhImageEdit.aspx?a=" & ImageID & "&c=" & PageID & """>Edit This Image</a> - " & _
                   "<a href=""" & mhConfig.mhWebHome & "admin/mhImageEdit.aspx?a=" & ImageID & "&c=" & PageID & """>Delete</a><br>"
        Else
            Return ""
        End If
    End Function

    Private Function BuildNavigation(ByVal PrevPageName As String, ByVal GalleryName As String, ByVal NextPageName As String, ByRef sbCatalog As StringBuilder) As Boolean
        If (PrevPageName <> "") Then
            sbCatalog.Replace("<PrevImg>", "<a href=""" & mhUTIL.FormatPageNameURL(GalleryName & "-" & PrevPageName) & """>« &nbsp;" & PrevPageName & "</a>")
            sbCatalog.Replace("<PrevImgURL>", mhUTIL.FormatPageNameURL(GalleryName & "-" & PrevPageName))
        Else
            sbCatalog.Replace("<PrevImg>", "")
            sbCatalog.Replace("<PrevImgURL>", "")
        End If
        If (NextPageName <> "") Then
            sbCatalog.Replace("<NextImg>", "<a href=""" & mhUTIL.FormatPageNameURL(GalleryName & "-" & NextPageName) & """>" & NextPageName & " &nbsp;»</a>")
            sbCatalog.Replace("<NextImgURL>", mhUTIL.FormatPageNameURL(GalleryName & "-" & NextPageName))
        Else
            sbCatalog.Replace("<NextImg>", "")
            sbCatalog.Replace("<NextImgURL>", "")
        End If
        Return True
    End Function
    Private Function ShowImageDetail(ByVal sImageID As String, ByVal PageID As String, ByRef sbCatalog As StringBuilder, ByVal SiteGallery As String) As String
        Dim pageNumber As Integer = 0
        Dim intImagesPerPage As Integer = 0
        Dim PrevImageName As String = ("")
        Dim NextImageName As String = ("")
        Dim iRow As Integer = 0
        Dim myrow As DataRow
        Dim intPageImageOrder As Integer = 0
        Dim dtPageImage As New DataTable
        dtPageImage = mhDataCon.GetPageImage(PageID, mySiteMap.mySession.CompanyID, mySiteMap.mySession.GroupID)

        For iRow = 0 To dtPageImage.Rows.Count - 1
            myrow = dtPageImage.Rows.Item(iRow)

            If mhUTIL.GetDBString(myrow.Item("PageID")) = PageID Then
                intPageImageOrder = intPageImageOrder + 1
                intImagesPerPage = CInt(myrow.Item("ImagesPerRow")) * CInt(myrow.Item("RowsPerPage"))
                pageNumber = GetPageNumber(intPageImageOrder, intImagesPerPage)
                If Val(myrow.Item("ImageID")) = Val(sImageID) Then
                    If intPageImageOrder > 1 Then
                        PrevImageName = mhUTIL.GetDBString(dtPageImage.Rows.Item(iRow - 1).Item("ImageName"))
                    Else
                        PrevImageName = mhUTIL.GetDBString(dtPageImage.Rows.Item(dtPageImage.Rows.Count - 1).Item("ImageName"))
                    End If
                    If iRow + 1 <= dtPageImage.Rows.Count - 1 Then
                        NextImageName = mhUTIL.GetDBString(dtPageImage.Rows.Item(iRow + 1).Item("ImageName"))
                    Else
                        NextImageName = mhUTIL.GetDBString(dtPageImage.Rows.Item(0).Item("ImageName"))
                    End If
                    sbCatalog.Replace("<ImgName>", mhUTIL.GetDBString(myrow.Item("ImageName")))
                    sbCatalog.Replace("<ImgDesc>", mhUTIL.GetDBString(myrow.Item("ImageDescription")))
                    sbCatalog.Replace("<ImgTag>", ("<a title=""" & mhUTIL.GetDBString(myrow.Item("ImageDescription")) & _
                                                    """ href=""" & mhUTIL.FixInvalidCharacters(mhUTIL.GetDBString(myrow.Item("PageName"))) & """>" & _
                                                    "<img src=""" & GetImagePath(SiteGallery, mhUTIL.GetDBString(myrow.Item("ImageFileName")), Trim(mhUTIL.GetDBString(myrow.Item("PageName"))), sImageWidth) & """  " & _
                                                         "alt=""" & mhUTIL.GetDBString(myrow.Item("ImageDescription")) & """ border=""0"" /></a>"))
                    sbCatalog.Replace("<ImgFileName>", GetImagePath(SiteGallery, mhUTIL.GetDBString(myrow.Item("ImageFileName")), Trim(mhUTIL.GetDBString(myrow.Item("PageName")))))
                    sbCatalog.Replace("<ImgComment>", mhUTIL.GetDBString(myrow.Item("ImageComment")))
                    sbCatalog.Replace("<ImgAdmin>", BuildImageAdmin(mhUTIL.GetDBString(myrow.Item("ImageID")), mhUTIL.GetDBString(myrow.Item("PageID"))))
                    sbCatalog.Replace("<ImgTitle>", mhUTIL.GetDBString(myrow.Item("title")))
                    sbCatalog.Replace("<ImgMedium>", mhUTIL.GetDBString(myrow.Item("medium")))
                    sbCatalog.Replace("<ImgSize>", mhUTIL.GetDBString(myrow.Item("size")))
                    sbCatalog.Replace("<ContactID>", mhUTIL.GetDBString(myrow.Item("ContactID")))
                    sbCatalog.Replace("<HomeLink>", ("<a href=""" & mhUTIL.FormatPageNameURL(mhUTIL.GetDBString(myrow.Item("PageName"))) & """>" & mhUTIL.GetDBString(myrow.Item("PageName")) & "</a>"))
                    BuildNavigation(PrevImageName, mhUTIL.GetDBString(myrow.Item("PageName")), NextImageName, sbCatalog)
                    sbCatalog.Append(BuildImageAdmin(mhUTIL.GetDBString(myrow.Item("ImageID")), mhUTIL.GetDBString(myrow.Item("PageID"))))
                End If
            End If
        Next
        If intPageImageOrder = 0 Then
            sbCatalog.Append("<P STYLE=""font-size:10pt;""><center>No Results Found!<br /><a href=""/default.aspx"">Home</a></center></P>")
        End If
        Return sbCatalog.ToString
    End Function

    Public Function FixFolders(ByVal sPageID As String) As Boolean
        Dim sNewPagePath As String = ("")
        Dim sNewImagePath As String = ("")
        Dim sNewThumbnailPath As String = ("")
        Dim dtPageImage As New DataTable
        If Not Me.mySiteMap.mySiteFile.SingleSiteGallery Then
            For Each myRow As DataRow In mhweb.mhDataCon.GetPageImage(Me.mySiteMap.mySession.CompanyID).Rows
                If mhUTIL.GetDBString(myRow.Item("PageID")) = sPageID Then
                    sNewPagePath = mySiteMap.mySiteFile.SiteGallery & _
                                   "pages/" & _
                                   mhUTIL.FormatPageNameForURL(mhUTIL.GetDBString(myRow.Item("PageName")))
                    ' Create a Folder for Page 
                    sNewPagePath = HttpContext.Current.Server.MapPath(sNewPagePath)
                    mhfio.CreateFolder(sNewPagePath)
                    ' Create a Folder for Page/Image and move this image file
                    sNewImagePath = sNewPagePath & "\image"
                    mhfio.CreateFolder(sNewImagePath)
                    mhfio.MoveFile(HttpContext.Current.Server.MapPath(mySiteMap.mySiteFile.SiteGallery & _
                                   mhUTIL.GetDBString(myRow.Item("ImageFileName"))), _
                                   sNewPagePath & "/" & mhUTIL.GetDBString(myRow.Item("ImageFileName")))
                End If
            Next
        End If

    End Function
End Class
