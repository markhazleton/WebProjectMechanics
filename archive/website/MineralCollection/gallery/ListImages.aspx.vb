Imports WebProjectMechanics
Imports System.IO
Imports System
Imports System.Text
Imports wpmMineralCollection
Imports ImageResizer

Public Class MineralCollection_ListImages
    Inherits AdminPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If wpm_GetProperty("action", String.Empty) = "delete" Then
            Dim myFile = wpm_GetProperty("ImageFileNM", String.Empty)
            If String.IsNullOrEmpty(myFile) Then
                ApplicationLogging.ErrorLog("Error - a valid file was not provided with DELETE request", "MineralCollection_ListImages.Page_Load-Delete")
            Else
                If wpm_DeleteFile(Server.MapPath(String.Format("/{0}/{1}/", wpm_SiteGallery, STR_ImageFolder)), myFile) Then
                    Response.Redirect("/MineralCollection/gallery/ListImages.aspx")
                Else
                    ApplicationLogging.ErrorLog("Error - unable to DELETE ", "MineralCollection_ListImages.Page_Load-Delete")
                End If
            End If
        End If
        ProcessSiteGallery()
    End Sub

    Public Function ProcessSiteGallery() As Boolean
        ProcessGalleryFiles(Server.MapPath(String.Format("{0}/{1}/", wpm_SiteGallery, STR_ImageFolder)), "NotAssigned")
        ProcessImageFolder()
        Return True
    End Function

    Public Function ProcessImageFolder() As Boolean
        Dim myPath As String
        Dim myRelPath As String
        If Not FileProcessing.VerifyFolderExists(Server.MapPath(String.Format("{0}/{1}/", wpm_SiteGallery, STR_ImageFolder))) Then
            FileProcessing.CreateFolder(Server.MapPath(String.Format("{0}/{1}/", wpm_SiteGallery, STR_ImageFolder)))
        End If
        Dim myFolders() As String = Directory.GetDirectories(Server.MapPath(wpm_SiteGallery.ToString & String.Format("/{0}/", STR_ImageFolder)))
        For y As Integer = 0 To myFolders.Length - 1
            myPath = myFolders(y).ToString
            myRelPath = Replace(myFolders(y).ToString, Server.MapPath(Session("SiteGallery") & String.Format("{0}/", STR_ImageFolder)), "")
            ProcessGalleryFiles(myPath, "NotAssigned")
        Next
        Return True
    End Function
    Public Function ProcessGalleryFiles(ByVal FolderPath As String, ByVal reqFilter As String) As Boolean
        Dim myFilter As String = wpm_GetProperty("Filter", reqFilter)

        Dim myReturn As Boolean = True
        Dim myImageFiles As New List(Of Object)
        Dim mySpecimenImages As New SpecimenImageList
        mySpecimenImages.GetCollectionImages(2)
        Try

            Dim myDir As DirectoryInfo = New DirectoryInfo(HttpContext.Current.Server.MapPath("/sites/nrc/images/"))
            Dim dbImages = (From o In mySpecimenImages Select o.ImageFileNM).ToList()
            Dim myColl = (From i As FileInfo In myDir.GetFiles("*.jpg") Where Not dbImages.Contains(i.Name.ToLower()))

            Select Case myFilter
                Case "Assigned"
                    myImageFiles.AddRange(From i In mySpecimenImages Where Not String.IsNullOrEmpty(i.ImageFileNM))
                Case "NotAssigned"
                    myImageFiles.AddRange(From i In myColl Select New SpecimenImage With {.ImageFileNM = i.Name, .SpecimenNumber = "Not Assigned"})
                Case Else
                    myImageFiles.AddRange(From i In mySpecimenImages Where Not String.IsNullOrEmpty(i.ImageFileNM))
                    myImageFiles.AddRange(From i In myColl Select New SpecimenImage With {.ImageFileNM = i.Name, .SpecimenNumber = "Not Assigned"})
            End Select


            Dim myTableHeader As New DisplayTableHeader() With {.TableTitle = "Images (Filter: <a href='/MineralCollection/Gallery/ListImages.aspx?Filter=ALL'>All Images</a> | <a href='/MineralCollection/Gallery/ListImages.aspx?Filter=Assigned'>Assigned</a> | <a href='/MineralCollection/Gallery/ListImages.aspx?Filter=NotAssigned'>Not Assigned</a>)",
                                                                .DetailFieldName = "ImageFileNM",
                                                                .DetailKeyName = "ImageFileNM",
                                                                .DetailPath = "/MineralCollection/Gallery/EditSpecimenImage.aspx?ImageFileNM={0}"}
            myTableHeader.AddHeaderThumbnailItem("Thumbnail",
                                            "ImageFileNM",
                                            "/MineralCollection/Gallery/EditSpecimenImage.aspx?ImageFileNM={0}",
                                            "ImageFileNM",
                                             "ThumbImageFileNM",
                                            "/sites/nrc/thumbnails/")
            myTableHeader.AddHeaderItem("ImageType", "ImageType")
            myTableHeader.AddHeaderItem("DisplayOrder", "DisplayOrder")
            myTableHeader.AddHeaderItem("Image Name", "ImageNM")
            myTableHeader.AddHeaderItem("Image Description", "ImageDS")
            myTableHeader.AddHeaderItem("Specimen #", "SpecimenNumber")
            myTableHeader.AddLinkHeaderItem("Primary Mineral", "MineralNM", "/MineralCollection/Admin.aspx?Type=Image&MineralID={0}", "PrimaryMineralID", "MineralNM")
            dtList.BuildTable(myTableHeader, myImageFiles)
        Catch ex As Exception
            ApplicationLogging.AuditLog(ex.ToString, "ProcessGalleryFiles - ListImages.aspx")
            myReturn = False
        End Try
        Return myReturn
    End Function

    'Protected Sub cmd_FileUpload_Click(sender As Object, e As EventArgs)

    '    Dim myDir As DirectoryInfo = New DirectoryInfo(HttpContext.Current.Server.MapPath("/sites/nrc/images/"))

    '    Dim myColl = (From i As FileInfo In myDir.GetFiles("*.jpg") Where i.Name.Contains("jms_") Order By i.Name Descending).ToList()
    '    Dim maxFileInt As Integer = 0

    '    If myColl.Count > 0 Then
    '        maxFileInt = wpm_GetDBInteger(myColl(0).Name.Replace("jms_", String.Empty).Replace(".jpg",String.Empty).Replace(".jpeg",String.Empty))
    '    Else
    '        maxFileInt = 1000
    '    End If
    '    Dim newFileName As String = String.Format("jms_{0}.jpg", maxFileInt+1)

    '    If FileUpload1.HasFile Then
    '        Try
    '            Dim ImageFilePath As String = Server.MapPath("/sites/nrc/images/") & newFileName
    '            FileUpload1.SaveAs(ImageFilePath)
    '            litUpload.Text = String.Format("<br/>File name: {0}<br>File Size: {1} kb<br>Content type: {2}", newFileName, FileUpload1.PostedFile.ContentLength, FileUpload1.PostedFile.ContentType)
    '            ' Create Thumbnail
    '            Dim ThumbPath As String = GenerateThumbnail(ImageFilePath)
    '        Catch ex As Exception
    '            litUpload.Text = "ERROR: " & ex.Message
    '        End Try
    '    Else
    '        litUpload.Text = "You have not specified a file."
    '    End If
    '    ProcessSiteGallery()
    'End Sub
    'Public Function GenerateThumbnail(original As String) As String
    '    Dim settings As New ImageResizer.Instructions
    '    settings.Width = 200
    '    settings.Height = 200
    '    settings.Format = "png"

    '    Dim ThumbFileNM As String = original.ToLower().Replace(".jpg", "").Replace(".jpeg", "").Replace("\images\", "\thumbnails\")
    '    Dim ThumbFile As String = ImageBuilder.Current.Build(New ImageJob(original, ThumbFileNM, settings, False, True)).FinalPath
    '    Return ThumbFile
    'End Function




End Class
