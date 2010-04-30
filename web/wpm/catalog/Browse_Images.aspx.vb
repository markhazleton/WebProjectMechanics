Imports System.IO
Imports System.Drawing
Imports System.Drawing.Bitmap
Imports System.Drawing.Drawing2D
Partial Class wpm_catalog_Browse_Images
    Inherits AspNetMaker7_WPMGen
    Implements IImageRow
    Dim myImagePresenter As wpmImagePresenter
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        myImagePresenter = New wpmImagePresenter(Me)
        Dim sRequestImageID As String = GetProperty("ImageID", String.Empty)
        Dim sRequestFolder As String = wpmUTIL.FixInvalidCharacters(GetProperty("SubFolder", String.Empty))
        Dim myImage As New wpmSiteImageList(pageActiveSite.CompanyID)

        If Me.IsPostBack Then
            sRequestFolder = Me.x_SubFolder.Value.ToString
            If Me.x_ImageID.Value <> "" Then
                myImagePresenter.updateMyUi(Me.x_ImageID.Value)
                Me.Results.Text = myImagePresenter.Status
            End If
        Else
            Me.x_SubFolder.Value = sRequestFolder
            Me.Results.Text = "No Image To Update"
        End If

        Me.BuildThumbnailList(myImage, sRequestImageID, sRequestFolder)
    End Sub

    Private Function PopulateContactDropDownList(ByVal sContactID As String, ByVal CompanyID As String, ByRef myDDL As DropDownList) As Boolean
        Dim sqlwrk As String
        Dim mydt As DataTable
        sqlwrk = "SELECT ContactID, PrimaryContact & ' (' & LogonName & ')' FROM Contact where Contact.CompanyID=" & CompanyID & " ORDER BY PrimaryContact "
        mydt = wpmDB.GetDataTable(sqlwrk, "Browse_Image.PopulateContactDropDown")
        For Each row As DataRow In mydt.Rows
            Dim MyLI As New ListItem
            MyLI.Text = wpmUTIL.GetDBString(row(1))
            MyLI.Value = wpmUTIL.GetDBString(row(0))
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
        mydt = wpmDB.GetDataTable(sqlwrk, "Browse_Image.PopulatePageDropDown")
        For Each row As DataRow In mydt.Rows
            Dim MyLI As New ListItem
            MyLI.Text = wpmUTIL.GetDBString(row(1))
            MyLI.Value = wpmUTIL.GetDBString(row(0))
            If MyLI.Value = sPageID Then
                MyLI.Selected = True
            End If
            myDDL.Items.Add(MyLI)
        Next
        Return True
    End Function

    Private Function ValidateFolder(ByVal DirPath As String) As Boolean
        If Not wpmFileIO.IsValidFolder(DirPath) Then
            wpmFileIO.CreateFolder(DirPath)
        End If
        Return True
    End Function
    Private Function BuildThumbnailList(ByRef myImageRows As wpmSiteImageList, ByRef sReqImageID As String, ByRef sReqSubFolder As String) As Boolean
        Dim mySB As New StringBuilder("<a href=""/wpm/admin/browse_images.aspx"">View Folder List</a>")
        Dim myRelPath As New String(String.Empty)
        Dim myPath As New String(String.Empty)
        Dim filename As New String("")
        Dim DirPath As String = Server.MapPath(pageActiveSite.SiteGallery & "/image")
        ValidateFolder(DirPath)
        If sReqSubFolder = String.Empty Then
            Dim files() As String = Directory.GetFiles(DirPath, "*.jpg")
            mySB.Append("<strong>Base Image<strong><ul>")
            For i As Integer = 0 To files.Length - 1
                Dim myImageFile As New Bitmap(files(i).ToString())
                filename = Replace(files(i).ToString, Server.MapPath("/" & pageActiveSite.SiteGallery), "")
                mySB.Append("<li><a href=""/wpm/admin/browse_images.aspx?ImageID=" & CheckForImage(filename, myImageRows, sReqImageID) & """>" & _
                            filename & "</a></li>")
            Next
            mySB.Append("</ul><br/>")
        End If
        '
        '  Code to list all sub-directories
        '
        Dim myFolders() As String = Directory.GetDirectories(DirPath)
        For y As Integer = 0 To myFolders.Length - 1
            myPath = myFolders(y).ToString
            myRelPath = Replace(myFolders(y).ToString, Server.MapPath(Session("SiteGallery") & "image/"), "")
            If sReqSubFolder = String.Empty Then
                mySB.Append("<hr><a href=""/wpm/admin/browse_images.aspx?SubFolder=" & myRelPath & """>" & myRelPath & "</a>")
            Else
                If sReqSubFolder = myRelPath Then
                    mySB.Append("<ul>")
                    Dim myFiles() As String = Directory.GetFiles(myPath, "*.jpg")
                    For i As Integer = 0 To myFiles.Length - 1
                        filename = Replace(myFiles(i).ToString, Server.MapPath("/" & pageActiveSite.SiteGallery), "")
                        mySB.Append("<li><a href=""/wpm/admin/browse_images.aspx?SubFolder=" & myRelPath & "&ImageID=" & CheckForImage(filename, myImageRows, sReqImageID) & """>" & filename & "</a></li>")
                    Next
                    mySB.Append("</ul>")
                End If
            End If
        Next
        Dim myLabel As New Label With {.Text = mySB.ToString}
        Me.pnlThumbs.Controls.Add(myLabel)
        Return True
    End Function
    Private Function CheckForImage(ByVal sImageFileName As String, ByRef myImageRows As wpmSiteImageList, ByRef sReqImageID As String) As String
        Dim sReturnID As String = ""
        For Each myImageRow As wpmImage In myImageRows
            If wpmFileIO.CompareFileName(sImageFileName, myImageRow.ImageFileName) Then
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
                    Me.imgMain.ImageUrl = pageActiveSite.SiteGallery & myImageRow.ImageFileName
                End If
                Exit For
            End If
        Next
        If sReturnID = "" Then
            ' Create New Image Record
            Dim myNewRow As New wpmImage()
            myNewRow.ImageFileName = Replace(sImageFileName, "\", "/")
            myNewRow.ImageName = sImageFileName
            myNewRow.ImageComment = "Image Record Created by Browse-Images"
            myNewRow.CompanyID = pageActiveSite.CompanyID
            myNewRow.ContactID = pageActiveSite.ContactID
            sReturnID = myNewRow.createImage()
        End If
        Return sReturnID
    End Function
    Public Property Active() As Boolean Implements IImageRow.Active
        Get
            Return True
        End Get
        Set(ByVal value As Boolean)
            '
        End Set
    End Property
    Public Property Color() As String Implements IImageRow.Color
        Get
            Return wpmUTIL.GetDBString(Me.lblImageColor.Text)
        End Get
        Set(ByVal value As String)
            Me.lblImageColor.Text = value
        End Set
    End Property
    Public Property CompanyID() As String Implements IImageRow.CompanyID
        Get
            Return wpmUTIL.GetDBString(Me.x_CompanyID.Value)
        End Get
        Set(ByVal value As String)
            Me.x_CompanyID.Value = value
        End Set
    End Property
    Public Property ContactID() As String Implements IImageRow.ContactID
        Get
            Return pageActiveSite.Session.ContactID
        End Get
        Set(ByVal value As String)
            pageActiveSite.Session.ContactID = value
        End Set
    End Property
    Public Property ImageComment() As String Implements IImageRow.ImageComment
        Get
            Return wpmUTIL.GetDBString(Me.lblImageComment.Text)
        End Get
        Set(ByVal value As String)
            Me.lblImageComment.Text = value
        End Set
    End Property
    Public Property ImageDate() As Date Implements IImageRow.ImageDate
        Get
            Return wpmUTIL.GetDBDate(Me.lblImageDate.Text)
        End Get
        Set(ByVal value As Date)
            Me.lblImageDate.Text = value.ToShortDateString
        End Set
    End Property
    Public Property ImageDescription() As String Implements IImageRow.ImageDescription
        Get
            Return wpmUTIL.GetDBString(Me.lblImageDescription.Text)
        End Get
        Set(ByVal value As String)
            Me.lblImageDescription.Text = value
        End Set
    End Property
    Public Property ImageFileName() As String Implements IImageRow.ImageFileName
        Get
            Return wpmUTIL.GetDBString(Me.lblImageFileName.Text)
        End Get
        Set(ByVal value As String)
            Me.lblImageFileName.Text = value
        End Set
    End Property
    Public Property ImageThumbFileName() As String Implements IImageRow.ImageThumbFileName
        Get
            Return wpmUTIL.GetDBString(Me.lblImageFileName.Text)
        End Get
        Set(ByVal value As String)
            ''
        End Set
    End Property
    Public Property ImageID() As String Implements IImageRow.ImageID
        Get
            Return wpmUTIL.GetDBString(Me.x_ImageID.Value)
        End Get
        Set(ByVal value As String)
            Me.x_ImageID.Value = value
        End Set
    End Property
    Public Property ImageName() As String Implements IImageRow.ImageName
        Get
            Return wpmUTIL.GetDBString(Me.lblImageName.Text)
        End Get
        Set(ByVal value As String)
            Me.lblImageName.Text = value
        End Set
    End Property
    Public Property Medium() As String Implements IImageRow.Medium
        Get
            Return wpmUTIL.GetDBString(Me.lblImageMedium.Text)
        End Get
        Set(ByVal value As String)
            Me.lblImageMedium.Text = value
        End Set
    End Property
    Public Property ModifiedDate() As Date Implements IImageRow.ModifiedDate
        Get
            Return System.DateTime.Now()
        End Get
        Set(ByVal value As Date)
            Me.x_modifiedDT.Value = value
        End Set
    End Property
    Public Property Price() As String Implements IImageRow.Price
        Get
            Return wpmUTIL.GetDBString(Me.lblImagePrice.Text)
        End Get
        Set(ByVal value As String)
            Me.lblImagePrice.Text = value
        End Set
    End Property
    Public Property Size() As String Implements IImageRow.Size
        Get
            Return wpmUTIL.GetDBString(Me.lblImageSize.Text)
        End Get
        Set(ByVal value As String)
            Me.lblImageSize.Text = value
        End Set
    End Property
    Public Property Sold() As Boolean Implements IImageRow.Sold
        Get
            Return wpmUTIL.GetDBBoolean(Me.cbImageSold.Checked)
        End Get
        Set(ByVal value As Boolean)
            Me.cbImageSold.Checked = value
        End Set
    End Property
    Public Property Subject() As String Implements IImageRow.Subject
        Get
            Return wpmUTIL.GetDBString(Me.lblImageSubject.Text)
        End Get
        Set(ByVal value As String)
            Me.lblImageSubject.Text = value
        End Set
    End Property
    Public Property Title1() As String Implements IImageRow.Title
        Get
            Return wpmUTIL.GetDBString(Me.lblImageTitle.Text)
        End Get
        Set(ByVal value As String)
            Me.lblImageTitle.Text = value
        End Set
    End Property
    Public Property VersionNumber() As Integer Implements IImageRow.VersionNumber
        Get
            Return wpmUTIL.GetDBInteger(Me.x_VersionNumber.Value) + 1
        End Get
        Set(ByVal value As Integer)
            Me.x_VersionNumber.Value = value
        End Set
    End Property

End Class
