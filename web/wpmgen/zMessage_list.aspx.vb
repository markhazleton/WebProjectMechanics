Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class zMessage_list
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public zMessage_list As czMessage_list

	'
	' Page Class
	'
	Class czMessage_list
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
				If zMessage.UseTokenInUrl Then Url = Url & "t=" & zMessage.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If zMessage.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (zMessage.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (zMessage.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' zMessage
		Public Property zMessage() As czMessage
			Get				
				Return ParentPage.zMessage
			End Get
			Set(ByVal v As czMessage)
				ParentPage.zMessage = v	
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
			m_PageObjName = "zMessage_list"
			m_PageObjTypeName = "czMessage_list"

			' Table Name
			m_TableName = "Message"

			' Initialize table object
			zMessage = New czMessage(Me)

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
			zMessage.Export = ew_Get("export") ' Get export parameter
			ParentPage.gsExport = zMessage.Export ' Get export parameter, used in header
			ParentPage.gsExportFile = zMessage.TableVar ' Get export file, used in header
			If zMessage.Export = "excel" Then
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
			zMessage.Dispose()
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
		If (zMessage.RecordsPerPage = -1 OrElse zMessage.RecordsPerPage > 0) Then
			lDisplayRecs = zMessage.RecordsPerPage ' Restore from Session
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
		zMessage.Recordset_Searching(sSrchWhere)

		' Save search criteria
		If sSrchWhere <> "" Then
			If sSrchBasic = "" Then ResetBasicSearchParms()
			If sSrchAdvanced = "" Then ResetAdvancedSearchParms()
			zMessage.SearchWhere = sSrchWhere ' Save to Session
			lStartRec = 1 ' Reset start record counter
			zMessage.StartRecordNumber = lStartRec
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
		zMessage.SessionWhere = sFilter
		zMessage.CurrentFilter = ""

		' Export Data only
		If zMessage.Export = "html" OrElse zMessage.Export = "csv" OrElse zMessage.Export = "word" OrElse zMessage.Export = "excel" OrElse zMessage.Export = "xml" Then
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
			zMessage.RecordsPerPage = lDisplayRecs ' Save to Session

			' Reset start position
			lStartRec = 1
			zMessage.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	' Return Advanced Search WHERE based on QueryString parameters
	'
	Function AdvancedSearchWhere() As String		
		Dim sWhere As String = ""
		BuildSearchSql(sWhere, zMessage.MessageID, False) ' MessageID
		BuildSearchSql(sWhere, zMessage.zPageID, False) ' PageID
		BuildSearchSql(sWhere, zMessage.ParentMessageID, False) ' ParentMessageID
		BuildSearchSql(sWhere, zMessage.Subject, False) ' Subject
		BuildSearchSql(sWhere, zMessage.Author, False) ' Author
		BuildSearchSql(sWhere, zMessage.zEmail, False) ' Email
		BuildSearchSql(sWhere, zMessage.City, False) ' City
		BuildSearchSql(sWhere, zMessage.URL, False) ' URL
		BuildSearchSql(sWhere, zMessage.MessageDate, False) ' MessageDate
		BuildSearchSql(sWhere, zMessage.Body, False) ' Body

		' Set up search parm
		If sWhere <> "" Then
			SetSearchParm(zMessage.MessageID) ' MessageID
			SetSearchParm(zMessage.zPageID) ' PageID
			SetSearchParm(zMessage.ParentMessageID) ' ParentMessageID
			SetSearchParm(zMessage.Subject) ' Subject
			SetSearchParm(zMessage.Author) ' Author
			SetSearchParm(zMessage.zEmail) ' Email
			SetSearchParm(zMessage.City) ' City
			SetSearchParm(zMessage.URL) ' URL
			SetSearchParm(zMessage.MessageDate) ' MessageDate
			SetSearchParm(zMessage.Body) ' Body
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
		zMessage.SetAdvancedSearch("x_" & FldParm, Fld.AdvancedSearch.SearchValue)
		zMessage.SetAdvancedSearch("z_" & FldParm, Fld.AdvancedSearch.SearchOperator)
		zMessage.SetAdvancedSearch("v_" & FldParm, Fld.AdvancedSearch.SearchCondition)
		zMessage.SetAdvancedSearch("y_" & FldParm, Fld.AdvancedSearch.SearchValue2)
		zMessage.SetAdvancedSearch("w_" & FldParm, Fld.AdvancedSearch.SearchOperator2)
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
		sSql = sSql & "[Subject] LIKE '%" & sKeyword & "%' OR "
		sSql = sSql & "[Author] LIKE '%" & sKeyword & "%' OR "
		sSql = sSql & "[Email] LIKE '%" & sKeyword & "%' OR "
		sSql = sSql & "[City] LIKE '%" & sKeyword & "%' OR "
		sSql = sSql & "[URL] LIKE '%" & sKeyword & "%' OR "
		sSql = sSql & "[Body] LIKE '%" & sKeyword & "%' OR "
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
			zMessage.BasicSearchKeyword = sSearchKeyword
			zMessage.BasicSearchType = sSearchType
		End If
		Return sSearchStr
	End Function

	'
	' Clear all search parameters
	'
	Sub ResetSearchParms()

		' Clear search where
		sSrchWhere = ""
		zMessage.SearchWhere = sSrchWhere

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
		zMessage.BasicSearchKeyword = ""
		zMessage.BasicSearchType = ""
	End Sub

	'
	' Clear all advanced search parameters
	'
	Sub ResetAdvancedSearchParms()
		zMessage.SetAdvancedSearch("x_MessageID", "")
		zMessage.SetAdvancedSearch("x_zPageID", "")
		zMessage.SetAdvancedSearch("x_ParentMessageID", "")
		zMessage.SetAdvancedSearch("x_Subject", "")
		zMessage.SetAdvancedSearch("x_Author", "")
		zMessage.SetAdvancedSearch("x_zEmail", "")
		zMessage.SetAdvancedSearch("x_City", "")
		zMessage.SetAdvancedSearch("x_URL", "")
		zMessage.SetAdvancedSearch("x_MessageDate", "")
		zMessage.SetAdvancedSearch("x_Body", "")
	End Sub

	'
	' Restore all search parameters
	'
	Sub RestoreSearchParms()
		sSrchWhere = zMessage.SearchWhere

		' Restore advanced search settings
		If ParentPage.gsSearchError = "" Then
			RestoreAdvancedSearchParms()
		End If
	End Sub

	'
	' Restore all advanced search parameters
	'
	Sub RestoreAdvancedSearchParms()
		zMessage.MessageID.AdvancedSearch.SearchValue = zMessage.GetAdvancedSearch("x_MessageID")
		zMessage.zPageID.AdvancedSearch.SearchValue = zMessage.GetAdvancedSearch("x_zPageID")
		zMessage.ParentMessageID.AdvancedSearch.SearchValue = zMessage.GetAdvancedSearch("x_ParentMessageID")
		zMessage.Subject.AdvancedSearch.SearchValue = zMessage.GetAdvancedSearch("x_Subject")
		zMessage.Author.AdvancedSearch.SearchValue = zMessage.GetAdvancedSearch("x_Author")
		zMessage.zEmail.AdvancedSearch.SearchValue = zMessage.GetAdvancedSearch("x_zEmail")
		zMessage.City.AdvancedSearch.SearchValue = zMessage.GetAdvancedSearch("x_City")
		zMessage.URL.AdvancedSearch.SearchValue = zMessage.GetAdvancedSearch("x_URL")
		zMessage.MessageDate.AdvancedSearch.SearchValue = zMessage.GetAdvancedSearch("x_MessageDate")
		zMessage.Body.AdvancedSearch.SearchValue = zMessage.GetAdvancedSearch("x_Body")
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
			zMessage.CurrentOrder = ew_Get("order")
			zMessage.CurrentOrderType = ew_Get("ordertype")
			zMessage.UpdateSort(zMessage.MessageID) ' MessageID
			zMessage.UpdateSort(zMessage.zPageID) ' PageID
			zMessage.UpdateSort(zMessage.ParentMessageID) ' ParentMessageID
			zMessage.UpdateSort(zMessage.Subject) ' Subject
			zMessage.UpdateSort(zMessage.Author) ' Author
			zMessage.UpdateSort(zMessage.zEmail) ' Email
			zMessage.UpdateSort(zMessage.City) ' City
			zMessage.UpdateSort(zMessage.URL) ' URL
			zMessage.UpdateSort(zMessage.MessageDate) ' MessageDate
			zMessage.StartRecordNumber = 1 ' Reset start position
		End If
	End Sub

	'
	' Load Sort Order parameters
	'
	Sub LoadSortOrder()
		Dim sOrderBy As String
		sOrderBy = zMessage.SessionOrderBy ' Get order by from Session
		If sOrderBy = "" Then
			If zMessage.SqlOrderBy <> "" Then
				sOrderBy = zMessage.SqlOrderBy
				zMessage.SessionOrderBy = sOrderBy
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
				zMessage.SessionOrderBy = sOrderBy
				zMessage.MessageID.Sort = ""
				zMessage.zPageID.Sort = ""
				zMessage.ParentMessageID.Sort = ""
				zMessage.Subject.Sort = ""
				zMessage.Author.Sort = ""
				zMessage.zEmail.Sort = ""
				zMessage.City.Sort = ""
				zMessage.URL.Sort = ""
				zMessage.MessageDate.Sort = ""
			End If

			' Reset start position
			lStartRec = 1
			zMessage.StartRecordNumber = lStartRec
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
				zMessage.StartRecordNumber = lStartRec
			ElseIf ew_Get(EW_TABLE_PAGE_NO) <> "" AndAlso IsNumeric(ew_Get(EW_TABLE_PAGE_NO)) Then
				nPageNo = ew_ConvertToInt(ew_Get(EW_TABLE_PAGE_NO))
				lStartRec = (nPageNo-1)*lDisplayRecs+1
				If lStartRec <= 0 Then
					lStartRec = 1
				ElseIf lStartRec >= ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 Then
					lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1
				End If
				zMessage.StartRecordNumber = lStartRec
			End If
		End If
		lStartRec = zMessage.StartRecordNumber

		' Check if correct start record counter
		If lStartRec <= 0 Then ' Avoid invalid start record counter
			lStartRec = 1 ' Reset start record counter
			zMessage.StartRecordNumber = lStartRec
		ElseIf lStartRec > lTotalRecs Then ' Avoid starting record > total records
			lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to last page first record
			zMessage.StartRecordNumber = lStartRec
		ElseIf (lStartRec-1) Mod lDisplayRecs <> 0 Then
			lStartRec = ((lStartRec-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to page boundary
			zMessage.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	' Load default values
	'
	Sub LoadDefaultValues()
		zMessage.zPageID.CurrentValue = 0
		zMessage.ParentMessageID.CurrentValue = 0
		zMessage.MessageDate.CurrentValue = Now()
	End Sub

	'
	'  Load search values for validation
	'
	Sub LoadSearchValues()
		zMessage.MessageID.AdvancedSearch.SearchValue = ew_Get("x_MessageID")
    	zMessage.MessageID.AdvancedSearch.SearchOperator = ew_Get("z_MessageID")
		zMessage.zPageID.AdvancedSearch.SearchValue = ew_Get("x_zPageID")
    	zMessage.zPageID.AdvancedSearch.SearchOperator = ew_Get("z_zPageID")
		zMessage.ParentMessageID.AdvancedSearch.SearchValue = ew_Get("x_ParentMessageID")
    	zMessage.ParentMessageID.AdvancedSearch.SearchOperator = ew_Get("z_ParentMessageID")
		zMessage.Subject.AdvancedSearch.SearchValue = ew_Get("x_Subject")
    	zMessage.Subject.AdvancedSearch.SearchOperator = ew_Get("z_Subject")
		zMessage.Author.AdvancedSearch.SearchValue = ew_Get("x_Author")
    	zMessage.Author.AdvancedSearch.SearchOperator = ew_Get("z_Author")
		zMessage.zEmail.AdvancedSearch.SearchValue = ew_Get("x_zEmail")
    	zMessage.zEmail.AdvancedSearch.SearchOperator = ew_Get("z_zEmail")
		zMessage.City.AdvancedSearch.SearchValue = ew_Get("x_City")
    	zMessage.City.AdvancedSearch.SearchOperator = ew_Get("z_City")
		zMessage.URL.AdvancedSearch.SearchValue = ew_Get("x_URL")
    	zMessage.URL.AdvancedSearch.SearchOperator = ew_Get("z_URL")
		zMessage.MessageDate.AdvancedSearch.SearchValue = ew_Get("x_MessageDate")
    	zMessage.MessageDate.AdvancedSearch.SearchOperator = ew_Get("z_MessageDate")
		zMessage.Body.AdvancedSearch.SearchValue = ew_Get("x_Body")
    	zMessage.Body.AdvancedSearch.SearchOperator = ew_Get("z_Body")
	End Sub

	'
	' Load recordset
	'
	Function LoadRecordset() As OleDbDataReader

		' Recordset Selecting event
		zMessage.Recordset_Selecting(zMessage.CurrentFilter)

		' Load list page SQL
		Dim sSql As String = zMessage.ListSQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)

		' Count
		lTotalRecs = -1		
		Try			
			If sSql.StartsWith("SELECT * FROM ", StringComparison.InvariantCultureIgnoreCase) AndAlso _				
				ew_Empty(zMessage.SqlGroupBy) AndAlso _
				ew_Empty(zMessage.SqlHaving) Then
				Dim sCntSql As String = zMessage.SelectCountSQL

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
		zMessage.Recordset_Selected(Rs)
		Return Rs
	End Function

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = zMessage.KeyFilter

		' Row Selecting event
		zMessage.Row_Selecting(sFilter)

		' Load SQL based on filter
		zMessage.CurrentFilter = sFilter
		Dim sSql As String = zMessage.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				zMessage.Row_Selected(RsRow)
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
		zMessage.MessageID.DbValue = RsRow("MessageID")
		zMessage.zPageID.DbValue = RsRow("PageID")
		zMessage.ParentMessageID.DbValue = RsRow("ParentMessageID")
		zMessage.Subject.DbValue = RsRow("Subject")
		zMessage.Author.DbValue = RsRow("Author")
		zMessage.zEmail.DbValue = RsRow("Email")
		zMessage.City.DbValue = RsRow("City")
		zMessage.URL.DbValue = RsRow("URL")
		zMessage.MessageDate.DbValue = RsRow("MessageDate")
		zMessage.Body.DbValue = RsRow("Body")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		zMessage.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' MessageID

		zMessage.MessageID.CellCssStyle = ""
		zMessage.MessageID.CellCssClass = ""

		' PageID
		zMessage.zPageID.CellCssStyle = ""
		zMessage.zPageID.CellCssClass = ""

		' ParentMessageID
		zMessage.ParentMessageID.CellCssStyle = ""
		zMessage.ParentMessageID.CellCssClass = ""

		' Subject
		zMessage.Subject.CellCssStyle = ""
		zMessage.Subject.CellCssClass = ""

		' Author
		zMessage.Author.CellCssStyle = ""
		zMessage.Author.CellCssClass = ""

		' Email
		zMessage.zEmail.CellCssStyle = ""
		zMessage.zEmail.CellCssClass = ""

		' City
		zMessage.City.CellCssStyle = ""
		zMessage.City.CellCssClass = ""

		' URL
		zMessage.URL.CellCssStyle = ""
		zMessage.URL.CellCssClass = ""

		' MessageDate
		zMessage.MessageDate.CellCssStyle = ""
		zMessage.MessageDate.CellCssClass = ""

		'
		'  View  Row
		'

		If zMessage.RowType = EW_ROWTYPE_VIEW Then ' View row

			' MessageID
			zMessage.MessageID.ViewValue = zMessage.MessageID.CurrentValue
			zMessage.MessageID.CssStyle = ""
			zMessage.MessageID.CssClass = ""
			zMessage.MessageID.ViewCustomAttributes = ""

			' PageID
			zMessage.zPageID.ViewValue = zMessage.zPageID.CurrentValue
			zMessage.zPageID.CssStyle = ""
			zMessage.zPageID.CssClass = ""
			zMessage.zPageID.ViewCustomAttributes = ""

			' ParentMessageID
			zMessage.ParentMessageID.ViewValue = zMessage.ParentMessageID.CurrentValue
			zMessage.ParentMessageID.CssStyle = ""
			zMessage.ParentMessageID.CssClass = ""
			zMessage.ParentMessageID.ViewCustomAttributes = ""

			' Subject
			zMessage.Subject.ViewValue = zMessage.Subject.CurrentValue
			zMessage.Subject.CssStyle = ""
			zMessage.Subject.CssClass = ""
			zMessage.Subject.ViewCustomAttributes = ""

			' Author
			zMessage.Author.ViewValue = zMessage.Author.CurrentValue
			zMessage.Author.CssStyle = ""
			zMessage.Author.CssClass = ""
			zMessage.Author.ViewCustomAttributes = ""

			' Email
			zMessage.zEmail.ViewValue = zMessage.zEmail.CurrentValue
			zMessage.zEmail.CssStyle = ""
			zMessage.zEmail.CssClass = ""
			zMessage.zEmail.ViewCustomAttributes = ""

			' City
			zMessage.City.ViewValue = zMessage.City.CurrentValue
			zMessage.City.CssStyle = ""
			zMessage.City.CssClass = ""
			zMessage.City.ViewCustomAttributes = ""

			' URL
			zMessage.URL.ViewValue = zMessage.URL.CurrentValue
			zMessage.URL.CssStyle = ""
			zMessage.URL.CssClass = ""
			zMessage.URL.ViewCustomAttributes = ""

			' MessageDate
			zMessage.MessageDate.ViewValue = zMessage.MessageDate.CurrentValue
			zMessage.MessageDate.ViewValue = ew_FormatDateTime(zMessage.MessageDate.ViewValue, 6)
			zMessage.MessageDate.CssStyle = ""
			zMessage.MessageDate.CssClass = ""
			zMessage.MessageDate.ViewCustomAttributes = ""

			' View refer script
			' MessageID

			zMessage.MessageID.HrefValue = ""

			' PageID
			zMessage.zPageID.HrefValue = ""

			' ParentMessageID
			zMessage.ParentMessageID.HrefValue = ""

			' Subject
			zMessage.Subject.HrefValue = ""

			' Author
			zMessage.Author.HrefValue = ""

			' Email
			zMessage.zEmail.HrefValue = ""

			' City
			zMessage.City.HrefValue = ""

			' URL
			zMessage.URL.HrefValue = ""

			' MessageDate
			zMessage.MessageDate.HrefValue = ""
		End If

		' Row Rendered event
		zMessage.Row_Rendered()
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
		zMessage.MessageID.AdvancedSearch.SearchValue = zMessage.GetAdvancedSearch("x_MessageID")
		zMessage.zPageID.AdvancedSearch.SearchValue = zMessage.GetAdvancedSearch("x_zPageID")
		zMessage.ParentMessageID.AdvancedSearch.SearchValue = zMessage.GetAdvancedSearch("x_ParentMessageID")
		zMessage.Subject.AdvancedSearch.SearchValue = zMessage.GetAdvancedSearch("x_Subject")
		zMessage.Author.AdvancedSearch.SearchValue = zMessage.GetAdvancedSearch("x_Author")
		zMessage.zEmail.AdvancedSearch.SearchValue = zMessage.GetAdvancedSearch("x_zEmail")
		zMessage.City.AdvancedSearch.SearchValue = zMessage.GetAdvancedSearch("x_City")
		zMessage.URL.AdvancedSearch.SearchValue = zMessage.GetAdvancedSearch("x_URL")
		zMessage.MessageDate.AdvancedSearch.SearchValue = zMessage.GetAdvancedSearch("x_MessageDate")
		zMessage.Body.AdvancedSearch.SearchValue = zMessage.GetAdvancedSearch("x_Body")
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
		If zMessage.ExportAll Then
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
		If zMessage.Export = "xml" Then
			oXmlDoc = New XmlDocument()
			oXmlTbl = oXmlDoc.CreateElement("table")
			oXmlDoc.AppendChild(oXmlTbl)
		Else
			ew_Write(ew_ExportHeader(zMessage.Export))

			' Horizontal format, write header
			If sExportStyle <> "v" OrElse zMessage.Export = "csv" Then
				sExportStr = ""
				ew_ExportAddValue(sExportStr, "MessageID", zMessage.Export)
				ew_ExportAddValue(sExportStr, "PageID", zMessage.Export)
				ew_ExportAddValue(sExportStr, "ParentMessageID", zMessage.Export)
				ew_ExportAddValue(sExportStr, "Subject", zMessage.Export)
				ew_ExportAddValue(sExportStr, "Author", zMessage.Export)
				ew_ExportAddValue(sExportStr, "Email", zMessage.Export)
				ew_ExportAddValue(sExportStr, "City", zMessage.Export)
				ew_ExportAddValue(sExportStr, "URL", zMessage.Export)
				ew_ExportAddValue(sExportStr, "MessageDate", zMessage.Export)
				ew_Write(ew_ExportLine(sExportStr, zMessage.Export))
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
				zMessage.RowType = EW_ROWTYPE_VIEW ' Render view
				RenderRow()
				If zMessage.Export = "xml" Then
					oXmlRec = oXmlDoc.CreateElement("record")
					oXmlTbl.AppendChild(oXmlRec)
					oXmlFld = oXmlDoc.CreateElement("MessageID") ' MessageID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(zMessage.MessageID.ExportValue(zMessage.Export, zMessage.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("zPageID") ' PageID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(zMessage.zPageID.ExportValue(zMessage.Export, zMessage.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("ParentMessageID") ' ParentMessageID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(zMessage.ParentMessageID.ExportValue(zMessage.Export, zMessage.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("Subject") ' Subject
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(zMessage.Subject.ExportValue(zMessage.Export, zMessage.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("Author") ' Author
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(zMessage.Author.ExportValue(zMessage.Export, zMessage.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("zEmail") ' Email
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(zMessage.zEmail.ExportValue(zMessage.Export, zMessage.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("City") ' City
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(zMessage.City.ExportValue(zMessage.Export, zMessage.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("URL") ' URL
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(zMessage.URL.ExportValue(zMessage.Export, zMessage.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("MessageDate") ' MessageDate
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(zMessage.MessageDate.ExportValue(zMessage.Export, zMessage.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
				Else

					' Vertical format
					If sExportStyle = "v" AndAlso zMessage.Export <> "csv" Then
						ew_Write(ew_ExportField("MessageID", zMessage.MessageID.ExportValue(zMessage.Export, zMessage.ExportOriginalValue), zMessage.Export)) ' MessageID
						ew_Write(ew_ExportField("PageID", zMessage.zPageID.ExportValue(zMessage.Export, zMessage.ExportOriginalValue), zMessage.Export)) ' PageID
						ew_Write(ew_ExportField("ParentMessageID", zMessage.ParentMessageID.ExportValue(zMessage.Export, zMessage.ExportOriginalValue), zMessage.Export)) ' ParentMessageID
						ew_Write(ew_ExportField("Subject", zMessage.Subject.ExportValue(zMessage.Export, zMessage.ExportOriginalValue), zMessage.Export)) ' Subject
						ew_Write(ew_ExportField("Author", zMessage.Author.ExportValue(zMessage.Export, zMessage.ExportOriginalValue), zMessage.Export)) ' Author
						ew_Write(ew_ExportField("Email", zMessage.zEmail.ExportValue(zMessage.Export, zMessage.ExportOriginalValue), zMessage.Export)) ' Email
						ew_Write(ew_ExportField("City", zMessage.City.ExportValue(zMessage.Export, zMessage.ExportOriginalValue), zMessage.Export)) ' City
						ew_Write(ew_ExportField("URL", zMessage.URL.ExportValue(zMessage.Export, zMessage.ExportOriginalValue), zMessage.Export)) ' URL
						ew_Write(ew_ExportField("MessageDate", zMessage.MessageDate.ExportValue(zMessage.Export, zMessage.ExportOriginalValue), zMessage.Export)) ' MessageDate

					' Horizontal format
					Else
						sExportStr = ""
						ew_ExportAddValue(sExportStr, zMessage.MessageID.ExportValue(zMessage.Export, zMessage.ExportOriginalValue), zMessage.Export) ' MessageID
						ew_ExportAddValue(sExportStr, zMessage.zPageID.ExportValue(zMessage.Export, zMessage.ExportOriginalValue), zMessage.Export) ' PageID
						ew_ExportAddValue(sExportStr, zMessage.ParentMessageID.ExportValue(zMessage.Export, zMessage.ExportOriginalValue), zMessage.Export) ' ParentMessageID
						ew_ExportAddValue(sExportStr, zMessage.Subject.ExportValue(zMessage.Export, zMessage.ExportOriginalValue), zMessage.Export) ' Subject
						ew_ExportAddValue(sExportStr, zMessage.Author.ExportValue(zMessage.Export, zMessage.ExportOriginalValue), zMessage.Export) ' Author
						ew_ExportAddValue(sExportStr, zMessage.zEmail.ExportValue(zMessage.Export, zMessage.ExportOriginalValue), zMessage.Export) ' Email
						ew_ExportAddValue(sExportStr, zMessage.City.ExportValue(zMessage.Export, zMessage.ExportOriginalValue), zMessage.Export) ' City
						ew_ExportAddValue(sExportStr, zMessage.URL.ExportValue(zMessage.Export, zMessage.ExportOriginalValue), zMessage.Export) ' URL
						ew_ExportAddValue(sExportStr, zMessage.MessageDate.ExportValue(zMessage.Export, zMessage.ExportOriginalValue), zMessage.Export) ' MessageDate
						ew_Write(ew_ExportLine(sExportStr, zMessage.Export))
					End If
				End If
			End If
		Loop

		' Close recordset
		Rs.Close()
		Rs.Dispose()
		If zMessage.Export = "xml" Then
			ew_Write("<?xml version=""1.0"" standalone=""yes""?>" & vbcrlf)
			ew_Write(oXmlDoc.OuterXml)
		Else
			ew_Write(ew_ExportFooter(zMessage.Export))
		End If
	End Sub

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "Message"
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
		zMessage_list = New czMessage_list(Me)		
		zMessage_list.Page_Init()

		' Page main processing
		zMessage_list.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If zMessage_list IsNot Nothing Then zMessage_list.Dispose()
	End Sub
End Class
