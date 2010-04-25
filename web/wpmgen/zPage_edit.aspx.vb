Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class zPage_edit
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public zPage_edit As czPage_edit

	'
	' Page Class
	'
	Class czPage_edit
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
				If zPage.UseTokenInUrl Then Url = Url & "t=" & zPage.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

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

		' zPage
		Public Property zPage() As czPage
			Get				
				Return ParentPage.zPage
			End Get
			Set(ByVal v As czPage)
				ParentPage.zPage = v	
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
			m_PageObjName = "zPage_edit"
			m_PageObjTypeName = "czPage_edit"

			' Table Name
			m_TableName = "Page"

			' Initialize table object
			zPage = New czPage(Me)

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
			zPage.Dispose()

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
		If ew_Get("zPageID") <> "" Then
			zPage.zPageID.QueryStringValue = ew_Get("zPageID")
		End If

		' Create form object
		ObjForm = New cFormObj
		If ObjForm.GetValue("a_edit") <> "" Then
			zPage.CurrentAction = ObjForm.GetValue("a_edit") ' Get action code
			LoadFormValues() ' Get form values

			' Validate Form
			If Not ValidateForm() Then
				zPage.CurrentAction = "" ' Form error, reset action
				Message = ParentPage.gsFormError
				LoadRow() ' Restore row
				RestoreFormValues() ' Restore form values if validate failed
			End If
		Else
			zPage.CurrentAction = "I" ' Default action is display
		End If

		' Check if valid key
		If ew_Empty(zPage.zPageID.CurrentValue) Then Page_Terminate("zPage_list.aspx") ' Invalid key, return to list
		Select Case zPage.CurrentAction
			Case "I" ' Get a record to display
				If Not LoadRow() Then ' Load Record based on key
					Message = "No records found" ' No record found
					Page_Terminate("zPage_list.aspx") ' No matching record, return to list
				End If
			Case "U" ' Update
				zPage.SendEmail = True ' Send email on update success
				If EditRow() Then ' Update Record based on key
					Message = "Update succeeded" ' Update success
					Dim sReturnUrl As String = zPage.ReturnUrl
					If ew_GetPageName(sReturnUrl) = "zPage_view.aspx" Then sReturnUrl = zPage.ViewUrl ' View paging, return to view page with keyurl directly
					Page_Terminate(sReturnUrl) ' Return to caller
				Else
					LoadRow() ' Restore row
					RestoreFormValues() ' Restore form values if update failed
				End If
		End Select

		' Render the record
		zPage.RowType = EW_ROWTYPE_EDIT ' Render as edit

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
		zPage.zPageID.FormValue = ObjForm.GetValue("x_zPageID")
		zPage.zPageID.OldValue = ObjForm.GetValue("o_zPageID")
		zPage.CompanyID.FormValue = ObjForm.GetValue("x_CompanyID")
		zPage.CompanyID.OldValue = ObjForm.GetValue("o_CompanyID")
		zPage.PageOrder.FormValue = ObjForm.GetValue("x_PageOrder")
		zPage.PageOrder.OldValue = ObjForm.GetValue("o_PageOrder")
		zPage.GroupID.FormValue = ObjForm.GetValue("x_GroupID")
		zPage.GroupID.OldValue = ObjForm.GetValue("o_GroupID")
		zPage.ParentPageID.FormValue = ObjForm.GetValue("x_ParentPageID")
		zPage.ParentPageID.OldValue = ObjForm.GetValue("o_ParentPageID")
		zPage.PageTypeID.FormValue = ObjForm.GetValue("x_PageTypeID")
		zPage.PageTypeID.OldValue = ObjForm.GetValue("o_PageTypeID")
		zPage.Active.FormValue = ObjForm.GetValue("x_Active")
		zPage.Active.OldValue = ObjForm.GetValue("o_Active")
		zPage.zPageName.FormValue = ObjForm.GetValue("x_zPageName")
		zPage.zPageName.OldValue = ObjForm.GetValue("o_zPageName")
		zPage.PageTitle.FormValue = ObjForm.GetValue("x_PageTitle")
		zPage.PageTitle.OldValue = ObjForm.GetValue("o_PageTitle")
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
		zPage.VersionNo.FormValue = ObjForm.GetValue("x_VersionNo")
		zPage.VersionNo.OldValue = ObjForm.GetValue("o_VersionNo")
		zPage.SiteCategoryID.FormValue = ObjForm.GetValue("x_SiteCategoryID")
		zPage.SiteCategoryID.OldValue = ObjForm.GetValue("o_SiteCategoryID")
		zPage.SiteCategoryGroupID.FormValue = ObjForm.GetValue("x_SiteCategoryGroupID")
		zPage.SiteCategoryGroupID.OldValue = ObjForm.GetValue("o_SiteCategoryGroupID")
		zPage.ModifiedDT.FormValue = ObjForm.GetValue("x_ModifiedDT")
		zPage.ModifiedDT.CurrentValue = ew_UnFormatDateTime(zPage.ModifiedDT.CurrentValue, 6)
		zPage.ModifiedDT.OldValue = ObjForm.GetValue("o_ModifiedDT")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		zPage.zPageID.CurrentValue = zPage.zPageID.FormValue
		zPage.CompanyID.CurrentValue = zPage.CompanyID.FormValue
		zPage.PageOrder.CurrentValue = zPage.PageOrder.FormValue
		zPage.GroupID.CurrentValue = zPage.GroupID.FormValue
		zPage.ParentPageID.CurrentValue = zPage.ParentPageID.FormValue
		zPage.PageTypeID.CurrentValue = zPage.PageTypeID.FormValue
		zPage.Active.CurrentValue = zPage.Active.FormValue
		zPage.zPageName.CurrentValue = zPage.zPageName.FormValue
		zPage.PageTitle.CurrentValue = zPage.PageTitle.FormValue
		zPage.PageDescription.CurrentValue = zPage.PageDescription.FormValue
		zPage.PageKeywords.CurrentValue = zPage.PageKeywords.FormValue
		zPage.ImagesPerRow.CurrentValue = zPage.ImagesPerRow.FormValue
		zPage.RowsPerPage.CurrentValue = zPage.RowsPerPage.FormValue
		zPage.PageFileName.CurrentValue = zPage.PageFileName.FormValue
		zPage.VersionNo.CurrentValue = zPage.VersionNo.FormValue
		zPage.SiteCategoryID.CurrentValue = zPage.SiteCategoryID.FormValue
		zPage.SiteCategoryGroupID.CurrentValue = zPage.SiteCategoryGroupID.FormValue
		zPage.ModifiedDT.CurrentValue = zPage.ModifiedDT.FormValue
		zPage.ModifiedDT.CurrentValue = ew_UnFormatDateTime(zPage.ModifiedDT.CurrentValue, 6)
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
		If EW_DEBUG_ENABLED Then ew_Write(sSql)
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
		zPage.zPageID.DbValue = RsRow("PageID")
		zPage.CompanyID.DbValue = RsRow("CompanyID")
		zPage.PageOrder.DbValue = RsRow("PageOrder")
		zPage.GroupID.DbValue = RsRow("GroupID")
		zPage.ParentPageID.DbValue = RsRow("ParentPageID")
		zPage.PageTypeID.DbValue = RsRow("PageTypeID")
		zPage.Active.DbValue = IIf(ew_ConvertToBool(RsRow("Active")), "1", "0")
		zPage.zPageName.DbValue = RsRow("PageName")
		zPage.PageTitle.DbValue = RsRow("PageTitle")
		zPage.PageDescription.DbValue = RsRow("PageDescription")
		zPage.PageKeywords.DbValue = RsRow("PageKeywords")
		zPage.ImagesPerRow.DbValue = RsRow("ImagesPerRow")
		zPage.RowsPerPage.DbValue = RsRow("RowsPerPage")
		zPage.AllowMessage.DbValue = IIf(ew_ConvertToBool(RsRow("AllowMessage")), "1", "0")
		zPage.PageFileName.DbValue = RsRow("PageFileName")
		zPage.VersionNo.DbValue = RsRow("VersionNo")
		zPage.SiteCategoryID.DbValue = RsRow("SiteCategoryID")
		zPage.SiteCategoryGroupID.DbValue = RsRow("SiteCategoryGroupID")
		zPage.ModifiedDT.DbValue = RsRow("ModifiedDT")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		zPage.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' PageID

		zPage.zPageID.CellCssStyle = ""
		zPage.zPageID.CellCssClass = ""

		' CompanyID
		zPage.CompanyID.CellCssStyle = ""
		zPage.CompanyID.CellCssClass = ""

		' PageOrder
		zPage.PageOrder.CellCssStyle = ""
		zPage.PageOrder.CellCssClass = ""

		' GroupID
		zPage.GroupID.CellCssStyle = ""
		zPage.GroupID.CellCssClass = ""

		' ParentPageID
		zPage.ParentPageID.CellCssStyle = ""
		zPage.ParentPageID.CellCssClass = ""

		' PageTypeID
		zPage.PageTypeID.CellCssStyle = ""
		zPage.PageTypeID.CellCssClass = ""

		' Active
		zPage.Active.CellCssStyle = ""
		zPage.Active.CellCssClass = ""

		' PageName
		zPage.zPageName.CellCssStyle = ""
		zPage.zPageName.CellCssClass = ""

		' PageTitle
		zPage.PageTitle.CellCssStyle = ""
		zPage.PageTitle.CellCssClass = ""

		' PageDescription
		zPage.PageDescription.CellCssStyle = ""
		zPage.PageDescription.CellCssClass = ""

		' PageKeywords
		zPage.PageKeywords.CellCssStyle = ""
		zPage.PageKeywords.CellCssClass = ""

		' ImagesPerRow
		zPage.ImagesPerRow.CellCssStyle = ""
		zPage.ImagesPerRow.CellCssClass = ""

		' RowsPerPage
		zPage.RowsPerPage.CellCssStyle = ""
		zPage.RowsPerPage.CellCssClass = ""

		' PageFileName
		zPage.PageFileName.CellCssStyle = ""
		zPage.PageFileName.CellCssClass = ""

		' VersionNo
		zPage.VersionNo.CellCssStyle = ""
		zPage.VersionNo.CellCssClass = ""

		' SiteCategoryID
		zPage.SiteCategoryID.CellCssStyle = ""
		zPage.SiteCategoryID.CellCssClass = ""

		' SiteCategoryGroupID
		zPage.SiteCategoryGroupID.CellCssStyle = ""
		zPage.SiteCategoryGroupID.CellCssClass = ""

		' ModifiedDT
		zPage.ModifiedDT.CellCssStyle = ""
		zPage.ModifiedDT.CellCssClass = ""

		'
		'  View  Row
		'

		If zPage.RowType = EW_ROWTYPE_VIEW Then ' View row

			' PageID
			zPage.zPageID.ViewValue = zPage.zPageID.CurrentValue
			zPage.zPageID.CssStyle = ""
			zPage.zPageID.CssClass = ""
			zPage.zPageID.ViewCustomAttributes = ""

			' CompanyID
			If ew_NotEmpty(zPage.CompanyID.CurrentValue) Then
				sSqlWrk = "SELECT [CompanyName] FROM [Company] WHERE [CompanyID] = " & ew_AdjustSql(zPage.CompanyID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
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

			' PageOrder
			zPage.PageOrder.ViewValue = zPage.PageOrder.CurrentValue
			zPage.PageOrder.CssStyle = ""
			zPage.PageOrder.CssClass = ""
			zPage.PageOrder.ViewCustomAttributes = ""

			' GroupID
			If ew_NotEmpty(zPage.GroupID.CurrentValue) Then
				sSqlWrk = "SELECT [GroupName] FROM [Group] WHERE [GroupID] = " & ew_AdjustSql(zPage.GroupID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [GroupID] Asc"
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

			' ParentPageID
			If ew_NotEmpty(zPage.ParentPageID.CurrentValue) Then
				sSqlWrk = "SELECT [PageName] FROM [Page] WHERE [PageID] = " & ew_AdjustSql(zPage.ParentPageID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [PageName] Asc"
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

			' PageTypeID
			If ew_NotEmpty(zPage.PageTypeID.CurrentValue) Then
				sSqlWrk = "SELECT [PageTypeDesc] FROM [PageType] WHERE [PageTypeID] = " & ew_AdjustSql(zPage.PageTypeID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [PageTypeDesc] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					zPage.PageTypeID.ViewValue = RsWrk("PageTypeDesc")
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

			' Active
			If Convert.ToString(zPage.Active.CurrentValue) = "1" Then
				zPage.Active.ViewValue = "Yes"
			Else
				zPage.Active.ViewValue = "No"
			End If
			zPage.Active.CssStyle = ""
			zPage.Active.CssClass = ""
			zPage.Active.ViewCustomAttributes = ""

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

			' AllowMessage
			If Convert.ToString(zPage.AllowMessage.CurrentValue) = "1" Then
				zPage.AllowMessage.ViewValue = "Yes"
			Else
				zPage.AllowMessage.ViewValue = "No"
			End If
			zPage.AllowMessage.CssStyle = ""
			zPage.AllowMessage.CssClass = ""
			zPage.AllowMessage.ViewCustomAttributes = ""

			' PageFileName
			zPage.PageFileName.ViewValue = zPage.PageFileName.CurrentValue
			zPage.PageFileName.CssStyle = ""
			zPage.PageFileName.CssClass = ""
			zPage.PageFileName.ViewCustomAttributes = ""

			' VersionNo
			zPage.VersionNo.ViewValue = zPage.VersionNo.CurrentValue
			zPage.VersionNo.CssStyle = ""
			zPage.VersionNo.CssClass = ""
			zPage.VersionNo.ViewCustomAttributes = ""

			' SiteCategoryID
			If ew_NotEmpty(zPage.SiteCategoryID.CurrentValue) Then
				sSqlWrk = "SELECT [CategoryName] FROM [SiteCategory] WHERE [SiteCategoryID] = " & ew_AdjustSql(zPage.SiteCategoryID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [CategoryName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					zPage.SiteCategoryID.ViewValue = RsWrk("CategoryName")
				Else
					zPage.SiteCategoryID.ViewValue = zPage.SiteCategoryID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				zPage.SiteCategoryID.ViewValue = System.DBNull.Value
			End If
			zPage.SiteCategoryID.CssStyle = ""
			zPage.SiteCategoryID.CssClass = ""
			zPage.SiteCategoryID.ViewCustomAttributes = ""

			' SiteCategoryGroupID
			If ew_NotEmpty(zPage.SiteCategoryGroupID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteCategoryGroupNM] FROM [SiteCategoryGroup] WHERE [SiteCategoryGroupID] = " & ew_AdjustSql(zPage.SiteCategoryGroupID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryGroupNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					zPage.SiteCategoryGroupID.ViewValue = RsWrk("SiteCategoryGroupNM")
				Else
					zPage.SiteCategoryGroupID.ViewValue = zPage.SiteCategoryGroupID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				zPage.SiteCategoryGroupID.ViewValue = System.DBNull.Value
			End If
			zPage.SiteCategoryGroupID.CssStyle = ""
			zPage.SiteCategoryGroupID.CssClass = ""
			zPage.SiteCategoryGroupID.ViewCustomAttributes = ""

			' ModifiedDT
			zPage.ModifiedDT.ViewValue = zPage.ModifiedDT.CurrentValue
			zPage.ModifiedDT.ViewValue = ew_FormatDateTime(zPage.ModifiedDT.ViewValue, 6)
			zPage.ModifiedDT.CssStyle = ""
			zPage.ModifiedDT.CssClass = ""
			zPage.ModifiedDT.ViewCustomAttributes = ""

			' View refer script
			' PageID

			zPage.zPageID.HrefValue = ""

			' CompanyID
			zPage.CompanyID.HrefValue = ""

			' PageOrder
			zPage.PageOrder.HrefValue = ""

			' GroupID
			zPage.GroupID.HrefValue = ""

			' ParentPageID
			zPage.ParentPageID.HrefValue = ""

			' PageTypeID
			zPage.PageTypeID.HrefValue = ""

			' Active
			zPage.Active.HrefValue = ""

			' PageName
			zPage.zPageName.HrefValue = ""

			' PageTitle
			zPage.PageTitle.HrefValue = ""

			' PageDescription
			zPage.PageDescription.HrefValue = ""

			' PageKeywords
			zPage.PageKeywords.HrefValue = ""

			' ImagesPerRow
			zPage.ImagesPerRow.HrefValue = ""

			' RowsPerPage
			zPage.RowsPerPage.HrefValue = ""

			' PageFileName
			zPage.PageFileName.HrefValue = ""

			' VersionNo
			zPage.VersionNo.HrefValue = ""

			' SiteCategoryID
			zPage.SiteCategoryID.HrefValue = ""

			' SiteCategoryGroupID
			zPage.SiteCategoryGroupID.HrefValue = ""

			' ModifiedDT
			zPage.ModifiedDT.HrefValue = ""

		'
		'  Edit Row
		'

		ElseIf zPage.RowType = EW_ROWTYPE_EDIT Then ' Edit row

			' PageID
			zPage.zPageID.EditCustomAttributes = ""

			' CompanyID
			zPage.CompanyID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [CompanyID], [CompanyName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Company]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			zPage.CompanyID.EditValue = arwrk

			' PageOrder
			zPage.PageOrder.EditCustomAttributes = ""
			zPage.PageOrder.EditValue = ew_HtmlEncode(zPage.PageOrder.CurrentValue)

			' GroupID
			zPage.GroupID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [GroupID], [GroupName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Group]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [GroupID] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			zPage.GroupID.EditValue = arwrk

			' ParentPageID
			zPage.ParentPageID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [PageID], [PageName], '' AS Disp2Fld, [CompanyID] FROM [Page]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [PageName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select", ""}) 
			zPage.ParentPageID.EditValue = arwrk

			' PageTypeID
			zPage.PageTypeID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [PageTypeID], [PageTypeDesc], '' AS Disp2Fld, '' AS SelectFilterFld FROM [PageType]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [PageTypeDesc] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			zPage.PageTypeID.EditValue = arwrk

			' Active
			zPage.Active.EditCustomAttributes = ""

			' PageName
			zPage.zPageName.EditCustomAttributes = ""
			zPage.zPageName.EditValue = ew_HtmlEncode(zPage.zPageName.CurrentValue)

			' PageTitle
			zPage.PageTitle.EditCustomAttributes = ""
			zPage.PageTitle.EditValue = ew_HtmlEncode(zPage.PageTitle.CurrentValue)

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

			' VersionNo
			zPage.VersionNo.EditCustomAttributes = ""
			zPage.VersionNo.EditValue = ew_HtmlEncode(zPage.VersionNo.CurrentValue)

			' SiteCategoryID
			zPage.SiteCategoryID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryID], [CategoryName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteCategory]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [CategoryName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			zPage.SiteCategoryID.EditValue = arwrk

			' SiteCategoryGroupID
			zPage.SiteCategoryGroupID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryGroupID], [SiteCategoryGroupNM], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteCategoryGroup]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryGroupNM] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			zPage.SiteCategoryGroupID.EditValue = arwrk

			' ModifiedDT
			' Edit refer script
			' PageID

			zPage.zPageID.HrefValue = ""

			' CompanyID
			zPage.CompanyID.HrefValue = ""

			' PageOrder
			zPage.PageOrder.HrefValue = ""

			' GroupID
			zPage.GroupID.HrefValue = ""

			' ParentPageID
			zPage.ParentPageID.HrefValue = ""

			' PageTypeID
			zPage.PageTypeID.HrefValue = ""

			' Active
			zPage.Active.HrefValue = ""

			' PageName
			zPage.zPageName.HrefValue = ""

			' PageTitle
			zPage.PageTitle.HrefValue = ""

			' PageDescription
			zPage.PageDescription.HrefValue = ""

			' PageKeywords
			zPage.PageKeywords.HrefValue = ""

			' ImagesPerRow
			zPage.ImagesPerRow.HrefValue = ""

			' RowsPerPage
			zPage.RowsPerPage.HrefValue = ""

			' PageFileName
			zPage.PageFileName.HrefValue = ""

			' VersionNo
			zPage.VersionNo.HrefValue = ""

			' SiteCategoryID
			zPage.SiteCategoryID.HrefValue = ""

			' SiteCategoryGroupID
			zPage.SiteCategoryGroupID.HrefValue = ""

			' ModifiedDT
			zPage.ModifiedDT.HrefValue = ""
		End If

		' Row Rendered event
		zPage.Row_Rendered()
	End Sub

	'
	' Validate form
	'
	Function ValidateForm() As Boolean

		' Initialize
		ParentPage.gsFormError = ""

		' Check if validation required
		If Not EW_SERVER_VALIDATE Then Return (ParentPage.gsFormError = "")
		If ew_Empty(zPage.CompanyID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter required field - Company"
		End If
		If ew_Empty(zPage.PageOrder.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter required field - Order"
		End If
		If Not ew_CheckInteger(zPage.PageOrder.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Incorrect integer - Order"
		End If
		If ew_Empty(zPage.GroupID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter required field - Group"
		End If
		If ew_Empty(zPage.PageTypeID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter required field - PageType"
		End If
		If ew_Empty(zPage.zPageName.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter required field - Page Name"
		End If
		If Not ew_CheckInteger(zPage.ImagesPerRow.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Incorrect integer - Images Per Row"
		End If
		If Not ew_CheckInteger(zPage.RowsPerPage.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Incorrect integer - Rows Per Page"
		End If
		If Not ew_CheckInteger(zPage.VersionNo.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Incorrect integer - Version No"
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
		sFilter = zPage.KeyFilter
		zPage.CurrentFilter  = sFilter
		sSql = zPage.SQL
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

			' PageID
			' CompanyID

			zPage.CompanyID.SetDbValue(zPage.CompanyID.CurrentValue, System.DBNull.Value)
			Rs("CompanyID") = zPage.CompanyID.DbValue

			' PageOrder
			zPage.PageOrder.SetDbValue(zPage.PageOrder.CurrentValue, 0)
			Rs("PageOrder") = zPage.PageOrder.DbValue

			' GroupID
			zPage.GroupID.SetDbValue(zPage.GroupID.CurrentValue, System.DBNull.Value)
			Rs("GroupID") = zPage.GroupID.DbValue

			' ParentPageID
			zPage.ParentPageID.SetDbValue(zPage.ParentPageID.CurrentValue, System.DBNull.Value)
			Rs("ParentPageID") = zPage.ParentPageID.DbValue

			' PageTypeID
			zPage.PageTypeID.SetDbValue(zPage.PageTypeID.CurrentValue, System.DBNull.Value)
			Rs("PageTypeID") = zPage.PageTypeID.DbValue

			' Active
			zPage.Active.SetDbValue((zPage.Active.CurrentValue <> "" And Not IsDBNull(zPage.Active.CurrentValue)), System.DBNull.Value)
			Rs("Active") = zPage.Active.DbValue

			' PageName
			zPage.zPageName.SetDbValue(zPage.zPageName.CurrentValue, "")
			Rs("PageName") = zPage.zPageName.DbValue

			' PageTitle
			zPage.PageTitle.SetDbValue(zPage.PageTitle.CurrentValue, System.DBNull.Value)
			Rs("PageTitle") = zPage.PageTitle.DbValue

			' PageDescription
			zPage.PageDescription.SetDbValue(zPage.PageDescription.CurrentValue, System.DBNull.Value)
			Rs("PageDescription") = zPage.PageDescription.DbValue

			' PageKeywords
			zPage.PageKeywords.SetDbValue(zPage.PageKeywords.CurrentValue, System.DBNull.Value)
			Rs("PageKeywords") = zPage.PageKeywords.DbValue

			' ImagesPerRow
			zPage.ImagesPerRow.SetDbValue(zPage.ImagesPerRow.CurrentValue, System.DBNull.Value)
			Rs("ImagesPerRow") = zPage.ImagesPerRow.DbValue

			' RowsPerPage
			zPage.RowsPerPage.SetDbValue(zPage.RowsPerPage.CurrentValue, System.DBNull.Value)
			Rs("RowsPerPage") = zPage.RowsPerPage.DbValue

			' PageFileName
			zPage.PageFileName.SetDbValue(zPage.PageFileName.CurrentValue, System.DBNull.Value)
			Rs("PageFileName") = zPage.PageFileName.DbValue

			' VersionNo
			zPage.VersionNo.SetDbValue(zPage.VersionNo.CurrentValue, System.DBNull.Value)
			Rs("VersionNo") = zPage.VersionNo.DbValue

			' SiteCategoryID
			zPage.SiteCategoryID.SetDbValue(zPage.SiteCategoryID.CurrentValue, System.DBNull.Value)
			Rs("SiteCategoryID") = zPage.SiteCategoryID.DbValue

			' SiteCategoryGroupID
			zPage.SiteCategoryGroupID.SetDbValue(zPage.SiteCategoryGroupID.CurrentValue, System.DBNull.Value)
			Rs("SiteCategoryGroupID") = zPage.SiteCategoryGroupID.DbValue

			' ModifiedDT
			zPage.ModifiedDT.DbValue = ew_CurrentDate()
			Rs("ModifiedDT") = zPage.ModifiedDT.DbValue

			' Row Updating event
			bUpdateRow = zPage.Row_Updating(RsOld, Rs)
			If bUpdateRow Then
				Try
					zPage.Update(Rs)
					EditRow = True
				Catch e As Exception
					If EW_DEBUG_ENABLED Then Throw
					Message = e.Message
					EditRow = False
				End Try
			Else
				If zPage.CancelMessage <> "" Then
					Message = zPage.CancelMessage
					zPage.CancelMessage = ""
				Else
					Message = "Update cancelled"
				End If
				EditRow = False
			End If
		End If

		' Row Updated event
		If EditRow Then
			zPage.Row_Updated(RsOld, Rs)
		End If
		If EditRow Then
			WriteAuditTrailOnEdit(RsOld, Rs)
		End If
	End Function

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "Page"
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
		Dim table As String = "Page"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsOld("PageID")

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
			fld = zPage.FieldByName(fldname)
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
		zPage_edit = New czPage_edit(Me)		
		zPage_edit.Page_Init()

		' Page main processing
		zPage_edit.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If zPage_edit IsNot Nothing Then zPage_edit.Dispose()
	End Sub
End Class
