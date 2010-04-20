Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class SiteCategoryType_edit
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public SiteCategoryType_edit As cSiteCategoryType_edit

	'
	' Page Class
	'
	Class cSiteCategoryType_edit
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
				If SiteCategoryType.UseTokenInUrl Then Url = Url & "t=" & SiteCategoryType.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If SiteCategoryType.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (SiteCategoryType.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (SiteCategoryType.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' SiteCategoryType
		Public Property SiteCategoryType() As cSiteCategoryType
			Get				
				Return ParentPage.SiteCategoryType
			End Get
			Set(ByVal v As cSiteCategoryType)
				ParentPage.SiteCategoryType = v	
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
			m_PageID = "edit"
			m_PageObjName = "SiteCategoryType_edit"
			m_PageObjTypeName = "cSiteCategoryType_edit"

			' Table Name
			m_TableName = "SiteCategoryType"

			' Initialize table object
			SiteCategoryType = New cSiteCategoryType(Me)

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
			SiteCategoryType.Dispose()

			' Go to URL if specified
			If url <> "" Then
				HttpContext.Current.Response.Clear()
				HttpContext.Current.Response.Redirect(url)
			End If
		End Sub

	'
	' Page main processing
	'
	Sub Page_Main()

		' Load key from QueryString
		If ew_Get("SiteCategoryTypeID") <> "" Then
			SiteCategoryType.SiteCategoryTypeID.QueryStringValue = ew_Get("SiteCategoryTypeID")
		End If

		' Create form object
		ObjForm = New cFormObj
		If ObjForm.GetValue("a_edit") <> "" Then
			SiteCategoryType.CurrentAction = ObjForm.GetValue("a_edit") ' Get action code
			LoadFormValues() ' Get form values

			' Validate Form
			If Not ValidateForm() Then
				SiteCategoryType.CurrentAction = "" ' Form error, reset action
				Message = ParentPage.gsFormError
				LoadRow() ' Restore row
				RestoreFormValues() ' Restore form values if validate failed
			End If
		Else
			SiteCategoryType.CurrentAction = "I" ' Default action is display
		End If

		' Check if valid key
		If ew_Empty(SiteCategoryType.SiteCategoryTypeID.CurrentValue) Then Page_Terminate("SiteCategoryType_list.aspx") ' Invalid key, return to list
		Select Case SiteCategoryType.CurrentAction
			Case "I" ' Get a record to display
				If Not LoadRow() Then ' Load Record based on key
					Message = "No records found" ' No record found
					Page_Terminate("SiteCategoryType_list.aspx") ' No matching record, return to list
				End If
			Case "U" ' Update
				SiteCategoryType.SendEmail = True ' Send email on update success
				If EditRow() Then ' Update Record based on key
					Message = "Update succeeded" ' Update success
					Dim sReturnUrl As String = SiteCategoryType.ReturnUrl
					If ew_GetPageName(sReturnUrl) = "SiteCategoryType_view.aspx" Then sReturnUrl = SiteCategoryType.ViewUrl ' View paging, return to view page with keyurl directly
					Page_Terminate(sReturnUrl) ' Return to caller
				Else
					LoadRow() ' Restore row
					RestoreFormValues() ' Restore form values if update failed
				End If
		End Select

		' Render the record
		SiteCategoryType.RowType = EW_ROWTYPE_EDIT ' Render as edit

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
		SiteCategoryType.SiteCategoryTypeID.FormValue = ObjForm.GetValue("x_SiteCategoryTypeID")
		SiteCategoryType.SiteCategoryTypeID.OldValue = ObjForm.GetValue("o_SiteCategoryTypeID")
		SiteCategoryType.SiteCategoryTypeNM.FormValue = ObjForm.GetValue("x_SiteCategoryTypeNM")
		SiteCategoryType.SiteCategoryTypeNM.OldValue = ObjForm.GetValue("o_SiteCategoryTypeNM")
		SiteCategoryType.SiteCategoryTypeDS.FormValue = ObjForm.GetValue("x_SiteCategoryTypeDS")
		SiteCategoryType.SiteCategoryTypeDS.OldValue = ObjForm.GetValue("o_SiteCategoryTypeDS")
		SiteCategoryType.SiteCategoryComment.FormValue = ObjForm.GetValue("x_SiteCategoryComment")
		SiteCategoryType.SiteCategoryComment.OldValue = ObjForm.GetValue("o_SiteCategoryComment")
		SiteCategoryType.SiteCategoryFileName.FormValue = ObjForm.GetValue("x_SiteCategoryFileName")
		SiteCategoryType.SiteCategoryFileName.OldValue = ObjForm.GetValue("o_SiteCategoryFileName")
		SiteCategoryType.SiteCategoryTransferURL.FormValue = ObjForm.GetValue("x_SiteCategoryTransferURL")
		SiteCategoryType.SiteCategoryTransferURL.OldValue = ObjForm.GetValue("o_SiteCategoryTransferURL")
		SiteCategoryType.DefaultSiteCategoryID.FormValue = ObjForm.GetValue("x_DefaultSiteCategoryID")
		SiteCategoryType.DefaultSiteCategoryID.OldValue = ObjForm.GetValue("o_DefaultSiteCategoryID")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		SiteCategoryType.SiteCategoryTypeID.CurrentValue = SiteCategoryType.SiteCategoryTypeID.FormValue
		SiteCategoryType.SiteCategoryTypeNM.CurrentValue = SiteCategoryType.SiteCategoryTypeNM.FormValue
		SiteCategoryType.SiteCategoryTypeDS.CurrentValue = SiteCategoryType.SiteCategoryTypeDS.FormValue
		SiteCategoryType.SiteCategoryComment.CurrentValue = SiteCategoryType.SiteCategoryComment.FormValue
		SiteCategoryType.SiteCategoryFileName.CurrentValue = SiteCategoryType.SiteCategoryFileName.FormValue
		SiteCategoryType.SiteCategoryTransferURL.CurrentValue = SiteCategoryType.SiteCategoryTransferURL.FormValue
		SiteCategoryType.DefaultSiteCategoryID.CurrentValue = SiteCategoryType.DefaultSiteCategoryID.FormValue
	End Sub

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = SiteCategoryType.KeyFilter

		' Row Selecting event
		SiteCategoryType.Row_Selecting(sFilter)

		' Load SQL based on filter
		SiteCategoryType.CurrentFilter = sFilter
		Dim sSql As String = SiteCategoryType.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				SiteCategoryType.Row_Selected(RsRow)
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
		SiteCategoryType.SiteCategoryTypeID.DbValue = RsRow("SiteCategoryTypeID")
		SiteCategoryType.SiteCategoryTypeNM.DbValue = RsRow("SiteCategoryTypeNM")
		SiteCategoryType.SiteCategoryTypeDS.DbValue = RsRow("SiteCategoryTypeDS")
		SiteCategoryType.SiteCategoryComment.DbValue = RsRow("SiteCategoryComment")
		SiteCategoryType.SiteCategoryFileName.DbValue = RsRow("SiteCategoryFileName")
		SiteCategoryType.SiteCategoryTransferURL.DbValue = RsRow("SiteCategoryTransferURL")
		SiteCategoryType.DefaultSiteCategoryID.DbValue = RsRow("DefaultSiteCategoryID")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		SiteCategoryType.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' SiteCategoryTypeID

		SiteCategoryType.SiteCategoryTypeID.CellCssStyle = ""
		SiteCategoryType.SiteCategoryTypeID.CellCssClass = ""

		' SiteCategoryTypeNM
		SiteCategoryType.SiteCategoryTypeNM.CellCssStyle = ""
		SiteCategoryType.SiteCategoryTypeNM.CellCssClass = ""

		' SiteCategoryTypeDS
		SiteCategoryType.SiteCategoryTypeDS.CellCssStyle = ""
		SiteCategoryType.SiteCategoryTypeDS.CellCssClass = ""

		' SiteCategoryComment
		SiteCategoryType.SiteCategoryComment.CellCssStyle = ""
		SiteCategoryType.SiteCategoryComment.CellCssClass = ""

		' SiteCategoryFileName
		SiteCategoryType.SiteCategoryFileName.CellCssStyle = ""
		SiteCategoryType.SiteCategoryFileName.CellCssClass = ""

		' SiteCategoryTransferURL
		SiteCategoryType.SiteCategoryTransferURL.CellCssStyle = ""
		SiteCategoryType.SiteCategoryTransferURL.CellCssClass = ""

		' DefaultSiteCategoryID
		SiteCategoryType.DefaultSiteCategoryID.CellCssStyle = ""
		SiteCategoryType.DefaultSiteCategoryID.CellCssClass = ""

		'
		'  View  Row
		'

		If SiteCategoryType.RowType = EW_ROWTYPE_VIEW Then ' View row

			' SiteCategoryTypeID
			SiteCategoryType.SiteCategoryTypeID.ViewValue = SiteCategoryType.SiteCategoryTypeID.CurrentValue
			SiteCategoryType.SiteCategoryTypeID.CssStyle = ""
			SiteCategoryType.SiteCategoryTypeID.CssClass = ""
			SiteCategoryType.SiteCategoryTypeID.ViewCustomAttributes = ""

			' SiteCategoryTypeNM
			SiteCategoryType.SiteCategoryTypeNM.ViewValue = SiteCategoryType.SiteCategoryTypeNM.CurrentValue
			SiteCategoryType.SiteCategoryTypeNM.CssStyle = ""
			SiteCategoryType.SiteCategoryTypeNM.CssClass = ""
			SiteCategoryType.SiteCategoryTypeNM.ViewCustomAttributes = ""

			' SiteCategoryTypeDS
			SiteCategoryType.SiteCategoryTypeDS.ViewValue = SiteCategoryType.SiteCategoryTypeDS.CurrentValue
			SiteCategoryType.SiteCategoryTypeDS.CssStyle = ""
			SiteCategoryType.SiteCategoryTypeDS.CssClass = ""
			SiteCategoryType.SiteCategoryTypeDS.ViewCustomAttributes = ""

			' SiteCategoryComment
			SiteCategoryType.SiteCategoryComment.ViewValue = SiteCategoryType.SiteCategoryComment.CurrentValue
			SiteCategoryType.SiteCategoryComment.CssStyle = ""
			SiteCategoryType.SiteCategoryComment.CssClass = ""
			SiteCategoryType.SiteCategoryComment.ViewCustomAttributes = ""

			' SiteCategoryFileName
			SiteCategoryType.SiteCategoryFileName.ViewValue = SiteCategoryType.SiteCategoryFileName.CurrentValue
			SiteCategoryType.SiteCategoryFileName.CssStyle = ""
			SiteCategoryType.SiteCategoryFileName.CssClass = ""
			SiteCategoryType.SiteCategoryFileName.ViewCustomAttributes = ""

			' SiteCategoryTransferURL
			SiteCategoryType.SiteCategoryTransferURL.ViewValue = SiteCategoryType.SiteCategoryTransferURL.CurrentValue
			SiteCategoryType.SiteCategoryTransferURL.CssStyle = ""
			SiteCategoryType.SiteCategoryTransferURL.CssClass = ""
			SiteCategoryType.SiteCategoryTransferURL.ViewCustomAttributes = ""

			' DefaultSiteCategoryID
			If ew_NotEmpty(SiteCategoryType.DefaultSiteCategoryID.CurrentValue) Then
				sSqlWrk = "SELECT [CategoryName], [SiteCategoryTypeID] FROM [SiteCategory] WHERE [SiteCategoryID] = " & ew_AdjustSql(SiteCategoryType.DefaultSiteCategoryID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryTypeID] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					SiteCategoryType.DefaultSiteCategoryID.ViewValue = RsWrk("CategoryName")
					SiteCategoryType.DefaultSiteCategoryID.ViewValue = SiteCategoryType.DefaultSiteCategoryID.ViewValue & ew_ValueSeparator(0) & RsWrk("SiteCategoryTypeID")
				Else
					SiteCategoryType.DefaultSiteCategoryID.ViewValue = SiteCategoryType.DefaultSiteCategoryID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				SiteCategoryType.DefaultSiteCategoryID.ViewValue = System.DBNull.Value
			End If
			SiteCategoryType.DefaultSiteCategoryID.CssStyle = ""
			SiteCategoryType.DefaultSiteCategoryID.CssClass = ""
			SiteCategoryType.DefaultSiteCategoryID.ViewCustomAttributes = ""

			' View refer script
			' SiteCategoryTypeID

			SiteCategoryType.SiteCategoryTypeID.HrefValue = ""

			' SiteCategoryTypeNM
			SiteCategoryType.SiteCategoryTypeNM.HrefValue = ""

			' SiteCategoryTypeDS
			SiteCategoryType.SiteCategoryTypeDS.HrefValue = ""

			' SiteCategoryComment
			SiteCategoryType.SiteCategoryComment.HrefValue = ""

			' SiteCategoryFileName
			SiteCategoryType.SiteCategoryFileName.HrefValue = ""

			' SiteCategoryTransferURL
			SiteCategoryType.SiteCategoryTransferURL.HrefValue = ""

			' DefaultSiteCategoryID
			SiteCategoryType.DefaultSiteCategoryID.HrefValue = ""

		'
		'  Edit Row
		'

		ElseIf SiteCategoryType.RowType = EW_ROWTYPE_EDIT Then ' Edit row

			' SiteCategoryTypeID
			SiteCategoryType.SiteCategoryTypeID.EditCustomAttributes = ""

			' SiteCategoryTypeNM
			SiteCategoryType.SiteCategoryTypeNM.EditCustomAttributes = ""
			SiteCategoryType.SiteCategoryTypeNM.EditValue = ew_HtmlEncode(SiteCategoryType.SiteCategoryTypeNM.CurrentValue)

			' SiteCategoryTypeDS
			SiteCategoryType.SiteCategoryTypeDS.EditCustomAttributes = ""
			SiteCategoryType.SiteCategoryTypeDS.EditValue = ew_HtmlEncode(SiteCategoryType.SiteCategoryTypeDS.CurrentValue)

			' SiteCategoryComment
			SiteCategoryType.SiteCategoryComment.EditCustomAttributes = ""
			SiteCategoryType.SiteCategoryComment.EditValue = ew_HtmlEncode(SiteCategoryType.SiteCategoryComment.CurrentValue)

			' SiteCategoryFileName
			SiteCategoryType.SiteCategoryFileName.EditCustomAttributes = ""
			SiteCategoryType.SiteCategoryFileName.EditValue = ew_HtmlEncode(SiteCategoryType.SiteCategoryFileName.CurrentValue)

			' SiteCategoryTransferURL
			SiteCategoryType.SiteCategoryTransferURL.EditCustomAttributes = ""
			SiteCategoryType.SiteCategoryTransferURL.EditValue = ew_HtmlEncode(SiteCategoryType.SiteCategoryTransferURL.CurrentValue)

			' DefaultSiteCategoryID
			SiteCategoryType.DefaultSiteCategoryID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [SiteCategoryID], [CategoryName], [SiteCategoryTypeID], '' AS SelectFilterFld FROM [SiteCategory]"
			If Convert.ToString(SiteCategoryType.DefaultSiteCategoryID.CurrentValue) = "" Then
				sWhereWrk = "0=1"
			Else
				sWhereWrk = "[SiteCategoryID] = " & ew_AdjustSql(SiteCategoryType.DefaultSiteCategoryID.CurrentValue) & ""
			End If
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryTypeID] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select", ""}) 
			SiteCategoryType.DefaultSiteCategoryID.EditValue = arwrk

			' Edit refer script
			' SiteCategoryTypeID

			SiteCategoryType.SiteCategoryTypeID.HrefValue = ""

			' SiteCategoryTypeNM
			SiteCategoryType.SiteCategoryTypeNM.HrefValue = ""

			' SiteCategoryTypeDS
			SiteCategoryType.SiteCategoryTypeDS.HrefValue = ""

			' SiteCategoryComment
			SiteCategoryType.SiteCategoryComment.HrefValue = ""

			' SiteCategoryFileName
			SiteCategoryType.SiteCategoryFileName.HrefValue = ""

			' SiteCategoryTransferURL
			SiteCategoryType.SiteCategoryTransferURL.HrefValue = ""

			' DefaultSiteCategoryID
			SiteCategoryType.DefaultSiteCategoryID.HrefValue = ""
		End If

		' Row Rendered event
		SiteCategoryType.Row_Rendered()
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
		sFilter = SiteCategoryType.KeyFilter
		SiteCategoryType.CurrentFilter  = sFilter
		sSql = SiteCategoryType.SQL
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
			RsOld = Conn.GetRow(RsEdit)
			RsEdit.Close()

			' SiteCategoryTypeID
			' SiteCategoryTypeNM

			SiteCategoryType.SiteCategoryTypeNM.SetDbValue(SiteCategoryType.SiteCategoryTypeNM.CurrentValue, System.DBNull.Value)
			Rs("SiteCategoryTypeNM") = SiteCategoryType.SiteCategoryTypeNM.DbValue

			' SiteCategoryTypeDS
			SiteCategoryType.SiteCategoryTypeDS.SetDbValue(SiteCategoryType.SiteCategoryTypeDS.CurrentValue, System.DBNull.Value)
			Rs("SiteCategoryTypeDS") = SiteCategoryType.SiteCategoryTypeDS.DbValue

			' SiteCategoryComment
			SiteCategoryType.SiteCategoryComment.SetDbValue(SiteCategoryType.SiteCategoryComment.CurrentValue, System.DBNull.Value)
			Rs("SiteCategoryComment") = SiteCategoryType.SiteCategoryComment.DbValue

			' SiteCategoryFileName
			SiteCategoryType.SiteCategoryFileName.SetDbValue(SiteCategoryType.SiteCategoryFileName.CurrentValue, System.DBNull.Value)
			Rs("SiteCategoryFileName") = SiteCategoryType.SiteCategoryFileName.DbValue

			' SiteCategoryTransferURL
			SiteCategoryType.SiteCategoryTransferURL.SetDbValue(SiteCategoryType.SiteCategoryTransferURL.CurrentValue, System.DBNull.Value)
			Rs("SiteCategoryTransferURL") = SiteCategoryType.SiteCategoryTransferURL.DbValue

			' DefaultSiteCategoryID
			SiteCategoryType.DefaultSiteCategoryID.SetDbValue(SiteCategoryType.DefaultSiteCategoryID.CurrentValue, System.DBNull.Value)
			Rs("DefaultSiteCategoryID") = SiteCategoryType.DefaultSiteCategoryID.DbValue

			' Row Updating event
			bUpdateRow = SiteCategoryType.Row_Updating(RsOld, Rs)
			If bUpdateRow Then
				Try
					SiteCategoryType.Update(Rs)
					EditRow = True
				Catch e As Exception
					If EW_DEBUG_ENABLED Then Throw
					Message = e.Message
					EditRow = False
				End Try
			Else
				If SiteCategoryType.CancelMessage <> "" Then
					Message = SiteCategoryType.CancelMessage
					SiteCategoryType.CancelMessage = ""
				Else
					Message = "Update cancelled"
				End If
				EditRow = False
			End If
		End If

		' Row Updated event
		If EditRow Then
			SiteCategoryType.Row_Updated(RsOld, Rs)
		End If
		If EditRow Then
			WriteAuditTrailOnEdit(RsOld, Rs)
		End If
	End Function

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "SiteCategoryType"
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

	' Write Audit Trail (edit page)
	Sub WriteAuditTrailOnEdit(ByRef RsOld As OrderedDictionary, ByRef RsNew As OrderedDictionary)
		Dim table As String = "SiteCategoryType"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsOld("SiteCategoryTypeID")

		' Write Audit Trail
		Dim filePfx As String = "log"
		Dim action As String = "U"
		Dim curDate As String, curTime As String, id As String, user As String, field As String
		Dim keyvalue As Object, oldvalue As Object, newvalue As Object
		Dim dt As DateTime = Now()
		Dim fld As cField, fldname As String, Modified As Boolean
		curDate = dt.ToString("yyyy/MM/dd")
		curTime = dt.ToString("HH:mm:ss")
		id = HttpContext.Current.Request.ServerVariables("SCRIPT_NAME")
    user = CurrentUserName()
		Dim arFldNames(RsNew.Count - 1) As String
		RsNew.Keys.CopyTo(arFldNames, 0)
		For Each fldname In arFldNames
			fld = SiteCategoryType.FieldByName(fldname)
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
					ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, fldname, keyvalue, oldvalue, newvalue)
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
		SiteCategoryType_edit = New cSiteCategoryType_edit(Me)		
		SiteCategoryType_edit.Page_Init()

		' Page main processing
		SiteCategoryType_edit.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If SiteCategoryType_edit IsNot Nothing Then SiteCategoryType_edit.Dispose()
	End Sub
End Class
