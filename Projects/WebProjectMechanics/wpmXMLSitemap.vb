Imports System.Xml
Imports System.Web

Public Class wpmXmlSiteMap
    Implements IDisposable

    Private writer As XmlTextWriter
    Private mySiteMap As New wpmActiveSite("MODIFIED", HttpContext.Current.Session)

    Public Sub New(ByVal stream As System.IO.Stream, ByVal encoding As System.Text.Encoding)
        writer = New XmlTextWriter(stream, encoding)
        writer.Formatting = Formatting.Indented
    End Sub
    Public Sub New(ByVal w As System.IO.TextWriter)
        writer = New XmlTextWriter(w)
        writer.Formatting = Formatting.Indented
    End Sub
    Public Sub WriteSitemapDocument()
        writer.WriteStartDocument()
        writer.WriteStartElement("urlset")
        writer.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance")
        writer.WriteAttributeString("xsi:schemaLocation", "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd")
        writer.WriteAttributeString("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9")

        For Each myrow As wpmLocation In mySiteMap.LocationList
            If myrow.ActiveFL Then
                If Left(myrow.DisplayURL, 4) <> "http" Then
                    If mySiteMap.UseBreadCrumbURL Then
                        WriteItem(myrow.PageName, String.Format("http://{0}{1}", HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), myrow.BreadCrumbURL), myrow.PageDescription, mySiteMap.CompanyName, myrow.ModifiedDate, myrow.PageKeywords)
                    Else
                        WriteItem(myrow.PageName, String.Format("http://{0}{1}", HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), myrow.DisplayURL), myrow.PageDescription, mySiteMap.CompanyName, myrow.ModifiedDate, myrow.PageKeywords)
                    End If
                End If
            End If
        Next
        writer.WriteEndElement()
        writer.WriteEndDocument()
    End Sub


    Public Sub WriteItem(ByVal sPageName As String, ByVal link As String, ByVal description As String, ByVal author As String, ByVal publishedDate As DateTime, ByVal sPageKeywords As String)
        writer.WriteStartElement("url")
        writer.WriteElementString("loc", link)
        writer.WriteElementString("lastmod", _
          String.Format("{0}-{1}-{2}", ZeroPad(Year(publishedDate).ToString, 4), ZeroPad(Month(publishedDate).ToString, 2), ZeroPad(Day(publishedDate).ToString, 2)))

        If publishedDate > System.DateTime.Now().AddDays(-30) Then
            writer.WriteElementString("changefreq", "daily")
            writer.WriteElementString("priority", "0.9")
        Else
            writer.WriteElementString("changefreq", "monthly")
            writer.WriteElementString("priority", "0.4")
        End If
        writer.WriteEndElement()
    End Sub
    Private Shared Function ZeroPad(ByVal str As String, ByVal iSize As Integer) As String
        Return str.PadLeft(iSize, CChar("0"))
    End Function

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If
            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        writer.Close()
        Dispose()
        disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
