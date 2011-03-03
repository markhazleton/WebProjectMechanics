Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class sitelink_edit
	Inherits AspNetMaker8_wpmWebsite

	' Page object
	Public SiteLink_edit As cSiteLink_edit

	'
	' Page Class
	'
	Class cSiteLink_edit
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
				If SiteLink.UseTokenInUrl Then Url = Url & "t=" & SiteLink.TableVar & "&" ' Add page token
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

		' ASP.NET page object
		Public ReadOnly Property AspNetPage() As sitelink_edit
			Get
				Return CType(m_ParentPage, sitelink_edit)
			End Get
		End Property

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
		Public Sub New(ByRef APage As AspNetMaker8_wpmWebsite)				
			m_ParentPage = APage
			m_Page = Me
			m_PageID = "edit"
			m_PageObjName = "SiteLink_edit"
			m_PageObjTypeName = "cSiteLink_edit"			

			' Initialize language object
			Language = New cLanguage(Me)

			' Table Name
			m_TableName = "SiteLink"

			' Initialize table object
			SiteLink = New cSiteLink(Me)

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
			SiteLink.Dispose()
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

	'
	' Page main processing
	'
	Sub Page_Main()

		' Load key from QueryString
		If ew_Get("ID") <> "" Then
			SiteLink.ID.QueryStringValue = ew_Get("ID")
		End If
		If ObjForm.GetValue("a_edit") <> "" Then
			SiteLink.CurrentAction = ObjForm.GetValue("a_edit") ' Get action code
			LoadFormValues() ' Get form values

			' Validate Form
			If Not ValidateForm() Then
				SiteLink.CurrentAction = "" ' Form error, reset action
				Message = ParentPage.gsFormError
				SiteLink.EventCancelled = True ' Event cancelled
				LoadRow() ' Restore row
				RestoreFormValues() ' Restore form values if validate failed
			End If
		Else
			SiteLink.CurrentAction = "I" ' Default action is display
		End If

		' Check if valid key
		If ew_Empty(SiteLink.ID.CurrentValue) Then Page_Terminate("sitelink_list.aspx") ' Invalid key, return to list
		Select Case SiteLink.CurrentAction
			Case "I" ' Get a record to display
				If Not LoadRow() Then ' Load Record based on key
					Message = Language.Phrase("NoRecord") ' No record found
					Page_Terminate("sitelink_list.aspx") ' No matching record, return to list
				End If
			Case "U" ' Update
				SiteLink.SendEmail = True ' Send email on update success
				If EditRow() Then ' Update Record based on key
					Message = Language.Phrase("UpdateSuccess") ' Update success
					Dim sReturnUrl As String = SiteLink.ReturnUrl
					Page_Terminate(sReturnUrl) ' Return to caller
				Else
					SiteLink.EventCancelled = True ' Event cancelled 
					LoadRow() ' Restore row
					RestoreFormValues() ' Restore form values if update failed
				End If
		End Select

		' Render the record
		SiteLink.RowType = EW_ROWTYPE_EDIT ' Render as edit

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
		SiteLink.ID.FormValue = ObjForm.GetValue("x_ID")
		SiteLink.ID.OldValue = ObjForm.GetValue("o_ID")
		SiteLink.CompanyID.FormValue = ObjForm.GetValue("x_CompanyID")
		SiteLink.CompanyID.OldValue = ObjForm.GetValue("o_CompanyID")
		SiteLink.LinkTypeCD.FormValue = ObjForm.GetValue("x_LinkTypeCD")
		SiteLink.LinkTypeCD.OldValue = ObjForm.GetValue("o_LinkTypeCD")
		SiteLink.Title.FormValue = ObjForm.GetValue("x_Title")
		SiteLink.Title.OldValue = ObjForm.GetValue("o_Title")
		SiteLink.Description.FormValue = ObjForm.GetValue("x_Description")
		SiteLink.Description.OldValue = ObjForm.GetValue("o_Description")
		SiteLink.DateAdd.FormValue = ObjForm.GetValue("x_DateAdd")
		SiteLink.DateAdd.CurrentValue = ew_UnFormatDateTime(SiteLink.DateAdd.CurrentValue, 8)
		SiteLink.DateAdd.OldValue = ObjForm.GetValue("o_DateAdd")
		SiteLink.Ranks.FormValue = ObjForm.GetValue("x_Ranks")
		SiteLink.Ranks.OldValue = ObjForm.GetValue("o_Ranks")
		SiteLink.Views.FormValue = ObjForm.GetValue("x_Views")
		SiteLink.Views.OldValue = ObjForm.GetValue("o_Views")
		SiteLink.UserName.FormValue = ObjForm.GetValue("x_UserName")
		SiteLink.UserName.OldValue = ObjForm.GetValue("o_UserName")
		SiteLink.UserID.FormValue = ObjForm.GetValue("x_UserID")
		SiteLink.UserID.OldValue = ObjForm.GetValue("o_UserID")
		SiteLink.ASIN.FormValue = ObjForm.GetValue("x_ASIN")
		SiteLink.ASIN.OldValue = ObjForm.GetValue("o_ASIN")
		SiteLink.URL.FormValue = ObjForm.GetValue("x_URL")
		SiteLink.URL.OldValue = ObjForm.GetValue("o_URL")
		SiteLink.CategoryID.FormValue = ObjForm.GetValue("x_CategoryID")
		SiteLink.CategoryID.OldValue = ObjForm.GetValue("o_CategoryID")
		SiteLink.SiteCategoryID.FormValue = ObjForm.GetValue("x_SiteCategoryID")
		SiteLink.SiteCategoryID.OldValue = ObjForm.GetValue("o_SiteCategoryID")
		SiteLink.SiteCategoryTypeID.FormValue = ObjForm.GetValue("x_SiteCategoryTypeID")
		SiteLink.SiteCategoryTypeID.OldValue = ObjForm.GetValue("o_SiteCategoryTypeID")
		SiteLink.SiteCategoryGroupID.FormValue = ObjForm.GetValue("x_SiteCategoryGroupID")
		SiteLink.SiteCategoryGroupID.OldValue = ObjForm.GetValue("o_SiteCategoryGroupID")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		SiteLink.ID.CurrentValue = SiteLink.ID.FormValue
		SiteLink.CompanyID.CurrentValue = SiteLink.CompanyID.FormValue
		SiteLink.LinkTypeCD.CurrentValue = SiteLink.LinkTypeCD.FormValue
		SiteLink.Title.CurrentValue = SiteLink.Title.FormValue
		SiteLink.Description.CurrentValue = SiteLink.Description.FormValue
		SiteLink.DateAdd.CurrentValue = SiteLink.DateAdd.FormValue
		SiteLink.DateAdd.CurrentValue = ew_UnFormatDateTime(SiteLink.DateAdd.CurrentValue, 8)
		SiteLink.Ranks.CurrentValue = SiteLink.Ranks.FormValue
		SiteLink.Views.CurrentValue = SiteLink.Views.FormValue
		SiteLink.UserName.CurrentValue = SiteLink.UserName.FormValue
		SiteLink.UserID.CurrentValue = SiteLink.UserID.FormValue
		SiteLink.ASIN.CurrentValue = SiteLink.ASIN.FormValue
		SiteLink.URL.CurrentValue = SiteLink.URL.FormValue
		SiteLink.CategoryID.CurrentValue = SiteLink.CategoryID.FormValue
		SiteLink.SiteCategoryID.CurrentValue = SiteLink.SiteCategoryID.FormValue
		SiteLink.SiteCategoryTypeID.CurrentValue = SiteLink.SiteCategoryTypeID.FormValue
		SiteLink.SiteCategoryGroupID.CurrentValue = SiteLink.SiteCategoryGroupID.FormValue
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
		If EW_DEBUG_ENABLED Then DebugMsg = sSql
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
		Dim sDetailFilter As String
		SiteLink.ID.DbValue = RsRow("ID")
		SiteLink.CompanyID.DbValue = RsRow("CompanyID")
		SiteLink.LinkTypeCD.DbValue = RsRow("LinkTypeCD")
		SiteLink.Title.DbValue = RsRow("Title")
		SiteLink.Description.DbValue = RsRow("Description")
		SiteLink.DateAdd.DbValue = RsRow("DateAdd")
		SiteLink.Ranks.DbValue = RsRow("Ranks")
		SiteLink.Views.DbValue = IIf(ew_ConvertToBool(RsRow("Views")), "1", "0")
		SiteLink.UserName.DbValue = RsRow("UserName")
		SiteLink.UserID.DbValue = RsRow("UserID")
		SiteLink.ASIN.DbValue = RsRow("ASIN")
		SiteLink.URL.DbValue = RsRow("URL")
		SiteLink.CategoryID.DbValue = RsRow("CategoryID")
		SiteLink.SiteCategoryID.DbValue = RsRow("SiteCategoryID")
		SiteLink.SiteCategoryTypeID.DbValue = RsRow("SiteCategoryTypeID")
		SiteLink.SiteCategoryGroupID.DbValue = RsRow("SiteCategoryGroupID")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Initialize urls
		' Row Rendering event

		SiteLink.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' ID

		SiteLink.ID.CellCssStyle = ""
		SiteLink.ID.CellCssClass = ""
		SiteLink.ID.CellAttrs.Clear(): SiteLink.ID.ViewAttrs.Clear(): SiteLink.ID.EditAttrs.Clear()

		' CompanyID
		SiteLink.CompanyID.CellCssStyle = ""
		SiteLink.CompanyID.CellCssClass = ""
		SiteLink.CompanyID.CellAttrs.Clear(): SiteLink.CompanyID.ViewAttrs.Clear(): SiteLink.CompanyID.EditAttrs.Clear()

		' LinkTypeCD
		SiteLink.LinkTypeCD.CellCssStyle = ""
		SiteLink.LinkTypeCD.CellCssClass = ""
		SiteLink.LinkTypeCD.CellAttrs.Clear(): SiteLink.LinkTypeCD.ViewAttrs.Clear(): SiteLink.LinkTypeCD.EditAttrs.Clear()

		' Title
		SiteLink.Title.CellCssStyle = ""
		SiteLink.Title.CellCssClass = ""
		SiteLink.Title.CellAttrs.Clear(): SiteLink.Title.ViewAttrs.Clear(): SiteLink.Title.EditAttrs.Clear()

		' Description
		SiteLink.Description.CellCssStyle = ""
		SiteLink.Description.CellCssClass = ""
		SiteLink.Description.CellAttrs.Clear(): SiteLink.Description.ViewAttrs.Clear(): SiteLink.Description.EditAttrs.Clear()

		' DateAdd
		SiteLink.DateAdd.CellCssStyle = ""
		SiteLink.DateAdd.CellCssClass = ""
		SiteLink.DateAdd.CellAttrs.Clear(): SiteLink.DateAdd.ViewAttrs.Clear(): SiteLink.DateAdd.EditAttrs.Clear()

		' Ranks
		SiteLink.Ranks.CellCssStyle = ""
		SiteLink.Ranks.CellCssClass = ""
		SiteLink.Ranks.CellAttrs.Clear(): SiteLink.Ranks.ViewAttrs.Clear(): SiteLink.Ranks.EditAttrs.Clear()

		' Views
		SiteLink.Views.CellCssStyle = ""
		SiteLink.Views.CellCssClass = ""
		SiteLink.Views.CellAttrs.Clear(): SiteLink.Views.ViewAttrs.Clear(): SiteLink.Views.EditAttrs.Clear()

		' UserName
		SiteLink.UserName.CellCssStyle = ""
		SiteLink.UserName.CellCssClass = ""
		SiteLink.UserName.CellAttrs.Clear(): SiteLink.UserName.ViewAttrs.Clear(): SiteLink.UserName.EditAttrs.Clear()

		' UserID
		SiteLink.UserID.CellCssStyle = ""
		SiteLink.UserID.CellCssClass = ""
		SiteLink.UserID.CellAttrs.Clear(): SiteLink.UserID.ViewAttrs.Clear(): SiteLink.UserID.EditAttrs.Clear()

		' ASIN
		SiteLink.ASIN.CellCssStyle = ""
		SiteLink.ASIN.CellCssClass = ""
		SiteLink.ASIN.CellAttrs.Clear(): SiteLink.ASIN.ViewAttrs.Clear(): SiteLink.ASIN.EditAttrs.Clear()

		' URL
		SiteLink.URL.CellCssStyle = ""
		SiteLink.URL.CellCssClass = ""
		SiteLink.URL.CellAttrs.Clear(): SiteLink.URL.ViewAttrs.Clear(): SiteLink.URL.EditAttrs.Clear()

		' CategoryID
		SiteLink.CategoryID.CellCssStyle = ""
		SiteLink.CategoryID.CellCssClass = ""
		SiteLink.CategoryID.CellAttrs.Clear(): SiteLink.CategoryID.ViewAttrs.Clear(): SiteLink.CategoryID.EditAttrs.Clear()

		' SiteCategoryID
		SiteLink.SiteCategoryID.CellCssStyle = ""
		SiteLink.SiteCategoryID.CellCssClass = ""
		SiteLink.SiteCategoryID.CellAttrs.Clear(): SiteLink.SiteCategoryID.ViewAttrs.Clear(): SiteLink.SiteCategoryID.EditAttrs.Clear()

		' SiteCategoryTypeID
		SiteLink.SiteCategoryTypeID.CellCssStyle = ""
		SiteLink.SiteCategoryTypeID.CellCssClass = ""
		SiteLink.SiteCategoryTypeID.CellAttrs.Clear(): SiteLink.SiteCategoryTypeID.ViewAttrs.Clear(): SiteLink.SiteCategoryTypeID.EditAttrs.Clear()

		' SiteCategoryGroupID
		SiteLink.SiteCategoryGroupID.CellCssStyle = ""
		SiteLink.SiteCategoryGroupID.CellCssClass = ""
		SiteLink.SiteCategoryGroupID.CellAttrs.Clear(): SiteLink.SiteCategoryGroupID.ViewAttrs.Clear(): SiteLink.SiteCategoryGroupID.EditAttrs.Clear()

		'
		'  View  Row
		'

		If SiteLink.RowType = EW_ROWTYPE_VIEW Then ' View row

			' ID
			SiteLink.ID.ViewValue = SiteLink.ID.CurrentValue
			SiteLink.ID.CssStyle = ""
			SiteLink.ID.CssClass = ""
			SiteLink.ID.ViewCustomAttributes = ""

			' CompanyID
			If ew_NotEmpty(SiteLink.CompanyID.CurrentValue) Then
				sFilterWrk = "[CompanyID] = " & ew_AdjustSql(SiteLink.CompanyID.CurrentValue) & ""
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

			' LinkTypeCD
			If ew_NotEmpty(SiteLink.LinkTypeCD.CurrentValue) Then
				sFilterWrk = "[LinkTypeCD] = '" & ew_AdjustSql(SiteLink.LinkTypeCD.CurrentValue) & "'"
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

			' Title
			SiteLink.Title.ViewValue = SiteLink.Title.CurrentValue
			SiteLink.Title.CssStyle = ""
			SiteLink.Title.CssClass = ""
			SiteLink.Title.ViewCustomAttributes = ""

			' Description
			SiteLink.Description.ViewValue = SiteLink.Description.CurrentValue
			SiteLink.Description.CssStyle = ""
			SiteLink.Description.CssClass = ""
			SiteLink.Description.ViewCustomAttributes = ""

			' DateAdd
			SiteLink.DateAdd.ViewValue = SiteLink.DateAdd.CurrentValue
			SiteLink.DateAdd.CssStyle = ""
			SiteLink.DateAdd.CssClass = ""
			SiteLink.DateAdd.ViewCustomAttributes = ""

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

			' UserName
			SiteLink.UserName.ViewValue = SiteLink.UserName.CurrentValue
			SiteLink.UserName.CssStyle = ""
			SiteLink.UserName.CssClass = ""
			SiteLink.UserName.ViewCustomAttributes = ""

			' UserID
			SiteLink.UserID.ViewValue = SiteLink.UserID.CurrentValue
			SiteLink.UserID.CssStyle = ""
			SiteLink.UserID.CssClass = ""
			SiteLink.UserID.ViewCustomAttributes = ""

			' ASIN
			SiteLink.ASIN.ViewValue = SiteLink.ASIN.CurrentValue
			SiteLink.ASIN.CssStyle = ""
			SiteLink.ASIN.CssClass = ""
			SiteLink.ASIN.ViewCustomAttributes = ""

			' URL
			SiteLink.URL.ViewValue = SiteLink.URL.CurrentValue
			SiteLink.URL.CssStyle = ""
			SiteLink.URL.CssClass = ""
			SiteLink.URL.ViewCustomAttributes = ""

			' CategoryID
			If ew_NotEmpty(SiteLink.CategoryID.CurrentValue) Then
				sFilterWrk = "[ID] = " & ew_AdjustSql(SiteLink.CategoryID.CurrentValue) & ""
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

			' SiteCategoryID
			SiteLink.SiteCategoryID.ViewValue = SiteLink.SiteCategoryID.CurrentValue
			SiteLink.SiteCategoryID.CssStyle = ""
			SiteLink.SiteCategoryID.CssClass = ""
			SiteLink.SiteCategoryID.ViewCustomAttributes = ""

			' SiteCategoryTypeID
			SiteLink.SiteCategoryTypeID.ViewValue = SiteLink.SiteCategoryTypeID.CurrentValue
			SiteLink.SiteCategoryTypeID.CssStyle = ""
			SiteLink.SiteCategoryTypeID.CssClass = ""
			SiteLink.SiteCategoryTypeID.ViewCustomAttributes = ""

			' SiteCategoryGroupID
			SiteLink.SiteCategoryGroupID.ViewValue = SiteLink.SiteCategoryGroupID.CurrentValue
			SiteLink.SiteCategoryGroupID.CssStyle = ""
			SiteLink.SiteCategoryGroupID.CssClass = ""
			SiteLink.SiteCategoryGroupID.ViewCustomAttributes = ""

			' View refer script
			' ID

			SiteLink.ID.HrefValue = ""
			SiteLink.ID.TooltipValue = ""

			' CompanyID
			SiteLink.CompanyID.HrefValue = ""
			SiteLink.CompanyID.TooltipValue = ""

			' LinkTypeCD
			SiteLink.LinkTypeCD.HrefValue = ""
			SiteLink.LinkTypeCD.TooltipValue = ""

			' Title
			SiteLink.Title.HrefValue = ""
			SiteLink.Title.TooltipValue = ""

			' Description
			SiteLink.Description.HrefValue = ""
			SiteLink.Description.TooltipValue = ""

			' DateAdd
			SiteLink.DateAdd.HrefValue = ""
			SiteLink.DateAdd.TooltipValue = ""

			' Ranks
			SiteLink.Ranks.HrefValue = ""
			SiteLink.Ranks.TooltipValue = ""

			' Views
			SiteLink.Views.HrefValue = ""
			SiteLink.Views.TooltipValue = ""

			' UserName
			SiteLink.UserName.HrefValue = ""
			SiteLink.UserName.TooltipValue = ""

			' UserID
			SiteLink.UserID.HrefValue = ""
			SiteLink.UserID.TooltipValue = ""

			' ASIN
			SiteLink.ASIN.HrefValue = ""
			SiteLink.ASIN.TooltipValue = ""

			' URL
			SiteLink.URL.HrefValue = ""
			SiteLink.URL.TooltipValue = ""

			' CategoryID
			SiteLink.CategoryID.HrefValue = ""
			SiteLink.CategoryID.TooltipValue = ""

			' SiteCategoryID
			SiteLink.SiteCategoryID.HrefValue = ""
			SiteLink.SiteCategoryID.TooltipValue = ""

			' SiteCategoryTypeID
			SiteLink.SiteCategoryTypeID.HrefValue = ""
			SiteLink.SiteCategoryTypeID.TooltipValue = ""

			' SiteCategoryGroupID
			SiteLink.SiteCategoryGroupID.HrefValue = ""
			SiteLink.SiteCategoryGroupID.TooltipValue = ""

		'
		'  Edit Row
		'

		ElseIf SiteLink.RowType = EW_ROWTYPE_EDIT Then ' Edit row

			' ID
			SiteLink.ID.EditCustomAttributes = ""
			SiteLink.ID.EditValue = SiteLink.ID.CurrentValue
			SiteLink.ID.CssStyle = ""
			SiteLink.ID.CssClass = ""
			SiteLink.ID.ViewCustomAttributes = ""

			' CompanyID
			SiteLink.CompanyID.EditCustomAttributes = ""
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
			SiteLink.CompanyID.EditValue = arwrk

			' LinkTypeCD
			SiteLink.LinkTypeCD.EditCustomAttributes = ""
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
			SiteLink.LinkTypeCD.EditValue = arwrk

			' Title
			SiteLink.Title.EditCustomAttributes = ""
			SiteLink.Title.EditValue = ew_HtmlEncode(SiteLink.Title.CurrentValue)

			' Description
			SiteLink.Description.EditCustomAttributes = ""
			SiteLink.Description.EditValue = ew_HtmlEncode(SiteLink.Description.CurrentValue)

			' DateAdd
			SiteLink.DateAdd.EditCustomAttributes = ""
			SiteLink.DateAdd.EditValue = SiteLink.DateAdd.CurrentValue

			' Ranks
			SiteLink.Ranks.EditCustomAttributes = ""
			SiteLink.Ranks.EditValue = ew_HtmlEncode(SiteLink.Ranks.CurrentValue)

			' Views
			SiteLink.Views.EditCustomAttributes = ""

			' UserName
			SiteLink.UserName.EditCustomAttributes = ""
			SiteLink.UserName.EditValue = ew_HtmlEncode(SiteLink.UserName.CurrentValue)

			' UserID
			SiteLink.UserID.EditCustomAttributes = ""
			SiteLink.UserID.EditValue = ew_HtmlEncode(SiteLink.UserID.CurrentValue)

			' ASIN
			SiteLink.ASIN.EditCustomAttributes = ""
			SiteLink.ASIN.EditValue = ew_HtmlEncode(SiteLink.ASIN.CurrentValue)

			' URL
			SiteLink.URL.EditCustomAttributes = ""
			SiteLink.URL.EditValue = ew_HtmlEncode(SiteLink.URL.CurrentValue)

			' CategoryID
			SiteLink.CategoryID.EditCustomAttributes = ""
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
			SiteLink.CategoryID.EditValue = arwrk

			' SiteCategoryID
			SiteLink.SiteCategoryID.EditCustomAttributes = ""
			SiteLink.SiteCategoryID.EditValue = ew_HtmlEncode(SiteLink.SiteCategoryID.CurrentValue)

			' SiteCategoryTypeID
			SiteLink.SiteCategoryTypeID.EditCustomAttributes = ""
			SiteLink.SiteCategoryTypeID.EditValue = ew_HtmlEncode(SiteLink.SiteCategoryTypeID.CurrentValue)

			' SiteCategoryGroupID
			SiteLink.SiteCategoryGroupID.EditCustomAttributes = ""
			SiteLink.SiteCategoryGroupID.EditValue = ew_HtmlEncode(SiteLink.SiteCategoryGroupID.CurrentValue)

			' Edit refer script
			' ID

			SiteLink.ID.HrefValue = ""

			' CompanyID
			SiteLink.CompanyID.HrefValue = ""

			' LinkTypeCD
			SiteLink.LinkTypeCD.HrefValue = ""

			' Title
			SiteLink.Title.HrefValue = ""

			' Description
			SiteLink.Description.HrefValue = ""

			' DateAdd
			SiteLink.DateAdd.HrefValue = ""

			' Ranks
			SiteLink.Ranks.HrefValue = ""

			' Views
			SiteLink.Views.HrefValue = ""

			' UserName
			SiteLink.UserName.HrefValue = ""

			' UserID
			SiteLink.UserID.HrefValue = ""

			' ASIN
			SiteLink.ASIN.HrefValue = ""

			' URL
			SiteLink.URL.HrefValue = ""

			' CategoryID
			SiteLink.CategoryID.HrefValue = ""

			' SiteCategoryID
			SiteLink.SiteCategoryID.HrefValue = ""

			' SiteCategoryTypeID
			SiteLink.SiteCategoryTypeID.HrefValue = ""

			' SiteCategoryGroupID
			SiteLink.SiteCategoryGroupID.HrefValue = ""
		End If

		' Row Rendered event
		If SiteLink.RowType <> EW_ROWTYPE_AGGREGATEINIT Then
			SiteLink.Row_Rendered()
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
		If Not ew_CheckInteger(SiteLink.Ranks.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError &= SiteLink.Ranks.FldErrMsg
		End If
		If Not ew_CheckInteger(SiteLink.UserID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError &= SiteLink.UserID.FldErrMsg
		End If
		If Not ew_CheckInteger(SiteLink.SiteCategoryID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError &= SiteLink.SiteCategoryID.FldErrMsg
		End If
		If Not ew_CheckInteger(SiteLink.SiteCategoryTypeID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError &= SiteLink.SiteCategoryTypeID.FldErrMsg
		End If
		If Not ew_CheckInteger(SiteLink.SiteCategoryGroupID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError &= SiteLink.SiteCategoryGroupID.FldErrMsg
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
		sFilter = SiteLink.KeyFilter
		SiteLink.CurrentFilter  = sFilter
		sSql = SiteLink.SQL
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
			Try
				RsOld = Conn.GetRow(RsEdit)					 

				' ID
				' CompanyID

				SiteLink.CompanyID.SetDbValue(Rs, SiteLink.CompanyID.CurrentValue, System.DBNull.Value, False)

				' LinkTypeCD
				SiteLink.LinkTypeCD.SetDbValue(Rs, SiteLink.LinkTypeCD.CurrentValue, System.DBNull.Value, False)

				' Title
				SiteLink.Title.SetDbValue(Rs, SiteLink.Title.CurrentValue, System.DBNull.Value, False)

				' Description
				SiteLink.Description.SetDbValue(Rs, SiteLink.Description.CurrentValue, System.DBNull.Value, False)

				' DateAdd
				SiteLink.DateAdd.SetDbValue(Rs, SiteLink.DateAdd.CurrentValue, System.DBNull.Value, False)

				' Ranks
				SiteLink.Ranks.SetDbValue(Rs, SiteLink.Ranks.CurrentValue, System.DBNull.Value, False)

				' Views
				SiteLink.Views.SetDbValue(Rs, (SiteLink.Views.CurrentValue <> "" AndAlso Not IsDBNull(SiteLink.Views.CurrentValue)), False, False)

				' UserName
				SiteLink.UserName.SetDbValue(Rs, SiteLink.UserName.CurrentValue, System.DBNull.Value, False)

				' UserID
				SiteLink.UserID.SetDbValue(Rs, SiteLink.UserID.CurrentValue, System.DBNull.Value, False)

				' ASIN
				SiteLink.ASIN.SetDbValue(Rs, SiteLink.ASIN.CurrentValue, System.DBNull.Value, False)

				' URL
				SiteLink.URL.SetDbValue(Rs, SiteLink.URL.CurrentValue, System.DBNull.Value, False)

				' CategoryID
				SiteLink.CategoryID.SetDbValue(Rs, SiteLink.CategoryID.CurrentValue, System.DBNull.Value, False)

				' SiteCategoryID
				SiteLink.SiteCategoryID.SetDbValue(Rs, SiteLink.SiteCategoryID.CurrentValue, System.DBNull.Value, False)

				' SiteCategoryTypeID
				SiteLink.SiteCategoryTypeID.SetDbValue(Rs, SiteLink.SiteCategoryTypeID.CurrentValue, System.DBNull.Value, False)

				' SiteCategoryGroupID
				SiteLink.SiteCategoryGroupID.SetDbValue(Rs, SiteLink.SiteCategoryGroupID.CurrentValue, System.DBNull.Value, False)
			Catch e As Exception
				If EW_DEBUG_ENABLED Then Throw
				Message = e.Message
				EditRow = False
			End Try
			RsEdit.Close()

			' Row Updating event
			bUpdateRow = SiteLink.Row_Updating(RsOld, Rs)
			If bUpdateRow Then
				Try
					SiteLink.Update(Rs)
					EditRow = True
				Catch e As Exception
					If EW_DEBUG_ENABLED Then Throw
					Message = e.Message
					EditRow = False
				End Try
			Else
				If SiteLink.CancelMessage <> "" Then
					Message = SiteLink.CancelMessage
					SiteLink.CancelMessage = ""
				Else
					Message = Language.Phrase("UpdateCancelled")
				End If
				EditRow = False
			End If
		End If

		' Row Updated event
		If EditRow Then
			SiteLink.Row_Updated(RsOld, Rs)
		End If
		If EditRow Then
			WriteAuditTrailOnEdit(RsOld, Rs)
		End If
	End Function

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "SiteLink"
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

	' Write Audit Trail (edit page)
	Sub WriteAuditTrailOnEdit(ByRef RsOld As OrderedDictionary, ByRef RsNew As OrderedDictionary)
		Dim table As String = "SiteLink"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsOld("ID")

		' Write Audit Trail
		Dim filePfx As String = "log"
		Dim action As String = "U"
		Dim curDateTime As String, id As String, user As String, field As String
		Dim keyvalue As Object, oldvalue As Object, newvalue As Object
		Dim dt As DateTime = Now()
		Dim fld As cField, fldname As String, Modified As Boolean
		curDateTime = dt.ToString("yyyy/MM/dd HH:mm:ss")
		id = HttpContext.Current.Request.ServerVariables("SCRIPT_NAME")
    user = CurrentUserName()
		Dim arFldNames(RsNew.Count - 1) As String
		RsNew.Keys.CopyTo(arFldNames, 0)
		For Each fldname In arFldNames
			fld = SiteLink.FieldByName(fldname)
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
					ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, fldname, keyvalue, oldvalue, newvalue)
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
		SiteLink_edit = New cSiteLink_edit(Me)		
		SiteLink_edit.Page_Init()		

		' Set buffer/cache option
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

		' Page main processing
		SiteLink_edit.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If SiteLink_edit IsNot Nothing Then SiteLink_edit.Dispose()
	End Sub
End Class
