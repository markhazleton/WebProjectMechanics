Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class article_srch
	Inherits AspNetMaker8_wpmWebsite

	' Page object
	Public Article_search As cArticle_search

	'
	' Page Class
	'
	Class cArticle_search
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
				If Article.UseTokenInUrl Then Url = Url & "t=" & Article.TableVar & "&" ' Add page token
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
			If Article.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (Article.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (Article.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' ASP.NET page object
		Public ReadOnly Property AspNetPage() As article_srch
			Get
				Return CType(m_ParentPage, article_srch)
			End Get
		End Property

		' Article
		Public Property Article() As cArticle
			Get				
				Return ParentPage.Article
			End Get
			Set(ByVal v As cArticle)
				ParentPage.Article = v	
			End Set	
		End Property

		' Article
		Public Property Company() As cCompany
			Get				
				Return ParentPage.Company
			End Get
			Set(ByVal v As cCompany)
				ParentPage.Company = v	
			End Set	
		End Property

		' Article
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
		Public Sub New(ByRef APage As AspNetMaker8_wpmWebsite)				
			m_ParentPage = APage
			m_Page = Me
			m_PageID = "search"
			m_PageObjName = "Article_search"
			m_PageObjTypeName = "cArticle_search"			

			' Initialize language object
			Language = New cLanguage(Me)

			' Table Name
			m_TableName = "Article"

			' Initialize table object
			Article = New cArticle(Me)
			Company = New cCompany(Me)
			zPage = New czPage(Me)

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
			Article.Dispose()
			Company.Dispose()
			zPage.Dispose()
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
			Article.CurrentAction = ObjForm.GetValue("a_search")
			Select Case Article.CurrentAction
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
						sSrchStr = Article.UrlParm(sSrchStr)
					Page_Terminate("article_list.aspx" & "?" & sSrchStr) ' Go to list page
					End If
			End Select
		End If

		' Restore search settings from Session
		If ParentPage.gsSearchError = "" Then
			LoadAdvancedSearch()
		End If

		' Render row for search
		Article.RowType = EW_ROWTYPE_SEARCH
		RenderRow()
	End Sub

	'
	' Build advanced search
	'
	Function BuildAdvancedSearch() As String
		Dim sSrchUrl As String = ""
		BuildSearchUrl(sSrchUrl, Article.ArticleID) ' ArticleID
		BuildSearchUrl(sSrchUrl, Article.Active) ' Active
		BuildSearchUrl(sSrchUrl, Article.StartDT) ' StartDT
		BuildSearchUrl(sSrchUrl, Article.Title) ' Title
		BuildSearchUrl(sSrchUrl, Article.CompanyID) ' CompanyID
		BuildSearchUrl(sSrchUrl, Article.zPageID) ' PageID
		BuildSearchUrl(sSrchUrl, Article.userID) ' userID
		BuildSearchUrl(sSrchUrl, Article.Description) ' Description
		BuildSearchUrl(sSrchUrl, Article.ArticleSummary) ' ArticleSummary
		BuildSearchUrl(sSrchUrl, Article.ArticleBody) ' ArticleBody
		BuildSearchUrl(sSrchUrl, Article.EndDT) ' EndDT
		BuildSearchUrl(sSrchUrl, Article.ExpireDT) ' ExpireDT
		BuildSearchUrl(sSrchUrl, Article.Author) ' Author
		BuildSearchUrl(sSrchUrl, Article.Counter) ' Counter
		BuildSearchUrl(sSrchUrl, Article.VersionNo) ' VersionNo
		BuildSearchUrl(sSrchUrl, Article.ContactID) ' ContactID
		BuildSearchUrl(sSrchUrl, Article.ModifiedDT) ' ModifiedDT
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
		Article.ArticleID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_ArticleID")
    	Article.ArticleID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_ArticleID")
		Article.Active.AdvancedSearch.SearchValue = ObjForm.GetValue("x_Active")
    	Article.Active.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_Active")
		Article.StartDT.AdvancedSearch.SearchValue = ObjForm.GetValue("x_StartDT")
    	Article.StartDT.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_StartDT")
		Article.Title.AdvancedSearch.SearchValue = ObjForm.GetValue("x_Title")
    	Article.Title.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_Title")
		Article.CompanyID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_CompanyID")
    	Article.CompanyID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_CompanyID")
		Article.zPageID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_zPageID")
    	Article.zPageID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_zPageID")
		Article.userID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_userID")
    	Article.userID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_userID")
		Article.Description.AdvancedSearch.SearchValue = ObjForm.GetValue("x_Description")
    	Article.Description.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_Description")
		Article.ArticleSummary.AdvancedSearch.SearchValue = ObjForm.GetValue("x_ArticleSummary")
    	Article.ArticleSummary.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_ArticleSummary")
		Article.ArticleBody.AdvancedSearch.SearchValue = ObjForm.GetValue("x_ArticleBody")
    	Article.ArticleBody.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_ArticleBody")
		Article.EndDT.AdvancedSearch.SearchValue = ObjForm.GetValue("x_EndDT")
    	Article.EndDT.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_EndDT")
		Article.ExpireDT.AdvancedSearch.SearchValue = ObjForm.GetValue("x_ExpireDT")
    	Article.ExpireDT.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_ExpireDT")
		Article.Author.AdvancedSearch.SearchValue = ObjForm.GetValue("x_Author")
    	Article.Author.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_Author")
		Article.Counter.AdvancedSearch.SearchValue = ObjForm.GetValue("x_Counter")
    	Article.Counter.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_Counter")
		Article.VersionNo.AdvancedSearch.SearchValue = ObjForm.GetValue("x_VersionNo")
    	Article.VersionNo.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_VersionNo")
		Article.ContactID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_ContactID")
    	Article.ContactID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_ContactID")
		Article.ModifiedDT.AdvancedSearch.SearchValue = ObjForm.GetValue("x_ModifiedDT")
    	Article.ModifiedDT.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_ModifiedDT")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Initialize urls
		' Row Rendering event

		Article.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' ArticleID

		Article.ArticleID.CellCssStyle = ""
		Article.ArticleID.CellCssClass = ""
		Article.ArticleID.CellAttrs.Clear(): Article.ArticleID.ViewAttrs.Clear(): Article.ArticleID.EditAttrs.Clear()

		' Active
		Article.Active.CellCssStyle = ""
		Article.Active.CellCssClass = ""
		Article.Active.CellAttrs.Clear(): Article.Active.ViewAttrs.Clear(): Article.Active.EditAttrs.Clear()

		' StartDT
		Article.StartDT.CellCssStyle = ""
		Article.StartDT.CellCssClass = ""
		Article.StartDT.CellAttrs.Clear(): Article.StartDT.ViewAttrs.Clear(): Article.StartDT.EditAttrs.Clear()

		' Title
		Article.Title.CellCssStyle = ""
		Article.Title.CellCssClass = ""
		Article.Title.CellAttrs.Clear(): Article.Title.ViewAttrs.Clear(): Article.Title.EditAttrs.Clear()

		' CompanyID
		Article.CompanyID.CellCssStyle = ""
		Article.CompanyID.CellCssClass = ""
		Article.CompanyID.CellAttrs.Clear(): Article.CompanyID.ViewAttrs.Clear(): Article.CompanyID.EditAttrs.Clear()

		' PageID
		Article.zPageID.CellCssStyle = ""
		Article.zPageID.CellCssClass = ""
		Article.zPageID.CellAttrs.Clear(): Article.zPageID.ViewAttrs.Clear(): Article.zPageID.EditAttrs.Clear()

		' userID
		Article.userID.CellCssStyle = ""
		Article.userID.CellCssClass = ""
		Article.userID.CellAttrs.Clear(): Article.userID.ViewAttrs.Clear(): Article.userID.EditAttrs.Clear()

		' Description
		Article.Description.CellCssStyle = ""
		Article.Description.CellCssClass = ""
		Article.Description.CellAttrs.Clear(): Article.Description.ViewAttrs.Clear(): Article.Description.EditAttrs.Clear()

		' ArticleSummary
		Article.ArticleSummary.CellCssStyle = ""
		Article.ArticleSummary.CellCssClass = ""
		Article.ArticleSummary.CellAttrs.Clear(): Article.ArticleSummary.ViewAttrs.Clear(): Article.ArticleSummary.EditAttrs.Clear()

		' ArticleBody
		Article.ArticleBody.CellCssStyle = ""
		Article.ArticleBody.CellCssClass = ""
		Article.ArticleBody.CellAttrs.Clear(): Article.ArticleBody.ViewAttrs.Clear(): Article.ArticleBody.EditAttrs.Clear()

		' EndDT
		Article.EndDT.CellCssStyle = ""
		Article.EndDT.CellCssClass = ""
		Article.EndDT.CellAttrs.Clear(): Article.EndDT.ViewAttrs.Clear(): Article.EndDT.EditAttrs.Clear()

		' ExpireDT
		Article.ExpireDT.CellCssStyle = ""
		Article.ExpireDT.CellCssClass = ""
		Article.ExpireDT.CellAttrs.Clear(): Article.ExpireDT.ViewAttrs.Clear(): Article.ExpireDT.EditAttrs.Clear()

		' Author
		Article.Author.CellCssStyle = ""
		Article.Author.CellCssClass = ""
		Article.Author.CellAttrs.Clear(): Article.Author.ViewAttrs.Clear(): Article.Author.EditAttrs.Clear()

		' Counter
		Article.Counter.CellCssStyle = ""
		Article.Counter.CellCssClass = ""
		Article.Counter.CellAttrs.Clear(): Article.Counter.ViewAttrs.Clear(): Article.Counter.EditAttrs.Clear()

		' VersionNo
		Article.VersionNo.CellCssStyle = ""
		Article.VersionNo.CellCssClass = ""
		Article.VersionNo.CellAttrs.Clear(): Article.VersionNo.ViewAttrs.Clear(): Article.VersionNo.EditAttrs.Clear()

		' ContactID
		Article.ContactID.CellCssStyle = ""
		Article.ContactID.CellCssClass = ""
		Article.ContactID.CellAttrs.Clear(): Article.ContactID.ViewAttrs.Clear(): Article.ContactID.EditAttrs.Clear()

		' ModifiedDT
		Article.ModifiedDT.CellCssStyle = ""
		Article.ModifiedDT.CellCssClass = ""
		Article.ModifiedDT.CellAttrs.Clear(): Article.ModifiedDT.ViewAttrs.Clear(): Article.ModifiedDT.EditAttrs.Clear()

		'
		'  View  Row
		'

		If Article.RowType = EW_ROWTYPE_VIEW Then ' View row

			' ArticleID
			Article.ArticleID.ViewValue = Article.ArticleID.CurrentValue
			Article.ArticleID.CssStyle = ""
			Article.ArticleID.CssClass = ""
			Article.ArticleID.ViewCustomAttributes = ""

			' Active
			If Convert.ToString(Article.Active.CurrentValue) = "1" Then
				Article.Active.ViewValue = "Yes"
			Else
				Article.Active.ViewValue = "No"
			End If
			Article.Active.CssStyle = ""
			Article.Active.CssClass = ""
			Article.Active.ViewCustomAttributes = ""

			' StartDT
			Article.StartDT.ViewValue = Article.StartDT.CurrentValue
			Article.StartDT.CssStyle = ""
			Article.StartDT.CssClass = ""
			Article.StartDT.ViewCustomAttributes = ""

			' Title
			Article.Title.ViewValue = Article.Title.CurrentValue
			Article.Title.CssStyle = ""
			Article.Title.CssClass = ""
			Article.Title.ViewCustomAttributes = ""

			' CompanyID
			If ew_NotEmpty(Article.CompanyID.CurrentValue) Then
				sFilterWrk = "[CompanyID] = " & ew_AdjustSql(Article.CompanyID.CurrentValue) & ""
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
					Article.CompanyID.ViewValue = RsWrk("CompanyName")
				Else
					Article.CompanyID.ViewValue = Article.CompanyID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Article.CompanyID.ViewValue = System.DBNull.Value
			End If
			Article.CompanyID.CssStyle = ""
			Article.CompanyID.CssClass = ""
			Article.CompanyID.ViewCustomAttributes = ""

			' PageID
			If ew_NotEmpty(Article.zPageID.CurrentValue) Then
				sFilterWrk = "[PageID] = " & ew_AdjustSql(Article.zPageID.CurrentValue) & ""
			sSqlWrk = "SELECT [PageName] FROM [Page]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sWhereWrk &= " AND "
			sWhereWrk &= "(" & "[CompanyID]=" & HttpContext.Current.Session("CompanyID") & "" & ")"
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [PageName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Article.zPageID.ViewValue = RsWrk("PageName")
				Else
					Article.zPageID.ViewValue = Article.zPageID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Article.zPageID.ViewValue = System.DBNull.Value
			End If
			Article.zPageID.CssStyle = ""
			Article.zPageID.CssClass = ""
			Article.zPageID.ViewCustomAttributes = ""

			' userID
			If ew_NotEmpty(Article.userID.CurrentValue) Then
				sFilterWrk = "[ContactID] = " & ew_AdjustSql(Article.userID.CurrentValue) & ""
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
					Article.userID.ViewValue = RsWrk("PrimaryContact")
				Else
					Article.userID.ViewValue = Article.userID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Article.userID.ViewValue = System.DBNull.Value
			End If
			Article.userID.CssStyle = ""
			Article.userID.CssClass = ""
			Article.userID.ViewCustomAttributes = ""

			' Description
			Article.Description.ViewValue = Article.Description.CurrentValue
			Article.Description.CssStyle = ""
			Article.Description.CssClass = ""
			Article.Description.ViewCustomAttributes = ""

			' ArticleSummary
			Article.ArticleSummary.ViewValue = Article.ArticleSummary.CurrentValue
			Article.ArticleSummary.CssStyle = ""
			Article.ArticleSummary.CssClass = ""
			Article.ArticleSummary.ViewCustomAttributes = ""

			' ArticleBody
			Article.ArticleBody.ViewValue = Article.ArticleBody.CurrentValue
			Article.ArticleBody.CssStyle = ""
			Article.ArticleBody.CssClass = ""
			Article.ArticleBody.ViewCustomAttributes = ""

			' EndDT
			Article.EndDT.ViewValue = Article.EndDT.CurrentValue
			Article.EndDT.CssStyle = ""
			Article.EndDT.CssClass = ""
			Article.EndDT.ViewCustomAttributes = ""

			' ExpireDT
			Article.ExpireDT.ViewValue = Article.ExpireDT.CurrentValue
			Article.ExpireDT.CssStyle = ""
			Article.ExpireDT.CssClass = ""
			Article.ExpireDT.ViewCustomAttributes = ""

			' Author
			If ew_NotEmpty(Article.Author.CurrentValue) Then
				sFilterWrk = "[PrimaryContact] = '" & ew_AdjustSql(Article.Author.CurrentValue) & "'"
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
					Article.Author.ViewValue = RsWrk("PrimaryContact")
				Else
					Article.Author.ViewValue = Article.Author.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Article.Author.ViewValue = System.DBNull.Value
			End If
			Article.Author.CssStyle = ""
			Article.Author.CssClass = ""
			Article.Author.ViewCustomAttributes = ""

			' Counter
			Article.Counter.ViewValue = Article.Counter.CurrentValue
			Article.Counter.CssStyle = ""
			Article.Counter.CssClass = ""
			Article.Counter.ViewCustomAttributes = ""

			' VersionNo
			Article.VersionNo.ViewValue = Article.VersionNo.CurrentValue
			Article.VersionNo.CssStyle = ""
			Article.VersionNo.CssClass = ""
			Article.VersionNo.ViewCustomAttributes = ""

			' ContactID
			Article.ContactID.ViewValue = Article.ContactID.CurrentValue
			Article.ContactID.CssStyle = ""
			Article.ContactID.CssClass = ""
			Article.ContactID.ViewCustomAttributes = ""

			' ModifiedDT
			Article.ModifiedDT.ViewValue = Article.ModifiedDT.CurrentValue
			Article.ModifiedDT.CssStyle = ""
			Article.ModifiedDT.CssClass = ""
			Article.ModifiedDT.ViewCustomAttributes = ""

			' View refer script
			' ArticleID

			Article.ArticleID.HrefValue = ""
			Article.ArticleID.TooltipValue = ""

			' Active
			Article.Active.HrefValue = ""
			Article.Active.TooltipValue = ""

			' StartDT
			Article.StartDT.HrefValue = ""
			Article.StartDT.TooltipValue = ""

			' Title
			Article.Title.HrefValue = ""
			Article.Title.TooltipValue = ""

			' CompanyID
			Article.CompanyID.HrefValue = ""
			Article.CompanyID.TooltipValue = ""

			' PageID
			Article.zPageID.HrefValue = ""
			Article.zPageID.TooltipValue = ""

			' userID
			Article.userID.HrefValue = ""
			Article.userID.TooltipValue = ""

			' Description
			Article.Description.HrefValue = ""
			Article.Description.TooltipValue = ""

			' ArticleSummary
			Article.ArticleSummary.HrefValue = ""
			Article.ArticleSummary.TooltipValue = ""

			' ArticleBody
			Article.ArticleBody.HrefValue = ""
			Article.ArticleBody.TooltipValue = ""

			' EndDT
			Article.EndDT.HrefValue = ""
			Article.EndDT.TooltipValue = ""

			' ExpireDT
			Article.ExpireDT.HrefValue = ""
			Article.ExpireDT.TooltipValue = ""

			' Author
			Article.Author.HrefValue = ""
			Article.Author.TooltipValue = ""

			' Counter
			Article.Counter.HrefValue = ""
			Article.Counter.TooltipValue = ""

			' VersionNo
			Article.VersionNo.HrefValue = ""
			Article.VersionNo.TooltipValue = ""

			' ContactID
			Article.ContactID.HrefValue = ""
			Article.ContactID.TooltipValue = ""

			' ModifiedDT
			Article.ModifiedDT.HrefValue = ""
			Article.ModifiedDT.TooltipValue = ""

		'
		'  Search Row
		'

		ElseIf Article.RowType = EW_ROWTYPE_SEARCH Then ' Search row

			' ArticleID
			Article.ArticleID.EditCustomAttributes = ""
			Article.ArticleID.EditValue = ew_HtmlEncode(Article.ArticleID.AdvancedSearch.SearchValue)

			' Active
			Article.Active.EditCustomAttributes = ""

			' StartDT
			Article.StartDT.EditCustomAttributes = ""
			Article.StartDT.EditValue = Article.StartDT.AdvancedSearch.SearchValue

			' Title
			Article.Title.EditCustomAttributes = ""
			Article.Title.EditValue = ew_HtmlEncode(Article.Title.AdvancedSearch.SearchValue)

			' CompanyID
			Article.CompanyID.EditCustomAttributes = ""
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
			Article.CompanyID.EditValue = arwrk

			' PageID
			Article.zPageID.EditCustomAttributes = ""
			sFilterWrk = ""
			sSqlWrk = "SELECT [PageID], [PageName], '' AS Disp2Fld, [CompanyID] FROM [Page]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sWhereWrk &= " AND "
			sWhereWrk &= "(" & "[CompanyID]=" & HttpContext.Current.Session("CompanyID") & "" & ")"
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [PageName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", Language.Phrase("PleaseSelect"), ""}) 
			Article.zPageID.EditValue = arwrk

			' userID
			Article.userID.EditCustomAttributes = ""
			sFilterWrk = ""
			sSqlWrk = "SELECT [ContactID], [PrimaryContact], '' AS Disp2Fld, [CompanyID] FROM [Contact]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [PrimaryContact] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", Language.Phrase("PleaseSelect"), ""}) 
			Article.userID.EditValue = arwrk

			' Description
			Article.Description.EditCustomAttributes = ""
			Article.Description.EditValue = ew_HtmlEncode(Article.Description.AdvancedSearch.SearchValue)

			' ArticleSummary
			Article.ArticleSummary.EditCustomAttributes = ""
			Article.ArticleSummary.EditValue = ew_HtmlEncode(Article.ArticleSummary.AdvancedSearch.SearchValue)

			' ArticleBody
			Article.ArticleBody.EditCustomAttributes = ""
			Article.ArticleBody.EditValue = ew_HtmlEncode(Article.ArticleBody.AdvancedSearch.SearchValue)

			' EndDT
			Article.EndDT.EditCustomAttributes = ""
			Article.EndDT.EditValue = Article.EndDT.AdvancedSearch.SearchValue

			' ExpireDT
			Article.ExpireDT.EditCustomAttributes = ""
			Article.ExpireDT.EditValue = Article.ExpireDT.AdvancedSearch.SearchValue

			' Author
			Article.Author.EditCustomAttributes = ""
			sFilterWrk = ""
			sSqlWrk = "SELECT [PrimaryContact], [PrimaryContact], '' AS Disp2Fld, [CompanyID] FROM [Contact]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [PrimaryContact] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", Language.Phrase("PleaseSelect"), ""}) 
			Article.Author.EditValue = arwrk

			' Counter
			Article.Counter.EditCustomAttributes = ""
			Article.Counter.EditValue = ew_HtmlEncode(Article.Counter.AdvancedSearch.SearchValue)

			' VersionNo
			Article.VersionNo.EditCustomAttributes = ""
			Article.VersionNo.EditValue = ew_HtmlEncode(Article.VersionNo.AdvancedSearch.SearchValue)

			' ContactID
			Article.ContactID.EditCustomAttributes = ""
			Article.ContactID.EditValue = ew_HtmlEncode(Article.ContactID.AdvancedSearch.SearchValue)

			' ModifiedDT
			Article.ModifiedDT.EditCustomAttributes = ""
			Article.ModifiedDT.EditValue = Article.ModifiedDT.AdvancedSearch.SearchValue
		End If

		' Row Rendered event
		If Article.RowType <> EW_ROWTYPE_AGGREGATEINIT Then
			Article.Row_Rendered()
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
		If Not ew_CheckUSDate(Article.StartDT.AdvancedSearch.SearchValue) Then
			If ParentPage.gsSearchError <> "" Then ParentPage.gsSearchError = ParentPage.gsSearchError & "<br />"
			ParentPage.gsSearchError &= Article.StartDT.FldErrMsg
		End If
		If Not ew_CheckUSDate(Article.EndDT.AdvancedSearch.SearchValue) Then
			If ParentPage.gsSearchError <> "" Then ParentPage.gsSearchError = ParentPage.gsSearchError & "<br />"
			ParentPage.gsSearchError &= Article.EndDT.FldErrMsg
		End If
		If Not ew_CheckUSDate(Article.ExpireDT.AdvancedSearch.SearchValue) Then
			If ParentPage.gsSearchError <> "" Then ParentPage.gsSearchError = ParentPage.gsSearchError & "<br />"
			ParentPage.gsSearchError &= Article.ExpireDT.FldErrMsg
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
		Article.ArticleID.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_ArticleID")
		Article.Active.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_Active")
		Article.StartDT.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_StartDT")
		Article.Title.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_Title")
		Article.CompanyID.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_CompanyID")
		Article.zPageID.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_zPageID")
		Article.userID.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_userID")
		Article.Description.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_Description")
		Article.ArticleSummary.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_ArticleSummary")
		Article.ArticleBody.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_ArticleBody")
		Article.EndDT.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_EndDT")
		Article.ExpireDT.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_ExpireDT")
		Article.Author.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_Author")
		Article.Counter.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_Counter")
		Article.VersionNo.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_VersionNo")
		Article.ContactID.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_ContactID")
		Article.ModifiedDT.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_ModifiedDT")
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
		Article_search = New cArticle_search(Me)		
		Article_search.Page_Init()		

		' Set buffer/cache option
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

		' Page main processing
		Article_search.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If Article_search IsNot Nothing Then Article_search.Dispose()
	End Sub
End Class
