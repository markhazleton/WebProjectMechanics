Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class company_add
	Inherits AspNetMaker8_wpmWebsite

	' Page object
	Public Company_add As cCompany_add

	'
	' Page Class
	'
	Class cCompany_add
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
		Public ReadOnly Property AspNetPage() As company_add
			Get
				Return CType(m_ParentPage, company_add)
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
			m_PageID = "add"
			m_PageObjName = "Company_add"
			m_PageObjTypeName = "cCompany_add"			

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

	Public sDbMasterFilter As String, sDbDetailFilter As String

	Public x_ewPriv As Integer

	'
	' Page main processing
	'
	Sub Page_Main()

		' Load key values from QueryString
		Dim bCopy As Boolean = True
		If ew_Get("CompanyID") <> "" Then
			Company.CompanyID.QueryStringValue = ew_Get("CompanyID")
		Else
			bCopy = False
		End If

		' Process form if post back
		If ObjForm.GetValue("a_add") <> "" Then
			Company.CurrentAction = ObjForm.GetValue("a_add") ' Get form action
			LoadFormValues() ' Load form values

			' Validate Form
			If Not ValidateForm() Then
				Company.CurrentAction = "I" ' Form error, reset action
				Message = ParentPage.gsFormError
			End If

		' Not post back
		Else
			If bCopy Then
				Company.CurrentAction = "C" ' Copy Record
			Else
				Company.CurrentAction = "I" ' Display Blank Record
				LoadDefaultValues() ' Load default values
			End If
		End If

		' Perform action based on action code
		Select Case Company.CurrentAction
			Case "I" ' Blank record, no action required
			Case "C" ' Copy an existing record
				If Not LoadRow() Then ' Load record based on key
					Message = Language.Phrase("NoRecord") ' No record found
					Page_Terminate("company_list.aspx") ' No matching record, return to list
				End If
			Case "A" ' Add new record
				Company.SendEmail = True ' Send email on add success
				If AddRow() Then ' Add successful
					Message = Language.Phrase("AddSuccess") ' Set up success message
					Dim sReturnUrl As String = Company.ReturnUrl
					Page_Terminate(sReturnUrl) ' Clean up and return
				Else
					RestoreFormValues() ' Add failed, restore form values
				End If
		End Select

		' Render row based on row type
		Company.RowType = EW_ROWTYPE_ADD ' Render add type

		' Render row
		RenderRow()
	End Sub

	'
	' Get upload file
	'
	Sub GetUploadFiles()

		' Get upload data
	End Sub

	'
	' Load default values
	'
	Sub LoadDefaultValues()
		Company.HomePageID.CurrentValue = 0
		Company.DefaultArticleID.CurrentValue = 0
		Company.UseBreadCrumbURL.CurrentValue = 0
		Company.SingleSiteGallery.CurrentValue = 0
	End Sub

	'
	' Load form values
	'
	Sub LoadFormValues()
		Company.CompanyName.FormValue = ObjForm.GetValue("x_CompanyName")
		Company.CompanyName.OldValue = ObjForm.GetValue("o_CompanyName")
		Company.SiteCategoryTypeID.FormValue = ObjForm.GetValue("x_SiteCategoryTypeID")
		Company.SiteCategoryTypeID.OldValue = ObjForm.GetValue("o_SiteCategoryTypeID")
		Company.SiteTitle.FormValue = ObjForm.GetValue("x_SiteTitle")
		Company.SiteTitle.OldValue = ObjForm.GetValue("o_SiteTitle")
		Company.SiteURL.FormValue = ObjForm.GetValue("x_SiteURL")
		Company.SiteURL.OldValue = ObjForm.GetValue("o_SiteURL")
		Company.GalleryFolder.FormValue = ObjForm.GetValue("x_GalleryFolder")
		Company.GalleryFolder.OldValue = ObjForm.GetValue("o_GalleryFolder")
		Company.HomePageID.FormValue = ObjForm.GetValue("x_HomePageID")
		Company.HomePageID.OldValue = ObjForm.GetValue("o_HomePageID")
		Company.DefaultArticleID.FormValue = ObjForm.GetValue("x_DefaultArticleID")
		Company.DefaultArticleID.OldValue = ObjForm.GetValue("o_DefaultArticleID")
		Company.SiteTemplate.FormValue = ObjForm.GetValue("x_SiteTemplate")
		Company.SiteTemplate.OldValue = ObjForm.GetValue("o_SiteTemplate")
		Company.DefaultSiteTemplate.FormValue = ObjForm.GetValue("x_DefaultSiteTemplate")
		Company.DefaultSiteTemplate.OldValue = ObjForm.GetValue("o_DefaultSiteTemplate")
		Company.Address.FormValue = ObjForm.GetValue("x_Address")
		Company.Address.OldValue = ObjForm.GetValue("o_Address")
		Company.City.FormValue = ObjForm.GetValue("x_City")
		Company.City.OldValue = ObjForm.GetValue("o_City")
		Company.StateOrProvince.FormValue = ObjForm.GetValue("x_StateOrProvince")
		Company.StateOrProvince.OldValue = ObjForm.GetValue("o_StateOrProvince")
		Company.PostalCode.FormValue = ObjForm.GetValue("x_PostalCode")
		Company.PostalCode.OldValue = ObjForm.GetValue("o_PostalCode")
		Company.Country.FormValue = ObjForm.GetValue("x_Country")
		Company.Country.OldValue = ObjForm.GetValue("o_Country")
		Company.PhoneNumber.FormValue = ObjForm.GetValue("x_PhoneNumber")
		Company.PhoneNumber.OldValue = ObjForm.GetValue("o_PhoneNumber")
		Company.FaxNumber.FormValue = ObjForm.GetValue("x_FaxNumber")
		Company.FaxNumber.OldValue = ObjForm.GetValue("o_FaxNumber")
		Company.DefaultPaymentTerms.FormValue = ObjForm.GetValue("x_DefaultPaymentTerms")
		Company.DefaultPaymentTerms.OldValue = ObjForm.GetValue("o_DefaultPaymentTerms")
		Company.DefaultInvoiceDescription.FormValue = ObjForm.GetValue("x_DefaultInvoiceDescription")
		Company.DefaultInvoiceDescription.OldValue = ObjForm.GetValue("o_DefaultInvoiceDescription")
		Company.Component.FormValue = ObjForm.GetValue("x_Component")
		Company.Component.OldValue = ObjForm.GetValue("o_Component")
		Company.FromEmail.FormValue = ObjForm.GetValue("x_FromEmail")
		Company.FromEmail.OldValue = ObjForm.GetValue("o_FromEmail")
		Company.SMTP.FormValue = ObjForm.GetValue("x_SMTP")
		Company.SMTP.OldValue = ObjForm.GetValue("o_SMTP")
		Company.ActiveFL.FormValue = ObjForm.GetValue("x_ActiveFL")
		Company.ActiveFL.OldValue = ObjForm.GetValue("o_ActiveFL")
		Company.UseBreadCrumbURL.FormValue = ObjForm.GetValue("x_UseBreadCrumbURL")
		Company.UseBreadCrumbURL.OldValue = ObjForm.GetValue("o_UseBreadCrumbURL")
		Company.SingleSiteGallery.FormValue = ObjForm.GetValue("x_SingleSiteGallery")
		Company.SingleSiteGallery.OldValue = ObjForm.GetValue("o_SingleSiteGallery")
		Company.CompanyID.FormValue = ObjForm.GetValue("x_CompanyID")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		Company.CompanyName.CurrentValue = Company.CompanyName.FormValue
		Company.SiteCategoryTypeID.CurrentValue = Company.SiteCategoryTypeID.FormValue
		Company.SiteTitle.CurrentValue = Company.SiteTitle.FormValue
		Company.SiteURL.CurrentValue = Company.SiteURL.FormValue
		Company.GalleryFolder.CurrentValue = Company.GalleryFolder.FormValue
		Company.HomePageID.CurrentValue = Company.HomePageID.FormValue
		Company.DefaultArticleID.CurrentValue = Company.DefaultArticleID.FormValue
		Company.SiteTemplate.CurrentValue = Company.SiteTemplate.FormValue
		Company.DefaultSiteTemplate.CurrentValue = Company.DefaultSiteTemplate.FormValue
		Company.Address.CurrentValue = Company.Address.FormValue
		Company.City.CurrentValue = Company.City.FormValue
		Company.StateOrProvince.CurrentValue = Company.StateOrProvince.FormValue
		Company.PostalCode.CurrentValue = Company.PostalCode.FormValue
		Company.Country.CurrentValue = Company.Country.FormValue
		Company.PhoneNumber.CurrentValue = Company.PhoneNumber.FormValue
		Company.FaxNumber.CurrentValue = Company.FaxNumber.FormValue
		Company.DefaultPaymentTerms.CurrentValue = Company.DefaultPaymentTerms.FormValue
		Company.DefaultInvoiceDescription.CurrentValue = Company.DefaultInvoiceDescription.FormValue
		Company.Component.CurrentValue = Company.Component.FormValue
		Company.FromEmail.CurrentValue = Company.FromEmail.FormValue
		Company.SMTP.CurrentValue = Company.SMTP.FormValue
		Company.ActiveFL.CurrentValue = Company.ActiveFL.FormValue
		Company.UseBreadCrumbURL.CurrentValue = Company.UseBreadCrumbURL.FormValue
		Company.SingleSiteGallery.CurrentValue = Company.SingleSiteGallery.FormValue
		Company.CompanyID.CurrentValue = Company.CompanyID.FormValue
	End Sub

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
		' Row Rendering event

		Company.Row_Rendering()

		'
		'  Common render codes for all row types
		'
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
		'  Add Row
		'

		ElseIf Company.RowType = EW_ROWTYPE_ADD Then ' Add row

			' CompanyName
			Company.CompanyName.EditCustomAttributes = ""
			Company.CompanyName.EditValue = ew_HtmlEncode(Company.CompanyName.CurrentValue)

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
			Company.SiteTitle.EditValue = ew_HtmlEncode(Company.SiteTitle.CurrentValue)

			' SiteURL
			Company.SiteURL.EditCustomAttributes = ""
			Company.SiteURL.EditValue = ew_HtmlEncode(Company.SiteURL.CurrentValue)

			' GalleryFolder
			Company.GalleryFolder.EditCustomAttributes = ""
			Company.GalleryFolder.EditValue = ew_HtmlEncode(Company.GalleryFolder.CurrentValue)

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
			Company.Address.EditValue = ew_HtmlEncode(Company.Address.CurrentValue)

			' City
			Company.City.EditCustomAttributes = ""
			Company.City.EditValue = ew_HtmlEncode(Company.City.CurrentValue)

			' StateOrProvince
			Company.StateOrProvince.EditCustomAttributes = ""
			Company.StateOrProvince.EditValue = ew_HtmlEncode(Company.StateOrProvince.CurrentValue)

			' PostalCode
			Company.PostalCode.EditCustomAttributes = ""
			Company.PostalCode.EditValue = ew_HtmlEncode(Company.PostalCode.CurrentValue)

			' Country
			Company.Country.EditCustomAttributes = ""
			Company.Country.EditValue = ew_HtmlEncode(Company.Country.CurrentValue)

			' PhoneNumber
			Company.PhoneNumber.EditCustomAttributes = ""
			Company.PhoneNumber.EditValue = ew_HtmlEncode(Company.PhoneNumber.CurrentValue)

			' FaxNumber
			Company.FaxNumber.EditCustomAttributes = ""
			Company.FaxNumber.EditValue = ew_HtmlEncode(Company.FaxNumber.CurrentValue)

			' DefaultPaymentTerms
			Company.DefaultPaymentTerms.EditCustomAttributes = ""
			Company.DefaultPaymentTerms.EditValue = ew_HtmlEncode(Company.DefaultPaymentTerms.CurrentValue)

			' DefaultInvoiceDescription
			Company.DefaultInvoiceDescription.EditCustomAttributes = ""
			Company.DefaultInvoiceDescription.EditValue = ew_HtmlEncode(Company.DefaultInvoiceDescription.CurrentValue)

			' Component
			Company.Component.EditCustomAttributes = ""
			Company.Component.EditValue = ew_HtmlEncode(Company.Component.CurrentValue)

			' FromEmail
			Company.FromEmail.EditCustomAttributes = ""
			Company.FromEmail.EditValue = ew_HtmlEncode(Company.FromEmail.CurrentValue)

			' SMTP
			Company.SMTP.EditCustomAttributes = ""
			Company.SMTP.EditValue = ew_HtmlEncode(Company.SMTP.CurrentValue)

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
	' Validate form
	'
	Function ValidateForm() As Boolean

		' Initialize
		ParentPage.gsFormError = ""

		' Check if validation required
		If Not EW_SERVER_VALIDATE Then Return (ParentPage.gsFormError = "")
		If ew_Empty(Company.CompanyName.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError &= Language.Phrase("EnterRequiredField") & " - " & Company.CompanyName.FldCaption
		End If

		' Return validate result
		Dim Valid As Boolean = (ParentPage.gsFormError = "")

		' Form_CustomValidate event
		Dim sFormCustomError As String = ""
		Valid = Valid And Form_CustomValidate(sFormCustomError)
		If sFormCustomError <> "" Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & sFormCustomError
		End If
		Return Valid
	End Function

	'
	' Add record
	'
	Function AddRow() As Boolean
		Dim Rs As New OrderedDictionary
		Dim sSql As String, sFilter As String
		Dim bInsertRow As Boolean
		Dim RsChk As OleDbDataReader
		Dim sIdxErrMsg As String
		Dim LastInsertId As Object
		If ew_NotEmpty(Company.CompanyName.CurrentValue) Then ' Check field with unique index
			sFilter = "([CompanyName] = '" & ew_AdjustSql(Company.CompanyName.CurrentValue) & "')"
			RsChk = Company.LoadRs(sFilter)
			If RsChk IsNot Nothing Then
				sIdxErrMsg = Language.Phrase("DupIndex").Replace("%f", "CompanyName")
				sIdxErrMsg = sIdxErrMsg.Replace("%v", Company.CompanyName.CurrentValue)
				Message = sIdxErrMsg
				RsChk.Close()
				RsChk.Dispose()
				Return False
			End If
		End If
		Try

			' CompanyName
			Company.CompanyName.SetDbValue(Rs, Company.CompanyName.CurrentValue, "", False)

			' SiteCategoryTypeID
			Company.SiteCategoryTypeID.SetDbValue(Rs, Company.SiteCategoryTypeID.CurrentValue, System.DBNull.Value, False)

			' SiteTitle
			Company.SiteTitle.SetDbValue(Rs, Company.SiteTitle.CurrentValue, System.DBNull.Value, False)

			' SiteURL
			Company.SiteURL.SetDbValue(Rs, Company.SiteURL.CurrentValue, System.DBNull.Value, False)

			' GalleryFolder
			Company.GalleryFolder.SetDbValue(Rs, Company.GalleryFolder.CurrentValue, System.DBNull.Value, False)

			' HomePageID
			Company.HomePageID.SetDbValue(Rs, Company.HomePageID.CurrentValue, System.DBNull.Value, True)

			' DefaultArticleID
			Company.DefaultArticleID.SetDbValue(Rs, Company.DefaultArticleID.CurrentValue, System.DBNull.Value, True)

			' SiteTemplate
			Company.SiteTemplate.SetDbValue(Rs, Company.SiteTemplate.CurrentValue, System.DBNull.Value, False)

			' DefaultSiteTemplate
			Company.DefaultSiteTemplate.SetDbValue(Rs, Company.DefaultSiteTemplate.CurrentValue, System.DBNull.Value, False)

			' Address
			Company.Address.SetDbValue(Rs, Company.Address.CurrentValue, System.DBNull.Value, False)

			' City
			Company.City.SetDbValue(Rs, Company.City.CurrentValue, System.DBNull.Value, False)

			' StateOrProvince
			Company.StateOrProvince.SetDbValue(Rs, Company.StateOrProvince.CurrentValue, System.DBNull.Value, False)

			' PostalCode
			Company.PostalCode.SetDbValue(Rs, Company.PostalCode.CurrentValue, System.DBNull.Value, False)

			' Country
			Company.Country.SetDbValue(Rs, Company.Country.CurrentValue, System.DBNull.Value, False)

			' PhoneNumber
			Company.PhoneNumber.SetDbValue(Rs, Company.PhoneNumber.CurrentValue, System.DBNull.Value, False)

			' FaxNumber
			Company.FaxNumber.SetDbValue(Rs, Company.FaxNumber.CurrentValue, System.DBNull.Value, False)

			' DefaultPaymentTerms
			Company.DefaultPaymentTerms.SetDbValue(Rs, Company.DefaultPaymentTerms.CurrentValue, System.DBNull.Value, False)

			' DefaultInvoiceDescription
			Company.DefaultInvoiceDescription.SetDbValue(Rs, Company.DefaultInvoiceDescription.CurrentValue, System.DBNull.Value, False)

			' Component
			Company.Component.SetDbValue(Rs, Company.Component.CurrentValue, System.DBNull.Value, False)

			' FromEmail
			Company.FromEmail.SetDbValue(Rs, Company.FromEmail.CurrentValue, System.DBNull.Value, False)

			' SMTP
			Company.SMTP.SetDbValue(Rs, Company.SMTP.CurrentValue, System.DBNull.Value, False)

			' ActiveFL
			Company.ActiveFL.SetDbValue(Rs, (Company.ActiveFL.CurrentValue <> "" AndAlso Not IsDBNull(Company.ActiveFL.CurrentValue)), System.DBNull.Value, False)

			' UseBreadCrumbURL
			Company.UseBreadCrumbURL.SetDbValue(Rs, (Company.UseBreadCrumbURL.CurrentValue <> "" AndAlso Not IsDBNull(Company.UseBreadCrumbURL.CurrentValue)), System.DBNull.Value, True)

			' SingleSiteGallery
			Company.SingleSiteGallery.SetDbValue(Rs, (Company.SingleSiteGallery.CurrentValue <> "" AndAlso Not IsDBNull(Company.SingleSiteGallery.CurrentValue)), System.DBNull.Value, True)
		Catch e As Exception
			If EW_DEBUG_ENABLED Then Throw
			Message = e.Message
			Return False
		End Try

		' Row Inserting event
		bInsertRow = Company.Row_Inserting(Rs)
		If bInsertRow Then
			Try	
				Company.Insert(Rs)
				AddRow = True
			Catch e As Exception
				If EW_DEBUG_ENABLED Then Throw
				Message = e.Message				
				AddRow = False
			End Try
		Else
			If Company.CancelMessage <> "" Then
				Message = Company.CancelMessage
				Company.CancelMessage = ""
			Else
				Message = Language.Phrase("InsertCancelled")
			End If
			AddRow = False
		End If
		If AddRow Then
			LastInsertId = Conn.GetLastInsertId()
			Company.CompanyID.DbValue = LastInsertId
			Rs("CompanyID") = Company.CompanyID.DbValue		

			' Row Inserted event
			Company.Row_Inserted(Rs)
			WriteAuditTrailOnAdd(Rs)
		End If
	End Function

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

	' Write Audit Trail (add page)
	Sub WriteAuditTrailOnAdd(ByRef RsSrc As OrderedDictionary)
		Dim table As String = "Company"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsSrc("CompanyID")

		' Write Audit Trail
		Dim filePfx As String = "log"
		Dim action As String = "A"
		Dim oldvalue As Object = ""
		Dim curDateTime As String, id As String, user As String, field As String
		Dim keyvalue As Object
		Dim dt As DateTime = Now()
		curDateTime = dt.ToString("yyyy/MM/dd HH:mm:ss")
		id = HttpContext.Current.Request.ServerVariables("SCRIPT_NAME")
    user = CurrentUserName()
		keyvalue = sKey

		' CompanyName Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "CompanyName", keyvalue, oldvalue, RsSrc("CompanyName"))

		' SiteCategoryTypeID Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "SiteCategoryTypeID", keyvalue, oldvalue, RsSrc("SiteCategoryTypeID"))

		' SiteTitle Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "SiteTitle", keyvalue, oldvalue, RsSrc("SiteTitle"))

		' SiteURL Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "SiteURL", keyvalue, oldvalue, RsSrc("SiteURL"))

		' GalleryFolder Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "GalleryFolder", keyvalue, oldvalue, RsSrc("GalleryFolder"))

		' HomePageID Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "HomePageID", keyvalue, oldvalue, RsSrc("HomePageID"))

		' DefaultArticleID Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "DefaultArticleID", keyvalue, oldvalue, RsSrc("DefaultArticleID"))

		' SiteTemplate Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "SiteTemplate", keyvalue, oldvalue, RsSrc("SiteTemplate"))

		' DefaultSiteTemplate Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "DefaultSiteTemplate", keyvalue, oldvalue, RsSrc("DefaultSiteTemplate"))

		' Address Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "Address", keyvalue, oldvalue, RsSrc("Address"))

		' City Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "City", keyvalue, oldvalue, RsSrc("City"))

		' StateOrProvince Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "StateOrProvince", keyvalue, oldvalue, RsSrc("StateOrProvince"))

		' PostalCode Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "PostalCode", keyvalue, oldvalue, RsSrc("PostalCode"))

		' Country Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "Country", keyvalue, oldvalue, RsSrc("Country"))

		' PhoneNumber Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "PhoneNumber", keyvalue, oldvalue, RsSrc("PhoneNumber"))

		' FaxNumber Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "FaxNumber", keyvalue, oldvalue, RsSrc("FaxNumber"))

		' DefaultPaymentTerms Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "DefaultPaymentTerms", keyvalue, oldvalue, RsSrc("DefaultPaymentTerms"))

		' DefaultInvoiceDescription Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "DefaultInvoiceDescription", keyvalue, oldvalue, "<MEMO>")

		' Component Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "Component", keyvalue, oldvalue, RsSrc("Component"))

		' FromEmail Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "FromEmail", keyvalue, oldvalue, RsSrc("FromEmail"))

		' SMTP Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "SMTP", keyvalue, oldvalue, RsSrc("SMTP"))

		' ActiveFL Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "ActiveFL", keyvalue, oldvalue, RsSrc("ActiveFL"))

		' UseBreadCrumbURL Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "UseBreadCrumbURL", keyvalue, oldvalue, RsSrc("UseBreadCrumbURL"))

		' SingleSiteGallery Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "SingleSiteGallery", keyvalue, oldvalue, RsSrc("SingleSiteGallery"))
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
		Company_add = New cCompany_add(Me)		
		Company_add.Page_Init()		

		' Set buffer/cache option
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

		' Page main processing
		Company_add.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If Company_add IsNot Nothing Then Company_add.Dispose()
	End Sub
End Class
