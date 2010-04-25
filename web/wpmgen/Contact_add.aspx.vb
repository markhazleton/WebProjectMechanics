Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class Contact_add
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public Contact_add As cContact_add

	'
	' Page Class
	'
	Class cContact_add
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
			m_PageID = "add"
			m_PageObjName = "Contact_add"
			m_PageObjTypeName = "cContact_add"

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

	Public x_ewPriv As Integer

	'
	' Page main processing
	'
	Sub Page_Main()

		' Load key values from QueryString
		Dim bCopy As Boolean = True
		If ew_Get("ContactID") <> "" Then
			Contact.ContactID.QueryStringValue = ew_Get("ContactID")
		Else
			bCopy = False
		End If

		' Create form object
		ObjForm = New cFormObj

		' Process form if post back
		If ObjForm.GetValue("a_add") <> "" Then
			Contact.CurrentAction = ObjForm.GetValue("a_add") ' Get form action
			LoadFormValues() ' Load form values

			' Validate Form
			If Not ValidateForm() Then
				Contact.CurrentAction = "I" ' Form error, reset action
				Message = ParentPage.gsFormError
			End If

		' Not post back
		Else
			If bCopy Then
				Contact.CurrentAction = "C" ' Copy Record
			Else
				Contact.CurrentAction = "I" ' Display Blank Record
				LoadDefaultValues() ' Load default values
			End If
		End If

		' Perform action based on action code
		Select Case Contact.CurrentAction
			Case "I" ' Blank record, no action required
			Case "C" ' Copy an existing record
				If Not LoadRow() Then ' Load record based on key
					Message = "No records found" ' No record found
					Page_Terminate("Contact_list.aspx") ' No matching record, return to list
				End If
			Case "A" ' Add new record
				Contact.SendEmail = True ' Send email on add success
				If AddRow() Then ' Add successful
					Message = "Add succeeded" ' Set up success message
					Dim sReturnUrl As String = Contact.ReturnUrl
					If ew_GetPageName(sReturnUrl) = "Contact_view.aspx" Then sReturnUrl = Contact.ViewUrl ' View paging, return to view page with keyurl directly
					Page_Terminate(sReturnUrl) ' Clean up and return
				Else
					RestoreFormValues() ' Add failed, restore form values
				End If
		End Select

		' Render row based on row type
		Contact.RowType = EW_ROWTYPE_ADD ' Render add type

		' Render row
		RenderRow()
	End Sub

	'
	' Get upload file
	'
	Sub GetUploadFiles()

		' Get upload data
	End Sub

	'
	' Load default values
	'
	Sub LoadDefaultValues()
		Contact.Active.CurrentValue = 1
		Contact.CompanyID.CurrentValue = 0
		Contact.GroupID.CurrentValue = 0
		Contact.RoleID.CurrentValue = 0
	End Sub

	'
	' Load form values
	'
	Sub LoadFormValues()
		Contact.LogonName.FormValue = ObjForm.GetValue("x_LogonName")
		Contact.LogonName.OldValue = ObjForm.GetValue("o_LogonName")
		Contact.LogonPassword.FormValue = ObjForm.GetValue("x_LogonPassword")
		Contact.LogonPassword.OldValue = ObjForm.GetValue("o_LogonPassword")
		Contact.PrimaryContact.FormValue = ObjForm.GetValue("x_PrimaryContact")
		Contact.PrimaryContact.OldValue = ObjForm.GetValue("o_PrimaryContact")
		Contact.zEMail.FormValue = ObjForm.GetValue("x_zEMail")
		Contact.zEMail.OldValue = ObjForm.GetValue("o_zEMail")
		Contact.Active.FormValue = ObjForm.GetValue("x_Active")
		Contact.Active.OldValue = ObjForm.GetValue("o_Active")
		Contact.CompanyID.FormValue = ObjForm.GetValue("x_CompanyID")
		Contact.CompanyID.OldValue = ObjForm.GetValue("o_CompanyID")
		Contact.GroupID.FormValue = ObjForm.GetValue("x_GroupID")
		Contact.GroupID.OldValue = ObjForm.GetValue("o_GroupID")
		Contact.TemplatePrefix.FormValue = ObjForm.GetValue("x_TemplatePrefix")
		Contact.TemplatePrefix.OldValue = ObjForm.GetValue("o_TemplatePrefix")
		Contact.RoleID.FormValue = ObjForm.GetValue("x_RoleID")
		Contact.RoleID.OldValue = ObjForm.GetValue("o_RoleID")
		Contact.ContactID.FormValue = ObjForm.GetValue("x_ContactID")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		Contact.LogonName.CurrentValue = Contact.LogonName.FormValue
		Contact.LogonPassword.CurrentValue = Contact.LogonPassword.FormValue
		Contact.PrimaryContact.CurrentValue = Contact.PrimaryContact.FormValue
		Contact.zEMail.CurrentValue = Contact.zEMail.FormValue
		Contact.Active.CurrentValue = Contact.Active.FormValue
		Contact.CompanyID.CurrentValue = Contact.CompanyID.FormValue
		Contact.GroupID.CurrentValue = Contact.GroupID.FormValue
		Contact.TemplatePrefix.CurrentValue = Contact.TemplatePrefix.FormValue
		Contact.RoleID.CurrentValue = Contact.RoleID.FormValue
		Contact.ContactID.CurrentValue = Contact.ContactID.FormValue
	End Sub

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = Contact.KeyFilter

		' Row Selecting event
		Contact.Row_Selecting(sFilter)

		' Load SQL based on filter
		Contact.CurrentFilter = sFilter
		Dim sSql As String = Contact.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				Contact.Row_Selected(RsRow)
				Return True	
			End If
		Catch
			If EW_DEBUG_ENABLED Then Throw
			Return False
		Finally
			Conn.CloseTempDataReader()
		End Try
	End Function

	'
	' Load row values from recordset
	'
	Sub LoadRowValues(ByRef RsRow As OleDbDataReader)
		Contact.ContactID.DbValue = RsRow("ContactID")
		Contact.LogonName.DbValue = RsRow("LogonName")
		Contact.LogonPassword.DbValue = RsRow("LogonPassword")
		Contact.PrimaryContact.DbValue = RsRow("PrimaryContact")
		Contact.zEMail.DbValue = RsRow("EMail")
		Contact.Active.DbValue = IIf(ew_ConvertToBool(RsRow("Active")), "1", "0")
		Contact.CompanyID.DbValue = RsRow("CompanyID")
		Contact.GroupID.DbValue = RsRow("GroupID")
		Contact.TemplatePrefix.DbValue = RsRow("TemplatePrefix")
		Contact.RoleID.DbValue = RsRow("RoleID")
		Contact.FirstName.DbValue = RsRow("FirstName")
		Contact.MiddleInitial.DbValue = RsRow("MiddleInitial")
		Contact.LastName.DbValue = RsRow("LastName")
		Contact.OfficePhone.DbValue = RsRow("OfficePhone")
		Contact.HomePhone.DbValue = RsRow("HomePhone")
		Contact.MobilPhone.DbValue = RsRow("MobilPhone")
		Contact.Pager.DbValue = RsRow("Pager")
		Contact.URL.DbValue = RsRow("URL")
		Contact.Address1.DbValue = RsRow("Address1")
		Contact.Address2.DbValue = RsRow("Address2")
		Contact.City.DbValue = RsRow("City")
		Contact.State.DbValue = RsRow("State")
		Contact.Country.DbValue = RsRow("Country")
		Contact.PostalCode.DbValue = RsRow("PostalCode")
		Contact.Biography.DbValue = RsRow("Biography")
		Contact.CreateDT.DbValue = RsRow("CreateDT")
		Contact.Paid.DbValue = RsRow("Paid")
		Contact.email_subscribe.DbValue = IIf(ew_ConvertToBool(RsRow("email_subscribe")), "1", "0")
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

		' LogonPassword
		Contact.LogonPassword.CellCssStyle = ""
		Contact.LogonPassword.CellCssClass = ""

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

			' LogonPassword
			Contact.LogonPassword.HrefValue = ""

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
		'  Add Row
		'

		ElseIf Contact.RowType = EW_ROWTYPE_ADD Then ' Add row

			' LogonName
			Contact.LogonName.EditCustomAttributes = ""
			Contact.LogonName.EditValue = ew_HtmlEncode(Contact.LogonName.CurrentValue)

			' LogonPassword
			Contact.LogonPassword.EditCustomAttributes = ""
			Contact.LogonPassword.EditValue = ew_HtmlEncode(Contact.LogonPassword.CurrentValue)

			' PrimaryContact
			Contact.PrimaryContact.EditCustomAttributes = ""
			Contact.PrimaryContact.EditValue = ew_HtmlEncode(Contact.PrimaryContact.CurrentValue)

			' EMail
			Contact.zEMail.EditCustomAttributes = ""
			Contact.zEMail.EditValue = ew_HtmlEncode(Contact.zEMail.CurrentValue)

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
	' Validate form
	'
	Function ValidateForm() As Boolean

		' Initialize
		ParentPage.gsFormError = ""

		' Check if validation required
		If Not EW_SERVER_VALIDATE Then Return (ParentPage.gsFormError = "")
		If ew_Empty(Contact.LogonName.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter required field - Logon Name"
		End If
		If ew_Empty(Contact.LogonPassword.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter required field - Logon Password"
		End If
		If Not ew_CheckEmail(Contact.zEMail.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Incorrect email - EMail"
		End If
		If ew_Empty(Contact.TemplatePrefix.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter required field - Name"
		End If

		' Return validate result
		Dim Valid As Boolean = (ParentPage.gsFormError = "")

		' Form_CustomValidate event
		Dim sFormCustomError As String = ""
		Valid = Valid And Form_CustomValidate(sFormCustomError)
		If sFormCustomError <> "" Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & sFormCustomError
		End If
		Return Valid
	End Function

	'
	' Add record
	'
	Function AddRow() As Boolean
		Dim Rs As New OrderedDictionary
		Dim sSql As String, sFilter As String
		Dim bInsertRow As Boolean
		Dim RsChk As OleDbDataReader
		Dim sIdxErrMsg As String
		Dim LastInsertId As Object
		If ew_NotEmpty(Contact.LogonName.CurrentValue) Then ' Check field with unique index
			sFilter = "([LogonName] = '" & ew_AdjustSql(Contact.LogonName.CurrentValue) & "')"
			RsChk = Contact.LoadRs(sFilter)
			If RsChk IsNot Nothing Then
				sIdxErrMsg = "Duplicate value '%v' for unique index '%f'".Replace("%f", "LogonName")
				sIdxErrMsg = sIdxErrMsg.Replace("%v", Contact.LogonName.CurrentValue)
				Message = sIdxErrMsg
				RsChk.Close()
				RsChk.Dispose()
				Return False
			End If
		End If

		' LogonName
		Contact.LogonName.SetDbValue(Contact.LogonName.CurrentValue, "")
		Rs("LogonName") = Contact.LogonName.DbValue

		' LogonPassword
		Contact.LogonPassword.SetDbValue(Contact.LogonPassword.CurrentValue, System.DBNull.Value)
		Rs("LogonPassword") = Contact.LogonPassword.DbValue

		' PrimaryContact
		Contact.PrimaryContact.SetDbValue(Contact.PrimaryContact.CurrentValue, System.DBNull.Value)
		Rs("PrimaryContact") = Contact.PrimaryContact.DbValue

		' EMail
		Contact.zEMail.SetDbValue(Contact.zEMail.CurrentValue, System.DBNull.Value)
		Rs("EMail") = Contact.zEMail.DbValue

		' Active
		Contact.Active.SetDbValue((Contact.Active.CurrentValue <> "" And Not IsDBNull(Contact.Active.CurrentValue)), System.DBNull.Value)
		Rs("Active") = Contact.Active.DbValue

		' CompanyID
		Contact.CompanyID.SetDbValue(Contact.CompanyID.CurrentValue, System.DBNull.Value)
		Rs("CompanyID") = Contact.CompanyID.DbValue

		' GroupID
		Contact.GroupID.SetDbValue(Contact.GroupID.CurrentValue, System.DBNull.Value)
		Rs("GroupID") = Contact.GroupID.DbValue

		' TemplatePrefix
		Contact.TemplatePrefix.SetDbValue(Contact.TemplatePrefix.CurrentValue, System.DBNull.Value)
		Rs("TemplatePrefix") = Contact.TemplatePrefix.DbValue

		' RoleID
		Contact.RoleID.SetDbValue(Contact.RoleID.CurrentValue, System.DBNull.Value)
		Rs("RoleID") = Contact.RoleID.DbValue

		' Row Inserting event
		bInsertRow = Contact.Row_Inserting(Rs)
		If bInsertRow Then
			Try	
				Contact.Insert(Rs)
				AddRow = True
			Catch e As Exception
				If EW_DEBUG_ENABLED Then Throw
				Message = e.Message				
				AddRow = False
			End Try
		Else
			If Contact.CancelMessage <> "" Then
				Message = Contact.CancelMessage
				Contact.CancelMessage = ""
			Else
				Message = "Insert cancelled"
			End If
			AddRow = False
		End If
		If AddRow Then
			LastInsertId = Conn.GetLastInsertId()
			Contact.ContactID.DbValue = LastInsertId
			Rs("ContactID") = Contact.ContactID.DbValue		

			' Row Inserted event
			Contact.Row_Inserted(Rs)
			WriteAuditTrailOnAdd(Rs)
		End If
	End Function

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "Contact"
		Dim filePfx As String = "log"
		Dim curDate As String, curTime As String, id As String, user As String, action As String
		Dim i As Integer
		Dim dt As DateTime = Now()		
		curDate = dt.ToString("yyyy/MM/dd")
		curTime = dt.ToString("HH:mm:ss")
		id = HttpContext.Current.Request.ServerVariables("SCRIPT_NAME")
    user = CurrentUserName()
		action = typ
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "", "", "", "")
	End Sub

	' Write Audit Trail (add page)
	Sub WriteAuditTrailOnAdd(ByRef RsSrc As OrderedDictionary)
		Dim table As String = "Contact"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsSrc("ContactID")

		' Write Audit Trail
		Dim filePfx As String = "log"
		Dim action As String = "A"
		Dim oldvalue As Object = ""
		Dim curDate As String, curTime As String, id As String, user As String, field As String
		Dim keyvalue As Object
		Dim dt As DateTime = Now()
		curDate = dt.ToString("yyyy/MM/dd")
		curTime = dt.ToString("HH:mm:ss")
		id = HttpContext.Current.Request.ServerVariables("SCRIPT_NAME")
    user = CurrentUserName()
		keyvalue = sKey

		' LogonName Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "LogonName", keyvalue, oldvalue, RsSrc("LogonName"))

		' LogonPassword Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "LogonPassword", keyvalue, oldvalue, RsSrc("LogonPassword"))

		' PrimaryContact Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "PrimaryContact", keyvalue, oldvalue, RsSrc("PrimaryContact"))

		' EMail Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "EMail", keyvalue, oldvalue, RsSrc("EMail"))

		' Active Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "Active", keyvalue, oldvalue, RsSrc("Active"))

		' CompanyID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "CompanyID", keyvalue, oldvalue, RsSrc("CompanyID"))

		' GroupID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "GroupID", keyvalue, oldvalue, RsSrc("GroupID"))

		' TemplatePrefix Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "TemplatePrefix", keyvalue, oldvalue, RsSrc("TemplatePrefix"))

		' RoleID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "RoleID", keyvalue, oldvalue, RsSrc("RoleID"))
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
		Contact_add = New cContact_add(Me)		
		Contact_add.Page_Init()

		' Page main processing
		Contact_add.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If Contact_add IsNot Nothing Then Contact_add.Dispose()
	End Sub
End Class
