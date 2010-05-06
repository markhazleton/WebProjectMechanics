Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class CompanySiteParameter_list
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public CompanySiteParameter_list As cCompanySiteParameter_list

	'
	' Page Class
	'
	Class cCompanySiteParameter_list
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
				If CompanySiteParameter.UseTokenInUrl Then Url = Url & "t=" & CompanySiteParameter.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If CompanySiteParameter.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (CompanySiteParameter.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (CompanySiteParameter.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' CompanySiteParameter
		Public Property CompanySiteParameter() As cCompanySiteParameter
			Get				
				Return ParentPage.CompanySiteParameter
			End Get
			Set(ByVal v As cCompanySiteParameter)
				ParentPage.CompanySiteParameter = v	
			End Set	
		End Property

		' CompanySiteParameter
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
			m_PageObjName = "CompanySiteParameter_list"
			m_PageObjTypeName = "cCompanySiteParameter_list"

			' Table Name
			m_TableName = "CompanySiteParameter"

			' Initialize table object
			CompanySiteParameter = New cCompanySiteParameter(Me)
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
			CompanySiteParameter.Export = ew_Get("export") ' Get export parameter
			ParentPage.gsExport = CompanySiteParameter.Export ' Get export parameter, used in header
			ParentPage.gsExportFile = CompanySiteParameter.TableVar ' Get export file, used in header
			If CompanySiteParameter.Export = "excel" Then
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
			CompanySiteParameter.Dispose()
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

			' Set up master detail parameters
			SetUpMasterDetail()

			' Check QueryString parameters
			If ObjForm.GetValue("a_list") = "" Then ' Check if post back first
				If ew_Get("a") <> "" Then
					CompanySiteParameter.CurrentAction = ew_Get("a")

					' Clear inline mode
					If CompanySiteParameter.CurrentAction = "cancel" Then
						ClearInlineMode()
					End If

					' Switch to grid edit mode
					If CompanySiteParameter.CurrentAction = "gridedit" Then
						GridEditMode()
					End If

					' Switch to inline edit mode
					If CompanySiteParameter.CurrentAction = "edit" Then
						InlineEditMode()
					End If

					' Switch to grid add mode
					If CompanySiteParameter.CurrentAction = "gridadd" Then
						GridAddMode()
					End If
				End If
			Else
				CompanySiteParameter.CurrentAction = ObjForm.GetValue("a_list") ' Get action

				' Grid Update
				If CompanySiteParameter.CurrentAction = "gridupdate" AndAlso ew_SameStr(ew_Session(EW_SESSION_INLINE_MODE), "gridedit") Then
					GridUpdate()
				End If

				' Inline Update
				If CompanySiteParameter.CurrentAction = "update" AndAlso ew_SameStr(ew_Session(EW_SESSION_INLINE_MODE), "edit") Then
					InlineUpdate()
				End If

				' Grid Insert
				If CompanySiteParameter.CurrentAction = "gridinsert" AndAlso ew_SameStr(ew_Session(EW_SESSION_INLINE_MODE), "gridadd") Then
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
		If (CompanySiteParameter.RecordsPerPage = -1 OrElse CompanySiteParameter.RecordsPerPage > 0) Then
			lDisplayRecs = CompanySiteParameter.RecordsPerPage ' Restore from Session
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
		CompanySiteParameter.Recordset_Searching(sSrchWhere)

		' Save search criteria
		If sSrchWhere <> "" Then
			If sSrchAdvanced = "" Then ResetAdvancedSearchParms()
			CompanySiteParameter.SearchWhere = sSrchWhere ' Save to Session
			lStartRec = 1 ' Reset start record counter
			CompanySiteParameter.StartRecordNumber = lStartRec
		Else
			RestoreSearchParms()
		End If

		' Build filter
		sFilter = ""

		' Restore master/detail filter
		sDbMasterFilter = CompanySiteParameter.MasterFilter ' Restore master filter
		sDbDetailFilter = CompanySiteParameter.DetailFilter ' Restore detail filter
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
		Dim RsMaster As OleDbDataReader

		' Load master record
		If CompanySiteParameter.MasterFilter <> "" AndAlso CompanySiteParameter.CurrentMasterTable = "SiteParameterType" Then
			RsMaster = SiteParameterType.LoadRs(sDbMasterFilter)
			bMasterRecordExists = RsMaster IsNot Nothing
			If Not bMasterRecordExists Then
				CompanySiteParameter.MasterFilter = "" ' Clear master filter
				CompanySiteParameter.DetailFilter = "" ' Clear detail filter
				Message = "No records found" ' Set no record found
			Page_Terminate(CompanySiteParameter.ReturnUrl) ' Return to caller
			Else
				RsMaster.Read()
				SiteParameterType.LoadListRowValues(RsMaster)
				SiteParameterType.RowType = EW_ROWTYPE_MASTER ' Master row
				SiteParameterType.RenderListRow()
				RsMaster.Close()
				RsMaster.Dispose()
			End If
		End If

		' Set up filter in Session
		CompanySiteParameter.SessionWhere = sFilter
		CompanySiteParameter.CurrentFilter = ""

		' Export Data only
		If CompanySiteParameter.Export = "html" OrElse CompanySiteParameter.Export = "csv" OrElse CompanySiteParameter.Export = "word" OrElse CompanySiteParameter.Export = "excel" OrElse CompanySiteParameter.Export = "xml" Then
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
			CompanySiteParameter.RecordsPerPage = lDisplayRecs ' Save to Session

			' Reset start position
			lStartRec = 1
			CompanySiteParameter.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	'  Exit out of inline mode
	'
	Sub ClearInlineMode()
		CompanySiteParameter.SetKey("CompanySiteParameterID", "") ' Clear inline edit key
		CompanySiteParameter.CurrentAction = "" ' Clear action
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
		If ew_Get("CompanySiteParameterID") <> "" Then
			CompanySiteParameter.CompanySiteParameterID.QueryStringValue = ew_Get("CompanySiteParameterID")
		Else
			bInlineEdit = False
		End If
		If bInlineEdit AndAlso LoadRow() Then
			CompanySiteParameter.SetKey("CompanySiteParameterID", CompanySiteParameter.CompanySiteParameterID.CurrentValue) ' Set up inline edit key
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
				CompanySiteParameter.SendEmail = True ' Send email on update success
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
			CompanySiteParameter.EventCancelled = True ' Cancel event
			CompanySiteParameter.CurrentAction = "edit" ' Stay in edit mode
		End If
	End Sub

	'
	' Check inline edit key
	'
	Function CheckInlineEditKey() As Boolean
		If Not ew_SameStr(CompanySiteParameter.GetKey("CompanySiteParameterID"), CompanySiteParameter.CompanySiteParameterID.CurrentValue) Then
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
		CompanySiteParameter.CurrentFilter = BuildKeyFilter()
		sSql = CompanySiteParameter.SQL
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
					CompanySiteParameter.SendEmail = False ' Do not send email on update success
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
			CompanySiteParameter.EventCancelled = True ' Set event cancelled
			CompanySiteParameter.CurrentAction = "gridedit" ' Stay in gridedit mode
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
				sFilter = CompanySiteParameter.KeyFilter
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
			CompanySiteParameter.CompanySiteParameterID.FormValue = arKeyFlds(0)
			If Not IsNumeric(CompanySiteParameter.CompanySiteParameterID.FormValue) Then	Return False
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
				CompanySiteParameter.SendEmail = False ' Do not send email on insert success

				' Validate Form
				If Not ValidateForm() Then
					bGridInsert = False ' Form error, reset action
					Message = ParentPage.gsFormError
				Else
					bGridInsert = AddRow() ' Insert this row
				End If
				If bGridInsert Then
					If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
					sKey = sKey & CompanySiteParameter.CompanySiteParameterID.CurrentValue

					' Add filter for this record
					sFilter = CompanySiteParameter.KeyFilter
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
			CompanySiteParameter.CurrentFilter = sWrkFilter
			sSql = CompanySiteParameter.SQL
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
			CompanySiteParameter.EventCancelled = True ' Set event cancelled
			CompanySiteParameter.CurrentAction = "gridadd" ' Stay in gridadd mode
		End If
	End Sub

	Function EmptyRow() As Boolean
		Dim Empty As Boolean = True
		If Empty Then Empty = ew_SameStr(CompanySiteParameter.CompanyID.CurrentValue, CompanySiteParameter.CompanyID.OldValue)
		If Empty Then Empty = ew_SameStr(CompanySiteParameter.zPageID.CurrentValue, CompanySiteParameter.zPageID.OldValue)
		If Empty Then Empty = ew_SameStr(CompanySiteParameter.SiteCategoryGroupID.CurrentValue, CompanySiteParameter.SiteCategoryGroupID.OldValue)
		If Empty Then Empty = ew_SameStr(CompanySiteParameter.SiteParameterTypeID.CurrentValue, CompanySiteParameter.SiteParameterTypeID.OldValue)
		If Empty Then Empty = ew_SameStr(CompanySiteParameter.SortOrder.CurrentValue, CompanySiteParameter.SortOrder.OldValue)
		Return Empty
	End Function

	'
	' Restore form values for current row
	'
	Sub RestoreCurrentRowFormValues(idx As Integer)
		Dim sKey As String, arKeyFlds() As String

		' Get row based on current index
		ObjForm.Index = idx
		If CompanySiteParameter.CurrentAction = "gridadd" Then
			LoadFormValues() ' Load form values
		End If
		If CompanySiteParameter.CurrentAction = "gridedit" Then
			sKey = ObjForm.GetValue("k_key")
			arKeyFlds = sKey.Split(New Char() {Convert.ToChar(EW_COMPOSITE_KEY_SEPARATOR)})
			If arKeyFlds.Length >= 1 Then
				If ew_SameStr(arKeyFlds(0), CompanySiteParameter.CompanySiteParameterID.CurrentValue) Then
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
		BuildSearchSql(sWhere, CompanySiteParameter.CompanyID, False) ' CompanyID
		BuildSearchSql(sWhere, CompanySiteParameter.zPageID, False) ' PageID
		BuildSearchSql(sWhere, CompanySiteParameter.SiteCategoryGroupID, False) ' SiteCategoryGroupID
		BuildSearchSql(sWhere, CompanySiteParameter.SiteParameterTypeID, False) ' SiteParameterTypeID

		' Set up search parm
		If sWhere <> "" Then
			SetSearchParm(CompanySiteParameter.CompanyID) ' CompanyID
			SetSearchParm(CompanySiteParameter.zPageID) ' PageID
			SetSearchParm(CompanySiteParameter.SiteCategoryGroupID) ' SiteCategoryGroupID
			SetSearchParm(CompanySiteParameter.SiteParameterTypeID) ' SiteParameterTypeID
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
		CompanySiteParameter.SetAdvancedSearch("x_" & FldParm, Fld.AdvancedSearch.SearchValue)
		CompanySiteParameter.SetAdvancedSearch("z_" & FldParm, Fld.AdvancedSearch.SearchOperator)
		CompanySiteParameter.SetAdvancedSearch("v_" & FldParm, Fld.AdvancedSearch.SearchCondition)
		CompanySiteParameter.SetAdvancedSearch("y_" & FldParm, Fld.AdvancedSearch.SearchValue2)
		CompanySiteParameter.SetAdvancedSearch("w_" & FldParm, Fld.AdvancedSearch.SearchOperator2)
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
		CompanySiteParameter.SearchWhere = sSrchWhere

		' Clear advanced search parameters
		ResetAdvancedSearchParms()
	End Sub

	'
	' Clear all advanced search parameters
	'
	Sub ResetAdvancedSearchParms()
		CompanySiteParameter.SetAdvancedSearch("x_CompanyID", "")
		CompanySiteParameter.SetAdvancedSearch("x_zPageID", "")
		CompanySiteParameter.SetAdvancedSearch("x_SiteCategoryGroupID", "")
		CompanySiteParameter.SetAdvancedSearch("x_SiteParameterTypeID", "")
	End Sub

	'
	' Restore all search parameters
	'
	Sub RestoreSearchParms()
		sSrchWhere = CompanySiteParameter.SearchWhere

		' Restore advanced search settings
		If ParentPage.gsSearchError = "" Then
			RestoreAdvancedSearchParms()
		End If
	End Sub

	'
	' Restore all advanced search parameters
	'
	Sub RestoreAdvancedSearchParms()
		CompanySiteParameter.CompanyID.AdvancedSearch.SearchValue = CompanySiteParameter.GetAdvancedSearch("x_CompanyID")
		CompanySiteParameter.zPageID.AdvancedSearch.SearchValue = CompanySiteParameter.GetAdvancedSearch("x_zPageID")
		CompanySiteParameter.SiteCategoryGroupID.AdvancedSearch.SearchValue = CompanySiteParameter.GetAdvancedSearch("x_SiteCategoryGroupID")
		CompanySiteParameter.SiteParameterTypeID.AdvancedSearch.SearchValue = CompanySiteParameter.GetAdvancedSearch("x_SiteParameterTypeID")
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
			CompanySiteParameter.CurrentOrder = ew_Get("order")
			CompanySiteParameter.CurrentOrderType = ew_Get("ordertype")
			CompanySiteParameter.UpdateSort(CompanySiteParameter.CompanyID) ' CompanyID
			CompanySiteParameter.UpdateSort(CompanySiteParameter.zPageID) ' PageID
			CompanySiteParameter.UpdateSort(CompanySiteParameter.SiteCategoryGroupID) ' SiteCategoryGroupID
			CompanySiteParameter.UpdateSort(CompanySiteParameter.SiteParameterTypeID) ' SiteParameterTypeID
			CompanySiteParameter.UpdateSort(CompanySiteParameter.SortOrder) ' SortOrder
			CompanySiteParameter.StartRecordNumber = 1 ' Reset start position
		End If
	End Sub

	'
	' Load Sort Order parameters
	'
	Sub LoadSortOrder()
		Dim sOrderBy As String
		sOrderBy = CompanySiteParameter.SessionOrderBy ' Get order by from Session
		If sOrderBy = "" Then
			If CompanySiteParameter.SqlOrderBy <> "" Then
				sOrderBy = CompanySiteParameter.SqlOrderBy
				CompanySiteParameter.SessionOrderBy = sOrderBy
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

			' Reset master/detail keys
			If ew_SameText(sCmd, "resetall") Then
				CompanySiteParameter.CurrentMasterTable = "" ' Clear master table
				CompanySiteParameter.MasterFilter = "" ' Clear master filter
				sDbMasterFilter = ""
				CompanySiteParameter.DetailFilter = "" ' Clear detail filter
				sDbDetailFilter = ""
				CompanySiteParameter.SiteParameterTypeID.SessionValue = ""
			End If

			' Reset sort criteria
			If ew_SameText(sCmd, "resetsort") Then
				Dim sOrderBy As String = ""
				CompanySiteParameter.SessionOrderBy = sOrderBy
				CompanySiteParameter.CompanyID.Sort = ""
				CompanySiteParameter.zPageID.Sort = ""
				CompanySiteParameter.SiteCategoryGroupID.Sort = ""
				CompanySiteParameter.SiteParameterTypeID.Sort = ""
				CompanySiteParameter.SortOrder.Sort = ""
			End If

			' Reset start position
			lStartRec = 1
			CompanySiteParameter.StartRecordNumber = lStartRec
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
				CompanySiteParameter.StartRecordNumber = lStartRec
			ElseIf ew_Get(EW_TABLE_PAGE_NO) <> "" AndAlso IsNumeric(ew_Get(EW_TABLE_PAGE_NO)) Then
				nPageNo = ew_ConvertToInt(ew_Get(EW_TABLE_PAGE_NO))
				lStartRec = (nPageNo-1)*lDisplayRecs+1
				If lStartRec <= 0 Then
					lStartRec = 1
				ElseIf lStartRec >= ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 Then
					lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1
				End If
				CompanySiteParameter.StartRecordNumber = lStartRec
			End If
		End If
		lStartRec = CompanySiteParameter.StartRecordNumber

		' Check if correct start record counter
		If lStartRec <= 0 Then ' Avoid invalid start record counter
			lStartRec = 1 ' Reset start record counter
			CompanySiteParameter.StartRecordNumber = lStartRec
		ElseIf lStartRec > lTotalRecs Then ' Avoid starting record > total records
			lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to last page first record
			CompanySiteParameter.StartRecordNumber = lStartRec
		ElseIf (lStartRec-1) Mod lDisplayRecs <> 0 Then
			lStartRec = ((lStartRec-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to page boundary
			CompanySiteParameter.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	' Load default values
	'
	Sub LoadDefaultValues()
		CompanySiteParameter.CompanyID.OldValue = CompanySiteParameter.CompanyID.CurrentValue
		CompanySiteParameter.zPageID.OldValue = CompanySiteParameter.zPageID.CurrentValue
		CompanySiteParameter.SiteCategoryGroupID.OldValue = CompanySiteParameter.SiteCategoryGroupID.CurrentValue
		CompanySiteParameter.SiteParameterTypeID.OldValue = CompanySiteParameter.SiteParameterTypeID.CurrentValue
		CompanySiteParameter.SortOrder.CurrentValue = 999
		CompanySiteParameter.SortOrder.OldValue = CompanySiteParameter.SortOrder.CurrentValue
	End Sub

	'
	'  Load search values for validation
	'
	Sub LoadSearchValues()
		CompanySiteParameter.CompanyID.AdvancedSearch.SearchValue = ew_Get("x_CompanyID")
    	CompanySiteParameter.CompanyID.AdvancedSearch.SearchOperator = ew_Get("z_CompanyID")
		CompanySiteParameter.zPageID.AdvancedSearch.SearchValue = ew_Get("x_zPageID")
    	CompanySiteParameter.zPageID.AdvancedSearch.SearchOperator = ew_Get("z_zPageID")
		CompanySiteParameter.SiteCategoryGroupID.AdvancedSearch.SearchValue = ew_Get("x_SiteCategoryGroupID")
    	CompanySiteParameter.SiteCategoryGroupID.AdvancedSearch.SearchOperator = ew_Get("z_SiteCategoryGroupID")
		CompanySiteParameter.SiteParameterTypeID.AdvancedSearch.SearchValue = ew_Get("x_SiteParameterTypeID")
    	CompanySiteParameter.SiteParameterTypeID.AdvancedSearch.SearchOperator = ew_Get("z_SiteParameterTypeID")
	End Sub

	'
	' Load form values
	'
	Sub LoadFormValues()
		CompanySiteParameter.CompanyID.FormValue = ObjForm.GetValue("x_CompanyID")
		CompanySiteParameter.CompanyID.OldValue = ObjForm.GetValue("o_CompanyID")
		CompanySiteParameter.zPageID.FormValue = ObjForm.GetValue("x_zPageID")
		CompanySiteParameter.zPageID.OldValue = ObjForm.GetValue("o_zPageID")
		CompanySiteParameter.SiteCategoryGroupID.FormValue = ObjForm.GetValue("x_SiteCategoryGroupID")
		CompanySiteParameter.SiteCategoryGroupID.OldValue = ObjForm.GetValue("o_SiteCategoryGroupID")
		CompanySiteParameter.SiteParameterTypeID.FormValue = ObjForm.GetValue("x_SiteParameterTypeID")
		CompanySiteParameter.SiteParameterTypeID.OldValue = ObjForm.GetValue("o_SiteParameterTypeID")
		CompanySiteParameter.SortOrder.FormValue = ObjForm.GetValue("x_SortOrder")
		CompanySiteParameter.SortOrder.OldValue = ObjForm.GetValue("o_SortOrder")
		CompanySiteParameter.CompanySiteParameterID.FormValue = ObjForm.GetValue("x_CompanySiteParameterID")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		CompanySiteParameter.CompanyID.CurrentValue = CompanySiteParameter.CompanyID.FormValue
		CompanySiteParameter.zPageID.CurrentValue = CompanySiteParameter.zPageID.FormValue
		CompanySiteParameter.SiteCategoryGroupID.CurrentValue = CompanySiteParameter.SiteCategoryGroupID.FormValue
		CompanySiteParameter.SiteParameterTypeID.CurrentValue = CompanySiteParameter.SiteParameterTypeID.FormValue
		CompanySiteParameter.SortOrder.CurrentValue = CompanySiteParameter.SortOrder.FormValue
		CompanySiteParameter.CompanySiteParameterID.CurrentValue = CompanySiteParameter.CompanySiteParameterID.FormValue
	End Sub

	'
	' Load recordset
	'
	Function LoadRecordset() As OleDbDataReader

		' Recordset Selecting event
		CompanySiteParameter.Recordset_Selecting(CompanySiteParameter.CurrentFilter)

		' Load list page SQL
		Dim sSql As String = CompanySiteParameter.ListSQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)

		' Count
		lTotalRecs = -1		
		Try			
			If sSql.StartsWith("SELECT * FROM ", StringComparison.InvariantCultureIgnoreCase) AndAlso _				
				ew_Empty(CompanySiteParameter.SqlGroupBy) AndAlso _
				ew_Empty(CompanySiteParameter.SqlHaving) Then
				Dim sCntSql As String = CompanySiteParameter.SelectCountSQL

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
		CompanySiteParameter.Recordset_Selected(Rs)
		Return Rs
	End Function

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = CompanySiteParameter.KeyFilter

		' Row Selecting event
		CompanySiteParameter.Row_Selecting(sFilter)

		' Load SQL based on filter
		CompanySiteParameter.CurrentFilter = sFilter
		Dim sSql As String = CompanySiteParameter.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				CompanySiteParameter.Row_Selected(RsRow)
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
		CompanySiteParameter.CompanySiteParameterID.DbValue = RsRow("CompanySiteParameterID")
		CompanySiteParameter.CompanyID.DbValue = RsRow("CompanyID")
		CompanySiteParameter.zPageID.DbValue = RsRow("PageID")
		CompanySiteParameter.SiteCategoryGroupID.DbValue = RsRow("SiteCategoryGroupID")
		CompanySiteParameter.SiteParameterTypeID.DbValue = RsRow("SiteParameterTypeID")
		CompanySiteParameter.SortOrder.DbValue = RsRow("SortOrder")
		CompanySiteParameter.ParameterValue.DbValue = RsRow("ParameterValue")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		CompanySiteParameter.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' CompanyID

		CompanySiteParameter.CompanyID.CellCssStyle = ""
		CompanySiteParameter.CompanyID.CellCssClass = ""

		' PageID
		CompanySiteParameter.zPageID.CellCssStyle = ""
		CompanySiteParameter.zPageID.CellCssClass = ""

		' SiteCategoryGroupID
		CompanySiteParameter.SiteCategoryGroupID.CellCssStyle = ""
		CompanySiteParameter.SiteCategoryGroupID.CellCssClass = ""

		' SiteParameterTypeID
		CompanySiteParameter.SiteParameterTypeID.CellCssStyle = ""
		CompanySiteParameter.SiteParameterTypeID.CellCssClass = ""

		' SortOrder
		CompanySiteParameter.SortOrder.CellCssStyle = ""
		CompanySiteParameter.SortOrder.CellCssClass = ""

		'
		'  View  Row
		'

		If CompanySiteParameter.RowType = EW_ROWTYPE_VIEW Then ' View row

			' CompanyID
			If ew_NotEmpty(CompanySiteParameter.CompanyID.CurrentValue) Then
				sSqlWrk = "SELECT [CompanyName] FROM [Company] WHERE [CompanyID] = " & ew_AdjustSql(CompanySiteParameter.CompanyID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanySiteParameter.CompanyID.ViewValue = RsWrk("CompanyName")
				Else
					CompanySiteParameter.CompanyID.ViewValue = CompanySiteParameter.CompanyID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanySiteParameter.CompanyID.ViewValue = System.DBNull.Value
			End If
			CompanySiteParameter.CompanyID.CssStyle = ""
			CompanySiteParameter.CompanyID.CssClass = ""
			CompanySiteParameter.CompanyID.ViewCustomAttributes = ""

			' PageID
			If ew_NotEmpty(CompanySiteParameter.zPageID.CurrentValue) Then
				sSqlWrk = "SELECT [PageName] FROM [Page] WHERE [PageID] = " & ew_AdjustSql(CompanySiteParameter.zPageID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [PageName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanySiteParameter.zPageID.ViewValue = RsWrk("PageName")
				Else
					CompanySiteParameter.zPageID.ViewValue = CompanySiteParameter.zPageID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanySiteParameter.zPageID.ViewValue = System.DBNull.Value
			End If
			CompanySiteParameter.zPageID.CssStyle = ""
			CompanySiteParameter.zPageID.CssClass = ""
			CompanySiteParameter.zPageID.ViewCustomAttributes = ""

			' SiteCategoryGroupID
			If ew_NotEmpty(CompanySiteParameter.SiteCategoryGroupID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteCategoryGroupNM] FROM [SiteCategoryGroup] WHERE [SiteCategoryGroupID] = " & ew_AdjustSql(CompanySiteParameter.SiteCategoryGroupID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryGroupNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanySiteParameter.SiteCategoryGroupID.ViewValue = RsWrk("SiteCategoryGroupNM")
				Else
					CompanySiteParameter.SiteCategoryGroupID.ViewValue = CompanySiteParameter.SiteCategoryGroupID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanySiteParameter.SiteCategoryGroupID.ViewValue = System.DBNull.Value
			End If
			CompanySiteParameter.SiteCategoryGroupID.CssStyle = ""
			CompanySiteParameter.SiteCategoryGroupID.CssClass = ""
			CompanySiteParameter.SiteCategoryGroupID.ViewCustomAttributes = ""

			' SiteParameterTypeID
			If ew_NotEmpty(CompanySiteParameter.SiteParameterTypeID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteParameterTypeNM] FROM [SiteParameterType] WHERE [SiteParameterTypeID] = " & ew_AdjustSql(CompanySiteParameter.SiteParameterTypeID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteParameterTypeNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanySiteParameter.SiteParameterTypeID.ViewValue = RsWrk("SiteParameterTypeNM")
				Else
					CompanySiteParameter.SiteParameterTypeID.ViewValue = CompanySiteParameter.SiteParameterTypeID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanySiteParameter.SiteParameterTypeID.ViewValue = System.DBNull.Value
			End If
			CompanySiteParameter.SiteParameterTypeID.CssStyle = ""
			CompanySiteParameter.SiteParameterTypeID.CssClass = ""
			CompanySiteParameter.SiteParameterTypeID.ViewCustomAttributes = ""

			' SortOrder
			CompanySiteParameter.SortOrder.ViewValue = CompanySiteParameter.SortOrder.CurrentValue
			CompanySiteParameter.SortOrder.CssStyle = ""
			CompanySiteParameter.SortOrder.CssClass = ""
			CompanySiteParameter.SortOrder.ViewCustomAttributes = ""

			' View refer script
			' CompanyID

			CompanySiteParameter.CompanyID.HrefValue = ""

			' PageID
			CompanySiteParameter.zPageID.HrefValue = ""

			' SiteCategoryGroupID
			CompanySiteParameter.SiteCategoryGroupID.HrefValue = ""

			' SiteParameterTypeID
			CompanySiteParameter.SiteParameterTypeID.HrefValue = ""

			' SortOrder
			CompanySiteParameter.SortOrder.HrefValue = ""

		'
		'  Add Row
		'

		ElseIf CompanySiteParameter.RowType = EW_ROWTYPE_ADD Then ' Add row

			' CompanyID
			CompanySiteParameter.CompanyID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [CompanyID], [CompanyName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Company]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			CompanySiteParameter.CompanyID.EditValue = arwrk

			' PageID
			CompanySiteParameter.zPageID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [PageID], [PageName], '' AS Disp2Fld, [CompanyID] FROM [Page]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [PageName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select", ""}) 
			CompanySiteParameter.zPageID.EditValue = arwrk

			' SiteCategoryGroupID
			CompanySiteParameter.SiteCategoryGroupID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryGroupID], [SiteCategoryGroupNM], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteCategoryGroup]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryGroupNM] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			CompanySiteParameter.SiteCategoryGroupID.EditValue = arwrk

			' SiteParameterTypeID
			CompanySiteParameter.SiteParameterTypeID.EditCustomAttributes = ""
			If CompanySiteParameter.SiteParameterTypeID.SessionValue <> "" Then
				CompanySiteParameter.SiteParameterTypeID.CurrentValue = CompanySiteParameter.SiteParameterTypeID.SessionValue
				CompanySiteParameter.SiteParameterTypeID.OldValue = CompanySiteParameter.SiteParameterTypeID.CurrentValue
			If ew_NotEmpty(CompanySiteParameter.SiteParameterTypeID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteParameterTypeNM] FROM [SiteParameterType] WHERE [SiteParameterTypeID] = " & ew_AdjustSql(CompanySiteParameter.SiteParameterTypeID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteParameterTypeNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanySiteParameter.SiteParameterTypeID.ViewValue = RsWrk("SiteParameterTypeNM")
				Else
					CompanySiteParameter.SiteParameterTypeID.ViewValue = CompanySiteParameter.SiteParameterTypeID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanySiteParameter.SiteParameterTypeID.ViewValue = System.DBNull.Value
			End If
			CompanySiteParameter.SiteParameterTypeID.CssStyle = ""
			CompanySiteParameter.SiteParameterTypeID.CssClass = ""
			CompanySiteParameter.SiteParameterTypeID.ViewCustomAttributes = ""
			Else
			sSqlWrk = "SELECT [SiteParameterTypeID], [SiteParameterTypeNM], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteParameterType]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [SiteParameterTypeNM] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			CompanySiteParameter.SiteParameterTypeID.EditValue = arwrk
			End If

			' SortOrder
			CompanySiteParameter.SortOrder.EditCustomAttributes = ""
			CompanySiteParameter.SortOrder.EditValue = ew_HtmlEncode(CompanySiteParameter.SortOrder.CurrentValue)

		'
		'  Edit Row
		'

		ElseIf CompanySiteParameter.RowType = EW_ROWTYPE_EDIT Then ' Edit row

			' CompanyID
			CompanySiteParameter.CompanyID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [CompanyID], [CompanyName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Company]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			CompanySiteParameter.CompanyID.EditValue = arwrk

			' PageID
			CompanySiteParameter.zPageID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [PageID], [PageName], '' AS Disp2Fld, [CompanyID] FROM [Page]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [PageName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select", ""}) 
			CompanySiteParameter.zPageID.EditValue = arwrk

			' SiteCategoryGroupID
			CompanySiteParameter.SiteCategoryGroupID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryGroupID], [SiteCategoryGroupNM], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteCategoryGroup]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryGroupNM] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			CompanySiteParameter.SiteCategoryGroupID.EditValue = arwrk

			' SiteParameterTypeID
			CompanySiteParameter.SiteParameterTypeID.EditCustomAttributes = ""
			If CompanySiteParameter.SiteParameterTypeID.SessionValue <> "" Then
				CompanySiteParameter.SiteParameterTypeID.CurrentValue = CompanySiteParameter.SiteParameterTypeID.SessionValue
				CompanySiteParameter.SiteParameterTypeID.OldValue = CompanySiteParameter.SiteParameterTypeID.CurrentValue
			If ew_NotEmpty(CompanySiteParameter.SiteParameterTypeID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteParameterTypeNM] FROM [SiteParameterType] WHERE [SiteParameterTypeID] = " & ew_AdjustSql(CompanySiteParameter.SiteParameterTypeID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteParameterTypeNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanySiteParameter.SiteParameterTypeID.ViewValue = RsWrk("SiteParameterTypeNM")
				Else
					CompanySiteParameter.SiteParameterTypeID.ViewValue = CompanySiteParameter.SiteParameterTypeID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanySiteParameter.SiteParameterTypeID.ViewValue = System.DBNull.Value
			End If
			CompanySiteParameter.SiteParameterTypeID.CssStyle = ""
			CompanySiteParameter.SiteParameterTypeID.CssClass = ""
			CompanySiteParameter.SiteParameterTypeID.ViewCustomAttributes = ""
			Else
			sSqlWrk = "SELECT [SiteParameterTypeID], [SiteParameterTypeNM], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteParameterType]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [SiteParameterTypeNM] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			CompanySiteParameter.SiteParameterTypeID.EditValue = arwrk
			End If

			' SortOrder
			CompanySiteParameter.SortOrder.EditCustomAttributes = ""
			CompanySiteParameter.SortOrder.EditValue = ew_HtmlEncode(CompanySiteParameter.SortOrder.CurrentValue)

			' Edit refer script
			' CompanyID

			CompanySiteParameter.CompanyID.HrefValue = ""

			' PageID
			CompanySiteParameter.zPageID.HrefValue = ""

			' SiteCategoryGroupID
			CompanySiteParameter.SiteCategoryGroupID.HrefValue = ""

			' SiteParameterTypeID
			CompanySiteParameter.SiteParameterTypeID.HrefValue = ""

			' SortOrder
			CompanySiteParameter.SortOrder.HrefValue = ""
		End If

		' Row Rendered event
		CompanySiteParameter.Row_Rendered()
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
		If ew_Empty(CompanySiteParameter.CompanyID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter required field - Site"
		End If
		If ew_Empty(CompanySiteParameter.SiteParameterTypeID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter required field -  Parameter"
		End If
		If Not ew_CheckInteger(CompanySiteParameter.SortOrder.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Incorrect integer - Process Order"
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
		sFilter = CompanySiteParameter.KeyFilter
		CompanySiteParameter.CurrentFilter  = sFilter
		sSql = CompanySiteParameter.SQL
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

			' CompanyID
			CompanySiteParameter.CompanyID.SetDbValue(CompanySiteParameter.CompanyID.CurrentValue, 0)
			Rs("CompanyID") = CompanySiteParameter.CompanyID.DbValue

			' PageID
			CompanySiteParameter.zPageID.SetDbValue(CompanySiteParameter.zPageID.CurrentValue, System.DBNull.Value)
			Rs("PageID") = CompanySiteParameter.zPageID.DbValue

			' SiteCategoryGroupID
			CompanySiteParameter.SiteCategoryGroupID.SetDbValue(CompanySiteParameter.SiteCategoryGroupID.CurrentValue, System.DBNull.Value)
			Rs("SiteCategoryGroupID") = CompanySiteParameter.SiteCategoryGroupID.DbValue

			' SiteParameterTypeID
			CompanySiteParameter.SiteParameterTypeID.SetDbValue(CompanySiteParameter.SiteParameterTypeID.CurrentValue, 0)
			Rs("SiteParameterTypeID") = CompanySiteParameter.SiteParameterTypeID.DbValue

			' SortOrder
			CompanySiteParameter.SortOrder.SetDbValue(CompanySiteParameter.SortOrder.CurrentValue, System.DBNull.Value)
			Rs("SortOrder") = CompanySiteParameter.SortOrder.DbValue

			' Row Updating event
			bUpdateRow = CompanySiteParameter.Row_Updating(RsOld, Rs)
			If bUpdateRow Then
				Try
					CompanySiteParameter.Update(Rs)
					EditRow = True
				Catch e As Exception
					If EW_DEBUG_ENABLED Then Throw
					Message = e.Message
					EditRow = False
				End Try
			Else
				If CompanySiteParameter.CancelMessage <> "" Then
					Message = CompanySiteParameter.CancelMessage
					CompanySiteParameter.CancelMessage = ""
				Else
					Message = "Update cancelled"
				End If
				EditRow = False
			End If
		End If

		' Row Updated event
		If EditRow Then
			CompanySiteParameter.Row_Updated(RsOld, Rs)
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

		' CompanyID
		CompanySiteParameter.CompanyID.SetDbValue(CompanySiteParameter.CompanyID.CurrentValue, 0)
		Rs("CompanyID") = CompanySiteParameter.CompanyID.DbValue

		' PageID
		CompanySiteParameter.zPageID.SetDbValue(CompanySiteParameter.zPageID.CurrentValue, System.DBNull.Value)
		Rs("PageID") = CompanySiteParameter.zPageID.DbValue

		' SiteCategoryGroupID
		CompanySiteParameter.SiteCategoryGroupID.SetDbValue(CompanySiteParameter.SiteCategoryGroupID.CurrentValue, System.DBNull.Value)
		Rs("SiteCategoryGroupID") = CompanySiteParameter.SiteCategoryGroupID.DbValue

		' SiteParameterTypeID
		CompanySiteParameter.SiteParameterTypeID.SetDbValue(CompanySiteParameter.SiteParameterTypeID.CurrentValue, 0)
		Rs("SiteParameterTypeID") = CompanySiteParameter.SiteParameterTypeID.DbValue

		' SortOrder
		CompanySiteParameter.SortOrder.SetDbValue(CompanySiteParameter.SortOrder.CurrentValue, System.DBNull.Value)
		Rs("SortOrder") = CompanySiteParameter.SortOrder.DbValue

		' Row Inserting event
		bInsertRow = CompanySiteParameter.Row_Inserting(Rs)
		If bInsertRow Then
			Try	
				CompanySiteParameter.Insert(Rs)
				AddRow = True
			Catch e As Exception
				If EW_DEBUG_ENABLED Then Throw
				Message = e.Message				
				AddRow = False
			End Try
		Else
			If CompanySiteParameter.CancelMessage <> "" Then
				Message = CompanySiteParameter.CancelMessage
				CompanySiteParameter.CancelMessage = ""
			Else
				Message = "Insert cancelled"
			End If
			AddRow = False
		End If
		If AddRow Then
			LastInsertId = Conn.GetLastInsertId()
			CompanySiteParameter.CompanySiteParameterID.DbValue = LastInsertId
			Rs("CompanySiteParameterID") = CompanySiteParameter.CompanySiteParameterID.DbValue		

			' Row Inserted event
			CompanySiteParameter.Row_Inserted(Rs)
			WriteAuditTrailOnAdd(Rs)
		End If
	End Function

	'
	' Load advanced search
	'
	Sub LoadAdvancedSearch()
		CompanySiteParameter.CompanyID.AdvancedSearch.SearchValue = CompanySiteParameter.GetAdvancedSearch("x_CompanyID")
		CompanySiteParameter.zPageID.AdvancedSearch.SearchValue = CompanySiteParameter.GetAdvancedSearch("x_zPageID")
		CompanySiteParameter.SiteCategoryGroupID.AdvancedSearch.SearchValue = CompanySiteParameter.GetAdvancedSearch("x_SiteCategoryGroupID")
		CompanySiteParameter.SiteParameterTypeID.AdvancedSearch.SearchValue = CompanySiteParameter.GetAdvancedSearch("x_SiteParameterTypeID")
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
		If CompanySiteParameter.ExportAll Then
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
		If CompanySiteParameter.Export = "xml" Then
			oXmlDoc = New XmlDocument()
			oXmlTbl = oXmlDoc.CreateElement("table")
			oXmlDoc.AppendChild(oXmlTbl)
		Else
			ew_Write(ew_ExportHeader(CompanySiteParameter.Export))

			' Horizontal format, write header
			If sExportStyle <> "v" OrElse CompanySiteParameter.Export = "csv" Then
				sExportStr = ""
				ew_ExportAddValue(sExportStr, "CompanyID", CompanySiteParameter.Export)
				ew_ExportAddValue(sExportStr, "PageID", CompanySiteParameter.Export)
				ew_ExportAddValue(sExportStr, "SiteCategoryGroupID", CompanySiteParameter.Export)
				ew_ExportAddValue(sExportStr, "SiteParameterTypeID", CompanySiteParameter.Export)
				ew_ExportAddValue(sExportStr, "SortOrder", CompanySiteParameter.Export)
				ew_Write(ew_ExportLine(sExportStr, CompanySiteParameter.Export))
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
				CompanySiteParameter.RowType = EW_ROWTYPE_VIEW ' Render view
				RenderRow()
				If CompanySiteParameter.Export = "xml" Then
					oXmlRec = oXmlDoc.CreateElement("record")
					oXmlTbl.AppendChild(oXmlRec)
					oXmlFld = oXmlDoc.CreateElement("CompanyID") ' CompanyID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(CompanySiteParameter.CompanyID.ExportValue(CompanySiteParameter.Export, CompanySiteParameter.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("zPageID") ' PageID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(CompanySiteParameter.zPageID.ExportValue(CompanySiteParameter.Export, CompanySiteParameter.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("SiteCategoryGroupID") ' SiteCategoryGroupID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(CompanySiteParameter.SiteCategoryGroupID.ExportValue(CompanySiteParameter.Export, CompanySiteParameter.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("SiteParameterTypeID") ' SiteParameterTypeID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(CompanySiteParameter.SiteParameterTypeID.ExportValue(CompanySiteParameter.Export, CompanySiteParameter.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("SortOrder") ' SortOrder
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(CompanySiteParameter.SortOrder.ExportValue(CompanySiteParameter.Export, CompanySiteParameter.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
				Else

					' Vertical format
					If sExportStyle = "v" AndAlso CompanySiteParameter.Export <> "csv" Then
						ew_Write(ew_ExportField("CompanyID", CompanySiteParameter.CompanyID.ExportValue(CompanySiteParameter.Export, CompanySiteParameter.ExportOriginalValue), CompanySiteParameter.Export)) ' CompanyID
						ew_Write(ew_ExportField("PageID", CompanySiteParameter.zPageID.ExportValue(CompanySiteParameter.Export, CompanySiteParameter.ExportOriginalValue), CompanySiteParameter.Export)) ' PageID
						ew_Write(ew_ExportField("SiteCategoryGroupID", CompanySiteParameter.SiteCategoryGroupID.ExportValue(CompanySiteParameter.Export, CompanySiteParameter.ExportOriginalValue), CompanySiteParameter.Export)) ' SiteCategoryGroupID
						ew_Write(ew_ExportField("SiteParameterTypeID", CompanySiteParameter.SiteParameterTypeID.ExportValue(CompanySiteParameter.Export, CompanySiteParameter.ExportOriginalValue), CompanySiteParameter.Export)) ' SiteParameterTypeID
						ew_Write(ew_ExportField("SortOrder", CompanySiteParameter.SortOrder.ExportValue(CompanySiteParameter.Export, CompanySiteParameter.ExportOriginalValue), CompanySiteParameter.Export)) ' SortOrder

					' Horizontal format
					Else
						sExportStr = ""
						ew_ExportAddValue(sExportStr, CompanySiteParameter.CompanyID.ExportValue(CompanySiteParameter.Export, CompanySiteParameter.ExportOriginalValue), CompanySiteParameter.Export) ' CompanyID
						ew_ExportAddValue(sExportStr, CompanySiteParameter.zPageID.ExportValue(CompanySiteParameter.Export, CompanySiteParameter.ExportOriginalValue), CompanySiteParameter.Export) ' PageID
						ew_ExportAddValue(sExportStr, CompanySiteParameter.SiteCategoryGroupID.ExportValue(CompanySiteParameter.Export, CompanySiteParameter.ExportOriginalValue), CompanySiteParameter.Export) ' SiteCategoryGroupID
						ew_ExportAddValue(sExportStr, CompanySiteParameter.SiteParameterTypeID.ExportValue(CompanySiteParameter.Export, CompanySiteParameter.ExportOriginalValue), CompanySiteParameter.Export) ' SiteParameterTypeID
						ew_ExportAddValue(sExportStr, CompanySiteParameter.SortOrder.ExportValue(CompanySiteParameter.Export, CompanySiteParameter.ExportOriginalValue), CompanySiteParameter.Export) ' SortOrder
						ew_Write(ew_ExportLine(sExportStr, CompanySiteParameter.Export))
					End If
				End If
			End If
		Loop

		' Close recordset
		Rs.Close()
		Rs.Dispose()
		If CompanySiteParameter.Export = "xml" Then
			ew_Write("<?xml version=""1.0"" standalone=""yes""?>" & vbcrlf)
			ew_Write(oXmlDoc.OuterXml)
		Else
			ew_Write(ew_ExportFooter(CompanySiteParameter.Export))
		End If
	End Sub

	'
	' Set up Master Detail based on querystring parameter
	'
	Sub SetUpMasterDetail()
		Dim bValidMaster As Boolean = False, sMasterTblVar As String

		' Get the keys for master table
		If ew_Get(EW_TABLE_SHOW_MASTER) <> "" Then
			sMasterTblVar = ew_Get(EW_TABLE_SHOW_MASTER)
			If sMasterTblVar = "" Then
				bValidMaster = True
				sDbMasterFilter = ""
				sDbDetailFilter = ""
			End If
			If sMasterTblVar = "SiteParameterType" Then
				bValidMaster = True
				sDbMasterFilter = CompanySiteParameter.SqlMasterFilter_SiteParameterType
				sDbDetailFilter = CompanySiteParameter.SqlDetailFilter_SiteParameterType
				If ew_Get("SiteParameterTypeID") <> "" Then
					SiteParameterType.SiteParameterTypeID.QueryStringValue = ew_Get("SiteParameterTypeID")
					CompanySiteParameter.SiteParameterTypeID.QueryStringValue = SiteParameterType.SiteParameterTypeID.QueryStringValue
					CompanySiteParameter.SiteParameterTypeID.SessionValue = CompanySiteParameter.SiteParameterTypeID.QueryStringValue
					If Not IsNumeric(SiteParameterType.SiteParameterTypeID.QueryStringValue) Then bValidMaster = False
					sDbMasterFilter = sDbMasterFilter.Replace("@SiteParameterTypeID@", ew_AdjustSql(SiteParameterType.SiteParameterTypeID.QueryStringValue))
					sDbDetailFilter = sDbDetailFilter.Replace("@SiteParameterTypeID@", ew_AdjustSql(SiteParameterType.SiteParameterTypeID.QueryStringValue))
				Else
					bValidMaster = False
				End If
			End If
		End If
		If bValidMaster Then

			' Save current master table
			CompanySiteParameter.CurrentMasterTable = sMasterTblVar

			' Reset start record counter (new master key)
			lStartRec = 1
			CompanySiteParameter.StartRecordNumber = lStartRec
			CompanySiteParameter.MasterFilter = sDbMasterFilter ' Set up master filter
			CompanySiteParameter.DetailFilter = sDbDetailFilter ' Set up detail filter

			' Clear previous master session values
			If sMasterTblVar <> "SiteParameterType" Then
				If CompanySiteParameter.SiteParameterTypeID.QueryStringValue = "" Then CompanySiteParameter.SiteParameterTypeID.SessionValue = ""
			End If
		End If
	End Sub

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "CompanySiteParameter"
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
		Dim table As String = "CompanySiteParameter"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsSrc("CompanySiteParameterID")

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

		' CompanyID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "CompanyID", keyvalue, oldvalue, RsSrc("CompanyID"))

		' PageID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "PageID", keyvalue, oldvalue, RsSrc("PageID"))

		' SiteCategoryGroupID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteCategoryGroupID", keyvalue, oldvalue, RsSrc("SiteCategoryGroupID"))

		' SiteParameterTypeID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteParameterTypeID", keyvalue, oldvalue, RsSrc("SiteParameterTypeID"))

		' SortOrder Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SortOrder", keyvalue, oldvalue, RsSrc("SortOrder"))
	End Sub

	' Write Audit Trail (edit page)
	Sub WriteAuditTrailOnEdit(ByRef RsOld As OrderedDictionary, ByRef RsNew As OrderedDictionary)
		Dim table As String = "CompanySiteParameter"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsOld("CompanySiteParameterID")

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
			fld = CompanySiteParameter.FieldByName(fldname)
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
		CompanySiteParameter_list = New cCompanySiteParameter_list(Me)		
		CompanySiteParameter_list.Page_Init()

		' Page main processing
		CompanySiteParameter_list.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If CompanySiteParameter_list IsNot Nothing Then CompanySiteParameter_list.Dispose()
	End Sub
End Class
