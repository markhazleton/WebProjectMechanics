Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class pageimage_add
	Inherits AspNetMaker8_wpmWebsite

	' Page object
	Public PageImage_add As cPageImage_add

	'
	' Page Class
	'
	Class cPageImage_add
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
				If PageImage.UseTokenInUrl Then Url = Url & "t=" & PageImage.TableVar & "&" ' Add page token
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

		' ASP.NET page object
		Public ReadOnly Property AspNetPage() As pageimage_add
			Get
				Return CType(m_ParentPage, pageimage_add)
			End Get
		End Property

		' PageImage
		Public Property PageImage() As cPageImage
			Get				
				Return ParentPage.PageImage
			End Get
			Set(ByVal v As cPageImage)
				ParentPage.PageImage = v	
			End Set	
		End Property

		' PageImage
		Public Property Image() As cImage
			Get				
				Return ParentPage.Image
			End Get
			Set(ByVal v As cImage)
				ParentPage.Image = v	
			End Set	
		End Property

		' PageImage
		Public Property zPage() As czPage
			Get				
				Return ParentPage.zPage
			End Get
			Set(ByVal v As czPage)
				ParentPage.zPage = v	
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
			m_PageObjName = "PageImage_add"
			m_PageObjTypeName = "cPageImage_add"			

			' Initialize language object
			Language = New cLanguage(Me)

			' Table Name
			m_TableName = "PageImage"

			' Initialize table object
			PageImage = New cPageImage(Me)
			Image = New cImage(Me)
			zPage = New czPage(Me)

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
			PageImage.Dispose()
			Image.Dispose()
			zPage.Dispose()
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
		If ew_Get("PageImageID") <> "" Then
			PageImage.PageImageID.QueryStringValue = ew_Get("PageImageID")
		Else
			bCopy = False
		End If

		' Set up master detail parameters
		SetUpMasterDetail()

		' Process form if post back
		If ObjForm.GetValue("a_add") <> "" Then
			PageImage.CurrentAction = ObjForm.GetValue("a_add") ' Get form action
			LoadFormValues() ' Load form values

			' Validate Form
			If Not ValidateForm() Then
				PageImage.CurrentAction = "I" ' Form error, reset action
				Message = ParentPage.gsFormError
			End If

		' Not post back
		Else
			If bCopy Then
				PageImage.CurrentAction = "C" ' Copy Record
			Else
				PageImage.CurrentAction = "I" ' Display Blank Record
				LoadDefaultValues() ' Load default values
			End If
		End If

		' Perform action based on action code
		Select Case PageImage.CurrentAction
			Case "I" ' Blank record, no action required
			Case "C" ' Copy an existing record
				If Not LoadRow() Then ' Load record based on key
					Message = Language.Phrase("NoRecord") ' No record found
					Page_Terminate("pageimage_list.aspx") ' No matching record, return to list
				End If
			Case "A" ' Add new record
				PageImage.SendEmail = True ' Send email on add success
				If AddRow() Then ' Add successful
					Message = Language.Phrase("AddSuccess") ' Set up success message
					Dim sReturnUrl As String = PageImage.ReturnUrl
					Page_Terminate(sReturnUrl) ' Clean up and return
				Else
					RestoreFormValues() ' Add failed, restore form values
				End If
		End Select

		' Render row based on row type
		PageImage.RowType = EW_ROWTYPE_ADD ' Render add type

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
		PageImage.zPageID.CurrentValue = 0
		PageImage.ImageID.CurrentValue = 0
		PageImage.PageImagePosition.CurrentValue = 0
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
		If EW_DEBUG_ENABLED Then DebugMsg = sSql
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
		Dim sDetailFilter As String
		PageImage.PageImageID.DbValue = RsRow("PageImageID")
		PageImage.zPageID.DbValue = RsRow("PageID")
		PageImage.ImageID.DbValue = RsRow("ImageID")
		PageImage.PageImagePosition.DbValue = RsRow("PageImagePosition")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Initialize urls
		' Row Rendering event

		PageImage.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' PageID

		PageImage.zPageID.CellCssStyle = ""
		PageImage.zPageID.CellCssClass = ""
		PageImage.zPageID.CellAttrs.Clear(): PageImage.zPageID.ViewAttrs.Clear(): PageImage.zPageID.EditAttrs.Clear()

		' ImageID
		PageImage.ImageID.CellCssStyle = ""
		PageImage.ImageID.CellCssClass = ""
		PageImage.ImageID.CellAttrs.Clear(): PageImage.ImageID.ViewAttrs.Clear(): PageImage.ImageID.EditAttrs.Clear()

		' PageImagePosition
		PageImage.PageImagePosition.CellCssStyle = ""
		PageImage.PageImagePosition.CellCssClass = ""
		PageImage.PageImagePosition.CellAttrs.Clear(): PageImage.PageImagePosition.ViewAttrs.Clear(): PageImage.PageImagePosition.EditAttrs.Clear()

		'
		'  View  Row
		'

		If PageImage.RowType = EW_ROWTYPE_VIEW Then ' View row

			' PageImageID
			PageImage.PageImageID.ViewValue = PageImage.PageImageID.CurrentValue
			PageImage.PageImageID.CssStyle = ""
			PageImage.PageImageID.CssClass = ""
			PageImage.PageImageID.ViewCustomAttributes = ""

			' PageID
			If ew_NotEmpty(PageImage.zPageID.CurrentValue) Then
				sFilterWrk = "[PageID] = " & ew_AdjustSql(PageImage.zPageID.CurrentValue) & ""
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
				sFilterWrk = "[ImageID] = " & ew_AdjustSql(PageImage.ImageID.CurrentValue) & ""
			sSqlWrk = "SELECT [ImageName] FROM [Image]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [ImageName] Asc"
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
			PageImage.zPageID.TooltipValue = ""

			' ImageID
			PageImage.ImageID.HrefValue = ""
			PageImage.ImageID.TooltipValue = ""

			' PageImagePosition
			PageImage.PageImagePosition.HrefValue = ""
			PageImage.PageImagePosition.TooltipValue = ""

		'
		'  Add Row
		'

		ElseIf PageImage.RowType = EW_ROWTYPE_ADD Then ' Add row

			' PageID
			PageImage.zPageID.EditCustomAttributes = ""
			If PageImage.zPageID.SessionValue <> "" Then
				PageImage.zPageID.CurrentValue = PageImage.zPageID.SessionValue
			If ew_NotEmpty(PageImage.zPageID.CurrentValue) Then
				sFilterWrk = "[PageID] = " & ew_AdjustSql(PageImage.zPageID.CurrentValue) & ""
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
			Else
			sFilterWrk = ""
			sSqlWrk = "SELECT [PageID], [PageName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Page]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [PageName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", Language.Phrase("PleaseSelect")}) 
			PageImage.zPageID.EditValue = arwrk
			End If

			' ImageID
			PageImage.ImageID.EditCustomAttributes = ""
			If PageImage.ImageID.SessionValue <> "" Then
				PageImage.ImageID.CurrentValue = PageImage.ImageID.SessionValue
			If ew_NotEmpty(PageImage.ImageID.CurrentValue) Then
				sFilterWrk = "[ImageID] = " & ew_AdjustSql(PageImage.ImageID.CurrentValue) & ""
			sSqlWrk = "SELECT [ImageName] FROM [Image]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [ImageName] Asc"
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
			Else
			sFilterWrk = ""
			sSqlWrk = "SELECT [ImageID], [ImageName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Image]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [ImageName] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", Language.Phrase("PleaseSelect")}) 
			PageImage.ImageID.EditValue = arwrk
			End If

			' PageImagePosition
			PageImage.PageImagePosition.EditCustomAttributes = ""
			PageImage.PageImagePosition.EditValue = ew_HtmlEncode(PageImage.PageImagePosition.CurrentValue)
		End If

		' Row Rendered event
		If PageImage.RowType <> EW_ROWTYPE_AGGREGATEINIT Then
			PageImage.Row_Rendered()
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
		If Not ew_CheckInteger(PageImage.PageImagePosition.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError &= PageImage.PageImagePosition.FldErrMsg
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

			' PageID
			PageImage.zPageID.SetDbValue(Rs, PageImage.zPageID.CurrentValue, System.DBNull.Value, True)

			' ImageID
			PageImage.ImageID.SetDbValue(Rs, PageImage.ImageID.CurrentValue, System.DBNull.Value, True)

			' PageImagePosition
			PageImage.PageImagePosition.SetDbValue(Rs, PageImage.PageImagePosition.CurrentValue, System.DBNull.Value, True)
		Catch e As Exception
			If EW_DEBUG_ENABLED Then Throw
			Message = e.Message
			Return False
		End Try

		' Row Inserting event
		bInsertRow = PageImage.Row_Inserting(Rs)
		If bInsertRow Then
			Try	
				PageImage.Insert(Rs)
				AddRow = True
			Catch e As Exception
				If EW_DEBUG_ENABLED Then Throw
				Message = e.Message				
				AddRow = False
			End Try
		Else
			If PageImage.CancelMessage <> "" Then
				Message = PageImage.CancelMessage
				PageImage.CancelMessage = ""
			Else
				Message = Language.Phrase("InsertCancelled")
			End If
			AddRow = False
		End If
		If AddRow Then
			LastInsertId = Conn.GetLastInsertId()
			PageImage.PageImageID.DbValue = LastInsertId
			Rs("PageImageID") = PageImage.PageImageID.DbValue		

			' Row Inserted event
			PageImage.Row_Inserted(Rs)
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
			If sMasterTblVar = "zPage" Then
				bValidMaster = True
				sDbMasterFilter = PageImage.SqlMasterFilter_zPage
				sDbDetailFilter = PageImage.SqlDetailFilter_zPage
				If ew_Get("zPageID") <> "" Then
					zPage.zPageID.QueryStringValue = ew_Get("zPageID")
					PageImage.zPageID.QueryStringValue = zPage.zPageID.QueryStringValue
					PageImage.zPageID.SessionValue = PageImage.zPageID.QueryStringValue
					If Not IsNumeric(zPage.zPageID.QueryStringValue) Then bValidMaster = False
					sDbMasterFilter = sDbMasterFilter.Replace("@zPageID@", ew_AdjustSql(zPage.zPageID.QueryStringValue))
					sDbDetailFilter = sDbDetailFilter.Replace("@zPageID@", ew_AdjustSql(zPage.zPageID.QueryStringValue))
				Else
					bValidMaster = False
				End If
			End If
			If sMasterTblVar = "Image" Then
				bValidMaster = True
				sDbMasterFilter = PageImage.SqlMasterFilter_Image
				sDbDetailFilter = PageImage.SqlDetailFilter_Image
				If ew_Get("ImageID") <> "" Then
					Image.ImageID.QueryStringValue = ew_Get("ImageID")
					PageImage.ImageID.QueryStringValue = Image.ImageID.QueryStringValue
					PageImage.ImageID.SessionValue = PageImage.ImageID.QueryStringValue
					If Not IsNumeric(Image.ImageID.QueryStringValue) Then bValidMaster = False
					sDbMasterFilter = sDbMasterFilter.Replace("@ImageID@", ew_AdjustSql(Image.ImageID.QueryStringValue))
					sDbDetailFilter = sDbDetailFilter.Replace("@ImageID@", ew_AdjustSql(Image.ImageID.QueryStringValue))
				Else
					bValidMaster = False
				End If
			End If
		End If
		If bValidMaster Then

			' Save current master table
			PageImage.CurrentMasterTable = sMasterTblVar
			PageImage.MasterFilter = sDbMasterFilter ' Set up master filter
			PageImage.DetailFilter = sDbDetailFilter ' Set up detail filter

			' Clear previous master session values
			If sMasterTblVar <> "zPage" Then
				If PageImage.zPageID.QueryStringValue = "" Then PageImage.zPageID.SessionValue = ""
			End If
			If sMasterTblVar <> "Image" Then
				If PageImage.ImageID.QueryStringValue = "" Then PageImage.ImageID.SessionValue = ""
			End If
		End If
	End Sub

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "PageImage"
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
		Dim table As String = "PageImage"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsSrc("PageImageID")

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

		' PageID Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "PageID", keyvalue, oldvalue, RsSrc("PageID"))

		' ImageID Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "ImageID", keyvalue, oldvalue, RsSrc("ImageID"))

		' PageImagePosition Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "PageImagePosition", keyvalue, oldvalue, RsSrc("PageImagePosition"))
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
		PageImage_add = New cPageImage_add(Me)		
		PageImage_add.Page_Init()		

		' Set buffer/cache option
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

		' Page main processing
		PageImage_add.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If PageImage_add IsNot Nothing Then PageImage_add.Dispose()
	End Sub
End Class
