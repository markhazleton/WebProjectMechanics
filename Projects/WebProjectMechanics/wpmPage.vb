Imports System.IO
Imports System.Text.RegularExpressions


Public Class wpmPage
    Inherits System.Web.UI.Page

    Public Shared mySession As WebProjectMechanics.wpmSession
    Public Shared pageActiveSite As WebProjectMechanics.wpmActiveSite
    Public Shared StartTimer As Long

    Private bInPortal As Boolean
#Region "Constants"
    ' EW?
    Public Const EW_RESPONSE_BUFFER As Boolean = True

    ' Delimiters/Separators
    Public Const wpm_RECORD_DELIMITER As String = vbCr
    Public Const wpm_FIELD_DELIMITER As String = "|"
    Public Const wpm_COMPOSITE_KEY_SEPARATOR As String = "," ' Composite key separator 
    Public Const wpm_EMAIL_KEYWORD_SEPARATOR As String = "" ' Email keyword separator 
    Public Const wpm_EMAIL_CHARSET As String = "utf-8" ' Email charset 

    ' Date format
    Public Const wpm_DATE_SEPARATOR As String = "/"
    Public Const wpm_DEFAULT_DATE_FORMAT As Short = 6

    ' Highlight	
    Public Const wpm_HIGHLIGHT_COMPARE As Boolean = True ' Case-insensitive

    ' Language settings
    Public Const wpm_LANGUAGE_FOLDER As String = "lang/"

