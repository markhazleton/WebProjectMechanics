<%@ WebHandler Language="VB" Class="wpm_Runtime_Image" %>

Imports System
Imports System.Web
Imports System.Drawing.Imaging
Imports System.Drawing
Imports WebProjectMechanics
Imports System.IO

Public Class wpm_Runtime_Image : Implements IHttpHandler

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim myImage As New pmImage() With {
            .SourceURL = wpm_GetProperty("img", String.Empty),
            .Height = wpm_GetIntegerProperty("h", 0),
            .Width = wpm_GetIntegerProperty("w", 0),
            .OverlayFont = New Drawing.Font(New Drawing.FontFamily("Arial"), 10),
            .OverlayText = wpm_GetStringValue(Request.QueryString("text"))}

        If Not FileProcessing.IsValidPath(HttpContext.Current.Server.MapPath(myImage.SourceURL)) Then
            myImage.OverlayText = myImage.SourceURL
            myImage.SourceURL = "/images/default.gif"
        End If

        Dim file__1 = myImage.SourceURL
        Dim fileName = file__1.Substring(file__1.LastIndexOf("/"c) + 1)
        Dim extension = file__1.Substring(file__1.LastIndexOf("."c))

        context.Response.Clear()
        context.Response.Cache.SetExpires(DateTime.Now.AddDays(10))
        context.Response.Cache.SetCacheability(HttpCacheability.[Public])
        context.Response.Cache.SetValidUntilExpires(False)
        context.Response.AddHeader("content-disposition", "inline;filename=" + myImage.SourceURL)
        
        'Using myBitmap As System.Drawing.Bitmap = myImage.GetBitmap()
        '    Response.ContentType = "image/png"
        '    myBitmap.Save(Response.OutputStream, ImageFormat.Png)
        'End Using
        Dim fullPath = HttpContext.Current.Server.MapPath(myImage.SourceURL)
         Using fs = New FileStream(fullPath , FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
            Using img = New Bitmap(fs)
                Using ms = New MemoryStream()
                    Dim bmpOut As Bitmap = Nothing

                    If myImage.width > 0 AndAlso myImage.height = 0 Then
                        Dim tmp As Double = img.Height / CDbl(img.Width)
                        bmpOut = myImage.GenerateThumb(img, myImage.width, CInt(myImage.width * tmp))
                    End If
                    If myImage.height > 0 AndAlso myImage.width = 0 Then
                        Dim tmp As Double = img.Width / CDbl(img.Height)
                        bmpOut = myImage.GenerateThumb(img, CInt(myImage.height * tmp), myImage.height)
                    End If
                    If myImage.height > 0 AndAlso myImage.width > 0 Then
                        bmpOut = myImage.GenerateThumb(img, myImage.width, myImage.height)
                    End If
                    If myImage.height = 0 AndAlso myImage.width = 0 Then
                        bmpOut = myImage.GenerateThumb(img, img.Width, img.Height)
                    End If
                    If myImage.GetContentType(extension) = "image/jpeg" Then
                        Dim info = ImageCodecInfo.GetImageEncoders()
                        Dim encoderParameters = New EncoderParameters(1)
                        encoderParameters.Param(0) = New EncoderParameter(Encoder.Quality, 100L)
                        If bmpOut IsNot Nothing Then
                            bmpOut.Save(ms, info(1), encoderParameters)
                        End If
                    ElseIf bmpOut IsNot Nothing Then
                        bmpOut.Save(ms, myImage.GetImageFormat(extension))
                    End If
                    Dim arrImg = New Byte(CInt(ms.Length - 1)) {}
                    ms.Position = 0
                    ms.Read(arrImg, 0, CInt(ms.Length))
                    img.Dispose()
                    context.Response.ContentType = myImage.GetContentType(extension)
                    context.Response.BinaryWrite(arrImg)
                    context.Response.[End]()
                End Using
            End Using
        End Using
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

    Private ReadOnly Property Request() As HttpRequest
        Get
            Return HttpContext.Current.Request
        End Get
    End Property
    Private ReadOnly Property Response As HttpResponse
        Get
            Return HttpContext.Current.Response
        End Get
    End Property
    Private ReadOnly Property Server As HttpServerUtility
        Get
            Return HttpContext.Current.Server
        End Get
    End Property
    

End Class