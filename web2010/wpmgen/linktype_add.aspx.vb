Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class linktype_add
	Inherits AspNetMaker8_wpmWebsite

	' Page object
	Public LinkType_add As cLinkType_add

	'
	' Page Class
	'
	Class cLinkType_add
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
				If LinkType.UseTokenInUrl Then Url = Url & "t=" & LinkType.TableVar & "&" ' Add page token
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
			If LinkType.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (LinkType.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (LinkType.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' ASP.NET page object
		Public ReadOnly Property AspNetPage() As linktype_add
			Get
				Return CType(m_ParentPage, linktype_add)
			End Get
		End Property

		' LinkType
		Public Property LinkType() As cLinkType
			Get				
				Return ParentPage.LinkType
			End Get
			Set(ByVal v As cLinkType)
				ParentPage.LinkType = v	
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
			m_PageObjName = "LinkType_add"
			m_PageObjTypeName = "cLinkType_add"			

			' Initialize language object
			Language = New cLanguage(Me)

			' Table Name
			m_TableName = "LinkType"

			' Initialize table object
			LinkType = New cLinkType(Me)

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
			LinkType.Dispose()
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
		If ew_Get("LinkTypeCD") <> "" Then
			LinkType.LinkTypeCD.QueryStringValue = ew_Get("LinkTypeCD")
		Else
			bCopy = False
		End If

		' Process form if post back
		If ObjForm.GetValue("a_add") <> "" Then
			LinkType.CurrentAction = ObjForm.GetValue("a_add") ' Get form action
			LoadFormValues() ' Load form values

			' Validate Form
			If Not ValidateForm() Then
				LinkType.CurrentAction = "I" ' Form error, reset action
				Message = ParentPage.gsFormError
			End If

		' Not post back
		Else
			If bCopy Then
				LinkType.CurrentAction = "C" ' Copy Record
			Else
				LinkType.CurrentAction = "I" ' Display Blank Record
				LoadDefaultValues() ' Load default values
			End If
		End If

		' Perform action based on action code
		Select Case LinkType.CurrentAction
			Case "I" ' Blank record, no action required
			Case "C" ' Copy an existing record
				If Not LoadRow() Then ' Load record based on key
					Message = Language.Phrase("NoRecord") ' No record found
					Page_Terminate("linktype_list.aspx") ' No matching record, return to list
				End If
			Case "A" ' Add new record
				LinkType.SendEmail = True ' Send email on add success
				If AddRow() Then ' Add successful
					Message = Language.Phrase("AddSuccess") ' Set up success message
					Dim sReturnUrl As String = LinkType.ReturnUrl
					Page_Terminate(sReturnUrl) ' Clean up and return
				Else
					RestoreFormValues() ' Add failed, restore form values
				End If
		End Select

		' Render row based on row type
		LinkType.RowType = EW_ROWTYPE_ADD ' Render add type

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
		LinkType.LinkTypeCD.FormValue = ObjForm.GetValue("x_LinkTypeCD")
		LinkType.LinkTypeCD.OldValue = ObjForm.GetValue("o_LinkTypeCD")
		LinkType.LinkTypeDesc.FormValue = ObjForm.GetValue("x_LinkTypeDesc")
		LinkType.LinkTypeDesc.OldValue = ObjForm.GetValue("o_LinkTypeDesc")
		LinkType.LinkTypeComment.FormValue = ObjForm.GetValue("x_LinkTypeComment")
		LinkType.LinkTypeComment.OldValue = ObjForm.GetValue("o_LinkTypeComment")
		LinkType.LinkTypeTarget.FormValue = ObjForm.GetValue("x_LinkTypeTarget")
		LinkType.LinkTypeTarget.OldValue = ObjForm.GetValue("o_LinkTypeTarget")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		LinkType.LinkTypeCD.CurrentValue = LinkType.LinkTypeCD.FormValue
		LinkType.LinkTypeDesc.CurrentValue = LinkType.LinkTypeDesc.FormValue
		LinkType.LinkTypeComment.CurrentValue = LinkType.LinkTypeComment.FormValue
		LinkType.LinkTypeTarget.CurrentValue = LinkType.LinkTypeTarget.FormValue
	End Sub

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = LinkType.KeyFilter

		' Row Selecting event
		LinkType.Row_Selecting(sFilter)

		' Load SQL based on filter
		LinkType.CurrentFilter = sFilter
		Dim sSql As String = LinkType.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then DebugMsg = sSql
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				LinkType.Row_Selected(RsRow)
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
		LinkType.LinkTypeCD.DbValue = RsRow("LinkTypeCD")
		LinkType.LinkTypeDesc.DbValue = RsRow("LinkTypeDesc")
		LinkType.LinkTypeComment.DbValue = RsRow("LinkTypeComment")
		LinkType.LinkTypeTarget.DbValue = RsRow("LinkTypeTarget")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Initialize urls
		' Row Rendering event

		LinkType.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' LinkTypeCD

		LinkType.LinkTypeCD.CellCssStyle = ""
		LinkType.LinkTypeCD.CellCssClass = ""
		LinkType.LinkTypeCD.CellAttrs.Clear(): LinkType.LinkTypeCD.ViewAttrs.Clear(): LinkType.LinkTypeCD.EditAttrs.Clear()

		' LinkTypeDesc
		LinkType.LinkTypeDesc.CellCssStyle = ""
		LinkType.LinkTypeDesc.CellCssClass = ""
		LinkType.LinkTypeDesc.CellAttrs.Clear(): LinkType.LinkTypeDesc.ViewAttrs.Clear(): LinkType.LinkTypeDesc.EditAttrs.Clear()

		' LinkTypeComment
		LinkType.LinkTypeComment.CellCssStyle = ""
		LinkType.LinkTypeComment.CellCssClass = ""
		LinkType.LinkTypeComment.CellAttrs.Clear(): LinkType.LinkTypeComment.ViewAttrs.Clear(): LinkType.LinkTypeComment.EditAttrs.Clear()

		' LinkTypeTarget
		LinkType.LinkTypeTarget.CellCssStyle = ""
		LinkType.LinkTypeTarget.CellCssClass = ""
		LinkType.LinkTypeTarget.CellAttrs.Clear(): LinkType.LinkTypeTarget.ViewAttrs.Clear(): LinkType.LinkTypeTarget.EditAttrs.Clear()

		'
		'  View  Row
		'

		If LinkType.RowType = EW_ROWTYPE_VIEW Then ' View row

			' LinkTypeCD
			LinkType.LinkTypeCD.ViewValue = LinkType.LinkTypeCD.CurrentValue
			LinkType.LinkTypeCD.CssStyle = ""
			LinkType.LinkTypeCD.CssClass = ""
			LinkType.LinkTypeCD.ViewCustomAttributes = ""

			' LinkTypeDesc
			LinkType.LinkTypeDesc.ViewValue = LinkType.LinkTypeDesc.CurrentValue
			LinkType.LinkTypeDesc.CssStyle = ""
			LinkType.LinkTypeDesc.CssClass = ""
			LinkType.LinkTypeDesc.ViewCustomAttributes = ""

			' LinkTypeComment
			LinkType.LinkTypeComment.ViewValue = LinkType.LinkTypeComment.CurrentValue
			LinkType.LinkTypeComment.CssStyle = ""
			LinkType.LinkTypeComment.CssClass = ""
			LinkType.LinkTypeComment.ViewCustomAttributes = ""

			' LinkTypeTarget
			LinkType.LinkTypeTarget.ViewValue = LinkType.LinkTypeTarget.CurrentValue
			LinkType.LinkTypeTarget.CssStyle = ""
			LinkType.LinkTypeTarget.CssClass = ""
			LinkType.LinkTypeTarget.ViewCustomAttributes = ""

			' View refer script
			' LinkTypeCD

			LinkType.LinkTypeCD.HrefValue = ""
			LinkType.LinkTypeCD.TooltipValue = ""

			' LinkTypeDesc
			LinkType.LinkTypeDesc.HrefValue = ""
			LinkType.LinkTypeDesc.TooltipValue = ""

			' LinkTypeComment
			LinkType.LinkTypeComment.HrefValue = ""
			LinkType.LinkTypeComment.TooltipValue = ""

			' LinkTypeTarget
			LinkType.LinkTypeTarget.HrefValue = ""
			LinkType.LinkTypeTarget.TooltipValue = ""

		'
		'  Add Row
		'

		ElseIf LinkType.RowType = EW_ROWTYPE_ADD Then ' Add row

			' LinkTypeCD
			LinkType.LinkTypeCD.EditCustomAttributes = ""
			LinkType.LinkTypeCD.EditValue = ew_HtmlEncode(LinkType.LinkTypeCD.CurrentValue)

			' LinkTypeDesc
			LinkType.LinkTypeDesc.EditCustomAttributes = ""
			LinkType.LinkTypeDesc.EditValue = ew_HtmlEncode(LinkType.LinkTypeDesc.CurrentValue)

			' LinkTypeComment
			LinkType.LinkTypeComment.EditCustomAttributes = ""
			LinkType.LinkTypeComment.EditValue = ew_HtmlEncode(LinkType.LinkTypeComment.CurrentValue)

			' LinkTypeTarget
			LinkType.LinkTypeTarget.EditCustomAttributes = ""
			LinkType.LinkTypeTarget.EditValue = ew_HtmlEncode(LinkType.LinkTypeTarget.CurrentValue)
		End If

		' Row Rendered event
		If LinkType.RowType <> EW_ROWTYPE_AGGREGATEINIT Then
			LinkType.Row_Rendered()
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

		' Check if key value entered
		If ew_Empty(LinkType.LinkTypeCD.CurrentValue) And ew_Empty(LinkType.LinkTypeCD.SessionValue) Then
			Message = Language.Phrase("InvalidKeyValue")
			Return False
		End If

		' Check for duplicate key
		Dim bCheckKey As Boolean = True
		sFilter = LinkType.KeyFilter
		If bCheckKey Then
			RsChk = LinkType.LoadRs(sFilter)
			If RsChk IsNot Nothing Then
				Dim sKeyErrMsg As String = Language.Phrase("DupKey").Replace("%f", sFilter)
				Message = sKeyErrMsg
				RsChk.Close()
				RsChk.Dispose()
				Return False
			End If
		End If
		Try

			' LinkTypeCD
			LinkType.LinkTypeCD.SetDbValue(Rs, LinkType.LinkTypeCD.CurrentValue, System.DBNull.Value, False)

			' LinkTypeDesc
			LinkType.LinkTypeDesc.SetDbValue(Rs, LinkType.LinkTypeDesc.CurrentValue, System.DBNull.Value, False)

			' LinkTypeComment
			LinkType.LinkTypeComment.SetDbValue(Rs, LinkType.LinkTypeComment.CurrentValue, System.DBNull.Value, False)

			' LinkTypeTarget
			LinkType.LinkTypeTarget.SetDbValue(Rs, LinkType.LinkTypeTarget.CurrentValue, System.DBNull.Value, False)
		Catch e As Exception
			If EW_DEBUG_ENABLED Then Throw
			Message = e.Message
			Return False
		End Try

		' Row Inserting event
		bInsertRow = LinkType.Row_Inserting(Rs)
		If bInsertRow Then
			Try	
				LinkType.Insert(Rs)
				AddRow = True
			Catch e As Exception
				If EW_DEBUG_ENABLED Then Throw
				Message = e.Message				
				AddRow = False
			End Try
		Else
			If LinkType.CancelMessage <> "" Then
				Message = LinkType.CancelMessage
				LinkType.CancelMessage = ""
			Else
				Message = Language.Phrase("InsertCancelled")
			End If
			AddRow = False
		End If
		If AddRow Then
			LastInsertId = Conn.GetLastInsertId()

			' Row Inserted event
			LinkType.Row_Inserted(Rs)
			WriteAuditTrailOnAdd(Rs)
		End If
	End Function

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "LinkType"
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
		Dim table As String = "LinkType"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsSrc("LinkTypeCD")

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

		' LinkTypeCD Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "LinkTypeCD", keyvalue, oldvalue, RsSrc("LinkTypeCD"))

		' LinkTypeDesc Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "LinkTypeDesc", keyvalue, oldvalue, RsSrc("LinkTypeDesc"))

		' LinkTypeComment Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "LinkTypeComment", keyvalue, oldvalue, "<MEMO>")

		' LinkTypeTarget Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "LinkTypeTarget", keyvalue, oldvalue, RsSrc("LinkTypeTarget"))
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
		LinkType_add = New cLinkType_add(Me)		
		LinkType_add.Page_Init()		

		' Set buffer/cache option
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

		' Page main processing
		LinkType_add.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If LinkType_add IsNot Nothing Then LinkType_add.Dispose()
	End Sub
End Class
