Imports System.Xml
Imports System.Xml.Xsl
Imports System.Web

Public Class wpmXML
    Inherits XmlDocument
#Region "properties"
    ' **********************************************
    ' Properties
    ' **********************************************
    Private _xmlURL As String
    Public Property xmlURL() As String
        Get
            Return _xmlURL
        End Get
        Set(ByVal value As String)
            _xmlURL = wpmUTIL.RemoveHtml(value)
            _xmlURL = wpmUTIL.ClearLineFeeds(_xmlURL)
            _xmlURL = _xmlURL.Replace("&amp;", "&")
        End Set
    End Property
    Private _FeedName As String
    Public Property FeedName() As String
        Get
            Return _FeedName
        End Get
        Set(ByVal value As String)
            _FeedName = value
        End Set
    End Property
    Private _xsltPath As String
    Public Property xsltPath() As String
        Get
            Return _xsltPath
        End Get
        Set(ByVal value As String)
            _xsltPath = value
        End Set
    End Property
    Private _templatePath As String
    Public Property templatePath() As String
        Get
            Return _templatePath
        End Get
        Set(ByVal value As String)
            _templatePath = value
        End Set
    End Property
    Private _filePrefix As String
    Public Property filePrefix() As String
        Get
            Return _filePrefix
        End Get
        Set(ByVal value As String)
            _filePrefix = value
        End Set
    End Property
    Private _xmlFileName As String
    Private _xsltFileName As String
#End Region
#Region "constructors"
    ' **********************************************
    ' Constructors
    ' **********************************************
    ''' <summary>
    ''' Initializes a new instance of the wpmXML class
    ''' </summary>
    '''  
    Sub New(ByVal sFeedName As String, ByVal sURL As String, ByVal sXSLTPath As String, ByVal CompanyID As String, ByVal sTemplatePath As String)
        FeedName = sFeedName
        xmlURL = sURL
        xsltPath = sXSLTPath
        filePrefix = CompanyID
        templatePath = sTemplatePath
        _xmlFileName = getXMLFileName()
    End Sub
    Sub New(ByVal sXMLFileName As String, ByVal sXSLTFileName As String)
        FeedName = String.Empty
        xmlURL = String.Empty
        templatePath = String.Empty
        filePrefix = String.Empty
        xsltPath = sXSLTFileName
        _xmlFileName = sXMLFileName
    End Sub
