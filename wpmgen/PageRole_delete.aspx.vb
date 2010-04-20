Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class PageRole_delete
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public PageRole_delete As cPageRole_delete

	'
	' Page Class
	'
	Class cPageRole_delete
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
			m_PageID = "delete"
			m_PageObjName = "PageRole_delete"
			m_PageObjTypeName = "cPageRole_delete"

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

	Public lTotalRecs As Integer

	Public lRecCnt As Integer

	Public arRecKeys() As String

	'
	' Page main processing
	'
	Sub Page_Main()
		Dim sKey As String = ""
		Dim bSingleDelete As Boolean = True ' Initialize as single delete
		Dim nKeySelected As Integer = 0 ' Initialize selected key count
		Dim sKeyFld As String, arKeyFlds() As String
		Dim sFilter As String		

		' Load Key Parameters
		If ew_Get("PageRoleID") <> "" Then
			PageRole.PageRoleID.QueryStringValue = ew_Get("PageRoleID")
			If Not IsNumeric(PageRole.PageRoleID.QueryStringValue) Then
			Page_Terminate("PageRole_list.aspx") ' Prevent SQL injection, return to list
			End If
			sKey = sKey & PageRole.PageRoleID.QueryStringValue
		Else
			bSingleDelete = False
		End If
		If bSingleDelete Then
			nKeySelected = 1 ' Set up key selected count
			Array.Resize(arRecKeys, 1) ' Set up key
			arRecKeys(0) = sKey
		Else
			If HttpContext.Current.Request.Form("key_m") IsNot Nothing Then ' Key in form
				arRecKeys = HttpContext.Current.Request.Form.GetValues("key_m")
				nKeySelected = arRecKeys.Length				
			End If
		End If
		If nKeySelected <= 0 Then Page_Terminate("PageRole_list.aspx") ' No key specified, return to list

		' Build filter
		For i As Integer = 0 to arRecKeys.GetUpperBound(0)
			sKey = arRecKeys(i).Trim()
			sFilter = sFilter & "("

			' Set up key field
			sKeyFld = sKey
			If Not IsNumeric(sKeyFld) Then Page_Terminate("PageRole_list.aspx") ' Prevent SQL injection, return to list
			sFilter = sFilter & "[PageRoleID]=" & ew_AdjustSql(sKeyFld) & " AND "
			If sFilter.EndsWith(" AND ") Then sFilter = sFilter.Substring(0, sFilter.Length-5) & ") OR "
		Next
		If sFilter.EndsWith(" OR ") Then sFilter = sFilter.Substring(0, sFilter.Length-4)

		' Set up filter (SQL WHERE clause)
		' SQL constructor in PageRole class, PageRoleinfo.aspx

		PageRole.CurrentFilter = sFilter

		' Get action
		If ew_Post("a_delete") <> "" Then
			PageRole.CurrentAction = ew_Post("a_delete")
		Else
			PageRole.CurrentAction = "I"	' Display record
		End If
		Select Case PageRole.CurrentAction
			Case "D" ' Delete
				PageRole.SendEmail = True ' Send email on delete success
				If DeleteRows() Then ' delete rows
					Message = "Delete succeeded" ' Set up success message
				Page_Terminate(PageRole.ReturnUrl) ' Return to caller
				End If
		End Select
	End Sub

	'
	'  Function DeleteRows
	'  - Delete Records based on current filter
	'
	Function DeleteRows() As Boolean
		Dim sKey As String, sThisKey As String, sKeyFld As String
		Dim arKeyFlds() As String
		Dim RsDelete As OleDbDataReader = Nothing
		Dim sSql As String, sWrkFilter As String 
		Dim RsOld As ArrayList
		DeleteRows = True
		sWrkFilter = PageRole.CurrentFilter

		' Set up filter (SQL WHERE Clause)
		' SQL constructor in PageRole class, PageRoleinfo.aspx

		PageRole.CurrentFilter = sWrkFilter
		sSql = PageRole.SQL
		Conn.BeginTrans() ' Begin transaction
		Try
			RsDelete = Conn.GetDataReader(sSql)
			If Not RsDelete.HasRows Then
				Message = "No records found" ' No record found
				RsDelete.Close()
				RsDelete.Dispose()					
				Return False
			End If
		Catch e As Exception
			If EW_DEBUG_ENABLED Then Throw
			Message = e.Message
			Return False
		End Try

		' Clone old rows
		RsOld = Conn.GetRows(RsDelete)
		RsDelete.Close()
		RsDelete.Dispose()

		' Call Row_Deleting event
		If DeleteRows Then
			For Each Row As OrderedDictionary in RsOld
				DeleteRows = PageRole.Row_Deleting(Row)
				If Not DeleteRows Then Exit For
			Next Row
		End If
		If DeleteRows Then
			sKey = ""
			For Each Row As OrderedDictionary in RsOld
				sThisKey = ""
				If sThisKey <> "" Then sThisKey = sThisKey & EW_COMPOSITE_KEY_SEPARATOR
				sThisKey = sThisKey & Convert.ToString(Row("PageRoleID"))
				Try
					PageRole.Delete(Row)
				Catch e As Exception
					If EW_DEBUG_ENABLED Then Throw			
					Message = e.Message ' Set up error message
					DeleteRows = False
					Exit For
				End Try
				If sKey <> "" Then sKey = sKey & ", "
				sKey = sKey & sThisKey
			Next Row
		Else

			' Set up error message
			If PageRole.CancelMessage <> "" Then
				Message = PageRole.CancelMessage
				PageRole.CancelMessage = ""
			Else
				Message = "Delete cancelled"
			End If
		End If
		If DeleteRows Then

			' Commit the changes
			Conn.CommitTrans()				

			' Write audit trail	
			For Each Row As OrderedDictionary in RsOld
				WriteAuditTrailOnDelete(Row)
			Next

			' Row deleted event
			For Each Row As OrderedDictionary in RsOld
				PageRole.Row_Deleted(Row)
			Next
		Else
			Conn.RollbackTrans() ' Rollback transaction			
		End If
	End Function

	'
	' Load default values
	'
	Sub LoadDefaultValues()
	End Sub

	'
	' Load recordset
	'
	Function LoadRecordset() As OleDbDataReader

		' Recordset Selecting event
		PageRole.Recordset_Selecting(PageRole.CurrentFilter)

		' Load list page SQL
		Dim sSql As String = PageRole.ListSQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)

		' Count
		lTotalRecs = -1		
		Try			
			If sSql.StartsWith("SELECT * FROM ", StringComparison.InvariantCultureIgnoreCase) AndAlso _				
				ew_Empty(PageRole.SqlGroupBy) AndAlso _
				ew_Empty(PageRole.SqlHaving) Then
				Dim sCntSql As String = PageRole.SelectCountSQL

				' Write SQL for debug
				If EW_DEBUG_ENABLED Then ew_Write("<br>" & sCntSql)
				lTotalRecs = Conn.ExecuteScalar(sCntSql)
			End If			
		Catch
		End Try

		' Load recordset
		Dim Rs As OleDbDataReader = Conn.GetDataReader(sSql)
		If lTotalRecs < 0 AndAlso Rs.HasRows Then
			lTotalRecs = 0
			While Rs.Read()
				lTotalRecs = lTotalRecs + 1
			End While
			Rs.Close()		
			Rs = Conn.GetDataReader(sSql)
		End If

		' Recordset Selected event
		PageRole.Recordset_Selected(Rs)
		Return Rs
	End Function

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
		' PageRoleID

		PageRole.PageRoleID.CellCssStyle = ""
		PageRole.PageRoleID.CellCssClass = ""

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
			' PageRoleID

			PageRole.PageRoleID.HrefValue = ""

			' RoleID
			PageRole.RoleID.HrefValue = ""

			' PageID
			PageRole.zPageID.HrefValue = ""

			' CompanyID
			PageRole.CompanyID.HrefValue = ""
		End If

		' Row Rendered event
		PageRole.Row_Rendered()
	End Sub

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

	' Write Audit Trail (delete page)
	Sub WriteAuditTrailOnDelete(ByRef RsSrc As OrderedDictionary)
		Dim table As String = "PageRole"
		Dim filePfx As String = "log"
		Dim action As String = "D"
		Dim newvalue As Object = ""
		Dim curDate As String, curTime As String, id As String, user As String, field As String
		Dim keyvalue As Object, oldvalue As Object
		Dim dt As DateTime = Now()
		curDate = dt.ToString("yyyy/MM/dd")
		curTime = dt.ToString("HH:mm:ss")
		id = HttpContext.Current.Request.ServerVariables("SCRIPT_NAME")
    user = CurrentUserName()

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsSrc("PageRoleID")
		keyvalue = sKey

		' PageRoleID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "PageRoleID", keyvalue, RsSrc("PageRoleID"), newvalue)

		' RoleID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "RoleID", keyvalue, RsSrc("RoleID"), newvalue)

		' PageID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "PageID", keyvalue, RsSrc("PageID"), newvalue)

		' CompanyID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "CompanyID", keyvalue, RsSrc("CompanyID"), newvalue)
	End Sub

		' Page Load event
		Public Sub Page_Load()

			'HttpContext.Current.Response.Write("Page Load")
		End Sub

		' Page Unload event
		Public Sub Page_Unload()

			'HttpContext.Current.Response.Write("Page Unload")
		End Sub
	End Class

	'
	' ASP.NET Page_Load event
	'

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

		' Page init
		PageRole_delete = New cPageRole_delete(Me)		
		PageRole_delete.Page_Init()

		' Page main processing
		PageRole_delete.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If PageRole_delete IsNot Nothing Then PageRole_delete.Dispose()
	End Sub
End Class
