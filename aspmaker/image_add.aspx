<%@ Page EnableViewState="true" Language="VB" ValidateRequest="False" EnableEventValidation="False" Debug="True" MasterPageFile="masterpage.master" %>
<%@ MasterType VirtualPath="masterpage.master" %>
<%@ Import Namespace="PMGEN" %>
<%@ Import Namespace="EW.Data" %>
<%@ Import Namespace="EW.Security" %>
<%@ Import Namespace="EW.Data.Utilities" %>
<%@ Import Namespace="EW.Web" %>
<%@ Register NameSpace="PMGEN" TagPrefix="PMGEN" %>
<script runat="server">
	Dim sUserFilter As String = string.Empty
	Dim key As Imagekey ' record key
	Dim oldrow As Imagerow ' old record data input by user
	Dim newrow As Imagerow ' new record data input by user
	Dim objProfile As TableProfile ' table profile

	' *************************
	' *  Handler for Page Init
	' *************************

	Protected Sub Page_Init(ByVal s As Object, ByVal e As System.EventArgs)
		If Not mhUser.IsAdmin() Then
		  Session("Login_Link") = Request.RawUrl
		  Response.Redirect("/mhweb/login/login.aspx")
		End If
		objProfile = CustomProfile.GetTable(Share.ProjectName, Imageinf.TableVar)
	End Sub

	' *************************
	' *  Handler for Page Load
	' *************************

	Protected Sub Page_Load(ByVal s As Object, ByVal e As System.EventArgs)
		Page.ClientScript.RegisterClientScriptInclude("ewv", "ewv.js")
		Response.Cache.SetCacheability(HttpCacheability.NoCache)
		If (Not Page.IsPostBack) Then
			If (Request.QueryString.Count > 0) Then
				key = New Imagekey()
				Dim messageList As ArrayList = Imageinf.LoadKey(key)
				If (messageList IsNot Nothing) Then key = Nothing ' error, ignore
			End If
		End If
		If (ImageDetailsView.Visible) Then
			RegisterClientID("CtrlID", ImageDetailsView)
		End If
			If (ImageDetailsView.FindControl("x_ImageName") IsNot Nothing) Then
				Page.Form.DefaultFocus = ImageDetailsView.FindControl("x_ImageName").ClientID
			End If
	End Sub

	' **********************************************
	' *  Load Copy Data & Default Data to Controls
	' **********************************************

	Private Sub LoadData()
		Dim source As WebControl = TryCast(ImageDetailsView, WebControl)
		If (oldrow IsNot Nothing) Then ' old record (from recovery) exists, just restore
			RowToControl(oldrow, source, Core.CtrlType.Edit)
			oldrow = Nothing
		ElseIf (key IsNot Nothing) Then ' key loaded, try to copy record
			Dim data As Imagedal = New Imagedal()
			Dim row As Imagerow = data.LoadRow(key, Imageinf.GetUserFilter())
			If (row Is Nothing) Then
				objProfile.Message = "Record not found"
				Response.Redirect(lblReturnUrl.Text)
			End If
			RowToControl(row, source, Core.CtrlType.Edit)
			row = Nothing
		Else 
			Dim row As Imagerow = New Imagerow()
			row.VersionNo = Convert.ToInt32(0) ' set default value
			row.ContactID = Convert.ToInt32(0) ' set default value
			row.CompanyID = Convert.ToInt32(0) ' set default value
			row.sold = Convert.ToBoolean(0) ' set default value
			RowToControl(row, source, Core.CtrlType.Edit)
			row = Nothing
		End If
	End Sub

	' *****************************************
	' *  Handler when Cancel button is clicked
	' *****************************************

	Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
		Response.Redirect(lblReturnUrl.Text)
	End Sub

	' ******************************
	' *  Handler for DetailsView Init
	' ******************************

	Protected Sub ImageDetailsView_Init(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			Imagedal.OpenConnection()
		Catch err As Exception
			lblMessage.Text = err.Message
			pnlMessage.Visible = True
		End Try
	End Sub

	' *************************************
	' *  Handler for DetailsView DataBound
	' *************************************

	Protected Sub ImageDetailsView_DataBound(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim source As DetailsView = ImageDetailsView
		If (ImageDetailsView.CurrentMode = DetailsViewMode.Insert) Then
			LoadData()
		End If
	End Sub

	' *****************************************
	' *  Handler for DetailsView ItemInserting
	' *****************************************

	Protected Sub ImageDetailsView_ItemInserting(ByVal sender As Object, ByVal e As DetailsViewInsertEventArgs)
		Dim source As WebControl = TryCast(sender, WebControl)

		' Validate input values
		Dim messageList As ArrayList = ValidateInputValues(CType(sender, Control), Core.PageType.Add)
		If (messageList IsNot Nothing) Then
			lblMessage.Text = String.Empty
			For Each sMsg As String In messageList
				lblMessage.Text += sMsg + "<br>"
			Next
			pnlMessage.Visible = True
			e.Cancel = True
			Return
		End If
		messageList = CheckDuplicateKey(TryCast(sender,WebControl))
		If (messageList IsNot Nothing) Then
			lblMessage.Text = String.Empty
			For Each sMsg As string In messageList
				lblMessage.Text += sMsg + "<br>"
			Next
			pnlMessage.Visible = True
			e.Cancel = True
			Return
		End If
	End Sub

	' ****************************************
	' *  Handler for DetailsView ItemInserted
	' ****************************************

	Protected Sub ImageDetailsView_ItemInserted(ByVal sender As Object, ByVal e As DetailsViewInsertedEventArgs)
		If (e.AffectedRows = 0) Then

			' Keep in insert mode
			e.KeepInInsertMode = True

			' Get original values
			Dim source As WebControl = TryCast(sender, WebControl)
			If (source Is Nothing) Then Return
			oldrow = New Imagerow()
			ControlToRow(oldrow, source)
		Else
			objProfile.Message = "Add new record successful"
			Response.Redirect(lblReturnUrl.Text)
		End If
	End Sub

	' *********************************
	' *  Handler for DetailsView Unload
	' *********************************

	Protected Sub ImageDetailsView_Unload(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			Imagedal.CloseAndDisposeConnection()
		Catch
		End Try
	End Sub

	' ********************************
	' * Register ClientID of Controls
	' ********************************

	Private Sub RegisterClientID(ByVal sBlockName As String, ByVal control As Control)
		Dim jsString As String = String.Empty
		If (control.FindControl("x_ImageName") IsNot Nothing) Then
			jsString &= "ew.Controls['x_ImageName'] = '" & control.FindControl("x_ImageName").ClientID & "';"
		End If
		If (control.FindControl("x_Active") IsNot Nothing) Then
			jsString &= "ew.Controls['x_Active'] = '" & control.FindControl("x_Active").ClientID & "';"
		End If
		If (control.FindControl("x_ImageDescription") IsNot Nothing) Then
			jsString &= "ew.Controls['x_ImageDescription'] = '" & control.FindControl("x_ImageDescription").ClientID & "';"
		End If
		If (control.FindControl("x_ImageComment") IsNot Nothing) Then
			jsString &= "ew.Controls['x_ImageComment'] = '" & control.FindControl("x_ImageComment").ClientID & "';"
		End If
		If (control.FindControl("x_ImageFileName") IsNot Nothing) Then
			jsString &= "ew.Controls['x_ImageFileName'] = '" & control.FindControl("x_ImageFileName").ClientID & "';"
		End If
		If (control.FindControl("x_ImageThumbFileName") IsNot Nothing) Then
			jsString &= "ew.Controls['x_ImageThumbFileName'] = '" & control.FindControl("x_ImageThumbFileName").ClientID & "';"
		End If
		If (control.FindControl("x_ImageDate") IsNot Nothing) Then
			jsString &= "ew.Controls['x_ImageDate'] = '" & control.FindControl("x_ImageDate").ClientID & "';"
		End If
		If (control.FindControl("x_ModifiedDT") IsNot Nothing) Then
			jsString &= "ew.Controls['x_ModifiedDT'] = '" & control.FindControl("x_ModifiedDT").ClientID & "';"
		End If
		If (control.FindControl("x_VersionNo") IsNot Nothing) Then
			jsString &= "ew.Controls['x_VersionNo'] = '" & control.FindControl("x_VersionNo").ClientID & "';"
		End If
		If (control.FindControl("x_ContactID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_ContactID'] = '" & control.FindControl("x_ContactID").ClientID & "';"
		End If
		If (control.FindControl("x_CompanyID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_CompanyID'] = '" & control.FindControl("x_CompanyID").ClientID & "';"
		End If
		If (control.FindControl("x_title") IsNot Nothing) Then
			jsString &= "ew.Controls['x_title'] = '" & control.FindControl("x_title").ClientID & "';"
		End If
		If (control.FindControl("x_medium") IsNot Nothing) Then
			jsString &= "ew.Controls['x_medium'] = '" & control.FindControl("x_medium").ClientID & "';"
		End If
		If (control.FindControl("x_size") IsNot Nothing) Then
			jsString &= "ew.Controls['x_size'] = '" & control.FindControl("x_size").ClientID & "';"
		End If
		If (control.FindControl("x_price") IsNot Nothing) Then
			jsString &= "ew.Controls['x_price'] = '" & control.FindControl("x_price").ClientID & "';"
		End If
		If (control.FindControl("x_color") IsNot Nothing) Then
			jsString &= "ew.Controls['x_color'] = '" & control.FindControl("x_color").ClientID & "';"
		End If
		If (control.FindControl("x_subject") IsNot Nothing) Then
			jsString &= "ew.Controls['x_subject'] = '" & control.FindControl("x_subject").ClientID & "';"
		End If
		If (control.FindControl("x_sold") IsNot Nothing) Then
			jsString &= "ew.Controls['x_sold'] = '" & control.FindControl("x_sold").ClientID & "';"
		End If
		Page.ClientScript.RegisterClientScriptBlock(System.Type.GetType("System.String"), sBlockName, jsString, True)
	End Sub

	' *****************************************************
	' *  Processing Input Values Before Updating/Inserting
	' *****************************************************

	Private Function ValidateInputValues(ByVal control As Control, ByVal pageType As Core.PageType) As ArrayList
		Dim messageList As ArrayList = New ArrayList()
		DataFormat.SetDateSeparator("/")
		If (pageType <> Core.PageType.Search) Then ' Add/Edit Validation
			Dim x_ImageName As TextBox = TryCast(control.FindControl("x_ImageName"), TextBox)
			If (x_ImageName IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_ImageName.Text)) Then
					messageList.Add("Invalid Value (String): Name")
				ElseIf (String.IsNullOrEmpty(x_ImageName.Text)) Then
					messageList.Add("Please enter required field (String): Name")
				End If
			End If
			Dim x_ImageDescription As TextBox = TryCast(control.FindControl("x_ImageDescription"), TextBox)
			If (x_ImageDescription IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_ImageDescription.Text)) Then
					messageList.Add("Invalid Value (String): Description")
				End If
			End If
			Dim x_ImageComment As TextBox = TryCast(control.FindControl("x_ImageComment"), TextBox)
			If (x_ImageComment IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_ImageComment.Text)) Then
					messageList.Add("Invalid Value (String): Comment")
				End If
			End If
			Dim x_ImageFileName As TextBox = TryCast(control.FindControl("x_ImageFileName"), TextBox)
			If (x_ImageFileName IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_ImageFileName.Text)) Then
					messageList.Add("Invalid Value (String): File URL")
				ElseIf (String.IsNullOrEmpty(x_ImageFileName.Text)) Then
					messageList.Add("Please enter required field (String): File URL")
				End If
			End If
			Dim x_ImageThumbFileName As TextBox = TryCast(control.FindControl("x_ImageThumbFileName"), TextBox)
			If (x_ImageThumbFileName IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_ImageThumbFileName.Text)) Then
					messageList.Add("Invalid Value (String): Thumbnail URL")
				End If
			End If
			Dim x_ImageDate As TextBox = TryCast(control.FindControl("x_ImageDate"), TextBox)
			If (x_ImageDate IsNot Nothing) Then
				If (Not DataFormat.CheckDateTime(x_ImageDate.Text)) Then
					messageList.Add("Invalid Value (DateTime): Created")
				End If
			End If
			Dim x_ModifiedDT As TextBox = TryCast(control.FindControl("x_ModifiedDT"), TextBox)
			If (x_ModifiedDT IsNot Nothing) Then
				If (Not DataFormat.CheckDateTime(x_ModifiedDT.Text)) Then
					messageList.Add("Invalid Value (DateTime): Modified")
				End If
			End If
			Dim x_VersionNo As TextBox = TryCast(control.FindControl("x_VersionNo"), TextBox)
			If (x_VersionNo IsNot Nothing) Then
				If (Not DataFormat.CheckInt32(x_VersionNo.Text)) Then
					messageList.Add("Invalid Value (Int32): Version")
				End If
			End If
			Dim x_ContactID As DropDownList = TryCast(control.FindControl("x_ContactID"), DropDownList)
			If (x_ContactID IsNot Nothing) Then
				If ((x_ContactID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_ContactID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Contact")
				End If
			End If
			Dim x_CompanyID As DropDownList = TryCast(control.FindControl("x_CompanyID"), DropDownList)
			If (x_CompanyID IsNot Nothing) Then
				If ((x_CompanyID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_CompanyID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Company")
				End If
			End If
			Dim x_title As TextBox = TryCast(control.FindControl("x_title"), TextBox)
			If (x_title IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_title.Text)) Then
					messageList.Add("Invalid Value (String): Title")
				End If
			End If
			Dim x_medium As TextBox = TryCast(control.FindControl("x_medium"), TextBox)
			If (x_medium IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_medium.Text)) Then
					messageList.Add("Invalid Value (String): Medium")
				End If
			End If
			Dim x_size As TextBox = TryCast(control.FindControl("x_size"), TextBox)
			If (x_size IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_size.Text)) Then
					messageList.Add("Invalid Value (String): Size")
				End If
			End If
			Dim x_price As TextBox = TryCast(control.FindControl("x_price"), TextBox)
			If (x_price IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_price.Text)) Then
					messageList.Add("Invalid Value (String): Price")
				End If
			End If
			Dim x_color As TextBox = TryCast(control.FindControl("x_color"), TextBox)
			If (x_color IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_color.Text)) Then
					messageList.Add("Invalid Value (String): Color")
				End If
			End If
			Dim x_subject As TextBox = TryCast(control.FindControl("x_subject"), TextBox)
			If (x_subject IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_subject.Text)) Then
					messageList.Add("Invalid Value (String): Subject")
				End If
			End If
		End If
		Return IIf(messageList.Count = 0, Nothing, messageList)
	End Function

	' **********************************************
	' *  Convert Input Control Values to Row Object
	' **********************************************

	Private Sub ControlToRow(ByVal row As Imagerow, ByVal control As WebControl)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		row.SetPageType(Core.PageType.Add) ' set page type = list for update

		' Field ImageName
		Dim x_ImageName As TextBox = TryCast(control.FindControl("x_ImageName"), TextBox)
		If (x_ImageName.Text <> String.Empty) Then row.ImageName = x_ImageName.Text Else row.ImageName = String.Empty

		' Field Active
		Dim x_Active As CheckBox = TryCast(control.FindControl("x_Active"), CheckBox)
		If (x_Active.Checked) Then
			row.Active = True
		Else
			row.Active = False
		End If

		' Field ImageDescription
		Dim x_ImageDescription As TextBox = TryCast(control.FindControl("x_ImageDescription"), TextBox)
		If (x_ImageDescription.Text <> String.Empty) Then row.ImageDescription = x_ImageDescription.Text Else row.ImageDescription = Nothing

		' Field ImageComment
		Dim x_ImageComment As TextBox = TryCast(control.FindControl("x_ImageComment"), TextBox)
		If (x_ImageComment.Text <> String.Empty) Then row.ImageComment = x_ImageComment.Text Else row.ImageComment = Nothing

		' Field ImageFileName
		Dim x_ImageFileName As TextBox = TryCast(control.FindControl("x_ImageFileName"), TextBox)
		If (x_ImageFileName.Text <> String.Empty) Then row.ImageFileName = x_ImageFileName.Text Else row.ImageFileName = String.Empty

		' Field ImageThumbFileName
		Dim x_ImageThumbFileName As TextBox = TryCast(control.FindControl("x_ImageThumbFileName"), TextBox)
		If (x_ImageThumbFileName.Text <> String.Empty) Then row.ImageThumbFileName = x_ImageThumbFileName.Text Else row.ImageThumbFileName = Nothing

		' Field ImageDate
		Dim x_ImageDate As TextBox = TryCast(control.FindControl("x_ImageDate"), TextBox)
		If (x_ImageDate.Text <> String.Empty) Then row.ImageDate = Convert.ToDateTime(DataFormat.UnFormatDateTime(x_ImageDate.Text, 6, "/"c)) Else row.ImageDate = CType(Nothing, Nullable(Of DateTime))

		' Field ModifiedDT
		Dim x_ModifiedDT As TextBox = TryCast(control.FindControl("x_ModifiedDT"), TextBox)
		If (x_ModifiedDT.Text <> String.Empty) Then row.ModifiedDT = Convert.ToDateTime(DataFormat.UnFormatDateTime(x_ModifiedDT.Text, 6, "/"c)) Else row.ModifiedDT = CType(Nothing, Nullable(Of DateTime))

		' Field VersionNo
		Dim x_VersionNo As TextBox = TryCast(control.FindControl("x_VersionNo"), TextBox)
		If (x_VersionNo.Text <> String.Empty) Then row.VersionNo = Convert.ToInt32(x_VersionNo.Text) Else row.VersionNo = CType(Nothing, Nullable(Of Int32))

		' Field ContactID
		Dim x_ContactID As DropDownList = TryCast(control.FindControl("x_ContactID"), DropDownList)
		Dim v_ContactID As String = String.Empty
		If (x_ContactID.SelectedIndex >= 0) Then
			For Each li As ListItem In x_ContactID.Items
				If (li.Selected) Then
					v_ContactID += li.Value
					Exit For
				End If
			Next
		End If
		If (v_ContactID <> String.Empty) Then row.ContactID = Convert.ToInt32(v_ContactID) Else row.ContactID = CType(Nothing, Nullable(Of Int32))

		' Field CompanyID
		Dim x_CompanyID As DropDownList = TryCast(control.FindControl("x_CompanyID"), DropDownList)
		Dim v_CompanyID As String = String.Empty
		If (x_CompanyID.SelectedIndex >= 0) Then
			For Each li As ListItem In x_CompanyID.Items
				If (li.Selected) Then
					v_CompanyID += li.Value
					Exit For
				End If
			Next
		End If
		If (v_CompanyID <> String.Empty) Then row.CompanyID = Convert.ToInt32(v_CompanyID) Else row.CompanyID = CType(Nothing, Nullable(Of Int32))

		' Field title
		Dim x_title As TextBox = TryCast(control.FindControl("x_title"), TextBox)
		If (x_title.Text <> String.Empty) Then row.title = x_title.Text Else row.title = Nothing

		' Field medium
		Dim x_medium As TextBox = TryCast(control.FindControl("x_medium"), TextBox)
		If (x_medium.Text <> String.Empty) Then row.medium = x_medium.Text Else row.medium = Nothing

		' Field size
		Dim x_size As TextBox = TryCast(control.FindControl("x_size"), TextBox)
		If (x_size.Text <> String.Empty) Then row.size = x_size.Text Else row.size = Nothing

		' Field price
		Dim x_price As TextBox = TryCast(control.FindControl("x_price"), TextBox)
		If (x_price.Text <> String.Empty) Then row.price = x_price.Text Else row.price = Nothing

		' Field color
		Dim x_color As TextBox = TryCast(control.FindControl("x_color"), TextBox)
		If (x_color.Text <> String.Empty) Then row.color = x_color.Text Else row.color = Nothing

		' Field subject
		Dim x_subject As TextBox = TryCast(control.FindControl("x_subject"), TextBox)
		If (x_subject.Text <> String.Empty) Then row.subject = x_subject.Text Else row.subject = Nothing

		' Field sold
		Dim x_sold As CheckBox = TryCast(control.FindControl("x_sold"), CheckBox)
		If (x_sold.Checked) Then
			row.sold = True
		Else
			row.sold = False
		End If
	End Sub

	' ****************************
	' *  Check for duplicate Key
	' ****************************

	Private Function CheckDuplicateKey(ByVal control As WebControl) As ArrayList 
		Dim sWhere As String = String.Empty
		Dim messageList As ArrayList = New ArrayList()
		Dim data As Imagedal = New Imagedal()
		Dim newkey As Imagekey = New Imagekey()
		Try
		data = Nothing
		Catch e As Exception
			messageList.Add(e.Message)
		End Try
		Return IIf(messageList.Count = 0, Nothing, messageList)
	End Function

	' ********************************************
	' *  Convert Row Object Value To Input Control
	' ********************************************

	Private Sub RowToControl(ByVal row As Imagerow, ByVal control As WebControl, ByVal ct As Core.CtrlType)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		If (ct = Core.CtrlType.Edit) Then ' set up edit control

			' Field ImageName
			Dim x_ImageName As TextBox = TryCast(control.FindControl("x_ImageName"), TextBox)
			If (row.ImageName IsNot Nothing) Then x_ImageName.Text = row.ImageName.ToString() Else x_ImageName.Text = String.Empty

			' Field Active
			Dim x_Active As CheckBox = TryCast(control.FindControl("x_Active"), CheckBox)
			Dim sActive As String = row.Active.ToString()
			If ((sActive IsNot Nothing) AndAlso sActive <> "") Then
				If (Convert.ToBoolean(sActive)) Then
					x_Active.Checked = True
				Else
					x_Active.Checked = False
				End If
			End If

			' Field ImageDescription
			Dim x_ImageDescription As TextBox = TryCast(control.FindControl("x_ImageDescription"), TextBox)
			If (row.ImageDescription IsNot Nothing) Then x_ImageDescription.Text = row.ImageDescription.ToString() Else x_ImageDescription.Text = String.Empty

			' Field ImageComment
			Dim x_ImageComment As TextBox = TryCast(control.FindControl("x_ImageComment"), TextBox)
			If (row.ImageComment IsNot Nothing) Then x_ImageComment.Text = row.ImageComment.ToString() Else x_ImageComment.Text = String.Empty

			' Field ImageFileName
			Dim x_ImageFileName As TextBox = TryCast(control.FindControl("x_ImageFileName"), TextBox)
			If (row.ImageFileName IsNot Nothing) Then x_ImageFileName.Text = row.ImageFileName.ToString() Else x_ImageFileName.Text = String.Empty

			' Field ImageThumbFileName
			Dim x_ImageThumbFileName As TextBox = TryCast(control.FindControl("x_ImageThumbFileName"), TextBox)
			If (row.ImageThumbFileName IsNot Nothing) Then x_ImageThumbFileName.Text = row.ImageThumbFileName.ToString() Else x_ImageThumbFileName.Text = String.Empty

			' Field ImageDate
			Dim x_ImageDate As TextBox = TryCast(control.FindControl("x_ImageDate"), TextBox)
			If (row.ImageDate.HasValue) Then x_ImageDate.Text = DataFormat.DateTimeFormat(6, "/", row.ImageDate) Else x_ImageDate.Text = String.Empty

			' Field ModifiedDT
			Dim x_ModifiedDT As TextBox = TryCast(control.FindControl("x_ModifiedDT"), TextBox)
			If (row.ModifiedDT.HasValue) Then x_ModifiedDT.Text = DataFormat.DateTimeFormat(6, "/", row.ModifiedDT) Else x_ModifiedDT.Text = String.Empty

			' Field VersionNo
			Dim x_VersionNo As TextBox = TryCast(control.FindControl("x_VersionNo"), TextBox)
			If (row.VersionNo.HasValue) Then x_VersionNo.Text = row.VersionNo.ToString() Else x_VersionNo.Text = String.Empty

			' Field ContactID
			If (control.FindControl("ax_ContactID") IsNot Nothing) Then
				Dim cdd As CascadingDropDown = TryCast(control.FindControl("ax_ContactID"), CascadingDropDown)
				Dim v_ContactID As String
				If (row.ContactID.HasValue) Then v_ContactID = Convert.ToString(row.ContactID) Else v_ContactID = String.Empty
				cdd.SelectedValue = v_ContactID
			End If

			' Field CompanyID
			If (control.FindControl("ax_CompanyID") IsNot Nothing) Then
				Dim cdd As CascadingDropDown = TryCast(control.FindControl("ax_CompanyID"), CascadingDropDown)
				Dim v_CompanyID As String
				If (row.CompanyID.HasValue) Then v_CompanyID = Convert.ToString(row.CompanyID) Else v_CompanyID = String.Empty
				cdd.SelectedValue = v_CompanyID
			End If

			' Field title
			Dim x_title As TextBox = TryCast(control.FindControl("x_title"), TextBox)
			If (row.title IsNot Nothing) Then x_title.Text = row.title.ToString() Else x_title.Text = String.Empty

			' Field medium
			Dim x_medium As TextBox = TryCast(control.FindControl("x_medium"), TextBox)
			If (row.medium IsNot Nothing) Then x_medium.Text = row.medium.ToString() Else x_medium.Text = String.Empty

			' Field size
			Dim x_size As TextBox = TryCast(control.FindControl("x_size"), TextBox)
			If (row.size IsNot Nothing) Then x_size.Text = row.size.ToString() Else x_size.Text = String.Empty

			' Field price
			Dim x_price As TextBox = TryCast(control.FindControl("x_price"), TextBox)
			If (row.price IsNot Nothing) Then x_price.Text = row.price.ToString() Else x_price.Text = String.Empty

			' Field color
			Dim x_color As TextBox = TryCast(control.FindControl("x_color"), TextBox)
			If (row.color IsNot Nothing) Then x_color.Text = row.color.ToString() Else x_color.Text = String.Empty

			' Field subject
			Dim x_subject As TextBox = TryCast(control.FindControl("x_subject"), TextBox)
			If (row.subject IsNot Nothing) Then x_subject.Text = row.subject.ToString() Else x_subject.Text = String.Empty

			' Field sold
			Dim x_sold As CheckBox = TryCast(control.FindControl("x_sold"), CheckBox)
			Dim ssold As String = row.sold.ToString()
			If ((ssold IsNot Nothing) AndAlso ssold <> "") Then
				If (Convert.ToBoolean(ssold)) Then
					x_sold.Checked = True
				Else
					x_sold.Checked = False
				End If
			End If
		End If
	End Sub

	' ********************
	' *  Build Search Parm
	' ********************

	Private Function BuildSearchParm(Byval sFldName As String, ByVal sFldVal As String, ByVal sOprName As String, ByVal sOprVal As String) As String 
		Return "&" + sFldName + "=" + Server.UrlEncode(sFldVal) + _
			 "&" + sOprName + "=" + Server.UrlEncode(sOprVal)
	End Function

	' *********************
	' *  Build Search Parm
	' *********************

	Private Function BuildSearchParm(Byval sFldName As String, ByVal sFldVal As String) As String
		Return "&" + sFldName + "=" + Server.UrlEncode(sFldVal)
	End Function

	' ************************************
	' *  Handler for DataSource Inserting
	' ************************************

	Protected Sub ImageDataSource_Inserting(ByVal sender As Object, ByVal e As ObjectDataSourceMethodEventArgs)
		Dim control As WebControl = ImageDetailsView

		' Set up row object
		Dim row As Imagerow = TryCast(e.InputParameters(0), Imagerow)
		ControlToRow(row, control)
		If (Imagebll.Inserting(row)) Then
		Else
			e.Cancel = True
			If (Not String.IsNullOrEmpty(objProfile.Message)) Then
				lblMessage.Text = objProfile.Message
				pnlMessage.Visible = True
			End If
		End If
	End Sub

	' ***********************************
	' *  Handler for DataSource Inserted
	' ***********************************

	Protected Sub ImageDataSource_Inserted(ByVal sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.Exception IsNot Nothing) Then 
			e.AffectedRows = 0
			e.ExceptionHandled = True
			lblMessage.Text = e.Exception.InnerException.Message
			pnlMessage.Visible = True
		Else
			newrow = TryCast(e.ReturnValue, Imagerow) ' get new row objectinsert method
			Imagebll.Inserted(newrow)
		End If
	End Sub

	' *******************
	' *  Get User Filter
	' *******************

	Private Function GetUserFilter() As String 
		Return Imageinf.GetUserFilter()
	End Function
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server" />
<script type="text/javascript">
<!--
var ew_DateSep = "/"; // set date separator
var ew_FieldSep = "<%= Share.ValueSeparator(0) %>"; // set value separator
var ew_CurrencyDecimalSeparator = "<%=System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator %>";
//-->
</script>
<script type="text/javascript">
<!--
    var ew_DHTMLEditors = [];
