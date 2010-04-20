Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class PageAlias_add
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public PageAlias_add As cPageAlias_add

	'
	' Page Class
	'
	Class cPageAlias_add
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
				If PageAlias.UseTokenInUrl Then Url = Url & "t=" & PageAlias.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If PageAlias.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (PageAlias.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (PageAlias.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' PageAlias
		Public Property PageAlias() As cPageAlias
			Get				
				Return ParentPage.PageAlias
			End Get
			Set(ByVal v As cPageAlias)
				ParentPage.PageAlias = v	
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
			m_PageObjName = "PageAlias_add"
			m_PageObjTypeName = "cPageAlias_add"

			' Table Name
			m_TableName = "PageAlias"

			' Initialize table object
			PageAlias = New cPageAlias(Me)

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
			PageAlias.Dispose()

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
		If ew_Get("PageAliasID") <> "" Then
			PageAlias.PageAliasID.QueryStringValue = ew_Get("PageAliasID")
		Else
			bCopy = False
		End If

		' Create form object
		ObjForm = New cFormObj

		' Process form if post back
		If ObjForm.GetValue("a_add") <> "" Then
			PageAlias.CurrentAction = ObjForm.GetValue("a_add") ' Get form action
			LoadFormValues() ' Load form values

			' Validate Form
			If Not ValidateForm() Then
				PageAlias.CurrentAction = "I" ' Form error, reset action
				Message = ParentPage.gsFormError
			End If

		' Not post back
		Else
			If bCopy Then
				PageAlias.CurrentAction = "C" ' Copy Record
			Else
				PageAlias.CurrentAction = "I" ' Display Blank Record
				LoadDefaultValues() ' Load default values
			End If
		End If

		' Perform action based on action code
		Select Case PageAlias.CurrentAction
			Case "I" ' Blank record, no action required
			Case "C" ' Copy an existing record
				If Not LoadRow() Then ' Load record based on key
					Message = "No records found" ' No record found
					Page_Terminate("PageAlias_list.aspx") ' No matching record, return to list
				End If
			Case "A" ' Add new record
				PageAlias.SendEmail = True ' Send email on add success
				If AddRow() Then ' Add successful
					Message = "Add succeeded" ' Set up success message
					Dim sReturnUrl As String = PageAlias.ReturnUrl
					If ew_GetPageName(sReturnUrl) = "PageAlias_view.aspx" Then sReturnUrl = PageAlias.ViewUrl ' View paging, return to view page with keyurl directly
					Page_Terminate(sReturnUrl) ' Clean up and return
				Else
					RestoreFormValues() ' Add failed, restore form values
				End If
		End Select

		' Render row based on row type
		PageAlias.RowType = EW_ROWTYPE_ADD ' Render add type

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
		PageAlias.zPageURL.FormValue = ObjForm.GetValue("x_zPageURL")
		PageAlias.zPageURL.OldValue = ObjForm.GetValue("o_zPageURL")
		PageAlias.TargetURL.FormValue = ObjForm.GetValue("x_TargetURL")
		PageAlias.TargetURL.OldValue = ObjForm.GetValue("o_TargetURL")
		PageAlias.AliasType.FormValue = ObjForm.GetValue("x_AliasType")
		PageAlias.AliasType.OldValue = ObjForm.GetValue("o_AliasType")
		PageAlias.CompanyID.FormValue = ObjForm.GetValue("x_CompanyID")
		PageAlias.CompanyID.OldValue = ObjForm.GetValue("o_CompanyID")
		PageAlias.PageAliasID.FormValue = ObjForm.GetValue("x_PageAliasID")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		PageAlias.zPageURL.CurrentValue = PageAlias.zPageURL.FormValue
		PageAlias.TargetURL.CurrentValue = PageAlias.TargetURL.FormValue
		PageAlias.AliasType.CurrentValue = PageAlias.AliasType.FormValue
		PageAlias.CompanyID.CurrentValue = PageAlias.CompanyID.FormValue
		PageAlias.PageAliasID.CurrentValue = PageAlias.PageAliasID.FormValue
	End Sub

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = PageAlias.KeyFilter

		' Row Selecting event
		PageAlias.Row_Selecting(sFilter)

		' Load SQL based on filter
		PageAlias.CurrentFilter = sFilter
		Dim sSql As String = PageAlias.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				PageAlias.Row_Selected(RsRow)
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
		PageAlias.PageAliasID.DbValue = RsRow("PageAliasID")
		PageAlias.zPageURL.DbValue = RsRow("PageURL")
		PageAlias.TargetURL.DbValue = RsRow("TargetURL")
		PageAlias.AliasType.DbValue = RsRow("AliasType")
		PageAlias.CompanyID.DbValue = RsRow("CompanyID")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		PageAlias.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' PageURL

		PageAlias.zPageURL.CellCssStyle = ""
		PageAlias.zPageURL.CellCssClass = ""

		' TargetURL
		PageAlias.TargetURL.CellCssStyle = ""
		PageAlias.TargetURL.CellCssClass = ""

		' AliasType
		PageAlias.AliasType.CellCssStyle = ""
		PageAlias.AliasType.CellCssClass = ""

		' CompanyID
		PageAlias.CompanyID.CellCssStyle = ""
		PageAlias.CompanyID.CellCssClass = ""

		'
		'  View  Row
		'

		If PageAlias.RowType = EW_ROWTYPE_VIEW Then ' View row

			' PageURL
			PageAlias.zPageURL.ViewValue = PageAlias.zPageURL.CurrentValue
			PageAlias.zPageURL.CssStyle = ""
			PageAlias.zPageURL.CssClass = ""
			PageAlias.zPageURL.ViewCustomAttributes = ""

			' TargetURL
			PageAlias.TargetURL.ViewValue = PageAlias.TargetURL.CurrentValue
			PageAlias.TargetURL.CssStyle = ""
			PageAlias.TargetURL.CssClass = ""
			PageAlias.TargetURL.ViewCustomAttributes = ""

			' AliasType
			PageAlias.AliasType.ViewValue = PageAlias.AliasType.CurrentValue
			PageAlias.AliasType.CssStyle = ""
			PageAlias.AliasType.CssClass = ""
			PageAlias.AliasType.ViewCustomAttributes = ""

			' CompanyID
			If ew_NotEmpty(PageAlias.CompanyID.CurrentValue) Then
				sSqlWrk = "SELECT [CompanyName] FROM [Company] WHERE [CompanyID] = " & ew_AdjustSql(PageAlias.CompanyID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					PageAlias.CompanyID.ViewValue = RsWrk("CompanyName")
				Else
					PageAlias.CompanyID.ViewValue = PageAlias.CompanyID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				PageAlias.CompanyID.ViewValue = System.DBNull.Value
			End If
			PageAlias.CompanyID.CssStyle = ""
			PageAlias.CompanyID.CssClass = ""
			PageAlias.CompanyID.ViewCustomAttributes = ""

			' View refer script
			' PageURL

			PageAlias.zPageURL.HrefValue = ""

			' TargetURL
			PageAlias.TargetURL.HrefValue = ""

			' AliasType
			PageAlias.AliasType.HrefValue = ""

			' CompanyID
			PageAlias.CompanyID.HrefValue = ""

		'
		'  Add Row
		'

		ElseIf PageAlias.RowType = EW_ROWTYPE_ADD Then ' Add row

			' PageURL
			PageAlias.zPageURL.EditCustomAttributes = ""
			PageAlias.zPageURL.EditValue = ew_HtmlEncode(PageAlias.zPageURL.CurrentValue)

			' TargetURL
			PageAlias.TargetURL.EditCustomAttributes = ""
			PageAlias.TargetURL.EditValue = ew_HtmlEncode(PageAlias.TargetURL.CurrentValue)

			' AliasType
			PageAlias.AliasType.EditCustomAttributes = ""
			PageAlias.AliasType.EditValue = ew_HtmlEncode(PageAlias.AliasType.CurrentValue)

			' CompanyID
			PageAlias.CompanyID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [CompanyID], [CompanyName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Company]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			PageAlias.CompanyID.EditValue = arwrk
		End If

		' Row Rendered event
		PageAlias.Row_Rendered()
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

		' PageURL
		PageAlias.zPageURL.SetDbValue(PageAlias.zPageURL.CurrentValue, System.DBNull.Value)
		Rs("PageURL") = PageAlias.zPageURL.DbValue

		' TargetURL
		PageAlias.TargetURL.SetDbValue(PageAlias.TargetURL.CurrentValue, System.DBNull.Value)
		Rs("TargetURL") = PageAlias.TargetURL.DbValue

		' AliasType
		PageAlias.AliasType.SetDbValue(PageAlias.AliasType.CurrentValue, System.DBNull.Value)
		Rs("AliasType") = PageAlias.AliasType.DbValue

		' CompanyID
		PageAlias.CompanyID.SetDbValue(PageAlias.CompanyID.CurrentValue, System.DBNull.Value)
		Rs("CompanyID") = PageAlias.CompanyID.DbValue

		' Row Inserting event
		bInsertRow = PageAlias.Row_Inserting(Rs)
		If bInsertRow Then
			Try	
				PageAlias.Insert(Rs)
				AddRow = True
			Catch e As Exception
				If EW_DEBUG_ENABLED Then Throw
				Message = e.Message				
				AddRow = False
			End Try
		Else
			If PageAlias.CancelMessage <> "" Then
				Message = PageAlias.CancelMessage
				PageAlias.CancelMessage = ""
			Else
				Message = "Insert cancelled"
			End If
			AddRow = False
		End If
		If AddRow Then
			LastInsertId = Conn.GetLastInsertId()
			PageAlias.PageAliasID.DbValue = LastInsertId
			Rs("PageAliasID") = PageAlias.PageAliasID.DbValue		

			' Row Inserted event
			PageAlias.Row_Inserted(Rs)
			WriteAuditTrailOnAdd(Rs)
		End If
	End Function

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "PageAlias"
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
		Dim table As String = "PageAlias"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsSrc("PageAliasID")

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

		' PageURL Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "PageURL", keyvalue, oldvalue, RsSrc("PageURL"))

		' TargetURL Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "TargetURL", keyvalue, oldvalue, RsSrc("TargetURL"))

		' AliasType Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "AliasType", keyvalue, oldvalue, RsSrc("AliasType"))

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
		PageAlias_add = New cPageAlias_add(Me)		
		PageAlias_add.Page_Init()

		' Page main processing
		PageAlias_add.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If PageAlias_add IsNot Nothing Then PageAlias_add.Dispose()
	End Sub
End Class
