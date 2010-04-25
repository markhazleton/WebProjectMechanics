Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class PageAlias_view
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public PageAlias_view As cPageAlias_view

	'
	' Page Class
	'
	Class cPageAlias_view
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
			m_PageID = "view"
			m_PageObjName = "PageAlias_view"
			m_PageObjTypeName = "cPageAlias_view"

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

	Public lDisplayRecs As Integer ' Number of display records

	Public lStartRec As Integer, lStopRec As Integer, lTotalRecs As Integer, lRecRange As Integer

	Public lRecCnt As Integer

	Public sSrchWhere As String

	'
	' Page main processing
	'
	Sub Page_Main()

		' Paging variables
		lDisplayRecs = 1
		lRecRange = EW_PAGER_RANGE

		' Load current record
		Dim bLoadCurrentRecord As Boolean = False
		Dim sReturnUrl As String = ""
		Dim bMatchRecord As Boolean = False
		If IsPageRequest Then ' Validate request
			If ew_Get("PageAliasID") <> "" Then
				PageAlias.PageAliasID.QueryStringValue = ew_Get("PageAliasID")
			Else
				bLoadCurrentRecord = True
			End If

			' Get action
			PageAlias.CurrentAction = "I" ' Display form
			Select Case PageAlias.CurrentAction
				Case "I" ' Get a record to display
					lStartRec = 1 ' Initialize start position
					Dim Rs As OleDbDataReader = LoadRecordset() ' Load records
					If lTotalRecs <= 0 Then ' No record found
						Message = "No records found" ' Set no record message
						sReturnUrl = "PageAlias_list.aspx"
					ElseIf bLoadCurrentRecord Then ' Load current record position
						SetUpStartRec() ' Set up start record position

						' Point to current record
						If lStartRec <= lTotalRecs Then
							bMatchRecord = True
							For i As Integer = 1 to lStartRec
								Rs.Read()
							Next
						End If
					Else ' Match key values
						Do While Rs.Read()
							If ew_SameStr(PageAlias.PageAliasID.CurrentValue, Rs("PageAliasID")) Then
								PageAlias.StartRecordNumber = lStartRec ' Save record position
								bMatchRecord = True
								Exit Do
							Else
								lStartRec = lStartRec + 1
							End If
						Loop
					End If
					If Not bMatchRecord Then
						Message = "No records found" ' Set no record message
						sReturnUrl = "PageAlias_list.aspx" ' No matching record, return to list
					Else
						LoadRowValues(Rs) ' Load row values
					End If
					If Rs IsNot Nothing Then
						Rs.Close()
						Rs.Dispose()
					End If
			End Select
		Else
			sReturnUrl = "PageAlias_list.aspx" ' Not page request, return to list
		End If
		If sReturnUrl <> "" Then Page_Terminate(sReturnUrl)

		' Render row
		PageAlias.RowType = EW_ROWTYPE_VIEW
		RenderRow()
	End Sub

	Public Pager As Object

	'
	' Set up Starting Record parameters
	'
	Sub SetUpStartRec()
		Dim nPageNo As Integer

		' Exit if lDisplayRecs = 0
		If lDisplayRecs = 0 Then Exit Sub
		If IsPageRequest Then ' Validate request

			' Check for a "start" parameter
			If ew_Get(EW_TABLE_START_REC) <> "" AndAlso IsNumeric(ew_Get(EW_TABLE_START_REC)) Then
				lStartRec = ew_ConvertToInt(ew_Get(EW_TABLE_START_REC))
				PageAlias.StartRecordNumber = lStartRec
			ElseIf ew_Get(EW_TABLE_PAGE_NO) <> "" AndAlso IsNumeric(ew_Get(EW_TABLE_PAGE_NO)) Then
				nPageNo = ew_ConvertToInt(ew_Get(EW_TABLE_PAGE_NO))
				lStartRec = (nPageNo-1)*lDisplayRecs+1
				If lStartRec <= 0 Then
					lStartRec = 1
				ElseIf lStartRec >= ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 Then
					lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1
				End If
				PageAlias.StartRecordNumber = lStartRec
			End If
		End If
		lStartRec = PageAlias.StartRecordNumber

		' Check if correct start record counter
		If lStartRec <= 0 Then ' Avoid invalid start record counter
			lStartRec = 1 ' Reset start record counter
			PageAlias.StartRecordNumber = lStartRec
		ElseIf lStartRec > lTotalRecs Then ' Avoid starting record > total records
			lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to last page first record
			PageAlias.StartRecordNumber = lStartRec
		ElseIf (lStartRec-1) Mod lDisplayRecs <> 0 Then
			lStartRec = ((lStartRec-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to page boundary
			PageAlias.StartRecordNumber = lStartRec
		End If
	End Sub

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
		PageAlias.Recordset_Selecting(PageAlias.CurrentFilter)

		' Load list page SQL
		Dim sSql As String = PageAlias.ListSQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)

		' Count
		lTotalRecs = -1		
		Try			
			If sSql.StartsWith("SELECT * FROM ", StringComparison.InvariantCultureIgnoreCase) AndAlso _				
				ew_Empty(PageAlias.SqlGroupBy) AndAlso _
				ew_Empty(PageAlias.SqlHaving) Then
				Dim sCntSql As String = PageAlias.SelectCountSQL

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
		PageAlias.Recordset_Selected(Rs)
		Return Rs
	End Function

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
		End If

		' Row Rendered event
		PageAlias.Row_Rendered()
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
		PageAlias_view = New cPageAlias_view(Me)		
		PageAlias_view.Page_Init()

		' Page main processing
		PageAlias_view.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If PageAlias_view IsNot Nothing Then PageAlias_view.Dispose()
	End Sub
End Class
