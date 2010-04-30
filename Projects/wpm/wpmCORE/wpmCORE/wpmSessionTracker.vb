Imports System.Text
Imports System.Web

Public Class wpmSessionTracker
    Private _context As HttpContext
    Private _expires As Date
    Private _visitCount As Integer
    Private _userHostAddress As String
    Private _userAgent As String
    Private _originalReferrer As String
    Private _originalURL As String
    Private _sessionReferrer As String
    Private _sessionURL As String
    Private _browser As HttpBrowserCapabilities
    Private _pages As New ArrayList()

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
        Dim pti As New wpmSessionTrackerPage()
        pti.PageName = pageName
        'set a time stamp
        pti.Time = Now

        'add the page tracker item to the array list
        _pages.Add(pti)
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
        Dim cookie As HttpCookie
        cookie = New HttpCookie(key, value)
        cookie.Expires = _expires
        _context.Response.Cookies.Set(cookie)
    End Sub

    'assemble the tracker properties into an HTML table
    Public Function createTrackerMessageBody() As String
        Dim body As New StringBuilder(String.Empty)
        body.Append("<table style='font-size:8pt; font-family:Verdana;'>")

        'link to ARIN Whois
        Dim ArinURL As String = "<a href='http://ws.arin.net/cgi-bin/whois.pl?queryinput=" & SessionUserHostAddress & "'>ARIN Whois</a>"

        'link to RIPE Whois
        Dim RipeURL As String = "<a href='http://ripe.net/perl/whois?form_type=simple&full_query_string=&searchtext=" & SessionUserHostAddress & "'>RIPE Whois</a>"

        'link to NSLookup
        Dim NsLookupURL As String = "<a href='http://www.zoneedit.com/lookup.html?ipaddress=" & SessionUserHostAddress & "&server=&reverse=Look+it+up'>NSLookup</a>"

        body.Append("<tr><td><b>UserHostAddress:</b></td><td>" & SessionUserHostAddress)
        body.Append("&nbsp;" & ArinURL & "&nbsp;" & RipeURL & "&nbsp;" & NsLookupURL & "</td></tr>" & vbCrLf)
        body.Append("<tr><td><b>UserAgent:</b></td><td>" & SessionUserAgent & "</td></tr>" & vbCrLf)
        body.Append("<tr><td><b>Browser:</b></td><td>" & Browser.Browser & "</td></tr>" & vbCrLf)
        body.Append("<tr><td><b>Crawler:</b></td><td>" & Browser.Crawler & "</td></tr>" & vbCrLf)

        'line
        body.Append("<tr height=1 bgcolor=#CCCCCC><td></td><td></td></tr>" & vbCrLf)

        'Session URL
        Dim url As String = SessionURL
        body.Append("<tr><td><b>URL</td><td><a href='" & url & "'>" & url & "</a></td></tr>" & vbCrLf)

        'Session Referrer
        If Not IsNothing(SessionReferrer) Then
            url = SessionReferrer
            body.Append("<tr><td><b>Referer:</b></td><td><a href='" & url & "'>" & url & "</a></td></tr>" & vbCrLf)
        Else
            body.Append("<tr><td><b>Referer:</b></td><td>" & "</td></tr>" & vbCrLf)
        End If

        'line
        body.Append("<tr height=1 bgcolor=#CCCCCC><td></td><td></td></tr>" & vbCrLf)

        body.Append("<tr><td><b>visits:</b></td><td>" & VisitCount.ToString & "</td></tr>" & vbCrLf)
        body.Append("<tr><td><b>orig referer:</b></td><td><a href='" & OriginalReferrer & "'>" & OriginalReferrer & "</a></td></tr>" & vbCrLf)
        body.Append("<tr><td><b>orig url:</b></td><td><a href='" & OriginalURL & "'>" & OriginalURL & "</a></td></tr>" & vbCrLf)

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
        If Not IsNothing(Pages) Then
            Dim pti As wpmSessionTrackerPage
            Dim lastPage As String = ""
            body.Append("<tr><td><b>Visited Pages Count:</b></td><td>" & Pages.Count & "</td></tr>" & vbCrLf)
            'line
            body.Append("<tr height=1 bgcolor=#CCCCCC><td></td><td></td></tr>" & vbCrLf)
            body.Append("<tr><td><b>Visited Pages</b></td><td><b>Elapsed Time</b></td></tr>" & vbCrLf)
            'line
            body.Append("<tr height=1 bgcolor=#CCCCCC><td></td><td></td></tr>" & vbCrLf)

            For Each pti In Pages
                If first Then
                    FirstTime = pti.Time
                    first = False
                Else
                    ElapsedTime = pti.Time.Subtract(PreviousTime)
                    body.Append("<tr><td>" & lastPage & "</td><td>&nbsp;&nbsp;" & ElapsedTime.ToString.Substring(0, 8) & "</td></tr>" & vbCrLf)
                End If
                lastPage = pti.PageName
                PreviousTime = pti.Time
            Next
            body.Append("<tr><td>" & lastPage & "</td><td></td></tr>" & vbCrLf)
            ElapsedTime = PreviousTime.Subtract(FirstTime)
            body.Append("<tr><td><b>Total Time:</b></td><td>&nbsp;&nbsp;<b>" & ElapsedTime.ToString.Substring(0, 8) & "</b></td></tr>" & vbCrLf)

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
    ReadOnly Property Pages() As ArrayList
        Get
            Return _pages
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
