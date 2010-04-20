Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class PageRole_add
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public PageRole_add As cPageRole_add

	'
	' Page Class
	'
	Class cPageRole_add
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
				If PageRole.UseTokenInUrl Then Url = Url & "t=" & PageRole.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

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
		Public Sub New(ByRef APage As AspNetMaker7_WPMGen)				
			m_ParentPage = APage
			m_Page = Me	
			m_PageID = "add"
			m_PageObjName = "PageRole_add"
			m_PageObjTypeName = "cPageRole_add"

			' Table Name
			m_TableName = "PageRole"

			' Initialize table object
			PageRole = New cPageRole(Me)

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
			PageRole.Dispose()

			' Go to URL if specified
			If url <> "" Then
				HttpContext.Current.Response.Clear()
				HttpContext.Current.Response.Redirect(url)
			End If
		End Sub

	Public x_ewPriv As Integer

	'
	' Page main processing
	'
	Sub Page_Main()

		' Load key values from QueryString
		Dim bCopy As Boolean = True
		If ew_Get("PageRoleID") <> "" Then
			PageRole.PageRoleID.QueryStringValue = ew_Get("PageRoleID")
		Else
			bCopy = False
		End If

		' Create form object
		ObjForm = New cFormObj

		' Process form if post back
		If ObjForm.GetValue("a_add") <> "" Then
			PageRole.CurrentAction = ObjForm.GetValue("a_add") ' Get form action
			LoadFormValues() ' Load form values

			' Validate Form
			If Not ValidateForm() Then
				PageRole.CurrentAction = "I" ' Form error, reset action
				Message = ParentPage.gsFormError
			End If

		' Not post back
		Else
			If bCopy Then
				PageRole.CurrentAction = "C" ' Copy Record
			Else
				PageRole.CurrentAction = "I" ' Display Blank Record
				LoadDefaultValues() ' Load default values
			End If
		End If

		' Perform action based on action code
		Select Case PageRole.CurrentAction
			Case "I" ' Blank record, no action required
			Case "C" ' Copy an existing record
				If Not LoadRow() Then ' Load record based on key
					Message = "No records found" ' No record found
					Page_Terminate("PageRole_list.aspx") ' No matching record, return to list
				End If
			Case "A" ' Add new record
				PageRole.SendEmail = True ' Send email on add success
				If AddRow() Then ' Add successful
					Message = "Add succeeded" ' Set up success message
					Dim sReturnUrl As String = PageRole.ReturnUrl
					If ew_GetPageName(sReturnUrl) = "PageRole_view.aspx" Then sReturnUrl = PageRole.ViewUrl ' View paging, return to view page with keyurl directly
					Page_Terminate(sReturnUrl) ' Clean up and return
				Else
					RestoreFormValues() ' Add failed, restore form values
				End If
		End Select

		' Render row based on row type
		PageRole.RowType = EW_ROWTYPE_ADD ' Render add type

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
		PageRole.RoleID.CurrentValue = 0
		PageRole.zPageID.CurrentValue = 0
		PageRole.CompanyID.CurrentValue = 0
	End Sub

	'
	' Load form values
	'
	Sub LoadFormValues()
		PageRole.RoleID.FormValue = ObjForm.GetValue("x_RoleID")
		PageRole.RoleID.OldValue = ObjForm.GetValue("o_RoleID")
		PageRole.zPageID.FormValue = ObjForm.GetValue("x_zPageID")
		PageRole.zPageID.OldValue = ObjForm.GetValue("o_zPageID")
		PageRole.CompanyID.FormValue = ObjForm.GetValue("x_CompanyID")
		PageRole.CompanyID.OldValue = ObjForm.GetValue("o_CompanyID")
		PageRole.PageRoleID.FormValue = ObjForm.GetValue("x_PageRoleID")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		PageRole.RoleID.CurrentValue = PageRole.RoleID.FormValue
		PageRole.zPageID.CurrentValue = PageRole.zPageID.FormValue
		PageRole.CompanyID.CurrentValue = PageRole.CompanyID.FormValue
		PageRole.PageRoleID.CurrentValue = PageRole.PageRoleID.FormValue
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
		If EW_DEBUG_ENABLED Then ew_Write(sSql)
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
		PageRole.PageRoleID.DbValue = RsRow("PageRoleID")
		PageRole.RoleID.DbValue = RsRow("RoleID")
		PageRole.zPageID.DbValue = RsRow("PageID")
		PageRole.CompanyID.DbValue = RsRow("CompanyID")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		PageRole.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' RoleID

		PageRole.RoleID.CellCssStyle = ""
		PageRole.RoleID.CellCssClass = ""

		' PageID
		PageRole.zPageID.CellCssStyle = ""
		PageRole.zPageID.CellCssClass = ""

		' CompanyID
		PageRole.CompanyID.CellCssStyle = ""
		PageRole.CompanyID.CellCssClass = ""

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
			' RoleID

			PageRole.RoleID.HrefValue = ""

			' PageID
			PageRole.zPageID.HrefValue = ""

			' CompanyID
			PageRole.CompanyID.HrefValue = ""

		'
		'  Add Row
		'

		ElseIf PageRole.RowType = EW_ROWTYPE_ADD Then ' Add row

			' RoleID
			PageRole.RoleID.EditCustomAttributes = ""
			PageRole.RoleID.EditValue = ew_HtmlEncode(PageRole.RoleID.CurrentValue)

			' PageID
			PageRole.zPageID.EditCustomAttributes = ""
			PageRole.zPageID.EditValue = ew_HtmlEncode(PageRole.zPageID.CurrentValue)

			' CompanyID
			PageRole.CompanyID.EditCustomAttributes = ""
			PageRole.CompanyID.EditValue = ew_HtmlEncode(PageRole.CompanyID.CurrentValue)
		End If

		' Row Rendered event
		PageRole.Row_Rendered()
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
			ParentPage.gsFormError = ParentPage.gsFormError & "Incorrect integer - Role ID"
		End If
		If Not ew_CheckInteger(PageRole.zPageID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Incorrect integer - Page ID"
		End If
		If Not ew_CheckInteger(PageRole.CompanyID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Incorrect integer - Company ID"
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

		' RoleID
		PageRole.RoleID.SetDbValue(PageRole.RoleID.CurrentValue, System.DBNull.Value)
		Rs("RoleID") = PageRole.RoleID.DbValue

		' PageID
		PageRole.zPageID.SetDbValue(PageRole.zPageID.CurrentValue, System.DBNull.Value)
		Rs("PageID") = PageRole.zPageID.DbValue

		' CompanyID
		PageRole.CompanyID.SetDbValue(PageRole.CompanyID.CurrentValue, System.DBNull.Value)
		Rs("CompanyID") = PageRole.CompanyID.DbValue

		' Row Inserting event
		bInsertRow = PageRole.Row_Inserting(Rs)
		If bInsertRow Then
			Try	
				PageRole.Insert(Rs)
				AddRow = True
			Catch e As Exception
				If EW_DEBUG_ENABLED Then Throw
				Message = e.Message				
				AddRow = False
			End Try
		Else
			If PageRole.CancelMessage <> "" Then
				Message = PageRole.CancelMessage
				PageRole.CancelMessage = ""
			Else
				Message = "Insert cancelled"
			End If
			AddRow = False
		End If
		If AddRow Then
			LastInsertId = Conn.GetLastInsertId()
			PageRole.PageRoleID.DbValue = LastInsertId
			Rs("PageRoleID") = PageRole.PageRoleID.DbValue		

			' Row Inserted event
			PageRole.Row_Inserted(Rs)
			WriteAuditTrailOnAdd(Rs)
		End If
	End Function

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "PageRole"
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

	' Write Audit Trail (add page)
	Sub WriteAuditTrailOnAdd(ByRef RsSrc As OrderedDictionary)
		Dim table As String = "PageRole"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsSrc("PageRoleID")

		' Write Audit Trail
		Dim filePfx As String = "log"
		Dim action As String = "A"
		Dim oldvalue As Object = ""
		Dim curDate As String, curTime As String, id As String, user As String, field As String
		Dim keyvalue As Object
		Dim dt As DateTime = Now()
		curDate = dt.ToString("yyyy/MM/dd")
		curTime = dt.ToString("HH:mm:ss")
		id = HttpContext.Current.Request.ServerVariables("SCRIPT_NAME")
    user = CurrentUserName()
		keyvalue = sKey

		' RoleID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "RoleID", keyvalue, oldvalue, RsSrc("RoleID"))

		' PageID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "PageID", keyvalue, oldvalue, RsSrc("PageID"))

		' CompanyID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "CompanyID", keyvalue, oldvalue, RsSrc("CompanyID"))
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
		PageRole_add = New cPageRole_add(Me)		
		PageRole_add.Page_Init()

		' Page main processing
		PageRole_add.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If PageRole_add IsNot Nothing Then PageRole_add.Dispose()
	End Sub
End Class
