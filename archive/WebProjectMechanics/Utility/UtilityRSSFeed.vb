Imports System.Text
Imports System.Xml

Public Class UtilityRSSFeed
    Implements IDisposable
    Private ActiveSite As ActiveCompany
    Private ItemCount As Integer

    Private writer As XmlTextWriter

    Public Sub New(ByVal w As System.IO.TextWriter, ByRef SiteMap As ActiveCompany)
        ActiveSite = SiteMap
        writer = New XmlTextWriter(w)
        writer.Formatting = Formatting.Indented
    End Sub
    Public Sub New(ByVal stream As System.IO.Stream, ByVal encoding As Encoding, ByRef SiteMap As ActiveCompany)
        ActiveSite = SiteMap
        writer = New XmlTextWriter(stream, encoding)
        writer.Formatting = Formatting.Indented
    End Sub

    Private Function GetLocation(ByRef ActiveSite As ActiveCompany, ByVal iRow As Integer) As String
        If Left(ActiveSite.LocationList(iRow).BreadCrumbURL, 4) = "http" Then
            Return String.Format("{0}", ActiveSite.LocationList(iRow).BreadCrumbURL)
        Else
            If Left(ActiveSite.SiteURL, 4) = "http" Then
                Return String.Format("{0}{1}", ActiveSite.SiteURL, ActiveSite.LocationList(iRow).BreadCrumbURL)
            Else
                Return String.Format("http://{0}{1}", ActiveSite.SiteURL, ActiveSite.LocationList(iRow).BreadCrumbURL)
            End If
        End If
    End Function

    Private Shared Function ValidForTimeScope(ByVal TimeScope As String, ByVal StartDT As Date, ByVal EndDT As Date) As Boolean
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

    Public Shared Function ApplyXMLFormatting(ByRef strInput As String) As String
        strInput = "~" & strInput
        strInput = Replace(strInput, "&", "&amp;")
        strInput = Replace(strInput, "'", "&apos;")
        strInput = Replace(strInput, """", "&quot;")
        strInput = Replace(strInput, ">", "&gt;")
        strInput = Replace(strInput, "<", "&lt;")
        strInput = Replace(strInput, "~", String.Empty)

        ApplyXMLFormatting = strInput
    End Function

    Public Shared Function CleanDBString(ByVal sString As String) As String
        Dim sReturn As String
        If IsDBNull(sString) Then
            sReturn = String.Empty
        Else
            sReturn = sString
        End If
        CleanDBString = ApplyXMLFormatting(sReturn)
    End Function
    Public Sub Close()
        writer.Flush()
        writer.Close()
    End Sub

    Public Shared Function FormatRSSBottom() As String
        FormatRSSBottom = vbCrLf & "</rss>"
    End Function

    Public Shared Function FormatRSSChannelBottom() As String
        FormatRSSChannelBottom = vbCrLf & "</channel>"
    End Function

    Public Shared Function FormatRSSChannelTop(ByVal sTitle As String, ByVal sLink As String, ByVal sDescription As String) As String
        Dim sReturn As String = vbCrLf & "<channel>"
        Dim sBaseURL As String = "http://" & System.Web.HttpContext.Current.Session.Item("SERVER_NAME").ToString
        sReturn = String.Format("{0}{1}   {2}", sReturn, vbCrLf, FormatXML("title", sTitle))
        sReturn = String.Format("{0}{1}   {2}", sReturn, vbCrLf, FormatXML("link", sBaseURL & sLink))
        sReturn = String.Format("{0}{1}   {2}", sReturn, vbCrLf, FormatXML("description", sDescription))
        FormatRSSChannelTop = sReturn
    End Function

    Public Shared Function FormatRSSItem(ByVal sTitle As String, ByRef sLink As String, ByRef sDecription As String, ByRef sPubDate As String) As String
        Dim sReturn As String = vbCrLf & "    <item>"
        Dim sBaseURL As String
        Dim session As System.Web.SessionState.HttpSessionState = System.Web.HttpContext.Current.Session
        sBaseURL = "http://" & session.Item("SERVER_NAME").ToString
        sReturn = String.Format("{0}{1}      {2}", sReturn, vbCrLf, FormatXML("title", sTitle))
        sReturn = String.Format("{0}{1}      {2}", sReturn, vbCrLf, FormatXML("link", sBaseURL & sLink))
        sReturn = String.Format("{0}{1}      {2}", sReturn, vbCrLf, FormatXML("description", sDecription))
        sReturn = String.Format("{0}{1}      {2}", sReturn, vbCrLf, FormatXML("pubDate", sPubDate))
        sReturn = String.Format("{0}{1}    </item>", sReturn, vbCrLf)
        FormatRSSItem = sReturn
    End Function

    Public Shared Function FormatRSSTop() As String
        FormatRSSTop = String.Format("<?xml version=""1.0"" ?><?xml-stylesheet type=""text/xsl"" href=""{0}style/news.xsl""?>{1}<rss version=""2.0"">", wpm_SiteConfig.ApplicationHome(), vbCrLf)
    End Function

    Public Shared Function FormatXML(ByVal sTag As String, ByVal sValue As String) As String
        FormatXML = String.Format("<{0}>{1}</{0}>", sTag, sValue)
    End Function

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

    Public Sub WriteRSSArticle()
        writer.WriteStartDocument()
        writer.WriteStartElement("rss")
        writer.WriteAttributeString("version", "2.0")
        Dim mySBDescription As New StringBuilder(String.Empty)

        WriteStartChannel(wpm_HostName & " Menu", _
          "http://" & ActiveSite.SiteURL, _
          wpm_HostName & " Menu in RSS", _
          "Copyright " & wpm_HostName, _
          "webmaster@" & Replace(HttpContext.Current.Request.ServerVariables("SERVER_NAME"), "http://", String.Empty))
        ItemCount = 0
        Dim myDateComp As LocationModifiedDateCompare = New LocationModifiedDateCompare() With {.Direction = UtilitySortDirection.DESC}
        ActiveSite.LocationList.Sort(myDateComp)

        For iRow As Integer = 0 To ActiveSite.LocationList.Count - 1
            mySBDescription.Length = 0
            mySBDescription.Append(wpm_RemoveHtml(ActiveSite.LocationList(iRow).LocationDescription))
            ActiveSite.BuildTemplate(mySBDescription)
            If (ActiveSite.LocationList(iRow).RecordSource = "Article") And ActiveSite.LocationList(iRow).ActiveFL And (Left(ActiveSite.LocationList(iRow).LocationURL.ToLower, 4)) <> "http" Then
                WriteItem(ActiveSite.LocationList(iRow).LocationName, GetLocation(ActiveSite, iRow), mySBDescription.ToString, ActiveSite.FromEmail, wpm_GetRFC822Date(ActiveSite.LocationList(iRow).ModifiedDT))
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
        Dim ArticleSummary As String = String.Empty
        Dim sSQL As String
        writer.WriteStartDocument()
        writer.WriteStartElement("rss")
        writer.WriteAttributeString("version", "2.0")

        If ActiveSite.CurrentLocationID = String.Empty Then
            sSQL = (String.Format("SELECT Page.PageID,Page.PageName,Page.PageDescription,Page.ParentPageID,pagetype.PageFileName, Page.AllowMessage, Page.PageFileName, Page.CompanyID, Article.StartDT, Article.EndDT, Article.ArticleBody, Article.Title, Article.Description, Article.ArticleSummary, Contact.PrimaryContact, Contact.EMail FROM (Page INNER JOIN pagetype ON Page.PageTypeID = pagetype.PageTypeID) INNER JOIN (article LEFT JOIN contact ON article.ContactID = contact.ContactID) ON Page.PageID = article.PageID  WHERE Page.Active = TRUE and Page.GroupID = 4  AND Page.CompanyID = {0} ORDER BY Article.StartDT DESC ", wpm_CurrentSiteID))
        Else
            sSQL = (String.Format("SELECT Page.PageID,Page.PageName,Page.PageDescription,Page.ParentPageID,pagetype.PageFileName, Page.AllowMessage, Page.PageFileName, Page.CompanyID, Article.StartDT, Article.EndDT, Article.ArticleBody, Article.Title, Article.Description, Article.ArticleSummary, Contact.PrimaryContact, Contact.EMail FROM (Page INNER JOIN pagetype ON Page.PageTypeID = pagetype.PageTypeID) INNER JOIN (article LEFT JOIN contact ON article.ContactID = contact.ContactID) ON Page.PageID = article.PageID  WHERE Page.Active = TRUE and Page.GroupID = 4  AND Page.CompanyID = {0} AND Page.PageID={1}  ORDER BY Article.StartDT DESC ", wpm_CurrentSiteID, ActiveSite.CurrentLocationID()))
        End If

        WriteStartChannel(ActiveSite.CurrentLocationNM(), _
                              String.Format("http://{0}{1}", HttpContext.Current.Request.ServerVariables("SERVER_NAME"), wpm_FormatPageNameURL(ActiveSite.CurrentLocationNM())), _
                              ActiveSite.CurrentLocationDS(), _
                              String.Format("Copyright {0}, {1}", wpm_HostName, Year(Now())), _
                              ActiveSite.FromEmail)
        For Each myrow As DataRow In wpm_GetDataTable(sSQL, "WriteRSSBlogDocument").Rows
            ArticleSummary = wpm_GetDBString(myrow.Item("ArticleSummary"))
            If ArticleSummary = String.Empty Then
                ArticleSummary = wpm_GetDBString(myrow.Item("Description"))
            End If

            If ValidForTimeScope(TimeScope, wpm_GetDBDate(myrow.Item("StartDT")), wpm_GetDBDate(myrow.Item("EndDT"))) Then
                WriteItem(myrow.Item("Title").ToString, _
                String.Format("http://{0}{1}", HttpContext.Current.Request.ServerVariables("SERVER_NAME"), wpm_FormatPageNameURL(myrow.Item("Title").ToString)), _
                ArticleSummary, _
                myrow.Item("EMail").ToString, _
                wpm_GetRFC822Date(myrow.Item("StartDT")))
            End If
        Next
        writer.WriteEndElement()
        writer.WriteEndDocument()
    End Sub

    Public Sub WriteRSSMenuDocument()
        writer.WriteStartDocument()
        writer.WriteStartElement("rss")
        writer.WriteAttributeString("version", "2.0")
        Dim mySBDescription As New StringBuilder(String.Empty)

        WriteStartChannel(wpm_HostName & " Menu", _
          "http://" & ActiveSite.SiteURL, _
          wpm_HostName & " Menu in RSS", _
          "Copyright " & wpm_HostName, _
          "webmaster@" & Replace(HttpContext.Current.Request.ServerVariables("SERVER_NAME"), "http://", String.Empty))
        ItemCount = 0
        Dim myDateComp As LocationModifiedDateCompare = New LocationModifiedDateCompare() With {.Direction = UtilitySortDirection.DESC}
        ActiveSite.LocationList.Sort(myDateComp)

        For iRow As Integer = 0 To ActiveSite.LocationList.Count - 1
            mySBDescription.Length = 0
            mySBDescription.Append(ActiveSite.LocationList(iRow).LocationDescription)
            ActiveSite.BuildTemplate(mySBDescription)
            If (ActiveSite.LocationList(iRow).RecordSource = "Page" Or ActiveSite.LocationList(iRow).RecordSource = "Category") And ActiveSite.LocationList(iRow).ActiveFL And (Left(ActiveSite.LocationList(iRow).LocationURL.ToLower, 4)) <> "http" Then
                WriteItem(ActiveSite.LocationList(iRow).LocationName, GetLocation(ActiveSite, iRow), mySBDescription.ToString, ActiveSite.FromEmail, wpm_GetRFC822Date(ActiveSite.LocationList(iRow).ModifiedDT))
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
    Public Sub WriteStartChannel(ByVal title As String, ByVal link As String, ByVal description As String, ByVal copyright As String, ByVal webMaster As String)
        writer.WriteStartElement("channel")
        writer.WriteElementString("title", title)
        writer.WriteElementString("link", link)
        writer.WriteElementString("description", description)
        writer.WriteElementString("language", "en-us")
        writer.WriteElementString("copyright", copyright)
        writer.WriteElementString("generator", "Project Mechanics RSS Feed Generator v0.5")
        writer.WriteElementString("webMaster", webMaster)
        writer.WriteElementString("lastBuildDate", DateTime.Now.ToString("r"))
        writer.WriteElementString("ttl", "20")
    End Sub

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

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If
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
