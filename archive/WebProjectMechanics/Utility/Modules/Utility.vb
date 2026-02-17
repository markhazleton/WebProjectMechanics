Imports System.Text
Imports System.Text.RegularExpressions

Public Module Utility

    Public Function wpm_CheckForMatch(ByVal StringOne As String, ByVal StringTwo As String) As Boolean
        Dim bMatch As Boolean = False
        If StringOne Is Nothing Or StringTwo Is Nothing Then
            If StringOne Is Nothing And StringTwo Is Nothing Then
                bMatch = True
            End If
        Else
            ' To Make this Easier, let's ignore case and spaces and apmersands and dashes
            StringOne = (StringOne.ToLower)
            StringOne = Replace(StringOne, "/", String.Empty)
            StringOne = Replace(StringOne, ".html", String.Empty)
            StringOne = Replace(StringOne, ".htm", String.Empty)
            StringOne = Replace(StringOne, "&amp;", "&")
            StringOne = Replace(StringOne, "%20", String.Empty)
            StringOne = Replace(StringOne, "-", String.Empty)
            StringOne = Replace(StringOne, " ", String.Empty)
            StringOne = Replace(StringOne, wpm_SiteConfig.DefaultExtension, String.Empty)
            StringTwo = (StringTwo.ToLower)
            StringTwo = Replace(StringTwo, "/", String.Empty)
            StringTwo = Replace(StringTwo, ".html", String.Empty)
            StringTwo = Replace(StringTwo, ".htm", String.Empty)
            StringTwo = Replace(StringTwo, "%20", String.Empty)
            StringTwo = Replace(StringTwo, " ", String.Empty)
            StringTwo = Replace(StringTwo, "&amp;", "&")
            StringTwo = Replace(StringTwo, "-", String.Empty)
            StringTwo = Replace(StringTwo, wpm_SiteConfig.DefaultExtension, String.Empty)
            If (StringOne = StringTwo) Then
                bMatch = True
            Else
                bMatch = False
            End If
        End If
        Return bMatch
    End Function
    Public Function wpm_ConvertRichTextToHTML(ByVal sTextToCovert As String) As String
        Dim sbReturn As New StringBuilder(HttpContext.Current.Server.HtmlEncode(sTextToCovert))
        sbReturn.Replace("'", "&#39;")
        sbReturn.Replace("  ", " &nbsp;")
        sbReturn.Replace(" & ", " &amp; ")
        sbReturn.Replace(vbCrLf, "<br/>")
        sbReturn.Replace(vbLf, "<br/>")
        Return sbReturn.ToString
    End Function
    Public Function wpm_ClearLineFeeds(ByVal sTextToCovert As String) As String
        sTextToCovert = sTextToCovert.Replace(vbCrLf, String.Empty)
        sTextToCovert = sTextToCovert.Replace(vbLf, String.Empty)
        Return sTextToCovert
    End Function
    Public Function wpm_RemoveInvalidCharacters(ByVal Value As String) As String
        If Value Is Nothing Then
            Value = String.Empty
        Else
            Value = (Trim(Value.ToLower))
            Value = Value.Replace(" & ", String.Empty)
            Value = Value.Replace("&", String.Empty)
            Value = Value.Replace(" | ", String.Empty)
            Value = Value.Replace("|", String.Empty)
            Value = Value.Replace(",", String.Empty)
            Value = Value.Replace("/", String.Empty)
            Value = Value.Replace("\", String.Empty)
            Value = Value.Replace("<", String.Empty)
            Value = Value.Replace(">", String.Empty)
            Value = Value.Replace("(", String.Empty)
            Value = Value.Replace(")", String.Empty)
            Value = Value.Replace("{", String.Empty)
            Value = Value.Replace("}", String.Empty)
            Value = Value.Replace("[", String.Empty)
            Value = Value.Replace("]", String.Empty)
            Value = Value.Replace(",", String.Empty)
            Value = Value.Replace("'", String.Empty)
            Value = Value.Replace(";", String.Empty)
            Value = Value.Replace(":", String.Empty)
            Value = Value.Replace("-", String.Empty)
            Value = Value.Replace(".", String.Empty)
            Value = Value.Replace("http", String.Empty)
            Value = Value.Replace(" ", String.Empty)
            Value = Value.Replace("?", String.Empty)
            Value = Value.Replace("%22", String.Empty)
            Value = Value.Replace("=", String.Empty)
        End If
        Return Value
    End Function
    Public Function wpm_FixSingleQuote(ByVal FixString As String) As String
        Dim strFix As New StringBuilder(FixString)
        strFix.Replace("""", "&quot;")
        strFix.Replace("''", "&#39;")
        strFix.Replace("'", "&#39;")
        ' strFix.Replace(vbCrLf, "<br/>")
        Return strFix.ToString
    End Function

    Public Function wpm_FixInvalidCharacters(ByVal Value As String) As String
        If Value Is Nothing Then
            Value = String.Empty
        Else
            Value = (Trim(Value.ToLower))
            Value = value.Replace(" & ", "-and-")
            Value = value.Replace("&", "-and-")
            Value = value.Replace(" | ", "-and-")
            Value = value.Replace("|", "-and-")
            Value = value.Replace(",", "-")
            Value = value.Replace("/", "-")
            Value = value.Replace("\", "-")
            Value = value.Replace("<", "-")
            Value = value.Replace(">", "-")
            Value = value.Replace("(", "-")
            Value = value.Replace(")", "-")
            Value = value.Replace("[", "-")
            Value = value.Replace("]", "-")
            Value = value.Replace("{", "-")
            Value = value.Replace("}", "-")
            Value = value.Replace(",", "-")
            Value = value.Replace("'", "-")
            Value = value.Replace(";", "-")
            Value = value.Replace(":", "-")
            Value = value.Replace(" ", "-")
            Value = value.Replace(vbCr, " ")
            Value = value.Replace(vbLf, " ")
        End If
        Return Value
    End Function
    Public Function wpm_RemoveHtml(ByVal sContent As String) As String
        Return Regex.Replace(sContent, "<[^>]*>", String.Empty)
    End Function
    Public Function wpm_RemoveTags(ByVal sContent As String) As String
        Return Regex.Replace(sContent, "~~(.|\n)+?~~", String.Empty)
    End Function
    Public Function wpm_FormatNameForURL(ByVal Name As String) As String
        Name = (Replace(Name.ToLower, " ", "-"))
        Name = Replace(Name, "(", "-")
        Name = Replace(Name, ")", "-")
        Return Name
        ' Return FixInvalidCharacters(Name)
    End Function
    Public Function wpm_ApplyHTMLFormatting(ByVal strInput As String) As String
        strInput = "~" & strInput
        strInput = Replace(strInput, ",", "-")
        strInput = Replace(strInput, "'", "&quot;")
        strInput = Replace(strInput, """", "&quot;")
        strInput = Replace(strInput, "~", String.Empty)
        '  strInput = Replace(strInput, " " , "_")
        Return strInput
    End Function
    '********************************************************************************
    Public Function wpm_GetDBString(ByVal dbObject As Object) As String
        Dim strEntry As String = String.Empty
        If Not (IsDBNull(dbObject) Or dbObject Is Nothing) Then
            strEntry = CStr(dbObject)
            If strEntry = " " Then strEntry = String.Empty
        End If
        Return strEntry
    End Function
    Public Function wpm_GetStringValue(ByVal myString As String) As String
        If (IsDBNull(myString) Or myString Is Nothing) Then
            myString = String.Empty
        End If
        Return myString
    End Function
    '********************************************************************************
    Public Function wpm_GetDBDate(ByVal dbObject As Object) As DateTime
        If IsDBNull(dbObject) Then
            Return New DateTime
        ElseIf dbObject Is Nothing Then
            Return New DateTime
        ElseIf String.IsNullOrWhiteSpace(dbObject.ToString) Then
            Return New DateTime
        ElseIf wpm_IsDate(dbObject.ToString) Then
            Return CDate(dbObject)
        Else
            Return New DateTime
        End If

    End Function
    Public Function wpm_GetDBDouble(ByVal dbObject As Object) As Double
        If IsDBNull(dbObject) Then
            Return Nothing
        ElseIf dbObject Is Nothing Then
            Return Nothing
        Else
            If IsNumeric(dbObject) Then
                Return CDbl(dbObject)
            Else
                Return Nothing
            End If
        End If
    End Function
    Public Function wpm_GetDBInteger(ByVal dbObject As Object) As Integer
        If IsDBNull(dbObject) Then
            Return Nothing
        ElseIf dbObject Is Nothing Then
            Return Nothing
        Else
            If IsNumeric(dbObject) Then
                Return CInt(dbObject)
            Else
                Return Nothing
            End If
        End If
    End Function
    Public Function wpm_GetDBInteger(ByVal dbObject As Object, ByVal DefaultValue As Integer) As Integer
        If IsDBNull(dbObject) Then
            Return DefaultValue
        ElseIf dbObject Is Nothing Then
            Return DefaultValue
        Else
            If IsNumeric(dbObject) Then
                Return CInt(dbObject)
            Else
                Return DefaultValue
            End If
        End If
    End Function
    Public Function wpm_GetDBBoolean(ByVal dbObject As Object) As Boolean
        If IsDBNull(dbObject) Then
            Return False
        Else
            Return CBool(dbObject)
        End If
    End Function
    Public Function wpm_FormatPageNameForURL(ByVal PageName As String) As String
        If PageName Is Nothing Then
            Return Nothing
        Else
            Return (wpm_FixInvalidCharacters(PageName.ToLower))
        End If
    End Function
    ' ****************************************************************************
    Public Function wpm_LeadingZero(ByVal pStrValue As String) As String
        If Len(CStr(pStrValue)) < 2 Then pStrValue = "0" & pStrValue
        Return pStrValue
    End Function
    Public Function wpm_GetDayOrdinal(ByVal intDay As Integer) As String
        ' Accepts a day of the month as an integer and returns the
        ' appropriate suffix
        Dim strOrd As String = (String.Empty)
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
    Public Function wpm_GetRFC822Date(ByVal dbObject As Object) As String
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
    Public Function wpm_FormatDateString(ByVal strDate As String, ByVal strFormat As String) As String
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
        Dim int12HourPart As New Integer
        Dim str24HourPart As String = (String.Empty)
        Dim strMinutePart As String = (String.Empty)
        Dim strSecondPart As String = (String.Empty)
        Dim strAMPM As String = (String.Empty)

        ' Insert Month Numbers
        mySB.Replace("%m", DatePart("m", strDate).ToString)

        ' Insert non-Abbreviated Month Names
        mySB.Replace("%B", MonthName(DatePart("m", strDate), False))

        ' Insert Abbreviated Month Names
        mySB.Replace("%b", MonthName(DatePart("m", strDate), True))

        ' Insert Day Of Month
        mySB.Replace("%d", DatePart("d", strDate).ToString)

        ' Insert Day of Month Ordinal (eg st, th, or rd)
        mySB.Replace("%O", wpm_GetDayOrdinal(Day(CDate(strDate))))

        ' Insert Day of Year
        mySB.Replace("%j", DatePart("y", CDate(strDate)).ToString)

        ' Insert Long Year (4 digit)
        mySB.Replace("%Y", DatePart("yyyy", CDate(strDate)).ToString)

        ' Insert Short Year (2 digit)
        mySB.Replace("%y", Right(DatePart("yyyy", CDate(strDate)).ToString, 2))

        ' Insert Weekday as Integer (eg 0 = Sunday)
        mySB.Replace("%w", DatePart("w", CDate(strDate), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1).ToString)

        ' Insert Abbreviated Weekday Name (eg Sun)
        mySB.Replace("%a", WeekdayName(DatePart("w", strDate, FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1), True))

        ' Insert non-Abbreviated Weekday Name
        mySB.Replace("%A", WeekdayName(DatePart("w", strDate, FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1), False))

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
            mySB.Replace("%n", String.Empty)
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
    Public Function wpm_IsBreak(ByVal hInput As Object, ByVal hBreakPoint As Double) As Boolean
        ' remove anything after the decimal point and the decimal point if found
        ' because we need a whole number to use Mod
        If InStr(hInput.ToString, ".") <> 0 Then hInput = Left(hInput.ToString, InStrRev(hInput.ToString, ".") - 1)
        ' If not numeric, exit with a Null value
        If Not IsNumeric(hInput) Then
            Return False
            Exit Function
        End If
        ' Add one to adjust for starting at zero with arrays
        ' zero would always be the break point
        hInput = CInt(hInput) + 1
        ' use the Mod function to get the remainder of dividing the input by the break point. If it's 0, the
        ' number is we are NOT at a break point, If it's 1, the we ARE at a break point.
        If CBool(CInt(hInput) Mod hBreakPoint) Then
            Return False
        Else
            Return True
        End If
    End Function
    Public Function wpm_FormatGraphicLink(ByVal LinkURL As String, ByVal sImage As String) As String
        Return String.Format("<a href=""{0}""><img alt=""{1}"" border=0 src=""{2}images/{1}.gif""  /></a>", _
                                LinkURL, _
                                sImage, wpm_SiteConfig.ApplicationHome)
    End Function
    Public Function wpm_FormatLink(ByVal LinkName As String, ByVal LinkType As String, ByVal LinkURL As String) As String
        Dim sReturn As String
        Select Case LinkType
            Case "Contact"
                sReturn = LinkName
            Case Else
                sReturn = String.Format("<a href=""{0}"">{1}</a>", LinkURL, LinkName)
        End Select
        Return sReturn
    End Function
    Public Function wpm_FormatLink(ByVal LinkName As String, ByVal LinkType As String, ByVal LinkURL As String, ByVal LinkDescription As String) As String
        Dim sReturn As String
        Select Case LinkType
            Case "Contact"
                sReturn = LinkName
            Case Else
                sReturn = String.Format("<a href=""{0}"" title=""{1}"">{2}</a>", LinkURL, LinkDescription, LinkName)
        End Select
        Return sReturn
    End Function
    Public Function wpm_FormatPageNameLink(ByVal sPageName As String) As String
        Dim sReturn As String = String.Empty
        If Trim(sPageName) = String.Empty Then
            sReturn = "<a href=""/"">Home Page</a>"
        Else
            sReturn = String.Format("<a href=""{0}"">{1}</a>", wpm_FormatPageNameURL(sPageName), sPageName)
        End If
        Return (sReturn.ToLower)
    End Function
    Public Function wpm_FormatPageNameURL(ByVal sPageName As String) As String
        If Trim(sPageName) = String.Empty Then
            Return "/"
        Else
            Return String.Format("/{0}{1}", wpm_FixInvalidCharacters(sPageName), wpm_SiteConfig.DefaultExtension())
        End If
    End Function
    Public Function wpm_FormatTextEntry(ByVal strEntry As String) As String
        If strEntry = String.Empty Then strEntry = " "
        strEntry = Replace(strEntry, "'", "&#39;")
        Return strEntry
    End Function
    Public Function wpm_InternetTime() As String
        Dim lLngTime As Single = (Hour(TimeOfDay) * 3600 * 1000) + (Minute(TimeOfDay) * 60 * 1000) + (Second(TimeOfDay) * 1000 + 3600000)
        Dim lLngBeats As Single = lLngTime / 86400
        Dim lLngBeatsRound As Double = Math.Round(lLngBeats)
        If lLngBeatsRound > 1000 Then
            Return "@0" & lLngBeatsRound
        ElseIf lLngBeatsRound > 100 Then
            Return "@0" & lLngBeatsRound
        Else
            Return "@" & lLngBeatsRound
        End If
    End Function ' InternetTime()
    Public Function wpm_LeadingZero(ByRef pStrValue As Integer) As String
        If Len(CStr(pStrValue)) < 2 Then
            Return "0" & pStrValue.ToString
        Else
            Return pStrValue.ToString
        End If
    End Function ' LeadingZero(ByRef pStrValue)
    Public Function wpm_IsNowLeapYear() As Boolean
        Dim lLngYear As Integer = Year(Now)
        If (lLngYear Mod 4 = 0) And (lLngYear Mod 100 <> 0) Or (lLngYear Mod 400 = 0) Then
            Return True
        Else
            Return False
        End If
    End Function ' IsLeap()
    Public Function wpm_DaysInMonth() As String
        Dim lLngMonth As Integer = Month(Now)
        Select Case lLngMonth
            Case 9, 4, 6, 11
                Return "30"
            Case 2
                If CBool(wpm_IsNowLeapYear()) Then wpm_DaysInMonth = "29" Else wpm_DaysInMonth = "28"
            Case Else
                Return "31"
        End Select
    End Function ' DaysInMonth()
    Public Function wpm_FormatHour() As Integer
        Dim lDtmNow As String = FormatDateTime(Now, DateFormat.LongTime)
        Return CInt(Left(lDtmNow, InStr(lDtmNow, ":") - 1))
    End Function ' FormatHour()
    Public Function wpm_OrdinalSuffix() As String
        Dim lLngDay As Integer = Day(Now)
        If Right(lLngDay.ToString, 1) = "1" Then
            Return "st"
        Else
            Return "th"
        End If
        If Right(lLngDay.ToString, 1) = "2" Then Return "nd"
        If Right(lLngDay.ToString, 1) = "3" Then Return "rd"
    End Function ' OrdinalSuffix()
    Public Function wpm_GetCurrentDate() As String
        Dim dRightNow As DateTime = DateTime.Now
        Return dRightNow.ToLongDateString
    End Function
    Public Function wpm_FormatDate(ByVal lDtmNow As Date, ByRef pStrDate As String) As String
        ' Define local variables
        Dim lObjRegExp As Regex = New Regex("([a-z][a-z]*[a-z])*[a-z]", RegexOptions.IgnoreCase)
        Dim lLngHour As Integer = Hour(lDtmNow)
        Dim lLngWeekDay As Integer = Weekday(lDtmNow)
        Dim lLngSecond As Integer = Second(lDtmNow)
        Dim lLngMinute As Integer = Minute(lDtmNow)
        Dim lLngDay As Integer = Day(lDtmNow)
        Dim lLngMonth As Integer = Month(lDtmNow)
        Dim lLngYear As Integer = Year(lDtmNow)

        ' Prepare RegExp object and set parameters

        ' List each individual match and compare to different date functoids
        For Each lObjMatch As Match In lObjRegExp.Matches(pStrDate)
            Select Case lObjMatch.Value
                Case "a"
                    pStrDate = Replace(pStrDate, "a", (Right(lDtmNow.ToString.ToLower, 2)))
                Case "A"
                    pStrDate = Replace(pStrDate, "A", (Right(lDtmNow.ToString.ToUpper, 2)))
                Case "B"
                    pStrDate = Replace(pStrDate, "B", wpm_InternetTime())
                Case "d"
                    pStrDate = Replace(pStrDate, "d", wpm_LeadingZero(lLngDay))
                Case "D"
                    pStrDate = Replace(pStrDate, "D", Left(WeekdayName(lLngWeekDay), 3))
                Case "M"
                    pStrDate = Replace(pStrDate, "M", MonthName(lLngMonth))
                Case "g"
                    pStrDate = Replace(pStrDate, "g", wpm_FormatHour().ToString)
                Case "G"
                    pStrDate = Replace(pStrDate, "G", lLngHour.ToString)
                Case "h"
                    pStrDate = Replace(pStrDate, "h", wpm_LeadingZero(wpm_FormatHour()))
                Case "H"
                    pStrDate = Replace(pStrDate, "H", wpm_LeadingZero(lLngHour))
                Case "i"
                    pStrDate = Replace(pStrDate, "i", wpm_LeadingZero(lLngMinute))
                Case "j"
                    pStrDate = Replace(pStrDate, "j", lLngDay.ToString)
                Case "l"
                    pStrDate = Replace(pStrDate, "l", WeekdayName(lLngWeekDay))
                Case "L"
                    pStrDate = Replace(pStrDate, "L", wpm_IsNowLeapYear().ToString)
                Case "m"
                    pStrDate = Replace(pStrDate, "m", wpm_LeadingZero(lLngMonth))
                Case "M"
                    pStrDate = Replace(pStrDate, "M", Left(MonthName(lLngMonth), 3))
                Case "n"
                    pStrDate = Replace(pStrDate, "n", lLngMonth.ToString)
                Case "r"
                    pStrDate = Replace(pStrDate, "r", String.Format("{0}, {1} {2} {3} {4}:{5}", Left(WeekdayName(lLngWeekDay), 3), lLngDay, Left(MonthName(lLngMonth), 3), lLngYear, FormatDateTime(TimeOfDay, DateFormat.LongTime), wpm_LeadingZero(lLngSecond)))
                Case "s"
                    pStrDate = Replace(pStrDate, "s", wpm_LeadingZero(lLngSecond))
                Case "S"
                    pStrDate = Replace(pStrDate, "S", wpm_OrdinalSuffix())
                Case "t"
                    pStrDate = Replace(pStrDate, "t", wpm_DaysInMonth())
                Case "U"
                    pStrDate = Replace(pStrDate, "U", CStr(DateDiff(DateInterval.Second, DateSerial(1970, 1, 1), lDtmNow)))
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
                    pStrDate = pStrDate & String.Empty
            End Select
        Next lObjMatch
        lObjRegExp = Nothing
        wpm_FormatDate = pStrDate
    End Function
    Public Function wpm_IsDate(ByVal strDate As String) As Boolean
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
    Public Function wpm_Build301Redirect(ByVal sNewURL As String) As Boolean
        HttpContext.Current.Response.Status = "301 Moved Permanently"
        HttpContext.Current.Response.AddHeader("Location", sNewURL)
        Return True
    End Function
    Public Function wpm_HexStringToColor(ByVal hex As String) As System.Drawing.Color
        hex = hex.Replace("#", String.Empty)
        If hex.Length <> 6 Then
            Throw New Exception(hex & " is not a valid 6-place hexadecimal color code.")
        End If
        Return System.Drawing.Color.FromArgb(wpm_HexStringToBase10Int(hex.Substring(0, 2)), wpm_HexStringToBase10Int(hex.Substring(2, 2)), wpm_HexStringToBase10Int(hex.Substring(4, 2)))
    End Function
    Public Function wpm_HexStringToBase10Int(ByVal hex As String) As Integer
        Dim base10value As Integer = 0
        Try
            base10value = Convert.ToInt32(hex, 16)
        Catch
            base10value = 0
        End Try
        Return base10value
    End Function

End Module

