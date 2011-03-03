Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class sitecategorytype_list
	Inherits AspNetMaker8_wpmWebsite

	' Page object
	Public SiteCategoryType_list As cSiteCategoryType_list

	'
	' Page Class
	'
	Class cSiteCategoryType_list
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
				If SiteCategoryType.UseTokenInUrl Then Url = Url & "t=" & SiteCategoryType.TableVar & "&" ' Add page token
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
			If SiteCategoryType.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (SiteCategoryType.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (SiteCategoryType.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' ASP.NET page object
		Public ReadOnly Property AspNetPage() As sitecategorytype_list
			Get
				Return CType(m_ParentPage, sitecategorytype_list)
			End Get
		End Property

		' SiteCategoryType
		Public Property SiteCategoryType() As cSiteCategoryType
			Get				
				Return ParentPage.SiteCategoryType
			End Get
			Set(ByVal v As cSiteCategoryType)
				ParentPage.SiteCategoryType = v	
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
			m_PageObjName = "SiteCategoryType_list"
			m_PageObjTypeName = "cSiteCategoryType_list"			

			' Initialize language object
			Language = New cLanguage(Me)

			' Table Name
			m_TableName = "SiteCategoryType"

			' Initialize table object
			SiteCategoryType = New cSiteCategoryType(Me)

			' Initialize URLs
			ExportPrintUrl = PageUrl & "export=print"
			ExportExcelUrl = PageUrl & "export=excel"
			ExportWordUrl = PageUrl & "export=word"
			ExportHtmlUrl = PageUrl & "export=html"
			ExportXmlUrl = PageUrl & "export=xml"
			ExportCsvUrl = PageUrl & "export=csv"
			AddUrl = SiteCategoryType.AddUrl
			InlineAddUrl = PageUrl & "a=add"
			GridAddUrl = PageUrl & "a=gridadd"
			GridEditUrl = PageUrl & "a=gridedit"
			MultiDeleteUrl = "sitecategorytype_delete.aspx"
			MultiUpdateUrl = "sitecategorytype_update.aspx"

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
				SiteCategoryType.Export = ew_Get("export")
			ElseIf ew_Post("exporttype") <> "" Then
				SiteCategoryType.Export = ew_Post("exporttype")
			Else
				SiteCategoryType.ExportReturnUrl = ew_CurrentUrl()
			End If
			ParentPage.gsExport = SiteCategoryType.Export ' Get export parameter, used in header
			ParentPage.gsExportFile = SiteCategoryType.TableVar ' Get export file, used in header
			If SiteCategoryType.Export = "excel" Then
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
			SiteCategoryType.Dispose()
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
			Call SiteCategoryType.Recordset_SearchValidated()

			' Set Up Sorting Order
			SetUpSortOrder()

			' Get search criteria for advanced search
			If ParentPage.gsSearchError = "" Then
				sSrchAdvanced = AdvancedSearchWhere()
			End If
		End If

		' Restore display records
		If (SiteCategoryType.RecordsPerPage = -1 OrElse SiteCategoryType.RecordsPerPage > 0) Then
			lDisplayRecs = SiteCategoryType.RecordsPerPage ' Restore from Session
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
		SiteCategoryType.Recordset_Searching(sSrchWhere)

		' Save search criteria
		If sSrchWhere <> "" Then
			If sSrchAdvanced = "" Then ResetAdvancedSearchParms()
			SiteCategoryType.SearchWhere = sSrchWhere ' Save to Session
			If RestoreSearch Then
				lStartRec = 1 ' Reset start record counter
				SiteCategoryType.StartRecordNumber = lStartRec
			End If
		Else
			sSrchWhere = SiteCategoryType.SearchWhere
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
		SiteCategoryType.SessionWhere = sFilter
		SiteCategoryType.CurrentFilter = ""

		' Export Data only
		If SiteCategoryType.Export = "html" OrElse SiteCategoryType.Export = "csv" OrElse SiteCategoryType.Export = "word" OrElse SiteCategoryType.Export = "excel" OrElse SiteCategoryType.Export = "xml" Then
			ExportData()
			Page_Terminate("") ' Clean up
			ew_End() ' Terminate response
		ElseIf SiteCategoryType.Export = "email" Then
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
			SiteCategoryType.RecordsPerPage = lDisplayRecs ' Save to Session

			' Reset start position
			lStartRec = 1
			SiteCategoryType.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	' Return Advanced Search WHERE based on QueryString parameters
	'
	Function AdvancedSearchWhere() As String		
		Dim sWhere As String = ""
		BuildSearchSql(sWhere, SiteCategoryType.SiteCategoryTypeID, False) ' SiteCategoryTypeID
		BuildSearchSql(sWhere, SiteCategoryType.SiteCategoryTypeNM, False) ' SiteCategoryTypeNM
		BuildSearchSql(sWhere, SiteCategoryType.SiteCategoryTypeDS, False) ' SiteCategoryTypeDS
		BuildSearchSql(sWhere, SiteCategoryType.SiteCategoryComment, False) ' SiteCategoryComment
		BuildSearchSql(sWhere, SiteCategoryType.SiteCategoryFileName, False) ' SiteCategoryFileName
		BuildSearchSql(sWhere, SiteCategoryType.SiteCategoryTransferURL, False) ' SiteCategoryTransferURL
		BuildSearchSql(sWhere, SiteCategoryType.DefaultSiteCategoryID, False) ' DefaultSiteCategoryID

		' Set up search parm
		If sWhere <> "" Then
			SetSearchParm(SiteCategoryType.SiteCategoryTypeID) ' SiteCategoryTypeID
			SetSearchParm(SiteCategoryType.SiteCategoryTypeNM) ' SiteCategoryTypeNM
			SetSearchParm(SiteCategoryType.SiteCategoryTypeDS) ' SiteCategoryTypeDS
			SetSearchParm(SiteCategoryType.SiteCategoryComment) ' SiteCategoryComment
			SetSearchParm(SiteCategoryType.SiteCategoryFileName) ' SiteCategoryFileName
			SetSearchParm(SiteCategoryType.SiteCategoryTransferURL) ' SiteCategoryTransferURL
			SetSearchParm(SiteCategoryType.DefaultSiteCategoryID) ' DefaultSiteCategoryID
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
		SiteCategoryType.SetAdvancedSearch("x_" & FldParm, Fld.AdvancedSearch.SearchValue)
		SiteCategoryType.SetAdvancedSearch("z_" & FldParm, Fld.AdvancedSearch.SearchOperator)
		SiteCategoryType.SetAdvancedSearch("v_" & FldParm, Fld.AdvancedSearch.SearchCondition)
		SiteCategoryType.SetAdvancedSearch("y_" & FldParm, Fld.AdvancedSearch.SearchValue2)
		SiteCategoryType.SetAdvancedSearch("w_" & FldParm, Fld.AdvancedSearch.SearchOperator2)
	End Sub

	'
	' Get search parm
	'
	Sub GetSearchParm(ByRef Fld As Object)
		Dim FldParm As String = Fld.FldVar.Substring(2)
		Fld.AdvancedSearch.SearchValue = SiteCategoryType.GetAdvancedSearch("x_" & FldParm)
		Fld.AdvancedSearch.SearchOperator = SiteCategoryType.GetAdvancedSearch("z_" & FldParm)
		Fld.AdvancedSearch.SearchCondition = SiteCategoryType.GetAdvancedSearch("v_" & FldParm)
		Fld.AdvancedSearch.SearchValue2 = SiteCategoryType.GetAdvancedSearch("y_" & FldParm)
		Fld.AdvancedSearch.SearchOperator2 = SiteCategoryType.GetAdvancedSearch("w_" & FldParm)
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
		SiteCategoryType.SearchWhere = sSrchWhere

		' Clear advanced search parameters
		ResetAdvancedSearchParms()
	End Sub

	'
	' Clear all advanced search parameters
	'
	Sub ResetAdvancedSearchParms()
		SiteCategoryType.SetAdvancedSearch("x_SiteCategoryTypeID", "")
		SiteCategoryType.SetAdvancedSearch("x_SiteCategoryTypeNM", "")
		SiteCategoryType.SetAdvancedSearch("x_SiteCategoryTypeDS", "")
		SiteCategoryType.SetAdvancedSearch("x_SiteCategoryComment", "")
		SiteCategoryType.SetAdvancedSearch("x_SiteCategoryFileName", "")
		SiteCategoryType.SetAdvancedSearch("x_SiteCategoryTransferURL", "")
		SiteCategoryType.SetAdvancedSearch("x_DefaultSiteCategoryID", "")
	End Sub

	'
	' Restore all search parameters
	'
	Sub RestoreSearchParms()
		Dim bRestore As Boolean = True
		If ew_Get("x_SiteCategoryTypeID") <> "" Then bRestore = False
		If ew_Get("x_SiteCategoryTypeNM") <> "" Then bRestore = False
		If ew_Get("x_SiteCategoryTypeDS") <> "" Then bRestore = False
		If ew_Get("x_SiteCategoryComment") <> "" Then bRestore = False
		If ew_Get("x_SiteCategoryFileName") <> "" Then bRestore = False
		If ew_Get("x_SiteCategoryTransferURL") <> "" Then bRestore = False
		If ew_Get("x_DefaultSiteCategoryID") <> "" Then bRestore = False
		RestoreSearch = bRestore
		If bRestore Then

			' Restore advanced search values
			Call GetSearchParm(SiteCategoryType.SiteCategoryTypeID)
			Call GetSearchParm(SiteCategoryType.SiteCategoryTypeNM)
			Call GetSearchParm(SiteCategoryType.SiteCategoryTypeDS)
			Call GetSearchParm(SiteCategoryType.SiteCategoryComment)
			Call GetSearchParm(SiteCategoryType.SiteCategoryFileName)
			Call GetSearchParm(SiteCategoryType.SiteCategoryTransferURL)
			Call GetSearchParm(SiteCategoryType.DefaultSiteCategoryID)
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
			SiteCategoryType.CurrentOrder = ew_Get("order")
			SiteCategoryType.CurrentOrderType = ew_Get("ordertype")
			SiteCategoryType.UpdateSort(SiteCategoryType.SiteCategoryTypeID) ' SiteCategoryTypeID
			SiteCategoryType.UpdateSort(SiteCategoryType.SiteCategoryTypeNM) ' SiteCategoryTypeNM
			SiteCategoryType.UpdateSort(SiteCategoryType.SiteCategoryTypeDS) ' SiteCategoryTypeDS
			SiteCategoryType.UpdateSort(SiteCategoryType.SiteCategoryComment) ' SiteCategoryComment
			SiteCategoryType.UpdateSort(SiteCategoryType.SiteCategoryFileName) ' SiteCategoryFileName
			SiteCategoryType.UpdateSort(SiteCategoryType.SiteCategoryTransferURL) ' SiteCategoryTransferURL
			SiteCategoryType.UpdateSort(SiteCategoryType.DefaultSiteCategoryID) ' DefaultSiteCategoryID
			SiteCategoryType.StartRecordNumber = 1 ' Reset start position
		End If
	End Sub

	'
	' Load Sort Order parameters
	'
	Sub LoadSortOrder()
		Dim sOrderBy As String
		sOrderBy = SiteCategoryType.SessionOrderBy ' Get order by from Session
		If sOrderBy = "" Then
			If SiteCategoryType.SqlOrderBy <> "" Then
				sOrderBy = SiteCategoryType.SqlOrderBy
				SiteCategoryType.SessionOrderBy = sOrderBy
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
				SiteCategoryType.SessionOrderBy = sOrderBy
				SiteCategoryType.SiteCategoryTypeID.Sort = ""
				SiteCategoryType.SiteCategoryTypeNM.Sort = ""
				SiteCategoryType.SiteCategoryTypeDS.Sort = ""
				SiteCategoryType.SiteCategoryComment.Sort = ""
				SiteCategoryType.SiteCategoryFileName.Sort = ""
				SiteCategoryType.SiteCategoryTransferURL.Sort = ""
				SiteCategoryType.DefaultSiteCategoryID.Sort = ""
			End If

			' Reset start position
			lStartRec = 1
			SiteCategoryType.StartRecordNumber = lStartRec
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
		If SiteCategoryType.Export <> "" Or SiteCategoryType.CurrentAction = "gridadd" Or SiteCategoryType.CurrentAction = "gridedit" Then
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
				SiteCategoryType.StartRecordNumber = lStartRec
			ElseIf ew_Get(EW_TABLE_PAGE_NO) <> "" AndAlso IsNumeric(ew_Get(EW_TABLE_PAGE_NO)) Then
				nPageNo = ew_ConvertToInt(ew_Get(EW_TABLE_PAGE_NO))
				lStartRec = (nPageNo-1)*lDisplayRecs+1
				If lStartRec <= 0 Then
					lStartRec = 1
				ElseIf lStartRec >= ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 Then
					lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1
				End If
				SiteCategoryType.StartRecordNumber = lStartRec
			End If
		End If
		lStartRec = SiteCategoryType.StartRecordNumber

		' Check if correct start record counter
		If lStartRec <= 0 Then ' Avoid invalid start record counter
			lStartRec = 1 ' Reset start record counter
			SiteCategoryType.StartRecordNumber = lStartRec
		ElseIf lStartRec > lTotalRecs Then ' Avoid starting record > total records
			lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to last page first record
			SiteCategoryType.StartRecordNumber = lStartRec
		ElseIf (lStartRec-1) Mod lDisplayRecs <> 0 Then
			lStartRec = ((lStartRec-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to page boundary
			SiteCategoryType.StartRecordNumber = lStartRec
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
		SiteCategoryType.SiteCategoryTypeID.AdvancedSearch.SearchValue = ew_Get("x_SiteCategoryTypeID")
    	SiteCategoryType.SiteCategoryTypeID.AdvancedSearch.SearchOperator = ew_Get("z_SiteCategoryTypeID")
		SiteCategoryType.SiteCategoryTypeNM.AdvancedSearch.SearchValue = ew_Get("x_SiteCategoryTypeNM")
    	SiteCategoryType.SiteCategoryTypeNM.AdvancedSearch.SearchOperator = ew_Get("z_SiteCategoryTypeNM")
		SiteCategoryType.SiteCategoryTypeDS.AdvancedSearch.SearchValue = ew_Get("x_SiteCategoryTypeDS")
    	SiteCategoryType.SiteCategoryTypeDS.AdvancedSearch.SearchOperator = ew_Get("z_SiteCategoryTypeDS")
		SiteCategoryType.SiteCategoryComment.AdvancedSearch.SearchValue = ew_Get("x_SiteCategoryComment")
    	SiteCategoryType.SiteCategoryComment.AdvancedSearch.SearchOperator = ew_Get("z_SiteCategoryComment")
		SiteCategoryType.SiteCategoryFileName.AdvancedSearch.SearchValue = ew_Get("x_SiteCategoryFileName")
    	SiteCategoryType.SiteCategoryFileName.AdvancedSearch.SearchOperator = ew_Get("z_SiteCategoryFileName")
		SiteCategoryType.SiteCategoryTransferURL.AdvancedSearch.SearchValue = ew_Get("x_SiteCategoryTransferURL")
    	SiteCategoryType.SiteCategoryTransferURL.AdvancedSearch.SearchOperator = ew_Get("z_SiteCategoryTransferURL")
		SiteCategoryType.DefaultSiteCategoryID.AdvancedSearch.SearchValue = ew_Get("x_DefaultSiteCategoryID")
    	SiteCategoryType.DefaultSiteCategoryID.AdvancedSearch.SearchOperator = ew_Get("z_DefaultSiteCategoryID")
	End Sub

	'
	' Load recordset
	'
	Function LoadRecordset() As OleDbDataReader

		' Recordset Selecting event
		SiteCategoryType.Recordset_Selecting(SiteCategoryType.CurrentFilter)

		' Load list page SQL
		Dim sSql As String = SiteCategoryType.ListSQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then DebugMsg = sSql

		' Count
		lTotalRecs = -1		
		Try
			Dim sCntSql As String = SiteCategoryType.SelectCountSQL

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
		SiteCategoryType.Recordset_Selected(Rs)
		Return Rs
	End Function

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = SiteCategoryType.KeyFilter

		' Row Selecting event
		SiteCategoryType.Row_Selecting(sFilter)

		' Load SQL based on filter
		SiteCategoryType.CurrentFilter = sFilter
		Dim sSql As String = SiteCategoryType.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then DebugMsg = sSql
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				SiteCategoryType.Row_Selected(RsRow)
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
		SiteCategoryType.SiteCategoryTypeID.DbValue = RsRow("SiteCategoryTypeID")
		SiteCategoryType.SiteCategoryTypeNM.DbValue = RsRow("SiteCategoryTypeNM")
		SiteCategoryType.SiteCategoryTypeDS.DbValue = RsRow("SiteCategoryTypeDS")
		SiteCategoryType.SiteCategoryComment.DbValue = RsRow("SiteCategoryComment")
		SiteCategoryType.SiteCategoryFileName.DbValue = RsRow("SiteCategoryFileName")
		SiteCategoryType.SiteCategoryTransferURL.DbValue = RsRow("SiteCategoryTransferURL")
		SiteCategoryType.DefaultSiteCategoryID.DbValue = RsRow("DefaultSiteCategoryID")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Initialize urls
		ViewUrl = SiteCategoryType.ViewUrl
		EditUrl = SiteCategoryType.EditUrl
		InlineEditUrl = SiteCategoryType.InlineEditUrl
		CopyUrl = SiteCategoryType.CopyUrl
		InlineCopyUrl = SiteCategoryType.InlineCopyUrl
		DeleteUrl = SiteCategoryType.DeleteUrl

		' Row Rendering event
		SiteCategoryType.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' SiteCategoryTypeID

		SiteCategoryType.SiteCategoryTypeID.CellCssStyle = ""
		SiteCategoryType.SiteCategoryTypeID.CellCssClass = ""
		SiteCategoryType.SiteCategoryTypeID.CellAttrs.Clear(): SiteCategoryType.SiteCategoryTypeID.ViewAttrs.Clear(): SiteCategoryType.SiteCategoryTypeID.EditAttrs.Clear()

		' SiteCategoryTypeNM
		SiteCategoryType.SiteCategoryTypeNM.CellCssStyle = ""
		SiteCategoryType.SiteCategoryTypeNM.CellCssClass = ""
		SiteCategoryType.SiteCategoryTypeNM.CellAttrs.Clear(): SiteCategoryType.SiteCategoryTypeNM.ViewAttrs.Clear(): SiteCategoryType.SiteCategoryTypeNM.EditAttrs.Clear()

		' SiteCategoryTypeDS
		SiteCategoryType.SiteCategoryTypeDS.CellCssStyle = ""
		SiteCategoryType.SiteCategoryTypeDS.CellCssClass = ""
		SiteCategoryType.SiteCategoryTypeDS.CellAttrs.Clear(): SiteCategoryType.SiteCategoryTypeDS.ViewAttrs.Clear(): SiteCategoryType.SiteCategoryTypeDS.EditAttrs.Clear()

		' SiteCategoryComment
		SiteCategoryType.SiteCategoryComment.CellCssStyle = ""
		SiteCategoryType.SiteCategoryComment.CellCssClass = ""
		SiteCategoryType.SiteCategoryComment.CellAttrs.Clear(): SiteCategoryType.SiteCategoryComment.ViewAttrs.Clear(): SiteCategoryType.SiteCategoryComment.EditAttrs.Clear()

		' SiteCategoryFileName
		SiteCategoryType.SiteCategoryFileName.CellCssStyle = ""
		SiteCategoryType.SiteCategoryFileName.CellCssClass = ""
		SiteCategoryType.SiteCategoryFileName.CellAttrs.Clear(): SiteCategoryType.SiteCategoryFileName.ViewAttrs.Clear(): SiteCategoryType.SiteCategoryFileName.EditAttrs.Clear()

		' SiteCategoryTransferURL
		SiteCategoryType.SiteCategoryTransferURL.CellCssStyle = ""
		SiteCategoryType.SiteCategoryTransferURL.CellCssClass = ""
		SiteCategoryType.SiteCategoryTransferURL.CellAttrs.Clear(): SiteCategoryType.SiteCategoryTransferURL.ViewAttrs.Clear(): SiteCategoryType.SiteCategoryTransferURL.EditAttrs.Clear()

		' DefaultSiteCategoryID
		SiteCategoryType.DefaultSiteCategoryID.CellCssStyle = ""
		SiteCategoryType.DefaultSiteCategoryID.CellCssClass = ""
		SiteCategoryType.DefaultSiteCategoryID.CellAttrs.Clear(): SiteCategoryType.DefaultSiteCategoryID.ViewAttrs.Clear(): SiteCategoryType.DefaultSiteCategoryID.EditAttrs.Clear()

		'
		'  View  Row
		'

		If SiteCategoryType.RowType = EW_ROWTYPE_VIEW Then ' View row

			' SiteCategoryTypeID
			SiteCategoryType.SiteCategoryTypeID.ViewValue = SiteCategoryType.SiteCategoryTypeID.CurrentValue
			SiteCategoryType.SiteCategoryTypeID.CssStyle = ""
			SiteCategoryType.SiteCategoryTypeID.CssClass = ""
			SiteCategoryType.SiteCategoryTypeID.ViewCustomAttributes = ""

			' SiteCategoryTypeNM
			SiteCategoryType.SiteCategoryTypeNM.ViewValue = SiteCategoryType.SiteCategoryTypeNM.CurrentValue
			SiteCategoryType.SiteCategoryTypeNM.CssStyle = ""
			SiteCategoryType.SiteCategoryTypeNM.CssClass = ""
			SiteCategoryType.SiteCategoryTypeNM.ViewCustomAttributes = ""

			' SiteCategoryTypeDS
			SiteCategoryType.SiteCategoryTypeDS.ViewValue = SiteCategoryType.SiteCategoryTypeDS.CurrentValue
			SiteCategoryType.SiteCategoryTypeDS.CssStyle = ""
			SiteCategoryType.SiteCategoryTypeDS.CssClass = ""
			SiteCategoryType.SiteCategoryTypeDS.ViewCustomAttributes = ""

			' SiteCategoryComment
			SiteCategoryType.SiteCategoryComment.ViewValue = SiteCategoryType.SiteCategoryComment.CurrentValue
			SiteCategoryType.SiteCategoryComment.CssStyle = ""
			SiteCategoryType.SiteCategoryComment.CssClass = ""
			SiteCategoryType.SiteCategoryComment.ViewCustomAttributes = ""

			' SiteCategoryFileName
			SiteCategoryType.SiteCategoryFileName.ViewValue = SiteCategoryType.SiteCategoryFileName.CurrentValue
			SiteCategoryType.SiteCategoryFileName.CssStyle = ""
			SiteCategoryType.SiteCategoryFileName.CssClass = ""
			SiteCategoryType.SiteCategoryFileName.ViewCustomAttributes = ""

			' SiteCategoryTransferURL
			SiteCategoryType.SiteCategoryTransferURL.ViewValue = SiteCategoryType.SiteCategoryTransferURL.CurrentValue
			SiteCategoryType.SiteCategoryTransferURL.CssStyle = ""
			SiteCategoryType.SiteCategoryTransferURL.CssClass = ""
			SiteCategoryType.SiteCategoryTransferURL.ViewCustomAttributes = ""

			' DefaultSiteCategoryID
			SiteCategoryType.DefaultSiteCategoryID.ViewValue = SiteCategoryType.DefaultSiteCategoryID.CurrentValue
			SiteCategoryType.DefaultSiteCategoryID.CssStyle = ""
			SiteCategoryType.DefaultSiteCategoryID.CssClass = ""
			SiteCategoryType.DefaultSiteCategoryID.ViewCustomAttributes = ""

			' View refer script
			' SiteCategoryTypeID

			SiteCategoryType.SiteCategoryTypeID.HrefValue = ""
			SiteCategoryType.SiteCategoryTypeID.TooltipValue = ""

			' SiteCategoryTypeNM
			SiteCategoryType.SiteCategoryTypeNM.HrefValue = ""
			SiteCategoryType.SiteCategoryTypeNM.TooltipValue = ""

			' SiteCategoryTypeDS
			SiteCategoryType.SiteCategoryTypeDS.HrefValue = ""
			SiteCategoryType.SiteCategoryTypeDS.TooltipValue = ""

			' SiteCategoryComment
			SiteCategoryType.SiteCategoryComment.HrefValue = ""
			SiteCategoryType.SiteCategoryComment.TooltipValue = ""

			' SiteCategoryFileName
			SiteCategoryType.SiteCategoryFileName.HrefValue = ""
			SiteCategoryType.SiteCategoryFileName.TooltipValue = ""

			' SiteCategoryTransferURL
			SiteCategoryType.SiteCategoryTransferURL.HrefValue = ""
			SiteCategoryType.SiteCategoryTransferURL.TooltipValue = ""

			' DefaultSiteCategoryID
			SiteCategoryType.DefaultSiteCategoryID.HrefValue = ""
			SiteCategoryType.DefaultSiteCategoryID.TooltipValue = ""
		End If

		' Row Rendered event
		If SiteCategoryType.RowType <> EW_ROWTYPE_AGGREGATEINIT Then
			SiteCategoryType.Row_Rendered()
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
		SiteCategoryType.SiteCategoryTypeID.AdvancedSearch.SearchValue = SiteCategoryType.GetAdvancedSearch("x_SiteCategoryTypeID")
		SiteCategoryType.SiteCategoryTypeNM.AdvancedSearch.SearchValue = SiteCategoryType.GetAdvancedSearch("x_SiteCategoryTypeNM")
		SiteCategoryType.SiteCategoryTypeDS.AdvancedSearch.SearchValue = SiteCategoryType.GetAdvancedSearch("x_SiteCategoryTypeDS")
		SiteCategoryType.SiteCategoryComment.AdvancedSearch.SearchValue = SiteCategoryType.GetAdvancedSearch("x_SiteCategoryComment")
		SiteCategoryType.SiteCategoryFileName.AdvancedSearch.SearchValue = SiteCategoryType.GetAdvancedSearch("x_SiteCategoryFileName")
		SiteCategoryType.SiteCategoryTransferURL.AdvancedSearch.SearchValue = SiteCategoryType.GetAdvancedSearch("x_SiteCategoryTransferURL")
		SiteCategoryType.DefaultSiteCategoryID.AdvancedSearch.SearchValue = SiteCategoryType.GetAdvancedSearch("x_DefaultSiteCategoryID")
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
		If SiteCategoryType.ExportAll Then
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
		If SiteCategoryType.Export = "xml" Then
			oXmlDoc = New cXMLDocument()
		Else
			sOutputStr = ew_ExportHeader(SiteCategoryType.Export)

			' Horizontal format, write header
			If sExportStyle <> "v" OrElse SiteCategoryType.Export = "csv" Then
				sExportStr = ""
				ew_ExportAddValue(sExportStr, SiteCategoryType.SiteCategoryTypeID.ExportCaption, SiteCategoryType.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategoryType.SiteCategoryTypeID.CellStyles, ""))
				ew_ExportAddValue(sExportStr, SiteCategoryType.SiteCategoryTypeNM.ExportCaption, SiteCategoryType.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategoryType.SiteCategoryTypeNM.CellStyles, ""))
				ew_ExportAddValue(sExportStr, SiteCategoryType.SiteCategoryTypeDS.ExportCaption, SiteCategoryType.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategoryType.SiteCategoryTypeDS.CellStyles, ""))
				ew_ExportAddValue(sExportStr, SiteCategoryType.SiteCategoryComment.ExportCaption, SiteCategoryType.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategoryType.SiteCategoryComment.CellStyles, ""))
				ew_ExportAddValue(sExportStr, SiteCategoryType.SiteCategoryFileName.ExportCaption, SiteCategoryType.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategoryType.SiteCategoryFileName.CellStyles, ""))
				ew_ExportAddValue(sExportStr, SiteCategoryType.SiteCategoryTransferURL.ExportCaption, SiteCategoryType.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategoryType.SiteCategoryTransferURL.CellStyles, ""))
				ew_ExportAddValue(sExportStr, SiteCategoryType.DefaultSiteCategoryID.ExportCaption, SiteCategoryType.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategoryType.DefaultSiteCategoryID.CellStyles, ""))
				sOutputStr &= ew_ExportLine(sExportStr, SiteCategoryType.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategoryType.RowStyles, ""))
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
				SiteCategoryType.CssClass = ""
				SiteCategoryType.CssStyle = ""
				SiteCategoryType.RowType = EW_ROWTYPE_VIEW ' Render view
				RenderRow()
				If SiteCategoryType.Export = "xml" Then
					oXmlDoc.AddRow()
					oXmlDoc.AddField("SiteCategoryTypeID", SiteCategoryType.SiteCategoryTypeID.ExportValue(SiteCategoryType.Export, SiteCategoryType.ExportOriginalValue)) ' SiteCategoryTypeID
					oXmlDoc.AddField("SiteCategoryTypeNM", SiteCategoryType.SiteCategoryTypeNM.ExportValue(SiteCategoryType.Export, SiteCategoryType.ExportOriginalValue)) ' SiteCategoryTypeNM
					oXmlDoc.AddField("SiteCategoryTypeDS", SiteCategoryType.SiteCategoryTypeDS.ExportValue(SiteCategoryType.Export, SiteCategoryType.ExportOriginalValue)) ' SiteCategoryTypeDS
					oXmlDoc.AddField("SiteCategoryComment", SiteCategoryType.SiteCategoryComment.ExportValue(SiteCategoryType.Export, SiteCategoryType.ExportOriginalValue)) ' SiteCategoryComment
					oXmlDoc.AddField("SiteCategoryFileName", SiteCategoryType.SiteCategoryFileName.ExportValue(SiteCategoryType.Export, SiteCategoryType.ExportOriginalValue)) ' SiteCategoryFileName
					oXmlDoc.AddField("SiteCategoryTransferURL", SiteCategoryType.SiteCategoryTransferURL.ExportValue(SiteCategoryType.Export, SiteCategoryType.ExportOriginalValue)) ' SiteCategoryTransferURL
					oXmlDoc.AddField("DefaultSiteCategoryID", SiteCategoryType.DefaultSiteCategoryID.ExportValue(SiteCategoryType.Export, SiteCategoryType.ExportOriginalValue)) ' DefaultSiteCategoryID
				Else

					' Vertical format
					If sExportStyle = "v" AndAlso SiteCategoryType.Export <> "csv" Then
						sOutputStr &= ew_ExportField(SiteCategoryType.SiteCategoryTypeID.ExportCaption, SiteCategoryType.SiteCategoryTypeID.ExportValue(SiteCategoryType.Export, SiteCategoryType.ExportOriginalValue), SiteCategoryType.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategoryType.SiteCategoryTypeID.CellStyles, "")) ' SiteCategoryTypeID
						sOutputStr &= ew_ExportField(SiteCategoryType.SiteCategoryTypeNM.ExportCaption, SiteCategoryType.SiteCategoryTypeNM.ExportValue(SiteCategoryType.Export, SiteCategoryType.ExportOriginalValue), SiteCategoryType.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategoryType.SiteCategoryTypeNM.CellStyles, "")) ' SiteCategoryTypeNM
						sOutputStr &= ew_ExportField(SiteCategoryType.SiteCategoryTypeDS.ExportCaption, SiteCategoryType.SiteCategoryTypeDS.ExportValue(SiteCategoryType.Export, SiteCategoryType.ExportOriginalValue), SiteCategoryType.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategoryType.SiteCategoryTypeDS.CellStyles, "")) ' SiteCategoryTypeDS
						sOutputStr &= ew_ExportField(SiteCategoryType.SiteCategoryComment.ExportCaption, SiteCategoryType.SiteCategoryComment.ExportValue(SiteCategoryType.Export, SiteCategoryType.ExportOriginalValue), SiteCategoryType.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategoryType.SiteCategoryComment.CellStyles, "")) ' SiteCategoryComment
						sOutputStr &= ew_ExportField(SiteCategoryType.SiteCategoryFileName.ExportCaption, SiteCategoryType.SiteCategoryFileName.ExportValue(SiteCategoryType.Export, SiteCategoryType.ExportOriginalValue), SiteCategoryType.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategoryType.SiteCategoryFileName.CellStyles, "")) ' SiteCategoryFileName
						sOutputStr &= ew_ExportField(SiteCategoryType.SiteCategoryTransferURL.ExportCaption, SiteCategoryType.SiteCategoryTransferURL.ExportValue(SiteCategoryType.Export, SiteCategoryType.ExportOriginalValue), SiteCategoryType.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategoryType.SiteCategoryTransferURL.CellStyles, "")) ' SiteCategoryTransferURL
						sOutputStr &= ew_ExportField(SiteCategoryType.DefaultSiteCategoryID.ExportCaption, SiteCategoryType.DefaultSiteCategoryID.ExportValue(SiteCategoryType.Export, SiteCategoryType.ExportOriginalValue), SiteCategoryType.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategoryType.DefaultSiteCategoryID.CellStyles, "")) ' DefaultSiteCategoryID

					' Horizontal format
					Else
						sExportStr = ""
						ew_ExportAddValue(sExportStr, SiteCategoryType.SiteCategoryTypeID.ExportValue(SiteCategoryType.Export, SiteCategoryType.ExportOriginalValue), SiteCategoryType.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategoryType.SiteCategoryTypeID.CellStyles, ""))
						ew_ExportAddValue(sExportStr, SiteCategoryType.SiteCategoryTypeNM.ExportValue(SiteCategoryType.Export, SiteCategoryType.ExportOriginalValue), SiteCategoryType.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategoryType.SiteCategoryTypeNM.CellStyles, ""))
						ew_ExportAddValue(sExportStr, SiteCategoryType.SiteCategoryTypeDS.ExportValue(SiteCategoryType.Export, SiteCategoryType.ExportOriginalValue), SiteCategoryType.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategoryType.SiteCategoryTypeDS.CellStyles, ""))
						ew_ExportAddValue(sExportStr, SiteCategoryType.SiteCategoryComment.ExportValue(SiteCategoryType.Export, SiteCategoryType.ExportOriginalValue), SiteCategoryType.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategoryType.SiteCategoryComment.CellStyles, ""))
						ew_ExportAddValue(sExportStr, SiteCategoryType.SiteCategoryFileName.ExportValue(SiteCategoryType.Export, SiteCategoryType.ExportOriginalValue), SiteCategoryType.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategoryType.SiteCategoryFileName.CellStyles, ""))
						ew_ExportAddValue(sExportStr, SiteCategoryType.SiteCategoryTransferURL.ExportValue(SiteCategoryType.Export, SiteCategoryType.ExportOriginalValue), SiteCategoryType.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategoryType.SiteCategoryTransferURL.CellStyles, ""))
						ew_ExportAddValue(sExportStr, SiteCategoryType.DefaultSiteCategoryID.ExportValue(SiteCategoryType.Export, SiteCategoryType.ExportOriginalValue), SiteCategoryType.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategoryType.DefaultSiteCategoryID.CellStyles, ""))
						sOutputStr &= ew_ExportLine(sExportStr, SiteCategoryType.Export, IIf(EW_EXPORT_CSS_STYLES, SiteCategoryType.RowStyles, ""))
					End If
				End If
			End If
		Loop

		' Close recordset
		Rs.Close()
		Rs.Dispose()
		If SiteCategoryType.Export = "xml" Then
			oXmlDoc.Output
		Else
			sOutputStr &= ew_ExportFooter(SiteCategoryType.Export)
			ew_Write(sOutputStr)
		End If
	End Sub

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "SiteCategoryType"
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
		SiteCategoryType_list = New cSiteCategoryType_list(Me)		
		SiteCategoryType_list.Page_Init()		

		' Set buffer/cache option
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

		' Page main processing
		SiteCategoryType_list.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If SiteCategoryType_list IsNot Nothing Then SiteCategoryType_list.Dispose()
	End Sub
End Class
