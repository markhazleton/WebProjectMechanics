Imports System.Net
Imports WebProjectMechanics
Imports System.Web.Script.Serialization


Partial Class WpmAdminMaster
    Inherits ApplicationMasterPage
    ReadOnly Property SiteName As String
        Get
            Return wpm_CurrentSiteName
        End Get
    End Property
    Public Property Themes As Dictionary(Of String, String)
        Get
            If Application("siteThemes") Is Nothing Then
                Application("siteThemes") = GetSiteThemes()
            End If
            Return TryCast(Application("siteThemes"), Dictionary(Of String, String))
        End Get
        Set(value As Dictionary(Of String, String))
            Application("siteThemes") = value
        End Set
    End Property
    Public ReadOnly Property ThemeCSS As String
        Get
            Try
                Return String.Format("<link rel='stylesheet' href='{0}' >", Themes(ThemeName))
            Catch ex As Exception
                Return String.Format("<link rel='stylesheet' href='{0}' >", "https://maxcdn.bootstrapcdn.com/bootswatch/latest/yeti/bootstrap.min.css")
            End Try
        End Get
    End Property
    Public Property ThemeName As String
        Get
            Return Application("ThemeName")
        End Get
        Set(value As String)
            Application("ThemeName") = value
        End Set
    End Property

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not wpm_IsAdmin And Not Page.IsCallback Then
            wpm_ListPageURL = Request.Url.AbsoluteUri
            Response.Redirect(String.Format("{0}login/login.aspx", wpm_SiteConfig.AdminFolder))
        End If
        If String.IsNullOrEmpty(ThemeName) Then
            ThemeName = "Yeti"
        End If
    End Sub

    Private Function GetSiteThemes() As Dictionary(Of String, String)
        Dim SiteThemes As New Dictionary(Of String, String)
        Try

            Dim myClient As New WebClient()
            Dim myJSONstring = myClient.DownloadString("http://bootswatch.com/api/3.json")

            Dim jss As New JavaScriptSerializer()
            For Each myTheme As Theme In jss.Deserialize(Of RootObject)(myJSONstring).themes
                SiteThemes.Add(myTheme.name, myTheme.cssCdn)
            Next
        Catch ex As Exception

        End Try
        Return SiteThemes
    End Function


    Public Class Theme
        Public Property name() As String
            Get
                Return m_name
            End Get
            Set
                m_name = Value
            End Set
        End Property
        Private m_name As String
        Public Property description() As String
            Get
                Return m_description
            End Get
            Set
                m_description = Value
            End Set
        End Property
        Private m_description As String
        Public Property thumbnail() As String
            Get
                Return m_thumbnail
            End Get
            Set
                m_thumbnail = Value
            End Set
        End Property
        Private m_thumbnail As String
        Public Property preview() As String
            Get
                Return m_preview
            End Get
            Set
                m_preview = Value
            End Set
        End Property
        Private m_preview As String
        Public Property css() As String
            Get
                Return m_css
            End Get
            Set
                m_css = Value
            End Set
        End Property
        Private m_css As String
        Public Property cssMin() As String
            Get
                Return m_cssMin
            End Get
            Set
                m_cssMin = Value
            End Set
        End Property
        Private m_cssMin As String
        Public Property cssCdn() As String
            Get
                Return m_cssCdn
            End Get
            Set
                m_cssCdn = Value
            End Set
        End Property
        Private m_cssCdn As String
        Public Property less() As String
            Get
                Return m_less
            End Get
            Set
                m_less = Value
            End Set
        End Property
        Private m_less As String
        Public Property lessVariables() As String
            Get
                Return m_lessVariables
            End Get
            Set
                m_lessVariables = Value
            End Set
        End Property
        Private m_lessVariables As String
        Public Property scss() As String
            Get
                Return m_scss
            End Get
            Set
                m_scss = Value
            End Set
        End Property
        Private m_scss As String
        Public Property scssVariables() As String
            Get
                Return m_scssVariables
            End Get
            Set
                m_scssVariables = Value
            End Set
        End Property
        Private m_scssVariables As String
    End Class

    Public Class RootObject
        Public Property version() As String
            Get
                Return m_version
            End Get
            Set
                m_version = Value
            End Set
        End Property
        Private m_version As String
        Public Property themes() As List(Of Theme)
            Get
                Return m_themes
            End Get
            Set
                m_themes = Value
            End Set
        End Property
        Private m_themes As List(Of Theme)
    End Class


End Class

