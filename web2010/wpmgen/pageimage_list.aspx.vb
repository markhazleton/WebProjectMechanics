Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class pageimage_list
	Inherits AspNetMaker8_wpmWebsite

	' Page object
	Public PageImage_list As cPageImage_list

	'
	' Page Class
	'
	Class cPageImage_list
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
				If PageImage.UseTokenInUrl Then Url = Url & "t=" & PageImage.TableVar & "&" ' Add page token
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
			If PageImage.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (PageImage.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (PageImage.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' ASP.NET page object
		Public ReadOnly Property AspNetPage() As pageimage_list
			Get
				Return CType(m_ParentPage, pageimage_list)
			End Get
		End Property

		' PageImage
		Public Property PageImage() As cPageImage
			Get				
				Return ParentPage.PageImage
			End Get
			Set(ByVal v As cPageImage)
				ParentPage.PageImage = v	
			End Set	
		End Property

		' PageImage
		Public Property Image() As cImage
			Get				
				Return ParentPage.Image
			End Get
			Set(ByVal v As cImage)
				ParentPage.Image = v	
			End Set	
		End Property

		' PageImage
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
			m_PageObjName = "PageImage_list"
			m_PageObjTypeName = "cPageImage_list"			

			' Initialize language object
			Language = New cLanguage(Me)

			' Table Name
			m_TableName = "PageImage"

			' Initialize table object
			PageImage = New cPageImage(Me)
			Image = New cImage(Me)
			zPage = New czPage(Me)

			' Initialize URLs
			ExportPrintUrl = PageUrl & "export=print"
			ExportExcelUrl = PageUrl & "export=excel"
			ExportWordUrl = PageUrl & "export=word"
			ExportHtmlUrl = PageUrl & "export=html"
			ExportXmlUrl = PageUrl & "export=xml"
			ExportCsvUrl = PageUrl & "export=csv"
			AddUrl = PageImage.AddUrl
			InlineAddUrl = PageUrl & "a=add"
			GridAddUrl = PageUrl & "a=gridadd"
			GridEditUrl = PageUrl & "a=gridedit"
			MultiDeleteUrl = "pageimage_delete.aspx"
			MultiUpdateUrl = "pageimage_update.aspx"

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
				PageImage.Export = ew_Get("export")
			ElseIf ew_Post("exporttype") <> "" Then
				PageImage.Export = ew_Post("exporttype")
			Else
				PageImage.ExportReturnUrl = ew_CurrentUrl()
			End If
			ParentPage.gsExport = PageImage.Export ' Get export parameter, used in header
			ParentPage.gsExportFile = PageImage.TableVar ' Get export file, used in header
			If PageImage.Export = "excel" Then
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
			PageImage.Dispose()
			Image.Dispose()
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
			Call PageImage.Recordset_SearchValidated()

			' Set Up Sorting Order
			SetUpSortOrder()

			' Get search criteria for advanced search
			If ParentPage.gsSearchError = "" Then
				sSrchAdvanced = AdvancedSearchWhere()
			End If
		End If

		' Restore display records
		If (PageImage.RecordsPerPage = -1 OrElse PageImage.RecordsPerPage > 0) Then
			lDisplayRecs = PageImage.RecordsPerPage ' Restore from Session
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
		PageImage.Recordset_Searching(sSrchWhere)

		' Save search criteria
		If sSrchWhere <> "" Then
			If sSrchAdvanced = "" Then ResetAdvancedSearchParms()
			PageImage.SearchWhere = sSrchWhere ' Save to Session
			If RestoreSearch Then
				lStartRec = 1 ' Reset start record counter
				PageImage.StartRecordNumber = lStartRec
			End If
		Else
			sSrchWhere = PageImage.SearchWhere
		End If

		' Build filter
		sFilter = ""

		' Restore master/detail filter
		sDbMasterFilter = PageImage.MasterFilter ' Restore master filter
		sDbDetailFilter = PageImage.DetailFilter ' Restore detail filter
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
		If PageImage.MasterFilter <> "" AndAlso PageImage.CurrentMasterTable = "zPage" Then
			RsMaster = zPage.LoadRs(sDbMasterFilter)
			bMasterRecordExists = RsMaster IsNot Nothing
			If Not bMasterRecordExists Then
				PageImage.MasterFilter = "" ' Clear master filter
				PageImage.DetailFilter = "" ' Clear detail filter
				Message = Language.Phrase("NoRecord") ' Set no record found
			Page_Terminate(PageImage.ReturnUrl) ' Return to caller
			Else
				RsMaster.Read()
				zPage.LoadListRowValues(RsMaster)
				zPage.RowType = EW_ROWTYPE_MASTER ' Master row
				zPage.RenderListRow()
				RsMaster.Close()
				RsMaster.Dispose()
			End If
		End If

		' Load master record
		If PageImage.MasterFilter <> "" AndAlso PageImage.CurrentMasterTable = "Image" Then
			RsMaster = Image.LoadRs(sDbMasterFilter)
			bMasterRecordExists = RsMaster IsNot Nothing
			If Not bMasterRecordExists Then
				PageImage.MasterFilter = "" ' Clear master filter
				PageImage.DetailFilter = "" ' Clear detail filter
				Message = Language.Phrase("NoRecord") ' Set no record found
			Page_Terminate(PageImage.ReturnUrl) ' Return to caller
			Else
				RsMaster.Read()
				Image.LoadListRowValues(RsMaster)
				Image.RowType = EW_ROWTYPE_MASTER ' Master row
				Image.RenderListRow()
				RsMaster.Close()
				RsMaster.Dispose()
			End If
		End If

		' Set up filter in Session
		PageImage.SessionWhere = sFilter
		PageImage.CurrentFilter = ""

		' Export Data only
		If PageImage.Export = "html" OrElse PageImage.Export = "csv" OrElse PageImage.Export = "word" OrElse PageImage.Export = "excel" OrElse PageImage.Export = "xml" Then
			ExportData()
			Page_Terminate("") ' Clean up
			ew_End() ' Terminate response
		ElseIf PageImage.Export = "email" Then
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
			PageImage.RecordsPerPage = lDisplayRecs ' Save to Session

			' Reset start position
			lStartRec = 1
			PageImage.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	' Return Advanced Search WHERE based on QueryString parameters
	'
	Function AdvancedSearchWhere() As String		
		Dim sWhere As String = ""
		BuildSearchSql(sWhere, PageImage.zPageID, False) ' PageID
		BuildSearchSql(sWhere, PageImage.ImageID, False) ' ImageID
		BuildSearchSql(sWhere, PageImage.PageImagePosition, False) ' PageImagePosition

		' Set up search parm
		If sWhere <> "" Then
			SetSearchParm(PageImage.zPageID) ' PageID
			SetSearchParm(PageImage.ImageID) ' ImageID
			SetSearchParm(PageImage.PageImagePosition) ' PageImagePosition
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
		PageImage.SetAdvancedSearch("x_" & FldParm, Fld.AdvancedSearch.SearchValue)
		PageImage.SetAdvancedSearch("z_" & FldParm, Fld.AdvancedSearch.SearchOperator)
		PageImage.SetAdvancedSearch("v_" & FldParm, Fld.AdvancedSearch.SearchCondition)
		PageImage.SetAdvancedSearch("y_" & FldParm, Fld.AdvancedSearch.SearchValue2)
		PageImage.SetAdvancedSearch("w_" & FldParm, Fld.AdvancedSearch.SearchOperator2)
	End Sub

	'
	' Get search parm
	'
	Sub GetSearchParm(ByRef Fld As Object)
		Dim FldParm As String = Fld.FldVar.Substring(2)
		Fld.AdvancedSearch.SearchValue = PageImage.GetAdvancedSearch("x_" & FldParm)
		Fld.AdvancedSearch.SearchOperator = PageImage.GetAdvancedSearch("z_" & FldParm)
		Fld.AdvancedSearch.SearchCondition = PageImage.GetAdvancedSearch("v_" & FldParm)
		Fld.AdvancedSearch.SearchValue2 = PageImage.GetAdvancedSearch("y_" & FldParm)
		Fld.AdvancedSearch.SearchOperator2 = PageImage.GetAdvancedSearch("w_" & FldParm)
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
		PageImage.SearchWhere = sSrchWhere

		' Clear advanced search parameters
		ResetAdvancedSearchParms()
	End Sub

	'
	' Clear all advanced search parameters
	'
	Sub ResetAdvancedSearchParms()
		PageImage.SetAdvancedSearch("x_zPageID", "")
		PageImage.SetAdvancedSearch("x_ImageID", "")
		PageImage.SetAdvancedSearch("x_PageImagePosition", "")
	End Sub

	'
	' Restore all search parameters
	'
	Sub RestoreSearchParms()
		Dim bRestore As Boolean = True
		If ew_Get("x_zPageID") <> "" Then bRestore = False
		If ew_Get("x_ImageID") <> "" Then bRestore = False
		If ew_Get("x_PageImagePosition") <> "" Then bRestore = False
		RestoreSearch = bRestore
		If bRestore Then

			' Restore advanced search values
			Call GetSearchParm(PageImage.zPageID)
			Call GetSearchParm(PageImage.ImageID)
			Call GetSearchParm(PageImage.PageImagePosition)
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
			PageImage.CurrentOrder = ew_Get("order")
			PageImage.CurrentOrderType = ew_Get("ordertype")
			PageImage.UpdateSort(PageImage.zPageID) ' PageID
			PageImage.UpdateSort(PageImage.ImageID) ' ImageID
			PageImage.UpdateSort(PageImage.PageImagePosition) ' PageImagePosition
			PageImage.StartRecordNumber = 1 ' Reset start position
		End If
	End Sub

	'
	' Load Sort Order parameters
	'
	Sub LoadSortOrder()
		Dim sOrderBy As String
		sOrderBy = PageImage.SessionOrderBy ' Get order by from Session
		If sOrderBy = "" Then
			If PageImage.SqlOrderBy <> "" Then
				sOrderBy = PageImage.SqlOrderBy
				PageImage.SessionOrderBy = sOrderBy
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
				PageImage.CurrentMasterTable = "" ' Clear master table
				PageImage.MasterFilter = "" ' Clear master filter
				sDbMasterFilter = ""
				PageImage.DetailFilter = "" ' Clear detail filter
				sDbDetailFilter = ""
				PageImage.zPageID.SessionValue = ""
				PageImage.ImageID.SessionValue = ""
			End If

			' Reset sort criteria
			If ew_SameText(sCmd, "resetsort") Then
				Dim sOrderBy As String = ""
				PageImage.SessionOrderBy = sOrderBy
				PageImage.zPageID.Sort = ""
				PageImage.ImageID.Sort = ""
				PageImage.PageImagePosition.Sort = ""
			End If

			' Reset start position
			lStartRec = 1
			PageImage.StartRecordNumber = lStartRec
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
		If PageImage.Export <> "" Or PageImage.CurrentAction = "gridadd" Or PageImage.CurrentAction = "gridedit" Then
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
				PageImage.StartRecordNumber = lStartRec
			ElseIf ew_Get(EW_TABLE_PAGE_NO) <> "" AndAlso IsNumeric(ew_Get(EW_TABLE_PAGE_NO)) Then
				nPageNo = ew_ConvertToInt(ew_Get(EW_TABLE_PAGE_NO))
				lStartRec = (nPageNo-1)*lDisplayRecs+1
				If lStartRec <= 0 Then
					lStartRec = 1
				ElseIf lStartRec >= ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 Then
					lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1
				End If
				PageImage.StartRecordNumber = lStartRec
			End If
		End If
		lStartRec = PageImage.StartRecordNumber

		' Check if correct start record counter
		If lStartRec <= 0 Then ' Avoid invalid start record counter
			lStartRec = 1 ' Reset start record counter
			PageImage.StartRecordNumber = lStartRec
		ElseIf lStartRec > lTotalRecs Then ' Avoid starting record > total records
			lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to last page first record
			PageImage.StartRecordNumber = lStartRec
		ElseIf (lStartRec-1) Mod lDisplayRecs <> 0 Then
			lStartRec = ((lStartRec-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to page boundary
			PageImage.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	' Load default values
	'
	Sub LoadDefaultValues()
		PageImage.zPageID.CurrentValue = 0
		PageImage.ImageID.CurrentValue = 0
		PageImage.PageImagePosition.CurrentValue = 0
	End Sub

	'
	'  Load search values for validation
	'
	Sub LoadSearchValues()
		PageImage.zPageID.AdvancedSearch.SearchValue = ew_Get("x_zPageID")
    	PageImage.zPageID.AdvancedSearch.SearchOperator = ew_Get("z_zPageID")
		PageImage.ImageID.AdvancedSearch.SearchValue = ew_Get("x_ImageID")
    	PageImage.ImageID.AdvancedSearch.SearchOperator = ew_Get("z_ImageID")
		PageImage.PageImagePosition.AdvancedSearch.SearchValue = ew_Get("x_PageImagePosition")
    	PageImage.PageImagePosition.AdvancedSearch.SearchOperator = ew_Get("z_PageImagePosition")
	End Sub

	'
	' Load recordset
	'
	Function LoadRecordset() As OleDbDataReader

		' Recordset Selecting event
		PageImage.Recordset_Selecting(PageImage.CurrentFilter)

		' Load list page SQL
		Dim sSql As String = PageImage.ListSQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then DebugMsg = sSql

		' Count
		lTotalRecs = -1		
		Try
			Dim sCntSql As String = PageImage.SelectCountSQL

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
		PageImage.Recordset_Selected(Rs)
		Return Rs
	End Function

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = PageImage.KeyFilter

		' Row Selecting event
		PageImage.Row_Selecting(sFilter)

		' Load SQL based on filter
		PageImage.CurrentFilter = sFilter
		Dim sSql As String = PageImage.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then DebugMsg = sSql
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				PageImage.Row_Selected(RsRow)
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
		PageImage.PageImageID.DbValue = RsRow("PageImageID")
		PageImage.zPageID.DbValue = RsRow("PageID")
		PageImage.ImageID.DbValue = RsRow("ImageID")
		PageImage.PageImagePosition.DbValue = RsRow("PageImagePosition")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Initialize urls
		ViewUrl = PageImage.ViewUrl
		EditUrl = PageImage.EditUrl
		InlineEditUrl = PageImage.InlineEditUrl
		CopyUrl = PageImage.CopyUrl
		InlineCopyUrl = PageImage.InlineCopyUrl
		DeleteUrl = PageImage.DeleteUrl

		' Row Rendering event
		PageImage.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' PageID

		PageImage.zPageID.CellCssStyle = "white-space: nowrap;"
		PageImage.zPageID.CellCssClass = ""
		PageImage.zPageID.CellAttrs.Clear(): PageImage.zPageID.ViewAttrs.Clear(): PageImage.zPageID.EditAttrs.Clear()

		' ImageID
		PageImage.ImageID.CellCssStyle = "white-space: nowrap;"
		PageImage.ImageID.CellCssClass = ""
		PageImage.ImageID.CellAttrs.Clear(): PageImage.ImageID.ViewAttrs.Clear(): PageImage.ImageID.EditAttrs.Clear()

		' PageImagePosition
		PageImage.PageImagePosition.CellCssStyle = "white-space: nowrap;"
		PageImage.PageImagePosition.CellCssClass = ""
		PageImage.PageImagePosition.CellAttrs.Clear(): PageImage.PageImagePosition.ViewAttrs.Clear(): PageImage.PageImagePosition.EditAttrs.Clear()

		'
		'  View  Row
		'

		If PageImage.RowType = EW_ROWTYPE_VIEW Then ' View row

			' PageImageID
			PageImage.PageImageID.ViewValue = PageImage.PageImageID.CurrentValue
			PageImage.PageImageID.CssStyle = ""
			PageImage.PageImageID.CssClass = ""
			PageImage.PageImageID.ViewCustomAttributes = ""

			' PageID
			If ew_NotEmpty(PageImage.zPageID.CurrentValue) Then
				sFilterWrk = "[PageID] = " & ew_AdjustSql(PageImage.zPageID.CurrentValue) & ""
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
					PageImage.zPageID.ViewValue = RsWrk("PageName")
				Else
					PageImage.zPageID.ViewValue = PageImage.zPageID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				PageImage.zPageID.ViewValue = System.DBNull.Value
			End If
			PageImage.zPageID.CssStyle = ""
			PageImage.zPageID.CssClass = ""
			PageImage.zPageID.ViewCustomAttributes = ""

			' ImageID
			If ew_NotEmpty(PageImage.ImageID.CurrentValue) Then
				sFilterWrk = "[ImageID] = " & ew_AdjustSql(PageImage.ImageID.CurrentValue) & ""
			sSqlWrk = "SELECT [ImageName] FROM [Image]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [ImageName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					PageImage.ImageID.ViewValue = RsWrk("ImageName")
				Else
					PageImage.ImageID.ViewValue = PageImage.ImageID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				PageImage.ImageID.ViewValue = System.DBNull.Value
			End If
			PageImage.ImageID.CssStyle = ""
			PageImage.ImageID.CssClass = ""
			PageImage.ImageID.ViewCustomAttributes = ""

			' PageImagePosition
			PageImage.PageImagePosition.ViewValue = PageImage.PageImagePosition.CurrentValue
			PageImage.PageImagePosition.CssStyle = ""
			PageImage.PageImagePosition.CssClass = ""
			PageImage.PageImagePosition.ViewCustomAttributes = ""

			' View refer script
			' PageID

			PageImage.zPageID.HrefValue = ""
			PageImage.zPageID.TooltipValue = ""

			' ImageID
			PageImage.ImageID.HrefValue = ""
			PageImage.ImageID.TooltipValue = ""

			' PageImagePosition
			PageImage.PageImagePosition.HrefValue = ""
			PageImage.PageImagePosition.TooltipValue = ""
		End If

		' Row Rendered event
		If PageImage.RowType <> EW_ROWTYPE_AGGREGATEINIT Then
			PageImage.Row_Rendered()
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
		PageImage.zPageID.AdvancedSearch.SearchValue = PageImage.GetAdvancedSearch("x_zPageID")
		PageImage.ImageID.AdvancedSearch.SearchValue = PageImage.GetAdvancedSearch("x_ImageID")
		PageImage.PageImagePosition.AdvancedSearch.SearchValue = PageImage.GetAdvancedSearch("x_PageImagePosition")
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
		If PageImage.ExportAll Then
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
		If PageImage.Export = "xml" Then
			oXmlDoc = New cXMLDocument()
		Else
			sOutputStr = ew_ExportHeader(PageImage.Export)

			' Horizontal format, write header
			If sExportStyle <> "v" OrElse PageImage.Export = "csv" Then
				sExportStr = ""
				ew_ExportAddValue(sExportStr, PageImage.PageImageID.ExportCaption, PageImage.Export, IIf(EW_EXPORT_CSS_STYLES, PageImage.PageImageID.CellStyles, ""))
				ew_ExportAddValue(sExportStr, PageImage.zPageID.ExportCaption, PageImage.Export, IIf(EW_EXPORT_CSS_STYLES, PageImage.zPageID.CellStyles, ""))
				ew_ExportAddValue(sExportStr, PageImage.ImageID.ExportCaption, PageImage.Export, IIf(EW_EXPORT_CSS_STYLES, PageImage.ImageID.CellStyles, ""))
				ew_ExportAddValue(sExportStr, PageImage.PageImagePosition.ExportCaption, PageImage.Export, IIf(EW_EXPORT_CSS_STYLES, PageImage.PageImagePosition.CellStyles, ""))
				sOutputStr &= ew_ExportLine(sExportStr, PageImage.Export, IIf(EW_EXPORT_CSS_STYLES, PageImage.RowStyles, ""))
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
				PageImage.CssClass = ""
				PageImage.CssStyle = ""
				PageImage.RowType = EW_ROWTYPE_VIEW ' Render view
				RenderRow()
				If PageImage.Export = "xml" Then
					oXmlDoc.AddRow()
					oXmlDoc.AddField("PageImageID", PageImage.PageImageID.ExportValue(PageImage.Export, PageImage.ExportOriginalValue)) ' PageImageID
					oXmlDoc.AddField("zPageID", PageImage.zPageID.ExportValue(PageImage.Export, PageImage.ExportOriginalValue)) ' PageID
					oXmlDoc.AddField("ImageID", PageImage.ImageID.ExportValue(PageImage.Export, PageImage.ExportOriginalValue)) ' ImageID
					oXmlDoc.AddField("PageImagePosition", PageImage.PageImagePosition.ExportValue(PageImage.Export, PageImage.ExportOriginalValue)) ' PageImagePosition
				Else

					' Vertical format
					If sExportStyle = "v" AndAlso PageImage.Export <> "csv" Then
						sOutputStr &= ew_ExportField(PageImage.PageImageID.ExportCaption, PageImage.PageImageID.ExportValue(PageImage.Export, PageImage.ExportOriginalValue), PageImage.Export, IIf(EW_EXPORT_CSS_STYLES, PageImage.PageImageID.CellStyles, "")) ' PageImageID
						sOutputStr &= ew_ExportField(PageImage.zPageID.ExportCaption, PageImage.zPageID.ExportValue(PageImage.Export, PageImage.ExportOriginalValue), PageImage.Export, IIf(EW_EXPORT_CSS_STYLES, PageImage.zPageID.CellStyles, "")) ' PageID
						sOutputStr &= ew_ExportField(PageImage.ImageID.ExportCaption, PageImage.ImageID.ExportValue(PageImage.Export, PageImage.ExportOriginalValue), PageImage.Export, IIf(EW_EXPORT_CSS_STYLES, PageImage.ImageID.CellStyles, "")) ' ImageID
						sOutputStr &= ew_ExportField(PageImage.PageImagePosition.ExportCaption, PageImage.PageImagePosition.ExportValue(PageImage.Export, PageImage.ExportOriginalValue), PageImage.Export, IIf(EW_EXPORT_CSS_STYLES, PageImage.PageImagePosition.CellStyles, "")) ' PageImagePosition

					' Horizontal format
					Else
						sExportStr = ""
						ew_ExportAddValue(sExportStr, PageImage.PageImageID.ExportValue(PageImage.Export, PageImage.ExportOriginalValue), PageImage.Export, IIf(EW_EXPORT_CSS_STYLES, PageImage.PageImageID.CellStyles, ""))
						ew_ExportAddValue(sExportStr, PageImage.zPageID.ExportValue(PageImage.Export, PageImage.ExportOriginalValue), PageImage.Export, IIf(EW_EXPORT_CSS_STYLES, PageImage.zPageID.CellStyles, ""))
						ew_ExportAddValue(sExportStr, PageImage.ImageID.ExportValue(PageImage.Export, PageImage.ExportOriginalValue), PageImage.Export, IIf(EW_EXPORT_CSS_STYLES, PageImage.ImageID.CellStyles, ""))
						ew_ExportAddValue(sExportStr, PageImage.PageImagePosition.ExportValue(PageImage.Export, PageImage.ExportOriginalValue), PageImage.Export, IIf(EW_EXPORT_CSS_STYLES, PageImage.PageImagePosition.CellStyles, ""))
						sOutputStr &= ew_ExportLine(sExportStr, PageImage.Export, IIf(EW_EXPORT_CSS_STYLES, PageImage.RowStyles, ""))
					End If
				End If
			End If
		Loop

		' Close recordset
		Rs.Close()
		Rs.Dispose()
		If PageImage.Export = "xml" Then
			oXmlDoc.Output
		Else
			sOutputStr &= ew_ExportFooter(PageImage.Export)
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
			If sMasterTblVar = "zPage" Then
				bValidMaster = True
				sDbMasterFilter = PageImage.SqlMasterFilter_zPage
				sDbDetailFilter = PageImage.SqlDetailFilter_zPage
				If ew_Get("zPageID") <> "" Then
					zPage.zPageID.QueryStringValue = ew_Get("zPageID")
					PageImage.zPageID.QueryStringValue = zPage.zPageID.QueryStringValue
					PageImage.zPageID.SessionValue = PageImage.zPageID.QueryStringValue
					If Not IsNumeric(zPage.zPageID.QueryStringValue) Then bValidMaster = False
					sDbMasterFilter = sDbMasterFilter.Replace("@zPageID@", ew_AdjustSql(zPage.zPageID.QueryStringValue))
					sDbDetailFilter = sDbDetailFilter.Replace("@zPageID@", ew_AdjustSql(zPage.zPageID.QueryStringValue))
				Else
					bValidMaster = False
				End If
			End If
			If sMasterTblVar = "Image" Then
				bValidMaster = True
				sDbMasterFilter = PageImage.SqlMasterFilter_Image
				sDbDetailFilter = PageImage.SqlDetailFilter_Image
				If ew_Get("ImageID") <> "" Then
					Image.ImageID.QueryStringValue = ew_Get("ImageID")
					PageImage.ImageID.QueryStringValue = Image.ImageID.QueryStringValue
					PageImage.ImageID.SessionValue = PageImage.ImageID.QueryStringValue
					If Not IsNumeric(Image.ImageID.QueryStringValue) Then bValidMaster = False
					sDbMasterFilter = sDbMasterFilter.Replace("@ImageID@", ew_AdjustSql(Image.ImageID.QueryStringValue))
					sDbDetailFilter = sDbDetailFilter.Replace("@ImageID@", ew_AdjustSql(Image.ImageID.QueryStringValue))
				Else
					bValidMaster = False
				End If
			End If
		End If
		If bValidMaster Then

			' Save current master table
			PageImage.CurrentMasterTable = sMasterTblVar

			' Reset start record counter (new master key)
			lStartRec = 1
			PageImage.StartRecordNumber = lStartRec
			PageImage.MasterFilter = sDbMasterFilter ' Set up master filter
			PageImage.DetailFilter = sDbDetailFilter ' Set up detail filter

			' Clear previous master session values
			If sMasterTblVar <> "zPage" Then
				If PageImage.zPageID.QueryStringValue = "" Then PageImage.zPageID.SessionValue = ""
			End If
			If sMasterTblVar <> "Image" Then
				If PageImage.ImageID.QueryStringValue = "" Then PageImage.ImageID.SessionValue = ""
			End If
		End If
	End Sub

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "PageImage"
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
		PageImage_list = New cPageImage_list(Me)		
		PageImage_list.Page_Init()		

		' Set buffer/cache option
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

		' Page main processing
		PageImage_list.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If PageImage_list IsNot Nothing Then PageImage_list.Dispose()
	End Sub
End Class
