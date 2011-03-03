Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class zpage_list
	Inherits AspNetMaker8_wpmWebsite

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

		Private sFilterWrk As String

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

		' ASP.NET page object
		Public ReadOnly Property AspNetPage() As zpage_list
			Get
				Return CType(m_ParentPage, zpage_list)
			End Get
		End Property

		' zPage
		Public Property zPage() As czPage
			Get				
				Return ParentPage.zPage
			End Get
			Set(ByVal v As czPage)
				ParentPage.zPage = v	
			End Set	
		End Property

		' zPage
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
		Public Sub New(ByRef APage As AspNetMaker8_wpmWebsite)				
			m_ParentPage = APage
			m_Page = Me
			m_PageID = "list"
			m_PageObjName = "zPage_list"
			m_PageObjTypeName = "czPage_list"			

			' Initialize language object
			Language = New cLanguage(Me)

			' Table Name
			m_TableName = "Page"

			' Initialize table object
			zPage = New czPage(Me)
			Company = New cCompany(Me)

			' Initialize URLs
			ExportPrintUrl = PageUrl & "export=print"
			ExportExcelUrl = PageUrl & "export=excel"
			ExportWordUrl = PageUrl & "export=word"
			ExportHtmlUrl = PageUrl & "export=html"
			ExportXmlUrl = PageUrl & "export=xml"
			ExportCsvUrl = PageUrl & "export=csv"
			AddUrl = zPage.AddUrl
			InlineAddUrl = PageUrl & "a=add"
			GridAddUrl = PageUrl & "a=gridadd"
			GridEditUrl = PageUrl & "a=gridedit"
			MultiDeleteUrl = "zpage_delete.aspx"
			MultiUpdateUrl = "zpage_update.aspx"

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
				zPage.Export = ew_Get("export")
			ElseIf ew_Post("exporttype") <> "" Then
				zPage.Export = ew_Post("exporttype")
			Else
				zPage.ExportReturnUrl = ew_CurrentUrl()
			End If
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
			Company.Dispose()
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
			Call zPage.Recordset_SearchValidated()

			' Set Up Sorting Order
			SetUpSortOrder()

			' Get search criteria for advanced search
			If ParentPage.gsSearchError = "" Then
				sSrchAdvanced = AdvancedSearchWhere()
			End If
		End If

		' Restore display records
		If (zPage.RecordsPerPage = -1 OrElse zPage.RecordsPerPage > 0) Then
			lDisplayRecs = zPage.RecordsPerPage ' Restore from Session
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
		zPage.Recordset_Searching(sSrchWhere)

		' Save search criteria
		If sSrchWhere <> "" Then
			If sSrchAdvanced = "" Then ResetAdvancedSearchParms()
			zPage.SearchWhere = sSrchWhere ' Save to Session
			If RestoreSearch Then
				lStartRec = 1 ' Reset start record counter
				zPage.StartRecordNumber = lStartRec
			End If
		Else
			sSrchWhere = zPage.SearchWhere
		End If

		' Build filter
		sFilter = ""

		' Restore master/detail filter
		sDbMasterFilter = zPage.MasterFilter ' Restore master filter
		sDbDetailFilter = zPage.DetailFilter ' Restore detail filter
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
		If zPage.MasterFilter <> "" AndAlso zPage.CurrentMasterTable = "Company" Then
			RsMaster = Company.LoadRs(sDbMasterFilter)
			bMasterRecordExists = RsMaster IsNot Nothing
			If Not bMasterRecordExists Then
				zPage.MasterFilter = "" ' Clear master filter
				zPage.DetailFilter = "" ' Clear detail filter
				Message = Language.Phrase("NoRecord") ' Set no record found
			Page_Terminate(zPage.ReturnUrl) ' Return to caller
			Else
				RsMaster.Read()
				Company.LoadListRowValues(RsMaster)
				Company.RowType = EW_ROWTYPE_MASTER ' Master row
				Company.RenderListRow()
				RsMaster.Close()
				RsMaster.Dispose()
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
		ElseIf zPage.Export = "email" Then
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
			zPage.RecordsPerPage = lDisplayRecs ' Save to Session

			' Reset start position
			lStartRec = 1
			zPage.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	' Return Advanced Search WHERE based on QueryString parameters
	'
	Function AdvancedSearchWhere() As String		
		Dim sWhere As String = ""
		BuildSearchSql(sWhere, zPage.ParentPageID, False) ' ParentPageID
		BuildSearchSql(sWhere, zPage.zPageName, False) ' PageName
		BuildSearchSql(sWhere, zPage.PageTitle, False) ' PageTitle
		BuildSearchSql(sWhere, zPage.PageTypeID, False) ' PageTypeID
		BuildSearchSql(sWhere, zPage.GroupID, False) ' GroupID
		BuildSearchSql(sWhere, zPage.Active, False) ' Active
		BuildSearchSql(sWhere, zPage.PageOrder, False) ' PageOrder
		BuildSearchSql(sWhere, zPage.CompanyID, False) ' CompanyID
		BuildSearchSql(sWhere, zPage.PageDescription, False) ' PageDescription
		BuildSearchSql(sWhere, zPage.PageKeywords, False) ' PageKeywords
		BuildSearchSql(sWhere, zPage.ImagesPerRow, False) ' ImagesPerRow
		BuildSearchSql(sWhere, zPage.RowsPerPage, False) ' RowsPerPage
		BuildSearchSql(sWhere, zPage.PageFileName, False) ' PageFileName
		BuildSearchSql(sWhere, zPage.AllowMessage, False) ' AllowMessage
		BuildSearchSql(sWhere, zPage.SiteCategoryID, False) ' SiteCategoryID
		BuildSearchSql(sWhere, zPage.SiteCategoryGroupID, False) ' SiteCategoryGroupID
		BuildSearchSql(sWhere, zPage.VersionNo, False) ' VersionNo
		BuildSearchSql(sWhere, zPage.ModifiedDT, False) ' ModifiedDT

		' Set up search parm
		If sWhere <> "" Then
			SetSearchParm(zPage.ParentPageID) ' ParentPageID
			SetSearchParm(zPage.zPageName) ' PageName
			SetSearchParm(zPage.PageTitle) ' PageTitle
			SetSearchParm(zPage.PageTypeID) ' PageTypeID
			SetSearchParm(zPage.GroupID) ' GroupID
			SetSearchParm(zPage.Active) ' Active
			SetSearchParm(zPage.PageOrder) ' PageOrder
			SetSearchParm(zPage.CompanyID) ' CompanyID
			SetSearchParm(zPage.PageDescription) ' PageDescription
			SetSearchParm(zPage.PageKeywords) ' PageKeywords
			SetSearchParm(zPage.ImagesPerRow) ' ImagesPerRow
			SetSearchParm(zPage.RowsPerPage) ' RowsPerPage
			SetSearchParm(zPage.PageFileName) ' PageFileName
			SetSearchParm(zPage.AllowMessage) ' AllowMessage
			SetSearchParm(zPage.SiteCategoryID) ' SiteCategoryID
			SetSearchParm(zPage.SiteCategoryGroupID) ' SiteCategoryGroupID
			SetSearchParm(zPage.VersionNo) ' VersionNo
			SetSearchParm(zPage.ModifiedDT) ' ModifiedDT
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
	' Get search parm
	'
	Sub GetSearchParm(ByRef Fld As Object)
		Dim FldParm As String = Fld.FldVar.Substring(2)
		Fld.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_" & FldParm)
		Fld.AdvancedSearch.SearchOperator = zPage.GetAdvancedSearch("z_" & FldParm)
		Fld.AdvancedSearch.SearchCondition = zPage.GetAdvancedSearch("v_" & FldParm)
		Fld.AdvancedSearch.SearchValue2 = zPage.GetAdvancedSearch("y_" & FldParm)
		Fld.AdvancedSearch.SearchOperator2 = zPage.GetAdvancedSearch("w_" & FldParm)
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
		zPage.SearchWhere = sSrchWhere

		' Clear advanced search parameters
		ResetAdvancedSearchParms()
	End Sub

	'
	' Clear all advanced search parameters
	'
	Sub ResetAdvancedSearchParms()
		zPage.SetAdvancedSearch("x_ParentPageID", "")
		zPage.SetAdvancedSearch("x_zPageName", "")
		zPage.SetAdvancedSearch("x_PageTitle", "")
		zPage.SetAdvancedSearch("x_PageTypeID", "")
		zPage.SetAdvancedSearch("x_GroupID", "")
		zPage.SetAdvancedSearch("x_Active", "")
		zPage.SetAdvancedSearch("x_PageOrder", "")
		zPage.SetAdvancedSearch("x_CompanyID", "")
		zPage.SetAdvancedSearch("x_PageDescription", "")
		zPage.SetAdvancedSearch("x_PageKeywords", "")
		zPage.SetAdvancedSearch("x_ImagesPerRow", "")
		zPage.SetAdvancedSearch("x_RowsPerPage", "")
		zPage.SetAdvancedSearch("x_PageFileName", "")
		zPage.SetAdvancedSearch("x_AllowMessage", "")
		zPage.SetAdvancedSearch("x_SiteCategoryID", "")
		zPage.SetAdvancedSearch("x_SiteCategoryGroupID", "")
		zPage.SetAdvancedSearch("x_VersionNo", "")
		zPage.SetAdvancedSearch("x_ModifiedDT", "")
	End Sub

	'
	' Restore all search parameters
	'
	Sub RestoreSearchParms()
		Dim bRestore As Boolean = True
		If ew_Get("x_ParentPageID") <> "" Then bRestore = False
		If ew_Get("x_zPageName") <> "" Then bRestore = False
		If ew_Get("x_PageTitle") <> "" Then bRestore = False
		If ew_Get("x_PageTypeID") <> "" Then bRestore = False
		If ew_Get("x_GroupID") <> "" Then bRestore = False
		If ew_Get("x_Active") <> "" Then bRestore = False
		If ew_Get("x_PageOrder") <> "" Then bRestore = False
		If ew_Get("x_CompanyID") <> "" Then bRestore = False
		If ew_Get("x_PageDescription") <> "" Then bRestore = False
		If ew_Get("x_PageKeywords") <> "" Then bRestore = False
		If ew_Get("x_ImagesPerRow") <> "" Then bRestore = False
		If ew_Get("x_RowsPerPage") <> "" Then bRestore = False
		If ew_Get("x_PageFileName") <> "" Then bRestore = False
		If ew_Get("x_AllowMessage") <> "" Then bRestore = False
		If ew_Get("x_SiteCategoryID") <> "" Then bRestore = False
		If ew_Get("x_SiteCategoryGroupID") <> "" Then bRestore = False
		If ew_Get("x_VersionNo") <> "" Then bRestore = False
		If ew_Get("x_ModifiedDT") <> "" Then bRestore = False
		RestoreSearch = bRestore
		If bRestore Then

			' Restore advanced search values
			Call GetSearchParm(zPage.ParentPageID)
			Call GetSearchParm(zPage.zPageName)
			Call GetSearchParm(zPage.PageTitle)
			Call GetSearchParm(zPage.PageTypeID)
			Call GetSearchParm(zPage.GroupID)
			Call GetSearchParm(zPage.Active)
			Call GetSearchParm(zPage.PageOrder)
			Call GetSearchParm(zPage.CompanyID)
			Call GetSearchParm(zPage.PageDescription)
			Call GetSearchParm(zPage.PageKeywords)
			Call GetSearchParm(zPage.ImagesPerRow)
			Call GetSearchParm(zPage.RowsPerPage)
			Call GetSearchParm(zPage.PageFileName)
			Call GetSearchParm(zPage.AllowMessage)
			Call GetSearchParm(zPage.SiteCategoryID)
			Call GetSearchParm(zPage.SiteCategoryGroupID)
			Call GetSearchParm(zPage.VersionNo)
			Call GetSearchParm(zPage.ModifiedDT)
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
			zPage.CurrentOrder = ew_Get("order")
			zPage.CurrentOrderType = ew_Get("ordertype")
			zPage.UpdateSort(zPage.ParentPageID) ' ParentPageID
			zPage.UpdateSort(zPage.zPageName) ' PageName
			zPage.UpdateSort(zPage.PageTitle) ' PageTitle
			zPage.UpdateSort(zPage.PageTypeID) ' PageTypeID
			zPage.UpdateSort(zPage.GroupID) ' GroupID
			zPage.UpdateSort(zPage.Active) ' Active
			zPage.UpdateSort(zPage.PageOrder) ' PageOrder
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

			' Reset master/detail keys
			If ew_SameText(sCmd, "resetall") Then
				zPage.CurrentMasterTable = "" ' Clear master table
				zPage.MasterFilter = "" ' Clear master filter
				sDbMasterFilter = ""
				zPage.DetailFilter = "" ' Clear detail filter
				sDbDetailFilter = ""
				zPage.CompanyID.SessionValue = ""
			End If

			' Reset sort criteria
			If ew_SameText(sCmd, "resetsort") Then
				Dim sOrderBy As String = ""
				zPage.SessionOrderBy = sOrderBy
				zPage.ParentPageID.Sort = ""
				zPage.zPageName.Sort = ""
				zPage.PageTitle.Sort = ""
				zPage.PageTypeID.Sort = ""
				zPage.GroupID.Sort = ""
				zPage.Active.Sort = ""
				zPage.PageOrder.Sort = ""
			End If

			' Reset start position
			lStartRec = 1
			zPage.StartRecordNumber = lStartRec
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
		ListOptions.Add("detail_PageImage")
		ListOptions.GetItem("detail_PageImage").CssStyle = "white-space: nowrap;"
		ListOptions.GetItem("detail_PageImage").Visible = True
		ListOptions.GetItem("detail_PageImage").OnLeft = True
		ListOptions.Add("detail_Article")
		ListOptions.GetItem("detail_Article").CssStyle = "white-space: nowrap;"
		ListOptions.GetItem("detail_Article").Visible = True
		ListOptions.GetItem("detail_Article").OnLeft = True
		ListOptions_Load()
		If zPage.Export <> "" Or zPage.CurrentAction = "gridadd" Or zPage.CurrentAction = "gridedit" Then
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
		If True Then
			oListOpt = ListOptions.GetItem("detail_PageImage")
			oListOpt.Body = "<img src=""images/detail.gif"" alt=""" & ew_HtmlEncode(Language.Phrase("DetailLink")) & """ title=""" & ew_HtmlEncode(Language.Phrase("DetailLink")) & """ width=""16"" height=""16"" border=""0"" />" & Language.TablePhrase("PageImage", "TblCaption")
			oListOpt.Body = "<a href=""pageimage_list.aspx?" & EW_TABLE_SHOW_MASTER & "=zPage&zPageID=" & HttpContext.Current.Server.UrlEncode(Convert.ToString(zPage.zPageID.CurrentValue)) & """>" & oListOpt.Body & "</a>"
		End If
		If True Then
			oListOpt = ListOptions.GetItem("detail_Article")
			oListOpt.Body = "<img src=""images/detail.gif"" alt=""" & ew_HtmlEncode(Language.Phrase("DetailLink")) & """ title=""" & ew_HtmlEncode(Language.Phrase("DetailLink")) & """ width=""16"" height=""16"" border=""0"" />" & Language.TablePhrase("Article", "TblCaption")
			oListOpt.Body = "<a href=""article_list.aspx?" & EW_TABLE_SHOW_MASTER & "=zPage&zPageID=" & HttpContext.Current.Server.UrlEncode(Convert.ToString(zPage.zPageID.CurrentValue)) & """>" & oListOpt.Body & "</a>"
		End If
		RenderListOptionsExt()
		ListOptions_Rendered()
	End Sub

	Sub RenderListOptionsExt()
		Dim sHyperLinkParm As String, oListOpt As cListOption
		sSqlWrk = "[PageID]=" & ew_AdjustSql(zPage.zPageID.CurrentValue) & ""
		sSqlWrk = cTEA.Encrypt(sSqlWrk, EW_RANDOM_KEY)
		sSqlWrk = sSqlWrk.Replace("'", "\'")
		sHyperLinkParm = " href=""pageimage_list.aspx?" & EW_TABLE_SHOW_MASTER & "=zPage&zPageID=" & HttpContext.Current.Server.URLEncode(Convert.ToString(zPage.zPageID.CurrentValue)) & """"
		oListOpt = ListOptions.GetItem("detail_PageImage")
		oListOpt.Body = Language.TablePhrase("PageImage", "TblCaption")
		oListOpt.Body &= "<img src=""images/detail.gif"" alt=""" & ew_HtmlEncode(Language.Phrase("DetailLink")) & """ title=""" & ew_HtmlEncode(Language.Phrase("DetailLink")) & """ width=""16"" height=""16"" border=""0"" />"
		oListOpt.Body = "<a" & sHyperLinkParm & ">" & oListOpt.Body & "</a>"
		sHyperLinkParm = " href=""javascript:void(0);"" name=""ew_zPage_PageImage_DetailLink%i"" id=""ew_zPage_PageImage_DetailLink%i"" onclick=""ew_AjaxShowDetails2(event, this, 'pageimage_preview.aspx?f=%s')"""
		sHyperLinkParm = sHyperLinkParm.Replace("%i", lRowCnt)
		sHyperLinkParm = sHyperLinkParm.Replace("%s", sSqlWrk)
		oListOpt.Body = "<a" & sHyperLinkParm & "><img class=""ewPreviewRowImage"" src=""images/expand.gif"" width=""9"" height=""9"" border=""0""></a>&nbsp;" & oListOpt.Body
		sSqlWrk = "[PageID]=" & ew_AdjustSql(zPage.zPageID.CurrentValue) & ""
		sSqlWrk = cTEA.Encrypt(sSqlWrk, EW_RANDOM_KEY)
		sSqlWrk = sSqlWrk.Replace("'", "\'")
		sHyperLinkParm = " href=""article_list.aspx?" & EW_TABLE_SHOW_MASTER & "=zPage&zPageID=" & HttpContext.Current.Server.URLEncode(Convert.ToString(zPage.zPageID.CurrentValue)) & """"
		oListOpt = ListOptions.GetItem("detail_Article")
		oListOpt.Body = Language.TablePhrase("Article", "TblCaption")
		oListOpt.Body &= "<img src=""images/detail.gif"" alt=""" & ew_HtmlEncode(Language.Phrase("DetailLink")) & """ title=""" & ew_HtmlEncode(Language.Phrase("DetailLink")) & """ width=""16"" height=""16"" border=""0"" />"
		oListOpt.Body = "<a" & sHyperLinkParm & ">" & oListOpt.Body & "</a>"
		sHyperLinkParm = " href=""javascript:void(0);"" name=""ew_zPage_Article_DetailLink%i"" id=""ew_zPage_Article_DetailLink%i"" onclick=""ew_AjaxShowDetails2(event, this, 'article_preview.aspx?f=%s')"""
		sHyperLinkParm = sHyperLinkParm.Replace("%i", lRowCnt)
		sHyperLinkParm = sHyperLinkParm.Replace("%s", sSqlWrk)
		oListOpt.Body = "<a" & sHyperLinkParm & "><img class=""ewPreviewRowImage"" src=""images/expand.gif"" width=""9"" height=""9"" border=""0""></a>&nbsp;" & oListOpt.Body
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
		zPage.ParentPageID.CurrentValue = 0
		zPage.PageTypeID.CurrentValue = 0
		zPage.GroupID.CurrentValue = 0
		zPage.PageOrder.CurrentValue = 0
	End Sub

	'
	'  Load search values for validation
	'
	Sub LoadSearchValues()
		zPage.ParentPageID.AdvancedSearch.SearchValue = ew_Get("x_ParentPageID")
    	zPage.ParentPageID.AdvancedSearch.SearchOperator = ew_Get("z_ParentPageID")
		zPage.zPageName.AdvancedSearch.SearchValue = ew_Get("x_zPageName")
    	zPage.zPageName.AdvancedSearch.SearchOperator = ew_Get("z_zPageName")
		zPage.PageTitle.AdvancedSearch.SearchValue = ew_Get("x_PageTitle")
    	zPage.PageTitle.AdvancedSearch.SearchOperator = ew_Get("z_PageTitle")
		zPage.PageTypeID.AdvancedSearch.SearchValue = ew_Get("x_PageTypeID")
    	zPage.PageTypeID.AdvancedSearch.SearchOperator = ew_Get("z_PageTypeID")
		zPage.GroupID.AdvancedSearch.SearchValue = ew_Get("x_GroupID")
    	zPage.GroupID.AdvancedSearch.SearchOperator = ew_Get("z_GroupID")
		zPage.Active.AdvancedSearch.SearchValue = ew_Get("x_Active")
    	zPage.Active.AdvancedSearch.SearchOperator = ew_Get("z_Active")
		zPage.PageOrder.AdvancedSearch.SearchValue = ew_Get("x_PageOrder")
    	zPage.PageOrder.AdvancedSearch.SearchOperator = ew_Get("z_PageOrder")
		zPage.CompanyID.AdvancedSearch.SearchValue = ew_Get("x_CompanyID")
    	zPage.CompanyID.AdvancedSearch.SearchOperator = ew_Get("z_CompanyID")
		zPage.PageDescription.AdvancedSearch.SearchValue = ew_Get("x_PageDescription")
    	zPage.PageDescription.AdvancedSearch.SearchOperator = ew_Get("z_PageDescription")
		zPage.PageKeywords.AdvancedSearch.SearchValue = ew_Get("x_PageKeywords")
    	zPage.PageKeywords.AdvancedSearch.SearchOperator = ew_Get("z_PageKeywords")
		zPage.ImagesPerRow.AdvancedSearch.SearchValue = ew_Get("x_ImagesPerRow")
    	zPage.ImagesPerRow.AdvancedSearch.SearchOperator = ew_Get("z_ImagesPerRow")
		zPage.RowsPerPage.AdvancedSearch.SearchValue = ew_Get("x_RowsPerPage")
    	zPage.RowsPerPage.AdvancedSearch.SearchOperator = ew_Get("z_RowsPerPage")
		zPage.PageFileName.AdvancedSearch.SearchValue = ew_Get("x_PageFileName")
    	zPage.PageFileName.AdvancedSearch.SearchOperator = ew_Get("z_PageFileName")
		zPage.AllowMessage.AdvancedSearch.SearchValue = ew_Get("x_AllowMessage")
    	zPage.AllowMessage.AdvancedSearch.SearchOperator = ew_Get("z_AllowMessage")
		zPage.SiteCategoryID.AdvancedSearch.SearchValue = ew_Get("x_SiteCategoryID")
    	zPage.SiteCategoryID.AdvancedSearch.SearchOperator = ew_Get("z_SiteCategoryID")
		zPage.SiteCategoryGroupID.AdvancedSearch.SearchValue = ew_Get("x_SiteCategoryGroupID")
    	zPage.SiteCategoryGroupID.AdvancedSearch.SearchOperator = ew_Get("z_SiteCategoryGroupID")
		zPage.VersionNo.AdvancedSearch.SearchValue = ew_Get("x_VersionNo")
    	zPage.VersionNo.AdvancedSearch.SearchOperator = ew_Get("z_VersionNo")
		zPage.ModifiedDT.AdvancedSearch.SearchValue = ew_Get("x_ModifiedDT")
    	zPage.ModifiedDT.AdvancedSearch.SearchOperator = ew_Get("z_ModifiedDT")
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
		If EW_DEBUG_ENABLED Then DebugMsg = sSql

		' Count
		lTotalRecs = -1		
		Try
			Dim sCntSql As String = zPage.SelectCountSQL

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
		If EW_DEBUG_ENABLED Then DebugMsg = sSql
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
		Dim sDetailFilter As String
		zPage.zPageID.DbValue = RsRow("PageID")
		zPage.ParentPageID.DbValue = RsRow("ParentPageID")
		zPage.zPageName.DbValue = RsRow("PageName")
		zPage.PageTitle.DbValue = RsRow("PageTitle")
		zPage.PageTypeID.DbValue = RsRow("PageTypeID")
		zPage.GroupID.DbValue = RsRow("GroupID")
		zPage.Active.DbValue = IIf(ew_ConvertToBool(RsRow("Active")), "1", "0")
		zPage.PageOrder.DbValue = RsRow("PageOrder")
		zPage.CompanyID.DbValue = RsRow("CompanyID")
		zPage.PageDescription.DbValue = RsRow("PageDescription")
		zPage.PageKeywords.DbValue = RsRow("PageKeywords")
		zPage.ImagesPerRow.DbValue = RsRow("ImagesPerRow")
		zPage.RowsPerPage.DbValue = RsRow("RowsPerPage")
		zPage.PageFileName.DbValue = RsRow("PageFileName")
		zPage.AllowMessage.DbValue = IIf(ew_ConvertToBool(RsRow("AllowMessage")), "1", "0")
		zPage.SiteCategoryID.DbValue = RsRow("SiteCategoryID")
		zPage.SiteCategoryGroupID.DbValue = RsRow("SiteCategoryGroupID")
		zPage.VersionNo.DbValue = RsRow("VersionNo")
		zPage.ModifiedDT.DbValue = RsRow("ModifiedDT")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Initialize urls
		ViewUrl = zPage.ViewUrl
		EditUrl = zPage.EditUrl
		InlineEditUrl = zPage.InlineEditUrl
		CopyUrl = zPage.CopyUrl
		InlineCopyUrl = zPage.InlineCopyUrl
		DeleteUrl = zPage.DeleteUrl

		' Row Rendering event
		zPage.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' ParentPageID

		zPage.ParentPageID.CellCssStyle = "white-space: nowrap;"
		zPage.ParentPageID.CellCssClass = ""
		zPage.ParentPageID.CellAttrs.Clear(): zPage.ParentPageID.ViewAttrs.Clear(): zPage.ParentPageID.EditAttrs.Clear()

		' PageName
		zPage.zPageName.CellCssStyle = "white-space: nowrap;"
		zPage.zPageName.CellCssClass = ""
		zPage.zPageName.CellAttrs.Clear(): zPage.zPageName.ViewAttrs.Clear(): zPage.zPageName.EditAttrs.Clear()

		' PageTitle
		zPage.PageTitle.CellCssStyle = "white-space: nowrap;"
		zPage.PageTitle.CellCssClass = ""
		zPage.PageTitle.CellAttrs.Clear(): zPage.PageTitle.ViewAttrs.Clear(): zPage.PageTitle.EditAttrs.Clear()

		' PageTypeID
		zPage.PageTypeID.CellCssStyle = "white-space: nowrap;"
		zPage.PageTypeID.CellCssClass = ""
		zPage.PageTypeID.CellAttrs.Clear(): zPage.PageTypeID.ViewAttrs.Clear(): zPage.PageTypeID.EditAttrs.Clear()

		' GroupID
		zPage.GroupID.CellCssStyle = "white-space: nowrap;"
		zPage.GroupID.CellCssClass = ""
		zPage.GroupID.CellAttrs.Clear(): zPage.GroupID.ViewAttrs.Clear(): zPage.GroupID.EditAttrs.Clear()

		' Active
		zPage.Active.CellCssStyle = "white-space: nowrap;"
		zPage.Active.CellCssClass = ""
		zPage.Active.CellAttrs.Clear(): zPage.Active.ViewAttrs.Clear(): zPage.Active.EditAttrs.Clear()

		' PageOrder
		zPage.PageOrder.CellCssStyle = "white-space: nowrap;"
		zPage.PageOrder.CellCssClass = ""
		zPage.PageOrder.CellAttrs.Clear(): zPage.PageOrder.ViewAttrs.Clear(): zPage.PageOrder.EditAttrs.Clear()

		'
		'  View  Row
		'

		If zPage.RowType = EW_ROWTYPE_VIEW Then ' View row

			' ParentPageID
			If ew_NotEmpty(zPage.ParentPageID.CurrentValue) Then
				sFilterWrk = "[PageID] = " & ew_AdjustSql(zPage.ParentPageID.CurrentValue) & ""
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

			' PageTypeID
			If ew_NotEmpty(zPage.PageTypeID.CurrentValue) Then
				sFilterWrk = "[PageTypeID] = " & ew_AdjustSql(zPage.PageTypeID.CurrentValue) & ""
			sSqlWrk = "SELECT [PageTypeCD] FROM [PageType]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [PageTypeCD] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					zPage.PageTypeID.ViewValue = RsWrk("PageTypeCD")
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

			' GroupID
			If ew_NotEmpty(zPage.GroupID.CurrentValue) Then
				sFilterWrk = "[GroupID] = " & ew_AdjustSql(zPage.GroupID.CurrentValue) & ""
			sSqlWrk = "SELECT [GroupName] FROM [Group]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [GroupName] Asc"
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

			' Active
			If Convert.ToString(zPage.Active.CurrentValue) = "1" Then
				zPage.Active.ViewValue = "Yes"
			Else
				zPage.Active.ViewValue = "No"
			End If
			zPage.Active.CssStyle = ""
			zPage.Active.CssClass = ""
			zPage.Active.ViewCustomAttributes = ""

			' PageOrder
			zPage.PageOrder.ViewValue = zPage.PageOrder.CurrentValue
			zPage.PageOrder.CssStyle = ""
			zPage.PageOrder.CssClass = ""
			zPage.PageOrder.ViewCustomAttributes = ""

			' CompanyID
			If ew_NotEmpty(zPage.CompanyID.CurrentValue) Then
				sFilterWrk = "[CompanyID] = " & ew_AdjustSql(zPage.CompanyID.CurrentValue) & ""
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

			' PageFileName
			zPage.PageFileName.ViewValue = zPage.PageFileName.CurrentValue
			zPage.PageFileName.CssStyle = ""
			zPage.PageFileName.CssClass = ""
			zPage.PageFileName.ViewCustomAttributes = ""

			' AllowMessage
			If Convert.ToString(zPage.AllowMessage.CurrentValue) = "1" Then
				zPage.AllowMessage.ViewValue = "Yes"
			Else
				zPage.AllowMessage.ViewValue = "No"
			End If
			zPage.AllowMessage.CssStyle = ""
			zPage.AllowMessage.CssClass = ""
			zPage.AllowMessage.ViewCustomAttributes = ""

			' SiteCategoryID
			zPage.SiteCategoryID.ViewValue = zPage.SiteCategoryID.CurrentValue
			zPage.SiteCategoryID.CssStyle = ""
			zPage.SiteCategoryID.CssClass = ""
			zPage.SiteCategoryID.ViewCustomAttributes = ""

			' SiteCategoryGroupID
			zPage.SiteCategoryGroupID.ViewValue = zPage.SiteCategoryGroupID.CurrentValue
			zPage.SiteCategoryGroupID.CssStyle = ""
			zPage.SiteCategoryGroupID.CssClass = ""
			zPage.SiteCategoryGroupID.ViewCustomAttributes = ""

			' VersionNo
			zPage.VersionNo.ViewValue = zPage.VersionNo.CurrentValue
			zPage.VersionNo.CssStyle = ""
			zPage.VersionNo.CssClass = ""
			zPage.VersionNo.ViewCustomAttributes = ""

			' ModifiedDT
			zPage.ModifiedDT.ViewValue = zPage.ModifiedDT.CurrentValue
			zPage.ModifiedDT.CssStyle = ""
			zPage.ModifiedDT.CssClass = ""
			zPage.ModifiedDT.ViewCustomAttributes = ""

			' View refer script
			' ParentPageID

			zPage.ParentPageID.HrefValue = ""
			zPage.ParentPageID.TooltipValue = ""

			' PageName
			zPage.zPageName.HrefValue = ""
			zPage.zPageName.TooltipValue = ""

			' PageTitle
			zPage.PageTitle.HrefValue = ""
			zPage.PageTitle.TooltipValue = ""

			' PageTypeID
			zPage.PageTypeID.HrefValue = ""
			zPage.PageTypeID.TooltipValue = ""

			' GroupID
			zPage.GroupID.HrefValue = ""
			zPage.GroupID.TooltipValue = ""

			' Active
			zPage.Active.HrefValue = ""
			zPage.Active.TooltipValue = ""

			' PageOrder
			zPage.PageOrder.HrefValue = ""
			zPage.PageOrder.TooltipValue = ""
		End If

		' Row Rendered event
		If zPage.RowType <> EW_ROWTYPE_AGGREGATEINIT Then
			zPage.Row_Rendered()
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
		zPage.ParentPageID.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_ParentPageID")
		zPage.zPageName.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_zPageName")
		zPage.PageTitle.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_PageTitle")
		zPage.PageTypeID.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_PageTypeID")
		zPage.GroupID.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_GroupID")
		zPage.Active.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_Active")
		zPage.PageOrder.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_PageOrder")
		zPage.CompanyID.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_CompanyID")
		zPage.PageDescription.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_PageDescription")
		zPage.PageKeywords.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_PageKeywords")
		zPage.ImagesPerRow.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_ImagesPerRow")
		zPage.RowsPerPage.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_RowsPerPage")
		zPage.PageFileName.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_PageFileName")
		zPage.AllowMessage.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_AllowMessage")
		zPage.SiteCategoryID.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_SiteCategoryID")
		zPage.SiteCategoryGroupID.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_SiteCategoryGroupID")
		zPage.VersionNo.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_VersionNo")
		zPage.ModifiedDT.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_ModifiedDT")
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
			oXmlDoc = New cXMLDocument()
		Else
			sOutputStr = ew_ExportHeader(zPage.Export)

			' Horizontal format, write header
			If sExportStyle <> "v" OrElse zPage.Export = "csv" Then
				sExportStr = ""
				ew_ExportAddValue(sExportStr, zPage.ParentPageID.ExportCaption, zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.ParentPageID.CellStyles, ""))
				ew_ExportAddValue(sExportStr, zPage.zPageName.ExportCaption, zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.zPageName.CellStyles, ""))
				ew_ExportAddValue(sExportStr, zPage.PageTitle.ExportCaption, zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.PageTitle.CellStyles, ""))
				ew_ExportAddValue(sExportStr, zPage.PageTypeID.ExportCaption, zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.PageTypeID.CellStyles, ""))
				ew_ExportAddValue(sExportStr, zPage.GroupID.ExportCaption, zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.GroupID.CellStyles, ""))
				ew_ExportAddValue(sExportStr, zPage.Active.ExportCaption, zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.Active.CellStyles, ""))
				ew_ExportAddValue(sExportStr, zPage.PageOrder.ExportCaption, zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.PageOrder.CellStyles, ""))
				ew_ExportAddValue(sExportStr, zPage.CompanyID.ExportCaption, zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.CompanyID.CellStyles, ""))
				ew_ExportAddValue(sExportStr, zPage.PageDescription.ExportCaption, zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.PageDescription.CellStyles, ""))
				ew_ExportAddValue(sExportStr, zPage.PageKeywords.ExportCaption, zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.PageKeywords.CellStyles, ""))
				ew_ExportAddValue(sExportStr, zPage.ImagesPerRow.ExportCaption, zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.ImagesPerRow.CellStyles, ""))
				ew_ExportAddValue(sExportStr, zPage.RowsPerPage.ExportCaption, zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.RowsPerPage.CellStyles, ""))
				ew_ExportAddValue(sExportStr, zPage.PageFileName.ExportCaption, zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.PageFileName.CellStyles, ""))
				ew_ExportAddValue(sExportStr, zPage.AllowMessage.ExportCaption, zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.AllowMessage.CellStyles, ""))
				ew_ExportAddValue(sExportStr, zPage.SiteCategoryID.ExportCaption, zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.SiteCategoryID.CellStyles, ""))
				ew_ExportAddValue(sExportStr, zPage.SiteCategoryGroupID.ExportCaption, zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.SiteCategoryGroupID.CellStyles, ""))
				ew_ExportAddValue(sExportStr, zPage.VersionNo.ExportCaption, zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.VersionNo.CellStyles, ""))
				ew_ExportAddValue(sExportStr, zPage.ModifiedDT.ExportCaption, zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.ModifiedDT.CellStyles, ""))
				sOutputStr &= ew_ExportLine(sExportStr, zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.RowStyles, ""))
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
				zPage.CssClass = ""
				zPage.CssStyle = ""
				zPage.RowType = EW_ROWTYPE_VIEW ' Render view
				RenderRow()
				If zPage.Export = "xml" Then
					oXmlDoc.AddRow()
					oXmlDoc.AddField("ParentPageID", zPage.ParentPageID.ExportValue(zPage.Export, zPage.ExportOriginalValue)) ' ParentPageID
					oXmlDoc.AddField("zPageName", zPage.zPageName.ExportValue(zPage.Export, zPage.ExportOriginalValue)) ' PageName
					oXmlDoc.AddField("PageTitle", zPage.PageTitle.ExportValue(zPage.Export, zPage.ExportOriginalValue)) ' PageTitle
					oXmlDoc.AddField("PageTypeID", zPage.PageTypeID.ExportValue(zPage.Export, zPage.ExportOriginalValue)) ' PageTypeID
					oXmlDoc.AddField("GroupID", zPage.GroupID.ExportValue(zPage.Export, zPage.ExportOriginalValue)) ' GroupID
					oXmlDoc.AddField("Active", zPage.Active.ExportValue(zPage.Export, zPage.ExportOriginalValue)) ' Active
					oXmlDoc.AddField("PageOrder", zPage.PageOrder.ExportValue(zPage.Export, zPage.ExportOriginalValue)) ' PageOrder
					oXmlDoc.AddField("CompanyID", zPage.CompanyID.ExportValue(zPage.Export, zPage.ExportOriginalValue)) ' CompanyID
					oXmlDoc.AddField("PageDescription", zPage.PageDescription.ExportValue(zPage.Export, zPage.ExportOriginalValue)) ' PageDescription
					oXmlDoc.AddField("PageKeywords", zPage.PageKeywords.ExportValue(zPage.Export, zPage.ExportOriginalValue)) ' PageKeywords
					oXmlDoc.AddField("ImagesPerRow", zPage.ImagesPerRow.ExportValue(zPage.Export, zPage.ExportOriginalValue)) ' ImagesPerRow
					oXmlDoc.AddField("RowsPerPage", zPage.RowsPerPage.ExportValue(zPage.Export, zPage.ExportOriginalValue)) ' RowsPerPage
					oXmlDoc.AddField("PageFileName", zPage.PageFileName.ExportValue(zPage.Export, zPage.ExportOriginalValue)) ' PageFileName
					oXmlDoc.AddField("AllowMessage", zPage.AllowMessage.ExportValue(zPage.Export, zPage.ExportOriginalValue)) ' AllowMessage
					oXmlDoc.AddField("SiteCategoryID", zPage.SiteCategoryID.ExportValue(zPage.Export, zPage.ExportOriginalValue)) ' SiteCategoryID
					oXmlDoc.AddField("SiteCategoryGroupID", zPage.SiteCategoryGroupID.ExportValue(zPage.Export, zPage.ExportOriginalValue)) ' SiteCategoryGroupID
					oXmlDoc.AddField("VersionNo", zPage.VersionNo.ExportValue(zPage.Export, zPage.ExportOriginalValue)) ' VersionNo
					oXmlDoc.AddField("ModifiedDT", zPage.ModifiedDT.ExportValue(zPage.Export, zPage.ExportOriginalValue)) ' ModifiedDT
				Else

					' Vertical format
					If sExportStyle = "v" AndAlso zPage.Export <> "csv" Then
						sOutputStr &= ew_ExportField(zPage.ParentPageID.ExportCaption, zPage.ParentPageID.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.ParentPageID.CellStyles, "")) ' ParentPageID
						sOutputStr &= ew_ExportField(zPage.zPageName.ExportCaption, zPage.zPageName.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.zPageName.CellStyles, "")) ' PageName
						sOutputStr &= ew_ExportField(zPage.PageTitle.ExportCaption, zPage.PageTitle.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.PageTitle.CellStyles, "")) ' PageTitle
						sOutputStr &= ew_ExportField(zPage.PageTypeID.ExportCaption, zPage.PageTypeID.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.PageTypeID.CellStyles, "")) ' PageTypeID
						sOutputStr &= ew_ExportField(zPage.GroupID.ExportCaption, zPage.GroupID.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.GroupID.CellStyles, "")) ' GroupID
						sOutputStr &= ew_ExportField(zPage.Active.ExportCaption, zPage.Active.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.Active.CellStyles, "")) ' Active
						sOutputStr &= ew_ExportField(zPage.PageOrder.ExportCaption, zPage.PageOrder.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.PageOrder.CellStyles, "")) ' PageOrder
						sOutputStr &= ew_ExportField(zPage.CompanyID.ExportCaption, zPage.CompanyID.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.CompanyID.CellStyles, "")) ' CompanyID
						sOutputStr &= ew_ExportField(zPage.PageDescription.ExportCaption, zPage.PageDescription.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.PageDescription.CellStyles, "")) ' PageDescription
						sOutputStr &= ew_ExportField(zPage.PageKeywords.ExportCaption, zPage.PageKeywords.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.PageKeywords.CellStyles, "")) ' PageKeywords
						sOutputStr &= ew_ExportField(zPage.ImagesPerRow.ExportCaption, zPage.ImagesPerRow.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.ImagesPerRow.CellStyles, "")) ' ImagesPerRow
						sOutputStr &= ew_ExportField(zPage.RowsPerPage.ExportCaption, zPage.RowsPerPage.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.RowsPerPage.CellStyles, "")) ' RowsPerPage
						sOutputStr &= ew_ExportField(zPage.PageFileName.ExportCaption, zPage.PageFileName.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.PageFileName.CellStyles, "")) ' PageFileName
						sOutputStr &= ew_ExportField(zPage.AllowMessage.ExportCaption, zPage.AllowMessage.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.AllowMessage.CellStyles, "")) ' AllowMessage
						sOutputStr &= ew_ExportField(zPage.SiteCategoryID.ExportCaption, zPage.SiteCategoryID.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.SiteCategoryID.CellStyles, "")) ' SiteCategoryID
						sOutputStr &= ew_ExportField(zPage.SiteCategoryGroupID.ExportCaption, zPage.SiteCategoryGroupID.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.SiteCategoryGroupID.CellStyles, "")) ' SiteCategoryGroupID
						sOutputStr &= ew_ExportField(zPage.VersionNo.ExportCaption, zPage.VersionNo.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.VersionNo.CellStyles, "")) ' VersionNo
						sOutputStr &= ew_ExportField(zPage.ModifiedDT.ExportCaption, zPage.ModifiedDT.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.ModifiedDT.CellStyles, "")) ' ModifiedDT

					' Horizontal format
					Else
						sExportStr = ""
						ew_ExportAddValue(sExportStr, zPage.ParentPageID.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.ParentPageID.CellStyles, ""))
						ew_ExportAddValue(sExportStr, zPage.zPageName.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.zPageName.CellStyles, ""))
						ew_ExportAddValue(sExportStr, zPage.PageTitle.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.PageTitle.CellStyles, ""))
						ew_ExportAddValue(sExportStr, zPage.PageTypeID.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.PageTypeID.CellStyles, ""))
						ew_ExportAddValue(sExportStr, zPage.GroupID.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.GroupID.CellStyles, ""))
						ew_ExportAddValue(sExportStr, zPage.Active.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.Active.CellStyles, ""))
						ew_ExportAddValue(sExportStr, zPage.PageOrder.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.PageOrder.CellStyles, ""))
						ew_ExportAddValue(sExportStr, zPage.CompanyID.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.CompanyID.CellStyles, ""))
						ew_ExportAddValue(sExportStr, zPage.PageDescription.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.PageDescription.CellStyles, ""))
						ew_ExportAddValue(sExportStr, zPage.PageKeywords.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.PageKeywords.CellStyles, ""))
						ew_ExportAddValue(sExportStr, zPage.ImagesPerRow.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.ImagesPerRow.CellStyles, ""))
						ew_ExportAddValue(sExportStr, zPage.RowsPerPage.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.RowsPerPage.CellStyles, ""))
						ew_ExportAddValue(sExportStr, zPage.PageFileName.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.PageFileName.CellStyles, ""))
						ew_ExportAddValue(sExportStr, zPage.AllowMessage.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.AllowMessage.CellStyles, ""))
						ew_ExportAddValue(sExportStr, zPage.SiteCategoryID.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.SiteCategoryID.CellStyles, ""))
						ew_ExportAddValue(sExportStr, zPage.SiteCategoryGroupID.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.SiteCategoryGroupID.CellStyles, ""))
						ew_ExportAddValue(sExportStr, zPage.VersionNo.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.VersionNo.CellStyles, ""))
						ew_ExportAddValue(sExportStr, zPage.ModifiedDT.ExportValue(zPage.Export, zPage.ExportOriginalValue), zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.ModifiedDT.CellStyles, ""))
						sOutputStr &= ew_ExportLine(sExportStr, zPage.Export, IIf(EW_EXPORT_CSS_STYLES, zPage.RowStyles, ""))
					End If
				End If
			End If
		Loop

		' Close recordset
		Rs.Close()
		Rs.Dispose()
		If zPage.Export = "xml" Then
			oXmlDoc.Output
		Else
			sOutputStr &= ew_ExportFooter(zPage.Export)
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
				sDbMasterFilter = zPage.SqlMasterFilter_Company
				sDbDetailFilter = zPage.SqlDetailFilter_Company
				If ew_Get("CompanyID") <> "" Then
					Company.CompanyID.QueryStringValue = ew_Get("CompanyID")
					zPage.CompanyID.QueryStringValue = Company.CompanyID.QueryStringValue
					zPage.CompanyID.SessionValue = zPage.CompanyID.QueryStringValue
					If Not IsNumeric(Company.CompanyID.QueryStringValue) Then bValidMaster = False
					sDbMasterFilter = sDbMasterFilter.Replace("@CompanyID@", ew_AdjustSql(Company.CompanyID.QueryStringValue))
					sDbDetailFilter = sDbDetailFilter.Replace("@CompanyID@", ew_AdjustSql(Company.CompanyID.QueryStringValue))
				Else
					bValidMaster = False
				End If
			End If
		End If
		If bValidMaster Then

			' Save current master table
			zPage.CurrentMasterTable = sMasterTblVar

			' Reset start record counter (new master key)
			lStartRec = 1
			zPage.StartRecordNumber = lStartRec
			zPage.MasterFilter = sDbMasterFilter ' Set up master filter
			zPage.DetailFilter = sDbDetailFilter ' Set up detail filter

			' Clear previous master session values
			If sMasterTblVar <> "Company" Then
				If zPage.CompanyID.QueryStringValue = "" Then zPage.CompanyID.SessionValue = ""
			End If
		End If
	End Sub

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "Page"
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
		zPage_list = New czPage_list(Me)		
		zPage_list.Page_Init()		

		' Set buffer/cache option
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

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
