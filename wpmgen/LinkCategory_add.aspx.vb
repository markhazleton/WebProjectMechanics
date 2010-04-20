Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class LinkCategory_add
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public LinkCategory_add As cLinkCategory_add

	'
	' Page Class
	'
	Class cLinkCategory_add
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
				If LinkCategory.UseTokenInUrl Then Url = Url & "t=" & LinkCategory.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If LinkCategory.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (LinkCategory.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (LinkCategory.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' LinkCategory
		Public Property LinkCategory() As cLinkCategory
			Get				
				Return ParentPage.LinkCategory
			End Get
			Set(ByVal v As cLinkCategory)
				ParentPage.LinkCategory = v	
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
			m_PageObjName = "LinkCategory_add"
			m_PageObjTypeName = "cLinkCategory_add"

			' Table Name
			m_TableName = "LinkCategory"

			' Initialize table object
			LinkCategory = New cLinkCategory(Me)

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
			LinkCategory.Dispose()

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
		If ew_Get("ID") <> "" Then
			LinkCategory.ID.QueryStringValue = ew_Get("ID")
		Else
			bCopy = False
		End If

		' Create form object
		ObjForm = New cFormObj

		' Process form if post back
		If ObjForm.GetValue("a_add") <> "" Then
			LinkCategory.CurrentAction = ObjForm.GetValue("a_add") ' Get form action
			LoadFormValues() ' Load form values

			' Validate Form
			If Not ValidateForm() Then
				LinkCategory.CurrentAction = "I" ' Form error, reset action
				Message = ParentPage.gsFormError
			End If

		' Not post back
		Else
			If bCopy Then
				LinkCategory.CurrentAction = "C" ' Copy Record
			Else
				LinkCategory.CurrentAction = "I" ' Display Blank Record
				LoadDefaultValues() ' Load default values
			End If
		End If

		' Perform action based on action code
		Select Case LinkCategory.CurrentAction
			Case "I" ' Blank record, no action required
			Case "C" ' Copy an existing record
				If Not LoadRow() Then ' Load record based on key
					Message = "No records found" ' No record found
					Page_Terminate("LinkCategory_list.aspx") ' No matching record, return to list
				End If
			Case "A" ' Add new record
				LinkCategory.SendEmail = True ' Send email on add success
				If AddRow() Then ' Add successful
					Message = "Add succeeded" ' Set up success message
					Dim sReturnUrl As String = LinkCategory.ReturnUrl
					If ew_GetPageName(sReturnUrl) = "LinkCategory_view.aspx" Then sReturnUrl = LinkCategory.ViewUrl ' View paging, return to view page with keyurl directly
					Page_Terminate(sReturnUrl) ' Clean up and return
				Else
					RestoreFormValues() ' Add failed, restore form values
				End If
		End Select

		' Render row based on row type
		LinkCategory.RowType = EW_ROWTYPE_ADD ' Render add type

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
		LinkCategory.ParentID.CurrentValue = 0
		LinkCategory.zPageID.CurrentValue = 0
	End Sub

	'
	' Load form values
	'
	Sub LoadFormValues()
		LinkCategory.Title.FormValue = ObjForm.GetValue("x_Title")
		LinkCategory.Title.OldValue = ObjForm.GetValue("o_Title")
		LinkCategory.Description.FormValue = ObjForm.GetValue("x_Description")
		LinkCategory.Description.OldValue = ObjForm.GetValue("o_Description")
		LinkCategory.ParentID.FormValue = ObjForm.GetValue("x_ParentID")
		LinkCategory.ParentID.OldValue = ObjForm.GetValue("o_ParentID")
		LinkCategory.zPageID.FormValue = ObjForm.GetValue("x_zPageID")
		LinkCategory.zPageID.OldValue = ObjForm.GetValue("o_zPageID")
		LinkCategory.ID.FormValue = ObjForm.GetValue("x_ID")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		LinkCategory.Title.CurrentValue = LinkCategory.Title.FormValue
		LinkCategory.Description.CurrentValue = LinkCategory.Description.FormValue
		LinkCategory.ParentID.CurrentValue = LinkCategory.ParentID.FormValue
		LinkCategory.zPageID.CurrentValue = LinkCategory.zPageID.FormValue
		LinkCategory.ID.CurrentValue = LinkCategory.ID.FormValue
	End Sub

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = LinkCategory.KeyFilter

		' Row Selecting event
		LinkCategory.Row_Selecting(sFilter)

		' Load SQL based on filter
		LinkCategory.CurrentFilter = sFilter
		Dim sSql As String = LinkCategory.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				LinkCategory.Row_Selected(RsRow)
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
		LinkCategory.ID.DbValue = RsRow("ID")
		LinkCategory.Title.DbValue = RsRow("Title")
		LinkCategory.Description.DbValue = RsRow("Description")
		LinkCategory.ParentID.DbValue = RsRow("ParentID")
		LinkCategory.zPageID.DbValue = RsRow("PageID")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		LinkCategory.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' Title

		LinkCategory.Title.CellCssStyle = ""
		LinkCategory.Title.CellCssClass = ""

		' Description
		LinkCategory.Description.CellCssStyle = ""
		LinkCategory.Description.CellCssClass = ""

		' ParentID
		LinkCategory.ParentID.CellCssStyle = ""
		LinkCategory.ParentID.CellCssClass = ""

		' PageID
		LinkCategory.zPageID.CellCssStyle = ""
		LinkCategory.zPageID.CellCssClass = ""

		'
		'  View  Row
		'

		If LinkCategory.RowType = EW_ROWTYPE_VIEW Then ' View row

			' ID
			LinkCategory.ID.ViewValue = LinkCategory.ID.CurrentValue
			LinkCategory.ID.CssStyle = ""
			LinkCategory.ID.CssClass = ""
			LinkCategory.ID.ViewCustomAttributes = ""

			' Title
			LinkCategory.Title.ViewValue = LinkCategory.Title.CurrentValue
			LinkCategory.Title.CssStyle = ""
			LinkCategory.Title.CssClass = ""
			LinkCategory.Title.ViewCustomAttributes = ""

			' Description
			LinkCategory.Description.ViewValue = LinkCategory.Description.CurrentValue
			LinkCategory.Description.CssStyle = ""
			LinkCategory.Description.CssClass = ""
			LinkCategory.Description.ViewCustomAttributes = ""

			' ParentID
			If ew_NotEmpty(LinkCategory.ParentID.CurrentValue) Then
				sSqlWrk = "SELECT [Title] FROM [LinkCategory] WHERE [ID] = " & ew_AdjustSql(LinkCategory.ParentID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [Title] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					LinkCategory.ParentID.ViewValue = RsWrk("Title")
				Else
					LinkCategory.ParentID.ViewValue = LinkCategory.ParentID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				LinkCategory.ParentID.ViewValue = System.DBNull.Value
			End If
			LinkCategory.ParentID.CssStyle = ""
			LinkCategory.ParentID.CssClass = ""
			LinkCategory.ParentID.ViewCustomAttributes = ""

			' PageID
			If ew_NotEmpty(LinkCategory.zPageID.CurrentValue) Then
				sSqlWrk = "SELECT [PageName] FROM [Page] WHERE [PageID] = " & ew_AdjustSql(LinkCategory.zPageID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [PageName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					LinkCategory.zPageID.ViewValue = RsWrk("PageName")
				Else
					LinkCategory.zPageID.ViewValue = LinkCategory.zPageID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				LinkCategory.zPageID.ViewValue = System.DBNull.Value
			End If
			LinkCategory.zPageID.CssStyle = ""
			LinkCategory.zPageID.CssClass = ""
			LinkCategory.zPageID.ViewCustomAttributes = ""

			' View refer script
			' Title

			LinkCategory.Title.HrefValue = ""

			' Description
			LinkCategory.Description.HrefValue = ""

			' ParentID
			LinkCategory.ParentID.HrefValue = ""

			' PageID
			LinkCategory.zPageID.HrefValue = ""

		'
		'  Add Row
		'

		ElseIf LinkCategory.RowType = EW_ROWTYPE_ADD Then ' Add row

			' Title
			LinkCategory.Title.EditCustomAttributes = ""
			LinkCategory.Title.EditValue = ew_HtmlEncode(LinkCategory.Title.CurrentValue)

			' Description
			LinkCategory.Description.EditCustomAttributes = ""
			LinkCategory.Description.EditValue = ew_HtmlEncode(LinkCategory.Description.CurrentValue)

			' ParentID
			LinkCategory.ParentID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [ID], [Title], '' AS Disp2Fld, '' AS SelectFilterFld FROM [LinkCategory]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [Title] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			LinkCategory.ParentID.EditValue = arwrk

			' PageID
			LinkCategory.zPageID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [PageID], [PageName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Page]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [PageName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			LinkCategory.zPageID.EditValue = arwrk
		End If

		' Row Rendered event
		LinkCategory.Row_Rendered()
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

		' Title
		LinkCategory.Title.SetDbValue(LinkCategory.Title.CurrentValue, System.DBNull.Value)
		Rs("Title") = LinkCategory.Title.DbValue

		' Description
		LinkCategory.Description.SetDbValue(LinkCategory.Description.CurrentValue, System.DBNull.Value)
		Rs("Description") = LinkCategory.Description.DbValue

		' ParentID
		LinkCategory.ParentID.SetDbValue(LinkCategory.ParentID.CurrentValue, System.DBNull.Value)
		Rs("ParentID") = LinkCategory.ParentID.DbValue

		' PageID
		LinkCategory.zPageID.SetDbValue(LinkCategory.zPageID.CurrentValue, System.DBNull.Value)
		Rs("PageID") = LinkCategory.zPageID.DbValue

		' Row Inserting event
		bInsertRow = LinkCategory.Row_Inserting(Rs)
		If bInsertRow Then
			Try	
				LinkCategory.Insert(Rs)
				AddRow = True
			Catch e As Exception
				If EW_DEBUG_ENABLED Then Throw
				Message = e.Message				
				AddRow = False
			End Try
		Else
			If LinkCategory.CancelMessage <> "" Then
				Message = LinkCategory.CancelMessage
				LinkCategory.CancelMessage = ""
			Else
				Message = "Insert cancelled"
			End If
			AddRow = False
		End If
		If AddRow Then
			LastInsertId = Conn.GetLastInsertId()
			LinkCategory.ID.DbValue = LastInsertId
			Rs("ID") = LinkCategory.ID.DbValue		

			' Row Inserted event
			LinkCategory.Row_Inserted(Rs)
			WriteAuditTrailOnAdd(Rs)
		End If
	End Function

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "LinkCategory"
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
		Dim table As String = "LinkCategory"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsSrc("ID")

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

		' Title Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "Title", keyvalue, oldvalue, RsSrc("Title"))

		' Description Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "Description", keyvalue, oldvalue, "<MEMO>")

		' ParentID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "ParentID", keyvalue, oldvalue, RsSrc("ParentID"))

		' PageID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "PageID", keyvalue, oldvalue, RsSrc("PageID"))
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
		LinkCategory_add = New cLinkCategory_add(Me)		
		LinkCategory_add.Page_Init()

		' Page main processing
		LinkCategory_add.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If LinkCategory_add IsNot Nothing Then LinkCategory_add.Dispose()
	End Sub
End Class
