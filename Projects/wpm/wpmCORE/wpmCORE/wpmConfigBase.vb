Imports System.Xml
Imports System.Web.Configuration
Imports System.Web

Public Class ConfigurationBase
#Region "Constructors"
    Public Sub New(ByVal myConfigFilePath As String)
        myDoc = New XmlDocument()
        ConfigFilePath = HttpContext.Current.Server.MapPath(myConfigFilePath)
        Try
            myDoc.Load(ConfigFilePath)
        Catch
            Try
                MakeXML(ConfigFilePath)
                myDoc.Load(ConfigFilePath)
            Catch ex As Exception
                wpmUTIL.WriteLog("Can't Create Config File: " & ConfigFilePath, ex.ToString, HttpContext.Current.Server.MapPath("error.csv"))
            End Try
        End Try
    End Sub
#End Region
#Region "Properties"
    Private myDoc As XmlDocument
    Dim _ConfigFilePath As String
    Private Property ConfigFilePath() As String
        Get
            Return _ConfigFilePath
        End Get
        Set(ByVal value As String)
            _ConfigFilePath = value
        End Set
    End Property
#End Region
#Region "Functions"
    Public Function GetSiteConfig(ByVal ConfigName As String) As String
        If myDoc IsNot Nothing Then
            Try
                Return myDoc.GetElementsByTagName(ConfigName).Item(0).ChildNodes.Item(0).Value
            Catch ex As Exception
                SetSiteConfig(ConfigName, String.Empty)
                Return String.Empty
            End Try
        Else
            Return String.Empty
        End If
    End Function
    Public Function GetSiteConfig(ByVal ConfigName As String, ByVal ConfigValaue As String) As String
        Dim myReturn As String
        myReturn = GetSiteConfig(ConfigName)
        If myReturn = String.Empty Then
            SetSiteConfig(ConfigName, ConfigValaue)
            myReturn = ConfigValaue
        End If
        HttpContext.Current.Application(ConfigName) = myReturn
        Return myReturn
    End Function
    Public Sub SetSiteConfig(ByVal ConfigName As String, ByVal ConfigValue As String)
        If myDoc IsNot Nothing Then
            Dim myElement As XmlElement
            Dim myRoot As XmlElement = myDoc.DocumentElement
            Dim myConfig As XmlNodeList = myDoc.GetElementsByTagName(ConfigName)
            Try
                If myConfig.Count = 0 Then
                    myElement = myDoc.CreateElement(ConfigName)
                    myElement.InnerText = ConfigValue
                    myRoot.AppendChild(myElement)
                Else
                    myConfig.Item(0).InnerText = ConfigValue
                End If
                wpmUTIL.AuditLog("Change Site Config:" & ConfigName, ConfigValue)
                myDoc.Save(ConfigFilePath())
                HttpContext.Current.Application(ConfigName) = ConfigValue
            Catch ex As Exception
                wpmUTIL.WriteLog("ErrorSetting Value: " & ConfigFilePath, ex.ToString, HttpContext.Current.Server.MapPath("error.csv"))
            End Try
        End If
    End Sub

    Private Function MakeXML(ByVal myFileName As String) As Boolean
        Dim xmlDoc As New XmlDocument()
        Dim xmlDeclaration As XmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", Nothing)
        Dim rootNode As XmlElement = xmlDoc.CreateElement("Configuration")
        xmlDoc.InsertBefore(xmlDeclaration, xmlDoc.DocumentElement)
        xmlDoc.AppendChild(rootNode)
        xmlDoc.Save(myFileName)
        Return True
    End Function
#End Region
End Class



'Public Class ConfigurationBase
'    Private myDoc As XmlDocument
'#Region "Constructors"
'    Public Sub New(ByVal myConfigFilePath As String)
'        myDoc = New XmlDocument()
'        myDoc.Load(HttpContext.Current.Server.MapPath(myConfigFilePath))
'        ConfigFilePath = HttpContext.Current.Server.MapPath(myConfigFilePath)
'    End Sub
'#End Region
'    Dim _ConfigFilePath As String
'    Private Property ConfigFilePath() As String
'        Get
'            Return _ConfigFilePath
'        End Get
'        Set(ByVal value As String)
'            _ConfigFilePath = value
'        End Set
'    End Property
'    Private ReadOnly Property ConfigFile() As String
'        Get
'            If Not IsNothing(WebConfigurationManager.AppSettings.Item("wpmConfigFolder")) Then
'                Return HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings.Item("wpmConfigFolder")) & Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", "") & "_config.xml"
'            Else
'                Return HttpContext.Current.Server.MapPath("~/access_db/") & Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", "") & "_config.xml"
'            End If
'        End Get
'    End Property

