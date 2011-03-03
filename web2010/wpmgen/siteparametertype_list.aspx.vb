Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class siteparametertype_list
	Inherits AspNetMaker8_wpmWebsite

	' Page object
	Public SiteParameterType_list As cSiteParameterType_list

	'
	' Page Class
	'
	Class cSiteParameterType_list
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
				If SiteParameterType.UseTokenInUrl Then Url = Url & "t=" & SiteParameterType.TableVar & "&" ' Add page token
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
			If SiteParameterType.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (SiteParameterType.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (SiteParameterType.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' ASP.NET page object
		Public ReadOnly Property AspNetPage() As siteparametertype_list
			Get
				Return CType(m_ParentPage, siteparametertype_list)
			End Get
		End Property

		' SiteParameterType
		Public Property SiteParameterType() As cSiteParameterType
			Get				
				Return ParentPage.SiteParameterType
			End Get
			Set(ByVal v As cSiteParameterType)
				ParentPage.SiteParameterType = v	
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
			m_PageObjName = "SiteParameterType_list"
			m_PageObjTypeName = "cSiteParameterType_list"			

			' Initialize language object
			Language = New cLanguage(Me)

			' Table Name
			m_TableName = "SiteParameterType"

			' Initialize table object
			SiteParameterType = New cSiteParameterType(Me)

			' Initialize URLs
			ExportPrintUrl = PageUrl & "export=print"
			ExportExcelUrl = PageUrl & "export=excel"
			ExportWordUrl = PageUrl & "export=word"
			ExportHtmlUrl = PageUrl & "export=html"
			ExportXmlUrl = PageUrl & "export=xml"
			ExportCsvUrl = PageUrl & "export=csv"
			AddUrl = SiteParameterType.AddUrl
			InlineAddUrl = PageUrl & "a=add"
			GridAddUrl = PageUrl & "a=gridadd"
			GridEditUrl = PageUrl & "a=gridedit"
			MultiDeleteUrl = "siteparametertype_delete.aspx"
			MultiUpdateUrl = "siteparametertype_update.aspx"

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
				SiteParameterType.Export = ew_Get("export")
			ElseIf ew_Post("exporttype") <> "" Then
				SiteParameterType.Export = ew_Post("exporttype")
			Else
				SiteParameterType.ExportReturnUrl = ew_CurrentUrl()
			End If
			ParentPage.gsExport = SiteParameterType.Export ' Get export parameter, used in header
			ParentPage.gsExportFile = SiteParameterType.TableVar ' Get export file, used in header
			If SiteParameterType.Export = "excel" Then
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
			SiteParameterType.Dispose()
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
			Call SiteParameterType.Recordset_SearchValidated()

			' Set Up Sorting Order
			SetUpSortOrder()

			' Get search criteria for advanced search
			If ParentPage.gsSearchError = "" Then
				sSrchAdvanced = AdvancedSearchWhere()
			End If
		End If

		' Restore display records
		If (SiteParameterType.RecordsPerPage = -1 OrElse SiteParameterType.RecordsPerPage > 0) Then
			lDisplayRecs = SiteParameterType.RecordsPerPage ' Restore from Session
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
		SiteParameterType.Recordset_Searching(sSrchWhere)

		' Save search criteria
		If sSrchWhere <> "" Then
			If sSrchAdvanced = "" Then ResetAdvancedSearchParms()
			SiteParameterType.SearchWhere = sSrchWhere ' Save to Session
			If RestoreSearch Then
				lStartRec = 1 ' Reset start record counter
				SiteParameterType.StartRecordNumber = lStartRec
			End If
		Else
			sSrchWhere = SiteParameterType.SearchWhere
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
		SiteParameterType.SessionWhere = sFilter
		SiteParameterType.CurrentFilter = ""

		' Export Data only
		If SiteParameterType.Export = "html" OrElse SiteParameterType.Export = "csv" OrElse SiteParameterType.Export = "word" OrElse SiteParameterType.Export = "excel" OrElse SiteParameterType.Export = "xml" Then
			ExportData()
			Page_Terminate("") ' Clean up
			ew_End() ' Terminate response
		ElseIf SiteParameterType.Export = "email" Then
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
			SiteParameterType.RecordsPerPage = lDisplayRecs ' Save to Session

			' Reset start position
			lStartRec = 1
			SiteParameterType.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	' Return Advanced Search WHERE based on QueryString parameters
	'
	Function AdvancedSearchWhere() As String		
		Dim sWhere As String = ""
		BuildSearchSql(sWhere, SiteParameterType.SiteParameterTypeNM, False) ' SiteParameterTypeNM
		BuildSearchSql(sWhere, SiteParameterType.SiteParameterTypeDS, False) ' SiteParameterTypeDS
		BuildSearchSql(sWhere, SiteParameterType.SiteParameterTypeOrder, False) ' SiteParameterTypeOrder
		BuildSearchSql(sWhere, SiteParameterType.SiteParameterTemplate, False) ' SiteParameterTemplate

		' Set up search parm
		If sWhere <> "" Then
			SetSearchParm(SiteParameterType.SiteParameterTypeNM) ' SiteParameterTypeNM
			SetSearchParm(SiteParameterType.SiteParameterTypeDS) ' SiteParameterTypeDS
			SetSearchParm(SiteParameterType.SiteParameterTypeOrder) ' SiteParameterTypeOrder
			SetSearchParm(SiteParameterType.SiteParameterTemplate) ' SiteParameterTemplate
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
		SiteParameterType.SetAdvancedSearch("x_" & FldParm, Fld.AdvancedSearch.SearchValue)
		SiteParameterType.SetAdvancedSearch("z_" & FldParm, Fld.AdvancedSearch.SearchOperator)
		SiteParameterType.SetAdvancedSearch("v_" & FldParm, Fld.AdvancedSearch.SearchCondition)
		SiteParameterType.SetAdvancedSearch("y_" & FldParm, Fld.AdvancedSearch.SearchValue2)
		SiteParameterType.SetAdvancedSearch("w_" & FldParm, Fld.AdvancedSearch.SearchOperator2)
	End Sub

	'
	' Get search parm
	'
	Sub GetSearchParm(ByRef Fld As Object)
		Dim FldParm As String = Fld.FldVar.Substring(2)
		Fld.AdvancedSearch.SearchValue = SiteParameterType.GetAdvancedSearch("x_" & FldParm)
		Fld.AdvancedSearch.SearchOperator = SiteParameterType.GetAdvancedSearch("z_" & FldParm)
		Fld.AdvancedSearch.SearchCondition = SiteParameterType.GetAdvancedSearch("v_" & FldParm)
		Fld.AdvancedSearch.SearchValue2 = SiteParameterType.GetAdvancedSearch("y_" & FldParm)
		Fld.AdvancedSearch.SearchOperator2 = SiteParameterType.GetAdvancedSearch("w_" & FldParm)
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
		SiteParameterType.SearchWhere = sSrchWhere

		' Clear advanced search parameters
		ResetAdvancedSearchParms()
	End Sub

	'
	' Clear all advanced search parameters
	'
	Sub ResetAdvancedSearchParms()
		SiteParameterType.SetAdvancedSearch("x_SiteParameterTypeNM", "")
		SiteParameterType.SetAdvancedSearch("x_SiteParameterTypeDS", "")
		SiteParameterType.SetAdvancedSearch("x_SiteParameterTypeOrder", "")
		SiteParameterType.SetAdvancedSearch("x_SiteParameterTemplate", "")
	End Sub

	'
	' Restore all search parameters
	'
	Sub RestoreSearchParms()
		Dim bRestore As Boolean = True
		If ew_Get("x_SiteParameterTypeNM") <> "" Then bRestore = False
		If ew_Get("x_SiteParameterTypeDS") <> "" Then bRestore = False
		If ew_Get("x_SiteParameterTypeOrder") <> "" Then bRestore = False
		If ew_Get("x_SiteParameterTemplate") <> "" Then bRestore = False
		RestoreSearch = bRestore
		If bRestore Then

			' Restore advanced search values
			Call GetSearchParm(SiteParameterType.SiteParameterTypeNM)
			Call GetSearchParm(SiteParameterType.SiteParameterTypeDS)
			Call GetSearchParm(SiteParameterType.SiteParameterTypeOrder)
			Call GetSearchParm(SiteParameterType.SiteParameterTemplate)
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
			SiteParameterType.CurrentOrder = ew_Get("order")
			SiteParameterType.CurrentOrderType = ew_Get("ordertype")
			SiteParameterType.UpdateSort(SiteParameterType.SiteParameterTypeNM) ' SiteParameterTypeNM
			SiteParameterType.UpdateSort(SiteParameterType.SiteParameterTypeDS) ' SiteParameterTypeDS
			SiteParameterType.UpdateSort(SiteParameterType.SiteParameterTypeOrder) ' SiteParameterTypeOrder
			SiteParameterType.StartRecordNumber = 1 ' Reset start position
		End If
	End Sub

	'
	' Load Sort Order parameters
	'
	Sub LoadSortOrder()
		Dim sOrderBy As String
		sOrderBy = SiteParameterType.SessionOrderBy ' Get order by from Session
		If sOrderBy = "" Then
			If SiteParameterType.SqlOrderBy <> "" Then
				sOrderBy = SiteParameterType.SqlOrderBy
				SiteParameterType.SessionOrderBy = sOrderBy
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
				SiteParameterType.SessionOrderBy = sOrderBy
				SiteParameterType.SiteParameterTypeNM.Sort = ""
				SiteParameterType.SiteParameterTypeDS.Sort = ""
				SiteParameterType.SiteParameterTypeOrder.Sort = ""
			End If

			' Reset start position
			lStartRec = 1
			SiteParameterType.StartRecordNumber = lStartRec
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
		If SiteParameterType.Export <> "" Or SiteParameterType.CurrentAction = "gridadd" Or SiteParameterType.CurrentAction = "gridedit" Then
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
				SiteParameterType.StartRecordNumber = lStartRec
			ElseIf ew_Get(EW_TABLE_PAGE_NO) <> "" AndAlso IsNumeric(ew_Get(EW_TABLE_PAGE_NO)) Then
				nPageNo = ew_ConvertToInt(ew_Get(EW_TABLE_PAGE_NO))
				lStartRec = (nPageNo-1)*lDisplayRecs+1
				If lStartRec <= 0 Then
					lStartRec = 1
				ElseIf lStartRec >= ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 Then
					lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1
				End If
				SiteParameterType.StartRecordNumber = lStartRec
			End If
		End If
		lStartRec = SiteParameterType.StartRecordNumber

		' Check if correct start record counter
		If lStartRec <= 0 Then ' Avoid invalid start record counter
			lStartRec = 1 ' Reset start record counter
			SiteParameterType.StartRecordNumber = lStartRec
		ElseIf lStartRec > lTotalRecs Then ' Avoid starting record > total records
			lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to last page first record
			SiteParameterType.StartRecordNumber = lStartRec
		ElseIf (lStartRec-1) Mod lDisplayRecs <> 0 Then
			lStartRec = ((lStartRec-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to page boundary
			SiteParameterType.StartRecordNumber = lStartRec
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
		SiteParameterType.SiteParameterTypeNM.AdvancedSearch.SearchValue = ew_Get("x_SiteParameterTypeNM")
    	SiteParameterType.SiteParameterTypeNM.AdvancedSearch.SearchOperator = ew_Get("z_SiteParameterTypeNM")
		SiteParameterType.SiteParameterTypeDS.AdvancedSearch.SearchValue = ew_Get("x_SiteParameterTypeDS")
    	SiteParameterType.SiteParameterTypeDS.AdvancedSearch.SearchOperator = ew_Get("z_SiteParameterTypeDS")
		SiteParameterType.SiteParameterTypeOrder.AdvancedSearch.SearchValue = ew_Get("x_SiteParameterTypeOrder")
    	SiteParameterType.SiteParameterTypeOrder.AdvancedSearch.SearchOperator = ew_Get("z_SiteParameterTypeOrder")
		SiteParameterType.SiteParameterTemplate.AdvancedSearch.SearchValue = ew_Get("x_SiteParameterTemplate")
    	SiteParameterType.SiteParameterTemplate.AdvancedSearch.SearchOperator = ew_Get("z_SiteParameterTemplate")
	End Sub

	'
	' Load recordset
	'
	Function LoadRecordset() As OleDbDataReader

		' Recordset Selecting event
		SiteParameterType.Recordset_Selecting(SiteParameterType.CurrentFilter)

		' Load list page SQL
		Dim sSql As String = SiteParameterType.ListSQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then DebugMsg = sSql

		' Count
		lTotalRecs = -1		
		Try
			Dim sCntSql As String = SiteParameterType.SelectCountSQL

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
		SiteParameterType.Recordset_Selected(Rs)
		Return Rs
	End Function

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = SiteParameterType.KeyFilter

		' Row Selecting event
		SiteParameterType.Row_Selecting(sFilter)

		' Load SQL based on filter
		SiteParameterType.CurrentFilter = sFilter
		Dim sSql As String = SiteParameterType.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then DebugMsg = sSql
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				SiteParameterType.Row_Selected(RsRow)
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
		SiteParameterType.SiteParameterTypeID.DbValue = RsRow("SiteParameterTypeID")
		SiteParameterType.SiteParameterTypeNM.DbValue = RsRow("SiteParameterTypeNM")
		SiteParameterType.SiteParameterTypeDS.DbValue = RsRow("SiteParameterTypeDS")
		SiteParameterType.SiteParameterTypeOrder.DbValue = RsRow("SiteParameterTypeOrder")
		SiteParameterType.SiteParameterTemplate.DbValue = RsRow("SiteParameterTemplate")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Initialize urls
		ViewUrl = SiteParameterType.ViewUrl
		EditUrl = SiteParameterType.EditUrl
		InlineEditUrl = SiteParameterType.InlineEditUrl
		CopyUrl = SiteParameterType.CopyUrl
		InlineCopyUrl = SiteParameterType.InlineCopyUrl
		DeleteUrl = SiteParameterType.DeleteUrl

		' Row Rendering event
		SiteParameterType.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' SiteParameterTypeNM

		SiteParameterType.SiteParameterTypeNM.CellCssStyle = "white-space: nowrap;"
		SiteParameterType.SiteParameterTypeNM.CellCssClass = ""
		SiteParameterType.SiteParameterTypeNM.CellAttrs.Clear(): SiteParameterType.SiteParameterTypeNM.ViewAttrs.Clear(): SiteParameterType.SiteParameterTypeNM.EditAttrs.Clear()

		' SiteParameterTypeDS
		SiteParameterType.SiteParameterTypeDS.CellCssStyle = "white-space: nowrap;"
		SiteParameterType.SiteParameterTypeDS.CellCssClass = ""
		SiteParameterType.SiteParameterTypeDS.CellAttrs.Clear(): SiteParameterType.SiteParameterTypeDS.ViewAttrs.Clear(): SiteParameterType.SiteParameterTypeDS.EditAttrs.Clear()

		' SiteParameterTypeOrder
		SiteParameterType.SiteParameterTypeOrder.CellCssStyle = "white-space: nowrap;"
		SiteParameterType.SiteParameterTypeOrder.CellCssClass = ""
		SiteParameterType.SiteParameterTypeOrder.CellAttrs.Clear(): SiteParameterType.SiteParameterTypeOrder.ViewAttrs.Clear(): SiteParameterType.SiteParameterTypeOrder.EditAttrs.Clear()

		'
		'  View  Row
		'

		If SiteParameterType.RowType = EW_ROWTYPE_VIEW Then ' View row

			' SiteParameterTypeID
			SiteParameterType.SiteParameterTypeID.ViewValue = SiteParameterType.SiteParameterTypeID.CurrentValue
			SiteParameterType.SiteParameterTypeID.CssStyle = ""
			SiteParameterType.SiteParameterTypeID.CssClass = ""
			SiteParameterType.SiteParameterTypeID.ViewCustomAttributes = ""

			' SiteParameterTypeNM
			SiteParameterType.SiteParameterTypeNM.ViewValue = SiteParameterType.SiteParameterTypeNM.CurrentValue
			SiteParameterType.SiteParameterTypeNM.CssStyle = ""
			SiteParameterType.SiteParameterTypeNM.CssClass = ""
			SiteParameterType.SiteParameterTypeNM.ViewCustomAttributes = ""

			' SiteParameterTypeDS
			SiteParameterType.SiteParameterTypeDS.ViewValue = SiteParameterType.SiteParameterTypeDS.CurrentValue
			SiteParameterType.SiteParameterTypeDS.CssStyle = ""
			SiteParameterType.SiteParameterTypeDS.CssClass = ""
			SiteParameterType.SiteParameterTypeDS.ViewCustomAttributes = ""

			' SiteParameterTypeOrder
			SiteParameterType.SiteParameterTypeOrder.ViewValue = SiteParameterType.SiteParameterTypeOrder.CurrentValue
			SiteParameterType.SiteParameterTypeOrder.CssStyle = ""
			SiteParameterType.SiteParameterTypeOrder.CssClass = ""
			SiteParameterType.SiteParameterTypeOrder.ViewCustomAttributes = ""

			' View refer script
			' SiteParameterTypeNM

			SiteParameterType.SiteParameterTypeNM.HrefValue = ""
			SiteParameterType.SiteParameterTypeNM.TooltipValue = ""

			' SiteParameterTypeDS
			SiteParameterType.SiteParameterTypeDS.HrefValue = ""
			SiteParameterType.SiteParameterTypeDS.TooltipValue = ""

			' SiteParameterTypeOrder
			SiteParameterType.SiteParameterTypeOrder.HrefValue = ""
			SiteParameterType.SiteParameterTypeOrder.TooltipValue = ""
		End If

		' Row Rendered event
		If SiteParameterType.RowType <> EW_ROWTYPE_AGGREGATEINIT Then
			SiteParameterType.Row_Rendered()
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
		SiteParameterType.SiteParameterTypeNM.AdvancedSearch.SearchValue = SiteParameterType.GetAdvancedSearch("x_SiteParameterTypeNM")
		SiteParameterType.SiteParameterTypeDS.AdvancedSearch.SearchValue = SiteParameterType.GetAdvancedSearch("x_SiteParameterTypeDS")
		SiteParameterType.SiteParameterTypeOrder.AdvancedSearch.SearchValue = SiteParameterType.GetAdvancedSearch("x_SiteParameterTypeOrder")
		SiteParameterType.SiteParameterTemplate.AdvancedSearch.SearchValue = SiteParameterType.GetAdvancedSearch("x_SiteParameterTemplate")
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
		If SiteParameterType.ExportAll Then
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
		If SiteParameterType.Export = "xml" Then
			oXmlDoc = New cXMLDocument()
		Else
			sOutputStr = ew_ExportHeader(SiteParameterType.Export)

			' Horizontal format, write header
			If sExportStyle <> "v" OrElse SiteParameterType.Export = "csv" Then
				sExportStr = ""
				ew_ExportAddValue(sExportStr, SiteParameterType.SiteParameterTypeID.ExportCaption, SiteParameterType.Export, IIf(EW_EXPORT_CSS_STYLES, SiteParameterType.SiteParameterTypeID.CellStyles, ""))
				ew_ExportAddValue(sExportStr, SiteParameterType.SiteParameterTypeNM.ExportCaption, SiteParameterType.Export, IIf(EW_EXPORT_CSS_STYLES, SiteParameterType.SiteParameterTypeNM.CellStyles, ""))
				ew_ExportAddValue(sExportStr, SiteParameterType.SiteParameterTypeDS.ExportCaption, SiteParameterType.Export, IIf(EW_EXPORT_CSS_STYLES, SiteParameterType.SiteParameterTypeDS.CellStyles, ""))
				ew_ExportAddValue(sExportStr, SiteParameterType.SiteParameterTypeOrder.ExportCaption, SiteParameterType.Export, IIf(EW_EXPORT_CSS_STYLES, SiteParameterType.SiteParameterTypeOrder.CellStyles, ""))
				sOutputStr &= ew_ExportLine(sExportStr, SiteParameterType.Export, IIf(EW_EXPORT_CSS_STYLES, SiteParameterType.RowStyles, ""))
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
				SiteParameterType.CssClass = ""
				SiteParameterType.CssStyle = ""
				SiteParameterType.RowType = EW_ROWTYPE_VIEW ' Render view
				RenderRow()
				If SiteParameterType.Export = "xml" Then
					oXmlDoc.AddRow()
					oXmlDoc.AddField("SiteParameterTypeID", SiteParameterType.SiteParameterTypeID.ExportValue(SiteParameterType.Export, SiteParameterType.ExportOriginalValue)) ' SiteParameterTypeID
					oXmlDoc.AddField("SiteParameterTypeNM", SiteParameterType.SiteParameterTypeNM.ExportValue(SiteParameterType.Export, SiteParameterType.ExportOriginalValue)) ' SiteParameterTypeNM
					oXmlDoc.AddField("SiteParameterTypeDS", SiteParameterType.SiteParameterTypeDS.ExportValue(SiteParameterType.Export, SiteParameterType.ExportOriginalValue)) ' SiteParameterTypeDS
					oXmlDoc.AddField("SiteParameterTypeOrder", SiteParameterType.SiteParameterTypeOrder.ExportValue(SiteParameterType.Export, SiteParameterType.ExportOriginalValue)) ' SiteParameterTypeOrder
				Else

					' Vertical format
					If sExportStyle = "v" AndAlso SiteParameterType.Export <> "csv" Then
						sOutputStr &= ew_ExportField(SiteParameterType.SiteParameterTypeID.ExportCaption, SiteParameterType.SiteParameterTypeID.ExportValue(SiteParameterType.Export, SiteParameterType.ExportOriginalValue), SiteParameterType.Export, IIf(EW_EXPORT_CSS_STYLES, SiteParameterType.SiteParameterTypeID.CellStyles, "")) ' SiteParameterTypeID
						sOutputStr &= ew_ExportField(SiteParameterType.SiteParameterTypeNM.ExportCaption, SiteParameterType.SiteParameterTypeNM.ExportValue(SiteParameterType.Export, SiteParameterType.ExportOriginalValue), SiteParameterType.Export, IIf(EW_EXPORT_CSS_STYLES, SiteParameterType.SiteParameterTypeNM.CellStyles, "")) ' SiteParameterTypeNM
						sOutputStr &= ew_ExportField(SiteParameterType.SiteParameterTypeDS.ExportCaption, SiteParameterType.SiteParameterTypeDS.ExportValue(SiteParameterType.Export, SiteParameterType.ExportOriginalValue), SiteParameterType.Export, IIf(EW_EXPORT_CSS_STYLES, SiteParameterType.SiteParameterTypeDS.CellStyles, "")) ' SiteParameterTypeDS
						sOutputStr &= ew_ExportField(SiteParameterType.SiteParameterTypeOrder.ExportCaption, SiteParameterType.SiteParameterTypeOrder.ExportValue(SiteParameterType.Export, SiteParameterType.ExportOriginalValue), SiteParameterType.Export, IIf(EW_EXPORT_CSS_STYLES, SiteParameterType.SiteParameterTypeOrder.CellStyles, "")) ' SiteParameterTypeOrder

					' Horizontal format
					Else
						sExportStr = ""
						ew_ExportAddValue(sExportStr, SiteParameterType.SiteParameterTypeID.ExportValue(SiteParameterType.Export, SiteParameterType.ExportOriginalValue), SiteParameterType.Export, IIf(EW_EXPORT_CSS_STYLES, SiteParameterType.SiteParameterTypeID.CellStyles, ""))
						ew_ExportAddValue(sExportStr, SiteParameterType.SiteParameterTypeNM.ExportValue(SiteParameterType.Export, SiteParameterType.ExportOriginalValue), SiteParameterType.Export, IIf(EW_EXPORT_CSS_STYLES, SiteParameterType.SiteParameterTypeNM.CellStyles, ""))
						ew_ExportAddValue(sExportStr, SiteParameterType.SiteParameterTypeDS.ExportValue(SiteParameterType.Export, SiteParameterType.ExportOriginalValue), SiteParameterType.Export, IIf(EW_EXPORT_CSS_STYLES, SiteParameterType.SiteParameterTypeDS.CellStyles, ""))
						ew_ExportAddValue(sExportStr, SiteParameterType.SiteParameterTypeOrder.ExportValue(SiteParameterType.Export, SiteParameterType.ExportOriginalValue), SiteParameterType.Export, IIf(EW_EXPORT_CSS_STYLES, SiteParameterType.SiteParameterTypeOrder.CellStyles, ""))
						sOutputStr &= ew_ExportLine(sExportStr, SiteParameterType.Export, IIf(EW_EXPORT_CSS_STYLES, SiteParameterType.RowStyles, ""))
					End If
				End If
			End If
		Loop

		' Close recordset
		Rs.Close()
		Rs.Dispose()
		If SiteParameterType.Export = "xml" Then
			oXmlDoc.Output
		Else
			sOutputStr &= ew_ExportFooter(SiteParameterType.Export)
			ew_Write(sOutputStr)
		End If
	End Sub

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "SiteParameterType"
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
		SiteParameterType_list = New cSiteParameterType_list(Me)		
		SiteParameterType_list.Page_Init()		

		' Set buffer/cache option
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

		' Page main processing
		SiteParameterType_list.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If SiteParameterType_list IsNot Nothing Then SiteParameterType_list.Dispose()
	End Sub
End Class
