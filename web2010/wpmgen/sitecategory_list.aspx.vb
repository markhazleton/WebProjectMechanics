Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class sitecategory_list
	Inherits AspNetMaker8_wpmWebsite

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

		Private sFilterWrk As String

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

		' ASP.NET page object
		Public ReadOnly Property AspNetPage() As sitecategory_list
			Get
				Return CType(m_ParentPage, sitecategory_list)
			End Get
		End Property

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
		Public Sub New(ByRef APage As AspNetMaker8_wpmWebsite)				
			m_ParentPage = APage
			m_Page = Me
			m_PageID = "list"
			m_PageObjName = "SiteCategory_list"
			m_PageObjTypeName = "cSiteCategory_list"			

			' Initialize language object
			Language = New cLanguage(Me)

			' Table Name
			m_TableName = "SiteCategory"

			' Initialize table object
			SiteCategory = New cSiteCategory(Me)

			' Initialize URLs
			ExportPrintUrl = PageUrl & "export=print"
			ExportExcelUrl = PageUrl & "export=excel"
			ExportWordUrl = PageUrl & "export=word"
			ExportHtmlUrl = PageUrl & "export=html"
			ExportXmlUrl = PageUrl & "export=xml"
			ExportCsvUrl = PageUrl & "export=csv"
			AddUrl = SiteCategory.AddUrl
			InlineAddUrl = PageUrl & "a=add"
			GridAddUrl = PageUrl & "a=gridadd"
			GridEditUrl = PageUrl & "a=gridedit"
			MultiDeleteUrl = "sitecategory_delete.aspx"
			MultiUpdateUrl = "sitecategory_update.aspx"

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
				SiteCategory.Export = ew_Get("export")
			ElseIf ew_Post("exporttype") <> "" Then
				SiteCategory.Export = ew_Post("exporttype")
			Else
				SiteCategory.ExportReturnUrl = ew_CurrentUrl()
			End If
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
			Call SiteCategory.Recordset_SearchValidated()

			' Set Up Sorting Order
			SetUpSortOrder()

			' Get search criteria for advanced search
			If ParentPage.gsSearchError = "" Then
				sSrchAdvanced = AdvancedSearchWhere()
			End If
		End If

		' Restore display records
		If (SiteCategory.RecordsPerPage = -1 OrElse SiteCategory.RecordsPerPage > 0) Then
			lDisplayRecs = SiteCategory.RecordsPerPage ' Restore from Session
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
		SiteCategory.Recordset_Searching(sSrchWhere)

		' Save search criteria
		If sSrchWhere <> "" Then
			If sSrchAdvanced = "" Then ResetAdvancedSearchParms()
			SiteCategory.SearchWhere = sSrchWhere ' Save to Session
			If RestoreSearch Then
				lStartRec = 1 ' Reset start record counter
				SiteCategory.StartRecordNumber = lStartRec
			End If
		Else
			sSrchWhere = SiteCategory.SearchWhere
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
		ElseIf SiteCategory.Export = "email" Then
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
			SiteCategory.RecordsPerPage = lDisplayRecs ' Save to Session

			' Reset start position
			lStartRec = 1
			SiteCategory.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	' Return Advanced Search WHERE based on QueryString parameters
	'
	Function AdvancedSearchWhere() As String		
		Dim sWhere As String = ""
		BuildSearchSql(sWhere, SiteCategory.SiteCategoryID, False) ' SiteCategoryID
		BuildSearchSql(sWhere, SiteCategory.CategoryKeywords, False) ' CategoryKeywords
		BuildSearchSql(sWhere, SiteCategory.CategoryName, False) ' CategoryName
		BuildSearchSql(sWhere, SiteCategory.CategoryTitle, False) ' CategoryTitle
		BuildSearchSql(sWhere, SiteCategory.CategoryDescription, False) ' CategoryDescription
		BuildSearchSql(sWhere, SiteCategory.GroupOrder, False) ' GroupOrder
		BuildSearchSql(sWhere, SiteCategory.ParentCategoryID, False) ' ParentCategoryID
		BuildSearchSql(sWhere, SiteCategory.CategoryFileName, False) ' CategoryFileName
		BuildSearchSql(sWhere, SiteCategory.SiteCategoryTypeID, False) ' SiteCategoryTypeID
		BuildSearchSql(sWhere, SiteCategory.SiteCategoryGroupID, False) ' SiteCategoryGroupID

		' Set up search parm
		If sWhere <> "" Then
			SetSearchParm(SiteCategory.SiteCategoryID) ' SiteCategoryID
			SetSearchParm(SiteCategory.CategoryKeywords) ' CategoryKeywords
			SetSearchParm(SiteCategory.CategoryName) ' CategoryName
			SetSearchParm(SiteCategory.CategoryTitle) ' CategoryTitle
			SetSearchParm(SiteCategory.CategoryDescription) ' CategoryDescription
			SetSearchParm(SiteCategory.GroupOrder) ' GroupOrder
			SetSearchParm(SiteCategory.ParentCategoryID) ' ParentCategoryID
			SetSearchParm(SiteCategory.CategoryFileName) ' CategoryFileName
			SetSearchParm(SiteCategory.SiteCategoryTypeID) ' SiteCategoryTypeID
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
	' Get search parm
	'
	Sub GetSearchParm(ByRef Fld As Object)
		Dim FldParm As String = Fld.FldVar.Substring(2)
		Fld.AdvancedSearch.SearchValue = SiteCategory.GetAdvancedSearch("x_" & FldParm)
		Fld.AdvancedSearch.SearchOperator = SiteCategory.GetAdvancedSearch("z_" & FldParm)
		Fld.AdvancedSearch.SearchCondition = SiteCategory.GetAdvancedSearch("v_" & FldParm)
		Fld.AdvancedSearch.SearchValue2 = SiteCategory.GetAdvancedSearch("y_" & FldParm)
		Fld.AdvancedSearch.SearchOperator2 = SiteCategory.GetAdvancedSearch("w_" & FldParm)
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
		SiteCategory.SearchWhere = sSrchWhere

		' Clear advanced search parameters
		ResetAdvancedSearchParms()
	End Sub

	'
	' Clear all advanced search parameters
	'
	Sub ResetAdvancedSearchParms()
		SiteCategory.SetAdvancedSearch("x_SiteCategoryID", "")
		SiteCategory.SetAdvancedSearch("x_CategoryKeywords", "")
		SiteCategory.SetAdvancedSearch("x_CategoryName", "")
		SiteCategory.SetAdvancedSearch("x_CategoryTitle", "")
		SiteCategory.SetAdvancedSearch("x_CategoryDescription", "")
		SiteCategory.SetAdvancedSearch("x_GroupOrder", "")
		SiteCategory.SetAdvancedSearch("x_ParentCategoryID", "")
		SiteCategory.SetAdvancedSearch("x_CategoryFileName", "")
		SiteCategory.SetAdvancedSearch("x_SiteCategoryTypeID", "")
		SiteCategory.SetAdvancedSearch("x_SiteCategoryGroupID", "")
	End Sub

	'
	' Restore all search parameters
	'
	Sub RestoreSearchParms()
		Dim bRestore As Boolean = True
		If ew_Get("x_SiteCategoryID") <> "" Then bRestore = False
		If ew_Get("x_CategoryKeywords") <> "" Then bRestore = False
		If ew_Get("x_CategoryName") <> "" Then bRestore = False
		If ew_Get("x_CategoryTitle") <> "" Then bRestore = False
		If ew_Get("x_CategoryDescription") <> "" Then bRestore = False
		If ew_Get("x_GroupOrder") <> "" Then bRestore = False
		If ew_Get("x_ParentCategoryID") <> "" Then bRestore = False
		If ew_Get("x_CategoryFileName") <> "" Then bRestore = False
		If ew_Get("x_SiteCategoryTypeID") <> "" Then bRestore = False
		If ew_Get("x_SiteCategoryGroupID") <> "" Then bRestore = False
		RestoreSearch = bRestore
		If bRestore Then

			' Restore advanced search values
			Call GetSearchParm(SiteCategory.SiteCategoryID)
			Call GetSearchParm(SiteCategory.CategoryKeywords)
			Call GetSearchParm(SiteCategory.CategoryName)
			Call GetSearchParm(SiteCategory.CategoryTitle)
			Call GetSearchParm(SiteCategory.CategoryDescription)
			Call GetSearchParm(SiteCategory.GroupOrder)
			Call GetSearchParm(SiteCategory.ParentCategoryID)
			Call GetSearchParm(SiteCategory.CategoryFileName)
			Call GetSearchParm(SiteCategory.SiteCategoryTypeID)
			Call GetSearchParm(SiteCategory.SiteCategoryGroupID)
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
			SiteCategory.CurrentOrder = ew_Get("order")
			SiteCategory.CurrentOrderType = ew_Get("ordertype")
			SiteCategory.UpdateSort(SiteCategory.SiteCategoryID) ' SiteCategoryID
			SiteCategory.UpdateSort(SiteCategory.CategoryKeywords) ' CategoryKeywords
			SiteCategory.UpdateSort(SiteCategory.CategoryName) ' CategoryName
			SiteCategory.UpdateSort(SiteCategory.CategoryTitle) ' CategoryTitle
			SiteCategory.UpdateSort(SiteCategory.CategoryDescription) ' CategoryDescription
			SiteCategory.UpdateSort(SiteCategory.GroupOrder) ' GroupOrder
			SiteCategory.UpdateSort(SiteCategory.ParentCategoryID) ' ParentCategoryID
			SiteCategory.UpdateSort(SiteCategory.CategoryFileName) ' CategoryFileName
			SiteCategory.UpdateSort(SiteCategory.SiteCategoryTypeID) ' SiteCategoryTypeID
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
				SiteCategory.SiteCategoryID.Sort = ""
				SiteCategory.CategoryKeywords.Sort = ""
				SiteCategory.CategoryName.Sort = ""
				SiteCategory.CategoryTitle.Sort = ""
				SiteCategory.CategoryDescription.Sort = ""
				SiteCategory.GroupOrder.Sort = ""
				SiteCategory.ParentCategoryID.Sort = ""
				SiteCategory.CategoryFileName.Sort = ""
				SiteCategory.SiteCategoryTypeID.Sort = ""
				SiteCategory.SiteCategoryGroupID.Sort = ""
			End If

			' Reset start position
			lStartRec = 1
			SiteCategory.StartRecordNumber = lStartRec
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
		If SiteCategory.Export <> "" Or SiteCategory.CurrentAction = "gridadd" Or SiteCategory.CurrentAction = "gridedit" Then
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
	End Sub

	'
	'  Load search values for validation
	'
	Sub LoadSearchValues()
		SiteCategory.SiteCategoryID.AdvancedSearch.SearchValue = ew_Get("x_SiteCategoryID")
    	SiteCategory.SiteCategoryID.AdvancedSearch.SearchOperator = ew_Get("z_SiteCategoryID")
		SiteCategory.CategoryKeywords.AdvancedSearch.SearchValue = ew_Get("x_CategoryKeywords")
    	SiteCategory.CategoryKeywords.AdvancedSearch.SearchOperator = ew_Get("z_CategoryKeywords")
		SiteCategory.CategoryName.AdvancedSearch.SearchValue = ew_Get("x_CategoryName")
    	SiteCategory.CategoryName.AdvancedSearch.SearchOperator = ew_Get("z_CategoryName")
		SiteCategory.CategoryTitle.AdvancedSearch.SearchValue = ew_Get("x_CategoryTitle")
    	SiteCategory.CategoryTitle.AdvancedSearch.SearchOperator = ew_Get("z_CategoryTitle")
		SiteCategory.CategoryDescription.AdvancedSearch.SearchValue = ew_Get("x_CategoryDescription")
    	SiteCategory.CategoryDescription.AdvancedSearch.SearchOperator = ew_Get("z_CategoryDescription")
		SiteCategory.GroupOrder.AdvancedSearch.SearchValue = ew_Get("x_GroupOrder")
    	SiteCategory.GroupOrder.AdvancedSearch.SearchOperator = ew_Get("z_GroupOrder")
		SiteCategory.ParentCategoryID.AdvancedSearch.SearchValue = ew_Get("x_ParentCategoryID")
    	SiteCategory.ParentCategoryID.AdvancedSearch.SearchOperator = ew_Get("z_ParentCategoryID")
		SiteCategory.CategoryFileName.AdvancedSearch.SearchValue = ew_Get("x_CategoryFileName")
    	SiteCategory.CategoryFileName.AdvancedSearch.SearchOperator = ew_Get("z_CategoryFileName")
		SiteCategory.SiteCategoryTypeID.AdvancedSearch.SearchValue = ew_Get("x_SiteCategoryTypeID")
    	SiteCategory.SiteCategoryTypeID.AdvancedSearch.SearchOperator = ew_Get("z_SiteCategoryTypeID")
		SiteCategory.SiteCategoryGroupID.AdvancedSearch.SearchValue = ew_Get("x_SiteCategoryGroupID")
    	SiteCategory.SiteCategoryGroupID.AdvancedSearch.SearchOperator = ew_Get("z_SiteCategoryGroupID")
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
		If EW_DEBUG_ENABLED Then DebugMsg = sSql

		' Count
		lTotalRecs = -1		
		Try
			Dim sCntSql As String = SiteCategory.SelectCountSQL

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
		If EW_DEBUG_ENABLED Then DebugMsg = sSql
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
		Dim sDetailFilter As String
		SiteCategory.SiteCategoryID.DbValue = RsRow("SiteCategoryID")
		SiteCategory.CategoryKeywords.DbValue = RsRow("CategoryKeywords")
		SiteCategory.CategoryName.DbValue = RsRow("CategoryName")
		SiteCategory.CategoryTitle.DbValue = RsRow("CategoryTitle")
		SiteCategory.CategoryDescription.DbValue = RsRow("CategoryDescription")
		SiteCategory.GroupOrder.DbValue = RsRow("GroupOrder")
		SiteCategory.ParentCategoryID.DbValue = RsRow("ParentCategoryID")
		SiteCategory.CategoryFileName.DbValue = RsRow("CategoryFileName")
		SiteCategory.SiteCategoryTypeID.DbValue = RsRow("SiteCategoryTypeID")
		SiteCategory.SiteCategoryGroupID.DbValue = RsRow("SiteCategoryGroupID")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Initialize urls
		ViewUrl = SiteCategory.ViewUrl
		EditUrl = SiteCategory.EditUrl
		InlineEditUrl = SiteCategory.InlineEditUrl
		CopyUrl = SiteCategory.CopyUrl
		InlineCopyUrl = SiteCategory.InlineCopyUrl
		DeleteUrl = SiteCategory.DeleteUrl

		' Row Rendering event
		SiteCategory.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' SiteCategoryID

		SiteCategory.SiteCategoryID.CellCssStyle = ""
		SiteCategory.SiteCategoryID.CellCssClass = ""
		SiteCategory.SiteCategoryID.CellAttrs.Clear(): SiteCategory.SiteCategoryID.ViewAttrs.Clear(): SiteCategory.SiteCategoryID.EditAttrs.Clear()

		' CategoryKeywords
		SiteCategory.CategoryKeywords.CellCssStyle = ""
		SiteCategory.CategoryKeywords.CellCssClass = ""
		SiteCategory.CategoryKeywords.CellAttrs.Clear(): SiteCategory.CategoryKeywords.ViewAttrs.Clear(): SiteCategory.CategoryKeywords.EditAttrs.Clear()

		' CategoryName
		SiteCategory.CategoryName.CellCssStyle = ""
		SiteCategory.CategoryName.CellCssClass = ""
		SiteCategory.CategoryName.CellAttrs.Clear(): SiteCategory.CategoryName.ViewAttrs.Clear(): SiteCategory.CategoryName.EditAttrs.Clear()

		' CategoryTitle
		SiteCategory.CategoryTitle.CellCssStyle = ""
		SiteCategory.CategoryTitle.CellCssClass = ""
		SiteCategory.CategoryTitle.CellAttrs.Clear(): SiteCategory.CategoryTitle.ViewAttrs.Clear(): SiteCategory.CategoryTitle.EditAttrs.Clear()

		' CategoryDescription
		SiteCategory.CategoryDescription.CellCssStyle = ""
		SiteCategory.CategoryDescription.CellCssClass = ""
		SiteCategory.CategoryDescription.CellAttrs.Clear(): SiteCategory.CategoryDescription.ViewAttrs.Clear(): SiteCategory.CategoryDescription.EditAttrs.Clear()

		' GroupOrder
		SiteCategory.GroupOrder.CellCssStyle = ""
		SiteCategory.GroupOrder.CellCssClass = ""
		SiteCategory.GroupOrder.CellAttrs.Clear(): SiteCategory.GroupOrder.ViewAttrs.Clear(): SiteCategory.GroupOrder.EditAttrs.Clear()

		' ParentCategoryID
		SiteCategory.ParentCategoryID.CellCssStyle = ""
		SiteCategory.ParentCategoryID.CellCssClass = ""
		SiteCategory.ParentCategoryID.CellAttrs.Clear(): SiteCategory.ParentCategoryID.ViewAttrs.Clear(): SiteCategory.ParentCategoryID.EditAttrs.Clear()

		' CategoryFileName
		SiteCategory.CategoryFileName.CellCssStyle = ""
		SiteCategory.CategoryFileName.CellCssClass = ""
		SiteCategory.CategoryFileName.CellAttrs.Clear(): SiteCategory.CategoryFileName.ViewAttrs.Clear(): SiteCategory.CategoryFileName.EditAttrs.Clear()

		' SiteCategoryTypeID
		SiteCategory.SiteCategoryTypeID.CellCssStyle = ""
		SiteCategory.SiteCategoryTypeID.CellCssClass = ""
		SiteCategory.SiteCategoryTypeID.CellAttrs.Clear(): SiteCategory.SiteCategoryTypeID.ViewAttrs.Clear(): SiteCategory.SiteCategoryTypeID.EditAttrs.Clear()

		' SiteCategoryGroupID
		SiteCategory.SiteCategoryGroupID.CellCssStyle = ""
		SiteCategory.SiteCategoryGroupID.CellCssClass = ""
		SiteCategory.SiteCategoryGroupID.CellAttrs.Clear(): SiteCategory.SiteCategoryGroupID.ViewAttrs.Clear(): SiteCategory.SiteCategoryGroupID.EditAttrs.Clear()

		'
		'  View  Row
		'

		If SiteCategory.RowType = EW_ROWTYPE_VIEW Then ' View row

			' SiteCategoryID
			SiteCategory.SiteCategoryID.ViewValue = SiteCategory.SiteCategoryID.CurrentValue
			SiteCategory.SiteCategoryID.CssStyle = ""
			SiteCategory.SiteCategoryID.CssClass = ""
			SiteCategory.SiteCategoryID.ViewCustomAttributes = ""

			' CategoryKeywords
			SiteCategory.CategoryKeywords.ViewValue = SiteCategory.CategoryKeywords.CurrentValue
			SiteCategory.CategoryKeywords.CssStyle = ""
			SiteCategory.CategoryKeywords.CssClass = ""
			SiteCategory.CategoryKeywords.ViewCustomAttributes = ""

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

			' GroupOrder
			SiteCategory.GroupOrder.ViewValue = SiteCategory.GroupOrder.CurrentValue
			SiteCategory.GroupOrder.CssStyle = ""
			SiteCategory.GroupOrder.CssClass = ""
			SiteCategory.GroupOrder.ViewCustomAttributes = ""

			' ParentCategoryID
			SiteCategory.ParentCategoryID.ViewValue = SiteCategory.ParentCategoryID.CurrentValue
			SiteCategory.ParentCategoryID.CssStyle = ""
			SiteCategory.ParentCategoryID.CssClass = ""
			SiteCategory.ParentCategoryID.ViewCustomAttributes = ""

			' CategoryFileName
			SiteCategory.CategoryFileName.ViewValue = SiteCategory.CategoryFileName.CurrentValue
			SiteCategory.CategoryFileName.CssStyle = ""
			SiteCategory.CategoryFileName.CssClass = ""
			SiteCategory.CategoryFileName.ViewCustomAttributes = ""

			' SiteCategoryTypeID
			SiteCategory.SiteCategoryTypeID.ViewValue = SiteCategory.SiteCategoryTypeID.CurrentValue
			SiteCategory.SiteCategoryTypeID.CssStyle = ""
			SiteCategory.SiteCategoryTypeID.CssClass = ""
			SiteCategory.SiteCategoryTypeID.ViewCustomAttributes = ""

			' SiteCategoryGroupID
			SiteCategory.SiteCategoryGroupID.ViewValue = SiteCategory.SiteCategoryGroupID.CurrentValue
			SiteCategory.SiteCategoryGroupID.CssStyle = ""
			SiteCategory.SiteCategoryGroupID.CssClass = ""
			SiteCategory.SiteCategoryGroupID.ViewCustomAttributes = ""

			' View refer script
			' SiteCategoryID

			SiteCategory.SiteCategoryID.HrefValue = ""
			SiteCategory.SiteCategoryID.TooltipValue = ""

			' CategoryKeywords
			SiteCategory.CategoryKeywords.HrefValue = ""
			SiteCategory.CategoryKeywords.TooltipValue = ""

			' CategoryName
			SiteCategory.CategoryName.HrefValue = ""
			SiteCategory.CategoryName.TooltipValue = ""

			' CategoryTitle
			SiteCategory.CategoryTitle.HrefValue = ""
			SiteCategory.CategoryTitle.TooltipValue = ""

			' CategoryDescription
			SiteCategory.CategoryDescription.HrefValue = ""
			SiteCategory.CategoryDescription.TooltipValue = ""

			' GroupOrder
			SiteCategory.GroupOrder.HrefValue = ""
			SiteCategory.GroupOrder.TooltipValue = ""

			' ParentCategoryID
			SiteCategory.ParentCategoryID.HrefValue = ""
			SiteCategory.ParentCategoryID.TooltipValue = ""

			' CategoryFileName
			SiteCategory.CategoryFileName.HrefValue = ""
			SiteCategory.CategoryFileName.TooltipValue = ""

			' SiteCategoryTypeID
			SiteCategory.SiteCategoryTypeID.HrefValue = ""
			SiteCategory.SiteCategoryTypeID.TooltipValue = ""

			' SiteCategoryGroupID
			SiteCategory.SiteCategoryGroupID.HrefValue = ""
			SiteCategory.SiteCategoryGroupID.TooltipValue = ""
		End If

		' Row Rendered event
		If SiteCategory.RowType <> EW_ROWTYPE_AGGREGATEINIT Then
			SiteCategory.Row_Rendered()
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
		SiteCategory.SiteCategoryID.AdvancedSearch.SearchValue = SiteCategory.GetAdvancedSearch("x_SiteCategoryID")
		SiteCategory.CategoryKeywords.AdvancedSearch.SearchValue = SiteCategory.GetAdvancedSearch("x_CategoryKeywords")
		SiteCategory.CategoryName.AdvancedSearch.SearchValue = SiteCategory.GetAdvancedSearch("x_CategoryName")
		SiteCategory.CategoryTitle.AdvancedSearch.SearchValue = SiteCategory.GetAdvancedSearch("x_CategoryTitle")
		SiteCategory.CategoryDescription.AdvancedSearch.SearchValue = SiteCategory.GetAdvancedSearch("x_CategoryDescription")
		SiteCategory.GroupOrder.AdvancedSearch.SearchValue = SiteCategory.GetAdvancedSearch("x_GroupOrder")
		SiteCategory.ParentCategoryID.AdvancedSearch.SearchValue = SiteCategory.GetAdvancedSearch("x_ParentCategoryID")
		SiteCategory.CategoryFileName.AdvancedSearch.SearchValue = SiteCategory.GetAdvancedSearch("x_CategoryFileName")
		SiteCategory.SiteCategoryTypeID.AdvancedSearch.SearchValue = SiteCategory.GetAdvancedSearch("x_SiteCategoryTypeID")
		SiteCategory.SiteCategoryGroupID.AdvancedSearch.SearchValue = SiteCategory.GetAdvancedSearch("x_SiteCategoryGroupID")
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
			oXmlDoc = New cXMLDocument()
		Else
			sOutputStr = ew_ExportHeader(SiteCategory.Export)

			' Horizontal format, write header
			If sExportStyle <> "v" OrElse SiteCategory.Export = "csv" Then
				sExportStr = ""
				ew_ExportAddValue(sExportStr, SiteCategory.SiteCategoryID.ExportCaption, SiteCategory.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategory.SiteCategoryID.CellStyles, ""))
				ew_ExportAddValue(sExportStr, SiteCategory.CategoryKeywords.ExportCaption, SiteCategory.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategory.CategoryKeywords.CellStyles, ""))
				ew_ExportAddValue(sExportStr, SiteCategory.CategoryName.ExportCaption, SiteCategory.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategory.CategoryName.CellStyles, ""))
				ew_ExportAddValue(sExportStr, SiteCategory.CategoryTitle.ExportCaption, SiteCategory.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategory.CategoryTitle.CellStyles, ""))
				ew_ExportAddValue(sExportStr, SiteCategory.CategoryDescription.ExportCaption, SiteCategory.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategory.CategoryDescription.CellStyles, ""))
				ew_ExportAddValue(sExportStr, SiteCategory.GroupOrder.ExportCaption, SiteCategory.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategory.GroupOrder.CellStyles, ""))
				ew_ExportAddValue(sExportStr, SiteCategory.ParentCategoryID.ExportCaption, SiteCategory.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategory.ParentCategoryID.CellStyles, ""))
				ew_ExportAddValue(sExportStr, SiteCategory.CategoryFileName.ExportCaption, SiteCategory.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategory.CategoryFileName.CellStyles, ""))
				ew_ExportAddValue(sExportStr, SiteCategory.SiteCategoryTypeID.ExportCaption, SiteCategory.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategory.SiteCategoryTypeID.CellStyles, ""))
				ew_ExportAddValue(sExportStr, SiteCategory.SiteCategoryGroupID.ExportCaption, SiteCategory.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategory.SiteCategoryGroupID.CellStyles, ""))
				sOutputStr &= ew_ExportLine(sExportStr, SiteCategory.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategory.RowStyles, ""))
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
				SiteCategory.CssClass = ""
				SiteCategory.CssStyle = ""
				SiteCategory.RowType = EW_ROWTYPE_VIEW ' Render view
				RenderRow()
				If SiteCategory.Export = "xml" Then
					oXmlDoc.AddRow()
					oXmlDoc.AddField("SiteCategoryID", SiteCategory.SiteCategoryID.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue)) ' SiteCategoryID
					oXmlDoc.AddField("CategoryKeywords", SiteCategory.CategoryKeywords.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue)) ' CategoryKeywords
					oXmlDoc.AddField("CategoryName", SiteCategory.CategoryName.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue)) ' CategoryName
					oXmlDoc.AddField("CategoryTitle", SiteCategory.CategoryTitle.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue)) ' CategoryTitle
					oXmlDoc.AddField("CategoryDescription", SiteCategory.CategoryDescription.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue)) ' CategoryDescription
					oXmlDoc.AddField("GroupOrder", SiteCategory.GroupOrder.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue)) ' GroupOrder
					oXmlDoc.AddField("ParentCategoryID", SiteCategory.ParentCategoryID.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue)) ' ParentCategoryID
					oXmlDoc.AddField("CategoryFileName", SiteCategory.CategoryFileName.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue)) ' CategoryFileName
					oXmlDoc.AddField("SiteCategoryTypeID", SiteCategory.SiteCategoryTypeID.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue)) ' SiteCategoryTypeID
					oXmlDoc.AddField("SiteCategoryGroupID", SiteCategory.SiteCategoryGroupID.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue)) ' SiteCategoryGroupID
				Else

					' Vertical format
					If sExportStyle = "v" AndAlso SiteCategory.Export <> "csv" Then
						sOutputStr &= ew_ExportField(SiteCategory.SiteCategoryID.ExportCaption, SiteCategory.SiteCategoryID.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue), SiteCategory.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategory.SiteCategoryID.CellStyles, "")) ' SiteCategoryID
						sOutputStr &= ew_ExportField(SiteCategory.CategoryKeywords.ExportCaption, SiteCategory.CategoryKeywords.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue), SiteCategory.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategory.CategoryKeywords.CellStyles, "")) ' CategoryKeywords
						sOutputStr &= ew_ExportField(SiteCategory.CategoryName.ExportCaption, SiteCategory.CategoryName.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue), SiteCategory.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategory.CategoryName.CellStyles, "")) ' CategoryName
						sOutputStr &= ew_ExportField(SiteCategory.CategoryTitle.ExportCaption, SiteCategory.CategoryTitle.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue), SiteCategory.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategory.CategoryTitle.CellStyles, "")) ' CategoryTitle
						sOutputStr &= ew_ExportField(SiteCategory.CategoryDescription.ExportCaption, SiteCategory.CategoryDescription.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue), SiteCategory.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategory.CategoryDescription.CellStyles, "")) ' CategoryDescription
						sOutputStr &= ew_ExportField(SiteCategory.GroupOrder.ExportCaption, SiteCategory.GroupOrder.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue), SiteCategory.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategory.GroupOrder.CellStyles, "")) ' GroupOrder
						sOutputStr &= ew_ExportField(SiteCategory.ParentCategoryID.ExportCaption, SiteCategory.ParentCategoryID.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue), SiteCategory.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategory.ParentCategoryID.CellStyles, "")) ' ParentCategoryID
						sOutputStr &= ew_ExportField(SiteCategory.CategoryFileName.ExportCaption, SiteCategory.CategoryFileName.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue), SiteCategory.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategory.CategoryFileName.CellStyles, "")) ' CategoryFileName
						sOutputStr &= ew_ExportField(SiteCategory.SiteCategoryTypeID.ExportCaption, SiteCategory.SiteCategoryTypeID.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue), SiteCategory.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategory.SiteCategoryTypeID.CellStyles, "")) ' SiteCategoryTypeID
						sOutputStr &= ew_ExportField(SiteCategory.SiteCategoryGroupID.ExportCaption, SiteCategory.SiteCategoryGroupID.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue), SiteCategory.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategory.SiteCategoryGroupID.CellStyles, "")) ' SiteCategoryGroupID

					' Horizontal format
					Else
						sExportStr = ""
						ew_ExportAddValue(sExportStr, SiteCategory.SiteCategoryID.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue), SiteCategory.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategory.SiteCategoryID.CellStyles, ""))
						ew_ExportAddValue(sExportStr, SiteCategory.CategoryKeywords.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue), SiteCategory.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategory.CategoryKeywords.CellStyles, ""))
						ew_ExportAddValue(sExportStr, SiteCategory.CategoryName.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue), SiteCategory.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategory.CategoryName.CellStyles, ""))
						ew_ExportAddValue(sExportStr, SiteCategory.CategoryTitle.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue), SiteCategory.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategory.CategoryTitle.CellStyles, ""))
						ew_ExportAddValue(sExportStr, SiteCategory.CategoryDescription.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue), SiteCategory.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategory.CategoryDescription.CellStyles, ""))
						ew_ExportAddValue(sExportStr, SiteCategory.GroupOrder.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue), SiteCategory.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategory.GroupOrder.CellStyles, ""))
						ew_ExportAddValue(sExportStr, SiteCategory.ParentCategoryID.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue), SiteCategory.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategory.ParentCategoryID.CellStyles, ""))
						ew_ExportAddValue(sExportStr, SiteCategory.CategoryFileName.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue), SiteCategory.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategory.CategoryFileName.CellStyles, ""))
						ew_ExportAddValue(sExportStr, SiteCategory.SiteCategoryTypeID.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue), SiteCategory.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategory.SiteCategoryTypeID.CellStyles, ""))
						ew_ExportAddValue(sExportStr, SiteCategory.SiteCategoryGroupID.ExportValue(SiteCategory.Export, SiteCategory.ExportOriginalValue), SiteCategory.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategory.SiteCategoryGroupID.CellStyles, ""))
						sOutputStr &= ew_ExportLine(sExportStr, SiteCategory.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategory.RowStyles, ""))
					End If
				End If
			End If
		Loop

		' Close recordset
		Rs.Close()
		Rs.Dispose()
		If SiteCategory.Export = "xml" Then
			oXmlDoc.Output
		Else
			sOutputStr &= ew_ExportFooter(SiteCategory.Export)
			ew_Write(sOutputStr)
		End If
	End Sub

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "SiteCategory"
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
		SiteCategory_list = New cSiteCategory_list(Me)		
		SiteCategory_list.Page_Init()		

		' Set buffer/cache option
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

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
