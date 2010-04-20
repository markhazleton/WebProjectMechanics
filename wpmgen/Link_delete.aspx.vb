Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class Link_delete
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public Link_delete As cLink_delete

	'
	' Page Class
	'
	Class cLink_delete
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
				If Link.UseTokenInUrl Then Url = Url & "t=" & Link.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If Link.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (Link.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (Link.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' Link
		Public Property Link() As cLink
			Get				
				Return ParentPage.Link
			End Get
			Set(ByVal v As cLink)
				ParentPage.Link = v	
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
			m_PageObjName = "Link_delete"
			m_PageObjTypeName = "cLink_delete"

			' Table Name
			m_TableName = "Link"

			' Initialize table object
			Link = New cLink(Me)

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
			Link.Dispose()

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
		If ew_Get("ID") <> "" Then
			Link.ID.QueryStringValue = ew_Get("ID")
			If Not IsNumeric(Link.ID.QueryStringValue) Then
			Page_Terminate("Link_list.aspx") ' Prevent SQL injection, return to list
			End If
			sKey = sKey & Link.ID.QueryStringValue
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
		If nKeySelected <= 0 Then Page_Terminate("Link_list.aspx") ' No key specified, return to list

		' Build filter
		For i As Integer = 0 to arRecKeys.GetUpperBound(0)
			sKey = arRecKeys(i).Trim()
			sFilter = sFilter & "("

			' Set up key field
			sKeyFld = sKey
			If Not IsNumeric(sKeyFld) Then Page_Terminate("Link_list.aspx") ' Prevent SQL injection, return to list
			sFilter = sFilter & "[ID]=" & ew_AdjustSql(sKeyFld) & " AND "
			If sFilter.EndsWith(" AND ") Then sFilter = sFilter.Substring(0, sFilter.Length-5) & ") OR "
		Next
		If sFilter.EndsWith(" OR ") Then sFilter = sFilter.Substring(0, sFilter.Length-4)

		' Set up filter (SQL WHERE clause)
		' SQL constructor in Link class, Linkinfo.aspx

		Link.CurrentFilter = sFilter

		' Get action
		If ew_Post("a_delete") <> "" Then
			Link.CurrentAction = ew_Post("a_delete")
		Else
			Link.CurrentAction = "I"	' Display record
		End If
		Select Case Link.CurrentAction
			Case "D" ' Delete
				Link.SendEmail = True ' Send email on delete success
				If DeleteRows() Then ' delete rows
					Message = "Delete succeeded" ' Set up success message
				Page_Terminate(Link.ReturnUrl) ' Return to caller
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
		sWrkFilter = Link.CurrentFilter

		' Set up filter (SQL WHERE Clause)
		' SQL constructor in Link class, Linkinfo.aspx

		Link.CurrentFilter = sWrkFilter
		sSql = Link.SQL
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
				DeleteRows = Link.Row_Deleting(Row)
				If Not DeleteRows Then Exit For
			Next Row
		End If
		If DeleteRows Then
			sKey = ""
			For Each Row As OrderedDictionary in RsOld
				sThisKey = ""
				If sThisKey <> "" Then sThisKey = sThisKey & EW_COMPOSITE_KEY_SEPARATOR
				sThisKey = sThisKey & Convert.ToString(Row("ID"))
				Try
					Link.Delete(Row)
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
			If Link.CancelMessage <> "" Then
				Message = Link.CancelMessage
				Link.CancelMessage = ""
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
				Link.Row_Deleted(Row)
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
		Link.Recordset_Selecting(Link.CurrentFilter)

		' Load list page SQL
		Dim sSql As String = Link.ListSQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)

		' Count
		lTotalRecs = -1		
		Try			
			If sSql.StartsWith("SELECT * FROM ", StringComparison.InvariantCultureIgnoreCase) AndAlso _				
				ew_Empty(Link.SqlGroupBy) AndAlso _
				ew_Empty(Link.SqlHaving) Then
				Dim sCntSql As String = Link.SelectCountSQL

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
		Link.Recordset_Selected(Rs)
		Return Rs
	End Function

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = Link.KeyFilter

		' Row Selecting event
		Link.Row_Selecting(sFilter)

		' Load SQL based on filter
		Link.CurrentFilter = sFilter
		Dim sSql As String = Link.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				Link.Row_Selected(RsRow)
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
		Link.ID.DbValue = RsRow("ID")
		Link.Title.DbValue = RsRow("Title")
		Link.LinkTypeCD.DbValue = RsRow("LinkTypeCD")
		Link.CategoryID.DbValue = RsRow("CategoryID")
		Link.CompanyID.DbValue = RsRow("CompanyID")
		Link.SiteCategoryGroupID.DbValue = RsRow("SiteCategoryGroupID")
		Link.zPageID.DbValue = RsRow("PageID")
		Link.Views.DbValue = IIf(ew_ConvertToBool(RsRow("Views")), "1", "0")
		Link.Description.DbValue = RsRow("Description")
		Link.URL.DbValue = RsRow("URL")
		Link.Ranks.DbValue = RsRow("Ranks")
		Link.UserID.DbValue = RsRow("UserID")
		Link.ASIN.DbValue = RsRow("ASIN")
		Link.UserName.DbValue = RsRow("UserName")
		Link.DateAdd.DbValue = RsRow("DateAdd")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		Link.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' Title

		Link.Title.CellCssStyle = ""
		Link.Title.CellCssClass = ""

		' LinkTypeCD
		Link.LinkTypeCD.CellCssStyle = ""
		Link.LinkTypeCD.CellCssClass = ""

		' CategoryID
		Link.CategoryID.CellCssStyle = ""
		Link.CategoryID.CellCssClass = ""

		' SiteCategoryGroupID
		Link.SiteCategoryGroupID.CellCssStyle = ""
		Link.SiteCategoryGroupID.CellCssClass = ""

		' PageID
		Link.zPageID.CellCssStyle = ""
		Link.zPageID.CellCssClass = ""

		' Views
		Link.Views.CellCssStyle = ""
		Link.Views.CellCssClass = ""

		' Ranks
		Link.Ranks.CellCssStyle = ""
		Link.Ranks.CellCssClass = ""

		' DateAdd
		Link.DateAdd.CellCssStyle = ""
		Link.DateAdd.CellCssClass = ""

		'
		'  View  Row
		'

		If Link.RowType = EW_ROWTYPE_VIEW Then ' View row

			' Title
			Link.Title.ViewValue = Link.Title.CurrentValue
			Link.Title.CssStyle = ""
			Link.Title.CssClass = ""
			Link.Title.ViewCustomAttributes = ""

			' LinkTypeCD
			If ew_NotEmpty(Link.LinkTypeCD.CurrentValue) Then
				sSqlWrk = "SELECT [LinkTypeDesc] FROM [LinkType] WHERE [LinkTypeCD] = '" & ew_AdjustSql(Link.LinkTypeCD.CurrentValue) & "'"
				sSqlWrk = sSqlWrk & " ORDER BY [LinkTypeDesc] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Link.LinkTypeCD.ViewValue = RsWrk("LinkTypeDesc")
				Else
					Link.LinkTypeCD.ViewValue = Link.LinkTypeCD.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Link.LinkTypeCD.ViewValue = System.DBNull.Value
			End If
			Link.LinkTypeCD.CssStyle = ""
			Link.LinkTypeCD.CssClass = ""
			Link.LinkTypeCD.ViewCustomAttributes = ""

			' CategoryID
			If ew_NotEmpty(Link.CategoryID.CurrentValue) Then
				sSqlWrk = "SELECT [Title] FROM [LinkCategory] WHERE [ID] = " & ew_AdjustSql(Link.CategoryID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [Title] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Link.CategoryID.ViewValue = RsWrk("Title")
				Else
					Link.CategoryID.ViewValue = Link.CategoryID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Link.CategoryID.ViewValue = System.DBNull.Value
			End If
			Link.CategoryID.CssStyle = ""
			Link.CategoryID.CssClass = ""
			Link.CategoryID.ViewCustomAttributes = ""

			' CompanyID
			If ew_NotEmpty(Link.CompanyID.CurrentValue) Then
				sSqlWrk = "SELECT [CompanyName] FROM [Company] WHERE [CompanyID] = " & ew_AdjustSql(Link.CompanyID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Link.CompanyID.ViewValue = RsWrk("CompanyName")
				Else
					Link.CompanyID.ViewValue = Link.CompanyID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Link.CompanyID.ViewValue = System.DBNull.Value
			End If
			Link.CompanyID.CssStyle = ""
			Link.CompanyID.CssClass = ""
			Link.CompanyID.ViewCustomAttributes = ""

			' SiteCategoryGroupID
			If ew_NotEmpty(Link.SiteCategoryGroupID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteCategoryGroupNM] FROM [SiteCategoryGroup] WHERE [SiteCategoryGroupID] = " & ew_AdjustSql(Link.SiteCategoryGroupID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryGroupNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Link.SiteCategoryGroupID.ViewValue = RsWrk("SiteCategoryGroupNM")
				Else
					Link.SiteCategoryGroupID.ViewValue = Link.SiteCategoryGroupID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Link.SiteCategoryGroupID.ViewValue = System.DBNull.Value
			End If
			Link.SiteCategoryGroupID.CssStyle = ""
			Link.SiteCategoryGroupID.CssClass = ""
			Link.SiteCategoryGroupID.ViewCustomAttributes = ""

			' PageID
			If ew_NotEmpty(Link.zPageID.CurrentValue) Then
				sSqlWrk = "SELECT [PageName] FROM [Page] WHERE [PageID] = " & ew_AdjustSql(Link.zPageID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [PageName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Link.zPageID.ViewValue = RsWrk("PageName")
				Else
					Link.zPageID.ViewValue = Link.zPageID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Link.zPageID.ViewValue = System.DBNull.Value
			End If
			Link.zPageID.CssStyle = ""
			Link.zPageID.CssClass = ""
			Link.zPageID.ViewCustomAttributes = ""

			' Views
			If Convert.ToString(Link.Views.CurrentValue) = "1" Then
				Link.Views.ViewValue = "Yes"
			Else
				Link.Views.ViewValue = "No"
			End If
			Link.Views.CssStyle = ""
			Link.Views.CssClass = ""
			Link.Views.ViewCustomAttributes = ""

			' Description
			Link.Description.ViewValue = Link.Description.CurrentValue
			Link.Description.CssStyle = ""
			Link.Description.CssClass = ""
			Link.Description.ViewCustomAttributes = ""

			' URL
			Link.URL.ViewValue = Link.URL.CurrentValue
			Link.URL.CssStyle = ""
			Link.URL.CssClass = ""
			Link.URL.ViewCustomAttributes = ""

			' Ranks
			Link.Ranks.ViewValue = Link.Ranks.CurrentValue
			Link.Ranks.CssStyle = ""
			Link.Ranks.CssClass = ""
			Link.Ranks.ViewCustomAttributes = ""

			' DateAdd
			Link.DateAdd.ViewValue = Link.DateAdd.CurrentValue
			Link.DateAdd.ViewValue = ew_FormatDateTime(Link.DateAdd.ViewValue, 6)
			Link.DateAdd.CssStyle = ""
			Link.DateAdd.CssClass = ""
			Link.DateAdd.ViewCustomAttributes = ""

			' View refer script
			' Title

			Link.Title.HrefValue = ""

			' LinkTypeCD
			Link.LinkTypeCD.HrefValue = ""

			' CategoryID
			Link.CategoryID.HrefValue = ""

			' SiteCategoryGroupID
			Link.SiteCategoryGroupID.HrefValue = ""

			' PageID
			Link.zPageID.HrefValue = ""

			' Views
			Link.Views.HrefValue = ""

			' Ranks
			Link.Ranks.HrefValue = ""

			' DateAdd
			Link.DateAdd.HrefValue = ""
		End If

		' Row Rendered event
		Link.Row_Rendered()
	End Sub

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "Link"
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
		Dim table As String = "Link"
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
		sKey = sKey & RsSrc("ID")
		keyvalue = sKey

		' ID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "ID", keyvalue, RsSrc("ID"), newvalue)

		' Title Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "Title", keyvalue, RsSrc("Title"), newvalue)

		' LinkTypeCD Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "LinkTypeCD", keyvalue, RsSrc("LinkTypeCD"), newvalue)

		' CategoryID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "CategoryID", keyvalue, RsSrc("CategoryID"), newvalue)

		' CompanyID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "CompanyID", keyvalue, RsSrc("CompanyID"), newvalue)

		' SiteCategoryGroupID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteCategoryGroupID", keyvalue, RsSrc("SiteCategoryGroupID"), newvalue)

		' PageID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "PageID", keyvalue, RsSrc("PageID"), newvalue)

		' Views Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "Views", keyvalue, RsSrc("Views"), newvalue)

		' Description Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "Description", keyvalue, "<MEMO>", newvalue)

		' URL Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "URL", keyvalue, "<MEMO>", newvalue)

		' Ranks Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "Ranks", keyvalue, RsSrc("Ranks"), newvalue)

		' UserID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "UserID", keyvalue, RsSrc("UserID"), newvalue)

		' ASIN Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "ASIN", keyvalue, RsSrc("ASIN"), newvalue)

		' UserName Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "UserName", keyvalue, RsSrc("UserName"), newvalue)

		' DateAdd Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "DateAdd", keyvalue, RsSrc("DateAdd"), newvalue)
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
		Link_delete = New cLink_delete(Me)		
		Link_delete.Page_Init()

		' Page main processing
		Link_delete.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If Link_delete IsNot Nothing Then Link_delete.Dispose()
	End Sub
End Class