//-->
</script>
	<p><span class="aspnetmaker">Add to TABLE: Image</span></p>
	<p>
		<asp:Hyperlink id="lnkList" Text="Back to List" NavigateUrl="image_list.aspx" CssClass="aspnetmaker" runat="server" />
		<asp:Label id="lblReturnUrl" Visible="False" CssClass="aspnetmaker" runat="server">image_list.aspx</asp:Label>
	</p>
	<asp:ValidationSummary id="xevs_Image" CssClass="aspnetmaker" runat="server"
	HeaderText=""
	ShowSummary="False"
	ShowMessageBox="True"
	ForeColor="#FF0000" />
	<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
		<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
	</asp:PlaceHolder>
	<!-- Data Source -->
<asp:ObjectDataSource ID="ImageDataSource"
	TypeName="PMGEN.Imagedal"
	DataObjectTypeName="PMGEN.Imagerow"
	InsertMethod="Insert"
	OnInserting="ImageDataSource_Inserting"
	OnInserted="ImageDataSource_Inserted"
	runat="server">
</asp:ObjectDataSource>
	<asp:DetailsView ID="ImageDetailsView"
		DataKeyNames="ImageID"
		DataSourceID="ImageDataSource"
		DefaultMode="Insert"
		GridLines="None"
		AutoGenerateRows="False" CssClass="ewDetailTable"
		OnInit="ImageDetailsView_Init"
		OnDataBound="ImageDetailsView_DataBound"
		OnItemInserting="ImageDetailsView_ItemInserting"
		OnItemInserted="ImageDetailsView_ItemInserted"
		OnUnload="ImageDetailsView_Unload"
		AllowPaging="True"
		PagerSettings-Mode="NextPreviousFirstLast"
		PagerSettings-Position="Top"
		runat="server">
		<RowStyle CssClass="ewTableRow" />
		<AlternatingRowStyle CssClass="ewTableAltRow" />
		<EditRowStyle />
		<FooterStyle CssClass="ewTableFooter" />
		<PagerStyle CssClass="ewTablePager" />
		<Fields>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_ImageName">Name<span class='ewmsg'>&nbsp;*</span></asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_ImageName" Columns="30" MaxLength="50" CssClass="aspnetmaker" runat="server" />
