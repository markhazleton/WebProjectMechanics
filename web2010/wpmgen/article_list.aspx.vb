Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class article_list
	Inherits AspNetMaker8_wpmWebsite

	' Page object
	Public Article_list As cArticle_list

	'
	' Page Class
	'
	Class cArticle_list
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
				If Article.UseTokenInUrl Then Url = Url & "t=" & Article.TableVar & "&" ' Add page token
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
			If Article.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (Article.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (Article.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' ASP.NET page object
		Public ReadOnly Property AspNetPage() As article_list
			Get
				Return CType(m_ParentPage, article_list)
			End Get
		End Property

		' Article
		Public Property Article() As cArticle
			Get				
				Return ParentPage.Article
			End Get
			Set(ByVal v As cArticle)
				ParentPage.Article = v	
			End Set	
		End Property

		' Article
		Public Property Company() As cCompany
			Get				
				Return ParentPage.Company
			End Get
			Set(ByVal v As cCompany)
				ParentPage.Company = v	
			End Set	
		End Property

		' Article
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
		Public Sub New(ByRef APage As AspNetMaker8_wpmWebsite)				
			m_ParentPage = APage
			m_Page = Me
			m_PageID = "list"
			m_PageObjName = "Article_list"
			m_PageObjTypeName = "cArticle_list"			

			' Initialize language object
			Language = New cLanguage(Me)

			' Table Name
			m_TableName = "Article"

			' Initialize table object
			Article = New cArticle(Me)
			Company = New cCompany(Me)
			zPage = New czPage(Me)

			' Initialize URLs
			ExportPrintUrl = PageUrl & "export=print"
			ExportExcelUrl = PageUrl & "export=excel"
			ExportWordUrl = PageUrl & "export=word"
			ExportHtmlUrl = PageUrl & "export=html"
			ExportXmlUrl = PageUrl & "export=xml"
			ExportCsvUrl = PageUrl & "export=csv"
			AddUrl = Article.AddUrl
			InlineAddUrl = PageUrl & "a=add"
			GridAddUrl = PageUrl & "a=gridadd"
			GridEditUrl = PageUrl & "a=gridedit"
			MultiDeleteUrl = "article_delete.aspx"
			MultiUpdateUrl = "article_update.aspx"

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
				Article.Export = ew_Get("export")
			ElseIf ew_Post("exporttype") <> "" Then
				Article.Export = ew_Post("exporttype")
			Else
				Article.ExportReturnUrl = ew_CurrentUrl()
			End If
			ParentPage.gsExport = Article.Export ' Get export parameter, used in header
			ParentPage.gsExportFile = Article.TableVar ' Get export file, used in header
			If Article.Export = "excel" Then
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
			Article.Dispose()
			Company.Dispose()
			zPage.Dispose()
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
			Call Article.Recordset_SearchValidated()

			' Set Up Sorting Order
			SetUpSortOrder()

			' Get search criteria for advanced search
			If ParentPage.gsSearchError = "" Then
				sSrchAdvanced = AdvancedSearchWhere()
			End If
		End If

		' Restore display records
		If (Article.RecordsPerPage = -1 OrElse Article.RecordsPerPage > 0) Then
			lDisplayRecs = Article.RecordsPerPage ' Restore from Session
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
		Article.Recordset_Searching(sSrchWhere)

		' Save search criteria
		If sSrchWhere <> "" Then
			If sSrchAdvanced = "" Then ResetAdvancedSearchParms()
			Article.SearchWhere = sSrchWhere ' Save to Session
			If RestoreSearch Then
				lStartRec = 1 ' Reset start record counter
				Article.StartRecordNumber = lStartRec
			End If
		Else
			sSrchWhere = Article.SearchWhere
		End If

		' Build filter
		sFilter = ""

		' Restore master/detail filter
		sDbMasterFilter = Article.MasterFilter ' Restore master filter
		sDbDetailFilter = Article.DetailFilter ' Restore detail filter
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
		If Article.MasterFilter <> "" AndAlso Article.CurrentMasterTable = "Company" Then
			RsMaster = Company.LoadRs(sDbMasterFilter)
			bMasterRecordExists = RsMaster IsNot Nothing
			If Not bMasterRecordExists Then
				Article.MasterFilter = "" ' Clear master filter
				Article.DetailFilter = "" ' Clear detail filter
				Message = Language.Phrase("NoRecord") ' Set no record found
			Page_Terminate(Article.ReturnUrl) ' Return to caller
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
		If Article.MasterFilter <> "" AndAlso Article.CurrentMasterTable = "zPage" Then
			RsMaster = zPage.LoadRs(sDbMasterFilter)
			bMasterRecordExists = RsMaster IsNot Nothing
			If Not bMasterRecordExists Then
				Article.MasterFilter = "" ' Clear master filter
				Article.DetailFilter = "" ' Clear detail filter
				Message = Language.Phrase("NoRecord") ' Set no record found
			Page_Terminate(Article.ReturnUrl) ' Return to caller
			Else
				RsMaster.Read()
				zPage.LoadListRowValues(RsMaster)
				zPage.RowType = EW_ROWTYPE_MASTER ' Master row
				zPage.RenderListRow()
				RsMaster.Close()
				RsMaster.Dispose()
			End If
		End If

		' Set up filter in Session
		Article.SessionWhere = sFilter
		Article.CurrentFilter = ""

		' Export Data only
		If Article.Export = "html" OrElse Article.Export = "csv" OrElse Article.Export = "word" OrElse Article.Export = "excel" OrElse Article.Export = "xml" Then
			ExportData()
			Page_Terminate("") ' Clean up
			ew_End() ' Terminate response
		ElseIf Article.Export = "email" Then
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
			Article.RecordsPerPage = lDisplayRecs ' Save to Session

			' Reset start position
			lStartRec = 1
			Article.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	' Return Advanced Search WHERE based on QueryString parameters
	'
	Function AdvancedSearchWhere() As String		
		Dim sWhere As String = ""
		BuildSearchSql(sWhere, Article.ArticleID, False) ' ArticleID
		BuildSearchSql(sWhere, Article.Active, False) ' Active
		BuildSearchSql(sWhere, Article.StartDT, False) ' StartDT
		BuildSearchSql(sWhere, Article.Title, False) ' Title
		BuildSearchSql(sWhere, Article.CompanyID, False) ' CompanyID
		BuildSearchSql(sWhere, Article.zPageID, False) ' PageID
		BuildSearchSql(sWhere, Article.userID, False) ' userID
		BuildSearchSql(sWhere, Article.Description, False) ' Description
		BuildSearchSql(sWhere, Article.ArticleSummary, False) ' ArticleSummary
		BuildSearchSql(sWhere, Article.ArticleBody, False) ' ArticleBody
		BuildSearchSql(sWhere, Article.EndDT, False) ' EndDT
		BuildSearchSql(sWhere, Article.ExpireDT, False) ' ExpireDT
		BuildSearchSql(sWhere, Article.Author, False) ' Author
		BuildSearchSql(sWhere, Article.Counter, False) ' Counter
		BuildSearchSql(sWhere, Article.VersionNo, False) ' VersionNo
		BuildSearchSql(sWhere, Article.ContactID, False) ' ContactID
		BuildSearchSql(sWhere, Article.ModifiedDT, False) ' ModifiedDT

		' Set up search parm
		If sWhere <> "" Then
			SetSearchParm(Article.ArticleID) ' ArticleID
			SetSearchParm(Article.Active) ' Active
			SetSearchParm(Article.StartDT) ' StartDT
			SetSearchParm(Article.Title) ' Title
			SetSearchParm(Article.CompanyID) ' CompanyID
			SetSearchParm(Article.zPageID) ' PageID
			SetSearchParm(Article.userID) ' userID
			SetSearchParm(Article.Description) ' Description
			SetSearchParm(Article.ArticleSummary) ' ArticleSummary
			SetSearchParm(Article.ArticleBody) ' ArticleBody
			SetSearchParm(Article.EndDT) ' EndDT
			SetSearchParm(Article.ExpireDT) ' ExpireDT
			SetSearchParm(Article.Author) ' Author
			SetSearchParm(Article.Counter) ' Counter
			SetSearchParm(Article.VersionNo) ' VersionNo
			SetSearchParm(Article.ContactID) ' ContactID
			SetSearchParm(Article.ModifiedDT) ' ModifiedDT
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
		Article.SetAdvancedSearch("x_" & FldParm, Fld.AdvancedSearch.SearchValue)
		Article.SetAdvancedSearch("z_" & FldParm, Fld.AdvancedSearch.SearchOperator)
		Article.SetAdvancedSearch("v_" & FldParm, Fld.AdvancedSearch.SearchCondition)
		Article.SetAdvancedSearch("y_" & FldParm, Fld.AdvancedSearch.SearchValue2)
		Article.SetAdvancedSearch("w_" & FldParm, Fld.AdvancedSearch.SearchOperator2)
	End Sub

	'
	' Get search parm
	'
	Sub GetSearchParm(ByRef Fld As Object)
		Dim FldParm As String = Fld.FldVar.Substring(2)
		Fld.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_" & FldParm)
		Fld.AdvancedSearch.SearchOperator = Article.GetAdvancedSearch("z_" & FldParm)
		Fld.AdvancedSearch.SearchCondition = Article.GetAdvancedSearch("v_" & FldParm)
		Fld.AdvancedSearch.SearchValue2 = Article.GetAdvancedSearch("y_" & FldParm)
		Fld.AdvancedSearch.SearchOperator2 = Article.GetAdvancedSearch("w_" & FldParm)
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
		Article.SearchWhere = sSrchWhere

		' Clear advanced search parameters
		ResetAdvancedSearchParms()
	End Sub

	'
	' Clear all advanced search parameters
	'
	Sub ResetAdvancedSearchParms()
		Article.SetAdvancedSearch("x_ArticleID", "")
		Article.SetAdvancedSearch("x_Active", "")
		Article.SetAdvancedSearch("x_StartDT", "")
		Article.SetAdvancedSearch("x_Title", "")
		Article.SetAdvancedSearch("x_CompanyID", "")
		Article.SetAdvancedSearch("x_zPageID", "")
		Article.SetAdvancedSearch("x_userID", "")
		Article.SetAdvancedSearch("x_Description", "")
		Article.SetAdvancedSearch("x_ArticleSummary", "")
		Article.SetAdvancedSearch("x_ArticleBody", "")
		Article.SetAdvancedSearch("x_EndDT", "")
		Article.SetAdvancedSearch("x_ExpireDT", "")
		Article.SetAdvancedSearch("x_Author", "")
		Article.SetAdvancedSearch("x_Counter", "")
		Article.SetAdvancedSearch("x_VersionNo", "")
		Article.SetAdvancedSearch("x_ContactID", "")
		Article.SetAdvancedSearch("x_ModifiedDT", "")
	End Sub

	'
	' Restore all search parameters
	'
	Sub RestoreSearchParms()
		Dim bRestore As Boolean = True
		If ew_Get("x_ArticleID") <> "" Then bRestore = False
		If ew_Get("x_Active") <> "" Then bRestore = False
		If ew_Get("x_StartDT") <> "" Then bRestore = False
		If ew_Get("x_Title") <> "" Then bRestore = False
		If ew_Get("x_CompanyID") <> "" Then bRestore = False
		If ew_Get("x_zPageID") <> "" Then bRestore = False
		If ew_Get("x_userID") <> "" Then bRestore = False
		If ew_Get("x_Description") <> "" Then bRestore = False
		If ew_Get("x_ArticleSummary") <> "" Then bRestore = False
		If ew_Get("x_ArticleBody") <> "" Then bRestore = False
		If ew_Get("x_EndDT") <> "" Then bRestore = False
		If ew_Get("x_ExpireDT") <> "" Then bRestore = False
		If ew_Get("x_Author") <> "" Then bRestore = False
		If ew_Get("x_Counter") <> "" Then bRestore = False
		If ew_Get("x_VersionNo") <> "" Then bRestore = False
		If ew_Get("x_ContactID") <> "" Then bRestore = False
		If ew_Get("x_ModifiedDT") <> "" Then bRestore = False
		RestoreSearch = bRestore
		If bRestore Then

			' Restore advanced search values
			Call GetSearchParm(Article.ArticleID)
			Call GetSearchParm(Article.Active)
			Call GetSearchParm(Article.StartDT)
			Call GetSearchParm(Article.Title)
			Call GetSearchParm(Article.CompanyID)
			Call GetSearchParm(Article.zPageID)
			Call GetSearchParm(Article.userID)
			Call GetSearchParm(Article.Description)
			Call GetSearchParm(Article.ArticleSummary)
			Call GetSearchParm(Article.ArticleBody)
			Call GetSearchParm(Article.EndDT)
			Call GetSearchParm(Article.ExpireDT)
			Call GetSearchParm(Article.Author)
			Call GetSearchParm(Article.Counter)
			Call GetSearchParm(Article.VersionNo)
			Call GetSearchParm(Article.ContactID)
			Call GetSearchParm(Article.ModifiedDT)
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
			Article.CurrentOrder = ew_Get("order")
			Article.CurrentOrderType = ew_Get("ordertype")
			Article.UpdateSort(Article.Active) ' Active
			Article.UpdateSort(Article.Title) ' Title
			Article.UpdateSort(Article.zPageID) ' PageID
			Article.StartRecordNumber = 1 ' Reset start position
		End If
	End Sub

	'
	' Load Sort Order parameters
	'
	Sub LoadSortOrder()
		Dim sOrderBy As String
		sOrderBy = Article.SessionOrderBy ' Get order by from Session
		If sOrderBy = "" Then
			If Article.SqlOrderBy <> "" Then
				sOrderBy = Article.SqlOrderBy
				Article.SessionOrderBy = sOrderBy
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
				Article.CurrentMasterTable = "" ' Clear master table
				Article.MasterFilter = "" ' Clear master filter
				sDbMasterFilter = ""
				Article.DetailFilter = "" ' Clear detail filter
				sDbDetailFilter = ""
				Article.CompanyID.SessionValue = ""
				Article.zPageID.SessionValue = ""
			End If

			' Reset sort criteria
			If ew_SameText(sCmd, "resetsort") Then
				Dim sOrderBy As String = ""
				Article.SessionOrderBy = sOrderBy
				Article.Active.Sort = ""
				Article.Title.Sort = ""
				Article.zPageID.Sort = ""
			End If

			' Reset start position
			lStartRec = 1
			Article.StartRecordNumber = lStartRec
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
		If Article.Export <> "" Or Article.CurrentAction = "gridadd" Or Article.CurrentAction = "gridedit" Then
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
				Article.StartRecordNumber = lStartRec
			ElseIf ew_Get(EW_TABLE_PAGE_NO) <> "" AndAlso IsNumeric(ew_Get(EW_TABLE_PAGE_NO)) Then
				nPageNo = ew_ConvertToInt(ew_Get(EW_TABLE_PAGE_NO))
				lStartRec = (nPageNo-1)*lDisplayRecs+1
				If lStartRec <= 0 Then
					lStartRec = 1
				ElseIf lStartRec >= ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 Then
					lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1
				End If
				Article.StartRecordNumber = lStartRec
			End If
		End If
		lStartRec = Article.StartRecordNumber

		' Check if correct start record counter
		If lStartRec <= 0 Then ' Avoid invalid start record counter
			lStartRec = 1 ' Reset start record counter
			Article.StartRecordNumber = lStartRec
		ElseIf lStartRec > lTotalRecs Then ' Avoid starting record > total records
			lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to last page first record
			Article.StartRecordNumber = lStartRec
		ElseIf (lStartRec-1) Mod lDisplayRecs <> 0 Then
			lStartRec = ((lStartRec-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to page boundary
			Article.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	' Load default values
	'
	Sub LoadDefaultValues()
		Article.Active.CurrentValue = 1
		Article.zPageID.CurrentValue = 0
	End Sub

	'
	'  Load search values for validation
	'
	Sub LoadSearchValues()
		Article.ArticleID.AdvancedSearch.SearchValue = ew_Get("x_ArticleID")
    	Article.ArticleID.AdvancedSearch.SearchOperator = ew_Get("z_ArticleID")
		Article.Active.AdvancedSearch.SearchValue = ew_Get("x_Active")
    	Article.Active.AdvancedSearch.SearchOperator = ew_Get("z_Active")
		Article.StartDT.AdvancedSearch.SearchValue = ew_Get("x_StartDT")
    	Article.StartDT.AdvancedSearch.SearchOperator = ew_Get("z_StartDT")
		Article.Title.AdvancedSearch.SearchValue = ew_Get("x_Title")
    	Article.Title.AdvancedSearch.SearchOperator = ew_Get("z_Title")
		Article.CompanyID.AdvancedSearch.SearchValue = ew_Get("x_CompanyID")
    	Article.CompanyID.AdvancedSearch.SearchOperator = ew_Get("z_CompanyID")
		Article.zPageID.AdvancedSearch.SearchValue = ew_Get("x_zPageID")
    	Article.zPageID.AdvancedSearch.SearchOperator = ew_Get("z_zPageID")
		Article.userID.AdvancedSearch.SearchValue = ew_Get("x_userID")
    	Article.userID.AdvancedSearch.SearchOperator = ew_Get("z_userID")
		Article.Description.AdvancedSearch.SearchValue = ew_Get("x_Description")
    	Article.Description.AdvancedSearch.SearchOperator = ew_Get("z_Description")
		Article.ArticleSummary.AdvancedSearch.SearchValue = ew_Get("x_ArticleSummary")
    	Article.ArticleSummary.AdvancedSearch.SearchOperator = ew_Get("z_ArticleSummary")
		Article.ArticleBody.AdvancedSearch.SearchValue = ew_Get("x_ArticleBody")
    	Article.ArticleBody.AdvancedSearch.SearchOperator = ew_Get("z_ArticleBody")
		Article.EndDT.AdvancedSearch.SearchValue = ew_Get("x_EndDT")
    	Article.EndDT.AdvancedSearch.SearchOperator = ew_Get("z_EndDT")
		Article.ExpireDT.AdvancedSearch.SearchValue = ew_Get("x_ExpireDT")
    	Article.ExpireDT.AdvancedSearch.SearchOperator = ew_Get("z_ExpireDT")
		Article.Author.AdvancedSearch.SearchValue = ew_Get("x_Author")
    	Article.Author.AdvancedSearch.SearchOperator = ew_Get("z_Author")
		Article.Counter.AdvancedSearch.SearchValue = ew_Get("x_Counter")
    	Article.Counter.AdvancedSearch.SearchOperator = ew_Get("z_Counter")
		Article.VersionNo.AdvancedSearch.SearchValue = ew_Get("x_VersionNo")
    	Article.VersionNo.AdvancedSearch.SearchOperator = ew_Get("z_VersionNo")
		Article.ContactID.AdvancedSearch.SearchValue = ew_Get("x_ContactID")
    	Article.ContactID.AdvancedSearch.SearchOperator = ew_Get("z_ContactID")
		Article.ModifiedDT.AdvancedSearch.SearchValue = ew_Get("x_ModifiedDT")
    	Article.ModifiedDT.AdvancedSearch.SearchOperator = ew_Get("z_ModifiedDT")
	End Sub

	'
	' Load recordset
	'
	Function LoadRecordset() As OleDbDataReader

		' Recordset Selecting event
		Article.Recordset_Selecting(Article.CurrentFilter)

		' Load list page SQL
		Dim sSql As String = Article.ListSQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then DebugMsg = sSql

		' Count
		lTotalRecs = -1		
		Try
			Dim sCntSql As String = Article.SelectCountSQL

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
		Article.Recordset_Selected(Rs)
		Return Rs
	End Function

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = Article.KeyFilter

		' Row Selecting event
		Article.Row_Selecting(sFilter)

		' Load SQL based on filter
		Article.CurrentFilter = sFilter
		Dim sSql As String = Article.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then DebugMsg = sSql
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				Article.Row_Selected(RsRow)
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
		Article.ArticleID.DbValue = RsRow("ArticleID")
		Article.Active.DbValue = IIf(ew_ConvertToBool(RsRow("Active")), "1", "0")
		Article.StartDT.DbValue = RsRow("StartDT")
		Article.Title.DbValue = RsRow("Title")
		Article.CompanyID.DbValue = RsRow("CompanyID")
		Article.zPageID.DbValue = RsRow("PageID")
		Article.userID.DbValue = RsRow("userID")
		Article.Description.DbValue = RsRow("Description")
		Article.ArticleSummary.DbValue = RsRow("ArticleSummary")
		Article.ArticleBody.DbValue = RsRow("ArticleBody")
		Article.EndDT.DbValue = RsRow("EndDT")
		Article.ExpireDT.DbValue = RsRow("ExpireDT")
		Article.Author.DbValue = RsRow("Author")
		Article.Counter.DbValue = RsRow("Counter")
		Article.VersionNo.DbValue = RsRow("VersionNo")
		Article.ContactID.DbValue = RsRow("ContactID")
		Article.ModifiedDT.DbValue = RsRow("ModifiedDT")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Initialize urls
		ViewUrl = Article.ViewUrl
		EditUrl = Article.EditUrl
		InlineEditUrl = Article.InlineEditUrl
		CopyUrl = Article.CopyUrl
		InlineCopyUrl = Article.InlineCopyUrl
		DeleteUrl = Article.DeleteUrl

		' Row Rendering event
		Article.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' Active

		Article.Active.CellCssStyle = "white-space: nowrap;"
		Article.Active.CellCssClass = ""
		Article.Active.CellAttrs.Clear(): Article.Active.ViewAttrs.Clear(): Article.Active.EditAttrs.Clear()

		' Title
		Article.Title.CellCssStyle = "white-space: nowrap;"
		Article.Title.CellCssClass = ""
		Article.Title.CellAttrs.Clear(): Article.Title.ViewAttrs.Clear(): Article.Title.EditAttrs.Clear()

		' PageID
		Article.zPageID.CellCssStyle = "white-space: nowrap;"
		Article.zPageID.CellCssClass = ""
		Article.zPageID.CellAttrs.Clear(): Article.zPageID.ViewAttrs.Clear(): Article.zPageID.EditAttrs.Clear()

		'
		'  View  Row
		'

		If Article.RowType = EW_ROWTYPE_VIEW Then ' View row

			' ArticleID
			Article.ArticleID.ViewValue = Article.ArticleID.CurrentValue
			Article.ArticleID.CssStyle = ""
			Article.ArticleID.CssClass = ""
			Article.ArticleID.ViewCustomAttributes = ""

			' Active
			If Convert.ToString(Article.Active.CurrentValue) = "1" Then
				Article.Active.ViewValue = "Yes"
			Else
				Article.Active.ViewValue = "No"
			End If
			Article.Active.CssStyle = ""
			Article.Active.CssClass = ""
			Article.Active.ViewCustomAttributes = ""

			' StartDT
			Article.StartDT.ViewValue = Article.StartDT.CurrentValue
			Article.StartDT.CssStyle = ""
			Article.StartDT.CssClass = ""
			Article.StartDT.ViewCustomAttributes = ""

			' Title
			Article.Title.ViewValue = Article.Title.CurrentValue
			Article.Title.CssStyle = ""
			Article.Title.CssClass = ""
			Article.Title.ViewCustomAttributes = ""

			' CompanyID
			If ew_NotEmpty(Article.CompanyID.CurrentValue) Then
				sFilterWrk = "[CompanyID] = " & ew_AdjustSql(Article.CompanyID.CurrentValue) & ""
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
					Article.CompanyID.ViewValue = RsWrk("CompanyName")
				Else
					Article.CompanyID.ViewValue = Article.CompanyID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Article.CompanyID.ViewValue = System.DBNull.Value
			End If
			Article.CompanyID.CssStyle = ""
			Article.CompanyID.CssClass = ""
			Article.CompanyID.ViewCustomAttributes = ""

			' PageID
			If ew_NotEmpty(Article.zPageID.CurrentValue) Then
				sFilterWrk = "[PageID] = " & ew_AdjustSql(Article.zPageID.CurrentValue) & ""
			sSqlWrk = "SELECT [PageName] FROM [Page]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sWhereWrk &= " AND "
			sWhereWrk &= "(" & "[CompanyID]=" & HttpContext.Current.Session("CompanyID") & "" & ")"
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [PageName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Article.zPageID.ViewValue = RsWrk("PageName")
				Else
					Article.zPageID.ViewValue = Article.zPageID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Article.zPageID.ViewValue = System.DBNull.Value
			End If
			Article.zPageID.CssStyle = ""
			Article.zPageID.CssClass = ""
			Article.zPageID.ViewCustomAttributes = ""

			' userID
			If ew_NotEmpty(Article.userID.CurrentValue) Then
				sFilterWrk = "[ContactID] = " & ew_AdjustSql(Article.userID.CurrentValue) & ""
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
					Article.userID.ViewValue = RsWrk("PrimaryContact")
				Else
					Article.userID.ViewValue = Article.userID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Article.userID.ViewValue = System.DBNull.Value
			End If
			Article.userID.CssStyle = ""
			Article.userID.CssClass = ""
			Article.userID.ViewCustomAttributes = ""

			' Description
			Article.Description.ViewValue = Article.Description.CurrentValue
			Article.Description.CssStyle = ""
			Article.Description.CssClass = ""
			Article.Description.ViewCustomAttributes = ""

			' EndDT
			Article.EndDT.ViewValue = Article.EndDT.CurrentValue
			Article.EndDT.CssStyle = ""
			Article.EndDT.CssClass = ""
			Article.EndDT.ViewCustomAttributes = ""

			' ExpireDT
			Article.ExpireDT.ViewValue = Article.ExpireDT.CurrentValue
			Article.ExpireDT.CssStyle = ""
			Article.ExpireDT.CssClass = ""
			Article.ExpireDT.ViewCustomAttributes = ""

			' Author
			If ew_NotEmpty(Article.Author.CurrentValue) Then
				sFilterWrk = "[PrimaryContact] = '" & ew_AdjustSql(Article.Author.CurrentValue) & "'"
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
					Article.Author.ViewValue = RsWrk("PrimaryContact")
				Else
					Article.Author.ViewValue = Article.Author.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Article.Author.ViewValue = System.DBNull.Value
			End If
			Article.Author.CssStyle = ""
			Article.Author.CssClass = ""
			Article.Author.ViewCustomAttributes = ""

			' Counter
			Article.Counter.ViewValue = Article.Counter.CurrentValue
			Article.Counter.CssStyle = ""
			Article.Counter.CssClass = ""
			Article.Counter.ViewCustomAttributes = ""

			' VersionNo
			Article.VersionNo.ViewValue = Article.VersionNo.CurrentValue
			Article.VersionNo.CssStyle = ""
			Article.VersionNo.CssClass = ""
			Article.VersionNo.ViewCustomAttributes = ""

			' ContactID
			Article.ContactID.ViewValue = Article.ContactID.CurrentValue
			Article.ContactID.CssStyle = ""
			Article.ContactID.CssClass = ""
			Article.ContactID.ViewCustomAttributes = ""

			' ModifiedDT
			Article.ModifiedDT.ViewValue = Article.ModifiedDT.CurrentValue
			Article.ModifiedDT.CssStyle = ""
			Article.ModifiedDT.CssClass = ""
			Article.ModifiedDT.ViewCustomAttributes = ""

			' View refer script
			' Active

			Article.Active.HrefValue = ""
			Article.Active.TooltipValue = ""

			' Title
			Article.Title.HrefValue = ""
			Article.Title.TooltipValue = ""

			' PageID
			Article.zPageID.HrefValue = ""
			Article.zPageID.TooltipValue = ""
		End If

		' Row Rendered event
		If Article.RowType <> EW_ROWTYPE_AGGREGATEINIT Then
			Article.Row_Rendered()
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
		Article.ArticleID.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_ArticleID")
		Article.Active.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_Active")
		Article.StartDT.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_StartDT")
		Article.Title.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_Title")
		Article.CompanyID.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_CompanyID")
		Article.zPageID.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_zPageID")
		Article.userID.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_userID")
		Article.Description.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_Description")
		Article.ArticleSummary.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_ArticleSummary")
		Article.ArticleBody.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_ArticleBody")
		Article.EndDT.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_EndDT")
		Article.ExpireDT.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_ExpireDT")
		Article.Author.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_Author")
		Article.Counter.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_Counter")
		Article.VersionNo.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_VersionNo")
		Article.ContactID.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_ContactID")
		Article.ModifiedDT.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_ModifiedDT")
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
		If Article.ExportAll Then
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
		If Article.Export = "xml" Then
			oXmlDoc = New cXMLDocument()
		Else
			sOutputStr = ew_ExportHeader(Article.Export)

			' Horizontal format, write header
			If sExportStyle <> "v" OrElse Article.Export = "csv" Then
				sExportStr = ""
				ew_ExportAddValue(sExportStr, Article.ArticleID.ExportCaption, Article.Export, IIf(EW_EXPORT_CSS_STYLES, Article.ArticleID.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Article.Active.ExportCaption, Article.Export, IIf(EW_EXPORT_CSS_STYLES, Article.Active.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Article.StartDT.ExportCaption, Article.Export, IIf(EW_EXPORT_CSS_STYLES, Article.StartDT.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Article.Title.ExportCaption, Article.Export, IIf(EW_EXPORT_CSS_STYLES, Article.Title.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Article.CompanyID.ExportCaption, Article.Export, IIf(EW_EXPORT_CSS_STYLES, Article.CompanyID.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Article.zPageID.ExportCaption, Article.Export, IIf(EW_EXPORT_CSS_STYLES, Article.zPageID.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Article.userID.ExportCaption, Article.Export, IIf(EW_EXPORT_CSS_STYLES, Article.userID.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Article.Description.ExportCaption, Article.Export, IIf(EW_EXPORT_CSS_STYLES, Article.Description.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Article.EndDT.ExportCaption, Article.Export, IIf(EW_EXPORT_CSS_STYLES, Article.EndDT.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Article.ExpireDT.ExportCaption, Article.Export, IIf(EW_EXPORT_CSS_STYLES, Article.ExpireDT.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Article.Author.ExportCaption, Article.Export, IIf(EW_EXPORT_CSS_STYLES, Article.Author.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Article.Counter.ExportCaption, Article.Export, IIf(EW_EXPORT_CSS_STYLES, Article.Counter.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Article.VersionNo.ExportCaption, Article.Export, IIf(EW_EXPORT_CSS_STYLES, Article.VersionNo.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Article.ContactID.ExportCaption, Article.Export, IIf(EW_EXPORT_CSS_STYLES, Article.ContactID.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Article.ModifiedDT.ExportCaption, Article.Export, IIf(EW_EXPORT_CSS_STYLES, Article.ModifiedDT.CellStyles, ""))
				sOutputStr &= ew_ExportLine(sExportStr, Article.Export, IIf(EW_EXPORT_CSS_STYLES, Article.RowStyles, ""))
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
				Article.CssClass = ""
				Article.CssStyle = ""
				Article.RowType = EW_ROWTYPE_VIEW ' Render view
				RenderRow()
				If Article.Export = "xml" Then
					oXmlDoc.AddRow()
					oXmlDoc.AddField("ArticleID", Article.ArticleID.ExportValue(Article.Export, Article.ExportOriginalValue)) ' ArticleID
					oXmlDoc.AddField("Active", Article.Active.ExportValue(Article.Export, Article.ExportOriginalValue)) ' Active
					oXmlDoc.AddField("StartDT", Article.StartDT.ExportValue(Article.Export, Article.ExportOriginalValue)) ' StartDT
					oXmlDoc.AddField("Title", Article.Title.ExportValue(Article.Export, Article.ExportOriginalValue)) ' Title
					oXmlDoc.AddField("CompanyID", Article.CompanyID.ExportValue(Article.Export, Article.ExportOriginalValue)) ' CompanyID
					oXmlDoc.AddField("zPageID", Article.zPageID.ExportValue(Article.Export, Article.ExportOriginalValue)) ' PageID
					oXmlDoc.AddField("userID", Article.userID.ExportValue(Article.Export, Article.ExportOriginalValue)) ' userID
					oXmlDoc.AddField("Description", Article.Description.ExportValue(Article.Export, Article.ExportOriginalValue)) ' Description
					oXmlDoc.AddField("EndDT", Article.EndDT.ExportValue(Article.Export, Article.ExportOriginalValue)) ' EndDT
					oXmlDoc.AddField("ExpireDT", Article.ExpireDT.ExportValue(Article.Export, Article.ExportOriginalValue)) ' ExpireDT
					oXmlDoc.AddField("Author", Article.Author.ExportValue(Article.Export, Article.ExportOriginalValue)) ' Author
					oXmlDoc.AddField("Counter", Article.Counter.ExportValue(Article.Export, Article.ExportOriginalValue)) ' Counter
					oXmlDoc.AddField("VersionNo", Article.VersionNo.ExportValue(Article.Export, Article.ExportOriginalValue)) ' VersionNo
					oXmlDoc.AddField("ContactID", Article.ContactID.ExportValue(Article.Export, Article.ExportOriginalValue)) ' ContactID
					oXmlDoc.AddField("ModifiedDT", Article.ModifiedDT.ExportValue(Article.Export, Article.ExportOriginalValue)) ' ModifiedDT
				Else

					' Vertical format
					If sExportStyle = "v" AndAlso Article.Export <> "csv" Then
						sOutputStr &= ew_ExportField(Article.ArticleID.ExportCaption, Article.ArticleID.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export, IIf(EW_EXPORT_CSS_STYLES, Article.ArticleID.CellStyles, "")) ' ArticleID
						sOutputStr &= ew_ExportField(Article.Active.ExportCaption, Article.Active.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export, IIf(EW_EXPORT_CSS_STYLES, Article.Active.CellStyles, "")) ' Active
						sOutputStr &= ew_ExportField(Article.StartDT.ExportCaption, Article.StartDT.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export, IIf(EW_EXPORT_CSS_STYLES, Article.StartDT.CellStyles, "")) ' StartDT
						sOutputStr &= ew_ExportField(Article.Title.ExportCaption, Article.Title.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export, IIf(EW_EXPORT_CSS_STYLES, Article.Title.CellStyles, "")) ' Title
						sOutputStr &= ew_ExportField(Article.CompanyID.ExportCaption, Article.CompanyID.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export, IIf(EW_EXPORT_CSS_STYLES, Article.CompanyID.CellStyles, "")) ' CompanyID
						sOutputStr &= ew_ExportField(Article.zPageID.ExportCaption, Article.zPageID.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export, IIf(EW_EXPORT_CSS_STYLES, Article.zPageID.CellStyles, "")) ' PageID
						sOutputStr &= ew_ExportField(Article.userID.ExportCaption, Article.userID.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export, IIf(EW_EXPORT_CSS_STYLES, Article.userID.CellStyles, "")) ' userID
						sOutputStr &= ew_ExportField(Article.Description.ExportCaption, Article.Description.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export, IIf(EW_EXPORT_CSS_STYLES, Article.Description.CellStyles, "")) ' Description
						sOutputStr &= ew_ExportField(Article.EndDT.ExportCaption, Article.EndDT.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export, IIf(EW_EXPORT_CSS_STYLES, Article.EndDT.CellStyles, "")) ' EndDT
						sOutputStr &= ew_ExportField(Article.ExpireDT.ExportCaption, Article.ExpireDT.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export, IIf(EW_EXPORT_CSS_STYLES, Article.ExpireDT.CellStyles, "")) ' ExpireDT
						sOutputStr &= ew_ExportField(Article.Author.ExportCaption, Article.Author.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export, IIf(EW_EXPORT_CSS_STYLES, Article.Author.CellStyles, "")) ' Author
						sOutputStr &= ew_ExportField(Article.Counter.ExportCaption, Article.Counter.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export, IIf(EW_EXPORT_CSS_STYLES, Article.Counter.CellStyles, "")) ' Counter
						sOutputStr &= ew_ExportField(Article.VersionNo.ExportCaption, Article.VersionNo.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export, IIf(EW_EXPORT_CSS_STYLES, Article.VersionNo.CellStyles, "")) ' VersionNo
						sOutputStr &= ew_ExportField(Article.ContactID.ExportCaption, Article.ContactID.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export, IIf(EW_EXPORT_CSS_STYLES, Article.ContactID.CellStyles, "")) ' ContactID
						sOutputStr &= ew_ExportField(Article.ModifiedDT.ExportCaption, Article.ModifiedDT.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export, IIf(EW_EXPORT_CSS_STYLES, Article.ModifiedDT.CellStyles, "")) ' ModifiedDT

					' Horizontal format
					Else
						sExportStr = ""
						ew_ExportAddValue(sExportStr, Article.ArticleID.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export, IIf(EW_EXPORT_CSS_STYLES, Article.ArticleID.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Article.Active.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export, IIf(EW_EXPORT_CSS_STYLES, Article.Active.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Article.StartDT.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export, IIf(EW_EXPORT_CSS_STYLES, Article.StartDT.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Article.Title.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export, IIf(EW_EXPORT_CSS_STYLES, Article.Title.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Article.CompanyID.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export, IIf(EW_EXPORT_CSS_STYLES, Article.CompanyID.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Article.zPageID.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export, IIf(EW_EXPORT_CSS_STYLES, Article.zPageID.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Article.userID.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export, IIf(EW_EXPORT_CSS_STYLES, Article.userID.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Article.Description.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export, IIf(EW_EXPORT_CSS_STYLES, Article.Description.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Article.EndDT.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export, IIf(EW_EXPORT_CSS_STYLES, Article.EndDT.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Article.ExpireDT.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export, IIf(EW_EXPORT_CSS_STYLES, Article.ExpireDT.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Article.Author.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export, IIf(EW_EXPORT_CSS_STYLES, Article.Author.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Article.Counter.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export, IIf(EW_EXPORT_CSS_STYLES, Article.Counter.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Article.VersionNo.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export, IIf(EW_EXPORT_CSS_STYLES, Article.VersionNo.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Article.ContactID.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export, IIf(EW_EXPORT_CSS_STYLES, Article.ContactID.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Article.ModifiedDT.ExportValue(Article.Export, Article.ExportOriginalValue), Article.Export, IIf(EW_EXPORT_CSS_STYLES, Article.ModifiedDT.CellStyles, ""))
						sOutputStr &= ew_ExportLine(sExportStr, Article.Export, IIf(EW_EXPORT_CSS_STYLES, Article.RowStyles, ""))
					End If
				End If
			End If
		Loop

		' Close recordset
		Rs.Close()
		Rs.Dispose()
		If Article.Export = "xml" Then
			oXmlDoc.Output
		Else
			sOutputStr &= ew_ExportFooter(Article.Export)
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
				sDbMasterFilter = Article.SqlMasterFilter_Company
				sDbDetailFilter = Article.SqlDetailFilter_Company
				If ew_Get("CompanyID") <> "" Then
					Company.CompanyID.QueryStringValue = ew_Get("CompanyID")
					Article.CompanyID.QueryStringValue = Company.CompanyID.QueryStringValue
					Article.CompanyID.SessionValue = Article.CompanyID.QueryStringValue
					If Not IsNumeric(Company.CompanyID.QueryStringValue) Then bValidMaster = False
					sDbMasterFilter = sDbMasterFilter.Replace("@CompanyID@", ew_AdjustSql(Company.CompanyID.QueryStringValue))
					sDbDetailFilter = sDbDetailFilter.Replace("@CompanyID@", ew_AdjustSql(Company.CompanyID.QueryStringValue))
				Else
					bValidMaster = False
				End If
			End If
			If sMasterTblVar = "zPage" Then
				bValidMaster = True
				sDbMasterFilter = Article.SqlMasterFilter_zPage
				sDbDetailFilter = Article.SqlDetailFilter_zPage
				If ew_Get("zPageID") <> "" Then
					zPage.zPageID.QueryStringValue = ew_Get("zPageID")
					Article.zPageID.QueryStringValue = zPage.zPageID.QueryStringValue
					Article.zPageID.SessionValue = Article.zPageID.QueryStringValue
					If Not IsNumeric(zPage.zPageID.QueryStringValue) Then bValidMaster = False
					sDbMasterFilter = sDbMasterFilter.Replace("@zPageID@", ew_AdjustSql(zPage.zPageID.QueryStringValue))
					sDbDetailFilter = sDbDetailFilter.Replace("@zPageID@", ew_AdjustSql(zPage.zPageID.QueryStringValue))
				Else
					bValidMaster = False
				End If
			End If
		End If
		If bValidMaster Then

			' Save current master table
			Article.CurrentMasterTable = sMasterTblVar

			' Reset start record counter (new master key)
			lStartRec = 1
			Article.StartRecordNumber = lStartRec
			Article.MasterFilter = sDbMasterFilter ' Set up master filter
			Article.DetailFilter = sDbDetailFilter ' Set up detail filter

			' Clear previous master session values
			If sMasterTblVar <> "Company" Then
				If Article.CompanyID.QueryStringValue = "" Then Article.CompanyID.SessionValue = ""
			End If
			If sMasterTblVar <> "zPage" Then
				If Article.zPageID.QueryStringValue = "" Then Article.zPageID.SessionValue = ""
			End If
		End If
	End Sub

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "Article"
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
		Article_list = New cArticle_list(Me)		
		Article_list.Page_Init()		

		' Set buffer/cache option
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

		' Page main processing
		Article_list.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If Article_list IsNot Nothing Then Article_list.Dispose()
	End Sub
End Class
