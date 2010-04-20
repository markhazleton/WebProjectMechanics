Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class Group_add
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public Group_add As cGroup_add

	'
	' Page Class
	'
	Class cGroup_add
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
				If Group.UseTokenInUrl Then Url = Url & "t=" & Group.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If Group.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (Group.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (Group.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' Group
		Public Property Group() As cGroup
			Get				
				Return ParentPage.Group
			End Get
			Set(ByVal v As cGroup)
				ParentPage.Group = v	
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
			m_PageObjName = "Group_add"
			m_PageObjTypeName = "cGroup_add"

			' Table Name
			m_TableName = "Group"

			' Initialize table object
			Group = New cGroup(Me)

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
			Group.Dispose()

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
		If ew_Get("GroupID") <> "" Then
			Group.GroupID.QueryStringValue = ew_Get("GroupID")
		Else
			bCopy = False
		End If

		' Create form object
		ObjForm = New cFormObj

		' Process form if post back
		If ObjForm.GetValue("a_add") <> "" Then
			Group.CurrentAction = ObjForm.GetValue("a_add") ' Get form action
			LoadFormValues() ' Load form values

			' Validate Form
			If Not ValidateForm() Then
				Group.CurrentAction = "I" ' Form error, reset action
				Message = ParentPage.gsFormError
			End If

		' Not post back
		Else
			If bCopy Then
				Group.CurrentAction = "C" ' Copy Record
			Else
				Group.CurrentAction = "I" ' Display Blank Record
				LoadDefaultValues() ' Load default values
			End If
		End If

		' Perform action based on action code
		Select Case Group.CurrentAction
			Case "I" ' Blank record, no action required
			Case "C" ' Copy an existing record
				If Not LoadRow() Then ' Load record based on key
					Message = "No records found" ' No record found
					Page_Terminate("Group_list.aspx") ' No matching record, return to list
				End If
			Case "A" ' Add new record
				Group.SendEmail = True ' Send email on add success
				If AddRow() Then ' Add successful
					Message = "Add succeeded" ' Set up success message
					Dim sReturnUrl As String = Group.ReturnUrl
					If ew_GetPageName(sReturnUrl) = "Group_view.aspx" Then sReturnUrl = Group.ViewUrl ' View paging, return to view page with keyurl directly
					Page_Terminate(sReturnUrl) ' Clean up and return
				Else
					RestoreFormValues() ' Add failed, restore form values
				End If
		End Select

		' Render row based on row type
		Group.RowType = EW_ROWTYPE_ADD ' Render add type

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
		Group.GroupName.FormValue = ObjForm.GetValue("x_GroupName")
		Group.GroupName.OldValue = ObjForm.GetValue("o_GroupName")
		Group.GroupComment.FormValue = ObjForm.GetValue("x_GroupComment")
		Group.GroupComment.OldValue = ObjForm.GetValue("o_GroupComment")
		Group.GroupID.FormValue = ObjForm.GetValue("x_GroupID")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		Group.GroupName.CurrentValue = Group.GroupName.FormValue
		Group.GroupComment.CurrentValue = Group.GroupComment.FormValue
		Group.GroupID.CurrentValue = Group.GroupID.FormValue
	End Sub

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = Group.KeyFilter

		' Row Selecting event
		Group.Row_Selecting(sFilter)

		' Load SQL based on filter
		Group.CurrentFilter = sFilter
		Dim sSql As String = Group.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				Group.Row_Selected(RsRow)
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
		Group.GroupID.DbValue = RsRow("GroupID")
		Group.GroupName.DbValue = RsRow("GroupName")
		Group.GroupComment.DbValue = RsRow("GroupComment")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		Group.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' GroupName

		Group.GroupName.CellCssStyle = ""
		Group.GroupName.CellCssClass = ""

		' GroupComment
		Group.GroupComment.CellCssStyle = ""
		Group.GroupComment.CellCssClass = ""

		'
		'  View  Row
		'

		If Group.RowType = EW_ROWTYPE_VIEW Then ' View row

			' GroupName
			Group.GroupName.ViewValue = Group.GroupName.CurrentValue
			Group.GroupName.CssStyle = ""
			Group.GroupName.CssClass = ""
			Group.GroupName.ViewCustomAttributes = ""

			' GroupComment
			Group.GroupComment.ViewValue = Group.GroupComment.CurrentValue
			Group.GroupComment.CssStyle = ""
			Group.GroupComment.CssClass = ""
			Group.GroupComment.ViewCustomAttributes = ""

			' View refer script
			' GroupName

			Group.GroupName.HrefValue = ""

			' GroupComment
			Group.GroupComment.HrefValue = ""

		'
		'  Add Row
		'

		ElseIf Group.RowType = EW_ROWTYPE_ADD Then ' Add row

			' GroupName
			Group.GroupName.EditCustomAttributes = ""
			Group.GroupName.EditValue = ew_HtmlEncode(Group.GroupName.CurrentValue)

			' GroupComment
			Group.GroupComment.EditCustomAttributes = ""
			Group.GroupComment.EditValue = ew_HtmlEncode(Group.GroupComment.CurrentValue)
		End If

		' Row Rendered event
		Group.Row_Rendered()
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

		' GroupName
		Group.GroupName.SetDbValue(Group.GroupName.CurrentValue, System.DBNull.Value)
		Rs("GroupName") = Group.GroupName.DbValue

		' GroupComment
		Group.GroupComment.SetDbValue(Group.GroupComment.CurrentValue, System.DBNull.Value)
		Rs("GroupComment") = Group.GroupComment.DbValue

		' Row Inserting event
		bInsertRow = Group.Row_Inserting(Rs)
		If bInsertRow Then
			Try	
				Group.Insert(Rs)
				AddRow = True
			Catch e As Exception
				If EW_DEBUG_ENABLED Then Throw
				Message = e.Message				
				AddRow = False
			End Try
		Else
			If Group.CancelMessage <> "" Then
				Message = Group.CancelMessage
				Group.CancelMessage = ""
			Else
				Message = "Insert cancelled"
			End If
			AddRow = False
		End If
		If AddRow Then
			LastInsertId = Conn.GetLastInsertId()
			Group.GroupID.DbValue = LastInsertId
			Rs("GroupID") = Group.GroupID.DbValue		

			' Row Inserted event
			Group.Row_Inserted(Rs)
			WriteAuditTrailOnAdd(Rs)
		End If
	End Function

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "Group"
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
		Dim table As String = "Group"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsSrc("GroupID")

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

		' GroupName Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "GroupName", keyvalue, oldvalue, RsSrc("GroupName"))

		' GroupComment Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "GroupComment", keyvalue, oldvalue, RsSrc("GroupComment"))
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
		Group_add = New cGroup_add(Me)		
		Group_add.Page_Init()

		' Page main processing
		Group_add.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If Group_add IsNot Nothing Then Group_add.Dispose()
	End Sub
End Class
