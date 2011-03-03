Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class pagealias_add
	Inherits AspNetMaker8_wpmWebsite

	' Page object
	Public PageAlias_add As cPageAlias_add

	'
	' Page Class
	'
	Class cPageAlias_add
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
				If PageAlias.UseTokenInUrl Then Url = Url & "t=" & PageAlias.TableVar & "&" ' Add page token
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
			If PageAlias.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (PageAlias.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (PageAlias.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' ASP.NET page object
		Public ReadOnly Property AspNetPage() As pagealias_add
			Get
				Return CType(m_ParentPage, pagealias_add)
			End Get
		End Property

		' PageAlias
		Public Property PageAlias() As cPageAlias
			Get				
				Return ParentPage.PageAlias
			End Get
			Set(ByVal v As cPageAlias)
				ParentPage.PageAlias = v	
			End Set	
		End Property

		' PageAlias
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
			m_PageID = "add"
			m_PageObjName = "PageAlias_add"
			m_PageObjTypeName = "cPageAlias_add"			

			' Initialize language object
			Language = New cLanguage(Me)

			' Table Name
			m_TableName = "PageAlias"

			' Initialize table object
			PageAlias = New cPageAlias(Me)
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
			PageAlias.Dispose()
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

	Public sDbMasterFilter As String, sDbDetailFilter As String

	Public x_ewPriv As Integer

	'
	' Page main processing
	'
	Sub Page_Main()

		' Load key values from QueryString
		Dim bCopy As Boolean = True
		If ew_Get("PageAliasID") <> "" Then
			PageAlias.PageAliasID.QueryStringValue = ew_Get("PageAliasID")
		Else
			bCopy = False
		End If

		' Set up master detail parameters
		SetUpMasterDetail()

		' Process form if post back
		If ObjForm.GetValue("a_add") <> "" Then
			PageAlias.CurrentAction = ObjForm.GetValue("a_add") ' Get form action
			LoadFormValues() ' Load form values

			' Validate Form
			If Not ValidateForm() Then
				PageAlias.CurrentAction = "I" ' Form error, reset action
				Message = ParentPage.gsFormError
			End If

		' Not post back
		Else
			If bCopy Then
				PageAlias.CurrentAction = "C" ' Copy Record
			Else
				PageAlias.CurrentAction = "I" ' Display Blank Record
				LoadDefaultValues() ' Load default values
			End If
		End If

		' Perform action based on action code
		Select Case PageAlias.CurrentAction
			Case "I" ' Blank record, no action required
			Case "C" ' Copy an existing record
				If Not LoadRow() Then ' Load record based on key
					Message = Language.Phrase("NoRecord") ' No record found
					Page_Terminate("pagealias_list.aspx") ' No matching record, return to list
				End If
			Case "A" ' Add new record
				PageAlias.SendEmail = True ' Send email on add success
				If AddRow() Then ' Add successful
					Message = Language.Phrase("AddSuccess") ' Set up success message
					Dim sReturnUrl As String = PageAlias.ReturnUrl
					Page_Terminate(sReturnUrl) ' Clean up and return
				Else
					RestoreFormValues() ' Add failed, restore form values
				End If
		End Select

		' Render row based on row type
		PageAlias.RowType = EW_ROWTYPE_ADD ' Render add type

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
		PageAlias.AliasType.CurrentValue = "301"
	End Sub

	'
	' Load form values
	'
	Sub LoadFormValues()
		PageAlias.zPageURL.FormValue = ObjForm.GetValue("x_zPageURL")
		PageAlias.zPageURL.OldValue = ObjForm.GetValue("o_zPageURL")
		PageAlias.TargetURL.FormValue = ObjForm.GetValue("x_TargetURL")
		PageAlias.TargetURL.OldValue = ObjForm.GetValue("o_TargetURL")
		PageAlias.AliasType.FormValue = ObjForm.GetValue("x_AliasType")
		PageAlias.AliasType.OldValue = ObjForm.GetValue("o_AliasType")
		PageAlias.CompanyID.FormValue = ObjForm.GetValue("x_CompanyID")
		PageAlias.CompanyID.OldValue = ObjForm.GetValue("o_CompanyID")
		PageAlias.PageAliasID.FormValue = ObjForm.GetValue("x_PageAliasID")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		PageAlias.zPageURL.CurrentValue = PageAlias.zPageURL.FormValue
		PageAlias.TargetURL.CurrentValue = PageAlias.TargetURL.FormValue
		PageAlias.AliasType.CurrentValue = PageAlias.AliasType.FormValue
		PageAlias.CompanyID.CurrentValue = PageAlias.CompanyID.FormValue
		PageAlias.PageAliasID.CurrentValue = PageAlias.PageAliasID.FormValue
	End Sub

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = PageAlias.KeyFilter

		' Row Selecting event
		PageAlias.Row_Selecting(sFilter)

		' Load SQL based on filter
		PageAlias.CurrentFilter = sFilter
		Dim sSql As String = PageAlias.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then DebugMsg = sSql
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				PageAlias.Row_Selected(RsRow)
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
		PageAlias.PageAliasID.DbValue = RsRow("PageAliasID")
		PageAlias.zPageURL.DbValue = RsRow("PageURL")
		PageAlias.TargetURL.DbValue = RsRow("TargetURL")
		PageAlias.AliasType.DbValue = RsRow("AliasType")
		PageAlias.CompanyID.DbValue = RsRow("CompanyID")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Initialize urls
		' Row Rendering event

		PageAlias.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' PageURL

		PageAlias.zPageURL.CellCssStyle = ""
		PageAlias.zPageURL.CellCssClass = ""
		PageAlias.zPageURL.CellAttrs.Clear(): PageAlias.zPageURL.ViewAttrs.Clear(): PageAlias.zPageURL.EditAttrs.Clear()

		' TargetURL
		PageAlias.TargetURL.CellCssStyle = ""
		PageAlias.TargetURL.CellCssClass = ""
		PageAlias.TargetURL.CellAttrs.Clear(): PageAlias.TargetURL.ViewAttrs.Clear(): PageAlias.TargetURL.EditAttrs.Clear()

		' AliasType
		PageAlias.AliasType.CellCssStyle = ""
		PageAlias.AliasType.CellCssClass = ""
		PageAlias.AliasType.CellAttrs.Clear(): PageAlias.AliasType.ViewAttrs.Clear(): PageAlias.AliasType.EditAttrs.Clear()

		' CompanyID
		PageAlias.CompanyID.CellCssStyle = ""
		PageAlias.CompanyID.CellCssClass = ""
		PageAlias.CompanyID.CellAttrs.Clear(): PageAlias.CompanyID.ViewAttrs.Clear(): PageAlias.CompanyID.EditAttrs.Clear()

		'
		'  View  Row
		'

		If PageAlias.RowType = EW_ROWTYPE_VIEW Then ' View row

			' PageAliasID
			PageAlias.PageAliasID.ViewValue = PageAlias.PageAliasID.CurrentValue
			PageAlias.PageAliasID.CssStyle = ""
			PageAlias.PageAliasID.CssClass = ""
			PageAlias.PageAliasID.ViewCustomAttributes = ""

			' PageURL
			PageAlias.zPageURL.ViewValue = PageAlias.zPageURL.CurrentValue
			PageAlias.zPageURL.CssStyle = ""
			PageAlias.zPageURL.CssClass = ""
			PageAlias.zPageURL.ViewCustomAttributes = ""

			' TargetURL
			PageAlias.TargetURL.ViewValue = PageAlias.TargetURL.CurrentValue
			PageAlias.TargetURL.CssStyle = ""
			PageAlias.TargetURL.CssClass = ""
			PageAlias.TargetURL.ViewCustomAttributes = ""

			' AliasType
			PageAlias.AliasType.ViewValue = PageAlias.AliasType.CurrentValue
			PageAlias.AliasType.CssStyle = ""
			PageAlias.AliasType.CssClass = ""
			PageAlias.AliasType.ViewCustomAttributes = ""

			' CompanyID
			If ew_NotEmpty(PageAlias.CompanyID.CurrentValue) Then
				sFilterWrk = "[CompanyID] = " & ew_AdjustSql(PageAlias.CompanyID.CurrentValue) & ""
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
					PageAlias.CompanyID.ViewValue = RsWrk("CompanyName")
				Else
					PageAlias.CompanyID.ViewValue = PageAlias.CompanyID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				PageAlias.CompanyID.ViewValue = System.DBNull.Value
			End If
			PageAlias.CompanyID.CssStyle = ""
			PageAlias.CompanyID.CssClass = ""
			PageAlias.CompanyID.ViewCustomAttributes = ""

			' View refer script
			' PageURL

			PageAlias.zPageURL.HrefValue = ""
			PageAlias.zPageURL.TooltipValue = ""

			' TargetURL
			PageAlias.TargetURL.HrefValue = ""
			PageAlias.TargetURL.TooltipValue = ""

			' AliasType
			PageAlias.AliasType.HrefValue = ""
			PageAlias.AliasType.TooltipValue = ""

			' CompanyID
			PageAlias.CompanyID.HrefValue = ""
			PageAlias.CompanyID.TooltipValue = ""

		'
		'  Add Row
		'

		ElseIf PageAlias.RowType = EW_ROWTYPE_ADD Then ' Add row

			' PageURL
			PageAlias.zPageURL.EditCustomAttributes = ""
			PageAlias.zPageURL.EditValue = ew_HtmlEncode(PageAlias.zPageURL.CurrentValue)

			' TargetURL
			PageAlias.TargetURL.EditCustomAttributes = ""
			PageAlias.TargetURL.EditValue = ew_HtmlEncode(PageAlias.TargetURL.CurrentValue)

			' AliasType
			PageAlias.AliasType.EditCustomAttributes = ""
			PageAlias.AliasType.EditValue = ew_HtmlEncode(PageAlias.AliasType.CurrentValue)

			' CompanyID
			PageAlias.CompanyID.EditCustomAttributes = ""
			If PageAlias.CompanyID.SessionValue <> "" Then
				PageAlias.CompanyID.CurrentValue = PageAlias.CompanyID.SessionValue
			If ew_NotEmpty(PageAlias.CompanyID.CurrentValue) Then
				sFilterWrk = "[CompanyID] = " & ew_AdjustSql(PageAlias.CompanyID.CurrentValue) & ""
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
					PageAlias.CompanyID.ViewValue = RsWrk("CompanyName")
				Else
					PageAlias.CompanyID.ViewValue = PageAlias.CompanyID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				PageAlias.CompanyID.ViewValue = System.DBNull.Value
			End If
			PageAlias.CompanyID.CssStyle = ""
			PageAlias.CompanyID.CssClass = ""
			PageAlias.CompanyID.ViewCustomAttributes = ""
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
			PageAlias.CompanyID.EditValue = arwrk
			End If
		End If

		' Row Rendered event
		If PageAlias.RowType <> EW_ROWTYPE_AGGREGATEINIT Then
			PageAlias.Row_Rendered()
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

			' PageURL
			PageAlias.zPageURL.SetDbValue(Rs, PageAlias.zPageURL.CurrentValue, System.DBNull.Value, False)

			' TargetURL
			PageAlias.TargetURL.SetDbValue(Rs, PageAlias.TargetURL.CurrentValue, System.DBNull.Value, False)

			' AliasType
			PageAlias.AliasType.SetDbValue(Rs, PageAlias.AliasType.CurrentValue, System.DBNull.Value, False)

			' CompanyID
			PageAlias.CompanyID.SetDbValue(Rs, PageAlias.CompanyID.CurrentValue, System.DBNull.Value, False)
		Catch e As Exception
			If EW_DEBUG_ENABLED Then Throw
			Message = e.Message
			Return False
		End Try

		' Row Inserting event
		bInsertRow = PageAlias.Row_Inserting(Rs)
		If bInsertRow Then
			Try	
				PageAlias.Insert(Rs)
				AddRow = True
			Catch e As Exception
				If EW_DEBUG_ENABLED Then Throw
				Message = e.Message				
				AddRow = False
			End Try
		Else
			If PageAlias.CancelMessage <> "" Then
				Message = PageAlias.CancelMessage
				PageAlias.CancelMessage = ""
			Else
				Message = Language.Phrase("InsertCancelled")
			End If
			AddRow = False
		End If
		If AddRow Then
			LastInsertId = Conn.GetLastInsertId()
			PageAlias.PageAliasID.DbValue = LastInsertId
			Rs("PageAliasID") = PageAlias.PageAliasID.DbValue		

			' Row Inserted event
			PageAlias.Row_Inserted(Rs)
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
				sDbMasterFilter = PageAlias.SqlMasterFilter_Company
				sDbDetailFilter = PageAlias.SqlDetailFilter_Company
				If ew_Get("CompanyID") <> "" Then
					Company.CompanyID.QueryStringValue = ew_Get("CompanyID")
					PageAlias.CompanyID.QueryStringValue = Company.CompanyID.QueryStringValue
					PageAlias.CompanyID.SessionValue = PageAlias.CompanyID.QueryStringValue
					If Not IsNumeric(Company.CompanyID.QueryStringValue) Then bValidMaster = False
					sDbMasterFilter = sDbMasterFilter.Replace("@CompanyID@", ew_AdjustSql(Company.CompanyID.QueryStringValue))
					sDbDetailFilter = sDbDetailFilter.Replace("@CompanyID@", ew_AdjustSql(Company.CompanyID.QueryStringValue))
				Else
					bValidMaster = False
				End If
			End If
		End If
		If bValidMaster Then

			' Save current master table
			PageAlias.CurrentMasterTable = sMasterTblVar
			PageAlias.MasterFilter = sDbMasterFilter ' Set up master filter
			PageAlias.DetailFilter = sDbDetailFilter ' Set up detail filter

			' Clear previous master session values
			If sMasterTblVar <> "Company" Then
				If PageAlias.CompanyID.QueryStringValue = "" Then PageAlias.CompanyID.SessionValue = ""
			End If
		End If
	End Sub

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "PageAlias"
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
		Dim table As String = "PageAlias"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsSrc("PageAliasID")

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

		' PageURL Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "PageURL", keyvalue, oldvalue, RsSrc("PageURL"))

		' TargetURL Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "TargetURL", keyvalue, oldvalue, RsSrc("TargetURL"))

		' AliasType Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "AliasType", keyvalue, oldvalue, RsSrc("AliasType"))

		' CompanyID Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "CompanyID", keyvalue, oldvalue, RsSrc("CompanyID"))
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
		PageAlias_add = New cPageAlias_add(Me)		
		PageAlias_add.Page_Init()		

		' Set buffer/cache option
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

		' Page main processing
		PageAlias_add.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If PageAlias_add IsNot Nothing Then PageAlias_add.Dispose()
	End Sub
End Class
