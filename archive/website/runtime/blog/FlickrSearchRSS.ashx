<%@ WebHandler Language="VB" Class="FlickrSearchRSS" %>

Imports System
Imports System.Web
Imports System.Xml
Imports System.IO
Imports System.Net
Imports WebProjectMechanics

Public Class FlickrSearchRSS : Implements IHttpHandler

    Dim SearchKeyword As String
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        If Not IsNothing(context.Request.QueryString.GetValues("q")) Then
            SearchKeyword = context.Request.QueryString.Item("q")
        Else
            SearchKeyword = "travel"
        End If
        context.Response.ContentType = "text/xml"
        Dim mySB As New StringBuilder
        Dim uri As New Uri(wpm_RemoveHtml(String.Format("http://api.flickr.com/services/feeds/photos_public.gne?format=rss_200&lang=en-us&tags={0}", SearchKeyword)))
        Dim request As HttpWebRequest = DirectCast(WebRequest.Create(uri), HttpWebRequest)
        Dim page As String = String.Empty
        Try
            request.KeepAlive = False
            request.ProtocolVersion = HttpVersion.Version11
            request.Method = "GET"
            request.AllowAutoRedirect = True
            request.MaximumAutomaticRedirections = 10
            request.Timeout = CInt(New TimeSpan(0, 0, 10).TotalMilliseconds)
            request.UserAgent = "Mozilla/3.0 (compatible; My Browser/1.0)"
            Using response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
                Using responseStream As Stream = response.GetResponseStream()
                    Using readStream As New StreamReader(responseStream, Encoding.UTF8)
                        mySB.Append(readStream.ReadToEnd())
                        readStream.Close()
                    End Using
                    responseStream.Close()
                End Using
                response.Close()
            End Using
        Catch ex As Exception
        End Try
        mySB.Replace("l.php", "redirect.ashx")
        context.Response.Write(mySB.ToString)
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class