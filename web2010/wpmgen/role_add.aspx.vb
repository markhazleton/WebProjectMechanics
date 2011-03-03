Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class role_add
	Inherits AspNetMaker8_wpmWebsite

	' Page object
	Public role_add As crole_add

	'
	' Page Class
	'
	Class crole_add
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
				If role.UseTokenInUrl Then Url = Url & "t=" & role.TableVar & "&" ' Add page token
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
			If role.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (role.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (role.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' ASP.NET page object
		Public ReadOnly Property AspNetPage() As role_add
			Get
				Return CType(m_ParentPage, role_add)
			End Get
		End Property

		' role
		Public Property role() As crole
			Get				
				Return ParentPage.role
			End Get
			Set(ByVal v As crole)
				ParentPage.role = v	
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
			m_PageObjName = "role_add"
			m_PageObjTypeName = "crole_add"			

			' Initialize language object
			Language = New cLanguage(Me)

			' Table Name
			m_TableName = "role"

			' Initialize table object
			role = New crole(Me)

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
			role.Dispose()
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
		If ew_Get("RoleID") <> "" Then
			role.RoleID.QueryStringValue = ew_Get("RoleID")
		Else
			bCopy = False
		End If

		' Process form if post back
		If ObjForm.GetValue("a_add") <> "" Then
			role.CurrentAction = ObjForm.GetValue("a_add") ' Get form action
			LoadFormValues() ' Load form values

			' Validate Form
			If Not ValidateForm() Then
				role.CurrentAction = "I" ' Form error, reset action
				Message = ParentPage.gsFormError
			End If

		' Not post back
		Else
			If bCopy Then
				role.CurrentAction = "C" ' Copy Record
			Else
				role.CurrentAction = "I" ' Display Blank Record
				LoadDefaultValues() ' Load default values
			End If
		End If

		' Perform action based on action code
		Select Case role.CurrentAction
			Case "I" ' Blank record, no action required
			Case "C" ' Copy an existing record
				If Not LoadRow() Then ' Load record based on key
					Message = Language.Phrase("NoRecord") ' No record found
					Page_Terminate("role_list.aspx") ' No matching record, return to list
				End If
			Case "A" ' Add new record
				role.SendEmail = True ' Send email on add success
				If AddRow() Then ' Add successful
					Message = Language.Phrase("AddSuccess") ' Set up success message
					Dim sReturnUrl As String = role.ReturnUrl
					Page_Terminate(sReturnUrl) ' Clean up and return
				Else
					RestoreFormValues() ' Add failed, restore form values
				End If
		End Select

		' Render row based on row type
		role.RowType = EW_ROWTYPE_ADD ' Render add type

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
		role.RoleName.FormValue = ObjForm.GetValue("x_RoleName")
		role.RoleName.OldValue = ObjForm.GetValue("o_RoleName")
		role.RoleTitle.FormValue = ObjForm.GetValue("x_RoleTitle")
		role.RoleTitle.OldValue = ObjForm.GetValue("o_RoleTitle")
		role.RoleComment.FormValue = ObjForm.GetValue("x_RoleComment")
		role.RoleComment.OldValue = ObjForm.GetValue("o_RoleComment")
		role.FilterMenu.FormValue = ObjForm.GetValue("x_FilterMenu")
		role.FilterMenu.OldValue = ObjForm.GetValue("o_FilterMenu")
		role.RoleID.FormValue = ObjForm.GetValue("x_RoleID")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		role.RoleName.CurrentValue = role.RoleName.FormValue
		role.RoleTitle.CurrentValue = role.RoleTitle.FormValue
		role.RoleComment.CurrentValue = role.RoleComment.FormValue
		role.FilterMenu.CurrentValue = role.FilterMenu.FormValue
		role.RoleID.CurrentValue = role.RoleID.FormValue
	End Sub

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = role.KeyFilter

		' Row Selecting event
		role.Row_Selecting(sFilter)

		' Load SQL based on filter
		role.CurrentFilter = sFilter
		Dim sSql As String = role.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then DebugMsg = sSql
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				role.Row_Selected(RsRow)
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
		role.RoleID.DbValue = RsRow("RoleID")
		role.RoleName.DbValue = RsRow("RoleName")
		role.RoleTitle.DbValue = RsRow("RoleTitle")
		role.RoleComment.DbValue = RsRow("RoleComment")
		role.FilterMenu.DbValue = IIf(ew_ConvertToBool(RsRow("FilterMenu")), "1", "0")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Initialize urls
		' Row Rendering event

		role.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' RoleName

		role.RoleName.CellCssStyle = ""
		role.RoleName.CellCssClass = ""
		role.RoleName.CellAttrs.Clear(): role.RoleName.ViewAttrs.Clear(): role.RoleName.EditAttrs.Clear()

		' RoleTitle
		role.RoleTitle.CellCssStyle = ""
		role.RoleTitle.CellCssClass = ""
		role.RoleTitle.CellAttrs.Clear(): role.RoleTitle.ViewAttrs.Clear(): role.RoleTitle.EditAttrs.Clear()

		' RoleComment
		role.RoleComment.CellCssStyle = ""
		role.RoleComment.CellCssClass = ""
		role.RoleComment.CellAttrs.Clear(): role.RoleComment.ViewAttrs.Clear(): role.RoleComment.EditAttrs.Clear()

		' FilterMenu
		role.FilterMenu.CellCssStyle = ""
		role.FilterMenu.CellCssClass = ""
		role.FilterMenu.CellAttrs.Clear(): role.FilterMenu.ViewAttrs.Clear(): role.FilterMenu.EditAttrs.Clear()

		'
		'  View  Row
		'

		If role.RowType = EW_ROWTYPE_VIEW Then ' View row

			' RoleID
			role.RoleID.ViewValue = role.RoleID.CurrentValue
			role.RoleID.CssStyle = ""
			role.RoleID.CssClass = ""
			role.RoleID.ViewCustomAttributes = ""

			' RoleName
			role.RoleName.ViewValue = role.RoleName.CurrentValue
			role.RoleName.CssStyle = ""
			role.RoleName.CssClass = ""
			role.RoleName.ViewCustomAttributes = ""

			' RoleTitle
			role.RoleTitle.ViewValue = role.RoleTitle.CurrentValue
			role.RoleTitle.CssStyle = ""
			role.RoleTitle.CssClass = ""
			role.RoleTitle.ViewCustomAttributes = ""

			' RoleComment
			role.RoleComment.ViewValue = role.RoleComment.CurrentValue
			role.RoleComment.CssStyle = ""
			role.RoleComment.CssClass = ""
			role.RoleComment.ViewCustomAttributes = ""

			' FilterMenu
			If Convert.ToString(role.FilterMenu.CurrentValue) = "1" Then
				role.FilterMenu.ViewValue = "Yes"
			Else
				role.FilterMenu.ViewValue = "No"
			End If
			role.FilterMenu.CssStyle = ""
			role.FilterMenu.CssClass = ""
			role.FilterMenu.ViewCustomAttributes = ""

			' View refer script
			' RoleName

			role.RoleName.HrefValue = ""
			role.RoleName.TooltipValue = ""

			' RoleTitle
			role.RoleTitle.HrefValue = ""
			role.RoleTitle.TooltipValue = ""

			' RoleComment
			role.RoleComment.HrefValue = ""
			role.RoleComment.TooltipValue = ""

			' FilterMenu
			role.FilterMenu.HrefValue = ""
			role.FilterMenu.TooltipValue = ""

		'
		'  Add Row
		'

		ElseIf role.RowType = EW_ROWTYPE_ADD Then ' Add row

			' RoleName
			role.RoleName.EditCustomAttributes = ""
			role.RoleName.EditValue = ew_HtmlEncode(role.RoleName.CurrentValue)

			' RoleTitle
			role.RoleTitle.EditCustomAttributes = ""
			role.RoleTitle.EditValue = ew_HtmlEncode(role.RoleTitle.CurrentValue)

			' RoleComment
			role.RoleComment.EditCustomAttributes = ""
			role.RoleComment.EditValue = ew_HtmlEncode(role.RoleComment.CurrentValue)

			' FilterMenu
			role.FilterMenu.EditCustomAttributes = ""
		End If

		' Row Rendered event
		If role.RowType <> EW_ROWTYPE_AGGREGATEINIT Then
			role.Row_Rendered()
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
		Try

			' RoleName
			role.RoleName.SetDbValue(Rs, role.RoleName.CurrentValue, System.DBNull.Value, False)

			' RoleTitle
			role.RoleTitle.SetDbValue(Rs, role.RoleTitle.CurrentValue, System.DBNull.Value, False)

			' RoleComment
			role.RoleComment.SetDbValue(Rs, role.RoleComment.CurrentValue, System.DBNull.Value, False)

			' FilterMenu
			role.FilterMenu.SetDbValue(Rs, (role.FilterMenu.CurrentValue <> "" AndAlso Not IsDBNull(role.FilterMenu.CurrentValue)), System.DBNull.Value, False)
		Catch e As Exception
			If EW_DEBUG_ENABLED Then Throw
			Message = e.Message
			Return False
		End Try

		' Row Inserting event
		bInsertRow = role.Row_Inserting(Rs)
		If bInsertRow Then
			Try	
				role.Insert(Rs)
				AddRow = True
			Catch e As Exception
				If EW_DEBUG_ENABLED Then Throw
				Message = e.Message				
				AddRow = False
			End Try
		Else
			If role.CancelMessage <> "" Then
				Message = role.CancelMessage
				role.CancelMessage = ""
			Else
				Message = Language.Phrase("InsertCancelled")
			End If
			AddRow = False
		End If
		If AddRow Then
			LastInsertId = Conn.GetLastInsertId()
			role.RoleID.DbValue = LastInsertId
			Rs("RoleID") = role.RoleID.DbValue		

			' Row Inserted event
			role.Row_Inserted(Rs)
			WriteAuditTrailOnAdd(Rs)
		End If
	End Function

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "role"
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
		Dim table As String = "role"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsSrc("RoleID")

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

		' RoleName Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "RoleName", keyvalue, oldvalue, RsSrc("RoleName"))

		' RoleTitle Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "RoleTitle", keyvalue, oldvalue, RsSrc("RoleTitle"))

		' RoleComment Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "RoleComment", keyvalue, oldvalue, RsSrc("RoleComment"))

		' FilterMenu Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "FilterMenu", keyvalue, oldvalue, RsSrc("FilterMenu"))
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
		role_add = New crole_add(Me)		
		role_add.Page_Init()		

		' Set buffer/cache option
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

		' Page main processing
		role_add.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If role_add IsNot Nothing Then role_add.Dispose()
	End Sub
End Class