
Public Class SiteConfig
    Implements IConfig

    Public Property AccessLogFile As String Implements IConfig.AccessLogFile
    Public Property AdminFolder As String Implements IConfig.AdminFolder
    Public Property ApplicationHome As String Implements IConfig.ApplicationHome
    Public Property AuditLogFile As String Implements IConfig.AuditLogFile
    Public Property CachingEnabled As Boolean Implements IConfig.CachingEnabled
    Public Property CommonFolder As String Implements IConfig.CommonFolder
    Public Property ConfigFolder As String Implements IConfig.ConfigFolder
    Public ReadOnly Property ConfigFolderPath As String Implements IConfig.ConfigFolderPath
        Get
            Return HttpContext.Current.Server.MapPath(ConfigFolder)
        End Get
    End Property
    Public Property DefaultExtension As String Implements IConfig.DefaultExtension
    Public Property ForceSSL As Boolean Implements IConfig.ForceSSL
    Public Property FullDebuggingOn As Boolean Implements IConfig.FullDebuggingOn
    Public Property FullLoggingOn As Boolean Implements IConfig.FullLoggingOn
    Public Property MaxXMLFeedAge As Double Implements IConfig.MaxXMLFeedAge
    Public Property RemoveWWW As Boolean Implements IConfig.RemoveWWW
    Public Property SQLLogFile As String Implements IConfig.SQLLogFile
    Public Property ThowExceptionOnError As Boolean Implements IConfig.ThowExceptionOnError
    Public Property Use404Processing As Boolean Implements IConfig.Use404Processing

    Public Sub New()
        With New FileConfig()
            ConfigFolder = .ConfigFolder
            AdminFolder = .AdminFolder
            ApplicationHome = .ApplicationHome
            CachingEnabled = .CachingEnabled
            DefaultExtension = .DefaultExtension
            FullLoggingOn = .FullLoggingOn
            MaxXMLFeedAge = .MaxXMLFeedAge
            RemoveWWW = .RemoveWWW
            Use404Processing = .Use404Processing
            AccessLogFile = .AccessLogFile
            AuditLogFile = .AuditLogFile
            CommonFolder = .CommonFolder
            ForceSSL = .ForceSSL
            FullDebuggingOn = .FullDebuggingOn
            SQLLogFile = .SQLLogFile
            ThowExceptionOnError = .ThowExceptionOnError
        End With
    End Sub
End Class