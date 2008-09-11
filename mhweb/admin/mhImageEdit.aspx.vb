
Partial Class mhweb_mhImage
    Inherits mhPage
    Implements IImageRow
    Dim myimage As mhImagePresenter
    Dim reqImageID As String
    Dim reqPageID As String
    Dim mycatalog As mhcatalog
    Dim mysitemap As mhSiteMap
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        mysitemap = New mhSiteMap(Session)
        mycatalog = New mhcatalog(mysitemap)
        reqImageID = GetProperty("a", "")
        reqPageID = GetProperty("c", "")
        If mhUser.IsEditor Then
            mysitemap.FindCurrentRow(reqPageID, "", "Page")
            myimage = New mhImagePresenter(Me)
            If Not IsPostBack Then
                If reqImageID <> "" Then
                    myimage.SetImageValues(reqImageID)
                End If
            End If
            imgThumbnail.ImageUrl = mycatalog.GetImagePath(mysitemap.mySiteFile.SiteGallery, ImageFileName, mysitemap.CurrentMapRow.PageName, "200")
            imgThumbnail.ToolTip = "This is the thumbnail for " & ImageTitle
            mysitemap.mySession.AddHTMLHead = mysitemap.mySession.AddHTMLHead & ""
            Dim mycontents As New StringBuilder
            mhfio.ReadTextFile(HttpContext.Current.Server.MapPath("/util/forms/AddToHTMLHead.txt"), mycontents)
            mysitemap.mySession.AddHTMLHead = mycontents.ToString
        Else
            Response.Redirect(Session("ListPageURL"))
        End If
    End Sub
    Protected Sub Submit1_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Submit1.ServerClick
        If mhUser.IsEditor Then
            myimage.updateMyUi(reqImageID)
        End If
        Response.Redirect(Session("ListPageURL"))
    End Sub
    Protected Sub Cancel_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cancel.ServerClick
        Response.Redirect(Session("ListPageURL"))
    End Sub
    Protected Sub Delete_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Delete.ServerClick
        DeleteImage(reqImageID)
        mhfio.DeleteFile(Server.MapPath(mycatalog.GetImagePath(mysitemap.mySiteFile.SiteGallery, Me.ImageFileName, mysitemap.CurrentMapRow.PageName, "800")))
        Response.Redirect(mysitemap.CurrentMapRow.DisplayURL)
    End Sub
    Public Function DeleteImage(ByVal ReqImageID As String) As Boolean
        Dim result As Boolean
        Dim mySQL As String
        Try
            mySQL = ("delete from pageimage where imageid=" & ReqImageID)
            mhDB.RunDeleteSQL(mySQL, "PageImage by Image")
        Catch ex As Exception
            mhUTIL.AuditLog("Error on DeleteImage-pageimage", ex.ToString)
        End Try
        Try
            mySQL = ("delete from [image] where [image].[imageid]=" & ReqImageID)
            mhDB.RunDeleteSQL(mySQL, "Image by ImageID")
        Catch ex As Exception
            mhUTIL.AuditLog("Error on DeleteImage-image", ex.ToString)
        End Try

        Return result
    End Function

    Public Property ImageTitle() As String Implements mhweb.IImageRow.Title
        Get
            Return mhUTIL.GetDBString(tbImageTitle.Text)
        End Get
        Set(ByVal value As String)
            tbImageTitle.Text = value
        End Set
    End Property
    Public Property ImageActive() As Boolean Implements mhweb.IImageRow.Active
        Get
            Return cbImageActive.Checked
        End Get
        Set(ByVal value As Boolean)
            cbImageActive.Checked = value
        End Set
    End Property

    Public Property ImageColor() As String Implements mhweb.IImageRow.Color
        Get
            Return tbImageColor.Text
        End Get
        Set(ByVal value As String)
            tbImageColor.Text = value
        End Set
    End Property

    Public Property CompanyID() As String Implements mhweb.IImageRow.CompanyID
        Get
            Return x_CompanyID.Value
        End Get
        Set(ByVal value As String)
            x_CompanyID.Value = value
        End Set
    End Property

    Public Property ContactID() As String Implements mhweb.IImageRow.ContactID
        Get
            Return x_ContactID.Value
        End Get
        Set(ByVal value As String)
            x_ContactID.Value = value
        End Set
    End Property

    Public Property ImageComment() As String Implements mhweb.IImageRow.ImageComment
        Get
            Return mhUTIL.GetDBString(tbImageComment.Text)
        End Get
        Set(ByVal value As String)
            tbImageComment.Text = value
        End Set
    End Property

    Public Property ImageDate() As Date Implements mhweb.IImageRow.ImageDate
        Get
            Return mhUTIL.GetDBDate(tbImageDate.Text)
        End Get
        Set(ByVal value As Date)
            tbImageDate.Text = value
        End Set
    End Property

    Public Property ImageDescription() As String Implements mhweb.IImageRow.ImageDescription
        Get
            Return mhUTIL.GetDBString(tbImageDescription.Text)
        End Get
        Set(ByVal value As String)
            tbImageDescription.Text = value
        End Set
    End Property

    Public Property ImageFileName() As String Implements mhweb.IImageRow.ImageFileName
        Get
            Return mhUTIL.GetDBString(tbImageFileName.Text)
        End Get
        Set(ByVal value As String)
            tbImageFileName.Text = value
        End Set
    End Property
    Public Property ImageThumbFileName() As String Implements mhweb.IImageRow.ImageThumbFileName
        Get
            Return mhUTIL.GetDBString(tbImageFileName.Text)
        End Get
        Set(ByVal value As String)
            ' Do Nothing
        End Set
    End Property

    Public Property ImageID() As String Implements mhweb.IImageRow.ImageID
        Get
            Return x_ImageID.Value
        End Get
        Set(ByVal value As String)
            x_ImageID.Value = value
        End Set
    End Property

    Public Property ImageName() As String Implements mhweb.IImageRow.ImageName
        Get
            Return mhUTIL.GetDBString(tbImageName.Text)
        End Get
        Set(ByVal value As String)
            tbImageName.Text = value
        End Set
    End Property

    Public Property Medium() As String Implements mhweb.IImageRow.Medium
        Get
            Return mhUTIL.GetDBString(tbImageMedium.Text)
        End Get
        Set(ByVal value As String)
            tbImageMedium.Text = value
        End Set
    End Property

    Public Property ModifiedDate() As Date Implements mhweb.IImageRow.ModifiedDate
        Get
            Return mhUTIL.GetDBDate(tbImageDate.Text)
        End Get
        Set(ByVal value As Date)
            tbImageDate.Text = value
        End Set
    End Property

    Public Property Price() As String Implements mhweb.IImageRow.Price
        Get
            Return mhUTIL.GetDBString(tbImagePrice.Text)
        End Get
        Set(ByVal value As String)
            tbImagePrice.Text = value
        End Set
    End Property

    Public Property Size() As String Implements mhweb.IImageRow.Size
        Get
            Return mhUTIL.GetDBString(tbImageSize.Text)
        End Get
        Set(ByVal value As String)
            tbImageSize.Text = value
        End Set
    End Property

    Public Property Sold() As Boolean Implements mhweb.IImageRow.Sold
        Get
            Return mhUTIL.GetDBBoolean(cbImageSold.Checked)
        End Get
        Set(ByVal value As Boolean)
            cbImageSold.Checked = value
        End Set
    End Property

    Public Property Subject() As String Implements mhweb.IImageRow.Subject
        Get
            Return mhUTIL.GetDBString(tbImageSubject.Text)
        End Get
        Set(ByVal value As String)
            tbImageSubject.Text = value
        End Set
    End Property

    Public Property VersionNumber() As Integer Implements mhweb.IImageRow.VersionNumber
        Get
            Return mhUTIL.GetDBInteger(x_VersionNumber.Value)
        End Get
        Set(ByVal value As Integer)
            x_VersionNumber.Value = value
        End Set
    End Property

End Class
