Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class zPage_srch
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public zPage_search As czPage_search

	'
	' Page Class
	'
	Class czPage_search
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
				If zPage.UseTokenInUrl Then Url = Url & "t=" & zPage.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If zPage.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (zPage.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (zPage.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' zPage
		Public Property zPage() As czPage
			Get				
				Return ParentPage.zPage
			End Get
			Set(ByVal v As czPage)
				ParentPage.zPage = v	
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
			m_PageObjName = "zPage_search"
			m_PageObjTypeName = "czPage_search"

			' Table Name
			m_TableName = "Page"

			' Initialize table object
			zPage = New czPage(Me)

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
			zPage.Dispose()

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
			zPage.CurrentAction = ObjForm.GetValue("a_search")
			Select Case zPage.CurrentAction
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
						sSrchStr = zPage.UrlParm(sSrchStr)
					Page_Terminate("zPage_list.aspx" & "?" & sSrchStr) ' Go to list page
					End If
			End Select
		End If

		' Restore search settings from Session
		If ParentPage.gsSearchError = "" Then
			LoadAdvancedSearch()
		End If

		' Render row for search
		zPage.RowType = EW_ROWTYPE_SEARCH
		RenderRow()
	End Sub

	'
	' Build advanced search
	'
	Function BuildAdvancedSearch() As String
		Dim sSrchUrl As String = ""
		BuildSearchUrl(sSrchUrl, zPage.CompanyID) ' CompanyID
		BuildSearchUrl(sSrchUrl, zPage.GroupID) ' GroupID
		BuildSearchUrl(sSrchUrl, zPage.ParentPageID) ' ParentPageID
		BuildSearchUrl(sSrchUrl, zPage.PageTypeID) ' PageTypeID
		BuildSearchUrl(sSrchUrl, zPage.Active) ' Active
		BuildSearchUrl(sSrchUrl, zPage.zPageName) ' PageName
		BuildSearchUrl(sSrchUrl, zPage.PageTitle) ' PageTitle
		BuildSearchUrl(sSrchUrl, zPage.PageDescription) ' PageDescription
		BuildSearchUrl(sSrchUrl, zPage.PageKeywords) ' PageKeywords
		BuildSearchUrl(sSrchUrl, zPage.SiteCategoryID) ' SiteCategoryID
		BuildSearchUrl(sSrchUrl, zPage.SiteCategoryGroupID) ' SiteCategoryGroupID
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
		zPage.CompanyID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_CompanyID")
    	zPage.CompanyID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_CompanyID")
		zPage.GroupID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_GroupID")
    	zPage.GroupID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_GroupID")
		zPage.ParentPageID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_ParentPageID")
    	zPage.ParentPageID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_ParentPageID")
		zPage.PageTypeID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_PageTypeID")
    	zPage.PageTypeID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_PageTypeID")
		zPage.Active.AdvancedSearch.SearchValue = ObjForm.GetValue("x_Active")
    	zPage.Active.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_Active")
		zPage.zPageName.AdvancedSearch.SearchValue = ObjForm.GetValue("x_zPageName")
    	zPage.zPageName.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_zPageName")
		zPage.PageTitle.AdvancedSearch.SearchValue = ObjForm.GetValue("x_PageTitle")
    	zPage.PageTitle.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_PageTitle")
		zPage.PageDescription.AdvancedSearch.SearchValue = ObjForm.GetValue("x_PageDescription")
    	zPage.PageDescription.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_PageDescription")
		zPage.PageKeywords.AdvancedSearch.SearchValue = ObjForm.GetValue("x_PageKeywords")
    	zPage.PageKeywords.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_PageKeywords")
		zPage.SiteCategoryID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_SiteCategoryID")
    	zPage.SiteCategoryID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_SiteCategoryID")
		zPage.SiteCategoryGroupID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_SiteCategoryGroupID")
    	zPage.SiteCategoryGroupID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_SiteCategoryGroupID")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		zPage.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' CompanyID

		zPage.CompanyID.CellCssStyle = ""
		zPage.CompanyID.CellCssClass = ""

		' GroupID
		zPage.GroupID.CellCssStyle = ""
		zPage.GroupID.CellCssClass = ""

		' ParentPageID
		zPage.ParentPageID.CellCssStyle = ""
		zPage.ParentPageID.CellCssClass = ""

		' PageTypeID
		zPage.PageTypeID.CellCssStyle = ""
		zPage.PageTypeID.CellCssClass = ""

		' Active
		zPage.Active.CellCssStyle = ""
		zPage.Active.CellCssClass = ""

		' PageName
		zPage.zPageName.CellCssStyle = ""
		zPage.zPageName.CellCssClass = ""

		' PageTitle
		zPage.PageTitle.CellCssStyle = ""
		zPage.PageTitle.CellCssClass = ""

		' PageDescription
		zPage.PageDescription.CellCssStyle = ""
		zPage.PageDescription.CellCssClass = ""

		' PageKeywords
		zPage.PageKeywords.CellCssStyle = ""
		zPage.PageKeywords.CellCssClass = ""

		' SiteCategoryID
		zPage.SiteCategoryID.CellCssStyle = ""
		zPage.SiteCategoryID.CellCssClass = ""

		' SiteCategoryGroupID
		zPage.SiteCategoryGroupID.CellCssStyle = ""
		zPage.SiteCategoryGroupID.CellCssClass = ""

		'
		'  View  Row
		'

		If zPage.RowType = EW_ROWTYPE_VIEW Then ' View row

			' CompanyID
			If ew_NotEmpty(zPage.CompanyID.CurrentValue) Then
				sSqlWrk = "SELECT [CompanyName] FROM [Company] WHERE [CompanyID] = " & ew_AdjustSql(zPage.CompanyID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					zPage.CompanyID.ViewValue = RsWrk("CompanyName")
				Else
					zPage.CompanyID.ViewValue = zPage.CompanyID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				zPage.CompanyID.ViewValue = System.DBNull.Value
			End If
			zPage.CompanyID.CssStyle = ""
			zPage.CompanyID.CssClass = ""
			zPage.CompanyID.ViewCustomAttributes = ""

			' PageOrder
			zPage.PageOrder.ViewValue = zPage.PageOrder.CurrentValue
			zPage.PageOrder.CssStyle = ""
			zPage.PageOrder.CssClass = ""
			zPage.PageOrder.ViewCustomAttributes = ""

			' GroupID
			If ew_NotEmpty(zPage.GroupID.CurrentValue) Then
				sSqlWrk = "SELECT [GroupName] FROM [Group] WHERE [GroupID] = " & ew_AdjustSql(zPage.GroupID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [GroupID] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					zPage.GroupID.ViewValue = RsWrk("GroupName")
				Else
					zPage.GroupID.ViewValue = zPage.GroupID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				zPage.GroupID.ViewValue = System.DBNull.Value
			End If
			zPage.GroupID.CssStyle = ""
			zPage.GroupID.CssClass = ""
			zPage.GroupID.ViewCustomAttributes = ""

			' ParentPageID
			If ew_NotEmpty(zPage.ParentPageID.CurrentValue) Then
				sSqlWrk = "SELECT [PageName] FROM [Page] WHERE [PageID] = " & ew_AdjustSql(zPage.ParentPageID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [PageName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					zPage.ParentPageID.ViewValue = RsWrk("PageName")
				Else
					zPage.ParentPageID.ViewValue = zPage.ParentPageID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				zPage.ParentPageID.ViewValue = System.DBNull.Value
			End If
			zPage.ParentPageID.CssStyle = ""
			zPage.ParentPageID.CssClass = ""
			zPage.ParentPageID.ViewCustomAttributes = ""

			' PageTypeID
			If ew_NotEmpty(zPage.PageTypeID.CurrentValue) Then
				sSqlWrk = "SELECT [PageTypeDesc] FROM [PageType] WHERE [PageTypeID] = " & ew_AdjustSql(zPage.PageTypeID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [PageTypeDesc] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					zPage.PageTypeID.ViewValue = RsWrk("PageTypeDesc")
				Else
					zPage.PageTypeID.ViewValue = zPage.PageTypeID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				zPage.PageTypeID.ViewValue = System.DBNull.Value
			End If
			zPage.PageTypeID.CssStyle = ""
			zPage.PageTypeID.CssClass = ""
			zPage.PageTypeID.ViewCustomAttributes = ""

			' Active
			If Convert.ToString(zPage.Active.CurrentValue) = "1" Then
				zPage.Active.ViewValue = "Yes"
			Else
				zPage.Active.ViewValue = "No"
			End If
			zPage.Active.CssStyle = ""
			zPage.Active.CssClass = ""
			zPage.Active.ViewCustomAttributes = ""

			' PageName
			zPage.zPageName.ViewValue = zPage.zPageName.CurrentValue
			zPage.zPageName.CssStyle = ""
			zPage.zPageName.CssClass = ""
			zPage.zPageName.ViewCustomAttributes = ""

			' PageTitle
			zPage.PageTitle.ViewValue = zPage.PageTitle.CurrentValue
			zPage.PageTitle.CssStyle = ""
			zPage.PageTitle.CssClass = ""
			zPage.PageTitle.ViewCustomAttributes = ""

			' PageDescription
			zPage.PageDescription.ViewValue = zPage.PageDescription.CurrentValue
			zPage.PageDescription.CssStyle = ""
			zPage.PageDescription.CssClass = ""
			zPage.PageDescription.ViewCustomAttributes = ""

			' PageKeywords
			zPage.PageKeywords.ViewValue = zPage.PageKeywords.CurrentValue
			zPage.PageKeywords.CssStyle = ""
			zPage.PageKeywords.CssClass = ""
			zPage.PageKeywords.ViewCustomAttributes = ""

			' ImagesPerRow
			zPage.ImagesPerRow.ViewValue = zPage.ImagesPerRow.CurrentValue
			zPage.ImagesPerRow.CssStyle = ""
			zPage.ImagesPerRow.CssClass = ""
			zPage.ImagesPerRow.ViewCustomAttributes = ""

			' RowsPerPage
			zPage.RowsPerPage.ViewValue = zPage.RowsPerPage.CurrentValue
			zPage.RowsPerPage.CssStyle = ""
			zPage.RowsPerPage.CssClass = ""
			zPage.RowsPerPage.ViewCustomAttributes = ""

			' AllowMessage
			If Convert.ToString(zPage.AllowMessage.CurrentValue) = "1" Then
				zPage.AllowMessage.ViewValue = "Yes"
			Else
				zPage.AllowMessage.ViewValue = "No"
			End If
			zPage.AllowMessage.CssStyle = ""
			zPage.AllowMessage.CssClass = ""
			zPage.AllowMessage.ViewCustomAttributes = ""

			' PageFileName
			zPage.PageFileName.ViewValue = zPage.PageFileName.CurrentValue
			zPage.PageFileName.CssStyle = ""
			zPage.PageFileName.CssClass = ""
			zPage.PageFileName.ViewCustomAttributes = ""

			' VersionNo
			zPage.VersionNo.ViewValue = zPage.VersionNo.CurrentValue
			zPage.VersionNo.CssStyle = ""
			zPage.VersionNo.CssClass = ""
			zPage.VersionNo.ViewCustomAttributes = ""

			' SiteCategoryID
			If ew_NotEmpty(zPage.SiteCategoryID.CurrentValue) Then
				sSqlWrk = "SELECT [CategoryName] FROM [SiteCategory] WHERE [SiteCategoryID] = " & ew_AdjustSql(zPage.SiteCategoryID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [CategoryName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					zPage.SiteCategoryID.ViewValue = RsWrk("CategoryName")
				Else
					zPage.SiteCategoryID.ViewValue = zPage.SiteCategoryID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				zPage.SiteCategoryID.ViewValue = System.DBNull.Value
			End If
			zPage.SiteCategoryID.CssStyle = ""
			zPage.SiteCategoryID.CssClass = ""
			zPage.SiteCategoryID.ViewCustomAttributes = ""

			' SiteCategoryGroupID
			If ew_NotEmpty(zPage.SiteCategoryGroupID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteCategoryGroupNM] FROM [SiteCategoryGroup] WHERE [SiteCategoryGroupID] = " & ew_AdjustSql(zPage.SiteCategoryGroupID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryGroupNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					zPage.SiteCategoryGroupID.ViewValue = RsWrk("SiteCategoryGroupNM")
				Else
					zPage.SiteCategoryGroupID.ViewValue = zPage.SiteCategoryGroupID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				zPage.SiteCategoryGroupID.ViewValue = System.DBNull.Value
			End If
			zPage.SiteCategoryGroupID.CssStyle = ""
			zPage.SiteCategoryGroupID.CssClass = ""
			zPage.SiteCategoryGroupID.ViewCustomAttributes = ""

			' ModifiedDT
			zPage.ModifiedDT.ViewValue = zPage.ModifiedDT.CurrentValue
			zPage.ModifiedDT.ViewValue = ew_FormatDateTime(zPage.ModifiedDT.ViewValue, 6)
			zPage.ModifiedDT.CssStyle = ""
			zPage.ModifiedDT.CssClass = ""
			zPage.ModifiedDT.ViewCustomAttributes = ""

			' View refer script
			' CompanyID

			zPage.CompanyID.HrefValue = ""

			' GroupID
			zPage.GroupID.HrefValue = ""

			' ParentPageID
			zPage.ParentPageID.HrefValue = ""

			' PageTypeID
			zPage.PageTypeID.HrefValue = ""

			' Active
			zPage.Active.HrefValue = ""

			' PageName
			zPage.zPageName.HrefValue = ""

			' PageTitle
			zPage.PageTitle.HrefValue = ""

			' PageDescription
			zPage.PageDescription.HrefValue = ""

			' PageKeywords
			zPage.PageKeywords.HrefValue = ""

			' SiteCategoryID
			zPage.SiteCategoryID.HrefValue = ""

			' SiteCategoryGroupID
			zPage.SiteCategoryGroupID.HrefValue = ""

		'
		'  Search Row
		'

		ElseIf zPage.RowType = EW_ROWTYPE_SEARCH Then ' Search row

			' CompanyID
			zPage.CompanyID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [CompanyID], [CompanyName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Company]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			zPage.CompanyID.EditValue = arwrk

			' GroupID
			zPage.GroupID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [GroupID], [GroupName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Group]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [GroupID] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			zPage.GroupID.EditValue = arwrk

			' ParentPageID
			zPage.ParentPageID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [PageID], [PageName], '' AS Disp2Fld, [CompanyID] FROM [Page]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [PageName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select", ""}) 
			zPage.ParentPageID.EditValue = arwrk

			' PageTypeID
			zPage.PageTypeID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [PageTypeID], [PageTypeDesc], '' AS Disp2Fld, '' AS SelectFilterFld FROM [PageType]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [PageTypeDesc] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			zPage.PageTypeID.EditValue = arwrk

			' Active
			zPage.Active.EditCustomAttributes = ""

			' PageName
			zPage.zPageName.EditCustomAttributes = ""
			zPage.zPageName.EditValue = ew_HtmlEncode(zPage.zPageName.AdvancedSearch.SearchValue)

			' PageTitle
			zPage.PageTitle.EditCustomAttributes = ""
			zPage.PageTitle.EditValue = ew_HtmlEncode(zPage.PageTitle.AdvancedSearch.SearchValue)

			' PageDescription
			zPage.PageDescription.EditCustomAttributes = ""
			zPage.PageDescription.EditValue = ew_HtmlEncode(zPage.PageDescription.AdvancedSearch.SearchValue)

			' PageKeywords
			zPage.PageKeywords.EditCustomAttributes = ""
			zPage.PageKeywords.EditValue = ew_HtmlEncode(zPage.PageKeywords.AdvancedSearch.SearchValue)

			' SiteCategoryID
			zPage.SiteCategoryID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryID], [CategoryName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteCategory]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [CategoryName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			zPage.SiteCategoryID.EditValue = arwrk

			' SiteCategoryGroupID
			zPage.SiteCategoryGroupID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryGroupID], [SiteCategoryGroupNM], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteCategoryGroup]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryGroupNM] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			zPage.SiteCategoryGroupID.EditValue = arwrk
		End If

		' Row Rendered event
		zPage.Row_Rendered()
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
		zPage.CompanyID.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_CompanyID")
		zPage.GroupID.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_GroupID")
		zPage.ParentPageID.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_ParentPageID")
		zPage.PageTypeID.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_PageTypeID")
		zPage.Active.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_Active")
		zPage.zPageName.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_zPageName")
		zPage.PageTitle.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_PageTitle")
		zPage.PageDescription.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_PageDescription")
		zPage.PageKeywords.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_PageKeywords")
		zPage.SiteCategoryID.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_SiteCategoryID")
		zPage.SiteCategoryGroupID.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_SiteCategoryGroupID")
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
		zPage_search = New czPage_search(Me)		
		zPage_search.Page_Init()

		' Page main processing
		zPage_search.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If zPage_search IsNot Nothing Then zPage_search.Dispose()
	End Sub
End Class
