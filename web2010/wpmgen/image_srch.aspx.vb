Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class image_srch
	Inherits AspNetMaker8_wpmWebsite

	' Page object
	Public Image_search As cImage_search

	'
	' Page Class
	'
	Class cImage_search
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
		Public ReadOnly Property AspNetPage() As image_srch
			Get
				Return CType(m_ParentPage, image_srch)
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
			m_PageID = "search"
			m_PageObjName = "Image_search"
			m_PageObjTypeName = "cImage_search"			

			' Initialize language object
			Language = New cLanguage(Me)

			' Table Name
			m_TableName = "Image"

			' Initialize table object
			Image = New cImage(Me)
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
			Image.Dispose()
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
			Image.CurrentAction = ObjForm.GetValue("a_search")
			Select Case Image.CurrentAction
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
						sSrchStr = Image.UrlParm(sSrchStr)
					Page_Terminate("image_list.aspx" & "?" & sSrchStr) ' Go to list page
					End If
			End Select
		End If

		' Restore search settings from Session
		If ParentPage.gsSearchError = "" Then
			LoadAdvancedSearch()
		End If

		' Render row for search
		Image.RowType = EW_ROWTYPE_SEARCH
		RenderRow()
	End Sub

	'
	' Build advanced search
	'
	Function BuildAdvancedSearch() As String
		Dim sSrchUrl As String = ""
		BuildSearchUrl(sSrchUrl, Image.ImageID) ' ImageID
		BuildSearchUrl(sSrchUrl, Image.ImageName) ' ImageName
		BuildSearchUrl(sSrchUrl, Image.ImageFileName) ' ImageFileName
		BuildSearchUrl(sSrchUrl, Image.ImageThumbFileName) ' ImageThumbFileName
		BuildSearchUrl(sSrchUrl, Image.ImageDescription) ' ImageDescription
		BuildSearchUrl(sSrchUrl, Image.ImageComment) ' ImageComment
		BuildSearchUrl(sSrchUrl, Image.ImageDate) ' ImageDate
		BuildSearchUrl(sSrchUrl, Image.Active) ' Active
		BuildSearchUrl(sSrchUrl, Image.ModifiedDT) ' ModifiedDT
		BuildSearchUrl(sSrchUrl, Image.VersionNo) ' VersionNo
		BuildSearchUrl(sSrchUrl, Image.ContactID) ' ContactID
		BuildSearchUrl(sSrchUrl, Image.CompanyID) ' CompanyID
		BuildSearchUrl(sSrchUrl, Image.title) ' title
		BuildSearchUrl(sSrchUrl, Image.medium) ' medium
		BuildSearchUrl(sSrchUrl, Image.size) ' size
		BuildSearchUrl(sSrchUrl, Image.price) ' price
		BuildSearchUrl(sSrchUrl, Image.color) ' color
		BuildSearchUrl(sSrchUrl, Image.subject) ' subject
		BuildSearchUrl(sSrchUrl, Image.sold) ' sold
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
		Image.ImageID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_ImageID")
    	Image.ImageID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_ImageID")
		Image.ImageName.AdvancedSearch.SearchValue = ObjForm.GetValue("x_ImageName")
    	Image.ImageName.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_ImageName")
		Image.ImageFileName.AdvancedSearch.SearchValue = ObjForm.GetValue("x_ImageFileName")
    	Image.ImageFileName.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_ImageFileName")
		Image.ImageThumbFileName.AdvancedSearch.SearchValue = ObjForm.GetValue("x_ImageThumbFileName")
    	Image.ImageThumbFileName.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_ImageThumbFileName")
		Image.ImageDescription.AdvancedSearch.SearchValue = ObjForm.GetValue("x_ImageDescription")
    	Image.ImageDescription.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_ImageDescription")
		Image.ImageComment.AdvancedSearch.SearchValue = ObjForm.GetValue("x_ImageComment")
    	Image.ImageComment.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_ImageComment")
		Image.ImageDate.AdvancedSearch.SearchValue = ObjForm.GetValue("x_ImageDate")
    	Image.ImageDate.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_ImageDate")
		Image.Active.AdvancedSearch.SearchValue = ObjForm.GetValue("x_Active")
    	Image.Active.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_Active")
		Image.ModifiedDT.AdvancedSearch.SearchValue = ObjForm.GetValue("x_ModifiedDT")
    	Image.ModifiedDT.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_ModifiedDT")
		Image.VersionNo.AdvancedSearch.SearchValue = ObjForm.GetValue("x_VersionNo")
    	Image.VersionNo.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_VersionNo")
		Image.ContactID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_ContactID")
    	Image.ContactID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_ContactID")
		Image.CompanyID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_CompanyID")
    	Image.CompanyID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_CompanyID")
		Image.title.AdvancedSearch.SearchValue = ObjForm.GetValue("x_title")
    	Image.title.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_title")
		Image.medium.AdvancedSearch.SearchValue = ObjForm.GetValue("x_medium")
    	Image.medium.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_medium")
		Image.size.AdvancedSearch.SearchValue = ObjForm.GetValue("x_size")
    	Image.size.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_size")
		Image.price.AdvancedSearch.SearchValue = ObjForm.GetValue("x_price")
    	Image.price.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_price")
		Image.color.AdvancedSearch.SearchValue = ObjForm.GetValue("x_color")
    	Image.color.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_color")
		Image.subject.AdvancedSearch.SearchValue = ObjForm.GetValue("x_subject")
    	Image.subject.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_subject")
		Image.sold.AdvancedSearch.SearchValue = ObjForm.GetValue("x_sold")
    	Image.sold.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_sold")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Initialize urls
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

		' ImageDescription
		Image.ImageDescription.CellCssStyle = ""
		Image.ImageDescription.CellCssClass = ""
		Image.ImageDescription.CellAttrs.Clear(): Image.ImageDescription.ViewAttrs.Clear(): Image.ImageDescription.EditAttrs.Clear()

		' ImageComment
		Image.ImageComment.CellCssStyle = ""
		Image.ImageComment.CellCssClass = ""
		Image.ImageComment.CellAttrs.Clear(): Image.ImageComment.ViewAttrs.Clear(): Image.ImageComment.EditAttrs.Clear()

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

			' ImageDescription
			Image.ImageDescription.ViewValue = Image.ImageDescription.CurrentValue
			Image.ImageDescription.CssStyle = ""
			Image.ImageDescription.CssClass = ""
			Image.ImageDescription.ViewCustomAttributes = ""

			' ImageComment
			Image.ImageComment.ViewValue = Image.ImageComment.CurrentValue
			Image.ImageComment.CssStyle = ""
			Image.ImageComment.CssClass = ""
			Image.ImageComment.ViewCustomAttributes = ""

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

			' ImageDescription
			Image.ImageDescription.HrefValue = ""
			Image.ImageDescription.TooltipValue = ""

			' ImageComment
			Image.ImageComment.HrefValue = ""
			Image.ImageComment.TooltipValue = ""

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

		'
		'  Search Row
		'

		ElseIf Image.RowType = EW_ROWTYPE_SEARCH Then ' Search row

			' ImageID
			Image.ImageID.EditCustomAttributes = ""
			Image.ImageID.EditValue = ew_HtmlEncode(Image.ImageID.AdvancedSearch.SearchValue)

			' ImageName
			Image.ImageName.EditCustomAttributes = ""
			Image.ImageName.EditValue = ew_HtmlEncode(Image.ImageName.AdvancedSearch.SearchValue)

			' ImageFileName
			Image.ImageFileName.EditCustomAttributes = ""
			Image.ImageFileName.EditValue = ew_HtmlEncode(Image.ImageFileName.AdvancedSearch.SearchValue)

			' ImageThumbFileName
			Image.ImageThumbFileName.EditCustomAttributes = ""
			Image.ImageThumbFileName.EditValue = ew_HtmlEncode(Image.ImageThumbFileName.AdvancedSearch.SearchValue)

			' ImageDescription
			Image.ImageDescription.EditCustomAttributes = ""
			Image.ImageDescription.EditValue = ew_HtmlEncode(Image.ImageDescription.AdvancedSearch.SearchValue)

			' ImageComment
			Image.ImageComment.EditCustomAttributes = ""
			Image.ImageComment.EditValue = ew_HtmlEncode(Image.ImageComment.AdvancedSearch.SearchValue)

			' ImageDate
			Image.ImageDate.EditCustomAttributes = ""
			Image.ImageDate.EditValue = Image.ImageDate.AdvancedSearch.SearchValue

			' Active
			Image.Active.EditCustomAttributes = ""

			' ModifiedDT
			Image.ModifiedDT.EditCustomAttributes = ""
			Image.ModifiedDT.EditValue = Image.ModifiedDT.AdvancedSearch.SearchValue

			' VersionNo
			Image.VersionNo.EditCustomAttributes = ""
			Image.VersionNo.EditValue = ew_HtmlEncode(Image.VersionNo.AdvancedSearch.SearchValue)

			' ContactID
			Image.ContactID.EditCustomAttributes = ""
			Image.ContactID.EditValue = ew_HtmlEncode(Image.ContactID.AdvancedSearch.SearchValue)

			' CompanyID
			Image.CompanyID.EditCustomAttributes = ""
			Image.CompanyID.EditValue = ew_HtmlEncode(Image.CompanyID.AdvancedSearch.SearchValue)

			' title
			Image.title.EditCustomAttributes = ""
			Image.title.EditValue = ew_HtmlEncode(Image.title.AdvancedSearch.SearchValue)

			' medium
			Image.medium.EditCustomAttributes = ""
			Image.medium.EditValue = ew_HtmlEncode(Image.medium.AdvancedSearch.SearchValue)

			' size
			Image.size.EditCustomAttributes = ""
			Image.size.EditValue = ew_HtmlEncode(Image.size.AdvancedSearch.SearchValue)

			' price
			Image.price.EditCustomAttributes = ""
			Image.price.EditValue = ew_HtmlEncode(Image.price.AdvancedSearch.SearchValue)

			' color
			Image.color.EditCustomAttributes = ""
			Image.color.EditValue = ew_HtmlEncode(Image.color.AdvancedSearch.SearchValue)

			' subject
			Image.subject.EditCustomAttributes = ""
			Image.subject.EditValue = ew_HtmlEncode(Image.subject.AdvancedSearch.SearchValue)

			' sold
			Image.sold.EditCustomAttributes = ""
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
		If Not ew_CheckInteger(Image.ImageID.AdvancedSearch.SearchValue) Then
			If ParentPage.gsSearchError <> "" Then ParentPage.gsSearchError = ParentPage.gsSearchError & "<br />"
			ParentPage.gsSearchError &= Image.ImageID.FldErrMsg
		End If
		If Not ew_CheckInteger(Image.VersionNo.AdvancedSearch.SearchValue) Then
			If ParentPage.gsSearchError <> "" Then ParentPage.gsSearchError = ParentPage.gsSearchError & "<br />"
			ParentPage.gsSearchError &= Image.VersionNo.FldErrMsg
		End If
		If Not ew_CheckInteger(Image.ContactID.AdvancedSearch.SearchValue) Then
			If ParentPage.gsSearchError <> "" Then ParentPage.gsSearchError = ParentPage.gsSearchError & "<br />"
			ParentPage.gsSearchError &= Image.ContactID.FldErrMsg
		End If
		If Not ew_CheckInteger(Image.CompanyID.AdvancedSearch.SearchValue) Then
			If ParentPage.gsSearchError <> "" Then ParentPage.gsSearchError = ParentPage.gsSearchError & "<br />"
			ParentPage.gsSearchError &= Image.CompanyID.FldErrMsg
		End If

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
		Image_search = New cImage_search(Me)		
		Image_search.Page_Init()		

		' Set buffer/cache option
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

		' Page main processing
		Image_search.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If Image_search IsNot Nothing Then Image_search.Dispose()
	End Sub
End Class
