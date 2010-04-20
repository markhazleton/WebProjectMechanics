Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class SiteCategoryType_srch
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public SiteCategoryType_search As cSiteCategoryType_search

	'
	' Page Class
	'
	Class cSiteCategoryType_search
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
				If SiteCategoryType.UseTokenInUrl Then Url = Url & "t=" & SiteCategoryType.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If SiteCategoryType.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (SiteCategoryType.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (SiteCategoryType.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' SiteCategoryType
		Public Property SiteCategoryType() As cSiteCategoryType
			Get				
				Return ParentPage.SiteCategoryType
			End Get
			Set(ByVal v As cSiteCategoryType)
				ParentPage.SiteCategoryType = v	
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
			m_PageObjName = "SiteCategoryType_search"
			m_PageObjTypeName = "cSiteCategoryType_search"

			' Table Name
			m_TableName = "SiteCategoryType"

			' Initialize table object
			SiteCategoryType = New cSiteCategoryType(Me)

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
			SiteCategoryType.Dispose()

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
			SiteCategoryType.CurrentAction = ObjForm.GetValue("a_search")
			Select Case SiteCategoryType.CurrentAction
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
						sSrchStr = SiteCategoryType.UrlParm(sSrchStr)
					Page_Terminate("SiteCategoryType_list.aspx" & "?" & sSrchStr) ' Go to list page
					End If
			End Select
		End If

		' Restore search settings from Session
		If ParentPage.gsSearchError = "" Then
			LoadAdvancedSearch()
		End If

		' Render row for search
		SiteCategoryType.RowType = EW_ROWTYPE_SEARCH
		RenderRow()
	End Sub

	'
	' Build advanced search
	'
	Function BuildAdvancedSearch() As String
		Dim sSrchUrl As String = ""
		BuildSearchUrl(sSrchUrl, SiteCategoryType.SiteCategoryTypeNM) ' SiteCategoryTypeNM
		BuildSearchUrl(sSrchUrl, SiteCategoryType.SiteCategoryTypeDS) ' SiteCategoryTypeDS
		BuildSearchUrl(sSrchUrl, SiteCategoryType.SiteCategoryComment) ' SiteCategoryComment
		BuildSearchUrl(sSrchUrl, SiteCategoryType.SiteCategoryFileName) ' SiteCategoryFileName
		BuildSearchUrl(sSrchUrl, SiteCategoryType.SiteCategoryTransferURL) ' SiteCategoryTransferURL
		BuildSearchUrl(sSrchUrl, SiteCategoryType.DefaultSiteCategoryID) ' DefaultSiteCategoryID
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
		SiteCategoryType.SiteCategoryTypeNM.AdvancedSearch.SearchValue = ObjForm.GetValue("x_SiteCategoryTypeNM")
    	SiteCategoryType.SiteCategoryTypeNM.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_SiteCategoryTypeNM")
		SiteCategoryType.SiteCategoryTypeDS.AdvancedSearch.SearchValue = ObjForm.GetValue("x_SiteCategoryTypeDS")
    	SiteCategoryType.SiteCategoryTypeDS.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_SiteCategoryTypeDS")
		SiteCategoryType.SiteCategoryComment.AdvancedSearch.SearchValue = ObjForm.GetValue("x_SiteCategoryComment")
    	SiteCategoryType.SiteCategoryComment.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_SiteCategoryComment")
		SiteCategoryType.SiteCategoryFileName.AdvancedSearch.SearchValue = ObjForm.GetValue("x_SiteCategoryFileName")
    	SiteCategoryType.SiteCategoryFileName.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_SiteCategoryFileName")
		SiteCategoryType.SiteCategoryTransferURL.AdvancedSearch.SearchValue = ObjForm.GetValue("x_SiteCategoryTransferURL")
    	SiteCategoryType.SiteCategoryTransferURL.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_SiteCategoryTransferURL")
		SiteCategoryType.DefaultSiteCategoryID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_DefaultSiteCategoryID")
    	SiteCategoryType.DefaultSiteCategoryID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_DefaultSiteCategoryID")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		SiteCategoryType.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' SiteCategoryTypeNM

		SiteCategoryType.SiteCategoryTypeNM.CellCssStyle = ""
		SiteCategoryType.SiteCategoryTypeNM.CellCssClass = ""

		' SiteCategoryTypeDS
		SiteCategoryType.SiteCategoryTypeDS.CellCssStyle = ""
		SiteCategoryType.SiteCategoryTypeDS.CellCssClass = ""

		' SiteCategoryComment
		SiteCategoryType.SiteCategoryComment.CellCssStyle = ""
		SiteCategoryType.SiteCategoryComment.CellCssClass = ""

		' SiteCategoryFileName
		SiteCategoryType.SiteCategoryFileName.CellCssStyle = ""
		SiteCategoryType.SiteCategoryFileName.CellCssClass = ""

		' SiteCategoryTransferURL
		SiteCategoryType.SiteCategoryTransferURL.CellCssStyle = ""
		SiteCategoryType.SiteCategoryTransferURL.CellCssClass = ""

		' DefaultSiteCategoryID
		SiteCategoryType.DefaultSiteCategoryID.CellCssStyle = ""
		SiteCategoryType.DefaultSiteCategoryID.CellCssClass = ""

		'
		'  View  Row
		'

		If SiteCategoryType.RowType = EW_ROWTYPE_VIEW Then ' View row

			' SiteCategoryTypeID
			SiteCategoryType.SiteCategoryTypeID.ViewValue = SiteCategoryType.SiteCategoryTypeID.CurrentValue
			SiteCategoryType.SiteCategoryTypeID.CssStyle = ""
			SiteCategoryType.SiteCategoryTypeID.CssClass = ""
			SiteCategoryType.SiteCategoryTypeID.ViewCustomAttributes = ""

			' SiteCategoryTypeNM
			SiteCategoryType.SiteCategoryTypeNM.ViewValue = SiteCategoryType.SiteCategoryTypeNM.CurrentValue
			SiteCategoryType.SiteCategoryTypeNM.CssStyle = ""
			SiteCategoryType.SiteCategoryTypeNM.CssClass = ""
			SiteCategoryType.SiteCategoryTypeNM.ViewCustomAttributes = ""

			' SiteCategoryTypeDS
			SiteCategoryType.SiteCategoryTypeDS.ViewValue = SiteCategoryType.SiteCategoryTypeDS.CurrentValue
			SiteCategoryType.SiteCategoryTypeDS.CssStyle = ""
			SiteCategoryType.SiteCategoryTypeDS.CssClass = ""
			SiteCategoryType.SiteCategoryTypeDS.ViewCustomAttributes = ""

			' SiteCategoryComment
			SiteCategoryType.SiteCategoryComment.ViewValue = SiteCategoryType.SiteCategoryComment.CurrentValue
			SiteCategoryType.SiteCategoryComment.CssStyle = ""
			SiteCategoryType.SiteCategoryComment.CssClass = ""
			SiteCategoryType.SiteCategoryComment.ViewCustomAttributes = ""

			' SiteCategoryFileName
			SiteCategoryType.SiteCategoryFileName.ViewValue = SiteCategoryType.SiteCategoryFileName.CurrentValue
			SiteCategoryType.SiteCategoryFileName.CssStyle = ""
			SiteCategoryType.SiteCategoryFileName.CssClass = ""
			SiteCategoryType.SiteCategoryFileName.ViewCustomAttributes = ""

			' SiteCategoryTransferURL
			SiteCategoryType.SiteCategoryTransferURL.ViewValue = SiteCategoryType.SiteCategoryTransferURL.CurrentValue
			SiteCategoryType.SiteCategoryTransferURL.CssStyle = ""
			SiteCategoryType.SiteCategoryTransferURL.CssClass = ""
			SiteCategoryType.SiteCategoryTransferURL.ViewCustomAttributes = ""

			' DefaultSiteCategoryID
			If ew_NotEmpty(SiteCategoryType.DefaultSiteCategoryID.CurrentValue) Then
				sSqlWrk = "SELECT [CategoryName], [SiteCategoryTypeID] FROM [SiteCategory] WHERE [SiteCategoryID] = " & ew_AdjustSql(SiteCategoryType.DefaultSiteCategoryID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryTypeID] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					SiteCategoryType.DefaultSiteCategoryID.ViewValue = RsWrk("CategoryName")
					SiteCategoryType.DefaultSiteCategoryID.ViewValue = SiteCategoryType.DefaultSiteCategoryID.ViewValue & ew_ValueSeparator(0) & RsWrk("SiteCategoryTypeID")
				Else
					SiteCategoryType.DefaultSiteCategoryID.ViewValue = SiteCategoryType.DefaultSiteCategoryID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				SiteCategoryType.DefaultSiteCategoryID.ViewValue = System.DBNull.Value
			End If
			SiteCategoryType.DefaultSiteCategoryID.CssStyle = ""
			SiteCategoryType.DefaultSiteCategoryID.CssClass = ""
			SiteCategoryType.DefaultSiteCategoryID.ViewCustomAttributes = ""

			' View refer script
			' SiteCategoryTypeNM

			SiteCategoryType.SiteCategoryTypeNM.HrefValue = ""

			' SiteCategoryTypeDS
			SiteCategoryType.SiteCategoryTypeDS.HrefValue = ""

			' SiteCategoryComment
			SiteCategoryType.SiteCategoryComment.HrefValue = ""

			' SiteCategoryFileName
			SiteCategoryType.SiteCategoryFileName.HrefValue = ""

			' SiteCategoryTransferURL
			SiteCategoryType.SiteCategoryTransferURL.HrefValue = ""

			' DefaultSiteCategoryID
			SiteCategoryType.DefaultSiteCategoryID.HrefValue = ""

		'
		'  Search Row
		'

		ElseIf SiteCategoryType.RowType = EW_ROWTYPE_SEARCH Then ' Search row

			' SiteCategoryTypeNM
			SiteCategoryType.SiteCategoryTypeNM.EditCustomAttributes = ""
			SiteCategoryType.SiteCategoryTypeNM.EditValue = ew_HtmlEncode(SiteCategoryType.SiteCategoryTypeNM.AdvancedSearch.SearchValue)

			' SiteCategoryTypeDS
			SiteCategoryType.SiteCategoryTypeDS.EditCustomAttributes = ""
			SiteCategoryType.SiteCategoryTypeDS.EditValue = ew_HtmlEncode(SiteCategoryType.SiteCategoryTypeDS.AdvancedSearch.SearchValue)

			' SiteCategoryComment
			SiteCategoryType.SiteCategoryComment.EditCustomAttributes = ""
			SiteCategoryType.SiteCategoryComment.EditValue = ew_HtmlEncode(SiteCategoryType.SiteCategoryComment.AdvancedSearch.SearchValue)

			' SiteCategoryFileName
			SiteCategoryType.SiteCategoryFileName.EditCustomAttributes = ""
			SiteCategoryType.SiteCategoryFileName.EditValue = ew_HtmlEncode(SiteCategoryType.SiteCategoryFileName.AdvancedSearch.SearchValue)

			' SiteCategoryTransferURL
			SiteCategoryType.SiteCategoryTransferURL.EditCustomAttributes = ""
			SiteCategoryType.SiteCategoryTransferURL.EditValue = ew_HtmlEncode(SiteCategoryType.SiteCategoryTransferURL.AdvancedSearch.SearchValue)

			' DefaultSiteCategoryID
			SiteCategoryType.DefaultSiteCategoryID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryID], [CategoryName], [SiteCategoryTypeID], '' AS SelectFilterFld FROM [SiteCategory]"
			If Convert.ToString(SiteCategoryType.DefaultSiteCategoryID.AdvancedSearch.SearchValue) = "" Then
				sWhereWrk = "0=1"
			Else
				sWhereWrk = "[SiteCategoryID] = " & ew_AdjustSql(SiteCategoryType.DefaultSiteCategoryID.AdvancedSearch.SearchValue) & ""
			End If
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryTypeID] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select", ""}) 
			SiteCategoryType.DefaultSiteCategoryID.EditValue = arwrk
		End If

		' Row Rendered event
		SiteCategoryType.Row_Rendered()
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
		SiteCategoryType.SiteCategoryTypeNM.AdvancedSearch.SearchValue = SiteCategoryType.GetAdvancedSearch("x_SiteCategoryTypeNM")
		SiteCategoryType.SiteCategoryTypeDS.AdvancedSearch.SearchValue = SiteCategoryType.GetAdvancedSearch("x_SiteCategoryTypeDS")
		SiteCategoryType.SiteCategoryComment.AdvancedSearch.SearchValue = SiteCategoryType.GetAdvancedSearch("x_SiteCategoryComment")
		SiteCategoryType.SiteCategoryFileName.AdvancedSearch.SearchValue = SiteCategoryType.GetAdvancedSearch("x_SiteCategoryFileName")
		SiteCategoryType.SiteCategoryTransferURL.AdvancedSearch.SearchValue = SiteCategoryType.GetAdvancedSearch("x_SiteCategoryTransferURL")
		SiteCategoryType.DefaultSiteCategoryID.AdvancedSearch.SearchValue = SiteCategoryType.GetAdvancedSearch("x_DefaultSiteCategoryID")
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
		SiteCategoryType_search = New cSiteCategoryType_search(Me)		
		SiteCategoryType_search.Page_Init()

		' Page main processing
		SiteCategoryType_search.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If SiteCategoryType_search IsNot Nothing Then SiteCategoryType_search.Dispose()
	End Sub
End Class
