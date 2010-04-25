Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class SiteLink_view
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public SiteLink_view As cSiteLink_view

	'
	' Page Class
	'
	Class cSiteLink_view
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
				If SiteLink.UseTokenInUrl Then Url = Url & "t=" & SiteLink.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If SiteLink.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (SiteLink.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (SiteLink.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' SiteLink
		Public Property SiteLink() As cSiteLink
			Get				
				Return ParentPage.SiteLink
			End Get
			Set(ByVal v As cSiteLink)
				ParentPage.SiteLink = v	
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
			m_PageObjName = "SiteLink_view"
			m_PageObjTypeName = "cSiteLink_view"

			' Table Name
			m_TableName = "SiteLink"

			' Initialize table object
			SiteLink = New cSiteLink(Me)

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
			SiteLink.Dispose()

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
			If ew_Get("ID") <> "" Then
				SiteLink.ID.QueryStringValue = ew_Get("ID")
			Else
				bLoadCurrentRecord = True
			End If

			' Get action
			SiteLink.CurrentAction = "I" ' Display form
			Select Case SiteLink.CurrentAction
				Case "I" ' Get a record to display
					lStartRec = 1 ' Initialize start position
					Dim Rs As OleDbDataReader = LoadRecordset() ' Load records
					If lTotalRecs <= 0 Then ' No record found
						Message = "No records found" ' Set no record message
						sReturnUrl = "SiteLink_list.aspx"
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
							If ew_SameStr(SiteLink.ID.CurrentValue, Rs("ID")) Then
								SiteLink.StartRecordNumber = lStartRec ' Save record position
								bMatchRecord = True
								Exit Do
							Else
								lStartRec = lStartRec + 1
							End If
						Loop
					End If
					If Not bMatchRecord Then
						Message = "No records found" ' Set no record message
						sReturnUrl = "SiteLink_list.aspx" ' No matching record, return to list
					Else
						LoadRowValues(Rs) ' Load row values
					End If
					If Rs IsNot Nothing Then
						Rs.Close()
						Rs.Dispose()
					End If
			End Select
		Else
			sReturnUrl = "SiteLink_list.aspx" ' Not page request, return to list
		End If
		If sReturnUrl <> "" Then Page_Terminate(sReturnUrl)

		' Render row
		SiteLink.RowType = EW_ROWTYPE_VIEW
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
				SiteLink.StartRecordNumber = lStartRec
			ElseIf ew_Get(EW_TABLE_PAGE_NO) <> "" AndAlso IsNumeric(ew_Get(EW_TABLE_PAGE_NO)) Then
				nPageNo = ew_ConvertToInt(ew_Get(EW_TABLE_PAGE_NO))
				lStartRec = (nPageNo-1)*lDisplayRecs+1
				If lStartRec <= 0 Then
					lStartRec = 1
				ElseIf lStartRec >= ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 Then
					lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1
				End If
				SiteLink.StartRecordNumber = lStartRec
			End If
		End If
		lStartRec = SiteLink.StartRecordNumber

		' Check if correct start record counter
		If lStartRec <= 0 Then ' Avoid invalid start record counter
			lStartRec = 1 ' Reset start record counter
			SiteLink.StartRecordNumber = lStartRec
		ElseIf lStartRec > lTotalRecs Then ' Avoid starting record > total records
			lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to last page first record
			SiteLink.StartRecordNumber = lStartRec
		ElseIf (lStartRec-1) Mod lDisplayRecs <> 0 Then
			lStartRec = ((lStartRec-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to page boundary
			SiteLink.StartRecordNumber = lStartRec
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
		SiteLink.Recordset_Selecting(SiteLink.CurrentFilter)

		' Load list page SQL
		Dim sSql As String = SiteLink.ListSQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)

		' Count
		lTotalRecs = -1		
		Try			
			If sSql.StartsWith("SELECT * FROM ", StringComparison.InvariantCultureIgnoreCase) AndAlso _				
				ew_Empty(SiteLink.SqlGroupBy) AndAlso _
				ew_Empty(SiteLink.SqlHaving) Then
				Dim sCntSql As String = SiteLink.SelectCountSQL

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
		SiteLink.Recordset_Selected(Rs)
		Return Rs
	End Function

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = SiteLink.KeyFilter

		' Row Selecting event
		SiteLink.Row_Selecting(sFilter)

		' Load SQL based on filter
		SiteLink.CurrentFilter = sFilter
		Dim sSql As String = SiteLink.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				SiteLink.Row_Selected(RsRow)
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
		SiteLink.ID.DbValue = RsRow("ID")
		SiteLink.SiteCategoryTypeID.DbValue = RsRow("SiteCategoryTypeID")
		SiteLink.Title.DbValue = RsRow("Title")
		SiteLink.LinkTypeCD.DbValue = RsRow("LinkTypeCD")
		SiteLink.CategoryID.DbValue = RsRow("CategoryID")
		SiteLink.CompanyID.DbValue = RsRow("CompanyID")
		SiteLink.SiteCategoryID.DbValue = RsRow("SiteCategoryID")
		SiteLink.SiteCategoryGroupID.DbValue = RsRow("SiteCategoryGroupID")
		SiteLink.Description.DbValue = RsRow("Description")
		SiteLink.URL.DbValue = RsRow("URL")
		SiteLink.Ranks.DbValue = RsRow("Ranks")
		SiteLink.Views.DbValue = IIf(ew_ConvertToBool(RsRow("Views")), "1", "0")
		SiteLink.DateAdd.DbValue = RsRow("DateAdd")
		SiteLink.UserName.DbValue = RsRow("UserName")
		SiteLink.UserID.DbValue = RsRow("UserID")
		SiteLink.ASIN.DbValue = RsRow("ASIN")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		SiteLink.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' SiteCategoryTypeID

		SiteLink.SiteCategoryTypeID.CellCssStyle = ""
		SiteLink.SiteCategoryTypeID.CellCssClass = ""

		' Title
		SiteLink.Title.CellCssStyle = ""
		SiteLink.Title.CellCssClass = ""

		' LinkTypeCD
		SiteLink.LinkTypeCD.CellCssStyle = ""
		SiteLink.LinkTypeCD.CellCssClass = ""

		' CategoryID
		SiteLink.CategoryID.CellCssStyle = ""
		SiteLink.CategoryID.CellCssClass = ""

		' CompanyID
		SiteLink.CompanyID.CellCssStyle = ""
		SiteLink.CompanyID.CellCssClass = ""

		' SiteCategoryID
		SiteLink.SiteCategoryID.CellCssStyle = ""
		SiteLink.SiteCategoryID.CellCssClass = ""

		' SiteCategoryGroupID
		SiteLink.SiteCategoryGroupID.CellCssStyle = ""
		SiteLink.SiteCategoryGroupID.CellCssClass = ""

		' Description
		SiteLink.Description.CellCssStyle = ""
		SiteLink.Description.CellCssClass = ""

		' URL
		SiteLink.URL.CellCssStyle = ""
		SiteLink.URL.CellCssClass = ""

		' Ranks
		SiteLink.Ranks.CellCssStyle = ""
		SiteLink.Ranks.CellCssClass = ""

		' Views
		SiteLink.Views.CellCssStyle = ""
		SiteLink.Views.CellCssClass = ""

		'
		'  View  Row
		'

		If SiteLink.RowType = EW_ROWTYPE_VIEW Then ' View row

			' SiteCategoryTypeID
			If ew_NotEmpty(SiteLink.SiteCategoryTypeID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteCategoryTypeNM] FROM [SiteCategoryType] WHERE [SiteCategoryTypeID] = " & ew_AdjustSql(SiteLink.SiteCategoryTypeID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryTypeNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					SiteLink.SiteCategoryTypeID.ViewValue = RsWrk("SiteCategoryTypeNM")
				Else
					SiteLink.SiteCategoryTypeID.ViewValue = SiteLink.SiteCategoryTypeID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				SiteLink.SiteCategoryTypeID.ViewValue = System.DBNull.Value
			End If
			SiteLink.SiteCategoryTypeID.CssStyle = ""
			SiteLink.SiteCategoryTypeID.CssClass = ""
			SiteLink.SiteCategoryTypeID.ViewCustomAttributes = ""

			' Title
			SiteLink.Title.ViewValue = SiteLink.Title.CurrentValue
			SiteLink.Title.CssStyle = ""
			SiteLink.Title.CssClass = ""
			SiteLink.Title.ViewCustomAttributes = ""

			' LinkTypeCD
			If ew_NotEmpty(SiteLink.LinkTypeCD.CurrentValue) Then
				sSqlWrk = "SELECT [LinkTypeDesc] FROM [LinkType] WHERE [LinkTypeCD] = '" & ew_AdjustSql(SiteLink.LinkTypeCD.CurrentValue) & "'"
				sSqlWrk = sSqlWrk & " ORDER BY [LinkTypeDesc] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					SiteLink.LinkTypeCD.ViewValue = RsWrk("LinkTypeDesc")
				Else
					SiteLink.LinkTypeCD.ViewValue = SiteLink.LinkTypeCD.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				SiteLink.LinkTypeCD.ViewValue = System.DBNull.Value
			End If
			SiteLink.LinkTypeCD.CssStyle = ""
			SiteLink.LinkTypeCD.CssClass = ""
			SiteLink.LinkTypeCD.ViewCustomAttributes = ""

			' CategoryID
			If ew_NotEmpty(SiteLink.CategoryID.CurrentValue) Then
				sSqlWrk = "SELECT [Title] FROM [LinkCategory] WHERE [ID] = " & ew_AdjustSql(SiteLink.CategoryID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [Title] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					SiteLink.CategoryID.ViewValue = RsWrk("Title")
				Else
					SiteLink.CategoryID.ViewValue = SiteLink.CategoryID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				SiteLink.CategoryID.ViewValue = System.DBNull.Value
			End If
			SiteLink.CategoryID.CssStyle = ""
			SiteLink.CategoryID.CssClass = ""
			SiteLink.CategoryID.ViewCustomAttributes = ""

			' CompanyID
			If ew_NotEmpty(SiteLink.CompanyID.CurrentValue) Then
				sSqlWrk = "SELECT [CompanyName] FROM [Company] WHERE [CompanyID] = " & ew_AdjustSql(SiteLink.CompanyID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					SiteLink.CompanyID.ViewValue = RsWrk("CompanyName")
				Else
					SiteLink.CompanyID.ViewValue = SiteLink.CompanyID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				SiteLink.CompanyID.ViewValue = System.DBNull.Value
			End If
			SiteLink.CompanyID.CssStyle = ""
			SiteLink.CompanyID.CssClass = ""
			SiteLink.CompanyID.ViewCustomAttributes = ""

			' SiteCategoryID
			If ew_NotEmpty(SiteLink.SiteCategoryID.CurrentValue) Then
				sSqlWrk = "SELECT [CategoryName] FROM [SiteCategory] WHERE [SiteCategoryID] = " & ew_AdjustSql(SiteLink.SiteCategoryID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [CategoryName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					SiteLink.SiteCategoryID.ViewValue = RsWrk("CategoryName")
				Else
					SiteLink.SiteCategoryID.ViewValue = SiteLink.SiteCategoryID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				SiteLink.SiteCategoryID.ViewValue = System.DBNull.Value
			End If
			SiteLink.SiteCategoryID.CssStyle = ""
			SiteLink.SiteCategoryID.CssClass = ""
			SiteLink.SiteCategoryID.ViewCustomAttributes = ""

			' SiteCategoryGroupID
			If ew_NotEmpty(SiteLink.SiteCategoryGroupID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteCategoryGroupNM] FROM [SiteCategoryGroup] WHERE [SiteCategoryGroupID] = " & ew_AdjustSql(SiteLink.SiteCategoryGroupID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryGroupOrder] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					SiteLink.SiteCategoryGroupID.ViewValue = RsWrk("SiteCategoryGroupNM")
				Else
					SiteLink.SiteCategoryGroupID.ViewValue = SiteLink.SiteCategoryGroupID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				SiteLink.SiteCategoryGroupID.ViewValue = System.DBNull.Value
			End If
			SiteLink.SiteCategoryGroupID.CssStyle = ""
			SiteLink.SiteCategoryGroupID.CssClass = ""
			SiteLink.SiteCategoryGroupID.ViewCustomAttributes = ""

			' Description
			SiteLink.Description.ViewValue = SiteLink.Description.CurrentValue
			SiteLink.Description.CssStyle = ""
			SiteLink.Description.CssClass = ""
			SiteLink.Description.ViewCustomAttributes = ""

			' URL
			SiteLink.URL.ViewValue = SiteLink.URL.CurrentValue
			SiteLink.URL.CssStyle = ""
			SiteLink.URL.CssClass = ""
			SiteLink.URL.ViewCustomAttributes = ""

			' Ranks
			SiteLink.Ranks.ViewValue = SiteLink.Ranks.CurrentValue
			SiteLink.Ranks.CssStyle = ""
			SiteLink.Ranks.CssClass = ""
			SiteLink.Ranks.ViewCustomAttributes = ""

			' Views
			If Convert.ToString(SiteLink.Views.CurrentValue) = "1" Then
				SiteLink.Views.ViewValue = "Yes"
			Else
				SiteLink.Views.ViewValue = "No"
			End If
			SiteLink.Views.CssStyle = ""
			SiteLink.Views.CssClass = ""
			SiteLink.Views.ViewCustomAttributes = ""

			' View refer script
			' SiteCategoryTypeID

			SiteLink.SiteCategoryTypeID.HrefValue = ""

			' Title
			SiteLink.Title.HrefValue = ""

			' LinkTypeCD
			SiteLink.LinkTypeCD.HrefValue = ""

			' CategoryID
			SiteLink.CategoryID.HrefValue = ""

			' CompanyID
			SiteLink.CompanyID.HrefValue = ""

			' SiteCategoryID
			SiteLink.SiteCategoryID.HrefValue = ""

			' SiteCategoryGroupID
			SiteLink.SiteCategoryGroupID.HrefValue = ""

			' Description
			SiteLink.Description.HrefValue = ""

			' URL
			SiteLink.URL.HrefValue = ""

			' Ranks
			SiteLink.Ranks.HrefValue = ""

			' Views
			SiteLink.Views.HrefValue = ""
		End If

		' Row Rendered event
		SiteLink.Row_Rendered()
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
		SiteLink_view = New cSiteLink_view(Me)		
		SiteLink_view.Page_Init()

		' Page main processing
		SiteLink_view.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If SiteLink_view IsNot Nothing Then SiteLink_view.Dispose()
	End Sub
End Class
