Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class CompanySiteParameter_edit
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public CompanySiteParameter_edit As cCompanySiteParameter_edit

	'
	' Page Class
	'
	Class cCompanySiteParameter_edit
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
				If CompanySiteParameter.UseTokenInUrl Then Url = Url & "t=" & CompanySiteParameter.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If CompanySiteParameter.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (CompanySiteParameter.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (CompanySiteParameter.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' CompanySiteParameter
		Public Property CompanySiteParameter() As cCompanySiteParameter
			Get				
				Return ParentPage.CompanySiteParameter
			End Get
			Set(ByVal v As cCompanySiteParameter)
				ParentPage.CompanySiteParameter = v	
			End Set	
		End Property

		' CompanySiteParameter
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
			m_PageID = "edit"
			m_PageObjName = "CompanySiteParameter_edit"
			m_PageObjTypeName = "cCompanySiteParameter_edit"

			' Table Name
			m_TableName = "CompanySiteParameter"

			' Initialize table object
			CompanySiteParameter = New cCompanySiteParameter(Me)
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
			CompanySiteParameter.Dispose()
			SiteParameterType.Dispose()

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

		' Load key from QueryString
		If ew_Get("CompanySiteParameterID") <> "" Then
			CompanySiteParameter.CompanySiteParameterID.QueryStringValue = ew_Get("CompanySiteParameterID")
		End If

		' Create form object
		ObjForm = New cFormObj
		If ObjForm.GetValue("a_edit") <> "" Then
			CompanySiteParameter.CurrentAction = ObjForm.GetValue("a_edit") ' Get action code
			LoadFormValues() ' Get form values

			' Validate Form
			If Not ValidateForm() Then
				CompanySiteParameter.CurrentAction = "" ' Form error, reset action
				Message = ParentPage.gsFormError
				LoadRow() ' Restore row
				RestoreFormValues() ' Restore form values if validate failed
			End If
		Else
			CompanySiteParameter.CurrentAction = "I" ' Default action is display
		End If

		' Check if valid key
		If ew_Empty(CompanySiteParameter.CompanySiteParameterID.CurrentValue) Then Page_Terminate("CompanySiteParameter_list.aspx") ' Invalid key, return to list
		Select Case CompanySiteParameter.CurrentAction
			Case "I" ' Get a record to display
				If Not LoadRow() Then ' Load Record based on key
					Message = "No records found" ' No record found
					Page_Terminate("CompanySiteParameter_list.aspx") ' No matching record, return to list
				End If
			Case "U" ' Update
				CompanySiteParameter.SendEmail = True ' Send email on update success
				If EditRow() Then ' Update Record based on key
					Message = "Update succeeded" ' Update success
					Dim sReturnUrl As String = CompanySiteParameter.ReturnUrl
					If ew_GetPageName(sReturnUrl) = "CompanySiteParameter_view.aspx" Then sReturnUrl = CompanySiteParameter.ViewUrl ' View paging, return to view page with keyurl directly
					Page_Terminate(sReturnUrl) ' Return to caller
				Else
					LoadRow() ' Restore row
					RestoreFormValues() ' Restore form values if update failed
				End If
		End Select

		' Render the record
		CompanySiteParameter.RowType = EW_ROWTYPE_EDIT ' Render as edit

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
	End Sub

	'
	' Load form values
	'
	Sub LoadFormValues()
		CompanySiteParameter.CompanyID.FormValue = ObjForm.GetValue("x_CompanyID")
		CompanySiteParameter.CompanyID.OldValue = ObjForm.GetValue("o_CompanyID")
		CompanySiteParameter.zPageID.FormValue = ObjForm.GetValue("x_zPageID")
		CompanySiteParameter.zPageID.OldValue = ObjForm.GetValue("o_zPageID")
		CompanySiteParameter.SiteCategoryGroupID.FormValue = ObjForm.GetValue("x_SiteCategoryGroupID")
		CompanySiteParameter.SiteCategoryGroupID.OldValue = ObjForm.GetValue("o_SiteCategoryGroupID")
		CompanySiteParameter.SiteParameterTypeID.FormValue = ObjForm.GetValue("x_SiteParameterTypeID")
		CompanySiteParameter.SiteParameterTypeID.OldValue = ObjForm.GetValue("o_SiteParameterTypeID")
		CompanySiteParameter.SortOrder.FormValue = ObjForm.GetValue("x_SortOrder")
		CompanySiteParameter.SortOrder.OldValue = ObjForm.GetValue("o_SortOrder")
		CompanySiteParameter.ParameterValue.FormValue = ObjForm.GetValue("x_ParameterValue")
		CompanySiteParameter.ParameterValue.OldValue = ObjForm.GetValue("o_ParameterValue")
		CompanySiteParameter.CompanySiteParameterID.FormValue = ObjForm.GetValue("x_CompanySiteParameterID")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		CompanySiteParameter.CompanyID.CurrentValue = CompanySiteParameter.CompanyID.FormValue
		CompanySiteParameter.zPageID.CurrentValue = CompanySiteParameter.zPageID.FormValue
		CompanySiteParameter.SiteCategoryGroupID.CurrentValue = CompanySiteParameter.SiteCategoryGroupID.FormValue
		CompanySiteParameter.SiteParameterTypeID.CurrentValue = CompanySiteParameter.SiteParameterTypeID.FormValue
		CompanySiteParameter.SortOrder.CurrentValue = CompanySiteParameter.SortOrder.FormValue
		CompanySiteParameter.ParameterValue.CurrentValue = CompanySiteParameter.ParameterValue.FormValue
		CompanySiteParameter.CompanySiteParameterID.CurrentValue = CompanySiteParameter.CompanySiteParameterID.FormValue
	End Sub

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = CompanySiteParameter.KeyFilter

		' Row Selecting event
		CompanySiteParameter.Row_Selecting(sFilter)

		' Load SQL based on filter
		CompanySiteParameter.CurrentFilter = sFilter
		Dim sSql As String = CompanySiteParameter.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				CompanySiteParameter.Row_Selected(RsRow)
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
		CompanySiteParameter.CompanySiteParameterID.DbValue = RsRow("CompanySiteParameterID")
		CompanySiteParameter.CompanyID.DbValue = RsRow("CompanyID")
		CompanySiteParameter.zPageID.DbValue = RsRow("PageID")
		CompanySiteParameter.SiteCategoryGroupID.DbValue = RsRow("SiteCategoryGroupID")
		CompanySiteParameter.SiteParameterTypeID.DbValue = RsRow("SiteParameterTypeID")
		CompanySiteParameter.SortOrder.DbValue = RsRow("SortOrder")
		CompanySiteParameter.ParameterValue.DbValue = RsRow("ParameterValue")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		CompanySiteParameter.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' CompanyID

		CompanySiteParameter.CompanyID.CellCssStyle = ""
		CompanySiteParameter.CompanyID.CellCssClass = ""

		' PageID
		CompanySiteParameter.zPageID.CellCssStyle = ""
		CompanySiteParameter.zPageID.CellCssClass = ""

		' SiteCategoryGroupID
		CompanySiteParameter.SiteCategoryGroupID.CellCssStyle = ""
		CompanySiteParameter.SiteCategoryGroupID.CellCssClass = ""

		' SiteParameterTypeID
		CompanySiteParameter.SiteParameterTypeID.CellCssStyle = ""
		CompanySiteParameter.SiteParameterTypeID.CellCssClass = ""

		' SortOrder
		CompanySiteParameter.SortOrder.CellCssStyle = ""
		CompanySiteParameter.SortOrder.CellCssClass = ""

		' ParameterValue
		CompanySiteParameter.ParameterValue.CellCssStyle = ""
		CompanySiteParameter.ParameterValue.CellCssClass = ""

		'
		'  View  Row
		'

		If CompanySiteParameter.RowType = EW_ROWTYPE_VIEW Then ' View row

			' CompanyID
			If ew_NotEmpty(CompanySiteParameter.CompanyID.CurrentValue) Then
				sSqlWrk = "SELECT [CompanyName] FROM [Company] WHERE [CompanyID] = " & ew_AdjustSql(CompanySiteParameter.CompanyID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanySiteParameter.CompanyID.ViewValue = RsWrk("CompanyName")
				Else
					CompanySiteParameter.CompanyID.ViewValue = CompanySiteParameter.CompanyID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanySiteParameter.CompanyID.ViewValue = System.DBNull.Value
			End If
			CompanySiteParameter.CompanyID.CssStyle = ""
			CompanySiteParameter.CompanyID.CssClass = ""
			CompanySiteParameter.CompanyID.ViewCustomAttributes = ""

			' PageID
			If ew_NotEmpty(CompanySiteParameter.zPageID.CurrentValue) Then
				sSqlWrk = "SELECT [PageName] FROM [Page] WHERE [PageID] = " & ew_AdjustSql(CompanySiteParameter.zPageID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [PageName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanySiteParameter.zPageID.ViewValue = RsWrk("PageName")
				Else
					CompanySiteParameter.zPageID.ViewValue = CompanySiteParameter.zPageID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanySiteParameter.zPageID.ViewValue = System.DBNull.Value
			End If
			CompanySiteParameter.zPageID.CssStyle = ""
			CompanySiteParameter.zPageID.CssClass = ""
			CompanySiteParameter.zPageID.ViewCustomAttributes = ""

			' SiteCategoryGroupID
			If ew_NotEmpty(CompanySiteParameter.SiteCategoryGroupID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteCategoryGroupNM] FROM [SiteCategoryGroup] WHERE [SiteCategoryGroupID] = " & ew_AdjustSql(CompanySiteParameter.SiteCategoryGroupID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryGroupNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanySiteParameter.SiteCategoryGroupID.ViewValue = RsWrk("SiteCategoryGroupNM")
				Else
					CompanySiteParameter.SiteCategoryGroupID.ViewValue = CompanySiteParameter.SiteCategoryGroupID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanySiteParameter.SiteCategoryGroupID.ViewValue = System.DBNull.Value
			End If
			CompanySiteParameter.SiteCategoryGroupID.CssStyle = ""
			CompanySiteParameter.SiteCategoryGroupID.CssClass = ""
			CompanySiteParameter.SiteCategoryGroupID.ViewCustomAttributes = ""

			' SiteParameterTypeID
			If ew_NotEmpty(CompanySiteParameter.SiteParameterTypeID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteParameterTypeNM] FROM [SiteParameterType] WHERE [SiteParameterTypeID] = " & ew_AdjustSql(CompanySiteParameter.SiteParameterTypeID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteParameterTypeNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanySiteParameter.SiteParameterTypeID.ViewValue = RsWrk("SiteParameterTypeNM")
				Else
					CompanySiteParameter.SiteParameterTypeID.ViewValue = CompanySiteParameter.SiteParameterTypeID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanySiteParameter.SiteParameterTypeID.ViewValue = System.DBNull.Value
			End If
			CompanySiteParameter.SiteParameterTypeID.CssStyle = ""
			CompanySiteParameter.SiteParameterTypeID.CssClass = ""
			CompanySiteParameter.SiteParameterTypeID.ViewCustomAttributes = ""

			' SortOrder
			CompanySiteParameter.SortOrder.ViewValue = CompanySiteParameter.SortOrder.CurrentValue
			CompanySiteParameter.SortOrder.CssStyle = ""
			CompanySiteParameter.SortOrder.CssClass = ""
			CompanySiteParameter.SortOrder.ViewCustomAttributes = ""

			' ParameterValue
			CompanySiteParameter.ParameterValue.ViewValue = CompanySiteParameter.ParameterValue.CurrentValue
			CompanySiteParameter.ParameterValue.CssStyle = ""
			CompanySiteParameter.ParameterValue.CssClass = ""
			CompanySiteParameter.ParameterValue.ViewCustomAttributes = ""

			' View refer script
			' CompanyID

			CompanySiteParameter.CompanyID.HrefValue = ""

			' PageID
			CompanySiteParameter.zPageID.HrefValue = ""

			' SiteCategoryGroupID
			CompanySiteParameter.SiteCategoryGroupID.HrefValue = ""

			' SiteParameterTypeID
			CompanySiteParameter.SiteParameterTypeID.HrefValue = ""

			' SortOrder
			CompanySiteParameter.SortOrder.HrefValue = ""

			' ParameterValue
			CompanySiteParameter.ParameterValue.HrefValue = ""

		'
		'  Edit Row
		'

		ElseIf CompanySiteParameter.RowType = EW_ROWTYPE_EDIT Then ' Edit row

			' CompanyID
			CompanySiteParameter.CompanyID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [CompanyID], [CompanyName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Company]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			CompanySiteParameter.CompanyID.EditValue = arwrk

			' PageID
			CompanySiteParameter.zPageID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [PageID], [PageName], '' AS Disp2Fld, [CompanyID] FROM [Page]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [PageName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select", ""}) 
			CompanySiteParameter.zPageID.EditValue = arwrk

			' SiteCategoryGroupID
			CompanySiteParameter.SiteCategoryGroupID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryGroupID], [SiteCategoryGroupNM], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteCategoryGroup]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryGroupNM] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			CompanySiteParameter.SiteCategoryGroupID.EditValue = arwrk

			' SiteParameterTypeID
			CompanySiteParameter.SiteParameterTypeID.EditCustomAttributes = ""
			If CompanySiteParameter.SiteParameterTypeID.SessionValue <> "" Then
				CompanySiteParameter.SiteParameterTypeID.CurrentValue = CompanySiteParameter.SiteParameterTypeID.SessionValue
			If ew_NotEmpty(CompanySiteParameter.SiteParameterTypeID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteParameterTypeNM] FROM [SiteParameterType] WHERE [SiteParameterTypeID] = " & ew_AdjustSql(CompanySiteParameter.SiteParameterTypeID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteParameterTypeNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanySiteParameter.SiteParameterTypeID.ViewValue = RsWrk("SiteParameterTypeNM")
				Else
					CompanySiteParameter.SiteParameterTypeID.ViewValue = CompanySiteParameter.SiteParameterTypeID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanySiteParameter.SiteParameterTypeID.ViewValue = System.DBNull.Value
			End If
			CompanySiteParameter.SiteParameterTypeID.CssStyle = ""
			CompanySiteParameter.SiteParameterTypeID.CssClass = ""
			CompanySiteParameter.SiteParameterTypeID.ViewCustomAttributes = ""
			Else
			sSqlWrk = "SELECT [SiteParameterTypeID], [SiteParameterTypeNM], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteParameterType]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [SiteParameterTypeNM] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			CompanySiteParameter.SiteParameterTypeID.EditValue = arwrk
			End If

			' SortOrder
			CompanySiteParameter.SortOrder.EditCustomAttributes = ""
			CompanySiteParameter.SortOrder.EditValue = ew_HtmlEncode(CompanySiteParameter.SortOrder.CurrentValue)

			' ParameterValue
			CompanySiteParameter.ParameterValue.EditCustomAttributes = ""
			CompanySiteParameter.ParameterValue.EditValue = ew_HtmlEncode(CompanySiteParameter.ParameterValue.CurrentValue)

			' Edit refer script
			' CompanyID

			CompanySiteParameter.CompanyID.HrefValue = ""

			' PageID
			CompanySiteParameter.zPageID.HrefValue = ""

			' SiteCategoryGroupID
			CompanySiteParameter.SiteCategoryGroupID.HrefValue = ""

			' SiteParameterTypeID
			CompanySiteParameter.SiteParameterTypeID.HrefValue = ""

			' SortOrder
			CompanySiteParameter.SortOrder.HrefValue = ""

			' ParameterValue
			CompanySiteParameter.ParameterValue.HrefValue = ""
		End If

		' Row Rendered event
		CompanySiteParameter.Row_Rendered()
	End Sub

	'
	' Validate form
	'
	Function ValidateForm() As Boolean

		' Initialize
		ParentPage.gsFormError = ""

		' Check if validation required
		If Not EW_SERVER_VALIDATE Then Return (ParentPage.gsFormError = "")
		If ew_Empty(CompanySiteParameter.CompanyID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter required field - Site"
		End If
		If ew_Empty(CompanySiteParameter.SiteParameterTypeID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter required field -  Parameter"
		End If
		If Not ew_CheckInteger(CompanySiteParameter.SortOrder.FormValue) Then
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
	' Update record based on key values
	'
	Function EditRow() As Boolean
		Dim RsEdit As OleDbDataReader, RsChk As OleDbDataReader
		Dim sSql As String, sFilter As String
		Dim sSqlChk As String, sFilterChk As String
		Dim bUpdateRow As Boolean
		Dim RsOld As OrderedDictionary
		Dim sIdxErrMsg As String
		Dim Rs As New OrderedDictionary
		sFilter = CompanySiteParameter.KeyFilter
		CompanySiteParameter.CurrentFilter  = sFilter
		sSql = CompanySiteParameter.SQL
		Try
			RsEdit = Conn.GetDataReader(sSql) 
		Catch e As Exception
			If EW_DEBUG_ENABLED Then Throw
			Message = e.Message
			RsEdit.Close()
			EditRow = False
		End Try
		If Not RsEdit.Read() Then
			RsEdit.Close()
			EditRow = False ' Update Failed
		Else
			RsOld = Conn.GetRow(RsEdit)
			RsEdit.Close()

			' CompanyID
			CompanySiteParameter.CompanyID.SetDbValue(CompanySiteParameter.CompanyID.CurrentValue, 0)
			Rs("CompanyID") = CompanySiteParameter.CompanyID.DbValue

			' PageID
			CompanySiteParameter.zPageID.SetDbValue(CompanySiteParameter.zPageID.CurrentValue, System.DBNull.Value)
			Rs("PageID") = CompanySiteParameter.zPageID.DbValue

			' SiteCategoryGroupID
			CompanySiteParameter.SiteCategoryGroupID.SetDbValue(CompanySiteParameter.SiteCategoryGroupID.CurrentValue, System.DBNull.Value)
			Rs("SiteCategoryGroupID") = CompanySiteParameter.SiteCategoryGroupID.DbValue

			' SiteParameterTypeID
			CompanySiteParameter.SiteParameterTypeID.SetDbValue(CompanySiteParameter.SiteParameterTypeID.CurrentValue, 0)
			Rs("SiteParameterTypeID") = CompanySiteParameter.SiteParameterTypeID.DbValue

			' SortOrder
			CompanySiteParameter.SortOrder.SetDbValue(CompanySiteParameter.SortOrder.CurrentValue, System.DBNull.Value)
			Rs("SortOrder") = CompanySiteParameter.SortOrder.DbValue

			' ParameterValue
			CompanySiteParameter.ParameterValue.SetDbValue(CompanySiteParameter.ParameterValue.CurrentValue, System.DBNull.Value)
			Rs("ParameterValue") = CompanySiteParameter.ParameterValue.DbValue

			' Row Updating event
			bUpdateRow = CompanySiteParameter.Row_Updating(RsOld, Rs)
			If bUpdateRow Then
				Try
					CompanySiteParameter.Update(Rs)
					EditRow = True
				Catch e As Exception
					If EW_DEBUG_ENABLED Then Throw
					Message = e.Message
					EditRow = False
				End Try
			Else
				If CompanySiteParameter.CancelMessage <> "" Then
					Message = CompanySiteParameter.CancelMessage
					CompanySiteParameter.CancelMessage = ""
				Else
					Message = "Update cancelled"
				End If
				EditRow = False
			End If
		End If

		' Row Updated event
		If EditRow Then
			CompanySiteParameter.Row_Updated(RsOld, Rs)
		End If
		If EditRow Then
			WriteAuditTrailOnEdit(RsOld, Rs)
		End If
	End Function

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "CompanySiteParameter"
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

	' Write Audit Trail (edit page)
	Sub WriteAuditTrailOnEdit(ByRef RsOld As OrderedDictionary, ByRef RsNew As OrderedDictionary)
		Dim table As String = "CompanySiteParameter"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsOld("CompanySiteParameterID")

		' Write Audit Trail
		Dim filePfx As String = "log"
		Dim action As String = "U"
		Dim curDate As String, curTime As String, id As String, user As String, field As String
		Dim keyvalue As Object, oldvalue As Object, newvalue As Object
		Dim dt As DateTime = Now()
		Dim fld As cField, fldname As String, Modified As Boolean
		curDate = dt.ToString("yyyy/MM/dd")
		curTime = dt.ToString("HH:mm:ss")
		id = HttpContext.Current.Request.ServerVariables("SCRIPT_NAME")
    user = CurrentUserName()
		Dim arFldNames(RsNew.Count - 1) As String
		RsNew.Keys.CopyTo(arFldNames, 0)
		For Each fldname In arFldNames
			fld = CompanySiteParameter.FieldByName(fldname)
			If fld IsNot Nothing AndAlso fld.FldDataType <> EW_DATATYPE_BLOB AndAlso fld.FldDataType <> EW_DATATYPE_MEMO Then ' Ignore Blob/Memo Field
				oldvalue = RsOld(fldname)
				newvalue = RsNew(fldname)

				'oldvalue = ew_Conv(oldvalue, fld.FldType)
				'newvalue = ew_Conv(newvalue, fld.FldType)

				If fld.FldDataType = EW_DATATYPE_DATE Then ' DateTime Field
					Modified = Not ew_SameStr(ew_FormatDateTime(oldvalue, 8), ew_FormatDateTime(newvalue, 8))
				Else
					Modified = Not ew_CompareValue(oldvalue, newvalue)
				End If				
				If Modified Then					
					keyvalue = sKey
					ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, fldname, keyvalue, oldvalue, newvalue)
				End If
			End If
		Next
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
		CompanySiteParameter_edit = New cCompanySiteParameter_edit(Me)		
		CompanySiteParameter_edit.Page_Init()

		' Page main processing
		CompanySiteParameter_edit.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If CompanySiteParameter_edit IsNot Nothing Then CompanySiteParameter_edit.Dispose()
	End Sub
End Class
