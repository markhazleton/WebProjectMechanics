Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class siteparametertype_add
	Inherits AspNetMaker8_wpmWebsite

	' Page object
	Public SiteParameterType_add As cSiteParameterType_add

	'
	' Page Class
	'
	Class cSiteParameterType_add
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
				If SiteParameterType.UseTokenInUrl Then Url = Url & "t=" & SiteParameterType.TableVar & "&" ' Add page token
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
			If SiteParameterType.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (SiteParameterType.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (SiteParameterType.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' ASP.NET page object
		Public ReadOnly Property AspNetPage() As siteparametertype_add
			Get
				Return CType(m_ParentPage, siteparametertype_add)
			End Get
		End Property

		' SiteParameterType
		Public Property SiteParameterType() As cSiteParameterType
			Get				
				Return ParentPage.SiteParameterType
			End Get
			Set(ByVal v As cSiteParameterType)
				ParentPage.SiteParameterType = v	
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
			m_PageID = "add"
			m_PageObjName = "SiteParameterType_add"
			m_PageObjTypeName = "cSiteParameterType_add"			

			' Initialize language object
			Language = New cLanguage(Me)

			' Table Name
			m_TableName = "SiteParameterType"

			' Initialize table object
			SiteParameterType = New cSiteParameterType(Me)

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
			SiteParameterType.Dispose()
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

	Public x_ewPriv As Integer

	'
	' Page main processing
	'
	Sub Page_Main()

		' Load key values from QueryString
		Dim bCopy As Boolean = True
		If ew_Get("SiteParameterTypeID") <> "" Then
			SiteParameterType.SiteParameterTypeID.QueryStringValue = ew_Get("SiteParameterTypeID")
		Else
			bCopy = False
		End If

		' Process form if post back
		If ObjForm.GetValue("a_add") <> "" Then
			SiteParameterType.CurrentAction = ObjForm.GetValue("a_add") ' Get form action
			LoadFormValues() ' Load form values

			' Validate Form
			If Not ValidateForm() Then
				SiteParameterType.CurrentAction = "I" ' Form error, reset action
				Message = ParentPage.gsFormError
			End If

		' Not post back
		Else
			If bCopy Then
				SiteParameterType.CurrentAction = "C" ' Copy Record
			Else
				SiteParameterType.CurrentAction = "I" ' Display Blank Record
				LoadDefaultValues() ' Load default values
			End If
		End If

		' Perform action based on action code
		Select Case SiteParameterType.CurrentAction
			Case "I" ' Blank record, no action required
			Case "C" ' Copy an existing record
				If Not LoadRow() Then ' Load record based on key
					Message = Language.Phrase("NoRecord") ' No record found
					Page_Terminate("siteparametertype_list.aspx") ' No matching record, return to list
				End If
			Case "A" ' Add new record
				SiteParameterType.SendEmail = True ' Send email on add success
				If AddRow() Then ' Add successful
					Message = Language.Phrase("AddSuccess") ' Set up success message
					Dim sReturnUrl As String = SiteParameterType.ReturnUrl
					Page_Terminate(sReturnUrl) ' Clean up and return
				Else
					RestoreFormValues() ' Add failed, restore form values
				End If
		End Select

		' Render row based on row type
		SiteParameterType.RowType = EW_ROWTYPE_ADD ' Render add type

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
		SiteParameterType.SiteParameterTypeNM.FormValue = ObjForm.GetValue("x_SiteParameterTypeNM")
		SiteParameterType.SiteParameterTypeNM.OldValue = ObjForm.GetValue("o_SiteParameterTypeNM")
		SiteParameterType.SiteParameterTypeDS.FormValue = ObjForm.GetValue("x_SiteParameterTypeDS")
		SiteParameterType.SiteParameterTypeDS.OldValue = ObjForm.GetValue("o_SiteParameterTypeDS")
		SiteParameterType.SiteParameterTypeOrder.FormValue = ObjForm.GetValue("x_SiteParameterTypeOrder")
		SiteParameterType.SiteParameterTypeOrder.OldValue = ObjForm.GetValue("o_SiteParameterTypeOrder")
		SiteParameterType.SiteParameterTemplate.FormValue = ObjForm.GetValue("x_SiteParameterTemplate")
		SiteParameterType.SiteParameterTemplate.OldValue = ObjForm.GetValue("o_SiteParameterTemplate")
		SiteParameterType.SiteParameterTypeID.FormValue = ObjForm.GetValue("x_SiteParameterTypeID")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		SiteParameterType.SiteParameterTypeNM.CurrentValue = SiteParameterType.SiteParameterTypeNM.FormValue
		SiteParameterType.SiteParameterTypeDS.CurrentValue = SiteParameterType.SiteParameterTypeDS.FormValue
		SiteParameterType.SiteParameterTypeOrder.CurrentValue = SiteParameterType.SiteParameterTypeOrder.FormValue
		SiteParameterType.SiteParameterTemplate.CurrentValue = SiteParameterType.SiteParameterTemplate.FormValue
		SiteParameterType.SiteParameterTypeID.CurrentValue = SiteParameterType.SiteParameterTypeID.FormValue
	End Sub

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = SiteParameterType.KeyFilter

		' Row Selecting event
		SiteParameterType.Row_Selecting(sFilter)

		' Load SQL based on filter
		SiteParameterType.CurrentFilter = sFilter
		Dim sSql As String = SiteParameterType.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then DebugMsg = sSql
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				SiteParameterType.Row_Selected(RsRow)
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
		SiteParameterType.SiteParameterTypeID.DbValue = RsRow("SiteParameterTypeID")
		SiteParameterType.SiteParameterTypeNM.DbValue = RsRow("SiteParameterTypeNM")
		SiteParameterType.SiteParameterTypeDS.DbValue = RsRow("SiteParameterTypeDS")
		SiteParameterType.SiteParameterTypeOrder.DbValue = RsRow("SiteParameterTypeOrder")
		SiteParameterType.SiteParameterTemplate.DbValue = RsRow("SiteParameterTemplate")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Initialize urls
		' Row Rendering event

		SiteParameterType.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' SiteParameterTypeNM

		SiteParameterType.SiteParameterTypeNM.CellCssStyle = ""
		SiteParameterType.SiteParameterTypeNM.CellCssClass = ""
		SiteParameterType.SiteParameterTypeNM.CellAttrs.Clear(): SiteParameterType.SiteParameterTypeNM.ViewAttrs.Clear(): SiteParameterType.SiteParameterTypeNM.EditAttrs.Clear()

		' SiteParameterTypeDS
		SiteParameterType.SiteParameterTypeDS.CellCssStyle = ""
		SiteParameterType.SiteParameterTypeDS.CellCssClass = ""
		SiteParameterType.SiteParameterTypeDS.CellAttrs.Clear(): SiteParameterType.SiteParameterTypeDS.ViewAttrs.Clear(): SiteParameterType.SiteParameterTypeDS.EditAttrs.Clear()

		' SiteParameterTypeOrder
		SiteParameterType.SiteParameterTypeOrder.CellCssStyle = ""
		SiteParameterType.SiteParameterTypeOrder.CellCssClass = ""
		SiteParameterType.SiteParameterTypeOrder.CellAttrs.Clear(): SiteParameterType.SiteParameterTypeOrder.ViewAttrs.Clear(): SiteParameterType.SiteParameterTypeOrder.EditAttrs.Clear()

		' SiteParameterTemplate
		SiteParameterType.SiteParameterTemplate.CellCssStyle = ""
		SiteParameterType.SiteParameterTemplate.CellCssClass = ""
		SiteParameterType.SiteParameterTemplate.CellAttrs.Clear(): SiteParameterType.SiteParameterTemplate.ViewAttrs.Clear(): SiteParameterType.SiteParameterTemplate.EditAttrs.Clear()

		'
		'  View  Row
		'

		If SiteParameterType.RowType = EW_ROWTYPE_VIEW Then ' View row

			' SiteParameterTypeID
			SiteParameterType.SiteParameterTypeID.ViewValue = SiteParameterType.SiteParameterTypeID.CurrentValue
			SiteParameterType.SiteParameterTypeID.CssStyle = ""
			SiteParameterType.SiteParameterTypeID.CssClass = ""
			SiteParameterType.SiteParameterTypeID.ViewCustomAttributes = ""

			' SiteParameterTypeNM
			SiteParameterType.SiteParameterTypeNM.ViewValue = SiteParameterType.SiteParameterTypeNM.CurrentValue
			SiteParameterType.SiteParameterTypeNM.CssStyle = ""
			SiteParameterType.SiteParameterTypeNM.CssClass = ""
			SiteParameterType.SiteParameterTypeNM.ViewCustomAttributes = ""

			' SiteParameterTypeDS
			SiteParameterType.SiteParameterTypeDS.ViewValue = SiteParameterType.SiteParameterTypeDS.CurrentValue
			SiteParameterType.SiteParameterTypeDS.CssStyle = ""
			SiteParameterType.SiteParameterTypeDS.CssClass = ""
			SiteParameterType.SiteParameterTypeDS.ViewCustomAttributes = ""

			' SiteParameterTypeOrder
			SiteParameterType.SiteParameterTypeOrder.ViewValue = SiteParameterType.SiteParameterTypeOrder.CurrentValue
			SiteParameterType.SiteParameterTypeOrder.CssStyle = ""
			SiteParameterType.SiteParameterTypeOrder.CssClass = ""
			SiteParameterType.SiteParameterTypeOrder.ViewCustomAttributes = ""

			' SiteParameterTemplate
			SiteParameterType.SiteParameterTemplate.ViewValue = SiteParameterType.SiteParameterTemplate.CurrentValue
			SiteParameterType.SiteParameterTemplate.CssStyle = ""
			SiteParameterType.SiteParameterTemplate.CssClass = ""
			SiteParameterType.SiteParameterTemplate.ViewCustomAttributes = ""

			' View refer script
			' SiteParameterTypeNM

			SiteParameterType.SiteParameterTypeNM.HrefValue = ""
			SiteParameterType.SiteParameterTypeNM.TooltipValue = ""

			' SiteParameterTypeDS
			SiteParameterType.SiteParameterTypeDS.HrefValue = ""
			SiteParameterType.SiteParameterTypeDS.TooltipValue = ""

			' SiteParameterTypeOrder
			SiteParameterType.SiteParameterTypeOrder.HrefValue = ""
			SiteParameterType.SiteParameterTypeOrder.TooltipValue = ""

			' SiteParameterTemplate
			SiteParameterType.SiteParameterTemplate.HrefValue = ""
			SiteParameterType.SiteParameterTemplate.TooltipValue = ""

		'
		'  Add Row
		'

		ElseIf SiteParameterType.RowType = EW_ROWTYPE_ADD Then ' Add row

			' SiteParameterTypeNM
			SiteParameterType.SiteParameterTypeNM.EditCustomAttributes = ""
			SiteParameterType.SiteParameterTypeNM.EditValue = ew_HtmlEncode(SiteParameterType.SiteParameterTypeNM.CurrentValue)

			' SiteParameterTypeDS
			SiteParameterType.SiteParameterTypeDS.EditCustomAttributes = ""
			SiteParameterType.SiteParameterTypeDS.EditValue = ew_HtmlEncode(SiteParameterType.SiteParameterTypeDS.CurrentValue)

			' SiteParameterTypeOrder
			SiteParameterType.SiteParameterTypeOrder.EditCustomAttributes = ""
			SiteParameterType.SiteParameterTypeOrder.EditValue = ew_HtmlEncode(SiteParameterType.SiteParameterTypeOrder.CurrentValue)

			' SiteParameterTemplate
			SiteParameterType.SiteParameterTemplate.EditCustomAttributes = ""
			SiteParameterType.SiteParameterTemplate.EditValue = ew_HtmlEncode(SiteParameterType.SiteParameterTemplate.CurrentValue)
		End If

		' Row Rendered event
		If SiteParameterType.RowType <> EW_ROWTYPE_AGGREGATEINIT Then
			SiteParameterType.Row_Rendered()
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
		If Not ew_CheckInteger(SiteParameterType.SiteParameterTypeOrder.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError &= SiteParameterType.SiteParameterTypeOrder.FldErrMsg
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
		Try

			' SiteParameterTypeNM
			SiteParameterType.SiteParameterTypeNM.SetDbValue(Rs, SiteParameterType.SiteParameterTypeNM.CurrentValue, System.DBNull.Value, False)

			' SiteParameterTypeDS
			SiteParameterType.SiteParameterTypeDS.SetDbValue(Rs, SiteParameterType.SiteParameterTypeDS.CurrentValue, System.DBNull.Value, False)

			' SiteParameterTypeOrder
			SiteParameterType.SiteParameterTypeOrder.SetDbValue(Rs, SiteParameterType.SiteParameterTypeOrder.CurrentValue, System.DBNull.Value, False)

			' SiteParameterTemplate
			SiteParameterType.SiteParameterTemplate.SetDbValue(Rs, SiteParameterType.SiteParameterTemplate.CurrentValue, System.DBNull.Value, False)
		Catch e As Exception
			If EW_DEBUG_ENABLED Then Throw
			Message = e.Message
			Return False
		End Try

		' Row Inserting event
		bInsertRow = SiteParameterType.Row_Inserting(Rs)
		If bInsertRow Then
			Try	
				SiteParameterType.Insert(Rs)
				AddRow = True
			Catch e As Exception
				If EW_DEBUG_ENABLED Then Throw
				Message = e.Message				
				AddRow = False
			End Try
		Else
			If SiteParameterType.CancelMessage <> "" Then
				Message = SiteParameterType.CancelMessage
				SiteParameterType.CancelMessage = ""
			Else
				Message = Language.Phrase("InsertCancelled")
			End If
			AddRow = False
		End If
		If AddRow Then
			LastInsertId = Conn.GetLastInsertId()
			SiteParameterType.SiteParameterTypeID.DbValue = LastInsertId
			Rs("SiteParameterTypeID") = SiteParameterType.SiteParameterTypeID.DbValue		

			' Row Inserted event
			SiteParameterType.Row_Inserted(Rs)
			WriteAuditTrailOnAdd(Rs)
		End If
	End Function

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "SiteParameterType"
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

	' Write Audit Trail (add page)
	Sub WriteAuditTrailOnAdd(ByRef RsSrc As OrderedDictionary)
		Dim table As String = "SiteParameterType"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsSrc("SiteParameterTypeID")

		' Write Audit Trail
		Dim filePfx As String = "log"
		Dim action As String = "A"
		Dim oldvalue As Object = ""
		Dim curDateTime As String, id As String, user As String, field As String
		Dim keyvalue As Object
		Dim dt As DateTime = Now()
		curDateTime = dt.ToString("yyyy/MM/dd HH:mm:ss")
		id = HttpContext.Current.Request.ServerVariables("SCRIPT_NAME")
    user = CurrentUserName()
		keyvalue = sKey

		' SiteParameterTypeNM Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "SiteParameterTypeNM", keyvalue, oldvalue, RsSrc("SiteParameterTypeNM"))

		' SiteParameterTypeDS Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "SiteParameterTypeDS", keyvalue, oldvalue, RsSrc("SiteParameterTypeDS"))

		' SiteParameterTypeOrder Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "SiteParameterTypeOrder", keyvalue, oldvalue, RsSrc("SiteParameterTypeOrder"))

		' SiteParameterTemplate Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "SiteParameterTemplate", keyvalue, oldvalue, "<MEMO>")
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
		SiteParameterType_add = New cSiteParameterType_add(Me)		
		SiteParameterType_add.Page_Init()		

		' Set buffer/cache option
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

		' Page main processing
		SiteParameterType_add.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If SiteParameterType_add IsNot Nothing Then SiteParameterType_add.Dispose()
	End Sub
End Class
