Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class _register
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public register As cregister

	'
	' Page Class
	'
	Class cregister
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
			m_PageID = "register"
			m_PageObjName = "register"
			m_PageObjTypeName = "cregister"

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

	'
	' Page main processing
	'
	Sub Page_Main()
		Dim bUserExists As Boolean
		Dim sSql As String
		Dim RsNew As ArrayList

		' Create form object
		ObjForm = New cFormObj
		If ObjForm.GetValue("a_register") <> "" Then

			' Get action
			Contact.CurrentAction = ObjForm.GetValue("a_register")
			LoadFormValues() ' Get form values

			' Validate Form
			If Not ValidateForm() Then
				Contact.CurrentAction = "I" ' Form error, reset action
				Message = ParentPage.gsFormError
			End If
		Else
			Contact.CurrentAction = "I" ' Display blank record
			LoadDefaultValues() ' Load default values
		End If

		' Handle email activation
		If ew_Get("action") <> "" Then
			Dim sAction As String = ew_Get("action")
			Dim sEmail As String = ew_Get("email")
			Dim sApprovalCode As String = ew_Get("code")
			If sEmail = cTEA.Decrypt(sApprovalCode, EW_RANDOM_KEY) Then
				If sAction.ToLower() = "confirm" Then ' Email activation
					If ActivateEmail(sEmail) Then ' Activate this email
						Message = "Your account is activated" ' Set message acount activated
					Page_Terminate("login.aspx") ' Go to login page
					End If
				End If
			End If
			If Message = "" Then Message = "Activation failed" ' Set activate failed message
			Page_Terminate("login.aspx") ' Go to login page
		End If
		Select Case Contact.CurrentAction
			Case "I" ' Blank record, no action required
			Case "A" ' Add

				' Check for Duplicate User ID				
				Dim sFilter As String = "([EMail] = '" & ew_AdjustSql(Contact.zEMail.CurrentValue) & "')"

				' Get SQL from GetSql function in Contact class, Contactinfo.aspx
				Dim sUserSql As String = Contact.GetSQL(sFilter, "")
				Dim RsChk As OleDbDataReader = Conn.GetDataReader(sUserSql)
				If RsChk.Read() Then
					bUserExists = True
					RestoreFormValues() ' Restore form values
					Message = "User already exists" ' Set user exist message
				End If
				RsChk.Close()
				RsChk.Dispose()
				If Not bUserExists Then
					Contact.SendEmail = True ' Send email on add success
					If AddRow() Then ' Add record

						' Load Registrant Email
						Dim sBccEmail As String
						Dim sReceiverEmail As String = Contact.zEMail.CurrentValue
						If sReceiverEmail = "" Then ' Send to recipient directly
							sReceiverEmail = EW_RECIPIENT_EMAIL
							sBccEmail = ""
						Else ' Bcc recipient
							sBccEmail = EW_RECIPIENT_EMAIL
						End If

						' Create Email Object
						Dim Email As New cEmail
						Email.Load(HttpContext.Current.Server.MapPath("txt/register.txt"))
						Email.ReplaceSender(EW_SENDER_EMAIL) ' Replace Sender
						Email.ReplaceRecipient(sReceiverEmail) ' Replace Recipient
						If sBccEmail <> "" Then Email.AddBcc(sBccEmail) ' Add Bcc

						' Set up email content
						Email.ReplaceContent("<!--LogonPassword-->", Convert.ToString(Contact.LogonPassword.CurrentValue))
						Email.ReplaceContent("<!--PrimaryContact-->", Convert.ToString(Contact.PrimaryContact.CurrentValue))
						Email.ReplaceContent("<!--EMail-->", Convert.ToString(Contact.zEMail.CurrentValue))
						Dim sActivateLink As String
						sActivateLink = ew_FullUrl() & "?action=confirm"
						sActivateLink = sActivateLink & "&email=" & Contact.zEMail.CurrentValue
						sActivateLink = sActivateLink & "&code=" & cTEA.Encrypt(Contact.zEMail.CurrentValue, EW_RANDOM_KEY)
						Email.ReplaceContent("<!--ActivateLink-->", sActivateLink)

						' Get new record
						Contact.CurrentFilter = Contact.KeyFilter
						sSql = Contact.SQL
						RsNew = Conn.GetRows(sSql)
						Dim EventArgs As New Hashtable
						If RsNew.Count > 0 Then EventArgs.Add("Rs", RsNew(0)) ' Argument is OrderedDictionary
						If Email_Sending(Email, EventArgs) Then
							Email.Send()
						End If
						Message = "Registration succeeded. An email has been sent to your email address, please click the link in the email to activate your account." ' Activate success
					Page_Terminate("login.aspx") ' Return
					Else
						RestoreFormValues() ' Restore form values
					End If
				End If
		End Select

		' Render row
		If Contact.CurrentAction = "F" Then ' Confirm page
			Contact.RowType = EW_ROWTYPE_VIEW ' Render view
		Else
			Contact.RowType = EW_ROWTYPE_ADD ' Render add
		End If

		' Render row
		RenderRow()
	End Sub

	'
	' Activate account based on email
	'
	Function ActivateEmail(email As String) As Boolean	 
		Dim sFilter As String = "([EMail] = '" & ew_AdjustSql(email) & "')"
		Dim sSql As String = Contact.GetSQL(sFilter, "")
		Try
			Dim RsValidate As OleDbDataReader = Conn.GetTempDataReader(sSql)
			If RsValidate.Read() Then
				Dim RsAct As New OrderedDictionary
				RsAct("Active") = True ' Auto register
				Contact.CurrentFilter = sFilter
				Contact.Update(RsAct)
				Return True
			Else
				Message = "No records found"
				Return False
			End If
		Catch e As Exception
			If EW_DEBUG_ENABLED Then Throw
			Message = e.Message			
			Return False
		Finally		
			Conn.CloseTempDataReader()
		End Try
	End Function

	'
	' Get upload file
	'
	Sub GetUploadFiles()

		' Get upload data
	End Sub

	'
	' Load default values
	'
	Sub LoadDefaultValues()
	End Sub

	'
	' Load form values
	'
	Sub LoadFormValues()
		Contact.LogonPassword.FormValue = ObjForm.GetValue("x_LogonPassword")
		Contact.LogonPassword.ConfirmValue = ObjForm.GetValue("c_LogonPassword")
		Contact.LogonPassword.OldValue = ObjForm.GetValue("o_LogonPassword")
		Contact.PrimaryContact.FormValue = ObjForm.GetValue("x_PrimaryContact")
		Contact.PrimaryContact.OldValue = ObjForm.GetValue("o_PrimaryContact")
		Contact.zEMail.FormValue = ObjForm.GetValue("x_zEMail")
		Contact.zEMail.OldValue = ObjForm.GetValue("o_zEMail")
		Contact.ContactID.FormValue = ObjForm.GetValue("x_ContactID")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		Contact.LogonPassword.CurrentValue = Contact.LogonPassword.FormValue
		Contact.PrimaryContact.CurrentValue = Contact.PrimaryContact.FormValue
		Contact.zEMail.CurrentValue = Contact.zEMail.FormValue
		Contact.ContactID.CurrentValue = Contact.ContactID.FormValue
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		Contact.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' LogonPassword

		Contact.LogonPassword.CellCssStyle = ""
		Contact.LogonPassword.CellCssClass = ""

		' PrimaryContact
		Contact.PrimaryContact.CellCssStyle = ""
		Contact.PrimaryContact.CellCssClass = ""

		' EMail
		Contact.zEMail.CellCssStyle = ""
		Contact.zEMail.CellCssClass = ""

		'
		'  View  Row
		'

		If Contact.RowType = EW_ROWTYPE_VIEW Then ' View row

			' LogonName
			Contact.LogonName.ViewValue = Contact.LogonName.CurrentValue
			Contact.LogonName.CssStyle = ""
			Contact.LogonName.CssClass = ""
			Contact.LogonName.ViewCustomAttributes = ""

			' LogonPassword
			Contact.LogonPassword.ViewValue = "********"
			Contact.LogonPassword.CssStyle = ""
			Contact.LogonPassword.CssClass = ""
			Contact.LogonPassword.ViewCustomAttributes = ""

			' PrimaryContact
			Contact.PrimaryContact.ViewValue = Contact.PrimaryContact.CurrentValue
			Contact.PrimaryContact.CssStyle = ""
			Contact.PrimaryContact.CssClass = ""
			Contact.PrimaryContact.ViewCustomAttributes = ""

			' EMail
			Contact.zEMail.ViewValue = Contact.zEMail.CurrentValue
			Contact.zEMail.CssStyle = ""
			Contact.zEMail.CssClass = ""
			Contact.zEMail.ViewCustomAttributes = ""

			' Active
			If Convert.ToString(Contact.Active.CurrentValue) = "1" Then
				Contact.Active.ViewValue = "Yes"
			Else
				Contact.Active.ViewValue = "No"
			End If
			Contact.Active.CssStyle = ""
			Contact.Active.CssClass = ""
			Contact.Active.ViewCustomAttributes = ""

			' CompanyID
			If ew_NotEmpty(Contact.CompanyID.CurrentValue) Then
				sSqlWrk = "SELECT [CompanyName] FROM [Company] WHERE [CompanyID] = " & ew_AdjustSql(Contact.CompanyID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Contact.CompanyID.ViewValue = RsWrk("CompanyName")
				Else
					Contact.CompanyID.ViewValue = Contact.CompanyID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Contact.CompanyID.ViewValue = System.DBNull.Value
			End If
			Contact.CompanyID.CssStyle = ""
			Contact.CompanyID.CssClass = ""
			Contact.CompanyID.ViewCustomAttributes = ""

			' GroupID
			If ew_NotEmpty(Contact.GroupID.CurrentValue) Then
				sSqlWrk = "SELECT [GroupName] FROM [Group] WHERE [GroupID] = " & ew_AdjustSql(Contact.GroupID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [GroupID] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Contact.GroupID.ViewValue = RsWrk("GroupName")
				Else
					Contact.GroupID.ViewValue = Contact.GroupID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Contact.GroupID.ViewValue = System.DBNull.Value
			End If
			Contact.GroupID.CssStyle = ""
			Contact.GroupID.CssClass = ""
			Contact.GroupID.ViewCustomAttributes = ""

			' View refer script
			' LogonPassword

			Contact.LogonPassword.HrefValue = ""

			' PrimaryContact
			Contact.PrimaryContact.HrefValue = ""

			' EMail
			Contact.zEMail.HrefValue = ""

		'
		'  Add Row
		'

		ElseIf Contact.RowType = EW_ROWTYPE_ADD Then ' Add row

			' LogonPassword
			Contact.LogonPassword.EditCustomAttributes = ""
			Contact.LogonPassword.EditValue = ew_HtmlEncode(Contact.LogonPassword.CurrentValue)

			' PrimaryContact
			Contact.PrimaryContact.EditCustomAttributes = ""
			Contact.PrimaryContact.EditValue = ew_HtmlEncode(Contact.PrimaryContact.CurrentValue)

			' EMail
			Contact.zEMail.EditCustomAttributes = ""
			Contact.zEMail.EditValue = ew_HtmlEncode(Contact.zEMail.CurrentValue)
		End If

		' Row Rendered event
		Contact.Row_Rendered()
	End Sub

	'
	' Validate form
	'
	Function ValidateForm() As Boolean

		' Initialize
		ParentPage.gsFormError = ""

		' Check if validation required
		If Not EW_SERVER_VALIDATE Then Return (ParentPage.gsFormError = "")
		If ew_Empty(Contact.LogonPassword.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter required field - Logon Password"
		End If
		If Contact.LogonPassword.FormValue = "" Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter password"
		End If
		If Contact.LogonPassword.ConfirmValue <> Contact.LogonPassword.FormValue Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Mismatch Password"
		End If
		If Not ew_CheckEmail(Contact.zEMail.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Incorrect email - EMail"
		End If

		' Return validate result
		Dim Valid As Boolean = (ParentPage.gsFormError = "")

		' Form_CustomValidate event
		Dim sFormCustomError As String = ""
		Valid = Valid And Form_CustomValidate(sFormCustomError)
		If sFormCustomError <> "" Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & sFormCustomError
		End If
		Return Valid
	End Function

	'
	' Add record
	'
	Function AddRow() As Boolean
		Dim Rs As New OrderedDictionary
		Dim sSql As String, sFilter As String
		Dim bInsertRow As Boolean
		Dim RsChk As OleDbDataReader
		Dim sIdxErrMsg As String
		Dim LastInsertId As Object
		If ew_NotEmpty(Contact.LogonName.CurrentValue) Then ' Check field with unique index
			sFilter = "([LogonName] = '" & ew_AdjustSql(Contact.LogonName.CurrentValue) & "')"
			RsChk = Contact.LoadRs(sFilter)
			If RsChk IsNot Nothing Then
				sIdxErrMsg = "Duplicate value '%v' for unique index '%f'".Replace("%f", "LogonName")
				sIdxErrMsg = sIdxErrMsg.Replace("%v", Contact.LogonName.CurrentValue)
				Message = sIdxErrMsg
				RsChk.Close()
				RsChk.Dispose()
				Return False
			End If
		End If

		' LogonPassword
		Contact.LogonPassword.SetDbValue(Contact.LogonPassword.CurrentValue, System.DBNull.Value)
		Rs("LogonPassword") = Contact.LogonPassword.DbValue

		' PrimaryContact
		Contact.PrimaryContact.SetDbValue(Contact.PrimaryContact.CurrentValue, System.DBNull.Value)
		Rs("PrimaryContact") = Contact.PrimaryContact.DbValue

		' EMail
		Contact.zEMail.SetDbValue(Contact.zEMail.CurrentValue, System.DBNull.Value)
		Rs("EMail") = Contact.zEMail.DbValue

		' CompanyID
		' Row Inserting event

		bInsertRow = Contact.Row_Inserting(Rs)
		If bInsertRow Then
			Try	
				Contact.Insert(Rs)
				AddRow = True
			Catch e As Exception
				If EW_DEBUG_ENABLED Then Throw
				Message = e.Message				
				AddRow = False
			End Try
		Else
			If Contact.CancelMessage <> "" Then
				Message = Contact.CancelMessage
				Contact.CancelMessage = ""
			Else
				Message = "Insert cancelled"
			End If
			AddRow = False
		End If
		If AddRow Then
			LastInsertId = Conn.GetLastInsertId()
			Contact.ContactID.DbValue = LastInsertId
			Rs("ContactID") = Contact.ContactID.DbValue		

			' Row Inserted event
			Contact.Row_Inserted(Rs)
			WriteAuditTrailOnAdd(Rs)
		End If
	End Function

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "Contact"
		Dim filePfx As String = "log"
		Dim curDate As String, curTime As String, id As String, user As String, action As String
		Dim i As Integer
		Dim dt As DateTime = Now()		
		curDate = dt.ToString("yyyy/MM/dd")
		curTime = dt.ToString("HH:mm:ss")
		id = HttpContext.Current.Request.ServerVariables("SCRIPT_NAME")
    user = CurrentUserID()
		action = typ
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "", "", "", "")
	End Sub

	' Write Audit Trail (add page)
	Sub WriteAuditTrailOnAdd(ByRef RsSrc As OrderedDictionary)
		Dim table As String = "Contact"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsSrc("ContactID")

		' Write Audit Trail
		Dim filePfx As String = "log"
		Dim action As String = "A"
		Dim oldvalue As Object = ""
		Dim curDate As String, curTime As String, id As String, user As String, field As String
		Dim keyvalue As Object
		Dim dt As DateTime = Now()
		curDate = dt.ToString("yyyy/MM/dd")
		curTime = dt.ToString("HH:mm:ss")
		id = HttpContext.Current.Request.ServerVariables("SCRIPT_NAME")
    user = CurrentUserID()
		keyvalue = sKey

		' LogonPassword Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "LogonPassword", keyvalue, oldvalue, RsSrc("LogonPassword"))

		' PrimaryContact Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "PrimaryContact", keyvalue, oldvalue, RsSrc("PrimaryContact"))

		' EMail Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "EMail", keyvalue, oldvalue, RsSrc("EMail"))
	End Sub

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
		register = New cregister(Me)		
		register.Page_Init()

		' Page main processing
		register.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If register IsNot Nothing Then register.Dispose()
	End Sub
End Class
