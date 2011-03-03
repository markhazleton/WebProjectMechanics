<%@Import Namespace="System.Drawing.Imaging" %>
<script language="VB" runat="server">
    Function ThumbnailCallback() As Boolean
        Return False
    End Function
    Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
        'Read in the image filename to create a thumbnail of
        Dim imageUrl As String = Request.QueryString("img")
        'Read in the width and height
        Dim imageHeight As Integer = Request.QueryString("h")
        Dim imageWidth As Integer = Request.QueryString("w")
        ''Make sure that the image URL doesn't contain any /'s or \'s
        'If imageUrl.IndexOf("/") >= 0 Or imageUrl.IndexOf("\") >= 0 Then
        '    'We found a / or \
        '    Response.End()
        'End If
        ''Add on the appropriate directory
        'imageUrl = "/images/" & imageUrl
        If WebProjectMechanics.wpmFileProcessing.IsValidPath(Server.MapPath(imageUrl)) Then
            Dim fullSizeImg As System.Drawing.Image
            fullSizeImg = System.Drawing.Image.FromFile(Server.MapPath(imageUrl))
            Dim scalevalue As Double
            If fullSizeImg.Width > imageWidth And imageWidth <> 0 Then
                scalevalue = imageWidth / fullSizeImg.Width
                imageHeight = fullSizeImg.Height * scalevalue
            End If
            If fullSizeImg.Height > imageHeight And imageHeight <> 0 Then
                scalevalue = imageHeight / fullSizeImg.Height
                imageWidth = fullSizeImg.Width * scalevalue
            End If
            'Do we need to create a thumbnail?
            Response.ContentType = "image/jpeg"
            If imageHeight > 0 And imageWidth > 0 Then
                Dim dummyCallBack As System.Drawing.Image.GetThumbnailImageAbort
                dummyCallBack = New System.Drawing.Image.GetThumbnailImageAbort(AddressOf ThumbnailCallback)
                Dim thumbNailImg As System.Drawing.Image
                thumbNailImg = fullSizeImg.GetThumbnailImage(imageWidth, imageHeight, dummyCallBack, IntPtr.Zero)
                thumbNailImg.Save(Response.OutputStream, ImageFormat.Jpeg)
                'Clean up / Dispose...
                thumbNailImg.Dispose()
            Else
                fullSizeImg.Save(Response.OutputStream, ImageFormat.Jpeg)
            End If
            'Clean up / Dispose...
            fullSizeImg.Dispose()
        End If
    End Sub
    Private Function Resize(ByVal ImageFileName As String) As System.Drawing.Image
        ' Get the scale factor.
        Dim scale_factor As Single = 100
        ' Get the source bitmap.
        Dim bm_source As New System.Drawing.Bitmap(ImageFileName)
        ' Make a bitmap for the result.
        Dim bm_dest As New System.Drawing.Bitmap( _
            CInt(bm_source.Width * scale_factor), _
            CInt(bm_source.Height * scale_factor))
        ' Make a Graphics object for the result Bitmap.
        Dim gr_dest As System.Drawing.Graphics = System.Drawing.Graphics.FromImage(bm_dest)
        ' Copy the source image into the destination bitmap.
        gr_dest.DrawImage(bm_source, 0, 0, _
            bm_dest.Width + 1, _
            bm_dest.Height + 1)
        Return Nothing
    End Function
</script>