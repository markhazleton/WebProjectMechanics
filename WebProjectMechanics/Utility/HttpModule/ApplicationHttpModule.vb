Imports System.Text.RegularExpressions
Public Class ApplicationHttpModule
    Implements IHttpModule
    Private bRedirect As Boolean = False
    Public Shared ReadOnly Property ModuleName() As String
        Get
            Return "ApplicationHttpModule"
        End Get
    End Property
    ' In the Init function, register for HttpApplication 
    ' events by adding your handlers. 
    Public Sub Init(ByVal application As HttpApplication) _
            Implements IHttpModule.Init
        AddHandler application.BeginRequest, _
            AddressOf Application_BeginRequest
    End Sub
    Private Sub Application_BeginRequest(ByVal source As Object, _
             ByVal e As EventArgs)
        ' Create HttpApplication and HttpContext objects to access 
        ' request and response properties. 
        Dim application As HttpApplication = DirectCast(source, HttpApplication)
        Dim context As HttpContext = application.Context
        Dim url As String = context.Request.Url.ToString()
        If url.Contains("://www.") AndAlso wpm_SiteConfig.RemoveWWW Then
            RemoveWww(context)
            bRedirect = True
        End If
        If Not url.Contains("://www.") AndAlso Not wpm_SiteConfig.RemoveWWW Then
            AddWww(context)
            bRedirect = True
        End If
    End Sub

    ''' <summary>
    ''' Adds the www subdomain to the request and redirects.
    ''' </summary>
    Private Shared Sub AddWww(ByVal context As HttpContext)
        Dim url As String
        If InStr(context.Request.RawUrl(), "404.aspx?404;") > 0 Then
            url = Replace(context.Request.RawUrl(), wpm_SiteConfig.ApplicationHome & "404.aspx?404;", String.Empty)
        Else
            url = context.Request.Url.ToString()
        End If
        url = url.Replace("://", "://www.")
        PermanentRedirect(url, context)
    End Sub
    Private Shared ReadOnly _Regex As New Regex("(http|https)://www\.", RegexOptions.IgnoreCase Or RegexOptions.Compiled)
    ''' <summary>
    ''' Removes the www subdomain from the request and redirects.
    ''' </summary>
    Private Shared Sub RemoveWww(ByVal context As HttpContext)
        Dim url As String
        If InStr(context.Request.RawUrl(), "404.aspx?404;") > 0 Then
            url = Replace(context.Request.RawUrl(), wpm_SiteConfig.ApplicationHome & "404.aspx?404;", String.Empty)
        Else
            url = context.Request.Url.ToString()
        End If
        If _Regex.IsMatch(url) Then
            url = _Regex.Replace(url, "$1://")
            PermanentRedirect(url, context)
        End If
    End Sub

    ''' <summary>
    ''' Sends permanent redirection headers (301)
    ''' </summary>
    Private Shared Sub PermanentRedirect(ByVal url As String, ByVal context As HttpContext)
        If (url.EndsWith("default.aspx", StringComparison.OrdinalIgnoreCase)) Then
            url = Replace((url.ToLower), "default.aspx", String.Empty)
        End If
        context.Response.Clear()
        context.Response.StatusCode = 301
        context.Response.AppendHeader("location", url)
        context.Response.[End]()
    End Sub

    Public Sub Dispose() Implements IHttpModule.Dispose
    End Sub
End Class
