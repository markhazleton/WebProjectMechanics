Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class article_add
	Inherits AspNetMaker8_wpmWebsite

	' Page object
	Public Article_add As cArticle_add

	'
	' Page Class
	'
	Class cArticle_add
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
				If Article.UseTokenInUrl Then Url = Url & "t=" & Article.TableVar & "&" ' Add page token
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
			If Article.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (Article.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (Article.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' ASP.NET page object
		Public ReadOnly Property AspNetPage() As article_add
			Get
				Return CType(m_ParentPage, article_add)
			End Get
		End Property

		' Article
		Public Property Article() As cArticle
			Get				
				Return ParentPage.Article
			End Get
			Set(ByVal v As cArticle)
				ParentPage.Article = v	
			End Set	
		End Property

		' Article
		Public Property Company() As cCompany
			Get				
				Return ParentPage.Company
			End Get
			Set(ByVal v As cCompany)
				ParentPage.Company = v	
			End Set	
		End Property

		' Article
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
		Public Sub New(ByRef APage As AspNetMaker8_wpmWebsite)				
			m_ParentPage = APage
			m_Page = Me
			m_PageID = "add"
			m_PageObjName = "Article_add"
			m_PageObjTypeName = "cArticle_add"			

			' Initialize language object
			Language = New cLanguage(Me)

			' Table Name
			m_TableName = "Article"

			' Initialize table object
			Article = New cArticle(Me)
			Company = New cCompany(Me)
			zPage = New czPage(Me)

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
			Article.Dispose()
			Company.Dispose()
			zPage.Dispose()
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
		If ew_Get("ArticleID") <> "" Then
			Article.ArticleID.QueryStringValue = ew_Get("ArticleID")
		Else
			bCopy = False
		End If

		' Set up master detail parameters
		SetUpMasterDetail()

		' Process form if post back
		If ObjForm.GetValue("a_add") <> "" Then
			Article.CurrentAction = ObjForm.GetValue("a_add") ' Get form action
			LoadFormValues() ' Load form values

			' Validate Form
			If Not ValidateForm() Then
				Article.CurrentAction = "I" ' Form error, reset action
				Message = ParentPage.gsFormError
			End If

		' Not post back
		Else
			If bCopy Then
				Article.CurrentAction = "C" ' Copy Record
			Else
				Article.CurrentAction = "I" ' Display Blank Record
				LoadDefaultValues() ' Load default values
			End If
		End If

		' Perform action based on action code
		Select Case Article.CurrentAction
			Case "I" ' Blank record, no action required
			Case "C" ' Copy an existing record
				If Not LoadRow() Then ' Load record based on key
					Message = Language.Phrase("NoRecord") ' No record found
					Page_Terminate("article_list.aspx") ' No matching record, return to list
				End If
			Case "A" ' Add new record
				Article.SendEmail = True ' Send email on add success
				If AddRow() Then ' Add successful
					Message = Language.Phrase("AddSuccess") ' Set up success message
					Dim sReturnUrl As String = Article.ReturnUrl
					Page_Terminate(sReturnUrl) ' Clean up and return
				Else
					RestoreFormValues() ' Add failed, restore form values
				End If
		End Select

		' Render row based on row type
		Article.RowType = EW_ROWTYPE_ADD ' Render add type

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
		Article.Active.CurrentValue = 1
		Article.CompanyID.CurrentValue = 0
		Article.zPageID.CurrentValue = 0
		Article.userID.CurrentValue = 0
		Article.Counter.CurrentValue = 0
		Article.VersionNo.CurrentValue = 0
		Article.ContactID.CurrentValue = 0
	End Sub

	'
	' Load form values
	'
	Sub LoadFormValues()
		Article.Active.FormValue = ObjForm.GetValue("x_Active")
		Article.Active.OldValue = ObjForm.GetValue("o_Active")
		Article.StartDT.FormValue = ObjForm.GetValue("x_StartDT")
		Article.StartDT.CurrentValue = ew_UnFormatDateTime(Article.StartDT.CurrentValue, 8)
		Article.StartDT.OldValue = ObjForm.GetValue("o_StartDT")
		Article.Title.FormValue = ObjForm.GetValue("x_Title")
		Article.Title.OldValue = ObjForm.GetValue("o_Title")
		Article.CompanyID.FormValue = ObjForm.GetValue("x_CompanyID")
		Article.CompanyID.OldValue = ObjForm.GetValue("o_CompanyID")
		Article.zPageID.FormValue = ObjForm.GetValue("x_zPageID")
		Article.zPageID.OldValue = ObjForm.GetValue("o_zPageID")
		Article.userID.FormValue = ObjForm.GetValue("x_userID")
		Article.userID.OldValue = ObjForm.GetValue("o_userID")
		Article.Description.FormValue = ObjForm.GetValue("x_Description")
		Article.Description.OldValue = ObjForm.GetValue("o_Description")
		Article.ArticleSummary.FormValue = ObjForm.GetValue("x_ArticleSummary")
		Article.ArticleSummary.OldValue = ObjForm.GetValue("o_ArticleSummary")
		Article.ArticleBody.FormValue = ObjForm.GetValue("x_ArticleBody")
		Article.ArticleBody.OldValue = ObjForm.GetValue("o_ArticleBody")
		Article.EndDT.FormValue = ObjForm.GetValue("x_EndDT")
		Article.EndDT.CurrentValue = ew_UnFormatDateTime(Article.EndDT.CurrentValue, 8)
		Article.EndDT.OldValue = ObjForm.GetValue("o_EndDT")
		Article.ExpireDT.FormValue = ObjForm.GetValue("x_ExpireDT")
		Article.ExpireDT.CurrentValue = ew_UnFormatDateTime(Article.ExpireDT.CurrentValue, 8)
		Article.ExpireDT.OldValue = ObjForm.GetValue("o_ExpireDT")
		Article.Author.FormValue = ObjForm.GetValue("x_Author")
		Article.Author.OldValue = ObjForm.GetValue("o_Author")
		Article.Counter.FormValue = ObjForm.GetValue("x_Counter")
		Article.Counter.OldValue = ObjForm.GetValue("o_Counter")
		Article.VersionNo.FormValue = ObjForm.GetValue("x_VersionNo")
		Article.VersionNo.OldValue = ObjForm.GetValue("o_VersionNo")
		Article.ContactID.FormValue = ObjForm.GetValue("x_ContactID")
		Article.ContactID.OldValue = ObjForm.GetValue("o_ContactID")
		Article.ModifiedDT.FormValue = ObjForm.GetValue("x_ModifiedDT")
		Article.ModifiedDT.CurrentValue = ew_UnFormatDateTime(Article.ModifiedDT.CurrentValue, 8)
		Article.ModifiedDT.OldValue = ObjForm.GetValue("o_ModifiedDT")
		Article.ArticleID.FormValue = ObjForm.GetValue("x_ArticleID")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		Article.Active.CurrentValue = Article.Active.FormValue
		Article.StartDT.CurrentValue = Article.StartDT.FormValue
		Article.StartDT.CurrentValue = ew_UnFormatDateTime(Article.StartDT.CurrentValue, 8)
		Article.Title.CurrentValue = Article.Title.FormValue
		Article.CompanyID.CurrentValue = Article.CompanyID.FormValue
		Article.zPageID.CurrentValue = Article.zPageID.FormValue
		Article.userID.CurrentValue = Article.userID.FormValue
		Article.Description.CurrentValue = Article.Description.FormValue
		Article.ArticleSummary.CurrentValue = Article.ArticleSummary.FormValue
		Article.ArticleBody.CurrentValue = Article.ArticleBody.FormValue
		Article.EndDT.CurrentValue = Article.EndDT.FormValue
		Article.EndDT.CurrentValue = ew_UnFormatDateTime(Article.EndDT.CurrentValue, 8)
		Article.ExpireDT.CurrentValue = Article.ExpireDT.FormValue
		Article.ExpireDT.CurrentValue = ew_UnFormatDateTime(Article.ExpireDT.CurrentValue, 8)
		Article.Author.CurrentValue = Article.Author.FormValue
		Article.Counter.CurrentValue = Article.Counter.FormValue
		Article.VersionNo.CurrentValue = Article.VersionNo.FormValue
		Article.ContactID.CurrentValue = Article.ContactID.FormValue
		Article.ModifiedDT.CurrentValue = Article.ModifiedDT.FormValue
		Article.ModifiedDT.CurrentValue = ew_UnFormatDateTime(Article.ModifiedDT.CurrentValue, 8)
		Article.ArticleID.CurrentValue = Article.ArticleID.FormValue
	End Sub

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = Article.KeyFilter

		' Row Selecting event
		Article.Row_Selecting(sFilter)

		' Load SQL based on filter
		Article.CurrentFilter = sFilter
		Dim sSql As String = Article.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then DebugMsg = sSql
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				Article.Row_Selected(RsRow)
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
		Article.ArticleID.DbValue = RsRow("ArticleID")
		Article.Active.DbValue = IIf(ew_ConvertToBool(RsRow("Active")), "1", "0")
		Article.StartDT.DbValue = RsRow("StartDT")
		Article.Title.DbValue = RsRow("Title")
		Article.CompanyID.DbValue = RsRow("CompanyID")
		Article.zPageID.DbValue = RsRow("PageID")
		Article.userID.DbValue = RsRow("userID")
		Article.Description.DbValue = RsRow("Description")
		Article.ArticleSummary.DbValue = RsRow("ArticleSummary")
		Article.ArticleBody.DbValue = RsRow("ArticleBody")
		Article.EndDT.DbValue = RsRow("EndDT")
		Article.ExpireDT.DbValue = RsRow("ExpireDT")
		Article.Author.DbValue = RsRow("Author")
		Article.Counter.DbValue = RsRow("Counter")
		Article.VersionNo.DbValue = RsRow("VersionNo")
		Article.ContactID.DbValue = RsRow("ContactID")
		Article.ModifiedDT.DbValue = RsRow("ModifiedDT")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Initialize urls
		' Row Rendering event

		Article.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' Active

		Article.Active.CellCssStyle = ""
		Article.Active.CellCssClass = ""
		Article.Active.CellAttrs.Clear(): Article.Active.ViewAttrs.Clear(): Article.Active.EditAttrs.Clear()

		' StartDT
		Article.StartDT.CellCssStyle = ""
		Article.StartDT.CellCssClass = ""
		Article.StartDT.CellAttrs.Clear(): Article.StartDT.ViewAttrs.Clear(): Article.StartDT.EditAttrs.Clear()

		' Title
		Article.Title.CellCssStyle = ""
		Article.Title.CellCssClass = ""
		Article.Title.CellAttrs.Clear(): Article.Title.ViewAttrs.Clear(): Article.Title.EditAttrs.Clear()

		' CompanyID
		Article.CompanyID.CellCssStyle = ""
		Article.CompanyID.CellCssClass = ""
		Article.CompanyID.CellAttrs.Clear(): Article.CompanyID.ViewAttrs.Clear(): Article.CompanyID.EditAttrs.Clear()

		' PageID
		Article.zPageID.CellCssStyle = ""
		Article.zPageID.CellCssClass = ""
		Article.zPageID.CellAttrs.Clear(): Article.zPageID.ViewAttrs.Clear(): Article.zPageID.EditAttrs.Clear()

		' userID
		Article.userID.CellCssStyle = ""
		Article.userID.CellCssClass = ""
		Article.userID.CellAttrs.Clear(): Article.userID.ViewAttrs.Clear(): Article.userID.EditAttrs.Clear()

		' Description
		Article.Description.CellCssStyle = ""
		Article.Description.CellCssClass = ""
		Article.Description.CellAttrs.Clear(): Article.Description.ViewAttrs.Clear(): Article.Description.EditAttrs.Clear()

		' ArticleSummary
		Article.ArticleSummary.CellCssStyle = ""
		Article.ArticleSummary.CellCssClass = ""
		Article.ArticleSummary.CellAttrs.Clear(): Article.ArticleSummary.ViewAttrs.Clear(): Article.ArticleSummary.EditAttrs.Clear()

		' ArticleBody
		Article.ArticleBody.CellCssStyle = ""
		Article.ArticleBody.CellCssClass = ""
		Article.ArticleBody.CellAttrs.Clear(): Article.ArticleBody.ViewAttrs.Clear(): Article.ArticleBody.EditAttrs.Clear()

		' EndDT
		Article.EndDT.CellCssStyle = ""
		Article.EndDT.CellCssClass = ""
		Article.EndDT.CellAttrs.Clear(): Article.EndDT.ViewAttrs.Clear(): Article.EndDT.EditAttrs.Clear()

		' ExpireDT
		Article.ExpireDT.CellCssStyle = ""
		Article.ExpireDT.CellCssClass = ""
		Article.ExpireDT.CellAttrs.Clear(): Article.ExpireDT.ViewAttrs.Clear(): Article.ExpireDT.EditAttrs.Clear()

		' Author
		Article.Author.CellCssStyle = ""
		Article.Author.CellCssClass = ""
		Article.Author.CellAttrs.Clear(): Article.Author.ViewAttrs.Clear(): Article.Author.EditAttrs.Clear()

		' Counter
		Article.Counter.CellCssStyle = ""
		Article.Counter.CellCssClass = ""
		Article.Counter.CellAttrs.Clear(): Article.Counter.ViewAttrs.Clear(): Article.Counter.EditAttrs.Clear()

		' VersionNo
		Article.VersionNo.CellCssStyle = ""
		Article.VersionNo.CellCssClass = ""
		Article.VersionNo.CellAttrs.Clear(): Article.VersionNo.ViewAttrs.Clear(): Article.VersionNo.EditAttrs.Clear()

		' ContactID
		Article.ContactID.CellCssStyle = ""
		Article.ContactID.CellCssClass = ""
		Article.ContactID.CellAttrs.Clear(): Article.ContactID.ViewAttrs.Clear(): Article.ContactID.EditAttrs.Clear()

		' ModifiedDT
		Article.ModifiedDT.CellCssStyle = ""
		Article.ModifiedDT.CellCssClass = ""
		Article.ModifiedDT.CellAttrs.Clear(): Article.ModifiedDT.ViewAttrs.Clear(): Article.ModifiedDT.EditAttrs.Clear()

		'
		'  View  Row
		'

		If Article.RowType = EW_ROWTYPE_VIEW Then ' View row

			' ArticleID
			Article.ArticleID.ViewValue = Article.ArticleID.CurrentValue
			Article.ArticleID.CssStyle = ""
			Article.ArticleID.CssClass = ""
			Article.ArticleID.ViewCustomAttributes = ""

			' Active
			If Convert.ToString(Article.Active.CurrentValue) = "1" Then
				Article.Active.ViewValue = "Yes"
			Else
				Article.Active.ViewValue = "No"
			End If
			Article.Active.CssStyle = ""
			Article.Active.CssClass = ""
			Article.Active.ViewCustomAttributes = ""

			' StartDT
			Article.StartDT.ViewValue = Article.StartDT.CurrentValue
			Article.StartDT.CssStyle = ""
			Article.StartDT.CssClass = ""
			Article.StartDT.ViewCustomAttributes = ""

			' Title
			Article.Title.ViewValue = Article.Title.CurrentValue
			Article.Title.CssStyle = ""
			Article.Title.CssClass = ""
			Article.Title.ViewCustomAttributes = ""

			' CompanyID
			If ew_NotEmpty(Article.CompanyID.CurrentValue) Then
				sFilterWrk = "[CompanyID] = " & ew_AdjustSql(Article.CompanyID.CurrentValue) & ""
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
					Article.CompanyID.ViewValue = RsWrk("CompanyName")
				Else
					Article.CompanyID.ViewValue = Article.CompanyID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Article.CompanyID.ViewValue = System.DBNull.Value
			End If
			Article.CompanyID.CssStyle = ""
			Article.CompanyID.CssClass = ""
			Article.CompanyID.ViewCustomAttributes = ""

			' PageID
			If ew_NotEmpty(Article.zPageID.CurrentValue) Then
				sFilterWrk = "[PageID] = " & ew_AdjustSql(Article.zPageID.CurrentValue) & ""
			sSqlWrk = "SELECT [PageName] FROM [Page]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sWhereWrk &= " AND "
			sWhereWrk &= "(" & "[CompanyID]=" & HttpContext.Current.Session("CompanyID") & "" & ")"
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [PageName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Article.zPageID.ViewValue = RsWrk("PageName")
				Else
					Article.zPageID.ViewValue = Article.zPageID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Article.zPageID.ViewValue = System.DBNull.Value
			End If
			Article.zPageID.CssStyle = ""
			Article.zPageID.CssClass = ""
			Article.zPageID.ViewCustomAttributes = ""

			' userID
			If ew_NotEmpty(Article.userID.CurrentValue) Then
				sFilterWrk = "[ContactID] = " & ew_AdjustSql(Article.userID.CurrentValue) & ""
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
					Article.userID.ViewValue = RsWrk("PrimaryContact")
				Else
					Article.userID.ViewValue = Article.userID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Article.userID.ViewValue = System.DBNull.Value
			End If
			Article.userID.CssStyle = ""
			Article.userID.CssClass = ""
			Article.userID.ViewCustomAttributes = ""

			' Description
			Article.Description.ViewValue = Article.Description.CurrentValue
			Article.Description.CssStyle = ""
			Article.Description.CssClass = ""
			Article.Description.ViewCustomAttributes = ""

			' ArticleSummary
			Article.ArticleSummary.ViewValue = Article.ArticleSummary.CurrentValue
			Article.ArticleSummary.CssStyle = ""
			Article.ArticleSummary.CssClass = ""
			Article.ArticleSummary.ViewCustomAttributes = ""

			' ArticleBody
			Article.ArticleBody.ViewValue = Article.ArticleBody.CurrentValue
			Article.ArticleBody.CssStyle = ""
			Article.ArticleBody.CssClass = ""
			Article.ArticleBody.ViewCustomAttributes = ""

			' EndDT
			Article.EndDT.ViewValue = Article.EndDT.CurrentValue
			Article.EndDT.CssStyle = ""
			Article.EndDT.CssClass = ""
			Article.EndDT.ViewCustomAttributes = ""

			' ExpireDT
			Article.ExpireDT.ViewValue = Article.ExpireDT.CurrentValue
			Article.ExpireDT.CssStyle = ""
			Article.ExpireDT.CssClass = ""
			Article.ExpireDT.ViewCustomAttributes = ""

			' Author
			If ew_NotEmpty(Article.Author.CurrentValue) Then
				sFilterWrk = "[PrimaryContact] = '" & ew_AdjustSql(Article.Author.CurrentValue) & "'"
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
					Article.Author.ViewValue = RsWrk("PrimaryContact")
				Else
					Article.Author.ViewValue = Article.Author.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Article.Author.ViewValue = System.DBNull.Value
			End If
			Article.Author.CssStyle = ""
			Article.Author.CssClass = ""
			Article.Author.ViewCustomAttributes = ""

			' Counter
			Article.Counter.ViewValue = Article.Counter.CurrentValue
			Article.Counter.CssStyle = ""
			Article.Counter.CssClass = ""
			Article.Counter.ViewCustomAttributes = ""

			' VersionNo
			Article.VersionNo.ViewValue = Article.VersionNo.CurrentValue
			Article.VersionNo.CssStyle = ""
			Article.VersionNo.CssClass = ""
			Article.VersionNo.ViewCustomAttributes = ""

			' ContactID
			Article.ContactID.ViewValue = Article.ContactID.CurrentValue
			Article.ContactID.CssStyle = ""
			Article.ContactID.CssClass = ""
			Article.ContactID.ViewCustomAttributes = ""

			' ModifiedDT
			Article.ModifiedDT.ViewValue = Article.ModifiedDT.CurrentValue
			Article.ModifiedDT.CssStyle = ""
			Article.ModifiedDT.CssClass = ""
			Article.ModifiedDT.ViewCustomAttributes = ""

			' View refer script
			' Active

			Article.Active.HrefValue = ""
			Article.Active.TooltipValue = ""

			' StartDT
			Article.StartDT.HrefValue = ""
			Article.StartDT.TooltipValue = ""

			' Title
			Article.Title.HrefValue = ""
			Article.Title.TooltipValue = ""

			' CompanyID
			Article.CompanyID.HrefValue = ""
			Article.CompanyID.TooltipValue = ""

			' PageID
			Article.zPageID.HrefValue = ""
			Article.zPageID.TooltipValue = ""

			' userID
			Article.userID.HrefValue = ""
			Article.userID.TooltipValue = ""

			' Description
			Article.Description.HrefValue = ""
			Article.Description.TooltipValue = ""

			' ArticleSummary
			Article.ArticleSummary.HrefValue = ""
			Article.ArticleSummary.TooltipValue = ""

			' ArticleBody
			Article.ArticleBody.HrefValue = ""
			Article.ArticleBody.TooltipValue = ""

			' EndDT
			Article.EndDT.HrefValue = ""
			Article.EndDT.TooltipValue = ""

			' ExpireDT
			Article.ExpireDT.HrefValue = ""
			Article.ExpireDT.TooltipValue = ""

			' Author
			Article.Author.HrefValue = ""
			Article.Author.TooltipValue = ""

			' Counter
			Article.Counter.HrefValue = ""
			Article.Counter.TooltipValue = ""

			' VersionNo
			Article.VersionNo.HrefValue = ""
			Article.VersionNo.TooltipValue = ""

			' ContactID
			Article.ContactID.HrefValue = ""
			Article.ContactID.TooltipValue = ""

			' ModifiedDT
			Article.ModifiedDT.HrefValue = ""
			Article.ModifiedDT.TooltipValue = ""

		'
		'  Add Row
		'

		ElseIf Article.RowType = EW_ROWTYPE_ADD Then ' Add row

			' Active
			Article.Active.EditCustomAttributes = ""

			' StartDT
			' Title

			Article.Title.EditCustomAttributes = ""
			Article.Title.EditValue = ew_HtmlEncode(Article.Title.CurrentValue)

			' CompanyID
			Article.CompanyID.EditCustomAttributes = ""
			If Article.CompanyID.SessionValue <> "" Then
				Article.CompanyID.CurrentValue = Article.CompanyID.SessionValue
			If ew_NotEmpty(Article.CompanyID.CurrentValue) Then
				sFilterWrk = "[CompanyID] = " & ew_AdjustSql(Article.CompanyID.CurrentValue) & ""
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
					Article.CompanyID.ViewValue = RsWrk("CompanyName")
				Else
					Article.CompanyID.ViewValue = Article.CompanyID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Article.CompanyID.ViewValue = System.DBNull.Value
			End If
			Article.CompanyID.CssStyle = ""
			Article.CompanyID.CssClass = ""
			Article.CompanyID.ViewCustomAttributes = ""
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
			Article.CompanyID.EditValue = arwrk
			End If

			' PageID
			Article.zPageID.EditCustomAttributes = ""
			If Article.zPageID.SessionValue <> "" Then
				Article.zPageID.CurrentValue = Article.zPageID.SessionValue
			If ew_NotEmpty(Article.zPageID.CurrentValue) Then
				sFilterWrk = "[PageID] = " & ew_AdjustSql(Article.zPageID.CurrentValue) & ""
			sSqlWrk = "SELECT [PageName] FROM [Page]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sWhereWrk &= " AND "
			sWhereWrk &= "(" & "[CompanyID]=" & HttpContext.Current.Session("CompanyID") & "" & ")"
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [PageName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Article.zPageID.ViewValue = RsWrk("PageName")
				Else
					Article.zPageID.ViewValue = Article.zPageID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Article.zPageID.ViewValue = System.DBNull.Value
			End If
			Article.zPageID.CssStyle = ""
			Article.zPageID.CssClass = ""
			Article.zPageID.ViewCustomAttributes = ""
			Else
			sFilterWrk = ""
			sSqlWrk = "SELECT [PageID], [PageName], '' AS Disp2Fld, [CompanyID] FROM [Page]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sWhereWrk &= " AND "
			sWhereWrk &= "(" & "[CompanyID]=" & HttpContext.Current.Session("CompanyID") & "" & ")"
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [PageName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", Language.Phrase("PleaseSelect"), ""}) 
			Article.zPageID.EditValue = arwrk
			End If

			' userID
			Article.userID.EditCustomAttributes = ""
			sFilterWrk = ""
			sSqlWrk = "SELECT [ContactID], [PrimaryContact], '' AS Disp2Fld, [CompanyID] FROM [Contact]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [PrimaryContact] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", Language.Phrase("PleaseSelect"), ""}) 
			Article.userID.EditValue = arwrk

			' Description
			Article.Description.EditCustomAttributes = ""
			Article.Description.EditValue = ew_HtmlEncode(Article.Description.CurrentValue)

			' ArticleSummary
			Article.ArticleSummary.EditCustomAttributes = ""
			Article.ArticleSummary.EditValue = ew_HtmlEncode(Article.ArticleSummary.CurrentValue)

			' ArticleBody
			Article.ArticleBody.EditCustomAttributes = ""
			Article.ArticleBody.EditValue = ew_HtmlEncode(Article.ArticleBody.CurrentValue)

			' EndDT
			Article.EndDT.EditCustomAttributes = ""
			Article.EndDT.EditValue = Article.EndDT.CurrentValue

			' ExpireDT
			Article.ExpireDT.EditCustomAttributes = ""
			Article.ExpireDT.EditValue = Article.ExpireDT.CurrentValue

			' Author
			Article.Author.EditCustomAttributes = ""
			sFilterWrk = ""
			sSqlWrk = "SELECT [PrimaryContact], [PrimaryContact], '' AS Disp2Fld, [CompanyID] FROM [Contact]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [PrimaryContact] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", Language.Phrase("PleaseSelect"), ""}) 
			Article.Author.EditValue = arwrk

			' Counter
			Article.Counter.EditCustomAttributes = ""
			Article.Counter.CurrentValue = 0

			' VersionNo
			Article.VersionNo.EditCustomAttributes = ""
			Article.VersionNo.CurrentValue = 0

			' ContactID
			Article.ContactID.EditCustomAttributes = ""
			Article.ContactID.CurrentValue = 0

			' ModifiedDT
		End If

		' Row Rendered event
		If Article.RowType <> EW_ROWTYPE_AGGREGATEINIT Then
			Article.Row_Rendered()
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
		If ew_Empty(Article.Title.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError &= Language.Phrase("EnterRequiredField") & " - " & Article.Title.FldCaption
		End If
		If Not ew_CheckUSDate(Article.EndDT.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError &= Article.EndDT.FldErrMsg
		End If
		If Not ew_CheckUSDate(Article.ExpireDT.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError &= Article.ExpireDT.FldErrMsg
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

			' Active
			Article.Active.SetDbValue(Rs, (Article.Active.CurrentValue <> "" AndAlso Not IsDBNull(Article.Active.CurrentValue)), System.DBNull.Value, True)

			' StartDT
			Article.StartDT.DbValue = ew_CurrentDate()
			Rs("StartDT") = Article.StartDT.DbValue

			' Title
			Article.Title.SetDbValue(Rs, Article.Title.CurrentValue, "", False)

			' CompanyID
			Article.CompanyID.SetDbValue(Rs, Article.CompanyID.CurrentValue, System.DBNull.Value, True)

			' PageID
			Article.zPageID.SetDbValue(Rs, Article.zPageID.CurrentValue, System.DBNull.Value, True)

			' userID
			Article.userID.SetDbValue(Rs, Article.userID.CurrentValue, System.DBNull.Value, True)

			' Description
			Article.Description.SetDbValue(Rs, Article.Description.CurrentValue, System.DBNull.Value, False)

			' ArticleSummary
			Article.ArticleSummary.SetDbValue(Rs, Article.ArticleSummary.CurrentValue, System.DBNull.Value, False)

			' ArticleBody
			Article.ArticleBody.SetDbValue(Rs, Article.ArticleBody.CurrentValue, System.DBNull.Value, False)

			' EndDT
			Article.EndDT.SetDbValue(Rs, Article.EndDT.CurrentValue, System.DBNull.Value, False)

			' ExpireDT
			Article.ExpireDT.SetDbValue(Rs, Article.ExpireDT.CurrentValue, System.DBNull.Value, False)

			' Author
			Article.Author.SetDbValue(Rs, Article.Author.CurrentValue, System.DBNull.Value, False)

			' Counter
			Article.Counter.SetDbValue(Rs, Article.Counter.CurrentValue, System.DBNull.Value, True)

			' VersionNo
			Article.VersionNo.SetDbValue(Rs, Article.VersionNo.CurrentValue, System.DBNull.Value, True)

			' ContactID
			Article.ContactID.SetDbValue(Rs, Article.ContactID.CurrentValue, System.DBNull.Value, True)

			' ModifiedDT
			Article.ModifiedDT.DbValue = ew_CurrentDateTime()
			Rs("ModifiedDT") = Article.ModifiedDT.DbValue
		Catch e As Exception
			If EW_DEBUG_ENABLED Then Throw
			Message = e.Message
			Return False
		End Try

		' Row Inserting event
		bInsertRow = Article.Row_Inserting(Rs)
		If bInsertRow Then
			Try	
				Article.Insert(Rs)
				AddRow = True
			Catch e As Exception
				If EW_DEBUG_ENABLED Then Throw
				Message = e.Message				
				AddRow = False
			End Try
		Else
			If Article.CancelMessage <> "" Then
				Message = Article.CancelMessage
				Article.CancelMessage = ""
			Else
				Message = Language.Phrase("InsertCancelled")
			End If
			AddRow = False
		End If
		If AddRow Then
			LastInsertId = Conn.GetLastInsertId()
			Article.ArticleID.DbValue = LastInsertId
			Rs("ArticleID") = Article.ArticleID.DbValue		

			' Row Inserted event
			Article.Row_Inserted(Rs)
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
				sDbMasterFilter = Article.SqlMasterFilter_Company
				sDbDetailFilter = Article.SqlDetailFilter_Company
				If ew_Get("CompanyID") <> "" Then
					Company.CompanyID.QueryStringValue = ew_Get("CompanyID")
					Article.CompanyID.QueryStringValue = Company.CompanyID.QueryStringValue
					Article.CompanyID.SessionValue = Article.CompanyID.QueryStringValue
					If Not IsNumeric(Company.CompanyID.QueryStringValue) Then bValidMaster = False
					sDbMasterFilter = sDbMasterFilter.Replace("@CompanyID@", ew_AdjustSql(Company.CompanyID.QueryStringValue))
					sDbDetailFilter = sDbDetailFilter.Replace("@CompanyID@", ew_AdjustSql(Company.CompanyID.QueryStringValue))
				Else
					bValidMaster = False
				End If
			End If
			If sMasterTblVar = "zPage" Then
				bValidMaster = True
				sDbMasterFilter = Article.SqlMasterFilter_zPage
				sDbDetailFilter = Article.SqlDetailFilter_zPage
				If ew_Get("zPageID") <> "" Then
					zPage.zPageID.QueryStringValue = ew_Get("zPageID")
					Article.zPageID.QueryStringValue = zPage.zPageID.QueryStringValue
					Article.zPageID.SessionValue = Article.zPageID.QueryStringValue
					If Not IsNumeric(zPage.zPageID.QueryStringValue) Then bValidMaster = False
					sDbMasterFilter = sDbMasterFilter.Replace("@zPageID@", ew_AdjustSql(zPage.zPageID.QueryStringValue))
					sDbDetailFilter = sDbDetailFilter.Replace("@zPageID@", ew_AdjustSql(zPage.zPageID.QueryStringValue))
				Else
					bValidMaster = False
				End If
			End If
		End If
		If bValidMaster Then

			' Save current master table
			Article.CurrentMasterTable = sMasterTblVar
			Article.MasterFilter = sDbMasterFilter ' Set up master filter
			Article.DetailFilter = sDbDetailFilter ' Set up detail filter

			' Clear previous master session values
			If sMasterTblVar <> "Company" Then
				If Article.CompanyID.QueryStringValue = "" Then Article.CompanyID.SessionValue = ""
			End If
			If sMasterTblVar <> "zPage" Then
				If Article.zPageID.QueryStringValue = "" Then Article.zPageID.SessionValue = ""
			End If
		End If
	End Sub

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "Article"
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
		Dim table As String = "Article"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsSrc("ArticleID")

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

		' Active Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "Active", keyvalue, oldvalue, RsSrc("Active"))

		' StartDT Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "StartDT", keyvalue, oldvalue, RsSrc("StartDT"))

		' Title Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "Title", keyvalue, oldvalue, RsSrc("Title"))

		' CompanyID Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "CompanyID", keyvalue, oldvalue, RsSrc("CompanyID"))

		' PageID Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "PageID", keyvalue, oldvalue, RsSrc("PageID"))

		' userID Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "userID", keyvalue, oldvalue, RsSrc("userID"))

		' Description Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "Description", keyvalue, oldvalue, RsSrc("Description"))

		' ArticleSummary Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "ArticleSummary", keyvalue, oldvalue, "<MEMO>")

		' ArticleBody Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "ArticleBody", keyvalue, oldvalue, "<MEMO>")

		' EndDT Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "EndDT", keyvalue, oldvalue, RsSrc("EndDT"))

		' ExpireDT Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "ExpireDT", keyvalue, oldvalue, RsSrc("ExpireDT"))

		' Author Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "Author", keyvalue, oldvalue, RsSrc("Author"))

		' Counter Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "Counter", keyvalue, oldvalue, RsSrc("Counter"))

		' VersionNo Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "VersionNo", keyvalue, oldvalue, RsSrc("VersionNo"))

		' ContactID Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "ContactID", keyvalue, oldvalue, RsSrc("ContactID"))

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
		Article_add = New cArticle_add(Me)		
		Article_add.Page_Init()		

		' Set buffer/cache option
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

		' Page main processing
		Article_add.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If Article_add IsNot Nothing Then Article_add.Dispose()
	End Sub
End Class
