Imports System.IO
Imports WebProjectMechanics
Imports System

Public Class admin_catalog_BrowseImages
    Inherits AdminPage
    Implements ILocationImage

    Dim myImagePresenter As LocationImagePresenter
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        myImagePresenter = New LocationImagePresenter(Me)
        Dim sRequestImageID As String = String.Empty
        Dim sRequestFolder As String = String.Empty
        ProcessRequestParameters(sRequestImageID, sRequestFolder)
        BuildThumbnailList(masterPage.myCompany.SiteImageList, sRequestImageID, sRequestFolder)
    End Sub

    Private Sub ProcessRequestParameters(ByRef sRequestImageID As String, ByRef sRequestFolder As String)
        sRequestImageID = wpm_GetProperty("ImageID", String.Empty)
        sRequestFolder = wpm_GetProperty("SubFolder", String.Empty)
        If IsPostBack Then
            sRequestFolder = x_SubFolder.Value.ToString
            If x_ImageID.Value <> "" Then
                myImagePresenter.updateMyLocationImage(x_ImageID.Value)
                Results.Text = myImagePresenter.Status
            End If
        Else
            x_SubFolder.Value = sRequestFolder
            Results.Text = "No Image To Update"
        End If
    End Sub
    Private Shared Function ValidateFolder(ByVal DirPath As String) As Boolean
        If Not FileProcessing.IsValidFolder(DirPath) Then
            FileProcessing.CreateFolder(DirPath)
        End If
        Return True
    End Function
    Private Function BuildThumbnailList(ByRef myImageRows As CompanyImageList, ByRef sReqImageID As String, ByRef sReqSubFolder As String) As Boolean
        Dim mySB As New StringBuilder("<a href=""/admin/catalog/BrowseImages.aspx"">View Folder List</a>")
        Dim myRelPath As String = String.Empty
        Dim myPath As String = String.Empty
        Dim filename As String = String.Empty
        Dim DirPath As String = Server.MapPath(wpm_SiteGallery)
        Dim I As Integer
        ValidateFolder(DirPath)
        If sReqSubFolder = String.Empty Then
            Dim files() As String = Directory.GetFiles(DirPath, "*.jpg")
            mySB.Append("<br/><strong>Base Image<strong><ul>")
            For I = 0 To files.Length - 1
                filename = Replace(files(I).ToString, Server.MapPath("/" & wpm_SiteGallery), "")
                mySB.Append(String.Format("<li><a href=""/admin/catalog/BrowseImages.aspx?ImageID={0}"">{1}</a></li>", CheckForImage(filename, myImageRows, sReqImageID), filename))
            Next
            mySB.Append("</ul><br/>")
        Else
            If sReqSubFolder.Contains("\") Then
                DirPath = Server.MapPath(String.Format("{0}\{1}", wpm_SiteGallery, sReqSubFolder))
                mySB.Append("<ul>")
                Dim myFiles() As String = Directory.GetFiles(DirPath, "*.jpg")
                For I = 0 To myFiles.Length - 1
                    filename = Replace(myFiles(I).ToString, Server.MapPath("/" & wpm_SiteGallery), "")
                    mySB.Append(String.Format("<li><a href=""{3}catalog/BrowseImages.aspx?SubFolder={0}&ImageID={1}"">{2}</a></li>", sReqSubFolder, CheckForImage(filename, myImageRows, sReqImageID), filename, wpm_SiteConfig.AdminFolder))
                Next
                mySB.Append("</ul>")
            End If
        End If
        '
        '  Code to list all sub-directories
        '
        Dim myFolders() As String = Directory.GetDirectories(DirPath)

        For y As Integer = 0 To myFolders.Length - 1
            myPath = myFolders(y).ToString
            myRelPath = Replace(myFolders(y).ToString, DirPath, "")
            If sReqSubFolder = String.Empty Then
                mySB.Append(String.Format("<hr><a href=""{1}catalog/BrowseImages.aspx?SubFolder={0}"">{0}</a>", myRelPath, wpm_SiteConfig.AdminFolder))
            Else
                If sReqSubFolder = myRelPath Then
                    mySB.Append("<ul>")
                    Dim myFiles() As String = Directory.GetFiles(myPath, "*.jpg")
                    For I = 0 To myFiles.Length - 1
                        filename = Replace(myFiles(I).ToString, Server.MapPath("/" & wpm_SiteGallery), "")
                        mySB.Append(String.Format("<li><a href=""{3}catalog/BrowseImages.aspx?SubFolder={0}&ImageID={1}"">{2}</a></li>", myRelPath, CheckForImage(filename, myImageRows, sReqImageID), filename, wpm_SiteConfig.AdminFolder))
                    Next
                    mySB.Append("</ul>")
                    Dim subFolders() As String = Directory.GetDirectories(myFolders(y).ToString)
                    For Each subFolder In subFolders
                        mySB.Append(String.Format("<hr><a href=""{1}catalog/BrowseImages.aspx?SubFolder={0}"">{0}</a>", Replace(subFolder, DirPath, ""), wpm_SiteConfig.AdminFolder))
                    Next

                End If
            End If
        Next
        pnlThumbs.Controls.Add(New Label With {.Text = mySB.ToString})
        Return True
    End Function
    Private Function CheckForImage(ByVal sImageFileName As String, ByRef myLocationImageList As CompanyImageList, ByRef sReqImageID As String) As String
        Dim sReturnID As String = ""
        For Each myImageRow As LocationImage In myLocationImageList
            If FileProcessing.CompareFileName(sImageFileName, myImageRow.ImageFileName) Then
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
                    lblImageDate.Text = myImageRow.ImageDate
                    imgMain.ImageUrl = String.Format("/runtime/catalog/FindImage.ashx?w=200&img={0}{1}", wpm_SiteGallery, myImageRow.ImageFileName)
                End If
                Exit For
            End If
        Next
        If sReturnID = "" Then
            ' Create New Image Record
            ' Create New Image Record
            Dim myNewRow As New LocationImage() With {.ImageFileName = Replace(sImageFileName, _
                                                                           "\", _
                                                                           "/"), .ImageName = sImageFileName, .ImageComment = "Image Record Created by Browse-Images", .CompanyID = wpm_CurrentSiteID, .ContactID = wpm_ContactID}
            sReturnID = myNewRow.createImage()
        End If
        Return sReturnID
    End Function
    Public Property Active() As Boolean Implements ILocationImage.Active
        Get
            Return True
        End Get
        Set(ByVal value As Boolean)
            '
        End Set
    End Property
    Public Property Color() As String Implements ILocationImage.Color
        Get
            Return String.Empty
        End Get
        Set(ByVal value As String)

        End Set
    End Property
    Public Property LocationImageContactID() As String Implements ILocationImage.ContactId
        Get
            Return wpm_ContactID
        End Get
        Set(ByVal value As String)
            ' Always User Global Contact ID
        End Set
    End Property
    Public Property ImageComment() As String Implements ILocationImage.ImageComment
        Get
            Return wpm_GetDBString(lblImageComment.Text)
        End Get
        Set(ByVal value As String)
            lblImageComment.Text = value
        End Set
    End Property
    Public Property ImageDate() As Date Implements ILocationImage.ImageDate
        Get
            Return wpm_GetDBDate(lblImageDate.Text)
        End Get
        Set(ByVal value As Date)
            lblImageDate.Text = value.ToShortDateString
        End Set
    End Property
    Public Property ImageDescription() As String Implements ILocationImage.ImageDescription
        Get
            Return wpm_GetDBString(lblImageDescription.Text)
        End Get
        Set(ByVal value As String)
            lblImageDescription.Text = value
        End Set
    End Property
    Public Property ImageFileName() As String Implements ILocationImage.ImageFileName
        Get
            Return wpm_GetDBString(lblImageFileName.Text)
        End Get
        Set(ByVal value As String)
            lblImageFileName.Text = value
        End Set
    End Property
    Public Property ImageThumbFileName() As String Implements ILocationImage.ImageThumbFileName
        Get
            Return wpm_GetDBString(lblImageFileName.Text)
        End Get
        Set(ByVal value As String)
            ''
        End Set
    End Property
    Public Property ImageID() As String Implements ILocationImage.ImageId
        Get
            Return wpm_GetDBString(x_ImageID.Value)
        End Get
        Set(ByVal value As String)
            x_ImageID.Value = value
        End Set
    End Property
    Public Property ImageName() As String Implements ILocationImage.ImageName
        Get
            Return wpm_GetDBString(lblImageName.Text)
        End Get
        Set(ByVal value As String)
            lblImageName.Text = value
        End Set
    End Property
    Public Property Medium() As String Implements ILocationImage.Medium
        Get
            Return String.Empty
        End Get
        Set(ByVal value As String)

        End Set
    End Property
    Public Property ModifiedDate() As Date Implements ILocationImage.ModifiedDate
        Get
            Return DateTime.Now()
        End Get
        Set(ByVal value As Date)
            x_modifiedDT.Value = value
        End Set
    End Property
    Public Property Price() As String Implements ILocationImage.Price
        Get
            Return String.Empty
        End Get
        Set(ByVal value As String)
        End Set
    End Property
    Public Property Size() As String Implements ILocationImage.Size
        Get
            Return String.Empty
        End Get
        Set(ByVal value As String)

        End Set
    End Property
    Public Property Sold() As Boolean Implements ILocationImage.Sold
        Get
            Return False
        End Get
        Set(ByVal value As Boolean)

        End Set
    End Property
    Public Property Subject() As String Implements ILocationImage.Subject
        Get
            Return String.Empty
        End Get
        Set(ByVal value As String)

        End Set
    End Property
    Public Property Title1() As String Implements ILocationImage.Title
        Get
            Return wpm_GetDBString(lblImageTitle.Text)
        End Get
        Set(ByVal value As String)
            lblImageTitle.Text = value
        End Set
    End Property
    Public Property VersionNumber() As Integer Implements ILocationImage.VersionNumber
        Get
            Return wpm_GetDBInteger(x_VersionNumber.Value) + 1
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

'Private Function PopulateContactDropDownList(ByVal sContactID As String, ByVal CompanyID As String, ByRef myDDL As DropDownList) As Boolean
'    Dim sqlwrk As String
'    Dim mydt As DataTable
'    sqlwrk = String.Format("SELECT ContactID, PrimaryContact & ' (' & LogonName & ')' FROM Contact where Contact.CompanyID={0} ORDER BY PrimaryContact ", CompanyID)
'    mydt = UtilityDB.GetDataTable(sqlwrk, "Browse_Image.PopulateContactDropDown")
'    For Each row As DataRow In mydt.Rows
'        Dim MyLI As New ListItem() With {.Text = wpm_GetDBString(row(1)), .Value = wpm_GetDBString(row(0))}
'        If MyLI.Value = sContactID Then
'            MyLI.Selected = True
'        End If
'        myDDL.Items.Add(MyLI)
'    Next
'    Return True
'End Function
'Private Shared Function PopulatePageDropDownList(ByVal sPageID As String, ByVal CompanyID As String, ByRef myDDL As DropDownList) As Boolean
'    Dim sqlwrk As String
'    Dim mydt As DataTable
'    sqlwrk = String.Format("SELECT PageID, PageName FROM Page where Page.CompanyID={0} ORDER BY PageName ", CompanyID)
'    mydt = UtilityDB.GetDataTable(sqlwrk, "Browse_Image.PopulatePageDropDown")
'    For Each row As DataRow In mydt.Rows
'        Dim MyLI As New ListItem() With {.Text = wpm_GetDBString(row(1)), .Value = wpm_GetDBString(row(0))}
'        If MyLI.Value = sPageID Then
'            MyLI.Selected = True
'        End If
'        myDDL.Items.Add(MyLI)
'    Next
'    Return True
'End Function