Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class CompanySiteTypeParameter_srch
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public CompanySiteTypeParameter_search As cCompanySiteTypeParameter_search

	'
	' Page Class
	'
	Class cCompanySiteTypeParameter_search
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
				If CompanySiteTypeParameter.UseTokenInUrl Then Url = Url & "t=" & CompanySiteTypeParameter.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If CompanySiteTypeParameter.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (CompanySiteTypeParameter.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (CompanySiteTypeParameter.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' CompanySiteTypeParameter
		Public Property CompanySiteTypeParameter() As cCompanySiteTypeParameter
			Get				
				Return ParentPage.CompanySiteTypeParameter
			End Get
			Set(ByVal v As cCompanySiteTypeParameter)
				ParentPage.CompanySiteTypeParameter = v	
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
			m_PageObjName = "CompanySiteTypeParameter_search"
			m_PageObjTypeName = "cCompanySiteTypeParameter_search"

			' Table Name
			m_TableName = "CompanySiteTypeParameter"

			' Initialize table object
			CompanySiteTypeParameter = New cCompanySiteTypeParameter(Me)

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
			CompanySiteTypeParameter.Dispose()

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
			CompanySiteTypeParameter.CurrentAction = ObjForm.GetValue("a_search")
			Select Case CompanySiteTypeParameter.CurrentAction
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
						sSrchStr = CompanySiteTypeParameter.UrlParm(sSrchStr)
					Page_Terminate("CompanySiteTypeParameter_list.aspx" & "?" & sSrchStr) ' Go to list page
					End If
			End Select
		End If

		' Restore search settings from Session
		If ParentPage.gsSearchError = "" Then
			LoadAdvancedSearch()
		End If

		' Render row for search
		CompanySiteTypeParameter.RowType = EW_ROWTYPE_SEARCH
		RenderRow()
	End Sub

	'
	' Build advanced search
	'
	Function BuildAdvancedSearch() As String
		Dim sSrchUrl As String = ""
		BuildSearchUrl(sSrchUrl, CompanySiteTypeParameter.SiteParameterTypeID) ' SiteParameterTypeID
		BuildSearchUrl(sSrchUrl, CompanySiteTypeParameter.CompanyID) ' CompanyID
		BuildSearchUrl(sSrchUrl, CompanySiteTypeParameter.SiteCategoryTypeID) ' SiteCategoryTypeID
		BuildSearchUrl(sSrchUrl, CompanySiteTypeParameter.SiteCategoryGroupID) ' SiteCategoryGroupID
		BuildSearchUrl(sSrchUrl, CompanySiteTypeParameter.SiteCategoryID) ' SiteCategoryID
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
		CompanySiteTypeParameter.SiteParameterTypeID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_SiteParameterTypeID")
    	CompanySiteTypeParameter.SiteParameterTypeID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_SiteParameterTypeID")
		CompanySiteTypeParameter.CompanyID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_CompanyID")
    	CompanySiteTypeParameter.CompanyID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_CompanyID")
		CompanySiteTypeParameter.SiteCategoryTypeID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_SiteCategoryTypeID")
    	CompanySiteTypeParameter.SiteCategoryTypeID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_SiteCategoryTypeID")
		CompanySiteTypeParameter.SiteCategoryGroupID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_SiteCategoryGroupID")
    	CompanySiteTypeParameter.SiteCategoryGroupID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_SiteCategoryGroupID")
		CompanySiteTypeParameter.SiteCategoryID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_SiteCategoryID")
    	CompanySiteTypeParameter.SiteCategoryID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_SiteCategoryID")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		CompanySiteTypeParameter.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' SiteParameterTypeID

		CompanySiteTypeParameter.SiteParameterTypeID.CellCssStyle = ""
		CompanySiteTypeParameter.SiteParameterTypeID.CellCssClass = ""

		' CompanyID
		CompanySiteTypeParameter.CompanyID.CellCssStyle = ""
		CompanySiteTypeParameter.CompanyID.CellCssClass = ""

		' SiteCategoryTypeID
		CompanySiteTypeParameter.SiteCategoryTypeID.CellCssStyle = ""
		CompanySiteTypeParameter.SiteCategoryTypeID.CellCssClass = ""

		' SiteCategoryGroupID
		CompanySiteTypeParameter.SiteCategoryGroupID.CellCssStyle = ""
		CompanySiteTypeParameter.SiteCategoryGroupID.CellCssClass = ""

		' SiteCategoryID
		CompanySiteTypeParameter.SiteCategoryID.CellCssStyle = ""
		CompanySiteTypeParameter.SiteCategoryID.CellCssClass = ""

		'
		'  View  Row
		'

		If CompanySiteTypeParameter.RowType = EW_ROWTYPE_VIEW Then ' View row

			' SiteParameterTypeID
			If ew_NotEmpty(CompanySiteTypeParameter.SiteParameterTypeID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteParameterTypeNM] FROM [SiteParameterType] WHERE [SiteParameterTypeID] = " & ew_AdjustSql(CompanySiteTypeParameter.SiteParameterTypeID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteParameterTypeNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanySiteTypeParameter.SiteParameterTypeID.ViewValue = RsWrk("SiteParameterTypeNM")
				Else
					CompanySiteTypeParameter.SiteParameterTypeID.ViewValue = CompanySiteTypeParameter.SiteParameterTypeID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanySiteTypeParameter.SiteParameterTypeID.ViewValue = System.DBNull.Value
			End If
			CompanySiteTypeParameter.SiteParameterTypeID.CssStyle = ""
			CompanySiteTypeParameter.SiteParameterTypeID.CssClass = ""
			CompanySiteTypeParameter.SiteParameterTypeID.ViewCustomAttributes = ""

			' CompanyID
			If ew_NotEmpty(CompanySiteTypeParameter.CompanyID.CurrentValue) Then
				sSqlWrk = "SELECT [CompanyName] FROM [Company] WHERE [CompanyID] = " & ew_AdjustSql(CompanySiteTypeParameter.CompanyID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanySiteTypeParameter.CompanyID.ViewValue = RsWrk("CompanyName")
				Else
					CompanySiteTypeParameter.CompanyID.ViewValue = CompanySiteTypeParameter.CompanyID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanySiteTypeParameter.CompanyID.ViewValue = System.DBNull.Value
			End If
			CompanySiteTypeParameter.CompanyID.CssStyle = ""
			CompanySiteTypeParameter.CompanyID.CssClass = ""
			CompanySiteTypeParameter.CompanyID.ViewCustomAttributes = ""

			' SiteCategoryTypeID
			If ew_NotEmpty(CompanySiteTypeParameter.SiteCategoryTypeID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteCategoryTypeNM] FROM [SiteCategoryType] WHERE [SiteCategoryTypeID] = " & ew_AdjustSql(CompanySiteTypeParameter.SiteCategoryTypeID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryTypeNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanySiteTypeParameter.SiteCategoryTypeID.ViewValue = RsWrk("SiteCategoryTypeNM")
				Else
					CompanySiteTypeParameter.SiteCategoryTypeID.ViewValue = CompanySiteTypeParameter.SiteCategoryTypeID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanySiteTypeParameter.SiteCategoryTypeID.ViewValue = System.DBNull.Value
			End If
			CompanySiteTypeParameter.SiteCategoryTypeID.CssStyle = ""
			CompanySiteTypeParameter.SiteCategoryTypeID.CssClass = ""
			CompanySiteTypeParameter.SiteCategoryTypeID.ViewCustomAttributes = ""

			' SiteCategoryGroupID
			If ew_NotEmpty(CompanySiteTypeParameter.SiteCategoryGroupID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteCategoryGroupNM] FROM [SiteCategoryGroup] WHERE [SiteCategoryGroupID] = " & ew_AdjustSql(CompanySiteTypeParameter.SiteCategoryGroupID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryGroupNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanySiteTypeParameter.SiteCategoryGroupID.ViewValue = RsWrk("SiteCategoryGroupNM")
				Else
					CompanySiteTypeParameter.SiteCategoryGroupID.ViewValue = CompanySiteTypeParameter.SiteCategoryGroupID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanySiteTypeParameter.SiteCategoryGroupID.ViewValue = System.DBNull.Value
			End If
			CompanySiteTypeParameter.SiteCategoryGroupID.CssStyle = ""
			CompanySiteTypeParameter.SiteCategoryGroupID.CssClass = ""
			CompanySiteTypeParameter.SiteCategoryGroupID.ViewCustomAttributes = ""

			' SiteCategoryID
			If ew_NotEmpty(CompanySiteTypeParameter.SiteCategoryID.CurrentValue) Then
				sSqlWrk = "SELECT [CategoryName] FROM [SiteCategory] WHERE [SiteCategoryID] = " & ew_AdjustSql(CompanySiteTypeParameter.SiteCategoryID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [CategoryName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanySiteTypeParameter.SiteCategoryID.ViewValue = RsWrk("CategoryName")
				Else
					CompanySiteTypeParameter.SiteCategoryID.ViewValue = CompanySiteTypeParameter.SiteCategoryID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanySiteTypeParameter.SiteCategoryID.ViewValue = System.DBNull.Value
			End If
			CompanySiteTypeParameter.SiteCategoryID.CssStyle = ""
			CompanySiteTypeParameter.SiteCategoryID.CssClass = ""
			CompanySiteTypeParameter.SiteCategoryID.ViewCustomAttributes = ""

			' SortOrder
			CompanySiteTypeParameter.SortOrder.ViewValue = CompanySiteTypeParameter.SortOrder.CurrentValue
			CompanySiteTypeParameter.SortOrder.CssStyle = ""
			CompanySiteTypeParameter.SortOrder.CssClass = ""
			CompanySiteTypeParameter.SortOrder.ViewCustomAttributes = ""

			' View refer script
			' SiteParameterTypeID

			CompanySiteTypeParameter.SiteParameterTypeID.HrefValue = ""

			' CompanyID
			CompanySiteTypeParameter.CompanyID.HrefValue = ""

			' SiteCategoryTypeID
			CompanySiteTypeParameter.SiteCategoryTypeID.HrefValue = ""

			' SiteCategoryGroupID
			CompanySiteTypeParameter.SiteCategoryGroupID.HrefValue = ""

			' SiteCategoryID
			CompanySiteTypeParameter.SiteCategoryID.HrefValue = ""

		'
		'  Search Row
		'

		ElseIf CompanySiteTypeParameter.RowType = EW_ROWTYPE_SEARCH Then ' Search row

			' SiteParameterTypeID
			CompanySiteTypeParameter.SiteParameterTypeID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteParameterTypeID], [SiteParameterTypeNM], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteParameterType]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [SiteParameterTypeNM] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			CompanySiteTypeParameter.SiteParameterTypeID.EditValue = arwrk

			' CompanyID
			CompanySiteTypeParameter.CompanyID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [CompanyID], [CompanyName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Company]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			CompanySiteTypeParameter.CompanyID.EditValue = arwrk

			' SiteCategoryTypeID
			CompanySiteTypeParameter.SiteCategoryTypeID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryTypeID], [SiteCategoryTypeNM], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteCategoryType]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryTypeNM] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			CompanySiteTypeParameter.SiteCategoryTypeID.EditValue = arwrk

			' SiteCategoryGroupID
			CompanySiteTypeParameter.SiteCategoryGroupID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryGroupID], [SiteCategoryGroupNM], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteCategoryGroup]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryGroupNM] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			CompanySiteTypeParameter.SiteCategoryGroupID.EditValue = arwrk

			' SiteCategoryID
			CompanySiteTypeParameter.SiteCategoryID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryID], [CategoryName], '' AS Disp2Fld, [SiteCategoryTypeID] FROM [SiteCategory]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [CategoryName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select", ""}) 
			CompanySiteTypeParameter.SiteCategoryID.EditValue = arwrk
		End If

		' Row Rendered event
		CompanySiteTypeParameter.Row_Rendered()
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
		CompanySiteTypeParameter.SiteParameterTypeID.AdvancedSearch.SearchValue = CompanySiteTypeParameter.GetAdvancedSearch("x_SiteParameterTypeID")
		CompanySiteTypeParameter.CompanyID.AdvancedSearch.SearchValue = CompanySiteTypeParameter.GetAdvancedSearch("x_CompanyID")
		CompanySiteTypeParameter.SiteCategoryTypeID.AdvancedSearch.SearchValue = CompanySiteTypeParameter.GetAdvancedSearch("x_SiteCategoryTypeID")
		CompanySiteTypeParameter.SiteCategoryGroupID.AdvancedSearch.SearchValue = CompanySiteTypeParameter.GetAdvancedSearch("x_SiteCategoryGroupID")
		CompanySiteTypeParameter.SiteCategoryID.AdvancedSearch.SearchValue = CompanySiteTypeParameter.GetAdvancedSearch("x_SiteCategoryID")
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
		CompanySiteTypeParameter_search = New cCompanySiteTypeParameter_search(Me)		
		CompanySiteTypeParameter_search.Page_Init()

		' Page main processing
		CompanySiteTypeParameter_search.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If CompanySiteTypeParameter_search IsNot Nothing Then CompanySiteTypeParameter_search.Dispose()
	End Sub
End Class
