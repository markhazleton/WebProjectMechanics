Public Class mhXMLSitemap
    Private writer As XmlTextWriter
    Private mySiteMap As New mhSiteMap("MODIFIED", HttpContext.Current.Session)

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
        writer.WriteAttributeString("xmlns", "http://www.google.com/schemas/sitemap/0.84")
        writer.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance")
        writer.WriteAttributeString("xsi:schemaLocation", "http://www.google.com/schemas/sitemap/0.84 http://www.google.com/schemas/sitemap/0.84/sitemap.xsd")
        For irow As Integer = 0 To mySiteMap.mySiteFile.SiteMapRows.Count - 1
            If mySiteMap.mySiteFile.SiteMapRows.Item(irow).ActiveFL Then
                If mySiteMap.mySiteFile.UseBreadCrumbURL() Then
                    WriteItem(mySiteMap.mySiteFile.SiteMapRows.Item(irow).PageName, _
                      "http://" & HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME") & mySiteMap.mySiteFile.SiteMapRows.Item(irow).BreadCrumbURL, _
                      mySiteMap.mySiteFile.SiteMapRows.Item(irow).PageDescription, _
                      "mark.hazleton@projectmechanics.com", _
                      mySiteMap.mySiteFile.SiteMapRows.Item(irow).ModifiedDate, _
                      mySiteMap.mySiteFile.SiteMapRows.Item(irow).PageKeywords)
                Else
                    WriteItem(mySiteMap.mySiteFile.SiteMapRows.Item(irow).PageName, _
                      "http://" & HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME") & mySiteMap.mySiteFile.SiteMapRows.Item(irow).DisplayURL, _
                      mySiteMap.mySiteFile.SiteMapRows.Item(irow).PageDescription, _
                      "mark.hazleton@projectmechanics.com", _
                      mySiteMap.mySiteFile.SiteMapRows.Item(irow).ModifiedDate, _
                      mySiteMap.mySiteFile.SiteMapRows.Item(irow).PageKeywords)
                End If
            End If
        Next
        writer.WriteEndElement()
        writer.WriteEndDocument()
    End Sub

    Public Sub Close()
        writer.Flush()
        writer.Close()
    End Sub
    Public Sub WriteItem(ByVal sPageName As String, ByVal link As String, ByVal description As String, ByVal author As String, ByVal publishedDate As DateTime, ByVal sPageKeywords As String)
        writer.WriteStartElement("url")
        writer.WriteElementString("loc", link)
        writer.WriteElementString("lastmod", _
          ZeroPad(Year(publishedDate).ToString, 4) & "-" & ZeroPad(Month(publishedDate).ToString, 2) & "-" & ZeroPad(Day(publishedDate).ToString, 2))
        writer.WriteEndElement()
    End Sub
    Private Function ZeroPad(ByVal str As String, ByVal iSize As Integer) As String
        Return str.PadLeft(iSize, CChar("0"))
    End Function
End Class
