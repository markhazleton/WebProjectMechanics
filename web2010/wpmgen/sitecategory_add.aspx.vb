Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class sitecategory_add
	Inherits AspNetMaker8_wpmWebsite

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

		Private sFilterWrk As String

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

		' Common urls
		Public AddUrl As String = ""

		Public EditUrl As String = ""

		Public CopyUrl As String = ""

		Public DeleteUrl As String = ""

		Public ViewUrl As String = ""

		Public ListUrl As String = ""

		' Export urls
		Public ExportPrintUrl As String = ""

		Public ExportHtmlUrl As String = ""

		Public ExportExcelUrl As String = ""

		Public ExportWordUrl As String = ""

		Public ExportXmlUrl As String = ""

		Public ExportCsvUrl As String = ""

		' Inline urls
		Public InlineAddUrl As String = ""

		Public InlineCopyUrl As String = ""

		Public InlineEditUrl As String = ""

		Public GridAddUrl As String = ""

		Public GridEditUrl As String = ""

		Public MultiDeleteUrl As String = ""

		Public MultiUpdateUrl As String = ""
		Protected m_DebugMsg As String = ""

		Public Property DebugMsg() As String
			Get
				Return IIf(m_DebugMsg <> "", "<p>" & m_DebugMsg & "</p>", m_DebugMsg)
			End Get
			Set(ByVal v As String)
				If m_DebugMsg <> "" Then ' Append
					m_DebugMsg = m_DebugMsg & "<br />" & v
				Else
					m_DebugMsg = v
				End If
			End Set
		End Property

		' Message
		Public Property Message() As String
			Get
				Return ew_Session(EW_SESSION_MESSAGE)
			End Get
			Set(ByVal v As String)
				If ew_NotEmpty(ew_Session(EW_SESSION_MESSAGE)) Then
					If Not ew_SameStr(ew_Session(EW_SESSION_MESSAGE), v) Then ' Append
						ew_Session(EW_SESSION_MESSAGE) = ew_Session(EW_SESSION_MESSAGE) & "<br>" & v
					End If
				Else
					ew_Session(EW_SESSION_MESSAGE) = v
				End If
			End Set	
		End Property

		' Show Message
		Public Sub ShowMessage()
			Dim sMessage As String
			sMessage = Message
			Message_Showing(sMessage)
			If sMessage <> "" Then ew_Write("<p><span class=""ewMessage"">" & sMessage & "</span></p>")
			ew_Session(EW_SESSION_MESSAGE) = "" ' Clear message in Session
		End Sub			

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

		' ASP.NET page object
		Public ReadOnly Property AspNetPage() As sitecategory_add
			Get
				Return CType(m_ParentPage, sitecategory_add)
			End Get
		End Property

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
		Public Sub New(ByRef APage As AspNetMaker8_wpmWebsite)				
			m_ParentPage = APage
			m_Page = Me
			m_PageID = "add"
			m_PageObjName = "SiteCategory_add"
			m_PageObjTypeName = "cSiteCategory_add"			

			' Initialize language object
			Language = New cLanguage(Me)

			' Table Name
			m_TableName = "SiteCategory"

			' Initialize table object
			SiteCategory = New cSiteCategory(Me)

			' Initialize URLs
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

		' Create form object
		ObjForm = New cFormObj()

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
			ObjForm = Nothing

			' Go to URL if specified
			Dim sRedirectUrl As String = url
			Page_Redirecting(sRedirectUrl)
			If sRedirectUrl <> "" Then
				HttpContext.Current.Response.Clear()
				HttpContext.Current.Response.Redirect(sReDirectUrl)
			End If
		End Sub

	Public sDbMasterFilter As String, sDbDetailFilter As String

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
					Message = Language.Phrase("NoRecord") ' No record found
					Page_Terminate("sitecategory_list.aspx") ' No matching record, return to list
				End If
			Case "A" ' Add new record
				SiteCategory.SendEmail = True ' Send email on add success
				If AddRow() Then ' Add successful
					Message = Language.Phrase("AddSuccess") ' Set up success message
					Dim sReturnUrl As String = SiteCategory.ReturnUrl
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
		SiteCategory.CategoryKeywords.FormValue = ObjForm.GetValue("x_CategoryKeywords")
		SiteCategory.CategoryKeywords.OldValue = ObjForm.GetValue("o_CategoryKeywords")
		SiteCategory.CategoryName.FormValue = ObjForm.GetValue("x_CategoryName")
		SiteCategory.CategoryName.OldValue = ObjForm.GetValue("o_CategoryName")
		SiteCategory.CategoryTitle.FormValue = ObjForm.GetValue("x_CategoryTitle")
		SiteCategory.CategoryTitle.OldValue = ObjForm.GetValue("o_CategoryTitle")
		SiteCategory.CategoryDescription.FormValue = ObjForm.GetValue("x_CategoryDescription")
		SiteCategory.CategoryDescription.OldValue = ObjForm.GetValue("o_CategoryDescription")
		SiteCategory.GroupOrder.FormValue = ObjForm.GetValue("x_GroupOrder")
		SiteCategory.GroupOrder.OldValue = ObjForm.GetValue("o_GroupOrder")
		SiteCategory.ParentCategoryID.FormValue = ObjForm.GetValue("x_ParentCategoryID")
		SiteCategory.ParentCategoryID.OldValue = ObjForm.GetValue("o_ParentCategoryID")
		SiteCategory.CategoryFileName.FormValue = ObjForm.GetValue("x_CategoryFileName")
		SiteCategory.CategoryFileName.OldValue = ObjForm.GetValue("o_CategoryFileName")
		SiteCategory.SiteCategoryTypeID.FormValue = ObjForm.GetValue("x_SiteCategoryTypeID")
		SiteCategory.SiteCategoryTypeID.OldValue = ObjForm.GetValue("o_SiteCategoryTypeID")
		SiteCategory.SiteCategoryGroupID.FormValue = ObjForm.GetValue("x_SiteCategoryGroupID")
		SiteCategory.SiteCategoryGroupID.OldValue = ObjForm.GetValue("o_SiteCategoryGroupID")
		SiteCategory.SiteCategoryID.FormValue = ObjForm.GetValue("x_SiteCategoryID")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		SiteCategory.CategoryKeywords.CurrentValue = SiteCategory.CategoryKeywords.FormValue
		SiteCategory.CategoryName.CurrentValue = SiteCategory.CategoryName.FormValue
		SiteCategory.CategoryTitle.CurrentValue = SiteCategory.CategoryTitle.FormValue
		SiteCategory.CategoryDescription.CurrentValue = SiteCategory.CategoryDescription.FormValue
		SiteCategory.GroupOrder.CurrentValue = SiteCategory.GroupOrder.FormValue
		SiteCategory.ParentCategoryID.CurrentValue = SiteCategory.ParentCategoryID.FormValue
		SiteCategory.CategoryFileName.CurrentValue = SiteCategory.CategoryFileName.FormValue
		SiteCategory.SiteCategoryTypeID.CurrentValue = SiteCategory.SiteCategoryTypeID.FormValue
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
		If EW_DEBUG_ENABLED Then DebugMsg = sSql
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
		Dim sDetailFilter As String
		SiteCategory.SiteCategoryID.DbValue = RsRow("SiteCategoryID")
		SiteCategory.CategoryKeywords.DbValue = RsRow("CategoryKeywords")
		SiteCategory.CategoryName.DbValue = RsRow("CategoryName")
		SiteCategory.CategoryTitle.DbValue = RsRow("CategoryTitle")
		SiteCategory.CategoryDescription.DbValue = RsRow("CategoryDescription")
		SiteCategory.GroupOrder.DbValue = RsRow("GroupOrder")
		SiteCategory.ParentCategoryID.DbValue = RsRow("ParentCategoryID")
		SiteCategory.CategoryFileName.DbValue = RsRow("CategoryFileName")
		SiteCategory.SiteCategoryTypeID.DbValue = RsRow("SiteCategoryTypeID")
		SiteCategory.SiteCategoryGroupID.DbValue = RsRow("SiteCategoryGroupID")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Initialize urls
		' Row Rendering event

		SiteCategory.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' CategoryKeywords

		SiteCategory.CategoryKeywords.CellCssStyle = ""
		SiteCategory.CategoryKeywords.CellCssClass = ""
		SiteCategory.CategoryKeywords.CellAttrs.Clear(): SiteCategory.CategoryKeywords.ViewAttrs.Clear(): SiteCategory.CategoryKeywords.EditAttrs.Clear()

		' CategoryName
		SiteCategory.CategoryName.CellCssStyle = ""
		SiteCategory.CategoryName.CellCssClass = ""
		SiteCategory.CategoryName.CellAttrs.Clear(): SiteCategory.CategoryName.ViewAttrs.Clear(): SiteCategory.CategoryName.EditAttrs.Clear()

		' CategoryTitle
		SiteCategory.CategoryTitle.CellCssStyle = ""
		SiteCategory.CategoryTitle.CellCssClass = ""
		SiteCategory.CategoryTitle.CellAttrs.Clear(): SiteCategory.CategoryTitle.ViewAttrs.Clear(): SiteCategory.CategoryTitle.EditAttrs.Clear()

		' CategoryDescription
		SiteCategory.CategoryDescription.CellCssStyle = ""
		SiteCategory.CategoryDescription.CellCssClass = ""
		SiteCategory.CategoryDescription.CellAttrs.Clear(): SiteCategory.CategoryDescription.ViewAttrs.Clear(): SiteCategory.CategoryDescription.EditAttrs.Clear()

		' GroupOrder
		SiteCategory.GroupOrder.CellCssStyle = ""
		SiteCategory.GroupOrder.CellCssClass = ""
		SiteCategory.GroupOrder.CellAttrs.Clear(): SiteCategory.GroupOrder.ViewAttrs.Clear(): SiteCategory.GroupOrder.EditAttrs.Clear()

		' ParentCategoryID
		SiteCategory.ParentCategoryID.CellCssStyle = ""
		SiteCategory.ParentCategoryID.CellCssClass = ""
		SiteCategory.ParentCategoryID.CellAttrs.Clear(): SiteCategory.ParentCategoryID.ViewAttrs.Clear(): SiteCategory.ParentCategoryID.EditAttrs.Clear()

		' CategoryFileName
		SiteCategory.CategoryFileName.CellCssStyle = ""
		SiteCategory.CategoryFileName.CellCssClass = ""
		SiteCategory.CategoryFileName.CellAttrs.Clear(): SiteCategory.CategoryFileName.ViewAttrs.Clear(): SiteCategory.CategoryFileName.EditAttrs.Clear()

		' SiteCategoryTypeID
		SiteCategory.SiteCategoryTypeID.CellCssStyle = ""
		SiteCategory.SiteCategoryTypeID.CellCssClass = ""
		SiteCategory.SiteCategoryTypeID.CellAttrs.Clear(): SiteCategory.SiteCategoryTypeID.ViewAttrs.Clear(): SiteCategory.SiteCategoryTypeID.EditAttrs.Clear()

		' SiteCategoryGroupID
		SiteCategory.SiteCategoryGroupID.CellCssStyle = ""
		SiteCategory.SiteCategoryGroupID.CellCssClass = ""
		SiteCategory.SiteCategoryGroupID.CellAttrs.Clear(): SiteCategory.SiteCategoryGroupID.ViewAttrs.Clear(): SiteCategory.SiteCategoryGroupID.EditAttrs.Clear()

		'
		'  View  Row
		'

		If SiteCategory.RowType = EW_ROWTYPE_VIEW Then ' View row

			' SiteCategoryID
			SiteCategory.SiteCategoryID.ViewValue = SiteCategory.SiteCategoryID.CurrentValue
			SiteCategory.SiteCategoryID.CssStyle = ""
			SiteCategory.SiteCategoryID.CssClass = ""
			SiteCategory.SiteCategoryID.ViewCustomAttributes = ""

			' CategoryKeywords
			SiteCategory.CategoryKeywords.ViewValue = SiteCategory.CategoryKeywords.CurrentValue
			SiteCategory.CategoryKeywords.CssStyle = ""
			SiteCategory.CategoryKeywords.CssClass = ""
			SiteCategory.CategoryKeywords.ViewCustomAttributes = ""

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

			' GroupOrder
			SiteCategory.GroupOrder.ViewValue = SiteCategory.GroupOrder.CurrentValue
			SiteCategory.GroupOrder.CssStyle = ""
			SiteCategory.GroupOrder.CssClass = ""
			SiteCategory.GroupOrder.ViewCustomAttributes = ""

			' ParentCategoryID
			SiteCategory.ParentCategoryID.ViewValue = SiteCategory.ParentCategoryID.CurrentValue
			SiteCategory.ParentCategoryID.CssStyle = ""
			SiteCategory.ParentCategoryID.CssClass = ""
			SiteCategory.ParentCategoryID.ViewCustomAttributes = ""

			' CategoryFileName
			SiteCategory.CategoryFileName.ViewValue = SiteCategory.CategoryFileName.CurrentValue
			SiteCategory.CategoryFileName.CssStyle = ""
			SiteCategory.CategoryFileName.CssClass = ""
			SiteCategory.CategoryFileName.ViewCustomAttributes = ""

			' SiteCategoryTypeID
			SiteCategory.SiteCategoryTypeID.ViewValue = SiteCategory.SiteCategoryTypeID.CurrentValue
			SiteCategory.SiteCategoryTypeID.CssStyle = ""
			SiteCategory.SiteCategoryTypeID.CssClass = ""
			SiteCategory.SiteCategoryTypeID.ViewCustomAttributes = ""

			' SiteCategoryGroupID
			SiteCategory.SiteCategoryGroupID.ViewValue = SiteCategory.SiteCategoryGroupID.CurrentValue
			SiteCategory.SiteCategoryGroupID.CssStyle = ""
			SiteCategory.SiteCategoryGroupID.CssClass = ""
			SiteCategory.SiteCategoryGroupID.ViewCustomAttributes = ""

			' View refer script
			' CategoryKeywords

			SiteCategory.CategoryKeywords.HrefValue = ""
			SiteCategory.CategoryKeywords.TooltipValue = ""

			' CategoryName
			SiteCategory.CategoryName.HrefValue = ""
			SiteCategory.CategoryName.TooltipValue = ""

			' CategoryTitle
			SiteCategory.CategoryTitle.HrefValue = ""
			SiteCategory.CategoryTitle.TooltipValue = ""

			' CategoryDescription
			SiteCategory.CategoryDescription.HrefValue = ""
			SiteCategory.CategoryDescription.TooltipValue = ""

			' GroupOrder
			SiteCategory.GroupOrder.HrefValue = ""
			SiteCategory.GroupOrder.TooltipValue = ""

			' ParentCategoryID
			SiteCategory.ParentCategoryID.HrefValue = ""
			SiteCategory.ParentCategoryID.TooltipValue = ""

			' CategoryFileName
			SiteCategory.CategoryFileName.HrefValue = ""
			SiteCategory.CategoryFileName.TooltipValue = ""

			' SiteCategoryTypeID
			SiteCategory.SiteCategoryTypeID.HrefValue = ""
			SiteCategory.SiteCategoryTypeID.TooltipValue = ""

			' SiteCategoryGroupID
			SiteCategory.SiteCategoryGroupID.HrefValue = ""
			SiteCategory.SiteCategoryGroupID.TooltipValue = ""

		'
		'  Add Row
		'

		ElseIf SiteCategory.RowType = EW_ROWTYPE_ADD Then ' Add row

			' CategoryKeywords
			SiteCategory.CategoryKeywords.EditCustomAttributes = ""
			SiteCategory.CategoryKeywords.EditValue = ew_HtmlEncode(SiteCategory.CategoryKeywords.CurrentValue)

			' CategoryName
			SiteCategory.CategoryName.EditCustomAttributes = ""
			SiteCategory.CategoryName.EditValue = ew_HtmlEncode(SiteCategory.CategoryName.CurrentValue)

			' CategoryTitle
			SiteCategory.CategoryTitle.EditCustomAttributes = ""
			SiteCategory.CategoryTitle.EditValue = ew_HtmlEncode(SiteCategory.CategoryTitle.CurrentValue)

			' CategoryDescription
			SiteCategory.CategoryDescription.EditCustomAttributes = ""
			SiteCategory.CategoryDescription.EditValue = ew_HtmlEncode(SiteCategory.CategoryDescription.CurrentValue)

			' GroupOrder
			SiteCategory.GroupOrder.EditCustomAttributes = ""
			SiteCategory.GroupOrder.EditValue = ew_HtmlEncode(SiteCategory.GroupOrder.CurrentValue)

			' ParentCategoryID
			SiteCategory.ParentCategoryID.EditCustomAttributes = ""
			SiteCategory.ParentCategoryID.EditValue = ew_HtmlEncode(SiteCategory.ParentCategoryID.CurrentValue)

			' CategoryFileName
			SiteCategory.CategoryFileName.EditCustomAttributes = ""
			SiteCategory.CategoryFileName.EditValue = ew_HtmlEncode(SiteCategory.CategoryFileName.CurrentValue)

			' SiteCategoryTypeID
			SiteCategory.SiteCategoryTypeID.EditCustomAttributes = ""
			SiteCategory.SiteCategoryTypeID.EditValue = ew_HtmlEncode(SiteCategory.SiteCategoryTypeID.CurrentValue)

			' SiteCategoryGroupID
			SiteCategory.SiteCategoryGroupID.EditCustomAttributes = ""
			SiteCategory.SiteCategoryGroupID.EditValue = ew_HtmlEncode(SiteCategory.SiteCategoryGroupID.CurrentValue)
		End If

		' Row Rendered event
		If SiteCategory.RowType <> EW_ROWTYPE_AGGREGATEINIT Then
			SiteCategory.Row_Rendered()
		End If
	End Sub

	'
	' Validate form
	'
	Function ValidateForm() As Boolean

		' Initialize
		ParentPage.gsFormError = ""

		' Check if validation required
		If Not EW_SERVER_VALIDATE Then Return (ParentPage.gsFormError = "")
		If Not ew_CheckNumber(SiteCategory.GroupOrder.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError &= SiteCategory.GroupOrder.FldErrMsg
		End If
		If Not ew_CheckInteger(SiteCategory.SiteCategoryTypeID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError &= SiteCategory.SiteCategoryTypeID.FldErrMsg
		End If
		If Not ew_CheckInteger(SiteCategory.SiteCategoryGroupID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError &= SiteCategory.SiteCategoryGroupID.FldErrMsg
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
		Try

			' CategoryKeywords
			SiteCategory.CategoryKeywords.SetDbValue(Rs, SiteCategory.CategoryKeywords.CurrentValue, System.DBNull.Value, False)

			' CategoryName
			SiteCategory.CategoryName.SetDbValue(Rs, SiteCategory.CategoryName.CurrentValue, System.DBNull.Value, False)

			' CategoryTitle
			SiteCategory.CategoryTitle.SetDbValue(Rs, SiteCategory.CategoryTitle.CurrentValue, System.DBNull.Value, False)

			' CategoryDescription
			SiteCategory.CategoryDescription.SetDbValue(Rs, SiteCategory.CategoryDescription.CurrentValue, System.DBNull.Value, False)

			' GroupOrder
			SiteCategory.GroupOrder.SetDbValue(Rs, SiteCategory.GroupOrder.CurrentValue, System.DBNull.Value, False)

			' ParentCategoryID
			SiteCategory.ParentCategoryID.SetDbValue(Rs, SiteCategory.ParentCategoryID.CurrentValue, System.DBNull.Value, False)

			' CategoryFileName
			SiteCategory.CategoryFileName.SetDbValue(Rs, SiteCategory.CategoryFileName.CurrentValue, System.DBNull.Value, False)

			' SiteCategoryTypeID
			SiteCategory.SiteCategoryTypeID.SetDbValue(Rs, SiteCategory.SiteCategoryTypeID.CurrentValue, System.DBNull.Value, False)

			' SiteCategoryGroupID
			SiteCategory.SiteCategoryGroupID.SetDbValue(Rs, SiteCategory.SiteCategoryGroupID.CurrentValue, System.DBNull.Value, False)
		Catch e As Exception
			If EW_DEBUG_ENABLED Then Throw
			Message = e.Message
			Return False
		End Try

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
				Message = Language.Phrase("InsertCancelled")
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
		Dim curDateTime As String, id As String, user As String, action As String
		Dim i As Integer
		Dim dt As DateTime = Now()		
		curDateTime = dt.ToString("yyyy/MM/dd HH:mm:ss")
		id = HttpContext.Current.Request.ServerVariables("SCRIPT_NAME")
    user = CurrentUserName()
		action = typ
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "", "", "", "")
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
		Dim curDateTime As String, id As String, user As String, field As String
		Dim keyvalue As Object
		Dim dt As DateTime = Now()
		curDateTime = dt.ToString("yyyy/MM/dd HH:mm:ss")
		id = HttpContext.Current.Request.ServerVariables("SCRIPT_NAME")
    user = CurrentUserName()
		keyvalue = sKey

		' CategoryKeywords Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "CategoryKeywords", keyvalue, oldvalue, RsSrc("CategoryKeywords"))

		' CategoryName Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "CategoryName", keyvalue, oldvalue, RsSrc("CategoryName"))

		' CategoryTitle Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "CategoryTitle", keyvalue, oldvalue, RsSrc("CategoryTitle"))

		' CategoryDescription Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "CategoryDescription", keyvalue, oldvalue, RsSrc("CategoryDescription"))

		' GroupOrder Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "GroupOrder", keyvalue, oldvalue, RsSrc("GroupOrder"))

		' ParentCategoryID Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "ParentCategoryID", keyvalue, oldvalue, RsSrc("ParentCategoryID"))

		' CategoryFileName Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "CategoryFileName", keyvalue, oldvalue, RsSrc("CategoryFileName"))

		' SiteCategoryTypeID Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "SiteCategoryTypeID", keyvalue, oldvalue, RsSrc("SiteCategoryTypeID"))

		' SiteCategoryGroupID Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "SiteCategoryGroupID", keyvalue, oldvalue, RsSrc("SiteCategoryGroupID"))
	End Sub

		' Page Load event
		Public Sub Page_Load()

			'HttpContext.Current.Response.Write("Page Load")
		End Sub

		' Page Unload event
		Public Sub Page_Unload()

			'HttpContext.Current.Response.Write("Page Unload")
		End Sub

		' Page Redirecting event
		Public Sub Page_Redirecting(ByRef url As String)

			'url = newurl
		End Sub

		' Message Showing event
		Public Sub Message_Showing(ByRef msg As String)

			'msg = newmsg
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

		' Page init
		SiteCategory_add = New cSiteCategory_add(Me)		
		SiteCategory_add.Page_Init()		

		' Set buffer/cache option
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

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