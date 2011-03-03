Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class sitecategorygroup_list
	Inherits AspNetMaker8_wpmWebsite

	' Page object
	Public SiteCategoryGroup_list As cSiteCategoryGroup_list

	'
	' Page Class
	'
	Class cSiteCategoryGroup_list
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
				If SiteCategoryGroup.UseTokenInUrl Then Url = Url & "t=" & SiteCategoryGroup.TableVar & "&" ' Add page token
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
			If SiteCategoryGroup.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (SiteCategoryGroup.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (SiteCategoryGroup.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' ASP.NET page object
		Public ReadOnly Property AspNetPage() As sitecategorygroup_list
			Get
				Return CType(m_ParentPage, sitecategorygroup_list)
			End Get
		End Property

		' SiteCategoryGroup
		Public Property SiteCategoryGroup() As cSiteCategoryGroup
			Get				
				Return ParentPage.SiteCategoryGroup
			End Get
			Set(ByVal v As cSiteCategoryGroup)
				ParentPage.SiteCategoryGroup = v	
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
			m_PageObjName = "SiteCategoryGroup_list"
			m_PageObjTypeName = "cSiteCategoryGroup_list"			

			' Initialize language object
			Language = New cLanguage(Me)

			' Table Name
			m_TableName = "SiteCategoryGroup"

			' Initialize table object
			SiteCategoryGroup = New cSiteCategoryGroup(Me)

			' Initialize URLs
			ExportPrintUrl = PageUrl & "export=print"
			ExportExcelUrl = PageUrl & "export=excel"
			ExportWordUrl = PageUrl & "export=word"
			ExportHtmlUrl = PageUrl & "export=html"
			ExportXmlUrl = PageUrl & "export=xml"
			ExportCsvUrl = PageUrl & "export=csv"
			AddUrl = SiteCategoryGroup.AddUrl
			InlineAddUrl = PageUrl & "a=add"
			GridAddUrl = PageUrl & "a=gridadd"
			GridEditUrl = PageUrl & "a=gridedit"
			MultiDeleteUrl = "sitecategorygroup_delete.aspx"
			MultiUpdateUrl = "sitecategorygroup_update.aspx"

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
				SiteCategoryGroup.Export = ew_Get("export")
			ElseIf ew_Post("exporttype") <> "" Then
				SiteCategoryGroup.Export = ew_Post("exporttype")
			Else
				SiteCategoryGroup.ExportReturnUrl = ew_CurrentUrl()
			End If
			ParentPage.gsExport = SiteCategoryGroup.Export ' Get export parameter, used in header
			ParentPage.gsExportFile = SiteCategoryGroup.TableVar ' Get export file, used in header
			If SiteCategoryGroup.Export = "excel" Then
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
			SiteCategoryGroup.Dispose()
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
			Call SiteCategoryGroup.Recordset_SearchValidated()

			' Set Up Sorting Order
			SetUpSortOrder()

			' Get search criteria for advanced search
			If ParentPage.gsSearchError = "" Then
				sSrchAdvanced = AdvancedSearchWhere()
			End If
		End If

		' Restore display records
		If (SiteCategoryGroup.RecordsPerPage = -1 OrElse SiteCategoryGroup.RecordsPerPage > 0) Then
			lDisplayRecs = SiteCategoryGroup.RecordsPerPage ' Restore from Session
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
		SiteCategoryGroup.Recordset_Searching(sSrchWhere)

		' Save search criteria
		If sSrchWhere <> "" Then
			If sSrchAdvanced = "" Then ResetAdvancedSearchParms()
			SiteCategoryGroup.SearchWhere = sSrchWhere ' Save to Session
			If RestoreSearch Then
				lStartRec = 1 ' Reset start record counter
				SiteCategoryGroup.StartRecordNumber = lStartRec
			End If
		Else
			sSrchWhere = SiteCategoryGroup.SearchWhere
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
		SiteCategoryGroup.SessionWhere = sFilter
		SiteCategoryGroup.CurrentFilter = ""

		' Export Data only
		If SiteCategoryGroup.Export = "html" OrElse SiteCategoryGroup.Export = "csv" OrElse SiteCategoryGroup.Export = "word" OrElse SiteCategoryGroup.Export = "excel" OrElse SiteCategoryGroup.Export = "xml" Then
			ExportData()
			Page_Terminate("") ' Clean up
			ew_End() ' Terminate response
		ElseIf SiteCategoryGroup.Export = "email" Then
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
			SiteCategoryGroup.RecordsPerPage = lDisplayRecs ' Save to Session

			' Reset start position
			lStartRec = 1
			SiteCategoryGroup.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	' Return Advanced Search WHERE based on QueryString parameters
	'
	Function AdvancedSearchWhere() As String		
		Dim sWhere As String = ""
		BuildSearchSql(sWhere, SiteCategoryGroup.SiteCategoryGroupID, False) ' SiteCategoryGroupID
		BuildSearchSql(sWhere, SiteCategoryGroup.SiteCategoryGroupNM, False) ' SiteCategoryGroupNM
		BuildSearchSql(sWhere, SiteCategoryGroup.SiteCategoryGroupDS, False) ' SiteCategoryGroupDS
		BuildSearchSql(sWhere, SiteCategoryGroup.SiteCategoryGroupOrder, False) ' SiteCategoryGroupOrder

		' Set up search parm
		If sWhere <> "" Then
			SetSearchParm(SiteCategoryGroup.SiteCategoryGroupID) ' SiteCategoryGroupID
			SetSearchParm(SiteCategoryGroup.SiteCategoryGroupNM) ' SiteCategoryGroupNM
			SetSearchParm(SiteCategoryGroup.SiteCategoryGroupDS) ' SiteCategoryGroupDS
			SetSearchParm(SiteCategoryGroup.SiteCategoryGroupOrder) ' SiteCategoryGroupOrder
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
		SiteCategoryGroup.SetAdvancedSearch("x_" & FldParm, Fld.AdvancedSearch.SearchValue)
		SiteCategoryGroup.SetAdvancedSearch("z_" & FldParm, Fld.AdvancedSearch.SearchOperator)
		SiteCategoryGroup.SetAdvancedSearch("v_" & FldParm, Fld.AdvancedSearch.SearchCondition)
		SiteCategoryGroup.SetAdvancedSearch("y_" & FldParm, Fld.AdvancedSearch.SearchValue2)
		SiteCategoryGroup.SetAdvancedSearch("w_" & FldParm, Fld.AdvancedSearch.SearchOperator2)
	End Sub

	'
	' Get search parm
	'
	Sub GetSearchParm(ByRef Fld As Object)
		Dim FldParm As String = Fld.FldVar.Substring(2)
		Fld.AdvancedSearch.SearchValue = SiteCategoryGroup.GetAdvancedSearch("x_" & FldParm)
		Fld.AdvancedSearch.SearchOperator = SiteCategoryGroup.GetAdvancedSearch("z_" & FldParm)
		Fld.AdvancedSearch.SearchCondition = SiteCategoryGroup.GetAdvancedSearch("v_" & FldParm)
		Fld.AdvancedSearch.SearchValue2 = SiteCategoryGroup.GetAdvancedSearch("y_" & FldParm)
		Fld.AdvancedSearch.SearchOperator2 = SiteCategoryGroup.GetAdvancedSearch("w_" & FldParm)
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
		SiteCategoryGroup.SearchWhere = sSrchWhere

		' Clear advanced search parameters
		ResetAdvancedSearchParms()
	End Sub

	'
	' Clear all advanced search parameters
	'
	Sub ResetAdvancedSearchParms()
		SiteCategoryGroup.SetAdvancedSearch("x_SiteCategoryGroupID", "")
		SiteCategoryGroup.SetAdvancedSearch("x_SiteCategoryGroupNM", "")
		SiteCategoryGroup.SetAdvancedSearch("x_SiteCategoryGroupDS", "")
		SiteCategoryGroup.SetAdvancedSearch("x_SiteCategoryGroupOrder", "")
	End Sub

	'
	' Restore all search parameters
	'
	Sub RestoreSearchParms()
		Dim bRestore As Boolean = True
		If ew_Get("x_SiteCategoryGroupID") <> "" Then bRestore = False
		If ew_Get("x_SiteCategoryGroupNM") <> "" Then bRestore = False
		If ew_Get("x_SiteCategoryGroupDS") <> "" Then bRestore = False
		If ew_Get("x_SiteCategoryGroupOrder") <> "" Then bRestore = False
		RestoreSearch = bRestore
		If bRestore Then

			' Restore advanced search values
			Call GetSearchParm(SiteCategoryGroup.SiteCategoryGroupID)
			Call GetSearchParm(SiteCategoryGroup.SiteCategoryGroupNM)
			Call GetSearchParm(SiteCategoryGroup.SiteCategoryGroupDS)
			Call GetSearchParm(SiteCategoryGroup.SiteCategoryGroupOrder)
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
			SiteCategoryGroup.CurrentOrder = ew_Get("order")
			SiteCategoryGroup.CurrentOrderType = ew_Get("ordertype")
			SiteCategoryGroup.UpdateSort(SiteCategoryGroup.SiteCategoryGroupID) ' SiteCategoryGroupID
			SiteCategoryGroup.UpdateSort(SiteCategoryGroup.SiteCategoryGroupNM) ' SiteCategoryGroupNM
			SiteCategoryGroup.UpdateSort(SiteCategoryGroup.SiteCategoryGroupDS) ' SiteCategoryGroupDS
			SiteCategoryGroup.UpdateSort(SiteCategoryGroup.SiteCategoryGroupOrder) ' SiteCategoryGroupOrder
			SiteCategoryGroup.StartRecordNumber = 1 ' Reset start position
		End If
	End Sub

	'
	' Load Sort Order parameters
	'
	Sub LoadSortOrder()
		Dim sOrderBy As String
		sOrderBy = SiteCategoryGroup.SessionOrderBy ' Get order by from Session
		If sOrderBy = "" Then
			If SiteCategoryGroup.SqlOrderBy <> "" Then
				sOrderBy = SiteCategoryGroup.SqlOrderBy
				SiteCategoryGroup.SessionOrderBy = sOrderBy
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
				SiteCategoryGroup.SessionOrderBy = sOrderBy
				SiteCategoryGroup.SiteCategoryGroupID.Sort = ""
				SiteCategoryGroup.SiteCategoryGroupNM.Sort = ""
				SiteCategoryGroup.SiteCategoryGroupDS.Sort = ""
				SiteCategoryGroup.SiteCategoryGroupOrder.Sort = ""
			End If

			' Reset start position
			lStartRec = 1
			SiteCategoryGroup.StartRecordNumber = lStartRec
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
		If SiteCategoryGroup.Export <> "" Or SiteCategoryGroup.CurrentAction = "gridadd" Or SiteCategoryGroup.CurrentAction = "gridedit" Then
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
				SiteCategoryGroup.StartRecordNumber = lStartRec
			ElseIf ew_Get(EW_TABLE_PAGE_NO) <> "" AndAlso IsNumeric(ew_Get(EW_TABLE_PAGE_NO)) Then
				nPageNo = ew_ConvertToInt(ew_Get(EW_TABLE_PAGE_NO))
				lStartRec = (nPageNo-1)*lDisplayRecs+1
				If lStartRec <= 0 Then
					lStartRec = 1
				ElseIf lStartRec >= ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 Then
					lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1
				End If
				SiteCategoryGroup.StartRecordNumber = lStartRec
			End If
		End If
		lStartRec = SiteCategoryGroup.StartRecordNumber

		' Check if correct start record counter
		If lStartRec <= 0 Then ' Avoid invalid start record counter
			lStartRec = 1 ' Reset start record counter
			SiteCategoryGroup.StartRecordNumber = lStartRec
		ElseIf lStartRec > lTotalRecs Then ' Avoid starting record > total records
			lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to last page first record
			SiteCategoryGroup.StartRecordNumber = lStartRec
		ElseIf (lStartRec-1) Mod lDisplayRecs <> 0 Then
			lStartRec = ((lStartRec-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to page boundary
			SiteCategoryGroup.StartRecordNumber = lStartRec
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
		SiteCategoryGroup.SiteCategoryGroupID.AdvancedSearch.SearchValue = ew_Get("x_SiteCategoryGroupID")
    	SiteCategoryGroup.SiteCategoryGroupID.AdvancedSearch.SearchOperator = ew_Get("z_SiteCategoryGroupID")
		SiteCategoryGroup.SiteCategoryGroupNM.AdvancedSearch.SearchValue = ew_Get("x_SiteCategoryGroupNM")
    	SiteCategoryGroup.SiteCategoryGroupNM.AdvancedSearch.SearchOperator = ew_Get("z_SiteCategoryGroupNM")
		SiteCategoryGroup.SiteCategoryGroupDS.AdvancedSearch.SearchValue = ew_Get("x_SiteCategoryGroupDS")
    	SiteCategoryGroup.SiteCategoryGroupDS.AdvancedSearch.SearchOperator = ew_Get("z_SiteCategoryGroupDS")
		SiteCategoryGroup.SiteCategoryGroupOrder.AdvancedSearch.SearchValue = ew_Get("x_SiteCategoryGroupOrder")
    	SiteCategoryGroup.SiteCategoryGroupOrder.AdvancedSearch.SearchOperator = ew_Get("z_SiteCategoryGroupOrder")
	End Sub

	'
	' Load recordset
	'
	Function LoadRecordset() As OleDbDataReader

		' Recordset Selecting event
		SiteCategoryGroup.Recordset_Selecting(SiteCategoryGroup.CurrentFilter)

		' Load list page SQL
		Dim sSql As String = SiteCategoryGroup.ListSQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then DebugMsg = sSql

		' Count
		lTotalRecs = -1		
		Try
			Dim sCntSql As String = SiteCategoryGroup.SelectCountSQL

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
		SiteCategoryGroup.Recordset_Selected(Rs)
		Return Rs
	End Function

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = SiteCategoryGroup.KeyFilter

		' Row Selecting event
		SiteCategoryGroup.Row_Selecting(sFilter)

		' Load SQL based on filter
		SiteCategoryGroup.CurrentFilter = sFilter
		Dim sSql As String = SiteCategoryGroup.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then DebugMsg = sSql
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				SiteCategoryGroup.Row_Selected(RsRow)
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
		SiteCategoryGroup.SiteCategoryGroupID.DbValue = RsRow("SiteCategoryGroupID")
		SiteCategoryGroup.SiteCategoryGroupNM.DbValue = RsRow("SiteCategoryGroupNM")
		SiteCategoryGroup.SiteCategoryGroupDS.DbValue = RsRow("SiteCategoryGroupDS")
		SiteCategoryGroup.SiteCategoryGroupOrder.DbValue = RsRow("SiteCategoryGroupOrder")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Initialize urls
		ViewUrl = SiteCategoryGroup.ViewUrl
		EditUrl = SiteCategoryGroup.EditUrl
		InlineEditUrl = SiteCategoryGroup.InlineEditUrl
		CopyUrl = SiteCategoryGroup.CopyUrl
		InlineCopyUrl = SiteCategoryGroup.InlineCopyUrl
		DeleteUrl = SiteCategoryGroup.DeleteUrl

		' Row Rendering event
		SiteCategoryGroup.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' SiteCategoryGroupID

		SiteCategoryGroup.SiteCategoryGroupID.CellCssStyle = ""
		SiteCategoryGroup.SiteCategoryGroupID.CellCssClass = ""
		SiteCategoryGroup.SiteCategoryGroupID.CellAttrs.Clear(): SiteCategoryGroup.SiteCategoryGroupID.ViewAttrs.Clear(): SiteCategoryGroup.SiteCategoryGroupID.EditAttrs.Clear()

		' SiteCategoryGroupNM
		SiteCategoryGroup.SiteCategoryGroupNM.CellCssStyle = ""
		SiteCategoryGroup.SiteCategoryGroupNM.CellCssClass = ""
		SiteCategoryGroup.SiteCategoryGroupNM.CellAttrs.Clear(): SiteCategoryGroup.SiteCategoryGroupNM.ViewAttrs.Clear(): SiteCategoryGroup.SiteCategoryGroupNM.EditAttrs.Clear()

		' SiteCategoryGroupDS
		SiteCategoryGroup.SiteCategoryGroupDS.CellCssStyle = ""
		SiteCategoryGroup.SiteCategoryGroupDS.CellCssClass = ""
		SiteCategoryGroup.SiteCategoryGroupDS.CellAttrs.Clear(): SiteCategoryGroup.SiteCategoryGroupDS.ViewAttrs.Clear(): SiteCategoryGroup.SiteCategoryGroupDS.EditAttrs.Clear()

		' SiteCategoryGroupOrder
		SiteCategoryGroup.SiteCategoryGroupOrder.CellCssStyle = ""
		SiteCategoryGroup.SiteCategoryGroupOrder.CellCssClass = ""
		SiteCategoryGroup.SiteCategoryGroupOrder.CellAttrs.Clear(): SiteCategoryGroup.SiteCategoryGroupOrder.ViewAttrs.Clear(): SiteCategoryGroup.SiteCategoryGroupOrder.EditAttrs.Clear()

		'
		'  View  Row
		'

		If SiteCategoryGroup.RowType = EW_ROWTYPE_VIEW Then ' View row

			' SiteCategoryGroupID
			SiteCategoryGroup.SiteCategoryGroupID.ViewValue = SiteCategoryGroup.SiteCategoryGroupID.CurrentValue
			SiteCategoryGroup.SiteCategoryGroupID.CssStyle = ""
			SiteCategoryGroup.SiteCategoryGroupID.CssClass = ""
			SiteCategoryGroup.SiteCategoryGroupID.ViewCustomAttributes = ""

			' SiteCategoryGroupNM
			SiteCategoryGroup.SiteCategoryGroupNM.ViewValue = SiteCategoryGroup.SiteCategoryGroupNM.CurrentValue
			SiteCategoryGroup.SiteCategoryGroupNM.CssStyle = ""
			SiteCategoryGroup.SiteCategoryGroupNM.CssClass = ""
			SiteCategoryGroup.SiteCategoryGroupNM.ViewCustomAttributes = ""

			' SiteCategoryGroupDS
			SiteCategoryGroup.SiteCategoryGroupDS.ViewValue = SiteCategoryGroup.SiteCategoryGroupDS.CurrentValue
			SiteCategoryGroup.SiteCategoryGroupDS.CssStyle = ""
			SiteCategoryGroup.SiteCategoryGroupDS.CssClass = ""
			SiteCategoryGroup.SiteCategoryGroupDS.ViewCustomAttributes = ""

			' SiteCategoryGroupOrder
			SiteCategoryGroup.SiteCategoryGroupOrder.ViewValue = SiteCategoryGroup.SiteCategoryGroupOrder.CurrentValue
			SiteCategoryGroup.SiteCategoryGroupOrder.CssStyle = ""
			SiteCategoryGroup.SiteCategoryGroupOrder.CssClass = ""
			SiteCategoryGroup.SiteCategoryGroupOrder.ViewCustomAttributes = ""

			' View refer script
			' SiteCategoryGroupID

			SiteCategoryGroup.SiteCategoryGroupID.HrefValue = ""
			SiteCategoryGroup.SiteCategoryGroupID.TooltipValue = ""

			' SiteCategoryGroupNM
			SiteCategoryGroup.SiteCategoryGroupNM.HrefValue = ""
			SiteCategoryGroup.SiteCategoryGroupNM.TooltipValue = ""

			' SiteCategoryGroupDS
			SiteCategoryGroup.SiteCategoryGroupDS.HrefValue = ""
			SiteCategoryGroup.SiteCategoryGroupDS.TooltipValue = ""

			' SiteCategoryGroupOrder
			SiteCategoryGroup.SiteCategoryGroupOrder.HrefValue = ""
			SiteCategoryGroup.SiteCategoryGroupOrder.TooltipValue = ""
		End If

		' Row Rendered event
		If SiteCategoryGroup.RowType <> EW_ROWTYPE_AGGREGATEINIT Then
			SiteCategoryGroup.Row_Rendered()
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
		SiteCategoryGroup.SiteCategoryGroupID.AdvancedSearch.SearchValue = SiteCategoryGroup.GetAdvancedSearch("x_SiteCategoryGroupID")
		SiteCategoryGroup.SiteCategoryGroupNM.AdvancedSearch.SearchValue = SiteCategoryGroup.GetAdvancedSearch("x_SiteCategoryGroupNM")
		SiteCategoryGroup.SiteCategoryGroupDS.AdvancedSearch.SearchValue = SiteCategoryGroup.GetAdvancedSearch("x_SiteCategoryGroupDS")
		SiteCategoryGroup.SiteCategoryGroupOrder.AdvancedSearch.SearchValue = SiteCategoryGroup.GetAdvancedSearch("x_SiteCategoryGroupOrder")
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
		If SiteCategoryGroup.ExportAll Then
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
		If SiteCategoryGroup.Export = "xml" Then
			oXmlDoc = New cXMLDocument()
		Else
			sOutputStr = ew_ExportHeader(SiteCategoryGroup.Export)

			' Horizontal format, write header
			If sExportStyle <> "v" OrElse SiteCategoryGroup.Export = "csv" Then
				sExportStr = ""
				ew_ExportAddValue(sExportStr, SiteCategoryGroup.SiteCategoryGroupID.ExportCaption, SiteCategoryGroup.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategoryGroup.SiteCategoryGroupID.CellStyles, ""))
				ew_ExportAddValue(sExportStr, SiteCategoryGroup.SiteCategoryGroupNM.ExportCaption, SiteCategoryGroup.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategoryGroup.SiteCategoryGroupNM.CellStyles, ""))
				ew_ExportAddValue(sExportStr, SiteCategoryGroup.SiteCategoryGroupDS.ExportCaption, SiteCategoryGroup.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategoryGroup.SiteCategoryGroupDS.CellStyles, ""))
				ew_ExportAddValue(sExportStr, SiteCategoryGroup.SiteCategoryGroupOrder.ExportCaption, SiteCategoryGroup.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategoryGroup.SiteCategoryGroupOrder.CellStyles, ""))
				sOutputStr &= ew_ExportLine(sExportStr, SiteCategoryGroup.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategoryGroup.RowStyles, ""))
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
				SiteCategoryGroup.CssClass = ""
				SiteCategoryGroup.CssStyle = ""
				SiteCategoryGroup.RowType = EW_ROWTYPE_VIEW ' Render view
				RenderRow()
				If SiteCategoryGroup.Export = "xml" Then
					oXmlDoc.AddRow()
					oXmlDoc.AddField("SiteCategoryGroupID", SiteCategoryGroup.SiteCategoryGroupID.ExportValue(SiteCategoryGroup.Export, SiteCategoryGroup.ExportOriginalValue)) ' SiteCategoryGroupID
					oXmlDoc.AddField("SiteCategoryGroupNM", SiteCategoryGroup.SiteCategoryGroupNM.ExportValue(SiteCategoryGroup.Export, SiteCategoryGroup.ExportOriginalValue)) ' SiteCategoryGroupNM
					oXmlDoc.AddField("SiteCategoryGroupDS", SiteCategoryGroup.SiteCategoryGroupDS.ExportValue(SiteCategoryGroup.Export, SiteCategoryGroup.ExportOriginalValue)) ' SiteCategoryGroupDS
					oXmlDoc.AddField("SiteCategoryGroupOrder", SiteCategoryGroup.SiteCategoryGroupOrder.ExportValue(SiteCategoryGroup.Export, SiteCategoryGroup.ExportOriginalValue)) ' SiteCategoryGroupOrder
				Else

					' Vertical format
					If sExportStyle = "v" AndAlso SiteCategoryGroup.Export <> "csv" Then
						sOutputStr &= ew_ExportField(SiteCategoryGroup.SiteCategoryGroupID.ExportCaption, SiteCategoryGroup.SiteCategoryGroupID.ExportValue(SiteCategoryGroup.Export, SiteCategoryGroup.ExportOriginalValue), SiteCategoryGroup.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategoryGroup.SiteCategoryGroupID.CellStyles, "")) ' SiteCategoryGroupID
						sOutputStr &= ew_ExportField(SiteCategoryGroup.SiteCategoryGroupNM.ExportCaption, SiteCategoryGroup.SiteCategoryGroupNM.ExportValue(SiteCategoryGroup.Export, SiteCategoryGroup.ExportOriginalValue), SiteCategoryGroup.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategoryGroup.SiteCategoryGroupNM.CellStyles, "")) ' SiteCategoryGroupNM
						sOutputStr &= ew_ExportField(SiteCategoryGroup.SiteCategoryGroupDS.ExportCaption, SiteCategoryGroup.SiteCategoryGroupDS.ExportValue(SiteCategoryGroup.Export, SiteCategoryGroup.ExportOriginalValue), SiteCategoryGroup.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategoryGroup.SiteCategoryGroupDS.CellStyles, "")) ' SiteCategoryGroupDS
						sOutputStr &= ew_ExportField(SiteCategoryGroup.SiteCategoryGroupOrder.ExportCaption, SiteCategoryGroup.SiteCategoryGroupOrder.ExportValue(SiteCategoryGroup.Export, SiteCategoryGroup.ExportOriginalValue), SiteCategoryGroup.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategoryGroup.SiteCategoryGroupOrder.CellStyles, "")) ' SiteCategoryGroupOrder

					' Horizontal format
					Else
						sExportStr = ""
						ew_ExportAddValue(sExportStr, SiteCategoryGroup.SiteCategoryGroupID.ExportValue(SiteCategoryGroup.Export, SiteCategoryGroup.ExportOriginalValue), SiteCategoryGroup.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategoryGroup.SiteCategoryGroupID.CellStyles, ""))
						ew_ExportAddValue(sExportStr, SiteCategoryGroup.SiteCategoryGroupNM.ExportValue(SiteCategoryGroup.Export, SiteCategoryGroup.ExportOriginalValue), SiteCategoryGroup.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategoryGroup.SiteCategoryGroupNM.CellStyles, ""))
						ew_ExportAddValue(sExportStr, SiteCategoryGroup.SiteCategoryGroupDS.ExportValue(SiteCategoryGroup.Export, SiteCategoryGroup.ExportOriginalValue), SiteCategoryGroup.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategoryGroup.SiteCategoryGroupDS.CellStyles, ""))
						ew_ExportAddValue(sExportStr, SiteCategoryGroup.SiteCategoryGroupOrder.ExportValue(SiteCategoryGroup.Export, SiteCategoryGroup.ExportOriginalValue), SiteCategoryGroup.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategoryGroup.SiteCategoryGroupOrder.CellStyles, ""))
						sOutputStr &= ew_ExportLine(sExportStr, SiteCategoryGroup.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategoryGroup.RowStyles, ""))
					End If
				End If
			End If
		Loop

		' Close recordset
		Rs.Close()
		Rs.Dispose()
		If SiteCategoryGroup.Export = "xml" Then
			oXmlDoc.Output
		Else
			sOutputStr &= ew_ExportFooter(SiteCategoryGroup.Export)
			ew_Write(sOutputStr)
		End If
	End Sub

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "SiteCategoryGroup"
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
		SiteCategoryGroup_list = New cSiteCategoryGroup_list(Me)		
		SiteCategoryGroup_list.Page_Init()		

		' Set buffer/cache option
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

		' Page main processing
		SiteCategoryGroup_list.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If SiteCategoryGroup_list IsNot Nothing Then SiteCategoryGroup_list.Dispose()
	End Sub
End Class
