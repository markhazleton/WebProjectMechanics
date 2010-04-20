Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class Image_srch
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public Image_search As cImage_search

	'
	' Page Class
	'
	Class cImage_search
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
				If Image.UseTokenInUrl Then Url = Url & "t=" & Image.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If Image.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (Image.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (Image.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' Image
		Public Property Image() As cImage
			Get				
				Return ParentPage.Image
			End Get
			Set(ByVal v As cImage)
				ParentPage.Image = v	
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
			m_PageObjName = "Image_search"
			m_PageObjTypeName = "cImage_search"

			' Table Name
			m_TableName = "Image"

			' Initialize table object
			Image = New cImage(Me)

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
			Image.Dispose()

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
			Image.CurrentAction = ObjForm.GetValue("a_search")
			Select Case Image.CurrentAction
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
						sSrchStr = Image.UrlParm(sSrchStr)
					Page_Terminate("Image_list.aspx" & "?" & sSrchStr) ' Go to list page
					End If
			End Select
		End If

		' Restore search settings from Session
		If ParentPage.gsSearchError = "" Then
			LoadAdvancedSearch()
		End If

		' Render row for search
		Image.RowType = EW_ROWTYPE_SEARCH
		RenderRow()
	End Sub

	'
	' Build advanced search
	'
	Function BuildAdvancedSearch() As String
		Dim sSrchUrl As String = ""
		BuildSearchUrl(sSrchUrl, Image.CompanyID) ' CompanyID
		BuildSearchUrl(sSrchUrl, Image.title) ' title
		BuildSearchUrl(sSrchUrl, Image.ImageName) ' ImageName
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
		Image.CompanyID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_CompanyID")
    	Image.CompanyID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_CompanyID")
		Image.title.AdvancedSearch.SearchValue = ObjForm.GetValue("x_title")
    	Image.title.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_title")
		Image.ImageName.AdvancedSearch.SearchValue = ObjForm.GetValue("x_ImageName")
    	Image.ImageName.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_ImageName")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		Image.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' CompanyID

		Image.CompanyID.CellCssStyle = ""
		Image.CompanyID.CellCssClass = ""

		' title
		Image.title.CellCssStyle = ""
		Image.title.CellCssClass = ""

		' ImageName
		Image.ImageName.CellCssStyle = ""
		Image.ImageName.CellCssClass = ""

		'
		'  View  Row
		'

		If Image.RowType = EW_ROWTYPE_VIEW Then ' View row

			' CompanyID
			If ew_NotEmpty(Image.CompanyID.CurrentValue) Then
				sSqlWrk = "SELECT [CompanyName] FROM [Company] WHERE [CompanyID] = " & ew_AdjustSql(Image.CompanyID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] "
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Image.CompanyID.ViewValue = RsWrk("CompanyName")
				Else
					Image.CompanyID.ViewValue = Image.CompanyID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Image.CompanyID.ViewValue = System.DBNull.Value
			End If
			Image.CompanyID.CssStyle = ""
			Image.CompanyID.CssClass = ""
			Image.CompanyID.ViewCustomAttributes = ""

			' title
			Image.title.ViewValue = Image.title.CurrentValue
			Image.title.CssStyle = ""
			Image.title.CssClass = ""
			Image.title.ViewCustomAttributes = ""

			' ImageName
			Image.ImageName.ViewValue = Image.ImageName.CurrentValue
			Image.ImageName.CssStyle = ""
			Image.ImageName.CssClass = ""
			Image.ImageName.ViewCustomAttributes = ""

			' View refer script
			' CompanyID

			Image.CompanyID.HrefValue = ""

			' title
			Image.title.HrefValue = ""

			' ImageName
			Image.ImageName.HrefValue = ""

		'
		'  Search Row
		'

		ElseIf Image.RowType = EW_ROWTYPE_SEARCH Then ' Search row

			' CompanyID
			Image.CompanyID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [CompanyID], [CompanyName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Company]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] "
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			Image.CompanyID.EditValue = arwrk

			' title
			Image.title.EditCustomAttributes = ""
			Image.title.EditValue = ew_HtmlEncode(Image.title.AdvancedSearch.SearchValue)

			' ImageName
			Image.ImageName.EditCustomAttributes = ""
			Image.ImageName.EditValue = ew_HtmlEncode(Image.ImageName.AdvancedSearch.SearchValue)
		End If

		' Row Rendered event
		Image.Row_Rendered()
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
		Image.CompanyID.AdvancedSearch.SearchValue = Image.GetAdvancedSearch("x_CompanyID")
		Image.title.AdvancedSearch.SearchValue = Image.GetAdvancedSearch("x_title")
		Image.ImageName.AdvancedSearch.SearchValue = Image.GetAdvancedSearch("x_ImageName")
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
		Image_search = New cImage_search(Me)		
		Image_search.Page_Init()

		' Page main processing
		Image_search.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If Image_search IsNot Nothing Then Image_search.Dispose()
	End Sub
End Class
