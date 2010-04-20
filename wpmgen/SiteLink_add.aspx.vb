Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class SiteLink_add
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public SiteLink_add As cSiteLink_add

	'
	' Page Class
	'
	Class cSiteLink_add
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
				If SiteLink.UseTokenInUrl Then Url = Url & "t=" & SiteLink.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If SiteLink.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (SiteLink.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (SiteLink.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' SiteLink
		Public Property SiteLink() As cSiteLink
			Get				
				Return ParentPage.SiteLink
			End Get
			Set(ByVal v As cSiteLink)
				ParentPage.SiteLink = v	
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
			m_PageObjName = "SiteLink_add"
			m_PageObjTypeName = "cSiteLink_add"

			' Table Name
			m_TableName = "SiteLink"

			' Initialize table object
			SiteLink = New cSiteLink(Me)

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
			SiteLink.Dispose()

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
		If ew_Get("ID") <> "" Then
			SiteLink.ID.QueryStringValue = ew_Get("ID")
		Else
			bCopy = False
		End If

		' Create form object
		ObjForm = New cFormObj

		' Process form if post back
		If ObjForm.GetValue("a_add") <> "" Then
			SiteLink.CurrentAction = ObjForm.GetValue("a_add") ' Get form action
			LoadFormValues() ' Load form values

			' Validate Form
			If Not ValidateForm() Then
				SiteLink.CurrentAction = "I" ' Form error, reset action
				Message = ParentPage.gsFormError
			End If

		' Not post back
		Else
			If bCopy Then
				SiteLink.CurrentAction = "C" ' Copy Record
			Else
				SiteLink.CurrentAction = "I" ' Display Blank Record
				LoadDefaultValues() ' Load default values
			End If
		End If

		' Perform action based on action code
		Select Case SiteLink.CurrentAction
			Case "I" ' Blank record, no action required
			Case "C" ' Copy an existing record
				If Not LoadRow() Then ' Load record based on key
					Message = "No records found" ' No record found
					Page_Terminate("SiteLink_list.aspx") ' No matching record, return to list
				End If
			Case "A" ' Add new record
				SiteLink.SendEmail = True ' Send email on add success
				If AddRow() Then ' Add successful
					Message = "Add succeeded" ' Set up success message
					Dim sReturnUrl As String = SiteLink.ReturnUrl
					If ew_GetPageName(sReturnUrl) = "SiteLink_view.aspx" Then sReturnUrl = SiteLink.ViewUrl ' View paging, return to view page with keyurl directly
					Page_Terminate(sReturnUrl) ' Clean up and return
				Else
					RestoreFormValues() ' Add failed, restore form values
				End If
		End Select

		' Render row based on row type
		SiteLink.RowType = EW_ROWTYPE_ADD ' Render add type

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
		SiteLink.CategoryID.CurrentValue = 0
		SiteLink.CompanyID.CurrentValue = 0
		SiteLink.Ranks.CurrentValue = 0
		SiteLink.Views.CurrentValue = 0
	End Sub

	'
	' Load form values
	'
	Sub LoadFormValues()
		SiteLink.SiteCategoryTypeID.FormValue = ObjForm.GetValue("x_SiteCategoryTypeID")
		SiteLink.SiteCategoryTypeID.OldValue = ObjForm.GetValue("o_SiteCategoryTypeID")
		SiteLink.Title.FormValue = ObjForm.GetValue("x_Title")
		SiteLink.Title.OldValue = ObjForm.GetValue("o_Title")
		SiteLink.LinkTypeCD.FormValue = ObjForm.GetValue("x_LinkTypeCD")
		SiteLink.LinkTypeCD.OldValue = ObjForm.GetValue("o_LinkTypeCD")
		SiteLink.CategoryID.FormValue = ObjForm.GetValue("x_CategoryID")
		SiteLink.CategoryID.OldValue = ObjForm.GetValue("o_CategoryID")
		SiteLink.CompanyID.FormValue = ObjForm.GetValue("x_CompanyID")
		SiteLink.CompanyID.OldValue = ObjForm.GetValue("o_CompanyID")
		SiteLink.SiteCategoryID.FormValue = ObjForm.GetValue("x_SiteCategoryID")
		SiteLink.SiteCategoryID.OldValue = ObjForm.GetValue("o_SiteCategoryID")
		SiteLink.SiteCategoryGroupID.FormValue = ObjForm.GetValue("x_SiteCategoryGroupID")
		SiteLink.SiteCategoryGroupID.OldValue = ObjForm.GetValue("o_SiteCategoryGroupID")
		SiteLink.Description.FormValue = ObjForm.GetValue("x_Description")
		SiteLink.Description.OldValue = ObjForm.GetValue("o_Description")
		SiteLink.URL.FormValue = ObjForm.GetValue("x_URL")
		SiteLink.URL.OldValue = ObjForm.GetValue("o_URL")
		SiteLink.Ranks.FormValue = ObjForm.GetValue("x_Ranks")
		SiteLink.Ranks.OldValue = ObjForm.GetValue("o_Ranks")
		SiteLink.Views.FormValue = ObjForm.GetValue("x_Views")
		SiteLink.Views.OldValue = ObjForm.GetValue("o_Views")
		SiteLink.ID.FormValue = ObjForm.GetValue("x_ID")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		SiteLink.SiteCategoryTypeID.CurrentValue = SiteLink.SiteCategoryTypeID.FormValue
		SiteLink.Title.CurrentValue = SiteLink.Title.FormValue
		SiteLink.LinkTypeCD.CurrentValue = SiteLink.LinkTypeCD.FormValue
		SiteLink.CategoryID.CurrentValue = SiteLink.CategoryID.FormValue
		SiteLink.CompanyID.CurrentValue = SiteLink.CompanyID.FormValue
		SiteLink.SiteCategoryID.CurrentValue = SiteLink.SiteCategoryID.FormValue
		SiteLink.SiteCategoryGroupID.CurrentValue = SiteLink.SiteCategoryGroupID.FormValue
		SiteLink.Description.CurrentValue = SiteLink.Description.FormValue
		SiteLink.URL.CurrentValue = SiteLink.URL.FormValue
		SiteLink.Ranks.CurrentValue = SiteLink.Ranks.FormValue
		SiteLink.Views.CurrentValue = SiteLink.Views.FormValue
		SiteLink.ID.CurrentValue = SiteLink.ID.FormValue
	End Sub

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = SiteLink.KeyFilter

		' Row Selecting event
		SiteLink.Row_Selecting(sFilter)

		' Load SQL based on filter
		SiteLink.CurrentFilter = sFilter
		Dim sSql As String = SiteLink.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				SiteLink.Row_Selected(RsRow)
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
		SiteLink.ID.DbValue = RsRow("ID")
		SiteLink.SiteCategoryTypeID.DbValue = RsRow("SiteCategoryTypeID")
		SiteLink.Title.DbValue = RsRow("Title")
		SiteLink.LinkTypeCD.DbValue = RsRow("LinkTypeCD")
		SiteLink.CategoryID.DbValue = RsRow("CategoryID")
		SiteLink.CompanyID.DbValue = RsRow("CompanyID")
		SiteLink.SiteCategoryID.DbValue = RsRow("SiteCategoryID")
		SiteLink.SiteCategoryGroupID.DbValue = RsRow("SiteCategoryGroupID")
		SiteLink.Description.DbValue = RsRow("Description")
		SiteLink.URL.DbValue = RsRow("URL")
		SiteLink.Ranks.DbValue = RsRow("Ranks")
		SiteLink.Views.DbValue = IIf(ew_ConvertToBool(RsRow("Views")), "1", "0")
		SiteLink.DateAdd.DbValue = RsRow("DateAdd")
		SiteLink.UserName.DbValue = RsRow("UserName")
		SiteLink.UserID.DbValue = RsRow("UserID")
		SiteLink.ASIN.DbValue = RsRow("ASIN")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		SiteLink.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' SiteCategoryTypeID

		SiteLink.SiteCategoryTypeID.CellCssStyle = ""
		SiteLink.SiteCategoryTypeID.CellCssClass = ""

		' Title
		SiteLink.Title.CellCssStyle = ""
		SiteLink.Title.CellCssClass = ""

		' LinkTypeCD
		SiteLink.LinkTypeCD.CellCssStyle = ""
		SiteLink.LinkTypeCD.CellCssClass = ""

		' CategoryID
		SiteLink.CategoryID.CellCssStyle = ""
		SiteLink.CategoryID.CellCssClass = ""

		' CompanyID
		SiteLink.CompanyID.CellCssStyle = ""
		SiteLink.CompanyID.CellCssClass = ""

		' SiteCategoryID
		SiteLink.SiteCategoryID.CellCssStyle = ""
		SiteLink.SiteCategoryID.CellCssClass = ""

		' SiteCategoryGroupID
		SiteLink.SiteCategoryGroupID.CellCssStyle = ""
		SiteLink.SiteCategoryGroupID.CellCssClass = ""

		' Description
		SiteLink.Description.CellCssStyle = ""
		SiteLink.Description.CellCssClass = ""

		' URL
		SiteLink.URL.CellCssStyle = ""
		SiteLink.URL.CellCssClass = ""

		' Ranks
		SiteLink.Ranks.CellCssStyle = ""
		SiteLink.Ranks.CellCssClass = ""

		' Views
		SiteLink.Views.CellCssStyle = ""
		SiteLink.Views.CellCssClass = ""

		'
		'  View  Row
		'

		If SiteLink.RowType = EW_ROWTYPE_VIEW Then ' View row

			' SiteCategoryTypeID
			If ew_NotEmpty(SiteLink.SiteCategoryTypeID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteCategoryTypeNM] FROM [SiteCategoryType] WHERE [SiteCategoryTypeID] = " & ew_AdjustSql(SiteLink.SiteCategoryTypeID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryTypeNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					SiteLink.SiteCategoryTypeID.ViewValue = RsWrk("SiteCategoryTypeNM")
				Else
					SiteLink.SiteCategoryTypeID.ViewValue = SiteLink.SiteCategoryTypeID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				SiteLink.SiteCategoryTypeID.ViewValue = System.DBNull.Value
			End If
			SiteLink.SiteCategoryTypeID.CssStyle = ""
			SiteLink.SiteCategoryTypeID.CssClass = ""
			SiteLink.SiteCategoryTypeID.ViewCustomAttributes = ""

			' Title
			SiteLink.Title.ViewValue = SiteLink.Title.CurrentValue
			SiteLink.Title.CssStyle = ""
			SiteLink.Title.CssClass = ""
			SiteLink.Title.ViewCustomAttributes = ""

			' LinkTypeCD
			If ew_NotEmpty(SiteLink.LinkTypeCD.CurrentValue) Then
				sSqlWrk = "SELECT [LinkTypeDesc] FROM [LinkType] WHERE [LinkTypeCD] = '" & ew_AdjustSql(SiteLink.LinkTypeCD.CurrentValue) & "'"
				sSqlWrk = sSqlWrk & " ORDER BY [LinkTypeDesc] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					SiteLink.LinkTypeCD.ViewValue = RsWrk("LinkTypeDesc")
				Else
					SiteLink.LinkTypeCD.ViewValue = SiteLink.LinkTypeCD.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				SiteLink.LinkTypeCD.ViewValue = System.DBNull.Value
			End If
			SiteLink.LinkTypeCD.CssStyle = ""
			SiteLink.LinkTypeCD.CssClass = ""
			SiteLink.LinkTypeCD.ViewCustomAttributes = ""

			' CategoryID
			If ew_NotEmpty(SiteLink.CategoryID.CurrentValue) Then
				sSqlWrk = "SELECT [Title] FROM [LinkCategory] WHERE [ID] = " & ew_AdjustSql(SiteLink.CategoryID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [Title] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					SiteLink.CategoryID.ViewValue = RsWrk("Title")
				Else
					SiteLink.CategoryID.ViewValue = SiteLink.CategoryID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				SiteLink.CategoryID.ViewValue = System.DBNull.Value
			End If
			SiteLink.CategoryID.CssStyle = ""
			SiteLink.CategoryID.CssClass = ""
			SiteLink.CategoryID.ViewCustomAttributes = ""

			' CompanyID
			If ew_NotEmpty(SiteLink.CompanyID.CurrentValue) Then
				sSqlWrk = "SELECT [CompanyName] FROM [Company] WHERE [CompanyID] = " & ew_AdjustSql(SiteLink.CompanyID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					SiteLink.CompanyID.ViewValue = RsWrk("CompanyName")
				Else
					SiteLink.CompanyID.ViewValue = SiteLink.CompanyID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				SiteLink.CompanyID.ViewValue = System.DBNull.Value
			End If
			SiteLink.CompanyID.CssStyle = ""
			SiteLink.CompanyID.CssClass = ""
			SiteLink.CompanyID.ViewCustomAttributes = ""

			' SiteCategoryID
			If ew_NotEmpty(SiteLink.SiteCategoryID.CurrentValue) Then
				sSqlWrk = "SELECT [CategoryName] FROM [SiteCategory] WHERE [SiteCategoryID] = " & ew_AdjustSql(SiteLink.SiteCategoryID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [CategoryName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					SiteLink.SiteCategoryID.ViewValue = RsWrk("CategoryName")
				Else
					SiteLink.SiteCategoryID.ViewValue = SiteLink.SiteCategoryID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				SiteLink.SiteCategoryID.ViewValue = System.DBNull.Value
			End If
			SiteLink.SiteCategoryID.CssStyle = ""
			SiteLink.SiteCategoryID.CssClass = ""
			SiteLink.SiteCategoryID.ViewCustomAttributes = ""

			' SiteCategoryGroupID
			If ew_NotEmpty(SiteLink.SiteCategoryGroupID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteCategoryGroupNM] FROM [SiteCategoryGroup] WHERE [SiteCategoryGroupID] = " & ew_AdjustSql(SiteLink.SiteCategoryGroupID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryGroupOrder] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					SiteLink.SiteCategoryGroupID.ViewValue = RsWrk("SiteCategoryGroupNM")
				Else
					SiteLink.SiteCategoryGroupID.ViewValue = SiteLink.SiteCategoryGroupID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				SiteLink.SiteCategoryGroupID.ViewValue = System.DBNull.Value
			End If
			SiteLink.SiteCategoryGroupID.CssStyle = ""
			SiteLink.SiteCategoryGroupID.CssClass = ""
			SiteLink.SiteCategoryGroupID.ViewCustomAttributes = ""

			' Description
			SiteLink.Description.ViewValue = SiteLink.Description.CurrentValue
			SiteLink.Description.CssStyle = ""
			SiteLink.Description.CssClass = ""
			SiteLink.Description.ViewCustomAttributes = ""

			' URL
			SiteLink.URL.ViewValue = SiteLink.URL.CurrentValue
			SiteLink.URL.CssStyle = ""
			SiteLink.URL.CssClass = ""
			SiteLink.URL.ViewCustomAttributes = ""

			' Ranks
			SiteLink.Ranks.ViewValue = SiteLink.Ranks.CurrentValue
			SiteLink.Ranks.CssStyle = ""
			SiteLink.Ranks.CssClass = ""
			SiteLink.Ranks.ViewCustomAttributes = ""

			' Views
			If Convert.ToString(SiteLink.Views.CurrentValue) = "1" Then
				SiteLink.Views.ViewValue = "Yes"
			Else
				SiteLink.Views.ViewValue = "No"
			End If
			SiteLink.Views.CssStyle = ""
			SiteLink.Views.CssClass = ""
			SiteLink.Views.ViewCustomAttributes = ""

			' View refer script
			' SiteCategoryTypeID

			SiteLink.SiteCategoryTypeID.HrefValue = ""

			' Title
			SiteLink.Title.HrefValue = ""

			' LinkTypeCD
			SiteLink.LinkTypeCD.HrefValue = ""

			' CategoryID
			SiteLink.CategoryID.HrefValue = ""

			' CompanyID
			SiteLink.CompanyID.HrefValue = ""

			' SiteCategoryID
			SiteLink.SiteCategoryID.HrefValue = ""

			' SiteCategoryGroupID
			SiteLink.SiteCategoryGroupID.HrefValue = ""

			' Description
			SiteLink.Description.HrefValue = ""

			' URL
			SiteLink.URL.HrefValue = ""

			' Ranks
			SiteLink.Ranks.HrefValue = ""

			' Views
			SiteLink.Views.HrefValue = ""

		'
		'  Add Row
		'

		ElseIf SiteLink.RowType = EW_ROWTYPE_ADD Then ' Add row

			' SiteCategoryTypeID
			SiteLink.SiteCategoryTypeID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryTypeID], [SiteCategoryTypeNM], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteCategoryType]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryTypeNM] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			SiteLink.SiteCategoryTypeID.EditValue = arwrk

			' Title
			SiteLink.Title.EditCustomAttributes = ""
			SiteLink.Title.EditValue = ew_HtmlEncode(SiteLink.Title.CurrentValue)

			' LinkTypeCD
			SiteLink.LinkTypeCD.EditCustomAttributes = ""
			sSqlWrk = "SELECT [LinkTypeCD], [LinkTypeDesc], '' AS Disp2Fld, '' AS SelectFilterFld FROM [LinkType]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [LinkTypeDesc] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			SiteLink.LinkTypeCD.EditValue = arwrk

			' CategoryID
			SiteLink.CategoryID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [ID], [Title], '' AS Disp2Fld, '' AS SelectFilterFld FROM [LinkCategory]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [Title] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			SiteLink.CategoryID.EditValue = arwrk

			' CompanyID
			SiteLink.CompanyID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [CompanyID], [CompanyName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Company]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			SiteLink.CompanyID.EditValue = arwrk

			' SiteCategoryID
			SiteLink.SiteCategoryID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryID], [CategoryName], '' AS Disp2Fld, [SiteCategoryTypeID] FROM [SiteCategory]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [CategoryName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select", ""}) 
			SiteLink.SiteCategoryID.EditValue = arwrk

			' SiteCategoryGroupID
			SiteLink.SiteCategoryGroupID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryGroupID], [SiteCategoryGroupNM], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteCategoryGroup]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryGroupOrder] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			SiteLink.SiteCategoryGroupID.EditValue = arwrk

			' Description
			SiteLink.Description.EditCustomAttributes = ""
			SiteLink.Description.EditValue = ew_HtmlEncode(SiteLink.Description.CurrentValue)

			' URL
			SiteLink.URL.EditCustomAttributes = ""
			SiteLink.URL.EditValue = ew_HtmlEncode(SiteLink.URL.CurrentValue)

			' Ranks
			SiteLink.Ranks.EditCustomAttributes = ""
			SiteLink.Ranks.EditValue = ew_HtmlEncode(SiteLink.Ranks.CurrentValue)

			' Views
			SiteLink.Views.EditCustomAttributes = ""
		End If

		' Row Rendered event
		SiteLink.Row_Rendered()
	End Sub

	'
	' Validate form
	'
	Function ValidateForm() As Boolean

		' Initialize
		ParentPage.gsFormError = ""

		' Check if validation required
		If Not EW_SERVER_VALIDATE Then Return (ParentPage.gsFormError = "")
		If ew_Empty(SiteLink.SiteCategoryTypeID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter required field - Site Type"
		End If
		If ew_Empty(SiteLink.Title.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter required field - Title"
		End If
		If ew_Empty(SiteLink.LinkTypeCD.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter required field - Link Type"
		End If
		If ew_Empty(SiteLink.CategoryID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter required field - Link Category"
		End If
		If Not ew_CheckInteger(SiteLink.Ranks.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Incorrect integer - Ranks"
		End If
		If ew_Empty(SiteLink.Views.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter required field - Active/Visible"
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
		SiteLink.SiteCategoryTypeID.SetDbValue(SiteLink.SiteCategoryTypeID.CurrentValue, System.DBNull.Value)
		Rs("SiteCategoryTypeID") = SiteLink.SiteCategoryTypeID.DbValue

		' Title
		SiteLink.Title.SetDbValue(SiteLink.Title.CurrentValue, System.DBNull.Value)
		Rs("Title") = SiteLink.Title.DbValue

		' LinkTypeCD
		SiteLink.LinkTypeCD.SetDbValue(SiteLink.LinkTypeCD.CurrentValue, System.DBNull.Value)
		Rs("LinkTypeCD") = SiteLink.LinkTypeCD.DbValue

		' CategoryID
		SiteLink.CategoryID.SetDbValue(SiteLink.CategoryID.CurrentValue, System.DBNull.Value)
		Rs("CategoryID") = SiteLink.CategoryID.DbValue

		' CompanyID
		SiteLink.CompanyID.SetDbValue(SiteLink.CompanyID.CurrentValue, System.DBNull.Value)
		Rs("CompanyID") = SiteLink.CompanyID.DbValue

		' SiteCategoryID
		SiteLink.SiteCategoryID.SetDbValue(SiteLink.SiteCategoryID.CurrentValue, System.DBNull.Value)
		Rs("SiteCategoryID") = SiteLink.SiteCategoryID.DbValue

		' SiteCategoryGroupID
		SiteLink.SiteCategoryGroupID.SetDbValue(SiteLink.SiteCategoryGroupID.CurrentValue, System.DBNull.Value)
		Rs("SiteCategoryGroupID") = SiteLink.SiteCategoryGroupID.DbValue

		' Description
		SiteLink.Description.SetDbValue(SiteLink.Description.CurrentValue, System.DBNull.Value)
		Rs("Description") = SiteLink.Description.DbValue

		' URL
		SiteLink.URL.SetDbValue(SiteLink.URL.CurrentValue, System.DBNull.Value)
		Rs("URL") = SiteLink.URL.DbValue

		' Ranks
		SiteLink.Ranks.SetDbValue(SiteLink.Ranks.CurrentValue, System.DBNull.Value)
		Rs("Ranks") = SiteLink.Ranks.DbValue

		' Views
		SiteLink.Views.SetDbValue((SiteLink.Views.CurrentValue <> "" And Not IsDBNull(SiteLink.Views.CurrentValue)), False)
		Rs("Views") = SiteLink.Views.DbValue

		' Row Inserting event
		bInsertRow = SiteLink.Row_Inserting(Rs)
		If bInsertRow Then
			Try	
				SiteLink.Insert(Rs)
				AddRow = True
			Catch e As Exception
				If EW_DEBUG_ENABLED Then Throw
				Message = e.Message				
				AddRow = False
			End Try
		Else
			If SiteLink.CancelMessage <> "" Then
				Message = SiteLink.CancelMessage
				SiteLink.CancelMessage = ""
			Else
				Message = "Insert cancelled"
			End If
			AddRow = False
		End If
		If AddRow Then
			LastInsertId = Conn.GetLastInsertId()
			SiteLink.ID.DbValue = LastInsertId
			Rs("ID") = SiteLink.ID.DbValue		

			' Row Inserted event
			SiteLink.Row_Inserted(Rs)
			WriteAuditTrailOnAdd(Rs)
		End If
	End Function

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "SiteLink"
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
		Dim table As String = "SiteLink"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsSrc("ID")

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

		' Title Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "Title", keyvalue, oldvalue, RsSrc("Title"))

		' LinkTypeCD Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "LinkTypeCD", keyvalue, oldvalue, RsSrc("LinkTypeCD"))

		' CategoryID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "CategoryID", keyvalue, oldvalue, RsSrc("CategoryID"))

		' CompanyID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "CompanyID", keyvalue, oldvalue, RsSrc("CompanyID"))

		' SiteCategoryID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteCategoryID", keyvalue, oldvalue, RsSrc("SiteCategoryID"))

		' SiteCategoryGroupID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteCategoryGroupID", keyvalue, oldvalue, RsSrc("SiteCategoryGroupID"))

		' Description Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "Description", keyvalue, oldvalue, "<MEMO>")

		' URL Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "URL", keyvalue, oldvalue, "<MEMO>")

		' Ranks Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "Ranks", keyvalue, oldvalue, RsSrc("Ranks"))

		' Views Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "Views", keyvalue, oldvalue, RsSrc("Views"))
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
		SiteLink_add = New cSiteLink_add(Me)		
		SiteLink_add.Page_Init()

		' Page main processing
		SiteLink_add.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If SiteLink_add IsNot Nothing Then SiteLink_add.Dispose()
	End Sub
End Class
