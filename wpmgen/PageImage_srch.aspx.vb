Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class PageImage_srch
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public PageImage_search As cPageImage_search

	'
	' Page Class
	'
	Class cPageImage_search
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
				If PageImage.UseTokenInUrl Then Url = Url & "t=" & PageImage.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If PageImage.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (PageImage.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (PageImage.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' PageImage
		Public Property PageImage() As cPageImage
			Get				
				Return ParentPage.PageImage
			End Get
			Set(ByVal v As cPageImage)
				ParentPage.PageImage = v	
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
			m_PageObjName = "PageImage_search"
			m_PageObjTypeName = "cPageImage_search"

			' Table Name
			m_TableName = "PageImage"

			' Initialize table object
			PageImage = New cPageImage(Me)

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
			PageImage.Dispose()

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
			PageImage.CurrentAction = ObjForm.GetValue("a_search")
			Select Case PageImage.CurrentAction
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
						sSrchStr = PageImage.UrlParm(sSrchStr)
					Page_Terminate("PageImage_list.aspx" & "?" & sSrchStr) ' Go to list page
					End If
			End Select
		End If

		' Restore search settings from Session
		If ParentPage.gsSearchError = "" Then
			LoadAdvancedSearch()
		End If

		' Render row for search
		PageImage.RowType = EW_ROWTYPE_SEARCH
		RenderRow()
	End Sub

	'
	' Build advanced search
	'
	Function BuildAdvancedSearch() As String
		Dim sSrchUrl As String = ""
		BuildSearchUrl(sSrchUrl, PageImage.zPageID) ' PageID
		BuildSearchUrl(sSrchUrl, PageImage.ImageID) ' ImageID
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
		PageImage.zPageID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_zPageID")
    	PageImage.zPageID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_zPageID")
		PageImage.ImageID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_ImageID")
    	PageImage.ImageID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_ImageID")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		PageImage.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' PageID

		PageImage.zPageID.CellCssStyle = ""
		PageImage.zPageID.CellCssClass = ""

		' ImageID
		PageImage.ImageID.CellCssStyle = ""
		PageImage.ImageID.CellCssClass = ""

		'
		'  View  Row
		'

		If PageImage.RowType = EW_ROWTYPE_VIEW Then ' View row

			' PageID
			If ew_NotEmpty(PageImage.zPageID.CurrentValue) Then
				sSqlWrk = "SELECT [PageName] FROM [Page] WHERE [PageID] = " & ew_AdjustSql(PageImage.zPageID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " AND (" & "[CompanyID]=" & httpContext.Current.Session("CompanyID") & " " & ")"
				sSqlWrk = sSqlWrk & " ORDER BY [PageName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					PageImage.zPageID.ViewValue = RsWrk("PageName")
				Else
					PageImage.zPageID.ViewValue = PageImage.zPageID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				PageImage.zPageID.ViewValue = System.DBNull.Value
			End If
			PageImage.zPageID.CssStyle = ""
			PageImage.zPageID.CssClass = ""
			PageImage.zPageID.ViewCustomAttributes = ""

			' ImageID
			If ew_NotEmpty(PageImage.ImageID.CurrentValue) Then
				sSqlWrk = "SELECT [ImageName] FROM [Image] WHERE [ImageID] = " & ew_AdjustSql(PageImage.ImageID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " AND (" & "[CompanyID]=" & httpContext.Current.Session("CompanyID") & " " & ")"
				sSqlWrk = sSqlWrk & " ORDER BY [ImageName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					PageImage.ImageID.ViewValue = RsWrk("ImageName")
				Else
					PageImage.ImageID.ViewValue = PageImage.ImageID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				PageImage.ImageID.ViewValue = System.DBNull.Value
			End If
			PageImage.ImageID.CssStyle = ""
			PageImage.ImageID.CssClass = ""
			PageImage.ImageID.ViewCustomAttributes = ""

			' PageImagePosition
			PageImage.PageImagePosition.ViewValue = PageImage.PageImagePosition.CurrentValue
			PageImage.PageImagePosition.CssStyle = ""
			PageImage.PageImagePosition.CssClass = ""
			PageImage.PageImagePosition.ViewCustomAttributes = ""

			' View refer script
			' PageID

			PageImage.zPageID.HrefValue = ""

			' ImageID
			PageImage.ImageID.HrefValue = ""

		'
		'  Search Row
		'

		ElseIf PageImage.RowType = EW_ROWTYPE_SEARCH Then ' Search row

			' PageID
			PageImage.zPageID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [PageID], [PageName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Page]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sWhereWrk = "(" & sWhereWrk & ") AND "
			sWhereWrk = sWhereWrk & "(" & "[CompanyID]=" & httpContext.Current.Session("CompanyID") & " " & ")"
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [PageName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			PageImage.zPageID.EditValue = arwrk

			' ImageID
			PageImage.ImageID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [ImageID], [ImageName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Image]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sWhereWrk = "(" & sWhereWrk & ") AND "
			sWhereWrk = sWhereWrk & "(" & "[CompanyID]=" & httpContext.Current.Session("CompanyID") & " " & ")"
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [ImageName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			PageImage.ImageID.EditValue = arwrk
		End If

		' Row Rendered event
		PageImage.Row_Rendered()
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
		PageImage.zPageID.AdvancedSearch.SearchValue = PageImage.GetAdvancedSearch("x_zPageID")
		PageImage.ImageID.AdvancedSearch.SearchValue = PageImage.GetAdvancedSearch("x_ImageID")
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
		PageImage_search = New cPageImage_search(Me)		
		PageImage_search.Page_Init()

		' Page main processing
		PageImage_search.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If PageImage_search IsNot Nothing Then PageImage_search.Dispose()
	End Sub
End Class
