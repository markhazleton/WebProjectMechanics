Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class Article_add
	Inherits AspNetMaker7_WPMGen

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

		' Article
		Public Property Article() As cArticle
			Get				
				Return ParentPage.Article
			End Get
			Set(ByVal v As cArticle)
				ParentPage.Article = v	
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
			m_PageObjName = "Article_add"
			m_PageObjTypeName = "cArticle_add"

			' Table Name
			m_TableName = "Article"

			' Initialize table object
			Article = New cArticle(Me)

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
			Article.Dispose()

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
		If ew_Get("ArticleID") <> "" Then
			Article.ArticleID.QueryStringValue = ew_Get("ArticleID")
		Else
			bCopy = False
		End If

		' Create form object
		ObjForm = New cFormObj

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
					Message = "No records found" ' No record found
					Page_Terminate("Article_list.aspx") ' No matching record, return to list
				End If
			Case "A" ' Add new record
				Article.SendEmail = True ' Send email on add success
				If AddRow() Then ' Add successful
					Message = "Add succeeded" ' Set up success message
					Dim sReturnUrl As String = Article.ReturnUrl
					If ew_GetPageName(sReturnUrl) = "Article_view.aspx" Then sReturnUrl = Article.ViewUrl ' View paging, return to view page with keyurl directly
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
		Article.StartDT.CurrentValue = ew_CurrentDate()
		Article.CompanyID.CurrentValue = 0
		Article.zPageID.CurrentValue = 0
		Article.ContactID.CurrentValue = HttpContext.Current.Session("ContactID")
		Article.ModifiedDT.CurrentValue = ew_CurrentDate()
		Article.EndDT.CurrentValue = ew_CurrentDate()
		Article.ExpireDT.CurrentValue = ew_CurrentDate()
	End Sub

	'
	' Load form values
	'
	Sub LoadFormValues()
		Article.Active.FormValue = ObjForm.GetValue("x_Active")
		Article.Active.OldValue = ObjForm.GetValue("o_Active")
		Article.StartDT.FormValue = ObjForm.GetValue("x_StartDT")
		Article.StartDT.CurrentValue = ew_UnFormatDateTime(Article.StartDT.CurrentValue, 6)
		Article.StartDT.OldValue = ObjForm.GetValue("o_StartDT")
		Article.Title.FormValue = ObjForm.GetValue("x_Title")
		Article.Title.OldValue = ObjForm.GetValue("o_Title")
		Article.CompanyID.FormValue = ObjForm.GetValue("x_CompanyID")
		Article.CompanyID.OldValue = ObjForm.GetValue("o_CompanyID")
		Article.zPageID.FormValue = ObjForm.GetValue("x_zPageID")
		Article.zPageID.OldValue = ObjForm.GetValue("o_zPageID")
		Article.ContactID.FormValue = ObjForm.GetValue("x_ContactID")
		Article.ContactID.OldValue = ObjForm.GetValue("o_ContactID")
		Article.Description.FormValue = ObjForm.GetValue("x_Description")
		Article.Description.OldValue = ObjForm.GetValue("o_Description")
		Article.ArticleBody.FormValue = ObjForm.GetValue("x_ArticleBody")
		Article.ArticleBody.OldValue = ObjForm.GetValue("o_ArticleBody")
		Article.ModifiedDT.FormValue = ObjForm.GetValue("x_ModifiedDT")
		Article.ModifiedDT.CurrentValue = ew_UnFormatDateTime(Article.ModifiedDT.CurrentValue, 6)
		Article.ModifiedDT.OldValue = ObjForm.GetValue("o_ModifiedDT")
		Article.EndDT.FormValue = ObjForm.GetValue("x_EndDT")
		Article.EndDT.CurrentValue = ew_UnFormatDateTime(Article.EndDT.CurrentValue, 6)
		Article.EndDT.OldValue = ObjForm.GetValue("o_EndDT")
		Article.ExpireDT.FormValue = ObjForm.GetValue("x_ExpireDT")
		Article.ExpireDT.CurrentValue = ew_UnFormatDateTime(Article.ExpireDT.CurrentValue, 6)
		Article.ExpireDT.OldValue = ObjForm.GetValue("o_ExpireDT")
		Article.ArticleSummary.FormValue = ObjForm.GetValue("x_ArticleSummary")
		Article.ArticleSummary.OldValue = ObjForm.GetValue("o_ArticleSummary")
		Article.ArticleID.FormValue = ObjForm.GetValue("x_ArticleID")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		Article.Active.CurrentValue = Article.Active.FormValue
		Article.StartDT.CurrentValue = Article.StartDT.FormValue
		Article.StartDT.CurrentValue = ew_UnFormatDateTime(Article.StartDT.CurrentValue, 6)
		Article.Title.CurrentValue = Article.Title.FormValue
		Article.CompanyID.CurrentValue = Article.CompanyID.FormValue
		Article.zPageID.CurrentValue = Article.zPageID.FormValue
		Article.ContactID.CurrentValue = Article.ContactID.FormValue
		Article.Description.CurrentValue = Article.Description.FormValue
		Article.ArticleBody.CurrentValue = Article.ArticleBody.FormValue
		Article.ModifiedDT.CurrentValue = Article.ModifiedDT.FormValue
		Article.ModifiedDT.CurrentValue = ew_UnFormatDateTime(Article.ModifiedDT.CurrentValue, 6)
		Article.EndDT.CurrentValue = Article.EndDT.FormValue
		Article.EndDT.CurrentValue = ew_UnFormatDateTime(Article.EndDT.CurrentValue, 6)
		Article.ExpireDT.CurrentValue = Article.ExpireDT.FormValue
		Article.ExpireDT.CurrentValue = ew_UnFormatDateTime(Article.ExpireDT.CurrentValue, 6)
		Article.ArticleSummary.CurrentValue = Article.ArticleSummary.FormValue
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
		If EW_DEBUG_ENABLED Then ew_Write(sSql)
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
		Article.ArticleID.DbValue = RsRow("ArticleID")
		Article.Active.DbValue = IIf(ew_ConvertToBool(RsRow("Active")), "1", "0")
		Article.StartDT.DbValue = RsRow("StartDT")
		Article.Title.DbValue = RsRow("Title")
		Article.CompanyID.DbValue = RsRow("CompanyID")
		Article.zPageID.DbValue = RsRow("PageID")
		Article.ContactID.DbValue = RsRow("ContactID")
		Article.Description.DbValue = RsRow("Description")
		Article.ArticleBody.DbValue = RsRow("ArticleBody")
		Article.ModifiedDT.DbValue = RsRow("ModifiedDT")
		Article.Counter.DbValue = RsRow("Counter")
		Article.VersionNo.DbValue = RsRow("VersionNo")
		Article.userID.DbValue = RsRow("userID")
		Article.EndDT.DbValue = RsRow("EndDT")
		Article.ExpireDT.DbValue = RsRow("ExpireDT")
		Article.Author.DbValue = RsRow("Author")
		Article.ArticleSummary.DbValue = RsRow("ArticleSummary")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		Article.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' Active

		Article.Active.CellCssStyle = ""
		Article.Active.CellCssClass = ""

		' StartDT
		Article.StartDT.CellCssStyle = ""
		Article.StartDT.CellCssClass = ""

		' Title
		Article.Title.CellCssStyle = ""
		Article.Title.CellCssClass = ""

		' CompanyID
		Article.CompanyID.CellCssStyle = ""
		Article.CompanyID.CellCssClass = ""

		' PageID
		Article.zPageID.CellCssStyle = ""
		Article.zPageID.CellCssClass = ""

		' ContactID
		Article.ContactID.CellCssStyle = ""
		Article.ContactID.CellCssClass = ""

		' Description
		Article.Description.CellCssStyle = ""
		Article.Description.CellCssClass = ""

		' ArticleBody
		Article.ArticleBody.CellCssStyle = ""
		Article.ArticleBody.CellCssClass = ""

		' ModifiedDT
		Article.ModifiedDT.CellCssStyle = ""
		Article.ModifiedDT.CellCssClass = ""

		' EndDT
		Article.EndDT.CellCssStyle = ""
		Article.EndDT.CellCssClass = ""

		' ExpireDT
		Article.ExpireDT.CellCssStyle = ""
		Article.ExpireDT.CellCssClass = ""

		' ArticleSummary
		Article.ArticleSummary.CellCssStyle = ""
		Article.ArticleSummary.CellCssClass = ""

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
			Article.StartDT.ViewValue = ew_FormatDateTime(Article.StartDT.ViewValue, 6)
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
				sSqlWrk = "SELECT [CompanyName] FROM [Company] WHERE [CompanyID] = " & ew_AdjustSql(Article.CompanyID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " AND (" & "[CompanyID]=" & httpcontext.current.session("CompanyID") & " " & ")"
				sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
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
				sSqlWrk = "SELECT [PageName] FROM [Page] WHERE [PageID] = " & ew_AdjustSql(Article.zPageID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [PageName] Asc"
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

			' ContactID
			If ew_NotEmpty(Article.ContactID.CurrentValue) Then
				sSqlWrk = "SELECT [PrimaryContact] FROM [Contact] WHERE [ContactID] = " & ew_AdjustSql(Article.ContactID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [PrimaryContact] "
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Article.ContactID.ViewValue = RsWrk("PrimaryContact")
				Else
					Article.ContactID.ViewValue = Article.ContactID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Article.ContactID.ViewValue = System.DBNull.Value
			End If
			Article.ContactID.CssStyle = ""
			Article.ContactID.CssClass = ""
			Article.ContactID.ViewCustomAttributes = ""

			' Description
			Article.Description.ViewValue = Article.Description.CurrentValue
			Article.Description.CssStyle = ""
			Article.Description.CssClass = ""
			Article.Description.ViewCustomAttributes = ""

			' ArticleBody
			Article.ArticleBody.ViewValue = Article.ArticleBody.CurrentValue
			Article.ArticleBody.CssStyle = ""
			Article.ArticleBody.CssClass = ""
			Article.ArticleBody.ViewCustomAttributes = ""

			' ModifiedDT
			Article.ModifiedDT.ViewValue = Article.ModifiedDT.CurrentValue
			Article.ModifiedDT.ViewValue = ew_FormatDateTime(Article.ModifiedDT.ViewValue, 6)
			Article.ModifiedDT.CssStyle = ""
			Article.ModifiedDT.CssClass = ""
			Article.ModifiedDT.ViewCustomAttributes = ""

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

			' userID
			Article.userID.ViewValue = Article.userID.CurrentValue
			Article.userID.CssStyle = ""
			Article.userID.CssClass = ""
			Article.userID.ViewCustomAttributes = ""

			' EndDT
			Article.EndDT.ViewValue = Article.EndDT.CurrentValue
			Article.EndDT.ViewValue = ew_FormatDateTime(Article.EndDT.ViewValue, 6)
			Article.EndDT.CssStyle = ""
			Article.EndDT.CssClass = ""
			Article.EndDT.ViewCustomAttributes = ""

			' ExpireDT
			Article.ExpireDT.ViewValue = Article.ExpireDT.CurrentValue
			Article.ExpireDT.ViewValue = ew_FormatDateTime(Article.ExpireDT.ViewValue, 6)
			Article.ExpireDT.CssStyle = ""
			Article.ExpireDT.CssClass = ""
			Article.ExpireDT.ViewCustomAttributes = ""

			' Author
			Article.Author.ViewValue = Article.Author.CurrentValue
			Article.Author.CssStyle = ""
			Article.Author.CssClass = ""
			Article.Author.ViewCustomAttributes = ""

			' ArticleSummary
			Article.ArticleSummary.ViewValue = Article.ArticleSummary.CurrentValue
			Article.ArticleSummary.CssStyle = ""
			Article.ArticleSummary.CssClass = ""
			Article.ArticleSummary.ViewCustomAttributes = ""

			' View refer script
			' Active

			Article.Active.HrefValue = ""

			' StartDT
			Article.StartDT.HrefValue = ""

			' Title
			Article.Title.HrefValue = ""

			' CompanyID
			Article.CompanyID.HrefValue = ""

			' PageID
			Article.zPageID.HrefValue = ""

			' ContactID
			Article.ContactID.HrefValue = ""

			' Description
			Article.Description.HrefValue = ""

			' ArticleBody
			Article.ArticleBody.HrefValue = ""

			' ModifiedDT
			Article.ModifiedDT.HrefValue = ""

			' EndDT
			Article.EndDT.HrefValue = ""

			' ExpireDT
			Article.ExpireDT.HrefValue = ""

			' ArticleSummary
			Article.ArticleSummary.HrefValue = ""

		'
		'  Add Row
		'

		ElseIf Article.RowType = EW_ROWTYPE_ADD Then ' Add row

			' Active
			Article.Active.EditCustomAttributes = ""

			' StartDT
			Article.StartDT.EditCustomAttributes = ""
			Article.StartDT.EditValue = ew_FormatDateTime(Article.StartDT.CurrentValue, 6)

			' Title
			Article.Title.EditCustomAttributes = ""
			Article.Title.EditValue = ew_HtmlEncode(Article.Title.CurrentValue)

			' CompanyID
			Article.CompanyID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [CompanyID], [CompanyName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Company]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sWhereWrk = "(" & sWhereWrk & ") AND "
			sWhereWrk = sWhereWrk & "(" & "[CompanyID]=" & httpcontext.current.session("CompanyID") & " " & ")"
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			Article.CompanyID.EditValue = arwrk

			' PageID
			Article.zPageID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [PageID], [PageName], '' AS Disp2Fld, [CompanyID] FROM [Page]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [PageName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select", ""}) 
			Article.zPageID.EditValue = arwrk

			' ContactID
			Article.ContactID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [ContactID], [PrimaryContact], '' AS Disp2Fld, [CompanyID] FROM [Contact]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [PrimaryContact] "
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select", ""}) 
			Article.ContactID.EditValue = arwrk

			' Description
			Article.Description.EditCustomAttributes = ""
			Article.Description.EditValue = ew_HtmlEncode(Article.Description.CurrentValue)

			' ArticleBody
			Article.ArticleBody.EditCustomAttributes = ""
			Article.ArticleBody.EditValue = ew_HtmlEncode(Article.ArticleBody.CurrentValue)

			' ModifiedDT
			' EndDT

			Article.EndDT.EditCustomAttributes = ""
			Article.EndDT.EditValue = ew_FormatDateTime(Article.EndDT.CurrentValue, 6)

			' ExpireDT
			Article.ExpireDT.EditCustomAttributes = ""
			Article.ExpireDT.EditValue = ew_FormatDateTime(Article.ExpireDT.CurrentValue, 6)

			' ArticleSummary
			Article.ArticleSummary.EditCustomAttributes = ""
			Article.ArticleSummary.EditValue = ew_HtmlEncode(Article.ArticleSummary.CurrentValue)
		End If

		' Row Rendered event
		Article.Row_Rendered()
	End Sub

	'
	' Validate form
	'
	Function ValidateForm() As Boolean

		' Initialize
		ParentPage.gsFormError = ""

		' Check if validation required
		If Not EW_SERVER_VALIDATE Then Return (ParentPage.gsFormError = "")
		If ew_Empty(Article.StartDT.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter required field - Start DT"
		End If
		If Not ew_CheckUSDate(Article.StartDT.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Incorrect date, format = mm/dd/yyyy - Start DT"
		End If
		If ew_Empty(Article.CompanyID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter required field - Site"
		End If
		If Not ew_CheckUSDate(Article.EndDT.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Incorrect date, format = mm/dd/yyyy - End DT"
		End If
		If Not ew_CheckUSDate(Article.ExpireDT.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Incorrect date, format = mm/dd/yyyy - Expire DT"
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

		' Active
		Article.Active.SetDbValue((Article.Active.CurrentValue <> "" And Not IsDBNull(Article.Active.CurrentValue)), System.DBNull.Value)
		Rs("Active") = Article.Active.DbValue

		' StartDT
		Article.StartDT.SetDbValue(ew_UnFormatDateTime(Article.StartDT.CurrentValue, 6), System.DBNull.Value)
		Rs("StartDT") = Article.StartDT.DbValue

		' Title
		Article.Title.SetDbValue(Article.Title.CurrentValue, System.DBNull.Value)
		Rs("Title") = Article.Title.DbValue

		' CompanyID
		Article.CompanyID.SetDbValue(Article.CompanyID.CurrentValue, System.DBNull.Value)
		Rs("CompanyID") = Article.CompanyID.DbValue

		' PageID
		Article.zPageID.SetDbValue(Article.zPageID.CurrentValue, System.DBNull.Value)
		Rs("PageID") = Article.zPageID.DbValue

		' ContactID
		Article.ContactID.SetDbValue(Article.ContactID.CurrentValue, System.DBNull.Value)
		Rs("ContactID") = Article.ContactID.DbValue

		' Description
		Article.Description.SetDbValue(Article.Description.CurrentValue, System.DBNull.Value)
		Rs("Description") = Article.Description.DbValue

		' ArticleBody
		Article.ArticleBody.SetDbValue(Article.ArticleBody.CurrentValue, System.DBNull.Value)
		Rs("ArticleBody") = Article.ArticleBody.DbValue

		' ModifiedDT
		Article.ModifiedDT.DbValue = ew_CurrentDate()
		Rs("ModifiedDT") = Article.ModifiedDT.DbValue

		' EndDT
		Article.EndDT.SetDbValue(ew_UnFormatDateTime(Article.EndDT.CurrentValue, 6), System.DBNull.Value)
		Rs("EndDT") = Article.EndDT.DbValue

		' ExpireDT
		Article.ExpireDT.SetDbValue(ew_UnFormatDateTime(Article.ExpireDT.CurrentValue, 6), System.DBNull.Value)
		Rs("ExpireDT") = Article.ExpireDT.DbValue

		' ArticleSummary
		Article.ArticleSummary.SetDbValue(Article.ArticleSummary.CurrentValue, System.DBNull.Value)
		Rs("ArticleSummary") = Article.ArticleSummary.DbValue

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
				Message = "Insert cancelled"
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

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "Article"
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
		Dim table As String = "Article"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsSrc("ArticleID")

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

		' Active Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "Active", keyvalue, oldvalue, RsSrc("Active"))

		' StartDT Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "StartDT", keyvalue, oldvalue, RsSrc("StartDT"))

		' Title Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "Title", keyvalue, oldvalue, RsSrc("Title"))

		' CompanyID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "CompanyID", keyvalue, oldvalue, RsSrc("CompanyID"))

		' PageID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "PageID", keyvalue, oldvalue, RsSrc("PageID"))

		' ContactID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "ContactID", keyvalue, oldvalue, RsSrc("ContactID"))

		' Description Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "Description", keyvalue, oldvalue, RsSrc("Description"))

		' ArticleBody Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "ArticleBody", keyvalue, oldvalue, "<MEMO>")

		' ModifiedDT Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "ModifiedDT", keyvalue, oldvalue, RsSrc("ModifiedDT"))

		' EndDT Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "EndDT", keyvalue, oldvalue, RsSrc("EndDT"))

		' ExpireDT Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "ExpireDT", keyvalue, oldvalue, RsSrc("ExpireDT"))

		' ArticleSummary Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "ArticleSummary", keyvalue, oldvalue, "<MEMO>")
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
		Article_add = New cArticle_add(Me)		
		Article_add.Page_Init()

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
