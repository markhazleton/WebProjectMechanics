Imports System.Data
Imports System.Data.Common
Imports System.IO
Imports System.Security.Cryptography
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Drawing.Drawing2D
Imports System.Reflection
Imports System.Data.OleDb

'
' ASP.NET Maker 7 Project Class (Shared Functions)
' (C)2009 e.World Technology Limited. All rights reserved.
'
Public Partial Class AspNetMaker7_WPMGen
	Inherits wpmPage

	' Compare object as string
	Public Shared Function ew_SameStr(v1 As Object, v2 As Object) As Boolean
		Return String.Equals(Convert.ToString(v1).Trim(), Convert.ToString(v2).Trim())
	End Function

	' Compare object as string (case insensitive)
	Public Shared Function ew_SameText(v1 As Object, v2 As Object) As Boolean
		Return String.Equals(Convert.ToString(v1).Trim().ToLower(), Convert.ToString(v2).Trim().ToLower())
	End Function

	' Check if empty string
	Public Shared Function ew_Empty(value As Object) As Boolean
		Return String.Equals(Convert.ToString(value).Trim(), String.Empty)
	End Function

	' Check if not empty string
	Public Shared Function ew_NotEmpty(value As Object) As Boolean
		Return Not ew_Empty(value)
	End Function

	' Convert object to integer
	Public Shared Function ew_ConvertToInt(value As Object) As Integer
		Try
			Return Convert.ToInt32(value)
		Catch
			Return 0
		End Try
	End Function

	' Convert object to double
	Public Shared Function ew_ConvertToDouble(value As Object) As Double
		Try
			Return Convert.ToDouble(value)
		Catch
			Return 0
		End Try
	End Function

	' Convert object to bool
	Public Shared Function ew_ConvertToBool(ByVal value As Object) As Boolean
		Try
			If Information.IsNumeric(value) Then
				Return Convert.ToBoolean(ew_ConvertToDouble(value))
			Else
				Return Convert.ToBoolean(value)
			End If
		Catch
			Return False
		End Try
	End Function

	' Get user IP
	Public Shared Function ew_CurrentUserIP() As String
		Return HttpContext.Current.Request.ServerVariables("REMOTE_HOST")
	End Function	

	' Get current host name, e.g. "www.mycompany.com"
	Public Shared Function ew_CurrentHost() As String
		Return HttpContext.Current.Request.ServerVariables("HTTP_HOST")
	End Function	

	' Get current date in default date format
	Public Shared Function ew_CurrentDate() As String
		If EW_DEFAULT_DATE_FORMAT = 6 OrElse EW_DEFAULT_DATE_FORMAT = 7 Then
			Return ew_FormatDateTime(Today, EW_DEFAULT_DATE_FORMAT)
		Else
			Return ew_FormatDateTime(Today, 5)
		End If
	End Function	

	' Get current time in hh:mm:ss format
	Public Shared Function ew_CurrentTime() As String
		Dim DT As DateTime = Now()
		Return DT.ToString("HH:mm:ss")
	End Function	

	' Get current date in default date format with
	' Current time in hh:mm:ss format
	Public Shared Function ew_CurrentDateTime() As String
		Dim DT As DateTime = Now()
		If EW_DEFAULT_DATE_FORMAT = 6 OrElse EW_DEFAULT_DATE_FORMAT = 7 Then
			ew_CurrentDateTime = ew_FormatDateTime(DT, EW_DEFAULT_DATE_FORMAT)
		Else
			ew_CurrentDateTime = ew_FormatDateTime(DT, 5)
		End If
		ew_CurrentDateTime = ew_CurrentDateTime & " " & DT.ToString("HH:mm:ss")
	End Function	

	' Remove XSS
	Public Shared Function ew_RemoveXSS(val As Object) As Object		 
		Dim val_before As String, pattern As String, replacement As String

		' Handle null value
		If IsDBNull(val) Then Return val

		' Remove all non-printable characters. CR(0a) and LF(0b) and TAB(9) are allowed 
		' This prevents some character re-spacing such as <java\0script> 
		' Note that you have to handle splits with \n, \r, and \t later since they *are* allowed in some inputs

		Dim regEx As Regex = New Regex("([\x00-\x08][\x0b-\x0c][\x0e-\x20])", RegexOptions.IgnoreCase) ' Create regular expression.
		val = regEx.Replace(Convert.ToString(val), "")

		' Straight replacements, the user should never need these since they're normal characters 
		' This prevents like <IMG SRC=&#X40&#X61&#X76&#X61&#X73&#X63&#X72&#X69&#X70&#X74&#X3A&#X61&#X6C&#X65&#X72&#X74&#X28&#X27&#X58&#X53&#X53&#X27&#X29> 

		Dim search As String = "abcdefghijklmnopqrstuvwxyz"
		search = search & "ABCDEFGHIJKLMNOPQRSTUVWXYZ" 
		search = search & "1234567890!@#$%^&*()" 
		search = search & "~`"";:?+/={}[]-_|'\"
		For i As Integer = 0 To search.Length - 1

			' ;? matches the ;, which is optional 
			' 0{0,7} matches any padded zeros, which are optional and go up to 8 chars	
			' &#x0040 @ search for the hex values

			regEx = New Regex("(&#[x|X]0{0,8}" & Hex(Asc(search(i))) & ";?)") ' With a ;
			val = regEx.Replace(val, search(i))

			' &#00064 @ 0{0,7} matches '0' zero to seven times
			regEx = New Regex("(&#0{0,8}" & Asc(search(i)) & ";?)") ' With a ;
			val = regEx.Replace(val, search(i))
		Next

		' Now the only remaining whitespace attacks are \t, \n, and \r	
		Dim Found As Boolean = True ' Keep replacing as long as the previous round replaced something 
		Do While Found
			val_before = val
			For i As Integer = 0 To EW_REMOVE_XSS_KEYWORDS.GetUpperBound(0)
				pattern = ""
				For j As Integer = 0 To EW_REMOVE_XSS_KEYWORDS(i).Length - 1
					If j > 0 Then
						pattern = pattern & "("
						pattern = pattern & "(&#[x|X]0{0,8}([9][a][b]);?)?"
						pattern = pattern & "|(&#0{0,8}([9][10][13]);?)?"
						pattern = pattern & ")?"
					End If
					pattern = pattern & EW_REMOVE_XSS_KEYWORDS(i)(j)
				Next
				replacement = EW_REMOVE_XSS_KEYWORDS(i).Substring(0, 2) & "<x>" & EW_REMOVE_XSS_KEYWORDS(i).Substring(2) ' Add in <> to nerf the tag
				regEx = New Regex(pattern)
				val = regEx.Replace(val, replacement) ' Filter out the hex tags
				If val_before = val Then					
					Found = False ' No replacements were made, so exit the loop
				End If
			Next
		Loop
		Return val
	End Function

	' Highlight keywords
	Public Shared Function ew_Highlight(name As String, src As Object, bkw As String, bkwtype As Object, akw As String) As String
		Dim kw As String, kwlist() As String, kwstr As String = "", wrksrc As String, outstr As String = ""
		Dim x As Integer, y As Integer, xx As Integer, yy As Integer
		If ew_NotEmpty(src) AndAlso (ew_NotEmpty(bkw) OrElse ew_NotEmpty(akw)) Then
			src = Convert.ToString(src)
			xx = 0
			yy = src.IndexOf("<", xx)
			If yy < 0 Then yy = src.Length
			Do While yy > 0
				If yy > xx Then
					wrksrc = src.Substring(xx, yy - xx)
					If ew_NotEmpty(bkw) Then kwstr = bkw.Trim()
					If ew_NotEmpty(akw) Then
						If kwstr.Length > 0 Then kwstr = kwstr & " "
						kwstr = kwstr & akw.Trim()
					End If
					kwlist = kwstr.Split(New Char() {" "})
					x = 0
					ew_GetKeyword(wrksrc, kwlist, x, y, kw)
					Do While y >= 0
						outstr = outstr & wrksrc.Substring(x, y - x) & _
							"<span id=""" & name & """ name=""" & name & """ class=""ewHighlightSearch"">" & _
							wrksrc.Substring(y, kw.Length) & "</span>"
						x = y + kw.Length
						ew_GetKeyword(wrksrc, kwlist, x, y, kw)
					Loop 
					outstr = outstr & wrksrc.Substring(x)
					xx = xx + wrksrc.Length
				End If
				If xx < src.Length Then
					yy = src.IndexOf(">", xx)
					If yy >= 0 Then
						outstr = outstr & src.Substring(xx, yy - xx + 1)
						xx = yy + 1
						yy = src.IndexOf("<", xx)
						If yy < 0 Then yy = src.Length + 1
					Else
						outstr = outstr & src.Substring(xx)
						yy = -1
					End If
				Else
					yy = -1
				End If
			Loop
		Else
			outstr = Convert.ToString(src)
		End If
		Return outstr
	End Function

	' Get keyword
	Public Shared Sub ew_GetKeyword(src As String, kwlist As String(), x As Integer, ByRef y As Integer, ByRef kw As String)
		Dim wrky As Integer, thisy As Integer = -1, thiskw As String = "", wrkkw As String
		For i As Integer = 0 To kwlist.GetUpperBound(0)
			wrkkw = kwlist(i).Trim()
			If wrkkw <> "" Then
				If EW_HIGHLIGHT_COMPARE Then ' Case-insensitive
					wrky = src.IndexOf(wrkkw, x, StringComparison.CurrentCultureIgnoreCase)
				Else
					wrky = src.IndexOf(wrkkw, x)
				End If
				If wrky > -1 Then
					If thisy = -1 Then
						thisy = wrky
						thiskw = wrkkw
					ElseIf wrky < thisy Then 
						thisy = wrky
						thiskw = wrkkw
					End If
				End If
			End If
		Next 
		y = thisy
		kw = thiskw
	End Sub

	'
	' Security shortcut functions
	'
	' Get current user name
	Public Shared Function CurrentUserName() As String		
		Return Convert.ToString(ew_Session(EW_SESSION_USER_NAME))
	End Function	

	' Get current user ID
	Public Shared Function CurrentUserID() As Object
		Return Convert.ToString(ew_Session(EW_SESSION_USER_ID))
	End Function	

	' Get current parent user ID
	Public Shared Function CurrentParentUserID() As Object
		Return Convert.ToString(ew_Session(EW_SESSION_PARENT_USER_ID))
	End Function	

	' Get current user level
	Public Shared Function CurrentUserLevel() As Integer
		Return ew_Session(EW_SESSION_USER_LEVEL_ID)
	End Function

	' Is Logged In
	Public Shared Function IsLoggedIn() As Boolean
		Return ew_SameStr(ew_Session(EW_SESSION_STATUS), "login")
	End Function	

	' Is System Admin
	Public Shared Function IsSysAdmin() As Boolean
		Return (ew_Session(EW_SESSION_SYS_ADMIN) = 1)
	End Function

	' Execute SQL
	Public Shared Function ew_Execute(Sql As String) As Integer
		Dim c As New cConnection()
		Try
			Return c.Execute(Sql)
		Finally
			c.Dispose()
		End Try
	End Function

	' Execute SQL and return first value of first row
	Public Shared Function ew_ExecuteScalar(Sql As String) As Object
		Dim c As New cConnection()
		Try
			Return c.ExecuteScalar(Sql)
		Finally
			c.Dispose()
		End Try
	End Function

	' Execute SQL and return first rowr
	Public Shared Function ew_ExecuteRow(Sql As String) As OrderedDictionary
		Dim dr As OleDbDataReader = Nothing
		Dim c As New cConnection()
		Try
			dr = c.GetDataReader(Sql)
			If dr.Read() Then
				Return c.GetRow(dr)
			Else
				Return Nothing
			End If
		Finally
			If dr IsNot Nothing Then
				dr.Close()
				dr.Dispose()
			End If
			c.Dispose()
		End Try
	End Function

	'
	' TEA encrypt/decrypt class
	'
	Public Class cTEA

    Public Shared Function Encrypt(ByVal Data As String, ByVal Key As String) As String
			Try
				If Data.Length = 0 Then
					Throw New ArgumentException("Data must be at least 1 character in length.")
				End If
				Dim formattedKey As UInteger() = FormatKey(Key)
				If Data.Length Mod 2 <> 0 Then Data += Chr(0) ' Make sure array is even in length
				Dim dataBytes As Byte() = System.Text.ASCIIEncoding.ASCII.GetBytes(Data)
				Dim cipher As String = String.Empty
				Dim tempData As UInteger() = New UInteger(1) {}
				For i As Integer = 0 To dataBytes.Length - 1 Step 2
					tempData(0) = dataBytes(i)
					tempData(1) = dataBytes(i + 1)
					Code(tempData, formattedKey)
					cipher += ConvertUIntToString(tempData(0)) + ConvertUIntToString(tempData(1))
				Next
				Return UrlEncode(cipher)
			Catch
				Return Data	
			End Try
    End Function

    Public Shared Function Decrypt(ByVal Data As String, ByVal Key As String) As String
			Try
				Data = UrlDecode(Data)
				Dim formattedKey As UInteger() = FormatKey(Key)
				Dim x As Integer = 0
				Dim tempData As UInteger() = New UInteger(1) {}
				Dim dataBytes As Byte() = New Byte(Data.Length / 8 * 2 - 1) {}
				For i As Integer = 0 To Data.Length - 1 Step 8
					tempData(0) = ConvertStringToUInt(Data.Substring(i, 4))
					tempData(1) = ConvertStringToUInt(Data.Substring(i + 4, 4))
					Decode(tempData, formattedKey)
					dataBytes(x) = CByte(tempData(0))
					x += 1
					dataBytes(x) = CByte(tempData(1))
					x += 1
				Next
				Dim decipheredString As String = System.Text.ASCIIEncoding.ASCII.GetString(dataBytes, 0, dataBytes.Length)
				If decipheredString(decipheredString.Length - 1) = Chr(0) Then ' Strip the null char if it was added
					decipheredString = decipheredString.Substring(0, decipheredString.Length - 1)
				End If
				Return decipheredString
			Catch
				Return Data
			End Try
    End Function

    Private Shared Function FormatKey(ByVal Key As String) As UInteger()
			If Key.Length = 0 Then
				Throw New ArgumentException("Key must be between 1 and 16 characters in length")
			End If
			Key = Key.PadRight(16, " "c).Substring(0, 16) ' Ensure that the key is 16 chars in length
			Dim formattedKey As UInteger() = New UInteger(3) {}

			' Get the key into the correct format for TEA usage
			Dim j As Integer = 0
			For i As Integer = 0 To Key.Length - 1 Step 4
				formattedKey(j) = ConvertStringToUInt(Key.Substring(i, 4))
				j += 1
			Next
			Return formattedKey
    End Function

    Private Shared Function Add(ByVal v1 As ULong, ByVal v2 As ULong) As UInteger
			Dim t As ULong
			If v1 = 4294967295 And v2 = 4294967295 Then
				t = 0
			Else
			  t = v1 + v2
			End If
			If t > 2 ^ 32 Then t = t - 2 ^ 32
			Return t
    End Function

    Private Shared Function Minus(ByVal v1 As Long, ByVal v2 As Long) As UInteger
			Dim t As Long
			t = v1 - v2
			If t > 2 ^ 32 Then
				t = t - 2 ^ 32
			ElseIf t < 0 Then
				t = t + 2 ^ 32
			End If
			Return t
    End Function

    Private Shared Sub Code(ByVal v As UInteger(), ByVal k As UInteger())
			Dim y As UInteger = v(0)
			Dim z As UInteger = v(1)
			Dim sum As UInteger = 0
			Dim delta As UInteger = 2654435769
			Dim n As UInteger = 32
			While n > 0
				y = Add(y, Add(z << 4 Xor z >> 5, z) Xor Add(sum, k(sum And 3)))
				sum = Add(sum, delta)
				z = Add(z, Add(y << 4 Xor y >> 5, y) Xor (Add(sum, k((sum >> 11) And 3))))
				n -= 1
			End While
			v(0) = y
			v(1) = z
    End Sub

    Private Shared Sub Decode(ByVal v As UInteger(), ByVal k As UInteger())
			Dim y As UInteger = v(0)
			Dim z As UInteger = v(1)
			Dim sum As UInteger = 3337565984
			Dim delta As UInteger = 2654435769
			Dim n As UInteger = 32
			While n > 0
				z = Minus(z, Add(y << 4 Xor y >> 5, y) Xor Add(sum, k(sum >> 11 And 3)))
				sum = Minus(sum, delta)
				y = Minus(y, Add(z << 4 Xor z >> 5, z) Xor Add(sum, k(sum And 3)))
				n -= 1
			End While
			v(0) = y
			v(1) = z
    End Sub

    Private Shared Function ConvertStringToUInt(ByVal Input As String) As UInteger
			Dim output As UInteger
			output = Convert.ToUInt32(Input(0))
			output += (Convert.ToUInt32(Input(1)) << 8)
			output += (Convert.ToUInt32(Input(2)) << 16)
			output += (Convert.ToUInt32(Input(3)) << 24)
			Return output
    End Function

    Private Shared Function ConvertUIntToString(ByVal Input As UInteger) As String
			Dim output As New System.Text.StringBuilder()
			output.Append(Convert.ToChar(Input And 255))
			output.Append(Convert.ToChar((Input >> 8) And 255))
			output.Append(Convert.ToChar((Input >> 16) And 255))
			output.Append(Convert.ToChar((Input >> 24) And 255))
			Return output.ToString()
    End Function

    Private Shared Function UrlEncode(ByVal str As String) As String
			Dim encoding As New System.Text.UnicodeEncoding()
			str = Convert.ToBase64String(encoding.GetBytes(str))
			str = str.Replace("+"c, "-"c)
			str = str.Replace("/"c, "_"c)
			str = str.Replace("="c, "."c)
			Return str
    End Function

    Private Shared Function UrlDecode(ByVal str As String) As String
			str = str.Replace("-"c, "+"c)
			str = str.Replace("_"c, "/"c)
			str = str.Replace("."c, "="c)
			Dim dataBytes As Byte() = Convert.FromBase64String(str)
			Dim encoding As New System.Text.UnicodeEncoding()
			Return encoding.GetString(dataBytes)
    End Function
	End Class

	' Save binary to file
	Public Shared Function ew_SaveFile(folder As String, fn As String, ByRef filedata As Byte()) As Boolean
		If ew_CreateFolder(folder) Then
			Try
				Dim fs As FileStream
				fs = New FileStream(folder & fn, FileMode.Create)
				fs.Write(filedata, 0, filedata.Length)
				fs.Close()
				Return True
			Catch
				If EW_DEBUG_ENABLED Then Throw
				Return False
			End Try			
		End If
	End Function

	'
	' Common base class
	'
	Class AspNetMakerBase

		' Parent page (The ASP.NET page inherited from wpmPage)
		Protected m_ParentPage As AspNetMaker7_WPMGen

		' Page (ASP.NET Maker page, e.g. List/View/Add/Edit/Delete)
		Protected m_Page As AspNetMakerPage

		' Parent page
		Public Property ParentPage() As AspNetMaker7_WPMGen
			Get				
				Return m_ParentPage
			End Get
			Set(ByVal v As AspNetMaker7_WPMGen)
				m_ParentPage = v	
			End Set					
		End Property

		' Page
		Public Property Page() As AspNetMakerPage
			Get				
				Return m_Page
			End Get
			Set(ByVal v As AspNetMakerPage)
				m_Page = v	
			End Set						
		End Property

		' Connection
		Public Property Conn() As cConnection
			Get				
				Return ParentPage.Conn
			End Get
			Set(ByVal v As cConnection)
				ParentPage.Conn = v	
			End Set			
		End Property

		' Security
		Public Property Security() As cAdvancedSecurity
			Get				
				Return ParentPage.Security
			End Get
			Set(ByVal v As cAdvancedSecurity)
				ParentPage.Security = v	
			End Set	
		End Property

		' Form
		Public Property ObjForm() As cFormObj
			Get				
				Return ParentPage.ObjForm
			End Get
			Set(ByVal v As cFormObj)
				ParentPage.ObjForm = v	
			End Set	
		End Property		
	End Class

	'
	' Common page class
	'
	Class AspNetMakerPage
		Inherits AspNetMakerBase		

		' Page ID
		Protected m_PageID As String = ""

		Public ReadOnly Property PageID() As String
			Get
				Return m_PageID
			End Get
		End Property

		' Table name
		Protected m_TableName As String = ""

		Public ReadOnly Property TableName() As String
			Get
				Return m_TableName
			End Get
		End Property

		' Page object name
		Protected m_PageObjName As String = ""

		Public ReadOnly Property PageObjName() As String
			Get
				Return m_PageObjName
			End Get
		End Property

		' Page object type name
		Protected m_PageObjTypeName As String = ""

		Public ReadOnly Property PageObjTypeName() As String
			Get
				Return m_PageObjTypeName
			End Get
		End Property

		' Page Name
		Public ReadOnly Property PageName() As String
			Get
				Return ew_CurrentPage()
			End Get
		End Property

		' Message
		Public Property Message() As String
			Get
				Return ew_Session(EW_SESSION_MESSAGE)
			End Get
			Set(ByVal v As String)
				If ew_NotEmpty(ew_Session(EW_SESSION_MESSAGE)) Then
					If Not ew_SameStr(ew_Session(EW_SESSION_MESSAGE), v) Then ' Append
						ew_Session(EW_SESSION_MESSAGE) = ew_Session(EW_SESSION_MESSAGE) & "<br>" & v
					End If
				Else
					ew_Session(EW_SESSION_MESSAGE) = v
				End If
			End Set	
		End Property

		' Show Message
		Public Sub ShowMessage()
			If Message <> "" Then ' Message in Session, display
				ew_Write("<p><span class=""ewMessage"">" & Message & "</span></p>")
				ew_Session(EW_SESSION_MESSAGE) = "" ' Clear message in Session
			End If
		End Sub			
	End Class

	'
	' Email class
	'
	Class cEmail

		Public Sender As String ' Sender

		Public Recipient As String ' Recipient

		Public Cc As String = "" ' Cc

		Public Bcc As String = "" ' Bcc

		Public Subject As String ' Subject

		Public Format As String ' Format

		Public Content As String ' Content

		' Load email from template
		Public Sub Load(fn As String)
			Dim sHeader As String, sWrk As String, arrHeader() As String
			Dim sName As String, sValue As String
			Dim i, j As Integer
			sWrk = ew_LoadTxt(fn) ' Load text file content
			sWrk = sWrk.Replace(vbCrLf, vbLf) ' Convert to Lf
			sWrk = sWrk.Replace(vbCr, vbLf) ' Convert to Lf
			If sWrk <> "" Then 
				i = sWrk.IndexOf(vbLf & vbLf) ' Locate header and mail content
				If i > 0 Then
					sHeader = sWrk.Substring(0, i + 1)
					Content = sWrk.Substring(i + 2)
					arrHeader = sHeader.Split(New Char() {vbLf})
					For j = 0 To arrHeader.GetUpperBound(0)
						i = arrHeader(j).IndexOf(":")
						If i > 0 Then
							sName = arrHeader(j).Substring(0, i).Trim()
							sValue = arrHeader(j).Substring(i + 1).Trim()
							Select Case sName.ToLower()
								Case "subject"
									Subject = sValue
								Case "from"
									Sender = sValue
								Case "to"
									Recipient = sValue
								Case "cc"
									Cc = sValue
								Case "bcc"
									Bcc = sValue
								Case "format"
									Format = sValue
							End Select
						End If
					Next 
				End If
			End If
		End Sub

		' Replace sender
		Public Sub ReplaceSender(ASender As String)
			Sender = Sender.Replace("<!--$From-->", ASender)
		End Sub

		' Replace recipient
		Public Sub ReplaceRecipient(ARecipient As String)
			Recipient = Recipient.Replace("<!--$To-->", ARecipient)
		End Sub

		' Add cc email
		Public Sub AddCc(ACc As String)
			If ACc <> "" Then
				If Cc <> "" Then Cc = Cc & ";"
				Cc = Cc & ACc
			End If
		End Sub

		' Add bcc email
		Public Sub AddBcc(ABcc As String)
			If ABcc <> "" Then
				If Bcc <> "" Then Bcc = Bcc & ";"
				Bcc = Bcc & ABcc
			End If
		End Sub

		' Replace subject
		Public Sub ReplaceSubject(ASubject As String)
			Subject = Subject.Replace("<!--$Subject-->", ASubject)
		End Sub

		' Replace content
		Public Sub ReplaceContent(Find As String, ReplaceWith As String)
			Content = Content.Replace(Find, ReplaceWith)
		End Sub

		' Send email
		Public Function Send() As Boolean
			Return ew_SendEmail(Sender, Recipient, Cc, Bcc, Subject, Content, Format)
		End Function

		' Display as string
		Public Function AsString() As String
			Return "{" & "Sender: " & Sender & ", " & "Recipient: " & Recipient & ", " & "Cc: " & Cc & ", " & "Bcc: " & Bcc & ", " & "Subject: " & Subject & ", " & "Format: " & Format & ", " & "Content: " & Content & "}"
		End Function
	End Class	

	'
	' Class for Pager item
	'	
	Class cPagerItem

		Public Text As String

		Public Start As Integer

		Public Enabled As Boolean

		' Constructor
		Public Sub New(AStart As Integer, AText As String, AEnabled As Boolean)
			Text = AText
			Start = AStart
			Enabled = AEnabled
		End Sub

		' Constructor
		Public Sub New()

			' Do nothing
		End Sub	
	End Class	

	'
	' Class for Numeric pager
	'	
	Class cNumericPager

		Public Items As New ArrayList

		Public PageSize As Integer, ToIndex As Integer, Count As Integer, FromIndex As Integer, RecordCount As Integer, Range As Integer

		Public LastButton As cPagerItem, PrevButton As cPagerItem, FirstButton As cPagerItem, NextButton As cPagerItem

		Public ButtonCount As Integer

		Public Visible As Boolean

		' Constructor
		Public Sub New(AFromIndex As Integer, APageSize As Integer, ARecordCount As Integer, ARange As Integer)
			FromIndex = AFromIndex
			PageSize = APageSize
			RecordCount = ARecordCount
			Range = ARange
			FirstButton = New cPagerItem
			PrevButton = New cPagerItem
			NextButton = New cPagerItem
			LastButton = New cPagerItem
			Visible = True
			Init()
		End Sub

		' Init pager
		Public Sub Init()
			If FromIndex > RecordCount Then FromIndex = RecordCount
			ToIndex = FromIndex + PageSize - 1
			If ToIndex > RecordCount Then ToIndex = RecordCount
			Count = 0
			SetupNumericPager()

			' Update button count
			ButtonCount = Count + 1
			If FirstButton.Enabled Then ButtonCount = ButtonCount + 1
			If PrevButton.Enabled Then ButtonCount = ButtonCount + 1
			If NextButton.Enabled Then ButtonCount = ButtonCount + 1
			If LastButton.Enabled Then ButtonCount = ButtonCount + 1
		End Sub

		' Add pager item
		Private Sub AddPagerItem(StartIndex As Integer, Text As String, Enabled As Boolean)
			Items.Add(New cPagerItem(StartIndex, Text, Enabled))
			Count = Items.Count
		End Sub

		' Setup pager items
		Private Sub SetupNumericPager()
			Dim HasPrev As Boolean, NoNext As Boolean
			Dim dy2 As Integer, dx2 As Integer, y As Integer, x As Integer, dx1 As Integer, dy1 As Integer, ny As Integer, TempIndex As Integer
			If RecordCount > PageSize Then
				NoNext = (RecordCount < (FromIndex + PageSize))
				HasPrev = (FromIndex > 1)

				' First Button
				TempIndex = 1
				FirstButton.Start = TempIndex
				FirstButton.Enabled = (FromIndex > TempIndex)

				' Prev Button
				TempIndex = FromIndex - PageSize
				If TempIndex < 1 Then TempIndex = 1
				PrevButton.Start = TempIndex
				PrevButton.Enabled = HasPrev

				' Page links
				If HasPrev Or Not NoNext Then
					x = 1
					y = 1
					dx1 = ((FromIndex - 1) \ (PageSize * Range)) * PageSize * Range + 1
					dy1 = ((FromIndex - 1) \ (PageSize * Range)) * Range + 1
					If (dx1 + PageSize * Range - 1) > RecordCount Then
						dx2 = (RecordCount \ PageSize) * PageSize + 1
						dy2 = (RecordCount \ PageSize) + 1
					Else
						dx2 = dx1 + PageSize * Range - 1
						dy2 = dy1 + Range - 1
					End If
					While x <= RecordCount
						If x >= dx1 And x <= dx2 Then
							AddPagerItem(x, y, FromIndex <> x)
							x = x + PageSize
							y = y + 1
						ElseIf x >= (dx1 - PageSize * Range) And x <= (dx2 + PageSize * Range) Then 
							If x + Range * PageSize < RecordCount Then
								AddPagerItem(x, y & "-" & (y + Range - 1), True)
							Else
								ny = (RecordCount - 1) \ PageSize + 1
								If ny = y Then
									AddPagerItem(x, y, True)
								Else
									AddPagerItem(x, y & "-" & ny, True)
								End If
							End If
							x = x + Range * PageSize
							y = y + Range
						Else
							x = x + Range * PageSize
							y = y + Range
						End If
					End While
				End If

				' Next Button
				NextButton.Start = FromIndex + PageSize
				TempIndex = FromIndex + PageSize
				NextButton.Start = TempIndex
				NextButton.Enabled = Not NoNext

				' Last Button
				TempIndex = ((RecordCount - 1) \ PageSize) * PageSize + 1
				LastButton.Start = TempIndex
				LastButton.Enabled = (FromIndex < TempIndex)
			End If
		End Sub
	End Class	

	'
	' Class for PrevNext pager
	'	
	Class cPrevNextPager

		Public NextButton As cPagerItem, FirstButton As cPagerItem, PrevButton As cPagerItem, LastButton As cPagerItem

		Public ToIndex As Integer, PageCount As Integer, CurrentPage As Integer, PageSize As Integer, FromIndex As Integer, RecordCount As Integer

		Public Visible As Boolean

		' Constructor
		Public Sub New(AFromIndex As Integer, APageSize As Integer, ARecordCount As Integer)
			FromIndex = AFromIndex
			PageSize = APageSize
			RecordCount = ARecordCount
			FirstButton = New cPagerItem
			PrevButton = New cPagerItem
			NextButton = New cPagerItem
			LastButton = New cPagerItem
			Visible = True
			Init()
		End Sub

		' Method to init pager
		Public Sub Init()
			Dim TempIndex As Integer
			If PageSize > 0 Then
				CurrentPage = (FromIndex - 1) \ PageSize + 1
				PageCount = (RecordCount - 1) \ PageSize + 1
				If FromIndex > RecordCount Then FromIndex = RecordCount
				ToIndex = FromIndex + PageSize - 1
				If ToIndex > RecordCount Then ToIndex = RecordCount

				' First Button
				TempIndex = 1
				FirstButton.Start = TempIndex
				FirstButton.Enabled = (TempIndex <> FromIndex)

				' Prev Button
				TempIndex = FromIndex - PageSize
				If TempIndex < 1 Then TempIndex = 1
				PrevButton.Start = TempIndex
				PrevButton.Enabled = (TempIndex <> FromIndex)

				' Next Button
				TempIndex = FromIndex + PageSize
				If TempIndex > RecordCount Then TempIndex = FromIndex
				NextButton.Start = TempIndex
				NextButton.Enabled = (TempIndex <> FromIndex)

				' Last Button
				TempIndex = ((RecordCount - 1) \ PageSize) * PageSize + 1
				LastButton.Start = TempIndex
				LastButton.Enabled = (TempIndex <> FromIndex)
			End If
		End Sub
	End Class	

	'
	'  Field class
	'
	Class cField
		Implements IDisposable

		Public TblVar As String ' Table var

		Public FldName As String ' Field name

		Public FldVar As String ' Field variable name

		Public FldExpression As String ' Field expression (used in SQL)

		Public FldType As Integer ' Field type (ADO data type)

		Public FldDbType As OleDbType ' Field type (.NET data type)

		Public FldDataType As Integer ' Field type (ASP.NET Maker data type)

		Public Visible As Boolean ' Visible

		Public FldDateTimeFormat As Integer ' Date time format

		Public CssStyle As String ' CSS style

		Public CssClass As String ' CSS class

		Public ImageAlt As String ' Image alt

		Public ImageWidth As Integer ' Image width

		Public ImageHeight As Integer ' Image height

		Public ViewCustomAttributes As String

		Public EditCustomAttributes As String

		Public CustomMsg As String ' Custom message

		Public RowAttributes As String ' Row attributes

		Public CellCssClass As String ' Cell CSS class

		Public CellCssStyle As String ' Cell CSS style

		Public CellCustomAttributes As String

		Public MultiUpdate As Object ' Multi update

		Public OldValue As Object ' Old Value

		Public ConfirmValue As Object ' Confirm Value

		Public CurrentValue As Object ' Current value

		Public ViewValue As Object ' View value

		Public EditValue As Object ' Edit value

		Public EditValue2 As Object ' Edit value 2 (search)

		Public HrefValue As Object ' Href value

		Public HrefValue2 As Object

		Private m_FormValue As Object ' Form value

		Private m_QueryStringValue As Object ' QueryString value

		Private m_DbValue As Object ' Database Value

		' Create new field object
		Public Sub New(atblvar As String, afldvar As String, afldname As String, afldexpression As String, afldtype As Integer, aflddbtype As OleDbType, aflddatatype As Integer, aflddtformat As Integer)
			TblVar = atblvar
			FldVar = afldvar
			FldName = afldname
			FldExpression = afldexpression
			FldType = afldtype
			FldDbType = aflddbtype
			FldDataType = aflddatatype
			FldDateTimeFormat = aflddtformat
			ImageWidth = 0
			ImageHeight = 0
			Visible = True
		End Sub		

		' View Attributes
		Public ReadOnly Property ViewAttributes() As Object
			Get
				Dim sAtt As String = ""
				If ew_NotEmpty(CssStyle) Then
					sAtt = sAtt & " style=""" & CssStyle.Trim() & """"
				End If
				If ew_NotEmpty(CssClass) Then
					sAtt = sAtt & " class=""" & CssClass.Trim() & """"
				End If
				If ew_NotEmpty(ImageAlt) Then
					sAtt = sAtt & " alt=""" & ImageAlt.Trim() & """"
				End If
				If ImageWidth > 0 Then
					sAtt = sAtt & " width=""" & ImageWidth & """"
				End If
				If ImageHeight > 0 Then
					sAtt = sAtt & " height=""" & ImageHeight & """"
				End If
				If ew_NotEmpty(ViewCustomAttributes) Then
					sAtt = sAtt & " " & ViewCustomAttributes.Trim()
				End If
				Return sAtt
			End Get
		End Property

		' Edit Attributes
		Public ReadOnly Property EditAttributes() As Object
			Get
				Dim sAtt As String = ""
				If ew_NotEmpty(CssStyle) Then
					sAtt = sAtt & " style=""" & CssStyle.Trim() & """"
				End If
				If ew_NotEmpty(CssClass) Then
					sAtt = sAtt & " class=""" & CssClass.Trim() & """"
				End If
				If ew_NotEmpty(EditCustomAttributes) Then
					sAtt = sAtt & " " & EditCustomAttributes.Trim()
				End If
				Return sAtt
			End Get
		End Property

		' Cell Attributes
		Public ReadOnly Property CellAttributes() As Object
			Get
				Dim sAtt As String = ""
				If ew_NotEmpty(CellCssStyle) Then
					sAtt = sAtt & " style=""" & CellCssStyle.Trim() & """"
				End If
				If ew_NotEmpty(CellCssClass) Then
					sAtt = sAtt & " class=""" & CellCssClass.Trim() & """"
				End If
				If ew_NotEmpty(CellCustomAttributes) Then
					sAtt = sAtt & " " & CellCustomAttributes.Trim() ' Cell custom attributes
				End If
				Return sAtt
			End Get
		End Property

		' Sort Attributes		
		Public Property Sort() As Object
			Get
				Return ew_Session(EW_PROJECT_NAME & "_" & TblVar & "_" & EW_TABLE_SORT & "_" & FldVar)
			End Get
			Set(ByVal Value As Object)
				If ew_Session(EW_PROJECT_NAME & "_" & TblVar & "_" & EW_TABLE_SORT & "_" & FldVar) <> Value Then
					ew_Session(EW_PROJECT_NAME & "_" & TblVar & "_" & EW_TABLE_SORT & "_" & FldVar) = Value
				End If
			End Set
		End Property

		' List View value
		Public ReadOnly Property ListViewValue() As String
			Get
				If ew_Empty(ViewValue) Then
					Return "&nbsp;"
				Else
					Dim Result As String = Convert.ToString(ViewValue)
					Dim Result2 As String = Regex.Replace(Result, "<[^>]*>", String.Empty) ' Remove HTML tags
					Return IIf(Result2.Trim().Equals(String.Empty), "&nbsp;", Result)	
				End If
			End Get
		End Property

		' Export Value
		Public Function ExportValue(ByVal Export As Object, ByVal Original As Object) As Object
			Dim ExpVal As Object
			ExpVal = IIf(Original, CurrentValue, ViewValue)				
			If Export = "xml" AndAlso IsDbNull(ExpVal) Then ExpVal = "<Null>"
			Return ExpVal
		End Function		

		Public Property FormValue() As Object
			Get
				Return m_FormValue
			End Get
			Set(ByVal Value As Object)
				m_FormValue = Value
				CurrentValue = m_FormValue
			End Set
		End Property

		Public Property QueryStringValue() As Object
			Get
				Return m_QueryStringValue
			End Get
			Set(ByVal Value As Object)
				m_QueryStringValue = Value
				CurrentValue = m_QueryStringValue
			End Set
		End Property

		Public Property DbValue() As Object
			Get
				Return m_DbValue
			End Get
			Set(ByVal Value As Object)
				m_DbValue = Value
				CurrentValue = m_DbValue
			End Set
		End Property

		' Session Value		
		Public Property SessionValue() As Object
			Get
				Return ew_Session(EW_PROJECT_NAME & "_" & TblVar & "_" & FldVar & "_SessionValue")
			End Get
			Set(ByVal Value As Object)
				ew_Session(EW_PROJECT_NAME & "_" & TblVar & "_" & FldVar & "_SessionValue") = Value
			End Set
		End Property

		Public ReadOnly Property AdvancedSearch() As cAdvancedSearch
			Get
				If m_AdvancedSearch Is Nothing Then m_AdvancedSearch = New cAdvancedSearch
				Return m_AdvancedSearch
			End Get
		End Property

		Public ReadOnly Property Upload() As cUpload
			Get
				If m_Upload Is Nothing Then
					m_Upload = New cUpload(TblVar, FldVar)
				End If
				Return m_Upload
			End Get
		End Property		

		Public Function ReverseSort() As String
			Return IIf(Sort = "ASC", "DESC", "ASC")			
		End Function		

		' Set up database value
		Public Sub SetDbValue(value As Object, def As Object)
			Select Case FldType
				Case 2, 3, 16, 17, 18, 19, 20, 21 ' Int
					If IsNumeric(value) Then
						m_DbValue = ew_Conv(value, FldType)
					Else
						m_DbValue = def
					End If
				Case 5, 6, 14, 131 ' Double
					If IsNumeric(value) Then
						m_DbValue = ew_Conv(value, FldType)
					Else
						m_DbValue = def
					End If
				Case 4 ' Single
					If IsNumeric(value) Then
						m_DbValue = ew_Conv(value, FldType)
					Else
						m_DbValue = def
					End If
				Case 7, 133, 134, 135 ' Date
					If IsDate(value) Then
						m_DbValue = Convert.ToDateTime(value)
					Else
						m_DbValue = def
					End If
				Case 201, 203, 129, 130, 200, 202 ' String
					If EW_REMOVE_XSS Then
						m_DbValue = ew_RemoveXSS(Convert.ToString(value))
					Else
						m_DbValue = Convert.ToString(value)
					End If
					If Convert.ToString(m_DbValue) = "" Then m_DbValue = def
				Case 128, 204, 205 ' Binary
					If IsDbNull(value) Then
						m_DbValue = def
					Else
						m_DbValue = value
					End If
				Case 72 ' GUID
					If ew_NotEmpty(value) AndAlso ew_CheckGUID(value.Trim()) Then
						m_DbValue = value
					Else
						m_DbValue = def
					End If
				Case Else
					m_DbValue = value
			End Select
		End Sub

		Public Count As Integer ' Count

		Public Total As Object ' Total

		' AdvancedSearch Object
		Private m_AdvancedSearch As Object

		' Upload Object
		Private m_Upload As Object

		' Show object as string
		Public Function AsString() As Object
			Dim AdvancedSearchAsString As String, UploadAsString As String
			If m_AdvancedSearch IsNot Nothing Then
				AdvancedSearchAsString = m_AdvancedSearch.AsString
			Else
				AdvancedSearchAsString = "{Null}"
			End If
			If m_Upload IsNot Nothing Then
				UploadAsString = m_Upload.AsString
			Else
				UploadAsString = "{Null}"
			End If
			Return "{" & "FldName: " & FldName & ", " & "FldVar: " & FldVar & ", " & "FldExpression: " & FldExpression & ", " & "FldType: " & FldType & ", " & "FldDateTimeFormat: " & FldDateTimeFormat & ", " & "CssStyle: " & CssStyle & ", " & "CssClass: " & CssClass & ", " & "ImageAlt: " & ImageAlt & ", " & "ImageWidth: " & ImageWidth & ", " & "ImageHeight: " & ImageHeight & ", " & "ViewCustomAttributes: " & ViewCustomAttributes & ", " & "EditCustomAttributes: " & EditCustomAttributes & ", " & "CellCssStyle: " & CellCssStyle & ", " & "CellCssClass: " & CellCssClass & ", " & "Sort: " & Sort & ", " & "MultiUpdate: " & MultiUpdate & ", " & "CurrentValue: " & CurrentValue & ", " & "ViewValue: " & ViewValue & ", " & "EditValue: " & Convert.ToString(EditValue) & ", " & "EditValue2: " & Convert.ToString(EditValue2) & ", " & "HrefValue: " & HrefValue & ", " & "HrefValue2: " & HrefValue2 & ", " & "FormValue: " & m_FormValue & ", " & "QueryStringValue: " & m_QueryStringValue & ", " & "DbValue: " & m_DbValue & ", " & "SessionValue: " & SessionValue & ", " & "Count: " & Count & ", " & "Total: " & Total & ", " & "AdvancedSearch: " & AdvancedSearchAsString & ", " & "Upload: " & UploadAsString & "}"
		End Function

		' Class terminate
		Public Sub Dispose() Implements IDisposable.Dispose
			If m_AdvancedSearch IsNot Nothing Then
				m_AdvancedSearch = Nothing
			End If
			If m_Upload IsNot Nothing Then
				m_Upload = Nothing
			End If
		End Sub
	End Class

	'
	'  List option collection class
	'	
	Class cListOptions

		Public Items As ArrayList

		Public Sub New()
			Items = New ArrayList
		End Sub

		' Add and return a new option
		Public Function Add() As cListOption
			Add = New cListOption
			Items.Add(Add)
		End Function
	End Class

	'
	'  List option class
	'	
	Class cListOption

		Public Visible As Boolean

		Public HeaderCellHtml As String

		Public FooterCellHtml As String

		Public BodyCellHtml As String

		Public MultiColumnLinkHtml As String

		' Constructor
		Public Sub New()
			Visible = True
		End Sub

		' Convert to string
		Public Function AsString() As String
			Return "{" & "Visible: " & Visible & ", " & "HeaderCellHtml: " & ew_HtmlEncode(HeaderCellHtml) & ", " & "FooterCellHtml: " & ew_HtmlEncode(FooterCellHtml) & ", " & "BodyCellHtml: " & ew_HtmlEncode(BodyCellHtml) & ", " & "MultiColumnLinkHtml: " & ew_HtmlEncode(MultiColumnLinkHtml) & "}"
		End Function
	End Class

	'
	' Connection object
	'
	Public Class cConnection
		Implements IDisposable

		Public ConnectionString As String = wpmConfig.ConnStr  ' EW_DB_CONNECTION_STRING

		Public Conn As OleDbConnection

		Public Trans As OleDbTransaction

		Private TempConn As OleDbConnection

		Private TempCommand As OleDbCommand

		Private TempDataReader As OleDbDataReader

		' Constructor
		Public Sub New(ConnStr As String)
			ConnectionString = ConnStr
			Conn = New OleDbConnection(ConnectionString)
			Conn.Open()
		End Sub

		' Constructor
		Public Sub New()
			Conn = New OleDbConnection(ConnectionString)
			Conn.Open()
		End Sub

		' Execute SQL
		Public Function Execute(Sql As String) As Integer
			Dim Cmd As OleDbCommand = GetCommand(Sql)				
			Return Cmd.ExecuteNonQuery()			
		End Function

		' Execute SQL and return first value of first row
		Public Function ExecuteScalar(Sql As String) As Object
			Try
				Dim Cmd As OleDbCommand = GetCommand(Sql)
				Return Cmd.ExecuteScalar()
			Catch
				If EW_DEBUG_ENABLED Then Throw
				Return Nothing 
			End Try				
		End Function

		' Get last insert ID
		Public Function GetLastInsertId() As Object
			Dim Id As Object = System.DBNull.Value 
			Id = ExecuteScalar("SELECT @@Identity")			
			Return Id
		End Function

		' Get data reader
		Public Function GetDataReader(Sql As String) As OleDbDataReader
			Try
				Dim Cmd As OleDbCommand = GetCommand(Sql)
				Return Cmd.ExecuteReader()
			Catch
				If EW_DEBUG_ENABLED Then Throw
				Return Nothing 
			End Try	
		End Function

		' Get temporary data reader
		Public Function GetTempDataReader(Sql As String) As OleDbDataReader
			Try
				If TempConn Is Nothing Then
					TempConn = New OleDbConnection(ConnectionString)
					TempConn.Open()
				End If
				If TempCommand Is Nothing Then
					TempCommand = New OleDbCommand(Sql, TempConn)
				End If
				CloseTempDataReader()
				TempCommand.CommandText = Sql
				TempDataReader = TempCommand.ExecuteReader()			
				Return TempDataReader
			Catch
				If EW_DEBUG_ENABLED Then Throw
				Return Nothing 
			End Try	
		End Function

		' Close temporary data reader
		Public Sub CloseTempDataReader()
			If TempDataReader IsNot Nothing	Then
				TempDataReader.Close()
				TempDataReader.Dispose()
			End If			
		End Sub

		' Get OrderedDictionary from data reader
		Public Function GetRow(ByRef dr As OleDbDataReader) As OrderedDictionary
			Dim od As New OrderedDictionary
			For i As Integer = 0 to dr.FieldCount - 1 
				od(dr.GetName(i)) = dr(i)
			Next
			Return od
		End Function

		' Get rows
		Public Overloads Function GetRows(ByRef dr As OleDbDataReader) As ArrayList
			Dim Rows As New ArrayList() 
			While dr.Read() 
				Rows.Add(GetRow(dr)) 
			End While
			Return Rows 
		End Function

		' Get rows by SQL
		Public Overloads Function GetRows(Sql As String) As ArrayList
			Dim dr As OleDbDataReader = GetTempDataReader(Sql)
			Try
				Return GetRows(dr)
			Finally
				CloseTempDataReader()
			End Try 		
		End Function	

		' Get dataset
		Public Function GetDataSet(Sql As String) As DataSet
			Try
				Dim Adapter As New OleDbDataAdapter(Sql, Conn)
				Dim DS As DataSet = new DataSet()
				Adapter.Fill(DS)
				Return DS
			Catch
				If EW_DEBUG_ENABLED Then Throw
				Return Nothing
			End Try	
		End Function

		' Get command
		Public Function GetCommand(Sql As String) As OleDbCommand
			Dim Cmd As New OleDbCommand(Sql, Conn)
			If Trans IsNot Nothing Then Cmd.Transaction = Trans
			Return Cmd
		End Function

		' Begin transaction
		Public Sub BeginTrans()
			Try
				Trans = Conn.BeginTransaction()
			Catch
				If EW_DEBUG_ENABLED Then Throw 
			End Try
		End Sub

		' Commit transaction
		Public Sub CommitTrans()
			If Trans IsNot Nothing Then Trans.Commit()
		End Sub

		' Rollback transaction
		Public Sub RollbackTrans()
			If Trans IsNot Nothing Then Trans.Rollback()
		End Sub

		' Dispose	
		Public Sub Dispose() Implements IDisposable.Dispose
			If Trans IsNot Nothing Then Trans.Dispose()
			Conn.Close()
			Conn.Dispose()
			If TempCommand IsNot Nothing Then
				TempCommand.Dispose()
			End If
			If TempConn IsNot Nothing Then
				TempConn.Close()
				TempConn.Dispose()
			End If
		End Sub
	End Class	

	'
	'  Advanced Search class
	'	
	Class cAdvancedSearch

		Public SearchValue As Object ' Search value

		Public SearchOperator As String = "=" ' Search operator

		Public SearchCondition As String = "AND" ' Search condition

		Public SearchValue2 As Object ' Search value 2

		Public SearchOperator2 As String = "" ' Search operator 2

		' Show object as string
		Public Function AsString() As Object
			AsString = "{" & "SearchValue: " & SearchValue & ", " & "SearchOperator: " & SearchOperator & ", " & "SearchCondition: " & SearchCondition & ", " & "SearchValue2: " & SearchValue2 & ", " & "SearchOperator2: " & SearchOperator2 & "}"
		End Function
	End Class

	'
	'  Upload class
	'
	Class cUpload

		Public Index As Integer = 0 ' Index to handle multiple form elements

		Public TblVar As String ' Table variable

		Public FldVar As String ' Field variable

		Public DbValue As Object ' Value from database

		Public UploadPath As String = "" ' Upload path		

		Private m_Message As String = "" ' Error message		

		Private m_Value As Object ' Upload value		

		Private m_Action As String = "" ' Upload action		

		Private m_FileName As String = "" ' Upload file name		

		Private m_FileSize As Long = -1 ' Upload file size		

		Private m_ContentType As String = "" ' File content type		

		Private m_ImageWidth As Integer = -1 ' Image width		

		Private m_ImageHeight As Integer = -1 ' Image height

		' Reference to page form object
		Private m_FormObj As cFormObj		

		' Contructor
		Public Sub New(ATblVar As String, AFldVar As String)
			TblVar = ATblVar
			FldVar = AFldVar
		End Sub

		' Form object
		Public ReadOnly Property ObjForm() As cFormObj
			Get				
				Return m_FormObj
			End Get			
		End Property

		Public ReadOnly Property Message() As Object
			Get
				Return m_Message
			End Get
		End Property		

		Public Property Value() As Object
			Get
				Return m_Value
			End Get
			Set(ByVal Value As Object)
				m_Value = Value
			End Set
		End Property

		Public ReadOnly Property Action() As String
			Get
				Return m_Action
			End Get
		End Property

		Public ReadOnly Property FileName() As String
			Get
				Return m_FileName
			End Get
		End Property

		Public ReadOnly Property FileSize() As Long
			Get
				Return m_FileSize
			End Get
		End Property

		Public ReadOnly Property ContentType() As String
			Get
				Return m_ContentType
			End Get
		End Property

		Public ReadOnly Property ImageWidth() As Integer
			Get
				Return m_ImageWidth
			End Get
		End Property

		Public ReadOnly Property ImageHeight() As Integer
			Get
				Return m_ImageHeight
			End Get
		End Property

		' Set form object
		Public Sub SetForm(ByRef Obj As cFormObj)			
			m_FormObj = Obj
			Index = Obj.Index	
		End Sub		

		' Save Db value to Session
		Public Sub SaveDbToSession()			
			Dim sSessionID As String = EW_PROJECT_NAME & "_" & TblVar & "_" & FldVar & "_" & Index
			ew_Session(sSessionID & "_DbValue") = DbValue
		End Sub

		' Restore Db value from Session
		Public Sub RestoreDbFromSession()			
			Dim sSessionID As String = EW_PROJECT_NAME & "_" & TblVar & "_" & FldVar & "_" & Index
			DbValue = ew_Session(sSessionID & "_DbValue")
		End Sub

		' Remove Db value from Session
		Public Sub RemoveDbFromSession()			
			Dim sSessionID As String = EW_PROJECT_NAME & "_" & TblVar & "_" & FldVar & "_" & Index
			HttpContext.Current.Session.Contents.Remove((sSessionID & "_DbValue"))
		End Sub

		' Save Upload values to Session
		Public Sub SaveToSession()			
			Dim sSessionID As String = EW_PROJECT_NAME & "_" & TblVar & "_" & FldVar & "_" & Index
			ew_Session(sSessionID & "_Action") = m_Action
			ew_Session(sSessionID & "_FileSize") = m_FileSize
			ew_Session(sSessionID & "_FileName") = m_FileName
			ew_Session(sSessionID & "_ContentType") = m_ContentType
			ew_Session(sSessionID & "_ImageWidth") = m_ImageWidth
			ew_Session(sSessionID & "_ImageHeight") = m_ImageHeight
			ew_Session(sSessionID & "_Value") = m_Value
		End Sub

		' Restore Upload values from Session
		Public Sub RestoreFromSession()			
			Dim sSessionID As String = EW_PROJECT_NAME & "_" & TblVar & "_" & FldVar & "_" & Index
			m_Action = ew_Session(sSessionID & "_Action")
			m_FileSize = ew_Session(sSessionID & "_FileSize")
			m_FileName = ew_Session(sSessionID & "_FileName")
			m_ContentType = ew_Session(sSessionID & "_ContentType")
			m_ImageWidth = ew_Session(sSessionID & "_ImageWidth")
			m_ImageHeight = ew_Session(sSessionID & "_ImageHeight")
			m_Value = ew_Session(sSessionID & "_Value")
		End Sub

		' Remove Upload values from Session
		Public Sub RemoveFromSession()
			Dim sSessionID As String = EW_PROJECT_NAME & "_" & TblVar & "_" & FldVar & "_" & Index
			HttpContext.Current.Session.Contents.Remove((sSessionID & "_Action"))
			HttpContext.Current.Session.Contents.Remove((sSessionID & "_FileSize"))
			HttpContext.Current.Session.Contents.Remove((sSessionID & "_FileName"))
			HttpContext.Current.Session.Contents.Remove((sSessionID & "_ContentType"))
			HttpContext.Current.Session.Contents.Remove((sSessionID & "_ImageWidth"))
			HttpContext.Current.Session.Contents.Remove((sSessionID & "_ImageHeight"))
			HttpContext.Current.Session.Contents.Remove((sSessionID & "_Value"))
		End Sub

		' Check the file type of the uploaded file
		Private Function UploadAllowedFileExt(FileName As String) As Boolean
			Return ew_CheckFileType(FileName)
		End Function

		' Get upload file
		Public Function UploadFile() As Boolean
			Try
				Dim gsFldVar As String = FldVar
				Dim gsFldVarAction As String = "a" & gsFldVar.Substring(1)
				Dim gsFldVarWidth As String = "wd" & gsFldVar.Substring(1)
				Dim gsFldVarHeight As String = "ht" & gsFldVar.Substring(1)

				' Initialize upload value
				m_Value = System.DBNull.Value

				' Get action
				m_Action = ObjForm.GetValue(gsFldVarAction)

				' Get and check the upload file size
				m_FileSize = ObjForm.GetUploadFileSize(gsFldVar)

				' Get and check the upload file type
				m_FileName = ObjForm.GetUploadFileName(gsFldVar)

				' Get upload file content type
				m_ContentType = ObjForm.GetUploadFileContentType(gsFldVar)

				' Get upload value
				m_Value = ObjForm.GetUploadFileData(gsFldVar)

				' Get image width and height
				m_ImageWidth = ObjForm.GetUploadImageWidth(gsFldVar)
				m_ImageHeight = ObjForm.GetUploadImageHeight(gsFldVar)
				If m_ImageWidth < 0 OrElse m_ImageHeight < 0 Then
					If IsNumeric(ObjForm.GetValue(gsFldVarWidth)) Then m_ImageWidth = ObjForm.GetValue(gsFldVarWidth)
					If IsNumeric(ObjForm.GetValue(gsFldVarHeight)) Then m_ImageHeight = ObjForm.GetValue(gsFldVarHeight)
				End If
				Return True
			Catch
				Return False
			End Try
		End Function

		' Resize image
		Public Function Resize(Width As Integer, Height As Integer, Interpolation As Integer) As Boolean
			Dim wrkWidth As Integer, wrkHeight As Integer
			If Not IsDbNull(m_Value) Then
				wrkWidth = Width
				wrkHeight = Height
				Resize = ew_ResizeBinary(m_Value, wrkWidth, wrkHeight, Interpolation)
				If Resize Then
					m_ImageWidth = wrkWidth
					m_ImageHeight = wrkHeight
					m_FileSize = m_Value.Length
				End If
			End If
		End Function

		' Save uploaded data to file (Path relative to application root)
		Public Function SaveToFile(Path As String, NewFileName As String, Overwrite As Boolean) As Boolean
			If Not IsDbNull(m_Value) Then
				Path = ew_UploadPathEx(True, Path)
				If ew_Empty(NewFileName) Then NewFileName = m_FileName
				If Overwrite Then
					Return ew_SaveFile(Path, NewFileName, m_Value)
				Else
					Return ew_SaveFile(Path, ew_UploadFileNameEx(Path, NewFileName), m_Value)
				End If
			End If
			Return False
		End Function

		' Resize and save uploaded data to file (Path relative to application root)
		Public Function ResizeAndSaveToFile(Width As Integer, Height As Integer, Interpolation As Integer, Path As String, NewFileName As String, Overwrite As Boolean) As Boolean						
			If Not IsDbNull(m_Value) Then
				Dim OldValue As Object = m_Value ' Save old values
				Dim OldWidth As Integer = m_ImageWidth
				Dim OldHeight As Integer = m_ImageHeight
				Dim OldFileSize As Long = m_FileSize
				Try
					Resize(Width, Height, Interpolation)
					Return SaveToFile(Path, NewFileName, OverWrite)
				Finally
					m_Value = OldValue  ' Restore old values
					m_ImageWidth = OldWidth
					m_ImageHeight = OldHeight
					m_FileSize = OldFileSize
				End Try
			End If
			Return False
		End Function		

		' Show object as string
		Public Function AsString() As String
			AsString = "{" & "Index: " & Index & ", " & "Message: " & m_Message & ", " & "Action: " & m_Action & ", " & "UploadPath: " & UploadPath & ", " & "FileName: " & m_FileName & ", " & "FileSize: " & m_FileSize & ", " & "ContentType: " & m_ContentType & ", " & "ImageWidth: " & m_ImageWidth & ", " & "ImageHeight: " & m_ImageHeight & "}"
		End Function
	End Class

	'
	' Advanced Security class
	'
	Class cAdvancedSecurity
		Inherits AspNetMakerBase

'		Private m_Page As AspNetMaker7_WPMGen
		Private m_ArUserLevel As ArrayList

		Private m_ArUserLevelPriv As ArrayList

		Private m_ArUserLevelID() As Integer

		' Current User Level ID / User Level
		Public CurrentUserLevelID As Integer

		Public CurrentUserLevel As Integer

		' Current User ID / Parent User ID / User ID array
		Public CurrentUserID As Object

		Public CurrentParentUserID As Object

		Private m_ArUserID() As Object

		' Init
		Public Sub New(ByRef APage As AspNetMakerPage)
			m_Page = APage
			m_ParentPage = APage.ParentPage
			m_ArUserLevel = New ArrayList()
			m_ArUserLevelPriv = New ArrayList()

			' Init User Level
			CurrentUserLevelID = SessionUserLevelID
			If IsNumeric(CurrentUserLevelID) Then
				If CurrentUserLevelID >= -1 Then					
					Array.Resize(m_ArUserLevelID, 1)
					m_ArUserLevelID(0) = CurrentUserLevelID
				End If
			End If

			' Init User ID
			CurrentUserID = SessionUserID
			CurrentParentUserID = SessionParentUserID

			' Load user level (for TablePermission_Loading event)
			LoadUserLevel()
		End Sub

		' Session User ID		
		Public Property SessionUserID() As Object
			Get
				Return Convert.ToString(ew_Session(EW_SESSION_USER_ID))
			End Get
			Set(ByVal Value As Object)
				ew_Session(EW_SESSION_USER_ID) = Value
				CurrentUserID = Value
			End Set
		End Property

		' Session parent User ID		
		Public Property SessionParentUserID() As Object
			Get
				Return Convert.ToString(ew_Session(EW_SESSION_PARENT_USER_ID))
			End Get
			Set(ByVal Value As Object)
				ew_Session(EW_SESSION_PARENT_USER_ID) = Value
				CurrentParentUserID = Value
			End Set
		End Property

		' Current user name		
		Public Property CurrentUserName() As Object
			Get
				Return Convert.ToString(ew_Session(EW_SESSION_USER_NAME))
			End Get
			Set(ByVal Value As Object)
				ew_Session(EW_SESSION_USER_NAME) = Value
			End Set
		End Property

		' Session User Level ID		
		Public Property SessionUserLevelID() As Object
			Get
				Return ew_Session(EW_SESSION_USER_LEVEL_ID)
			End Get
			Set(ByVal Value As Object)
				ew_Session(EW_SESSION_USER_LEVEL_ID) = Value
				CurrentUserLevelID = Value
			End Set
		End Property

		' Session User Level value	
		Public Property SessionUserLevel() As Object
			Get
				Return ew_Session(EW_SESSION_USER_LEVEL)
			End Get
			Set(ByVal Value As Object)
				ew_Session(EW_SESSION_USER_LEVEL) = Value
				CurrentUserLevel = Value
			End Set
		End Property

		' Can add		
		Public Property CanAdd() As Boolean
			Get
				Return ((CurrentUserLevel And EW_ALLOW_ADD) = EW_ALLOW_ADD)
			End Get
			Set(ByVal Value As Boolean)
				If (Value) Then
					CurrentUserLevel = (CurrentUserLevel Or EW_ALLOW_ADD)
				Else
					CurrentUserLevel = (CurrentUserLevel And (Not EW_ALLOW_ADD))
				End If
			End Set
		End Property

		' Can delete		
		Public Property CanDelete() As Boolean
			Get
				Return ((CurrentUserLevel And EW_ALLOW_DELETE) = EW_ALLOW_DELETE)
			End Get
			Set(ByVal Value As Boolean)
				If (Value) Then
					CurrentUserLevel = (CurrentUserLevel Or EW_ALLOW_DELETE)
				Else
					CurrentUserLevel = (CurrentUserLevel And (Not EW_ALLOW_DELETE))
				End If
			End Set
		End Property

		' Can edit		
		Public Property CanEdit() As Boolean
			Get
				Return ((CurrentUserLevel And EW_ALLOW_EDIT) = EW_ALLOW_EDIT)
			End Get
			Set(ByVal Value As Boolean)
				If (Value) Then
					CurrentUserLevel = (CurrentUserLevel Or EW_ALLOW_EDIT)
				Else
					CurrentUserLevel = (CurrentUserLevel And (Not EW_ALLOW_EDIT))
				End If
			End Set
		End Property

		' Can view		
		Public Property CanView() As Boolean
			Get
				Return ((CurrentUserLevel And EW_ALLOW_VIEW) = EW_ALLOW_VIEW)
			End Get
			Set(ByVal Value As Boolean)
				If (Value) Then
					CurrentUserLevel = (CurrentUserLevel Or EW_ALLOW_VIEW)
				Else
					CurrentUserLevel = (CurrentUserLevel And (Not EW_ALLOW_VIEW))
				End If
			End Set
		End Property

		' Can list		
		Public Property CanList() As Boolean
			Get
				Return ((CurrentUserLevel And EW_ALLOW_LIST) = EW_ALLOW_LIST)
			End Get
			Set(ByVal Value As Boolean)
				If (Value) Then
					CurrentUserLevel = (CurrentUserLevel Or EW_ALLOW_LIST)
				Else
					CurrentUserLevel = (CurrentUserLevel And (Not EW_ALLOW_LIST))
				End If
			End Set
		End Property

		' Can report		
		Public Property CanReport() As Boolean
			Get
				Return ((CurrentUserLevel And EW_ALLOW_REPORT) = EW_ALLOW_REPORT)
			End Get
			Set(ByVal Value As Boolean)
				If (Value) Then
					CurrentUserLevel = (CurrentUserLevel Or EW_ALLOW_REPORT)
				Else
					CurrentUserLevel = (CurrentUserLevel And (Not EW_ALLOW_REPORT))
				End If
			End Set
		End Property

		' Can search		
		Public Property CanSearch() As Boolean
			Get
				Return ((CurrentUserLevel And EW_ALLOW_SEARCH) = EW_ALLOW_SEARCH)
			End Get
			Set(ByVal Value As Boolean)
				If (Value) Then
					CurrentUserLevel = (CurrentUserLevel Or EW_ALLOW_SEARCH)
				Else
					CurrentUserLevel = (CurrentUserLevel And (Not EW_ALLOW_SEARCH))
				End If
			End Set
		End Property

		' Can admin		
		Public Property CanAdmin() As Boolean
			Get
				Return ((CurrentUserLevel And EW_ALLOW_ADMIN) = EW_ALLOW_ADMIN)
			End Get
			Set(ByVal Value As Boolean)
				If (Value) Then
					CurrentUserLevel = (CurrentUserLevel Or EW_ALLOW_ADMIN)
				Else
					CurrentUserLevel = (CurrentUserLevel And (Not EW_ALLOW_ADMIN))
				End If
			End Set
		End Property

		' Last URL
		Public ReadOnly Property LastUrl() As String
			Get
				Return ew_Cookie("lasturl")
			End Get
		End Property

		' Save last URL
		Public Sub SaveLastUrl()
			Dim s As String = HttpContext.Current.Request.ServerVariables("SCRIPT_NAME")
			Dim q As String = HttpContext.Current.Request.ServerVariables("QUERY_STRING")			
			If q <> "" Then s = s & "?" & q
			If LastUrl = s Then s = ""
			ew_Cookie("lasturl") = s
		End Sub

		' Auto login
		Public Function AutoLogin() As Boolean
			If ew_SameStr(ew_Cookie("autologin"), "autologin") Then
				Dim sUsr As String = ew_Cookie("username")
				Dim sPwd As String = ew_Cookie("password")
				sPwd = cTEA.Decrypt(sPwd, EW_RANDOM_KEY)
				Dim bValid As Boolean = ValidateUser(sUsr, sPwd)	
				If bValid Then ew_WriteAuditTrailOnLogInOut("autologin", sUsr)
				Return bValid
			Else
				Return False
			End If
		End Function		

		' Validate user
		Public Function ValidateUser(usr As String, pwd As String) As Boolean
			Dim sFilter As String, sSql As String, RsUser As OleDbDataReader
			ValidateUser = False
		End Function

		' No user level security
		Public Sub SetUpUserLevel()
		End Sub

		' Add user permission
		Public Sub AddUserPermission(UserLevelName As String, TableName As String, UserPermission As Integer)
			Dim UserLevelID As String = ""

			' Get user level id from user name
			If ew_IsArrayList(m_ArUserLevel) Then
				For Each Row As OrderedDictionary In m_ArUserLevel
					If ew_SameStr(UserLevelName, Row(1)) Then
						UserLevelID = Row(0)
						Exit For
					End If
				Next 
			End If
			If ew_IsArrayList(m_ArUserLevelPriv) AndAlso UserLevelID <> "" Then
				For Each Row As OrderedDictionary In m_ArUserLevelPriv
					If ew_SameStr(Row(0), TableName) AndAlso ew_SameStr(Row(1), UserLevelID) Then
						Row(2) = Row(2) Or UserPermission ' Add permission
						Exit For
					End If
				Next 
			End If
		End Sub

		' Delete user permission
		Public Sub DeleteUserPermission(UserLevelName As String, TableName As String, UserPermission As Integer)
			Dim UserLevelID As String = ""

			' Get user level id from user name
			If ew_IsArrayList(m_ArUserLevel) Then
				For Each Row As OrderedDictionary In m_ArUserLevel
					If ew_SameStr(UserLevelName, Row(1)) Then
						UserLevelID = Row(0)
						Exit For
					End If
				Next 
			End If
			If ew_IsArrayList(m_ArUserLevelPriv) AndAlso UserLevelID <> "" Then
				For Each Row As OrderedDictionary In m_ArUserLevelPriv
					If ew_SameStr(Row(0), TableName) AndAlso ew_SameStr(Row(1), UserLevelID) Then
						Row(2) = Row(2) And (127 - UserPermission) ' Remove permission
						Exit For
					End If
				Next 
			End If
		End Sub

		' Load current user level
		Public Sub LoadCurrentUserLevel(Table As String)
			LoadUserLevel()
			SessionUserLevel = CurrentUserLevelPriv(Table)
		End Sub

		' Get current user privilege
		Private Function CurrentUserLevelPriv(TableName As String) As Integer
			If IsLoggedIn() Then
				CurrentUserLevelPriv = 0
				For i As Integer = 0 To m_ArUserLevelID.GetUpperBound(0)
					CurrentUserLevelPriv = CurrentUserLevelPriv Or GetUserLevelPrivEx(TableName, m_ArUserLevelID(i))
				Next 
			Else
				Return 0
			End If
		End Function

		' Get user level ID by user level name
		Public Function GetUserLevelID(UserLevelName As String) As Integer
			If ew_SameStr(UserLevelName, "Administrator") Then
				Return -1
			ElseIf UserLevelName <> "" Then 
				If ew_IsArrayList(m_ArUserLevel) Then
					For Each Row As OrderedDictionary In m_ArUserLevel
						If ew_SameStr(Row(1), UserLevelName) Then
							Return Row(0)
						End If
					Next 
				End If
			End If
			Return -2 ' Unknown
		End Function

		' Add user level (for use with UserLevel_Loading event)
		Public Sub AddUserLevel(UserLevelName As String)
			Dim bFound As Boolean = False
			If ew_Empty(UserLevelName) Then Exit Sub
			Dim UserLevelID As Integer = GetUserLevelID(UserLevelName)
			If Not IsNumeric(UserLevelID) Then Exit Sub
			If UserLevelID < -1 Then Exit Sub
			If Not IsArray(m_ArUserLevelID) Then
				Array.Resize(m_ArUserLevelID, 1)
			Else
				For i As Integer = 0 To m_ArUserLevelID.GetUpperBound(0)
					If m_ArUserLevelID(i) = UserLevelID Then
						bFound = True
						Exit For
					End If
				Next 
				If Not bFound Then
					Array.Resize(m_ArUserLevelID, m_ArUserLevelID.Length + 1)
				End If
			End If
			If Not bFound Then
				m_ArUserLevelID(m_ArUserLevelID.GetUpperBound(0)) = UserLevelID
			End If
		End Sub

		' Delete user level (for use with UserLevel_Loading event)
		Public Sub DeleteUserLevel(UserLevelName As String)			
			If UserLevelName = "" OrElse IsDbNull(UserLevelName) Then Exit Sub
			Dim UserLevelID As Integer = GetUserLevelID(UserLevelName)
			If Not IsNumeric(UserLevelID) Then Exit Sub
			If UserLevelID < -1 Then Exit Sub
			If IsArray(m_ArUserLevelID) Then
				For i As Integer = 0 To m_ArUserLevelID.GetUpperBound(0)
					If m_ArUserLevelID(i) = UserLevelID Then
						For j As Integer = i + 1 To m_ArUserLevelID.GetUpperBound(0)
							m_ArUserLevelID(j - 1) = m_ArUserLevelID(j)
						Next 
						If m_ArUserLevelID.Length > 0 Then

							'm_ArUserLevelID = ""
						'Else

							Array.Resize(m_ArUserLevelID, m_ArUserLevelID.Length - 1)
						End If
						Exit Sub
					End If
				Next 
			End If
		End Sub

		' User level list
		Public Function UserLevelList() As String
			Dim List As String = ""
			If IsArray(m_ArUserLevelID) Then
				For i As Integer = 0 To m_ArUserLevelID.GetUpperBound(0)
					If List <> "" Then List = List & ", "
					List = List & m_ArUserLevelID(i)
				Next 
			End If
			Return List
		End Function

		' User level name list
		Public Function UserLevelNameList() As String
			Dim List As String = ""
			If IsArray(m_ArUserLevelID) Then
				For i As Integer = 0 To m_ArUserLevelID.GetUpperBound(0)
					If List <> "" Then List = List & ", "
					List = List & ew_QuotedValue(GetUserLevelName(m_ArUserLevelID(i)), EW_DATATYPE_STRING)
				Next 
			End If
			Return List
		End Function

		' Get user privilege based on table name and user level
		Public Function GetUserLevelPrivEx(TableName As String, UserLevelID As Integer) As Integer
			If ew_SameStr(UserLevelID, "-1") Then ' System Administrator
				If EW_USER_LEVEL_COMPAT Then
					Return 31 ' Use old user level values
				Else
					Return 127 ' Use new user level values (separate View/Search)
				End If
			ElseIf UserLevelID >= 0 Then 
				If ew_IsArrayList(m_ArUserLevelPriv) Then
					For Each Row As OrderedDictionary In m_ArUserLevelPriv
						If ew_SameStr(Row(0), TableName) AndAlso ew_SameStr(Row(1), UserLevelID) Then
							Return ew_ConvertToInt(Row(2))
						End If
					Next
				End If
				Return 0
			End If			
		End Function

		' Get current user level name
		Public Function CurrentUserLevelName() As String
			Return GetUserLevelName(CurrentUserLevelID)
		End Function

		' Get user level name based on user level
		Public Function GetUserLevelName(UserLevelID As Object) As String
			If ew_SameStr(UserLevelID, "-1") Then
				Return "Administrator"
			ElseIf UserLevelID >= 0 Then 
				If ew_IsArrayList(m_ArUserLevel) Then
					For Each Row As OrderedDictionary In m_ArUserLevel
						If ew_SameStr(Row(0), UserLevelID) Then
							Return Row(1)
						End If
					Next 
				End If
			End If
			Return ""
		End Function

		' Display all the User Level settings (for debug only)
		Public Sub ShowUserLevelInfo()
			If ew_IsArrayList(m_ArUserLevel) Then
				ew_Write("User Levels:<br>")
				ew_Write("UserLevelId, UserLevelName<br>")
				For Each Row As OrderedDictionary In m_ArUserLevel
					ew_Write("&nbsp;&nbsp;" & Row(0) & ", " & Row(1) & "<br>")
				Next 
			Else
				ew_Write("No User Level definitions." & "<br>")
			End If
			If ew_IsArrayList(m_ArUserLevelPriv) Then
				ew_Write("User Level Privs:<br>")
				ew_Write("TableName, UserLevelId, UserLevelPriv<br>")
				For Each Row As OrderedDictionary In m_ArUserLevelPriv
					ew_Write("&nbsp;&nbsp;" & Row(0) & ", " & Row(1) & ", " & Row(2) & "<br>")
				Next 
			Else
				ew_Write("No User Level privilege settings." & "<br>")
			End If
			ew_Write("CurrentUserLevel = " & CurrentUserLevel & "<br>")
			ew_Write("User Levels = " & UserLevelList() & "<br>")
		End Sub

		' Check privilege for List page (for menu items)
		Public Function AllowList(TableName As String) As Boolean
			Return ew_ConvertToBool(CurrentUserLevelPriv(TableName) And EW_ALLOW_LIST)
		End Function

		' Check privilege for Add
		Public Function AllowAdd(TableName As String) As Boolean
			Return ew_ConvertToBool(CurrentUserLevelPriv(TableName) And EW_ALLOW_ADD)
		End Function

		' Check if user is logged in
		Public Function IsLoggedIn() As Boolean
			Return ew_SameStr(ew_Session(EW_SESSION_STATUS), "login")
		End Function

		' Check if user is system administrator
		Public Function IsSysAdmin() As Boolean
			Return (ew_Session(EW_SESSION_SYS_ADMIN) = 1)
		End Function

		' Check if user is administrator
		Public Function IsAdmin() As Boolean
			Dim Result As Boolean = IsSysAdmin()
			Return Result
		End Function

		' Save user level to session
		Public Sub SaveUserLevel()
			ew_Session(EW_SESSION_AR_USER_LEVEL) = m_ArUserLevel
			ew_Session(EW_SESSION_AR_USER_LEVEL_PRIV) = m_ArUserLevelPriv			
		End Sub

		' Load user level from session
		Public Sub LoadUserLevel()
			If Not ew_IsArrayList(ew_Session(EW_SESSION_AR_USER_LEVEL)) Then
				SetUpUserLevel()
				SaveUserLevel()
			Else
				m_ArUserLevel = ew_Session(EW_SESSION_AR_USER_LEVEL)
				m_ArUserLevelPriv = ew_Session(EW_SESSION_AR_USER_LEVEL_PRIV)
			End If
		End Sub

		' UserID Loading event
		Public Sub UserID_Loading()

			'HttpContext.Current.Response.Write("UserID Loading: " & CurrentUserID & "<br>")
		End Sub

		' UserID Loaded event
		Public Sub UserID_Loaded()

			'HttpContext.Current.Response.Write("UserID Loaded: " & UserIDList & "<br>")
		End Sub

		' User Level Loaded event
		Public Sub UserLevel_Loaded()

			'AddUserPermission(<UserLevelName>, <TableName>, <UserPermission>)
			'DeleteUserPermission(<UserLevelName>, <TableName>, <UserPermission>)

		End Sub

		' Table Permission Loading event
		Public Sub TablePermission_Loading()

			'HttpContext.Current.Response.Write("Table Permission Loading: " & CurrentUserLevelID & "<br>")
		End Sub

		' Table Permission Loaded event
		Public Sub TablePermission_Loaded()

			'HttpContext.Current.Response.Write("Table Permission Loaded: " & CurrentUserLevel & "<br>")
		End Sub

		' User Validated event
		Public Sub User_Validated(rs As DbDataReader)

			'HttpContext.Current.Session("UserEmail") = rs("Email")
		End Sub
	End Class

	' Return multi-value search SQL
	Public Shared Function ew_GetMultiSearchSql(ByRef Fld As Object, FldVal As String) As String
		Dim sSql As String, sVal As String, sWrk As String = ""
		Dim arVal() As String = FldVal.Split(New Char() {","c})
		For i As Integer = 0 To arVal.GetUpperBound(0)
			sVal = arVal(i).Trim()
			If arVal.GetUpperBound(0) = 0 OrElse EW_SEARCH_MULTI_VALUE_OPTION = 3 Then
				sSql = Fld.FldExpression & " = '" & ew_AdjustSql(sVal) & "' OR " & ew_GetMultiSearchSqlPart(Fld, sVal)
			Else
				sSql = ew_GetMultiSearchSqlPart(Fld, sVal)
			End If
			If sWrk <> "" Then
				If EW_SEARCH_MULTI_VALUE_OPTION = 2 Then
					sWrk = sWrk & " AND "
				ElseIf EW_SEARCH_MULTI_VALUE_OPTION = 3 Then 
					sWrk = sWrk & " OR "
				End If
			End If
			sWrk = sWrk & "(" & sSql & ")"
		Next 
		Return sWrk
	End Function

	' Get multi search SQL part
	Public Shared Function ew_GetMultiSearchSqlPart(ByRef Fld As Object, FldVal As String) As String
		Return Fld.FldExpression & " LIKE '" & ew_AdjustSql(FldVal) & ",%' OR " & Fld.FldExpression & " LIKE '%," & FldVal & ",%' OR " & Fld.FldExpression & " LIKE '%," & FldVal & "'"
	End Function

	' Get search sql
	Public Shared Function ew_GetSearchSql(ByRef Fld As Object, FldVal As String, FldOpr As String, FldCond As String, FldVal2 As String, FldOpr2 As String) As String
		Dim IsValidValue As Boolean
		Dim sSql As String = ""
		If FldOpr = "BETWEEN" Then
			IsValidValue = (Fld.FldDataType <> EW_DATATYPE_NUMBER) OrElse (Fld.FldDataType = EW_DATATYPE_NUMBER AndAlso IsNumeric(FldVal) AndAlso IsNumeric(FldVal2))
			If FldVal <> "" AndAlso FldVal2 <> "" AndAlso IsValidValue Then
				sSql = Fld.FldExpression & " BETWEEN " & ew_QuotedValue(FldVal, Fld.FldDataType) & " AND " & ew_QuotedValue(FldVal2, Fld.FldDataType)
			End If
		ElseIf FldOpr = "IS NULL" OrElse FldOpr = "IS NOT NULL" Then 
			sSql = Fld.FldExpression & " " & FldOpr
		Else
			IsValidValue = (Fld.FldDataType <> EW_DATATYPE_NUMBER) OrElse (Fld.FldDataType = EW_DATATYPE_NUMBER AndAlso IsNumeric(FldVal))
			If FldVal <> "" AndAlso IsValidValue AndAlso ew_IsValidOpr(FldOpr, Fld.FldDataType) Then
				sSql = Fld.FldExpression & ew_SearchString(FldOpr, FldVal, Fld.FldDataType)
			End If
			IsValidValue = (Fld.FldDataType <> EW_DATATYPE_NUMBER) OrElse (Fld.FldDataType = EW_DATATYPE_NUMBER AndAlso IsNumeric(FldVal2))
			If FldVal2 <> "" AndAlso IsValidValue AndAlso ew_IsValidOpr(FldOpr2, Fld.FldDataType) Then
				If sSql <> "" Then
					sSql = sSql & " " & IIf(FldCond = "OR", "OR", "AND") & " "
				End If
				sSql = "(" & sSql & Fld.FldExpression & ew_SearchString(FldOpr2, FldVal2, Fld.FldDataType) & ")"
			End If
		End If
		Return sSql
	End Function

	' Return search string
	Public Shared Function ew_SearchString(FldOpr As String, FldVal As String, FldType As Integer) As String
		If FldOpr = "LIKE" OrElse FldOpr = "NOT LIKE" Then
			Return " " & FldOpr & " " & ew_QuotedValue("%" & FldVal & "%", FldType)
		ElseIf FldOpr = "STARTS WITH" Then 
			Return " LIKE " & ew_QuotedValue(FldVal & "%", FldType)
		Else
			Return " " & FldOpr & " " & ew_QuotedValue(FldVal, FldType)
		End If
	End Function

	' Check if valid operator
	Public Shared Function ew_IsValidOpr(Opr As String, FldType As Integer) As Boolean
		Dim Valid As Boolean = (Opr = "=" OrElse Opr = "<" OrElse Opr = "<=" OrElse Opr = ">" OrElse Opr = ">=" OrElse Opr = "<>")
		If FldType = EW_DATATYPE_STRING OrElse FldType = EW_DATATYPE_MEMO Then
			Valid = Valid OrElse Opr = "LIKE" OrElse Opr = "NOT LIKE" OrElse Opr = "STARTS WITH"
		End If
		Return Valid
	End Function	

	' Quoted value for field type
	Public Shared Function ew_QuotedValue(Value As Object, FldType As Integer) As String
		Value = Convert.ToString(Value)
		Select Case FldType
			Case EW_DATATYPE_STRING, EW_DATATYPE_MEMO
				Return "'" & ew_AdjustSql(Value) & "'"
			Case EW_DATATYPE_GUID
				If EW_IS_MSACCESS Then
					If Value.StartsWith("{") Then
						Return Value
					Else
						Return "{" & ew_AdjustSql(Value) & "}"
					End If
				Else
					Return "'" & ew_AdjustSql(Value) & "'"
				End If
			Case EW_DATATYPE_DATE, EW_DATATYPE_TIME
				If EW_IS_MSACCESS Then
					Return "#" & ew_AdjustSql(Value) & "#"
				Else
					Return "'" & ew_AdjustSql(Value) & "'"
				End If
			Case Else
				Return Value
		End Select
	End Function	

	' Pad zeros before number
	Public Shared Function ew_ZeroPad(m As Object, t As Integer) As String
		Return Convert.ToString(m).PadLeft(t, "0"c)
	End Function	

	' Convert numeric value
	Public Shared Function ew_Conv(v As Object, t As Integer) As Object	
		If IsDbNull(v) Then Return System.DBNull.Value
		Select Case t			
			Case 20 ' adBigInt
				Return Convert.ToInt64(v)
			Case 21 ' adUnsignedBigInt
				Return Convert.ToUInt64(v)				
			Case 2, 16 ' adSmallInt/adTinyInt
				Return Convert.ToInt16(v)
			Case 3 ' adInteger
				Return Convert.ToInt32(v)
			Case 17, 18 ' adUnsignedTinyInt/adUnsignedSmallInt
				Return Convert.ToUInt16(v)
			Case 19 ' adUnsignedInt
				Return Convert.ToUInt32(v)
			Case 4 ' adSingle
				Return Convert.ToSingle(v)				
			Case 5, 6, 131 ' adDouble/adCurrency/adNumeric
				Return Convert.ToDouble(v)
			Case Else
				Return v
		End Select
	End Function	

	' Public Shared Function for debug
	Public Shared Sub ew_Trace(Msg As Object)
		Try
			Dim FileName as String = HttpContext.Current.Server.MapPath("debug.txt")   
	    Dim sw as StreamWriter = File.AppendText(FileName)   
	    sw.WriteLine(Convert.ToString(Msg))   
	    sw.Close()
		Catch
			If EW_DEBUG_ENABLED Then Throw		
		End Try       
	End Sub	

	' Compare values with special handling for null values
	Public Shared Function ew_CompareValue(v1 As Object, v2 As Object) As Boolean
		If IsDbNull(v1) AndAlso IsDbNull(v2) Then
			Return True
		ElseIf IsDbNull(v1) OrElse IsDbNull(v2) Then 
			Return False
		Else
			Return ew_SameStr(v1, v2)
		End If
	End Function	

	' Adjust SQL for special characters
	Public Shared Function ew_AdjustSql(value As Object) As String
		Dim sWrk As String = Convert.ToString(value).Trim()
		sWrk = sWrk.Replace("'", "''") ' Adjust for Single Quote
		sWrk = sWrk.Replace("[", "[[]") ' Adjust for Open Square Bracket
		Return sWrk
	End Function	

	' Build select SQL based on different SQL part
	Public Shared Function ew_BuildSelectSql(sSelect As String, sWhere As String, sGroupBy As String, sHaving As String, sOrderBy As String, sFilter As String, sSort As String) As String
		Dim sSql As String, sDbOrderBy As String
		Dim sDbWhere As String = sWhere
		If sDbWhere <> "" Then
			If sFilter <> "" Then sDbWhere = "(" & sDbWhere & ") AND (" & sFilter & ")"
		Else
			sDbWhere = sFilter
		End If
		sDbOrderBy = sOrderBy
		If sSort <> "" Then
			sDbOrderBy = sSort
		End If
		sSql = sSelect
		If sDbWhere <> "" Then
			sSql = sSql & " WHERE " & sDbWhere
		End If
		If sGroupBy <> "" Then
			sSql = sSql & " GROUP BY " & sGroupBy
		End If
		If sHaving <> "" Then
			sSql = sSql & " HAVING " & sHaving
		End If
		If sDbOrderBy <> "" Then
			sSql = sSql & " ORDER BY " & sDbOrderBy
		End If
		Return sSql
	End Function	

	' Load a text file
	Public Shared Function ew_LoadTxt(fn As String) As String
		Dim sTxt As String = ""
		If ew_NotEmpty(fn) Then
			Dim sw as StreamReader = File.OpenText(fn)   
	    sTxt = sw.ReadToEnd()      
	    sw.Close()
			End If
		Return sTxt 
	End Function	

	' Write audit trail (login/logout)
	Public Shared Sub ew_WriteAuditTrailOnLogInOut(logtype As String, username As String)
		Try
			Dim field As String = "", oldvalue As String = "", keyvalue As String = "", newvalue As String = ""
			Dim filePfx As String = "log"
			Dim dt As DateTime = DateTime.Now()
			Dim curDate As String = dt.ToString("yyyy/MM/dd")
			Dim curTime As String = dt.ToString("HH:mm:ss")
			Dim table As String = logtype	
			Dim id As String = HttpContext.Current.Request.ServerVariables("SCRIPT_NAME")			
			ew_WriteAuditTrail(filePfx, curDate, curTime, id, username, logtype, table, field, keyvalue, oldvalue, newvalue)
		Catch
			If EW_DEBUG_ENABLED Then Throw	
		End Try
	End Sub	

	' Write audit trail (insert/update/delete)
	Public Shared Sub ew_WriteAuditTrail(pfx As String, curDate As String, curTime As String, id As String, user As String, action As String, table As String, field As String, keyvalue As Object, oldvalue As Object, newvalue As Object)
		Try			
			Dim userwrk As String = user
			If userwrk = "" Then userwrk = "-1" ' assume Administrator if no user

			' Write audit trail to log file
			Dim sHeader As String = "date" & vbTab & "time" & vbTab & "id" & vbTab & "user" & vbTab & "action" & vbTab & "table" & vbTab & "field" & vbTab & "key value" & vbTab & "old value" & vbTab & "new value"
			Dim sMsg As String = curDate & vbTab & curTime & vbTab & id & vbTab & userwrk & vbTab & action & vbTab & table & vbTab & field & vbTab & Convert.ToString(keyvalue) & vbTab & Convert.ToString(oldvalue) & vbTab & Convert.ToString(newvalue)
			Dim sFolder As String = EW_AUDIT_TRAIL_PATH			
			Dim sFn As String = pfx & "_" & DateTime.Now.ToString("yyyyMMdd") & ".txt"			
			Dim bWriteHeader As Boolean = Not File.Exists(ew_UploadPathEx(True, sFolder) & sFn)
			Dim sw as StreamWriter = File.AppendText(ew_UploadPathEx(True, sFolder) & sFn)
			If bWriteHeader Then sw.WriteLine(sHeader) 
			sw.WriteLine(sMsg)
			sw.Close()

			' Sample code to write audit trail to database  (e.g. MS Access)
			' Dim sAuditSql As String = "INSERT INTO AuditTrailTable" & _
			'	" ([date], [time], [id], [user], [action], [table], [field], [keyvalue], [oldvalue], [newvalue])" & _
			'	" VALUES (" & _
			' 	"#" & ew_AdjustSql(curDate) & "#, " & _
			'	"#" & ew_AdjustSql(curTime) & "#, " & _
			'	"""" & ew_AdjustSql(id) & """, " & _
			' 	"""" & ew_AdjustSql(userwrk) & """, " & _
			'	"""" & ew_AdjustSql(action) & """, " & _
			'	"""" & ew_AdjustSql(table) & """, " & _
			'	"""" & ew_AdjustSql(field) & """, " & _
			'	"""" & ew_AdjustSql(keyvalue) & """, " & _
			'	"""" & ew_AdjustSql(oldvalue) & """, " & _
			'	"""" & ew_AdjustSql(newvalue) & """)"
			'	' ew_Write(sAuditSql) ' uncomment to debug
			'	' ew_End()
			' ew_Execute(sAuditSql) 

		Catch
			If EW_DEBUG_ENABLED Then Throw	
		End Try		
	End Sub

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
	Public Shared Function ew_FormatDateTime(ADate As Object, ANamedFormat As Integer) As String
		Dim sDT As String
		If IsDate(ADate) Then
			Dim DT As DateTime = Convert.ToDateTime(ADate)
			If ANamedFormat >= 0 AndAlso ANamedFormat <= 4 Then
				sDT = FormatDateTime(ADate, ANamedFormat)
			ElseIf ANamedFormat = 5 OrElse ANamedFormat = 9 Then 
				sDT = DT.Year & EW_DATE_SEPARATOR & DT.Month & EW_DATE_SEPARATOR & DT.Day
			ElseIf ANamedFormat = 6 OrElse ANamedFormat = 10 Then 
				sDT = DT.Month & EW_DATE_SEPARATOR & DT.Day & EW_DATE_SEPARATOR & DT.Year
			ElseIf ANamedFormat = 7 OrElse ANamedFormat = 11 Then 
				sDT = DT.Day & EW_DATE_SEPARATOR & DT.Month & EW_DATE_SEPARATOR & DT.Year
			ElseIf ANamedFormat = 8 Then 
				sDT = FormatDateTime(ADate, 2)
				If DT.Hour <> 0 OrElse DT.Minute <> 0 OrElse DT.Second <> 0 Then
					sDT = sDT & " " & DT.ToString("HH:mm:ss")
				End If
			ElseIf ANamedFormat = 12 Then 
				sDT = DT.ToString("HH:mm:ss")
			Else
				Return Convert.ToString(DT)
			End If
			If ANamedFormat >= 9 AndAlso ANamedFormat <= 11 Then
				sDT = sDT & " " & DT.ToString("HH:mm:ss")
			End If
			Return sDT
		Else
			Return Convert.ToString(ADate)
		End If
	End Function	

	' Unformat date time based on format type
	Public Shared Function ew_UnFormatDateTime(ADate As Object, ANamedFormat As Integer) As String
		Dim arDate() As String, arDateTime() As String
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
					sDT = sDT & " " & arDateTime(i)
				Next 
			End If
			Return sDT
		Else
			arDate = arDateTime(0).Split(New Char() {Convert.ToChar(EW_DATE_SEPARATOR)})
			If arDate.GetUpperBound(0) = 2 Then
				sDT = arDateTime(0)
				If ANamedFormat = 6 OrElse ANamedFormat = 10 Then ' mmddyyyy
					If arDate(0).Length <= 2 AndAlso arDate(1).Length <= 2 AndAlso arDate(2).Length <= 4 Then
						sDT = arDate(2) & "/" & arDate(0) & "/" & arDate(1)
					End If
				ElseIf ANamedFormat = 7 OrElse ANamedFormat = 11 Then  ' ddmmyyyy
					If arDate(0).Length <= 2 AndAlso arDate(1).Length <= 2 AndAlso arDate(2).Length <= 4 Then
						sDT = arDate(2) & "/" & arDate(1) & "/" & arDate(0)
					End If
				ElseIf ANamedFormat = 5 OrElse ANamedFormat = 9 Then  ' yyyymmdd
					If arDate(0).Length <= 4 AndAlso arDate(1).Length <= 2 AndAlso arDate(2).Length <= 2 Then
						sDT = arDate(0) & "/" & arDate(1) & "/" & arDate(2)
					End If
				End If
				If arDateTime.GetUpperBound(0) > 0 Then
					If IsDate(arDateTime(1)) Then ' Is time
						sDT = sDT & " " & arDateTime(1)
					End If
				End If
				Return sDT
			Else
				Return ADate.ToString()
			End If
		End If
	End Function	

	' Format currency
	Public Shared Function ew_FormatCurrency(Expression As Object, NumDigitsAfterDecimal As Integer, IncludeLeadingDigit As Integer, UseParensForNegativeNumbers As Integer, GroupDigits As Integer) As String
		If Not Information.IsNumeric(Expression) Then Return Convert.ToString(Expression)
		If IsDbNull(Expression) Then Return String.Empty 		
		Return Strings.FormatCurrency(Expression, NumDigitsAfterDecimal, IncludeLeadingDigit, UseParensForNegativeNumbers, GroupDigits)
	End Function	

	' Format number
	Public Shared Function ew_FormatNumber(Expression As Object, NumDigitsAfterDecimal As Integer, IncludeLeadingDigit As Integer, UseParensForNegativeNumbers As Integer, GroupDigits As Integer) As String
		If Not Information.IsNumeric(Expression) Then Return Convert.ToString(Expression)
		If IsDbNull(Expression) Then Return String.Empty
		Return Strings.FormatNumber(Expression, NumDigitsAfterDecimal, IncludeLeadingDigit, UseParensForNegativeNumbers, GroupDigits)
	End Function	

	' Format percent
	Public Shared Function ew_FormatPercent(Expression As Object, NumDigitsAfterDecimal As Integer, IncludeLeadingDigit As Integer, UseParensForNegativeNumbers As Integer, GroupDigits As Integer) As String
		If Not Information.IsNumeric(Expression) Then Return Convert.ToString(Expression)
		If IsDbNull(Expression) Then Return String.Empty
		Return Strings.FormatPercent(Expression, NumDigitsAfterDecimal, IncludeLeadingDigit, UseParensForNegativeNumbers, GroupDigits)		
	End Function	

	' Encode HTML
	Public Shared Function ew_HtmlEncode(Expression As Object) As String
		Return HttpContext.Current.Server.HtmlEncode(Convert.ToString(Expression))
	End Function	

	' Encode value for single-quoted JavaScript string
	Public Shared Function ew_JsEncode(val As Object) As String
		Return Convert.ToString(val).Replace("'", "\'")
	End Function	

	' Generate Value Separator based on current row count
	' rowcnt - zero based row count
	Public Shared Function ew_ValueSeparator(rowcnt As Integer) As String
		Return ", "
	End Function	

	' Generate View Option Separator based on current row count (Multi-Select / CheckBox)
	' rowcnt - zero based row count
	Public Shared Function ew_ViewOptionSeparator(rowcnt As Integer) As String
		Dim Sep As String = ", "

		' Sample code to adjust 2 options per row
		'If ((rowcnt + 1) Mod 2 = 0) Then ' 2 options per row
		'	Sep = Sep & "<br>"
		'End If

		Return Sep		
	End Function	

	' Render repeat column table
	' rowcnt - zero based row count
	Public Shared Function ew_RepeatColumnTable(totcnt As Integer, rowcnt As Integer, repeatcnt As Integer, rendertype As Integer) As String
		Dim sWrk As String = ""
		If rendertype = 1 Then ' Render control start
			If rowcnt = 0 Then sWrk = sWrk & "<table class=""" & EW_ITEM_TABLE_CLASSNAME & """>"
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
	Public Shared Function ew_TruncateMemo(str As String, ln As Integer) As String
		Dim j As Integer, i As Integer, k As Integer
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
						If i < j Then	k = i	Else k = j
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

	' Send notify email
	Public Shared Sub ew_SendNotifyEmail(sFn As String, sSubject As String, sTable As String, sKey As String, sAction As String)
		Try
			Dim bEmailSent As Boolean
			If EW_SENDER_EMAIL <> "" And EW_RECIPIENT_EMAIL <> "" Then
				bEmailSent = ew_SendTemplateEmail(sFn, EW_SENDER_EMAIL, EW_RECIPIENT_EMAIL, "", "", sSubject, New String(){"<!--table-->", sTable, "<!--key-->", sKey, "<!--action-->", sAction})
			End If
		Catch
			If EW_DEBUG_ENABLED Then Throw
		End Try
	End Sub	

	' Send email by template
	Public Shared Function ew_SendTemplateEmail(sTemplate As String, sSender As String, sRecipient As String, sCcEmail As String, sBccEmail As String, sSubject As String, arContent As String()) As Boolean
		Try
			Dim cnt As Integer
			Dim Email As cEmail
			If sSender <> "" AndAlso sRecipient <> "" Then
				Email = New cEmail
				Email.Load(sTemplate)
				Email.ReplaceSender(sSender) ' Replace Sender
				Email.ReplaceRecipient(sRecipient) ' Replace Recipient
				If sCcEmail <> "" Then Email.AddCC(sCcEmail) ' Add Cc
				If sBccEmail <> "" Then Email.AddBcc(sBccEmail) ' Add Bcc
				If sSubject <> "" Then Email.ReplaceSubject(sSubject) ' Replace subject
				If IsArray(arContent) Then
					cnt = arContent.Length
					If cnt Mod 2 = 1 Then cnt = cnt - 1
					For i As Integer = 0 To cnt Step 2
						Email.ReplaceContent(arContent(i), arContent(i + 1))
					Next 
				End If
				Return Email.Send()
			Else
				Return False
			End If
		Catch
			If EW_DEBUG_ENABLED Then Throw
			Return False
		End Try
	End Function	

	' Send email
	Public Shared Function ew_SendEmail(sFrEmail As String, sToEmail As String, sCcEmail As String, sBccEmail As String, sSubject As String, sMail As String, sFormat As String) As Boolean
		Dim mail As New System.Net.Mail.MailMessage()
		If sFrEmail <> "" Then
			mail.From = New System.Net.Mail.MailAddress(sFrEmail)
		End If
		If sToEmail <> "" Then
			sToEmail = sToEmail.Replace(","c, ";"c)
			Dim arTo() As String = sToEmail.Split(New Char() {";"c})
			For Each strTo As String In arTo
				mail.To.Add(strTo)
			Next
		End If
		If sCcEmail <> "" Then
			sCcEmail = sCcEmail.Replace(","c, ";"c)
			Dim arCC() As String = sCcEmail.Split(New Char() {";"c})
			For Each strCC As String In arCC
				mail.CC.Add(strCC)
			Next
		End If
		If sBccEmail <> "" Then
			sBccEmail = sBccEmail.Replace(","c, ";"c)
			Dim arBcc() As String = sBccEmail.Split(New Char() {";"c})
			For Each strBcc As String In arBcc
				mail.Bcc.Add(strBcc)
			Next
		End If		
		mail.Subject = sSubject
		mail.Body = sMail
		mail.IsBodyHtml = ew_SameText(sFormat, "html")
		Dim smtp As New System.Net.Mail.SmtpClient()
		If EW_SMTP_SERVER <> "" Then
			smtp.Host = EW_SMTP_SERVER
		Else
			smtp.Host = "localhost"
		End If
		If EW_SMTP_SERVER_PORT > 0 Then
			smtp.Port = EW_SMTP_SERVER_PORT
		End If
		If EW_SMTP_SERVER_USERNAME <> "" AndAlso EW_SMTP_SERVER_PASSWORD <> "" Then
			Dim smtpuser As New System.Net.NetworkCredential()
			smtpuser.UserName = EW_SMTP_SERVER_USERNAME
			smtpuser.Password = EW_SMTP_SERVER_PASSWORD
			smtp.UseDefaultCredentials = False
			smtp.Credentials = smtpUser
		End If
		Try
			smtp.Send(mail)
			Return True
		Catch
			If EW_DEBUG_ENABLED Then Throw
			Return False
		End Try
	End Function	

	' Return path of the uploaded file
	'	Parameter: If PhyPath is true(1), return physical path on the server
	'	           If PhyPath is false(0), return relative URL
	Public Shared Function ew_UploadPathEx(PhyPath As String, DestPath As String) As String
		Dim pos As Integer, Path As String
		If DestPath.StartsWith("~/") Then DestPath = DestPath.Substring(2)
		If PhyPath Then				
			Path = HttpContext.Current.Request.ServerVariables("APPL_PHYSICAL_PATH")
			Path = ew_IncludeTrailingDelimiter(Path, PhyPath)
			Path = Path & DestPath.Replace("/", "\")
		Else
			Path = HttpContext.Current.Request.ServerVariables("APPL_MD_PATH")
			pos = Path.IndexOf("Root", StringComparison.InvariantCultureIgnoreCase)
			If pos > 0 Then Path = Path.Substring(pos + 4)
			Path = ew_IncludeTrailingDelimiter(Path, PhyPath)
			Path = Path & DestPath
		End If
		Return ew_IncludeTrailingDelimiter(Path, PhyPath)
	End Function	

	' Change the file name of the uploaded file
	Public Shared Function ew_UploadFileNameEx(folder As String, FileName As String) As String
		Dim OutFileName As String

		' By default, ewUniqueFileName() is used to get an unique file name.
		' Amend your logic here

		OutFileName = ew_UniqueFileName(folder, FileName)

		' Return computed output file name
		Return OutFileName
	End Function	

	' Return path of the uploaded file
	' returns global upload folder, for backward compatibility only
	Public Shared Function ew_UploadPath(PhyPath As Boolean) As String
		Return ew_UploadPathEx(PhyPath, EW_UPLOAD_DEST_PATH)
	End Function	

	' Change the file name of the uploaded file
	' use global upload folder, for backward compatibility only
	Public Shared Function ew_UploadFileName(FileName As String) As String
		Return ew_UploadFileNameEx(ew_UploadPath(True), FileName)
	End Function	

	' Generate an unique file name (filename(n).ext)
	Public Shared Function ew_UniqueFileName(folder As String, FileName As String) As String
		If FileName = "" Then FileName = ew_DefaultFileName()
		If FileName = "." Then Throw New Exception("Invalid file name: " & FileName)
		If folder = "" Then Throw New Exception("Unspecified folder")
		Dim Name As String = Path.GetFileNameWithoutExtension(FileName)
		Dim Ext As String = Path.GetExtension(FileName)
		folder = ew_IncludeTrailingDelimiter(folder, True)
		If Not Directory.Exists(folder) AndAlso Not ew_CreateFolder(folder) Then
			Throw New Exception("Folder does not exist: " & folder)
		End If
		Dim Index As Integer = 0
		Dim Suffix As String = ""		

		' Check to see if filename exists
		While File.Exists(folder & Name & Suffix & Ext)
			Index = Index + 1
			Suffix = "(" & Index & ")"			
		End While

		' Return unique file name
		Return Name & Suffix & Ext
	End Function	

	' Create a default file name (yyyymmddhhmmss.bin)
	Public Shared Function ew_DefaultFileName() As Object
		Dim DT As DateTime = DateTime.Now()
		Return DT.ToString("yyyyMMddHHmmss") & ".bin"
	End Function	

	' Get path relative to application root
	Public Shared Function ew_ServerMapPath(Path As String) As String
		Return ew_PathCombine(HttpContext.Current.Request.ServerVariables("APPL_PHYSICAL_PATH"), Path, True)
	End Function

	' Get path relative to a base path
	Public Shared Function ew_PathCombine(BasePath As String, RelPath As String, PhyPath As Boolean) As String
		Dim p2 As Integer, p1 As Integer
		Dim Path2 As String, Path As String, Delimiter As String
		BasePath = ew_RemoveTrailingDelimiter(BasePath, True)
		If PhyPath Then
			Delimiter = "\"
			RelPath = RelPath.Replace("/", "\")
		Else
			Delimiter = "/"
			RelPath = RelPath.Replace("\", "/")
		End If
		If RelPath = "." Or RelPath = ".." Then RelPath = RelPath & Delimiter
		p1 = RelPath.IndexOf(Delimiter)
		Path2 = ""
		While p1 > -1
			Path = RelPath.Substring(0, p1 + 1)
			If Path = Delimiter OrElse Path = "." & Delimiter Then				

				' Skip
			ElseIf Path = ".." & Delimiter Then 
				p2 = BasePath.LastIndexOf(Delimiter)
				If p2 > -1 Then BasePath = BasePath.Substring(0, p2)
			Else
				Path2 = Path2 & Path
			End If
			RelPath = RelPath.Substring(p1 + 1)
			p1 = RelPath.IndexOf(Delimiter)
		End While
		Return ew_IncludeTrailingDelimiter(BasePath, True) & Path2 & RelPath
	End Function	

	' Remove the last delimiter for a path
	Public Shared Function ew_RemoveTrailingDelimiter(Path As String, ByVal PhyPath As Boolean) As String
		Dim Delimiter As String
		If PhyPath Then Delimiter = "\" Else Delimiter = "/"
		While Path.EndsWith(Delimiter)
			Path = Path.Substring(0, Path.Length - 1)
		End While
		Return Path
	End Function	

	' Include the last delimiter for a path
	Public Shared Function ew_IncludeTrailingDelimiter(Path As String, PhyPath As Boolean) As String
		Dim Delimiter As String
		Path = ew_RemoveTrailingDelimiter(Path, PhyPath)
		If PhyPath Then Delimiter = "\" Else Delimiter = "/"
		Return Path & Delimiter
	End Function	

	' Write the paths for config/debug only
	Public Shared Sub ew_WriteUploadPaths()
		ew_Write("APPL_PHYSICAL_PATH = " & HttpContext.Current.Request.ServerVariables("APPL_PHYSICAL_PATH") & "<br>")
		ew_Write("APPL_MD_PATH = " & HttpContext.Current.Request.ServerVariables("APPL_MD_PATH") & "<br>")
	End Sub	

	' Get current page name
	Public Shared Function ew_CurrentPage() As String
		Return ew_GetPageName(HttpContext.Current.Request.ServerVariables("SCRIPT_NAME"))
	End Function	

	' Get refer page name
	Public Shared Function ew_ReferPage() As String
		Return ew_GetPageName(HttpContext.Current.Request.ServerVariables("HTTP_REFERER"))
	End Function	

	' Get page name
	Public Shared Function ew_GetPageName(url As String) As String
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
	Public Shared Function ew_FullUrl() As String
		Dim sUrl As String = "http"
		Dim bSSL As Boolean = Not ew_SameText(HttpContext.Current.Request.ServerVariables("HTTPS"), "off")
		Dim sPort As String = HttpContext.Current.Request.ServerVariables("SERVER_PORT")
		Dim defPort As String = IIf(bSSL, "443", "80")
		If sPort = defPort Then sPort = "" Else sPort = ":" & sPort
		If bSSL Then sUrl = sUrl & "s"
		Return sUrl & "://" & HttpContext.Current.Request.ServerVariables("SERVER_NAME") & sPort & HttpContext.Current.Request.ServerVariables("SCRIPT_NAME")
	End Function	

	' Convert to full URL
	Public Shared Function ew_ConvertFullUrl(url As String) As String		
		If url = "" Then
			Return ""
		ElseIf url.Contains("://") Then 
			Return url
		Else
			Dim sUrl As String = ew_FullUrl()
			Return sUrl.Substring(0, sUrl.LastIndexOf("/")+1) & url
		End If
	End Function	

	' Check if folder exists
	Public Shared Function ew_FolderExists(folder As String) As Boolean
		Return Directory.Exists(folder)	
	End Function	

	' Check if file exists
	Public Shared Function ew_FileExists(folder As String, fn As String) As Boolean
		Return File.Exists(folder & fn)
	End Function	

	' Delete file
	Public Shared Sub ew_DeleteFile(FilePath As String)
		File.Delete(FilePath)
	End Sub	

	' Rename file
	Sub ew_RenameFile(OldFilePath As String, NewFilePath As String)
		File.Move(OldFilePath, NewFilePath)	
	End Sub	

	' Create folder
	Public Shared Function ew_CreateFolder(folder As String) As Boolean
		Try
			Dim di As DirectoryInfo = Directory.CreateDirectory(folder)
			Return (di IsNot Nothing)
		Catch
			Return False
		End Try
	End Function

	' Export header
	Public Shared Function ew_ExportHeader(ExpType As String) As String
		Select Case ExpType
			Case "html"
				Return "<link rel=""stylesheet"" type=""text/css"" href=""" & EW_PROJECT_CSSFILE & """>" & _
					"<table class=""ewExportTable"">"
			Case "word", "excel"
				Return "<table>"
			Case Else ' "csv"
				Return ""
		End Select
	End Function	

	' Export footer
	Public Shared Function ew_ExportFooter(ExpType As String) As String
		Select Case ExpType
			Case "html", "word", "excel"
				Return "</table>"
			Case Else ' "csv"
				Return ""
		End Select
	End Function	

	' Export value
	Public Shared Sub ew_ExportAddValue(ByRef str As String, val As Object, ExpType As String)
		Select Case ExpType
			Case "html", "word", "excel"
				str = str & "<td>" & Convert.ToString(val) & "</td>"
			Case "csv"
				If str <> "" Then str = str & ","
				str = str & """" & Convert.ToString(val).Replace("""", """""") & """"
		End Select
	End Sub	

	' Export line
	Public Shared Function ew_ExportLine(str As String, ExpType As String) As String
		Select Case ExpType
			Case "html", "word", "excel"
				Return "<tr>" & str & "</tr>"
			Case Else ' "csv"
				Return str & vbCrLf
		End Select
	End Function	

	' Export field
	Public Shared Function ew_ExportField(cap As String, val As Object, ExpType As String) As String
		Return "<tr><td>" & cap & "</td><td>" & Convert.ToString(val) & "</td></tr>"
	End Function		

	'
	'  Form object class
	'
	Class cFormObj

		Public Index As Integer = 0 ' Index to handle multiple form elements

		' Get form element name based on index
		Public Function GetIndexedName(name As String) As String
			Dim Pos As Integer
			If Index <= 0 Then
				Return name
			Else
				Pos = name.IndexOf("_")
				If Pos = 1 OrElse Pos = 2 Then
					Return name.Substring(0, Pos) & Index & name.Substring(Pos)
				Else
					Return name
				End If
			End If
		End Function

		' Get value for form element
		Public Function GetValue(name As String) As Object
			Dim wrkname As String = GetIndexedName(name)
			Return ew_Post(wrkname)			
		End Function

		' Get upload file size
		Function GetUploadFileSize(name As String) As Long
			Dim wrkname As String = GetIndexedName(name)
			Dim file As HttpPostedFile = HttpContext.Current.Request.Files(wrkname)
			If file IsNot Nothing AndAlso file.ContentLength > 0 Then
				Return HttpContext.Current.Request.Files(wrkname).ContentLength
			Else
				Return -1
			End If
		End Function

		' Get upload file name
		Function GetUploadFileName(name As String) As String			
			Dim wrkname As String = GetIndexedName(name)
			Dim file As HttpPostedFile = HttpContext.Current.Request.Files(wrkname)
			If file IsNot Nothing AndAlso file.ContentLength > 0 Then
				Dim FileName As String = HttpContext.Current.Request.Files(wrkname).FileName
				Return Path.GetFileName(FileName)
			Else
				Return ""
			End If
		End Function

		' Get file content type
		Function GetUploadFileContentType(name As String) As String
			Dim wrkname As String = GetIndexedName(name)
			Dim file As HttpPostedFile = HttpContext.Current.Request.Files(wrkname)
			If file IsNot Nothing AndAlso file.ContentLength > 0 Then
				Return HttpContext.Current.Request.Files(wrkname).ContentType
			Else
				Return ""
			End If
		End Function

		' Get upload file data
		Function GetUploadFileData(name As String) As Object
			Dim wrkname As String = GetIndexedName(name)
			Dim file As HttpPostedFile = HttpContext.Current.Request.Files(wrkname)
			If file IsNot Nothing AndAlso file.ContentLength > 0 Then
				Dim filelength As Integer = file.ContentLength		    
				Dim bindata(filelength) As Byte
				Dim fs As Stream = file.InputStream
				fs.Read(bindata, 0, filelength)				
				Return bindata
			Else
				Return System.DBNull.Value
			End If
		End Function

		' Get file image width
		Function GetUploadImageWidth(name As String) As Integer
			Dim wrkname As String = GetIndexedName(name)
			Dim file As HttpPostedFile = HttpContext.Current.Request.Files(wrkname)
			If file IsNot Nothing AndAlso file.ContentLength > 0 Then
				Try
					Dim img As System.Drawing.Image = System.Drawing.Image.FromStream(file.InputStream)
					Return img.PhysicalDimension.Width
				Catch
					Return -1	
				End Try			
			Else
				Return -1
			End If
		End Function

		' Get file image height
		Function GetUploadImageHeight(name As String) As Integer
			Dim wrkname As String = GetIndexedName(name)
			Dim file As HttpPostedFile = HttpContext.Current.Request.Files(wrkname)
			If file IsNot Nothing AndAlso file.ContentLength > 0 Then
				Try
					Dim img As System.Drawing.Image = System.Drawing.Image.FromStream(file.InputStream)
					Return img.PhysicalDimension.Height
				Catch
					Return -1	
				End Try
			Else
				Return -1
			End If
		End Function
	End Class

	' Resize binary to thumbnail
	Public Shared Function ew_ResizeBinary(ByRef filedata As Byte(), ByRef width As Integer, ByRef height As Integer, interpolation As Integer) As Boolean
		Return True ' No resize
	End Function	

	' Resize file to thumbnail file
	Public Shared Function ew_ResizeFile(fn As String, tn As String, ByRef width As Integer, ByRef height As Integer, interpolation As Integer) As Boolean
		Try
			If File.Exists(fn) Then
				File.Copy(fn, tn) ' Copy only
				Return True
			End If
		Catch
			If EW_DEBUG_ENABLED Then Throw
			Return False
		End Try		
	End Function	

	' Resize file to binary
	Public Shared Function ew_ResizeFileToBinary(fn As String, ByRef width As Integer, ByRef height As Integer, interpolation As Integer) As Object
		Try
			If File.Exists(fn) Then
				Dim oFile As FileInfo
		    oFile = New FileInfo(fn)	
		    Dim fs As FileStream = oFile.OpenRead()
		    Dim lBytes As Long = fs.Length
				If lBytes > 0 Then
					Dim fileData(lBytes-1) As Byte				
					fs.Read(fileData, 0, lBytes) ' Read the file into a byte array
					fs.Close()
					fs.Dispose()
					Return fileData
				End If	  		
		  End If
			Return System.DBNull.Value
		Catch
			If EW_DEBUG_ENABLED Then Throw
			Return System.DBNull.Value
		End Try		
	End Function

	' Menu class	
	Class cMenu
		Inherits AspNetMakerBase

		Public Id As Object		

		Public IsRoot As Boolean		

		Public ItemData As New ArrayList ' ArrayList of cMenuItem

		' Init
		Public Sub New(AId As Object, ARoot As Boolean)
			Id = AId
			IsRoot = ARoot			
		End Sub			

		' Add a menu item
		Sub AddMenuItem(id As Integer, text As String, url As String, parentid As Integer, src As String, allowed As Boolean)
			Dim oParentMenu As cMenuItem
			Dim item As New cMenuItem(id, text, url, parentid, src, allowed)
			item.ParentPage = Me.ParentPage
			If Not ParentPage.MenuItem_Adding(item) Then
				Exit Sub
			End If
			If item.ParentId < 0 Then
				AddItem(item)
			Else
				If FindItem(item.ParentId, oParentMenu) Then
					oParentMenu.AddItem(item)
				End If
			End If
		End Sub

		' Add item to internal ArrayList
		Sub AddItem(ByRef item As cMenuItem)
			ItemData.Add(item)
		End Sub

		' Find item
		Function FindItem(id As Integer, ByRef out As Object) As Boolean
			Dim item As cMenuItem
			FindItem = False
			For i As Integer = 0 To ItemData.Count - 1
				item = CType(ItemData(i), cMenuItem)
				If item.Id = id Then
					out = item
					Return True
				ElseIf Not IsDbNull(item.SubMenu) Then 
					If item.SubMenu.FindItem(id, out) Then Return True
				End If
			Next 
		End Function

		' Check if a menu item should be shown
		Public Function RenderItem(ByVal item As cMenuItem) As Boolean
			If Not IsDbNull(item.SubMenu) Then
				For Each subitem As cMenuItem In item.SubMenu.ItemData
					If item.SubMenu.RenderItem(subitem) Then
						Return True
					End If
				Next
			End If
			Return (item.Allowed AndAlso ew_NotEmpty(item.Url))
		End Function

		' Check if this menu should be rendered
		Public Function RenderMenu() As Boolean
			For Each item As cMenuItem In ItemData
				If RenderItem(item) Then
					Return True
				End If
			Next
			Return False
		End Function

		' Render the menu
		Sub Render()
			If Not RenderMenu() Then Return
			Dim i As Integer
			Dim item As cMenuItem
			Dim itemcnt As Integer = ItemData.Count
			ew_Write("<ul")
			If ew_NotEmpty(Id) Then
				If IsNumeric(Id) Then
					ew_Write(" id=""menu_" & Id & """")
				Else
					ew_Write(" id=""" & Id & """")
				End If
			End If
			If IsRoot Then
				ew_Write(" class=""" & EW_MENUBAR_VERTICAL_CLASSNAME & """")
			End If
			ew_Write(">" & vbCrLf)
			For i = 0 To itemcnt - 1
				If RenderItem(ItemData(i)) Then
					ew_Write("<li><a")
					If Not IsDbNull(ItemData(i).SubMenu) Then
						ew_Write(" class=""" & EW_MENUBAR_SUBMENU_CLASSNAME & """")
					End If
					If ItemData(i).Url <> "" Then
						ew_Write(" href=""" & ew_HtmlEncode(ItemData(i).Url) & """")
					End If
					ew_Write(">" & ItemData(i).Text & "</a>" & vbCrLf)
					If Not IsDbNull(CType(ItemData(i), cMenuItem).SubMenu) Then
						item = CType(ItemData(i), cMenuItem)
						item.SubMenu.Render()
					End If
					ew_Write("</li>" & vbCrLf)
				End If
			Next 
			ew_Write("</ul>" & vbCrLf)
		End Sub		
	End Class	

	' Menu item class
	Class cMenuItem
		Inherits AspNetMakerBase

		Public Id As Integer

		Public Text As String

      Private _url As String

        Public Property Url() As String
            Get
                Return _url
            End Get
            Set(ByVal value As String)
                If Left(value, 1) <> "/" Then
                    _url = "/wpmgen/" & value
                Else
                    _url = value
                End If
            End Set
        End Property

		Public ParentId As Integer

		Public SubMenu As Object

		Public Source As String

		Public Allowed As Boolean = True

		Public Sub New(AId As Integer, AText As String, AUrl As String, AParentId As Integer, ASource As String, AAllowed As Boolean)
			Id = AId
			Text = AText
			Url = AUrl
			ParentId = AParentId
			SubMenu = System.DBNull.Value
			Source = ASource
			Allowed = AAllowed
		End Sub

		Sub AddItem(ByRef item As cMenuItem) ' Add submenu item
			If IsDBNull(SubMenu) Then
				SubMenu = New cMenu(Id, False)
				SubMenu.ParentPage = Me.ParentPage
			End If
			SubMenu.AddItem(item)
		End Sub

		Function AsString() As String
			Dim OutStr As String = "{ Id: " & Id & ", Text: " & Text & ", Url: " & Url & ", ParentId: " & ParentId
			If IsDBNull(SubMenu) Then
				OutStr = OutStr & ", SubMenu: (Null)"
			Else
				OutStr = OutStr & ", SubMenu: (Object)"
			End If
			OutStr = OutStr & ", Source: " & Source
			OutStr = OutStr & ", Allowed: " & Allowed
			Return OutStr & " }" & "<br />"
		End Function
	End Class

	' Is Admin
	Public Function IsAdmin() As Boolean
		If Security IsNot Nothing Then
			Return Security.IsAdmin()
		Else
			Return (ew_ConvertToInt(ew_Session(EW_SESSION_USER_LEVEL_ID)) = -1 OrElse ew_ConvertToInt(ew_Session(EW_SESSION_SYS_ADMIN)) = 1)
		End If
	End Function

	' Allow list
	Public Function AllowList(TableName As String) As Boolean
		If Security IsNot Nothing Then
			Return Security.AllowList(TableName)
		Else
			Return True
		End If
	End Function	

	' Allow add
	Public Function AllowAdd(TableName As String) As Boolean
		If Security IsNot Nothing Then
			Return Security.AllowAdd(TableName)
		Else
			Return True
		End If
	End Function

	' Validation functions
	' Check date format
	' format: std/us/euro
	Public Shared Function ew_CheckDateEx(value As String, format As String, sep As String) As Boolean
		If value = ""	Then Return True
		While value.Contains("  ")
			value = value.Replace("  ", " ")
		End While
		value = value.Trim()
		Dim arDT() As String, arD() As String, pattern As String
		Dim sYear As String, sMonth As String, sDay As String
		arDT = value.Split(New Char() {" "c})
		If arDT.Length > 0 Then
			sep = "\" & sep
			Select Case format
				Case "std"
					pattern = "^([0-9]{4})" & sep & "([0]?[1-9]|[1][0-2])" & sep & "([0]?[1-9]|[1|2][0-9]|[3][0|1])"
				Case "us"
					pattern = "^([0]?[1-9]|[1][0-2])" & sep & "([0]?[1-9]|[1|2][0-9]|[3][0|1])" & sep & "([0-9]{4})"
				Case "euro"
					pattern = "^([0]?[1-9]|[1|2][0-9]|[3][0|1])" & sep & "([0]?[1-9]|[1][0-2])" & sep & "([0-9]{4})"
			End Select
			Dim re As New Regex(pattern) 
			If Not re.IsMatch(arDT(0)) Then Return False
			arD = arDT(0).split(New Char() {Convert.ToChar(EW_DATE_SEPARATOR)})
			Select Case format
				Case "std"
					sYear = arD(0)
					sMonth = arD(1)
					sDay = arD(2)				
				Case "us"
					sYear = arD(2)
					sMonth = arD(0)
					sDay = arD(1)
				Case "euro"
					sYear = arD(2)
					sMonth = arD(1)
					sDay = arD(0)					
			End Select
			If Not ew_CheckDay(ew_ConvertToInt(sYear), ew_ConvertToInt(sMonth), ew_ConvertToInt(sDay)) Then Return False
		End If
		If arDT.Length > 1 AndAlso Not ew_CheckTime(arDT(1)) Then Return False
		Return True
	End Function

	' Check Date format (yyyy/mm/dd)
	Public Shared Function ew_CheckDate(value As String) As Boolean
		Return ew_CheckDateEx(value, "std", EW_DATE_SEPARATOR)
	End Function

	' Check US Date format (mm/dd/yyyy)
	Public Shared Function ew_CheckUSDate(value As String) As Boolean
		Return ew_CheckDateEx(value, "us", EW_DATE_SEPARATOR)
	End Function

	' Check Euro Date format (dd/mm/yyyy)
	Public Shared Function ew_CheckEuroDate(value As String) As Boolean
		Return ew_CheckDateEx(value, "euro", EW_DATE_SEPARATOR)
	End Function

	' Check day
	Public Shared Function ew_CheckDay(checkYear As Integer, checkMonth As Integer, checkDay As Integer) As Boolean
		Dim maxDay As Integer = 31
		If checkMonth = 4 OrElse checkMonth = 6 OrElse checkMonth = 9 OrElse checkMonth = 11 Then
			maxDay = 30
		ElseIf checkMonth = 2 Then
			If checkYear Mod 4 > 0 Then
				maxDay = 28
			ElseIf checkYear Mod 100 = 0 AndAlso checkYear Mod 400 > 0 Then
				maxDay = 28
			Else
				maxDay = 29
			End If
		End If
		Return ew_CheckRange(checkDay, 1, maxDay)
	End Function

	' Check integer
	Public Shared Function ew_CheckInteger(value As String) As Boolean
		If value = ""	Then Return True
		Dim re As New Regex("^\-?\+?[0-9]+") 
		Return re.IsMatch(value)
	End Function

	' Check number range
	Public Shared Function ew_NumberRange(value As String, min As Object, max As Object) As Boolean
		If (min IsNot Nothing AndAlso Convert.ToDouble(value) < Convert.ToDouble(min)) OrElse (max IsNot Nothing AndAlso Convert.ToDouble(value) > Convert.ToDouble(max)) Then
			Return False
		End If
		Return True
	End Function

	' Check number
	Public Shared Function ew_CheckNumber(value As String) As Boolean
		If value = ""	Then Return True
		Return IsNumeric(Trim(value))
	End Function

	' Check range
	Public Shared Function ew_CheckRange(value As String, min As Object, max As Object) As Boolean
		If value = ""	Then Return True
		If Not ew_CheckNumber(value) Then Return False
		Return ew_NumberRange(value, min, max)
	End Function

	' Check time
	Public Shared Function ew_CheckTime(value As String) As Boolean
		If value = ""	Then Return True
		Dim re As New Regex("^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]") 
		Return re.IsMatch(value)
	End Function

	' Check US phone number
	Public Shared Function ew_CheckPhone(value As String) As Boolean
		If value = ""	Then Return True
		Dim re As New Regex("^\(\d{3}\) ?\d{3}( |-)?\d{4}|^\d{3}( |-)?\d{3}( |-)?\d{4}") 
		Return re.IsMatch(value)
	End Function

	' Check US zip code
	Public Shared Function ew_CheckZip(value As String) As Boolean
		If value = ""	Then Return True
		Dim re As New Regex("^\d{5}|^\d{5}-\d{4}") 
		Return re.IsMatch(value)
	End Function

	' Check credit card
	Public Shared Function ew_CheckCreditCard(value As String) As Boolean
		If value = ""	Then Return True
		Dim creditcard As New Hashtable
		Dim match As Boolean = False
		creditcard.Add("visa", "^4\d{3}[ -]?\d{4}[ -]?\d{4}[ -]?\d{4}")
		creditcard.Add("mastercard", "^5[1-5]\d{2}[ -]?\d{4}[ -]?\d{4}[ -]?\d{4}")
		creditcard.Add("discover", "^6011[ -]?\d{4}[ -]?\d{4}[ -]?\d{4}")
		creditcard.Add("amex", "^3[4,7]\d{13}")
		creditcard.Add("diners", "^3[0,6,8]\d{12}")
		creditcard.Add("bankcard", "^5610[ -]?\d{4}[ -]?\d{4}[ -]?\d{4}")
		creditcard.Add("jcb", "^[3088|3096|3112|3158|3337|3528]\d{12}")
		creditcard.Add("enroute", "^[2014|2149]\d{11}")
		creditcard.Add("switch", "^[4903|4911|4936|5641|6333|6759|6334|6767]\d{12}")
		Dim re As Regex 
		For Each de As DictionaryEntry In creditcard
			re = New Regex(de.Value)
			If re.IsMatch(value) Then
				Return ew_CheckSum(value)
			End If
		Next
		Return False
	End Function

	' Check sum
	Public Shared Function ew_CheckSum(value As String) As Boolean
		Dim checksum As Integer, digit As Byte
		value = value.Replace("-", "")
		value = value.Replace(" ", "")
		checksum = 0
		For i As Integer = 2-(value.Length Mod 2) To value.Length Step 2
			checksum = checksum + Convert.ToByte(value.Chars(i-1))
		Next
	  For i As Integer = (value.Length Mod 2)+1 To value.Length Step 2
		  digit = Convert.ToByte(value.Chars(i-1)) * 2
			checksum = checksum + IIf(digit < 10, digit, digit-9)
	  Next
		Return (checksum Mod 10 = 0)
	End Function

	' Check US social security number
	Public Shared Function ew_CheckSSC(value As String) As Boolean
		If value = ""	Then Return True
		Dim re As New Regex("^(?!000)([0-6]\d{2}|7([0-6]\d|7[012]))([ -]?)(?!00)\d\d\3(?!0000)\d{4}") 
		Return re.IsMatch(value)
	End Function

	' Check email
	Public Shared Function ew_CheckEmail(value As String) As Boolean
		If value = ""	Then Return True
		Dim re As New Regex("^[A-Za-z0-9\._\-+]+@[A-Za-z0-9_\-+]+(\.[A-Za-z0-9_\-+]+)+") 
		Return re.IsMatch(value)
	End Function

	' Check GUID
	Public Shared Function ew_CheckGUID(value As String) As Boolean
		If value = ""	Then Return True
		Dim re1 As New Regex("^{{1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}}{1}")
		Dim re2 As New Regex("^([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}")
		Return re1.IsMatch(value) OrElse re2.IsMatch(value)
	End Function

	' Check file extension
	Public Shared Function ew_CheckFileType(value As String) As Boolean
		If value = ""	OrElse ew_Empty(EW_UPLOAD_ALLOWED_FILE_EXT)	Then Return True		
		Dim extension As String = Path.GetExtension(value).Substring(1)
		Dim allowExt() As String = EW_UPLOAD_ALLOWED_FILE_EXT.Split(New Char() {","c})
		For Each ext As String In allowExt
			If ew_SameText(ext, extension) Then Return True
		Next
		Return False
	End Function

	' MD5
	Public Shared Function MD5(InputStr As String) As String
		Dim Md5Hasher As New MD5CryptoServiceProvider()
		Dim Data As Byte() = Md5Hasher.ComputeHash(Encoding.Default.GetBytes(InputStr))
		Dim sBuilder As New StringBuilder()
		For i As Integer = 0 To Data.Length - 1
			sBuilder.Append(Data(i).ToString("x2"))
		Next i
		Return sBuilder.ToString()	
	End Function

	' Invoke method with no parameter
	Public Function ew_InvokeMethod(ByVal name As String, ByVal parameters As Object()) As Object
		Dim mi As MethodInfo = GetType(AspNetMaker7_WPMGen).GetMethod(name)
		If mi IsNot Nothing Then
			Return mi.Invoke(Me, parameters)
		Else
			Return False
		End If
	End Function

	' Get field value
	Public Shared Function ew_GetFieldValue(ByVal name As String) As Object
		Dim fi As FieldInfo = GetType(AspNetMaker7_WPMGen).GetField(name)
		If fi IsNot Nothing Then Return fi.GetValue(Nothing)
		Return Nothing
	End Function

	' Check if object is ArrayList
	Public Shared Function ew_IsArrayList(ByVal obj As Object) As Boolean
		Return (obj IsNot Nothing) AndAlso (obj.GetType().ToString() = "System.Collections.ArrayList")
	End Function

	' Global random
	Private Shared GlobalRandom As New Random()

	' Get a random number
	Public Shared Function ew_Random() As Integer
		SyncLock GlobalRandom
			Dim NewRandom As New Random(GlobalRandom.Next())
			Return NewRandom.Next()
		End SyncLock
	End Function

	' Get query string value
	Public Shared Function ew_Get(name As String) As String
		If HttpContext.Current.Request.QueryString(name) IsNot Nothing Then
			Return HttpContext.Current.Request.QueryString(name)
		Else
			Return ""
		End If
	End Function

	' Get form value
	Public Shared Function ew_Post(name As String) As String
		If HttpContext.Current.Request.Form(name) IsNot Nothing Then
			Return HttpContext.Current.Request.Form(name)
		Else
			Return ""
		End If
	End Function

	' Get/set session values
	Public Shared Property ew_Session(name As String) As Object
		Get
			Return HttpContext.Current.Session(name)
		End Get
		Set(ByVal Value As Object)
			HttpContext.Current.Session(name) = Value
		End Set
	End Property

	' Get project cookie
	Public Shared Property ew_Cookie(name As String) As String
		Get
			If HttpContext.Current.Request.Cookies(EW_PROJECT_NAME) IsNot Nothing Then
				Return HttpContext.Current.Request.Cookies(EW_PROJECT_NAME)(name)
			Else
				Return ""
			End If
		End Get
		Set(ByVal value As String)
			Dim c As HttpCookie
			If HttpContext.Current.Request.Cookies(EW_PROJECT_NAME) IsNot Nothing Then
				c = HttpContext.Current.Request.Cookies(EW_PROJECT_NAME)
				c.Values(name) = value
			Else
				c = New HttpCookie(EW_PROJECT_NAME)
			End If
			c.Values(name) = value
			c.Expires = DateAdd("d", 365, Today()) ' Change the expiry date of the cookies here
			HttpContext.Current.Response.Cookies.Add(c)	
		End Set
	End Property

	' Response.Write
	Public Shared Sub ew_Write(value As Object)
		HttpContext.Current.Response.Write(value)
	End Sub

	' Response.End
	Public Shared Sub ew_End()
		HttpContext.Current.Response.End()
	End Sub
End Class
