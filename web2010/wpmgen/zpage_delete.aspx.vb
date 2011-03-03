Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class zpage_delete
	Inherits AspNetMaker8_wpmWebsite

	' Page object
	Public zPage_delete As czPage_delete

	'
	' Page Class
	'
	Class czPage_delete
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
				If zPage.UseTokenInUrl Then Url = Url & "t=" & zPage.TableVar & "&" ' Add page token
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
			If zPage.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (zPage.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (zPage.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' ASP.NET page object
		Public ReadOnly Property AspNetPage() As zpage_delete
			Get
				Return CType(m_ParentPage, zpage_delete)
			End Get
		End Property

		' zPage
		Public Property zPage() As czPage
			Get				
				Return ParentPage.zPage
			End Get
			Set(ByVal v As czPage)
				ParentPage.zPage = v	
			End Set	
		End Property

		' zPage
		Public Property Company() As cCompany
			Get				
				Return ParentPage.Company
			End Get
			Set(ByVal v As cCompany)
				ParentPage.Company = v	
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
			m_PageID = "delete"
			m_PageObjName = "zPage_delete"
			m_PageObjTypeName = "czPage_delete"			

			' Initialize language object
			Language = New cLanguage(Me)

			' Table Name
			m_TableName = "Page"

			' Initialize table object
			zPage = New czPage(Me)
			Company = New cCompany(Me)

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
			zPage.Dispose()
			Company.Dispose()
			ObjForm = Nothing

			' Go to URL if specified
			Dim sRedirectUrl As String = url
			Page_Redirecting(sRedirectUrl)
			If sRedirectUrl <> "" Then
				HttpContext.Current.Response.Clear()
				HttpContext.Current.Response.Redirect(sReDirectUrl)
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
		If ew_Get("zPageID") <> "" Then
			zPage.zPageID.QueryStringValue = ew_Get("zPageID")
			If Not IsNumeric(zPage.zPageID.QueryStringValue) Then
			Page_Terminate("zpage_list.aspx") ' Prevent SQL injection, return to list
			End If
			sKey = sKey & zPage.zPageID.QueryStringValue
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
		If nKeySelected <= 0 Then Page_Terminate("zpage_list.aspx") ' No key specified, return to list

		' Build filter
		For i As Integer = 0 to arRecKeys.GetUpperBound(0)
			sKey = arRecKeys(i).Trim()
			sFilter = sFilter & "("

			' Set up key field
			sKeyFld = sKey
			If Not IsNumeric(sKeyFld) Then Page_Terminate("zpage_list.aspx") ' Prevent SQL injection, return to list
			sFilter &= "[PageID]=" & ew_AdjustSql(sKeyFld) & " AND "
			If sFilter.EndsWith(" AND ") Then sFilter = sFilter.Substring(0, sFilter.Length-5) & ") OR "
		Next
		If sFilter.EndsWith(" OR ") Then sFilter = sFilter.Substring(0, sFilter.Length-4)

		' Set up filter (SQL WHERE clause)
		' SQL constructor in zPage class, zPageinfo.aspx

		zPage.CurrentFilter = sFilter

		' Get action
		If ew_Post("a_delete") <> "" Then
			zPage.CurrentAction = ew_Post("a_delete")
		Else
			zPage.CurrentAction = "I"	' Display record
		End If
		Select Case zPage.CurrentAction
			Case "D" ' Delete
				zPage.SendEmail = True ' Send email on delete success
				If DeleteRows() Then ' delete rows
					Message = Language.Phrase("DeleteSuccess") ' Set up success message
					Page_Terminate(zPage.ReturnUrl) ' Return to caller
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
		sWrkFilter = zPage.CurrentFilter

		' Set up filter (SQL WHERE Clause)
		' SQL constructor in zPage class, zPageinfo.aspx

		zPage.CurrentFilter = sWrkFilter
		sSql = zPage.SQL
		Conn.BeginTrans() ' Begin transaction
		Try
			RsDelete = Conn.GetDataReader(sSql)
			If Not RsDelete.HasRows Then
				Message = Language.Phrase("NoRecord") ' No record found
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
				DeleteRows = zPage.Row_Deleting(Row)
				If Not DeleteRows Then Exit For
			Next Row
		End If
		If DeleteRows Then
			sKey = ""
			For Each Row As OrderedDictionary in RsOld
				sThisKey = ""
				If sThisKey <> "" Then sThisKey = sThisKey & EW_COMPOSITE_KEY_SEPARATOR
				sThisKey = sThisKey & Convert.ToString(Row("PageID"))
				Try
					zPage.Delete(Row)
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
			If zPage.CancelMessage <> "" Then
				Message = zPage.CancelMessage
				zPage.CancelMessage = ""
			Else
				Message = Language.Phrase("DeleteCancelled")
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
				zPage.Row_Deleted(Row)
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
		zPage.Recordset_Selecting(zPage.CurrentFilter)

		' Load list page SQL
		Dim sSql As String = zPage.ListSQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then DebugMsg = sSql

		' Count
		lTotalRecs = -1		
		Try
			Dim sCntSql As String = zPage.SelectCountSQL

			' Write SQL for debug
			If EW_DEBUG_ENABLED Then DebugMsg = sCntSql
			lTotalRecs = Conn.ExecuteScalar(sCntSql)
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
		zPage.Recordset_Selected(Rs)
		Return Rs
	End Function

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = zPage.KeyFilter

		' Row Selecting event
		zPage.Row_Selecting(sFilter)

		' Load SQL based on filter
		zPage.CurrentFilter = sFilter
		Dim sSql As String = zPage.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then DebugMsg = sSql
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				zPage.Row_Selected(RsRow)
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
		zPage.zPageID.DbValue = RsRow("PageID")
		zPage.ParentPageID.DbValue = RsRow("ParentPageID")
		zPage.zPageName.DbValue = RsRow("PageName")
		zPage.PageTitle.DbValue = RsRow("PageTitle")
		zPage.PageTypeID.DbValue = RsRow("PageTypeID")
		zPage.GroupID.DbValue = RsRow("GroupID")
		zPage.Active.DbValue = IIf(ew_ConvertToBool(RsRow("Active")), "1", "0")
		zPage.PageOrder.DbValue = RsRow("PageOrder")
		zPage.CompanyID.DbValue = RsRow("CompanyID")
		zPage.PageDescription.DbValue = RsRow("PageDescription")
		zPage.PageKeywords.DbValue = RsRow("PageKeywords")
		zPage.ImagesPerRow.DbValue = RsRow("ImagesPerRow")
		zPage.RowsPerPage.DbValue = RsRow("RowsPerPage")
		zPage.PageFileName.DbValue = RsRow("PageFileName")
		zPage.AllowMessage.DbValue = IIf(ew_ConvertToBool(RsRow("AllowMessage")), "1", "0")
		zPage.SiteCategoryID.DbValue = RsRow("SiteCategoryID")
		zPage.SiteCategoryGroupID.DbValue = RsRow("SiteCategoryGroupID")
		zPage.VersionNo.DbValue = RsRow("VersionNo")
		zPage.ModifiedDT.DbValue = RsRow("ModifiedDT")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Initialize urls
		' Row Rendering event

		zPage.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' ParentPageID

		zPage.ParentPageID.CellCssStyle = "white-space: nowrap;"
		zPage.ParentPageID.CellCssClass = ""
		zPage.ParentPageID.CellAttrs.Clear(): zPage.ParentPageID.ViewAttrs.Clear(): zPage.ParentPageID.EditAttrs.Clear()

		' PageName
		zPage.zPageName.CellCssStyle = "white-space: nowrap;"
		zPage.zPageName.CellCssClass = ""
		zPage.zPageName.CellAttrs.Clear(): zPage.zPageName.ViewAttrs.Clear(): zPage.zPageName.EditAttrs.Clear()

		' PageTitle
		zPage.PageTitle.CellCssStyle = "white-space: nowrap;"
		zPage.PageTitle.CellCssClass = ""
		zPage.PageTitle.CellAttrs.Clear(): zPage.PageTitle.ViewAttrs.Clear(): zPage.PageTitle.EditAttrs.Clear()

		' PageTypeID
		zPage.PageTypeID.CellCssStyle = "white-space: nowrap;"
		zPage.PageTypeID.CellCssClass = ""
		zPage.PageTypeID.CellAttrs.Clear(): zPage.PageTypeID.ViewAttrs.Clear(): zPage.PageTypeID.EditAttrs.Clear()

		' GroupID
		zPage.GroupID.CellCssStyle = "white-space: nowrap;"
		zPage.GroupID.CellCssClass = ""
		zPage.GroupID.CellAttrs.Clear(): zPage.GroupID.ViewAttrs.Clear(): zPage.GroupID.EditAttrs.Clear()

		' Active
		zPage.Active.CellCssStyle = "white-space: nowrap;"
		zPage.Active.CellCssClass = ""
		zPage.Active.CellAttrs.Clear(): zPage.Active.ViewAttrs.Clear(): zPage.Active.EditAttrs.Clear()

		' PageOrder
		zPage.PageOrder.CellCssStyle = "white-space: nowrap;"
		zPage.PageOrder.CellCssClass = ""
		zPage.PageOrder.CellAttrs.Clear(): zPage.PageOrder.ViewAttrs.Clear(): zPage.PageOrder.EditAttrs.Clear()

		'
		'  View  Row
		'

		If zPage.RowType = EW_ROWTYPE_VIEW Then ' View row

			' ParentPageID
			If ew_NotEmpty(zPage.ParentPageID.CurrentValue) Then
				sFilterWrk = "[PageID] = " & ew_AdjustSql(zPage.ParentPageID.CurrentValue) & ""
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
					zPage.ParentPageID.ViewValue = RsWrk("PageName")
				Else
					zPage.ParentPageID.ViewValue = zPage.ParentPageID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				zPage.ParentPageID.ViewValue = System.DBNull.Value
			End If
			zPage.ParentPageID.CssStyle = ""
			zPage.ParentPageID.CssClass = ""
			zPage.ParentPageID.ViewCustomAttributes = ""

			' PageName
			zPage.zPageName.ViewValue = zPage.zPageName.CurrentValue
			zPage.zPageName.CssStyle = ""
			zPage.zPageName.CssClass = ""
			zPage.zPageName.ViewCustomAttributes = ""

			' PageTitle
			zPage.PageTitle.ViewValue = zPage.PageTitle.CurrentValue
			zPage.PageTitle.CssStyle = ""
			zPage.PageTitle.CssClass = ""
			zPage.PageTitle.ViewCustomAttributes = ""

			' PageTypeID
			If ew_NotEmpty(zPage.PageTypeID.CurrentValue) Then
				sFilterWrk = "[PageTypeID] = " & ew_AdjustSql(zPage.PageTypeID.CurrentValue) & ""
			sSqlWrk = "SELECT [PageTypeCD] FROM [PageType]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [PageTypeCD] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					zPage.PageTypeID.ViewValue = RsWrk("PageTypeCD")
				Else
					zPage.PageTypeID.ViewValue = zPage.PageTypeID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				zPage.PageTypeID.ViewValue = System.DBNull.Value
			End If
			zPage.PageTypeID.CssStyle = ""
			zPage.PageTypeID.CssClass = ""
			zPage.PageTypeID.ViewCustomAttributes = ""

			' GroupID
			If ew_NotEmpty(zPage.GroupID.CurrentValue) Then
				sFilterWrk = "[GroupID] = " & ew_AdjustSql(zPage.GroupID.CurrentValue) & ""
			sSqlWrk = "SELECT [GroupName] FROM [Group]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [GroupName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					zPage.GroupID.ViewValue = RsWrk("GroupName")
				Else
					zPage.GroupID.ViewValue = zPage.GroupID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				zPage.GroupID.ViewValue = System.DBNull.Value
			End If
			zPage.GroupID.CssStyle = ""
			zPage.GroupID.CssClass = ""
			zPage.GroupID.ViewCustomAttributes = ""

			' Active
			If Convert.ToString(zPage.Active.CurrentValue) = "1" Then
				zPage.Active.ViewValue = "Yes"
			Else
				zPage.Active.ViewValue = "No"
			End If
			zPage.Active.CssStyle = ""
			zPage.Active.CssClass = ""
			zPage.Active.ViewCustomAttributes = ""

			' PageOrder
			zPage.PageOrder.ViewValue = zPage.PageOrder.CurrentValue
			zPage.PageOrder.CssStyle = ""
			zPage.PageOrder.CssClass = ""
			zPage.PageOrder.ViewCustomAttributes = ""

			' CompanyID
			If ew_NotEmpty(zPage.CompanyID.CurrentValue) Then
				sFilterWrk = "[CompanyID] = " & ew_AdjustSql(zPage.CompanyID.CurrentValue) & ""
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
					zPage.CompanyID.ViewValue = RsWrk("CompanyName")
				Else
					zPage.CompanyID.ViewValue = zPage.CompanyID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				zPage.CompanyID.ViewValue = System.DBNull.Value
			End If
			zPage.CompanyID.CssStyle = ""
			zPage.CompanyID.CssClass = ""
			zPage.CompanyID.ViewCustomAttributes = ""

			' PageDescription
			zPage.PageDescription.ViewValue = zPage.PageDescription.CurrentValue
			zPage.PageDescription.CssStyle = ""
			zPage.PageDescription.CssClass = ""
			zPage.PageDescription.ViewCustomAttributes = ""

			' PageKeywords
			zPage.PageKeywords.ViewValue = zPage.PageKeywords.CurrentValue
			zPage.PageKeywords.CssStyle = ""
			zPage.PageKeywords.CssClass = ""
			zPage.PageKeywords.ViewCustomAttributes = ""

			' ImagesPerRow
			zPage.ImagesPerRow.ViewValue = zPage.ImagesPerRow.CurrentValue
			zPage.ImagesPerRow.CssStyle = ""
			zPage.ImagesPerRow.CssClass = ""
			zPage.ImagesPerRow.ViewCustomAttributes = ""

			' RowsPerPage
			zPage.RowsPerPage.ViewValue = zPage.RowsPerPage.CurrentValue
			zPage.RowsPerPage.CssStyle = ""
			zPage.RowsPerPage.CssClass = ""
			zPage.RowsPerPage.ViewCustomAttributes = ""

			' PageFileName
			zPage.PageFileName.ViewValue = zPage.PageFileName.CurrentValue
			zPage.PageFileName.CssStyle = ""
			zPage.PageFileName.CssClass = ""
			zPage.PageFileName.ViewCustomAttributes = ""

			' AllowMessage
			If Convert.ToString(zPage.AllowMessage.CurrentValue) = "1" Then
				zPage.AllowMessage.ViewValue = "Yes"
			Else
				zPage.AllowMessage.ViewValue = "No"
			End If
			zPage.AllowMessage.CssStyle = ""
			zPage.AllowMessage.CssClass = ""
			zPage.AllowMessage.ViewCustomAttributes = ""

			' SiteCategoryID
			zPage.SiteCategoryID.ViewValue = zPage.SiteCategoryID.CurrentValue
			zPage.SiteCategoryID.CssStyle = ""
			zPage.SiteCategoryID.CssClass = ""
			zPage.SiteCategoryID.ViewCustomAttributes = ""

			' SiteCategoryGroupID
			zPage.SiteCategoryGroupID.ViewValue = zPage.SiteCategoryGroupID.CurrentValue
			zPage.SiteCategoryGroupID.CssStyle = ""
			zPage.SiteCategoryGroupID.CssClass = ""
			zPage.SiteCategoryGroupID.ViewCustomAttributes = ""

			' VersionNo
			zPage.VersionNo.ViewValue = zPage.VersionNo.CurrentValue
			zPage.VersionNo.CssStyle = ""
			zPage.VersionNo.CssClass = ""
			zPage.VersionNo.ViewCustomAttributes = ""

			' ModifiedDT
			zPage.ModifiedDT.ViewValue = zPage.ModifiedDT.CurrentValue
			zPage.ModifiedDT.CssStyle = ""
			zPage.ModifiedDT.CssClass = ""
			zPage.ModifiedDT.ViewCustomAttributes = ""

			' View refer script
			' ParentPageID

			zPage.ParentPageID.HrefValue = ""
			zPage.ParentPageID.TooltipValue = ""

			' PageName
			zPage.zPageName.HrefValue = ""
			zPage.zPageName.TooltipValue = ""

			' PageTitle
			zPage.PageTitle.HrefValue = ""
			zPage.PageTitle.TooltipValue = ""

			' PageTypeID
			zPage.PageTypeID.HrefValue = ""
			zPage.PageTypeID.TooltipValue = ""

			' GroupID
			zPage.GroupID.HrefValue = ""
			zPage.GroupID.TooltipValue = ""

			' Active
			zPage.Active.HrefValue = ""
			zPage.Active.TooltipValue = ""

			' PageOrder
			zPage.PageOrder.HrefValue = ""
			zPage.PageOrder.TooltipValue = ""
		End If

		' Row Rendered event
		If zPage.RowType <> EW_ROWTYPE_AGGREGATEINIT Then
			zPage.Row_Rendered()
		End If
	End Sub

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "Page"
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

	' Write Audit Trail (delete page)
	Sub WriteAuditTrailOnDelete(ByRef RsSrc As OrderedDictionary)
		Dim table As String = "Page"
		Dim filePfx As String = "log"
		Dim action As String = "D"
		Dim newvalue As Object = ""
		Dim curDateTime As String, id As String, user As String, field As String
		Dim keyvalue As Object, oldvalue As Object
		Dim dt As DateTime = Now()
		curDateTime = dt.ToString("yyyy/MM/dd HH:mm:ss")
		id = HttpContext.Current.Request.ServerVariables("SCRIPT_NAME")
    user = CurrentUserName()

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsSrc("PageID")
		keyvalue = sKey

		' PageID Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "PageID", keyvalue, RsSrc("PageID"), newvalue)

		' ParentPageID Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "ParentPageID", keyvalue, RsSrc("ParentPageID"), newvalue)

		' PageName Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "PageName", keyvalue, RsSrc("PageName"), newvalue)

		' PageTitle Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "PageTitle", keyvalue, RsSrc("PageTitle"), newvalue)

		' PageTypeID Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "PageTypeID", keyvalue, RsSrc("PageTypeID"), newvalue)

		' GroupID Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "GroupID", keyvalue, RsSrc("GroupID"), newvalue)

		' Active Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "Active", keyvalue, RsSrc("Active"), newvalue)

		' PageOrder Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "PageOrder", keyvalue, RsSrc("PageOrder"), newvalue)

		' CompanyID Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "CompanyID", keyvalue, RsSrc("CompanyID"), newvalue)

		' PageDescription Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "PageDescription", keyvalue, RsSrc("PageDescription"), newvalue)

		' PageKeywords Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "PageKeywords", keyvalue, RsSrc("PageKeywords"), newvalue)

		' ImagesPerRow Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "ImagesPerRow", keyvalue, RsSrc("ImagesPerRow"), newvalue)

		' RowsPerPage Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "RowsPerPage", keyvalue, RsSrc("RowsPerPage"), newvalue)

		' PageFileName Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "PageFileName", keyvalue, RsSrc("PageFileName"), newvalue)

		' AllowMessage Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "AllowMessage", keyvalue, RsSrc("AllowMessage"), newvalue)

		' SiteCategoryID Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "SiteCategoryID", keyvalue, RsSrc("SiteCategoryID"), newvalue)

		' SiteCategoryGroupID Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "SiteCategoryGroupID", keyvalue, RsSrc("SiteCategoryGroupID"), newvalue)

		' VersionNo Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "VersionNo", keyvalue, RsSrc("VersionNo"), newvalue)

		' ModifiedDT Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "ModifiedDT", keyvalue, RsSrc("ModifiedDT"), newvalue)
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
	End Class

	'
	' ASP.NET Page_Load event
	'

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

		' Page init
		zPage_delete = New czPage_delete(Me)		
		zPage_delete.Page_Init()		

		' Set buffer/cache option
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

		' Page main processing
		zPage_delete.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If zPage_delete IsNot Nothing Then zPage_delete.Dispose()
	End Sub
End Class
