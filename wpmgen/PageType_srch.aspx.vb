Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class PageType_srch
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public PageType_search As cPageType_search

	'
	' Page Class
	'
	Class cPageType_search
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
				If PageType.UseTokenInUrl Then Url = Url & "t=" & PageType.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If PageType.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (PageType.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (PageType.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' PageType
		Public Property PageType() As cPageType
			Get				
				Return ParentPage.PageType
			End Get
			Set(ByVal v As cPageType)
				ParentPage.PageType = v	
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
			m_PageID = "search"
			m_PageObjName = "PageType_search"
			m_PageObjTypeName = "cPageType_search"

			' Table Name
			m_TableName = "PageType"

			' Initialize table object
			PageType = New cPageType(Me)

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
			PageType.Dispose()

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
		ObjForm = New cFormObj
		If IsPageRequest Then ' Validate request

			' Get action
			PageType.CurrentAction = ObjForm.GetValue("a_search")
			Select Case PageType.CurrentAction
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
						sSrchStr = PageType.UrlParm(sSrchStr)
					Page_Terminate("PageType_list.aspx" & "?" & sSrchStr) ' Go to list page
					End If
			End Select
		End If

		' Restore search settings from Session
		If ParentPage.gsSearchError = "" Then
			LoadAdvancedSearch()
		End If

		' Render row for search
		PageType.RowType = EW_ROWTYPE_SEARCH
		RenderRow()
	End Sub

	'
	' Build advanced search
	'
	Function BuildAdvancedSearch() As String
		Dim sSrchUrl As String = ""
		BuildSearchUrl(sSrchUrl, PageType.PageTypeID) ' PageTypeID
		BuildSearchUrl(sSrchUrl, PageType.PageTypeCD) ' PageTypeCD
		BuildSearchUrl(sSrchUrl, PageType.PageTypeDesc) ' PageTypeDesc
		BuildSearchUrl(sSrchUrl, PageType.PageFileName) ' PageFileName
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
		If ew_SameText(FldOpr, "BETWEEN") Then
			IsValidValue = (Fld.FldDataType <> EW_DATATYPE_NUMBER) OrElse _
				(Fld.FldDataType = EW_DATATYPE_NUMBER AndAlso IsNumeric(FldVal) AndAlso IsNumeric(FldVal2))
			If ew_NotEmpty(FldVal) AndAlso ew_NotEmpty(FldVal2) AndAlso IsValidValue Then
				sWrk = "x_" & FldParm & "=" & HttpContext.Current.Server.UrlEncode(FldVal) & _
					"&y_" & FldParm & "=" & HttpContext.Current.Server.UrlEncode(FldVal2) & _
					"&z_" & FldParm & "=" & HttpContext.Current.Server.UrlEncode(FldOpr)
			End If
		ElseIf ew_SameText(FldOpr, "IS NULL") OrElse ew_SameText(FldOpr, "IS NOT NULL") Then
			sWrk = "x_" & FldParm & "=" & HttpContext.Current.Server.UrlEncode(FldVal) & _
				"&z_" & FldParm & "=" & HttpContext.Current.Server.UrlEncode(FldOpr)
		Else
			IsValidValue = (Fld.FldDataType <> EW_DATATYPE_NUMBER) OrElse _
				(Fld.FldDataType = EW_DATATYPE_NUMBER AndAlso IsNumeric(FldVal))
			If ew_NotEmpty(FldVal) AndAlso IsValidValue AndAlso ew_IsValidOpr(FldOpr, Fld.FldDataType) Then
				sWrk = "x_" & FldParm & "=" & HttpContext.Current.Server.UrlEncode(FldVal) & _
					"&z_" & FldParm & "=" & HttpContext.Current.Server.UrlEncode(FldOpr)
			End If
			IsValidValue = (Fld.FldDataType <> EW_DATATYPE_NUMBER) OrElse _
				(Fld.FldDataType = EW_DATATYPE_NUMBER AndAlso IsNumeric(FldVal2))
			If ew_NotEmpty(FldVal2) AndAlso IsValidValue AndAlso ew_IsValidOpr(FldOpr2, Fld.FldDataType) Then
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
		PageType.PageTypeID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_PageTypeID")
    	PageType.PageTypeID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_PageTypeID")
		PageType.PageTypeCD.AdvancedSearch.SearchValue = ObjForm.GetValue("x_PageTypeCD")
    	PageType.PageTypeCD.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_PageTypeCD")
		PageType.PageTypeDesc.AdvancedSearch.SearchValue = ObjForm.GetValue("x_PageTypeDesc")
    	PageType.PageTypeDesc.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_PageTypeDesc")
		PageType.PageFileName.AdvancedSearch.SearchValue = ObjForm.GetValue("x_PageFileName")
    	PageType.PageFileName.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_PageFileName")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		PageType.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' PageTypeID

		PageType.PageTypeID.CellCssStyle = ""
		PageType.PageTypeID.CellCssClass = ""

		' PageTypeCD
		PageType.PageTypeCD.CellCssStyle = ""
		PageType.PageTypeCD.CellCssClass = ""

		' PageTypeDesc
		PageType.PageTypeDesc.CellCssStyle = ""
		PageType.PageTypeDesc.CellCssClass = ""

		' PageFileName
		PageType.PageFileName.CellCssStyle = ""
		PageType.PageFileName.CellCssClass = ""

		'
		'  View  Row
		'

		If PageType.RowType = EW_ROWTYPE_VIEW Then ' View row

			' PageTypeID
			PageType.PageTypeID.ViewValue = PageType.PageTypeID.CurrentValue
			PageType.PageTypeID.CssStyle = ""
			PageType.PageTypeID.CssClass = ""
			PageType.PageTypeID.ViewCustomAttributes = ""

			' PageTypeCD
			PageType.PageTypeCD.ViewValue = PageType.PageTypeCD.CurrentValue
			PageType.PageTypeCD.CssStyle = ""
			PageType.PageTypeCD.CssClass = ""
			PageType.PageTypeCD.ViewCustomAttributes = ""

			' PageTypeDesc
			PageType.PageTypeDesc.ViewValue = PageType.PageTypeDesc.CurrentValue
			PageType.PageTypeDesc.CssStyle = ""
			PageType.PageTypeDesc.CssClass = ""
			PageType.PageTypeDesc.ViewCustomAttributes = ""

			' PageFileName
			PageType.PageFileName.ViewValue = PageType.PageFileName.CurrentValue
			PageType.PageFileName.CssStyle = ""
			PageType.PageFileName.CssClass = ""
			PageType.PageFileName.ViewCustomAttributes = ""

			' View refer script
			' PageTypeID

			PageType.PageTypeID.HrefValue = ""

			' PageTypeCD
			PageType.PageTypeCD.HrefValue = ""

			' PageTypeDesc
			PageType.PageTypeDesc.HrefValue = ""

			' PageFileName
			PageType.PageFileName.HrefValue = ""

		'
		'  Search Row
		'

		ElseIf PageType.RowType = EW_ROWTYPE_SEARCH Then ' Search row

			' PageTypeID
			PageType.PageTypeID.EditCustomAttributes = ""
			PageType.PageTypeID.EditValue = ew_HtmlEncode(PageType.PageTypeID.AdvancedSearch.SearchValue)

			' PageTypeCD
			PageType.PageTypeCD.EditCustomAttributes = ""
			PageType.PageTypeCD.EditValue = ew_HtmlEncode(PageType.PageTypeCD.AdvancedSearch.SearchValue)

			' PageTypeDesc
			PageType.PageTypeDesc.EditCustomAttributes = ""
			PageType.PageTypeDesc.EditValue = ew_HtmlEncode(PageType.PageTypeDesc.AdvancedSearch.SearchValue)

			' PageFileName
			PageType.PageFileName.EditCustomAttributes = ""
			PageType.PageFileName.EditValue = ew_HtmlEncode(PageType.PageFileName.AdvancedSearch.SearchValue)
		End If

		' Row Rendered event
		PageType.Row_Rendered()
	End Sub

	'
	' Validate search
	'
	Function ValidateSearch() As Boolean

		' Initialize
		ParentPage.gsSearchError = ""

		' Check if validation required
		If Not EW_SERVER_VALIDATE Then Return True ' Skip
		If Not ew_CheckInteger(PageType.PageTypeID.AdvancedSearch.SearchValue) Then
			If ParentPage.gsSearchError <> "" Then ParentPage.gsSearchError = ParentPage.gsSearchError & "<br />"
			ParentPage.gsSearchError = ParentPage.gsSearchError & "Incorrect integer - Page Type ID"
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
		PageType.PageTypeID.AdvancedSearch.SearchValue = PageType.GetAdvancedSearch("x_PageTypeID")
		PageType.PageTypeCD.AdvancedSearch.SearchValue = PageType.GetAdvancedSearch("x_PageTypeCD")
		PageType.PageTypeDesc.AdvancedSearch.SearchValue = PageType.GetAdvancedSearch("x_PageTypeDesc")
		PageType.PageFileName.AdvancedSearch.SearchValue = PageType.GetAdvancedSearch("x_PageFileName")
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
		PageType_search = New cPageType_search(Me)		
		PageType_search.Page_Init()

		' Page main processing
		PageType_search.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If PageType_search IsNot Nothing Then PageType_search.Dispose()
	End Sub
End Class
