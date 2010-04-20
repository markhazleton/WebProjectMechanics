Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class SiteCategory_add
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public SiteCategory_add As cSiteCategory_add

	'
	' Page Class
	'
	Class cSiteCategory_add
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
				If SiteCategory.UseTokenInUrl Then Url = Url & "t=" & SiteCategory.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If SiteCategory.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (SiteCategory.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (SiteCategory.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' SiteCategory
		Public Property SiteCategory() As cSiteCategory
			Get				
				Return ParentPage.SiteCategory
			End Get
			Set(ByVal v As cSiteCategory)
				ParentPage.SiteCategory = v	
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
			m_PageObjName = "SiteCategory_add"
			m_PageObjTypeName = "cSiteCategory_add"

			' Table Name
			m_TableName = "SiteCategory"

			' Initialize table object
			SiteCategory = New cSiteCategory(Me)

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
			SiteCategory.Dispose()

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
		If ew_Get("SiteCategoryID") <> "" Then
			SiteCategory.SiteCategoryID.QueryStringValue = ew_Get("SiteCategoryID")
		Else
			bCopy = False
		End If

		' Create form object
		ObjForm = New cFormObj

		' Process form if post back
		If ObjForm.GetValue("a_add") <> "" Then
			SiteCategory.CurrentAction = ObjForm.GetValue("a_add") ' Get form action
			LoadFormValues() ' Load form values

			' Validate Form
			If Not ValidateForm() Then
				SiteCategory.CurrentAction = "I" ' Form error, reset action
				Message = ParentPage.gsFormError
			End If

		' Not post back
		Else
			If bCopy Then
				SiteCategory.CurrentAction = "C" ' Copy Record
			Else
				SiteCategory.CurrentAction = "I" ' Display Blank Record
				LoadDefaultValues() ' Load default values
			End If
		End If

		' Perform action based on action code
		Select Case SiteCategory.CurrentAction
			Case "I" ' Blank record, no action required
			Case "C" ' Copy an existing record
				If Not LoadRow() Then ' Load record based on key
					Message = "No records found" ' No record found
					Page_Terminate("SiteCategory_list.aspx") ' No matching record, return to list
				End If
			Case "A" ' Add new record
				SiteCategory.SendEmail = True ' Send email on add success
				If AddRow() Then ' Add successful
					Message = "Add succeeded" ' Set up success message
					Dim sReturnUrl As String = SiteCategory.ReturnUrl
					If ew_GetPageName(sReturnUrl) = "SiteCategory_view.aspx" Then sReturnUrl = SiteCategory.ViewUrl ' View paging, return to view page with keyurl directly
					Page_Terminate(sReturnUrl) ' Clean up and return
				Else
					RestoreFormValues() ' Add failed, restore form values
				End If
		End Select

		' Render row based on row type
		SiteCategory.RowType = EW_ROWTYPE_ADD ' Render add type

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
		SiteCategory.SiteCategoryTypeID.FormValue = ObjForm.GetValue("x_SiteCategoryTypeID")
		SiteCategory.SiteCategoryTypeID.OldValue = ObjForm.GetValue("o_SiteCategoryTypeID")
		SiteCategory.GroupOrder.FormValue = ObjForm.GetValue("x_GroupOrder")
		SiteCategory.GroupOrder.OldValue = ObjForm.GetValue("o_GroupOrder")
		SiteCategory.CategoryName.FormValue = ObjForm.GetValue("x_CategoryName")
		SiteCategory.CategoryName.OldValue = ObjForm.GetValue("o_CategoryName")
		SiteCategory.CategoryTitle.FormValue = ObjForm.GetValue("x_CategoryTitle")
		SiteCategory.CategoryTitle.OldValue = ObjForm.GetValue("o_CategoryTitle")
		SiteCategory.CategoryDescription.FormValue = ObjForm.GetValue("x_CategoryDescription")
		SiteCategory.CategoryDescription.OldValue = ObjForm.GetValue("o_CategoryDescription")
		SiteCategory.CategoryKeywords.FormValue = ObjForm.GetValue("x_CategoryKeywords")
		SiteCategory.CategoryKeywords.OldValue = ObjForm.GetValue("o_CategoryKeywords")
		SiteCategory.ParentCategoryID.FormValue = ObjForm.GetValue("x_ParentCategoryID")
		SiteCategory.ParentCategoryID.OldValue = ObjForm.GetValue("o_ParentCategoryID")
		SiteCategory.CategoryFileName.FormValue = ObjForm.GetValue("x_CategoryFileName")
		SiteCategory.CategoryFileName.OldValue = ObjForm.GetValue("o_CategoryFileName")
		SiteCategory.SiteCategoryGroupID.FormValue = ObjForm.GetValue("x_SiteCategoryGroupID")
		SiteCategory.SiteCategoryGroupID.OldValue = ObjForm.GetValue("o_SiteCategoryGroupID")
		SiteCategory.SiteCategoryID.FormValue = ObjForm.GetValue("x_SiteCategoryID")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		SiteCategory.SiteCategoryTypeID.CurrentValue = SiteCategory.SiteCategoryTypeID.FormValue
		SiteCategory.GroupOrder.CurrentValue = SiteCategory.GroupOrder.FormValue
		SiteCategory.CategoryName.CurrentValue = SiteCategory.CategoryName.FormValue
		SiteCategory.CategoryTitle.CurrentValue = SiteCategory.CategoryTitle.FormValue
		SiteCategory.CategoryDescription.CurrentValue = SiteCategory.CategoryDescription.FormValue
		SiteCategory.CategoryKeywords.CurrentValue = SiteCategory.CategoryKeywords.FormValue
		SiteCategory.ParentCategoryID.CurrentValue = SiteCategory.ParentCategoryID.FormValue
		SiteCategory.CategoryFileName.CurrentValue = SiteCategory.CategoryFileName.FormValue
		SiteCategory.SiteCategoryGroupID.CurrentValue = SiteCategory.SiteCategoryGroupID.FormValue
		SiteCategory.SiteCategoryID.CurrentValue = SiteCategory.SiteCategoryID.FormValue
	End Sub

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = SiteCategory.KeyFilter

		' Row Selecting event
		SiteCategory.Row_Selecting(sFilter)

		' Load SQL based on filter
		SiteCategory.CurrentFilter = sFilter
		Dim sSql As String = SiteCategory.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				SiteCategory.Row_Selected(RsRow)
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
		SiteCategory.SiteCategoryID.DbValue = RsRow("SiteCategoryID")
		SiteCategory.SiteCategoryTypeID.DbValue = RsRow("SiteCategoryTypeID")
		SiteCategory.GroupOrder.DbValue = RsRow("GroupOrder")
		SiteCategory.CategoryName.DbValue = RsRow("CategoryName")
		SiteCategory.CategoryTitle.DbValue = RsRow("CategoryTitle")
		SiteCategory.CategoryDescription.DbValue = RsRow("CategoryDescription")
		SiteCategory.CategoryKeywords.DbValue = RsRow("CategoryKeywords")
		SiteCategory.ParentCategoryID.DbValue = RsRow("ParentCategoryID")
		SiteCategory.CategoryFileName.DbValue = RsRow("CategoryFileName")
		SiteCategory.SiteCategoryGroupID.DbValue = RsRow("SiteCategoryGroupID")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		SiteCategory.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' SiteCategoryTypeID

		SiteCategory.SiteCategoryTypeID.CellCssStyle = ""
		SiteCategory.SiteCategoryTypeID.CellCssClass = ""

		' GroupOrder
		SiteCategory.GroupOrder.CellCssStyle = ""
		SiteCategory.GroupOrder.CellCssClass = ""

		' CategoryName
		SiteCategory.CategoryName.CellCssStyle = ""
		SiteCategory.CategoryName.CellCssClass = ""

		' CategoryTitle
		SiteCategory.CategoryTitle.CellCssStyle = ""
		SiteCategory.CategoryTitle.CellCssClass = ""

		' CategoryDescription
		SiteCategory.CategoryDescription.CellCssStyle = ""
		SiteCategory.CategoryDescription.CellCssClass = ""

		' CategoryKeywords
		SiteCategory.CategoryKeywords.CellCssStyle = ""
		SiteCategory.CategoryKeywords.CellCssClass = ""

		' ParentCategoryID
		SiteCategory.ParentCategoryID.CellCssStyle = ""
		SiteCategory.ParentCategoryID.CellCssClass = ""

		' CategoryFileName
		SiteCategory.CategoryFileName.CellCssStyle = ""
		SiteCategory.CategoryFileName.CellCssClass = ""

		' SiteCategoryGroupID
		SiteCategory.SiteCategoryGroupID.CellCssStyle = ""
		SiteCategory.SiteCategoryGroupID.CellCssClass = ""

		'
		'  View  Row
		'

		If SiteCategory.RowType = EW_ROWTYPE_VIEW Then ' View row

			' SiteCategoryTypeID
			If ew_NotEmpty(SiteCategory.SiteCategoryTypeID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteCategoryTypeNM] FROM [SiteCategoryType] WHERE [SiteCategoryTypeID] = " & ew_AdjustSql(SiteCategory.SiteCategoryTypeID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryTypeNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					SiteCategory.SiteCategoryTypeID.ViewValue = RsWrk("SiteCategoryTypeNM")
				Else
					SiteCategory.SiteCategoryTypeID.ViewValue = SiteCategory.SiteCategoryTypeID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				SiteCategory.SiteCategoryTypeID.ViewValue = System.DBNull.Value
			End If
			SiteCategory.SiteCategoryTypeID.CssStyle = ""
			SiteCategory.SiteCategoryTypeID.CssClass = ""
			SiteCategory.SiteCategoryTypeID.ViewCustomAttributes = ""

			' GroupOrder
			SiteCategory.GroupOrder.ViewValue = SiteCategory.GroupOrder.CurrentValue
			SiteCategory.GroupOrder.CssStyle = ""
			SiteCategory.GroupOrder.CssClass = ""
			SiteCategory.GroupOrder.ViewCustomAttributes = ""

			' CategoryName
			SiteCategory.CategoryName.ViewValue = SiteCategory.CategoryName.CurrentValue
			SiteCategory.CategoryName.CssStyle = ""
			SiteCategory.CategoryName.CssClass = ""
			SiteCategory.CategoryName.ViewCustomAttributes = ""

			' CategoryTitle
			SiteCategory.CategoryTitle.ViewValue = SiteCategory.CategoryTitle.CurrentValue
			SiteCategory.CategoryTitle.CssStyle = ""
			SiteCategory.CategoryTitle.CssClass = ""
			SiteCategory.CategoryTitle.ViewCustomAttributes = ""

			' CategoryDescription
			SiteCategory.CategoryDescription.ViewValue = SiteCategory.CategoryDescription.CurrentValue
			SiteCategory.CategoryDescription.CssStyle = ""
			SiteCategory.CategoryDescription.CssClass = ""
			SiteCategory.CategoryDescription.ViewCustomAttributes = ""

			' CategoryKeywords
			SiteCategory.CategoryKeywords.ViewValue = SiteCategory.CategoryKeywords.CurrentValue
			SiteCategory.CategoryKeywords.CssStyle = ""
			SiteCategory.CategoryKeywords.CssClass = ""
			SiteCategory.CategoryKeywords.ViewCustomAttributes = ""

			' ParentCategoryID
			If ew_NotEmpty(SiteCategory.ParentCategoryID.CurrentValue) Then
				sSqlWrk = "SELECT [CategoryName] FROM [SiteCategory] WHERE [SiteCategoryID] = " & ew_AdjustSql(SiteCategory.ParentCategoryID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [CategoryName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					SiteCategory.ParentCategoryID.ViewValue = RsWrk("CategoryName")
				Else
					SiteCategory.ParentCategoryID.ViewValue = SiteCategory.ParentCategoryID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				SiteCategory.ParentCategoryID.ViewValue = System.DBNull.Value
			End If
			SiteCategory.ParentCategoryID.CssStyle = ""
			SiteCategory.ParentCategoryID.CssClass = ""
			SiteCategory.ParentCategoryID.ViewCustomAttributes = ""

			' CategoryFileName
			SiteCategory.CategoryFileName.ViewValue = SiteCategory.CategoryFileName.CurrentValue
			SiteCategory.CategoryFileName.CssStyle = ""
			SiteCategory.CategoryFileName.CssClass = ""
			SiteCategory.CategoryFileName.ViewCustomAttributes = ""

			' SiteCategoryGroupID
			If ew_NotEmpty(SiteCategory.SiteCategoryGroupID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteCategoryGroupNM] FROM [SiteCategoryGroup] WHERE [SiteCategoryGroupID] = " & ew_AdjustSql(SiteCategory.SiteCategoryGroupID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryGroupNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					SiteCategory.SiteCategoryGroupID.ViewValue = RsWrk("SiteCategoryGroupNM")
				Else
					SiteCategory.SiteCategoryGroupID.ViewValue = SiteCategory.SiteCategoryGroupID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				SiteCategory.SiteCategoryGroupID.ViewValue = System.DBNull.Value
			End If
			SiteCategory.SiteCategoryGroupID.CssStyle = ""
			SiteCategory.SiteCategoryGroupID.CssClass = ""
			SiteCategory.SiteCategoryGroupID.ViewCustomAttributes = ""

			' View refer script
			' SiteCategoryTypeID

			SiteCategory.SiteCategoryTypeID.HrefValue = ""

			' GroupOrder
			SiteCategory.GroupOrder.HrefValue = ""

			' CategoryName
			SiteCategory.CategoryName.HrefValue = ""

			' CategoryTitle
			SiteCategory.CategoryTitle.HrefValue = ""

			' CategoryDescription
			SiteCategory.CategoryDescription.HrefValue = ""

			' CategoryKeywords
			SiteCategory.CategoryKeywords.HrefValue = ""

			' ParentCategoryID
			SiteCategory.ParentCategoryID.HrefValue = ""

			' CategoryFileName
			SiteCategory.CategoryFileName.HrefValue = ""

			' SiteCategoryGroupID
			SiteCategory.SiteCategoryGroupID.HrefValue = ""

		'
		'  Add Row
		'

		ElseIf SiteCategory.RowType = EW_ROWTYPE_ADD Then ' Add row

			' SiteCategoryTypeID
			SiteCategory.SiteCategoryTypeID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryTypeID], [SiteCategoryTypeNM], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteCategoryType]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryTypeNM] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			SiteCategory.SiteCategoryTypeID.EditValue = arwrk

			' GroupOrder
			SiteCategory.GroupOrder.EditCustomAttributes = ""
			SiteCategory.GroupOrder.EditValue = ew_HtmlEncode(SiteCategory.GroupOrder.CurrentValue)

			' CategoryName
			SiteCategory.CategoryName.EditCustomAttributes = ""
			SiteCategory.CategoryName.EditValue = ew_HtmlEncode(SiteCategory.CategoryName.CurrentValue)

			' CategoryTitle
			SiteCategory.CategoryTitle.EditCustomAttributes = ""
			SiteCategory.CategoryTitle.EditValue = ew_HtmlEncode(SiteCategory.CategoryTitle.CurrentValue)

			' CategoryDescription
			SiteCategory.CategoryDescription.EditCustomAttributes = ""
			SiteCategory.CategoryDescription.EditValue = ew_HtmlEncode(SiteCategory.CategoryDescription.CurrentValue)

			' CategoryKeywords
			SiteCategory.CategoryKeywords.EditCustomAttributes = ""
			SiteCategory.CategoryKeywords.EditValue = ew_HtmlEncode(SiteCategory.CategoryKeywords.CurrentValue)

			' ParentCategoryID
			SiteCategory.ParentCategoryID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryID], [CategoryName], '' AS Disp2Fld, [SiteCategoryTypeID] FROM [SiteCategory]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [CategoryName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select", ""}) 
			SiteCategory.ParentCategoryID.EditValue = arwrk

			' CategoryFileName
			SiteCategory.CategoryFileName.EditCustomAttributes = ""
			SiteCategory.CategoryFileName.EditValue = ew_HtmlEncode(SiteCategory.CategoryFileName.CurrentValue)

			' SiteCategoryGroupID
			SiteCategory.SiteCategoryGroupID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryGroupID], [SiteCategoryGroupNM], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteCategoryGroup]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryGroupNM] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			SiteCategory.SiteCategoryGroupID.EditValue = arwrk
		End If

		' Row Rendered event
		SiteCategory.Row_Rendered()
	End Sub

	'
	' Validate form
	'
	Function ValidateForm() As Boolean

		' Initialize
		ParentPage.gsFormError = ""

		' Check if validation required
		If Not EW_SERVER_VALIDATE Then Return (ParentPage.gsFormError = "")
		If ew_Empty(SiteCategory.SiteCategoryTypeID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter required field - Site Type"
		End If
		If Not ew_CheckNumber(SiteCategory.GroupOrder.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Incorrect floating point number - Order"
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

		' SiteCategoryTypeID
		SiteCategory.SiteCategoryTypeID.SetDbValue(SiteCategory.SiteCategoryTypeID.CurrentValue, System.DBNull.Value)
		Rs("SiteCategoryTypeID") = SiteCategory.SiteCategoryTypeID.DbValue

		' GroupOrder
		SiteCategory.GroupOrder.SetDbValue(SiteCategory.GroupOrder.CurrentValue, System.DBNull.Value)
		Rs("GroupOrder") = SiteCategory.GroupOrder.DbValue

		' CategoryName
		SiteCategory.CategoryName.SetDbValue(SiteCategory.CategoryName.CurrentValue, System.DBNull.Value)
		Rs("CategoryName") = SiteCategory.CategoryName.DbValue

		' CategoryTitle
		SiteCategory.CategoryTitle.SetDbValue(SiteCategory.CategoryTitle.CurrentValue, System.DBNull.Value)
		Rs("CategoryTitle") = SiteCategory.CategoryTitle.DbValue

		' CategoryDescription
		SiteCategory.CategoryDescription.SetDbValue(SiteCategory.CategoryDescription.CurrentValue, System.DBNull.Value)
		Rs("CategoryDescription") = SiteCategory.CategoryDescription.DbValue

		' CategoryKeywords
		SiteCategory.CategoryKeywords.SetDbValue(SiteCategory.CategoryKeywords.CurrentValue, System.DBNull.Value)
		Rs("CategoryKeywords") = SiteCategory.CategoryKeywords.DbValue

		' ParentCategoryID
		SiteCategory.ParentCategoryID.SetDbValue(SiteCategory.ParentCategoryID.CurrentValue, System.DBNull.Value)
		Rs("ParentCategoryID") = SiteCategory.ParentCategoryID.DbValue

		' CategoryFileName
		SiteCategory.CategoryFileName.SetDbValue(SiteCategory.CategoryFileName.CurrentValue, System.DBNull.Value)
		Rs("CategoryFileName") = SiteCategory.CategoryFileName.DbValue

		' SiteCategoryGroupID
		SiteCategory.SiteCategoryGroupID.SetDbValue(SiteCategory.SiteCategoryGroupID.CurrentValue, System.DBNull.Value)
		Rs("SiteCategoryGroupID") = SiteCategory.SiteCategoryGroupID.DbValue

		' Row Inserting event
		bInsertRow = SiteCategory.Row_Inserting(Rs)
		If bInsertRow Then
			Try	
				SiteCategory.Insert(Rs)
				AddRow = True
			Catch e As Exception
				If EW_DEBUG_ENABLED Then Throw
				Message = e.Message				
				AddRow = False
			End Try
		Else
			If SiteCategory.CancelMessage <> "" Then
				Message = SiteCategory.CancelMessage
				SiteCategory.CancelMessage = ""
			Else
				Message = "Insert cancelled"
			End If
			AddRow = False
		End If
		If AddRow Then
			LastInsertId = Conn.GetLastInsertId()
			SiteCategory.SiteCategoryID.DbValue = LastInsertId
			Rs("SiteCategoryID") = SiteCategory.SiteCategoryID.DbValue		

			' Row Inserted event
			SiteCategory.Row_Inserted(Rs)
			WriteAuditTrailOnAdd(Rs)
		End If
	End Function

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "SiteCategory"
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
		Dim table As String = "SiteCategory"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsSrc("SiteCategoryID")

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

		' SiteCategoryTypeID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteCategoryTypeID", keyvalue, oldvalue, RsSrc("SiteCategoryTypeID"))

		' GroupOrder Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "GroupOrder", keyvalue, oldvalue, RsSrc("GroupOrder"))

		' CategoryName Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "CategoryName", keyvalue, oldvalue, RsSrc("CategoryName"))

		' CategoryTitle Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "CategoryTitle", keyvalue, oldvalue, RsSrc("CategoryTitle"))

		' CategoryDescription Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "CategoryDescription", keyvalue, oldvalue, RsSrc("CategoryDescription"))

		' CategoryKeywords Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "CategoryKeywords", keyvalue, oldvalue, RsSrc("CategoryKeywords"))

		' ParentCategoryID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "ParentCategoryID", keyvalue, oldvalue, RsSrc("ParentCategoryID"))

		' CategoryFileName Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "CategoryFileName", keyvalue, oldvalue, RsSrc("CategoryFileName"))

		' SiteCategoryGroupID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteCategoryGroupID", keyvalue, oldvalue, RsSrc("SiteCategoryGroupID"))
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
		SiteCategory_add = New cSiteCategory_add(Me)		
		SiteCategory_add.Page_Init()

		' Page main processing
		SiteCategory_add.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If SiteCategory_add IsNot Nothing Then SiteCategory_add.Dispose()
	End Sub
End Class
