Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO

Public Class pmImageNew
    Implements IDisposable

    Public Property SourceURL As String
    Public Property OverlayText As String
    Public Property Width As Integer
    Public Property Height As Integer
    Public Property OverlayFont As Font
    Public Property OverlayColor As Color
    Public Property bAddShadow As Boolean = False
    Public Property bAddAlpha As Boolean = False
    Public Property Position As ContentAlignment = ContentAlignment.TopCenter
    Public Property PercentFill As Single = 0.8!

    Public Sub Dispose() Implements IDisposable.Dispose
        If OverlayFont IsNot Nothing Then
            OverlayFont.Dispose()
            OverlayFont = Nothing
        End If
    End Sub
    Sub New()
        OverlayColor = Color.Black
        OverlayFont = New Font(New FontFamily("Arial"), 20)
        OverlayText = String.Empty
    End Sub

    Public Function GetImage() As Bitmap
        If FileProcessing.IsValidPath(HttpContext.Current.Server.MapPath(SourceURL)) Then
            Dim extension = SourceURL.Substring(SourceURL.LastIndexOf("."c))

            If String.IsNullOrEmpty(OverlayText) Then
                Using fs = New FileStream(HttpContext.Current.Server.MapPath(SourceURL), FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                    Using img = New Bitmap(fs)
                        Return GenerateThumb(img, Width, Height)
                    End Using
                End Using
            Else
                Return VBOverlay(Image.FromFile(HttpContext.Current.Server.MapPath(SourceURL)))
            End If
        Else
            If Width < 10 then
                Width = 150
            End If
            If Height < 10 then
                Height = 150
            End If
            Dim rectangleFont = New Font("Arial", 14, FontStyle.Bold)
            Dim bitmap = New Bitmap(Width, Height, PixelFormat.Format24bppRgb)
            Using g = Graphics.FromImage(bitmap)
                g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
                Dim backgroundColor = Color.Bisque
                g.Clear(backgroundColor)
                g.DrawString(Right(SourceURL, Len(SourceURL) - SourceURL.LastIndexOf("/") - 1), rectangleFont, SystemBrushes.WindowText, New PointF(10, 40))
            End Using
            Return VBOverlay(bitmap)
        End If
    End Function

    Public Sub ProcessRequest(context As HttpContext)
        Dim file__1 = context.Request.FilePath.Replace(".ashx", [String].Empty)
        Dim fileName = file__1.Substring(file__1.LastIndexOf("/"c) + 1)
        Dim extension = file__1.Substring(file__1.LastIndexOf("."c))
        Dim width As Integer
        Dim height As Integer
        Integer.TryParse(context.Request("w"), width)
        Integer.TryParse(context.Request("h"), height)
        Dim path = context.Server.MapPath(file__1)
        If Not File.Exists(path) Then
            context.Response.StatusCode = 404
            context.Response.[End]()
            Return
        End If
        context.Response.Clear()
        context.Response.Cache.SetExpires(DateTime.Now.AddDays(10))
        context.Response.Cache.SetCacheability(HttpCacheability.[Public])
        context.Response.Cache.SetValidUntilExpires(False)
        context.Response.AddHeader("content-disposition", "inline;filename=" + fileName)

        Using fs = New FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
            Using img = New Bitmap(fs)
                Using ms = New MemoryStream()
                    Dim bmpOut As Bitmap = Nothing

                    If width > 0 AndAlso height = 0 Then
                        Dim tmp As Double = img.Height / CDbl(img.Width)
                        bmpOut = GenerateThumb(img, width, CInt(width * tmp))
                    End If
                    If height > 0 AndAlso width = 0 Then
                        Dim tmp As Double = img.Width / CDbl(img.Height)
                        bmpOut = GenerateThumb(img, CInt(height * tmp), height)
                    End If
                    If height > 0 AndAlso width > 0 Then
                        bmpOut = GenerateThumb(img, width, height)
                    End If
                    If height = 0 AndAlso width = 0 Then
                        bmpOut = GenerateThumb(img, img.Width, img.Height)
                    End If
                    If GetContentType(extension) = "image/jpeg" Then
                        Dim info = ImageCodecInfo.GetImageEncoders()
                        Dim encoderParameters = New EncoderParameters(1)
                        encoderParameters.Param(0) = New EncoderParameter(Encoder.Quality, 100L)
                        If bmpOut IsNot Nothing Then
                            bmpOut.Save(ms, info(1), encoderParameters)
                        End If
                    ElseIf bmpOut IsNot Nothing Then
                        bmpOut.Save(ms, GetImageFormat(extension))
                    End If
                    Dim arrImg = New Byte(CInt(ms.Length - 1)) {}
                    ms.Position = 0
                    ms.Read(arrImg, 0, CInt(ms.Length))
                    img.Dispose()
                    context.Response.ContentType = GetContentType(extension)
                    context.Response.BinaryWrite(arrImg)
                    context.Response.[End]()
                End Using
            End Using
        End Using
    End Sub
    Public Function GenerateThumb(bmp As Bitmap, width As Integer, height As Integer) As Bitmap
        Dim result As Bitmap = Nothing
        Try
            Dim sourceWidth = bmp.Width
            Dim sourceHeight = bmp.Height
            If width > sourceWidth Then
                width = sourceWidth
            End If
            If height > sourceHeight Then
                height = sourceHeight
            End If
            Dim widthPercent = (width / CSng(sourceWidth))
            Dim heightPercent = (height / CSng(sourceHeight))
            Dim finalPercent = If(heightPercent < widthPercent, widthPercent, heightPercent)
            If finalPercent = 1 Then
                Return bmp
            ElseIf finalPercent = 0 Then
                Return bmp
            Else
                Dim destWidth = CInt(sourceWidth * finalPercent)
                Dim destHeight = CInt(sourceHeight * finalPercent)

                result = New Bitmap(destWidth, destHeight, PixelFormat.Format24bppRgb)
                result.SetResolution(bmp.HorizontalResolution, bmp.VerticalResolution)
                Dim graphics__1 = Graphics.FromImage(result)
                graphics__1.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic
                graphics__1.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality
                graphics__1.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality
                graphics__1.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality
                graphics__1.FillRectangle(Brushes.White, 0, 0, destWidth, destHeight)
                graphics__1.DrawImage(bmp, New Rectangle(0, 0, destWidth, destHeight), New Rectangle(0, 0, sourceWidth, sourceHeight), GraphicsUnit.Pixel)
                bmp.Dispose()
                graphics__1.Dispose()
                Return result
            End If
        Catch ex As Exception
            ApplicationLogging.ErrorLog(ex.InnerException.ToString, "Images.ProcessRequest")
            Return bmp
        End Try
    End Function
    Public Function Resize(ByVal ImageFileName As String, ByVal scale_factor As Single) As Image
        ' Get the source bitmap.
        Dim bm_source As New Bitmap(ImageFileName)
        ' Make a bitmap for the result.
        Dim bm_dest As New Bitmap( _
            CInt(bm_source.Width * scale_factor), _
            CInt(bm_source.Height * scale_factor))
        ' Make a Graphics object for the result Bitmap.
        Dim gr_dest As Graphics = Graphics.FromImage(bm_dest)
        ' Copy the source image into the destination bitmap.
        gr_dest.DrawImage(bm_source, 0, 0, _
            bm_dest.Width + 1, _
            bm_dest.Height + 1)
        Return Nothing
    End Function
    Public Function OverlayTextImage(oImage As Image, text As String) As Bitmap
        Dim oBitmap As New Bitmap(oImage.Width, oImage.Height, PixelFormat.Format24bppRgb)
        Dim oGraphics As Graphics = Graphics.FromImage(oBitmap)
        oGraphics.DrawImage(oImage, 0, 0)
        Dim f As New Font(New FontFamily("Arial"), 10)
        Dim oBrush As New SolidBrush(Color.FromArgb(255, ColorTranslator.FromHtml("#000000")))
        Dim rect As New RectangleF(10, oImage.Height - 150, oImage.Width - 50, 100)
        Dim strFormat As StringFormat = StringFormat.GenericTypographic
        oGraphics.DrawString(text, f, oBrush, rect, strFormat)
        oGraphics.Dispose()
        oBrush.Dispose()
        f.Dispose()
        Return oBitmap
    End Function

    Private Function GetImageFormat(ext As String) As ImageFormat
        Select Case ext.ToLower()
            Case ".gif"
                Return ImageFormat.Gif
            Case ".jpg", ".jpeg"
                Return ImageFormat.Jpeg
            Case ".png"
                Return ImageFormat.Png
            Case ".bmp"
                Return ImageFormat.Bmp
            Case Else
                Throw New NotSupportedException("Invalid Image Format")
        End Select
    End Function
    Private Function GetContentType(ext As String) As String
        Select Case ext.ToLower()
            Case ".gif"
                Return "image/gif"
            Case ".jpg", ".jpeg"
                Return "image/jpeg"
            Case ".png"
                Return "image/png"
            Case ".bmp"
                Return "image/bmp"
            Case Else
                Throw New NotSupportedException("Invalid Image Format")
        End Select
    End Function
    Public ReadOnly Property IsReusable() As Boolean
        Get
            Return True
        End Get
    End Property

    ' Draw text directly onto an image (scaled for best-fit)
    Public Function VBOverlay(ByVal img As Image) As Bitmap
        If OverlayText > String.Empty AndAlso PercentFill > 0 Then
            ' create bitmap and graphics used for drawing
            ' "clone" image but use 24RGB format
            Dim bmp As New Bitmap(img.Width, img.Height, PixelFormat.Format24bppRgb)
            Dim g As Graphics = Graphics.FromImage(bmp)
            g.DrawImage(img, 0, 0)
            Dim alpha As Integer = 255
            If bAddAlpha Then
                ' Compute transparency: Longer text should be less transparent or it gets lost.
                alpha = 90 + (OverlayText.Length * 2)
                If alpha >= 255 Then alpha = 255
            End If
            ' Create the brush based on the color and alpha
            Dim b As New SolidBrush(Color.FromArgb(alpha, OverlayColor))

            ' Measure the text to render (unscaled, unwrapped)
            Dim strFormat As StringFormat = StringFormat.GenericTypographic
            Dim s As SizeF = g.MeasureString(OverlayText, OverlayFont, 100000, strFormat)

            ' Enlarge font to specified fill (estimated by AREA)
            Dim zoom As Single = CSng(Math.Sqrt((CDbl(img.Width * img.Height) * PercentFill) / CDbl(s.Width * s.Height)))
            Dim sty As FontStyle = OverlayFont.Style
            Dim oldFont As New Font(OverlayFont.FontFamily, CSng(OverlayFont.Size) * zoom, sty)

            ' Measure using new font size, allow to wrap as needed.
            Dim charFit, linesFit As Integer
            Dim SQRTFill As Single = CSng(Math.Sqrt(PercentFill))
            strFormat.FormatFlags = StringFormatFlags.NoClip 'Or StringFormatFlags.LineLimit 'Or StringFormatFlags.MeasureTrailingSpaces
            strFormat.Trimming = StringTrimming.Word
            Dim layout As New SizeF(CSng(img.Width) * SQRTFill, CSng(img.Height) * 1.5!) ' fit to width, allow height to go over
            s = g.MeasureString(OverlayText, oldFont, layout, strFormat, charFit, linesFit)

            ' Reduce size until it actually fits...
            ' Most text only has to be reduced 1 or 2 times.
            Do Until s.Height <= CSng(img.Height) * SQRTFill AndAlso s.Width <= layout.Width
                zoom = Math.Max(s.Height / (CSng(img.Height) * SQRTFill), s.Width / layout.Width)
                zoom = oldFont.Size / zoom
                If zoom > 16 Then zoom = CSng(Math.Floor(zoom)) ' use a whole number to reduce "jaggies"
                If zoom >= oldFont.Size Then zoom -= 1
                oldFont = New Font(OverlayFont.FontFamily, zoom, sty)
                s = g.MeasureString(OverlayText, oldFont, layout, strFormat, charFit, linesFit)
                If zoom <= 1 Then Exit Do ' bail
            Loop

            ' Dim f As Font = FindFont(g, OverlayText, New Size(Width, Height), OverlayFont)
            Dim f As Font = oldFont

            ' Determine draw area based on placement
            Dim rect As RectangleF
            Select Case Position
                Case ContentAlignment.TopLeft ' =1
                    rect = New RectangleF(f.Size * 0.15!, _
                        (f.Size * 0.1!), _
                        layout.Width, _
                        CSng(img.Height) * SQRTFill)
                Case ContentAlignment.TopCenter ' =2
                    rect = New RectangleF((bmp.Width - s.Width) / 2, _
                        (f.Size * 0.1!), _
                        layout.Width, _
                        CSng(img.Height) * SQRTFill)
                Case ContentAlignment.TopRight ' =4
                    rect = New RectangleF((bmp.Width - s.Width) - (f.Size * 0.1!), _
                        (f.Size * 0.1!), _
                        layout.Width, _
                        CSng(img.Height) * SQRTFill)
                Case ContentAlignment.MiddleLeft ' =16  huh?  where's 8?
                    rect = New RectangleF(f.Size * 0.15!, _
                        (bmp.Height - s.Height) / 2, _
                        layout.Width, _
                        CSng(img.Height) * SQRTFill)
                Case ContentAlignment.MiddleCenter ' =32
                    rect = New RectangleF((bmp.Width - s.Width) / 2, _
                        (bmp.Height - s.Height) / 2, _
                        layout.Width, _
                        CSng(img.Height) * SQRTFill)
                Case ContentAlignment.MiddleRight '=64
                    rect = New RectangleF((bmp.Width - s.Width) - (f.Size * 0.1!), _
                        (bmp.Height - s.Height) / 2, _
                        layout.Width, _
                        CSng(img.Height) * SQRTFill)
                Case ContentAlignment.BottomLeft ' =256  and 128?
                    rect = New RectangleF(f.Size * 0.15!, _
                        (bmp.Height - s.Height) - (f.Size * 0.1!), _
                        layout.Width, _
                        CSng(img.Height) * SQRTFill)
                Case ContentAlignment.BottomCenter ' =512
                    rect = New RectangleF((bmp.Width - s.Width) / 2, _
                        (bmp.Height - s.Height) - (f.Size * 0.1!), _
                        layout.Width, _
                        CSng(img.Height) * SQRTFill)
                Case ContentAlignment.BottomRight ' =1024
                    rect = New RectangleF((bmp.Width - s.Width) - (f.Size * 0.1!), _
                        (bmp.Height - s.Height) - (f.Size * 0.1!), _
                        layout.Width, _
                        CSng(img.Height) * SQRTFill)
            End Select

            ' Add rendering hint (thx to Thomas)
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias

            If bAddShadow Then
                ' Add "drop shadow" at half transparency and offset by 1/10 font size
                Dim shadow As New SolidBrush(Color.FromArgb(CInt(alpha / 2), OverlayColor))
                Dim sRect As New RectangleF(rect.X - (f.Size * 0.1!), rect.Y - (f.Size * 0.1!), rect.Width, rect.Height)
                g.DrawString(OverlayText, f, shadow, sRect, strFormat)
            End If

            ' Finally, draw centered text!
            g.DrawString(OverlayText, f, b, rect, strFormat)

            ' clean-up
            g.Dispose()
            b.Dispose()
            f.Dispose()
            Return bmp
        Else
            ' nothing to overlay!  regurgitate image
            Return New Bitmap(img)
        End If
    End Function

    'This function checks the room size and your text and appropriate font for your text to fit in room
    'PreferedFont is the Font that you wish to apply
    'Room is your space in which your text should be in.
    'LongString is the string which it's bounds is more than room bounds.
    Public Function FindFont(g As Graphics, longString As String, Room As Size, PreferedFont As Font) As Font
        'you should perform some scale functions!!!
        Dim RealSize As SizeF = g.MeasureString(longString, PreferedFont)
        Dim HeightScaleRatio As Single = Room.Height / RealSize.Height
        Dim WidthScaleRatio As Single = Room.Width / RealSize.Width

        Dim ScaleRatio As Single
        If (HeightScaleRatio < WidthScaleRatio) Then
            ScaleRatio = WidthScaleRatio
        Else
            ScaleRatio = HeightScaleRatio
        End If

        Dim ScaleFontSize As Single = PreferedFont.Size * ScaleRatio
        Return New Font(PreferedFont.FontFamily, ScaleFontSize)
    End Function

    Public Function CombineImageText(oImage As Image) As Bitmap
        Dim yStart As Integer = 10
        Dim xStart As Integer = 10
        Dim oBitmap As New Bitmap(oImage.Width, oImage.Height, PixelFormat.Format24bppRgb)
        Dim oGraphics As Graphics = Graphics.FromImage(oBitmap)
        oGraphics.DrawImage(oImage, 0, 0)

        Dim f As New Font(New FontFamily("Arial"), 20)
        yStart = CInt(Math.Round(oImage.Height / 2.5, 0))
        xStart = CInt(Math.Round(oImage.Width / 2.5, 0))

        Dim oBrush As New SolidBrush(Color.FromArgb(255, ColorTranslator.FromHtml("#000000")))
        Dim rect As New RectangleF(xStart, yStart, oImage.Width, oImage.Height)
        Dim strFormat As StringFormat = StringFormat.GenericTypographic
        oGraphics.DrawString(OverlayText, f, oBrush, rect, strFormat)
        oGraphics.Dispose()
        oBrush.Dispose()
        f.Dispose()
        Return oBitmap
    End Function

    Private Function ImageResize(ByVal ImageFileName As String) As Image
        ' Get the scale factor.
        Dim scale_factor As Single = 100
        ' Get the source bitmap.
        Dim bm_source As New Bitmap(ImageFileName)
        ' Make a bitmap for the result.
        Dim bm_dest As New Bitmap( _
            CInt(bm_source.Width * scale_factor), _
            CInt(bm_source.Height * scale_factor))
        ' Make a Graphics object for the result Bitmap.
        Dim gr_dest As Graphics = Graphics.FromImage(bm_dest)
        ' Copy the source image into the destination bitmap.
        gr_dest.DrawImage(bm_source, 0, 0, _
            bm_dest.Width + 1, _
            bm_dest.Height + 1)
        Return Nothing
    End Function
    Public Sub AdjustSizeRatio(ByRef myBitmap As Bitmap)
        Dim Wscalevalue As Double
        Dim Hscalevalue As Double
        If myBitmap.Width > Width And Width <> 0 Then
            Wscalevalue = Width / myBitmap.Width
        Else
            Wscalevalue = 1
        End If
        If myBitmap.Height > Height And Height <> 0 Then
            Hscalevalue = Height / myBitmap.Height
        Else
            Hscalevalue = 1
        End If
        If Hscalevalue > Wscalevalue Then
            Height = CInt(myBitmap.Height * Wscalevalue)
            Width = CInt(myBitmap.Width * Wscalevalue)
        Else
            Height = CInt(myBitmap.Height * Hscalevalue)
            Width = CInt(myBitmap.Width * Hscalevalue)
        End If
    End Sub

End Class
