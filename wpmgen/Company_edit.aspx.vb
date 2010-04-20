Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class Company_edit
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public Company_edit As cCompany_edit

	'
	' Page Class
	'
	Class cCompany_edit
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
			m_PageID = "edit"
			m_PageObjName = "Company_edit"
			m_PageObjTypeName = "cCompany_edit"

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

	'
	' Page main processing
	'
	Sub Page_Main()

		' Load key from QueryString
		If ew_Get("CompanyID") <> "" Then
			Company.CompanyID.QueryStringValue = ew_Get("CompanyID")
		End If

		' Create form object
		ObjForm = New cFormObj
		If ObjForm.GetValue("a_edit") <> "" Then
			Company.CurrentAction = ObjForm.GetValue("a_edit") ' Get action code
			LoadFormValues() ' Get form values

			' Validate Form
			If Not ValidateForm() Then
				Company.CurrentAction = "" ' Form error, reset action
				Message = ParentPage.gsFormError
				LoadRow() ' Restore row
				RestoreFormValues() ' Restore form values if validate failed
			End If
		Else
			Company.CurrentAction = "I" ' Default action is display
		End If

		' Check if valid key
		If ew_Empty(Company.CompanyID.CurrentValue) Then Page_Terminate("Company_list.aspx") ' Invalid key, return to list
		Select Case Company.CurrentAction
			Case "I" ' Get a record to display
				If Not LoadRow() Then ' Load Record based on key
					Message = "No records found" ' No record found
					Page_Terminate("Company_list.aspx") ' No matching record, return to list
				End If
			Case "U" ' Update
				Company.SendEmail = True ' Send email on update success
				If EditRow() Then ' Update Record based on key
					Message = "Update succeeded" ' Update success
					Dim sReturnUrl As String = Company.ReturnUrl
					If ew_GetPageName(sReturnUrl) = "Company_view.aspx" Then sReturnUrl = Company.ViewUrl ' View paging, return to view page with keyurl directly
					Page_Terminate(sReturnUrl) ' Return to caller
				Else
					LoadRow() ' Restore row
					RestoreFormValues() ' Restore form values if update failed
				End If
		End Select

		' Render the record
		Company.RowType = EW_ROWTYPE_EDIT ' Render as edit

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
	End Sub

	'
	' Load form values
	'
	Sub LoadFormValues()
		Company.CompanyID.FormValue = ObjForm.GetValue("x_CompanyID")
		Company.CompanyID.OldValue = ObjForm.GetValue("o_CompanyID")
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
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		Company.CompanyID.CurrentValue = Company.CompanyID.FormValue
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
		' CompanyID

		Company.CompanyID.CellCssStyle = ""
		Company.CompanyID.CellCssClass = ""

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
			' CompanyID

			Company.CompanyID.HrefValue = ""

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
		'  Edit Row
		'

		ElseIf Company.RowType = EW_ROWTYPE_EDIT Then ' Edit row

			' CompanyID
			Company.CompanyID.EditCustomAttributes = ""
			Company.CompanyID.EditValue = Company.CompanyID.CurrentValue
			Company.CompanyID.CssStyle = ""
			Company.CompanyID.CssClass = ""
			Company.CompanyID.ViewCustomAttributes = ""

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

			' Edit refer script
			' CompanyID

			Company.CompanyID.HrefValue = ""

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
	' Update record based on key values
	'
	Function EditRow() As Boolean
		Dim RsEdit As OleDbDataReader, RsChk As OleDbDataReader
		Dim sSql As String, sFilter As String
		Dim sSqlChk As String, sFilterChk As String
		Dim bUpdateRow As Boolean
		Dim RsOld As OrderedDictionary
		Dim sIdxErrMsg As String
		Dim Rs As New OrderedDictionary
		sFilter = Company.KeyFilter
		If Company.CompanyName.CurrentValue <> "" Then ' Check field with unique index
			sFilterChk = "(CompanyName = '" & ew_AdjustSql(Company.CompanyName.CurrentValue) & "')"
			sFilterChk = sFilterChk & " AND NOT (" & sFilter & ")"
			Company.CurrentFilter = sFilterChk
			sSqlChk = Company.SQL
			Try
				RsChk = Conn.GetDataReader(sSqlChk)
				If RsChk.Read() Then
					sIdxErrMsg = "Duplicate value '%v' for unique index '%f'".Replace("%f", "CompanyName")
					sIdxErrMsg = sIdxErrMsg.Replace("%v", Company.CompanyName.CurrentValue)
					Message = sIdxErrMsg			
					Return False
				End If
			Catch e As Exception
				If EW_DEBUG_ENABLED Then Throw
				Message = e.Message
				Return False
			Finally
				RsChk.Close()
				RsChk.Dispose()	
			End Try				
		End If
		Company.CurrentFilter  = sFilter
		sSql = Company.SQL
		Try
			RsEdit = Conn.GetDataReader(sSql) 
		Catch e As Exception
			If EW_DEBUG_ENABLED Then Throw
			Message = e.Message
			RsEdit.Close()
			EditRow = False
		End Try
		If Not RsEdit.Read() Then
			RsEdit.Close()
			EditRow = False ' Update Failed
		Else
			RsOld = Conn.GetRow(RsEdit)
			RsEdit.Close()

			' CompanyID
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

			' Row Updating event
			bUpdateRow = Company.Row_Updating(RsOld, Rs)
			If bUpdateRow Then
				Try
					Company.Update(Rs)
					EditRow = True
				Catch e As Exception
					If EW_DEBUG_ENABLED Then Throw
					Message = e.Message
					EditRow = False
				End Try
			Else
				If Company.CancelMessage <> "" Then
					Message = Company.CancelMessage
					Company.CancelMessage = ""
				Else
					Message = "Update cancelled"
				End If
				EditRow = False
			End If
		End If

		' Row Updated event
		If EditRow Then
			Company.Row_Updated(RsOld, Rs)
		End If
		If EditRow Then
			WriteAuditTrailOnEdit(RsOld, Rs)
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

	' Write Audit Trail (edit page)
	Sub WriteAuditTrailOnEdit(ByRef RsOld As OrderedDictionary, ByRef RsNew As OrderedDictionary)
		Dim table As String = "Company"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsOld("CompanyID")

		' Write Audit Trail
		Dim filePfx As String = "log"
		Dim action As String = "U"
		Dim curDate As String, curTime As String, id As String, user As String, field As String
		Dim keyvalue As Object, oldvalue As Object, newvalue As Object
		Dim dt As DateTime = Now()
		Dim fld As cField, fldname As String, Modified As Boolean
		curDate = dt.ToString("yyyy/MM/dd")
		curTime = dt.ToString("HH:mm:ss")
		id = HttpContext.Current.Request.ServerVariables("SCRIPT_NAME")
    user = CurrentUserName()
		Dim arFldNames(RsNew.Count - 1) As String
		RsNew.Keys.CopyTo(arFldNames, 0)
		For Each fldname In arFldNames
			fld = Company.FieldByName(fldname)
			If fld IsNot Nothing AndAlso fld.FldDataType <> EW_DATATYPE_BLOB AndAlso fld.FldDataType <> EW_DATATYPE_MEMO Then ' Ignore Blob/Memo Field
				oldvalue = RsOld(fldname)
				newvalue = RsNew(fldname)

				'oldvalue = ew_Conv(oldvalue, fld.FldType)
				'newvalue = ew_Conv(newvalue, fld.FldType)

				If fld.FldDataType = EW_DATATYPE_DATE Then ' DateTime Field
					Modified = Not ew_SameStr(ew_FormatDateTime(oldvalue, 8), ew_FormatDateTime(newvalue, 8))
				Else
					Modified = Not ew_CompareValue(oldvalue, newvalue)
				End If				
				If Modified Then					
					keyvalue = sKey
					ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, fldname, keyvalue, oldvalue, newvalue)
				End If
			End If
		Next
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
		Company_edit = New cCompany_edit(Me)		
		Company_edit.Page_Init()

		' Page main processing
		Company_edit.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If Company_edit IsNot Nothing Then Company_edit.Dispose()
	End Sub
End Class
