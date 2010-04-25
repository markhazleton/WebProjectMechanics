Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class Article_list
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public Article_list As cArticle_list

	'
	' Page Class
	'
	Class cArticle_list
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
				If Article.UseTokenInUrl Then Url = Url & "t=" & Article.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If Article.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (Article.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (Article.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' Article
		Public Property Article() As cArticle
			Get				
				Return ParentPage.Article
			End Get
			Set(ByVal v As cArticle)
				ParentPage.Article = v	
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
			m_PageObjName = "Article_list"
			m_PageObjTypeName = "cArticle_list"

			' Table Name
			m_TableName = "Article"

			' Initialize table object
			Article = New cArticle(Me)

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
			Article.Export = ew_Get("export") ' Get export parameter
			ParentPage.gsExport = Article.Export ' Get export parameter, used in header
			ParentPage.gsExportFile = Article.TableVar ' Get export file, used in header
			If Article.Export = "excel" Then
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
			Article.Dispose()
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
		sDeleteConfirmMsg = "Do you want to delete the selected records?"

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
		If (Article.RecordsPerPage = -1 OrElse Article.RecordsPerPage > 0) Then
			lDisplayRecs = Article.RecordsPerPage ' Restore from Session
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
		Article.Recordset_Searching(sSrchWhere)

		' Save search criteria
		If sSrchWhere <> "" Then
			If sSrchBasic = "" Then ResetBasicSearchParms()
			If sSrchAdvanced = "" Then ResetAdvancedSearchParms()
			Article.SearchWhere = sSrchWhere ' Save to Session
			lStartRec = 1 ' Reset start record counter
			Article.StartRecordNumber = lStartRec
		Else
			RestoreSearchParms()
		End If

		' Build filter
		sFilter = "[CompanyID]=" & httpContext.Current.Session("CompanyID") & " "
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
		Article.SessionWhere = sFilter
		Article.CurrentFilter = ""

		' Export Data only
		If Article.Export = "html" OrElse Article.Export = "csv" OrElse Article.Export = "word" OrElse Article.Export = "excel" OrElse Article.Export = "xml" Then
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
			Article.RecordsPerPage = lDisplayRecs ' Save to Session

			' Reset start position
			lStartRec = 1
			Article.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	' Return Advanced Search WHERE based on QueryString parameters
	'
	Function AdvancedSearchWhere() As String		
		Dim sWhere As String = ""
		BuildSearchSql(sWhere, Article.Active, False) ' Active
		BuildSearchSql(sWhere, Article.Title, False) ' Title
		BuildSearchSql(sWhere, Article.CompanyID, False) ' CompanyID
		BuildSearchSql(sWhere, Article.zPageID, False) ' PageID
		BuildSearchSql(sWhere, Article.ContactID, False) ' ContactID
		BuildSearchSql(sWhere, Article.Description, False) ' Description
		BuildSearchSql(sWhere, Article.ModifiedDT, False) ' ModifiedDT

		' Set up search parm
		If sWhere <> "" Then
			SetSearchParm(Article.Active) ' Active
			SetSearchParm(Article.Title) ' Title
			SetSearchParm(Article.CompanyID) ' CompanyID
			SetSearchParm(Article.zPageID) ' PageID
			SetSearchParm(Article.ContactID) ' ContactID
			SetSearchParm(Article.Description) ' Description
			SetSearchParm(Article.ModifiedDT) ' ModifiedDT
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
		Article.SetAdvancedSearch("x_" & FldParm, Fld.AdvancedSearch.SearchValue)
		Article.SetAdvancedSearch("z_" & FldParm, Fld.AdvancedSearch.SearchOperator)
		Article.SetAdvancedSearch("v_" & FldParm, Fld.AdvancedSearch.SearchCondition)
		Article.SetAdvancedSearch("y_" & FldParm, Fld.AdvancedSearch.SearchValue2)
		Article.SetAdvancedSearch("w_" & FldParm, Fld.AdvancedSearch.SearchOperator2)
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
		sSql = sSql & "[Title] LIKE '%" & sKeyword & "%' OR "
		sSql = sSql & "[Description] LIKE '%" & sKeyword & "%' OR "
		sSql = sSql & "[ArticleBody] LIKE '%" & sKeyword & "%' OR "
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
			Article.BasicSearchKeyword = sSearchKeyword
			Article.BasicSearchType = sSearchType
		End If
		Return sSearchStr
	End Function

	'
	' Clear all search parameters
	'
	Sub ResetSearchParms()

		' Clear search where
		sSrchWhere = ""
		Article.SearchWhere = sSrchWhere

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
		Article.BasicSearchKeyword = ""
		Article.BasicSearchType = ""
	End Sub

	'
	' Clear all advanced search parameters
	'
	Sub ResetAdvancedSearchParms()
		Article.SetAdvancedSearch("x_Active", "")
		Article.SetAdvancedSearch("x_Title", "")
		Article.SetAdvancedSearch("x_CompanyID", "")
		Article.SetAdvancedSearch("x_zPageID", "")
		Article.SetAdvancedSearch("x_ContactID", "")
		Article.SetAdvancedSearch("x_Description", "")
		Article.SetAdvancedSearch("x_ModifiedDT", "")
	End Sub

	'
	' Restore all search parameters
	'
	Sub RestoreSearchParms()
		sSrchWhere = Article.SearchWhere

		' Restore advanced search settings
		If ParentPage.gsSearchError = "" Then
			RestoreAdvancedSearchParms()
		End If
	End Sub

	'
	' Restore all advanced search parameters
	'
	Sub RestoreAdvancedSearchParms()
		Article.Active.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_Active")
		Article.Title.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_Title")
		Article.CompanyID.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_CompanyID")
		Article.zPageID.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_zPageID")
		Article.ContactID.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_ContactID")
		Article.Description.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_Description")
		Article.ModifiedDT.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_ModifiedDT")
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
			Article.CurrentOrder = ew_Get("order")
			Article.CurrentOrderType = ew_Get("ordertype")
			Article.UpdateSort(Article.Active) ' Active
			Article.UpdateSort(Article.StartDT) ' StartDT
			Article.UpdateSort(Article.Title) ' Title
			Article.UpdateSort(Article.zPageID) ' PageID
			Article.StartRecordNumber = 1 ' Reset start position
		End If
	End Sub

	'
	' Load Sort Order parameters
	'
	Sub LoadSortOrder()
		Dim sOrderBy As String
		sOrderBy = Article.SessionOrderBy ' Get order by from Session
		If sOrderBy = "" Then
			If Article.SqlOrderBy <> "" Then
				sOrderBy = Article.SqlOrderBy
				Article.SessionOrderBy = sOrderBy
				Article.Title.Sort = "ASC"
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
				Article.SessionOrderBy = sOrderBy
				Article.Active.Sort = ""
				Article.StartDT.Sort = ""
				Article.Title.Sort = ""
				Article.zPageID.Sort = ""
			End If

			' Reset start position
			lStartRec = 1
			Article.StartRecordNumber = lStartRec
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
				Article.StartRecordNumber = lStartRec
			ElseIf ew_Get(EW_TABLE_PAGE_NO) <> "" AndAlso IsNumeric(ew_Get(EW_TABLE_PAGE_NO)) Then
				nPageNo = ew_ConvertToInt(ew_Get(EW_TABLE_PAGE_NO))
				lStartRec = (nPageNo-1)*lDisplayRecs+1
				If lStartRec <= 0 Then
					lStartRec = 1
				ElseIf lStartRec >= ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 Then
					lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1
				End If
				Article.StartRecordNumber = lStartRec
			End If
		End If
		lStartRec = Article.StartRecordNumber

		' Check if correct start record counter
		If lStartRec <= 0 Then ' Avoid invalid start record counter
			lStartRec = 1 ' Reset start record counter
			Article.StartRecordNumber = lStartRec
		ElseIf lStartRec > lTotalRecs Then ' Avoid starting record > total records
			lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to last page first record
			Article.StartRecordNumber = lStartRec
		ElseIf (lStartRec-1) Mod lDisplayRecs <> 0 Then
			lStartRec = ((lStartRec-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to page boundary
			Article.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	' Load default values
	'
	Sub LoadDefaultValues()
		Article.Active.CurrentValue = 1
		Article.StartDT.CurrentValue = ew_CurrentDate()
		Article.zPageID.CurrentValue = 0
	End Sub

	'
	'  Load search values for validation
	'
	Sub LoadSearchValues()
		Article.Active.AdvancedSearch.SearchValue = ew_Get("x_Active")
    	Article.Active.AdvancedSearch.SearchOperator = ew_Get("z_Active")
		Article.Title.AdvancedSearch.SearchValue = ew_Get("x_Title")
    	Article.Title.AdvancedSearch.SearchOperator = ew_Get("z_Title")
		Article.CompanyID.AdvancedSearch.SearchValue = ew_Get("x_CompanyID")
    	Article.CompanyID.AdvancedSearch.SearchOperator = ew_Get("z_CompanyID")
		Article.zPageID.AdvancedSearch.SearchValue = ew_Get("x_zPageID")
    	Article.zPageID.AdvancedSearch.SearchOperator = ew_Get("z_zPageID")
		Article.ContactID.AdvancedSearch.SearchValue = ew_Get("x_ContactID")
    	Article.ContactID.AdvancedSearch.SearchOperator = ew_Get("z_ContactID")
		Article.Description.AdvancedSearch.SearchValue = ew_Get("x_Description")
    	Article.Description.AdvancedSearch.SearchOperator = ew_Get("z_Description")
		Article.ModifiedDT.AdvancedSearch.SearchValue = ew_Get("x_ModifiedDT")
    	Article.ModifiedDT.AdvancedSearch.SearchOperator = ew_Get("z_ModifiedDT")
	End Sub

	'
	' Load recordset
	'
	Function LoadRecordset() As OleDbDataReader

		' Recordset Selecting event
		Article.Recordset_Selecting(Article.CurrentFilter)

		' Load list page SQL
		Dim sSql As String = Article.ListSQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)

		' Count
		lTotalRecs = -1		
		Try			
			If sSql.StartsWith("SELECT * FROM ", StringComparison.InvariantCultureIgnoreCase) AndAlso _				
				ew_Empty(Article.SqlGroupBy) AndAlso _
				ew_Empty(Article.SqlHaving) Then
				Dim sCntSql As String = Article.SelectCountSQL

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
		Article.Recordset_Selected(Rs)
		Return Rs
	End Function

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = Article.KeyFilter

		' Row Selecting event
		Article.Row_Selecting(sFilter)

		' Load SQL based on filter
		Article.CurrentFilter = sFilter
		Dim sSql As String = Article.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				Article.Row_Selected(RsRow)
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
		Article.ArticleID.DbValue = RsRow("ArticleID")
		Article.Active.DbValue = IIf(ew_ConvertToBool(RsRow("Active")), "1", "0")
		Article.StartDT.DbValue = RsRow("StartDT")
		Article.Title.DbValue = RsRow("Title")
		Article.CompanyID.DbValue = RsRow("CompanyID")
		Article.zPageID.DbValue = RsRow("PageID")
		Article.ContactID.DbValue = RsRow("ContactID")
		Article.Description.DbValue = RsRow("Description")
		Article.ArticleBody.DbValue = RsRow("ArticleBody")
		Article.ModifiedDT.DbValue = RsRow("ModifiedDT")
		Article.Counter.DbValue = RsRow("Counter")
		Article.VersionNo.DbValue = RsRow("VersionNo")
		Article.userID.DbValue = RsRow("userID")
		Article.EndDT.DbValue = RsRow("EndDT")
		Article.ExpireDT.DbValue = RsRow("ExpireDT")
		Article.Author.DbValue = RsRow("Author")
		Article.ArticleSummary.DbValue = RsRow("ArticleSummary")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		Article.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' Active

		Article.Active.CellCssStyle = "white-space: nowrap;"
		Article.Active.CellCssClass = ""

		' StartDT
		Article.StartDT.CellCssStyle = "white-space: nowrap;"
		Article.StartDT.CellCssClass = ""

		' Title
		Article.Title.CellCssStyle = "white-space: nowrap;"
		Article.Title.CellCssClass = ""

		' PageID
		Article.zPageID.CellCssStyle = "white-space: nowrap;"
		Article.zPageID.CellCssClass = ""

		'
		'  View  Row
		'

		If Article.RowType = EW_ROWTYPE_VIEW Then ' View row

			' ArticleID
			Article.ArticleID.ViewValue = Article.ArticleID.CurrentValue
			Article.ArticleID.CssStyle = ""
			Article.ArticleID.CssClass = ""
			Article.ArticleID.ViewCustomAttributes = ""

			' Active
			If Convert.ToString(Article.Active.CurrentValue) = "1" Then
				Article.Active.ViewValue = "Yes"
			Else
				Article.Active.ViewValue = "No"
			End If
			Article.Active.CssStyle = ""
			Article.Active.CssClass = ""
			Article.Active.ViewCustomAttributes = ""

			' StartDT
			Article.StartDT.ViewValue = Article.StartDT.CurrentValue
			Article.StartDT.ViewValue = ew_FormatDateTime(Article.StartDT.ViewValue, 6)
			Article.StartDT.CssStyle = ""
			Article.StartDT.CssClass = ""
			Article.StartDT.ViewCustomAttributes = ""

			' Title
			Article.Title.ViewValue = Article.Title.CurrentValue
			Article.Title.CssStyle = ""
			Article.Title.CssClass = ""
			Article.Title.ViewCustomAttributes = ""

			' CompanyID
			If ew_NotEmpty(Article.CompanyID.CurrentValue) Then
				sSqlWrk = "SELECT [CompanyName] FROM [Company] WHERE [CompanyID] = " & ew_AdjustSql(Article.CompanyID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " AND (" & "[CompanyID]=" & httpcontext.current.session("CompanyID") & " " & ")"
				sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Article.CompanyID.ViewValue = RsWrk("CompanyName")
				Else
					Article.CompanyID.ViewValue = Article.CompanyID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Article.CompanyID.ViewValue = System.DBNull.Value
			End If
			Article.CompanyID.CssStyle = ""
			Article.CompanyID.CssClass = ""
			Article.CompanyID.ViewCustomAttributes = ""

			' PageID
			If ew_NotEmpty(Article.zPageID.CurrentValue) Then
				sSqlWrk = "SELECT [PageName] FROM [Page] WHERE [PageID] = " & ew_AdjustSql(Article.zPageID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [PageName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Article.zPageID.ViewValue = RsWrk("PageName")
				Else
					Article.zPageID.ViewValue = Article.zPageID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Article.zPageID.ViewValue = System.DBNull.Value
			End If
			Article.zPageID.CssStyle = ""
			Article.zPageID.CssClass = ""
			Article.zPageID.ViewCustomAttributes = ""

			' ContactID
			If ew_NotEmpty(Article.ContactID.CurrentValue) Then
				sSqlWrk = "SELECT [PrimaryContact] FROM [Contact] WHERE [ContactID] = " & ew_AdjustSql(Article.ContactID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [PrimaryContact] "
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Article.ContactID.ViewValue = RsWrk("PrimaryContact")
				Else
					Article.ContactID.ViewValue = Article.ContactID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Article.ContactID.ViewValue = System.DBNull.Value
			End If
			Article.ContactID.CssStyle = ""
			Article.ContactID.CssClass = ""
			Article.ContactID.ViewCustomAttributes = ""

			' Description
			Article.Description.ViewValue = Article.Description.CurrentValue
			Article.Description.CssStyle = ""
			Article.Description.CssClass = ""
			Article.Description.ViewCustomAttributes = ""

			' ArticleBody
			Article.ArticleBody.ViewValue = Article.ArticleBody.CurrentValue
			Article.ArticleBody.CssStyle = ""
			Article.ArticleBody.CssClass = ""
			Article.ArticleBody.ViewCustomAttributes = ""

			' ModifiedDT
			Article.ModifiedDT.ViewValue = Article.ModifiedDT.CurrentValue
			Article.ModifiedDT.ViewValue = ew_FormatDateTime(Article.ModifiedDT.ViewValue, 6)
			Article.ModifiedDT.CssStyle = ""
			Article.ModifiedDT.CssClass = ""
			Article.ModifiedDT.ViewCustomAttributes = ""

			' Counter
			Article.Counter.ViewValue = Article.Counter.CurrentValue
			Article.Counter.CssStyle = ""
			Article.Counter.CssClass = ""
			Article.Counter.ViewCustomAttributes = ""

			' VersionNo
			Article.VersionNo.ViewValue = Article.VersionNo.CurrentValue
			Article.VersionNo.CssStyle = ""
			Article.VersionNo.CssClass = ""
			Article.VersionNo.ViewCustomAttributes = ""

			' userID
			Article.userID.ViewValue = Article.userID.CurrentValue
			Article.userID.CssStyle = ""
			Article.userID.CssClass = ""
			Article.userID.ViewCustomAttributes = ""

			' EndDT
			Article.EndDT.ViewValue = Article.EndDT.CurrentValue
			Article.EndDT.ViewValue = ew_FormatDateTime(Article.EndDT.ViewValue, 6)
			Article.EndDT.CssStyle = ""
			Article.EndDT.CssClass = ""
			Article.EndDT.ViewCustomAttributes = ""

			' ExpireDT
			Article.ExpireDT.ViewValue = Article.ExpireDT.CurrentValue
			Article.ExpireDT.ViewValue = ew_FormatDateTime(Article.ExpireDT.ViewValue, 6)
			Article.ExpireDT.CssStyle = ""
			Article.ExpireDT.CssClass = ""
			Article.ExpireDT.ViewCustomAttributes = ""

			' Author
			Article.Author.ViewValue = Article.Author.CurrentValue
			Article.Author.CssStyle = ""
			Article.Author.CssClass = ""
			Article.Author.ViewCustomAttributes = ""

			' View refer script
			' Active

			Article.Active.HrefValue = ""

			' StartDT
			Article.StartDT.HrefValue = ""

			' Title
			Article.Title.HrefValue = ""

			' PageID
			Article.zPageID.HrefValue = ""
		End If

		' Row Rendered event
		Article.Row_Rendered()
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
		Article.Active.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_Active")
		Article.Title.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_Title")
		Article.CompanyID.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_CompanyID")
		Article.zPageID.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_zPageID")
		Article.ContactID.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_ContactID")
		Article.Description.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_Description")
		Article.ModifiedDT.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_ModifiedDT")
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
		If Article.ExportAll Then
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
		If Article.Export = "xml" Then
			oXmlDoc = New XmlDocument()
			oXmlTbl = oXmlDoc.CreateElement("table")
			oXmlDoc.AppendChild(oXmlTbl)
		Else
			ew_Write(ew_ExportHeader(Article.Export))

			' Horizontal format, write header
			If sExportStyle <> "v" OrElse Article.Export = "csv" Then
				sExportStr = ""
				ew_ExportAddValue(sExportStr, "ArticleID", Article.Export)
				ew_ExportAddValue(sExportStr, "Active", Article.Export)
				ew_ExportAddValue(sExportStr, "StartDT", Article.Export)
				ew_ExportAddValue(sExportStr, "Title", Article.Export)
				ew_ExportAddValue(sExportStr, "CompanyID", Article.Export)
				ew_ExportAddValue(sExportStr, "PageID", Article.Export)
				ew_ExportAddValue(sExportStr, "ContactID", Article.Export)
				ew_ExportAddValue(sExportStr, "Description", Article.Export)
				ew_ExportAddValue(sExportStr, "ArticleBody", Article.Export)
				ew_ExportAddValue(sExportStr, "ModifiedDT", Article.Export)
				ew_ExportAddValue(sExportStr, "Counter", Article.Export)
				ew_ExportAddValue(sExportStr, "VersionNo", Article.Export)
				ew_ExportAddValue(sExportStr, "userID", Article.Export)
				ew_ExportAddValue(sExportStr, "EndDT", Article.Export)
				ew_ExportAddValue(sExportStr, "ExpireDT", Article.Export)
				ew_ExportAddValue(sExportStr, "Author", Article.Export)
				ew_Write(ew_ExportLine(sExportStr, Article.Export))
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
				Article.RowType = EW_ROWTYPE_VIEW ' Render view
				RenderRow()
				If Article.Export = "xml" Then
					oXmlRec = oXmlDoc.CreateElement("record")
					oXmlTbl.AppendChild(oXmlRec)
					oXmlFld = oXmlDoc.CreateElement("ArticleID") ' ArticleID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Article.ArticleID.ExportValue(Article.Export, Article.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("Active") ' Active
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Article.Active.ExportValue(Article.Export, Article.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("StartDT") ' StartDT
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Article.StartDT.ExportValue(Article.Export, Article.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("Title") ' Title
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Article.Title.ExportValue(Article.Export, Article.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("CompanyID") ' CompanyID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Article.CompanyID.ExportValue(Article.Export, Article.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("zPageID") ' PageID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Article.zPageID.ExportValue(Article.Export, Article.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("ContactID") ' ContactID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Article.ContactID.ExportValue(Article.Export, Article.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("Description") ' Description
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Article.Description.ExportValue(Article.Export, Article.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("ArticleBody") ' ArticleBody
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Article.ArticleBody.ExportValue(Article.Export, Article.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("ModifiedDT") ' ModifiedDT
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Article.ModifiedDT.ExportValue(Article.Export, Article.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("Counter") ' Counter
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Article.Counter.ExportValue(Article.Export, Article.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("VersionNo") ' VersionNo
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Article.VersionNo.ExportValue(Article.Export, Article.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("userID") ' userID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Article.userID.ExportValue(Article.Export, Article.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("EndDT") ' EndDT
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Article.EndDT.ExportValue(Article.Export, Article.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("ExpireDT") ' ExpireDT
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Article.ExpireDT.ExportValue(Article.Export, Article.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("Author") ' Author
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Article.Author.ExportValue(Article.Export, Article.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
				Else

					' Vertical format
					If sExportStyle = "v" AndAlso Article.Export <> "csv" Then
						ew_Write(ew_ExportField("ArticleID", Article.ArticleID.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export)) ' ArticleID
						ew_Write(ew_ExportField("Active", Article.Active.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export)) ' Active
						ew_Write(ew_ExportField("StartDT", Article.StartDT.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export)) ' StartDT
						ew_Write(ew_ExportField("Title", Article.Title.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export)) ' Title
						ew_Write(ew_ExportField("CompanyID", Article.CompanyID.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export)) ' CompanyID
						ew_Write(ew_ExportField("PageID", Article.zPageID.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export)) ' PageID
						ew_Write(ew_ExportField("ContactID", Article.ContactID.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export)) ' ContactID
						ew_Write(ew_ExportField("Description", Article.Description.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export)) ' Description
						ew_Write(ew_ExportField("ArticleBody", Article.ArticleBody.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export)) ' ArticleBody
						ew_Write(ew_ExportField("ModifiedDT", Article.ModifiedDT.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export)) ' ModifiedDT
						ew_Write(ew_ExportField("Counter", Article.Counter.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export)) ' Counter
						ew_Write(ew_ExportField("VersionNo", Article.VersionNo.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export)) ' VersionNo
						ew_Write(ew_ExportField("userID", Article.userID.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export)) ' userID
						ew_Write(ew_ExportField("EndDT", Article.EndDT.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export)) ' EndDT
						ew_Write(ew_ExportField("ExpireDT", Article.ExpireDT.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export)) ' ExpireDT
						ew_Write(ew_ExportField("Author", Article.Author.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export)) ' Author

					' Horizontal format
					Else
						sExportStr = ""
						ew_ExportAddValue(sExportStr, Article.ArticleID.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export) ' ArticleID
						ew_ExportAddValue(sExportStr, Article.Active.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export) ' Active
						ew_ExportAddValue(sExportStr, Article.StartDT.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export) ' StartDT
						ew_ExportAddValue(sExportStr, Article.Title.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export) ' Title
						ew_ExportAddValue(sExportStr, Article.CompanyID.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export) ' CompanyID
						ew_ExportAddValue(sExportStr, Article.zPageID.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export) ' PageID
						ew_ExportAddValue(sExportStr, Article.ContactID.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export) ' ContactID
						ew_ExportAddValue(sExportStr, Article.Description.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export) ' Description
						ew_ExportAddValue(sExportStr, Article.ArticleBody.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export) ' ArticleBody
						ew_ExportAddValue(sExportStr, Article.ModifiedDT.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export) ' ModifiedDT
						ew_ExportAddValue(sExportStr, Article.Counter.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export) ' Counter
						ew_ExportAddValue(sExportStr, Article.VersionNo.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export) ' VersionNo
						ew_ExportAddValue(sExportStr, Article.userID.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export) ' userID
						ew_ExportAddValue(sExportStr, Article.EndDT.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export) ' EndDT
						ew_ExportAddValue(sExportStr, Article.ExpireDT.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export) ' ExpireDT
						ew_ExportAddValue(sExportStr, Article.Author.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export) ' Author
						ew_Write(ew_ExportLine(sExportStr, Article.Export))
					End If
				End If
			End If
		Loop

		' Close recordset
		Rs.Close()
		Rs.Dispose()
		If Article.Export = "xml" Then
			ew_Write("<?xml version=""1.0"" standalone=""yes""?>" & vbcrlf)
			ew_Write(oXmlDoc.OuterXml)
		Else
			ew_Write(ew_ExportFooter(Article.Export))
		End If
	End Sub

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "Article"
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
		Article_list = New cArticle_list(Me)		
		Article_list.Page_Init()

		' Page main processing
		Article_list.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If Article_list IsNot Nothing Then Article_list.Dispose()
	End Sub
End Class
