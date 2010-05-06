Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class CompanySiteTypeParameter_delete
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public CompanySiteTypeParameter_delete As cCompanySiteTypeParameter_delete

	'
	' Page Class
	'
	Class cCompanySiteTypeParameter_delete
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
				If CompanySiteTypeParameter.UseTokenInUrl Then Url = Url & "t=" & CompanySiteTypeParameter.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If CompanySiteTypeParameter.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (CompanySiteTypeParameter.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (CompanySiteTypeParameter.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' CompanySiteTypeParameter
		Public Property CompanySiteTypeParameter() As cCompanySiteTypeParameter
			Get				
				Return ParentPage.CompanySiteTypeParameter
			End Get
			Set(ByVal v As cCompanySiteTypeParameter)
				ParentPage.CompanySiteTypeParameter = v	
			End Set	
		End Property

		' CompanySiteTypeParameter
		Public Property SiteParameterType() As cSiteParameterType
			Get				
				Return ParentPage.SiteParameterType
			End Get
			Set(ByVal v As cSiteParameterType)
				ParentPage.SiteParameterType = v	
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
			m_PageObjName = "CompanySiteTypeParameter_delete"
			m_PageObjTypeName = "cCompanySiteTypeParameter_delete"

			' Table Name
			m_TableName = "CompanySiteTypeParameter"

			' Initialize table object
			CompanySiteTypeParameter = New cCompanySiteTypeParameter(Me)
			SiteParameterType = New cSiteParameterType(Me)

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
			CompanySiteTypeParameter.Dispose()
			SiteParameterType.Dispose()

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
		If ew_Get("CompanySiteTypeParameterID") <> "" Then
			CompanySiteTypeParameter.CompanySiteTypeParameterID.QueryStringValue = ew_Get("CompanySiteTypeParameterID")
			If Not IsNumeric(CompanySiteTypeParameter.CompanySiteTypeParameterID.QueryStringValue) Then
			Page_Terminate("CompanySiteTypeParameter_list.aspx") ' Prevent SQL injection, return to list
			End If
			sKey = sKey & CompanySiteTypeParameter.CompanySiteTypeParameterID.QueryStringValue
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
		If nKeySelected <= 0 Then Page_Terminate("CompanySiteTypeParameter_list.aspx") ' No key specified, return to list

		' Build filter
		For i As Integer = 0 to arRecKeys.GetUpperBound(0)
			sKey = arRecKeys(i).Trim()
			sFilter = sFilter & "("

			' Set up key field
			sKeyFld = sKey
			If Not IsNumeric(sKeyFld) Then Page_Terminate("CompanySiteTypeParameter_list.aspx") ' Prevent SQL injection, return to list
			sFilter = sFilter & "[CompanySiteTypeParameterID]=" & ew_AdjustSql(sKeyFld) & " AND "
			If sFilter.EndsWith(" AND ") Then sFilter = sFilter.Substring(0, sFilter.Length-5) & ") OR "
		Next
		If sFilter.EndsWith(" OR ") Then sFilter = sFilter.Substring(0, sFilter.Length-4)

		' Set up filter (SQL WHERE clause)
		' SQL constructor in CompanySiteTypeParameter class, CompanySiteTypeParameterinfo.aspx

		CompanySiteTypeParameter.CurrentFilter = sFilter

		' Get action
		If ew_Post("a_delete") <> "" Then
			CompanySiteTypeParameter.CurrentAction = ew_Post("a_delete")
		Else
			CompanySiteTypeParameter.CurrentAction = "I"	' Display record
		End If
		Select Case CompanySiteTypeParameter.CurrentAction
			Case "D" ' Delete
				CompanySiteTypeParameter.SendEmail = True ' Send email on delete success
				If DeleteRows() Then ' delete rows
					Message = "Delete succeeded" ' Set up success message
				Page_Terminate(CompanySiteTypeParameter.ReturnUrl) ' Return to caller
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
		sWrkFilter = CompanySiteTypeParameter.CurrentFilter

		' Set up filter (SQL WHERE Clause)
		' SQL constructor in CompanySiteTypeParameter class, CompanySiteTypeParameterinfo.aspx

		CompanySiteTypeParameter.CurrentFilter = sWrkFilter
		sSql = CompanySiteTypeParameter.SQL
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
				DeleteRows = CompanySiteTypeParameter.Row_Deleting(Row)
				If Not DeleteRows Then Exit For
			Next Row
		End If
		If DeleteRows Then
			sKey = ""
			For Each Row As OrderedDictionary in RsOld
				sThisKey = ""
				If sThisKey <> "" Then sThisKey = sThisKey & EW_COMPOSITE_KEY_SEPARATOR
				sThisKey = sThisKey & Convert.ToString(Row("CompanySiteTypeParameterID"))
				Try
					CompanySiteTypeParameter.Delete(Row)
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
			If CompanySiteTypeParameter.CancelMessage <> "" Then
				Message = CompanySiteTypeParameter.CancelMessage
				CompanySiteTypeParameter.CancelMessage = ""
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
				CompanySiteTypeParameter.Row_Deleted(Row)
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
		CompanySiteTypeParameter.Recordset_Selecting(CompanySiteTypeParameter.CurrentFilter)

		' Load list page SQL
		Dim sSql As String = CompanySiteTypeParameter.ListSQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)

		' Count
		lTotalRecs = -1		
		Try			
			If sSql.StartsWith("SELECT * FROM ", StringComparison.InvariantCultureIgnoreCase) AndAlso _				
				ew_Empty(CompanySiteTypeParameter.SqlGroupBy) AndAlso _
				ew_Empty(CompanySiteTypeParameter.SqlHaving) Then
				Dim sCntSql As String = CompanySiteTypeParameter.SelectCountSQL

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
		CompanySiteTypeParameter.Recordset_Selected(Rs)
		Return Rs
	End Function

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = CompanySiteTypeParameter.KeyFilter

		' Row Selecting event
		CompanySiteTypeParameter.Row_Selecting(sFilter)

		' Load SQL based on filter
		CompanySiteTypeParameter.CurrentFilter = sFilter
		Dim sSql As String = CompanySiteTypeParameter.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				CompanySiteTypeParameter.Row_Selected(RsRow)
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
		CompanySiteTypeParameter.CompanySiteTypeParameterID.DbValue = RsRow("CompanySiteTypeParameterID")
		CompanySiteTypeParameter.SiteParameterTypeID.DbValue = RsRow("SiteParameterTypeID")
		CompanySiteTypeParameter.CompanyID.DbValue = RsRow("CompanyID")
		CompanySiteTypeParameter.SiteCategoryTypeID.DbValue = RsRow("SiteCategoryTypeID")
		CompanySiteTypeParameter.SiteCategoryGroupID.DbValue = RsRow("SiteCategoryGroupID")
		CompanySiteTypeParameter.SiteCategoryID.DbValue = RsRow("SiteCategoryID")
		CompanySiteTypeParameter.SortOrder.DbValue = RsRow("SortOrder")
		CompanySiteTypeParameter.ParameterValue.DbValue = RsRow("ParameterValue")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		CompanySiteTypeParameter.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' SiteParameterTypeID

		CompanySiteTypeParameter.SiteParameterTypeID.CellCssStyle = ""
		CompanySiteTypeParameter.SiteParameterTypeID.CellCssClass = ""

		' CompanyID
		CompanySiteTypeParameter.CompanyID.CellCssStyle = ""
		CompanySiteTypeParameter.CompanyID.CellCssClass = ""

		' SiteCategoryTypeID
		CompanySiteTypeParameter.SiteCategoryTypeID.CellCssStyle = ""
		CompanySiteTypeParameter.SiteCategoryTypeID.CellCssClass = ""

		' SiteCategoryGroupID
		CompanySiteTypeParameter.SiteCategoryGroupID.CellCssStyle = ""
		CompanySiteTypeParameter.SiteCategoryGroupID.CellCssClass = ""

		' SiteCategoryID
		CompanySiteTypeParameter.SiteCategoryID.CellCssStyle = ""
		CompanySiteTypeParameter.SiteCategoryID.CellCssClass = ""

		' SortOrder
		CompanySiteTypeParameter.SortOrder.CellCssStyle = ""
		CompanySiteTypeParameter.SortOrder.CellCssClass = ""

		'
		'  View  Row
		'

		If CompanySiteTypeParameter.RowType = EW_ROWTYPE_VIEW Then ' View row

			' SiteParameterTypeID
			If ew_NotEmpty(CompanySiteTypeParameter.SiteParameterTypeID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteParameterTypeNM] FROM [SiteParameterType] WHERE [SiteParameterTypeID] = " & ew_AdjustSql(CompanySiteTypeParameter.SiteParameterTypeID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteParameterTypeNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanySiteTypeParameter.SiteParameterTypeID.ViewValue = RsWrk("SiteParameterTypeNM")
				Else
					CompanySiteTypeParameter.SiteParameterTypeID.ViewValue = CompanySiteTypeParameter.SiteParameterTypeID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanySiteTypeParameter.SiteParameterTypeID.ViewValue = System.DBNull.Value
			End If
			CompanySiteTypeParameter.SiteParameterTypeID.CssStyle = ""
			CompanySiteTypeParameter.SiteParameterTypeID.CssClass = ""
			CompanySiteTypeParameter.SiteParameterTypeID.ViewCustomAttributes = ""

			' CompanyID
			If ew_NotEmpty(CompanySiteTypeParameter.CompanyID.CurrentValue) Then
				sSqlWrk = "SELECT [CompanyName] FROM [Company] WHERE [CompanyID] = " & ew_AdjustSql(CompanySiteTypeParameter.CompanyID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanySiteTypeParameter.CompanyID.ViewValue = RsWrk("CompanyName")
				Else
					CompanySiteTypeParameter.CompanyID.ViewValue = CompanySiteTypeParameter.CompanyID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanySiteTypeParameter.CompanyID.ViewValue = System.DBNull.Value
			End If
			CompanySiteTypeParameter.CompanyID.CssStyle = ""
			CompanySiteTypeParameter.CompanyID.CssClass = ""
			CompanySiteTypeParameter.CompanyID.ViewCustomAttributes = ""

			' SiteCategoryTypeID
			If ew_NotEmpty(CompanySiteTypeParameter.SiteCategoryTypeID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteCategoryTypeNM] FROM [SiteCategoryType] WHERE [SiteCategoryTypeID] = " & ew_AdjustSql(CompanySiteTypeParameter.SiteCategoryTypeID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryTypeNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanySiteTypeParameter.SiteCategoryTypeID.ViewValue = RsWrk("SiteCategoryTypeNM")
				Else
					CompanySiteTypeParameter.SiteCategoryTypeID.ViewValue = CompanySiteTypeParameter.SiteCategoryTypeID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanySiteTypeParameter.SiteCategoryTypeID.ViewValue = System.DBNull.Value
			End If
			CompanySiteTypeParameter.SiteCategoryTypeID.CssStyle = ""
			CompanySiteTypeParameter.SiteCategoryTypeID.CssClass = ""
			CompanySiteTypeParameter.SiteCategoryTypeID.ViewCustomAttributes = ""

			' SiteCategoryGroupID
			If ew_NotEmpty(CompanySiteTypeParameter.SiteCategoryGroupID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteCategoryGroupNM] FROM [SiteCategoryGroup] WHERE [SiteCategoryGroupID] = " & ew_AdjustSql(CompanySiteTypeParameter.SiteCategoryGroupID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryGroupNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanySiteTypeParameter.SiteCategoryGroupID.ViewValue = RsWrk("SiteCategoryGroupNM")
				Else
					CompanySiteTypeParameter.SiteCategoryGroupID.ViewValue = CompanySiteTypeParameter.SiteCategoryGroupID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanySiteTypeParameter.SiteCategoryGroupID.ViewValue = System.DBNull.Value
			End If
			CompanySiteTypeParameter.SiteCategoryGroupID.CssStyle = ""
			CompanySiteTypeParameter.SiteCategoryGroupID.CssClass = ""
			CompanySiteTypeParameter.SiteCategoryGroupID.ViewCustomAttributes = ""

			' SiteCategoryID
			If ew_NotEmpty(CompanySiteTypeParameter.SiteCategoryID.CurrentValue) Then
				sSqlWrk = "SELECT [CategoryName] FROM [SiteCategory] WHERE [SiteCategoryID] = " & ew_AdjustSql(CompanySiteTypeParameter.SiteCategoryID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [CategoryName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanySiteTypeParameter.SiteCategoryID.ViewValue = RsWrk("CategoryName")
				Else
					CompanySiteTypeParameter.SiteCategoryID.ViewValue = CompanySiteTypeParameter.SiteCategoryID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanySiteTypeParameter.SiteCategoryID.ViewValue = System.DBNull.Value
			End If
			CompanySiteTypeParameter.SiteCategoryID.CssStyle = ""
			CompanySiteTypeParameter.SiteCategoryID.CssClass = ""
			CompanySiteTypeParameter.SiteCategoryID.ViewCustomAttributes = ""

			' SortOrder
			CompanySiteTypeParameter.SortOrder.ViewValue = CompanySiteTypeParameter.SortOrder.CurrentValue
			CompanySiteTypeParameter.SortOrder.CssStyle = ""
			CompanySiteTypeParameter.SortOrder.CssClass = ""
			CompanySiteTypeParameter.SortOrder.ViewCustomAttributes = ""

			' View refer script
			' SiteParameterTypeID

			CompanySiteTypeParameter.SiteParameterTypeID.HrefValue = ""

			' CompanyID
			CompanySiteTypeParameter.CompanyID.HrefValue = ""

			' SiteCategoryTypeID
			CompanySiteTypeParameter.SiteCategoryTypeID.HrefValue = ""

			' SiteCategoryGroupID
			CompanySiteTypeParameter.SiteCategoryGroupID.HrefValue = ""

			' SiteCategoryID
			CompanySiteTypeParameter.SiteCategoryID.HrefValue = ""

			' SortOrder
			CompanySiteTypeParameter.SortOrder.HrefValue = ""
		End If

		' Row Rendered event
		CompanySiteTypeParameter.Row_Rendered()
	End Sub

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "CompanySiteTypeParameter"
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
		Dim table As String = "CompanySiteTypeParameter"
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
		sKey = sKey & RsSrc("CompanySiteTypeParameterID")
		keyvalue = sKey

		' CompanySiteTypeParameterID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "CompanySiteTypeParameterID", keyvalue, RsSrc("CompanySiteTypeParameterID"), newvalue)

		' SiteParameterTypeID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteParameterTypeID", keyvalue, RsSrc("SiteParameterTypeID"), newvalue)

		' CompanyID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "CompanyID", keyvalue, RsSrc("CompanyID"), newvalue)

		' SiteCategoryTypeID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteCategoryTypeID", keyvalue, RsSrc("SiteCategoryTypeID"), newvalue)

		' SiteCategoryGroupID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteCategoryGroupID", keyvalue, RsSrc("SiteCategoryGroupID"), newvalue)

		' SiteCategoryID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteCategoryID", keyvalue, RsSrc("SiteCategoryID"), newvalue)

		' SortOrder Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SortOrder", keyvalue, RsSrc("SortOrder"), newvalue)

		' ParameterValue Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "ParameterValue", keyvalue, "<MEMO>", newvalue)
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
		CompanySiteTypeParameter_delete = New cCompanySiteTypeParameter_delete(Me)		
		CompanySiteTypeParameter_delete.Page_Init()

		' Page main processing
		CompanySiteTypeParameter_delete.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If CompanySiteTypeParameter_delete IsNot Nothing Then CompanySiteTypeParameter_delete.Dispose()
	End Sub
End Class
