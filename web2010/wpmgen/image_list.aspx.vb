Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class image_list
	Inherits AspNetMaker8_wpmWebsite

	' Page object
	Public Image_list As cImage_list

	'
	' Page Class
	'
	Class cImage_list
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
				If Image.UseTokenInUrl Then Url = Url & "t=" & Image.TableVar & "&" ' Add page token
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
			If Image.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (Image.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (Image.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' ASP.NET page object
		Public ReadOnly Property AspNetPage() As image_list
			Get
				Return CType(m_ParentPage, image_list)
			End Get
		End Property

		' Image
		Public Property Image() As cImage
			Get				
				Return ParentPage.Image
			End Get
			Set(ByVal v As cImage)
				ParentPage.Image = v	
			End Set	
		End Property

		' Image
		Public Property Company() As cCompany
			Get				
				Return ParentPage.Company
			End Get
			Set(ByVal v As cCompany)
				ParentPage.Company = v	
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
			m_PageObjName = "Image_list"
			m_PageObjTypeName = "cImage_list"			

			' Initialize language object
			Language = New cLanguage(Me)

			' Table Name
			m_TableName = "Image"

			' Initialize table object
			Image = New cImage(Me)
			Company = New cCompany(Me)

			' Initialize URLs
			ExportPrintUrl = PageUrl & "export=print"
			ExportExcelUrl = PageUrl & "export=excel"
			ExportWordUrl = PageUrl & "export=word"
			ExportHtmlUrl = PageUrl & "export=html"
			ExportXmlUrl = PageUrl & "export=xml"
			ExportCsvUrl = PageUrl & "export=csv"
			AddUrl = Image.AddUrl
			InlineAddUrl = PageUrl & "a=add"
			GridAddUrl = PageUrl & "a=gridadd"
			GridEditUrl = PageUrl & "a=gridedit"
			MultiDeleteUrl = "image_delete.aspx"
			MultiUpdateUrl = "image_update.aspx"

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
				Image.Export = ew_Get("export")
			ElseIf ew_Post("exporttype") <> "" Then
				Image.Export = ew_Post("exporttype")
			Else
				Image.ExportReturnUrl = ew_CurrentUrl()
			End If
			ParentPage.gsExport = Image.Export ' Get export parameter, used in header
			ParentPage.gsExportFile = Image.TableVar ' Get export file, used in header
			If Image.Export = "excel" Then
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
			Image.Dispose()
			Company.Dispose()
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
			Call Image.Recordset_SearchValidated()

			' Set Up Sorting Order
			SetUpSortOrder()

			' Get search criteria for advanced search
			If ParentPage.gsSearchError = "" Then
				sSrchAdvanced = AdvancedSearchWhere()
			End If
		End If

		' Restore display records
		If (Image.RecordsPerPage = -1 OrElse Image.RecordsPerPage > 0) Then
			lDisplayRecs = Image.RecordsPerPage ' Restore from Session
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
		Image.Recordset_Searching(sSrchWhere)

		' Save search criteria
		If sSrchWhere <> "" Then
			If sSrchAdvanced = "" Then ResetAdvancedSearchParms()
			Image.SearchWhere = sSrchWhere ' Save to Session
			If RestoreSearch Then
				lStartRec = 1 ' Reset start record counter
				Image.StartRecordNumber = lStartRec
			End If
		Else
			sSrchWhere = Image.SearchWhere
		End If

		' Build filter
		sFilter = ""

		' Restore master/detail filter
		sDbMasterFilter = Image.MasterFilter ' Restore master filter
		sDbDetailFilter = Image.DetailFilter ' Restore detail filter
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
		If Image.MasterFilter <> "" AndAlso Image.CurrentMasterTable = "Company" Then
			RsMaster = Company.LoadRs(sDbMasterFilter)
			bMasterRecordExists = RsMaster IsNot Nothing
			If Not bMasterRecordExists Then
				Image.MasterFilter = "" ' Clear master filter
				Image.DetailFilter = "" ' Clear detail filter
				Message = Language.Phrase("NoRecord") ' Set no record found
			Page_Terminate(Image.ReturnUrl) ' Return to caller
			Else
				RsMaster.Read()
				Company.LoadListRowValues(RsMaster)
				Company.RowType = EW_ROWTYPE_MASTER ' Master row
				Company.RenderListRow()
				RsMaster.Close()
				RsMaster.Dispose()
			End If
		End If

		' Set up filter in Session
		Image.SessionWhere = sFilter
		Image.CurrentFilter = ""

		' Export Data only
		If Image.Export = "html" OrElse Image.Export = "csv" OrElse Image.Export = "word" OrElse Image.Export = "excel" OrElse Image.Export = "xml" Then
			ExportData()
			Page_Terminate("") ' Clean up
			ew_End() ' Terminate response
		ElseIf Image.Export = "email" Then
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
			Image.RecordsPerPage = lDisplayRecs ' Save to Session

			' Reset start position
			lStartRec = 1
			Image.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	' Return Advanced Search WHERE based on QueryString parameters
	'
	Function AdvancedSearchWhere() As String		
		Dim sWhere As String = ""
		BuildSearchSql(sWhere, Image.ImageID, False) ' ImageID
		BuildSearchSql(sWhere, Image.ImageName, False) ' ImageName
		BuildSearchSql(sWhere, Image.ImageFileName, False) ' ImageFileName
		BuildSearchSql(sWhere, Image.ImageThumbFileName, False) ' ImageThumbFileName
		BuildSearchSql(sWhere, Image.ImageDescription, False) ' ImageDescription
		BuildSearchSql(sWhere, Image.ImageComment, False) ' ImageComment
		BuildSearchSql(sWhere, Image.ImageDate, False) ' ImageDate
		BuildSearchSql(sWhere, Image.Active, False) ' Active
		BuildSearchSql(sWhere, Image.ModifiedDT, False) ' ModifiedDT
		BuildSearchSql(sWhere, Image.VersionNo, False) ' VersionNo
		BuildSearchSql(sWhere, Image.ContactID, False) ' ContactID
		BuildSearchSql(sWhere, Image.CompanyID, False) ' CompanyID
		BuildSearchSql(sWhere, Image.title, False) ' title
		BuildSearchSql(sWhere, Image.medium, False) ' medium
		BuildSearchSql(sWhere, Image.size, False) ' size
		BuildSearchSql(sWhere, Image.price, False) ' price
		BuildSearchSql(sWhere, Image.color, False) ' color
		BuildSearchSql(sWhere, Image.subject, False) ' subject
		BuildSearchSql(sWhere, Image.sold, False) ' sold

		' Set up search parm
		If sWhere <> "" Then
			SetSearchParm(Image.ImageID) ' ImageID
			SetSearchParm(Image.ImageName) ' ImageName
			SetSearchParm(Image.ImageFileName) ' ImageFileName
			SetSearchParm(Image.ImageThumbFileName) ' ImageThumbFileName
			SetSearchParm(Image.ImageDescription) ' ImageDescription
			SetSearchParm(Image.ImageComment) ' ImageComment
			SetSearchParm(Image.ImageDate) ' ImageDate
			SetSearchParm(Image.Active) ' Active
			SetSearchParm(Image.ModifiedDT) ' ModifiedDT
			SetSearchParm(Image.VersionNo) ' VersionNo
			SetSearchParm(Image.ContactID) ' ContactID
			SetSearchParm(Image.CompanyID) ' CompanyID
			SetSearchParm(Image.title) ' title
			SetSearchParm(Image.medium) ' medium
			SetSearchParm(Image.size) ' size
			SetSearchParm(Image.price) ' price
			SetSearchParm(Image.color) ' color
			SetSearchParm(Image.subject) ' subject
			SetSearchParm(Image.sold) ' sold
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
		Image.SetAdvancedSearch("x_" & FldParm, Fld.AdvancedSearch.SearchValue)
		Image.SetAdvancedSearch("z_" & FldParm, Fld.AdvancedSearch.SearchOperator)
		Image.SetAdvancedSearch("v_" & FldParm, Fld.AdvancedSearch.SearchCondition)
		Image.SetAdvancedSearch("y_" & FldParm, Fld.AdvancedSearch.SearchValue2)
		Image.SetAdvancedSearch("w_" & FldParm, Fld.AdvancedSearch.SearchOperator2)
	End Sub

	'
	' Get search parm
	'
	Sub GetSearchParm(ByRef Fld As Object)
		Dim FldParm As String = Fld.FldVar.Substring(2)
		Fld.AdvancedSearch.SearchValue = Image.GetAdvancedSearch("x_" & FldParm)
		Fld.AdvancedSearch.SearchOperator = Image.GetAdvancedSearch("z_" & FldParm)
		Fld.AdvancedSearch.SearchCondition = Image.GetAdvancedSearch("v_" & FldParm)
		Fld.AdvancedSearch.SearchValue2 = Image.GetAdvancedSearch("y_" & FldParm)
		Fld.AdvancedSearch.SearchOperator2 = Image.GetAdvancedSearch("w_" & FldParm)
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
		Image.SearchWhere = sSrchWhere

		' Clear advanced search parameters
		ResetAdvancedSearchParms()
	End Sub

	'
	' Clear all advanced search parameters
	'
	Sub ResetAdvancedSearchParms()
		Image.SetAdvancedSearch("x_ImageID", "")
		Image.SetAdvancedSearch("x_ImageName", "")
		Image.SetAdvancedSearch("x_ImageFileName", "")
		Image.SetAdvancedSearch("x_ImageThumbFileName", "")
		Image.SetAdvancedSearch("x_ImageDescription", "")
		Image.SetAdvancedSearch("x_ImageComment", "")
		Image.SetAdvancedSearch("x_ImageDate", "")
		Image.SetAdvancedSearch("x_Active", "")
		Image.SetAdvancedSearch("x_ModifiedDT", "")
		Image.SetAdvancedSearch("x_VersionNo", "")
		Image.SetAdvancedSearch("x_ContactID", "")
		Image.SetAdvancedSearch("x_CompanyID", "")
		Image.SetAdvancedSearch("x_title", "")
		Image.SetAdvancedSearch("x_medium", "")
		Image.SetAdvancedSearch("x_size", "")
		Image.SetAdvancedSearch("x_price", "")
		Image.SetAdvancedSearch("x_color", "")
		Image.SetAdvancedSearch("x_subject", "")
		Image.SetAdvancedSearch("x_sold", "")
	End Sub

	'
	' Restore all search parameters
	'
	Sub RestoreSearchParms()
		Dim bRestore As Boolean = True
		If ew_Get("x_ImageID") <> "" Then bRestore = False
		If ew_Get("x_ImageName") <> "" Then bRestore = False
		If ew_Get("x_ImageFileName") <> "" Then bRestore = False
		If ew_Get("x_ImageThumbFileName") <> "" Then bRestore = False
		If ew_Get("x_ImageDescription") <> "" Then bRestore = False
		If ew_Get("x_ImageComment") <> "" Then bRestore = False
		If ew_Get("x_ImageDate") <> "" Then bRestore = False
		If ew_Get("x_Active") <> "" Then bRestore = False
		If ew_Get("x_ModifiedDT") <> "" Then bRestore = False
		If ew_Get("x_VersionNo") <> "" Then bRestore = False
		If ew_Get("x_ContactID") <> "" Then bRestore = False
		If ew_Get("x_CompanyID") <> "" Then bRestore = False
		If ew_Get("x_title") <> "" Then bRestore = False
		If ew_Get("x_medium") <> "" Then bRestore = False
		If ew_Get("x_size") <> "" Then bRestore = False
		If ew_Get("x_price") <> "" Then bRestore = False
		If ew_Get("x_color") <> "" Then bRestore = False
		If ew_Get("x_subject") <> "" Then bRestore = False
		If ew_Get("x_sold") <> "" Then bRestore = False
		RestoreSearch = bRestore
		If bRestore Then

			' Restore advanced search values
			Call GetSearchParm(Image.ImageID)
			Call GetSearchParm(Image.ImageName)
			Call GetSearchParm(Image.ImageFileName)
			Call GetSearchParm(Image.ImageThumbFileName)
			Call GetSearchParm(Image.ImageDescription)
			Call GetSearchParm(Image.ImageComment)
			Call GetSearchParm(Image.ImageDate)
			Call GetSearchParm(Image.Active)
			Call GetSearchParm(Image.ModifiedDT)
			Call GetSearchParm(Image.VersionNo)
			Call GetSearchParm(Image.ContactID)
			Call GetSearchParm(Image.CompanyID)
			Call GetSearchParm(Image.title)
			Call GetSearchParm(Image.medium)
			Call GetSearchParm(Image.size)
			Call GetSearchParm(Image.price)
			Call GetSearchParm(Image.color)
			Call GetSearchParm(Image.subject)
			Call GetSearchParm(Image.sold)
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
			Image.CurrentOrder = ew_Get("order")
			Image.CurrentOrderType = ew_Get("ordertype")
			Image.UpdateSort(Image.ImageID) ' ImageID
			Image.UpdateSort(Image.ImageName) ' ImageName
			Image.UpdateSort(Image.ImageFileName) ' ImageFileName
			Image.UpdateSort(Image.ImageThumbFileName) ' ImageThumbFileName
			Image.UpdateSort(Image.ImageDate) ' ImageDate
			Image.UpdateSort(Image.Active) ' Active
			Image.UpdateSort(Image.ModifiedDT) ' ModifiedDT
			Image.UpdateSort(Image.VersionNo) ' VersionNo
			Image.UpdateSort(Image.ContactID) ' ContactID
			Image.UpdateSort(Image.CompanyID) ' CompanyID
			Image.UpdateSort(Image.title) ' title
			Image.UpdateSort(Image.medium) ' medium
			Image.UpdateSort(Image.size) ' size
			Image.UpdateSort(Image.price) ' price
			Image.UpdateSort(Image.color) ' color
			Image.UpdateSort(Image.subject) ' subject
			Image.UpdateSort(Image.sold) ' sold
			Image.StartRecordNumber = 1 ' Reset start position
		End If
	End Sub

	'
	' Load Sort Order parameters
	'
	Sub LoadSortOrder()
		Dim sOrderBy As String
		sOrderBy = Image.SessionOrderBy ' Get order by from Session
		If sOrderBy = "" Then
			If Image.SqlOrderBy <> "" Then
				sOrderBy = Image.SqlOrderBy
				Image.SessionOrderBy = sOrderBy
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
				Image.CurrentMasterTable = "" ' Clear master table
				Image.MasterFilter = "" ' Clear master filter
				sDbMasterFilter = ""
				Image.DetailFilter = "" ' Clear detail filter
				sDbDetailFilter = ""
				Image.CompanyID.SessionValue = ""
			End If

			' Reset sort criteria
			If ew_SameText(sCmd, "resetsort") Then
				Dim sOrderBy As String = ""
				Image.SessionOrderBy = sOrderBy
				Image.ImageID.Sort = ""
				Image.ImageName.Sort = ""
				Image.ImageFileName.Sort = ""
				Image.ImageThumbFileName.Sort = ""
				Image.ImageDate.Sort = ""
				Image.Active.Sort = ""
				Image.ModifiedDT.Sort = ""
				Image.VersionNo.Sort = ""
				Image.ContactID.Sort = ""
				Image.CompanyID.Sort = ""
				Image.title.Sort = ""
				Image.medium.Sort = ""
				Image.size.Sort = ""
				Image.price.Sort = ""
				Image.color.Sort = ""
				Image.subject.Sort = ""
				Image.sold.Sort = ""
			End If

			' Reset start position
			lStartRec = 1
			Image.StartRecordNumber = lStartRec
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
		ListOptions.Add("detail_PageImage")
		ListOptions.GetItem("detail_PageImage").CssStyle = "white-space: nowrap;"
		ListOptions.GetItem("detail_PageImage").Visible = True
		ListOptions.GetItem("detail_PageImage").OnLeft = True
		ListOptions_Load()
		If Image.Export <> "" Or Image.CurrentAction = "gridadd" Or Image.CurrentAction = "gridedit" Then
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
			oListOpt = ListOptions.GetItem("detail_PageImage")
			oListOpt.Body = "<img src=""images/detail.gif"" alt=""" & ew_HtmlEncode(Language.Phrase("DetailLink")) & """ title=""" & ew_HtmlEncode(Language.Phrase("DetailLink")) & """ width=""16"" height=""16"" border=""0"" />" & Language.TablePhrase("PageImage", "TblCaption")
			oListOpt.Body = "<a href=""pageimage_list.aspx?" & EW_TABLE_SHOW_MASTER & "=Image&ImageID=" & HttpContext.Current.Server.UrlEncode(Convert.ToString(Image.ImageID.CurrentValue)) & """>" & oListOpt.Body & "</a>"
		End If
		RenderListOptionsExt()
		ListOptions_Rendered()
	End Sub

	Sub RenderListOptionsExt()
		Dim sHyperLinkParm As String, oListOpt As cListOption
		sSqlWrk = "[ImageID]=" & ew_AdjustSql(Image.ImageID.CurrentValue) & ""
		sSqlWrk = cTEA.Encrypt(sSqlWrk, EW_RANDOM_KEY)
		sSqlWrk = sSqlWrk.Replace("'", "\'")
		sHyperLinkParm = " href=""pageimage_list.aspx?" & EW_TABLE_SHOW_MASTER & "=Image&ImageID=" & HttpContext.Current.Server.URLEncode(Convert.ToString(Image.ImageID.CurrentValue)) & """"
		oListOpt = ListOptions.GetItem("detail_PageImage")
		oListOpt.Body = Language.TablePhrase("PageImage", "TblCaption")
		oListOpt.Body &= "<img src=""images/detail.gif"" alt=""" & ew_HtmlEncode(Language.Phrase("DetailLink")) & """ title=""" & ew_HtmlEncode(Language.Phrase("DetailLink")) & """ width=""16"" height=""16"" border=""0"" />"
		oListOpt.Body = "<a" & sHyperLinkParm & ">" & oListOpt.Body & "</a>"
		sHyperLinkParm = " href=""javascript:void(0);"" name=""ew_Image_PageImage_DetailLink%i"" id=""ew_Image_PageImage_DetailLink%i"" onclick=""ew_AjaxShowDetails2(event, this, 'pageimage_preview.aspx?f=%s')"""
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
				Image.StartRecordNumber = lStartRec
			ElseIf ew_Get(EW_TABLE_PAGE_NO) <> "" AndAlso IsNumeric(ew_Get(EW_TABLE_PAGE_NO)) Then
				nPageNo = ew_ConvertToInt(ew_Get(EW_TABLE_PAGE_NO))
				lStartRec = (nPageNo-1)*lDisplayRecs+1
				If lStartRec <= 0 Then
					lStartRec = 1
				ElseIf lStartRec >= ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 Then
					lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1
				End If
				Image.StartRecordNumber = lStartRec
			End If
		End If
		lStartRec = Image.StartRecordNumber

		' Check if correct start record counter
		If lStartRec <= 0 Then ' Avoid invalid start record counter
			lStartRec = 1 ' Reset start record counter
			Image.StartRecordNumber = lStartRec
		ElseIf lStartRec > lTotalRecs Then ' Avoid starting record > total records
			lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to last page first record
			Image.StartRecordNumber = lStartRec
		ElseIf (lStartRec-1) Mod lDisplayRecs <> 0 Then
			lStartRec = ((lStartRec-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to page boundary
			Image.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	' Load default values
	'
	Sub LoadDefaultValues()
		Image.VersionNo.CurrentValue = 0
		Image.ContactID.CurrentValue = 0
		Image.CompanyID.CurrentValue = 0
		Image.sold.CurrentValue = 0
	End Sub

	'
	'  Load search values for validation
	'
	Sub LoadSearchValues()
		Image.ImageID.AdvancedSearch.SearchValue = ew_Get("x_ImageID")
    	Image.ImageID.AdvancedSearch.SearchOperator = ew_Get("z_ImageID")
		Image.ImageName.AdvancedSearch.SearchValue = ew_Get("x_ImageName")
    	Image.ImageName.AdvancedSearch.SearchOperator = ew_Get("z_ImageName")
		Image.ImageFileName.AdvancedSearch.SearchValue = ew_Get("x_ImageFileName")
    	Image.ImageFileName.AdvancedSearch.SearchOperator = ew_Get("z_ImageFileName")
		Image.ImageThumbFileName.AdvancedSearch.SearchValue = ew_Get("x_ImageThumbFileName")
    	Image.ImageThumbFileName.AdvancedSearch.SearchOperator = ew_Get("z_ImageThumbFileName")
		Image.ImageDescription.AdvancedSearch.SearchValue = ew_Get("x_ImageDescription")
    	Image.ImageDescription.AdvancedSearch.SearchOperator = ew_Get("z_ImageDescription")
		Image.ImageComment.AdvancedSearch.SearchValue = ew_Get("x_ImageComment")
    	Image.ImageComment.AdvancedSearch.SearchOperator = ew_Get("z_ImageComment")
		Image.ImageDate.AdvancedSearch.SearchValue = ew_Get("x_ImageDate")
    	Image.ImageDate.AdvancedSearch.SearchOperator = ew_Get("z_ImageDate")
		Image.Active.AdvancedSearch.SearchValue = ew_Get("x_Active")
    	Image.Active.AdvancedSearch.SearchOperator = ew_Get("z_Active")
		Image.ModifiedDT.AdvancedSearch.SearchValue = ew_Get("x_ModifiedDT")
    	Image.ModifiedDT.AdvancedSearch.SearchOperator = ew_Get("z_ModifiedDT")
		Image.VersionNo.AdvancedSearch.SearchValue = ew_Get("x_VersionNo")
    	Image.VersionNo.AdvancedSearch.SearchOperator = ew_Get("z_VersionNo")
		Image.ContactID.AdvancedSearch.SearchValue = ew_Get("x_ContactID")
    	Image.ContactID.AdvancedSearch.SearchOperator = ew_Get("z_ContactID")
		Image.CompanyID.AdvancedSearch.SearchValue = ew_Get("x_CompanyID")
    	Image.CompanyID.AdvancedSearch.SearchOperator = ew_Get("z_CompanyID")
		Image.title.AdvancedSearch.SearchValue = ew_Get("x_title")
    	Image.title.AdvancedSearch.SearchOperator = ew_Get("z_title")
		Image.medium.AdvancedSearch.SearchValue = ew_Get("x_medium")
    	Image.medium.AdvancedSearch.SearchOperator = ew_Get("z_medium")
		Image.size.AdvancedSearch.SearchValue = ew_Get("x_size")
    	Image.size.AdvancedSearch.SearchOperator = ew_Get("z_size")
		Image.price.AdvancedSearch.SearchValue = ew_Get("x_price")
    	Image.price.AdvancedSearch.SearchOperator = ew_Get("z_price")
		Image.color.AdvancedSearch.SearchValue = ew_Get("x_color")
    	Image.color.AdvancedSearch.SearchOperator = ew_Get("z_color")
		Image.subject.AdvancedSearch.SearchValue = ew_Get("x_subject")
    	Image.subject.AdvancedSearch.SearchOperator = ew_Get("z_subject")
		Image.sold.AdvancedSearch.SearchValue = ew_Get("x_sold")
    	Image.sold.AdvancedSearch.SearchOperator = ew_Get("z_sold")
	End Sub

	'
	' Load recordset
	'
	Function LoadRecordset() As OleDbDataReader

		' Recordset Selecting event
		Image.Recordset_Selecting(Image.CurrentFilter)

		' Load list page SQL
		Dim sSql As String = Image.ListSQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then DebugMsg = sSql

		' Count
		lTotalRecs = -1		
		Try
			Dim sCntSql As String = Image.SelectCountSQL

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
		Image.Recordset_Selected(Rs)
		Return Rs
	End Function

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = Image.KeyFilter

		' Row Selecting event
		Image.Row_Selecting(sFilter)

		' Load SQL based on filter
		Image.CurrentFilter = sFilter
		Dim sSql As String = Image.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then DebugMsg = sSql
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				Image.Row_Selected(RsRow)
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
		Image.ImageID.DbValue = RsRow("ImageID")
		Image.ImageName.DbValue = RsRow("ImageName")
		Image.ImageFileName.DbValue = RsRow("ImageFileName")
		Image.ImageThumbFileName.DbValue = RsRow("ImageThumbFileName")
		Image.ImageDescription.DbValue = RsRow("ImageDescription")
		Image.ImageComment.DbValue = RsRow("ImageComment")
		Image.ImageDate.DbValue = RsRow("ImageDate")
		Image.Active.DbValue = IIf(ew_ConvertToBool(RsRow("Active")), "1", "0")
		Image.ModifiedDT.DbValue = RsRow("ModifiedDT")
		Image.VersionNo.DbValue = RsRow("VersionNo")
		Image.ContactID.DbValue = RsRow("ContactID")
		Image.CompanyID.DbValue = RsRow("CompanyID")
		Image.title.DbValue = RsRow("title")
		Image.medium.DbValue = RsRow("medium")
		Image.size.DbValue = RsRow("size")
		Image.price.DbValue = RsRow("price")
		Image.color.DbValue = RsRow("color")
		Image.subject.DbValue = RsRow("subject")
		Image.sold.DbValue = IIf(ew_ConvertToBool(RsRow("sold")), "1", "0")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Initialize urls
		ViewUrl = Image.ViewUrl
		EditUrl = Image.EditUrl
		InlineEditUrl = Image.InlineEditUrl
		CopyUrl = Image.CopyUrl
		InlineCopyUrl = Image.InlineCopyUrl
		DeleteUrl = Image.DeleteUrl

		' Row Rendering event
		Image.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' ImageID

		Image.ImageID.CellCssStyle = ""
		Image.ImageID.CellCssClass = ""
		Image.ImageID.CellAttrs.Clear(): Image.ImageID.ViewAttrs.Clear(): Image.ImageID.EditAttrs.Clear()

		' ImageName
		Image.ImageName.CellCssStyle = ""
		Image.ImageName.CellCssClass = ""
		Image.ImageName.CellAttrs.Clear(): Image.ImageName.ViewAttrs.Clear(): Image.ImageName.EditAttrs.Clear()

		' ImageFileName
		Image.ImageFileName.CellCssStyle = ""
		Image.ImageFileName.CellCssClass = ""
		Image.ImageFileName.CellAttrs.Clear(): Image.ImageFileName.ViewAttrs.Clear(): Image.ImageFileName.EditAttrs.Clear()

		' ImageThumbFileName
		Image.ImageThumbFileName.CellCssStyle = ""
		Image.ImageThumbFileName.CellCssClass = ""
		Image.ImageThumbFileName.CellAttrs.Clear(): Image.ImageThumbFileName.ViewAttrs.Clear(): Image.ImageThumbFileName.EditAttrs.Clear()

		' ImageDate
		Image.ImageDate.CellCssStyle = ""
		Image.ImageDate.CellCssClass = ""
		Image.ImageDate.CellAttrs.Clear(): Image.ImageDate.ViewAttrs.Clear(): Image.ImageDate.EditAttrs.Clear()

		' Active
		Image.Active.CellCssStyle = ""
		Image.Active.CellCssClass = ""
		Image.Active.CellAttrs.Clear(): Image.Active.ViewAttrs.Clear(): Image.Active.EditAttrs.Clear()

		' ModifiedDT
		Image.ModifiedDT.CellCssStyle = ""
		Image.ModifiedDT.CellCssClass = ""
		Image.ModifiedDT.CellAttrs.Clear(): Image.ModifiedDT.ViewAttrs.Clear(): Image.ModifiedDT.EditAttrs.Clear()

		' VersionNo
		Image.VersionNo.CellCssStyle = ""
		Image.VersionNo.CellCssClass = ""
		Image.VersionNo.CellAttrs.Clear(): Image.VersionNo.ViewAttrs.Clear(): Image.VersionNo.EditAttrs.Clear()

		' ContactID
		Image.ContactID.CellCssStyle = ""
		Image.ContactID.CellCssClass = ""
		Image.ContactID.CellAttrs.Clear(): Image.ContactID.ViewAttrs.Clear(): Image.ContactID.EditAttrs.Clear()

		' CompanyID
		Image.CompanyID.CellCssStyle = ""
		Image.CompanyID.CellCssClass = ""
		Image.CompanyID.CellAttrs.Clear(): Image.CompanyID.ViewAttrs.Clear(): Image.CompanyID.EditAttrs.Clear()

		' title
		Image.title.CellCssStyle = ""
		Image.title.CellCssClass = ""
		Image.title.CellAttrs.Clear(): Image.title.ViewAttrs.Clear(): Image.title.EditAttrs.Clear()

		' medium
		Image.medium.CellCssStyle = ""
		Image.medium.CellCssClass = ""
		Image.medium.CellAttrs.Clear(): Image.medium.ViewAttrs.Clear(): Image.medium.EditAttrs.Clear()

		' size
		Image.size.CellCssStyle = ""
		Image.size.CellCssClass = ""
		Image.size.CellAttrs.Clear(): Image.size.ViewAttrs.Clear(): Image.size.EditAttrs.Clear()

		' price
		Image.price.CellCssStyle = ""
		Image.price.CellCssClass = ""
		Image.price.CellAttrs.Clear(): Image.price.ViewAttrs.Clear(): Image.price.EditAttrs.Clear()

		' color
		Image.color.CellCssStyle = ""
		Image.color.CellCssClass = ""
		Image.color.CellAttrs.Clear(): Image.color.ViewAttrs.Clear(): Image.color.EditAttrs.Clear()

		' subject
		Image.subject.CellCssStyle = ""
		Image.subject.CellCssClass = ""
		Image.subject.CellAttrs.Clear(): Image.subject.ViewAttrs.Clear(): Image.subject.EditAttrs.Clear()

		' sold
		Image.sold.CellCssStyle = ""
		Image.sold.CellCssClass = ""
		Image.sold.CellAttrs.Clear(): Image.sold.ViewAttrs.Clear(): Image.sold.EditAttrs.Clear()

		'
		'  View  Row
		'

		If Image.RowType = EW_ROWTYPE_VIEW Then ' View row

			' ImageID
			Image.ImageID.ViewValue = Image.ImageID.CurrentValue
			Image.ImageID.CssStyle = ""
			Image.ImageID.CssClass = ""
			Image.ImageID.ViewCustomAttributes = ""

			' ImageName
			Image.ImageName.ViewValue = Image.ImageName.CurrentValue
			Image.ImageName.CssStyle = ""
			Image.ImageName.CssClass = ""
			Image.ImageName.ViewCustomAttributes = ""

			' ImageFileName
			Image.ImageFileName.ViewValue = Image.ImageFileName.CurrentValue
			Image.ImageFileName.CssStyle = ""
			Image.ImageFileName.CssClass = ""
			Image.ImageFileName.ViewCustomAttributes = ""

			' ImageThumbFileName
			Image.ImageThumbFileName.ViewValue = Image.ImageThumbFileName.CurrentValue
			Image.ImageThumbFileName.CssStyle = ""
			Image.ImageThumbFileName.CssClass = ""
			Image.ImageThumbFileName.ViewCustomAttributes = ""

			' ImageDate
			Image.ImageDate.ViewValue = Image.ImageDate.CurrentValue
			Image.ImageDate.CssStyle = ""
			Image.ImageDate.CssClass = ""
			Image.ImageDate.ViewCustomAttributes = ""

			' Active
			If Convert.ToString(Image.Active.CurrentValue) = "1" Then
				Image.Active.ViewValue = "Yes"
			Else
				Image.Active.ViewValue = "No"
			End If
			Image.Active.CssStyle = ""
			Image.Active.CssClass = ""
			Image.Active.ViewCustomAttributes = ""

			' ModifiedDT
			Image.ModifiedDT.ViewValue = Image.ModifiedDT.CurrentValue
			Image.ModifiedDT.CssStyle = ""
			Image.ModifiedDT.CssClass = ""
			Image.ModifiedDT.ViewCustomAttributes = ""

			' VersionNo
			Image.VersionNo.ViewValue = Image.VersionNo.CurrentValue
			Image.VersionNo.CssStyle = ""
			Image.VersionNo.CssClass = ""
			Image.VersionNo.ViewCustomAttributes = ""

			' ContactID
			Image.ContactID.ViewValue = Image.ContactID.CurrentValue
			Image.ContactID.CssStyle = ""
			Image.ContactID.CssClass = ""
			Image.ContactID.ViewCustomAttributes = ""

			' CompanyID
			Image.CompanyID.ViewValue = Image.CompanyID.CurrentValue
			Image.CompanyID.CssStyle = ""
			Image.CompanyID.CssClass = ""
			Image.CompanyID.ViewCustomAttributes = ""

			' title
			Image.title.ViewValue = Image.title.CurrentValue
			Image.title.CssStyle = ""
			Image.title.CssClass = ""
			Image.title.ViewCustomAttributes = ""

			' medium
			Image.medium.ViewValue = Image.medium.CurrentValue
			Image.medium.CssStyle = ""
			Image.medium.CssClass = ""
			Image.medium.ViewCustomAttributes = ""

			' size
			Image.size.ViewValue = Image.size.CurrentValue
			Image.size.CssStyle = ""
			Image.size.CssClass = ""
			Image.size.ViewCustomAttributes = ""

			' price
			Image.price.ViewValue = Image.price.CurrentValue
			Image.price.CssStyle = ""
			Image.price.CssClass = ""
			Image.price.ViewCustomAttributes = ""

			' color
			Image.color.ViewValue = Image.color.CurrentValue
			Image.color.CssStyle = ""
			Image.color.CssClass = ""
			Image.color.ViewCustomAttributes = ""

			' subject
			Image.subject.ViewValue = Image.subject.CurrentValue
			Image.subject.CssStyle = ""
			Image.subject.CssClass = ""
			Image.subject.ViewCustomAttributes = ""

			' sold
			If Convert.ToString(Image.sold.CurrentValue) = "1" Then
				Image.sold.ViewValue = "Yes"
			Else
				Image.sold.ViewValue = "No"
			End If
			Image.sold.CssStyle = ""
			Image.sold.CssClass = ""
			Image.sold.ViewCustomAttributes = ""

			' View refer script
			' ImageID

			Image.ImageID.HrefValue = ""
			Image.ImageID.TooltipValue = ""

			' ImageName
			Image.ImageName.HrefValue = ""
			Image.ImageName.TooltipValue = ""

			' ImageFileName
			Image.ImageFileName.HrefValue = ""
			Image.ImageFileName.TooltipValue = ""

			' ImageThumbFileName
			Image.ImageThumbFileName.HrefValue = ""
			Image.ImageThumbFileName.TooltipValue = ""

			' ImageDate
			Image.ImageDate.HrefValue = ""
			Image.ImageDate.TooltipValue = ""

			' Active
			Image.Active.HrefValue = ""
			Image.Active.TooltipValue = ""

			' ModifiedDT
			Image.ModifiedDT.HrefValue = ""
			Image.ModifiedDT.TooltipValue = ""

			' VersionNo
			Image.VersionNo.HrefValue = ""
			Image.VersionNo.TooltipValue = ""

			' ContactID
			Image.ContactID.HrefValue = ""
			Image.ContactID.TooltipValue = ""

			' CompanyID
			Image.CompanyID.HrefValue = ""
			Image.CompanyID.TooltipValue = ""

			' title
			Image.title.HrefValue = ""
			Image.title.TooltipValue = ""

			' medium
			Image.medium.HrefValue = ""
			Image.medium.TooltipValue = ""

			' size
			Image.size.HrefValue = ""
			Image.size.TooltipValue = ""

			' price
			Image.price.HrefValue = ""
			Image.price.TooltipValue = ""

			' color
			Image.color.HrefValue = ""
			Image.color.TooltipValue = ""

			' subject
			Image.subject.HrefValue = ""
			Image.subject.TooltipValue = ""

			' sold
			Image.sold.HrefValue = ""
			Image.sold.TooltipValue = ""
		End If

		' Row Rendered event
		If Image.RowType <> EW_ROWTYPE_AGGREGATEINIT Then
			Image.Row_Rendered()
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
		Image.ImageID.AdvancedSearch.SearchValue = Image.GetAdvancedSearch("x_ImageID")
		Image.ImageName.AdvancedSearch.SearchValue = Image.GetAdvancedSearch("x_ImageName")
		Image.ImageFileName.AdvancedSearch.SearchValue = Image.GetAdvancedSearch("x_ImageFileName")
		Image.ImageThumbFileName.AdvancedSearch.SearchValue = Image.GetAdvancedSearch("x_ImageThumbFileName")
		Image.ImageDescription.AdvancedSearch.SearchValue = Image.GetAdvancedSearch("x_ImageDescription")
		Image.ImageComment.AdvancedSearch.SearchValue = Image.GetAdvancedSearch("x_ImageComment")
		Image.ImageDate.AdvancedSearch.SearchValue = Image.GetAdvancedSearch("x_ImageDate")
		Image.Active.AdvancedSearch.SearchValue = Image.GetAdvancedSearch("x_Active")
		Image.ModifiedDT.AdvancedSearch.SearchValue = Image.GetAdvancedSearch("x_ModifiedDT")
		Image.VersionNo.AdvancedSearch.SearchValue = Image.GetAdvancedSearch("x_VersionNo")
		Image.ContactID.AdvancedSearch.SearchValue = Image.GetAdvancedSearch("x_ContactID")
		Image.CompanyID.AdvancedSearch.SearchValue = Image.GetAdvancedSearch("x_CompanyID")
		Image.title.AdvancedSearch.SearchValue = Image.GetAdvancedSearch("x_title")
		Image.medium.AdvancedSearch.SearchValue = Image.GetAdvancedSearch("x_medium")
		Image.size.AdvancedSearch.SearchValue = Image.GetAdvancedSearch("x_size")
		Image.price.AdvancedSearch.SearchValue = Image.GetAdvancedSearch("x_price")
		Image.color.AdvancedSearch.SearchValue = Image.GetAdvancedSearch("x_color")
		Image.subject.AdvancedSearch.SearchValue = Image.GetAdvancedSearch("x_subject")
		Image.sold.AdvancedSearch.SearchValue = Image.GetAdvancedSearch("x_sold")
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
		If Image.ExportAll Then
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
		If Image.Export = "xml" Then
			oXmlDoc = New cXMLDocument()
		Else
			sOutputStr = ew_ExportHeader(Image.Export)

			' Horizontal format, write header
			If sExportStyle <> "v" OrElse Image.Export = "csv" Then
				sExportStr = ""
				ew_ExportAddValue(sExportStr, Image.ImageID.ExportCaption, Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.ImageID.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Image.ImageName.ExportCaption, Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.ImageName.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Image.ImageFileName.ExportCaption, Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.ImageFileName.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Image.ImageThumbFileName.ExportCaption, Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.ImageThumbFileName.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Image.ImageDate.ExportCaption, Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.ImageDate.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Image.Active.ExportCaption, Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.Active.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Image.ModifiedDT.ExportCaption, Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.ModifiedDT.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Image.VersionNo.ExportCaption, Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.VersionNo.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Image.ContactID.ExportCaption, Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.ContactID.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Image.CompanyID.ExportCaption, Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.CompanyID.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Image.title.ExportCaption, Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.title.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Image.medium.ExportCaption, Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.medium.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Image.size.ExportCaption, Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.size.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Image.price.ExportCaption, Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.price.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Image.color.ExportCaption, Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.color.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Image.subject.ExportCaption, Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.subject.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Image.sold.ExportCaption, Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.sold.CellStyles, ""))
				sOutputStr &= ew_ExportLine(sExportStr, Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.RowStyles, ""))
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
				Image.CssClass = ""
				Image.CssStyle = ""
				Image.RowType = EW_ROWTYPE_VIEW ' Render view
				RenderRow()
				If Image.Export = "xml" Then
					oXmlDoc.AddRow()
					oXmlDoc.AddField("ImageID", Image.ImageID.ExportValue(Image.Export, Image.ExportOriginalValue)) ' ImageID
					oXmlDoc.AddField("ImageName", Image.ImageName.ExportValue(Image.Export, Image.ExportOriginalValue)) ' ImageName
					oXmlDoc.AddField("ImageFileName", Image.ImageFileName.ExportValue(Image.Export, Image.ExportOriginalValue)) ' ImageFileName
					oXmlDoc.AddField("ImageThumbFileName", Image.ImageThumbFileName.ExportValue(Image.Export, Image.ExportOriginalValue)) ' ImageThumbFileName
					oXmlDoc.AddField("ImageDate", Image.ImageDate.ExportValue(Image.Export, Image.ExportOriginalValue)) ' ImageDate
					oXmlDoc.AddField("Active", Image.Active.ExportValue(Image.Export, Image.ExportOriginalValue)) ' Active
					oXmlDoc.AddField("ModifiedDT", Image.ModifiedDT.ExportValue(Image.Export, Image.ExportOriginalValue)) ' ModifiedDT
					oXmlDoc.AddField("VersionNo", Image.VersionNo.ExportValue(Image.Export, Image.ExportOriginalValue)) ' VersionNo
					oXmlDoc.AddField("ContactID", Image.ContactID.ExportValue(Image.Export, Image.ExportOriginalValue)) ' ContactID
					oXmlDoc.AddField("CompanyID", Image.CompanyID.ExportValue(Image.Export, Image.ExportOriginalValue)) ' CompanyID
					oXmlDoc.AddField("title", Image.title.ExportValue(Image.Export, Image.ExportOriginalValue)) ' title
					oXmlDoc.AddField("medium", Image.medium.ExportValue(Image.Export, Image.ExportOriginalValue)) ' medium
					oXmlDoc.AddField("size", Image.size.ExportValue(Image.Export, Image.ExportOriginalValue)) ' size
					oXmlDoc.AddField("price", Image.price.ExportValue(Image.Export, Image.ExportOriginalValue)) ' price
					oXmlDoc.AddField("color", Image.color.ExportValue(Image.Export, Image.ExportOriginalValue)) ' color
					oXmlDoc.AddField("subject", Image.subject.ExportValue(Image.Export, Image.ExportOriginalValue)) ' subject
					oXmlDoc.AddField("sold", Image.sold.ExportValue(Image.Export, Image.ExportOriginalValue)) ' sold
				Else

					' Vertical format
					If sExportStyle = "v" AndAlso Image.Export <> "csv" Then
						sOutputStr &= ew_ExportField(Image.ImageID.ExportCaption, Image.ImageID.ExportValue(Image.Export, Image.ExportOriginalValue), Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.ImageID.CellStyles, "")) ' ImageID
						sOutputStr &= ew_ExportField(Image.ImageName.ExportCaption, Image.ImageName.ExportValue(Image.Export, Image.ExportOriginalValue), Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.ImageName.CellStyles, "")) ' ImageName
						sOutputStr &= ew_ExportField(Image.ImageFileName.ExportCaption, Image.ImageFileName.ExportValue(Image.Export, Image.ExportOriginalValue), Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.ImageFileName.CellStyles, "")) ' ImageFileName
						sOutputStr &= ew_ExportField(Image.ImageThumbFileName.ExportCaption, Image.ImageThumbFileName.ExportValue(Image.Export, Image.ExportOriginalValue), Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.ImageThumbFileName.CellStyles, "")) ' ImageThumbFileName
						sOutputStr &= ew_ExportField(Image.ImageDate.ExportCaption, Image.ImageDate.ExportValue(Image.Export, Image.ExportOriginalValue), Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.ImageDate.CellStyles, "")) ' ImageDate
						sOutputStr &= ew_ExportField(Image.Active.ExportCaption, Image.Active.ExportValue(Image.Export, Image.ExportOriginalValue), Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.Active.CellStyles, "")) ' Active
						sOutputStr &= ew_ExportField(Image.ModifiedDT.ExportCaption, Image.ModifiedDT.ExportValue(Image.Export, Image.ExportOriginalValue), Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.ModifiedDT.CellStyles, "")) ' ModifiedDT
						sOutputStr &= ew_ExportField(Image.VersionNo.ExportCaption, Image.VersionNo.ExportValue(Image.Export, Image.ExportOriginalValue), Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.VersionNo.CellStyles, "")) ' VersionNo
						sOutputStr &= ew_ExportField(Image.ContactID.ExportCaption, Image.ContactID.ExportValue(Image.Export, Image.ExportOriginalValue), Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.ContactID.CellStyles, "")) ' ContactID
						sOutputStr &= ew_ExportField(Image.CompanyID.ExportCaption, Image.CompanyID.ExportValue(Image.Export, Image.ExportOriginalValue), Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.CompanyID.CellStyles, "")) ' CompanyID
						sOutputStr &= ew_ExportField(Image.title.ExportCaption, Image.title.ExportValue(Image.Export, Image.ExportOriginalValue), Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.title.CellStyles, "")) ' title
						sOutputStr &= ew_ExportField(Image.medium.ExportCaption, Image.medium.ExportValue(Image.Export, Image.ExportOriginalValue), Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.medium.CellStyles, "")) ' medium
						sOutputStr &= ew_ExportField(Image.size.ExportCaption, Image.size.ExportValue(Image.Export, Image.ExportOriginalValue), Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.size.CellStyles, "")) ' size
						sOutputStr &= ew_ExportField(Image.price.ExportCaption, Image.price.ExportValue(Image.Export, Image.ExportOriginalValue), Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.price.CellStyles, "")) ' price
						sOutputStr &= ew_ExportField(Image.color.ExportCaption, Image.color.ExportValue(Image.Export, Image.ExportOriginalValue), Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.color.CellStyles, "")) ' color
						sOutputStr &= ew_ExportField(Image.subject.ExportCaption, Image.subject.ExportValue(Image.Export, Image.ExportOriginalValue), Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.subject.CellStyles, "")) ' subject
						sOutputStr &= ew_ExportField(Image.sold.ExportCaption, Image.sold.ExportValue(Image.Export, Image.ExportOriginalValue), Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.sold.CellStyles, "")) ' sold

					' Horizontal format
					Else
						sExportStr = ""
						ew_ExportAddValue(sExportStr, Image.ImageID.ExportValue(Image.Export, Image.ExportOriginalValue), Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.ImageID.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Image.ImageName.ExportValue(Image.Export, Image.ExportOriginalValue), Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.ImageName.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Image.ImageFileName.ExportValue(Image.Export, Image.ExportOriginalValue), Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.ImageFileName.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Image.ImageThumbFileName.ExportValue(Image.Export, Image.ExportOriginalValue), Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.ImageThumbFileName.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Image.ImageDate.ExportValue(Image.Export, Image.ExportOriginalValue), Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.ImageDate.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Image.Active.ExportValue(Image.Export, Image.ExportOriginalValue), Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.Active.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Image.ModifiedDT.ExportValue(Image.Export, Image.ExportOriginalValue), Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.ModifiedDT.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Image.VersionNo.ExportValue(Image.Export, Image.ExportOriginalValue), Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.VersionNo.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Image.ContactID.ExportValue(Image.Export, Image.ExportOriginalValue), Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.ContactID.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Image.CompanyID.ExportValue(Image.Export, Image.ExportOriginalValue), Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.CompanyID.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Image.title.ExportValue(Image.Export, Image.ExportOriginalValue), Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.title.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Image.medium.ExportValue(Image.Export, Image.ExportOriginalValue), Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.medium.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Image.size.ExportValue(Image.Export, Image.ExportOriginalValue), Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.size.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Image.price.ExportValue(Image.Export, Image.ExportOriginalValue), Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.price.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Image.color.ExportValue(Image.Export, Image.ExportOriginalValue), Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.color.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Image.subject.ExportValue(Image.Export, Image.ExportOriginalValue), Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.subject.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Image.sold.ExportValue(Image.Export, Image.ExportOriginalValue), Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.sold.CellStyles, ""))
						sOutputStr &= ew_ExportLine(sExportStr, Image.Export, IIf(EW_EXPORT_CSS_STYLES, Image.RowStyles, ""))
					End If
				End If
			End If
		Loop

		' Close recordset
		Rs.Close()
		Rs.Dispose()
		If Image.Export = "xml" Then
			oXmlDoc.Output
		Else
			sOutputStr &= ew_ExportFooter(Image.Export)
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
			If sMasterTblVar = "Company" Then
				bValidMaster = True
				sDbMasterFilter = Image.SqlMasterFilter_Company
				sDbDetailFilter = Image.SqlDetailFilter_Company
				If ew_Get("CompanyID") <> "" Then
					Company.CompanyID.QueryStringValue = ew_Get("CompanyID")
					Image.CompanyID.QueryStringValue = Company.CompanyID.QueryStringValue
					Image.CompanyID.SessionValue = Image.CompanyID.QueryStringValue
					If Not IsNumeric(Company.CompanyID.QueryStringValue) Then bValidMaster = False
					sDbMasterFilter = sDbMasterFilter.Replace("@CompanyID@", ew_AdjustSql(Company.CompanyID.QueryStringValue))
					sDbDetailFilter = sDbDetailFilter.Replace("@CompanyID@", ew_AdjustSql(Company.CompanyID.QueryStringValue))
				Else
					bValidMaster = False
				End If
			End If
		End If
		If bValidMaster Then

			' Save current master table
			Image.CurrentMasterTable = sMasterTblVar

			' Reset start record counter (new master key)
			lStartRec = 1
			Image.StartRecordNumber = lStartRec
			Image.MasterFilter = sDbMasterFilter ' Set up master filter
			Image.DetailFilter = sDbDetailFilter ' Set up detail filter

			' Clear previous master session values
			If sMasterTblVar <> "Company" Then
				If Image.CompanyID.QueryStringValue = "" Then Image.CompanyID.SessionValue = ""
			End If
		End If
	End Sub

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "Image"
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
		Image_list = New cImage_list(Me)		
		Image_list.Page_Init()		

		' Set buffer/cache option
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

		' Page main processing
		Image_list.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If Image_list IsNot Nothing Then Image_list.Dispose()
	End Sub
End Class
