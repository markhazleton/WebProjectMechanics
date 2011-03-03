Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class sitecategorygroup_edit
	Inherits AspNetMaker8_wpmWebsite

	' Page object
	Public SiteCategoryGroup_edit As cSiteCategoryGroup_edit

	'
	' Page Class
	'
	Class cSiteCategoryGroup_edit
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
				If SiteCategoryGroup.UseTokenInUrl Then Url = Url & "t=" & SiteCategoryGroup.TableVar & "&" ' Add page token
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
			If SiteCategoryGroup.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (SiteCategoryGroup.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (SiteCategoryGroup.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' ASP.NET page object
		Public ReadOnly Property AspNetPage() As sitecategorygroup_edit
			Get
				Return CType(m_ParentPage, sitecategorygroup_edit)
			End Get
		End Property

		' SiteCategoryGroup
		Public Property SiteCategoryGroup() As cSiteCategoryGroup
			Get				
				Return ParentPage.SiteCategoryGroup
			End Get
			Set(ByVal v As cSiteCategoryGroup)
				ParentPage.SiteCategoryGroup = v	
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
			m_PageObjName = "SiteCategoryGroup_edit"
			m_PageObjTypeName = "cSiteCategoryGroup_edit"			

			' Initialize language object
			Language = New cLanguage(Me)

			' Table Name
			m_TableName = "SiteCategoryGroup"

			' Initialize table object
			SiteCategoryGroup = New cSiteCategoryGroup(Me)

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
			SiteCategoryGroup.Dispose()
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
		If ew_Get("SiteCategoryGroupID") <> "" Then
			SiteCategoryGroup.SiteCategoryGroupID.QueryStringValue = ew_Get("SiteCategoryGroupID")
		End If
		If ObjForm.GetValue("a_edit") <> "" Then
			SiteCategoryGroup.CurrentAction = ObjForm.GetValue("a_edit") ' Get action code
			LoadFormValues() ' Get form values

			' Validate Form
			If Not ValidateForm() Then
				SiteCategoryGroup.CurrentAction = "" ' Form error, reset action
				Message = ParentPage.gsFormError
				SiteCategoryGroup.EventCancelled = True ' Event cancelled
				LoadRow() ' Restore row
				RestoreFormValues() ' Restore form values if validate failed
			End If
		Else
			SiteCategoryGroup.CurrentAction = "I" ' Default action is display
		End If

		' Check if valid key
		If ew_Empty(SiteCategoryGroup.SiteCategoryGroupID.CurrentValue) Then Page_Terminate("sitecategorygroup_list.aspx") ' Invalid key, return to list
		Select Case SiteCategoryGroup.CurrentAction
			Case "I" ' Get a record to display
				If Not LoadRow() Then ' Load Record based on key
					Message = Language.Phrase("NoRecord") ' No record found
					Page_Terminate("sitecategorygroup_list.aspx") ' No matching record, return to list
				End If
			Case "U" ' Update
				SiteCategoryGroup.SendEmail = True ' Send email on update success
				If EditRow() Then ' Update Record based on key
					Message = Language.Phrase("UpdateSuccess") ' Update success
					Dim sReturnUrl As String = SiteCategoryGroup.ReturnUrl
					Page_Terminate(sReturnUrl) ' Return to caller
				Else
					SiteCategoryGroup.EventCancelled = True ' Event cancelled 
					LoadRow() ' Restore row
					RestoreFormValues() ' Restore form values if update failed
				End If
		End Select

		' Render the record
		SiteCategoryGroup.RowType = EW_ROWTYPE_EDIT ' Render as edit

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
		SiteCategoryGroup.SiteCategoryGroupID.FormValue = ObjForm.GetValue("x_SiteCategoryGroupID")
		SiteCategoryGroup.SiteCategoryGroupID.OldValue = ObjForm.GetValue("o_SiteCategoryGroupID")
		SiteCategoryGroup.SiteCategoryGroupNM.FormValue = ObjForm.GetValue("x_SiteCategoryGroupNM")
		SiteCategoryGroup.SiteCategoryGroupNM.OldValue = ObjForm.GetValue("o_SiteCategoryGroupNM")
		SiteCategoryGroup.SiteCategoryGroupDS.FormValue = ObjForm.GetValue("x_SiteCategoryGroupDS")
		SiteCategoryGroup.SiteCategoryGroupDS.OldValue = ObjForm.GetValue("o_SiteCategoryGroupDS")
		SiteCategoryGroup.SiteCategoryGroupOrder.FormValue = ObjForm.GetValue("x_SiteCategoryGroupOrder")
		SiteCategoryGroup.SiteCategoryGroupOrder.OldValue = ObjForm.GetValue("o_SiteCategoryGroupOrder")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		SiteCategoryGroup.SiteCategoryGroupID.CurrentValue = SiteCategoryGroup.SiteCategoryGroupID.FormValue
		SiteCategoryGroup.SiteCategoryGroupNM.CurrentValue = SiteCategoryGroup.SiteCategoryGroupNM.FormValue
		SiteCategoryGroup.SiteCategoryGroupDS.CurrentValue = SiteCategoryGroup.SiteCategoryGroupDS.FormValue
		SiteCategoryGroup.SiteCategoryGroupOrder.CurrentValue = SiteCategoryGroup.SiteCategoryGroupOrder.FormValue
	End Sub

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = SiteCategoryGroup.KeyFilter

		' Row Selecting event
		SiteCategoryGroup.Row_Selecting(sFilter)

		' Load SQL based on filter
		SiteCategoryGroup.CurrentFilter = sFilter
		Dim sSql As String = SiteCategoryGroup.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then DebugMsg = sSql
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				SiteCategoryGroup.Row_Selected(RsRow)
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
		SiteCategoryGroup.SiteCategoryGroupID.DbValue = RsRow("SiteCategoryGroupID")
		SiteCategoryGroup.SiteCategoryGroupNM.DbValue = RsRow("SiteCategoryGroupNM")
		SiteCategoryGroup.SiteCategoryGroupDS.DbValue = RsRow("SiteCategoryGroupDS")
		SiteCategoryGroup.SiteCategoryGroupOrder.DbValue = RsRow("SiteCategoryGroupOrder")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Initialize urls
		' Row Rendering event

		SiteCategoryGroup.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' SiteCategoryGroupID

		SiteCategoryGroup.SiteCategoryGroupID.CellCssStyle = ""
		SiteCategoryGroup.SiteCategoryGroupID.CellCssClass = ""
		SiteCategoryGroup.SiteCategoryGroupID.CellAttrs.Clear(): SiteCategoryGroup.SiteCategoryGroupID.ViewAttrs.Clear(): SiteCategoryGroup.SiteCategoryGroupID.EditAttrs.Clear()

		' SiteCategoryGroupNM
		SiteCategoryGroup.SiteCategoryGroupNM.CellCssStyle = ""
		SiteCategoryGroup.SiteCategoryGroupNM.CellCssClass = ""
		SiteCategoryGroup.SiteCategoryGroupNM.CellAttrs.Clear(): SiteCategoryGroup.SiteCategoryGroupNM.ViewAttrs.Clear(): SiteCategoryGroup.SiteCategoryGroupNM.EditAttrs.Clear()

		' SiteCategoryGroupDS
		SiteCategoryGroup.SiteCategoryGroupDS.CellCssStyle = ""
		SiteCategoryGroup.SiteCategoryGroupDS.CellCssClass = ""
		SiteCategoryGroup.SiteCategoryGroupDS.CellAttrs.Clear(): SiteCategoryGroup.SiteCategoryGroupDS.ViewAttrs.Clear(): SiteCategoryGroup.SiteCategoryGroupDS.EditAttrs.Clear()

		' SiteCategoryGroupOrder
		SiteCategoryGroup.SiteCategoryGroupOrder.CellCssStyle = ""
		SiteCategoryGroup.SiteCategoryGroupOrder.CellCssClass = ""
		SiteCategoryGroup.SiteCategoryGroupOrder.CellAttrs.Clear(): SiteCategoryGroup.SiteCategoryGroupOrder.ViewAttrs.Clear(): SiteCategoryGroup.SiteCategoryGroupOrder.EditAttrs.Clear()

		'
		'  View  Row
		'

		If SiteCategoryGroup.RowType = EW_ROWTYPE_VIEW Then ' View row

			' SiteCategoryGroupID
			SiteCategoryGroup.SiteCategoryGroupID.ViewValue = SiteCategoryGroup.SiteCategoryGroupID.CurrentValue
			SiteCategoryGroup.SiteCategoryGroupID.CssStyle = ""
			SiteCategoryGroup.SiteCategoryGroupID.CssClass = ""
			SiteCategoryGroup.SiteCategoryGroupID.ViewCustomAttributes = ""

			' SiteCategoryGroupNM
			SiteCategoryGroup.SiteCategoryGroupNM.ViewValue = SiteCategoryGroup.SiteCategoryGroupNM.CurrentValue
			SiteCategoryGroup.SiteCategoryGroupNM.CssStyle = ""
			SiteCategoryGroup.SiteCategoryGroupNM.CssClass = ""
			SiteCategoryGroup.SiteCategoryGroupNM.ViewCustomAttributes = ""

			' SiteCategoryGroupDS
			SiteCategoryGroup.SiteCategoryGroupDS.ViewValue = SiteCategoryGroup.SiteCategoryGroupDS.CurrentValue
			SiteCategoryGroup.SiteCategoryGroupDS.CssStyle = ""
			SiteCategoryGroup.SiteCategoryGroupDS.CssClass = ""
			SiteCategoryGroup.SiteCategoryGroupDS.ViewCustomAttributes = ""

			' SiteCategoryGroupOrder
			SiteCategoryGroup.SiteCategoryGroupOrder.ViewValue = SiteCategoryGroup.SiteCategoryGroupOrder.CurrentValue
			SiteCategoryGroup.SiteCategoryGroupOrder.CssStyle = ""
			SiteCategoryGroup.SiteCategoryGroupOrder.CssClass = ""
			SiteCategoryGroup.SiteCategoryGroupOrder.ViewCustomAttributes = ""

			' View refer script
			' SiteCategoryGroupID

			SiteCategoryGroup.SiteCategoryGroupID.HrefValue = ""
			SiteCategoryGroup.SiteCategoryGroupID.TooltipValue = ""

			' SiteCategoryGroupNM
			SiteCategoryGroup.SiteCategoryGroupNM.HrefValue = ""
			SiteCategoryGroup.SiteCategoryGroupNM.TooltipValue = ""

			' SiteCategoryGroupDS
			SiteCategoryGroup.SiteCategoryGroupDS.HrefValue = ""
			SiteCategoryGroup.SiteCategoryGroupDS.TooltipValue = ""

			' SiteCategoryGroupOrder
			SiteCategoryGroup.SiteCategoryGroupOrder.HrefValue = ""
			SiteCategoryGroup.SiteCategoryGroupOrder.TooltipValue = ""

		'
		'  Edit Row
		'

		ElseIf SiteCategoryGroup.RowType = EW_ROWTYPE_EDIT Then ' Edit row

			' SiteCategoryGroupID
			SiteCategoryGroup.SiteCategoryGroupID.EditCustomAttributes = ""
			SiteCategoryGroup.SiteCategoryGroupID.EditValue = SiteCategoryGroup.SiteCategoryGroupID.CurrentValue
			SiteCategoryGroup.SiteCategoryGroupID.CssStyle = ""
			SiteCategoryGroup.SiteCategoryGroupID.CssClass = ""
			SiteCategoryGroup.SiteCategoryGroupID.ViewCustomAttributes = ""

			' SiteCategoryGroupNM
			SiteCategoryGroup.SiteCategoryGroupNM.EditCustomAttributes = ""
			SiteCategoryGroup.SiteCategoryGroupNM.EditValue = ew_HtmlEncode(SiteCategoryGroup.SiteCategoryGroupNM.CurrentValue)

			' SiteCategoryGroupDS
			SiteCategoryGroup.SiteCategoryGroupDS.EditCustomAttributes = ""
			SiteCategoryGroup.SiteCategoryGroupDS.EditValue = ew_HtmlEncode(SiteCategoryGroup.SiteCategoryGroupDS.CurrentValue)

			' SiteCategoryGroupOrder
			SiteCategoryGroup.SiteCategoryGroupOrder.EditCustomAttributes = ""
			SiteCategoryGroup.SiteCategoryGroupOrder.EditValue = ew_HtmlEncode(SiteCategoryGroup.SiteCategoryGroupOrder.CurrentValue)

			' Edit refer script
			' SiteCategoryGroupID

			SiteCategoryGroup.SiteCategoryGroupID.HrefValue = ""

			' SiteCategoryGroupNM
			SiteCategoryGroup.SiteCategoryGroupNM.HrefValue = ""

			' SiteCategoryGroupDS
			SiteCategoryGroup.SiteCategoryGroupDS.HrefValue = ""

			' SiteCategoryGroupOrder
			SiteCategoryGroup.SiteCategoryGroupOrder.HrefValue = ""
		End If

		' Row Rendered event
		If SiteCategoryGroup.RowType <> EW_ROWTYPE_AGGREGATEINIT Then
			SiteCategoryGroup.Row_Rendered()
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
		If Not ew_CheckInteger(SiteCategoryGroup.SiteCategoryGroupOrder.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError &= SiteCategoryGroup.SiteCategoryGroupOrder.FldErrMsg
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
		sFilter = SiteCategoryGroup.KeyFilter
		SiteCategoryGroup.CurrentFilter  = sFilter
		sSql = SiteCategoryGroup.SQL
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

				' SiteCategoryGroupID
				' SiteCategoryGroupNM

				SiteCategoryGroup.SiteCategoryGroupNM.SetDbValue(Rs, SiteCategoryGroup.SiteCategoryGroupNM.CurrentValue, System.DBNull.Value, False)

				' SiteCategoryGroupDS
				SiteCategoryGroup.SiteCategoryGroupDS.SetDbValue(Rs, SiteCategoryGroup.SiteCategoryGroupDS.CurrentValue, System.DBNull.Value, False)

				' SiteCategoryGroupOrder
				SiteCategoryGroup.SiteCategoryGroupOrder.SetDbValue(Rs, SiteCategoryGroup.SiteCategoryGroupOrder.CurrentValue, System.DBNull.Value, False)
			Catch e As Exception
				If EW_DEBUG_ENABLED Then Throw
				Message = e.Message
				EditRow = False
			End Try
			RsEdit.Close()

			' Row Updating event
			bUpdateRow = SiteCategoryGroup.Row_Updating(RsOld, Rs)
			If bUpdateRow Then
				Try
					SiteCategoryGroup.Update(Rs)
					EditRow = True
				Catch e As Exception
					If EW_DEBUG_ENABLED Then Throw
					Message = e.Message
					EditRow = False
				End Try
			Else
				If SiteCategoryGroup.CancelMessage <> "" Then
					Message = SiteCategoryGroup.CancelMessage
					SiteCategoryGroup.CancelMessage = ""
				Else
					Message = Language.Phrase("UpdateCancelled")
				End If
				EditRow = False
			End If
		End If

		' Row Updated event
		If EditRow Then
			SiteCategoryGroup.Row_Updated(RsOld, Rs)
		End If
		If EditRow Then
			WriteAuditTrailOnEdit(RsOld, Rs)
		End If
	End Function

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "SiteCategoryGroup"
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
		Dim table As String = "SiteCategoryGroup"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsOld("SiteCategoryGroupID")

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
			fld = SiteCategoryGroup.FieldByName(fldname)
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
		SiteCategoryGroup_edit = New cSiteCategoryGroup_edit(Me)		
		SiteCategoryGroup_edit.Page_Init()		

		' Set buffer/cache option
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

		' Page main processing
		SiteCategoryGroup_edit.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If SiteCategoryGroup_edit IsNot Nothing Then SiteCategoryGroup_edit.Dispose()
	End Sub
End Class
