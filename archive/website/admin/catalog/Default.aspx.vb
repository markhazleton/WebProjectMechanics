Imports WebProjectMechanics
Imports ImageResizer
Imports System.IO

Partial Class admin_catalog_Default
    Inherits AdminPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If wpm_GetProperty("action", String.Empty) = "delete" Then
            Dim myFile = wpm_GetProperty("ImageFileNM", String.Empty)
            If String.IsNullOrEmpty(myFile) Then
                ApplicationLogging.ErrorLog("Error - a valid file was not provided with DELETE request", "Admin_Catalog_Default.Page_Load-Delete")
            Else
                If wpm_DeleteFile(Server.MapPath(String.Format("/{0}/{1}/", wpm_SiteGallery, STR_ImageFolder)), myFile) Then
                    Response.Redirect("/admin/catalog/default.aspx")
                Else
                    ApplicationLogging.ErrorLog("Error - unable to DELETE ", "Admin_Catalog_Default.Page_Load-Delete")
                End If
            End If
        End If
        ProcessSiteGallery()
    End Sub

    Public Function ProcessSiteGallery() As Boolean

        ' ProcessGalleryFiles(Server.MapPath(String.Format("{0}/{1}/", wpm_SiteGallery, STR_ImageFolder)), "NotAssigned")

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
        Dim mySpecimenImages As New WebProjectMechanics.CompanyImageList(wpm_CurrentSiteID)
        
        Try
            Dim myDir As DirectoryInfo = New DirectoryInfo(HttpContext.Current.Server.MapPath(String.Format("/{0}/{1}/", wpm_SiteGallery, STR_ImageFolder)))
            Dim dbImages = (From o In mySpecimenImages ).ToList()
            Dim myColl = (From i As FileInfo In myDir.GetFiles("*.jpg") )

            Select Case myFilter
                Case "Assigned"
                    myImageFiles.AddRange(From i In mySpecimenImages )
                Case "NotAssigned"
                    myImageFiles.AddRange((From i In myColl Select New LocationImage With {.ImageFileName = i.Name,
                                                                                          .ImageDescription = i.Name}).ToList)
                Case Else
                    myImageFiles.AddRange(From i In mySpecimenImages )
                    myImageFiles.AddRange((From i In myColl Select New LocationImage With {.ImageFileName = i.Name,
                                                                                          .ImageDescription = i.Name}).ToList)
            End Select

            Dim myTableHeader As New DisplayTableHeader() With {.TableTitle = "Images (Filter: <a href='//admin/catalog/default.aspx?Filter=ALL'>All Images</a> | <a href='/admin/catalog/default.aspx?Filter=Assigned'>Assigned</a> | <a href='/admin/catalog/default.aspx?Filter=NotAssigned'>Not Assigned</a>)",
                                                                .DetailFieldName = "ImageFileName",
                                                                .DetailKeyName = "ImageFileName",
                                                                .DetailPath = "/admin/catalog/default.aspx?ImageFileNM={0}"}
            '  myTableHeader.AddLinkHeaderItem("Thumbnail", "ImageFileName", "/admin/catalog/default.aspx?ImageFileName={0}", "ImageFileName", "ImageFileName")
            myTableHeader.AddHeaderItem("Image Name", "ImageName")
            myTableHeader.AddHeaderItem("Image ID", "ImageID")
            myTableHeader.AddHeaderItem("Image Description", "ImageDescription")
            dtList.BuildTable(myTableHeader, myImageFiles)
        Catch ex As Exception
            ApplicationLogging.AuditLog(ex.ToString, "ProcessGalleryFiles - ListImages.aspx")
            myReturn = False
        End Try
        Return myReturn
    End Function

    Protected Sub cmd_FileUpload_Click(sender As Object, e As EventArgs)
        If FileUpload1.HasFile Then
            Try
                Dim ImageFilePath As String = Server.MapPath(String.Format("/{0}/{1}/", wpm_SiteGallery, STR_ImageFolder)) & FileUpload1.FileName
                FileUpload1.SaveAs(ImageFilePath)
                litUpload.Text = String.Format("<br/>File name: {0}<br>File Size: {1} kb<br>Content type: {2}", FileUpload1.PostedFile.FileName, FileUpload1.PostedFile.ContentLength, FileUpload1.PostedFile.ContentType)
                ' Create Thumbnail
                Dim ThumbPath As String = GenerateThumbnail(ImageFilePath)
            Catch ex As Exception
                litUpload.Text = "ERROR: " & ex.Message
            End Try
        Else
            litUpload.Text = "You have not specified a file."
        End If
        ProcessSiteGallery()
    End Sub
    Public Function GenerateThumbnail(original As String) As String
        'Dim settings As New ImageResizer.Instructions With {
        '    .Width = 200,
        '    .Height = 200,
        '    .Format = "png"
        '}
        'Dim ThumbPath As String = String.Empty

        'If FileUpload1.PostedFile.ContentType.Contains("jpeg") Then
        '    ThumbPath = FileUpload1.PostedFile.FileName.Replace(".jpg", "").Replace(".jpeg", "")
        'End If
        'ThumbPath = HttpContext.Current.Server.MapPath("/sites/nrc/thumbnails/") & ThumbPath
        'Dim ThumbFile As String = ImageBuilder.Current.Build(New ImageJob(original, ThumbPath, settings, False, True)).FinalPath
        'Return ThumbFile
        Return original
    End Function


End Class
