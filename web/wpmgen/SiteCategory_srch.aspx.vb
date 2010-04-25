Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class SiteCategory_srch
	Inherits AspNetMaker7_WPMGen

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
		Public Sub New(ByRef APage As AspNetMaker7_WPMGen)				
			m_ParentPage = APage
			m_Page = Me	
			m_PageID = "search"
			m_PageObjName = "SiteCategory_search"
			m_PageObjTypeName = "cSiteCategory_search"

			' Table Name
			m_TableName = "SiteCategory"

			' Initialize table object
			SiteCategory = New cSiteCategory(Me)

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
			SiteCategory.Dispose()

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
					Page_Terminate("SiteCategory_list.aspx" & "?" & sSrchStr) ' Go to list page
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
		BuildSearchUrl(sSrchUrl, SiteCategory.SiteCategoryTypeID) ' SiteCategoryTypeID
		BuildSearchUrl(sSrchUrl, SiteCategory.GroupOrder) ' GroupOrder
		BuildSearchUrl(sSrchUrl, SiteCategory.CategoryName) ' CategoryName
		BuildSearchUrl(sSrchUrl, SiteCategory.CategoryTitle) ' CategoryTitle
		BuildSearchUrl(sSrchUrl, SiteCategory.CategoryDescription) ' CategoryDescription
		BuildSearchUrl(sSrchUrl, SiteCategory.CategoryKeywords) ' CategoryKeywords
		BuildSearchUrl(sSrchUrl, SiteCategory.ParentCategoryID) ' ParentCategoryID
		BuildSearchUrl(sSrchUrl, SiteCategory.CategoryFileName) ' CategoryFileName
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
		SiteCategory.SiteCategoryTypeID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_SiteCategoryTypeID")
    	SiteCategory.SiteCategoryTypeID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_SiteCategoryTypeID")
		SiteCategory.GroupOrder.AdvancedSearch.SearchValue = ObjForm.GetValue("x_GroupOrder")
    	SiteCategory.GroupOrder.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_GroupOrder")
		SiteCategory.CategoryName.AdvancedSearch.SearchValue = ObjForm.GetValue("x_CategoryName")
    	SiteCategory.CategoryName.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_CategoryName")
		SiteCategory.CategoryTitle.AdvancedSearch.SearchValue = ObjForm.GetValue("x_CategoryTitle")
    	SiteCategory.CategoryTitle.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_CategoryTitle")
		SiteCategory.CategoryDescription.AdvancedSearch.SearchValue = ObjForm.GetValue("x_CategoryDescription")
    	SiteCategory.CategoryDescription.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_CategoryDescription")
		SiteCategory.CategoryKeywords.AdvancedSearch.SearchValue = ObjForm.GetValue("x_CategoryKeywords")
    	SiteCategory.CategoryKeywords.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_CategoryKeywords")
		SiteCategory.ParentCategoryID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_ParentCategoryID")
    	SiteCategory.ParentCategoryID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_ParentCategoryID")
		SiteCategory.CategoryFileName.AdvancedSearch.SearchValue = ObjForm.GetValue("x_CategoryFileName")
    	SiteCategory.CategoryFileName.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_CategoryFileName")
		SiteCategory.SiteCategoryGroupID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_SiteCategoryGroupID")
    	SiteCategory.SiteCategoryGroupID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_SiteCategoryGroupID")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		SiteCategory.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' SiteCategoryTypeID

		SiteCategory.SiteCategoryTypeID.CellCssStyle = ""
		SiteCategory.SiteCategoryTypeID.CellCssClass = ""

		' GroupOrder
		SiteCategory.GroupOrder.CellCssStyle = ""
		SiteCategory.GroupOrder.CellCssClass = ""

		' CategoryName
		SiteCategory.CategoryName.CellCssStyle = ""
		SiteCategory.CategoryName.CellCssClass = ""

		' CategoryTitle
		SiteCategory.CategoryTitle.CellCssStyle = ""
		SiteCategory.CategoryTitle.CellCssClass = ""

		' CategoryDescription
		SiteCategory.CategoryDescription.CellCssStyle = ""
		SiteCategory.CategoryDescription.CellCssClass = ""

		' CategoryKeywords
		SiteCategory.CategoryKeywords.CellCssStyle = ""
		SiteCategory.CategoryKeywords.CellCssClass = ""

		' ParentCategoryID
		SiteCategory.ParentCategoryID.CellCssStyle = ""
		SiteCategory.ParentCategoryID.CellCssClass = ""

		' CategoryFileName
		SiteCategory.CategoryFileName.CellCssStyle = ""
		SiteCategory.CategoryFileName.CellCssClass = ""

		' SiteCategoryGroupID
		SiteCategory.SiteCategoryGroupID.CellCssStyle = ""
		SiteCategory.SiteCategoryGroupID.CellCssClass = ""

		'
		'  View  Row
		'

		If SiteCategory.RowType = EW_ROWTYPE_VIEW Then ' View row

			' SiteCategoryTypeID
			If ew_NotEmpty(SiteCategory.SiteCategoryTypeID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteCategoryTypeNM] FROM [SiteCategoryType] WHERE [SiteCategoryTypeID] = " & ew_AdjustSql(SiteCategory.SiteCategoryTypeID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryTypeNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					SiteCategory.SiteCategoryTypeID.ViewValue = RsWrk("SiteCategoryTypeNM")
				Else
					SiteCategory.SiteCategoryTypeID.ViewValue = SiteCategory.SiteCategoryTypeID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				SiteCategory.SiteCategoryTypeID.ViewValue = System.DBNull.Value
			End If
			SiteCategory.SiteCategoryTypeID.CssStyle = ""
			SiteCategory.SiteCategoryTypeID.CssClass = ""
			SiteCategory.SiteCategoryTypeID.ViewCustomAttributes = ""

			' GroupOrder
			SiteCategory.GroupOrder.ViewValue = SiteCategory.GroupOrder.CurrentValue
			SiteCategory.GroupOrder.CssStyle = ""
			SiteCategory.GroupOrder.CssClass = ""
			SiteCategory.GroupOrder.ViewCustomAttributes = ""

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

			' CategoryKeywords
			SiteCategory.CategoryKeywords.ViewValue = SiteCategory.CategoryKeywords.CurrentValue
			SiteCategory.CategoryKeywords.CssStyle = ""
			SiteCategory.CategoryKeywords.CssClass = ""
			SiteCategory.CategoryKeywords.ViewCustomAttributes = ""

			' ParentCategoryID
			If ew_NotEmpty(SiteCategory.ParentCategoryID.CurrentValue) Then
				sSqlWrk = "SELECT [CategoryName] FROM [SiteCategory] WHERE [SiteCategoryID] = " & ew_AdjustSql(SiteCategory.ParentCategoryID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [CategoryName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					SiteCategory.ParentCategoryID.ViewValue = RsWrk("CategoryName")
				Else
					SiteCategory.ParentCategoryID.ViewValue = SiteCategory.ParentCategoryID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				SiteCategory.ParentCategoryID.ViewValue = System.DBNull.Value
			End If
			SiteCategory.ParentCategoryID.CssStyle = ""
			SiteCategory.ParentCategoryID.CssClass = ""
			SiteCategory.ParentCategoryID.ViewCustomAttributes = ""

			' CategoryFileName
			SiteCategory.CategoryFileName.ViewValue = SiteCategory.CategoryFileName.CurrentValue
			SiteCategory.CategoryFileName.CssStyle = ""
			SiteCategory.CategoryFileName.CssClass = ""
			SiteCategory.CategoryFileName.ViewCustomAttributes = ""

			' SiteCategoryGroupID
			If ew_NotEmpty(SiteCategory.SiteCategoryGroupID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteCategoryGroupNM] FROM [SiteCategoryGroup] WHERE [SiteCategoryGroupID] = " & ew_AdjustSql(SiteCategory.SiteCategoryGroupID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryGroupNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					SiteCategory.SiteCategoryGroupID.ViewValue = RsWrk("SiteCategoryGroupNM")
				Else
					SiteCategory.SiteCategoryGroupID.ViewValue = SiteCategory.SiteCategoryGroupID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				SiteCategory.SiteCategoryGroupID.ViewValue = System.DBNull.Value
			End If
			SiteCategory.SiteCategoryGroupID.CssStyle = ""
			SiteCategory.SiteCategoryGroupID.CssClass = ""
			SiteCategory.SiteCategoryGroupID.ViewCustomAttributes = ""

			' View refer script
			' SiteCategoryTypeID

			SiteCategory.SiteCategoryTypeID.HrefValue = ""

			' GroupOrder
			SiteCategory.GroupOrder.HrefValue = ""

			' CategoryName
			SiteCategory.CategoryName.HrefValue = ""

			' CategoryTitle
			SiteCategory.CategoryTitle.HrefValue = ""

			' CategoryDescription
			SiteCategory.CategoryDescription.HrefValue = ""

			' CategoryKeywords
			SiteCategory.CategoryKeywords.HrefValue = ""

			' ParentCategoryID
			SiteCategory.ParentCategoryID.HrefValue = ""

			' CategoryFileName
			SiteCategory.CategoryFileName.HrefValue = ""

			' SiteCategoryGroupID
			SiteCategory.SiteCategoryGroupID.HrefValue = ""

		'
		'  Search Row
		'

		ElseIf SiteCategory.RowType = EW_ROWTYPE_SEARCH Then ' Search row

			' SiteCategoryTypeID
			SiteCategory.SiteCategoryTypeID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryTypeID], [SiteCategoryTypeNM], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteCategoryType]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryTypeNM] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			SiteCategory.SiteCategoryTypeID.EditValue = arwrk

			' GroupOrder
			SiteCategory.GroupOrder.EditCustomAttributes = ""
			SiteCategory.GroupOrder.EditValue = ew_HtmlEncode(SiteCategory.GroupOrder.AdvancedSearch.SearchValue)

			' CategoryName
			SiteCategory.CategoryName.EditCustomAttributes = ""
			SiteCategory.CategoryName.EditValue = ew_HtmlEncode(SiteCategory.CategoryName.AdvancedSearch.SearchValue)

			' CategoryTitle
			SiteCategory.CategoryTitle.EditCustomAttributes = ""
			SiteCategory.CategoryTitle.EditValue = ew_HtmlEncode(SiteCategory.CategoryTitle.AdvancedSearch.SearchValue)

			' CategoryDescription
			SiteCategory.CategoryDescription.EditCustomAttributes = ""
			SiteCategory.CategoryDescription.EditValue = ew_HtmlEncode(SiteCategory.CategoryDescription.AdvancedSearch.SearchValue)

			' CategoryKeywords
			SiteCategory.CategoryKeywords.EditCustomAttributes = ""
			SiteCategory.CategoryKeywords.EditValue = ew_HtmlEncode(SiteCategory.CategoryKeywords.AdvancedSearch.SearchValue)

			' ParentCategoryID
			SiteCategory.ParentCategoryID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryID], [CategoryName], '' AS Disp2Fld, [SiteCategoryTypeID] FROM [SiteCategory]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [CategoryName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select", ""}) 
			SiteCategory.ParentCategoryID.EditValue = arwrk

			' CategoryFileName
			SiteCategory.CategoryFileName.EditCustomAttributes = ""
			SiteCategory.CategoryFileName.EditValue = ew_HtmlEncode(SiteCategory.CategoryFileName.AdvancedSearch.SearchValue)

			' SiteCategoryGroupID
			SiteCategory.SiteCategoryGroupID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryGroupID], [SiteCategoryGroupNM], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteCategoryGroup]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryGroupNM] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			SiteCategory.SiteCategoryGroupID.EditValue = arwrk
		End If

		' Row Rendered event
		SiteCategory.Row_Rendered()
	End Sub

	'
	' Validate search
	'
	Function ValidateSearch() As Boolean

		' Initialize
		ParentPage.gsSearchError = ""

		' Check if validation required
		If Not EW_SERVER_VALIDATE Then Return True ' Skip
		If Not ew_CheckNumber(SiteCategory.GroupOrder.AdvancedSearch.SearchValue) Then
			If ParentPage.gsSearchError <> "" Then ParentPage.gsSearchError = ParentPage.gsSearchError & "<br />"
			ParentPage.gsSearchError = ParentPage.gsSearchError & "Incorrect floating point number - Order"
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
		SiteCategory.SiteCategoryTypeID.AdvancedSearch.SearchValue = SiteCategory.GetAdvancedSearch("x_SiteCategoryTypeID")
		SiteCategory.GroupOrder.AdvancedSearch.SearchValue = SiteCategory.GetAdvancedSearch("x_GroupOrder")
		SiteCategory.CategoryName.AdvancedSearch.SearchValue = SiteCategory.GetAdvancedSearch("x_CategoryName")
		SiteCategory.CategoryTitle.AdvancedSearch.SearchValue = SiteCategory.GetAdvancedSearch("x_CategoryTitle")
		SiteCategory.CategoryDescription.AdvancedSearch.SearchValue = SiteCategory.GetAdvancedSearch("x_CategoryDescription")
		SiteCategory.CategoryKeywords.AdvancedSearch.SearchValue = SiteCategory.GetAdvancedSearch("x_CategoryKeywords")
		SiteCategory.ParentCategoryID.AdvancedSearch.SearchValue = SiteCategory.GetAdvancedSearch("x_ParentCategoryID")
		SiteCategory.CategoryFileName.AdvancedSearch.SearchValue = SiteCategory.GetAdvancedSearch("x_CategoryFileName")
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
		SiteCategory_search = New cSiteCategory_search(Me)		
		SiteCategory_search.Page_Init()

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
