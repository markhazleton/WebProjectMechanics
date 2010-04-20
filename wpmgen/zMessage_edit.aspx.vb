Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class zMessage_edit
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public zMessage_edit As czMessage_edit

	'
	' Page Class
	'
	Class czMessage_edit
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
			m_PageID = "edit"
			m_PageObjName = "zMessage_edit"
			m_PageObjTypeName = "czMessage_edit"

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

	'
	' Page main processing
	'
	Sub Page_Main()

		' Load key from QueryString
		If ew_Get("MessageID") <> "" Then
			zMessage.MessageID.QueryStringValue = ew_Get("MessageID")
		End If

		' Create form object
		ObjForm = New cFormObj
		If ObjForm.GetValue("a_edit") <> "" Then
			zMessage.CurrentAction = ObjForm.GetValue("a_edit") ' Get action code
			LoadFormValues() ' Get form values

			' Validate Form
			If Not ValidateForm() Then
				zMessage.CurrentAction = "" ' Form error, reset action
				Message = ParentPage.gsFormError
				LoadRow() ' Restore row
				RestoreFormValues() ' Restore form values if validate failed
			End If
		Else
			zMessage.CurrentAction = "I" ' Default action is display
		End If

		' Check if valid key
		If ew_Empty(zMessage.MessageID.CurrentValue) Then Page_Terminate("zMessage_list.aspx") ' Invalid key, return to list
		Select Case zMessage.CurrentAction
			Case "I" ' Get a record to display
				If Not LoadRow() Then ' Load Record based on key
					Message = "No records found" ' No record found
					Page_Terminate("zMessage_list.aspx") ' No matching record, return to list
				End If
			Case "U" ' Update
				zMessage.SendEmail = True ' Send email on update success
				If EditRow() Then ' Update Record based on key
					Message = "Update succeeded" ' Update success
					Dim sReturnUrl As String = zMessage.ReturnUrl
					If ew_GetPageName(sReturnUrl) = "zMessage_view.aspx" Then sReturnUrl = zMessage.ViewUrl ' View paging, return to view page with keyurl directly
					Page_Terminate(sReturnUrl) ' Return to caller
				Else
					LoadRow() ' Restore row
					RestoreFormValues() ' Restore form values if update failed
				End If
		End Select

		' Render the record
		zMessage.RowType = EW_ROWTYPE_EDIT ' Render as edit

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
	End Sub

	'
	' Load form values
	'
	Sub LoadFormValues()
		zMessage.MessageID.FormValue = ObjForm.GetValue("x_MessageID")
		zMessage.MessageID.OldValue = ObjForm.GetValue("o_MessageID")
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
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		zMessage.MessageID.CurrentValue = zMessage.MessageID.FormValue
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
		' MessageID

		zMessage.MessageID.CellCssStyle = ""
		zMessage.MessageID.CellCssClass = ""

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
			' MessageID

			zMessage.MessageID.HrefValue = ""

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
		'  Edit Row
		'

		ElseIf zMessage.RowType = EW_ROWTYPE_EDIT Then ' Edit row

			' MessageID
			zMessage.MessageID.EditCustomAttributes = ""
			zMessage.MessageID.EditValue = zMessage.MessageID.CurrentValue
			zMessage.MessageID.CssStyle = ""
			zMessage.MessageID.CssClass = ""
			zMessage.MessageID.ViewCustomAttributes = ""

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

			' Edit refer script
			' MessageID

			zMessage.MessageID.HrefValue = ""

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
	' Update record based on key values
	'
	Function EditRow() As Boolean
		Dim RsEdit As OleDbDataReader, RsChk As OleDbDataReader
		Dim sSql As String, sFilter As String
		Dim sSqlChk As String, sFilterChk As String
		Dim bUpdateRow As Boolean
		Dim RsOld As OrderedDictionary
		Dim sIdxErrMsg As String
		Dim Rs As New OrderedDictionary
		sFilter = zMessage.KeyFilter
		zMessage.CurrentFilter  = sFilter
		sSql = zMessage.SQL
		Try
			RsEdit = Conn.GetDataReader(sSql) 
		Catch e As Exception
			If EW_DEBUG_ENABLED Then Throw
			Message = e.Message
			RsEdit.Close()
			EditRow = False
		End Try
		If Not RsEdit.Read() Then
			RsEdit.Close()
			EditRow = False ' Update Failed
		Else
			RsOld = Conn.GetRow(RsEdit)
			RsEdit.Close()

			' MessageID
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

			' Row Updating event
			bUpdateRow = zMessage.Row_Updating(RsOld, Rs)
			If bUpdateRow Then
				Try
					zMessage.Update(Rs)
					EditRow = True
				Catch e As Exception
					If EW_DEBUG_ENABLED Then Throw
					Message = e.Message
					EditRow = False
				End Try
			Else
				If zMessage.CancelMessage <> "" Then
					Message = zMessage.CancelMessage
					zMessage.CancelMessage = ""
				Else
					Message = "Update cancelled"
				End If
				EditRow = False
			End If
		End If

		' Row Updated event
		If EditRow Then
			zMessage.Row_Updated(RsOld, Rs)
		End If
		If EditRow Then
			WriteAuditTrailOnEdit(RsOld, Rs)
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

	' Write Audit Trail (edit page)
	Sub WriteAuditTrailOnEdit(ByRef RsOld As OrderedDictionary, ByRef RsNew As OrderedDictionary)
		Dim table As String = "Message"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsOld("MessageID")

		' Write Audit Trail
		Dim filePfx As String = "log"
		Dim action As String = "U"
		Dim curDate As String, curTime As String, id As String, user As String, field As String
		Dim keyvalue As Object, oldvalue As Object, newvalue As Object
		Dim dt As DateTime = Now()
		Dim fld As cField, fldname As String, Modified As Boolean
		curDate = dt.ToString("yyyy/MM/dd")
		curTime = dt.ToString("HH:mm:ss")
		id = HttpContext.Current.Request.ServerVariables("SCRIPT_NAME")
    user = CurrentUserName()
		Dim arFldNames(RsNew.Count - 1) As String
		RsNew.Keys.CopyTo(arFldNames, 0)
		For Each fldname In arFldNames
			fld = zMessage.FieldByName(fldname)
			If fld IsNot Nothing AndAlso fld.FldDataType <> EW_DATATYPE_BLOB AndAlso fld.FldDataType <> EW_DATATYPE_MEMO Then ' Ignore Blob/Memo Field
				oldvalue = RsOld(fldname)
				newvalue = RsNew(fldname)

				'oldvalue = ew_Conv(oldvalue, fld.FldType)
				'newvalue = ew_Conv(newvalue, fld.FldType)

				If fld.FldDataType = EW_DATATYPE_DATE Then ' DateTime Field
					Modified = Not ew_SameStr(ew_FormatDateTime(oldvalue, 8), ew_FormatDateTime(newvalue, 8))
				Else
					Modified = Not ew_CompareValue(oldvalue, newvalue)
				End If				
				If Modified Then					
					keyvalue = sKey
					ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, fldname, keyvalue, oldvalue, newvalue)
				End If
			End If
		Next
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
		zMessage_edit = New czMessage_edit(Me)		
		zMessage_edit.Page_Init()

		' Page main processing
		zMessage_edit.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If zMessage_edit IsNot Nothing Then zMessage_edit.Dispose()
	End Sub
End Class
