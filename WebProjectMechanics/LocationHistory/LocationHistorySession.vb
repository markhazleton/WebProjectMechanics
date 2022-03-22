Imports System.Text

Public Class LocationHistorySession
    Private ReadOnly _context As HttpContext
    Private ReadOnly _expires As Date
    Private _visitCount As Integer
    Private ReadOnly _userHostAddress As String
    Private ReadOnly _userAgent As String
    Private _originalReferrer As String
    Private _originalURL As String
    Private ReadOnly _sessionReferrer As String
    Private ReadOnly _sessionURL As String
    Private ReadOnly _browser As HttpBrowserCapabilities
    Private ReadOnly _LocationHistory As New LocationHistoryList()


    Public Sub New()
        'HttpContext.Current allows us to gain access to all 
        'the intrinsic ASP context objects like Request, Response, Session, etc
        _context = HttpContext.Current
        'provides a default expiration for cookies
        _expires = Now.AddYears(1)
        'load up the tracker
        IncrementVisitCount()
        _userHostAddress = _context.Request.UserHostAddress.ToString
        _userAgent = _context.Request.UserAgent.ToString
        If Not IsNothing(_context.Request.UrlReferrer) Then
            'set original referrer if not set
            SetOriginalReferrer(_context.Request.UrlReferrer.ToString)
            _sessionReferrer = _context.Request.UrlReferrer.ToString
        End If

        If Not IsNothing(_context.Request.Url) Then
            'set original url if not set
            SetOriginalURL(_context.Request.Url.ToString)
            _sessionURL = _context.Request.Url.ToString
        End If

        'set the browser capabilities
        _browser = _context.Request.Browser

    End Sub
    'add the page to the member arraylist 
    Public Sub AddPage(ByVal pageName As String)
        'create a new page tracker item 
        'set a time stamp
        'add the page tracker item to the array list
        _LocationHistory.Add(New LocationHistory() With {.PageName = pageName, .ViewTime = Now})
    End Sub

    'increment the visit count and save in a cookie
    Public Sub IncrementVisitCount()
        Const KEY As String = "VisitCount"

        'check is cookie has been set yet
        If IsNothing(_context.Request.Cookies.Get(KEY)) Then
            _visitCount = 1
        Else
            _visitCount = CInt(CDbl(_context.Request.Cookies.Get(KEY).Value) + 1)
        End If

        'set or reset the cookie
        addCookie(KEY, CStr(_visitCount))
    End Sub

    'set the original referrer to a cookie
    Public Sub SetOriginalReferrer(ByVal val As String)
        Const KEY As String = "OriginalReferrer"

        'check is cookie has been set yet
        If Not IsNothing(_context.Request.Cookies.Get(KEY)) Then
            _originalReferrer = _context.Request.Cookies.Get(KEY).Value
        Else
            addCookie(KEY, val)
            _originalReferrer = val
        End If
    End Sub

    'set the original url to a cookie
    Public Sub SetOriginalURL(ByVal val As String)
        Const KEY As String = "OriginalURL"

        'check is cookie has been set yet
        If Not IsNothing(_context.Request.Cookies.Get(KEY)) Then
            _originalURL = _context.Request.Cookies.Get(KEY).Value
        Else
            addCookie(KEY, val)
            _originalURL = val
        End If
    End Sub

    Private Sub addCookie(ByVal key As String, ByVal value As String)
        Dim cookie As HttpCookie = New HttpCookie(key, value) With {.Expires = _expires}
        _context.Response.Cookies.Set(cookie)
    End Sub

    'assemble the tracker properties into an HTML table
    Public Function createTrackerMessageBody() As String
        Dim body As New StringBuilder(String.Empty)
        body.Append("<table style='font-size:8pt; font-family:Verdana;'>")

        'link to ARIN Whois
        Dim ArinURL As String = String.Format("<a href='http://ws.arin.net/cgi-bin/whois.pl?queryinput={0}'>ARIN Whois</a>", SessionUserHostAddress)

        'link to RIPE Whois
        Dim RipeURL As String = String.Format("<a href='http://ripe.net/perl/whois?form_type=simple&full_query_string=&searchtext={0}'>RIPE Whois</a>", SessionUserHostAddress)

        'link to NSLookup
        Dim NsLookupURL As String = String.Format("<a href='http://www.zoneedit.com/lookup.html?ipaddress={0}&server=&reverse=Look+it+up'>NSLookup</a>", SessionUserHostAddress)

        body.Append("<tr><td><b>UserHostAddress:</b></td><td>" & SessionUserHostAddress)
        body.Append(String.Format("&nbsp;{0}&nbsp;{1}&nbsp;{2}</td></tr>{3}", ArinURL, RipeURL, NsLookupURL, vbCrLf))
        body.Append(String.Format("<tr><td><b>UserAgent:</b></td><td>{0}</td></tr>{1}", SessionUserAgent, vbCrLf))
        body.Append(String.Format("<tr><td><b>Browser:</b></td><td>{0}</td></tr>{1}", Browser.Browser, vbCrLf))
        body.Append(String.Format("<tr><td><b>Crawler:</b></td><td>{0}</td></tr>{1}", Browser.Crawler, vbCrLf))

        'line
        body.Append("<tr height=1 bgcolor=#CCCCCC><td></td><td></td></tr>" & vbCrLf)

        'Session URL
        Dim url As String = SessionURL
        body.Append(String.Format("<tr><td><b>URL</td><td><a href='{0}'>{0}</a></td></tr>{1}", url, vbCrLf))

        'Session Referrer
        If Not IsNothing(SessionReferrer) Then
            url = SessionReferrer
            body.Append(String.Format("<tr><td><b>Referer:</b></td><td><a href='{0}'>{0}</a></td></tr>{1}", url, vbCrLf))
        Else
            body.Append(String.Format("<tr><td><b>Referer:</b></td><td></td></tr>{0}", vbCrLf))
        End If

        'line
        body.Append("<tr height=1 bgcolor=#CCCCCC><td></td><td></td></tr>" & vbCrLf)

        body.Append(String.Format("<tr><td><b>visits:</b></td><td>{0}</td></tr>{1}", VisitCount, vbCrLf))
        body.Append(String.Format("<tr><td><b>orig referer:</b></td><td><a href='{0}'>{0}</a></td></tr>{1}", OriginalReferrer, vbCrLf))
        body.Append(String.Format("<tr><td><b>orig url:</b></td><td><a href='{0}'>{0}</a></td></tr>{1}", OriginalURL, vbCrLf))

        body.Append("<tr height=1 bgcolor=#CCCCCC><td></td><td></td></tr>" & vbCrLf)
        body.Append("</table>")

        Return body.ToString
    End Function

    'run through the session page history and get elapsed times for pages
    Public Function createTrackerPageListing() As String
        Dim ElapsedTime As TimeSpan
        Dim PreviousTime As Date
        Dim FirstTime As Date
        Dim first As Boolean = True
        Dim body As New StringBuilder(String.Empty)

        body.Append("<br><table style='font-size:8pt; font-family:Verdana;'>")
        If Not IsNothing(LocationHistory) Then
            Dim pti As LocationHistory
            Dim lastPage As String = String.Empty
            body.Append(String.Format("<tr><td><b>Visited Pages Count:</b></td><td>{0}</td></tr>{1}", LocationHistory.Count, vbCrLf))
            'line
            body.Append("<tr height=1 bgcolor=#CCCCCC><td></td><td></td></tr>" & vbCrLf)
            body.Append("<tr><td><b>Visited Pages</b></td><td><b>Elapsed Time</b></td></tr>" & vbCrLf)
            'line
            body.Append("<tr height=1 bgcolor=#CCCCCC><td></td><td></td></tr>" & vbCrLf)

            For Each pti In LocationHistory
                If first Then
                    FirstTime = pti.ViewTime
                    first = False
                Else
                    ElapsedTime = pti.ViewTime.Subtract(PreviousTime)
                    body.Append(String.Format("<tr><td>{0}</td><td>&nbsp;&nbsp;{1}</td></tr>{2}", lastPage, ElapsedTime.ToString.Substring(0, 8), vbCrLf))
                End If
                lastPage = pti.PageName
                PreviousTime = pti.ViewTime
            Next
            body.Append(String.Format("<tr><td>{0}</td><td></td></tr>{1}", lastPage, vbCrLf))
            ElapsedTime = PreviousTime.Subtract(FirstTime)
            body.Append(String.Format("<tr><td><b>Total Time:</b></td><td>&nbsp;&nbsp;<b>{0}</b></td></tr>{1}", ElapsedTime.ToString.Substring(0, 8), vbCrLf))

        Else
            body.Append("<tr><td><b>Visited Pages Count:</b></td><td>0</td></tr>" & vbCrLf)
        End If
        body.Append("</table>")

        Return body.ToString

    End Function

