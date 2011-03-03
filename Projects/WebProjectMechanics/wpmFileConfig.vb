Imports System.Web.Configuration


Public Class wpmFileConfig
    Inherits ConfigurationBase
#Region "Constructors"
    Public Sub New()
        MyBase.New(WebConfigurationManager.AppSettings("wpm_FileConfig"))
    End Sub
    Public Sub New(ByVal myConfigFilePath As String)
        MyBase.New(myConfigFilePath)
    End Sub

#End Region
#Region "Properties"
    Public ReadOnly Property ConfigFolderPath() As String
        Get
            Return HttpContext.Current.Server.MapPath(ConfigFolder)
        End Get
    End Property
    Public ReadOnly Property SiteListPath As String
        Get
            Return wpmApp.Config.ConfigFolderPath & GetSiteConfig("SiteListXML", "wpmSiteList.xml")
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
    Public Property ForceSSL() As Boolean
        Get
            Return CBool(GetSiteConfig("ForceSSL", CStr(False)))
        End Get
        Set(ByVal value As Boolean)
            SetSiteConfig("ForceSSL", CStr(value))
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
    Public Property ThowExceptionOnError() As Boolean
        Get
            Return CBool(GetSiteConfig("ThowExceptionOnError", CStr(True)))
        End Get
        Set(ByVal value As Boolean)
            SetSiteConfig("ThowExceptionOnError", CStr(value))
        End Set
    End Property
    Public Property FullDebuggingOn() As Boolean
        Get
            Return CBool(GetSiteConfig("FullDebuggingOn", CStr(False)))
        End Get
        Set(ByVal value As Boolean)
            SetSiteConfig("FullDebuggingOn", CStr(value))
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
