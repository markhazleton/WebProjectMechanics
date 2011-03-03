Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class role_list
	Inherits AspNetMaker8_wpmWebsite

	' Page object
	Public role_list As crole_list

	'
	' Page Class
	'
	Class crole_list
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
				If role.UseTokenInUrl Then Url = Url & "t=" & role.TableVar & "&" ' Add page token
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
			If role.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (role.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (role.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' ASP.NET page object
		Public ReadOnly Property AspNetPage() As role_list
			Get
				Return CType(m_ParentPage, role_list)
			End Get
		End Property

		' role
		Public Property role() As crole
			Get				
				Return ParentPage.role
			End Get
			Set(ByVal v As crole)
				ParentPage.role = v	
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
			m_PageObjName = "role_list"
			m_PageObjTypeName = "crole_list"			

			' Initialize language object
			Language = New cLanguage(Me)

			' Table Name
			m_TableName = "role"

			' Initialize table object
			role = New crole(Me)

			' Initialize URLs
			ExportPrintUrl = PageUrl & "export=print"
			ExportExcelUrl = PageUrl & "export=excel"
			ExportWordUrl = PageUrl & "export=word"
			ExportHtmlUrl = PageUrl & "export=html"
			ExportXmlUrl = PageUrl & "export=xml"
			ExportCsvUrl = PageUrl & "export=csv"
			AddUrl = role.AddUrl
			InlineAddUrl = PageUrl & "a=add"
			GridAddUrl = PageUrl & "a=gridadd"
			GridEditUrl = PageUrl & "a=gridedit"
			MultiDeleteUrl = "role_delete.aspx"
			MultiUpdateUrl = "role_update.aspx"

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
				role.Export = ew_Get("export")
			ElseIf ew_Post("exporttype") <> "" Then
				role.Export = ew_Post("exporttype")
			Else
				role.ExportReturnUrl = ew_CurrentUrl()
			End If
			ParentPage.gsExport = role.Export ' Get export parameter, used in header
			ParentPage.gsExportFile = role.TableVar ' Get export file, used in header
			If role.Export = "excel" Then
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
			role.Dispose()
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
			Call role.Recordset_SearchValidated()

			' Set Up Sorting Order
			SetUpSortOrder()

			' Get search criteria for advanced search
			If ParentPage.gsSearchError = "" Then
				sSrchAdvanced = AdvancedSearchWhere()
			End If
		End If

		' Restore display records
		If (role.RecordsPerPage = -1 OrElse role.RecordsPerPage > 0) Then
			lDisplayRecs = role.RecordsPerPage ' Restore from Session
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
		role.Recordset_Searching(sSrchWhere)

		' Save search criteria
		If sSrchWhere <> "" Then
			If sSrchAdvanced = "" Then ResetAdvancedSearchParms()
			role.SearchWhere = sSrchWhere ' Save to Session
			If RestoreSearch Then
				lStartRec = 1 ' Reset start record counter
				role.StartRecordNumber = lStartRec
			End If
		Else
			sSrchWhere = role.SearchWhere
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
		role.SessionWhere = sFilter
		role.CurrentFilter = ""

		' Export Data only
		If role.Export = "html" OrElse role.Export = "csv" OrElse role.Export = "word" OrElse role.Export = "excel" OrElse role.Export = "xml" Then
			ExportData()
			Page_Terminate("") ' Clean up
			ew_End() ' Terminate response
		ElseIf role.Export = "email" Then
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
			role.RecordsPerPage = lDisplayRecs ' Save to Session

			' Reset start position
			lStartRec = 1
			role.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	' Return Advanced Search WHERE based on QueryString parameters
	'
	Function AdvancedSearchWhere() As String		
		Dim sWhere As String = ""
		BuildSearchSql(sWhere, role.RoleID, False) ' RoleID
		BuildSearchSql(sWhere, role.RoleName, False) ' RoleName
		BuildSearchSql(sWhere, role.RoleTitle, False) ' RoleTitle
		BuildSearchSql(sWhere, role.RoleComment, False) ' RoleComment
		BuildSearchSql(sWhere, role.FilterMenu, False) ' FilterMenu

		' Set up search parm
		If sWhere <> "" Then
			SetSearchParm(role.RoleID) ' RoleID
			SetSearchParm(role.RoleName) ' RoleName
			SetSearchParm(role.RoleTitle) ' RoleTitle
			SetSearchParm(role.RoleComment) ' RoleComment
			SetSearchParm(role.FilterMenu) ' FilterMenu
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
		role.SetAdvancedSearch("x_" & FldParm, Fld.AdvancedSearch.SearchValue)
		role.SetAdvancedSearch("z_" & FldParm, Fld.AdvancedSearch.SearchOperator)
		role.SetAdvancedSearch("v_" & FldParm, Fld.AdvancedSearch.SearchCondition)
		role.SetAdvancedSearch("y_" & FldParm, Fld.AdvancedSearch.SearchValue2)
		role.SetAdvancedSearch("w_" & FldParm, Fld.AdvancedSearch.SearchOperator2)
	End Sub

	'
	' Get search parm
	'
	Sub GetSearchParm(ByRef Fld As Object)
		Dim FldParm As String = Fld.FldVar.Substring(2)
		Fld.AdvancedSearch.SearchValue = role.GetAdvancedSearch("x_" & FldParm)
		Fld.AdvancedSearch.SearchOperator = role.GetAdvancedSearch("z_" & FldParm)
		Fld.AdvancedSearch.SearchCondition = role.GetAdvancedSearch("v_" & FldParm)
		Fld.AdvancedSearch.SearchValue2 = role.GetAdvancedSearch("y_" & FldParm)
		Fld.AdvancedSearch.SearchOperator2 = role.GetAdvancedSearch("w_" & FldParm)
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
		role.SearchWhere = sSrchWhere

		' Clear advanced search parameters
		ResetAdvancedSearchParms()
	End Sub

	'
	' Clear all advanced search parameters
	'
	Sub ResetAdvancedSearchParms()
		role.SetAdvancedSearch("x_RoleID", "")
		role.SetAdvancedSearch("x_RoleName", "")
		role.SetAdvancedSearch("x_RoleTitle", "")
		role.SetAdvancedSearch("x_RoleComment", "")
		role.SetAdvancedSearch("x_FilterMenu", "")
	End Sub

	'
	' Restore all search parameters
	'
	Sub RestoreSearchParms()
		Dim bRestore As Boolean = True
		If ew_Get("x_RoleID") <> "" Then bRestore = False
		If ew_Get("x_RoleName") <> "" Then bRestore = False
		If ew_Get("x_RoleTitle") <> "" Then bRestore = False
		If ew_Get("x_RoleComment") <> "" Then bRestore = False
		If ew_Get("x_FilterMenu") <> "" Then bRestore = False
		RestoreSearch = bRestore
		If bRestore Then

			' Restore advanced search values
			Call GetSearchParm(role.RoleID)
			Call GetSearchParm(role.RoleName)
			Call GetSearchParm(role.RoleTitle)
			Call GetSearchParm(role.RoleComment)
			Call GetSearchParm(role.FilterMenu)
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
			role.CurrentOrder = ew_Get("order")
			role.CurrentOrderType = ew_Get("ordertype")
			role.UpdateSort(role.RoleID) ' RoleID
			role.UpdateSort(role.RoleName) ' RoleName
			role.UpdateSort(role.RoleTitle) ' RoleTitle
			role.UpdateSort(role.RoleComment) ' RoleComment
			role.UpdateSort(role.FilterMenu) ' FilterMenu
			role.StartRecordNumber = 1 ' Reset start position
		End If
	End Sub

	'
	' Load Sort Order parameters
	'
	Sub LoadSortOrder()
		Dim sOrderBy As String
		sOrderBy = role.SessionOrderBy ' Get order by from Session
		If sOrderBy = "" Then
			If role.SqlOrderBy <> "" Then
				sOrderBy = role.SqlOrderBy
				role.SessionOrderBy = sOrderBy
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
				role.SessionOrderBy = sOrderBy
				role.RoleID.Sort = ""
				role.RoleName.Sort = ""
				role.RoleTitle.Sort = ""
				role.RoleComment.Sort = ""
				role.FilterMenu.Sort = ""
			End If

			' Reset start position
			lStartRec = 1
			role.StartRecordNumber = lStartRec
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
		If role.Export <> "" Or role.CurrentAction = "gridadd" Or role.CurrentAction = "gridedit" Then
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
				role.StartRecordNumber = lStartRec
			ElseIf ew_Get(EW_TABLE_PAGE_NO) <> "" AndAlso IsNumeric(ew_Get(EW_TABLE_PAGE_NO)) Then
				nPageNo = ew_ConvertToInt(ew_Get(EW_TABLE_PAGE_NO))
				lStartRec = (nPageNo-1)*lDisplayRecs+1
				If lStartRec <= 0 Then
					lStartRec = 1
				ElseIf lStartRec >= ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 Then
					lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1
				End If
				role.StartRecordNumber = lStartRec
			End If
		End If
		lStartRec = role.StartRecordNumber

		' Check if correct start record counter
		If lStartRec <= 0 Then ' Avoid invalid start record counter
			lStartRec = 1 ' Reset start record counter
			role.StartRecordNumber = lStartRec
		ElseIf lStartRec > lTotalRecs Then ' Avoid starting record > total records
			lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to last page first record
			role.StartRecordNumber = lStartRec
		ElseIf (lStartRec-1) Mod lDisplayRecs <> 0 Then
			lStartRec = ((lStartRec-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to page boundary
			role.StartRecordNumber = lStartRec
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
		role.RoleID.AdvancedSearch.SearchValue = ew_Get("x_RoleID")
    	role.RoleID.AdvancedSearch.SearchOperator = ew_Get("z_RoleID")
		role.RoleName.AdvancedSearch.SearchValue = ew_Get("x_RoleName")
    	role.RoleName.AdvancedSearch.SearchOperator = ew_Get("z_RoleName")
		role.RoleTitle.AdvancedSearch.SearchValue = ew_Get("x_RoleTitle")
    	role.RoleTitle.AdvancedSearch.SearchOperator = ew_Get("z_RoleTitle")
		role.RoleComment.AdvancedSearch.SearchValue = ew_Get("x_RoleComment")
    	role.RoleComment.AdvancedSearch.SearchOperator = ew_Get("z_RoleComment")
		role.FilterMenu.AdvancedSearch.SearchValue = ew_Get("x_FilterMenu")
    	role.FilterMenu.AdvancedSearch.SearchOperator = ew_Get("z_FilterMenu")
	End Sub

	'
	' Load recordset
	'
	Function LoadRecordset() As OleDbDataReader

		' Recordset Selecting event
		role.Recordset_Selecting(role.CurrentFilter)

		' Load list page SQL
		Dim sSql As String = role.ListSQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then DebugMsg = sSql

		' Count
		lTotalRecs = -1		
		Try
			Dim sCntSql As String = role.SelectCountSQL

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
		role.Recordset_Selected(Rs)
		Return Rs
	End Function

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = role.KeyFilter

		' Row Selecting event
		role.Row_Selecting(sFilter)

		' Load SQL based on filter
		role.CurrentFilter = sFilter
		Dim sSql As String = role.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then DebugMsg = sSql
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				role.Row_Selected(RsRow)
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
		role.RoleID.DbValue = RsRow("RoleID")
		role.RoleName.DbValue = RsRow("RoleName")
		role.RoleTitle.DbValue = RsRow("RoleTitle")
		role.RoleComment.DbValue = RsRow("RoleComment")
		role.FilterMenu.DbValue = IIf(ew_ConvertToBool(RsRow("FilterMenu")), "1", "0")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Initialize urls
		ViewUrl = role.ViewUrl
		EditUrl = role.EditUrl
		InlineEditUrl = role.InlineEditUrl
		CopyUrl = role.CopyUrl
		InlineCopyUrl = role.InlineCopyUrl
		DeleteUrl = role.DeleteUrl

		' Row Rendering event
		role.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' RoleID

		role.RoleID.CellCssStyle = ""
		role.RoleID.CellCssClass = ""
		role.RoleID.CellAttrs.Clear(): role.RoleID.ViewAttrs.Clear(): role.RoleID.EditAttrs.Clear()

		' RoleName
		role.RoleName.CellCssStyle = ""
		role.RoleName.CellCssClass = ""
		role.RoleName.CellAttrs.Clear(): role.RoleName.ViewAttrs.Clear(): role.RoleName.EditAttrs.Clear()

		' RoleTitle
		role.RoleTitle.CellCssStyle = ""
		role.RoleTitle.CellCssClass = ""
		role.RoleTitle.CellAttrs.Clear(): role.RoleTitle.ViewAttrs.Clear(): role.RoleTitle.EditAttrs.Clear()

		' RoleComment
		role.RoleComment.CellCssStyle = ""
		role.RoleComment.CellCssClass = ""
		role.RoleComment.CellAttrs.Clear(): role.RoleComment.ViewAttrs.Clear(): role.RoleComment.EditAttrs.Clear()

		' FilterMenu
		role.FilterMenu.CellCssStyle = ""
		role.FilterMenu.CellCssClass = ""
		role.FilterMenu.CellAttrs.Clear(): role.FilterMenu.ViewAttrs.Clear(): role.FilterMenu.EditAttrs.Clear()

		'
		'  View  Row
		'

		If role.RowType = EW_ROWTYPE_VIEW Then ' View row

			' RoleID
			role.RoleID.ViewValue = role.RoleID.CurrentValue
			role.RoleID.CssStyle = ""
			role.RoleID.CssClass = ""
			role.RoleID.ViewCustomAttributes = ""

			' RoleName
			role.RoleName.ViewValue = role.RoleName.CurrentValue
			role.RoleName.CssStyle = ""
			role.RoleName.CssClass = ""
			role.RoleName.ViewCustomAttributes = ""

			' RoleTitle
			role.RoleTitle.ViewValue = role.RoleTitle.CurrentValue
			role.RoleTitle.CssStyle = ""
			role.RoleTitle.CssClass = ""
			role.RoleTitle.ViewCustomAttributes = ""

			' RoleComment
			role.RoleComment.ViewValue = role.RoleComment.CurrentValue
			role.RoleComment.CssStyle = ""
			role.RoleComment.CssClass = ""
			role.RoleComment.ViewCustomAttributes = ""

			' FilterMenu
			If Convert.ToString(role.FilterMenu.CurrentValue) = "1" Then
				role.FilterMenu.ViewValue = "Yes"
			Else
				role.FilterMenu.ViewValue = "No"
			End If
			role.FilterMenu.CssStyle = ""
			role.FilterMenu.CssClass = ""
			role.FilterMenu.ViewCustomAttributes = ""

			' View refer script
			' RoleID

			role.RoleID.HrefValue = ""
			role.RoleID.TooltipValue = ""

			' RoleName
			role.RoleName.HrefValue = ""
			role.RoleName.TooltipValue = ""

			' RoleTitle
			role.RoleTitle.HrefValue = ""
			role.RoleTitle.TooltipValue = ""

			' RoleComment
			role.RoleComment.HrefValue = ""
			role.RoleComment.TooltipValue = ""

			' FilterMenu
			role.FilterMenu.HrefValue = ""
			role.FilterMenu.TooltipValue = ""
		End If

		' Row Rendered event
		If role.RowType <> EW_ROWTYPE_AGGREGATEINIT Then
			role.Row_Rendered()
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
		role.RoleID.AdvancedSearch.SearchValue = role.GetAdvancedSearch("x_RoleID")
		role.RoleName.AdvancedSearch.SearchValue = role.GetAdvancedSearch("x_RoleName")
		role.RoleTitle.AdvancedSearch.SearchValue = role.GetAdvancedSearch("x_RoleTitle")
		role.RoleComment.AdvancedSearch.SearchValue = role.GetAdvancedSearch("x_RoleComment")
		role.FilterMenu.AdvancedSearch.SearchValue = role.GetAdvancedSearch("x_FilterMenu")
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
		If role.ExportAll Then
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
		If role.Export = "xml" Then
			oXmlDoc = New cXMLDocument()
		Else
			sOutputStr = ew_ExportHeader(role.Export)

			' Horizontal format, write header
			If sExportStyle <> "v" OrElse role.Export = "csv" Then
				sExportStr = ""
				ew_ExportAddValue(sExportStr, role.RoleID.ExportCaption, role.Export, IIf(EW_EXPORT_CSS_STYLES, role.RoleID.CellStyles, ""))
				ew_ExportAddValue(sExportStr, role.RoleName.ExportCaption, role.Export, IIf(EW_EXPORT_CSS_STYLES, role.RoleName.CellStyles, ""))
				ew_ExportAddValue(sExportStr, role.RoleTitle.ExportCaption, role.Export, IIf(EW_EXPORT_CSS_STYLES, role.RoleTitle.CellStyles, ""))
				ew_ExportAddValue(sExportStr, role.RoleComment.ExportCaption, role.Export, IIf(EW_EXPORT_CSS_STYLES, role.RoleComment.CellStyles, ""))
				ew_ExportAddValue(sExportStr, role.FilterMenu.ExportCaption, role.Export, IIf(EW_EXPORT_CSS_STYLES, role.FilterMenu.CellStyles, ""))
				sOutputStr &= ew_ExportLine(sExportStr, role.Export, IIf(EW_EXPORT_CSS_STYLES, role.RowStyles, ""))
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
				role.CssClass = ""
				role.CssStyle = ""
				role.RowType = EW_ROWTYPE_VIEW ' Render view
				RenderRow()
				If role.Export = "xml" Then
					oXmlDoc.AddRow()
					oXmlDoc.AddField("RoleID", role.RoleID.ExportValue(role.Export, role.ExportOriginalValue)) ' RoleID
					oXmlDoc.AddField("RoleName", role.RoleName.ExportValue(role.Export, role.ExportOriginalValue)) ' RoleName
					oXmlDoc.AddField("RoleTitle", role.RoleTitle.ExportValue(role.Export, role.ExportOriginalValue)) ' RoleTitle
					oXmlDoc.AddField("RoleComment", role.RoleComment.ExportValue(role.Export, role.ExportOriginalValue)) ' RoleComment
					oXmlDoc.AddField("FilterMenu", role.FilterMenu.ExportValue(role.Export, role.ExportOriginalValue)) ' FilterMenu
				Else

					' Vertical format
					If sExportStyle = "v" AndAlso role.Export <> "csv" Then
						sOutputStr &= ew_ExportField(role.RoleID.ExportCaption, role.RoleID.ExportValue(role.Export, role.ExportOriginalValue), role.Export, IIf(EW_EXPORT_CSS_STYLES, role.RoleID.CellStyles, "")) ' RoleID
						sOutputStr &= ew_ExportField(role.RoleName.ExportCaption, role.RoleName.ExportValue(role.Export, role.ExportOriginalValue), role.Export, IIf(EW_EXPORT_CSS_STYLES, role.RoleName.CellStyles, "")) ' RoleName
						sOutputStr &= ew_ExportField(role.RoleTitle.ExportCaption, role.RoleTitle.ExportValue(role.Export, role.ExportOriginalValue), role.Export, IIf(EW_EXPORT_CSS_STYLES, role.RoleTitle.CellStyles, "")) ' RoleTitle
						sOutputStr &= ew_ExportField(role.RoleComment.ExportCaption, role.RoleComment.ExportValue(role.Export, role.ExportOriginalValue), role.Export, IIf(EW_EXPORT_CSS_STYLES, role.RoleComment.CellStyles, "")) ' RoleComment
						sOutputStr &= ew_ExportField(role.FilterMenu.ExportCaption, role.FilterMenu.ExportValue(role.Export, role.ExportOriginalValue), role.Export, IIf(EW_EXPORT_CSS_STYLES, role.FilterMenu.CellStyles, "")) ' FilterMenu

					' Horizontal format
					Else
						sExportStr = ""
						ew_ExportAddValue(sExportStr, role.RoleID.ExportValue(role.Export, role.ExportOriginalValue), role.Export, IIf(EW_EXPORT_CSS_STYLES, role.RoleID.CellStyles, ""))
						ew_ExportAddValue(sExportStr, role.RoleName.ExportValue(role.Export, role.ExportOriginalValue), role.Export, IIf(EW_EXPORT_CSS_STYLES, role.RoleName.CellStyles, ""))
						ew_ExportAddValue(sExportStr, role.RoleTitle.ExportValue(role.Export, role.ExportOriginalValue), role.Export, IIf(EW_EXPORT_CSS_STYLES, role.RoleTitle.CellStyles, ""))
						ew_ExportAddValue(sExportStr, role.RoleComment.ExportValue(role.Export, role.ExportOriginalValue), role.Export, IIf(EW_EXPORT_CSS_STYLES, role.RoleComment.CellStyles, ""))
						ew_ExportAddValue(sExportStr, role.FilterMenu.ExportValue(role.Export, role.ExportOriginalValue), role.Export, IIf(EW_EXPORT_CSS_STYLES, role.FilterMenu.CellStyles, ""))
						sOutputStr &= ew_ExportLine(sExportStr, role.Export, IIf(EW_EXPORT_CSS_STYLES, role.RowStyles, ""))
					End If
				End If
			End If
		Loop

		' Close recordset
		Rs.Close()
		Rs.Dispose()
		If role.Export = "xml" Then
			oXmlDoc.Output
		Else
			sOutputStr &= ew_ExportFooter(role.Export)
			ew_Write(sOutputStr)
		End If
	End Sub

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "role"
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
		role_list = New crole_list(Me)		
		role_list.Page_Init()		

		' Set buffer/cache option
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

		' Page main processing
		role_list.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If role_list IsNot Nothing Then role_list.Dispose()
	End Sub
End Class