#End Region
#Region "publicfunctions"
    ' **********************************************
    ' Public Functions
    ' **********************************************
    Public Function getXMLTransform(ByVal sXMLFile As String, ByVal sXSLTFile As String) As String
        Dim myXslDoc As XslCompiledTransform = New XslCompiledTransform()
        Dim myStringBuilder As StringBuilder = New StringBuilder()
        Dim myStringWriter As StringWriter = New StringWriter(myStringBuilder)
        _xmlFileName = sXMLFile
        _xsltFileName = sXSLTFile
        If LoadXML() Then
            Try
                Dim mySettings As New System.Xml.Xsl.XsltSettings
                mySettings.EnableDocumentFunction = True
                mySettings.EnableScript = True
                myXslDoc.Load(_xsltFileName)
                myXslDoc.Transform(Me, Nothing, myStringWriter)
            Catch ex As Exception
                wpmLog.XMLLog("Problem processing XSL/XML - (" & _xsltFileName & ") - ", "xml url (" & xmlURL & ")")
                wpmLog.AuditLog("Problem processing XSL/XML - (" & _xsltFileName & ") - " & ex.ToString, "wpmXML.getXMLTransform")
            End Try
        End If
        Return myStringBuilder.ToString()
    End Function
    Public Function getXMLTransform() As String
        Dim myXslDoc As XslCompiledTransform = New XslCompiledTransform()
        Dim myStringBuilder As StringBuilder = New StringBuilder()
        Dim myStringWriter As StringWriter = New StringWriter(myStringBuilder)
        If LoadXML() Then
            Try
                If wpmFileIO.IsValidPath(xsltPath) Then
                    _xsltFileName = xsltPath
                Else
                    If wpmFileIO.IsValidPath(HttpContext.Current.Server.MapPath((templatePath & "xsl/" & xsltPath))) Then
                        _xsltFileName = HttpContext.Current.Server.MapPath((templatePath & "xsl/" & xsltPath))
                    ElseIf wpmFileIO.IsValidPath(HttpContext.Current.Server.MapPath((App.Config.wpmWebHome() & "style/" & xsltPath))) Then
                        wpmLog.XMLLog("Problem Finding XSL 1 - (" & HttpContext.Current.Server.MapPath(templatePath & "xsl/" & xsltPath) & ") - ", "xml url (" & xmlURL & ")")
                        _xsltFileName = HttpContext.Current.Server.MapPath((App.Config.wpmWebHome() & "style/" & xsltPath))
                    Else
                        wpmLog.XMLLog("Problem Finding XSL 2 - (" & HttpContext.Current.Server.MapPath(templatePath & "xsl/" & xsltPath) & ") - ", "xml url (" & xmlURL & ")")
                        _xsltFileName = HttpContext.Current.Server.MapPath((App.Config.wpmWebHome() & "style/rss_title.xsl"))
                    End If
                End If
                Dim mySettings As New System.Xml.Xsl.XsltSettings
                mySettings.EnableDocumentFunction = True
                mySettings.EnableScript = True
                myXslDoc.Load(_xsltFileName)
                myXslDoc.Transform(Me, Nothing, myStringWriter)
            Catch ex As Exception
                wpmLog.XMLLog("Problem processing XSL/XML - (" & _xsltFileName & ") - ", "xml url (" & xmlURL & ")")
                wpmLog.AuditLog("Problem processing XSL/XML - (" & _xsltFileName & ") - " & ex.Message.ToString, "wpmXML.getXMLTransform")
            End Try
        End If
        Return myStringBuilder.ToString()
    End Function
#End Region
#Region "privatefunctions"
    ' **********************************************
    ' Private Functions
    ' **********************************************
    Private Overloads Function LoadXML() As Boolean
        Dim path As String = getXMLFileName()
        Dim bReturn As Boolean = True
        Try
            If (xmlURL <> String.Empty) And (wpmFileIO.GetFileAgeMinutes(path) > App.Config.MaxXMLFeedAge) Then
                Dim request As System.Net.WebRequest = System.Net.WebRequest.Create(xmlURL)
                Dim response As System.Net.WebResponse = request.GetResponse()
                Dim mystream As Stream = response.GetResponseStream()
                Dim xmlreader As New XmlTextReader(mystream)
                xmlreader.ProhibitDtd = False
                Me.Load(xmlreader)
                Me.Save(path)
            Else
                Me.Load(path)
            End If
        Catch url_ex As Exception
            wpmLog.AuditLog("URL Exception", url_ex.Message & " - on " & xmlURL)
            Try
                Me.Load(path)
            Catch ex As Exception
                wpmLog.AuditLog("Problem getting XML Feed - (" & path & ") - " & ex.ToString, "wpmXML.GetXLMFromURL")
                bReturn = False
            End Try
        End Try
        Return bReturn
    End Function
    Private Function getXMLFileName() As String
        If Not wpmFileIO.VerifyFolderExists(App.Config.ConfigFolderPath & "xml") Then
            wpmFileIO.CreateFolder(App.Config.ConfigFolderPath & "xml")
        End If
        If FeedName <> String.Empty And filePrefix <> String.Empty Then
            _xmlFileName = App.Config.ConfigFolderPath & "xml\" & wpmUTIL.RemoveInvalidCharacters(Me.xmlURL) & ".xml"
        End If
        Return _xmlFileName
    End Function
#End Region
End Class
