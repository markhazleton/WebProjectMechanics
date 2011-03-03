Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class siteparametertype_srch
	Inherits AspNetMaker8_wpmWebsite

	' Page object
	Public SiteParameterType_search As cSiteParameterType_search

	'
	' Page Class
	'
	Class cSiteParameterType_search
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
				If SiteParameterType.UseTokenInUrl Then Url = Url & "t=" & SiteParameterType.TableVar & "&" ' Add page token
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
			If SiteParameterType.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (SiteParameterType.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (SiteParameterType.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' ASP.NET page object
		Public ReadOnly Property AspNetPage() As siteparametertype_srch
			Get
				Return CType(m_ParentPage, siteparametertype_srch)
			End Get
		End Property

		' SiteParameterType
		Public Property SiteParameterType() As cSiteParameterType
			Get				
				Return ParentPage.SiteParameterType
			End Get
			Set(ByVal v As cSiteParameterType)
				ParentPage.SiteParameterType = v	
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
			m_PageObjName = "SiteParameterType_search"
			m_PageObjTypeName = "cSiteParameterType_search"			

			' Initialize language object
			Language = New cLanguage(Me)

			' Table Name
			m_TableName = "SiteParameterType"

			' Initialize table object
			SiteParameterType = New cSiteParameterType(Me)

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
			SiteParameterType.Dispose()
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
			SiteParameterType.CurrentAction = ObjForm.GetValue("a_search")
			Select Case SiteParameterType.CurrentAction
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
						sSrchStr = SiteParameterType.UrlParm(sSrchStr)
					Page_Terminate("siteparametertype_list.aspx" & "?" & sSrchStr) ' Go to list page
					End If
			End Select
		End If

		' Restore search settings from Session
		If ParentPage.gsSearchError = "" Then
			LoadAdvancedSearch()
		End If

		' Render row for search
		SiteParameterType.RowType = EW_ROWTYPE_SEARCH
		RenderRow()
	End Sub

	'
	' Build advanced search
	'
	Function BuildAdvancedSearch() As String
		Dim sSrchUrl As String = ""
		BuildSearchUrl(sSrchUrl, SiteParameterType.SiteParameterTypeNM) ' SiteParameterTypeNM
		BuildSearchUrl(sSrchUrl, SiteParameterType.SiteParameterTypeDS) ' SiteParameterTypeDS
		BuildSearchUrl(sSrchUrl, SiteParameterType.SiteParameterTypeOrder) ' SiteParameterTypeOrder
		BuildSearchUrl(sSrchUrl, SiteParameterType.SiteParameterTemplate) ' SiteParameterTemplate
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
		SiteParameterType.SiteParameterTypeNM.AdvancedSearch.SearchValue = ObjForm.GetValue("x_SiteParameterTypeNM")
    	SiteParameterType.SiteParameterTypeNM.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_SiteParameterTypeNM")
		SiteParameterType.SiteParameterTypeDS.AdvancedSearch.SearchValue = ObjForm.GetValue("x_SiteParameterTypeDS")
    	SiteParameterType.SiteParameterTypeDS.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_SiteParameterTypeDS")
		SiteParameterType.SiteParameterTypeOrder.AdvancedSearch.SearchValue = ObjForm.GetValue("x_SiteParameterTypeOrder")
    	SiteParameterType.SiteParameterTypeOrder.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_SiteParameterTypeOrder")
		SiteParameterType.SiteParameterTemplate.AdvancedSearch.SearchValue = ObjForm.GetValue("x_SiteParameterTemplate")
    	SiteParameterType.SiteParameterTemplate.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_SiteParameterTemplate")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Initialize urls
		' Row Rendering event

		SiteParameterType.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' SiteParameterTypeNM

		SiteParameterType.SiteParameterTypeNM.CellCssStyle = ""
		SiteParameterType.SiteParameterTypeNM.CellCssClass = ""
		SiteParameterType.SiteParameterTypeNM.CellAttrs.Clear(): SiteParameterType.SiteParameterTypeNM.ViewAttrs.Clear(): SiteParameterType.SiteParameterTypeNM.EditAttrs.Clear()

		' SiteParameterTypeDS
		SiteParameterType.SiteParameterTypeDS.CellCssStyle = ""
		SiteParameterType.SiteParameterTypeDS.CellCssClass = ""
		SiteParameterType.SiteParameterTypeDS.CellAttrs.Clear(): SiteParameterType.SiteParameterTypeDS.ViewAttrs.Clear(): SiteParameterType.SiteParameterTypeDS.EditAttrs.Clear()

		' SiteParameterTypeOrder
		SiteParameterType.SiteParameterTypeOrder.CellCssStyle = ""
		SiteParameterType.SiteParameterTypeOrder.CellCssClass = ""
		SiteParameterType.SiteParameterTypeOrder.CellAttrs.Clear(): SiteParameterType.SiteParameterTypeOrder.ViewAttrs.Clear(): SiteParameterType.SiteParameterTypeOrder.EditAttrs.Clear()

		' SiteParameterTemplate
		SiteParameterType.SiteParameterTemplate.CellCssStyle = ""
		SiteParameterType.SiteParameterTemplate.CellCssClass = ""
		SiteParameterType.SiteParameterTemplate.CellAttrs.Clear(): SiteParameterType.SiteParameterTemplate.ViewAttrs.Clear(): SiteParameterType.SiteParameterTemplate.EditAttrs.Clear()

		'
		'  View  Row
		'

		If SiteParameterType.RowType = EW_ROWTYPE_VIEW Then ' View row

			' SiteParameterTypeID
			SiteParameterType.SiteParameterTypeID.ViewValue = SiteParameterType.SiteParameterTypeID.CurrentValue
			SiteParameterType.SiteParameterTypeID.CssStyle = ""
			SiteParameterType.SiteParameterTypeID.CssClass = ""
			SiteParameterType.SiteParameterTypeID.ViewCustomAttributes = ""

			' SiteParameterTypeNM
			SiteParameterType.SiteParameterTypeNM.ViewValue = SiteParameterType.SiteParameterTypeNM.CurrentValue
			SiteParameterType.SiteParameterTypeNM.CssStyle = ""
			SiteParameterType.SiteParameterTypeNM.CssClass = ""
			SiteParameterType.SiteParameterTypeNM.ViewCustomAttributes = ""

			' SiteParameterTypeDS
			SiteParameterType.SiteParameterTypeDS.ViewValue = SiteParameterType.SiteParameterTypeDS.CurrentValue
			SiteParameterType.SiteParameterTypeDS.CssStyle = ""
			SiteParameterType.SiteParameterTypeDS.CssClass = ""
			SiteParameterType.SiteParameterTypeDS.ViewCustomAttributes = ""

			' SiteParameterTypeOrder
			SiteParameterType.SiteParameterTypeOrder.ViewValue = SiteParameterType.SiteParameterTypeOrder.CurrentValue
			SiteParameterType.SiteParameterTypeOrder.CssStyle = ""
			SiteParameterType.SiteParameterTypeOrder.CssClass = ""
			SiteParameterType.SiteParameterTypeOrder.ViewCustomAttributes = ""

			' SiteParameterTemplate
			SiteParameterType.SiteParameterTemplate.ViewValue = SiteParameterType.SiteParameterTemplate.CurrentValue
			SiteParameterType.SiteParameterTemplate.CssStyle = ""
			SiteParameterType.SiteParameterTemplate.CssClass = ""
			SiteParameterType.SiteParameterTemplate.ViewCustomAttributes = ""

			' View refer script
			' SiteParameterTypeNM

			SiteParameterType.SiteParameterTypeNM.HrefValue = ""
			SiteParameterType.SiteParameterTypeNM.TooltipValue = ""

			' SiteParameterTypeDS
			SiteParameterType.SiteParameterTypeDS.HrefValue = ""
			SiteParameterType.SiteParameterTypeDS.TooltipValue = ""

			' SiteParameterTypeOrder
			SiteParameterType.SiteParameterTypeOrder.HrefValue = ""
			SiteParameterType.SiteParameterTypeOrder.TooltipValue = ""

			' SiteParameterTemplate
			SiteParameterType.SiteParameterTemplate.HrefValue = ""
			SiteParameterType.SiteParameterTemplate.TooltipValue = ""

		'
		'  Search Row
		'

		ElseIf SiteParameterType.RowType = EW_ROWTYPE_SEARCH Then ' Search row

			' SiteParameterTypeNM
			SiteParameterType.SiteParameterTypeNM.EditCustomAttributes = ""
			SiteParameterType.SiteParameterTypeNM.EditValue = ew_HtmlEncode(SiteParameterType.SiteParameterTypeNM.AdvancedSearch.SearchValue)

			' SiteParameterTypeDS
			SiteParameterType.SiteParameterTypeDS.EditCustomAttributes = ""
			SiteParameterType.SiteParameterTypeDS.EditValue = ew_HtmlEncode(SiteParameterType.SiteParameterTypeDS.AdvancedSearch.SearchValue)

			' SiteParameterTypeOrder
			SiteParameterType.SiteParameterTypeOrder.EditCustomAttributes = ""
			SiteParameterType.SiteParameterTypeOrder.EditValue = ew_HtmlEncode(SiteParameterType.SiteParameterTypeOrder.AdvancedSearch.SearchValue)

			' SiteParameterTemplate
			SiteParameterType.SiteParameterTemplate.EditCustomAttributes = ""
			SiteParameterType.SiteParameterTemplate.EditValue = ew_HtmlEncode(SiteParameterType.SiteParameterTemplate.AdvancedSearch.SearchValue)
		End If

		' Row Rendered event
		If SiteParameterType.RowType <> EW_ROWTYPE_AGGREGATEINIT Then
			SiteParameterType.Row_Rendered()
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
		If Not ew_CheckInteger(SiteParameterType.SiteParameterTypeOrder.AdvancedSearch.SearchValue) Then
			If ParentPage.gsSearchError <> "" Then ParentPage.gsSearchError = ParentPage.gsSearchError & "<br />"
			ParentPage.gsSearchError &= SiteParameterType.SiteParameterTypeOrder.FldErrMsg
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
		SiteParameterType.SiteParameterTypeNM.AdvancedSearch.SearchValue = SiteParameterType.GetAdvancedSearch("x_SiteParameterTypeNM")
		SiteParameterType.SiteParameterTypeDS.AdvancedSearch.SearchValue = SiteParameterType.GetAdvancedSearch("x_SiteParameterTypeDS")
		SiteParameterType.SiteParameterTypeOrder.AdvancedSearch.SearchValue = SiteParameterType.GetAdvancedSearch("x_SiteParameterTypeOrder")
		SiteParameterType.SiteParameterTemplate.AdvancedSearch.SearchValue = SiteParameterType.GetAdvancedSearch("x_SiteParameterTemplate")
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
		SiteParameterType_search = New cSiteParameterType_search(Me)		
		SiteParameterType_search.Page_Init()		

		' Set buffer/cache option
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

		' Page main processing
		SiteParameterType_search.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If SiteParameterType_search IsNot Nothing Then SiteParameterType_search.Dispose()
	End Sub
End Class
