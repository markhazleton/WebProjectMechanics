Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class SiteLink_delete
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public SiteLink_delete As cSiteLink_delete

	'
	' Page Class
	'
	Class cSiteLink_delete
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
			m_PageID = "delete"
			m_PageObjName = "SiteLink_delete"
			m_PageObjTypeName = "cSiteLink_delete"

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

	Public lTotalRecs As Integer

	Public lRecCnt As Integer

	Public arRecKeys() As String

	'
	' Page main processing
	'
	Sub Page_Main()
		Dim sKey As String = ""
		Dim bSingleDelete As Boolean = True ' Initialize as single delete
		Dim nKeySelected As Integer = 0 ' Initialize selected key count
		Dim sKeyFld As String, arKeyFlds() As String
		Dim sFilter As String		

		' Load Key Parameters
		If ew_Get("ID") <> "" Then
			SiteLink.ID.QueryStringValue = ew_Get("ID")
			If Not IsNumeric(SiteLink.ID.QueryStringValue) Then
			Page_Terminate("SiteLink_list.aspx") ' Prevent SQL injection, return to list
			End If
			sKey = sKey & SiteLink.ID.QueryStringValue
		Else
			bSingleDelete = False
		End If
		If bSingleDelete Then
			nKeySelected = 1 ' Set up key selected count
			Array.Resize(arRecKeys, 1) ' Set up key
			arRecKeys(0) = sKey
		Else
			If HttpContext.Current.Request.Form("key_m") IsNot Nothing Then ' Key in form
				arRecKeys = HttpContext.Current.Request.Form.GetValues("key_m")
				nKeySelected = arRecKeys.Length				
			End If
		End If
		If nKeySelected <= 0 Then Page_Terminate("SiteLink_list.aspx") ' No key specified, return to list

		' Build filter
		For i As Integer = 0 to arRecKeys.GetUpperBound(0)
			sKey = arRecKeys(i).Trim()
			sFilter = sFilter & "("

			' Set up key field
			sKeyFld = sKey
			If Not IsNumeric(sKeyFld) Then Page_Terminate("SiteLink_list.aspx") ' Prevent SQL injection, return to list
			sFilter = sFilter & "[ID]=" & ew_AdjustSql(sKeyFld) & " AND "
			If sFilter.EndsWith(" AND ") Then sFilter = sFilter.Substring(0, sFilter.Length-5) & ") OR "
		Next
		If sFilter.EndsWith(" OR ") Then sFilter = sFilter.Substring(0, sFilter.Length-4)

		' Set up filter (SQL WHERE clause)
		' SQL constructor in SiteLink class, SiteLinkinfo.aspx

		SiteLink.CurrentFilter = sFilter

		' Get action
		If ew_Post("a_delete") <> "" Then
			SiteLink.CurrentAction = ew_Post("a_delete")
		Else
			SiteLink.CurrentAction = "I"	' Display record
		End If
		Select Case SiteLink.CurrentAction
			Case "D" ' Delete
				SiteLink.SendEmail = True ' Send email on delete success
				If DeleteRows() Then ' delete rows
					Message = "Delete succeeded" ' Set up success message
				Page_Terminate(SiteLink.ReturnUrl) ' Return to caller
				End If
		End Select
	End Sub

	'
	'  Function DeleteRows
	'  - Delete Records based on current filter
	'
	Function DeleteRows() As Boolean
		Dim sKey As String, sThisKey As String, sKeyFld As String
		Dim arKeyFlds() As String
		Dim RsDelete As OleDbDataReader = Nothing
		Dim sSql As String, sWrkFilter As String 
		Dim RsOld As ArrayList
		DeleteRows = True
		sWrkFilter = SiteLink.CurrentFilter

		' Set up filter (SQL WHERE Clause)
		' SQL constructor in SiteLink class, SiteLinkinfo.aspx

		SiteLink.CurrentFilter = sWrkFilter
		sSql = SiteLink.SQL
		Conn.BeginTrans() ' Begin transaction
		Try
			RsDelete = Conn.GetDataReader(sSql)
			If Not RsDelete.HasRows Then
				Message = "No records found" ' No record found
				RsDelete.Close()
				RsDelete.Dispose()					
				Return False
			End If
		Catch e As Exception
			If EW_DEBUG_ENABLED Then Throw
			Message = e.Message
			Return False
		End Try

		' Clone old rows
		RsOld = Conn.GetRows(RsDelete)
		RsDelete.Close()
		RsDelete.Dispose()

		' Call Row_Deleting event
		If DeleteRows Then
			For Each Row As OrderedDictionary in RsOld
				DeleteRows = SiteLink.Row_Deleting(Row)
				If Not DeleteRows Then Exit For
			Next Row
		End If
		If DeleteRows Then
			sKey = ""
			For Each Row As OrderedDictionary in RsOld
				sThisKey = ""
				If sThisKey <> "" Then sThisKey = sThisKey & EW_COMPOSITE_KEY_SEPARATOR
				sThisKey = sThisKey & Convert.ToString(Row("ID"))
				Try
					SiteLink.Delete(Row)
				Catch e As Exception
					If EW_DEBUG_ENABLED Then Throw			
					Message = e.Message ' Set up error message
					DeleteRows = False
					Exit For
				End Try
				If sKey <> "" Then sKey = sKey & ", "
				sKey = sKey & sThisKey
			Next Row
		Else

			' Set up error message
			If SiteLink.CancelMessage <> "" Then
				Message = SiteLink.CancelMessage
				SiteLink.CancelMessage = ""
			Else
				Message = "Delete cancelled"
			End If
		End If
		If DeleteRows Then

			' Commit the changes
			Conn.CommitTrans()				

			' Write audit trail	
			For Each Row As OrderedDictionary in RsOld
				WriteAuditTrailOnDelete(Row)
			Next

			' Row deleted event
			For Each Row As OrderedDictionary in RsOld
				SiteLink.Row_Deleted(Row)
			Next
		Else
			Conn.RollbackTrans() ' Rollback transaction			
		End If
	End Function

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
		SiteLink.Recordset_Selecting(SiteLink.CurrentFilter)

		' Load list page SQL
		Dim sSql As String = SiteLink.ListSQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)

		' Count
		lTotalRecs = -1		
		Try			
			If sSql.StartsWith("SELECT * FROM ", StringComparison.InvariantCultureIgnoreCase) AndAlso _				
				ew_Empty(SiteLink.SqlGroupBy) AndAlso _
				ew_Empty(SiteLink.SqlHaving) Then
				Dim sCntSql As String = SiteLink.SelectCountSQL

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
		SiteLink.Recordset_Selected(Rs)
		Return Rs
	End Function

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

			' Ranks
			SiteLink.Ranks.HrefValue = ""

			' Views
			SiteLink.Views.HrefValue = ""
		End If

		' Row Rendered event
		SiteLink.Row_Rendered()
	End Sub

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

	' Write Audit Trail (delete page)
	Sub WriteAuditTrailOnDelete(ByRef RsSrc As OrderedDictionary)
		Dim table As String = "SiteLink"
		Dim filePfx As String = "log"
		Dim action As String = "D"
		Dim newvalue As Object = ""
		Dim curDate As String, curTime As String, id As String, user As String, field As String
		Dim keyvalue As Object, oldvalue As Object
		Dim dt As DateTime = Now()
		curDate = dt.ToString("yyyy/MM/dd")
		curTime = dt.ToString("HH:mm:ss")
		id = HttpContext.Current.Request.ServerVariables("SCRIPT_NAME")
    user = CurrentUserName()

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsSrc("ID")
		keyvalue = sKey

		' ID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "ID", keyvalue, RsSrc("ID"), newvalue)

		' SiteCategoryTypeID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteCategoryTypeID", keyvalue, RsSrc("SiteCategoryTypeID"), newvalue)

		' Title Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "Title", keyvalue, RsSrc("Title"), newvalue)

		' LinkTypeCD Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "LinkTypeCD", keyvalue, RsSrc("LinkTypeCD"), newvalue)

		' CategoryID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "CategoryID", keyvalue, RsSrc("CategoryID"), newvalue)

		' CompanyID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "CompanyID", keyvalue, RsSrc("CompanyID"), newvalue)

		' SiteCategoryID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteCategoryID", keyvalue, RsSrc("SiteCategoryID"), newvalue)

		' SiteCategoryGroupID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteCategoryGroupID", keyvalue, RsSrc("SiteCategoryGroupID"), newvalue)

		' Description Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "Description", keyvalue, "<MEMO>", newvalue)

		' URL Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "URL", keyvalue, "<MEMO>", newvalue)

		' Ranks Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "Ranks", keyvalue, RsSrc("Ranks"), newvalue)

		' Views Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "Views", keyvalue, RsSrc("Views"), newvalue)

		' DateAdd Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "DateAdd", keyvalue, RsSrc("DateAdd"), newvalue)

		' UserName Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "UserName", keyvalue, RsSrc("UserName"), newvalue)

		' UserID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "UserID", keyvalue, RsSrc("UserID"), newvalue)

		' ASIN Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "ASIN", keyvalue, RsSrc("ASIN"), newvalue)
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
		SiteLink_delete = New cSiteLink_delete(Me)		
		SiteLink_delete.Page_Init()

		' Page main processing
		SiteLink_delete.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If SiteLink_delete IsNot Nothing Then SiteLink_delete.Dispose()
	End Sub
End Class
