Imports System.Xml.Serialization
Imports System.Web.Configuration
Imports System.Web
Imports System.Xml

Public Class App
    Public Shared Config As wpmFileConfig
    Public Shared SiteList As wpmSiteList
    Shared Sub New()
        Config = New wpmFileConfig()
        SiteList = LoadSiteList()
    End Sub

    Private Shared Function LoadSiteList() As wpmSiteList
        Dim mySiteList As wpmSiteList
        If wpmFileIO.IsValidPath(App.Config.ConfigFolderPath & "wpmSiteList.xml") Then
            Dim sr As New StreamReader(App.Config.ConfigFolderPath & "wpmSiteList.xml")
            Try
                Dim xs As New XmlSerializer(GetType(wpmSiteList))
                mySiteList = DirectCast(xs.Deserialize(sr), wpmSiteList)
            Finally
                sr.Close()
            End Try
        Else
            mySiteList = New wpmSiteList
        End If
        Return mySiteList
    End Function
    Private Shared Sub SaveSiteList(ByVal fname As String, ByVal obj As wpmSiteList)
        Dim sw As New StreamWriter(fname)
        Try
            Dim xs As New XmlSerializer(GetType(wpmSiteList))
            xs.Serialize(sw, obj)
        Finally
            sw.Close()
        End Try
    End Sub

End Class

Public Class wpmConfig

    Public Shared ReadOnly Property wpmConfigFile() As String
        Get
            If Not IsNothing(WebConfigurationManager.AppSettings.Item("wpmConfigFolder")) Then
                Return HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings.Item("wpmConfigFolder")) & Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", "") & ".xml"
            Else
                Return HttpContext.Current.Server.MapPath("~/access_db/") & Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", "") & ".xml"
            End If
        End Get
    End Property
    Public Shared ReadOnly Property ConnStr() As String
        Get
            Return App.SiteList.GetSiteByDomain(Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", "")).SQLDBConnString
        End Get
    End Property
End Class


Public Class wpmFileConfig
    Inherits ConfigurationBase
#Region "Constructors"
    Public Sub New()
        MyBase.New(WebConfigurationManager.AppSettings("wpm_FileConfig"))
    End Sub
#End Region
#Region "Properties"
    Public ReadOnly Property ConfigFolderPath() As String
        Get
            Return HttpContext.Current.Server.MapPath(ConfigFolder)
        End Get
    End Property
    Public Property ConfigFolder() As String
        Get
            Return GetSiteConfig("ConfigFolder", "/access_db/")
        End Get
        Set(ByVal value As String)
            SetSiteConfig("ConfigFolder", value)
        End Set
    End Property
    Public Property wpmWebHome() As String
        Get
            Return GetSiteConfig("wpmHome", "/wpm/")
        End Get
        Set(ByVal value As String)
            SetSiteConfig("wpmHome", value)
        End Set
    End Property
    Public Property AspMakerGen() As String
        Get
            Return GetSiteConfig("AspMakerGen", "/wpmgen/")
        End Get
        Set(ByVal value As String)
            SetSiteConfig("AspMakerGen", value)
        End Set
    End Property
    Public Property AuditLogFile() As String
        Get
            Return HttpContext.Current.Server.MapPath(GetSiteConfig("AuditLogFile", "/access_db/log/auditlog.csv"))
        End Get
        Set(ByVal value As String)
            SetSiteConfig("AuditLogFile", value)
        End Set
    End Property
    Public Property SQLLogFile() As String
        Get
            Return HttpContext.Current.Server.MapPath(GetSiteConfig("SQLLogFile", "/access_db/log/sqllog.csv"))
        End Get
        Set(ByVal value As String)
            SetSiteConfig("SQLLogFile", value)
        End Set
    End Property
    Public Property AccessLogFile() As String
        Get
            Return HttpContext.Current.Server.MapPath(GetSiteConfig("AccessLogFile", "/access_db/log/accesslog.csv"))
        End Get
        Set(ByVal value As String)
            SetSiteConfig("AccessLogFile", value)
        End Set
    End Property
    Public Property RemoveWWW() As Boolean
        Get
            Return CBool(GetSiteConfig("RemoveWWW", CStr(False)))
        End Get
        Set(ByVal value As Boolean)
            SetSiteConfig("RemoveWWW", CStr(value))
        End Set
    End Property
    Public Property DefaultExtension() As String
        Get
            Dim myEXT As String
            myEXT = GetSiteConfig("DefaultExtension", "none")
            If myEXT = "none" Then
                Return String.Empty
            Else
                Return myEXT
            End If
        End Get
        Set(ByVal value As String)
            SetSiteConfig("DefaultExtension", value)
        End Set
    End Property
    Public Property MaxXMLFeedAge() As Double
        Get
            Return CDbl((GetSiteConfig("MaxXMLFeedAge", "600")))
        End Get
        Set(ByVal value As Double)
            SetSiteConfig("MaxXMLFeedAge", CStr(value))
        End Set
    End Property
    Public Property Use404Processing() As Boolean
        Get
            Return CBool(GetSiteConfig("Use404Processing", CStr(False)))
        End Get
        Set(ByVal value As Boolean)
            SetSiteConfig("Use404Processing", CStr(value))
        End Set
    End Property
    Public Property FullLoggingOn() As Boolean
        Get
            Return CBool(GetSiteConfig("FullLoggingOn", CStr(False)))
        End Get
        Set(ByVal value As Boolean)
            SetSiteConfig("FullLoggingOn", CStr(value))
        End Set
    End Property
    Public Property CachingEnabled() As Boolean
        Get
            Return CBool(GetSiteConfig("CachingEnabled", CStr(False)))
        End Get
        Set(ByVal value As Boolean)
            SetSiteConfig("CachingEnabled", CStr(value))
        End Set
    End Property
    Public Property CommonFolder() As String
        Get
            Return GetSiteConfig("wpm_RootFolder", "\")
        End Get
        Set(ByVal value As String)
            SetSiteConfig("wpm_RootFolder", value)
        End Set
    End Property
#End Region
#Region "Methods"

#End Region

End Class



Public Class ConfigurationBase
    Private myDoc As XmlDocument
#Region "Constructors"
    Public Sub New(ByVal myConfigFilePath As String)
        myDoc = New XmlDocument()
        myDoc.Load(HttpContext.Current.Server.MapPath(myConfigFilePath))
        ConfigFilePath = HttpContext.Current.Server.MapPath(myConfigFilePath)
    End Sub
#End Region
    Dim _ConfigFilePath As String
    Private Property ConfigFilePath() As String
        Get
            Return _ConfigFilePath
        End Get
        Set(ByVal value As String)
            _ConfigFilePath = value
        End Set
    End Property
    Private ReadOnly Property ConfigFile() As String
        Get
            If Not IsNothing(WebConfigurationManager.AppSettings.Item("wpmConfigFolder")) Then
                Return HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings.Item("wpmConfigFolder")) & Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", "") & "_config.xml"
            Else
                Return HttpContext.Current.Server.MapPath("~/access_db/") & Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", "") & "_config.xml"
            End If
        End Get
    End Property

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
        End If
    End Sub
End Class


