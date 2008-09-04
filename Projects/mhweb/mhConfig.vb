Public Class mhConfig
    Public Shared Function GetSiteDB() As String
        Dim SiteConfig As MHSiteSettings = MHSiteSettings.Load(mhConfig.mhWebConfig)
        Return (SiteConfig.mhSite.SQLDBConnString.ToString)
        '  Can't use session object because ASPMAKER generated code grabs the connection string early
        '  in the page lifecycle, BEFORE the session object is complete. 
        'Return HttpContext.Current.Session.Item("SiteDB").ToString
    End Function
    Public Shared ReadOnly Property Use404Processing() As Boolean
        Get
            Dim bReturn As Boolean = False
            If IsNothing(WebConfigurationManager.AppSettings.Item("Use404Processing")) Then
                bReturn = False
            Else
                If (WebConfigurationManager.AppSettings.Item("Use404Processing").ToString = "1") Then
                    bReturn = True
                Else
                    bReturn = False
                End If
            End If
            Return bReturn
        End Get
    End Property
    Public Shared ReadOnly Property mhWebHome() As String
        Get
            If IsNothing(WebConfigurationManager.AppSettings.Item("mhWebHome")) Then
                Return HttpContext.Current.Server.MapPath("~/mhweb/")
            Else
                Return WebConfigurationManager.AppSettings.Item("mhWebHome").ToString
            End If
        End Get
    End Property
    Public Shared ReadOnly Property mhASPMakerGen() As String
        Get
            If IsNothing(WebConfigurationManager.AppSettings.Item("mhASPMakerGen")) Then
                Return "/aspmaker/"
            Else
                Return WebConfigurationManager.AppSettings.Item("mhASPMakerGen").ToString
            End If
        End Get
    End Property
    Public Shared ReadOnly Property mhWebConfigFolder() As String
        Get
            Return WebConfigurationManager.AppSettings.Item("configfolder")
        End Get
    End Property
    Public Shared ReadOnly Property DefaultExtension() As String
        Get
            If IsNothing(WebConfigurationManager.AppSettings.Item("DefaultExtension")) Then
                Return ".html"
            Else
                Return WebConfigurationManager.AppSettings.Item("DefaultExtension")
            End If
        End Get
    End Property
    Public Shared ReadOnly Property CacheState() As Boolean
        Get
            Dim bReturn As Boolean = False
            If IsNothing(WebConfigurationManager.AppSettings.Item("cachestate")) Then
                bReturn = False
            Else
                If (WebConfigurationManager.AppSettings.Item("cachestate").ToString = "1") Then
                    bReturn = True
                Else
                    bReturn = False
                End If
            End If
            Return bReturn
        End Get
    End Property
    Public Shared ReadOnly Property mhSQLLOG() As String
        Get
            Return WebConfigurationManager.AppSettings.Item("sqllog")
        End Get
    End Property
    Public Shared ReadOnly Property mhAuditLog() As String
        Get
            Return WebConfigurationManager.AppSettings.Item("auditlog")
        End Get
    End Property
    Public Shared ReadOnly Property mhWebConfigURL() As String
        Get
            Return WebConfigurationManager.AppSettings.Item("configURL")
        End Get
    End Property
    Public Shared ReadOnly Property mhWebConfig() As String
        Get
            If Not IsNothing(WebConfigurationManager.AppSettings.Item("configfolder")) Then
                Return WebConfigurationManager.AppSettings.Item("configfolder").ToString() & Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", "") & ".xml"
            Else
                Return HttpContext.Current.Server.MapPath("~/access_db/") & Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", "") & ".xml"
            End If
        End Get
    End Property
    Public Shared ReadOnly Property mhAccessLog() As String
        Get
            Return WebConfigurationManager.AppSettings.Item("accesslog")
        End Get
    End Property

    Public Shared ReadOnly Property ConnStr() As String
        Get
            Return GetSiteDB()
        End Get
    End Property

End Class

