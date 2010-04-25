Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class zPage_view
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public zPage_view As czPage_view

	'
	' Page Class
	'
	Class czPage_view
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
				If zPage.UseTokenInUrl Then Url = Url & "t=" & zPage.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If zPage.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (zPage.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (zPage.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' zPage
		Public Property zPage() As czPage
			Get				
				Return ParentPage.zPage
			End Get
			Set(ByVal v As czPage)
				ParentPage.zPage = v	
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
			m_PageObjName = "zPage_view"
			m_PageObjTypeName = "czPage_view"

			' Table Name
			m_TableName = "Page"

			' Initialize table object
			zPage = New czPage(Me)

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
			zPage.Dispose()

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
			If ew_Get("zPageID") <> "" Then
				zPage.zPageID.QueryStringValue = ew_Get("zPageID")
			Else
				bLoadCurrentRecord = True
			End If

			' Get action
			zPage.CurrentAction = "I" ' Display form
			Select Case zPage.CurrentAction
				Case "I" ' Get a record to display
					lStartRec = 1 ' Initialize start position
					Dim Rs As OleDbDataReader = LoadRecordset() ' Load records
					If lTotalRecs <= 0 Then ' No record found
						Message = "No records found" ' Set no record message
						sReturnUrl = "zPage_list.aspx"
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
							If ew_SameStr(zPage.zPageID.CurrentValue, Rs("PageID")) Then
								zPage.StartRecordNumber = lStartRec ' Save record position
								bMatchRecord = True
								Exit Do
							Else
								lStartRec = lStartRec + 1
							End If
						Loop
					End If
					If Not bMatchRecord Then
						Message = "No records found" ' Set no record message
						sReturnUrl = "zPage_list.aspx" ' No matching record, return to list
					Else
						LoadRowValues(Rs) ' Load row values
					End If
					If Rs IsNot Nothing Then
						Rs.Close()
						Rs.Dispose()
					End If
			End Select
		Else
			sReturnUrl = "zPage_list.aspx" ' Not page request, return to list
		End If
		If sReturnUrl <> "" Then Page_Terminate(sReturnUrl)

		' Render row
		zPage.RowType = EW_ROWTYPE_VIEW
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
				zPage.StartRecordNumber = lStartRec
			ElseIf ew_Get(EW_TABLE_PAGE_NO) <> "" AndAlso IsNumeric(ew_Get(EW_TABLE_PAGE_NO)) Then
				nPageNo = ew_ConvertToInt(ew_Get(EW_TABLE_PAGE_NO))
				lStartRec = (nPageNo-1)*lDisplayRecs+1
				If lStartRec <= 0 Then
					lStartRec = 1
				ElseIf lStartRec >= ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 Then
					lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1
				End If
				zPage.StartRecordNumber = lStartRec
			End If
		End If
		lStartRec = zPage.StartRecordNumber

		' Check if correct start record counter
		If lStartRec <= 0 Then ' Avoid invalid start record counter
			lStartRec = 1 ' Reset start record counter
			zPage.StartRecordNumber = lStartRec
		ElseIf lStartRec > lTotalRecs Then ' Avoid starting record > total records
			lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to last page first record
			zPage.StartRecordNumber = lStartRec
		ElseIf (lStartRec-1) Mod lDisplayRecs <> 0 Then
			lStartRec = ((lStartRec-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to page boundary
			zPage.StartRecordNumber = lStartRec
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
		zPage.Recordset_Selecting(zPage.CurrentFilter)

		' Load list page SQL
		Dim sSql As String = zPage.ListSQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)

		' Count
		lTotalRecs = -1		
		Try			
			If sSql.StartsWith("SELECT * FROM ", StringComparison.InvariantCultureIgnoreCase) AndAlso _				
				ew_Empty(zPage.SqlGroupBy) AndAlso _
				ew_Empty(zPage.SqlHaving) Then
				Dim sCntSql As String = zPage.SelectCountSQL

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
		zPage.Recordset_Selected(Rs)
		Return Rs
	End Function

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = zPage.KeyFilter

		' Row Selecting event
		zPage.Row_Selecting(sFilter)

		' Load SQL based on filter
		zPage.CurrentFilter = sFilter
		Dim sSql As String = zPage.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				zPage.Row_Selected(RsRow)
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
		zPage.zPageID.DbValue = RsRow("PageID")
		zPage.CompanyID.DbValue = RsRow("CompanyID")
		zPage.PageOrder.DbValue = RsRow("PageOrder")
		zPage.GroupID.DbValue = RsRow("GroupID")
		zPage.ParentPageID.DbValue = RsRow("ParentPageID")
		zPage.PageTypeID.DbValue = RsRow("PageTypeID")
		zPage.Active.DbValue = IIf(ew_ConvertToBool(RsRow("Active")), "1", "0")
		zPage.zPageName.DbValue = RsRow("PageName")
		zPage.PageTitle.DbValue = RsRow("PageTitle")
		zPage.PageDescription.DbValue = RsRow("PageDescription")
		zPage.PageKeywords.DbValue = RsRow("PageKeywords")
		zPage.ImagesPerRow.DbValue = RsRow("ImagesPerRow")
		zPage.RowsPerPage.DbValue = RsRow("RowsPerPage")
		zPage.AllowMessage.DbValue = IIf(ew_ConvertToBool(RsRow("AllowMessage")), "1", "0")
		zPage.PageFileName.DbValue = RsRow("PageFileName")
		zPage.VersionNo.DbValue = RsRow("VersionNo")
		zPage.SiteCategoryID.DbValue = RsRow("SiteCategoryID")
		zPage.SiteCategoryGroupID.DbValue = RsRow("SiteCategoryGroupID")
		zPage.ModifiedDT.DbValue = RsRow("ModifiedDT")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		zPage.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' CompanyID

		zPage.CompanyID.CellCssStyle = ""
		zPage.CompanyID.CellCssClass = ""

		' PageOrder
		zPage.PageOrder.CellCssStyle = ""
		zPage.PageOrder.CellCssClass = ""

		' GroupID
		zPage.GroupID.CellCssStyle = ""
		zPage.GroupID.CellCssClass = ""

		' ParentPageID
		zPage.ParentPageID.CellCssStyle = ""
		zPage.ParentPageID.CellCssClass = ""

		' PageTypeID
		zPage.PageTypeID.CellCssStyle = ""
		zPage.PageTypeID.CellCssClass = ""

		' Active
		zPage.Active.CellCssStyle = ""
		zPage.Active.CellCssClass = ""

		' PageName
		zPage.zPageName.CellCssStyle = ""
		zPage.zPageName.CellCssClass = ""

		' PageTitle
		zPage.PageTitle.CellCssStyle = ""
		zPage.PageTitle.CellCssClass = ""

		' PageDescription
		zPage.PageDescription.CellCssStyle = ""
		zPage.PageDescription.CellCssClass = ""

		' PageKeywords
		zPage.PageKeywords.CellCssStyle = ""
		zPage.PageKeywords.CellCssClass = ""

		' ImagesPerRow
		zPage.ImagesPerRow.CellCssStyle = ""
		zPage.ImagesPerRow.CellCssClass = ""

		' RowsPerPage
		zPage.RowsPerPage.CellCssStyle = ""
		zPage.RowsPerPage.CellCssClass = ""

		' PageFileName
		zPage.PageFileName.CellCssStyle = ""
		zPage.PageFileName.CellCssClass = ""

		' VersionNo
		zPage.VersionNo.CellCssStyle = ""
		zPage.VersionNo.CellCssClass = ""

		' SiteCategoryID
		zPage.SiteCategoryID.CellCssStyle = ""
		zPage.SiteCategoryID.CellCssClass = ""

		' SiteCategoryGroupID
		zPage.SiteCategoryGroupID.CellCssStyle = ""
		zPage.SiteCategoryGroupID.CellCssClass = ""

		' ModifiedDT
		zPage.ModifiedDT.CellCssStyle = ""
		zPage.ModifiedDT.CellCssClass = ""

		'
		'  View  Row
		'

		If zPage.RowType = EW_ROWTYPE_VIEW Then ' View row

			' CompanyID
			If ew_NotEmpty(zPage.CompanyID.CurrentValue) Then
				sSqlWrk = "SELECT [CompanyName] FROM [Company] WHERE [CompanyID] = " & ew_AdjustSql(zPage.CompanyID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					zPage.CompanyID.ViewValue = RsWrk("CompanyName")
				Else
					zPage.CompanyID.ViewValue = zPage.CompanyID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				zPage.CompanyID.ViewValue = System.DBNull.Value
			End If
			zPage.CompanyID.CssStyle = ""
			zPage.CompanyID.CssClass = ""
			zPage.CompanyID.ViewCustomAttributes = ""

			' PageOrder
			zPage.PageOrder.ViewValue = zPage.PageOrder.CurrentValue
			zPage.PageOrder.CssStyle = ""
			zPage.PageOrder.CssClass = ""
			zPage.PageOrder.ViewCustomAttributes = ""

			' GroupID
			If ew_NotEmpty(zPage.GroupID.CurrentValue) Then
				sSqlWrk = "SELECT [GroupName] FROM [Group] WHERE [GroupID] = " & ew_AdjustSql(zPage.GroupID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [GroupID] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					zPage.GroupID.ViewValue = RsWrk("GroupName")
				Else
					zPage.GroupID.ViewValue = zPage.GroupID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				zPage.GroupID.ViewValue = System.DBNull.Value
			End If
			zPage.GroupID.CssStyle = ""
			zPage.GroupID.CssClass = ""
			zPage.GroupID.ViewCustomAttributes = ""

			' ParentPageID
			If ew_NotEmpty(zPage.ParentPageID.CurrentValue) Then
				sSqlWrk = "SELECT [PageName] FROM [Page] WHERE [PageID] = " & ew_AdjustSql(zPage.ParentPageID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [PageName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					zPage.ParentPageID.ViewValue = RsWrk("PageName")
				Else
					zPage.ParentPageID.ViewValue = zPage.ParentPageID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				zPage.ParentPageID.ViewValue = System.DBNull.Value
			End If
			zPage.ParentPageID.CssStyle = ""
			zPage.ParentPageID.CssClass = ""
			zPage.ParentPageID.ViewCustomAttributes = ""

			' PageTypeID
			If ew_NotEmpty(zPage.PageTypeID.CurrentValue) Then
				sSqlWrk = "SELECT [PageTypeDesc] FROM [PageType] WHERE [PageTypeID] = " & ew_AdjustSql(zPage.PageTypeID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [PageTypeDesc] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					zPage.PageTypeID.ViewValue = RsWrk("PageTypeDesc")
				Else
					zPage.PageTypeID.ViewValue = zPage.PageTypeID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				zPage.PageTypeID.ViewValue = System.DBNull.Value
			End If
			zPage.PageTypeID.CssStyle = ""
			zPage.PageTypeID.CssClass = ""
			zPage.PageTypeID.ViewCustomAttributes = ""

			' Active
			If Convert.ToString(zPage.Active.CurrentValue) = "1" Then
				zPage.Active.ViewValue = "Yes"
			Else
				zPage.Active.ViewValue = "No"
			End If
			zPage.Active.CssStyle = ""
			zPage.Active.CssClass = ""
			zPage.Active.ViewCustomAttributes = ""

			' PageName
			zPage.zPageName.ViewValue = zPage.zPageName.CurrentValue
			zPage.zPageName.CssStyle = ""
			zPage.zPageName.CssClass = ""
			zPage.zPageName.ViewCustomAttributes = ""

			' PageTitle
			zPage.PageTitle.ViewValue = zPage.PageTitle.CurrentValue
			zPage.PageTitle.CssStyle = ""
			zPage.PageTitle.CssClass = ""
			zPage.PageTitle.ViewCustomAttributes = ""

			' PageDescription
			zPage.PageDescription.ViewValue = zPage.PageDescription.CurrentValue
			zPage.PageDescription.CssStyle = ""
			zPage.PageDescription.CssClass = ""
			zPage.PageDescription.ViewCustomAttributes = ""

			' PageKeywords
			zPage.PageKeywords.ViewValue = zPage.PageKeywords.CurrentValue
			zPage.PageKeywords.CssStyle = ""
			zPage.PageKeywords.CssClass = ""
			zPage.PageKeywords.ViewCustomAttributes = ""

			' ImagesPerRow
			zPage.ImagesPerRow.ViewValue = zPage.ImagesPerRow.CurrentValue
			zPage.ImagesPerRow.CssStyle = ""
			zPage.ImagesPerRow.CssClass = ""
			zPage.ImagesPerRow.ViewCustomAttributes = ""

			' RowsPerPage
			zPage.RowsPerPage.ViewValue = zPage.RowsPerPage.CurrentValue
			zPage.RowsPerPage.CssStyle = ""
			zPage.RowsPerPage.CssClass = ""
			zPage.RowsPerPage.ViewCustomAttributes = ""

			' AllowMessage
			If Convert.ToString(zPage.AllowMessage.CurrentValue) = "1" Then
				zPage.AllowMessage.ViewValue = "Yes"
			Else
				zPage.AllowMessage.ViewValue = "No"
			End If
			zPage.AllowMessage.CssStyle = ""
			zPage.AllowMessage.CssClass = ""
			zPage.AllowMessage.ViewCustomAttributes = ""

			' PageFileName
			zPage.PageFileName.ViewValue = zPage.PageFileName.CurrentValue
			zPage.PageFileName.CssStyle = ""
			zPage.PageFileName.CssClass = ""
			zPage.PageFileName.ViewCustomAttributes = ""

			' VersionNo
			zPage.VersionNo.ViewValue = zPage.VersionNo.CurrentValue
			zPage.VersionNo.CssStyle = ""
			zPage.VersionNo.CssClass = ""
			zPage.VersionNo.ViewCustomAttributes = ""

			' SiteCategoryID
			If ew_NotEmpty(zPage.SiteCategoryID.CurrentValue) Then
				sSqlWrk = "SELECT [CategoryName] FROM [SiteCategory] WHERE [SiteCategoryID] = " & ew_AdjustSql(zPage.SiteCategoryID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [CategoryName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					zPage.SiteCategoryID.ViewValue = RsWrk("CategoryName")
				Else
					zPage.SiteCategoryID.ViewValue = zPage.SiteCategoryID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				zPage.SiteCategoryID.ViewValue = System.DBNull.Value
			End If
			zPage.SiteCategoryID.CssStyle = ""
			zPage.SiteCategoryID.CssClass = ""
			zPage.SiteCategoryID.ViewCustomAttributes = ""

			' SiteCategoryGroupID
			If ew_NotEmpty(zPage.SiteCategoryGroupID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteCategoryGroupNM] FROM [SiteCategoryGroup] WHERE [SiteCategoryGroupID] = " & ew_AdjustSql(zPage.SiteCategoryGroupID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryGroupNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					zPage.SiteCategoryGroupID.ViewValue = RsWrk("SiteCategoryGroupNM")
				Else
					zPage.SiteCategoryGroupID.ViewValue = zPage.SiteCategoryGroupID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				zPage.SiteCategoryGroupID.ViewValue = System.DBNull.Value
			End If
			zPage.SiteCategoryGroupID.CssStyle = ""
			zPage.SiteCategoryGroupID.CssClass = ""
			zPage.SiteCategoryGroupID.ViewCustomAttributes = ""

			' ModifiedDT
			zPage.ModifiedDT.ViewValue = zPage.ModifiedDT.CurrentValue
			zPage.ModifiedDT.ViewValue = ew_FormatDateTime(zPage.ModifiedDT.ViewValue, 6)
			zPage.ModifiedDT.CssStyle = ""
			zPage.ModifiedDT.CssClass = ""
			zPage.ModifiedDT.ViewCustomAttributes = ""

			' View refer script
			' CompanyID

			zPage.CompanyID.HrefValue = ""

			' PageOrder
			zPage.PageOrder.HrefValue = ""

			' GroupID
			zPage.GroupID.HrefValue = ""

			' ParentPageID
			zPage.ParentPageID.HrefValue = ""

			' PageTypeID
			zPage.PageTypeID.HrefValue = ""

			' Active
			zPage.Active.HrefValue = ""

			' PageName
			zPage.zPageName.HrefValue = ""

			' PageTitle
			zPage.PageTitle.HrefValue = ""

			' PageDescription
			zPage.PageDescription.HrefValue = ""

			' PageKeywords
			zPage.PageKeywords.HrefValue = ""

			' ImagesPerRow
			zPage.ImagesPerRow.HrefValue = ""

			' RowsPerPage
			zPage.RowsPerPage.HrefValue = ""

			' PageFileName
			zPage.PageFileName.HrefValue = ""

			' VersionNo
			zPage.VersionNo.HrefValue = ""

			' SiteCategoryID
			zPage.SiteCategoryID.HrefValue = ""

			' SiteCategoryGroupID
			zPage.SiteCategoryGroupID.HrefValue = ""

			' ModifiedDT
			zPage.ModifiedDT.HrefValue = ""
		End If

		' Row Rendered event
		zPage.Row_Rendered()
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
		zPage_view = New czPage_view(Me)		
		zPage_view.Page_Init()

		' Page main processing
		zPage_view.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If zPage_view IsNot Nothing Then zPage_view.Dispose()
	End Sub
End Class
