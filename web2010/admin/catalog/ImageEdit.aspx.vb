Imports WebProjectMechanics

Partial Class Admin_LocationImageEdit
    Inherits ApplicationPage
    Implements ILocationImage

    Private myimage As LocationImagePresenter
    Private reqLocationImageID As String
    Private reqLocationID As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        reqLocationImageID = wpm_GetProperty("ImageID", "")
        reqLocationID = wpm_GetProperty("LocationID", "")
        If wpm_IsEditor Then
            myimage = New LocationImagePresenter(Me)
            If Not IsPostBack Then
                If reqLocationImageID <> "" Then
                    myimage.SetImageValues(reqLocationImageID)
                End If
                imgThumbnail.ImageUrl = myimage.GetThubmailURL(400, wpm_SiteGallery)
                imgThumbnail.ToolTip = "This is the thumbnail for " & ImageTitle
            End If
        Else
            wpm_ListPageURL = Me.Page.Request.Url.AbsoluteUri.ToString
            Response.Redirect(wpm_SiteConfig.AdminFolder & "login/login.aspx")
        End If
    End Sub
    Protected Sub Submit1_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Submit1.ServerClick
        If wpm_IsEditor Then
            myimage.updateMyLocationImage(reqLocationImageID)
        End If

        If masterPage.myCompany.FindCurrentRow(masterPage.myCompany.CurrentLocationID, String.Empty, "Page") Then
            Response.Redirect(masterPage.myCompany.CurrentDisplayURL)
        Else
            Response.Redirect(wpm_ListPageURL)
        End If

    End Sub
    Protected Sub Cancel_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cancel.ServerClick
        Response.Redirect(wpm_ListPageURL)
    End Sub
    Protected Sub Delete_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Delete.ServerClick
        DeleteImage(reqLocationImageID)
        Response.Redirect(wpm_ListPageURL)
    End Sub
    Public Shared Function DeleteImage(ByVal ReqImageID As String) As Boolean
        Dim result As Boolean
        Dim mySQL As String
        Try
            mySQL = ("delete from pageimage where imageid=" & ReqImageID)
            wpm_RunDeleteSQL(mySQL, "PageImage by Image")
        Catch ex As Exception
            ApplicationLogging.AuditLog("Error on DeleteImage-pageimage", ex.ToString)
        End Try
        Try
            mySQL = ("delete from [image] where [image].[imageid]=" & ReqImageID)
            wpm_RunDeleteSQL(mySQL, "Image by ImageID")
        Catch ex As Exception
            ApplicationLogging.AuditLog("Error on DeleteImage-image", ex.ToString)
        End Try

        Return result
    End Function

    Public Property ImageTitle() As String Implements ILocationImage.Title
        Get
            Return wpm_GetDBString(tbImageTitle.Text)
        End Get
        Set(ByVal value As String)
            tbImageTitle.Text = value
        End Set
    End Property
    Public Property ImageActive() As Boolean Implements ILocationImage.Active
        Get
            ' Return cbImageActive.Checked
            Return True
        End Get
        Set(ByVal value As Boolean)
            cbImageActive.Checked = True
        End Set
    End Property

    Public Property ImageColor() As String Implements ILocationImage.Color
        Get
            Return tbImageColor.Text
        End Get
        Set(ByVal value As String)
            tbImageColor.Text = value
        End Set
    End Property

    Public Property ContactID() As String Implements ILocationImage.ContactId
        Get
            Return x_ContactID.Value
        End Get
        Set(ByVal value As String)
            x_ContactID.Value = value
        End Set
    End Property

    Public Property ImageComment() As String Implements ILocationImage.ImageComment
        Get
            Return wpm_GetDBString(tbImageComment.Text)
        End Get
        Set(ByVal value As String)
            tbImageComment.Text = value
        End Set
    End Property

    Public Property ImageDate() As Date Implements ILocationImage.ImageDate
        Get
            Return wpm_GetDBDate(tbImageDate.Text)
        End Get
        Set(ByVal value As Date)
            tbImageDate.Text = value
        End Set
    End Property

    Public Property ImageDescription() As String Implements ILocationImage.ImageDescription
        Get
            Return wpm_GetDBString(tbImageDescription.Text)
        End Get
        Set(ByVal value As String)
            tbImageDescription.Text = value
        End Set
    End Property

    Public Property ImageFileName() As String Implements ILocationImage.ImageFileName
        Get
            Return wpm_GetDBString(tbImageFileName.Text)
        End Get
        Set(ByVal value As String)
            tbImageFileName.Text = value
        End Set
    End Property
    Public Property ImageThumbFileName() As String Implements ILocationImage.ImageThumbFileName
        Get
            Return wpm_GetDBString(tbImageFileName.Text)
        End Get
        Set(ByVal value As String)
            ' Do Nothing
        End Set
    End Property

    Public Property ImageID() As String Implements ILocationImage.ImageId
        Get
            Return x_ImageID.Value
        End Get
        Set(ByVal value As String)
            x_ImageID.Value = value
        End Set
    End Property

    Public Property ImageName() As String Implements ILocationImage.ImageName
        Get
            Return wpm_GetDBString(tbImageName.Text)
        End Get
        Set(ByVal value As String)
            tbImageName.Text = value
        End Set
    End Property

    Public Property Medium() As String Implements ILocationImage.Medium
        Get
            Return wpm_GetDBString(tbImageMedium.Text)
        End Get
        Set(ByVal value As String)
            tbImageMedium.Text = value
        End Set
    End Property

    Public Property ModifiedDate() As Date Implements ILocationImage.ModifiedDate
        Get
            Return wpm_GetDBDate(tbImageDate.Text)
        End Get
        Set(ByVal value As Date)
            tbImageDate.Text = value
        End Set
    End Property

    Public Property Price() As String Implements ILocationImage.Price
        Get
            Return wpm_GetDBString(tbImagePrice.Text)
        End Get
        Set(ByVal value As String)
            tbImagePrice.Text = value
        End Set
    End Property

    Public Property Size() As String Implements ILocationImage.Size
        Get
            Return wpm_GetDBString(tbImageSize.Text)
        End Get
        Set(ByVal value As String)
            tbImageSize.Text = value
        End Set
    End Property

    Public Property Sold() As Boolean Implements ILocationImage.Sold
        Get
            Return wpm_GetDBBoolean(cbImageSold.Checked)
        End Get
        Set(ByVal value As Boolean)
            cbImageSold.Checked = value
        End Set
    End Property

    Public Property Subject() As String Implements ILocationImage.Subject
        Get
            Return wpm_GetDBString(tbImageSubject.Text)
        End Get
        Set(ByVal value As String)
            tbImageSubject.Text = value
        End Set
    End Property

    Public Property VersionNumber() As Integer Implements ILocationImage.VersionNumber
        Get
            Return wpm_GetDBInteger(x_VersionNumber.Value)
        End Get
        Set(ByVal value As Integer)
            x_VersionNumber.Value = value
        End Set
    End Property

    Public Property LocationID As String Implements ILocationImage.LocationID
        Get
            Return String.Empty
        End Get
        Set(value As String)

        End Set
    End Property

    Public Property ParentLocationID As String Implements ILocationImage.ParentLocationID
        Get
            Return String.Empty
        End Get
        Set(value As String)

        End Set
    End Property
End Class
