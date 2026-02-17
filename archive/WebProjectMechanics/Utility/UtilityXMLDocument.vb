Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Xml
Imports System.Xml.Xsl

Public Class UtilityXMLDocument
    Inherits XmlDocument
    Private _xmlFileName As String
    Private _xsltFileName As String

    Public Sub New()

    End Sub
    Public Sub New(ByVal nt As XmlNameTable)
        MyBase.New(nt)
    End Sub
    Sub New(ByVal sXMLFileName As String, ByVal sXSLTFileName As String)
        FeedName = String.Empty
        xmlURL = String.Empty
        templatePath = String.Empty
        xsltPath = sXSLTFileName
        _xmlFileName = sXMLFileName
    End Sub
    ''' <summary>
    ''' Initializes a new instance of the UtilityXMLDocument class
    ''' </summary>
    '''  
    Sub New(ByVal sFeedName As String, ByVal sURL As String, ByVal sXSLTPath As String, ByVal sTemplatePath As String)
        FeedName = sFeedName
        xmlURL = sURL
        xsltPath = sXSLTPath
        templatePath = sTemplatePath
        _xmlFileName = getXMLFileName()
    End Sub
    Protected Friend Sub New(ByVal imp As XmlImplementation)
        MyBase.New(imp)
    End Sub

    Public Property FeedName() As String
    Public Property templatePath() As String
    Public Property xmlURL() As String
    Public Property xsltPath() As String

    ''' <summary>
    ''' Get the MD5 Hash of a String
    ''' </summary>
    ''' <param name="strToHash">The String to Hash</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function getMD5Hash(ByVal strToHash As String) As String
        Dim md5Obj As New System.Security.Cryptography.MD5CryptoServiceProvider
        Dim bytesToHash() As Byte = System.Text.Encoding.ASCII.GetBytes(strToHash)
        bytesToHash = md5Obj.ComputeHash(bytesToHash)
        Dim strResult As String = String.Empty
        For Each b As Byte In bytesToHash
            strResult += b.ToString("x2")
        Next
        Return strResult
    End Function
    Public Function getXMLTransform() As String
        Dim myXslDoc As New XslCompiledTransform()
        Dim myStringBuilder As New StringBuilder()
        Using myStringWriter As New StringWriter(myStringBuilder)
            If LoadXML() Then
                Try
                    If FileProcessing.IsValidPath(xsltPath) Then
                        _xsltFileName = xsltPath
                    Else
                        If FileProcessing.IsValidPath(HttpContext.Current.Server.MapPath((String.Format("{0}xsl/{1}", templatePath, xsltPath)))) Then
                            _xsltFileName = HttpContext.Current.Server.MapPath((String.Format("{0}xsl/{1}", templatePath, xsltPath)))
                        ElseIf FileProcessing.IsValidPath(HttpContext.Current.Server.MapPath((String.Format("{0}style/{1}", wpm_SiteConfig.ApplicationHome(), xsltPath)))) Then
                            _xsltFileName = HttpContext.Current.Server.MapPath((String.Format("{0}style/{1}", wpm_SiteConfig.ApplicationHome(), xsltPath)))
                        Else
                            _xsltFileName = HttpContext.Current.Server.MapPath((wpm_SiteConfig.ApplicationHome() & "style/rss_title.xsl"))
                        End If
                    End If
                    myXslDoc.Load(_xsltFileName)
                    myXslDoc.Transform(getXMLFileName(), Nothing, myStringWriter)
                Catch ex As Exception
                    '' ApplicationLogging.XMLLog(String.Format("UtilityXMLDocument.getXMLTransform XSL/XML - ({0}) - {1}", _xsltFileName, ex.Message), String.Format("xml url ({0})", xmlURL))
                End Try
            End If
        End Using
        Return myStringBuilder.ToString()
    End Function
    Private Shared Function requestpost(ByVal TheURL As String, ByVal SavePath As String) As String
        Dim uri As New Uri(wpm_RemoveHtml(TheURL))
        Dim request As HttpWebRequest = DirectCast(WebRequest.Create(uri), HttpWebRequest)
        Dim page As String = String.Empty
        Try
            request.KeepAlive = False
            request.ProtocolVersion = HttpVersion.Version11
            request.Method = "GET"
            request.AllowAutoRedirect = True
            request.MaximumAutomaticRedirections = 10
            request.Timeout = CInt(New TimeSpan(0, 0, 10).TotalMilliseconds)
            request.UserAgent = "Mozilla/3.0 (compatible; My Browser/1.0)"
            Using response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
                Using responseStream As Stream = response.GetResponseStream()
                    Using readStream As New StreamReader(responseStream, Encoding.UTF8)
                        FileProcessing.CreateFile(SavePath, readStream.ReadToEnd())
                        readStream.Close()
                    End Using
                    responseStream.Close()
                End Using
                response.Close()
            End Using
        Catch ex As Exception
            '' ApplicationLogging.XMLLog(String.Format("UtilityXMLDocument.requestpost  theURL - ({0}) - save Path {1}", TheURL, SavePath), ex.Message)
            page = String.Empty
        End Try
        Return page
    End Function

    Private Function getXMLFileName() As String
        If Not FileProcessing.VerifyFolderExists(String.Format("{0}sites", wpm_SiteConfig.ConfigFolderPath)) Then
            FileProcessing.CreateFolder(String.Format("{0}sites", wpm_SiteConfig.ConfigFolderPath))
        End If

        If Not FileProcessing.VerifyFolderExists(String.Format("{0}sites\{1}", wpm_SiteConfig.ConfigFolderPath, wpm_HostName)) Then
            FileProcessing.CreateFolder(String.Format("{0}sites\{1}", wpm_SiteConfig.ConfigFolderPath, wpm_HostName()))
        End If

        If Not FileProcessing.VerifyFolderExists(String.Format("{0}sites\{1}\xml", wpm_SiteConfig.ConfigFolderPath, wpm_HostName())) Then
            FileProcessing.CreateFolder(String.Format("{0}sites\{1}\xml", wpm_SiteConfig.ConfigFolderPath, wpm_HostName()))
        End If

        If Not String.IsNullOrEmpty(FeedName) Then
            _xmlFileName = String.Format("{0}sites\{1}\xml\{2}.xml", wpm_SiteConfig.ConfigFolderPath, wpm_HostName(), getMD5Hash(xmlURL))
        End If
        Return _xmlFileName
    End Function
    Private Overloads Function LoadXML() As Boolean
        Dim path As String = getXMLFileName()
        Dim bReturn As Boolean = True
        Try
            If Not String.IsNullOrEmpty(xmlURL) And (FileProcessing.GetFileAgeMinutes(path) > wpm_SiteConfig.MaxXMLFeedAge) Then
                requestpost(xmlURL, path)
            End If
        Catch ex As Exception
            '' ApplicationLogging.XMLLog(String.Format("UtilityXMLDocument.LoadXML  - {0}", xmlURL), ex.Message)
            bReturn = False
        End Try
        Return bReturn
    End Function
End Class
