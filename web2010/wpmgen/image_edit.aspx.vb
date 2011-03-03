Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class image_edit
	Inherits AspNetMaker8_wpmWebsite

	' Page object
	Public Image_edit As cImage_edit

	'
	' Page Class
	'
	Class cImage_edit
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
		Public ReadOnly Property AspNetPage() As image_edit
			Get
				Return CType(m_ParentPage, image_edit)
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
			m_PageID = "edit"
			m_PageObjName = "Image_edit"
			m_PageObjTypeName = "cImage_edit"			

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

	Public sDbMasterFilter As String, sDbDetailFilter As String

	'
	' Page main processing
	'
	Sub Page_Main()

		' Load key from QueryString
		If ew_Get("ImageID") <> "" Then
			Image.ImageID.QueryStringValue = ew_Get("ImageID")
		End If

		' Set up master detail parameters
		SetUpMasterDetail()
		If ObjForm.GetValue("a_edit") <> "" Then
			Image.CurrentAction = ObjForm.GetValue("a_edit") ' Get action code
			LoadFormValues() ' Get form values

			' Validate Form
			If Not ValidateForm() Then
				Image.CurrentAction = "" ' Form error, reset action
				Message = ParentPage.gsFormError
				Image.EventCancelled = True ' Event cancelled
				LoadRow() ' Restore row
				RestoreFormValues() ' Restore form values if validate failed
			End If
		Else
			Image.CurrentAction = "I" ' Default action is display
		End If

		' Check if valid key
		If ew_Empty(Image.ImageID.CurrentValue) Then Page_Terminate("image_list.aspx") ' Invalid key, return to list
		Select Case Image.CurrentAction
			Case "I" ' Get a record to display
				If Not LoadRow() Then ' Load Record based on key
					Message = Language.Phrase("NoRecord") ' No record found
					Page_Terminate("image_list.aspx") ' No matching record, return to list
				End If
			Case "U" ' Update
				Image.SendEmail = True ' Send email on update success
				If EditRow() Then ' Update Record based on key
					Message = Language.Phrase("UpdateSuccess") ' Update success
					Dim sReturnUrl As String = Image.ReturnUrl
					Page_Terminate(sReturnUrl) ' Return to caller
				Else
					Image.EventCancelled = True ' Event cancelled 
					LoadRow() ' Restore row
					RestoreFormValues() ' Restore form values if update failed
				End If
		End Select

		' Render the record
		Image.RowType = EW_ROWTYPE_EDIT ' Render as edit

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
		Image.ImageID.FormValue = ObjForm.GetValue("x_ImageID")
		Image.ImageID.OldValue = ObjForm.GetValue("o_ImageID")
		Image.ImageName.FormValue = ObjForm.GetValue("x_ImageName")
		Image.ImageName.OldValue = ObjForm.GetValue("o_ImageName")
		Image.ImageFileName.FormValue = ObjForm.GetValue("x_ImageFileName")
		Image.ImageFileName.OldValue = ObjForm.GetValue("o_ImageFileName")
		Image.ImageThumbFileName.FormValue = ObjForm.GetValue("x_ImageThumbFileName")
		Image.ImageThumbFileName.OldValue = ObjForm.GetValue("o_ImageThumbFileName")
		Image.ImageDescription.FormValue = ObjForm.GetValue("x_ImageDescription")
		Image.ImageDescription.OldValue = ObjForm.GetValue("o_ImageDescription")
		Image.ImageComment.FormValue = ObjForm.GetValue("x_ImageComment")
		Image.ImageComment.OldValue = ObjForm.GetValue("o_ImageComment")
		Image.ImageDate.FormValue = ObjForm.GetValue("x_ImageDate")
		Image.ImageDate.CurrentValue = ew_UnFormatDateTime(Image.ImageDate.CurrentValue, 8)
		Image.ImageDate.OldValue = ObjForm.GetValue("o_ImageDate")
		Image.Active.FormValue = ObjForm.GetValue("x_Active")
		Image.Active.OldValue = ObjForm.GetValue("o_Active")
		Image.ModifiedDT.FormValue = ObjForm.GetValue("x_ModifiedDT")
		Image.ModifiedDT.CurrentValue = ew_UnFormatDateTime(Image.ModifiedDT.CurrentValue, 8)
		Image.ModifiedDT.OldValue = ObjForm.GetValue("o_ModifiedDT")
		Image.VersionNo.FormValue = ObjForm.GetValue("x_VersionNo")
		Image.VersionNo.OldValue = ObjForm.GetValue("o_VersionNo")
		Image.ContactID.FormValue = ObjForm.GetValue("x_ContactID")
		Image.ContactID.OldValue = ObjForm.GetValue("o_ContactID")
		Image.CompanyID.FormValue = ObjForm.GetValue("x_CompanyID")
		Image.CompanyID.OldValue = ObjForm.GetValue("o_CompanyID")
		Image.title.FormValue = ObjForm.GetValue("x_title")
		Image.title.OldValue = ObjForm.GetValue("o_title")
		Image.medium.FormValue = ObjForm.GetValue("x_medium")
		Image.medium.OldValue = ObjForm.GetValue("o_medium")
		Image.size.FormValue = ObjForm.GetValue("x_size")
		Image.size.OldValue = ObjForm.GetValue("o_size")
		Image.price.FormValue = ObjForm.GetValue("x_price")
		Image.price.OldValue = ObjForm.GetValue("o_price")
		Image.color.FormValue = ObjForm.GetValue("x_color")
		Image.color.OldValue = ObjForm.GetValue("o_color")
		Image.subject.FormValue = ObjForm.GetValue("x_subject")
		Image.subject.OldValue = ObjForm.GetValue("o_subject")
		Image.sold.FormValue = ObjForm.GetValue("x_sold")
		Image.sold.OldValue = ObjForm.GetValue("o_sold")
	End Sub

	'
	' Restore form values
	'
	Sub RestoreFormValues()
		Image.ImageID.CurrentValue = Image.ImageID.FormValue
		Image.ImageName.CurrentValue = Image.ImageName.FormValue
		Image.ImageFileName.CurrentValue = Image.ImageFileName.FormValue
		Image.ImageThumbFileName.CurrentValue = Image.ImageThumbFileName.FormValue
		Image.ImageDescription.CurrentValue = Image.ImageDescription.FormValue
		Image.ImageComment.CurrentValue = Image.ImageComment.FormValue
		Image.ImageDate.CurrentValue = Image.ImageDate.FormValue
		Image.ImageDate.CurrentValue = ew_UnFormatDateTime(Image.ImageDate.CurrentValue, 8)
		Image.Active.CurrentValue = Image.Active.FormValue
		Image.ModifiedDT.CurrentValue = Image.ModifiedDT.FormValue
		Image.ModifiedDT.CurrentValue = ew_UnFormatDateTime(Image.ModifiedDT.CurrentValue, 8)
		Image.VersionNo.CurrentValue = Image.VersionNo.FormValue
		Image.ContactID.CurrentValue = Image.ContactID.FormValue
		Image.CompanyID.CurrentValue = Image.CompanyID.FormValue
		Image.title.CurrentValue = Image.title.FormValue
		Image.medium.CurrentValue = Image.medium.FormValue
		Image.size.CurrentValue = Image.size.FormValue
		Image.price.CurrentValue = Image.price.FormValue
		Image.color.CurrentValue = Image.color.FormValue
		Image.subject.CurrentValue = Image.subject.FormValue
		Image.sold.CurrentValue = Image.sold.FormValue
	End Sub

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

		' ImageDescription
		Image.ImageDescription.CellCssStyle = ""
		Image.ImageDescription.CellCssClass = ""
		Image.ImageDescription.CellAttrs.Clear(): Image.ImageDescription.ViewAttrs.Clear(): Image.ImageDescription.EditAttrs.Clear()

		' ImageComment
		Image.ImageComment.CellCssStyle = ""
		Image.ImageComment.CellCssClass = ""
		Image.ImageComment.CellAttrs.Clear(): Image.ImageComment.ViewAttrs.Clear(): Image.ImageComment.EditAttrs.Clear()

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

			' ImageDescription
			Image.ImageDescription.ViewValue = Image.ImageDescription.CurrentValue
			Image.ImageDescription.CssStyle = ""
			Image.ImageDescription.CssClass = ""
			Image.ImageDescription.ViewCustomAttributes = ""

			' ImageComment
			Image.ImageComment.ViewValue = Image.ImageComment.CurrentValue
			Image.ImageComment.CssStyle = ""
			Image.ImageComment.CssClass = ""
			Image.ImageComment.ViewCustomAttributes = ""

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

			' ImageDescription
			Image.ImageDescription.HrefValue = ""
			Image.ImageDescription.TooltipValue = ""

			' ImageComment
			Image.ImageComment.HrefValue = ""
			Image.ImageComment.TooltipValue = ""

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

		'
		'  Edit Row
		'

		ElseIf Image.RowType = EW_ROWTYPE_EDIT Then ' Edit row

			' ImageID
			Image.ImageID.EditCustomAttributes = ""
			Image.ImageID.EditValue = Image.ImageID.CurrentValue
			Image.ImageID.CssStyle = ""
			Image.ImageID.CssClass = ""
			Image.ImageID.ViewCustomAttributes = ""

			' ImageName
			Image.ImageName.EditCustomAttributes = ""
			Image.ImageName.EditValue = ew_HtmlEncode(Image.ImageName.CurrentValue)

			' ImageFileName
			Image.ImageFileName.EditCustomAttributes = ""
			Image.ImageFileName.EditValue = ew_HtmlEncode(Image.ImageFileName.CurrentValue)

			' ImageThumbFileName
			Image.ImageThumbFileName.EditCustomAttributes = ""
			Image.ImageThumbFileName.EditValue = ew_HtmlEncode(Image.ImageThumbFileName.CurrentValue)

			' ImageDescription
			Image.ImageDescription.EditCustomAttributes = ""
			Image.ImageDescription.EditValue = ew_HtmlEncode(Image.ImageDescription.CurrentValue)

			' ImageComment
			Image.ImageComment.EditCustomAttributes = ""
			Image.ImageComment.EditValue = ew_HtmlEncode(Image.ImageComment.CurrentValue)

			' ImageDate
			Image.ImageDate.EditCustomAttributes = ""
			Image.ImageDate.EditValue = Image.ImageDate.CurrentValue

			' Active
			Image.Active.EditCustomAttributes = ""

			' ModifiedDT
			Image.ModifiedDT.EditCustomAttributes = ""
			Image.ModifiedDT.EditValue = Image.ModifiedDT.CurrentValue

			' VersionNo
			Image.VersionNo.EditCustomAttributes = ""
			Image.VersionNo.EditValue = ew_HtmlEncode(Image.VersionNo.CurrentValue)

			' ContactID
			Image.ContactID.EditCustomAttributes = ""
			Image.ContactID.EditValue = ew_HtmlEncode(Image.ContactID.CurrentValue)

			' CompanyID
			Image.CompanyID.EditCustomAttributes = ""
			If Image.CompanyID.SessionValue <> "" Then
				Image.CompanyID.CurrentValue = Image.CompanyID.SessionValue
			Image.CompanyID.ViewValue = Image.CompanyID.CurrentValue
			Image.CompanyID.CssStyle = ""
			Image.CompanyID.CssClass = ""
			Image.CompanyID.ViewCustomAttributes = ""
			Else
			Image.CompanyID.EditValue = ew_HtmlEncode(Image.CompanyID.CurrentValue)
			End If

			' title
			Image.title.EditCustomAttributes = ""
			Image.title.EditValue = ew_HtmlEncode(Image.title.CurrentValue)

			' medium
			Image.medium.EditCustomAttributes = ""
			Image.medium.EditValue = ew_HtmlEncode(Image.medium.CurrentValue)

			' size
			Image.size.EditCustomAttributes = ""
			Image.size.EditValue = ew_HtmlEncode(Image.size.CurrentValue)

			' price
			Image.price.EditCustomAttributes = ""
			Image.price.EditValue = ew_HtmlEncode(Image.price.CurrentValue)

			' color
			Image.color.EditCustomAttributes = ""
			Image.color.EditValue = ew_HtmlEncode(Image.color.CurrentValue)

			' subject
			Image.subject.EditCustomAttributes = ""
			Image.subject.EditValue = ew_HtmlEncode(Image.subject.CurrentValue)

			' sold
			Image.sold.EditCustomAttributes = ""

			' Edit refer script
			' ImageID

			Image.ImageID.HrefValue = ""

			' ImageName
			Image.ImageName.HrefValue = ""

			' ImageFileName
			Image.ImageFileName.HrefValue = ""

			' ImageThumbFileName
			Image.ImageThumbFileName.HrefValue = ""

			' ImageDescription
			Image.ImageDescription.HrefValue = ""

			' ImageComment
			Image.ImageComment.HrefValue = ""

			' ImageDate
			Image.ImageDate.HrefValue = ""

			' Active
			Image.Active.HrefValue = ""

			' ModifiedDT
			Image.ModifiedDT.HrefValue = ""

			' VersionNo
			Image.VersionNo.HrefValue = ""

			' ContactID
			Image.ContactID.HrefValue = ""

			' CompanyID
			Image.CompanyID.HrefValue = ""

			' title
			Image.title.HrefValue = ""

			' medium
			Image.medium.HrefValue = ""

			' size
			Image.size.HrefValue = ""

			' price
			Image.price.HrefValue = ""

			' color
			Image.color.HrefValue = ""

			' subject
			Image.subject.HrefValue = ""

			' sold
			Image.sold.HrefValue = ""
		End If

		' Row Rendered event
		If Image.RowType <> EW_ROWTYPE_AGGREGATEINIT Then
			Image.Row_Rendered()
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
		If ew_Empty(Image.ImageName.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError &= Language.Phrase("EnterRequiredField") & " - " & Image.ImageName.FldCaption
		End If
		If ew_Empty(Image.ImageFileName.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError &= Language.Phrase("EnterRequiredField") & " - " & Image.ImageFileName.FldCaption
		End If
		If Not ew_CheckInteger(Image.VersionNo.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError &= Image.VersionNo.FldErrMsg
		End If
		If Not ew_CheckInteger(Image.ContactID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError &= Image.ContactID.FldErrMsg
		End If
		If Not ew_CheckInteger(Image.CompanyID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError &= Image.CompanyID.FldErrMsg
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
		sFilter = Image.KeyFilter
		Image.CurrentFilter  = sFilter
		sSql = Image.SQL
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

				' ImageID
				' ImageName

				Image.ImageName.SetDbValue(Rs, Image.ImageName.CurrentValue, "", False)

				' ImageFileName
				Image.ImageFileName.SetDbValue(Rs, Image.ImageFileName.CurrentValue, "", False)

				' ImageThumbFileName
				Image.ImageThumbFileName.SetDbValue(Rs, Image.ImageThumbFileName.CurrentValue, System.DBNull.Value, False)

				' ImageDescription
				Image.ImageDescription.SetDbValue(Rs, Image.ImageDescription.CurrentValue, System.DBNull.Value, False)

				' ImageComment
				Image.ImageComment.SetDbValue(Rs, Image.ImageComment.CurrentValue, System.DBNull.Value, False)

				' ImageDate
				Image.ImageDate.SetDbValue(Rs, Image.ImageDate.CurrentValue, System.DBNull.Value, False)

				' Active
				Image.Active.SetDbValue(Rs, (Image.Active.CurrentValue <> "" AndAlso Not IsDBNull(Image.Active.CurrentValue)), System.DBNull.Value, False)

				' ModifiedDT
				Image.ModifiedDT.SetDbValue(Rs, Image.ModifiedDT.CurrentValue, System.DBNull.Value, False)

				' VersionNo
				Image.VersionNo.SetDbValue(Rs, Image.VersionNo.CurrentValue, System.DBNull.Value, False)

				' ContactID
				Image.ContactID.SetDbValue(Rs, Image.ContactID.CurrentValue, System.DBNull.Value, False)

				' CompanyID
				Image.CompanyID.SetDbValue(Rs, Image.CompanyID.CurrentValue, System.DBNull.Value, False)

				' title
				Image.title.SetDbValue(Rs, Image.title.CurrentValue, System.DBNull.Value, False)

				' medium
				Image.medium.SetDbValue(Rs, Image.medium.CurrentValue, System.DBNull.Value, False)

				' size
				Image.size.SetDbValue(Rs, Image.size.CurrentValue, System.DBNull.Value, False)

				' price
				Image.price.SetDbValue(Rs, Image.price.CurrentValue, System.DBNull.Value, False)

				' color
				Image.color.SetDbValue(Rs, Image.color.CurrentValue, System.DBNull.Value, False)

				' subject
				Image.subject.SetDbValue(Rs, Image.subject.CurrentValue, System.DBNull.Value, False)

				' sold
				Image.sold.SetDbValue(Rs, (Image.sold.CurrentValue <> "" AndAlso Not IsDBNull(Image.sold.CurrentValue)), System.DBNull.Value, False)
			Catch e As Exception
				If EW_DEBUG_ENABLED Then Throw
				Message = e.Message
				EditRow = False
			End Try
			RsEdit.Close()

			' Row Updating event
			bUpdateRow = Image.Row_Updating(RsOld, Rs)
			If bUpdateRow Then
				Try
					Image.Update(Rs)
					EditRow = True
				Catch e As Exception
					If EW_DEBUG_ENABLED Then Throw
					Message = e.Message
					EditRow = False
				End Try
			Else
				If Image.CancelMessage <> "" Then
					Message = Image.CancelMessage
					Image.CancelMessage = ""
				Else
					Message = Language.Phrase("UpdateCancelled")
				End If
				EditRow = False
			End If
		End If

		' Row Updated event
		If EditRow Then
			Image.Row_Updated(RsOld, Rs)
		End If
		If EditRow Then
			WriteAuditTrailOnEdit(RsOld, Rs)
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
				sDbMasterFilter = Image.SqlMasterFilter_Company
				sDbDetailFilter = Image.SqlDetailFilter_Company
				If ew_Get("CompanyID") <> "" Then
					Company.CompanyID.QueryStringValue = ew_Get("CompanyID")
					Image.CompanyID.QueryStringValue = Company.CompanyID.QueryStringValue
					Image.CompanyID.SessionValue = Image.CompanyID.QueryStringValue
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
			Image.CurrentMasterTable = sMasterTblVar
			Image.MasterFilter = sDbMasterFilter ' Set up master filter
			Image.DetailFilter = sDbDetailFilter ' Set up detail filter

			' Clear previous master session values
			If sMasterTblVar <> "Company" Then
				If Image.CompanyID.QueryStringValue = "" Then Image.CompanyID.SessionValue = ""
			End If
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

	' Write Audit Trail (edit page)
	Sub WriteAuditTrailOnEdit(ByRef RsOld As OrderedDictionary, ByRef RsNew As OrderedDictionary)
		Dim table As String = "Image"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsOld("ImageID")

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
			fld = Image.FieldByName(fldname)
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
		Image_edit = New cImage_edit(Me)		
		Image_edit.Page_Init()		

		' Set buffer/cache option
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

		' Page main processing
		Image_edit.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If Image_edit IsNot Nothing Then Image_edit.Dispose()
	End Sub
End Class
