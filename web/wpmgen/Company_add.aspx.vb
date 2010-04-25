Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class Company_add
	Inherits AspNetMaker7_WPMGen

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
		Public Sub New(ByRef APage As AspNetMaker7_WPMGen)				
			m_ParentPage = APage
			m_Page = Me	
			m_PageID = "add"
			m_PageObjName = "Company_add"
			m_PageObjTypeName = "cCompany_add"

			' Table Name
			m_TableName = "Company"

			' Initialize table object
			Company = New cCompany(Me)

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

			' Go to URL if specified
			If url <> "" Then
				HttpContext.Current.Response.Clear()
				HttpContext.Current.Response.Redirect(url)
			End If
		End Sub

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

		' Create form object
		ObjForm = New cFormObj

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
					Message = "No records found" ' No record found
					Page_Terminate("Company_list.aspx") ' No matching record, return to list
				End If
			Case "A" ' Add new record
				Company.SendEmail = True ' Send email on add success
				If AddRow() Then ' Add successful
					Message = "Add succeeded" ' Set up success message
					Dim sReturnUrl As String = Company.ReturnUrl
					If ew_GetPageName(sReturnUrl) = "Company_view.aspx" Then sReturnUrl = Company.ViewUrl ' View paging, return to view page with keyurl directly
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
		Company.SiteTitle.FormValue = ObjForm.GetValue("x_SiteTitle")
		Company.SiteTitle.OldValue = ObjForm.GetValue("o_SiteTitle")
		Company.SiteURL.FormValue = ObjForm.GetValue("x_SiteURL")
		Company.SiteURL.OldValue = ObjForm.GetValue("o_SiteURL")
		Company.GalleryFolder.FormValue = ObjForm.GetValue("x_GalleryFolder")
		Company.GalleryFolder.OldValue = ObjForm.GetValue("o_GalleryFolder")
		Company.SiteCategoryTypeID.FormValue = ObjForm.GetValue("x_SiteCategoryTypeID")
		Company.SiteCategoryTypeID.OldValue = ObjForm.GetValue("o_SiteCategoryTypeID")
		Company.HomePageID.FormValue = ObjForm.GetValue("x_HomePageID")
		Company.HomePageID.OldValue = ObjForm.GetValue("o_HomePageID")
		Company.DefaultArticleID.FormValue = ObjForm.GetValue("x_DefaultArticleID")
		Company.DefaultArticleID.OldValue = ObjForm.GetValue("o_DefaultArticleID")
		Company.SiteTemplate.FormValue = ObjForm.GetValue("x_SiteTemplate")
		Company.SiteTemplate.OldValue = ObjForm.GetValue("o_SiteTemplate")
		Company.DefaultSiteTemplate.FormValue = ObjForm.GetValue("x_DefaultSiteTemplate")
		Company.DefaultSiteTemplate.OldValue = ObjForm.GetValue("o_DefaultSiteTemplate")
		Company.UseBreadCrumbURL.FormValue = ObjForm.GetValue("x_UseBreadCrumbURL")
		Company.UseBreadCrumbURL.OldValue = ObjForm.GetValue("o_UseBreadCrumbURL")
		Company.SingleSiteGallery.FormValue = ObjForm.GetValue("x_SingleSiteGallery")
		Company.SingleSiteGallery.OldValue = ObjForm.GetValue("o_SingleSiteGallery")
		Company.ActiveFL.FormValue = ObjForm.GetValue("x_ActiveFL")
		Company.ActiveFL.OldValue = ObjForm.GetValue("o_ActiveFL")
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
		Company.Component.FormValue = ObjForm.GetValue("x_Component")
		Company.Component.OldValue = ObjForm.GetValue("o_Component")
		Company.FromEmail.FormValue = ObjForm.GetValue("x_FromEmail")
		Company.FromEmail.OldValue = ObjForm.GetValue("o_FromEmail")
		Company.SMTP.FormValue = ObjForm.GetValue("x_SMTP")
		Company.SMTP.OldValue = ObjForm.GetValue("o_SMTP")
		Company.CompanyID.FormValue = ObjForm.GetValue("x_CompanyID")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		Company.CompanyName.CurrentValue = Company.CompanyName.FormValue
		Company.SiteTitle.CurrentValue = Company.SiteTitle.FormValue
		Company.SiteURL.CurrentValue = Company.SiteURL.FormValue
		Company.GalleryFolder.CurrentValue = Company.GalleryFolder.FormValue
		Company.SiteCategoryTypeID.CurrentValue = Company.SiteCategoryTypeID.FormValue
		Company.HomePageID.CurrentValue = Company.HomePageID.FormValue
		Company.DefaultArticleID.CurrentValue = Company.DefaultArticleID.FormValue
		Company.SiteTemplate.CurrentValue = Company.SiteTemplate.FormValue
		Company.DefaultSiteTemplate.CurrentValue = Company.DefaultSiteTemplate.FormValue
		Company.UseBreadCrumbURL.CurrentValue = Company.UseBreadCrumbURL.FormValue
		Company.SingleSiteGallery.CurrentValue = Company.SingleSiteGallery.FormValue
		Company.ActiveFL.CurrentValue = Company.ActiveFL.FormValue
		Company.Address.CurrentValue = Company.Address.FormValue
		Company.City.CurrentValue = Company.City.FormValue
		Company.StateOrProvince.CurrentValue = Company.StateOrProvince.FormValue
		Company.PostalCode.CurrentValue = Company.PostalCode.FormValue
		Company.Country.CurrentValue = Company.Country.FormValue
		Company.Component.CurrentValue = Company.Component.FormValue
		Company.FromEmail.CurrentValue = Company.FromEmail.FormValue
		Company.SMTP.CurrentValue = Company.SMTP.FormValue
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
		If EW_DEBUG_ENABLED Then ew_Write(sSql)
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
		Company.CompanyID.DbValue = RsRow("CompanyID")
		Company.CompanyName.DbValue = RsRow("CompanyName")
		Company.SiteTitle.DbValue = RsRow("SiteTitle")
		Company.SiteURL.DbValue = RsRow("SiteURL")
		Company.GalleryFolder.DbValue = RsRow("GalleryFolder")
		Company.SiteCategoryTypeID.DbValue = RsRow("SiteCategoryTypeID")
		Company.HomePageID.DbValue = RsRow("HomePageID")
		Company.DefaultArticleID.DbValue = RsRow("DefaultArticleID")
		Company.SiteTemplate.DbValue = RsRow("SiteTemplate")
		Company.DefaultSiteTemplate.DbValue = RsRow("DefaultSiteTemplate")
		Company.UseBreadCrumbURL.DbValue = IIf(ew_ConvertToBool(RsRow("UseBreadCrumbURL")), "1", "0")
		Company.SingleSiteGallery.DbValue = IIf(ew_ConvertToBool(RsRow("SingleSiteGallery")), "1", "0")
		Company.ActiveFL.DbValue = IIf(ew_ConvertToBool(RsRow("ActiveFL")), "1", "0")
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
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		Company.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' CompanyName

		Company.CompanyName.CellCssStyle = ""
		Company.CompanyName.CellCssClass = ""

		' SiteTitle
		Company.SiteTitle.CellCssStyle = ""
		Company.SiteTitle.CellCssClass = ""

		' SiteURL
		Company.SiteURL.CellCssStyle = ""
		Company.SiteURL.CellCssClass = ""

		' GalleryFolder
		Company.GalleryFolder.CellCssStyle = ""
		Company.GalleryFolder.CellCssClass = ""

		' SiteCategoryTypeID
		Company.SiteCategoryTypeID.CellCssStyle = ""
		Company.SiteCategoryTypeID.CellCssClass = ""

		' HomePageID
		Company.HomePageID.CellCssStyle = ""
		Company.HomePageID.CellCssClass = ""

		' DefaultArticleID
		Company.DefaultArticleID.CellCssStyle = ""
		Company.DefaultArticleID.CellCssClass = ""

		' SiteTemplate
		Company.SiteTemplate.CellCssStyle = ""
		Company.SiteTemplate.CellCssClass = ""

		' DefaultSiteTemplate
		Company.DefaultSiteTemplate.CellCssStyle = ""
		Company.DefaultSiteTemplate.CellCssClass = ""

		' UseBreadCrumbURL
		Company.UseBreadCrumbURL.CellCssStyle = ""
		Company.UseBreadCrumbURL.CellCssClass = ""

		' SingleSiteGallery
		Company.SingleSiteGallery.CellCssStyle = ""
		Company.SingleSiteGallery.CellCssClass = ""

		' ActiveFL
		Company.ActiveFL.CellCssStyle = ""
		Company.ActiveFL.CellCssClass = ""

		' Address
		Company.Address.CellCssStyle = ""
		Company.Address.CellCssClass = ""

		' City
		Company.City.CellCssStyle = ""
		Company.City.CellCssClass = ""

		' StateOrProvince
		Company.StateOrProvince.CellCssStyle = ""
		Company.StateOrProvince.CellCssClass = ""

		' PostalCode
		Company.PostalCode.CellCssStyle = ""
		Company.PostalCode.CellCssClass = ""

		' Country
		Company.Country.CellCssStyle = ""
		Company.Country.CellCssClass = ""

		' Component
		Company.Component.CellCssStyle = ""
		Company.Component.CellCssClass = ""

		' FromEmail
		Company.FromEmail.CellCssStyle = ""
		Company.FromEmail.CellCssClass = ""

		' SMTP
		Company.SMTP.CellCssStyle = ""
		Company.SMTP.CellCssClass = ""

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

			' SiteCategoryTypeID
			If ew_NotEmpty(Company.SiteCategoryTypeID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteCategoryTypeNM] FROM [SiteCategoryType] WHERE [SiteCategoryTypeID] = " & ew_AdjustSql(Company.SiteCategoryTypeID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryTypeNM] Asc"
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

			' HomePageID
			If ew_NotEmpty(Company.HomePageID.CurrentValue) Then
				sSqlWrk = "SELECT [PageName] FROM [Page] WHERE [PageID] = " & ew_AdjustSql(Company.HomePageID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [PageName] Asc"
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
				sSqlWrk = "SELECT [Title] FROM [Article] WHERE [ArticleID] = " & ew_AdjustSql(Company.DefaultArticleID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [Title] Asc"
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
				sSqlWrk = "SELECT [Name] FROM [SiteTemplate] WHERE [TemplatePrefix] = '" & ew_AdjustSql(Company.SiteTemplate.CurrentValue) & "'"
				sSqlWrk = sSqlWrk & " ORDER BY [Name] Asc"
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
				sSqlWrk = "SELECT [Name] FROM [SiteTemplate] WHERE [TemplatePrefix] = '" & ew_AdjustSql(Company.DefaultSiteTemplate.CurrentValue) & "'"
				sSqlWrk = sSqlWrk & " ORDER BY [Name] Asc"
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

			' ActiveFL
			If Convert.ToString(Company.ActiveFL.CurrentValue) = "1" Then
				Company.ActiveFL.ViewValue = "Yes"
			Else
				Company.ActiveFL.ViewValue = "No"
			End If
			Company.ActiveFL.CssStyle = ""
			Company.ActiveFL.CssClass = ""
			Company.ActiveFL.ViewCustomAttributes = ""

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

			' View refer script
			' CompanyName

			Company.CompanyName.HrefValue = ""

			' SiteTitle
			Company.SiteTitle.HrefValue = ""

			' SiteURL
			Company.SiteURL.HrefValue = ""

			' GalleryFolder
			Company.GalleryFolder.HrefValue = ""

			' SiteCategoryTypeID
			Company.SiteCategoryTypeID.HrefValue = ""

			' HomePageID
			Company.HomePageID.HrefValue = ""

			' DefaultArticleID
			Company.DefaultArticleID.HrefValue = ""

			' SiteTemplate
			Company.SiteTemplate.HrefValue = ""

			' DefaultSiteTemplate
			Company.DefaultSiteTemplate.HrefValue = ""

			' UseBreadCrumbURL
			Company.UseBreadCrumbURL.HrefValue = ""

			' SingleSiteGallery
			Company.SingleSiteGallery.HrefValue = ""

			' ActiveFL
			Company.ActiveFL.HrefValue = ""

			' Address
			Company.Address.HrefValue = ""

			' City
			Company.City.HrefValue = ""

			' StateOrProvince
			Company.StateOrProvince.HrefValue = ""

			' PostalCode
			Company.PostalCode.HrefValue = ""

			' Country
			Company.Country.HrefValue = ""

			' Component
			Company.Component.HrefValue = ""

			' FromEmail
			Company.FromEmail.HrefValue = ""

			' SMTP
			Company.SMTP.HrefValue = ""

		'
		'  Add Row
		'

		ElseIf Company.RowType = EW_ROWTYPE_ADD Then ' Add row

			' CompanyName
			Company.CompanyName.EditCustomAttributes = ""
			Company.CompanyName.EditValue = ew_HtmlEncode(Company.CompanyName.CurrentValue)

			' SiteTitle
			Company.SiteTitle.EditCustomAttributes = ""
			Company.SiteTitle.EditValue = ew_HtmlEncode(Company.SiteTitle.CurrentValue)

			' SiteURL
			Company.SiteURL.EditCustomAttributes = ""
			Company.SiteURL.EditValue = ew_HtmlEncode(Company.SiteURL.CurrentValue)

			' GalleryFolder
			Company.GalleryFolder.EditCustomAttributes = ""
			Company.GalleryFolder.EditValue = ew_HtmlEncode(Company.GalleryFolder.CurrentValue)

			' SiteCategoryTypeID
			Company.SiteCategoryTypeID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryTypeID], [SiteCategoryTypeNM], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteCategoryType]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryTypeNM] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			Company.SiteCategoryTypeID.EditValue = arwrk

			' HomePageID
			Company.HomePageID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [PageID], [PageName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Page]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [PageName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			Company.HomePageID.EditValue = arwrk

			' DefaultArticleID
			Company.DefaultArticleID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [ArticleID], [Title], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Article]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [Title] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			Company.DefaultArticleID.EditValue = arwrk

			' SiteTemplate
			Company.SiteTemplate.EditCustomAttributes = ""
			sSqlWrk = "SELECT [TemplatePrefix], [Name], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteTemplate]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [Name] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			Company.SiteTemplate.EditValue = arwrk

			' DefaultSiteTemplate
			Company.DefaultSiteTemplate.EditCustomAttributes = ""
			sSqlWrk = "SELECT [TemplatePrefix], [Name], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteTemplate]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [Name] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			Company.DefaultSiteTemplate.EditValue = arwrk

			' UseBreadCrumbURL
			Company.UseBreadCrumbURL.EditCustomAttributes = ""

			' SingleSiteGallery
			Company.SingleSiteGallery.EditCustomAttributes = ""

			' ActiveFL
			Company.ActiveFL.EditCustomAttributes = ""

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

			' Component
			Company.Component.EditCustomAttributes = ""
			Company.Component.EditValue = ew_HtmlEncode(Company.Component.CurrentValue)

			' FromEmail
			Company.FromEmail.EditCustomAttributes = ""
			Company.FromEmail.EditValue = ew_HtmlEncode(Company.FromEmail.CurrentValue)

			' SMTP
			Company.SMTP.EditCustomAttributes = ""
			Company.SMTP.EditValue = ew_HtmlEncode(Company.SMTP.CurrentValue)
		End If

		' Row Rendered event
		Company.Row_Rendered()
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
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter required field - Company Name"
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
				sIdxErrMsg = "Duplicate value '%v' for unique index '%f'".Replace("%f", "CompanyName")
				sIdxErrMsg = sIdxErrMsg.Replace("%v", Company.CompanyName.CurrentValue)
				Message = sIdxErrMsg
				RsChk.Close()
				RsChk.Dispose()
				Return False
			End If
		End If

		' CompanyName
		Company.CompanyName.SetDbValue(Company.CompanyName.CurrentValue, "")
		Rs("CompanyName") = Company.CompanyName.DbValue

		' SiteTitle
		Company.SiteTitle.SetDbValue(Company.SiteTitle.CurrentValue, System.DBNull.Value)
		Rs("SiteTitle") = Company.SiteTitle.DbValue

		' SiteURL
		Company.SiteURL.SetDbValue(Company.SiteURL.CurrentValue, System.DBNull.Value)
		Rs("SiteURL") = Company.SiteURL.DbValue

		' GalleryFolder
		Company.GalleryFolder.SetDbValue(Company.GalleryFolder.CurrentValue, System.DBNull.Value)
		Rs("GalleryFolder") = Company.GalleryFolder.DbValue

		' SiteCategoryTypeID
		Company.SiteCategoryTypeID.SetDbValue(Company.SiteCategoryTypeID.CurrentValue, System.DBNull.Value)
		Rs("SiteCategoryTypeID") = Company.SiteCategoryTypeID.DbValue

		' HomePageID
		Company.HomePageID.SetDbValue(Company.HomePageID.CurrentValue, System.DBNull.Value)
		Rs("HomePageID") = Company.HomePageID.DbValue

		' DefaultArticleID
		Company.DefaultArticleID.SetDbValue(Company.DefaultArticleID.CurrentValue, System.DBNull.Value)
		Rs("DefaultArticleID") = Company.DefaultArticleID.DbValue

		' SiteTemplate
		Company.SiteTemplate.SetDbValue(Company.SiteTemplate.CurrentValue, System.DBNull.Value)
		Rs("SiteTemplate") = Company.SiteTemplate.DbValue

		' DefaultSiteTemplate
		Company.DefaultSiteTemplate.SetDbValue(Company.DefaultSiteTemplate.CurrentValue, System.DBNull.Value)
		Rs("DefaultSiteTemplate") = Company.DefaultSiteTemplate.DbValue

		' UseBreadCrumbURL
		Company.UseBreadCrumbURL.SetDbValue((Company.UseBreadCrumbURL.CurrentValue <> "" And Not IsDBNull(Company.UseBreadCrumbURL.CurrentValue)), System.DBNull.Value)
		Rs("UseBreadCrumbURL") = Company.UseBreadCrumbURL.DbValue

		' SingleSiteGallery
		Company.SingleSiteGallery.SetDbValue((Company.SingleSiteGallery.CurrentValue <> "" And Not IsDBNull(Company.SingleSiteGallery.CurrentValue)), System.DBNull.Value)
		Rs("SingleSiteGallery") = Company.SingleSiteGallery.DbValue

		' ActiveFL
		Company.ActiveFL.SetDbValue((Company.ActiveFL.CurrentValue <> "" And Not IsDBNull(Company.ActiveFL.CurrentValue)), System.DBNull.Value)
		Rs("ActiveFL") = Company.ActiveFL.DbValue

		' Address
		Company.Address.SetDbValue(Company.Address.CurrentValue, System.DBNull.Value)
		Rs("Address") = Company.Address.DbValue

		' City
		Company.City.SetDbValue(Company.City.CurrentValue, System.DBNull.Value)
		Rs("City") = Company.City.DbValue

		' StateOrProvince
		Company.StateOrProvince.SetDbValue(Company.StateOrProvince.CurrentValue, System.DBNull.Value)
		Rs("StateOrProvince") = Company.StateOrProvince.DbValue

		' PostalCode
		Company.PostalCode.SetDbValue(Company.PostalCode.CurrentValue, System.DBNull.Value)
		Rs("PostalCode") = Company.PostalCode.DbValue

		' Country
		Company.Country.SetDbValue(Company.Country.CurrentValue, System.DBNull.Value)
		Rs("Country") = Company.Country.DbValue

		' Component
		Company.Component.SetDbValue(Company.Component.CurrentValue, System.DBNull.Value)
		Rs("Component") = Company.Component.DbValue

		' FromEmail
		Company.FromEmail.SetDbValue(Company.FromEmail.CurrentValue, System.DBNull.Value)
		Rs("FromEmail") = Company.FromEmail.DbValue

		' SMTP
		Company.SMTP.SetDbValue(Company.SMTP.CurrentValue, System.DBNull.Value)
		Rs("SMTP") = Company.SMTP.DbValue

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
				Message = "Insert cancelled"
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
		Dim curDate As String, curTime As String, id As String, user As String, action As String
		Dim i As Integer
		Dim dt As DateTime = Now()		
		curDate = dt.ToString("yyyy/MM/dd")
		curTime = dt.ToString("HH:mm:ss")
		id = HttpContext.Current.Request.ServerVariables("SCRIPT_NAME")
    user = CurrentUserName()
		action = typ
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "", "", "", "")
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
		Dim curDate As String, curTime As String, id As String, user As String, field As String
		Dim keyvalue As Object
		Dim dt As DateTime = Now()
		curDate = dt.ToString("yyyy/MM/dd")
		curTime = dt.ToString("HH:mm:ss")
		id = HttpContext.Current.Request.ServerVariables("SCRIPT_NAME")
    user = CurrentUserName()
		keyvalue = sKey

		' CompanyName Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "CompanyName", keyvalue, oldvalue, RsSrc("CompanyName"))

		' SiteTitle Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteTitle", keyvalue, oldvalue, RsSrc("SiteTitle"))

		' SiteURL Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteURL", keyvalue, oldvalue, RsSrc("SiteURL"))

		' GalleryFolder Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "GalleryFolder", keyvalue, oldvalue, RsSrc("GalleryFolder"))

		' SiteCategoryTypeID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteCategoryTypeID", keyvalue, oldvalue, RsSrc("SiteCategoryTypeID"))

		' HomePageID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "HomePageID", keyvalue, oldvalue, RsSrc("HomePageID"))

		' DefaultArticleID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "DefaultArticleID", keyvalue, oldvalue, RsSrc("DefaultArticleID"))

		' SiteTemplate Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteTemplate", keyvalue, oldvalue, RsSrc("SiteTemplate"))

		' DefaultSiteTemplate Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "DefaultSiteTemplate", keyvalue, oldvalue, RsSrc("DefaultSiteTemplate"))

		' UseBreadCrumbURL Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "UseBreadCrumbURL", keyvalue, oldvalue, RsSrc("UseBreadCrumbURL"))

		' SingleSiteGallery Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SingleSiteGallery", keyvalue, oldvalue, RsSrc("SingleSiteGallery"))

		' ActiveFL Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "ActiveFL", keyvalue, oldvalue, RsSrc("ActiveFL"))

		' Address Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "Address", keyvalue, oldvalue, RsSrc("Address"))

		' City Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "City", keyvalue, oldvalue, RsSrc("City"))

		' StateOrProvince Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "StateOrProvince", keyvalue, oldvalue, RsSrc("StateOrProvince"))

		' PostalCode Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "PostalCode", keyvalue, oldvalue, RsSrc("PostalCode"))

		' Country Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "Country", keyvalue, oldvalue, RsSrc("Country"))

		' Component Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "Component", keyvalue, oldvalue, RsSrc("Component"))

		' FromEmail Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "FromEmail", keyvalue, oldvalue, RsSrc("FromEmail"))

		' SMTP Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SMTP", keyvalue, oldvalue, RsSrc("SMTP"))
	End Sub

		' Page Load event
		Public Sub Page_Load()

			'HttpContext.Current.Response.Write("Page Load")
		End Sub

		' Page Unload event
		Public Sub Page_Unload()

			'HttpContext.Current.Response.Write("Page Unload")
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
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

		' Page init
		Company_add = New cCompany_add(Me)		
		Company_add.Page_Init()

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
