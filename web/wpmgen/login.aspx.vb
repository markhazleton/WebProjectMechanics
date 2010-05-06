Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class _login
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public login As clogin

	'
	' Page Class
	'
	Class clogin
		Inherits AspNetMakerPage
		Implements IDisposable		

		' Used by system generated functions
		Private RsWrk As Object, sSqlWrk As String, sWhereWrk As String

		Private arwrk As Object

		Private armultiwrk() As String		

		' Page URL
		Public ReadOnly Property PageUrl() As String
			Get
				Dim Url As String = ew_CurrentPage() & "?"
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Return True			
		End Function	

		' Contact
		Public Property Contact() As cContact
			Get				
				Return ParentPage.Contact
			End Get
			Set(ByVal v As cContact)
				ParentPage.Contact = v	
			End Set	
		End Property	

		'
		'  Constructor
		'  - init objects
		'  - open connection
		'
		Public Sub New(ByRef APage As AspNetMaker7_WPMGen)				
			m_ParentPage = APage
			m_Page = Me	
			m_PageID = "login"
			m_PageObjName = "login"
			m_PageObjTypeName = "clogin"

			' Initialize table object
			Contact = New cContact(Me)

			' Connect to database
			Conn = New cConnection()
		End Sub

		'
		'  Subroutine Page_Init
		'  - called before page main
		'  - check Security
		'  - set up response header
		'  - call page load events
		'
		Public Sub Page_Init()
			Security = New cAdvancedSecurity(Me)

			' Global page loading event (in ewglobal*.vb)
			ParentPage.Page_Loading()

			' Page load event, used in current page
			Page_Load()
		End Sub

		'
		'  Class terminate
		'  - clean up page object
		'
		Public Sub Dispose() Implements IDisposable.Dispose
			Page_Terminate("")
		End Sub

		'
		'  Sub Page_Terminate
		'  - called when exit page
		'  - clean up connection and objects
		'  - if URL specified, redirect to URL
		'
		Sub Page_Terminate(url As String)

			' Page unload event, used in current page
			Page_Unload()

			' Global page unloaded event (in ewglobal*.vb)
			ParentPage.Page_Unloaded()

			' Close connection
			Conn.Dispose()
			Security = Nothing
			Contact.Dispose()

			' Go to URL if specified
			If url <> "" Then
				HttpContext.Current.Response.Clear()
				HttpContext.Current.Response.Redirect(url)
			End If
		End Sub

	Public sUsername As String

	Public sLoginType As String

	'
	' Page main processing
	'
	Sub Page_Main()
		Dim bValidate As Boolean, bValidPwd As Boolean
		Dim sPassword As String
		Dim sLastUrl As String = Security.LastUrl ' Get last URL
		If ew_Empty(sLastUrl) Then sLastUrl = "default.aspx"
		If Not Security.IsLoggedIn() Then Security.AutoLogin()
		If HttpContext.Current.Request.RequestType = "POST" Then

			' Setup variables
			sUsername = ew_Post("Username")
			sPassword = ew_Post("Password")
			sLoginType = ew_Post("rememberme").ToLower()
			bValidate = ValidateForm(sUsername, sPassword)
			If Not bValidate Then
				Message = ParentPage.gsFormError
			End If
		Else
			If Security.IsLoggedIn() Then
				If Message = "" Then Page_Terminate(sLastUrl) ' Return to last accessed page
			End If
			bValidate = False

			' Restore settings
			sUsername = ew_Cookie("username")
			If ew_Cookie("autologin") = "autologin" Then
				sLoginType = "a"
			ElseIf ew_Cookie("autologin") = "rememberusername" Then
				sLoginType = "u"
			Else
				sLoginType = ""
			End If
		End If
		If bValidate Then
			bValidPwd = False

			' loggin in event
			bValidate = User_LoggingIn(sUsername, sPassword)
			If bValidate Then
				bValidPwd = Security.ValidateUser(sUsername, sPassword)
				If Not bValidPwd Then Message = "Incorrect user ID or password" ' Invalid user id/password
			Else
				If Message = "" Then Message = "Login cancelled" ' Login cancelled
			End If

			' Write cookies
			If bValidPwd Then
				If sLoginType = "a" Then ' Auto login
					ew_Cookie("autologin") = "autologin" ' Set up autologin cookies
					ew_Cookie("username") = sUsername ' Set up user name cookies
					ew_Cookie("password") = cTEA.Encrypt(sPassword, EW_RANDOM_KEY) ' Set up password cookies
					HttpContext.Current.Response.Cookies(EW_PROJECT_NAME).Expires = DateAdd("d", 365, Today()) ' Change the expiry date of the cookies here
				ElseIf sLoginType = "u" Then ' Remember user name
					ew_Cookie("autologin") = "rememberusername" ' Set up remember user name cookies
					ew_Cookie("username") = sUsername ' Set up user name cookies
					HttpContext.Current.Response.Cookies(EW_PROJECT_NAME).Expires = DateAdd("d", 365, Today()) ' Change the expiry date of the cookies here
				Else
					ew_Cookie("autologin") = "" ' Clear autologin cookies
				End If

				' User_LoggedIn event
				User_LoggedIn(sUsername)
				ew_WriteAuditTrailOnLogInOut("login", sUsername)
			Page_Terminate(sLastUrl) ' Return to last accessed url
			Else

				' user login error event
				User_LoginError(sUsername, sPassword)
			End If
		End If
	End Sub

	'
	' Validate form
	'
	Function ValidateForm(usr As String, pwd As String) As Boolean

		' Initialize
		ParentPage.gsFormError = ""

		' Check if validation required
		If Not EW_SERVER_VALIDATE Then Return True ' Skip
		If usr = "" Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter user ID"
		End If
		If pwd = "" Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter password"
		End If

		' Return validate result
		ValidateForm = (ParentPage.gsFormError = "")

		' Form_CustomValidate event
		Dim sFormCustomError As String = ""
		ValidateForm = ValidateForm And Form_CustomValidate(sFormCustomError)
		If sFormCustomError <> "" Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & sFormCustomError
		End If
	End Function

		' Page Load event
		Public Sub Page_Load()

			'HttpContext.Current.Response.Write("Page Load")
		End Sub

		' Page Unload event
		Public Sub Page_Unload()

			'HttpContext.Current.Response.Write("Page Unload")
		End Sub

	' User Logging In event
	Public Function User_LoggingIn(usr As String, pwd As String) As Boolean

		' Enter your code here
		' To cancel, set return value to False

		Return True
	End Function

	' User Logged In event
	Public Sub User_LoggedIn(usr As String)

		'HttpContext.Current.Response.Write("User Logged In")
	End Sub

	' User Login Error event
	Public Sub User_LoginError(usr As String, pwd As String)

		'HttpContext.Current.Response.Write("User Login Error")
	End Sub

	' Form Custom Validate event
	Public Function Form_CustomValidate(ByRef CustomError As String) As Boolean

		'Return error message in CustomError
		Return True
	End Function
	End Class

	'
	' ASP.NET Page_Load event
	'

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

		' Page init
		login = New clogin(Me)		
		login.Page_Init()

		' Page main processing
		login.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If login IsNot Nothing Then login.Dispose()
	End Sub
End Class
