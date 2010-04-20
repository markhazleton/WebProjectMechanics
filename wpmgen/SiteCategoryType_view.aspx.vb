Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class SiteCategoryType_view
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public SiteCategoryType_view As cSiteCategoryType_view

	'
	' Page Class
	'
	Class cSiteCategoryType_view
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
				If SiteCategoryType.UseTokenInUrl Then Url = Url & "t=" & SiteCategoryType.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If SiteCategoryType.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (SiteCategoryType.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (SiteCategoryType.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' SiteCategoryType
		Public Property SiteCategoryType() As cSiteCategoryType
			Get				
				Return ParentPage.SiteCategoryType
			End Get
			Set(ByVal v As cSiteCategoryType)
				ParentPage.SiteCategoryType = v	
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
			m_PageObjName = "SiteCategoryType_view"
			m_PageObjTypeName = "cSiteCategoryType_view"

			' Table Name
			m_TableName = "SiteCategoryType"

			' Initialize table object
			SiteCategoryType = New cSiteCategoryType(Me)

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
			SiteCategoryType.Dispose()

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
			If ew_Get("SiteCategoryTypeID") <> "" Then
				SiteCategoryType.SiteCategoryTypeID.QueryStringValue = ew_Get("SiteCategoryTypeID")
			Else
				bLoadCurrentRecord = True
			End If

			' Get action
			SiteCategoryType.CurrentAction = "I" ' Display form
			Select Case SiteCategoryType.CurrentAction
				Case "I" ' Get a record to display
					lStartRec = 1 ' Initialize start position
					Dim Rs As OleDbDataReader = LoadRecordset() ' Load records
					If lTotalRecs <= 0 Then ' No record found
						Message = "No records found" ' Set no record message
						sReturnUrl = "SiteCategoryType_list.aspx"
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
							If ew_SameStr(SiteCategoryType.SiteCategoryTypeID.CurrentValue, Rs("SiteCategoryTypeID")) Then
								SiteCategoryType.StartRecordNumber = lStartRec ' Save record position
								bMatchRecord = True
								Exit Do
							Else
								lStartRec = lStartRec + 1
							End If
						Loop
					End If
					If Not bMatchRecord Then
						Message = "No records found" ' Set no record message
						sReturnUrl = "SiteCategoryType_list.aspx" ' No matching record, return to list
					Else
						LoadRowValues(Rs) ' Load row values
					End If
					If Rs IsNot Nothing Then
						Rs.Close()
						Rs.Dispose()
					End If
			End Select
		Else
			sReturnUrl = "SiteCategoryType_list.aspx" ' Not page request, return to list
		End If
		If sReturnUrl <> "" Then Page_Terminate(sReturnUrl)

		' Render row
		SiteCategoryType.RowType = EW_ROWTYPE_VIEW
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
				SiteCategoryType.StartRecordNumber = lStartRec
			ElseIf ew_Get(EW_TABLE_PAGE_NO) <> "" AndAlso IsNumeric(ew_Get(EW_TABLE_PAGE_NO)) Then
				nPageNo = ew_ConvertToInt(ew_Get(EW_TABLE_PAGE_NO))
				lStartRec = (nPageNo-1)*lDisplayRecs+1
				If lStartRec <= 0 Then
					lStartRec = 1
				ElseIf lStartRec >= ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 Then
					lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1
				End If
				SiteCategoryType.StartRecordNumber = lStartRec
			End If
		End If
		lStartRec = SiteCategoryType.StartRecordNumber

		' Check if correct start record counter
		If lStartRec <= 0 Then ' Avoid invalid start record counter
			lStartRec = 1 ' Reset start record counter
			SiteCategoryType.StartRecordNumber = lStartRec
		ElseIf lStartRec > lTotalRecs Then ' Avoid starting record > total records
			lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to last page first record
			SiteCategoryType.StartRecordNumber = lStartRec
		ElseIf (lStartRec-1) Mod lDisplayRecs <> 0 Then
			lStartRec = ((lStartRec-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to page boundary
			SiteCategoryType.StartRecordNumber = lStartRec
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
		SiteCategoryType.Recordset_Selecting(SiteCategoryType.CurrentFilter)

		' Load list page SQL
		Dim sSql As String = SiteCategoryType.ListSQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)

		' Count
		lTotalRecs = -1		
		Try			
			If sSql.StartsWith("SELECT * FROM ", StringComparison.InvariantCultureIgnoreCase) AndAlso _				
				ew_Empty(SiteCategoryType.SqlGroupBy) AndAlso _
				ew_Empty(SiteCategoryType.SqlHaving) Then
				Dim sCntSql As String = SiteCategoryType.SelectCountSQL

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
		SiteCategoryType.Recordset_Selected(Rs)
		Return Rs
	End Function

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = SiteCategoryType.KeyFilter

		' Row Selecting event
		SiteCategoryType.Row_Selecting(sFilter)

		' Load SQL based on filter
		SiteCategoryType.CurrentFilter = sFilter
		Dim sSql As String = SiteCategoryType.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				SiteCategoryType.Row_Selected(RsRow)
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
		SiteCategoryType.SiteCategoryTypeID.DbValue = RsRow("SiteCategoryTypeID")
		SiteCategoryType.SiteCategoryTypeNM.DbValue = RsRow("SiteCategoryTypeNM")
		SiteCategoryType.SiteCategoryTypeDS.DbValue = RsRow("SiteCategoryTypeDS")
		SiteCategoryType.SiteCategoryComment.DbValue = RsRow("SiteCategoryComment")
		SiteCategoryType.SiteCategoryFileName.DbValue = RsRow("SiteCategoryFileName")
		SiteCategoryType.SiteCategoryTransferURL.DbValue = RsRow("SiteCategoryTransferURL")
		SiteCategoryType.DefaultSiteCategoryID.DbValue = RsRow("DefaultSiteCategoryID")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		SiteCategoryType.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' SiteCategoryTypeID

		SiteCategoryType.SiteCategoryTypeID.CellCssStyle = ""
		SiteCategoryType.SiteCategoryTypeID.CellCssClass = ""

		' SiteCategoryTypeNM
		SiteCategoryType.SiteCategoryTypeNM.CellCssStyle = ""
		SiteCategoryType.SiteCategoryTypeNM.CellCssClass = ""

		' SiteCategoryTypeDS
		SiteCategoryType.SiteCategoryTypeDS.CellCssStyle = ""
		SiteCategoryType.SiteCategoryTypeDS.CellCssClass = ""

		' SiteCategoryComment
		SiteCategoryType.SiteCategoryComment.CellCssStyle = ""
		SiteCategoryType.SiteCategoryComment.CellCssClass = ""

		' SiteCategoryFileName
		SiteCategoryType.SiteCategoryFileName.CellCssStyle = ""
		SiteCategoryType.SiteCategoryFileName.CellCssClass = ""

		' SiteCategoryTransferURL
		SiteCategoryType.SiteCategoryTransferURL.CellCssStyle = ""
		SiteCategoryType.SiteCategoryTransferURL.CellCssClass = ""

		' DefaultSiteCategoryID
		SiteCategoryType.DefaultSiteCategoryID.CellCssStyle = ""
		SiteCategoryType.DefaultSiteCategoryID.CellCssClass = ""

		'
		'  View  Row
		'

		If SiteCategoryType.RowType = EW_ROWTYPE_VIEW Then ' View row

			' SiteCategoryTypeID
			SiteCategoryType.SiteCategoryTypeID.ViewValue = SiteCategoryType.SiteCategoryTypeID.CurrentValue
			SiteCategoryType.SiteCategoryTypeID.CssStyle = ""
			SiteCategoryType.SiteCategoryTypeID.CssClass = ""
			SiteCategoryType.SiteCategoryTypeID.ViewCustomAttributes = ""

			' SiteCategoryTypeNM
			SiteCategoryType.SiteCategoryTypeNM.ViewValue = SiteCategoryType.SiteCategoryTypeNM.CurrentValue
			SiteCategoryType.SiteCategoryTypeNM.CssStyle = ""
			SiteCategoryType.SiteCategoryTypeNM.CssClass = ""
			SiteCategoryType.SiteCategoryTypeNM.ViewCustomAttributes = ""

			' SiteCategoryTypeDS
			SiteCategoryType.SiteCategoryTypeDS.ViewValue = SiteCategoryType.SiteCategoryTypeDS.CurrentValue
			SiteCategoryType.SiteCategoryTypeDS.CssStyle = ""
			SiteCategoryType.SiteCategoryTypeDS.CssClass = ""
			SiteCategoryType.SiteCategoryTypeDS.ViewCustomAttributes = ""

			' SiteCategoryComment
			SiteCategoryType.SiteCategoryComment.ViewValue = SiteCategoryType.SiteCategoryComment.CurrentValue
			SiteCategoryType.SiteCategoryComment.CssStyle = ""
			SiteCategoryType.SiteCategoryComment.CssClass = ""
			SiteCategoryType.SiteCategoryComment.ViewCustomAttributes = ""

			' SiteCategoryFileName
			SiteCategoryType.SiteCategoryFileName.ViewValue = SiteCategoryType.SiteCategoryFileName.CurrentValue
			SiteCategoryType.SiteCategoryFileName.CssStyle = ""
			SiteCategoryType.SiteCategoryFileName.CssClass = ""
			SiteCategoryType.SiteCategoryFileName.ViewCustomAttributes = ""

			' SiteCategoryTransferURL
			SiteCategoryType.SiteCategoryTransferURL.ViewValue = SiteCategoryType.SiteCategoryTransferURL.CurrentValue
			SiteCategoryType.SiteCategoryTransferURL.CssStyle = ""
			SiteCategoryType.SiteCategoryTransferURL.CssClass = ""
			SiteCategoryType.SiteCategoryTransferURL.ViewCustomAttributes = ""

			' DefaultSiteCategoryID
			If ew_NotEmpty(SiteCategoryType.DefaultSiteCategoryID.CurrentValue) Then
				sSqlWrk = "SELECT [CategoryName], [SiteCategoryTypeID] FROM [SiteCategory] WHERE [SiteCategoryID] = " & ew_AdjustSql(SiteCategoryType.DefaultSiteCategoryID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryTypeID] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					SiteCategoryType.DefaultSiteCategoryID.ViewValue = RsWrk("CategoryName")
					SiteCategoryType.DefaultSiteCategoryID.ViewValue = SiteCategoryType.DefaultSiteCategoryID.ViewValue & ew_ValueSeparator(0) & RsWrk("SiteCategoryTypeID")
				Else
					SiteCategoryType.DefaultSiteCategoryID.ViewValue = SiteCategoryType.DefaultSiteCategoryID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				SiteCategoryType.DefaultSiteCategoryID.ViewValue = System.DBNull.Value
			End If
			SiteCategoryType.DefaultSiteCategoryID.CssStyle = ""
			SiteCategoryType.DefaultSiteCategoryID.CssClass = ""
			SiteCategoryType.DefaultSiteCategoryID.ViewCustomAttributes = ""

			' View refer script
			' SiteCategoryTypeID

			SiteCategoryType.SiteCategoryTypeID.HrefValue = ""

			' SiteCategoryTypeNM
			SiteCategoryType.SiteCategoryTypeNM.HrefValue = ""

			' SiteCategoryTypeDS
			SiteCategoryType.SiteCategoryTypeDS.HrefValue = ""

			' SiteCategoryComment
			SiteCategoryType.SiteCategoryComment.HrefValue = ""

			' SiteCategoryFileName
			SiteCategoryType.SiteCategoryFileName.HrefValue = ""

			' SiteCategoryTransferURL
			SiteCategoryType.SiteCategoryTransferURL.HrefValue = ""

			' DefaultSiteCategoryID
			SiteCategoryType.DefaultSiteCategoryID.HrefValue = ""
		End If

		' Row Rendered event
		SiteCategoryType.Row_Rendered()
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
		SiteCategoryType_view = New cSiteCategoryType_view(Me)		
		SiteCategoryType_view.Page_Init()

		' Page main processing
		SiteCategoryType_view.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If SiteCategoryType_view IsNot Nothing Then SiteCategoryType_view.Dispose()
	End Sub
End Class
