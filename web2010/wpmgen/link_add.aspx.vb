Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class link_add
	Inherits AspNetMaker8_wpmWebsite

	' Page object
	Public Link_add As cLink_add

	'
	' Page Class
	'
	Class cLink_add
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
				If Link.UseTokenInUrl Then Url = Url & "t=" & Link.TableVar & "&" ' Add page token
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

		' ASP.NET page object
		Public ReadOnly Property AspNetPage() As link_add
			Get
				Return CType(m_ParentPage, link_add)
			End Get
		End Property

		' Link
		Public Property Link() As cLink
			Get				
				Return ParentPage.Link
			End Get
			Set(ByVal v As cLink)
				ParentPage.Link = v	
			End Set	
		End Property

		' Link
		Public Property Company() As cCompany
			Get				
				Return ParentPage.Company
			End Get
			Set(ByVal v As cCompany)
				ParentPage.Company = v	
			End Set	
		End Property

		' Link
		Public Property LinkCategory() As cLinkCategory
			Get				
				Return ParentPage.LinkCategory
			End Get
			Set(ByVal v As cLinkCategory)
				ParentPage.LinkCategory = v	
			End Set	
		End Property

		' Link
		Public Property LinkType() As cLinkType
			Get				
				Return ParentPage.LinkType
			End Get
			Set(ByVal v As cLinkType)
				ParentPage.LinkType = v	
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
			m_PageObjName = "Link_add"
			m_PageObjTypeName = "cLink_add"			

			' Initialize language object
			Language = New cLanguage(Me)

			' Table Name
			m_TableName = "Link"

			' Initialize table object
			Link = New cLink(Me)
			Company = New cCompany(Me)
			LinkCategory = New cLinkCategory(Me)
			LinkType = New cLinkType(Me)

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
			Link.Dispose()
			Company.Dispose()
			LinkCategory.Dispose()
			LinkType.Dispose()
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
		If ew_Get("ID") <> "" Then
			Link.ID.QueryStringValue = ew_Get("ID")
		Else
			bCopy = False
		End If

		' Set up master detail parameters
		SetUpMasterDetail()

		' Process form if post back
		If ObjForm.GetValue("a_add") <> "" Then
			Link.CurrentAction = ObjForm.GetValue("a_add") ' Get form action
			LoadFormValues() ' Load form values

			' Validate Form
			If Not ValidateForm() Then
				Link.CurrentAction = "I" ' Form error, reset action
				Message = ParentPage.gsFormError
			End If

		' Not post back
		Else
			If bCopy Then
				Link.CurrentAction = "C" ' Copy Record
			Else
				Link.CurrentAction = "I" ' Display Blank Record
				LoadDefaultValues() ' Load default values
			End If
		End If

		' Perform action based on action code
		Select Case Link.CurrentAction
			Case "I" ' Blank record, no action required
			Case "C" ' Copy an existing record
				If Not LoadRow() Then ' Load record based on key
					Message = Language.Phrase("NoRecord") ' No record found
					Page_Terminate("link_list.aspx") ' No matching record, return to list
				End If
			Case "A" ' Add new record
				Link.SendEmail = True ' Send email on add success
				If AddRow() Then ' Add successful
					Message = Language.Phrase("AddSuccess") ' Set up success message
					Dim sReturnUrl As String = Link.ReturnUrl
					Page_Terminate(sReturnUrl) ' Clean up and return
				Else
					RestoreFormValues() ' Add failed, restore form values
				End If
		End Select

		' Render row based on row type
		Link.RowType = EW_ROWTYPE_ADD ' Render add type

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
		Link.CategoryID.CurrentValue = 0
		Link.CompanyID.CurrentValue = 0
		Link.zPageID.CurrentValue = 0
		Link.Views.CurrentValue = 0
		Link.Ranks.CurrentValue = 0
		Link.UserID.CurrentValue = 0
	End Sub

	'
	' Load form values
	'
	Sub LoadFormValues()
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
		Link.DateAdd.FormValue = ObjForm.GetValue("x_DateAdd")
		Link.DateAdd.CurrentValue = ew_UnFormatDateTime(Link.DateAdd.CurrentValue, 8)
		Link.DateAdd.OldValue = ObjForm.GetValue("o_DateAdd")
		Link.ID.FormValue = ObjForm.GetValue("x_ID")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
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
		Link.DateAdd.CurrentValue = Link.DateAdd.FormValue
		Link.DateAdd.CurrentValue = ew_UnFormatDateTime(Link.DateAdd.CurrentValue, 8)
		Link.ID.CurrentValue = Link.ID.FormValue
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
		If EW_DEBUG_ENABLED Then DebugMsg = sSql
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
		Dim sDetailFilter As String
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
		Link.DateAdd.DbValue = RsRow("DateAdd")
		Link.UserName.DbValue = RsRow("UserName")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Initialize urls
		' Row Rendering event

		Link.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' Title

		Link.Title.CellCssStyle = ""
		Link.Title.CellCssClass = ""
		Link.Title.CellAttrs.Clear(): Link.Title.ViewAttrs.Clear(): Link.Title.EditAttrs.Clear()

		' LinkTypeCD
		Link.LinkTypeCD.CellCssStyle = ""
		Link.LinkTypeCD.CellCssClass = ""
		Link.LinkTypeCD.CellAttrs.Clear(): Link.LinkTypeCD.ViewAttrs.Clear(): Link.LinkTypeCD.EditAttrs.Clear()

		' CategoryID
		Link.CategoryID.CellCssStyle = ""
		Link.CategoryID.CellCssClass = ""
		Link.CategoryID.CellAttrs.Clear(): Link.CategoryID.ViewAttrs.Clear(): Link.CategoryID.EditAttrs.Clear()

		' CompanyID
		Link.CompanyID.CellCssStyle = ""
		Link.CompanyID.CellCssClass = ""
		Link.CompanyID.CellAttrs.Clear(): Link.CompanyID.ViewAttrs.Clear(): Link.CompanyID.EditAttrs.Clear()

		' SiteCategoryGroupID
		Link.SiteCategoryGroupID.CellCssStyle = ""
		Link.SiteCategoryGroupID.CellCssClass = ""
		Link.SiteCategoryGroupID.CellAttrs.Clear(): Link.SiteCategoryGroupID.ViewAttrs.Clear(): Link.SiteCategoryGroupID.EditAttrs.Clear()

		' PageID
		Link.zPageID.CellCssStyle = ""
		Link.zPageID.CellCssClass = ""
		Link.zPageID.CellAttrs.Clear(): Link.zPageID.ViewAttrs.Clear(): Link.zPageID.EditAttrs.Clear()

		' Views
		Link.Views.CellCssStyle = ""
		Link.Views.CellCssClass = ""
		Link.Views.CellAttrs.Clear(): Link.Views.ViewAttrs.Clear(): Link.Views.EditAttrs.Clear()

		' Description
		Link.Description.CellCssStyle = ""
		Link.Description.CellCssClass = ""
		Link.Description.CellAttrs.Clear(): Link.Description.ViewAttrs.Clear(): Link.Description.EditAttrs.Clear()

		' URL
		Link.URL.CellCssStyle = ""
		Link.URL.CellCssClass = ""
		Link.URL.CellAttrs.Clear(): Link.URL.ViewAttrs.Clear(): Link.URL.EditAttrs.Clear()

		' Ranks
		Link.Ranks.CellCssStyle = ""
		Link.Ranks.CellCssClass = ""
		Link.Ranks.CellAttrs.Clear(): Link.Ranks.ViewAttrs.Clear(): Link.Ranks.EditAttrs.Clear()

		' UserID
		Link.UserID.CellCssStyle = ""
		Link.UserID.CellCssClass = ""
		Link.UserID.CellAttrs.Clear(): Link.UserID.ViewAttrs.Clear(): Link.UserID.EditAttrs.Clear()

		' ASIN
		Link.ASIN.CellCssStyle = ""
		Link.ASIN.CellCssClass = ""
		Link.ASIN.CellAttrs.Clear(): Link.ASIN.ViewAttrs.Clear(): Link.ASIN.EditAttrs.Clear()

		' DateAdd
		Link.DateAdd.CellCssStyle = ""
		Link.DateAdd.CellCssClass = ""
		Link.DateAdd.CellAttrs.Clear(): Link.DateAdd.ViewAttrs.Clear(): Link.DateAdd.EditAttrs.Clear()

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
				sFilterWrk = "[LinkTypeCD] = '" & ew_AdjustSql(Link.LinkTypeCD.CurrentValue) & "'"
			sSqlWrk = "SELECT [LinkTypeDesc] FROM [LinkType]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [LinkTypeDesc] Asc"
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
				sFilterWrk = "[ID] = " & ew_AdjustSql(Link.CategoryID.CurrentValue) & ""
			sSqlWrk = "SELECT [Title] FROM [LinkCategory]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [Title] Asc"
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
				sFilterWrk = "[CompanyID] = " & ew_AdjustSql(Link.CompanyID.CurrentValue) & ""
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
				sFilterWrk = "[SiteCategoryGroupID] = " & ew_AdjustSql(Link.SiteCategoryGroupID.CurrentValue) & ""
			sSqlWrk = "SELECT [SiteCategoryGroupNM] FROM [SiteCategoryGroup]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [SiteCategoryGroupNM] Asc"
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
				sFilterWrk = "[PageID] = " & ew_AdjustSql(Link.zPageID.CurrentValue) & ""
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
				sFilterWrk = "[ContactID] = " & ew_AdjustSql(Link.UserID.CurrentValue) & ""
			sSqlWrk = "SELECT [PrimaryContact] FROM [Contact]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [PrimaryContact] Asc"
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

			' DateAdd
			Link.DateAdd.ViewValue = Link.DateAdd.CurrentValue
			Link.DateAdd.CssStyle = ""
			Link.DateAdd.CssClass = ""
			Link.DateAdd.ViewCustomAttributes = ""

			' UserName
			Link.UserName.ViewValue = Link.UserName.CurrentValue
			Link.UserName.CssStyle = ""
			Link.UserName.CssClass = ""
			Link.UserName.ViewCustomAttributes = ""

			' View refer script
			' Title

			Link.Title.HrefValue = ""
			Link.Title.TooltipValue = ""

			' LinkTypeCD
			Link.LinkTypeCD.HrefValue = ""
			Link.LinkTypeCD.TooltipValue = ""

			' CategoryID
			Link.CategoryID.HrefValue = ""
			Link.CategoryID.TooltipValue = ""

			' CompanyID
			Link.CompanyID.HrefValue = ""
			Link.CompanyID.TooltipValue = ""

			' SiteCategoryGroupID
			Link.SiteCategoryGroupID.HrefValue = ""
			Link.SiteCategoryGroupID.TooltipValue = ""

			' PageID
			Link.zPageID.HrefValue = ""
			Link.zPageID.TooltipValue = ""

			' Views
			Link.Views.HrefValue = ""
			Link.Views.TooltipValue = ""

			' Description
			Link.Description.HrefValue = ""
			Link.Description.TooltipValue = ""

			' URL
			Link.URL.HrefValue = ""
			Link.URL.TooltipValue = ""

			' Ranks
			Link.Ranks.HrefValue = ""
			Link.Ranks.TooltipValue = ""

			' UserID
			Link.UserID.HrefValue = ""
			Link.UserID.TooltipValue = ""

			' ASIN
			Link.ASIN.HrefValue = ""
			Link.ASIN.TooltipValue = ""

			' DateAdd
			Link.DateAdd.HrefValue = ""
			Link.DateAdd.TooltipValue = ""

		'
		'  Add Row
		'

		ElseIf Link.RowType = EW_ROWTYPE_ADD Then ' Add row

			' Title
			Link.Title.EditCustomAttributes = ""
			Link.Title.EditValue = ew_HtmlEncode(Link.Title.CurrentValue)

			' LinkTypeCD
			Link.LinkTypeCD.EditCustomAttributes = ""
			If Link.LinkTypeCD.SessionValue <> "" Then
				Link.LinkTypeCD.CurrentValue = Link.LinkTypeCD.SessionValue
			If ew_NotEmpty(Link.LinkTypeCD.CurrentValue) Then
				sFilterWrk = "[LinkTypeCD] = '" & ew_AdjustSql(Link.LinkTypeCD.CurrentValue) & "'"
			sSqlWrk = "SELECT [LinkTypeDesc] FROM [LinkType]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [LinkTypeDesc] Asc"
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
			Else
			sFilterWrk = ""
			sSqlWrk = "SELECT [LinkTypeCD], [LinkTypeDesc], '' AS Disp2Fld, '' AS SelectFilterFld FROM [LinkType]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [LinkTypeDesc] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", Language.Phrase("PleaseSelect")}) 
			Link.LinkTypeCD.EditValue = arwrk
			End If

			' CategoryID
			Link.CategoryID.EditCustomAttributes = ""
			If Link.CategoryID.SessionValue <> "" Then
				Link.CategoryID.CurrentValue = Link.CategoryID.SessionValue
			If ew_NotEmpty(Link.CategoryID.CurrentValue) Then
				sFilterWrk = "[ID] = " & ew_AdjustSql(Link.CategoryID.CurrentValue) & ""
			sSqlWrk = "SELECT [Title] FROM [LinkCategory]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [Title] Asc"
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
			Else
			sFilterWrk = ""
			sSqlWrk = "SELECT [ID], [Title], '' AS Disp2Fld, '' AS SelectFilterFld FROM [LinkCategory]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [Title] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", Language.Phrase("PleaseSelect")}) 
			Link.CategoryID.EditValue = arwrk
			End If

			' CompanyID
			Link.CompanyID.EditCustomAttributes = ""
			If Link.CompanyID.SessionValue <> "" Then
				Link.CompanyID.CurrentValue = Link.CompanyID.SessionValue
			If ew_NotEmpty(Link.CompanyID.CurrentValue) Then
				sFilterWrk = "[CompanyID] = " & ew_AdjustSql(Link.CompanyID.CurrentValue) & ""
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
			Link.CompanyID.EditValue = arwrk
			End If

			' SiteCategoryGroupID
			Link.SiteCategoryGroupID.EditCustomAttributes = ""
			sFilterWrk = ""
			sSqlWrk = "SELECT [SiteCategoryGroupID], [SiteCategoryGroupNM], '' AS Disp2Fld, '' AS SelectFilterFld FROM [SiteCategoryGroup]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [SiteCategoryGroupNM] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", Language.Phrase("PleaseSelect")}) 
			Link.SiteCategoryGroupID.EditValue = arwrk

			' PageID
			Link.zPageID.EditCustomAttributes = ""
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
			sFilterWrk = ""
			sSqlWrk = "SELECT [ContactID], [PrimaryContact], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Contact]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [PrimaryContact] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", Language.Phrase("PleaseSelect")}) 
			Link.UserID.EditValue = arwrk

			' ASIN
			Link.ASIN.EditCustomAttributes = ""
			Link.ASIN.EditValue = ew_HtmlEncode(Link.ASIN.CurrentValue)

			' DateAdd
			Link.DateAdd.EditCustomAttributes = ""
			Link.DateAdd.EditValue = Link.DateAdd.CurrentValue
		End If

		' Row Rendered event
		If Link.RowType <> EW_ROWTYPE_AGGREGATEINIT Then
			Link.Row_Rendered()
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
		If Not ew_CheckInteger(Link.Ranks.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError &= Link.Ranks.FldErrMsg
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

			' Title
			Link.Title.SetDbValue(Rs, Link.Title.CurrentValue, System.DBNull.Value, False)

			' LinkTypeCD
			Link.LinkTypeCD.SetDbValue(Rs, Link.LinkTypeCD.CurrentValue, System.DBNull.Value, False)

			' CategoryID
			Link.CategoryID.SetDbValue(Rs, Link.CategoryID.CurrentValue, System.DBNull.Value, True)

			' CompanyID
			Link.CompanyID.SetDbValue(Rs, Link.CompanyID.CurrentValue, 0, True)

			' SiteCategoryGroupID
			Link.SiteCategoryGroupID.SetDbValue(Rs, Link.SiteCategoryGroupID.CurrentValue, System.DBNull.Value, False)

			' PageID
			Link.zPageID.SetDbValue(Rs, Link.zPageID.CurrentValue, System.DBNull.Value, True)

			' Views
			Link.Views.SetDbValue(Rs, (Link.Views.CurrentValue <> "" AndAlso Not IsDBNull(Link.Views.CurrentValue)), False, True)

			' Description
			Link.Description.SetDbValue(Rs, Link.Description.CurrentValue, System.DBNull.Value, False)

			' URL
			Link.URL.SetDbValue(Rs, Link.URL.CurrentValue, System.DBNull.Value, False)

			' Ranks
			Link.Ranks.SetDbValue(Rs, Link.Ranks.CurrentValue, System.DBNull.Value, True)

			' UserID
			Link.UserID.SetDbValue(Rs, Link.UserID.CurrentValue, System.DBNull.Value, True)

			' ASIN
			Link.ASIN.SetDbValue(Rs, Link.ASIN.CurrentValue, System.DBNull.Value, False)

			' DateAdd
			Link.DateAdd.SetDbValue(Rs, Link.DateAdd.CurrentValue, System.DBNull.Value, False)
		Catch e As Exception
			If EW_DEBUG_ENABLED Then Throw
			Message = e.Message
			Return False
		End Try

		' Row Inserting event
		bInsertRow = Link.Row_Inserting(Rs)
		If bInsertRow Then
			Try	
				Link.Insert(Rs)
				AddRow = True
			Catch e As Exception
				If EW_DEBUG_ENABLED Then Throw
				Message = e.Message				
				AddRow = False
			End Try
		Else
			If Link.CancelMessage <> "" Then
				Message = Link.CancelMessage
				Link.CancelMessage = ""
			Else
				Message = Language.Phrase("InsertCancelled")
			End If
			AddRow = False
		End If
		If AddRow Then
			LastInsertId = Conn.GetLastInsertId()
			Link.ID.DbValue = LastInsertId
			Rs("ID") = Link.ID.DbValue		

			' Row Inserted event
			Link.Row_Inserted(Rs)
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
				sDbMasterFilter = Link.SqlMasterFilter_Company
				sDbDetailFilter = Link.SqlDetailFilter_Company
				If ew_Get("CompanyID") <> "" Then
					Company.CompanyID.QueryStringValue = ew_Get("CompanyID")
					Link.CompanyID.QueryStringValue = Company.CompanyID.QueryStringValue
					Link.CompanyID.SessionValue = Link.CompanyID.QueryStringValue
					If Not IsNumeric(Company.CompanyID.QueryStringValue) Then bValidMaster = False
					sDbMasterFilter = sDbMasterFilter.Replace("@CompanyID@", ew_AdjustSql(Company.CompanyID.QueryStringValue))
					sDbDetailFilter = sDbDetailFilter.Replace("@CompanyID@", ew_AdjustSql(Company.CompanyID.QueryStringValue))
				Else
					bValidMaster = False
				End If
			End If
			If sMasterTblVar = "LinkType" Then
				bValidMaster = True
				sDbMasterFilter = Link.SqlMasterFilter_LinkType
				sDbDetailFilter = Link.SqlDetailFilter_LinkType
				If ew_Get("LinkTypeCD") <> "" Then
					LinkType.LinkTypeCD.QueryStringValue = ew_Get("LinkTypeCD")
					Link.LinkTypeCD.QueryStringValue = LinkType.LinkTypeCD.QueryStringValue
					Link.LinkTypeCD.SessionValue = Link.LinkTypeCD.QueryStringValue
					sDbMasterFilter = sDbMasterFilter.Replace("@LinkTypeCD@", ew_AdjustSql(LinkType.LinkTypeCD.QueryStringValue))
					sDbDetailFilter = sDbDetailFilter.Replace("@LinkTypeCD@", ew_AdjustSql(LinkType.LinkTypeCD.QueryStringValue))
				Else
					bValidMaster = False
				End If
			End If
			If sMasterTblVar = "LinkCategory" Then
				bValidMaster = True
				sDbMasterFilter = Link.SqlMasterFilter_LinkCategory
				sDbDetailFilter = Link.SqlDetailFilter_LinkCategory
				If ew_Get("ID") <> "" Then
					LinkCategory.ID.QueryStringValue = ew_Get("ID")
					Link.CategoryID.QueryStringValue = LinkCategory.ID.QueryStringValue
					Link.CategoryID.SessionValue = Link.CategoryID.QueryStringValue
					If Not IsNumeric(LinkCategory.ID.QueryStringValue) Then bValidMaster = False
					sDbMasterFilter = sDbMasterFilter.Replace("@ID@", ew_AdjustSql(LinkCategory.ID.QueryStringValue))
					sDbDetailFilter = sDbDetailFilter.Replace("@CategoryID@", ew_AdjustSql(LinkCategory.ID.QueryStringValue))
				Else
					bValidMaster = False
				End If
			End If
		End If
		If bValidMaster Then

			' Save current master table
			Link.CurrentMasterTable = sMasterTblVar
			Link.MasterFilter = sDbMasterFilter ' Set up master filter
			Link.DetailFilter = sDbDetailFilter ' Set up detail filter

			' Clear previous master session values
			If sMasterTblVar <> "Company" Then
				If Link.CompanyID.QueryStringValue = "" Then Link.CompanyID.SessionValue = ""
			End If
			If sMasterTblVar <> "LinkType" Then
				If Link.LinkTypeCD.QueryStringValue = "" Then Link.LinkTypeCD.SessionValue = ""
			End If
			If sMasterTblVar <> "LinkCategory" Then
				If Link.CategoryID.QueryStringValue = "" Then Link.CategoryID.SessionValue = ""
			End If
		End If
	End Sub

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "Link"
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
		Dim table As String = "Link"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsSrc("ID")

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

		' Title Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "Title", keyvalue, oldvalue, RsSrc("Title"))

		' LinkTypeCD Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "LinkTypeCD", keyvalue, oldvalue, RsSrc("LinkTypeCD"))

		' CategoryID Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "CategoryID", keyvalue, oldvalue, RsSrc("CategoryID"))

		' CompanyID Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "CompanyID", keyvalue, oldvalue, RsSrc("CompanyID"))

		' SiteCategoryGroupID Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "SiteCategoryGroupID", keyvalue, oldvalue, RsSrc("SiteCategoryGroupID"))

		' PageID Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "PageID", keyvalue, oldvalue, RsSrc("PageID"))

		' Views Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "Views", keyvalue, oldvalue, RsSrc("Views"))

		' Description Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "Description", keyvalue, oldvalue, "<MEMO>")

		' URL Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "URL", keyvalue, oldvalue, "<MEMO>")

		' Ranks Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "Ranks", keyvalue, oldvalue, RsSrc("Ranks"))

		' UserID Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "UserID", keyvalue, oldvalue, RsSrc("UserID"))

		' ASIN Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "ASIN", keyvalue, oldvalue, RsSrc("ASIN"))

		' DateAdd Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "DateAdd", keyvalue, oldvalue, RsSrc("DateAdd"))
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
		Link_add = New cLink_add(Me)		
		Link_add.Page_Init()		

		' Set buffer/cache option
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

		' Page main processing
		Link_add.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If Link_add IsNot Nothing Then Link_add.Dispose()
	End Sub
End Class
