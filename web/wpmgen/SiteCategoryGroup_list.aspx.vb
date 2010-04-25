Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class SiteCategoryGroup_list
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public SiteCategoryGroup_list As cSiteCategoryGroup_list

	'
	' Page Class
	'
	Class cSiteCategoryGroup_list
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
				If SiteCategoryGroup.UseTokenInUrl Then Url = Url & "t=" & SiteCategoryGroup.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If SiteCategoryGroup.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (SiteCategoryGroup.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (SiteCategoryGroup.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' SiteCategoryGroup
		Public Property SiteCategoryGroup() As cSiteCategoryGroup
			Get				
				Return ParentPage.SiteCategoryGroup
			End Get
			Set(ByVal v As cSiteCategoryGroup)
				ParentPage.SiteCategoryGroup = v	
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
			m_PageObjName = "SiteCategoryGroup_list"
			m_PageObjTypeName = "cSiteCategoryGroup_list"

			' Table Name
			m_TableName = "SiteCategoryGroup"

			' Initialize table object
			SiteCategoryGroup = New cSiteCategoryGroup(Me)

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
			SiteCategoryGroup.Export = ew_Get("export") ' Get export parameter
			ParentPage.gsExport = SiteCategoryGroup.Export ' Get export parameter, used in header
			ParentPage.gsExportFile = SiteCategoryGroup.TableVar ' Get export file, used in header
			If SiteCategoryGroup.Export = "excel" Then
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
			SiteCategoryGroup.Dispose()
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
					SiteCategoryGroup.CurrentAction = ew_Get("a")

					' Clear inline mode
					If SiteCategoryGroup.CurrentAction = "cancel" Then
						ClearInlineMode()
					End If

					' Switch to grid edit mode
					If SiteCategoryGroup.CurrentAction = "gridedit" Then
						GridEditMode()
					End If

					' Switch to grid add mode
					If SiteCategoryGroup.CurrentAction = "gridadd" Then
						GridAddMode()
					End If
				End If
			Else
				SiteCategoryGroup.CurrentAction = ObjForm.GetValue("a_list") ' Get action

				' Grid Update
				If SiteCategoryGroup.CurrentAction = "gridupdate" AndAlso ew_SameStr(ew_Session(EW_SESSION_INLINE_MODE), "gridedit") Then
					GridUpdate()
				End If

				' Grid Insert
				If SiteCategoryGroup.CurrentAction = "gridinsert" AndAlso ew_SameStr(ew_Session(EW_SESSION_INLINE_MODE), "gridadd") Then
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
		If (SiteCategoryGroup.RecordsPerPage = -1 OrElse SiteCategoryGroup.RecordsPerPage > 0) Then
			lDisplayRecs = SiteCategoryGroup.RecordsPerPage ' Restore from Session
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
		SiteCategoryGroup.Recordset_Searching(sSrchWhere)

		' Save search criteria
		If sSrchWhere <> "" Then
			If sSrchBasic = "" Then ResetBasicSearchParms()
			If sSrchAdvanced = "" Then ResetAdvancedSearchParms()
			SiteCategoryGroup.SearchWhere = sSrchWhere ' Save to Session
			lStartRec = 1 ' Reset start record counter
			SiteCategoryGroup.StartRecordNumber = lStartRec
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
		SiteCategoryGroup.SessionWhere = sFilter
		SiteCategoryGroup.CurrentFilter = ""

		' Export Data only
		If SiteCategoryGroup.Export = "html" OrElse SiteCategoryGroup.Export = "csv" OrElse SiteCategoryGroup.Export = "word" OrElse SiteCategoryGroup.Export = "excel" OrElse SiteCategoryGroup.Export = "xml" Then
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
			SiteCategoryGroup.RecordsPerPage = lDisplayRecs ' Save to Session

			' Reset start position
			lStartRec = 1
			SiteCategoryGroup.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	'  Exit out of inline mode
	'
	Sub ClearInlineMode()
		SiteCategoryGroup.CurrentAction = "" ' Clear action
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
		SiteCategoryGroup.CurrentFilter = BuildKeyFilter()
		sSql = SiteCategoryGroup.SQL
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
					SiteCategoryGroup.SendEmail = False ' Do not send email on update success
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
			SiteCategoryGroup.EventCancelled = True ' Set event cancelled
			SiteCategoryGroup.CurrentAction = "gridedit" ' Stay in gridedit mode
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
				sFilter = SiteCategoryGroup.KeyFilter
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
			SiteCategoryGroup.SiteCategoryGroupID.FormValue = arKeyFlds(0)
			If Not IsNumeric(SiteCategoryGroup.SiteCategoryGroupID.FormValue) Then	Return False
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
				SiteCategoryGroup.SendEmail = False ' Do not send email on insert success

				' Validate Form
				If Not ValidateForm() Then
					bGridInsert = False ' Form error, reset action
					Message = ParentPage.gsFormError
				Else
					bGridInsert = AddRow() ' Insert this row
				End If
				If bGridInsert Then
					If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
					sKey = sKey & SiteCategoryGroup.SiteCategoryGroupID.CurrentValue

					' Add filter for this record
					sFilter = SiteCategoryGroup.KeyFilter
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
			SiteCategoryGroup.CurrentFilter = sWrkFilter
			sSql = SiteCategoryGroup.SQL
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
			SiteCategoryGroup.EventCancelled = True ' Set event cancelled
			SiteCategoryGroup.CurrentAction = "gridadd" ' Stay in gridadd mode
		End If
	End Sub

	Function EmptyRow() As Boolean
		Dim Empty As Boolean = True
		If Empty Then Empty = ew_SameStr(SiteCategoryGroup.SiteCategoryGroupNM.CurrentValue, SiteCategoryGroup.SiteCategoryGroupNM.OldValue)
		If Empty Then Empty = ew_SameStr(SiteCategoryGroup.SiteCategoryGroupDS.CurrentValue, SiteCategoryGroup.SiteCategoryGroupDS.OldValue)
		If Empty Then Empty = ew_SameStr(SiteCategoryGroup.SiteCategoryGroupOrder.CurrentValue, SiteCategoryGroup.SiteCategoryGroupOrder.OldValue)
		Return Empty
	End Function

	'
	' Restore form values for current row
	'
	Sub RestoreCurrentRowFormValues(idx As Integer)
		Dim sKey As String, arKeyFlds() As String

		' Get row based on current index
		ObjForm.Index = idx
		If SiteCategoryGroup.CurrentAction = "gridadd" Then
			LoadFormValues() ' Load form values
		End If
		If SiteCategoryGroup.CurrentAction = "gridedit" Then
			sKey = ObjForm.GetValue("k_key")
			arKeyFlds = sKey.Split(New Char() {Convert.ToChar(EW_COMPOSITE_KEY_SEPARATOR)})
			If arKeyFlds.Length >= 1 Then
				If ew_SameStr(arKeyFlds(0), SiteCategoryGroup.SiteCategoryGroupID.CurrentValue) Then
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
		BuildSearchSql(sWhere, SiteCategoryGroup.SiteCategoryGroupID, False) ' SiteCategoryGroupID
		BuildSearchSql(sWhere, SiteCategoryGroup.SiteCategoryGroupNM, False) ' SiteCategoryGroupNM
		BuildSearchSql(sWhere, SiteCategoryGroup.SiteCategoryGroupDS, False) ' SiteCategoryGroupDS
		BuildSearchSql(sWhere, SiteCategoryGroup.SiteCategoryGroupOrder, False) ' SiteCategoryGroupOrder

		' Set up search parm
		If sWhere <> "" Then
			SetSearchParm(SiteCategoryGroup.SiteCategoryGroupID) ' SiteCategoryGroupID
			SetSearchParm(SiteCategoryGroup.SiteCategoryGroupNM) ' SiteCategoryGroupNM
			SetSearchParm(SiteCategoryGroup.SiteCategoryGroupDS) ' SiteCategoryGroupDS
			SetSearchParm(SiteCategoryGroup.SiteCategoryGroupOrder) ' SiteCategoryGroupOrder
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
		SiteCategoryGroup.SetAdvancedSearch("x_" & FldParm, Fld.AdvancedSearch.SearchValue)
		SiteCategoryGroup.SetAdvancedSearch("z_" & FldParm, Fld.AdvancedSearch.SearchOperator)
		SiteCategoryGroup.SetAdvancedSearch("v_" & FldParm, Fld.AdvancedSearch.SearchCondition)
		SiteCategoryGroup.SetAdvancedSearch("y_" & FldParm, Fld.AdvancedSearch.SearchValue2)
		SiteCategoryGroup.SetAdvancedSearch("w_" & FldParm, Fld.AdvancedSearch.SearchOperator2)
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
		sSql = sSql & "[SiteCategoryGroupNM] LIKE '%" & sKeyword & "%' OR "
		sSql = sSql & "[SiteCategoryGroupDS] LIKE '%" & sKeyword & "%' OR "
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
			SiteCategoryGroup.BasicSearchKeyword = sSearchKeyword
			SiteCategoryGroup.BasicSearchType = sSearchType
		End If
		Return sSearchStr
	End Function

	'
	' Clear all search parameters
	'
	Sub ResetSearchParms()

		' Clear search where
		sSrchWhere = ""
		SiteCategoryGroup.SearchWhere = sSrchWhere

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
		SiteCategoryGroup.BasicSearchKeyword = ""
		SiteCategoryGroup.BasicSearchType = ""
	End Sub

	'
	' Clear all advanced search parameters
	'
	Sub ResetAdvancedSearchParms()
		SiteCategoryGroup.SetAdvancedSearch("x_SiteCategoryGroupID", "")
		SiteCategoryGroup.SetAdvancedSearch("x_SiteCategoryGroupNM", "")
		SiteCategoryGroup.SetAdvancedSearch("x_SiteCategoryGroupDS", "")
		SiteCategoryGroup.SetAdvancedSearch("x_SiteCategoryGroupOrder", "")
	End Sub

	'
	' Restore all search parameters
	'
	Sub RestoreSearchParms()
		sSrchWhere = SiteCategoryGroup.SearchWhere

		' Restore advanced search settings
		If ParentPage.gsSearchError = "" Then
			RestoreAdvancedSearchParms()
		End If
	End Sub

	'
	' Restore all advanced search parameters
	'
	Sub RestoreAdvancedSearchParms()
		SiteCategoryGroup.SiteCategoryGroupID.AdvancedSearch.SearchValue = SiteCategoryGroup.GetAdvancedSearch("x_SiteCategoryGroupID")
		SiteCategoryGroup.SiteCategoryGroupNM.AdvancedSearch.SearchValue = SiteCategoryGroup.GetAdvancedSearch("x_SiteCategoryGroupNM")
		SiteCategoryGroup.SiteCategoryGroupDS.AdvancedSearch.SearchValue = SiteCategoryGroup.GetAdvancedSearch("x_SiteCategoryGroupDS")
		SiteCategoryGroup.SiteCategoryGroupOrder.AdvancedSearch.SearchValue = SiteCategoryGroup.GetAdvancedSearch("x_SiteCategoryGroupOrder")
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
			SiteCategoryGroup.CurrentOrder = ew_Get("order")
			SiteCategoryGroup.CurrentOrderType = ew_Get("ordertype")
			SiteCategoryGroup.UpdateSort(SiteCategoryGroup.SiteCategoryGroupID) ' SiteCategoryGroupID
			SiteCategoryGroup.UpdateSort(SiteCategoryGroup.SiteCategoryGroupNM) ' SiteCategoryGroupNM
			SiteCategoryGroup.UpdateSort(SiteCategoryGroup.SiteCategoryGroupDS) ' SiteCategoryGroupDS
			SiteCategoryGroup.UpdateSort(SiteCategoryGroup.SiteCategoryGroupOrder) ' SiteCategoryGroupOrder
			SiteCategoryGroup.StartRecordNumber = 1 ' Reset start position
		End If
	End Sub

	'
	' Load Sort Order parameters
	'
	Sub LoadSortOrder()
		Dim sOrderBy As String
		sOrderBy = SiteCategoryGroup.SessionOrderBy ' Get order by from Session
		If sOrderBy = "" Then
			If SiteCategoryGroup.SqlOrderBy <> "" Then
				sOrderBy = SiteCategoryGroup.SqlOrderBy
				SiteCategoryGroup.SessionOrderBy = sOrderBy
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
				SiteCategoryGroup.SessionOrderBy = sOrderBy
				SiteCategoryGroup.SiteCategoryGroupID.Sort = ""
				SiteCategoryGroup.SiteCategoryGroupNM.Sort = ""
				SiteCategoryGroup.SiteCategoryGroupDS.Sort = ""
				SiteCategoryGroup.SiteCategoryGroupOrder.Sort = ""
			End If

			' Reset start position
			lStartRec = 1
			SiteCategoryGroup.StartRecordNumber = lStartRec
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
				SiteCategoryGroup.StartRecordNumber = lStartRec
			ElseIf ew_Get(EW_TABLE_PAGE_NO) <> "" AndAlso IsNumeric(ew_Get(EW_TABLE_PAGE_NO)) Then
				nPageNo = ew_ConvertToInt(ew_Get(EW_TABLE_PAGE_NO))
				lStartRec = (nPageNo-1)*lDisplayRecs+1
				If lStartRec <= 0 Then
					lStartRec = 1
				ElseIf lStartRec >= ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 Then
					lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1
				End If
				SiteCategoryGroup.StartRecordNumber = lStartRec
			End If
		End If
		lStartRec = SiteCategoryGroup.StartRecordNumber

		' Check if correct start record counter
		If lStartRec <= 0 Then ' Avoid invalid start record counter
			lStartRec = 1 ' Reset start record counter
			SiteCategoryGroup.StartRecordNumber = lStartRec
		ElseIf lStartRec > lTotalRecs Then ' Avoid starting record > total records
			lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to last page first record
			SiteCategoryGroup.StartRecordNumber = lStartRec
		ElseIf (lStartRec-1) Mod lDisplayRecs <> 0 Then
			lStartRec = ((lStartRec-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to page boundary
			SiteCategoryGroup.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	' Load default values
	'
	Sub LoadDefaultValues()
		SiteCategoryGroup.SiteCategoryGroupID.OldValue = SiteCategoryGroup.SiteCategoryGroupID.CurrentValue
		SiteCategoryGroup.SiteCategoryGroupNM.OldValue = SiteCategoryGroup.SiteCategoryGroupNM.CurrentValue
		SiteCategoryGroup.SiteCategoryGroupDS.OldValue = SiteCategoryGroup.SiteCategoryGroupDS.CurrentValue
		SiteCategoryGroup.SiteCategoryGroupOrder.OldValue = SiteCategoryGroup.SiteCategoryGroupOrder.CurrentValue
	End Sub

	'
	'  Load search values for validation
	'
	Sub LoadSearchValues()
		SiteCategoryGroup.SiteCategoryGroupID.AdvancedSearch.SearchValue = ew_Get("x_SiteCategoryGroupID")
    	SiteCategoryGroup.SiteCategoryGroupID.AdvancedSearch.SearchOperator = ew_Get("z_SiteCategoryGroupID")
		SiteCategoryGroup.SiteCategoryGroupNM.AdvancedSearch.SearchValue = ew_Get("x_SiteCategoryGroupNM")
    	SiteCategoryGroup.SiteCategoryGroupNM.AdvancedSearch.SearchOperator = ew_Get("z_SiteCategoryGroupNM")
		SiteCategoryGroup.SiteCategoryGroupDS.AdvancedSearch.SearchValue = ew_Get("x_SiteCategoryGroupDS")
    	SiteCategoryGroup.SiteCategoryGroupDS.AdvancedSearch.SearchOperator = ew_Get("z_SiteCategoryGroupDS")
		SiteCategoryGroup.SiteCategoryGroupOrder.AdvancedSearch.SearchValue = ew_Get("x_SiteCategoryGroupOrder")
    	SiteCategoryGroup.SiteCategoryGroupOrder.AdvancedSearch.SearchOperator = ew_Get("z_SiteCategoryGroupOrder")
	End Sub

	'
	' Load form values
	'
	Sub LoadFormValues()
		SiteCategoryGroup.SiteCategoryGroupID.FormValue = ObjForm.GetValue("x_SiteCategoryGroupID")
		SiteCategoryGroup.SiteCategoryGroupID.OldValue = ObjForm.GetValue("o_SiteCategoryGroupID")
		SiteCategoryGroup.SiteCategoryGroupNM.FormValue = ObjForm.GetValue("x_SiteCategoryGroupNM")
		SiteCategoryGroup.SiteCategoryGroupNM.OldValue = ObjForm.GetValue("o_SiteCategoryGroupNM")
		SiteCategoryGroup.SiteCategoryGroupDS.FormValue = ObjForm.GetValue("x_SiteCategoryGroupDS")
		SiteCategoryGroup.SiteCategoryGroupDS.OldValue = ObjForm.GetValue("o_SiteCategoryGroupDS")
		SiteCategoryGroup.SiteCategoryGroupOrder.FormValue = ObjForm.GetValue("x_SiteCategoryGroupOrder")
		SiteCategoryGroup.SiteCategoryGroupOrder.OldValue = ObjForm.GetValue("o_SiteCategoryGroupOrder")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		SiteCategoryGroup.SiteCategoryGroupID.CurrentValue = SiteCategoryGroup.SiteCategoryGroupID.FormValue
		SiteCategoryGroup.SiteCategoryGroupNM.CurrentValue = SiteCategoryGroup.SiteCategoryGroupNM.FormValue
		SiteCategoryGroup.SiteCategoryGroupDS.CurrentValue = SiteCategoryGroup.SiteCategoryGroupDS.FormValue
		SiteCategoryGroup.SiteCategoryGroupOrder.CurrentValue = SiteCategoryGroup.SiteCategoryGroupOrder.FormValue
	End Sub

	'
	' Load recordset
	'
	Function LoadRecordset() As OleDbDataReader

		' Recordset Selecting event
		SiteCategoryGroup.Recordset_Selecting(SiteCategoryGroup.CurrentFilter)

		' Load list page SQL
		Dim sSql As String = SiteCategoryGroup.ListSQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)

		' Count
		lTotalRecs = -1		
		Try			
			If sSql.StartsWith("SELECT * FROM ", StringComparison.InvariantCultureIgnoreCase) AndAlso _				
				ew_Empty(SiteCategoryGroup.SqlGroupBy) AndAlso _
				ew_Empty(SiteCategoryGroup.SqlHaving) Then
				Dim sCntSql As String = SiteCategoryGroup.SelectCountSQL

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
		SiteCategoryGroup.Recordset_Selected(Rs)
		Return Rs
	End Function

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = SiteCategoryGroup.KeyFilter

		' Row Selecting event
		SiteCategoryGroup.Row_Selecting(sFilter)

		' Load SQL based on filter
		SiteCategoryGroup.CurrentFilter = sFilter
		Dim sSql As String = SiteCategoryGroup.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				SiteCategoryGroup.Row_Selected(RsRow)
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
		SiteCategoryGroup.SiteCategoryGroupID.DbValue = RsRow("SiteCategoryGroupID")
		SiteCategoryGroup.SiteCategoryGroupNM.DbValue = RsRow("SiteCategoryGroupNM")
		SiteCategoryGroup.SiteCategoryGroupDS.DbValue = RsRow("SiteCategoryGroupDS")
		SiteCategoryGroup.SiteCategoryGroupOrder.DbValue = RsRow("SiteCategoryGroupOrder")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		SiteCategoryGroup.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' SiteCategoryGroupID

		SiteCategoryGroup.SiteCategoryGroupID.CellCssStyle = ""
		SiteCategoryGroup.SiteCategoryGroupID.CellCssClass = ""

		' SiteCategoryGroupNM
		SiteCategoryGroup.SiteCategoryGroupNM.CellCssStyle = ""
		SiteCategoryGroup.SiteCategoryGroupNM.CellCssClass = ""

		' SiteCategoryGroupDS
		SiteCategoryGroup.SiteCategoryGroupDS.CellCssStyle = ""
		SiteCategoryGroup.SiteCategoryGroupDS.CellCssClass = ""

		' SiteCategoryGroupOrder
		SiteCategoryGroup.SiteCategoryGroupOrder.CellCssStyle = ""
		SiteCategoryGroup.SiteCategoryGroupOrder.CellCssClass = ""

		'
		'  View  Row
		'

		If SiteCategoryGroup.RowType = EW_ROWTYPE_VIEW Then ' View row

			' SiteCategoryGroupID
			SiteCategoryGroup.SiteCategoryGroupID.ViewValue = SiteCategoryGroup.SiteCategoryGroupID.CurrentValue
			SiteCategoryGroup.SiteCategoryGroupID.CssStyle = ""
			SiteCategoryGroup.SiteCategoryGroupID.CssClass = ""
			SiteCategoryGroup.SiteCategoryGroupID.ViewCustomAttributes = ""

			' SiteCategoryGroupNM
			SiteCategoryGroup.SiteCategoryGroupNM.ViewValue = SiteCategoryGroup.SiteCategoryGroupNM.CurrentValue
			SiteCategoryGroup.SiteCategoryGroupNM.CssStyle = ""
			SiteCategoryGroup.SiteCategoryGroupNM.CssClass = ""
			SiteCategoryGroup.SiteCategoryGroupNM.ViewCustomAttributes = ""

			' SiteCategoryGroupDS
			SiteCategoryGroup.SiteCategoryGroupDS.ViewValue = SiteCategoryGroup.SiteCategoryGroupDS.CurrentValue
			SiteCategoryGroup.SiteCategoryGroupDS.CssStyle = ""
			SiteCategoryGroup.SiteCategoryGroupDS.CssClass = ""
			SiteCategoryGroup.SiteCategoryGroupDS.ViewCustomAttributes = ""

			' SiteCategoryGroupOrder
			SiteCategoryGroup.SiteCategoryGroupOrder.ViewValue = SiteCategoryGroup.SiteCategoryGroupOrder.CurrentValue
			SiteCategoryGroup.SiteCategoryGroupOrder.CssStyle = ""
			SiteCategoryGroup.SiteCategoryGroupOrder.CssClass = ""
			SiteCategoryGroup.SiteCategoryGroupOrder.ViewCustomAttributes = ""

			' View refer script
			' SiteCategoryGroupID

			SiteCategoryGroup.SiteCategoryGroupID.HrefValue = ""

			' SiteCategoryGroupNM
			SiteCategoryGroup.SiteCategoryGroupNM.HrefValue = ""

			' SiteCategoryGroupDS
			SiteCategoryGroup.SiteCategoryGroupDS.HrefValue = ""

			' SiteCategoryGroupOrder
			SiteCategoryGroup.SiteCategoryGroupOrder.HrefValue = ""

		'
		'  Add Row
		'

		ElseIf SiteCategoryGroup.RowType = EW_ROWTYPE_ADD Then ' Add row

			' SiteCategoryGroupID
			' SiteCategoryGroupNM

			SiteCategoryGroup.SiteCategoryGroupNM.EditCustomAttributes = ""
			SiteCategoryGroup.SiteCategoryGroupNM.EditValue = ew_HtmlEncode(SiteCategoryGroup.SiteCategoryGroupNM.CurrentValue)

			' SiteCategoryGroupDS
			SiteCategoryGroup.SiteCategoryGroupDS.EditCustomAttributes = ""
			SiteCategoryGroup.SiteCategoryGroupDS.EditValue = ew_HtmlEncode(SiteCategoryGroup.SiteCategoryGroupDS.CurrentValue)

			' SiteCategoryGroupOrder
			SiteCategoryGroup.SiteCategoryGroupOrder.EditCustomAttributes = ""
			SiteCategoryGroup.SiteCategoryGroupOrder.EditValue = ew_HtmlEncode(SiteCategoryGroup.SiteCategoryGroupOrder.CurrentValue)

		'
		'  Edit Row
		'

		ElseIf SiteCategoryGroup.RowType = EW_ROWTYPE_EDIT Then ' Edit row

			' SiteCategoryGroupID
			SiteCategoryGroup.SiteCategoryGroupID.EditCustomAttributes = ""
			SiteCategoryGroup.SiteCategoryGroupID.EditValue = SiteCategoryGroup.SiteCategoryGroupID.CurrentValue
			SiteCategoryGroup.SiteCategoryGroupID.CssStyle = ""
			SiteCategoryGroup.SiteCategoryGroupID.CssClass = ""
			SiteCategoryGroup.SiteCategoryGroupID.ViewCustomAttributes = ""

			' SiteCategoryGroupNM
			SiteCategoryGroup.SiteCategoryGroupNM.EditCustomAttributes = ""
			SiteCategoryGroup.SiteCategoryGroupNM.EditValue = ew_HtmlEncode(SiteCategoryGroup.SiteCategoryGroupNM.CurrentValue)

			' SiteCategoryGroupDS
			SiteCategoryGroup.SiteCategoryGroupDS.EditCustomAttributes = ""
			SiteCategoryGroup.SiteCategoryGroupDS.EditValue = ew_HtmlEncode(SiteCategoryGroup.SiteCategoryGroupDS.CurrentValue)

			' SiteCategoryGroupOrder
			SiteCategoryGroup.SiteCategoryGroupOrder.EditCustomAttributes = ""
			SiteCategoryGroup.SiteCategoryGroupOrder.EditValue = ew_HtmlEncode(SiteCategoryGroup.SiteCategoryGroupOrder.CurrentValue)

			' Edit refer script
			' SiteCategoryGroupID

			SiteCategoryGroup.SiteCategoryGroupID.HrefValue = ""

			' SiteCategoryGroupNM
			SiteCategoryGroup.SiteCategoryGroupNM.HrefValue = ""

			' SiteCategoryGroupDS
			SiteCategoryGroup.SiteCategoryGroupDS.HrefValue = ""

			' SiteCategoryGroupOrder
			SiteCategoryGroup.SiteCategoryGroupOrder.HrefValue = ""
		End If

		' Row Rendered event
		SiteCategoryGroup.Row_Rendered()
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
		If Not ew_CheckInteger(SiteCategoryGroup.SiteCategoryGroupOrder.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Incorrect integer - Site Category Group Order"
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
		sFilter = SiteCategoryGroup.KeyFilter
		SiteCategoryGroup.CurrentFilter  = sFilter
		sSql = SiteCategoryGroup.SQL
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

			' SiteCategoryGroupID
			' SiteCategoryGroupNM

			SiteCategoryGroup.SiteCategoryGroupNM.SetDbValue(SiteCategoryGroup.SiteCategoryGroupNM.CurrentValue, System.DBNull.Value)
			Rs("SiteCategoryGroupNM") = SiteCategoryGroup.SiteCategoryGroupNM.DbValue

			' SiteCategoryGroupDS
			SiteCategoryGroup.SiteCategoryGroupDS.SetDbValue(SiteCategoryGroup.SiteCategoryGroupDS.CurrentValue, System.DBNull.Value)
			Rs("SiteCategoryGroupDS") = SiteCategoryGroup.SiteCategoryGroupDS.DbValue

			' SiteCategoryGroupOrder
			SiteCategoryGroup.SiteCategoryGroupOrder.SetDbValue(SiteCategoryGroup.SiteCategoryGroupOrder.CurrentValue, System.DBNull.Value)
			Rs("SiteCategoryGroupOrder") = SiteCategoryGroup.SiteCategoryGroupOrder.DbValue

			' Row Updating event
			bUpdateRow = SiteCategoryGroup.Row_Updating(RsOld, Rs)
			If bUpdateRow Then
				Try
					SiteCategoryGroup.Update(Rs)
					EditRow = True
				Catch e As Exception
					If EW_DEBUG_ENABLED Then Throw
					Message = e.Message
					EditRow = False
				End Try
			Else
				If SiteCategoryGroup.CancelMessage <> "" Then
					Message = SiteCategoryGroup.CancelMessage
					SiteCategoryGroup.CancelMessage = ""
				Else
					Message = "Update cancelled"
				End If
				EditRow = False
			End If
		End If

		' Row Updated event
		If EditRow Then
			SiteCategoryGroup.Row_Updated(RsOld, Rs)
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

		' SiteCategoryGroupID
		' SiteCategoryGroupNM

		SiteCategoryGroup.SiteCategoryGroupNM.SetDbValue(SiteCategoryGroup.SiteCategoryGroupNM.CurrentValue, System.DBNull.Value)
		Rs("SiteCategoryGroupNM") = SiteCategoryGroup.SiteCategoryGroupNM.DbValue

		' SiteCategoryGroupDS
		SiteCategoryGroup.SiteCategoryGroupDS.SetDbValue(SiteCategoryGroup.SiteCategoryGroupDS.CurrentValue, System.DBNull.Value)
		Rs("SiteCategoryGroupDS") = SiteCategoryGroup.SiteCategoryGroupDS.DbValue

		' SiteCategoryGroupOrder
		SiteCategoryGroup.SiteCategoryGroupOrder.SetDbValue(SiteCategoryGroup.SiteCategoryGroupOrder.CurrentValue, System.DBNull.Value)
		Rs("SiteCategoryGroupOrder") = SiteCategoryGroup.SiteCategoryGroupOrder.DbValue

		' Row Inserting event
		bInsertRow = SiteCategoryGroup.Row_Inserting(Rs)
		If bInsertRow Then
			Try	
				SiteCategoryGroup.Insert(Rs)
				AddRow = True
			Catch e As Exception
				If EW_DEBUG_ENABLED Then Throw
				Message = e.Message				
				AddRow = False
			End Try
		Else
			If SiteCategoryGroup.CancelMessage <> "" Then
				Message = SiteCategoryGroup.CancelMessage
				SiteCategoryGroup.CancelMessage = ""
			Else
				Message = "Insert cancelled"
			End If
			AddRow = False
		End If
		If AddRow Then
			LastInsertId = Conn.GetLastInsertId()
			SiteCategoryGroup.SiteCategoryGroupID.DbValue = LastInsertId
			Rs("SiteCategoryGroupID") = SiteCategoryGroup.SiteCategoryGroupID.DbValue		

			' Row Inserted event
			SiteCategoryGroup.Row_Inserted(Rs)
			WriteAuditTrailOnAdd(Rs)
		End If
	End Function

	'
	' Load advanced search
	'
	Sub LoadAdvancedSearch()
		SiteCategoryGroup.SiteCategoryGroupID.AdvancedSearch.SearchValue = SiteCategoryGroup.GetAdvancedSearch("x_SiteCategoryGroupID")
		SiteCategoryGroup.SiteCategoryGroupNM.AdvancedSearch.SearchValue = SiteCategoryGroup.GetAdvancedSearch("x_SiteCategoryGroupNM")
		SiteCategoryGroup.SiteCategoryGroupDS.AdvancedSearch.SearchValue = SiteCategoryGroup.GetAdvancedSearch("x_SiteCategoryGroupDS")
		SiteCategoryGroup.SiteCategoryGroupOrder.AdvancedSearch.SearchValue = SiteCategoryGroup.GetAdvancedSearch("x_SiteCategoryGroupOrder")
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
		If SiteCategoryGroup.ExportAll Then
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
		If SiteCategoryGroup.Export = "xml" Then
			oXmlDoc = New XmlDocument()
			oXmlTbl = oXmlDoc.CreateElement("table")
			oXmlDoc.AppendChild(oXmlTbl)
		Else
			ew_Write(ew_ExportHeader(SiteCategoryGroup.Export))

			' Horizontal format, write header
			If sExportStyle <> "v" OrElse SiteCategoryGroup.Export = "csv" Then
				sExportStr = ""
				ew_ExportAddValue(sExportStr, "SiteCategoryGroupID", SiteCategoryGroup.Export)
				ew_ExportAddValue(sExportStr, "SiteCategoryGroupNM", SiteCategoryGroup.Export)
				ew_ExportAddValue(sExportStr, "SiteCategoryGroupDS", SiteCategoryGroup.Export)
				ew_ExportAddValue(sExportStr, "SiteCategoryGroupOrder", SiteCategoryGroup.Export)
				ew_Write(ew_ExportLine(sExportStr, SiteCategoryGroup.Export))
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
				SiteCategoryGroup.RowType = EW_ROWTYPE_VIEW ' Render view
				RenderRow()
				If SiteCategoryGroup.Export = "xml" Then
					oXmlRec = oXmlDoc.CreateElement("record")
					oXmlTbl.AppendChild(oXmlRec)
					oXmlFld = oXmlDoc.CreateElement("SiteCategoryGroupID") ' SiteCategoryGroupID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(SiteCategoryGroup.SiteCategoryGroupID.ExportValue(SiteCategoryGroup.Export, SiteCategoryGroup.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("SiteCategoryGroupNM") ' SiteCategoryGroupNM
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(SiteCategoryGroup.SiteCategoryGroupNM.ExportValue(SiteCategoryGroup.Export, SiteCategoryGroup.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("SiteCategoryGroupDS") ' SiteCategoryGroupDS
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(SiteCategoryGroup.SiteCategoryGroupDS.ExportValue(SiteCategoryGroup.Export, SiteCategoryGroup.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("SiteCategoryGroupOrder") ' SiteCategoryGroupOrder
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(SiteCategoryGroup.SiteCategoryGroupOrder.ExportValue(SiteCategoryGroup.Export, SiteCategoryGroup.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
				Else

					' Vertical format
					If sExportStyle = "v" AndAlso SiteCategoryGroup.Export <> "csv" Then
						ew_Write(ew_ExportField("SiteCategoryGroupID", SiteCategoryGroup.SiteCategoryGroupID.ExportValue(SiteCategoryGroup.Export, SiteCategoryGroup.ExportOriginalValue), SiteCategoryGroup.Export)) ' SiteCategoryGroupID
						ew_Write(ew_ExportField("SiteCategoryGroupNM", SiteCategoryGroup.SiteCategoryGroupNM.ExportValue(SiteCategoryGroup.Export, SiteCategoryGroup.ExportOriginalValue), SiteCategoryGroup.Export)) ' SiteCategoryGroupNM
						ew_Write(ew_ExportField("SiteCategoryGroupDS", SiteCategoryGroup.SiteCategoryGroupDS.ExportValue(SiteCategoryGroup.Export, SiteCategoryGroup.ExportOriginalValue), SiteCategoryGroup.Export)) ' SiteCategoryGroupDS
						ew_Write(ew_ExportField("SiteCategoryGroupOrder", SiteCategoryGroup.SiteCategoryGroupOrder.ExportValue(SiteCategoryGroup.Export, SiteCategoryGroup.ExportOriginalValue), SiteCategoryGroup.Export)) ' SiteCategoryGroupOrder

					' Horizontal format
					Else
						sExportStr = ""
						ew_ExportAddValue(sExportStr, SiteCategoryGroup.SiteCategoryGroupID.ExportValue(SiteCategoryGroup.Export, SiteCategoryGroup.ExportOriginalValue), SiteCategoryGroup.Export) ' SiteCategoryGroupID
						ew_ExportAddValue(sExportStr, SiteCategoryGroup.SiteCategoryGroupNM.ExportValue(SiteCategoryGroup.Export, SiteCategoryGroup.ExportOriginalValue), SiteCategoryGroup.Export) ' SiteCategoryGroupNM
						ew_ExportAddValue(sExportStr, SiteCategoryGroup.SiteCategoryGroupDS.ExportValue(SiteCategoryGroup.Export, SiteCategoryGroup.ExportOriginalValue), SiteCategoryGroup.Export) ' SiteCategoryGroupDS
						ew_ExportAddValue(sExportStr, SiteCategoryGroup.SiteCategoryGroupOrder.ExportValue(SiteCategoryGroup.Export, SiteCategoryGroup.ExportOriginalValue), SiteCategoryGroup.Export) ' SiteCategoryGroupOrder
						ew_Write(ew_ExportLine(sExportStr, SiteCategoryGroup.Export))
					End If
				End If
			End If
		Loop

		' Close recordset
		Rs.Close()
		Rs.Dispose()
		If SiteCategoryGroup.Export = "xml" Then
			ew_Write("<?xml version=""1.0"" standalone=""yes""?>" & vbcrlf)
			ew_Write(oXmlDoc.OuterXml)
		Else
			ew_Write(ew_ExportFooter(SiteCategoryGroup.Export))
		End If
	End Sub

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "SiteCategoryGroup"
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
		Dim table As String = "SiteCategoryGroup"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsSrc("SiteCategoryGroupID")

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

		' SiteCategoryGroupID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteCategoryGroupID", keyvalue, oldvalue, RsSrc("SiteCategoryGroupID"))

		' SiteCategoryGroupNM Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteCategoryGroupNM", keyvalue, oldvalue, RsSrc("SiteCategoryGroupNM"))

		' SiteCategoryGroupDS Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteCategoryGroupDS", keyvalue, oldvalue, RsSrc("SiteCategoryGroupDS"))

		' SiteCategoryGroupOrder Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteCategoryGroupOrder", keyvalue, oldvalue, RsSrc("SiteCategoryGroupOrder"))
	End Sub

	' Write Audit Trail (edit page)
	Sub WriteAuditTrailOnEdit(ByRef RsOld As OrderedDictionary, ByRef RsNew As OrderedDictionary)
		Dim table As String = "SiteCategoryGroup"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsOld("SiteCategoryGroupID")

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
			fld = SiteCategoryGroup.FieldByName(fldname)
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
		SiteCategoryGroup_list = New cSiteCategoryGroup_list(Me)		
		SiteCategoryGroup_list.Page_Init()

		' Page main processing
		SiteCategoryGroup_list.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If SiteCategoryGroup_list IsNot Nothing Then SiteCategoryGroup_list.Dispose()
	End Sub
End Class
