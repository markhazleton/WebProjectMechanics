Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class PageAlias_edit
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public PageAlias_edit As cPageAlias_edit

	'
	' Page Class
	'
	Class cPageAlias_edit
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
				If PageAlias.UseTokenInUrl Then Url = Url & "t=" & PageAlias.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

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

		' PageAlias
		Public Property PageAlias() As cPageAlias
			Get				
				Return ParentPage.PageAlias
			End Get
			Set(ByVal v As cPageAlias)
				ParentPage.PageAlias = v	
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
			m_PageObjName = "PageAlias_edit"
			m_PageObjTypeName = "cPageAlias_edit"

			' Table Name
			m_TableName = "PageAlias"

			' Initialize table object
			PageAlias = New cPageAlias(Me)

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
			PageAlias.Dispose()

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
		If ew_Get("PageAliasID") <> "" Then
			PageAlias.PageAliasID.QueryStringValue = ew_Get("PageAliasID")
		End If

		' Create form object
		ObjForm = New cFormObj
		If ObjForm.GetValue("a_edit") <> "" Then
			PageAlias.CurrentAction = ObjForm.GetValue("a_edit") ' Get action code
			LoadFormValues() ' Get form values

			' Validate Form
			If Not ValidateForm() Then
				PageAlias.CurrentAction = "" ' Form error, reset action
				Message = ParentPage.gsFormError
				LoadRow() ' Restore row
				RestoreFormValues() ' Restore form values if validate failed
			End If
		Else
			PageAlias.CurrentAction = "I" ' Default action is display
		End If

		' Check if valid key
		If ew_Empty(PageAlias.PageAliasID.CurrentValue) Then Page_Terminate("PageAlias_list.aspx") ' Invalid key, return to list
		Select Case PageAlias.CurrentAction
			Case "I" ' Get a record to display
				If Not LoadRow() Then ' Load Record based on key
					Message = "No records found" ' No record found
					Page_Terminate("PageAlias_list.aspx") ' No matching record, return to list
				End If
			Case "U" ' Update
				PageAlias.SendEmail = True ' Send email on update success
				If EditRow() Then ' Update Record based on key
					Message = "Update succeeded" ' Update success
					Dim sReturnUrl As String = PageAlias.ReturnUrl
					If ew_GetPageName(sReturnUrl) = "PageAlias_view.aspx" Then sReturnUrl = PageAlias.ViewUrl ' View paging, return to view page with keyurl directly
					Page_Terminate(sReturnUrl) ' Return to caller
				Else
					LoadRow() ' Restore row
					RestoreFormValues() ' Restore form values if update failed
				End If
		End Select

		' Render the record
		PageAlias.RowType = EW_ROWTYPE_EDIT ' Render as edit

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
		If EW_DEBUG_ENABLED Then ew_Write(sSql)
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

		' Row Rendering event
		PageAlias.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' PageURL

		PageAlias.zPageURL.CellCssStyle = ""
		PageAlias.zPageURL.CellCssClass = ""

		' TargetURL
		PageAlias.TargetURL.CellCssStyle = ""
		PageAlias.TargetURL.CellCssClass = ""

		' AliasType
		PageAlias.AliasType.CellCssStyle = ""
		PageAlias.AliasType.CellCssClass = ""

		' CompanyID
		PageAlias.CompanyID.CellCssStyle = ""
		PageAlias.CompanyID.CellCssClass = ""

		'
		'  View  Row
		'

		If PageAlias.RowType = EW_ROWTYPE_VIEW Then ' View row

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
				sSqlWrk = "SELECT [CompanyName] FROM [Company] WHERE [CompanyID] = " & ew_AdjustSql(PageAlias.CompanyID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
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

			' TargetURL
			PageAlias.TargetURL.HrefValue = ""

			' AliasType
			PageAlias.AliasType.HrefValue = ""

			' CompanyID
			PageAlias.CompanyID.HrefValue = ""

		'
		'  Edit Row
		'

		ElseIf PageAlias.RowType = EW_ROWTYPE_EDIT Then ' Edit row

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
			sSqlWrk = "SELECT [CompanyID], [CompanyName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Company]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			PageAlias.CompanyID.EditValue = arwrk

			' Edit refer script
			' PageURL

			PageAlias.zPageURL.HrefValue = ""

			' TargetURL
			PageAlias.TargetURL.HrefValue = ""

			' AliasType
			PageAlias.AliasType.HrefValue = ""

			' CompanyID
			PageAlias.CompanyID.HrefValue = ""
		End If

		' Row Rendered event
		PageAlias.Row_Rendered()
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
		sFilter = PageAlias.KeyFilter
		PageAlias.CurrentFilter  = sFilter
		sSql = PageAlias.SQL
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

			' PageURL
			PageAlias.zPageURL.SetDbValue(PageAlias.zPageURL.CurrentValue, System.DBNull.Value)
			Rs("PageURL") = PageAlias.zPageURL.DbValue

			' TargetURL
			PageAlias.TargetURL.SetDbValue(PageAlias.TargetURL.CurrentValue, System.DBNull.Value)
			Rs("TargetURL") = PageAlias.TargetURL.DbValue

			' AliasType
			PageAlias.AliasType.SetDbValue(PageAlias.AliasType.CurrentValue, System.DBNull.Value)
			Rs("AliasType") = PageAlias.AliasType.DbValue

			' CompanyID
			PageAlias.CompanyID.SetDbValue(PageAlias.CompanyID.CurrentValue, System.DBNull.Value)
			Rs("CompanyID") = PageAlias.CompanyID.DbValue

			' Row Updating event
			bUpdateRow = PageAlias.Row_Updating(RsOld, Rs)
			If bUpdateRow Then
				Try
					PageAlias.Update(Rs)
					EditRow = True
				Catch e As Exception
					If EW_DEBUG_ENABLED Then Throw
					Message = e.Message
					EditRow = False
				End Try
			Else
				If PageAlias.CancelMessage <> "" Then
					Message = PageAlias.CancelMessage
					PageAlias.CancelMessage = ""
				Else
					Message = "Update cancelled"
				End If
				EditRow = False
			End If
		End If

		' Row Updated event
		If EditRow Then
			PageAlias.Row_Updated(RsOld, Rs)
		End If
		If EditRow Then
			WriteAuditTrailOnEdit(RsOld, Rs)
		End If
	End Function

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "PageAlias"
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
		Dim table As String = "PageAlias"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsOld("PageAliasID")

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
			fld = PageAlias.FieldByName(fldname)
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
		PageAlias_edit = New cPageAlias_edit(Me)		
		PageAlias_edit.Page_Init()

		' Page main processing
		PageAlias_edit.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If PageAlias_edit IsNot Nothing Then PageAlias_edit.Dispose()
	End Sub
End Class
