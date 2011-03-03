Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class pagerole_edit
	Inherits AspNetMaker8_wpmWebsite

	' Page object
	Public PageRole_edit As cPageRole_edit

	'
	' Page Class
	'
	Class cPageRole_edit
		Inherits AspNetMakerPage
		Implements IDisposable		

		' Used by system generated functions
		Private RsWrk As Object, sSqlWrk As String, sWhereWrk As String

		Private sFilterWrk As String

		Private arwrk As Object

		Private armultiwrk() As String		

		' Page URL
		Public ReadOnly Property PageUrl() As String
			Get
				Dim Url As String = ew_CurrentPage() & "?"
				If PageRole.UseTokenInUrl Then Url = Url & "t=" & PageRole.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Common urls
		Public AddUrl As String = ""

		Public EditUrl As String = ""

		Public CopyUrl As String = ""

		Public DeleteUrl As String = ""

		Public ViewUrl As String = ""

		Public ListUrl As String = ""

		' Export urls
		Public ExportPrintUrl As String = ""

		Public ExportHtmlUrl As String = ""

		Public ExportExcelUrl As String = ""

		Public ExportWordUrl As String = ""

		Public ExportXmlUrl As String = ""

		Public ExportCsvUrl As String = ""

		' Inline urls
		Public InlineAddUrl As String = ""

		Public InlineCopyUrl As String = ""

		Public InlineEditUrl As String = ""

		Public GridAddUrl As String = ""

		Public GridEditUrl As String = ""

		Public MultiDeleteUrl As String = ""

		Public MultiUpdateUrl As String = ""
		Protected m_DebugMsg As String = ""

		Public Property DebugMsg() As String
			Get
				Return IIf(m_DebugMsg <> "", "<p>" & m_DebugMsg & "</p>", m_DebugMsg)
			End Get
			Set(ByVal v As String)
				If m_DebugMsg <> "" Then ' Append
					m_DebugMsg = m_DebugMsg & "<br />" & v
				Else
					m_DebugMsg = v
				End If
			End Set
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
			Dim sMessage As String
			sMessage = Message
			Message_Showing(sMessage)
			If sMessage <> "" Then ew_Write("<p><span class=""ewMessage"">" & sMessage & "</span></p>")
			ew_Session(EW_SESSION_MESSAGE) = "" ' Clear message in Session
		End Sub			

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If PageRole.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (PageRole.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (PageRole.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' ASP.NET page object
		Public ReadOnly Property AspNetPage() As pagerole_edit
			Get
				Return CType(m_ParentPage, pagerole_edit)
			End Get
		End Property

		' PageRole
		Public Property PageRole() As cPageRole
			Get				
				Return ParentPage.PageRole
			End Get
			Set(ByVal v As cPageRole)
				ParentPage.PageRole = v	
			End Set	
		End Property

		'
		'  Constructor
		'  - init objects
		'  - open connection
		'
		Public Sub New(ByRef APage As AspNetMaker8_wpmWebsite)				
			m_ParentPage = APage
			m_Page = Me
			m_PageID = "edit"
			m_PageObjName = "PageRole_edit"
			m_PageObjTypeName = "cPageRole_edit"			

			' Initialize language object
			Language = New cLanguage(Me)

			' Table Name
			m_TableName = "PageRole"

			' Initialize table object
			PageRole = New cPageRole(Me)

			' Initialize URLs
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

		' Create form object
		ObjForm = New cFormObj()

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
			PageRole.Dispose()
			ObjForm = Nothing

			' Go to URL if specified
			Dim sRedirectUrl As String = url
			Page_Redirecting(sRedirectUrl)
			If sRedirectUrl <> "" Then
				HttpContext.Current.Response.Clear()
				HttpContext.Current.Response.Redirect(sReDirectUrl)
			End If
		End Sub

	Public sDbMasterFilter As String, sDbDetailFilter As String

	'
	' Page main processing
	'
	Sub Page_Main()

		' Load key from QueryString
		If ew_Get("PageRoleID") <> "" Then
			PageRole.PageRoleID.QueryStringValue = ew_Get("PageRoleID")
		End If
		If ObjForm.GetValue("a_edit") <> "" Then
			PageRole.CurrentAction = ObjForm.GetValue("a_edit") ' Get action code
			LoadFormValues() ' Get form values

			' Validate Form
			If Not ValidateForm() Then
				PageRole.CurrentAction = "" ' Form error, reset action
				Message = ParentPage.gsFormError
				PageRole.EventCancelled = True ' Event cancelled
				LoadRow() ' Restore row
				RestoreFormValues() ' Restore form values if validate failed
			End If
		Else
			PageRole.CurrentAction = "I" ' Default action is display
		End If

		' Check if valid key
		If ew_Empty(PageRole.PageRoleID.CurrentValue) Then Page_Terminate("pagerole_list.aspx") ' Invalid key, return to list
		Select Case PageRole.CurrentAction
			Case "I" ' Get a record to display
				If Not LoadRow() Then ' Load Record based on key
					Message = Language.Phrase("NoRecord") ' No record found
					Page_Terminate("pagerole_list.aspx") ' No matching record, return to list
				End If
			Case "U" ' Update
				PageRole.SendEmail = True ' Send email on update success
				If EditRow() Then ' Update Record based on key
					Message = Language.Phrase("UpdateSuccess") ' Update success
					Dim sReturnUrl As String = PageRole.ReturnUrl
					Page_Terminate(sReturnUrl) ' Return to caller
				Else
					PageRole.EventCancelled = True ' Event cancelled 
					LoadRow() ' Restore row
					RestoreFormValues() ' Restore form values if update failed
				End If
		End Select

		' Render the record
		PageRole.RowType = EW_ROWTYPE_EDIT ' Render as edit

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
		PageRole.PageRoleID.FormValue = ObjForm.GetValue("x_PageRoleID")
		PageRole.PageRoleID.OldValue = ObjForm.GetValue("o_PageRoleID")
		PageRole.RoleID.FormValue = ObjForm.GetValue("x_RoleID")
		PageRole.RoleID.OldValue = ObjForm.GetValue("o_RoleID")
		PageRole.zPageID.FormValue = ObjForm.GetValue("x_zPageID")
		PageRole.zPageID.OldValue = ObjForm.GetValue("o_zPageID")
		PageRole.CompanyID.FormValue = ObjForm.GetValue("x_CompanyID")
		PageRole.CompanyID.OldValue = ObjForm.GetValue("o_CompanyID")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		PageRole.PageRoleID.CurrentValue = PageRole.PageRoleID.FormValue
		PageRole.RoleID.CurrentValue = PageRole.RoleID.FormValue
		PageRole.zPageID.CurrentValue = PageRole.zPageID.FormValue
		PageRole.CompanyID.CurrentValue = PageRole.CompanyID.FormValue
	End Sub

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = PageRole.KeyFilter

		' Row Selecting event
		PageRole.Row_Selecting(sFilter)

		' Load SQL based on filter
		PageRole.CurrentFilter = sFilter
		Dim sSql As String = PageRole.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then DebugMsg = sSql
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				PageRole.Row_Selected(RsRow)
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
		Dim sDetailFilter As String
		PageRole.PageRoleID.DbValue = RsRow("PageRoleID")
		PageRole.RoleID.DbValue = RsRow("RoleID")
		PageRole.zPageID.DbValue = RsRow("PageID")
		PageRole.CompanyID.DbValue = RsRow("CompanyID")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Initialize urls
		' Row Rendering event

		PageRole.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' PageRoleID

		PageRole.PageRoleID.CellCssStyle = ""
		PageRole.PageRoleID.CellCssClass = ""
		PageRole.PageRoleID.CellAttrs.Clear(): PageRole.PageRoleID.ViewAttrs.Clear(): PageRole.PageRoleID.EditAttrs.Clear()

		' RoleID
		PageRole.RoleID.CellCssStyle = ""
		PageRole.RoleID.CellCssClass = ""
		PageRole.RoleID.CellAttrs.Clear(): PageRole.RoleID.ViewAttrs.Clear(): PageRole.RoleID.EditAttrs.Clear()

		' PageID
		PageRole.zPageID.CellCssStyle = ""
		PageRole.zPageID.CellCssClass = ""
		PageRole.zPageID.CellAttrs.Clear(): PageRole.zPageID.ViewAttrs.Clear(): PageRole.zPageID.EditAttrs.Clear()

		' CompanyID
		PageRole.CompanyID.CellCssStyle = ""
		PageRole.CompanyID.CellCssClass = ""
		PageRole.CompanyID.CellAttrs.Clear(): PageRole.CompanyID.ViewAttrs.Clear(): PageRole.CompanyID.EditAttrs.Clear()

		'
		'  View  Row
		'

		If PageRole.RowType = EW_ROWTYPE_VIEW Then ' View row

			' PageRoleID
			PageRole.PageRoleID.ViewValue = PageRole.PageRoleID.CurrentValue
			PageRole.PageRoleID.CssStyle = ""
			PageRole.PageRoleID.CssClass = ""
			PageRole.PageRoleID.ViewCustomAttributes = ""

			' RoleID
			PageRole.RoleID.ViewValue = PageRole.RoleID.CurrentValue
			PageRole.RoleID.CssStyle = ""
			PageRole.RoleID.CssClass = ""
			PageRole.RoleID.ViewCustomAttributes = ""

			' PageID
			PageRole.zPageID.ViewValue = PageRole.zPageID.CurrentValue
			PageRole.zPageID.CssStyle = ""
			PageRole.zPageID.CssClass = ""
			PageRole.zPageID.ViewCustomAttributes = ""

			' CompanyID
			PageRole.CompanyID.ViewValue = PageRole.CompanyID.CurrentValue
			PageRole.CompanyID.CssStyle = ""
			PageRole.CompanyID.CssClass = ""
			PageRole.CompanyID.ViewCustomAttributes = ""

			' View refer script
			' PageRoleID

			PageRole.PageRoleID.HrefValue = ""
			PageRole.PageRoleID.TooltipValue = ""

			' RoleID
			PageRole.RoleID.HrefValue = ""
			PageRole.RoleID.TooltipValue = ""

			' PageID
			PageRole.zPageID.HrefValue = ""
			PageRole.zPageID.TooltipValue = ""

			' CompanyID
			PageRole.CompanyID.HrefValue = ""
			PageRole.CompanyID.TooltipValue = ""

		'
		'  Edit Row
		'

		ElseIf PageRole.RowType = EW_ROWTYPE_EDIT Then ' Edit row

			' PageRoleID
			PageRole.PageRoleID.EditCustomAttributes = ""
			PageRole.PageRoleID.EditValue = PageRole.PageRoleID.CurrentValue
			PageRole.PageRoleID.CssStyle = ""
			PageRole.PageRoleID.CssClass = ""
			PageRole.PageRoleID.ViewCustomAttributes = ""

			' RoleID
			PageRole.RoleID.EditCustomAttributes = ""
			PageRole.RoleID.EditValue = ew_HtmlEncode(PageRole.RoleID.CurrentValue)

			' PageID
			PageRole.zPageID.EditCustomAttributes = ""
			PageRole.zPageID.EditValue = ew_HtmlEncode(PageRole.zPageID.CurrentValue)

			' CompanyID
			PageRole.CompanyID.EditCustomAttributes = ""
			PageRole.CompanyID.EditValue = ew_HtmlEncode(PageRole.CompanyID.CurrentValue)

			' Edit refer script
			' PageRoleID

			PageRole.PageRoleID.HrefValue = ""

			' RoleID
			PageRole.RoleID.HrefValue = ""

			' PageID
			PageRole.zPageID.HrefValue = ""

			' CompanyID
			PageRole.CompanyID.HrefValue = ""
		End If

		' Row Rendered event
		If PageRole.RowType <> EW_ROWTYPE_AGGREGATEINIT Then
			PageRole.Row_Rendered()
		End If
	End Sub

	'
	' Validate form
	'
	Function ValidateForm() As Boolean

		' Initialize
		ParentPage.gsFormError = ""

		' Check if validation required
		If Not EW_SERVER_VALIDATE Then Return (ParentPage.gsFormError = "")
		If Not ew_CheckInteger(PageRole.RoleID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError &= PageRole.RoleID.FldErrMsg
		End If
		If Not ew_CheckInteger(PageRole.zPageID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError &= PageRole.zPageID.FldErrMsg
		End If
		If Not ew_CheckInteger(PageRole.CompanyID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError &= PageRole.CompanyID.FldErrMsg
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
		sFilter = PageRole.KeyFilter
		PageRole.CurrentFilter  = sFilter
		sSql = PageRole.SQL
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
			Try
				RsOld = Conn.GetRow(RsEdit)					 

				' PageRoleID
				' RoleID

				PageRole.RoleID.SetDbValue(Rs, PageRole.RoleID.CurrentValue, System.DBNull.Value, False)

				' PageID
				PageRole.zPageID.SetDbValue(Rs, PageRole.zPageID.CurrentValue, System.DBNull.Value, False)

				' CompanyID
				PageRole.CompanyID.SetDbValue(Rs, PageRole.CompanyID.CurrentValue, System.DBNull.Value, False)
			Catch e As Exception
				If EW_DEBUG_ENABLED Then Throw
				Message = e.Message
				EditRow = False
			End Try
			RsEdit.Close()

			' Row Updating event
			bUpdateRow = PageRole.Row_Updating(RsOld, Rs)
			If bUpdateRow Then
				Try
					PageRole.Update(Rs)
					EditRow = True
				Catch e As Exception
					If EW_DEBUG_ENABLED Then Throw
					Message = e.Message
					EditRow = False
				End Try
			Else
				If PageRole.CancelMessage <> "" Then
					Message = PageRole.CancelMessage
					PageRole.CancelMessage = ""
				Else
					Message = Language.Phrase("UpdateCancelled")
				End If
				EditRow = False
			End If
		End If

		' Row Updated event
		If EditRow Then
			PageRole.Row_Updated(RsOld, Rs)
		End If
		If EditRow Then
			WriteAuditTrailOnEdit(RsOld, Rs)
		End If
	End Function

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "PageRole"
		Dim filePfx As String = "log"
		Dim curDateTime As String, id As String, user As String, action As String
		Dim i As Integer
		Dim dt As DateTime = Now()		
		curDateTime = dt.ToString("yyyy/MM/dd HH:mm:ss")
		id = HttpContext.Current.Request.ServerVariables("SCRIPT_NAME")
    user = CurrentUserName()
		action = typ
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "", "", "", "")
	End Sub

	' Write Audit Trail (edit page)
	Sub WriteAuditTrailOnEdit(ByRef RsOld As OrderedDictionary, ByRef RsNew As OrderedDictionary)
		Dim table As String = "PageRole"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsOld("PageRoleID")

		' Write Audit Trail
		Dim filePfx As String = "log"
		Dim action As String = "U"
		Dim curDateTime As String, id As String, user As String, field As String
		Dim keyvalue As Object, oldvalue As Object, newvalue As Object
		Dim dt As DateTime = Now()
		Dim fld As cField, fldname As String, Modified As Boolean
		curDateTime = dt.ToString("yyyy/MM/dd HH:mm:ss")
		id = HttpContext.Current.Request.ServerVariables("SCRIPT_NAME")
    user = CurrentUserName()
		Dim arFldNames(RsNew.Count - 1) As String
		RsNew.Keys.CopyTo(arFldNames, 0)
		For Each fldname In arFldNames
			fld = PageRole.FieldByName(fldname)
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
					ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, fldname, keyvalue, oldvalue, newvalue)
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

		' Page Redirecting event
		Public Sub Page_Redirecting(ByRef url As String)

			'url = newurl
		End Sub

		' Message Showing event
		Public Sub Message_Showing(ByRef msg As String)

			'msg = newmsg
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

		' Page init
		PageRole_edit = New cPageRole_edit(Me)		
		PageRole_edit.Page_Init()		

		' Set buffer/cache option
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

		' Page main processing
		PageRole_edit.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If PageRole_edit IsNot Nothing Then PageRole_edit.Dispose()
	End Sub
End Class
