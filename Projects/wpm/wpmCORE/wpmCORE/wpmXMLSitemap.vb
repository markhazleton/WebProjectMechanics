Imports System.Xml
Imports System.Web

Public Class wpmXMLSitemap
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
                        WriteItem(myrow.PageName, "http://" & HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME") & myrow.BreadCrumbURL, myrow.PageDescription, mySiteMap.CompanyName, myrow.ModifiedDate, myrow.PageKeywords)
                    Else
                        WriteItem(myrow.PageName, "http://" & HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME") & myrow.DisplayURL, myrow.PageDescription, mySiteMap.CompanyName, myrow.ModifiedDate, myrow.PageKeywords)
                    End If
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

        If publishedDate > System.DateTime.Now().AddDays(-30) Then
            writer.WriteElementString("changefreq", "daily")
            writer.WriteElementString("priority", "0.9")
        Else
            writer.WriteElementString("changefreq", "monthly")
            writer.WriteElementString("priority", "0.4")
        End If
        writer.WriteEndElement()
    End Sub
    Private Function ZeroPad(ByVal str As String, ByVal iSize As Integer) As String
        Return str.PadLeft(iSize, CChar("0"))
    End Function
End Class
