Public Class mhHttpModule
    Implements IHttpModule

    Public Sub New()
    End Sub

    Public ReadOnly Property ModuleName() As String
        Get
            Return "mhHttpModule"
        End Get
    End Property

    ' In the Init function, register for HttpApplication 
    ' events by adding your handlers. 
    Public Sub Init(ByVal application As HttpApplication) _
            Implements IHttpModule.Init
        AddHandler application.BeginRequest, _
            AddressOf Me.Application_BeginRequest
    End Sub

    Private Sub Application_BeginRequest(ByVal source As Object, _
             ByVal e As EventArgs)
        ' Create HttpApplication and HttpContext objects to access 
        ' request and response properties. 
        Dim application As HttpApplication = DirectCast(source, _
            HttpApplication)
        Dim context As HttpContext = application.Context
        Dim rule As String = ConfigurationManager.AppSettings.[Get]("WwwRule")
        Dim url As String = context.Request.Url.ToString()
        If url.Contains("://www.") AndAlso rule = "remove" Then
            RemoveWww(context)
        End If
        If Not url.Contains("://www.") AndAlso rule = "add" Then
            AddWww(context)
        End If

    End Sub


    ''' <summary>
    ''' Adds the www subdomain to the request and redirects.
    ''' </summary>
    Private Shared Sub AddWww(ByVal context As HttpContext)
        Dim url As String = context.Request.Url.ToString()
        If InStr(context.Request.RawUrl(), "404.aspx?404;") > 0 Then
            url = Replace(context.Request.RawUrl(), "/mhweb/404.aspx?404;", "")
        End If

        url = url.Replace("://", "://www.")
        PermanentRedirect(url, context)
    End Sub

    Private Shared ReadOnly _Regex As New Regex("(http|https)://www\.", RegexOptions.IgnoreCase Or RegexOptions.Compiled)

    ''' <summary>
    ''' Removes the www subdomain from the request and redirects.
    ''' </summary>
    Private Shared Sub RemoveWww(ByVal context As HttpContext)
        Dim url As String = context.Request.Url.ToString()

        If InStr(context.Request.RawUrl(), "404.aspx?404;") > 0 Then
            url = Replace(context.Request.RawUrl(), "/mhweb/404.aspx?404;", "")
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
            url = Replace(LCase(url), "default.aspx", String.Empty)
        End If

        context.Response.Clear()
        context.Response.StatusCode = 301
        context.Response.AppendHeader("location", url)
        context.Response.[End]()
    End Sub

    Public Sub Dispose() Implements System.Web.IHttpModule.Dispose
    End Sub
End Class
