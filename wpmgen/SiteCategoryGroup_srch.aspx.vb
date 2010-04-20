Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class SiteCategoryGroup_srch
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public SiteCategoryGroup_search As cSiteCategoryGroup_search

	'
	' Page Class
	'
	Class cSiteCategoryGroup_search
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
				If SiteCategoryGroup.UseTokenInUrl Then Url = Url & "t=" & SiteCategoryGroup.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If SiteCategoryGroup.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (SiteCategoryGroup.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (SiteCategoryGroup.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' SiteCategoryGroup
		Public Property SiteCategoryGroup() As cSiteCategoryGroup
			Get				
				Return ParentPage.SiteCategoryGroup
			End Get
			Set(ByVal v As cSiteCategoryGroup)
				ParentPage.SiteCategoryGroup = v	
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
			m_PageObjName = "SiteCategoryGroup_search"
			m_PageObjTypeName = "cSiteCategoryGroup_search"

			' Table Name
			m_TableName = "SiteCategoryGroup"

			' Initialize table object
			SiteCategoryGroup = New cSiteCategoryGroup(Me)

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
			SiteCategoryGroup.Dispose()

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
			SiteCategoryGroup.CurrentAction = ObjForm.GetValue("a_search")
			Select Case SiteCategoryGroup.CurrentAction
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
						sSrchStr = SiteCategoryGroup.UrlParm(sSrchStr)
					Page_Terminate("SiteCategoryGroup_list.aspx" & "?" & sSrchStr) ' Go to list page
					End If
			End Select
		End If

		' Restore search settings from Session
		If ParentPage.gsSearchError = "" Then
			LoadAdvancedSearch()
		End If

		' Render row for search
		SiteCategoryGroup.RowType = EW_ROWTYPE_SEARCH
		RenderRow()
	End Sub

	'
	' Build advanced search
	'
	Function BuildAdvancedSearch() As String
		Dim sSrchUrl As String = ""
		BuildSearchUrl(sSrchUrl, SiteCategoryGroup.SiteCategoryGroupID) ' SiteCategoryGroupID
		BuildSearchUrl(sSrchUrl, SiteCategoryGroup.SiteCategoryGroupNM) ' SiteCategoryGroupNM
		BuildSearchUrl(sSrchUrl, SiteCategoryGroup.SiteCategoryGroupDS) ' SiteCategoryGroupDS
		BuildSearchUrl(sSrchUrl, SiteCategoryGroup.SiteCategoryGroupOrder) ' SiteCategoryGroupOrder
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
		SiteCategoryGroup.SiteCategoryGroupID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_SiteCategoryGroupID")
    	SiteCategoryGroup.SiteCategoryGroupID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_SiteCategoryGroupID")
		SiteCategoryGroup.SiteCategoryGroupNM.AdvancedSearch.SearchValue = ObjForm.GetValue("x_SiteCategoryGroupNM")
    	SiteCategoryGroup.SiteCategoryGroupNM.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_SiteCategoryGroupNM")
		SiteCategoryGroup.SiteCategoryGroupDS.AdvancedSearch.SearchValue = ObjForm.GetValue("x_SiteCategoryGroupDS")
    	SiteCategoryGroup.SiteCategoryGroupDS.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_SiteCategoryGroupDS")
		SiteCategoryGroup.SiteCategoryGroupOrder.AdvancedSearch.SearchValue = ObjForm.GetValue("x_SiteCategoryGroupOrder")
    	SiteCategoryGroup.SiteCategoryGroupOrder.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_SiteCategoryGroupOrder")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		SiteCategoryGroup.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' SiteCategoryGroupID

		SiteCategoryGroup.SiteCategoryGroupID.CellCssStyle = ""
		SiteCategoryGroup.SiteCategoryGroupID.CellCssClass = ""

		' SiteCategoryGroupNM
		SiteCategoryGroup.SiteCategoryGroupNM.CellCssStyle = ""
		SiteCategoryGroup.SiteCategoryGroupNM.CellCssClass = ""

		' SiteCategoryGroupDS
		SiteCategoryGroup.SiteCategoryGroupDS.CellCssStyle = ""
		SiteCategoryGroup.SiteCategoryGroupDS.CellCssClass = ""

		' SiteCategoryGroupOrder
		SiteCategoryGroup.SiteCategoryGroupOrder.CellCssStyle = ""
		SiteCategoryGroup.SiteCategoryGroupOrder.CellCssClass = ""

		'
		'  View  Row
		'

		If SiteCategoryGroup.RowType = EW_ROWTYPE_VIEW Then ' View row

			' SiteCategoryGroupID
			SiteCategoryGroup.SiteCategoryGroupID.ViewValue = SiteCategoryGroup.SiteCategoryGroupID.CurrentValue
			SiteCategoryGroup.SiteCategoryGroupID.CssStyle = ""
			SiteCategoryGroup.SiteCategoryGroupID.CssClass = ""
			SiteCategoryGroup.SiteCategoryGroupID.ViewCustomAttributes = ""

			' SiteCategoryGroupNM
			SiteCategoryGroup.SiteCategoryGroupNM.ViewValue = SiteCategoryGroup.SiteCategoryGroupNM.CurrentValue
			SiteCategoryGroup.SiteCategoryGroupNM.CssStyle = ""
			SiteCategoryGroup.SiteCategoryGroupNM.CssClass = ""
			SiteCategoryGroup.SiteCategoryGroupNM.ViewCustomAttributes = ""

			' SiteCategoryGroupDS
			SiteCategoryGroup.SiteCategoryGroupDS.ViewValue = SiteCategoryGroup.SiteCategoryGroupDS.CurrentValue
			SiteCategoryGroup.SiteCategoryGroupDS.CssStyle = ""
			SiteCategoryGroup.SiteCategoryGroupDS.CssClass = ""
			SiteCategoryGroup.SiteCategoryGroupDS.ViewCustomAttributes = ""

			' SiteCategoryGroupOrder
			SiteCategoryGroup.SiteCategoryGroupOrder.ViewValue = SiteCategoryGroup.SiteCategoryGroupOrder.CurrentValue
			SiteCategoryGroup.SiteCategoryGroupOrder.CssStyle = ""
			SiteCategoryGroup.SiteCategoryGroupOrder.CssClass = ""
			SiteCategoryGroup.SiteCategoryGroupOrder.ViewCustomAttributes = ""

			' View refer script
			' SiteCategoryGroupID

			SiteCategoryGroup.SiteCategoryGroupID.HrefValue = ""

			' SiteCategoryGroupNM
			SiteCategoryGroup.SiteCategoryGroupNM.HrefValue = ""

			' SiteCategoryGroupDS
			SiteCategoryGroup.SiteCategoryGroupDS.HrefValue = ""

			' SiteCategoryGroupOrder
			SiteCategoryGroup.SiteCategoryGroupOrder.HrefValue = ""

		'
		'  Search Row
		'

		ElseIf SiteCategoryGroup.RowType = EW_ROWTYPE_SEARCH Then ' Search row

			' SiteCategoryGroupID
			SiteCategoryGroup.SiteCategoryGroupID.EditCustomAttributes = ""
			SiteCategoryGroup.SiteCategoryGroupID.EditValue = ew_HtmlEncode(SiteCategoryGroup.SiteCategoryGroupID.AdvancedSearch.SearchValue)

			' SiteCategoryGroupNM
			SiteCategoryGroup.SiteCategoryGroupNM.EditCustomAttributes = ""
			SiteCategoryGroup.SiteCategoryGroupNM.EditValue = ew_HtmlEncode(SiteCategoryGroup.SiteCategoryGroupNM.AdvancedSearch.SearchValue)

			' SiteCategoryGroupDS
			SiteCategoryGroup.SiteCategoryGroupDS.EditCustomAttributes = ""
			SiteCategoryGroup.SiteCategoryGroupDS.EditValue = ew_HtmlEncode(SiteCategoryGroup.SiteCategoryGroupDS.AdvancedSearch.SearchValue)

			' SiteCategoryGroupOrder
			SiteCategoryGroup.SiteCategoryGroupOrder.EditCustomAttributes = ""
			SiteCategoryGroup.SiteCategoryGroupOrder.EditValue = ew_HtmlEncode(SiteCategoryGroup.SiteCategoryGroupOrder.AdvancedSearch.SearchValue)
		End If

		' Row Rendered event
		SiteCategoryGroup.Row_Rendered()
	End Sub

	'
	' Validate search
	'
	Function ValidateSearch() As Boolean

		' Initialize
		ParentPage.gsSearchError = ""

		' Check if validation required
		If Not EW_SERVER_VALIDATE Then Return True ' Skip
		If Not ew_CheckInteger(SiteCategoryGroup.SiteCategoryGroupID.AdvancedSearch.SearchValue) Then
			If ParentPage.gsSearchError <> "" Then ParentPage.gsSearchError = ParentPage.gsSearchError & "<br />"
			ParentPage.gsSearchError = ParentPage.gsSearchError & "Incorrect integer - Site Category Group ID"
		End If
		If Not ew_CheckInteger(SiteCategoryGroup.SiteCategoryGroupOrder.AdvancedSearch.SearchValue) Then
			If ParentPage.gsSearchError <> "" Then ParentPage.gsSearchError = ParentPage.gsSearchError & "<br />"
			ParentPage.gsSearchError = ParentPage.gsSearchError & "Incorrect integer - Site Category Group Order"
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
		SiteCategoryGroup.SiteCategoryGroupID.AdvancedSearch.SearchValue = SiteCategoryGroup.GetAdvancedSearch("x_SiteCategoryGroupID")
		SiteCategoryGroup.SiteCategoryGroupNM.AdvancedSearch.SearchValue = SiteCategoryGroup.GetAdvancedSearch("x_SiteCategoryGroupNM")
		SiteCategoryGroup.SiteCategoryGroupDS.AdvancedSearch.SearchValue = SiteCategoryGroup.GetAdvancedSearch("x_SiteCategoryGroupDS")
		SiteCategoryGroup.SiteCategoryGroupOrder.AdvancedSearch.SearchValue = SiteCategoryGroup.GetAdvancedSearch("x_SiteCategoryGroupOrder")
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
		SiteCategoryGroup_search = New cSiteCategoryGroup_search(Me)		
		SiteCategoryGroup_search.Page_Init()

		' Page main processing
		SiteCategoryGroup_search.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If SiteCategoryGroup_search IsNot Nothing Then SiteCategoryGroup_search.Dispose()
	End Sub
End Class
