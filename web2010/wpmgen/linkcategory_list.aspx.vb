Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class linkcategory_list
	Inherits AspNetMaker8_wpmWebsite

	' Page object
	Public LinkCategory_list As cLinkCategory_list

	'
	' Page Class
	'
	Class cLinkCategory_list
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
				If LinkCategory.UseTokenInUrl Then Url = Url & "t=" & LinkCategory.TableVar & "&" ' Add page token
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
			If LinkCategory.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (LinkCategory.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (LinkCategory.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' ASP.NET page object
		Public ReadOnly Property AspNetPage() As linkcategory_list
			Get
				Return CType(m_ParentPage, linkcategory_list)
			End Get
		End Property

		' LinkCategory
		Public Property LinkCategory() As cLinkCategory
			Get				
				Return ParentPage.LinkCategory
			End Get
			Set(ByVal v As cLinkCategory)
				ParentPage.LinkCategory = v	
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
			m_PageObjName = "LinkCategory_list"
			m_PageObjTypeName = "cLinkCategory_list"			

			' Initialize language object
			Language = New cLanguage(Me)

			' Table Name
			m_TableName = "LinkCategory"

			' Initialize table object
			LinkCategory = New cLinkCategory(Me)

			' Initialize URLs
			ExportPrintUrl = PageUrl & "export=print"
			ExportExcelUrl = PageUrl & "export=excel"
			ExportWordUrl = PageUrl & "export=word"
			ExportHtmlUrl = PageUrl & "export=html"
			ExportXmlUrl = PageUrl & "export=xml"
			ExportCsvUrl = PageUrl & "export=csv"
			AddUrl = LinkCategory.AddUrl
			InlineAddUrl = PageUrl & "a=add"
			GridAddUrl = PageUrl & "a=gridadd"
			GridEditUrl = PageUrl & "a=gridedit"
			MultiDeleteUrl = "linkcategory_delete.aspx"
			MultiUpdateUrl = "linkcategory_update.aspx"

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
				LinkCategory.Export = ew_Get("export")
			ElseIf ew_Post("exporttype") <> "" Then
				LinkCategory.Export = ew_Post("exporttype")
			Else
				LinkCategory.ExportReturnUrl = ew_CurrentUrl()
			End If
			ParentPage.gsExport = LinkCategory.Export ' Get export parameter, used in header
			ParentPage.gsExportFile = LinkCategory.TableVar ' Get export file, used in header
			If LinkCategory.Export = "excel" Then
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
			LinkCategory.Dispose()
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
			Call LinkCategory.Recordset_SearchValidated()

			' Set Up Sorting Order
			SetUpSortOrder()

			' Get search criteria for advanced search
			If ParentPage.gsSearchError = "" Then
				sSrchAdvanced = AdvancedSearchWhere()
			End If
		End If

		' Restore display records
		If (LinkCategory.RecordsPerPage = -1 OrElse LinkCategory.RecordsPerPage > 0) Then
			lDisplayRecs = LinkCategory.RecordsPerPage ' Restore from Session
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
		LinkCategory.Recordset_Searching(sSrchWhere)

		' Save search criteria
		If sSrchWhere <> "" Then
			If sSrchAdvanced = "" Then ResetAdvancedSearchParms()
			LinkCategory.SearchWhere = sSrchWhere ' Save to Session
			If RestoreSearch Then
				lStartRec = 1 ' Reset start record counter
				LinkCategory.StartRecordNumber = lStartRec
			End If
		Else
			sSrchWhere = LinkCategory.SearchWhere
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
		LinkCategory.SessionWhere = sFilter
		LinkCategory.CurrentFilter = ""

		' Export Data only
		If LinkCategory.Export = "html" OrElse LinkCategory.Export = "csv" OrElse LinkCategory.Export = "word" OrElse LinkCategory.Export = "excel" OrElse LinkCategory.Export = "xml" Then
			ExportData()
			Page_Terminate("") ' Clean up
			ew_End() ' Terminate response
		ElseIf LinkCategory.Export = "email" Then
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
			LinkCategory.RecordsPerPage = lDisplayRecs ' Save to Session

			' Reset start position
			lStartRec = 1
			LinkCategory.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	' Return Advanced Search WHERE based on QueryString parameters
	'
	Function AdvancedSearchWhere() As String		
		Dim sWhere As String = ""
		BuildSearchSql(sWhere, LinkCategory.ID, False) ' ID
		BuildSearchSql(sWhere, LinkCategory.Title, False) ' Title
		BuildSearchSql(sWhere, LinkCategory.Description, False) ' Description
		BuildSearchSql(sWhere, LinkCategory.ParentID, False) ' ParentID
		BuildSearchSql(sWhere, LinkCategory.zPageID, False) ' PageID

		' Set up search parm
		If sWhere <> "" Then
			SetSearchParm(LinkCategory.ID) ' ID
			SetSearchParm(LinkCategory.Title) ' Title
			SetSearchParm(LinkCategory.Description) ' Description
			SetSearchParm(LinkCategory.ParentID) ' ParentID
			SetSearchParm(LinkCategory.zPageID) ' PageID
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
		LinkCategory.SetAdvancedSearch("x_" & FldParm, Fld.AdvancedSearch.SearchValue)
		LinkCategory.SetAdvancedSearch("z_" & FldParm, Fld.AdvancedSearch.SearchOperator)
		LinkCategory.SetAdvancedSearch("v_" & FldParm, Fld.AdvancedSearch.SearchCondition)
		LinkCategory.SetAdvancedSearch("y_" & FldParm, Fld.AdvancedSearch.SearchValue2)
		LinkCategory.SetAdvancedSearch("w_" & FldParm, Fld.AdvancedSearch.SearchOperator2)
	End Sub

	'
	' Get search parm
	'
	Sub GetSearchParm(ByRef Fld As Object)
		Dim FldParm As String = Fld.FldVar.Substring(2)
		Fld.AdvancedSearch.SearchValue = LinkCategory.GetAdvancedSearch("x_" & FldParm)
		Fld.AdvancedSearch.SearchOperator = LinkCategory.GetAdvancedSearch("z_" & FldParm)
		Fld.AdvancedSearch.SearchCondition = LinkCategory.GetAdvancedSearch("v_" & FldParm)
		Fld.AdvancedSearch.SearchValue2 = LinkCategory.GetAdvancedSearch("y_" & FldParm)
		Fld.AdvancedSearch.SearchOperator2 = LinkCategory.GetAdvancedSearch("w_" & FldParm)
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
		LinkCategory.SearchWhere = sSrchWhere

		' Clear advanced search parameters
		ResetAdvancedSearchParms()
	End Sub

	'
	' Clear all advanced search parameters
	'
	Sub ResetAdvancedSearchParms()
		LinkCategory.SetAdvancedSearch("x_ID", "")
		LinkCategory.SetAdvancedSearch("x_Title", "")
		LinkCategory.SetAdvancedSearch("x_Description", "")
		LinkCategory.SetAdvancedSearch("x_ParentID", "")
		LinkCategory.SetAdvancedSearch("x_zPageID", "")
	End Sub

	'
	' Restore all search parameters
	'
	Sub RestoreSearchParms()
		Dim bRestore As Boolean = True
		If ew_Get("x_ID") <> "" Then bRestore = False
		If ew_Get("x_Title") <> "" Then bRestore = False
		If ew_Get("x_Description") <> "" Then bRestore = False
		If ew_Get("x_ParentID") <> "" Then bRestore = False
		If ew_Get("x_zPageID") <> "" Then bRestore = False
		RestoreSearch = bRestore
		If bRestore Then

			' Restore advanced search values
			Call GetSearchParm(LinkCategory.ID)
			Call GetSearchParm(LinkCategory.Title)
			Call GetSearchParm(LinkCategory.Description)
			Call GetSearchParm(LinkCategory.ParentID)
			Call GetSearchParm(LinkCategory.zPageID)
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
			LinkCategory.CurrentOrder = ew_Get("order")
			LinkCategory.CurrentOrderType = ew_Get("ordertype")
			LinkCategory.UpdateSort(LinkCategory.ID) ' ID
			LinkCategory.UpdateSort(LinkCategory.Title) ' Title
			LinkCategory.UpdateSort(LinkCategory.ParentID) ' ParentID
			LinkCategory.UpdateSort(LinkCategory.zPageID) ' PageID
			LinkCategory.StartRecordNumber = 1 ' Reset start position
		End If
	End Sub

	'
	' Load Sort Order parameters
	'
	Sub LoadSortOrder()
		Dim sOrderBy As String
		sOrderBy = LinkCategory.SessionOrderBy ' Get order by from Session
		If sOrderBy = "" Then
			If LinkCategory.SqlOrderBy <> "" Then
				sOrderBy = LinkCategory.SqlOrderBy
				LinkCategory.SessionOrderBy = sOrderBy
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
				LinkCategory.SessionOrderBy = sOrderBy
				LinkCategory.ID.Sort = ""
				LinkCategory.Title.Sort = ""
				LinkCategory.ParentID.Sort = ""
				LinkCategory.zPageID.Sort = ""
			End If

			' Reset start position
			lStartRec = 1
			LinkCategory.StartRecordNumber = lStartRec
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
		If LinkCategory.Export <> "" Or LinkCategory.CurrentAction = "gridadd" Or LinkCategory.CurrentAction = "gridedit" Then
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
			oListOpt.Body = "<a href=""link_list.aspx?" & EW_TABLE_SHOW_MASTER & "=LinkCategory&ID=" & HttpContext.Current.Server.UrlEncode(Convert.ToString(LinkCategory.ID.CurrentValue)) & """>" & oListOpt.Body & "</a>"
		End If
		RenderListOptionsExt()
		ListOptions_Rendered()
	End Sub

	Sub RenderListOptionsExt()
		Dim sHyperLinkParm As String, oListOpt As cListOption
		sSqlWrk = "[CategoryID]=" & ew_AdjustSql(LinkCategory.ID.CurrentValue) & ""
		sSqlWrk = cTEA.Encrypt(sSqlWrk, EW_RANDOM_KEY)
		sSqlWrk = sSqlWrk.Replace("'", "\'")
		sHyperLinkParm = " href=""link_list.aspx?" & EW_TABLE_SHOW_MASTER & "=LinkCategory&ID=" & HttpContext.Current.Server.URLEncode(Convert.ToString(LinkCategory.ID.CurrentValue)) & """"
		oListOpt = ListOptions.GetItem("detail_Link")
		oListOpt.Body = Language.TablePhrase("Link", "TblCaption")
		oListOpt.Body &= "<img src=""images/detail.gif"" alt=""" & ew_HtmlEncode(Language.Phrase("DetailLink")) & """ title=""" & ew_HtmlEncode(Language.Phrase("DetailLink")) & """ width=""16"" height=""16"" border=""0"" />"
		oListOpt.Body = "<a" & sHyperLinkParm & ">" & oListOpt.Body & "</a>"
		sHyperLinkParm = " href=""javascript:void(0);"" name=""ew_LinkCategory_Link_DetailLink%i"" id=""ew_LinkCategory_Link_DetailLink%i"" onclick=""ew_AjaxShowDetails2(event, this, 'link_preview.aspx?f=%s')"""
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
				LinkCategory.StartRecordNumber = lStartRec
			ElseIf ew_Get(EW_TABLE_PAGE_NO) <> "" AndAlso IsNumeric(ew_Get(EW_TABLE_PAGE_NO)) Then
				nPageNo = ew_ConvertToInt(ew_Get(EW_TABLE_PAGE_NO))
				lStartRec = (nPageNo-1)*lDisplayRecs+1
				If lStartRec <= 0 Then
					lStartRec = 1
				ElseIf lStartRec >= ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 Then
					lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1
				End If
				LinkCategory.StartRecordNumber = lStartRec
			End If
		End If
		lStartRec = LinkCategory.StartRecordNumber

		' Check if correct start record counter
		If lStartRec <= 0 Then ' Avoid invalid start record counter
			lStartRec = 1 ' Reset start record counter
			LinkCategory.StartRecordNumber = lStartRec
		ElseIf lStartRec > lTotalRecs Then ' Avoid starting record > total records
			lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to last page first record
			LinkCategory.StartRecordNumber = lStartRec
		ElseIf (lStartRec-1) Mod lDisplayRecs <> 0 Then
			lStartRec = ((lStartRec-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to page boundary
			LinkCategory.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	' Load default values
	'
	Sub LoadDefaultValues()
		LinkCategory.ParentID.CurrentValue = 0
		LinkCategory.zPageID.CurrentValue = 0
	End Sub

	'
	'  Load search values for validation
	'
	Sub LoadSearchValues()
		LinkCategory.ID.AdvancedSearch.SearchValue = ew_Get("x_ID")
    	LinkCategory.ID.AdvancedSearch.SearchOperator = ew_Get("z_ID")
		LinkCategory.Title.AdvancedSearch.SearchValue = ew_Get("x_Title")
    	LinkCategory.Title.AdvancedSearch.SearchOperator = ew_Get("z_Title")
		LinkCategory.Description.AdvancedSearch.SearchValue = ew_Get("x_Description")
    	LinkCategory.Description.AdvancedSearch.SearchOperator = ew_Get("z_Description")
		LinkCategory.ParentID.AdvancedSearch.SearchValue = ew_Get("x_ParentID")
    	LinkCategory.ParentID.AdvancedSearch.SearchOperator = ew_Get("z_ParentID")
		LinkCategory.zPageID.AdvancedSearch.SearchValue = ew_Get("x_zPageID")
    	LinkCategory.zPageID.AdvancedSearch.SearchOperator = ew_Get("z_zPageID")
	End Sub

	'
	' Load recordset
	'
	Function LoadRecordset() As OleDbDataReader

		' Recordset Selecting event
		LinkCategory.Recordset_Selecting(LinkCategory.CurrentFilter)

		' Load list page SQL
		Dim sSql As String = LinkCategory.ListSQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then DebugMsg = sSql

		' Count
		lTotalRecs = -1		
		Try
			Dim sCntSql As String = LinkCategory.SelectCountSQL

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
		LinkCategory.Recordset_Selected(Rs)
		Return Rs
	End Function

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = LinkCategory.KeyFilter

		' Row Selecting event
		LinkCategory.Row_Selecting(sFilter)

		' Load SQL based on filter
		LinkCategory.CurrentFilter = sFilter
		Dim sSql As String = LinkCategory.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then DebugMsg = sSql
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				LinkCategory.Row_Selected(RsRow)
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
		LinkCategory.ID.DbValue = RsRow("ID")
		LinkCategory.Title.DbValue = RsRow("Title")
		LinkCategory.Description.DbValue = RsRow("Description")
		LinkCategory.ParentID.DbValue = RsRow("ParentID")
		LinkCategory.zPageID.DbValue = RsRow("PageID")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Initialize urls
		ViewUrl = LinkCategory.ViewUrl
		EditUrl = LinkCategory.EditUrl
		InlineEditUrl = LinkCategory.InlineEditUrl
		CopyUrl = LinkCategory.CopyUrl
		InlineCopyUrl = LinkCategory.InlineCopyUrl
		DeleteUrl = LinkCategory.DeleteUrl

		' Row Rendering event
		LinkCategory.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' ID

		LinkCategory.ID.CellCssStyle = ""
		LinkCategory.ID.CellCssClass = ""
		LinkCategory.ID.CellAttrs.Clear(): LinkCategory.ID.ViewAttrs.Clear(): LinkCategory.ID.EditAttrs.Clear()

		' Title
		LinkCategory.Title.CellCssStyle = ""
		LinkCategory.Title.CellCssClass = ""
		LinkCategory.Title.CellAttrs.Clear(): LinkCategory.Title.ViewAttrs.Clear(): LinkCategory.Title.EditAttrs.Clear()

		' ParentID
		LinkCategory.ParentID.CellCssStyle = ""
		LinkCategory.ParentID.CellCssClass = ""
		LinkCategory.ParentID.CellAttrs.Clear(): LinkCategory.ParentID.ViewAttrs.Clear(): LinkCategory.ParentID.EditAttrs.Clear()

		' PageID
		LinkCategory.zPageID.CellCssStyle = ""
		LinkCategory.zPageID.CellCssClass = ""
		LinkCategory.zPageID.CellAttrs.Clear(): LinkCategory.zPageID.ViewAttrs.Clear(): LinkCategory.zPageID.EditAttrs.Clear()

		'
		'  View  Row
		'

		If LinkCategory.RowType = EW_ROWTYPE_VIEW Then ' View row

			' ID
			LinkCategory.ID.ViewValue = LinkCategory.ID.CurrentValue
			LinkCategory.ID.CssStyle = ""
			LinkCategory.ID.CssClass = ""
			LinkCategory.ID.ViewCustomAttributes = ""

			' Title
			LinkCategory.Title.ViewValue = LinkCategory.Title.CurrentValue
			LinkCategory.Title.CssStyle = ""
			LinkCategory.Title.CssClass = ""
			LinkCategory.Title.ViewCustomAttributes = ""

			' ParentID
			LinkCategory.ParentID.ViewValue = LinkCategory.ParentID.CurrentValue
			LinkCategory.ParentID.CssStyle = ""
			LinkCategory.ParentID.CssClass = ""
			LinkCategory.ParentID.ViewCustomAttributes = ""

			' PageID
			LinkCategory.zPageID.ViewValue = LinkCategory.zPageID.CurrentValue
			LinkCategory.zPageID.CssStyle = ""
			LinkCategory.zPageID.CssClass = ""
			LinkCategory.zPageID.ViewCustomAttributes = ""

			' View refer script
			' ID

			LinkCategory.ID.HrefValue = ""
			LinkCategory.ID.TooltipValue = ""

			' Title
			LinkCategory.Title.HrefValue = ""
			LinkCategory.Title.TooltipValue = ""

			' ParentID
			LinkCategory.ParentID.HrefValue = ""
			LinkCategory.ParentID.TooltipValue = ""

			' PageID
			LinkCategory.zPageID.HrefValue = ""
			LinkCategory.zPageID.TooltipValue = ""
		End If

		' Row Rendered event
		If LinkCategory.RowType <> EW_ROWTYPE_AGGREGATEINIT Then
			LinkCategory.Row_Rendered()
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
		LinkCategory.ID.AdvancedSearch.SearchValue = LinkCategory.GetAdvancedSearch("x_ID")
		LinkCategory.Title.AdvancedSearch.SearchValue = LinkCategory.GetAdvancedSearch("x_Title")
		LinkCategory.Description.AdvancedSearch.SearchValue = LinkCategory.GetAdvancedSearch("x_Description")
		LinkCategory.ParentID.AdvancedSearch.SearchValue = LinkCategory.GetAdvancedSearch("x_ParentID")
		LinkCategory.zPageID.AdvancedSearch.SearchValue = LinkCategory.GetAdvancedSearch("x_zPageID")
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
		If LinkCategory.ExportAll Then
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
		If LinkCategory.Export = "xml" Then
			oXmlDoc = New cXMLDocument()
		Else
			sOutputStr = ew_ExportHeader(LinkCategory.Export)

			' Horizontal format, write header
			If sExportStyle <> "v" OrElse LinkCategory.Export = "csv" Then
				sExportStr = ""
				ew_ExportAddValue(sExportStr, LinkCategory.ID.ExportCaption, LinkCategory.Export, IIf(EW_EXPORT_CSS_STYLES, LinkCategory.ID.CellStyles, ""))
				ew_ExportAddValue(sExportStr, LinkCategory.Title.ExportCaption, LinkCategory.Export, IIf(EW_EXPORT_CSS_STYLES, LinkCategory.Title.CellStyles, ""))
				ew_ExportAddValue(sExportStr, LinkCategory.ParentID.ExportCaption, LinkCategory.Export, IIf(EW_EXPORT_CSS_STYLES, LinkCategory.ParentID.CellStyles, ""))
				ew_ExportAddValue(sExportStr, LinkCategory.zPageID.ExportCaption, LinkCategory.Export, IIf(EW_EXPORT_CSS_STYLES, LinkCategory.zPageID.CellStyles, ""))
				sOutputStr &= ew_ExportLine(sExportStr, LinkCategory.Export, IIf(EW_EXPORT_CSS_STYLES, LinkCategory.RowStyles, ""))
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
				LinkCategory.CssClass = ""
				LinkCategory.CssStyle = ""
				LinkCategory.RowType = EW_ROWTYPE_VIEW ' Render view
				RenderRow()
				If LinkCategory.Export = "xml" Then
					oXmlDoc.AddRow()
					oXmlDoc.AddField("ID", LinkCategory.ID.ExportValue(LinkCategory.Export, LinkCategory.ExportOriginalValue)) ' ID
					oXmlDoc.AddField("Title", LinkCategory.Title.ExportValue(LinkCategory.Export, LinkCategory.ExportOriginalValue)) ' Title
					oXmlDoc.AddField("ParentID", LinkCategory.ParentID.ExportValue(LinkCategory.Export, LinkCategory.ExportOriginalValue)) ' ParentID
					oXmlDoc.AddField("zPageID", LinkCategory.zPageID.ExportValue(LinkCategory.Export, LinkCategory.ExportOriginalValue)) ' PageID
				Else

					' Vertical format
					If sExportStyle = "v" AndAlso LinkCategory.Export <> "csv" Then
						sOutputStr &= ew_ExportField(LinkCategory.ID.ExportCaption, LinkCategory.ID.ExportValue(LinkCategory.Export, LinkCategory.ExportOriginalValue), LinkCategory.Export, IIf(EW_EXPORT_CSS_STYLES, LinkCategory.ID.CellStyles, "")) ' ID
						sOutputStr &= ew_ExportField(LinkCategory.Title.ExportCaption, LinkCategory.Title.ExportValue(LinkCategory.Export, LinkCategory.ExportOriginalValue), LinkCategory.Export, IIf(EW_EXPORT_CSS_STYLES, LinkCategory.Title.CellStyles, "")) ' Title
						sOutputStr &= ew_ExportField(LinkCategory.ParentID.ExportCaption, LinkCategory.ParentID.ExportValue(LinkCategory.Export, LinkCategory.ExportOriginalValue), LinkCategory.Export, IIf(EW_EXPORT_CSS_STYLES, LinkCategory.ParentID.CellStyles, "")) ' ParentID
						sOutputStr &= ew_ExportField(LinkCategory.zPageID.ExportCaption, LinkCategory.zPageID.ExportValue(LinkCategory.Export, LinkCategory.ExportOriginalValue), LinkCategory.Export, IIf(EW_EXPORT_CSS_STYLES, LinkCategory.zPageID.CellStyles, "")) ' PageID

					' Horizontal format
					Else
						sExportStr = ""
						ew_ExportAddValue(sExportStr, LinkCategory.ID.ExportValue(LinkCategory.Export, LinkCategory.ExportOriginalValue), LinkCategory.Export, IIf(EW_EXPORT_CSS_STYLES, LinkCategory.ID.CellStyles, ""))
						ew_ExportAddValue(sExportStr, LinkCategory.Title.ExportValue(LinkCategory.Export, LinkCategory.ExportOriginalValue), LinkCategory.Export, IIf(EW_EXPORT_CSS_STYLES, LinkCategory.Title.CellStyles, ""))
						ew_ExportAddValue(sExportStr, LinkCategory.ParentID.ExportValue(LinkCategory.Export, LinkCategory.ExportOriginalValue), LinkCategory.Export, IIf(EW_EXPORT_CSS_STYLES, LinkCategory.ParentID.CellStyles, ""))
						ew_ExportAddValue(sExportStr, LinkCategory.zPageID.ExportValue(LinkCategory.Export, LinkCategory.ExportOriginalValue), LinkCategory.Export, IIf(EW_EXPORT_CSS_STYLES, LinkCategory.zPageID.CellStyles, ""))
						sOutputStr &= ew_ExportLine(sExportStr, LinkCategory.Export, IIf(EW_EXPORT_CSS_STYLES, LinkCategory.RowStyles, ""))
					End If
				End If
			End If
		Loop

		' Close recordset
		Rs.Close()
		Rs.Dispose()
		If LinkCategory.Export = "xml" Then
			oXmlDoc.Output
		Else
			sOutputStr &= ew_ExportFooter(LinkCategory.Export)
			ew_Write(sOutputStr)
		End If
	End Sub

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "LinkCategory"
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
		LinkCategory_list = New cLinkCategory_list(Me)		
		LinkCategory_list.Page_Init()		

		' Set buffer/cache option
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

		' Page main processing
		LinkCategory_list.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If LinkCategory_list IsNot Nothing Then LinkCategory_list.Dispose()
	End Sub
End Class
