Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class Article_srch
	Inherits AspNetMaker7_WPMGen

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

		' Article
		Public Property Article() As cArticle
			Get				
				Return ParentPage.Article
			End Get
			Set(ByVal v As cArticle)
				ParentPage.Article = v	
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
			m_PageObjName = "Article_search"
			m_PageObjTypeName = "cArticle_search"

			' Table Name
			m_TableName = "Article"

			' Initialize table object
			Article = New cArticle(Me)

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
			Article.Dispose()

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
					Page_Terminate("Article_list.aspx" & "?" & sSrchStr) ' Go to list page
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
		BuildSearchUrl(sSrchUrl, Article.Active) ' Active
		BuildSearchUrl(sSrchUrl, Article.Title) ' Title
		BuildSearchUrl(sSrchUrl, Article.CompanyID) ' CompanyID
		BuildSearchUrl(sSrchUrl, Article.zPageID) ' PageID
		BuildSearchUrl(sSrchUrl, Article.ContactID) ' ContactID
		BuildSearchUrl(sSrchUrl, Article.Description) ' Description
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
		Article.Active.AdvancedSearch.SearchValue = ObjForm.GetValue("x_Active")
    	Article.Active.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_Active")
		Article.Title.AdvancedSearch.SearchValue = ObjForm.GetValue("x_Title")
    	Article.Title.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_Title")
		Article.CompanyID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_CompanyID")
    	Article.CompanyID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_CompanyID")
		Article.zPageID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_zPageID")
    	Article.zPageID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_zPageID")
		Article.ContactID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_ContactID")
    	Article.ContactID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_ContactID")
		Article.Description.AdvancedSearch.SearchValue = ObjForm.GetValue("x_Description")
    	Article.Description.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_Description")
		Article.ModifiedDT.AdvancedSearch.SearchValue = ObjForm.GetValue("x_ModifiedDT")
    	Article.ModifiedDT.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_ModifiedDT")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		Article.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' Active

		Article.Active.CellCssStyle = ""
		Article.Active.CellCssClass = ""

		' Title
		Article.Title.CellCssStyle = ""
		Article.Title.CellCssClass = ""

		' CompanyID
		Article.CompanyID.CellCssStyle = ""
		Article.CompanyID.CellCssClass = ""

		' PageID
		Article.zPageID.CellCssStyle = ""
		Article.zPageID.CellCssClass = ""

		' ContactID
		Article.ContactID.CellCssStyle = ""
		Article.ContactID.CellCssClass = ""

		' Description
		Article.Description.CellCssStyle = ""
		Article.Description.CellCssClass = ""

		' ModifiedDT
		Article.ModifiedDT.CellCssStyle = ""
		Article.ModifiedDT.CellCssClass = ""

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
			Article.StartDT.ViewValue = ew_FormatDateTime(Article.StartDT.ViewValue, 6)
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
				sSqlWrk = "SELECT [CompanyName] FROM [Company] WHERE [CompanyID] = " & ew_AdjustSql(Article.CompanyID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " AND (" & "[CompanyID]=" & httpcontext.current.session("CompanyID") & " " & ")"
				sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
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
				sSqlWrk = "SELECT [PageName] FROM [Page] WHERE [PageID] = " & ew_AdjustSql(Article.zPageID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [PageName] Asc"
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

			' ContactID
			If ew_NotEmpty(Article.ContactID.CurrentValue) Then
				sSqlWrk = "SELECT [PrimaryContact] FROM [Contact] WHERE [ContactID] = " & ew_AdjustSql(Article.ContactID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [PrimaryContact] "
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Article.ContactID.ViewValue = RsWrk("PrimaryContact")
				Else
					Article.ContactID.ViewValue = Article.ContactID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Article.ContactID.ViewValue = System.DBNull.Value
			End If
			Article.ContactID.CssStyle = ""
			Article.ContactID.CssClass = ""
			Article.ContactID.ViewCustomAttributes = ""

			' Description
			Article.Description.ViewValue = Article.Description.CurrentValue
			Article.Description.CssStyle = ""
			Article.Description.CssClass = ""
			Article.Description.ViewCustomAttributes = ""

			' ArticleBody
			Article.ArticleBody.ViewValue = Article.ArticleBody.CurrentValue
			Article.ArticleBody.CssStyle = ""
			Article.ArticleBody.CssClass = ""
			Article.ArticleBody.ViewCustomAttributes = ""

			' ModifiedDT
			Article.ModifiedDT.ViewValue = Article.ModifiedDT.CurrentValue
			Article.ModifiedDT.ViewValue = ew_FormatDateTime(Article.ModifiedDT.ViewValue, 6)
			Article.ModifiedDT.CssStyle = ""
			Article.ModifiedDT.CssClass = ""
			Article.ModifiedDT.ViewCustomAttributes = ""

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

			' userID
			Article.userID.ViewValue = Article.userID.CurrentValue
			Article.userID.CssStyle = ""
			Article.userID.CssClass = ""
			Article.userID.ViewCustomAttributes = ""

			' EndDT
			Article.EndDT.ViewValue = Article.EndDT.CurrentValue
			Article.EndDT.ViewValue = ew_FormatDateTime(Article.EndDT.ViewValue, 6)
			Article.EndDT.CssStyle = ""
			Article.EndDT.CssClass = ""
			Article.EndDT.ViewCustomAttributes = ""

			' ExpireDT
			Article.ExpireDT.ViewValue = Article.ExpireDT.CurrentValue
			Article.ExpireDT.ViewValue = ew_FormatDateTime(Article.ExpireDT.ViewValue, 6)
			Article.ExpireDT.CssStyle = ""
			Article.ExpireDT.CssClass = ""
			Article.ExpireDT.ViewCustomAttributes = ""

			' Author
			Article.Author.ViewValue = Article.Author.CurrentValue
			Article.Author.CssStyle = ""
			Article.Author.CssClass = ""
			Article.Author.ViewCustomAttributes = ""

			' View refer script
			' Active

			Article.Active.HrefValue = ""

			' Title
			Article.Title.HrefValue = ""

			' CompanyID
			Article.CompanyID.HrefValue = ""

			' PageID
			Article.zPageID.HrefValue = ""

			' ContactID
			Article.ContactID.HrefValue = ""

			' Description
			Article.Description.HrefValue = ""

			' ModifiedDT
			Article.ModifiedDT.HrefValue = ""

		'
		'  Search Row
		'

		ElseIf Article.RowType = EW_ROWTYPE_SEARCH Then ' Search row

			' Active
			Article.Active.EditCustomAttributes = ""

			' Title
			Article.Title.EditCustomAttributes = ""
			Article.Title.EditValue = ew_HtmlEncode(Article.Title.AdvancedSearch.SearchValue)

			' CompanyID
			Article.CompanyID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [CompanyID], [CompanyName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Company]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sWhereWrk = "(" & sWhereWrk & ") AND "
			sWhereWrk = sWhereWrk & "(" & "[CompanyID]=" & httpcontext.current.session("CompanyID") & " " & ")"
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			Article.CompanyID.EditValue = arwrk

			' PageID
			Article.zPageID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [PageID], [PageName], '' AS Disp2Fld, [CompanyID] FROM [Page]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [PageName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select", ""}) 
			Article.zPageID.EditValue = arwrk

			' ContactID
			Article.ContactID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [ContactID], [PrimaryContact], '' AS Disp2Fld, [CompanyID] FROM [Contact]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [PrimaryContact] "
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select", ""}) 
			Article.ContactID.EditValue = arwrk

			' Description
			Article.Description.EditCustomAttributes = ""
			Article.Description.EditValue = ew_HtmlEncode(Article.Description.AdvancedSearch.SearchValue)

			' ModifiedDT
			Article.ModifiedDT.EditCustomAttributes = ""
			Article.ModifiedDT.EditValue = Article.ModifiedDT.AdvancedSearch.SearchValue
		End If

		' Row Rendered event
		Article.Row_Rendered()
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
		Article.Active.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_Active")
		Article.Title.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_Title")
		Article.CompanyID.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_CompanyID")
		Article.zPageID.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_zPageID")
		Article.ContactID.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_ContactID")
		Article.Description.AdvancedSearch.SearchValue = Article.GetAdvancedSearch("x_Description")
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
		Article_search = New cArticle_search(Me)		
		Article_search.Page_Init()

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
