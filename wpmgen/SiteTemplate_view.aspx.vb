Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class SiteTemplate_view
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public SiteTemplate_view As cSiteTemplate_view

	'
	' Page Class
	'
	Class cSiteTemplate_view
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
				If SiteTemplate.UseTokenInUrl Then Url = Url & "t=" & SiteTemplate.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If SiteTemplate.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (SiteTemplate.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (SiteTemplate.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' SiteTemplate
		Public Property SiteTemplate() As cSiteTemplate
			Get				
				Return ParentPage.SiteTemplate
			End Get
			Set(ByVal v As cSiteTemplate)
				ParentPage.SiteTemplate = v	
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
			m_PageID = "view"
			m_PageObjName = "SiteTemplate_view"
			m_PageObjTypeName = "cSiteTemplate_view"

			' Table Name
			m_TableName = "SiteTemplate"

			' Initialize table object
			SiteTemplate = New cSiteTemplate(Me)

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
			SiteTemplate.Dispose()

			' Go to URL if specified
			If url <> "" Then
				HttpContext.Current.Response.Clear()
				HttpContext.Current.Response.Redirect(url)
			End If
		End Sub

	Public lDisplayRecs As Integer ' Number of display records

	Public lStartRec As Integer, lStopRec As Integer, lTotalRecs As Integer, lRecRange As Integer

	Public lRecCnt As Integer

	Public sSrchWhere As String

	'
	' Page main processing
	'
	Sub Page_Main()

		' Paging variables
		lDisplayRecs = 1
		lRecRange = EW_PAGER_RANGE

		' Load current record
		Dim bLoadCurrentRecord As Boolean = False
		Dim sReturnUrl As String = ""
		Dim bMatchRecord As Boolean = False
		If IsPageRequest Then ' Validate request
			If ew_Get("TemplatePrefix") <> "" Then
				SiteTemplate.TemplatePrefix.QueryStringValue = ew_Get("TemplatePrefix")
			Else
				bLoadCurrentRecord = True
			End If

			' Get action
			SiteTemplate.CurrentAction = "I" ' Display form
			Select Case SiteTemplate.CurrentAction
				Case "I" ' Get a record to display
					lStartRec = 1 ' Initialize start position
					Dim Rs As OleDbDataReader = LoadRecordset() ' Load records
					If lTotalRecs <= 0 Then ' No record found
						Message = "No records found" ' Set no record message
						sReturnUrl = "SiteTemplate_list.aspx"
					ElseIf bLoadCurrentRecord Then ' Load current record position
						SetUpStartRec() ' Set up start record position

						' Point to current record
						If lStartRec <= lTotalRecs Then
							bMatchRecord = True
							For i As Integer = 1 to lStartRec
								Rs.Read()
							Next
						End If
					Else ' Match key values
						Do While Rs.Read()
							If ew_SameStr(SiteTemplate.TemplatePrefix.CurrentValue, Rs("TemplatePrefix")) Then
								SiteTemplate.StartRecordNumber = lStartRec ' Save record position
								bMatchRecord = True
								Exit Do
							Else
								lStartRec = lStartRec + 1
							End If
						Loop
					End If
					If Not bMatchRecord Then
						Message = "No records found" ' Set no record message
						sReturnUrl = "SiteTemplate_list.aspx" ' No matching record, return to list
					Else
						LoadRowValues(Rs) ' Load row values
					End If
					If Rs IsNot Nothing Then
						Rs.Close()
						Rs.Dispose()
					End If
			End Select
		Else
			sReturnUrl = "SiteTemplate_list.aspx" ' Not page request, return to list
		End If
		If sReturnUrl <> "" Then Page_Terminate(sReturnUrl)

		' Render row
		SiteTemplate.RowType = EW_ROWTYPE_VIEW
		RenderRow()
	End Sub

	Public Pager As Object

	'
	' Set up Starting Record parameters
	'
	Sub SetUpStartRec()
		Dim nPageNo As Integer

		' Exit if lDisplayRecs = 0
		If lDisplayRecs = 0 Then Exit Sub
		If IsPageRequest Then ' Validate request

			' Check for a "start" parameter
			If ew_Get(EW_TABLE_START_REC) <> "" AndAlso IsNumeric(ew_Get(EW_TABLE_START_REC)) Then
				lStartRec = ew_ConvertToInt(ew_Get(EW_TABLE_START_REC))
				SiteTemplate.StartRecordNumber = lStartRec
			ElseIf ew_Get(EW_TABLE_PAGE_NO) <> "" AndAlso IsNumeric(ew_Get(EW_TABLE_PAGE_NO)) Then
				nPageNo = ew_ConvertToInt(ew_Get(EW_TABLE_PAGE_NO))
				lStartRec = (nPageNo-1)*lDisplayRecs+1
				If lStartRec <= 0 Then
					lStartRec = 1
				ElseIf lStartRec >= ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 Then
					lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1
				End If
				SiteTemplate.StartRecordNumber = lStartRec
			End If
		End If
		lStartRec = SiteTemplate.StartRecordNumber

		' Check if correct start record counter
		If lStartRec <= 0 Then ' Avoid invalid start record counter
			lStartRec = 1 ' Reset start record counter
			SiteTemplate.StartRecordNumber = lStartRec
		ElseIf lStartRec > lTotalRecs Then ' Avoid starting record > total records
			lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to last page first record
			SiteTemplate.StartRecordNumber = lStartRec
		ElseIf (lStartRec-1) Mod lDisplayRecs <> 0 Then
			lStartRec = ((lStartRec-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to page boundary
			SiteTemplate.StartRecordNumber = lStartRec
		End If
	End Sub

	'
	' Load default values
	'
	Sub LoadDefaultValues()
	End Sub

	'
	' Load recordset
	'
	Function LoadRecordset() As OleDbDataReader

		' Recordset Selecting event
		SiteTemplate.Recordset_Selecting(SiteTemplate.CurrentFilter)

		' Load list page SQL
		Dim sSql As String = SiteTemplate.ListSQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)

		' Count
		lTotalRecs = -1		
		Try			
			If sSql.StartsWith("SELECT * FROM ", StringComparison.InvariantCultureIgnoreCase) AndAlso _				
				ew_Empty(SiteTemplate.SqlGroupBy) AndAlso _
				ew_Empty(SiteTemplate.SqlHaving) Then
				Dim sCntSql As String = SiteTemplate.SelectCountSQL

				' Write SQL for debug
				If EW_DEBUG_ENABLED Then ew_Write("<br>" & sCntSql)
				lTotalRecs = Conn.ExecuteScalar(sCntSql)
			End If			
		Catch
		End Try

		' Load recordset
		Dim Rs As OleDbDataReader = Conn.GetDataReader(sSql)
		If lTotalRecs < 0 AndAlso Rs.HasRows Then
			lTotalRecs = 0
			While Rs.Read()
				lTotalRecs = lTotalRecs + 1
			End While
			Rs.Close()		
			Rs = Conn.GetDataReader(sSql)
		End If

		' Recordset Selected event
		SiteTemplate.Recordset_Selected(Rs)
		Return Rs
	End Function

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = SiteTemplate.KeyFilter

		' Row Selecting event
		SiteTemplate.Row_Selecting(sFilter)

		' Load SQL based on filter
		SiteTemplate.CurrentFilter = sFilter
		Dim sSql As String = SiteTemplate.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				SiteTemplate.Row_Selected(RsRow)
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
		SiteTemplate.TemplatePrefix.DbValue = RsRow("TemplatePrefix")
		SiteTemplate.zName.DbValue = RsRow("Name")
		SiteTemplate.Top.DbValue = RsRow("Top")
		SiteTemplate.Bottom.DbValue = RsRow("Bottom")
		SiteTemplate.CSS.DbValue = RsRow("CSS")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		SiteTemplate.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' TemplatePrefix

		SiteTemplate.TemplatePrefix.CellCssStyle = ""
		SiteTemplate.TemplatePrefix.CellCssClass = ""

		' Name
		SiteTemplate.zName.CellCssStyle = ""
		SiteTemplate.zName.CellCssClass = ""

		' Top
		SiteTemplate.Top.CellCssStyle = ""
		SiteTemplate.Top.CellCssClass = ""

		' Bottom
		SiteTemplate.Bottom.CellCssStyle = ""
		SiteTemplate.Bottom.CellCssClass = ""

		'
		'  View  Row
		'

		If SiteTemplate.RowType = EW_ROWTYPE_VIEW Then ' View row

			' TemplatePrefix
			SiteTemplate.TemplatePrefix.ViewValue = SiteTemplate.TemplatePrefix.CurrentValue
			SiteTemplate.TemplatePrefix.CssStyle = ""
			SiteTemplate.TemplatePrefix.CssClass = ""
			SiteTemplate.TemplatePrefix.ViewCustomAttributes = ""

			' Name
			SiteTemplate.zName.ViewValue = SiteTemplate.zName.CurrentValue
			SiteTemplate.zName.CssStyle = ""
			SiteTemplate.zName.CssClass = ""
			SiteTemplate.zName.ViewCustomAttributes = ""

			' Top
			SiteTemplate.Top.ViewValue = SiteTemplate.Top.CurrentValue
			SiteTemplate.Top.CssStyle = ""
			SiteTemplate.Top.CssClass = ""
			SiteTemplate.Top.ViewCustomAttributes = ""

			' Bottom
			SiteTemplate.Bottom.ViewValue = SiteTemplate.Bottom.CurrentValue
			SiteTemplate.Bottom.CssStyle = ""
			SiteTemplate.Bottom.CssClass = ""
			SiteTemplate.Bottom.ViewCustomAttributes = ""

			' View refer script
			' TemplatePrefix

			SiteTemplate.TemplatePrefix.HrefValue = ""

			' Name
			SiteTemplate.zName.HrefValue = ""

			' Top
			SiteTemplate.Top.HrefValue = ""

			' Bottom
			SiteTemplate.Bottom.HrefValue = ""
		End If

		' Row Rendered event
		SiteTemplate.Row_Rendered()
	End Sub

		' Page Load event
		Public Sub Page_Load()

			'HttpContext.Current.Response.Write("Page Load")
		End Sub

		' Page Unload event
		Public Sub Page_Unload()

			'HttpContext.Current.Response.Write("Page Unload")
		End Sub
	End Class

	'
	' ASP.NET Page_Load event
	'

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

		' Page init
		SiteTemplate_view = New cSiteTemplate_view(Me)		
		SiteTemplate_view.Page_Init()

		' Page main processing
		SiteTemplate_view.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If SiteTemplate_view IsNot Nothing Then SiteTemplate_view.Dispose()
	End Sub
End Class
