Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class linkrank_edit
	Inherits AspNetMaker8_wpmWebsite

	' Page object
	Public LinkRank_edit As cLinkRank_edit

	'
	' Page Class
	'
	Class cLinkRank_edit
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
				If LinkRank.UseTokenInUrl Then Url = Url & "t=" & LinkRank.TableVar & "&" ' Add page token
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
			If LinkRank.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (LinkRank.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (LinkRank.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' ASP.NET page object
		Public ReadOnly Property AspNetPage() As linkrank_edit
			Get
				Return CType(m_ParentPage, linkrank_edit)
			End Get
		End Property

		' LinkRank
		Public Property LinkRank() As cLinkRank
			Get				
				Return ParentPage.LinkRank
			End Get
			Set(ByVal v As cLinkRank)
				ParentPage.LinkRank = v	
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
			m_PageID = "edit"
			m_PageObjName = "LinkRank_edit"
			m_PageObjTypeName = "cLinkRank_edit"			

			' Initialize language object
			Language = New cLanguage(Me)

			' Table Name
			m_TableName = "LinkRank"

			' Initialize table object
			LinkRank = New cLinkRank(Me)

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
			LinkRank.Dispose()
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

	'
	' Page main processing
	'
	Sub Page_Main()

		' Load key from QueryString
		If ew_Get("ID") <> "" Then
			LinkRank.ID.QueryStringValue = ew_Get("ID")
		End If
		If ObjForm.GetValue("a_edit") <> "" Then
			LinkRank.CurrentAction = ObjForm.GetValue("a_edit") ' Get action code
			LoadFormValues() ' Get form values

			' Validate Form
			If Not ValidateForm() Then
				LinkRank.CurrentAction = "" ' Form error, reset action
				Message = ParentPage.gsFormError
				LinkRank.EventCancelled = True ' Event cancelled
				LoadRow() ' Restore row
				RestoreFormValues() ' Restore form values if validate failed
			End If
		Else
			LinkRank.CurrentAction = "I" ' Default action is display
		End If

		' Check if valid key
		If ew_Empty(LinkRank.ID.CurrentValue) Then Page_Terminate("linkrank_list.aspx") ' Invalid key, return to list
		Select Case LinkRank.CurrentAction
			Case "I" ' Get a record to display
				If Not LoadRow() Then ' Load Record based on key
					Message = Language.Phrase("NoRecord") ' No record found
					Page_Terminate("linkrank_list.aspx") ' No matching record, return to list
				End If
			Case "U" ' Update
				LinkRank.SendEmail = True ' Send email on update success
				If EditRow() Then ' Update Record based on key
					Message = Language.Phrase("UpdateSuccess") ' Update success
					Dim sReturnUrl As String = LinkRank.ReturnUrl
					Page_Terminate(sReturnUrl) ' Return to caller
				Else
					LinkRank.EventCancelled = True ' Event cancelled 
					LoadRow() ' Restore row
					RestoreFormValues() ' Restore form values if update failed
				End If
		End Select

		' Render the record
		LinkRank.RowType = EW_ROWTYPE_EDIT ' Render as edit

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
	End Sub

	'
	' Load form values
	'
	Sub LoadFormValues()
		LinkRank.ID.FormValue = ObjForm.GetValue("x_ID")
		LinkRank.ID.OldValue = ObjForm.GetValue("o_ID")
		LinkRank.LinkID.FormValue = ObjForm.GetValue("x_LinkID")
		LinkRank.LinkID.OldValue = ObjForm.GetValue("o_LinkID")
		LinkRank.UserID.FormValue = ObjForm.GetValue("x_UserID")
		LinkRank.UserID.OldValue = ObjForm.GetValue("o_UserID")
		LinkRank.RankNum.FormValue = ObjForm.GetValue("x_RankNum")
		LinkRank.RankNum.OldValue = ObjForm.GetValue("o_RankNum")
		LinkRank.CateID.FormValue = ObjForm.GetValue("x_CateID")
		LinkRank.CateID.OldValue = ObjForm.GetValue("o_CateID")
		LinkRank.Comment.FormValue = ObjForm.GetValue("x_Comment")
		LinkRank.Comment.OldValue = ObjForm.GetValue("o_Comment")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		LinkRank.ID.CurrentValue = LinkRank.ID.FormValue
		LinkRank.LinkID.CurrentValue = LinkRank.LinkID.FormValue
		LinkRank.UserID.CurrentValue = LinkRank.UserID.FormValue
		LinkRank.RankNum.CurrentValue = LinkRank.RankNum.FormValue
		LinkRank.CateID.CurrentValue = LinkRank.CateID.FormValue
		LinkRank.Comment.CurrentValue = LinkRank.Comment.FormValue
	End Sub

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = LinkRank.KeyFilter

		' Row Selecting event
		LinkRank.Row_Selecting(sFilter)

		' Load SQL based on filter
		LinkRank.CurrentFilter = sFilter
		Dim sSql As String = LinkRank.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then DebugMsg = sSql
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				LinkRank.Row_Selected(RsRow)
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
		LinkRank.ID.DbValue = RsRow("ID")
		LinkRank.LinkID.DbValue = RsRow("LinkID")
		LinkRank.UserID.DbValue = RsRow("UserID")
		LinkRank.RankNum.DbValue = RsRow("RankNum")
		LinkRank.CateID.DbValue = RsRow("CateID")
		LinkRank.Comment.DbValue = RsRow("Comment")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Initialize urls
		' Row Rendering event

		LinkRank.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' ID

		LinkRank.ID.CellCssStyle = ""
		LinkRank.ID.CellCssClass = ""
		LinkRank.ID.CellAttrs.Clear(): LinkRank.ID.ViewAttrs.Clear(): LinkRank.ID.EditAttrs.Clear()

		' LinkID
		LinkRank.LinkID.CellCssStyle = ""
		LinkRank.LinkID.CellCssClass = ""
		LinkRank.LinkID.CellAttrs.Clear(): LinkRank.LinkID.ViewAttrs.Clear(): LinkRank.LinkID.EditAttrs.Clear()

		' UserID
		LinkRank.UserID.CellCssStyle = ""
		LinkRank.UserID.CellCssClass = ""
		LinkRank.UserID.CellAttrs.Clear(): LinkRank.UserID.ViewAttrs.Clear(): LinkRank.UserID.EditAttrs.Clear()

		' RankNum
		LinkRank.RankNum.CellCssStyle = ""
		LinkRank.RankNum.CellCssClass = ""
		LinkRank.RankNum.CellAttrs.Clear(): LinkRank.RankNum.ViewAttrs.Clear(): LinkRank.RankNum.EditAttrs.Clear()

		' CateID
		LinkRank.CateID.CellCssStyle = ""
		LinkRank.CateID.CellCssClass = ""
		LinkRank.CateID.CellAttrs.Clear(): LinkRank.CateID.ViewAttrs.Clear(): LinkRank.CateID.EditAttrs.Clear()

		' Comment
		LinkRank.Comment.CellCssStyle = ""
		LinkRank.Comment.CellCssClass = ""
		LinkRank.Comment.CellAttrs.Clear(): LinkRank.Comment.ViewAttrs.Clear(): LinkRank.Comment.EditAttrs.Clear()

		'
		'  View  Row
		'

		If LinkRank.RowType = EW_ROWTYPE_VIEW Then ' View row

			' ID
			LinkRank.ID.ViewValue = LinkRank.ID.CurrentValue
			LinkRank.ID.CssStyle = ""
			LinkRank.ID.CssClass = ""
			LinkRank.ID.ViewCustomAttributes = ""

			' LinkID
			LinkRank.LinkID.ViewValue = LinkRank.LinkID.CurrentValue
			LinkRank.LinkID.CssStyle = ""
			LinkRank.LinkID.CssClass = ""
			LinkRank.LinkID.ViewCustomAttributes = ""

			' UserID
			LinkRank.UserID.ViewValue = LinkRank.UserID.CurrentValue
			LinkRank.UserID.CssStyle = ""
			LinkRank.UserID.CssClass = ""
			LinkRank.UserID.ViewCustomAttributes = ""

			' RankNum
			LinkRank.RankNum.ViewValue = LinkRank.RankNum.CurrentValue
			LinkRank.RankNum.CssStyle = ""
			LinkRank.RankNum.CssClass = ""
			LinkRank.RankNum.ViewCustomAttributes = ""

			' CateID
			LinkRank.CateID.ViewValue = LinkRank.CateID.CurrentValue
			LinkRank.CateID.CssStyle = ""
			LinkRank.CateID.CssClass = ""
			LinkRank.CateID.ViewCustomAttributes = ""

			' Comment
			LinkRank.Comment.ViewValue = LinkRank.Comment.CurrentValue
			LinkRank.Comment.CssStyle = ""
			LinkRank.Comment.CssClass = ""
			LinkRank.Comment.ViewCustomAttributes = ""

			' View refer script
			' ID

			LinkRank.ID.HrefValue = ""
			LinkRank.ID.TooltipValue = ""

			' LinkID
			LinkRank.LinkID.HrefValue = ""
			LinkRank.LinkID.TooltipValue = ""

			' UserID
			LinkRank.UserID.HrefValue = ""
			LinkRank.UserID.TooltipValue = ""

			' RankNum
			LinkRank.RankNum.HrefValue = ""
			LinkRank.RankNum.TooltipValue = ""

			' CateID
			LinkRank.CateID.HrefValue = ""
			LinkRank.CateID.TooltipValue = ""

			' Comment
			LinkRank.Comment.HrefValue = ""
			LinkRank.Comment.TooltipValue = ""

		'
		'  Edit Row
		'

		ElseIf LinkRank.RowType = EW_ROWTYPE_EDIT Then ' Edit row

			' ID
			LinkRank.ID.EditCustomAttributes = ""
			LinkRank.ID.EditValue = LinkRank.ID.CurrentValue
			LinkRank.ID.CssStyle = ""
			LinkRank.ID.CssClass = ""
			LinkRank.ID.ViewCustomAttributes = ""

			' LinkID
			LinkRank.LinkID.EditCustomAttributes = ""
			LinkRank.LinkID.EditValue = ew_HtmlEncode(LinkRank.LinkID.CurrentValue)

			' UserID
			LinkRank.UserID.EditCustomAttributes = ""
			LinkRank.UserID.EditValue = ew_HtmlEncode(LinkRank.UserID.CurrentValue)

			' RankNum
			LinkRank.RankNum.EditCustomAttributes = ""
			LinkRank.RankNum.EditValue = ew_HtmlEncode(LinkRank.RankNum.CurrentValue)

			' CateID
			LinkRank.CateID.EditCustomAttributes = ""
			LinkRank.CateID.EditValue = ew_HtmlEncode(LinkRank.CateID.CurrentValue)

			' Comment
			LinkRank.Comment.EditCustomAttributes = ""
			LinkRank.Comment.EditValue = ew_HtmlEncode(LinkRank.Comment.CurrentValue)

			' Edit refer script
			' ID

			LinkRank.ID.HrefValue = ""

			' LinkID
			LinkRank.LinkID.HrefValue = ""

			' UserID
			LinkRank.UserID.HrefValue = ""

			' RankNum
			LinkRank.RankNum.HrefValue = ""

			' CateID
			LinkRank.CateID.HrefValue = ""

			' Comment
			LinkRank.Comment.HrefValue = ""
		End If

		' Row Rendered event
		If LinkRank.RowType <> EW_ROWTYPE_AGGREGATEINIT Then
			LinkRank.Row_Rendered()
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
		If Not ew_CheckInteger(LinkRank.LinkID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError &= LinkRank.LinkID.FldErrMsg
		End If
		If Not ew_CheckInteger(LinkRank.UserID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError &= LinkRank.UserID.FldErrMsg
		End If
		If Not ew_CheckInteger(LinkRank.RankNum.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError &= LinkRank.RankNum.FldErrMsg
		End If
		If Not ew_CheckInteger(LinkRank.CateID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError &= LinkRank.CateID.FldErrMsg
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
	' Update record based on key values
	'
	Function EditRow() As Boolean
		Dim RsEdit As OleDbDataReader, RsChk As OleDbDataReader
		Dim sSql As String, sFilter As String
		Dim sSqlChk As String, sFilterChk As String
		Dim bUpdateRow As Boolean
		Dim RsOld As OrderedDictionary
		Dim sIdxErrMsg As String
		Dim Rs As New OrderedDictionary
		sFilter = LinkRank.KeyFilter
		LinkRank.CurrentFilter  = sFilter
		sSql = LinkRank.SQL
		Try
			RsEdit = Conn.GetDataReader(sSql) 
		Catch e As Exception
			If EW_DEBUG_ENABLED Then Throw
			Message = e.Message
			RsEdit.Close()
			EditRow = False
		End Try
		If Not RsEdit.Read() Then
			RsEdit.Close()
			EditRow = False ' Update Failed
		Else
			Try
				RsOld = Conn.GetRow(RsEdit)					 

				' ID
				' LinkID

				LinkRank.LinkID.SetDbValue(Rs, LinkRank.LinkID.CurrentValue, System.DBNull.Value, False)

				' UserID
				LinkRank.UserID.SetDbValue(Rs, LinkRank.UserID.CurrentValue, System.DBNull.Value, False)

				' RankNum
				LinkRank.RankNum.SetDbValue(Rs, LinkRank.RankNum.CurrentValue, System.DBNull.Value, False)

				' CateID
				LinkRank.CateID.SetDbValue(Rs, LinkRank.CateID.CurrentValue, System.DBNull.Value, False)

				' Comment
				LinkRank.Comment.SetDbValue(Rs, LinkRank.Comment.CurrentValue, System.DBNull.Value, False)
			Catch e As Exception
				If EW_DEBUG_ENABLED Then Throw
				Message = e.Message
				EditRow = False
			End Try
			RsEdit.Close()

			' Row Updating event
			bUpdateRow = LinkRank.Row_Updating(RsOld, Rs)
			If bUpdateRow Then
				Try
					LinkRank.Update(Rs)
					EditRow = True
				Catch e As Exception
					If EW_DEBUG_ENABLED Then Throw
					Message = e.Message
					EditRow = False
				End Try
			Else
				If LinkRank.CancelMessage <> "" Then
					Message = LinkRank.CancelMessage
					LinkRank.CancelMessage = ""
				Else
					Message = Language.Phrase("UpdateCancelled")
				End If
				EditRow = False
			End If
		End If

		' Row Updated event
		If EditRow Then
			LinkRank.Row_Updated(RsOld, Rs)
		End If
		If EditRow Then
			WriteAuditTrailOnEdit(RsOld, Rs)
		End If
	End Function

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "LinkRank"
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

	' Write Audit Trail (edit page)
	Sub WriteAuditTrailOnEdit(ByRef RsOld As OrderedDictionary, ByRef RsNew As OrderedDictionary)
		Dim table As String = "LinkRank"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsOld("ID")

		' Write Audit Trail
		Dim filePfx As String = "log"
		Dim action As String = "U"
		Dim curDateTime As String, id As String, user As String, field As String
		Dim keyvalue As Object, oldvalue As Object, newvalue As Object
		Dim dt As DateTime = Now()
		Dim fld As cField, fldname As String, Modified As Boolean
		curDateTime = dt.ToString("yyyy/MM/dd HH:mm:ss")
		id = HttpContext.Current.Request.ServerVariables("SCRIPT_NAME")
    user = CurrentUserName()
		Dim arFldNames(RsNew.Count - 1) As String
		RsNew.Keys.CopyTo(arFldNames, 0)
		For Each fldname In arFldNames
			fld = LinkRank.FieldByName(fldname)
			If fld IsNot Nothing AndAlso fld.FldDataType <> EW_DATATYPE_BLOB AndAlso fld.FldDataType <> EW_DATATYPE_MEMO Then ' Ignore Blob/Memo Field
				oldvalue = RsOld(fldname)
				newvalue = RsNew(fldname)

				'oldvalue = ew_Conv(oldvalue, fld.FldType)
				'newvalue = ew_Conv(newvalue, fld.FldType)

				If fld.FldDataType = EW_DATATYPE_DATE Then ' DateTime Field
					Modified = Not ew_SameStr(ew_FormatDateTime(oldvalue, 8), ew_FormatDateTime(newvalue, 8))
				Else
					Modified = Not ew_CompareValue(oldvalue, newvalue)
				End If				
				If Modified Then					
					keyvalue = sKey
					ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, fldname, keyvalue, oldvalue, newvalue)
				End If
			End If
		Next
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
		LinkRank_edit = New cLinkRank_edit(Me)		
		LinkRank_edit.Page_Init()		

		' Set buffer/cache option
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

		' Page main processing
		LinkRank_edit.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If LinkRank_edit IsNot Nothing Then LinkRank_edit.Dispose()
	End Sub
End Class
