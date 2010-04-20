Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class PageAlias_list
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public PageAlias_list As cPageAlias_list

	'
	' Page Class
	'
	Class cPageAlias_list
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
				If PageAlias.UseTokenInUrl Then Url = Url & "t=" & PageAlias.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If PageAlias.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (PageAlias.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (PageAlias.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' PageAlias
		Public Property PageAlias() As cPageAlias
			Get				
				Return ParentPage.PageAlias
			End Get
			Set(ByVal v As cPageAlias)
				ParentPage.PageAlias = v	
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
			m_PageObjName = "PageAlias_list"
			m_PageObjTypeName = "cPageAlias_list"

			' Table Name
			m_TableName = "PageAlias"

			' Initialize table object
			PageAlias = New cPageAlias(Me)

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
			PageAlias.Export = ew_Get("export") ' Get export parameter
			ParentPage.gsExport = PageAlias.Export ' Get export parameter, used in header
			ParentPage.gsExportFile = PageAlias.TableVar ' Get export file, used in header
			If PageAlias.Export = "excel" Then
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
			PageAlias.Dispose()
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
					PageAlias.CurrentAction = ew_Get("a")

					' Clear inline mode
					If PageAlias.CurrentAction = "cancel" Then
						ClearInlineMode()
					End If

					' Switch to grid edit mode
					If PageAlias.CurrentAction = "gridedit" Then
						GridEditMode()
					End If

					' Switch to inline edit mode
					If PageAlias.CurrentAction = "edit" Then
						InlineEditMode()
					End If

					' Switch to grid add mode
					If PageAlias.CurrentAction = "gridadd" Then
						GridAddMode()
					End If
				End If
			Else
				PageAlias.CurrentAction = ObjForm.GetValue("a_list") ' Get action

				' Grid Update
				If PageAlias.CurrentAction = "gridupdate" AndAlso ew_SameStr(ew_Session(EW_SESSION_INLINE_MODE), "gridedit") Then
					GridUpdate()
				End If

				' Inline Update
				If PageAlias.CurrentAction = "update" AndAlso ew_SameStr(ew_Session(EW_SESSION_INLINE_MODE), "edit") Then
					InlineUpdate()
				End If

				' Grid Insert
				If PageAlias.CurrentAction = "gridinsert" AndAlso ew_SameStr(ew_Session(EW_SESSION_INLINE_MODE), "gridadd") Then
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
		If (PageAlias.RecordsPerPage = -1 OrElse PageAlias.RecordsPerPage > 0) Then
			lDisplayRecs = PageAlias.RecordsPerPage ' Restore from Session
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
		PageAlias.Recordset_Searching(sSrchWhere)

		' Save search criteria
		If sSrchWhere <> "" Then
			If sSrchBasic = "" Then ResetBasicSearchParms()
			If sSrchAdvanced = "" Then ResetAdvancedSearchParms()
			PageAlias.SearchWhere = sSrchWhere ' Save to Session
			lStartRec = 1 ' Reset start record counter
			PageAlias.StartRecordNumber = lStartRec
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
		PageAlias.SessionWhere = sFilter
		PageAlias.CurrentFilter = ""

		' Export Data only
		If PageAlias.Export = "html" OrElse PageAlias.Export = "csv" OrElse PageAlias.Export = "word" OrElse PageAlias.Export = "excel" OrElse PageAlias.Export = "xml" Then
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
			PageAlias.RecordsPerPage = lDisplayRecs ' Save to Session

			' Reset start position
			lStartRec = 1
			PageAlias.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	'  Exit out of inline mode
	'
	Sub ClearInlineMode()
		PageAlias.SetKey("PageAliasID", "") ' Clear inline edit key
		PageAlias.CurrentAction = "" ' Clear action
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
	' Switch to Inline Edit Mode
	'
	Sub InlineEditMode()
		Dim bInlineEdit As Boolean = True
		If ew_Get("PageAliasID") <> "" Then
			PageAlias.PageAliasID.QueryStringValue = ew_Get("PageAliasID")
		Else
			bInlineEdit = False
		End If
		If bInlineEdit AndAlso LoadRow() Then
			PageAlias.SetKey("PageAliasID", PageAlias.PageAliasID.CurrentValue) ' Set up inline edit key
			ew_Session(EW_SESSION_INLINE_MODE) = "edit" ' Enabled inline edit
		End If
	End Sub

	'
	' Peform update to inline edit record
	'
	Sub InlineUpdate()
		Dim bInlineUpdate As Boolean
		ObjForm.Index = 1
		LoadFormValues() ' Get form values

		' Validate Form
		If Not ValidateForm() Then
			bInlineUpdate = False ' Form error, reset action
			Message = ParentPage.gsFormError
		Else
			bInlineUpdate = False
			If CheckInlineEditKey() Then ' Check key
				PageAlias.SendEmail = True ' Send email on update success
				bInlineUpdate = EditRow() ' Update record
			Else
				bInlineUpdate = False
			End If
		End If
		If bInlineUpdate Then ' Update success
			Message = "Update succeeded" ' Set success message
			ClearInlineMode() ' Clear inline edit mode
		Else
			If Message = "" Then
				Message = "Update failed" ' Set update failed message
			End If
			PageAlias.EventCancelled = True ' Cancel event
			PageAlias.CurrentAction = "edit" ' Stay in edit mode
		End If
	End Sub

	'
	' Check inline edit key
	'
	Function CheckInlineEditKey() As Boolean
		If Not ew_SameStr(PageAlias.GetKey("PageAliasID"), PageAlias.PageAliasID.CurrentValue) Then
			Return False
		End If
		Return True
	End Function

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
		PageAlias.CurrentFilter = BuildKeyFilter()
		sSql = PageAlias.SQL
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
					PageAlias.SendEmail = False ' Do not send email on update success
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
			PageAlias.EventCancelled = True ' Set event cancelled
			PageAlias.CurrentAction = "gridedit" ' Stay in gridedit mode
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
				sFilter = PageAlias.KeyFilter
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
			PageAlias.PageAliasID.FormValue = arKeyFlds(0)
			If Not IsNumeric(PageAlias.PageAliasID.FormValue) Then	Return False
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
				PageAlias.SendEmail = False ' Do not send email on insert success

				' Validate Form
				If Not ValidateForm() Then
					bGridInsert = False ' Form error, reset action
					Message = ParentPage.gsFormError
				Else
					bGridInsert = AddRow() ' Insert this row
				End If
				If bGridInsert Then
					If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
					sKey = sKey & PageAlias.PageAliasID.CurrentValue

					' Add filter for this record
					sFilter = PageAlias.KeyFilter
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
			PageAlias.CurrentFilter = sWrkFilter
			sSql = PageAlias.SQL
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
			PageAlias.EventCancelled = True ' Set event cancelled
			PageAlias.CurrentAction = "gridadd" ' Stay in gridadd mode
		End If
	End Sub

	Function EmptyRow() As Boolean
		Dim Empty As Boolean = True
		If Empty Then Empty = ew_SameStr(PageAlias.zPageURL.CurrentValue, PageAlias.zPageURL.OldValue)
		If Empty Then Empty = ew_SameStr(PageAlias.TargetURL.CurrentValue, PageAlias.TargetURL.OldValue)
		If Empty Then Empty = ew_SameStr(PageAlias.AliasType.CurrentValue, PageAlias.AliasType.OldValue)
		If Empty Then Empty = ew_SameStr(PageAlias.CompanyID.CurrentValue, PageAlias.CompanyID.OldValue)
		Return Empty
	End Function

	'
	' Restore form values for current row
	'
	Sub RestoreCurrentRowFormValues(idx As Integer)
		Dim sKey As String, arKeyFlds() As String

		' Get row based on current index
		ObjForm.Index = idx
		If PageAlias.CurrentAction = "gridadd" Then
			LoadFormValues() ' Load form values
		End If
		If PageAlias.CurrentAction = "gridedit" Then
			sKey = ObjForm.GetValue("k_key")
			arKeyFlds = sKey.Split(New Char() {Convert.ToChar(EW_COMPOSITE_KEY_SEPARATOR)})
			If arKeyFlds.Length >= 1 Then
				If ew_SameStr(arKeyFlds(0), PageAlias.PageAliasID.CurrentValue) Then
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
		BuildSearchSql(sWhere, PageAlias.zPageURL, False) ' PageURL
		BuildSearchSql(sWhere, PageAlias.TargetURL, False) ' TargetURL
		BuildSearchSql(sWhere, PageAlias.AliasType, False) ' AliasType
		BuildSearchSql(sWhere, PageAlias.CompanyID, False) ' CompanyID

		' Set up search parm
		If sWhere <> "" Then
			SetSearchParm(PageAlias.zPageURL) ' PageURL
			SetSearchParm(PageAlias.TargetURL) ' TargetURL
			SetSearchParm(PageAlias.AliasType) ' AliasType
			SetSearchParm(PageAlias.CompanyID) ' CompanyID
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
		PageAlias.SetAdvancedSearch("x_" & FldParm, Fld.AdvancedSearch.SearchValue)
		PageAlias.SetAdvancedSearch("z_" & FldParm, Fld.AdvancedSearch.SearchOperator)
		PageAlias.SetAdvancedSearch("v_" & FldParm, Fld.AdvancedSearch.SearchCondition)
		PageAlias.SetAdvancedSearch("y_" & FldParm, Fld.AdvancedSearch.SearchValue2)
		PageAlias.SetAdvancedSearch("w_" & FldParm, Fld.AdvancedSearch.SearchOperator2)
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
		sSql = sSql & "[PageURL] LIKE '%" & sKeyword & "%' OR "
		sSql = sSql & "[TargetURL] LIKE '%" & sKeyword & "%' OR "
		sSql = sSql & "[AliasType] LIKE '%" & sKeyword & "%' OR "
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
			PageAlias.BasicSearchKeyword = sSearchKeyword
			PageAlias.BasicSearchType = sSearchType
		End If
		Return sSearchStr
	End Function

	'
	' Clear all search parameters
	'
	Sub ResetSearchParms()

		' Clear search where
		sSrchWhere = ""
		PageAlias.SearchWhere = sSrchWhere

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
		PageAlias.BasicSearchKeyword = ""
		PageAlias.BasicSearchType = ""
	End Sub

	'
	' Clear all advanced search parameters
	'
	Sub ResetAdvancedSearchParms()
		PageAlias.SetAdvancedSearch("x_zPageURL", "")
		PageAlias.SetAdvancedSearch("x_TargetURL", "")
		PageAlias.SetAdvancedSearch("x_AliasType", "")
		PageAlias.SetAdvancedSearch("x_CompanyID", "")
	End Sub

	'
	' Restore all search parameters
	'
	Sub RestoreSearchParms()
		sSrchWhere = PageAlias.SearchWhere

		' Restore advanced search settings
		If ParentPage.gsSearchError = "" Then
			RestoreAdvancedSearchParms()
		End If
	End Sub

	'
	' Restore all advanced search parameters
	'
	Sub RestoreAdvancedSearchParms()
		PageAlias.zPageURL.AdvancedSearch.SearchValue = PageAlias.GetAdvancedSearch("x_zPageURL")
		PageAlias.TargetURL.AdvancedSearch.SearchValue = PageAlias.GetAdvancedSearch("x_TargetURL")
		PageAlias.AliasType.AdvancedSearch.SearchValue = PageAlias.GetAdvancedSearch("x_AliasType")
		PageAlias.CompanyID.AdvancedSearch.SearchValue = PageAlias.GetAdvancedSearch("x_CompanyID")
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
			PageAlias.CurrentOrder = ew_Get("order")
			PageAlias.CurrentOrderType = ew_Get("ordertype")
			PageAlias.UpdateSort(PageAlias.zPageURL) ' PageURL
			PageAlias.UpdateSort(PageAlias.TargetURL) ' TargetURL
			PageAlias.UpdateSort(PageAlias.AliasType) ' AliasType
			PageAlias.UpdateSort(PageAlias.CompanyID) ' CompanyID
			PageAlias.StartRecordNumber = 1 ' Reset start position
		End If
	End Sub

	'
	' Load Sort Order parameters
	'
	Sub LoadSortOrder()
		Dim sOrderBy As String
		sOrderBy = PageAlias.SessionOrderBy ' Get order by from Session
		If sOrderBy = "" Then
			If PageAlias.SqlOrderBy <> "" Then
				sOrderBy = PageAlias.SqlOrderBy
				PageAlias.SessionOrderBy = sOrderBy
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
				PageAlias.SessionOrderBy = sOrderBy
				PageAlias.zPageURL.Sort = ""
				PageAlias.TargetURL.Sort = ""
				PageAlias.AliasType.Sort = ""
				PageAlias.CompanyID.Sort = ""
			End If

			' Reset start position
			lStartRec = 1
			PageAlias.StartRecordNumber = lStartRec
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
				PageAlias.StartRecordNumber = lStartRec
			ElseIf ew_Get(EW_TABLE_PAGE_NO) <> "" AndAlso IsNumeric(ew_Get(EW_TABLE_PAGE_NO)) Then
				nPageNo = ew_ConvertToInt(ew_Get(EW_TABLE_PAGE_NO))
				lStartRec = (nPageNo-1)*lDisplayRecs+1
				If lStartRec <= 0 Then
					lStartRec = 1
				ElseIf lStartRec >= ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 Then
					lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1
				End If
				PageAlias.StartRecordNumber = lStartRec
			End If
		End If
		lStartRec = PageAlias.StartRecordNumber

		' Check if correct start record counter
		If lStartRec <= 0 Then ' Avoid invalid start record counter
			lStartRec = 1 ' Reset start record counter
			PageAlias.StartRecordNumber = lStartRec
		ElseIf lStartRec > lTotalRecs Then ' Avoid starting record > total records
			lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to last page first record
			PageAlias.StartRecordNumber = lStartRec
		ElseIf (lStartRec-1) Mod lDisplayRecs <> 0 Then
			lStartRec = ((lStartRec-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to page boundary
			PageAlias.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	' Load default values
	'
	Sub LoadDefaultValues()
		PageAlias.zPageURL.OldValue = PageAlias.zPageURL.CurrentValue
		PageAlias.TargetURL.OldValue = PageAlias.TargetURL.CurrentValue
		PageAlias.AliasType.OldValue = PageAlias.AliasType.CurrentValue
		PageAlias.CompanyID.OldValue = PageAlias.CompanyID.CurrentValue
	End Sub

	'
	'  Load search values for validation
	'
	Sub LoadSearchValues()
		PageAlias.zPageURL.AdvancedSearch.SearchValue = ew_Get("x_zPageURL")
    	PageAlias.zPageURL.AdvancedSearch.SearchOperator = ew_Get("z_zPageURL")
		PageAlias.TargetURL.AdvancedSearch.SearchValue = ew_Get("x_TargetURL")
    	PageAlias.TargetURL.AdvancedSearch.SearchOperator = ew_Get("z_TargetURL")
		PageAlias.AliasType.AdvancedSearch.SearchValue = ew_Get("x_AliasType")
    	PageAlias.AliasType.AdvancedSearch.SearchOperator = ew_Get("z_AliasType")
		PageAlias.CompanyID.AdvancedSearch.SearchValue = ew_Get("x_CompanyID")
    	PageAlias.CompanyID.AdvancedSearch.SearchOperator = ew_Get("z_CompanyID")
	End Sub

	'
	' Load form values
	'
	Sub LoadFormValues()
		PageAlias.zPageURL.FormValue = ObjForm.GetValue("x_zPageURL")
		PageAlias.zPageURL.OldValue = ObjForm.GetValue("o_zPageURL")
		PageAlias.TargetURL.FormValue = ObjForm.GetValue("x_TargetURL")
		PageAlias.TargetURL.OldValue = ObjForm.GetValue("o_TargetURL")
		PageAlias.AliasType.FormValue = ObjForm.GetValue("x_AliasType")
		PageAlias.AliasType.OldValue = ObjForm.GetValue("o_AliasType")
		PageAlias.CompanyID.FormValue = ObjForm.GetValue("x_CompanyID")
		PageAlias.CompanyID.OldValue = ObjForm.GetValue("o_CompanyID")
		PageAlias.PageAliasID.FormValue = ObjForm.GetValue("x_PageAliasID")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		PageAlias.zPageURL.CurrentValue = PageAlias.zPageURL.FormValue
		PageAlias.TargetURL.CurrentValue = PageAlias.TargetURL.FormValue
		PageAlias.AliasType.CurrentValue = PageAlias.AliasType.FormValue
		PageAlias.CompanyID.CurrentValue = PageAlias.CompanyID.FormValue
		PageAlias.PageAliasID.CurrentValue = PageAlias.PageAliasID.FormValue
	End Sub

	'
	' Load recordset
	'
	Function LoadRecordset() As OleDbDataReader

		' Recordset Selecting event
		PageAlias.Recordset_Selecting(PageAlias.CurrentFilter)

		' Load list page SQL
		Dim sSql As String = PageAlias.ListSQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)

		' Count
		lTotalRecs = -1		
		Try			
			If sSql.StartsWith("SELECT * FROM ", StringComparison.InvariantCultureIgnoreCase) AndAlso _				
				ew_Empty(PageAlias.SqlGroupBy) AndAlso _
				ew_Empty(PageAlias.SqlHaving) Then
				Dim sCntSql As String = PageAlias.SelectCountSQL

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
		PageAlias.Recordset_Selected(Rs)
		Return Rs
	End Function

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = PageAlias.KeyFilter

		' Row Selecting event
		PageAlias.Row_Selecting(sFilter)

		' Load SQL based on filter
		PageAlias.CurrentFilter = sFilter
		Dim sSql As String = PageAlias.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				PageAlias.Row_Selected(RsRow)
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
		PageAlias.PageAliasID.DbValue = RsRow("PageAliasID")
		PageAlias.zPageURL.DbValue = RsRow("PageURL")
		PageAlias.TargetURL.DbValue = RsRow("TargetURL")
		PageAlias.AliasType.DbValue = RsRow("AliasType")
		PageAlias.CompanyID.DbValue = RsRow("CompanyID")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		PageAlias.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' PageURL

		PageAlias.zPageURL.CellCssStyle = ""
		PageAlias.zPageURL.CellCssClass = ""

		' TargetURL
		PageAlias.TargetURL.CellCssStyle = ""
		PageAlias.TargetURL.CellCssClass = ""

		' AliasType
		PageAlias.AliasType.CellCssStyle = ""
		PageAlias.AliasType.CellCssClass = ""

		' CompanyID
		PageAlias.CompanyID.CellCssStyle = ""
		PageAlias.CompanyID.CellCssClass = ""

		'
		'  View  Row
		'

		If PageAlias.RowType = EW_ROWTYPE_VIEW Then ' View row

			' PageURL
			PageAlias.zPageURL.ViewValue = PageAlias.zPageURL.CurrentValue
			PageAlias.zPageURL.CssStyle = ""
			PageAlias.zPageURL.CssClass = ""
			PageAlias.zPageURL.ViewCustomAttributes = ""

			' TargetURL
			PageAlias.TargetURL.ViewValue = PageAlias.TargetURL.CurrentValue
			PageAlias.TargetURL.CssStyle = ""
			PageAlias.TargetURL.CssClass = ""
			PageAlias.TargetURL.ViewCustomAttributes = ""

			' AliasType
			PageAlias.AliasType.ViewValue = PageAlias.AliasType.CurrentValue
			PageAlias.AliasType.CssStyle = ""
			PageAlias.AliasType.CssClass = ""
			PageAlias.AliasType.ViewCustomAttributes = ""

			' CompanyID
			If ew_NotEmpty(PageAlias.CompanyID.CurrentValue) Then
				sSqlWrk = "SELECT [CompanyName] FROM [Company] WHERE [CompanyID] = " & ew_AdjustSql(PageAlias.CompanyID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					PageAlias.CompanyID.ViewValue = RsWrk("CompanyName")
				Else
					PageAlias.CompanyID.ViewValue = PageAlias.CompanyID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				PageAlias.CompanyID.ViewValue = System.DBNull.Value
			End If
			PageAlias.CompanyID.CssStyle = ""
			PageAlias.CompanyID.CssClass = ""
			PageAlias.CompanyID.ViewCustomAttributes = ""

			' View refer script
			' PageURL

			PageAlias.zPageURL.HrefValue = ""

			' TargetURL
			PageAlias.TargetURL.HrefValue = ""

			' AliasType
			PageAlias.AliasType.HrefValue = ""

			' CompanyID
			PageAlias.CompanyID.HrefValue = ""

		'
		'  Add Row
		'

		ElseIf PageAlias.RowType = EW_ROWTYPE_ADD Then ' Add row

			' PageURL
			PageAlias.zPageURL.EditCustomAttributes = ""
			PageAlias.zPageURL.EditValue = ew_HtmlEncode(PageAlias.zPageURL.CurrentValue)

			' TargetURL
			PageAlias.TargetURL.EditCustomAttributes = ""
			PageAlias.TargetURL.EditValue = ew_HtmlEncode(PageAlias.TargetURL.CurrentValue)

			' AliasType
			PageAlias.AliasType.EditCustomAttributes = ""
			PageAlias.AliasType.EditValue = ew_HtmlEncode(PageAlias.AliasType.CurrentValue)

			' CompanyID
			PageAlias.CompanyID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [CompanyID], [CompanyName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Company]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			PageAlias.CompanyID.EditValue = arwrk

		'
		'  Edit Row
		'

		ElseIf PageAlias.RowType = EW_ROWTYPE_EDIT Then ' Edit row

			' PageURL
			PageAlias.zPageURL.EditCustomAttributes = ""
			PageAlias.zPageURL.EditValue = ew_HtmlEncode(PageAlias.zPageURL.CurrentValue)

			' TargetURL
			PageAlias.TargetURL.EditCustomAttributes = ""
			PageAlias.TargetURL.EditValue = ew_HtmlEncode(PageAlias.TargetURL.CurrentValue)

			' AliasType
			PageAlias.AliasType.EditCustomAttributes = ""
			PageAlias.AliasType.EditValue = ew_HtmlEncode(PageAlias.AliasType.CurrentValue)

			' CompanyID
			PageAlias.CompanyID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [CompanyID], [CompanyName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Company]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			PageAlias.CompanyID.EditValue = arwrk

			' Edit refer script
			' PageURL

			PageAlias.zPageURL.HrefValue = ""

			' TargetURL
			PageAlias.TargetURL.HrefValue = ""

			' AliasType
			PageAlias.AliasType.HrefValue = ""

			' CompanyID
			PageAlias.CompanyID.HrefValue = ""
		End If

		' Row Rendered event
		PageAlias.Row_Rendered()
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
		sFilter = PageAlias.KeyFilter
		PageAlias.CurrentFilter  = sFilter
		sSql = PageAlias.SQL
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

			' PageURL
			PageAlias.zPageURL.SetDbValue(PageAlias.zPageURL.CurrentValue, System.DBNull.Value)
			Rs("PageURL") = PageAlias.zPageURL.DbValue

			' TargetURL
			PageAlias.TargetURL.SetDbValue(PageAlias.TargetURL.CurrentValue, System.DBNull.Value)
			Rs("TargetURL") = PageAlias.TargetURL.DbValue

			' AliasType
			PageAlias.AliasType.SetDbValue(PageAlias.AliasType.CurrentValue, System.DBNull.Value)
			Rs("AliasType") = PageAlias.AliasType.DbValue

			' CompanyID
			PageAlias.CompanyID.SetDbValue(PageAlias.CompanyID.CurrentValue, System.DBNull.Value)
			Rs("CompanyID") = PageAlias.CompanyID.DbValue

			' Row Updating event
			bUpdateRow = PageAlias.Row_Updating(RsOld, Rs)
			If bUpdateRow Then
				Try
					PageAlias.Update(Rs)
					EditRow = True
				Catch e As Exception
					If EW_DEBUG_ENABLED Then Throw
					Message = e.Message
					EditRow = False
				End Try
			Else
				If PageAlias.CancelMessage <> "" Then
					Message = PageAlias.CancelMessage
					PageAlias.CancelMessage = ""
				Else
					Message = "Update cancelled"
				End If
				EditRow = False
			End If
		End If

		' Row Updated event
		If EditRow Then
			PageAlias.Row_Updated(RsOld, Rs)
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

		' PageURL
		PageAlias.zPageURL.SetDbValue(PageAlias.zPageURL.CurrentValue, System.DBNull.Value)
		Rs("PageURL") = PageAlias.zPageURL.DbValue

		' TargetURL
		PageAlias.TargetURL.SetDbValue(PageAlias.TargetURL.CurrentValue, System.DBNull.Value)
		Rs("TargetURL") = PageAlias.TargetURL.DbValue

		' AliasType
		PageAlias.AliasType.SetDbValue(PageAlias.AliasType.CurrentValue, System.DBNull.Value)
		Rs("AliasType") = PageAlias.AliasType.DbValue

		' CompanyID
		PageAlias.CompanyID.SetDbValue(PageAlias.CompanyID.CurrentValue, System.DBNull.Value)
		Rs("CompanyID") = PageAlias.CompanyID.DbValue

		' Row Inserting event
		bInsertRow = PageAlias.Row_Inserting(Rs)
		If bInsertRow Then
			Try	
				PageAlias.Insert(Rs)
				AddRow = True
			Catch e As Exception
				If EW_DEBUG_ENABLED Then Throw
				Message = e.Message				
				AddRow = False
			End Try
		Else
			If PageAlias.CancelMessage <> "" Then
				Message = PageAlias.CancelMessage
				PageAlias.CancelMessage = ""
			Else
				Message = "Insert cancelled"
			End If
			AddRow = False
		End If
		If AddRow Then
			LastInsertId = Conn.GetLastInsertId()
			PageAlias.PageAliasID.DbValue = LastInsertId
			Rs("PageAliasID") = PageAlias.PageAliasID.DbValue		

			' Row Inserted event
			PageAlias.Row_Inserted(Rs)
			WriteAuditTrailOnAdd(Rs)
		End If
	End Function

	'
	' Load advanced search
	'
	Sub LoadAdvancedSearch()
		PageAlias.zPageURL.AdvancedSearch.SearchValue = PageAlias.GetAdvancedSearch("x_zPageURL")
		PageAlias.TargetURL.AdvancedSearch.SearchValue = PageAlias.GetAdvancedSearch("x_TargetURL")
		PageAlias.AliasType.AdvancedSearch.SearchValue = PageAlias.GetAdvancedSearch("x_AliasType")
		PageAlias.CompanyID.AdvancedSearch.SearchValue = PageAlias.GetAdvancedSearch("x_CompanyID")
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
		If PageAlias.ExportAll Then
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
		If PageAlias.Export = "xml" Then
			oXmlDoc = New XmlDocument()
			oXmlTbl = oXmlDoc.CreateElement("table")
			oXmlDoc.AppendChild(oXmlTbl)
		Else
			ew_Write(ew_ExportHeader(PageAlias.Export))

			' Horizontal format, write header
			If sExportStyle <> "v" OrElse PageAlias.Export = "csv" Then
				sExportStr = ""
				ew_ExportAddValue(sExportStr, "PageURL", PageAlias.Export)
				ew_ExportAddValue(sExportStr, "TargetURL", PageAlias.Export)
				ew_ExportAddValue(sExportStr, "AliasType", PageAlias.Export)
				ew_ExportAddValue(sExportStr, "CompanyID", PageAlias.Export)
				ew_Write(ew_ExportLine(sExportStr, PageAlias.Export))
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
				PageAlias.RowType = EW_ROWTYPE_VIEW ' Render view
				RenderRow()
				If PageAlias.Export = "xml" Then
					oXmlRec = oXmlDoc.CreateElement("record")
					oXmlTbl.AppendChild(oXmlRec)
					oXmlFld = oXmlDoc.CreateElement("zPageURL") ' PageURL
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(PageAlias.zPageURL.ExportValue(PageAlias.Export, PageAlias.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("TargetURL") ' TargetURL
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(PageAlias.TargetURL.ExportValue(PageAlias.Export, PageAlias.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("AliasType") ' AliasType
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(PageAlias.AliasType.ExportValue(PageAlias.Export, PageAlias.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("CompanyID") ' CompanyID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(PageAlias.CompanyID.ExportValue(PageAlias.Export, PageAlias.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
				Else

					' Vertical format
					If sExportStyle = "v" AndAlso PageAlias.Export <> "csv" Then
						ew_Write(ew_ExportField("PageURL", PageAlias.zPageURL.ExportValue(PageAlias.Export, PageAlias.ExportOriginalValue), PageAlias.Export)) ' PageURL
						ew_Write(ew_ExportField("TargetURL", PageAlias.TargetURL.ExportValue(PageAlias.Export, PageAlias.ExportOriginalValue), PageAlias.Export)) ' TargetURL
						ew_Write(ew_ExportField("AliasType", PageAlias.AliasType.ExportValue(PageAlias.Export, PageAlias.ExportOriginalValue), PageAlias.Export)) ' AliasType
						ew_Write(ew_ExportField("CompanyID", PageAlias.CompanyID.ExportValue(PageAlias.Export, PageAlias.ExportOriginalValue), PageAlias.Export)) ' CompanyID

					' Horizontal format
					Else
						sExportStr = ""
						ew_ExportAddValue(sExportStr, PageAlias.zPageURL.ExportValue(PageAlias.Export, PageAlias.ExportOriginalValue), PageAlias.Export) ' PageURL
						ew_ExportAddValue(sExportStr, PageAlias.TargetURL.ExportValue(PageAlias.Export, PageAlias.ExportOriginalValue), PageAlias.Export) ' TargetURL
						ew_ExportAddValue(sExportStr, PageAlias.AliasType.ExportValue(PageAlias.Export, PageAlias.ExportOriginalValue), PageAlias.Export) ' AliasType
						ew_ExportAddValue(sExportStr, PageAlias.CompanyID.ExportValue(PageAlias.Export, PageAlias.ExportOriginalValue), PageAlias.Export) ' CompanyID
						ew_Write(ew_ExportLine(sExportStr, PageAlias.Export))
					End If
				End If
			End If
		Loop

		' Close recordset
		Rs.Close()
		Rs.Dispose()
		If PageAlias.Export = "xml" Then
			ew_Write("<?xml version=""1.0"" standalone=""yes""?>" & vbcrlf)
			ew_Write(oXmlDoc.OuterXml)
		Else
			ew_Write(ew_ExportFooter(PageAlias.Export))
		End If
	End Sub

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "PageAlias"
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
		Dim table As String = "PageAlias"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsSrc("PageAliasID")

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

		' PageURL Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "PageURL", keyvalue, oldvalue, RsSrc("PageURL"))

		' TargetURL Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "TargetURL", keyvalue, oldvalue, RsSrc("TargetURL"))

		' AliasType Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "AliasType", keyvalue, oldvalue, RsSrc("AliasType"))

		' CompanyID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "CompanyID", keyvalue, oldvalue, RsSrc("CompanyID"))
	End Sub

	' Write Audit Trail (edit page)
	Sub WriteAuditTrailOnEdit(ByRef RsOld As OrderedDictionary, ByRef RsNew As OrderedDictionary)
		Dim table As String = "PageAlias"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsOld("PageAliasID")

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
			fld = PageAlias.FieldByName(fldname)
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
		PageAlias_list = New cPageAlias_list(Me)		
		PageAlias_list.Page_Init()

		' Page main processing
		PageAlias_list.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If PageAlias_list IsNot Nothing Then PageAlias_list.Dispose()
	End Sub
End Class
