Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class Company_list
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public Company_list As cCompany_list

	'
	' Page Class
	'
	Class cCompany_list
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
				If Company.UseTokenInUrl Then Url = Url & "t=" & Company.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If Company.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (Company.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (Company.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' Company
		Public Property Company() As cCompany
			Get				
				Return ParentPage.Company
			End Get
			Set(ByVal v As cCompany)
				ParentPage.Company = v	
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
			m_PageObjName = "Company_list"
			m_PageObjTypeName = "cCompany_list"

			' Table Name
			m_TableName = "Company"

			' Initialize table object
			Company = New cCompany(Me)

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
			Company.Export = ew_Get("export") ' Get export parameter
			ParentPage.gsExport = Company.Export ' Get export parameter, used in header
			ParentPage.gsExportFile = Company.TableVar ' Get export file, used in header
			If Company.Export = "excel" Then
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
			Company.Dispose()
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

			' Get basic search criteria
			If ParentPage.gsSearchError = "" Then
				sSrchBasic = BasicSearchWhere()
			End If

			' Set Up Sorting Order
			SetUpSortOrder()
		End If

		' Restore display records
		If (Company.RecordsPerPage = -1 OrElse Company.RecordsPerPage > 0) Then
			lDisplayRecs = Company.RecordsPerPage ' Restore from Session
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
		Company.Recordset_Searching(sSrchWhere)

		' Save search criteria
		If sSrchWhere <> "" Then
			If sSrchBasic = "" Then ResetBasicSearchParms()
			Company.SearchWhere = sSrchWhere ' Save to Session
			lStartRec = 1 ' Reset start record counter
			Company.StartRecordNumber = lStartRec
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
		Company.SessionWhere = sFilter
		Company.CurrentFilter = ""

		' Export Data only
		If Company.Export = "html" OrElse Company.Export = "csv" OrElse Company.Export = "word" OrElse Company.Export = "excel" OrElse Company.Export = "xml" Then
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
			Company.RecordsPerPage = lDisplayRecs ' Save to Session

			' Reset start position
			lStartRec = 1
			Company.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	' Return Basic Search SQL
	'
	Function BasicSearchSQL(Keyword As String) As String
		Dim sKeyword As String, sSql As String = ""
		sKeyword = ew_AdjustSql(Keyword)		
		sSql = sSql & "[CompanyName] LIKE '%" & sKeyword & "%' OR "
		sSql = sSql & "[SiteTitle] LIKE '%" & sKeyword & "%' OR "
		sSql = sSql & "[SiteURL] LIKE '%" & sKeyword & "%' OR "
		sSql = sSql & "[GalleryFolder] LIKE '%" & sKeyword & "%' OR "
		sSql = sSql & "[SiteTemplate] LIKE '%" & sKeyword & "%' OR "
		sSql = sSql & "[DefaultSiteTemplate] LIKE '%" & sKeyword & "%' OR "
		sSql = sSql & "[Address] LIKE '%" & sKeyword & "%' OR "
		sSql = sSql & "[City] LIKE '%" & sKeyword & "%' OR "
		sSql = sSql & "[StateOrProvince] LIKE '%" & sKeyword & "%' OR "
		sSql = sSql & "[PostalCode] LIKE '%" & sKeyword & "%' OR "
		sSql = sSql & "[Country] LIKE '%" & sKeyword & "%' OR "
		sSql = sSql & "[PhoneNumber] LIKE '%" & sKeyword & "%' OR "
		sSql = sSql & "[FaxNumber] LIKE '%" & sKeyword & "%' OR "
		sSql = sSql & "[DefaultPaymentTerms] LIKE '%" & sKeyword & "%' OR "
		sSql = sSql & "[DefaultInvoiceDescription] LIKE '%" & sKeyword & "%' OR "
		sSql = sSql & "[Component] LIKE '%" & sKeyword & "%' OR "
		sSql = sSql & "[FromEmail] LIKE '%" & sKeyword & "%' OR "
		sSql = sSql & "[SMTP] LIKE '%" & sKeyword & "%' OR "
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
			Company.BasicSearchKeyword = sSearchKeyword
			Company.BasicSearchType = sSearchType
		End If
		Return sSearchStr
	End Function

	'
	' Clear all search parameters
	'
	Sub ResetSearchParms()

		' Clear search where
		sSrchWhere = ""
		Company.SearchWhere = sSrchWhere

		' Clear basic search parameters
		ResetBasicSearchParms()
	End Sub

	'
	' Clear all basic search parameters
	'
	Sub ResetBasicSearchParms()

		' Clear basic search parameters
		Company.BasicSearchKeyword = ""
		Company.BasicSearchType = ""
	End Sub

	'
	' Restore all search parameters
	'
	Sub RestoreSearchParms()
		sSrchWhere = Company.SearchWhere
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
			Company.CurrentOrder = ew_Get("order")
			Company.CurrentOrderType = ew_Get("ordertype")
			Company.UpdateSort(Company.CompanyName) ' CompanyName
			Company.UpdateSort(Company.SiteTitle) ' SiteTitle
			Company.UpdateSort(Company.SiteURL) ' SiteURL
			Company.UpdateSort(Company.SiteTemplate) ' SiteTemplate
			Company.UpdateSort(Company.DefaultSiteTemplate) ' DefaultSiteTemplate
			Company.UpdateSort(Company.ActiveFL) ' ActiveFL
			Company.StartRecordNumber = 1 ' Reset start position
		End If
	End Sub

	'
	' Load Sort Order parameters
	'
	Sub LoadSortOrder()
		Dim sOrderBy As String
		sOrderBy = Company.SessionOrderBy ' Get order by from Session
		If sOrderBy = "" Then
			If Company.SqlOrderBy <> "" Then
				sOrderBy = Company.SqlOrderBy
				Company.SessionOrderBy = sOrderBy
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
				Company.SessionOrderBy = sOrderBy
				Company.CompanyName.Sort = ""
				Company.SiteTitle.Sort = ""
				Company.SiteURL.Sort = ""
				Company.SiteTemplate.Sort = ""
				Company.DefaultSiteTemplate.Sort = ""
				Company.ActiveFL.Sort = ""
			End If

			' Reset start position
			lStartRec = 1
			Company.StartRecordNumber = lStartRec
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
				Company.StartRecordNumber = lStartRec
			ElseIf ew_Get(EW_TABLE_PAGE_NO) <> "" AndAlso IsNumeric(ew_Get(EW_TABLE_PAGE_NO)) Then
				nPageNo = ew_ConvertToInt(ew_Get(EW_TABLE_PAGE_NO))
				lStartRec = (nPageNo-1)*lDisplayRecs+1
				If lStartRec <= 0 Then
					lStartRec = 1
				ElseIf lStartRec >= ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 Then
					lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1
				End If
				Company.StartRecordNumber = lStartRec
			End If
		End If
		lStartRec = Company.StartRecordNumber

		' Check if correct start record counter
		If lStartRec <= 0 Then ' Avoid invalid start record counter
			lStartRec = 1 ' Reset start record counter
			Company.StartRecordNumber = lStartRec
		ElseIf lStartRec > lTotalRecs Then ' Avoid starting record > total records
			lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to last page first record
			Company.StartRecordNumber = lStartRec
		ElseIf (lStartRec-1) Mod lDisplayRecs <> 0 Then
			lStartRec = ((lStartRec-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to page boundary
			Company.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	' Load default values
	'
	Sub LoadDefaultValues()
	End Sub

	'
	' Load recordset
	'
	Function LoadRecordset() As OleDbDataReader

		' Recordset Selecting event
		Company.Recordset_Selecting(Company.CurrentFilter)

		' Load list page SQL
		Dim sSql As String = Company.ListSQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)

		' Count
		lTotalRecs = -1		
		Try			
			If sSql.StartsWith("SELECT * FROM ", StringComparison.InvariantCultureIgnoreCase) AndAlso _				
				ew_Empty(Company.SqlGroupBy) AndAlso _
				ew_Empty(Company.SqlHaving) Then
				Dim sCntSql As String = Company.SelectCountSQL

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
		Company.Recordset_Selected(Rs)
		Return Rs
	End Function

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = Company.KeyFilter

		' Row Selecting event
		Company.Row_Selecting(sFilter)

		' Load SQL based on filter
		Company.CurrentFilter = sFilter
		Dim sSql As String = Company.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				Company.Row_Selected(RsRow)
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
		Company.CompanyID.DbValue = RsRow("CompanyID")
		Company.CompanyName.DbValue = RsRow("CompanyName")
		Company.SiteTitle.DbValue = RsRow("SiteTitle")
		Company.SiteURL.DbValue = RsRow("SiteURL")
		Company.GalleryFolder.DbValue = RsRow("GalleryFolder")
		Company.SiteCategoryTypeID.DbValue = RsRow("SiteCategoryTypeID")
		Company.HomePageID.DbValue = RsRow("HomePageID")
		Company.DefaultArticleID.DbValue = RsRow("DefaultArticleID")
		Company.SiteTemplate.DbValue = RsRow("SiteTemplate")
		Company.DefaultSiteTemplate.DbValue = RsRow("DefaultSiteTemplate")
		Company.UseBreadCrumbURL.DbValue = IIf(ew_ConvertToBool(RsRow("UseBreadCrumbURL")), "1", "0")
		Company.SingleSiteGallery.DbValue = IIf(ew_ConvertToBool(RsRow("SingleSiteGallery")), "1", "0")
		Company.ActiveFL.DbValue = IIf(ew_ConvertToBool(RsRow("ActiveFL")), "1", "0")
		Company.Address.DbValue = RsRow("Address")
		Company.City.DbValue = RsRow("City")
		Company.StateOrProvince.DbValue = RsRow("StateOrProvince")
		Company.PostalCode.DbValue = RsRow("PostalCode")
		Company.Country.DbValue = RsRow("Country")
		Company.PhoneNumber.DbValue = RsRow("PhoneNumber")
		Company.FaxNumber.DbValue = RsRow("FaxNumber")
		Company.DefaultPaymentTerms.DbValue = RsRow("DefaultPaymentTerms")
		Company.DefaultInvoiceDescription.DbValue = RsRow("DefaultInvoiceDescription")
		Company.Component.DbValue = RsRow("Component")
		Company.FromEmail.DbValue = RsRow("FromEmail")
		Company.SMTP.DbValue = RsRow("SMTP")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		Company.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' CompanyName

		Company.CompanyName.CellCssStyle = ""
		Company.CompanyName.CellCssClass = ""

		' SiteTitle
		Company.SiteTitle.CellCssStyle = ""
		Company.SiteTitle.CellCssClass = ""

		' SiteURL
		Company.SiteURL.CellCssStyle = ""
		Company.SiteURL.CellCssClass = ""

		' SiteTemplate
		Company.SiteTemplate.CellCssStyle = ""
		Company.SiteTemplate.CellCssClass = ""

		' DefaultSiteTemplate
		Company.DefaultSiteTemplate.CellCssStyle = ""
		Company.DefaultSiteTemplate.CellCssClass = ""

		' ActiveFL
		Company.ActiveFL.CellCssStyle = ""
		Company.ActiveFL.CellCssClass = ""

		'
		'  View  Row
		'

		If Company.RowType = EW_ROWTYPE_VIEW Then ' View row

			' CompanyID
			Company.CompanyID.ViewValue = Company.CompanyID.CurrentValue
			Company.CompanyID.CssStyle = ""
			Company.CompanyID.CssClass = ""
			Company.CompanyID.ViewCustomAttributes = ""

			' CompanyName
			Company.CompanyName.ViewValue = Company.CompanyName.CurrentValue
			Company.CompanyName.CssStyle = ""
			Company.CompanyName.CssClass = ""
			Company.CompanyName.ViewCustomAttributes = ""

			' SiteTitle
			Company.SiteTitle.ViewValue = Company.SiteTitle.CurrentValue
			Company.SiteTitle.CssStyle = ""
			Company.SiteTitle.CssClass = ""
			Company.SiteTitle.ViewCustomAttributes = ""

			' SiteURL
			Company.SiteURL.ViewValue = Company.SiteURL.CurrentValue
			Company.SiteURL.CssStyle = ""
			Company.SiteURL.CssClass = ""
			Company.SiteURL.ViewCustomAttributes = ""

			' GalleryFolder
			Company.GalleryFolder.ViewValue = Company.GalleryFolder.CurrentValue
			Company.GalleryFolder.CssStyle = ""
			Company.GalleryFolder.CssClass = ""
			Company.GalleryFolder.ViewCustomAttributes = ""

			' SiteCategoryTypeID
			If ew_NotEmpty(Company.SiteCategoryTypeID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteCategoryTypeNM] FROM [SiteCategoryType] WHERE [SiteCategoryTypeID] = " & ew_AdjustSql(Company.SiteCategoryTypeID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryTypeNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Company.SiteCategoryTypeID.ViewValue = RsWrk("SiteCategoryTypeNM")
				Else
					Company.SiteCategoryTypeID.ViewValue = Company.SiteCategoryTypeID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Company.SiteCategoryTypeID.ViewValue = System.DBNull.Value
			End If
			Company.SiteCategoryTypeID.CssStyle = ""
			Company.SiteCategoryTypeID.CssClass = ""
			Company.SiteCategoryTypeID.ViewCustomAttributes = ""

			' HomePageID
			If ew_NotEmpty(Company.HomePageID.CurrentValue) Then
				sSqlWrk = "SELECT [PageName] FROM [Page] WHERE [PageID] = " & ew_AdjustSql(Company.HomePageID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [PageName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Company.HomePageID.ViewValue = RsWrk("PageName")
				Else
					Company.HomePageID.ViewValue = Company.HomePageID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Company.HomePageID.ViewValue = System.DBNull.Value
			End If
			Company.HomePageID.CssStyle = ""
			Company.HomePageID.CssClass = ""
			Company.HomePageID.ViewCustomAttributes = ""

			' DefaultArticleID
			If ew_NotEmpty(Company.DefaultArticleID.CurrentValue) Then
				sSqlWrk = "SELECT [Title] FROM [Article] WHERE [ArticleID] = " & ew_AdjustSql(Company.DefaultArticleID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [Title] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Company.DefaultArticleID.ViewValue = RsWrk("Title")
				Else
					Company.DefaultArticleID.ViewValue = Company.DefaultArticleID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Company.DefaultArticleID.ViewValue = System.DBNull.Value
			End If
			Company.DefaultArticleID.CssStyle = ""
			Company.DefaultArticleID.CssClass = ""
			Company.DefaultArticleID.ViewCustomAttributes = ""

			' SiteTemplate
			If ew_NotEmpty(Company.SiteTemplate.CurrentValue) Then
				sSqlWrk = "SELECT [Name] FROM [SiteTemplate] WHERE [TemplatePrefix] = '" & ew_AdjustSql(Company.SiteTemplate.CurrentValue) & "'"
				sSqlWrk = sSqlWrk & " ORDER BY [Name] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Company.SiteTemplate.ViewValue = RsWrk("Name")
				Else
					Company.SiteTemplate.ViewValue = Company.SiteTemplate.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Company.SiteTemplate.ViewValue = System.DBNull.Value
			End If
			Company.SiteTemplate.CssStyle = ""
			Company.SiteTemplate.CssClass = ""
			Company.SiteTemplate.ViewCustomAttributes = ""

			' DefaultSiteTemplate
			If ew_NotEmpty(Company.DefaultSiteTemplate.CurrentValue) Then
				sSqlWrk = "SELECT [Name] FROM [SiteTemplate] WHERE [TemplatePrefix] = '" & ew_AdjustSql(Company.DefaultSiteTemplate.CurrentValue) & "'"
				sSqlWrk = sSqlWrk & " ORDER BY [Name] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Company.DefaultSiteTemplate.ViewValue = RsWrk("Name")
				Else
					Company.DefaultSiteTemplate.ViewValue = Company.DefaultSiteTemplate.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Company.DefaultSiteTemplate.ViewValue = System.DBNull.Value
			End If
			Company.DefaultSiteTemplate.CssStyle = ""
			Company.DefaultSiteTemplate.CssClass = ""
			Company.DefaultSiteTemplate.ViewCustomAttributes = ""

			' UseBreadCrumbURL
			If Convert.ToString(Company.UseBreadCrumbURL.CurrentValue) = "1" Then
				Company.UseBreadCrumbURL.ViewValue = "Yes"
			Else
				Company.UseBreadCrumbURL.ViewValue = "No"
			End If
			Company.UseBreadCrumbURL.CssStyle = ""
			Company.UseBreadCrumbURL.CssClass = ""
			Company.UseBreadCrumbURL.ViewCustomAttributes = ""

			' SingleSiteGallery
			If Convert.ToString(Company.SingleSiteGallery.CurrentValue) = "1" Then
				Company.SingleSiteGallery.ViewValue = "Yes"
			Else
				Company.SingleSiteGallery.ViewValue = "No"
			End If
			Company.SingleSiteGallery.CssStyle = ""
			Company.SingleSiteGallery.CssClass = ""
			Company.SingleSiteGallery.ViewCustomAttributes = ""

			' ActiveFL
			If Convert.ToString(Company.ActiveFL.CurrentValue) = "1" Then
				Company.ActiveFL.ViewValue = "Yes"
			Else
				Company.ActiveFL.ViewValue = "No"
			End If
			Company.ActiveFL.CssStyle = ""
			Company.ActiveFL.CssClass = ""
			Company.ActiveFL.ViewCustomAttributes = ""

			' Address
			Company.Address.ViewValue = Company.Address.CurrentValue
			Company.Address.CssStyle = ""
			Company.Address.CssClass = ""
			Company.Address.ViewCustomAttributes = ""

			' City
			Company.City.ViewValue = Company.City.CurrentValue
			Company.City.CssStyle = ""
			Company.City.CssClass = ""
			Company.City.ViewCustomAttributes = ""

			' StateOrProvince
			Company.StateOrProvince.ViewValue = Company.StateOrProvince.CurrentValue
			Company.StateOrProvince.CssStyle = ""
			Company.StateOrProvince.CssClass = ""
			Company.StateOrProvince.ViewCustomAttributes = ""

			' PostalCode
			Company.PostalCode.ViewValue = Company.PostalCode.CurrentValue
			Company.PostalCode.CssStyle = ""
			Company.PostalCode.CssClass = ""
			Company.PostalCode.ViewCustomAttributes = ""

			' Country
			Company.Country.ViewValue = Company.Country.CurrentValue
			Company.Country.CssStyle = ""
			Company.Country.CssClass = ""
			Company.Country.ViewCustomAttributes = ""

			' PhoneNumber
			Company.PhoneNumber.ViewValue = Company.PhoneNumber.CurrentValue
			Company.PhoneNumber.CssStyle = ""
			Company.PhoneNumber.CssClass = ""
			Company.PhoneNumber.ViewCustomAttributes = ""

			' FaxNumber
			Company.FaxNumber.ViewValue = Company.FaxNumber.CurrentValue
			Company.FaxNumber.CssStyle = ""
			Company.FaxNumber.CssClass = ""
			Company.FaxNumber.ViewCustomAttributes = ""

			' DefaultPaymentTerms
			Company.DefaultPaymentTerms.ViewValue = Company.DefaultPaymentTerms.CurrentValue
			Company.DefaultPaymentTerms.CssStyle = ""
			Company.DefaultPaymentTerms.CssClass = ""
			Company.DefaultPaymentTerms.ViewCustomAttributes = ""

			' DefaultInvoiceDescription
			Company.DefaultInvoiceDescription.ViewValue = Company.DefaultInvoiceDescription.CurrentValue
			Company.DefaultInvoiceDescription.CssStyle = ""
			Company.DefaultInvoiceDescription.CssClass = ""
			Company.DefaultInvoiceDescription.ViewCustomAttributes = ""

			' Component
			Company.Component.ViewValue = Company.Component.CurrentValue
			Company.Component.CssStyle = ""
			Company.Component.CssClass = ""
			Company.Component.ViewCustomAttributes = ""

			' FromEmail
			Company.FromEmail.ViewValue = Company.FromEmail.CurrentValue
			Company.FromEmail.CssStyle = ""
			Company.FromEmail.CssClass = ""
			Company.FromEmail.ViewCustomAttributes = ""

			' SMTP
			Company.SMTP.ViewValue = Company.SMTP.CurrentValue
			Company.SMTP.CssStyle = ""
			Company.SMTP.CssClass = ""
			Company.SMTP.ViewCustomAttributes = ""

			' View refer script
			' CompanyName

			Company.CompanyName.HrefValue = ""

			' SiteTitle
			Company.SiteTitle.HrefValue = ""

			' SiteURL
			Company.SiteURL.HrefValue = ""

			' SiteTemplate
			Company.SiteTemplate.HrefValue = ""

			' DefaultSiteTemplate
			Company.DefaultSiteTemplate.HrefValue = ""

			' ActiveFL
			Company.ActiveFL.HrefValue = ""
		End If

		' Row Rendered event
		Company.Row_Rendered()
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
		If Company.ExportAll Then
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
		If Company.Export = "xml" Then
			oXmlDoc = New XmlDocument()
			oXmlTbl = oXmlDoc.CreateElement("table")
			oXmlDoc.AppendChild(oXmlTbl)
		Else
			ew_Write(ew_ExportHeader(Company.Export))

			' Horizontal format, write header
			If sExportStyle <> "v" OrElse Company.Export = "csv" Then
				sExportStr = ""
				ew_ExportAddValue(sExportStr, "CompanyID", Company.Export)
				ew_ExportAddValue(sExportStr, "CompanyName", Company.Export)
				ew_ExportAddValue(sExportStr, "SiteTitle", Company.Export)
				ew_ExportAddValue(sExportStr, "SiteURL", Company.Export)
				ew_ExportAddValue(sExportStr, "GalleryFolder", Company.Export)
				ew_ExportAddValue(sExportStr, "SiteCategoryTypeID", Company.Export)
				ew_ExportAddValue(sExportStr, "HomePageID", Company.Export)
				ew_ExportAddValue(sExportStr, "DefaultArticleID", Company.Export)
				ew_ExportAddValue(sExportStr, "SiteTemplate", Company.Export)
				ew_ExportAddValue(sExportStr, "DefaultSiteTemplate", Company.Export)
				ew_ExportAddValue(sExportStr, "UseBreadCrumbURL", Company.Export)
				ew_ExportAddValue(sExportStr, "SingleSiteGallery", Company.Export)
				ew_ExportAddValue(sExportStr, "ActiveFL", Company.Export)
				ew_ExportAddValue(sExportStr, "Address", Company.Export)
				ew_ExportAddValue(sExportStr, "City", Company.Export)
				ew_ExportAddValue(sExportStr, "StateOrProvince", Company.Export)
				ew_ExportAddValue(sExportStr, "PostalCode", Company.Export)
				ew_ExportAddValue(sExportStr, "Country", Company.Export)
				ew_ExportAddValue(sExportStr, "PhoneNumber", Company.Export)
				ew_ExportAddValue(sExportStr, "FaxNumber", Company.Export)
				ew_ExportAddValue(sExportStr, "DefaultPaymentTerms", Company.Export)
				ew_ExportAddValue(sExportStr, "DefaultInvoiceDescription", Company.Export)
				ew_ExportAddValue(sExportStr, "Component", Company.Export)
				ew_ExportAddValue(sExportStr, "FromEmail", Company.Export)
				ew_ExportAddValue(sExportStr, "SMTP", Company.Export)
				ew_Write(ew_ExportLine(sExportStr, Company.Export))
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
				Company.RowType = EW_ROWTYPE_VIEW ' Render view
				RenderRow()
				If Company.Export = "xml" Then
					oXmlRec = oXmlDoc.CreateElement("record")
					oXmlTbl.AppendChild(oXmlRec)
					oXmlFld = oXmlDoc.CreateElement("CompanyID") ' CompanyID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Company.CompanyID.ExportValue(Company.Export, Company.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("CompanyName") ' CompanyName
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Company.CompanyName.ExportValue(Company.Export, Company.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("SiteTitle") ' SiteTitle
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Company.SiteTitle.ExportValue(Company.Export, Company.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("SiteURL") ' SiteURL
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Company.SiteURL.ExportValue(Company.Export, Company.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("GalleryFolder") ' GalleryFolder
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Company.GalleryFolder.ExportValue(Company.Export, Company.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("SiteCategoryTypeID") ' SiteCategoryTypeID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Company.SiteCategoryTypeID.ExportValue(Company.Export, Company.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("HomePageID") ' HomePageID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Company.HomePageID.ExportValue(Company.Export, Company.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("DefaultArticleID") ' DefaultArticleID
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Company.DefaultArticleID.ExportValue(Company.Export, Company.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("SiteTemplate") ' SiteTemplate
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Company.SiteTemplate.ExportValue(Company.Export, Company.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("DefaultSiteTemplate") ' DefaultSiteTemplate
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Company.DefaultSiteTemplate.ExportValue(Company.Export, Company.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("UseBreadCrumbURL") ' UseBreadCrumbURL
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Company.UseBreadCrumbURL.ExportValue(Company.Export, Company.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("SingleSiteGallery") ' SingleSiteGallery
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Company.SingleSiteGallery.ExportValue(Company.Export, Company.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("ActiveFL") ' ActiveFL
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Company.ActiveFL.ExportValue(Company.Export, Company.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("Address") ' Address
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Company.Address.ExportValue(Company.Export, Company.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("City") ' City
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Company.City.ExportValue(Company.Export, Company.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("StateOrProvince") ' StateOrProvince
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Company.StateOrProvince.ExportValue(Company.Export, Company.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("PostalCode") ' PostalCode
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Company.PostalCode.ExportValue(Company.Export, Company.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("Country") ' Country
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Company.Country.ExportValue(Company.Export, Company.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("PhoneNumber") ' PhoneNumber
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Company.PhoneNumber.ExportValue(Company.Export, Company.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("FaxNumber") ' FaxNumber
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Company.FaxNumber.ExportValue(Company.Export, Company.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("DefaultPaymentTerms") ' DefaultPaymentTerms
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Company.DefaultPaymentTerms.ExportValue(Company.Export, Company.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("DefaultInvoiceDescription") ' DefaultInvoiceDescription
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Company.DefaultInvoiceDescription.ExportValue(Company.Export, Company.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("Component") ' Component
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Company.Component.ExportValue(Company.Export, Company.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("FromEmail") ' FromEmail
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Company.FromEmail.ExportValue(Company.Export, Company.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
					oXmlFld = oXmlDoc.CreateElement("SMTP") ' SMTP
					oXmlFld.AppendChild(oXmlDoc.CreateTextNode(Company.SMTP.ExportValue(Company.Export, Company.ExportOriginalValue)))
					oXmlRec.AppendChild(oXmlFld)
				Else

					' Vertical format
					If sExportStyle = "v" AndAlso Company.Export <> "csv" Then
						ew_Write(ew_ExportField("CompanyID", Company.CompanyID.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export)) ' CompanyID
						ew_Write(ew_ExportField("CompanyName", Company.CompanyName.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export)) ' CompanyName
						ew_Write(ew_ExportField("SiteTitle", Company.SiteTitle.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export)) ' SiteTitle
						ew_Write(ew_ExportField("SiteURL", Company.SiteURL.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export)) ' SiteURL
						ew_Write(ew_ExportField("GalleryFolder", Company.GalleryFolder.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export)) ' GalleryFolder
						ew_Write(ew_ExportField("SiteCategoryTypeID", Company.SiteCategoryTypeID.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export)) ' SiteCategoryTypeID
						ew_Write(ew_ExportField("HomePageID", Company.HomePageID.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export)) ' HomePageID
						ew_Write(ew_ExportField("DefaultArticleID", Company.DefaultArticleID.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export)) ' DefaultArticleID
						ew_Write(ew_ExportField("SiteTemplate", Company.SiteTemplate.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export)) ' SiteTemplate
						ew_Write(ew_ExportField("DefaultSiteTemplate", Company.DefaultSiteTemplate.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export)) ' DefaultSiteTemplate
						ew_Write(ew_ExportField("UseBreadCrumbURL", Company.UseBreadCrumbURL.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export)) ' UseBreadCrumbURL
						ew_Write(ew_ExportField("SingleSiteGallery", Company.SingleSiteGallery.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export)) ' SingleSiteGallery
						ew_Write(ew_ExportField("ActiveFL", Company.ActiveFL.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export)) ' ActiveFL
						ew_Write(ew_ExportField("Address", Company.Address.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export)) ' Address
						ew_Write(ew_ExportField("City", Company.City.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export)) ' City
						ew_Write(ew_ExportField("StateOrProvince", Company.StateOrProvince.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export)) ' StateOrProvince
						ew_Write(ew_ExportField("PostalCode", Company.PostalCode.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export)) ' PostalCode
						ew_Write(ew_ExportField("Country", Company.Country.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export)) ' Country
						ew_Write(ew_ExportField("PhoneNumber", Company.PhoneNumber.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export)) ' PhoneNumber
						ew_Write(ew_ExportField("FaxNumber", Company.FaxNumber.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export)) ' FaxNumber
						ew_Write(ew_ExportField("DefaultPaymentTerms", Company.DefaultPaymentTerms.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export)) ' DefaultPaymentTerms
						ew_Write(ew_ExportField("DefaultInvoiceDescription", Company.DefaultInvoiceDescription.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export)) ' DefaultInvoiceDescription
						ew_Write(ew_ExportField("Component", Company.Component.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export)) ' Component
						ew_Write(ew_ExportField("FromEmail", Company.FromEmail.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export)) ' FromEmail
						ew_Write(ew_ExportField("SMTP", Company.SMTP.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export)) ' SMTP

					' Horizontal format
					Else
						sExportStr = ""
						ew_ExportAddValue(sExportStr, Company.CompanyID.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export) ' CompanyID
						ew_ExportAddValue(sExportStr, Company.CompanyName.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export) ' CompanyName
						ew_ExportAddValue(sExportStr, Company.SiteTitle.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export) ' SiteTitle
						ew_ExportAddValue(sExportStr, Company.SiteURL.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export) ' SiteURL
						ew_ExportAddValue(sExportStr, Company.GalleryFolder.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export) ' GalleryFolder
						ew_ExportAddValue(sExportStr, Company.SiteCategoryTypeID.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export) ' SiteCategoryTypeID
						ew_ExportAddValue(sExportStr, Company.HomePageID.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export) ' HomePageID
						ew_ExportAddValue(sExportStr, Company.DefaultArticleID.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export) ' DefaultArticleID
						ew_ExportAddValue(sExportStr, Company.SiteTemplate.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export) ' SiteTemplate
						ew_ExportAddValue(sExportStr, Company.DefaultSiteTemplate.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export) ' DefaultSiteTemplate
						ew_ExportAddValue(sExportStr, Company.UseBreadCrumbURL.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export) ' UseBreadCrumbURL
						ew_ExportAddValue(sExportStr, Company.SingleSiteGallery.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export) ' SingleSiteGallery
						ew_ExportAddValue(sExportStr, Company.ActiveFL.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export) ' ActiveFL
						ew_ExportAddValue(sExportStr, Company.Address.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export) ' Address
						ew_ExportAddValue(sExportStr, Company.City.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export) ' City
						ew_ExportAddValue(sExportStr, Company.StateOrProvince.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export) ' StateOrProvince
						ew_ExportAddValue(sExportStr, Company.PostalCode.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export) ' PostalCode
						ew_ExportAddValue(sExportStr, Company.Country.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export) ' Country
						ew_ExportAddValue(sExportStr, Company.PhoneNumber.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export) ' PhoneNumber
						ew_ExportAddValue(sExportStr, Company.FaxNumber.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export) ' FaxNumber
						ew_ExportAddValue(sExportStr, Company.DefaultPaymentTerms.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export) ' DefaultPaymentTerms
						ew_ExportAddValue(sExportStr, Company.DefaultInvoiceDescription.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export) ' DefaultInvoiceDescription
						ew_ExportAddValue(sExportStr, Company.Component.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export) ' Component
						ew_ExportAddValue(sExportStr, Company.FromEmail.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export) ' FromEmail
						ew_ExportAddValue(sExportStr, Company.SMTP.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export) ' SMTP
						ew_Write(ew_ExportLine(sExportStr, Company.Export))
					End If
				End If
			End If
		Loop

		' Close recordset
		Rs.Close()
		Rs.Dispose()
		If Company.Export = "xml" Then
			ew_Write("<?xml version=""1.0"" standalone=""yes""?>" & vbcrlf)
			ew_Write(oXmlDoc.OuterXml)
		Else
			ew_Write(ew_ExportFooter(Company.Export))
		End If
	End Sub

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "Company"
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
		Company_list = New cCompany_list(Me)		
		Company_list.Page_Init()

		' Page main processing
		Company_list.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If Company_list IsNot Nothing Then Company_list.Dispose()
	End Sub
End Class
