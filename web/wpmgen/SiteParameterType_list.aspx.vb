Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class SiteParameterType_list
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public SiteParameterType_list As cSiteParameterType_list

	'
	' Page Class
	'
	Class cSiteParameterType_list
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
				If SiteParameterType.UseTokenInUrl Then Url = Url & "t=" & SiteParameterType.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If SiteParameterType.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (SiteParameterType.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (SiteParameterType.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' SiteParameterType
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
			m_PageID = "list"
			m_PageObjName = "SiteParameterType_list"
			m_PageObjTypeName = "cSiteParameterType_list"

			' Table Name
			m_TableName = "SiteParameterType"

			' Initialize table object
			SiteParameterType = New cSiteParameterType(Me)

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
			SiteParameterType.Export = ew_Get("export") ' Get export parameter
			ParentPage.gsExport = SiteParameterType.Export ' Get export parameter, used in header
			ParentPage.gsExportFile = SiteParameterType.TableVar ' Get export file, used in header
			If SiteParameterType.Export = "excel" Then
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
			SiteParameterType.Dispose()
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

		' Create form object
		ObjForm = New cFormObj
		If IsPageRequest Then ' Validate request

			' Set up records per page dynamically
			SetUpDisplayRecs()

			' Handle reset command
			ResetCmd()

			' Check QueryString parameters
			If ObjForm.GetValue("a_list") = "" Then ' Check if post back first
				If ew_Get("a") <> "" Then
					SiteParameterType.CurrentAction = ew_Get("a")

					' Clear inline mode
					If SiteParameterType.CurrentAction = "cancel" Then
						ClearInlineMode()
					End If

					' Switch to grid edit mode
					If SiteParameterType.CurrentAction = "gridedit" Then
						GridEditMode()
					End If

					' Switch to grid add mode
					If SiteParameterType.CurrentAction = "gridadd" Then
						GridAddMode()
					End If
				End If
			Else
				SiteParameterType.CurrentAction = ObjForm.GetValue("a_list") ' Get action

				' Grid Update
				If SiteParameterType.CurrentAction = "gridupdate" AndAlso ew_SameStr(ew_Session(EW_SESSION_INLINE_MODE), "gridedit") Then
					GridUpdate()
				End If

				' Grid Insert
				If SiteParameterType.CurrentAction = "gridinsert" AndAlso ew_SameStr(ew_Session(EW_SESSION_INLINE_MODE), "gridadd") Then
					GridInsert()
				End If
			End If

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
		If (SiteParameterType.RecordsPerPage = -1 OrElse SiteParameterType.RecordsPerPage > 0) Then
			lDisplayRecs = SiteParameterType.RecordsPerPage ' Restore from Session
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
		SiteParameterType.Recordset_Searching(sSrchWhere)

		' Save search criteria
		If sSrchWhere <> "" Then
			If sSrchBasic = "" Then ResetBasicSearchParms()
			If sSrchAdvanced = "" Then ResetAdvancedSearchParms()
			SiteParameterType.SearchWhere = sSrchWhere ' Save to Session
			lStartRec = 1 ' Reset start record counter
			SiteParameterType.StartRecordNumber = lStartRec
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
		SiteParameterType.SessionWhere = sFilter
		SiteParameterType.CurrentFilter = ""

		' Export Data only
		If SiteParameterType.Export = "html" OrElse SiteParameterType.Export = "csv" OrElse SiteParameterType.Export = "word" OrElse SiteParameterType.Export = "excel" OrElse SiteParameterType.Export = "xml" Then
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
			SiteParameterType.RecordsPerPage = lDisplayRecs ' Save to Session

			' Reset start position
			lStartRec = 1
			SiteParameterType.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	'  Exit out of inline mode
	'
	Sub ClearInlineMode()
		SiteParameterType.CurrentAction = "" ' Clear action
		ew_Session(EW_SESSION_INLINE_MODE) = "" ' Clear inline mode
	End Sub

	'
	' Switch to Grid Add Mode
	'
	Sub GridAddMode()
		ew_Session(EW_SESSION_INLINE_MODE) = "gridadd" ' Enabled grid add
	End Sub

	'
	' Switch to Grid Edit Mode
	'
	Sub GridEditMode()
		ew_Session(EW_SESSION_INLINE_MODE) = "gridedit" ' Enabled grid edit
	End Sub

	'
	' Peform update to grid
	'
	Sub GridUpdate()
		Dim rowindex As Integer = 1
		Dim bGridUpdate As Boolean = True
		Dim sKey As String = "", sThisKey As String, sSql As String
		Dim RsOld As ArrayList, RsNew As ArrayList
		Conn.BeginTrans() ' Begin transaction		
		WriteAuditTrailDummy("*** Batch update begin ***") ' Batch update begin

		' Get old recordset
		SiteParameterType.CurrentFilter = BuildKeyFilter()
		sSql = SiteParameterType.SQL
		RsOld = Conn.GetRows(sSql)

		' Update row index and get row key
		ObjForm.Index = rowindex
		sThisKey = ObjForm.GetValue("k_key")

		' Update all rows based on key
		Do While (sThisKey <> "")

			' Load all values and keys
			LoadFormValues() ' Get form values

			' Validate Form
			If Not ValidateForm() Then
				bGridUpdate = False ' Form error, reset action
				Message = ParentPage.gsFormError
			Else
				If SetupKeyValues(sThisKey) Then ' Set up key values
					SiteParameterType.SendEmail = False ' Do not send email on update success
					bGridUpdate = EditRow() ' Update this row
				Else
					bGridUpdate = False ' update failed
				End If
			End If
			If bGridUpdate Then
				If sKey <> "" Then sKey = sKey & ", "
				sKey = sKey & sThisKey
			Else
				Exit Do
			End If

			' Update row index and get row key
			rowindex = rowindex + 1 ' next row
			ObjForm.Index = rowindex
			sThisKey = ObjForm.GetValue("k_key")
		Loop
		If bGridUpdate Then
			Conn.CommitTrans() ' Commit transaction			

			' Get new recordset
			RsNew = Conn.GetRows(sSql)
			WriteAuditTrailDummy("*** Batch update successful ***") ' Batch update success
			Message = "Update succeeded" ' Set update success message
			ClearInlineMode() ' Clear inline edit mode
		Else
			Conn.RollbackTrans() ' Rollback transaction			
			WriteAuditTrailDummy("*** Batch update rollback ***") ' Batch update rollback
			If Message = "" Then
				Message = "Update failed" ' Set update failed message
			End If
			SiteParameterType.EventCancelled = True ' Set event cancelled
			SiteParameterType.CurrentAction = "gridedit" ' Stay in gridedit mode
		End If
	End Sub

	'
	'  Build filter for all keys
	'
	Function BuildKeyFilter() As String
		Dim rowindex As Integer = 1, sThisKey As String
		Dim sKey As String
		Dim sWrkFilter As String = "", sFilter As String

		' Update row index and get row key
		ObjForm.Index = rowindex
		sThisKey = ObjForm.GetValue("k_key")
		Do While (sThisKey <> "")
			If SetupKeyValues(sThisKey) Then
				sFilter = SiteParameterType.KeyFilter
				If sWrkFilter <> "" Then sWrkFilter = sWrkFilter & " OR "
				sWrkFilter = sWrkFilter & sFilter
			Else
				sWrkFilter = "0=1"
				Exit Do
			End If

			' Update row index and get row key
			rowindex = rowindex + 1 ' next row
			ObjForm.Index = rowindex
			sThisKey = ObjForm.GetValue("k_key")
		Loop
		Return sWrkFilter
	End Function

	'
	' Set up key values
	'
	Function SetupKeyValues(key As String) As Boolean
		Dim arKeyFlds() As String
		arKeyFlds = key.Split(New Char() {Convert.ToChar(EW_COMPOSITE_KEY_SEPARATOR)})
		If arKeyFlds.Length >= 1 Then
			SiteParameterType.SiteParameterTypeID.FormValue = arKeyFlds(0)
			If Not IsNumeric(SiteParameterType.SiteParameterTypeID.FormValue) Then	Return False
			Return True
		End If
		Return False
	End Function

	' Grid Insert
	Sub GridInsert()
		Dim addcnt As Integer = 0
		Dim rowcnt As Object
		Dim rowindex As Integer = 1
		Dim bGridInsert As Boolean = False
		Dim sSql As String, sWrkFilter As String = ""
		Dim sFilter As String, sKey As String = "", sThisKey As String
		Dim RsNew As ArrayList
		Conn.BeginTrans() ' Begin transaction
		WriteAuditTrailDummy("*** Batch insert begin ***") ' Batch insert begin

		' Get row count
		objForm.Index = 0
		rowcnt = objForm.GetValue("key_count")
		If rowcnt = "" OrElse Not IsNumeric(rowcnt) Then rowcnt = 0

		' Insert all rows
		For rowindex = 1 to rowcnt

			' Load current row values
			objForm.Index = rowindex
			LoadFormValues() ' Get form values
			If Not EmptyRow() Then
				addcnt = addcnt + 1
				SiteParameterType.SendEmail = False ' Do not send email on insert success

				' Validate Form
				If Not ValidateForm() Then
					bGridInsert = False ' Form error, reset action
					Message = ParentPage.gsFormError
				Else
					bGridInsert = AddRow() ' Insert this row
				End If
				If bGridInsert Then
					If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
					sKey = sKey & SiteParameterType.SiteParameterTypeID.CurrentValue

					' Add filter for this record
					sFilter = SiteParameterType.KeyFilter
					If sWrkFilter <> "" Then sWrkFilter = sWrkFilter & " OR "
					sWrkFilter = sWrkFilter & sFilter
				Else
					Exit For
				End If
			End If
		Next
		If bGridInsert Then
			Conn.CommitTrans() ' Commit transaction			

			' Get new recordset
			SiteParameterType.CurrentFilter = sWrkFilter
			sSql = SiteParameterType.SQL
			RsNew = Conn.GetRows(sSql)
			WriteAuditTrailDummy("*** Batch insert successful ***") ' Batch insert success
			Message = "Insert succeeded" ' Set insert success message
			ClearInlineMode() ' Clear grid add mode
		Else
			Conn.RollbackTrans() ' Rollback transaction
			WriteAuditTrailDummy("*** Batch insert rollback ***") ' Batch insert rollback
			If addcnt = 0 Then ' No record inserted
				Message = "No records to be added"
			ElseIf Message = "" Then
				Message = "Insert failed" ' Set insert failed message
			End If
			SiteParameterType.EventCancelled = True ' Set event cancelled
			SiteParameterType.CurrentAction = "gridadd" ' Stay in gridadd mode
		End If
	End Sub

	Function EmptyRow() As Boolean
		Dim Empty As Boolean = True
		If Empty Then Empty = ew_SameStr(SiteParameterType.SiteParameterTypeNM.CurrentValue, SiteParameterType.SiteParameterTypeNM.OldValue)
		If Empty Then Empty = ew_SameStr(SiteParameterType.SiteParameterTypeDS.CurrentValue, SiteParameterType.SiteParameterTypeDS.OldValue)
		If Empty Then Empty = ew_SameStr(SiteParameterType.SiteParameterTypeOrder.CurrentValue, SiteParameterType.SiteParameterTypeOrder.OldValue)
		Return Empty
	End Function

	'
	' Restore form values for current row
	'
	Sub RestoreCurrentRowFormValues(idx As Integer)
		Dim sKey As String, arKeyFlds() As String

		' Get row based on current index
		ObjForm.Index = idx
		If SiteParameterType.CurrentAction = "gridadd" Then
			LoadFormValues() ' Load form values
		End If
		If SiteParameterType.CurrentAction = "gridedit" Then
			sKey = ObjForm.GetValue("k_key")
			arKeyFlds = sKey.Split(New Char() {Convert.ToChar(EW_COMPOSITE_KEY_SEPARATOR)})
			If arKeyFlds.Length >= 1 Then
				If ew_SameStr(arKeyFlds(0), SiteParameterType.SiteParameterTypeID.CurrentValue) Then
					LoadFormValues() ' Load form values
				End If
			End If
		End If
	End Sub

	'
	' Return Advanced Search WHERE based on QueryString parameters
	'
	Function AdvancedSearchWhere() As String		
		Dim sWhere As String = ""
		BuildSearchSql(sWhere, SiteParameterType.SiteParameterTypeNM, False) ' SiteParameterTypeNM
		BuildSearchSql(sWhere, SiteParameterType.SiteParameterTypeDS, False) ' SiteParameterTypeDS
		BuildSearchSql(sWhere, SiteParameterType.SiteParameterTypeOrder, False) ' SiteParameterTypeOrder

		' Set up search parm
		If sWhere <> "" Then
			SetSearchParm(SiteParameterType.SiteParameterTypeNM) ' SiteParameterTypeNM
			SetSearchParm(SiteParameterType.SiteParameterTypeDS) ' SiteParameterTypeDS
			SetSearchParm(SiteParameterType.SiteParameterTypeOrder) ' SiteParameterTypeOrder
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
		SiteParameterType.SetAdvancedSearch("x_" & FldParm, Fld.AdvancedSearch.SearchValue)
		SiteParameterType.SetAdvancedSearch("z_" & FldParm, Fld.AdvancedSearch.SearchOperator)
		SiteParameterType.SetAdvancedSearch("v_" & FldParm, Fld.AdvancedSearch.SearchCondition)
		SiteParameterType.SetAdvancedSearch("y_" & FldParm, Fld.AdvancedSearch.SearchValue2)
		SiteParameterType.SetAdvancedSearch("w_" & FldParm, Fld.AdvancedSearch.SearchOperator2)
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
		sSql = sSql & "[SiteParameterTypeNM] LIKE '%" & sKeyword & "%' OR "
		sSql = sSql & "[SiteParameterTypeDS] LIKE '%" & sKeyword & "%' OR "
		sSql = sSql & "[SiteParameterTemplate] LIKE '%" & sKeyword & "%' OR "
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
			SiteParameterType.BasicSearchKeyword = sSearchKeyword
			SiteParameterType.BasicSearchType = sSearchType
		End If
		Return sSearchStr
	End Function

	'
	' Clear all search parameters
	'
	Sub ResetSearchParms()

		' Clear search where
		sSrchWhere = ""
		SiteParameterType.SearchWhere = sSrchWhere

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
		SiteParameterType.BasicSearchKeyword = ""
		SiteParameterType.BasicSearchType = ""
	End Sub

	'
	' Clear all advanced search parameters
	'
	Sub ResetAdvancedSearchParms()
		SiteParameterType.SetAdvancedSearch("x_SiteParameterTypeNM", "")
		SiteParameterType.SetAdvancedSearch("x_SiteParameterTypeDS", "")
		SiteParameterType.SetAdvancedSearch("x_SiteParameterTypeOrder", "")
	End Sub

	'
	' Restore all search parameters
	'
	Sub RestoreSearchParms()
		sSrchWhere = SiteParameterType.SearchWhere

		' Restore advanced search settings
		If ParentPage.gsSearchError = "" Then
			RestoreAdvancedSearchParms()
		End If
	End Sub

	'
	' Restore all advanced search parameters
	'
	Sub RestoreAdvancedSearchParms()
		SiteParameterType.SiteParameterTypeNM.AdvancedSearch.SearchValue = SiteParameterType.GetAdvancedSearch("x_SiteParameterTypeNM")
		SiteParameterType.SiteParameterTypeDS.AdvancedSearch.SearchValue = SiteParameterType.GetAdvancedSearch("x_SiteParameterTypeDS")
		SiteParameterType.SiteParameterTypeOrder.AdvancedSearch.SearchValue = SiteParameterType.GetAdvancedSearch("x_SiteParameterTypeOrder")
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
			SiteParameterType.CurrentOrder = ew_Get("order")
			SiteParameterType.CurrentOrderType = ew_Get("ordertype")
			SiteParameterType.UpdateSort(SiteParameterType.SiteParameterTypeNM) ' SiteParameterTypeNM
			SiteParameterType.UpdateSort(SiteParameterType.SiteParameterTypeDS) ' SiteParameterTypeDS
			SiteParameterType.UpdateSort(SiteParameterType.SiteParameterTypeOrder) ' SiteParameterTypeOrder
			SiteParameterType.StartRecordNumber = 1 ' Reset start position
		End If
	End Sub

	'
	' Load Sort Order parameters
	'
	Sub LoadSortOrder()
		Dim sOrderBy As String
		sOrderBy = SiteParameterType.SessionOrderBy ' Get order by from Session
		If sOrderBy = "" Then
			If SiteParameterType.SqlOrderBy <> "" Then
				sOrderBy = SiteParameterType.SqlOrderBy
				SiteParameterType.SessionOrderBy = sOrderBy
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
				SiteParameterType.SessionOrderBy = sOrderBy
				SiteParameterType.SiteParameterTypeNM.Sort = ""
				SiteParameterType.SiteParameterTypeDS.Sort = ""
				SiteParameterType.SiteParameterTypeOrder.Sort = ""
			End If

			' Reset start position
			lStartRec = 1
			SiteParameterType.StartRecordNumber = lStartRec
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
				SiteParameterType.StartRecordNumber = lStartRec
			ElseIf ew_Get(EW_TABLE_PAGE_NO) <> "" AndAlso IsNumeric(ew_Get(EW_TABLE_PAGE_NO)) Then
				nPageNo = ew_ConvertToInt(ew_Get(EW_TABLE_PAGE_NO))
				lStartRec = (nPageNo-1)*lDisplayRecs+1
				If lStartRec <= 0 Then
					lStartRec = 1
				ElseIf lStartRec >= ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 Then
					lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1
				End If
				SiteParameterType.StartRecordNumber = lStartRec
			End If
		End If
		lStartRec = SiteParameterType.StartRecordNumber

		' Check if correct start record counter
		If lStartRec <= 0 Then ' Avoid invalid start record counter
			lStartRec = 1 ' Reset start record counter
			SiteParameterType.StartRecordNumber = lStartRec
		ElseIf lStartRec > lTotalRecs Then ' Avoid starting record > total records
			lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to last page first record
			SiteParameterType.StartRecordNumber = lStartRec
		ElseIf (lStartRec-1) Mod lDisplayRecs <> 0 Then
			lStartRec = ((lStartRec-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to page boundary
			SiteParameterType.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	' Load default values
	'
	Sub LoadDefaultValues()
		SiteParameterType.SiteParameterTypeNM.OldValue = SiteParameterType.SiteParameterTypeNM.CurrentValue
		SiteParameterType.SiteParameterTypeDS.OldValue = SiteParameterType.SiteParameterTypeDS.CurrentValue
		SiteParameterType.SiteParameterTypeOrder.OldValue = SiteParameterType.SiteParameterTypeOrder.CurrentValue
	End Sub

	'
	'  Load search values for validation
	'
	Sub LoadSearchValues()
		SiteParameterType.SiteParameterTypeNM.AdvancedSearch.SearchValue = ew_Get("x_SiteParameterTypeNM")
    	SiteParameterType.SiteParameterTypeNM.AdvancedSearch.SearchOperator = ew_Get("z_SiteParameterTypeNM")
		SiteParameterType.SiteParameterTypeDS.AdvancedSearch.SearchValue = ew_Get("x_SiteParameterTypeDS")
    	SiteParameterType.SiteParameterTypeDS.AdvancedSearch.SearchOperator = ew_Get("z_SiteParameterTypeDS")
		SiteParameterType.SiteParameterTypeOrder.AdvancedSearch.SearchValue = ew_Get("x_SiteParameterTypeOrder")
    	SiteParameterType.SiteParameterTypeOrder.AdvancedSearch.SearchOperator = ew_Get("z_SiteParameterTypeOrder")
	End Sub

	'
	' Load form values
	'
	Sub LoadFormValues()
		SiteParameterType.SiteParameterTypeNM.FormValue = ObjForm.GetValue("x_SiteParameterTypeNM")
		SiteParameterType.SiteParameterTypeNM.OldValue = ObjForm.GetValue("o_SiteParameterTypeNM")
		SiteParameterType.SiteParameterTypeDS.FormValue = ObjForm.GetValue("x_SiteParameterTypeDS")
		SiteParameterType.SiteParameterTypeDS.OldValue = ObjForm.GetValue("o_SiteParameterTypeDS")
		SiteParameterType.SiteParameterTypeOrder.FormValue = ObjForm.GetValue("x_SiteParameterTypeOrder")
		SiteParameterType.SiteParameterTypeOrder.OldValue = ObjForm.GetValue("o_SiteParameterTypeOrder")
		SiteParameterType.SiteParameterTypeID.FormValue = ObjForm.GetValue("x_SiteParameterTypeID")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		SiteParameterType.SiteParameterTypeNM.CurrentValue = SiteParameterType.SiteParameterTypeNM.FormValue
		SiteParameterType.SiteParameterTypeDS.CurrentValue = SiteParameterType.SiteParameterTypeDS.FormValue
		SiteParameterType.SiteParameterTypeOrder.CurrentValue = SiteParameterType.SiteParameterTypeOrder.FormValue
		SiteParameterType.SiteParameterTypeID.CurrentValue = SiteParameterType.SiteParameterTypeID.FormValue
	End Sub

	'
	' Load recordset
	'
	Function LoadRecordset() As OleDbDataReader

		' Recordset Selecting event
		SiteParameterType.Recordset_Selecting(SiteParameterType.CurrentFilter)

		' Load list page SQL
		Dim sSql As String = SiteParameterType.ListSQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)

		' Count
		lTotalRecs = -1		
		Try			
			If sSql.StartsWith("SELECT * FROM ", StringComparison.InvariantCultureIgnoreCase) AndAlso _				
				ew_Empty(SiteParameterType.SqlGroupBy) AndAlso _
				ew_Empty(SiteParameterType.SqlHaving) Then
				Dim sCntSql As String = SiteParameterType.SelectCountSQL

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
		SiteParameterType.Recordset_Selected(Rs)
		Return Rs
	End Function

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = SiteParameterType.KeyFilter

		' Row Selecting event
		SiteParameterType.Row_Selecting(sFilter)

		' Load SQL based on filter
		SiteParameterType.CurrentFilter = sFilter
		Dim sSql As String = SiteParameterType.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				SiteParameterType.Row_Selected(RsRow)
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
		SiteParameterType.SiteParameterTypeID.DbValue = RsRow("SiteParameterTypeID")
		SiteParameterType.SiteParameterTypeNM.DbValue = RsRow("SiteParameterTypeNM")
		SiteParameterType.SiteParameterTypeDS.DbValue = RsRow("SiteParameterTypeDS")
		SiteParameterType.SiteParameterTypeOrder.DbValue = RsRow("SiteParameterTypeOrder")
		SiteParameterType.SiteParameterTemplate.DbValue = RsRow("SiteParameterTemplate")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		SiteParameterType.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' SiteParameterTypeNM

		SiteParameterType.SiteParameterTypeNM.CellCssStyle = ""
		SiteParameterType.SiteParameterTypeNM.CellCssClass = ""

		' SiteParameterTypeDS
		SiteParameterType.SiteParameterTypeDS.CellCssStyle = ""
		SiteParameterType.SiteParameterTypeDS.CellCssClass = ""

		' SiteParameterTypeOrder
		SiteParameterType.SiteParameterTypeOrder.CellCssStyle = ""
		SiteParameterType.SiteParameterTypeOrder.CellCssClass = ""

		'
		'  View  Row
		'

		If SiteParameterType.RowType = EW_ROWTYPE_VIEW Then ' View row

			' SiteParameterTypeNM
			SiteParameterType.SiteParameterTypeNM.ViewValue = SiteParameterType.SiteParameterTypeNM.CurrentValue
			SiteParameterType.SiteParameterTypeNM.CssStyle = ""
			SiteParameterType.SiteParameterTypeNM.CssClass = ""
			SiteParameterType.SiteParameterTypeNM.ViewCustomAttributes = ""

			' SiteParameterTypeDS
			SiteParameterType.SiteParameterTypeDS.ViewValue = SiteParameterType.SiteParameterTypeDS.CurrentValue
			SiteParameterType.SiteParameterTypeDS.CssStyle = ""
			SiteParameterType.SiteParameterTypeDS.CssClass = ""
			SiteParameterType.SiteParameterTypeDS.ViewCustomAttributes = ""

			' SiteParameterTypeOrder
			SiteParameterType.SiteParameterTypeOrder.ViewValue = SiteParameterType.SiteParameterTypeOrder.CurrentValue
			SiteParameterType.SiteParameterTypeOrder.CssStyle = ""
			SiteParameterType.SiteParameterTypeOrder.CssClass = ""
			SiteParameterType.SiteParameterTypeOrder.ViewCustomAttributes = ""

			' SiteParameterTemplate
			SiteParameterType.SiteParameterTemplate.ViewValue = SiteParameterType.SiteParameterTemplate.CurrentValue
			SiteParameterType.SiteParameterTemplate.CssStyle = ""
			SiteParameterType.SiteParameterTemplate.CssClass = ""
			SiteParameterType.SiteParameterTemplate.ViewCustomAttributes = ""

			' View refer script
			' SiteParameterTypeNM

			SiteParameterType.SiteParameterTypeNM.HrefValue = ""

			' SiteParameterTypeDS
			SiteParameterType.SiteParameterTypeDS.HrefValue = ""

			' SiteParameterTypeOrder
			SiteParameterType.SiteParameterTypeOrder.HrefValue = ""

		'
		'  Add Row
		'

		ElseIf SiteParameterType.RowType = EW_ROWTYPE_ADD Then ' Add row

			' SiteParameterTypeNM
			SiteParameterType.SiteParameterTypeNM.EditCustomAttributes = ""
			SiteParameterType.SiteParameterTypeNM.EditValue = ew_HtmlEncode(SiteParameterType.SiteParameterTypeNM.CurrentValue)

			' SiteParameterTypeDS
			SiteParameterType.SiteParameterTypeDS.EditCustomAttributes = ""
			SiteParameterType.SiteParameterTypeDS.EditValue = ew_HtmlEncode(SiteParameterType.SiteParameterTypeDS.CurrentValue)

			' SiteParameterTypeOrder
			SiteParameterType.SiteParameterTypeOrder.EditCustomAttributes = ""
			SiteParameterType.SiteParameterTypeOrder.EditValue = ew_HtmlEncode(SiteParameterType.SiteParameterTypeOrder.CurrentValue)

		'
		'  Edit Row
		'

		ElseIf SiteParameterType.RowType = EW_ROWTYPE_EDIT Then ' Edit row

			' SiteParameterTypeNM
			SiteParameterType.SiteParameterTypeNM.EditCustomAttributes = ""
			SiteParameterType.SiteParameterTypeNM.EditValue = ew_HtmlEncode(SiteParameterType.SiteParameterTypeNM.CurrentValue)

			' SiteParameterTypeDS
			SiteParameterType.SiteParameterTypeDS.EditCustomAttributes = ""
			SiteParameterType.SiteParameterTypeDS.EditValue = ew_HtmlEncode(SiteParameterType.SiteParameterTypeDS.CurrentValue)

			' SiteParameterTypeOrder
			SiteParameterType.SiteParameterTypeOrder.EditCustomAttributes = ""
			SiteParameterType.SiteParameterTypeOrder.EditValue = ew_HtmlEncode(SiteParameterType.SiteParameterTypeOrder.CurrentValue)

			' Edit refer script
			' SiteParameterTypeNM

			SiteParameterType.SiteParameterTypeNM.HrefValue = ""

			' SiteParameterTypeDS
			SiteParameterType.SiteParameterTypeDS.HrefValue = ""

			' SiteParameterTypeOrder
			SiteParameterType.SiteParameterTypeOrder.HrefValue = ""
		End If

		' Row Rendered event
		SiteParameterType.Row_Rendered()
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
	' Validate form
	'
	Function ValidateForm() As Boolean

		' Initialize
		ParentPage.gsFormError = ""

		' Check if validation required
		If Not EW_SERVER_VALIDATE Then Return (ParentPage.gsFormError = "")
		If Not ew_CheckInteger(SiteParameterType.SiteParameterTypeOrder.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Incorrect integer - Parameter Order"
		End If

		' Return validate result
		Dim Valid As Boolean = (ParentPage.gsFormError = "")

		' Form_CustomValidate event
		Dim sFormCustomError As String = ""
		Valid = Valid And Form_CustomValidate(sFormCustomError)
		If sFormCustomError <> "" Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & sFormCustomError
		End If
		Return Valid
	End Function

	'
	' Update record based on key values
	'
	Function EditRow() As Boolean
		Dim RsEdit As OleDbDataReader, RsChk As OleDbDataReader
		Dim sSql As String, sFilter As String
		Dim sSqlChk As String, sFilterChk As String
		Dim bUpdateRow As Boolean
		Dim RsOld As OrderedDictionary
		Dim sIdxErrMsg As String
		Dim Rs As New OrderedDictionary
		sFilter = SiteParameterType.KeyFilter
		SiteParameterType.CurrentFilter  = sFilter
		sSql = SiteParameterType.SQL
		Try
			RsEdit = Conn.GetDataReader(sSql) 
		Catch e As Exception
			If EW_DEBUG_ENABLED Then Throw
			Message = e.Message
			RsEdit.Close()
			EditRow = False
		End Try
		If Not RsEdit.Read() Then
			RsEdit.Close()
			EditRow = False ' Update Failed
		Else
			RsOld = Conn.GetRow(RsEdit)
			RsEdit.Close()

			' SiteParameterTypeNM
			SiteParameterType.SiteParameterTypeNM.SetDbValue(SiteParameterType.SiteParameterTypeNM.CurrentValue, System.DBNull.Value)
			Rs("SiteParameterTypeNM") = SiteParameterType.SiteParameterTypeNM.DbValue

			' SiteParameterTypeDS
			SiteParameterType.SiteParameterTypeDS.SetDbValue(SiteParameterType.SiteParameterTypeDS.CurrentValue, System.DBNull.Value)
			Rs("SiteParameterTypeDS") = SiteParameterType.SiteParameterTypeDS.DbValue

			' SiteParameterTypeOrder
			SiteParameterType.SiteParameterTypeOrder.SetDbValue(SiteParameterType.SiteParameterTypeOrder.CurrentValue, System.DBNull.Value)
			Rs("SiteParameterTypeOrder") = SiteParameterType.SiteParameterTypeOrder.DbValue

			' Row Updating event
			bUpdateRow = SiteParameterType.Row_Updating(RsOld, Rs)
			If bUpdateRow Then
				Try
					SiteParameterType.Update(Rs)
					EditRow = True
				Catch e As Exception
					If EW_DEBUG_ENABLED Then Throw
					Message = e.Message
					EditRow = False
				End Try
			Else
				If SiteParameterType.CancelMessage <> "" Then
					Message = SiteParameterType.CancelMessage
					SiteParameterType.CancelMessage = ""
				Else
					Message = "Update cancelled"
				End If
				EditRow = False
			End If
		End If

		' Row Updated event
		If EditRow Then
			SiteParameterType.Row_Updated(RsOld, Rs)
		End If
		If EditRow Then
			WriteAuditTrailOnEdit(RsOld, Rs)
		End If
	End Function

	'
	' Add record
	'
	Function AddRow() As Boolean
		Dim Rs As New OrderedDictionary
		Dim sSql As String, sFilter As String
		Dim bInsertRow As Boolean
		Dim RsChk As OleDbDataReader
		Dim sIdxErrMsg As String
		Dim LastInsertId As Object

		' SiteParameterTypeNM
		SiteParameterType.SiteParameterTypeNM.SetDbValue(SiteParameterType.SiteParameterTypeNM.CurrentValue, System.DBNull.Value)
		Rs("SiteParameterTypeNM") = SiteParameterType.SiteParameterTypeNM.DbValue

		' SiteParameterTypeDS
		SiteParameterType.SiteParameterTypeDS.SetDbValue(SiteParameterType.SiteParameterTypeDS.CurrentValue, System.DBNull.Value)
		Rs("SiteParameterTypeDS") = SiteParameterType.SiteParameterTypeDS.DbValue

		' SiteParameterTypeOrder
		SiteParameterType.SiteParameterTypeOrder.SetDbValue(SiteParameterType.SiteParameterTypeOrder.CurrentValue, System.DBNull.Value)
		Rs("SiteParameterTypeOrder") = SiteParameterType.SiteParameterTypeOrder.DbValue

		' Row Inserting event
		bInsertRow = SiteParameterType.Row_Inserting(Rs)
		If bInsertRow Then
			Try	
				SiteParameterType.Insert(Rs)
				AddRow = True
			Catch e As Exception
				If EW_DEBUG_ENABLED Then Throw
				Message = e.Message				
				AddRow = False
			End Try
		Else
			If SiteParameterType.CancelMessage <> "" Then
				Message = SiteParameterType.CancelMessage
				SiteParameterType.CancelMessage = ""
			Else
				Message = "Insert cancelled"
			End If
			AddRow = False
		End If
		If AddRow Then
			LastInsertId = Conn.GetLastInsertId()
			SiteParameterType.SiteParameterTypeID.DbValue = LastInsertId
			Rs("SiteParameterTypeID") = SiteParameterType.SiteParameterTypeID.DbValue		

			' Row Inserted event
			SiteParameterType.Row_Inserted(Rs)
			WriteAuditTrailOnAdd(Rs)
		End If
	End Function

	'
	' Load advanced search
	'
	Sub LoadAdvancedSearch()
		SiteParameterType.SiteParameterTypeNM.AdvancedSearch.SearchValue = SiteParameterType.GetAdvancedSearch("x_SiteParameterTypeNM")
		SiteParameterType.SiteParameterTypeDS.AdvancedSearch.SearchValue = SiteParameterType.GetAdvancedSearch("x_SiteParameterTypeDS")
		SiteParameterType.SiteParameterTypeOrder.AdvancedSearch.SearchValue = SiteParameterType.GetAdvancedSearch("x_SiteParameterTypeOrder")
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
		If SiteParameterType.ExportAll Then
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
		If SiteParameterType.Export = "xml" Then
			oXmlDoc = New XmlDocument()
			oXmlTbl = oXmlDoc.CreateElement("table")
			oXmlDoc.AppendChild(oXmlTbl)
		Else
			ew_Write(ew_ExportHeader(SiteParameterType.Export))

			' Horizontal format, write header
			If sExportStyle <> "v" OrElse SiteParameterType.Export = "csv" Then
				sExportStr = ""
				ew_ExportAddValue(sExportStr, "SiteParameterTypeNM", SiteParameterType.Export)
				ew_ExportAddValue(sExportStr, "SiteParameterTypeDS", SiteParameterType.Export)
				ew_ExportAddValue(sExportStr, "SiteParameterTypeOrder", SiteParameterType.Export)
				ew_ExportAddValue(sExportStr, "SiteParameterTemplate", SiteParameterType.Export)
				ew_Write(ew_ExportLine(sExportStr, SiteParameterType.Export))
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
				SiteParameterType.RowType = EW_ROWTYPE_VIEW ' Render view
				RenderRow()
				If SiteParameterType.Export = "xml" Then
					oXmlRec = oXmlDoc.CreateElement("record")
					oXmlTbl.AppendChild(oXmlRec)
					oXmlFld = oXmlDoc.CreateElement("SiteParameterTypeNM") ' SiteParameterTypeNM
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(SiteParameterType.SiteParameterTypeNM.ExportValue(SiteParameterType.Export, SiteParameterType.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("SiteParameterTypeDS") ' SiteParameterTypeDS
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(SiteParameterType.SiteParameterTypeDS.ExportValue(SiteParameterType.Export, SiteParameterType.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("SiteParameterTypeOrder") ' SiteParameterTypeOrder
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(SiteParameterType.SiteParameterTypeOrder.ExportValue(SiteParameterType.Export, SiteParameterType.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("SiteParameterTemplate") ' SiteParameterTemplate
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(SiteParameterType.SiteParameterTemplate.ExportValue(SiteParameterType.Export, SiteParameterType.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
				Else

					' Vertical format
					If sExportStyle = "v" AndAlso SiteParameterType.Export <> "csv" Then
						ew_Write(ew_ExportField("SiteParameterTypeNM", SiteParameterType.SiteParameterTypeNM.ExportValue(SiteParameterType.Export, SiteParameterType.ExportOriginalValue), SiteParameterType.Export)) ' SiteParameterTypeNM
						ew_Write(ew_ExportField("SiteParameterTypeDS", SiteParameterType.SiteParameterTypeDS.ExportValue(SiteParameterType.Export, SiteParameterType.ExportOriginalValue), SiteParameterType.Export)) ' SiteParameterTypeDS
						ew_Write(ew_ExportField("SiteParameterTypeOrder", SiteParameterType.SiteParameterTypeOrder.ExportValue(SiteParameterType.Export, SiteParameterType.ExportOriginalValue), SiteParameterType.Export)) ' SiteParameterTypeOrder
						ew_Write(ew_ExportField("SiteParameterTemplate", SiteParameterType.SiteParameterTemplate.ExportValue(SiteParameterType.Export, SiteParameterType.ExportOriginalValue), SiteParameterType.Export)) ' SiteParameterTemplate

					' Horizontal format
					Else
						sExportStr = ""
						ew_ExportAddValue(sExportStr, SiteParameterType.SiteParameterTypeNM.ExportValue(SiteParameterType.Export, SiteParameterType.ExportOriginalValue), SiteParameterType.Export) ' SiteParameterTypeNM
						ew_ExportAddValue(sExportStr, SiteParameterType.SiteParameterTypeDS.ExportValue(SiteParameterType.Export, SiteParameterType.ExportOriginalValue), SiteParameterType.Export) ' SiteParameterTypeDS
						ew_ExportAddValue(sExportStr, SiteParameterType.SiteParameterTypeOrder.ExportValue(SiteParameterType.Export, SiteParameterType.ExportOriginalValue), SiteParameterType.Export) ' SiteParameterTypeOrder
						ew_ExportAddValue(sExportStr, SiteParameterType.SiteParameterTemplate.ExportValue(SiteParameterType.Export, SiteParameterType.ExportOriginalValue), SiteParameterType.Export) ' SiteParameterTemplate
						ew_Write(ew_ExportLine(sExportStr, SiteParameterType.Export))
					End If
				End If
			End If
		Loop

		' Close recordset
		Rs.Close()
		Rs.Dispose()
		If SiteParameterType.Export = "xml" Then
			ew_Write("<?xml version=""1.0"" standalone=""yes""?>" & vbcrlf)
			ew_Write(oXmlDoc.OuterXml)
		Else
			ew_Write(ew_ExportFooter(SiteParameterType.Export))
		End If
	End Sub

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "SiteParameterType"
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

	' Write Audit Trail (add page)
	Sub WriteAuditTrailOnAdd(ByRef RsSrc As OrderedDictionary)
		Dim table As String = "SiteParameterType"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsSrc("SiteParameterTypeID")

		' Write Audit Trail
		Dim filePfx As String = "log"
		Dim action As String = "A"
		Dim oldvalue As Object = ""
		Dim curDate As String, curTime As String, id As String, user As String, field As String
		Dim keyvalue As Object
		Dim dt As DateTime = Now()
		curDate = dt.ToString("yyyy/MM/dd")
		curTime = dt.ToString("HH:mm:ss")
		id = HttpContext.Current.Request.ServerVariables("SCRIPT_NAME")
    user = CurrentUserName()
		keyvalue = sKey

		' SiteParameterTypeNM Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteParameterTypeNM", keyvalue, oldvalue, RsSrc("SiteParameterTypeNM"))

		' SiteParameterTypeDS Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteParameterTypeDS", keyvalue, oldvalue, RsSrc("SiteParameterTypeDS"))

		' SiteParameterTypeOrder Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteParameterTypeOrder", keyvalue, oldvalue, RsSrc("SiteParameterTypeOrder"))
	End Sub

	' Write Audit Trail (edit page)
	Sub WriteAuditTrailOnEdit(ByRef RsOld As OrderedDictionary, ByRef RsNew As OrderedDictionary)
		Dim table As String = "SiteParameterType"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsOld("SiteParameterTypeID")

		' Write Audit Trail
		Dim filePfx As String = "log"
		Dim action As String = "U"
		Dim curDate As String, curTime As String, id As String, user As String, field As String
		Dim keyvalue As Object, oldvalue As Object, newvalue As Object
		Dim dt As DateTime = Now()
		Dim fld As cField, fldname As String, Modified As Boolean
		curDate = dt.ToString("yyyy/MM/dd")
		curTime = dt.ToString("HH:mm:ss")
		id = HttpContext.Current.Request.ServerVariables("SCRIPT_NAME")
    user = CurrentUserName()
		Dim arFldNames(RsNew.Count - 1) As String
		RsNew.Keys.CopyTo(arFldNames, 0)
		For Each fldname In arFldNames
			fld = SiteParameterType.FieldByName(fldname)
			If fld IsNot Nothing AndAlso fld.FldDataType <> EW_DATATYPE_BLOB AndAlso fld.FldDataType <> EW_DATATYPE_MEMO Then ' Ignore Blob/Memo Field
				oldvalue = RsOld(fldname)
				newvalue = RsNew(fldname)

				'oldvalue = ew_Conv(oldvalue, fld.FldType)
				'newvalue = ew_Conv(newvalue, fld.FldType)

				If fld.FldDataType = EW_DATATYPE_DATE Then ' DateTime Field
					Modified = Not ew_SameStr(ew_FormatDateTime(oldvalue, 8), ew_FormatDateTime(newvalue, 8))
				Else
					Modified = Not ew_CompareValue(oldvalue, newvalue)
				End If				
				If Modified Then					
					keyvalue = sKey
					ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, fldname, keyvalue, oldvalue, newvalue)
				End If
			End If
		Next
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
		SiteParameterType_list = New cSiteParameterType_list(Me)		
		SiteParameterType_list.Page_Init()

		' Page main processing
		SiteParameterType_list.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If SiteParameterType_list IsNot Nothing Then SiteParameterType_list.Dispose()
	End Sub
End Class