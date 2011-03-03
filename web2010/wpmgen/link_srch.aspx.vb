Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class link_srch
	Inherits AspNetMaker8_wpmWebsite

	' Page object
	Public Link_search As cLink_search

	'
	' Page Class
	'
	Class cLink_search
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
				If Link.UseTokenInUrl Then Url = Url & "t=" & Link.TableVar & "&" ' Add page token
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
			If Link.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (Link.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (Link.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' ASP.NET page object
		Public ReadOnly Property AspNetPage() As link_srch
			Get
				Return CType(m_ParentPage, link_srch)
			End Get
		End Property

		' Link
		Public Property Link() As cLink
			Get				
				Return ParentPage.Link
			End Get
			Set(ByVal v As cLink)
				ParentPage.Link = v	
			End Set	
		End Property

		' Link
		Public Property Company() As cCompany
			Get				
				Return ParentPage.Company
			End Get
			Set(ByVal v As cCompany)
				ParentPage.Company = v	
			End Set	
		End Property

		' Link
		Public Property LinkCategory() As cLinkCategory
			Get				
				Return ParentPage.LinkCategory
			End Get
			Set(ByVal v As cLinkCategory)
				ParentPage.LinkCategory = v	
			End Set	
		End Property

		' Link
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
		Public Sub New(ByRef APage As AspNetMaker8_wpmWebsite)				
			m_ParentPage = APage
			m_Page = Me
			m_PageID = "search"
			m_PageObjName = "Link_search"
			m_PageObjTypeName = "cLink_search"			

			' Initialize language object
			Language = New cLanguage(Me)

			' Table Name
			m_TableName = "Link"

			' Initialize table object
			Link = New cLink(Me)
			Company = New cCompany(Me)
			LinkCategory = New cLinkCategory(Me)
			LinkType = New cLinkType(Me)

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
			Link.Dispose()
			Company.Dispose()
			LinkCategory.Dispose()
			LinkType.Dispose()
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
			Link.CurrentAction = ObjForm.GetValue("a_search")
			Select Case Link.CurrentAction
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
						sSrchStr = Link.UrlParm(sSrchStr)
					Page_Terminate("link_list.aspx" & "?" & sSrchStr) ' Go to list page
					End If
			End Select
		End If

		' Restore search settings from Session
		If ParentPage.gsSearchError = "" Then
			LoadAdvancedSearch()
		End If

		' Render row for search
		Link.RowType = EW_ROWTYPE_SEARCH
		RenderRow()
	End Sub

	'
	' Build advanced search
	'
	Function BuildAdvancedSearch() As String
		Dim sSrchUrl As String = ""
		BuildSearchUrl(sSrchUrl, Link.Title) ' Title
		BuildSearchUrl(sSrchUrl, Link.LinkTypeCD) ' LinkTypeCD
		BuildSearchUrl(sSrchUrl, Link.CategoryID) ' CategoryID
		BuildSearchUrl(sSrchUrl, Link.CompanyID) ' CompanyID
		BuildSearchUrl(sSrchUrl, Link.SiteCategoryGroupID) ' SiteCategoryGroupID
		BuildSearchUrl(sSrchUrl, Link.zPageID) ' PageID
		BuildSearchUrl(sSrchUrl, Link.Views) ' Views
		BuildSearchUrl(sSrchUrl, Link.Description) ' Description
		BuildSearchUrl(sSrchUrl, Link.ASIN) ' ASIN
		BuildSearchUrl(sSrchUrl, Link.DateAdd) ' DateAdd
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
		Link.Title.AdvancedSearch.SearchValue = ObjForm.GetValue("x_Title")
    	Link.Title.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_Title")
		Link.LinkTypeCD.AdvancedSearch.SearchValue = ObjForm.GetValue("x_LinkTypeCD")
    	Link.LinkTypeCD.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_LinkTypeCD")
		Link.CategoryID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_CategoryID")
    	Link.CategoryID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_CategoryID")
		Link.CompanyID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_CompanyID")
    	Link.CompanyID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_CompanyID")
		Link.SiteCategoryGroupID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_SiteCategoryGroupID")
    	Link.SiteCategoryGroupID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_SiteCategoryGroupID")
		Link.zPageID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_zPageID")
    	Link.zPageID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_zPageID")
		Link.Views.AdvancedSearch.SearchValue = ObjForm.GetValue("x_Views")
    	Link.Views.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_Views")
		Link.Description.AdvancedSearch.SearchValue = ObjForm.GetValue("x_Description")
    	Link.Description.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_Description")
		Link.ASIN.AdvancedSearch.SearchValue = ObjForm.GetValue("x_ASIN")
    	Link.ASIN.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_ASIN")
		Link.DateAdd.AdvancedSearch.SearchValue = ObjForm.GetValue("x_DateAdd")
    	Link.DateAdd.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_DateAdd")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Initialize urls
		' Row Rendering event

		Link.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' Title

		Link.Title.CellCssStyle = ""
		Link.Title.CellCssClass = ""
		Link.Title.CellAttrs.Clear(): Link.Title.ViewAttrs.Clear(): Link.Title.EditAttrs.Clear()

		' LinkTypeCD
		Link.LinkTypeCD.CellCssStyle = ""
		Link.LinkTypeCD.CellCssClass = ""
		Link.LinkTypeCD.CellAttrs.Clear(): Link.LinkTypeCD.ViewAttrs.Clear(): Link.LinkTypeCD.EditAttrs.Clear()

		' CategoryID
		Link.CategoryID.CellCssStyle = ""
		Link.CategoryID.CellCssClass = ""
		Link.CategoryID.CellAttrs.Clear(): Link.CategoryID.ViewAttrs.Clear(): Link.CategoryID.EditAttrs.Clear()

		' CompanyID
		Link.CompanyID.CellCssStyle = ""
		Link.CompanyID.CellCssClass = ""
		Link.CompanyID.CellAttrs.Clear(): Link.CompanyID.ViewAttrs.Clear(): Link.CompanyID.EditAttrs.Clear()

		' SiteCategoryGroupID
		Link.SiteCategoryGroupID.CellCssStyle = ""
		Link.SiteCategoryGroupID.CellCssClass = ""
		Link.SiteCategoryGroupID.CellAttrs.Clear(): Link.SiteCategoryGroupID.ViewAttrs.Clear(): Link.SiteCategoryGroupID.EditAttrs.Clear()

		' PageID
		Link.zPageID.CellCssStyle = ""
		Link.zPageID.CellCssClass = ""
		Link.zPageID.CellAttrs.Clear(): Link.zPageID.ViewAttrs.Clear(): Link.zPageID.EditAttrs.Clear()

		' Views
		Link.Views.CellCssStyle = ""
		Link.Views.CellCssClass = ""
		Link.Views.CellAttrs.Clear(): Link.Views.ViewAttrs.Clear(): Link.Views.EditAttrs.Clear()

		' Description
		Link.Description.CellCssStyle = ""
		Link.Description.CellCssClass = ""
		Link.Description.CellAttrs.Clear(): Link.Description.ViewAttrs.Clear(): Link.Description.EditAttrs.Clear()

		' ASIN
		Link.ASIN.CellCssStyle = ""
		Link.ASIN.CellCssClass = ""
		Link.ASIN.CellAttrs.Clear(): Link.ASIN.ViewAttrs.Clear(): Link.ASIN.EditAttrs.Clear()

		' DateAdd
		Link.DateAdd.CellCssStyle = ""
		Link.DateAdd.CellCssClass = ""
		Link.DateAdd.CellAttrs.Clear(): Link.DateAdd.ViewAttrs.Clear(): Link.DateAdd.EditAttrs.Clear()

		'
		'  View  Row
		'

		If Link.RowType = EW_ROWTYPE_VIEW Then ' View row

			' ID
			Link.ID.ViewValue = Link.ID.CurrentValue
			Link.ID.CssStyle = ""
			Link.ID.CssClass = ""
			Link.ID.ViewCustomAttributes = ""

			' Title
			Link.Title.ViewValue = Link.Title.CurrentValue
			Link.Title.CssStyle = ""
			Link.Title.CssClass = ""
			Link.Title.ViewCustomAttributes = ""

			' LinkTypeCD
			If ew_NotEmpty(Link.LinkTypeCD.CurrentValue) Then
				sFilterWrk = "[LinkTypeCD] = '" & ew_AdjustSql(Link.LinkTypeCD.CurrentValue) & "'"
			sSqlWrk = "SELECT [LinkTypeDesc] FROM [LinkType]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [LinkTypeDesc] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Link.LinkTypeCD.ViewValue = RsWrk("LinkTypeDesc")
				Else
					Link.LinkTypeCD.ViewValue = Link.LinkTypeCD.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Link.LinkTypeCD.ViewValue = System.DBNull.Value
			End If
			Link.LinkTypeCD.CssStyle = ""
			Link.LinkTypeCD.CssClass = ""
			Link.LinkTypeCD.ViewCustomAttributes = ""

			' CategoryID
			If ew_NotEmpty(Link.CategoryID.CurrentValue) Then
				sFilterWrk = "[ID] = " & ew_AdjustSql(Link.CategoryID.CurrentValue) & ""
			sSqlWrk = "SELECT [Title] FROM [LinkCategory]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [Title] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Link.CategoryID.ViewValue = RsWrk("Title")
				Else
					Link.CategoryID.ViewValue = Link.CategoryID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Link.CategoryID.ViewValue = System.DBNull.Value
			End If
			Link.CategoryID.CssStyle = ""
			Link.CategoryID.CssClass = ""
			Link.CategoryID.ViewCustomAttributes = ""

			' CompanyID
			If ew_NotEmpty(Link.CompanyID.CurrentValue) Then
				sFilterWrk = "[CompanyID] = " & ew_AdjustSql(Link.CompanyID.CurrentValue) & ""
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
					Link.CompanyID.ViewValue = RsWrk("CompanyName")
				Else
					Link.CompanyID.ViewValue = Link.CompanyID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Link.CompanyID.ViewValue = System.DBNull.Value
			End If
			Link.CompanyID.CssStyle = ""
			Link.CompanyID.CssClass = ""
			Link.CompanyID.ViewCustomAttributes = ""

			' SiteCategoryGroupID
			If ew_NotEmpty(Link.SiteCategoryGroupID.CurrentValue) Then
				sFilterWrk = "[SiteCategoryGroupID] = " & ew_AdjustSql(Link.SiteCategoryGroupID.CurrentValue) & ""
			sSqlWrk = "SELECT [SiteCategoryGroupNM] FROM [SiteCategoryGroup]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [SiteCategoryGroupNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Link.SiteCategoryGroupID.ViewValue = RsWrk("SiteCategoryGroupNM")
				Else
					Link.SiteCategoryGroupID.ViewValue = Link.SiteCategoryGroupID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Link.SiteCategoryGroupID.ViewValue = System.DBNull.Value
			End If
			Link.SiteCategoryGroupID.CssStyle = ""
			Link.SiteCategoryGroupID.CssClass = ""
			Link.SiteCategoryGroupID.ViewCustomAttributes = ""

			' PageID
			If ew_NotEmpty(Link.zPageID.CurrentValue) Then
				sFilterWrk = "[PageID] = " & ew_AdjustSql(Link.zPageID.CurrentValue) & ""
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
					Link.zPageID.ViewValue = RsWrk("PageName")
				Else
					Link.zPageID.ViewValue = Link.zPageID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Link.zPageID.ViewValue = System.DBNull.Value
			End If
			Link.zPageID.CssStyle = ""
			Link.zPageID.CssClass = ""
			Link.zPageID.ViewCustomAttributes = ""

			' Views
			If Convert.ToString(Link.Views.CurrentValue) = "1" Then
				Link.Views.ViewValue = "Yes"
			Else
				Link.Views.ViewValue = "No"
			End If
			Link.Views.CssStyle = ""
			Link.Views.CssClass = ""
			Link.Views.ViewCustomAttributes = ""

			' Description
			Link.Description.ViewValue = Link.Description.CurrentValue
			Link.Description.CssStyle = ""
			Link.Description.CssClass = ""
			Link.Description.ViewCustomAttributes = ""

			' URL
			Link.URL.ViewValue = Link.URL.CurrentValue
			Link.URL.CssStyle = ""
			Link.URL.CssClass = ""
			Link.URL.ViewCustomAttributes = ""

			' Ranks
			Link.Ranks.ViewValue = Link.Ranks.CurrentValue
			Link.Ranks.CssStyle = ""
			Link.Ranks.CssClass = ""
			Link.Ranks.ViewCustomAttributes = ""

			' UserID
			If ew_NotEmpty(Link.UserID.CurrentValue) Then
				sFilterWrk = "[ContactID] = " & ew_AdjustSql(Link.UserID.CurrentValue) & ""
			sSqlWrk = "SELECT [PrimaryContact] FROM [Contact]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [PrimaryContact] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Link.UserID.ViewValue = RsWrk("PrimaryContact")
				Else
					Link.UserID.ViewValue = Link.UserID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Link.UserID.ViewValue = System.DBNull.Value
			End If
			Link.UserID.CssStyle = ""
			Link.UserID.CssClass = ""
			Link.UserID.ViewCustomAttributes = ""

			' ASIN
			Link.ASIN.ViewValue = Link.ASIN.CurrentValue
			Link.ASIN.CssStyle = ""
			Link.ASIN.CssClass = ""
			Link.ASIN.ViewCustomAttributes = ""

			' DateAdd
			Link.DateAdd.ViewValue = Link.DateAdd.CurrentValue
			Link.DateAdd.CssStyle = ""
			Link.DateAdd.CssClass = ""
			Link.DateAdd.ViewCustomAttributes = ""

			' UserName
			Link.UserName.ViewValue = Link.UserName.CurrentValue
			Link.UserName.CssStyle = ""
			Link.UserName.CssClass = ""
			Link.UserName.ViewCustomAttributes = ""

			' View refer script
			' Title

			Link.Title.HrefValue = ""
			Link.Title.TooltipValue = ""

			' LinkTypeCD
			Link.LinkTypeCD.HrefValue = ""
			Link.LinkTypeCD.TooltipValue = ""

			' CategoryID
			Link.CategoryID.HrefValue = ""
			Link.CategoryID.TooltipValue = ""

			' CompanyID
			Link.CompanyID.HrefValue = ""
			Link.CompanyID.TooltipValue = ""

			' SiteCategoryGroupID
			Link.SiteCategoryGroupID.HrefValue = ""
			Link.SiteCategoryGroupID.TooltipValue = ""

			' PageID
			Link.zPageID.HrefValue = ""
			Link.zPageID.TooltipValue = ""

			' Views
			Link.Views.HrefValue = ""
			Link.Views.TooltipValue = ""

			' Description
			Link.Description.HrefValue = ""
			Link.Description.TooltipValue = ""

			' ASIN
			Link.ASIN.HrefValue = ""
			Link.ASIN.TooltipValue = ""

			' DateAdd
			Link.DateAdd.HrefValue = ""
			Link.DateAdd.TooltipValue = ""

		'
		'  Search Row
		'

		ElseIf Link.RowType = EW_ROWTYPE_SEARCH Then ' Search row

			' Title
			Link.Title.EditCustomAttributes = ""
			Link.Title.EditValue = ew_HtmlEncode(Link.Title.AdvancedSearch.SearchValue)

			' LinkTypeCD
			Link.LinkTypeCD.EditCustomAttributes = ""
			sFilterWrk = ""
			sSqlWrk = "SELECT [LinkTypeCD], [LinkTypeDesc], '' AS Disp2Fld, '' AS SelectFilterFld FROM [LinkType]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [LinkTypeDesc] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", Language.Phrase("PleaseSelect")}) 
			Link.LinkTypeCD.EditValue = arwrk

			' CategoryID
			Link.CategoryID.EditCustomAttributes = ""
			sFilterWrk = ""
			sSqlWrk = "SELECT [ID], [Title], '' AS Disp2Fld, '' AS SelectFilterFld FROM [LinkCategory]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [Title] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", Language.Phrase("PleaseSelect")}) 
			Link.CategoryID.EditValue = arwrk

			' CompanyID
			Link.CompanyID.EditCustomAttributes = ""
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
			Link.CompanyID.EditValue = arwrk

			' SiteCategoryGroupID
			Link.SiteCategoryGroupID.EditCustomAttributes = ""
			sFilterWrk = ""
			sSqlWrk = "SELECT [SiteCategoryGroupID], [SiteCategoryGroupNM], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteCategoryGroup]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [SiteCategoryGroupNM] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", Language.Phrase("PleaseSelect")}) 
			Link.SiteCategoryGroupID.EditValue = arwrk

			' PageID
			Link.zPageID.EditCustomAttributes = ""
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
			Link.zPageID.EditValue = arwrk

			' Views
			Link.Views.EditCustomAttributes = ""

			' Description
			Link.Description.EditCustomAttributes = ""
			Link.Description.EditValue = ew_HtmlEncode(Link.Description.AdvancedSearch.SearchValue)

			' ASIN
			Link.ASIN.EditCustomAttributes = ""
			Link.ASIN.EditValue = ew_HtmlEncode(Link.ASIN.AdvancedSearch.SearchValue)

			' DateAdd
			Link.DateAdd.EditCustomAttributes = ""
			Link.DateAdd.EditValue = Link.DateAdd.AdvancedSearch.SearchValue
		End If

		' Row Rendered event
		If Link.RowType <> EW_ROWTYPE_AGGREGATEINIT Then
			Link.Row_Rendered()
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
		Link.Title.AdvancedSearch.SearchValue = Link.GetAdvancedSearch("x_Title")
		Link.LinkTypeCD.AdvancedSearch.SearchValue = Link.GetAdvancedSearch("x_LinkTypeCD")
		Link.CategoryID.AdvancedSearch.SearchValue = Link.GetAdvancedSearch("x_CategoryID")
		Link.CompanyID.AdvancedSearch.SearchValue = Link.GetAdvancedSearch("x_CompanyID")
		Link.SiteCategoryGroupID.AdvancedSearch.SearchValue = Link.GetAdvancedSearch("x_SiteCategoryGroupID")
		Link.zPageID.AdvancedSearch.SearchValue = Link.GetAdvancedSearch("x_zPageID")
		Link.Views.AdvancedSearch.SearchValue = Link.GetAdvancedSearch("x_Views")
		Link.Description.AdvancedSearch.SearchValue = Link.GetAdvancedSearch("x_Description")
		Link.ASIN.AdvancedSearch.SearchValue = Link.GetAdvancedSearch("x_ASIN")
		Link.DateAdd.AdvancedSearch.SearchValue = Link.GetAdvancedSearch("x_DateAdd")
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
		Link_search = New cLink_search(Me)		
		Link_search.Page_Init()		

		' Set buffer/cache option
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

		' Page main processing
		Link_search.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If Link_search IsNot Nothing Then Link_search.Dispose()
	End Sub
End Class
