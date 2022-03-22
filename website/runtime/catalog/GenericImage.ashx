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

    Public Shared Function TruncateAtWord(value As String, length As Integer) As String
        If value Is Nothing OrElse value.Length < length OrElse value.IndexOf(" ", length) = -1 Then
            Return value
        End If
        Return value.Substring(0, value.IndexOf(" ", length))
    End Function

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        Dim sText As String = context.Request.QueryString("Name")
        Dim fontSize As Integer = 8
        If Len(sText) > 25 Then
            sText = TruncateAtWord(sText, 35)
            fontSize = 8
        End If
        Using rectangleFont = New Font("Comic Sans", fontSize, FontStyle.Bold)
            Using bitmap = New Bitmap(300, 200, PixelFormat.Format24bppRgb)
                Using g = Graphics.FromImage(bitmap)
                    g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
                    Dim backgroundColor = Color.AliceBlue
                    g.Clear(backgroundColor)
                    g.DrawString(sText, rectangleFont, SystemBrushes.WindowText, New PointF(40, 80))
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