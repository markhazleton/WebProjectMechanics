Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class CompanySiteParameter_srch
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public CompanySiteParameter_search As cCompanySiteParameter_search

	'
	' Page Class
	'
	Class cCompanySiteParameter_search
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
				If CompanySiteParameter.UseTokenInUrl Then Url = Url & "t=" & CompanySiteParameter.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If CompanySiteParameter.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (CompanySiteParameter.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (CompanySiteParameter.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' CompanySiteParameter
		Public Property CompanySiteParameter() As cCompanySiteParameter
			Get				
				Return ParentPage.CompanySiteParameter
			End Get
			Set(ByVal v As cCompanySiteParameter)
				ParentPage.CompanySiteParameter = v	
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
			m_PageObjName = "CompanySiteParameter_search"
			m_PageObjTypeName = "cCompanySiteParameter_search"

			' Table Name
			m_TableName = "CompanySiteParameter"

			' Initialize table object
			CompanySiteParameter = New cCompanySiteParameter(Me)

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
			CompanySiteParameter.Dispose()

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
			CompanySiteParameter.CurrentAction = ObjForm.GetValue("a_search")
			Select Case CompanySiteParameter.CurrentAction
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
						sSrchStr = CompanySiteParameter.UrlParm(sSrchStr)
					Page_Terminate("CompanySiteParameter_list.aspx" & "?" & sSrchStr) ' Go to list page
					End If
			End Select
		End If

		' Restore search settings from Session
		If ParentPage.gsSearchError = "" Then
			LoadAdvancedSearch()
		End If

		' Render row for search
		CompanySiteParameter.RowType = EW_ROWTYPE_SEARCH
		RenderRow()
	End Sub

	'
	' Build advanced search
	'
	Function BuildAdvancedSearch() As String
		Dim sSrchUrl As String = ""
		BuildSearchUrl(sSrchUrl, CompanySiteParameter.CompanyID) ' CompanyID
		BuildSearchUrl(sSrchUrl, CompanySiteParameter.zPageID) ' PageID
		BuildSearchUrl(sSrchUrl, CompanySiteParameter.SiteCategoryGroupID) ' SiteCategoryGroupID
		BuildSearchUrl(sSrchUrl, CompanySiteParameter.SiteParameterTypeID) ' SiteParameterTypeID
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
		CompanySiteParameter.CompanyID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_CompanyID")
    	CompanySiteParameter.CompanyID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_CompanyID")
		CompanySiteParameter.zPageID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_zPageID")
    	CompanySiteParameter.zPageID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_zPageID")
		CompanySiteParameter.SiteCategoryGroupID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_SiteCategoryGroupID")
    	CompanySiteParameter.SiteCategoryGroupID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_SiteCategoryGroupID")
		CompanySiteParameter.SiteParameterTypeID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_SiteParameterTypeID")
    	CompanySiteParameter.SiteParameterTypeID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_SiteParameterTypeID")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		CompanySiteParameter.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' CompanyID

		CompanySiteParameter.CompanyID.CellCssStyle = ""
		CompanySiteParameter.CompanyID.CellCssClass = ""

		' PageID
		CompanySiteParameter.zPageID.CellCssStyle = ""
		CompanySiteParameter.zPageID.CellCssClass = ""

		' SiteCategoryGroupID
		CompanySiteParameter.SiteCategoryGroupID.CellCssStyle = ""
		CompanySiteParameter.SiteCategoryGroupID.CellCssClass = ""

		' SiteParameterTypeID
		CompanySiteParameter.SiteParameterTypeID.CellCssStyle = ""
		CompanySiteParameter.SiteParameterTypeID.CellCssClass = ""

		'
		'  View  Row
		'

		If CompanySiteParameter.RowType = EW_ROWTYPE_VIEW Then ' View row

			' CompanyID
			If ew_NotEmpty(CompanySiteParameter.CompanyID.CurrentValue) Then
				sSqlWrk = "SELECT [CompanyName] FROM [Company] WHERE [CompanyID] = " & ew_AdjustSql(CompanySiteParameter.CompanyID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanySiteParameter.CompanyID.ViewValue = RsWrk("CompanyName")
				Else
					CompanySiteParameter.CompanyID.ViewValue = CompanySiteParameter.CompanyID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanySiteParameter.CompanyID.ViewValue = System.DBNull.Value
			End If
			CompanySiteParameter.CompanyID.CssStyle = ""
			CompanySiteParameter.CompanyID.CssClass = ""
			CompanySiteParameter.CompanyID.ViewCustomAttributes = ""

			' PageID
			If ew_NotEmpty(CompanySiteParameter.zPageID.CurrentValue) Then
				sSqlWrk = "SELECT [PageName] FROM [Page] WHERE [PageID] = " & ew_AdjustSql(CompanySiteParameter.zPageID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [PageName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanySiteParameter.zPageID.ViewValue = RsWrk("PageName")
				Else
					CompanySiteParameter.zPageID.ViewValue = CompanySiteParameter.zPageID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanySiteParameter.zPageID.ViewValue = System.DBNull.Value
			End If
			CompanySiteParameter.zPageID.CssStyle = ""
			CompanySiteParameter.zPageID.CssClass = ""
			CompanySiteParameter.zPageID.ViewCustomAttributes = ""

			' SiteCategoryGroupID
			If ew_NotEmpty(CompanySiteParameter.SiteCategoryGroupID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteCategoryGroupNM] FROM [SiteCategoryGroup] WHERE [SiteCategoryGroupID] = " & ew_AdjustSql(CompanySiteParameter.SiteCategoryGroupID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryGroupNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanySiteParameter.SiteCategoryGroupID.ViewValue = RsWrk("SiteCategoryGroupNM")
				Else
					CompanySiteParameter.SiteCategoryGroupID.ViewValue = CompanySiteParameter.SiteCategoryGroupID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanySiteParameter.SiteCategoryGroupID.ViewValue = System.DBNull.Value
			End If
			CompanySiteParameter.SiteCategoryGroupID.CssStyle = ""
			CompanySiteParameter.SiteCategoryGroupID.CssClass = ""
			CompanySiteParameter.SiteCategoryGroupID.ViewCustomAttributes = ""

			' SiteParameterTypeID
			If ew_NotEmpty(CompanySiteParameter.SiteParameterTypeID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteParameterTypeNM] FROM [SiteParameterType] WHERE [SiteParameterTypeID] = " & ew_AdjustSql(CompanySiteParameter.SiteParameterTypeID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteParameterTypeNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanySiteParameter.SiteParameterTypeID.ViewValue = RsWrk("SiteParameterTypeNM")
				Else
					CompanySiteParameter.SiteParameterTypeID.ViewValue = CompanySiteParameter.SiteParameterTypeID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanySiteParameter.SiteParameterTypeID.ViewValue = System.DBNull.Value
			End If
			CompanySiteParameter.SiteParameterTypeID.CssStyle = ""
			CompanySiteParameter.SiteParameterTypeID.CssClass = ""
			CompanySiteParameter.SiteParameterTypeID.ViewCustomAttributes = ""

			' SortOrder
			CompanySiteParameter.SortOrder.ViewValue = CompanySiteParameter.SortOrder.CurrentValue
			CompanySiteParameter.SortOrder.CssStyle = ""
			CompanySiteParameter.SortOrder.CssClass = ""
			CompanySiteParameter.SortOrder.ViewCustomAttributes = ""

			' View refer script
			' CompanyID

			CompanySiteParameter.CompanyID.HrefValue = ""

			' PageID
			CompanySiteParameter.zPageID.HrefValue = ""

			' SiteCategoryGroupID
			CompanySiteParameter.SiteCategoryGroupID.HrefValue = ""

			' SiteParameterTypeID
			CompanySiteParameter.SiteParameterTypeID.HrefValue = ""

		'
		'  Search Row
		'

		ElseIf CompanySiteParameter.RowType = EW_ROWTYPE_SEARCH Then ' Search row

			' CompanyID
			CompanySiteParameter.CompanyID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [CompanyID], [CompanyName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Company]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			CompanySiteParameter.CompanyID.EditValue = arwrk

			' PageID
			CompanySiteParameter.zPageID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [PageID], [PageName], '' AS Disp2Fld, [CompanyID] FROM [Page]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [PageName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select", ""}) 
			CompanySiteParameter.zPageID.EditValue = arwrk

			' SiteCategoryGroupID
			CompanySiteParameter.SiteCategoryGroupID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryGroupID], [SiteCategoryGroupNM], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteCategoryGroup]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryGroupNM] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			CompanySiteParameter.SiteCategoryGroupID.EditValue = arwrk

			' SiteParameterTypeID
			CompanySiteParameter.SiteParameterTypeID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteParameterTypeID], [SiteParameterTypeNM], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteParameterType]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [SiteParameterTypeNM] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			CompanySiteParameter.SiteParameterTypeID.EditValue = arwrk
		End If

		' Row Rendered event
		CompanySiteParameter.Row_Rendered()
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
		CompanySiteParameter.CompanyID.AdvancedSearch.SearchValue = CompanySiteParameter.GetAdvancedSearch("x_CompanyID")
		CompanySiteParameter.zPageID.AdvancedSearch.SearchValue = CompanySiteParameter.GetAdvancedSearch("x_zPageID")
		CompanySiteParameter.SiteCategoryGroupID.AdvancedSearch.SearchValue = CompanySiteParameter.GetAdvancedSearch("x_SiteCategoryGroupID")
		CompanySiteParameter.SiteParameterTypeID.AdvancedSearch.SearchValue = CompanySiteParameter.GetAdvancedSearch("x_SiteParameterTypeID")
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
		CompanySiteParameter_search = New cCompanySiteParameter_search(Me)		
		CompanySiteParameter_search.Page_Init()

		' Page main processing
		CompanySiteParameter_search.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If CompanySiteParameter_search IsNot Nothing Then CompanySiteParameter_search.Dispose()
	End Sub
End Class
