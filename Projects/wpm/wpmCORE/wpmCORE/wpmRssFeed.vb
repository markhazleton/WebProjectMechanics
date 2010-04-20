Imports System.Xml
Imports System.Web

Public Class wpmRssFeed
    Private writer As XmlTextWriter
    Private ActiveSite As wpmActiveSite
    Private ItemCount As Integer = 0
    Public Sub New(ByVal stream As System.IO.Stream, ByVal encoding As System.Text.Encoding, ByRef SiteMap As wpmActiveSite)
        ActiveSite = SiteMap
        writer = New XmlTextWriter(stream, encoding)
        writer.Formatting = Formatting.Indented
    End Sub

    Public Sub New(ByVal w As System.IO.TextWriter, ByRef SiteMap As wpmActiveSite)
        ActiveSite = SiteMap
        writer = New XmlTextWriter(w)
        writer.Formatting = Formatting.Indented
    End Sub

    Public Sub WriteRSSMenuDocument()
        writer.WriteStartDocument()
        writer.WriteStartElement("rss")
        writer.WriteAttributeString("version", "2.0")
        Dim mySBDescription As New StringBuilder(String.Empty)

        WriteStartChannel(ActiveSite.CompanyName & " Menu", _
          "http://" & ActiveSite.SiteURL, _
          ActiveSite.CompanyName & " Menu in RSS", _
          "Copyright " & ActiveSite.CompanyName, _
          "webmaster@" & Replace(HttpContext.Current.Request.ServerVariables("SERVER_NAME"), "http://", ""))
        ItemCount = 0
        Dim myDateComp As LocationModifiedDateCompare = New LocationModifiedDateCompare()
        myDateComp.Direction = wpmSortDirection.DESC
        ActiveSite.LocationList.Sort(myDateComp)

        For iRow As Integer = 0 To ActiveSite.LocationList.Count - 1
            mySBDescription.Length = 0
            mySBDescription.Append(ActiveSite.LocationList(iRow).PageDescription)
            ActiveSite.BuildTemplate(mySBDescription)
            If (ActiveSite.LocationList(iRow).RecordSource = "Page" Or ActiveSite.LocationList(iRow).RecordSource = "Category") And ActiveSite.LocationList(iRow).ActiveFL And (Left(ActiveSite.LocationList(iRow).DisplayURL.ToLower, 4)) <> "http" Then
                WriteItem(ActiveSite.LocationList(iRow).PageName, "http://" & ActiveSite.SiteURL & ActiveSite.LocationList(iRow).BreadCrumbURL, mySBDescription.ToString, ActiveSite.FromEmail, wpmUTIL.GetRFC822Date(ActiveSite.LocationList(iRow).ModifiedDate))
                ItemCount = ItemCount + 1
            End If
            If ItemCount > 19 Then
                Exit For
            End If
        Next
        ActiveSite.LocationList.DefaultSort()
        writer.WriteEndElement()
        writer.WriteEndElement()
        writer.WriteEndDocument()
    End Sub

    Public Sub WriteRSSBlogDocument(ByVal TimeScope As String)
        Dim ArticleSummary As String = ""
        writer.WriteStartDocument()
        writer.WriteStartElement("rss")
        writer.WriteAttributeString("version", "2.0")

        Dim sSQL As String = ("SELECT Page.PageID," & _
                       "Page.PageName," & _
                       "Page.PageDescription," & _
                       "Page.ParentPageID," & _
                       "pagetype.PageFileName, " & _
                       "Page.AllowMessage, " & _
                       "Page.PageFileName, " & _
                       "Page.CompanyID, " & _
                       "Article.StartDT, " & _
                       "Article.EndDT, " & _
                       "Article.ArticleBody, " & _
                       "Article.Title, " & _
                       "Article.Description, " & _
                       "Article.ArticleSummary, " & _
                       "Contact.PrimaryContact, " & _
                       "Contact.EMail " & _
                       "FROM (Page INNER JOIN pagetype ON Page.PageTypeID = pagetype.PageTypeID) INNER JOIN (article LEFT JOIN contact ON article.ContactID = contact.ContactID) ON Page.PageID = article.PageID " & _
                " WHERE Page.Active = TRUE and Page.GroupID = 4 " & _
                  " AND Page.CompanyID = " & ActiveSite.CompanyID & _
                  " AND Page.PageID=" & ActiveSite.CurrentPageID() & " " & _
                 " ORDER BY Article.StartDT DESC ")

        WriteStartChannel(ActiveSite.GetCurrentPageName(), _
                              "http://" & HttpContext.Current.Request.ServerVariables("SERVER_NAME") & _
                                   wpmUTIL.FormatPageNameURL(ActiveSite.GetCurrentPageName()), _
                              ActiveSite.CurrentPageDescription(), _
                              "Copyright " & ActiveSite.CompanyName & ", " & Year(Now()), _
                              ActiveSite.FromEmail)
        For Each myrow As DataRow In wpmDB.GetDataTable(sSQL, "WriteRSSBlogDocument").Rows
            ArticleSummary = wpmUTIL.GetDBString(myrow.Item("ArticleSummary"))
            If ArticleSummary = String.Empty Then
                ArticleSummary = wpmUTIL.GetDBString(myrow.Item("Description"))
            End If

            If ValidForTimeScope(TimeScope, wpmUTIL.GetDBDate(myrow.Item("StartDT")), wpmUTIL.GetDBDate(myrow.Item("EndDT"))) Then
                WriteItem(myrow.Item("Title").ToString, _
                "http://" & HttpContext.Current.Request.ServerVariables("SERVER_NAME") & _
                  wpmUTIL.FormatPageNameURL(myrow.Item("Title").ToString), _
                ArticleSummary, _
                myrow.Item("EMail").ToString, _
                wpmUTIL.GetRFC822Date(myrow.Item("StartDT")))
            End If
        Next
        writer.WriteEndElement()
        writer.WriteEndDocument()
    End Sub
    Public Sub Close()
        writer.Flush()
        writer.Close()
    End Sub

    Private Function ValidForTimeScope(ByVal TimeScope As String, ByVal StartDT As Date, ByVal EndDT As Date) As Boolean
        Dim bReturn As Boolean = False
        Select Case TimeScope.ToUpper
            Case "UPCOMING"
                If StartDT > System.DateTime.Now() Or EndDT > System.DateTime.Now() Then
                    bReturn = True
                End If
            Case "PAST"
                If StartDT < System.DateTime.Now() And EndDT < System.DateTime.Now() Then
                    bReturn = True
                End If
            Case Else
                bReturn = True
        End Select
        Return bReturn
    End Function
    Public Sub WriteStartChannel(ByVal title As String, ByVal link As String, ByVal description As String, ByVal copyright As String, ByVal webMaster As String)
        writer.WriteStartElement("channel")
        writer.WriteElementString("title", title)
        writer.WriteElementString("link", link)
        writer.WriteElementString("description", description)
        writer.WriteElementString("language", "en-us")
        writer.WriteElementString("copyright", copyright)
        writer.WriteElementString("generator", "WPM RSS Feed Generator v0.9")
        writer.WriteElementString("webMaster", webMaster)
        writer.WriteElementString("lastBuildDate", DateTime.Now.ToString("r"))
        writer.WriteElementString("ttl", "20")
    End Sub

    Public Sub WriteItem(ByVal title As String, ByVal link As String, ByVal description As String, ByVal author As String, ByVal publishedDate As String)
        writer.WriteStartElement("item")
        writer.WriteElementString("title", title)
        writer.WriteElementString("link", link)
        writer.WriteElementString("guid", link)
        writer.WriteStartElement("description")
        writer.WriteCData(description)
        writer.WriteEndElement()
        writer.WriteElementString("author", author)
        writer.WriteElementString("pubDate", publishedDate)
        writer.WriteEndElement()
    End Sub

    Public Shared Function CleanDBString(ByVal sString As String) As String
        Dim sReturn As String
        If IsDBNull(sString) Then
            sReturn = ""
        Else
            sReturn = sString
        End If
        CleanDBString = ApplyXMLFormatting(sReturn)
    End Function

    Public Shared Function ApplyXMLFormatting(ByRef strInput As String) As String
        strInput = "~" & strInput
        strInput = Replace(strInput, "&", "&amp;")
        strInput = Replace(strInput, "'", "&apos;")
        strInput = Replace(strInput, """", "&quot;")
        strInput = Replace(strInput, ">", "&gt;")
        strInput = Replace(strInput, "<", "&lt;")
        strInput = Replace(strInput, "~", "")

        ApplyXMLFormatting = strInput
    End Function

    Public Shared Function FormatXML(ByVal sTag As String, ByVal sValue As String) As String
        FormatXML = "<" & sTag & ">" & sValue & "</" & sTag & ">"
    End Function

    Public Shared Function FormatRSSChannelTop(ByVal sTitle As String, ByVal sLink As String, ByVal sDescription As String) As String
        Dim sReturn As String
        Dim sBaseURL As String
        sBaseURL = "http://" & System.Web.HttpContext.Current.Session.Item("SERVER_NAME").ToString
        sReturn = vbCrLf & "<channel>"
        sReturn = sReturn & vbCrLf & "   " & FormatXML("title", sTitle)
        sReturn = sReturn & vbCrLf & "   " & FormatXML("link", sBaseURL & sLink)
        sReturn = sReturn & vbCrLf & "   " & FormatXML("description", sDescription)
        FormatRSSChannelTop = sReturn
    End Function

    Public Shared Function FormatRSSChannelBottom() As String
        FormatRSSChannelBottom = vbCrLf & "</channel>"
    End Function

    Public Shared Function FormatRSSTop() As String
        FormatRSSTop = "<?xml version=""1.0"" ?><?xml-stylesheet type=""text/xsl"" href=""" & App.Config.wpmWebHome() & "style/news.xsl""?>" & vbCrLf & "<rss version=""2.0"">"
    End Function

    Public Shared Function FormatRSSBottom() As String
        FormatRSSBottom = vbCrLf & "</rss>"
    End Function

    Public Shared Function FormatRSSItem(ByVal sTitle As String, ByRef sLink As String, ByRef sDecription As String, ByRef sPubDate As String) As String
        Dim sReturn As String
        Dim sBaseURL As String
        Dim session As System.Web.SessionState.HttpSessionState = System.Web.HttpContext.Current.Session
        sBaseURL = "http://" & session.Item("SERVER_NAME").ToString
        sReturn = vbCrLf & "    <item>"
        sReturn = sReturn & vbCrLf & "      " & FormatXML("title", sTitle)
        sReturn = sReturn & vbCrLf & "      " & FormatXML("link", sBaseURL & sLink)
        sReturn = sReturn & vbCrLf & "      " & FormatXML("description", sDecription)
        sReturn = sReturn & vbCrLf & "      " & FormatXML("pubDate", sPubDate)
        sReturn = sReturn & vbCrLf & "    </item>"
        FormatRSSItem = sReturn
    End Function

    'Public Shared Function getXML(ByRef sourceFile As Object) As Object
    '    '        Dim xmlhttp As MSXML2.ServerXMLHTTP
    '    'xmlhttp = New MSXML2.ServerXMLHTTP
    '    'xmlhttp.Open("GET", sourceFile, False)
    '    'xmlhttp.Send()
    '    'getXML = xmlhttp.ResponseText
    '    'xmlhttp = Nothing
    '    Return Nothing
    'End Function

    'Public Shared Function applyStyle(ByRef xml As Object, ByRef xsl As Object) As String
    '    'Dim source As MSXML.DOMDocument
    '    'Dim style As MSXML.DOMDocument
    '    'source = New MSXML.DOMDocument
    '    'source.async = False
    '    'source.loadxml(xml)
    '    'style = New MSXML.DOMDocument
    '    'style.async = False
    '    'style.load(HttpContext.Current.Server.MapPath(xsl))
    '    'applyStyle = source.transformNode(style)
    '    'source = Nothing
    '    'style = Nothing
    '    Return ""
    'End Function
End Class
