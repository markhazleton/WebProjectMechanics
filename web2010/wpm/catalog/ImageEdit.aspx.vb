Imports WebProjectMechanics

Partial Class wpm_wpmImageEdit
    Inherits wpmPage
    Implements IImageRow
    Private myimage As wpmImagePresenter
    Private reqImageID As String
    Private reqPageID As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        reqImageID = GetProperty("a", "")
        reqPageID = GetProperty("c", "")

        If wpmUser.IsEditor Then
            myimage = New wpmImagePresenter(Me)
            If Not IsPostBack Then
                If reqImageID <> "" Then
                    myimage.SetImageValues(reqImageID)
                End If
                imgThumbnail.ImageUrl = myimage.GetThubmailURL(400, pageActiveSite.SiteGallery)
                imgThumbnail.ToolTip = "This is the thumbnail for " & ImageTitle
            End If

        Else
            Session("ListPageURL") = Me.Page.Request.Url.AbsoluteUri.ToString
            Response.Redirect("/wpm/login/login.aspx")
        End If
    End Sub
    Protected Sub Submit1_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Submit1.ServerClick
        If wpmUser.IsEditor Then
            myimage.updateMyUi(reqImageID)
        End If

        If pageActiveSite.FindCurrentRow(pageActiveSite.CurrentPageID, String.Empty, "Page") Then
            Response.Redirect(pageActiveSite.CurrentDisplayURL)
        Else
            Response.Redirect(Session("ListPageURL"))
        End If

    End Sub
    Protected Sub Cancel_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cancel.ServerClick
        Response.Redirect(Session("ListPageURL"))
    End Sub
    Protected Sub Delete_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Delete.ServerClick
        DeleteImage(reqImageID)
        '        wpmFileProcessing.DeleteFile(Server.MapPath(mycatalog.GetImagePath(mySiteMap.SiteGallery, Me.ImageFileName, mysitemap.GetCurrentPageName, "800")))
        Response.Redirect(Session("ListPageURL"))
    End Sub
    Public Function DeleteImage(ByVal ReqImageID As String) As Boolean
        Dim result As Boolean
        Dim mySQL As String
        Try
            mySQL = ("delete from pageimage where imageid=" & ReqImageID)
            wpmDB.RunDeleteSQL(mySQL, "PageImage by Image")
        Catch ex As Exception
            wpmLogging.AuditLog("Error on DeleteImage-pageimage", ex.ToString)
        End Try
        Try
            mySQL = ("delete from [image] where [image].[imageid]=" & ReqImageID)
            wpmDB.RunDeleteSQL(mySQL, "Image by ImageID")
        Catch ex As Exception
            wpmLogging.AuditLog("Error on DeleteImage-image", ex.ToString)
        End Try

        Return result
    End Function

    Public Property ImageTitle() As String Implements IImageRow.Title
        Get
            Return wpmUTIL.GetDBString(tbImageTitle.Text)
        End Get
        Set(ByVal value As String)
            tbImageTitle.Text = value
        End Set
    End Property
    Public Property ImageActive() As Boolean Implements IImageRow.Active
        Get
            Return cbImageActive.Checked
        End Get
        Set(ByVal value As Boolean)
            cbImageActive.Checked = value
        End Set
    End Property

    Public Property ImageColor() As String Implements IImageRow.Color
        Get
            Return tbImageColor.Text
        End Get
        Set(ByVal value As String)
            tbImageColor.Text = value
        End Set
    End Property

    Public Property CompanyID() As String Implements IImageRow.CompanyId
        Get
            Return x_CompanyID.Value
        End Get
        Set(ByVal value As String)
            x_CompanyID.Value = value
        End Set
    End Property

    Public Property ContactID() As String Implements IImageRow.ContactId
        Get
            Return x_ContactID.Value
        End Get
        Set(ByVal value As String)
            x_ContactID.Value = value
        End Set
    End Property

    Public Property ImageComment() As String Implements IImageRow.ImageComment
        Get
            Return wpmUTIL.GetDBString(tbImageComment.Text)
        End Get
        Set(ByVal value As String)
            tbImageComment.Text = value
        End Set
    End Property

    Public Property ImageDate() As Date Implements IImageRow.ImageDate
        Get
            Return wpmUTIL.GetDBDate(tbImageDate.Text)
        End Get
        Set(ByVal value As Date)
            tbImageDate.Text = value
        End Set
    End Property

    Public Property ImageDescription() As String Implements IImageRow.ImageDescription
        Get
            Return wpmUTIL.GetDBString(tbImageDescription.Text)
        End Get
        Set(ByVal value As String)
            tbImageDescription.Text = value
        End Set
    End Property

    Public Property ImageFileName() As String Implements IImageRow.ImageFileName
        Get
            Return wpmUTIL.GetDBString(tbImageFileName.Text)
        End Get
        Set(ByVal value As String)
            tbImageFileName.Text = value
        End Set
    End Property
    Public Property ImageThumbFileName() As String Implements IImageRow.ImageThumbFileName
        Get
            Return wpmUTIL.GetDBString(tbImageFileName.Text)
        End Get
        Set(ByVal value As String)
            ' Do Nothing
        End Set
    End Property

    Public Property ImageID() As String Implements IImageRow.ImageId
        Get
            Return x_ImageID.Value
        End Get
        Set(ByVal value As String)
            x_ImageID.Value = value
        End Set
    End Property

    Public Property ImageName() As String Implements IImageRow.ImageName
        Get
            Return wpmUTIL.GetDBString(tbImageName.Text)
        End Get
        Set(ByVal value As String)
            tbImageName.Text = value
        End Set
    End Property

    Public Property Medium() As String Implements IImageRow.Medium
        Get
            Return wpmUTIL.GetDBString(tbImageMedium.Text)
        End Get
        Set(ByVal value As String)
            tbImageMedium.Text = value
        End Set
    End Property

    Public Property ModifiedDate() As Date Implements IImageRow.ModifiedDate
        Get
            Return wpmUTIL.GetDBDate(tbImageDate.Text)
        End Get
        Set(ByVal value As Date)
            tbImageDate.Text = value
        End Set
    End Property

    Public Property Price() As String Implements IImageRow.Price
        Get
            Return wpmUTIL.GetDBString(tbImagePrice.Text)
        End Get
        Set(ByVal value As String)
            tbImagePrice.Text = value
        End Set
    End Property

    Public Property Size() As String Implements IImageRow.Size
        Get
            Return wpmUTIL.GetDBString(tbImageSize.Text)
        End Get
        Set(ByVal value As String)
            tbImageSize.Text = value
        End Set
    End Property

    Public Property Sold() As Boolean Implements IImageRow.Sold
        Get
            Return wpmUTIL.GetDBBoolean(cbImageSold.Checked)
        End Get
        Set(ByVal value As Boolean)
            cbImageSold.Checked = value
        End Set
    End Property

    Public Property Subject() As String Implements IImageRow.Subject
        Get
            Return wpmUTIL.GetDBString(tbImageSubject.Text)
        End Get
        Set(ByVal value As String)
            tbImageSubject.Text = value
        End Set
    End Property

    Public Property VersionNumber() As Integer Implements IImageRow.VersionNumber
        Get
            Return wpmUTIL.GetDBInteger(x_VersionNumber.Value)
        End Get
        Set(ByVal value As Integer)
            x_VersionNumber.Value = value
        End Set
    End Property

End Class
