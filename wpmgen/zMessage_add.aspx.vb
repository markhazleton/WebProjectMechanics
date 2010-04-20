Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class zMessage_add
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public zMessage_add As czMessage_add

	'
	' Page Class
	'
	Class czMessage_add
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
				If zMessage.UseTokenInUrl Then Url = Url & "t=" & zMessage.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If zMessage.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (zMessage.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (zMessage.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' zMessage
		Public Property zMessage() As czMessage
			Get				
				Return ParentPage.zMessage
			End Get
			Set(ByVal v As czMessage)
				ParentPage.zMessage = v	
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
			m_PageID = "add"
			m_PageObjName = "zMessage_add"
			m_PageObjTypeName = "czMessage_add"

			' Table Name
			m_TableName = "Message"

			' Initialize table object
			zMessage = New czMessage(Me)

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
			zMessage.Dispose()

			' Go to URL if specified
			If url <> "" Then
				HttpContext.Current.Response.Clear()
				HttpContext.Current.Response.Redirect(url)
			End If
		End Sub

	Public x_ewPriv As Integer

	'
	' Page main processing
	'
	Sub Page_Main()

		' Load key values from QueryString
		Dim bCopy As Boolean = True
		If ew_Get("MessageID") <> "" Then
			zMessage.MessageID.QueryStringValue = ew_Get("MessageID")
		Else
			bCopy = False
		End If

		' Create form object
		ObjForm = New cFormObj

		' Process form if post back
		If ObjForm.GetValue("a_add") <> "" Then
			zMessage.CurrentAction = ObjForm.GetValue("a_add") ' Get form action
			LoadFormValues() ' Load form values

			' Validate Form
			If Not ValidateForm() Then
				zMessage.CurrentAction = "I" ' Form error, reset action
				Message = ParentPage.gsFormError
			End If

		' Not post back
		Else
			If bCopy Then
				zMessage.CurrentAction = "C" ' Copy Record
			Else
				zMessage.CurrentAction = "I" ' Display Blank Record
				LoadDefaultValues() ' Load default values
			End If
		End If

		' Perform action based on action code
		Select Case zMessage.CurrentAction
			Case "I" ' Blank record, no action required
			Case "C" ' Copy an existing record
				If Not LoadRow() Then ' Load record based on key
					Message = "No records found" ' No record found
					Page_Terminate("zMessage_list.aspx") ' No matching record, return to list
				End If
			Case "A" ' Add new record
				zMessage.SendEmail = True ' Send email on add success
				If AddRow() Then ' Add successful
					Message = "Add succeeded" ' Set up success message
					Dim sReturnUrl As String = zMessage.ReturnUrl
					If ew_GetPageName(sReturnUrl) = "zMessage_view.aspx" Then sReturnUrl = zMessage.ViewUrl ' View paging, return to view page with keyurl directly
					Page_Terminate(sReturnUrl) ' Clean up and return
				Else
					RestoreFormValues() ' Add failed, restore form values
				End If
		End Select

		' Render row based on row type
		zMessage.RowType = EW_ROWTYPE_ADD ' Render add type

		' Render row
		RenderRow()
	End Sub

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
		zMessage.zPageID.CurrentValue = 0
		zMessage.ParentMessageID.CurrentValue = 0
		zMessage.MessageDate.CurrentValue = Now()
	End Sub

	'
	' Load form values
	'
	Sub LoadFormValues()
		zMessage.zPageID.FormValue = ObjForm.GetValue("x_zPageID")
		zMessage.zPageID.OldValue = ObjForm.GetValue("o_zPageID")
		zMessage.ParentMessageID.FormValue = ObjForm.GetValue("x_ParentMessageID")
		zMessage.ParentMessageID.OldValue = ObjForm.GetValue("o_ParentMessageID")
		zMessage.Subject.FormValue = ObjForm.GetValue("x_Subject")
		zMessage.Subject.OldValue = ObjForm.GetValue("o_Subject")
		zMessage.Author.FormValue = ObjForm.GetValue("x_Author")
		zMessage.Author.OldValue = ObjForm.GetValue("o_Author")
		zMessage.zEmail.FormValue = ObjForm.GetValue("x_zEmail")
		zMessage.zEmail.OldValue = ObjForm.GetValue("o_zEmail")
		zMessage.City.FormValue = ObjForm.GetValue("x_City")
		zMessage.City.OldValue = ObjForm.GetValue("o_City")
		zMessage.URL.FormValue = ObjForm.GetValue("x_URL")
		zMessage.URL.OldValue = ObjForm.GetValue("o_URL")
		zMessage.MessageDate.FormValue = ObjForm.GetValue("x_MessageDate")
		zMessage.MessageDate.CurrentValue = ew_UnFormatDateTime(zMessage.MessageDate.CurrentValue, 6)
		zMessage.MessageDate.OldValue = ObjForm.GetValue("o_MessageDate")
		zMessage.Body.FormValue = ObjForm.GetValue("x_Body")
		zMessage.Body.OldValue = ObjForm.GetValue("o_Body")
		zMessage.MessageID.FormValue = ObjForm.GetValue("x_MessageID")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		zMessage.zPageID.CurrentValue = zMessage.zPageID.FormValue
		zMessage.ParentMessageID.CurrentValue = zMessage.ParentMessageID.FormValue
		zMessage.Subject.CurrentValue = zMessage.Subject.FormValue
		zMessage.Author.CurrentValue = zMessage.Author.FormValue
		zMessage.zEmail.CurrentValue = zMessage.zEmail.FormValue
		zMessage.City.CurrentValue = zMessage.City.FormValue
		zMessage.URL.CurrentValue = zMessage.URL.FormValue
		zMessage.MessageDate.CurrentValue = zMessage.MessageDate.FormValue
		zMessage.MessageDate.CurrentValue = ew_UnFormatDateTime(zMessage.MessageDate.CurrentValue, 6)
		zMessage.Body.CurrentValue = zMessage.Body.FormValue
		zMessage.MessageID.CurrentValue = zMessage.MessageID.FormValue
	End Sub

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = zMessage.KeyFilter

		' Row Selecting event
		zMessage.Row_Selecting(sFilter)

		' Load SQL based on filter
		zMessage.CurrentFilter = sFilter
		Dim sSql As String = zMessage.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				zMessage.Row_Selected(RsRow)
				Return True	
			End If
		Catch
			If EW_DEBUG_ENABLED Then Throw
			Return False
		Finally
			Conn.CloseTempDataReader()
		End Try
	End Function

	'
	' Load row values from recordset
	'
	Sub LoadRowValues(ByRef RsRow As OleDbDataReader)
		zMessage.MessageID.DbValue = RsRow("MessageID")
		zMessage.zPageID.DbValue = RsRow("PageID")
		zMessage.ParentMessageID.DbValue = RsRow("ParentMessageID")
		zMessage.Subject.DbValue = RsRow("Subject")
		zMessage.Author.DbValue = RsRow("Author")
		zMessage.zEmail.DbValue = RsRow("Email")
		zMessage.City.DbValue = RsRow("City")
		zMessage.URL.DbValue = RsRow("URL")
		zMessage.MessageDate.DbValue = RsRow("MessageDate")
		zMessage.Body.DbValue = RsRow("Body")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		zMessage.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' PageID

		zMessage.zPageID.CellCssStyle = ""
		zMessage.zPageID.CellCssClass = ""

		' ParentMessageID
		zMessage.ParentMessageID.CellCssStyle = ""
		zMessage.ParentMessageID.CellCssClass = ""

		' Subject
		zMessage.Subject.CellCssStyle = ""
		zMessage.Subject.CellCssClass = ""

		' Author
		zMessage.Author.CellCssStyle = ""
		zMessage.Author.CellCssClass = ""

		' Email
		zMessage.zEmail.CellCssStyle = ""
		zMessage.zEmail.CellCssClass = ""

		' City
		zMessage.City.CellCssStyle = ""
		zMessage.City.CellCssClass = ""

		' URL
		zMessage.URL.CellCssStyle = ""
		zMessage.URL.CellCssClass = ""

		' MessageDate
		zMessage.MessageDate.CellCssStyle = ""
		zMessage.MessageDate.CellCssClass = ""

		' Body
		zMessage.Body.CellCssStyle = ""
		zMessage.Body.CellCssClass = ""

		'
		'  View  Row
		'

		If zMessage.RowType = EW_ROWTYPE_VIEW Then ' View row

			' MessageID
			zMessage.MessageID.ViewValue = zMessage.MessageID.CurrentValue
			zMessage.MessageID.CssStyle = ""
			zMessage.MessageID.CssClass = ""
			zMessage.MessageID.ViewCustomAttributes = ""

			' PageID
			zMessage.zPageID.ViewValue = zMessage.zPageID.CurrentValue
			zMessage.zPageID.CssStyle = ""
			zMessage.zPageID.CssClass = ""
			zMessage.zPageID.ViewCustomAttributes = ""

			' ParentMessageID
			zMessage.ParentMessageID.ViewValue = zMessage.ParentMessageID.CurrentValue
			zMessage.ParentMessageID.CssStyle = ""
			zMessage.ParentMessageID.CssClass = ""
			zMessage.ParentMessageID.ViewCustomAttributes = ""

			' Subject
			zMessage.Subject.ViewValue = zMessage.Subject.CurrentValue
			zMessage.Subject.CssStyle = ""
			zMessage.Subject.CssClass = ""
			zMessage.Subject.ViewCustomAttributes = ""

			' Author
			zMessage.Author.ViewValue = zMessage.Author.CurrentValue
			zMessage.Author.CssStyle = ""
			zMessage.Author.CssClass = ""
			zMessage.Author.ViewCustomAttributes = ""

			' Email
			zMessage.zEmail.ViewValue = zMessage.zEmail.CurrentValue
			zMessage.zEmail.CssStyle = ""
			zMessage.zEmail.CssClass = ""
			zMessage.zEmail.ViewCustomAttributes = ""

			' City
			zMessage.City.ViewValue = zMessage.City.CurrentValue
			zMessage.City.CssStyle = ""
			zMessage.City.CssClass = ""
			zMessage.City.ViewCustomAttributes = ""

			' URL
			zMessage.URL.ViewValue = zMessage.URL.CurrentValue
			zMessage.URL.CssStyle = ""
			zMessage.URL.CssClass = ""
			zMessage.URL.ViewCustomAttributes = ""

			' MessageDate
			zMessage.MessageDate.ViewValue = zMessage.MessageDate.CurrentValue
			zMessage.MessageDate.ViewValue = ew_FormatDateTime(zMessage.MessageDate.ViewValue, 6)
			zMessage.MessageDate.CssStyle = ""
			zMessage.MessageDate.CssClass = ""
			zMessage.MessageDate.ViewCustomAttributes = ""

			' Body
			zMessage.Body.ViewValue = zMessage.Body.CurrentValue
			zMessage.Body.CssStyle = ""
			zMessage.Body.CssClass = ""
			zMessage.Body.ViewCustomAttributes = ""

			' View refer script
			' PageID

			zMessage.zPageID.HrefValue = ""

			' ParentMessageID
			zMessage.ParentMessageID.HrefValue = ""

			' Subject
			zMessage.Subject.HrefValue = ""

			' Author
			zMessage.Author.HrefValue = ""

			' Email
			zMessage.zEmail.HrefValue = ""

			' City
			zMessage.City.HrefValue = ""

			' URL
			zMessage.URL.HrefValue = ""

			' MessageDate
			zMessage.MessageDate.HrefValue = ""

			' Body
			zMessage.Body.HrefValue = ""

		'
		'  Add Row
		'

		ElseIf zMessage.RowType = EW_ROWTYPE_ADD Then ' Add row

			' PageID
			zMessage.zPageID.EditCustomAttributes = ""
			zMessage.zPageID.EditValue = ew_HtmlEncode(zMessage.zPageID.CurrentValue)

			' ParentMessageID
			zMessage.ParentMessageID.EditCustomAttributes = ""
			zMessage.ParentMessageID.EditValue = ew_HtmlEncode(zMessage.ParentMessageID.CurrentValue)

			' Subject
			zMessage.Subject.EditCustomAttributes = ""
			zMessage.Subject.EditValue = ew_HtmlEncode(zMessage.Subject.CurrentValue)

			' Author
			zMessage.Author.EditCustomAttributes = ""
			zMessage.Author.EditValue = ew_HtmlEncode(zMessage.Author.CurrentValue)

			' Email
			zMessage.zEmail.EditCustomAttributes = ""
			zMessage.zEmail.EditValue = ew_HtmlEncode(zMessage.zEmail.CurrentValue)

			' City
			zMessage.City.EditCustomAttributes = ""
			zMessage.City.EditValue = ew_HtmlEncode(zMessage.City.CurrentValue)

			' URL
			zMessage.URL.EditCustomAttributes = ""
			zMessage.URL.EditValue = ew_HtmlEncode(zMessage.URL.CurrentValue)

			' MessageDate
			zMessage.MessageDate.EditCustomAttributes = ""
			zMessage.MessageDate.EditValue = ew_FormatDateTime(zMessage.MessageDate.CurrentValue, 6)

			' Body
			zMessage.Body.EditCustomAttributes = ""
			zMessage.Body.EditValue = ew_HtmlEncode(zMessage.Body.CurrentValue)
		End If

		' Row Rendered event
		zMessage.Row_Rendered()
	End Sub

	'
	' Validate form
	'
	Function ValidateForm() As Boolean

		' Initialize
		ParentPage.gsFormError = ""

		' Check if validation required
		If Not EW_SERVER_VALIDATE Then Return (ParentPage.gsFormError = "")
		If Not ew_CheckInteger(zMessage.zPageID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Incorrect integer - Page ID"
		End If
		If Not ew_CheckInteger(zMessage.ParentMessageID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Incorrect integer - Parent Message ID"
		End If
		If Not ew_CheckUSDate(zMessage.MessageDate.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Incorrect date, format = mm/dd/yyyy - Message Date"
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

		' PageID
		zMessage.zPageID.SetDbValue(zMessage.zPageID.CurrentValue, System.DBNull.Value)
		Rs("PageID") = zMessage.zPageID.DbValue

		' ParentMessageID
		zMessage.ParentMessageID.SetDbValue(zMessage.ParentMessageID.CurrentValue, System.DBNull.Value)
		Rs("ParentMessageID") = zMessage.ParentMessageID.DbValue

		' Subject
		zMessage.Subject.SetDbValue(zMessage.Subject.CurrentValue, System.DBNull.Value)
		Rs("Subject") = zMessage.Subject.DbValue

		' Author
		zMessage.Author.SetDbValue(zMessage.Author.CurrentValue, System.DBNull.Value)
		Rs("Author") = zMessage.Author.DbValue

		' Email
		zMessage.zEmail.SetDbValue(zMessage.zEmail.CurrentValue, System.DBNull.Value)
		Rs("Email") = zMessage.zEmail.DbValue

		' City
		zMessage.City.SetDbValue(zMessage.City.CurrentValue, System.DBNull.Value)
		Rs("City") = zMessage.City.DbValue

		' URL
		zMessage.URL.SetDbValue(zMessage.URL.CurrentValue, System.DBNull.Value)
		Rs("URL") = zMessage.URL.DbValue

		' MessageDate
		zMessage.MessageDate.SetDbValue(ew_UnFormatDateTime(zMessage.MessageDate.CurrentValue, 6), System.DBNull.Value)
		Rs("MessageDate") = zMessage.MessageDate.DbValue

		' Body
		zMessage.Body.SetDbValue(zMessage.Body.CurrentValue, System.DBNull.Value)
		Rs("Body") = zMessage.Body.DbValue

		' Row Inserting event
		bInsertRow = zMessage.Row_Inserting(Rs)
		If bInsertRow Then
			Try	
				zMessage.Insert(Rs)
				AddRow = True
			Catch e As Exception
				If EW_DEBUG_ENABLED Then Throw
				Message = e.Message				
				AddRow = False
			End Try
		Else
			If zMessage.CancelMessage <> "" Then
				Message = zMessage.CancelMessage
				zMessage.CancelMessage = ""
			Else
				Message = "Insert cancelled"
			End If
			AddRow = False
		End If
		If AddRow Then
			LastInsertId = Conn.GetLastInsertId()
			zMessage.MessageID.DbValue = LastInsertId
			Rs("MessageID") = zMessage.MessageID.DbValue		

			' Row Inserted event
			zMessage.Row_Inserted(Rs)
			WriteAuditTrailOnAdd(Rs)
		End If
	End Function

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "Message"
		Dim filePfx As String = "log"
		Dim curDate As String, curTime As String, id As String, user As String, action As String
		Dim i As Integer
		Dim dt As DateTime = Now()		
		curDate = dt.ToString("yyyy/MM/dd")
		curTime = dt.ToString("HH:mm:ss")
		id = HttpContext.Current.Request.ServerVariables("SCRIPT_NAME")
    user = CurrentUserName()
		action = typ
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "", "", "", "")
	End Sub

	' Write Audit Trail (add page)
	Sub WriteAuditTrailOnAdd(ByRef RsSrc As OrderedDictionary)
		Dim table As String = "Message"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsSrc("MessageID")

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
    user = CurrentUserName()
		keyvalue = sKey

		' PageID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "PageID", keyvalue, oldvalue, RsSrc("PageID"))

		' ParentMessageID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "ParentMessageID", keyvalue, oldvalue, RsSrc("ParentMessageID"))

		' Subject Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "Subject", keyvalue, oldvalue, RsSrc("Subject"))

		' Author Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "Author", keyvalue, oldvalue, RsSrc("Author"))

		' Email Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "Email", keyvalue, oldvalue, RsSrc("Email"))

		' City Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "City", keyvalue, oldvalue, RsSrc("City"))

		' URL Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "URL", keyvalue, oldvalue, RsSrc("URL"))

		' MessageDate Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "MessageDate", keyvalue, oldvalue, RsSrc("MessageDate"))

		' Body Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "Body", keyvalue, oldvalue, "<MEMO>")
	End Sub

		' Page Load event
		Public Sub Page_Load()

			'HttpContext.Current.Response.Write("Page Load")
		End Sub

		' Page Unload event
		Public Sub Page_Unload()

			'HttpContext.Current.Response.Write("Page Unload")
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
		zMessage_add = New czMessage_add(Me)		
		zMessage_add.Page_Init()

		' Page main processing
		zMessage_add.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If zMessage_add IsNot Nothing Then zMessage_add.Dispose()
	End Sub
End Class
