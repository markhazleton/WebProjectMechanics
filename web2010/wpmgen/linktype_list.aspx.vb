Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class linktype_list
	Inherits AspNetMaker8_wpmWebsite

	' Page object
	Public LinkType_list As cLinkType_list

	'
	' Page Class
	'
	Class cLinkType_list
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
				If LinkType.UseTokenInUrl Then Url = Url & "t=" & LinkType.TableVar & "&" ' Add page token
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
			If LinkType.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (LinkType.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (LinkType.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' ASP.NET page object
		Public ReadOnly Property AspNetPage() As linktype_list
			Get
				Return CType(m_ParentPage, linktype_list)
			End Get
		End Property

		' LinkType
		Public Property LinkType() As cLinkType
			Get				
				Return ParentPage.LinkType
			End Get
			Set(ByVal v As cLinkType)
				ParentPage.LinkType = v	
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
			m_PageObjName = "LinkType_list"
			m_PageObjTypeName = "cLinkType_list"			

			' Initialize language object
			Language = New cLanguage(Me)

			' Table Name
			m_TableName = "LinkType"

			' Initialize table object
			LinkType = New cLinkType(Me)

			' Initialize URLs
			ExportPrintUrl = PageUrl & "export=print"
			ExportExcelUrl = PageUrl & "export=excel"
			ExportWordUrl = PageUrl & "export=word"
			ExportHtmlUrl = PageUrl & "export=html"
			ExportXmlUrl = PageUrl & "export=xml"
			ExportCsvUrl = PageUrl & "export=csv"
			AddUrl = LinkType.AddUrl
			InlineAddUrl = PageUrl & "a=add"
			GridAddUrl = PageUrl & "a=gridadd"
			GridEditUrl = PageUrl & "a=gridedit"
			MultiDeleteUrl = "linktype_delete.aspx"
			MultiUpdateUrl = "linktype_update.aspx"

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
				LinkType.Export = ew_Get("export")
			ElseIf ew_Post("exporttype") <> "" Then
				LinkType.Export = ew_Post("exporttype")
			Else
				LinkType.ExportReturnUrl = ew_CurrentUrl()
			End If
			ParentPage.gsExport = LinkType.Export ' Get export parameter, used in header
			ParentPage.gsExportFile = LinkType.TableVar ' Get export file, used in header
			If LinkType.Export = "excel" Then
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
			LinkType.Dispose()
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
			Call LinkType.Recordset_SearchValidated()

			' Set Up Sorting Order
			SetUpSortOrder()

			' Get search criteria for advanced search
			If ParentPage.gsSearchError = "" Then
				sSrchAdvanced = AdvancedSearchWhere()
			End If
		End If

		' Restore display records
		If (LinkType.RecordsPerPage = -1 OrElse LinkType.RecordsPerPage > 0) Then
			lDisplayRecs = LinkType.RecordsPerPage ' Restore from Session
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
		LinkType.Recordset_Searching(sSrchWhere)

		' Save search criteria
		If sSrchWhere <> "" Then
			If sSrchAdvanced = "" Then ResetAdvancedSearchParms()
			LinkType.SearchWhere = sSrchWhere ' Save to Session
			If RestoreSearch Then
				lStartRec = 1 ' Reset start record counter
				LinkType.StartRecordNumber = lStartRec
			End If
		Else
			sSrchWhere = LinkType.SearchWhere
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
		LinkType.SessionWhere = sFilter
		LinkType.CurrentFilter = ""

		' Export Data only
		If LinkType.Export = "html" OrElse LinkType.Export = "csv" OrElse LinkType.Export = "word" OrElse LinkType.Export = "excel" OrElse LinkType.Export = "xml" Then
			ExportData()
			Page_Terminate("") ' Clean up
			ew_End() ' Terminate response
		ElseIf LinkType.Export = "email" Then
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
			LinkType.RecordsPerPage = lDisplayRecs ' Save to Session

			' Reset start position
			lStartRec = 1
			LinkType.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	' Return Advanced Search WHERE based on QueryString parameters
	'
	Function AdvancedSearchWhere() As String		
		Dim sWhere As String = ""
		BuildSearchSql(sWhere, LinkType.LinkTypeCD, False) ' LinkTypeCD
		BuildSearchSql(sWhere, LinkType.LinkTypeDesc, False) ' LinkTypeDesc
		BuildSearchSql(sWhere, LinkType.LinkTypeComment, False) ' LinkTypeComment
		BuildSearchSql(sWhere, LinkType.LinkTypeTarget, False) ' LinkTypeTarget

		' Set up search parm
		If sWhere <> "" Then
			SetSearchParm(LinkType.LinkTypeCD) ' LinkTypeCD
			SetSearchParm(LinkType.LinkTypeDesc) ' LinkTypeDesc
			SetSearchParm(LinkType.LinkTypeComment) ' LinkTypeComment
			SetSearchParm(LinkType.LinkTypeTarget) ' LinkTypeTarget
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
		LinkType.SetAdvancedSearch("x_" & FldParm, Fld.AdvancedSearch.SearchValue)
		LinkType.SetAdvancedSearch("z_" & FldParm, Fld.AdvancedSearch.SearchOperator)
		LinkType.SetAdvancedSearch("v_" & FldParm, Fld.AdvancedSearch.SearchCondition)
		LinkType.SetAdvancedSearch("y_" & FldParm, Fld.AdvancedSearch.SearchValue2)
		LinkType.SetAdvancedSearch("w_" & FldParm, Fld.AdvancedSearch.SearchOperator2)
	End Sub

	'
	' Get search parm
	'
	Sub GetSearchParm(ByRef Fld As Object)
		Dim FldParm As String = Fld.FldVar.Substring(2)
		Fld.AdvancedSearch.SearchValue = LinkType.GetAdvancedSearch("x_" & FldParm)
		Fld.AdvancedSearch.SearchOperator = LinkType.GetAdvancedSearch("z_" & FldParm)
		Fld.AdvancedSearch.SearchCondition = LinkType.GetAdvancedSearch("v_" & FldParm)
		Fld.AdvancedSearch.SearchValue2 = LinkType.GetAdvancedSearch("y_" & FldParm)
		Fld.AdvancedSearch.SearchOperator2 = LinkType.GetAdvancedSearch("w_" & FldParm)
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
		LinkType.SearchWhere = sSrchWhere

		' Clear advanced search parameters
		ResetAdvancedSearchParms()
	End Sub

	'
	' Clear all advanced search parameters
	'
	Sub ResetAdvancedSearchParms()
		LinkType.SetAdvancedSearch("x_LinkTypeCD", "")
		LinkType.SetAdvancedSearch("x_LinkTypeDesc", "")
		LinkType.SetAdvancedSearch("x_LinkTypeComment", "")
		LinkType.SetAdvancedSearch("x_LinkTypeTarget", "")
	End Sub

	'
	' Restore all search parameters
	'
	Sub RestoreSearchParms()
		Dim bRestore As Boolean = True
		If ew_Get("x_LinkTypeCD") <> "" Then bRestore = False
		If ew_Get("x_LinkTypeDesc") <> "" Then bRestore = False
		If ew_Get("x_LinkTypeComment") <> "" Then bRestore = False
		If ew_Get("x_LinkTypeTarget") <> "" Then bRestore = False
		RestoreSearch = bRestore
		If bRestore Then

			' Restore advanced search values
			Call GetSearchParm(LinkType.LinkTypeCD)
			Call GetSearchParm(LinkType.LinkTypeDesc)
			Call GetSearchParm(LinkType.LinkTypeComment)
			Call GetSearchParm(LinkType.LinkTypeTarget)
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
			LinkType.CurrentOrder = ew_Get("order")
			LinkType.CurrentOrderType = ew_Get("ordertype")
			LinkType.UpdateSort(LinkType.LinkTypeCD) ' LinkTypeCD
			LinkType.UpdateSort(LinkType.LinkTypeDesc) ' LinkTypeDesc
			LinkType.UpdateSort(LinkType.LinkTypeTarget) ' LinkTypeTarget
			LinkType.StartRecordNumber = 1 ' Reset start position
		End If
	End Sub

	'
	' Load Sort Order parameters
	'
	Sub LoadSortOrder()
		Dim sOrderBy As String
		sOrderBy = LinkType.SessionOrderBy ' Get order by from Session
		If sOrderBy = "" Then
			If LinkType.SqlOrderBy <> "" Then
				sOrderBy = LinkType.SqlOrderBy
				LinkType.SessionOrderBy = sOrderBy
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
				LinkType.SessionOrderBy = sOrderBy
				LinkType.LinkTypeCD.Sort = ""
				LinkType.LinkTypeDesc.Sort = ""
				LinkType.LinkTypeTarget.Sort = ""
			End If

			' Reset start position
			lStartRec = 1
			LinkType.StartRecordNumber = lStartRec
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
		ListOptions.Add("detail_Link")
		ListOptions.GetItem("detail_Link").CssStyle = "white-space: nowrap;"
		ListOptions.GetItem("detail_Link").Visible = True
		ListOptions.GetItem("detail_Link").OnLeft = True
		ListOptions_Load()
		If LinkType.Export <> "" Or LinkType.CurrentAction = "gridadd" Or LinkType.CurrentAction = "gridedit" Then
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
			oListOpt = ListOptions.GetItem("detail_Link")
			oListOpt.Body = "<img src=""images/detail.gif"" alt=""" & ew_HtmlEncode(Language.Phrase("DetailLink")) & """ title=""" & ew_HtmlEncode(Language.Phrase("DetailLink")) & """ width=""16"" height=""16"" border=""0"" />" & Language.TablePhrase("Link", "TblCaption")
			oListOpt.Body = "<a href=""link_list.aspx?" & EW_TABLE_SHOW_MASTER & "=LinkType&LinkTypeCD=" & HttpContext.Current.Server.UrlEncode(Convert.ToString(LinkType.LinkTypeCD.CurrentValue)) & """>" & oListOpt.Body & "</a>"
		End If
		RenderListOptionsExt()
		ListOptions_Rendered()
	End Sub

	Sub RenderListOptionsExt()
		Dim sHyperLinkParm As String, oListOpt As cListOption
		sSqlWrk = "[LinkTypeCD]='" & ew_AdjustSql(LinkType.LinkTypeCD.CurrentValue) & "'"
		sSqlWrk = cTEA.Encrypt(sSqlWrk, EW_RANDOM_KEY)
		sSqlWrk = sSqlWrk.Replace("'", "\'")
		sHyperLinkParm = " href=""link_list.aspx?" & EW_TABLE_SHOW_MASTER & "=LinkType&LinkTypeCD=" & HttpContext.Current.Server.URLEncode(Convert.ToString(LinkType.LinkTypeCD.CurrentValue)) & """"
		oListOpt = ListOptions.GetItem("detail_Link")
		oListOpt.Body = Language.TablePhrase("Link", "TblCaption")
		oListOpt.Body &= "<img src=""images/detail.gif"" alt=""" & ew_HtmlEncode(Language.Phrase("DetailLink")) & """ title=""" & ew_HtmlEncode(Language.Phrase("DetailLink")) & """ width=""16"" height=""16"" border=""0"" />"
		oListOpt.Body = "<a" & sHyperLinkParm & ">" & oListOpt.Body & "</a>"
		sHyperLinkParm = " href=""javascript:void(0);"" name=""ew_LinkType_Link_DetailLink%i"" id=""ew_LinkType_Link_DetailLink%i"" onclick=""ew_AjaxShowDetails2(event, this, 'link_preview.aspx?f=%s')"""
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
				LinkType.StartRecordNumber = lStartRec
			ElseIf ew_Get(EW_TABLE_PAGE_NO) <> "" AndAlso IsNumeric(ew_Get(EW_TABLE_PAGE_NO)) Then
				nPageNo = ew_ConvertToInt(ew_Get(EW_TABLE_PAGE_NO))
				lStartRec = (nPageNo-1)*lDisplayRecs+1
				If lStartRec <= 0 Then
					lStartRec = 1
				ElseIf lStartRec >= ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 Then
					lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1
				End If
				LinkType.StartRecordNumber = lStartRec
			End If
		End If
		lStartRec = LinkType.StartRecordNumber

		' Check if correct start record counter
		If lStartRec <= 0 Then ' Avoid invalid start record counter
			lStartRec = 1 ' Reset start record counter
			LinkType.StartRecordNumber = lStartRec
		ElseIf lStartRec > lTotalRecs Then ' Avoid starting record > total records
			lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to last page first record
			LinkType.StartRecordNumber = lStartRec
		ElseIf (lStartRec-1) Mod lDisplayRecs <> 0 Then
			lStartRec = ((lStartRec-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to page boundary
			LinkType.StartRecordNumber = lStartRec
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
		LinkType.LinkTypeCD.AdvancedSearch.SearchValue = ew_Get("x_LinkTypeCD")
    	LinkType.LinkTypeCD.AdvancedSearch.SearchOperator = ew_Get("z_LinkTypeCD")
		LinkType.LinkTypeDesc.AdvancedSearch.SearchValue = ew_Get("x_LinkTypeDesc")
    	LinkType.LinkTypeDesc.AdvancedSearch.SearchOperator = ew_Get("z_LinkTypeDesc")
		LinkType.LinkTypeComment.AdvancedSearch.SearchValue = ew_Get("x_LinkTypeComment")
    	LinkType.LinkTypeComment.AdvancedSearch.SearchOperator = ew_Get("z_LinkTypeComment")
		LinkType.LinkTypeTarget.AdvancedSearch.SearchValue = ew_Get("x_LinkTypeTarget")
    	LinkType.LinkTypeTarget.AdvancedSearch.SearchOperator = ew_Get("z_LinkTypeTarget")
	End Sub

	'
	' Load recordset
	'
	Function LoadRecordset() As OleDbDataReader

		' Recordset Selecting event
		LinkType.Recordset_Selecting(LinkType.CurrentFilter)

		' Load list page SQL
		Dim sSql As String = LinkType.ListSQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then DebugMsg = sSql

		' Count
		lTotalRecs = -1		
		Try
			Dim sCntSql As String = LinkType.SelectCountSQL

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
		LinkType.Recordset_Selected(Rs)
		Return Rs
	End Function

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = LinkType.KeyFilter

		' Row Selecting event
		LinkType.Row_Selecting(sFilter)

		' Load SQL based on filter
		LinkType.CurrentFilter = sFilter
		Dim sSql As String = LinkType.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then DebugMsg = sSql
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				LinkType.Row_Selected(RsRow)
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
		LinkType.LinkTypeCD.DbValue = RsRow("LinkTypeCD")
		LinkType.LinkTypeDesc.DbValue = RsRow("LinkTypeDesc")
		LinkType.LinkTypeComment.DbValue = RsRow("LinkTypeComment")
		LinkType.LinkTypeTarget.DbValue = RsRow("LinkTypeTarget")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Initialize urls
		ViewUrl = LinkType.ViewUrl
		EditUrl = LinkType.EditUrl
		InlineEditUrl = LinkType.InlineEditUrl
		CopyUrl = LinkType.CopyUrl
		InlineCopyUrl = LinkType.InlineCopyUrl
		DeleteUrl = LinkType.DeleteUrl

		' Row Rendering event
		LinkType.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' LinkTypeCD

		LinkType.LinkTypeCD.CellCssStyle = ""
		LinkType.LinkTypeCD.CellCssClass = ""
		LinkType.LinkTypeCD.CellAttrs.Clear(): LinkType.LinkTypeCD.ViewAttrs.Clear(): LinkType.LinkTypeCD.EditAttrs.Clear()

		' LinkTypeDesc
		LinkType.LinkTypeDesc.CellCssStyle = ""
		LinkType.LinkTypeDesc.CellCssClass = ""
		LinkType.LinkTypeDesc.CellAttrs.Clear(): LinkType.LinkTypeDesc.ViewAttrs.Clear(): LinkType.LinkTypeDesc.EditAttrs.Clear()

		' LinkTypeTarget
		LinkType.LinkTypeTarget.CellCssStyle = ""
		LinkType.LinkTypeTarget.CellCssClass = ""
		LinkType.LinkTypeTarget.CellAttrs.Clear(): LinkType.LinkTypeTarget.ViewAttrs.Clear(): LinkType.LinkTypeTarget.EditAttrs.Clear()

		'
		'  View  Row
		'

		If LinkType.RowType = EW_ROWTYPE_VIEW Then ' View row

			' LinkTypeCD
			LinkType.LinkTypeCD.ViewValue = LinkType.LinkTypeCD.CurrentValue
			LinkType.LinkTypeCD.CssStyle = ""
			LinkType.LinkTypeCD.CssClass = ""
			LinkType.LinkTypeCD.ViewCustomAttributes = ""

			' LinkTypeDesc
			LinkType.LinkTypeDesc.ViewValue = LinkType.LinkTypeDesc.CurrentValue
			LinkType.LinkTypeDesc.CssStyle = ""
			LinkType.LinkTypeDesc.CssClass = ""
			LinkType.LinkTypeDesc.ViewCustomAttributes = ""

			' LinkTypeTarget
			LinkType.LinkTypeTarget.ViewValue = LinkType.LinkTypeTarget.CurrentValue
			LinkType.LinkTypeTarget.CssStyle = ""
			LinkType.LinkTypeTarget.CssClass = ""
			LinkType.LinkTypeTarget.ViewCustomAttributes = ""

			' View refer script
			' LinkTypeCD

			LinkType.LinkTypeCD.HrefValue = ""
			LinkType.LinkTypeCD.TooltipValue = ""

			' LinkTypeDesc
			LinkType.LinkTypeDesc.HrefValue = ""
			LinkType.LinkTypeDesc.TooltipValue = ""

			' LinkTypeTarget
			LinkType.LinkTypeTarget.HrefValue = ""
			LinkType.LinkTypeTarget.TooltipValue = ""
		End If

		' Row Rendered event
		If LinkType.RowType <> EW_ROWTYPE_AGGREGATEINIT Then
			LinkType.Row_Rendered()
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
		LinkType.LinkTypeCD.AdvancedSearch.SearchValue = LinkType.GetAdvancedSearch("x_LinkTypeCD")
		LinkType.LinkTypeDesc.AdvancedSearch.SearchValue = LinkType.GetAdvancedSearch("x_LinkTypeDesc")
		LinkType.LinkTypeComment.AdvancedSearch.SearchValue = LinkType.GetAdvancedSearch("x_LinkTypeComment")
		LinkType.LinkTypeTarget.AdvancedSearch.SearchValue = LinkType.GetAdvancedSearch("x_LinkTypeTarget")
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
		If LinkType.ExportAll Then
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
		If LinkType.Export = "xml" Then
			oXmlDoc = New cXMLDocument()
		Else
			sOutputStr = ew_ExportHeader(LinkType.Export)

			' Horizontal format, write header
			If sExportStyle <> "v" OrElse LinkType.Export = "csv" Then
				sExportStr = ""
				ew_ExportAddValue(sExportStr, LinkType.LinkTypeCD.ExportCaption, LinkType.Export, IIf(EW_EXPORT_CSS_STYLES, LinkType.LinkTypeCD.CellStyles, ""))
				ew_ExportAddValue(sExportStr, LinkType.LinkTypeDesc.ExportCaption, LinkType.Export, IIf(EW_EXPORT_CSS_STYLES, LinkType.LinkTypeDesc.CellStyles, ""))
				ew_ExportAddValue(sExportStr, LinkType.LinkTypeTarget.ExportCaption, LinkType.Export, IIf(EW_EXPORT_CSS_STYLES, LinkType.LinkTypeTarget.CellStyles, ""))
				sOutputStr &= ew_ExportLine(sExportStr, LinkType.Export, IIf(EW_EXPORT_CSS_STYLES, LinkType.RowStyles, ""))
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
				LinkType.CssClass = ""
				LinkType.CssStyle = ""
				LinkType.RowType = EW_ROWTYPE_VIEW ' Render view
				RenderRow()
				If LinkType.Export = "xml" Then
					oXmlDoc.AddRow()
					oXmlDoc.AddField("LinkTypeCD", LinkType.LinkTypeCD.ExportValue(LinkType.Export, LinkType.ExportOriginalValue)) ' LinkTypeCD
					oXmlDoc.AddField("LinkTypeDesc", LinkType.LinkTypeDesc.ExportValue(LinkType.Export, LinkType.ExportOriginalValue)) ' LinkTypeDesc
					oXmlDoc.AddField("LinkTypeTarget", LinkType.LinkTypeTarget.ExportValue(LinkType.Export, LinkType.ExportOriginalValue)) ' LinkTypeTarget
				Else

					' Vertical format
					If sExportStyle = "v" AndAlso LinkType.Export <> "csv" Then
						sOutputStr &= ew_ExportField(LinkType.LinkTypeCD.ExportCaption, LinkType.LinkTypeCD.ExportValue(LinkType.Export, LinkType.ExportOriginalValue), LinkType.Export, IIf(EW_EXPORT_CSS_STYLES, LinkType.LinkTypeCD.CellStyles, "")) ' LinkTypeCD
						sOutputStr &= ew_ExportField(LinkType.LinkTypeDesc.ExportCaption, LinkType.LinkTypeDesc.ExportValue(LinkType.Export, LinkType.ExportOriginalValue), LinkType.Export, IIf(EW_EXPORT_CSS_STYLES, LinkType.LinkTypeDesc.CellStyles, "")) ' LinkTypeDesc
						sOutputStr &= ew_ExportField(LinkType.LinkTypeTarget.ExportCaption, LinkType.LinkTypeTarget.ExportValue(LinkType.Export, LinkType.ExportOriginalValue), LinkType.Export, IIf(EW_EXPORT_CSS_STYLES, LinkType.LinkTypeTarget.CellStyles, "")) ' LinkTypeTarget

					' Horizontal format
					Else
						sExportStr = ""
						ew_ExportAddValue(sExportStr, LinkType.LinkTypeCD.ExportValue(LinkType.Export, LinkType.ExportOriginalValue), LinkType.Export, IIf(EW_EXPORT_CSS_STYLES, LinkType.LinkTypeCD.CellStyles, ""))
						ew_ExportAddValue(sExportStr, LinkType.LinkTypeDesc.ExportValue(LinkType.Export, LinkType.ExportOriginalValue), LinkType.Export, IIf(EW_EXPORT_CSS_STYLES, LinkType.LinkTypeDesc.CellStyles, ""))
						ew_ExportAddValue(sExportStr, LinkType.LinkTypeTarget.ExportValue(LinkType.Export, LinkType.ExportOriginalValue), LinkType.Export, IIf(EW_EXPORT_CSS_STYLES, LinkType.LinkTypeTarget.CellStyles, ""))
						sOutputStr &= ew_ExportLine(sExportStr, LinkType.Export, IIf(EW_EXPORT_CSS_STYLES, LinkType.RowStyles, ""))
					End If
				End If
			End If
		Loop

		' Close recordset
		Rs.Close()
		Rs.Dispose()
		If LinkType.Export = "xml" Then
			oXmlDoc.Output
		Else
			sOutputStr &= ew_ExportFooter(LinkType.Export)
			ew_Write(sOutputStr)
		End If
	End Sub

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "LinkType"
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
		LinkType_list = New cLinkType_list(Me)		
		LinkType_list.Page_Init()		

		' Set buffer/cache option
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

		' Page main processing
		LinkType_list.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If LinkType_list IsNot Nothing Then LinkType_list.Dispose()
	End Sub
End Class
