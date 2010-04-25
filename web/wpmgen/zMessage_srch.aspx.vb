Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class zMessage_srch
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public zMessage_search As czMessage_search

	'
	' Page Class
	'
	Class czMessage_search
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
				If zMessage.UseTokenInUrl Then Url = Url & "t=" & zMessage.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If zMessage.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (zMessage.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (zMessage.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' zMessage
		Public Property zMessage() As czMessage
			Get				
				Return ParentPage.zMessage
			End Get
			Set(ByVal v As czMessage)
				ParentPage.zMessage = v	
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
			m_PageObjName = "zMessage_search"
			m_PageObjTypeName = "czMessage_search"

			' Table Name
			m_TableName = "Message"

			' Initialize table object
			zMessage = New czMessage(Me)

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
			zMessage.Dispose()

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
			zMessage.CurrentAction = ObjForm.GetValue("a_search")
			Select Case zMessage.CurrentAction
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
						sSrchStr = zMessage.UrlParm(sSrchStr)
					Page_Terminate("zMessage_list.aspx" & "?" & sSrchStr) ' Go to list page
					End If
			End Select
		End If

		' Restore search settings from Session
		If ParentPage.gsSearchError = "" Then
			LoadAdvancedSearch()
		End If

		' Render row for search
		zMessage.RowType = EW_ROWTYPE_SEARCH
		RenderRow()
	End Sub

	'
	' Build advanced search
	'
	Function BuildAdvancedSearch() As String
		Dim sSrchUrl As String = ""
		BuildSearchUrl(sSrchUrl, zMessage.MessageID) ' MessageID
		BuildSearchUrl(sSrchUrl, zMessage.zPageID) ' PageID
		BuildSearchUrl(sSrchUrl, zMessage.ParentMessageID) ' ParentMessageID
		BuildSearchUrl(sSrchUrl, zMessage.Subject) ' Subject
		BuildSearchUrl(sSrchUrl, zMessage.Author) ' Author
		BuildSearchUrl(sSrchUrl, zMessage.zEmail) ' Email
		BuildSearchUrl(sSrchUrl, zMessage.City) ' City
		BuildSearchUrl(sSrchUrl, zMessage.URL) ' URL
		BuildSearchUrl(sSrchUrl, zMessage.MessageDate) ' MessageDate
		BuildSearchUrl(sSrchUrl, zMessage.Body) ' Body
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
		zMessage.MessageID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_MessageID")
    	zMessage.MessageID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_MessageID")
		zMessage.zPageID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_zPageID")
    	zMessage.zPageID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_zPageID")
		zMessage.ParentMessageID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_ParentMessageID")
    	zMessage.ParentMessageID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_ParentMessageID")
		zMessage.Subject.AdvancedSearch.SearchValue = ObjForm.GetValue("x_Subject")
    	zMessage.Subject.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_Subject")
		zMessage.Author.AdvancedSearch.SearchValue = ObjForm.GetValue("x_Author")
    	zMessage.Author.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_Author")
		zMessage.zEmail.AdvancedSearch.SearchValue = ObjForm.GetValue("x_zEmail")
    	zMessage.zEmail.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_zEmail")
		zMessage.City.AdvancedSearch.SearchValue = ObjForm.GetValue("x_City")
    	zMessage.City.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_City")
		zMessage.URL.AdvancedSearch.SearchValue = ObjForm.GetValue("x_URL")
    	zMessage.URL.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_URL")
		zMessage.MessageDate.AdvancedSearch.SearchValue = ObjForm.GetValue("x_MessageDate")
    	zMessage.MessageDate.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_MessageDate")
		zMessage.Body.AdvancedSearch.SearchValue = ObjForm.GetValue("x_Body")
    	zMessage.Body.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_Body")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		zMessage.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' MessageID

		zMessage.MessageID.CellCssStyle = ""
		zMessage.MessageID.CellCssClass = ""

		' PageID
		zMessage.zPageID.CellCssStyle = ""
		zMessage.zPageID.CellCssClass = ""

		' ParentMessageID
		zMessage.ParentMessageID.CellCssStyle = ""
		zMessage.ParentMessageID.CellCssClass = ""

		' Subject
		zMessage.Subject.CellCssStyle = ""
		zMessage.Subject.CellCssClass = ""

		' Author
		zMessage.Author.CellCssStyle = ""
		zMessage.Author.CellCssClass = ""

		' Email
		zMessage.zEmail.CellCssStyle = ""
		zMessage.zEmail.CellCssClass = ""

		' City
		zMessage.City.CellCssStyle = ""
		zMessage.City.CellCssClass = ""

		' URL
		zMessage.URL.CellCssStyle = ""
		zMessage.URL.CellCssClass = ""

		' MessageDate
		zMessage.MessageDate.CellCssStyle = ""
		zMessage.MessageDate.CellCssClass = ""

		' Body
		zMessage.Body.CellCssStyle = ""
		zMessage.Body.CellCssClass = ""

		'
		'  View  Row
		'

		If zMessage.RowType = EW_ROWTYPE_VIEW Then ' View row

			' MessageID
			zMessage.MessageID.ViewValue = zMessage.MessageID.CurrentValue
			zMessage.MessageID.CssStyle = ""
			zMessage.MessageID.CssClass = ""
			zMessage.MessageID.ViewCustomAttributes = ""

			' PageID
			zMessage.zPageID.ViewValue = zMessage.zPageID.CurrentValue
			zMessage.zPageID.CssStyle = ""
			zMessage.zPageID.CssClass = ""
			zMessage.zPageID.ViewCustomAttributes = ""

			' ParentMessageID
			zMessage.ParentMessageID.ViewValue = zMessage.ParentMessageID.CurrentValue
			zMessage.ParentMessageID.CssStyle = ""
			zMessage.ParentMessageID.CssClass = ""
			zMessage.ParentMessageID.ViewCustomAttributes = ""

			' Subject
			zMessage.Subject.ViewValue = zMessage.Subject.CurrentValue
			zMessage.Subject.CssStyle = ""
			zMessage.Subject.CssClass = ""
			zMessage.Subject.ViewCustomAttributes = ""

			' Author
			zMessage.Author.ViewValue = zMessage.Author.CurrentValue
			zMessage.Author.CssStyle = ""
			zMessage.Author.CssClass = ""
			zMessage.Author.ViewCustomAttributes = ""

			' Email
			zMessage.zEmail.ViewValue = zMessage.zEmail.CurrentValue
			zMessage.zEmail.CssStyle = ""
			zMessage.zEmail.CssClass = ""
			zMessage.zEmail.ViewCustomAttributes = ""

			' City
			zMessage.City.ViewValue = zMessage.City.CurrentValue
			zMessage.City.CssStyle = ""
			zMessage.City.CssClass = ""
			zMessage.City.ViewCustomAttributes = ""

			' URL
			zMessage.URL.ViewValue = zMessage.URL.CurrentValue
			zMessage.URL.CssStyle = ""
			zMessage.URL.CssClass = ""
			zMessage.URL.ViewCustomAttributes = ""

			' MessageDate
			zMessage.MessageDate.ViewValue = zMessage.MessageDate.CurrentValue
			zMessage.MessageDate.ViewValue = ew_FormatDateTime(zMessage.MessageDate.ViewValue, 6)
			zMessage.MessageDate.CssStyle = ""
			zMessage.MessageDate.CssClass = ""
			zMessage.MessageDate.ViewCustomAttributes = ""

			' Body
			zMessage.Body.ViewValue = zMessage.Body.CurrentValue
			zMessage.Body.CssStyle = ""
			zMessage.Body.CssClass = ""
			zMessage.Body.ViewCustomAttributes = ""

			' View refer script
			' MessageID

			zMessage.MessageID.HrefValue = ""

			' PageID
			zMessage.zPageID.HrefValue = ""

			' ParentMessageID
			zMessage.ParentMessageID.HrefValue = ""

			' Subject
			zMessage.Subject.HrefValue = ""

			' Author
			zMessage.Author.HrefValue = ""

			' Email
			zMessage.zEmail.HrefValue = ""

			' City
			zMessage.City.HrefValue = ""

			' URL
			zMessage.URL.HrefValue = ""

			' MessageDate
			zMessage.MessageDate.HrefValue = ""

			' Body
			zMessage.Body.HrefValue = ""

		'
		'  Search Row
		'

		ElseIf zMessage.RowType = EW_ROWTYPE_SEARCH Then ' Search row

			' MessageID
			zMessage.MessageID.EditCustomAttributes = ""
			zMessage.MessageID.EditValue = ew_HtmlEncode(zMessage.MessageID.AdvancedSearch.SearchValue)

			' PageID
			zMessage.zPageID.EditCustomAttributes = ""
			zMessage.zPageID.EditValue = ew_HtmlEncode(zMessage.zPageID.AdvancedSearch.SearchValue)

			' ParentMessageID
			zMessage.ParentMessageID.EditCustomAttributes = ""
			zMessage.ParentMessageID.EditValue = ew_HtmlEncode(zMessage.ParentMessageID.AdvancedSearch.SearchValue)

			' Subject
			zMessage.Subject.EditCustomAttributes = ""
			zMessage.Subject.EditValue = ew_HtmlEncode(zMessage.Subject.AdvancedSearch.SearchValue)

			' Author
			zMessage.Author.EditCustomAttributes = ""
			zMessage.Author.EditValue = ew_HtmlEncode(zMessage.Author.AdvancedSearch.SearchValue)

			' Email
			zMessage.zEmail.EditCustomAttributes = ""
			zMessage.zEmail.EditValue = ew_HtmlEncode(zMessage.zEmail.AdvancedSearch.SearchValue)

			' City
			zMessage.City.EditCustomAttributes = ""
			zMessage.City.EditValue = ew_HtmlEncode(zMessage.City.AdvancedSearch.SearchValue)

			' URL
			zMessage.URL.EditCustomAttributes = ""
			zMessage.URL.EditValue = ew_HtmlEncode(zMessage.URL.AdvancedSearch.SearchValue)

			' MessageDate
			zMessage.MessageDate.EditCustomAttributes = ""
			zMessage.MessageDate.EditValue = zMessage.MessageDate.AdvancedSearch.SearchValue

			' Body
			zMessage.Body.EditCustomAttributes = ""
			zMessage.Body.EditValue = ew_HtmlEncode(zMessage.Body.AdvancedSearch.SearchValue)
		End If

		' Row Rendered event
		zMessage.Row_Rendered()
	End Sub

	'
	' Validate search
	'
	Function ValidateSearch() As Boolean

		' Initialize
		ParentPage.gsSearchError = ""

		' Check if validation required
		If Not EW_SERVER_VALIDATE Then Return True ' Skip
		If Not ew_CheckInteger(zMessage.MessageID.AdvancedSearch.SearchValue) Then
			If ParentPage.gsSearchError <> "" Then ParentPage.gsSearchError = ParentPage.gsSearchError & "<br />"
			ParentPage.gsSearchError = ParentPage.gsSearchError & "Incorrect integer - Message ID"
		End If
		If Not ew_CheckInteger(zMessage.zPageID.AdvancedSearch.SearchValue) Then
			If ParentPage.gsSearchError <> "" Then ParentPage.gsSearchError = ParentPage.gsSearchError & "<br />"
			ParentPage.gsSearchError = ParentPage.gsSearchError & "Incorrect integer - Page ID"
		End If
		If Not ew_CheckInteger(zMessage.ParentMessageID.AdvancedSearch.SearchValue) Then
			If ParentPage.gsSearchError <> "" Then ParentPage.gsSearchError = ParentPage.gsSearchError & "<br />"
			ParentPage.gsSearchError = ParentPage.gsSearchError & "Incorrect integer - Parent Message ID"
		End If
		If Not ew_CheckUSDate(zMessage.MessageDate.AdvancedSearch.SearchValue) Then
			If ParentPage.gsSearchError <> "" Then ParentPage.gsSearchError = ParentPage.gsSearchError & "<br />"
			ParentPage.gsSearchError = ParentPage.gsSearchError & "Incorrect date, format = mm/dd/yyyy - Message Date"
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
		zMessage.MessageID.AdvancedSearch.SearchValue = zMessage.GetAdvancedSearch("x_MessageID")
		zMessage.zPageID.AdvancedSearch.SearchValue = zMessage.GetAdvancedSearch("x_zPageID")
		zMessage.ParentMessageID.AdvancedSearch.SearchValue = zMessage.GetAdvancedSearch("x_ParentMessageID")
		zMessage.Subject.AdvancedSearch.SearchValue = zMessage.GetAdvancedSearch("x_Subject")
		zMessage.Author.AdvancedSearch.SearchValue = zMessage.GetAdvancedSearch("x_Author")
		zMessage.zEmail.AdvancedSearch.SearchValue = zMessage.GetAdvancedSearch("x_zEmail")
		zMessage.City.AdvancedSearch.SearchValue = zMessage.GetAdvancedSearch("x_City")
		zMessage.URL.AdvancedSearch.SearchValue = zMessage.GetAdvancedSearch("x_URL")
		zMessage.MessageDate.AdvancedSearch.SearchValue = zMessage.GetAdvancedSearch("x_MessageDate")
		zMessage.Body.AdvancedSearch.SearchValue = zMessage.GetAdvancedSearch("x_Body")
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
		zMessage_search = New czMessage_search(Me)		
		zMessage_search.Page_Init()

		' Page main processing
		zMessage_search.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If zMessage_search IsNot Nothing Then zMessage_search.Dispose()
	End Sub
End Class
