Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class sitelink_list
	Inherits AspNetMaker8_wpmWebsite

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

		Private sFilterWrk As String

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

		' ASP.NET page object
		Public ReadOnly Property AspNetPage() As sitelink_list
			Get
				Return CType(m_ParentPage, sitelink_list)
			End Get
		End Property

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
		Public Sub New(ByRef APage As AspNetMaker8_wpmWebsite)				
			m_ParentPage = APage
			m_Page = Me
			m_PageID = "list"
			m_PageObjName = "SiteLink_list"
			m_PageObjTypeName = "cSiteLink_list"			

			' Initialize language object
			Language = New cLanguage(Me)

			' Table Name
			m_TableName = "SiteLink"

			' Initialize table object
			SiteLink = New cSiteLink(Me)

			' Initialize URLs
			ExportPrintUrl = PageUrl & "export=print"
			ExportExcelUrl = PageUrl & "export=excel"
			ExportWordUrl = PageUrl & "export=word"
			ExportHtmlUrl = PageUrl & "export=html"
			ExportXmlUrl = PageUrl & "export=xml"
			ExportCsvUrl = PageUrl & "export=csv"
			AddUrl = SiteLink.AddUrl
			InlineAddUrl = PageUrl & "a=add"
			GridAddUrl = PageUrl & "a=gridadd"
			GridEditUrl = PageUrl & "a=gridedit"
			MultiDeleteUrl = "sitelink_delete.aspx"
			MultiUpdateUrl = "sitelink_update.aspx"

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
				SiteLink.Export = ew_Get("export")
			ElseIf ew_Post("exporttype") <> "" Then
				SiteLink.Export = ew_Post("exporttype")
			Else
				SiteLink.ExportReturnUrl = ew_CurrentUrl()
			End If
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
			Call SiteLink.Recordset_SearchValidated()

			' Set Up Sorting Order
			SetUpSortOrder()

			' Get search criteria for advanced search
			If ParentPage.gsSearchError = "" Then
				sSrchAdvanced = AdvancedSearchWhere()
			End If
		End If

		' Restore display records
		If (SiteLink.RecordsPerPage = -1 OrElse SiteLink.RecordsPerPage > 0) Then
			lDisplayRecs = SiteLink.RecordsPerPage ' Restore from Session
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
		SiteLink.Recordset_Searching(sSrchWhere)

		' Save search criteria
		If sSrchWhere <> "" Then
			If sSrchAdvanced = "" Then ResetAdvancedSearchParms()
			SiteLink.SearchWhere = sSrchWhere ' Save to Session
			If RestoreSearch Then
				lStartRec = 1 ' Reset start record counter
				SiteLink.StartRecordNumber = lStartRec
			End If
		Else
			sSrchWhere = SiteLink.SearchWhere
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
		SiteLink.SessionWhere = sFilter
		SiteLink.CurrentFilter = ""

		' Export Data only
		If SiteLink.Export = "html" OrElse SiteLink.Export = "csv" OrElse SiteLink.Export = "word" OrElse SiteLink.Export = "excel" OrElse SiteLink.Export = "xml" Then
			ExportData()
			Page_Terminate("") ' Clean up
			ew_End() ' Terminate response
		ElseIf SiteLink.Export = "email" Then
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
			SiteLink.RecordsPerPage = lDisplayRecs ' Save to Session

			' Reset start position
			lStartRec = 1
			SiteLink.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	' Return Advanced Search WHERE based on QueryString parameters
	'
	Function AdvancedSearchWhere() As String		
		Dim sWhere As String = ""
		BuildSearchSql(sWhere, SiteLink.ID, False) ' ID
		BuildSearchSql(sWhere, SiteLink.CompanyID, False) ' CompanyID
		BuildSearchSql(sWhere, SiteLink.LinkTypeCD, False) ' LinkTypeCD
		BuildSearchSql(sWhere, SiteLink.Title, False) ' Title
		BuildSearchSql(sWhere, SiteLink.Description, False) ' Description
		BuildSearchSql(sWhere, SiteLink.DateAdd, False) ' DateAdd
		BuildSearchSql(sWhere, SiteLink.Ranks, False) ' Ranks
		BuildSearchSql(sWhere, SiteLink.Views, False) ' Views
		BuildSearchSql(sWhere, SiteLink.UserName, False) ' UserName
		BuildSearchSql(sWhere, SiteLink.UserID, False) ' UserID
		BuildSearchSql(sWhere, SiteLink.ASIN, False) ' ASIN
		BuildSearchSql(sWhere, SiteLink.URL, False) ' URL
		BuildSearchSql(sWhere, SiteLink.CategoryID, False) ' CategoryID
		BuildSearchSql(sWhere, SiteLink.SiteCategoryID, False) ' SiteCategoryID
		BuildSearchSql(sWhere, SiteLink.SiteCategoryTypeID, False) ' SiteCategoryTypeID
		BuildSearchSql(sWhere, SiteLink.SiteCategoryGroupID, False) ' SiteCategoryGroupID

		' Set up search parm
		If sWhere <> "" Then
			SetSearchParm(SiteLink.ID) ' ID
			SetSearchParm(SiteLink.CompanyID) ' CompanyID
			SetSearchParm(SiteLink.LinkTypeCD) ' LinkTypeCD
			SetSearchParm(SiteLink.Title) ' Title
			SetSearchParm(SiteLink.Description) ' Description
			SetSearchParm(SiteLink.DateAdd) ' DateAdd
			SetSearchParm(SiteLink.Ranks) ' Ranks
			SetSearchParm(SiteLink.Views) ' Views
			SetSearchParm(SiteLink.UserName) ' UserName
			SetSearchParm(SiteLink.UserID) ' UserID
			SetSearchParm(SiteLink.ASIN) ' ASIN
			SetSearchParm(SiteLink.URL) ' URL
			SetSearchParm(SiteLink.CategoryID) ' CategoryID
			SetSearchParm(SiteLink.SiteCategoryID) ' SiteCategoryID
			SetSearchParm(SiteLink.SiteCategoryTypeID) ' SiteCategoryTypeID
			SetSearchParm(SiteLink.SiteCategoryGroupID) ' SiteCategoryGroupID
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
	' Get search parm
	'
	Sub GetSearchParm(ByRef Fld As Object)
		Dim FldParm As String = Fld.FldVar.Substring(2)
		Fld.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_" & FldParm)
		Fld.AdvancedSearch.SearchOperator = SiteLink.GetAdvancedSearch("z_" & FldParm)
		Fld.AdvancedSearch.SearchCondition = SiteLink.GetAdvancedSearch("v_" & FldParm)
		Fld.AdvancedSearch.SearchValue2 = SiteLink.GetAdvancedSearch("y_" & FldParm)
		Fld.AdvancedSearch.SearchOperator2 = SiteLink.GetAdvancedSearch("w_" & FldParm)
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
		SiteLink.SearchWhere = sSrchWhere

		' Clear advanced search parameters
		ResetAdvancedSearchParms()
	End Sub

	'
	' Clear all advanced search parameters
	'
	Sub ResetAdvancedSearchParms()
		SiteLink.SetAdvancedSearch("x_ID", "")
		SiteLink.SetAdvancedSearch("x_CompanyID", "")
		SiteLink.SetAdvancedSearch("x_LinkTypeCD", "")
		SiteLink.SetAdvancedSearch("x_Title", "")
		SiteLink.SetAdvancedSearch("x_Description", "")
		SiteLink.SetAdvancedSearch("x_DateAdd", "")
		SiteLink.SetAdvancedSearch("x_Ranks", "")
		SiteLink.SetAdvancedSearch("x_Views", "")
		SiteLink.SetAdvancedSearch("x_UserName", "")
		SiteLink.SetAdvancedSearch("x_UserID", "")
		SiteLink.SetAdvancedSearch("x_ASIN", "")
		SiteLink.SetAdvancedSearch("x_URL", "")
		SiteLink.SetAdvancedSearch("x_CategoryID", "")
		SiteLink.SetAdvancedSearch("x_SiteCategoryID", "")
		SiteLink.SetAdvancedSearch("x_SiteCategoryTypeID", "")
		SiteLink.SetAdvancedSearch("x_SiteCategoryGroupID", "")
	End Sub

	'
	' Restore all search parameters
	'
	Sub RestoreSearchParms()
		Dim bRestore As Boolean = True
		If ew_Get("x_ID") <> "" Then bRestore = False
		If ew_Get("x_CompanyID") <> "" Then bRestore = False
		If ew_Get("x_LinkTypeCD") <> "" Then bRestore = False
		If ew_Get("x_Title") <> "" Then bRestore = False
		If ew_Get("x_Description") <> "" Then bRestore = False
		If ew_Get("x_DateAdd") <> "" Then bRestore = False
		If ew_Get("x_Ranks") <> "" Then bRestore = False
		If ew_Get("x_Views") <> "" Then bRestore = False
		If ew_Get("x_UserName") <> "" Then bRestore = False
		If ew_Get("x_UserID") <> "" Then bRestore = False
		If ew_Get("x_ASIN") <> "" Then bRestore = False
		If ew_Get("x_URL") <> "" Then bRestore = False
		If ew_Get("x_CategoryID") <> "" Then bRestore = False
		If ew_Get("x_SiteCategoryID") <> "" Then bRestore = False
		If ew_Get("x_SiteCategoryTypeID") <> "" Then bRestore = False
		If ew_Get("x_SiteCategoryGroupID") <> "" Then bRestore = False
		RestoreSearch = bRestore
		If bRestore Then

			' Restore advanced search values
			Call GetSearchParm(SiteLink.ID)
			Call GetSearchParm(SiteLink.CompanyID)
			Call GetSearchParm(SiteLink.LinkTypeCD)
			Call GetSearchParm(SiteLink.Title)
			Call GetSearchParm(SiteLink.Description)
			Call GetSearchParm(SiteLink.DateAdd)
			Call GetSearchParm(SiteLink.Ranks)
			Call GetSearchParm(SiteLink.Views)
			Call GetSearchParm(SiteLink.UserName)
			Call GetSearchParm(SiteLink.UserID)
			Call GetSearchParm(SiteLink.ASIN)
			Call GetSearchParm(SiteLink.URL)
			Call GetSearchParm(SiteLink.CategoryID)
			Call GetSearchParm(SiteLink.SiteCategoryID)
			Call GetSearchParm(SiteLink.SiteCategoryTypeID)
			Call GetSearchParm(SiteLink.SiteCategoryGroupID)
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
			SiteLink.CurrentOrder = ew_Get("order")
			SiteLink.CurrentOrderType = ew_Get("ordertype")
			SiteLink.UpdateSort(SiteLink.ID) ' ID
			SiteLink.UpdateSort(SiteLink.CompanyID) ' CompanyID
			SiteLink.UpdateSort(SiteLink.LinkTypeCD) ' LinkTypeCD
			SiteLink.UpdateSort(SiteLink.Title) ' Title
			SiteLink.UpdateSort(SiteLink.DateAdd) ' DateAdd
			SiteLink.UpdateSort(SiteLink.Ranks) ' Ranks
			SiteLink.UpdateSort(SiteLink.Views) ' Views
			SiteLink.UpdateSort(SiteLink.UserName) ' UserName
			SiteLink.UpdateSort(SiteLink.UserID) ' UserID
			SiteLink.UpdateSort(SiteLink.ASIN) ' ASIN
			SiteLink.UpdateSort(SiteLink.CategoryID) ' CategoryID
			SiteLink.UpdateSort(SiteLink.SiteCategoryID) ' SiteCategoryID
			SiteLink.UpdateSort(SiteLink.SiteCategoryTypeID) ' SiteCategoryTypeID
			SiteLink.UpdateSort(SiteLink.SiteCategoryGroupID) ' SiteCategoryGroupID
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
				SiteLink.ID.Sort = ""
				SiteLink.CompanyID.Sort = ""
				SiteLink.LinkTypeCD.Sort = ""
				SiteLink.Title.Sort = ""
				SiteLink.DateAdd.Sort = ""
				SiteLink.Ranks.Sort = ""
				SiteLink.Views.Sort = ""
				SiteLink.UserName.Sort = ""
				SiteLink.UserID.Sort = ""
				SiteLink.ASIN.Sort = ""
				SiteLink.CategoryID.Sort = ""
				SiteLink.SiteCategoryID.Sort = ""
				SiteLink.SiteCategoryTypeID.Sort = ""
				SiteLink.SiteCategoryGroupID.Sort = ""
			End If

			' Reset start position
			lStartRec = 1
			SiteLink.StartRecordNumber = lStartRec
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
		If SiteLink.Export <> "" Or SiteLink.CurrentAction = "gridadd" Or SiteLink.CurrentAction = "gridedit" Then
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
		SiteLink.CompanyID.CurrentValue = 0
		SiteLink.Ranks.CurrentValue = 0
		SiteLink.Views.CurrentValue = 0
		SiteLink.UserID.CurrentValue = 0
		SiteLink.CategoryID.CurrentValue = 0
	End Sub

	'
	'  Load search values for validation
	'
	Sub LoadSearchValues()
		SiteLink.ID.AdvancedSearch.SearchValue = ew_Get("x_ID")
    	SiteLink.ID.AdvancedSearch.SearchOperator = ew_Get("z_ID")
		SiteLink.CompanyID.AdvancedSearch.SearchValue = ew_Get("x_CompanyID")
    	SiteLink.CompanyID.AdvancedSearch.SearchOperator = ew_Get("z_CompanyID")
		SiteLink.LinkTypeCD.AdvancedSearch.SearchValue = ew_Get("x_LinkTypeCD")
    	SiteLink.LinkTypeCD.AdvancedSearch.SearchOperator = ew_Get("z_LinkTypeCD")
		SiteLink.Title.AdvancedSearch.SearchValue = ew_Get("x_Title")
    	SiteLink.Title.AdvancedSearch.SearchOperator = ew_Get("z_Title")
		SiteLink.Description.AdvancedSearch.SearchValue = ew_Get("x_Description")
    	SiteLink.Description.AdvancedSearch.SearchOperator = ew_Get("z_Description")
		SiteLink.DateAdd.AdvancedSearch.SearchValue = ew_Get("x_DateAdd")
    	SiteLink.DateAdd.AdvancedSearch.SearchOperator = ew_Get("z_DateAdd")
		SiteLink.Ranks.AdvancedSearch.SearchValue = ew_Get("x_Ranks")
    	SiteLink.Ranks.AdvancedSearch.SearchOperator = ew_Get("z_Ranks")
		SiteLink.Views.AdvancedSearch.SearchValue = ew_Get("x_Views")
    	SiteLink.Views.AdvancedSearch.SearchOperator = ew_Get("z_Views")
		SiteLink.UserName.AdvancedSearch.SearchValue = ew_Get("x_UserName")
    	SiteLink.UserName.AdvancedSearch.SearchOperator = ew_Get("z_UserName")
		SiteLink.UserID.AdvancedSearch.SearchValue = ew_Get("x_UserID")
    	SiteLink.UserID.AdvancedSearch.SearchOperator = ew_Get("z_UserID")
		SiteLink.ASIN.AdvancedSearch.SearchValue = ew_Get("x_ASIN")
    	SiteLink.ASIN.AdvancedSearch.SearchOperator = ew_Get("z_ASIN")
		SiteLink.URL.AdvancedSearch.SearchValue = ew_Get("x_URL")
    	SiteLink.URL.AdvancedSearch.SearchOperator = ew_Get("z_URL")
		SiteLink.CategoryID.AdvancedSearch.SearchValue = ew_Get("x_CategoryID")
    	SiteLink.CategoryID.AdvancedSearch.SearchOperator = ew_Get("z_CategoryID")
		SiteLink.SiteCategoryID.AdvancedSearch.SearchValue = ew_Get("x_SiteCategoryID")
    	SiteLink.SiteCategoryID.AdvancedSearch.SearchOperator = ew_Get("z_SiteCategoryID")
		SiteLink.SiteCategoryTypeID.AdvancedSearch.SearchValue = ew_Get("x_SiteCategoryTypeID")
    	SiteLink.SiteCategoryTypeID.AdvancedSearch.SearchOperator = ew_Get("z_SiteCategoryTypeID")
		SiteLink.SiteCategoryGroupID.AdvancedSearch.SearchValue = ew_Get("x_SiteCategoryGroupID")
    	SiteLink.SiteCategoryGroupID.AdvancedSearch.SearchOperator = ew_Get("z_SiteCategoryGroupID")
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
		If EW_DEBUG_ENABLED Then DebugMsg = sSql

		' Count
		lTotalRecs = -1		
		Try
			Dim sCntSql As String = SiteLink.SelectCountSQL

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
		If EW_DEBUG_ENABLED Then DebugMsg = sSql
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
		Dim sDetailFilter As String
		SiteLink.ID.DbValue = RsRow("ID")
		SiteLink.CompanyID.DbValue = RsRow("CompanyID")
		SiteLink.LinkTypeCD.DbValue = RsRow("LinkTypeCD")
		SiteLink.Title.DbValue = RsRow("Title")
		SiteLink.Description.DbValue = RsRow("Description")
		SiteLink.DateAdd.DbValue = RsRow("DateAdd")
		SiteLink.Ranks.DbValue = RsRow("Ranks")
		SiteLink.Views.DbValue = IIf(ew_ConvertToBool(RsRow("Views")), "1", "0")
		SiteLink.UserName.DbValue = RsRow("UserName")
		SiteLink.UserID.DbValue = RsRow("UserID")
		SiteLink.ASIN.DbValue = RsRow("ASIN")
		SiteLink.URL.DbValue = RsRow("URL")
		SiteLink.CategoryID.DbValue = RsRow("CategoryID")
		SiteLink.SiteCategoryID.DbValue = RsRow("SiteCategoryID")
		SiteLink.SiteCategoryTypeID.DbValue = RsRow("SiteCategoryTypeID")
		SiteLink.SiteCategoryGroupID.DbValue = RsRow("SiteCategoryGroupID")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Initialize urls
		ViewUrl = SiteLink.ViewUrl
		EditUrl = SiteLink.EditUrl
		InlineEditUrl = SiteLink.InlineEditUrl
		CopyUrl = SiteLink.CopyUrl
		InlineCopyUrl = SiteLink.InlineCopyUrl
		DeleteUrl = SiteLink.DeleteUrl

		' Row Rendering event
		SiteLink.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' ID

		SiteLink.ID.CellCssStyle = ""
		SiteLink.ID.CellCssClass = ""
		SiteLink.ID.CellAttrs.Clear(): SiteLink.ID.ViewAttrs.Clear(): SiteLink.ID.EditAttrs.Clear()

		' CompanyID
		SiteLink.CompanyID.CellCssStyle = ""
		SiteLink.CompanyID.CellCssClass = ""
		SiteLink.CompanyID.CellAttrs.Clear(): SiteLink.CompanyID.ViewAttrs.Clear(): SiteLink.CompanyID.EditAttrs.Clear()

		' LinkTypeCD
		SiteLink.LinkTypeCD.CellCssStyle = ""
		SiteLink.LinkTypeCD.CellCssClass = ""
		SiteLink.LinkTypeCD.CellAttrs.Clear(): SiteLink.LinkTypeCD.ViewAttrs.Clear(): SiteLink.LinkTypeCD.EditAttrs.Clear()

		' Title
		SiteLink.Title.CellCssStyle = ""
		SiteLink.Title.CellCssClass = ""
		SiteLink.Title.CellAttrs.Clear(): SiteLink.Title.ViewAttrs.Clear(): SiteLink.Title.EditAttrs.Clear()

		' DateAdd
		SiteLink.DateAdd.CellCssStyle = ""
		SiteLink.DateAdd.CellCssClass = ""
		SiteLink.DateAdd.CellAttrs.Clear(): SiteLink.DateAdd.ViewAttrs.Clear(): SiteLink.DateAdd.EditAttrs.Clear()

		' Ranks
		SiteLink.Ranks.CellCssStyle = ""
		SiteLink.Ranks.CellCssClass = ""
		SiteLink.Ranks.CellAttrs.Clear(): SiteLink.Ranks.ViewAttrs.Clear(): SiteLink.Ranks.EditAttrs.Clear()

		' Views
		SiteLink.Views.CellCssStyle = ""
		SiteLink.Views.CellCssClass = ""
		SiteLink.Views.CellAttrs.Clear(): SiteLink.Views.ViewAttrs.Clear(): SiteLink.Views.EditAttrs.Clear()

		' UserName
		SiteLink.UserName.CellCssStyle = ""
		SiteLink.UserName.CellCssClass = ""
		SiteLink.UserName.CellAttrs.Clear(): SiteLink.UserName.ViewAttrs.Clear(): SiteLink.UserName.EditAttrs.Clear()

		' UserID
		SiteLink.UserID.CellCssStyle = ""
		SiteLink.UserID.CellCssClass = ""
		SiteLink.UserID.CellAttrs.Clear(): SiteLink.UserID.ViewAttrs.Clear(): SiteLink.UserID.EditAttrs.Clear()

		' ASIN
		SiteLink.ASIN.CellCssStyle = ""
		SiteLink.ASIN.CellCssClass = ""
		SiteLink.ASIN.CellAttrs.Clear(): SiteLink.ASIN.ViewAttrs.Clear(): SiteLink.ASIN.EditAttrs.Clear()

		' CategoryID
		SiteLink.CategoryID.CellCssStyle = ""
		SiteLink.CategoryID.CellCssClass = ""
		SiteLink.CategoryID.CellAttrs.Clear(): SiteLink.CategoryID.ViewAttrs.Clear(): SiteLink.CategoryID.EditAttrs.Clear()

		' SiteCategoryID
		SiteLink.SiteCategoryID.CellCssStyle = ""
		SiteLink.SiteCategoryID.CellCssClass = ""
		SiteLink.SiteCategoryID.CellAttrs.Clear(): SiteLink.SiteCategoryID.ViewAttrs.Clear(): SiteLink.SiteCategoryID.EditAttrs.Clear()

		' SiteCategoryTypeID
		SiteLink.SiteCategoryTypeID.CellCssStyle = ""
		SiteLink.SiteCategoryTypeID.CellCssClass = ""
		SiteLink.SiteCategoryTypeID.CellAttrs.Clear(): SiteLink.SiteCategoryTypeID.ViewAttrs.Clear(): SiteLink.SiteCategoryTypeID.EditAttrs.Clear()

		' SiteCategoryGroupID
		SiteLink.SiteCategoryGroupID.CellCssStyle = ""
		SiteLink.SiteCategoryGroupID.CellCssClass = ""
		SiteLink.SiteCategoryGroupID.CellAttrs.Clear(): SiteLink.SiteCategoryGroupID.ViewAttrs.Clear(): SiteLink.SiteCategoryGroupID.EditAttrs.Clear()

		'
		'  View  Row
		'

		If SiteLink.RowType = EW_ROWTYPE_VIEW Then ' View row

			' ID
			SiteLink.ID.ViewValue = SiteLink.ID.CurrentValue
			SiteLink.ID.CssStyle = ""
			SiteLink.ID.CssClass = ""
			SiteLink.ID.ViewCustomAttributes = ""

			' CompanyID
			If ew_NotEmpty(SiteLink.CompanyID.CurrentValue) Then
				sFilterWrk = "[CompanyID] = " & ew_AdjustSql(SiteLink.CompanyID.CurrentValue) & ""
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

			' LinkTypeCD
			If ew_NotEmpty(SiteLink.LinkTypeCD.CurrentValue) Then
				sFilterWrk = "[LinkTypeCD] = '" & ew_AdjustSql(SiteLink.LinkTypeCD.CurrentValue) & "'"
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

			' Title
			SiteLink.Title.ViewValue = SiteLink.Title.CurrentValue
			SiteLink.Title.CssStyle = ""
			SiteLink.Title.CssClass = ""
			SiteLink.Title.ViewCustomAttributes = ""

			' DateAdd
			SiteLink.DateAdd.ViewValue = SiteLink.DateAdd.CurrentValue
			SiteLink.DateAdd.CssStyle = ""
			SiteLink.DateAdd.CssClass = ""
			SiteLink.DateAdd.ViewCustomAttributes = ""

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

			' UserName
			SiteLink.UserName.ViewValue = SiteLink.UserName.CurrentValue
			SiteLink.UserName.CssStyle = ""
			SiteLink.UserName.CssClass = ""
			SiteLink.UserName.ViewCustomAttributes = ""

			' UserID
			SiteLink.UserID.ViewValue = SiteLink.UserID.CurrentValue
			SiteLink.UserID.CssStyle = ""
			SiteLink.UserID.CssClass = ""
			SiteLink.UserID.ViewCustomAttributes = ""

			' ASIN
			SiteLink.ASIN.ViewValue = SiteLink.ASIN.CurrentValue
			SiteLink.ASIN.CssStyle = ""
			SiteLink.ASIN.CssClass = ""
			SiteLink.ASIN.ViewCustomAttributes = ""

			' CategoryID
			If ew_NotEmpty(SiteLink.CategoryID.CurrentValue) Then
				sFilterWrk = "[ID] = " & ew_AdjustSql(SiteLink.CategoryID.CurrentValue) & ""
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

			' SiteCategoryID
			SiteLink.SiteCategoryID.ViewValue = SiteLink.SiteCategoryID.CurrentValue
			SiteLink.SiteCategoryID.CssStyle = ""
			SiteLink.SiteCategoryID.CssClass = ""
			SiteLink.SiteCategoryID.ViewCustomAttributes = ""

			' SiteCategoryTypeID
			SiteLink.SiteCategoryTypeID.ViewValue = SiteLink.SiteCategoryTypeID.CurrentValue
			SiteLink.SiteCategoryTypeID.CssStyle = ""
			SiteLink.SiteCategoryTypeID.CssClass = ""
			SiteLink.SiteCategoryTypeID.ViewCustomAttributes = ""

			' SiteCategoryGroupID
			SiteLink.SiteCategoryGroupID.ViewValue = SiteLink.SiteCategoryGroupID.CurrentValue
			SiteLink.SiteCategoryGroupID.CssStyle = ""
			SiteLink.SiteCategoryGroupID.CssClass = ""
			SiteLink.SiteCategoryGroupID.ViewCustomAttributes = ""

			' View refer script
			' ID

			SiteLink.ID.HrefValue = ""
			SiteLink.ID.TooltipValue = ""

			' CompanyID
			SiteLink.CompanyID.HrefValue = ""
			SiteLink.CompanyID.TooltipValue = ""

			' LinkTypeCD
			SiteLink.LinkTypeCD.HrefValue = ""
			SiteLink.LinkTypeCD.TooltipValue = ""

			' Title
			SiteLink.Title.HrefValue = ""
			SiteLink.Title.TooltipValue = ""

			' DateAdd
			SiteLink.DateAdd.HrefValue = ""
			SiteLink.DateAdd.TooltipValue = ""

			' Ranks
			SiteLink.Ranks.HrefValue = ""
			SiteLink.Ranks.TooltipValue = ""

			' Views
			SiteLink.Views.HrefValue = ""
			SiteLink.Views.TooltipValue = ""

			' UserName
			SiteLink.UserName.HrefValue = ""
			SiteLink.UserName.TooltipValue = ""

			' UserID
			SiteLink.UserID.HrefValue = ""
			SiteLink.UserID.TooltipValue = ""

			' ASIN
			SiteLink.ASIN.HrefValue = ""
			SiteLink.ASIN.TooltipValue = ""

			' CategoryID
			SiteLink.CategoryID.HrefValue = ""
			SiteLink.CategoryID.TooltipValue = ""

			' SiteCategoryID
			SiteLink.SiteCategoryID.HrefValue = ""
			SiteLink.SiteCategoryID.TooltipValue = ""

			' SiteCategoryTypeID
			SiteLink.SiteCategoryTypeID.HrefValue = ""
			SiteLink.SiteCategoryTypeID.TooltipValue = ""

			' SiteCategoryGroupID
			SiteLink.SiteCategoryGroupID.HrefValue = ""
			SiteLink.SiteCategoryGroupID.TooltipValue = ""
		End If

		' Row Rendered event
		If SiteLink.RowType <> EW_ROWTYPE_AGGREGATEINIT Then
			SiteLink.Row_Rendered()
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
		SiteLink.ID.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_ID")
		SiteLink.CompanyID.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_CompanyID")
		SiteLink.LinkTypeCD.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_LinkTypeCD")
		SiteLink.Title.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_Title")
		SiteLink.Description.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_Description")
		SiteLink.DateAdd.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_DateAdd")
		SiteLink.Ranks.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_Ranks")
		SiteLink.Views.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_Views")
		SiteLink.UserName.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_UserName")
		SiteLink.UserID.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_UserID")
		SiteLink.ASIN.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_ASIN")
		SiteLink.URL.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_URL")
		SiteLink.CategoryID.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_CategoryID")
		SiteLink.SiteCategoryID.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_SiteCategoryID")
		SiteLink.SiteCategoryTypeID.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_SiteCategoryTypeID")
		SiteLink.SiteCategoryGroupID.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_SiteCategoryGroupID")
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
			oXmlDoc = New cXMLDocument()
		Else
			sOutputStr = ew_ExportHeader(SiteLink.Export)

			' Horizontal format, write header
			If sExportStyle <> "v" OrElse SiteLink.Export = "csv" Then
				sExportStr = ""
				ew_ExportAddValue(sExportStr, SiteLink.ID.ExportCaption, SiteLink.Export, IIf(EW_EXPORT_CSS_STYLES, SiteLink.ID.CellStyles, ""))
				ew_ExportAddValue(sExportStr, SiteLink.CompanyID.ExportCaption, SiteLink.Export, IIf(EW_EXPORT_CSS_STYLES, SiteLink.CompanyID.CellStyles, ""))
				ew_ExportAddValue(sExportStr, SiteLink.LinkTypeCD.ExportCaption, SiteLink.Export, IIf(EW_EXPORT_CSS_STYLES, SiteLink.LinkTypeCD.CellStyles, ""))
				ew_ExportAddValue(sExportStr, SiteLink.Title.ExportCaption, SiteLink.Export, IIf(EW_EXPORT_CSS_STYLES, SiteLink.Title.CellStyles, ""))
				ew_ExportAddValue(sExportStr, SiteLink.DateAdd.ExportCaption, SiteLink.Export, IIf(EW_EXPORT_CSS_STYLES, SiteLink.DateAdd.CellStyles, ""))
				ew_ExportAddValue(sExportStr, SiteLink.Ranks.ExportCaption, SiteLink.Export, IIf(EW_EXPORT_CSS_STYLES, SiteLink.Ranks.CellStyles, ""))
				ew_ExportAddValue(sExportStr, SiteLink.Views.ExportCaption, SiteLink.Export, IIf(EW_EXPORT_CSS_STYLES, SiteLink.Views.CellStyles, ""))
				ew_ExportAddValue(sExportStr, SiteLink.UserName.ExportCaption, SiteLink.Export, IIf(EW_EXPORT_CSS_STYLES, SiteLink.UserName.CellStyles, ""))
				ew_ExportAddValue(sExportStr, SiteLink.UserID.ExportCaption, SiteLink.Export, IIf(EW_EXPORT_CSS_STYLES, SiteLink.UserID.CellStyles, ""))
				ew_ExportAddValue(sExportStr, SiteLink.ASIN.ExportCaption, SiteLink.Export, IIf(EW_EXPORT_CSS_STYLES, SiteLink.ASIN.CellStyles, ""))
				ew_ExportAddValue(sExportStr, SiteLink.CategoryID.ExportCaption, SiteLink.Export, IIf(EW_EXPORT_CSS_STYLES, SiteLink.CategoryID.CellStyles, ""))
				ew_ExportAddValue(sExportStr, SiteLink.SiteCategoryID.ExportCaption, SiteLink.Export, IIf(EW_EXPORT_CSS_STYLES, SiteLink.SiteCategoryID.CellStyles, ""))
				ew_ExportAddValue(sExportStr, SiteLink.SiteCategoryTypeID.ExportCaption, SiteLink.Export, IIf(EW_EXPORT_CSS_STYLES, SiteLink.SiteCategoryTypeID.CellStyles, ""))
				ew_ExportAddValue(sExportStr, SiteLink.SiteCategoryGroupID.ExportCaption, SiteLink.Export, IIf(EW_EXPORT_CSS_STYLES, SiteLink.SiteCategoryGroupID.CellStyles, ""))
				sOutputStr &= ew_ExportLine(sExportStr, SiteLink.Export, IIf(EW_EXPORT_CSS_STYLES, SiteLink.RowStyles, ""))
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
				SiteLink.CssClass = ""
				SiteLink.CssStyle = ""
				SiteLink.RowType = EW_ROWTYPE_VIEW ' Render view
				RenderRow()
				If SiteLink.Export = "xml" Then
					oXmlDoc.AddRow()
					oXmlDoc.AddField("ID", SiteLink.ID.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue)) ' ID
					oXmlDoc.AddField("CompanyID", SiteLink.CompanyID.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue)) ' CompanyID
					oXmlDoc.AddField("LinkTypeCD", SiteLink.LinkTypeCD.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue)) ' LinkTypeCD
					oXmlDoc.AddField("Title", SiteLink.Title.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue)) ' Title
					oXmlDoc.AddField("DateAdd", SiteLink.DateAdd.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue)) ' DateAdd
					oXmlDoc.AddField("Ranks", SiteLink.Ranks.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue)) ' Ranks
					oXmlDoc.AddField("Views", SiteLink.Views.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue)) ' Views
					oXmlDoc.AddField("UserName", SiteLink.UserName.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue)) ' UserName
					oXmlDoc.AddField("UserID", SiteLink.UserID.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue)) ' UserID
					oXmlDoc.AddField("ASIN", SiteLink.ASIN.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue)) ' ASIN
					oXmlDoc.AddField("CategoryID", SiteLink.CategoryID.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue)) ' CategoryID
					oXmlDoc.AddField("SiteCategoryID", SiteLink.SiteCategoryID.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue)) ' SiteCategoryID
					oXmlDoc.AddField("SiteCategoryTypeID", SiteLink.SiteCategoryTypeID.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue)) ' SiteCategoryTypeID
					oXmlDoc.AddField("SiteCategoryGroupID", SiteLink.SiteCategoryGroupID.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue)) ' SiteCategoryGroupID
				Else

					' Vertical format
					If sExportStyle = "v" AndAlso SiteLink.Export <> "csv" Then
						sOutputStr &= ew_ExportField(SiteLink.ID.ExportCaption, SiteLink.ID.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue), SiteLink.Export, IIf(EW_EXPORT_CSS_STYLES, SiteLink.ID.CellStyles, "")) ' ID
						sOutputStr &= ew_ExportField(SiteLink.CompanyID.ExportCaption, SiteLink.CompanyID.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue), SiteLink.Export, IIf(EW_EXPORT_CSS_STYLES, SiteLink.CompanyID.CellStyles, "")) ' CompanyID
						sOutputStr &= ew_ExportField(SiteLink.LinkTypeCD.ExportCaption, SiteLink.LinkTypeCD.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue), SiteLink.Export, IIf(EW_EXPORT_CSS_STYLES, SiteLink.LinkTypeCD.CellStyles, "")) ' LinkTypeCD
						sOutputStr &= ew_ExportField(SiteLink.Title.ExportCaption, SiteLink.Title.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue), SiteLink.Export, IIf(EW_EXPORT_CSS_STYLES, SiteLink.Title.CellStyles, "")) ' Title
						sOutputStr &= ew_ExportField(SiteLink.DateAdd.ExportCaption, SiteLink.DateAdd.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue), SiteLink.Export, IIf(EW_EXPORT_CSS_STYLES, SiteLink.DateAdd.CellStyles, "")) ' DateAdd
						sOutputStr &= ew_ExportField(SiteLink.Ranks.ExportCaption, SiteLink.Ranks.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue), SiteLink.Export, IIf(EW_EXPORT_CSS_STYLES, SiteLink.Ranks.CellStyles, "")) ' Ranks
						sOutputStr &= ew_ExportField(SiteLink.Views.ExportCaption, SiteLink.Views.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue), SiteLink.Export, IIf(EW_EXPORT_CSS_STYLES, SiteLink.Views.CellStyles, "")) ' Views
						sOutputStr &= ew_ExportField(SiteLink.UserName.ExportCaption, SiteLink.UserName.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue), SiteLink.Export, IIf(EW_EXPORT_CSS_STYLES, SiteLink.UserName.CellStyles, "")) ' UserName
						sOutputStr &= ew_ExportField(SiteLink.UserID.ExportCaption, SiteLink.UserID.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue), SiteLink.Export, IIf(EW_EXPORT_CSS_STYLES, SiteLink.UserID.CellStyles, "")) ' UserID
						sOutputStr &= ew_ExportField(SiteLink.ASIN.ExportCaption, SiteLink.ASIN.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue), SiteLink.Export, IIf(EW_EXPORT_CSS_STYLES, SiteLink.ASIN.CellStyles, "")) ' ASIN
						sOutputStr &= ew_ExportField(SiteLink.CategoryID.ExportCaption, SiteLink.CategoryID.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue), SiteLink.Export, IIf(EW_EXPORT_CSS_STYLES, SiteLink.CategoryID.CellStyles, "")) ' CategoryID
						sOutputStr &= ew_ExportField(SiteLink.SiteCategoryID.ExportCaption, SiteLink.SiteCategoryID.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue), SiteLink.Export, IIf(EW_EXPORT_CSS_STYLES, SiteLink.SiteCategoryID.CellStyles, "")) ' SiteCategoryID
						sOutputStr &= ew_ExportField(SiteLink.SiteCategoryTypeID.ExportCaption, SiteLink.SiteCategoryTypeID.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue), SiteLink.Export, IIf(EW_EXPORT_CSS_STYLES, SiteLink.SiteCategoryTypeID.CellStyles, "")) ' SiteCategoryTypeID
						sOutputStr &= ew_ExportField(SiteLink.SiteCategoryGroupID.ExportCaption, SiteLink.SiteCategoryGroupID.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue), SiteLink.Export, IIf(EW_EXPORT_CSS_STYLES, SiteLink.SiteCategoryGroupID.CellStyles, "")) ' SiteCategoryGroupID

					' Horizontal format
					Else
						sExportStr = ""
						ew_ExportAddValue(sExportStr, SiteLink.ID.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue), SiteLink.Export, IIf(EW_EXPORT_CSS_STYLES, SiteLink.ID.CellStyles, ""))
						ew_ExportAddValue(sExportStr, SiteLink.CompanyID.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue), SiteLink.Export, IIf(EW_EXPORT_CSS_STYLES, SiteLink.CompanyID.CellStyles, ""))
						ew_ExportAddValue(sExportStr, SiteLink.LinkTypeCD.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue), SiteLink.Export, IIf(EW_EXPORT_CSS_STYLES, SiteLink.LinkTypeCD.CellStyles, ""))
						ew_ExportAddValue(sExportStr, SiteLink.Title.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue), SiteLink.Export, IIf(EW_EXPORT_CSS_STYLES, SiteLink.Title.CellStyles, ""))
						ew_ExportAddValue(sExportStr, SiteLink.DateAdd.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue), SiteLink.Export, IIf(EW_EXPORT_CSS_STYLES, SiteLink.DateAdd.CellStyles, ""))
						ew_ExportAddValue(sExportStr, SiteLink.Ranks.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue), SiteLink.Export, IIf(EW_EXPORT_CSS_STYLES, SiteLink.Ranks.CellStyles, ""))
						ew_ExportAddValue(sExportStr, SiteLink.Views.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue), SiteLink.Export, IIf(EW_EXPORT_CSS_STYLES, SiteLink.Views.CellStyles, ""))
						ew_ExportAddValue(sExportStr, SiteLink.UserName.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue), SiteLink.Export, IIf(EW_EXPORT_CSS_STYLES, SiteLink.UserName.CellStyles, ""))
						ew_ExportAddValue(sExportStr, SiteLink.UserID.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue), SiteLink.Export, IIf(EW_EXPORT_CSS_STYLES, SiteLink.UserID.CellStyles, ""))
						ew_ExportAddValue(sExportStr, SiteLink.ASIN.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue), SiteLink.Export, IIf(EW_EXPORT_CSS_STYLES, SiteLink.ASIN.CellStyles, ""))
						ew_ExportAddValue(sExportStr, SiteLink.CategoryID.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue), SiteLink.Export, IIf(EW_EXPORT_CSS_STYLES, SiteLink.CategoryID.CellStyles, ""))
						ew_ExportAddValue(sExportStr, SiteLink.SiteCategoryID.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue), SiteLink.Export, IIf(EW_EXPORT_CSS_STYLES, SiteLink.SiteCategoryID.CellStyles, ""))
						ew_ExportAddValue(sExportStr, SiteLink.SiteCategoryTypeID.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue), SiteLink.Export, IIf(EW_EXPORT_CSS_STYLES, SiteLink.SiteCategoryTypeID.CellStyles, ""))
						ew_ExportAddValue(sExportStr, SiteLink.SiteCategoryGroupID.ExportValue(SiteLink.Export, SiteLink.ExportOriginalValue), SiteLink.Export, IIf(EW_EXPORT_CSS_STYLES, SiteLink.SiteCategoryGroupID.CellStyles, ""))
						sOutputStr &= ew_ExportLine(sExportStr, SiteLink.Export, IIf(EW_EXPORT_CSS_STYLES, SiteLink.RowStyles, ""))
					End If
				End If
			End If
		Loop

		' Close recordset
		Rs.Close()
		Rs.Dispose()
		If SiteLink.Export = "xml" Then
			oXmlDoc.Output
		Else
			sOutputStr &= ew_ExportFooter(SiteLink.Export)
			ew_Write(sOutputStr)
		End If
	End Sub

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "SiteLink"
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
		SiteLink_list = New cSiteLink_list(Me)		
		SiteLink_list.Page_Init()		

		' Set buffer/cache option
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

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
