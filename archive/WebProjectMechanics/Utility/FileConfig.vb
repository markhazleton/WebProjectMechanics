Public Class FileConfig
    Inherits ConfigurationBase
    Implements IConfig
#Region "Constructors"
    Public Sub New()
        MyBase.New(WebConfigurationManager.AppSettings("ProjectMechanicsConfig"))
    End Sub
    Public Sub New(ByVal myConfigFilePath As String)
        MyBase.New(myConfigFilePath)
    End Sub
#End Region
    Public ReadOnly Property ConfigFolderPath() As String Implements IConfig.ConfigFolderPath
        Get
            Return HttpContext.Current.Server.MapPath(ConfigFolder)
        End Get
    End Property
    Public Property ConfigFolder() As String Implements IConfig.ConfigFolder
        Get
            Return GetConfigValue("wpm_ConfigFolder", "/App_Data/")
        End Get
        Set(ByVal value As String)
            SetConfigValue("wpm_ConfigFolder", value)
        End Set
    End Property
    Public Property ApplicationHome() As String Implements IConfig.ApplicationHome
        Get
            Return GetConfigValue("wpm_ApplicationHome", "/runtime/")
        End Get
        Set(ByVal value As String)
            SetConfigValue("wpm_ApplicationHome", value)
        End Set
    End Property
    Public Property AdminFolder() As String Implements IConfig.AdminFolder
        Get
            Return GetConfigValue("wpm_AdminFolder", "/admin/")
        End Get
        Set(ByVal value As String)
            SetConfigValue("wpm_AdminFolder", value)
        End Set
    End Property
    Public Property AuditLogFile() As String Implements IConfig.AuditLogFile
        Get
            Return HttpContext.Current.Server.MapPath(GetConfigValue("wpm_AuditLogFile", "/App_Data/log/auditlog.csv"))
        End Get
        Set(ByVal value As String)
            SetConfigValue("wpm_AuditLogFile", value)
        End Set
    End Property
    Public Property SQLLogFile() As String Implements IConfig.SQLLogFile
        Get
            Return HttpContext.Current.Server.MapPath(GetConfigValue("wpm_SQLLogFile", "/App_Data/log/sqllog.csv"))
        End Get
        Set(ByVal value As String)
            SetConfigValue("wpm_SQLLogFile", value)
        End Set
    End Property
    Public Property AccessLogFile() As String Implements IConfig.AccessLogFile
        Get
            Return HttpContext.Current.Server.MapPath(GetConfigValue("wpm_AccessLogFile", "/App_Data/log/accesslog.csv"))
        End Get
        Set(ByVal value As String)
            SetConfigValue("wpm_AccessLogFile", value)
        End Set
    End Property
    Public Property RemoveWWW() As Boolean Implements IConfig.RemoveWWW
        Get
            Return CBool(GetConfigValue("wpm_RemoveWWW", CStr(False)))
        End Get
        Set(ByVal value As Boolean)
            SetConfigValue("wpm_RemoveWWW", CStr(value))
        End Set
    End Property
    Public Property DefaultExtension() As String Implements IConfig.DefaultExtension
        Get
            Dim myEXT As String = GetConfigValue("wpm_DefaultExtension", "none")
            If myEXT = "none" Then
                Return String.Empty
            Else
                Return myEXT
            End If
        End Get
        Set(ByVal value As String)
            SetConfigValue("wpm_DefaultExtension", value)
        End Set
    End Property
    Public Property MaxXMLFeedAge() As Double Implements IConfig.MaxXMLFeedAge
        Get
            Return CDbl((GetConfigValue("wpm_MaxXMLFeedAge", "600")))
        End Get
        Set(ByVal value As Double)
            SetConfigValue("wpm_MaxXMLFeedAge", CStr(value))
        End Set
    End Property
    Public Property ForceSSL() As Boolean Implements IConfig.ForceSSL
        Get
            Return CBool(GetConfigValue("wpm_ForceSSL", CStr(False)))
        End Get
        Set(ByVal value As Boolean)
            SetConfigValue("wpm_ForceSSL", CStr(value))
        End Set
    End Property
    Public Property Use404Processing() As Boolean Implements IConfig.Use404Processing
        Get
            Return CBool(GetConfigValue("wpm_Use404Processing", CStr(False)))
        End Get
        Set(ByVal value As Boolean)
            SetConfigValue("wpm_Use404Processing", CStr(value))
        End Set
    End Property
    Public Property FullLoggingOn() As Boolean Implements IConfig.FullLoggingOn
        Get
            Return CBool(GetConfigValue("wpm_FullLoggingOn", CStr(False)))
        End Get
        Set(ByVal value As Boolean)
            SetConfigValue("wpm_FullLoggingOn", CStr(value))
        End Set
    End Property
    Public Property ThowExceptionOnError() As Boolean Implements IConfig.ThowExceptionOnError
        Get
            Return CBool(GetConfigValue("wpm_ThowExceptionOnError", CStr(True)))
        End Get
        Set(ByVal value As Boolean)
            SetConfigValue("wpm_ThowExceptionOnError", CStr(value))
        End Set
    End Property
    Public Property FullDebuggingOn() As Boolean Implements IConfig.FullDebuggingOn
        Get
            Return CBool(GetConfigValue("wpm_FullDebuggingOn", CStr(False)))
        End Get
        Set(ByVal value As Boolean)
            SetConfigValue("wpm_FullDebuggingOn", CStr(value))
        End Set
    End Property
    Public Property CachingEnabled() As Boolean Implements IConfig.CachingEnabled
        Get
            Return CBool(GetConfigValue("wpm_CachingEnabled", CStr(False)))
        End Get
        Set(ByVal value As Boolean)
            SetConfigValue("wpm_CachingEnabled", CStr(value))
        End Set
    End Property
    Public Property CommonFolder() As String Implements IConfig.CommonFolder
        Get
            Return GetConfigValue("wpm_CommonFolder", "\")
        End Get
        Set(ByVal value As String)
            SetConfigValue("wpm_CommonFolder", value)
        End Set
    End Property

End Class
