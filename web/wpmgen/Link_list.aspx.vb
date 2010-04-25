Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class Link_list
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public Link_list As cLink_list

	'
	' Page Class
	'
	Class cLink_list
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
				If Link.UseTokenInUrl Then Url = Url & "t=" & Link.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If Link.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (Link.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (Link.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' Link
		Public Property Link() As cLink
			Get				
				Return ParentPage.Link
			End Get
			Set(ByVal v As cLink)
				ParentPage.Link = v	
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
			m_PageObjName = "Link_list"
			m_PageObjTypeName = "cLink_list"

			' Table Name
			m_TableName = "Link"

			' Initialize table object
			Link = New cLink(Me)

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
			Link.Export = ew_Get("export") ' Get export parameter
			ParentPage.gsExport = Link.Export ' Get export parameter, used in header
			ParentPage.gsExportFile = Link.TableVar ' Get export file, used in header
			If Link.Export = "excel" Then
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
			Link.Dispose()
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
					Link.CurrentAction = ew_Get("a")

					' Clear inline mode
					If Link.CurrentAction = "cancel" Then
						ClearInlineMode()
					End If

					' Switch to grid edit mode
					If Link.CurrentAction = "gridedit" Then
						GridEditMode()
					End If

					' Switch to inline edit mode
					If Link.CurrentAction = "edit" Then
						InlineEditMode()
					End If

					' Switch to grid add mode
					If Link.CurrentAction = "gridadd" Then
						GridAddMode()
					End If
				End If
			Else
				Link.CurrentAction = ObjForm.GetValue("a_list") ' Get action

				' Grid Update
				If Link.CurrentAction = "gridupdate" AndAlso ew_SameStr(ew_Session(EW_SESSION_INLINE_MODE), "gridedit") Then
					GridUpdate()
				End If

				' Inline Update
				If Link.CurrentAction = "update" AndAlso ew_SameStr(ew_Session(EW_SESSION_INLINE_MODE), "edit") Then
					InlineUpdate()
				End If

				' Grid Insert
				If Link.CurrentAction = "gridinsert" AndAlso ew_SameStr(ew_Session(EW_SESSION_INLINE_MODE), "gridadd") Then
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
		If (Link.RecordsPerPage = -1 OrElse Link.RecordsPerPage > 0) Then
			lDisplayRecs = Link.RecordsPerPage ' Restore from Session
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
		Link.Recordset_Searching(sSrchWhere)

		' Save search criteria
		If sSrchWhere <> "" Then
			If sSrchBasic = "" Then ResetBasicSearchParms()
			If sSrchAdvanced = "" Then ResetAdvancedSearchParms()
			Link.SearchWhere = sSrchWhere ' Save to Session
			lStartRec = 1 ' Reset start record counter
			Link.StartRecordNumber = lStartRec
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
		Link.SessionWhere = sFilter
		Link.CurrentFilter = ""

		' Export Data only
		If Link.Export = "html" OrElse Link.Export = "csv" OrElse Link.Export = "word" OrElse Link.Export = "excel" OrElse Link.Export = "xml" Then
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
			Link.RecordsPerPage = lDisplayRecs ' Save to Session

			' Reset start position
			lStartRec = 1
			Link.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	'  Exit out of inline mode
	'
	Sub ClearInlineMode()
		Link.SetKey("ID", "") ' Clear inline edit key
		Link.CurrentAction = "" ' Clear action
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
		If ew_Get("ID") <> "" Then
			Link.ID.QueryStringValue = ew_Get("ID")
		Else
			bInlineEdit = False
		End If
		If bInlineEdit AndAlso LoadRow() Then
			Link.SetKey("ID", Link.ID.CurrentValue) ' Set up inline edit key
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
				Link.SendEmail = True ' Send email on update success
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
			Link.EventCancelled = True ' Cancel event
			Link.CurrentAction = "edit" ' Stay in edit mode
		End If
	End Sub

	'
	' Check inline edit key
	'
	Function CheckInlineEditKey() As Boolean
		If Not ew_SameStr(Link.GetKey("ID"), Link.ID.CurrentValue) Then
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
		Link.CurrentFilter = BuildKeyFilter()
		sSql = Link.SQL
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
					Link.SendEmail = False ' Do not send email on update success
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
			Link.EventCancelled = True ' Set event cancelled
			Link.CurrentAction = "gridedit" ' Stay in gridedit mode
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
				sFilter = Link.KeyFilter
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
			Link.ID.FormValue = arKeyFlds(0)
			If Not IsNumeric(Link.ID.FormValue) Then	Return False
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
				Link.SendEmail = False ' Do not send email on insert success

				' Validate Form
				If Not ValidateForm() Then
					bGridInsert = False ' Form error, reset action
					Message = ParentPage.gsFormError
				Else
					bGridInsert = AddRow() ' Insert this row
				End If
				If bGridInsert Then
					If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
					sKey = sKey & Link.ID.CurrentValue

					' Add filter for this record
					sFilter = Link.KeyFilter
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
			Link.CurrentFilter = sWrkFilter
			sSql = Link.SQL
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
			Link.EventCancelled = True ' Set event cancelled
			Link.CurrentAction = "gridadd" ' Stay in gridadd mode
		End If
	End Sub

	Function EmptyRow() As Boolean
		Dim Empty As Boolean = True
		If Empty Then Empty = ew_SameStr(Link.Title.CurrentValue, Link.Title.OldValue)
		If Empty Then Empty = ew_SameStr(Link.LinkTypeCD.CurrentValue, Link.LinkTypeCD.OldValue)
		If Empty Then Empty = ew_SameStr(Link.CategoryID.CurrentValue, Link.CategoryID.OldValue)
		If Empty Then Empty = ew_SameStr(Link.SiteCategoryGroupID.CurrentValue, Link.SiteCategoryGroupID.OldValue)
		If Empty Then Empty = ew_SameStr(Link.zPageID.CurrentValue, Link.zPageID.OldValue)
		If Empty Then Empty = ew_SameStr(Link.Views.CurrentValue, Link.Views.OldValue)
		If Empty Then Empty = ew_SameStr(Link.Ranks.CurrentValue, Link.Ranks.OldValue)
		If Empty Then Empty = ew_SameStr(Link.DateAdd.CurrentValue, Link.DateAdd.OldValue)
		Return Empty
	End Function

	'
	' Restore form values for current row
	'
	Sub RestoreCurrentRowFormValues(idx As Integer)
		Dim sKey As String, arKeyFlds() As String

		' Get row based on current index
		ObjForm.Index = idx
		If Link.CurrentAction = "gridadd" Then
			LoadFormValues() ' Load form values
		End If
		If Link.CurrentAction = "gridedit" Then
			sKey = ObjForm.GetValue("k_key")
			arKeyFlds = sKey.Split(New Char() {Convert.ToChar(EW_COMPOSITE_KEY_SEPARATOR)})
			If arKeyFlds.Length >= 1 Then
				If ew_SameStr(arKeyFlds(0), Link.ID.CurrentValue) Then
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
		BuildSearchSql(sWhere, Link.Title, False) ' Title
		BuildSearchSql(sWhere, Link.LinkTypeCD, False) ' LinkTypeCD
		BuildSearchSql(sWhere, Link.CategoryID, False) ' CategoryID
		BuildSearchSql(sWhere, Link.CompanyID, False) ' CompanyID
		BuildSearchSql(sWhere, Link.SiteCategoryGroupID, False) ' SiteCategoryGroupID
		BuildSearchSql(sWhere, Link.zPageID, False) ' PageID
		BuildSearchSql(sWhere, Link.Views, False) ' Views
		BuildSearchSql(sWhere, Link.Description, False) ' Description
		BuildSearchSql(sWhere, Link.URL, False) ' URL
		BuildSearchSql(sWhere, Link.Ranks, False) ' Ranks
		BuildSearchSql(sWhere, Link.UserID, False) ' UserID
		BuildSearchSql(sWhere, Link.ASIN, False) ' ASIN
		BuildSearchSql(sWhere, Link.DateAdd, False) ' DateAdd

		' Set up search parm
		If sWhere <> "" Then
			SetSearchParm(Link.Title) ' Title
			SetSearchParm(Link.LinkTypeCD) ' LinkTypeCD
			SetSearchParm(Link.CategoryID) ' CategoryID
			SetSearchParm(Link.CompanyID) ' CompanyID
			SetSearchParm(Link.SiteCategoryGroupID) ' SiteCategoryGroupID
			SetSearchParm(Link.zPageID) ' PageID
			SetSearchParm(Link.Views) ' Views
			SetSearchParm(Link.Description) ' Description
			SetSearchParm(Link.URL) ' URL
			SetSearchParm(Link.Ranks) ' Ranks
			SetSearchParm(Link.UserID) ' UserID
			SetSearchParm(Link.ASIN) ' ASIN
			SetSearchParm(Link.DateAdd) ' DateAdd
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
		Link.SetAdvancedSearch("x_" & FldParm, Fld.AdvancedSearch.SearchValue)
		Link.SetAdvancedSearch("z_" & FldParm, Fld.AdvancedSearch.SearchOperator)
		Link.SetAdvancedSearch("v_" & FldParm, Fld.AdvancedSearch.SearchCondition)
		Link.SetAdvancedSearch("y_" & FldParm, Fld.AdvancedSearch.SearchValue2)
		Link.SetAdvancedSearch("w_" & FldParm, Fld.AdvancedSearch.SearchOperator2)
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
		sSql = sSql & "[Title] LIKE '%" & sKeyword & "%' OR "
		sSql = sSql & "[LinkTypeCD] LIKE '%" & sKeyword & "%' OR "
		sSql = sSql & "[Description] LIKE '%" & sKeyword & "%' OR "
		sSql = sSql & "[URL] LIKE '%" & sKeyword & "%' OR "
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
			Link.BasicSearchKeyword = sSearchKeyword
			Link.BasicSearchType = sSearchType
		End If
		Return sSearchStr
	End Function

	'
	' Clear all search parameters
	'
	Sub ResetSearchParms()

		' Clear search where
		sSrchWhere = ""
		Link.SearchWhere = sSrchWhere

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
		Link.BasicSearchKeyword = ""
		Link.BasicSearchType = ""
	End Sub

	'
	' Clear all advanced search parameters
	'
	Sub ResetAdvancedSearchParms()
		Link.SetAdvancedSearch("x_Title", "")
		Link.SetAdvancedSearch("x_LinkTypeCD", "")
		Link.SetAdvancedSearch("x_CategoryID", "")
		Link.SetAdvancedSearch("x_CompanyID", "")
		Link.SetAdvancedSearch("x_SiteCategoryGroupID", "")
		Link.SetAdvancedSearch("x_zPageID", "")
		Link.SetAdvancedSearch("x_Views", "")
		Link.SetAdvancedSearch("x_Description", "")
		Link.SetAdvancedSearch("x_URL", "")
		Link.SetAdvancedSearch("x_Ranks", "")
		Link.SetAdvancedSearch("x_UserID", "")
		Link.SetAdvancedSearch("x_ASIN", "")
		Link.SetAdvancedSearch("x_DateAdd", "")
	End Sub

	'
	' Restore all search parameters
	'
	Sub RestoreSearchParms()
		sSrchWhere = Link.SearchWhere

		' Restore advanced search settings
		If ParentPage.gsSearchError = "" Then
			RestoreAdvancedSearchParms()
		End If
	End Sub

	'
	' Restore all advanced search parameters
	'
	Sub RestoreAdvancedSearchParms()
		Link.Title.AdvancedSearch.SearchValue = Link.GetAdvancedSearch("x_Title")
		Link.LinkTypeCD.AdvancedSearch.SearchValue = Link.GetAdvancedSearch("x_LinkTypeCD")
		Link.CategoryID.AdvancedSearch.SearchValue = Link.GetAdvancedSearch("x_CategoryID")
		Link.CompanyID.AdvancedSearch.SearchValue = Link.GetAdvancedSearch("x_CompanyID")
		Link.SiteCategoryGroupID.AdvancedSearch.SearchValue = Link.GetAdvancedSearch("x_SiteCategoryGroupID")
		Link.zPageID.AdvancedSearch.SearchValue = Link.GetAdvancedSearch("x_zPageID")
		Link.Views.AdvancedSearch.SearchValue = Link.GetAdvancedSearch("x_Views")
		Link.Description.AdvancedSearch.SearchValue = Link.GetAdvancedSearch("x_Description")
		Link.URL.AdvancedSearch.SearchValue = Link.GetAdvancedSearch("x_URL")
		Link.Ranks.AdvancedSearch.SearchValue = Link.GetAdvancedSearch("x_Ranks")
		Link.UserID.AdvancedSearch.SearchValue = Link.GetAdvancedSearch("x_UserID")
		Link.ASIN.AdvancedSearch.SearchValue = Link.GetAdvancedSearch("x_ASIN")
		Link.DateAdd.AdvancedSearch.SearchValue = Link.GetAdvancedSearch("x_DateAdd")
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
			Link.CurrentOrder = ew_Get("order")
			Link.CurrentOrderType = ew_Get("ordertype")
			Link.UpdateSort(Link.Title) ' Title
			Link.UpdateSort(Link.LinkTypeCD) ' LinkTypeCD
			Link.UpdateSort(Link.CategoryID) ' CategoryID
			Link.UpdateSort(Link.SiteCategoryGroupID) ' SiteCategoryGroupID
			Link.UpdateSort(Link.zPageID) ' PageID
			Link.UpdateSort(Link.Views) ' Views
			Link.UpdateSort(Link.Ranks) ' Ranks
			Link.UpdateSort(Link.DateAdd) ' DateAdd
			Link.StartRecordNumber = 1 ' Reset start position
		End If
	End Sub

	'
	' Load Sort Order parameters
	'
	Sub LoadSortOrder()
		Dim sOrderBy As String
		sOrderBy = Link.SessionOrderBy ' Get order by from Session
		If sOrderBy = "" Then
			If Link.SqlOrderBy <> "" Then
				sOrderBy = Link.SqlOrderBy
				Link.SessionOrderBy = sOrderBy
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
				Link.SessionOrderBy = sOrderBy
				Link.Title.Sort = ""
				Link.LinkTypeCD.Sort = ""
				Link.CategoryID.Sort = ""
				Link.SiteCategoryGroupID.Sort = ""
				Link.zPageID.Sort = ""
				Link.Views.Sort = ""
				Link.Ranks.Sort = ""
				Link.DateAdd.Sort = ""
			End If

			' Reset start position
			lStartRec = 1
			Link.StartRecordNumber = lStartRec
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
				Link.StartRecordNumber = lStartRec
			ElseIf ew_Get(EW_TABLE_PAGE_NO) <> "" AndAlso IsNumeric(ew_Get(EW_TABLE_PAGE_NO)) Then
				nPageNo = ew_ConvertToInt(ew_Get(EW_TABLE_PAGE_NO))
				lStartRec = (nPageNo-1)*lDisplayRecs+1
				If lStartRec <= 0 Then
					lStartRec = 1
				ElseIf lStartRec >= ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 Then
					lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1
				End If
				Link.StartRecordNumber = lStartRec
			End If
		End If
		lStartRec = Link.StartRecordNumber

		' Check if correct start record counter
		If lStartRec <= 0 Then ' Avoid invalid start record counter
			lStartRec = 1 ' Reset start record counter
			Link.StartRecordNumber = lStartRec
		ElseIf lStartRec > lTotalRecs Then ' Avoid starting record > total records
			lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to last page first record
			Link.StartRecordNumber = lStartRec
		ElseIf (lStartRec-1) Mod lDisplayRecs <> 0 Then
			lStartRec = ((lStartRec-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to page boundary
			Link.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	' Load default values
	'
	Sub LoadDefaultValues()
		Link.Title.OldValue = Link.Title.CurrentValue
		Link.LinkTypeCD.OldValue = Link.LinkTypeCD.CurrentValue
		Link.CategoryID.CurrentValue = 0
		Link.CategoryID.OldValue = Link.CategoryID.CurrentValue
		Link.SiteCategoryGroupID.OldValue = Link.SiteCategoryGroupID.CurrentValue
		Link.zPageID.CurrentValue = 0
		Link.zPageID.OldValue = Link.zPageID.CurrentValue
		Link.Views.CurrentValue = 0
		Link.Views.OldValue = Link.Views.CurrentValue
		Link.Ranks.CurrentValue = 0
		Link.Ranks.OldValue = Link.Ranks.CurrentValue
		Link.DateAdd.OldValue = Link.DateAdd.CurrentValue
	End Sub

	'
	'  Load search values for validation
	'
	Sub LoadSearchValues()
		Link.Title.AdvancedSearch.SearchValue = ew_Get("x_Title")
    	Link.Title.AdvancedSearch.SearchOperator = ew_Get("z_Title")
		Link.LinkTypeCD.AdvancedSearch.SearchValue = ew_Get("x_LinkTypeCD")
    	Link.LinkTypeCD.AdvancedSearch.SearchOperator = ew_Get("z_LinkTypeCD")
		Link.CategoryID.AdvancedSearch.SearchValue = ew_Get("x_CategoryID")
    	Link.CategoryID.AdvancedSearch.SearchOperator = ew_Get("z_CategoryID")
		Link.CompanyID.AdvancedSearch.SearchValue = ew_Get("x_CompanyID")
    	Link.CompanyID.AdvancedSearch.SearchOperator = ew_Get("z_CompanyID")
		Link.SiteCategoryGroupID.AdvancedSearch.SearchValue = ew_Get("x_SiteCategoryGroupID")
    	Link.SiteCategoryGroupID.AdvancedSearch.SearchOperator = ew_Get("z_SiteCategoryGroupID")
		Link.zPageID.AdvancedSearch.SearchValue = ew_Get("x_zPageID")
    	Link.zPageID.AdvancedSearch.SearchOperator = ew_Get("z_zPageID")
		Link.Views.AdvancedSearch.SearchValue = ew_Get("x_Views")
    	Link.Views.AdvancedSearch.SearchOperator = ew_Get("z_Views")
		Link.Description.AdvancedSearch.SearchValue = ew_Get("x_Description")
    	Link.Description.AdvancedSearch.SearchOperator = ew_Get("z_Description")
		Link.URL.AdvancedSearch.SearchValue = ew_Get("x_URL")
    	Link.URL.AdvancedSearch.SearchOperator = ew_Get("z_URL")
		Link.Ranks.AdvancedSearch.SearchValue = ew_Get("x_Ranks")
    	Link.Ranks.AdvancedSearch.SearchOperator = ew_Get("z_Ranks")
		Link.UserID.AdvancedSearch.SearchValue = ew_Get("x_UserID")
    	Link.UserID.AdvancedSearch.SearchOperator = ew_Get("z_UserID")
		Link.ASIN.AdvancedSearch.SearchValue = ew_Get("x_ASIN")
    	Link.ASIN.AdvancedSearch.SearchOperator = ew_Get("z_ASIN")
		Link.DateAdd.AdvancedSearch.SearchValue = ew_Get("x_DateAdd")
    	Link.DateAdd.AdvancedSearch.SearchOperator = ew_Get("z_DateAdd")
	End Sub

	'
	' Load form values
	'
	Sub LoadFormValues()
		Link.Title.FormValue = ObjForm.GetValue("x_Title")
		Link.Title.OldValue = ObjForm.GetValue("o_Title")
		Link.LinkTypeCD.FormValue = ObjForm.GetValue("x_LinkTypeCD")
		Link.LinkTypeCD.OldValue = ObjForm.GetValue("o_LinkTypeCD")
		Link.CategoryID.FormValue = ObjForm.GetValue("x_CategoryID")
		Link.CategoryID.OldValue = ObjForm.GetValue("o_CategoryID")
		Link.SiteCategoryGroupID.FormValue = ObjForm.GetValue("x_SiteCategoryGroupID")
		Link.SiteCategoryGroupID.OldValue = ObjForm.GetValue("o_SiteCategoryGroupID")
		Link.zPageID.FormValue = ObjForm.GetValue("x_zPageID")
		Link.zPageID.OldValue = ObjForm.GetValue("o_zPageID")
		Link.Views.FormValue = ObjForm.GetValue("x_Views")
		Link.Views.OldValue = ObjForm.GetValue("o_Views")
		Link.Ranks.FormValue = ObjForm.GetValue("x_Ranks")
		Link.Ranks.OldValue = ObjForm.GetValue("o_Ranks")
		Link.DateAdd.FormValue = ObjForm.GetValue("x_DateAdd")
		Link.DateAdd.CurrentValue = ew_UnFormatDateTime(Link.DateAdd.CurrentValue, 6)
		Link.DateAdd.OldValue = ObjForm.GetValue("o_DateAdd")
		Link.ID.FormValue = ObjForm.GetValue("x_ID")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		Link.Title.CurrentValue = Link.Title.FormValue
		Link.LinkTypeCD.CurrentValue = Link.LinkTypeCD.FormValue
		Link.CategoryID.CurrentValue = Link.CategoryID.FormValue
		Link.SiteCategoryGroupID.CurrentValue = Link.SiteCategoryGroupID.FormValue
		Link.zPageID.CurrentValue = Link.zPageID.FormValue
		Link.Views.CurrentValue = Link.Views.FormValue
		Link.Ranks.CurrentValue = Link.Ranks.FormValue
		Link.DateAdd.CurrentValue = Link.DateAdd.FormValue
		Link.DateAdd.CurrentValue = ew_UnFormatDateTime(Link.DateAdd.CurrentValue, 6)
		Link.ID.CurrentValue = Link.ID.FormValue
	End Sub

	'
	' Load recordset
	'
	Function LoadRecordset() As OleDbDataReader

		' Recordset Selecting event
		Link.Recordset_Selecting(Link.CurrentFilter)

		' Load list page SQL
		Dim sSql As String = Link.ListSQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)

		' Count
		lTotalRecs = -1		
		Try			
			If sSql.StartsWith("SELECT * FROM ", StringComparison.InvariantCultureIgnoreCase) AndAlso _				
				ew_Empty(Link.SqlGroupBy) AndAlso _
				ew_Empty(Link.SqlHaving) Then
				Dim sCntSql As String = Link.SelectCountSQL

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
		Link.Recordset_Selected(Rs)
		Return Rs
	End Function

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = Link.KeyFilter

		' Row Selecting event
		Link.Row_Selecting(sFilter)

		' Load SQL based on filter
		Link.CurrentFilter = sFilter
		Dim sSql As String = Link.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				Link.Row_Selected(RsRow)
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
		Link.ID.DbValue = RsRow("ID")
		Link.Title.DbValue = RsRow("Title")
		Link.LinkTypeCD.DbValue = RsRow("LinkTypeCD")
		Link.CategoryID.DbValue = RsRow("CategoryID")
		Link.CompanyID.DbValue = RsRow("CompanyID")
		Link.SiteCategoryGroupID.DbValue = RsRow("SiteCategoryGroupID")
		Link.zPageID.DbValue = RsRow("PageID")
		Link.Views.DbValue = IIf(ew_ConvertToBool(RsRow("Views")), "1", "0")
		Link.Description.DbValue = RsRow("Description")
		Link.URL.DbValue = RsRow("URL")
		Link.Ranks.DbValue = RsRow("Ranks")
		Link.UserID.DbValue = RsRow("UserID")
		Link.ASIN.DbValue = RsRow("ASIN")
		Link.UserName.DbValue = RsRow("UserName")
		Link.DateAdd.DbValue = RsRow("DateAdd")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		Link.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' Title

		Link.Title.CellCssStyle = ""
		Link.Title.CellCssClass = ""

		' LinkTypeCD
		Link.LinkTypeCD.CellCssStyle = ""
		Link.LinkTypeCD.CellCssClass = ""

		' CategoryID
		Link.CategoryID.CellCssStyle = ""
		Link.CategoryID.CellCssClass = ""

		' SiteCategoryGroupID
		Link.SiteCategoryGroupID.CellCssStyle = ""
		Link.SiteCategoryGroupID.CellCssClass = ""

		' PageID
		Link.zPageID.CellCssStyle = ""
		Link.zPageID.CellCssClass = ""

		' Views
		Link.Views.CellCssStyle = ""
		Link.Views.CellCssClass = ""

		' Ranks
		Link.Ranks.CellCssStyle = ""
		Link.Ranks.CellCssClass = ""

		' DateAdd
		Link.DateAdd.CellCssStyle = ""
		Link.DateAdd.CellCssClass = ""

		'
		'  View  Row
		'

		If Link.RowType = EW_ROWTYPE_VIEW Then ' View row

			' Title
			Link.Title.ViewValue = Link.Title.CurrentValue
			Link.Title.CssStyle = ""
			Link.Title.CssClass = ""
			Link.Title.ViewCustomAttributes = ""

			' LinkTypeCD
			If ew_NotEmpty(Link.LinkTypeCD.CurrentValue) Then
				sSqlWrk = "SELECT [LinkTypeDesc] FROM [LinkType] WHERE [LinkTypeCD] = '" & ew_AdjustSql(Link.LinkTypeCD.CurrentValue) & "'"
				sSqlWrk = sSqlWrk & " ORDER BY [LinkTypeDesc] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Link.LinkTypeCD.ViewValue = RsWrk("LinkTypeDesc")
				Else
					Link.LinkTypeCD.ViewValue = Link.LinkTypeCD.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Link.LinkTypeCD.ViewValue = System.DBNull.Value
			End If
			Link.LinkTypeCD.CssStyle = ""
			Link.LinkTypeCD.CssClass = ""
			Link.LinkTypeCD.ViewCustomAttributes = ""

			' CategoryID
			If ew_NotEmpty(Link.CategoryID.CurrentValue) Then
				sSqlWrk = "SELECT [Title] FROM [LinkCategory] WHERE [ID] = " & ew_AdjustSql(Link.CategoryID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [Title] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Link.CategoryID.ViewValue = RsWrk("Title")
				Else
					Link.CategoryID.ViewValue = Link.CategoryID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Link.CategoryID.ViewValue = System.DBNull.Value
			End If
			Link.CategoryID.CssStyle = ""
			Link.CategoryID.CssClass = ""
			Link.CategoryID.ViewCustomAttributes = ""

			' CompanyID
			If ew_NotEmpty(Link.CompanyID.CurrentValue) Then
				sSqlWrk = "SELECT [CompanyName] FROM [Company] WHERE [CompanyID] = " & ew_AdjustSql(Link.CompanyID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Link.CompanyID.ViewValue = RsWrk("CompanyName")
				Else
					Link.CompanyID.ViewValue = Link.CompanyID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Link.CompanyID.ViewValue = System.DBNull.Value
			End If
			Link.CompanyID.CssStyle = ""
			Link.CompanyID.CssClass = ""
			Link.CompanyID.ViewCustomAttributes = ""

			' SiteCategoryGroupID
			If ew_NotEmpty(Link.SiteCategoryGroupID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteCategoryGroupNM] FROM [SiteCategoryGroup] WHERE [SiteCategoryGroupID] = " & ew_AdjustSql(Link.SiteCategoryGroupID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryGroupNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Link.SiteCategoryGroupID.ViewValue = RsWrk("SiteCategoryGroupNM")
				Else
					Link.SiteCategoryGroupID.ViewValue = Link.SiteCategoryGroupID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Link.SiteCategoryGroupID.ViewValue = System.DBNull.Value
			End If
			Link.SiteCategoryGroupID.CssStyle = ""
			Link.SiteCategoryGroupID.CssClass = ""
			Link.SiteCategoryGroupID.ViewCustomAttributes = ""

			' PageID
			If ew_NotEmpty(Link.zPageID.CurrentValue) Then
				sSqlWrk = "SELECT [PageName] FROM [Page] WHERE [PageID] = " & ew_AdjustSql(Link.zPageID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [PageName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Link.zPageID.ViewValue = RsWrk("PageName")
				Else
					Link.zPageID.ViewValue = Link.zPageID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Link.zPageID.ViewValue = System.DBNull.Value
			End If
			Link.zPageID.CssStyle = ""
			Link.zPageID.CssClass = ""
			Link.zPageID.ViewCustomAttributes = ""

			' Views
			If Convert.ToString(Link.Views.CurrentValue) = "1" Then
				Link.Views.ViewValue = "Yes"
			Else
				Link.Views.ViewValue = "No"
			End If
			Link.Views.CssStyle = ""
			Link.Views.CssClass = ""
			Link.Views.ViewCustomAttributes = ""

			' Description
			Link.Description.ViewValue = Link.Description.CurrentValue
			Link.Description.CssStyle = ""
			Link.Description.CssClass = ""
			Link.Description.ViewCustomAttributes = ""

			' URL
			Link.URL.ViewValue = Link.URL.CurrentValue
			Link.URL.CssStyle = ""
			Link.URL.CssClass = ""
			Link.URL.ViewCustomAttributes = ""

			' Ranks
			Link.Ranks.ViewValue = Link.Ranks.CurrentValue
			Link.Ranks.CssStyle = ""
			Link.Ranks.CssClass = ""
			Link.Ranks.ViewCustomAttributes = ""

			' DateAdd
			Link.DateAdd.ViewValue = Link.DateAdd.CurrentValue
			Link.DateAdd.ViewValue = ew_FormatDateTime(Link.DateAdd.ViewValue, 6)
			Link.DateAdd.CssStyle = ""
			Link.DateAdd.CssClass = ""
			Link.DateAdd.ViewCustomAttributes = ""

			' View refer script
			' Title

			Link.Title.HrefValue = ""

			' LinkTypeCD
			Link.LinkTypeCD.HrefValue = ""

			' CategoryID
			Link.CategoryID.HrefValue = ""

			' SiteCategoryGroupID
			Link.SiteCategoryGroupID.HrefValue = ""

			' PageID
			Link.zPageID.HrefValue = ""

			' Views
			Link.Views.HrefValue = ""

			' Ranks
			Link.Ranks.HrefValue = ""

			' DateAdd
			Link.DateAdd.HrefValue = ""

		'
		'  Add Row
		'

		ElseIf Link.RowType = EW_ROWTYPE_ADD Then ' Add row

			' Title
			Link.Title.EditCustomAttributes = ""
			Link.Title.EditValue = ew_HtmlEncode(Link.Title.CurrentValue)

			' LinkTypeCD
			Link.LinkTypeCD.EditCustomAttributes = ""
			sSqlWrk = "SELECT [LinkTypeCD], [LinkTypeDesc], '' AS Disp2Fld, '' AS SelectFilterFld FROM [LinkType]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [LinkTypeDesc] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			Link.LinkTypeCD.EditValue = arwrk

			' CategoryID
			Link.CategoryID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [ID], [Title], '' AS Disp2Fld, '' AS SelectFilterFld FROM [LinkCategory]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [Title] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			Link.CategoryID.EditValue = arwrk

			' SiteCategoryGroupID
			Link.SiteCategoryGroupID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryGroupID], [SiteCategoryGroupNM], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteCategoryGroup]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryGroupNM] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			Link.SiteCategoryGroupID.EditValue = arwrk

			' PageID
			Link.zPageID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [PageID], [PageName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Page]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [PageName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			Link.zPageID.EditValue = arwrk

			' Views
			Link.Views.EditCustomAttributes = ""

			' Ranks
			Link.Ranks.EditCustomAttributes = ""
			Link.Ranks.EditValue = ew_HtmlEncode(Link.Ranks.CurrentValue)

			' DateAdd
			Link.DateAdd.EditCustomAttributes = ""
			Link.DateAdd.EditValue = ew_FormatDateTime(Link.DateAdd.CurrentValue, 6)

		'
		'  Edit Row
		'

		ElseIf Link.RowType = EW_ROWTYPE_EDIT Then ' Edit row

			' Title
			Link.Title.EditCustomAttributes = ""
			Link.Title.EditValue = ew_HtmlEncode(Link.Title.CurrentValue)

			' LinkTypeCD
			Link.LinkTypeCD.EditCustomAttributes = ""
			sSqlWrk = "SELECT [LinkTypeCD], [LinkTypeDesc], '' AS Disp2Fld, '' AS SelectFilterFld FROM [LinkType]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [LinkTypeDesc] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			Link.LinkTypeCD.EditValue = arwrk

			' CategoryID
			Link.CategoryID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [ID], [Title], '' AS Disp2Fld, '' AS SelectFilterFld FROM [LinkCategory]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [Title] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			Link.CategoryID.EditValue = arwrk

			' SiteCategoryGroupID
			Link.SiteCategoryGroupID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryGroupID], [SiteCategoryGroupNM], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteCategoryGroup]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryGroupNM] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			Link.SiteCategoryGroupID.EditValue = arwrk

			' PageID
			Link.zPageID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [PageID], [PageName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Page]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [PageName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			Link.zPageID.EditValue = arwrk

			' Views
			Link.Views.EditCustomAttributes = ""

			' Ranks
			Link.Ranks.EditCustomAttributes = ""
			Link.Ranks.EditValue = ew_HtmlEncode(Link.Ranks.CurrentValue)

			' DateAdd
			Link.DateAdd.EditCustomAttributes = ""
			Link.DateAdd.EditValue = ew_FormatDateTime(Link.DateAdd.CurrentValue, 6)

			' Edit refer script
			' Title

			Link.Title.HrefValue = ""

			' LinkTypeCD
			Link.LinkTypeCD.HrefValue = ""

			' CategoryID
			Link.CategoryID.HrefValue = ""

			' SiteCategoryGroupID
			Link.SiteCategoryGroupID.HrefValue = ""

			' PageID
			Link.zPageID.HrefValue = ""

			' Views
			Link.Views.HrefValue = ""

			' Ranks
			Link.Ranks.HrefValue = ""

			' DateAdd
			Link.DateAdd.HrefValue = ""
		End If

		' Row Rendered event
		Link.Row_Rendered()
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
		If ew_Empty(Link.Title.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter required field - Title"
		End If
		If ew_Empty(Link.LinkTypeCD.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter required field - Part Type"
		End If
		If ew_Empty(Link.CategoryID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter required field - Part Category"
		End If
		If Not ew_CheckInteger(Link.Ranks.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Incorrect integer - Ranks"
		End If
		If Not ew_CheckUSDate(Link.DateAdd.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Incorrect date, format = mm/dd/yyyy - Date Add"
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
		sFilter = Link.KeyFilter
		Link.CurrentFilter  = sFilter
		sSql = Link.SQL
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

			' Title
			Link.Title.SetDbValue(Link.Title.CurrentValue, System.DBNull.Value)
			Rs("Title") = Link.Title.DbValue

			' LinkTypeCD
			Link.LinkTypeCD.SetDbValue(Link.LinkTypeCD.CurrentValue, System.DBNull.Value)
			Rs("LinkTypeCD") = Link.LinkTypeCD.DbValue

			' CategoryID
			Link.CategoryID.SetDbValue(Link.CategoryID.CurrentValue, System.DBNull.Value)
			Rs("CategoryID") = Link.CategoryID.DbValue

			' SiteCategoryGroupID
			Link.SiteCategoryGroupID.SetDbValue(Link.SiteCategoryGroupID.CurrentValue, System.DBNull.Value)
			Rs("SiteCategoryGroupID") = Link.SiteCategoryGroupID.DbValue

			' PageID
			Link.zPageID.SetDbValue(Link.zPageID.CurrentValue, System.DBNull.Value)
			Rs("PageID") = Link.zPageID.DbValue

			' Views
			Link.Views.SetDbValue((Link.Views.CurrentValue <> "" And Not IsDBNull(Link.Views.CurrentValue)), False)
			Rs("Views") = Link.Views.DbValue

			' Ranks
			Link.Ranks.SetDbValue(Link.Ranks.CurrentValue, System.DBNull.Value)
			Rs("Ranks") = Link.Ranks.DbValue

			' DateAdd
			Link.DateAdd.SetDbValue(ew_UnFormatDateTime(Link.DateAdd.CurrentValue, 6), System.DBNull.Value)
			Rs("DateAdd") = Link.DateAdd.DbValue

			' Row Updating event
			bUpdateRow = Link.Row_Updating(RsOld, Rs)
			If bUpdateRow Then
				Try
					Link.Update(Rs)
					EditRow = True
				Catch e As Exception
					If EW_DEBUG_ENABLED Then Throw
					Message = e.Message
					EditRow = False
				End Try
			Else
				If Link.CancelMessage <> "" Then
					Message = Link.CancelMessage
					Link.CancelMessage = ""
				Else
					Message = "Update cancelled"
				End If
				EditRow = False
			End If
		End If

		' Row Updated event
		If EditRow Then
			Link.Row_Updated(RsOld, Rs)
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

		' Title
		Link.Title.SetDbValue(Link.Title.CurrentValue, System.DBNull.Value)
		Rs("Title") = Link.Title.DbValue

		' LinkTypeCD
		Link.LinkTypeCD.SetDbValue(Link.LinkTypeCD.CurrentValue, System.DBNull.Value)
		Rs("LinkTypeCD") = Link.LinkTypeCD.DbValue

		' CategoryID
		Link.CategoryID.SetDbValue(Link.CategoryID.CurrentValue, System.DBNull.Value)
		Rs("CategoryID") = Link.CategoryID.DbValue

		' SiteCategoryGroupID
		Link.SiteCategoryGroupID.SetDbValue(Link.SiteCategoryGroupID.CurrentValue, System.DBNull.Value)
		Rs("SiteCategoryGroupID") = Link.SiteCategoryGroupID.DbValue

		' PageID
		Link.zPageID.SetDbValue(Link.zPageID.CurrentValue, System.DBNull.Value)
		Rs("PageID") = Link.zPageID.DbValue

		' Views
		Link.Views.SetDbValue((Link.Views.CurrentValue <> "" And Not IsDBNull(Link.Views.CurrentValue)), False)
		Rs("Views") = Link.Views.DbValue

		' Ranks
		Link.Ranks.SetDbValue(Link.Ranks.CurrentValue, System.DBNull.Value)
		Rs("Ranks") = Link.Ranks.DbValue

		' DateAdd
		Link.DateAdd.SetDbValue(ew_UnFormatDateTime(Link.DateAdd.CurrentValue, 6), System.DBNull.Value)
		Rs("DateAdd") = Link.DateAdd.DbValue

		' Row Inserting event
		bInsertRow = Link.Row_Inserting(Rs)
		If bInsertRow Then
			Try	
				Link.Insert(Rs)
				AddRow = True
			Catch e As Exception
				If EW_DEBUG_ENABLED Then Throw
				Message = e.Message				
				AddRow = False
			End Try
		Else
			If Link.CancelMessage <> "" Then
				Message = Link.CancelMessage
				Link.CancelMessage = ""
			Else
				Message = "Insert cancelled"
			End If
			AddRow = False
		End If
		If AddRow Then
			LastInsertId = Conn.GetLastInsertId()
			Link.ID.DbValue = LastInsertId
			Rs("ID") = Link.ID.DbValue		

			' Row Inserted event
			Link.Row_Inserted(Rs)
			WriteAuditTrailOnAdd(Rs)
		End If
	End Function

	'
	' Load advanced search
	'
	Sub LoadAdvancedSearch()
		Link.Title.AdvancedSearch.SearchValue = Link.GetAdvancedSearch("x_Title")
		Link.LinkTypeCD.AdvancedSearch.SearchValue = Link.GetAdvancedSearch("x_LinkTypeCD")
		Link.CategoryID.AdvancedSearch.SearchValue = Link.GetAdvancedSearch("x_CategoryID")
		Link.CompanyID.AdvancedSearch.SearchValue = Link.GetAdvancedSearch("x_CompanyID")
		Link.SiteCategoryGroupID.AdvancedSearch.SearchValue = Link.GetAdvancedSearch("x_SiteCategoryGroupID")
		Link.zPageID.AdvancedSearch.SearchValue = Link.GetAdvancedSearch("x_zPageID")
		Link.Views.AdvancedSearch.SearchValue = Link.GetAdvancedSearch("x_Views")
		Link.Description.AdvancedSearch.SearchValue = Link.GetAdvancedSearch("x_Description")
		Link.URL.AdvancedSearch.SearchValue = Link.GetAdvancedSearch("x_URL")
		Link.Ranks.AdvancedSearch.SearchValue = Link.GetAdvancedSearch("x_Ranks")
		Link.UserID.AdvancedSearch.SearchValue = Link.GetAdvancedSearch("x_UserID")
		Link.ASIN.AdvancedSearch.SearchValue = Link.GetAdvancedSearch("x_ASIN")
		Link.DateAdd.AdvancedSearch.SearchValue = Link.GetAdvancedSearch("x_DateAdd")
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
		If Link.ExportAll Then
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
		If Link.Export = "xml" Then
			oXmlDoc = New XmlDocument()
			oXmlTbl = oXmlDoc.CreateElement("table")
			oXmlDoc.AppendChild(oXmlTbl)
		Else
			ew_Write(ew_ExportHeader(Link.Export))

			' Horizontal format, write header
			If sExportStyle <> "v" OrElse Link.Export = "csv" Then
				sExportStr = ""
				ew_ExportAddValue(sExportStr, "Title", Link.Export)
				ew_ExportAddValue(sExportStr, "LinkTypeCD", Link.Export)
				ew_ExportAddValue(sExportStr, "CategoryID", Link.Export)
				ew_ExportAddValue(sExportStr, "CompanyID", Link.Export)
				ew_ExportAddValue(sExportStr, "SiteCategoryGroupID", Link.Export)
				ew_ExportAddValue(sExportStr, "PageID", Link.Export)
				ew_ExportAddValue(sExportStr, "Views", Link.Export)
				ew_ExportAddValue(sExportStr, "Description", Link.Export)
				ew_ExportAddValue(sExportStr, "URL", Link.Export)
				ew_ExportAddValue(sExportStr, "Ranks", Link.Export)
				ew_ExportAddValue(sExportStr, "DateAdd", Link.Export)
				ew_Write(ew_ExportLine(sExportStr, Link.Export))
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
				Link.RowType = EW_ROWTYPE_VIEW ' Render view
				RenderRow()
				If Link.Export = "xml" Then
					oXmlRec = oXmlDoc.CreateElement("record")
					oXmlTbl.AppendChild(oXmlRec)
					oXmlFld = oXmlDoc.CreateElement("Title") ' Title
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Link.Title.ExportValue(Link.Export, Link.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("LinkTypeCD") ' LinkTypeCD
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Link.LinkTypeCD.ExportValue(Link.Export, Link.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("CategoryID") ' CategoryID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Link.CategoryID.ExportValue(Link.Export, Link.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("CompanyID") ' CompanyID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Link.CompanyID.ExportValue(Link.Export, Link.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("SiteCategoryGroupID") ' SiteCategoryGroupID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Link.SiteCategoryGroupID.ExportValue(Link.Export, Link.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("zPageID") ' PageID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Link.zPageID.ExportValue(Link.Export, Link.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("Views") ' Views
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Link.Views.ExportValue(Link.Export, Link.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("Description") ' Description
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Link.Description.ExportValue(Link.Export, Link.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("URL") ' URL
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Link.URL.ExportValue(Link.Export, Link.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("Ranks") ' Ranks
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Link.Ranks.ExportValue(Link.Export, Link.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("DateAdd") ' DateAdd
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Link.DateAdd.ExportValue(Link.Export, Link.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
				Else

					' Vertical format
					If sExportStyle = "v" AndAlso Link.Export <> "csv" Then
						ew_Write(ew_ExportField("Title", Link.Title.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export)) ' Title
						ew_Write(ew_ExportField("LinkTypeCD", Link.LinkTypeCD.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export)) ' LinkTypeCD
						ew_Write(ew_ExportField("CategoryID", Link.CategoryID.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export)) ' CategoryID
						ew_Write(ew_ExportField("CompanyID", Link.CompanyID.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export)) ' CompanyID
						ew_Write(ew_ExportField("SiteCategoryGroupID", Link.SiteCategoryGroupID.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export)) ' SiteCategoryGroupID
						ew_Write(ew_ExportField("PageID", Link.zPageID.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export)) ' PageID
						ew_Write(ew_ExportField("Views", Link.Views.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export)) ' Views
						ew_Write(ew_ExportField("Description", Link.Description.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export)) ' Description
						ew_Write(ew_ExportField("URL", Link.URL.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export)) ' URL
						ew_Write(ew_ExportField("Ranks", Link.Ranks.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export)) ' Ranks
						ew_Write(ew_ExportField("DateAdd", Link.DateAdd.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export)) ' DateAdd

					' Horizontal format
					Else
						sExportStr = ""
						ew_ExportAddValue(sExportStr, Link.Title.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export) ' Title
						ew_ExportAddValue(sExportStr, Link.LinkTypeCD.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export) ' LinkTypeCD
						ew_ExportAddValue(sExportStr, Link.CategoryID.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export) ' CategoryID
						ew_ExportAddValue(sExportStr, Link.CompanyID.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export) ' CompanyID
						ew_ExportAddValue(sExportStr, Link.SiteCategoryGroupID.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export) ' SiteCategoryGroupID
						ew_ExportAddValue(sExportStr, Link.zPageID.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export) ' PageID
						ew_ExportAddValue(sExportStr, Link.Views.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export) ' Views
						ew_ExportAddValue(sExportStr, Link.Description.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export) ' Description
						ew_ExportAddValue(sExportStr, Link.URL.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export) ' URL
						ew_ExportAddValue(sExportStr, Link.Ranks.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export) ' Ranks
						ew_ExportAddValue(sExportStr, Link.DateAdd.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export) ' DateAdd
						ew_Write(ew_ExportLine(sExportStr, Link.Export))
					End If
				End If
			End If
		Loop

		' Close recordset
		Rs.Close()
		Rs.Dispose()
		If Link.Export = "xml" Then
			ew_Write("<?xml version=""1.0"" standalone=""yes""?>" & vbcrlf)
			ew_Write(oXmlDoc.OuterXml)
		Else
			ew_Write(ew_ExportFooter(Link.Export))
		End If
	End Sub

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "Link"
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
		Dim table As String = "Link"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsSrc("ID")

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

		' Title Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "Title", keyvalue, oldvalue, RsSrc("Title"))

		' LinkTypeCD Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "LinkTypeCD", keyvalue, oldvalue, RsSrc("LinkTypeCD"))

		' CategoryID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "CategoryID", keyvalue, oldvalue, RsSrc("CategoryID"))

		' SiteCategoryGroupID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteCategoryGroupID", keyvalue, oldvalue, RsSrc("SiteCategoryGroupID"))

		' PageID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "PageID", keyvalue, oldvalue, RsSrc("PageID"))

		' Views Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "Views", keyvalue, oldvalue, RsSrc("Views"))

		' Ranks Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "Ranks", keyvalue, oldvalue, RsSrc("Ranks"))

		' DateAdd Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "DateAdd", keyvalue, oldvalue, RsSrc("DateAdd"))
	End Sub

	' Write Audit Trail (edit page)
	Sub WriteAuditTrailOnEdit(ByRef RsOld As OrderedDictionary, ByRef RsNew As OrderedDictionary)
		Dim table As String = "Link"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsOld("ID")

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
			fld = Link.FieldByName(fldname)
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
		Link_list = New cLink_list(Me)		
		Link_list.Page_Init()

		' Page main processing
		Link_list.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If Link_list IsNot Nothing Then Link_list.Dispose()
	End Sub
End Class
