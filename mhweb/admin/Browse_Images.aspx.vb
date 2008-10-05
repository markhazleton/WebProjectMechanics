Imports System.IO
Imports System.Drawing
Imports System.Drawing.Bitmap
Imports System.Drawing.Drawing2D
Partial Class mhweb_catalog_Browse_Images
    Inherits mhPage
    Implements IImageRow
    Dim myImagePresenter As mhImagePresenter
    Private mySiteMap As mhSiteMap
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        mySiteMap = New mhSiteMap(Session)
        Dim mycatalog As New mhcatalog(mySiteMap)
        myImagePresenter = New mhImagePresenter(Me)
        Dim sRequestImageID As String = GetProperty("ImageID", "")
        If Me.IsPostBack Then
            If Me.x_ImageID.Value <> "" Then
                myImagePresenter.updateMyUi(Me.x_ImageID.Value)
                Me.Results.Text = myImagePresenter.Status
            End If
        Else
            Me.Results.Text = ""
        End If
        Dim myImage As New mhImageRows(mySiteMap.mySession.CompanyID)
        Me.BuildThumbnailList(myImage, sRequestImageID)
    End Sub
    
    Private Function PopulateContactDropDownList(ByVal sContactID As String, ByVal CompanyID As String, ByRef myDDL As DropDownList) As Boolean
        Dim sqlwrk As String
        Dim mydt As DataTable
        sqlwrk = "SELECT ContactID, PrimaryContact & ' (' & LogonName & ')' FROM Contact where Contact.CompanyID=" & CompanyID & " ORDER BY PrimaryContact "
        mydt = mhDB.GetDataTable(sqlwrk, "Browse_Image.PopulateContactDropDown")
        For Each row As DataRow In mydt.Rows
            Dim MyLI As New ListItem
            MyLI.Text = mhUTIL.GetDBString(row(1))
            MyLI.Value = mhUTIL.GetDBString(row(0))
            If MyLI.Value = sContactID Then
                MyLI.Selected = True
            End If
            myDDL.Items.Add(MyLI)
        Next
        Return True
    End Function
    Private Function PopulatePageDropDownList(ByVal sPageID As String, ByVal CompanyID As String, ByRef myDDL As DropDownList) As Boolean
        Dim sqlwrk As String
        Dim mydt As DataTable
        sqlwrk = "SELECT PageID, PageName FROM Page where Page.CompanyID=" & CompanyID & " ORDER BY PageName "
        mydt = mhDB.GetDataTable(sqlwrk, "Browse_Image.PopulatePageDropDown")
        For Each row As DataRow In mydt.Rows
            Dim MyLI As New ListItem
            MyLI.Text = mhUTIL.GetDBString(row(1))
            MyLI.Value = mhUTIL.GetDBString(row(0))
            If MyLI.Value = sPageID Then
                MyLI.Selected = True
            End If
            myDDL.Items.Add(MyLI)
        Next
        Return True
    End Function

    Private Function ValidateFolder(ByVal DirPath As String) As Boolean
        If Not mhfio.IsValidFolder(DirPath) Then
            mhfio.CreateFolder(DirPath)
        End If
        Return True
    End Function
    Private Function BuildThumbnailList(ByRef myImageRows As mhImageRows, ByRef sReqImageID As String) As Boolean
        Dim mySB As New StringBuilder(String.Empty)
        Dim myRelPath As New String(String.Empty)
        Dim myPath As New String(String.Empty)
        Dim filename As New String("")
        Dim DirPath As String = Server.MapPath(mySiteMap.mySiteFile.SiteGallery & "/image")
        ValidateFolder(DirPath)
        Dim files() As String = Directory.GetFiles(DirPath, "*.jpg")
        mySB.Append("<strong>Base Image<strong><ul>")
        For i As Integer = 0 To files.Length - 1
            Dim myImageFile As New Bitmap(files(i).ToString())
            filename = Replace(files(i).ToString, Server.MapPath("/" & mySiteMap.mySiteFile.SiteGallery), "")
            mySB.Append("<li><a href=""/mhweb/admin/browse_images.aspx?ImageID=" & CheckForImage(filename, myImageRows, sReqImageID) & """>" & filename & "</a></li>")
        Next
        mySB.Append("</ul><br/>")
        Dim myFolders() As String = Directory.GetDirectories(DirPath)
        For y As Integer = 0 To myFolders.Length - 1
            myPath = myFolders(y).ToString
            myRelPath = Replace(myFolders(y).ToString, Server.MapPath(Session("SiteGallery") & "image/"), "")
            mySB.Append("<hr/><strong>" & myRelPath & "</strong><ul>")
            Dim myFiles() As String = Directory.GetFiles(myPath, "*.jpg")
            For i As Integer = 0 To myFiles.Length - 1
                filename = Replace(myFiles(i).ToString, Server.MapPath("/" & mySiteMap.mySiteFile.SiteGallery), "")
                mySB.Append("<li><a href=""/mhweb/admin/browse_images.aspx?ImageID=" & CheckForImage(filename, myImageRows, sReqImageID) & """>" & filename & "</a></li>")
            Next
            mySB.Append("</ul>")
        Next
        Dim myLabel As New Label
        myLabel.Text = mySB.ToString
        Me.pnlThumbs.Controls.Add(myLabel)
        Return True
    End Function
    Private Function CheckForImage(ByVal sImageFileName As String, ByRef myImageRows As mhImageRows, ByRef sReqImageID As String) As String
        Dim sReturnID As String = ""
        For Each myImageRow As mhImageRow In myImageRows
            If mhfio.CompareFileName(sImageFileName, myImageRow.ImageFileName) Then
                sReturnID = myImageRow.ImageID
                If (myImageRow.ImageID = sReqImageID) Then
                    x_ImageID.Value = myImageRow.ImageID
                    x_VersionNumber.Value = myImageRow.VersionNumber
                    x_CompanyID.Value = myImageRow.CompanyID
                    lblImageTitle.Text = myImageRow.Title
                    lblImageName.Text = myImageRow.ImageName
                    lblImageDescription.Text = myImageRow.ImageDescription
                    lblImageComment.Text = myImageRow.ImageComment
                    lblImageFileName.Text = myImageRow.ImageFileName
                    lblImageSize.Text = myImageRow.Size
                    lblImagePrice.Text = myImageRow.Price
                    lblImageColor.Text = myImageRow.Color
                    lblImageSubject.Text = myImageRow.Subject
                    lblImageDate.Text = myImageRow.ImageDate
                    lblImageMedium.Text = myImageRow.Medium
                    cbImageSold.Checked = myImageRow.Sold
                    Me.imgMain.ImageUrl = mySiteMap.mySiteFile.SiteGallery & myImageRow.ImageFileName
                End If
                Exit For
            End If
        Next
        If sReturnID = "" Then
            ' Create New Image Record
            Dim myNewRow As New mhImageRow()
            myNewRow.ImageFileName = Replace(sImageFileName, "\", "/")
            myNewRow.ImageName = sImageFileName
            myNewRow.ImageComment = "Image Record Created by Browse-Images"
            myNewRow.CompanyID = mySiteMap.mySession.CompanyID
            myNewRow.ContactID = mySiteMap.mySession.ContactID
            sReturnID = myNewRow.createImage()
        End If
        Return sReturnID
    End Function
    Public Property Active() As Boolean Implements mhweb.IImageRow.Active
        Get
            Return True
        End Get
        Set(ByVal value As Boolean)
            '
        End Set
    End Property
    Public Property Color() As String Implements mhweb.IImageRow.Color
        Get
            Return mhUTIL.GetDBString(Me.lblImageColor.Text)
        End Get
        Set(ByVal value As String)
            Me.lblImageColor.Text = value
        End Set
    End Property
    Public Property CompanyID() As String Implements mhweb.IImageRow.CompanyID
        Get
            Return mhUTIL.GetDBString(Me.x_CompanyID.Value)
        End Get
        Set(ByVal value As String)
            Me.x_CompanyID.Value = value
        End Set
    End Property
    Public Property ContactID() As String Implements mhweb.IImageRow.ContactID
        Get
            Return Me.mySession.ContactID
        End Get
        Set(ByVal value As String)
            Me.mySession.ContactID = value
        End Set
    End Property
    Public Property ImageComment() As String Implements mhweb.IImageRow.ImageComment
        Get
            Return mhUTIL.GetDBString(Me.lblImageComment.Text)
        End Get
        Set(ByVal value As String)
            Me.lblImageComment.Text = value
        End Set
    End Property
    Public Property ImageDate() As Date Implements mhweb.IImageRow.ImageDate
        Get
            Return mhUTIL.GetDBDate(Me.lblImageDate.Text)
        End Get
        Set(ByVal value As Date)
            Me.lblImageDate.Text = value.ToShortDateString
        End Set
    End Property
    Public Property ImageDescription() As String Implements mhweb.IImageRow.ImageDescription
        Get
            Return mhUTIL.GetDBString(Me.lblImageDescription.Text)
        End Get
        Set(ByVal value As String)
            Me.lblImageDescription.Text = value
        End Set
    End Property
    Public Property ImageFileName() As String Implements mhweb.IImageRow.ImageFileName
        Get
            Return mhUTIL.GetDBString(Me.lblImageFileName.Text)
        End Get
        Set(ByVal value As String)
            Me.lblImageFileName.Text = value
        End Set
    End Property
    Public Property ImageThumbFileName() As String Implements mhweb.IImageRow.ImageThumbFileName
        Get
            Return mhUTIL.GetDBString(Me.lblImageFileName.Text)
        End Get
        Set(ByVal value As String)
            ''
        End Set
    End Property
    Public Property ImageID() As String Implements mhweb.IImageRow.ImageID
        Get
            Return mhUTIL.GetDBString(Me.x_ImageID.Value)
        End Get
        Set(ByVal value As String)
            Me.x_ImageID.Value = value
        End Set
    End Property
    Public Property ImageName() As String Implements mhweb.IImageRow.ImageName
        Get
            Return mhUTIL.GetDBString(Me.lblImageName.Text)
        End Get
        Set(ByVal value As String)
            Me.lblImageName.Text = value
        End Set
    End Property
    Public Property Medium() As String Implements mhweb.IImageRow.Medium
        Get
            Return mhUTIL.GetDBString(Me.lblImageMedium.Text)
        End Get
        Set(ByVal value As String)
            Me.lblImageMedium.Text = value
        End Set
    End Property
    Public Property ModifiedDate() As Date Implements mhweb.IImageRow.ModifiedDate
        Get
            Return Now()
        End Get
        Set(ByVal value As Date)
            Me.x_modifiedDT.Value = value
        End Set
    End Property
    Public Property Price() As String Implements mhweb.IImageRow.Price
        Get
            Return mhUTIL.GetDBString(Me.lblImagePrice.Text)
        End Get
        Set(ByVal value As String)
            Me.lblImagePrice.Text = value
        End Set
    End Property
    Public Property Size() As String Implements mhweb.IImageRow.Size
        Get
            Return mhUTIL.GetDBString(Me.lblImageSize.Text)
        End Get
        Set(ByVal value As String)
            Me.lblImageSize.Text = value
        End Set
    End Property
    Public Property Sold() As Boolean Implements mhweb.IImageRow.Sold
        Get
            Return mhUTIL.GetDBBoolean(Me.cbImageSold.Checked)
        End Get
        Set(ByVal value As Boolean)
            Me.cbImageSold.Checked = value
        End Set
    End Property
    Public Property Subject() As String Implements mhweb.IImageRow.Subject
        Get
            Return mhUTIL.GetDBString(Me.lblImageSubject.Text)
        End Get
        Set(ByVal value As String)
            Me.lblImageSubject.Text = value
        End Set
    End Property
    Public Property Title1() As String Implements mhweb.IImageRow.Title
        Get
            Return mhUTIL.GetDBString(Me.lblImageTitle.Text)
        End Get
        Set(ByVal value As String)
            Me.lblImageTitle.Text = value
        End Set
    End Property
    Public Property VersionNumber() As Integer Implements mhweb.IImageRow.VersionNumber
        Get
            Return mhUTIL.GetDBInteger(Me.x_VersionNumber.Value) + 1
        End Get
        Set(ByVal value As Integer)
            Me.x_VersionNumber.Value = value
        End Set
    End Property

End Class
