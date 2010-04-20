Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class SiteLink_list
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public SiteLink_list As cSiteLink_list

	'
	' Page Class
	'
	Class cSiteLink_list
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
				If SiteLink.UseTokenInUrl Then Url = Url & "t=" & SiteLink.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If SiteLink.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (SiteLink.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (SiteLink.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' SiteLink
		Public Property SiteLink() As cSiteLink
			Get				
				Return ParentPage.SiteLink
			End Get
			Set(ByVal v As cSiteLink)
				ParentPage.SiteLink = v	
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
			m_PageObjName = "SiteLink_list"
			m_PageObjTypeName = "cSiteLink_list"

			' Table Name
			m_TableName = "SiteLink"

			' Initialize table object
			SiteLink = New cSiteLink(Me)

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
			SiteLink.Export = ew_Get("export") ' Get export parameter
			ParentPage.gsExport = SiteLink.Export ' Get export parameter, used in header
			ParentPage.gsExportFile = SiteLink.TableVar ' Get export file, used in header
			If SiteLink.Export = "excel" Then
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
			SiteLink.Dispose()
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
					SiteLink.CurrentAction = ew_Get("a")

					' Clear inline mode
					If SiteLink.CurrentAction = "cancel" Then
						ClearInlineMode()
					End If

					' Switch to grid edit mode
					If SiteLink.CurrentAction = "gridedit" Then
						GridEditMode()
					End If

					' Switch to grid add mode
					If SiteLink.CurrentAction = "gridadd" Then
						GridAddMode()
					End If
				End If
			Else
				SiteLink.CurrentAction = ObjForm.GetValue("a_list") ' Get action

				' Grid Update
				If SiteLink.CurrentAction = "gridupdate" AndAlso ew_SameStr(ew_Session(EW_SESSION_INLINE_MODE), "gridedit") Then
					GridUpdate()
				End If

				' Grid Insert
				If SiteLink.CurrentAction = "gridinsert" AndAlso ew_SameStr(ew_Session(EW_SESSION_INLINE_MODE), "gridadd") Then
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
		If (SiteLink.RecordsPerPage = -1 OrElse SiteLink.RecordsPerPage > 0) Then
			lDisplayRecs = SiteLink.RecordsPerPage ' Restore from Session
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
		SiteLink.Recordset_Searching(sSrchWhere)

		' Save search criteria
		If sSrchWhere <> "" Then
			If sSrchBasic = "" Then ResetBasicSearchParms()
			If sSrchAdvanced = "" Then ResetAdvancedSearchParms()
			SiteLink.SearchWhere = sSrchWhere ' Save to Session
			lStartRec = 1 ' Reset start record counter
			SiteLink.StartRecordNumber = lStartRec
		Else
			RestoreSearchParms()
		End If

		' Build filter
		sFilter = "[SiteCategoryTypeID]="  & httpContext.Current.Session("SiteCategoryTypeID") & " "
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
		SiteLink.SessionWhere = sFilter
		SiteLink.CurrentFilter = ""

		' Export Data only
		If SiteLink.Export = "html" OrElse SiteLink.Export = "csv" OrElse SiteLink.Export = "word" OrElse SiteLink.Export = "excel" OrElse SiteLink.Export = "xml" Then
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
			SiteLink.RecordsPerPage = lDisplayRecs ' Save to Session

			' Reset start position
			lStartRec = 1
			SiteLink.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	'  Exit out of inline mode
	'
	Sub ClearInlineMode()
		SiteLink.CurrentAction = "" ' Clear action
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
		SiteLink.CurrentFilter = BuildKeyFilter()
		sSql = SiteLink.SQL
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
					SiteLink.SendEmail = False ' Do not send email on update success
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
			SiteLink.EventCancelled = True ' Set event cancelled
			SiteLink.CurrentAction = "gridedit" ' Stay in gridedit mode
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
				sFilter = SiteLink.KeyFilter
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
			SiteLink.ID.FormValue = arKeyFlds(0)
			If Not IsNumeric(SiteLink.ID.FormValue) Then	Return False
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
				SiteLink.SendEmail = False ' Do not send email on insert success

				' Validate Form
				If Not ValidateForm() Then
					bGridInsert = False ' Form error, reset action
					Message = ParentPage.gsFormError
				Else
					bGridInsert = AddRow() ' Insert this row
				End If
				If bGridInsert Then
					If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
					sKey = sKey & SiteLink.ID.CurrentValue

					' Add filter for this record
					sFilter = SiteLink.KeyFilter
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
			SiteLink.CurrentFilter = sWrkFilter
			sSql = SiteLink.SQL
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
			SiteLink.EventCancelled = True ' Set event cancelled
			SiteLink.CurrentAction = "gridadd" ' Stay in gridadd mode
		End If
	End Sub

	Function EmptyRow() As Boolean
		Dim Empty As Boolean = True
		If Empty Then Empty = ew_SameStr(SiteLink.SiteCategoryTypeID.CurrentValue, SiteLink.SiteCategoryTypeID.OldValue)
		If Empty Then Empty = ew_SameStr(SiteLink.Title.CurrentValue, SiteLink.Title.OldValue)
		If Empty Then Empty = ew_SameStr(SiteLink.LinkTypeCD.CurrentValue, SiteLink.LinkTypeCD.OldValue)
		If Empty Then Empty = ew_SameStr(SiteLink.CategoryID.CurrentValue, SiteLink.CategoryID.OldValue)
		If Empty Then Empty = ew_SameStr(SiteLink.CompanyID.CurrentValue, SiteLink.CompanyID.OldValue)
		If Empty Then Empty = ew_SameStr(SiteLink.SiteCategoryID.CurrentValue, SiteLink.SiteCategoryID.OldValue)
		If Empty Then Empty = ew_SameStr(SiteLink.SiteCategoryGroupID.CurrentValue, SiteLink.SiteCategoryGroupID.OldValue)
		If Empty Then Empty = ew_SameStr(SiteLink.Ranks.CurrentValue, SiteLink.Ranks.OldValue)
		If Empty Then Empty = ew_SameStr(SiteLink.Views.CurrentValue, SiteLink.Views.OldValue)
		Return Empty
	End Function

	'
	' Restore form values for current row
	'
	Sub RestoreCurrentRowFormValues(idx As Integer)
		Dim sKey As String, arKeyFlds() As String

		' Get row based on current index
		ObjForm.Index = idx
		If SiteLink.CurrentAction = "gridadd" Then
			LoadFormValues() ' Load form values
		End If
		If SiteLink.CurrentAction = "gridedit" Then
			sKey = ObjForm.GetValue("k_key")
			arKeyFlds = sKey.Split(New Char() {Convert.ToChar(EW_COMPOSITE_KEY_SEPARATOR)})
			If arKeyFlds.Length >= 1 Then
				If ew_SameStr(arKeyFlds(0), SiteLink.ID.CurrentValue) Then
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
		BuildSearchSql(sWhere, SiteLink.SiteCategoryTypeID, False) ' SiteCategoryTypeID
		BuildSearchSql(sWhere, SiteLink.Title, False) ' Title
		BuildSearchSql(sWhere, SiteLink.LinkTypeCD, False) ' LinkTypeCD
		BuildSearchSql(sWhere, SiteLink.CategoryID, False) ' CategoryID
		BuildSearchSql(sWhere, SiteLink.CompanyID, False) ' CompanyID
		BuildSearchSql(sWhere, SiteLink.SiteCategoryID, False) ' SiteCategoryID
		BuildSearchSql(sWhere, SiteLink.SiteCategoryGroupID, False) ' SiteCategoryGroupID
		BuildSearchSql(sWhere, SiteLink.Ranks, False) ' Ranks
		BuildSearchSql(sWhere, SiteLink.Views, False) ' Views

		' Set up search parm
		If sWhere <> "" Then
			SetSearchParm(SiteLink.SiteCategoryTypeID) ' SiteCategoryTypeID
			SetSearchParm(SiteLink.Title) ' Title
			SetSearchParm(SiteLink.LinkTypeCD) ' LinkTypeCD
			SetSearchParm(SiteLink.CategoryID) ' CategoryID
			SetSearchParm(SiteLink.CompanyID) ' CompanyID
			SetSearchParm(SiteLink.SiteCategoryID) ' SiteCategoryID
			SetSearchParm(SiteLink.SiteCategoryGroupID) ' SiteCategoryGroupID
			SetSearchParm(SiteLink.Ranks) ' Ranks
			SetSearchParm(SiteLink.Views) ' Views
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
		SiteLink.SetAdvancedSearch("x_" & FldParm, Fld.AdvancedSearch.SearchValue)
		SiteLink.SetAdvancedSearch("z_" & FldParm, Fld.AdvancedSearch.SearchOperator)
		SiteLink.SetAdvancedSearch("v_" & FldParm, Fld.AdvancedSearch.SearchCondition)
		SiteLink.SetAdvancedSearch("y_" & FldParm, Fld.AdvancedSearch.SearchValue2)
		SiteLink.SetAdvancedSearch("w_" & FldParm, Fld.AdvancedSearch.SearchOperator2)
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
			SiteLink.BasicSearchKeyword = sSearchKeyword
			SiteLink.BasicSearchType = sSearchType
		End If
		Return sSearchStr
	End Function

	'
	' Clear all search parameters
	'
	Sub ResetSearchParms()

		' Clear search where
		sSrchWhere = ""
		SiteLink.SearchWhere = sSrchWhere

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
		SiteLink.BasicSearchKeyword = ""
		SiteLink.BasicSearchType = ""
	End Sub

	'
	' Clear all advanced search parameters
	'
	Sub ResetAdvancedSearchParms()
		SiteLink.SetAdvancedSearch("x_SiteCategoryTypeID", "")
		SiteLink.SetAdvancedSearch("x_Title", "")
		SiteLink.SetAdvancedSearch("x_LinkTypeCD", "")
		SiteLink.SetAdvancedSearch("x_CategoryID", "")
		SiteLink.SetAdvancedSearch("x_CompanyID", "")
		SiteLink.SetAdvancedSearch("x_SiteCategoryID", "")
		SiteLink.SetAdvancedSearch("x_SiteCategoryGroupID", "")
		SiteLink.SetAdvancedSearch("x_Ranks", "")
		SiteLink.SetAdvancedSearch("x_Views", "")
	End Sub

	'
	' Restore all search parameters
	'
	Sub RestoreSearchParms()
		sSrchWhere = SiteLink.SearchWhere

		' Restore advanced search settings
		If ParentPage.gsSearchError = "" Then
			RestoreAdvancedSearchParms()
		End If
	End Sub

	'
	' Restore all advanced search parameters
	'
	Sub RestoreAdvancedSearchParms()
		SiteLink.SiteCategoryTypeID.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_SiteCategoryTypeID")
		SiteLink.Title.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_Title")
		SiteLink.LinkTypeCD.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_LinkTypeCD")
		SiteLink.CategoryID.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_CategoryID")
		SiteLink.CompanyID.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_CompanyID")
		SiteLink.SiteCategoryID.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_SiteCategoryID")
		SiteLink.SiteCategoryGroupID.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_SiteCategoryGroupID")
		SiteLink.Ranks.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_Ranks")
		SiteLink.Views.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_Views")
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
			SiteLink.CurrentOrder = ew_Get("order")
			SiteLink.CurrentOrderType = ew_Get("ordertype")
			SiteLink.UpdateSort(SiteLink.SiteCategoryTypeID) ' SiteCategoryTypeID
			SiteLink.UpdateSort(SiteLink.Title) ' Title
			SiteLink.UpdateSort(SiteLink.LinkTypeCD) ' LinkTypeCD
			SiteLink.UpdateSort(SiteLink.CategoryID) ' CategoryID
			SiteLink.UpdateSort(SiteLink.CompanyID) ' CompanyID
			SiteLink.UpdateSort(SiteLink.SiteCategoryID) ' SiteCategoryID
			SiteLink.UpdateSort(SiteLink.SiteCategoryGroupID) ' SiteCategoryGroupID
			SiteLink.UpdateSort(SiteLink.Ranks) ' Ranks
			SiteLink.UpdateSort(SiteLink.Views) ' Views
			SiteLink.StartRecordNumber = 1 ' Reset start position
		End If
	End Sub

	'
	' Load Sort Order parameters
	'
	Sub LoadSortOrder()
		Dim sOrderBy As String
		sOrderBy = SiteLink.SessionOrderBy ' Get order by from Session
		If sOrderBy = "" Then
			If SiteLink.SqlOrderBy <> "" Then
				sOrderBy = SiteLink.SqlOrderBy
				SiteLink.SessionOrderBy = sOrderBy
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
				SiteLink.SessionOrderBy = sOrderBy
				SiteLink.SiteCategoryTypeID.Sort = ""
				SiteLink.Title.Sort = ""
				SiteLink.LinkTypeCD.Sort = ""
				SiteLink.CategoryID.Sort = ""
				SiteLink.CompanyID.Sort = ""
				SiteLink.SiteCategoryID.Sort = ""
				SiteLink.SiteCategoryGroupID.Sort = ""
				SiteLink.Ranks.Sort = ""
				SiteLink.Views.Sort = ""
			End If

			' Reset start position
			lStartRec = 1
			SiteLink.StartRecordNumber = lStartRec
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
				SiteLink.StartRecordNumber = lStartRec
			ElseIf ew_Get(EW_TABLE_PAGE_NO) <> "" AndAlso IsNumeric(ew_Get(EW_TABLE_PAGE_NO)) Then
				nPageNo = ew_ConvertToInt(ew_Get(EW_TABLE_PAGE_NO))
				lStartRec = (nPageNo-1)*lDisplayRecs+1
				If lStartRec <= 0 Then
					lStartRec = 1
				ElseIf lStartRec >= ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 Then
					lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1
				End If
				SiteLink.StartRecordNumber = lStartRec
			End If
		End If
		lStartRec = SiteLink.StartRecordNumber

		' Check if correct start record counter
		If lStartRec <= 0 Then ' Avoid invalid start record counter
			lStartRec = 1 ' Reset start record counter
			SiteLink.StartRecordNumber = lStartRec
		ElseIf lStartRec > lTotalRecs Then ' Avoid starting record > total records
			lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to last page first record
			SiteLink.StartRecordNumber = lStartRec
		ElseIf (lStartRec-1) Mod lDisplayRecs <> 0 Then
			lStartRec = ((lStartRec-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to page boundary
			SiteLink.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	' Load default values
	'
	Sub LoadDefaultValues()
		SiteLink.SiteCategoryTypeID.OldValue = SiteLink.SiteCategoryTypeID.CurrentValue
		SiteLink.Title.OldValue = SiteLink.Title.CurrentValue
		SiteLink.LinkTypeCD.OldValue = SiteLink.LinkTypeCD.CurrentValue
		SiteLink.CategoryID.CurrentValue = 0
		SiteLink.CategoryID.OldValue = SiteLink.CategoryID.CurrentValue
		SiteLink.CompanyID.CurrentValue = 0
		SiteLink.CompanyID.OldValue = SiteLink.CompanyID.CurrentValue
		SiteLink.SiteCategoryID.OldValue = SiteLink.SiteCategoryID.CurrentValue
		SiteLink.SiteCategoryGroupID.OldValue = SiteLink.SiteCategoryGroupID.CurrentValue
		SiteLink.Ranks.CurrentValue = 0
		SiteLink.Ranks.OldValue = SiteLink.Ranks.CurrentValue
		SiteLink.Views.CurrentValue = 0
		SiteLink.Views.OldValue = SiteLink.Views.CurrentValue
	End Sub

	'
	'  Load search values for validation
	'
	Sub LoadSearchValues()
		SiteLink.SiteCategoryTypeID.AdvancedSearch.SearchValue = ew_Get("x_SiteCategoryTypeID")
    	SiteLink.SiteCategoryTypeID.AdvancedSearch.SearchOperator = ew_Get("z_SiteCategoryTypeID")
		SiteLink.Title.AdvancedSearch.SearchValue = ew_Get("x_Title")
    	SiteLink.Title.AdvancedSearch.SearchOperator = ew_Get("z_Title")
		SiteLink.LinkTypeCD.AdvancedSearch.SearchValue = ew_Get("x_LinkTypeCD")
    	SiteLink.LinkTypeCD.AdvancedSearch.SearchOperator = ew_Get("z_LinkTypeCD")
		SiteLink.CategoryID.AdvancedSearch.SearchValue = ew_Get("x_CategoryID")
    	SiteLink.CategoryID.AdvancedSearch.SearchOperator = ew_Get("z_CategoryID")
		SiteLink.CompanyID.AdvancedSearch.SearchValue = ew_Get("x_CompanyID")
    	SiteLink.CompanyID.AdvancedSearch.SearchOperator = ew_Get("z_CompanyID")
		SiteLink.SiteCategoryID.AdvancedSearch.SearchValue = ew_Get("x_SiteCategoryID")
    	SiteLink.SiteCategoryID.AdvancedSearch.SearchOperator = ew_Get("z_SiteCategoryID")
		SiteLink.SiteCategoryGroupID.AdvancedSearch.SearchValue = ew_Get("x_SiteCategoryGroupID")
    	SiteLink.SiteCategoryGroupID.AdvancedSearch.SearchOperator = ew_Get("z_SiteCategoryGroupID")
		SiteLink.Ranks.AdvancedSearch.SearchValue = ew_Get("x_Ranks")
    	SiteLink.Ranks.AdvancedSearch.SearchOperator = ew_Get("z_Ranks")
		SiteLink.Views.AdvancedSearch.SearchValue = ew_Get("x_Views")
    	SiteLink.Views.AdvancedSearch.SearchOperator = ew_Get("z_Views")
	End Sub

	'
	' Load form values
	'
	Sub LoadFormValues()
		SiteLink.SiteCategoryTypeID.FormValue = ObjForm.GetValue("x_SiteCategoryTypeID")
		SiteLink.SiteCategoryTypeID.OldValue = ObjForm.GetValue("o_SiteCategoryTypeID")
		SiteLink.Title.FormValue = ObjForm.GetValue("x_Title")
		SiteLink.Title.OldValue = ObjForm.GetValue("o_Title")
		SiteLink.LinkTypeCD.FormValue = ObjForm.GetValue("x_LinkTypeCD")
		SiteLink.LinkTypeCD.OldValue = ObjForm.GetValue("o_LinkTypeCD")
		SiteLink.CategoryID.FormValue = ObjForm.GetValue("x_CategoryID")
		SiteLink.CategoryID.OldValue = ObjForm.GetValue("o_CategoryID")
		SiteLink.CompanyID.FormValue = ObjForm.GetValue("x_CompanyID")
		SiteLink.CompanyID.OldValue = ObjForm.GetValue("o_CompanyID")
		SiteLink.SiteCategoryID.FormValue = ObjForm.GetValue("x_SiteCategoryID")
		SiteLink.SiteCategoryID.OldValue = ObjForm.GetValue("o_SiteCategoryID")
		SiteLink.SiteCategoryGroupID.FormValue = ObjForm.GetValue("x_SiteCategoryGroupID")
		SiteLink.SiteCategoryGroupID.OldValue = ObjForm.GetValue("o_SiteCategoryGroupID")
		SiteLink.Ranks.FormValue = ObjForm.GetValue("x_Ranks")
		SiteLink.Ranks.OldValue = ObjForm.GetValue("o_Ranks")
		SiteLink.Views.FormValue = ObjForm.GetValue("x_Views")
		SiteLink.Views.OldValue = ObjForm.GetValue("o_Views")
		SiteLink.ID.FormValue = ObjForm.GetValue("x_ID")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		SiteLink.SiteCategoryTypeID.CurrentValue = SiteLink.SiteCategoryTypeID.FormValue
		SiteLink.Title.CurrentValue = SiteLink.Title.FormValue
		SiteLink.LinkTypeCD.CurrentValue = SiteLink.LinkTypeCD.FormValue
		SiteLink.CategoryID.CurrentValue = SiteLink.CategoryID.FormValue
		SiteLink.CompanyID.CurrentValue = SiteLink.CompanyID.FormValue
		SiteLink.SiteCategoryID.CurrentValue = SiteLink.SiteCategoryID.FormValue
		SiteLink.SiteCategoryGroupID.CurrentValue = SiteLink.SiteCategoryGroupID.FormValue
		SiteLink.Ranks.CurrentValue = SiteLink.Ranks.FormValue
		SiteLink.Views.CurrentValue = SiteLink.Views.FormValue
		SiteLink.ID.CurrentValue = SiteLink.ID.FormValue
	End Sub

	'
	' Load recordset
	'
	Function LoadRecordset() As OleDbDataReader

		' Recordset Selecting event
		SiteLink.Recordset_Selecting(SiteLink.CurrentFilter)

		' Load list page SQL
		Dim sSql As String = SiteLink.ListSQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)

		' Count
		lTotalRecs = -1		
		Try			
			If sSql.StartsWith("SELECT * FROM ", StringComparison.InvariantCultureIgnoreCase) AndAlso _				
				ew_Empty(SiteLink.SqlGroupBy) AndAlso _
				ew_Empty(SiteLink.SqlHaving) Then
				Dim sCntSql As String = SiteLink.SelectCountSQL

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
		SiteLink.Recordset_Selected(Rs)
		Return Rs
	End Function

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = SiteLink.KeyFilter

		' Row Selecting event
		SiteLink.Row_Selecting(sFilter)

		' Load SQL based on filter
		SiteLink.CurrentFilter = sFilter
		Dim sSql As String = SiteLink.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				SiteLink.Row_Selected(RsRow)
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
		SiteLink.ID.DbValue = RsRow("ID")
		SiteLink.SiteCategoryTypeID.DbValue = RsRow("SiteCategoryTypeID")
		SiteLink.Title.DbValue = RsRow("Title")
		SiteLink.LinkTypeCD.DbValue = RsRow("LinkTypeCD")
		SiteLink.CategoryID.DbValue = RsRow("CategoryID")
		SiteLink.CompanyID.DbValue = RsRow("CompanyID")
		SiteLink.SiteCategoryID.DbValue = RsRow("SiteCategoryID")
		SiteLink.SiteCategoryGroupID.DbValue = RsRow("SiteCategoryGroupID")
		SiteLink.Description.DbValue = RsRow("Description")
		SiteLink.URL.DbValue = RsRow("URL")
		SiteLink.Ranks.DbValue = RsRow("Ranks")
		SiteLink.Views.DbValue = IIf(ew_ConvertToBool(RsRow("Views")), "1", "0")
		SiteLink.DateAdd.DbValue = RsRow("DateAdd")
		SiteLink.UserName.DbValue = RsRow("UserName")
		SiteLink.UserID.DbValue = RsRow("UserID")
		SiteLink.ASIN.DbValue = RsRow("ASIN")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		SiteLink.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' SiteCategoryTypeID

		SiteLink.SiteCategoryTypeID.CellCssStyle = ""
		SiteLink.SiteCategoryTypeID.CellCssClass = ""

		' Title
		SiteLink.Title.CellCssStyle = ""
		SiteLink.Title.CellCssClass = ""

		' LinkTypeCD
		SiteLink.LinkTypeCD.CellCssStyle = ""
		SiteLink.LinkTypeCD.CellCssClass = ""

		' CategoryID
		SiteLink.CategoryID.CellCssStyle = ""
		SiteLink.CategoryID.CellCssClass = ""

		' CompanyID
		SiteLink.CompanyID.CellCssStyle = ""
		SiteLink.CompanyID.CellCssClass = ""

		' SiteCategoryID
		SiteLink.SiteCategoryID.CellCssStyle = ""
		SiteLink.SiteCategoryID.CellCssClass = ""

		' SiteCategoryGroupID
		SiteLink.SiteCategoryGroupID.CellCssStyle = ""
		SiteLink.SiteCategoryGroupID.CellCssClass = ""

		' Ranks
		SiteLink.Ranks.CellCssStyle = ""
		SiteLink.Ranks.CellCssClass = ""

		' Views
		SiteLink.Views.CellCssStyle = ""
		SiteLink.Views.CellCssClass = ""

		'
		'  View  Row
		'

		If SiteLink.RowType = EW_ROWTYPE_VIEW Then ' View row

			' SiteCategoryTypeID
			If ew_NotEmpty(SiteLink.SiteCategoryTypeID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteCategoryTypeNM] FROM [SiteCategoryType] WHERE [SiteCategoryTypeID] = " & ew_AdjustSql(SiteLink.SiteCategoryTypeID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryTypeNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					SiteLink.SiteCategoryTypeID.ViewValue = RsWrk("SiteCategoryTypeNM")
				Else
					SiteLink.SiteCategoryTypeID.ViewValue = SiteLink.SiteCategoryTypeID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				SiteLink.SiteCategoryTypeID.ViewValue = System.DBNull.Value
			End If
			SiteLink.SiteCategoryTypeID.CssStyle = ""
			SiteLink.SiteCategoryTypeID.CssClass = ""
			SiteLink.SiteCategoryTypeID.ViewCustomAttributes = ""

			' Title
			SiteLink.Title.ViewValue = SiteLink.Title.CurrentValue
			SiteLink.Title.CssStyle = ""
			SiteLink.Title.CssClass = ""
			SiteLink.Title.ViewCustomAttributes = ""

			' LinkTypeCD
			If ew_NotEmpty(SiteLink.LinkTypeCD.CurrentValue) Then
				sSqlWrk = "SELECT [LinkTypeDesc] FROM [LinkType] WHERE [LinkTypeCD] = '" & ew_AdjustSql(SiteLink.LinkTypeCD.CurrentValue) & "'"
				sSqlWrk = sSqlWrk & " ORDER BY [LinkTypeDesc] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					SiteLink.LinkTypeCD.ViewValue = RsWrk("LinkTypeDesc")
				Else
					SiteLink.LinkTypeCD.ViewValue = SiteLink.LinkTypeCD.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				SiteLink.LinkTypeCD.ViewValue = System.DBNull.Value
			End If
			SiteLink.LinkTypeCD.CssStyle = ""
			SiteLink.LinkTypeCD.CssClass = ""
			SiteLink.LinkTypeCD.ViewCustomAttributes = ""

			' CategoryID
			If ew_NotEmpty(SiteLink.CategoryID.CurrentValue) Then
				sSqlWrk = "SELECT [Title] FROM [LinkCategory] WHERE [ID] = " & ew_AdjustSql(SiteLink.CategoryID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [Title] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					SiteLink.CategoryID.ViewValue = RsWrk("Title")
				Else
					SiteLink.CategoryID.ViewValue = SiteLink.CategoryID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				SiteLink.CategoryID.ViewValue = System.DBNull.Value
			End If
			SiteLink.CategoryID.CssStyle = ""
			SiteLink.CategoryID.CssClass = ""
			SiteLink.CategoryID.ViewCustomAttributes = ""

			' CompanyID
			If ew_NotEmpty(SiteLink.CompanyID.CurrentValue) Then
				sSqlWrk = "SELECT [CompanyName] FROM [Company] WHERE [CompanyID] = " & ew_AdjustSql(SiteLink.CompanyID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					SiteLink.CompanyID.ViewValue = RsWrk("CompanyName")
				Else
					SiteLink.CompanyID.ViewValue = SiteLink.CompanyID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				SiteLink.CompanyID.ViewValue = System.DBNull.Value
			End If
			SiteLink.CompanyID.CssStyle = ""
			SiteLink.CompanyID.CssClass = ""
			SiteLink.CompanyID.ViewCustomAttributes = ""

			' SiteCategoryID
			If ew_NotEmpty(SiteLink.SiteCategoryID.CurrentValue) Then
				sSqlWrk = "SELECT [CategoryName] FROM [SiteCategory] WHERE [SiteCategoryID] = " & ew_AdjustSql(SiteLink.SiteCategoryID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [CategoryName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					SiteLink.SiteCategoryID.ViewValue = RsWrk("CategoryName")
				Else
					SiteLink.SiteCategoryID.ViewValue = SiteLink.SiteCategoryID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				SiteLink.SiteCategoryID.ViewValue = System.DBNull.Value
			End If
			SiteLink.SiteCategoryID.CssStyle = ""
			SiteLink.SiteCategoryID.CssClass = ""
			SiteLink.SiteCategoryID.ViewCustomAttributes = ""

			' SiteCategoryGroupID
			If ew_NotEmpty(SiteLink.SiteCategoryGroupID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteCategoryGroupNM] FROM [SiteCategoryGroup] WHERE [SiteCategoryGroupID] = " & ew_AdjustSql(SiteLink.SiteCategoryGroupID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryGroupOrder] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					SiteLink.SiteCategoryGroupID.ViewValue = RsWrk("SiteCategoryGroupNM")
				Else
					SiteLink.SiteCategoryGroupID.ViewValue = SiteLink.SiteCategoryGroupID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				SiteLink.SiteCategoryGroupID.ViewValue = System.DBNull.Value
			End If
			SiteLink.SiteCategoryGroupID.CssStyle = ""
			SiteLink.SiteCategoryGroupID.CssClass = ""
			SiteLink.SiteCategoryGroupID.ViewCustomAttributes = ""

			' Ranks
			SiteLink.Ranks.ViewValue = SiteLink.Ranks.CurrentValue
			SiteLink.Ranks.CssStyle = ""
			SiteLink.Ranks.CssClass = ""
			SiteLink.Ranks.ViewCustomAttributes = ""

			' Views
			If Convert.ToString(SiteLink.Views.CurrentValue) = "1" Then
				SiteLink.Views.ViewValue = "Yes"
			Else
				SiteLink.Views.ViewValue = "No"
			End If
			SiteLink.Views.CssStyle = ""
			SiteLink.Views.CssClass = ""
			SiteLink.Views.ViewCustomAttributes = ""

			' View refer script
			' SiteCategoryTypeID

			SiteLink.SiteCategoryTypeID.HrefValue = ""

			' Title
			SiteLink.Title.HrefValue = ""

			' LinkTypeCD
			SiteLink.LinkTypeCD.HrefValue = ""

			' CategoryID
			SiteLink.CategoryID.HrefValue = ""

			' CompanyID
			SiteLink.CompanyID.HrefValue = ""

			' SiteCategoryID
			SiteLink.SiteCategoryID.HrefValue = ""

			' SiteCategoryGroupID
			SiteLink.SiteCategoryGroupID.HrefValue = ""

			' Ranks
			SiteLink.Ranks.HrefValue = ""

			' Views
			SiteLink.Views.HrefValue = ""

		'
		'  Add Row
		'

		ElseIf SiteLink.RowType = EW_ROWTYPE_ADD Then ' Add row

			' SiteCategoryTypeID
			SiteLink.SiteCategoryTypeID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryTypeID], [SiteCategoryTypeNM], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteCategoryType]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryTypeNM] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			SiteLink.SiteCategoryTypeID.EditValue = arwrk

			' Title
			SiteLink.Title.EditCustomAttributes = ""
			SiteLink.Title.EditValue = ew_HtmlEncode(SiteLink.Title.CurrentValue)

			' LinkTypeCD
			SiteLink.LinkTypeCD.EditCustomAttributes = ""
			sSqlWrk = "SELECT [LinkTypeCD], [LinkTypeDesc], '' AS Disp2Fld, '' AS SelectFilterFld FROM [LinkType]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [LinkTypeDesc] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			SiteLink.LinkTypeCD.EditValue = arwrk

			' CategoryID
			SiteLink.CategoryID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [ID], [Title], '' AS Disp2Fld, '' AS SelectFilterFld FROM [LinkCategory]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [Title] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			SiteLink.CategoryID.EditValue = arwrk

			' CompanyID
			SiteLink.CompanyID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [CompanyID], [CompanyName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Company]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			SiteLink.CompanyID.EditValue = arwrk

			' SiteCategoryID
			SiteLink.SiteCategoryID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryID], [CategoryName], '' AS Disp2Fld, [SiteCategoryTypeID] FROM [SiteCategory]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [CategoryName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select", ""}) 
			SiteLink.SiteCategoryID.EditValue = arwrk

			' SiteCategoryGroupID
			SiteLink.SiteCategoryGroupID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryGroupID], [SiteCategoryGroupNM], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteCategoryGroup]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryGroupOrder] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			SiteLink.SiteCategoryGroupID.EditValue = arwrk

			' Ranks
			SiteLink.Ranks.EditCustomAttributes = ""
			SiteLink.Ranks.EditValue = ew_HtmlEncode(SiteLink.Ranks.CurrentValue)

			' Views
			SiteLink.Views.EditCustomAttributes = ""

		'
		'  Edit Row
		'

		ElseIf SiteLink.RowType = EW_ROWTYPE_EDIT Then ' Edit row

			' SiteCategoryTypeID
			SiteLink.SiteCategoryTypeID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryTypeID], [SiteCategoryTypeNM], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteCategoryType]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryTypeNM] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			SiteLink.SiteCategoryTypeID.EditValue = arwrk

			' Title
			SiteLink.Title.EditCustomAttributes = ""
			SiteLink.Title.EditValue = ew_HtmlEncode(SiteLink.Title.CurrentValue)

			' LinkTypeCD
			SiteLink.LinkTypeCD.EditCustomAttributes = ""
			sSqlWrk = "SELECT [LinkTypeCD], [LinkTypeDesc], '' AS Disp2Fld, '' AS SelectFilterFld FROM [LinkType]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [LinkTypeDesc] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			SiteLink.LinkTypeCD.EditValue = arwrk

			' CategoryID
			SiteLink.CategoryID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [ID], [Title], '' AS Disp2Fld, '' AS SelectFilterFld FROM [LinkCategory]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [Title] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			SiteLink.CategoryID.EditValue = arwrk

			' CompanyID
			SiteLink.CompanyID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [CompanyID], [CompanyName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Company]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			SiteLink.CompanyID.EditValue = arwrk

			' SiteCategoryID
			SiteLink.SiteCategoryID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryID], [CategoryName], '' AS Disp2Fld, [SiteCategoryTypeID] FROM [SiteCategory]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [CategoryName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select", ""}) 
			SiteLink.SiteCategoryID.EditValue = arwrk

			' SiteCategoryGroupID
			SiteLink.SiteCategoryGroupID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryGroupID], [SiteCategoryGroupNM], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteCategoryGroup]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryGroupOrder] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			SiteLink.SiteCategoryGroupID.EditValue = arwrk

			' Ranks
			SiteLink.Ranks.EditCustomAttributes = ""
			SiteLink.Ranks.EditValue = ew_HtmlEncode(SiteLink.Ranks.CurrentValue)

			' Views
			SiteLink.Views.EditCustomAttributes = ""

			' Edit refer script
			' SiteCategoryTypeID

			SiteLink.SiteCategoryTypeID.HrefValue = ""

			' Title
			SiteLink.Title.HrefValue = ""

			' LinkTypeCD
			SiteLink.LinkTypeCD.HrefValue = ""

			' CategoryID
			SiteLink.CategoryID.HrefValue = ""

			' CompanyID
			SiteLink.CompanyID.HrefValue = ""

			' SiteCategoryID
			SiteLink.SiteCategoryID.HrefValue = ""

			' SiteCategoryGroupID
			SiteLink.SiteCategoryGroupID.HrefValue = ""

			' Ranks
			SiteLink.Ranks.HrefValue = ""

			' Views
			SiteLink.Views.HrefValue = ""
		End If

		' Row Rendered event
		SiteLink.Row_Rendered()
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
		If ew_Empty(SiteLink.SiteCategoryTypeID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter required field - Site Type"
		End If
		If ew_Empty(SiteLink.Title.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter required field - Title"
		End If
		If ew_Empty(SiteLink.LinkTypeCD.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter required field - Link Type"
		End If
		If ew_Empty(SiteLink.CategoryID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter required field - Link Category"
		End If
		If Not ew_CheckInteger(SiteLink.Ranks.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Incorrect integer - Ranks"
		End If
		If ew_Empty(SiteLink.Views.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter required field - Active/Visible"
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
		sFilter = SiteLink.KeyFilter
		SiteLink.CurrentFilter  = sFilter
		sSql = SiteLink.SQL
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
			SiteLink.SiteCategoryTypeID.SetDbValue(SiteLink.SiteCategoryTypeID.CurrentValue, System.DBNull.Value)
			Rs("SiteCategoryTypeID") = SiteLink.SiteCategoryTypeID.DbValue

			' Title
			SiteLink.Title.SetDbValue(SiteLink.Title.CurrentValue, System.DBNull.Value)
			Rs("Title") = SiteLink.Title.DbValue

			' LinkTypeCD
			SiteLink.LinkTypeCD.SetDbValue(SiteLink.LinkTypeCD.CurrentValue, System.DBNull.Value)
			Rs("LinkTypeCD") = SiteLink.LinkTypeCD.DbValue

			' CategoryID
			SiteLink.CategoryID.SetDbValue(SiteLink.CategoryID.CurrentValue, System.DBNull.Value)
			Rs("CategoryID") = SiteLink.CategoryID.DbValue

			' CompanyID
			SiteLink.CompanyID.SetDbValue(SiteLink.CompanyID.CurrentValue, System.DBNull.Value)
			Rs("CompanyID") = SiteLink.CompanyID.DbValue

			' SiteCategoryID
			SiteLink.SiteCategoryID.SetDbValue(SiteLink.SiteCategoryID.CurrentValue, System.DBNull.Value)
			Rs("SiteCategoryID") = SiteLink.SiteCategoryID.DbValue

			' SiteCategoryGroupID
			SiteLink.SiteCategoryGroupID.SetDbValue(SiteLink.SiteCategoryGroupID.CurrentValue, System.DBNull.Value)
			Rs("SiteCategoryGroupID") = SiteLink.SiteCategoryGroupID.DbValue

			' Ranks
			SiteLink.Ranks.SetDbValue(SiteLink.Ranks.CurrentValue, System.DBNull.Value)
			Rs("Ranks") = SiteLink.Ranks.DbValue

			' Views
			SiteLink.Views.SetDbValue((SiteLink.Views.CurrentValue <> "" And Not IsDBNull(SiteLink.Views.CurrentValue)), False)
			Rs("Views") = SiteLink.Views.DbValue

			' Row Updating event
			bUpdateRow = SiteLink.Row_Updating(RsOld, Rs)
			If bUpdateRow Then
				Try
					SiteLink.Update(Rs)
					EditRow = True
				Catch e As Exception
					If EW_DEBUG_ENABLED Then Throw
					Message = e.Message
					EditRow = False
				End Try
			Else
				If SiteLink.CancelMessage <> "" Then
					Message = SiteLink.CancelMessage
					SiteLink.CancelMessage = ""
				Else
					Message = "Update cancelled"
				End If
				EditRow = False
			End If
		End If

		' Row Updated event
		If EditRow Then
			SiteLink.Row_Updated(RsOld, Rs)
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
		SiteLink.SiteCategoryTypeID.SetDbValue(SiteLink.SiteCategoryTypeID.CurrentValue, System.DBNull.Value)
		Rs("SiteCategoryTypeID") = SiteLink.SiteCategoryTypeID.DbValue

		' Title
		SiteLink.Title.SetDbValue(SiteLink.Title.CurrentValue, System.DBNull.Value)
		Rs("Title") = SiteLink.Title.DbValue

		' LinkTypeCD
		SiteLink.LinkTypeCD.SetDbValue(SiteLink.LinkTypeCD.CurrentValue, System.DBNull.Value)
		Rs("LinkTypeCD") = SiteLink.LinkTypeCD.DbValue

		' CategoryID
		SiteLink.CategoryID.SetDbValue(SiteLink.CategoryID.CurrentValue, System.DBNull.Value)
		Rs("CategoryID") = SiteLink.CategoryID.DbValue

		' CompanyID
		SiteLink.CompanyID.SetDbValue(SiteLink.CompanyID.CurrentValue, System.DBNull.Value)
		Rs("CompanyID") = SiteLink.CompanyID.DbValue

		' SiteCategoryID
		SiteLink.SiteCategoryID.SetDbValue(SiteLink.SiteCategoryID.CurrentValue, System.DBNull.Value)
		Rs("SiteCategoryID") = SiteLink.SiteCategoryID.DbValue

		' SiteCategoryGroupID
		SiteLink.SiteCategoryGroupID.SetDbValue(SiteLink.SiteCategoryGroupID.CurrentValue, System.DBNull.Value)
		Rs("SiteCategoryGroupID") = SiteLink.SiteCategoryGroupID.DbValue

		' Ranks
		SiteLink.Ranks.SetDbValue(SiteLink.Ranks.CurrentValue, System.DBNull.Value)
		Rs("Ranks") = SiteLink.Ranks.DbValue

		' Views
		SiteLink.Views.SetDbValue((SiteLink.Views.CurrentValue <> "" And Not IsDBNull(SiteLink.Views.CurrentValue)), False)
		Rs("Views") = SiteLink.Views.DbValue

		' Row Inserting event
		bInsertRow = SiteLink.Row_Inserting(Rs)
		If bInsertRow Then
			Try	
				SiteLink.Insert(Rs)
				AddRow = True
			Catch e As Exception
				If EW_DEBUG_ENABLED Then Throw
				Message = e.Message				
				AddRow = False
			End Try
		Else
			If SiteLink.CancelMessage <> "" Then
				Message = SiteLink.CancelMessage
				SiteLink.CancelMessage = ""
			Else
				Message = "Insert cancelled"
			End If
			AddRow = False
		End If
		If AddRow Then
			LastInsertId = Conn.GetLastInsertId()
			SiteLink.ID.DbValue = LastInsertId
			Rs("ID") = SiteLink.ID.DbValue		

			' Row Inserted event
			SiteLink.Row_Inserted(Rs)
			WriteAuditTrailOnAdd(Rs)
		End If
	End Function

	'
	' Load advanced search
	'
	Sub LoadAdvancedSearch()
		SiteLink.SiteCategoryTypeID.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_SiteCategoryTypeID")
		SiteLink.Title.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_Title")
		SiteLink.LinkTypeCD.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_LinkTypeCD")
		SiteLink.CategoryID.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_CategoryID")
		SiteLink.CompanyID.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_CompanyID")
		SiteLink.SiteCategoryID.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_SiteCategoryID")
		SiteLink.SiteCategoryGroupID.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_SiteCategoryGroupID")
		SiteLink.Ranks.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_Ranks")
		SiteLink.Views.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_Views")
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
		If SiteLink.ExportAll Then
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
		If SiteLink.Export = "xml" Then
			oXmlDoc = New XmlDocument()
			oXmlTbl = oXmlDoc.CreateElement("table")
			oXmlDoc.AppendChild(oXmlTbl)
		Else
			ew_Write(ew_ExportHeader(SiteLink.Export))

			' Horizontal format, write header
			If sExportStyle <> "v" OrElse SiteLink.Export = "csv" Then
				sExportStr = ""
				ew_ExportAddValue(sExportStr, "SiteCategoryTypeID", SiteLink.Export)
				ew_ExportAddValue(sExportStr, "Title", SiteLink.Export)
				ew_ExportAddValue(sExportStr, "LinkTypeCD", SiteLink.Export)
				ew_ExportAddValue(sExportStr, "CategoryID", SiteLink.Export)
				ew_ExportAddValue(sExportStr, "CompanyID", SiteLink.Export)
				ew_ExportAddValue(sExportStr, "SiteCategoryID", SiteLink.Export)
				ew_ExportAddValue(sExportStr, "SiteCategoryGroupID", SiteLink.Export)
				ew_ExportAddValue(sExportStr, "Ranks", SiteLink.Export)
				ew_ExportAddValue(sExportStr, "Views", SiteLink.Export)
				ew_Write(ew_ExportLine(sExportStr, SiteLink.Export))
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
				SiteLink.RowType = EW_ROWTYPE_VIEW ' Render view
				RenderRow()
				If SiteLink.Export = "xml" Then
					oXmlRec = oXmlDoc.CreateElement("record")
					oXmlTbl.AppendChild(oXmlRec)
					oXmlFld = oXmlDoc.CreateElement("SiteCategoryTypeID") ' SiteCategoryTypeID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(SiteLink.SiteCategoryTypeID.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("Title") ' Title
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(SiteLink.Title.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("LinkTypeCD") ' LinkTypeCD
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(SiteLink.LinkTypeCD.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("CategoryID") ' CategoryID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(SiteLink.CategoryID.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("CompanyID") ' CompanyID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(SiteLink.CompanyID.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("SiteCategoryID") ' SiteCategoryID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(SiteLink.SiteCategoryID.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("SiteCategoryGroupID") ' SiteCategoryGroupID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(SiteLink.SiteCategoryGroupID.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("Ranks") ' Ranks
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(SiteLink.Ranks.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("Views") ' Views
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(SiteLink.Views.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
				Else

					' Vertical format
					If sExportStyle = "v" AndAlso SiteLink.Export <> "csv" Then
						ew_Write(ew_ExportField("SiteCategoryTypeID", SiteLink.SiteCategoryTypeID.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue), SiteLink.Export)) ' SiteCategoryTypeID
						ew_Write(ew_ExportField("Title", SiteLink.Title.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue), SiteLink.Export)) ' Title
						ew_Write(ew_ExportField("LinkTypeCD", SiteLink.LinkTypeCD.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue), SiteLink.Export)) ' LinkTypeCD
						ew_Write(ew_ExportField("CategoryID", SiteLink.CategoryID.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue), SiteLink.Export)) ' CategoryID
						ew_Write(ew_ExportField("CompanyID", SiteLink.CompanyID.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue), SiteLink.Export)) ' CompanyID
						ew_Write(ew_ExportField("SiteCategoryID", SiteLink.SiteCategoryID.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue), SiteLink.Export)) ' SiteCategoryID
						ew_Write(ew_ExportField("SiteCategoryGroupID", SiteLink.SiteCategoryGroupID.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue), SiteLink.Export)) ' SiteCategoryGroupID
						ew_Write(ew_ExportField("Ranks", SiteLink.Ranks.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue), SiteLink.Export)) ' Ranks
						ew_Write(ew_ExportField("Views", SiteLink.Views.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue), SiteLink.Export)) ' Views

					' Horizontal format
					Else
						sExportStr = ""
						ew_ExportAddValue(sExportStr, SiteLink.SiteCategoryTypeID.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue), SiteLink.Export) ' SiteCategoryTypeID
						ew_ExportAddValue(sExportStr, SiteLink.Title.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue), SiteLink.Export) ' Title
						ew_ExportAddValue(sExportStr, SiteLink.LinkTypeCD.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue), SiteLink.Export) ' LinkTypeCD
						ew_ExportAddValue(sExportStr, SiteLink.CategoryID.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue), SiteLink.Export) ' CategoryID
						ew_ExportAddValue(sExportStr, SiteLink.CompanyID.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue), SiteLink.Export) ' CompanyID
						ew_ExportAddValue(sExportStr, SiteLink.SiteCategoryID.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue), SiteLink.Export) ' SiteCategoryID
						ew_ExportAddValue(sExportStr, SiteLink.SiteCategoryGroupID.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue), SiteLink.Export) ' SiteCategoryGroupID
						ew_ExportAddValue(sExportStr, SiteLink.Ranks.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue), SiteLink.Export) ' Ranks
						ew_ExportAddValue(sExportStr, SiteLink.Views.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue), SiteLink.Export) ' Views
						ew_Write(ew_ExportLine(sExportStr, SiteLink.Export))
					End If
				End If
			End If
		Loop

		' Close recordset
		Rs.Close()
		Rs.Dispose()
		If SiteLink.Export = "xml" Then
			ew_Write("<?xml version=""1.0"" standalone=""yes""?>" & vbcrlf)
			ew_Write(oXmlDoc.OuterXml)
		Else
			ew_Write(ew_ExportFooter(SiteLink.Export))
		End If
	End Sub

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "SiteLink"
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
		Dim table As String = "SiteLink"

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

		' SiteCategoryTypeID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteCategoryTypeID", keyvalue, oldvalue, RsSrc("SiteCategoryTypeID"))

		' Title Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "Title", keyvalue, oldvalue, RsSrc("Title"))

		' LinkTypeCD Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "LinkTypeCD", keyvalue, oldvalue, RsSrc("LinkTypeCD"))

		' CategoryID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "CategoryID", keyvalue, oldvalue, RsSrc("CategoryID"))

		' CompanyID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "CompanyID", keyvalue, oldvalue, RsSrc("CompanyID"))

		' SiteCategoryID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteCategoryID", keyvalue, oldvalue, RsSrc("SiteCategoryID"))

		' SiteCategoryGroupID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteCategoryGroupID", keyvalue, oldvalue, RsSrc("SiteCategoryGroupID"))

		' Ranks Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "Ranks", keyvalue, oldvalue, RsSrc("Ranks"))

		' Views Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "Views", keyvalue, oldvalue, RsSrc("Views"))
	End Sub

	' Write Audit Trail (edit page)
	Sub WriteAuditTrailOnEdit(ByRef RsOld As OrderedDictionary, ByRef RsNew As OrderedDictionary)
		Dim table As String = "SiteLink"

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
			fld = SiteLink.FieldByName(fldname)
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
		SiteLink_list = New cSiteLink_list(Me)		
		SiteLink_list.Page_Init()

		' Page main processing
		SiteLink_list.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If SiteLink_list IsNot Nothing Then SiteLink_list.Dispose()
	End Sub
End Class
