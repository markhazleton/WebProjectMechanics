<%@ WebHandler Language="VB" Class="GenericImage" %>

Imports System
Imports System.Web
Imports System.Xml
Imports System.IO
Imports System.Net
Imports WebProjectMechanics
Imports System.Drawing.Imaging
Imports System.Drawing

Public Class GenericImage : Implements IHttpHandler

    Dim SearchKeyword As String
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Using rectangleFont = New Font("Arial", 14, FontStyle.Bold)
            Using bitmap = New Bitmap(320, 110, PixelFormat.Format24bppRgb)
                Using g = Graphics.FromImage(bitmap)
                    g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
                    Dim backgroundColor = Color.Bisque
                    g.Clear(backgroundColor)
                    g.DrawString("File Not Found", rectangleFont, SystemBrushes.WindowText, New PointF(10, 40))
                    context.Response.ContentType = "image/png"
                    bitmap.Save(context.Response.OutputStream, ImageFormat.Png)
                End Using
            End Using
        End Using
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class