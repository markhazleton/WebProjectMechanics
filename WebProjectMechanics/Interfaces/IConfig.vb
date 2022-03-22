Public Interface IConfig
    ReadOnly Property ConfigFolderPath() As String
    Property ConfigFolder() As String
    Property ApplicationHome() As String
    Property AdminFolder() As String
    Property AuditLogFile() As String
    Property SQLLogFile() As String
    Property AccessLogFile() As String
    Property RemoveWWW() As Boolean
    Property DefaultExtension() As String
    Property MaxXMLFeedAge() As Double
    Property ForceSSL() As Boolean
    Property Use404Processing() As Boolean
    Property FullLoggingOn() As Boolean
    Property ThowExceptionOnError() As Boolean
    Property FullDebuggingOn() As Boolean
    Property CachingEnabled() As Boolean
    Property CommonFolder() As String
End Interface
