Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class sitecategory_delete
	Inherits AspNetMaker8_wpmWebsite

	' Page object
	Public SiteCategory_delete As cSiteCategory_delete

	'
	' Page Class
	'
	Class cSiteCategory_delete
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
				If SiteCategory.UseTokenInUrl Then Url = Url & "t=" & SiteCategory.TableVar & "&" ' Add page token
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
			If SiteCategory.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (SiteCategory.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (SiteCategory.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' ASP.NET page object
		Public ReadOnly Property AspNetPage() As sitecategory_delete
			Get
				Return CType(m_ParentPage, sitecategory_delete)
			End Get
		End Property

		' SiteCategory
		Public Property SiteCategory() As cSiteCategory
			Get				
				Return ParentPage.SiteCategory
			End Get
			Set(ByVal v As cSiteCategory)
				ParentPage.SiteCategory = v	
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
			m_PageObjName = "SiteCategory_delete"
			m_PageObjTypeName = "cSiteCategory_delete"			

			' Initialize language object
			Language = New cLanguage(Me)

			' Table Name
			m_TableName = "SiteCategory"

			' Initialize table object
			SiteCategory = New cSiteCategory(Me)

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
			SiteCategory.Dispose()
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
		If ew_Get("SiteCategoryID") <> "" Then
			SiteCategory.SiteCategoryID.QueryStringValue = ew_Get("SiteCategoryID")
			If Not IsNumeric(SiteCategory.SiteCategoryID.QueryStringValue) Then
			Page_Terminate("sitecategory_list.aspx") ' Prevent SQL injection, return to list
			End If
			sKey = sKey & SiteCategory.SiteCategoryID.QueryStringValue
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
		If nKeySelected <= 0 Then Page_Terminate("sitecategory_list.aspx") ' No key specified, return to list

		' Build filter
		For i As Integer = 0 to arRecKeys.GetUpperBound(0)
			sKey = arRecKeys(i).Trim()
			sFilter = sFilter & "("

			' Set up key field
			sKeyFld = sKey
			If Not IsNumeric(sKeyFld) Then Page_Terminate("sitecategory_list.aspx") ' Prevent SQL injection, return to list
			sFilter &= "[SiteCategoryID]=" & ew_AdjustSql(sKeyFld) & " AND "
			If sFilter.EndsWith(" AND ") Then sFilter = sFilter.Substring(0, sFilter.Length-5) & ") OR "
		Next
		If sFilter.EndsWith(" OR ") Then sFilter = sFilter.Substring(0, sFilter.Length-4)

		' Set up filter (SQL WHERE clause)
		' SQL constructor in SiteCategory class, SiteCategoryinfo.aspx

		SiteCategory.CurrentFilter = sFilter

		' Get action
		If ew_Post("a_delete") <> "" Then
			SiteCategory.CurrentAction = ew_Post("a_delete")
		Else
			SiteCategory.CurrentAction = "I"	' Display record
		End If
		Select Case SiteCategory.CurrentAction
			Case "D" ' Delete
				SiteCategory.SendEmail = True ' Send email on delete success
				If DeleteRows() Then ' delete rows
					Message = Language.Phrase("DeleteSuccess") ' Set up success message
					Page_Terminate(SiteCategory.ReturnUrl) ' Return to caller
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
		sWrkFilter = SiteCategory.CurrentFilter

		' Set up filter (SQL WHERE Clause)
		' SQL constructor in SiteCategory class, SiteCategoryinfo.aspx

		SiteCategory.CurrentFilter = sWrkFilter
		sSql = SiteCategory.SQL
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
				DeleteRows = SiteCategory.Row_Deleting(Row)
				If Not DeleteRows Then Exit For
			Next Row
		End If
		If DeleteRows Then
			sKey = ""
			For Each Row As OrderedDictionary in RsOld
				sThisKey = ""
				If sThisKey <> "" Then sThisKey = sThisKey & EW_COMPOSITE_KEY_SEPARATOR
				sThisKey = sThisKey & Convert.ToString(Row("SiteCategoryID"))
				Try
					SiteCategory.Delete(Row)
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
			If SiteCategory.CancelMessage <> "" Then
				Message = SiteCategory.CancelMessage
				SiteCategory.CancelMessage = ""
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
				SiteCategory.Row_Deleted(Row)
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
		SiteCategory.Recordset_Selecting(SiteCategory.CurrentFilter)

		' Load list page SQL
		Dim sSql As String = SiteCategory.ListSQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then DebugMsg = sSql

		' Count
		lTotalRecs = -1		
		Try
			Dim sCntSql As String = SiteCategory.SelectCountSQL

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
		SiteCategory.Recordset_Selected(Rs)
		Return Rs
	End Function

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = SiteCategory.KeyFilter

		' Row Selecting event
		SiteCategory.Row_Selecting(sFilter)

		' Load SQL based on filter
		SiteCategory.CurrentFilter = sFilter
		Dim sSql As String = SiteCategory.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then DebugMsg = sSql
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				SiteCategory.Row_Selected(RsRow)
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
		SiteCategory.SiteCategoryID.DbValue = RsRow("SiteCategoryID")
		SiteCategory.CategoryKeywords.DbValue = RsRow("CategoryKeywords")
		SiteCategory.CategoryName.DbValue = RsRow("CategoryName")
		SiteCategory.CategoryTitle.DbValue = RsRow("CategoryTitle")
		SiteCategory.CategoryDescription.DbValue = RsRow("CategoryDescription")
		SiteCategory.GroupOrder.DbValue = RsRow("GroupOrder")
		SiteCategory.ParentCategoryID.DbValue = RsRow("ParentCategoryID")
		SiteCategory.CategoryFileName.DbValue = RsRow("CategoryFileName")
		SiteCategory.SiteCategoryTypeID.DbValue = RsRow("SiteCategoryTypeID")
		SiteCategory.SiteCategoryGroupID.DbValue = RsRow("SiteCategoryGroupID")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Initialize urls
		' Row Rendering event

		SiteCategory.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' SiteCategoryID

		SiteCategory.SiteCategoryID.CellCssStyle = ""
		SiteCategory.SiteCategoryID.CellCssClass = ""
		SiteCategory.SiteCategoryID.CellAttrs.Clear(): SiteCategory.SiteCategoryID.ViewAttrs.Clear(): SiteCategory.SiteCategoryID.EditAttrs.Clear()

		' CategoryKeywords
		SiteCategory.CategoryKeywords.CellCssStyle = ""
		SiteCategory.CategoryKeywords.CellCssClass = ""
		SiteCategory.CategoryKeywords.CellAttrs.Clear(): SiteCategory.CategoryKeywords.ViewAttrs.Clear(): SiteCategory.CategoryKeywords.EditAttrs.Clear()

		' CategoryName
		SiteCategory.CategoryName.CellCssStyle = ""
		SiteCategory.CategoryName.CellCssClass = ""
		SiteCategory.CategoryName.CellAttrs.Clear(): SiteCategory.CategoryName.ViewAttrs.Clear(): SiteCategory.CategoryName.EditAttrs.Clear()

		' CategoryTitle
		SiteCategory.CategoryTitle.CellCssStyle = ""
		SiteCategory.CategoryTitle.CellCssClass = ""
		SiteCategory.CategoryTitle.CellAttrs.Clear(): SiteCategory.CategoryTitle.ViewAttrs.Clear(): SiteCategory.CategoryTitle.EditAttrs.Clear()

		' CategoryDescription
		SiteCategory.CategoryDescription.CellCssStyle = ""
		SiteCategory.CategoryDescription.CellCssClass = ""
		SiteCategory.CategoryDescription.CellAttrs.Clear(): SiteCategory.CategoryDescription.ViewAttrs.Clear(): SiteCategory.CategoryDescription.EditAttrs.Clear()

		' GroupOrder
		SiteCategory.GroupOrder.CellCssStyle = ""
		SiteCategory.GroupOrder.CellCssClass = ""
		SiteCategory.GroupOrder.CellAttrs.Clear(): SiteCategory.GroupOrder.ViewAttrs.Clear(): SiteCategory.GroupOrder.EditAttrs.Clear()

		' ParentCategoryID
		SiteCategory.ParentCategoryID.CellCssStyle = ""
		SiteCategory.ParentCategoryID.CellCssClass = ""
		SiteCategory.ParentCategoryID.CellAttrs.Clear(): SiteCategory.ParentCategoryID.ViewAttrs.Clear(): SiteCategory.ParentCategoryID.EditAttrs.Clear()

		' CategoryFileName
		SiteCategory.CategoryFileName.CellCssStyle = ""
		SiteCategory.CategoryFileName.CellCssClass = ""
		SiteCategory.CategoryFileName.CellAttrs.Clear(): SiteCategory.CategoryFileName.ViewAttrs.Clear(): SiteCategory.CategoryFileName.EditAttrs.Clear()

		' SiteCategoryTypeID
		SiteCategory.SiteCategoryTypeID.CellCssStyle = ""
		SiteCategory.SiteCategoryTypeID.CellCssClass = ""
		SiteCategory.SiteCategoryTypeID.CellAttrs.Clear(): SiteCategory.SiteCategoryTypeID.ViewAttrs.Clear(): SiteCategory.SiteCategoryTypeID.EditAttrs.Clear()

		' SiteCategoryGroupID
		SiteCategory.SiteCategoryGroupID.CellCssStyle = ""
		SiteCategory.SiteCategoryGroupID.CellCssClass = ""
		SiteCategory.SiteCategoryGroupID.CellAttrs.Clear(): SiteCategory.SiteCategoryGroupID.ViewAttrs.Clear(): SiteCategory.SiteCategoryGroupID.EditAttrs.Clear()

		'
		'  View  Row
		'

		If SiteCategory.RowType = EW_ROWTYPE_VIEW Then ' View row

			' SiteCategoryID
			SiteCategory.SiteCategoryID.ViewValue = SiteCategory.SiteCategoryID.CurrentValue
			SiteCategory.SiteCategoryID.CssStyle = ""
			SiteCategory.SiteCategoryID.CssClass = ""
			SiteCategory.SiteCategoryID.ViewCustomAttributes = ""

			' CategoryKeywords
			SiteCategory.CategoryKeywords.ViewValue = SiteCategory.CategoryKeywords.CurrentValue
			SiteCategory.CategoryKeywords.CssStyle = ""
			SiteCategory.CategoryKeywords.CssClass = ""
			SiteCategory.CategoryKeywords.ViewCustomAttributes = ""

			' CategoryName
			SiteCategory.CategoryName.ViewValue = SiteCategory.CategoryName.CurrentValue
			SiteCategory.CategoryName.CssStyle = ""
			SiteCategory.CategoryName.CssClass = ""
			SiteCategory.CategoryName.ViewCustomAttributes = ""

			' CategoryTitle
			SiteCategory.CategoryTitle.ViewValue = SiteCategory.CategoryTitle.CurrentValue
			SiteCategory.CategoryTitle.CssStyle = ""
			SiteCategory.CategoryTitle.CssClass = ""
			SiteCategory.CategoryTitle.ViewCustomAttributes = ""

			' CategoryDescription
			SiteCategory.CategoryDescription.ViewValue = SiteCategory.CategoryDescription.CurrentValue
			SiteCategory.CategoryDescription.CssStyle = ""
			SiteCategory.CategoryDescription.CssClass = ""
			SiteCategory.CategoryDescription.ViewCustomAttributes = ""

			' GroupOrder
			SiteCategory.GroupOrder.ViewValue = SiteCategory.GroupOrder.CurrentValue
			SiteCategory.GroupOrder.CssStyle = ""
			SiteCategory.GroupOrder.CssClass = ""
			SiteCategory.GroupOrder.ViewCustomAttributes = ""

			' ParentCategoryID
			SiteCategory.ParentCategoryID.ViewValue = SiteCategory.ParentCategoryID.CurrentValue
			SiteCategory.ParentCategoryID.CssStyle = ""
			SiteCategory.ParentCategoryID.CssClass = ""
			SiteCategory.ParentCategoryID.ViewCustomAttributes = ""

			' CategoryFileName
			SiteCategory.CategoryFileName.ViewValue = SiteCategory.CategoryFileName.CurrentValue
			SiteCategory.CategoryFileName.CssStyle = ""
			SiteCategory.CategoryFileName.CssClass = ""
			SiteCategory.CategoryFileName.ViewCustomAttributes = ""

			' SiteCategoryTypeID
			SiteCategory.SiteCategoryTypeID.ViewValue = SiteCategory.SiteCategoryTypeID.CurrentValue
			SiteCategory.SiteCategoryTypeID.CssStyle = ""
			SiteCategory.SiteCategoryTypeID.CssClass = ""
			SiteCategory.SiteCategoryTypeID.ViewCustomAttributes = ""

			' SiteCategoryGroupID
			SiteCategory.SiteCategoryGroupID.ViewValue = SiteCategory.SiteCategoryGroupID.CurrentValue
			SiteCategory.SiteCategoryGroupID.CssStyle = ""
			SiteCategory.SiteCategoryGroupID.CssClass = ""
			SiteCategory.SiteCategoryGroupID.ViewCustomAttributes = ""

			' View refer script
			' SiteCategoryID

			SiteCategory.SiteCategoryID.HrefValue = ""
			SiteCategory.SiteCategoryID.TooltipValue = ""

			' CategoryKeywords
			SiteCategory.CategoryKeywords.HrefValue = ""
			SiteCategory.CategoryKeywords.TooltipValue = ""

			' CategoryName
			SiteCategory.CategoryName.HrefValue = ""
			SiteCategory.CategoryName.TooltipValue = ""

			' CategoryTitle
			SiteCategory.CategoryTitle.HrefValue = ""
			SiteCategory.CategoryTitle.TooltipValue = ""

			' CategoryDescription
			SiteCategory.CategoryDescription.HrefValue = ""
			SiteCategory.CategoryDescription.TooltipValue = ""

			' GroupOrder
			SiteCategory.GroupOrder.HrefValue = ""
			SiteCategory.GroupOrder.TooltipValue = ""

			' ParentCategoryID
			SiteCategory.ParentCategoryID.HrefValue = ""
			SiteCategory.ParentCategoryID.TooltipValue = ""

			' CategoryFileName
			SiteCategory.CategoryFileName.HrefValue = ""
			SiteCategory.CategoryFileName.TooltipValue = ""

			' SiteCategoryTypeID
			SiteCategory.SiteCategoryTypeID.HrefValue = ""
			SiteCategory.SiteCategoryTypeID.TooltipValue = ""

			' SiteCategoryGroupID
			SiteCategory.SiteCategoryGroupID.HrefValue = ""
			SiteCategory.SiteCategoryGroupID.TooltipValue = ""
		End If

		' Row Rendered event
		If SiteCategory.RowType <> EW_ROWTYPE_AGGREGATEINIT Then
			SiteCategory.Row_Rendered()
		End If
	End Sub

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "SiteCategory"
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
		Dim table As String = "SiteCategory"
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
		sKey = sKey & RsSrc("SiteCategoryID")
		keyvalue = sKey

		' SiteCategoryID Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "SiteCategoryID", keyvalue, RsSrc("SiteCategoryID"), newvalue)

		' CategoryKeywords Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "CategoryKeywords", keyvalue, RsSrc("CategoryKeywords"), newvalue)

		' CategoryName Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "CategoryName", keyvalue, RsSrc("CategoryName"), newvalue)

		' CategoryTitle Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "CategoryTitle", keyvalue, RsSrc("CategoryTitle"), newvalue)

		' CategoryDescription Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "CategoryDescription", keyvalue, RsSrc("CategoryDescription"), newvalue)

		' GroupOrder Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "GroupOrder", keyvalue, RsSrc("GroupOrder"), newvalue)

		' ParentCategoryID Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "ParentCategoryID", keyvalue, RsSrc("ParentCategoryID"), newvalue)

		' CategoryFileName Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "CategoryFileName", keyvalue, RsSrc("CategoryFileName"), newvalue)

		' SiteCategoryTypeID Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "SiteCategoryTypeID", keyvalue, RsSrc("SiteCategoryTypeID"), newvalue)

		' SiteCategoryGroupID Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "SiteCategoryGroupID", keyvalue, RsSrc("SiteCategoryGroupID"), newvalue)
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
		SiteCategory_delete = New cSiteCategory_delete(Me)		
		SiteCategory_delete.Page_Init()		

		' Set buffer/cache option
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

		' Page main processing
		SiteCategory_delete.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If SiteCategory_delete IsNot Nothing Then SiteCategory_delete.Dispose()
	End Sub
End Class
