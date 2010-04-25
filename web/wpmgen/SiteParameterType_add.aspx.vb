Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class SiteParameterType_add
	Inherits AspNetMaker7_WPMGen

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
		Public Sub New(ByRef APage As AspNetMaker7_WPMGen)				
			m_ParentPage = APage
			m_Page = Me	
			m_PageID = "add"
			m_PageObjName = "SiteParameterType_add"
			m_PageObjTypeName = "cSiteParameterType_add"

			' Table Name
			m_TableName = "SiteParameterType"

			' Initialize table object
			SiteParameterType = New cSiteParameterType(Me)

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
			SiteParameterType.Dispose()

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
		If ew_Get("SiteParameterTypeID") <> "" Then
			SiteParameterType.SiteParameterTypeID.QueryStringValue = ew_Get("SiteParameterTypeID")
		Else
			bCopy = False
		End If

		' Create form object
		ObjForm = New cFormObj

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
					Message = "No records found" ' No record found
					Page_Terminate("SiteParameterType_list.aspx") ' No matching record, return to list
				End If
			Case "A" ' Add new record
				SiteParameterType.SendEmail = True ' Send email on add success
				If AddRow() Then ' Add successful
					Message = "Add succeeded" ' Set up success message
					Dim sReturnUrl As String = SiteParameterType.ReturnUrl
					If ew_GetPageName(sReturnUrl) = "SiteParameterType_view.aspx" Then sReturnUrl = SiteParameterType.ViewUrl ' View paging, return to view page with keyurl directly
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
		If EW_DEBUG_ENABLED Then ew_Write(sSql)
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

		' Row Rendering event
		SiteParameterType.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' SiteParameterTypeNM

		SiteParameterType.SiteParameterTypeNM.CellCssStyle = ""
		SiteParameterType.SiteParameterTypeNM.CellCssClass = ""

		' SiteParameterTypeDS
		SiteParameterType.SiteParameterTypeDS.CellCssStyle = ""
		SiteParameterType.SiteParameterTypeDS.CellCssClass = ""

		' SiteParameterTypeOrder
		SiteParameterType.SiteParameterTypeOrder.CellCssStyle = ""
		SiteParameterType.SiteParameterTypeOrder.CellCssClass = ""

		' SiteParameterTemplate
		SiteParameterType.SiteParameterTemplate.CellCssStyle = ""
		SiteParameterType.SiteParameterTemplate.CellCssClass = ""

		'
		'  View  Row
		'

		If SiteParameterType.RowType = EW_ROWTYPE_VIEW Then ' View row

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

			' SiteParameterTypeDS
			SiteParameterType.SiteParameterTypeDS.HrefValue = ""

			' SiteParameterTypeOrder
			SiteParameterType.SiteParameterTypeOrder.HrefValue = ""

			' SiteParameterTemplate
			SiteParameterType.SiteParameterTemplate.HrefValue = ""

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
		SiteParameterType.Row_Rendered()
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
			ParentPage.gsFormError = ParentPage.gsFormError & "Incorrect integer - Parameter Order"
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

		' SiteParameterTypeNM
		SiteParameterType.SiteParameterTypeNM.SetDbValue(SiteParameterType.SiteParameterTypeNM.CurrentValue, System.DBNull.Value)
		Rs("SiteParameterTypeNM") = SiteParameterType.SiteParameterTypeNM.DbValue

		' SiteParameterTypeDS
		SiteParameterType.SiteParameterTypeDS.SetDbValue(SiteParameterType.SiteParameterTypeDS.CurrentValue, System.DBNull.Value)
		Rs("SiteParameterTypeDS") = SiteParameterType.SiteParameterTypeDS.DbValue

		' SiteParameterTypeOrder
		SiteParameterType.SiteParameterTypeOrder.SetDbValue(SiteParameterType.SiteParameterTypeOrder.CurrentValue, System.DBNull.Value)
		Rs("SiteParameterTypeOrder") = SiteParameterType.SiteParameterTypeOrder.DbValue

		' SiteParameterTemplate
		SiteParameterType.SiteParameterTemplate.SetDbValue(SiteParameterType.SiteParameterTemplate.CurrentValue, System.DBNull.Value)
		Rs("SiteParameterTemplate") = SiteParameterType.SiteParameterTemplate.DbValue

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
				Message = "Insert cancelled"
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
		Dim table As String = "SiteParameterType"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsSrc("SiteParameterTypeID")

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

		' SiteParameterTypeNM Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteParameterTypeNM", keyvalue, oldvalue, RsSrc("SiteParameterTypeNM"))

		' SiteParameterTypeDS Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteParameterTypeDS", keyvalue, oldvalue, RsSrc("SiteParameterTypeDS"))

		' SiteParameterTypeOrder Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteParameterTypeOrder", keyvalue, oldvalue, RsSrc("SiteParameterTypeOrder"))

		' SiteParameterTemplate Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteParameterTemplate", keyvalue, oldvalue, "<MEMO>")
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
		SiteParameterType_add = New cSiteParameterType_add(Me)		
		SiteParameterType_add.Page_Init()

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
