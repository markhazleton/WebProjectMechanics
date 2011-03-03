Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class zpage_srch
	Inherits AspNetMaker8_wpmWebsite

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

		Private sFilterWrk As String

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

		' ASP.NET page object
		Public ReadOnly Property AspNetPage() As zpage_srch
			Get
				Return CType(m_ParentPage, zpage_srch)
			End Get
		End Property

		' zPage
		Public Property zPage() As czPage
			Get				
				Return ParentPage.zPage
			End Get
			Set(ByVal v As czPage)
				ParentPage.zPage = v	
			End Set	
		End Property

		' zPage
		Public Property Company() As cCompany
			Get				
				Return ParentPage.Company
			End Get
			Set(ByVal v As cCompany)
				ParentPage.Company = v	
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
			m_PageObjName = "zPage_search"
			m_PageObjTypeName = "czPage_search"			

			' Initialize language object
			Language = New cLanguage(Me)

			' Table Name
			m_TableName = "Page"

			' Initialize table object
			zPage = New czPage(Me)
			Company = New cCompany(Me)

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
			zPage.Dispose()
			Company.Dispose()
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
					Page_Terminate("zpage_list.aspx" & "?" & sSrchStr) ' Go to list page
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
		BuildSearchUrl(sSrchUrl, zPage.ParentPageID) ' ParentPageID
		BuildSearchUrl(sSrchUrl, zPage.zPageName) ' PageName
		BuildSearchUrl(sSrchUrl, zPage.PageTitle) ' PageTitle
		BuildSearchUrl(sSrchUrl, zPage.PageTypeID) ' PageTypeID
		BuildSearchUrl(sSrchUrl, zPage.GroupID) ' GroupID
		BuildSearchUrl(sSrchUrl, zPage.Active) ' Active
		BuildSearchUrl(sSrchUrl, zPage.PageOrder) ' PageOrder
		BuildSearchUrl(sSrchUrl, zPage.CompanyID) ' CompanyID
		BuildSearchUrl(sSrchUrl, zPage.PageDescription) ' PageDescription
		BuildSearchUrl(sSrchUrl, zPage.PageKeywords) ' PageKeywords
		BuildSearchUrl(sSrchUrl, zPage.ImagesPerRow) ' ImagesPerRow
		BuildSearchUrl(sSrchUrl, zPage.RowsPerPage) ' RowsPerPage
		BuildSearchUrl(sSrchUrl, zPage.PageFileName) ' PageFileName
		BuildSearchUrl(sSrchUrl, zPage.AllowMessage) ' AllowMessage
		BuildSearchUrl(sSrchUrl, zPage.SiteCategoryID) ' SiteCategoryID
		BuildSearchUrl(sSrchUrl, zPage.SiteCategoryGroupID) ' SiteCategoryGroupID
		BuildSearchUrl(sSrchUrl, zPage.VersionNo) ' VersionNo
		BuildSearchUrl(sSrchUrl, zPage.ModifiedDT) ' ModifiedDT
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
		zPage.ParentPageID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_ParentPageID")
    	zPage.ParentPageID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_ParentPageID")
		zPage.zPageName.AdvancedSearch.SearchValue = ObjForm.GetValue("x_zPageName")
    	zPage.zPageName.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_zPageName")
		zPage.PageTitle.AdvancedSearch.SearchValue = ObjForm.GetValue("x_PageTitle")
    	zPage.PageTitle.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_PageTitle")
		zPage.PageTypeID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_PageTypeID")
    	zPage.PageTypeID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_PageTypeID")
		zPage.GroupID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_GroupID")
    	zPage.GroupID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_GroupID")
		zPage.Active.AdvancedSearch.SearchValue = ObjForm.GetValue("x_Active")
    	zPage.Active.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_Active")
		zPage.PageOrder.AdvancedSearch.SearchValue = ObjForm.GetValue("x_PageOrder")
    	zPage.PageOrder.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_PageOrder")
		zPage.CompanyID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_CompanyID")
    	zPage.CompanyID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_CompanyID")
		zPage.PageDescription.AdvancedSearch.SearchValue = ObjForm.GetValue("x_PageDescription")
    	zPage.PageDescription.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_PageDescription")
		zPage.PageKeywords.AdvancedSearch.SearchValue = ObjForm.GetValue("x_PageKeywords")
    	zPage.PageKeywords.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_PageKeywords")
		zPage.ImagesPerRow.AdvancedSearch.SearchValue = ObjForm.GetValue("x_ImagesPerRow")
    	zPage.ImagesPerRow.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_ImagesPerRow")
		zPage.RowsPerPage.AdvancedSearch.SearchValue = ObjForm.GetValue("x_RowsPerPage")
    	zPage.RowsPerPage.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_RowsPerPage")
		zPage.PageFileName.AdvancedSearch.SearchValue = ObjForm.GetValue("x_PageFileName")
    	zPage.PageFileName.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_PageFileName")
		zPage.AllowMessage.AdvancedSearch.SearchValue = ObjForm.GetValue("x_AllowMessage")
    	zPage.AllowMessage.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_AllowMessage")
		zPage.SiteCategoryID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_SiteCategoryID")
    	zPage.SiteCategoryID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_SiteCategoryID")
		zPage.SiteCategoryGroupID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_SiteCategoryGroupID")
    	zPage.SiteCategoryGroupID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_SiteCategoryGroupID")
		zPage.VersionNo.AdvancedSearch.SearchValue = ObjForm.GetValue("x_VersionNo")
    	zPage.VersionNo.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_VersionNo")
		zPage.ModifiedDT.AdvancedSearch.SearchValue = ObjForm.GetValue("x_ModifiedDT")
    	zPage.ModifiedDT.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_ModifiedDT")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Initialize urls
		' Row Rendering event

		zPage.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' ParentPageID

		zPage.ParentPageID.CellCssStyle = ""
		zPage.ParentPageID.CellCssClass = ""
		zPage.ParentPageID.CellAttrs.Clear(): zPage.ParentPageID.ViewAttrs.Clear(): zPage.ParentPageID.EditAttrs.Clear()

		' PageName
		zPage.zPageName.CellCssStyle = ""
		zPage.zPageName.CellCssClass = ""
		zPage.zPageName.CellAttrs.Clear(): zPage.zPageName.ViewAttrs.Clear(): zPage.zPageName.EditAttrs.Clear()

		' PageTitle
		zPage.PageTitle.CellCssStyle = ""
		zPage.PageTitle.CellCssClass = ""
		zPage.PageTitle.CellAttrs.Clear(): zPage.PageTitle.ViewAttrs.Clear(): zPage.PageTitle.EditAttrs.Clear()

		' PageTypeID
		zPage.PageTypeID.CellCssStyle = ""
		zPage.PageTypeID.CellCssClass = ""
		zPage.PageTypeID.CellAttrs.Clear(): zPage.PageTypeID.ViewAttrs.Clear(): zPage.PageTypeID.EditAttrs.Clear()

		' GroupID
		zPage.GroupID.CellCssStyle = ""
		zPage.GroupID.CellCssClass = ""
		zPage.GroupID.CellAttrs.Clear(): zPage.GroupID.ViewAttrs.Clear(): zPage.GroupID.EditAttrs.Clear()

		' Active
		zPage.Active.CellCssStyle = ""
		zPage.Active.CellCssClass = ""
		zPage.Active.CellAttrs.Clear(): zPage.Active.ViewAttrs.Clear(): zPage.Active.EditAttrs.Clear()

		' PageOrder
		zPage.PageOrder.CellCssStyle = ""
		zPage.PageOrder.CellCssClass = ""
		zPage.PageOrder.CellAttrs.Clear(): zPage.PageOrder.ViewAttrs.Clear(): zPage.PageOrder.EditAttrs.Clear()

		' CompanyID
		zPage.CompanyID.CellCssStyle = ""
		zPage.CompanyID.CellCssClass = ""
		zPage.CompanyID.CellAttrs.Clear(): zPage.CompanyID.ViewAttrs.Clear(): zPage.CompanyID.EditAttrs.Clear()

		' PageDescription
		zPage.PageDescription.CellCssStyle = ""
		zPage.PageDescription.CellCssClass = ""
		zPage.PageDescription.CellAttrs.Clear(): zPage.PageDescription.ViewAttrs.Clear(): zPage.PageDescription.EditAttrs.Clear()

		' PageKeywords
		zPage.PageKeywords.CellCssStyle = ""
		zPage.PageKeywords.CellCssClass = ""
		zPage.PageKeywords.CellAttrs.Clear(): zPage.PageKeywords.ViewAttrs.Clear(): zPage.PageKeywords.EditAttrs.Clear()

		' ImagesPerRow
		zPage.ImagesPerRow.CellCssStyle = ""
		zPage.ImagesPerRow.CellCssClass = ""
		zPage.ImagesPerRow.CellAttrs.Clear(): zPage.ImagesPerRow.ViewAttrs.Clear(): zPage.ImagesPerRow.EditAttrs.Clear()

		' RowsPerPage
		zPage.RowsPerPage.CellCssStyle = ""
		zPage.RowsPerPage.CellCssClass = ""
		zPage.RowsPerPage.CellAttrs.Clear(): zPage.RowsPerPage.ViewAttrs.Clear(): zPage.RowsPerPage.EditAttrs.Clear()

		' PageFileName
		zPage.PageFileName.CellCssStyle = ""
		zPage.PageFileName.CellCssClass = ""
		zPage.PageFileName.CellAttrs.Clear(): zPage.PageFileName.ViewAttrs.Clear(): zPage.PageFileName.EditAttrs.Clear()

		' AllowMessage
		zPage.AllowMessage.CellCssStyle = ""
		zPage.AllowMessage.CellCssClass = ""
		zPage.AllowMessage.CellAttrs.Clear(): zPage.AllowMessage.ViewAttrs.Clear(): zPage.AllowMessage.EditAttrs.Clear()

		' SiteCategoryID
		zPage.SiteCategoryID.CellCssStyle = ""
		zPage.SiteCategoryID.CellCssClass = ""
		zPage.SiteCategoryID.CellAttrs.Clear(): zPage.SiteCategoryID.ViewAttrs.Clear(): zPage.SiteCategoryID.EditAttrs.Clear()

		' SiteCategoryGroupID
		zPage.SiteCategoryGroupID.CellCssStyle = ""
		zPage.SiteCategoryGroupID.CellCssClass = ""
		zPage.SiteCategoryGroupID.CellAttrs.Clear(): zPage.SiteCategoryGroupID.ViewAttrs.Clear(): zPage.SiteCategoryGroupID.EditAttrs.Clear()

		' VersionNo
		zPage.VersionNo.CellCssStyle = ""
		zPage.VersionNo.CellCssClass = ""
		zPage.VersionNo.CellAttrs.Clear(): zPage.VersionNo.ViewAttrs.Clear(): zPage.VersionNo.EditAttrs.Clear()

		' ModifiedDT
		zPage.ModifiedDT.CellCssStyle = ""
		zPage.ModifiedDT.CellCssClass = ""
		zPage.ModifiedDT.CellAttrs.Clear(): zPage.ModifiedDT.ViewAttrs.Clear(): zPage.ModifiedDT.EditAttrs.Clear()

		'
		'  View  Row
		'

		If zPage.RowType = EW_ROWTYPE_VIEW Then ' View row

			' ParentPageID
			If ew_NotEmpty(zPage.ParentPageID.CurrentValue) Then
				sFilterWrk = "[PageID] = " & ew_AdjustSql(zPage.ParentPageID.CurrentValue) & ""
			sSqlWrk = "SELECT [PageName] FROM [Page]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [PageName] Asc"
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

			' PageTypeID
			If ew_NotEmpty(zPage.PageTypeID.CurrentValue) Then
				sFilterWrk = "[PageTypeID] = " & ew_AdjustSql(zPage.PageTypeID.CurrentValue) & ""
			sSqlWrk = "SELECT [PageTypeCD] FROM [PageType]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [PageTypeCD] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					zPage.PageTypeID.ViewValue = RsWrk("PageTypeCD")
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

			' GroupID
			If ew_NotEmpty(zPage.GroupID.CurrentValue) Then
				sFilterWrk = "[GroupID] = " & ew_AdjustSql(zPage.GroupID.CurrentValue) & ""
			sSqlWrk = "SELECT [GroupName] FROM [Group]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [GroupName] Asc"
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

			' Active
			If Convert.ToString(zPage.Active.CurrentValue) = "1" Then
				zPage.Active.ViewValue = "Yes"
			Else
				zPage.Active.ViewValue = "No"
			End If
			zPage.Active.CssStyle = ""
			zPage.Active.CssClass = ""
			zPage.Active.ViewCustomAttributes = ""

			' PageOrder
			zPage.PageOrder.ViewValue = zPage.PageOrder.CurrentValue
			zPage.PageOrder.CssStyle = ""
			zPage.PageOrder.CssClass = ""
			zPage.PageOrder.ViewCustomAttributes = ""

			' CompanyID
			If ew_NotEmpty(zPage.CompanyID.CurrentValue) Then
				sFilterWrk = "[CompanyID] = " & ew_AdjustSql(zPage.CompanyID.CurrentValue) & ""
			sSqlWrk = "SELECT [CompanyName] FROM [Company]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [CompanyName] Asc"
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

			' PageFileName
			zPage.PageFileName.ViewValue = zPage.PageFileName.CurrentValue
			zPage.PageFileName.CssStyle = ""
			zPage.PageFileName.CssClass = ""
			zPage.PageFileName.ViewCustomAttributes = ""

			' AllowMessage
			If Convert.ToString(zPage.AllowMessage.CurrentValue) = "1" Then
				zPage.AllowMessage.ViewValue = "Yes"
			Else
				zPage.AllowMessage.ViewValue = "No"
			End If
			zPage.AllowMessage.CssStyle = ""
			zPage.AllowMessage.CssClass = ""
			zPage.AllowMessage.ViewCustomAttributes = ""

			' SiteCategoryID
			zPage.SiteCategoryID.ViewValue = zPage.SiteCategoryID.CurrentValue
			zPage.SiteCategoryID.CssStyle = ""
			zPage.SiteCategoryID.CssClass = ""
			zPage.SiteCategoryID.ViewCustomAttributes = ""

			' SiteCategoryGroupID
			zPage.SiteCategoryGroupID.ViewValue = zPage.SiteCategoryGroupID.CurrentValue
			zPage.SiteCategoryGroupID.CssStyle = ""
			zPage.SiteCategoryGroupID.CssClass = ""
			zPage.SiteCategoryGroupID.ViewCustomAttributes = ""

			' VersionNo
			zPage.VersionNo.ViewValue = zPage.VersionNo.CurrentValue
			zPage.VersionNo.CssStyle = ""
			zPage.VersionNo.CssClass = ""
			zPage.VersionNo.ViewCustomAttributes = ""

			' ModifiedDT
			zPage.ModifiedDT.ViewValue = zPage.ModifiedDT.CurrentValue
			zPage.ModifiedDT.CssStyle = ""
			zPage.ModifiedDT.CssClass = ""
			zPage.ModifiedDT.ViewCustomAttributes = ""

			' View refer script
			' ParentPageID

			zPage.ParentPageID.HrefValue = ""
			zPage.ParentPageID.TooltipValue = ""

			' PageName
			zPage.zPageName.HrefValue = ""
			zPage.zPageName.TooltipValue = ""

			' PageTitle
			zPage.PageTitle.HrefValue = ""
			zPage.PageTitle.TooltipValue = ""

			' PageTypeID
			zPage.PageTypeID.HrefValue = ""
			zPage.PageTypeID.TooltipValue = ""

			' GroupID
			zPage.GroupID.HrefValue = ""
			zPage.GroupID.TooltipValue = ""

			' Active
			zPage.Active.HrefValue = ""
			zPage.Active.TooltipValue = ""

			' PageOrder
			zPage.PageOrder.HrefValue = ""
			zPage.PageOrder.TooltipValue = ""

			' CompanyID
			zPage.CompanyID.HrefValue = ""
			zPage.CompanyID.TooltipValue = ""

			' PageDescription
			zPage.PageDescription.HrefValue = ""
			zPage.PageDescription.TooltipValue = ""

			' PageKeywords
			zPage.PageKeywords.HrefValue = ""
			zPage.PageKeywords.TooltipValue = ""

			' ImagesPerRow
			zPage.ImagesPerRow.HrefValue = ""
			zPage.ImagesPerRow.TooltipValue = ""

			' RowsPerPage
			zPage.RowsPerPage.HrefValue = ""
			zPage.RowsPerPage.TooltipValue = ""

			' PageFileName
			zPage.PageFileName.HrefValue = ""
			zPage.PageFileName.TooltipValue = ""

			' AllowMessage
			zPage.AllowMessage.HrefValue = ""
			zPage.AllowMessage.TooltipValue = ""

			' SiteCategoryID
			zPage.SiteCategoryID.HrefValue = ""
			zPage.SiteCategoryID.TooltipValue = ""

			' SiteCategoryGroupID
			zPage.SiteCategoryGroupID.HrefValue = ""
			zPage.SiteCategoryGroupID.TooltipValue = ""

			' VersionNo
			zPage.VersionNo.HrefValue = ""
			zPage.VersionNo.TooltipValue = ""

			' ModifiedDT
			zPage.ModifiedDT.HrefValue = ""
			zPage.ModifiedDT.TooltipValue = ""

		'
		'  Search Row
		'

		ElseIf zPage.RowType = EW_ROWTYPE_SEARCH Then ' Search row

			' ParentPageID
			zPage.ParentPageID.EditCustomAttributes = ""
			sFilterWrk = ""
			sSqlWrk = "SELECT [PageID], [PageName], '' AS Disp2Fld, [CompanyID] FROM [Page]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [PageName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", Language.Phrase("PleaseSelect"), ""}) 
			zPage.ParentPageID.EditValue = arwrk

			' PageName
			zPage.zPageName.EditCustomAttributes = ""
			zPage.zPageName.EditValue = ew_HtmlEncode(zPage.zPageName.AdvancedSearch.SearchValue)

			' PageTitle
			zPage.PageTitle.EditCustomAttributes = ""
			zPage.PageTitle.EditValue = ew_HtmlEncode(zPage.PageTitle.AdvancedSearch.SearchValue)

			' PageTypeID
			zPage.PageTypeID.EditCustomAttributes = ""
			sFilterWrk = ""
			sSqlWrk = "SELECT [PageTypeID], [PageTypeCD], '' AS Disp2Fld, '' AS SelectFilterFld FROM [PageType]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [PageTypeCD] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", Language.Phrase("PleaseSelect")}) 
			zPage.PageTypeID.EditValue = arwrk

			' GroupID
			zPage.GroupID.EditCustomAttributes = ""
			sFilterWrk = ""
			sSqlWrk = "SELECT [GroupID], [GroupName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Group]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [GroupName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", Language.Phrase("PleaseSelect")}) 
			zPage.GroupID.EditValue = arwrk

			' Active
			zPage.Active.EditCustomAttributes = ""

			' PageOrder
			zPage.PageOrder.EditCustomAttributes = ""
			zPage.PageOrder.EditValue = ew_HtmlEncode(zPage.PageOrder.AdvancedSearch.SearchValue)

			' CompanyID
			zPage.CompanyID.EditCustomAttributes = ""
			sFilterWrk = ""
			sSqlWrk = "SELECT [CompanyID], [CompanyName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Company]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [CompanyName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", Language.Phrase("PleaseSelect")}) 
			zPage.CompanyID.EditValue = arwrk

			' PageDescription
			zPage.PageDescription.EditCustomAttributes = ""
			zPage.PageDescription.EditValue = ew_HtmlEncode(zPage.PageDescription.AdvancedSearch.SearchValue)

			' PageKeywords
			zPage.PageKeywords.EditCustomAttributes = ""
			zPage.PageKeywords.EditValue = ew_HtmlEncode(zPage.PageKeywords.AdvancedSearch.SearchValue)

			' ImagesPerRow
			zPage.ImagesPerRow.EditCustomAttributes = ""
			zPage.ImagesPerRow.EditValue = ew_HtmlEncode(zPage.ImagesPerRow.AdvancedSearch.SearchValue)

			' RowsPerPage
			zPage.RowsPerPage.EditCustomAttributes = ""
			zPage.RowsPerPage.EditValue = ew_HtmlEncode(zPage.RowsPerPage.AdvancedSearch.SearchValue)

			' PageFileName
			zPage.PageFileName.EditCustomAttributes = ""
			zPage.PageFileName.EditValue = ew_HtmlEncode(zPage.PageFileName.AdvancedSearch.SearchValue)

			' AllowMessage
			zPage.AllowMessage.EditCustomAttributes = ""

			' SiteCategoryID
			zPage.SiteCategoryID.EditCustomAttributes = ""
			zPage.SiteCategoryID.EditValue = ew_HtmlEncode(zPage.SiteCategoryID.AdvancedSearch.SearchValue)

			' SiteCategoryGroupID
			zPage.SiteCategoryGroupID.EditCustomAttributes = ""
			zPage.SiteCategoryGroupID.EditValue = ew_HtmlEncode(zPage.SiteCategoryGroupID.AdvancedSearch.SearchValue)

			' VersionNo
			zPage.VersionNo.EditCustomAttributes = ""
			zPage.VersionNo.EditValue = ew_HtmlEncode(zPage.VersionNo.AdvancedSearch.SearchValue)

			' ModifiedDT
			zPage.ModifiedDT.EditCustomAttributes = ""
			zPage.ModifiedDT.EditValue = zPage.ModifiedDT.AdvancedSearch.SearchValue
		End If

		' Row Rendered event
		If zPage.RowType <> EW_ROWTYPE_AGGREGATEINIT Then
			zPage.Row_Rendered()
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
		If Not ew_CheckInteger(zPage.PageOrder.AdvancedSearch.SearchValue) Then
			If ParentPage.gsSearchError <> "" Then ParentPage.gsSearchError = ParentPage.gsSearchError & "<br />"
			ParentPage.gsSearchError &= zPage.PageOrder.FldErrMsg
		End If
		If Not ew_CheckInteger(zPage.ImagesPerRow.AdvancedSearch.SearchValue) Then
			If ParentPage.gsSearchError <> "" Then ParentPage.gsSearchError = ParentPage.gsSearchError & "<br />"
			ParentPage.gsSearchError &= zPage.ImagesPerRow.FldErrMsg
		End If
		If Not ew_CheckInteger(zPage.RowsPerPage.AdvancedSearch.SearchValue) Then
			If ParentPage.gsSearchError <> "" Then ParentPage.gsSearchError = ParentPage.gsSearchError & "<br />"
			ParentPage.gsSearchError &= zPage.RowsPerPage.FldErrMsg
		End If
		If Not ew_CheckInteger(zPage.SiteCategoryID.AdvancedSearch.SearchValue) Then
			If ParentPage.gsSearchError <> "" Then ParentPage.gsSearchError = ParentPage.gsSearchError & "<br />"
			ParentPage.gsSearchError &= zPage.SiteCategoryID.FldErrMsg
		End If
		If Not ew_CheckInteger(zPage.SiteCategoryGroupID.AdvancedSearch.SearchValue) Then
			If ParentPage.gsSearchError <> "" Then ParentPage.gsSearchError = ParentPage.gsSearchError & "<br />"
			ParentPage.gsSearchError &= zPage.SiteCategoryGroupID.FldErrMsg
		End If
		If Not ew_CheckInteger(zPage.VersionNo.AdvancedSearch.SearchValue) Then
			If ParentPage.gsSearchError <> "" Then ParentPage.gsSearchError = ParentPage.gsSearchError & "<br />"
			ParentPage.gsSearchError &= zPage.VersionNo.FldErrMsg
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
		zPage.ParentPageID.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_ParentPageID")
		zPage.zPageName.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_zPageName")
		zPage.PageTitle.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_PageTitle")
		zPage.PageTypeID.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_PageTypeID")
		zPage.GroupID.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_GroupID")
		zPage.Active.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_Active")
		zPage.PageOrder.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_PageOrder")
		zPage.CompanyID.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_CompanyID")
		zPage.PageDescription.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_PageDescription")
		zPage.PageKeywords.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_PageKeywords")
		zPage.ImagesPerRow.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_ImagesPerRow")
		zPage.RowsPerPage.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_RowsPerPage")
		zPage.PageFileName.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_PageFileName")
		zPage.AllowMessage.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_AllowMessage")
		zPage.SiteCategoryID.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_SiteCategoryID")
		zPage.SiteCategoryGroupID.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_SiteCategoryGroupID")
		zPage.VersionNo.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_VersionNo")
		zPage.ModifiedDT.AdvancedSearch.SearchValue = zPage.GetAdvancedSearch("x_ModifiedDT")
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
		zPage_search = New czPage_search(Me)		
		zPage_search.Page_Init()		

		' Set buffer/cache option
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

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
