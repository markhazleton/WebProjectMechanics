Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class Link_edit
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public Link_edit As cLink_edit

	'
	' Page Class
	'
	Class cLink_edit
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
				If Link.UseTokenInUrl Then Url = Url & "t=" & Link.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If Link.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (Link.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (Link.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' Link
		Public Property Link() As cLink
			Get				
				Return ParentPage.Link
			End Get
			Set(ByVal v As cLink)
				ParentPage.Link = v	
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
			m_PageObjName = "Link_edit"
			m_PageObjTypeName = "cLink_edit"

			' Table Name
			m_TableName = "Link"

			' Initialize table object
			Link = New cLink(Me)

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
			Link.Dispose()

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
		If ew_Get("ID") <> "" Then
			Link.ID.QueryStringValue = ew_Get("ID")
		End If

		' Create form object
		ObjForm = New cFormObj
		If ObjForm.GetValue("a_edit") <> "" Then
			Link.CurrentAction = ObjForm.GetValue("a_edit") ' Get action code
			LoadFormValues() ' Get form values

			' Validate Form
			If Not ValidateForm() Then
				Link.CurrentAction = "" ' Form error, reset action
				Message = ParentPage.gsFormError
				LoadRow() ' Restore row
				RestoreFormValues() ' Restore form values if validate failed
			End If
		Else
			Link.CurrentAction = "I" ' Default action is display
		End If

		' Check if valid key
		If ew_Empty(Link.ID.CurrentValue) Then Page_Terminate("Link_list.aspx") ' Invalid key, return to list
		Select Case Link.CurrentAction
			Case "I" ' Get a record to display
				If Not LoadRow() Then ' Load Record based on key
					Message = "No records found" ' No record found
					Page_Terminate("Link_list.aspx") ' No matching record, return to list
				End If
			Case "U" ' Update
				Link.SendEmail = True ' Send email on update success
				If EditRow() Then ' Update Record based on key
					Message = "Update succeeded" ' Update success
					Dim sReturnUrl As String = Link.ReturnUrl
					If ew_GetPageName(sReturnUrl) = "Link_view.aspx" Then sReturnUrl = Link.ViewUrl ' View paging, return to view page with keyurl directly
					Page_Terminate(sReturnUrl) ' Return to caller
				Else
					LoadRow() ' Restore row
					RestoreFormValues() ' Restore form values if update failed
				End If
		End Select

		' Render the record
		Link.RowType = EW_ROWTYPE_EDIT ' Render as edit

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
		Link.ID.FormValue = ObjForm.GetValue("x_ID")
		Link.ID.OldValue = ObjForm.GetValue("o_ID")
		Link.Title.FormValue = ObjForm.GetValue("x_Title")
		Link.Title.OldValue = ObjForm.GetValue("o_Title")
		Link.LinkTypeCD.FormValue = ObjForm.GetValue("x_LinkTypeCD")
		Link.LinkTypeCD.OldValue = ObjForm.GetValue("o_LinkTypeCD")
		Link.CategoryID.FormValue = ObjForm.GetValue("x_CategoryID")
		Link.CategoryID.OldValue = ObjForm.GetValue("o_CategoryID")
		Link.CompanyID.FormValue = ObjForm.GetValue("x_CompanyID")
		Link.CompanyID.OldValue = ObjForm.GetValue("o_CompanyID")
		Link.SiteCategoryGroupID.FormValue = ObjForm.GetValue("x_SiteCategoryGroupID")
		Link.SiteCategoryGroupID.OldValue = ObjForm.GetValue("o_SiteCategoryGroupID")
		Link.zPageID.FormValue = ObjForm.GetValue("x_zPageID")
		Link.zPageID.OldValue = ObjForm.GetValue("o_zPageID")
		Link.Views.FormValue = ObjForm.GetValue("x_Views")
		Link.Views.OldValue = ObjForm.GetValue("o_Views")
		Link.Description.FormValue = ObjForm.GetValue("x_Description")
		Link.Description.OldValue = ObjForm.GetValue("o_Description")
		Link.URL.FormValue = ObjForm.GetValue("x_URL")
		Link.URL.OldValue = ObjForm.GetValue("o_URL")
		Link.Ranks.FormValue = ObjForm.GetValue("x_Ranks")
		Link.Ranks.OldValue = ObjForm.GetValue("o_Ranks")
		Link.UserID.FormValue = ObjForm.GetValue("x_UserID")
		Link.UserID.OldValue = ObjForm.GetValue("o_UserID")
		Link.ASIN.FormValue = ObjForm.GetValue("x_ASIN")
		Link.ASIN.OldValue = ObjForm.GetValue("o_ASIN")
		Link.UserName.FormValue = ObjForm.GetValue("x_UserName")
		Link.UserName.OldValue = ObjForm.GetValue("o_UserName")
		Link.DateAdd.FormValue = ObjForm.GetValue("x_DateAdd")
		Link.DateAdd.CurrentValue = ew_UnFormatDateTime(Link.DateAdd.CurrentValue, 6)
		Link.DateAdd.OldValue = ObjForm.GetValue("o_DateAdd")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		Link.ID.CurrentValue = Link.ID.FormValue
		Link.Title.CurrentValue = Link.Title.FormValue
		Link.LinkTypeCD.CurrentValue = Link.LinkTypeCD.FormValue
		Link.CategoryID.CurrentValue = Link.CategoryID.FormValue
		Link.CompanyID.CurrentValue = Link.CompanyID.FormValue
		Link.SiteCategoryGroupID.CurrentValue = Link.SiteCategoryGroupID.FormValue
		Link.zPageID.CurrentValue = Link.zPageID.FormValue
		Link.Views.CurrentValue = Link.Views.FormValue
		Link.Description.CurrentValue = Link.Description.FormValue
		Link.URL.CurrentValue = Link.URL.FormValue
		Link.Ranks.CurrentValue = Link.Ranks.FormValue
		Link.UserID.CurrentValue = Link.UserID.FormValue
		Link.ASIN.CurrentValue = Link.ASIN.FormValue
		Link.UserName.CurrentValue = Link.UserName.FormValue
		Link.DateAdd.CurrentValue = Link.DateAdd.FormValue
		Link.DateAdd.CurrentValue = ew_UnFormatDateTime(Link.DateAdd.CurrentValue, 6)
	End Sub

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = Link.KeyFilter

		' Row Selecting event
		Link.Row_Selecting(sFilter)

		' Load SQL based on filter
		Link.CurrentFilter = sFilter
		Dim sSql As String = Link.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				Link.Row_Selected(RsRow)
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
		Link.ID.DbValue = RsRow("ID")
		Link.Title.DbValue = RsRow("Title")
		Link.LinkTypeCD.DbValue = RsRow("LinkTypeCD")
		Link.CategoryID.DbValue = RsRow("CategoryID")
		Link.CompanyID.DbValue = RsRow("CompanyID")
		Link.SiteCategoryGroupID.DbValue = RsRow("SiteCategoryGroupID")
		Link.zPageID.DbValue = RsRow("PageID")
		Link.Views.DbValue = IIf(ew_ConvertToBool(RsRow("Views")), "1", "0")
		Link.Description.DbValue = RsRow("Description")
		Link.URL.DbValue = RsRow("URL")
		Link.Ranks.DbValue = RsRow("Ranks")
		Link.UserID.DbValue = RsRow("UserID")
		Link.ASIN.DbValue = RsRow("ASIN")
		Link.UserName.DbValue = RsRow("UserName")
		Link.DateAdd.DbValue = RsRow("DateAdd")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		Link.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' ID

		Link.ID.CellCssStyle = ""
		Link.ID.CellCssClass = ""

		' Title
		Link.Title.CellCssStyle = ""
		Link.Title.CellCssClass = ""

		' LinkTypeCD
		Link.LinkTypeCD.CellCssStyle = ""
		Link.LinkTypeCD.CellCssClass = ""

		' CategoryID
		Link.CategoryID.CellCssStyle = ""
		Link.CategoryID.CellCssClass = ""

		' CompanyID
		Link.CompanyID.CellCssStyle = ""
		Link.CompanyID.CellCssClass = ""

		' SiteCategoryGroupID
		Link.SiteCategoryGroupID.CellCssStyle = ""
		Link.SiteCategoryGroupID.CellCssClass = ""

		' PageID
		Link.zPageID.CellCssStyle = ""
		Link.zPageID.CellCssClass = ""

		' Views
		Link.Views.CellCssStyle = ""
		Link.Views.CellCssClass = ""

		' Description
		Link.Description.CellCssStyle = ""
		Link.Description.CellCssClass = ""

		' URL
		Link.URL.CellCssStyle = ""
		Link.URL.CellCssClass = ""

		' Ranks
		Link.Ranks.CellCssStyle = ""
		Link.Ranks.CellCssClass = ""

		' UserID
		Link.UserID.CellCssStyle = ""
		Link.UserID.CellCssClass = ""

		' ASIN
		Link.ASIN.CellCssStyle = ""
		Link.ASIN.CellCssClass = ""

		' UserName
		Link.UserName.CellCssStyle = ""
		Link.UserName.CellCssClass = ""

		' DateAdd
		Link.DateAdd.CellCssStyle = ""
		Link.DateAdd.CellCssClass = ""

		'
		'  View  Row
		'

		If Link.RowType = EW_ROWTYPE_VIEW Then ' View row

			' ID
			Link.ID.ViewValue = Link.ID.CurrentValue
			Link.ID.CssStyle = ""
			Link.ID.CssClass = ""
			Link.ID.ViewCustomAttributes = ""

			' Title
			Link.Title.ViewValue = Link.Title.CurrentValue
			Link.Title.CssStyle = ""
			Link.Title.CssClass = ""
			Link.Title.ViewCustomAttributes = ""

			' LinkTypeCD
			If ew_NotEmpty(Link.LinkTypeCD.CurrentValue) Then
				sSqlWrk = "SELECT [LinkTypeDesc] FROM [LinkType] WHERE [LinkTypeCD] = '" & ew_AdjustSql(Link.LinkTypeCD.CurrentValue) & "'"
				sSqlWrk = sSqlWrk & " ORDER BY [LinkTypeDesc] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Link.LinkTypeCD.ViewValue = RsWrk("LinkTypeDesc")
				Else
					Link.LinkTypeCD.ViewValue = Link.LinkTypeCD.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Link.LinkTypeCD.ViewValue = System.DBNull.Value
			End If
			Link.LinkTypeCD.CssStyle = ""
			Link.LinkTypeCD.CssClass = ""
			Link.LinkTypeCD.ViewCustomAttributes = ""

			' CategoryID
			If ew_NotEmpty(Link.CategoryID.CurrentValue) Then
				sSqlWrk = "SELECT [Title] FROM [LinkCategory] WHERE [ID] = " & ew_AdjustSql(Link.CategoryID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [Title] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Link.CategoryID.ViewValue = RsWrk("Title")
				Else
					Link.CategoryID.ViewValue = Link.CategoryID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Link.CategoryID.ViewValue = System.DBNull.Value
			End If
			Link.CategoryID.CssStyle = ""
			Link.CategoryID.CssClass = ""
			Link.CategoryID.ViewCustomAttributes = ""

			' CompanyID
			If ew_NotEmpty(Link.CompanyID.CurrentValue) Then
				sSqlWrk = "SELECT [CompanyName] FROM [Company] WHERE [CompanyID] = " & ew_AdjustSql(Link.CompanyID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Link.CompanyID.ViewValue = RsWrk("CompanyName")
				Else
					Link.CompanyID.ViewValue = Link.CompanyID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Link.CompanyID.ViewValue = System.DBNull.Value
			End If
			Link.CompanyID.CssStyle = ""
			Link.CompanyID.CssClass = ""
			Link.CompanyID.ViewCustomAttributes = ""

			' SiteCategoryGroupID
			If ew_NotEmpty(Link.SiteCategoryGroupID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteCategoryGroupNM] FROM [SiteCategoryGroup] WHERE [SiteCategoryGroupID] = " & ew_AdjustSql(Link.SiteCategoryGroupID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryGroupNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Link.SiteCategoryGroupID.ViewValue = RsWrk("SiteCategoryGroupNM")
				Else
					Link.SiteCategoryGroupID.ViewValue = Link.SiteCategoryGroupID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Link.SiteCategoryGroupID.ViewValue = System.DBNull.Value
			End If
			Link.SiteCategoryGroupID.CssStyle = ""
			Link.SiteCategoryGroupID.CssClass = ""
			Link.SiteCategoryGroupID.ViewCustomAttributes = ""

			' PageID
			If ew_NotEmpty(Link.zPageID.CurrentValue) Then
				sSqlWrk = "SELECT [PageName] FROM [Page] WHERE [PageID] = " & ew_AdjustSql(Link.zPageID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [PageName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Link.zPageID.ViewValue = RsWrk("PageName")
				Else
					Link.zPageID.ViewValue = Link.zPageID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Link.zPageID.ViewValue = System.DBNull.Value
			End If
			Link.zPageID.CssStyle = ""
			Link.zPageID.CssClass = ""
			Link.zPageID.ViewCustomAttributes = ""

			' Views
			If Convert.ToString(Link.Views.CurrentValue) = "1" Then
				Link.Views.ViewValue = "Yes"
			Else
				Link.Views.ViewValue = "No"
			End If
			Link.Views.CssStyle = ""
			Link.Views.CssClass = ""
			Link.Views.ViewCustomAttributes = ""

			' Description
			Link.Description.ViewValue = Link.Description.CurrentValue
			Link.Description.CssStyle = ""
			Link.Description.CssClass = ""
			Link.Description.ViewCustomAttributes = ""

			' URL
			Link.URL.ViewValue = Link.URL.CurrentValue
			Link.URL.CssStyle = ""
			Link.URL.CssClass = ""
			Link.URL.ViewCustomAttributes = ""

			' Ranks
			Link.Ranks.ViewValue = Link.Ranks.CurrentValue
			Link.Ranks.CssStyle = ""
			Link.Ranks.CssClass = ""
			Link.Ranks.ViewCustomAttributes = ""

			' UserID
			If ew_NotEmpty(Link.UserID.CurrentValue) Then
				sSqlWrk = "SELECT [PrimaryContact] FROM [Contact] WHERE [ContactID] = " & ew_AdjustSql(Link.UserID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [PrimaryContact] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Link.UserID.ViewValue = RsWrk("PrimaryContact")
				Else
					Link.UserID.ViewValue = Link.UserID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Link.UserID.ViewValue = System.DBNull.Value
			End If
			Link.UserID.CssStyle = ""
			Link.UserID.CssClass = ""
			Link.UserID.ViewCustomAttributes = ""

			' ASIN
			Link.ASIN.ViewValue = Link.ASIN.CurrentValue
			Link.ASIN.CssStyle = ""
			Link.ASIN.CssClass = ""
			Link.ASIN.ViewCustomAttributes = ""

			' UserName
			Link.UserName.ViewValue = Link.UserName.CurrentValue
			Link.UserName.CssStyle = ""
			Link.UserName.CssClass = ""
			Link.UserName.ViewCustomAttributes = ""

			' DateAdd
			Link.DateAdd.ViewValue = Link.DateAdd.CurrentValue
			Link.DateAdd.ViewValue = ew_FormatDateTime(Link.DateAdd.ViewValue, 6)
			Link.DateAdd.CssStyle = ""
			Link.DateAdd.CssClass = ""
			Link.DateAdd.ViewCustomAttributes = ""

			' View refer script
			' ID

			Link.ID.HrefValue = ""

			' Title
			Link.Title.HrefValue = ""

			' LinkTypeCD
			Link.LinkTypeCD.HrefValue = ""

			' CategoryID
			Link.CategoryID.HrefValue = ""

			' CompanyID
			Link.CompanyID.HrefValue = ""

			' SiteCategoryGroupID
			Link.SiteCategoryGroupID.HrefValue = ""

			' PageID
			Link.zPageID.HrefValue = ""

			' Views
			Link.Views.HrefValue = ""

			' Description
			Link.Description.HrefValue = ""

			' URL
			Link.URL.HrefValue = ""

			' Ranks
			Link.Ranks.HrefValue = ""

			' UserID
			Link.UserID.HrefValue = ""

			' ASIN
			Link.ASIN.HrefValue = ""

			' UserName
			Link.UserName.HrefValue = ""

			' DateAdd
			Link.DateAdd.HrefValue = ""

		'
		'  Edit Row
		'

		ElseIf Link.RowType = EW_ROWTYPE_EDIT Then ' Edit row

			' ID
			Link.ID.EditCustomAttributes = ""

			' Title
			Link.Title.EditCustomAttributes = ""
			Link.Title.EditValue = ew_HtmlEncode(Link.Title.CurrentValue)

			' LinkTypeCD
			Link.LinkTypeCD.EditCustomAttributes = ""
			sSqlWrk = "SELECT [LinkTypeCD], [LinkTypeDesc], '' AS Disp2Fld, '' AS SelectFilterFld FROM [LinkType]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [LinkTypeDesc] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			Link.LinkTypeCD.EditValue = arwrk

			' CategoryID
			Link.CategoryID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [ID], [Title], '' AS Disp2Fld, '' AS SelectFilterFld FROM [LinkCategory]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [Title] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			Link.CategoryID.EditValue = arwrk

			' CompanyID
			Link.CompanyID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [CompanyID], [CompanyName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Company]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			Link.CompanyID.EditValue = arwrk

			' SiteCategoryGroupID
			Link.SiteCategoryGroupID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryGroupID], [SiteCategoryGroupNM], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteCategoryGroup]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryGroupNM] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			Link.SiteCategoryGroupID.EditValue = arwrk

			' PageID
			Link.zPageID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [PageID], [PageName], '' AS Disp2Fld, [CompanyID] FROM [Page]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [PageName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select", ""}) 
			Link.zPageID.EditValue = arwrk

			' Views
			Link.Views.EditCustomAttributes = ""

			' Description
			Link.Description.EditCustomAttributes = ""
			Link.Description.EditValue = ew_HtmlEncode(Link.Description.CurrentValue)

			' URL
			Link.URL.EditCustomAttributes = ""
			Link.URL.EditValue = ew_HtmlEncode(Link.URL.CurrentValue)

			' Ranks
			Link.Ranks.EditCustomAttributes = ""
			Link.Ranks.EditValue = ew_HtmlEncode(Link.Ranks.CurrentValue)

			' UserID
			Link.UserID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [ContactID], [PrimaryContact], '' AS Disp2Fld, [CompanyID] FROM [Contact]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [PrimaryContact] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select", ""}) 
			Link.UserID.EditValue = arwrk

			' ASIN
			Link.ASIN.EditCustomAttributes = ""
			Link.ASIN.EditValue = ew_HtmlEncode(Link.ASIN.CurrentValue)

			' UserName
			Link.UserName.EditCustomAttributes = ""
			Link.UserName.EditValue = ew_HtmlEncode(Link.UserName.CurrentValue)

			' DateAdd
			Link.DateAdd.EditCustomAttributes = ""
			Link.DateAdd.EditValue = ew_FormatDateTime(Link.DateAdd.CurrentValue, 6)

			' Edit refer script
			' ID

			Link.ID.HrefValue = ""

			' Title
			Link.Title.HrefValue = ""

			' LinkTypeCD
			Link.LinkTypeCD.HrefValue = ""

			' CategoryID
			Link.CategoryID.HrefValue = ""

			' CompanyID
			Link.CompanyID.HrefValue = ""

			' SiteCategoryGroupID
			Link.SiteCategoryGroupID.HrefValue = ""

			' PageID
			Link.zPageID.HrefValue = ""

			' Views
			Link.Views.HrefValue = ""

			' Description
			Link.Description.HrefValue = ""

			' URL
			Link.URL.HrefValue = ""

			' Ranks
			Link.Ranks.HrefValue = ""

			' UserID
			Link.UserID.HrefValue = ""

			' ASIN
			Link.ASIN.HrefValue = ""

			' UserName
			Link.UserName.HrefValue = ""

			' DateAdd
			Link.DateAdd.HrefValue = ""
		End If

		' Row Rendered event
		Link.Row_Rendered()
	End Sub

	'
	' Validate form
	'
	Function ValidateForm() As Boolean

		' Initialize
		ParentPage.gsFormError = ""

		' Check if validation required
		If Not EW_SERVER_VALIDATE Then Return (ParentPage.gsFormError = "")
		If ew_Empty(Link.Title.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter required field - Title"
		End If
		If ew_Empty(Link.LinkTypeCD.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter required field - Part Type"
		End If
		If ew_Empty(Link.CategoryID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter required field - Part Category"
		End If
		If ew_Empty(Link.CompanyID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter required field - Company"
		End If
		If Not ew_CheckInteger(Link.Ranks.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Incorrect integer - Ranks"
		End If
		If ew_Empty(Link.UserID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter required field - User"
		End If
		If Not ew_CheckUSDate(Link.DateAdd.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Incorrect date, format = mm/dd/yyyy - Date Add"
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
		sFilter = Link.KeyFilter
		Link.CurrentFilter  = sFilter
		sSql = Link.SQL
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

			' ID
			' Title

			Link.Title.SetDbValue(Link.Title.CurrentValue, System.DBNull.Value)
			Rs("Title") = Link.Title.DbValue

			' LinkTypeCD
			Link.LinkTypeCD.SetDbValue(Link.LinkTypeCD.CurrentValue, System.DBNull.Value)
			Rs("LinkTypeCD") = Link.LinkTypeCD.DbValue

			' CategoryID
			Link.CategoryID.SetDbValue(Link.CategoryID.CurrentValue, System.DBNull.Value)
			Rs("CategoryID") = Link.CategoryID.DbValue

			' CompanyID
			Link.CompanyID.SetDbValue(Link.CompanyID.CurrentValue, 0)
			Rs("CompanyID") = Link.CompanyID.DbValue

			' SiteCategoryGroupID
			Link.SiteCategoryGroupID.SetDbValue(Link.SiteCategoryGroupID.CurrentValue, System.DBNull.Value)
			Rs("SiteCategoryGroupID") = Link.SiteCategoryGroupID.DbValue

			' PageID
			Link.zPageID.SetDbValue(Link.zPageID.CurrentValue, System.DBNull.Value)
			Rs("PageID") = Link.zPageID.DbValue

			' Views
			Link.Views.SetDbValue((Link.Views.CurrentValue <> "" And Not IsDBNull(Link.Views.CurrentValue)), False)
			Rs("Views") = Link.Views.DbValue

			' Description
			Link.Description.SetDbValue(Link.Description.CurrentValue, System.DBNull.Value)
			Rs("Description") = Link.Description.DbValue

			' URL
			Link.URL.SetDbValue(Link.URL.CurrentValue, System.DBNull.Value)
			Rs("URL") = Link.URL.DbValue

			' Ranks
			Link.Ranks.SetDbValue(Link.Ranks.CurrentValue, System.DBNull.Value)
			Rs("Ranks") = Link.Ranks.DbValue

			' UserID
			Link.UserID.SetDbValue(Link.UserID.CurrentValue, System.DBNull.Value)
			Rs("UserID") = Link.UserID.DbValue

			' ASIN
			Link.ASIN.SetDbValue(Link.ASIN.CurrentValue, System.DBNull.Value)
			Rs("ASIN") = Link.ASIN.DbValue

			' UserName
			Link.UserName.SetDbValue(Link.UserName.CurrentValue, System.DBNull.Value)
			Rs("UserName") = Link.UserName.DbValue

			' DateAdd
			Link.DateAdd.SetDbValue(ew_UnFormatDateTime(Link.DateAdd.CurrentValue, 6), System.DBNull.Value)
			Rs("DateAdd") = Link.DateAdd.DbValue

			' Row Updating event
			bUpdateRow = Link.Row_Updating(RsOld, Rs)
			If bUpdateRow Then
				Try
					Link.Update(Rs)
					EditRow = True
				Catch e As Exception
					If EW_DEBUG_ENABLED Then Throw
					Message = e.Message
					EditRow = False
				End Try
			Else
				If Link.CancelMessage <> "" Then
					Message = Link.CancelMessage
					Link.CancelMessage = ""
				Else
					Message = "Update cancelled"
				End If
				EditRow = False
			End If
		End If

		' Row Updated event
		If EditRow Then
			Link.Row_Updated(RsOld, Rs)
		End If
		If EditRow Then
			WriteAuditTrailOnEdit(RsOld, Rs)
		End If
	End Function

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "Link"
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
		Dim table As String = "Link"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsOld("ID")

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
			fld = Link.FieldByName(fldname)
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
		Link_edit = New cLink_edit(Me)		
		Link_edit.Page_Init()

		' Page main processing
		Link_edit.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If Link_edit IsNot Nothing Then Link_edit.Dispose()
	End Sub
End Class
