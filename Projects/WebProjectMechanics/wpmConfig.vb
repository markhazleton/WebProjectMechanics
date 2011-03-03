Imports System.Web
Imports System.Web.Configuration
Public Class wpmConfig
    Public Shared mySite As wpmSite
    Public Shared ReadOnly Property wpmConfigFile() As String
        Get
            If Not IsNothing(WebConfigurationManager.AppSettings.Item("wpmConfigFolder")) Then
                Return String.Format("{0}{1}.xml", HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings.Item("wpmConfigFolder")), Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", ""))
            Else
                Return String.Format("{0}{1}.xml", HttpContext.Current.Server.MapPath("~/access_db/"), Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", ""))
            End If
        End Get
    End Property
    Public Shared ReadOnly Property ConnStr() As String
        Get
            mySite = wpmApp.SiteList.FindSiteByDomain(Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", ""))
            If Not (mySite.AccessDatabasePath Is Nothing) AndAlso (mySite.AccessDatabasePath.Trim <> String.Empty) Then
                Return String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};", HttpContext.Current.Server.MapPath(mySite.AccessDatabasePath))
            Else
                Return mySite.SQLDBConnString
                'Return "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=c:\SVN-work\wpm\web\access_db\wpm-demo.mdb;"
                ' Return App.SiteList.GetSiteByDomain(Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", "")).SQLDBConnString ' 
            End If
        End Get
    End Property

End Class
