Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class link_list
	Inherits AspNetMaker8_wpmWebsite

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

		Private sFilterWrk As String

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

		' Common urls
		Public AddUrl As String = ""

		Public EditUrl As String = ""

		Public CopyUrl As String = ""

		Public DeleteUrl As String = ""

		Public ViewUrl As String = ""

		Public ListUrl As String = ""

		' Export urls
		Public ExportPrintUrl As String = ""

		Public ExportHtmlUrl As String = ""

		Public ExportExcelUrl As String = ""

		Public ExportWordUrl As String = ""

		Public ExportXmlUrl As String = ""

		Public ExportCsvUrl As String = ""

		' Inline urls
		Public InlineAddUrl As String = ""

		Public InlineCopyUrl As String = ""

		Public InlineEditUrl As String = ""

		Public GridAddUrl As String = ""

		Public GridEditUrl As String = ""

		Public MultiDeleteUrl As String = ""

		Public MultiUpdateUrl As String = ""
		Protected m_DebugMsg As String = ""

		Public Property DebugMsg() As String
			Get
				Return IIf(m_DebugMsg <> "", "<p>" & m_DebugMsg & "</p>", m_DebugMsg)
			End Get
			Set(ByVal v As String)
				If m_DebugMsg <> "" Then ' Append
					m_DebugMsg = m_DebugMsg & "<br />" & v
				Else
					m_DebugMsg = v
				End If
			End Set
		End Property

		' Message
		Public Property Message() As String
			Get
				Return ew_Session(EW_SESSION_MESSAGE)
			End Get
			Set(ByVal v As String)
				If ew_NotEmpty(ew_Session(EW_SESSION_MESSAGE)) Then
					If Not ew_SameStr(ew_Session(EW_SESSION_MESSAGE), v) Then ' Append
						ew_Session(EW_SESSION_MESSAGE) = ew_Session(EW_SESSION_MESSAGE) & "<br>" & v
					End If
				Else
					ew_Session(EW_SESSION_MESSAGE) = v
				End If
			End Set	
		End Property

		' Show Message
		Public Sub ShowMessage()
			Dim sMessage As String
			sMessage = Message
			Message_Showing(sMessage)
			If sMessage <> "" Then ew_Write("<p><span class=""ewMessage"">" & sMessage & "</span></p>")
			ew_Session(EW_SESSION_MESSAGE) = "" ' Clear message in Session
		End Sub			

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

		' ASP.NET page object
		Public ReadOnly Property AspNetPage() As link_list
			Get
				Return CType(m_ParentPage, link_list)
			End Get
		End Property

		' Link
		Public Property Link() As cLink
			Get				
				Return ParentPage.Link
			End Get
			Set(ByVal v As cLink)
				ParentPage.Link = v	
			End Set	
		End Property

		' Link
		Public Property Company() As cCompany
			Get				
				Return ParentPage.Company
			End Get
			Set(ByVal v As cCompany)
				ParentPage.Company = v	
			End Set	
		End Property

		' Link
		Public Property LinkCategory() As cLinkCategory
			Get				
				Return ParentPage.LinkCategory
			End Get
			Set(ByVal v As cLinkCategory)
				ParentPage.LinkCategory = v	
			End Set	
		End Property

		' Link
		Public Property LinkType() As cLinkType
			Get				
				Return ParentPage.LinkType
			End Get
			Set(ByVal v As cLinkType)
				ParentPage.LinkType = v	
			End Set	
		End Property

		'
		'  Constructor
		'  - init objects
		'  - open connection
		'
		Public Sub New(ByRef APage As AspNetMaker8_wpmWebsite)				
			m_ParentPage = APage
			m_Page = Me
			m_PageID = "list"
			m_PageObjName = "Link_list"
			m_PageObjTypeName = "cLink_list"			

			' Initialize language object
			Language = New cLanguage(Me)

			' Table Name
			m_TableName = "Link"

			' Initialize table object
			Link = New cLink(Me)
			Company = New cCompany(Me)
			LinkCategory = New cLinkCategory(Me)
			LinkType = New cLinkType(Me)

			' Initialize URLs
			ExportPrintUrl = PageUrl & "export=print"
			ExportExcelUrl = PageUrl & "export=excel"
			ExportWordUrl = PageUrl & "export=word"
			ExportHtmlUrl = PageUrl & "export=html"
			ExportXmlUrl = PageUrl & "export=xml"
			ExportCsvUrl = PageUrl & "export=csv"
			AddUrl = Link.AddUrl
			InlineAddUrl = PageUrl & "a=add"
			GridAddUrl = PageUrl & "a=gridadd"
			GridEditUrl = PageUrl & "a=gridedit"
			MultiDeleteUrl = "link_delete.aspx"
			MultiUpdateUrl = "link_update.aspx"

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

			' Get export parameters
			If ew_Get("export") <> "" Then
				Link.Export = ew_Get("export")
			ElseIf ew_Post("exporttype") <> "" Then
				Link.Export = ew_Post("exporttype")
			Else
				Link.ExportReturnUrl = ew_CurrentUrl()
			End If
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
			Company.Dispose()
			LinkCategory.Dispose()
			LinkType.Dispose()
			ListOptions = Nothing
			ObjForm = Nothing

			' Go to URL if specified
			Dim sRedirectUrl As String = url
			Page_Redirecting(sRedirectUrl)
			If sRedirectUrl <> "" Then
				HttpContext.Current.Response.Clear()
				HttpContext.Current.Response.Redirect(sReDirectUrl)
			End If
		End Sub

	Public lDisplayRecs As Integer ' Number of display records

	Public lStartRec As Integer, lStopRec As Integer, lTotalRecs As Integer, lRecRange As Integer

	Public sSrchWhere As String

	Public lRecCnt As Integer

	Public lEditRowCnt As Integer

	Public lRowCnt As Integer, lRowIndex As Integer

	Public lRecPerRow As Integer, lColCnt As Integer

	Public sDbMasterFilter As String, sDbDetailFilter As String

	Public bMasterRecordExists As Boolean

	Public ListOptions As Object

	Public sMultiSelectKey As String

	Public RestoreSearch As Boolean

	Public x_ewPriv As Integer

	'
	' Page main processing
	'
	Public Sub Page_Main()
		lDisplayRecs = 20
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

			' Set up master detail parameters
			SetUpMasterDetail()

			' Set up list options
			SetupListOptions()

			' Get advanced search criteria
			LoadSearchValues()
			If ValidateSearch() Then

				' Nothing to do
			Else
				Message = ParentPage.gsSearchError
			End If

			' Restore search parms from Session
			Call RestoreSearchParms()

			' Call Recordset SearchValidated event
			Call Link.Recordset_SearchValidated()

			' Set Up Sorting Order
			SetUpSortOrder()

			' Get search criteria for advanced search
			If ParentPage.gsSearchError = "" Then
				sSrchAdvanced = AdvancedSearchWhere()
			End If
		End If

		' Restore display records
		If (Link.RecordsPerPage = -1 OrElse Link.RecordsPerPage > 0) Then
			lDisplayRecs = Link.RecordsPerPage ' Restore from Session
		Else
			lDisplayRecs = 20 ' Load default
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
			If sSrchAdvanced = "" Then ResetAdvancedSearchParms()
			Link.SearchWhere = sSrchWhere ' Save to Session
			If RestoreSearch Then
				lStartRec = 1 ' Reset start record counter
				Link.StartRecordNumber = lStartRec
			End If
		Else
			sSrchWhere = Link.SearchWhere
		End If

		' Build filter
		sFilter = ""

		' Restore master/detail filter
		sDbMasterFilter = Link.MasterFilter ' Restore master filter
		sDbDetailFilter = Link.DetailFilter ' Restore detail filter
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
		If Link.MasterFilter <> "" AndAlso Link.CurrentMasterTable = "Company" Then
			RsMaster = Company.LoadRs(sDbMasterFilter)
			bMasterRecordExists = RsMaster IsNot Nothing
			If Not bMasterRecordExists Then
				Link.MasterFilter = "" ' Clear master filter
				Link.DetailFilter = "" ' Clear detail filter
				Message = Language.Phrase("NoRecord") ' Set no record found
			Page_Terminate(Link.ReturnUrl) ' Return to caller
			Else
				RsMaster.Read()
				Company.LoadListRowValues(RsMaster)
				Company.RowType = EW_ROWTYPE_MASTER ' Master row
				Company.RenderListRow()
				RsMaster.Close()
				RsMaster.Dispose()
			End If
		End If

		' Load master record
		If Link.MasterFilter <> "" AndAlso Link.CurrentMasterTable = "LinkType" Then
			RsMaster = LinkType.LoadRs(sDbMasterFilter)
			bMasterRecordExists = RsMaster IsNot Nothing
			If Not bMasterRecordExists Then
				Link.MasterFilter = "" ' Clear master filter
				Link.DetailFilter = "" ' Clear detail filter
				Message = Language.Phrase("NoRecord") ' Set no record found
			Page_Terminate(Link.ReturnUrl) ' Return to caller
			Else
				RsMaster.Read()
				LinkType.LoadListRowValues(RsMaster)
				LinkType.RowType = EW_ROWTYPE_MASTER ' Master row
				LinkType.RenderListRow()
				RsMaster.Close()
				RsMaster.Dispose()
			End If
		End If

		' Load master record
		If Link.MasterFilter <> "" AndAlso Link.CurrentMasterTable = "LinkCategory" Then
			RsMaster = LinkCategory.LoadRs(sDbMasterFilter)
			bMasterRecordExists = RsMaster IsNot Nothing
			If Not bMasterRecordExists Then
				Link.MasterFilter = "" ' Clear master filter
				Link.DetailFilter = "" ' Clear detail filter
				Message = Language.Phrase("NoRecord") ' Set no record found
			Page_Terminate(Link.ReturnUrl) ' Return to caller
			Else
				RsMaster.Read()
				LinkCategory.LoadListRowValues(RsMaster)
				LinkCategory.RowType = EW_ROWTYPE_MASTER ' Master row
				LinkCategory.RenderListRow()
				RsMaster.Close()
				RsMaster.Dispose()
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
		ElseIf Link.Export = "email" Then
			ExportData()
			ew_End()
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
					lDisplayRecs = 20 ' Non-numeric, load default
				End If
			End If
			Link.RecordsPerPage = lDisplayRecs ' Save to Session

			' Reset start position
			lStartRec = 1
			Link.StartRecordNumber = lStartRec
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
	' Get search parm
	'
	Sub GetSearchParm(ByRef Fld As Object)
		Dim FldParm As String = Fld.FldVar.Substring(2)
		Fld.AdvancedSearch.SearchValue = Link.GetAdvancedSearch("x_" & FldParm)
		Fld.AdvancedSearch.SearchOperator = Link.GetAdvancedSearch("z_" & FldParm)
		Fld.AdvancedSearch.SearchCondition = Link.GetAdvancedSearch("v_" & FldParm)
		Fld.AdvancedSearch.SearchValue2 = Link.GetAdvancedSearch("y_" & FldParm)
		Fld.AdvancedSearch.SearchOperator2 = Link.GetAdvancedSearch("w_" & FldParm)
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
		Link.SearchWhere = sSrchWhere

		' Clear advanced search parameters
		ResetAdvancedSearchParms()
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
		Link.SetAdvancedSearch("x_ASIN", "")
		Link.SetAdvancedSearch("x_DateAdd", "")
	End Sub

	'
	' Restore all search parameters
	'
	Sub RestoreSearchParms()
		Dim bRestore As Boolean = True
		If ew_Get("x_Title") <> "" Then bRestore = False
		If ew_Get("x_LinkTypeCD") <> "" Then bRestore = False
		If ew_Get("x_CategoryID") <> "" Then bRestore = False
		If ew_Get("x_CompanyID") <> "" Then bRestore = False
		If ew_Get("x_SiteCategoryGroupID") <> "" Then bRestore = False
		If ew_Get("x_zPageID") <> "" Then bRestore = False
		If ew_Get("x_Views") <> "" Then bRestore = False
		If ew_Get("x_Description") <> "" Then bRestore = False
		If ew_Get("x_ASIN") <> "" Then bRestore = False
		If ew_Get("x_DateAdd") <> "" Then bRestore = False
		RestoreSearch = bRestore
		If bRestore Then

			' Restore advanced search values
			Call GetSearchParm(Link.Title)
			Call GetSearchParm(Link.LinkTypeCD)
			Call GetSearchParm(Link.CategoryID)
			Call GetSearchParm(Link.CompanyID)
			Call GetSearchParm(Link.SiteCategoryGroupID)
			Call GetSearchParm(Link.zPageID)
			Call GetSearchParm(Link.Views)
			Call GetSearchParm(Link.Description)
			Call GetSearchParm(Link.ASIN)
			Call GetSearchParm(Link.DateAdd)
		End If
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
			Link.UpdateSort(Link.CompanyID) ' CompanyID
			Link.UpdateSort(Link.SiteCategoryGroupID) ' SiteCategoryGroupID
			Link.UpdateSort(Link.zPageID) ' PageID
			Link.UpdateSort(Link.Views) ' Views
			Link.UpdateSort(Link.Ranks) ' Ranks
			Link.UpdateSort(Link.UserID) ' UserID
			Link.UpdateSort(Link.ASIN) ' ASIN
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

			' Reset master/detail keys
			If ew_SameText(sCmd, "resetall") Then
				Link.CurrentMasterTable = "" ' Clear master table
				Link.MasterFilter = "" ' Clear master filter
				sDbMasterFilter = ""
				Link.DetailFilter = "" ' Clear detail filter
				sDbDetailFilter = ""
				Link.CompanyID.SessionValue = ""
				Link.LinkTypeCD.SessionValue = ""
				Link.CategoryID.SessionValue = ""
			End If

			' Reset sort criteria
			If ew_SameText(sCmd, "resetsort") Then
				Dim sOrderBy As String = ""
				Link.SessionOrderBy = sOrderBy
				Link.Title.Sort = ""
				Link.LinkTypeCD.Sort = ""
				Link.CategoryID.Sort = ""
				Link.CompanyID.Sort = ""
				Link.SiteCategoryGroupID.Sort = ""
				Link.zPageID.Sort = ""
				Link.Views.Sort = ""
				Link.Ranks.Sort = ""
				Link.UserID.Sort = ""
				Link.ASIN.Sort = ""
				Link.DateAdd.Sort = ""
			End If

			' Reset start position
			lStartRec = 1
			Link.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	' Set up list options
	'
	Sub SetupListOptions()
		ListOptions.Add("edit")
		ListOptions.GetItem("edit").CssStyle = "white-space: nowrap;"
		ListOptions.GetItem("edit").Visible = True
		ListOptions.GetItem("edit").OnLeft = True
		ListOptions.Add("copy")
		ListOptions.GetItem("copy").CssStyle = "white-space: nowrap;"
		ListOptions.GetItem("copy").Visible = True
		ListOptions.GetItem("copy").OnLeft = True
		ListOptions.Add("delete")
		ListOptions.GetItem("delete").CssStyle = "white-space: nowrap;"
		ListOptions.GetItem("delete").Visible = True
		ListOptions.GetItem("delete").OnLeft = True
		ListOptions_Load()
		If Link.Export <> "" Or Link.CurrentAction = "gridadd" Or Link.CurrentAction = "gridedit" Then
			ListOptions.HideAllOptions()
		End If
	End Sub

	' Render list options
	Sub RenderListOptions()
		Dim oListOpt As cListOption
		ListOptions.LoadDefault()
		If ListOptions.GetItem("edit").Visible Then
			oListOpt = ListOptions.GetItem("edit")
			oListOpt.Body = "<a href=""" & EditUrl & """>" & "<img src=""images/edit.gif"" alt=""" & ew_HtmlEncode(Language.Phrase("EditLink")) & """ title=""" & ew_HtmlEncode(Language.Phrase("EditLink")) & """ width=""16"" height=""16"" border=""0"" />" & "</a>"
		End If
		If ListOptions.GetItem("copy").Visible Then
			oListOpt = ListOptions.GetItem("copy")
			oListOpt.Body = "<a href=""" & CopyUrl & """>" & "<img src=""images/copy.gif"" alt=""" & ew_HtmlEncode(Language.Phrase("CopyLink")) & """ title=""" & ew_HtmlEncode(Language.Phrase("CopyLink")) & """ width=""16"" height=""16"" border=""0"" />" & "</a>"
		End If
		If ListOptions.GetItem("delete").Visible Then
			ListOptions.GetItem("delete").Body = "<a" & "" & " href=""" & DeleteUrl & """>" & "<img src=""images/delete.gif"" alt=""" & ew_HtmlEncode(Language.Phrase("DeleteLink")) & """ title=""" & ew_HtmlEncode(Language.Phrase("DeleteLink")) & """ width=""16"" height=""16"" border=""0"" />" & "</a>"
		End If
		RenderListOptionsExt()
		ListOptions_Rendered()
	End Sub

	Sub RenderListOptionsExt()
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
		Link.CategoryID.CurrentValue = 0
		Link.CompanyID.CurrentValue = 0
		Link.zPageID.CurrentValue = 0
		Link.Views.CurrentValue = 0
		Link.Ranks.CurrentValue = 0
		Link.UserID.CurrentValue = 0
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
		Link.ASIN.AdvancedSearch.SearchValue = ew_Get("x_ASIN")
    	Link.ASIN.AdvancedSearch.SearchOperator = ew_Get("z_ASIN")
		Link.DateAdd.AdvancedSearch.SearchValue = ew_Get("x_DateAdd")
    	Link.DateAdd.AdvancedSearch.SearchOperator = ew_Get("z_DateAdd")
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
		If EW_DEBUG_ENABLED Then DebugMsg = sSql

		' Count
		lTotalRecs = -1		
		Try
			Dim sCntSql As String = Link.SelectCountSQL

			' Write SQL for debug
			If EW_DEBUG_ENABLED Then DebugMsg = sCntSql
			lTotalRecs = Conn.ExecuteScalar(sCntSql)
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
		If EW_DEBUG_ENABLED Then DebugMsg = sSql
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
		Dim sDetailFilter As String
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
		Link.DateAdd.DbValue = RsRow("DateAdd")
		Link.UserName.DbValue = RsRow("UserName")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Initialize urls
		ViewUrl = Link.ViewUrl
		EditUrl = Link.EditUrl
		InlineEditUrl = Link.InlineEditUrl
		CopyUrl = Link.CopyUrl
		InlineCopyUrl = Link.InlineCopyUrl
		DeleteUrl = Link.DeleteUrl

		' Row Rendering event
		Link.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' Title

		Link.Title.CellCssStyle = "white-space: nowrap;"
		Link.Title.CellCssClass = ""
		Link.Title.CellAttrs.Clear(): Link.Title.ViewAttrs.Clear(): Link.Title.EditAttrs.Clear()

		' LinkTypeCD
		Link.LinkTypeCD.CellCssStyle = "white-space: nowrap;"
		Link.LinkTypeCD.CellCssClass = ""
		Link.LinkTypeCD.CellAttrs.Clear(): Link.LinkTypeCD.ViewAttrs.Clear(): Link.LinkTypeCD.EditAttrs.Clear()

		' CategoryID
		Link.CategoryID.CellCssStyle = "white-space: nowrap;"
		Link.CategoryID.CellCssClass = ""
		Link.CategoryID.CellAttrs.Clear(): Link.CategoryID.ViewAttrs.Clear(): Link.CategoryID.EditAttrs.Clear()

		' CompanyID
		Link.CompanyID.CellCssStyle = "white-space: nowrap;"
		Link.CompanyID.CellCssClass = ""
		Link.CompanyID.CellAttrs.Clear(): Link.CompanyID.ViewAttrs.Clear(): Link.CompanyID.EditAttrs.Clear()

		' SiteCategoryGroupID
		Link.SiteCategoryGroupID.CellCssStyle = "white-space: nowrap;"
		Link.SiteCategoryGroupID.CellCssClass = ""
		Link.SiteCategoryGroupID.CellAttrs.Clear(): Link.SiteCategoryGroupID.ViewAttrs.Clear(): Link.SiteCategoryGroupID.EditAttrs.Clear()

		' PageID
		Link.zPageID.CellCssStyle = "white-space: nowrap;"
		Link.zPageID.CellCssClass = ""
		Link.zPageID.CellAttrs.Clear(): Link.zPageID.ViewAttrs.Clear(): Link.zPageID.EditAttrs.Clear()

		' Views
		Link.Views.CellCssStyle = "white-space: nowrap;"
		Link.Views.CellCssClass = ""
		Link.Views.CellAttrs.Clear(): Link.Views.ViewAttrs.Clear(): Link.Views.EditAttrs.Clear()

		' Ranks
		Link.Ranks.CellCssStyle = "white-space: nowrap;"
		Link.Ranks.CellCssClass = ""
		Link.Ranks.CellAttrs.Clear(): Link.Ranks.ViewAttrs.Clear(): Link.Ranks.EditAttrs.Clear()

		' UserID
		Link.UserID.CellCssStyle = "white-space: nowrap;"
		Link.UserID.CellCssClass = ""
		Link.UserID.CellAttrs.Clear(): Link.UserID.ViewAttrs.Clear(): Link.UserID.EditAttrs.Clear()

		' ASIN
		Link.ASIN.CellCssStyle = "white-space: nowrap;"
		Link.ASIN.CellCssClass = ""
		Link.ASIN.CellAttrs.Clear(): Link.ASIN.ViewAttrs.Clear(): Link.ASIN.EditAttrs.Clear()

		' DateAdd
		Link.DateAdd.CellCssStyle = "white-space: nowrap;"
		Link.DateAdd.CellCssClass = ""
		Link.DateAdd.CellAttrs.Clear(): Link.DateAdd.ViewAttrs.Clear(): Link.DateAdd.EditAttrs.Clear()

		'
		'  View  Row
		'

		If Link.RowType = EW_ROWTYPE_VIEW Then ' View row

			' ID
			Link.ID.ViewValue = Link.ID.CurrentValue
			Link.ID.CssStyle = ""
			Link.ID.CssClass = ""
			Link.ID.ViewCustomAttributes = ""

			' Title
			Link.Title.ViewValue = Link.Title.CurrentValue
			Link.Title.CssStyle = ""
			Link.Title.CssClass = ""
			Link.Title.ViewCustomAttributes = ""

			' LinkTypeCD
			If ew_NotEmpty(Link.LinkTypeCD.CurrentValue) Then
				sFilterWrk = "[LinkTypeCD] = '" & ew_AdjustSql(Link.LinkTypeCD.CurrentValue) & "'"
			sSqlWrk = "SELECT [LinkTypeDesc] FROM [LinkType]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [LinkTypeDesc] Asc"
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
				sFilterWrk = "[ID] = " & ew_AdjustSql(Link.CategoryID.CurrentValue) & ""
			sSqlWrk = "SELECT [Title] FROM [LinkCategory]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [Title] Asc"
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
				sFilterWrk = "[CompanyID] = " & ew_AdjustSql(Link.CompanyID.CurrentValue) & ""
			sSqlWrk = "SELECT [CompanyName] FROM [Company]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [CompanyName] Asc"
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
				sFilterWrk = "[SiteCategoryGroupID] = " & ew_AdjustSql(Link.SiteCategoryGroupID.CurrentValue) & ""
			sSqlWrk = "SELECT [SiteCategoryGroupNM] FROM [SiteCategoryGroup]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [SiteCategoryGroupNM] Asc"
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
				sFilterWrk = "[PageID] = " & ew_AdjustSql(Link.zPageID.CurrentValue) & ""
			sSqlWrk = "SELECT [PageName] FROM [Page]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [PageName] Asc"
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

			' UserID
			If ew_NotEmpty(Link.UserID.CurrentValue) Then
				sFilterWrk = "[ContactID] = " & ew_AdjustSql(Link.UserID.CurrentValue) & ""
			sSqlWrk = "SELECT [PrimaryContact] FROM [Contact]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [PrimaryContact] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Link.UserID.ViewValue = RsWrk("PrimaryContact")
				Else
					Link.UserID.ViewValue = Link.UserID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Link.UserID.ViewValue = System.DBNull.Value
			End If
			Link.UserID.CssStyle = ""
			Link.UserID.CssClass = ""
			Link.UserID.ViewCustomAttributes = ""

			' ASIN
			Link.ASIN.ViewValue = Link.ASIN.CurrentValue
			Link.ASIN.CssStyle = ""
			Link.ASIN.CssClass = ""
			Link.ASIN.ViewCustomAttributes = ""

			' DateAdd
			Link.DateAdd.ViewValue = Link.DateAdd.CurrentValue
			Link.DateAdd.CssStyle = ""
			Link.DateAdd.CssClass = ""
			Link.DateAdd.ViewCustomAttributes = ""

			' UserName
			Link.UserName.ViewValue = Link.UserName.CurrentValue
			Link.UserName.CssStyle = ""
			Link.UserName.CssClass = ""
			Link.UserName.ViewCustomAttributes = ""

			' View refer script
			' Title

			Link.Title.HrefValue = ""
			Link.Title.TooltipValue = ""

			' LinkTypeCD
			Link.LinkTypeCD.HrefValue = ""
			Link.LinkTypeCD.TooltipValue = ""

			' CategoryID
			Link.CategoryID.HrefValue = ""
			Link.CategoryID.TooltipValue = ""

			' CompanyID
			Link.CompanyID.HrefValue = ""
			Link.CompanyID.TooltipValue = ""

			' SiteCategoryGroupID
			Link.SiteCategoryGroupID.HrefValue = ""
			Link.SiteCategoryGroupID.TooltipValue = ""

			' PageID
			Link.zPageID.HrefValue = ""
			Link.zPageID.TooltipValue = ""

			' Views
			Link.Views.HrefValue = ""
			Link.Views.TooltipValue = ""

			' Ranks
			Link.Ranks.HrefValue = ""
			Link.Ranks.TooltipValue = ""

			' UserID
			Link.UserID.HrefValue = ""
			Link.UserID.TooltipValue = ""

			' ASIN
			Link.ASIN.HrefValue = ""
			Link.ASIN.TooltipValue = ""

			' DateAdd
			Link.DateAdd.HrefValue = ""
			Link.DateAdd.TooltipValue = ""
		End If

		' Row Rendered event
		If Link.RowType <> EW_ROWTYPE_AGGREGATEINIT Then
			Link.Row_Rendered()
		End If
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
		Link.Title.AdvancedSearch.SearchValue = Link.GetAdvancedSearch("x_Title")
		Link.LinkTypeCD.AdvancedSearch.SearchValue = Link.GetAdvancedSearch("x_LinkTypeCD")
		Link.CategoryID.AdvancedSearch.SearchValue = Link.GetAdvancedSearch("x_CategoryID")
		Link.CompanyID.AdvancedSearch.SearchValue = Link.GetAdvancedSearch("x_CompanyID")
		Link.SiteCategoryGroupID.AdvancedSearch.SearchValue = Link.GetAdvancedSearch("x_SiteCategoryGroupID")
		Link.zPageID.AdvancedSearch.SearchValue = Link.GetAdvancedSearch("x_zPageID")
		Link.Views.AdvancedSearch.SearchValue = Link.GetAdvancedSearch("x_Views")
		Link.Description.AdvancedSearch.SearchValue = Link.GetAdvancedSearch("x_Description")
		Link.ASIN.AdvancedSearch.SearchValue = Link.GetAdvancedSearch("x_ASIN")
		Link.DateAdd.AdvancedSearch.SearchValue = Link.GetAdvancedSearch("x_DateAdd")
	End Sub

	'
	' Export data in HTML/CSV/Word/Excel/XML/Email format
	'
	Sub ExportData()
		Dim oXmlDoc As Object
		Dim sExportStr As String, sExportValue As String
		Dim sOutputStr As String = ""

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
			oXmlDoc = New cXMLDocument()
		Else
			sOutputStr = ew_ExportHeader(Link.Export)

			' Horizontal format, write header
			If sExportStyle <> "v" OrElse Link.Export = "csv" Then
				sExportStr = ""
				ew_ExportAddValue(sExportStr, Link.ID.ExportCaption, Link.Export, IIf(EW_EXPORT_CSS_STYLES, Link.ID.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Link.Title.ExportCaption, Link.Export, IIf(EW_EXPORT_CSS_STYLES, Link.Title.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Link.LinkTypeCD.ExportCaption, Link.Export, IIf(EW_EXPORT_CSS_STYLES, Link.LinkTypeCD.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Link.CategoryID.ExportCaption, Link.Export, IIf(EW_EXPORT_CSS_STYLES, Link.CategoryID.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Link.CompanyID.ExportCaption, Link.Export, IIf(EW_EXPORT_CSS_STYLES, Link.CompanyID.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Link.SiteCategoryGroupID.ExportCaption, Link.Export, IIf(EW_EXPORT_CSS_STYLES, Link.SiteCategoryGroupID.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Link.zPageID.ExportCaption, Link.Export, IIf(EW_EXPORT_CSS_STYLES, Link.zPageID.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Link.Views.ExportCaption, Link.Export, IIf(EW_EXPORT_CSS_STYLES, Link.Views.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Link.Description.ExportCaption, Link.Export, IIf(EW_EXPORT_CSS_STYLES, Link.Description.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Link.URL.ExportCaption, Link.Export, IIf(EW_EXPORT_CSS_STYLES, Link.URL.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Link.Ranks.ExportCaption, Link.Export, IIf(EW_EXPORT_CSS_STYLES, Link.Ranks.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Link.UserID.ExportCaption, Link.Export, IIf(EW_EXPORT_CSS_STYLES, Link.UserID.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Link.ASIN.ExportCaption, Link.Export, IIf(EW_EXPORT_CSS_STYLES, Link.ASIN.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Link.DateAdd.ExportCaption, Link.Export, IIf(EW_EXPORT_CSS_STYLES, Link.DateAdd.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Link.UserName.ExportCaption, Link.Export, IIf(EW_EXPORT_CSS_STYLES, Link.UserName.CellStyles, ""))
				sOutputStr &= ew_ExportLine(sExportStr, Link.Export, IIf(EW_EXPORT_CSS_STYLES, Link.RowStyles, ""))
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
				Link.CssClass = ""
				Link.CssStyle = ""
				Link.RowType = EW_ROWTYPE_VIEW ' Render view
				RenderRow()
				If Link.Export = "xml" Then
					oXmlDoc.AddRow()
					oXmlDoc.AddField("ID", Link.ID.ExportValue(Link.Export, Link.ExportOriginalValue)) ' ID
					oXmlDoc.AddField("Title", Link.Title.ExportValue(Link.Export, Link.ExportOriginalValue)) ' Title
					oXmlDoc.AddField("LinkTypeCD", Link.LinkTypeCD.ExportValue(Link.Export, Link.ExportOriginalValue)) ' LinkTypeCD
					oXmlDoc.AddField("CategoryID", Link.CategoryID.ExportValue(Link.Export, Link.ExportOriginalValue)) ' CategoryID
					oXmlDoc.AddField("CompanyID", Link.CompanyID.ExportValue(Link.Export, Link.ExportOriginalValue)) ' CompanyID
					oXmlDoc.AddField("SiteCategoryGroupID", Link.SiteCategoryGroupID.ExportValue(Link.Export, Link.ExportOriginalValue)) ' SiteCategoryGroupID
					oXmlDoc.AddField("zPageID", Link.zPageID.ExportValue(Link.Export, Link.ExportOriginalValue)) ' PageID
					oXmlDoc.AddField("Views", Link.Views.ExportValue(Link.Export, Link.ExportOriginalValue)) ' Views
					oXmlDoc.AddField("Description", Link.Description.ExportValue(Link.Export, Link.ExportOriginalValue)) ' Description
					oXmlDoc.AddField("URL", Link.URL.ExportValue(Link.Export, Link.ExportOriginalValue)) ' URL
					oXmlDoc.AddField("Ranks", Link.Ranks.ExportValue(Link.Export, Link.ExportOriginalValue)) ' Ranks
					oXmlDoc.AddField("UserID", Link.UserID.ExportValue(Link.Export, Link.ExportOriginalValue)) ' UserID
					oXmlDoc.AddField("ASIN", Link.ASIN.ExportValue(Link.Export, Link.ExportOriginalValue)) ' ASIN
					oXmlDoc.AddField("DateAdd", Link.DateAdd.ExportValue(Link.Export, Link.ExportOriginalValue)) ' DateAdd
					oXmlDoc.AddField("UserName", Link.UserName.ExportValue(Link.Export, Link.ExportOriginalValue)) ' UserName
				Else

					' Vertical format
					If sExportStyle = "v" AndAlso Link.Export <> "csv" Then
						sOutputStr &= ew_ExportField(Link.ID.ExportCaption, Link.ID.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export, IIf(EW_EXPORT_CSS_STYLES, Link.ID.CellStyles, "")) ' ID
						sOutputStr &= ew_ExportField(Link.Title.ExportCaption, Link.Title.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export, IIf(EW_EXPORT_CSS_STYLES, Link.Title.CellStyles, "")) ' Title
						sOutputStr &= ew_ExportField(Link.LinkTypeCD.ExportCaption, Link.LinkTypeCD.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export, IIf(EW_EXPORT_CSS_STYLES, Link.LinkTypeCD.CellStyles, "")) ' LinkTypeCD
						sOutputStr &= ew_ExportField(Link.CategoryID.ExportCaption, Link.CategoryID.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export, IIf(EW_EXPORT_CSS_STYLES, Link.CategoryID.CellStyles, "")) ' CategoryID
						sOutputStr &= ew_ExportField(Link.CompanyID.ExportCaption, Link.CompanyID.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export, IIf(EW_EXPORT_CSS_STYLES, Link.CompanyID.CellStyles, "")) ' CompanyID
						sOutputStr &= ew_ExportField(Link.SiteCategoryGroupID.ExportCaption, Link.SiteCategoryGroupID.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export, IIf(EW_EXPORT_CSS_STYLES, Link.SiteCategoryGroupID.CellStyles, "")) ' SiteCategoryGroupID
						sOutputStr &= ew_ExportField(Link.zPageID.ExportCaption, Link.zPageID.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export, IIf(EW_EXPORT_CSS_STYLES, Link.zPageID.CellStyles, "")) ' PageID
						sOutputStr &= ew_ExportField(Link.Views.ExportCaption, Link.Views.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export, IIf(EW_EXPORT_CSS_STYLES, Link.Views.CellStyles, "")) ' Views
						sOutputStr &= ew_ExportField(Link.Description.ExportCaption, Link.Description.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export, IIf(EW_EXPORT_CSS_STYLES, Link.Description.CellStyles, "")) ' Description
						sOutputStr &= ew_ExportField(Link.URL.ExportCaption, Link.URL.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export, IIf(EW_EXPORT_CSS_STYLES, Link.URL.CellStyles, "")) ' URL
						sOutputStr &= ew_ExportField(Link.Ranks.ExportCaption, Link.Ranks.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export, IIf(EW_EXPORT_CSS_STYLES, Link.Ranks.CellStyles, "")) ' Ranks
						sOutputStr &= ew_ExportField(Link.UserID.ExportCaption, Link.UserID.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export, IIf(EW_EXPORT_CSS_STYLES, Link.UserID.CellStyles, "")) ' UserID
						sOutputStr &= ew_ExportField(Link.ASIN.ExportCaption, Link.ASIN.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export, IIf(EW_EXPORT_CSS_STYLES, Link.ASIN.CellStyles, "")) ' ASIN
						sOutputStr &= ew_ExportField(Link.DateAdd.ExportCaption, Link.DateAdd.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export, IIf(EW_EXPORT_CSS_STYLES, Link.DateAdd.CellStyles, "")) ' DateAdd
						sOutputStr &= ew_ExportField(Link.UserName.ExportCaption, Link.UserName.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export, IIf(EW_EXPORT_CSS_STYLES, Link.UserName.CellStyles, "")) ' UserName

					' Horizontal format
					Else
						sExportStr = ""
						ew_ExportAddValue(sExportStr, Link.ID.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export, IIf(EW_EXPORT_CSS_STYLES, Link.ID.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Link.Title.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export, IIf(EW_EXPORT_CSS_STYLES, Link.Title.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Link.LinkTypeCD.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export, IIf(EW_EXPORT_CSS_STYLES, Link.LinkTypeCD.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Link.CategoryID.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export, IIf(EW_EXPORT_CSS_STYLES, Link.CategoryID.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Link.CompanyID.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export, IIf(EW_EXPORT_CSS_STYLES, Link.CompanyID.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Link.SiteCategoryGroupID.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export, IIf(EW_EXPORT_CSS_STYLES, Link.SiteCategoryGroupID.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Link.zPageID.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export, IIf(EW_EXPORT_CSS_STYLES, Link.zPageID.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Link.Views.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export, IIf(EW_EXPORT_CSS_STYLES, Link.Views.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Link.Description.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export, IIf(EW_EXPORT_CSS_STYLES, Link.Description.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Link.URL.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export, IIf(EW_EXPORT_CSS_STYLES, Link.URL.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Link.Ranks.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export, IIf(EW_EXPORT_CSS_STYLES, Link.Ranks.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Link.UserID.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export, IIf(EW_EXPORT_CSS_STYLES, Link.UserID.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Link.ASIN.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export, IIf(EW_EXPORT_CSS_STYLES, Link.ASIN.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Link.DateAdd.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export, IIf(EW_EXPORT_CSS_STYLES, Link.DateAdd.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Link.UserName.ExportValue(Link.Export, Link.ExportOriginalValue), Link.Export, IIf(EW_EXPORT_CSS_STYLES, Link.UserName.CellStyles, ""))
						sOutputStr &= ew_ExportLine(sExportStr, Link.Export, IIf(EW_EXPORT_CSS_STYLES, Link.RowStyles, ""))
					End If
				End If
			End If
		Loop

		' Close recordset
		Rs.Close()
		Rs.Dispose()
		If Link.Export = "xml" Then
			oXmlDoc.Output
		Else
			sOutputStr &= ew_ExportFooter(Link.Export)
			ew_Write(sOutputStr)
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
			If sMasterTblVar = "Company" Then
				bValidMaster = True
				sDbMasterFilter = Link.SqlMasterFilter_Company
				sDbDetailFilter = Link.SqlDetailFilter_Company
				If ew_Get("CompanyID") <> "" Then
					Company.CompanyID.QueryStringValue = ew_Get("CompanyID")
					Link.CompanyID.QueryStringValue = Company.CompanyID.QueryStringValue
					Link.CompanyID.SessionValue = Link.CompanyID.QueryStringValue
					If Not IsNumeric(Company.CompanyID.QueryStringValue) Then bValidMaster = False
					sDbMasterFilter = sDbMasterFilter.Replace("@CompanyID@", ew_AdjustSql(Company.CompanyID.QueryStringValue))
					sDbDetailFilter = sDbDetailFilter.Replace("@CompanyID@", ew_AdjustSql(Company.CompanyID.QueryStringValue))
				Else
					bValidMaster = False
				End If
			End If
			If sMasterTblVar = "LinkType" Then
				bValidMaster = True
				sDbMasterFilter = Link.SqlMasterFilter_LinkType
				sDbDetailFilter = Link.SqlDetailFilter_LinkType
				If ew_Get("LinkTypeCD") <> "" Then
					LinkType.LinkTypeCD.QueryStringValue = ew_Get("LinkTypeCD")
					Link.LinkTypeCD.QueryStringValue = LinkType.LinkTypeCD.QueryStringValue
					Link.LinkTypeCD.SessionValue = Link.LinkTypeCD.QueryStringValue
					sDbMasterFilter = sDbMasterFilter.Replace("@LinkTypeCD@", ew_AdjustSql(LinkType.LinkTypeCD.QueryStringValue))
					sDbDetailFilter = sDbDetailFilter.Replace("@LinkTypeCD@", ew_AdjustSql(LinkType.LinkTypeCD.QueryStringValue))
				Else
					bValidMaster = False
				End If
			End If
			If sMasterTblVar = "LinkCategory" Then
				bValidMaster = True
				sDbMasterFilter = Link.SqlMasterFilter_LinkCategory
				sDbDetailFilter = Link.SqlDetailFilter_LinkCategory
				If ew_Get("ID") <> "" Then
					LinkCategory.ID.QueryStringValue = ew_Get("ID")
					Link.CategoryID.QueryStringValue = LinkCategory.ID.QueryStringValue
					Link.CategoryID.SessionValue = Link.CategoryID.QueryStringValue
					If Not IsNumeric(LinkCategory.ID.QueryStringValue) Then bValidMaster = False
					sDbMasterFilter = sDbMasterFilter.Replace("@ID@", ew_AdjustSql(LinkCategory.ID.QueryStringValue))
					sDbDetailFilter = sDbDetailFilter.Replace("@CategoryID@", ew_AdjustSql(LinkCategory.ID.QueryStringValue))
				Else
					bValidMaster = False
				End If
			End If
		End If
		If bValidMaster Then

			' Save current master table
			Link.CurrentMasterTable = sMasterTblVar

			' Reset start record counter (new master key)
			lStartRec = 1
			Link.StartRecordNumber = lStartRec
			Link.MasterFilter = sDbMasterFilter ' Set up master filter
			Link.DetailFilter = sDbDetailFilter ' Set up detail filter

			' Clear previous master session values
			If sMasterTblVar <> "Company" Then
				If Link.CompanyID.QueryStringValue = "" Then Link.CompanyID.SessionValue = ""
			End If
			If sMasterTblVar <> "LinkType" Then
				If Link.LinkTypeCD.QueryStringValue = "" Then Link.LinkTypeCD.SessionValue = ""
			End If
			If sMasterTblVar <> "LinkCategory" Then
				If Link.CategoryID.QueryStringValue = "" Then Link.CategoryID.SessionValue = ""
			End If
		End If
	End Sub

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "Link"
		Dim filePfx As String = "log"
		Dim curDateTime As String, id As String, user As String, action As String
		Dim i As Integer
		Dim dt As DateTime = Now()		
		curDateTime = dt.ToString("yyyy/MM/dd HH:mm:ss")
		id = HttpContext.Current.Request.ServerVariables("SCRIPT_NAME")
    user = CurrentUserName()
		action = typ
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "", "", "", "")
	End Sub

		' Page Load event
		Public Sub Page_Load()

			'HttpContext.Current.Response.Write("Page Load")
		End Sub

		' Page Unload event
		Public Sub Page_Unload()

			'HttpContext.Current.Response.Write("Page Unload")
		End Sub

		' Page Redirecting event
		Public Sub Page_Redirecting(ByRef url As String)

			'url = newurl
		End Sub

		' Message Showing event
		Public Sub Message_Showing(ByRef msg As String)

			'msg = newmsg
		End Sub

	' Form Custom Validate event
	Public Function Form_CustomValidate(ByRef CustomError As String) As Boolean

		'Return error message in CustomError
		Return True
	End Function

	' ListOptions Load event
	Public Sub ListOptions_Load()

		'Example: 
		'ListOptions.Add("new")
		'ListOptions.GetItem("new").OnLeft = True ' Link on left
		'ListOptions.MoveItem("new", 0) ' Move to first column

	End Sub

	' ListOptions Rendered event
	Public Sub ListOptions_Rendered()

		'Example: 
		'ListOptions.GetItem("new").Body = "xxx"

	End Sub
	End Class

	'
	' ASP.NET Page_Load event
	'

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

		' Page init
		Link_list = New cLink_list(Me)		
		Link_list.Page_Init()		

		' Set buffer/cache option
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

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
