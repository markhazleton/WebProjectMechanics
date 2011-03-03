Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class sitecategorytype_add
	Inherits AspNetMaker8_wpmWebsite

	' Page object
	Public SiteCategoryType_add As cSiteCategoryType_add

	'
	' Page Class
	'
	Class cSiteCategoryType_add
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
		Public ReadOnly Property AspNetPage() As sitecategorytype_add
			Get
				Return CType(m_ParentPage, sitecategorytype_add)
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
			m_PageID = "add"
			m_PageObjName = "SiteCategoryType_add"
			m_PageObjTypeName = "cSiteCategoryType_add"			

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

	Public x_ewPriv As Integer

	'
	' Page main processing
	'
	Sub Page_Main()

		' Load key values from QueryString
		Dim bCopy As Boolean = True
		If ew_Get("SiteCategoryTypeID") <> "" Then
			SiteCategoryType.SiteCategoryTypeID.QueryStringValue = ew_Get("SiteCategoryTypeID")
		Else
			bCopy = False
		End If

		' Process form if post back
		If ObjForm.GetValue("a_add") <> "" Then
			SiteCategoryType.CurrentAction = ObjForm.GetValue("a_add") ' Get form action
			LoadFormValues() ' Load form values

			' Validate Form
			If Not ValidateForm() Then
				SiteCategoryType.CurrentAction = "I" ' Form error, reset action
				Message = ParentPage.gsFormError
			End If

		' Not post back
		Else
			If bCopy Then
				SiteCategoryType.CurrentAction = "C" ' Copy Record
			Else
				SiteCategoryType.CurrentAction = "I" ' Display Blank Record
				LoadDefaultValues() ' Load default values
			End If
		End If

		' Perform action based on action code
		Select Case SiteCategoryType.CurrentAction
			Case "I" ' Blank record, no action required
			Case "C" ' Copy an existing record
				If Not LoadRow() Then ' Load record based on key
					Message = Language.Phrase("NoRecord") ' No record found
					Page_Terminate("sitecategorytype_list.aspx") ' No matching record, return to list
				End If
			Case "A" ' Add new record
				SiteCategoryType.SendEmail = True ' Send email on add success
				If AddRow() Then ' Add successful
					Message = Language.Phrase("AddSuccess") ' Set up success message
					Dim sReturnUrl As String = SiteCategoryType.ReturnUrl
					Page_Terminate(sReturnUrl) ' Clean up and return
				Else
					RestoreFormValues() ' Add failed, restore form values
				End If
		End Select

		' Render row based on row type
		SiteCategoryType.RowType = EW_ROWTYPE_ADD ' Render add type

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
		SiteCategoryType.SiteCategoryTypeID.FormValue = ObjForm.GetValue("x_SiteCategoryTypeID")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		SiteCategoryType.SiteCategoryTypeNM.CurrentValue = SiteCategoryType.SiteCategoryTypeNM.FormValue
		SiteCategoryType.SiteCategoryTypeDS.CurrentValue = SiteCategoryType.SiteCategoryTypeDS.FormValue
		SiteCategoryType.SiteCategoryComment.CurrentValue = SiteCategoryType.SiteCategoryComment.FormValue
		SiteCategoryType.SiteCategoryFileName.CurrentValue = SiteCategoryType.SiteCategoryFileName.FormValue
		SiteCategoryType.SiteCategoryTransferURL.CurrentValue = SiteCategoryType.SiteCategoryTransferURL.FormValue
		SiteCategoryType.DefaultSiteCategoryID.CurrentValue = SiteCategoryType.DefaultSiteCategoryID.FormValue
		SiteCategoryType.SiteCategoryTypeID.CurrentValue = SiteCategoryType.SiteCategoryTypeID.FormValue
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
		'  Add Row
		'

		ElseIf SiteCategoryType.RowType = EW_ROWTYPE_ADD Then ' Add row

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
			Return False
		End Try

		' Row Inserting event
		bInsertRow = SiteCategoryType.Row_Inserting(Rs)
		If bInsertRow Then
			Try	
				SiteCategoryType.Insert(Rs)
				AddRow = True
			Catch e As Exception
				If EW_DEBUG_ENABLED Then Throw
				Message = e.Message				
				AddRow = False
			End Try
		Else
			If SiteCategoryType.CancelMessage <> "" Then
				Message = SiteCategoryType.CancelMessage
				SiteCategoryType.CancelMessage = ""
			Else
				Message = Language.Phrase("InsertCancelled")
			End If
			AddRow = False
		End If
		If AddRow Then
			LastInsertId = Conn.GetLastInsertId()
			SiteCategoryType.SiteCategoryTypeID.DbValue = LastInsertId
			Rs("SiteCategoryTypeID") = SiteCategoryType.SiteCategoryTypeID.DbValue		

			' Row Inserted event
			SiteCategoryType.Row_Inserted(Rs)
			WriteAuditTrailOnAdd(Rs)
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

	' Write Audit Trail (add page)
	Sub WriteAuditTrailOnAdd(ByRef RsSrc As OrderedDictionary)
		Dim table As String = "SiteCategoryType"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsSrc("SiteCategoryTypeID")

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

		' SiteCategoryTypeNM Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "SiteCategoryTypeNM", keyvalue, oldvalue, RsSrc("SiteCategoryTypeNM"))

		' SiteCategoryTypeDS Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "SiteCategoryTypeDS", keyvalue, oldvalue, RsSrc("SiteCategoryTypeDS"))

		' SiteCategoryComment Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "SiteCategoryComment", keyvalue, oldvalue, RsSrc("SiteCategoryComment"))

		' SiteCategoryFileName Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "SiteCategoryFileName", keyvalue, oldvalue, RsSrc("SiteCategoryFileName"))

		' SiteCategoryTransferURL Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "SiteCategoryTransferURL", keyvalue, oldvalue, RsSrc("SiteCategoryTransferURL"))

		' DefaultSiteCategoryID Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "DefaultSiteCategoryID", keyvalue, oldvalue, RsSrc("DefaultSiteCategoryID"))
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
		SiteCategoryType_add = New cSiteCategoryType_add(Me)		
		SiteCategoryType_add.Page_Init()		

		' Set buffer/cache option
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

		' Page main processing
		SiteCategoryType_add.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If SiteCategoryType_add IsNot Nothing Then SiteCategoryType_add.Dispose()
	End Sub
End Class
