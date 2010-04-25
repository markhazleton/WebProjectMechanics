Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class LinkRank_srch
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public LinkRank_search As cLinkRank_search

	'
	' Page Class
	'
	Class cLinkRank_search
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
				If LinkRank.UseTokenInUrl Then Url = Url & "t=" & LinkRank.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If LinkRank.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (LinkRank.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (LinkRank.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' LinkRank
		Public Property LinkRank() As cLinkRank
			Get				
				Return ParentPage.LinkRank
			End Get
			Set(ByVal v As cLinkRank)
				ParentPage.LinkRank = v	
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
			m_PageObjName = "LinkRank_search"
			m_PageObjTypeName = "cLinkRank_search"

			' Table Name
			m_TableName = "LinkRank"

			' Initialize table object
			LinkRank = New cLinkRank(Me)

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
			LinkRank.Dispose()

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
			LinkRank.CurrentAction = ObjForm.GetValue("a_search")
			Select Case LinkRank.CurrentAction
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
						sSrchStr = LinkRank.UrlParm(sSrchStr)
					Page_Terminate("LinkRank_list.aspx" & "?" & sSrchStr) ' Go to list page
					End If
			End Select
		End If

		' Restore search settings from Session
		If ParentPage.gsSearchError = "" Then
			LoadAdvancedSearch()
		End If

		' Render row for search
		LinkRank.RowType = EW_ROWTYPE_SEARCH
		RenderRow()
	End Sub

	'
	' Build advanced search
	'
	Function BuildAdvancedSearch() As String
		Dim sSrchUrl As String = ""
		BuildSearchUrl(sSrchUrl, LinkRank.ID) ' ID
		BuildSearchUrl(sSrchUrl, LinkRank.LinkID) ' LinkID
		BuildSearchUrl(sSrchUrl, LinkRank.UserID) ' UserID
		BuildSearchUrl(sSrchUrl, LinkRank.RankNum) ' RankNum
		BuildSearchUrl(sSrchUrl, LinkRank.CateID) ' CateID
		BuildSearchUrl(sSrchUrl, LinkRank.Comment) ' Comment
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
		LinkRank.ID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_ID")
    	LinkRank.ID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_ID")
		LinkRank.LinkID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_LinkID")
    	LinkRank.LinkID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_LinkID")
		LinkRank.UserID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_UserID")
    	LinkRank.UserID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_UserID")
		LinkRank.RankNum.AdvancedSearch.SearchValue = ObjForm.GetValue("x_RankNum")
    	LinkRank.RankNum.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_RankNum")
		LinkRank.CateID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_CateID")
    	LinkRank.CateID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_CateID")
		LinkRank.Comment.AdvancedSearch.SearchValue = ObjForm.GetValue("x_Comment")
    	LinkRank.Comment.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_Comment")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		LinkRank.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' ID

		LinkRank.ID.CellCssStyle = ""
		LinkRank.ID.CellCssClass = ""

		' LinkID
		LinkRank.LinkID.CellCssStyle = ""
		LinkRank.LinkID.CellCssClass = ""

		' UserID
		LinkRank.UserID.CellCssStyle = ""
		LinkRank.UserID.CellCssClass = ""

		' RankNum
		LinkRank.RankNum.CellCssStyle = ""
		LinkRank.RankNum.CellCssClass = ""

		' CateID
		LinkRank.CateID.CellCssStyle = ""
		LinkRank.CateID.CellCssClass = ""

		' Comment
		LinkRank.Comment.CellCssStyle = ""
		LinkRank.Comment.CellCssClass = ""

		'
		'  View  Row
		'

		If LinkRank.RowType = EW_ROWTYPE_VIEW Then ' View row

			' ID
			LinkRank.ID.ViewValue = LinkRank.ID.CurrentValue
			LinkRank.ID.CssStyle = ""
			LinkRank.ID.CssClass = ""
			LinkRank.ID.ViewCustomAttributes = ""

			' LinkID
			LinkRank.LinkID.ViewValue = LinkRank.LinkID.CurrentValue
			LinkRank.LinkID.CssStyle = ""
			LinkRank.LinkID.CssClass = ""
			LinkRank.LinkID.ViewCustomAttributes = ""

			' UserID
			LinkRank.UserID.ViewValue = LinkRank.UserID.CurrentValue
			LinkRank.UserID.CssStyle = ""
			LinkRank.UserID.CssClass = ""
			LinkRank.UserID.ViewCustomAttributes = ""

			' RankNum
			LinkRank.RankNum.ViewValue = LinkRank.RankNum.CurrentValue
			LinkRank.RankNum.CssStyle = ""
			LinkRank.RankNum.CssClass = ""
			LinkRank.RankNum.ViewCustomAttributes = ""

			' CateID
			LinkRank.CateID.ViewValue = LinkRank.CateID.CurrentValue
			LinkRank.CateID.CssStyle = ""
			LinkRank.CateID.CssClass = ""
			LinkRank.CateID.ViewCustomAttributes = ""

			' Comment
			LinkRank.Comment.ViewValue = LinkRank.Comment.CurrentValue
			LinkRank.Comment.CssStyle = ""
			LinkRank.Comment.CssClass = ""
			LinkRank.Comment.ViewCustomAttributes = ""

			' View refer script
			' ID

			LinkRank.ID.HrefValue = ""

			' LinkID
			LinkRank.LinkID.HrefValue = ""

			' UserID
			LinkRank.UserID.HrefValue = ""

			' RankNum
			LinkRank.RankNum.HrefValue = ""

			' CateID
			LinkRank.CateID.HrefValue = ""

			' Comment
			LinkRank.Comment.HrefValue = ""

		'
		'  Search Row
		'

		ElseIf LinkRank.RowType = EW_ROWTYPE_SEARCH Then ' Search row

			' ID
			LinkRank.ID.EditCustomAttributes = ""
			LinkRank.ID.EditValue = ew_HtmlEncode(LinkRank.ID.AdvancedSearch.SearchValue)

			' LinkID
			LinkRank.LinkID.EditCustomAttributes = ""
			LinkRank.LinkID.EditValue = ew_HtmlEncode(LinkRank.LinkID.AdvancedSearch.SearchValue)

			' UserID
			LinkRank.UserID.EditCustomAttributes = ""
			LinkRank.UserID.EditValue = ew_HtmlEncode(LinkRank.UserID.AdvancedSearch.SearchValue)

			' RankNum
			LinkRank.RankNum.EditCustomAttributes = ""
			LinkRank.RankNum.EditValue = ew_HtmlEncode(LinkRank.RankNum.AdvancedSearch.SearchValue)

			' CateID
			LinkRank.CateID.EditCustomAttributes = ""
			LinkRank.CateID.EditValue = ew_HtmlEncode(LinkRank.CateID.AdvancedSearch.SearchValue)

			' Comment
			LinkRank.Comment.EditCustomAttributes = ""
			LinkRank.Comment.EditValue = ew_HtmlEncode(LinkRank.Comment.AdvancedSearch.SearchValue)
		End If

		' Row Rendered event
		LinkRank.Row_Rendered()
	End Sub

	'
	' Validate search
	'
	Function ValidateSearch() As Boolean

		' Initialize
		ParentPage.gsSearchError = ""

		' Check if validation required
		If Not EW_SERVER_VALIDATE Then Return True ' Skip
		If Not ew_CheckInteger(LinkRank.ID.AdvancedSearch.SearchValue) Then
			If ParentPage.gsSearchError <> "" Then ParentPage.gsSearchError = ParentPage.gsSearchError & "<br />"
			ParentPage.gsSearchError = ParentPage.gsSearchError & "Incorrect integer - ID"
		End If
		If Not ew_CheckInteger(LinkRank.LinkID.AdvancedSearch.SearchValue) Then
			If ParentPage.gsSearchError <> "" Then ParentPage.gsSearchError = ParentPage.gsSearchError & "<br />"
			ParentPage.gsSearchError = ParentPage.gsSearchError & "Incorrect integer - Link ID"
		End If
		If Not ew_CheckInteger(LinkRank.UserID.AdvancedSearch.SearchValue) Then
			If ParentPage.gsSearchError <> "" Then ParentPage.gsSearchError = ParentPage.gsSearchError & "<br />"
			ParentPage.gsSearchError = ParentPage.gsSearchError & "Incorrect integer - User ID"
		End If
		If Not ew_CheckInteger(LinkRank.RankNum.AdvancedSearch.SearchValue) Then
			If ParentPage.gsSearchError <> "" Then ParentPage.gsSearchError = ParentPage.gsSearchError & "<br />"
			ParentPage.gsSearchError = ParentPage.gsSearchError & "Incorrect integer - Rank Num"
		End If
		If Not ew_CheckInteger(LinkRank.CateID.AdvancedSearch.SearchValue) Then
			If ParentPage.gsSearchError <> "" Then ParentPage.gsSearchError = ParentPage.gsSearchError & "<br />"
			ParentPage.gsSearchError = ParentPage.gsSearchError & "Incorrect integer - Cate ID"
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
		LinkRank.ID.AdvancedSearch.SearchValue = LinkRank.GetAdvancedSearch("x_ID")
		LinkRank.LinkID.AdvancedSearch.SearchValue = LinkRank.GetAdvancedSearch("x_LinkID")
		LinkRank.UserID.AdvancedSearch.SearchValue = LinkRank.GetAdvancedSearch("x_UserID")
		LinkRank.RankNum.AdvancedSearch.SearchValue = LinkRank.GetAdvancedSearch("x_RankNum")
		LinkRank.CateID.AdvancedSearch.SearchValue = LinkRank.GetAdvancedSearch("x_CateID")
		LinkRank.Comment.AdvancedSearch.SearchValue = LinkRank.GetAdvancedSearch("x_Comment")
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
		LinkRank_search = New cLinkRank_search(Me)		
		LinkRank_search.Page_Init()

		' Page main processing
		LinkRank_search.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If LinkRank_search IsNot Nothing Then LinkRank_search.Dispose()
	End Sub
End Class
