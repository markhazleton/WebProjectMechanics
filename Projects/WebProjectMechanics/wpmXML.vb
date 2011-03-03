Imports System.Xml
Imports System.Xml.Xsl
Imports System.Web
Imports System.IO
Imports System.Text

Public Class wpmXML
    Inherits XmlDocument
#Region "properties"
    ' **********************************************
    ' Properties
    ' **********************************************
    Public Property xmlURL() As String
    Public Property FeedName() As String
    Public Property xsltPath() As String
    Public Property templatePath() As String
    Public Property filePrefix() As String
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
    Public Sub New()

    End Sub
    Public Sub New(ByVal nt As XmlNameTable)
        MyBase.New(nt)

    End Sub
    Protected Friend Sub New(ByVal imp As XmlImplementation)
        MyBase.New(imp)

    End Sub

#End Region
#Region "publicfunctions"
    ' **********************************************
    ' Public Functions
    ' **********************************************
    Public Function getXMLTransform(ByVal sXMLFile As String, ByVal sXSLTFile As String) As String
        Dim myXslDoc As XslCompiledTransform = New XslCompiledTransform()
        Dim myStringBuilder As StringBuilder = New StringBuilder()
        Using myStringWriter As StringWriter = New StringWriter(myStringBuilder)
            _xmlFileName = sXMLFile
            _xsltFileName = sXSLTFile
            If LoadXML() Then
                Try
                    ' Dim mySettings As New XsltSettings() With {.EnableDocumentFunction = True, .EnableScript = True}
                    myXslDoc.Load(_xsltFileName)
                    myXslDoc.Transform(Me, Nothing, myStringWriter)
                Catch ex As Exception
                    wpmLogging.XMLLog(String.Format("Problem processing XSL/XML - ({0}) - ", _xsltFileName), String.Format("xml url ({0})", xmlURL))
                    wpmLogging.AuditLog(String.Format("Problem processing XSL/XML - ({0}) - {1}", _xsltFileName, ex), "wpmXML.getXMLTransform")
                End Try
            End If
        End Using
        Return myStringBuilder.ToString()
    End Function
    Public Function getXMLTransform() As String
        Dim myXslDoc As New XslCompiledTransform()
        Dim myStringBuilder As New StringBuilder()
        Using myStringWriter As New StringWriter(myStringBuilder)
            If LoadXML() Then
                Try
                    If wpmFileProcessing.IsValidPath(xsltPath) Then
                        _xsltFileName = xsltPath
                    Else
                        If wpmFileProcessing.IsValidPath(HttpContext.Current.Server.MapPath((String.Format("{0}xsl/{1}", templatePath, xsltPath)))) Then
                            _xsltFileName = HttpContext.Current.Server.MapPath((String.Format("{0}xsl/{1}", templatePath, xsltPath)))
                        ElseIf wpmFileProcessing.IsValidPath(HttpContext.Current.Server.MapPath((String.Format("{0}style/{1}", wpmApp.Config.wpmWebHome(), xsltPath)))) Then
                            wpmLogging.XMLLog(String.Format("Problem Finding XSL 1 - ({0}) - ", HttpContext.Current.Server.MapPath(String.Format("{0}xsl/{1}", templatePath, xsltPath))), String.Format("xml url ({0})", xmlURL))
                            _xsltFileName = HttpContext.Current.Server.MapPath((String.Format("{0}style/{1}", wpmApp.Config.wpmWebHome(), xsltPath)))
                        Else
                            wpmLogging.XMLLog(String.Format("Problem Finding XSL 2 - ({0}) - ", HttpContext.Current.Server.MapPath(String.Format("{0}xsl/{1}", templatePath, xsltPath))), String.Format("xml url ({0})", xmlURL))
                            _xsltFileName = HttpContext.Current.Server.MapPath((wpmApp.Config.wpmWebHome() & "style/rss_title.xsl"))
                        End If
                    End If
                    ' Dim mySettings As New XsltSettings() With {.EnableDocumentFunction = True, .EnableScript = True}
                    myXslDoc.Load(_xsltFileName)
                    myXslDoc.Transform(Me, Nothing, myStringWriter)
                Catch ex As Exception
                    wpmLogging.XMLLog(String.Format("Problem processing XSL/XML - ({0}) - ", _xsltFileName), String.Format("xml url ({0})", xmlURL))
                    wpmLogging.AuditLog(String.Format("Problem processing XSL/XML - ({0}) - {1}", _xsltFileName, ex.Message), "wpmXML.getXMLTransform")
                End Try
            End If
        End Using
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
            If (xmlURL <> String.Empty) And (wpmFileProcessing.GetFileAgeMinutes(path) > wpmApp.Config.MaxXMLFeedAge) Then
                Dim request As System.Net.WebRequest = System.Net.WebRequest.Create(xmlURL)
                Dim response As System.Net.WebResponse = request.GetResponse()
                Dim mystream As Stream = response.GetResponseStream()
                Dim xmlreader As New XmlTextReader(mystream)
                ' xmlreader.ProhibitDtd = False
                Load(xmlreader)
                Save(path)
            Else
                Load(path)
            End If
        Catch url_ex As Exception
            wpmLogging.AuditLog("URL Exception", String.Format("{0} - on {1}", url_ex.Message, xmlURL))
            Try
                Load(path)
            Catch ex As Exception
                wpmLogging.AuditLog(String.Format("Problem getting XML Feed - ({0}) - {1}", path, ex), "wpmXML.GetXLMFromURL")
                bReturn = False
            End Try
        End Try
        Return bReturn
    End Function
    Private Function getXMLFileName() As String
        If Not wpmFileProcessing.VerifyFolderExists(wpmApp.Config.ConfigFolderPath & "xml") Then
            wpmFileProcessing.CreateFolder(wpmApp.Config.ConfigFolderPath & "xml")
        End If
        If FeedName <> String.Empty And filePrefix <> String.Empty Then
            _xmlFileName = String.Format("{0}xml\{1}.xml", wpmApp.Config.ConfigFolderPath, wpmUtil.RemoveInvalidCharacters(xmlURL))
        End If
        Return _xmlFileName
    End Function
#End Region
End Class
