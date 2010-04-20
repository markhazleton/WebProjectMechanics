Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class LinkCategory_srch
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public LinkCategory_search As cLinkCategory_search

	'
	' Page Class
	'
	Class cLinkCategory_search
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
				If LinkCategory.UseTokenInUrl Then Url = Url & "t=" & LinkCategory.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If LinkCategory.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (LinkCategory.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (LinkCategory.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' LinkCategory
		Public Property LinkCategory() As cLinkCategory
			Get				
				Return ParentPage.LinkCategory
			End Get
			Set(ByVal v As cLinkCategory)
				ParentPage.LinkCategory = v	
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
			m_PageObjName = "LinkCategory_search"
			m_PageObjTypeName = "cLinkCategory_search"

			' Table Name
			m_TableName = "LinkCategory"

			' Initialize table object
			LinkCategory = New cLinkCategory(Me)

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
			LinkCategory.Dispose()

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
			LinkCategory.CurrentAction = ObjForm.GetValue("a_search")
			Select Case LinkCategory.CurrentAction
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
						sSrchStr = LinkCategory.UrlParm(sSrchStr)
					Page_Terminate("LinkCategory_list.aspx" & "?" & sSrchStr) ' Go to list page
					End If
			End Select
		End If

		' Restore search settings from Session
		If ParentPage.gsSearchError = "" Then
			LoadAdvancedSearch()
		End If

		' Render row for search
		LinkCategory.RowType = EW_ROWTYPE_SEARCH
		RenderRow()
	End Sub

	'
	' Build advanced search
	'
	Function BuildAdvancedSearch() As String
		Dim sSrchUrl As String = ""
		BuildSearchUrl(sSrchUrl, LinkCategory.Title) ' Title
		BuildSearchUrl(sSrchUrl, LinkCategory.Description) ' Description
		BuildSearchUrl(sSrchUrl, LinkCategory.ParentID) ' ParentID
		BuildSearchUrl(sSrchUrl, LinkCategory.zPageID) ' PageID
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
		LinkCategory.Title.AdvancedSearch.SearchValue = ObjForm.GetValue("x_Title")
    	LinkCategory.Title.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_Title")
		LinkCategory.Description.AdvancedSearch.SearchValue = ObjForm.GetValue("x_Description")
    	LinkCategory.Description.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_Description")
		LinkCategory.ParentID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_ParentID")
    	LinkCategory.ParentID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_ParentID")
		LinkCategory.zPageID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_zPageID")
    	LinkCategory.zPageID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_zPageID")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		LinkCategory.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' Title

		LinkCategory.Title.CellCssStyle = ""
		LinkCategory.Title.CellCssClass = ""

		' Description
		LinkCategory.Description.CellCssStyle = ""
		LinkCategory.Description.CellCssClass = ""

		' ParentID
		LinkCategory.ParentID.CellCssStyle = ""
		LinkCategory.ParentID.CellCssClass = ""

		' PageID
		LinkCategory.zPageID.CellCssStyle = ""
		LinkCategory.zPageID.CellCssClass = ""

		'
		'  View  Row
		'

		If LinkCategory.RowType = EW_ROWTYPE_VIEW Then ' View row

			' ID
			LinkCategory.ID.ViewValue = LinkCategory.ID.CurrentValue
			LinkCategory.ID.CssStyle = ""
			LinkCategory.ID.CssClass = ""
			LinkCategory.ID.ViewCustomAttributes = ""

			' Title
			LinkCategory.Title.ViewValue = LinkCategory.Title.CurrentValue
			LinkCategory.Title.CssStyle = ""
			LinkCategory.Title.CssClass = ""
			LinkCategory.Title.ViewCustomAttributes = ""

			' Description
			LinkCategory.Description.ViewValue = LinkCategory.Description.CurrentValue
			LinkCategory.Description.CssStyle = ""
			LinkCategory.Description.CssClass = ""
			LinkCategory.Description.ViewCustomAttributes = ""

			' ParentID
			If ew_NotEmpty(LinkCategory.ParentID.CurrentValue) Then
				sSqlWrk = "SELECT [Title] FROM [LinkCategory] WHERE [ID] = " & ew_AdjustSql(LinkCategory.ParentID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [Title] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					LinkCategory.ParentID.ViewValue = RsWrk("Title")
				Else
					LinkCategory.ParentID.ViewValue = LinkCategory.ParentID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				LinkCategory.ParentID.ViewValue = System.DBNull.Value
			End If
			LinkCategory.ParentID.CssStyle = ""
			LinkCategory.ParentID.CssClass = ""
			LinkCategory.ParentID.ViewCustomAttributes = ""

			' PageID
			If ew_NotEmpty(LinkCategory.zPageID.CurrentValue) Then
				sSqlWrk = "SELECT [PageName] FROM [Page] WHERE [PageID] = " & ew_AdjustSql(LinkCategory.zPageID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [PageName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					LinkCategory.zPageID.ViewValue = RsWrk("PageName")
				Else
					LinkCategory.zPageID.ViewValue = LinkCategory.zPageID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				LinkCategory.zPageID.ViewValue = System.DBNull.Value
			End If
			LinkCategory.zPageID.CssStyle = ""
			LinkCategory.zPageID.CssClass = ""
			LinkCategory.zPageID.ViewCustomAttributes = ""

			' View refer script
			' Title

			LinkCategory.Title.HrefValue = ""

			' Description
			LinkCategory.Description.HrefValue = ""

			' ParentID
			LinkCategory.ParentID.HrefValue = ""

			' PageID
			LinkCategory.zPageID.HrefValue = ""

		'
		'  Search Row
		'

		ElseIf LinkCategory.RowType = EW_ROWTYPE_SEARCH Then ' Search row

			' Title
			LinkCategory.Title.EditCustomAttributes = ""
			LinkCategory.Title.EditValue = ew_HtmlEncode(LinkCategory.Title.AdvancedSearch.SearchValue)

			' Description
			LinkCategory.Description.EditCustomAttributes = ""
			LinkCategory.Description.EditValue = ew_HtmlEncode(LinkCategory.Description.AdvancedSearch.SearchValue)

			' ParentID
			LinkCategory.ParentID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [ID], [Title], '' AS Disp2Fld, '' AS SelectFilterFld FROM [LinkCategory]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [Title] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			LinkCategory.ParentID.EditValue = arwrk

			' PageID
			LinkCategory.zPageID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [PageID], [PageName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Page]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [PageName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			LinkCategory.zPageID.EditValue = arwrk
		End If

		' Row Rendered event
		LinkCategory.Row_Rendered()
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
		LinkCategory.Title.AdvancedSearch.SearchValue = LinkCategory.GetAdvancedSearch("x_Title")
		LinkCategory.Description.AdvancedSearch.SearchValue = LinkCategory.GetAdvancedSearch("x_Description")
		LinkCategory.ParentID.AdvancedSearch.SearchValue = LinkCategory.GetAdvancedSearch("x_ParentID")
		LinkCategory.zPageID.AdvancedSearch.SearchValue = LinkCategory.GetAdvancedSearch("x_zPageID")
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
		LinkCategory_search = New cLinkCategory_search(Me)		
		LinkCategory_search.Page_Init()

		' Page main processing
		LinkCategory_search.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If LinkCategory_search IsNot Nothing Then LinkCategory_search.Dispose()
	End Sub
End Class
