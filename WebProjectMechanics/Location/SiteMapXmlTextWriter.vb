Imports System.IO
Imports System.Text
Imports System.Xml

Public Class SiteMapXmlTextWriter
    Inherits XmlTextWriter

    ' Private ReadOnly writer As XmlTextWriter
    Private myActiveCompany As New ActiveCompany("MODIFIED")
    ' Private _textWriter As IO.TextWriter

    Sub New(textWriter As IO.TextWriter)
        MyBase.New(textWriter)
        '  _textWriter = textWriter
    End Sub
    Public Sub New(ByVal w As Stream, ByVal encoding As Encoding)
        MyBase.New(w, encoding)

    End Sub
    Public Sub New(ByVal filename As String, ByVal encoding As Encoding)
        MyBase.New(filename, encoding)

    End Sub

    Public Sub WriteSitemapDocument()
        WriteStartDocument()
        WriteStartElement("urlset")
        WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance")
        WriteAttributeString("xsi:schemaLocation", "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd")
        WriteAttributeString("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9")
        For Each myrow As Location In myActiveCompany.LocationList
            If myrow.ActiveFL Then
                WriteItem(GetLocation(myActiveCompany, myrow), myrow.ModifiedDT)
            End If
        Next
        WriteEndElement()
        WriteEndDocument()
        Close()

    End Sub

    Public Sub WriteSiteLocationDocument()
        WriteStartDocument()
        WriteStartElement("urlset")
        WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance")
        WriteAttributeString("xsi:schemaLocation", "http://www.projectmechanics.com/schemas/sitelocation/0.9 http://www.projectmechanics.com/schemas/sitelocation/0.9/sitelocation.xsd")
        WriteAttributeString("xmlns", "http://www.projectmechanics.com/schemas/sitelocation/0.9")
        WriteElementString("site", wpm_HostName)
        WriteElementString("contact", wpm_ContactName)
        For Each myrow As Location In myActiveCompany.LocationList
            If myrow.ActiveFL Then
                If Left(myrow.LocationURL, 4) <> "http" Then
                    WriteItem(myrow)
                End If
            End If
        Next
        WriteEndElement()
        WriteEndDocument()
    End Sub

    Public Sub WriteItem(ByVal link As String, ByVal publishedDate As DateTime)
        WriteStartElement("url")
        WriteElementString("loc", link)
        WriteElementString("lastmod", _
          String.Format("{0}-{1}-{2}", ZeroPad(Year(publishedDate).ToString, 4), ZeroPad(Month(publishedDate).ToString, 2), ZeroPad(Day(publishedDate).ToString, 2)))

        If publishedDate > DateTime.Now().AddDays(-30) Then
            WriteElementString("changefreq", "daily")
            WriteElementString("priority", "0.9")
        Else
            WriteElementString("changefreq", "monthly")
            WriteElementString("priority", "0.4")
        End If
        WriteEndElement()
    End Sub
    Public Sub WriteItem(ByVal myLocation As Location)
        WriteStartElement("url")
        WriteElementString("loc", myLocation.LocationURL)
        WriteElementString("keyword", myLocation.LocationKeywords)
        WriteElementString("lastmod", _
          String.Format("{0}-{1}-{2}", ZeroPad(Year(myLocation.ModifiedDT).ToString, 4), ZeroPad(Month(myLocation.ModifiedDT).ToString, 2), ZeroPad(Day(myLocation.ModifiedDT).ToString, 2)))

        If myLocation.ModifiedDT > DateTime.Now().AddDays(-30) Then
            WriteElementString("changefreq", "daily")
            WriteElementString("priority", "0.9")
        Else
            WriteElementString("changefreq", "monthly")
            WriteElementString("priority", "0.4")
        End If
        WriteEndElement()
    End Sub

    Private Function GetLocation(ByRef ActiveSite As ActiveCompany, ByRef myLocation As Location) As String

        If myActiveCompany.UseBreadCrumbURL Then
            If Left(myLocation.BreadCrumbURL, 4) = "http" Then
                Return String.Format("{0}", myLocation.BreadCrumbURL)
            Else
                If Left(ActiveSite.SiteURL, 4) = "http" Then
                    Return String.Format("{0}{1}", ActiveSite.SiteURL, myLocation.BreadCrumbURL)
                Else
                    Return String.Format("http://{0}{1}", ActiveSite.SiteURL, myLocation.BreadCrumbURL)
                End If
            End If
        Else
            If Left(myLocation.LocationURL, 4) = "http" Then
                Return String.Format("{0}", myLocation.LocationURL)
            Else
                If Left(ActiveSite.SiteURL, 4) = "http" Then
                    Return String.Format("{0}{1}", ActiveSite.SiteURL, myLocation.LocationURL)
                Else
                    Return String.Format("http://{0}{1}", ActiveSite.SiteURL, myLocation.LocationURL)
                End If
            End If

        End If

    End Function

    Private Shared Function ZeroPad(ByVal str As String, ByVal iSize As Integer) As String
        Return str.PadLeft(iSize, CChar("0"))
    End Function

End Class
