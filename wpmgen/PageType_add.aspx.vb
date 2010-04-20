Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class PageType_add
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public PageType_add As cPageType_add

	'
	' Page Class
	'
	Class cPageType_add
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
				If PageType.UseTokenInUrl Then Url = Url & "t=" & PageType.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If PageType.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (PageType.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (PageType.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' PageType
		Public Property PageType() As cPageType
			Get				
				Return ParentPage.PageType
			End Get
			Set(ByVal v As cPageType)
				ParentPage.PageType = v	
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
			m_PageObjName = "PageType_add"
			m_PageObjTypeName = "cPageType_add"

			' Table Name
			m_TableName = "PageType"

			' Initialize table object
			PageType = New cPageType(Me)

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
			PageType.Dispose()

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
		If ew_Get("PageTypeID") <> "" Then
			PageType.PageTypeID.QueryStringValue = ew_Get("PageTypeID")
		Else
			bCopy = False
		End If

		' Create form object
		ObjForm = New cFormObj

		' Process form if post back
		If ObjForm.GetValue("a_add") <> "" Then
			PageType.CurrentAction = ObjForm.GetValue("a_add") ' Get form action
			LoadFormValues() ' Load form values

			' Validate Form
			If Not ValidateForm() Then
				PageType.CurrentAction = "I" ' Form error, reset action
				Message = ParentPage.gsFormError
			End If

		' Not post back
		Else
			If bCopy Then
				PageType.CurrentAction = "C" ' Copy Record
			Else
				PageType.CurrentAction = "I" ' Display Blank Record
				LoadDefaultValues() ' Load default values
			End If
		End If

		' Perform action based on action code
		Select Case PageType.CurrentAction
			Case "I" ' Blank record, no action required
			Case "C" ' Copy an existing record
				If Not LoadRow() Then ' Load record based on key
					Message = "No records found" ' No record found
					Page_Terminate("PageType_list.aspx") ' No matching record, return to list
				End If
			Case "A" ' Add new record
				PageType.SendEmail = True ' Send email on add success
				If AddRow() Then ' Add successful
					Message = "Add succeeded" ' Set up success message
					Dim sReturnUrl As String = PageType.ReturnUrl
					If ew_GetPageName(sReturnUrl) = "PageType_view.aspx" Then sReturnUrl = PageType.ViewUrl ' View paging, return to view page with keyurl directly
					Page_Terminate(sReturnUrl) ' Clean up and return
				Else
					RestoreFormValues() ' Add failed, restore form values
				End If
		End Select

		' Render row based on row type
		PageType.RowType = EW_ROWTYPE_ADD ' Render add type

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
		PageType.PageTypeCD.FormValue = ObjForm.GetValue("x_PageTypeCD")
		PageType.PageTypeCD.OldValue = ObjForm.GetValue("o_PageTypeCD")
		PageType.PageTypeDesc.FormValue = ObjForm.GetValue("x_PageTypeDesc")
		PageType.PageTypeDesc.OldValue = ObjForm.GetValue("o_PageTypeDesc")
		PageType.PageFileName.FormValue = ObjForm.GetValue("x_PageFileName")
		PageType.PageFileName.OldValue = ObjForm.GetValue("o_PageFileName")
		PageType.PageTypeID.FormValue = ObjForm.GetValue("x_PageTypeID")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		PageType.PageTypeCD.CurrentValue = PageType.PageTypeCD.FormValue
		PageType.PageTypeDesc.CurrentValue = PageType.PageTypeDesc.FormValue
		PageType.PageFileName.CurrentValue = PageType.PageFileName.FormValue
		PageType.PageTypeID.CurrentValue = PageType.PageTypeID.FormValue
	End Sub

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = PageType.KeyFilter

		' Row Selecting event
		PageType.Row_Selecting(sFilter)

		' Load SQL based on filter
		PageType.CurrentFilter = sFilter
		Dim sSql As String = PageType.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				PageType.Row_Selected(RsRow)
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
		PageType.PageTypeID.DbValue = RsRow("PageTypeID")
		PageType.PageTypeCD.DbValue = RsRow("PageTypeCD")
		PageType.PageTypeDesc.DbValue = RsRow("PageTypeDesc")
		PageType.PageFileName.DbValue = RsRow("PageFileName")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		PageType.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' PageTypeCD

		PageType.PageTypeCD.CellCssStyle = ""
		PageType.PageTypeCD.CellCssClass = ""

		' PageTypeDesc
		PageType.PageTypeDesc.CellCssStyle = ""
		PageType.PageTypeDesc.CellCssClass = ""

		' PageFileName
		PageType.PageFileName.CellCssStyle = ""
		PageType.PageFileName.CellCssClass = ""

		'
		'  View  Row
		'

		If PageType.RowType = EW_ROWTYPE_VIEW Then ' View row

			' PageTypeID
			PageType.PageTypeID.ViewValue = PageType.PageTypeID.CurrentValue
			PageType.PageTypeID.CssStyle = ""
			PageType.PageTypeID.CssClass = ""
			PageType.PageTypeID.ViewCustomAttributes = ""

			' PageTypeCD
			PageType.PageTypeCD.ViewValue = PageType.PageTypeCD.CurrentValue
			PageType.PageTypeCD.CssStyle = ""
			PageType.PageTypeCD.CssClass = ""
			PageType.PageTypeCD.ViewCustomAttributes = ""

			' PageTypeDesc
			PageType.PageTypeDesc.ViewValue = PageType.PageTypeDesc.CurrentValue
			PageType.PageTypeDesc.CssStyle = ""
			PageType.PageTypeDesc.CssClass = ""
			PageType.PageTypeDesc.ViewCustomAttributes = ""

			' PageFileName
			PageType.PageFileName.ViewValue = PageType.PageFileName.CurrentValue
			PageType.PageFileName.CssStyle = ""
			PageType.PageFileName.CssClass = ""
			PageType.PageFileName.ViewCustomAttributes = ""

			' View refer script
			' PageTypeCD

			PageType.PageTypeCD.HrefValue = ""

			' PageTypeDesc
			PageType.PageTypeDesc.HrefValue = ""

			' PageFileName
			PageType.PageFileName.HrefValue = ""

		'
		'  Add Row
		'

		ElseIf PageType.RowType = EW_ROWTYPE_ADD Then ' Add row

			' PageTypeCD
			PageType.PageTypeCD.EditCustomAttributes = ""
			PageType.PageTypeCD.EditValue = ew_HtmlEncode(PageType.PageTypeCD.CurrentValue)

			' PageTypeDesc
			PageType.PageTypeDesc.EditCustomAttributes = ""
			PageType.PageTypeDesc.EditValue = ew_HtmlEncode(PageType.PageTypeDesc.CurrentValue)

			' PageFileName
			PageType.PageFileName.EditCustomAttributes = ""
			PageType.PageFileName.EditValue = ew_HtmlEncode(PageType.PageFileName.CurrentValue)
		End If

		' Row Rendered event
		PageType.Row_Rendered()
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

		' PageTypeCD
		PageType.PageTypeCD.SetDbValue(PageType.PageTypeCD.CurrentValue, System.DBNull.Value)
		Rs("PageTypeCD") = PageType.PageTypeCD.DbValue

		' PageTypeDesc
		PageType.PageTypeDesc.SetDbValue(PageType.PageTypeDesc.CurrentValue, System.DBNull.Value)
		Rs("PageTypeDesc") = PageType.PageTypeDesc.DbValue

		' PageFileName
		PageType.PageFileName.SetDbValue(PageType.PageFileName.CurrentValue, System.DBNull.Value)
		Rs("PageFileName") = PageType.PageFileName.DbValue

		' Row Inserting event
		bInsertRow = PageType.Row_Inserting(Rs)
		If bInsertRow Then
			Try	
				PageType.Insert(Rs)
				AddRow = True
			Catch e As Exception
				If EW_DEBUG_ENABLED Then Throw
				Message = e.Message				
				AddRow = False
			End Try
		Else
			If PageType.CancelMessage <> "" Then
				Message = PageType.CancelMessage
				PageType.CancelMessage = ""
			Else
				Message = "Insert cancelled"
			End If
			AddRow = False
		End If
		If AddRow Then
			LastInsertId = Conn.GetLastInsertId()
			PageType.PageTypeID.DbValue = LastInsertId
			Rs("PageTypeID") = PageType.PageTypeID.DbValue		

			' Row Inserted event
			PageType.Row_Inserted(Rs)
			WriteAuditTrailOnAdd(Rs)
		End If
	End Function

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "PageType"
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
		Dim table As String = "PageType"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsSrc("PageTypeID")

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

		' PageTypeCD Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "PageTypeCD", keyvalue, oldvalue, RsSrc("PageTypeCD"))

		' PageTypeDesc Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "PageTypeDesc", keyvalue, oldvalue, RsSrc("PageTypeDesc"))

		' PageFileName Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "PageFileName", keyvalue, oldvalue, RsSrc("PageFileName"))
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
		PageType_add = New cPageType_add(Me)		
		PageType_add.Page_Init()

		' Page main processing
		PageType_add.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If PageType_add IsNot Nothing Then PageType_add.Dispose()
	End Sub
End Class
