<%@ WebHandler Language="VB" Class="GalleryUpload" %>

Imports System
Imports System.Web
Imports WebProjectMechanics
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Imaging

Public Class GalleryUpload : Implements IHttpHandler : Implements IRequiresSessionState
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Try
            If context.Request.Files.Count > 0 Then
                ' Dim curCompany As New ActiveCompany
                For Each s As String In context.Request.Files
                    Dim file As HttpPostedFile = context.Request.Files(s)
                    
                    ApplicationLogging.AuditLog("MineralCollection.Gallery.Upload.ashx", String.Format("Received File {0}", file.FileName))
                    
                    If ValidateImageFile(file) Then
                        Dim fileSizeInBytes As Integer = file.ContentLength
                        Dim fileType = file.GetType
                        Dim fileName As String = file.FileName
                        Dim fileExtension As String = ".jpg"
                        If Not String.IsNullOrEmpty(fileName) Then
                            fileExtension = Path.GetExtension(fileName)
                        End If
                    
                        Dim myDir As DirectoryInfo = New DirectoryInfo(HttpContext.Current.Server.MapPath("/sites/nrc/images/"))
                        Dim maxFileInt As Integer = 0
                        Try
                            Dim myColl = (From i As FileInfo In myDir.GetFiles("*.jpg") Where i.Name.Contains("jms_") Order By i.Name Descending).ToList()
                            If myColl.Count > 0 Then
                                maxFileInt = wpm_GetDBInteger(myColl(0).Name.Replace("jms_", String.Empty).Replace(".jpg", String.Empty).Replace(".jpeg", String.Empty))
                            Else
                                maxFileInt = 1000
                            End If
                        Catch ex As Exception
                            ApplicationLogging.AuditLog("MineralCollection.Gallery.Upload.ashx - Get Max File Int", ex.Message)
                        End Try

                        Dim newFileName As String = String.Format("jms_{0}.jpg", maxFileInt + 1)
                        Dim sPath As String = context.Server.MapPath("/sites/nrc/images/")
                        If Not VerifyFolderExists(sPath) Then
                            CreateFolder(sPath)
                            ApplicationLogging.AuditLog("MineralCollection.Gallery.Upload.ashx - Create Folder", String.Format("Folder Name: {0}", sPath))
                        End If
                        
                        If wpm_FileExists(sPath, newFileName) Then
                            newFileName = newFileName.Replace(".jpg", "_" & wpm_FormatHour() & ".jpg")
                            ApplicationLogging.AuditLog("MineralCollection.Gallery.Upload.ashx - File Name Already Exists", String.Format("New File Name: {0}", newFileName))
                        End If
                        
                        Dim savedFileName As String = Path.Combine(sPath, newFileName)
                        Try
                            file.SaveAs(savedFileName)
                            Dim myGenThumbnail = GenerateThumbnail(savedFileName)
                        Catch ex As Exception
                            ApplicationLogging.AuditLog(String.Format("MineralCollection.Gallery.Upload.ashx - file.SaveAS({0}) exception!", savedFileName), ex.Message)
                        End Try
                       
                        
                        If wpm_GetDBInteger(context.Session("CollectionItemID"), 0) > 0 Then
                            Using myCon As New wpmMineralCollection.DataController()
                                Dim ImageExists = (From i In myCon.CollectionItemImages Where i.ImageFileNM = newFileName).ToList()
                                Dim CurrentSpecimenImages = (From i In myCon.CollectionItemImages Where i.CollectionItemID = wpm_GetDBInteger(context.Session("CollectionItemID"), 0) Select i).ToList()
                                
                                If ImageExists.Count = 0 Then
                                    Dim myImage As New wpmMineralCollection.CollectionItemImage
                                    With myImage
                                        .ImageFileNM = newFileName
                                        .CollectionItemID = wpm_GetDBInteger(context.Session("CollectionItemID"), 0)
                                        .ImageNM = newFileName
                                        .ImageType = "Photo"
                                        .DisplayOrder = CurrentSpecimenImages.Count + 1
                                        .ImageDS = String.Format("Source File:{0} on {1}", file.FileName, wpm_FormatDate(Now(), "m d, Y"))
                                        .ModifiedDT = Now()
                                        .ModifiedID = wpm_GetDBInteger(context.Session("ContactID"), 999)
                                    End With
                                    myCon.CollectionItemImages.InsertOnSubmit(myImage)
                                    myCon.SubmitChanges()
                                Else
                                    ApplicationLogging.ErrorLog("MineralCollection.Gallery.Upload.ashx - ProcessRequest", String.Format("Duplicate FileNM for {0}", newFileName))
                                End If
                            End Using
                        End If
                    ApplicationLogging.AuditLog("MineralCollection.Gallery.Upload.ashx", String.Format("Processed File Successfully: {0}", file.FileName))
                    End If
                Next
            End If
        Catch ex As Exception
            ApplicationLogging.ErrorLog("MineralCollection.Gallery.Upload.ashx - ProcessRequest", ex.ToString)
        End Try
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

    Public Function GenerateThumbnail(original As String) As String
        Try
            Dim ThumbFileNM As String = original.ToLower().Replace(".jpg", ".png").Replace(".jpeg", ".png").Replace("\images\", "\thumbnails\")
            Dim thumbDirectory As String = Path.GetDirectoryName(ThumbFileNM)

            If Not String.IsNullOrEmpty(thumbDirectory) AndAlso Not Directory.Exists(thumbDirectory) Then
                Directory.CreateDirectory(thumbDirectory)
            End If

            Using originalImage As Image = Image.FromFile(original)
                Dim thumbnailWidth As Integer = 200
                Dim thumbnailHeight As Integer = 200

                ' Calculate aspect ratio
                Dim ratioX As Double = CDbl(thumbnailWidth) / originalImage.Width
                Dim ratioY As Double = CDbl(thumbnailHeight) / originalImage.Height
                Dim ratio As Double = Math.Min(ratioX, ratioY)

                Dim newWidth As Integer = CInt(originalImage.Width * ratio)
                Dim newHeight As Integer = CInt(originalImage.Height * ratio)

                Using thumbnail As New Bitmap(newWidth, newHeight)
                    Using g As Graphics = Graphics.FromImage(thumbnail)
                        g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                        g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
                        g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
                        g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
                        g.DrawImage(originalImage, 0, 0, newWidth, newHeight)
                    End Using

                    thumbnail.Save(ThumbFileNM, ImageFormat.Png)
                End Using
            End Using

            Return ThumbFileNM
        Catch ex As Exception
            ApplicationLogging.ErrorLog("MineralCollection.Gallery.Upload.ashx - GenerateThumbnail", ex.ToString)
            Return original
        End Try
    End Function
    
    Public Function ValidateImageFile(ByRef myFile As HttpPostedFile) As Boolean
        Dim bReturn As Boolean = True
        ' DICTIONARY OF ALL IMAGE FILE HEADER
        Dim imageHeader As New Dictionary(Of String, Byte())()
        imageHeader.Add("JPG", New Byte() {&HFF, &HD8, &HFF, &HE0})
        imageHeader.Add("JPEG", New Byte() {&HFF, &HD8, &HFF, &HE0})
        imageHeader.Add("PNG", New Byte() {&H89, &H50, &H4E, &H47})
        imageHeader.Add("TIF", New Byte() {&H49, &H49, &H2A, &H0})
        imageHeader.Add("TIFF", New Byte() {&H49, &H49, &H2A, &H0})
        imageHeader.Add("GIF", New Byte() {&H47, &H49, &H46, &H38})
        imageHeader.Add("BMP", New Byte() {&H42, &H4D})
        imageHeader.Add("ICO", New Byte() {&H0, &H0, &H1, &H0})

        Dim header As Byte()
        ' GET FILE EXTENSION
        Dim fileExt As String = myFile.FileName.Substring(myFile.FileName.LastIndexOf("."c) + 1).ToUpper()
        ' CUSTOM VALIDATION GOES HERE BASED ON FILE EXTENSION IF ANY
        Dim tmp As Byte() = imageHeader(fileExt)
        header = New Byte(tmp.Length - 1) {}
        ' GET HEADER INFORMATION OF UPLOADED FILE
        myFile.InputStream.Read(header, 0, header.Length)
        If CompareArray(tmp, header) Then
            ApplicationLogging.AuditLog("MineralCollection.Gallery.Upload.ashx", String.Format("ValidateImageFile PASSED {0} temp={1}, header={2}", myFile.FileName,tmp.Length, header.Length))
            bReturn = True
        Else
            ApplicationLogging.AuditLog("MineralCollection.Gallery.Upload.ashx", String.Format("ValidateImageFile FAILED {0} temp={1}, header={2}", myFile.FileName,tmp.Length, header.Length))
        End If
        Return bReturn
    End Function
    Private Function CompareArray(a1 As Byte(), a2 As Byte()) As Boolean
        If a1.Length <> a2.Length Then
            Return False
        End If
        For i As Integer = 0 To a1.Length - 1
            If a1(i) <> a2(i) Then
                Return False
            End If
        Next
        Return True
    End Function
   
End Class