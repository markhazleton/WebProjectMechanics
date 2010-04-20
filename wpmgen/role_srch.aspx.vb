Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class role_srch
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public role_search As crole_search

	'
	' Page Class
	'
	Class crole_search
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
				If role.UseTokenInUrl Then Url = Url & "t=" & role.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If role.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (role.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (role.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' role
		Public Property role() As crole
			Get				
				Return ParentPage.role
			End Get
			Set(ByVal v As crole)
				ParentPage.role = v	
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
			m_PageObjName = "role_search"
			m_PageObjTypeName = "crole_search"

			' Table Name
			m_TableName = "role"

			' Initialize table object
			role = New crole(Me)

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
			role.Dispose()

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
			role.CurrentAction = ObjForm.GetValue("a_search")
			Select Case role.CurrentAction
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
						sSrchStr = role.UrlParm(sSrchStr)
					Page_Terminate("role_list.aspx" & "?" & sSrchStr) ' Go to list page
					End If
			End Select
		End If

		' Restore search settings from Session
		If ParentPage.gsSearchError = "" Then
			LoadAdvancedSearch()
		End If

		' Render row for search
		role.RowType = EW_ROWTYPE_SEARCH
		RenderRow()
	End Sub

	'
	' Build advanced search
	'
	Function BuildAdvancedSearch() As String
		Dim sSrchUrl As String = ""
		BuildSearchUrl(sSrchUrl, role.RoleID) ' RoleID
		BuildSearchUrl(sSrchUrl, role.RoleName) ' RoleName
		BuildSearchUrl(sSrchUrl, role.RoleTitle) ' RoleTitle
		BuildSearchUrl(sSrchUrl, role.RoleComment) ' RoleComment
		BuildSearchUrl(sSrchUrl, role.FilterMenu) ' FilterMenu
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
		role.RoleID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_RoleID")
    	role.RoleID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_RoleID")
		role.RoleName.AdvancedSearch.SearchValue = ObjForm.GetValue("x_RoleName")
    	role.RoleName.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_RoleName")
		role.RoleTitle.AdvancedSearch.SearchValue = ObjForm.GetValue("x_RoleTitle")
    	role.RoleTitle.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_RoleTitle")
		role.RoleComment.AdvancedSearch.SearchValue = ObjForm.GetValue("x_RoleComment")
    	role.RoleComment.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_RoleComment")
		role.FilterMenu.AdvancedSearch.SearchValue = ObjForm.GetValue("x_FilterMenu")
    	role.FilterMenu.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_FilterMenu")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		role.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' RoleID

		role.RoleID.CellCssStyle = ""
		role.RoleID.CellCssClass = ""

		' RoleName
		role.RoleName.CellCssStyle = ""
		role.RoleName.CellCssClass = ""

		' RoleTitle
		role.RoleTitle.CellCssStyle = ""
		role.RoleTitle.CellCssClass = ""

		' RoleComment
		role.RoleComment.CellCssStyle = ""
		role.RoleComment.CellCssClass = ""

		' FilterMenu
		role.FilterMenu.CellCssStyle = ""
		role.FilterMenu.CellCssClass = ""

		'
		'  View  Row
		'

		If role.RowType = EW_ROWTYPE_VIEW Then ' View row

			' RoleID
			role.RoleID.ViewValue = role.RoleID.CurrentValue
			role.RoleID.CssStyle = ""
			role.RoleID.CssClass = ""
			role.RoleID.ViewCustomAttributes = ""

			' RoleName
			role.RoleName.ViewValue = role.RoleName.CurrentValue
			role.RoleName.CssStyle = ""
			role.RoleName.CssClass = ""
			role.RoleName.ViewCustomAttributes = ""

			' RoleTitle
			role.RoleTitle.ViewValue = role.RoleTitle.CurrentValue
			role.RoleTitle.CssStyle = ""
			role.RoleTitle.CssClass = ""
			role.RoleTitle.ViewCustomAttributes = ""

			' RoleComment
			role.RoleComment.ViewValue = role.RoleComment.CurrentValue
			role.RoleComment.CssStyle = ""
			role.RoleComment.CssClass = ""
			role.RoleComment.ViewCustomAttributes = ""

			' FilterMenu
			If Convert.ToString(role.FilterMenu.CurrentValue) = "1" Then
				role.FilterMenu.ViewValue = "Yes"
			Else
				role.FilterMenu.ViewValue = "No"
			End If
			role.FilterMenu.CssStyle = ""
			role.FilterMenu.CssClass = ""
			role.FilterMenu.ViewCustomAttributes = ""

			' View refer script
			' RoleID

			role.RoleID.HrefValue = ""

			' RoleName
			role.RoleName.HrefValue = ""

			' RoleTitle
			role.RoleTitle.HrefValue = ""

			' RoleComment
			role.RoleComment.HrefValue = ""

			' FilterMenu
			role.FilterMenu.HrefValue = ""

		'
		'  Search Row
		'

		ElseIf role.RowType = EW_ROWTYPE_SEARCH Then ' Search row

			' RoleID
			role.RoleID.EditCustomAttributes = ""
			role.RoleID.EditValue = ew_HtmlEncode(role.RoleID.AdvancedSearch.SearchValue)

			' RoleName
			role.RoleName.EditCustomAttributes = ""
			role.RoleName.EditValue = ew_HtmlEncode(role.RoleName.AdvancedSearch.SearchValue)

			' RoleTitle
			role.RoleTitle.EditCustomAttributes = ""
			role.RoleTitle.EditValue = ew_HtmlEncode(role.RoleTitle.AdvancedSearch.SearchValue)

			' RoleComment
			role.RoleComment.EditCustomAttributes = ""
			role.RoleComment.EditValue = ew_HtmlEncode(role.RoleComment.AdvancedSearch.SearchValue)

			' FilterMenu
			role.FilterMenu.EditCustomAttributes = ""
		End If

		' Row Rendered event
		role.Row_Rendered()
	End Sub

	'
	' Validate search
	'
	Function ValidateSearch() As Boolean

		' Initialize
		ParentPage.gsSearchError = ""

		' Check if validation required
		If Not EW_SERVER_VALIDATE Then Return True ' Skip
		If Not ew_CheckInteger(role.RoleID.AdvancedSearch.SearchValue) Then
			If ParentPage.gsSearchError <> "" Then ParentPage.gsSearchError = ParentPage.gsSearchError & "<br />"
			ParentPage.gsSearchError = ParentPage.gsSearchError & "Incorrect integer - Role ID"
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
		role.RoleID.AdvancedSearch.SearchValue = role.GetAdvancedSearch("x_RoleID")
		role.RoleName.AdvancedSearch.SearchValue = role.GetAdvancedSearch("x_RoleName")
		role.RoleTitle.AdvancedSearch.SearchValue = role.GetAdvancedSearch("x_RoleTitle")
		role.RoleComment.AdvancedSearch.SearchValue = role.GetAdvancedSearch("x_RoleComment")
		role.FilterMenu.AdvancedSearch.SearchValue = role.GetAdvancedSearch("x_FilterMenu")
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
		role_search = New crole_search(Me)		
		role_search.Page_Init()

		' Page main processing
		role_search.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If role_search IsNot Nothing Then role_search.Dispose()
	End Sub
End Class
