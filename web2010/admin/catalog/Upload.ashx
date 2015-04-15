<%@ WebHandler Language="VB" Class="Upload" %>

Imports System
Imports System.Web
Imports WebProjectMechanics
Imports System.IO

Public Class Upload : Implements IHttpHandler : Implements IRequiresSessionState
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        If context.Request.Files.Count > 0 Then
            ' Dim curCompany As New ActiveCompany
            For Each s As String In context.Request.Files
                Dim file As HttpPostedFile = context.Request.Files(s)
                If ValidateImageFile(file) Then
                    Dim fileSizeInBytes As Integer = file.ContentLength
                    Dim fileType = file.GetType
                    Dim fileName As String = file.FileName
                    Dim fileExtension As String = ".jpg"
                    If Not String.IsNullOrEmpty(fileName) Then
                        fileExtension = Path.GetExtension(fileName)
                    End If
                    Dim sPath As String = context.Server.MapPath(wpm_SiteGallery & "/upload/")
                    If Not VerifyFolderExists(sPath) Then
                        CreateFolder(sPath)
                    End If
                    Dim savedFileName As String = Path.Combine(sPath, fileName)
                    file.SaveAs(savedFileName)
                End If
            Next
        End If
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

    Public Function ValidateImageFile(ByRef myFile As HttpPostedFile) As Boolean
        Dim bReturn As Boolean = False
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
            bReturn = True
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