Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class PageImage_edit
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public PageImage_edit As cPageImage_edit

	'
	' Page Class
	'
	Class cPageImage_edit
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
				If PageImage.UseTokenInUrl Then Url = Url & "t=" & PageImage.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If PageImage.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (PageImage.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (PageImage.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' PageImage
		Public Property PageImage() As cPageImage
			Get				
				Return ParentPage.PageImage
			End Get
			Set(ByVal v As cPageImage)
				ParentPage.PageImage = v	
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
			m_PageObjName = "PageImage_edit"
			m_PageObjTypeName = "cPageImage_edit"

			' Table Name
			m_TableName = "PageImage"

			' Initialize table object
			PageImage = New cPageImage(Me)

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
			PageImage.Dispose()

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
		If ew_Get("PageImageID") <> "" Then
			PageImage.PageImageID.QueryStringValue = ew_Get("PageImageID")
		End If

		' Create form object
		ObjForm = New cFormObj
		If ObjForm.GetValue("a_edit") <> "" Then
			PageImage.CurrentAction = ObjForm.GetValue("a_edit") ' Get action code
			LoadFormValues() ' Get form values

			' Validate Form
			If Not ValidateForm() Then
				PageImage.CurrentAction = "" ' Form error, reset action
				Message = ParentPage.gsFormError
				LoadRow() ' Restore row
				RestoreFormValues() ' Restore form values if validate failed
			End If
		Else
			PageImage.CurrentAction = "I" ' Default action is display
		End If

		' Check if valid key
		If ew_Empty(PageImage.PageImageID.CurrentValue) Then Page_Terminate("PageImage_list.aspx") ' Invalid key, return to list
		Select Case PageImage.CurrentAction
			Case "I" ' Get a record to display
				If Not LoadRow() Then ' Load Record based on key
					Message = "No records found" ' No record found
					Page_Terminate("PageImage_list.aspx") ' No matching record, return to list
				End If
			Case "U" ' Update
				PageImage.SendEmail = True ' Send email on update success
				If EditRow() Then ' Update Record based on key
					Message = "Update succeeded" ' Update success
					Dim sReturnUrl As String = PageImage.ReturnUrl
					If ew_GetPageName(sReturnUrl) = "PageImage_view.aspx" Then sReturnUrl = PageImage.ViewUrl ' View paging, return to view page with keyurl directly
					Page_Terminate(sReturnUrl) ' Return to caller
				Else
					LoadRow() ' Restore row
					RestoreFormValues() ' Restore form values if update failed
				End If
		End Select

		' Render the record
		PageImage.RowType = EW_ROWTYPE_EDIT ' Render as edit

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
		PageImage.zPageID.FormValue = ObjForm.GetValue("x_zPageID")
		PageImage.zPageID.OldValue = ObjForm.GetValue("o_zPageID")
		PageImage.ImageID.FormValue = ObjForm.GetValue("x_ImageID")
		PageImage.ImageID.OldValue = ObjForm.GetValue("o_ImageID")
		PageImage.PageImagePosition.FormValue = ObjForm.GetValue("x_PageImagePosition")
		PageImage.PageImagePosition.OldValue = ObjForm.GetValue("o_PageImagePosition")
		PageImage.PageImageID.FormValue = ObjForm.GetValue("x_PageImageID")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		PageImage.zPageID.CurrentValue = PageImage.zPageID.FormValue
		PageImage.ImageID.CurrentValue = PageImage.ImageID.FormValue
		PageImage.PageImagePosition.CurrentValue = PageImage.PageImagePosition.FormValue
		PageImage.PageImageID.CurrentValue = PageImage.PageImageID.FormValue
	End Sub

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = PageImage.KeyFilter

		' Row Selecting event
		PageImage.Row_Selecting(sFilter)

		' Load SQL based on filter
		PageImage.CurrentFilter = sFilter
		Dim sSql As String = PageImage.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				PageImage.Row_Selected(RsRow)
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
		PageImage.PageImageID.DbValue = RsRow("PageImageID")
		PageImage.zPageID.DbValue = RsRow("PageID")
		PageImage.ImageID.DbValue = RsRow("ImageID")
		PageImage.PageImagePosition.DbValue = RsRow("PageImagePosition")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		PageImage.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' PageID

		PageImage.zPageID.CellCssStyle = ""
		PageImage.zPageID.CellCssClass = ""

		' ImageID
		PageImage.ImageID.CellCssStyle = ""
		PageImage.ImageID.CellCssClass = ""

		' PageImagePosition
		PageImage.PageImagePosition.CellCssStyle = ""
		PageImage.PageImagePosition.CellCssClass = ""

		'
		'  View  Row
		'

		If PageImage.RowType = EW_ROWTYPE_VIEW Then ' View row

			' PageID
			If ew_NotEmpty(PageImage.zPageID.CurrentValue) Then
				sSqlWrk = "SELECT [PageName] FROM [Page] WHERE [PageID] = " & ew_AdjustSql(PageImage.zPageID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " AND (" & "[CompanyID]=" & httpContext.Current.Session("CompanyID") & " " & ")"
				sSqlWrk = sSqlWrk & " ORDER BY [PageName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					PageImage.zPageID.ViewValue = RsWrk("PageName")
				Else
					PageImage.zPageID.ViewValue = PageImage.zPageID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				PageImage.zPageID.ViewValue = System.DBNull.Value
			End If
			PageImage.zPageID.CssStyle = ""
			PageImage.zPageID.CssClass = ""
			PageImage.zPageID.ViewCustomAttributes = ""

			' ImageID
			If ew_NotEmpty(PageImage.ImageID.CurrentValue) Then
				sSqlWrk = "SELECT [ImageName] FROM [Image] WHERE [ImageID] = " & ew_AdjustSql(PageImage.ImageID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " AND (" & "[CompanyID]=" & httpContext.Current.Session("CompanyID") & " " & ")"
				sSqlWrk = sSqlWrk & " ORDER BY [ImageName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					PageImage.ImageID.ViewValue = RsWrk("ImageName")
				Else
					PageImage.ImageID.ViewValue = PageImage.ImageID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				PageImage.ImageID.ViewValue = System.DBNull.Value
			End If
			PageImage.ImageID.CssStyle = ""
			PageImage.ImageID.CssClass = ""
			PageImage.ImageID.ViewCustomAttributes = ""

			' PageImagePosition
			PageImage.PageImagePosition.ViewValue = PageImage.PageImagePosition.CurrentValue
			PageImage.PageImagePosition.CssStyle = ""
			PageImage.PageImagePosition.CssClass = ""
			PageImage.PageImagePosition.ViewCustomAttributes = ""

			' View refer script
			' PageID

			PageImage.zPageID.HrefValue = ""

			' ImageID
			PageImage.ImageID.HrefValue = ""

			' PageImagePosition
			PageImage.PageImagePosition.HrefValue = ""

		'
		'  Edit Row
		'

		ElseIf PageImage.RowType = EW_ROWTYPE_EDIT Then ' Edit row

			' PageID
			PageImage.zPageID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [PageID], [PageName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Page]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sWhereWrk = "(" & sWhereWrk & ") AND "
			sWhereWrk = sWhereWrk & "(" & "[CompanyID]=" & httpContext.Current.Session("CompanyID") & " " & ")"
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [PageName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			PageImage.zPageID.EditValue = arwrk

			' ImageID
			PageImage.ImageID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [ImageID], [ImageName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Image]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sWhereWrk = "(" & sWhereWrk & ") AND "
			sWhereWrk = sWhereWrk & "(" & "[CompanyID]=" & httpContext.Current.Session("CompanyID") & " " & ")"
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [ImageName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			PageImage.ImageID.EditValue = arwrk

			' PageImagePosition
			PageImage.PageImagePosition.EditCustomAttributes = ""
			PageImage.PageImagePosition.EditValue = ew_HtmlEncode(PageImage.PageImagePosition.CurrentValue)

			' Edit refer script
			' PageID

			PageImage.zPageID.HrefValue = ""

			' ImageID
			PageImage.ImageID.HrefValue = ""

			' PageImagePosition
			PageImage.PageImagePosition.HrefValue = ""
		End If

		' Row Rendered event
		PageImage.Row_Rendered()
	End Sub

	'
	' Validate form
	'
	Function ValidateForm() As Boolean

		' Initialize
		ParentPage.gsFormError = ""

		' Check if validation required
		If Not EW_SERVER_VALIDATE Then Return (ParentPage.gsFormError = "")
		If ew_Empty(PageImage.zPageID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter required field - PageName"
		End If
		If ew_Empty(PageImage.ImageID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter required field - ImageName"
		End If
		If Not ew_CheckInteger(PageImage.PageImagePosition.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Incorrect integer - Page Image Position"
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
		sFilter = PageImage.KeyFilter
		PageImage.CurrentFilter  = sFilter
		sSql = PageImage.SQL
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

			' PageID
			PageImage.zPageID.SetDbValue(PageImage.zPageID.CurrentValue, System.DBNull.Value)
			Rs("PageID") = PageImage.zPageID.DbValue

			' ImageID
			PageImage.ImageID.SetDbValue(PageImage.ImageID.CurrentValue, System.DBNull.Value)
			Rs("ImageID") = PageImage.ImageID.DbValue

			' PageImagePosition
			PageImage.PageImagePosition.SetDbValue(PageImage.PageImagePosition.CurrentValue, System.DBNull.Value)
			Rs("PageImagePosition") = PageImage.PageImagePosition.DbValue

			' Row Updating event
			bUpdateRow = PageImage.Row_Updating(RsOld, Rs)
			If bUpdateRow Then
				Try
					PageImage.Update(Rs)
					EditRow = True
				Catch e As Exception
					If EW_DEBUG_ENABLED Then Throw
					Message = e.Message
					EditRow = False
				End Try
			Else
				If PageImage.CancelMessage <> "" Then
					Message = PageImage.CancelMessage
					PageImage.CancelMessage = ""
				Else
					Message = "Update cancelled"
				End If
				EditRow = False
			End If
		End If

		' Row Updated event
		If EditRow Then
			PageImage.Row_Updated(RsOld, Rs)
		End If
		If EditRow Then
			WriteAuditTrailOnEdit(RsOld, Rs)
		End If
	End Function

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "PageImage"
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
		Dim table As String = "PageImage"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsOld("PageImageID")

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
			fld = PageImage.FieldByName(fldname)
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
		PageImage_edit = New cPageImage_edit(Me)		
		PageImage_edit.Page_Init()

		' Page main processing
		PageImage_edit.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If PageImage_edit IsNot Nothing Then PageImage_edit.Dispose()
	End Sub
End Class
