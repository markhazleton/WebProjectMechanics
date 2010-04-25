Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class SiteCategory_list
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public SiteCategory_list As cSiteCategory_list

	'
	' Page Class
	'
	Class cSiteCategory_list
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
				If SiteCategory.UseTokenInUrl Then Url = Url & "t=" & SiteCategory.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If SiteCategory.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (SiteCategory.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (SiteCategory.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' SiteCategory
		Public Property SiteCategory() As cSiteCategory
			Get				
				Return ParentPage.SiteCategory
			End Get
			Set(ByVal v As cSiteCategory)
				ParentPage.SiteCategory = v	
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
			m_PageObjName = "SiteCategory_list"
			m_PageObjTypeName = "cSiteCategory_list"

			' Table Name
			m_TableName = "SiteCategory"

			' Initialize table object
			SiteCategory = New cSiteCategory(Me)

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
			SiteCategory.Export = ew_Get("export") ' Get export parameter
			ParentPage.gsExport = SiteCategory.Export ' Get export parameter, used in header
			ParentPage.gsExportFile = SiteCategory.TableVar ' Get export file, used in header
			If SiteCategory.Export = "excel" Then
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
			SiteCategory.Dispose()
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
					SiteCategory.CurrentAction = ew_Get("a")

					' Clear inline mode
					If SiteCategory.CurrentAction = "cancel" Then
						ClearInlineMode()
					End If

					' Switch to grid edit mode
					If SiteCategory.CurrentAction = "gridedit" Then
						GridEditMode()
					End If

					' Switch to grid add mode
					If SiteCategory.CurrentAction = "gridadd" Then
						GridAddMode()
					End If
				End If
			Else
				SiteCategory.CurrentAction = ObjForm.GetValue("a_list") ' Get action

				' Grid Update
				If SiteCategory.CurrentAction = "gridupdate" AndAlso ew_SameStr(ew_Session(EW_SESSION_INLINE_MODE), "gridedit") Then
					GridUpdate()
				End If

				' Grid Insert
				If SiteCategory.CurrentAction = "gridinsert" AndAlso ew_SameStr(ew_Session(EW_SESSION_INLINE_MODE), "gridadd") Then
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
		If (SiteCategory.RecordsPerPage = -1 OrElse SiteCategory.RecordsPerPage > 0) Then
			lDisplayRecs = SiteCategory.RecordsPerPage ' Restore from Session
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
		SiteCategory.Recordset_Searching(sSrchWhere)

		' Save search criteria
		If sSrchWhere <> "" Then
			If sSrchBasic = "" Then ResetBasicSearchParms()
			If sSrchAdvanced = "" Then ResetAdvancedSearchParms()
			SiteCategory.SearchWhere = sSrchWhere ' Save to Session
			lStartRec = 1 ' Reset start record counter
			SiteCategory.StartRecordNumber = lStartRec
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
		SiteCategory.SessionWhere = sFilter
		SiteCategory.CurrentFilter = ""

		' Export Data only
		If SiteCategory.Export = "html" OrElse SiteCategory.Export = "csv" OrElse SiteCategory.Export = "word" OrElse SiteCategory.Export = "excel" OrElse SiteCategory.Export = "xml" Then
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
			SiteCategory.RecordsPerPage = lDisplayRecs ' Save to Session

			' Reset start position
			lStartRec = 1
			SiteCategory.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	'  Exit out of inline mode
	'
	Sub ClearInlineMode()
		SiteCategory.CurrentAction = "" ' Clear action
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
		SiteCategory.CurrentFilter = BuildKeyFilter()
		sSql = SiteCategory.SQL
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
					SiteCategory.SendEmail = False ' Do not send email on update success
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
			SiteCategory.EventCancelled = True ' Set event cancelled
			SiteCategory.CurrentAction = "gridedit" ' Stay in gridedit mode
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
				sFilter = SiteCategory.KeyFilter
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
			SiteCategory.SiteCategoryID.FormValue = arKeyFlds(0)
			If Not IsNumeric(SiteCategory.SiteCategoryID.FormValue) Then	Return False
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
				SiteCategory.SendEmail = False ' Do not send email on insert success

				' Validate Form
				If Not ValidateForm() Then
					bGridInsert = False ' Form error, reset action
					Message = ParentPage.gsFormError
				Else
					bGridInsert = AddRow() ' Insert this row
				End If
				If bGridInsert Then
					If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
					sKey = sKey & SiteCategory.SiteCategoryID.CurrentValue

					' Add filter for this record
					sFilter = SiteCategory.KeyFilter
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
			SiteCategory.CurrentFilter = sWrkFilter
			sSql = SiteCategory.SQL
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
			SiteCategory.EventCancelled = True ' Set event cancelled
			SiteCategory.CurrentAction = "gridadd" ' Stay in gridadd mode
		End If
	End Sub

	Function EmptyRow() As Boolean
		Dim Empty As Boolean = True
		If Empty Then Empty = ew_SameStr(SiteCategory.SiteCategoryTypeID.CurrentValue, SiteCategory.SiteCategoryTypeID.OldValue)
		If Empty Then Empty = ew_SameStr(SiteCategory.GroupOrder.CurrentValue, SiteCategory.GroupOrder.OldValue)
		If Empty Then Empty = ew_SameStr(SiteCategory.CategoryName.CurrentValue, SiteCategory.CategoryName.OldValue)
		If Empty Then Empty = ew_SameStr(SiteCategory.ParentCategoryID.CurrentValue, SiteCategory.ParentCategoryID.OldValue)
		If Empty Then Empty = ew_SameStr(SiteCategory.CategoryFileName.CurrentValue, SiteCategory.CategoryFileName.OldValue)
		If Empty Then Empty = ew_SameStr(SiteCategory.SiteCategoryGroupID.CurrentValue, SiteCategory.SiteCategoryGroupID.OldValue)
		Return Empty
	End Function

	'
	' Restore form values for current row
	'
	Sub RestoreCurrentRowFormValues(idx As Integer)
		Dim sKey As String, arKeyFlds() As String

		' Get row based on current index
		ObjForm.Index = idx
		If SiteCategory.CurrentAction = "gridadd" Then
			LoadFormValues() ' Load form values
		End If
		If SiteCategory.CurrentAction = "gridedit" Then
			sKey = ObjForm.GetValue("k_key")
			arKeyFlds = sKey.Split(New Char() {Convert.ToChar(EW_COMPOSITE_KEY_SEPARATOR)})
			If arKeyFlds.Length >= 1 Then
				If ew_SameStr(arKeyFlds(0), SiteCategory.SiteCategoryID.CurrentValue) Then
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
		BuildSearchSql(sWhere, SiteCategory.SiteCategoryTypeID, False) ' SiteCategoryTypeID
		BuildSearchSql(sWhere, SiteCategory.GroupOrder, False) ' GroupOrder
		BuildSearchSql(sWhere, SiteCategory.CategoryName, False) ' CategoryName
		BuildSearchSql(sWhere, SiteCategory.CategoryTitle, False) ' CategoryTitle
		BuildSearchSql(sWhere, SiteCategory.CategoryDescription, False) ' CategoryDescription
		BuildSearchSql(sWhere, SiteCategory.CategoryKeywords, False) ' CategoryKeywords
		BuildSearchSql(sWhere, SiteCategory.ParentCategoryID, False) ' ParentCategoryID
		BuildSearchSql(sWhere, SiteCategory.CategoryFileName, False) ' CategoryFileName
		BuildSearchSql(sWhere, SiteCategory.SiteCategoryGroupID, False) ' SiteCategoryGroupID

		' Set up search parm
		If sWhere <> "" Then
			SetSearchParm(SiteCategory.SiteCategoryTypeID) ' SiteCategoryTypeID
			SetSearchParm(SiteCategory.GroupOrder) ' GroupOrder
			SetSearchParm(SiteCategory.CategoryName) ' CategoryName
			SetSearchParm(SiteCategory.CategoryTitle) ' CategoryTitle
			SetSearchParm(SiteCategory.CategoryDescription) ' CategoryDescription
			SetSearchParm(SiteCategory.CategoryKeywords) ' CategoryKeywords
			SetSearchParm(SiteCategory.ParentCategoryID) ' ParentCategoryID
			SetSearchParm(SiteCategory.CategoryFileName) ' CategoryFileName
			SetSearchParm(SiteCategory.SiteCategoryGroupID) ' SiteCategoryGroupID
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
		SiteCategory.SetAdvancedSearch("x_" & FldParm, Fld.AdvancedSearch.SearchValue)
		SiteCategory.SetAdvancedSearch("z_" & FldParm, Fld.AdvancedSearch.SearchOperator)
		SiteCategory.SetAdvancedSearch("v_" & FldParm, Fld.AdvancedSearch.SearchCondition)
		SiteCategory.SetAdvancedSearch("y_" & FldParm, Fld.AdvancedSearch.SearchValue2)
		SiteCategory.SetAdvancedSearch("w_" & FldParm, Fld.AdvancedSearch.SearchOperator2)
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
		sSql = sSql & "[CategoryName] LIKE '%" & sKeyword & "%' OR "
		sSql = sSql & "[CategoryTitle] LIKE '%" & sKeyword & "%' OR "
		sSql = sSql & "[CategoryDescription] LIKE '%" & sKeyword & "%' OR "
		sSql = sSql & "[CategoryKeywords] LIKE '%" & sKeyword & "%' OR "
		sSql = sSql & "[ParentCategoryID] LIKE '%" & sKeyword & "%' OR "
		sSql = sSql & "[CategoryFileName] LIKE '%" & sKeyword & "%' OR "
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
			SiteCategory.BasicSearchKeyword = sSearchKeyword
			SiteCategory.BasicSearchType = sSearchType
		End If
		Return sSearchStr
	End Function

	'
	' Clear all search parameters
	'
	Sub ResetSearchParms()

		' Clear search where
		sSrchWhere = ""
		SiteCategory.SearchWhere = sSrchWhere

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
		SiteCategory.BasicSearchKeyword = ""
		SiteCategory.BasicSearchType = ""
	End Sub

	'
	' Clear all advanced search parameters
	'
	Sub ResetAdvancedSearchParms()
		SiteCategory.SetAdvancedSearch("x_SiteCategoryTypeID", "")
		SiteCategory.SetAdvancedSearch("x_GroupOrder", "")
		SiteCategory.SetAdvancedSearch("x_CategoryName", "")
		SiteCategory.SetAdvancedSearch("x_CategoryTitle", "")
		SiteCategory.SetAdvancedSearch("x_CategoryDescription", "")
		SiteCategory.SetAdvancedSearch("x_CategoryKeywords", "")
		SiteCategory.SetAdvancedSearch("x_ParentCategoryID", "")
		SiteCategory.SetAdvancedSearch("x_CategoryFileName", "")
		SiteCategory.SetAdvancedSearch("x_SiteCategoryGroupID", "")
	End Sub

	'
	' Restore all search parameters
	'
	Sub RestoreSearchParms()
		sSrchWhere = SiteCategory.SearchWhere

		' Restore advanced search settings
		If ParentPage.gsSearchError = "" Then
			RestoreAdvancedSearchParms()
		End If
	End Sub

	'
	' Restore all advanced search parameters
	'
	Sub RestoreAdvancedSearchParms()
		SiteCategory.SiteCategoryTypeID.AdvancedSearch.SearchValue = SiteCategory.GetAdvancedSearch("x_SiteCategoryTypeID")
		SiteCategory.GroupOrder.AdvancedSearch.SearchValue = SiteCategory.GetAdvancedSearch("x_GroupOrder")
		SiteCategory.CategoryName.AdvancedSearch.SearchValue = SiteCategory.GetAdvancedSearch("x_CategoryName")
		SiteCategory.CategoryTitle.AdvancedSearch.SearchValue = SiteCategory.GetAdvancedSearch("x_CategoryTitle")
		SiteCategory.CategoryDescription.AdvancedSearch.SearchValue = SiteCategory.GetAdvancedSearch("x_CategoryDescription")
		SiteCategory.CategoryKeywords.AdvancedSearch.SearchValue = SiteCategory.GetAdvancedSearch("x_CategoryKeywords")
		SiteCategory.ParentCategoryID.AdvancedSearch.SearchValue = SiteCategory.GetAdvancedSearch("x_ParentCategoryID")
		SiteCategory.CategoryFileName.AdvancedSearch.SearchValue = SiteCategory.GetAdvancedSearch("x_CategoryFileName")
		SiteCategory.SiteCategoryGroupID.AdvancedSearch.SearchValue = SiteCategory.GetAdvancedSearch("x_SiteCategoryGroupID")
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
			SiteCategory.CurrentOrder = ew_Get("order")
			SiteCategory.CurrentOrderType = ew_Get("ordertype")
			SiteCategory.UpdateSort(SiteCategory.SiteCategoryTypeID) ' SiteCategoryTypeID
			SiteCategory.UpdateSort(SiteCategory.GroupOrder) ' GroupOrder
			SiteCategory.UpdateSort(SiteCategory.CategoryName) ' CategoryName
			SiteCategory.UpdateSort(SiteCategory.ParentCategoryID) ' ParentCategoryID
			SiteCategory.UpdateSort(SiteCategory.CategoryFileName) ' CategoryFileName
			SiteCategory.UpdateSort(SiteCategory.SiteCategoryGroupID) ' SiteCategoryGroupID
			SiteCategory.StartRecordNumber = 1 ' Reset start position
		End If
	End Sub

	'
	' Load Sort Order parameters
	'
	Sub LoadSortOrder()
		Dim sOrderBy As String
		sOrderBy = SiteCategory.SessionOrderBy ' Get order by from Session
		If sOrderBy = "" Then
			If SiteCategory.SqlOrderBy <> "" Then
				sOrderBy = SiteCategory.SqlOrderBy
				SiteCategory.SessionOrderBy = sOrderBy
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
				SiteCategory.SessionOrderBy = sOrderBy
				SiteCategory.SiteCategoryTypeID.Sort = ""
				SiteCategory.GroupOrder.Sort = ""
				SiteCategory.CategoryName.Sort = ""
				SiteCategory.ParentCategoryID.Sort = ""
				SiteCategory.CategoryFileName.Sort = ""
				SiteCategory.SiteCategoryGroupID.Sort = ""
			End If

			' Reset start position
			lStartRec = 1
			SiteCategory.StartRecordNumber = lStartRec
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
				SiteCategory.StartRecordNumber = lStartRec
			ElseIf ew_Get(EW_TABLE_PAGE_NO) <> "" AndAlso IsNumeric(ew_Get(EW_TABLE_PAGE_NO)) Then
				nPageNo = ew_ConvertToInt(ew_Get(EW_TABLE_PAGE_NO))
				lStartRec = (nPageNo-1)*lDisplayRecs+1
				If lStartRec <= 0 Then
					lStartRec = 1
				ElseIf lStartRec >= ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 Then
					lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1
				End If
				SiteCategory.StartRecordNumber = lStartRec
			End If
		End If
		lStartRec = SiteCategory.StartRecordNumber

		' Check if correct start record counter
		If lStartRec <= 0 Then ' Avoid invalid start record counter
			lStartRec = 1 ' Reset start record counter
			SiteCategory.StartRecordNumber = lStartRec
		ElseIf lStartRec > lTotalRecs Then ' Avoid starting record > total records
			lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to last page first record
			SiteCategory.StartRecordNumber = lStartRec
		ElseIf (lStartRec-1) Mod lDisplayRecs <> 0 Then
			lStartRec = ((lStartRec-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to page boundary
			SiteCategory.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	' Load default values
	'
	Sub LoadDefaultValues()
		SiteCategory.SiteCategoryTypeID.OldValue = SiteCategory.SiteCategoryTypeID.CurrentValue
		SiteCategory.GroupOrder.OldValue = SiteCategory.GroupOrder.CurrentValue
		SiteCategory.CategoryName.OldValue = SiteCategory.CategoryName.CurrentValue
		SiteCategory.ParentCategoryID.OldValue = SiteCategory.ParentCategoryID.CurrentValue
		SiteCategory.CategoryFileName.OldValue = SiteCategory.CategoryFileName.CurrentValue
		SiteCategory.SiteCategoryGroupID.OldValue = SiteCategory.SiteCategoryGroupID.CurrentValue
	End Sub

	'
	'  Load search values for validation
	'
	Sub LoadSearchValues()
		SiteCategory.SiteCategoryTypeID.AdvancedSearch.SearchValue = ew_Get("x_SiteCategoryTypeID")
    	SiteCategory.SiteCategoryTypeID.AdvancedSearch.SearchOperator = ew_Get("z_SiteCategoryTypeID")
		SiteCategory.GroupOrder.AdvancedSearch.SearchValue = ew_Get("x_GroupOrder")
    	SiteCategory.GroupOrder.AdvancedSearch.SearchOperator = ew_Get("z_GroupOrder")
		SiteCategory.CategoryName.AdvancedSearch.SearchValue = ew_Get("x_CategoryName")
    	SiteCategory.CategoryName.AdvancedSearch.SearchOperator = ew_Get("z_CategoryName")
		SiteCategory.CategoryTitle.AdvancedSearch.SearchValue = ew_Get("x_CategoryTitle")
    	SiteCategory.CategoryTitle.AdvancedSearch.SearchOperator = ew_Get("z_CategoryTitle")
		SiteCategory.CategoryDescription.AdvancedSearch.SearchValue = ew_Get("x_CategoryDescription")
    	SiteCategory.CategoryDescription.AdvancedSearch.SearchOperator = ew_Get("z_CategoryDescription")
		SiteCategory.CategoryKeywords.AdvancedSearch.SearchValue = ew_Get("x_CategoryKeywords")
    	SiteCategory.CategoryKeywords.AdvancedSearch.SearchOperator = ew_Get("z_CategoryKeywords")
		SiteCategory.ParentCategoryID.AdvancedSearch.SearchValue = ew_Get("x_ParentCategoryID")
    	SiteCategory.ParentCategoryID.AdvancedSearch.SearchOperator = ew_Get("z_ParentCategoryID")
		SiteCategory.CategoryFileName.AdvancedSearch.SearchValue = ew_Get("x_CategoryFileName")
    	SiteCategory.CategoryFileName.AdvancedSearch.SearchOperator = ew_Get("z_CategoryFileName")
		SiteCategory.SiteCategoryGroupID.AdvancedSearch.SearchValue = ew_Get("x_SiteCategoryGroupID")
    	SiteCategory.SiteCategoryGroupID.AdvancedSearch.SearchOperator = ew_Get("z_SiteCategoryGroupID")
	End Sub

	'
	' Load form values
	'
	Sub LoadFormValues()
		SiteCategory.SiteCategoryTypeID.FormValue = ObjForm.GetValue("x_SiteCategoryTypeID")
		SiteCategory.SiteCategoryTypeID.OldValue = ObjForm.GetValue("o_SiteCategoryTypeID")
		SiteCategory.GroupOrder.FormValue = ObjForm.GetValue("x_GroupOrder")
		SiteCategory.GroupOrder.OldValue = ObjForm.GetValue("o_GroupOrder")
		SiteCategory.CategoryName.FormValue = ObjForm.GetValue("x_CategoryName")
		SiteCategory.CategoryName.OldValue = ObjForm.GetValue("o_CategoryName")
		SiteCategory.ParentCategoryID.FormValue = ObjForm.GetValue("x_ParentCategoryID")
		SiteCategory.ParentCategoryID.OldValue = ObjForm.GetValue("o_ParentCategoryID")
		SiteCategory.CategoryFileName.FormValue = ObjForm.GetValue("x_CategoryFileName")
		SiteCategory.CategoryFileName.OldValue = ObjForm.GetValue("o_CategoryFileName")
		SiteCategory.SiteCategoryGroupID.FormValue = ObjForm.GetValue("x_SiteCategoryGroupID")
		SiteCategory.SiteCategoryGroupID.OldValue = ObjForm.GetValue("o_SiteCategoryGroupID")
		SiteCategory.SiteCategoryID.FormValue = ObjForm.GetValue("x_SiteCategoryID")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		SiteCategory.SiteCategoryTypeID.CurrentValue = SiteCategory.SiteCategoryTypeID.FormValue
		SiteCategory.GroupOrder.CurrentValue = SiteCategory.GroupOrder.FormValue
		SiteCategory.CategoryName.CurrentValue = SiteCategory.CategoryName.FormValue
		SiteCategory.ParentCategoryID.CurrentValue = SiteCategory.ParentCategoryID.FormValue
		SiteCategory.CategoryFileName.CurrentValue = SiteCategory.CategoryFileName.FormValue
		SiteCategory.SiteCategoryGroupID.CurrentValue = SiteCategory.SiteCategoryGroupID.FormValue
		SiteCategory.SiteCategoryID.CurrentValue = SiteCategory.SiteCategoryID.FormValue
	End Sub

	'
	' Load recordset
	'
	Function LoadRecordset() As OleDbDataReader

		' Recordset Selecting event
		SiteCategory.Recordset_Selecting(SiteCategory.CurrentFilter)

		' Load list page SQL
		Dim sSql As String = SiteCategory.ListSQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)

		' Count
		lTotalRecs = -1		
		Try			
			If sSql.StartsWith("SELECT * FROM ", StringComparison.InvariantCultureIgnoreCase) AndAlso _				
				ew_Empty(SiteCategory.SqlGroupBy) AndAlso _
				ew_Empty(SiteCategory.SqlHaving) Then
				Dim sCntSql As String = SiteCategory.SelectCountSQL

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
		SiteCategory.Recordset_Selected(Rs)
		Return Rs
	End Function

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = SiteCategory.KeyFilter

		' Row Selecting event
		SiteCategory.Row_Selecting(sFilter)

		' Load SQL based on filter
		SiteCategory.CurrentFilter = sFilter
		Dim sSql As String = SiteCategory.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				SiteCategory.Row_Selected(RsRow)
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
		SiteCategory.SiteCategoryID.DbValue = RsRow("SiteCategoryID")
		SiteCategory.SiteCategoryTypeID.DbValue = RsRow("SiteCategoryTypeID")
		SiteCategory.GroupOrder.DbValue = RsRow("GroupOrder")
		SiteCategory.CategoryName.DbValue = RsRow("CategoryName")
		SiteCategory.CategoryTitle.DbValue = RsRow("CategoryTitle")
		SiteCategory.CategoryDescription.DbValue = RsRow("CategoryDescription")
		SiteCategory.CategoryKeywords.DbValue = RsRow("CategoryKeywords")
		SiteCategory.ParentCategoryID.DbValue = RsRow("ParentCategoryID")
		SiteCategory.CategoryFileName.DbValue = RsRow("CategoryFileName")
		SiteCategory.SiteCategoryGroupID.DbValue = RsRow("SiteCategoryGroupID")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		SiteCategory.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' SiteCategoryTypeID

		SiteCategory.SiteCategoryTypeID.CellCssStyle = "white-space: nowrap;"
		SiteCategory.SiteCategoryTypeID.CellCssClass = ""

		' GroupOrder
		SiteCategory.GroupOrder.CellCssStyle = "white-space: nowrap;"
		SiteCategory.GroupOrder.CellCssClass = ""

		' CategoryName
		SiteCategory.CategoryName.CellCssStyle = "white-space: nowrap;"
		SiteCategory.CategoryName.CellCssClass = ""

		' ParentCategoryID
		SiteCategory.ParentCategoryID.CellCssStyle = "white-space: nowrap;"
		SiteCategory.ParentCategoryID.CellCssClass = ""

		' CategoryFileName
		SiteCategory.CategoryFileName.CellCssStyle = "white-space: nowrap;"
		SiteCategory.CategoryFileName.CellCssClass = ""

		' SiteCategoryGroupID
		SiteCategory.SiteCategoryGroupID.CellCssStyle = "white-space: nowrap;"
		SiteCategory.SiteCategoryGroupID.CellCssClass = ""

		'
		'  View  Row
		'

		If SiteCategory.RowType = EW_ROWTYPE_VIEW Then ' View row

			' SiteCategoryTypeID
			If ew_NotEmpty(SiteCategory.SiteCategoryTypeID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteCategoryTypeNM] FROM [SiteCategoryType] WHERE [SiteCategoryTypeID] = " & ew_AdjustSql(SiteCategory.SiteCategoryTypeID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryTypeNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					SiteCategory.SiteCategoryTypeID.ViewValue = RsWrk("SiteCategoryTypeNM")
				Else
					SiteCategory.SiteCategoryTypeID.ViewValue = SiteCategory.SiteCategoryTypeID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				SiteCategory.SiteCategoryTypeID.ViewValue = System.DBNull.Value
			End If
			SiteCategory.SiteCategoryTypeID.CssStyle = ""
			SiteCategory.SiteCategoryTypeID.CssClass = ""
			SiteCategory.SiteCategoryTypeID.ViewCustomAttributes = ""

			' GroupOrder
			SiteCategory.GroupOrder.ViewValue = SiteCategory.GroupOrder.CurrentValue
			SiteCategory.GroupOrder.CssStyle = ""
			SiteCategory.GroupOrder.CssClass = ""
			SiteCategory.GroupOrder.ViewCustomAttributes = ""

			' CategoryName
			SiteCategory.CategoryName.ViewValue = SiteCategory.CategoryName.CurrentValue
			SiteCategory.CategoryName.CssStyle = ""
			SiteCategory.CategoryName.CssClass = ""
			SiteCategory.CategoryName.ViewCustomAttributes = ""

			' CategoryTitle
			SiteCategory.CategoryTitle.ViewValue = SiteCategory.CategoryTitle.CurrentValue
			SiteCategory.CategoryTitle.CssStyle = ""
			SiteCategory.CategoryTitle.CssClass = ""
			SiteCategory.CategoryTitle.ViewCustomAttributes = ""

			' CategoryDescription
			SiteCategory.CategoryDescription.ViewValue = SiteCategory.CategoryDescription.CurrentValue
			SiteCategory.CategoryDescription.CssStyle = ""
			SiteCategory.CategoryDescription.CssClass = ""
			SiteCategory.CategoryDescription.ViewCustomAttributes = ""

			' CategoryKeywords
			SiteCategory.CategoryKeywords.ViewValue = SiteCategory.CategoryKeywords.CurrentValue
			SiteCategory.CategoryKeywords.CssStyle = ""
			SiteCategory.CategoryKeywords.CssClass = ""
			SiteCategory.CategoryKeywords.ViewCustomAttributes = ""

			' ParentCategoryID
			If ew_NotEmpty(SiteCategory.ParentCategoryID.CurrentValue) Then
				sSqlWrk = "SELECT [CategoryName] FROM [SiteCategory] WHERE [SiteCategoryID] = " & ew_AdjustSql(SiteCategory.ParentCategoryID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [CategoryName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					SiteCategory.ParentCategoryID.ViewValue = RsWrk("CategoryName")
				Else
					SiteCategory.ParentCategoryID.ViewValue = SiteCategory.ParentCategoryID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				SiteCategory.ParentCategoryID.ViewValue = System.DBNull.Value
			End If
			SiteCategory.ParentCategoryID.CssStyle = ""
			SiteCategory.ParentCategoryID.CssClass = ""
			SiteCategory.ParentCategoryID.ViewCustomAttributes = ""

			' CategoryFileName
			SiteCategory.CategoryFileName.ViewValue = SiteCategory.CategoryFileName.CurrentValue
			SiteCategory.CategoryFileName.CssStyle = ""
			SiteCategory.CategoryFileName.CssClass = ""
			SiteCategory.CategoryFileName.ViewCustomAttributes = ""

			' SiteCategoryGroupID
			If ew_NotEmpty(SiteCategory.SiteCategoryGroupID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteCategoryGroupNM] FROM [SiteCategoryGroup] WHERE [SiteCategoryGroupID] = " & ew_AdjustSql(SiteCategory.SiteCategoryGroupID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryGroupNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					SiteCategory.SiteCategoryGroupID.ViewValue = RsWrk("SiteCategoryGroupNM")
				Else
					SiteCategory.SiteCategoryGroupID.ViewValue = SiteCategory.SiteCategoryGroupID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				SiteCategory.SiteCategoryGroupID.ViewValue = System.DBNull.Value
			End If
			SiteCategory.SiteCategoryGroupID.CssStyle = ""
			SiteCategory.SiteCategoryGroupID.CssClass = ""
			SiteCategory.SiteCategoryGroupID.ViewCustomAttributes = ""

			' View refer script
			' SiteCategoryTypeID

			SiteCategory.SiteCategoryTypeID.HrefValue = ""

			' GroupOrder
			SiteCategory.GroupOrder.HrefValue = ""

			' CategoryName
			SiteCategory.CategoryName.HrefValue = ""

			' ParentCategoryID
			SiteCategory.ParentCategoryID.HrefValue = ""

			' CategoryFileName
			SiteCategory.CategoryFileName.HrefValue = ""

			' SiteCategoryGroupID
			SiteCategory.SiteCategoryGroupID.HrefValue = ""

		'
		'  Add Row
		'

		ElseIf SiteCategory.RowType = EW_ROWTYPE_ADD Then ' Add row

			' SiteCategoryTypeID
			SiteCategory.SiteCategoryTypeID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryTypeID], [SiteCategoryTypeNM], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteCategoryType]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryTypeNM] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			SiteCategory.SiteCategoryTypeID.EditValue = arwrk

			' GroupOrder
			SiteCategory.GroupOrder.EditCustomAttributes = ""
			SiteCategory.GroupOrder.EditValue = ew_HtmlEncode(SiteCategory.GroupOrder.CurrentValue)

			' CategoryName
			SiteCategory.CategoryName.EditCustomAttributes = ""
			SiteCategory.CategoryName.EditValue = ew_HtmlEncode(SiteCategory.CategoryName.CurrentValue)

			' ParentCategoryID
			SiteCategory.ParentCategoryID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryID], [CategoryName], '' AS Disp2Fld, [SiteCategoryTypeID] FROM [SiteCategory]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [CategoryName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select", ""}) 
			SiteCategory.ParentCategoryID.EditValue = arwrk

			' CategoryFileName
			SiteCategory.CategoryFileName.EditCustomAttributes = ""
			SiteCategory.CategoryFileName.EditValue = ew_HtmlEncode(SiteCategory.CategoryFileName.CurrentValue)

			' SiteCategoryGroupID
			SiteCategory.SiteCategoryGroupID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryGroupID], [SiteCategoryGroupNM], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteCategoryGroup]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryGroupNM] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			SiteCategory.SiteCategoryGroupID.EditValue = arwrk

		'
		'  Edit Row
		'

		ElseIf SiteCategory.RowType = EW_ROWTYPE_EDIT Then ' Edit row

			' SiteCategoryTypeID
			SiteCategory.SiteCategoryTypeID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryTypeID], [SiteCategoryTypeNM], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteCategoryType]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryTypeNM] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			SiteCategory.SiteCategoryTypeID.EditValue = arwrk

			' GroupOrder
			SiteCategory.GroupOrder.EditCustomAttributes = ""
			SiteCategory.GroupOrder.EditValue = ew_HtmlEncode(SiteCategory.GroupOrder.CurrentValue)

			' CategoryName
			SiteCategory.CategoryName.EditCustomAttributes = ""
			SiteCategory.CategoryName.EditValue = ew_HtmlEncode(SiteCategory.CategoryName.CurrentValue)

			' ParentCategoryID
			SiteCategory.ParentCategoryID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryID], [CategoryName], '' AS Disp2Fld, [SiteCategoryTypeID] FROM [SiteCategory]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [CategoryName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select", ""}) 
			SiteCategory.ParentCategoryID.EditValue = arwrk

			' CategoryFileName
			SiteCategory.CategoryFileName.EditCustomAttributes = ""
			SiteCategory.CategoryFileName.EditValue = ew_HtmlEncode(SiteCategory.CategoryFileName.CurrentValue)

			' SiteCategoryGroupID
			SiteCategory.SiteCategoryGroupID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryGroupID], [SiteCategoryGroupNM], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteCategoryGroup]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryGroupNM] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			SiteCategory.SiteCategoryGroupID.EditValue = arwrk

			' Edit refer script
			' SiteCategoryTypeID

			SiteCategory.SiteCategoryTypeID.HrefValue = ""

			' GroupOrder
			SiteCategory.GroupOrder.HrefValue = ""

			' CategoryName
			SiteCategory.CategoryName.HrefValue = ""

			' ParentCategoryID
			SiteCategory.ParentCategoryID.HrefValue = ""

			' CategoryFileName
			SiteCategory.CategoryFileName.HrefValue = ""

			' SiteCategoryGroupID
			SiteCategory.SiteCategoryGroupID.HrefValue = ""
		End If

		' Row Rendered event
		SiteCategory.Row_Rendered()
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
		If ew_Empty(SiteCategory.SiteCategoryTypeID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter required field - Site Type"
		End If
		If Not ew_CheckNumber(SiteCategory.GroupOrder.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Incorrect floating point number - Order"
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
		sFilter = SiteCategory.KeyFilter
		SiteCategory.CurrentFilter  = sFilter
		sSql = SiteCategory.SQL
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
			SiteCategory.SiteCategoryTypeID.SetDbValue(SiteCategory.SiteCategoryTypeID.CurrentValue, System.DBNull.Value)
			Rs("SiteCategoryTypeID") = SiteCategory.SiteCategoryTypeID.DbValue

			' GroupOrder
			SiteCategory.GroupOrder.SetDbValue(SiteCategory.GroupOrder.CurrentValue, System.DBNull.Value)
			Rs("GroupOrder") = SiteCategory.GroupOrder.DbValue

			' CategoryName
			SiteCategory.CategoryName.SetDbValue(SiteCategory.CategoryName.CurrentValue, System.DBNull.Value)
			Rs("CategoryName") = SiteCategory.CategoryName.DbValue

			' ParentCategoryID
			SiteCategory.ParentCategoryID.SetDbValue(SiteCategory.ParentCategoryID.CurrentValue, System.DBNull.Value)
			Rs("ParentCategoryID") = SiteCategory.ParentCategoryID.DbValue

			' CategoryFileName
			SiteCategory.CategoryFileName.SetDbValue(SiteCategory.CategoryFileName.CurrentValue, System.DBNull.Value)
			Rs("CategoryFileName") = SiteCategory.CategoryFileName.DbValue

			' SiteCategoryGroupID
			SiteCategory.SiteCategoryGroupID.SetDbValue(SiteCategory.SiteCategoryGroupID.CurrentValue, System.DBNull.Value)
			Rs("SiteCategoryGroupID") = SiteCategory.SiteCategoryGroupID.DbValue

			' Row Updating event
			bUpdateRow = SiteCategory.Row_Updating(RsOld, Rs)
			If bUpdateRow Then
				Try
					SiteCategory.Update(Rs)
					EditRow = True
				Catch e As Exception
					If EW_DEBUG_ENABLED Then Throw
					Message = e.Message
					EditRow = False
				End Try
			Else
				If SiteCategory.CancelMessage <> "" Then
					Message = SiteCategory.CancelMessage
					SiteCategory.CancelMessage = ""
				Else
					Message = "Update cancelled"
				End If
				EditRow = False
			End If
		End If

		' Row Updated event
		If EditRow Then
			SiteCategory.Row_Updated(RsOld, Rs)
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
		SiteCategory.SiteCategoryTypeID.SetDbValue(SiteCategory.SiteCategoryTypeID.CurrentValue, System.DBNull.Value)
		Rs("SiteCategoryTypeID") = SiteCategory.SiteCategoryTypeID.DbValue

		' GroupOrder
		SiteCategory.GroupOrder.SetDbValue(SiteCategory.GroupOrder.CurrentValue, System.DBNull.Value)
		Rs("GroupOrder") = SiteCategory.GroupOrder.DbValue

		' CategoryName
		SiteCategory.CategoryName.SetDbValue(SiteCategory.CategoryName.CurrentValue, System.DBNull.Value)
		Rs("CategoryName") = SiteCategory.CategoryName.DbValue

		' ParentCategoryID
		SiteCategory.ParentCategoryID.SetDbValue(SiteCategory.ParentCategoryID.CurrentValue, System.DBNull.Value)
		Rs("ParentCategoryID") = SiteCategory.ParentCategoryID.DbValue

		' CategoryFileName
		SiteCategory.CategoryFileName.SetDbValue(SiteCategory.CategoryFileName.CurrentValue, System.DBNull.Value)
		Rs("CategoryFileName") = SiteCategory.CategoryFileName.DbValue

		' SiteCategoryGroupID
		SiteCategory.SiteCategoryGroupID.SetDbValue(SiteCategory.SiteCategoryGroupID.CurrentValue, System.DBNull.Value)
		Rs("SiteCategoryGroupID") = SiteCategory.SiteCategoryGroupID.DbValue

		' Row Inserting event
		bInsertRow = SiteCategory.Row_Inserting(Rs)
		If bInsertRow Then
			Try	
				SiteCategory.Insert(Rs)
				AddRow = True
			Catch e As Exception
				If EW_DEBUG_ENABLED Then Throw
				Message = e.Message				
				AddRow = False
			End Try
		Else
			If SiteCategory.CancelMessage <> "" Then
				Message = SiteCategory.CancelMessage
				SiteCategory.CancelMessage = ""
			Else
				Message = "Insert cancelled"
			End If
			AddRow = False
		End If
		If AddRow Then
			LastInsertId = Conn.GetLastInsertId()
			SiteCategory.SiteCategoryID.DbValue = LastInsertId
			Rs("SiteCategoryID") = SiteCategory.SiteCategoryID.DbValue		

			' Row Inserted event
			SiteCategory.Row_Inserted(Rs)
			WriteAuditTrailOnAdd(Rs)
		End If
	End Function

	'
	' Load advanced search
	'
	Sub LoadAdvancedSearch()
		SiteCategory.SiteCategoryTypeID.AdvancedSearch.SearchValue = SiteCategory.GetAdvancedSearch("x_SiteCategoryTypeID")
		SiteCategory.GroupOrder.AdvancedSearch.SearchValue = SiteCategory.GetAdvancedSearch("x_GroupOrder")
		SiteCategory.CategoryName.AdvancedSearch.SearchValue = SiteCategory.GetAdvancedSearch("x_CategoryName")
		SiteCategory.CategoryTitle.AdvancedSearch.SearchValue = SiteCategory.GetAdvancedSearch("x_CategoryTitle")
		SiteCategory.CategoryDescription.AdvancedSearch.SearchValue = SiteCategory.GetAdvancedSearch("x_CategoryDescription")
		SiteCategory.CategoryKeywords.AdvancedSearch.SearchValue = SiteCategory.GetAdvancedSearch("x_CategoryKeywords")
		SiteCategory.ParentCategoryID.AdvancedSearch.SearchValue = SiteCategory.GetAdvancedSearch("x_ParentCategoryID")
		SiteCategory.CategoryFileName.AdvancedSearch.SearchValue = SiteCategory.GetAdvancedSearch("x_CategoryFileName")
		SiteCategory.SiteCategoryGroupID.AdvancedSearch.SearchValue = SiteCategory.GetAdvancedSearch("x_SiteCategoryGroupID")
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
		If SiteCategory.ExportAll Then
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
		If SiteCategory.Export = "xml" Then
			oXmlDoc = New XmlDocument()
			oXmlTbl = oXmlDoc.CreateElement("table")
			oXmlDoc.AppendChild(oXmlTbl)
		Else
			ew_Write(ew_ExportHeader(SiteCategory.Export))

			' Horizontal format, write header
			If sExportStyle <> "v" OrElse SiteCategory.Export = "csv" Then
				sExportStr = ""
				ew_ExportAddValue(sExportStr, "SiteCategoryTypeID", SiteCategory.Export)
				ew_ExportAddValue(sExportStr, "GroupOrder", SiteCategory.Export)
				ew_ExportAddValue(sExportStr, "CategoryName", SiteCategory.Export)
				ew_ExportAddValue(sExportStr, "CategoryTitle", SiteCategory.Export)
				ew_ExportAddValue(sExportStr, "CategoryDescription", SiteCategory.Export)
				ew_ExportAddValue(sExportStr, "CategoryKeywords", SiteCategory.Export)
				ew_ExportAddValue(sExportStr, "ParentCategoryID", SiteCategory.Export)
				ew_ExportAddValue(sExportStr, "CategoryFileName", SiteCategory.Export)
				ew_ExportAddValue(sExportStr, "SiteCategoryGroupID", SiteCategory.Export)
				ew_Write(ew_ExportLine(sExportStr, SiteCategory.Export))
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
				SiteCategory.RowType = EW_ROWTYPE_VIEW ' Render view
				RenderRow()
				If SiteCategory.Export = "xml" Then
					oXmlRec = oXmlDoc.CreateElement("record")
					oXmlTbl.AppendChild(oXmlRec)
					oXmlFld = oXmlDoc.CreateElement("SiteCategoryTypeID") ' SiteCategoryTypeID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(SiteCategory.SiteCategoryTypeID.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("GroupOrder") ' GroupOrder
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(SiteCategory.GroupOrder.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("CategoryName") ' CategoryName
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(SiteCategory.CategoryName.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("CategoryTitle") ' CategoryTitle
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(SiteCategory.CategoryTitle.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("CategoryDescription") ' CategoryDescription
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(SiteCategory.CategoryDescription.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("CategoryKeywords") ' CategoryKeywords
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(SiteCategory.CategoryKeywords.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("ParentCategoryID") ' ParentCategoryID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(SiteCategory.ParentCategoryID.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("CategoryFileName") ' CategoryFileName
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(SiteCategory.CategoryFileName.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("SiteCategoryGroupID") ' SiteCategoryGroupID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(SiteCategory.SiteCategoryGroupID.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
				Else

					' Vertical format
					If sExportStyle = "v" AndAlso SiteCategory.Export <> "csv" Then
						ew_Write(ew_ExportField("SiteCategoryTypeID", SiteCategory.SiteCategoryTypeID.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue), SiteCategory.Export)) ' SiteCategoryTypeID
						ew_Write(ew_ExportField("GroupOrder", SiteCategory.GroupOrder.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue), SiteCategory.Export)) ' GroupOrder
						ew_Write(ew_ExportField("CategoryName", SiteCategory.CategoryName.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue), SiteCategory.Export)) ' CategoryName
						ew_Write(ew_ExportField("CategoryTitle", SiteCategory.CategoryTitle.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue), SiteCategory.Export)) ' CategoryTitle
						ew_Write(ew_ExportField("CategoryDescription", SiteCategory.CategoryDescription.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue), SiteCategory.Export)) ' CategoryDescription
						ew_Write(ew_ExportField("CategoryKeywords", SiteCategory.CategoryKeywords.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue), SiteCategory.Export)) ' CategoryKeywords
						ew_Write(ew_ExportField("ParentCategoryID", SiteCategory.ParentCategoryID.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue), SiteCategory.Export)) ' ParentCategoryID
						ew_Write(ew_ExportField("CategoryFileName", SiteCategory.CategoryFileName.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue), SiteCategory.Export)) ' CategoryFileName
						ew_Write(ew_ExportField("SiteCategoryGroupID", SiteCategory.SiteCategoryGroupID.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue), SiteCategory.Export)) ' SiteCategoryGroupID

					' Horizontal format
					Else
						sExportStr = ""
						ew_ExportAddValue(sExportStr, SiteCategory.SiteCategoryTypeID.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue), SiteCategory.Export) ' SiteCategoryTypeID
						ew_ExportAddValue(sExportStr, SiteCategory.GroupOrder.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue), SiteCategory.Export) ' GroupOrder
						ew_ExportAddValue(sExportStr, SiteCategory.CategoryName.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue), SiteCategory.Export) ' CategoryName
						ew_ExportAddValue(sExportStr, SiteCategory.CategoryTitle.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue), SiteCategory.Export) ' CategoryTitle
						ew_ExportAddValue(sExportStr, SiteCategory.CategoryDescription.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue), SiteCategory.Export) ' CategoryDescription
						ew_ExportAddValue(sExportStr, SiteCategory.CategoryKeywords.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue), SiteCategory.Export) ' CategoryKeywords
						ew_ExportAddValue(sExportStr, SiteCategory.ParentCategoryID.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue), SiteCategory.Export) ' ParentCategoryID
						ew_ExportAddValue(sExportStr, SiteCategory.CategoryFileName.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue), SiteCategory.Export) ' CategoryFileName
						ew_ExportAddValue(sExportStr, SiteCategory.SiteCategoryGroupID.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue), SiteCategory.Export) ' SiteCategoryGroupID
						ew_Write(ew_ExportLine(sExportStr, SiteCategory.Export))
					End If
				End If
			End If
		Loop

		' Close recordset
		Rs.Close()
		Rs.Dispose()
		If SiteCategory.Export = "xml" Then
			ew_Write("<?xml version=""1.0"" standalone=""yes""?>" & vbcrlf)
			ew_Write(oXmlDoc.OuterXml)
		Else
			ew_Write(ew_ExportFooter(SiteCategory.Export))
		End If
	End Sub

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "SiteCategory"
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
		Dim table As String = "SiteCategory"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsSrc("SiteCategoryID")

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

		' GroupOrder Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "GroupOrder", keyvalue, oldvalue, RsSrc("GroupOrder"))

		' CategoryName Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "CategoryName", keyvalue, oldvalue, RsSrc("CategoryName"))

		' ParentCategoryID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "ParentCategoryID", keyvalue, oldvalue, RsSrc("ParentCategoryID"))

		' CategoryFileName Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "CategoryFileName", keyvalue, oldvalue, RsSrc("CategoryFileName"))

		' SiteCategoryGroupID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteCategoryGroupID", keyvalue, oldvalue, RsSrc("SiteCategoryGroupID"))
	End Sub

	' Write Audit Trail (edit page)
	Sub WriteAuditTrailOnEdit(ByRef RsOld As OrderedDictionary, ByRef RsNew As OrderedDictionary)
		Dim table As String = "SiteCategory"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsOld("SiteCategoryID")

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
			fld = SiteCategory.FieldByName(fldname)
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
		SiteCategory_list = New cSiteCategory_list(Me)		
		SiteCategory_list.Page_Init()

		' Page main processing
		SiteCategory_list.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If SiteCategory_list IsNot Nothing Then SiteCategory_list.Dispose()
	End Sub
End Class
