Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class CompanySiteParameter_view
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public CompanySiteParameter_view As cCompanySiteParameter_view

	'
	' Page Class
	'
	Class cCompanySiteParameter_view
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
				If CompanySiteParameter.UseTokenInUrl Then Url = Url & "t=" & CompanySiteParameter.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If CompanySiteParameter.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (CompanySiteParameter.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (CompanySiteParameter.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' CompanySiteParameter
		Public Property CompanySiteParameter() As cCompanySiteParameter
			Get				
				Return ParentPage.CompanySiteParameter
			End Get
			Set(ByVal v As cCompanySiteParameter)
				ParentPage.CompanySiteParameter = v	
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
			m_PageObjName = "CompanySiteParameter_view"
			m_PageObjTypeName = "cCompanySiteParameter_view"

			' Table Name
			m_TableName = "CompanySiteParameter"

			' Initialize table object
			CompanySiteParameter = New cCompanySiteParameter(Me)

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
			CompanySiteParameter.Dispose()

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
			If ew_Get("CompanySiteParameterID") <> "" Then
				CompanySiteParameter.CompanySiteParameterID.QueryStringValue = ew_Get("CompanySiteParameterID")
			Else
				bLoadCurrentRecord = True
			End If

			' Get action
			CompanySiteParameter.CurrentAction = "I" ' Display form
			Select Case CompanySiteParameter.CurrentAction
				Case "I" ' Get a record to display
					lStartRec = 1 ' Initialize start position
					Dim Rs As OleDbDataReader = LoadRecordset() ' Load records
					If lTotalRecs <= 0 Then ' No record found
						Message = "No records found" ' Set no record message
						sReturnUrl = "CompanySiteParameter_list.aspx"
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
							If ew_SameStr(CompanySiteParameter.CompanySiteParameterID.CurrentValue, Rs("CompanySiteParameterID")) Then
								CompanySiteParameter.StartRecordNumber = lStartRec ' Save record position
								bMatchRecord = True
								Exit Do
							Else
								lStartRec = lStartRec + 1
							End If
						Loop
					End If
					If Not bMatchRecord Then
						Message = "No records found" ' Set no record message
						sReturnUrl = "CompanySiteParameter_list.aspx" ' No matching record, return to list
					Else
						LoadRowValues(Rs) ' Load row values
					End If
					If Rs IsNot Nothing Then
						Rs.Close()
						Rs.Dispose()
					End If
			End Select
		Else
			sReturnUrl = "CompanySiteParameter_list.aspx" ' Not page request, return to list
		End If
		If sReturnUrl <> "" Then Page_Terminate(sReturnUrl)

		' Render row
		CompanySiteParameter.RowType = EW_ROWTYPE_VIEW
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
				CompanySiteParameter.StartRecordNumber = lStartRec
			ElseIf ew_Get(EW_TABLE_PAGE_NO) <> "" AndAlso IsNumeric(ew_Get(EW_TABLE_PAGE_NO)) Then
				nPageNo = ew_ConvertToInt(ew_Get(EW_TABLE_PAGE_NO))
				lStartRec = (nPageNo-1)*lDisplayRecs+1
				If lStartRec <= 0 Then
					lStartRec = 1
				ElseIf lStartRec >= ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 Then
					lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1
				End If
				CompanySiteParameter.StartRecordNumber = lStartRec
			End If
		End If
		lStartRec = CompanySiteParameter.StartRecordNumber

		' Check if correct start record counter
		If lStartRec <= 0 Then ' Avoid invalid start record counter
			lStartRec = 1 ' Reset start record counter
			CompanySiteParameter.StartRecordNumber = lStartRec
		ElseIf lStartRec > lTotalRecs Then ' Avoid starting record > total records
			lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to last page first record
			CompanySiteParameter.StartRecordNumber = lStartRec
		ElseIf (lStartRec-1) Mod lDisplayRecs <> 0 Then
			lStartRec = ((lStartRec-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to page boundary
			CompanySiteParameter.StartRecordNumber = lStartRec
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
		CompanySiteParameter.Recordset_Selecting(CompanySiteParameter.CurrentFilter)

		' Load list page SQL
		Dim sSql As String = CompanySiteParameter.ListSQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)

		' Count
		lTotalRecs = -1		
		Try			
			If sSql.StartsWith("SELECT * FROM ", StringComparison.InvariantCultureIgnoreCase) AndAlso _				
				ew_Empty(CompanySiteParameter.SqlGroupBy) AndAlso _
				ew_Empty(CompanySiteParameter.SqlHaving) Then
				Dim sCntSql As String = CompanySiteParameter.SelectCountSQL

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
		CompanySiteParameter.Recordset_Selected(Rs)
		Return Rs
	End Function

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = CompanySiteParameter.KeyFilter

		' Row Selecting event
		CompanySiteParameter.Row_Selecting(sFilter)

		' Load SQL based on filter
		CompanySiteParameter.CurrentFilter = sFilter
		Dim sSql As String = CompanySiteParameter.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				CompanySiteParameter.Row_Selected(RsRow)
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
		CompanySiteParameter.CompanySiteParameterID.DbValue = RsRow("CompanySiteParameterID")
		CompanySiteParameter.CompanyID.DbValue = RsRow("CompanyID")
		CompanySiteParameter.zPageID.DbValue = RsRow("PageID")
		CompanySiteParameter.SiteCategoryGroupID.DbValue = RsRow("SiteCategoryGroupID")
		CompanySiteParameter.SiteParameterTypeID.DbValue = RsRow("SiteParameterTypeID")
		CompanySiteParameter.SortOrder.DbValue = RsRow("SortOrder")
		CompanySiteParameter.ParameterValue.DbValue = RsRow("ParameterValue")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		CompanySiteParameter.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' CompanyID

		CompanySiteParameter.CompanyID.CellCssStyle = ""
		CompanySiteParameter.CompanyID.CellCssClass = ""

		' PageID
		CompanySiteParameter.zPageID.CellCssStyle = ""
		CompanySiteParameter.zPageID.CellCssClass = ""

		' SiteCategoryGroupID
		CompanySiteParameter.SiteCategoryGroupID.CellCssStyle = ""
		CompanySiteParameter.SiteCategoryGroupID.CellCssClass = ""

		' SiteParameterTypeID
		CompanySiteParameter.SiteParameterTypeID.CellCssStyle = ""
		CompanySiteParameter.SiteParameterTypeID.CellCssClass = ""

		' SortOrder
		CompanySiteParameter.SortOrder.CellCssStyle = ""
		CompanySiteParameter.SortOrder.CellCssClass = ""

		' ParameterValue
		CompanySiteParameter.ParameterValue.CellCssStyle = ""
		CompanySiteParameter.ParameterValue.CellCssClass = ""

		'
		'  View  Row
		'

		If CompanySiteParameter.RowType = EW_ROWTYPE_VIEW Then ' View row

			' CompanyID
			If ew_NotEmpty(CompanySiteParameter.CompanyID.CurrentValue) Then
				sSqlWrk = "SELECT [CompanyName] FROM [Company] WHERE [CompanyID] = " & ew_AdjustSql(CompanySiteParameter.CompanyID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanySiteParameter.CompanyID.ViewValue = RsWrk("CompanyName")
				Else
					CompanySiteParameter.CompanyID.ViewValue = CompanySiteParameter.CompanyID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanySiteParameter.CompanyID.ViewValue = System.DBNull.Value
			End If
			CompanySiteParameter.CompanyID.CssStyle = ""
			CompanySiteParameter.CompanyID.CssClass = ""
			CompanySiteParameter.CompanyID.ViewCustomAttributes = ""

			' PageID
			If ew_NotEmpty(CompanySiteParameter.zPageID.CurrentValue) Then
				sSqlWrk = "SELECT [PageName] FROM [Page] WHERE [PageID] = " & ew_AdjustSql(CompanySiteParameter.zPageID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [PageName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanySiteParameter.zPageID.ViewValue = RsWrk("PageName")
				Else
					CompanySiteParameter.zPageID.ViewValue = CompanySiteParameter.zPageID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanySiteParameter.zPageID.ViewValue = System.DBNull.Value
			End If
			CompanySiteParameter.zPageID.CssStyle = ""
			CompanySiteParameter.zPageID.CssClass = ""
			CompanySiteParameter.zPageID.ViewCustomAttributes = ""

			' SiteCategoryGroupID
			If ew_NotEmpty(CompanySiteParameter.SiteCategoryGroupID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteCategoryGroupNM] FROM [SiteCategoryGroup] WHERE [SiteCategoryGroupID] = " & ew_AdjustSql(CompanySiteParameter.SiteCategoryGroupID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryGroupNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanySiteParameter.SiteCategoryGroupID.ViewValue = RsWrk("SiteCategoryGroupNM")
				Else
					CompanySiteParameter.SiteCategoryGroupID.ViewValue = CompanySiteParameter.SiteCategoryGroupID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanySiteParameter.SiteCategoryGroupID.ViewValue = System.DBNull.Value
			End If
			CompanySiteParameter.SiteCategoryGroupID.CssStyle = ""
			CompanySiteParameter.SiteCategoryGroupID.CssClass = ""
			CompanySiteParameter.SiteCategoryGroupID.ViewCustomAttributes = ""

			' SiteParameterTypeID
			If ew_NotEmpty(CompanySiteParameter.SiteParameterTypeID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteParameterTypeNM] FROM [SiteParameterType] WHERE [SiteParameterTypeID] = " & ew_AdjustSql(CompanySiteParameter.SiteParameterTypeID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteParameterTypeNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanySiteParameter.SiteParameterTypeID.ViewValue = RsWrk("SiteParameterTypeNM")
				Else
					CompanySiteParameter.SiteParameterTypeID.ViewValue = CompanySiteParameter.SiteParameterTypeID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanySiteParameter.SiteParameterTypeID.ViewValue = System.DBNull.Value
			End If
			CompanySiteParameter.SiteParameterTypeID.CssStyle = ""
			CompanySiteParameter.SiteParameterTypeID.CssClass = ""
			CompanySiteParameter.SiteParameterTypeID.ViewCustomAttributes = ""

			' SortOrder
			CompanySiteParameter.SortOrder.ViewValue = CompanySiteParameter.SortOrder.CurrentValue
			CompanySiteParameter.SortOrder.CssStyle = ""
			CompanySiteParameter.SortOrder.CssClass = ""
			CompanySiteParameter.SortOrder.ViewCustomAttributes = ""

			' ParameterValue
			CompanySiteParameter.ParameterValue.ViewValue = CompanySiteParameter.ParameterValue.CurrentValue
			CompanySiteParameter.ParameterValue.CssStyle = ""
			CompanySiteParameter.ParameterValue.CssClass = ""
			CompanySiteParameter.ParameterValue.ViewCustomAttributes = ""

			' View refer script
			' CompanyID

			CompanySiteParameter.CompanyID.HrefValue = ""

			' PageID
			CompanySiteParameter.zPageID.HrefValue = ""

			' SiteCategoryGroupID
			CompanySiteParameter.SiteCategoryGroupID.HrefValue = ""

			' SiteParameterTypeID
			CompanySiteParameter.SiteParameterTypeID.HrefValue = ""

			' SortOrder
			CompanySiteParameter.SortOrder.HrefValue = ""

			' ParameterValue
			CompanySiteParameter.ParameterValue.HrefValue = ""
		End If

		' Row Rendered event
		CompanySiteParameter.Row_Rendered()
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
		CompanySiteParameter_view = New cCompanySiteParameter_view(Me)		
		CompanySiteParameter_view.Page_Init()

		' Page main processing
		CompanySiteParameter_view.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If CompanySiteParameter_view IsNot Nothing Then CompanySiteParameter_view.Dispose()
	End Sub
End Class
