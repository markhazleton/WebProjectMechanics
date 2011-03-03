Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class company_srch
	Inherits AspNetMaker8_wpmWebsite

	' Page object
	Public Company_search As cCompany_search

	'
	' Page Class
	'
	Class cCompany_search
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
		Public ReadOnly Property AspNetPage() As company_srch
			Get
				Return CType(m_ParentPage, company_srch)
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
			m_PageID = "search"
			m_PageObjName = "Company_search"
			m_PageObjTypeName = "cCompany_search"			

			' Initialize language object
			Language = New cLanguage(Me)

			' Table Name
			m_TableName = "Company"

			' Initialize table object
			Company = New cCompany(Me)

			' Initialize URLs
			' Connect to database

			Conn = New cConnection()
		End Sub

		'
		'  Subroutine Page_Init
		'  - called before page main
		'  - check Security
		'  - set up response header
		'  - call page load events
		'
		Public Sub Page_Init()

		' Create form object
		ObjForm = New cFormObj()

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
			ObjForm = Nothing

			' Go to URL if specified
			Dim sRedirectUrl As String = url
			Page_Redirecting(sRedirectUrl)
			If sRedirectUrl <> "" Then
				HttpContext.Current.Response.Clear()
				HttpContext.Current.Response.Redirect(sReDirectUrl)
			End If
		End Sub

	'
	' Page main processing
	'
	Sub Page_Main()
		If IsPageRequest Then ' Validate request

			' Get action
			Company.CurrentAction = ObjForm.GetValue("a_search")
			Select Case Company.CurrentAction
				Case "S" ' Get Search Criteria

					' Build search string for advanced search, remove blank field
					Dim sSrchStr As String
					LoadSearchValues() ' Get search values
					If ValidateSearch() Then
						sSrchStr = BuildAdvancedSearch()
					Else
						sSrchStr = ""
						Message = ParentPage.gsSearchError
					End If
					If sSrchStr <> "" Then
						sSrchStr = Company.UrlParm(sSrchStr)
					Page_Terminate("company_list.aspx" & "?" & sSrchStr) ' Go to list page
					End If
			End Select
		End If

		' Restore search settings from Session
		If ParentPage.gsSearchError = "" Then
			LoadAdvancedSearch()
		End If

		' Render row for search
		Company.RowType = EW_ROWTYPE_SEARCH
		RenderRow()
	End Sub

	'
	' Build advanced search
	'
	Function BuildAdvancedSearch() As String
		Dim sSrchUrl As String = ""
		BuildSearchUrl(sSrchUrl, Company.CompanyID) ' CompanyID
		BuildSearchUrl(sSrchUrl, Company.CompanyName) ' CompanyName
		BuildSearchUrl(sSrchUrl, Company.SiteCategoryTypeID) ' SiteCategoryTypeID
		BuildSearchUrl(sSrchUrl, Company.SiteTitle) ' SiteTitle
		BuildSearchUrl(sSrchUrl, Company.SiteURL) ' SiteURL
		BuildSearchUrl(sSrchUrl, Company.GalleryFolder) ' GalleryFolder
		BuildSearchUrl(sSrchUrl, Company.HomePageID) ' HomePageID
		BuildSearchUrl(sSrchUrl, Company.DefaultArticleID) ' DefaultArticleID
		BuildSearchUrl(sSrchUrl, Company.SiteTemplate) ' SiteTemplate
		BuildSearchUrl(sSrchUrl, Company.DefaultSiteTemplate) ' DefaultSiteTemplate
		BuildSearchUrl(sSrchUrl, Company.Address) ' Address
		BuildSearchUrl(sSrchUrl, Company.City) ' City
		BuildSearchUrl(sSrchUrl, Company.StateOrProvince) ' StateOrProvince
		BuildSearchUrl(sSrchUrl, Company.PostalCode) ' PostalCode
		BuildSearchUrl(sSrchUrl, Company.Country) ' Country
		BuildSearchUrl(sSrchUrl, Company.PhoneNumber) ' PhoneNumber
		BuildSearchUrl(sSrchUrl, Company.FaxNumber) ' FaxNumber
		BuildSearchUrl(sSrchUrl, Company.DefaultPaymentTerms) ' DefaultPaymentTerms
		BuildSearchUrl(sSrchUrl, Company.DefaultInvoiceDescription) ' DefaultInvoiceDescription
		BuildSearchUrl(sSrchUrl, Company.Component) ' Component
		BuildSearchUrl(sSrchUrl, Company.FromEmail) ' FromEmail
		BuildSearchUrl(sSrchUrl, Company.SMTP) ' SMTP
		BuildSearchUrl(sSrchUrl, Company.ActiveFL) ' ActiveFL
		BuildSearchUrl(sSrchUrl, Company.UseBreadCrumbURL) ' UseBreadCrumbURL
		BuildSearchUrl(sSrchUrl, Company.SingleSiteGallery) ' SingleSiteGallery
		Return sSrchUrl
	End Function

	'
	' Build search URL
	'
	Sub BuildSearchUrl(ByRef Url As String, ByRef Fld As Object)
		Dim FldVal As String, FldOpr As String, FldCond As String, FldVal2 As String, FldOpr2 As String
		Dim FldParm As String
		Dim IsValidValue As Boolean, sWrk As String = ""
		FldParm = Fld.FldVar.Substring(2)
		FldVal = ObjForm.GetValue("x_" & FldParm)
		FldOpr = ObjForm.GetValue("z_" & FldParm)
		FldCond = ObjForm.GetValue("v_" & FldParm)
		FldVal2 = ObjForm.GetValue("y_" & FldParm)
		FldOpr2 = ObjForm.GetValue("w_" & FldParm)
		Dim lFldDataType As Integer
		If Fld.FldIsVirtual Then
			lFldDataType = EW_DATATYPE_STRING
		Else
			lFldDataType = Fld.FldDataType
		End If
		If ew_SameText(FldOpr, "BETWEEN") Then
			IsValidValue = (lFldDataType <> EW_DATATYPE_NUMBER) OrElse _
				(lFldDataType = EW_DATATYPE_NUMBER AndAlso IsNumeric(FldVal) AndAlso IsNumeric(FldVal2))
			If ew_NotEmpty(FldVal) AndAlso ew_NotEmpty(FldVal2) AndAlso IsValidValue Then
				sWrk = "x_" & FldParm & "=" & HttpContext.Current.Server.UrlEncode(FldVal) & _
					"&y_" & FldParm & "=" & HttpContext.Current.Server.UrlEncode(FldVal2) & _
					"&z_" & FldParm & "=" & HttpContext.Current.Server.UrlEncode(FldOpr)
			End If
		ElseIf ew_SameText(FldOpr, "IS NULL") OrElse ew_SameText(FldOpr, "IS NOT NULL") Then
			sWrk = "x_" & FldParm & "=" & HttpContext.Current.Server.UrlEncode(FldVal) & _
				"&z_" & FldParm & "=" & HttpContext.Current.Server.UrlEncode(FldOpr)
		Else
			IsValidValue = (lFldDataType <> EW_DATATYPE_NUMBER) OrElse _
				(lFldDataType = EW_DATATYPE_NUMBER AndAlso IsNumeric(FldVal))
			If ew_NotEmpty(FldVal) AndAlso IsValidValue AndAlso ew_IsValidOpr(FldOpr, lFldDataType) Then
				sWrk = "x_" & FldParm & "=" & HttpContext.Current.Server.UrlEncode(FldVal) & _
					"&z_" & FldParm & "=" & HttpContext.Current.Server.UrlEncode(FldOpr)
			End If
			IsValidValue = (lFldDataType <> EW_DATATYPE_NUMBER) OrElse _
				(lFldDataType = EW_DATATYPE_NUMBER AndAlso IsNumeric(FldVal2))
			If ew_NotEmpty(FldVal2) AndAlso IsValidValue AndAlso ew_IsValidOpr(FldOpr2, lFldDataType) Then
				If sWrk <> "" Then sWrk = sWrk & "&v_" & FldParm & "=" & FldCond & "&"
				sWrk = sWrk & "y_" & FldParm & "=" & HttpContext.Current.Server.UrlEncode(FldVal2) & _
					"&w_" & FldParm & "=" & HttpContext.Current.Server.UrlEncode(FldOpr2)
			End If
		End If
		If sWrk <> "" Then
			If Url <> "" Then Url = Url & "&"
			Url = Url & sWrk
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
		Company.CompanyID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_CompanyID")
    	Company.CompanyID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_CompanyID")
		Company.CompanyName.AdvancedSearch.SearchValue = ObjForm.GetValue("x_CompanyName")
    	Company.CompanyName.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_CompanyName")
		Company.SiteCategoryTypeID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_SiteCategoryTypeID")
    	Company.SiteCategoryTypeID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_SiteCategoryTypeID")
		Company.SiteTitle.AdvancedSearch.SearchValue = ObjForm.GetValue("x_SiteTitle")
    	Company.SiteTitle.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_SiteTitle")
		Company.SiteURL.AdvancedSearch.SearchValue = ObjForm.GetValue("x_SiteURL")
    	Company.SiteURL.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_SiteURL")
		Company.GalleryFolder.AdvancedSearch.SearchValue = ObjForm.GetValue("x_GalleryFolder")
    	Company.GalleryFolder.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_GalleryFolder")
		Company.HomePageID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_HomePageID")
    	Company.HomePageID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_HomePageID")
		Company.DefaultArticleID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_DefaultArticleID")
    	Company.DefaultArticleID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_DefaultArticleID")
		Company.SiteTemplate.AdvancedSearch.SearchValue = ObjForm.GetValue("x_SiteTemplate")
    	Company.SiteTemplate.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_SiteTemplate")
		Company.DefaultSiteTemplate.AdvancedSearch.SearchValue = ObjForm.GetValue("x_DefaultSiteTemplate")
    	Company.DefaultSiteTemplate.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_DefaultSiteTemplate")
		Company.Address.AdvancedSearch.SearchValue = ObjForm.GetValue("x_Address")
    	Company.Address.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_Address")
		Company.City.AdvancedSearch.SearchValue = ObjForm.GetValue("x_City")
    	Company.City.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_City")
		Company.StateOrProvince.AdvancedSearch.SearchValue = ObjForm.GetValue("x_StateOrProvince")
    	Company.StateOrProvince.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_StateOrProvince")
		Company.PostalCode.AdvancedSearch.SearchValue = ObjForm.GetValue("x_PostalCode")
    	Company.PostalCode.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_PostalCode")
		Company.Country.AdvancedSearch.SearchValue = ObjForm.GetValue("x_Country")
    	Company.Country.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_Country")
		Company.PhoneNumber.AdvancedSearch.SearchValue = ObjForm.GetValue("x_PhoneNumber")
    	Company.PhoneNumber.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_PhoneNumber")
		Company.FaxNumber.AdvancedSearch.SearchValue = ObjForm.GetValue("x_FaxNumber")
    	Company.FaxNumber.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_FaxNumber")
		Company.DefaultPaymentTerms.AdvancedSearch.SearchValue = ObjForm.GetValue("x_DefaultPaymentTerms")
    	Company.DefaultPaymentTerms.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_DefaultPaymentTerms")
		Company.DefaultInvoiceDescription.AdvancedSearch.SearchValue = ObjForm.GetValue("x_DefaultInvoiceDescription")
    	Company.DefaultInvoiceDescription.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_DefaultInvoiceDescription")
		Company.Component.AdvancedSearch.SearchValue = ObjForm.GetValue("x_Component")
    	Company.Component.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_Component")
		Company.FromEmail.AdvancedSearch.SearchValue = ObjForm.GetValue("x_FromEmail")
    	Company.FromEmail.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_FromEmail")
		Company.SMTP.AdvancedSearch.SearchValue = ObjForm.GetValue("x_SMTP")
    	Company.SMTP.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_SMTP")
		Company.ActiveFL.AdvancedSearch.SearchValue = ObjForm.GetValue("x_ActiveFL")
    	Company.ActiveFL.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_ActiveFL")
		Company.UseBreadCrumbURL.AdvancedSearch.SearchValue = ObjForm.GetValue("x_UseBreadCrumbURL")
    	Company.UseBreadCrumbURL.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_UseBreadCrumbURL")
		Company.SingleSiteGallery.AdvancedSearch.SearchValue = ObjForm.GetValue("x_SingleSiteGallery")
    	Company.SingleSiteGallery.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_SingleSiteGallery")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Initialize urls
		' Row Rendering event

		Company.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' CompanyID

		Company.CompanyID.CellCssStyle = ""
		Company.CompanyID.CellCssClass = ""
		Company.CompanyID.CellAttrs.Clear(): Company.CompanyID.ViewAttrs.Clear(): Company.CompanyID.EditAttrs.Clear()

		' CompanyName
		Company.CompanyName.CellCssStyle = ""
		Company.CompanyName.CellCssClass = ""
		Company.CompanyName.CellAttrs.Clear(): Company.CompanyName.ViewAttrs.Clear(): Company.CompanyName.EditAttrs.Clear()

		' SiteCategoryTypeID
		Company.SiteCategoryTypeID.CellCssStyle = ""
		Company.SiteCategoryTypeID.CellCssClass = ""
		Company.SiteCategoryTypeID.CellAttrs.Clear(): Company.SiteCategoryTypeID.ViewAttrs.Clear(): Company.SiteCategoryTypeID.EditAttrs.Clear()

		' SiteTitle
		Company.SiteTitle.CellCssStyle = ""
		Company.SiteTitle.CellCssClass = ""
		Company.SiteTitle.CellAttrs.Clear(): Company.SiteTitle.ViewAttrs.Clear(): Company.SiteTitle.EditAttrs.Clear()

		' SiteURL
		Company.SiteURL.CellCssStyle = ""
		Company.SiteURL.CellCssClass = ""
		Company.SiteURL.CellAttrs.Clear(): Company.SiteURL.ViewAttrs.Clear(): Company.SiteURL.EditAttrs.Clear()

		' GalleryFolder
		Company.GalleryFolder.CellCssStyle = ""
		Company.GalleryFolder.CellCssClass = ""
		Company.GalleryFolder.CellAttrs.Clear(): Company.GalleryFolder.ViewAttrs.Clear(): Company.GalleryFolder.EditAttrs.Clear()

		' HomePageID
		Company.HomePageID.CellCssStyle = ""
		Company.HomePageID.CellCssClass = ""
		Company.HomePageID.CellAttrs.Clear(): Company.HomePageID.ViewAttrs.Clear(): Company.HomePageID.EditAttrs.Clear()

		' DefaultArticleID
		Company.DefaultArticleID.CellCssStyle = ""
		Company.DefaultArticleID.CellCssClass = ""
		Company.DefaultArticleID.CellAttrs.Clear(): Company.DefaultArticleID.ViewAttrs.Clear(): Company.DefaultArticleID.EditAttrs.Clear()

		' SiteTemplate
		Company.SiteTemplate.CellCssStyle = ""
		Company.SiteTemplate.CellCssClass = ""
		Company.SiteTemplate.CellAttrs.Clear(): Company.SiteTemplate.ViewAttrs.Clear(): Company.SiteTemplate.EditAttrs.Clear()

		' DefaultSiteTemplate
		Company.DefaultSiteTemplate.CellCssStyle = ""
		Company.DefaultSiteTemplate.CellCssClass = ""
		Company.DefaultSiteTemplate.CellAttrs.Clear(): Company.DefaultSiteTemplate.ViewAttrs.Clear(): Company.DefaultSiteTemplate.EditAttrs.Clear()

		' Address
		Company.Address.CellCssStyle = ""
		Company.Address.CellCssClass = ""
		Company.Address.CellAttrs.Clear(): Company.Address.ViewAttrs.Clear(): Company.Address.EditAttrs.Clear()

		' City
		Company.City.CellCssStyle = ""
		Company.City.CellCssClass = ""
		Company.City.CellAttrs.Clear(): Company.City.ViewAttrs.Clear(): Company.City.EditAttrs.Clear()

		' StateOrProvince
		Company.StateOrProvince.CellCssStyle = ""
		Company.StateOrProvince.CellCssClass = ""
		Company.StateOrProvince.CellAttrs.Clear(): Company.StateOrProvince.ViewAttrs.Clear(): Company.StateOrProvince.EditAttrs.Clear()

		' PostalCode
		Company.PostalCode.CellCssStyle = ""
		Company.PostalCode.CellCssClass = ""
		Company.PostalCode.CellAttrs.Clear(): Company.PostalCode.ViewAttrs.Clear(): Company.PostalCode.EditAttrs.Clear()

		' Country
		Company.Country.CellCssStyle = ""
		Company.Country.CellCssClass = ""
		Company.Country.CellAttrs.Clear(): Company.Country.ViewAttrs.Clear(): Company.Country.EditAttrs.Clear()

		' PhoneNumber
		Company.PhoneNumber.CellCssStyle = ""
		Company.PhoneNumber.CellCssClass = ""
		Company.PhoneNumber.CellAttrs.Clear(): Company.PhoneNumber.ViewAttrs.Clear(): Company.PhoneNumber.EditAttrs.Clear()

		' FaxNumber
		Company.FaxNumber.CellCssStyle = ""
		Company.FaxNumber.CellCssClass = ""
		Company.FaxNumber.CellAttrs.Clear(): Company.FaxNumber.ViewAttrs.Clear(): Company.FaxNumber.EditAttrs.Clear()

		' DefaultPaymentTerms
		Company.DefaultPaymentTerms.CellCssStyle = ""
		Company.DefaultPaymentTerms.CellCssClass = ""
		Company.DefaultPaymentTerms.CellAttrs.Clear(): Company.DefaultPaymentTerms.ViewAttrs.Clear(): Company.DefaultPaymentTerms.EditAttrs.Clear()

		' DefaultInvoiceDescription
		Company.DefaultInvoiceDescription.CellCssStyle = ""
		Company.DefaultInvoiceDescription.CellCssClass = ""
		Company.DefaultInvoiceDescription.CellAttrs.Clear(): Company.DefaultInvoiceDescription.ViewAttrs.Clear(): Company.DefaultInvoiceDescription.EditAttrs.Clear()

		' Component
		Company.Component.CellCssStyle = ""
		Company.Component.CellCssClass = ""
		Company.Component.CellAttrs.Clear(): Company.Component.ViewAttrs.Clear(): Company.Component.EditAttrs.Clear()

		' FromEmail
		Company.FromEmail.CellCssStyle = ""
		Company.FromEmail.CellCssClass = ""
		Company.FromEmail.CellAttrs.Clear(): Company.FromEmail.ViewAttrs.Clear(): Company.FromEmail.EditAttrs.Clear()

		' SMTP
		Company.SMTP.CellCssStyle = ""
		Company.SMTP.CellCssClass = ""
		Company.SMTP.CellAttrs.Clear(): Company.SMTP.ViewAttrs.Clear(): Company.SMTP.EditAttrs.Clear()

		' ActiveFL
		Company.ActiveFL.CellCssStyle = ""
		Company.ActiveFL.CellCssClass = ""
		Company.ActiveFL.CellAttrs.Clear(): Company.ActiveFL.ViewAttrs.Clear(): Company.ActiveFL.EditAttrs.Clear()

		' UseBreadCrumbURL
		Company.UseBreadCrumbURL.CellCssStyle = ""
		Company.UseBreadCrumbURL.CellCssClass = ""
		Company.UseBreadCrumbURL.CellAttrs.Clear(): Company.UseBreadCrumbURL.ViewAttrs.Clear(): Company.UseBreadCrumbURL.EditAttrs.Clear()

		' SingleSiteGallery
		Company.SingleSiteGallery.CellCssStyle = ""
		Company.SingleSiteGallery.CellCssClass = ""
		Company.SingleSiteGallery.CellAttrs.Clear(): Company.SingleSiteGallery.ViewAttrs.Clear(): Company.SingleSiteGallery.EditAttrs.Clear()

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
			' CompanyID

			Company.CompanyID.HrefValue = ""
			Company.CompanyID.TooltipValue = ""

			' CompanyName
			Company.CompanyName.HrefValue = ""
			Company.CompanyName.TooltipValue = ""

			' SiteCategoryTypeID
			Company.SiteCategoryTypeID.HrefValue = ""
			Company.SiteCategoryTypeID.TooltipValue = ""

			' SiteTitle
			Company.SiteTitle.HrefValue = ""
			Company.SiteTitle.TooltipValue = ""

			' SiteURL
			Company.SiteURL.HrefValue = ""
			Company.SiteURL.TooltipValue = ""

			' GalleryFolder
			Company.GalleryFolder.HrefValue = ""
			Company.GalleryFolder.TooltipValue = ""

			' HomePageID
			Company.HomePageID.HrefValue = ""
			Company.HomePageID.TooltipValue = ""

			' DefaultArticleID
			Company.DefaultArticleID.HrefValue = ""
			Company.DefaultArticleID.TooltipValue = ""

			' SiteTemplate
			Company.SiteTemplate.HrefValue = ""
			Company.SiteTemplate.TooltipValue = ""

			' DefaultSiteTemplate
			Company.DefaultSiteTemplate.HrefValue = ""
			Company.DefaultSiteTemplate.TooltipValue = ""

			' Address
			Company.Address.HrefValue = ""
			Company.Address.TooltipValue = ""

			' City
			Company.City.HrefValue = ""
			Company.City.TooltipValue = ""

			' StateOrProvince
			Company.StateOrProvince.HrefValue = ""
			Company.StateOrProvince.TooltipValue = ""

			' PostalCode
			Company.PostalCode.HrefValue = ""
			Company.PostalCode.TooltipValue = ""

			' Country
			Company.Country.HrefValue = ""
			Company.Country.TooltipValue = ""

			' PhoneNumber
			Company.PhoneNumber.HrefValue = ""
			Company.PhoneNumber.TooltipValue = ""

			' FaxNumber
			Company.FaxNumber.HrefValue = ""
			Company.FaxNumber.TooltipValue = ""

			' DefaultPaymentTerms
			Company.DefaultPaymentTerms.HrefValue = ""
			Company.DefaultPaymentTerms.TooltipValue = ""

			' DefaultInvoiceDescription
			Company.DefaultInvoiceDescription.HrefValue = ""
			Company.DefaultInvoiceDescription.TooltipValue = ""

			' Component
			Company.Component.HrefValue = ""
			Company.Component.TooltipValue = ""

			' FromEmail
			Company.FromEmail.HrefValue = ""
			Company.FromEmail.TooltipValue = ""

			' SMTP
			Company.SMTP.HrefValue = ""
			Company.SMTP.TooltipValue = ""

			' ActiveFL
			Company.ActiveFL.HrefValue = ""
			Company.ActiveFL.TooltipValue = ""

			' UseBreadCrumbURL
			Company.UseBreadCrumbURL.HrefValue = ""
			Company.UseBreadCrumbURL.TooltipValue = ""

			' SingleSiteGallery
			Company.SingleSiteGallery.HrefValue = ""
			Company.SingleSiteGallery.TooltipValue = ""

		'
		'  Search Row
		'

		ElseIf Company.RowType = EW_ROWTYPE_SEARCH Then ' Search row

			' CompanyID
			Company.CompanyID.EditCustomAttributes = ""
			Company.CompanyID.EditValue = ew_HtmlEncode(Company.CompanyID.AdvancedSearch.SearchValue)

			' CompanyName
			Company.CompanyName.EditCustomAttributes = ""
			Company.CompanyName.EditValue = ew_HtmlEncode(Company.CompanyName.AdvancedSearch.SearchValue)

			' SiteCategoryTypeID
			Company.SiteCategoryTypeID.EditCustomAttributes = ""
			sFilterWrk = ""
			sSqlWrk = "SELECT [SiteCategoryTypeID], [SiteCategoryTypeNM], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteCategoryType]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [SiteCategoryTypeNM] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", Language.Phrase("PleaseSelect")}) 
			Company.SiteCategoryTypeID.EditValue = arwrk

			' SiteTitle
			Company.SiteTitle.EditCustomAttributes = ""
			Company.SiteTitle.EditValue = ew_HtmlEncode(Company.SiteTitle.AdvancedSearch.SearchValue)

			' SiteURL
			Company.SiteURL.EditCustomAttributes = ""
			Company.SiteURL.EditValue = ew_HtmlEncode(Company.SiteURL.AdvancedSearch.SearchValue)

			' GalleryFolder
			Company.GalleryFolder.EditCustomAttributes = ""
			Company.GalleryFolder.EditValue = ew_HtmlEncode(Company.GalleryFolder.AdvancedSearch.SearchValue)

			' HomePageID
			Company.HomePageID.EditCustomAttributes = ""
			sFilterWrk = ""
			sSqlWrk = "SELECT [PageID], [PageName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Page]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [PageName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", Language.Phrase("PleaseSelect")}) 
			Company.HomePageID.EditValue = arwrk

			' DefaultArticleID
			Company.DefaultArticleID.EditCustomAttributes = ""
			sFilterWrk = ""
			sSqlWrk = "SELECT [ArticleID], [Title], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Article]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [Title] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", Language.Phrase("PleaseSelect")}) 
			Company.DefaultArticleID.EditValue = arwrk

			' SiteTemplate
			Company.SiteTemplate.EditCustomAttributes = ""
			sFilterWrk = ""
			sSqlWrk = "SELECT [TemplatePrefix], [Name], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteTemplate]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [Name] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", Language.Phrase("PleaseSelect")}) 
			Company.SiteTemplate.EditValue = arwrk

			' DefaultSiteTemplate
			Company.DefaultSiteTemplate.EditCustomAttributes = ""
			sFilterWrk = ""
			sSqlWrk = "SELECT [TemplatePrefix], [Name], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteTemplate]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [Name] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", Language.Phrase("PleaseSelect")}) 
			Company.DefaultSiteTemplate.EditValue = arwrk

			' Address
			Company.Address.EditCustomAttributes = ""
			Company.Address.EditValue = ew_HtmlEncode(Company.Address.AdvancedSearch.SearchValue)

			' City
			Company.City.EditCustomAttributes = ""
			Company.City.EditValue = ew_HtmlEncode(Company.City.AdvancedSearch.SearchValue)

			' StateOrProvince
			Company.StateOrProvince.EditCustomAttributes = ""
			Company.StateOrProvince.EditValue = ew_HtmlEncode(Company.StateOrProvince.AdvancedSearch.SearchValue)

			' PostalCode
			Company.PostalCode.EditCustomAttributes = ""
			Company.PostalCode.EditValue = ew_HtmlEncode(Company.PostalCode.AdvancedSearch.SearchValue)

			' Country
			Company.Country.EditCustomAttributes = ""
			Company.Country.EditValue = ew_HtmlEncode(Company.Country.AdvancedSearch.SearchValue)

			' PhoneNumber
			Company.PhoneNumber.EditCustomAttributes = ""
			Company.PhoneNumber.EditValue = ew_HtmlEncode(Company.PhoneNumber.AdvancedSearch.SearchValue)

			' FaxNumber
			Company.FaxNumber.EditCustomAttributes = ""
			Company.FaxNumber.EditValue = ew_HtmlEncode(Company.FaxNumber.AdvancedSearch.SearchValue)

			' DefaultPaymentTerms
			Company.DefaultPaymentTerms.EditCustomAttributes = ""
			Company.DefaultPaymentTerms.EditValue = ew_HtmlEncode(Company.DefaultPaymentTerms.AdvancedSearch.SearchValue)

			' DefaultInvoiceDescription
			Company.DefaultInvoiceDescription.EditCustomAttributes = ""
			Company.DefaultInvoiceDescription.EditValue = ew_HtmlEncode(Company.DefaultInvoiceDescription.AdvancedSearch.SearchValue)

			' Component
			Company.Component.EditCustomAttributes = ""
			Company.Component.EditValue = ew_HtmlEncode(Company.Component.AdvancedSearch.SearchValue)

			' FromEmail
			Company.FromEmail.EditCustomAttributes = ""
			Company.FromEmail.EditValue = ew_HtmlEncode(Company.FromEmail.AdvancedSearch.SearchValue)

			' SMTP
			Company.SMTP.EditCustomAttributes = ""
			Company.SMTP.EditValue = ew_HtmlEncode(Company.SMTP.AdvancedSearch.SearchValue)

			' ActiveFL
			Company.ActiveFL.EditCustomAttributes = ""

			' UseBreadCrumbURL
			Company.UseBreadCrumbURL.EditCustomAttributes = ""

			' SingleSiteGallery
			Company.SingleSiteGallery.EditCustomAttributes = ""
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
	End Class

	'
	' ASP.NET Page_Load event
	'

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

		' Page init
		Company_search = New cCompany_search(Me)		
		Company_search.Page_Init()		

		' Set buffer/cache option
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

		' Page main processing
		Company_search.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If Company_search IsNot Nothing Then Company_search.Dispose()
	End Sub
End Class
