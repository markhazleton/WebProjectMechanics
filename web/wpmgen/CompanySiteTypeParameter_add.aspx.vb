Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class CompanySiteTypeParameter_add
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public CompanySiteTypeParameter_add As cCompanySiteTypeParameter_add

	'
	' Page Class
	'
	Class cCompanySiteTypeParameter_add
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
				If CompanySiteTypeParameter.UseTokenInUrl Then Url = Url & "t=" & CompanySiteTypeParameter.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If CompanySiteTypeParameter.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (CompanySiteTypeParameter.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (CompanySiteTypeParameter.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' CompanySiteTypeParameter
		Public Property CompanySiteTypeParameter() As cCompanySiteTypeParameter
			Get				
				Return ParentPage.CompanySiteTypeParameter
			End Get
			Set(ByVal v As cCompanySiteTypeParameter)
				ParentPage.CompanySiteTypeParameter = v	
			End Set	
		End Property

		' CompanySiteTypeParameter
		Public Property SiteParameterType() As cSiteParameterType
			Get				
				Return ParentPage.SiteParameterType
			End Get
			Set(ByVal v As cSiteParameterType)
				ParentPage.SiteParameterType = v	
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
			m_PageObjName = "CompanySiteTypeParameter_add"
			m_PageObjTypeName = "cCompanySiteTypeParameter_add"

			' Table Name
			m_TableName = "CompanySiteTypeParameter"

			' Initialize table object
			CompanySiteTypeParameter = New cCompanySiteTypeParameter(Me)
			SiteParameterType = New cSiteParameterType(Me)

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
			CompanySiteTypeParameter.Dispose()
			SiteParameterType.Dispose()

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
		If ew_Get("CompanySiteTypeParameterID") <> "" Then
			CompanySiteTypeParameter.CompanySiteTypeParameterID.QueryStringValue = ew_Get("CompanySiteTypeParameterID")
		Else
			bCopy = False
		End If

		' Create form object
		ObjForm = New cFormObj

		' Process form if post back
		If ObjForm.GetValue("a_add") <> "" Then
			CompanySiteTypeParameter.CurrentAction = ObjForm.GetValue("a_add") ' Get form action
			LoadFormValues() ' Load form values

			' Validate Form
			If Not ValidateForm() Then
				CompanySiteTypeParameter.CurrentAction = "I" ' Form error, reset action
				Message = ParentPage.gsFormError
			End If

		' Not post back
		Else
			If bCopy Then
				CompanySiteTypeParameter.CurrentAction = "C" ' Copy Record
			Else
				CompanySiteTypeParameter.CurrentAction = "I" ' Display Blank Record
				LoadDefaultValues() ' Load default values
			End If
		End If

		' Perform action based on action code
		Select Case CompanySiteTypeParameter.CurrentAction
			Case "I" ' Blank record, no action required
			Case "C" ' Copy an existing record
				If Not LoadRow() Then ' Load record based on key
					Message = "No records found" ' No record found
					Page_Terminate("CompanySiteTypeParameter_list.aspx") ' No matching record, return to list
				End If
			Case "A" ' Add new record
				CompanySiteTypeParameter.SendEmail = True ' Send email on add success
				If AddRow() Then ' Add successful
					Message = "Add succeeded" ' Set up success message
					Dim sReturnUrl As String = CompanySiteTypeParameter.ReturnUrl
					If ew_GetPageName(sReturnUrl) = "CompanySiteTypeParameter_view.aspx" Then sReturnUrl = CompanySiteTypeParameter.ViewUrl ' View paging, return to view page with keyurl directly
					Page_Terminate(sReturnUrl) ' Clean up and return
				Else
					RestoreFormValues() ' Add failed, restore form values
				End If
		End Select

		' Render row based on row type
		CompanySiteTypeParameter.RowType = EW_ROWTYPE_ADD ' Render add type

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
		CompanySiteTypeParameter.SortOrder.CurrentValue = 999
	End Sub

	'
	' Load form values
	'
	Sub LoadFormValues()
		CompanySiteTypeParameter.SiteParameterTypeID.FormValue = ObjForm.GetValue("x_SiteParameterTypeID")
		CompanySiteTypeParameter.SiteParameterTypeID.OldValue = ObjForm.GetValue("o_SiteParameterTypeID")
		CompanySiteTypeParameter.CompanyID.FormValue = ObjForm.GetValue("x_CompanyID")
		CompanySiteTypeParameter.CompanyID.OldValue = ObjForm.GetValue("o_CompanyID")
		CompanySiteTypeParameter.SiteCategoryTypeID.FormValue = ObjForm.GetValue("x_SiteCategoryTypeID")
		CompanySiteTypeParameter.SiteCategoryTypeID.OldValue = ObjForm.GetValue("o_SiteCategoryTypeID")
		CompanySiteTypeParameter.SiteCategoryGroupID.FormValue = ObjForm.GetValue("x_SiteCategoryGroupID")
		CompanySiteTypeParameter.SiteCategoryGroupID.OldValue = ObjForm.GetValue("o_SiteCategoryGroupID")
		CompanySiteTypeParameter.SiteCategoryID.FormValue = ObjForm.GetValue("x_SiteCategoryID")
		CompanySiteTypeParameter.SiteCategoryID.OldValue = ObjForm.GetValue("o_SiteCategoryID")
		CompanySiteTypeParameter.SortOrder.FormValue = ObjForm.GetValue("x_SortOrder")
		CompanySiteTypeParameter.SortOrder.OldValue = ObjForm.GetValue("o_SortOrder")
		CompanySiteTypeParameter.ParameterValue.FormValue = ObjForm.GetValue("x_ParameterValue")
		CompanySiteTypeParameter.ParameterValue.OldValue = ObjForm.GetValue("o_ParameterValue")
		CompanySiteTypeParameter.CompanySiteTypeParameterID.FormValue = ObjForm.GetValue("x_CompanySiteTypeParameterID")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		CompanySiteTypeParameter.SiteParameterTypeID.CurrentValue = CompanySiteTypeParameter.SiteParameterTypeID.FormValue
		CompanySiteTypeParameter.CompanyID.CurrentValue = CompanySiteTypeParameter.CompanyID.FormValue
		CompanySiteTypeParameter.SiteCategoryTypeID.CurrentValue = CompanySiteTypeParameter.SiteCategoryTypeID.FormValue
		CompanySiteTypeParameter.SiteCategoryGroupID.CurrentValue = CompanySiteTypeParameter.SiteCategoryGroupID.FormValue
		CompanySiteTypeParameter.SiteCategoryID.CurrentValue = CompanySiteTypeParameter.SiteCategoryID.FormValue
		CompanySiteTypeParameter.SortOrder.CurrentValue = CompanySiteTypeParameter.SortOrder.FormValue
		CompanySiteTypeParameter.ParameterValue.CurrentValue = CompanySiteTypeParameter.ParameterValue.FormValue
		CompanySiteTypeParameter.CompanySiteTypeParameterID.CurrentValue = CompanySiteTypeParameter.CompanySiteTypeParameterID.FormValue
	End Sub

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = CompanySiteTypeParameter.KeyFilter

		' Row Selecting event
		CompanySiteTypeParameter.Row_Selecting(sFilter)

		' Load SQL based on filter
		CompanySiteTypeParameter.CurrentFilter = sFilter
		Dim sSql As String = CompanySiteTypeParameter.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				CompanySiteTypeParameter.Row_Selected(RsRow)
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
		CompanySiteTypeParameter.CompanySiteTypeParameterID.DbValue = RsRow("CompanySiteTypeParameterID")
		CompanySiteTypeParameter.SiteParameterTypeID.DbValue = RsRow("SiteParameterTypeID")
		CompanySiteTypeParameter.CompanyID.DbValue = RsRow("CompanyID")
		CompanySiteTypeParameter.SiteCategoryTypeID.DbValue = RsRow("SiteCategoryTypeID")
		CompanySiteTypeParameter.SiteCategoryGroupID.DbValue = RsRow("SiteCategoryGroupID")
		CompanySiteTypeParameter.SiteCategoryID.DbValue = RsRow("SiteCategoryID")
		CompanySiteTypeParameter.SortOrder.DbValue = RsRow("SortOrder")
		CompanySiteTypeParameter.ParameterValue.DbValue = RsRow("ParameterValue")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		CompanySiteTypeParameter.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' SiteParameterTypeID

		CompanySiteTypeParameter.SiteParameterTypeID.CellCssStyle = ""
		CompanySiteTypeParameter.SiteParameterTypeID.CellCssClass = ""

		' CompanyID
		CompanySiteTypeParameter.CompanyID.CellCssStyle = ""
		CompanySiteTypeParameter.CompanyID.CellCssClass = ""

		' SiteCategoryTypeID
		CompanySiteTypeParameter.SiteCategoryTypeID.CellCssStyle = ""
		CompanySiteTypeParameter.SiteCategoryTypeID.CellCssClass = ""

		' SiteCategoryGroupID
		CompanySiteTypeParameter.SiteCategoryGroupID.CellCssStyle = ""
		CompanySiteTypeParameter.SiteCategoryGroupID.CellCssClass = ""

		' SiteCategoryID
		CompanySiteTypeParameter.SiteCategoryID.CellCssStyle = ""
		CompanySiteTypeParameter.SiteCategoryID.CellCssClass = ""

		' SortOrder
		CompanySiteTypeParameter.SortOrder.CellCssStyle = ""
		CompanySiteTypeParameter.SortOrder.CellCssClass = ""

		' ParameterValue
		CompanySiteTypeParameter.ParameterValue.CellCssStyle = ""
		CompanySiteTypeParameter.ParameterValue.CellCssClass = ""

		'
		'  View  Row
		'

		If CompanySiteTypeParameter.RowType = EW_ROWTYPE_VIEW Then ' View row

			' SiteParameterTypeID
			If ew_NotEmpty(CompanySiteTypeParameter.SiteParameterTypeID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteParameterTypeNM] FROM [SiteParameterType] WHERE [SiteParameterTypeID] = " & ew_AdjustSql(CompanySiteTypeParameter.SiteParameterTypeID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteParameterTypeNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanySiteTypeParameter.SiteParameterTypeID.ViewValue = RsWrk("SiteParameterTypeNM")
				Else
					CompanySiteTypeParameter.SiteParameterTypeID.ViewValue = CompanySiteTypeParameter.SiteParameterTypeID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanySiteTypeParameter.SiteParameterTypeID.ViewValue = System.DBNull.Value
			End If
			CompanySiteTypeParameter.SiteParameterTypeID.CssStyle = ""
			CompanySiteTypeParameter.SiteParameterTypeID.CssClass = ""
			CompanySiteTypeParameter.SiteParameterTypeID.ViewCustomAttributes = ""

			' CompanyID
			If ew_NotEmpty(CompanySiteTypeParameter.CompanyID.CurrentValue) Then
				sSqlWrk = "SELECT [CompanyName] FROM [Company] WHERE [CompanyID] = " & ew_AdjustSql(CompanySiteTypeParameter.CompanyID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanySiteTypeParameter.CompanyID.ViewValue = RsWrk("CompanyName")
				Else
					CompanySiteTypeParameter.CompanyID.ViewValue = CompanySiteTypeParameter.CompanyID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanySiteTypeParameter.CompanyID.ViewValue = System.DBNull.Value
			End If
			CompanySiteTypeParameter.CompanyID.CssStyle = ""
			CompanySiteTypeParameter.CompanyID.CssClass = ""
			CompanySiteTypeParameter.CompanyID.ViewCustomAttributes = ""

			' SiteCategoryTypeID
			If ew_NotEmpty(CompanySiteTypeParameter.SiteCategoryTypeID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteCategoryTypeNM] FROM [SiteCategoryType] WHERE [SiteCategoryTypeID] = " & ew_AdjustSql(CompanySiteTypeParameter.SiteCategoryTypeID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryTypeNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanySiteTypeParameter.SiteCategoryTypeID.ViewValue = RsWrk("SiteCategoryTypeNM")
				Else
					CompanySiteTypeParameter.SiteCategoryTypeID.ViewValue = CompanySiteTypeParameter.SiteCategoryTypeID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanySiteTypeParameter.SiteCategoryTypeID.ViewValue = System.DBNull.Value
			End If
			CompanySiteTypeParameter.SiteCategoryTypeID.CssStyle = ""
			CompanySiteTypeParameter.SiteCategoryTypeID.CssClass = ""
			CompanySiteTypeParameter.SiteCategoryTypeID.ViewCustomAttributes = ""

			' SiteCategoryGroupID
			If ew_NotEmpty(CompanySiteTypeParameter.SiteCategoryGroupID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteCategoryGroupNM] FROM [SiteCategoryGroup] WHERE [SiteCategoryGroupID] = " & ew_AdjustSql(CompanySiteTypeParameter.SiteCategoryGroupID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryGroupNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanySiteTypeParameter.SiteCategoryGroupID.ViewValue = RsWrk("SiteCategoryGroupNM")
				Else
					CompanySiteTypeParameter.SiteCategoryGroupID.ViewValue = CompanySiteTypeParameter.SiteCategoryGroupID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanySiteTypeParameter.SiteCategoryGroupID.ViewValue = System.DBNull.Value
			End If
			CompanySiteTypeParameter.SiteCategoryGroupID.CssStyle = ""
			CompanySiteTypeParameter.SiteCategoryGroupID.CssClass = ""
			CompanySiteTypeParameter.SiteCategoryGroupID.ViewCustomAttributes = ""

			' SiteCategoryID
			If ew_NotEmpty(CompanySiteTypeParameter.SiteCategoryID.CurrentValue) Then
				sSqlWrk = "SELECT [CategoryName] FROM [SiteCategory] WHERE [SiteCategoryID] = " & ew_AdjustSql(CompanySiteTypeParameter.SiteCategoryID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [CategoryName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanySiteTypeParameter.SiteCategoryID.ViewValue = RsWrk("CategoryName")
				Else
					CompanySiteTypeParameter.SiteCategoryID.ViewValue = CompanySiteTypeParameter.SiteCategoryID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanySiteTypeParameter.SiteCategoryID.ViewValue = System.DBNull.Value
			End If
			CompanySiteTypeParameter.SiteCategoryID.CssStyle = ""
			CompanySiteTypeParameter.SiteCategoryID.CssClass = ""
			CompanySiteTypeParameter.SiteCategoryID.ViewCustomAttributes = ""

			' SortOrder
			CompanySiteTypeParameter.SortOrder.ViewValue = CompanySiteTypeParameter.SortOrder.CurrentValue
			CompanySiteTypeParameter.SortOrder.CssStyle = ""
			CompanySiteTypeParameter.SortOrder.CssClass = ""
			CompanySiteTypeParameter.SortOrder.ViewCustomAttributes = ""

			' ParameterValue
			CompanySiteTypeParameter.ParameterValue.ViewValue = CompanySiteTypeParameter.ParameterValue.CurrentValue
			CompanySiteTypeParameter.ParameterValue.CssStyle = ""
			CompanySiteTypeParameter.ParameterValue.CssClass = ""
			CompanySiteTypeParameter.ParameterValue.ViewCustomAttributes = ""

			' View refer script
			' SiteParameterTypeID

			CompanySiteTypeParameter.SiteParameterTypeID.HrefValue = ""

			' CompanyID
			CompanySiteTypeParameter.CompanyID.HrefValue = ""

			' SiteCategoryTypeID
			CompanySiteTypeParameter.SiteCategoryTypeID.HrefValue = ""

			' SiteCategoryGroupID
			CompanySiteTypeParameter.SiteCategoryGroupID.HrefValue = ""

			' SiteCategoryID
			CompanySiteTypeParameter.SiteCategoryID.HrefValue = ""

			' SortOrder
			CompanySiteTypeParameter.SortOrder.HrefValue = ""

			' ParameterValue
			CompanySiteTypeParameter.ParameterValue.HrefValue = ""

		'
		'  Add Row
		'

		ElseIf CompanySiteTypeParameter.RowType = EW_ROWTYPE_ADD Then ' Add row

			' SiteParameterTypeID
			CompanySiteTypeParameter.SiteParameterTypeID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteParameterTypeID], [SiteParameterTypeNM], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteParameterType]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [SiteParameterTypeNM] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			CompanySiteTypeParameter.SiteParameterTypeID.EditValue = arwrk

			' CompanyID
			CompanySiteTypeParameter.CompanyID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [CompanyID], [CompanyName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Company]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			CompanySiteTypeParameter.CompanyID.EditValue = arwrk

			' SiteCategoryTypeID
			CompanySiteTypeParameter.SiteCategoryTypeID.EditCustomAttributes = ""
			If CompanySiteTypeParameter.SiteCategoryTypeID.SessionValue <> "" Then
				CompanySiteTypeParameter.SiteCategoryTypeID.CurrentValue = CompanySiteTypeParameter.SiteCategoryTypeID.SessionValue
			If ew_NotEmpty(CompanySiteTypeParameter.SiteCategoryTypeID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteCategoryTypeNM] FROM [SiteCategoryType] WHERE [SiteCategoryTypeID] = " & ew_AdjustSql(CompanySiteTypeParameter.SiteCategoryTypeID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryTypeNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanySiteTypeParameter.SiteCategoryTypeID.ViewValue = RsWrk("SiteCategoryTypeNM")
				Else
					CompanySiteTypeParameter.SiteCategoryTypeID.ViewValue = CompanySiteTypeParameter.SiteCategoryTypeID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanySiteTypeParameter.SiteCategoryTypeID.ViewValue = System.DBNull.Value
			End If
			CompanySiteTypeParameter.SiteCategoryTypeID.CssStyle = ""
			CompanySiteTypeParameter.SiteCategoryTypeID.CssClass = ""
			CompanySiteTypeParameter.SiteCategoryTypeID.ViewCustomAttributes = ""
			Else
			sSqlWrk = "SELECT [SiteCategoryTypeID], [SiteCategoryTypeNM], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteCategoryType]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryTypeNM] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			CompanySiteTypeParameter.SiteCategoryTypeID.EditValue = arwrk
			End If

			' SiteCategoryGroupID
			CompanySiteTypeParameter.SiteCategoryGroupID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryGroupID], [SiteCategoryGroupNM], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteCategoryGroup]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryGroupNM] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			CompanySiteTypeParameter.SiteCategoryGroupID.EditValue = arwrk

			' SiteCategoryID
			CompanySiteTypeParameter.SiteCategoryID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryID], [CategoryName], '' AS Disp2Fld, [SiteCategoryTypeID] FROM [SiteCategory]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [CategoryName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select", ""}) 
			CompanySiteTypeParameter.SiteCategoryID.EditValue = arwrk

			' SortOrder
			CompanySiteTypeParameter.SortOrder.EditCustomAttributes = ""
			CompanySiteTypeParameter.SortOrder.EditValue = ew_HtmlEncode(CompanySiteTypeParameter.SortOrder.CurrentValue)

			' ParameterValue
			CompanySiteTypeParameter.ParameterValue.EditCustomAttributes = ""
			CompanySiteTypeParameter.ParameterValue.EditValue = ew_HtmlEncode(CompanySiteTypeParameter.ParameterValue.CurrentValue)
		End If

		' Row Rendered event
		CompanySiteTypeParameter.Row_Rendered()
	End Sub

	'
	' Validate form
	'
	Function ValidateForm() As Boolean

		' Initialize
		ParentPage.gsFormError = ""

		' Check if validation required
		If Not EW_SERVER_VALIDATE Then Return (ParentPage.gsFormError = "")
		If ew_Empty(CompanySiteTypeParameter.SiteParameterTypeID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter required field - Parameter"
		End If
		If Not ew_CheckInteger(CompanySiteTypeParameter.SortOrder.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Incorrect integer - Process Order"
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

		' SiteParameterTypeID
		CompanySiteTypeParameter.SiteParameterTypeID.SetDbValue(CompanySiteTypeParameter.SiteParameterTypeID.CurrentValue, 0)
		Rs("SiteParameterTypeID") = CompanySiteTypeParameter.SiteParameterTypeID.DbValue

		' CompanyID
		CompanySiteTypeParameter.CompanyID.SetDbValue(CompanySiteTypeParameter.CompanyID.CurrentValue, System.DBNull.Value)
		Rs("CompanyID") = CompanySiteTypeParameter.CompanyID.DbValue

		' SiteCategoryTypeID
		CompanySiteTypeParameter.SiteCategoryTypeID.SetDbValue(CompanySiteTypeParameter.SiteCategoryTypeID.CurrentValue, System.DBNull.Value)
		Rs("SiteCategoryTypeID") = CompanySiteTypeParameter.SiteCategoryTypeID.DbValue

		' SiteCategoryGroupID
		CompanySiteTypeParameter.SiteCategoryGroupID.SetDbValue(CompanySiteTypeParameter.SiteCategoryGroupID.CurrentValue, System.DBNull.Value)
		Rs("SiteCategoryGroupID") = CompanySiteTypeParameter.SiteCategoryGroupID.DbValue

		' SiteCategoryID
		CompanySiteTypeParameter.SiteCategoryID.SetDbValue(CompanySiteTypeParameter.SiteCategoryID.CurrentValue, System.DBNull.Value)
		Rs("SiteCategoryID") = CompanySiteTypeParameter.SiteCategoryID.DbValue

		' SortOrder
		CompanySiteTypeParameter.SortOrder.SetDbValue(CompanySiteTypeParameter.SortOrder.CurrentValue, System.DBNull.Value)
		Rs("SortOrder") = CompanySiteTypeParameter.SortOrder.DbValue

		' ParameterValue
		CompanySiteTypeParameter.ParameterValue.SetDbValue(CompanySiteTypeParameter.ParameterValue.CurrentValue, System.DBNull.Value)
		Rs("ParameterValue") = CompanySiteTypeParameter.ParameterValue.DbValue

		' Row Inserting event
		bInsertRow = CompanySiteTypeParameter.Row_Inserting(Rs)
		If bInsertRow Then
			Try	
				CompanySiteTypeParameter.Insert(Rs)
				AddRow = True
			Catch e As Exception
				If EW_DEBUG_ENABLED Then Throw
				Message = e.Message				
				AddRow = False
			End Try
		Else
			If CompanySiteTypeParameter.CancelMessage <> "" Then
				Message = CompanySiteTypeParameter.CancelMessage
				CompanySiteTypeParameter.CancelMessage = ""
			Else
				Message = "Insert cancelled"
			End If
			AddRow = False
		End If
		If AddRow Then
			LastInsertId = Conn.GetLastInsertId()
			CompanySiteTypeParameter.CompanySiteTypeParameterID.DbValue = LastInsertId
			Rs("CompanySiteTypeParameterID") = CompanySiteTypeParameter.CompanySiteTypeParameterID.DbValue		

			' Row Inserted event
			CompanySiteTypeParameter.Row_Inserted(Rs)
			WriteAuditTrailOnAdd(Rs)
		End If
	End Function

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "CompanySiteTypeParameter"
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
		Dim table As String = "CompanySiteTypeParameter"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsSrc("CompanySiteTypeParameterID")

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

		' SiteParameterTypeID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteParameterTypeID", keyvalue, oldvalue, RsSrc("SiteParameterTypeID"))

		' CompanyID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "CompanyID", keyvalue, oldvalue, RsSrc("CompanyID"))

		' SiteCategoryTypeID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteCategoryTypeID", keyvalue, oldvalue, RsSrc("SiteCategoryTypeID"))

		' SiteCategoryGroupID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteCategoryGroupID", keyvalue, oldvalue, RsSrc("SiteCategoryGroupID"))

		' SiteCategoryID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteCategoryID", keyvalue, oldvalue, RsSrc("SiteCategoryID"))

		' SortOrder Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SortOrder", keyvalue, oldvalue, RsSrc("SortOrder"))

		' ParameterValue Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "ParameterValue", keyvalue, oldvalue, "<MEMO>")
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
		CompanySiteTypeParameter_add = New cCompanySiteTypeParameter_add(Me)		
		CompanySiteTypeParameter_add.Page_Init()

		' Page main processing
		CompanySiteTypeParameter_add.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If CompanySiteTypeParameter_add IsNot Nothing Then CompanySiteTypeParameter_add.Dispose()
	End Sub
End Class
