<%@ WebHandler Language="VB" Class="FindImage" %>

Imports System
Imports System.Web
Imports System.Xml
Imports System.IO
Imports System.Net
Imports WebProjectMechanics
Imports System.Drawing.Imaging
Imports System.Drawing

Public Class FindImage : Implements IHttpHandler

    Dim SearchKeyword As String
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim mypmImage As New pmImage()
        'Read in the image filename to create a thumbnail of
        mypmImage.SourceURL = context.Request.QueryString("img")
        mypmImage.Height = context.Request.QueryString("h")
        mypmImage.Width = context.Request.QueryString("w")
        mypmImage.OverlayFont = New Drawing.Font(New Drawing.FontFamily("Arial"), 10)
        mypmImage.OverlayText = context.Request.QueryString("text")
        
        
        
        If FileProcessing.IsValidPath(context.Server.MapPath(mypmImage.SourceURL)) Then
            Dim myBitmap As System.Drawing.Bitmap = mypmImage.GetImage()
            mypmImage.AdjustSizeRatio(myBitmap)
            'Do we need to create a thumbnail?
            context.Response.ContentType = "image/jpeg"
            If mypmImage.Height > 0 And mypmImage.Width > 0 Then
                Dim newImage As System.Drawing.Bitmap = mypmImage.GenerateThumb(myBitmap, mypmImage.Width, mypmImage.Height)
                newImage.Save(context.Response.OutputStream, ImageFormat.Jpeg)
                'Clean up / Dispose...
                newImage.Dispose()
            Else
                myBitmap.Save(context.Response.OutputStream, ImageFormat.Jpeg)
            End If
            'Clean up / Dispose...
            myBitmap.Dispose()
        Else
            Using rectangleFont = New Font("Arial", 14, FontStyle.Bold)
                Using bitmap = New Bitmap(320, 110, PixelFormat.Format24bppRgb)
                    Using g = Graphics.FromImage(bitmap)
                        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
                        Dim backgroundColor = Color.Bisque
                        g.Clear(backgroundColor)
                        g.DrawString(mypmImage.SourceURL, rectangleFont, SystemBrushes.WindowText, New PointF(10, 40))
                        context.Response.ContentType = "image/png"
                        bitmap.Save(context.Response.OutputStream, ImageFormat.Png)
                    End Using
                End Using
            End Using
            
            
        End If
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class