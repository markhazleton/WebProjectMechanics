Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class company_edit
	Inherits AspNetMaker8_wpmWebsite

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
		Public ReadOnly Property AspNetPage() As company_edit
			Get
				Return CType(m_ParentPage, company_edit)
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
			m_PageID = "edit"
			m_PageObjName = "Company_edit"
			m_PageObjTypeName = "cCompany_edit"			

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

	'
	' Page main processing
	'
	Sub Page_Main()

		' Load key from QueryString
		If ew_Get("CompanyID") <> "" Then
			Company.CompanyID.QueryStringValue = ew_Get("CompanyID")
		End If
		If ObjForm.GetValue("a_edit") <> "" Then
			Company.CurrentAction = ObjForm.GetValue("a_edit") ' Get action code
			LoadFormValues() ' Get form values

			' Validate Form
			If Not ValidateForm() Then
				Company.CurrentAction = "" ' Form error, reset action
				Message = ParentPage.gsFormError
				Company.EventCancelled = True ' Event cancelled
				LoadRow() ' Restore row
				RestoreFormValues() ' Restore form values if validate failed
			End If
		Else
			Company.CurrentAction = "I" ' Default action is display
		End If

		' Check if valid key
		If ew_Empty(Company.CompanyID.CurrentValue) Then Page_Terminate("company_list.aspx") ' Invalid key, return to list
		Select Case Company.CurrentAction
			Case "I" ' Get a record to display
				If Not LoadRow() Then ' Load Record based on key
					Message = Language.Phrase("NoRecord") ' No record found
					Page_Terminate("company_list.aspx") ' No matching record, return to list
				End If
			Case "U" ' Update
				Company.SendEmail = True ' Send email on update success
				If EditRow() Then ' Update Record based on key
					Message = Language.Phrase("UpdateSuccess") ' Update success
					Dim sReturnUrl As String = Company.ReturnUrl
					Page_Terminate(sReturnUrl) ' Return to caller
				Else
					Company.EventCancelled = True ' Event cancelled 
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
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		Company.CompanyID.CurrentValue = Company.CompanyID.FormValue
		Company.CompanyName.CurrentValue = Company.CompanyName.FormValue
		Company.SiteCategoryTypeID.CurrentValue = Company.SiteCategoryTypeID.FormValue
		Company.SiteTitle.CurrentValue = Company.SiteTitle.FormValue
		Company.SiteURL.CurrentValue = Company.SiteURL.FormValue
		Company.GalleryFolder.CurrentValue = Company.GalleryFolder.FormValue
		Company.HomePageID.CurrentValue = Company.HomePageID.FormValue
		Company.DefaultArticleID.CurrentValue = Company.DefaultArticleID.FormValue
		Company.SiteTemplate.CurrentValue = Company.SiteTemplate.FormValue
		Company.DefaultSiteTemplate.CurrentValue = Company.DefaultSiteTemplate.FormValue
		Company.Component.CurrentValue = Company.Component.FormValue
		Company.FromEmail.CurrentValue = Company.FromEmail.FormValue
		Company.SMTP.CurrentValue = Company.SMTP.FormValue
		Company.ActiveFL.CurrentValue = Company.ActiveFL.FormValue
		Company.UseBreadCrumbURL.CurrentValue = Company.UseBreadCrumbURL.FormValue
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

		'
		'  Edit Row
		'

		ElseIf Company.RowType = EW_ROWTYPE_EDIT Then ' Edit row

			' CompanyID
			Company.CompanyID.EditCustomAttributes = ""

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

			' Edit refer script
			' CompanyID

			Company.CompanyID.HrefValue = ""

			' CompanyName
			Company.CompanyName.HrefValue = ""

			' SiteCategoryTypeID
			Company.SiteCategoryTypeID.HrefValue = ""

			' SiteTitle
			Company.SiteTitle.HrefValue = ""

			' SiteURL
			Company.SiteURL.HrefValue = ""

			' GalleryFolder
			Company.GalleryFolder.HrefValue = ""

			' HomePageID
			Company.HomePageID.HrefValue = ""

			' DefaultArticleID
			Company.DefaultArticleID.HrefValue = ""

			' SiteTemplate
			Company.SiteTemplate.HrefValue = ""

			' DefaultSiteTemplate
			Company.DefaultSiteTemplate.HrefValue = ""

			' Component
			Company.Component.HrefValue = ""

			' FromEmail
			Company.FromEmail.HrefValue = ""

			' SMTP
			Company.SMTP.HrefValue = ""

			' ActiveFL
			Company.ActiveFL.HrefValue = ""

			' UseBreadCrumbURL
			Company.UseBreadCrumbURL.HrefValue = ""
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
			sFilterChk = "([CompanyName] = '" & ew_AdjustSql(Company.CompanyName.CurrentValue) & "')"
			sFilterChk = sFilterChk & " AND NOT (" & sFilter & ")"
			Company.CurrentFilter = sFilterChk
			sSqlChk = Company.SQL
			Try
				RsChk = Conn.GetDataReader(sSqlChk)
				If RsChk.Read() Then
					sIdxErrMsg = Language.Phrase("DupIndex").Replace("%f", "CompanyName")
					sIdxErrMsg = sIdxErrMsg.Replace("%v", Company.CompanyName.CurrentValue)
					Message = sIdxErrMsg			
					Return False
				End If
			Catch e As Exception
				If EW_DEBUG_ENABLED Then Throw
				Message = e.Message
				Return False
			Finally
				If RsChk IsNot Nothing Then
					RsChk.Close()
					RsChk.Dispose()
				End If
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
			Try
				RsOld = Conn.GetRow(RsEdit)					 

				' CompanyID
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
				Company.HomePageID.SetDbValue(Rs, Company.HomePageID.CurrentValue, System.DBNull.Value, False)

				' DefaultArticleID
				Company.DefaultArticleID.SetDbValue(Rs, Company.DefaultArticleID.CurrentValue, System.DBNull.Value, False)

				' SiteTemplate
				Company.SiteTemplate.SetDbValue(Rs, Company.SiteTemplate.CurrentValue, System.DBNull.Value, False)

				' DefaultSiteTemplate
				Company.DefaultSiteTemplate.SetDbValue(Rs, Company.DefaultSiteTemplate.CurrentValue, System.DBNull.Value, False)

				' Component
				Company.Component.SetDbValue(Rs, Company.Component.CurrentValue, System.DBNull.Value, False)

				' FromEmail
				Company.FromEmail.SetDbValue(Rs, Company.FromEmail.CurrentValue, System.DBNull.Value, False)

				' SMTP
				Company.SMTP.SetDbValue(Rs, Company.SMTP.CurrentValue, System.DBNull.Value, False)

				' ActiveFL
				Company.ActiveFL.SetDbValue(Rs, (Company.ActiveFL.CurrentValue <> "" AndAlso Not IsDBNull(Company.ActiveFL.CurrentValue)), System.DBNull.Value, False)

				' UseBreadCrumbURL
				Company.UseBreadCrumbURL.SetDbValue(Rs, (Company.UseBreadCrumbURL.CurrentValue <> "" AndAlso Not IsDBNull(Company.UseBreadCrumbURL.CurrentValue)), System.DBNull.Value, False)
			Catch e As Exception
				If EW_DEBUG_ENABLED Then Throw
				Message = e.Message
				EditRow = False
			End Try
			RsEdit.Close()

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
					Message = Language.Phrase("UpdateCancelled")
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
		Dim curDateTime As String, id As String, user As String, action As String
		Dim i As Integer
		Dim dt As DateTime = Now()		
		curDateTime = dt.ToString("yyyy/MM/dd HH:mm:ss")
		id = HttpContext.Current.Request.ServerVariables("SCRIPT_NAME")
    user = CurrentUserName()
		action = typ
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "", "", "", "")
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
		Dim curDateTime As String, id As String, user As String, field As String
		Dim keyvalue As Object, oldvalue As Object, newvalue As Object
		Dim dt As DateTime = Now()
		Dim fld As cField, fldname As String, Modified As Boolean
		curDateTime = dt.ToString("yyyy/MM/dd HH:mm:ss")
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
					ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, fldname, keyvalue, oldvalue, newvalue)
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
		Company_edit = New cCompany_edit(Me)		
		Company_edit.Page_Init()		

		' Set buffer/cache option
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

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
