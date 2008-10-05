Public Class mhRssFeed
    Private writer As XmlTextWriter
    Private mySiteMap As mhSiteMap
    Private ItemCount As Integer = 0
    Public Sub New(ByVal stream As System.IO.Stream, ByVal encoding As System.Text.Encoding, ByRef SiteMap As mhSiteMap)
        mySiteMap = SiteMap
        writer = New XmlTextWriter(stream, encoding)
        writer.Formatting = Formatting.Indented
    End Sub

    Public Sub New(ByVal w As System.IO.TextWriter, ByRef SiteMap As mhSiteMap)
        mySiteMap = SiteMap
        writer = New XmlTextWriter(w)
        writer.Formatting = Formatting.Indented
    End Sub

    Public Sub WriteRSSMenuDocument()
        writer.WriteStartDocument()
        writer.WriteStartElement("rss")
        writer.WriteAttributeString("version", "2.0")
        Dim mySiteMap As New mhSiteMap("MODIFIED", HttpContext.Current.Session)

        WriteStartChannel(mySiteMap.mySiteFile.CompanyName & " Menu", _
          "http://" & mySiteMap.mySiteFile.SiteURL, _
          mySiteMap.mySiteFile.CompanyName & " Menu in RSS", _
          "Copyright " & mySiteMap.mySiteFile.CompanyName, _
          "webmaster@" & Replace(HttpContext.Current.Request.ServerVariables("SERVER_NAME"), "http://", ""))
        ItemCount = 0
        For Each myrow As mhSiteMapRow In mySiteMap.mySiteFile.SiteMapRows
            If ItemCount < 100 And myrow.RecordSource = "Page" And myrow.ActiveFL And LCase(Left(myrow.DisplayURL, 4)) <> "http" Then
                WriteItem(myrow.PageName, "http://" & mySiteMap.mySiteFile.SiteURL & myrow.BreadCrumbURL, myrow.PageDescription, "mark.hazleton@projectmechanics.com", myrow.ModifiedDate)
                ItemCount = ItemCount + 1
            End If
        Next
        writer.WriteEndElement()
        writer.WriteEndElement()
        writer.WriteEndDocument()
    End Sub

    Public Sub WriteRSSBlogDocument(ByVal sPageID As String, ByVal CompanyID As String, ByVal CurrentPageName As String, ByVal CurrentPageDescription As String, ByVal CompanyName As String)
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
                       "Article.ArticleBody, " & _
                       "Article.Title, " & _
                       "Article.Description, " & _
                       "Contact.PrimaryContact, " & _
                       "Contact.EMail " & _
                 " FROM Page, pagetype, article,contact " & _
                " WHERE Page.PageTypeID = pagetype.PageTypeID " & _
                  " AND Page.Active = TRUE and Page.GroupID = 4 " & _
                  " AND Page.CompanyID = " & CompanyID & _
                  " AND Page.PageID=" & sPageID & " " & _
                  " AND Page.PageID=Article.PageID " & _
                  " AND Article.ContactID = Contact.ContactID " & _
                 " ORDER BY Article.StartDT DESC ")

        WriteStartChannel(CurrentPageName, _
                              "http://" & HttpContext.Current.Request.ServerVariables("SERVER_NAME") & _
                                   mhUTIL.FormatPageNameURL(CurrentPageName), _
                              CurrentPageDescription, _
                              "Copyright &copy; " & CompanyName & ", 1999-" & Year(Now()), _
                              "Mark.Hazleton@projectmechanics.com")
        For Each myrow As DataRow In mhDB.GetDataTable(sSQL, "WriteRSSBlogDocument").Rows
            WriteItem(myrow.Item("Title").ToString, _
            "http://" & HttpContext.Current.Request.ServerVariables("SERVER_NAME") & _
              mhUTIL.FormatPageNameURL(myrow.Item("Title").ToString), _
            myrow.Item("Description").ToString, _
            myrow.Item("EMail").ToString, _
            mhUTIL.GetDBDate(myrow.Item("StartDT")))
        Next
        writer.WriteEndElement()
        writer.WriteEndDocument()
    End Sub
    Public Sub Close()
        writer.Flush()
        writer.Close()
    End Sub

    Public Sub WriteStartChannel(ByVal title As String, ByVal link As String, ByVal description As String, ByVal copyright As String, ByVal webMaster As String)
        writer.WriteStartElement("channel")
        writer.WriteElementString("title", title)
        writer.WriteElementString("link", link)
        writer.WriteElementString("description", description)
        writer.WriteElementString("language", "en-us")
        writer.WriteElementString("copyright", copyright)
        writer.WriteElementString("generator", "MHWEB RSS Feed Generator v0.8")
        writer.WriteElementString("webMaster", webMaster)
        writer.WriteElementString("lastBuildDate", DateTime.Now.ToString("r"))
        writer.WriteElementString("ttl", "20")
    End Sub

    Public Sub WriteItem(ByVal title As String, ByVal link As String, ByVal description As String, ByVal author As String, ByVal publishedDate As DateTime)
        writer.WriteStartElement("item")
        writer.WriteElementString("title", title)
        writer.WriteElementString("link", link)
        writer.WriteElementString("guid", link)
        writer.WriteStartElement("description")
        writer.WriteCData(description)
        writer.WriteEndElement()
        writer.WriteElementString("author", author)
        writer.WriteElementString("pubDate", publishedDate.ToString("r"))
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
        FormatRSSTop = "<?xml version=""1.0"" ?><?xml-stylesheet type=""text/xsl"" href=""" & mhConfig.mhWebHome & "style/news.xsl""?>" & vbCrLf & "<rss version=""2.0"">"
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