<asp:RequiredFieldValidator ID="vx_ImageName" CssClass="aspnetmaker" runat="server" ControlToValidate="x_ImageName" ErrorMessage="Please enter required field - Name" ForeColor="Red" Display="None" Text="*" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_Active">Active</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:CheckBox ID="x_Active" CssClass="aspnetmakerlist" runat="server" />
<asp:HiddenField ID="hx_Active" Runat="server" Value='<%# Bind("Active") %>' />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_ImageDescription">Description</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_ImageDescription" TextMode="MultiLine" Rows="10" Columns="60" CssClass="aspnetmaker" runat="server" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_ImageComment">Comment</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_ImageComment" TextMode="MultiLine" Rows="5" Columns="60" CssClass="aspnetmaker" runat="server" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_ImageFileName">File URL<span class='ewmsg'>&nbsp;*</span></asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_ImageFileName" Columns="30" MaxLength="254" CssClass="aspnetmaker" runat="server" />
<asp:RequiredFieldValidator ID="vx_ImageFileName" CssClass="aspnetmaker" runat="server" ControlToValidate="x_ImageFileName" ErrorMessage="Please enter required field - File URL" ForeColor="Red" Display="None" Text="*" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_ImageThumbFileName">Thumbnail URL</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_ImageThumbFileName" Columns="30" MaxLength="254" CssClass="aspnetmaker" runat="server" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_ImageDate">Created</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_ImageDate" CssClass="aspnetmaker" runat="server" />
<ew:USDateValidator ID="vc_ImageDate" ClientValidationFunction="ew_CheckUSDate" CssClass="aspnetmaker" runat="server" ControlToValidate="x_ImageDate" ErrorMessage="Incorrect date, format = mm/dd/yyyy - Created" Display="None" ForeColor="Red" DateSeparator="/" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_ModifiedDT">Modified</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_ModifiedDT" CssClass="aspnetmaker" runat="server" />
<ew:USDateValidator ID="vc_ModifiedDT" ClientValidationFunction="ew_CheckUSDate" CssClass="aspnetmaker" runat="server" ControlToValidate="x_ModifiedDT" ErrorMessage="Incorrect date, format = mm/dd/yyyy - Modified" Display="None" ForeColor="Red" DateSeparator="/" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_VersionNo">Version</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_VersionNo" Columns="30" CssClass="aspnetmaker" runat="server" />
<ew:IntegerValidator ID="vc_VersionNo" ClientValidationFunction="ew_CheckInteger" CssClass="aspnetmaker" runat="server" ControlToValidate="x_VersionNo" ErrorMessage="Incorrect integer - Version" Display="None" ForeColor="Red" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_ContactID">Contact</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:DropDownList ID="x_ContactID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
<ew:CascadingDropDown ID="ax_ContactID" runat="server" EncryptKey="<%$ EWCode: Share.RandomKey %>"
	TargetControlID="x_ContactID" Category="x_ContactID" 
	PromptText="Please Select" ServiceMethod="GetDropDownList" ServicePath="ewWebService.asmx" 
	SqlCommand="SELECT [ContactID], [PrimaryContact] FROM [Contact] WHERE ([CompanyID]=@FILTER_VALUE) ORDER BY [PrimaryContact] Asc"
	ParentCategory="x_CompanyID" ParentControlID="x_CompanyID"
	DisplayFieldNames="PrimaryContact" EnabledDropDown="true" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_CompanyID">Company</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:DropDownList ID="x_CompanyID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
