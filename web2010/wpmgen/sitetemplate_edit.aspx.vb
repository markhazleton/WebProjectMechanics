Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class sitetemplate_edit
	Inherits AspNetMaker8_wpmWebsite

	' Page object
	Public SiteTemplate_edit As cSiteTemplate_edit

	'
	' Page Class
	'
	Class cSiteTemplate_edit
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
				If SiteTemplate.UseTokenInUrl Then Url = Url & "t=" & SiteTemplate.TableVar & "&" ' Add page token
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
			If SiteTemplate.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (SiteTemplate.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (SiteTemplate.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' ASP.NET page object
		Public ReadOnly Property AspNetPage() As sitetemplate_edit
			Get
				Return CType(m_ParentPage, sitetemplate_edit)
			End Get
		End Property

		' SiteTemplate
		Public Property SiteTemplate() As cSiteTemplate
			Get				
				Return ParentPage.SiteTemplate
			End Get
			Set(ByVal v As cSiteTemplate)
				ParentPage.SiteTemplate = v	
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
			m_PageObjName = "SiteTemplate_edit"
			m_PageObjTypeName = "cSiteTemplate_edit"			

			' Initialize language object
			Language = New cLanguage(Me)

			' Table Name
			m_TableName = "SiteTemplate"

			' Initialize table object
			SiteTemplate = New cSiteTemplate(Me)

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
			SiteTemplate.Dispose()
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
		If ew_Get("TemplatePrefix") <> "" Then
			SiteTemplate.TemplatePrefix.QueryStringValue = ew_Get("TemplatePrefix")
		End If
		If ObjForm.GetValue("a_edit") <> "" Then
			SiteTemplate.CurrentAction = ObjForm.GetValue("a_edit") ' Get action code
			LoadFormValues() ' Get form values

			' Validate Form
			If Not ValidateForm() Then
				SiteTemplate.CurrentAction = "" ' Form error, reset action
				Message = ParentPage.gsFormError
				SiteTemplate.EventCancelled = True ' Event cancelled
				LoadRow() ' Restore row
				RestoreFormValues() ' Restore form values if validate failed
			End If
		Else
			SiteTemplate.CurrentAction = "I" ' Default action is display
		End If

		' Check if valid key
		If ew_Empty(SiteTemplate.TemplatePrefix.CurrentValue) Then Page_Terminate("sitetemplate_list.aspx") ' Invalid key, return to list
		Select Case SiteTemplate.CurrentAction
			Case "I" ' Get a record to display
				If Not LoadRow() Then ' Load Record based on key
					Message = Language.Phrase("NoRecord") ' No record found
					Page_Terminate("sitetemplate_list.aspx") ' No matching record, return to list
				End If
			Case "U" ' Update
				SiteTemplate.SendEmail = True ' Send email on update success
				If EditRow() Then ' Update Record based on key
					Message = Language.Phrase("UpdateSuccess") ' Update success
					Dim sReturnUrl As String = SiteTemplate.ReturnUrl
					Page_Terminate(sReturnUrl) ' Return to caller
				Else
					SiteTemplate.EventCancelled = True ' Event cancelled 
					LoadRow() ' Restore row
					RestoreFormValues() ' Restore form values if update failed
				End If
		End Select

		' Render the record
		SiteTemplate.RowType = EW_ROWTYPE_EDIT ' Render as edit

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
		SiteTemplate.TemplatePrefix.FormValue = ObjForm.GetValue("x_TemplatePrefix")
		SiteTemplate.TemplatePrefix.OldValue = ObjForm.GetValue("o_TemplatePrefix")
		SiteTemplate.zName.FormValue = ObjForm.GetValue("x_zName")
		SiteTemplate.zName.OldValue = ObjForm.GetValue("o_zName")
		SiteTemplate.Top.FormValue = ObjForm.GetValue("x_Top")
		SiteTemplate.Top.OldValue = ObjForm.GetValue("o_Top")
		SiteTemplate.Bottom.FormValue = ObjForm.GetValue("x_Bottom")
		SiteTemplate.Bottom.OldValue = ObjForm.GetValue("o_Bottom")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		SiteTemplate.TemplatePrefix.CurrentValue = SiteTemplate.TemplatePrefix.FormValue
		SiteTemplate.zName.CurrentValue = SiteTemplate.zName.FormValue
		SiteTemplate.Top.CurrentValue = SiteTemplate.Top.FormValue
		SiteTemplate.Bottom.CurrentValue = SiteTemplate.Bottom.FormValue
	End Sub

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = SiteTemplate.KeyFilter

		' Row Selecting event
		SiteTemplate.Row_Selecting(sFilter)

		' Load SQL based on filter
		SiteTemplate.CurrentFilter = sFilter
		Dim sSql As String = SiteTemplate.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then DebugMsg = sSql
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				SiteTemplate.Row_Selected(RsRow)
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
		SiteTemplate.TemplatePrefix.DbValue = RsRow("TemplatePrefix")
		SiteTemplate.zName.DbValue = RsRow("Name")
		SiteTemplate.CSS.DbValue = RsRow("CSS")
		SiteTemplate.Top.DbValue = RsRow("Top")
		SiteTemplate.Bottom.DbValue = RsRow("Bottom")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Initialize urls
		' Row Rendering event

		SiteTemplate.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' TemplatePrefix

		SiteTemplate.TemplatePrefix.CellCssStyle = ""
		SiteTemplate.TemplatePrefix.CellCssClass = ""
		SiteTemplate.TemplatePrefix.CellAttrs.Clear(): SiteTemplate.TemplatePrefix.ViewAttrs.Clear(): SiteTemplate.TemplatePrefix.EditAttrs.Clear()

		' Name
		SiteTemplate.zName.CellCssStyle = ""
		SiteTemplate.zName.CellCssClass = ""
		SiteTemplate.zName.CellAttrs.Clear(): SiteTemplate.zName.ViewAttrs.Clear(): SiteTemplate.zName.EditAttrs.Clear()

		' Top
		SiteTemplate.Top.CellCssStyle = ""
		SiteTemplate.Top.CellCssClass = ""
		SiteTemplate.Top.CellAttrs.Clear(): SiteTemplate.Top.ViewAttrs.Clear(): SiteTemplate.Top.EditAttrs.Clear()

		' Bottom
		SiteTemplate.Bottom.CellCssStyle = ""
		SiteTemplate.Bottom.CellCssClass = ""
		SiteTemplate.Bottom.CellAttrs.Clear(): SiteTemplate.Bottom.ViewAttrs.Clear(): SiteTemplate.Bottom.EditAttrs.Clear()

		'
		'  View  Row
		'

		If SiteTemplate.RowType = EW_ROWTYPE_VIEW Then ' View row

			' TemplatePrefix
			SiteTemplate.TemplatePrefix.ViewValue = SiteTemplate.TemplatePrefix.CurrentValue
			SiteTemplate.TemplatePrefix.CssStyle = ""
			SiteTemplate.TemplatePrefix.CssClass = ""
			SiteTemplate.TemplatePrefix.ViewCustomAttributes = ""

			' Name
			SiteTemplate.zName.ViewValue = SiteTemplate.zName.CurrentValue
			SiteTemplate.zName.CssStyle = ""
			SiteTemplate.zName.CssClass = ""
			SiteTemplate.zName.ViewCustomAttributes = ""

			' Top
			SiteTemplate.Top.ViewValue = SiteTemplate.Top.CurrentValue
			SiteTemplate.Top.CssStyle = ""
			SiteTemplate.Top.CssClass = ""
			SiteTemplate.Top.ViewCustomAttributes = ""

			' Bottom
			SiteTemplate.Bottom.ViewValue = SiteTemplate.Bottom.CurrentValue
			SiteTemplate.Bottom.CssStyle = ""
			SiteTemplate.Bottom.CssClass = ""
			SiteTemplate.Bottom.ViewCustomAttributes = ""

			' View refer script
			' TemplatePrefix

			SiteTemplate.TemplatePrefix.HrefValue = ""
			SiteTemplate.TemplatePrefix.TooltipValue = ""

			' Name
			SiteTemplate.zName.HrefValue = ""
			SiteTemplate.zName.TooltipValue = ""

			' Top
			SiteTemplate.Top.HrefValue = ""
			SiteTemplate.Top.TooltipValue = ""

			' Bottom
			SiteTemplate.Bottom.HrefValue = ""
			SiteTemplate.Bottom.TooltipValue = ""

		'
		'  Edit Row
		'

		ElseIf SiteTemplate.RowType = EW_ROWTYPE_EDIT Then ' Edit row

			' TemplatePrefix
			SiteTemplate.TemplatePrefix.EditCustomAttributes = ""
			SiteTemplate.TemplatePrefix.EditValue = SiteTemplate.TemplatePrefix.CurrentValue
			SiteTemplate.TemplatePrefix.CssStyle = ""
			SiteTemplate.TemplatePrefix.CssClass = ""
			SiteTemplate.TemplatePrefix.ViewCustomAttributes = ""

			' Name
			SiteTemplate.zName.EditCustomAttributes = ""
			SiteTemplate.zName.EditValue = ew_HtmlEncode(SiteTemplate.zName.CurrentValue)

			' Top
			SiteTemplate.Top.EditCustomAttributes = ""
			SiteTemplate.Top.EditValue = ew_HtmlEncode(SiteTemplate.Top.CurrentValue)

			' Bottom
			SiteTemplate.Bottom.EditCustomAttributes = ""
			SiteTemplate.Bottom.EditValue = ew_HtmlEncode(SiteTemplate.Bottom.CurrentValue)

			' Edit refer script
			' TemplatePrefix

			SiteTemplate.TemplatePrefix.HrefValue = ""

			' Name
			SiteTemplate.zName.HrefValue = ""

			' Top
			SiteTemplate.Top.HrefValue = ""

			' Bottom
			SiteTemplate.Bottom.HrefValue = ""
		End If

		' Row Rendered event
		If SiteTemplate.RowType <> EW_ROWTYPE_AGGREGATEINIT Then
			SiteTemplate.Row_Rendered()
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
		If ew_Empty(SiteTemplate.zName.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError &= Language.Phrase("EnterRequiredField") & " - " & SiteTemplate.zName.FldCaption
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
		sFilter = SiteTemplate.KeyFilter
		If SiteTemplate.TemplatePrefix.CurrentValue <> "" Then ' Check field with unique index
			sFilterChk = "([TemplatePrefix] = '" & ew_AdjustSql(SiteTemplate.TemplatePrefix.CurrentValue) & "')"
			sFilterChk = sFilterChk & " AND NOT (" & sFilter & ")"
			SiteTemplate.CurrentFilter = sFilterChk
			sSqlChk = SiteTemplate.SQL
			Try
				RsChk = Conn.GetDataReader(sSqlChk)
				If RsChk.Read() Then
					sIdxErrMsg = Language.Phrase("DupIndex").Replace("%f", "TemplatePrefix")
					sIdxErrMsg = sIdxErrMsg.Replace("%v", SiteTemplate.TemplatePrefix.CurrentValue)
					Message = sIdxErrMsg			
					Return False
				End If
			Catch e As Exception
				If EW_DEBUG_ENABLED Then Throw
				Message = e.Message
				Return False
			Finally
				If RsChk IsNot Nothing Then
					RsChk.Close()
					RsChk.Dispose()
				End If
			End Try				
		End If
		If SiteTemplate.zName.CurrentValue <> "" Then ' Check field with unique index
			sFilterChk = "([Name] = '" & ew_AdjustSql(SiteTemplate.zName.CurrentValue) & "')"
			sFilterChk = sFilterChk & " AND NOT (" & sFilter & ")"
			SiteTemplate.CurrentFilter = sFilterChk
			sSqlChk = SiteTemplate.SQL
			Try
				RsChk = Conn.GetDataReader(sSqlChk)
				If RsChk.Read() Then
					sIdxErrMsg = Language.Phrase("DupIndex").Replace("%f", "Name")
					sIdxErrMsg = sIdxErrMsg.Replace("%v", SiteTemplate.zName.CurrentValue)
					Message = sIdxErrMsg			
					Return False
				End If
			Catch e As Exception
				If EW_DEBUG_ENABLED Then Throw
				Message = e.Message
				Return False
			Finally
				If RsChk IsNot Nothing Then
					RsChk.Close()
					RsChk.Dispose()
				End If
			End Try				
		End If
		SiteTemplate.CurrentFilter  = sFilter
		sSql = SiteTemplate.SQL
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

				' TemplatePrefix
				' Name

				SiteTemplate.zName.SetDbValue(Rs, SiteTemplate.zName.CurrentValue, "", False)

				' Top
				SiteTemplate.Top.SetDbValue(Rs, SiteTemplate.Top.CurrentValue, System.DBNull.Value, False)

				' Bottom
				SiteTemplate.Bottom.SetDbValue(Rs, SiteTemplate.Bottom.CurrentValue, System.DBNull.Value, False)
			Catch e As Exception
				If EW_DEBUG_ENABLED Then Throw
				Message = e.Message
				EditRow = False
			End Try
			RsEdit.Close()

			' Row Updating event
			bUpdateRow = SiteTemplate.Row_Updating(RsOld, Rs)
			If bUpdateRow Then
				Try
					SiteTemplate.Update(Rs)
					EditRow = True
				Catch e As Exception
					If EW_DEBUG_ENABLED Then Throw
					Message = e.Message
					EditRow = False
				End Try
			Else
				If SiteTemplate.CancelMessage <> "" Then
					Message = SiteTemplate.CancelMessage
					SiteTemplate.CancelMessage = ""
				Else
					Message = Language.Phrase("UpdateCancelled")
				End If
				EditRow = False
			End If
		End If

		' Row Updated event
		If EditRow Then
			SiteTemplate.Row_Updated(RsOld, Rs)
		End If
		If EditRow Then
			WriteAuditTrailOnEdit(RsOld, Rs)
		End If
	End Function

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "SiteTemplate"
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
		Dim table As String = "SiteTemplate"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsOld("TemplatePrefix")

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
			fld = SiteTemplate.FieldByName(fldname)
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
		SiteTemplate_edit = New cSiteTemplate_edit(Me)		
		SiteTemplate_edit.Page_Init()		

		' Set buffer/cache option
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

		' Page main processing
		SiteTemplate_edit.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If SiteTemplate_edit IsNot Nothing Then SiteTemplate_edit.Dispose()
	End Sub
End Class
