Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class CompanySiteTypeParameter_view
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public CompanySiteTypeParameter_view As cCompanySiteTypeParameter_view

	'
	' Page Class
	'
	Class cCompanySiteTypeParameter_view
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
				If CompanySiteTypeParameter.UseTokenInUrl Then Url = Url & "t=" & CompanySiteTypeParameter.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If CompanySiteTypeParameter.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (CompanySiteTypeParameter.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (CompanySiteTypeParameter.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' CompanySiteTypeParameter
		Public Property CompanySiteTypeParameter() As cCompanySiteTypeParameter
			Get				
				Return ParentPage.CompanySiteTypeParameter
			End Get
			Set(ByVal v As cCompanySiteTypeParameter)
				ParentPage.CompanySiteTypeParameter = v	
			End Set	
		End Property

		' CompanySiteTypeParameter
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
			m_PageID = "view"
			m_PageObjName = "CompanySiteTypeParameter_view"
			m_PageObjTypeName = "cCompanySiteTypeParameter_view"

			' Table Name
			m_TableName = "CompanySiteTypeParameter"

			' Initialize table object
			CompanySiteTypeParameter = New cCompanySiteTypeParameter(Me)
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
			CompanySiteTypeParameter.Dispose()
			SiteParameterType.Dispose()

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
			If ew_Get("CompanySiteTypeParameterID") <> "" Then
				CompanySiteTypeParameter.CompanySiteTypeParameterID.QueryStringValue = ew_Get("CompanySiteTypeParameterID")
			Else
				bLoadCurrentRecord = True
			End If

			' Get action
			CompanySiteTypeParameter.CurrentAction = "I" ' Display form
			Select Case CompanySiteTypeParameter.CurrentAction
				Case "I" ' Get a record to display
					lStartRec = 1 ' Initialize start position
					Dim Rs As OleDbDataReader = LoadRecordset() ' Load records
					If lTotalRecs <= 0 Then ' No record found
						Message = "No records found" ' Set no record message
						sReturnUrl = "CompanySiteTypeParameter_list.aspx"
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
							If ew_SameStr(CompanySiteTypeParameter.CompanySiteTypeParameterID.CurrentValue, Rs("CompanySiteTypeParameterID")) Then
								CompanySiteTypeParameter.StartRecordNumber = lStartRec ' Save record position
								bMatchRecord = True
								Exit Do
							Else
								lStartRec = lStartRec + 1
							End If
						Loop
					End If
					If Not bMatchRecord Then
						Message = "No records found" ' Set no record message
						sReturnUrl = "CompanySiteTypeParameter_list.aspx" ' No matching record, return to list
					Else
						LoadRowValues(Rs) ' Load row values
					End If
					If Rs IsNot Nothing Then
						Rs.Close()
						Rs.Dispose()
					End If
			End Select
		Else
			sReturnUrl = "CompanySiteTypeParameter_list.aspx" ' Not page request, return to list
		End If
		If sReturnUrl <> "" Then Page_Terminate(sReturnUrl)

		' Render row
		CompanySiteTypeParameter.RowType = EW_ROWTYPE_VIEW
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
				CompanySiteTypeParameter.StartRecordNumber = lStartRec
			ElseIf ew_Get(EW_TABLE_PAGE_NO) <> "" AndAlso IsNumeric(ew_Get(EW_TABLE_PAGE_NO)) Then
				nPageNo = ew_ConvertToInt(ew_Get(EW_TABLE_PAGE_NO))
				lStartRec = (nPageNo-1)*lDisplayRecs+1
				If lStartRec <= 0 Then
					lStartRec = 1
				ElseIf lStartRec >= ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 Then
					lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1
				End If
				CompanySiteTypeParameter.StartRecordNumber = lStartRec
			End If
		End If
		lStartRec = CompanySiteTypeParameter.StartRecordNumber

		' Check if correct start record counter
		If lStartRec <= 0 Then ' Avoid invalid start record counter
			lStartRec = 1 ' Reset start record counter
			CompanySiteTypeParameter.StartRecordNumber = lStartRec
		ElseIf lStartRec > lTotalRecs Then ' Avoid starting record > total records
			lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to last page first record
			CompanySiteTypeParameter.StartRecordNumber = lStartRec
		ElseIf (lStartRec-1) Mod lDisplayRecs <> 0 Then
			lStartRec = ((lStartRec-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to page boundary
			CompanySiteTypeParameter.StartRecordNumber = lStartRec
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
		CompanySiteTypeParameter.Recordset_Selecting(CompanySiteTypeParameter.CurrentFilter)

		' Load list page SQL
		Dim sSql As String = CompanySiteTypeParameter.ListSQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)

		' Count
		lTotalRecs = -1		
		Try			
			If sSql.StartsWith("SELECT * FROM ", StringComparison.InvariantCultureIgnoreCase) AndAlso _				
				ew_Empty(CompanySiteTypeParameter.SqlGroupBy) AndAlso _
				ew_Empty(CompanySiteTypeParameter.SqlHaving) Then
				Dim sCntSql As String = CompanySiteTypeParameter.SelectCountSQL

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
		CompanySiteTypeParameter.Recordset_Selected(Rs)
		Return Rs
	End Function

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = CompanySiteTypeParameter.KeyFilter

		' Row Selecting event
		CompanySiteTypeParameter.Row_Selecting(sFilter)

		' Load SQL based on filter
		CompanySiteTypeParameter.CurrentFilter = sFilter
		Dim sSql As String = CompanySiteTypeParameter.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				CompanySiteTypeParameter.Row_Selected(RsRow)
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
		CompanySiteTypeParameter.CompanySiteTypeParameterID.DbValue = RsRow("CompanySiteTypeParameterID")
		CompanySiteTypeParameter.SiteParameterTypeID.DbValue = RsRow("SiteParameterTypeID")
		CompanySiteTypeParameter.CompanyID.DbValue = RsRow("CompanyID")
		CompanySiteTypeParameter.SiteCategoryTypeID.DbValue = RsRow("SiteCategoryTypeID")
		CompanySiteTypeParameter.SiteCategoryGroupID.DbValue = RsRow("SiteCategoryGroupID")
		CompanySiteTypeParameter.SiteCategoryID.DbValue = RsRow("SiteCategoryID")
		CompanySiteTypeParameter.SortOrder.DbValue = RsRow("SortOrder")
		CompanySiteTypeParameter.ParameterValue.DbValue = RsRow("ParameterValue")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		CompanySiteTypeParameter.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' SiteParameterTypeID

		CompanySiteTypeParameter.SiteParameterTypeID.CellCssStyle = ""
		CompanySiteTypeParameter.SiteParameterTypeID.CellCssClass = ""

		' CompanyID
		CompanySiteTypeParameter.CompanyID.CellCssStyle = ""
		CompanySiteTypeParameter.CompanyID.CellCssClass = ""

		' SiteCategoryTypeID
		CompanySiteTypeParameter.SiteCategoryTypeID.CellCssStyle = ""
		CompanySiteTypeParameter.SiteCategoryTypeID.CellCssClass = ""

		' SiteCategoryGroupID
		CompanySiteTypeParameter.SiteCategoryGroupID.CellCssStyle = ""
		CompanySiteTypeParameter.SiteCategoryGroupID.CellCssClass = ""

		' SiteCategoryID
		CompanySiteTypeParameter.SiteCategoryID.CellCssStyle = ""
		CompanySiteTypeParameter.SiteCategoryID.CellCssClass = ""

		' SortOrder
		CompanySiteTypeParameter.SortOrder.CellCssStyle = ""
		CompanySiteTypeParameter.SortOrder.CellCssClass = ""

		' ParameterValue
		CompanySiteTypeParameter.ParameterValue.CellCssStyle = ""
		CompanySiteTypeParameter.ParameterValue.CellCssClass = ""

		'
		'  View  Row
		'

		If CompanySiteTypeParameter.RowType = EW_ROWTYPE_VIEW Then ' View row

			' SiteParameterTypeID
			If ew_NotEmpty(CompanySiteTypeParameter.SiteParameterTypeID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteParameterTypeNM] FROM [SiteParameterType] WHERE [SiteParameterTypeID] = " & ew_AdjustSql(CompanySiteTypeParameter.SiteParameterTypeID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteParameterTypeNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanySiteTypeParameter.SiteParameterTypeID.ViewValue = RsWrk("SiteParameterTypeNM")
				Else
					CompanySiteTypeParameter.SiteParameterTypeID.ViewValue = CompanySiteTypeParameter.SiteParameterTypeID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanySiteTypeParameter.SiteParameterTypeID.ViewValue = System.DBNull.Value
			End If
			CompanySiteTypeParameter.SiteParameterTypeID.CssStyle = ""
			CompanySiteTypeParameter.SiteParameterTypeID.CssClass = ""
			CompanySiteTypeParameter.SiteParameterTypeID.ViewCustomAttributes = ""

			' CompanyID
			If ew_NotEmpty(CompanySiteTypeParameter.CompanyID.CurrentValue) Then
				sSqlWrk = "SELECT [CompanyName] FROM [Company] WHERE [CompanyID] = " & ew_AdjustSql(CompanySiteTypeParameter.CompanyID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanySiteTypeParameter.CompanyID.ViewValue = RsWrk("CompanyName")
				Else
					CompanySiteTypeParameter.CompanyID.ViewValue = CompanySiteTypeParameter.CompanyID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanySiteTypeParameter.CompanyID.ViewValue = System.DBNull.Value
			End If
			CompanySiteTypeParameter.CompanyID.CssStyle = ""
			CompanySiteTypeParameter.CompanyID.CssClass = ""
			CompanySiteTypeParameter.CompanyID.ViewCustomAttributes = ""

			' SiteCategoryTypeID
			If ew_NotEmpty(CompanySiteTypeParameter.SiteCategoryTypeID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteCategoryTypeNM] FROM [SiteCategoryType] WHERE [SiteCategoryTypeID] = " & ew_AdjustSql(CompanySiteTypeParameter.SiteCategoryTypeID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryTypeNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanySiteTypeParameter.SiteCategoryTypeID.ViewValue = RsWrk("SiteCategoryTypeNM")
				Else
					CompanySiteTypeParameter.SiteCategoryTypeID.ViewValue = CompanySiteTypeParameter.SiteCategoryTypeID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanySiteTypeParameter.SiteCategoryTypeID.ViewValue = System.DBNull.Value
			End If
			CompanySiteTypeParameter.SiteCategoryTypeID.CssStyle = ""
			CompanySiteTypeParameter.SiteCategoryTypeID.CssClass = ""
			CompanySiteTypeParameter.SiteCategoryTypeID.ViewCustomAttributes = ""

			' SiteCategoryGroupID
			If ew_NotEmpty(CompanySiteTypeParameter.SiteCategoryGroupID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteCategoryGroupNM] FROM [SiteCategoryGroup] WHERE [SiteCategoryGroupID] = " & ew_AdjustSql(CompanySiteTypeParameter.SiteCategoryGroupID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryGroupNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanySiteTypeParameter.SiteCategoryGroupID.ViewValue = RsWrk("SiteCategoryGroupNM")
				Else
					CompanySiteTypeParameter.SiteCategoryGroupID.ViewValue = CompanySiteTypeParameter.SiteCategoryGroupID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanySiteTypeParameter.SiteCategoryGroupID.ViewValue = System.DBNull.Value
			End If
			CompanySiteTypeParameter.SiteCategoryGroupID.CssStyle = ""
			CompanySiteTypeParameter.SiteCategoryGroupID.CssClass = ""
			CompanySiteTypeParameter.SiteCategoryGroupID.ViewCustomAttributes = ""

			' SiteCategoryID
			If ew_NotEmpty(CompanySiteTypeParameter.SiteCategoryID.CurrentValue) Then
				sSqlWrk = "SELECT [CategoryName] FROM [SiteCategory] WHERE [SiteCategoryID] = " & ew_AdjustSql(CompanySiteTypeParameter.SiteCategoryID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [CategoryName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanySiteTypeParameter.SiteCategoryID.ViewValue = RsWrk("CategoryName")
				Else
					CompanySiteTypeParameter.SiteCategoryID.ViewValue = CompanySiteTypeParameter.SiteCategoryID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanySiteTypeParameter.SiteCategoryID.ViewValue = System.DBNull.Value
			End If
			CompanySiteTypeParameter.SiteCategoryID.CssStyle = ""
			CompanySiteTypeParameter.SiteCategoryID.CssClass = ""
			CompanySiteTypeParameter.SiteCategoryID.ViewCustomAttributes = ""

			' SortOrder
			CompanySiteTypeParameter.SortOrder.ViewValue = CompanySiteTypeParameter.SortOrder.CurrentValue
			CompanySiteTypeParameter.SortOrder.CssStyle = ""
			CompanySiteTypeParameter.SortOrder.CssClass = ""
			CompanySiteTypeParameter.SortOrder.ViewCustomAttributes = ""

			' ParameterValue
			CompanySiteTypeParameter.ParameterValue.ViewValue = CompanySiteTypeParameter.ParameterValue.CurrentValue
			CompanySiteTypeParameter.ParameterValue.CssStyle = ""
			CompanySiteTypeParameter.ParameterValue.CssClass = ""
			CompanySiteTypeParameter.ParameterValue.ViewCustomAttributes = ""

			' View refer script
			' SiteParameterTypeID

			CompanySiteTypeParameter.SiteParameterTypeID.HrefValue = ""

			' CompanyID
			CompanySiteTypeParameter.CompanyID.HrefValue = ""

			' SiteCategoryTypeID
			CompanySiteTypeParameter.SiteCategoryTypeID.HrefValue = ""

			' SiteCategoryGroupID
			CompanySiteTypeParameter.SiteCategoryGroupID.HrefValue = ""

			' SiteCategoryID
			CompanySiteTypeParameter.SiteCategoryID.HrefValue = ""

			' SortOrder
			CompanySiteTypeParameter.SortOrder.HrefValue = ""

			' ParameterValue
			CompanySiteTypeParameter.ParameterValue.HrefValue = ""
		End If

		' Row Rendered event
		CompanySiteTypeParameter.Row_Rendered()
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
		CompanySiteTypeParameter_view = New cCompanySiteTypeParameter_view(Me)		
		CompanySiteTypeParameter_view.Page_Init()

		' Page main processing
		CompanySiteTypeParameter_view.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If CompanySiteTypeParameter_view IsNot Nothing Then CompanySiteTypeParameter_view.Dispose()
	End Sub
End Class
