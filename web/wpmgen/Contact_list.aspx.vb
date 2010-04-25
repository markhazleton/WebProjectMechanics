Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class Contact_list
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public Contact_list As cContact_list

	'
	' Page Class
	'
	Class cContact_list
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
				If Contact.UseTokenInUrl Then Url = Url & "t=" & Contact.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If Contact.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (Contact.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (Contact.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' Contact
		Public Property Contact() As cContact
			Get				
				Return ParentPage.Contact
			End Get
			Set(ByVal v As cContact)
				ParentPage.Contact = v	
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
			m_PageObjName = "Contact_list"
			m_PageObjTypeName = "cContact_list"

			' Table Name
			m_TableName = "Contact"

			' Initialize table object
			Contact = New cContact(Me)

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
			Contact.Export = ew_Get("export") ' Get export parameter
			ParentPage.gsExport = Contact.Export ' Get export parameter, used in header
			ParentPage.gsExportFile = Contact.TableVar ' Get export file, used in header
			If Contact.Export = "excel" Then
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
			Contact.Dispose()
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
		If IsPageRequest Then ' Validate request

			' Set up records per page dynamically
			SetUpDisplayRecs()

			' Handle reset command
			ResetCmd()

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
		If (Contact.RecordsPerPage = -1 OrElse Contact.RecordsPerPage > 0) Then
			lDisplayRecs = Contact.RecordsPerPage ' Restore from Session
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
		Contact.Recordset_Searching(sSrchWhere)

		' Save search criteria
		If sSrchWhere <> "" Then
			If sSrchAdvanced = "" Then ResetAdvancedSearchParms()
			Contact.SearchWhere = sSrchWhere ' Save to Session
			lStartRec = 1 ' Reset start record counter
			Contact.StartRecordNumber = lStartRec
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
		Contact.SessionWhere = sFilter
		Contact.CurrentFilter = ""

		' Export Data only
		If Contact.Export = "html" OrElse Contact.Export = "csv" OrElse Contact.Export = "word" OrElse Contact.Export = "excel" OrElse Contact.Export = "xml" Then
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
			Contact.RecordsPerPage = lDisplayRecs ' Save to Session

			' Reset start position
			lStartRec = 1
			Contact.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	' Return Advanced Search WHERE based on QueryString parameters
	'
	Function AdvancedSearchWhere() As String		
		Dim sWhere As String = ""
		BuildSearchSql(sWhere, Contact.LogonName, False) ' LogonName
		BuildSearchSql(sWhere, Contact.PrimaryContact, False) ' PrimaryContact
		BuildSearchSql(sWhere, Contact.zEMail, False) ' EMail
		BuildSearchSql(sWhere, Contact.Active, False) ' Active
		BuildSearchSql(sWhere, Contact.CompanyID, False) ' CompanyID
		BuildSearchSql(sWhere, Contact.GroupID, False) ' GroupID
		BuildSearchSql(sWhere, Contact.TemplatePrefix, False) ' TemplatePrefix
		BuildSearchSql(sWhere, Contact.RoleID, False) ' RoleID

		' Set up search parm
		If sWhere <> "" Then
			SetSearchParm(Contact.LogonName) ' LogonName
			SetSearchParm(Contact.PrimaryContact) ' PrimaryContact
			SetSearchParm(Contact.zEMail) ' EMail
			SetSearchParm(Contact.Active) ' Active
			SetSearchParm(Contact.CompanyID) ' CompanyID
			SetSearchParm(Contact.GroupID) ' GroupID
			SetSearchParm(Contact.TemplatePrefix) ' TemplatePrefix
			SetSearchParm(Contact.RoleID) ' RoleID
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
		Contact.SetAdvancedSearch("x_" & FldParm, Fld.AdvancedSearch.SearchValue)
		Contact.SetAdvancedSearch("z_" & FldParm, Fld.AdvancedSearch.SearchOperator)
		Contact.SetAdvancedSearch("v_" & FldParm, Fld.AdvancedSearch.SearchCondition)
		Contact.SetAdvancedSearch("y_" & FldParm, Fld.AdvancedSearch.SearchValue2)
		Contact.SetAdvancedSearch("w_" & FldParm, Fld.AdvancedSearch.SearchOperator2)
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
		Contact.SearchWhere = sSrchWhere

		' Clear advanced search parameters
		ResetAdvancedSearchParms()
	End Sub

	'
	' Clear all advanced search parameters
	'
	Sub ResetAdvancedSearchParms()
		Contact.SetAdvancedSearch("x_LogonName", "")
		Contact.SetAdvancedSearch("x_PrimaryContact", "")
		Contact.SetAdvancedSearch("x_zEMail", "")
		Contact.SetAdvancedSearch("x_Active", "")
		Contact.SetAdvancedSearch("x_CompanyID", "")
		Contact.SetAdvancedSearch("x_GroupID", "")
		Contact.SetAdvancedSearch("x_TemplatePrefix", "")
		Contact.SetAdvancedSearch("x_RoleID", "")
	End Sub

	'
	' Restore all search parameters
	'
	Sub RestoreSearchParms()
		sSrchWhere = Contact.SearchWhere

		' Restore advanced search settings
		If ParentPage.gsSearchError = "" Then
			RestoreAdvancedSearchParms()
		End If
	End Sub

	'
	' Restore all advanced search parameters
	'
	Sub RestoreAdvancedSearchParms()
		Contact.LogonName.AdvancedSearch.SearchValue = Contact.GetAdvancedSearch("x_LogonName")
		Contact.PrimaryContact.AdvancedSearch.SearchValue = Contact.GetAdvancedSearch("x_PrimaryContact")
		Contact.zEMail.AdvancedSearch.SearchValue = Contact.GetAdvancedSearch("x_zEMail")
		Contact.Active.AdvancedSearch.SearchValue = Contact.GetAdvancedSearch("x_Active")
		Contact.CompanyID.AdvancedSearch.SearchValue = Contact.GetAdvancedSearch("x_CompanyID")
		Contact.GroupID.AdvancedSearch.SearchValue = Contact.GetAdvancedSearch("x_GroupID")
		Contact.TemplatePrefix.AdvancedSearch.SearchValue = Contact.GetAdvancedSearch("x_TemplatePrefix")
		Contact.RoleID.AdvancedSearch.SearchValue = Contact.GetAdvancedSearch("x_RoleID")
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
			Contact.CurrentOrder = ew_Get("order")
			Contact.CurrentOrderType = ew_Get("ordertype")
			Contact.UpdateSort(Contact.LogonName) ' LogonName
			Contact.UpdateSort(Contact.PrimaryContact) ' PrimaryContact
			Contact.UpdateSort(Contact.Active) ' Active
			Contact.UpdateSort(Contact.CompanyID) ' CompanyID
			Contact.UpdateSort(Contact.GroupID) ' GroupID
			Contact.StartRecordNumber = 1 ' Reset start position
		End If
	End Sub

	'
	' Load Sort Order parameters
	'
	Sub LoadSortOrder()
		Dim sOrderBy As String
		sOrderBy = Contact.SessionOrderBy ' Get order by from Session
		If sOrderBy = "" Then
			If Contact.SqlOrderBy <> "" Then
				sOrderBy = Contact.SqlOrderBy
				Contact.SessionOrderBy = sOrderBy
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
				Contact.SessionOrderBy = sOrderBy
				Contact.LogonName.Sort = ""
				Contact.PrimaryContact.Sort = ""
				Contact.Active.Sort = ""
				Contact.CompanyID.Sort = ""
				Contact.GroupID.Sort = ""
			End If

			' Reset start position
			lStartRec = 1
			Contact.StartRecordNumber = lStartRec
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
				Contact.StartRecordNumber = lStartRec
			ElseIf ew_Get(EW_TABLE_PAGE_NO) <> "" AndAlso IsNumeric(ew_Get(EW_TABLE_PAGE_NO)) Then
				nPageNo = ew_ConvertToInt(ew_Get(EW_TABLE_PAGE_NO))
				lStartRec = (nPageNo-1)*lDisplayRecs+1
				If lStartRec <= 0 Then
					lStartRec = 1
				ElseIf lStartRec >= ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 Then
					lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1
				End If
				Contact.StartRecordNumber = lStartRec
			End If
		End If
		lStartRec = Contact.StartRecordNumber

		' Check if correct start record counter
		If lStartRec <= 0 Then ' Avoid invalid start record counter
			lStartRec = 1 ' Reset start record counter
			Contact.StartRecordNumber = lStartRec
		ElseIf lStartRec > lTotalRecs Then ' Avoid starting record > total records
			lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to last page first record
			Contact.StartRecordNumber = lStartRec
		ElseIf (lStartRec-1) Mod lDisplayRecs <> 0 Then
			lStartRec = ((lStartRec-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to page boundary
			Contact.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	' Load default values
	'
	Sub LoadDefaultValues()
		Contact.Active.CurrentValue = 1
		Contact.CompanyID.CurrentValue = 0
		Contact.GroupID.CurrentValue = 0
	End Sub

	'
	'  Load search values for validation
	'
	Sub LoadSearchValues()
		Contact.LogonName.AdvancedSearch.SearchValue = ew_Get("x_LogonName")
    	Contact.LogonName.AdvancedSearch.SearchOperator = ew_Get("z_LogonName")
		Contact.PrimaryContact.AdvancedSearch.SearchValue = ew_Get("x_PrimaryContact")
    	Contact.PrimaryContact.AdvancedSearch.SearchOperator = ew_Get("z_PrimaryContact")
		Contact.zEMail.AdvancedSearch.SearchValue = ew_Get("x_zEMail")
    	Contact.zEMail.AdvancedSearch.SearchOperator = ew_Get("z_zEMail")
		Contact.Active.AdvancedSearch.SearchValue = ew_Get("x_Active")
    	Contact.Active.AdvancedSearch.SearchOperator = ew_Get("z_Active")
		Contact.CompanyID.AdvancedSearch.SearchValue = ew_Get("x_CompanyID")
    	Contact.CompanyID.AdvancedSearch.SearchOperator = ew_Get("z_CompanyID")
		Contact.GroupID.AdvancedSearch.SearchValue = ew_Get("x_GroupID")
    	Contact.GroupID.AdvancedSearch.SearchOperator = ew_Get("z_GroupID")
		Contact.TemplatePrefix.AdvancedSearch.SearchValue = ew_Get("x_TemplatePrefix")
    	Contact.TemplatePrefix.AdvancedSearch.SearchOperator = ew_Get("z_TemplatePrefix")
		Contact.RoleID.AdvancedSearch.SearchValue = ew_Get("x_RoleID")
    	Contact.RoleID.AdvancedSearch.SearchOperator = ew_Get("z_RoleID")
	End Sub

	'
	' Load recordset
	'
	Function LoadRecordset() As OleDbDataReader

		' Recordset Selecting event
		Contact.Recordset_Selecting(Contact.CurrentFilter)

		' Load list page SQL
		Dim sSql As String = Contact.ListSQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)

		' Count
		lTotalRecs = -1		
		Try			
			If sSql.StartsWith("SELECT * FROM ", StringComparison.InvariantCultureIgnoreCase) AndAlso _				
				ew_Empty(Contact.SqlGroupBy) AndAlso _
				ew_Empty(Contact.SqlHaving) Then
				Dim sCntSql As String = Contact.SelectCountSQL

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
		Contact.Recordset_Selected(Rs)
		Return Rs
	End Function

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = Contact.KeyFilter

		' Row Selecting event
		Contact.Row_Selecting(sFilter)

		' Load SQL based on filter
		Contact.CurrentFilter = sFilter
		Dim sSql As String = Contact.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				Contact.Row_Selected(RsRow)
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
		Contact.ContactID.DbValue = RsRow("ContactID")
		Contact.LogonName.DbValue = RsRow("LogonName")
		Contact.LogonPassword.DbValue = RsRow("LogonPassword")
		Contact.PrimaryContact.DbValue = RsRow("PrimaryContact")
		Contact.zEMail.DbValue = RsRow("EMail")
		Contact.Active.DbValue = IIf(ew_ConvertToBool(RsRow("Active")), "1", "0")
		Contact.CompanyID.DbValue = RsRow("CompanyID")
		Contact.GroupID.DbValue = RsRow("GroupID")
		Contact.TemplatePrefix.DbValue = RsRow("TemplatePrefix")
		Contact.RoleID.DbValue = RsRow("RoleID")
		Contact.FirstName.DbValue = RsRow("FirstName")
		Contact.MiddleInitial.DbValue = RsRow("MiddleInitial")
		Contact.LastName.DbValue = RsRow("LastName")
		Contact.OfficePhone.DbValue = RsRow("OfficePhone")
		Contact.HomePhone.DbValue = RsRow("HomePhone")
		Contact.MobilPhone.DbValue = RsRow("MobilPhone")
		Contact.Pager.DbValue = RsRow("Pager")
		Contact.URL.DbValue = RsRow("URL")
		Contact.Address1.DbValue = RsRow("Address1")
		Contact.Address2.DbValue = RsRow("Address2")
		Contact.City.DbValue = RsRow("City")
		Contact.State.DbValue = RsRow("State")
		Contact.Country.DbValue = RsRow("Country")
		Contact.PostalCode.DbValue = RsRow("PostalCode")
		Contact.Biography.DbValue = RsRow("Biography")
		Contact.CreateDT.DbValue = RsRow("CreateDT")
		Contact.Paid.DbValue = RsRow("Paid")
		Contact.email_subscribe.DbValue = IIf(ew_ConvertToBool(RsRow("email_subscribe")), "1", "0")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		Contact.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' LogonName

		Contact.LogonName.CellCssStyle = "white-space: nowrap;"
		Contact.LogonName.CellCssClass = ""

		' PrimaryContact
		Contact.PrimaryContact.CellCssStyle = "white-space: nowrap;"
		Contact.PrimaryContact.CellCssClass = ""

		' Active
		Contact.Active.CellCssStyle = "white-space: nowrap;"
		Contact.Active.CellCssClass = ""

		' CompanyID
		Contact.CompanyID.CellCssStyle = "white-space: nowrap;"
		Contact.CompanyID.CellCssClass = ""

		' GroupID
		Contact.GroupID.CellCssStyle = "white-space: nowrap;"
		Contact.GroupID.CellCssClass = ""

		'
		'  View  Row
		'

		If Contact.RowType = EW_ROWTYPE_VIEW Then ' View row

			' LogonName
			Contact.LogonName.ViewValue = Contact.LogonName.CurrentValue
			Contact.LogonName.CssStyle = ""
			Contact.LogonName.CssClass = ""
			Contact.LogonName.ViewCustomAttributes = ""

			' LogonPassword
			Contact.LogonPassword.ViewValue = "********"
			Contact.LogonPassword.CssStyle = ""
			Contact.LogonPassword.CssClass = ""
			Contact.LogonPassword.ViewCustomAttributes = ""

			' PrimaryContact
			Contact.PrimaryContact.ViewValue = Contact.PrimaryContact.CurrentValue
			Contact.PrimaryContact.CssStyle = ""
			Contact.PrimaryContact.CssClass = ""
			Contact.PrimaryContact.ViewCustomAttributes = ""

			' EMail
			Contact.zEMail.ViewValue = Contact.zEMail.CurrentValue
			Contact.zEMail.CssStyle = ""
			Contact.zEMail.CssClass = ""
			Contact.zEMail.ViewCustomAttributes = ""

			' Active
			If Convert.ToString(Contact.Active.CurrentValue) = "1" Then
				Contact.Active.ViewValue = "Yes"
			Else
				Contact.Active.ViewValue = "No"
			End If
			Contact.Active.CssStyle = ""
			Contact.Active.CssClass = ""
			Contact.Active.ViewCustomAttributes = ""

			' CompanyID
			If ew_NotEmpty(Contact.CompanyID.CurrentValue) Then
				sSqlWrk = "SELECT [CompanyName] FROM [Company] WHERE [CompanyID] = " & ew_AdjustSql(Contact.CompanyID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Contact.CompanyID.ViewValue = RsWrk("CompanyName")
				Else
					Contact.CompanyID.ViewValue = Contact.CompanyID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Contact.CompanyID.ViewValue = System.DBNull.Value
			End If
			Contact.CompanyID.CssStyle = ""
			Contact.CompanyID.CssClass = ""
			Contact.CompanyID.ViewCustomAttributes = ""

			' GroupID
			If ew_NotEmpty(Contact.GroupID.CurrentValue) Then
				sSqlWrk = "SELECT [GroupName] FROM [Group] WHERE [GroupID] = " & ew_AdjustSql(Contact.GroupID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [GroupID] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Contact.GroupID.ViewValue = RsWrk("GroupName")
				Else
					Contact.GroupID.ViewValue = Contact.GroupID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Contact.GroupID.ViewValue = System.DBNull.Value
			End If
			Contact.GroupID.CssStyle = ""
			Contact.GroupID.CssClass = ""
			Contact.GroupID.ViewCustomAttributes = ""

			' View refer script
			' LogonName

			Contact.LogonName.HrefValue = ""

			' PrimaryContact
			Contact.PrimaryContact.HrefValue = ""

			' Active
			Contact.Active.HrefValue = ""

			' CompanyID
			Contact.CompanyID.HrefValue = ""

			' GroupID
			Contact.GroupID.HrefValue = ""
		End If

		' Row Rendered event
		Contact.Row_Rendered()
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
	' Load advanced search
	'
	Sub LoadAdvancedSearch()
		Contact.LogonName.AdvancedSearch.SearchValue = Contact.GetAdvancedSearch("x_LogonName")
		Contact.PrimaryContact.AdvancedSearch.SearchValue = Contact.GetAdvancedSearch("x_PrimaryContact")
		Contact.zEMail.AdvancedSearch.SearchValue = Contact.GetAdvancedSearch("x_zEMail")
		Contact.Active.AdvancedSearch.SearchValue = Contact.GetAdvancedSearch("x_Active")
		Contact.CompanyID.AdvancedSearch.SearchValue = Contact.GetAdvancedSearch("x_CompanyID")
		Contact.GroupID.AdvancedSearch.SearchValue = Contact.GetAdvancedSearch("x_GroupID")
		Contact.TemplatePrefix.AdvancedSearch.SearchValue = Contact.GetAdvancedSearch("x_TemplatePrefix")
		Contact.RoleID.AdvancedSearch.SearchValue = Contact.GetAdvancedSearch("x_RoleID")
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
		If Contact.ExportAll Then
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
		If Contact.Export = "xml" Then
			oXmlDoc = New XmlDocument()
			oXmlTbl = oXmlDoc.CreateElement("table")
			oXmlDoc.AppendChild(oXmlTbl)
		Else
			ew_Write(ew_ExportHeader(Contact.Export))

			' Horizontal format, write header
			If sExportStyle <> "v" OrElse Contact.Export = "csv" Then
				sExportStr = ""
				ew_ExportAddValue(sExportStr, "LogonName", Contact.Export)
				ew_ExportAddValue(sExportStr, "LogonPassword", Contact.Export)
				ew_ExportAddValue(sExportStr, "PrimaryContact", Contact.Export)
				ew_ExportAddValue(sExportStr, "EMail", Contact.Export)
				ew_ExportAddValue(sExportStr, "Active", Contact.Export)
				ew_ExportAddValue(sExportStr, "CompanyID", Contact.Export)
				ew_ExportAddValue(sExportStr, "GroupID", Contact.Export)
				ew_Write(ew_ExportLine(sExportStr, Contact.Export))
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
				Contact.RowType = EW_ROWTYPE_VIEW ' Render view
				RenderRow()
				If Contact.Export = "xml" Then
					oXmlRec = oXmlDoc.CreateElement("record")
					oXmlTbl.AppendChild(oXmlRec)
					oXmlFld = oXmlDoc.CreateElement("LogonName") ' LogonName
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Contact.LogonName.ExportValue(Contact.Export, Contact.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("LogonPassword") ' LogonPassword
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Contact.LogonPassword.ExportValue(Contact.Export, Contact.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("PrimaryContact") ' PrimaryContact
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Contact.PrimaryContact.ExportValue(Contact.Export, Contact.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("zEMail") ' EMail
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Contact.zEMail.ExportValue(Contact.Export, Contact.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("Active") ' Active
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Contact.Active.ExportValue(Contact.Export, Contact.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("CompanyID") ' CompanyID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Contact.CompanyID.ExportValue(Contact.Export, Contact.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("GroupID") ' GroupID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Contact.GroupID.ExportValue(Contact.Export, Contact.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
				Else

					' Vertical format
					If sExportStyle = "v" AndAlso Contact.Export <> "csv" Then
						ew_Write(ew_ExportField("LogonName", Contact.LogonName.ExportValue(Contact.Export, Contact.ExportOriginalValue), Contact.Export)) ' LogonName
						ew_Write(ew_ExportField("LogonPassword", Contact.LogonPassword.ExportValue(Contact.Export, Contact.ExportOriginalValue), Contact.Export)) ' LogonPassword
						ew_Write(ew_ExportField("PrimaryContact", Contact.PrimaryContact.ExportValue(Contact.Export, Contact.ExportOriginalValue), Contact.Export)) ' PrimaryContact
						ew_Write(ew_ExportField("EMail", Contact.zEMail.ExportValue(Contact.Export, Contact.ExportOriginalValue), Contact.Export)) ' EMail
						ew_Write(ew_ExportField("Active", Contact.Active.ExportValue(Contact.Export, Contact.ExportOriginalValue), Contact.Export)) ' Active
						ew_Write(ew_ExportField("CompanyID", Contact.CompanyID.ExportValue(Contact.Export, Contact.ExportOriginalValue), Contact.Export)) ' CompanyID
						ew_Write(ew_ExportField("GroupID", Contact.GroupID.ExportValue(Contact.Export, Contact.ExportOriginalValue), Contact.Export)) ' GroupID

					' Horizontal format
					Else
						sExportStr = ""
						ew_ExportAddValue(sExportStr, Contact.LogonName.ExportValue(Contact.Export, Contact.ExportOriginalValue), Contact.Export) ' LogonName
						ew_ExportAddValue(sExportStr, Contact.LogonPassword.ExportValue(Contact.Export, Contact.ExportOriginalValue), Contact.Export) ' LogonPassword
						ew_ExportAddValue(sExportStr, Contact.PrimaryContact.ExportValue(Contact.Export, Contact.ExportOriginalValue), Contact.Export) ' PrimaryContact
						ew_ExportAddValue(sExportStr, Contact.zEMail.ExportValue(Contact.Export, Contact.ExportOriginalValue), Contact.Export) ' EMail
						ew_ExportAddValue(sExportStr, Contact.Active.ExportValue(Contact.Export, Contact.ExportOriginalValue), Contact.Export) ' Active
						ew_ExportAddValue(sExportStr, Contact.CompanyID.ExportValue(Contact.Export, Contact.ExportOriginalValue), Contact.Export) ' CompanyID
						ew_ExportAddValue(sExportStr, Contact.GroupID.ExportValue(Contact.Export, Contact.ExportOriginalValue), Contact.Export) ' GroupID
						ew_Write(ew_ExportLine(sExportStr, Contact.Export))
					End If
				End If
			End If
		Loop

		' Close recordset
		Rs.Close()
		Rs.Dispose()
		If Contact.Export = "xml" Then
			ew_Write("<?xml version=""1.0"" standalone=""yes""?>" & vbcrlf)
			ew_Write(oXmlDoc.OuterXml)
		Else
			ew_Write(ew_ExportFooter(Contact.Export))
		End If
	End Sub

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "Contact"
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
		Contact_list = New cContact_list(Me)		
		Contact_list.Page_Init()

		' Page main processing
		Contact_list.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If Contact_list IsNot Nothing Then Contact_list.Dispose()
	End Sub
End Class