#End Region

    Public Shared wpm_LANGUAGE_FILE()() As String = {New String() {"en", "", "english.xml"}}

    Private Shared Property wpm_ITEM_TABLE_CLASSNAME As New String(CType("ewTable", Char()))

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        MyBase.OnPreInit(e)
    End Sub

    Public ReadOnly Property InPortal() As Boolean
        Get
            Return bInPortal
        End Get
    End Property

    ' Return URL
    Public Shared ReadOnly Property ReturnUrl() As String
        Get ' Get referer URL automatically
            If HttpContext.Current.Request.ServerVariables("HTTP_REFERER") IsNot Nothing Then
                If wpm_ReferPage() <> wpm_CurrentPage() AndAlso wpm_ReferPage() <> "login.aspx" Then ' Referer not same page or login page
                    Dim url As String = HttpContext.Current.Request.ServerVariables("HTTP_REFERER")
                    If url.Contains("?a=") Then ' Remove action
                        Dim p1 As Integer = url.LastIndexOf("?a=")
                        Dim p2 As Integer = url.IndexOf("&", p1)
                        If p2 > -1 Then
                            url = url.Substring(0, p1 + 1) & url.Substring(p2 + 1)
                        Else
                            url = url.Substring(0, p1)
                        End If
                    End If
                    mySession.ListPageURL = url ' Save to Session
                End If
            End If
            If wpm_NotEmpty(mySession.ListPageURL) Then
                Return mySession.ListPageURL
            Else
                Return "/"
            End If
        End Get
    End Property
    ' Get/set session values
    Public Shared Property wpm_Session(ByVal name As String) As Object
        Get
            Return HttpContext.Current.Session(name)
        End Get
        Set(ByVal Value As Object)
            HttpContext.Current.Session(name) = Value
        End Set
    End Property

    Protected Shared Function GetPortalUserSetting(ByVal PropertyNM As String) As String
        Dim sReturn As String = ("")
        Return sReturn
    End Function

    Protected Shared Function SetPortalUserSetting(ByVal PropertyNM As String, ByVal ValueNM As String) As Boolean
        Return False
    End Function

    Protected Shared Function GetPortalUserNM() As String
        Return ""
    End Function

    Protected Shared Function GetPortalLogin() As String
        Return ""
    End Function
    Protected Shared Function GetPortalUserInfo() As String
        Return ""
    End Function
    Protected Function GetSessionProperty(ByVal myProperty As String) As String
        Dim myVal As String
        If Len(Session(myProperty)) > 0 Then
            myVal = Session(myProperty).ToString
        Else
            myVal = ""
        End If
        Return myVal
    End Function
    Protected Function ClearSessionProperty(ByVal myProperty As String) As Boolean
        Session(myProperty) = ""
        Return True
    End Function
    Protected Function SetSessionProperty(ByVal myProperty As String, ByVal myValue As String) As Boolean
        If Trim(myValue) <> "" Then
            Session(myProperty) = myValue
        End If
        Return True
    End Function
    Protected Function GetIntegerProperty(ByVal myProperty As String, ByVal curValue As Integer) As Integer
        Dim myValue As Integer
        If Len(Request.QueryString(myProperty)) > 0 Then
            myValue = CInt(Request.QueryString(myProperty))
        ElseIf Len(Request.Form.Item(myProperty)) > 0 Then
            myValue = CInt(Request.Form.Item(myProperty))
        Else
            myValue = curValue
        End If
        Return myValue
    End Function
    Protected Function GetFormProperty(ByVal myProperty As String, ByVal curValue As String, ByVal ControlPrefix As String) As String
        Dim myValue As String = ""
        If Len(Request.QueryString(myProperty)) > 0 Then
            myValue = Request.QueryString(myProperty).ToString
        ElseIf Len(Request.Form.Item(myProperty)) > 0 Then
            myValue = Request.Form.Item(myProperty).ToString
        ElseIf Len(Request.Form(String.Format("{0}{1}", ControlPrefix, myProperty))) > 0 Then
            myValue = Request.Form(String.Format("{0}{1}", ControlPrefix, myProperty))
        Else
            If curValue Is Nothing Then
                myValue = String.Empty
            Else
                myValue = curValue
            End If
        End If
        Return myValue
    End Function

    Protected Function GetProperty(ByVal myProperty As String, ByVal curValue As String) As String
        Dim myValue As String = ""
        If Len(Request.QueryString(myProperty)) > 0 Then
            myValue = Request.QueryString(myProperty).ToString
        ElseIf Len(Request.Form.Item(myProperty)) > 0 Then
            myValue = Request.Form.Item(myProperty).ToString
        Else
            If curValue Is Nothing Then
                myValue = String.Empty
            Else
                myValue = curValue
            End If
        End If
        Return myValue
    End Function
    ' Write the paths for config/debug only
    Public Shared Sub wpm_WriteUploadPaths()
        wpm_Write(String.Format("APPL_PHYSICAL_PATH = {0}<br>", HttpContext.Current.Request.ServerVariables("APPL_PHYSICAL_PATH")))
        wpm_Write(String.Format("APPL_MD_PATH = {0}<br>", HttpContext.Current.Request.ServerVariables("APPL_MD_PATH")))
    End Sub

    ' Get current page name
    Public Shared Function wpm_CurrentPage() As String
        Return wpm_GetPageName(HttpContext.Current.Request.ServerVariables("SCRIPT_NAME"))
    End Function

    ' Get refer page name
    Public Shared Function wpm_ReferPage() As String
        Return wpm_GetPageName(HttpContext.Current.Request.ServerVariables("HTTP_REFERER"))
    End Function

    ' Get page name
    Public Shared Function wpm_GetPageName(ByVal url As String) As String
        If url <> "" Then
            If url.Contains("?") Then
                url = url.Substring(0, url.LastIndexOf("?")) ' Remove querystring first
            End If
            Return url.Substring(url.LastIndexOf("/") + 1) ' Remove path
        Else
            Return ""
        End If
    End Function

    ' Get full URL
    Public Shared Function wpm_FullUrl() As String
        Return wpm_DomainUrl() & HttpContext.Current.Request.ServerVariables("SCRIPT_NAME")
    End Function

    ' Get domain URL
    Public Shared Function wpm_DomainUrl() As String
        Dim sUrl As String
        Dim bSSL As Boolean = Not wpm_SameText(HttpContext.Current.Request.ServerVariables("HTTPS"), "off")
        Dim sPort As String = HttpContext.Current.Request.ServerVariables("SERVER_PORT")
        Dim defPort As String = CStr(IIf(bSSL, "443", "80"))
        If sPort = defPort Then sPort = "" Else sPort = ":" & sPort
        If bSSL Then
            sUrl = "http" & "s"
        Else
            sUrl = "http"
        End If
        Return String.Format("{0}://{1}{2}", sUrl, HttpContext.Current.Request.ServerVariables("SERVER_NAME"), sPort)
    End Function

    ' Get current URL
    Public Shared Function wpm_CurrentUrl() As String
        Dim s As String = HttpContext.Current.Request.ServerVariables("SCRIPT_NAME")
        Dim q As String = HttpContext.Current.Request.ServerVariables("QUERY_STRING")
        If q <> "" Then s &= "?" & q
        Return s
    End Function

    ' Get application root path (relative to domain)
    Public Shared Function wpm_AppPath() As String
        Dim pos As String
        Dim Path As String = HttpContext.Current.Request.ServerVariables("APPL_MD_PATH")
        pos = Path.IndexOf("Root", StringComparison.InvariantCultureIgnoreCase)
        If pos > 0 Then Path = Path.Substring(pos + 4)
        Return Path
    End Function

    ' Convert to full URL
    Public Shared Function wpm_ConvertFullUrl(ByVal url As String) As String
        If url = "" Then
            Return ""
        ElseIf url.Contains("://") Then
            Return url
        Else
            Dim sUrl As String = wpm_FullUrl()
            Return sUrl.Substring(0, sUrl.LastIndexOf("/") + 1) & url
        End If
    End Function

    ' Check if folder exists
    Public Shared Function wpm_FolderExists(ByVal folder As String) As Boolean
        Return Directory.Exists(folder)
    End Function

    ' Check if file exists
    Public Shared Function wpm_FileExists(ByVal folder As String, ByVal fn As String) As Boolean
        Return File.Exists(folder & fn)
    End Function
    ' Response.Write
    Public Shared Sub wpm_Write(ByVal value As Object)
        HttpContext.Current.Response.Write(value)
    End Sub

    ' Response.End
    Public Shared Sub wpm_End()
        HttpContext.Current.Response.End()
    End Sub


    ' Compare object as string
    Public Shared Function wpm_SameStr(ByVal v1 As Object, ByVal v2 As Object) As Boolean
        Return String.Equals(Convert.ToString(v1).Trim(), Convert.ToString(v2).Trim())
    End Function

    ' Compare object as string (case insensitive)
    Public Shared Function wpm_SameText(ByVal v1 As Object, ByVal v2 As Object) As Boolean
        Return String.Equals(Convert.ToString(v1).Trim().ToLower(), Convert.ToString(v2).Trim().ToLower())
    End Function

    ' Check if empty string
    Public Shared Function wpm_Empty(ByVal value As Object) As Boolean
        Return String.Equals(Convert.ToString(value).Trim(), String.Empty)
    End Function

    ' Check if not empty string
    Public Shared Function wpm_NotEmpty(ByVal value As Object) As Boolean
        Return Not wpm_Empty(value)
    End Function

    ' Convert object to integer
    Public Shared Function wpm_ConvertToInt(ByVal value As Object) As Integer
        Try
            Return Convert.ToInt32(value)
        Catch
            Return 0
        End Try
    End Function

    ' Convert object to double
    Public Shared Function wpm_ConvertToDouble(ByVal value As Object) As Double
        Try
            Return Convert.ToDouble(value)
        Catch
            Return 0
        End Try
    End Function

    ' Convert object to bool
    Public Shared Function wpm_ConvertToBool(ByVal value As Object) As Boolean
        Try
            If Information.IsNumeric(value) Then
                Return Convert.ToBoolean(wpm_ConvertToDouble(value))
            Else
                Return Convert.ToBoolean(value)
            End If
        Catch
            Return False
        End Try
    End Function

    ' Get user IP
    Public Shared Function wpm_CurrentUserIP() As String
        Return HttpContext.Current.Request.ServerVariables("REMOTE_HOST")
    End Function

    ' Get current host name, e.g. "www.mycompany.com"
    Public Shared Function wpm_CurrentHost() As String
        Return HttpContext.Current.Request.ServerVariables("HTTP_HOST")
    End Function

    ' Get current date in default date format
    Public Shared Function wpm_CurrentDate() As String
        If wpm_DEFAULT_DATE_FORMAT = 6 OrElse wpm_DEFAULT_DATE_FORMAT = 7 Then
            Return wpm_FormatDateTime(Today, wpm_DEFAULT_DATE_FORMAT)
        Else
            Return wpm_FormatDateTime(Today, 5)
        End If
    End Function

    ' Get current time in hh:mm:ss format
    Public Shared Function wpm_CurrentTime() As String
        Dim DT As DateTime = Now()
        Return DT.ToString("HH:mm:ss")
    End Function

    ' Get current date in default date format with
    ' Current time in hh:mm:ss format
    Public Shared Function wpm_CurrentDateTime() As String
        Dim DT As DateTime = Now()
        If wpm_DEFAULT_DATE_FORMAT = 6 OrElse wpm_DEFAULT_DATE_FORMAT = 7 Then
            wpm_CurrentDateTime = wpm_FormatDateTime(DT, wpm_DEFAULT_DATE_FORMAT)
        Else
            wpm_CurrentDateTime = wpm_FormatDateTime(DT, 5)
        End If
        wpm_CurrentDateTime = String.Format("{0} {1:HH:mm:ss}", wpm_CurrentDateTime, DT)
    End Function

    ' Functions for default date format
    ' ANamedFormat = 0-8, where 0-4 same as VBScript
    ' 5 = "yyyymmdd"
    ' 6 = "mmddyyyy"
    ' 7 = "ddmmyyyy"
    ' 8 = Short Date + Short Time
    ' 9 = "yyyymmdd HH:MM:SS"
    ' 10 = "mmddyyyy HH:MM:SS"
    ' 11 = "ddmmyyyy HH:MM:SS"
    ' 12 = "HH:MM:SS"
    ' Format date time based on format type
    Public Shared Function wpm_FormatDateTime(ByVal ADate As Object, ByVal ANamedFormat As Integer) As String
        Dim sDT As String
        If IsDate(ADate) Then
            Dim DT As DateTime = Convert.ToDateTime(ADate)
            If ANamedFormat >= 0 AndAlso ANamedFormat <= 4 Then
                sDT = FormatDateTime(CDate(ADate), CType(ANamedFormat, DateFormat))
            ElseIf ANamedFormat = 5 OrElse ANamedFormat = 9 Then
                sDT = DT.Year & wpm_DATE_SEPARATOR & DT.Month & wpm_DATE_SEPARATOR & DT.Day
            ElseIf ANamedFormat = 6 OrElse ANamedFormat = 10 Then
                sDT = DT.Month & wpm_DATE_SEPARATOR & DT.Day & wpm_DATE_SEPARATOR & DT.Year
            ElseIf ANamedFormat = 7 OrElse ANamedFormat = 11 Then
                sDT = DT.Day & wpm_DATE_SEPARATOR & DT.Month & wpm_DATE_SEPARATOR & DT.Year
            ElseIf ANamedFormat = 8 Then
                If DT.Hour <> 0 OrElse DT.Minute <> 0 OrElse DT.Second <> 0 Then
                    sDT = String.Format("{0} {1:HH:mm:ss}", FormatDateTime(CDate(ADate), CType(2, DateFormat)), DT)
                Else
                    sDT = FormatDateTime(CDate(ADate), CType(2, DateFormat))
                End If
            ElseIf ANamedFormat = 12 Then
                sDT = DT.ToString("HH:mm:ss")
            Else
                Return Convert.ToString(DT)
            End If
            If ANamedFormat >= 9 AndAlso ANamedFormat <= 11 Then
                sDT = String.Format("{0} {1:HH:mm:ss}", sDT, DT)
            End If
            Return sDT
        Else
            Return Convert.ToString(ADate)
        End If
    End Function

    ' Unformat date time based on format type
    Public Shared Function wpm_UnFormatDateTime(ByVal ADate As String, ByVal ANamedFormat As Integer) As String
        Dim arDate() As String
        Dim arDateTime() As String
        Dim d As DateTime
        Dim sDT As String
        ADate = Convert.ToString(ADate).Trim()
        While ADate.Contains("  ")
            ADate = ADate.Replace("  ", " ")
        End While
        arDateTime = ADate.Split(New Char() {" "c})
        If ANamedFormat = 0 AndAlso IsDate(ADate) Then
            d = Convert.ToDateTime(arDateTime(0))
            sDT = d.ToString("yyyy/MM/dd")
            If arDateTime.GetUpperBound(0) > 0 Then
                For i As Integer = 1 To arDateTime.GetUpperBound(0)
                    sDT = String.Format("{0} {1}", sDT, arDateTime(i))
                Next
            End If
            Return sDT
        Else
            arDate = arDateTime(0).Split(New Char() {Convert.ToChar(wpm_DATE_SEPARATOR)})
            If arDate.GetUpperBound(0) = 2 Then
                sDT = arDateTime(0)
                If ANamedFormat = 6 OrElse ANamedFormat = 10 Then ' mmddyyyy
                    If arDate(0).Length <= 2 AndAlso arDate(1).Length <= 2 AndAlso arDate(2).Length <= 4 Then
                        sDT = String.Format("{0}/{1}/{2}", arDate(2), arDate(0), arDate(1))
                    End If
                ElseIf ANamedFormat = 7 OrElse ANamedFormat = 11 Then  ' ddmmyyyy
                    If arDate(0).Length <= 2 AndAlso arDate(1).Length <= 2 AndAlso arDate(2).Length <= 4 Then
                        sDT = String.Format("{0}/{1}/{2}", arDate(2), arDate(1), arDate(0))
                    End If
                ElseIf ANamedFormat = 5 OrElse ANamedFormat = 9 Then  ' yyyymmdd
                    If arDate(0).Length <= 4 AndAlso arDate(1).Length <= 2 AndAlso arDate(2).Length <= 2 Then
                        sDT = String.Format("{0}/{1}/{2}", arDate(0), arDate(1), arDate(2))
                    End If
                End If
                If arDateTime.GetUpperBound(0) > 0 Then
                    If IsDate(arDateTime(1)) Then ' Is time
                        sDT = String.Format("{0} {1}", sDT, arDateTime(1))
                    End If
                End If
                Return sDT
            Else
                Return ADate
            End If
        End If
    End Function

    ' Format currency
    Public Shared Function wpm_FormatCurrency(ByVal Expression As Object, ByVal NumDigitsAfterDecimal As Integer, ByVal IncludeLeadingDigit As Integer, ByVal UseParensForNegativeNumbers As Integer, ByVal GroupDigits As Integer) As String
        If Not Information.IsNumeric(Expression) Then Return Convert.ToString(Expression)
        If IsDBNull(Expression) Then Return String.Empty
        Return FormatCurrency(Expression, NumDigitsAfterDecimal, CType(IncludeLeadingDigit, TriState), CType(UseParensForNegativeNumbers, TriState), CType(GroupDigits, TriState))
    End Function

    ' Format number
    Public Shared Function wpm_FormatNumber(ByVal Expression As Object, ByVal NumDigitsAfterDecimal As Integer, ByVal IncludeLeadingDigit As Integer, ByVal UseParensForNegativeNumbers As Integer, ByVal GroupDigits As Integer) As String
        If Not Information.IsNumeric(Expression) Then Return Convert.ToString(Expression)
        If IsDBNull(Expression) Then Return String.Empty
        Return FormatNumber(Expression, NumDigitsAfterDecimal, CType(IncludeLeadingDigit, TriState), CType(UseParensForNegativeNumbers, TriState), CType(GroupDigits, TriState))
    End Function

    ' Format percent
    Public Shared Function wpm_FormatPercent(ByVal Expression As Object, ByVal NumDigitsAfterDecimal As Integer, ByVal IncludeLeadingDigit As Integer, ByVal UseParensForNegativeNumbers As Integer, ByVal GroupDigits As Integer) As String
        If Not Information.IsNumeric(Expression) Then Return Convert.ToString(Expression)
        If IsDBNull(Expression) Then Return String.Empty
        Return FormatPercent(Expression, NumDigitsAfterDecimal, CType(IncludeLeadingDigit, TriState), CType(UseParensForNegativeNumbers, TriState), CType(GroupDigits, TriState))
    End Function

    ' Encode HTML
    Public Shared Function wpm_HtmlEncode(ByVal Expression As Object) As String
        Return HttpContext.Current.Server.HtmlEncode(Convert.ToString(Expression))
    End Function

    ' Encode value for single-quoted JavaScript string
    Public Shared Function wpm_JsEncode(ByVal val As Object) As String
        Dim sReturn As String = Convert.ToString(val).Replace("\", "\\")
        sReturn = sReturn.Replace("'", "\'")
        sReturn = sReturn.Replace(vbCrLf, "<br>")
        sReturn = sReturn.Replace(vbCr, "<br>")
        sReturn = sReturn.Replace(vbLf, "<br>")
        Return sReturn
    End Function

    ' Encode value for double-quoted JavaScript string
    Public Shared Function wpm_JsEncode2(ByVal val As Object) As String
        Dim sReturn As String = Convert.ToString(val).Replace("\", "\\")
        sReturn = sReturn.Replace("""", "\""")
        sReturn = sReturn.Replace(vbCrLf, "<br>")
        sReturn = sReturn.Replace(vbCr, "<br>")
        sReturn = sReturn.Replace(vbLf, "<br>")
        Return sReturn
    End Function

    ' Generate Value Separator based on current row count
    ' rowcnt - zero based row count
    Public Shared Function wpm_ValueSeparator() As String
        Return ", "
    End Function

    ' Encode value to single-quoted Javascript string for HTML attributes
    Public Shared Function wpm_JsEncode3(ByVal val As Object) As String
        Dim out As String = Convert.ToString(val).Replace("\", "\\")
        out = out.Replace("'", "\'")
        out = out.Replace("""", "&quot;")
        Return out
    End Function

    ' Convert array to JSON for HTML attributes
    Public Shared Function wpm_ArrayToJsonAttr(ByVal ht As Hashtable) As String
        Dim Str As String = "{"
        For Each Item As DictionaryEntry In ht
            Str &= String.Format("{0}:'{1}',", Item.Key, wpm_JsEncode3(Item.Value))
        Next
        If Str.EndsWith(",") Then Str = Str.Substring(0, Str.Length - 1)
        Str &= "}"
        Return Str
    End Function

    ' Generate View Option Separator based on current row count (Multi-Select / CheckBox)
    ' rowcnt - zero based row count
    Public Shared Function wpm_ViewOptionSeparator() As String
        ' Sample code to adjust 2 options per row
        'If ((rowcnt + 1) Mod 2 = 0) Then ' 2 options per row
        '	Sep = Sep & "<br>"
        'End If

        Return ", "
    End Function

    ' Render repeat column table
    ' rowcnt - zero based row count
    Public Shared Function wpm_RepeatColumnTable(ByVal totcnt As Integer, ByVal rowcnt As Integer, ByVal repeatcnt As Integer, ByVal rendertype As Integer) As String
        Dim sWrk As String = ""
        If rendertype = 1 Then ' Render control start
            If rowcnt = 0 Then sWrk = String.Format("{0}<table class=""{1}"">", sWrk, wpm_ITEM_TABLE_CLASSNAME)
            If (rowcnt Mod repeatcnt = 0) Then sWrk = sWrk & "<tr>"
            sWrk = sWrk & "<td>"
        ElseIf rendertype = 2 Then ' Render control end
            sWrk = sWrk & "</td>"
            If (rowcnt Mod repeatcnt = repeatcnt - 1) Then
                sWrk = sWrk & "</tr>"
            ElseIf rowcnt = totcnt Then
                For i As Integer = ((rowcnt Mod repeatcnt) + 1) To repeatcnt - 1
                    sWrk = sWrk & "<td>&nbsp;</td>"
                Next
                sWrk = sWrk & "</tr>"
            End If
            If rowcnt = totcnt Then sWrk = sWrk & "</table>"
        End If
        Return sWrk
    End Function

    ' Truncate Memo Field based on specified length, string truncated to nearest space or CrLf
    Public Shared Function wpm_TruncateMemo(ByVal memostr As String, ByVal ln As Integer, ByVal removehtml As Boolean) As String
        Dim j As Integer, i As Integer, k As Integer
        Dim str As String
        If removehtml Then
            str = wpm_RemoveHtml(memostr) ' Remove HTML
        Else
            str = memostr
        End If
        If str.Length > 0 AndAlso str.Length > ln Then
            k = 0
            Do While k >= 0 AndAlso k < str.Length
                i = str.IndexOf(" ", k)
                j = str.IndexOf(vbCrLf, k)
                If i < 0 AndAlso j < 0 Then ' Unable to truncate
                    Return str
                Else

                    ' Get nearest space or CrLf
                    If i > 0 AndAlso j > 0 Then
                        If i < j Then k = i Else k = j
                    ElseIf i > 0 Then
                        k = i
                    ElseIf j > 0 Then
                        k = j
                    End If

                    ' Get truncated text
                    If k >= ln Then
                        Return str.Substring(0, k) & "..."
                    Else
                        k = k + 1
                    End If
                End If
            Loop
        End If
        Return str
    End Function

    ' Remove Html tags from text
    Public Shared Function wpm_RemoveHtml(ByVal str As String) As String
        Return Regex.Replace(str, "<[^>]*>", String.Empty)
    End Function


    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        pageActiveSite = New WebProjectMechanics.wpmActiveSite(Session)
        mySession = New WebProjectMechanics.wpmSession(HttpContext.Current.Session)
        ' MyBase.OnLoad(e)
    End Sub
    Public Shared Function ArrayToJsonAttr(ByVal ht As Hashtable) As String
        Dim Str As String = "{"
        For Each Item As DictionaryEntry In ht
            Str &= String.Format("{0}:'{1}',", Item.Key, JsEncode3(Item.Value))
        Next
        If Str.EndsWith(",") Then Str = Str.Substring(0, Str.Length - 1)
        Str &= "}"
        Return Str
    End Function
    Public Shared Function JsEncode3(ByVal val As Object) As String
        Return Convert.ToString(val).Replace("\", "\\")
    End Function
    Public Shared Function ViewOptionSeparator() As String
        ' Sample code to adjust 2 options per row
        'If ((rowcnt + 1) Mod 2 = 0) Then ' 2 options per row
        '	Sep = Sep & "<br>"
        'End If
        Return ", "
    End Function
    Public Shared Function ValueSeparator() As String
        Return ", "
    End Function
    Public Shared Function RepeatColumnTable(ByVal totcnt As Integer, ByVal rowcnt As Integer, ByVal repeatcnt As Integer, ByVal rendertype As Integer) As String
        Dim sWrk As String = ""
        If rendertype = 1 Then ' Render control start
            If rowcnt = 0 Then sWrk = String.Format("{0}<table class=""{1}"">", sWrk, p2p_ITEM_TABLE_CLASSNAME)
            If (rowcnt Mod repeatcnt = 0) Then sWrk = sWrk & "<tr>"
            sWrk = sWrk & "<td>"
        ElseIf rendertype = 2 Then ' Render control end
            sWrk = sWrk & "</td>"
            If (rowcnt Mod repeatcnt = repeatcnt - 1) Then
                sWrk = sWrk & "</tr>"
            ElseIf rowcnt = totcnt Then
                For i As Integer = ((rowcnt Mod repeatcnt) + 1) To repeatcnt - 1
                    sWrk = sWrk & "<td>&nbsp;</td>"
                Next
                sWrk = sWrk & "</tr>"
            End If
            If rowcnt = totcnt Then sWrk = sWrk & "</table>"
        End If
        Return sWrk
    End Function
    Private Shared Property p2p_ITEM_TABLE_CLASSNAME() As String
    Public Shared Function p2p_RemoveHtml(ByVal str As String) As String
        Return Regex.Replace(str, "<[^>]*>", String.Empty)
    End Function
    Public Shared Function TruncateMemo(ByVal memostr As String, ByVal ln As Integer, ByVal removehtml As Boolean) As String
        Dim j As Integer
        Dim i As Integer
        Dim k As Integer
        Dim str As String
        If removehtml Then
            str = p2p_RemoveHtml(memostr) ' Remove HTML
        Else
            str = memostr
        End If
        If str.Length > 0 AndAlso str.Length > ln Then
            k = 0
            Do While k >= 0 AndAlso k < str.Length
                i = str.IndexOf(" ", k)
                j = str.IndexOf(vbCrLf, k)
                If i < 0 AndAlso j < 0 Then ' Unable to truncate
                    Return str
                Else
                    ' Get nearest space or CrLf
                    If i > 0 AndAlso j > 0 Then
                        If i < j Then
                            k = i
                        Else
                            k = j
                        End If
                    ElseIf i > 0 Then
                        k = i
                    ElseIf j > 0 Then
                        k = j
                    End If
                    ' Get truncated text
                    If k >= ln Then
                        Return str.Substring(0, k) & "..."
                    Else
                        k = k + 1
                    End If
                End If
            Loop
        End If
        Return str
    End Function
    Public Shared Function JsEncode(ByVal val As Object) As String
        Dim sReturn As String = Convert.ToString(val).Replace("\", "\\")
        sReturn = sReturn.Replace("'", "\'")
        sReturn = sReturn.Replace(vbCrLf, "<br>")
        sReturn = sReturn.Replace(vbCr, "<br>")
        sReturn = sReturn.Replace(vbLf, "<br>")
        Return sReturn
    End Function
    Public Shared Function JsEncode2(ByVal val As Object) As String
        Dim sReturn As String = Convert.ToString(val).Replace("\", "\\")
        sReturn = sReturn.Replace("""", "\""")
        sReturn = sReturn.Replace(vbCrLf, "<br>")
        sReturn = sReturn.Replace(vbCr, "<br>")
        sReturn = sReturn.Replace(vbLf, "<br>")
        Return sReturn
    End Function
    Public Shared Function HtmlEncode(ByVal Expression As Object) As String
        Return HttpContext.Current.Server.HtmlEncode(Convert.ToString(Expression))
    End Function
    Public Shared Function FormatPercent(ByVal Expression As Object, ByVal NumDigitsAfterDecimal As Integer) As String
        Return FormatPercent(Expression, NumDigitsAfterDecimal, 0, 0, 0)
    End Function
    Public Shared Function FormatPercent(ByVal Expression As Object, ByVal NumDigitsAfterDecimal As Integer, ByVal IncludeLeadingDigit As Integer, ByVal UseParensForNegativeNumbers As Integer, ByVal GroupDigits As Integer) As String
        If Not Information.IsNumeric(Expression) Then Return Convert.ToString(Expression)
        If IsDBNull(Expression) Then Return String.Empty
        Return FormatPercent(Expression, NumDigitsAfterDecimal, CType(IncludeLeadingDigit, TriState), CType(UseParensForNegativeNumbers, TriState), CType(GroupDigits, TriState))
    End Function
    Public Shared Function FormatNumber(ByVal Expression As Object, ByVal NumDigitsAfterDecimal As Integer) As String
        Return FormatNumber(Expression, NumDigitsAfterDecimal, 0, 0, 0)
    End Function
    Public Shared Function FormatNumber(ByVal Expression As Object, ByVal NumDigitsAfterDecimal As Integer, ByVal IncludeLeadingDigit As Integer, ByVal UseParensForNegativeNumbers As Integer, ByVal GroupDigits As Integer) As String
        If Not Information.IsNumeric(Expression) Then Return Convert.ToString(Expression)
        If IsDBNull(Expression) Then Return String.Empty
        Return FormatNumber(Expression, NumDigitsAfterDecimal, CType(IncludeLeadingDigit, TriState), CType(UseParensForNegativeNumbers, TriState), CType(GroupDigits, TriState))
    End Function
    Public Shared Function FormatCurrency(ByVal Expression As Object, ByVal NumDigitsAfterDecimal As Integer) As String
        Return FormatCurrency(Expression, NumDigitsAfterDecimal, 0, 0, 0)
    End Function
    Public Shared Function FormatCurrency(ByVal Expression As Object, ByVal NumDigitsAfterDecimal As Integer, ByVal IncludeLeadingDigit As Integer, ByVal UseParensForNegativeNumbers As Integer, ByVal GroupDigits As Integer) As String
        If Not Information.IsNumeric(Expression) Then Return Convert.ToString(Expression)
        If IsDBNull(Expression) Then Return String.Empty
        Return FormatCurrency(Expression, NumDigitsAfterDecimal, CType(IncludeLeadingDigit, TriState), CType(UseParensForNegativeNumbers, TriState), CType(GroupDigits, TriState))
    End Function
    Public Shared Sub p2p_Write(ByVal value As Object)
        HttpContext.Current.Response.Write(value)
    End Sub
    Public Shared Sub WriteUploadPaths()
        p2p_Write(String.Format("APPL_PHYSICAL_PATH = {0}<br>", HttpContext.Current.Request.ServerVariables("APPL_PHYSICAL_PATH")))
        p2p_Write(String.Format("APPL_MD_PATH = {0}<br>", HttpContext.Current.Request.ServerVariables("APPL_MD_PATH")))
    End Sub
    Public Shared Function GetPageName(ByVal url As String) As String
        If url <> "" Then
            If url.Contains("?") Then
                url = url.Substring(0, url.LastIndexOf("?")) ' Remove querystring first
            End If
            Return url.Substring(url.LastIndexOf("/") + 1) ' Remove path
        Else
            Return ""
        End If
    End Function
    Public Shared Function ReferPage() As String
        Return GetPageName(HttpContext.Current.Request.ServerVariables("HTTP_REFERER"))
    End Function
    Public Shared Function ConvertFullUrl(ByVal url As String) As String
        If url = "" Then
            Return ""
        ElseIf url.Contains("://") Then
            Return url
        Else
            Dim sUrl As String = FullUrl()
            Return sUrl.Substring(0, sUrl.LastIndexOf("/") + 1) & url
        End If
    End Function
    Public Shared Function DomainUrl() As String
        Dim sUrl As String
        Dim bSSL As Boolean = Not p2p_SameText(HttpContext.Current.Request.ServerVariables("HTTPS"), "off")
        Dim sPort As String = HttpContext.Current.Request.ServerVariables("SERVER_PORT")
        Dim defPort As String = CStr(IIf(bSSL, "443", "80"))
        If sPort = defPort Then
            sPort = ""
        Else
            sPort = ":" & sPort
        End If
        If bSSL Then
            sUrl = "http" & "s"
        Else
            sUrl = "http"
        End If
        Return String.Format("{0}://{1}{2}", sUrl, HttpContext.Current.Request.ServerVariables("SERVER_NAME"), sPort)
    End Function
    Public Shared Function FullUrl() As String
        Return DomainUrl() & HttpContext.Current.Request.ServerVariables("SCRIPT_NAME")
    End Function
    Public Shared Function p2p_SameText(ByVal v1 As Object, ByVal v2 As Object) As Boolean
        Return String.Equals(Convert.ToString(v1).Trim().ToLower(), Convert.ToString(v2).Trim().ToLower())
    End Function
    Public Const p2p_DATE_SEPARATOR As String = "/"
    Public Shared Function UnFormatDateTime(ByVal ADate As String, ByVal ANamedFormat As Integer) As String
        Dim arDate() As String
        Dim arDateTime() As String
        Dim d As DateTime
        Dim sDT As String
        ADate = Convert.ToString(ADate).Trim()
        While ADate.Contains("  ")
            ADate = ADate.Replace("  ", " ")
        End While
        arDateTime = ADate.Split(New Char() {" "c})
        If ANamedFormat = 0 AndAlso IsDate(ADate) Then
            d = Convert.ToDateTime(arDateTime(0))
            sDT = d.ToString("yyyy/MM/dd")
            If arDateTime.GetUpperBound(0) > 0 Then
                For i As Integer = 1 To arDateTime.GetUpperBound(0)
                    sDT = String.Format("{0} {1}", sDT, arDateTime(i))
                Next
            End If
            Return sDT
        Else
            arDate = arDateTime(0).Split(New Char() {Convert.ToChar(p2p_DATE_SEPARATOR)})
            If arDate.GetUpperBound(0) = 2 Then
                sDT = arDateTime(0)
                If ANamedFormat = 6 OrElse ANamedFormat = 10 Then ' mmddyyyy
                    If arDate(0).Length <= 2 AndAlso arDate(1).Length <= 2 AndAlso arDate(2).Length <= 4 Then
                        sDT = String.Format("{0}/{1}/{2}", arDate(2), arDate(0), arDate(1))
                    End If
                ElseIf ANamedFormat = 7 OrElse ANamedFormat = 11 Then ' ddmmyyyy
                    If arDate(0).Length <= 2 AndAlso arDate(1).Length <= 2 AndAlso arDate(2).Length <= 4 Then
                        sDT = String.Format("{0}/{1}/{2}", arDate(2), arDate(1), arDate(0))
                    End If
                ElseIf ANamedFormat = 5 OrElse ANamedFormat = 9 Then ' yyyymmdd
                    If arDate(0).Length <= 4 AndAlso arDate(1).Length <= 2 AndAlso arDate(2).Length <= 2 Then
                        sDT = String.Format("{0}/{1}/{2}", arDate(0), arDate(1), arDate(2))
                    End If
                End If
                If arDateTime.GetUpperBound(0) > 0 Then
                    If IsDate(arDateTime(1)) Then ' Is time
                        sDT = String.Format("{0} {1}", sDT, arDateTime(1))
                    End If
                End If
                Return sDT
            Else
                Return ADate
            End If
        End If
    End Function
    Public Shared Function FormatDateTime(ByVal ADate As Object, ByVal ANamedFormat As Integer) As String
        Dim sDT As String
        If IsDate(ADate) Then
            Dim DT As DateTime = Convert.ToDateTime(ADate)
            If ANamedFormat >= 0 AndAlso ANamedFormat <= 4 Then
                ' sDT = FormatDateTime(CDate(ADate), CType(ANamedFormat, DateFormat))
                sDT = DT.Month & p2p_DATE_SEPARATOR & DT.Day & p2p_DATE_SEPARATOR & DT.Year
            ElseIf ANamedFormat = 5 OrElse ANamedFormat = 9 Then
                sDT = DT.Year & p2p_DATE_SEPARATOR & DT.Month & p2p_DATE_SEPARATOR & DT.Day
            ElseIf ANamedFormat = 6 OrElse ANamedFormat = 10 Then
                sDT = DT.Month & p2p_DATE_SEPARATOR & DT.Day & p2p_DATE_SEPARATOR & DT.Year
            ElseIf ANamedFormat = 7 OrElse ANamedFormat = 11 Then
                sDT = DT.Day & p2p_DATE_SEPARATOR & DT.Month & p2p_DATE_SEPARATOR & DT.Year
            ElseIf ANamedFormat = 8 Then
                If DT.Hour <> 0 OrElse DT.Minute <> 0 OrElse DT.Second <> 0 Then
                    sDT = String.Format("{0} {1:HH:mm:ss}", FormatDateTime(CDate(ADate), CType(2, DateFormat)), DT)
                Else
                    sDT = FormatDateTime(CDate(ADate), CType(2, DateFormat))
                End If
            ElseIf ANamedFormat = 12 Then
                sDT = DT.ToString("HH:mm:ss")
            Else
                Return Convert.ToString(DT)
            End If
            If ANamedFormat >= 9 AndAlso ANamedFormat <= 11 Then
                sDT = String.Format("{0} {1:HH:mm:ss}", sDT, DT)
            End If
            Return sDT
        Else
            Return Convert.ToString(ADate)
        End If
    End Function
    Public Const p2p_DEFAULT_DATE_FORMAT As Short = 6
    Public Shared Function CurrentDateTime() As String
        Dim DT As DateTime = Now()
        If p2p_DEFAULT_DATE_FORMAT = 6 OrElse p2p_DEFAULT_DATE_FORMAT = 7 Then
            CurrentDateTime = FormatDateTime(DT, p2p_DEFAULT_DATE_FORMAT)
        Else
            CurrentDateTime = FormatDateTime(DT, 5)
        End If
        CurrentDateTime = String.Format("{0} {1:HH:mm:ss}", CurrentDateTime, DT)
    End Function
    Public Shared Function CurrentTime() As String
        Dim DT As DateTime = Now()
        Return DT.ToString("HH:mm:ss")
    End Function
    Public Shared Function CurrentDate() As String
        If p2p_DEFAULT_DATE_FORMAT = 6 OrElse p2p_DEFAULT_DATE_FORMAT = 7 Then
            Return FormatDateTime(Today, p2p_DEFAULT_DATE_FORMAT)
        Else
            Return FormatDateTime(Today, 5)
        End If
    End Function
    Public Shared Function CurrentHost() As String
        Return HttpContext.Current.Request.ServerVariables("HTTP_HOST")
    End Function
    Public Shared Function CurrentUserIP() As String
        Return HttpContext.Current.Request.ServerVariables("REMOTE_HOST")
    End Function
    Public Shared Function ConvertToBool(ByVal value As Object) As Boolean
        Try
            If Information.IsNumeric(value) Then
                Return Convert.ToBoolean(ConvertToDouble(value))
            Else
                Return Convert.ToBoolean(value)
            End If
        Catch
            Return False
        End Try
    End Function
    Public Shared Function ConvertToDouble(ByVal value As Object) As Double
        Try
            Return Convert.ToDouble(value)
        Catch
            Return 0
        End Try
    End Function
    Public Shared Function ConvertToInt(ByVal value As Object) As Integer
        Try
            Return Convert.ToInt32(value)
        Catch
            Return 0
        End Try
    End Function

    ' Get/set session values
    Public Shared Property SessionObject(ByVal name As String) As Object
        Get
            Return HttpContext.Current.Session(name)
        End Get
        Set(ByVal Value As Object)
            HttpContext.Current.Session(name) = Value
        End Set
    End Property

    ' Get current page name
    Public Shared Function CurrentPage() As String
        Return GetPageName(HttpContext.Current.Request.ServerVariables("SCRIPT_NAME"))
    End Function

    ' Get current URL
    Public Shared Function CurrentUrl() As String
        Dim q As String = HttpContext.Current.Request.ServerVariables("QUERY_STRING")
        Dim s As String = HttpContext.Current.Request.ServerVariables("SCRIPT_NAME")
        If q <> "" Then s &= "?" & q
        Return s
    End Function

    ' Check if folder exists
    Public Shared Function FolderExists(ByVal folder As String) As Boolean
        Return Directory.Exists(folder)
    End Function

    ' Check if file exists
    Public Shared Function FileExists(ByVal folder As String, ByVal fn As String) As Boolean
        Return File.Exists(folder & fn)
    End Function

    ' Compare object as string
    Public Shared Function SameStr(ByVal v1 As Object, ByVal v2 As Object) As Boolean
        Return String.Equals(Convert.ToString(v1).Trim(), Convert.ToString(v2).Trim())
    End Function

    ' Check if empty string
    Public Shared Function IsEmpty(ByVal value As Object) As Boolean
        Return String.Equals(Convert.ToString(value).Trim(), String.Empty)
    End Function

    ' Check if not empty string
    Public Shared Function IsNotEmpty(ByVal value As Object) As Boolean
        Return Not IsEmpty(value)
    End Function
End Class