#Region "Properties"
    'Visit Count 
    ReadOnly Property VisitCount() As Integer
        Get
            Return _visitCount
        End Get
    End Property

    'Original Referrer 
    ReadOnly Property OriginalReferrer() As String
        Get
            Return _originalReferrer
        End Get
    End Property

    'Original URL 
    ReadOnly Property OriginalURL() As String
        Get
            Return _originalURL
        End Get
    End Property

    'Session Referrer
    ReadOnly Property SessionReferrer() As String
        Get
            Return _sessionReferrer
        End Get
    End Property

    'Session URL
    ReadOnly Property SessionURL() As String
        Get
            Return _sessionURL
        End Get
    End Property

    'Session User Host Address (IP)
    ReadOnly Property SessionUserHostAddress() As String
        Get
            Return _userHostAddress
        End Get
    End Property

    'Session User Agent
    ReadOnly Property SessionUserAgent() As String
        Get
            Return _userAgent
        End Get
    End Property

    'Pages - array list
    ReadOnly Property LocationHistory() As LocationHistoryList
        Get
            Return _LocationHistory
        End Get
    End Property

    'Browser Cap
    ReadOnly Property Browser() As HttpBrowserCapabilities
        Get
            Return _browser
        End Get
    End Property

#End Region

End Class
