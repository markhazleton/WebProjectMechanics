Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class LinkRank_add
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public LinkRank_add As cLinkRank_add

	'
	' Page Class
	'
	Class cLinkRank_add
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
				If LinkRank.UseTokenInUrl Then Url = Url & "t=" & LinkRank.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If LinkRank.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (LinkRank.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (LinkRank.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' LinkRank
		Public Property LinkRank() As cLinkRank
			Get				
				Return ParentPage.LinkRank
			End Get
			Set(ByVal v As cLinkRank)
				ParentPage.LinkRank = v	
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
			m_PageObjName = "LinkRank_add"
			m_PageObjTypeName = "cLinkRank_add"

			' Table Name
			m_TableName = "LinkRank"

			' Initialize table object
			LinkRank = New cLinkRank(Me)

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
			LinkRank.Dispose()

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
			LinkRank.ID.QueryStringValue = ew_Get("ID")
		Else
			bCopy = False
		End If

		' Create form object
		ObjForm = New cFormObj

		' Process form if post back
		If ObjForm.GetValue("a_add") <> "" Then
			LinkRank.CurrentAction = ObjForm.GetValue("a_add") ' Get form action
			LoadFormValues() ' Load form values

			' Validate Form
			If Not ValidateForm() Then
				LinkRank.CurrentAction = "I" ' Form error, reset action
				Message = ParentPage.gsFormError
			End If

		' Not post back
		Else
			If bCopy Then
				LinkRank.CurrentAction = "C" ' Copy Record
			Else
				LinkRank.CurrentAction = "I" ' Display Blank Record
				LoadDefaultValues() ' Load default values
			End If
		End If

		' Perform action based on action code
		Select Case LinkRank.CurrentAction
			Case "I" ' Blank record, no action required
			Case "C" ' Copy an existing record
				If Not LoadRow() Then ' Load record based on key
					Message = "No records found" ' No record found
					Page_Terminate("LinkRank_list.aspx") ' No matching record, return to list
				End If
			Case "A" ' Add new record
				LinkRank.SendEmail = True ' Send email on add success
				If AddRow() Then ' Add successful
					Message = "Add succeeded" ' Set up success message
					Dim sReturnUrl As String = LinkRank.ReturnUrl
					If ew_GetPageName(sReturnUrl) = "LinkRank_view.aspx" Then sReturnUrl = LinkRank.ViewUrl ' View paging, return to view page with keyurl directly
					Page_Terminate(sReturnUrl) ' Clean up and return
				Else
					RestoreFormValues() ' Add failed, restore form values
				End If
		End Select

		' Render row based on row type
		LinkRank.RowType = EW_ROWTYPE_ADD ' Render add type

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
		LinkRank.LinkID.CurrentValue = 0
		LinkRank.UserID.CurrentValue = 0
		LinkRank.RankNum.CurrentValue = 0
		LinkRank.CateID.CurrentValue = 0
	End Sub

	'
	' Load form values
	'
	Sub LoadFormValues()
		LinkRank.LinkID.FormValue = ObjForm.GetValue("x_LinkID")
		LinkRank.LinkID.OldValue = ObjForm.GetValue("o_LinkID")
		LinkRank.UserID.FormValue = ObjForm.GetValue("x_UserID")
		LinkRank.UserID.OldValue = ObjForm.GetValue("o_UserID")
		LinkRank.RankNum.FormValue = ObjForm.GetValue("x_RankNum")
		LinkRank.RankNum.OldValue = ObjForm.GetValue("o_RankNum")
		LinkRank.CateID.FormValue = ObjForm.GetValue("x_CateID")
		LinkRank.CateID.OldValue = ObjForm.GetValue("o_CateID")
		LinkRank.Comment.FormValue = ObjForm.GetValue("x_Comment")
		LinkRank.Comment.OldValue = ObjForm.GetValue("o_Comment")
		LinkRank.ID.FormValue = ObjForm.GetValue("x_ID")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		LinkRank.LinkID.CurrentValue = LinkRank.LinkID.FormValue
		LinkRank.UserID.CurrentValue = LinkRank.UserID.FormValue
		LinkRank.RankNum.CurrentValue = LinkRank.RankNum.FormValue
		LinkRank.CateID.CurrentValue = LinkRank.CateID.FormValue
		LinkRank.Comment.CurrentValue = LinkRank.Comment.FormValue
		LinkRank.ID.CurrentValue = LinkRank.ID.FormValue
	End Sub

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = LinkRank.KeyFilter

		' Row Selecting event
		LinkRank.Row_Selecting(sFilter)

		' Load SQL based on filter
		LinkRank.CurrentFilter = sFilter
		Dim sSql As String = LinkRank.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				LinkRank.Row_Selected(RsRow)
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
		LinkRank.ID.DbValue = RsRow("ID")
		LinkRank.LinkID.DbValue = RsRow("LinkID")
		LinkRank.UserID.DbValue = RsRow("UserID")
		LinkRank.RankNum.DbValue = RsRow("RankNum")
		LinkRank.CateID.DbValue = RsRow("CateID")
		LinkRank.Comment.DbValue = RsRow("Comment")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		LinkRank.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' LinkID

		LinkRank.LinkID.CellCssStyle = ""
		LinkRank.LinkID.CellCssClass = ""

		' UserID
		LinkRank.UserID.CellCssStyle = ""
		LinkRank.UserID.CellCssClass = ""

		' RankNum
		LinkRank.RankNum.CellCssStyle = ""
		LinkRank.RankNum.CellCssClass = ""

		' CateID
		LinkRank.CateID.CellCssStyle = ""
		LinkRank.CateID.CellCssClass = ""

		' Comment
		LinkRank.Comment.CellCssStyle = ""
		LinkRank.Comment.CellCssClass = ""

		'
		'  View  Row
		'

		If LinkRank.RowType = EW_ROWTYPE_VIEW Then ' View row

			' ID
			LinkRank.ID.ViewValue = LinkRank.ID.CurrentValue
			LinkRank.ID.CssStyle = ""
			LinkRank.ID.CssClass = ""
			LinkRank.ID.ViewCustomAttributes = ""

			' LinkID
			LinkRank.LinkID.ViewValue = LinkRank.LinkID.CurrentValue
			LinkRank.LinkID.CssStyle = ""
			LinkRank.LinkID.CssClass = ""
			LinkRank.LinkID.ViewCustomAttributes = ""

			' UserID
			LinkRank.UserID.ViewValue = LinkRank.UserID.CurrentValue
			LinkRank.UserID.CssStyle = ""
			LinkRank.UserID.CssClass = ""
			LinkRank.UserID.ViewCustomAttributes = ""

			' RankNum
			LinkRank.RankNum.ViewValue = LinkRank.RankNum.CurrentValue
			LinkRank.RankNum.CssStyle = ""
			LinkRank.RankNum.CssClass = ""
			LinkRank.RankNum.ViewCustomAttributes = ""

			' CateID
			LinkRank.CateID.ViewValue = LinkRank.CateID.CurrentValue
			LinkRank.CateID.CssStyle = ""
			LinkRank.CateID.CssClass = ""
			LinkRank.CateID.ViewCustomAttributes = ""

			' Comment
			LinkRank.Comment.ViewValue = LinkRank.Comment.CurrentValue
			LinkRank.Comment.CssStyle = ""
			LinkRank.Comment.CssClass = ""
			LinkRank.Comment.ViewCustomAttributes = ""

			' View refer script
			' LinkID

			LinkRank.LinkID.HrefValue = ""

			' UserID
			LinkRank.UserID.HrefValue = ""

			' RankNum
			LinkRank.RankNum.HrefValue = ""

			' CateID
			LinkRank.CateID.HrefValue = ""

			' Comment
			LinkRank.Comment.HrefValue = ""

		'
		'  Add Row
		'

		ElseIf LinkRank.RowType = EW_ROWTYPE_ADD Then ' Add row

			' LinkID
			LinkRank.LinkID.EditCustomAttributes = ""
			LinkRank.LinkID.EditValue = ew_HtmlEncode(LinkRank.LinkID.CurrentValue)

			' UserID
			LinkRank.UserID.EditCustomAttributes = ""
			LinkRank.UserID.EditValue = ew_HtmlEncode(LinkRank.UserID.CurrentValue)

			' RankNum
			LinkRank.RankNum.EditCustomAttributes = ""
			LinkRank.RankNum.EditValue = ew_HtmlEncode(LinkRank.RankNum.CurrentValue)

			' CateID
			LinkRank.CateID.EditCustomAttributes = ""
			LinkRank.CateID.EditValue = ew_HtmlEncode(LinkRank.CateID.CurrentValue)

			' Comment
			LinkRank.Comment.EditCustomAttributes = ""
			LinkRank.Comment.EditValue = ew_HtmlEncode(LinkRank.Comment.CurrentValue)
		End If

		' Row Rendered event
		LinkRank.Row_Rendered()
	End Sub

	'
	' Validate form
	'
	Function ValidateForm() As Boolean

		' Initialize
		ParentPage.gsFormError = ""

		' Check if validation required
		If Not EW_SERVER_VALIDATE Then Return (ParentPage.gsFormError = "")
		If Not ew_CheckInteger(LinkRank.LinkID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Incorrect integer - Link ID"
		End If
		If Not ew_CheckInteger(LinkRank.UserID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Incorrect integer - User ID"
		End If
		If Not ew_CheckInteger(LinkRank.RankNum.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Incorrect integer - Rank Num"
		End If
		If Not ew_CheckInteger(LinkRank.CateID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Incorrect integer - Cate ID"
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

		' LinkID
		LinkRank.LinkID.SetDbValue(LinkRank.LinkID.CurrentValue, System.DBNull.Value)
		Rs("LinkID") = LinkRank.LinkID.DbValue

		' UserID
		LinkRank.UserID.SetDbValue(LinkRank.UserID.CurrentValue, System.DBNull.Value)
		Rs("UserID") = LinkRank.UserID.DbValue

		' RankNum
		LinkRank.RankNum.SetDbValue(LinkRank.RankNum.CurrentValue, System.DBNull.Value)
		Rs("RankNum") = LinkRank.RankNum.DbValue

		' CateID
		LinkRank.CateID.SetDbValue(LinkRank.CateID.CurrentValue, System.DBNull.Value)
		Rs("CateID") = LinkRank.CateID.DbValue

		' Comment
		LinkRank.Comment.SetDbValue(LinkRank.Comment.CurrentValue, System.DBNull.Value)
		Rs("Comment") = LinkRank.Comment.DbValue

		' Row Inserting event
		bInsertRow = LinkRank.Row_Inserting(Rs)
		If bInsertRow Then
			Try	
				LinkRank.Insert(Rs)
				AddRow = True
			Catch e As Exception
				If EW_DEBUG_ENABLED Then Throw
				Message = e.Message				
				AddRow = False
			End Try
		Else
			If LinkRank.CancelMessage <> "" Then
				Message = LinkRank.CancelMessage
				LinkRank.CancelMessage = ""
			Else
				Message = "Insert cancelled"
			End If
			AddRow = False
		End If
		If AddRow Then
			LastInsertId = Conn.GetLastInsertId()
			LinkRank.ID.DbValue = LastInsertId
			Rs("ID") = LinkRank.ID.DbValue		

			' Row Inserted event
			LinkRank.Row_Inserted(Rs)
			WriteAuditTrailOnAdd(Rs)
		End If
	End Function

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "LinkRank"
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
		Dim table As String = "LinkRank"

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

		' LinkID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "LinkID", keyvalue, oldvalue, RsSrc("LinkID"))

		' UserID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "UserID", keyvalue, oldvalue, RsSrc("UserID"))

		' RankNum Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "RankNum", keyvalue, oldvalue, RsSrc("RankNum"))

		' CateID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "CateID", keyvalue, oldvalue, RsSrc("CateID"))

		' Comment Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "Comment", keyvalue, oldvalue, RsSrc("Comment"))
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
		LinkRank_add = New cLinkRank_add(Me)		
		LinkRank_add.Page_Init()

		' Page main processing
		LinkRank_add.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If LinkRank_add IsNot Nothing Then LinkRank_add.Dispose()
	End Sub
End Class
