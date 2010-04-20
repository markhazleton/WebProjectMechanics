Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class PageRole_list
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public PageRole_list As cPageRole_list

	'
	' Page Class
	'
	Class cPageRole_list
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
				If PageRole.UseTokenInUrl Then Url = Url & "t=" & PageRole.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If PageRole.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (PageRole.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (PageRole.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' PageRole
		Public Property PageRole() As cPageRole
			Get				
				Return ParentPage.PageRole
			End Get
			Set(ByVal v As cPageRole)
				ParentPage.PageRole = v	
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
			m_PageObjName = "PageRole_list"
			m_PageObjTypeName = "cPageRole_list"

			' Table Name
			m_TableName = "PageRole"

			' Initialize table object
			PageRole = New cPageRole(Me)

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
			PageRole.Export = ew_Get("export") ' Get export parameter
			ParentPage.gsExport = PageRole.Export ' Get export parameter, used in header
			ParentPage.gsExportFile = PageRole.TableVar ' Get export file, used in header
			If PageRole.Export = "excel" Then
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
			PageRole.Dispose()
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
					PageRole.CurrentAction = ew_Get("a")

					' Clear inline mode
					If PageRole.CurrentAction = "cancel" Then
						ClearInlineMode()
					End If

					' Switch to inline edit mode
					If PageRole.CurrentAction = "edit" Then
						InlineEditMode()
					End If

					' Switch to grid add mode
					If PageRole.CurrentAction = "gridadd" Then
						GridAddMode()
					End If
				End If
			Else
				PageRole.CurrentAction = ObjForm.GetValue("a_list") ' Get action

				' Inline Update
				If PageRole.CurrentAction = "update" AndAlso ew_SameStr(ew_Session(EW_SESSION_INLINE_MODE), "edit") Then
					InlineUpdate()
				End If

				' Grid Insert
				If PageRole.CurrentAction = "gridinsert" AndAlso ew_SameStr(ew_Session(EW_SESSION_INLINE_MODE), "gridadd") Then
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

			' Set Up Sorting Order
			SetUpSortOrder()
		End If

		' Restore display records
		If (PageRole.RecordsPerPage = -1 OrElse PageRole.RecordsPerPage > 0) Then
			lDisplayRecs = PageRole.RecordsPerPage ' Restore from Session
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
		PageRole.Recordset_Searching(sSrchWhere)

		' Save search criteria
		If sSrchWhere <> "" Then
			If sSrchAdvanced = "" Then ResetAdvancedSearchParms()
			PageRole.SearchWhere = sSrchWhere ' Save to Session
			lStartRec = 1 ' Reset start record counter
			PageRole.StartRecordNumber = lStartRec
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
		PageRole.SessionWhere = sFilter
		PageRole.CurrentFilter = ""

		' Export Data only
		If PageRole.Export = "html" OrElse PageRole.Export = "csv" OrElse PageRole.Export = "word" OrElse PageRole.Export = "excel" OrElse PageRole.Export = "xml" Then
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
			PageRole.RecordsPerPage = lDisplayRecs ' Save to Session

			' Reset start position
			lStartRec = 1
			PageRole.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	'  Exit out of inline mode
	'
	Sub ClearInlineMode()
		PageRole.SetKey("PageRoleID", "") ' Clear inline edit key
		PageRole.CurrentAction = "" ' Clear action
		ew_Session(EW_SESSION_INLINE_MODE) = "" ' Clear inline mode
	End Sub

	'
	' Switch to Grid Add Mode
	'
	Sub GridAddMode()
		ew_Session(EW_SESSION_INLINE_MODE) = "gridadd" ' Enabled grid add
	End Sub

	'
	' Switch to Inline Edit Mode
	'
	Sub InlineEditMode()
		Dim bInlineEdit As Boolean = True
		If ew_Get("PageRoleID") <> "" Then
			PageRole.PageRoleID.QueryStringValue = ew_Get("PageRoleID")
		Else
			bInlineEdit = False
		End If
		If bInlineEdit AndAlso LoadRow() Then
			PageRole.SetKey("PageRoleID", PageRole.PageRoleID.CurrentValue) ' Set up inline edit key
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
				PageRole.SendEmail = True ' Send email on update success
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
			PageRole.EventCancelled = True ' Cancel event
			PageRole.CurrentAction = "edit" ' Stay in edit mode
		End If
	End Sub

	'
	' Check inline edit key
	'
	Function CheckInlineEditKey() As Boolean
		If Not ew_SameStr(PageRole.GetKey("PageRoleID"), PageRole.PageRoleID.CurrentValue) Then
			Return False
		End If
		Return True
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
				PageRole.SendEmail = False ' Do not send email on insert success

				' Validate Form
				If Not ValidateForm() Then
					bGridInsert = False ' Form error, reset action
					Message = ParentPage.gsFormError
				Else
					bGridInsert = AddRow() ' Insert this row
				End If
				If bGridInsert Then
					If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
					sKey = sKey & PageRole.PageRoleID.CurrentValue

					' Add filter for this record
					sFilter = PageRole.KeyFilter
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
			PageRole.CurrentFilter = sWrkFilter
			sSql = PageRole.SQL
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
			PageRole.EventCancelled = True ' Set event cancelled
			PageRole.CurrentAction = "gridadd" ' Stay in gridadd mode
		End If
	End Sub

	Function EmptyRow() As Boolean
		Dim Empty As Boolean = True
		If Empty Then Empty = ew_SameStr(PageRole.RoleID.CurrentValue, PageRole.RoleID.OldValue)
		If Empty Then Empty = ew_SameStr(PageRole.zPageID.CurrentValue, PageRole.zPageID.OldValue)
		If Empty Then Empty = ew_SameStr(PageRole.CompanyID.CurrentValue, PageRole.CompanyID.OldValue)
		Return Empty
	End Function

	'
	' Restore form values for current row
	'
	Sub RestoreCurrentRowFormValues(idx As Integer)
		Dim sKey As String, arKeyFlds() As String

		' Get row based on current index
		ObjForm.Index = idx
		If PageRole.CurrentAction = "gridadd" Then
			LoadFormValues() ' Load form values
		End If
		If PageRole.CurrentAction = "gridedit" Then
			sKey = ObjForm.GetValue("k_key")
			arKeyFlds = sKey.Split(New Char() {Convert.ToChar(EW_COMPOSITE_KEY_SEPARATOR)})
			If arKeyFlds.Length >= 1 Then
				If ew_SameStr(arKeyFlds(0), PageRole.PageRoleID.CurrentValue) Then
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
		BuildSearchSql(sWhere, PageRole.PageRoleID, False) ' PageRoleID
		BuildSearchSql(sWhere, PageRole.RoleID, False) ' RoleID
		BuildSearchSql(sWhere, PageRole.zPageID, False) ' PageID
		BuildSearchSql(sWhere, PageRole.CompanyID, False) ' CompanyID

		' Set up search parm
		If sWhere <> "" Then
			SetSearchParm(PageRole.PageRoleID) ' PageRoleID
			SetSearchParm(PageRole.RoleID) ' RoleID
			SetSearchParm(PageRole.zPageID) ' PageID
			SetSearchParm(PageRole.CompanyID) ' CompanyID
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
		PageRole.SetAdvancedSearch("x_" & FldParm, Fld.AdvancedSearch.SearchValue)
		PageRole.SetAdvancedSearch("z_" & FldParm, Fld.AdvancedSearch.SearchOperator)
		PageRole.SetAdvancedSearch("v_" & FldParm, Fld.AdvancedSearch.SearchCondition)
		PageRole.SetAdvancedSearch("y_" & FldParm, Fld.AdvancedSearch.SearchValue2)
		PageRole.SetAdvancedSearch("w_" & FldParm, Fld.AdvancedSearch.SearchOperator2)
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
	' Clear all search parameters
	'
	Sub ResetSearchParms()

		' Clear search where
		sSrchWhere = ""
		PageRole.SearchWhere = sSrchWhere

		' Clear advanced search parameters
		ResetAdvancedSearchParms()
	End Sub

	'
	' Clear all advanced search parameters
	'
	Sub ResetAdvancedSearchParms()
		PageRole.SetAdvancedSearch("x_PageRoleID", "")
		PageRole.SetAdvancedSearch("x_RoleID", "")
		PageRole.SetAdvancedSearch("x_zPageID", "")
		PageRole.SetAdvancedSearch("x_CompanyID", "")
	End Sub

	'
	' Restore all search parameters
	'
	Sub RestoreSearchParms()
		sSrchWhere = PageRole.SearchWhere

		' Restore advanced search settings
		If ParentPage.gsSearchError = "" Then
			RestoreAdvancedSearchParms()
		End If
	End Sub

	'
	' Restore all advanced search parameters
	'
	Sub RestoreAdvancedSearchParms()
		PageRole.PageRoleID.AdvancedSearch.SearchValue = PageRole.GetAdvancedSearch("x_PageRoleID")
		PageRole.RoleID.AdvancedSearch.SearchValue = PageRole.GetAdvancedSearch("x_RoleID")
		PageRole.zPageID.AdvancedSearch.SearchValue = PageRole.GetAdvancedSearch("x_zPageID")
		PageRole.CompanyID.AdvancedSearch.SearchValue = PageRole.GetAdvancedSearch("x_CompanyID")
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
			PageRole.CurrentOrder = ew_Get("order")
			PageRole.CurrentOrderType = ew_Get("ordertype")
			PageRole.UpdateSort(PageRole.PageRoleID) ' PageRoleID
			PageRole.UpdateSort(PageRole.RoleID) ' RoleID
			PageRole.UpdateSort(PageRole.zPageID) ' PageID
			PageRole.UpdateSort(PageRole.CompanyID) ' CompanyID
			PageRole.StartRecordNumber = 1 ' Reset start position
		End If
	End Sub

	'
	' Load Sort Order parameters
	'
	Sub LoadSortOrder()
		Dim sOrderBy As String
		sOrderBy = PageRole.SessionOrderBy ' Get order by from Session
		If sOrderBy = "" Then
			If PageRole.SqlOrderBy <> "" Then
				sOrderBy = PageRole.SqlOrderBy
				PageRole.SessionOrderBy = sOrderBy
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
				PageRole.SessionOrderBy = sOrderBy
				PageRole.PageRoleID.Sort = ""
				PageRole.RoleID.Sort = ""
				PageRole.zPageID.Sort = ""
				PageRole.CompanyID.Sort = ""
			End If

			' Reset start position
			lStartRec = 1
			PageRole.StartRecordNumber = lStartRec
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
				PageRole.StartRecordNumber = lStartRec
			ElseIf ew_Get(EW_TABLE_PAGE_NO) <> "" AndAlso IsNumeric(ew_Get(EW_TABLE_PAGE_NO)) Then
				nPageNo = ew_ConvertToInt(ew_Get(EW_TABLE_PAGE_NO))
				lStartRec = (nPageNo-1)*lDisplayRecs+1
				If lStartRec <= 0 Then
					lStartRec = 1
				ElseIf lStartRec >= ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 Then
					lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1
				End If
				PageRole.StartRecordNumber = lStartRec
			End If
		End If
		lStartRec = PageRole.StartRecordNumber

		' Check if correct start record counter
		If lStartRec <= 0 Then ' Avoid invalid start record counter
			lStartRec = 1 ' Reset start record counter
			PageRole.StartRecordNumber = lStartRec
		ElseIf lStartRec > lTotalRecs Then ' Avoid starting record > total records
			lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to last page first record
			PageRole.StartRecordNumber = lStartRec
		ElseIf (lStartRec-1) Mod lDisplayRecs <> 0 Then
			lStartRec = ((lStartRec-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to page boundary
			PageRole.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	' Load default values
	'
	Sub LoadDefaultValues()
		PageRole.PageRoleID.OldValue = PageRole.PageRoleID.CurrentValue
		PageRole.RoleID.CurrentValue = 0
		PageRole.RoleID.OldValue = PageRole.RoleID.CurrentValue
		PageRole.zPageID.CurrentValue = 0
		PageRole.zPageID.OldValue = PageRole.zPageID.CurrentValue
		PageRole.CompanyID.CurrentValue = 0
		PageRole.CompanyID.OldValue = PageRole.CompanyID.CurrentValue
	End Sub

	'
	'  Load search values for validation
	'
	Sub LoadSearchValues()
		PageRole.PageRoleID.AdvancedSearch.SearchValue = ew_Get("x_PageRoleID")
    	PageRole.PageRoleID.AdvancedSearch.SearchOperator = ew_Get("z_PageRoleID")
		PageRole.RoleID.AdvancedSearch.SearchValue = ew_Get("x_RoleID")
    	PageRole.RoleID.AdvancedSearch.SearchOperator = ew_Get("z_RoleID")
		PageRole.zPageID.AdvancedSearch.SearchValue = ew_Get("x_zPageID")
    	PageRole.zPageID.AdvancedSearch.SearchOperator = ew_Get("z_zPageID")
		PageRole.CompanyID.AdvancedSearch.SearchValue = ew_Get("x_CompanyID")
    	PageRole.CompanyID.AdvancedSearch.SearchOperator = ew_Get("z_CompanyID")
	End Sub

	'
	' Load form values
	'
	Sub LoadFormValues()
		PageRole.PageRoleID.FormValue = ObjForm.GetValue("x_PageRoleID")
		PageRole.PageRoleID.OldValue = ObjForm.GetValue("o_PageRoleID")
		PageRole.RoleID.FormValue = ObjForm.GetValue("x_RoleID")
		PageRole.RoleID.OldValue = ObjForm.GetValue("o_RoleID")
		PageRole.zPageID.FormValue = ObjForm.GetValue("x_zPageID")
		PageRole.zPageID.OldValue = ObjForm.GetValue("o_zPageID")
		PageRole.CompanyID.FormValue = ObjForm.GetValue("x_CompanyID")
		PageRole.CompanyID.OldValue = ObjForm.GetValue("o_CompanyID")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		PageRole.PageRoleID.CurrentValue = PageRole.PageRoleID.FormValue
		PageRole.RoleID.CurrentValue = PageRole.RoleID.FormValue
		PageRole.zPageID.CurrentValue = PageRole.zPageID.FormValue
		PageRole.CompanyID.CurrentValue = PageRole.CompanyID.FormValue
	End Sub

	'
	' Load recordset
	'
	Function LoadRecordset() As OleDbDataReader

		' Recordset Selecting event
		PageRole.Recordset_Selecting(PageRole.CurrentFilter)

		' Load list page SQL
		Dim sSql As String = PageRole.ListSQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)

		' Count
		lTotalRecs = -1		
		Try			
			If sSql.StartsWith("SELECT * FROM ", StringComparison.InvariantCultureIgnoreCase) AndAlso _				
				ew_Empty(PageRole.SqlGroupBy) AndAlso _
				ew_Empty(PageRole.SqlHaving) Then
				Dim sCntSql As String = PageRole.SelectCountSQL

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
		PageRole.Recordset_Selected(Rs)
		Return Rs
	End Function

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = PageRole.KeyFilter

		' Row Selecting event
		PageRole.Row_Selecting(sFilter)

		' Load SQL based on filter
		PageRole.CurrentFilter = sFilter
		Dim sSql As String = PageRole.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				PageRole.Row_Selected(RsRow)
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
		PageRole.PageRoleID.DbValue = RsRow("PageRoleID")
		PageRole.RoleID.DbValue = RsRow("RoleID")
		PageRole.zPageID.DbValue = RsRow("PageID")
		PageRole.CompanyID.DbValue = RsRow("CompanyID")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		PageRole.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' PageRoleID

		PageRole.PageRoleID.CellCssStyle = ""
		PageRole.PageRoleID.CellCssClass = ""

		' RoleID
		PageRole.RoleID.CellCssStyle = ""
		PageRole.RoleID.CellCssClass = ""

		' PageID
		PageRole.zPageID.CellCssStyle = ""
		PageRole.zPageID.CellCssClass = ""

		' CompanyID
		PageRole.CompanyID.CellCssStyle = ""
		PageRole.CompanyID.CellCssClass = ""

		'
		'  View  Row
		'

		If PageRole.RowType = EW_ROWTYPE_VIEW Then ' View row

			' PageRoleID
			PageRole.PageRoleID.ViewValue = PageRole.PageRoleID.CurrentValue
			PageRole.PageRoleID.CssStyle = ""
			PageRole.PageRoleID.CssClass = ""
			PageRole.PageRoleID.ViewCustomAttributes = ""

			' RoleID
			PageRole.RoleID.ViewValue = PageRole.RoleID.CurrentValue
			PageRole.RoleID.CssStyle = ""
			PageRole.RoleID.CssClass = ""
			PageRole.RoleID.ViewCustomAttributes = ""

			' PageID
			PageRole.zPageID.ViewValue = PageRole.zPageID.CurrentValue
			PageRole.zPageID.CssStyle = ""
			PageRole.zPageID.CssClass = ""
			PageRole.zPageID.ViewCustomAttributes = ""

			' CompanyID
			PageRole.CompanyID.ViewValue = PageRole.CompanyID.CurrentValue
			PageRole.CompanyID.CssStyle = ""
			PageRole.CompanyID.CssClass = ""
			PageRole.CompanyID.ViewCustomAttributes = ""

			' View refer script
			' PageRoleID

			PageRole.PageRoleID.HrefValue = ""

			' RoleID
			PageRole.RoleID.HrefValue = ""

			' PageID
			PageRole.zPageID.HrefValue = ""

			' CompanyID
			PageRole.CompanyID.HrefValue = ""

		'
		'  Add Row
		'

		ElseIf PageRole.RowType = EW_ROWTYPE_ADD Then ' Add row

			' PageRoleID
			' RoleID

			PageRole.RoleID.EditCustomAttributes = ""
			PageRole.RoleID.EditValue = ew_HtmlEncode(PageRole.RoleID.CurrentValue)

			' PageID
			PageRole.zPageID.EditCustomAttributes = ""
			PageRole.zPageID.EditValue = ew_HtmlEncode(PageRole.zPageID.CurrentValue)

			' CompanyID
			PageRole.CompanyID.EditCustomAttributes = ""
			PageRole.CompanyID.EditValue = ew_HtmlEncode(PageRole.CompanyID.CurrentValue)

		'
		'  Edit Row
		'

		ElseIf PageRole.RowType = EW_ROWTYPE_EDIT Then ' Edit row

			' PageRoleID
			PageRole.PageRoleID.EditCustomAttributes = ""
			PageRole.PageRoleID.EditValue = PageRole.PageRoleID.CurrentValue
			PageRole.PageRoleID.CssStyle = ""
			PageRole.PageRoleID.CssClass = ""
			PageRole.PageRoleID.ViewCustomAttributes = ""

			' RoleID
			PageRole.RoleID.EditCustomAttributes = ""
			PageRole.RoleID.EditValue = ew_HtmlEncode(PageRole.RoleID.CurrentValue)

			' PageID
			PageRole.zPageID.EditCustomAttributes = ""
			PageRole.zPageID.EditValue = ew_HtmlEncode(PageRole.zPageID.CurrentValue)

			' CompanyID
			PageRole.CompanyID.EditCustomAttributes = ""
			PageRole.CompanyID.EditValue = ew_HtmlEncode(PageRole.CompanyID.CurrentValue)

			' Edit refer script
			' PageRoleID

			PageRole.PageRoleID.HrefValue = ""

			' RoleID
			PageRole.RoleID.HrefValue = ""

			' PageID
			PageRole.zPageID.HrefValue = ""

			' CompanyID
			PageRole.CompanyID.HrefValue = ""
		End If

		' Row Rendered event
		PageRole.Row_Rendered()
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
		If Not ew_CheckInteger(PageRole.RoleID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Incorrect integer - Role ID"
		End If
		If Not ew_CheckInteger(PageRole.zPageID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Incorrect integer - Page ID"
		End If
		If Not ew_CheckInteger(PageRole.CompanyID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Incorrect integer - Company ID"
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
		sFilter = PageRole.KeyFilter
		PageRole.CurrentFilter  = sFilter
		sSql = PageRole.SQL
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

			' PageRoleID
			' RoleID

			PageRole.RoleID.SetDbValue(PageRole.RoleID.CurrentValue, System.DBNull.Value)
			Rs("RoleID") = PageRole.RoleID.DbValue

			' PageID
			PageRole.zPageID.SetDbValue(PageRole.zPageID.CurrentValue, System.DBNull.Value)
			Rs("PageID") = PageRole.zPageID.DbValue

			' CompanyID
			PageRole.CompanyID.SetDbValue(PageRole.CompanyID.CurrentValue, System.DBNull.Value)
			Rs("CompanyID") = PageRole.CompanyID.DbValue

			' Row Updating event
			bUpdateRow = PageRole.Row_Updating(RsOld, Rs)
			If bUpdateRow Then
				Try
					PageRole.Update(Rs)
					EditRow = True
				Catch e As Exception
					If EW_DEBUG_ENABLED Then Throw
					Message = e.Message
					EditRow = False
				End Try
			Else
				If PageRole.CancelMessage <> "" Then
					Message = PageRole.CancelMessage
					PageRole.CancelMessage = ""
				Else
					Message = "Update cancelled"
				End If
				EditRow = False
			End If
		End If

		' Row Updated event
		If EditRow Then
			PageRole.Row_Updated(RsOld, Rs)
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

		' PageRoleID
		' RoleID

		PageRole.RoleID.SetDbValue(PageRole.RoleID.CurrentValue, System.DBNull.Value)
		Rs("RoleID") = PageRole.RoleID.DbValue

		' PageID
		PageRole.zPageID.SetDbValue(PageRole.zPageID.CurrentValue, System.DBNull.Value)
		Rs("PageID") = PageRole.zPageID.DbValue

		' CompanyID
		PageRole.CompanyID.SetDbValue(PageRole.CompanyID.CurrentValue, System.DBNull.Value)
		Rs("CompanyID") = PageRole.CompanyID.DbValue

		' Row Inserting event
		bInsertRow = PageRole.Row_Inserting(Rs)
		If bInsertRow Then
			Try	
				PageRole.Insert(Rs)
				AddRow = True
			Catch e As Exception
				If EW_DEBUG_ENABLED Then Throw
				Message = e.Message				
				AddRow = False
			End Try
		Else
			If PageRole.CancelMessage <> "" Then
				Message = PageRole.CancelMessage
				PageRole.CancelMessage = ""
			Else
				Message = "Insert cancelled"
			End If
			AddRow = False
		End If
		If AddRow Then
			LastInsertId = Conn.GetLastInsertId()
			PageRole.PageRoleID.DbValue = LastInsertId
			Rs("PageRoleID") = PageRole.PageRoleID.DbValue		

			' Row Inserted event
			PageRole.Row_Inserted(Rs)
			WriteAuditTrailOnAdd(Rs)
		End If
	End Function

	'
	' Load advanced search
	'
	Sub LoadAdvancedSearch()
		PageRole.PageRoleID.AdvancedSearch.SearchValue = PageRole.GetAdvancedSearch("x_PageRoleID")
		PageRole.RoleID.AdvancedSearch.SearchValue = PageRole.GetAdvancedSearch("x_RoleID")
		PageRole.zPageID.AdvancedSearch.SearchValue = PageRole.GetAdvancedSearch("x_zPageID")
		PageRole.CompanyID.AdvancedSearch.SearchValue = PageRole.GetAdvancedSearch("x_CompanyID")
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
		If PageRole.ExportAll Then
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
		If PageRole.Export = "xml" Then
			oXmlDoc = New XmlDocument()
			oXmlTbl = oXmlDoc.CreateElement("table")
			oXmlDoc.AppendChild(oXmlTbl)
		Else
			ew_Write(ew_ExportHeader(PageRole.Export))

			' Horizontal format, write header
			If sExportStyle <> "v" OrElse PageRole.Export = "csv" Then
				sExportStr = ""
				ew_ExportAddValue(sExportStr, "PageRoleID", PageRole.Export)
				ew_ExportAddValue(sExportStr, "RoleID", PageRole.Export)
				ew_ExportAddValue(sExportStr, "PageID", PageRole.Export)
				ew_ExportAddValue(sExportStr, "CompanyID", PageRole.Export)
				ew_Write(ew_ExportLine(sExportStr, PageRole.Export))
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
				PageRole.RowType = EW_ROWTYPE_VIEW ' Render view
				RenderRow()
				If PageRole.Export = "xml" Then
					oXmlRec = oXmlDoc.CreateElement("record")
					oXmlTbl.AppendChild(oXmlRec)
					oXmlFld = oXmlDoc.CreateElement("PageRoleID") ' PageRoleID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(PageRole.PageRoleID.ExportValue(PageRole.Export, PageRole.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("RoleID") ' RoleID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(PageRole.RoleID.ExportValue(PageRole.Export, PageRole.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("zPageID") ' PageID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(PageRole.zPageID.ExportValue(PageRole.Export, PageRole.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("CompanyID") ' CompanyID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(PageRole.CompanyID.ExportValue(PageRole.Export, PageRole.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
				Else

					' Vertical format
					If sExportStyle = "v" AndAlso PageRole.Export <> "csv" Then
						ew_Write(ew_ExportField("PageRoleID", PageRole.PageRoleID.ExportValue(PageRole.Export, PageRole.ExportOriginalValue), PageRole.Export)) ' PageRoleID
						ew_Write(ew_ExportField("RoleID", PageRole.RoleID.ExportValue(PageRole.Export, PageRole.ExportOriginalValue), PageRole.Export)) ' RoleID
						ew_Write(ew_ExportField("PageID", PageRole.zPageID.ExportValue(PageRole.Export, PageRole.ExportOriginalValue), PageRole.Export)) ' PageID
						ew_Write(ew_ExportField("CompanyID", PageRole.CompanyID.ExportValue(PageRole.Export, PageRole.ExportOriginalValue), PageRole.Export)) ' CompanyID

					' Horizontal format
					Else
						sExportStr = ""
						ew_ExportAddValue(sExportStr, PageRole.PageRoleID.ExportValue(PageRole.Export, PageRole.ExportOriginalValue), PageRole.Export) ' PageRoleID
						ew_ExportAddValue(sExportStr, PageRole.RoleID.ExportValue(PageRole.Export, PageRole.ExportOriginalValue), PageRole.Export) ' RoleID
						ew_ExportAddValue(sExportStr, PageRole.zPageID.ExportValue(PageRole.Export, PageRole.ExportOriginalValue), PageRole.Export) ' PageID
						ew_ExportAddValue(sExportStr, PageRole.CompanyID.ExportValue(PageRole.Export, PageRole.ExportOriginalValue), PageRole.Export) ' CompanyID
						ew_Write(ew_ExportLine(sExportStr, PageRole.Export))
					End If
				End If
			End If
		Loop

		' Close recordset
		Rs.Close()
		Rs.Dispose()
		If PageRole.Export = "xml" Then
			ew_Write("<?xml version=""1.0"" standalone=""yes""?>" & vbcrlf)
			ew_Write(oXmlDoc.OuterXml)
		Else
			ew_Write(ew_ExportFooter(PageRole.Export))
		End If
	End Sub

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "PageRole"
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
		Dim table As String = "PageRole"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsSrc("PageRoleID")

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

		' PageRoleID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "PageRoleID", keyvalue, oldvalue, RsSrc("PageRoleID"))

		' RoleID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "RoleID", keyvalue, oldvalue, RsSrc("RoleID"))

		' PageID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "PageID", keyvalue, oldvalue, RsSrc("PageID"))

		' CompanyID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "CompanyID", keyvalue, oldvalue, RsSrc("CompanyID"))
	End Sub

	' Write Audit Trail (edit page)
	Sub WriteAuditTrailOnEdit(ByRef RsOld As OrderedDictionary, ByRef RsNew As OrderedDictionary)
		Dim table As String = "PageRole"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsOld("PageRoleID")

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
			fld = PageRole.FieldByName(fldname)
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
		PageRole_list = New cPageRole_list(Me)		
		PageRole_list.Page_Init()

		' Page main processing
		PageRole_list.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If PageRole_list IsNot Nothing Then PageRole_list.Dispose()
	End Sub
End Class
