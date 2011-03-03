Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class company_list
	Inherits AspNetMaker8_wpmWebsite

	' Page object
	Public Company_list As cCompany_list

	'
	' Page Class
	'
	Class cCompany_list
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
				If Company.UseTokenInUrl Then Url = Url & "t=" & Company.TableVar & "&" ' Add page token
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
			If Company.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (Company.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (Company.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' ASP.NET page object
		Public ReadOnly Property AspNetPage() As company_list
			Get
				Return CType(m_ParentPage, company_list)
			End Get
		End Property

		' Company
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
			m_PageObjName = "Company_list"
			m_PageObjTypeName = "cCompany_list"			

			' Initialize language object
			Language = New cLanguage(Me)

			' Table Name
			m_TableName = "Company"

			' Initialize table object
			Company = New cCompany(Me)

			' Initialize URLs
			ExportPrintUrl = PageUrl & "export=print"
			ExportExcelUrl = PageUrl & "export=excel"
			ExportWordUrl = PageUrl & "export=word"
			ExportHtmlUrl = PageUrl & "export=html"
			ExportXmlUrl = PageUrl & "export=xml"
			ExportCsvUrl = PageUrl & "export=csv"
			AddUrl = Company.AddUrl
			InlineAddUrl = PageUrl & "a=add"
			GridAddUrl = PageUrl & "a=gridadd"
			GridEditUrl = PageUrl & "a=gridedit"
			MultiDeleteUrl = "company_delete.aspx"
			MultiUpdateUrl = "company_update.aspx"

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
				Company.Export = ew_Get("export")
			ElseIf ew_Post("exporttype") <> "" Then
				Company.Export = ew_Post("exporttype")
			Else
				Company.ExportReturnUrl = ew_CurrentUrl()
			End If
			ParentPage.gsExport = Company.Export ' Get export parameter, used in header
			ParentPage.gsExportFile = Company.TableVar ' Get export file, used in header
			If Company.Export = "excel" Then
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
			Call Company.Recordset_SearchValidated()

			' Set Up Sorting Order
			SetUpSortOrder()

			' Get search criteria for advanced search
			If ParentPage.gsSearchError = "" Then
				sSrchAdvanced = AdvancedSearchWhere()
			End If
		End If

		' Restore display records
		If (Company.RecordsPerPage = -1 OrElse Company.RecordsPerPage > 0) Then
			lDisplayRecs = Company.RecordsPerPage ' Restore from Session
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
		Company.Recordset_Searching(sSrchWhere)

		' Save search criteria
		If sSrchWhere <> "" Then
			If sSrchAdvanced = "" Then ResetAdvancedSearchParms()
			Company.SearchWhere = sSrchWhere ' Save to Session
			If RestoreSearch Then
				lStartRec = 1 ' Reset start record counter
				Company.StartRecordNumber = lStartRec
			End If
		Else
			sSrchWhere = Company.SearchWhere
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
		Company.SessionWhere = sFilter
		Company.CurrentFilter = ""

		' Export Data only
		If Company.Export = "html" OrElse Company.Export = "csv" OrElse Company.Export = "word" OrElse Company.Export = "excel" OrElse Company.Export = "xml" Then
			ExportData()
			Page_Terminate("") ' Clean up
			ew_End() ' Terminate response
		ElseIf Company.Export = "email" Then
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
			Company.RecordsPerPage = lDisplayRecs ' Save to Session

			' Reset start position
			lStartRec = 1
			Company.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	' Return Advanced Search WHERE based on QueryString parameters
	'
	Function AdvancedSearchWhere() As String		
		Dim sWhere As String = ""
		BuildSearchSql(sWhere, Company.CompanyID, False) ' CompanyID
		BuildSearchSql(sWhere, Company.CompanyName, False) ' CompanyName
		BuildSearchSql(sWhere, Company.SiteCategoryTypeID, False) ' SiteCategoryTypeID
		BuildSearchSql(sWhere, Company.SiteTitle, False) ' SiteTitle
		BuildSearchSql(sWhere, Company.SiteURL, False) ' SiteURL
		BuildSearchSql(sWhere, Company.GalleryFolder, False) ' GalleryFolder
		BuildSearchSql(sWhere, Company.HomePageID, False) ' HomePageID
		BuildSearchSql(sWhere, Company.DefaultArticleID, False) ' DefaultArticleID
		BuildSearchSql(sWhere, Company.SiteTemplate, False) ' SiteTemplate
		BuildSearchSql(sWhere, Company.DefaultSiteTemplate, False) ' DefaultSiteTemplate
		BuildSearchSql(sWhere, Company.Address, False) ' Address
		BuildSearchSql(sWhere, Company.City, False) ' City
		BuildSearchSql(sWhere, Company.StateOrProvince, False) ' StateOrProvince
		BuildSearchSql(sWhere, Company.PostalCode, False) ' PostalCode
		BuildSearchSql(sWhere, Company.Country, False) ' Country
		BuildSearchSql(sWhere, Company.PhoneNumber, False) ' PhoneNumber
		BuildSearchSql(sWhere, Company.FaxNumber, False) ' FaxNumber
		BuildSearchSql(sWhere, Company.DefaultPaymentTerms, False) ' DefaultPaymentTerms
		BuildSearchSql(sWhere, Company.DefaultInvoiceDescription, False) ' DefaultInvoiceDescription
		BuildSearchSql(sWhere, Company.Component, False) ' Component
		BuildSearchSql(sWhere, Company.FromEmail, False) ' FromEmail
		BuildSearchSql(sWhere, Company.SMTP, False) ' SMTP
		BuildSearchSql(sWhere, Company.ActiveFL, False) ' ActiveFL
		BuildSearchSql(sWhere, Company.UseBreadCrumbURL, False) ' UseBreadCrumbURL
		BuildSearchSql(sWhere, Company.SingleSiteGallery, False) ' SingleSiteGallery

		' Set up search parm
		If sWhere <> "" Then
			SetSearchParm(Company.CompanyID) ' CompanyID
			SetSearchParm(Company.CompanyName) ' CompanyName
			SetSearchParm(Company.SiteCategoryTypeID) ' SiteCategoryTypeID
			SetSearchParm(Company.SiteTitle) ' SiteTitle
			SetSearchParm(Company.SiteURL) ' SiteURL
			SetSearchParm(Company.GalleryFolder) ' GalleryFolder
			SetSearchParm(Company.HomePageID) ' HomePageID
			SetSearchParm(Company.DefaultArticleID) ' DefaultArticleID
			SetSearchParm(Company.SiteTemplate) ' SiteTemplate
			SetSearchParm(Company.DefaultSiteTemplate) ' DefaultSiteTemplate
			SetSearchParm(Company.Address) ' Address
			SetSearchParm(Company.City) ' City
			SetSearchParm(Company.StateOrProvince) ' StateOrProvince
			SetSearchParm(Company.PostalCode) ' PostalCode
			SetSearchParm(Company.Country) ' Country
			SetSearchParm(Company.PhoneNumber) ' PhoneNumber
			SetSearchParm(Company.FaxNumber) ' FaxNumber
			SetSearchParm(Company.DefaultPaymentTerms) ' DefaultPaymentTerms
			SetSearchParm(Company.DefaultInvoiceDescription) ' DefaultInvoiceDescription
			SetSearchParm(Company.Component) ' Component
			SetSearchParm(Company.FromEmail) ' FromEmail
			SetSearchParm(Company.SMTP) ' SMTP
			SetSearchParm(Company.ActiveFL) ' ActiveFL
			SetSearchParm(Company.UseBreadCrumbURL) ' UseBreadCrumbURL
			SetSearchParm(Company.SingleSiteGallery) ' SingleSiteGallery
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
		Company.SetAdvancedSearch("x_" & FldParm, Fld.AdvancedSearch.SearchValue)
		Company.SetAdvancedSearch("z_" & FldParm, Fld.AdvancedSearch.SearchOperator)
		Company.SetAdvancedSearch("v_" & FldParm, Fld.AdvancedSearch.SearchCondition)
		Company.SetAdvancedSearch("y_" & FldParm, Fld.AdvancedSearch.SearchValue2)
		Company.SetAdvancedSearch("w_" & FldParm, Fld.AdvancedSearch.SearchOperator2)
	End Sub

	'
	' Get search parm
	'
	Sub GetSearchParm(ByRef Fld As Object)
		Dim FldParm As String = Fld.FldVar.Substring(2)
		Fld.AdvancedSearch.SearchValue = Company.GetAdvancedSearch("x_" & FldParm)
		Fld.AdvancedSearch.SearchOperator = Company.GetAdvancedSearch("z_" & FldParm)
		Fld.AdvancedSearch.SearchCondition = Company.GetAdvancedSearch("v_" & FldParm)
		Fld.AdvancedSearch.SearchValue2 = Company.GetAdvancedSearch("y_" & FldParm)
		Fld.AdvancedSearch.SearchOperator2 = Company.GetAdvancedSearch("w_" & FldParm)
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
		Company.SearchWhere = sSrchWhere

		' Clear advanced search parameters
		ResetAdvancedSearchParms()
	End Sub

	'
	' Clear all advanced search parameters
	'
	Sub ResetAdvancedSearchParms()
		Company.SetAdvancedSearch("x_CompanyID", "")
		Company.SetAdvancedSearch("x_CompanyName", "")
		Company.SetAdvancedSearch("x_SiteCategoryTypeID", "")
		Company.SetAdvancedSearch("x_SiteTitle", "")
		Company.SetAdvancedSearch("x_SiteURL", "")
		Company.SetAdvancedSearch("x_GalleryFolder", "")
		Company.SetAdvancedSearch("x_HomePageID", "")
		Company.SetAdvancedSearch("x_DefaultArticleID", "")
		Company.SetAdvancedSearch("x_SiteTemplate", "")
		Company.SetAdvancedSearch("x_DefaultSiteTemplate", "")
		Company.SetAdvancedSearch("x_Address", "")
		Company.SetAdvancedSearch("x_City", "")
		Company.SetAdvancedSearch("x_StateOrProvince", "")
		Company.SetAdvancedSearch("x_PostalCode", "")
		Company.SetAdvancedSearch("x_Country", "")
		Company.SetAdvancedSearch("x_PhoneNumber", "")
		Company.SetAdvancedSearch("x_FaxNumber", "")
		Company.SetAdvancedSearch("x_DefaultPaymentTerms", "")
		Company.SetAdvancedSearch("x_DefaultInvoiceDescription", "")
		Company.SetAdvancedSearch("x_Component", "")
		Company.SetAdvancedSearch("x_FromEmail", "")
		Company.SetAdvancedSearch("x_SMTP", "")
		Company.SetAdvancedSearch("x_ActiveFL", "")
		Company.SetAdvancedSearch("x_UseBreadCrumbURL", "")
		Company.SetAdvancedSearch("x_SingleSiteGallery", "")
	End Sub

	'
	' Restore all search parameters
	'
	Sub RestoreSearchParms()
		Dim bRestore As Boolean = True
		If ew_Get("x_CompanyID") <> "" Then bRestore = False
		If ew_Get("x_CompanyName") <> "" Then bRestore = False
		If ew_Get("x_SiteCategoryTypeID") <> "" Then bRestore = False
		If ew_Get("x_SiteTitle") <> "" Then bRestore = False
		If ew_Get("x_SiteURL") <> "" Then bRestore = False
		If ew_Get("x_GalleryFolder") <> "" Then bRestore = False
		If ew_Get("x_HomePageID") <> "" Then bRestore = False
		If ew_Get("x_DefaultArticleID") <> "" Then bRestore = False
		If ew_Get("x_SiteTemplate") <> "" Then bRestore = False
		If ew_Get("x_DefaultSiteTemplate") <> "" Then bRestore = False
		If ew_Get("x_Address") <> "" Then bRestore = False
		If ew_Get("x_City") <> "" Then bRestore = False
		If ew_Get("x_StateOrProvince") <> "" Then bRestore = False
		If ew_Get("x_PostalCode") <> "" Then bRestore = False
		If ew_Get("x_Country") <> "" Then bRestore = False
		If ew_Get("x_PhoneNumber") <> "" Then bRestore = False
		If ew_Get("x_FaxNumber") <> "" Then bRestore = False
		If ew_Get("x_DefaultPaymentTerms") <> "" Then bRestore = False
		If ew_Get("x_DefaultInvoiceDescription") <> "" Then bRestore = False
		If ew_Get("x_Component") <> "" Then bRestore = False
		If ew_Get("x_FromEmail") <> "" Then bRestore = False
		If ew_Get("x_SMTP") <> "" Then bRestore = False
		If ew_Get("x_ActiveFL") <> "" Then bRestore = False
		If ew_Get("x_UseBreadCrumbURL") <> "" Then bRestore = False
		If ew_Get("x_SingleSiteGallery") <> "" Then bRestore = False
		RestoreSearch = bRestore
		If bRestore Then

			' Restore advanced search values
			Call GetSearchParm(Company.CompanyID)
			Call GetSearchParm(Company.CompanyName)
			Call GetSearchParm(Company.SiteCategoryTypeID)
			Call GetSearchParm(Company.SiteTitle)
			Call GetSearchParm(Company.SiteURL)
			Call GetSearchParm(Company.GalleryFolder)
			Call GetSearchParm(Company.HomePageID)
			Call GetSearchParm(Company.DefaultArticleID)
			Call GetSearchParm(Company.SiteTemplate)
			Call GetSearchParm(Company.DefaultSiteTemplate)
			Call GetSearchParm(Company.Address)
			Call GetSearchParm(Company.City)
			Call GetSearchParm(Company.StateOrProvince)
			Call GetSearchParm(Company.PostalCode)
			Call GetSearchParm(Company.Country)
			Call GetSearchParm(Company.PhoneNumber)
			Call GetSearchParm(Company.FaxNumber)
			Call GetSearchParm(Company.DefaultPaymentTerms)
			Call GetSearchParm(Company.DefaultInvoiceDescription)
			Call GetSearchParm(Company.Component)
			Call GetSearchParm(Company.FromEmail)
			Call GetSearchParm(Company.SMTP)
			Call GetSearchParm(Company.ActiveFL)
			Call GetSearchParm(Company.UseBreadCrumbURL)
			Call GetSearchParm(Company.SingleSiteGallery)
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
			Company.CurrentOrder = ew_Get("order")
			Company.CurrentOrderType = ew_Get("ordertype")
			Company.UpdateSort(Company.CompanyName) ' CompanyName
			Company.UpdateSort(Company.SiteTitle) ' SiteTitle
			Company.UpdateSort(Company.GalleryFolder) ' GalleryFolder
			Company.UpdateSort(Company.SiteTemplate) ' SiteTemplate
			Company.UpdateSort(Company.DefaultSiteTemplate) ' DefaultSiteTemplate
			Company.StartRecordNumber = 1 ' Reset start position
		End If
	End Sub

	'
	' Load Sort Order parameters
	'
	Sub LoadSortOrder()
		Dim sOrderBy As String
		sOrderBy = Company.SessionOrderBy ' Get order by from Session
		If sOrderBy = "" Then
			If Company.SqlOrderBy <> "" Then
				sOrderBy = Company.SqlOrderBy
				Company.SessionOrderBy = sOrderBy
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
				Company.SessionOrderBy = sOrderBy
				Company.CompanyName.Sort = ""
				Company.SiteTitle.Sort = ""
				Company.GalleryFolder.Sort = ""
				Company.SiteTemplate.Sort = ""
				Company.DefaultSiteTemplate.Sort = ""
			End If

			' Reset start position
			lStartRec = 1
			Company.StartRecordNumber = lStartRec
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
		ListOptions.Add("detail_zPage")
		ListOptions.GetItem("detail_zPage").CssStyle = "white-space: nowrap;"
		ListOptions.GetItem("detail_zPage").Visible = True
		ListOptions.GetItem("detail_zPage").OnLeft = True
		ListOptions.Add("detail_Image")
		ListOptions.GetItem("detail_Image").CssStyle = "white-space: nowrap;"
		ListOptions.GetItem("detail_Image").Visible = True
		ListOptions.GetItem("detail_Image").OnLeft = True
		ListOptions.Add("detail_Article")
		ListOptions.GetItem("detail_Article").CssStyle = "white-space: nowrap;"
		ListOptions.GetItem("detail_Article").Visible = True
		ListOptions.GetItem("detail_Article").OnLeft = True
		ListOptions.Add("detail_PageAlias")
		ListOptions.GetItem("detail_PageAlias").CssStyle = "white-space: nowrap;"
		ListOptions.GetItem("detail_PageAlias").Visible = True
		ListOptions.GetItem("detail_PageAlias").OnLeft = True
		ListOptions.Add("detail_Link")
		ListOptions.GetItem("detail_Link").CssStyle = "white-space: nowrap;"
		ListOptions.GetItem("detail_Link").Visible = True
		ListOptions.GetItem("detail_Link").OnLeft = True
		ListOptions_Load()
		If Company.Export <> "" Or Company.CurrentAction = "gridadd" Or Company.CurrentAction = "gridedit" Then
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
			oListOpt = ListOptions.GetItem("detail_zPage")
			oListOpt.Body = "<img src=""images/detail.gif"" alt=""" & ew_HtmlEncode(Language.Phrase("DetailLink")) & """ title=""" & ew_HtmlEncode(Language.Phrase("DetailLink")) & """ width=""16"" height=""16"" border=""0"" />" & Language.TablePhrase("zPage", "TblCaption")
			oListOpt.Body = "<a href=""zpage_list.aspx?" & EW_TABLE_SHOW_MASTER & "=Company&CompanyID=" & HttpContext.Current.Server.UrlEncode(Convert.ToString(Company.CompanyID.CurrentValue)) & """>" & oListOpt.Body & "</a>"
		End If
		If True Then
			oListOpt = ListOptions.GetItem("detail_Image")
			oListOpt.Body = "<img src=""images/detail.gif"" alt=""" & ew_HtmlEncode(Language.Phrase("DetailLink")) & """ title=""" & ew_HtmlEncode(Language.Phrase("DetailLink")) & """ width=""16"" height=""16"" border=""0"" />" & Language.TablePhrase("Image", "TblCaption")
			oListOpt.Body = "<a href=""image_list.aspx?" & EW_TABLE_SHOW_MASTER & "=Company&CompanyID=" & HttpContext.Current.Server.UrlEncode(Convert.ToString(Company.CompanyID.CurrentValue)) & """>" & oListOpt.Body & "</a>"
		End If
		If True Then
			oListOpt = ListOptions.GetItem("detail_Article")
			oListOpt.Body = "<img src=""images/detail.gif"" alt=""" & ew_HtmlEncode(Language.Phrase("DetailLink")) & """ title=""" & ew_HtmlEncode(Language.Phrase("DetailLink")) & """ width=""16"" height=""16"" border=""0"" />" & Language.TablePhrase("Article", "TblCaption")
			oListOpt.Body = "<a href=""article_list.aspx?" & EW_TABLE_SHOW_MASTER & "=Company&CompanyID=" & HttpContext.Current.Server.UrlEncode(Convert.ToString(Company.CompanyID.CurrentValue)) & """>" & oListOpt.Body & "</a>"
		End If
		If True Then
			oListOpt = ListOptions.GetItem("detail_PageAlias")
			oListOpt.Body = "<img src=""images/detail.gif"" alt=""" & ew_HtmlEncode(Language.Phrase("DetailLink")) & """ title=""" & ew_HtmlEncode(Language.Phrase("DetailLink")) & """ width=""16"" height=""16"" border=""0"" />" & Language.TablePhrase("PageAlias", "TblCaption")
			oListOpt.Body = "<a href=""pagealias_list.aspx?" & EW_TABLE_SHOW_MASTER & "=Company&CompanyID=" & HttpContext.Current.Server.UrlEncode(Convert.ToString(Company.CompanyID.CurrentValue)) & """>" & oListOpt.Body & "</a>"
		End If
		If True Then
			oListOpt = ListOptions.GetItem("detail_Link")
			oListOpt.Body = "<img src=""images/detail.gif"" alt=""" & ew_HtmlEncode(Language.Phrase("DetailLink")) & """ title=""" & ew_HtmlEncode(Language.Phrase("DetailLink")) & """ width=""16"" height=""16"" border=""0"" />" & Language.TablePhrase("Link", "TblCaption")
			oListOpt.Body = "<a href=""link_list.aspx?" & EW_TABLE_SHOW_MASTER & "=Company&CompanyID=" & HttpContext.Current.Server.UrlEncode(Convert.ToString(Company.CompanyID.CurrentValue)) & """>" & oListOpt.Body & "</a>"
		End If
		RenderListOptionsExt()
		ListOptions_Rendered()
	End Sub

	Sub RenderListOptionsExt()
		Dim sHyperLinkParm As String, oListOpt As cListOption
		sSqlWrk = "[CompanyID]=" & ew_AdjustSql(Company.CompanyID.CurrentValue) & ""
		sSqlWrk = cTEA.Encrypt(sSqlWrk, EW_RANDOM_KEY)
		sSqlWrk = sSqlWrk.Replace("'", "\'")
		sHyperLinkParm = " href=""zpage_list.aspx?" & EW_TABLE_SHOW_MASTER & "=Company&CompanyID=" & HttpContext.Current.Server.URLEncode(Convert.ToString(Company.CompanyID.CurrentValue)) & """"
		oListOpt = ListOptions.GetItem("detail_zPage")
		oListOpt.Body = Language.TablePhrase("zPage", "TblCaption")
		oListOpt.Body &= "<img src=""images/detail.gif"" alt=""" & ew_HtmlEncode(Language.Phrase("DetailLink")) & """ title=""" & ew_HtmlEncode(Language.Phrase("DetailLink")) & """ width=""16"" height=""16"" border=""0"" />"
		oListOpt.Body = "<a" & sHyperLinkParm & ">" & oListOpt.Body & "</a>"
		sHyperLinkParm = " href=""javascript:void(0);"" name=""ew_Company_zPage_DetailLink%i"" id=""ew_Company_zPage_DetailLink%i"" onclick=""ew_AjaxShowDetails2(event, this, 'zpage_preview.aspx?f=%s')"""
		sHyperLinkParm = sHyperLinkParm.Replace("%i", lRowCnt)
		sHyperLinkParm = sHyperLinkParm.Replace("%s", sSqlWrk)
		oListOpt.Body = "<a" & sHyperLinkParm & "><img class=""ewPreviewRowImage"" src=""images/expand.gif"" width=""9"" height=""9"" border=""0""></a>&nbsp;" & oListOpt.Body
		sSqlWrk = "[CompanyID]=" & ew_AdjustSql(Company.CompanyID.CurrentValue) & ""
		sSqlWrk = cTEA.Encrypt(sSqlWrk, EW_RANDOM_KEY)
		sSqlWrk = sSqlWrk.Replace("'", "\'")
		sHyperLinkParm = " href=""image_list.aspx?" & EW_TABLE_SHOW_MASTER & "=Company&CompanyID=" & HttpContext.Current.Server.URLEncode(Convert.ToString(Company.CompanyID.CurrentValue)) & """"
		oListOpt = ListOptions.GetItem("detail_Image")
		oListOpt.Body = Language.TablePhrase("Image", "TblCaption")
		oListOpt.Body &= "<img src=""images/detail.gif"" alt=""" & ew_HtmlEncode(Language.Phrase("DetailLink")) & """ title=""" & ew_HtmlEncode(Language.Phrase("DetailLink")) & """ width=""16"" height=""16"" border=""0"" />"
		oListOpt.Body = "<a" & sHyperLinkParm & ">" & oListOpt.Body & "</a>"
		sHyperLinkParm = " href=""javascript:void(0);"" name=""ew_Company_Image_DetailLink%i"" id=""ew_Company_Image_DetailLink%i"" onclick=""ew_AjaxShowDetails2(event, this, 'image_preview.aspx?f=%s')"""
		sHyperLinkParm = sHyperLinkParm.Replace("%i", lRowCnt)
		sHyperLinkParm = sHyperLinkParm.Replace("%s", sSqlWrk)
		oListOpt.Body = "<a" & sHyperLinkParm & "><img class=""ewPreviewRowImage"" src=""images/expand.gif"" width=""9"" height=""9"" border=""0""></a>&nbsp;" & oListOpt.Body
		sSqlWrk = "[CompanyID]=" & ew_AdjustSql(Company.CompanyID.CurrentValue) & ""
		sSqlWrk = cTEA.Encrypt(sSqlWrk, EW_RANDOM_KEY)
		sSqlWrk = sSqlWrk.Replace("'", "\'")
		sHyperLinkParm = " href=""article_list.aspx?" & EW_TABLE_SHOW_MASTER & "=Company&CompanyID=" & HttpContext.Current.Server.URLEncode(Convert.ToString(Company.CompanyID.CurrentValue)) & """"
		oListOpt = ListOptions.GetItem("detail_Article")
		oListOpt.Body = Language.TablePhrase("Article", "TblCaption")
		oListOpt.Body &= "<img src=""images/detail.gif"" alt=""" & ew_HtmlEncode(Language.Phrase("DetailLink")) & """ title=""" & ew_HtmlEncode(Language.Phrase("DetailLink")) & """ width=""16"" height=""16"" border=""0"" />"
		oListOpt.Body = "<a" & sHyperLinkParm & ">" & oListOpt.Body & "</a>"
		sHyperLinkParm = " href=""javascript:void(0);"" name=""ew_Company_Article_DetailLink%i"" id=""ew_Company_Article_DetailLink%i"" onclick=""ew_AjaxShowDetails2(event, this, 'article_preview.aspx?f=%s')"""
		sHyperLinkParm = sHyperLinkParm.Replace("%i", lRowCnt)
		sHyperLinkParm = sHyperLinkParm.Replace("%s", sSqlWrk)
		oListOpt.Body = "<a" & sHyperLinkParm & "><img class=""ewPreviewRowImage"" src=""images/expand.gif"" width=""9"" height=""9"" border=""0""></a>&nbsp;" & oListOpt.Body
		sSqlWrk = "[CompanyID]=" & ew_AdjustSql(Company.CompanyID.CurrentValue) & ""
		sSqlWrk = cTEA.Encrypt(sSqlWrk, EW_RANDOM_KEY)
		sSqlWrk = sSqlWrk.Replace("'", "\'")
		sHyperLinkParm = " href=""pagealias_list.aspx?" & EW_TABLE_SHOW_MASTER & "=Company&CompanyID=" & HttpContext.Current.Server.URLEncode(Convert.ToString(Company.CompanyID.CurrentValue)) & """"
		oListOpt = ListOptions.GetItem("detail_PageAlias")
		oListOpt.Body = Language.TablePhrase("PageAlias", "TblCaption")
		oListOpt.Body &= "<img src=""images/detail.gif"" alt=""" & ew_HtmlEncode(Language.Phrase("DetailLink")) & """ title=""" & ew_HtmlEncode(Language.Phrase("DetailLink")) & """ width=""16"" height=""16"" border=""0"" />"
		oListOpt.Body = "<a" & sHyperLinkParm & ">" & oListOpt.Body & "</a>"
		sHyperLinkParm = " href=""javascript:void(0);"" name=""ew_Company_PageAlias_DetailLink%i"" id=""ew_Company_PageAlias_DetailLink%i"" onclick=""ew_AjaxShowDetails2(event, this, 'pagealias_preview.aspx?f=%s')"""
		sHyperLinkParm = sHyperLinkParm.Replace("%i", lRowCnt)
		sHyperLinkParm = sHyperLinkParm.Replace("%s", sSqlWrk)
		oListOpt.Body = "<a" & sHyperLinkParm & "><img class=""ewPreviewRowImage"" src=""images/expand.gif"" width=""9"" height=""9"" border=""0""></a>&nbsp;" & oListOpt.Body
		sSqlWrk = "[CompanyID]=" & ew_AdjustSql(Company.CompanyID.CurrentValue) & ""
		sSqlWrk = cTEA.Encrypt(sSqlWrk, EW_RANDOM_KEY)
		sSqlWrk = sSqlWrk.Replace("'", "\'")
		sHyperLinkParm = " href=""link_list.aspx?" & EW_TABLE_SHOW_MASTER & "=Company&CompanyID=" & HttpContext.Current.Server.URLEncode(Convert.ToString(Company.CompanyID.CurrentValue)) & """"
		oListOpt = ListOptions.GetItem("detail_Link")
		oListOpt.Body = Language.TablePhrase("Link", "TblCaption")
		oListOpt.Body &= "<img src=""images/detail.gif"" alt=""" & ew_HtmlEncode(Language.Phrase("DetailLink")) & """ title=""" & ew_HtmlEncode(Language.Phrase("DetailLink")) & """ width=""16"" height=""16"" border=""0"" />"
		oListOpt.Body = "<a" & sHyperLinkParm & ">" & oListOpt.Body & "</a>"
		sHyperLinkParm = " href=""javascript:void(0);"" name=""ew_Company_Link_DetailLink%i"" id=""ew_Company_Link_DetailLink%i"" onclick=""ew_AjaxShowDetails2(event, this, 'link_preview.aspx?f=%s')"""
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
				Company.StartRecordNumber = lStartRec
			ElseIf ew_Get(EW_TABLE_PAGE_NO) <> "" AndAlso IsNumeric(ew_Get(EW_TABLE_PAGE_NO)) Then
				nPageNo = ew_ConvertToInt(ew_Get(EW_TABLE_PAGE_NO))
				lStartRec = (nPageNo-1)*lDisplayRecs+1
				If lStartRec <= 0 Then
					lStartRec = 1
				ElseIf lStartRec >= ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 Then
					lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1
				End If
				Company.StartRecordNumber = lStartRec
			End If
		End If
		lStartRec = Company.StartRecordNumber

		' Check if correct start record counter
		If lStartRec <= 0 Then ' Avoid invalid start record counter
			lStartRec = 1 ' Reset start record counter
			Company.StartRecordNumber = lStartRec
		ElseIf lStartRec > lTotalRecs Then ' Avoid starting record > total records
			lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to last page first record
			Company.StartRecordNumber = lStartRec
		ElseIf (lStartRec-1) Mod lDisplayRecs <> 0 Then
			lStartRec = ((lStartRec-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to page boundary
			Company.StartRecordNumber = lStartRec
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
		Company.CompanyID.AdvancedSearch.SearchValue = ew_Get("x_CompanyID")
    	Company.CompanyID.AdvancedSearch.SearchOperator = ew_Get("z_CompanyID")
		Company.CompanyName.AdvancedSearch.SearchValue = ew_Get("x_CompanyName")
    	Company.CompanyName.AdvancedSearch.SearchOperator = ew_Get("z_CompanyName")
		Company.SiteCategoryTypeID.AdvancedSearch.SearchValue = ew_Get("x_SiteCategoryTypeID")
    	Company.SiteCategoryTypeID.AdvancedSearch.SearchOperator = ew_Get("z_SiteCategoryTypeID")
		Company.SiteTitle.AdvancedSearch.SearchValue = ew_Get("x_SiteTitle")
    	Company.SiteTitle.AdvancedSearch.SearchOperator = ew_Get("z_SiteTitle")
		Company.SiteURL.AdvancedSearch.SearchValue = ew_Get("x_SiteURL")
    	Company.SiteURL.AdvancedSearch.SearchOperator = ew_Get("z_SiteURL")
		Company.GalleryFolder.AdvancedSearch.SearchValue = ew_Get("x_GalleryFolder")
    	Company.GalleryFolder.AdvancedSearch.SearchOperator = ew_Get("z_GalleryFolder")
		Company.HomePageID.AdvancedSearch.SearchValue = ew_Get("x_HomePageID")
    	Company.HomePageID.AdvancedSearch.SearchOperator = ew_Get("z_HomePageID")
		Company.DefaultArticleID.AdvancedSearch.SearchValue = ew_Get("x_DefaultArticleID")
    	Company.DefaultArticleID.AdvancedSearch.SearchOperator = ew_Get("z_DefaultArticleID")
		Company.SiteTemplate.AdvancedSearch.SearchValue = ew_Get("x_SiteTemplate")
    	Company.SiteTemplate.AdvancedSearch.SearchOperator = ew_Get("z_SiteTemplate")
		Company.DefaultSiteTemplate.AdvancedSearch.SearchValue = ew_Get("x_DefaultSiteTemplate")
    	Company.DefaultSiteTemplate.AdvancedSearch.SearchOperator = ew_Get("z_DefaultSiteTemplate")
		Company.Address.AdvancedSearch.SearchValue = ew_Get("x_Address")
    	Company.Address.AdvancedSearch.SearchOperator = ew_Get("z_Address")
		Company.City.AdvancedSearch.SearchValue = ew_Get("x_City")
    	Company.City.AdvancedSearch.SearchOperator = ew_Get("z_City")
		Company.StateOrProvince.AdvancedSearch.SearchValue = ew_Get("x_StateOrProvince")
    	Company.StateOrProvince.AdvancedSearch.SearchOperator = ew_Get("z_StateOrProvince")
		Company.PostalCode.AdvancedSearch.SearchValue = ew_Get("x_PostalCode")
    	Company.PostalCode.AdvancedSearch.SearchOperator = ew_Get("z_PostalCode")
		Company.Country.AdvancedSearch.SearchValue = ew_Get("x_Country")
    	Company.Country.AdvancedSearch.SearchOperator = ew_Get("z_Country")
		Company.PhoneNumber.AdvancedSearch.SearchValue = ew_Get("x_PhoneNumber")
    	Company.PhoneNumber.AdvancedSearch.SearchOperator = ew_Get("z_PhoneNumber")
		Company.FaxNumber.AdvancedSearch.SearchValue = ew_Get("x_FaxNumber")
    	Company.FaxNumber.AdvancedSearch.SearchOperator = ew_Get("z_FaxNumber")
		Company.DefaultPaymentTerms.AdvancedSearch.SearchValue = ew_Get("x_DefaultPaymentTerms")
    	Company.DefaultPaymentTerms.AdvancedSearch.SearchOperator = ew_Get("z_DefaultPaymentTerms")
		Company.DefaultInvoiceDescription.AdvancedSearch.SearchValue = ew_Get("x_DefaultInvoiceDescription")
    	Company.DefaultInvoiceDescription.AdvancedSearch.SearchOperator = ew_Get("z_DefaultInvoiceDescription")
		Company.Component.AdvancedSearch.SearchValue = ew_Get("x_Component")
    	Company.Component.AdvancedSearch.SearchOperator = ew_Get("z_Component")
		Company.FromEmail.AdvancedSearch.SearchValue = ew_Get("x_FromEmail")
    	Company.FromEmail.AdvancedSearch.SearchOperator = ew_Get("z_FromEmail")
		Company.SMTP.AdvancedSearch.SearchValue = ew_Get("x_SMTP")
    	Company.SMTP.AdvancedSearch.SearchOperator = ew_Get("z_SMTP")
		Company.ActiveFL.AdvancedSearch.SearchValue = ew_Get("x_ActiveFL")
    	Company.ActiveFL.AdvancedSearch.SearchOperator = ew_Get("z_ActiveFL")
		Company.UseBreadCrumbURL.AdvancedSearch.SearchValue = ew_Get("x_UseBreadCrumbURL")
    	Company.UseBreadCrumbURL.AdvancedSearch.SearchOperator = ew_Get("z_UseBreadCrumbURL")
		Company.SingleSiteGallery.AdvancedSearch.SearchValue = ew_Get("x_SingleSiteGallery")
    	Company.SingleSiteGallery.AdvancedSearch.SearchOperator = ew_Get("z_SingleSiteGallery")
	End Sub

	'
	' Load recordset
	'
	Function LoadRecordset() As OleDbDataReader

		' Recordset Selecting event
		Company.Recordset_Selecting(Company.CurrentFilter)

		' Load list page SQL
		Dim sSql As String = Company.ListSQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then DebugMsg = sSql

		' Count
		lTotalRecs = -1		
		Try
			Dim sCntSql As String = Company.SelectCountSQL

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
		Company.Recordset_Selected(Rs)
		Return Rs
	End Function

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = Company.KeyFilter

		' Row Selecting event
		Company.Row_Selecting(sFilter)

		' Load SQL based on filter
		Company.CurrentFilter = sFilter
		Dim sSql As String = Company.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then DebugMsg = sSql
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				Company.Row_Selected(RsRow)
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
		Company.CompanyID.DbValue = RsRow("CompanyID")
		Company.CompanyName.DbValue = RsRow("CompanyName")
		Company.SiteCategoryTypeID.DbValue = RsRow("SiteCategoryTypeID")
		Company.SiteTitle.DbValue = RsRow("SiteTitle")
		Company.SiteURL.DbValue = RsRow("SiteURL")
		Company.GalleryFolder.DbValue = RsRow("GalleryFolder")
		Company.HomePageID.DbValue = RsRow("HomePageID")
		Company.DefaultArticleID.DbValue = RsRow("DefaultArticleID")
		Company.SiteTemplate.DbValue = RsRow("SiteTemplate")
		Company.DefaultSiteTemplate.DbValue = RsRow("DefaultSiteTemplate")
		Company.Address.DbValue = RsRow("Address")
		Company.City.DbValue = RsRow("City")
		Company.StateOrProvince.DbValue = RsRow("StateOrProvince")
		Company.PostalCode.DbValue = RsRow("PostalCode")
		Company.Country.DbValue = RsRow("Country")
		Company.PhoneNumber.DbValue = RsRow("PhoneNumber")
		Company.FaxNumber.DbValue = RsRow("FaxNumber")
		Company.DefaultPaymentTerms.DbValue = RsRow("DefaultPaymentTerms")
		Company.DefaultInvoiceDescription.DbValue = RsRow("DefaultInvoiceDescription")
		Company.Component.DbValue = RsRow("Component")
		Company.FromEmail.DbValue = RsRow("FromEmail")
		Company.SMTP.DbValue = RsRow("SMTP")
		Company.ActiveFL.DbValue = IIf(ew_ConvertToBool(RsRow("ActiveFL")), "1", "0")
		Company.UseBreadCrumbURL.DbValue = IIf(ew_ConvertToBool(RsRow("UseBreadCrumbURL")), "1", "0")
		Company.SingleSiteGallery.DbValue = IIf(ew_ConvertToBool(RsRow("SingleSiteGallery")), "1", "0")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Initialize urls
		ViewUrl = Company.ViewUrl
		EditUrl = Company.EditUrl
		InlineEditUrl = Company.InlineEditUrl
		CopyUrl = Company.CopyUrl
		InlineCopyUrl = Company.InlineCopyUrl
		DeleteUrl = Company.DeleteUrl

		' Row Rendering event
		Company.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' CompanyName

		Company.CompanyName.CellCssStyle = ""
		Company.CompanyName.CellCssClass = ""
		Company.CompanyName.CellAttrs.Clear(): Company.CompanyName.ViewAttrs.Clear(): Company.CompanyName.EditAttrs.Clear()

		' SiteTitle
		Company.SiteTitle.CellCssStyle = ""
		Company.SiteTitle.CellCssClass = ""
		Company.SiteTitle.CellAttrs.Clear(): Company.SiteTitle.ViewAttrs.Clear(): Company.SiteTitle.EditAttrs.Clear()

		' GalleryFolder
		Company.GalleryFolder.CellCssStyle = ""
		Company.GalleryFolder.CellCssClass = ""
		Company.GalleryFolder.CellAttrs.Clear(): Company.GalleryFolder.ViewAttrs.Clear(): Company.GalleryFolder.EditAttrs.Clear()

		' SiteTemplate
		Company.SiteTemplate.CellCssStyle = ""
		Company.SiteTemplate.CellCssClass = ""
		Company.SiteTemplate.CellAttrs.Clear(): Company.SiteTemplate.ViewAttrs.Clear(): Company.SiteTemplate.EditAttrs.Clear()

		' DefaultSiteTemplate
		Company.DefaultSiteTemplate.CellCssStyle = ""
		Company.DefaultSiteTemplate.CellCssClass = ""
		Company.DefaultSiteTemplate.CellAttrs.Clear(): Company.DefaultSiteTemplate.ViewAttrs.Clear(): Company.DefaultSiteTemplate.EditAttrs.Clear()

		'
		'  View  Row
		'

		If Company.RowType = EW_ROWTYPE_VIEW Then ' View row

			' CompanyID
			Company.CompanyID.ViewValue = Company.CompanyID.CurrentValue
			Company.CompanyID.CssStyle = ""
			Company.CompanyID.CssClass = ""
			Company.CompanyID.ViewCustomAttributes = ""

			' CompanyName
			Company.CompanyName.ViewValue = Company.CompanyName.CurrentValue
			Company.CompanyName.CssStyle = ""
			Company.CompanyName.CssClass = ""
			Company.CompanyName.ViewCustomAttributes = ""

			' SiteCategoryTypeID
			If ew_NotEmpty(Company.SiteCategoryTypeID.CurrentValue) Then
				sFilterWrk = "[SiteCategoryTypeID] = " & ew_AdjustSql(Company.SiteCategoryTypeID.CurrentValue) & ""
			sSqlWrk = "SELECT [SiteCategoryTypeNM] FROM [SiteCategoryType]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [SiteCategoryTypeNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Company.SiteCategoryTypeID.ViewValue = RsWrk("SiteCategoryTypeNM")
				Else
					Company.SiteCategoryTypeID.ViewValue = Company.SiteCategoryTypeID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Company.SiteCategoryTypeID.ViewValue = System.DBNull.Value
			End If
			Company.SiteCategoryTypeID.CssStyle = ""
			Company.SiteCategoryTypeID.CssClass = ""
			Company.SiteCategoryTypeID.ViewCustomAttributes = ""

			' SiteTitle
			Company.SiteTitle.ViewValue = Company.SiteTitle.CurrentValue
			Company.SiteTitle.CssStyle = ""
			Company.SiteTitle.CssClass = ""
			Company.SiteTitle.ViewCustomAttributes = ""

			' SiteURL
			Company.SiteURL.ViewValue = Company.SiteURL.CurrentValue
			Company.SiteURL.CssStyle = ""
			Company.SiteURL.CssClass = ""
			Company.SiteURL.ViewCustomAttributes = ""

			' GalleryFolder
			Company.GalleryFolder.ViewValue = Company.GalleryFolder.CurrentValue
			Company.GalleryFolder.CssStyle = ""
			Company.GalleryFolder.CssClass = ""
			Company.GalleryFolder.ViewCustomAttributes = ""

			' HomePageID
			If ew_NotEmpty(Company.HomePageID.CurrentValue) Then
				sFilterWrk = "[PageID] = " & ew_AdjustSql(Company.HomePageID.CurrentValue) & ""
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
					Company.HomePageID.ViewValue = RsWrk("PageName")
				Else
					Company.HomePageID.ViewValue = Company.HomePageID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Company.HomePageID.ViewValue = System.DBNull.Value
			End If
			Company.HomePageID.CssStyle = ""
			Company.HomePageID.CssClass = ""
			Company.HomePageID.ViewCustomAttributes = ""

			' DefaultArticleID
			If ew_NotEmpty(Company.DefaultArticleID.CurrentValue) Then
				sFilterWrk = "[ArticleID] = " & ew_AdjustSql(Company.DefaultArticleID.CurrentValue) & ""
			sSqlWrk = "SELECT [Title] FROM [Article]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [Title] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Company.DefaultArticleID.ViewValue = RsWrk("Title")
				Else
					Company.DefaultArticleID.ViewValue = Company.DefaultArticleID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Company.DefaultArticleID.ViewValue = System.DBNull.Value
			End If
			Company.DefaultArticleID.CssStyle = ""
			Company.DefaultArticleID.CssClass = ""
			Company.DefaultArticleID.ViewCustomAttributes = ""

			' SiteTemplate
			If ew_NotEmpty(Company.SiteTemplate.CurrentValue) Then
				sFilterWrk = "[TemplatePrefix] = '" & ew_AdjustSql(Company.SiteTemplate.CurrentValue) & "'"
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
					Company.SiteTemplate.ViewValue = RsWrk("Name")
				Else
					Company.SiteTemplate.ViewValue = Company.SiteTemplate.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Company.SiteTemplate.ViewValue = System.DBNull.Value
			End If
			Company.SiteTemplate.CssStyle = ""
			Company.SiteTemplate.CssClass = ""
			Company.SiteTemplate.ViewCustomAttributes = ""

			' DefaultSiteTemplate
			If ew_NotEmpty(Company.DefaultSiteTemplate.CurrentValue) Then
				sFilterWrk = "[TemplatePrefix] = '" & ew_AdjustSql(Company.DefaultSiteTemplate.CurrentValue) & "'"
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
					Company.DefaultSiteTemplate.ViewValue = RsWrk("Name")
				Else
					Company.DefaultSiteTemplate.ViewValue = Company.DefaultSiteTemplate.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Company.DefaultSiteTemplate.ViewValue = System.DBNull.Value
			End If
			Company.DefaultSiteTemplate.CssStyle = ""
			Company.DefaultSiteTemplate.CssClass = ""
			Company.DefaultSiteTemplate.ViewCustomAttributes = ""

			' Address
			Company.Address.ViewValue = Company.Address.CurrentValue
			Company.Address.CssStyle = ""
			Company.Address.CssClass = ""
			Company.Address.ViewCustomAttributes = ""

			' City
			Company.City.ViewValue = Company.City.CurrentValue
			Company.City.CssStyle = ""
			Company.City.CssClass = ""
			Company.City.ViewCustomAttributes = ""

			' StateOrProvince
			Company.StateOrProvince.ViewValue = Company.StateOrProvince.CurrentValue
			Company.StateOrProvince.CssStyle = ""
			Company.StateOrProvince.CssClass = ""
			Company.StateOrProvince.ViewCustomAttributes = ""

			' PostalCode
			Company.PostalCode.ViewValue = Company.PostalCode.CurrentValue
			Company.PostalCode.CssStyle = ""
			Company.PostalCode.CssClass = ""
			Company.PostalCode.ViewCustomAttributes = ""

			' Country
			Company.Country.ViewValue = Company.Country.CurrentValue
			Company.Country.CssStyle = ""
			Company.Country.CssClass = ""
			Company.Country.ViewCustomAttributes = ""

			' PhoneNumber
			Company.PhoneNumber.ViewValue = Company.PhoneNumber.CurrentValue
			Company.PhoneNumber.CssStyle = ""
			Company.PhoneNumber.CssClass = ""
			Company.PhoneNumber.ViewCustomAttributes = ""

			' FaxNumber
			Company.FaxNumber.ViewValue = Company.FaxNumber.CurrentValue
			Company.FaxNumber.CssStyle = ""
			Company.FaxNumber.CssClass = ""
			Company.FaxNumber.ViewCustomAttributes = ""

			' DefaultPaymentTerms
			Company.DefaultPaymentTerms.ViewValue = Company.DefaultPaymentTerms.CurrentValue
			Company.DefaultPaymentTerms.CssStyle = ""
			Company.DefaultPaymentTerms.CssClass = ""
			Company.DefaultPaymentTerms.ViewCustomAttributes = ""

			' DefaultInvoiceDescription
			Company.DefaultInvoiceDescription.ViewValue = Company.DefaultInvoiceDescription.CurrentValue
			Company.DefaultInvoiceDescription.CssStyle = ""
			Company.DefaultInvoiceDescription.CssClass = ""
			Company.DefaultInvoiceDescription.ViewCustomAttributes = ""

			' Component
			Company.Component.ViewValue = Company.Component.CurrentValue
			Company.Component.CssStyle = ""
			Company.Component.CssClass = ""
			Company.Component.ViewCustomAttributes = ""

			' FromEmail
			Company.FromEmail.ViewValue = Company.FromEmail.CurrentValue
			Company.FromEmail.CssStyle = ""
			Company.FromEmail.CssClass = ""
			Company.FromEmail.ViewCustomAttributes = ""

			' SMTP
			Company.SMTP.ViewValue = Company.SMTP.CurrentValue
			Company.SMTP.CssStyle = ""
			Company.SMTP.CssClass = ""
			Company.SMTP.ViewCustomAttributes = ""

			' ActiveFL
			If Convert.ToString(Company.ActiveFL.CurrentValue) = "1" Then
				Company.ActiveFL.ViewValue = "Yes"
			Else
				Company.ActiveFL.ViewValue = "No"
			End If
			Company.ActiveFL.CssStyle = ""
			Company.ActiveFL.CssClass = ""
			Company.ActiveFL.ViewCustomAttributes = ""

			' UseBreadCrumbURL
			If Convert.ToString(Company.UseBreadCrumbURL.CurrentValue) = "1" Then
				Company.UseBreadCrumbURL.ViewValue = "Yes"
			Else
				Company.UseBreadCrumbURL.ViewValue = "No"
			End If
			Company.UseBreadCrumbURL.CssStyle = ""
			Company.UseBreadCrumbURL.CssClass = ""
			Company.UseBreadCrumbURL.ViewCustomAttributes = ""

			' SingleSiteGallery
			If Convert.ToString(Company.SingleSiteGallery.CurrentValue) = "1" Then
				Company.SingleSiteGallery.ViewValue = "Yes"
			Else
				Company.SingleSiteGallery.ViewValue = "No"
			End If
			Company.SingleSiteGallery.CssStyle = ""
			Company.SingleSiteGallery.CssClass = ""
			Company.SingleSiteGallery.ViewCustomAttributes = ""

			' View refer script
			' CompanyName

			Company.CompanyName.HrefValue = ""
			Company.CompanyName.TooltipValue = ""

			' SiteTitle
			Company.SiteTitle.HrefValue = ""
			Company.SiteTitle.TooltipValue = ""

			' GalleryFolder
			Company.GalleryFolder.HrefValue = ""
			Company.GalleryFolder.TooltipValue = ""

			' SiteTemplate
			Company.SiteTemplate.HrefValue = ""
			Company.SiteTemplate.TooltipValue = ""

			' DefaultSiteTemplate
			Company.DefaultSiteTemplate.HrefValue = ""
			Company.DefaultSiteTemplate.TooltipValue = ""
		End If

		' Row Rendered event
		If Company.RowType <> EW_ROWTYPE_AGGREGATEINIT Then
			Company.Row_Rendered()
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
		Company.CompanyID.AdvancedSearch.SearchValue = Company.GetAdvancedSearch("x_CompanyID")
		Company.CompanyName.AdvancedSearch.SearchValue = Company.GetAdvancedSearch("x_CompanyName")
		Company.SiteCategoryTypeID.AdvancedSearch.SearchValue = Company.GetAdvancedSearch("x_SiteCategoryTypeID")
		Company.SiteTitle.AdvancedSearch.SearchValue = Company.GetAdvancedSearch("x_SiteTitle")
		Company.SiteURL.AdvancedSearch.SearchValue = Company.GetAdvancedSearch("x_SiteURL")
		Company.GalleryFolder.AdvancedSearch.SearchValue = Company.GetAdvancedSearch("x_GalleryFolder")
		Company.HomePageID.AdvancedSearch.SearchValue = Company.GetAdvancedSearch("x_HomePageID")
		Company.DefaultArticleID.AdvancedSearch.SearchValue = Company.GetAdvancedSearch("x_DefaultArticleID")
		Company.SiteTemplate.AdvancedSearch.SearchValue = Company.GetAdvancedSearch("x_SiteTemplate")
		Company.DefaultSiteTemplate.AdvancedSearch.SearchValue = Company.GetAdvancedSearch("x_DefaultSiteTemplate")
		Company.Address.AdvancedSearch.SearchValue = Company.GetAdvancedSearch("x_Address")
		Company.City.AdvancedSearch.SearchValue = Company.GetAdvancedSearch("x_City")
		Company.StateOrProvince.AdvancedSearch.SearchValue = Company.GetAdvancedSearch("x_StateOrProvince")
		Company.PostalCode.AdvancedSearch.SearchValue = Company.GetAdvancedSearch("x_PostalCode")
		Company.Country.AdvancedSearch.SearchValue = Company.GetAdvancedSearch("x_Country")
		Company.PhoneNumber.AdvancedSearch.SearchValue = Company.GetAdvancedSearch("x_PhoneNumber")
		Company.FaxNumber.AdvancedSearch.SearchValue = Company.GetAdvancedSearch("x_FaxNumber")
		Company.DefaultPaymentTerms.AdvancedSearch.SearchValue = Company.GetAdvancedSearch("x_DefaultPaymentTerms")
		Company.DefaultInvoiceDescription.AdvancedSearch.SearchValue = Company.GetAdvancedSearch("x_DefaultInvoiceDescription")
		Company.Component.AdvancedSearch.SearchValue = Company.GetAdvancedSearch("x_Component")
		Company.FromEmail.AdvancedSearch.SearchValue = Company.GetAdvancedSearch("x_FromEmail")
		Company.SMTP.AdvancedSearch.SearchValue = Company.GetAdvancedSearch("x_SMTP")
		Company.ActiveFL.AdvancedSearch.SearchValue = Company.GetAdvancedSearch("x_ActiveFL")
		Company.UseBreadCrumbURL.AdvancedSearch.SearchValue = Company.GetAdvancedSearch("x_UseBreadCrumbURL")
		Company.SingleSiteGallery.AdvancedSearch.SearchValue = Company.GetAdvancedSearch("x_SingleSiteGallery")
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
		If Company.ExportAll Then
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
		If Company.Export = "xml" Then
			oXmlDoc = New cXMLDocument()
		Else
			sOutputStr = ew_ExportHeader(Company.Export)

			' Horizontal format, write header
			If sExportStyle <> "v" OrElse Company.Export = "csv" Then
				sExportStr = ""
				ew_ExportAddValue(sExportStr, Company.CompanyID.ExportCaption, Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.CompanyID.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Company.CompanyName.ExportCaption, Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.CompanyName.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Company.SiteCategoryTypeID.ExportCaption, Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.SiteCategoryTypeID.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Company.SiteTitle.ExportCaption, Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.SiteTitle.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Company.SiteURL.ExportCaption, Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.SiteURL.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Company.GalleryFolder.ExportCaption, Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.GalleryFolder.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Company.HomePageID.ExportCaption, Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.HomePageID.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Company.DefaultArticleID.ExportCaption, Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.DefaultArticleID.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Company.SiteTemplate.ExportCaption, Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.SiteTemplate.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Company.DefaultSiteTemplate.ExportCaption, Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.DefaultSiteTemplate.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Company.Address.ExportCaption, Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.Address.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Company.City.ExportCaption, Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.City.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Company.StateOrProvince.ExportCaption, Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.StateOrProvince.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Company.PostalCode.ExportCaption, Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.PostalCode.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Company.Country.ExportCaption, Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.Country.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Company.PhoneNumber.ExportCaption, Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.PhoneNumber.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Company.FaxNumber.ExportCaption, Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.FaxNumber.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Company.DefaultPaymentTerms.ExportCaption, Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.DefaultPaymentTerms.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Company.DefaultInvoiceDescription.ExportCaption, Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.DefaultInvoiceDescription.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Company.Component.ExportCaption, Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.Component.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Company.FromEmail.ExportCaption, Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.FromEmail.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Company.SMTP.ExportCaption, Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.SMTP.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Company.ActiveFL.ExportCaption, Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.ActiveFL.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Company.UseBreadCrumbURL.ExportCaption, Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.UseBreadCrumbURL.CellStyles, ""))
				ew_ExportAddValue(sExportStr, Company.SingleSiteGallery.ExportCaption, Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.SingleSiteGallery.CellStyles, ""))
				sOutputStr &= ew_ExportLine(sExportStr, Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.RowStyles, ""))
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
				Company.CssClass = ""
				Company.CssStyle = ""
				Company.RowType = EW_ROWTYPE_VIEW ' Render view
				RenderRow()
				If Company.Export = "xml" Then
					oXmlDoc.AddRow()
					oXmlDoc.AddField("CompanyID", Company.CompanyID.ExportValue(Company.Export, Company.ExportOriginalValue)) ' CompanyID
					oXmlDoc.AddField("CompanyName", Company.CompanyName.ExportValue(Company.Export, Company.ExportOriginalValue)) ' CompanyName
					oXmlDoc.AddField("SiteCategoryTypeID", Company.SiteCategoryTypeID.ExportValue(Company.Export, Company.ExportOriginalValue)) ' SiteCategoryTypeID
					oXmlDoc.AddField("SiteTitle", Company.SiteTitle.ExportValue(Company.Export, Company.ExportOriginalValue)) ' SiteTitle
					oXmlDoc.AddField("SiteURL", Company.SiteURL.ExportValue(Company.Export, Company.ExportOriginalValue)) ' SiteURL
					oXmlDoc.AddField("GalleryFolder", Company.GalleryFolder.ExportValue(Company.Export, Company.ExportOriginalValue)) ' GalleryFolder
					oXmlDoc.AddField("HomePageID", Company.HomePageID.ExportValue(Company.Export, Company.ExportOriginalValue)) ' HomePageID
					oXmlDoc.AddField("DefaultArticleID", Company.DefaultArticleID.ExportValue(Company.Export, Company.ExportOriginalValue)) ' DefaultArticleID
					oXmlDoc.AddField("SiteTemplate", Company.SiteTemplate.ExportValue(Company.Export, Company.ExportOriginalValue)) ' SiteTemplate
					oXmlDoc.AddField("DefaultSiteTemplate", Company.DefaultSiteTemplate.ExportValue(Company.Export, Company.ExportOriginalValue)) ' DefaultSiteTemplate
					oXmlDoc.AddField("Address", Company.Address.ExportValue(Company.Export, Company.ExportOriginalValue)) ' Address
					oXmlDoc.AddField("City", Company.City.ExportValue(Company.Export, Company.ExportOriginalValue)) ' City
					oXmlDoc.AddField("StateOrProvince", Company.StateOrProvince.ExportValue(Company.Export, Company.ExportOriginalValue)) ' StateOrProvince
					oXmlDoc.AddField("PostalCode", Company.PostalCode.ExportValue(Company.Export, Company.ExportOriginalValue)) ' PostalCode
					oXmlDoc.AddField("Country", Company.Country.ExportValue(Company.Export, Company.ExportOriginalValue)) ' Country
					oXmlDoc.AddField("PhoneNumber", Company.PhoneNumber.ExportValue(Company.Export, Company.ExportOriginalValue)) ' PhoneNumber
					oXmlDoc.AddField("FaxNumber", Company.FaxNumber.ExportValue(Company.Export, Company.ExportOriginalValue)) ' FaxNumber
					oXmlDoc.AddField("DefaultPaymentTerms", Company.DefaultPaymentTerms.ExportValue(Company.Export, Company.ExportOriginalValue)) ' DefaultPaymentTerms
					oXmlDoc.AddField("DefaultInvoiceDescription", Company.DefaultInvoiceDescription.ExportValue(Company.Export, Company.ExportOriginalValue)) ' DefaultInvoiceDescription
					oXmlDoc.AddField("Component", Company.Component.ExportValue(Company.Export, Company.ExportOriginalValue)) ' Component
					oXmlDoc.AddField("FromEmail", Company.FromEmail.ExportValue(Company.Export, Company.ExportOriginalValue)) ' FromEmail
					oXmlDoc.AddField("SMTP", Company.SMTP.ExportValue(Company.Export, Company.ExportOriginalValue)) ' SMTP
					oXmlDoc.AddField("ActiveFL", Company.ActiveFL.ExportValue(Company.Export, Company.ExportOriginalValue)) ' ActiveFL
					oXmlDoc.AddField("UseBreadCrumbURL", Company.UseBreadCrumbURL.ExportValue(Company.Export, Company.ExportOriginalValue)) ' UseBreadCrumbURL
					oXmlDoc.AddField("SingleSiteGallery", Company.SingleSiteGallery.ExportValue(Company.Export, Company.ExportOriginalValue)) ' SingleSiteGallery
				Else

					' Vertical format
					If sExportStyle = "v" AndAlso Company.Export <> "csv" Then
						sOutputStr &= ew_ExportField(Company.CompanyID.ExportCaption, Company.CompanyID.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.CompanyID.CellStyles, "")) ' CompanyID
						sOutputStr &= ew_ExportField(Company.CompanyName.ExportCaption, Company.CompanyName.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.CompanyName.CellStyles, "")) ' CompanyName
						sOutputStr &= ew_ExportField(Company.SiteCategoryTypeID.ExportCaption, Company.SiteCategoryTypeID.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.SiteCategoryTypeID.CellStyles, "")) ' SiteCategoryTypeID
						sOutputStr &= ew_ExportField(Company.SiteTitle.ExportCaption, Company.SiteTitle.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.SiteTitle.CellStyles, "")) ' SiteTitle
						sOutputStr &= ew_ExportField(Company.SiteURL.ExportCaption, Company.SiteURL.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.SiteURL.CellStyles, "")) ' SiteURL
						sOutputStr &= ew_ExportField(Company.GalleryFolder.ExportCaption, Company.GalleryFolder.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.GalleryFolder.CellStyles, "")) ' GalleryFolder
						sOutputStr &= ew_ExportField(Company.HomePageID.ExportCaption, Company.HomePageID.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.HomePageID.CellStyles, "")) ' HomePageID
						sOutputStr &= ew_ExportField(Company.DefaultArticleID.ExportCaption, Company.DefaultArticleID.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.DefaultArticleID.CellStyles, "")) ' DefaultArticleID
						sOutputStr &= ew_ExportField(Company.SiteTemplate.ExportCaption, Company.SiteTemplate.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.SiteTemplate.CellStyles, "")) ' SiteTemplate
						sOutputStr &= ew_ExportField(Company.DefaultSiteTemplate.ExportCaption, Company.DefaultSiteTemplate.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.DefaultSiteTemplate.CellStyles, "")) ' DefaultSiteTemplate
						sOutputStr &= ew_ExportField(Company.Address.ExportCaption, Company.Address.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.Address.CellStyles, "")) ' Address
						sOutputStr &= ew_ExportField(Company.City.ExportCaption, Company.City.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.City.CellStyles, "")) ' City
						sOutputStr &= ew_ExportField(Company.StateOrProvince.ExportCaption, Company.StateOrProvince.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.StateOrProvince.CellStyles, "")) ' StateOrProvince
						sOutputStr &= ew_ExportField(Company.PostalCode.ExportCaption, Company.PostalCode.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.PostalCode.CellStyles, "")) ' PostalCode
						sOutputStr &= ew_ExportField(Company.Country.ExportCaption, Company.Country.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.Country.CellStyles, "")) ' Country
						sOutputStr &= ew_ExportField(Company.PhoneNumber.ExportCaption, Company.PhoneNumber.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.PhoneNumber.CellStyles, "")) ' PhoneNumber
						sOutputStr &= ew_ExportField(Company.FaxNumber.ExportCaption, Company.FaxNumber.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.FaxNumber.CellStyles, "")) ' FaxNumber
						sOutputStr &= ew_ExportField(Company.DefaultPaymentTerms.ExportCaption, Company.DefaultPaymentTerms.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.DefaultPaymentTerms.CellStyles, "")) ' DefaultPaymentTerms
						sOutputStr &= ew_ExportField(Company.DefaultInvoiceDescription.ExportCaption, Company.DefaultInvoiceDescription.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.DefaultInvoiceDescription.CellStyles, "")) ' DefaultInvoiceDescription
						sOutputStr &= ew_ExportField(Company.Component.ExportCaption, Company.Component.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.Component.CellStyles, "")) ' Component
						sOutputStr &= ew_ExportField(Company.FromEmail.ExportCaption, Company.FromEmail.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.FromEmail.CellStyles, "")) ' FromEmail
						sOutputStr &= ew_ExportField(Company.SMTP.ExportCaption, Company.SMTP.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.SMTP.CellStyles, "")) ' SMTP
						sOutputStr &= ew_ExportField(Company.ActiveFL.ExportCaption, Company.ActiveFL.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.ActiveFL.CellStyles, "")) ' ActiveFL
						sOutputStr &= ew_ExportField(Company.UseBreadCrumbURL.ExportCaption, Company.UseBreadCrumbURL.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.UseBreadCrumbURL.CellStyles, "")) ' UseBreadCrumbURL
						sOutputStr &= ew_ExportField(Company.SingleSiteGallery.ExportCaption, Company.SingleSiteGallery.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.SingleSiteGallery.CellStyles, "")) ' SingleSiteGallery

					' Horizontal format
					Else
						sExportStr = ""
						ew_ExportAddValue(sExportStr, Company.CompanyID.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.CompanyID.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Company.CompanyName.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.CompanyName.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Company.SiteCategoryTypeID.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.SiteCategoryTypeID.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Company.SiteTitle.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.SiteTitle.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Company.SiteURL.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.SiteURL.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Company.GalleryFolder.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.GalleryFolder.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Company.HomePageID.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.HomePageID.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Company.DefaultArticleID.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.DefaultArticleID.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Company.SiteTemplate.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.SiteTemplate.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Company.DefaultSiteTemplate.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.DefaultSiteTemplate.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Company.Address.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.Address.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Company.City.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.City.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Company.StateOrProvince.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.StateOrProvince.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Company.PostalCode.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.PostalCode.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Company.Country.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.Country.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Company.PhoneNumber.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.PhoneNumber.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Company.FaxNumber.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.FaxNumber.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Company.DefaultPaymentTerms.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.DefaultPaymentTerms.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Company.DefaultInvoiceDescription.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.DefaultInvoiceDescription.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Company.Component.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.Component.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Company.FromEmail.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.FromEmail.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Company.SMTP.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.SMTP.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Company.ActiveFL.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.ActiveFL.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Company.UseBreadCrumbURL.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.UseBreadCrumbURL.CellStyles, ""))
						ew_ExportAddValue(sExportStr, Company.SingleSiteGallery.ExportValue(Company.Export, Company.ExportOriginalValue), Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.SingleSiteGallery.CellStyles, ""))
						sOutputStr &= ew_ExportLine(sExportStr, Company.Export, IIf(EW_EXPORT_CSS_STYLES, Company.RowStyles, ""))
					End If
				End If
			End If
		Loop

		' Close recordset
		Rs.Close()
		Rs.Dispose()
		If Company.Export = "xml" Then
			oXmlDoc.Output
		Else
			sOutputStr &= ew_ExportFooter(Company.Export)
			ew_Write(sOutputStr)
		End If
	End Sub

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "Company"
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
		Company_list = New cCompany_list(Me)		
		Company_list.Page_Init()		

		' Set buffer/cache option
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

		' Page main processing
		Company_list.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If Company_list IsNot Nothing Then Company_list.Dispose()
	End Sub
End Class
