Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class CompanySiteTypeParameter_list
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public CompanySiteTypeParameter_list As cCompanySiteTypeParameter_list

	'
	' Page Class
	'
	Class cCompanySiteTypeParameter_list
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
				If CompanySiteTypeParameter.UseTokenInUrl Then Url = Url & "t=" & CompanySiteTypeParameter.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If CompanySiteTypeParameter.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (CompanySiteTypeParameter.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (CompanySiteTypeParameter.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' CompanySiteTypeParameter
		Public Property CompanySiteTypeParameter() As cCompanySiteTypeParameter
			Get				
				Return ParentPage.CompanySiteTypeParameter
			End Get
			Set(ByVal v As cCompanySiteTypeParameter)
				ParentPage.CompanySiteTypeParameter = v	
			End Set	
		End Property

		' CompanySiteTypeParameter
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
			m_PageObjName = "CompanySiteTypeParameter_list"
			m_PageObjTypeName = "cCompanySiteTypeParameter_list"

			' Table Name
			m_TableName = "CompanySiteTypeParameter"

			' Initialize table object
			CompanySiteTypeParameter = New cCompanySiteTypeParameter(Me)
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
			CompanySiteTypeParameter.Export = ew_Get("export") ' Get export parameter
			ParentPage.gsExport = CompanySiteTypeParameter.Export ' Get export parameter, used in header
			ParentPage.gsExportFile = CompanySiteTypeParameter.TableVar ' Get export file, used in header
			If CompanySiteTypeParameter.Export = "excel" Then
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
			CompanySiteTypeParameter.Dispose()
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
					CompanySiteTypeParameter.CurrentAction = ew_Get("a")

					' Clear inline mode
					If CompanySiteTypeParameter.CurrentAction = "cancel" Then
						ClearInlineMode()
					End If

					' Switch to grid edit mode
					If CompanySiteTypeParameter.CurrentAction = "gridedit" Then
						GridEditMode()
					End If

					' Switch to inline edit mode
					If CompanySiteTypeParameter.CurrentAction = "edit" Then
						InlineEditMode()
					End If

					' Switch to grid add mode
					If CompanySiteTypeParameter.CurrentAction = "gridadd" Then
						GridAddMode()
					End If
				End If
			Else
				CompanySiteTypeParameter.CurrentAction = ObjForm.GetValue("a_list") ' Get action

				' Grid Update
				If CompanySiteTypeParameter.CurrentAction = "gridupdate" AndAlso ew_SameStr(ew_Session(EW_SESSION_INLINE_MODE), "gridedit") Then
					GridUpdate()
				End If

				' Inline Update
				If CompanySiteTypeParameter.CurrentAction = "update" AndAlso ew_SameStr(ew_Session(EW_SESSION_INLINE_MODE), "edit") Then
					InlineUpdate()
				End If

				' Grid Insert
				If CompanySiteTypeParameter.CurrentAction = "gridinsert" AndAlso ew_SameStr(ew_Session(EW_SESSION_INLINE_MODE), "gridadd") Then
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
		If (CompanySiteTypeParameter.RecordsPerPage = -1 OrElse CompanySiteTypeParameter.RecordsPerPage > 0) Then
			lDisplayRecs = CompanySiteTypeParameter.RecordsPerPage ' Restore from Session
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
		CompanySiteTypeParameter.Recordset_Searching(sSrchWhere)

		' Save search criteria
		If sSrchWhere <> "" Then
			If sSrchAdvanced = "" Then ResetAdvancedSearchParms()
			CompanySiteTypeParameter.SearchWhere = sSrchWhere ' Save to Session
			lStartRec = 1 ' Reset start record counter
			CompanySiteTypeParameter.StartRecordNumber = lStartRec
		Else
			RestoreSearchParms()
		End If

		' Build filter
		sFilter = ""

		' Restore master/detail filter
		sDbMasterFilter = CompanySiteTypeParameter.MasterFilter ' Restore master filter
		sDbDetailFilter = CompanySiteTypeParameter.DetailFilter ' Restore detail filter
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
		If CompanySiteTypeParameter.MasterFilter <> "" AndAlso CompanySiteTypeParameter.CurrentMasterTable = "SiteParameterType" Then
			RsMaster = SiteParameterType.LoadRs(sDbMasterFilter)
			bMasterRecordExists = RsMaster IsNot Nothing
			If Not bMasterRecordExists Then
				CompanySiteTypeParameter.MasterFilter = "" ' Clear master filter
				CompanySiteTypeParameter.DetailFilter = "" ' Clear detail filter
				Message = "No records found" ' Set no record found
			Page_Terminate(CompanySiteTypeParameter.ReturnUrl) ' Return to caller
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
		CompanySiteTypeParameter.SessionWhere = sFilter
		CompanySiteTypeParameter.CurrentFilter = ""

		' Export Data only
		If CompanySiteTypeParameter.Export = "html" OrElse CompanySiteTypeParameter.Export = "csv" OrElse CompanySiteTypeParameter.Export = "word" OrElse CompanySiteTypeParameter.Export = "excel" OrElse CompanySiteTypeParameter.Export = "xml" Then
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
			CompanySiteTypeParameter.RecordsPerPage = lDisplayRecs ' Save to Session

			' Reset start position
			lStartRec = 1
			CompanySiteTypeParameter.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	'  Exit out of inline mode
	'
	Sub ClearInlineMode()
		CompanySiteTypeParameter.SetKey("CompanySiteTypeParameterID", "") ' Clear inline edit key
		CompanySiteTypeParameter.CurrentAction = "" ' Clear action
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
		If ew_Get("CompanySiteTypeParameterID") <> "" Then
			CompanySiteTypeParameter.CompanySiteTypeParameterID.QueryStringValue = ew_Get("CompanySiteTypeParameterID")
		Else
			bInlineEdit = False
		End If
		If bInlineEdit AndAlso LoadRow() Then
			CompanySiteTypeParameter.SetKey("CompanySiteTypeParameterID", CompanySiteTypeParameter.CompanySiteTypeParameterID.CurrentValue) ' Set up inline edit key
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
				CompanySiteTypeParameter.SendEmail = True ' Send email on update success
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
			CompanySiteTypeParameter.EventCancelled = True ' Cancel event
			CompanySiteTypeParameter.CurrentAction = "edit" ' Stay in edit mode
		End If
	End Sub

	'
	' Check inline edit key
	'
	Function CheckInlineEditKey() As Boolean
		If Not ew_SameStr(CompanySiteTypeParameter.GetKey("CompanySiteTypeParameterID"), CompanySiteTypeParameter.CompanySiteTypeParameterID.CurrentValue) Then
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
		CompanySiteTypeParameter.CurrentFilter = BuildKeyFilter()
		sSql = CompanySiteTypeParameter.SQL
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
					CompanySiteTypeParameter.SendEmail = False ' Do not send email on update success
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
			CompanySiteTypeParameter.EventCancelled = True ' Set event cancelled
			CompanySiteTypeParameter.CurrentAction = "gridedit" ' Stay in gridedit mode
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
				sFilter = CompanySiteTypeParameter.KeyFilter
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
			CompanySiteTypeParameter.CompanySiteTypeParameterID.FormValue = arKeyFlds(0)
			If Not IsNumeric(CompanySiteTypeParameter.CompanySiteTypeParameterID.FormValue) Then	Return False
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
				CompanySiteTypeParameter.SendEmail = False ' Do not send email on insert success

				' Validate Form
				If Not ValidateForm() Then
					bGridInsert = False ' Form error, reset action
					Message = ParentPage.gsFormError
				Else
					bGridInsert = AddRow() ' Insert this row
				End If
				If bGridInsert Then
					If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
					sKey = sKey & CompanySiteTypeParameter.CompanySiteTypeParameterID.CurrentValue

					' Add filter for this record
					sFilter = CompanySiteTypeParameter.KeyFilter
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
			CompanySiteTypeParameter.CurrentFilter = sWrkFilter
			sSql = CompanySiteTypeParameter.SQL
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
			CompanySiteTypeParameter.EventCancelled = True ' Set event cancelled
			CompanySiteTypeParameter.CurrentAction = "gridadd" ' Stay in gridadd mode
		End If
	End Sub

	Function EmptyRow() As Boolean
		Dim Empty As Boolean = True
		If Empty Then Empty = ew_SameStr(CompanySiteTypeParameter.SiteParameterTypeID.CurrentValue, CompanySiteTypeParameter.SiteParameterTypeID.OldValue)
		If Empty Then Empty = ew_SameStr(CompanySiteTypeParameter.CompanyID.CurrentValue, CompanySiteTypeParameter.CompanyID.OldValue)
		If Empty Then Empty = ew_SameStr(CompanySiteTypeParameter.SiteCategoryTypeID.CurrentValue, CompanySiteTypeParameter.SiteCategoryTypeID.OldValue)
		If Empty Then Empty = ew_SameStr(CompanySiteTypeParameter.SiteCategoryGroupID.CurrentValue, CompanySiteTypeParameter.SiteCategoryGroupID.OldValue)
		If Empty Then Empty = ew_SameStr(CompanySiteTypeParameter.SiteCategoryID.CurrentValue, CompanySiteTypeParameter.SiteCategoryID.OldValue)
		If Empty Then Empty = ew_SameStr(CompanySiteTypeParameter.SortOrder.CurrentValue, CompanySiteTypeParameter.SortOrder.OldValue)
		Return Empty
	End Function

	'
	' Restore form values for current row
	'
	Sub RestoreCurrentRowFormValues(idx As Integer)
		Dim sKey As String, arKeyFlds() As String

		' Get row based on current index
		ObjForm.Index = idx
		If CompanySiteTypeParameter.CurrentAction = "gridadd" Then
			LoadFormValues() ' Load form values
		End If
		If CompanySiteTypeParameter.CurrentAction = "gridedit" Then
			sKey = ObjForm.GetValue("k_key")
			arKeyFlds = sKey.Split(New Char() {Convert.ToChar(EW_COMPOSITE_KEY_SEPARATOR)})
			If arKeyFlds.Length >= 1 Then
				If ew_SameStr(arKeyFlds(0), CompanySiteTypeParameter.CompanySiteTypeParameterID.CurrentValue) Then
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
		BuildSearchSql(sWhere, CompanySiteTypeParameter.SiteParameterTypeID, False) ' SiteParameterTypeID
		BuildSearchSql(sWhere, CompanySiteTypeParameter.CompanyID, False) ' CompanyID
		BuildSearchSql(sWhere, CompanySiteTypeParameter.SiteCategoryTypeID, False) ' SiteCategoryTypeID
		BuildSearchSql(sWhere, CompanySiteTypeParameter.SiteCategoryGroupID, False) ' SiteCategoryGroupID
		BuildSearchSql(sWhere, CompanySiteTypeParameter.SiteCategoryID, False) ' SiteCategoryID

		' Set up search parm
		If sWhere <> "" Then
			SetSearchParm(CompanySiteTypeParameter.SiteParameterTypeID) ' SiteParameterTypeID
			SetSearchParm(CompanySiteTypeParameter.CompanyID) ' CompanyID
			SetSearchParm(CompanySiteTypeParameter.SiteCategoryTypeID) ' SiteCategoryTypeID
			SetSearchParm(CompanySiteTypeParameter.SiteCategoryGroupID) ' SiteCategoryGroupID
			SetSearchParm(CompanySiteTypeParameter.SiteCategoryID) ' SiteCategoryID
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
		CompanySiteTypeParameter.SetAdvancedSearch("x_" & FldParm, Fld.AdvancedSearch.SearchValue)
		CompanySiteTypeParameter.SetAdvancedSearch("z_" & FldParm, Fld.AdvancedSearch.SearchOperator)
		CompanySiteTypeParameter.SetAdvancedSearch("v_" & FldParm, Fld.AdvancedSearch.SearchCondition)
		CompanySiteTypeParameter.SetAdvancedSearch("y_" & FldParm, Fld.AdvancedSearch.SearchValue2)
		CompanySiteTypeParameter.SetAdvancedSearch("w_" & FldParm, Fld.AdvancedSearch.SearchOperator2)
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
		CompanySiteTypeParameter.SearchWhere = sSrchWhere

		' Clear advanced search parameters
		ResetAdvancedSearchParms()
	End Sub

	'
	' Clear all advanced search parameters
	'
	Sub ResetAdvancedSearchParms()
		CompanySiteTypeParameter.SetAdvancedSearch("x_SiteParameterTypeID", "")
		CompanySiteTypeParameter.SetAdvancedSearch("x_CompanyID", "")
		CompanySiteTypeParameter.SetAdvancedSearch("x_SiteCategoryTypeID", "")
		CompanySiteTypeParameter.SetAdvancedSearch("x_SiteCategoryGroupID", "")
		CompanySiteTypeParameter.SetAdvancedSearch("x_SiteCategoryID", "")
	End Sub

	'
	' Restore all search parameters
	'
	Sub RestoreSearchParms()
		sSrchWhere = CompanySiteTypeParameter.SearchWhere

		' Restore advanced search settings
		If ParentPage.gsSearchError = "" Then
			RestoreAdvancedSearchParms()
		End If
	End Sub

	'
	' Restore all advanced search parameters
	'
	Sub RestoreAdvancedSearchParms()
		CompanySiteTypeParameter.SiteParameterTypeID.AdvancedSearch.SearchValue = CompanySiteTypeParameter.GetAdvancedSearch("x_SiteParameterTypeID")
		CompanySiteTypeParameter.CompanyID.AdvancedSearch.SearchValue = CompanySiteTypeParameter.GetAdvancedSearch("x_CompanyID")
		CompanySiteTypeParameter.SiteCategoryTypeID.AdvancedSearch.SearchValue = CompanySiteTypeParameter.GetAdvancedSearch("x_SiteCategoryTypeID")
		CompanySiteTypeParameter.SiteCategoryGroupID.AdvancedSearch.SearchValue = CompanySiteTypeParameter.GetAdvancedSearch("x_SiteCategoryGroupID")
		CompanySiteTypeParameter.SiteCategoryID.AdvancedSearch.SearchValue = CompanySiteTypeParameter.GetAdvancedSearch("x_SiteCategoryID")
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
			CompanySiteTypeParameter.CurrentOrder = ew_Get("order")
			CompanySiteTypeParameter.CurrentOrderType = ew_Get("ordertype")
			CompanySiteTypeParameter.UpdateSort(CompanySiteTypeParameter.SiteParameterTypeID) ' SiteParameterTypeID
			CompanySiteTypeParameter.UpdateSort(CompanySiteTypeParameter.CompanyID) ' CompanyID
			CompanySiteTypeParameter.UpdateSort(CompanySiteTypeParameter.SiteCategoryTypeID) ' SiteCategoryTypeID
			CompanySiteTypeParameter.UpdateSort(CompanySiteTypeParameter.SiteCategoryGroupID) ' SiteCategoryGroupID
			CompanySiteTypeParameter.UpdateSort(CompanySiteTypeParameter.SiteCategoryID) ' SiteCategoryID
			CompanySiteTypeParameter.UpdateSort(CompanySiteTypeParameter.SortOrder) ' SortOrder
			CompanySiteTypeParameter.StartRecordNumber = 1 ' Reset start position
		End If
	End Sub

	'
	' Load Sort Order parameters
	'
	Sub LoadSortOrder()
		Dim sOrderBy As String
		sOrderBy = CompanySiteTypeParameter.SessionOrderBy ' Get order by from Session
		If sOrderBy = "" Then
			If CompanySiteTypeParameter.SqlOrderBy <> "" Then
				sOrderBy = CompanySiteTypeParameter.SqlOrderBy
				CompanySiteTypeParameter.SessionOrderBy = sOrderBy
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
				CompanySiteTypeParameter.CurrentMasterTable = "" ' Clear master table
				CompanySiteTypeParameter.MasterFilter = "" ' Clear master filter
				sDbMasterFilter = ""
				CompanySiteTypeParameter.DetailFilter = "" ' Clear detail filter
				sDbDetailFilter = ""
				CompanySiteTypeParameter.SiteCategoryTypeID.SessionValue = ""
			End If

			' Reset sort criteria
			If ew_SameText(sCmd, "resetsort") Then
				Dim sOrderBy As String = ""
				CompanySiteTypeParameter.SessionOrderBy = sOrderBy
				CompanySiteTypeParameter.SiteParameterTypeID.Sort = ""
				CompanySiteTypeParameter.CompanyID.Sort = ""
				CompanySiteTypeParameter.SiteCategoryTypeID.Sort = ""
				CompanySiteTypeParameter.SiteCategoryGroupID.Sort = ""
				CompanySiteTypeParameter.SiteCategoryID.Sort = ""
				CompanySiteTypeParameter.SortOrder.Sort = ""
			End If

			' Reset start position
			lStartRec = 1
			CompanySiteTypeParameter.StartRecordNumber = lStartRec
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
				CompanySiteTypeParameter.StartRecordNumber = lStartRec
			ElseIf ew_Get(EW_TABLE_PAGE_NO) <> "" AndAlso IsNumeric(ew_Get(EW_TABLE_PAGE_NO)) Then
				nPageNo = ew_ConvertToInt(ew_Get(EW_TABLE_PAGE_NO))
				lStartRec = (nPageNo-1)*lDisplayRecs+1
				If lStartRec <= 0 Then
					lStartRec = 1
				ElseIf lStartRec >= ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 Then
					lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1
				End If
				CompanySiteTypeParameter.StartRecordNumber = lStartRec
			End If
		End If
		lStartRec = CompanySiteTypeParameter.StartRecordNumber

		' Check if correct start record counter
		If lStartRec <= 0 Then ' Avoid invalid start record counter
			lStartRec = 1 ' Reset start record counter
			CompanySiteTypeParameter.StartRecordNumber = lStartRec
		ElseIf lStartRec > lTotalRecs Then ' Avoid starting record > total records
			lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to last page first record
			CompanySiteTypeParameter.StartRecordNumber = lStartRec
		ElseIf (lStartRec-1) Mod lDisplayRecs <> 0 Then
			lStartRec = ((lStartRec-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to page boundary
			CompanySiteTypeParameter.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	' Load default values
	'
	Sub LoadDefaultValues()
		CompanySiteTypeParameter.SiteParameterTypeID.OldValue = CompanySiteTypeParameter.SiteParameterTypeID.CurrentValue
		CompanySiteTypeParameter.CompanyID.OldValue = CompanySiteTypeParameter.CompanyID.CurrentValue
		CompanySiteTypeParameter.SiteCategoryTypeID.OldValue = CompanySiteTypeParameter.SiteCategoryTypeID.CurrentValue
		CompanySiteTypeParameter.SiteCategoryGroupID.OldValue = CompanySiteTypeParameter.SiteCategoryGroupID.CurrentValue
		CompanySiteTypeParameter.SiteCategoryID.OldValue = CompanySiteTypeParameter.SiteCategoryID.CurrentValue
		CompanySiteTypeParameter.SortOrder.CurrentValue = 999
		CompanySiteTypeParameter.SortOrder.OldValue = CompanySiteTypeParameter.SortOrder.CurrentValue
	End Sub

	'
	'  Load search values for validation
	'
	Sub LoadSearchValues()
		CompanySiteTypeParameter.SiteParameterTypeID.AdvancedSearch.SearchValue = ew_Get("x_SiteParameterTypeID")
    	CompanySiteTypeParameter.SiteParameterTypeID.AdvancedSearch.SearchOperator = ew_Get("z_SiteParameterTypeID")
		CompanySiteTypeParameter.CompanyID.AdvancedSearch.SearchValue = ew_Get("x_CompanyID")
    	CompanySiteTypeParameter.CompanyID.AdvancedSearch.SearchOperator = ew_Get("z_CompanyID")
		CompanySiteTypeParameter.SiteCategoryTypeID.AdvancedSearch.SearchValue = ew_Get("x_SiteCategoryTypeID")
    	CompanySiteTypeParameter.SiteCategoryTypeID.AdvancedSearch.SearchOperator = ew_Get("z_SiteCategoryTypeID")
		CompanySiteTypeParameter.SiteCategoryGroupID.AdvancedSearch.SearchValue = ew_Get("x_SiteCategoryGroupID")
    	CompanySiteTypeParameter.SiteCategoryGroupID.AdvancedSearch.SearchOperator = ew_Get("z_SiteCategoryGroupID")
		CompanySiteTypeParameter.SiteCategoryID.AdvancedSearch.SearchValue = ew_Get("x_SiteCategoryID")
    	CompanySiteTypeParameter.SiteCategoryID.AdvancedSearch.SearchOperator = ew_Get("z_SiteCategoryID")
	End Sub

	'
	' Load form values
	'
	Sub LoadFormValues()
		CompanySiteTypeParameter.SiteParameterTypeID.FormValue = ObjForm.GetValue("x_SiteParameterTypeID")
		CompanySiteTypeParameter.SiteParameterTypeID.OldValue = ObjForm.GetValue("o_SiteParameterTypeID")
		CompanySiteTypeParameter.CompanyID.FormValue = ObjForm.GetValue("x_CompanyID")
		CompanySiteTypeParameter.CompanyID.OldValue = ObjForm.GetValue("o_CompanyID")
		CompanySiteTypeParameter.SiteCategoryTypeID.FormValue = ObjForm.GetValue("x_SiteCategoryTypeID")
		CompanySiteTypeParameter.SiteCategoryTypeID.OldValue = ObjForm.GetValue("o_SiteCategoryTypeID")
		CompanySiteTypeParameter.SiteCategoryGroupID.FormValue = ObjForm.GetValue("x_SiteCategoryGroupID")
		CompanySiteTypeParameter.SiteCategoryGroupID.OldValue = ObjForm.GetValue("o_SiteCategoryGroupID")
		CompanySiteTypeParameter.SiteCategoryID.FormValue = ObjForm.GetValue("x_SiteCategoryID")
		CompanySiteTypeParameter.SiteCategoryID.OldValue = ObjForm.GetValue("o_SiteCategoryID")
		CompanySiteTypeParameter.SortOrder.FormValue = ObjForm.GetValue("x_SortOrder")
		CompanySiteTypeParameter.SortOrder.OldValue = ObjForm.GetValue("o_SortOrder")
		CompanySiteTypeParameter.CompanySiteTypeParameterID.FormValue = ObjForm.GetValue("x_CompanySiteTypeParameterID")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		CompanySiteTypeParameter.SiteParameterTypeID.CurrentValue = CompanySiteTypeParameter.SiteParameterTypeID.FormValue
		CompanySiteTypeParameter.CompanyID.CurrentValue = CompanySiteTypeParameter.CompanyID.FormValue
		CompanySiteTypeParameter.SiteCategoryTypeID.CurrentValue = CompanySiteTypeParameter.SiteCategoryTypeID.FormValue
		CompanySiteTypeParameter.SiteCategoryGroupID.CurrentValue = CompanySiteTypeParameter.SiteCategoryGroupID.FormValue
		CompanySiteTypeParameter.SiteCategoryID.CurrentValue = CompanySiteTypeParameter.SiteCategoryID.FormValue
		CompanySiteTypeParameter.SortOrder.CurrentValue = CompanySiteTypeParameter.SortOrder.FormValue
		CompanySiteTypeParameter.CompanySiteTypeParameterID.CurrentValue = CompanySiteTypeParameter.CompanySiteTypeParameterID.FormValue
	End Sub

	'
	' Load recordset
	'
	Function LoadRecordset() As OleDbDataReader

		' Recordset Selecting event
		CompanySiteTypeParameter.Recordset_Selecting(CompanySiteTypeParameter.CurrentFilter)

		' Load list page SQL
		Dim sSql As String = CompanySiteTypeParameter.ListSQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)

		' Count
		lTotalRecs = -1		
		Try			
			If sSql.StartsWith("SELECT * FROM ", StringComparison.InvariantCultureIgnoreCase) AndAlso _				
				ew_Empty(CompanySiteTypeParameter.SqlGroupBy) AndAlso _
				ew_Empty(CompanySiteTypeParameter.SqlHaving) Then
				Dim sCntSql As String = CompanySiteTypeParameter.SelectCountSQL

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
		CompanySiteTypeParameter.Recordset_Selected(Rs)
		Return Rs
	End Function

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = CompanySiteTypeParameter.KeyFilter

		' Row Selecting event
		CompanySiteTypeParameter.Row_Selecting(sFilter)

		' Load SQL based on filter
		CompanySiteTypeParameter.CurrentFilter = sFilter
		Dim sSql As String = CompanySiteTypeParameter.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				CompanySiteTypeParameter.Row_Selected(RsRow)
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
		CompanySiteTypeParameter.CompanySiteTypeParameterID.DbValue = RsRow("CompanySiteTypeParameterID")
		CompanySiteTypeParameter.SiteParameterTypeID.DbValue = RsRow("SiteParameterTypeID")
		CompanySiteTypeParameter.CompanyID.DbValue = RsRow("CompanyID")
		CompanySiteTypeParameter.SiteCategoryTypeID.DbValue = RsRow("SiteCategoryTypeID")
		CompanySiteTypeParameter.SiteCategoryGroupID.DbValue = RsRow("SiteCategoryGroupID")
		CompanySiteTypeParameter.SiteCategoryID.DbValue = RsRow("SiteCategoryID")
		CompanySiteTypeParameter.SortOrder.DbValue = RsRow("SortOrder")
		CompanySiteTypeParameter.ParameterValue.DbValue = RsRow("ParameterValue")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		CompanySiteTypeParameter.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' SiteParameterTypeID

		CompanySiteTypeParameter.SiteParameterTypeID.CellCssStyle = ""
		CompanySiteTypeParameter.SiteParameterTypeID.CellCssClass = ""

		' CompanyID
		CompanySiteTypeParameter.CompanyID.CellCssStyle = ""
		CompanySiteTypeParameter.CompanyID.CellCssClass = ""

		' SiteCategoryTypeID
		CompanySiteTypeParameter.SiteCategoryTypeID.CellCssStyle = ""
		CompanySiteTypeParameter.SiteCategoryTypeID.CellCssClass = ""

		' SiteCategoryGroupID
		CompanySiteTypeParameter.SiteCategoryGroupID.CellCssStyle = ""
		CompanySiteTypeParameter.SiteCategoryGroupID.CellCssClass = ""

		' SiteCategoryID
		CompanySiteTypeParameter.SiteCategoryID.CellCssStyle = ""
		CompanySiteTypeParameter.SiteCategoryID.CellCssClass = ""

		' SortOrder
		CompanySiteTypeParameter.SortOrder.CellCssStyle = ""
		CompanySiteTypeParameter.SortOrder.CellCssClass = ""

		'
		'  View  Row
		'

		If CompanySiteTypeParameter.RowType = EW_ROWTYPE_VIEW Then ' View row

			' SiteParameterTypeID
			If ew_NotEmpty(CompanySiteTypeParameter.SiteParameterTypeID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteParameterTypeNM] FROM [SiteParameterType] WHERE [SiteParameterTypeID] = " & ew_AdjustSql(CompanySiteTypeParameter.SiteParameterTypeID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteParameterTypeNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanySiteTypeParameter.SiteParameterTypeID.ViewValue = RsWrk("SiteParameterTypeNM")
				Else
					CompanySiteTypeParameter.SiteParameterTypeID.ViewValue = CompanySiteTypeParameter.SiteParameterTypeID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanySiteTypeParameter.SiteParameterTypeID.ViewValue = System.DBNull.Value
			End If
			CompanySiteTypeParameter.SiteParameterTypeID.CssStyle = ""
			CompanySiteTypeParameter.SiteParameterTypeID.CssClass = ""
			CompanySiteTypeParameter.SiteParameterTypeID.ViewCustomAttributes = ""

			' CompanyID
			If ew_NotEmpty(CompanySiteTypeParameter.CompanyID.CurrentValue) Then
				sSqlWrk = "SELECT [CompanyName] FROM [Company] WHERE [CompanyID] = " & ew_AdjustSql(CompanySiteTypeParameter.CompanyID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanySiteTypeParameter.CompanyID.ViewValue = RsWrk("CompanyName")
				Else
					CompanySiteTypeParameter.CompanyID.ViewValue = CompanySiteTypeParameter.CompanyID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanySiteTypeParameter.CompanyID.ViewValue = System.DBNull.Value
			End If
			CompanySiteTypeParameter.CompanyID.CssStyle = ""
			CompanySiteTypeParameter.CompanyID.CssClass = ""
			CompanySiteTypeParameter.CompanyID.ViewCustomAttributes = ""

			' SiteCategoryTypeID
			If ew_NotEmpty(CompanySiteTypeParameter.SiteCategoryTypeID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteCategoryTypeNM] FROM [SiteCategoryType] WHERE [SiteCategoryTypeID] = " & ew_AdjustSql(CompanySiteTypeParameter.SiteCategoryTypeID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryTypeNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanySiteTypeParameter.SiteCategoryTypeID.ViewValue = RsWrk("SiteCategoryTypeNM")
				Else
					CompanySiteTypeParameter.SiteCategoryTypeID.ViewValue = CompanySiteTypeParameter.SiteCategoryTypeID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanySiteTypeParameter.SiteCategoryTypeID.ViewValue = System.DBNull.Value
			End If
			CompanySiteTypeParameter.SiteCategoryTypeID.CssStyle = ""
			CompanySiteTypeParameter.SiteCategoryTypeID.CssClass = ""
			CompanySiteTypeParameter.SiteCategoryTypeID.ViewCustomAttributes = ""

			' SiteCategoryGroupID
			If ew_NotEmpty(CompanySiteTypeParameter.SiteCategoryGroupID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteCategoryGroupNM] FROM [SiteCategoryGroup] WHERE [SiteCategoryGroupID] = " & ew_AdjustSql(CompanySiteTypeParameter.SiteCategoryGroupID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryGroupNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanySiteTypeParameter.SiteCategoryGroupID.ViewValue = RsWrk("SiteCategoryGroupNM")
				Else
					CompanySiteTypeParameter.SiteCategoryGroupID.ViewValue = CompanySiteTypeParameter.SiteCategoryGroupID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanySiteTypeParameter.SiteCategoryGroupID.ViewValue = System.DBNull.Value
			End If
			CompanySiteTypeParameter.SiteCategoryGroupID.CssStyle = ""
			CompanySiteTypeParameter.SiteCategoryGroupID.CssClass = ""
			CompanySiteTypeParameter.SiteCategoryGroupID.ViewCustomAttributes = ""

			' SiteCategoryID
			If ew_NotEmpty(CompanySiteTypeParameter.SiteCategoryID.CurrentValue) Then
				sSqlWrk = "SELECT [CategoryName] FROM [SiteCategory] WHERE [SiteCategoryID] = " & ew_AdjustSql(CompanySiteTypeParameter.SiteCategoryID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [CategoryName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanySiteTypeParameter.SiteCategoryID.ViewValue = RsWrk("CategoryName")
				Else
					CompanySiteTypeParameter.SiteCategoryID.ViewValue = CompanySiteTypeParameter.SiteCategoryID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanySiteTypeParameter.SiteCategoryID.ViewValue = System.DBNull.Value
			End If
			CompanySiteTypeParameter.SiteCategoryID.CssStyle = ""
			CompanySiteTypeParameter.SiteCategoryID.CssClass = ""
			CompanySiteTypeParameter.SiteCategoryID.ViewCustomAttributes = ""

			' SortOrder
			CompanySiteTypeParameter.SortOrder.ViewValue = CompanySiteTypeParameter.SortOrder.CurrentValue
			CompanySiteTypeParameter.SortOrder.CssStyle = ""
			CompanySiteTypeParameter.SortOrder.CssClass = ""
			CompanySiteTypeParameter.SortOrder.ViewCustomAttributes = ""

			' View refer script
			' SiteParameterTypeID

			CompanySiteTypeParameter.SiteParameterTypeID.HrefValue = ""

			' CompanyID
			CompanySiteTypeParameter.CompanyID.HrefValue = ""

			' SiteCategoryTypeID
			CompanySiteTypeParameter.SiteCategoryTypeID.HrefValue = ""

			' SiteCategoryGroupID
			CompanySiteTypeParameter.SiteCategoryGroupID.HrefValue = ""

			' SiteCategoryID
			CompanySiteTypeParameter.SiteCategoryID.HrefValue = ""

			' SortOrder
			CompanySiteTypeParameter.SortOrder.HrefValue = ""

		'
		'  Add Row
		'

		ElseIf CompanySiteTypeParameter.RowType = EW_ROWTYPE_ADD Then ' Add row

			' SiteParameterTypeID
			CompanySiteTypeParameter.SiteParameterTypeID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteParameterTypeID], [SiteParameterTypeNM], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteParameterType]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [SiteParameterTypeNM] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			CompanySiteTypeParameter.SiteParameterTypeID.EditValue = arwrk

			' CompanyID
			CompanySiteTypeParameter.CompanyID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [CompanyID], [CompanyName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Company]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			CompanySiteTypeParameter.CompanyID.EditValue = arwrk

			' SiteCategoryTypeID
			CompanySiteTypeParameter.SiteCategoryTypeID.EditCustomAttributes = ""
			If CompanySiteTypeParameter.SiteCategoryTypeID.SessionValue <> "" Then
				CompanySiteTypeParameter.SiteCategoryTypeID.CurrentValue = CompanySiteTypeParameter.SiteCategoryTypeID.SessionValue
				CompanySiteTypeParameter.SiteCategoryTypeID.OldValue = CompanySiteTypeParameter.SiteCategoryTypeID.CurrentValue
			If ew_NotEmpty(CompanySiteTypeParameter.SiteCategoryTypeID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteCategoryTypeNM] FROM [SiteCategoryType] WHERE [SiteCategoryTypeID] = " & ew_AdjustSql(CompanySiteTypeParameter.SiteCategoryTypeID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryTypeNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanySiteTypeParameter.SiteCategoryTypeID.ViewValue = RsWrk("SiteCategoryTypeNM")
				Else
					CompanySiteTypeParameter.SiteCategoryTypeID.ViewValue = CompanySiteTypeParameter.SiteCategoryTypeID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanySiteTypeParameter.SiteCategoryTypeID.ViewValue = System.DBNull.Value
			End If
			CompanySiteTypeParameter.SiteCategoryTypeID.CssStyle = ""
			CompanySiteTypeParameter.SiteCategoryTypeID.CssClass = ""
			CompanySiteTypeParameter.SiteCategoryTypeID.ViewCustomAttributes = ""
			Else
			sSqlWrk = "SELECT [SiteCategoryTypeID], [SiteCategoryTypeNM], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteCategoryType]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryTypeNM] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			CompanySiteTypeParameter.SiteCategoryTypeID.EditValue = arwrk
			End If

			' SiteCategoryGroupID
			CompanySiteTypeParameter.SiteCategoryGroupID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryGroupID], [SiteCategoryGroupNM], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteCategoryGroup]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryGroupNM] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			CompanySiteTypeParameter.SiteCategoryGroupID.EditValue = arwrk

			' SiteCategoryID
			CompanySiteTypeParameter.SiteCategoryID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryID], [CategoryName], '' AS Disp2Fld, [SiteCategoryTypeID] FROM [SiteCategory]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [CategoryName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select", ""}) 
			CompanySiteTypeParameter.SiteCategoryID.EditValue = arwrk

			' SortOrder
			CompanySiteTypeParameter.SortOrder.EditCustomAttributes = ""
			CompanySiteTypeParameter.SortOrder.EditValue = ew_HtmlEncode(CompanySiteTypeParameter.SortOrder.CurrentValue)

		'
		'  Edit Row
		'

		ElseIf CompanySiteTypeParameter.RowType = EW_ROWTYPE_EDIT Then ' Edit row

			' SiteParameterTypeID
			CompanySiteTypeParameter.SiteParameterTypeID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteParameterTypeID], [SiteParameterTypeNM], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteParameterType]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [SiteParameterTypeNM] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			CompanySiteTypeParameter.SiteParameterTypeID.EditValue = arwrk

			' CompanyID
			CompanySiteTypeParameter.CompanyID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [CompanyID], [CompanyName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Company]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			CompanySiteTypeParameter.CompanyID.EditValue = arwrk

			' SiteCategoryTypeID
			CompanySiteTypeParameter.SiteCategoryTypeID.EditCustomAttributes = ""
			If CompanySiteTypeParameter.SiteCategoryTypeID.SessionValue <> "" Then
				CompanySiteTypeParameter.SiteCategoryTypeID.CurrentValue = CompanySiteTypeParameter.SiteCategoryTypeID.SessionValue
				CompanySiteTypeParameter.SiteCategoryTypeID.OldValue = CompanySiteTypeParameter.SiteCategoryTypeID.CurrentValue
			If ew_NotEmpty(CompanySiteTypeParameter.SiteCategoryTypeID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteCategoryTypeNM] FROM [SiteCategoryType] WHERE [SiteCategoryTypeID] = " & ew_AdjustSql(CompanySiteTypeParameter.SiteCategoryTypeID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryTypeNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanySiteTypeParameter.SiteCategoryTypeID.ViewValue = RsWrk("SiteCategoryTypeNM")
				Else
					CompanySiteTypeParameter.SiteCategoryTypeID.ViewValue = CompanySiteTypeParameter.SiteCategoryTypeID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanySiteTypeParameter.SiteCategoryTypeID.ViewValue = System.DBNull.Value
			End If
			CompanySiteTypeParameter.SiteCategoryTypeID.CssStyle = ""
			CompanySiteTypeParameter.SiteCategoryTypeID.CssClass = ""
			CompanySiteTypeParameter.SiteCategoryTypeID.ViewCustomAttributes = ""
			Else
			sSqlWrk = "SELECT [SiteCategoryTypeID], [SiteCategoryTypeNM], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteCategoryType]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryTypeNM] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			CompanySiteTypeParameter.SiteCategoryTypeID.EditValue = arwrk
			End If

			' SiteCategoryGroupID
			CompanySiteTypeParameter.SiteCategoryGroupID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryGroupID], [SiteCategoryGroupNM], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteCategoryGroup]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryGroupNM] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			CompanySiteTypeParameter.SiteCategoryGroupID.EditValue = arwrk

			' SiteCategoryID
			CompanySiteTypeParameter.SiteCategoryID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryID], [CategoryName], '' AS Disp2Fld, [SiteCategoryTypeID] FROM [SiteCategory]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [CategoryName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select", ""}) 
			CompanySiteTypeParameter.SiteCategoryID.EditValue = arwrk

			' SortOrder
			CompanySiteTypeParameter.SortOrder.EditCustomAttributes = ""
			CompanySiteTypeParameter.SortOrder.EditValue = ew_HtmlEncode(CompanySiteTypeParameter.SortOrder.CurrentValue)

			' Edit refer script
			' SiteParameterTypeID

			CompanySiteTypeParameter.SiteParameterTypeID.HrefValue = ""

			' CompanyID
			CompanySiteTypeParameter.CompanyID.HrefValue = ""

			' SiteCategoryTypeID
			CompanySiteTypeParameter.SiteCategoryTypeID.HrefValue = ""

			' SiteCategoryGroupID
			CompanySiteTypeParameter.SiteCategoryGroupID.HrefValue = ""

			' SiteCategoryID
			CompanySiteTypeParameter.SiteCategoryID.HrefValue = ""

			' SortOrder
			CompanySiteTypeParameter.SortOrder.HrefValue = ""
		End If

		' Row Rendered event
		CompanySiteTypeParameter.Row_Rendered()
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
		If ew_Empty(CompanySiteTypeParameter.SiteParameterTypeID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter required field - Parameter"
		End If
		If Not ew_CheckInteger(CompanySiteTypeParameter.SortOrder.FormValue) Then
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
		sFilter = CompanySiteTypeParameter.KeyFilter
		CompanySiteTypeParameter.CurrentFilter  = sFilter
		sSql = CompanySiteTypeParameter.SQL
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

			' SiteParameterTypeID
			CompanySiteTypeParameter.SiteParameterTypeID.SetDbValue(CompanySiteTypeParameter.SiteParameterTypeID.CurrentValue, 0)
			Rs("SiteParameterTypeID") = CompanySiteTypeParameter.SiteParameterTypeID.DbValue

			' CompanyID
			CompanySiteTypeParameter.CompanyID.SetDbValue(CompanySiteTypeParameter.CompanyID.CurrentValue, System.DBNull.Value)
			Rs("CompanyID") = CompanySiteTypeParameter.CompanyID.DbValue

			' SiteCategoryTypeID
			CompanySiteTypeParameter.SiteCategoryTypeID.SetDbValue(CompanySiteTypeParameter.SiteCategoryTypeID.CurrentValue, System.DBNull.Value)
			Rs("SiteCategoryTypeID") = CompanySiteTypeParameter.SiteCategoryTypeID.DbValue

			' SiteCategoryGroupID
			CompanySiteTypeParameter.SiteCategoryGroupID.SetDbValue(CompanySiteTypeParameter.SiteCategoryGroupID.CurrentValue, System.DBNull.Value)
			Rs("SiteCategoryGroupID") = CompanySiteTypeParameter.SiteCategoryGroupID.DbValue

			' SiteCategoryID
			CompanySiteTypeParameter.SiteCategoryID.SetDbValue(CompanySiteTypeParameter.SiteCategoryID.CurrentValue, System.DBNull.Value)
			Rs("SiteCategoryID") = CompanySiteTypeParameter.SiteCategoryID.DbValue

			' SortOrder
			CompanySiteTypeParameter.SortOrder.SetDbValue(CompanySiteTypeParameter.SortOrder.CurrentValue, System.DBNull.Value)
			Rs("SortOrder") = CompanySiteTypeParameter.SortOrder.DbValue

			' Row Updating event
			bUpdateRow = CompanySiteTypeParameter.Row_Updating(RsOld, Rs)
			If bUpdateRow Then
				Try
					CompanySiteTypeParameter.Update(Rs)
					EditRow = True
				Catch e As Exception
					If EW_DEBUG_ENABLED Then Throw
					Message = e.Message
					EditRow = False
				End Try
			Else
				If CompanySiteTypeParameter.CancelMessage <> "" Then
					Message = CompanySiteTypeParameter.CancelMessage
					CompanySiteTypeParameter.CancelMessage = ""
				Else
					Message = "Update cancelled"
				End If
				EditRow = False
			End If
		End If

		' Row Updated event
		If EditRow Then
			CompanySiteTypeParameter.Row_Updated(RsOld, Rs)
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

		' SiteParameterTypeID
		CompanySiteTypeParameter.SiteParameterTypeID.SetDbValue(CompanySiteTypeParameter.SiteParameterTypeID.CurrentValue, 0)
		Rs("SiteParameterTypeID") = CompanySiteTypeParameter.SiteParameterTypeID.DbValue

		' CompanyID
		CompanySiteTypeParameter.CompanyID.SetDbValue(CompanySiteTypeParameter.CompanyID.CurrentValue, System.DBNull.Value)
		Rs("CompanyID") = CompanySiteTypeParameter.CompanyID.DbValue

		' SiteCategoryTypeID
		CompanySiteTypeParameter.SiteCategoryTypeID.SetDbValue(CompanySiteTypeParameter.SiteCategoryTypeID.CurrentValue, System.DBNull.Value)
		Rs("SiteCategoryTypeID") = CompanySiteTypeParameter.SiteCategoryTypeID.DbValue

		' SiteCategoryGroupID
		CompanySiteTypeParameter.SiteCategoryGroupID.SetDbValue(CompanySiteTypeParameter.SiteCategoryGroupID.CurrentValue, System.DBNull.Value)
		Rs("SiteCategoryGroupID") = CompanySiteTypeParameter.SiteCategoryGroupID.DbValue

		' SiteCategoryID
		CompanySiteTypeParameter.SiteCategoryID.SetDbValue(CompanySiteTypeParameter.SiteCategoryID.CurrentValue, System.DBNull.Value)
		Rs("SiteCategoryID") = CompanySiteTypeParameter.SiteCategoryID.DbValue

		' SortOrder
		CompanySiteTypeParameter.SortOrder.SetDbValue(CompanySiteTypeParameter.SortOrder.CurrentValue, System.DBNull.Value)
		Rs("SortOrder") = CompanySiteTypeParameter.SortOrder.DbValue

		' Row Inserting event
		bInsertRow = CompanySiteTypeParameter.Row_Inserting(Rs)
		If bInsertRow Then
			Try	
				CompanySiteTypeParameter.Insert(Rs)
				AddRow = True
			Catch e As Exception
				If EW_DEBUG_ENABLED Then Throw
				Message = e.Message				
				AddRow = False
			End Try
		Else
			If CompanySiteTypeParameter.CancelMessage <> "" Then
				Message = CompanySiteTypeParameter.CancelMessage
				CompanySiteTypeParameter.CancelMessage = ""
			Else
				Message = "Insert cancelled"
			End If
			AddRow = False
		End If
		If AddRow Then
			LastInsertId = Conn.GetLastInsertId()
			CompanySiteTypeParameter.CompanySiteTypeParameterID.DbValue = LastInsertId
			Rs("CompanySiteTypeParameterID") = CompanySiteTypeParameter.CompanySiteTypeParameterID.DbValue		

			' Row Inserted event
			CompanySiteTypeParameter.Row_Inserted(Rs)
			WriteAuditTrailOnAdd(Rs)
		End If
	End Function

	'
	' Load advanced search
	'
	Sub LoadAdvancedSearch()
		CompanySiteTypeParameter.SiteParameterTypeID.AdvancedSearch.SearchValue = CompanySiteTypeParameter.GetAdvancedSearch("x_SiteParameterTypeID")
		CompanySiteTypeParameter.CompanyID.AdvancedSearch.SearchValue = CompanySiteTypeParameter.GetAdvancedSearch("x_CompanyID")
		CompanySiteTypeParameter.SiteCategoryTypeID.AdvancedSearch.SearchValue = CompanySiteTypeParameter.GetAdvancedSearch("x_SiteCategoryTypeID")
		CompanySiteTypeParameter.SiteCategoryGroupID.AdvancedSearch.SearchValue = CompanySiteTypeParameter.GetAdvancedSearch("x_SiteCategoryGroupID")
		CompanySiteTypeParameter.SiteCategoryID.AdvancedSearch.SearchValue = CompanySiteTypeParameter.GetAdvancedSearch("x_SiteCategoryID")
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
		If CompanySiteTypeParameter.ExportAll Then
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
		If CompanySiteTypeParameter.Export = "xml" Then
			oXmlDoc = New XmlDocument()
			oXmlTbl = oXmlDoc.CreateElement("table")
			oXmlDoc.AppendChild(oXmlTbl)
		Else
			ew_Write(ew_ExportHeader(CompanySiteTypeParameter.Export))

			' Horizontal format, write header
			If sExportStyle <> "v" OrElse CompanySiteTypeParameter.Export = "csv" Then
				sExportStr = ""
				ew_ExportAddValue(sExportStr, "SiteParameterTypeID", CompanySiteTypeParameter.Export)
				ew_ExportAddValue(sExportStr, "CompanyID", CompanySiteTypeParameter.Export)
				ew_ExportAddValue(sExportStr, "SiteCategoryTypeID", CompanySiteTypeParameter.Export)
				ew_ExportAddValue(sExportStr, "SiteCategoryGroupID", CompanySiteTypeParameter.Export)
				ew_ExportAddValue(sExportStr, "SiteCategoryID", CompanySiteTypeParameter.Export)
				ew_ExportAddValue(sExportStr, "SortOrder", CompanySiteTypeParameter.Export)
				ew_Write(ew_ExportLine(sExportStr, CompanySiteTypeParameter.Export))
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
				CompanySiteTypeParameter.RowType = EW_ROWTYPE_VIEW ' Render view
				RenderRow()
				If CompanySiteTypeParameter.Export = "xml" Then
					oXmlRec = oXmlDoc.CreateElement("record")
					oXmlTbl.AppendChild(oXmlRec)
					oXmlFld = oXmlDoc.CreateElement("SiteParameterTypeID") ' SiteParameterTypeID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(CompanySiteTypeParameter.SiteParameterTypeID.ExportValue(CompanySiteTypeParameter.Export, CompanySiteTypeParameter.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("CompanyID") ' CompanyID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(CompanySiteTypeParameter.CompanyID.ExportValue(CompanySiteTypeParameter.Export, CompanySiteTypeParameter.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("SiteCategoryTypeID") ' SiteCategoryTypeID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(CompanySiteTypeParameter.SiteCategoryTypeID.ExportValue(CompanySiteTypeParameter.Export, CompanySiteTypeParameter.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("SiteCategoryGroupID") ' SiteCategoryGroupID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(CompanySiteTypeParameter.SiteCategoryGroupID.ExportValue(CompanySiteTypeParameter.Export, CompanySiteTypeParameter.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("SiteCategoryID") ' SiteCategoryID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(CompanySiteTypeParameter.SiteCategoryID.ExportValue(CompanySiteTypeParameter.Export, CompanySiteTypeParameter.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("SortOrder") ' SortOrder
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(CompanySiteTypeParameter.SortOrder.ExportValue(CompanySiteTypeParameter.Export, CompanySiteTypeParameter.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
				Else

					' Vertical format
					If sExportStyle = "v" AndAlso CompanySiteTypeParameter.Export <> "csv" Then
						ew_Write(ew_ExportField("SiteParameterTypeID", CompanySiteTypeParameter.SiteParameterTypeID.ExportValue(CompanySiteTypeParameter.Export, CompanySiteTypeParameter.ExportOriginalValue), CompanySiteTypeParameter.Export)) ' SiteParameterTypeID
						ew_Write(ew_ExportField("CompanyID", CompanySiteTypeParameter.CompanyID.ExportValue(CompanySiteTypeParameter.Export, CompanySiteTypeParameter.ExportOriginalValue), CompanySiteTypeParameter.Export)) ' CompanyID
						ew_Write(ew_ExportField("SiteCategoryTypeID", CompanySiteTypeParameter.SiteCategoryTypeID.ExportValue(CompanySiteTypeParameter.Export, CompanySiteTypeParameter.ExportOriginalValue), CompanySiteTypeParameter.Export)) ' SiteCategoryTypeID
						ew_Write(ew_ExportField("SiteCategoryGroupID", CompanySiteTypeParameter.SiteCategoryGroupID.ExportValue(CompanySiteTypeParameter.Export, CompanySiteTypeParameter.ExportOriginalValue), CompanySiteTypeParameter.Export)) ' SiteCategoryGroupID
						ew_Write(ew_ExportField("SiteCategoryID", CompanySiteTypeParameter.SiteCategoryID.ExportValue(CompanySiteTypeParameter.Export, CompanySiteTypeParameter.ExportOriginalValue), CompanySiteTypeParameter.Export)) ' SiteCategoryID
						ew_Write(ew_ExportField("SortOrder", CompanySiteTypeParameter.SortOrder.ExportValue(CompanySiteTypeParameter.Export, CompanySiteTypeParameter.ExportOriginalValue), CompanySiteTypeParameter.Export)) ' SortOrder

					' Horizontal format
					Else
						sExportStr = ""
						ew_ExportAddValue(sExportStr, CompanySiteTypeParameter.SiteParameterTypeID.ExportValue(CompanySiteTypeParameter.Export, CompanySiteTypeParameter.ExportOriginalValue), CompanySiteTypeParameter.Export) ' SiteParameterTypeID
						ew_ExportAddValue(sExportStr, CompanySiteTypeParameter.CompanyID.ExportValue(CompanySiteTypeParameter.Export, CompanySiteTypeParameter.ExportOriginalValue), CompanySiteTypeParameter.Export) ' CompanyID
						ew_ExportAddValue(sExportStr, CompanySiteTypeParameter.SiteCategoryTypeID.ExportValue(CompanySiteTypeParameter.Export, CompanySiteTypeParameter.ExportOriginalValue), CompanySiteTypeParameter.Export) ' SiteCategoryTypeID
						ew_ExportAddValue(sExportStr, CompanySiteTypeParameter.SiteCategoryGroupID.ExportValue(CompanySiteTypeParameter.Export, CompanySiteTypeParameter.ExportOriginalValue), CompanySiteTypeParameter.Export) ' SiteCategoryGroupID
						ew_ExportAddValue(sExportStr, CompanySiteTypeParameter.SiteCategoryID.ExportValue(CompanySiteTypeParameter.Export, CompanySiteTypeParameter.ExportOriginalValue), CompanySiteTypeParameter.Export) ' SiteCategoryID
						ew_ExportAddValue(sExportStr, CompanySiteTypeParameter.SortOrder.ExportValue(CompanySiteTypeParameter.Export, CompanySiteTypeParameter.ExportOriginalValue), CompanySiteTypeParameter.Export) ' SortOrder
						ew_Write(ew_ExportLine(sExportStr, CompanySiteTypeParameter.Export))
					End If
				End If
			End If
		Loop

		' Close recordset
		Rs.Close()
		Rs.Dispose()
		If CompanySiteTypeParameter.Export = "xml" Then
			ew_Write("<?xml version=""1.0"" standalone=""yes""?>" & vbcrlf)
			ew_Write(oXmlDoc.OuterXml)
		Else
			ew_Write(ew_ExportFooter(CompanySiteTypeParameter.Export))
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
				sDbMasterFilter = CompanySiteTypeParameter.SqlMasterFilter_SiteParameterType
				sDbDetailFilter = CompanySiteTypeParameter.SqlDetailFilter_SiteParameterType
				If ew_Get("SiteParameterTypeID") <> "" Then
					SiteParameterType.SiteParameterTypeID.QueryStringValue = ew_Get("SiteParameterTypeID")
					CompanySiteTypeParameter.SiteCategoryTypeID.QueryStringValue = SiteParameterType.SiteParameterTypeID.QueryStringValue
					CompanySiteTypeParameter.SiteCategoryTypeID.SessionValue = CompanySiteTypeParameter.SiteCategoryTypeID.QueryStringValue
					If Not IsNumeric(SiteParameterType.SiteParameterTypeID.QueryStringValue) Then bValidMaster = False
					sDbMasterFilter = sDbMasterFilter.Replace("@SiteParameterTypeID@", ew_AdjustSql(SiteParameterType.SiteParameterTypeID.QueryStringValue))
					sDbDetailFilter = sDbDetailFilter.Replace("@SiteCategoryTypeID@", ew_AdjustSql(SiteParameterType.SiteParameterTypeID.QueryStringValue))
				Else
					bValidMaster = False
				End If
			End If
		End If
		If bValidMaster Then

			' Save current master table
			CompanySiteTypeParameter.CurrentMasterTable = sMasterTblVar

			' Reset start record counter (new master key)
			lStartRec = 1
			CompanySiteTypeParameter.StartRecordNumber = lStartRec
			CompanySiteTypeParameter.MasterFilter = sDbMasterFilter ' Set up master filter
			CompanySiteTypeParameter.DetailFilter = sDbDetailFilter ' Set up detail filter

			' Clear previous master session values
			If sMasterTblVar <> "SiteParameterType" Then
				If CompanySiteTypeParameter.SiteCategoryTypeID.QueryStringValue = "" Then CompanySiteTypeParameter.SiteCategoryTypeID.SessionValue = ""
			End If
		End If
	End Sub

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "CompanySiteTypeParameter"
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
		Dim table As String = "CompanySiteTypeParameter"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsSrc("CompanySiteTypeParameterID")

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

		' SiteParameterTypeID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteParameterTypeID", keyvalue, oldvalue, RsSrc("SiteParameterTypeID"))

		' CompanyID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "CompanyID", keyvalue, oldvalue, RsSrc("CompanyID"))

		' SiteCategoryTypeID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteCategoryTypeID", keyvalue, oldvalue, RsSrc("SiteCategoryTypeID"))

		' SiteCategoryGroupID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteCategoryGroupID", keyvalue, oldvalue, RsSrc("SiteCategoryGroupID"))

		' SiteCategoryID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteCategoryID", keyvalue, oldvalue, RsSrc("SiteCategoryID"))

		' SortOrder Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SortOrder", keyvalue, oldvalue, RsSrc("SortOrder"))
	End Sub

	' Write Audit Trail (edit page)
	Sub WriteAuditTrailOnEdit(ByRef RsOld As OrderedDictionary, ByRef RsNew As OrderedDictionary)
		Dim table As String = "CompanySiteTypeParameter"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsOld("CompanySiteTypeParameterID")

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
			fld = CompanySiteTypeParameter.FieldByName(fldname)
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
		CompanySiteTypeParameter_list = New cCompanySiteTypeParameter_list(Me)		
		CompanySiteTypeParameter_list.Page_Init()

		' Page main processing
		CompanySiteTypeParameter_list.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If CompanySiteTypeParameter_list IsNot Nothing Then CompanySiteTypeParameter_list.Dispose()
	End Sub
End Class
