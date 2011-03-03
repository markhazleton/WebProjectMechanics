Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class zpage_add
	Inherits AspNetMaker8_wpmWebsite

	' Page object
	Public zPage_add As czPage_add

	'
	' Page Class
	'
	Class czPage_add
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
				If zPage.UseTokenInUrl Then Url = Url & "t=" & zPage.TableVar & "&" ' Add page token
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
			If zPage.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (zPage.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (zPage.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' ASP.NET page object
		Public ReadOnly Property AspNetPage() As zpage_add
			Get
				Return CType(m_ParentPage, zpage_add)
			End Get
		End Property

		' zPage
		Public Property zPage() As czPage
			Get				
				Return ParentPage.zPage
			End Get
			Set(ByVal v As czPage)
				ParentPage.zPage = v	
			End Set	
		End Property

		' zPage
		Public Property Company() As cCompany
			Get				
				Return ParentPage.Company
			End Get
			Set(ByVal v As cCompany)
				ParentPage.Company = v	
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
			m_PageObjName = "zPage_add"
			m_PageObjTypeName = "czPage_add"			

			' Initialize language object
			Language = New cLanguage(Me)

			' Table Name
			m_TableName = "Page"

			' Initialize table object
			zPage = New czPage(Me)
			Company = New cCompany(Me)

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
			zPage.Dispose()
			Company.Dispose()
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
		If ew_Get("zPageID") <> "" Then
			zPage.zPageID.QueryStringValue = ew_Get("zPageID")
		Else
			bCopy = False
		End If

		' Set up master detail parameters
		SetUpMasterDetail()

		' Process form if post back
		If ObjForm.GetValue("a_add") <> "" Then
			zPage.CurrentAction = ObjForm.GetValue("a_add") ' Get form action
			LoadFormValues() ' Load form values

			' Validate Form
			If Not ValidateForm() Then
				zPage.CurrentAction = "I" ' Form error, reset action
				Message = ParentPage.gsFormError
			End If

		' Not post back
		Else
			If bCopy Then
				zPage.CurrentAction = "C" ' Copy Record
			Else
				zPage.CurrentAction = "I" ' Display Blank Record
				LoadDefaultValues() ' Load default values
			End If
		End If

		' Perform action based on action code
		Select Case zPage.CurrentAction
			Case "I" ' Blank record, no action required
			Case "C" ' Copy an existing record
				If Not LoadRow() Then ' Load record based on key
					Message = Language.Phrase("NoRecord") ' No record found
					Page_Terminate("zpage_list.aspx") ' No matching record, return to list
				End If
			Case "A" ' Add new record
				zPage.SendEmail = True ' Send email on add success
				If AddRow() Then ' Add successful
					Message = Language.Phrase("AddSuccess") ' Set up success message
					Dim sReturnUrl As String = zPage.ReturnUrl
					Page_Terminate(sReturnUrl) ' Clean up and return
				Else
					RestoreFormValues() ' Add failed, restore form values
				End If
		End Select

		' Render row based on row type
		zPage.RowType = EW_ROWTYPE_ADD ' Render add type

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
		zPage.ParentPageID.CurrentValue = 0
		zPage.PageTypeID.CurrentValue = 0
		zPage.GroupID.CurrentValue = 0
		zPage.PageOrder.CurrentValue = 0
		zPage.CompanyID.CurrentValue = 0
		zPage.ImagesPerRow.CurrentValue = 0
		zPage.RowsPerPage.CurrentValue = 0
		zPage.VersionNo.CurrentValue = 0
	End Sub

	'
	' Load form values
	'
	Sub LoadFormValues()
		zPage.ParentPageID.FormValue = ObjForm.GetValue("x_ParentPageID")
		zPage.ParentPageID.OldValue = ObjForm.GetValue("o_ParentPageID")
		zPage.zPageName.FormValue = ObjForm.GetValue("x_zPageName")
		zPage.zPageName.OldValue = ObjForm.GetValue("o_zPageName")
		zPage.PageTitle.FormValue = ObjForm.GetValue("x_PageTitle")
		zPage.PageTitle.OldValue = ObjForm.GetValue("o_PageTitle")
		zPage.PageTypeID.FormValue = ObjForm.GetValue("x_PageTypeID")
		zPage.PageTypeID.OldValue = ObjForm.GetValue("o_PageTypeID")
		zPage.GroupID.FormValue = ObjForm.GetValue("x_GroupID")
		zPage.GroupID.OldValue = ObjForm.GetValue("o_GroupID")
		zPage.Active.FormValue = ObjForm.GetValue("x_Active")
		zPage.Active.OldValue = ObjForm.GetValue("o_Active")
		zPage.PageOrder.FormValue = ObjForm.GetValue("x_PageOrder")
		zPage.PageOrder.OldValue = ObjForm.GetValue("o_PageOrder")
		zPage.CompanyID.FormValue = ObjForm.GetValue("x_CompanyID")
		zPage.CompanyID.OldValue = ObjForm.GetValue("o_CompanyID")
		zPage.PageDescription.FormValue = ObjForm.GetValue("x_PageDescription")
		zPage.PageDescription.OldValue = ObjForm.GetValue("o_PageDescription")
		zPage.PageKeywords.FormValue = ObjForm.GetValue("x_PageKeywords")
		zPage.PageKeywords.OldValue = ObjForm.GetValue("o_PageKeywords")
		zPage.ImagesPerRow.FormValue = ObjForm.GetValue("x_ImagesPerRow")
		zPage.ImagesPerRow.OldValue = ObjForm.GetValue("o_ImagesPerRow")
		zPage.RowsPerPage.FormValue = ObjForm.GetValue("x_RowsPerPage")
		zPage.RowsPerPage.OldValue = ObjForm.GetValue("o_RowsPerPage")
		zPage.PageFileName.FormValue = ObjForm.GetValue("x_PageFileName")
		zPage.PageFileName.OldValue = ObjForm.GetValue("o_PageFileName")
		zPage.AllowMessage.FormValue = ObjForm.GetValue("x_AllowMessage")
		zPage.AllowMessage.OldValue = ObjForm.GetValue("o_AllowMessage")
		zPage.SiteCategoryID.FormValue = ObjForm.GetValue("x_SiteCategoryID")
		zPage.SiteCategoryID.OldValue = ObjForm.GetValue("o_SiteCategoryID")
		zPage.SiteCategoryGroupID.FormValue = ObjForm.GetValue("x_SiteCategoryGroupID")
		zPage.SiteCategoryGroupID.OldValue = ObjForm.GetValue("o_SiteCategoryGroupID")
		zPage.VersionNo.FormValue = ObjForm.GetValue("x_VersionNo")
		zPage.VersionNo.OldValue = ObjForm.GetValue("o_VersionNo")
		zPage.ModifiedDT.FormValue = ObjForm.GetValue("x_ModifiedDT")
		zPage.ModifiedDT.CurrentValue = ew_UnFormatDateTime(zPage.ModifiedDT.CurrentValue, 8)
		zPage.ModifiedDT.OldValue = ObjForm.GetValue("o_ModifiedDT")
		zPage.zPageID.FormValue = ObjForm.GetValue("x_zPageID")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		zPage.ParentPageID.CurrentValue = zPage.ParentPageID.FormValue
		zPage.zPageName.CurrentValue = zPage.zPageName.FormValue
		zPage.PageTitle.CurrentValue = zPage.PageTitle.FormValue
		zPage.PageTypeID.CurrentValue = zPage.PageTypeID.FormValue
		zPage.GroupID.CurrentValue = zPage.GroupID.FormValue
		zPage.Active.CurrentValue = zPage.Active.FormValue
		zPage.PageOrder.CurrentValue = zPage.PageOrder.FormValue
		zPage.CompanyID.CurrentValue = zPage.CompanyID.FormValue
		zPage.PageDescription.CurrentValue = zPage.PageDescription.FormValue
		zPage.PageKeywords.CurrentValue = zPage.PageKeywords.FormValue
		zPage.ImagesPerRow.CurrentValue = zPage.ImagesPerRow.FormValue
		zPage.RowsPerPage.CurrentValue = zPage.RowsPerPage.FormValue
		zPage.PageFileName.CurrentValue = zPage.PageFileName.FormValue
		zPage.AllowMessage.CurrentValue = zPage.AllowMessage.FormValue
		zPage.SiteCategoryID.CurrentValue = zPage.SiteCategoryID.FormValue
		zPage.SiteCategoryGroupID.CurrentValue = zPage.SiteCategoryGroupID.FormValue
		zPage.VersionNo.CurrentValue = zPage.VersionNo.FormValue
		zPage.ModifiedDT.CurrentValue = zPage.ModifiedDT.FormValue
		zPage.ModifiedDT.CurrentValue = ew_UnFormatDateTime(zPage.ModifiedDT.CurrentValue, 8)
		zPage.zPageID.CurrentValue = zPage.zPageID.FormValue
	End Sub

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = zPage.KeyFilter

		' Row Selecting event
		zPage.Row_Selecting(sFilter)

		' Load SQL based on filter
		zPage.CurrentFilter = sFilter
		Dim sSql As String = zPage.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then DebugMsg = sSql
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				zPage.Row_Selected(RsRow)
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
		zPage.zPageID.DbValue = RsRow("PageID")
		zPage.ParentPageID.DbValue = RsRow("ParentPageID")
		zPage.zPageName.DbValue = RsRow("PageName")
		zPage.PageTitle.DbValue = RsRow("PageTitle")
		zPage.PageTypeID.DbValue = RsRow("PageTypeID")
		zPage.GroupID.DbValue = RsRow("GroupID")
		zPage.Active.DbValue = IIf(ew_ConvertToBool(RsRow("Active")), "1", "0")
		zPage.PageOrder.DbValue = RsRow("PageOrder")
		zPage.CompanyID.DbValue = RsRow("CompanyID")
		zPage.PageDescription.DbValue = RsRow("PageDescription")
		zPage.PageKeywords.DbValue = RsRow("PageKeywords")
		zPage.ImagesPerRow.DbValue = RsRow("ImagesPerRow")
		zPage.RowsPerPage.DbValue = RsRow("RowsPerPage")
		zPage.PageFileName.DbValue = RsRow("PageFileName")
		zPage.AllowMessage.DbValue = IIf(ew_ConvertToBool(RsRow("AllowMessage")), "1", "0")
		zPage.SiteCategoryID.DbValue = RsRow("SiteCategoryID")
		zPage.SiteCategoryGroupID.DbValue = RsRow("SiteCategoryGroupID")
		zPage.VersionNo.DbValue = RsRow("VersionNo")
		zPage.ModifiedDT.DbValue = RsRow("ModifiedDT")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Initialize urls
		' Row Rendering event

		zPage.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' ParentPageID

		zPage.ParentPageID.CellCssStyle = ""
		zPage.ParentPageID.CellCssClass = ""
		zPage.ParentPageID.CellAttrs.Clear(): zPage.ParentPageID.ViewAttrs.Clear(): zPage.ParentPageID.EditAttrs.Clear()

		' PageName
		zPage.zPageName.CellCssStyle = ""
		zPage.zPageName.CellCssClass = ""
		zPage.zPageName.CellAttrs.Clear(): zPage.zPageName.ViewAttrs.Clear(): zPage.zPageName.EditAttrs.Clear()

		' PageTitle
		zPage.PageTitle.CellCssStyle = ""
		zPage.PageTitle.CellCssClass = ""
		zPage.PageTitle.CellAttrs.Clear(): zPage.PageTitle.ViewAttrs.Clear(): zPage.PageTitle.EditAttrs.Clear()

		' PageTypeID
		zPage.PageTypeID.CellCssStyle = ""
		zPage.PageTypeID.CellCssClass = ""
		zPage.PageTypeID.CellAttrs.Clear(): zPage.PageTypeID.ViewAttrs.Clear(): zPage.PageTypeID.EditAttrs.Clear()

		' GroupID
		zPage.GroupID.CellCssStyle = ""
		zPage.GroupID.CellCssClass = ""
		zPage.GroupID.CellAttrs.Clear(): zPage.GroupID.ViewAttrs.Clear(): zPage.GroupID.EditAttrs.Clear()

		' Active
		zPage.Active.CellCssStyle = ""
		zPage.Active.CellCssClass = ""
		zPage.Active.CellAttrs.Clear(): zPage.Active.ViewAttrs.Clear(): zPage.Active.EditAttrs.Clear()

		' PageOrder
		zPage.PageOrder.CellCssStyle = ""
		zPage.PageOrder.CellCssClass = ""
		zPage.PageOrder.CellAttrs.Clear(): zPage.PageOrder.ViewAttrs.Clear(): zPage.PageOrder.EditAttrs.Clear()

		' CompanyID
		zPage.CompanyID.CellCssStyle = ""
		zPage.CompanyID.CellCssClass = ""
		zPage.CompanyID.CellAttrs.Clear(): zPage.CompanyID.ViewAttrs.Clear(): zPage.CompanyID.EditAttrs.Clear()

		' PageDescription
		zPage.PageDescription.CellCssStyle = ""
		zPage.PageDescription.CellCssClass = ""
		zPage.PageDescription.CellAttrs.Clear(): zPage.PageDescription.ViewAttrs.Clear(): zPage.PageDescription.EditAttrs.Clear()

		' PageKeywords
		zPage.PageKeywords.CellCssStyle = ""
		zPage.PageKeywords.CellCssClass = ""
		zPage.PageKeywords.CellAttrs.Clear(): zPage.PageKeywords.ViewAttrs.Clear(): zPage.PageKeywords.EditAttrs.Clear()

		' ImagesPerRow
		zPage.ImagesPerRow.CellCssStyle = ""
		zPage.ImagesPerRow.CellCssClass = ""
		zPage.ImagesPerRow.CellAttrs.Clear(): zPage.ImagesPerRow.ViewAttrs.Clear(): zPage.ImagesPerRow.EditAttrs.Clear()

		' RowsPerPage
		zPage.RowsPerPage.CellCssStyle = ""
		zPage.RowsPerPage.CellCssClass = ""
		zPage.RowsPerPage.CellAttrs.Clear(): zPage.RowsPerPage.ViewAttrs.Clear(): zPage.RowsPerPage.EditAttrs.Clear()

		' PageFileName
		zPage.PageFileName.CellCssStyle = ""
		zPage.PageFileName.CellCssClass = ""
		zPage.PageFileName.CellAttrs.Clear(): zPage.PageFileName.ViewAttrs.Clear(): zPage.PageFileName.EditAttrs.Clear()

		' AllowMessage
		zPage.AllowMessage.CellCssStyle = ""
		zPage.AllowMessage.CellCssClass = ""
		zPage.AllowMessage.CellAttrs.Clear(): zPage.AllowMessage.ViewAttrs.Clear(): zPage.AllowMessage.EditAttrs.Clear()

		' SiteCategoryID
		zPage.SiteCategoryID.CellCssStyle = ""
		zPage.SiteCategoryID.CellCssClass = ""
		zPage.SiteCategoryID.CellAttrs.Clear(): zPage.SiteCategoryID.ViewAttrs.Clear(): zPage.SiteCategoryID.EditAttrs.Clear()

		' SiteCategoryGroupID
		zPage.SiteCategoryGroupID.CellCssStyle = ""
		zPage.SiteCategoryGroupID.CellCssClass = ""
		zPage.SiteCategoryGroupID.CellAttrs.Clear(): zPage.SiteCategoryGroupID.ViewAttrs.Clear(): zPage.SiteCategoryGroupID.EditAttrs.Clear()

		' VersionNo
		zPage.VersionNo.CellCssStyle = ""
		zPage.VersionNo.CellCssClass = ""
		zPage.VersionNo.CellAttrs.Clear(): zPage.VersionNo.ViewAttrs.Clear(): zPage.VersionNo.EditAttrs.Clear()

		' ModifiedDT
		zPage.ModifiedDT.CellCssStyle = ""
		zPage.ModifiedDT.CellCssClass = ""
		zPage.ModifiedDT.CellAttrs.Clear(): zPage.ModifiedDT.ViewAttrs.Clear(): zPage.ModifiedDT.EditAttrs.Clear()

		'
		'  View  Row
		'

		If zPage.RowType = EW_ROWTYPE_VIEW Then ' View row

			' ParentPageID
			If ew_NotEmpty(zPage.ParentPageID.CurrentValue) Then
				sFilterWrk = "[PageID] = " & ew_AdjustSql(zPage.ParentPageID.CurrentValue) & ""
			sSqlWrk = "SELECT [PageName] FROM [Page]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [PageName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					zPage.ParentPageID.ViewValue = RsWrk("PageName")
				Else
					zPage.ParentPageID.ViewValue = zPage.ParentPageID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				zPage.ParentPageID.ViewValue = System.DBNull.Value
			End If
			zPage.ParentPageID.CssStyle = ""
			zPage.ParentPageID.CssClass = ""
			zPage.ParentPageID.ViewCustomAttributes = ""

			' PageName
			zPage.zPageName.ViewValue = zPage.zPageName.CurrentValue
			zPage.zPageName.CssStyle = ""
			zPage.zPageName.CssClass = ""
			zPage.zPageName.ViewCustomAttributes = ""

			' PageTitle
			zPage.PageTitle.ViewValue = zPage.PageTitle.CurrentValue
			zPage.PageTitle.CssStyle = ""
			zPage.PageTitle.CssClass = ""
			zPage.PageTitle.ViewCustomAttributes = ""

			' PageTypeID
			If ew_NotEmpty(zPage.PageTypeID.CurrentValue) Then
				sFilterWrk = "[PageTypeID] = " & ew_AdjustSql(zPage.PageTypeID.CurrentValue) & ""
			sSqlWrk = "SELECT [PageTypeCD] FROM [PageType]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [PageTypeCD] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					zPage.PageTypeID.ViewValue = RsWrk("PageTypeCD")
				Else
					zPage.PageTypeID.ViewValue = zPage.PageTypeID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				zPage.PageTypeID.ViewValue = System.DBNull.Value
			End If
			zPage.PageTypeID.CssStyle = ""
			zPage.PageTypeID.CssClass = ""
			zPage.PageTypeID.ViewCustomAttributes = ""

			' GroupID
			If ew_NotEmpty(zPage.GroupID.CurrentValue) Then
				sFilterWrk = "[GroupID] = " & ew_AdjustSql(zPage.GroupID.CurrentValue) & ""
			sSqlWrk = "SELECT [GroupName] FROM [Group]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [GroupName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					zPage.GroupID.ViewValue = RsWrk("GroupName")
				Else
					zPage.GroupID.ViewValue = zPage.GroupID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				zPage.GroupID.ViewValue = System.DBNull.Value
			End If
			zPage.GroupID.CssStyle = ""
			zPage.GroupID.CssClass = ""
			zPage.GroupID.ViewCustomAttributes = ""

			' Active
			If Convert.ToString(zPage.Active.CurrentValue) = "1" Then
				zPage.Active.ViewValue = "Yes"
			Else
				zPage.Active.ViewValue = "No"
			End If
			zPage.Active.CssStyle = ""
			zPage.Active.CssClass = ""
			zPage.Active.ViewCustomAttributes = ""

			' PageOrder
			zPage.PageOrder.ViewValue = zPage.PageOrder.CurrentValue
			zPage.PageOrder.CssStyle = ""
			zPage.PageOrder.CssClass = ""
			zPage.PageOrder.ViewCustomAttributes = ""

			' CompanyID
			If ew_NotEmpty(zPage.CompanyID.CurrentValue) Then
				sFilterWrk = "[CompanyID] = " & ew_AdjustSql(zPage.CompanyID.CurrentValue) & ""
			sSqlWrk = "SELECT [CompanyName] FROM [Company]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [CompanyName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					zPage.CompanyID.ViewValue = RsWrk("CompanyName")
				Else
					zPage.CompanyID.ViewValue = zPage.CompanyID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				zPage.CompanyID.ViewValue = System.DBNull.Value
			End If
			zPage.CompanyID.CssStyle = ""
			zPage.CompanyID.CssClass = ""
			zPage.CompanyID.ViewCustomAttributes = ""

			' PageDescription
			zPage.PageDescription.ViewValue = zPage.PageDescription.CurrentValue
			zPage.PageDescription.CssStyle = ""
			zPage.PageDescription.CssClass = ""
			zPage.PageDescription.ViewCustomAttributes = ""

			' PageKeywords
			zPage.PageKeywords.ViewValue = zPage.PageKeywords.CurrentValue
			zPage.PageKeywords.CssStyle = ""
			zPage.PageKeywords.CssClass = ""
			zPage.PageKeywords.ViewCustomAttributes = ""

			' ImagesPerRow
			zPage.ImagesPerRow.ViewValue = zPage.ImagesPerRow.CurrentValue
			zPage.ImagesPerRow.CssStyle = ""
			zPage.ImagesPerRow.CssClass = ""
			zPage.ImagesPerRow.ViewCustomAttributes = ""

			' RowsPerPage
			zPage.RowsPerPage.ViewValue = zPage.RowsPerPage.CurrentValue
			zPage.RowsPerPage.CssStyle = ""
			zPage.RowsPerPage.CssClass = ""
			zPage.RowsPerPage.ViewCustomAttributes = ""

			' PageFileName
			zPage.PageFileName.ViewValue = zPage.PageFileName.CurrentValue
			zPage.PageFileName.CssStyle = ""
			zPage.PageFileName.CssClass = ""
			zPage.PageFileName.ViewCustomAttributes = ""

			' AllowMessage
			If Convert.ToString(zPage.AllowMessage.CurrentValue) = "1" Then
				zPage.AllowMessage.ViewValue = "Yes"
			Else
				zPage.AllowMessage.ViewValue = "No"
			End If
			zPage.AllowMessage.CssStyle = ""
			zPage.AllowMessage.CssClass = ""
			zPage.AllowMessage.ViewCustomAttributes = ""

			' SiteCategoryID
			zPage.SiteCategoryID.ViewValue = zPage.SiteCategoryID.CurrentValue
			zPage.SiteCategoryID.CssStyle = ""
			zPage.SiteCategoryID.CssClass = ""
			zPage.SiteCategoryID.ViewCustomAttributes = ""

			' SiteCategoryGroupID
			zPage.SiteCategoryGroupID.ViewValue = zPage.SiteCategoryGroupID.CurrentValue
			zPage.SiteCategoryGroupID.CssStyle = ""
			zPage.SiteCategoryGroupID.CssClass = ""
			zPage.SiteCategoryGroupID.ViewCustomAttributes = ""

			' VersionNo
			zPage.VersionNo.ViewValue = zPage.VersionNo.CurrentValue
			zPage.VersionNo.CssStyle = ""
			zPage.VersionNo.CssClass = ""
			zPage.VersionNo.ViewCustomAttributes = ""

			' ModifiedDT
			zPage.ModifiedDT.ViewValue = zPage.ModifiedDT.CurrentValue
			zPage.ModifiedDT.CssStyle = ""
			zPage.ModifiedDT.CssClass = ""
			zPage.ModifiedDT.ViewCustomAttributes = ""

			' View refer script
			' ParentPageID

			zPage.ParentPageID.HrefValue = ""
			zPage.ParentPageID.TooltipValue = ""

			' PageName
			zPage.zPageName.HrefValue = ""
			zPage.zPageName.TooltipValue = ""

			' PageTitle
			zPage.PageTitle.HrefValue = ""
			zPage.PageTitle.TooltipValue = ""

			' PageTypeID
			zPage.PageTypeID.HrefValue = ""
			zPage.PageTypeID.TooltipValue = ""

			' GroupID
			zPage.GroupID.HrefValue = ""
			zPage.GroupID.TooltipValue = ""

			' Active
			zPage.Active.HrefValue = ""
			zPage.Active.TooltipValue = ""

			' PageOrder
			zPage.PageOrder.HrefValue = ""
			zPage.PageOrder.TooltipValue = ""

			' CompanyID
			zPage.CompanyID.HrefValue = ""
			zPage.CompanyID.TooltipValue = ""

			' PageDescription
			zPage.PageDescription.HrefValue = ""
			zPage.PageDescription.TooltipValue = ""

			' PageKeywords
			zPage.PageKeywords.HrefValue = ""
			zPage.PageKeywords.TooltipValue = ""

			' ImagesPerRow
			zPage.ImagesPerRow.HrefValue = ""
			zPage.ImagesPerRow.TooltipValue = ""

			' RowsPerPage
			zPage.RowsPerPage.HrefValue = ""
			zPage.RowsPerPage.TooltipValue = ""

			' PageFileName
			zPage.PageFileName.HrefValue = ""
			zPage.PageFileName.TooltipValue = ""

			' AllowMessage
			zPage.AllowMessage.HrefValue = ""
			zPage.AllowMessage.TooltipValue = ""

			' SiteCategoryID
			zPage.SiteCategoryID.HrefValue = ""
			zPage.SiteCategoryID.TooltipValue = ""

			' SiteCategoryGroupID
			zPage.SiteCategoryGroupID.HrefValue = ""
			zPage.SiteCategoryGroupID.TooltipValue = ""

			' VersionNo
			zPage.VersionNo.HrefValue = ""
			zPage.VersionNo.TooltipValue = ""

			' ModifiedDT
			zPage.ModifiedDT.HrefValue = ""
			zPage.ModifiedDT.TooltipValue = ""

		'
		'  Add Row
		'

		ElseIf zPage.RowType = EW_ROWTYPE_ADD Then ' Add row

			' ParentPageID
			zPage.ParentPageID.EditCustomAttributes = ""
			sFilterWrk = ""
			sSqlWrk = "SELECT [PageID], [PageName], '' AS Disp2Fld, [CompanyID] FROM [Page]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [PageName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", Language.Phrase("PleaseSelect"), ""}) 
			zPage.ParentPageID.EditValue = arwrk

			' PageName
			zPage.zPageName.EditCustomAttributes = ""
			zPage.zPageName.EditValue = ew_HtmlEncode(zPage.zPageName.CurrentValue)

			' PageTitle
			zPage.PageTitle.EditCustomAttributes = ""
			zPage.PageTitle.EditValue = ew_HtmlEncode(zPage.PageTitle.CurrentValue)

			' PageTypeID
			zPage.PageTypeID.EditCustomAttributes = ""
			sFilterWrk = ""
			sSqlWrk = "SELECT [PageTypeID], [PageTypeCD], '' AS Disp2Fld, '' AS SelectFilterFld FROM [PageType]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [PageTypeCD] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", Language.Phrase("PleaseSelect")}) 
			zPage.PageTypeID.EditValue = arwrk

			' GroupID
			zPage.GroupID.EditCustomAttributes = ""
			sFilterWrk = ""
			sSqlWrk = "SELECT [GroupID], [GroupName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Group]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [GroupName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", Language.Phrase("PleaseSelect")}) 
			zPage.GroupID.EditValue = arwrk

			' Active
			zPage.Active.EditCustomAttributes = ""

			' PageOrder
			zPage.PageOrder.EditCustomAttributes = ""
			zPage.PageOrder.EditValue = ew_HtmlEncode(zPage.PageOrder.CurrentValue)

			' CompanyID
			zPage.CompanyID.EditCustomAttributes = ""
			If zPage.CompanyID.SessionValue <> "" Then
				zPage.CompanyID.CurrentValue = zPage.CompanyID.SessionValue
			If ew_NotEmpty(zPage.CompanyID.CurrentValue) Then
				sFilterWrk = "[CompanyID] = " & ew_AdjustSql(zPage.CompanyID.CurrentValue) & ""
			sSqlWrk = "SELECT [CompanyName] FROM [Company]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [CompanyName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					zPage.CompanyID.ViewValue = RsWrk("CompanyName")
				Else
					zPage.CompanyID.ViewValue = zPage.CompanyID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				zPage.CompanyID.ViewValue = System.DBNull.Value
			End If
			zPage.CompanyID.CssStyle = ""
			zPage.CompanyID.CssClass = ""
			zPage.CompanyID.ViewCustomAttributes = ""
			Else
			sFilterWrk = ""
			sSqlWrk = "SELECT [CompanyID], [CompanyName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Company]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [CompanyName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", Language.Phrase("PleaseSelect")}) 
			zPage.CompanyID.EditValue = arwrk
			End If

			' PageDescription
			zPage.PageDescription.EditCustomAttributes = ""
			zPage.PageDescription.EditValue = ew_HtmlEncode(zPage.PageDescription.CurrentValue)

			' PageKeywords
			zPage.PageKeywords.EditCustomAttributes = ""
			zPage.PageKeywords.EditValue = ew_HtmlEncode(zPage.PageKeywords.CurrentValue)

			' ImagesPerRow
			zPage.ImagesPerRow.EditCustomAttributes = ""
			zPage.ImagesPerRow.EditValue = ew_HtmlEncode(zPage.ImagesPerRow.CurrentValue)

			' RowsPerPage
			zPage.RowsPerPage.EditCustomAttributes = ""
			zPage.RowsPerPage.EditValue = ew_HtmlEncode(zPage.RowsPerPage.CurrentValue)

			' PageFileName
			zPage.PageFileName.EditCustomAttributes = ""
			zPage.PageFileName.EditValue = ew_HtmlEncode(zPage.PageFileName.CurrentValue)

			' AllowMessage
			zPage.AllowMessage.EditCustomAttributes = ""

			' SiteCategoryID
			zPage.SiteCategoryID.EditCustomAttributes = ""
			zPage.SiteCategoryID.EditValue = ew_HtmlEncode(zPage.SiteCategoryID.CurrentValue)

			' SiteCategoryGroupID
			zPage.SiteCategoryGroupID.EditCustomAttributes = ""
			zPage.SiteCategoryGroupID.EditValue = ew_HtmlEncode(zPage.SiteCategoryGroupID.CurrentValue)

			' VersionNo
			zPage.VersionNo.EditCustomAttributes = ""
			zPage.VersionNo.EditValue = ew_HtmlEncode(zPage.VersionNo.CurrentValue)

			' ModifiedDT
			zPage.ModifiedDT.EditCustomAttributes = ""
			zPage.ModifiedDT.EditValue = zPage.ModifiedDT.CurrentValue
		End If

		' Row Rendered event
		If zPage.RowType <> EW_ROWTYPE_AGGREGATEINIT Then
			zPage.Row_Rendered()
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
		If ew_Empty(zPage.zPageName.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError &= Language.Phrase("EnterRequiredField") & " - " & zPage.zPageName.FldCaption
		End If
		If Not ew_CheckInteger(zPage.PageOrder.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError &= zPage.PageOrder.FldErrMsg
		End If
		If Not ew_CheckInteger(zPage.ImagesPerRow.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError &= zPage.ImagesPerRow.FldErrMsg
		End If
		If Not ew_CheckInteger(zPage.RowsPerPage.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError &= zPage.RowsPerPage.FldErrMsg
		End If
		If Not ew_CheckInteger(zPage.SiteCategoryID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError &= zPage.SiteCategoryID.FldErrMsg
		End If
		If Not ew_CheckInteger(zPage.SiteCategoryGroupID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError &= zPage.SiteCategoryGroupID.FldErrMsg
		End If
		If Not ew_CheckInteger(zPage.VersionNo.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError &= zPage.VersionNo.FldErrMsg
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

			' ParentPageID
			zPage.ParentPageID.SetDbValue(Rs, zPage.ParentPageID.CurrentValue, System.DBNull.Value, True)

			' PageName
			zPage.zPageName.SetDbValue(Rs, zPage.zPageName.CurrentValue, "", False)

			' PageTitle
			zPage.PageTitle.SetDbValue(Rs, zPage.PageTitle.CurrentValue, System.DBNull.Value, False)

			' PageTypeID
			zPage.PageTypeID.SetDbValue(Rs, zPage.PageTypeID.CurrentValue, System.DBNull.Value, True)

			' GroupID
			zPage.GroupID.SetDbValue(Rs, zPage.GroupID.CurrentValue, System.DBNull.Value, True)

			' Active
			zPage.Active.SetDbValue(Rs, (zPage.Active.CurrentValue <> "" AndAlso Not IsDBNull(zPage.Active.CurrentValue)), System.DBNull.Value, False)

			' PageOrder
			zPage.PageOrder.SetDbValue(Rs, zPage.PageOrder.CurrentValue, 0, True)

			' CompanyID
			zPage.CompanyID.SetDbValue(Rs, zPage.CompanyID.CurrentValue, System.DBNull.Value, True)

			' PageDescription
			zPage.PageDescription.SetDbValue(Rs, zPage.PageDescription.CurrentValue, System.DBNull.Value, False)

			' PageKeywords
			zPage.PageKeywords.SetDbValue(Rs, zPage.PageKeywords.CurrentValue, System.DBNull.Value, False)

			' ImagesPerRow
			zPage.ImagesPerRow.SetDbValue(Rs, zPage.ImagesPerRow.CurrentValue, System.DBNull.Value, True)

			' RowsPerPage
			zPage.RowsPerPage.SetDbValue(Rs, zPage.RowsPerPage.CurrentValue, System.DBNull.Value, True)

			' PageFileName
			zPage.PageFileName.SetDbValue(Rs, zPage.PageFileName.CurrentValue, System.DBNull.Value, False)

			' AllowMessage
			zPage.AllowMessage.SetDbValue(Rs, (zPage.AllowMessage.CurrentValue <> "" AndAlso Not IsDBNull(zPage.AllowMessage.CurrentValue)), System.DBNull.Value, False)

			' SiteCategoryID
			zPage.SiteCategoryID.SetDbValue(Rs, zPage.SiteCategoryID.CurrentValue, System.DBNull.Value, False)

			' SiteCategoryGroupID
			zPage.SiteCategoryGroupID.SetDbValue(Rs, zPage.SiteCategoryGroupID.CurrentValue, System.DBNull.Value, False)

			' VersionNo
			zPage.VersionNo.SetDbValue(Rs, zPage.VersionNo.CurrentValue, System.DBNull.Value, True)

			' ModifiedDT
			zPage.ModifiedDT.SetDbValue(Rs, zPage.ModifiedDT.CurrentValue, System.DBNull.Value, False)
		Catch e As Exception
			If EW_DEBUG_ENABLED Then Throw
			Message = e.Message
			Return False
		End Try

		' Row Inserting event
		bInsertRow = zPage.Row_Inserting(Rs)
		If bInsertRow Then
			Try	
				zPage.Insert(Rs)
				AddRow = True
			Catch e As Exception
				If EW_DEBUG_ENABLED Then Throw
				Message = e.Message				
				AddRow = False
			End Try
		Else
			If zPage.CancelMessage <> "" Then
				Message = zPage.CancelMessage
				zPage.CancelMessage = ""
			Else
				Message = Language.Phrase("InsertCancelled")
			End If
			AddRow = False
		End If
		If AddRow Then
			LastInsertId = Conn.GetLastInsertId()
			zPage.zPageID.DbValue = LastInsertId
			Rs("PageID") = zPage.zPageID.DbValue		

			' Row Inserted event
			zPage.Row_Inserted(Rs)
			WriteAuditTrailOnAdd(Rs)
		End If
	End Function

	'
	' Set up Master Detail based on querystring parameter
	'
	Sub SetUpMasterDetail()
		Dim bValidMaster As Boolean = False, sMasterTblVar As String

		' Get the keys for master table
		If ew_Get(EW_TABLE_SHOW_MASTER) <> "" Then
			sMasterTblVar = ew_Get(EW_TABLE_SHOW_MASTER)
			If sMasterTblVar = "" Then
				bValidMaster = True
				sDbMasterFilter = ""
				sDbDetailFilter = ""
			End If
			If sMasterTblVar = "Company" Then
				bValidMaster = True
				sDbMasterFilter = zPage.SqlMasterFilter_Company
				sDbDetailFilter = zPage.SqlDetailFilter_Company
				If ew_Get("CompanyID") <> "" Then
					Company.CompanyID.QueryStringValue = ew_Get("CompanyID")
					zPage.CompanyID.QueryStringValue = Company.CompanyID.QueryStringValue
					zPage.CompanyID.SessionValue = zPage.CompanyID.QueryStringValue
					If Not IsNumeric(Company.CompanyID.QueryStringValue) Then bValidMaster = False
					sDbMasterFilter = sDbMasterFilter.Replace("@CompanyID@", ew_AdjustSql(Company.CompanyID.QueryStringValue))
					sDbDetailFilter = sDbDetailFilter.Replace("@CompanyID@", ew_AdjustSql(Company.CompanyID.QueryStringValue))
				Else
					bValidMaster = False
				End If
			End If
		End If
		If bValidMaster Then

			' Save current master table
			zPage.CurrentMasterTable = sMasterTblVar
			zPage.MasterFilter = sDbMasterFilter ' Set up master filter
			zPage.DetailFilter = sDbDetailFilter ' Set up detail filter

			' Clear previous master session values
			If sMasterTblVar <> "Company" Then
				If zPage.CompanyID.QueryStringValue = "" Then zPage.CompanyID.SessionValue = ""
			End If
		End If
	End Sub

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "Page"
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
		Dim table As String = "Page"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsSrc("PageID")

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

		' ParentPageID Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "ParentPageID", keyvalue, oldvalue, RsSrc("ParentPageID"))

		' PageName Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "PageName", keyvalue, oldvalue, RsSrc("PageName"))

		' PageTitle Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "PageTitle", keyvalue, oldvalue, RsSrc("PageTitle"))

		' PageTypeID Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "PageTypeID", keyvalue, oldvalue, RsSrc("PageTypeID"))

		' GroupID Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "GroupID", keyvalue, oldvalue, RsSrc("GroupID"))

		' Active Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "Active", keyvalue, oldvalue, RsSrc("Active"))

		' PageOrder Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "PageOrder", keyvalue, oldvalue, RsSrc("PageOrder"))

		' CompanyID Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "CompanyID", keyvalue, oldvalue, RsSrc("CompanyID"))

		' PageDescription Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "PageDescription", keyvalue, oldvalue, RsSrc("PageDescription"))

		' PageKeywords Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "PageKeywords", keyvalue, oldvalue, RsSrc("PageKeywords"))

		' ImagesPerRow Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "ImagesPerRow", keyvalue, oldvalue, RsSrc("ImagesPerRow"))

		' RowsPerPage Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "RowsPerPage", keyvalue, oldvalue, RsSrc("RowsPerPage"))

		' PageFileName Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "PageFileName", keyvalue, oldvalue, RsSrc("PageFileName"))

		' AllowMessage Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "AllowMessage", keyvalue, oldvalue, RsSrc("AllowMessage"))

		' SiteCategoryID Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "SiteCategoryID", keyvalue, oldvalue, RsSrc("SiteCategoryID"))

		' SiteCategoryGroupID Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "SiteCategoryGroupID", keyvalue, oldvalue, RsSrc("SiteCategoryGroupID"))

		' VersionNo Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "VersionNo", keyvalue, oldvalue, RsSrc("VersionNo"))

		' ModifiedDT Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "ModifiedDT", keyvalue, oldvalue, RsSrc("ModifiedDT"))
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
		zPage_add = New czPage_add(Me)		
		zPage_add.Page_Init()		

		' Set buffer/cache option
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

		' Page main processing
		zPage_add.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If zPage_add IsNot Nothing Then zPage_add.Dispose()
	End Sub
End Class
