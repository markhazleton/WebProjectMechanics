Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class pagealias_list
	Inherits AspNetMaker8_wpmWebsite

	' Page object
	Public PageAlias_list As cPageAlias_list

	'
	' Page Class
	'
	Class cPageAlias_list
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
				If PageAlias.UseTokenInUrl Then Url = Url & "t=" & PageAlias.TableVar & "&" ' Add page token
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
			If PageAlias.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (PageAlias.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (PageAlias.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' ASP.NET page object
		Public ReadOnly Property AspNetPage() As pagealias_list
			Get
				Return CType(m_ParentPage, pagealias_list)
			End Get
		End Property

		' PageAlias
		Public Property PageAlias() As cPageAlias
			Get				
				Return ParentPage.PageAlias
			End Get
			Set(ByVal v As cPageAlias)
				ParentPage.PageAlias = v	
			End Set	
		End Property

		' PageAlias
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
			m_PageObjName = "PageAlias_list"
			m_PageObjTypeName = "cPageAlias_list"			

			' Initialize language object
			Language = New cLanguage(Me)

			' Table Name
			m_TableName = "PageAlias"

			' Initialize table object
			PageAlias = New cPageAlias(Me)
			Company = New cCompany(Me)

			' Initialize URLs
			ExportPrintUrl = PageUrl & "export=print"
			ExportExcelUrl = PageUrl & "export=excel"
			ExportWordUrl = PageUrl & "export=word"
			ExportHtmlUrl = PageUrl & "export=html"
			ExportXmlUrl = PageUrl & "export=xml"
			ExportCsvUrl = PageUrl & "export=csv"
			AddUrl = PageAlias.AddUrl
			InlineAddUrl = PageUrl & "a=add"
			GridAddUrl = PageUrl & "a=gridadd"
			GridEditUrl = PageUrl & "a=gridedit"
			MultiDeleteUrl = "pagealias_delete.aspx"
			MultiUpdateUrl = "pagealias_update.aspx"

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
				PageAlias.Export = ew_Get("export")
			ElseIf ew_Post("exporttype") <> "" Then
				PageAlias.Export = ew_Post("exporttype")
			Else
				PageAlias.ExportReturnUrl = ew_CurrentUrl()
			End If
			ParentPage.gsExport = PageAlias.Export ' Get export parameter, used in header
			ParentPage.gsExportFile = PageAlias.TableVar ' Get export file, used in header
			If PageAlias.Export = "excel" Then
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
			PageAlias.Dispose()
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
			Call PageAlias.Recordset_SearchValidated()

			' Set Up Sorting Order
			SetUpSortOrder()

			' Get search criteria for advanced search
			If ParentPage.gsSearchError = "" Then
				sSrchAdvanced = AdvancedSearchWhere()
			End If
		End If

		' Restore display records
		If (PageAlias.RecordsPerPage = -1 OrElse PageAlias.RecordsPerPage > 0) Then
			lDisplayRecs = PageAlias.RecordsPerPage ' Restore from Session
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
		PageAlias.Recordset_Searching(sSrchWhere)

		' Save search criteria
		If sSrchWhere <> "" Then
			If sSrchAdvanced = "" Then ResetAdvancedSearchParms()
			PageAlias.SearchWhere = sSrchWhere ' Save to Session
			If RestoreSearch Then
				lStartRec = 1 ' Reset start record counter
				PageAlias.StartRecordNumber = lStartRec
			End If
		Else
			sSrchWhere = PageAlias.SearchWhere
		End If

		' Build filter
		sFilter = ""

		' Restore master/detail filter
		sDbMasterFilter = PageAlias.MasterFilter ' Restore master filter
		sDbDetailFilter = PageAlias.DetailFilter ' Restore detail filter
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
		If PageAlias.MasterFilter <> "" AndAlso PageAlias.CurrentMasterTable = "Company" Then
			RsMaster = Company.LoadRs(sDbMasterFilter)
			bMasterRecordExists = RsMaster IsNot Nothing
			If Not bMasterRecordExists Then
				PageAlias.MasterFilter = "" ' Clear master filter
				PageAlias.DetailFilter = "" ' Clear detail filter
				Message = Language.Phrase("NoRecord") ' Set no record found
			Page_Terminate(PageAlias.ReturnUrl) ' Return to caller
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
		PageAlias.SessionWhere = sFilter
		PageAlias.CurrentFilter = ""

		' Export Data only
		If PageAlias.Export = "html" OrElse PageAlias.Export = "csv" OrElse PageAlias.Export = "word" OrElse PageAlias.Export = "excel" OrElse PageAlias.Export = "xml" Then
			ExportData()
			Page_Terminate("") ' Clean up
			ew_End() ' Terminate response
		ElseIf PageAlias.Export = "email" Then
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
			PageAlias.RecordsPerPage = lDisplayRecs ' Save to Session

			' Reset start position
			lStartRec = 1
			PageAlias.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	' Return Advanced Search WHERE based on QueryString parameters
	'
	Function AdvancedSearchWhere() As String		
		Dim sWhere As String = ""
		BuildSearchSql(sWhere, PageAlias.zPageURL, False) ' PageURL
		BuildSearchSql(sWhere, PageAlias.TargetURL, False) ' TargetURL
		BuildSearchSql(sWhere, PageAlias.AliasType, False) ' AliasType
		BuildSearchSql(sWhere, PageAlias.CompanyID, False) ' CompanyID

		' Set up search parm
		If sWhere <> "" Then
			SetSearchParm(PageAlias.zPageURL) ' PageURL
			SetSearchParm(PageAlias.TargetURL) ' TargetURL
			SetSearchParm(PageAlias.AliasType) ' AliasType
			SetSearchParm(PageAlias.CompanyID) ' CompanyID
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
		PageAlias.SetAdvancedSearch("x_" & FldParm, Fld.AdvancedSearch.SearchValue)
		PageAlias.SetAdvancedSearch("z_" & FldParm, Fld.AdvancedSearch.SearchOperator)
		PageAlias.SetAdvancedSearch("v_" & FldParm, Fld.AdvancedSearch.SearchCondition)
		PageAlias.SetAdvancedSearch("y_" & FldParm, Fld.AdvancedSearch.SearchValue2)
		PageAlias.SetAdvancedSearch("w_" & FldParm, Fld.AdvancedSearch.SearchOperator2)
	End Sub

	'
	' Get search parm
	'
	Sub GetSearchParm(ByRef Fld As Object)
		Dim FldParm As String = Fld.FldVar.Substring(2)
		Fld.AdvancedSearch.SearchValue = PageAlias.GetAdvancedSearch("x_" & FldParm)
		Fld.AdvancedSearch.SearchOperator = PageAlias.GetAdvancedSearch("z_" & FldParm)
		Fld.AdvancedSearch.SearchCondition = PageAlias.GetAdvancedSearch("v_" & FldParm)
		Fld.AdvancedSearch.SearchValue2 = PageAlias.GetAdvancedSearch("y_" & FldParm)
		Fld.AdvancedSearch.SearchOperator2 = PageAlias.GetAdvancedSearch("w_" & FldParm)
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
		PageAlias.SearchWhere = sSrchWhere

		' Clear advanced search parameters
		ResetAdvancedSearchParms()
	End Sub

	'
	' Clear all advanced search parameters
	'
	Sub ResetAdvancedSearchParms()
		PageAlias.SetAdvancedSearch("x_zPageURL", "")
		PageAlias.SetAdvancedSearch("x_TargetURL", "")
		PageAlias.SetAdvancedSearch("x_AliasType", "")
		PageAlias.SetAdvancedSearch("x_CompanyID", "")
	End Sub

	'
	' Restore all search parameters
	'
	Sub RestoreSearchParms()
		Dim bRestore As Boolean = True
		If ew_Get("x_zPageURL") <> "" Then bRestore = False
		If ew_Get("x_TargetURL") <> "" Then bRestore = False
		If ew_Get("x_AliasType") <> "" Then bRestore = False
		If ew_Get("x_CompanyID") <> "" Then bRestore = False
		RestoreSearch = bRestore
		If bRestore Then

			' Restore advanced search values
			Call GetSearchParm(PageAlias.zPageURL)
			Call GetSearchParm(PageAlias.TargetURL)
			Call GetSearchParm(PageAlias.AliasType)
			Call GetSearchParm(PageAlias.CompanyID)
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
			PageAlias.CurrentOrder = ew_Get("order")
			PageAlias.CurrentOrderType = ew_Get("ordertype")
			PageAlias.UpdateSort(PageAlias.zPageURL) ' PageURL
			PageAlias.UpdateSort(PageAlias.TargetURL) ' TargetURL
			PageAlias.UpdateSort(PageAlias.AliasType) ' AliasType
			PageAlias.StartRecordNumber = 1 ' Reset start position
		End If
	End Sub

	'
	' Load Sort Order parameters
	'
	Sub LoadSortOrder()
		Dim sOrderBy As String
		sOrderBy = PageAlias.SessionOrderBy ' Get order by from Session
		If sOrderBy = "" Then
			If PageAlias.SqlOrderBy <> "" Then
				sOrderBy = PageAlias.SqlOrderBy
				PageAlias.SessionOrderBy = sOrderBy
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
				PageAlias.CurrentMasterTable = "" ' Clear master table
				PageAlias.MasterFilter = "" ' Clear master filter
				sDbMasterFilter = ""
				PageAlias.DetailFilter = "" ' Clear detail filter
				sDbDetailFilter = ""
				PageAlias.CompanyID.SessionValue = ""
			End If

			' Reset sort criteria
			If ew_SameText(sCmd, "resetsort") Then
				Dim sOrderBy As String = ""
				PageAlias.SessionOrderBy = sOrderBy
				PageAlias.zPageURL.Sort = ""
				PageAlias.TargetURL.Sort = ""
				PageAlias.AliasType.Sort = ""
			End If

			' Reset start position
			lStartRec = 1
			PageAlias.StartRecordNumber = lStartRec
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
		If PageAlias.Export <> "" Or PageAlias.CurrentAction = "gridadd" Or PageAlias.CurrentAction = "gridedit" Then
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
				PageAlias.StartRecordNumber = lStartRec
			ElseIf ew_Get(EW_TABLE_PAGE_NO) <> "" AndAlso IsNumeric(ew_Get(EW_TABLE_PAGE_NO)) Then
				nPageNo = ew_ConvertToInt(ew_Get(EW_TABLE_PAGE_NO))
				lStartRec = (nPageNo-1)*lDisplayRecs+1
				If lStartRec <= 0 Then
					lStartRec = 1
				ElseIf lStartRec >= ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 Then
					lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1
				End If
				PageAlias.StartRecordNumber = lStartRec
			End If
		End If
		lStartRec = PageAlias.StartRecordNumber

		' Check if correct start record counter
		If lStartRec <= 0 Then ' Avoid invalid start record counter
			lStartRec = 1 ' Reset start record counter
			PageAlias.StartRecordNumber = lStartRec
		ElseIf lStartRec > lTotalRecs Then ' Avoid starting record > total records
			lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to last page first record
			PageAlias.StartRecordNumber = lStartRec
		ElseIf (lStartRec-1) Mod lDisplayRecs <> 0 Then
			lStartRec = ((lStartRec-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to page boundary
			PageAlias.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	' Load default values
	'
	Sub LoadDefaultValues()
		PageAlias.AliasType.CurrentValue = "301"
	End Sub

	'
	'  Load search values for validation
	'
	Sub LoadSearchValues()
		PageAlias.zPageURL.AdvancedSearch.SearchValue = ew_Get("x_zPageURL")
    	PageAlias.zPageURL.AdvancedSearch.SearchOperator = ew_Get("z_zPageURL")
		PageAlias.TargetURL.AdvancedSearch.SearchValue = ew_Get("x_TargetURL")
    	PageAlias.TargetURL.AdvancedSearch.SearchOperator = ew_Get("z_TargetURL")
		PageAlias.AliasType.AdvancedSearch.SearchValue = ew_Get("x_AliasType")
    	PageAlias.AliasType.AdvancedSearch.SearchOperator = ew_Get("z_AliasType")
		PageAlias.CompanyID.AdvancedSearch.SearchValue = ew_Get("x_CompanyID")
    	PageAlias.CompanyID.AdvancedSearch.SearchOperator = ew_Get("z_CompanyID")
	End Sub

	'
	' Load recordset
	'
	Function LoadRecordset() As OleDbDataReader

		' Recordset Selecting event
		PageAlias.Recordset_Selecting(PageAlias.CurrentFilter)

		' Load list page SQL
		Dim sSql As String = PageAlias.ListSQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then DebugMsg = sSql

		' Count
		lTotalRecs = -1		
		Try
			Dim sCntSql As String = PageAlias.SelectCountSQL

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
		PageAlias.Recordset_Selected(Rs)
		Return Rs
	End Function

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = PageAlias.KeyFilter

		' Row Selecting event
		PageAlias.Row_Selecting(sFilter)

		' Load SQL based on filter
		PageAlias.CurrentFilter = sFilter
		Dim sSql As String = PageAlias.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then DebugMsg = sSql
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				PageAlias.Row_Selected(RsRow)
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
		PageAlias.PageAliasID.DbValue = RsRow("PageAliasID")
		PageAlias.zPageURL.DbValue = RsRow("PageURL")
		PageAlias.TargetURL.DbValue = RsRow("TargetURL")
		PageAlias.AliasType.DbValue = RsRow("AliasType")
		PageAlias.CompanyID.DbValue = RsRow("CompanyID")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Initialize urls
		ViewUrl = PageAlias.ViewUrl
		EditUrl = PageAlias.EditUrl
		InlineEditUrl = PageAlias.InlineEditUrl
		CopyUrl = PageAlias.CopyUrl
		InlineCopyUrl = PageAlias.InlineCopyUrl
		DeleteUrl = PageAlias.DeleteUrl

		' Row Rendering event
		PageAlias.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' PageURL

		PageAlias.zPageURL.CellCssStyle = "white-space: nowrap;"
		PageAlias.zPageURL.CellCssClass = ""
		PageAlias.zPageURL.CellAttrs.Clear(): PageAlias.zPageURL.ViewAttrs.Clear(): PageAlias.zPageURL.EditAttrs.Clear()

		' TargetURL
		PageAlias.TargetURL.CellCssStyle = "white-space: nowrap;"
		PageAlias.TargetURL.CellCssClass = ""
		PageAlias.TargetURL.CellAttrs.Clear(): PageAlias.TargetURL.ViewAttrs.Clear(): PageAlias.TargetURL.EditAttrs.Clear()

		' AliasType
		PageAlias.AliasType.CellCssStyle = "white-space: nowrap;"
		PageAlias.AliasType.CellCssClass = ""
		PageAlias.AliasType.CellAttrs.Clear(): PageAlias.AliasType.ViewAttrs.Clear(): PageAlias.AliasType.EditAttrs.Clear()

		'
		'  View  Row
		'

		If PageAlias.RowType = EW_ROWTYPE_VIEW Then ' View row

			' PageAliasID
			PageAlias.PageAliasID.ViewValue = PageAlias.PageAliasID.CurrentValue
			PageAlias.PageAliasID.CssStyle = ""
			PageAlias.PageAliasID.CssClass = ""
			PageAlias.PageAliasID.ViewCustomAttributes = ""

			' PageURL
			PageAlias.zPageURL.ViewValue = PageAlias.zPageURL.CurrentValue
			PageAlias.zPageURL.CssStyle = ""
			PageAlias.zPageURL.CssClass = ""
			PageAlias.zPageURL.ViewCustomAttributes = ""

			' TargetURL
			PageAlias.TargetURL.ViewValue = PageAlias.TargetURL.CurrentValue
			PageAlias.TargetURL.CssStyle = ""
			PageAlias.TargetURL.CssClass = ""
			PageAlias.TargetURL.ViewCustomAttributes = ""

			' AliasType
			PageAlias.AliasType.ViewValue = PageAlias.AliasType.CurrentValue
			PageAlias.AliasType.CssStyle = ""
			PageAlias.AliasType.CssClass = ""
			PageAlias.AliasType.ViewCustomAttributes = ""

			' View refer script
			' PageURL

			PageAlias.zPageURL.HrefValue = ""
			PageAlias.zPageURL.TooltipValue = ""

			' TargetURL
			PageAlias.TargetURL.HrefValue = ""
			PageAlias.TargetURL.TooltipValue = ""

			' AliasType
			PageAlias.AliasType.HrefValue = ""
			PageAlias.AliasType.TooltipValue = ""
		End If

		' Row Rendered event
		If PageAlias.RowType <> EW_ROWTYPE_AGGREGATEINIT Then
			PageAlias.Row_Rendered()
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
		PageAlias.zPageURL.AdvancedSearch.SearchValue = PageAlias.GetAdvancedSearch("x_zPageURL")
		PageAlias.TargetURL.AdvancedSearch.SearchValue = PageAlias.GetAdvancedSearch("x_TargetURL")
		PageAlias.AliasType.AdvancedSearch.SearchValue = PageAlias.GetAdvancedSearch("x_AliasType")
		PageAlias.CompanyID.AdvancedSearch.SearchValue = PageAlias.GetAdvancedSearch("x_CompanyID")
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
		If PageAlias.ExportAll Then
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
		If PageAlias.Export = "xml" Then
			oXmlDoc = New cXMLDocument()
		Else
			sOutputStr = ew_ExportHeader(PageAlias.Export)

			' Horizontal format, write header
			If sExportStyle <> "v" OrElse PageAlias.Export = "csv" Then
				sExportStr = ""
				ew_ExportAddValue(sExportStr, PageAlias.PageAliasID.ExportCaption, PageAlias.Export, IIf(EW_EXPORT_CSS_STYLES, PageAlias.PageAliasID.CellStyles, ""))
				ew_ExportAddValue(sExportStr, PageAlias.zPageURL.ExportCaption, PageAlias.Export, IIf(EW_EXPORT_CSS_STYLES, PageAlias.zPageURL.CellStyles, ""))
				ew_ExportAddValue(sExportStr, PageAlias.TargetURL.ExportCaption, PageAlias.Export, IIf(EW_EXPORT_CSS_STYLES, PageAlias.TargetURL.CellStyles, ""))
				ew_ExportAddValue(sExportStr, PageAlias.AliasType.ExportCaption, PageAlias.Export, IIf(EW_EXPORT_CSS_STYLES, PageAlias.AliasType.CellStyles, ""))
				sOutputStr &= ew_ExportLine(sExportStr, PageAlias.Export, IIf(EW_EXPORT_CSS_STYLES, PageAlias.RowStyles, ""))
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
				PageAlias.CssClass = ""
				PageAlias.CssStyle = ""
				PageAlias.RowType = EW_ROWTYPE_VIEW ' Render view
				RenderRow()
				If PageAlias.Export = "xml" Then
					oXmlDoc.AddRow()
					oXmlDoc.AddField("PageAliasID", PageAlias.PageAliasID.ExportValue(PageAlias.Export, PageAlias.ExportOriginalValue)) ' PageAliasID
					oXmlDoc.AddField("zPageURL", PageAlias.zPageURL.ExportValue(PageAlias.Export, PageAlias.ExportOriginalValue)) ' PageURL
					oXmlDoc.AddField("TargetURL", PageAlias.TargetURL.ExportValue(PageAlias.Export, PageAlias.ExportOriginalValue)) ' TargetURL
					oXmlDoc.AddField("AliasType", PageAlias.AliasType.ExportValue(PageAlias.Export, PageAlias.ExportOriginalValue)) ' AliasType
				Else

					' Vertical format
					If sExportStyle = "v" AndAlso PageAlias.Export <> "csv" Then
						sOutputStr &= ew_ExportField(PageAlias.PageAliasID.ExportCaption, PageAlias.PageAliasID.ExportValue(PageAlias.Export, PageAlias.ExportOriginalValue), PageAlias.Export, IIf(EW_EXPORT_CSS_STYLES, PageAlias.PageAliasID.CellStyles, "")) ' PageAliasID
						sOutputStr &= ew_ExportField(PageAlias.zPageURL.ExportCaption, PageAlias.zPageURL.ExportValue(PageAlias.Export, PageAlias.ExportOriginalValue), PageAlias.Export, IIf(EW_EXPORT_CSS_STYLES, PageAlias.zPageURL.CellStyles, "")) ' PageURL
						sOutputStr &= ew_ExportField(PageAlias.TargetURL.ExportCaption, PageAlias.TargetURL.ExportValue(PageAlias.Export, PageAlias.ExportOriginalValue), PageAlias.Export, IIf(EW_EXPORT_CSS_STYLES, PageAlias.TargetURL.CellStyles, "")) ' TargetURL
						sOutputStr &= ew_ExportField(PageAlias.AliasType.ExportCaption, PageAlias.AliasType.ExportValue(PageAlias.Export, PageAlias.ExportOriginalValue), PageAlias.Export, IIf(EW_EXPORT_CSS_STYLES, PageAlias.AliasType.CellStyles, "")) ' AliasType

					' Horizontal format
					Else
						sExportStr = ""
						ew_ExportAddValue(sExportStr, PageAlias.PageAliasID.ExportValue(PageAlias.Export, PageAlias.ExportOriginalValue), PageAlias.Export, IIf(EW_EXPORT_CSS_STYLES, PageAlias.PageAliasID.CellStyles, ""))
						ew_ExportAddValue(sExportStr, PageAlias.zPageURL.ExportValue(PageAlias.Export, PageAlias.ExportOriginalValue), PageAlias.Export, IIf(EW_EXPORT_CSS_STYLES, PageAlias.zPageURL.CellStyles, ""))
						ew_ExportAddValue(sExportStr, PageAlias.TargetURL.ExportValue(PageAlias.Export, PageAlias.ExportOriginalValue), PageAlias.Export, IIf(EW_EXPORT_CSS_STYLES, PageAlias.TargetURL.CellStyles, ""))
						ew_ExportAddValue(sExportStr, PageAlias.AliasType.ExportValue(PageAlias.Export, PageAlias.ExportOriginalValue), PageAlias.Export, IIf(EW_EXPORT_CSS_STYLES, PageAlias.AliasType.CellStyles, ""))
						sOutputStr &= ew_ExportLine(sExportStr, PageAlias.Export, IIf(EW_EXPORT_CSS_STYLES, PageAlias.RowStyles, ""))
					End If
				End If
			End If
		Loop

		' Close recordset
		Rs.Close()
		Rs.Dispose()
		If PageAlias.Export = "xml" Then
			oXmlDoc.Output
		Else
			sOutputStr &= ew_ExportFooter(PageAlias.Export)
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
				sDbMasterFilter = PageAlias.SqlMasterFilter_Company
				sDbDetailFilter = PageAlias.SqlDetailFilter_Company
				If ew_Get("CompanyID") <> "" Then
					Company.CompanyID.QueryStringValue = ew_Get("CompanyID")
					PageAlias.CompanyID.QueryStringValue = Company.CompanyID.QueryStringValue
					PageAlias.CompanyID.SessionValue = PageAlias.CompanyID.QueryStringValue
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
			PageAlias.CurrentMasterTable = sMasterTblVar

			' Reset start record counter (new master key)
			lStartRec = 1
			PageAlias.StartRecordNumber = lStartRec
			PageAlias.MasterFilter = sDbMasterFilter ' Set up master filter
			PageAlias.DetailFilter = sDbDetailFilter ' Set up detail filter

			' Clear previous master session values
			If sMasterTblVar <> "Company" Then
				If PageAlias.CompanyID.QueryStringValue = "" Then PageAlias.CompanyID.SessionValue = ""
			End If
		End If
	End Sub

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "PageAlias"
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
		PageAlias_list = New cPageAlias_list(Me)		
		PageAlias_list.Page_Init()		

		' Set buffer/cache option
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

		' Page main processing
		PageAlias_list.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If PageAlias_list IsNot Nothing Then PageAlias_list.Dispose()
	End Sub
End Class
