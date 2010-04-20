Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class LinkType_srch
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public LinkType_search As cLinkType_search

	'
	' Page Class
	'
	Class cLinkType_search
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
				If LinkType.UseTokenInUrl Then Url = Url & "t=" & LinkType.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If LinkType.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (LinkType.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (LinkType.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' LinkType
		Public Property LinkType() As cLinkType
			Get				
				Return ParentPage.LinkType
			End Get
			Set(ByVal v As cLinkType)
				ParentPage.LinkType = v	
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
			m_PageObjName = "LinkType_search"
			m_PageObjTypeName = "cLinkType_search"

			' Table Name
			m_TableName = "LinkType"

			' Initialize table object
			LinkType = New cLinkType(Me)

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
			LinkType.Dispose()

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
			LinkType.CurrentAction = ObjForm.GetValue("a_search")
			Select Case LinkType.CurrentAction
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
						sSrchStr = LinkType.UrlParm(sSrchStr)
					Page_Terminate("LinkType_list.aspx" & "?" & sSrchStr) ' Go to list page
					End If
			End Select
		End If

		' Restore search settings from Session
		If ParentPage.gsSearchError = "" Then
			LoadAdvancedSearch()
		End If

		' Render row for search
		LinkType.RowType = EW_ROWTYPE_SEARCH
		RenderRow()
	End Sub

	'
	' Build advanced search
	'
	Function BuildAdvancedSearch() As String
		Dim sSrchUrl As String = ""
		BuildSearchUrl(sSrchUrl, LinkType.LinkTypeCD) ' LinkTypeCD
		BuildSearchUrl(sSrchUrl, LinkType.LinkTypeDesc) ' LinkTypeDesc
		BuildSearchUrl(sSrchUrl, LinkType.LinkTypeComment) ' LinkTypeComment
		BuildSearchUrl(sSrchUrl, LinkType.LinkTypeTarget) ' LinkTypeTarget
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
		LinkType.LinkTypeCD.AdvancedSearch.SearchValue = ObjForm.GetValue("x_LinkTypeCD")
    	LinkType.LinkTypeCD.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_LinkTypeCD")
		LinkType.LinkTypeDesc.AdvancedSearch.SearchValue = ObjForm.GetValue("x_LinkTypeDesc")
    	LinkType.LinkTypeDesc.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_LinkTypeDesc")
		LinkType.LinkTypeComment.AdvancedSearch.SearchValue = ObjForm.GetValue("x_LinkTypeComment")
    	LinkType.LinkTypeComment.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_LinkTypeComment")
		LinkType.LinkTypeTarget.AdvancedSearch.SearchValue = ObjForm.GetValue("x_LinkTypeTarget")
    	LinkType.LinkTypeTarget.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_LinkTypeTarget")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		LinkType.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' LinkTypeCD

		LinkType.LinkTypeCD.CellCssStyle = ""
		LinkType.LinkTypeCD.CellCssClass = ""

		' LinkTypeDesc
		LinkType.LinkTypeDesc.CellCssStyle = ""
		LinkType.LinkTypeDesc.CellCssClass = ""

		' LinkTypeComment
		LinkType.LinkTypeComment.CellCssStyle = ""
		LinkType.LinkTypeComment.CellCssClass = ""

		' LinkTypeTarget
		LinkType.LinkTypeTarget.CellCssStyle = ""
		LinkType.LinkTypeTarget.CellCssClass = ""

		'
		'  View  Row
		'

		If LinkType.RowType = EW_ROWTYPE_VIEW Then ' View row

			' LinkTypeCD
			LinkType.LinkTypeCD.ViewValue = LinkType.LinkTypeCD.CurrentValue
			LinkType.LinkTypeCD.CssStyle = ""
			LinkType.LinkTypeCD.CssClass = ""
			LinkType.LinkTypeCD.ViewCustomAttributes = ""

			' LinkTypeDesc
			LinkType.LinkTypeDesc.ViewValue = LinkType.LinkTypeDesc.CurrentValue
			LinkType.LinkTypeDesc.CssStyle = ""
			LinkType.LinkTypeDesc.CssClass = ""
			LinkType.LinkTypeDesc.ViewCustomAttributes = ""

			' LinkTypeComment
			LinkType.LinkTypeComment.ViewValue = LinkType.LinkTypeComment.CurrentValue
			LinkType.LinkTypeComment.CssStyle = ""
			LinkType.LinkTypeComment.CssClass = ""
			LinkType.LinkTypeComment.ViewCustomAttributes = ""

			' LinkTypeTarget
			LinkType.LinkTypeTarget.ViewValue = LinkType.LinkTypeTarget.CurrentValue
			LinkType.LinkTypeTarget.CssStyle = ""
			LinkType.LinkTypeTarget.CssClass = ""
			LinkType.LinkTypeTarget.ViewCustomAttributes = ""

			' View refer script
			' LinkTypeCD

			LinkType.LinkTypeCD.HrefValue = ""

			' LinkTypeDesc
			LinkType.LinkTypeDesc.HrefValue = ""

			' LinkTypeComment
			LinkType.LinkTypeComment.HrefValue = ""

			' LinkTypeTarget
			LinkType.LinkTypeTarget.HrefValue = ""

		'
		'  Search Row
		'

		ElseIf LinkType.RowType = EW_ROWTYPE_SEARCH Then ' Search row

			' LinkTypeCD
			LinkType.LinkTypeCD.EditCustomAttributes = ""
			LinkType.LinkTypeCD.EditValue = ew_HtmlEncode(LinkType.LinkTypeCD.AdvancedSearch.SearchValue)

			' LinkTypeDesc
			LinkType.LinkTypeDesc.EditCustomAttributes = ""
			LinkType.LinkTypeDesc.EditValue = ew_HtmlEncode(LinkType.LinkTypeDesc.AdvancedSearch.SearchValue)

			' LinkTypeComment
			LinkType.LinkTypeComment.EditCustomAttributes = ""
			LinkType.LinkTypeComment.EditValue = ew_HtmlEncode(LinkType.LinkTypeComment.AdvancedSearch.SearchValue)

			' LinkTypeTarget
			LinkType.LinkTypeTarget.EditCustomAttributes = ""
			LinkType.LinkTypeTarget.EditValue = ew_HtmlEncode(LinkType.LinkTypeTarget.AdvancedSearch.SearchValue)
		End If

		' Row Rendered event
		LinkType.Row_Rendered()
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
		LinkType.LinkTypeCD.AdvancedSearch.SearchValue = LinkType.GetAdvancedSearch("x_LinkTypeCD")
		LinkType.LinkTypeDesc.AdvancedSearch.SearchValue = LinkType.GetAdvancedSearch("x_LinkTypeDesc")
		LinkType.LinkTypeComment.AdvancedSearch.SearchValue = LinkType.GetAdvancedSearch("x_LinkTypeComment")
		LinkType.LinkTypeTarget.AdvancedSearch.SearchValue = LinkType.GetAdvancedSearch("x_LinkTypeTarget")
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
		LinkType_search = New cLinkType_search(Me)		
		LinkType_search.Page_Init()

		' Page main processing
		LinkType_search.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If LinkType_search IsNot Nothing Then LinkType_search.Dispose()
	End Sub
End Class
