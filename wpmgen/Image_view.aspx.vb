Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class Image_view
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public Image_view As cImage_view

	'
	' Page Class
	'
	Class cImage_view
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
				If Image.UseTokenInUrl Then Url = Url & "t=" & Image.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If Image.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (Image.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (Image.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' Image
		Public Property Image() As cImage
			Get				
				Return ParentPage.Image
			End Get
			Set(ByVal v As cImage)
				ParentPage.Image = v	
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
			m_PageObjName = "Image_view"
			m_PageObjTypeName = "cImage_view"

			' Table Name
			m_TableName = "Image"

			' Initialize table object
			Image = New cImage(Me)

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
			Image.Dispose()

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
			If ew_Get("ImageID") <> "" Then
				Image.ImageID.QueryStringValue = ew_Get("ImageID")
			Else
				bLoadCurrentRecord = True
			End If

			' Get action
			Image.CurrentAction = "I" ' Display form
			Select Case Image.CurrentAction
				Case "I" ' Get a record to display
					lStartRec = 1 ' Initialize start position
					Dim Rs As OleDbDataReader = LoadRecordset() ' Load records
					If lTotalRecs <= 0 Then ' No record found
						Message = "No records found" ' Set no record message
						sReturnUrl = "Image_list.aspx"
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
							If ew_SameStr(Image.ImageID.CurrentValue, Rs("ImageID")) Then
								Image.StartRecordNumber = lStartRec ' Save record position
								bMatchRecord = True
								Exit Do
							Else
								lStartRec = lStartRec + 1
							End If
						Loop
					End If
					If Not bMatchRecord Then
						Message = "No records found" ' Set no record message
						sReturnUrl = "Image_list.aspx" ' No matching record, return to list
					Else
						LoadRowValues(Rs) ' Load row values
					End If
					If Rs IsNot Nothing Then
						Rs.Close()
						Rs.Dispose()
					End If
			End Select
		Else
			sReturnUrl = "Image_list.aspx" ' Not page request, return to list
		End If
		If sReturnUrl <> "" Then Page_Terminate(sReturnUrl)

		' Render row
		Image.RowType = EW_ROWTYPE_VIEW
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
				Image.StartRecordNumber = lStartRec
			ElseIf ew_Get(EW_TABLE_PAGE_NO) <> "" AndAlso IsNumeric(ew_Get(EW_TABLE_PAGE_NO)) Then
				nPageNo = ew_ConvertToInt(ew_Get(EW_TABLE_PAGE_NO))
				lStartRec = (nPageNo-1)*lDisplayRecs+1
				If lStartRec <= 0 Then
					lStartRec = 1
				ElseIf lStartRec >= ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 Then
					lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1
				End If
				Image.StartRecordNumber = lStartRec
			End If
		End If
		lStartRec = Image.StartRecordNumber

		' Check if correct start record counter
		If lStartRec <= 0 Then ' Avoid invalid start record counter
			lStartRec = 1 ' Reset start record counter
			Image.StartRecordNumber = lStartRec
		ElseIf lStartRec > lTotalRecs Then ' Avoid starting record > total records
			lStartRec = ((lTotalRecs-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to last page first record
			Image.StartRecordNumber = lStartRec
		ElseIf (lStartRec-1) Mod lDisplayRecs <> 0 Then
			lStartRec = ((lStartRec-1)\lDisplayRecs)*lDisplayRecs+1 ' Point to page boundary
			Image.StartRecordNumber = lStartRec
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
		Image.Recordset_Selecting(Image.CurrentFilter)

		' Load list page SQL
		Dim sSql As String = Image.ListSQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)

		' Count
		lTotalRecs = -1		
		Try			
			If sSql.StartsWith("SELECT * FROM ", StringComparison.InvariantCultureIgnoreCase) AndAlso _				
				ew_Empty(Image.SqlGroupBy) AndAlso _
				ew_Empty(Image.SqlHaving) Then
				Dim sCntSql As String = Image.SelectCountSQL

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
		Image.Recordset_Selected(Rs)
		Return Rs
	End Function

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = Image.KeyFilter

		' Row Selecting event
		Image.Row_Selecting(sFilter)

		' Load SQL based on filter
		Image.CurrentFilter = sFilter
		Dim sSql As String = Image.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				Image.Row_Selected(RsRow)
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
		Image.ImageID.DbValue = RsRow("ImageID")
		Image.CompanyID.DbValue = RsRow("CompanyID")
		Image.title.DbValue = RsRow("title")
		Image.ImageName.DbValue = RsRow("ImageName")
		Image.ImageDescription.DbValue = RsRow("ImageDescription")
		Image.ImageComment.DbValue = RsRow("ImageComment")
		Image.ImageFileName.DbValue = RsRow("ImageFileName")
		Image.ImageThumbFileName.DbValue = RsRow("ImageThumbFileName")
		Image.ImageDate.DbValue = RsRow("ImageDate")
		Image.Active.DbValue = IIf(ew_ConvertToBool(RsRow("Active")), "1", "0")
		Image.ModifiedDT.DbValue = RsRow("ModifiedDT")
		Image.VersionNo.DbValue = RsRow("VersionNo")
		Image.ContactID.DbValue = RsRow("ContactID")
		Image.medium.DbValue = RsRow("medium")
		Image.size.DbValue = RsRow("size")
		Image.price.DbValue = RsRow("price")
		Image.color.DbValue = RsRow("color")
		Image.subject.DbValue = RsRow("subject")
		Image.sold.DbValue = IIf(ew_ConvertToBool(RsRow("sold")), "1", "0")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		Image.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' CompanyID

		Image.CompanyID.CellCssStyle = ""
		Image.CompanyID.CellCssClass = ""

		' title
		Image.title.CellCssStyle = ""
		Image.title.CellCssClass = ""

		' ImageName
		Image.ImageName.CellCssStyle = ""
		Image.ImageName.CellCssClass = ""

		' ImageDescription
		Image.ImageDescription.CellCssStyle = ""
		Image.ImageDescription.CellCssClass = ""

		' ImageComment
		Image.ImageComment.CellCssStyle = ""
		Image.ImageComment.CellCssClass = ""

		' ImageFileName
		Image.ImageFileName.CellCssStyle = ""
		Image.ImageFileName.CellCssClass = ""

		' ImageThumbFileName
		Image.ImageThumbFileName.CellCssStyle = ""
		Image.ImageThumbFileName.CellCssClass = ""

		' ImageDate
		Image.ImageDate.CellCssStyle = ""
		Image.ImageDate.CellCssClass = ""

		' Active
		Image.Active.CellCssStyle = ""
		Image.Active.CellCssClass = ""

		' ModifiedDT
		Image.ModifiedDT.CellCssStyle = ""
		Image.ModifiedDT.CellCssClass = ""

		' VersionNo
		Image.VersionNo.CellCssStyle = ""
		Image.VersionNo.CellCssClass = ""

		' ContactID
		Image.ContactID.CellCssStyle = ""
		Image.ContactID.CellCssClass = ""

		' medium
		Image.medium.CellCssStyle = ""
		Image.medium.CellCssClass = ""

		' size
		Image.size.CellCssStyle = ""
		Image.size.CellCssClass = ""

		' price
		Image.price.CellCssStyle = ""
		Image.price.CellCssClass = ""

		' color
		Image.color.CellCssStyle = ""
		Image.color.CellCssClass = ""

		' subject
		Image.subject.CellCssStyle = ""
		Image.subject.CellCssClass = ""

		' sold
		Image.sold.CellCssStyle = ""
		Image.sold.CellCssClass = ""

		'
		'  View  Row
		'

		If Image.RowType = EW_ROWTYPE_VIEW Then ' View row

			' CompanyID
			If ew_NotEmpty(Image.CompanyID.CurrentValue) Then
				sSqlWrk = "SELECT [CompanyName] FROM [Company] WHERE [CompanyID] = " & ew_AdjustSql(Image.CompanyID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] "
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Image.CompanyID.ViewValue = RsWrk("CompanyName")
				Else
					Image.CompanyID.ViewValue = Image.CompanyID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Image.CompanyID.ViewValue = System.DBNull.Value
			End If
			Image.CompanyID.CssStyle = ""
			Image.CompanyID.CssClass = ""
			Image.CompanyID.ViewCustomAttributes = ""

			' title
			Image.title.ViewValue = Image.title.CurrentValue
			Image.title.CssStyle = ""
			Image.title.CssClass = ""
			Image.title.ViewCustomAttributes = ""

			' ImageName
			Image.ImageName.ViewValue = Image.ImageName.CurrentValue
			Image.ImageName.CssStyle = ""
			Image.ImageName.CssClass = ""
			Image.ImageName.ViewCustomAttributes = ""

			' ImageDescription
			Image.ImageDescription.ViewValue = Image.ImageDescription.CurrentValue
			Image.ImageDescription.CssStyle = ""
			Image.ImageDescription.CssClass = ""
			Image.ImageDescription.ViewCustomAttributes = ""

			' ImageComment
			Image.ImageComment.ViewValue = Image.ImageComment.CurrentValue
			Image.ImageComment.CssStyle = ""
			Image.ImageComment.CssClass = ""
			Image.ImageComment.ViewCustomAttributes = ""

			' ImageFileName
			Image.ImageFileName.ViewValue = Image.ImageFileName.CurrentValue
			Image.ImageFileName.CssStyle = ""
			Image.ImageFileName.CssClass = ""
			Image.ImageFileName.ViewCustomAttributes = ""

			' ImageThumbFileName
			Image.ImageThumbFileName.ViewValue = Image.ImageThumbFileName.CurrentValue
			Image.ImageThumbFileName.CssStyle = ""
			Image.ImageThumbFileName.CssClass = ""
			Image.ImageThumbFileName.ViewCustomAttributes = ""

			' ImageDate
			Image.ImageDate.ViewValue = Image.ImageDate.CurrentValue
			Image.ImageDate.ViewValue = ew_FormatDateTime(Image.ImageDate.ViewValue, 6)
			Image.ImageDate.CssStyle = ""
			Image.ImageDate.CssClass = ""
			Image.ImageDate.ViewCustomAttributes = ""

			' Active
			If Convert.ToString(Image.Active.CurrentValue) = "1" Then
				Image.Active.ViewValue = "Yes"
			Else
				Image.Active.ViewValue = "No"
			End If
			Image.Active.CssStyle = ""
			Image.Active.CssClass = ""
			Image.Active.ViewCustomAttributes = ""

			' ModifiedDT
			Image.ModifiedDT.ViewValue = Image.ModifiedDT.CurrentValue
			Image.ModifiedDT.ViewValue = ew_FormatDateTime(Image.ModifiedDT.ViewValue, 6)
			Image.ModifiedDT.CssStyle = ""
			Image.ModifiedDT.CssClass = ""
			Image.ModifiedDT.ViewCustomAttributes = ""

			' VersionNo
			Image.VersionNo.ViewValue = Image.VersionNo.CurrentValue
			Image.VersionNo.CssStyle = ""
			Image.VersionNo.CssClass = ""
			Image.VersionNo.ViewCustomAttributes = ""

			' ContactID
			If ew_NotEmpty(Image.ContactID.CurrentValue) Then
				sSqlWrk = "SELECT [PrimaryContact] FROM [Contact] WHERE [ContactID] = " & ew_AdjustSql(Image.ContactID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [PrimaryContact] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Image.ContactID.ViewValue = RsWrk("PrimaryContact")
				Else
					Image.ContactID.ViewValue = Image.ContactID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Image.ContactID.ViewValue = System.DBNull.Value
			End If
			Image.ContactID.CssStyle = ""
			Image.ContactID.CssClass = ""
			Image.ContactID.ViewCustomAttributes = ""

			' medium
			Image.medium.ViewValue = Image.medium.CurrentValue
			Image.medium.CssStyle = ""
			Image.medium.CssClass = ""
			Image.medium.ViewCustomAttributes = ""

			' size
			Image.size.ViewValue = Image.size.CurrentValue
			Image.size.CssStyle = ""
			Image.size.CssClass = ""
			Image.size.ViewCustomAttributes = ""

			' price
			Image.price.ViewValue = Image.price.CurrentValue
			Image.price.CssStyle = ""
			Image.price.CssClass = ""
			Image.price.ViewCustomAttributes = ""

			' color
			Image.color.ViewValue = Image.color.CurrentValue
			Image.color.CssStyle = ""
			Image.color.CssClass = ""
			Image.color.ViewCustomAttributes = ""

			' subject
			Image.subject.ViewValue = Image.subject.CurrentValue
			Image.subject.CssStyle = ""
			Image.subject.CssClass = ""
			Image.subject.ViewCustomAttributes = ""

			' sold
			If Convert.ToString(Image.sold.CurrentValue) = "1" Then
				Image.sold.ViewValue = "Yes"
			Else
				Image.sold.ViewValue = "No"
			End If
			Image.sold.CssStyle = ""
			Image.sold.CssClass = ""
			Image.sold.ViewCustomAttributes = ""

			' View refer script
			' CompanyID

			Image.CompanyID.HrefValue = ""

			' title
			Image.title.HrefValue = ""

			' ImageName
			Image.ImageName.HrefValue = ""

			' ImageDescription
			Image.ImageDescription.HrefValue = ""

			' ImageComment
			Image.ImageComment.HrefValue = ""

			' ImageFileName
			Image.ImageFileName.HrefValue = ""

			' ImageThumbFileName
			Image.ImageThumbFileName.HrefValue = ""

			' ImageDate
			Image.ImageDate.HrefValue = ""

			' Active
			Image.Active.HrefValue = ""

			' ModifiedDT
			Image.ModifiedDT.HrefValue = ""

			' VersionNo
			Image.VersionNo.HrefValue = ""

			' ContactID
			Image.ContactID.HrefValue = ""

			' medium
			Image.medium.HrefValue = ""

			' size
			Image.size.HrefValue = ""

			' price
			Image.price.HrefValue = ""

			' color
			Image.color.HrefValue = ""

			' subject
			Image.subject.HrefValue = ""

			' sold
			Image.sold.HrefValue = ""
		End If

		' Row Rendered event
		Image.Row_Rendered()
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
		Image_view = New cImage_view(Me)		
		Image_view.Page_Init()

		' Page main processing
		Image_view.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If Image_view IsNot Nothing Then Image_view.Dispose()
	End Sub
End Class
