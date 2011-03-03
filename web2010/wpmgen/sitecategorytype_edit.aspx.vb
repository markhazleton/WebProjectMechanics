Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class sitecategorytype_edit
	Inherits AspNetMaker8_wpmWebsite

	' Page object
	Public SiteCategoryType_edit As cSiteCategoryType_edit

	'
	' Page Class
	'
	Class cSiteCategoryType_edit
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
				If SiteCategoryType.UseTokenInUrl Then Url = Url & "t=" & SiteCategoryType.TableVar & "&" ' Add page token
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
			If SiteCategoryType.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (SiteCategoryType.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (SiteCategoryType.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' ASP.NET page object
		Public ReadOnly Property AspNetPage() As sitecategorytype_edit
			Get
				Return CType(m_ParentPage, sitecategorytype_edit)
			End Get
		End Property

		' SiteCategoryType
		Public Property SiteCategoryType() As cSiteCategoryType
			Get				
				Return ParentPage.SiteCategoryType
			End Get
			Set(ByVal v As cSiteCategoryType)
				ParentPage.SiteCategoryType = v	
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
			m_PageObjName = "SiteCategoryType_edit"
			m_PageObjTypeName = "cSiteCategoryType_edit"			

			' Initialize language object
			Language = New cLanguage(Me)

			' Table Name
			m_TableName = "SiteCategoryType"

			' Initialize table object
			SiteCategoryType = New cSiteCategoryType(Me)

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
			SiteCategoryType.Dispose()
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
		If ew_Get("SiteCategoryTypeID") <> "" Then
			SiteCategoryType.SiteCategoryTypeID.QueryStringValue = ew_Get("SiteCategoryTypeID")
		End If
		If ObjForm.GetValue("a_edit") <> "" Then
			SiteCategoryType.CurrentAction = ObjForm.GetValue("a_edit") ' Get action code
			LoadFormValues() ' Get form values

			' Validate Form
			If Not ValidateForm() Then
				SiteCategoryType.CurrentAction = "" ' Form error, reset action
				Message = ParentPage.gsFormError
				SiteCategoryType.EventCancelled = True ' Event cancelled
				LoadRow() ' Restore row
				RestoreFormValues() ' Restore form values if validate failed
			End If
		Else
			SiteCategoryType.CurrentAction = "I" ' Default action is display
		End If

		' Check if valid key
		If ew_Empty(SiteCategoryType.SiteCategoryTypeID.CurrentValue) Then Page_Terminate("sitecategorytype_list.aspx") ' Invalid key, return to list
		Select Case SiteCategoryType.CurrentAction
			Case "I" ' Get a record to display
				If Not LoadRow() Then ' Load Record based on key
					Message = Language.Phrase("NoRecord") ' No record found
					Page_Terminate("sitecategorytype_list.aspx") ' No matching record, return to list
				End If
			Case "U" ' Update
				SiteCategoryType.SendEmail = True ' Send email on update success
				If EditRow() Then ' Update Record based on key
					Message = Language.Phrase("UpdateSuccess") ' Update success
					Dim sReturnUrl As String = SiteCategoryType.ReturnUrl
					Page_Terminate(sReturnUrl) ' Return to caller
				Else
					SiteCategoryType.EventCancelled = True ' Event cancelled 
					LoadRow() ' Restore row
					RestoreFormValues() ' Restore form values if update failed
				End If
		End Select

		' Render the record
		SiteCategoryType.RowType = EW_ROWTYPE_EDIT ' Render as edit

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
		SiteCategoryType.SiteCategoryTypeID.FormValue = ObjForm.GetValue("x_SiteCategoryTypeID")
		SiteCategoryType.SiteCategoryTypeID.OldValue = ObjForm.GetValue("o_SiteCategoryTypeID")
		SiteCategoryType.SiteCategoryTypeNM.FormValue = ObjForm.GetValue("x_SiteCategoryTypeNM")
		SiteCategoryType.SiteCategoryTypeNM.OldValue = ObjForm.GetValue("o_SiteCategoryTypeNM")
		SiteCategoryType.SiteCategoryTypeDS.FormValue = ObjForm.GetValue("x_SiteCategoryTypeDS")
		SiteCategoryType.SiteCategoryTypeDS.OldValue = ObjForm.GetValue("o_SiteCategoryTypeDS")
		SiteCategoryType.SiteCategoryComment.FormValue = ObjForm.GetValue("x_SiteCategoryComment")
		SiteCategoryType.SiteCategoryComment.OldValue = ObjForm.GetValue("o_SiteCategoryComment")
		SiteCategoryType.SiteCategoryFileName.FormValue = ObjForm.GetValue("x_SiteCategoryFileName")
		SiteCategoryType.SiteCategoryFileName.OldValue = ObjForm.GetValue("o_SiteCategoryFileName")
		SiteCategoryType.SiteCategoryTransferURL.FormValue = ObjForm.GetValue("x_SiteCategoryTransferURL")
		SiteCategoryType.SiteCategoryTransferURL.OldValue = ObjForm.GetValue("o_SiteCategoryTransferURL")
		SiteCategoryType.DefaultSiteCategoryID.FormValue = ObjForm.GetValue("x_DefaultSiteCategoryID")
		SiteCategoryType.DefaultSiteCategoryID.OldValue = ObjForm.GetValue("o_DefaultSiteCategoryID")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		SiteCategoryType.SiteCategoryTypeID.CurrentValue = SiteCategoryType.SiteCategoryTypeID.FormValue
		SiteCategoryType.SiteCategoryTypeNM.CurrentValue = SiteCategoryType.SiteCategoryTypeNM.FormValue
		SiteCategoryType.SiteCategoryTypeDS.CurrentValue = SiteCategoryType.SiteCategoryTypeDS.FormValue
		SiteCategoryType.SiteCategoryComment.CurrentValue = SiteCategoryType.SiteCategoryComment.FormValue
		SiteCategoryType.SiteCategoryFileName.CurrentValue = SiteCategoryType.SiteCategoryFileName.FormValue
		SiteCategoryType.SiteCategoryTransferURL.CurrentValue = SiteCategoryType.SiteCategoryTransferURL.FormValue
		SiteCategoryType.DefaultSiteCategoryID.CurrentValue = SiteCategoryType.DefaultSiteCategoryID.FormValue
	End Sub

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = SiteCategoryType.KeyFilter

		' Row Selecting event
		SiteCategoryType.Row_Selecting(sFilter)

		' Load SQL based on filter
		SiteCategoryType.CurrentFilter = sFilter
		Dim sSql As String = SiteCategoryType.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then DebugMsg = sSql
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				SiteCategoryType.Row_Selected(RsRow)
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
		SiteCategoryType.SiteCategoryTypeID.DbValue = RsRow("SiteCategoryTypeID")
		SiteCategoryType.SiteCategoryTypeNM.DbValue = RsRow("SiteCategoryTypeNM")
		SiteCategoryType.SiteCategoryTypeDS.DbValue = RsRow("SiteCategoryTypeDS")
		SiteCategoryType.SiteCategoryComment.DbValue = RsRow("SiteCategoryComment")
		SiteCategoryType.SiteCategoryFileName.DbValue = RsRow("SiteCategoryFileName")
		SiteCategoryType.SiteCategoryTransferURL.DbValue = RsRow("SiteCategoryTransferURL")
		SiteCategoryType.DefaultSiteCategoryID.DbValue = RsRow("DefaultSiteCategoryID")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Initialize urls
		' Row Rendering event

		SiteCategoryType.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' SiteCategoryTypeID

		SiteCategoryType.SiteCategoryTypeID.CellCssStyle = ""
		SiteCategoryType.SiteCategoryTypeID.CellCssClass = ""
		SiteCategoryType.SiteCategoryTypeID.CellAttrs.Clear(): SiteCategoryType.SiteCategoryTypeID.ViewAttrs.Clear(): SiteCategoryType.SiteCategoryTypeID.EditAttrs.Clear()

		' SiteCategoryTypeNM
		SiteCategoryType.SiteCategoryTypeNM.CellCssStyle = ""
		SiteCategoryType.SiteCategoryTypeNM.CellCssClass = ""
		SiteCategoryType.SiteCategoryTypeNM.CellAttrs.Clear(): SiteCategoryType.SiteCategoryTypeNM.ViewAttrs.Clear(): SiteCategoryType.SiteCategoryTypeNM.EditAttrs.Clear()

		' SiteCategoryTypeDS
		SiteCategoryType.SiteCategoryTypeDS.CellCssStyle = ""
		SiteCategoryType.SiteCategoryTypeDS.CellCssClass = ""
		SiteCategoryType.SiteCategoryTypeDS.CellAttrs.Clear(): SiteCategoryType.SiteCategoryTypeDS.ViewAttrs.Clear(): SiteCategoryType.SiteCategoryTypeDS.EditAttrs.Clear()

		' SiteCategoryComment
		SiteCategoryType.SiteCategoryComment.CellCssStyle = ""
		SiteCategoryType.SiteCategoryComment.CellCssClass = ""
		SiteCategoryType.SiteCategoryComment.CellAttrs.Clear(): SiteCategoryType.SiteCategoryComment.ViewAttrs.Clear(): SiteCategoryType.SiteCategoryComment.EditAttrs.Clear()

		' SiteCategoryFileName
		SiteCategoryType.SiteCategoryFileName.CellCssStyle = ""
		SiteCategoryType.SiteCategoryFileName.CellCssClass = ""
		SiteCategoryType.SiteCategoryFileName.CellAttrs.Clear(): SiteCategoryType.SiteCategoryFileName.ViewAttrs.Clear(): SiteCategoryType.SiteCategoryFileName.EditAttrs.Clear()

		' SiteCategoryTransferURL
		SiteCategoryType.SiteCategoryTransferURL.CellCssStyle = ""
		SiteCategoryType.SiteCategoryTransferURL.CellCssClass = ""
		SiteCategoryType.SiteCategoryTransferURL.CellAttrs.Clear(): SiteCategoryType.SiteCategoryTransferURL.ViewAttrs.Clear(): SiteCategoryType.SiteCategoryTransferURL.EditAttrs.Clear()

		' DefaultSiteCategoryID
		SiteCategoryType.DefaultSiteCategoryID.CellCssStyle = ""
		SiteCategoryType.DefaultSiteCategoryID.CellCssClass = ""
		SiteCategoryType.DefaultSiteCategoryID.CellAttrs.Clear(): SiteCategoryType.DefaultSiteCategoryID.ViewAttrs.Clear(): SiteCategoryType.DefaultSiteCategoryID.EditAttrs.Clear()

		'
		'  View  Row
		'

		If SiteCategoryType.RowType = EW_ROWTYPE_VIEW Then ' View row

			' SiteCategoryTypeID
			SiteCategoryType.SiteCategoryTypeID.ViewValue = SiteCategoryType.SiteCategoryTypeID.CurrentValue
			SiteCategoryType.SiteCategoryTypeID.CssStyle = ""
			SiteCategoryType.SiteCategoryTypeID.CssClass = ""
			SiteCategoryType.SiteCategoryTypeID.ViewCustomAttributes = ""

			' SiteCategoryTypeNM
			SiteCategoryType.SiteCategoryTypeNM.ViewValue = SiteCategoryType.SiteCategoryTypeNM.CurrentValue
			SiteCategoryType.SiteCategoryTypeNM.CssStyle = ""
			SiteCategoryType.SiteCategoryTypeNM.CssClass = ""
			SiteCategoryType.SiteCategoryTypeNM.ViewCustomAttributes = ""

			' SiteCategoryTypeDS
			SiteCategoryType.SiteCategoryTypeDS.ViewValue = SiteCategoryType.SiteCategoryTypeDS.CurrentValue
			SiteCategoryType.SiteCategoryTypeDS.CssStyle = ""
			SiteCategoryType.SiteCategoryTypeDS.CssClass = ""
			SiteCategoryType.SiteCategoryTypeDS.ViewCustomAttributes = ""

			' SiteCategoryComment
			SiteCategoryType.SiteCategoryComment.ViewValue = SiteCategoryType.SiteCategoryComment.CurrentValue
			SiteCategoryType.SiteCategoryComment.CssStyle = ""
			SiteCategoryType.SiteCategoryComment.CssClass = ""
			SiteCategoryType.SiteCategoryComment.ViewCustomAttributes = ""

			' SiteCategoryFileName
			SiteCategoryType.SiteCategoryFileName.ViewValue = SiteCategoryType.SiteCategoryFileName.CurrentValue
			SiteCategoryType.SiteCategoryFileName.CssStyle = ""
			SiteCategoryType.SiteCategoryFileName.CssClass = ""
			SiteCategoryType.SiteCategoryFileName.ViewCustomAttributes = ""

			' SiteCategoryTransferURL
			SiteCategoryType.SiteCategoryTransferURL.ViewValue = SiteCategoryType.SiteCategoryTransferURL.CurrentValue
			SiteCategoryType.SiteCategoryTransferURL.CssStyle = ""
			SiteCategoryType.SiteCategoryTransferURL.CssClass = ""
			SiteCategoryType.SiteCategoryTransferURL.ViewCustomAttributes = ""

			' DefaultSiteCategoryID
			SiteCategoryType.DefaultSiteCategoryID.ViewValue = SiteCategoryType.DefaultSiteCategoryID.CurrentValue
			SiteCategoryType.DefaultSiteCategoryID.CssStyle = ""
			SiteCategoryType.DefaultSiteCategoryID.CssClass = ""
			SiteCategoryType.DefaultSiteCategoryID.ViewCustomAttributes = ""

			' View refer script
			' SiteCategoryTypeID

			SiteCategoryType.SiteCategoryTypeID.HrefValue = ""
			SiteCategoryType.SiteCategoryTypeID.TooltipValue = ""

			' SiteCategoryTypeNM
			SiteCategoryType.SiteCategoryTypeNM.HrefValue = ""
			SiteCategoryType.SiteCategoryTypeNM.TooltipValue = ""

			' SiteCategoryTypeDS
			SiteCategoryType.SiteCategoryTypeDS.HrefValue = ""
			SiteCategoryType.SiteCategoryTypeDS.TooltipValue = ""

			' SiteCategoryComment
			SiteCategoryType.SiteCategoryComment.HrefValue = ""
			SiteCategoryType.SiteCategoryComment.TooltipValue = ""

			' SiteCategoryFileName
			SiteCategoryType.SiteCategoryFileName.HrefValue = ""
			SiteCategoryType.SiteCategoryFileName.TooltipValue = ""

			' SiteCategoryTransferURL
			SiteCategoryType.SiteCategoryTransferURL.HrefValue = ""
			SiteCategoryType.SiteCategoryTransferURL.TooltipValue = ""

			' DefaultSiteCategoryID
			SiteCategoryType.DefaultSiteCategoryID.HrefValue = ""
			SiteCategoryType.DefaultSiteCategoryID.TooltipValue = ""

		'
		'  Edit Row
		'

		ElseIf SiteCategoryType.RowType = EW_ROWTYPE_EDIT Then ' Edit row

			' SiteCategoryTypeID
			SiteCategoryType.SiteCategoryTypeID.EditCustomAttributes = ""
			SiteCategoryType.SiteCategoryTypeID.EditValue = SiteCategoryType.SiteCategoryTypeID.CurrentValue
			SiteCategoryType.SiteCategoryTypeID.CssStyle = ""
			SiteCategoryType.SiteCategoryTypeID.CssClass = ""
			SiteCategoryType.SiteCategoryTypeID.ViewCustomAttributes = ""

			' SiteCategoryTypeNM
			SiteCategoryType.SiteCategoryTypeNM.EditCustomAttributes = ""
			SiteCategoryType.SiteCategoryTypeNM.EditValue = ew_HtmlEncode(SiteCategoryType.SiteCategoryTypeNM.CurrentValue)

			' SiteCategoryTypeDS
			SiteCategoryType.SiteCategoryTypeDS.EditCustomAttributes = ""
			SiteCategoryType.SiteCategoryTypeDS.EditValue = ew_HtmlEncode(SiteCategoryType.SiteCategoryTypeDS.CurrentValue)

			' SiteCategoryComment
			SiteCategoryType.SiteCategoryComment.EditCustomAttributes = ""
			SiteCategoryType.SiteCategoryComment.EditValue = ew_HtmlEncode(SiteCategoryType.SiteCategoryComment.CurrentValue)

			' SiteCategoryFileName
			SiteCategoryType.SiteCategoryFileName.EditCustomAttributes = ""
			SiteCategoryType.SiteCategoryFileName.EditValue = ew_HtmlEncode(SiteCategoryType.SiteCategoryFileName.CurrentValue)

			' SiteCategoryTransferURL
			SiteCategoryType.SiteCategoryTransferURL.EditCustomAttributes = ""
			SiteCategoryType.SiteCategoryTransferURL.EditValue = ew_HtmlEncode(SiteCategoryType.SiteCategoryTransferURL.CurrentValue)

			' DefaultSiteCategoryID
			SiteCategoryType.DefaultSiteCategoryID.EditCustomAttributes = ""
			SiteCategoryType.DefaultSiteCategoryID.EditValue = ew_HtmlEncode(SiteCategoryType.DefaultSiteCategoryID.CurrentValue)

			' Edit refer script
			' SiteCategoryTypeID

			SiteCategoryType.SiteCategoryTypeID.HrefValue = ""

			' SiteCategoryTypeNM
			SiteCategoryType.SiteCategoryTypeNM.HrefValue = ""

			' SiteCategoryTypeDS
			SiteCategoryType.SiteCategoryTypeDS.HrefValue = ""

			' SiteCategoryComment
			SiteCategoryType.SiteCategoryComment.HrefValue = ""

			' SiteCategoryFileName
			SiteCategoryType.SiteCategoryFileName.HrefValue = ""

			' SiteCategoryTransferURL
			SiteCategoryType.SiteCategoryTransferURL.HrefValue = ""

			' DefaultSiteCategoryID
			SiteCategoryType.DefaultSiteCategoryID.HrefValue = ""
		End If

		' Row Rendered event
		If SiteCategoryType.RowType <> EW_ROWTYPE_AGGREGATEINIT Then
			SiteCategoryType.Row_Rendered()
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
		If Not ew_CheckInteger(SiteCategoryType.DefaultSiteCategoryID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError &= SiteCategoryType.DefaultSiteCategoryID.FldErrMsg
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
		sFilter = SiteCategoryType.KeyFilter
		SiteCategoryType.CurrentFilter  = sFilter
		sSql = SiteCategoryType.SQL
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

				' SiteCategoryTypeID
				' SiteCategoryTypeNM

				SiteCategoryType.SiteCategoryTypeNM.SetDbValue(Rs, SiteCategoryType.SiteCategoryTypeNM.CurrentValue, System.DBNull.Value, False)

				' SiteCategoryTypeDS
				SiteCategoryType.SiteCategoryTypeDS.SetDbValue(Rs, SiteCategoryType.SiteCategoryTypeDS.CurrentValue, System.DBNull.Value, False)

				' SiteCategoryComment
				SiteCategoryType.SiteCategoryComment.SetDbValue(Rs, SiteCategoryType.SiteCategoryComment.CurrentValue, System.DBNull.Value, False)

				' SiteCategoryFileName
				SiteCategoryType.SiteCategoryFileName.SetDbValue(Rs, SiteCategoryType.SiteCategoryFileName.CurrentValue, System.DBNull.Value, False)

				' SiteCategoryTransferURL
				SiteCategoryType.SiteCategoryTransferURL.SetDbValue(Rs, SiteCategoryType.SiteCategoryTransferURL.CurrentValue, System.DBNull.Value, False)

				' DefaultSiteCategoryID
				SiteCategoryType.DefaultSiteCategoryID.SetDbValue(Rs, SiteCategoryType.DefaultSiteCategoryID.CurrentValue, System.DBNull.Value, False)
			Catch e As Exception
				If EW_DEBUG_ENABLED Then Throw
				Message = e.Message
				EditRow = False
			End Try
			RsEdit.Close()

			' Row Updating event
			bUpdateRow = SiteCategoryType.Row_Updating(RsOld, Rs)
			If bUpdateRow Then
				Try
					SiteCategoryType.Update(Rs)
					EditRow = True
				Catch e As Exception
					If EW_DEBUG_ENABLED Then Throw
					Message = e.Message
					EditRow = False
				End Try
			Else
				If SiteCategoryType.CancelMessage <> "" Then
					Message = SiteCategoryType.CancelMessage
					SiteCategoryType.CancelMessage = ""
				Else
					Message = Language.Phrase("UpdateCancelled")
				End If
				EditRow = False
			End If
		End If

		' Row Updated event
		If EditRow Then
			SiteCategoryType.Row_Updated(RsOld, Rs)
		End If
		If EditRow Then
			WriteAuditTrailOnEdit(RsOld, Rs)
		End If
	End Function

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "SiteCategoryType"
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
		Dim table As String = "SiteCategoryType"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsOld("SiteCategoryTypeID")

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
			fld = SiteCategoryType.FieldByName(fldname)
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
		SiteCategoryType_edit = New cSiteCategoryType_edit(Me)		
		SiteCategoryType_edit.Page_Init()		

		' Set buffer/cache option
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

		' Page main processing
		SiteCategoryType_edit.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If SiteCategoryType_edit IsNot Nothing Then SiteCategoryType_edit.Dispose()
	End Sub
End Class
