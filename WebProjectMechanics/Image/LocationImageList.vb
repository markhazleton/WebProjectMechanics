Imports System.Text
Public Class LocationImageList
    Private Const STR_ImgName As String = "<ImgName>"
    Private Const STR_ImgDesc As String = "<ImgDesc>"
    Private Const STR_ImgTag As String = "<ImgTag>"
    Private Const STR_ImgUrl As String = "<ImgUrl>"
    Private Const STR_ImgFileName As String = "<ImgFileName>"
    Private Const STR_ImgComment As String = "<ImgComment>"
    Private Const STR_ImgAdmin As String = "<ImgAdmin>"
    Private Const STR_ImgTitle As String = "<ImgTitle>"
    Private Const STR_ImgMedium As String = "<ImgMedium>"
    Private Const STR_ImgSize As String = "<ImgSize>"
    Private Const STR_ContactID As String = "<ContactID>"
    Private Const STR_HomeLink As String = "<HomeLink>"
    Private ReadOnly _ActiveSite As ActiveCompany
    Public ReadOnly Property ActiveSite() As ActiveCompany
        Get
            Return _ActiveSite
        End Get
    End Property
    Public Images As List(Of LocationImage)
    Public sImageWidth As String = "800"
    Public sThumbnailWidth As String = "150"
    Public iRowsPerPage As Integer = 3
    Public iImagesPerRow As Integer = 3
    Public ImageBaseFolder As String
    Public CatalogBasePageURL As String
    Public CatalogPageName As String
    Public PageFileName As String
    Private ReadOnly _CatalogLocationID As String
    Public ReadOnly Property CatalogPageID() As String
        Get
            Return _CatalogLocationID
        End Get
    End Property

    Public Sub New()
        ' Implement Default Constructor
    End Sub
    Public Sub New(ByRef ActiveSite As ActiveCompany)
        _ActiveSite = ActiveSite
        _CatalogLocationID = ActiveSite.CurrentLocationID
        Images = ActiveSite.GetLocationImages(CatalogPageID)
        Using mydt As DataTable = ApplicationDAL.GetPageImage(ActiveSite.CurrentLocationID, wpm_CurrentSiteID, wpm_GetUserGroup())
            For Each myrow As DataRow In mydt.Rows
                iImagesPerRow = wpm_GetDBInteger(myrow.Item("ImagesPerRow"))
                iRowsPerPage = wpm_GetDBInteger(myrow.Item("RowsPerPage"))
                ImageBaseFolder = String.Format("{0}pages/{1}/", wpm_SiteGallery, wpm_FixInvalidCharacters(myrow.Item("PageName").ToString))
                CatalogBasePageURL = wpm_FixInvalidCharacters(myrow.Item("PageName").ToString & wpm_SiteConfig.DefaultExtension)
                CatalogPageName = wpm_GetDBString(myrow.Item("PageName"))
                PageFileName = wpm_GetDBString(myrow.Item("PageFileName"))
                Exit For
            Next
        End Using
    End Sub

    Public Function ProcessPageRequest(ByVal ImageID As Integer, ByRef myArticle As Location) As String
        Dim sRightContent As String = (String.Empty)
        Dim myStringBuilder As StringBuilder = New StringBuilder(String.Empty)
        If (ImageID < 1) Then
            ' code for thumnail page
            myStringBuilder.Append(GetImageTextMerge(myArticle.LocationBody, True, sRightContent, wpm_SiteGallery))

            If myStringBuilder.ToString.IndexOf("~~pan-lightbox~~") > 0 Then
                Dim myContents As New StringBuilder
                FileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath(String.Format("{0}slider/pan.js", wpm_STR_CatalogPath)), myContents)
                myContents.Replace("<PageImageArray>", BuildLightboxSliderPageThumbnail())
                myStringBuilder.Replace("~~pan-lightbox~~", myContents.ToString)
            End If
            If myStringBuilder.ToString.IndexOf("~~slider~~") > 0 Then
                Dim myContents As New StringBuilder
                FileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath(String.Format("{0}slider/slider.js", wpm_STR_CatalogPath)), myContents)
                myContents.Replace("<PageImageArray>", BuildSliderPageImage())
                myStringBuilder.Replace("~~slider~~", myContents.ToString)
            End If
            If myStringBuilder.ToString.IndexOf("~~pan~~") > 0 Then
                Dim myContents As New StringBuilder
                FileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath(String.Format("{0}slider/pan.js", wpm_STR_CatalogPath)), myContents)
                myContents.Replace("<PageImageArray>", BuildSliderPageThumbnail())
                myStringBuilder.Replace("~~pan~~", myContents.ToString)
            End If
            If myStringBuilder.ToString.IndexOf("~~lightbox~~") > 0 Then
                myStringBuilder.Replace("~~lightbox~~", BuildLightboxPageImage())
                Dim myContents As New StringBuilder
                FileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath(String.Format("{0}lightbox/ADDToHTMLHead.txt", wpm_STR_CatalogPath)), myContents)
                wpm_AddHTMLHead = myContents.ToString
            End If
            If myStringBuilder.ToString.IndexOf("~~thumbnail~~") > 0 Then
                myStringBuilder.Replace("~~thumbnail~~", BuildCMotionPageImage(False))
                Dim myContents As New StringBuilder
                FileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath(String.Format("{0}cmotion/AddToHTMLHead.txt", wpm_STR_CatalogPath)), myContents)
                wpm_AddHTMLHead = myContents.ToString
            End If
            If myStringBuilder.ToString.IndexOf("~~cmotion~~") > 0 Then
                myStringBuilder.Replace("~~cmotion~~", BuildCMotionPageImage(False))
                Dim myContents As New StringBuilder
                FileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath(String.Format("{0}cmotion/AddToHTMLHead.txt", wpm_STR_CatalogPath)), myContents)
                wpm_AddHTMLHead = myContents.ToString
            End If
            If myStringBuilder.ToString.IndexOf("~~cmotionlb~~") > 0 Then
                myStringBuilder.Replace("~~cmotionlb~~", BuildCMotionPageImage(True))
                Dim myContents As New StringBuilder
                FileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath(String.Format("{0}cmotion/AddToHTMLHeadLB.txt", wpm_STR_CatalogPath)), myContents)
                wpm_AddHTMLHead = myContents.ToString
            End If
            If myStringBuilder.ToString.IndexOf("photoalbum") > 0 Then
                myStringBuilder.Replace("~~photoalbum~~", BuildPhotoAlbumPage())
                Dim myContents As New StringBuilder
                FileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath(String.Format("{0}photoalbum/ADDToHTMLHead.txt", wpm_STR_CatalogPath)), myContents)
                wpm_AddHTMLHead = myContents.ToString
            End If
            If myStringBuilder.ToString.IndexOf("~~drill-in~~") > 0 Then
                Dim myContents As New StringBuilder
                FileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath(String.Format("{0}slider/drill-in.js", wpm_STR_CatalogPath)), myContents)
                myContents.Replace("<PageImageArray>", BuildImageTags())
                myStringBuilder.Replace("~~drill-in~~", myContents.ToString)
            End If
            If myStringBuilder.ToString.IndexOf("~~fade~~") > 0 Then
                Dim myContents As New StringBuilder
                FileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath(String.Format("{0}slider/fadeimages.html", wpm_STR_CatalogPath)), myContents)
                myContents.Replace("<PageImageArray>", BuildFadeImage())
                myStringBuilder.Replace("~~fade~~", myContents.ToString)
            End If
            If myStringBuilder.ToString.IndexOf("~~yui-carousel~~") > 0 Then
                myStringBuilder.Replace("~~yui-carousel~~", BuildYUICarouselPageImage())
                Dim myContents As New StringBuilder
                FileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath(String.Format("{0}yui-carousel/ADDToHTMLHead.txt", wpm_STR_CatalogPath)), myContents)
                wpm_AddHTMLHead = myContents.ToString
            End If
            If myStringBuilder.ToString.IndexOf("~~featured~~") > 0 Then
                myStringBuilder.Replace("~~featured~~", String.Format("{0}<div id=""featured"">{0}<PageImageArray>{0}</div>{0}", vbCrLf))
                myStringBuilder.Replace("<PageImageArray>", BuildImageDiv(False))
            End If
            If myStringBuilder.ToString.IndexOf("~~FeaturedContent~~") > 0 Then
                myStringBuilder.Replace("~~FeaturedContent~~", String.Format("{0}<div id=""featured"">{0}<PageImageArray>{0}</div>{0}", vbCrLf))
                myStringBuilder.Replace("<PageImageArray>", BuildImageDiv(True))
            End If
            If myStringBuilder.ToString.IndexOf("~~MovingBoxes~~") > 0 Then
                myStringBuilder.Replace("~~MovingBoxes~~", String.Format("{0}<div>{0}<PageImageArray>{0}</div>{0}", vbCrLf))
                Dim myContents As New StringBuilder
                FileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath(String.Format("/framework/MovingBoxes/ADDToHTMLHead.txt", wpm_STR_CatalogPath)), myContents)
                wpm_AddHTMLHead = myContents.ToString
                myStringBuilder.Replace("<PageImageArray>", BuildImageUL(False, "slider"))
            End If

            If myStringBuilder.Length < 1 Then
                myStringBuilder.Append(BuildPhotoAlbumPage())
                Dim myContents As New StringBuilder
                FileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath(String.Format("{0}photoalbum/ADDToHTMLHead.txt", wpm_STR_CatalogPath)), myContents)
                wpm_AddHTMLHead = myContents.ToString
            End If
        Else
            ' Code for Detail Image
            If PageFileName = String.Empty Then
                If Not FileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath(wpm_SiteGallery & "/catalog_template.html"), myStringBuilder) Then
                    myStringBuilder.Append("<table><tr><td align=""left"" width=""33%""><PrevImg></td><td align=""center"" width=""33%""><HomeLink></td><td align=""right"" width=""33%""><NextImg></td></tr><tr><td colspan=""3""><ImgTag></td></tr><tr><td colspan=""3""><strong><ImgName></strong><br/><ImgDesc></td></tr></table><br/>")
                End If
            Else
                If Not FileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath(String.Format("{0}/{1}_template.html", wpm_SiteGallery, PageFileName)), myStringBuilder) Then
                    If Not FileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath(wpm_SiteGallery & "/catalog_template.html"), myStringBuilder) Then
                        myStringBuilder.Append("<table><tr><td align=""left"" width=""33%""><PrevImg></td><td align=""center"" width=""33%""><HomeLink></td><td align=""right"" width=""33%""><NextImg></td></tr><tr><td colspan=""3""><ImgTag></td></tr><tr><td colspan=""3""><strong><ImgName></strong><br/><ImgDesc></td></tr></table><br/>")
                    End If
                End If
            End If
            ShowImageDetail(ImageID, myStringBuilder)
        End If
        myStringBuilder.Replace("<ImageCount>", Images.Count.ToString)
        Return myStringBuilder.ToString
    End Function
    ' -----------------------------------------------------------------------------
    ' This Function is to merge and article and PageImageList
    ' -----------------------------------------------------------------------------
    Private Function GetImageTextMerge(ByVal strText As String, ByVal bAddLink As Boolean, ByRef sImageDesc As String, ByVal SiteGallery As String) As String
        Dim iImageCount As Integer = 0
        Dim stemp As String
        Using dtPageImage As New DataTable
            sImageDesc = "<div id=""imagedesc"">Photos left:<ul>"
            For Each myImage As LocationImage In Images
                If (bAddLink) Then
                    ' Replace a number in brackets with the thumbnail and link to the larger image
                    stemp = String.Format("<div id=""imagedesc""><a href=""{0}""><img align=""top"" alt='{1}' src=""{2}"" /></a></div>",
                                wpm_FixInvalidCharacters(String.Format("{0}-{1}",
                                                                           ActiveSite.CurrentLocationNM,
                                                                           myImage.ImageName)),
                                myImage.ImageDescription,
                                GetImagePath(myImage.ImageFileName, sThumbnailWidth))
                    strText = strText.Replace(String.Format("<{0}>", iImageCount), stemp)
                    strText = strText.Replace(String.Format("~{0}~", iImageCount), stemp)
                Else
                    ' Replace a number in brackets with the thumbnail and link to the larger image
                    stemp = String.Format("<img align=""top"" src=""{0}{1}"" />", SiteGallery, GetImagePath(myImage.ImageFileName, sThumbnailWidth))
                    strText = strText.Replace(String.Format("<{0}>", iImageCount), stemp)
                    strText = strText.Replace(String.Format("~{0}~", iImageCount), stemp)
                End If
                ' Replace custom page description tag with Page Description
                strText = strText.Replace("<$PageDescription$>", ActiveSite.CurrentLocationDS)
                strText = strText.Replace("~$PageDescription$~", ActiveSite.CurrentLocationDS)
                ' Replace custom image description tag with Image Description
                strText = strText.Replace(String.Format("<$imageDescription{0}$>", iImageCount), String.Format("{0}<br />{1}", stemp, myImage.ImageDescription))
                strText = strText.Replace(String.Format("~$imageDescription{0}$~", iImageCount), String.Format("{0}<br />{1}", stemp, myImage.ImageDescription))
                sImageDesc = String.Format("{0}<li>{1}</li>", sImageDesc, myImage.ImageDescription)
                iImageCount = iImageCount + 1
            Next
            sImageDesc = sImageDesc & "</ul></div>"
        End Using
        Return strText
    End Function
    Private Function BuildPhotoAlbumPage() As String
        Dim myStringBuilder As StringBuilder = New StringBuilder(String.Empty)
        If iImagesPerRow < 1 then
            iImagesPerRow = 3
        End If
        If iRowsPerPage < 1 then
            iRowsPerPage = 3
        End If

        myStringBuilder.Append(String.Format("<script type=""text/javascript"">{0}", vbCrLf))
        myStringBuilder.Append("var photoalbum=new Array() " & vbCrLf)
        Dim iImageNumber As Integer = 0
        If Images.Count > 0 Then
            For Each Image As LocationImage In Images
                myStringBuilder.Append(String.Format("photoalbum[{0}]=[""{1}""  ,""{2}"",""{3}"",""_self""] {4}", iImageNumber, GetImagePath(Image.ImageFileName, sThumbnailWidth), Image.ImageName, Image.ImageURL, vbCrLf))
                iImageNumber = iImageNumber + 1
            Next
        Else
            myStringBuilder.AppendLine(String.Format("<P STYLE=""font-size:10pt;"">{0}No Image Results Found!<br/></p>{0}", vbCrLf))
        End If
        myStringBuilder.AppendLine(String.Format("{0}{0}var thepics=new photogallery(photoalbum, {1}, {2}, '100%', '100%',['Browse {3} Photo Gallery:','Page ']){0}", vbCrLf, iImagesPerRow, iRowsPerPage, CatalogPageName))
        myStringBuilder.AppendLine(String.Format("</script>{0}{0}", vbCrLf))
        Return myStringBuilder.ToString
    End Function
    Private Function BuildSliderPageThumbnail() As String
        Dim myStringBuilder As StringBuilder = New StringBuilder(String.Empty)
        Dim iRowCount As Integer = 0
        If Images.Count > 0 Then
            For Each Image As LocationImage In Images
                myStringBuilder.Append(String.Format("leftrightslide[{0}]='", iRowCount))
                myStringBuilder.Append(String.Format("<a title=""{0}"" href=""{1}""><img style=""align:text-top;border:0;"" alt=""{0}"" src=""{2}""  /></a> ", Image.ImageDescription, Image.ImageURL, GetImagePath(Image.ImageFileName, "100", "100")))
                myStringBuilder.Append("'" & vbCrLf)
                iRowCount = iRowCount + 1
            Next
        Else
            myStringBuilder.Append(String.Format("<P STYLE=""font-size:10pt;"">{0}No Image Results Found!<br/></p>{0}", vbCrLf))
        End If
        Return myStringBuilder.ToString
    End Function
    Private Function BuildLightboxSliderPageThumbnail() As String
        Dim myStringBuilder As StringBuilder = New StringBuilder(String.Empty)
        Dim iRowCount As Integer = 0
        If Images.Count > 0 Then
            For Each Image As LocationImage In Images
                myStringBuilder.Append(String.Format("leftrightslide[{0}]='", iRowCount))
                myStringBuilder.Append(String.Format("<a title=""{0}"" href=""{1}"" rel=""lightbox[{2}]"" ><img style=""align:text-top;border:0;"" alt=""{0}"" src=""{3}""  /></a> ", Image.ImageDescription, Image.ImageURL, (CatalogPageName.Trim), GetImagePath(Image.ImageFileName, "100", "100")))
                myStringBuilder.Append("'" & vbCrLf)
                iRowCount = iRowCount + 1
            Next
        Else
            myStringBuilder.Append(String.Format("<P STYLE=""font-size:10pt;"">{0}No Image Results Found!<br/></p>{0}", vbCrLf))
        End If
        Return myStringBuilder.ToString
    End Function
    Private Function BuildSliderPageImage() As String
        Dim myStringBuilder As StringBuilder = New StringBuilder(String.Empty)
        Dim iRowCount As Integer = 0
        If Images.Count > 0 Then
            For Each Image As LocationImage In Images
                myStringBuilder.Append(String.Format("leftrightslide[{0}]='", iRowCount))
                myStringBuilder.Append(String.Format("<a title=""{0}"" href=""{1}""><img style=""align:text-top;border:0;"" alt=""{0}"" src=""{2}""  /></a> ", Image.ImageDescription, Image.ImageURL, GetImagePath(Image.ImageFileName, sThumbnailWidth)))
                myStringBuilder.Append("'" & vbCrLf)
                iRowCount = iRowCount + 1
            Next
        Else
            myStringBuilder.Append(String.Format("<P STYLE=""font-size:10pt;"">{0}No Image Results Found!<br/></p>{0}", vbCrLf))
        End If
        Return myStringBuilder.ToString
    End Function
    Private Function BuildYUICarouselPageImage() As String
        Dim myStringBuilder As StringBuilder = New StringBuilder(String.Empty)
        If Images.Count > 0 Then
            myStringBuilder.Append(String.Format("{0}{0}<div id=""container""><ol id=""carousel"">{0}", vbCrLf))
            For Each Image As LocationImage In Images
                myStringBuilder.Append(String.Format("<li><a title=""{0}"" href=""{1}"" rel=""lightbox[{2}]"" ><img width=""150"" alt=""{0}"" src=""{3}""  /></a></li> {4}", Image.ImageDescription, GetImagePath(Image.ImageFileName, "800"), (CatalogPageName.Trim), GetImagePath(Image.ImageFileName, "150", "150"), vbCrLf))
            Next
            myStringBuilder.Append("</ol></div><script>" & vbCrLf)
            myStringBuilder.Append("(function () {" & vbCrLf)
            myStringBuilder.Append("var carousel;" & vbCrLf)
            myStringBuilder.Append("YAHOO.util.Event.onDOMReady(function (ev) {" & vbCrLf)
            myStringBuilder.Append("var carousel    = new YAHOO.widget.Carousel(""container"", {" & vbCrLf)
            myStringBuilder.Append("revealAmount: 0" & vbCrLf)
            myStringBuilder.Append("});" & vbCrLf)
            myStringBuilder.Append("carousel.set(""numVisible"", 5); " & vbCrLf)
            myStringBuilder.Append("carousel.set(""animation"", { speed: 0.5 });" & vbCrLf)
            myStringBuilder.Append("carousel.render();" & vbCrLf)
            myStringBuilder.Append("carousel.show();" & vbCrLf)
            myStringBuilder.Append("});" & vbCrLf)
            myStringBuilder.Append("})();" & vbCrLf)
            myStringBuilder.Append(String.Format("</script>{0}{0}", vbCrLf))
        Else
            myStringBuilder.Append(String.Format("<P STYLE=""font-size:10pt;"">{0}No Image Results Found!<br/></p>{0}", vbCrLf))
        End If
        Return myStringBuilder.ToString
    End Function
    Private Function BuildLightboxPageImage() As String
        Dim myStringBuilder As StringBuilder = New StringBuilder(String.Empty)
        If Images.Count > 0 Then
            For Each Image As LocationImage In Images
                myStringBuilder.Append(String.Format("<a title=""{0}"" href=""{1}"" rel=""lightbox[{2}]"" ><img style=""align:text-top;border:0;"" alt=""{0}"" src=""{3}""  /></a> {4}", Image.ImageDescription, GetImagePath(Image.ImageFileName, sImageWidth), (CatalogPageName.Trim), GetImagePath(Image.ImageFileName, sThumbnailWidth), vbCrLf))
            Next
        Else
            myStringBuilder.Append(String.Format("<P STYLE=""font-size:10pt;"">{0}No Image Results Found!<br/></p>{0}", vbCrLf))
        End If
        Return myStringBuilder.ToString
    End Function
    Private Function BuildCMotionPageImage(ByVal AddLightbox As Boolean) As String
        Dim myStringBuilder As StringBuilder = New StringBuilder(String.Empty)
        If Images.Count > 0 Then
            myStringBuilder.Append("<div id=""motioncontainer"" style=""position:relative;overflow:hidden;valign:top;"">" & vbCrLf)
            myStringBuilder.Append(String.Format("<div id=""motiongallery"" style=""position:relative;left:0px;top:0px;white-space:nowrap;valign:top;"">{0}{0}", vbCrLf))
            myStringBuilder.Append("<nobr id=""trueContainer"">" & vbCrLf)
            For Each Image As LocationImage In Images
                If AddLightbox Then
                    myStringBuilder.Append(String.Format("<a title=""{0}"" href=""{1}"" rel=""lightbox[{2}]"" ><img style=""align:text-top;border:0;"" alt=""{0}"" src=""{3}""  /></a> {4}", Image.ImageDescription, GetImagePath(Image.ImageFileName, sImageWidth), CatalogPageName, GetImagePathHeight(Image.ImageFileName, sThumbnailWidth), vbCrLf))
                Else
                    myStringBuilder.Append(String.Format("<a title=""{0}"" href=""{1}""><img style=""align:text-top;border:0;"" alt=""{0}"" src=""{2}""  /></a> {3}", Image.ImageDescription, Image.ImageURL, GetImagePathHeight(Image.ImageFileName, sThumbnailWidth), vbCrLf))
                End If
            Next
            myStringBuilder.Append("</nobr>" & vbCrLf)
            myStringBuilder.Append("</div>" & vbCrLf)
            myStringBuilder.Append("</div>" & vbCrLf)
        Else
            myStringBuilder.Append("<P STYLE=""font-size:10pt;"">" & vbCrLf)
            myStringBuilder.Append("No Image Results Found!<br/>" & vbCrLf)
            myStringBuilder.Append("</P>" & vbCrLf)
        End If
        Return myStringBuilder.ToString
    End Function
    Private Function BuildImageUL(ByVal bIncludeDiv As Boolean, ByVal IDName As String) As String
        Dim myStringBuilder As StringBuilder = New StringBuilder(String.Empty)
        If Images.Count > 0 Then
            myStringBuilder.Append(String.Format("<ul id=""{0}""> {1}", IDName, vbCrLf))
            For Each Image As LocationImage In Images
                If bIncludeDiv Then
                    If Image.Title = String.Empty Then
                        Image.Title = String.Format("The name of this image is: {0}", Image.ImageName)
                    End If
                    If Image.Title = String.Empty Then
                        myStringBuilder.Append(String.Format("<li><img src=""{1}""  />{2}<p>{0}</p></li>", Image.Title, GetImagePath(Image.ImageFileName), vbCrLf))
                    Else
                        myStringBuilder.Append(String.Format("<li><img src=""{1}""  />{2}<p>{0}</p></li>", Image.Title, GetImagePath(Image.ImageFileName), vbCrLf))
                    End If
                Else
                    myStringBuilder.Append(String.Format("<li><img src=""{1}"" alt=""{0}""  /></li>{2}", "Picture", GetImagePath(Image.ImageFileName), vbCrLf))
                End If
            Next
            myStringBuilder.Append(String.Format("</ul>{0}", vbCrLf))
        Else
            myStringBuilder.Append("<P STYLE=""font-size:10pt;"">" & vbCrLf)
            myStringBuilder.Append("No Image Results Found!<br/>" & vbCrLf)
            myStringBuilder.Append("</P>" & vbCrLf)
        End If
        Return myStringBuilder.ToString
    End Function

    Private Function BuildImageDiv(ByVal bIncludeDiv As Boolean) As String
        Dim myStringBuilder As StringBuilder = New StringBuilder(String.Empty)
        If Images.Count > 0 Then
            For Each Image As LocationImage In Images
                If bIncludeDiv Then
                    If Image.Title = String.Empty Then
                        Image.Title = String.Format("The name of this image is: {0}", Image.ImageName)
                    End If
                    If Image.Title = String.Empty Then
                        myStringBuilder.Append(String.Format("<div><img src=""{1}"" alt=""{3}"" />{2}<p>{0}</p></div>", Image.Title, GetImagePath(Image.ImageFileName), vbCrLf, Image.ImageName))
                    Else
                        myStringBuilder.Append(String.Format("<div><img src=""{1}"" =""{3}""  />{2}<p>{0}</p></div>", Image.Title, GetImagePath(Image.ImageFileName), vbCrLf, Image.ImageName))
                    End If
                Else
                    myStringBuilder.Append(String.Format("<img src=""{1}"" alt=""{0}""  />{2}", Image.ImageName, GetImagePath(Image.ImageFileName), vbCrLf))
                End If
            Next
        Else
            myStringBuilder.Append("<P STYLE=""font-size:10pt;"">" & vbCrLf)
            myStringBuilder.Append("No Image Results Found!<br/>" & vbCrLf)
            myStringBuilder.Append("</P>" & vbCrLf)
        End If
        Return myStringBuilder.ToString
    End Function

    Private Function BuildImageTags() As String
        Dim myStringBuilder As StringBuilder = New StringBuilder(String.Empty)
        If Images.Count > 0 Then
            For Each Image As LocationImage In Images
                If Image.Title = String.Empty Then
                    myStringBuilder.Append(String.Format("<img src=""{1}"" alt=""{3}""   />{2}", Image.Title, GetImagePath(Image.ImageFileName), vbCrLf, Image.ImageName))
                Else
                    myStringBuilder.Append(String.Format("<img src=""{1}"" alt=""{3}""   />{2}", Image.Title, GetImagePath(Image.ImageFileName), vbCrLf, Image.ImageName))
                End If
            Next
        Else
            myStringBuilder.Append("<P STYLE=""font-size:10pt;"">" & vbCrLf)
            myStringBuilder.Append("No Image Results Found!<br/>" & vbCrLf)
            myStringBuilder.Append("</P>" & vbCrLf)
        End If
        Return myStringBuilder.ToString
    End Function
    Private Function BuildFadeImage() As String
        Dim myStringBuilder As StringBuilder = New StringBuilder(String.Empty)
        myStringBuilder.Append("var photoalbum=new Array() " & vbCrLf)
        Dim iImageNumber As Integer = 0
        If Images.Count > 0 Then
            For Each Image As LocationImage In Images
                myStringBuilder.Append(String.Format("photoalbum[{0}]=[""{1}""  ,""{2}"",""""] {3}", iImageNumber, GetImagePath(Image.ImageFileName, sThumbnailWidth), Image.ImageURL, vbCrLf))
                iImageNumber = iImageNumber + 1
            Next
        Else
            myStringBuilder.Append("<P STYLE=""font-size:10pt;"">" & vbCrLf)
            myStringBuilder.Append("No Image Results Found!" & vbCrLf)
            myStringBuilder.Append("</P>" & vbCrLf)
        End If
        Return myStringBuilder.ToString
    End Function
    ' Private Functions
    Private Function ReplaceImageTags(ByRef myImage As LocationImage, ByRef sbCatalog As StringBuilder) As Boolean
        sbCatalog.Replace(STR_ImgName, myImage.ImageName)
        sbCatalog.Replace(STR_ImgDesc, myImage.ImageDescription)
        sbCatalog.Replace(STR_ImgTag, (String.Format("<a title=""{0}"" href=""{1}""><img src=""{2}""  alt=""{0}"" border=""0"" class=""img-responsive"" /></a>", myImage.ImageDescription, wpm_FixInvalidCharacters(ActiveSite.CurrentLocationNM), GetImagePath(myImage.ImageFileName, sImageWidth))))
        sbCatalog.Replace(STR_ImgUrl, GetImagePath(myImage.ImageFileName, sImageWidth))
        sbCatalog.Replace(STR_ImgFileName, GetImagePath(myImage.ImageFileName, sImageWidth))
        sbCatalog.Replace(STR_ImgComment, myImage.ImageComment)
        sbCatalog.Replace(STR_ImgAdmin, BuildImageAdmin(myImage.ImageID, ActiveSite.CurrentLocationID))
        sbCatalog.Replace(STR_ImgTitle, myImage.Title)
        sbCatalog.Replace(STR_ContactID, myImage.ContactID)
        sbCatalog.Replace(STR_HomeLink, (String.Format("<a href=""{0}"">{1}</a>", CatalogBasePageURL, CatalogPageName)))
        sbCatalog.Append(BuildImageAdmin(myImage.ImageID, ActiveSite.CurrentLocationID))
        Return True
    End Function
    Private Function ShowImageDetail(ByVal sImageID As Integer, ByRef sbCatalog As StringBuilder) As String
        Dim pageNumber As Integer = 0
        Dim intImagesPerPage As Integer = 0
        Dim indexImageList As Integer = 0
        Using dtPageImage As New DataTable
            For Each myImage As LocationImage In Images
                intImagesPerPage = iRowsPerPage * iImagesPerRow
                pageNumber = GetPageNumber(indexImageList, intImagesPerPage)
                If Val(myImage.ImageID) = Val(sImageID) Then
                    If indexImageList > 0 Then
                        sbCatalog.Replace("<PrevImg>", String.Format("<a href=""{0}"">« &nbsp;{1}</a>", Images.Item(indexImageList - 1).ImageURL, Images.Item(indexImageList - 1).ImageName))
                        sbCatalog.Replace("<PrevImgURL>", Images.Item(indexImageList - 1).ImageURL)
                    Else
                        sbCatalog.Replace("<PrevImg>", String.Format("<a href=""{0}"">« &nbsp;{1}</a>", Images.Item(Images.Count - 1).ImageURL, Images.Item(Images.Count - 1).ImageName))
                        sbCatalog.Replace("<PrevImgURL>", Images.Item(Images.Count - 1).ImageURL)
                    End If

                    If indexImageList + 1 <= Images.Count - 1 Then
                        sbCatalog.Replace("<NextImg>", String.Format("<a href=""{0}"">{1} &nbsp;»</a>", Images.Item(indexImageList + 1).ImageURL, Images.Item(indexImageList + 1).ImageName))
                        sbCatalog.Replace("<NextImgURL>", Images.Item(indexImageList + 1).ImageURL)
                    Else
                        sbCatalog.Replace("<NextImg>", String.Format("<a href=""{0}"">{1} &nbsp;»</a>", Images.Item(0).ImageURL, Images.Item(0).ImageName))
                        sbCatalog.Replace("<NextImgURL>", Images.Item(0).ImageURL)
                    End If
                    ReplaceImageTags(myImage, sbCatalog)
                End If
                indexImageList = indexImageList + 1
            Next
            If indexImageList = 0 Then
                sbCatalog.Append("<P STYLE=""font-size:10pt;""><center>No Results Found!<br /><a href=""/default.aspx"">Home</a></center></P>")
            End If
        End Using
        Return sbCatalog.ToString
    End Function
    Public Function GetImagePath(ByVal ImagePath As String) As String
        Dim myReturn As String = wpm_SiteGallery & ImagePath
        myReturn = Replace(myReturn, "\", "/")
        Return Replace(myReturn, "//", "/")
    End Function
    Public Function GetImagePath(ByVal ImagePath As String, ByVal ImageWidth As String, ByVal ImageHeight As String) As String
        Dim myReturn As String = wpm_SiteGallery & ImagePath
        myReturn = Replace(myReturn, "\", "/")
        Return String.Format("/runtime/catalog/FindImage.ashx?w={0}&h={1}&img={2}", ImageWidth, ImageHeight, Replace(myReturn, "//", "/"))
    End Function
    Public Function GetImagePath(ByVal ImagePath As String, ByVal ImageWidth As String) As String
        Dim myReturn As String = wpm_SiteGallery & ImagePath
        myReturn = Replace(myReturn, "\", "/")
        Return String.Format("/runtime/catalog/FindImage.ashx?w={0}&img={1}", ImageWidth, Replace(myReturn, "//", "/"))
    End Function
    Private Function GetImagePathHeight(ByVal ImagePath As String, ByVal ImageHeight As String) As String
        Dim myReturn As String = wpm_SiteGallery & ImagePath
        myReturn = Replace(myReturn, "\", "/")
        Return String.Format("/runtime/catalog/FindImage.ashx?h={0}&img={1}", ImageHeight, Replace(myReturn, "//", "/"))
    End Function
    Private Shared Function GetPageNumber(ByVal iItemNumber As Integer, ByVal iItemsPerPage As Integer) As Integer
        Dim iPageNumber As Integer
        If (iItemsPerPage > 0) Then
            If (iItemsPerPage >= iItemNumber) Then
                iPageNumber = 1
            Else
                If (iItemNumber Mod iItemsPerPage) = 0 Then
                    iPageNumber = CInt(iItemNumber / iItemsPerPage)
                Else
                    If (iItemNumber Mod iItemsPerPage) > 0.5 Then
                        iPageNumber = CInt(Math.Round(iItemNumber / iItemsPerPage, 4)) + 1
                    Else
                        iPageNumber = CInt(Math.Round(iItemNumber / iItemsPerPage, 4))
                    End If
                End If
            End If
        Else
            iPageNumber = 1
        End If
        Return iPageNumber
    End Function
    Private Shared Function BuildImageAdmin(ByVal ImageID As String, ByVal PageID As String) As String
        If wpm_IsEditor() Then
            Return String.Format("<br/><a href=""/admin/maint/default.aspx?type=Image&ImageID={0}"" class=""btn btn-primary"">Edit This Image</a><br/>", ImageID)
        Else
            Return String.Empty
        End If
    End Function
End Class


'Public Function FixFolders(ByVal sPageID As String) As Boolean
'    Dim sNewPagePath As String = ("")
'    Dim sNewImagePath As String = ("")
'    '   Dim sNewThumbnailPath As String = ("")
'    Using dtPageImage As New DataTable
'        For Each myImage As LocationImage In Images
'            If ActiveSite.CurrentLocationID = sPageID Then
'                sNewPagePath = String.Format("{0}image/{1}", wpm_SiteGallery, wpm_FormatPageNameForURL(ActiveSite.GetCurrentPageName))
'                ' Create a Folder for Page 
'                sNewPagePath = HttpContext.Current.Server.MapPath(sNewPagePath)
'                FileProcessing.CreateFolder(sNewPagePath)
'                ' Create a Folder for Image/Page and move this image file to the folder
'                sNewImagePath = sNewPagePath
'                FileProcessing.CreateFolder(sNewImagePath)
'                FileProcessing.MoveFile(HttpContext.Current.Server.MapPath(wpm_SiteGallery & _
'                               myImage.ImageFileName), String.Format("{0}/{1}", sNewPagePath, myImage.ImageFileName))
'            End If
'        Next
'    End Using
'    Return True
'End Function
'Public Shared Function GetImageLocation(ByRef myImage As LocationImage, ByRef ActiveSite As ActiveCompany) As String
'    Return wpm_FixInvalidCharacters(String.Format("{0}-{1}{2}", ActiveSite.GetCurrentPageName, myImage.ImageName, wpm_SiteConfig.DefaultExtension))
'End Function
'Public Shared Function GetImagePathHeight(ByVal SiteGallery As String, ByVal ImagePath As String, ByVal ImageHeight As String) As String
'    Dim myReturn As String = SiteGallery & ImagePath
'    myReturn = Replace(myReturn, "\", "/")
'    Return String.Format("/runtime/catalog/FindImage.ashx?h={0}&img={1}", ImageHeight, Replace(myReturn, "//", "/"), wpm_SiteConfig.ApplicationHome)
'End Function