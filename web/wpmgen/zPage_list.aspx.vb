Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class zPage_list
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public zPage_list As czPage_list

	'
	' Page Class
	'
	Class czPage_list
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
				If zPage.UseTokenInUrl Then Url = Url & "t=" & zPage.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If zPage.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (zPage.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (zPage.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' zPage
		Public Property zPage() As czPage
			Get				
				Return ParentPage.zPage
			End Get
			Set(ByVal v As czPage)
				ParentPage.zPage = v	
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
			m_PageObjName = "zPage_list"
			m_PageObjTypeName = "czPage_list"

			' Table Name
			m_TableName = "Page"

			' Initialize table object
			zPage = New czPage(Me)

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
			zPage.Export = ew_Get("export") ' Get export parameter
			ParentPage.gsExport = zPage.Export ' Get export parameter, used in header
			ParentPage.gsExportFile = zPage.TableVar ' Get export file, used in header
			If zPage.Export = "excel" Then
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
			zPage.Dispose()
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
					zPage.CurrentAction = ew_Get("a")

					' Clear inline mode
					If zPage.CurrentAction = "cancel" Then
						ClearInlineMode()
					End If

					' Switch to grid edit mode
					If zPage.CurrentAction = "gridedit" Then
						GridEditMode()
					End If

					' Switch to inline edit mode
					If zPage.CurrentAction = "edit" Then
						InlineEditMode()
					End If

					' Switch to grid add mode
					If zPage.CurrentAction = "gridadd" Then
						GridAddMode()
					End If
				End If
			Else
				zPage.CurrentAction = ObjForm.GetValue("a_list") ' Get action

				' Grid Update
				If zPage.CurrentAction = "gridupdate" AndAlso ew_SameStr(ew_Session(EW_SESSION_INLINE_MODE), "gridedit") Then
					GridUpdate()
				End If

				' Inline Update
				If zPage.CurrentAction = "update" AndAlso ew_SameStr(ew_Session(EW_SESSION_INLINE_MODE), "edit") Then
					InlineUpdate()
				End If

				' Grid Insert
				If zPage.CurrentAction = "gridinsert" AndAlso ew_SameStr(ew_Session(EW_SESSION_INLINE_MODE), "gridadd") Then
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
		If (zPage.RecordsPerPage = -1 OrElse zPage.RecordsPerPage > 0) Then
			lDisplayRecs = zPage.RecordsPerPage ' Restore from Session
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
		zPage.Recordset_Searching(sSrchWhere)

		' Save search criteria
		If sSrchWhere <> "" Then
			If sSrchBasic = "" Then ResetBasicSearchParms()
			If sSrchAdvanced = "" Then ResetAdvancedSearchParms()
			zPage.SearchWhere = sSrchWhere ' Save to Session
			lStartRec = 1 ' Reset start record counter
			zPage.StartRecordNumber = lStartRec
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
		zPage.SessionWhere = sFilter
		zPage.CurrentFilter = ""

		' Export Data only
		If zPage.Export = "html" OrElse zPage.Export = "csv" OrElse zPage.Export = "word" OrElse zPage.Export = "excel" OrElse zPage.Export = "xml" Then
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
			zPage.RecordsPerPage = lDisplayRecs ' Save to Session

			' Reset start position
			lStartRec = 1
			zPage.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	'  Exit out of inline mode
	'
	Sub ClearInlineMode()
		zPage.SetKey("zPageID", "") ' Clear inline edit key
		zPage.CurrentAction = "" ' Clear action
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
		If ew_Get("zPageID") <> "" Then
			zPage.zPageID.QueryStringValue = ew_Get("zPageID")
		Else
			bInlineEdit = False
		End If
		If bInlineEdit AndAlso LoadRow() Then
			zPage.SetKey("zPageID", zPage.zPageID.CurrentValue) ' Set up inline edit key
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
				zPage.SendEmail = True ' Send email on update success
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
			zPage.EventCancelled = True ' Cancel event
			zPage.CurrentAction = "edit" ' Stay in edit mode
		End If
	End Sub

	'
	' Check inline edit key
	'
	Function CheckInlineEditKey() As Boolean
		If Not ew_SameStr(zPage.GetKey("zPageID"), zPage.zPageID.CurrentValue) Then
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
		zPage.CurrentFilter = BuildKeyFilter()
		sSql = zPage.SQL
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
					zPage.SendEmail = False ' Do not send email on update success
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
			zPage.EventCancelled = True ' Set event cancelled
			zPage.CurrentAction = "gridedit" ' Stay in gridedit mode
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
				sFilter = zPage.KeyFilter
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
			zPage.zPageID.FormValue = arKeyFlds(0)
			If Not IsNumeric(zPage.zPageID.FormValue) Then	Return False
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
				zPage.SendEmail = False ' Do not send email on insert success

				' Validate Form
				If Not ValidateForm() Then
					bGridInsert = False ' Form error, reset action
					Message = ParentPage.gsFormError
				Else
					bGridInsert = AddRow() ' Insert this row
				End If
				If bGridInsert Then
					If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
					sKey = sKey & zPage.zPageID.CurrentValue

					' Add filter for this record
					sFilter = zPage.KeyFilter
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
			zPage.CurrentFilter = sWrkFilter
			sSql = zPage.SQL
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
			zPage.EventCancelled = True ' Set event cancelled
			zPage.CurrentAction = "gridadd" ' Stay in gridadd mode
		End If
	End Sub

	Function EmptyRow() As Boolean
		Dim Empty As Boolean = True
		If Empty Then Empty = ew_SameStr(zPage.PageOrder.CurrentValue, zPage.PageOrder.OldValue)
		If Empty Then Empty = ew_SameStr(zPage.ParentPageID.CurrentValue, zPage.ParentPageID.OldValue)
		If Empty Then Empty = ew_SameStr(zPage.PageTypeID.CurrentValue, zPage.PageTypeID.OldValue)
		If Empty Then Empty = ew_SameStr(zPage.Active.CurrentValue, zPage.Active.OldValue)
		If Empty Then Empty = ew_SameStr(zPage.zPageName.CurrentValue, zPage.zPageName.OldValue)
		If Empty Then Empty = ew_SameStr(zPage.SiteCategoryID.CurrentValue, zPage.SiteCategoryID.OldValue)
		If Empty Then Empty = ew_SameStr(zPage.SiteCategoryGroupID.CurrentValue, zPage.SiteCategoryGroupID.OldValue)
		If Empty Then Empty = ew_SameStr(zPage.ModifiedDT.CurrentValue, zPage.ModifiedDT.OldValue)
		Return Empty
	End Function

	'
	' Restore form values for current row
	'
	Sub RestoreCurrentRowFormValues(idx As Integer)
		Dim sKey As String, arKeyFlds() As String

		' Get row based on current index
		ObjForm.Index = idx
		If zPage.CurrentAction = "gridadd" Then
			LoadFormValues() ' Load form values
		End If
		If zPage.CurrentAction = "gridedit" Then
			sKey = ObjForm.GetValue("k_key")
			arKeyFlds = sKey.Split(New Char() {Convert.ToChar(EW_COMPOSITE_KEY_SEPARATOR)})
			If arKeyFlds.Length >= 1 Then
				If ew_SameStr(arKeyFlds(0), zPage.zPageID.CurrentValue) Then
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
		BuildSearchSql(sWhere, zPage.CompanyID, False) ' CompanyID
		BuildSearchSql(sWhere, zPage.GroupID, False) ' GroupID
		BuildSearchSql(sWhere, zPage.ParentPageID, False) ' ParentPageID
		BuildSearchSql(sWhere, zPage.PageTypeID, False) ' PageTypeID
		BuildSearchSql(sWhere, zPage.Active, False) ' Active
		BuildSearchSql(sWhere, zPage.zPageName, False) ' PageName
		BuildSearchSql(sWhere, zPage.PageTitle, False) ' PageTitle
		BuildSearchSql(sWhere, zPage.PageDescription, False) ' PageDescription
		BuildSearchSql(sWhere, zPage.PageKeywords, False) ' PageKeywords
		BuildSearchSql(sWhere, zPage.SiteCategoryID, False) ' SiteCategoryID
		BuildSearchSql(sWhere, zPage.SiteCategoryGroupID, False) ' SiteCategoryGroupID

		' Set up search parm
		If sWhere <> "" Then
			SetSearchParm(zPage.CompanyID) ' CompanyID
			SetSearchParm(zPage.GroupID) ' GroupID
			SetSearchParm(zPage.ParentPageID) ' ParentPageID
			SetSearchParm(zPage.PageTypeID) ' PageTypeID
			SetSearchParm(zPage.Active) ' Active
			SetSearchParm(zPage.zPageName) ' PageName
			SetSearchParm(zPage.PageTitle) ' PageTitle
			SetSearchParm(zPage.PageDescription) ' PageDescription
			SetSearchParm(zPage.PageKeywords) ' PageKeywords
			SetSearchParm(zPage.SiteCategoryID) ' SiteCategoryID
			SetSearchParm(zPage.SiteCategoryGroupID) ' SiteCategoryGroupID
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
		zPage.SetAdvancedSearch("x_" & FldParm, Fld.AdvancedSearch.SearchValue)
		zPage.SetAdvancedSearch("z_" & FldParm, Fld.AdvancedSearch.SearchOperator)
		zPage.SetAdvancedSearch("v_" & FldParm, Fld.AdvancedSearch.SearchCondition)
		zPage.SetAdvancedSearch("y_" & FldParm, Fld.AdvancedSearch.SearchValue2)
		zPage.SetAdvancedSearch("w_" & FldParm, Fld.AdvancedSearch.SearchOperator2)
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
		sSql = sSql & "[PageName] LIKE '%" & sKeyword & "%' OR "
		sSql = sSql & "[PageTitle] LIKE '%" & sKeyword & "%' OR "
		sSql = sSql & "[PageDescription] LIKE '%" & sKeyword & "%' OR "
		sSql = sSql & "[PageKeywords] LIKE '%" & sKeyword & "%' OR "
		sSql = sSql & "[PageFileName] LIKE '%" & sKeyword & "%' OR "
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
			zPage.BasicSearchKeyword = sSearchKeyword
			zPage.BasicSearchType = sSearchType
		End If
		Return sSearchStr
	End Function

	'
	' Clear all search parameters
	'
	Sub ResetSearchParms()

		' Clear search where
		sSrchWhere = ""
		zPage.SearchWhere = sSrchWhere

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
		zPage.BasicSearchKeyword = ""
		zPage.BasicSearchType = ""
	End Sub

	'
	' Clear all advanced search parameters
	'
	Sub ResetAdvancedSearchParms()
		zPage.SetAdvancedSearch("x_CompanyID", "")
		zPage.SetAdvancedSearch("x_GroupID", "")
		zPage.SetAdvancedSearch("x_ParentPageID", "")
		zPage.SetAdvancedSearch("x_PageTypeID", "")
		zPage.SetAdvancedSearch("x_Active", "")
		zPage.SetAdvancedSearch("x_zPageName", "")
		zPage.SetAdvancedSearch("x_PageTitle", "")
		zPage.SetAdvancedSearch("x_PageDescription", "")
		zPage.SetAdvancedSearch("x_PageKeywords", "")
		zPage.SetAdvancedSearch("x_SiteCategoryID", "")
		zPage.SetAdvancedSearch("x_SiteCategoryGroupID", "")
	End Sub

	'
	' Restore all search parameters
	'
	Sub RestoreSearchParms()
		sSrchWhere = zPage.SearchWhere

		' Restore advanced search settings
		If ParentPage.gsSearchError = "" Then
			RestoreAdvancedSearchParms()
		End If
	End Sub

	'
	' Restore all advanced search parameters
	'
	Sub RestoreAdvancedSearchParms()
		zPage.CompanyID.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_CompanyID")
		zPage.GroupID.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_GroupID")
		zPage.ParentPageID.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_ParentPageID")
		zPage.PageTypeID.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_PageTypeID")
		zPage.Active.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_Active")
		zPage.zPageName.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_zPageName")
		zPage.PageTitle.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_PageTitle")
		zPage.PageDescription.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_PageDescription")
		zPage.PageKeywords.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_PageKeywords")
		zPage.SiteCategoryID.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_SiteCategoryID")
		zPage.SiteCategoryGroupID.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_SiteCategoryGroupID")
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
			zPage.CurrentOrder = ew_Get("order")
			zPage.CurrentOrderType = ew_Get("ordertype")
			zPage.UpdateSort(zPage.PageOrder) ' PageOrder
			zPage.UpdateSort(zPage.ParentPageID) ' ParentPageID
			zPage.UpdateSort(zPage.PageTypeID) ' PageTypeID
			zPage.UpdateSort(zPage.Active) ' Active
			zPage.UpdateSort(zPage.zPageName) ' PageName
			zPage.UpdateSort(zPage.SiteCategoryID) ' SiteCategoryID
			zPage.UpdateSort(zPage.SiteCategoryGroupID) ' SiteCategoryGroupID
			zPage.UpdateSort(zPage.ModifiedDT) ' ModifiedDT
			zPage.StartRecordNumber = 1 ' Reset start position
		End If
	End Sub

	'
	' Load Sort Order parameters
	'
	Sub LoadSortOrder()
		Dim sOrderBy As String
		sOrderBy = zPage.SessionOrderBy ' Get order by from Session
		If sOrderBy = "" Then
			If zPage.SqlOrderBy <> "" Then
				sOrderBy = zPage.SqlOrderBy
				zPage.SessionOrderBy = sOrderBy
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
				zPage.SessionOrderBy = sOrderBy
				zPage.PageOrder.Sort = ""
				zPage.ParentPageID.Sort = ""
				zPage.PageTypeID.Sort = ""
				zPage.Active.Sort = ""
				zPage.zPageName.Sort = ""
				zPage.SiteCategoryID.Sort = ""
				zPage.SiteCategoryGroupID.Sort = ""
				zPage.ModifiedDT.Sort = ""
			End If

			' Reset start position
			lStartRec = 1
			zPage.StartRecordNumber = lStartRec
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
				zPage.StartRecordNumber = lStartRec
			ElseIf ew_Get(EW_TABLE_PAGE_NO) <> "" AndAlso IsNumeric(ew_Get(EW_TABLE_PAGE_NO)) Then
				nPageNo = ew_ConvertToInt(ew_Get(EW_TABLE_PAGE_NO))
				lStartRec = (nPageNo-1)*lDisplayRecs+1
				If lStartRec <= 0 Then
					lStartRec = 1
				ElseIf lStartRec >= ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 Then
					lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1
				End If
				zPage.StartRecordNumber = lStartRec
			End If
		End If
		lStartRec = zPage.StartRecordNumber

		' Check if correct start record counter
		If lStartRec <= 0 Then ' Avoid invalid start record counter
			lStartRec = 1 ' Reset start record counter
			zPage.StartRecordNumber = lStartRec
		ElseIf lStartRec > lTotalRecs Then ' Avoid starting record > total records
			lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to last page first record
			zPage.StartRecordNumber = lStartRec
		ElseIf (lStartRec-1) Mod lDisplayRecs <> 0 Then
			lStartRec = ((lStartRec-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to page boundary
			zPage.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	' Load default values
	'
	Sub LoadDefaultValues()
		zPage.PageOrder.CurrentValue = 0
		zPage.PageOrder.OldValue = zPage.PageOrder.CurrentValue
		zPage.ParentPageID.CurrentValue = 0
		zPage.ParentPageID.OldValue = zPage.ParentPageID.CurrentValue
		zPage.PageTypeID.CurrentValue = 0
		zPage.PageTypeID.OldValue = zPage.PageTypeID.CurrentValue
		zPage.Active.OldValue = zPage.Active.CurrentValue
		zPage.zPageName.OldValue = zPage.zPageName.CurrentValue
		zPage.SiteCategoryID.OldValue = zPage.SiteCategoryID.CurrentValue
		zPage.SiteCategoryGroupID.OldValue = zPage.SiteCategoryGroupID.CurrentValue
		zPage.ModifiedDT.OldValue = zPage.ModifiedDT.CurrentValue
	End Sub

	'
	'  Load search values for validation
	'
	Sub LoadSearchValues()
		zPage.CompanyID.AdvancedSearch.SearchValue = ew_Get("x_CompanyID")
    	zPage.CompanyID.AdvancedSearch.SearchOperator = ew_Get("z_CompanyID")
		zPage.GroupID.AdvancedSearch.SearchValue = ew_Get("x_GroupID")
    	zPage.GroupID.AdvancedSearch.SearchOperator = ew_Get("z_GroupID")
		zPage.ParentPageID.AdvancedSearch.SearchValue = ew_Get("x_ParentPageID")
    	zPage.ParentPageID.AdvancedSearch.SearchOperator = ew_Get("z_ParentPageID")
		zPage.PageTypeID.AdvancedSearch.SearchValue = ew_Get("x_PageTypeID")
    	zPage.PageTypeID.AdvancedSearch.SearchOperator = ew_Get("z_PageTypeID")
		zPage.Active.AdvancedSearch.SearchValue = ew_Get("x_Active")
    	zPage.Active.AdvancedSearch.SearchOperator = ew_Get("z_Active")
		zPage.zPageName.AdvancedSearch.SearchValue = ew_Get("x_zPageName")
    	zPage.zPageName.AdvancedSearch.SearchOperator = ew_Get("z_zPageName")
		zPage.PageTitle.AdvancedSearch.SearchValue = ew_Get("x_PageTitle")
    	zPage.PageTitle.AdvancedSearch.SearchOperator = ew_Get("z_PageTitle")
		zPage.PageDescription.AdvancedSearch.SearchValue = ew_Get("x_PageDescription")
    	zPage.PageDescription.AdvancedSearch.SearchOperator = ew_Get("z_PageDescription")
		zPage.PageKeywords.AdvancedSearch.SearchValue = ew_Get("x_PageKeywords")
    	zPage.PageKeywords.AdvancedSearch.SearchOperator = ew_Get("z_PageKeywords")
		zPage.SiteCategoryID.AdvancedSearch.SearchValue = ew_Get("x_SiteCategoryID")
    	zPage.SiteCategoryID.AdvancedSearch.SearchOperator = ew_Get("z_SiteCategoryID")
		zPage.SiteCategoryGroupID.AdvancedSearch.SearchValue = ew_Get("x_SiteCategoryGroupID")
    	zPage.SiteCategoryGroupID.AdvancedSearch.SearchOperator = ew_Get("z_SiteCategoryGroupID")
	End Sub

	'
	' Load form values
	'
	Sub LoadFormValues()
		zPage.PageOrder.FormValue = ObjForm.GetValue("x_PageOrder")
		zPage.PageOrder.OldValue = ObjForm.GetValue("o_PageOrder")
		zPage.ParentPageID.FormValue = ObjForm.GetValue("x_ParentPageID")
		zPage.ParentPageID.OldValue = ObjForm.GetValue("o_ParentPageID")
		zPage.PageTypeID.FormValue = ObjForm.GetValue("x_PageTypeID")
		zPage.PageTypeID.OldValue = ObjForm.GetValue("o_PageTypeID")
		zPage.Active.FormValue = ObjForm.GetValue("x_Active")
		zPage.Active.OldValue = ObjForm.GetValue("o_Active")
		zPage.zPageName.FormValue = ObjForm.GetValue("x_zPageName")
		zPage.zPageName.OldValue = ObjForm.GetValue("o_zPageName")
		zPage.SiteCategoryID.FormValue = ObjForm.GetValue("x_SiteCategoryID")
		zPage.SiteCategoryID.OldValue = ObjForm.GetValue("o_SiteCategoryID")
		zPage.SiteCategoryGroupID.FormValue = ObjForm.GetValue("x_SiteCategoryGroupID")
		zPage.SiteCategoryGroupID.OldValue = ObjForm.GetValue("o_SiteCategoryGroupID")
		zPage.ModifiedDT.FormValue = ObjForm.GetValue("x_ModifiedDT")
		zPage.ModifiedDT.CurrentValue = ew_UnFormatDateTime(zPage.ModifiedDT.CurrentValue, 6)
		zPage.ModifiedDT.OldValue = ObjForm.GetValue("o_ModifiedDT")
		zPage.zPageID.FormValue = ObjForm.GetValue("x_zPageID")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		zPage.PageOrder.CurrentValue = zPage.PageOrder.FormValue
		zPage.ParentPageID.CurrentValue = zPage.ParentPageID.FormValue
		zPage.PageTypeID.CurrentValue = zPage.PageTypeID.FormValue
		zPage.Active.CurrentValue = zPage.Active.FormValue
		zPage.zPageName.CurrentValue = zPage.zPageName.FormValue
		zPage.SiteCategoryID.CurrentValue = zPage.SiteCategoryID.FormValue
		zPage.SiteCategoryGroupID.CurrentValue = zPage.SiteCategoryGroupID.FormValue
		zPage.ModifiedDT.CurrentValue = zPage.ModifiedDT.FormValue
		zPage.ModifiedDT.CurrentValue = ew_UnFormatDateTime(zPage.ModifiedDT.CurrentValue, 6)
		zPage.zPageID.CurrentValue = zPage.zPageID.FormValue
	End Sub

	'
	' Load recordset
	'
	Function LoadRecordset() As OleDbDataReader

		' Recordset Selecting event
		zPage.Recordset_Selecting(zPage.CurrentFilter)

		' Load list page SQL
		Dim sSql As String = zPage.ListSQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)

		' Count
		lTotalRecs = -1		
		Try			
			If sSql.StartsWith("SELECT * FROM ", StringComparison.InvariantCultureIgnoreCase) AndAlso _				
				ew_Empty(zPage.SqlGroupBy) AndAlso _
				ew_Empty(zPage.SqlHaving) Then
				Dim sCntSql As String = zPage.SelectCountSQL

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
		zPage.Recordset_Selected(Rs)
		Return Rs
	End Function

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = zPage.KeyFilter

		' Row Selecting event
		zPage.Row_Selecting(sFilter)

		' Load SQL based on filter
		zPage.CurrentFilter = sFilter
		Dim sSql As String = zPage.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				zPage.Row_Selected(RsRow)
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
		zPage.zPageID.DbValue = RsRow("PageID")
		zPage.CompanyID.DbValue = RsRow("CompanyID")
		zPage.PageOrder.DbValue = RsRow("PageOrder")
		zPage.GroupID.DbValue = RsRow("GroupID")
		zPage.ParentPageID.DbValue = RsRow("ParentPageID")
		zPage.PageTypeID.DbValue = RsRow("PageTypeID")
		zPage.Active.DbValue = IIf(ew_ConvertToBool(RsRow("Active")), "1", "0")
		zPage.zPageName.DbValue = RsRow("PageName")
		zPage.PageTitle.DbValue = RsRow("PageTitle")
		zPage.PageDescription.DbValue = RsRow("PageDescription")
		zPage.PageKeywords.DbValue = RsRow("PageKeywords")
		zPage.ImagesPerRow.DbValue = RsRow("ImagesPerRow")
		zPage.RowsPerPage.DbValue = RsRow("RowsPerPage")
		zPage.AllowMessage.DbValue = IIf(ew_ConvertToBool(RsRow("AllowMessage")), "1", "0")
		zPage.PageFileName.DbValue = RsRow("PageFileName")
		zPage.VersionNo.DbValue = RsRow("VersionNo")
		zPage.SiteCategoryID.DbValue = RsRow("SiteCategoryID")
		zPage.SiteCategoryGroupID.DbValue = RsRow("SiteCategoryGroupID")
		zPage.ModifiedDT.DbValue = RsRow("ModifiedDT")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		zPage.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' PageOrder

		zPage.PageOrder.CellCssStyle = ""
		zPage.PageOrder.CellCssClass = ""

		' ParentPageID
		zPage.ParentPageID.CellCssStyle = ""
		zPage.ParentPageID.CellCssClass = ""

		' PageTypeID
		zPage.PageTypeID.CellCssStyle = ""
		zPage.PageTypeID.CellCssClass = ""

		' Active
		zPage.Active.CellCssStyle = ""
		zPage.Active.CellCssClass = ""

		' PageName
		zPage.zPageName.CellCssStyle = ""
		zPage.zPageName.CellCssClass = ""

		' SiteCategoryID
		zPage.SiteCategoryID.CellCssStyle = ""
		zPage.SiteCategoryID.CellCssClass = ""

		' SiteCategoryGroupID
		zPage.SiteCategoryGroupID.CellCssStyle = ""
		zPage.SiteCategoryGroupID.CellCssClass = ""

		' ModifiedDT
		zPage.ModifiedDT.CellCssStyle = ""
		zPage.ModifiedDT.CellCssClass = ""

		'
		'  View  Row
		'

		If zPage.RowType = EW_ROWTYPE_VIEW Then ' View row

			' CompanyID
			If ew_NotEmpty(zPage.CompanyID.CurrentValue) Then
				sSqlWrk = "SELECT [CompanyName] FROM [Company] WHERE [CompanyID] = " & ew_AdjustSql(zPage.CompanyID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					zPage.CompanyID.ViewValue = RsWrk("CompanyName")
				Else
					zPage.CompanyID.ViewValue = zPage.CompanyID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				zPage.CompanyID.ViewValue = System.DBNull.Value
			End If
			zPage.CompanyID.CssStyle = ""
			zPage.CompanyID.CssClass = ""
			zPage.CompanyID.ViewCustomAttributes = ""

			' PageOrder
			zPage.PageOrder.ViewValue = zPage.PageOrder.CurrentValue
			zPage.PageOrder.CssStyle = ""
			zPage.PageOrder.CssClass = ""
			zPage.PageOrder.ViewCustomAttributes = ""

			' GroupID
			If ew_NotEmpty(zPage.GroupID.CurrentValue) Then
				sSqlWrk = "SELECT [GroupName] FROM [Group] WHERE [GroupID] = " & ew_AdjustSql(zPage.GroupID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [GroupID] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					zPage.GroupID.ViewValue = RsWrk("GroupName")
				Else
					zPage.GroupID.ViewValue = zPage.GroupID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				zPage.GroupID.ViewValue = System.DBNull.Value
			End If
			zPage.GroupID.CssStyle = ""
			zPage.GroupID.CssClass = ""
			zPage.GroupID.ViewCustomAttributes = ""

			' ParentPageID
			If ew_NotEmpty(zPage.ParentPageID.CurrentValue) Then
				sSqlWrk = "SELECT [PageName] FROM [Page] WHERE [PageID] = " & ew_AdjustSql(zPage.ParentPageID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [PageName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					zPage.ParentPageID.ViewValue = RsWrk("PageName")
				Else
					zPage.ParentPageID.ViewValue = zPage.ParentPageID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				zPage.ParentPageID.ViewValue = System.DBNull.Value
			End If
			zPage.ParentPageID.CssStyle = ""
			zPage.ParentPageID.CssClass = ""
			zPage.ParentPageID.ViewCustomAttributes = ""

			' PageTypeID
			If ew_NotEmpty(zPage.PageTypeID.CurrentValue) Then
				sSqlWrk = "SELECT [PageTypeDesc] FROM [PageType] WHERE [PageTypeID] = " & ew_AdjustSql(zPage.PageTypeID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [PageTypeDesc] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					zPage.PageTypeID.ViewValue = RsWrk("PageTypeDesc")
				Else
					zPage.PageTypeID.ViewValue = zPage.PageTypeID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				zPage.PageTypeID.ViewValue = System.DBNull.Value
			End If
			zPage.PageTypeID.CssStyle = ""
			zPage.PageTypeID.CssClass = ""
			zPage.PageTypeID.ViewCustomAttributes = ""

			' Active
			If Convert.ToString(zPage.Active.CurrentValue) = "1" Then
				zPage.Active.ViewValue = "Yes"
			Else
				zPage.Active.ViewValue = "No"
			End If
			zPage.Active.CssStyle = ""
			zPage.Active.CssClass = ""
			zPage.Active.ViewCustomAttributes = ""

			' PageName
			zPage.zPageName.ViewValue = zPage.zPageName.CurrentValue
			zPage.zPageName.CssStyle = ""
			zPage.zPageName.CssClass = ""
			zPage.zPageName.ViewCustomAttributes = ""

			' PageTitle
			zPage.PageTitle.ViewValue = zPage.PageTitle.CurrentValue
			zPage.PageTitle.CssStyle = ""
			zPage.PageTitle.CssClass = ""
			zPage.PageTitle.ViewCustomAttributes = ""

			' PageDescription
			zPage.PageDescription.ViewValue = zPage.PageDescription.CurrentValue
			zPage.PageDescription.CssStyle = ""
			zPage.PageDescription.CssClass = ""
			zPage.PageDescription.ViewCustomAttributes = ""

			' PageKeywords
			zPage.PageKeywords.ViewValue = zPage.PageKeywords.CurrentValue
			zPage.PageKeywords.CssStyle = ""
			zPage.PageKeywords.CssClass = ""
			zPage.PageKeywords.ViewCustomAttributes = ""

			' ImagesPerRow
			zPage.ImagesPerRow.ViewValue = zPage.ImagesPerRow.CurrentValue
			zPage.ImagesPerRow.CssStyle = ""
			zPage.ImagesPerRow.CssClass = ""
			zPage.ImagesPerRow.ViewCustomAttributes = ""

			' RowsPerPage
			zPage.RowsPerPage.ViewValue = zPage.RowsPerPage.CurrentValue
			zPage.RowsPerPage.CssStyle = ""
			zPage.RowsPerPage.CssClass = ""
			zPage.RowsPerPage.ViewCustomAttributes = ""

			' AllowMessage
			If Convert.ToString(zPage.AllowMessage.CurrentValue) = "1" Then
				zPage.AllowMessage.ViewValue = "Yes"
			Else
				zPage.AllowMessage.ViewValue = "No"
			End If
			zPage.AllowMessage.CssStyle = ""
			zPage.AllowMessage.CssClass = ""
			zPage.AllowMessage.ViewCustomAttributes = ""

			' PageFileName
			zPage.PageFileName.ViewValue = zPage.PageFileName.CurrentValue
			zPage.PageFileName.CssStyle = ""
			zPage.PageFileName.CssClass = ""
			zPage.PageFileName.ViewCustomAttributes = ""

			' VersionNo
			zPage.VersionNo.ViewValue = zPage.VersionNo.CurrentValue
			zPage.VersionNo.CssStyle = ""
			zPage.VersionNo.CssClass = ""
			zPage.VersionNo.ViewCustomAttributes = ""

			' SiteCategoryID
			If ew_NotEmpty(zPage.SiteCategoryID.CurrentValue) Then
				sSqlWrk = "SELECT [CategoryName] FROM [SiteCategory] WHERE [SiteCategoryID] = " & ew_AdjustSql(zPage.SiteCategoryID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [CategoryName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					zPage.SiteCategoryID.ViewValue = RsWrk("CategoryName")
				Else
					zPage.SiteCategoryID.ViewValue = zPage.SiteCategoryID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				zPage.SiteCategoryID.ViewValue = System.DBNull.Value
			End If
			zPage.SiteCategoryID.CssStyle = ""
			zPage.SiteCategoryID.CssClass = ""
			zPage.SiteCategoryID.ViewCustomAttributes = ""

			' SiteCategoryGroupID
			If ew_NotEmpty(zPage.SiteCategoryGroupID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteCategoryGroupNM] FROM [SiteCategoryGroup] WHERE [SiteCategoryGroupID] = " & ew_AdjustSql(zPage.SiteCategoryGroupID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryGroupNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					zPage.SiteCategoryGroupID.ViewValue = RsWrk("SiteCategoryGroupNM")
				Else
					zPage.SiteCategoryGroupID.ViewValue = zPage.SiteCategoryGroupID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				zPage.SiteCategoryGroupID.ViewValue = System.DBNull.Value
			End If
			zPage.SiteCategoryGroupID.CssStyle = ""
			zPage.SiteCategoryGroupID.CssClass = ""
			zPage.SiteCategoryGroupID.ViewCustomAttributes = ""

			' ModifiedDT
			zPage.ModifiedDT.ViewValue = zPage.ModifiedDT.CurrentValue
			zPage.ModifiedDT.ViewValue = ew_FormatDateTime(zPage.ModifiedDT.ViewValue, 6)
			zPage.ModifiedDT.CssStyle = ""
			zPage.ModifiedDT.CssClass = ""
			zPage.ModifiedDT.ViewCustomAttributes = ""

			' View refer script
			' PageOrder

			zPage.PageOrder.HrefValue = ""

			' ParentPageID
			zPage.ParentPageID.HrefValue = ""

			' PageTypeID
			zPage.PageTypeID.HrefValue = ""

			' Active
			zPage.Active.HrefValue = ""

			' PageName
			zPage.zPageName.HrefValue = ""

			' SiteCategoryID
			zPage.SiteCategoryID.HrefValue = ""

			' SiteCategoryGroupID
			zPage.SiteCategoryGroupID.HrefValue = ""

			' ModifiedDT
			zPage.ModifiedDT.HrefValue = ""

		'
		'  Add Row
		'

		ElseIf zPage.RowType = EW_ROWTYPE_ADD Then ' Add row

			' PageOrder
			zPage.PageOrder.EditCustomAttributes = ""
			zPage.PageOrder.EditValue = ew_HtmlEncode(zPage.PageOrder.CurrentValue)

			' ParentPageID
			zPage.ParentPageID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [PageID], [PageName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Page]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [PageName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			zPage.ParentPageID.EditValue = arwrk

			' PageTypeID
			zPage.PageTypeID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [PageTypeID], [PageTypeDesc], '' AS Disp2Fld, '' AS SelectFilterFld FROM [PageType]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [PageTypeDesc] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			zPage.PageTypeID.EditValue = arwrk

			' Active
			zPage.Active.EditCustomAttributes = ""

			' PageName
			zPage.zPageName.EditCustomAttributes = ""
			zPage.zPageName.EditValue = ew_HtmlEncode(zPage.zPageName.CurrentValue)

			' SiteCategoryID
			zPage.SiteCategoryID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryID], [CategoryName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteCategory]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [CategoryName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			zPage.SiteCategoryID.EditValue = arwrk

			' SiteCategoryGroupID
			zPage.SiteCategoryGroupID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryGroupID], [SiteCategoryGroupNM], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteCategoryGroup]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryGroupNM] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			zPage.SiteCategoryGroupID.EditValue = arwrk

			' ModifiedDT
		'
		'  Edit Row
		'

		ElseIf zPage.RowType = EW_ROWTYPE_EDIT Then ' Edit row

			' PageOrder
			zPage.PageOrder.EditCustomAttributes = ""
			zPage.PageOrder.EditValue = ew_HtmlEncode(zPage.PageOrder.CurrentValue)

			' ParentPageID
			zPage.ParentPageID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [PageID], [PageName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Page]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [PageName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			zPage.ParentPageID.EditValue = arwrk

			' PageTypeID
			zPage.PageTypeID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [PageTypeID], [PageTypeDesc], '' AS Disp2Fld, '' AS SelectFilterFld FROM [PageType]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [PageTypeDesc] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			zPage.PageTypeID.EditValue = arwrk

			' Active
			zPage.Active.EditCustomAttributes = ""

			' PageName
			zPage.zPageName.EditCustomAttributes = ""
			zPage.zPageName.EditValue = ew_HtmlEncode(zPage.zPageName.CurrentValue)

			' SiteCategoryID
			zPage.SiteCategoryID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryID], [CategoryName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteCategory]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [CategoryName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			zPage.SiteCategoryID.EditValue = arwrk

			' SiteCategoryGroupID
			zPage.SiteCategoryGroupID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryGroupID], [SiteCategoryGroupNM], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteCategoryGroup]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryGroupNM] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			zPage.SiteCategoryGroupID.EditValue = arwrk

			' ModifiedDT
			' Edit refer script
			' PageOrder

			zPage.PageOrder.HrefValue = ""

			' ParentPageID
			zPage.ParentPageID.HrefValue = ""

			' PageTypeID
			zPage.PageTypeID.HrefValue = ""

			' Active
			zPage.Active.HrefValue = ""

			' PageName
			zPage.zPageName.HrefValue = ""

			' SiteCategoryID
			zPage.SiteCategoryID.HrefValue = ""

			' SiteCategoryGroupID
			zPage.SiteCategoryGroupID.HrefValue = ""

			' ModifiedDT
			zPage.ModifiedDT.HrefValue = ""
		End If

		' Row Rendered event
		zPage.Row_Rendered()
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
		If ew_Empty(zPage.PageOrder.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter required field - Order"
		End If
		If Not ew_CheckInteger(zPage.PageOrder.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Incorrect integer - Order"
		End If
		If ew_Empty(zPage.PageTypeID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter required field - PageType"
		End If
		If ew_Empty(zPage.zPageName.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter required field - Page Name"
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
		sFilter = zPage.KeyFilter
		zPage.CurrentFilter  = sFilter
		sSql = zPage.SQL
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

			' PageOrder
			zPage.PageOrder.SetDbValue(zPage.PageOrder.CurrentValue, 0)
			Rs("PageOrder") = zPage.PageOrder.DbValue

			' ParentPageID
			zPage.ParentPageID.SetDbValue(zPage.ParentPageID.CurrentValue, System.DBNull.Value)
			Rs("ParentPageID") = zPage.ParentPageID.DbValue

			' PageTypeID
			zPage.PageTypeID.SetDbValue(zPage.PageTypeID.CurrentValue, System.DBNull.Value)
			Rs("PageTypeID") = zPage.PageTypeID.DbValue

			' Active
			zPage.Active.SetDbValue((zPage.Active.CurrentValue <> "" And Not IsDBNull(zPage.Active.CurrentValue)), System.DBNull.Value)
			Rs("Active") = zPage.Active.DbValue

			' PageName
			zPage.zPageName.SetDbValue(zPage.zPageName.CurrentValue, "")
			Rs("PageName") = zPage.zPageName.DbValue

			' SiteCategoryID
			zPage.SiteCategoryID.SetDbValue(zPage.SiteCategoryID.CurrentValue, System.DBNull.Value)
			Rs("SiteCategoryID") = zPage.SiteCategoryID.DbValue

			' SiteCategoryGroupID
			zPage.SiteCategoryGroupID.SetDbValue(zPage.SiteCategoryGroupID.CurrentValue, System.DBNull.Value)
			Rs("SiteCategoryGroupID") = zPage.SiteCategoryGroupID.DbValue

			' ModifiedDT
			zPage.ModifiedDT.DbValue = ew_CurrentDate()
			Rs("ModifiedDT") = zPage.ModifiedDT.DbValue

			' Row Updating event
			bUpdateRow = zPage.Row_Updating(RsOld, Rs)
			If bUpdateRow Then
				Try
					zPage.Update(Rs)
					EditRow = True
				Catch e As Exception
					If EW_DEBUG_ENABLED Then Throw
					Message = e.Message
					EditRow = False
				End Try
			Else
				If zPage.CancelMessage <> "" Then
					Message = zPage.CancelMessage
					zPage.CancelMessage = ""
				Else
					Message = "Update cancelled"
				End If
				EditRow = False
			End If
		End If

		' Row Updated event
		If EditRow Then
			zPage.Row_Updated(RsOld, Rs)
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

		' PageOrder
		zPage.PageOrder.SetDbValue(zPage.PageOrder.CurrentValue, 0)
		Rs("PageOrder") = zPage.PageOrder.DbValue

		' ParentPageID
		zPage.ParentPageID.SetDbValue(zPage.ParentPageID.CurrentValue, System.DBNull.Value)
		Rs("ParentPageID") = zPage.ParentPageID.DbValue

		' PageTypeID
		zPage.PageTypeID.SetDbValue(zPage.PageTypeID.CurrentValue, System.DBNull.Value)
		Rs("PageTypeID") = zPage.PageTypeID.DbValue

		' Active
		zPage.Active.SetDbValue((zPage.Active.CurrentValue <> "" And Not IsDBNull(zPage.Active.CurrentValue)), System.DBNull.Value)
		Rs("Active") = zPage.Active.DbValue

		' PageName
		zPage.zPageName.SetDbValue(zPage.zPageName.CurrentValue, "")
		Rs("PageName") = zPage.zPageName.DbValue

		' SiteCategoryID
		zPage.SiteCategoryID.SetDbValue(zPage.SiteCategoryID.CurrentValue, System.DBNull.Value)
		Rs("SiteCategoryID") = zPage.SiteCategoryID.DbValue

		' SiteCategoryGroupID
		zPage.SiteCategoryGroupID.SetDbValue(zPage.SiteCategoryGroupID.CurrentValue, System.DBNull.Value)
		Rs("SiteCategoryGroupID") = zPage.SiteCategoryGroupID.DbValue

		' ModifiedDT
		zPage.ModifiedDT.DbValue = ew_CurrentDate()
		Rs("ModifiedDT") = zPage.ModifiedDT.DbValue

		' Row Inserting event
		bInsertRow = zPage.Row_Inserting(Rs)
		If bInsertRow Then
			Try	
				zPage.Insert(Rs)
				AddRow = True
			Catch e As Exception
				If EW_DEBUG_ENABLED Then Throw
				Message = e.Message				
				AddRow = False
			End Try
		Else
			If zPage.CancelMessage <> "" Then
				Message = zPage.CancelMessage
				zPage.CancelMessage = ""
			Else
				Message = "Insert cancelled"
			End If
			AddRow = False
		End If
		If AddRow Then
			LastInsertId = Conn.GetLastInsertId()
			zPage.zPageID.DbValue = LastInsertId
			Rs("PageID") = zPage.zPageID.DbValue		

			' Row Inserted event
			zPage.Row_Inserted(Rs)
			WriteAuditTrailOnAdd(Rs)
		End If
	End Function

	'
	' Load advanced search
	'
	Sub LoadAdvancedSearch()
		zPage.CompanyID.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_CompanyID")
		zPage.GroupID.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_GroupID")
		zPage.ParentPageID.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_ParentPageID")
		zPage.PageTypeID.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_PageTypeID")
		zPage.Active.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_Active")
		zPage.zPageName.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_zPageName")
		zPage.PageTitle.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_PageTitle")
		zPage.PageDescription.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_PageDescription")
		zPage.PageKeywords.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_PageKeywords")
		zPage.SiteCategoryID.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_SiteCategoryID")
		zPage.SiteCategoryGroupID.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_SiteCategoryGroupID")
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
		If zPage.ExportAll Then
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
		If zPage.Export = "xml" Then
			oXmlDoc = New XmlDocument()
			oXmlTbl = oXmlDoc.CreateElement("table")
			oXmlDoc.AppendChild(oXmlTbl)
		Else
			ew_Write(ew_ExportHeader(zPage.Export))

			' Horizontal format, write header
			If sExportStyle <> "v" OrElse zPage.Export = "csv" Then
				sExportStr = ""
				ew_ExportAddValue(sExportStr, "CompanyID", zPage.Export)
				ew_ExportAddValue(sExportStr, "PageOrder", zPage.Export)
				ew_ExportAddValue(sExportStr, "GroupID", zPage.Export)
				ew_ExportAddValue(sExportStr, "ParentPageID", zPage.Export)
				ew_ExportAddValue(sExportStr, "PageTypeID", zPage.Export)
				ew_ExportAddValue(sExportStr, "Active", zPage.Export)
				ew_ExportAddValue(sExportStr, "PageName", zPage.Export)
				ew_ExportAddValue(sExportStr, "PageTitle", zPage.Export)
				ew_ExportAddValue(sExportStr, "PageDescription", zPage.Export)
				ew_ExportAddValue(sExportStr, "PageKeywords", zPage.Export)
				ew_ExportAddValue(sExportStr, "ImagesPerRow", zPage.Export)
				ew_ExportAddValue(sExportStr, "RowsPerPage", zPage.Export)
				ew_ExportAddValue(sExportStr, "AllowMessage", zPage.Export)
				ew_ExportAddValue(sExportStr, "PageFileName", zPage.Export)
				ew_ExportAddValue(sExportStr, "VersionNo", zPage.Export)
				ew_ExportAddValue(sExportStr, "SiteCategoryID", zPage.Export)
				ew_ExportAddValue(sExportStr, "SiteCategoryGroupID", zPage.Export)
				ew_ExportAddValue(sExportStr, "ModifiedDT", zPage.Export)
				ew_Write(ew_ExportLine(sExportStr, zPage.Export))
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
				zPage.RowType = EW_ROWTYPE_VIEW ' Render view
				RenderRow()
				If zPage.Export = "xml" Then
					oXmlRec = oXmlDoc.CreateElement("record")
					oXmlTbl.AppendChild(oXmlRec)
					oXmlFld = oXmlDoc.CreateElement("CompanyID") ' CompanyID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(zPage.CompanyID.ExportValue(zPage.Export, zPage.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("PageOrder") ' PageOrder
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(zPage.PageOrder.ExportValue(zPage.Export, zPage.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("GroupID") ' GroupID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(zPage.GroupID.ExportValue(zPage.Export, zPage.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("ParentPageID") ' ParentPageID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(zPage.ParentPageID.ExportValue(zPage.Export, zPage.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("PageTypeID") ' PageTypeID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(zPage.PageTypeID.ExportValue(zPage.Export, zPage.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("Active") ' Active
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(zPage.Active.ExportValue(zPage.Export, zPage.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("zPageName") ' PageName
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(zPage.zPageName.ExportValue(zPage.Export, zPage.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("PageTitle") ' PageTitle
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(zPage.PageTitle.ExportValue(zPage.Export, zPage.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("PageDescription") ' PageDescription
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(zPage.PageDescription.ExportValue(zPage.Export, zPage.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("PageKeywords") ' PageKeywords
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(zPage.PageKeywords.ExportValue(zPage.Export, zPage.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("ImagesPerRow") ' ImagesPerRow
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(zPage.ImagesPerRow.ExportValue(zPage.Export, zPage.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("RowsPerPage") ' RowsPerPage
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(zPage.RowsPerPage.ExportValue(zPage.Export, zPage.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("AllowMessage") ' AllowMessage
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(zPage.AllowMessage.ExportValue(zPage.Export, zPage.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("PageFileName") ' PageFileName
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(zPage.PageFileName.ExportValue(zPage.Export, zPage.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("VersionNo") ' VersionNo
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(zPage.VersionNo.ExportValue(zPage.Export, zPage.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("SiteCategoryID") ' SiteCategoryID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(zPage.SiteCategoryID.ExportValue(zPage.Export, zPage.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("SiteCategoryGroupID") ' SiteCategoryGroupID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(zPage.SiteCategoryGroupID.ExportValue(zPage.Export, zPage.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("ModifiedDT") ' ModifiedDT
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(zPage.ModifiedDT.ExportValue(zPage.Export, zPage.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
				Else

					' Vertical format
					If sExportStyle = "v" AndAlso zPage.Export <> "csv" Then
						ew_Write(ew_ExportField("CompanyID", zPage.CompanyID.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export)) ' CompanyID
						ew_Write(ew_ExportField("PageOrder", zPage.PageOrder.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export)) ' PageOrder
						ew_Write(ew_ExportField("GroupID", zPage.GroupID.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export)) ' GroupID
						ew_Write(ew_ExportField("ParentPageID", zPage.ParentPageID.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export)) ' ParentPageID
						ew_Write(ew_ExportField("PageTypeID", zPage.PageTypeID.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export)) ' PageTypeID
						ew_Write(ew_ExportField("Active", zPage.Active.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export)) ' Active
						ew_Write(ew_ExportField("PageName", zPage.zPageName.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export)) ' PageName
						ew_Write(ew_ExportField("PageTitle", zPage.PageTitle.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export)) ' PageTitle
						ew_Write(ew_ExportField("PageDescription", zPage.PageDescription.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export)) ' PageDescription
						ew_Write(ew_ExportField("PageKeywords", zPage.PageKeywords.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export)) ' PageKeywords
						ew_Write(ew_ExportField("ImagesPerRow", zPage.ImagesPerRow.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export)) ' ImagesPerRow
						ew_Write(ew_ExportField("RowsPerPage", zPage.RowsPerPage.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export)) ' RowsPerPage
						ew_Write(ew_ExportField("AllowMessage", zPage.AllowMessage.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export)) ' AllowMessage
						ew_Write(ew_ExportField("PageFileName", zPage.PageFileName.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export)) ' PageFileName
						ew_Write(ew_ExportField("VersionNo", zPage.VersionNo.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export)) ' VersionNo
						ew_Write(ew_ExportField("SiteCategoryID", zPage.SiteCategoryID.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export)) ' SiteCategoryID
						ew_Write(ew_ExportField("SiteCategoryGroupID", zPage.SiteCategoryGroupID.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export)) ' SiteCategoryGroupID
						ew_Write(ew_ExportField("ModifiedDT", zPage.ModifiedDT.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export)) ' ModifiedDT

					' Horizontal format
					Else
						sExportStr = ""
						ew_ExportAddValue(sExportStr, zPage.CompanyID.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export) ' CompanyID
						ew_ExportAddValue(sExportStr, zPage.PageOrder.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export) ' PageOrder
						ew_ExportAddValue(sExportStr, zPage.GroupID.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export) ' GroupID
						ew_ExportAddValue(sExportStr, zPage.ParentPageID.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export) ' ParentPageID
						ew_ExportAddValue(sExportStr, zPage.PageTypeID.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export) ' PageTypeID
						ew_ExportAddValue(sExportStr, zPage.Active.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export) ' Active
						ew_ExportAddValue(sExportStr, zPage.zPageName.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export) ' PageName
						ew_ExportAddValue(sExportStr, zPage.PageTitle.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export) ' PageTitle
						ew_ExportAddValue(sExportStr, zPage.PageDescription.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export) ' PageDescription
						ew_ExportAddValue(sExportStr, zPage.PageKeywords.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export) ' PageKeywords
						ew_ExportAddValue(sExportStr, zPage.ImagesPerRow.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export) ' ImagesPerRow
						ew_ExportAddValue(sExportStr, zPage.RowsPerPage.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export) ' RowsPerPage
						ew_ExportAddValue(sExportStr, zPage.AllowMessage.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export) ' AllowMessage
						ew_ExportAddValue(sExportStr, zPage.PageFileName.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export) ' PageFileName
						ew_ExportAddValue(sExportStr, zPage.VersionNo.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export) ' VersionNo
						ew_ExportAddValue(sExportStr, zPage.SiteCategoryID.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export) ' SiteCategoryID
						ew_ExportAddValue(sExportStr, zPage.SiteCategoryGroupID.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export) ' SiteCategoryGroupID
						ew_ExportAddValue(sExportStr, zPage.ModifiedDT.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export) ' ModifiedDT
						ew_Write(ew_ExportLine(sExportStr, zPage.Export))
					End If
				End If
			End If
		Loop

		' Close recordset
		Rs.Close()
		Rs.Dispose()
		If zPage.Export = "xml" Then
			ew_Write("<?xml version=""1.0"" standalone=""yes""?>" & vbcrlf)
			ew_Write(oXmlDoc.OuterXml)
		Else
			ew_Write(ew_ExportFooter(zPage.Export))
		End If
	End Sub

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "Page"
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
		Dim table As String = "Page"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsSrc("PageID")

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

		' PageOrder Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "PageOrder", keyvalue, oldvalue, RsSrc("PageOrder"))

		' ParentPageID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "ParentPageID", keyvalue, oldvalue, RsSrc("ParentPageID"))

		' PageTypeID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "PageTypeID", keyvalue, oldvalue, RsSrc("PageTypeID"))

		' Active Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "Active", keyvalue, oldvalue, RsSrc("Active"))

		' PageName Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "PageName", keyvalue, oldvalue, RsSrc("PageName"))

		' SiteCategoryID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteCategoryID", keyvalue, oldvalue, RsSrc("SiteCategoryID"))

		' SiteCategoryGroupID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteCategoryGroupID", keyvalue, oldvalue, RsSrc("SiteCategoryGroupID"))

		' ModifiedDT Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "ModifiedDT", keyvalue, oldvalue, RsSrc("ModifiedDT"))
	End Sub

	' Write Audit Trail (edit page)
	Sub WriteAuditTrailOnEdit(ByRef RsOld As OrderedDictionary, ByRef RsNew As OrderedDictionary)
		Dim table As String = "Page"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsOld("PageID")

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
			fld = zPage.FieldByName(fldname)
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
		zPage_list = New czPage_list(Me)		
		zPage_list.Page_Init()

		' Page main processing
		zPage_list.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If zPage_list IsNot Nothing Then zPage_list.Dispose()
	End Sub
End Class
