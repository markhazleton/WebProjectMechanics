<%@ Import Namespace="WebProjectMechanics" %>
<%@ Import Namespace="System.Drawing" %>
<%@ Import Namespace="System.Drawing.Imaging" %>

<script language="VB" runat="Server">
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim sText As String = Request.QueryString("Path")
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
    End Sub
</script>
