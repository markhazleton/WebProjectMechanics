Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class contact_list
	Inherits AspNetMaker8_wpmWebsite

	' Page object
	Public Contact_list As cContact_list

	'
	' Page Class
	'
	Class cContact_list
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
				If Contact.UseTokenInUrl Then Url = Url & "t=" & Contact.TableVar & "&" ' Add page token
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
			If Contact.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (Contact.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (Contact.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' ASP.NET page object
		Public ReadOnly Property AspNetPage() As contact_list
			Get
				Return CType(m_ParentPage, contact_list)
			End Get
		End Property

		' Contact
		Public Property Contact() As cContact
			Get				
				Return ParentPage.Contact
			End Get
			Set(ByVal v As cContact)
				ParentPage.Contact = v	
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
			m_PageObjName = "Contact_list"
			m_PageObjTypeName = "cContact_list"			

			' Initialize language object
			Language = New cLanguage(Me)

			' Table Name
			m_TableName = "Contact"

			' Initialize table object
			Contact = New cContact(Me)

			' Initialize URLs
			ExportPrintUrl = PageUrl & "export=print"
			ExportExcelUrl = PageUrl & "export=excel"
			ExportWordUrl = PageUrl & "export=word"
			ExportHtmlUrl = PageUrl & "export=html"
			ExportXmlUrl = PageUrl & "export=xml"
			ExportCsvUrl = PageUrl & "export=csv"
			AddUrl = Contact.AddUrl
			InlineAddUrl = PageUrl & "a=add"
			GridAddUrl = PageUrl & "a=gridadd"
			GridEditUrl = PageUrl & "a=gridedit"
			MultiDeleteUrl = "contact_delete.aspx"
			MultiUpdateUrl = "contact_update.aspx"

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
				Contact.Export = ew_Get("export")
			ElseIf ew_Post("exporttype") <> "" Then
				Contact.Export = ew_Post("exporttype")
			Else
				Contact.ExportReturnUrl = ew_CurrentUrl()
			End If
			ParentPage.gsExport = Contact.Export ' Get export parameter, used in header
			ParentPage.gsExportFile = Contact.TableVar ' Get export file, used in header
			If Contact.Export = "excel" Then
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
			Contact.Dispose()
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
			Call Contact.Recordset_SearchValidated()

			' Set Up Sorting Order
			SetUpSortOrder()

			' Get search criteria for advanced search
			If ParentPage.gsSearchError = "" Then
				sSrchAdvanced = AdvancedSearchWhere()
			End If
		End If

		' Restore display records
		If (Contact.RecordsPerPage = -1 OrElse Contact.RecordsPerPage > 0) Then
			lDisplayRecs = Contact.RecordsPerPage ' Restore from Session
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
		Contact.Recordset_Searching(sSrchWhere)

		' Save search criteria
		If sSrchWhere <> "" Then
			If sSrchAdvanced = "" Then ResetAdvancedSearchParms()
			Contact.SearchWhere = sSrchWhere ' Save to Session
			If RestoreSearch Then
				lStartRec = 1 ' Reset start record counter
				Contact.StartRecordNumber = lStartRec
			End If
		Else
			sSrchWhere = Contact.SearchWhere
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
		Contact.SessionWhere = sFilter
		Contact.CurrentFilter = ""

		' Export Data only
		If Contact.Export = "html" OrElse Contact.Export = "csv" OrElse Contact.Export = "word" OrElse Contact.Export = "excel" OrElse Contact.Export = "xml" Then
			ExportData()
			Page_Terminate("") ' Clean up
			ew_End() ' Terminate response
		ElseIf Contact.Export = "email" Then
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
			Contact.RecordsPerPage = lDisplayRecs ' Save to Session

			' Reset start position
			lStartRec = 1
			Contact.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	' Return Advanced Search WHERE based on QueryString parameters
	'
	Function AdvancedSearchWhere() As String		
		Dim sWhere As String = ""
		BuildSearchSql(sWhere, Contact.LogonName, False) ' LogonName
		BuildSearchSql(sWhere, Contact.GroupID, False) ' GroupID
		BuildSearchSql(sWhere, Contact.CompanyID, False) ' CompanyID
		BuildSearchSql(sWhere, Contact.TemplatePrefix, False) ' TemplatePrefix

		' Set up search parm
		If sWhere <> "" Then
			SetSearchParm(Contact.LogonName) ' LogonName
			SetSearchParm(Contact.GroupID) ' GroupID
			SetSearchParm(Contact.CompanyID) ' CompanyID
			SetSearchParm(Contact.TemplatePrefix) ' TemplatePrefix
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
		Contact.SetAdvancedSearch("x_" & FldParm, Fld.AdvancedSearch.SearchValue)
		Contact.SetAdvancedSearch("z_" & FldParm, Fld.AdvancedSearch.SearchOperator)
		Contact.SetAdvancedSearch("v_" & FldParm, Fld.AdvancedSearch.SearchCondition)
		Contact.SetAdvancedSearch("y_" & FldParm, Fld.AdvancedSearch.SearchValue2)
		Contact.SetAdvancedSearch("w_" & FldParm, Fld.AdvancedSearch.SearchOperator2)
	End Sub

	'
	' Get search parm
	'
	Sub GetSearchParm(ByRef Fld As Object)
		Dim FldParm As String = Fld.FldVar.Substring(2)
		Fld.AdvancedSearch.SearchValue = Contact.GetAdvancedSearch("x_" & FldParm)
		Fld.AdvancedSearch.SearchOperator = Contact.GetAdvancedSearch("z_" & FldParm)
		Fld.AdvancedSearch.SearchCondition = Contact.GetAdvancedSearch("v_" & FldParm)
		Fld.AdvancedSearch.SearchValue2 = Contact.GetAdvancedSearch("y_" & FldParm)
		Fld.AdvancedSearch.SearchOperator2 = Contact.GetAdvancedSearch("w_" & FldParm)
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
		Contact.SearchWhere = sSrchWhere

		' Clear advanced search parameters
		ResetAdvancedSearchParms()
	End Sub

	'
	' Clear all advanced search parameters
	'
	Sub ResetAdvancedSearchParms()
		Contact.SetAdvancedSearch("x_LogonName", "")
		Contact.SetAdvancedSearch("x_GroupID", "")
		Contact.SetAdvancedSearch("x_CompanyID", "")
		Contact.SetAdvancedSearch("x_TemplatePrefix", "")
	End Sub

	'
	' Restore all search parameters
	'
	Sub RestoreSearchParms()
		Dim bRestore As Boolean = True
		If ew_Get("x_LogonName") <> "" Then bRestore = False
		If ew_Get("x_GroupID") <> "" Then bRestore = False
		If ew_Get("x_CompanyID") <> "" Then bRestore = False
		If ew_Get("x_TemplatePrefix") <> "" Then bRestore = False
		RestoreSearch = bRestore
		If bRestore Then

			' Restore advanced search values
			Call GetSearchParm(Contact.LogonName)
			Call GetSearchParm(Contact.GroupID)
			Call GetSearchParm(Contact.CompanyID)
			Call GetSearchParm(Contact.TemplatePrefix)
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
			Contact.CurrentOrder = ew_Get("order")
			Contact.CurrentOrderType = ew_Get("ordertype")
			Contact.UpdateSort(Contact.LogonName) ' LogonName
			Contact.UpdateSort(Contact.CompanyID) ' CompanyID
			Contact.UpdateSort(Contact.zEMail) ' EMail
			Contact.UpdateSort(Contact.PrimaryContact) ' PrimaryContact
			Contact.StartRecordNumber = 1 ' Reset start position
		End If
	End Sub

	'
	' Load Sort Order parameters
	'
	Sub LoadSortOrder()
		Dim sOrderBy As String
		sOrderBy = Contact.SessionOrderBy ' Get order by from Session
		If sOrderBy = "" Then
			If Contact.SqlOrderBy <> "" Then
				sOrderBy = Contact.SqlOrderBy
				Contact.SessionOrderBy = sOrderBy
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
				Contact.SessionOrderBy = sOrderBy
				Contact.LogonName.Sort = ""
				Contact.CompanyID.Sort = ""
				Contact.zEMail.Sort = ""
				Contact.PrimaryContact.Sort = ""
			End If

			' Reset start position
			lStartRec = 1
			Contact.StartRecordNumber = lStartRec
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
		If Contact.Export <> "" Or Contact.CurrentAction = "gridadd" Or Contact.CurrentAction = "gridedit" Then
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
				Contact.StartRecordNumber = lStartRec
			ElseIf ew_Get(EW_TABLE_PAGE_NO) <> "" AndAlso IsNumeric(ew_Get(EW_TABLE_PAGE_NO)) Then
				nPageNo = ew_ConvertToInt(ew_Get(EW_TABLE_PAGE_NO))
				lStartRec = (nPageNo-1)*lDisplayRecs+1
				If lStartRec <= 0 Then
					lStartRec = 1
				ElseIf lStartRec >= ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 Then
					lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1
				End If
				Contact.StartRecordNumber = lStartRec
			End If
		End If
		lStartRec = Contact.StartRecordNumber

		' Check if correct start record counter
		If lStartRec <= 0 Then ' Avoid invalid start record counter
			lStartRec = 1 ' Reset start record counter
			Contact.StartRecordNumber = lStartRec
		ElseIf lStartRec > lTotalRecs Then ' Avoid starting record > total records
			lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to last page first record
			Contact.StartRecordNumber = lStartRec
		ElseIf (lStartRec-1) Mod lDisplayRecs <> 0 Then
			lStartRec = ((lStartRec-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to page boundary
			Contact.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	' Load default values
	'
	Sub LoadDefaultValues()
		Contact.CompanyID.CurrentValue = 0
	End Sub

	'
	'  Load search values for validation
	'
	Sub LoadSearchValues()
		Contact.LogonName.AdvancedSearch.SearchValue = ew_Get("x_LogonName")
    	Contact.LogonName.AdvancedSearch.SearchOperator = ew_Get("z_LogonName")
		Contact.GroupID.AdvancedSearch.SearchValue = ew_Get("x_GroupID")
    	Contact.GroupID.AdvancedSearch.SearchOperator = ew_Get("z_GroupID")
		Contact.CompanyID.AdvancedSearch.SearchValue = ew_Get("x_CompanyID")
    	Contact.CompanyID.AdvancedSearch.SearchOperator = ew_Get("z_CompanyID")
		Contact.TemplatePrefix.AdvancedSearch.SearchValue = ew_Get("x_TemplatePrefix")
    	Contact.TemplatePrefix.AdvancedSearch.SearchOperator = ew_Get("z_TemplatePrefix")
	End Sub

	'
	' Load recordset
	'
	Function LoadRecordset() As OleDbDataReader

		' Recordset Selecting event
		Contact.Recordset_Selecting(Contact.CurrentFilter)

		' Load list page SQL
		Dim sSql As String = Contact.ListSQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then DebugMsg = sSql

		' Count
		lTotalRecs = -1		
		Try
			Dim sCntSql As String = Contact.SelectCountSQL

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
		Contact.Recordset_Selected(Rs)
		Return Rs
	End Function

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = Contact.KeyFilter

		' Row Selecting event
		Contact.Row_Selecting(sFilter)

		' Load SQL based on filter
		Contact.CurrentFilter = sFilter
		Dim sSql As String = Contact.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then DebugMsg = sSql
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				Contact.Row_Selected(RsRow)
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
		Contact.ContactID.DbValue = RsRow("ContactID")
		Contact.LogonName.DbValue = RsRow("LogonName")
		Contact.LogonPassword.DbValue = RsRow("LogonPassword")
		Contact.GroupID.DbValue = RsRow("GroupID")
		Contact.CompanyID.DbValue = RsRow("CompanyID")
		Contact.TemplatePrefix.DbValue = RsRow("TemplatePrefix")
		Contact.Active.DbValue = IIf(ew_ConvertToBool(RsRow("Active")), "1", "0")
		Contact.zEMail.DbValue = RsRow("EMail")
		Contact.PrimaryContact.DbValue = RsRow("PrimaryContact")
		Contact.FirstName.DbValue = RsRow("FirstName")
		Contact.MiddleInitial.DbValue = RsRow("MiddleInitial")
		Contact.LastName.DbValue = RsRow("LastName")
		Contact.MobilPhone.DbValue = RsRow("MobilPhone")
		Contact.OfficePhone.DbValue = RsRow("OfficePhone")
		Contact.HomePhone.DbValue = RsRow("HomePhone")
		Contact.Pager.DbValue = RsRow("Pager")
		Contact.URL.DbValue = RsRow("URL")
		Contact.Address1.DbValue = RsRow("Address1")
		Contact.Address2.DbValue = RsRow("Address2")
		Contact.City.DbValue = RsRow("City")
		Contact.State.DbValue = RsRow("State")
		Contact.Country.DbValue = RsRow("Country")
		Contact.PostalCode.DbValue = RsRow("PostalCode")
		Contact.Biography.DbValue = RsRow("Biography")
		Contact.CreateDT.DbValue = RsRow("CreateDT")
		Contact.Paid.DbValue = RsRow("Paid")
		Contact.email_subscribe.DbValue = IIf(ew_ConvertToBool(RsRow("email_subscribe")), "1", "0")
		Contact.RoleID.DbValue = RsRow("RoleID")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Initialize urls
		ViewUrl = Contact.ViewUrl
		EditUrl = Contact.EditUrl
		InlineEditUrl = Contact.InlineEditUrl
		CopyUrl = Contact.CopyUrl
		InlineCopyUrl = Contact.InlineCopyUrl
		DeleteUrl = Contact.DeleteUrl

		' Row Rendering event
		Contact.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' LogonName

		Contact.LogonName.CellCssStyle = "white-space: nowrap;"
		Contact.LogonName.CellCssClass = ""
		Contact.LogonName.CellAttrs.Clear(): Contact.LogonName.ViewAttrs.Clear(): Contact.LogonName.EditAttrs.Clear()

		' CompanyID
		Contact.CompanyID.CellCssStyle = "white-space: nowrap;"
		Contact.CompanyID.CellCssClass = ""
		Contact.CompanyID.CellAttrs.Clear(): Contact.CompanyID.ViewAttrs.Clear(): Contact.CompanyID.EditAttrs.Clear()

		' EMail
		Contact.zEMail.CellCssStyle = "white-space: nowrap;"
		Contact.zEMail.CellCssClass = ""
		Contact.zEMail.CellAttrs.Clear(): Contact.zEMail.ViewAttrs.Clear(): Contact.zEMail.EditAttrs.Clear()

		' PrimaryContact
		Contact.PrimaryContact.CellCssStyle = "white-space: nowrap;"
		Contact.PrimaryContact.CellCssClass = ""
		Contact.PrimaryContact.CellAttrs.Clear(): Contact.PrimaryContact.ViewAttrs.Clear(): Contact.PrimaryContact.EditAttrs.Clear()

		'
		'  View  Row
		'

		If Contact.RowType = EW_ROWTYPE_VIEW Then ' View row

			' ContactID
			Contact.ContactID.ViewValue = Contact.ContactID.CurrentValue
			Contact.ContactID.CssStyle = ""
			Contact.ContactID.CssClass = ""
			Contact.ContactID.ViewCustomAttributes = ""

			' LogonName
			Contact.LogonName.ViewValue = Contact.LogonName.CurrentValue
			Contact.LogonName.CssStyle = ""
			Contact.LogonName.CssClass = ""
			Contact.LogonName.ViewCustomAttributes = ""

			' LogonPassword
			Contact.LogonPassword.ViewValue = Contact.LogonPassword.CurrentValue
			Contact.LogonPassword.CssStyle = ""
			Contact.LogonPassword.CssClass = ""
			Contact.LogonPassword.ViewCustomAttributes = ""

			' GroupID
			If ew_NotEmpty(Contact.GroupID.CurrentValue) Then
				sFilterWrk = "[GroupID] = " & ew_AdjustSql(Contact.GroupID.CurrentValue) & ""
			sSqlWrk = "SELECT [GroupName] FROM [Group]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [GroupName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Contact.GroupID.ViewValue = RsWrk("GroupName")
				Else
					Contact.GroupID.ViewValue = Contact.GroupID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Contact.GroupID.ViewValue = System.DBNull.Value
			End If
			Contact.GroupID.CssStyle = ""
			Contact.GroupID.CssClass = ""
			Contact.GroupID.ViewCustomAttributes = ""

			' CompanyID
			If ew_NotEmpty(Contact.CompanyID.CurrentValue) Then
				sFilterWrk = "[CompanyID] = " & ew_AdjustSql(Contact.CompanyID.CurrentValue) & ""
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
					Contact.CompanyID.ViewValue = RsWrk("CompanyName")
				Else
					Contact.CompanyID.ViewValue = Contact.CompanyID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Contact.CompanyID.ViewValue = System.DBNull.Value
			End If
			Contact.CompanyID.CssStyle = ""
			Contact.CompanyID.CssClass = ""
			Contact.CompanyID.ViewCustomAttributes = ""

			' TemplatePrefix
			If ew_NotEmpty(Contact.TemplatePrefix.CurrentValue) Then
				sFilterWrk = "[TemplatePrefix] = '" & ew_AdjustSql(Contact.TemplatePrefix.CurrentValue) & "'"
			sSqlWrk = "SELECT [Name] FROM [SiteTemplate]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [Name] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Contact.TemplatePrefix.ViewValue = RsWrk("Name")
				Else
					Contact.TemplatePrefix.ViewValue = Contact.TemplatePrefix.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Contact.TemplatePrefix.ViewValue = System.DBNull.Value
			End If
			Contact.TemplatePrefix.CssStyle = ""
			Contact.TemplatePrefix.CssClass = ""
			Contact.TemplatePrefix.ViewCustomAttributes = ""

			' Active
			If Convert.ToString(Contact.Active.CurrentValue) = "1" Then
				Contact.Active.ViewValue = "Yes"
			Else
				Contact.Active.ViewValue = "No"
			End If
			Contact.Active.CssStyle = ""
			Contact.Active.CssClass = ""
			Contact.Active.ViewCustomAttributes = ""

			' EMail
			Contact.zEMail.ViewValue = Contact.zEMail.CurrentValue
			Contact.zEMail.CssStyle = ""
			Contact.zEMail.CssClass = ""
			Contact.zEMail.ViewCustomAttributes = ""

			' PrimaryContact
			Contact.PrimaryContact.ViewValue = Contact.PrimaryContact.CurrentValue
			Contact.PrimaryContact.CssStyle = ""
			Contact.PrimaryContact.CssClass = ""
			Contact.PrimaryContact.ViewCustomAttributes = ""

			' RoleID
			If ew_NotEmpty(Contact.RoleID.CurrentValue) Then
				sFilterWrk = "[RoleID] = " & ew_AdjustSql(Contact.RoleID.CurrentValue) & ""
			sSqlWrk = "SELECT [RoleName] FROM [role]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [RoleName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Contact.RoleID.ViewValue = RsWrk("RoleName")
				Else
					Contact.RoleID.ViewValue = Contact.RoleID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Contact.RoleID.ViewValue = System.DBNull.Value
			End If
			Contact.RoleID.CssStyle = ""
			Contact.RoleID.CssClass = ""
			Contact.RoleID.ViewCustomAttributes = ""

			' View refer script
			' LogonName

			Contact.LogonName.HrefValue = ""
			Contact.LogonName.TooltipValue = ""

			' CompanyID
			Contact.CompanyID.HrefValue = ""
			Contact.CompanyID.TooltipValue = ""

			' EMail
			Contact.zEMail.HrefValue = ""
			Contact.zEMail.TooltipValue = ""

			' PrimaryContact
			Contact.PrimaryContact.HrefValue = ""
			Contact.PrimaryContact.TooltipValue = ""
		End If

		' Row Rendered event
		If Contact.RowType <> EW_ROWTYPE_AGGREGATEINIT Then
			Contact.Row_Rendered()
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
		Contact.LogonName.AdvancedSearch.SearchValue = Contact.GetAdvancedSearch("x_LogonName")
		Contact.GroupID.AdvancedSearch.SearchValue = Contact.GetAdvancedSearch("x_GroupID")
		Contact.CompanyID.AdvancedSearch.SearchValue = Contact.GetAdvancedSearch("x_CompanyID")
		Contact.TemplatePrefix.AdvancedSearch.SearchValue = Contact.GetAdvancedSearch("x_TemplatePrefix")
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
		If Contact.ExportAll Then
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
		If Contact.Export = "xml" Then
			oXmlDoc = New cXMLDocument()
		Else
			sOutputStr = ew_ExportHeader(Contact.Export)

			' Horizontal format, write header
			If sExportStyle <> "v" OrElse Contact.Export = "csv" Then
				sExportStr = ""
				ew_ExportAddValue(sExportStr, Contact.ContactID.ExportCaption, Contact.Export, IIf(EW_EXPORT_CSS_STYLES, Contact.ContactID.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Contact.LogonName.ExportCaption, Contact.Export, IIf(EW_EXPORT_CSS_STYLES, Contact.LogonName.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Contact.LogonPassword.ExportCaption, Contact.Export, IIf(EW_EXPORT_CSS_STYLES, Contact.LogonPassword.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Contact.GroupID.ExportCaption, Contact.Export, IIf(EW_EXPORT_CSS_STYLES, Contact.GroupID.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Contact.CompanyID.ExportCaption, Contact.Export, IIf(EW_EXPORT_CSS_STYLES, Contact.CompanyID.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Contact.TemplatePrefix.ExportCaption, Contact.Export, IIf(EW_EXPORT_CSS_STYLES, Contact.TemplatePrefix.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Contact.Active.ExportCaption, Contact.Export, IIf(EW_EXPORT_CSS_STYLES, Contact.Active.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Contact.zEMail.ExportCaption, Contact.Export, IIf(EW_EXPORT_CSS_STYLES, Contact.zEMail.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Contact.PrimaryContact.ExportCaption, Contact.Export, IIf(EW_EXPORT_CSS_STYLES, Contact.PrimaryContact.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Contact.RoleID.ExportCaption, Contact.Export, IIf(EW_EXPORT_CSS_STYLES, Contact.RoleID.CellStyles, ""))
				sOutputStr &= ew_ExportLine(sExportStr, Contact.Export, IIf(EW_EXPORT_CSS_STYLES, Contact.RowStyles, ""))
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
				Contact.CssClass = ""
				Contact.CssStyle = ""
				Contact.RowType = EW_ROWTYPE_VIEW ' Render view
				RenderRow()
				If Contact.Export = "xml" Then
					oXmlDoc.AddRow()
					oXmlDoc.AddField("ContactID", Contact.ContactID.ExportValue(Contact.Export, Contact.ExportOriginalValue)) ' ContactID
					oXmlDoc.AddField("LogonName", Contact.LogonName.ExportValue(Contact.Export, Contact.ExportOriginalValue)) ' LogonName
					oXmlDoc.AddField("LogonPassword", Contact.LogonPassword.ExportValue(Contact.Export, Contact.ExportOriginalValue)) ' LogonPassword
					oXmlDoc.AddField("GroupID", Contact.GroupID.ExportValue(Contact.Export, Contact.ExportOriginalValue)) ' GroupID
					oXmlDoc.AddField("CompanyID", Contact.CompanyID.ExportValue(Contact.Export, Contact.ExportOriginalValue)) ' CompanyID
					oXmlDoc.AddField("TemplatePrefix", Contact.TemplatePrefix.ExportValue(Contact.Export, Contact.ExportOriginalValue)) ' TemplatePrefix
					oXmlDoc.AddField("Active", Contact.Active.ExportValue(Contact.Export, Contact.ExportOriginalValue)) ' Active
					oXmlDoc.AddField("zEMail", Contact.zEMail.ExportValue(Contact.Export, Contact.ExportOriginalValue)) ' EMail
					oXmlDoc.AddField("PrimaryContact", Contact.PrimaryContact.ExportValue(Contact.Export, Contact.ExportOriginalValue)) ' PrimaryContact
					oXmlDoc.AddField("RoleID", Contact.RoleID.ExportValue(Contact.Export, Contact.ExportOriginalValue)) ' RoleID
				Else

					' Vertical format
					If sExportStyle = "v" AndAlso Contact.Export <> "csv" Then
						sOutputStr &= ew_ExportField(Contact.ContactID.ExportCaption, Contact.ContactID.ExportValue(Contact.Export, Contact.ExportOriginalValue), Contact.Export, IIf(EW_EXPORT_CSS_STYLES, Contact.ContactID.CellStyles, "")) ' ContactID
						sOutputStr &= ew_ExportField(Contact.LogonName.ExportCaption, Contact.LogonName.ExportValue(Contact.Export, Contact.ExportOriginalValue), Contact.Export, IIf(EW_EXPORT_CSS_STYLES, Contact.LogonName.CellStyles, "")) ' LogonName
						sOutputStr &= ew_ExportField(Contact.LogonPassword.ExportCaption, Contact.LogonPassword.ExportValue(Contact.Export, Contact.ExportOriginalValue), Contact.Export, IIf(EW_EXPORT_CSS_STYLES, Contact.LogonPassword.CellStyles, "")) ' LogonPassword
						sOutputStr &= ew_ExportField(Contact.GroupID.ExportCaption, Contact.GroupID.ExportValue(Contact.Export, Contact.ExportOriginalValue), Contact.Export, IIf(EW_EXPORT_CSS_STYLES, Contact.GroupID.CellStyles, "")) ' GroupID
						sOutputStr &= ew_ExportField(Contact.CompanyID.ExportCaption, Contact.CompanyID.ExportValue(Contact.Export, Contact.ExportOriginalValue), Contact.Export, IIf(EW_EXPORT_CSS_STYLES, Contact.CompanyID.CellStyles, "")) ' CompanyID
						sOutputStr &= ew_ExportField(Contact.TemplatePrefix.ExportCaption, Contact.TemplatePrefix.ExportValue(Contact.Export, Contact.ExportOriginalValue), Contact.Export, IIf(EW_EXPORT_CSS_STYLES, Contact.TemplatePrefix.CellStyles, "")) ' TemplatePrefix
						sOutputStr &= ew_ExportField(Contact.Active.ExportCaption, Contact.Active.ExportValue(Contact.Export, Contact.ExportOriginalValue), Contact.Export, IIf(EW_EXPORT_CSS_STYLES, Contact.Active.CellStyles, "")) ' Active
						sOutputStr &= ew_ExportField(Contact.zEMail.ExportCaption, Contact.zEMail.ExportValue(Contact.Export, Contact.ExportOriginalValue), Contact.Export, IIf(EW_EXPORT_CSS_STYLES, Contact.zEMail.CellStyles, "")) ' EMail
						sOutputStr &= ew_ExportField(Contact.PrimaryContact.ExportCaption, Contact.PrimaryContact.ExportValue(Contact.Export, Contact.ExportOriginalValue), Contact.Export, IIf(EW_EXPORT_CSS_STYLES, Contact.PrimaryContact.CellStyles, "")) ' PrimaryContact
						sOutputStr &= ew_ExportField(Contact.RoleID.ExportCaption, Contact.RoleID.ExportValue(Contact.Export, Contact.ExportOriginalValue), Contact.Export, IIf(EW_EXPORT_CSS_STYLES, Contact.RoleID.CellStyles, "")) ' RoleID

					' Horizontal format
					Else
						sExportStr = ""
						ew_ExportAddValue(sExportStr, Contact.ContactID.ExportValue(Contact.Export, Contact.ExportOriginalValue), Contact.Export, IIf(EW_EXPORT_CSS_STYLES, Contact.ContactID.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Contact.LogonName.ExportValue(Contact.Export, Contact.ExportOriginalValue), Contact.Export, IIf(EW_EXPORT_CSS_STYLES, Contact.LogonName.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Contact.LogonPassword.ExportValue(Contact.Export, Contact.ExportOriginalValue), Contact.Export, IIf(EW_EXPORT_CSS_STYLES, Contact.LogonPassword.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Contact.GroupID.ExportValue(Contact.Export, Contact.ExportOriginalValue), Contact.Export, IIf(EW_EXPORT_CSS_STYLES, Contact.GroupID.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Contact.CompanyID.ExportValue(Contact.Export, Contact.ExportOriginalValue), Contact.Export, IIf(EW_EXPORT_CSS_STYLES, Contact.CompanyID.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Contact.TemplatePrefix.ExportValue(Contact.Export, Contact.ExportOriginalValue), Contact.Export, IIf(EW_EXPORT_CSS_STYLES, Contact.TemplatePrefix.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Contact.Active.ExportValue(Contact.Export, Contact.ExportOriginalValue), Contact.Export, IIf(EW_EXPORT_CSS_STYLES, Contact.Active.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Contact.zEMail.ExportValue(Contact.Export, Contact.ExportOriginalValue), Contact.Export, IIf(EW_EXPORT_CSS_STYLES, Contact.zEMail.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Contact.PrimaryContact.ExportValue(Contact.Export, Contact.ExportOriginalValue), Contact.Export, IIf(EW_EXPORT_CSS_STYLES, Contact.PrimaryContact.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Contact.RoleID.ExportValue(Contact.Export, Contact.ExportOriginalValue), Contact.Export, IIf(EW_EXPORT_CSS_STYLES, Contact.RoleID.CellStyles, ""))
						sOutputStr &= ew_ExportLine(sExportStr, Contact.Export, IIf(EW_EXPORT_CSS_STYLES, Contact.RowStyles, ""))
					End If
				End If
			End If
		Loop

		' Close recordset
		Rs.Close()
		Rs.Dispose()
		If Contact.Export = "xml" Then
			oXmlDoc.Output
		Else
			sOutputStr &= ew_ExportFooter(Contact.Export)
			ew_Write(sOutputStr)
		End If
	End Sub

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "Contact"
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
		Contact_list = New cContact_list(Me)		
		Contact_list.Page_Init()		

		' Set buffer/cache option
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

		' Page main processing
		Contact_list.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If Contact_list IsNot Nothing Then Contact_list.Dispose()
	End Sub
End Class
