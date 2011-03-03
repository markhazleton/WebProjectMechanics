Imports System.Web
Imports System.Text

Public Class wpmImageList
    Inherits List(Of wpmImage)
End Class

Public Class wpmSiteImageList
    Inherits wpmImageList
    Private _SearchImageFileName As String
    Private _SearchImageID As String
    Public Sub New(ByVal CompanyID As String)
        Dim mydt As DataTable = wpmDataCon.GetImageList(CompanyID)
        For Each myrow As DataRow In mydt.Rows
            Dim myImage As New wpmImage()
            myImage.ImageID = wpmUtil.GetDBString(myrow.Item("ImageID"))
            myImage.CompanyID = wpmUtil.GetDBString(myrow.Item("CompanyID"))
            myImage.VersionNumber = wpmUtil.GetDBInteger(myrow.Item("VersionNo"))
            myImage.Title = wpmUtil.GetDBString(myrow.Item("Title"))
            myImage.ImageName = wpmUtil.GetDBString(myrow.Item("ImageName"))
            myImage.ContactID = wpmUtil.GetDBString(myrow.Item("ContactID"))
            myImage.ImageDescription = wpmUtil.GetDBString(myrow.Item("ImageDescription"))
            myImage.ImageComment = wpmUtil.GetDBString(myrow.Item("ImageComment"))
            myImage.ImageFileName = wpmUtil.GetDBString(myrow.Item("ImageFileName"))
            myImage.ImageThumbFileName = wpmUtil.GetDBString(myrow.Item("ImageFileName"))
            myImage.Size = wpmUtil.GetDBString(myrow.Item("Size"))
            myImage.Price = wpmUtil.GetDBString(myrow.Item("Price"))
            myImage.Color = wpmUtil.GetDBString(myrow.Item("Color"))
            myImage.Subject = wpmUtil.GetDBString(myrow.Item("Subject"))
            myImage.ImageDate = wpmUtil.GetDBDate(myrow.Item("ImageDate"))
            myImage.Medium = wpmUtil.GetDBString(myrow.Item("Medium"))
            myImage.Sold = wpmUtil.GetDBBoolean(myrow.Item("Sold"))
            Me.Add(myImage)
        Next
    End Sub
    Public Function FindImageByImageID(ByVal ImageID As String) As wpmImage
        _SearchImageID = ImageID
        Return Me.Find(AddressOf FindImageByID)
    End Function
    Private Function FindImageByID(ByVal Image As wpmImage) As Boolean
        If Image.ImageID = _SearchImageID Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function FindImageByImageFileName(ByVal ImageFileName As String) As wpmImage
        _SearchImageFileName = ImageFileName
        Return Me.Find(AddressOf FindImageByImageFileName)
    End Function
    Private Function FindImageByImageFileName(ByVal Image As wpmImage) As Boolean
        If Image.ImageFileName.Replace("\", "/").ToLower = _SearchImageFileName.Replace("\", "/").ToLower Then
            Return True
        Else
            Return False
        End If
    End Function
End Class

Public Class wpmPageImageList
    Inherits wpmImageList
    Private _ActiveSite As wpmActiveSite
    Public ReadOnly Property ActiveSite() As wpmActiveSite
        Get
            Return _ActiveSite
        End Get
    End Property
    Public sImageWidth As String = "800"
    Public sThumbnailWidth As String = "150"
    Public iRowsPerPage As Integer = 3
    Public iImagesPerRow As Integer = 3
    Public ImageBaseFolder As String
    Public CatalogBasePageURL As String
    Public CatalogPageName As String
    Public PageFileName As String
    Private _CatalogPageID As String
    Public ReadOnly Property CatalogPageID() As String
        Get
            Return _CatalogPageID
        End Get
    End Property

    Public Sub New(ByRef ActiveSite As wpmActiveSite)
        _ActiveSite = ActiveSite
        _CatalogPageID = ActiveSite.CurrentPageID

        Dim mydt As DataTable = wpmDataCon.GetPageImage(ActiveSite.CurrentPageID, ActiveSite.CompanyID, ActiveSite.GroupID)
        For Each myrow As DataRow In mydt.Rows
            Dim myImage As New wpmImage()
            Me.iImagesPerRow = wpmUtil.GetDBInteger(myrow.Item("ImagesPerRow"))
            Me.iRowsPerPage = wpmUtil.GetDBInteger(myrow.Item("RowsPerPage"))
            Me.ImageBaseFolder = ActiveSite.SiteGallery & "pages/" & wpmUtil.FixInvalidCharacters(myrow.Item("PageName").ToString) & "/"
            Me.CatalogBasePageURL = wpmUtil.FixInvalidCharacters(myrow.Item("PageName").ToString & wpmApp.Config.DefaultExtension)
            Me.CatalogPageName = wpmUtil.GetDBString(myrow.Item("PageName"))
            Me.PageFileName = wpmUtil.GetDBString(myrow.Item("PageFileName"))
            myImage.ImageURL = wpmUtil.FixInvalidCharacters(myrow.Item("PageName").ToString & "-" & myrow.Item("ImageName").ToString & wpmApp.Config.DefaultExtension)
            myImage.ImageID = wpmUtil.GetDBString(myrow.Item("ImageID"))
            myImage.VersionNumber = wpmUtil.GetDBInteger(myrow.Item("VersionNo"))
            myImage.Title = wpmUtil.GetDBString(myrow.Item("Title"))
            myImage.ImageName = wpmUtil.GetDBString(myrow.Item("ImageName"))
            myImage.ContactID = wpmUtil.GetDBString(myrow.Item("ContactID"))
            myImage.ImageDescription = wpmUtil.GetDBString(myrow.Item("ImageDescription"))
            myImage.ImageComment = wpmUtil.GetDBString(myrow.Item("ImageComment"))
            myImage.ImageFileName = wpmUtil.GetDBString(myrow.Item("ImageFileName"))
            myImage.ImageThumbFileName = wpmUtil.GetDBString(myrow.Item("ImageThumbFileName"))
            myImage.Size = wpmUtil.GetDBString(myrow.Item("Size"))
            myImage.Price = wpmUtil.GetDBString(myrow.Item("Price"))
            myImage.Color = wpmUtil.GetDBString(myrow.Item("Color"))
            myImage.Subject = wpmUtil.GetDBString(myrow.Item("Subject"))
            myImage.ImageDate = wpmUtil.GetDBDate(myrow.Item("ImageDate"))
            myImage.Medium = wpmUtil.GetDBString(myrow.Item("Medium"))
            myImage.Sold = wpmUtil.GetDBBoolean(myrow.Item("Sold"))
            Me.Add(myImage)
        Next
    End Sub

    Public Function ProcessPageRequest(ByVal PageNumber As String, ByVal ImageID As String) As String
        Dim sRightContent As String = ("")
        Dim myStringBuilder As StringBuilder = New StringBuilder("")
        If (ImageID = "") Then
            ' code for thumnail page
            myStringBuilder.Append(ProcesscatalogPage(sRightContent, CatalogPageID, ActiveSite.CurrentArticleID, ActiveSite.SiteGallery))
            If myStringBuilder.ToString.IndexOf("~~pan-lightbox~~") > 0 Then
                Dim myContents As New StringBuilder
                wpmFileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath("/wpm/catalog/slider/pan.js"), myContents)
                myContents.Replace("<PageImageArray>", BuildLightboxSliderPageThumbnail())
                myStringBuilder.Replace("~~pan-lightbox~~", myContents.ToString)
            End If
            If myStringBuilder.ToString.IndexOf("~~slider~~") > 0 Then
                Dim myContents As New StringBuilder
                wpmFileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath("/wpm/catalog/slider/slider.js"), myContents)
                myContents.Replace("<PageImageArray>", BuildSliderPageImage())
                myStringBuilder.Replace("~~slider~~", myContents.ToString)
            End If
            If myStringBuilder.ToString.IndexOf("~~pan~~") > 0 Then
                Dim myContents As New StringBuilder
                wpmFileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath("/wpm/catalog/slider/pan.js"), myContents)
                myContents.Replace("<PageImageArray>", BuildSliderPageThumbnail())
                myStringBuilder.Replace("~~pan~~", myContents.ToString)
            End If
            If myStringBuilder.ToString.IndexOf("~~lightbox~~") > 0 Then
                myStringBuilder.Replace("~~lightbox~~", BuildLightboxPageImage())
                Dim myContents As New StringBuilder
                wpmFileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath("/wpm/catalog/lightbox/ADDToHTMLHead.txt"), myContents)
                ActiveSite.AddHTMLHead = myContents.ToString
            End If
            If myStringBuilder.ToString.IndexOf("~~thumbnail~~") > 0 Then
                myStringBuilder.Replace("~~thumbnail~~", BuildCMotionPageImage(False))
                Dim myContents As New StringBuilder
                wpmFileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath("/wpm/catalog/cmotion/AddToHTMLHead.txt"), myContents)
                ActiveSite.AddHTMLHead = myContents.ToString
            End If
            If myStringBuilder.ToString.IndexOf("~~cmotion~~") > 0 Then
                myStringBuilder.Replace("~~cmotion~~", BuildCMotionPageImage(False))
                Dim myContents As New StringBuilder
                wpmFileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath("/wpm/catalog/cmotion/AddToHTMLHead.txt"), myContents)
                ActiveSite.AddHTMLHead = myContents.ToString
            End If
            If myStringBuilder.ToString.IndexOf("~~cmotionlb~~") > 0 Then
                myStringBuilder.Replace("~~cmotionlb~~", BuildCMotionPageImage(True))
                Dim myContents As New StringBuilder
                wpmFileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath("/wpm/catalog/cmotion/AddToHTMLHeadLB.txt"), myContents)
                ActiveSite.AddHTMLHead = myContents.ToString
            End If
            If myStringBuilder.ToString.IndexOf("photoalbum") > 0 Then
                myStringBuilder.Replace("~~photoalbum~~", BuildPhotoAlbumPage())
                Dim myContents As New StringBuilder
                wpmFileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath("/wpm/catalog/photoalbum/ADDToHTMLHead.txt"), myContents)
                ActiveSite.AddHTMLHead = myContents.ToString
            End If
            If myStringBuilder.ToString.IndexOf("~~drill-in~~") > 0 Then
                Dim myContents As New StringBuilder
                wpmFileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath("/wpm/catalog/slider/drill-in.js"), myContents)
                myContents.Replace("<PageImageArray>", BuildImageTags())
                myStringBuilder.Replace("~~drill-in~~", myContents.ToString)
            End If
            If myStringBuilder.ToString.IndexOf("~~fade~~") > 0 Then
                Dim myContents As New StringBuilder
                wpmFileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath("/wpm/catalog/slider/fadeimages.html"), myContents)
                myContents.Replace("<PageImageArray>", BuildFadeImage())
                myStringBuilder.Replace("~~fade~~", myContents.ToString)
            End If
            If myStringBuilder.ToString.IndexOf("~~yui-carousel~~") > 0 Then
                myStringBuilder.Replace("~~yui-carousel~~", BuildYUICarouselPageImage())
                Dim myContents As New StringBuilder
                wpmFileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath("/wpm/catalog/yui-carousel/ADDToHTMLHead.txt"), myContents)
                ActiveSite.AddHTMLHead = myContents.ToString

            End If
            If myStringBuilder.Length < 1 Then
                myStringBuilder.Append(BuildPhotoAlbumPage())
                Dim myContents As New StringBuilder
                wpmFileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath("/wpm/catalog/photoalbum/ADDToHTMLHead.txt"), myContents)
                ActiveSite.AddHTMLHead = myContents.ToString
            End If
            If wpmUser.IsAdmin() Then
                myStringBuilder.Append("<br /><strong><a href=""/wpm/admin/BrowseImages.aspx?SubFolder=" & Me.CatalogPageName & """>Browse Images</a></strong><br />")
            End If
        Else
            ' Code for Detail Image
            If Me.PageFileName = String.Empty Then
                wpmFileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath(ActiveSite.SiteGallery & "/catalog_template.html"), myStringBuilder)
            Else
                If Not wpmFileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath(ActiveSite.SiteGallery & "/" & Me.PageFileName & "_template.html"), myStringBuilder) Then
                    wpmFileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath(ActiveSite.SiteGallery & "/catalog_template.html"), myStringBuilder)
                End If
            End If
            ShowImageDetail(ImageID, myStringBuilder)
        End If
        Return myStringBuilder.ToString
    End Function
    Private Function ProcesscatalogPage(ByRef sImageDesc As String, ByVal CurrentPageID As String, ByVal DefaultArticleID As String, ByVal SiteGallery As String) As String
        Dim strArticleText As String = ""
        Dim myArticle As New wpmArticle("", CurrentPageID, DefaultArticleID)
        Return GetImageTextMerge(CurrentPageID, myArticle.ArticleBody, True, sImageDesc, CurrentPageID, SiteGallery)
    End Function
    ' -----------------------------------------------------------------------------
    ' This Function is to merge and article and PageImageList
    ' -----------------------------------------------------------------------------
    Private Function GetImageTextMerge(ByVal ReqPageID As String, ByVal strText As String, ByVal bAddLink As Boolean, ByRef sImageDesc As String, ByVal CurrentPageID As String, ByVal SiteGallery As String) As String
        Dim iImageCount As Integer = 0
        Dim stemp As String
        Using dtPageImage As New DataTable
            sImageDesc = "<div id=""imagedesc"">Photos left:<ul>"
            For Each myImage As wpmImage In Me
                If (bAddLink) Then
                    ' Replace a number in brackets with the thumbnail and link to the larger image
                    stemp = "<div id=""imagedesc""><a href=""" & _
                       wpmUtil.FixInvalidCharacters(Me.ActiveSite.GetCurrentPageName & _
                       "-" & myImage.ImageName) & _
                       """><img align=""top"" alt='" & myImage.ImageDescription & _
                       "' src=""" & GetImagePath(myImage.ImageFileName, sThumbnailWidth) & _
                       """ /></a></div>"
                    strText = strText.Replace("<" & iImageCount & ">", stemp)
                    strText = strText.Replace("~" & iImageCount & "~", stemp)
                Else
                    ' Replace a number in brackets with the thumbnail and link to the larger image
                    stemp = "<img align=""top"" src=""" & SiteGallery & GetImagePath(myImage.ImageFileName, sThumbnailWidth) & """ />"
                    strText = strText.Replace("<" & iImageCount & ">", stemp)
                    strText = strText.Replace("~" & iImageCount & "~", stemp)
                End If
                ' Replace custom page description tag with Page Description
                strText = strText.Replace("<$PageDescription$>", Me.ActiveSite.CurrentPageDescription)
                strText = strText.Replace("~$PageDescription$~", Me.ActiveSite.CurrentPageDescription)
                ' Replace custom image description tag with Image Description
                strText = strText.Replace("<$imageDescription" & iImageCount & "$>", stemp & "<br />" & myImage.ImageDescription)
                strText = strText.Replace("~$imageDescription" & iImageCount & "$~", stemp & "<br />" & myImage.ImageDescription)
                sImageDesc = sImageDesc & "<li>" & myImage.ImageDescription & "</li>"
                iImageCount = iImageCount + 1
            Next
            sImageDesc = sImageDesc & "</ul></div>"
        End Using
        Return strText
    End Function
    Private Function BuildPhotoAlbumPage() As String
        Dim myStringBuilder As StringBuilder = New StringBuilder("")
        myStringBuilder.Append("<sc" & "ript type=""text/javascript"">" & vbCrLf)
        myStringBuilder.Append("var photoalbum=new Array() " & vbCrLf)
        Dim iImageNumber As Integer = 0
        If Me.Count > 0 Then
            For Each Image As wpmImage In Me
                myStringBuilder.Append("photoalbum[" & iImageNumber & "]=[""" & GetImagePath(Image.ImageFileName, sThumbnailWidth) & """  " & _
                      ",""" & Image.ImageDescription & """,""" & _
                      Image.ImageURL & """,""_self""] " & vbCrLf)
                iImageNumber = iImageNumber + 1
            Next
        Else
            myStringBuilder.Append("<P STYLE=""font-size:10pt;"">" & vbCrLf & "No Image Results Found!<br/></p>" & vbCrLf)
        End If
        myStringBuilder.Append(vbCrLf & vbCrLf & "var thepics=new photogallery(photoalbum, " & Me.iImagesPerRow & ", " & Me.iRowsPerPage & ", '600px', '600px',['Browse " & Me.CatalogPageName & " Photo Gallery:','Page '])" & vbCrLf)
        myStringBuilder.Append("</sc" & "ript>" & vbCrLf & vbCrLf)
        Return myStringBuilder.ToString
    End Function
    Private Function BuildSliderPageThumbnail() As String
        Dim myStringBuilder As StringBuilder = New StringBuilder("")
        Dim iRowCount As Integer = 0
        If Me.Count > 0 Then
            For Each Image As wpmImage In Me
                myStringBuilder.Append("leftrightslide[" & iRowCount.ToString & "]='")
                myStringBuilder.Append("<a title=""" & Image.ImageDescription & _
                                       """ href=""" & Image.ImageURL & _
                                       """><img style=""align:text-top;border:0;"" alt=""" & Image.ImageDescription & _
                                       """ src=""" & GetImagePath(Image.ImageFileName, "100", "100") & """  /></a> ")
                myStringBuilder.Append("'" & vbCrLf)
                iRowCount = iRowCount + 1
            Next
        Else
            myStringBuilder.Append("<P STYLE=""font-size:10pt;"">" & vbCrLf & "No Image Results Found!<br/></p>" & vbCrLf)
        End If
        Return myStringBuilder.ToString
    End Function
    Private Function BuildLightboxSliderPageThumbnail() As String
        Dim myStringBuilder As StringBuilder = New StringBuilder("")
        Dim iRowCount As Integer = 0
        If Me.Count > 0 Then
            For Each Image As wpmImage In Me
                myStringBuilder.Append("leftrightslide[" & iRowCount.ToString & "]='")
                myStringBuilder.Append("<a title=""" & Image.ImageDescription & _
                                       """ href=""" & Image.ImageURL & _
                                       """ rel=""lightbox[" & (Me.CatalogPageName.Trim) & "]"" >" & _
                                       "<img style=""align:text-top;border:0;"" alt=""" & Image.ImageDescription & _
                                       """ src=""" & GetImagePath(Image.ImageFileName, "100", "100") & """  /></a> ")
                myStringBuilder.Append("'" & vbCrLf)
                iRowCount = iRowCount + 1
            Next
        Else
            myStringBuilder.Append("<P STYLE=""font-size:10pt;"">" & vbCrLf & "No Image Results Found!<br/></p>" & vbCrLf)
        End If
        Return myStringBuilder.ToString
    End Function
    Private Function BuildSliderPageImage() As String
        Dim myStringBuilder As StringBuilder = New StringBuilder("")
        Dim iRowCount As Integer = 0
        If Me.Count > 0 Then
            For Each Image As wpmImage In Me
                myStringBuilder.Append("leftrightslide[" & iRowCount.ToString & "]='")
                myStringBuilder.Append("<a title=""" & Image.ImageDescription & _
                                       """ href=""" & Image.ImageURL & _
                                       """><img style=""align:text-top;border:0;"" alt=""" & Image.ImageDescription & _
                                       """ src=""" & GetImagePath(Image.ImageFileName, Me.sThumbnailWidth) & """  /></a> ")
                myStringBuilder.Append("'" & vbCrLf)
                iRowCount = iRowCount + 1
            Next
        Else
            myStringBuilder.Append("<P STYLE=""font-size:10pt;"">" & vbCrLf & "No Image Results Found!<br/></p>" & vbCrLf)
        End If
        Return myStringBuilder.ToString
    End Function
    Private Function BuildYUICarouselPageImage() As String
        Dim myStringBuilder As StringBuilder = New StringBuilder("")
        If Me.Count > 0 Then
            myStringBuilder.Append(vbCrLf & vbCrLf & "<div id=""container""><ol id=""carousel"">" & vbCrLf)
            For Each Image As wpmImage In Me
                myStringBuilder.Append("<li><a title=""" & Image.ImageDescription & """ " & _
                                           "href=""" & GetImagePath(Image.ImageFileName, "800") & """ " & _
                                           "rel=""lightbox[" & (Me.CatalogPageName.Trim) & "]"" >" & _
                                           "<img width=""150"" alt=""" & Image.ImageDescription & _
                                           """ src=""" & GetImagePath(Image.ImageFileName, "150", "150") & """  /></a></li> " & vbCrLf)
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
            myStringBuilder.Append("</script>" & vbCrLf & vbCrLf)



        Else
            myStringBuilder.Append("<P STYLE=""font-size:10pt;"">" & vbCrLf & "No Image Results Found!<br/></p>" & vbCrLf)
        End If
        Return myStringBuilder.ToString
    End Function
    Private Function BuildLightboxPageImage() As String
        Dim myStringBuilder As StringBuilder = New StringBuilder("")
        If Me.Count > 0 Then
            For Each Image As wpmImage In Me
                myStringBuilder.Append("<a title=""" & Image.ImageDescription & """ " & _
                                           "href=""" & GetImagePath(Image.ImageFileName, sImageWidth) & """ " & _
                                           "rel=""lightbox[" & (Me.CatalogPageName.Trim) & "]"" >" & _
                                           "<img style=""align:text-top;border:0;"" alt=""" & Image.ImageDescription & _
                                           """ src=""" & GetImagePath(Image.ImageFileName, Me.sThumbnailWidth) & """  /></a> " & vbCrLf)
            Next
        Else
            myStringBuilder.Append("<P STYLE=""font-size:10pt;"">" & vbCrLf & "No Image Results Found!<br/></p>" & vbCrLf)
        End If
        Return myStringBuilder.ToString
    End Function
    Private Function BuildCMotionPageImage(ByVal AddLightbox As Boolean) As String
        Dim myStringBuilder As StringBuilder = New StringBuilder("")
        If Me.Count > 0 Then
            myStringBuilder.Append("<div id=""motioncontainer"" style=""position:relative;overflow:hidden;valign:top;"">" & vbCrLf)
            myStringBuilder.Append("<div id=""motiongallery"" style=""position:relative;left:0px;top:0px;white-space:nowrap;valign:top;"">" & vbCrLf & vbCrLf)
            myStringBuilder.Append("<nobr id=""trueContainer"">" & vbCrLf)
            For Each Image As wpmImage In Me
                If AddLightbox Then
                    myStringBuilder.Append("<a title=""" & Image.ImageDescription & _
                                               """ href=""" & GetImagePath(Image.ImageFileName, sImageWidth) & _
                                           """ rel=""lightbox[" & Me.CatalogPageName & "]"" >" & _
                                               "<img style=""align:text-top;border:0;"" alt=""" & Image.ImageDescription & _
                                               """ src=""" & GetImagePathHeight(Image.ImageFileName, Me.sThumbnailWidth) & """  /></a> " & vbCrLf)
                Else
                    myStringBuilder.Append("<a title=""" & Image.ImageDescription & _
                                               """ href=""" & Image.ImageURL & _
                                               """><img style=""align:text-top;border:0;"" alt=""" & Image.ImageDescription & _
                                               """ src=""" & GetImagePathHeight(Image.ImageFileName, Me.sThumbnailWidth) & """  /></a> " & vbCrLf)
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
    Private Function BuildImageTags() As String
        Dim myStringBuilder As StringBuilder = New StringBuilder("")
        If Me.Count > 0 Then
            For Each Image As wpmImage In Me
                myStringBuilder.Append("<img title=""" & Image.Title & _
                                           """ src=""" & GetImagePath(Image.ImageFileName) & """  />")
                myStringBuilder.Append("'" & vbCrLf)
            Next
        Else
            myStringBuilder.Append("<P STYLE=""font-size:10pt;"">" & vbCrLf)
            myStringBuilder.Append("No Image Results Found!<br/>" & vbCrLf)
            myStringBuilder.Append("</P>" & vbCrLf)
        End If
        Return myStringBuilder.ToString
    End Function
    Private Function BuildFadeImage() As String
        Dim myStringBuilder As StringBuilder = New StringBuilder("")
        myStringBuilder.Append("var photoalbum=new Array() " & vbCrLf)
        Dim iImageNumber As Integer = 0
        If Me.Count > 0 Then
            For Each Image As wpmImage In Me
                myStringBuilder.Append("photoalbum[" & iImageNumber & "]=[""" & GetImagePath(Image.ImageFileName, sThumbnailWidth) & """  " & _
                      ",""" & Image.ImageURL & """,""" & _
                       """] " & vbCrLf)
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
    Private Function ReplaceImageTags(ByRef myImage As wpmImage, ByRef sbCatalog As StringBuilder) As Boolean
        sbCatalog.Replace("<ImgName>", myImage.ImageName)
        sbCatalog.Replace("<ImgDesc>", myImage.ImageDescription)
        sbCatalog.Replace("<ImgTag>", ("<a title=""" & myImage.ImageDescription & _
                                        """ href=""" & wpmUtil.FixInvalidCharacters(Me.ActiveSite.GetCurrentPageName) & """>" & _
                                        "<img src=""" & GetImagePath(myImage.ImageFileName, sImageWidth) & """  " & _
                                             "alt=""" & myImage.ImageDescription & """ border=""0"" /></a>"))
        sbCatalog.Replace("<ImgFileName>", GetImagePath(myImage.ImageFileName, sImageWidth))
        sbCatalog.Replace("<ImgComment>", myImage.ImageComment)
        sbCatalog.Replace("<ImgAdmin>", BuildImageAdmin(myImage.ImageID, Me.ActiveSite.CurrentPageID))
        sbCatalog.Replace("<ImgTitle>", myImage.Title)
        sbCatalog.Replace("<ImgMedium>", myImage.Medium)
        sbCatalog.Replace("<ImgSize>", myImage.Size)
        sbCatalog.Replace("<ContactID>", myImage.ContactID)
        sbCatalog.Replace("<HomeLink>", ("<a href=""" & CatalogBasePageURL & """>" & Me.CatalogPageName & "</a>"))
        sbCatalog.Append(BuildImageAdmin(myImage.ImageID, Me.ActiveSite.CurrentPageID))
        Return True
    End Function
    Private Function ShowImageDetail(ByVal sImageID As String, ByRef sbCatalog As StringBuilder) As String
        Dim pageNumber As Integer = 0
        Dim intImagesPerPage As Integer = 0
        Dim indexImageList As Integer = 0
        Using dtPageImage As New DataTable
            For Each myImage As wpmImage In Me
                intImagesPerPage = Me.iRowsPerPage * Me.iImagesPerRow
                pageNumber = GetPageNumber(indexImageList, intImagesPerPage)
                If Val(myImage.ImageID) = Val(sImageID) Then
                    If indexImageList > 0 Then
                        sbCatalog.Replace("<PrevImg>", "<a href=""" & Me.Item(indexImageList - 1).ImageURL & """>« &nbsp;" & Me.Item(indexImageList - 1).ImageName & "</a>")
                        sbCatalog.Replace("<PrevImgURL>", Me.Item(indexImageList - 1).ImageURL)
                    Else
                        sbCatalog.Replace("<PrevImg>", "<a href=""" & Me.Item(Me.Count - 1).ImageURL & """>« &nbsp;" & Me.Item(Me.Count - 1).ImageName & "</a>")
                        sbCatalog.Replace("<PrevImgURL>", Me.Item(Me.Count - 1).ImageURL)
                    End If

                    If indexImageList + 1 <= Me.Count - 1 Then
                        sbCatalog.Replace("<NextImg>", "<a href=""" & Me.Item(indexImageList + 1).ImageURL & """>" & Me.Item(indexImageList + 1).ImageName & " &nbsp;»</a>")
                        sbCatalog.Replace("<NextImgURL>", Me.Item(indexImageList + 1).ImageURL)
                    Else
                        sbCatalog.Replace("<NextImg>", "<a href=""" & Me.Item(0).ImageURL & """>" & Me.Item(0).ImageName & " &nbsp;»</a>")
                        sbCatalog.Replace("<NextImgURL>", Me.Item(0).ImageURL)
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
        Dim myReturn As String
        myReturn = ActiveSite.SiteGallery & ImagePath
        myReturn = Replace(myReturn, "\", "/")
        Return Replace(myReturn, "//", "/")
    End Function
    Public Function GetImagePath(ByVal ImagePath As String, ByVal ImageWidth As String, ByVal ImageHeight As String) As String
        Dim myReturn As String
        myReturn = ActiveSite.SiteGallery & ImagePath
        myReturn = Replace(myReturn, "\", "/")
        Return "/wpm/catalog/ImageResize.aspx?w=" & ImageWidth & "&h=" & ImageHeight & "&img=" & Replace(myReturn, "//", "/")
    End Function
    Public Function GetImagePath(ByVal ImagePath As String, ByVal ImageWidth As String) As String
        Dim myReturn As String
        myReturn = ActiveSite.SiteGallery & ImagePath
        myReturn = Replace(myReturn, "\", "/")
        Return "/wpm/catalog/ImageResize.aspx?w=" & ImageWidth & "&img=" & Replace(myReturn, "//", "/")
    End Function
    Private Function GetImagePathHeight(ByVal ImagePath As String, ByVal ImageHeight As String) As String
        Dim myReturn As String
        myReturn = ActiveSite.SiteGallery & ImagePath
        myReturn = Replace(myReturn, "\", "/")
        Return "/wpm/catalog/ImageResize.aspx?h=" & ImageHeight & "&img=" & Replace(myReturn, "//", "/")
    End Function
    Private Function GetImageLocation(ByRef myImage As wpmImage) As String
        Return wpmUtil.FixInvalidCharacters(Me.ActiveSite.GetCurrentPageName & "-" & myImage.ImageName & wpmApp.Config.DefaultExtension)
        'Return GetImagePath(myImage.ImageFileName, sImageWidth)
    End Function
    Private Function GetImagePathHeight(ByVal SiteGallery As String, ByVal ImagePath As String, ByVal PageName As String, ByVal ImageHeight As String) As String
        Dim myReturn As String
        myReturn = SiteGallery & ImagePath
        myReturn = Replace(myReturn, "\", "/")
        Return "/wpm/catalog/ImageResize.aspx?h=" & ImageHeight & "&img=" & Replace(myReturn, "//", "/")
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
    Public Function FixFolders(ByVal sPageID As String) As Boolean
        Dim sNewPagePath As String = ("")
        Dim sNewImagePath As String = ("")
        Dim sNewThumbnailPath As String = ("")
        Using dtPageImage As New DataTable
            For Each myImage As wpmImage In Me
                If ActiveSite.CurrentPageID = sPageID Then
                    sNewPagePath = ActiveSite.SiteGallery & "image/" & wpmUtil.FormatPageNameForURL(ActiveSite.GetCurrentPageName)
                    ' Create a Folder for Page 
                    sNewPagePath = HttpContext.Current.Server.MapPath(sNewPagePath)
                    wpmFileProcessing.CreateFolder(sNewPagePath)
                    ' Create a Folder for Image/Page and move this image file to the folder
                    sNewImagePath = sNewPagePath
                    wpmFileProcessing.CreateFolder(sNewImagePath)
                    wpmFileProcessing.MoveFile(HttpContext.Current.Server.MapPath(ActiveSite.SiteGallery & _
                                   myImage.ImageFileName), sNewPagePath & "/" & myImage.ImageFileName)
                End If
            Next
        End Using
        Return True
    End Function
    Private Function BuildImageAdmin(ByVal ImageID As String, ByVal PageID As String) As String
        If wpmUser.IsEditor() Then
            Return "<hr><a href=""" & wpmApp.Config.wpmWebHome() & "admin/ImageEdit.aspx?a=" & ImageID & "&c=" & PageID & """>Edit This Image</a> - " & _
                   "<a href=""" & wpmApp.Config.wpmWebHome() & "admin/ImageEdit.aspx?a=" & ImageID & "&c=" & PageID & """>Delete</a><br>"
        Else
            Return ""
        End If
    End Function

    ' Find Image By Image ID
    Private SearchImageID As String = ""
    Private Function GetImageByImageID(ByVal ImageID As String) As wpmImage
        SearchImageID = ImageID
        Return Me.Find(AddressOf FindImageByImageID)
    End Function
    Private Function FindImageByImageID(ByVal Image As wpmImage) As Boolean
        If Image.ImageID = SearchImageID Then
            Return True
        Else
            Return False
        End If
    End Function
End Class

