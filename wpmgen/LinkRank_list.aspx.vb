Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class LinkRank_list
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public LinkRank_list As cLinkRank_list

	'
	' Page Class
	'
	Class cLinkRank_list
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
			m_PageID = "list"
			m_PageObjName = "LinkRank_list"
			m_PageObjTypeName = "cLinkRank_list"

			' Table Name
			m_TableName = "LinkRank"

			' Initialize table object
			LinkRank = New cLinkRank(Me)

			' Connect to database
			Conn = New cConnection()

			' Initialize list options
			ListOptions = New cListOptions
		End Sub

		'
		'  Subroutine Page_Init
		'  - called before page main
		'  - check Security
		'  - set up response header
		'  - call page load events
		'
		Public Sub Page_Init()
			LinkRank.Export = ew_Get("export") ' Get export parameter
			ParentPage.gsExport = LinkRank.Export ' Get export parameter, used in header
			ParentPage.gsExportFile = LinkRank.TableVar ' Get export file, used in header
			If LinkRank.Export = "excel" Then
				HttpContext.Current.Response.ContentType = "application/vnd.ms-excel"
				HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" & ParentPage.gsExportFile & ".xls")
			End If

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
			ListOptions = Nothing

			' Go to URL if specified
			If url <> "" Then
				HttpContext.Current.Response.Clear()
				HttpContext.Current.Response.Redirect(url)
			End If
		End Sub

	Public lDisplayRecs As Integer ' Number of display records

	Public lStartRec As Integer, lStopRec As Integer, lTotalRecs As Integer, lRecRange As Integer

	Public sSrchWhere As String

	Public lRecCnt As Integer

	Public lEditRowCnt As Integer

	Public lRowCnt As Integer, lRowIndex As Integer

	Public lOptionCnt As Integer

	Public lRecPerRow As Integer, lColCnt As Integer

	Public sDeleteConfirmMsg As String ' Delete confirm message

	Public sDbMasterFilter As String, sDbDetailFilter As String

	Public bMasterRecordExists As Boolean

	Public ListOptions As Object

	Public sMultiSelectKey As String

	Public x_ewPriv As Integer

	'
	' Page main processing
	'
	Public Sub Page_Main()
		lDisplayRecs = 25
		lRecRange = EW_PAGER_RANGE
		lRecCnt = 0 ' Record count

		' Search filters		
		Dim sSrchAdvanced As String = "" ' Advanced search filter
		Dim sSrchBasic As String = "" ' Basic search filter
		Dim sFilter As String = ""
		sSrchWhere = "" ' Search WHERE clause		

		' Master/Detail
		sDbMasterFilter = "" ' Master filter
		sDbDetailFilter = "" ' Detail filter
		If IsPageRequest Then ' Validate request

			' Set up records per page dynamically
			SetUpDisplayRecs()

			' Handle reset command
			ResetCmd()

			' Get advanced search criteria
			LoadSearchValues()
			If ValidateSearch() Then
				sSrchAdvanced = AdvancedSearchWhere()
			Else
				Message = ParentPage.gsSearchError
			End If

			' Get basic search criteria
			If ParentPage.gsSearchError = "" Then
				sSrchBasic = BasicSearchWhere()
			End If

			' Set Up Sorting Order
			SetUpSortOrder()
		End If

		' Restore display records
		If (LinkRank.RecordsPerPage = -1 OrElse LinkRank.RecordsPerPage > 0) Then
			lDisplayRecs = LinkRank.RecordsPerPage ' Restore from Session
		Else
			lDisplayRecs = 25 ' Load default
		End If

		' Load Sorting Order
		LoadSortOrder()

		' Build search criteria
		If sSrchAdvanced <> "" Then
			If sSrchWhere <> "" Then
				sSrchWhere = "(" & sSrchWhere & ") AND (" & sSrchAdvanced & ")"
			Else
				sSrchWhere = sSrchAdvanced
			End If
		End If
		If sSrchBasic <> "" Then
			If sSrchWhere <> "" Then
				sSrchWhere = "(" & sSrchWhere & ") AND (" & sSrchBasic & ")"
			Else
				sSrchWhere = sSrchBasic
			End If
		End If

		' Recordset Searching event
		LinkRank.Recordset_Searching(sSrchWhere)

		' Save search criteria
		If sSrchWhere <> "" Then
			If sSrchBasic = "" Then ResetBasicSearchParms()
			If sSrchAdvanced = "" Then ResetAdvancedSearchParms()
			LinkRank.SearchWhere = sSrchWhere ' Save to Session
			lStartRec = 1 ' Reset start record counter
			LinkRank.StartRecordNumber = lStartRec
		Else
			RestoreSearchParms()
		End If

		' Build filter
		sFilter = ""
		If sDbDetailFilter <> "" Then
			If sFilter <> "" Then
				sFilter = "(" & sFilter & ") AND (" & sDbDetailFilter & ")"
			Else
				sFilter = sDbDetailFilter
			End If
		End If
		If sSrchWhere <> "" Then
			If sFilter <> "" Then
				sFilter = "(" & sFilter & ") AND (" & sSrchWhere & ")"
			Else
				sFilter = sSrchWhere
			End If
		End If

		' Set up filter in Session
		LinkRank.SessionWhere = sFilter
		LinkRank.CurrentFilter = ""

		' Export Data only
		If LinkRank.Export = "html" OrElse LinkRank.Export = "csv" OrElse LinkRank.Export = "word" OrElse LinkRank.Export = "excel" OrElse LinkRank.Export = "xml" Then
			ExportData()
			Page_Terminate("") ' Clean up
			ew_End() ' Terminate response
		End If
	End Sub

	'
	' Set up number of records displayed per page
	'
	Sub SetUpDisplayRecs()
		Dim sWrk As String
		sWrk = ew_Get(EW_TABLE_REC_PER_PAGE)
		If sWrk <> "" Then
			If IsNumeric(sWrk) Then
				lDisplayRecs = ew_ConvertToInt(sWrk)
			Else
				If ew_SameText(sWrk, "all") Then ' Display all records
					lDisplayRecs = -1
				Else
					lDisplayRecs = 25 ' Non-numeric, load default
				End If
			End If
			LinkRank.RecordsPerPage = lDisplayRecs ' Save to Session

			' Reset start position
			lStartRec = 1
			LinkRank.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	' Return Advanced Search WHERE based on QueryString parameters
	'
	Function AdvancedSearchWhere() As String		
		Dim sWhere As String = ""
		BuildSearchSql(sWhere, LinkRank.ID, False) ' ID
		BuildSearchSql(sWhere, LinkRank.LinkID, False) ' LinkID
		BuildSearchSql(sWhere, LinkRank.UserID, False) ' UserID
		BuildSearchSql(sWhere, LinkRank.RankNum, False) ' RankNum
		BuildSearchSql(sWhere, LinkRank.CateID, False) ' CateID
		BuildSearchSql(sWhere, LinkRank.Comment, False) ' Comment

		' Set up search parm
		If sWhere <> "" Then
			SetSearchParm(LinkRank.ID) ' ID
			SetSearchParm(LinkRank.LinkID) ' LinkID
			SetSearchParm(LinkRank.UserID) ' UserID
			SetSearchParm(LinkRank.RankNum) ' RankNum
			SetSearchParm(LinkRank.CateID) ' CateID
			SetSearchParm(LinkRank.Comment) ' Comment
		End If
		Return sWhere
	End Function

	'
	' Build search SQL
	'
	Sub BuildSearchSql(ByRef Where As String, ByRef Fld As Object, MultiValue As Boolean)
		Dim FldParm As String = Fld.FldVar.Substring(2)
		Dim FldVal As String = Fld.AdvancedSearch.SearchValue
		Dim FldOpr As String = Fld.AdvancedSearch.SearchOperator
		Dim FldCond As String = Fld.AdvancedSearch.SearchCondition
		Dim FldVal2 As String = Fld.AdvancedSearch.SearchValue2
		Dim FldOpr2 As String = Fld.AdvancedSearch.SearchOperator2
		Dim sWrk As String = ""
		FldOpr = FldOpr.Trim().ToUpper()
		If (FldOpr = "") Then FldOpr = "="
		FldOpr2 = FldOpr2.Trim().ToUpper()
		If FldOpr2 = "" Then FldOpr2 = "="
		If EW_SEARCH_MULTI_VALUE_OPTION = 1 Then MultiValue = False
		If FldOpr <> "LIKE" Then MultiValue = False
		If FldOpr2 <> "LIKE" AndAlso FldVal2 <> "" Then MultiValue = False
		If MultiValue Then
			Dim sWrk1 As String, sWrk2 As String

			' Field value 1
			If FldVal <> "" Then
				sWrk1 = ew_GetMultiSearchSql(Fld, FldVal)
			Else
				sWrk1 = ""
			End If

			' Field value 2
			If FldVal2 <> "" AndAlso FldCond <> "" Then
				sWrk2 = ew_GetMultiSearchSql(Fld, FldVal2)
			Else
				sWrk2 = ""
			End If

			' Build final SQL
			sWrk = sWrk1
			If sWrk2 <> "" Then
				If sWrk <> "" Then
					sWrk = "(" & sWrk & ") " & FldCond & " (" & sWrk2 & ")"
				Else
					sWrk = sWrk2
				End If
			End If
		Else
			FldVal = ConvertSearchValue(Fld, FldVal)
			FldVal2 = ConvertSearchValue(Fld, FldVal2)
			sWrk = ew_GetSearchSql(Fld, FldVal, FldOpr, FldCond, FldVal2, FldOpr2)
		End If
		If sWrk <> "" Then
			If Where <> "" Then Where = Where & " AND "
			Where = Where & "(" & sWrk & ")"
		End If
	End Sub

	'
	' Set search parm
	'
	Sub SetSearchParm(ByRef Fld As Object)
		Dim FldParm As String = Fld.FldVar.Substring(2)
		LinkRank.SetAdvancedSearch("x_" & FldParm, Fld.AdvancedSearch.SearchValue)
		LinkRank.SetAdvancedSearch("z_" & FldParm, Fld.AdvancedSearch.SearchOperator)
		LinkRank.SetAdvancedSearch("v_" & FldParm, Fld.AdvancedSearch.SearchCondition)
		LinkRank.SetAdvancedSearch("y_" & FldParm, Fld.AdvancedSearch.SearchValue2)
		LinkRank.SetAdvancedSearch("w_" & FldParm, Fld.AdvancedSearch.SearchOperator2)
	End Sub

	'
	' Convert search value
	'
	Function ConvertSearchValue(ByRef Fld As cField, FldVal As Object) As Object
		If Fld.FldDataType = EW_DATATYPE_BOOLEAN Then
			If ew_NotEmpty(FldVal) Then Return IIf(FldVal="1", "True", "False")
		ElseIf Fld.FldDataType = EW_DATATYPE_DATE Then
			If ew_NotEmpty(FldVal) Then Return ew_UnFormatDateTime(FldVal, Fld.FldDateTimeFormat)
		End If
		Return FldVal
	End Function

	'
	' Return Basic Search SQL
	'
	Function BasicSearchSQL(Keyword As String) As String
		Dim sKeyword As String, sSql As String = ""
		sKeyword = ew_AdjustSql(Keyword)		
		sSql = sSql & "[Comment] LIKE '%" & sKeyword & "%' OR "
		If sSql.EndsWith(" OR ") Then sSql = sSql.SubString(0, sSql.Length-4)
		Return sSql
	End Function

	'
	' Return Basic Search WHERE based on search keyword and type
	'
	Function BasicSearchWhere() As String
		Dim sSearchStr As String = "", sSearchKeyword As String, sSearchType As String
		Dim sSearch As String, arKeyword() As String, sKeyword As String
		sSearchKeyword = ew_Get(EW_TABLE_BASIC_SEARCH)
		sSearchType = ew_Get(EW_TABLE_BASIC_SEARCH_TYPE)
		If sSearchKeyword <> "" Then
			sSearch = sSearchKeyword.Trim()
			If sSearchType <> "" Then
				While InStr(sSearch, "  ") > 0
					sSearch = sSearch.Replace("  ", " ")
				End While
				arKeyword = sSearch.Trim().Split(New Char() {" "c})
				For Each sKeyword In arKeyword
					If sSearchStr <> "" Then sSearchStr = sSearchStr & " " & sSearchType & " "
					sSearchStr = sSearchStr & "(" & BasicSearchSQL(sKeyword) & ")"
				Next
			Else
				sSearchStr = BasicSearchSQL(sSearch)
			End If
		End If
		If sSearchKeyword <> "" then
			LinkRank.BasicSearchKeyword = sSearchKeyword
			LinkRank.BasicSearchType = sSearchType
		End If
		Return sSearchStr
	End Function

	'
	' Clear all search parameters
	'
	Sub ResetSearchParms()

		' Clear search where
		sSrchWhere = ""
		LinkRank.SearchWhere = sSrchWhere

		' Clear basic search parameters
		ResetBasicSearchParms()

		' Clear advanced search parameters
		ResetAdvancedSearchParms()
	End Sub

	'
	' Clear all basic search parameters
	'
	Sub ResetBasicSearchParms()

		' Clear basic search parameters
		LinkRank.BasicSearchKeyword = ""
		LinkRank.BasicSearchType = ""
	End Sub

	'
	' Clear all advanced search parameters
	'
	Sub ResetAdvancedSearchParms()
		LinkRank.SetAdvancedSearch("x_ID", "")
		LinkRank.SetAdvancedSearch("x_LinkID", "")
		LinkRank.SetAdvancedSearch("x_UserID", "")
		LinkRank.SetAdvancedSearch("x_RankNum", "")
		LinkRank.SetAdvancedSearch("x_CateID", "")
		LinkRank.SetAdvancedSearch("x_Comment", "")
	End Sub

	'
	' Restore all search parameters
	'
	Sub RestoreSearchParms()
		sSrchWhere = LinkRank.SearchWhere

		' Restore advanced search settings
		If ParentPage.gsSearchError = "" Then
			RestoreAdvancedSearchParms()
		End If
	End Sub

	'
	' Restore all advanced search parameters
	'
	Sub RestoreAdvancedSearchParms()
		LinkRank.ID.AdvancedSearch.SearchValue = LinkRank.GetAdvancedSearch("x_ID")
		LinkRank.LinkID.AdvancedSearch.SearchValue = LinkRank.GetAdvancedSearch("x_LinkID")
		LinkRank.UserID.AdvancedSearch.SearchValue = LinkRank.GetAdvancedSearch("x_UserID")
		LinkRank.RankNum.AdvancedSearch.SearchValue = LinkRank.GetAdvancedSearch("x_RankNum")
		LinkRank.CateID.AdvancedSearch.SearchValue = LinkRank.GetAdvancedSearch("x_CateID")
		LinkRank.Comment.AdvancedSearch.SearchValue = LinkRank.GetAdvancedSearch("x_Comment")
	End Sub

	'
	' Set up Sort parameters based on Sort Links clicked
	'
	Sub SetUpSortOrder()
		Dim sOrderBy As String
		Dim sSortField As String, sLastSort As String, sThisSort As String
		Dim bCtrl As Boolean

		' Check for an Order parameter
		If ew_Get("order") <> "" Then
			LinkRank.CurrentOrder = ew_Get("order")
			LinkRank.CurrentOrderType = ew_Get("ordertype")
			LinkRank.UpdateSort(LinkRank.ID) ' ID
			LinkRank.UpdateSort(LinkRank.LinkID) ' LinkID
			LinkRank.UpdateSort(LinkRank.UserID) ' UserID
			LinkRank.UpdateSort(LinkRank.RankNum) ' RankNum
			LinkRank.UpdateSort(LinkRank.CateID) ' CateID
			LinkRank.UpdateSort(LinkRank.Comment) ' Comment
			LinkRank.StartRecordNumber = 1 ' Reset start position
		End If
	End Sub

	'
	' Load Sort Order parameters
	'
	Sub LoadSortOrder()
		Dim sOrderBy As String
		sOrderBy = LinkRank.SessionOrderBy ' Get order by from Session
		If sOrderBy = "" Then
			If LinkRank.SqlOrderBy <> "" Then
				sOrderBy = LinkRank.SqlOrderBy
				LinkRank.SessionOrderBy = sOrderBy
			End If
		End If
	End Sub

	'
	' Reset command based on querystring parameter "cmd"
	' - reset: reset search parameters
	' - resetall: reset search and master/detail parameters
	' - resetsort: reset sort parameters
	'
	Sub ResetCmd()
		Dim sCmd As String

		' Get reset cmd
		If ew_Get("cmd") <> "" Then
			sCmd = ew_Get("cmd")

			' Reset search criteria
			If ew_SameText(sCmd, "reset") OrElse ew_SameText(sCmd, "resetall") Then
				ResetSearchParms()
			End If

			' Reset sort criteria
			If ew_SameText(sCmd, "resetsort") Then
				Dim sOrderBy As String = ""
				LinkRank.SessionOrderBy = sOrderBy
				LinkRank.ID.Sort = ""
				LinkRank.LinkID.Sort = ""
				LinkRank.UserID.Sort = ""
				LinkRank.RankNum.Sort = ""
				LinkRank.CateID.Sort = ""
				LinkRank.Comment.Sort = ""
			End If

			' Reset start position
			lStartRec = 1
			LinkRank.StartRecordNumber = lStartRec
		End If
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
				LinkRank.StartRecordNumber = lStartRec
			ElseIf ew_Get(EW_TABLE_PAGE_NO) <> "" AndAlso IsNumeric(ew_Get(EW_TABLE_PAGE_NO)) Then
				nPageNo = ew_ConvertToInt(ew_Get(EW_TABLE_PAGE_NO))
				lStartRec = (nPageNo-1)*lDisplayRecs+1
				If lStartRec <= 0 Then
					lStartRec = 1
				ElseIf lStartRec >= ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 Then
					lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1
				End If
				LinkRank.StartRecordNumber = lStartRec
			End If
		End If
		lStartRec = LinkRank.StartRecordNumber

		' Check if correct start record counter
		If lStartRec <= 0 Then ' Avoid invalid start record counter
			lStartRec = 1 ' Reset start record counter
			LinkRank.StartRecordNumber = lStartRec
		ElseIf lStartRec > lTotalRecs Then ' Avoid starting record > total records
			lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to last page first record
			LinkRank.StartRecordNumber = lStartRec
		ElseIf (lStartRec-1) Mod lDisplayRecs <> 0 Then
			lStartRec = ((lStartRec-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to page boundary
			LinkRank.StartRecordNumber = lStartRec
		End If
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
	'  Load search values for validation
	'
	Sub LoadSearchValues()
		LinkRank.ID.AdvancedSearch.SearchValue = ew_Get("x_ID")
    	LinkRank.ID.AdvancedSearch.SearchOperator = ew_Get("z_ID")
		LinkRank.LinkID.AdvancedSearch.SearchValue = ew_Get("x_LinkID")
    	LinkRank.LinkID.AdvancedSearch.SearchOperator = ew_Get("z_LinkID")
		LinkRank.UserID.AdvancedSearch.SearchValue = ew_Get("x_UserID")
    	LinkRank.UserID.AdvancedSearch.SearchOperator = ew_Get("z_UserID")
		LinkRank.RankNum.AdvancedSearch.SearchValue = ew_Get("x_RankNum")
    	LinkRank.RankNum.AdvancedSearch.SearchOperator = ew_Get("z_RankNum")
		LinkRank.CateID.AdvancedSearch.SearchValue = ew_Get("x_CateID")
    	LinkRank.CateID.AdvancedSearch.SearchOperator = ew_Get("z_CateID")
		LinkRank.Comment.AdvancedSearch.SearchValue = ew_Get("x_Comment")
    	LinkRank.Comment.AdvancedSearch.SearchOperator = ew_Get("z_Comment")
	End Sub

	'
	' Load recordset
	'
	Function LoadRecordset() As OleDbDataReader

		' Recordset Selecting event
		LinkRank.Recordset_Selecting(LinkRank.CurrentFilter)

		' Load list page SQL
		Dim sSql As String = LinkRank.ListSQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)

		' Count
		lTotalRecs = -1		
		Try			
			If sSql.StartsWith("SELECT * FROM ", StringComparison.InvariantCultureIgnoreCase) AndAlso _				
				ew_Empty(LinkRank.SqlGroupBy) AndAlso _
				ew_Empty(LinkRank.SqlHaving) Then
				Dim sCntSql As String = LinkRank.SelectCountSQL

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
		LinkRank.Recordset_Selected(Rs)
		Return Rs
	End Function

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
		' ID

		LinkRank.ID.CellCssStyle = ""
		LinkRank.ID.CellCssClass = ""

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
			' ID

			LinkRank.ID.HrefValue = ""

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
		End If

		' Row Rendered event
		LinkRank.Row_Rendered()
	End Sub

	'
	' Validate search
	'
	Function ValidateSearch() As Boolean

		' Initialize
		ParentPage.gsSearchError = ""

		' Check if validation required
		If Not EW_SERVER_VALIDATE Then Return True ' Skip

		' Return validate result	
		Dim Valid As Boolean = (ParentPage.gsSearchError = "")

		' Form_CustomValidate event
		Dim sFormCustomError As String = ""
		Valid = Valid And Form_CustomValidate(sFormCustomError)
		If sFormCustomError <> "" Then
			If ParentPage.gsSearchError <> "" Then ParentPage.gsSearchError = ParentPage.gsSearchError & "<br />"
			ParentPage.gsSearchError = ParentPage.gsSearchError & sFormCustomError
		End If
		Return Valid
	End Function

	'
	' Load advanced search
	'
	Sub LoadAdvancedSearch()
		LinkRank.ID.AdvancedSearch.SearchValue = LinkRank.GetAdvancedSearch("x_ID")
		LinkRank.LinkID.AdvancedSearch.SearchValue = LinkRank.GetAdvancedSearch("x_LinkID")
		LinkRank.UserID.AdvancedSearch.SearchValue = LinkRank.GetAdvancedSearch("x_UserID")
		LinkRank.RankNum.AdvancedSearch.SearchValue = LinkRank.GetAdvancedSearch("x_RankNum")
		LinkRank.CateID.AdvancedSearch.SearchValue = LinkRank.GetAdvancedSearch("x_CateID")
		LinkRank.Comment.AdvancedSearch.SearchValue = LinkRank.GetAdvancedSearch("x_Comment")
	End Sub

	'
	' Export data in HTML/CSV/Word/Excel/XML format
	'
	Sub ExportData()
		Dim oXmlDoc As Object, oXmlTbl As Object, oXmlRec As Object, oXmlFld As Object
		Dim sExportStr As String, sExportValue As String

		' Default export style
		Dim sExportStyle As String = "h"

		' Load recordset
		Dim Rs As OleDbDataReader = LoadRecordset()
		lStartRec = 1

		' Export all
		If LinkRank.ExportAll Then
			lStopRec = lTotalRecs

		' Export one page only
		Else
			SetUpStartRec() ' Set up start record position

			' Set the last record to display
			If lDisplayRecs < 0 Then
				lStopRec = lTotalRecs
			Else
				lStopRec = lStartRec + lDisplayRecs - 1
			End If
		End If
		If LinkRank.Export = "xml" Then
			oXmlDoc = New XmlDocument()
			oXmlTbl = oXmlDoc.CreateElement("table")
			oXmlDoc.AppendChild(oXmlTbl)
		Else
			ew_Write(ew_ExportHeader(LinkRank.Export))

			' Horizontal format, write header
			If sExportStyle <> "v" OrElse LinkRank.Export = "csv" Then
				sExportStr = ""
				ew_ExportAddValue(sExportStr, "ID", LinkRank.Export)
				ew_ExportAddValue(sExportStr, "LinkID", LinkRank.Export)
				ew_ExportAddValue(sExportStr, "UserID", LinkRank.Export)
				ew_ExportAddValue(sExportStr, "RankNum", LinkRank.Export)
				ew_ExportAddValue(sExportStr, "CateID", LinkRank.Export)
				ew_ExportAddValue(sExportStr, "Comment", LinkRank.Export)
				ew_Write(ew_ExportLine(sExportStr, LinkRank.Export))
			End If
		End If

		' Move to first record
		lRecCnt = lStartRec - 1
		For i As Integer = 1 to lStartRec - 1
			Rs.Read()
		Next
		Do While Rs.Read() AndAlso lRecCnt < lStopRec
			lRecCnt = lRecCnt + 1
			If lRecCnt >= lStartRec Then
				LoadRowValues(Rs)

				' Render row for display
				LinkRank.RowType = EW_ROWTYPE_VIEW ' Render view
				RenderRow()
				If LinkRank.Export = "xml" Then
					oXmlRec = oXmlDoc.CreateElement("record")
					oXmlTbl.AppendChild(oXmlRec)
					oXmlFld = oXmlDoc.CreateElement("ID") ' ID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(LinkRank.ID.ExportValue(LinkRank.Export, LinkRank.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("LinkID") ' LinkID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(LinkRank.LinkID.ExportValue(LinkRank.Export, LinkRank.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("UserID") ' UserID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(LinkRank.UserID.ExportValue(LinkRank.Export, LinkRank.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("RankNum") ' RankNum
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(LinkRank.RankNum.ExportValue(LinkRank.Export, LinkRank.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("CateID") ' CateID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(LinkRank.CateID.ExportValue(LinkRank.Export, LinkRank.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("Comment") ' Comment
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(LinkRank.Comment.ExportValue(LinkRank.Export, LinkRank.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
				Else

					' Vertical format
					If sExportStyle = "v" AndAlso LinkRank.Export <> "csv" Then
						ew_Write(ew_ExportField("ID", LinkRank.ID.ExportValue(LinkRank.Export, LinkRank.ExportOriginalValue), LinkRank.Export)) ' ID
						ew_Write(ew_ExportField("LinkID", LinkRank.LinkID.ExportValue(LinkRank.Export, LinkRank.ExportOriginalValue), LinkRank.Export)) ' LinkID
						ew_Write(ew_ExportField("UserID", LinkRank.UserID.ExportValue(LinkRank.Export, LinkRank.ExportOriginalValue), LinkRank.Export)) ' UserID
						ew_Write(ew_ExportField("RankNum", LinkRank.RankNum.ExportValue(LinkRank.Export, LinkRank.ExportOriginalValue), LinkRank.Export)) ' RankNum
						ew_Write(ew_ExportField("CateID", LinkRank.CateID.ExportValue(LinkRank.Export, LinkRank.ExportOriginalValue), LinkRank.Export)) ' CateID
						ew_Write(ew_ExportField("Comment", LinkRank.Comment.ExportValue(LinkRank.Export, LinkRank.ExportOriginalValue), LinkRank.Export)) ' Comment

					' Horizontal format
					Else
						sExportStr = ""
						ew_ExportAddValue(sExportStr, LinkRank.ID.ExportValue(LinkRank.Export, LinkRank.ExportOriginalValue), LinkRank.Export) ' ID
						ew_ExportAddValue(sExportStr, LinkRank.LinkID.ExportValue(LinkRank.Export, LinkRank.ExportOriginalValue), LinkRank.Export) ' LinkID
						ew_ExportAddValue(sExportStr, LinkRank.UserID.ExportValue(LinkRank.Export, LinkRank.ExportOriginalValue), LinkRank.Export) ' UserID
						ew_ExportAddValue(sExportStr, LinkRank.RankNum.ExportValue(LinkRank.Export, LinkRank.ExportOriginalValue), LinkRank.Export) ' RankNum
						ew_ExportAddValue(sExportStr, LinkRank.CateID.ExportValue(LinkRank.Export, LinkRank.ExportOriginalValue), LinkRank.Export) ' CateID
						ew_ExportAddValue(sExportStr, LinkRank.Comment.ExportValue(LinkRank.Export, LinkRank.ExportOriginalValue), LinkRank.Export) ' Comment
						ew_Write(ew_ExportLine(sExportStr, LinkRank.Export))
					End If
				End If
			End If
		Loop

		' Close recordset
		Rs.Close()
		Rs.Dispose()
		If LinkRank.Export = "xml" Then
			ew_Write("<?xml version=""1.0"" standalone=""yes""?>" & vbcrlf)
			ew_Write(oXmlDoc.OuterXml)
		Else
			ew_Write(ew_ExportFooter(LinkRank.Export))
		End If
	End Sub

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
		LinkRank_list = New cLinkRank_list(Me)		
		LinkRank_list.Page_Init()

		' Page main processing
		LinkRank_list.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If LinkRank_list IsNot Nothing Then LinkRank_list.Dispose()
	End Sub
End Class
