Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class image_delete
	Inherits AspNetMaker8_wpmWebsite

	' Page object
	Public Image_delete As cImage_delete

	'
	' Page Class
	'
	Class cImage_delete
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
				If Image.UseTokenInUrl Then Url = Url & "t=" & Image.TableVar & "&" ' Add page token
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
			If Image.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (Image.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (Image.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

		' ASP.NET page object
		Public ReadOnly Property AspNetPage() As image_delete
			Get
				Return CType(m_ParentPage, image_delete)
			End Get
		End Property

		' Image
		Public Property Image() As cImage
			Get				
				Return ParentPage.Image
			End Get
			Set(ByVal v As cImage)
				ParentPage.Image = v	
			End Set	
		End Property

		' Image
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
			m_PageObjName = "Image_delete"
			m_PageObjTypeName = "cImage_delete"			

			' Initialize language object
			Language = New cLanguage(Me)

			' Table Name
			m_TableName = "Image"

			' Initialize table object
			Image = New cImage(Me)
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
			Image.Dispose()
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
		If ew_Get("ImageID") <> "" Then
			Image.ImageID.QueryStringValue = ew_Get("ImageID")
			If Not IsNumeric(Image.ImageID.QueryStringValue) Then
			Page_Terminate("image_list.aspx") ' Prevent SQL injection, return to list
			End If
			sKey = sKey & Image.ImageID.QueryStringValue
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
		If nKeySelected <= 0 Then Page_Terminate("image_list.aspx") ' No key specified, return to list

		' Build filter
		For i As Integer = 0 to arRecKeys.GetUpperBound(0)
			sKey = arRecKeys(i).Trim()
			sFilter = sFilter & "("

			' Set up key field
			sKeyFld = sKey
			If Not IsNumeric(sKeyFld) Then Page_Terminate("image_list.aspx") ' Prevent SQL injection, return to list
			sFilter &= "[ImageID]=" & ew_AdjustSql(sKeyFld) & " AND "
			If sFilter.EndsWith(" AND ") Then sFilter = sFilter.Substring(0, sFilter.Length-5) & ") OR "
		Next
		If sFilter.EndsWith(" OR ") Then sFilter = sFilter.Substring(0, sFilter.Length-4)

		' Set up filter (SQL WHERE clause)
		' SQL constructor in Image class, Imageinfo.aspx

		Image.CurrentFilter = sFilter

		' Get action
		If ew_Post("a_delete") <> "" Then
			Image.CurrentAction = ew_Post("a_delete")
		Else
			Image.CurrentAction = "I"	' Display record
		End If
		Select Case Image.CurrentAction
			Case "D" ' Delete
				Image.SendEmail = True ' Send email on delete success
				If DeleteRows() Then ' delete rows
					Message = Language.Phrase("DeleteSuccess") ' Set up success message
					Page_Terminate(Image.ReturnUrl) ' Return to caller
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
		sWrkFilter = Image.CurrentFilter

		' Set up filter (SQL WHERE Clause)
		' SQL constructor in Image class, Imageinfo.aspx

		Image.CurrentFilter = sWrkFilter
		sSql = Image.SQL
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
				DeleteRows = Image.Row_Deleting(Row)
				If Not DeleteRows Then Exit For
			Next Row
		End If
		If DeleteRows Then
			sKey = ""
			For Each Row As OrderedDictionary in RsOld
				sThisKey = ""
				If sThisKey <> "" Then sThisKey = sThisKey & EW_COMPOSITE_KEY_SEPARATOR
				sThisKey = sThisKey & Convert.ToString(Row("ImageID"))
				Try
					Image.Delete(Row)
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
			If Image.CancelMessage <> "" Then
				Message = Image.CancelMessage
				Image.CancelMessage = ""
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
				Image.Row_Deleted(Row)
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
		Image.Recordset_Selecting(Image.CurrentFilter)

		' Load list page SQL
		Dim sSql As String = Image.ListSQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then DebugMsg = sSql

		' Count
		lTotalRecs = -1		
		Try
			Dim sCntSql As String = Image.SelectCountSQL

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
		Image.Recordset_Selected(Rs)
		Return Rs
	End Function

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = Image.KeyFilter

		' Row Selecting event
		Image.Row_Selecting(sFilter)

		' Load SQL based on filter
		Image.CurrentFilter = sFilter
		Dim sSql As String = Image.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then DebugMsg = sSql
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				Image.Row_Selected(RsRow)
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
		Image.ImageID.DbValue = RsRow("ImageID")
		Image.ImageName.DbValue = RsRow("ImageName")
		Image.ImageFileName.DbValue = RsRow("ImageFileName")
		Image.ImageThumbFileName.DbValue = RsRow("ImageThumbFileName")
		Image.ImageDescription.DbValue = RsRow("ImageDescription")
		Image.ImageComment.DbValue = RsRow("ImageComment")
		Image.ImageDate.DbValue = RsRow("ImageDate")
		Image.Active.DbValue = IIf(ew_ConvertToBool(RsRow("Active")), "1", "0")
		Image.ModifiedDT.DbValue = RsRow("ModifiedDT")
		Image.VersionNo.DbValue = RsRow("VersionNo")
		Image.ContactID.DbValue = RsRow("ContactID")
		Image.CompanyID.DbValue = RsRow("CompanyID")
		Image.title.DbValue = RsRow("title")
		Image.medium.DbValue = RsRow("medium")
		Image.size.DbValue = RsRow("size")
		Image.price.DbValue = RsRow("price")
		Image.color.DbValue = RsRow("color")
		Image.subject.DbValue = RsRow("subject")
		Image.sold.DbValue = IIf(ew_ConvertToBool(RsRow("sold")), "1", "0")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Initialize urls
		' Row Rendering event

		Image.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' ImageID

		Image.ImageID.CellCssStyle = ""
		Image.ImageID.CellCssClass = ""
		Image.ImageID.CellAttrs.Clear(): Image.ImageID.ViewAttrs.Clear(): Image.ImageID.EditAttrs.Clear()

		' ImageName
		Image.ImageName.CellCssStyle = ""
		Image.ImageName.CellCssClass = ""
		Image.ImageName.CellAttrs.Clear(): Image.ImageName.ViewAttrs.Clear(): Image.ImageName.EditAttrs.Clear()

		' ImageFileName
		Image.ImageFileName.CellCssStyle = ""
		Image.ImageFileName.CellCssClass = ""
		Image.ImageFileName.CellAttrs.Clear(): Image.ImageFileName.ViewAttrs.Clear(): Image.ImageFileName.EditAttrs.Clear()

		' ImageThumbFileName
		Image.ImageThumbFileName.CellCssStyle = ""
		Image.ImageThumbFileName.CellCssClass = ""
		Image.ImageThumbFileName.CellAttrs.Clear(): Image.ImageThumbFileName.ViewAttrs.Clear(): Image.ImageThumbFileName.EditAttrs.Clear()

		' ImageDate
		Image.ImageDate.CellCssStyle = ""
		Image.ImageDate.CellCssClass = ""
		Image.ImageDate.CellAttrs.Clear(): Image.ImageDate.ViewAttrs.Clear(): Image.ImageDate.EditAttrs.Clear()

		' Active
		Image.Active.CellCssStyle = ""
		Image.Active.CellCssClass = ""
		Image.Active.CellAttrs.Clear(): Image.Active.ViewAttrs.Clear(): Image.Active.EditAttrs.Clear()

		' ModifiedDT
		Image.ModifiedDT.CellCssStyle = ""
		Image.ModifiedDT.CellCssClass = ""
		Image.ModifiedDT.CellAttrs.Clear(): Image.ModifiedDT.ViewAttrs.Clear(): Image.ModifiedDT.EditAttrs.Clear()

		' VersionNo
		Image.VersionNo.CellCssStyle = ""
		Image.VersionNo.CellCssClass = ""
		Image.VersionNo.CellAttrs.Clear(): Image.VersionNo.ViewAttrs.Clear(): Image.VersionNo.EditAttrs.Clear()

		' ContactID
		Image.ContactID.CellCssStyle = ""
		Image.ContactID.CellCssClass = ""
		Image.ContactID.CellAttrs.Clear(): Image.ContactID.ViewAttrs.Clear(): Image.ContactID.EditAttrs.Clear()

		' CompanyID
		Image.CompanyID.CellCssStyle = ""
		Image.CompanyID.CellCssClass = ""
		Image.CompanyID.CellAttrs.Clear(): Image.CompanyID.ViewAttrs.Clear(): Image.CompanyID.EditAttrs.Clear()

		' title
		Image.title.CellCssStyle = ""
		Image.title.CellCssClass = ""
		Image.title.CellAttrs.Clear(): Image.title.ViewAttrs.Clear(): Image.title.EditAttrs.Clear()

		' medium
		Image.medium.CellCssStyle = ""
		Image.medium.CellCssClass = ""
		Image.medium.CellAttrs.Clear(): Image.medium.ViewAttrs.Clear(): Image.medium.EditAttrs.Clear()

		' size
		Image.size.CellCssStyle = ""
		Image.size.CellCssClass = ""
		Image.size.CellAttrs.Clear(): Image.size.ViewAttrs.Clear(): Image.size.EditAttrs.Clear()

		' price
		Image.price.CellCssStyle = ""
		Image.price.CellCssClass = ""
		Image.price.CellAttrs.Clear(): Image.price.ViewAttrs.Clear(): Image.price.EditAttrs.Clear()

		' color
		Image.color.CellCssStyle = ""
		Image.color.CellCssClass = ""
		Image.color.CellAttrs.Clear(): Image.color.ViewAttrs.Clear(): Image.color.EditAttrs.Clear()

		' subject
		Image.subject.CellCssStyle = ""
		Image.subject.CellCssClass = ""
		Image.subject.CellAttrs.Clear(): Image.subject.ViewAttrs.Clear(): Image.subject.EditAttrs.Clear()

		' sold
		Image.sold.CellCssStyle = ""
		Image.sold.CellCssClass = ""
		Image.sold.CellAttrs.Clear(): Image.sold.ViewAttrs.Clear(): Image.sold.EditAttrs.Clear()

		'
		'  View  Row
		'

		If Image.RowType = EW_ROWTYPE_VIEW Then ' View row

			' ImageID
			Image.ImageID.ViewValue = Image.ImageID.CurrentValue
			Image.ImageID.CssStyle = ""
			Image.ImageID.CssClass = ""
			Image.ImageID.ViewCustomAttributes = ""

			' ImageName
			Image.ImageName.ViewValue = Image.ImageName.CurrentValue
			Image.ImageName.CssStyle = ""
			Image.ImageName.CssClass = ""
			Image.ImageName.ViewCustomAttributes = ""

			' ImageFileName
			Image.ImageFileName.ViewValue = Image.ImageFileName.CurrentValue
			Image.ImageFileName.CssStyle = ""
			Image.ImageFileName.CssClass = ""
			Image.ImageFileName.ViewCustomAttributes = ""

			' ImageThumbFileName
			Image.ImageThumbFileName.ViewValue = Image.ImageThumbFileName.CurrentValue
			Image.ImageThumbFileName.CssStyle = ""
			Image.ImageThumbFileName.CssClass = ""
			Image.ImageThumbFileName.ViewCustomAttributes = ""

			' ImageDate
			Image.ImageDate.ViewValue = Image.ImageDate.CurrentValue
			Image.ImageDate.CssStyle = ""
			Image.ImageDate.CssClass = ""
			Image.ImageDate.ViewCustomAttributes = ""

			' Active
			If Convert.ToString(Image.Active.CurrentValue) = "1" Then
				Image.Active.ViewValue = "Yes"
			Else
				Image.Active.ViewValue = "No"
			End If
			Image.Active.CssStyle = ""
			Image.Active.CssClass = ""
			Image.Active.ViewCustomAttributes = ""

			' ModifiedDT
			Image.ModifiedDT.ViewValue = Image.ModifiedDT.CurrentValue
			Image.ModifiedDT.CssStyle = ""
			Image.ModifiedDT.CssClass = ""
			Image.ModifiedDT.ViewCustomAttributes = ""

			' VersionNo
			Image.VersionNo.ViewValue = Image.VersionNo.CurrentValue
			Image.VersionNo.CssStyle = ""
			Image.VersionNo.CssClass = ""
			Image.VersionNo.ViewCustomAttributes = ""

			' ContactID
			Image.ContactID.ViewValue = Image.ContactID.CurrentValue
			Image.ContactID.CssStyle = ""
			Image.ContactID.CssClass = ""
			Image.ContactID.ViewCustomAttributes = ""

			' CompanyID
			Image.CompanyID.ViewValue = Image.CompanyID.CurrentValue
			Image.CompanyID.CssStyle = ""
			Image.CompanyID.CssClass = ""
			Image.CompanyID.ViewCustomAttributes = ""

			' title
			Image.title.ViewValue = Image.title.CurrentValue
			Image.title.CssStyle = ""
			Image.title.CssClass = ""
			Image.title.ViewCustomAttributes = ""

			' medium
			Image.medium.ViewValue = Image.medium.CurrentValue
			Image.medium.CssStyle = ""
			Image.medium.CssClass = ""
			Image.medium.ViewCustomAttributes = ""

			' size
			Image.size.ViewValue = Image.size.CurrentValue
			Image.size.CssStyle = ""
			Image.size.CssClass = ""
			Image.size.ViewCustomAttributes = ""

			' price
			Image.price.ViewValue = Image.price.CurrentValue
			Image.price.CssStyle = ""
			Image.price.CssClass = ""
			Image.price.ViewCustomAttributes = ""

			' color
			Image.color.ViewValue = Image.color.CurrentValue
			Image.color.CssStyle = ""
			Image.color.CssClass = ""
			Image.color.ViewCustomAttributes = ""

			' subject
			Image.subject.ViewValue = Image.subject.CurrentValue
			Image.subject.CssStyle = ""
			Image.subject.CssClass = ""
			Image.subject.ViewCustomAttributes = ""

			' sold
			If Convert.ToString(Image.sold.CurrentValue) = "1" Then
				Image.sold.ViewValue = "Yes"
			Else
				Image.sold.ViewValue = "No"
			End If
			Image.sold.CssStyle = ""
			Image.sold.CssClass = ""
			Image.sold.ViewCustomAttributes = ""

			' View refer script
			' ImageID

			Image.ImageID.HrefValue = ""
			Image.ImageID.TooltipValue = ""

			' ImageName
			Image.ImageName.HrefValue = ""
			Image.ImageName.TooltipValue = ""

			' ImageFileName
			Image.ImageFileName.HrefValue = ""
			Image.ImageFileName.TooltipValue = ""

			' ImageThumbFileName
			Image.ImageThumbFileName.HrefValue = ""
			Image.ImageThumbFileName.TooltipValue = ""

			' ImageDate
			Image.ImageDate.HrefValue = ""
			Image.ImageDate.TooltipValue = ""

			' Active
			Image.Active.HrefValue = ""
			Image.Active.TooltipValue = ""

			' ModifiedDT
			Image.ModifiedDT.HrefValue = ""
			Image.ModifiedDT.TooltipValue = ""

			' VersionNo
			Image.VersionNo.HrefValue = ""
			Image.VersionNo.TooltipValue = ""

			' ContactID
			Image.ContactID.HrefValue = ""
			Image.ContactID.TooltipValue = ""

			' CompanyID
			Image.CompanyID.HrefValue = ""
			Image.CompanyID.TooltipValue = ""

			' title
			Image.title.HrefValue = ""
			Image.title.TooltipValue = ""

			' medium
			Image.medium.HrefValue = ""
			Image.medium.TooltipValue = ""

			' size
			Image.size.HrefValue = ""
			Image.size.TooltipValue = ""

			' price
			Image.price.HrefValue = ""
			Image.price.TooltipValue = ""

			' color
			Image.color.HrefValue = ""
			Image.color.TooltipValue = ""

			' subject
			Image.subject.HrefValue = ""
			Image.subject.TooltipValue = ""

			' sold
			Image.sold.HrefValue = ""
			Image.sold.TooltipValue = ""
		End If

		' Row Rendered event
		If Image.RowType <> EW_ROWTYPE_AGGREGATEINIT Then
			Image.Row_Rendered()
		End If
	End Sub

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "Image"
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
		Dim table As String = "Image"
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
		sKey = sKey & RsSrc("ImageID")
		keyvalue = sKey

		' ImageID Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "ImageID", keyvalue, RsSrc("ImageID"), newvalue)

		' ImageName Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "ImageName", keyvalue, RsSrc("ImageName"), newvalue)

		' ImageFileName Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "ImageFileName", keyvalue, RsSrc("ImageFileName"), newvalue)

		' ImageThumbFileName Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "ImageThumbFileName", keyvalue, RsSrc("ImageThumbFileName"), newvalue)

		' ImageDescription Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "ImageDescription", keyvalue, "<MEMO>", newvalue)

		' ImageComment Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "ImageComment", keyvalue, "<MEMO>", newvalue)

		' ImageDate Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "ImageDate", keyvalue, RsSrc("ImageDate"), newvalue)

		' Active Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "Active", keyvalue, RsSrc("Active"), newvalue)

		' ModifiedDT Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "ModifiedDT", keyvalue, RsSrc("ModifiedDT"), newvalue)

		' VersionNo Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "VersionNo", keyvalue, RsSrc("VersionNo"), newvalue)

		' ContactID Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "ContactID", keyvalue, RsSrc("ContactID"), newvalue)

		' CompanyID Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "CompanyID", keyvalue, RsSrc("CompanyID"), newvalue)

		' title Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "title", keyvalue, RsSrc("title"), newvalue)

		' medium Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "medium", keyvalue, RsSrc("medium"), newvalue)

		' size Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "size", keyvalue, RsSrc("size"), newvalue)

		' price Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "price", keyvalue, RsSrc("price"), newvalue)

		' color Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "color", keyvalue, RsSrc("color"), newvalue)

		' subject Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "subject", keyvalue, RsSrc("subject"), newvalue)

		' sold Field
		ew_WriteAuditTrail(filePfx, curDateTime, id, user, action, table, "sold", keyvalue, RsSrc("sold"), newvalue)
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
		Image_delete = New cImage_delete(Me)		
		Image_delete.Page_Init()		

		' Set buffer/cache option
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

		' Page main processing
		Image_delete.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If Image_delete IsNot Nothing Then Image_delete.Dispose()
	End Sub
End Class
