Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class linkcategory_edit
	Inherits AspNetMaker8_wpmWebsite

	' Page object
	Public LinkCategory_edit As cLinkCategory_edit

	'
	' Page Class
	'
	Class cLinkCategory_edit
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
				If LinkCategory.UseTokenInUrl Then Url = Url & "t=" & LinkCategory.TableVar & "&" ' Add page token
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
			If LinkCategory.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (LinkCategory.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (LinkCategory.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' ASP.NET page object
		Public ReadOnly Property AspNetPage() As linkcategory_edit
			Get
				Return CType(m_ParentPage, linkcategory_edit)
			End Get
		End Property

		' LinkCategory
		Public Property LinkCategory() As cLinkCategory
			Get				
				Return ParentPage.LinkCategory
			End Get
			Set(ByVal v As cLinkCategory)
				ParentPage.LinkCategory = v	
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
			m_PageObjName = "LinkCategory_edit"
			m_PageObjTypeName = "cLinkCategory_edit"			

			' Initialize language object
			Language = New cLanguage(Me)

			' Table Name
			m_TableName = "LinkCategory"

			' Initialize table object
			LinkCategory = New cLinkCategory(Me)

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
			LinkCategory.Dispose()
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
		If ew_Get("ID") <> "" Then
			LinkCategory.ID.QueryStringValue = ew_Get("ID")
		End If
		If ObjForm.GetValue("a_edit") <> "" Then
			LinkCategory.CurrentAction = ObjForm.GetValue("a_edit") ' Get action code
			LoadFormValues() ' Get form values

			' Validate Form
			If Not ValidateForm() Then
				LinkCategory.CurrentAction = "" ' Form error, reset action
				Message = ParentPage.gsFormError
				LinkCategory.EventCancelled = True ' Event cancelled
				LoadRow() ' Restore row
				RestoreFormValues() ' Restore form values if validate failed
			End If
		Else
			LinkCategory.CurrentAction = "I" ' Default action is display
		End If

		' Check if valid key
		If ew_Empty(LinkCategory.ID.CurrentValue) Then Page_Terminate("linkcategory_list.aspx") ' Invalid key, return to list
		Select Case LinkCategory.CurrentAction
			Case "I" ' Get a record to display
				If Not LoadRow() Then ' Load Record based on key
					Message = Language.Phrase("NoRecord") ' No record found
					Page_Terminate("linkcategory_list.aspx") ' No matching record, return to list
				End If
			Case "U" ' Update
				LinkCategory.SendEmail = True ' Send email on update success
				If EditRow() Then ' Update Record based on key
					Message = Language.Phrase("UpdateSuccess") ' Update success
					Dim sReturnUrl As String = LinkCategory.ReturnUrl
					Page_Terminate(sReturnUrl) ' Return to caller
				Else
					LinkCategory.EventCancelled = True ' Event cancelled 
					LoadRow() ' Restore row
					RestoreFormValues() ' Restore form values if update failed
				End If
		End Select

		' Render the record
		LinkCategory.RowType = EW_ROWTYPE_EDIT ' Render as edit

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
		LinkCategory.ID.FormValue = ObjForm.GetValue("x_ID")
		LinkCategory.ID.OldValue = ObjForm.GetValue("o_ID")
		LinkCategory.Title.FormValue = ObjForm.GetValue("x_Title")
		LinkCategory.Title.OldValue = ObjForm.GetValue("o_Title")
		LinkCategory.Description.FormValue = ObjForm.GetValue("x_Description")
		LinkCategory.Description.OldValue = ObjForm.GetValue("o_Description")
		LinkCategory.ParentID.FormValue = ObjForm.GetValue("x_ParentID")
		LinkCategory.ParentID.OldValue = ObjForm.GetValue("o_ParentID")
		LinkCategory.zPageID.FormValue = ObjForm.GetValue("x_zPageID")
		LinkCategory.zPageID.OldValue = ObjForm.GetValue("o_zPageID")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		LinkCategory.ID.CurrentValue = LinkCategory.ID.FormValue
		LinkCategory.Title.CurrentValue = LinkCategory.Title.FormValue
		LinkCategory.Description.CurrentValue = LinkCategory.Description.FormValue
		LinkCategory.ParentID.CurrentValue = LinkCategory.ParentID.FormValue
		LinkCategory.zPageID.CurrentValue = LinkCategory.zPageID.FormValue
	End Sub

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = LinkCategory.KeyFilter

		' Row Selecting event
		LinkCategory.Row_Selecting(sFilter)

		' Load SQL based on filter
		LinkCategory.CurrentFilter = sFilter
		Dim sSql As String = LinkCategory.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then DebugMsg = sSql
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				LinkCategory.Row_Selected(RsRow)
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
		LinkCategory.ID.DbValue = RsRow("ID")
		LinkCategory.Title.DbValue = RsRow("Title")
		LinkCategory.Description.DbValue = RsRow("Description")
		LinkCategory.ParentID.DbValue = RsRow("ParentID")
		LinkCategory.zPageID.DbValue = RsRow("PageID")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Initialize urls
		' Row Rendering event

		LinkCategory.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' ID

		LinkCategory.ID.CellCssStyle = ""
		LinkCategory.ID.CellCssClass = ""
		LinkCategory.ID.CellAttrs.Clear(): LinkCategory.ID.ViewAttrs.Clear(): LinkCategory.ID.EditAttrs.Clear()

		' Title
		LinkCategory.Title.CellCssStyle = ""
		LinkCategory.Title.CellCssClass = ""
		LinkCategory.Title.CellAttrs.Clear(): LinkCategory.Title.ViewAttrs.Clear(): LinkCategory.Title.EditAttrs.Clear()

		' Description
		LinkCategory.Description.CellCssStyle = ""
		LinkCategory.Description.CellCssClass = ""
		LinkCategory.Description.CellAttrs.Clear(): LinkCategory.Description.ViewAttrs.Clear(): LinkCategory.Description.EditAttrs.Clear()

		' ParentID
		LinkCategory.ParentID.CellCssStyle = ""
		LinkCategory.ParentID.CellCssClass = ""
		LinkCategory.ParentID.CellAttrs.Clear(): LinkCategory.ParentID.ViewAttrs.Clear(): LinkCategory.ParentID.EditAttrs.Clear()

		' PageID
		LinkCategory.zPageID.CellCssStyle = ""
		LinkCategory.zPageID.CellCssClass = ""
		LinkCategory.zPageID.CellAttrs.Clear(): LinkCategory.zPageID.ViewAttrs.Clear(): LinkCategory.zPageID.EditAttrs.Clear()

		'
		'  View  Row
		'

		If LinkCategory.RowType = EW_ROWTYPE_VIEW Then ' View row

			' ID
			LinkCategory.ID.ViewValue = LinkCategory.ID.CurrentValue
			LinkCategory.ID.CssStyle = ""
			LinkCategory.ID.CssClass = ""
			LinkCategory.ID.ViewCustomAttributes = ""

			' Title
			LinkCategory.Title.ViewValue = LinkCategory.Title.CurrentValue
			LinkCategory.Title.CssStyle = ""
			LinkCategory.Title.CssClass = ""
			LinkCategory.Title.ViewCustomAttributes = ""

			' Description
			LinkCategory.Description.ViewValue = LinkCategory.Description.CurrentValue
			LinkCategory.Description.CssStyle = ""
			LinkCategory.Description.CssClass = ""
			LinkCategory.Description.ViewCustomAttributes = ""

			' ParentID
			LinkCategory.ParentID.ViewValue = LinkCategory.ParentID.CurrentValue
			LinkCategory.ParentID.CssStyle = ""
			LinkCategory.ParentID.CssClass = ""
			LinkCategory.ParentID.ViewCustomAttributes = ""

			' PageID
			LinkCategory.zPageID.ViewValue = LinkCategory.zPageID.CurrentValue
			LinkCategory.zPageID.CssStyle = ""
			LinkCategory.zPageID.CssClass = ""
			LinkCategory.zPageID.ViewCustomAttributes = ""

			' View refer script
			' ID

			LinkCategory.ID.HrefValue = ""
			LinkCategory.ID.TooltipValue = ""

			' Title
			LinkCategory.Title.HrefValue = ""
			LinkCategory.Title.TooltipValue = ""

			' Description
			LinkCategory.Description.HrefValue = ""
			LinkCategory.Description.TooltipValue = ""

			' ParentID
			LinkCategory.ParentID.HrefValue = ""
			LinkCategory.ParentID.TooltipValue = ""

			' PageID
			LinkCategory.zPageID.HrefValue = ""
			LinkCategory.zPageID.TooltipValue = ""

		'
		'  Edit Row
		'

		ElseIf LinkCategory.RowType = EW_ROWTYPE_EDIT Then ' Edit row

			' ID
			LinkCategory.ID.EditCustomAttributes = ""
			LinkCategory.ID.EditValue = LinkCategory.ID.CurrentValue
			LinkCategory.ID.CssStyle = ""
			LinkCategory.ID.CssClass = ""
			LinkCategory.ID.ViewCustomAttributes = ""

			' Title
			LinkCategory.Title.EditCustomAttributes = ""
			LinkCategory.Title.EditValue = ew_HtmlEncode(LinkCategory.Title.CurrentValue)

			' Description
			LinkCategory.Description.EditCustomAttributes = ""
			LinkCategory.Description.EditValue = ew_HtmlEncode(LinkCategory.Description.CurrentValue)

			' ParentID
			LinkCategory.ParentID.EditCustomAttributes = ""
			LinkCategory.ParentID.EditValue = ew_HtmlEncode(LinkCategory.ParentID.CurrentValue)

			' PageID
			LinkCategory.zPageID.EditCustomAttributes = ""
			LinkCategory.zPageID.EditValue = ew_HtmlEncode(LinkCategory.zPageID.CurrentValue)

			' Edit refer script
			' ID

			LinkCategory.ID.HrefValue = ""

			' Title
			LinkCategory.Title.HrefValue = ""

			' Description
			LinkCategory.Description.HrefValue = ""

			' ParentID
			LinkCategory.ParentID.HrefValue = ""

			' PageID
			LinkCategory.zPageID.HrefValue = ""
		End If

		' Row Rendered event
		If LinkCategory.RowType <> EW_ROWTYPE_AGGREGATEINIT Then
			LinkCategory.Row_Rendered()
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
		If Not ew_CheckInteger(LinkCategory.ParentID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError &= LinkCategory.ParentID.FldErrMsg
		End If
		If Not ew_CheckInteger(LinkCategory.zPageID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError &= LinkCategory.zPageID.FldErrMsg
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
		sFilter = LinkCategory.KeyFilter
		LinkCategory.CurrentFilter  = sFilter
		sSql = LinkCategory.SQL
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

				' ID
				' Title

				LinkCategory.Title.SetDbValue(Rs, LinkCategory.Title.CurrentValue, System.DBNull.Value, False)

				' Description
				LinkCategory.Description.SetDbValue(Rs, LinkCategory.Description.CurrentValue, System.DBNull.Value, False)

				' ParentID
				LinkCategory.ParentID.SetDbValue(Rs, LinkCategory.ParentID.CurrentValue, System.DBNull.Value, False)

				' PageID
				LinkCategory.zPageID.SetDbValue(Rs, LinkCategory.zPageID.CurrentValue, System.DBNull.Value, False)
			Catch e As Exception
				If EW_DEBUG_ENABLED Then Throw
				Message = e.Message
				EditRow = False
			End Try
			RsEdit.Close()

			' Row Updating event
			bUpdateRow = LinkCategory.Row_Updating(RsOld, Rs)
			If bUpdateRow Then
				Try
					LinkCategory.Update(Rs)
					EditRow = True
				Catch e As Exception
					If EW_DEBUG_ENABLED Then Throw
					Message = e.Message
					EditRow = False
				End Try
			Else
				If LinkCategory.CancelMessage <> "" Then
					Message = LinkCategory.CancelMessage
					LinkCategory.CancelMessage = ""
				Else
					Message = Language.Phrase("UpdateCancelled")
				End If
				EditRow = False
			End If
		End If

		' Row Updated event
		If EditRow Then
			LinkCategory.Row_Updated(RsOld, Rs)
		End If
		If EditRow Then
			WriteAuditTrailOnEdit(RsOld, Rs)
		End If
	End Function

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "LinkCategory"
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
		Dim table As String = "LinkCategory"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsOld("ID")

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
			fld = LinkCategory.FieldByName(fldname)
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
		LinkCategory_edit = New cLinkCategory_edit(Me)		
		LinkCategory_edit.Page_Init()		

		' Set buffer/cache option
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

		' Page main processing
		LinkCategory_edit.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If LinkCategory_edit IsNot Nothing Then LinkCategory_edit.Dispose()
	End Sub
End Class
