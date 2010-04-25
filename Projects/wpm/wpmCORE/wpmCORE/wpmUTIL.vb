Imports System.Web
Imports System.Text.RegularExpressions

Public Class wpmUTIL
    Public Shared Function CheckForMatch(ByVal StringOne As String, ByVal StringTwo As String) As Boolean
        Dim bMatch As Boolean = False
        ' To Make this Easier, let's ignore case and spaces and apmersands and dashes
        StringOne = (StringOne.ToLower)
        StringOne = Replace(StringOne, "/", "")
        StringOne = Replace(StringOne, ".html", "")
        StringOne = Replace(StringOne, ".htm", "")
        StringOne = Replace(StringOne, "&amp;", "&")
        StringOne = Replace(StringOne, "%20", "")
        StringOne = Replace(StringOne, "-", "")
        StringOne = Replace(StringOne, " ", "")
        StringOne = Replace(StringOne, App.Config.DefaultExtension, "")
        StringTwo = (StringTwo.ToLower)
        StringTwo = Replace(StringTwo, "/", "")
        StringTwo = Replace(StringTwo, ".html", "")
        StringTwo = Replace(StringTwo, ".htm", "")
        StringTwo = Replace(StringTwo, "%20", "")
        StringTwo = Replace(StringTwo, " ", "")
        StringTwo = Replace(StringTwo, "&amp;", "&")
        StringTwo = Replace(StringTwo, "-", "")
        StringTwo = Replace(StringTwo, App.Config.DefaultExtension, "")
        If (StringOne = StringTwo) Then
            bMatch = True
        Else
            bMatch = False
        End If
        Return bMatch
    End Function
    Public Shared Function ConvertRichTextToHTML(ByVal sTextToCovert As String) As String
        Dim sbReturn As New StringBuilder(HttpContext.Current.Server.HtmlEncode(sTextToCovert))
        sbReturn.Replace("'", "&#39;")
        sbReturn.Replace("  ", " &nbsp;")
        sbReturn.Replace(" & ", " &amp; ")
        sbReturn.Replace(vbCrLf, "<br/>")
        sbReturn.Replace(vbLf, "<br/>")
        Return sbReturn.ToString
    End Function
    Public Shared Function ClearLineFeeds(ByVal sTextToCovert As String) As String
        sTextToCovert = sTextToCovert.Replace(vbCrLf, "")
        sTextToCovert = sTextToCovert.Replace(vbLf, "")
        Return sTextToCovert
    End Function

    Public Shared Function RemoveInvalidCharacters(ByVal Value As String) As String
        Value = (Trim(Value.ToLower))
        Value = Replace(Value, " & ", String.Empty)
        Value = Replace(Value, "&", String.Empty)
        Value = Replace(Value, " | ", String.Empty)
        Value = Replace(Value, "|", String.Empty)
        Value = Replace(Value, ",", String.Empty)
        Value = Replace(Value, "/", String.Empty)
        Value = Replace(Value, "\", String.Empty)
        Value = Replace(Value, "<", String.Empty)
        Value = Replace(Value, ">", String.Empty)
        Value = Replace(Value, "(", String.Empty)
        Value = Replace(Value, ")", String.Empty)
        Value = Replace(Value, ",", String.Empty)
        Value = Replace(Value, "'", String.Empty)
        Value = Replace(Value, ";", String.Empty)
        Value = Replace(Value, ":", String.Empty)
        Value = Replace(Value, " ", String.Empty)
        Value = Replace(Value, "?", String.Empty)
        Value = Replace(Value, "%22", String.Empty)
        Value = Replace(Value, "=", String.Empty)
        Return Value
    End Function
    Public Shared Function FixInvalidCharacters(ByVal Value As String) As String
        Value = (Trim(Value.ToLower))
        Value = Replace(Value, " & ", "-and-")
        Value = Replace(Value, "&", "-and-")
        Value = Replace(Value, " | ", "-and-")
        Value = Replace(Value, "|", "-and-")
        Value = Replace(Value, ",", "-")
        Value = Replace(Value, "/", "-")
        Value = Replace(Value, "\", "-")
        Value = Replace(Value, "<", "-")
        Value = Replace(Value, ">", "-")
        Value = Replace(Value, "(", "-")
        Value = Replace(Value, ")", "-")
        Value = Replace(Value, ",", "-")
        Value = Replace(Value, "'", "-")
        Value = Replace(Value, ";", "-")
        Value = Replace(Value, ":", "-")
        Value = Replace(Value, " ", "-")
        Return Value
    End Function
    Public Shared Function RemoveHtml(ByVal sContent As String) As String
        Return System.Text.RegularExpressions.Regex.Replace(sContent, "<[^>]*>", "")
    End Function
    Public Shared Function RemoveTags(ByVal sContent As String) As String
        Return System.Text.RegularExpressions.Regex.Replace(sContent, "~~(.|\n)+?~~", "")
    End Function
    Public Shared Function FormatNameForURL(ByVal Name As String) As String
        Name = (Replace(Name.ToLower, " ", "-"))
        Name = Replace(Name, "(", "-")
        Name = Replace(Name, ")", "-")
        Return Name
        ' Return FixInvalidCharacters(Name)
    End Function
    Public Shared Function ApplyHTMLFormatting(ByVal strInput As String) As String
        strInput = "~" & strInput
        strInput = Replace(strInput, ",", "-")
        strInput = Replace(strInput, "'", "&quot;")
        strInput = Replace(strInput, """", "&quot;")
        strInput = Replace(strInput, "~", "")
        '  strInput = Replace(strInput, " " , "_")
        ApplyHTMLFormatting = strInput
    End Function
    '********************************************************************************
    Public Shared Function GetDBString(ByRef dbObject As Object) As String
        Dim strEntry As String = String.Empty
        If Not (IsDBNull(dbObject) Or dbObject Is Nothing) Then
            strEntry = dbObject.ToString
            If strEntry = " " Then strEntry = String.Empty
            '  strEntry = Replace(strEntry, "''", "'")
            '  strEntry = Replace(strEntry, "'", "&#39;")
            '  strEntry = Replace(strEntry, """", "&#39;")
        End If
        Return strEntry
    End Function
    Public Shared Function GetStringValue(ByRef myString As String) As String
        If (IsDBNull(myString) Or myString Is Nothing) Then
            myString = String.Empty
        End If
        Return myString
    End Function

    '********************************************************************************
    Public Shared Function GetDBDate(ByRef dbObject As Object) As Date
        If IsDBNull(dbObject) Then
            Return Now.AddDays(-100)
        Else
            Return CDate(dbObject)
        End If
    End Function
    Public Shared Function GetDBDouble(ByRef dbObject As Object) As Double
        If IsDBNull(dbObject) Then
            Return Nothing
        ElseIf dbObject Is Nothing Then
            Return Nothing
        Else
            Return CDbl(dbObject)
        End If
    End Function
    Public Shared Function GetDBInteger(ByRef dbObject As Object) As Integer
        If IsDBNull(dbObject) Then
            Return Nothing
        ElseIf dbObject Is Nothing Then
            Return Nothing
        Else
            Return CInt(dbObject)
        End If
    End Function
    Public Shared Function GetDBBoolean(ByRef dbObject As Object) As Boolean
        If IsDBNull(dbObject) Then
            Return False
        Else
            Return CBool(dbObject)
        End If
    End Function
    Public Shared Function FormatPageNameForURL(ByVal PageName As String) As String
        If PageName Is Nothing Then
            Return Nothing
        Else
            Return (wpmUTIL.FixInvalidCharacters(PageName.ToLower))
        End If
    End Function
    ' ****************************************************************************
    Public Shared Function LeadingZero(ByRef pStrValue As String) As String
        If Len(CStr(pStrValue)) < 2 Then pStrValue = "0" & pStrValue
        Return pStrValue
    End Function

    Public Shared Function fncGetDayOrdinal(ByVal intDay As Integer) As String
        ' Accepts a day of the month as an integer and returns the
        ' appropriate suffix
        Dim strOrd As String = ("")
        Select Case intDay
            Case 1, 21, 31
                strOrd = "st"
            Case 2, 22
                strOrd = "nd"
            Case 3, 23
                strOrd = "rd"
            Case Else
                strOrd = "th"
        End Select
        Return strOrd
    End Function ' fncGetDayOrdinal


    Public Shared Function GetRFC822Date(ByVal dbObject As Object) As String
        Dim offset As Integer = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours
        Dim timeZone__1 As String = "+" & offset.ToString().PadLeft(2, "0"c)
        Dim myDate As Date
        If IsDBNull(dbObject) Then
            myDate = Now.AddDays(-100)
        Else
            myDate = CDate(dbObject)
        End If

        If offset < 0 Then
            Dim i As Integer = offset * -1
            timeZone__1 = "-" & i.ToString().PadLeft(2, "0"c)
        End If
        Return myDate.ToString("ddd, dd MMM yyyy HH:mm:ss " & timeZone__1.PadRight(5, "0"c))
    End Function

    Public Shared Function fncFmtDate(ByVal strDate As String, ByRef strFormat As String) As String
        ' Accepts strDate as a valid date/time,
        ' strFormat as the output template.
        ' The function finds each item in the
        ' template and replaces it with the
        ' relevant information extracted from strDate

        ' Template items (example)
        ' %m Month as a decimal (02)
        ' %B Full month name (February)
        ' %b Abbreviated month name (Feb )
        ' %d Day of the month (23)
        ' %O Ordinal of day of month (eg st or rd or nd)
        ' %j Day of the year (54)
        ' %Y Year with century (1998)
        ' %y Year without century (98)
        ' %w Weekday as integer (0 is Sunday)
        ' %a Abbreviated day name (Fri)
        ' %A Weekday Name (Friday)
        ' %H Hour in 24 hour format (24)
        ' %h Hour in 12 hour format (12)
        ' %N Minute as an integer (01)
        ' %n Minute as optional if minute <> 0
        ' %S Second as an integer (55)
        ' %P AM/PM Indicator (PM)

        On Error Resume Next
        Dim mySB As New StringBuilder(strFormat)
        Dim intPosItem As Integer
        Dim int12HourPart As New Integer
        Dim str24HourPart As String = ("")
        Dim strMinutePart As String = ("")
        Dim strSecondPart As String = ("")
        Dim strAMPM As String = ("")

        ' Insert Month Numbers
        mySB.Replace("%m", DatePart("m", strDate).ToString)

        ' Insert non-Abbreviated Month Names
        mySB.Replace("%B", MonthName(DatePart("m", strDate), False))

        ' Insert Abbreviated Month Names
        mySB.Replace("%b", MonthName(DatePart("m", strDate), True))

        ' Insert Day Of Month
        mySB.Replace("%d", DatePart("d", strDate).ToString)

        ' Insert Day of Month Ordinal (eg st, th, or rd)
        mySB.Replace("%O", fncGetDayOrdinal(Day(CDate(strDate))))

        ' Insert Day of Year
        mySB.Replace("%j", DatePart("y", CDate(strDate)).ToString)

        ' Insert Long Year (4 digit)
        mySB.Replace("%Y", DatePart("yyyy", CDate(strDate)).ToString)

        ' Insert Short Year (2 digit)
        mySB.Replace("%y", Right(DatePart("yyyy", CDate(strDate)).ToString, 2))

        ' Insert Weekday as Integer (eg 0 = Sunday)
        mySB.Replace("%w", DatePart("w", CDate(strDate), Microsoft.VisualBasic.FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1).ToString)

        ' Insert Abbreviated Weekday Name (eg Sun)
        mySB.Replace("%a", WeekdayName(DatePart("w", strDate, Microsoft.VisualBasic.FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1), True))

        ' Insert non-Abbreviated Weekday Name
        mySB.Replace("%A", WeekdayName(DatePart("w", strDate, Microsoft.VisualBasic.FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1), False))

        ' Insert Hour in 24hr format
        str24HourPart = DatePart("h", CDate(strDate)).ToString
        If Len(str24HourPart) < 2 Then
            str24HourPart = "0" & str24HourPart
        End If
        mySB.Replace("%H", str24HourPart)

        ' Insert Hour in 12hr format
        int12HourPart = DatePart("h", strDate) Mod 12
        If int12HourPart = 0 Then
            int12HourPart = 12
        End If
        mySB.Replace("%h", int12HourPart.ToString)

        ' Insert Minutes
        strMinutePart = DatePart("n", CDate(strDate)).ToString
        If Len(strMinutePart) < 2 Then
            strMinutePart = "0" & strMinutePart
        End If
        mySB.Replace("%N", strMinutePart)

        ' Insert Optional Minutes
        If CInt(strMinutePart) = 0 Then
            mySB.Replace("%n", "")
        Else
            If CInt(strMinutePart) < 10 Then
                strMinutePart = "0" & strMinutePart
            End If
            strMinutePart = ":" & strMinutePart
            mySB.Replace("%n", strMinutePart)
        End If

        ' Insert Seconds
        strSecondPart = DatePart("s", CDate(strDate)).ToString
        If Len(strSecondPart) < 2 Then _
                 strSecondPart = "0" & strSecondPart
        mySB.Replace("%S", strSecondPart)

        ' Insert AM/PM indicator
        If DatePart("h", strDate) >= 12 Then
            strAMPM = "PM"
        Else
            strAMPM = "AM"
        End If

        mySB.Replace("%P", strAMPM)

        'If there is an error output its value
        If Err.Number <> 0 Then
            mySB.Append("ERROR in fncFmtDate: " & Err.Description)
        End If
        Return mySB.ToString
    End Function ' fncFmtDate
    Public Shared Function IsBreak(ByVal hInput As Object, ByVal hBreakPoint As Double) As Boolean
        ' remove anything after the decimal point and the decimal point if found
        ' because we need a whole number to use Mod
        If InStr(hInput.ToString, ".") <> 0 Then hInput = Left(hInput.ToString, InStrRev(hInput.ToString, ".") - 1)
        ' If not numeric, exit with a Null value
        If Not IsNumeric(hInput) Then
            IsBreak = False
            Exit Function
        End If
        ' Add one to adjust for starting at zero with arrays
        ' zero would always be the break point
        hInput = CInt(hInput) + 1
        ' use the Mod function to get the remainder of dividing the input by the break point. If it's 0, the
        ' number is we are NOT at a break point, If it's 1, the we ARE at a break point.
        If CBool(CInt(hInput) Mod hBreakPoint) Then
            IsBreak = False
        Else
            IsBreak = True
        End If
    End Function
    Public Shared Function FormatGraphicLink(ByVal LinkID As Object, ByVal LinkName As Object, ByVal LinkType As Object, ByVal LinkURL As String, ByVal sImage As String) As String
        Dim sReturn As String
        sReturn = "<a href=""" & LinkURL & """><img alt=""" & sImage & """ border=0 src=""/wpm/images/" & sImage & ".gif""  /></a>"
        FormatGraphicLink = sReturn
    End Function
    Public Shared Function FormatLink(ByVal LinkID As String, ByVal LinkName As String, ByVal LinkType As String, ByVal LinkURL As String) As String
        Dim sReturn As String
        Select Case LinkType
            Case "Contact"
                sReturn = LinkName
            Case Else
                sReturn = "<a href=""" & LinkURL & """>" & LinkName & "</a>"
        End Select
        FormatLink = sReturn
    End Function
    Public Shared Function FormatLink(ByVal LinkID As String, ByVal LinkName As String, ByVal LinkType As String, ByVal LinkURL As String, ByVal LinkDescription As String) As String
        Dim sReturn As String
        Select Case LinkType
            Case "Contact"
                sReturn = LinkName
            Case Else
                sReturn = "<a href=""" & LinkURL & """ title=""" & LinkDescription & """>" & LinkName & "</a>"
        End Select
        FormatLink = sReturn
    End Function
    Public Shared Function FormatPageNameLink(ByVal sPageName As String) As String
        Dim sReturn As String = ""
        If Trim(sPageName) = "" Then
            sReturn = "<a href=""/"">Home Page</a>"
        Else
            sReturn = "<a href=""" & FormatPageNameURL(sPageName) & """>" & sPageName & "</a>"
        End If
        FormatPageNameLink = (sReturn.ToLower)
    End Function
    Public Shared Function FormatPageNameURL(ByVal sPageName As String) As String
        If Trim(sPageName) = "" Then
            FormatPageNameURL = "/"
        Else
            FormatPageNameURL = "/" & wpmUTIL.FixInvalidCharacters(sPageName) & App.Config.DefaultExtension()
        End If
    End Function
    Public Shared Function FormatTextEntry(ByVal strEntry As String) As String
        If strEntry = "" Then strEntry = " "
        strEntry = Replace(strEntry, "'", "&#39;")
        Return strEntry
    End Function
    Public Shared Function InternetTime() As String
        Dim lLngTime As Single
        Dim lLngBeats As Single
        Dim lLngBeatsRound As Double
        lLngTime = (Hour(TimeOfDay) * 3600 * 1000) + (Minute(TimeOfDay) * 60 * 1000) + (Second(TimeOfDay) * 1000 + 3600000)
        lLngBeats = lLngTime / 86400
        lLngBeatsRound = System.Math.Round(lLngBeats)
        InternetTime = "@" & lLngBeatsRound
        If lLngBeatsRound > 1000 Then InternetTime = "@0" & lLngBeatsRound
        If lLngBeatsRound > 100 Then InternetTime = "@0" & lLngBeatsRound
    End Function ' InternetTime()
    Public Shared Function LeadingZero(ByRef pStrValue As Integer) As String
        If Len(CStr(pStrValue)) < 2 Then
            Return "0" & pStrValue.ToString
        Else
            Return pStrValue.ToString
        End If
    End Function ' LeadingZero(ByRef pStrValue)
    Public Shared Function IsLeap() As Boolean
        Dim lLngYear As Integer = Year(Now)
        If (lLngYear Mod 4 = 0) And (lLngYear Mod 100 <> 0) Or (lLngYear Mod 400 = 0) Then
            Return True
        Else
            Return False
        End If
    End Function ' IsLeap()
    Public Shared Function DaysInMonth() As String
        Dim lLngMonth As Integer
        lLngMonth = Month(Now)
        Select Case lLngMonth
            Case 9, 4, 6, 11
                DaysInMonth = "30"
            Case 2
                If CBool(IsLeap()) Then DaysInMonth = "29" Else DaysInMonth = "28"
            Case Else
                DaysInMonth = "31"
        End Select
    End Function ' DaysInMonth()
    Public Shared Function FormatHour() As Integer
        Dim lDtmNow As String
        lDtmNow = FormatDateTime(Now, DateFormat.LongTime)
        Return CInt(Left(lDtmNow, InStr(lDtmNow, ":") - 1))
    End Function ' FormatHour()
    Public Shared Function OrdinalSuffix() As String
        Dim lLngDay As Integer
        lLngDay = Microsoft.VisualBasic.Day(Now)
        OrdinalSuffix = "th"
        If Right(lLngDay.ToString, 1) = "1" Then OrdinalSuffix = "st"
        If Right(lLngDay.ToString, 1) = "2" Then OrdinalSuffix = "nd"
        If Right(lLngDay.ToString, 1) = "3" Then OrdinalSuffix = "rd"
    End Function ' OrdinalSuffix()
    Public Shared Function GetCurrentDate() As String
        Dim dRightNow As DateTime = DateTime.Now
        GetCurrentDate = dRightNow.ToLongDateString
    End Function
    Public Shared Function FormatDate(ByVal lDtmNow As Date, ByRef pStrDate As String) As String
        ' Define local variables
        Dim lObjRegExp As Regex
        Dim lLngHour As Integer = Hour(lDtmNow)
        Dim lLngWeekDay As Integer = Weekday(lDtmNow)
        Dim lLngSecond As Integer = Second(lDtmNow)
        Dim lLngMinute As Integer = Minute(lDtmNow)
        Dim lLngDay As Integer = Microsoft.VisualBasic.Day(lDtmNow)
        Dim lLngMonth As Integer = Month(lDtmNow)
        Dim lLngYear As Integer = Year(lDtmNow)

        ' Prepare RegExp object and set parameters
        lObjRegExp = New Regex("([a-z][a-z]*[a-z])*[a-z]", RegexOptions.IgnoreCase)

        ' List each individual match and compare to different date functoids
        For Each lObjMatch As Match In lObjRegExp.Matches(pStrDate)
            Select Case lObjMatch.Value
                Case "a"
                    pStrDate = Replace(pStrDate, "a", (Right(lDtmNow.ToString.ToLower, 2)))
                Case "A"
                    pStrDate = Replace(pStrDate, "A", (Right(lDtmNow.ToString.ToUpper, 2)))
                Case "B"
                    pStrDate = Replace(pStrDate, "B", InternetTime())
                Case "d"
                    pStrDate = Replace(pStrDate, "d", LeadingZero(lLngDay))
                Case "D"
                    pStrDate = Replace(pStrDate, "D", Left(WeekdayName(lLngWeekDay), 3))
                Case "M"
                    pStrDate = Replace(pStrDate, "M", MonthName(lLngMonth))
                Case "g"
                    pStrDate = Replace(pStrDate, "g", FormatHour().ToString)
                Case "G"
                    pStrDate = Replace(pStrDate, "G", lLngHour.ToString)
                Case "h"
                    pStrDate = Replace(pStrDate, "h", LeadingZero(FormatHour()))
                Case "H"
                    pStrDate = Replace(pStrDate, "H", LeadingZero(lLngHour))
                Case "i"
                    pStrDate = Replace(pStrDate, "i", LeadingZero(lLngMinute))
                Case "j"
                    pStrDate = Replace(pStrDate, "j", lLngDay.ToString)
                Case "l"
                    pStrDate = Replace(pStrDate, "l", WeekdayName(lLngWeekDay))
                Case "L"
                    pStrDate = Replace(pStrDate, "L", IsLeap().ToString)
                Case "m"
                    pStrDate = Replace(pStrDate, "m", LeadingZero(lLngMonth))
                Case "M"
                    pStrDate = Replace(pStrDate, "M", Left(MonthName(lLngMonth), 3))
                Case "n"
                    pStrDate = Replace(pStrDate, "n", lLngMonth.ToString)
                Case "r"
                    pStrDate = Replace(pStrDate, "r", Left(WeekdayName(lLngWeekDay), 3) & ", " & lLngDay & " " & Left(MonthName(lLngMonth), 3) & " " & lLngYear & " " & FormatDateTime(TimeOfDay, DateFormat.LongTime) & ":" & LeadingZero(lLngSecond))
                Case "s"
                    pStrDate = Replace(pStrDate, "s", LeadingZero(lLngSecond))
                Case "S"
                    pStrDate = Replace(pStrDate, "S", OrdinalSuffix())
                Case "t"
                    pStrDate = Replace(pStrDate, "t", DaysInMonth())
                Case "U"
                    pStrDate = Replace(pStrDate, "U", CStr(DateDiff(Microsoft.VisualBasic.DateInterval.Second, DateSerial(1970, 1, 1), lDtmNow)))
                Case "w"
                    pStrDate = Replace(pStrDate, "w", CStr(lLngWeekDay - 1))
                Case "W"
                    pStrDate = Replace(pStrDate, "W", "1")
                Case "Y"
                    pStrDate = Replace(pStrDate, "Y", lLngYear.ToString)
                Case "y"
                    pStrDate = Replace(pStrDate, "y", Right(lLngYear.ToString, 2))
                Case "z"
                    pStrDate = Replace(pStrDate, "z", "1")
                Case Else
                    pStrDate = pStrDate & ""
            End Select
        Next lObjMatch
        lObjRegExp = Nothing
        FormatDate = pStrDate
    End Function
    Public Shared Function IsDate(ByVal strDate As String) As Boolean
        Dim dtDate As DateTime
        Dim bValid As Boolean = True
        Try
            dtDate = DateTime.Parse(strDate)
        Catch eFormatException As FormatException
            ' the Parse method failed => the string strDate cannot be converted to a date.
            bValid = False
        End Try
        Return bValid
    End Function

    Public Shared Function Build301Redirect(ByVal sNewURL As String) As Boolean
        HttpContext.Current.Response.Status = "301 Moved Permanently"
        HttpContext.Current.Response.AddHeader("Location", sNewURL)
        Return True
    End Function
End Class

Public Class cdsnetFormActionModifier
    Inherits Stream
    Private _sink As Stream
    Private _position As Long
    Private _url As String
    Public Sub New(ByVal sink As Stream, ByVal url As String)
        _sink = sink
        _url = "$1" + url + "$3"
    End Sub

    Public Overloads Overrides ReadOnly Property CanRead() As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overloads Overrides ReadOnly Property CanSeek() As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overloads Overrides ReadOnly Property CanWrite() As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overloads Overrides ReadOnly Property Length() As Long
        Get
            Return 0
        End Get
    End Property

    Public Overloads Overrides Property Position() As Long
        Get
            Return _position
        End Get
        Set(ByVal value As Long)
            _position = value
        End Set
    End Property

    Public Overloads Overrides Function Seek(ByVal offset As Long, ByVal direction As System.IO.SeekOrigin) As Long
        Return _sink.Seek(offset, direction)
    End Function

    Public Overloads Overrides Sub SetLength(ByVal length As Long)
        _sink.SetLength(length)
    End Sub

    Public Overloads Overrides Sub Close()
        _sink.Close()
    End Sub

    Public Overloads Overrides Sub Flush()
        _sink.Flush()
    End Sub

    Public Overloads Overrides Function Read(ByVal buffer As Byte(), ByVal offset As Integer, ByVal count As Integer) As Integer
        Return _sink.Read(buffer, offset, count)
    End Function

    Public Overloads Overrides Sub Write(ByVal buffer As Byte(), ByVal offset As Integer, ByVal count As Integer)
        Dim s As String = System.Text.UTF8Encoding.UTF8.GetString(buffer, offset, count)
        Dim reg As New Regex("(<form.*action="")([^""]*)(""[^>]*>)", RegexOptions.IgnoreCase)
        Dim m As Match = reg.Match(s)
        If m.Success Then
            Dim form As String = reg.Replace(m.Value, _url)
            Dim iform As Integer = m.Index
            Dim lform As Integer = m.Length
            s = s.Substring(0, iform) + form + s.Substring(iform + lform)
        End If
        Dim yaz As Byte() = System.Text.UTF8Encoding.UTF8.GetBytes(s)
        _sink.Write(yaz, 0, yaz.Length)
    End Sub


End Class

