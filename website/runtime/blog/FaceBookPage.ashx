<%@ WebHandler Language="VB" Class="FaceBookPage" %>

Imports System
Imports System.Web
Imports System.Xml
Imports System.IO
Imports System.Net
Imports WebProjectMechanics

Public Class FaceBookPage : Implements IHttpHandler

    Dim PageGUID As String
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        If Not IsNothing(context.Request.QueryString.GetValues("id")) Then
            PageGUID = context.Request.QueryString.Item("id")
        Else
            PageGUID = "185839381457608"
        End If
        context.Response.ContentType = "text/xml"
        Dim mySB As New StringBuilder
        Dim uri As New Uri(wpm_RemoveHtml(String.Format("http://www.facebook.com/feeds/page.php?format=rss20&id={0}", PageGUID)))
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
        
        mySB.Replace("<a href=""/", "<a href=""https://www.facebook.com/")
        mySB.Replace("https://www.facebook.com/l.php?u=", "/redirect.ashx?u=")
        
        context.Response.Write(mySB.ToString)
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class