Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class sitelink_srch
	Inherits AspNetMaker8_wpmWebsite

	' Page object
	Public SiteLink_search As cSiteLink_search

	'
	' Page Class
	'
	Class cSiteLink_search
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
				If SiteLink.UseTokenInUrl Then Url = Url & "t=" & SiteLink.TableVar & "&" ' Add page token
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
			If SiteLink.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (SiteLink.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (SiteLink.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' ASP.NET page object
		Public ReadOnly Property AspNetPage() As sitelink_srch
			Get
				Return CType(m_ParentPage, sitelink_srch)
			End Get
		End Property

		' SiteLink
		Public Property SiteLink() As cSiteLink
			Get				
				Return ParentPage.SiteLink
			End Get
			Set(ByVal v As cSiteLink)
				ParentPage.SiteLink = v	
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
			m_PageObjName = "SiteLink_search"
			m_PageObjTypeName = "cSiteLink_search"			

			' Initialize language object
			Language = New cLanguage(Me)

			' Table Name
			m_TableName = "SiteLink"

			' Initialize table object
			SiteLink = New cSiteLink(Me)

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
			SiteLink.Dispose()
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
			SiteLink.CurrentAction = ObjForm.GetValue("a_search")
			Select Case SiteLink.CurrentAction
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
						sSrchStr = SiteLink.UrlParm(sSrchStr)
					Page_Terminate("sitelink_list.aspx" & "?" & sSrchStr) ' Go to list page
					End If
			End Select
		End If

		' Restore search settings from Session
		If ParentPage.gsSearchError = "" Then
			LoadAdvancedSearch()
		End If

		' Render row for search
		SiteLink.RowType = EW_ROWTYPE_SEARCH
		RenderRow()
	End Sub

	'
	' Build advanced search
	'
	Function BuildAdvancedSearch() As String
		Dim sSrchUrl As String = ""
		BuildSearchUrl(sSrchUrl, SiteLink.ID) ' ID
		BuildSearchUrl(sSrchUrl, SiteLink.CompanyID) ' CompanyID
		BuildSearchUrl(sSrchUrl, SiteLink.LinkTypeCD) ' LinkTypeCD
		BuildSearchUrl(sSrchUrl, SiteLink.Title) ' Title
		BuildSearchUrl(sSrchUrl, SiteLink.Description) ' Description
		BuildSearchUrl(sSrchUrl, SiteLink.DateAdd) ' DateAdd
		BuildSearchUrl(sSrchUrl, SiteLink.Ranks) ' Ranks
		BuildSearchUrl(sSrchUrl, SiteLink.Views) ' Views
		BuildSearchUrl(sSrchUrl, SiteLink.UserName) ' UserName
		BuildSearchUrl(sSrchUrl, SiteLink.UserID) ' UserID
		BuildSearchUrl(sSrchUrl, SiteLink.ASIN) ' ASIN
		BuildSearchUrl(sSrchUrl, SiteLink.URL) ' URL
		BuildSearchUrl(sSrchUrl, SiteLink.CategoryID) ' CategoryID
		BuildSearchUrl(sSrchUrl, SiteLink.SiteCategoryID) ' SiteCategoryID
		BuildSearchUrl(sSrchUrl, SiteLink.SiteCategoryTypeID) ' SiteCategoryTypeID
		BuildSearchUrl(sSrchUrl, SiteLink.SiteCategoryGroupID) ' SiteCategoryGroupID
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
		SiteLink.ID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_ID")
    	SiteLink.ID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_ID")
		SiteLink.CompanyID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_CompanyID")
    	SiteLink.CompanyID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_CompanyID")
		SiteLink.LinkTypeCD.AdvancedSearch.SearchValue = ObjForm.GetValue("x_LinkTypeCD")
    	SiteLink.LinkTypeCD.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_LinkTypeCD")
		SiteLink.Title.AdvancedSearch.SearchValue = ObjForm.GetValue("x_Title")
    	SiteLink.Title.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_Title")
		SiteLink.Description.AdvancedSearch.SearchValue = ObjForm.GetValue("x_Description")
    	SiteLink.Description.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_Description")
		SiteLink.DateAdd.AdvancedSearch.SearchValue = ObjForm.GetValue("x_DateAdd")
    	SiteLink.DateAdd.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_DateAdd")
		SiteLink.Ranks.AdvancedSearch.SearchValue = ObjForm.GetValue("x_Ranks")
    	SiteLink.Ranks.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_Ranks")
		SiteLink.Views.AdvancedSearch.SearchValue = ObjForm.GetValue("x_Views")
    	SiteLink.Views.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_Views")
		SiteLink.UserName.AdvancedSearch.SearchValue = ObjForm.GetValue("x_UserName")
    	SiteLink.UserName.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_UserName")
		SiteLink.UserID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_UserID")
    	SiteLink.UserID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_UserID")
		SiteLink.ASIN.AdvancedSearch.SearchValue = ObjForm.GetValue("x_ASIN")
    	SiteLink.ASIN.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_ASIN")
		SiteLink.URL.AdvancedSearch.SearchValue = ObjForm.GetValue("x_URL")
    	SiteLink.URL.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_URL")
		SiteLink.CategoryID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_CategoryID")
    	SiteLink.CategoryID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_CategoryID")
		SiteLink.SiteCategoryID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_SiteCategoryID")
    	SiteLink.SiteCategoryID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_SiteCategoryID")
		SiteLink.SiteCategoryTypeID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_SiteCategoryTypeID")
    	SiteLink.SiteCategoryTypeID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_SiteCategoryTypeID")
		SiteLink.SiteCategoryGroupID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_SiteCategoryGroupID")
    	SiteLink.SiteCategoryGroupID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_SiteCategoryGroupID")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Initialize urls
		' Row Rendering event

		SiteLink.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' ID

		SiteLink.ID.CellCssStyle = ""
		SiteLink.ID.CellCssClass = ""
		SiteLink.ID.CellAttrs.Clear(): SiteLink.ID.ViewAttrs.Clear(): SiteLink.ID.EditAttrs.Clear()

		' CompanyID
		SiteLink.CompanyID.CellCssStyle = ""
		SiteLink.CompanyID.CellCssClass = ""
		SiteLink.CompanyID.CellAttrs.Clear(): SiteLink.CompanyID.ViewAttrs.Clear(): SiteLink.CompanyID.EditAttrs.Clear()

		' LinkTypeCD
		SiteLink.LinkTypeCD.CellCssStyle = ""
		SiteLink.LinkTypeCD.CellCssClass = ""
		SiteLink.LinkTypeCD.CellAttrs.Clear(): SiteLink.LinkTypeCD.ViewAttrs.Clear(): SiteLink.LinkTypeCD.EditAttrs.Clear()

		' Title
		SiteLink.Title.CellCssStyle = ""
		SiteLink.Title.CellCssClass = ""
		SiteLink.Title.CellAttrs.Clear(): SiteLink.Title.ViewAttrs.Clear(): SiteLink.Title.EditAttrs.Clear()

		' Description
		SiteLink.Description.CellCssStyle = ""
		SiteLink.Description.CellCssClass = ""
		SiteLink.Description.CellAttrs.Clear(): SiteLink.Description.ViewAttrs.Clear(): SiteLink.Description.EditAttrs.Clear()

		' DateAdd
		SiteLink.DateAdd.CellCssStyle = ""
		SiteLink.DateAdd.CellCssClass = ""
		SiteLink.DateAdd.CellAttrs.Clear(): SiteLink.DateAdd.ViewAttrs.Clear(): SiteLink.DateAdd.EditAttrs.Clear()

		' Ranks
		SiteLink.Ranks.CellCssStyle = ""
		SiteLink.Ranks.CellCssClass = ""
		SiteLink.Ranks.CellAttrs.Clear(): SiteLink.Ranks.ViewAttrs.Clear(): SiteLink.Ranks.EditAttrs.Clear()

		' Views
		SiteLink.Views.CellCssStyle = ""
		SiteLink.Views.CellCssClass = ""
		SiteLink.Views.CellAttrs.Clear(): SiteLink.Views.ViewAttrs.Clear(): SiteLink.Views.EditAttrs.Clear()

		' UserName
		SiteLink.UserName.CellCssStyle = ""
		SiteLink.UserName.CellCssClass = ""
		SiteLink.UserName.CellAttrs.Clear(): SiteLink.UserName.ViewAttrs.Clear(): SiteLink.UserName.EditAttrs.Clear()

		' UserID
		SiteLink.UserID.CellCssStyle = ""
		SiteLink.UserID.CellCssClass = ""
		SiteLink.UserID.CellAttrs.Clear(): SiteLink.UserID.ViewAttrs.Clear(): SiteLink.UserID.EditAttrs.Clear()

		' ASIN
		SiteLink.ASIN.CellCssStyle = ""
		SiteLink.ASIN.CellCssClass = ""
		SiteLink.ASIN.CellAttrs.Clear(): SiteLink.ASIN.ViewAttrs.Clear(): SiteLink.ASIN.EditAttrs.Clear()

		' URL
		SiteLink.URL.CellCssStyle = ""
		SiteLink.URL.CellCssClass = ""
		SiteLink.URL.CellAttrs.Clear(): SiteLink.URL.ViewAttrs.Clear(): SiteLink.URL.EditAttrs.Clear()

		' CategoryID
		SiteLink.CategoryID.CellCssStyle = ""
		SiteLink.CategoryID.CellCssClass = ""
		SiteLink.CategoryID.CellAttrs.Clear(): SiteLink.CategoryID.ViewAttrs.Clear(): SiteLink.CategoryID.EditAttrs.Clear()

		' SiteCategoryID
		SiteLink.SiteCategoryID.CellCssStyle = ""
		SiteLink.SiteCategoryID.CellCssClass = ""
		SiteLink.SiteCategoryID.CellAttrs.Clear(): SiteLink.SiteCategoryID.ViewAttrs.Clear(): SiteLink.SiteCategoryID.EditAttrs.Clear()

		' SiteCategoryTypeID
		SiteLink.SiteCategoryTypeID.CellCssStyle = ""
		SiteLink.SiteCategoryTypeID.CellCssClass = ""
		SiteLink.SiteCategoryTypeID.CellAttrs.Clear(): SiteLink.SiteCategoryTypeID.ViewAttrs.Clear(): SiteLink.SiteCategoryTypeID.EditAttrs.Clear()

		' SiteCategoryGroupID
		SiteLink.SiteCategoryGroupID.CellCssStyle = ""
		SiteLink.SiteCategoryGroupID.CellCssClass = ""
		SiteLink.SiteCategoryGroupID.CellAttrs.Clear(): SiteLink.SiteCategoryGroupID.ViewAttrs.Clear(): SiteLink.SiteCategoryGroupID.EditAttrs.Clear()

		'
		'  View  Row
		'

		If SiteLink.RowType = EW_ROWTYPE_VIEW Then ' View row

			' ID
			SiteLink.ID.ViewValue = SiteLink.ID.CurrentValue
			SiteLink.ID.CssStyle = ""
			SiteLink.ID.CssClass = ""
			SiteLink.ID.ViewCustomAttributes = ""

			' CompanyID
			If ew_NotEmpty(SiteLink.CompanyID.CurrentValue) Then
				sFilterWrk = "[CompanyID] = " & ew_AdjustSql(SiteLink.CompanyID.CurrentValue) & ""
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
					SiteLink.CompanyID.ViewValue = RsWrk("CompanyName")
				Else
					SiteLink.CompanyID.ViewValue = SiteLink.CompanyID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				SiteLink.CompanyID.ViewValue = System.DBNull.Value
			End If
			SiteLink.CompanyID.CssStyle = ""
			SiteLink.CompanyID.CssClass = ""
			SiteLink.CompanyID.ViewCustomAttributes = ""

			' LinkTypeCD
			If ew_NotEmpty(SiteLink.LinkTypeCD.CurrentValue) Then
				sFilterWrk = "[LinkTypeCD] = '" & ew_AdjustSql(SiteLink.LinkTypeCD.CurrentValue) & "'"
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
					SiteLink.LinkTypeCD.ViewValue = RsWrk("LinkTypeDesc")
				Else
					SiteLink.LinkTypeCD.ViewValue = SiteLink.LinkTypeCD.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				SiteLink.LinkTypeCD.ViewValue = System.DBNull.Value
			End If
			SiteLink.LinkTypeCD.CssStyle = ""
			SiteLink.LinkTypeCD.CssClass = ""
			SiteLink.LinkTypeCD.ViewCustomAttributes = ""

			' Title
			SiteLink.Title.ViewValue = SiteLink.Title.CurrentValue
			SiteLink.Title.CssStyle = ""
			SiteLink.Title.CssClass = ""
			SiteLink.Title.ViewCustomAttributes = ""

			' Description
			SiteLink.Description.ViewValue = SiteLink.Description.CurrentValue
			SiteLink.Description.CssStyle = ""
			SiteLink.Description.CssClass = ""
			SiteLink.Description.ViewCustomAttributes = ""

			' DateAdd
			SiteLink.DateAdd.ViewValue = SiteLink.DateAdd.CurrentValue
			SiteLink.DateAdd.CssStyle = ""
			SiteLink.DateAdd.CssClass = ""
			SiteLink.DateAdd.ViewCustomAttributes = ""

			' Ranks
			SiteLink.Ranks.ViewValue = SiteLink.Ranks.CurrentValue
			SiteLink.Ranks.CssStyle = ""
			SiteLink.Ranks.CssClass = ""
			SiteLink.Ranks.ViewCustomAttributes = ""

			' Views
			If Convert.ToString(SiteLink.Views.CurrentValue) = "1" Then
				SiteLink.Views.ViewValue = "Yes"
			Else
				SiteLink.Views.ViewValue = "No"
			End If
			SiteLink.Views.CssStyle = ""
			SiteLink.Views.CssClass = ""
			SiteLink.Views.ViewCustomAttributes = ""

			' UserName
			SiteLink.UserName.ViewValue = SiteLink.UserName.CurrentValue
			SiteLink.UserName.CssStyle = ""
			SiteLink.UserName.CssClass = ""
			SiteLink.UserName.ViewCustomAttributes = ""

			' UserID
			SiteLink.UserID.ViewValue = SiteLink.UserID.CurrentValue
			SiteLink.UserID.CssStyle = ""
			SiteLink.UserID.CssClass = ""
			SiteLink.UserID.ViewCustomAttributes = ""

			' ASIN
			SiteLink.ASIN.ViewValue = SiteLink.ASIN.CurrentValue
			SiteLink.ASIN.CssStyle = ""
			SiteLink.ASIN.CssClass = ""
			SiteLink.ASIN.ViewCustomAttributes = ""

			' URL
			SiteLink.URL.ViewValue = SiteLink.URL.CurrentValue
			SiteLink.URL.CssStyle = ""
			SiteLink.URL.CssClass = ""
			SiteLink.URL.ViewCustomAttributes = ""

			' CategoryID
			If ew_NotEmpty(SiteLink.CategoryID.CurrentValue) Then
				sFilterWrk = "[ID] = " & ew_AdjustSql(SiteLink.CategoryID.CurrentValue) & ""
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
					SiteLink.CategoryID.ViewValue = RsWrk("Title")
				Else
					SiteLink.CategoryID.ViewValue = SiteLink.CategoryID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				SiteLink.CategoryID.ViewValue = System.DBNull.Value
			End If
			SiteLink.CategoryID.CssStyle = ""
			SiteLink.CategoryID.CssClass = ""
			SiteLink.CategoryID.ViewCustomAttributes = ""

			' SiteCategoryID
			SiteLink.SiteCategoryID.ViewValue = SiteLink.SiteCategoryID.CurrentValue
			SiteLink.SiteCategoryID.CssStyle = ""
			SiteLink.SiteCategoryID.CssClass = ""
			SiteLink.SiteCategoryID.ViewCustomAttributes = ""

			' SiteCategoryTypeID
			SiteLink.SiteCategoryTypeID.ViewValue = SiteLink.SiteCategoryTypeID.CurrentValue
			SiteLink.SiteCategoryTypeID.CssStyle = ""
			SiteLink.SiteCategoryTypeID.CssClass = ""
			SiteLink.SiteCategoryTypeID.ViewCustomAttributes = ""

			' SiteCategoryGroupID
			SiteLink.SiteCategoryGroupID.ViewValue = SiteLink.SiteCategoryGroupID.CurrentValue
			SiteLink.SiteCategoryGroupID.CssStyle = ""
			SiteLink.SiteCategoryGroupID.CssClass = ""
			SiteLink.SiteCategoryGroupID.ViewCustomAttributes = ""

			' View refer script
			' ID

			SiteLink.ID.HrefValue = ""
			SiteLink.ID.TooltipValue = ""

			' CompanyID
			SiteLink.CompanyID.HrefValue = ""
			SiteLink.CompanyID.TooltipValue = ""

			' LinkTypeCD
			SiteLink.LinkTypeCD.HrefValue = ""
			SiteLink.LinkTypeCD.TooltipValue = ""

			' Title
			SiteLink.Title.HrefValue = ""
			SiteLink.Title.TooltipValue = ""

			' Description
			SiteLink.Description.HrefValue = ""
			SiteLink.Description.TooltipValue = ""

			' DateAdd
			SiteLink.DateAdd.HrefValue = ""
			SiteLink.DateAdd.TooltipValue = ""

			' Ranks
			SiteLink.Ranks.HrefValue = ""
			SiteLink.Ranks.TooltipValue = ""

			' Views
			SiteLink.Views.HrefValue = ""
			SiteLink.Views.TooltipValue = ""

			' UserName
			SiteLink.UserName.HrefValue = ""
			SiteLink.UserName.TooltipValue = ""

			' UserID
			SiteLink.UserID.HrefValue = ""
			SiteLink.UserID.TooltipValue = ""

			' ASIN
			SiteLink.ASIN.HrefValue = ""
			SiteLink.ASIN.TooltipValue = ""

			' URL
			SiteLink.URL.HrefValue = ""
			SiteLink.URL.TooltipValue = ""

			' CategoryID
			SiteLink.CategoryID.HrefValue = ""
			SiteLink.CategoryID.TooltipValue = ""

			' SiteCategoryID
			SiteLink.SiteCategoryID.HrefValue = ""
			SiteLink.SiteCategoryID.TooltipValue = ""

			' SiteCategoryTypeID
			SiteLink.SiteCategoryTypeID.HrefValue = ""
			SiteLink.SiteCategoryTypeID.TooltipValue = ""

			' SiteCategoryGroupID
			SiteLink.SiteCategoryGroupID.HrefValue = ""
			SiteLink.SiteCategoryGroupID.TooltipValue = ""

		'
		'  Search Row
		'

		ElseIf SiteLink.RowType = EW_ROWTYPE_SEARCH Then ' Search row

			' ID
			SiteLink.ID.EditCustomAttributes = ""
			SiteLink.ID.EditValue = ew_HtmlEncode(SiteLink.ID.AdvancedSearch.SearchValue)

			' CompanyID
			SiteLink.CompanyID.EditCustomAttributes = ""
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
			SiteLink.CompanyID.EditValue = arwrk

			' LinkTypeCD
			SiteLink.LinkTypeCD.EditCustomAttributes = ""
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
			SiteLink.LinkTypeCD.EditValue = arwrk

			' Title
			SiteLink.Title.EditCustomAttributes = ""
			SiteLink.Title.EditValue = ew_HtmlEncode(SiteLink.Title.AdvancedSearch.SearchValue)

			' Description
			SiteLink.Description.EditCustomAttributes = ""
			SiteLink.Description.EditValue = ew_HtmlEncode(SiteLink.Description.AdvancedSearch.SearchValue)

			' DateAdd
			SiteLink.DateAdd.EditCustomAttributes = ""
			SiteLink.DateAdd.EditValue = SiteLink.DateAdd.AdvancedSearch.SearchValue

			' Ranks
			SiteLink.Ranks.EditCustomAttributes = ""
			SiteLink.Ranks.EditValue = ew_HtmlEncode(SiteLink.Ranks.AdvancedSearch.SearchValue)

			' Views
			SiteLink.Views.EditCustomAttributes = ""

			' UserName
			SiteLink.UserName.EditCustomAttributes = ""
			SiteLink.UserName.EditValue = ew_HtmlEncode(SiteLink.UserName.AdvancedSearch.SearchValue)

			' UserID
			SiteLink.UserID.EditCustomAttributes = ""
			SiteLink.UserID.EditValue = ew_HtmlEncode(SiteLink.UserID.AdvancedSearch.SearchValue)

			' ASIN
			SiteLink.ASIN.EditCustomAttributes = ""
			SiteLink.ASIN.EditValue = ew_HtmlEncode(SiteLink.ASIN.AdvancedSearch.SearchValue)

			' URL
			SiteLink.URL.EditCustomAttributes = ""
			SiteLink.URL.EditValue = ew_HtmlEncode(SiteLink.URL.AdvancedSearch.SearchValue)

			' CategoryID
			SiteLink.CategoryID.EditCustomAttributes = ""
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
			SiteLink.CategoryID.EditValue = arwrk

			' SiteCategoryID
			SiteLink.SiteCategoryID.EditCustomAttributes = ""
			SiteLink.SiteCategoryID.EditValue = ew_HtmlEncode(SiteLink.SiteCategoryID.AdvancedSearch.SearchValue)

			' SiteCategoryTypeID
			SiteLink.SiteCategoryTypeID.EditCustomAttributes = ""
			SiteLink.SiteCategoryTypeID.EditValue = ew_HtmlEncode(SiteLink.SiteCategoryTypeID.AdvancedSearch.SearchValue)

			' SiteCategoryGroupID
			SiteLink.SiteCategoryGroupID.EditCustomAttributes = ""
			SiteLink.SiteCategoryGroupID.EditValue = ew_HtmlEncode(SiteLink.SiteCategoryGroupID.AdvancedSearch.SearchValue)
		End If

		' Row Rendered event
		If SiteLink.RowType <> EW_ROWTYPE_AGGREGATEINIT Then
			SiteLink.Row_Rendered()
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
		If Not ew_CheckInteger(SiteLink.ID.AdvancedSearch.SearchValue) Then
			If ParentPage.gsSearchError <> "" Then ParentPage.gsSearchError = ParentPage.gsSearchError & "<br />"
			ParentPage.gsSearchError &= SiteLink.ID.FldErrMsg
		End If
		If Not ew_CheckInteger(SiteLink.Ranks.AdvancedSearch.SearchValue) Then
			If ParentPage.gsSearchError <> "" Then ParentPage.gsSearchError = ParentPage.gsSearchError & "<br />"
			ParentPage.gsSearchError &= SiteLink.Ranks.FldErrMsg
		End If
		If Not ew_CheckInteger(SiteLink.UserID.AdvancedSearch.SearchValue) Then
			If ParentPage.gsSearchError <> "" Then ParentPage.gsSearchError = ParentPage.gsSearchError & "<br />"
			ParentPage.gsSearchError &= SiteLink.UserID.FldErrMsg
		End If
		If Not ew_CheckInteger(SiteLink.SiteCategoryID.AdvancedSearch.SearchValue) Then
			If ParentPage.gsSearchError <> "" Then ParentPage.gsSearchError = ParentPage.gsSearchError & "<br />"
			ParentPage.gsSearchError &= SiteLink.SiteCategoryID.FldErrMsg
		End If
		If Not ew_CheckInteger(SiteLink.SiteCategoryTypeID.AdvancedSearch.SearchValue) Then
			If ParentPage.gsSearchError <> "" Then ParentPage.gsSearchError = ParentPage.gsSearchError & "<br />"
			ParentPage.gsSearchError &= SiteLink.SiteCategoryTypeID.FldErrMsg
		End If
		If Not ew_CheckInteger(SiteLink.SiteCategoryGroupID.AdvancedSearch.SearchValue) Then
			If ParentPage.gsSearchError <> "" Then ParentPage.gsSearchError = ParentPage.gsSearchError & "<br />"
			ParentPage.gsSearchError &= SiteLink.SiteCategoryGroupID.FldErrMsg
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
		SiteLink.ID.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_ID")
		SiteLink.CompanyID.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_CompanyID")
		SiteLink.LinkTypeCD.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_LinkTypeCD")
		SiteLink.Title.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_Title")
		SiteLink.Description.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_Description")
		SiteLink.DateAdd.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_DateAdd")
		SiteLink.Ranks.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_Ranks")
		SiteLink.Views.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_Views")
		SiteLink.UserName.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_UserName")
		SiteLink.UserID.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_UserID")
		SiteLink.ASIN.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_ASIN")
		SiteLink.URL.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_URL")
		SiteLink.CategoryID.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_CategoryID")
		SiteLink.SiteCategoryID.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_SiteCategoryID")
		SiteLink.SiteCategoryTypeID.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_SiteCategoryTypeID")
		SiteLink.SiteCategoryGroupID.AdvancedSearch.SearchValue = SiteLink.GetAdvancedSearch("x_SiteCategoryGroupID")
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
		SiteLink_search = New cSiteLink_search(Me)		
		SiteLink_search.Page_Init()		

		' Set buffer/cache option
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

		' Page main processing
		SiteLink_search.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If SiteLink_search IsNot Nothing Then SiteLink_search.Dispose()
	End Sub
End Class
