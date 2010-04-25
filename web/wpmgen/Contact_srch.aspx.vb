Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class Contact_srch
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public Contact_search As cContact_search

	'
	' Page Class
	'
	Class cContact_search
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
				If Contact.UseTokenInUrl Then Url = Url & "t=" & Contact.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If Contact.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (Contact.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (Contact.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' Contact
		Public Property Contact() As cContact
			Get				
				Return ParentPage.Contact
			End Get
			Set(ByVal v As cContact)
				ParentPage.Contact = v	
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
			m_PageObjName = "Contact_search"
			m_PageObjTypeName = "cContact_search"

			' Table Name
			m_TableName = "Contact"

			' Initialize table object
			Contact = New cContact(Me)

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
			Contact.Dispose()

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
			Contact.CurrentAction = ObjForm.GetValue("a_search")
			Select Case Contact.CurrentAction
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
						sSrchStr = Contact.UrlParm(sSrchStr)
					Page_Terminate("Contact_list.aspx" & "?" & sSrchStr) ' Go to list page
					End If
			End Select
		End If

		' Restore search settings from Session
		If ParentPage.gsSearchError = "" Then
			LoadAdvancedSearch()
		End If

		' Render row for search
		Contact.RowType = EW_ROWTYPE_SEARCH
		RenderRow()
	End Sub

	'
	' Build advanced search
	'
	Function BuildAdvancedSearch() As String
		Dim sSrchUrl As String = ""
		BuildSearchUrl(sSrchUrl, Contact.LogonName) ' LogonName
		BuildSearchUrl(sSrchUrl, Contact.PrimaryContact) ' PrimaryContact
		BuildSearchUrl(sSrchUrl, Contact.zEMail) ' EMail
		BuildSearchUrl(sSrchUrl, Contact.Active) ' Active
		BuildSearchUrl(sSrchUrl, Contact.CompanyID) ' CompanyID
		BuildSearchUrl(sSrchUrl, Contact.GroupID) ' GroupID
		BuildSearchUrl(sSrchUrl, Contact.TemplatePrefix) ' TemplatePrefix
		BuildSearchUrl(sSrchUrl, Contact.RoleID) ' RoleID
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
		Contact.LogonName.AdvancedSearch.SearchValue = ObjForm.GetValue("x_LogonName")
    	Contact.LogonName.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_LogonName")
		Contact.PrimaryContact.AdvancedSearch.SearchValue = ObjForm.GetValue("x_PrimaryContact")
    	Contact.PrimaryContact.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_PrimaryContact")
		Contact.zEMail.AdvancedSearch.SearchValue = ObjForm.GetValue("x_zEMail")
    	Contact.zEMail.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_zEMail")
		Contact.Active.AdvancedSearch.SearchValue = ObjForm.GetValue("x_Active")
    	Contact.Active.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_Active")
		Contact.CompanyID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_CompanyID")
    	Contact.CompanyID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_CompanyID")
		Contact.GroupID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_GroupID")
    	Contact.GroupID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_GroupID")
		Contact.TemplatePrefix.AdvancedSearch.SearchValue = ObjForm.GetValue("x_TemplatePrefix")
    	Contact.TemplatePrefix.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_TemplatePrefix")
		Contact.RoleID.AdvancedSearch.SearchValue = ObjForm.GetValue("x_RoleID")
    	Contact.RoleID.AdvancedSearch.SearchOperator = ObjForm.GetValue("z_RoleID")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		Contact.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' LogonName

		Contact.LogonName.CellCssStyle = ""
		Contact.LogonName.CellCssClass = ""

		' PrimaryContact
		Contact.PrimaryContact.CellCssStyle = ""
		Contact.PrimaryContact.CellCssClass = ""

		' EMail
		Contact.zEMail.CellCssStyle = ""
		Contact.zEMail.CellCssClass = ""

		' Active
		Contact.Active.CellCssStyle = ""
		Contact.Active.CellCssClass = ""

		' CompanyID
		Contact.CompanyID.CellCssStyle = ""
		Contact.CompanyID.CellCssClass = ""

		' GroupID
		Contact.GroupID.CellCssStyle = ""
		Contact.GroupID.CellCssClass = ""

		' TemplatePrefix
		Contact.TemplatePrefix.CellCssStyle = ""
		Contact.TemplatePrefix.CellCssClass = ""

		' RoleID
		Contact.RoleID.CellCssStyle = ""
		Contact.RoleID.CellCssClass = ""

		'
		'  View  Row
		'

		If Contact.RowType = EW_ROWTYPE_VIEW Then ' View row

			' LogonName
			Contact.LogonName.ViewValue = Contact.LogonName.CurrentValue
			Contact.LogonName.CssStyle = ""
			Contact.LogonName.CssClass = ""
			Contact.LogonName.ViewCustomAttributes = ""

			' LogonPassword
			Contact.LogonPassword.ViewValue = "********"
			Contact.LogonPassword.CssStyle = ""
			Contact.LogonPassword.CssClass = ""
			Contact.LogonPassword.ViewCustomAttributes = ""

			' PrimaryContact
			Contact.PrimaryContact.ViewValue = Contact.PrimaryContact.CurrentValue
			Contact.PrimaryContact.CssStyle = ""
			Contact.PrimaryContact.CssClass = ""
			Contact.PrimaryContact.ViewCustomAttributes = ""

			' EMail
			Contact.zEMail.ViewValue = Contact.zEMail.CurrentValue
			Contact.zEMail.CssStyle = ""
			Contact.zEMail.CssClass = ""
			Contact.zEMail.ViewCustomAttributes = ""

			' Active
			If Convert.ToString(Contact.Active.CurrentValue) = "1" Then
				Contact.Active.ViewValue = "Yes"
			Else
				Contact.Active.ViewValue = "No"
			End If
			Contact.Active.CssStyle = ""
			Contact.Active.CssClass = ""
			Contact.Active.ViewCustomAttributes = ""

			' CompanyID
			If ew_NotEmpty(Contact.CompanyID.CurrentValue) Then
				sSqlWrk = "SELECT [CompanyName] FROM [Company] WHERE [CompanyID] = " & ew_AdjustSql(Contact.CompanyID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Contact.CompanyID.ViewValue = RsWrk("CompanyName")
				Else
					Contact.CompanyID.ViewValue = Contact.CompanyID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Contact.CompanyID.ViewValue = System.DBNull.Value
			End If
			Contact.CompanyID.CssStyle = ""
			Contact.CompanyID.CssClass = ""
			Contact.CompanyID.ViewCustomAttributes = ""

			' GroupID
			If ew_NotEmpty(Contact.GroupID.CurrentValue) Then
				sSqlWrk = "SELECT [GroupName] FROM [Group] WHERE [GroupID] = " & ew_AdjustSql(Contact.GroupID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [GroupID] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Contact.GroupID.ViewValue = RsWrk("GroupName")
				Else
					Contact.GroupID.ViewValue = Contact.GroupID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Contact.GroupID.ViewValue = System.DBNull.Value
			End If
			Contact.GroupID.CssStyle = ""
			Contact.GroupID.CssClass = ""
			Contact.GroupID.ViewCustomAttributes = ""

			' TemplatePrefix
			If ew_NotEmpty(Contact.TemplatePrefix.CurrentValue) Then
				sSqlWrk = "SELECT [Name] FROM [SiteTemplate] WHERE [TemplatePrefix] = '" & ew_AdjustSql(Contact.TemplatePrefix.CurrentValue) & "'"
				sSqlWrk = sSqlWrk & " ORDER BY [Name] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Contact.TemplatePrefix.ViewValue = RsWrk("Name")
				Else
					Contact.TemplatePrefix.ViewValue = Contact.TemplatePrefix.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Contact.TemplatePrefix.ViewValue = System.DBNull.Value
			End If
			Contact.TemplatePrefix.CssStyle = ""
			Contact.TemplatePrefix.CssClass = ""
			Contact.TemplatePrefix.ViewCustomAttributes = ""

			' RoleID
			If ew_NotEmpty(Contact.RoleID.CurrentValue) Then
				sSqlWrk = "SELECT [RoleName] FROM [role] WHERE [RoleID] = " & ew_AdjustSql(Contact.RoleID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [RoleName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Contact.RoleID.ViewValue = RsWrk("RoleName")
				Else
					Contact.RoleID.ViewValue = Contact.RoleID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Contact.RoleID.ViewValue = System.DBNull.Value
			End If
			Contact.RoleID.CssStyle = ""
			Contact.RoleID.CssClass = ""
			Contact.RoleID.ViewCustomAttributes = ""

			' View refer script
			' LogonName

			Contact.LogonName.HrefValue = ""

			' PrimaryContact
			Contact.PrimaryContact.HrefValue = ""

			' EMail
			Contact.zEMail.HrefValue = ""

			' Active
			Contact.Active.HrefValue = ""

			' CompanyID
			Contact.CompanyID.HrefValue = ""

			' GroupID
			Contact.GroupID.HrefValue = ""

			' TemplatePrefix
			Contact.TemplatePrefix.HrefValue = ""

			' RoleID
			Contact.RoleID.HrefValue = ""

		'
		'  Search Row
		'

		ElseIf Contact.RowType = EW_ROWTYPE_SEARCH Then ' Search row

			' LogonName
			Contact.LogonName.EditCustomAttributes = ""
			Contact.LogonName.EditValue = ew_HtmlEncode(Contact.LogonName.AdvancedSearch.SearchValue)

			' PrimaryContact
			Contact.PrimaryContact.EditCustomAttributes = ""
			Contact.PrimaryContact.EditValue = ew_HtmlEncode(Contact.PrimaryContact.AdvancedSearch.SearchValue)

			' EMail
			Contact.zEMail.EditCustomAttributes = ""
			Contact.zEMail.EditValue = ew_HtmlEncode(Contact.zEMail.AdvancedSearch.SearchValue)

			' Active
			Contact.Active.EditCustomAttributes = ""

			' CompanyID
			Contact.CompanyID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [CompanyID], [CompanyName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Company]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			Contact.CompanyID.EditValue = arwrk

			' GroupID
			Contact.GroupID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [GroupID], [GroupName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Group]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [GroupID] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			Contact.GroupID.EditValue = arwrk

			' TemplatePrefix
			Contact.TemplatePrefix.EditCustomAttributes = ""
			sSqlWrk = "SELECT [TemplatePrefix], [Name], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteTemplate]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [Name] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			Contact.TemplatePrefix.EditValue = arwrk

			' RoleID
			Contact.RoleID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [RoleID], [RoleName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [role]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [RoleName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			Contact.RoleID.EditValue = arwrk
		End If

		' Row Rendered event
		Contact.Row_Rendered()
	End Sub

	'
	' Validate search
	'
	Function ValidateSearch() As Boolean

		' Initialize
		ParentPage.gsSearchError = ""

		' Check if validation required
		If Not EW_SERVER_VALIDATE Then Return True ' Skip
		If Not ew_CheckEmail(Contact.zEMail.AdvancedSearch.SearchValue) Then
			If ParentPage.gsSearchError <> "" Then ParentPage.gsSearchError = ParentPage.gsSearchError & "<br />"
			ParentPage.gsSearchError = ParentPage.gsSearchError & "Incorrect email - EMail"
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
		Contact.LogonName.AdvancedSearch.SearchValue = Contact.GetAdvancedSearch("x_LogonName")
		Contact.PrimaryContact.AdvancedSearch.SearchValue = Contact.GetAdvancedSearch("x_PrimaryContact")
		Contact.zEMail.AdvancedSearch.SearchValue = Contact.GetAdvancedSearch("x_zEMail")
		Contact.Active.AdvancedSearch.SearchValue = Contact.GetAdvancedSearch("x_Active")
		Contact.CompanyID.AdvancedSearch.SearchValue = Contact.GetAdvancedSearch("x_CompanyID")
		Contact.GroupID.AdvancedSearch.SearchValue = Contact.GetAdvancedSearch("x_GroupID")
		Contact.TemplatePrefix.AdvancedSearch.SearchValue = Contact.GetAdvancedSearch("x_TemplatePrefix")
		Contact.RoleID.AdvancedSearch.SearchValue = Contact.GetAdvancedSearch("x_RoleID")
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
		Contact_search = New cContact_search(Me)		
		Contact_search.Page_Init()

		' Page main processing
		Contact_search.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If Contact_search IsNot Nothing Then Contact_search.Dispose()
	End Sub
End Class
