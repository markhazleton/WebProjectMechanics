Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class Image_edit
	Inherits AspNetMaker7_WPMGen

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

		' Image
		Public Property Image() As cImage
			Get				
				Return ParentPage.Image
			End Get
			Set(ByVal v As cImage)
				ParentPage.Image = v	
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
			m_PageObjName = "Image_edit"
			m_PageObjTypeName = "cImage_edit"

			' Table Name
			m_TableName = "Image"

			' Initialize table object
			Image = New cImage(Me)

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
		If ew_Get("ImageID") <> "" Then
			Image.ImageID.QueryStringValue = ew_Get("ImageID")
		End If

		' Create form object
		ObjForm = New cFormObj
		If ObjForm.GetValue("a_edit") <> "" Then
			Image.CurrentAction = ObjForm.GetValue("a_edit") ' Get action code
			LoadFormValues() ' Get form values

			' Validate Form
			If Not ValidateForm() Then
				Image.CurrentAction = "" ' Form error, reset action
				Message = ParentPage.gsFormError
				LoadRow() ' Restore row
				RestoreFormValues() ' Restore form values if validate failed
			End If
		Else
			Image.CurrentAction = "I" ' Default action is display
		End If

		' Check if valid key
		If ew_Empty(Image.ImageID.CurrentValue) Then Page_Terminate("Image_list.aspx") ' Invalid key, return to list
		Select Case Image.CurrentAction
			Case "I" ' Get a record to display
				If Not LoadRow() Then ' Load Record based on key
					Message = "No records found" ' No record found
					Page_Terminate("Image_list.aspx") ' No matching record, return to list
				End If
			Case "U" ' Update
				Image.SendEmail = True ' Send email on update success
				If EditRow() Then ' Update Record based on key
					Message = "Update succeeded" ' Update success
					Dim sReturnUrl As String = Image.ReturnUrl
					If ew_GetPageName(sReturnUrl) = "Image_view.aspx" Then sReturnUrl = Image.ViewUrl ' View paging, return to view page with keyurl directly
					Page_Terminate(sReturnUrl) ' Return to caller
				Else
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
		Image.CompanyID.FormValue = ObjForm.GetValue("x_CompanyID")
		Image.CompanyID.OldValue = ObjForm.GetValue("o_CompanyID")
		Image.title.FormValue = ObjForm.GetValue("x_title")
		Image.title.OldValue = ObjForm.GetValue("o_title")
		Image.ImageName.FormValue = ObjForm.GetValue("x_ImageName")
		Image.ImageName.OldValue = ObjForm.GetValue("o_ImageName")
		Image.ImageDescription.FormValue = ObjForm.GetValue("x_ImageDescription")
		Image.ImageDescription.OldValue = ObjForm.GetValue("o_ImageDescription")
		Image.ImageComment.FormValue = ObjForm.GetValue("x_ImageComment")
		Image.ImageComment.OldValue = ObjForm.GetValue("o_ImageComment")
		Image.ImageFileName.FormValue = ObjForm.GetValue("x_ImageFileName")
		Image.ImageFileName.OldValue = ObjForm.GetValue("o_ImageFileName")
		Image.ImageThumbFileName.FormValue = ObjForm.GetValue("x_ImageThumbFileName")
		Image.ImageThumbFileName.OldValue = ObjForm.GetValue("o_ImageThumbFileName")
		Image.ImageDate.FormValue = ObjForm.GetValue("x_ImageDate")
		Image.ImageDate.CurrentValue = ew_UnFormatDateTime(Image.ImageDate.CurrentValue, 6)
		Image.ImageDate.OldValue = ObjForm.GetValue("o_ImageDate")
		Image.Active.FormValue = ObjForm.GetValue("x_Active")
		Image.Active.OldValue = ObjForm.GetValue("o_Active")
		Image.ModifiedDT.FormValue = ObjForm.GetValue("x_ModifiedDT")
		Image.ModifiedDT.CurrentValue = ew_UnFormatDateTime(Image.ModifiedDT.CurrentValue, 6)
		Image.ModifiedDT.OldValue = ObjForm.GetValue("o_ModifiedDT")
		Image.VersionNo.FormValue = ObjForm.GetValue("x_VersionNo")
		Image.VersionNo.OldValue = ObjForm.GetValue("o_VersionNo")
		Image.ContactID.FormValue = ObjForm.GetValue("x_ContactID")
		Image.ContactID.OldValue = ObjForm.GetValue("o_ContactID")
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
		Image.CompanyID.CurrentValue = Image.CompanyID.FormValue
		Image.title.CurrentValue = Image.title.FormValue
		Image.ImageName.CurrentValue = Image.ImageName.FormValue
		Image.ImageDescription.CurrentValue = Image.ImageDescription.FormValue
		Image.ImageComment.CurrentValue = Image.ImageComment.FormValue
		Image.ImageFileName.CurrentValue = Image.ImageFileName.FormValue
		Image.ImageThumbFileName.CurrentValue = Image.ImageThumbFileName.FormValue
		Image.ImageDate.CurrentValue = Image.ImageDate.FormValue
		Image.ImageDate.CurrentValue = ew_UnFormatDateTime(Image.ImageDate.CurrentValue, 6)
		Image.Active.CurrentValue = Image.Active.FormValue
		Image.ModifiedDT.CurrentValue = Image.ModifiedDT.FormValue
		Image.ModifiedDT.CurrentValue = ew_UnFormatDateTime(Image.ModifiedDT.CurrentValue, 6)
		Image.VersionNo.CurrentValue = Image.VersionNo.FormValue
		Image.ContactID.CurrentValue = Image.ContactID.FormValue
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
		If EW_DEBUG_ENABLED Then ew_Write(sSql)
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
		Image.ImageID.DbValue = RsRow("ImageID")
		Image.CompanyID.DbValue = RsRow("CompanyID")
		Image.title.DbValue = RsRow("title")
		Image.ImageName.DbValue = RsRow("ImageName")
		Image.ImageDescription.DbValue = RsRow("ImageDescription")
		Image.ImageComment.DbValue = RsRow("ImageComment")
		Image.ImageFileName.DbValue = RsRow("ImageFileName")
		Image.ImageThumbFileName.DbValue = RsRow("ImageThumbFileName")
		Image.ImageDate.DbValue = RsRow("ImageDate")
		Image.Active.DbValue = IIf(ew_ConvertToBool(RsRow("Active")), "1", "0")
		Image.ModifiedDT.DbValue = RsRow("ModifiedDT")
		Image.VersionNo.DbValue = RsRow("VersionNo")
		Image.ContactID.DbValue = RsRow("ContactID")
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

		' Row Rendering event
		Image.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' ImageID

		Image.ImageID.CellCssStyle = ""
		Image.ImageID.CellCssClass = ""

		' CompanyID
		Image.CompanyID.CellCssStyle = ""
		Image.CompanyID.CellCssClass = ""

		' title
		Image.title.CellCssStyle = ""
		Image.title.CellCssClass = ""

		' ImageName
		Image.ImageName.CellCssStyle = ""
		Image.ImageName.CellCssClass = ""

		' ImageDescription
		Image.ImageDescription.CellCssStyle = ""
		Image.ImageDescription.CellCssClass = ""

		' ImageComment
		Image.ImageComment.CellCssStyle = ""
		Image.ImageComment.CellCssClass = ""

		' ImageFileName
		Image.ImageFileName.CellCssStyle = ""
		Image.ImageFileName.CellCssClass = ""

		' ImageThumbFileName
		Image.ImageThumbFileName.CellCssStyle = ""
		Image.ImageThumbFileName.CellCssClass = ""

		' ImageDate
		Image.ImageDate.CellCssStyle = ""
		Image.ImageDate.CellCssClass = ""

		' Active
		Image.Active.CellCssStyle = ""
		Image.Active.CellCssClass = ""

		' ModifiedDT
		Image.ModifiedDT.CellCssStyle = ""
		Image.ModifiedDT.CellCssClass = ""

		' VersionNo
		Image.VersionNo.CellCssStyle = ""
		Image.VersionNo.CellCssClass = ""

		' ContactID
		Image.ContactID.CellCssStyle = ""
		Image.ContactID.CellCssClass = ""

		' medium
		Image.medium.CellCssStyle = ""
		Image.medium.CellCssClass = ""

		' size
		Image.size.CellCssStyle = ""
		Image.size.CellCssClass = ""

		' price
		Image.price.CellCssStyle = ""
		Image.price.CellCssClass = ""

		' color
		Image.color.CellCssStyle = ""
		Image.color.CellCssClass = ""

		' subject
		Image.subject.CellCssStyle = ""
		Image.subject.CellCssClass = ""

		' sold
		Image.sold.CellCssStyle = ""
		Image.sold.CellCssClass = ""

		'
		'  View  Row
		'

		If Image.RowType = EW_ROWTYPE_VIEW Then ' View row

			' ImageID
			Image.ImageID.ViewValue = Image.ImageID.CurrentValue
			Image.ImageID.CssStyle = ""
			Image.ImageID.CssClass = ""
			Image.ImageID.ViewCustomAttributes = ""

			' CompanyID
			If ew_NotEmpty(Image.CompanyID.CurrentValue) Then
				sSqlWrk = "SELECT [CompanyName] FROM [Company] WHERE [CompanyID] = " & ew_AdjustSql(Image.CompanyID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] "
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Image.CompanyID.ViewValue = RsWrk("CompanyName")
				Else
					Image.CompanyID.ViewValue = Image.CompanyID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Image.CompanyID.ViewValue = System.DBNull.Value
			End If
			Image.CompanyID.CssStyle = ""
			Image.CompanyID.CssClass = ""
			Image.CompanyID.ViewCustomAttributes = ""

			' title
			Image.title.ViewValue = Image.title.CurrentValue
			Image.title.CssStyle = ""
			Image.title.CssClass = ""
			Image.title.ViewCustomAttributes = ""

			' ImageName
			Image.ImageName.ViewValue = Image.ImageName.CurrentValue
			Image.ImageName.CssStyle = ""
			Image.ImageName.CssClass = ""
			Image.ImageName.ViewCustomAttributes = ""

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
			Image.ImageDate.ViewValue = ew_FormatDateTime(Image.ImageDate.ViewValue, 6)
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
			Image.ModifiedDT.ViewValue = ew_FormatDateTime(Image.ModifiedDT.ViewValue, 6)
			Image.ModifiedDT.CssStyle = ""
			Image.ModifiedDT.CssClass = ""
			Image.ModifiedDT.ViewCustomAttributes = ""

			' VersionNo
			Image.VersionNo.ViewValue = Image.VersionNo.CurrentValue
			Image.VersionNo.CssStyle = ""
			Image.VersionNo.CssClass = ""
			Image.VersionNo.ViewCustomAttributes = ""

			' ContactID
			If ew_NotEmpty(Image.ContactID.CurrentValue) Then
				sSqlWrk = "SELECT [PrimaryContact] FROM [Contact] WHERE [ContactID] = " & ew_AdjustSql(Image.ContactID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [PrimaryContact] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Image.ContactID.ViewValue = RsWrk("PrimaryContact")
				Else
					Image.ContactID.ViewValue = Image.ContactID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Image.ContactID.ViewValue = System.DBNull.Value
			End If
			Image.ContactID.CssStyle = ""
			Image.ContactID.CssClass = ""
			Image.ContactID.ViewCustomAttributes = ""

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

			' CompanyID
			Image.CompanyID.HrefValue = ""

			' title
			Image.title.HrefValue = ""

			' ImageName
			Image.ImageName.HrefValue = ""

			' ImageDescription
			Image.ImageDescription.HrefValue = ""

			' ImageComment
			Image.ImageComment.HrefValue = ""

			' ImageFileName
			Image.ImageFileName.HrefValue = ""

			' ImageThumbFileName
			Image.ImageThumbFileName.HrefValue = ""

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

		'
		'  Edit Row
		'

		ElseIf Image.RowType = EW_ROWTYPE_EDIT Then ' Edit row

			' ImageID
			Image.ImageID.EditCustomAttributes = ""

			' CompanyID
			Image.CompanyID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [CompanyID], [CompanyName], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Company]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] "
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			Image.CompanyID.EditValue = arwrk

			' title
			Image.title.EditCustomAttributes = ""
			Image.title.EditValue = ew_HtmlEncode(Image.title.CurrentValue)

			' ImageName
			Image.ImageName.EditCustomAttributes = ""
			Image.ImageName.EditValue = ew_HtmlEncode(Image.ImageName.CurrentValue)

			' ImageDescription
			Image.ImageDescription.EditCustomAttributes = ""
			Image.ImageDescription.EditValue = ew_HtmlEncode(Image.ImageDescription.CurrentValue)

			' ImageComment
			Image.ImageComment.EditCustomAttributes = ""
			Image.ImageComment.EditValue = ew_HtmlEncode(Image.ImageComment.CurrentValue)

			' ImageFileName
			Image.ImageFileName.EditCustomAttributes = ""
			Image.ImageFileName.EditValue = ew_HtmlEncode(Image.ImageFileName.CurrentValue)

			' ImageThumbFileName
			Image.ImageThumbFileName.EditCustomAttributes = ""
			Image.ImageThumbFileName.EditValue = ew_HtmlEncode(Image.ImageThumbFileName.CurrentValue)

			' ImageDate
			Image.ImageDate.EditCustomAttributes = ""
			Image.ImageDate.EditValue = ew_FormatDateTime(Image.ImageDate.CurrentValue, 6)

			' Active
			Image.Active.EditCustomAttributes = ""

			' ModifiedDT
			Image.ModifiedDT.EditCustomAttributes = ""
			Image.ModifiedDT.EditValue = ew_FormatDateTime(Image.ModifiedDT.CurrentValue, 6)

			' VersionNo
			Image.VersionNo.EditCustomAttributes = ""
			Image.VersionNo.EditValue = ew_HtmlEncode(Image.VersionNo.CurrentValue)

			' ContactID
			Image.ContactID.EditCustomAttributes = ""
			sSqlWrk = "SELECT [ContactID], [PrimaryContact], '' AS Disp2Fld, '' AS SelectFilterFld FROM [Contact]"
			sWhereWrk = ""
			If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
			sSqlWrk = sSqlWrk & " ORDER BY [PrimaryContact] Asc"
			arwrk = Conn.GetRows(sSqlWrk)
			arwrk.Insert(0, New Object(){"", "Please Select"}) 
			Image.ContactID.EditValue = arwrk

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

			' CompanyID
			Image.CompanyID.HrefValue = ""

			' title
			Image.title.HrefValue = ""

			' ImageName
			Image.ImageName.HrefValue = ""

			' ImageDescription
			Image.ImageDescription.HrefValue = ""

			' ImageComment
			Image.ImageComment.HrefValue = ""

			' ImageFileName
			Image.ImageFileName.HrefValue = ""

			' ImageThumbFileName
			Image.ImageThumbFileName.HrefValue = ""

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
		Image.Row_Rendered()
	End Sub

	'
	' Validate form
	'
	Function ValidateForm() As Boolean

		' Initialize
		ParentPage.gsFormError = ""

		' Check if validation required
		If Not EW_SERVER_VALIDATE Then Return (ParentPage.gsFormError = "")
		If ew_Empty(Image.CompanyID.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter required field - Company"
		End If
		If ew_Empty(Image.ImageName.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter required field - Name"
		End If
		If ew_Empty(Image.ImageFileName.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Please enter required field - File Name"
		End If
		If Not ew_CheckUSDate(Image.ImageDate.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Incorrect date, format = mm/dd/yyyy - Created"
		End If
		If Not ew_CheckUSDate(Image.ModifiedDT.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Incorrect date, format = mm/dd/yyyy - Modified"
		End If
		If Not ew_CheckInteger(Image.VersionNo.FormValue) Then
			If ParentPage.gsFormError <> "" Then ParentPage.gsFormError = ParentPage.gsFormError & "<br />"
			ParentPage.gsFormError = ParentPage.gsFormError & "Incorrect integer - Version No"
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
			RsOld = Conn.GetRow(RsEdit)
			RsEdit.Close()

			' ImageID
			' CompanyID

			Image.CompanyID.SetDbValue(Image.CompanyID.CurrentValue, System.DBNull.Value)
			Rs("CompanyID") = Image.CompanyID.DbValue

			' title
			Image.title.SetDbValue(Image.title.CurrentValue, System.DBNull.Value)
			Rs("title") = Image.title.DbValue

			' ImageName
			Image.ImageName.SetDbValue(Image.ImageName.CurrentValue, "")
			Rs("ImageName") = Image.ImageName.DbValue

			' ImageDescription
			Image.ImageDescription.SetDbValue(Image.ImageDescription.CurrentValue, System.DBNull.Value)
			Rs("ImageDescription") = Image.ImageDescription.DbValue

			' ImageComment
			Image.ImageComment.SetDbValue(Image.ImageComment.CurrentValue, System.DBNull.Value)
			Rs("ImageComment") = Image.ImageComment.DbValue

			' ImageFileName
			Image.ImageFileName.SetDbValue(Image.ImageFileName.CurrentValue, "")
			Rs("ImageFileName") = Image.ImageFileName.DbValue

			' ImageThumbFileName
			Image.ImageThumbFileName.SetDbValue(Image.ImageThumbFileName.CurrentValue, System.DBNull.Value)
			Rs("ImageThumbFileName") = Image.ImageThumbFileName.DbValue

			' ImageDate
			Image.ImageDate.SetDbValue(ew_UnFormatDateTime(Image.ImageDate.CurrentValue, 6), System.DBNull.Value)
			Rs("ImageDate") = Image.ImageDate.DbValue

			' Active
			Image.Active.SetDbValue((Image.Active.CurrentValue <> "" And Not IsDBNull(Image.Active.CurrentValue)), System.DBNull.Value)
			Rs("Active") = Image.Active.DbValue

			' ModifiedDT
			Image.ModifiedDT.SetDbValue(ew_UnFormatDateTime(Image.ModifiedDT.CurrentValue, 6), System.DBNull.Value)
			Rs("ModifiedDT") = Image.ModifiedDT.DbValue

			' VersionNo
			Image.VersionNo.SetDbValue(Image.VersionNo.CurrentValue, System.DBNull.Value)
			Rs("VersionNo") = Image.VersionNo.DbValue

			' ContactID
			Image.ContactID.SetDbValue(Image.ContactID.CurrentValue, System.DBNull.Value)
			Rs("ContactID") = Image.ContactID.DbValue

			' medium
			Image.medium.SetDbValue(Image.medium.CurrentValue, System.DBNull.Value)
			Rs("medium") = Image.medium.DbValue

			' size
			Image.size.SetDbValue(Image.size.CurrentValue, System.DBNull.Value)
			Rs("size") = Image.size.DbValue

			' price
			Image.price.SetDbValue(Image.price.CurrentValue, System.DBNull.Value)
			Rs("price") = Image.price.DbValue

			' color
			Image.color.SetDbValue(Image.color.CurrentValue, System.DBNull.Value)
			Rs("color") = Image.color.DbValue

			' subject
			Image.subject.SetDbValue(Image.subject.CurrentValue, System.DBNull.Value)
			Rs("subject") = Image.subject.DbValue

			' sold
			Image.sold.SetDbValue((Image.sold.CurrentValue <> "" And Not IsDBNull(Image.sold.CurrentValue)), System.DBNull.Value)
			Rs("sold") = Image.sold.DbValue

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
					Message = "Update cancelled"
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

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "Image"
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
		Dim table As String = "Image"

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsOld("ImageID")

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
		Image_edit = New cImage_edit(Me)		
		Image_edit.Page_Init()

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
