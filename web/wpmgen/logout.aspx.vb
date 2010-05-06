Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class _logout
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public logout As clogout

	'
	' Page Class
	'
	Class clogout
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

		'
		'  Constructor
		'  - init objects
		'  - open connection
		'
		Public Sub New(ByRef APage As AspNetMaker7_WPMGen)				
			m_ParentPage = APage
			m_Page = Me	
			m_PageID = "logout"
			m_PageObjName = "logout"
			m_PageObjTypeName = "clogout"

			' Initialize table object
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

			' Go to URL if specified
			If url <> "" Then
				HttpContext.Current.Response.Clear()
				HttpContext.Current.Response.Redirect(url)
			End If
		End Sub

	'
	' Page main processing
	'
	Sub Page_Main()
		Dim bValidate As Boolean = True
		Dim sLastUrl As String, sUsername As String
		sUsername = Security.CurrentUserName()

		' User LoggingOut event
		bValidate = User_LoggingOut(sUsername)
		If Not bValidate Then
			sLastUrl = Security.LastUrl
			If sLastUrl = "" Then sLastUrl = "default.aspx"
		Page_Terminate(sLastUrl) ' Go to last accessed URL
		Else
			If HttpContext.Current.Request.Cookies(EW_PROJECT_NAME) IsNot Nothing Then
				ew_Cookie("password") = "" ' Clear password
				ew_Cookie("lasturl") = "" ' Clear last URL
				If ew_Cookie("autologin") = "" Then ' Not auto login					
					ew_Cookie("username") = "" ' Clear user name				
				End If
			End If

			' Clear session
			HttpContext.Current.Session.Abandon()

			' User_LoggedOut event
			User_LoggedOut(sUsername)
			ew_WriteAuditTrailOnLogInOut("logout", sUsername)
		Page_Terminate("login.aspx") ' Go to login page
		End If
	End Sub

		' Page Load event
		Public Sub Page_Load()

			'HttpContext.Current.Response.Write("Page Load")
		End Sub

		' Page Unload event
		Public Sub Page_Unload()

			'HttpContext.Current.Response.Write("Page Unload")
		End Sub

	' User Logging Out event
	Public Function User_LoggingOut(usr As String) As Boolean

		' Enter your code here
		' To cancel, set return value to False

		Return True
	End Function

	' User Logged Out event
	Public Sub User_LoggedOut(usr As String)

		'HttpContext.Current.Response.Write("User Logged Out")
	End Sub
	End Class

	'
	' ASP.NET Page_Load event
	'

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

		' Page init
		logout = New clogout(Me)		
		logout.Page_Init()

		' Page main processing
		logout.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If logout IsNot Nothing Then logout.Dispose()
	End Sub
End Class