'    Public Function GetSiteConfig(ByVal ConfigName As String) As String
'        If myDoc IsNot Nothing Then
'            Try
'                Return myDoc.GetElementsByTagName(ConfigName).Item(0).ChildNodes.Item(0).Value
'            Catch ex As Exception
'                SetSiteConfig(ConfigName, String.Empty)
'                Return String.Empty
'            End Try
'        Else
'            Return String.Empty
'        End If
'    End Function
'    Public Function GetSiteConfig(ByVal ConfigName As String, ByVal ConfigValaue As String) As String
'        Dim myReturn As String
'        myReturn = GetSiteConfig(ConfigName)
'        If myReturn = String.Empty Then
'            SetSiteConfig(ConfigName, ConfigValaue)
'            myReturn = ConfigValaue
'        End If
'        HttpContext.Current.Application(ConfigName) = myReturn

'        Return myReturn
'    End Function

'    Public Sub SetSiteConfig(ByVal ConfigName As String, ByVal ConfigValue As String)
'        If myDoc IsNot Nothing Then
'            Dim myElement As XmlElement
'            Dim myRoot As XmlElement = myDoc.DocumentElement
'            Dim myConfig As XmlNodeList = myDoc.GetElementsByTagName(ConfigName)
'            If myConfig.Count = 0 Then
'                myElement = myDoc.CreateElement(ConfigName)
'                myElement.InnerText = ConfigValue
'                myRoot.AppendChild(myElement)
'            Else
'                myConfig.Item(0).InnerText = ConfigValue
'            End If
'            wpmUTIL.AuditLog("Change Site Config:" & ConfigName, ConfigValue)
'            myDoc.Save(ConfigFilePath())
'            HttpContext.Current.Application(ConfigName) = ConfigValue
'        End If
'    End Sub



'    '    Private myDoc As XmlDocument
'    '#Region "Constructors"
'    '    Public Sub New(ByVal myConfigFilePath As String)
'    '        myDoc = New XmlDocument()
'    '        myDoc.Load(HttpContext.Current.Server.MapPath(myConfigFilePath))
'    '        ConfigFilePath = HttpContext.Current.Server.MapPath(myConfigFilePath)
'    '    End Sub
'    '#End Region
'    '    Dim _ConfigFilePath As String
'    '    Private Property ConfigFilePath() As String
'    '        Get
'    '            Return _ConfigFilePath
'    '        End Get
'    '        Set(ByVal value As String)
'    '            _ConfigFilePath = value
'    '        End Set
'    '    End Property
'    '    Private ReadOnly Property ConfigFile() As String
'    '        Get
'    '            If Not IsNothing(WebConfigurationManager.AppSettings.Item("wpmConfigFolder")) Then
'    '                Return HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings.Item("wpmConfigFolder")) & Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", "") & "_config.xml"
'    '            Else
'    '                Return HttpContext.Current.Server.MapPath("~/access_db/") & Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", "") & "_config.xml"
'    '            End If
'    '        End Get
'    '    End Property

'    '    Public Function GetSiteConfig(ByVal ConfigName As String) As String
'    '        If myDoc IsNot Nothing Then
'    '            Try
'    '                Return myDoc.GetElementsByTagName(ConfigName).Item(0).ChildNodes.Item(0).Value
'    '            Catch ex As Exception
'    '                SetSiteConfig(ConfigName, String.Empty)
'    '                Return String.Empty
'    '            End Try
'    '        Else
'    '            Return String.Empty
'    '        End If
'    '    End Function
'    '    Public Function GetSiteConfig(ByVal ConfigName As String, ByVal ConfigValaue As String) As String
'    '        Dim myReturn As String
'    '        myReturn = GetSiteConfig(ConfigName)
'    '        If myReturn = String.Empty Then
'    '            SetSiteConfig(ConfigName, ConfigValaue)
'    '            myReturn = ConfigValaue
'    '        End If
'    '        HttpContext.Current.Application(ConfigName) = myReturn

'    '        Return myReturn
'    '    End Function

'    '    Public Sub SetSiteConfig(ByVal ConfigName As String, ByVal ConfigValue As String)
'    '        If myDoc IsNot Nothing Then
'    '            Dim myElement As XmlElement
'    '            Dim myRoot As XmlElement = myDoc.DocumentElement
'    '            Dim myConfig As XmlNodeList = myDoc.GetElementsByTagName(ConfigName)
'    '            If myConfig.Count = 0 Then
'    '                myElement = myDoc.CreateElement(ConfigName)
'    '                myElement.InnerText = ConfigValue
'    '                myRoot.AppendChild(myElement)
'    '            Else
'    '                myConfig.Item(0).InnerText = ConfigValue
'    '            End If
'    '            wpmUTIL.AuditLog("Change Site Config:" & ConfigName, ConfigValue)
'    '            myDoc.Save(ConfigFilePath())
'    '            HttpContext.Current.Application(ConfigName) = ConfigValue
'    '        End If
'    '    End Sub
'End Class
