<%@ Import Namespace="System.Drawing.Imaging" %>
<%@ Import Namespace="System.Drawing" %>
<%@ Import Namespace="WebProjectMechanics" %>
<script language="VB" runat="server">
    Function ThumbnailCallback() As Boolean
        Return False
    End Function
    Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
        Dim mypmImage As New pmImage()
        
        'Read in the image filename to create a thumbnail of
        mypmImage.SourceURL = Request.QueryString("img")
        mypmImage.Height = Request.QueryString("h")
        mypmImage.Width = Request.QueryString("w")
        mypmImage.OverlayFont = New Drawing.Font(New Drawing.FontFamily("Arial"), 10)
        mypmImage.OverlayText = Request.QueryString("text")
        
        If FileProcessing.IsValidPath(Server.MapPath(mypmImage.SourceURL)) Then
            Dim myBitmap As System.Drawing.Bitmap = mypmImage.GetImage()
            mypmImage.AdjustSizeRatio(myBitmap)
           
            'Do we need to create a thumbnail?
            Response.ContentType = "image/jpeg"
            
            If mypmImage.Height > 0 And mypmImage.Width > 0 Then
                Dim newImage As System.Drawing.Bitmap = mypmImage.GenerateThumb(myBitmap, mypmImage.Width, mypmImage.Height)
                newImage.Save(Response.OutputStream, ImageFormat.Jpeg)
                'Clean up / Dispose...
                newImage.Dispose()
            Else
                myBitmap.Save(Response.OutputStream, ImageFormat.Jpeg)
            End If
            'Clean up / Dispose...
            myBitmap.Dispose()
        Else
            Response.ContentType = "image/jpeg"
            Dim sText As String = Request.QueryString("img")
            sText = Right(sText, Len(sText) - sText.LastIndexOf("/") - 1)
            Using rectangleFont = New Font("Arial", 14, FontStyle.Bold)
                Using bitmap = New Bitmap(320, 110, PixelFormat.Format24bppRgb)
                    Using g = Graphics.FromImage(bitmap)
                        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
                        Dim backgroundColor = Color.Bisque
                        g.Clear(backgroundColor)
                        g.DrawString(sText, rectangleFont, SystemBrushes.WindowText, New PointF(10, 40))
                        Context.Response.ContentType = "image/png"
                        bitmap.Save(Context.Response.OutputStream, ImageFormat.Png)
                    End Using
                End Using
            End Using
        End If
    End Sub
    
</script>
