Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class _changepwd
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public changepwd As cchangepwd

	'
	' Page Class
	'
	Class cchangepwd
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
			m_PageID = "changepwd"
			m_PageObjName = "changepwd"
			m_PageObjTypeName = "cchangepwd"

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
			If Not Security.IsLoggedIn() Then Call Security.AutoLogin()
			If Not Security.IsLoggedIn() Or Security.IsSysAdmin() Then Call Page_Terminate("login.aspx")
			Call Security.LoadCurrentUserLevel("Contact")

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

	'
	' Page main processing
	'
	Sub Page_Main()
		If HttpContext.Current.Request.RequestType = "POST" Then
			Dim bPwdUpdated As Boolean = False
			Dim bValidPwd As Boolean
			Dim sEmail As String = ""

			' Setup variables
			Dim sUsername As String = Security.CurrentUserName()
			Dim sOPwd As String = ew_Post("opwd")
			Dim sNPwd As String = ew_Post("npwd")
			Dim sCPwd As String = ew_Post("cpwd")
			If ValidateForm(sOPwd, sNPwd, sCPwd) Then
				Dim sFilter As String = "(EMail = '" & ew_AdjustSql(sUsername) & "')"

				' Set up filter (SQL WHERE clause)
				' SQL constructor in Contact class, Contactinfo.aspx

				Contact.CurrentFilter = sFilter
				Dim sSql As String = Contact.SQL
				Dim RsUser As OleDbDataReader = Conn.GetTempDataReader(sSql)
				Dim RsPwd As New OrderedDictionary
				If RsUser.Read() Then
					If EW_CASE_SENSITIVE_PASSWORD Then
						If EW_MD5_PASSWORD Then
							bValidPwd = ew_SameStr(MD5(sOPwd), RsUser("LogonPassword"))
						Else
							bValidPwd = ew_SameStr(sOPwd, RsUser("LogonPassword"))
						End If
					Else
						If EW_MD5_PASSWORD Then
							bValidPwd = ew_SameStr(MD5(sOPwd.ToLower()), RsUser("LogonPassword"))
						Else
							bValidPwd = ew_SameText(sOPwd.ToLower(), RsUser("LogonPassword"))
						End If
					End If
					If bValidPwd Then
						If Not EW_CASE_SENSITIVE_PASSWORD Then sNPwd = sNPwd.ToLower()						
						RsPwd.Add("LogonPassword", sNPwd) ' Change Password
						sEmail = Convert.ToString(RsUser("EMail"))
						Contact.Update(RsPwd)
						bPwdUpdated = True
					Else
						Message = "Invalid Password"
					End If
				End If
				If bPwdUpdated Then

					' Send Email
					Dim Email As New cEmail
					Email.Load(HttpContext.Current.Server.MapPath("txt/changepwd.txt"))
					Email.ReplaceSender(EW_SENDER_EMAIL) ' Replace Sender
					Email.ReplaceRecipient(sEmail) ' Replace Recipient
					Email.ReplaceContent("<!--$Password-->", sNPwd)
					RsPwd = Conn.GetRow(RsUser) ' Get the whole row
					RsPwd("LogonPassword") = sNPwd ' Provide the new readable password for email sending
					Dim EventArgs As New Hashtable
					EventArgs.Add("Rs", RsPwd)
					If Email_Sending(Email, EventArgs) Then
						Email.Send()
					End If
					Message = "Password Changed" ' Set up message
				Page_Terminate("default.aspx") ' Exit page and clean up
				End If
				Conn.CloseTempDataReader()
			Else
				Message = ParentPage.gsFormError
			End If
		End If
	End Sub

	'
	' Validate form
	'
	Function ValidateForm(opwd As String, npwd As String, cpwd As String) As Boolean

		' Initialize
		ParentPage.gsFormError = ""

		' Check if validation required
		If Not EW_SERVER_VALIDATE Then Return True
		If opwd = "" Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter old password"
		End If
		If npwd = "" Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter new password"
		End If
		If npwd <> cpwd Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Mismatch Password"
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

	' Email Sending event
	Public Function Email_Sending(ByRef Email As cEmail, Args As Hashtable) As Boolean

		'HttpContext.Current.Response.Write(Email.AsString())
		'HttpContext.Current.Response.End()

		Return True
	End Function

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
		changepwd = New cchangepwd(Me)		
		changepwd.Page_Init()

		' Page main processing
		changepwd.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If changepwd IsNot Nothing Then changepwd.Dispose()
	End Sub
End Class
