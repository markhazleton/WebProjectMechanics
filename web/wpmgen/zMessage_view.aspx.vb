Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class zMessage_view
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public zMessage_view As czMessage_view

	'
	' Page Class
	'
	Class czMessage_view
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
				If zMessage.UseTokenInUrl Then Url = Url & "t=" & zMessage.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If zMessage.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (zMessage.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (zMessage.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' zMessage
		Public Property zMessage() As czMessage
			Get				
				Return ParentPage.zMessage
			End Get
			Set(ByVal v As czMessage)
				ParentPage.zMessage = v	
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
			m_PageObjName = "zMessage_view"
			m_PageObjTypeName = "czMessage_view"

			' Table Name
			m_TableName = "Message"

			' Initialize table object
			zMessage = New czMessage(Me)

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
			zMessage.Dispose()

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
			If ew_Get("MessageID") <> "" Then
				zMessage.MessageID.QueryStringValue = ew_Get("MessageID")
			Else
				bLoadCurrentRecord = True
			End If

			' Get action
			zMessage.CurrentAction = "I" ' Display form
			Select Case zMessage.CurrentAction
				Case "I" ' Get a record to display
					lStartRec = 1 ' Initialize start position
					Dim Rs As OleDbDataReader = LoadRecordset() ' Load records
					If lTotalRecs <= 0 Then ' No record found
						Message = "No records found" ' Set no record message
						sReturnUrl = "zMessage_list.aspx"
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
							If ew_SameStr(zMessage.MessageID.CurrentValue, Rs("MessageID")) Then
								zMessage.StartRecordNumber = lStartRec ' Save record position
								bMatchRecord = True
								Exit Do
							Else
								lStartRec = lStartRec + 1
							End If
						Loop
					End If
					If Not bMatchRecord Then
						Message = "No records found" ' Set no record message
						sReturnUrl = "zMessage_list.aspx" ' No matching record, return to list
					Else
						LoadRowValues(Rs) ' Load row values
					End If
					If Rs IsNot Nothing Then
						Rs.Close()
						Rs.Dispose()
					End If
			End Select
		Else
			sReturnUrl = "zMessage_list.aspx" ' Not page request, return to list
		End If
		If sReturnUrl <> "" Then Page_Terminate(sReturnUrl)

		' Render row
		zMessage.RowType = EW_ROWTYPE_VIEW
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
				zMessage.StartRecordNumber = lStartRec
			ElseIf ew_Get(EW_TABLE_PAGE_NO) <> "" AndAlso IsNumeric(ew_Get(EW_TABLE_PAGE_NO)) Then
				nPageNo = ew_ConvertToInt(ew_Get(EW_TABLE_PAGE_NO))
				lStartRec = (nPageNo-1)*lDisplayRecs+1
				If lStartRec <= 0 Then
					lStartRec = 1
				ElseIf lStartRec >= ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 Then
					lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1
				End If
				zMessage.StartRecordNumber = lStartRec
			End If
		End If
		lStartRec = zMessage.StartRecordNumber

		' Check if correct start record counter
		If lStartRec <= 0 Then ' Avoid invalid start record counter
			lStartRec = 1 ' Reset start record counter
			zMessage.StartRecordNumber = lStartRec
		ElseIf lStartRec > lTotalRecs Then ' Avoid starting record > total records
			lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to last page first record
			zMessage.StartRecordNumber = lStartRec
		ElseIf (lStartRec-1) Mod lDisplayRecs <> 0 Then
			lStartRec = ((lStartRec-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to page boundary
			zMessage.StartRecordNumber = lStartRec
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
		zMessage.Recordset_Selecting(zMessage.CurrentFilter)

		' Load list page SQL
		Dim sSql As String = zMessage.ListSQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)

		' Count
		lTotalRecs = -1		
		Try			
			If sSql.StartsWith("SELECT * FROM ", StringComparison.InvariantCultureIgnoreCase) AndAlso _				
				ew_Empty(zMessage.SqlGroupBy) AndAlso _
				ew_Empty(zMessage.SqlHaving) Then
				Dim sCntSql As String = zMessage.SelectCountSQL

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
		zMessage.Recordset_Selected(Rs)
		Return Rs
	End Function

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = zMessage.KeyFilter

		' Row Selecting event
		zMessage.Row_Selecting(sFilter)

		' Load SQL based on filter
		zMessage.CurrentFilter = sFilter
		Dim sSql As String = zMessage.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				zMessage.Row_Selected(RsRow)
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
		zMessage.MessageID.DbValue = RsRow("MessageID")
		zMessage.zPageID.DbValue = RsRow("PageID")
		zMessage.ParentMessageID.DbValue = RsRow("ParentMessageID")
		zMessage.Subject.DbValue = RsRow("Subject")
		zMessage.Author.DbValue = RsRow("Author")
		zMessage.zEmail.DbValue = RsRow("Email")
		zMessage.City.DbValue = RsRow("City")
		zMessage.URL.DbValue = RsRow("URL")
		zMessage.MessageDate.DbValue = RsRow("MessageDate")
		zMessage.Body.DbValue = RsRow("Body")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		zMessage.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' MessageID

		zMessage.MessageID.CellCssStyle = ""
		zMessage.MessageID.CellCssClass = ""

		' PageID
		zMessage.zPageID.CellCssStyle = ""
		zMessage.zPageID.CellCssClass = ""

		' ParentMessageID
		zMessage.ParentMessageID.CellCssStyle = ""
		zMessage.ParentMessageID.CellCssClass = ""

		' Subject
		zMessage.Subject.CellCssStyle = ""
		zMessage.Subject.CellCssClass = ""

		' Author
		zMessage.Author.CellCssStyle = ""
		zMessage.Author.CellCssClass = ""

		' Email
		zMessage.zEmail.CellCssStyle = ""
		zMessage.zEmail.CellCssClass = ""

		' City
		zMessage.City.CellCssStyle = ""
		zMessage.City.CellCssClass = ""

		' URL
		zMessage.URL.CellCssStyle = ""
		zMessage.URL.CellCssClass = ""

		' MessageDate
		zMessage.MessageDate.CellCssStyle = ""
		zMessage.MessageDate.CellCssClass = ""

		' Body
		zMessage.Body.CellCssStyle = ""
		zMessage.Body.CellCssClass = ""

		'
		'  View  Row
		'

		If zMessage.RowType = EW_ROWTYPE_VIEW Then ' View row

			' MessageID
			zMessage.MessageID.ViewValue = zMessage.MessageID.CurrentValue
			zMessage.MessageID.CssStyle = ""
			zMessage.MessageID.CssClass = ""
			zMessage.MessageID.ViewCustomAttributes = ""

			' PageID
			zMessage.zPageID.ViewValue = zMessage.zPageID.CurrentValue
			zMessage.zPageID.CssStyle = ""
			zMessage.zPageID.CssClass = ""
			zMessage.zPageID.ViewCustomAttributes = ""

			' ParentMessageID
			zMessage.ParentMessageID.ViewValue = zMessage.ParentMessageID.CurrentValue
			zMessage.ParentMessageID.CssStyle = ""
			zMessage.ParentMessageID.CssClass = ""
			zMessage.ParentMessageID.ViewCustomAttributes = ""

			' Subject
			zMessage.Subject.ViewValue = zMessage.Subject.CurrentValue
			zMessage.Subject.CssStyle = ""
			zMessage.Subject.CssClass = ""
			zMessage.Subject.ViewCustomAttributes = ""

			' Author
			zMessage.Author.ViewValue = zMessage.Author.CurrentValue
			zMessage.Author.CssStyle = ""
			zMessage.Author.CssClass = ""
			zMessage.Author.ViewCustomAttributes = ""

			' Email
			zMessage.zEmail.ViewValue = zMessage.zEmail.CurrentValue
			zMessage.zEmail.CssStyle = ""
			zMessage.zEmail.CssClass = ""
			zMessage.zEmail.ViewCustomAttributes = ""

			' City
			zMessage.City.ViewValue = zMessage.City.CurrentValue
			zMessage.City.CssStyle = ""
			zMessage.City.CssClass = ""
			zMessage.City.ViewCustomAttributes = ""

			' URL
			zMessage.URL.ViewValue = zMessage.URL.CurrentValue
			zMessage.URL.CssStyle = ""
			zMessage.URL.CssClass = ""
			zMessage.URL.ViewCustomAttributes = ""

			' MessageDate
			zMessage.MessageDate.ViewValue = zMessage.MessageDate.CurrentValue
			zMessage.MessageDate.ViewValue = ew_FormatDateTime(zMessage.MessageDate.ViewValue, 6)
			zMessage.MessageDate.CssStyle = ""
			zMessage.MessageDate.CssClass = ""
			zMessage.MessageDate.ViewCustomAttributes = ""

			' Body
			zMessage.Body.ViewValue = zMessage.Body.CurrentValue
			zMessage.Body.CssStyle = ""
			zMessage.Body.CssClass = ""
			zMessage.Body.ViewCustomAttributes = ""

			' View refer script
			' MessageID

			zMessage.MessageID.HrefValue = ""

			' PageID
			zMessage.zPageID.HrefValue = ""

			' ParentMessageID
			zMessage.ParentMessageID.HrefValue = ""

			' Subject
			zMessage.Subject.HrefValue = ""

			' Author
			zMessage.Author.HrefValue = ""

			' Email
			zMessage.zEmail.HrefValue = ""

			' City
			zMessage.City.HrefValue = ""

			' URL
			zMessage.URL.HrefValue = ""

			' MessageDate
			zMessage.MessageDate.HrefValue = ""

			' Body
			zMessage.Body.HrefValue = ""
		End If

		' Row Rendered event
		zMessage.Row_Rendered()
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
		zMessage_view = New czMessage_view(Me)		
		zMessage_view.Page_Init()

		' Page main processing
		zMessage_view.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If zMessage_view IsNot Nothing Then zMessage_view.Dispose()
	End Sub
End Class