<ew:CascadingDropDown ID="ax_CompanyID" runat="server" EncryptKey="<%$ EWCode: Share.RandomKey %>"
	TargetControlID="x_CompanyID" Category="x_CompanyID" 
	PromptText="Please Select" ServiceMethod="GetDropDownList" ServicePath="ewWebService.asmx" 
	SqlCommand="SELECT [CompanyID], [CompanyName] FROM [Company] ORDER BY [CompanyName] Asc"
	DisplayFieldNames="CompanyName" EnabledDropDown="true" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_title">Title</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_title" Columns="30" MaxLength="50" CssClass="aspnetmaker" runat="server" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_medium">Medium</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_medium" Columns="30" MaxLength="50" CssClass="aspnetmaker" runat="server" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_size">Size</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_size" Columns="30" MaxLength="50" CssClass="aspnetmaker" runat="server" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_price">Price</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_price" Columns="30" MaxLength="255" CssClass="aspnetmaker" runat="server" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_color">Color</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_color" Columns="30" MaxLength="255" CssClass="aspnetmaker" runat="server" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_subject">Subject</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_subject" Columns="30" MaxLength="255" CssClass="aspnetmaker" runat="server" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_sold">Sold</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:CheckBox ID="x_sold" CssClass="aspnetmakerlist" runat="server" />
<asp:HiddenField ID="hx_sold" Runat="server" Value='<%# Bind("sold") %>' />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField ShowHeader="False">
				<ItemStyle CssClass="ewTableFooter" />
				<InsertItemTemplate>
					<table border="0">
						<tr>
							<td><asp:LinkButton ID="btnInsert" runat="server" CausesValidation="True" CommandName="Insert" Text="ADD"></asp:LinkButton></td>
							<td><asp:LinkButton ID="btnCancel" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel" OnClick="btnCancel_Click"></asp:LinkButton></td>
						</tr>
					</table>
					<asp:HiddenField ID="k_ImageID" Runat="server" Value='<%# Bind("ImageID") %>' />
				</InsertItemTemplate>
			</asp:TemplateField>
		</Fields>
	</asp:DetailsView>
</asp:Content>
