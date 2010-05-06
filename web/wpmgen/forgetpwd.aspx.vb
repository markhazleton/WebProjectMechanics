Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class _forgetpwd
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public forgetpwd As cforgetpwd

	'
	' Page Class
	'
	Class cforgetpwd
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
			m_PageID = "forgetpwd"
			m_PageObjName = "forgetpwd"
			m_PageObjTypeName = "cforgetpwd"

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

	Public sEmail As String

	'
	' Page main processing
	'
	Sub Page_Main()
		Dim bEmailSent As Boolean
		Dim sUserName As String, sPassword As String
		Dim RsPwd As OrderedDictionary		
		If HttpContext.Current.Request.RequestType = "POST" Then
			Dim bValidEmail As Boolean = False

			' Setup variables
			sEmail = ew_Post("email")
			If ValidateForm(sEmail) Then

				' Set up filter (SQL WHERE clause)
				' SQL constructor in Contact class, Contactinfo.aspx

				Dim sFilter As String = "[EMail] = '" & ew_AdjustSql(sEmail) & "'"
				Contact.CurrentFilter = sFilter
				Dim sSql As String = Contact.SQL
				Dim RsUser As OleDbDataReader = Conn.GetTempDataReader(sSql)
				If RsUser.Read() Then					
					sUserName = Convert.ToString(RsUser("EMail"))
					sPassword = Convert.ToString(RsUser("LogonPassword"))
					If EW_MD5_PASSWORD Then
						RsPwd = New OrderedDictionary												
						RsPwd.Add("LogonPassword", sPassword) ' Reset password
						Contact.Update(RsPwd)						
					End If
					bValidEmail = True
				Else
					Message = "Invalid Email"
				End If
				If bValidEmail Then
					Dim Email As New cEmail
					Email.Load(HttpContext.Current.Server.MapPath("txt/forgetpwd.txt"))
					Email.ReplaceSender(EW_SENDER_EMAIL) ' Replace Sender
					Email.ReplaceRecipient(sEmail) ' Replace Recipient
					Email.ReplaceContent("<!--$UserName-->", sUserName)
					Email.ReplaceContent("<!--$Password-->", sPassword)
					RsPwd = Conn.GetRow(RsUser) ' Get the whole row
					If EW_MD5_PASSWORD Then RsPwd("LogonPassword") = MD5(sPassword) ' Update the password
					Dim EventArgs As New Hashtable					
					EventArgs.Add("Rs", RsPwd)
					If Email_Sending(Email, EventArgs) Then
						bEmailSent = Email.Send()
					Else
						bEmailSent = False
					End If
				Else
					bEmailSent = False
				End If
				Conn.CloseTempDataReader()
				If bEmailSent Then
					Message = "Password sent to your email" ' Set success message
					Page_Terminate("login.aspx") ' Return to login page
				ElseIf bValidEmail Then
					Message = "Failed to send mail" ' Set up error message
				End If
			Else
				Message = ParentPage.gsFormError
			End If
		End If
	End Sub

	'
	' Validate form
	'
	Function ValidateForm(email As String) As Boolean

		' Initialize
		ParentPage.gsFormError = ""

		' Check if validation required
		If Not EW_SERVER_VALIDATE Then Return True ' Skip
		If email = "" Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter valid Email Address!"
		End If
		If Not ew_CheckEmail(email) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter valid Email Address!"
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
		forgetpwd = New cforgetpwd(Me)		
		forgetpwd.Page_Init()

		' Page main processing
		forgetpwd.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If forgetpwd IsNot Nothing Then forgetpwd.Dispose()
	End Sub
End Class
