Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class SiteCategoryType_list
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public SiteCategoryType_list As cSiteCategoryType_list

	'
	' Page Class
	'
	Class cSiteCategoryType_list
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
				If SiteCategoryType.UseTokenInUrl Then Url = Url & "t=" & SiteCategoryType.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If SiteCategoryType.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (SiteCategoryType.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (SiteCategoryType.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' SiteCategoryType
		Public Property SiteCategoryType() As cSiteCategoryType
			Get				
				Return ParentPage.SiteCategoryType
			End Get
			Set(ByVal v As cSiteCategoryType)
				ParentPage.SiteCategoryType = v	
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
			m_PageObjName = "SiteCategoryType_list"
			m_PageObjTypeName = "cSiteCategoryType_list"

			' Table Name
			m_TableName = "SiteCategoryType"

			' Initialize table object
			SiteCategoryType = New cSiteCategoryType(Me)

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
			SiteCategoryType.Export = ew_Get("export") ' Get export parameter
			ParentPage.gsExport = SiteCategoryType.Export ' Get export parameter, used in header
			ParentPage.gsExportFile = SiteCategoryType.TableVar ' Get export file, used in header
			If SiteCategoryType.Export = "excel" Then
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
			SiteCategoryType.Dispose()
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
					SiteCategoryType.CurrentAction = ew_Get("a")

					' Clear inline mode
					If SiteCategoryType.CurrentAction = "cancel" Then
						ClearInlineMode()
					End If

					' Switch to grid edit mode
					If SiteCategoryType.CurrentAction = "gridedit" Then
						GridEditMode()
					End If

					' Switch to grid add mode
					If SiteCategoryType.CurrentAction = "gridadd" Then
						GridAddMode()
					End If
				End If
			Else
				SiteCategoryType.CurrentAction = ObjForm.GetValue("a_list") ' Get action

				' Grid Update
				If SiteCategoryType.CurrentAction = "gridupdate" AndAlso ew_SameStr(ew_Session(EW_SESSION_INLINE_MODE), "gridedit") Then
					GridUpdate()
				End If

				' Grid Insert
				If SiteCategoryType.CurrentAction = "gridinsert" AndAlso ew_SameStr(ew_Session(EW_SESSION_INLINE_MODE), "gridadd") Then
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
		If (SiteCategoryType.RecordsPerPage = -1 OrElse SiteCategoryType.RecordsPerPage > 0) Then
			lDisplayRecs = SiteCategoryType.RecordsPerPage ' Restore from Session
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
		SiteCategoryType.Recordset_Searching(sSrchWhere)

		' Save search criteria
		If sSrchWhere <> "" Then
			If sSrchBasic = "" Then ResetBasicSearchParms()
			If sSrchAdvanced = "" Then ResetAdvancedSearchParms()
			SiteCategoryType.SearchWhere = sSrchWhere ' Save to Session
			lStartRec = 1 ' Reset start record counter
			SiteCategoryType.StartRecordNumber = lStartRec
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
		SiteCategoryType.SessionWhere = sFilter
		SiteCategoryType.CurrentFilter = ""

		' Export Data only
		If SiteCategoryType.Export = "html" OrElse SiteCategoryType.Export = "csv" OrElse SiteCategoryType.Export = "word" OrElse SiteCategoryType.Export = "excel" OrElse SiteCategoryType.Export = "xml" Then
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
			SiteCategoryType.RecordsPerPage = lDisplayRecs ' Save to Session

			' Reset start position
			lStartRec = 1
			SiteCategoryType.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	'  Exit out of inline mode
	'
	Sub ClearInlineMode()
		SiteCategoryType.CurrentAction = "" ' Clear action
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
		SiteCategoryType.CurrentFilter = BuildKeyFilter()
		sSql = SiteCategoryType.SQL
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
					SiteCategoryType.SendEmail = False ' Do not send email on update success
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
			SiteCategoryType.EventCancelled = True ' Set event cancelled
			SiteCategoryType.CurrentAction = "gridedit" ' Stay in gridedit mode
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
				sFilter = SiteCategoryType.KeyFilter
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
			SiteCategoryType.SiteCategoryTypeID.FormValue = arKeyFlds(0)
			If Not IsNumeric(SiteCategoryType.SiteCategoryTypeID.FormValue) Then	Return False
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
				SiteCategoryType.SendEmail = False ' Do not send email on insert success

				' Validate Form
				If Not ValidateForm() Then
					bGridInsert = False ' Form error, reset action
					Message = ParentPage.gsFormError
				Else
					bGridInsert = AddRow() ' Insert this row
				End If
				If bGridInsert Then
					If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
					sKey = sKey & SiteCategoryType.SiteCategoryTypeID.CurrentValue

					' Add filter for this record
					sFilter = SiteCategoryType.KeyFilter
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
			SiteCategoryType.CurrentFilter = sWrkFilter
			sSql = SiteCategoryType.SQL
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
			SiteCategoryType.EventCancelled = True ' Set event cancelled
			SiteCategoryType.CurrentAction = "gridadd" ' Stay in gridadd mode
		End If
	End Sub

	Function EmptyRow() As Boolean
		Dim Empty As Boolean = True
		If Empty Then Empty = ew_SameStr(SiteCategoryType.SiteCategoryTypeNM.CurrentValue, SiteCategoryType.SiteCategoryTypeNM.OldValue)
		If Empty Then Empty = ew_SameStr(SiteCategoryType.SiteCategoryFileName.CurrentValue, SiteCategoryType.SiteCategoryFileName.OldValue)
		If Empty Then Empty = ew_SameStr(SiteCategoryType.SiteCategoryTransferURL.CurrentValue, SiteCategoryType.SiteCategoryTransferURL.OldValue)
		If Empty Then Empty = ew_SameStr(SiteCategoryType.DefaultSiteCategoryID.CurrentValue, SiteCategoryType.DefaultSiteCategoryID.OldValue)
		Return Empty
	End Function

	'
	' Restore form values for current row
	'
	Sub RestoreCurrentRowFormValues(idx As Integer)
		Dim sKey As String, arKeyFlds() As String

		' Get row based on current index
		ObjForm.Index = idx
		If SiteCategoryType.CurrentAction = "gridadd" Then
			LoadFormValues() ' Load form values
		End If
		If SiteCategoryType.CurrentAction = "gridedit" Then
			sKey = ObjForm.GetValue("k_key")
			arKeyFlds = sKey.Split(New Char() {Convert.ToChar(EW_COMPOSITE_KEY_SEPARATOR)})
			If arKeyFlds.Length >= 1 Then
				If ew_SameStr(arKeyFlds(0), SiteCategoryType.SiteCategoryTypeID.CurrentValue) Then
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
		BuildSearchSql(sWhere, SiteCategoryType.SiteCategoryTypeNM, False) ' SiteCategoryTypeNM
		BuildSearchSql(sWhere, SiteCategoryType.SiteCategoryTypeDS, False) ' SiteCategoryTypeDS
		BuildSearchSql(sWhere, SiteCategoryType.SiteCategoryComment, False) ' SiteCategoryComment
		BuildSearchSql(sWhere, SiteCategoryType.SiteCategoryFileName, False) ' SiteCategoryFileName
		BuildSearchSql(sWhere, SiteCategoryType.SiteCategoryTransferURL, False) ' SiteCategoryTransferURL
		BuildSearchSql(sWhere, SiteCategoryType.DefaultSiteCategoryID, False) ' DefaultSiteCategoryID

		' Set up search parm
		If sWhere <> "" Then
			SetSearchParm(SiteCategoryType.SiteCategoryTypeNM) ' SiteCategoryTypeNM
			SetSearchParm(SiteCategoryType.SiteCategoryTypeDS) ' SiteCategoryTypeDS
			SetSearchParm(SiteCategoryType.SiteCategoryComment) ' SiteCategoryComment
			SetSearchParm(SiteCategoryType.SiteCategoryFileName) ' SiteCategoryFileName
			SetSearchParm(SiteCategoryType.SiteCategoryTransferURL) ' SiteCategoryTransferURL
			SetSearchParm(SiteCategoryType.DefaultSiteCategoryID) ' DefaultSiteCategoryID
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
		SiteCategoryType.SetAdvancedSearch("x_" & FldParm, Fld.AdvancedSearch.SearchValue)
		SiteCategoryType.SetAdvancedSearch("z_" & FldParm, Fld.AdvancedSearch.SearchOperator)
		SiteCategoryType.SetAdvancedSearch("v_" & FldParm, Fld.AdvancedSearch.SearchCondition)
		SiteCategoryType.SetAdvancedSearch("y_" & FldParm, Fld.AdvancedSearch.SearchValue2)
		SiteCategoryType.SetAdvancedSearch("w_" & FldParm, Fld.AdvancedSearch.SearchOperator2)
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
		If IsNumeric(sKeyword) Then sSql = sSql & "[SiteCategoryTypeID] = " & sKeyword & " OR "
		sSql = sSql & "[SiteCategoryTypeNM] LIKE '%" & sKeyword & "%' OR "
		sSql = sSql & "[SiteCategoryTypeDS] LIKE '%" & sKeyword & "%' OR "
		sSql = sSql & "[SiteCategoryComment] LIKE '%" & sKeyword & "%' OR "
		sSql = sSql & "[SiteCategoryFileName] LIKE '%" & sKeyword & "%' OR "
		sSql = sSql & "[SiteCategoryTransferURL] LIKE '%" & sKeyword & "%' OR "
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
			SiteCategoryType.BasicSearchKeyword = sSearchKeyword
			SiteCategoryType.BasicSearchType = sSearchType
		End If
		Return sSearchStr
	End Function

	'
	' Clear all search parameters
	'
	Sub ResetSearchParms()

		' Clear search where
		sSrchWhere = ""
		SiteCategoryType.SearchWhere = sSrchWhere

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
		SiteCategoryType.BasicSearchKeyword = ""
		SiteCategoryType.BasicSearchType = ""
	End Sub

	'
	' Clear all advanced search parameters
	'
	Sub ResetAdvancedSearchParms()
		SiteCategoryType.SetAdvancedSearch("x_SiteCategoryTypeNM", "")
		SiteCategoryType.SetAdvancedSearch("x_SiteCategoryTypeDS", "")
		SiteCategoryType.SetAdvancedSearch("x_SiteCategoryComment", "")
		SiteCategoryType.SetAdvancedSearch("x_SiteCategoryFileName", "")
		SiteCategoryType.SetAdvancedSearch("x_SiteCategoryTransferURL", "")
		SiteCategoryType.SetAdvancedSearch("x_DefaultSiteCategoryID", "")
	End Sub

	'
	' Restore all search parameters
	'
	Sub RestoreSearchParms()
		sSrchWhere = SiteCategoryType.SearchWhere

		' Restore advanced search settings
		If ParentPage.gsSearchError = "" Then
			RestoreAdvancedSearchParms()
		End If
	End Sub

	'
	' Restore all advanced search parameters
	'
	Sub RestoreAdvancedSearchParms()
		SiteCategoryType.SiteCategoryTypeNM.AdvancedSearch.SearchValue = SiteCategoryType.GetAdvancedSearch("x_SiteCategoryTypeNM")
		SiteCategoryType.SiteCategoryTypeDS.AdvancedSearch.SearchValue = SiteCategoryType.GetAdvancedSearch("x_SiteCategoryTypeDS")
		SiteCategoryType.SiteCategoryComment.AdvancedSearch.SearchValue = SiteCategoryType.GetAdvancedSearch("x_SiteCategoryComment")
		SiteCategoryType.SiteCategoryFileName.AdvancedSearch.SearchValue = SiteCategoryType.GetAdvancedSearch("x_SiteCategoryFileName")
		SiteCategoryType.SiteCategoryTransferURL.AdvancedSearch.SearchValue = SiteCategoryType.GetAdvancedSearch("x_SiteCategoryTransferURL")
		SiteCategoryType.DefaultSiteCategoryID.AdvancedSearch.SearchValue = SiteCategoryType.GetAdvancedSearch("x_DefaultSiteCategoryID")
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
			SiteCategoryType.CurrentOrder = ew_Get("order")
			SiteCategoryType.CurrentOrderType = ew_Get("ordertype")
			SiteCategoryType.UpdateSort(SiteCategoryType.SiteCategoryTypeID) ' SiteCategoryTypeID
			SiteCategoryType.UpdateSort(SiteCategoryType.SiteCategoryTypeNM) ' SiteCategoryTypeNM
			SiteCategoryType.UpdateSort(SiteCategoryType.SiteCategoryFileName) ' SiteCategoryFileName
			SiteCategoryType.UpdateSort(SiteCategoryType.SiteCategoryTransferURL) ' SiteCategoryTransferURL
			SiteCategoryType.UpdateSort(SiteCategoryType.DefaultSiteCategoryID) ' DefaultSiteCategoryID
			SiteCategoryType.StartRecordNumber = 1 ' Reset start position
		End If
	End Sub

	'
	' Load Sort Order parameters
	'
	Sub LoadSortOrder()
		Dim sOrderBy As String
		sOrderBy = SiteCategoryType.SessionOrderBy ' Get order by from Session
		If sOrderBy = "" Then
			If SiteCategoryType.SqlOrderBy <> "" Then
				sOrderBy = SiteCategoryType.SqlOrderBy
				SiteCategoryType.SessionOrderBy = sOrderBy
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
				SiteCategoryType.SessionOrderBy = sOrderBy
				SiteCategoryType.SiteCategoryTypeID.Sort = ""
				SiteCategoryType.SiteCategoryTypeNM.Sort = ""
				SiteCategoryType.SiteCategoryFileName.Sort = ""
				SiteCategoryType.SiteCategoryTransferURL.Sort = ""
				SiteCategoryType.DefaultSiteCategoryID.Sort = ""
			End If

			' Reset start position
			lStartRec = 1
			SiteCategoryType.StartRecordNumber = lStartRec
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
				SiteCategoryType.StartRecordNumber = lStartRec
			ElseIf ew_Get(EW_TABLE_PAGE_NO) <> "" AndAlso IsNumeric(ew_Get(EW_TABLE_PAGE_NO)) Then
				nPageNo = ew_ConvertToInt(ew_Get(EW_TABLE_PAGE_NO))
				lStartRec = (nPageNo-1)*lDisplayRecs+1
				If lStartRec <= 0 Then
					lStartRec = 1
				ElseIf lStartRec >= ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 Then
					lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1
				End If
				SiteCategoryType.StartRecordNumber = lStartRec
			End If
		End If
		lStartRec = SiteCategoryType.StartRecordNumber

		' Check if correct start record counter
		If lStartRec <= 0 Then ' Avoid invalid start record counter
			lStartRec = 1 ' Reset start record counter
			SiteCategoryType.StartRecordNumber = lStartRec
		ElseIf lStartRec > lTotalRecs Then ' Avoid starting record > total records
			lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to last page first record
			SiteCategoryType.StartRecordNumber = lStartRec
		ElseIf (lStartRec-1) Mod lDisplayRecs <> 0 Then
			lStartRec = ((lStartRec-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to page boundary
			SiteCategoryType.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	' Load default values
	'
	Sub LoadDefaultValues()
		SiteCategoryType.SiteCategoryTypeID.OldValue = SiteCategoryType.SiteCategoryTypeID.CurrentValue
		SiteCategoryType.SiteCategoryTypeNM.OldValue = SiteCategoryType.SiteCategoryTypeNM.CurrentValue
		SiteCategoryType.SiteCategoryFileName.OldValue = SiteCategoryType.SiteCategoryFileName.CurrentValue
		SiteCategoryType.SiteCategoryTransferURL.OldValue = SiteCategoryType.SiteCategoryTransferURL.CurrentValue
		SiteCategoryType.DefaultSiteCategoryID.OldValue = SiteCategoryType.DefaultSiteCategoryID.CurrentValue
	End Sub

	'
	'  Load search values for validation
	'
	Sub LoadSearchValues()
		SiteCategoryType.SiteCategoryTypeNM.AdvancedSearch.SearchValue = ew_Get("x_SiteCategoryTypeNM")
    	SiteCategoryType.SiteCategoryTypeNM.AdvancedSearch.SearchOperator = ew_Get("z_SiteCategoryTypeNM")
		SiteCategoryType.SiteCategoryTypeDS.AdvancedSearch.SearchValue = ew_Get("x_SiteCategoryTypeDS")
    	SiteCategoryType.SiteCategoryTypeDS.AdvancedSearch.SearchOperator = ew_Get("z_SiteCategoryTypeDS")
		SiteCategoryType.SiteCategoryComment.AdvancedSearch.SearchValue = ew_Get("x_SiteCategoryComment")
    	SiteCategoryType.SiteCategoryComment.AdvancedSearch.SearchOperator = ew_Get("z_SiteCategoryComment")
		SiteCategoryType.SiteCategoryFileName.AdvancedSearch.SearchValue = ew_Get("x_SiteCategoryFileName")
    	SiteCategoryType.SiteCategoryFileName.AdvancedSearch.SearchOperator = ew_Get("z_SiteCategoryFileName")
		SiteCategoryType.SiteCategoryTransferURL.AdvancedSearch.SearchValue = ew_Get("x_SiteCategoryTransferURL")
    	SiteCategoryType.SiteCategoryTransferURL.AdvancedSearch.SearchOperator = ew_Get("z_SiteCategoryTransferURL")
		SiteCategoryType.DefaultSiteCategoryID.AdvancedSearch.SearchValue = ew_Get("x_DefaultSiteCategoryID")
    	SiteCategoryType.DefaultSiteCategoryID.AdvancedSearch.SearchOperator = ew_Get("z_DefaultSiteCategoryID")
	End Sub

	'
	' Load form values
	'
	Sub LoadFormValues()
		SiteCategoryType.SiteCategoryTypeID.FormValue = ObjForm.GetValue("x_SiteCategoryTypeID")
		SiteCategoryType.SiteCategoryTypeID.OldValue = ObjForm.GetValue("o_SiteCategoryTypeID")
		SiteCategoryType.SiteCategoryTypeNM.FormValue = ObjForm.GetValue("x_SiteCategoryTypeNM")
		SiteCategoryType.SiteCategoryTypeNM.OldValue = ObjForm.GetValue("o_SiteCategoryTypeNM")
		SiteCategoryType.SiteCategoryFileName.FormValue = ObjForm.GetValue("x_SiteCategoryFileName")
		SiteCategoryType.SiteCategoryFileName.OldValue = ObjForm.GetValue("o_SiteCategoryFileName")
		SiteCategoryType.SiteCategoryTransferURL.FormValue = ObjForm.GetValue("x_SiteCategoryTransferURL")
		SiteCategoryType.SiteCategoryTransferURL.OldValue = ObjForm.GetValue("o_SiteCategoryTransferURL")
		SiteCategoryType.DefaultSiteCategoryID.FormValue = ObjForm.GetValue("x_DefaultSiteCategoryID")
		SiteCategoryType.DefaultSiteCategoryID.OldValue = ObjForm.GetValue("o_DefaultSiteCategoryID")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		SiteCategoryType.SiteCategoryTypeID.CurrentValue = SiteCategoryType.SiteCategoryTypeID.FormValue
		SiteCategoryType.SiteCategoryTypeNM.CurrentValue = SiteCategoryType.SiteCategoryTypeNM.FormValue
		SiteCategoryType.SiteCategoryFileName.CurrentValue = SiteCategoryType.SiteCategoryFileName.FormValue
		SiteCategoryType.SiteCategoryTransferURL.CurrentValue = SiteCategoryType.SiteCategoryTransferURL.FormValue
		SiteCategoryType.DefaultSiteCategoryID.CurrentValue = SiteCategoryType.DefaultSiteCategoryID.FormValue
	End Sub

	'
	' Load recordset
	'
	Function LoadRecordset() As OleDbDataReader

		' Recordset Selecting event
		SiteCategoryType.Recordset_Selecting(SiteCategoryType.CurrentFilter)

		' Load list page SQL
		Dim sSql As String = SiteCategoryType.ListSQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)

		' Count
		lTotalRecs = -1		
		Try			
			If sSql.StartsWith("SELECT * FROM ", StringComparison.InvariantCultureIgnoreCase) AndAlso _				
				ew_Empty(SiteCategoryType.SqlGroupBy) AndAlso _
				ew_Empty(SiteCategoryType.SqlHaving) Then
				Dim sCntSql As String = SiteCategoryType.SelectCountSQL

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
		SiteCategoryType.Recordset_Selected(Rs)
		Return Rs
	End Function

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = SiteCategoryType.KeyFilter

		' Row Selecting event
		SiteCategoryType.Row_Selecting(sFilter)

		' Load SQL based on filter
		SiteCategoryType.CurrentFilter = sFilter
		Dim sSql As String = SiteCategoryType.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				SiteCategoryType.Row_Selected(RsRow)
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
		SiteCategoryType.SiteCategoryTypeID.DbValue = RsRow("SiteCategoryTypeID")
		SiteCategoryType.SiteCategoryTypeNM.DbValue = RsRow("SiteCategoryTypeNM")
		SiteCategoryType.SiteCategoryTypeDS.DbValue = RsRow("SiteCategoryTypeDS")
		SiteCategoryType.SiteCategoryComment.DbValue = RsRow("SiteCategoryComment")
		SiteCategoryType.SiteCategoryFileName.DbValue = RsRow("SiteCategoryFileName")
		SiteCategoryType.SiteCategoryTransferURL.DbValue = RsRow("SiteCategoryTransferURL")
		SiteCategoryType.DefaultSiteCategoryID.DbValue = RsRow("DefaultSiteCategoryID")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		SiteCategoryType.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' SiteCategoryTypeID

		SiteCategoryType.SiteCategoryTypeID.CellCssStyle = "white-space: nowrap;"
		SiteCategoryType.SiteCategoryTypeID.CellCssClass = ""

		' SiteCategoryTypeNM
		SiteCategoryType.SiteCategoryTypeNM.CellCssStyle = "white-space: nowrap;"
		SiteCategoryType.SiteCategoryTypeNM.CellCssClass = ""

		' SiteCategoryFileName
		SiteCategoryType.SiteCategoryFileName.CellCssStyle = "white-space: nowrap;"
		SiteCategoryType.SiteCategoryFileName.CellCssClass = ""

		' SiteCategoryTransferURL
		SiteCategoryType.SiteCategoryTransferURL.CellCssStyle = "white-space: nowrap;"
		SiteCategoryType.SiteCategoryTransferURL.CellCssClass = ""

		' DefaultSiteCategoryID
		SiteCategoryType.DefaultSiteCategoryID.CellCssStyle = "white-space: nowrap;"
		SiteCategoryType.DefaultSiteCategoryID.CellCssClass = ""

		'
		'  View  Row
		'

		If SiteCategoryType.RowType = EW_ROWTYPE_VIEW Then ' View row

			' SiteCategoryTypeID
			SiteCategoryType.SiteCategoryTypeID.ViewValue = SiteCategoryType.SiteCategoryTypeID.CurrentValue
			SiteCategoryType.SiteCategoryTypeID.CssStyle = ""
			SiteCategoryType.SiteCategoryTypeID.CssClass = ""
			SiteCategoryType.SiteCategoryTypeID.ViewCustomAttributes = ""

			' SiteCategoryTypeNM
			SiteCategoryType.SiteCategoryTypeNM.ViewValue = SiteCategoryType.SiteCategoryTypeNM.CurrentValue
			SiteCategoryType.SiteCategoryTypeNM.CssStyle = ""
			SiteCategoryType.SiteCategoryTypeNM.CssClass = ""
			SiteCategoryType.SiteCategoryTypeNM.ViewCustomAttributes = ""

			' SiteCategoryTypeDS
			SiteCategoryType.SiteCategoryTypeDS.ViewValue = SiteCategoryType.SiteCategoryTypeDS.CurrentValue
			SiteCategoryType.SiteCategoryTypeDS.CssStyle = ""
			SiteCategoryType.SiteCategoryTypeDS.CssClass = ""
			SiteCategoryType.SiteCategoryTypeDS.ViewCustomAttributes = ""

			' SiteCategoryComment
			SiteCategoryType.SiteCategoryComment.ViewValue = SiteCategoryType.SiteCategoryComment.CurrentValue
			SiteCategoryType.SiteCategoryComment.CssStyle = ""
			SiteCategoryType.SiteCategoryComment.CssClass = ""
			SiteCategoryType.SiteCategoryComment.ViewCustomAttributes = ""

			' SiteCategoryFileName
			SiteCategoryType.SiteCategoryFileName.ViewValue = SiteCategoryType.SiteCategoryFileName.CurrentValue
			SiteCategoryType.SiteCategoryFileName.CssStyle = ""
			SiteCategoryType.SiteCategoryFileName.CssClass = ""
			SiteCategoryType.SiteCategoryFileName.ViewCustomAttributes = ""

			' SiteCategoryTransferURL
			SiteCategoryType.SiteCategoryTransferURL.ViewValue = SiteCategoryType.SiteCategoryTransferURL.CurrentValue
			SiteCategoryType.SiteCategoryTransferURL.CssStyle = ""
			SiteCategoryType.SiteCategoryTransferURL.CssClass = ""
			SiteCategoryType.SiteCategoryTransferURL.ViewCustomAttributes = ""

			' DefaultSiteCategoryID
			If ew_NotEmpty(SiteCategoryType.DefaultSiteCategoryID.CurrentValue) Then
				sSqlWrk = "SELECT [CategoryName], [SiteCategoryTypeID] FROM [SiteCategory] WHERE [SiteCategoryID] = " & ew_AdjustSql(SiteCategoryType.DefaultSiteCategoryID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryTypeID] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					SiteCategoryType.DefaultSiteCategoryID.ViewValue = RsWrk("CategoryName")
					SiteCategoryType.DefaultSiteCategoryID.ViewValue = SiteCategoryType.DefaultSiteCategoryID.ViewValue & ew_ValueSeparator(0) & RsWrk("SiteCategoryTypeID")
				Else
					SiteCategoryType.DefaultSiteCategoryID.ViewValue = SiteCategoryType.DefaultSiteCategoryID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				SiteCategoryType.DefaultSiteCategoryID.ViewValue = System.DBNull.Value
			End If
			SiteCategoryType.DefaultSiteCategoryID.CssStyle = ""
			SiteCategoryType.DefaultSiteCategoryID.CssClass = ""
			SiteCategoryType.DefaultSiteCategoryID.ViewCustomAttributes = ""

			' View refer script
			' SiteCategoryTypeID

			SiteCategoryType.SiteCategoryTypeID.HrefValue = ""

			' SiteCategoryTypeNM
			SiteCategoryType.SiteCategoryTypeNM.HrefValue = ""

			' SiteCategoryFileName
			SiteCategoryType.SiteCategoryFileName.HrefValue = ""

			' SiteCategoryTransferURL
			SiteCategoryType.SiteCategoryTransferURL.HrefValue = ""

			' DefaultSiteCategoryID
			SiteCategoryType.DefaultSiteCategoryID.HrefValue = ""

		'
		'  Add Row
		'

		ElseIf SiteCategoryType.RowType = EW_ROWTYPE_ADD Then ' Add row

			' SiteCategoryTypeID
			' SiteCategoryTypeNM

			SiteCategoryType.SiteCategoryTypeNM.EditCustomAttributes = ""
			SiteCategoryType.SiteCategoryTypeNM.EditValue = ew_HtmlEncode(SiteCategoryType.SiteCategoryTypeNM.CurrentValue)

			' SiteCategoryFileName
			SiteCategoryType.SiteCategoryFileName.EditCustomAttributes = ""
			SiteCategoryType.SiteCategoryFileName.EditValue = ew_HtmlEncode(SiteCategoryType.SiteCategoryFileName.CurrentValue)

			' SiteCategoryTransferURL
			SiteCategoryType.SiteCategoryTransferURL.EditCustomAttributes = ""
			SiteCategoryType.SiteCategoryTransferURL.EditValue = ew_HtmlEncode(SiteCategoryType.SiteCategoryTransferURL.CurrentValue)

			' DefaultSiteCategoryID
			SiteCategoryType.DefaultSiteCategoryID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryID], [CategoryName], [SiteCategoryTypeID], '' AS SelectFilterFld FROM [SiteCategory]"
			If Convert.ToString(SiteCategoryType.DefaultSiteCategoryID.CurrentValue) = "" Then
				sWhereWrk = "0=1"
			Else
				sWhereWrk = "[SiteCategoryID] = " & ew_AdjustSql(SiteCategoryType.DefaultSiteCategoryID.CurrentValue) & ""
			End If
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryTypeID] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select", ""}) 
			SiteCategoryType.DefaultSiteCategoryID.EditValue = arwrk

		'
		'  Edit Row
		'

		ElseIf SiteCategoryType.RowType = EW_ROWTYPE_EDIT Then ' Edit row

			' SiteCategoryTypeID
			SiteCategoryType.SiteCategoryTypeID.EditCustomAttributes = ""

			' SiteCategoryTypeNM
			SiteCategoryType.SiteCategoryTypeNM.EditCustomAttributes = ""
			SiteCategoryType.SiteCategoryTypeNM.EditValue = ew_HtmlEncode(SiteCategoryType.SiteCategoryTypeNM.CurrentValue)

			' SiteCategoryFileName
			SiteCategoryType.SiteCategoryFileName.EditCustomAttributes = ""
			SiteCategoryType.SiteCategoryFileName.EditValue = ew_HtmlEncode(SiteCategoryType.SiteCategoryFileName.CurrentValue)

			' SiteCategoryTransferURL
			SiteCategoryType.SiteCategoryTransferURL.EditCustomAttributes = ""
			SiteCategoryType.SiteCategoryTransferURL.EditValue = ew_HtmlEncode(SiteCategoryType.SiteCategoryTransferURL.CurrentValue)

			' DefaultSiteCategoryID
			SiteCategoryType.DefaultSiteCategoryID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryID], [CategoryName], [SiteCategoryTypeID], '' AS SelectFilterFld FROM [SiteCategory]"
			If Convert.ToString(SiteCategoryType.DefaultSiteCategoryID.CurrentValue) = "" Then
				sWhereWrk = "0=1"
			Else
				sWhereWrk = "[SiteCategoryID] = " & ew_AdjustSql(SiteCategoryType.DefaultSiteCategoryID.CurrentValue) & ""
			End If
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryTypeID] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select", ""}) 
			SiteCategoryType.DefaultSiteCategoryID.EditValue = arwrk

			' Edit refer script
			' SiteCategoryTypeID

			SiteCategoryType.SiteCategoryTypeID.HrefValue = ""

			' SiteCategoryTypeNM
			SiteCategoryType.SiteCategoryTypeNM.HrefValue = ""

			' SiteCategoryFileName
			SiteCategoryType.SiteCategoryFileName.HrefValue = ""

			' SiteCategoryTransferURL
			SiteCategoryType.SiteCategoryTransferURL.HrefValue = ""

			' DefaultSiteCategoryID
			SiteCategoryType.DefaultSiteCategoryID.HrefValue = ""
		End If

		' Row Rendered event
		SiteCategoryType.Row_Rendered()
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
		sFilter = SiteCategoryType.KeyFilter
		SiteCategoryType.CurrentFilter  = sFilter
		sSql = SiteCategoryType.SQL
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

			' SiteCategoryTypeID
			' SiteCategoryTypeNM

			SiteCategoryType.SiteCategoryTypeNM.SetDbValue(SiteCategoryType.SiteCategoryTypeNM.CurrentValue, System.DBNull.Value)
			Rs("SiteCategoryTypeNM") = SiteCategoryType.SiteCategoryTypeNM.DbValue

			' SiteCategoryFileName
			SiteCategoryType.SiteCategoryFileName.SetDbValue(SiteCategoryType.SiteCategoryFileName.CurrentValue, System.DBNull.Value)
			Rs("SiteCategoryFileName") = SiteCategoryType.SiteCategoryFileName.DbValue

			' SiteCategoryTransferURL
			SiteCategoryType.SiteCategoryTransferURL.SetDbValue(SiteCategoryType.SiteCategoryTransferURL.CurrentValue, System.DBNull.Value)
			Rs("SiteCategoryTransferURL") = SiteCategoryType.SiteCategoryTransferURL.DbValue

			' DefaultSiteCategoryID
			SiteCategoryType.DefaultSiteCategoryID.SetDbValue(SiteCategoryType.DefaultSiteCategoryID.CurrentValue, System.DBNull.Value)
			Rs("DefaultSiteCategoryID") = SiteCategoryType.DefaultSiteCategoryID.DbValue

			' Row Updating event
			bUpdateRow = SiteCategoryType.Row_Updating(RsOld, Rs)
			If bUpdateRow Then
				Try
					SiteCategoryType.Update(Rs)
					EditRow = True
				Catch e As Exception
					If EW_DEBUG_ENABLED Then Throw
					Message = e.Message
					EditRow = False
				End Try
			Else
				If SiteCategoryType.CancelMessage <> "" Then
					Message = SiteCategoryType.CancelMessage
					SiteCategoryType.CancelMessage = ""
				Else
					Message = "Update cancelled"
				End If
				EditRow = False
			End If
		End If

		' Row Updated event
		If EditRow Then
			SiteCategoryType.Row_Updated(RsOld, Rs)
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

		' SiteCategoryTypeID
		' SiteCategoryTypeNM

		SiteCategoryType.SiteCategoryTypeNM.SetDbValue(SiteCategoryType.SiteCategoryTypeNM.CurrentValue, System.DBNull.Value)
		Rs("SiteCategoryTypeNM") = SiteCategoryType.SiteCategoryTypeNM.DbValue

		' SiteCategoryFileName
		SiteCategoryType.SiteCategoryFileName.SetDbValue(SiteCategoryType.SiteCategoryFileName.CurrentValue, System.DBNull.Value)
		Rs("SiteCategoryFileName") = SiteCategoryType.SiteCategoryFileName.DbValue

		' SiteCategoryTransferURL
		SiteCategoryType.SiteCategoryTransferURL.SetDbValue(SiteCategoryType.SiteCategoryTransferURL.CurrentValue, System.DBNull.Value)
		Rs("SiteCategoryTransferURL") = SiteCategoryType.SiteCategoryTransferURL.DbValue

		' DefaultSiteCategoryID
		SiteCategoryType.DefaultSiteCategoryID.SetDbValue(SiteCategoryType.DefaultSiteCategoryID.CurrentValue, System.DBNull.Value)
		Rs("DefaultSiteCategoryID") = SiteCategoryType.DefaultSiteCategoryID.DbValue

		' Row Inserting event
		bInsertRow = SiteCategoryType.Row_Inserting(Rs)
		If bInsertRow Then
			Try	
				SiteCategoryType.Insert(Rs)
				AddRow = True
			Catch e As Exception
				If EW_DEBUG_ENABLED Then Throw
				Message = e.Message				
				AddRow = False
			End Try
		Else
			If SiteCategoryType.CancelMessage <> "" Then
				Message = SiteCategoryType.CancelMessage
				SiteCategoryType.CancelMessage = ""
			Else
				Message = "Insert cancelled"
			End If
			AddRow = False
		End If
		If AddRow Then
			LastInsertId = Conn.GetLastInsertId()
			SiteCategoryType.SiteCategoryTypeID.DbValue = LastInsertId
			Rs("SiteCategoryTypeID") = SiteCategoryType.SiteCategoryTypeID.DbValue		

			' Row Inserted event
			SiteCategoryType.Row_Inserted(Rs)
			WriteAuditTrailOnAdd(Rs)
		End If
	End Function

	'
	' Load advanced search
	'
	Sub LoadAdvancedSearch()
		SiteCategoryType.SiteCategoryTypeNM.AdvancedSearch.SearchValue = SiteCategoryType.GetAdvancedSearch("x_SiteCategoryTypeNM")
		SiteCategoryType.SiteCategoryTypeDS.AdvancedSearch.SearchValue = SiteCategoryType.GetAdvancedSearch("x_SiteCategoryTypeDS")
		SiteCategoryType.SiteCategoryComment.AdvancedSearch.SearchValue = SiteCategoryType.GetAdvancedSearch("x_SiteCategoryComment")
		SiteCategoryType.SiteCategoryFileName.AdvancedSearch.SearchValue = SiteCategoryType.GetAdvancedSearch("x_SiteCategoryFileName")
		SiteCategoryType.SiteCategoryTransferURL.AdvancedSearch.SearchValue = SiteCategoryType.GetAdvancedSearch("x_SiteCategoryTransferURL")
		SiteCategoryType.DefaultSiteCategoryID.AdvancedSearch.SearchValue = SiteCategoryType.GetAdvancedSearch("x_DefaultSiteCategoryID")
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
		If SiteCategoryType.ExportAll Then
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
		If SiteCategoryType.Export = "xml" Then
			oXmlDoc = New XmlDocument()
			oXmlTbl = oXmlDoc.CreateElement("table")
			oXmlDoc.AppendChild(oXmlTbl)
		Else
			ew_Write(ew_ExportHeader(SiteCategoryType.Export))

			' Horizontal format, write header
			If sExportStyle <> "v" OrElse SiteCategoryType.Export = "csv" Then
				sExportStr = ""
				ew_ExportAddValue(sExportStr, "SiteCategoryTypeID", SiteCategoryType.Export)
				ew_ExportAddValue(sExportStr, "SiteCategoryTypeNM", SiteCategoryType.Export)
				ew_ExportAddValue(sExportStr, "SiteCategoryTypeDS", SiteCategoryType.Export)
				ew_ExportAddValue(sExportStr, "SiteCategoryComment", SiteCategoryType.Export)
				ew_ExportAddValue(sExportStr, "SiteCategoryFileName", SiteCategoryType.Export)
				ew_ExportAddValue(sExportStr, "SiteCategoryTransferURL", SiteCategoryType.Export)
				ew_ExportAddValue(sExportStr, "DefaultSiteCategoryID", SiteCategoryType.Export)
				ew_Write(ew_ExportLine(sExportStr, SiteCategoryType.Export))
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
				SiteCategoryType.RowType = EW_ROWTYPE_VIEW ' Render view
				RenderRow()
				If SiteCategoryType.Export = "xml" Then
					oXmlRec = oXmlDoc.CreateElement("record")
					oXmlTbl.AppendChild(oXmlRec)
					oXmlFld = oXmlDoc.CreateElement("SiteCategoryTypeID") ' SiteCategoryTypeID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(SiteCategoryType.SiteCategoryTypeID.ExportValue(SiteCategoryType.Export, SiteCategoryType.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("SiteCategoryTypeNM") ' SiteCategoryTypeNM
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(SiteCategoryType.SiteCategoryTypeNM.ExportValue(SiteCategoryType.Export, SiteCategoryType.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("SiteCategoryTypeDS") ' SiteCategoryTypeDS
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(SiteCategoryType.SiteCategoryTypeDS.ExportValue(SiteCategoryType.Export, SiteCategoryType.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("SiteCategoryComment") ' SiteCategoryComment
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(SiteCategoryType.SiteCategoryComment.ExportValue(SiteCategoryType.Export, SiteCategoryType.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("SiteCategoryFileName") ' SiteCategoryFileName
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(SiteCategoryType.SiteCategoryFileName.ExportValue(SiteCategoryType.Export, SiteCategoryType.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("SiteCategoryTransferURL") ' SiteCategoryTransferURL
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(SiteCategoryType.SiteCategoryTransferURL.ExportValue(SiteCategoryType.Export, SiteCategoryType.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("DefaultSiteCategoryID") ' DefaultSiteCategoryID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(SiteCategoryType.DefaultSiteCategoryID.ExportValue(SiteCategoryType.Export, SiteCategoryType.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
				Else

					' Vertical format
					If sExportStyle = "v" AndAlso SiteCategoryType.Export <> "csv" Then
						ew_Write(ew_ExportField("SiteCategoryTypeID", SiteCategoryType.SiteCategoryTypeID.ExportValue(SiteCategoryType.Export, SiteCategoryType.ExportOriginalValue), SiteCategoryType.Export)) ' SiteCategoryTypeID
						ew_Write(ew_ExportField("SiteCategoryTypeNM", SiteCategoryType.SiteCategoryTypeNM.ExportValue(SiteCategoryType.Export, SiteCategoryType.ExportOriginalValue), SiteCategoryType.Export)) ' SiteCategoryTypeNM
						ew_Write(ew_ExportField("SiteCategoryTypeDS", SiteCategoryType.SiteCategoryTypeDS.ExportValue(SiteCategoryType.Export, SiteCategoryType.ExportOriginalValue), SiteCategoryType.Export)) ' SiteCategoryTypeDS
						ew_Write(ew_ExportField("SiteCategoryComment", SiteCategoryType.SiteCategoryComment.ExportValue(SiteCategoryType.Export, SiteCategoryType.ExportOriginalValue), SiteCategoryType.Export)) ' SiteCategoryComment
						ew_Write(ew_ExportField("SiteCategoryFileName", SiteCategoryType.SiteCategoryFileName.ExportValue(SiteCategoryType.Export, SiteCategoryType.ExportOriginalValue), SiteCategoryType.Export)) ' SiteCategoryFileName
						ew_Write(ew_ExportField("SiteCategoryTransferURL", SiteCategoryType.SiteCategoryTransferURL.ExportValue(SiteCategoryType.Export, SiteCategoryType.ExportOriginalValue), SiteCategoryType.Export)) ' SiteCategoryTransferURL
						ew_Write(ew_ExportField("DefaultSiteCategoryID", SiteCategoryType.DefaultSiteCategoryID.ExportValue(SiteCategoryType.Export, SiteCategoryType.ExportOriginalValue), SiteCategoryType.Export)) ' DefaultSiteCategoryID

					' Horizontal format
					Else
						sExportStr = ""
						ew_ExportAddValue(sExportStr, SiteCategoryType.SiteCategoryTypeID.ExportValue(SiteCategoryType.Export, SiteCategoryType.ExportOriginalValue), SiteCategoryType.Export) ' SiteCategoryTypeID
						ew_ExportAddValue(sExportStr, SiteCategoryType.SiteCategoryTypeNM.ExportValue(SiteCategoryType.Export, SiteCategoryType.ExportOriginalValue), SiteCategoryType.Export) ' SiteCategoryTypeNM
						ew_ExportAddValue(sExportStr, SiteCategoryType.SiteCategoryTypeDS.ExportValue(SiteCategoryType.Export, SiteCategoryType.ExportOriginalValue), SiteCategoryType.Export) ' SiteCategoryTypeDS
						ew_ExportAddValue(sExportStr, SiteCategoryType.SiteCategoryComment.ExportValue(SiteCategoryType.Export, SiteCategoryType.ExportOriginalValue), SiteCategoryType.Export) ' SiteCategoryComment
						ew_ExportAddValue(sExportStr, SiteCategoryType.SiteCategoryFileName.ExportValue(SiteCategoryType.Export, SiteCategoryType.ExportOriginalValue), SiteCategoryType.Export) ' SiteCategoryFileName
						ew_ExportAddValue(sExportStr, SiteCategoryType.SiteCategoryTransferURL.ExportValue(SiteCategoryType.Export, SiteCategoryType.ExportOriginalValue), SiteCategoryType.Export) ' SiteCategoryTransferURL
						ew_ExportAddValue(sExportStr, SiteCategoryType.DefaultSiteCategoryID.ExportValue(SiteCategoryType.Export, SiteCategoryType.ExportOriginalValue), SiteCategoryType.Export) ' DefaultSiteCategoryID
						ew_Write(ew_ExportLine(sExportStr, SiteCategoryType.Export))
					End If
				End If
			End If
		Loop

		' Close recordset
		Rs.Close()
		Rs.Dispose()
		If SiteCategoryType.Export = "xml" Then
			ew_Write("<?xml version=""1.0"" standalone=""yes""?>" & vbcrlf)
			ew_Write(oXmlDoc.OuterXml)
		Else
			ew_Write(ew_ExportFooter(SiteCategoryType.Export))
		End If
	End Sub

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "SiteCategoryType"
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
		Dim table As String = "SiteCategoryType"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsSrc("SiteCategoryTypeID")

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

		' SiteCategoryTypeID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteCategoryTypeID", keyvalue, oldvalue, RsSrc("SiteCategoryTypeID"))

		' SiteCategoryTypeNM Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteCategoryTypeNM", keyvalue, oldvalue, RsSrc("SiteCategoryTypeNM"))

		' SiteCategoryFileName Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteCategoryFileName", keyvalue, oldvalue, RsSrc("SiteCategoryFileName"))

		' SiteCategoryTransferURL Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteCategoryTransferURL", keyvalue, oldvalue, RsSrc("SiteCategoryTransferURL"))

		' DefaultSiteCategoryID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "DefaultSiteCategoryID", keyvalue, oldvalue, RsSrc("DefaultSiteCategoryID"))
	End Sub

	' Write Audit Trail (edit page)
	Sub WriteAuditTrailOnEdit(ByRef RsOld As OrderedDictionary, ByRef RsNew As OrderedDictionary)
		Dim table As String = "SiteCategoryType"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsOld("SiteCategoryTypeID")

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
			fld = SiteCategoryType.FieldByName(fldname)
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
		SiteCategoryType_list = New cSiteCategoryType_list(Me)		
		SiteCategoryType_list.Page_Init()

		' Page main processing
		SiteCategoryType_list.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If SiteCategoryType_list IsNot Nothing Then SiteCategoryType_list.Dispose()
	End Sub
End Class
