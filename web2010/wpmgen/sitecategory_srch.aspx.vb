Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class sitecategory_srch
	Inherits AspNetMaker8_wpmWebsite

	' Page object
	Public SiteCategory_search As cSiteCategory_search

	'
	' Page Class
	'
	Class cSiteCategory_search
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
				If SiteCategory.UseTokenInUrl Then Url = Url & "t=" & SiteCategory.TableVar & "&" ' Add page token
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
			If SiteCategory.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (SiteCategory.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (SiteCategory.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' ASP.NET page object
		Public ReadOnly Property AspNetPage() As sitecategory_srch
			Get
				Return CType(m_ParentPage, sitecategory_srch)
			End Get
		End Property

		' SiteCategory
		Public Property SiteCategory() As cSiteCategory
			Get				
				Return ParentPage.SiteCategory
			End Get
			Set(ByVal v As cSiteCategory)
				ParentPage.SiteCategory = v	
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
			m_PageObjName = "SiteCategory_search"
			m_PageObjTypeName = "cSiteCategory_search"			

			' Initialize language object
			Language = New cLanguage(Me)

			' Table Name
			m_TableName = "SiteCategory"

			' Initialize table object
			SiteCategory = New cSiteCategory(Me)

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
			SiteCategory.Dispose()
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
			SiteCategory.CurrentAction = ObjForm.GetValue("a_search")
			Select Case SiteCategory.CurrentAction
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
						sSrchStr = SiteCategory.UrlParm(sSrchStr)
					Page_Terminate("sitecategory_list.aspx" & "?" & sSrchStr) ' Go to list page
					End If
			End Select
		End If

		' Restore search settings from Session
		If ParentPage.gsSearchError = "" Then
			LoadAdvancedSearch()
		End If

		' Render row for search
		SiteCategory.RowType = EW_ROWTYPE_SEARCH
		RenderRow()
	End Sub

	'
	' Build advanced search
	'
	Function BuildAdvancedSearch() As String
		Dim sSrchUrl As String = ""
		BuildSearchUrl(sSrchUrl, SiteCategory.SiteCategoryID) ' SiteCategoryID
		BuildSearchUrl(sSrchUrl, SiteCategory.CategoryKeywords) ' CategoryKeywords
		BuildSearchUrl(sSrchUrl, SiteCategory.CategoryName) ' CategoryName
		BuildSearchUrl(sSrchUrl, SiteCategory.CategoryTitle) ' CategoryTitle
		BuildSearchUrl(sSrchUrl, SiteCategory.CategoryDescription) ' CategoryDescription
		BuildSearchUrl(sSrchUrl, SiteCategory.GroupOrder) ' GroupOrder
		BuildSearchUrl(sSrchUrl, SiteCategory.ParentCategoryID) ' ParentCategoryID
		BuildSearchUrl(sSrchUrl, SiteCategory.CategoryFileName) ' CategoryFileName
		BuildSearchUrl(sSrchUrl, SiteCategory.SiteCategoryTypeID) ' SiteCategoryTypeID
		BuildSearchUrl(sSrchUrl, SiteCategory.SiteCategoryGroupID) ' SiteCategoryGroupID
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
		SiteCategory.SiteCategoryID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_SiteCategoryID")
    	SiteCategory.SiteCategoryID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_SiteCategoryID")
		SiteCategory.CategoryKeywords.AdvancedSearch.SearchValue = ObjForm.GetValue("x_CategoryKeywords")
    	SiteCategory.CategoryKeywords.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_CategoryKeywords")
		SiteCategory.CategoryName.AdvancedSearch.SearchValue = ObjForm.GetValue("x_CategoryName")
    	SiteCategory.CategoryName.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_CategoryName")
		SiteCategory.CategoryTitle.AdvancedSearch.SearchValue = ObjForm.GetValue("x_CategoryTitle")
    	SiteCategory.CategoryTitle.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_CategoryTitle")
		SiteCategory.CategoryDescription.AdvancedSearch.SearchValue = ObjForm.GetValue("x_CategoryDescription")
    	SiteCategory.CategoryDescription.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_CategoryDescription")
		SiteCategory.GroupOrder.AdvancedSearch.SearchValue = ObjForm.GetValue("x_GroupOrder")
    	SiteCategory.GroupOrder.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_GroupOrder")
		SiteCategory.ParentCategoryID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_ParentCategoryID")
    	SiteCategory.ParentCategoryID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_ParentCategoryID")
		SiteCategory.CategoryFileName.AdvancedSearch.SearchValue = ObjForm.GetValue("x_CategoryFileName")
    	SiteCategory.CategoryFileName.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_CategoryFileName")
		SiteCategory.SiteCategoryTypeID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_SiteCategoryTypeID")
    	SiteCategory.SiteCategoryTypeID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_SiteCategoryTypeID")
		SiteCategory.SiteCategoryGroupID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_SiteCategoryGroupID")
    	SiteCategory.SiteCategoryGroupID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_SiteCategoryGroupID")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Initialize urls
		' Row Rendering event

		SiteCategory.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' SiteCategoryID

		SiteCategory.SiteCategoryID.CellCssStyle = ""
		SiteCategory.SiteCategoryID.CellCssClass = ""
		SiteCategory.SiteCategoryID.CellAttrs.Clear(): SiteCategory.SiteCategoryID.ViewAttrs.Clear(): SiteCategory.SiteCategoryID.EditAttrs.Clear()

		' CategoryKeywords
		SiteCategory.CategoryKeywords.CellCssStyle = ""
		SiteCategory.CategoryKeywords.CellCssClass = ""
		SiteCategory.CategoryKeywords.CellAttrs.Clear(): SiteCategory.CategoryKeywords.ViewAttrs.Clear(): SiteCategory.CategoryKeywords.EditAttrs.Clear()

		' CategoryName
		SiteCategory.CategoryName.CellCssStyle = ""
		SiteCategory.CategoryName.CellCssClass = ""
		SiteCategory.CategoryName.CellAttrs.Clear(): SiteCategory.CategoryName.ViewAttrs.Clear(): SiteCategory.CategoryName.EditAttrs.Clear()

		' CategoryTitle
		SiteCategory.CategoryTitle.CellCssStyle = ""
		SiteCategory.CategoryTitle.CellCssClass = ""
		SiteCategory.CategoryTitle.CellAttrs.Clear(): SiteCategory.CategoryTitle.ViewAttrs.Clear(): SiteCategory.CategoryTitle.EditAttrs.Clear()

		' CategoryDescription
		SiteCategory.CategoryDescription.CellCssStyle = ""
		SiteCategory.CategoryDescription.CellCssClass = ""
		SiteCategory.CategoryDescription.CellAttrs.Clear(): SiteCategory.CategoryDescription.ViewAttrs.Clear(): SiteCategory.CategoryDescription.EditAttrs.Clear()

		' GroupOrder
		SiteCategory.GroupOrder.CellCssStyle = ""
		SiteCategory.GroupOrder.CellCssClass = ""
		SiteCategory.GroupOrder.CellAttrs.Clear(): SiteCategory.GroupOrder.ViewAttrs.Clear(): SiteCategory.GroupOrder.EditAttrs.Clear()

		' ParentCategoryID
		SiteCategory.ParentCategoryID.CellCssStyle = ""
		SiteCategory.ParentCategoryID.CellCssClass = ""
		SiteCategory.ParentCategoryID.CellAttrs.Clear(): SiteCategory.ParentCategoryID.ViewAttrs.Clear(): SiteCategory.ParentCategoryID.EditAttrs.Clear()

		' CategoryFileName
		SiteCategory.CategoryFileName.CellCssStyle = ""
		SiteCategory.CategoryFileName.CellCssClass = ""
		SiteCategory.CategoryFileName.CellAttrs.Clear(): SiteCategory.CategoryFileName.ViewAttrs.Clear(): SiteCategory.CategoryFileName.EditAttrs.Clear()

		' SiteCategoryTypeID
		SiteCategory.SiteCategoryTypeID.CellCssStyle = ""
		SiteCategory.SiteCategoryTypeID.CellCssClass = ""
		SiteCategory.SiteCategoryTypeID.CellAttrs.Clear(): SiteCategory.SiteCategoryTypeID.ViewAttrs.Clear(): SiteCategory.SiteCategoryTypeID.EditAttrs.Clear()

		' SiteCategoryGroupID
		SiteCategory.SiteCategoryGroupID.CellCssStyle = ""
		SiteCategory.SiteCategoryGroupID.CellCssClass = ""
		SiteCategory.SiteCategoryGroupID.CellAttrs.Clear(): SiteCategory.SiteCategoryGroupID.ViewAttrs.Clear(): SiteCategory.SiteCategoryGroupID.EditAttrs.Clear()

		'
		'  View  Row
		'

		If SiteCategory.RowType = EW_ROWTYPE_VIEW Then ' View row

			' SiteCategoryID
			SiteCategory.SiteCategoryID.ViewValue = SiteCategory.SiteCategoryID.CurrentValue
			SiteCategory.SiteCategoryID.CssStyle = ""
			SiteCategory.SiteCategoryID.CssClass = ""
			SiteCategory.SiteCategoryID.ViewCustomAttributes = ""

			' CategoryKeywords
			SiteCategory.CategoryKeywords.ViewValue = SiteCategory.CategoryKeywords.CurrentValue
			SiteCategory.CategoryKeywords.CssStyle = ""
			SiteCategory.CategoryKeywords.CssClass = ""
			SiteCategory.CategoryKeywords.ViewCustomAttributes = ""

			' CategoryName
			SiteCategory.CategoryName.ViewValue = SiteCategory.CategoryName.CurrentValue
			SiteCategory.CategoryName.CssStyle = ""
			SiteCategory.CategoryName.CssClass = ""
			SiteCategory.CategoryName.ViewCustomAttributes = ""

			' CategoryTitle
			SiteCategory.CategoryTitle.ViewValue = SiteCategory.CategoryTitle.CurrentValue
			SiteCategory.CategoryTitle.CssStyle = ""
			SiteCategory.CategoryTitle.CssClass = ""
			SiteCategory.CategoryTitle.ViewCustomAttributes = ""

			' CategoryDescription
			SiteCategory.CategoryDescription.ViewValue = SiteCategory.CategoryDescription.CurrentValue
			SiteCategory.CategoryDescription.CssStyle = ""
			SiteCategory.CategoryDescription.CssClass = ""
			SiteCategory.CategoryDescription.ViewCustomAttributes = ""

			' GroupOrder
			SiteCategory.GroupOrder.ViewValue = SiteCategory.GroupOrder.CurrentValue
			SiteCategory.GroupOrder.CssStyle = ""
			SiteCategory.GroupOrder.CssClass = ""
			SiteCategory.GroupOrder.ViewCustomAttributes = ""

			' ParentCategoryID
			SiteCategory.ParentCategoryID.ViewValue = SiteCategory.ParentCategoryID.CurrentValue
			SiteCategory.ParentCategoryID.CssStyle = ""
			SiteCategory.ParentCategoryID.CssClass = ""
			SiteCategory.ParentCategoryID.ViewCustomAttributes = ""

			' CategoryFileName
			SiteCategory.CategoryFileName.ViewValue = SiteCategory.CategoryFileName.CurrentValue
			SiteCategory.CategoryFileName.CssStyle = ""
			SiteCategory.CategoryFileName.CssClass = ""
			SiteCategory.CategoryFileName.ViewCustomAttributes = ""

			' SiteCategoryTypeID
			SiteCategory.SiteCategoryTypeID.ViewValue = SiteCategory.SiteCategoryTypeID.CurrentValue
			SiteCategory.SiteCategoryTypeID.CssStyle = ""
			SiteCategory.SiteCategoryTypeID.CssClass = ""
			SiteCategory.SiteCategoryTypeID.ViewCustomAttributes = ""

			' SiteCategoryGroupID
			SiteCategory.SiteCategoryGroupID.ViewValue = SiteCategory.SiteCategoryGroupID.CurrentValue
			SiteCategory.SiteCategoryGroupID.CssStyle = ""
			SiteCategory.SiteCategoryGroupID.CssClass = ""
			SiteCategory.SiteCategoryGroupID.ViewCustomAttributes = ""

			' View refer script
			' SiteCategoryID

			SiteCategory.SiteCategoryID.HrefValue = ""
			SiteCategory.SiteCategoryID.TooltipValue = ""

			' CategoryKeywords
			SiteCategory.CategoryKeywords.HrefValue = ""
			SiteCategory.CategoryKeywords.TooltipValue = ""

			' CategoryName
			SiteCategory.CategoryName.HrefValue = ""
			SiteCategory.CategoryName.TooltipValue = ""

			' CategoryTitle
			SiteCategory.CategoryTitle.HrefValue = ""
			SiteCategory.CategoryTitle.TooltipValue = ""

			' CategoryDescription
			SiteCategory.CategoryDescription.HrefValue = ""
			SiteCategory.CategoryDescription.TooltipValue = ""

			' GroupOrder
			SiteCategory.GroupOrder.HrefValue = ""
			SiteCategory.GroupOrder.TooltipValue = ""

			' ParentCategoryID
			SiteCategory.ParentCategoryID.HrefValue = ""
			SiteCategory.ParentCategoryID.TooltipValue = ""

			' CategoryFileName
			SiteCategory.CategoryFileName.HrefValue = ""
			SiteCategory.CategoryFileName.TooltipValue = ""

			' SiteCategoryTypeID
			SiteCategory.SiteCategoryTypeID.HrefValue = ""
			SiteCategory.SiteCategoryTypeID.TooltipValue = ""

			' SiteCategoryGroupID
			SiteCategory.SiteCategoryGroupID.HrefValue = ""
			SiteCategory.SiteCategoryGroupID.TooltipValue = ""

		'
		'  Search Row
		'

		ElseIf SiteCategory.RowType = EW_ROWTYPE_SEARCH Then ' Search row

			' SiteCategoryID
			SiteCategory.SiteCategoryID.EditCustomAttributes = ""
			SiteCategory.SiteCategoryID.EditValue = ew_HtmlEncode(SiteCategory.SiteCategoryID.AdvancedSearch.SearchValue)

			' CategoryKeywords
			SiteCategory.CategoryKeywords.EditCustomAttributes = ""
			SiteCategory.CategoryKeywords.EditValue = ew_HtmlEncode(SiteCategory.CategoryKeywords.AdvancedSearch.SearchValue)

			' CategoryName
			SiteCategory.CategoryName.EditCustomAttributes = ""
			SiteCategory.CategoryName.EditValue = ew_HtmlEncode(SiteCategory.CategoryName.AdvancedSearch.SearchValue)

			' CategoryTitle
			SiteCategory.CategoryTitle.EditCustomAttributes = ""
			SiteCategory.CategoryTitle.EditValue = ew_HtmlEncode(SiteCategory.CategoryTitle.AdvancedSearch.SearchValue)

			' CategoryDescription
			SiteCategory.CategoryDescription.EditCustomAttributes = ""
			SiteCategory.CategoryDescription.EditValue = ew_HtmlEncode(SiteCategory.CategoryDescription.AdvancedSearch.SearchValue)

			' GroupOrder
			SiteCategory.GroupOrder.EditCustomAttributes = ""
			SiteCategory.GroupOrder.EditValue = ew_HtmlEncode(SiteCategory.GroupOrder.AdvancedSearch.SearchValue)

			' ParentCategoryID
			SiteCategory.ParentCategoryID.EditCustomAttributes = ""
			SiteCategory.ParentCategoryID.EditValue = ew_HtmlEncode(SiteCategory.ParentCategoryID.AdvancedSearch.SearchValue)

			' CategoryFileName
			SiteCategory.CategoryFileName.EditCustomAttributes = ""
			SiteCategory.CategoryFileName.EditValue = ew_HtmlEncode(SiteCategory.CategoryFileName.AdvancedSearch.SearchValue)

			' SiteCategoryTypeID
			SiteCategory.SiteCategoryTypeID.EditCustomAttributes = ""
			SiteCategory.SiteCategoryTypeID.EditValue = ew_HtmlEncode(SiteCategory.SiteCategoryTypeID.AdvancedSearch.SearchValue)

			' SiteCategoryGroupID
			SiteCategory.SiteCategoryGroupID.EditCustomAttributes = ""
			SiteCategory.SiteCategoryGroupID.EditValue = ew_HtmlEncode(SiteCategory.SiteCategoryGroupID.AdvancedSearch.SearchValue)
		End If

		' Row Rendered event
		If SiteCategory.RowType <> EW_ROWTYPE_AGGREGATEINIT Then
			SiteCategory.Row_Rendered()
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
		If Not ew_CheckInteger(SiteCategory.SiteCategoryID.AdvancedSearch.SearchValue) Then
			If ParentPage.gsSearchError <> "" Then ParentPage.gsSearchError = ParentPage.gsSearchError & "<br />"
			ParentPage.gsSearchError &= SiteCategory.SiteCategoryID.FldErrMsg
		End If
		If Not ew_CheckNumber(SiteCategory.GroupOrder.AdvancedSearch.SearchValue) Then
			If ParentPage.gsSearchError <> "" Then ParentPage.gsSearchError = ParentPage.gsSearchError & "<br />"
			ParentPage.gsSearchError &= SiteCategory.GroupOrder.FldErrMsg
		End If
		If Not ew_CheckInteger(SiteCategory.SiteCategoryTypeID.AdvancedSearch.SearchValue) Then
			If ParentPage.gsSearchError <> "" Then ParentPage.gsSearchError = ParentPage.gsSearchError & "<br />"
			ParentPage.gsSearchError &= SiteCategory.SiteCategoryTypeID.FldErrMsg
		End If
		If Not ew_CheckInteger(SiteCategory.SiteCategoryGroupID.AdvancedSearch.SearchValue) Then
			If ParentPage.gsSearchError <> "" Then ParentPage.gsSearchError = ParentPage.gsSearchError & "<br />"
			ParentPage.gsSearchError &= SiteCategory.SiteCategoryGroupID.FldErrMsg
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
		SiteCategory.SiteCategoryID.AdvancedSearch.SearchValue = SiteCategory.GetAdvancedSearch("x_SiteCategoryID")
		SiteCategory.CategoryKeywords.AdvancedSearch.SearchValue = SiteCategory.GetAdvancedSearch("x_CategoryKeywords")
		SiteCategory.CategoryName.AdvancedSearch.SearchValue = SiteCategory.GetAdvancedSearch("x_CategoryName")
		SiteCategory.CategoryTitle.AdvancedSearch.SearchValue = SiteCategory.GetAdvancedSearch("x_CategoryTitle")
		SiteCategory.CategoryDescription.AdvancedSearch.SearchValue = SiteCategory.GetAdvancedSearch("x_CategoryDescription")
		SiteCategory.GroupOrder.AdvancedSearch.SearchValue = SiteCategory.GetAdvancedSearch("x_GroupOrder")
		SiteCategory.ParentCategoryID.AdvancedSearch.SearchValue = SiteCategory.GetAdvancedSearch("x_ParentCategoryID")
		SiteCategory.CategoryFileName.AdvancedSearch.SearchValue = SiteCategory.GetAdvancedSearch("x_CategoryFileName")
		SiteCategory.SiteCategoryTypeID.AdvancedSearch.SearchValue = SiteCategory.GetAdvancedSearch("x_SiteCategoryTypeID")
		SiteCategory.SiteCategoryGroupID.AdvancedSearch.SearchValue = SiteCategory.GetAdvancedSearch("x_SiteCategoryGroupID")
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
		SiteCategory_search = New cSiteCategory_search(Me)		
		SiteCategory_search.Page_Init()		

		' Set buffer/cache option
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

		' Page main processing
		SiteCategory_search.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If SiteCategory_search IsNot Nothing Then SiteCategory_search.Dispose()
	End Sub
End Class
