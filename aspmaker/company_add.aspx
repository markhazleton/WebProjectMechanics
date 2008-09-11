<%@ Page EnableViewState="true" Language="VB" ValidateRequest="False" Debug="True" MasterPageFile="masterpage.master" %>
<%@ MasterType VirtualPath="masterpage.master" %>
<%@ Import Namespace="PMGEN" %>
<%@ Import Namespace="EW.Data" %>
<%@ Import Namespace="EW.Security" %>
<%@ Import Namespace="EW.Data.Utilities" %>
<%@ Import Namespace="EW.Web" %>
<%@ Register NameSpace="PMGEN" TagPrefix="PMGEN" %>
<script runat="server">
	Dim sUserFilter As String = string.Empty
	Dim key As Companykey ' record key
	Dim oldrow As Companyrow ' old record data input by user
	Dim newrow As Companyrow ' new record data input by user
	Dim objProfile As TableProfile ' table profile

	' *************************
	' *  Handler for Page Init
	' *************************

	Protected Sub Page_Init(ByVal s As Object, ByVal e As System.EventArgs)
		If Not mhUser.IsAdmin() Then
		  Session("Login_Link") = Request.RawUrl
		  Response.Redirect("/mhweb/login/login.aspx")
		End If
		objProfile = CustomProfile.GetTable(Share.ProjectName, Companyinf.TableVar)
	End Sub

	' *************************
	' *  Handler for Page Load
	' *************************

	Protected Sub Page_Load(ByVal s As Object, ByVal e As System.EventArgs)
		Page.ClientScript.RegisterClientScriptInclude("ewv", "ewv.js")
		Response.Cache.SetCacheability(HttpCacheability.NoCache)
		If (Not Page.IsPostBack) Then
		End If
		If (CompanyDetailsView.Visible) Then
			RegisterClientID("CtrlID", CompanyDetailsView)
		End If
			If (CompanyDetailsView.FindControl("x_CompanyName") IsNot Nothing) Then
				Page.Form.DefaultFocus = CompanyDetailsView.FindControl("x_CompanyName").ClientID
			End If
	End Sub

	' **********************************************
	' *  Load Copy Data & Default Data to Controls
	' **********************************************

	Private Sub LoadData()
		Dim source As WebControl = TryCast(CompanyDetailsView, WebControl)
		If (oldrow IsNot Nothing) Then ' old record (from recovery) exists, just restore
			RowToControl(oldrow, source, Core.CtrlType.Edit)
			oldrow = Nothing
		ElseIf (key IsNot Nothing) Then ' key loaded, try to copy record
			Dim data As Companydal = New Companydal()
			Dim row As Companyrow = data.LoadRow(key, Companyinf.GetUserFilter())
			If (row Is Nothing) Then
				objProfile.Message = "Record not found"
				Response.Redirect(lblReturnUrl.Text)
			End If
			RowToControl(row, source, Core.CtrlType.Edit)
			row = Nothing
		Else 
			Dim row As Companyrow = New Companyrow()
			row.DefaultArticleID = Convert.ToInt32(0) ' set default value
			row.HomePageID = Convert.ToInt32(0) ' set default value
			row.UseBreadCrumbURL = Convert.ToBoolean(0) ' set default value
			row.SingleSiteGallery = Convert.ToBoolean(0) ' set default value
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

	Protected Sub CompanyDetailsView_Init(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			Companydal.OpenConnection()
		Catch err As Exception
			lblMessage.Text = err.Message
			pnlMessage.Visible = True
		End Try
	End Sub

	' *************************************
	' *  Handler for DetailsView DataBound
	' *************************************

	Protected Sub CompanyDetailsView_DataBound(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim source As DetailsView = CompanyDetailsView
		If (CompanyDetailsView.CurrentMode = DetailsViewMode.Insert) Then
			LoadData()
		End If
	End Sub

	' *****************************************
	' *  Handler for DetailsView ItemInserting
	' *****************************************

	Protected Sub CompanyDetailsView_ItemInserting(ByVal sender As Object, ByVal e As DetailsViewInsertEventArgs)
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

	Protected Sub CompanyDetailsView_ItemInserted(ByVal sender As Object, ByVal e As DetailsViewInsertedEventArgs)
		If (e.AffectedRows = 0) Then

			' Keep in insert mode
			e.KeepInInsertMode = True

			' Get original values
			Dim source As WebControl = TryCast(sender, WebControl)
			If (source Is Nothing) Then Return
			oldrow = New Companyrow()
			ControlToRow(oldrow, source)
		Else
			objProfile.Message = "Add new record successful"
			Response.Redirect(lblReturnUrl.Text)
		End If
	End Sub

	' *********************************
	' *  Handler for DetailsView Unload
	' *********************************

	Protected Sub CompanyDetailsView_Unload(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			Companydal.CloseAndDisposeConnection()
		Catch
		End Try
	End Sub

	' ********************************
	' * Register ClientID of Controls
	' ********************************

	Private Sub RegisterClientID(ByVal sBlockName As String, ByVal control As Control)
		Dim jsString As String = String.Empty
		If (control.FindControl("x_CompanyName") IsNot Nothing) Then
			jsString &= "ew.Controls['x_CompanyName'] = '" & control.FindControl("x_CompanyName").ClientID & "';"
		End If
		If (control.FindControl("x_SiteTitle") IsNot Nothing) Then
			jsString &= "ew.Controls['x_SiteTitle'] = '" & control.FindControl("x_SiteTitle").ClientID & "';"
		End If
		If (control.FindControl("x_SiteTemplate") IsNot Nothing) Then
			jsString &= "ew.Controls['x_SiteTemplate'] = '" & control.FindControl("x_SiteTemplate").ClientID & "';"
		End If
		If (control.FindControl("x_DefaultSiteTemplate") IsNot Nothing) Then
			jsString &= "ew.Controls['x_DefaultSiteTemplate'] = '" & control.FindControl("x_DefaultSiteTemplate").ClientID & "';"
		End If
		If (control.FindControl("x_GalleryFolder") IsNot Nothing) Then
			jsString &= "ew.Controls['x_GalleryFolder'] = '" & control.FindControl("x_GalleryFolder").ClientID & "';"
		End If
		If (control.FindControl("x_SiteURL") IsNot Nothing) Then
			jsString &= "ew.Controls['x_SiteURL'] = '" & control.FindControl("x_SiteURL").ClientID & "';"
		End If
		If (control.FindControl("x_DefaultPaymentTerms") IsNot Nothing) Then
			jsString &= "ew.Controls['x_DefaultPaymentTerms'] = '" & control.FindControl("x_DefaultPaymentTerms").ClientID & "';"
		End If
		If (control.FindControl("x_DefaultInvoiceDescription") IsNot Nothing) Then
			jsString &= "ew.Controls['x_DefaultInvoiceDescription'] = '" & control.FindControl("x_DefaultInvoiceDescription").ClientID & "';"
		End If
		If (control.FindControl("x_DefaultArticleID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_DefaultArticleID'] = '" & control.FindControl("x_DefaultArticleID").ClientID & "';"
		End If
		If (control.FindControl("x_HomePageID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_HomePageID'] = '" & control.FindControl("x_HomePageID").ClientID & "';"
		End If
		If (control.FindControl("x_UseBreadCrumbURL") IsNot Nothing) Then
			jsString &= "ew.Controls['x_UseBreadCrumbURL'] = '" & control.FindControl("x_UseBreadCrumbURL").ClientID & "';"
		End If
		If (control.FindControl("x_SingleSiteGallery") IsNot Nothing) Then
			jsString &= "ew.Controls['x_SingleSiteGallery'] = '" & control.FindControl("x_SingleSiteGallery").ClientID & "';"
		End If
		If (control.FindControl("x_SiteCategoryTypeID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_SiteCategoryTypeID'] = '" & control.FindControl("x_SiteCategoryTypeID").ClientID & "';"
		End If
		If (control.FindControl("x_ActiveFL") IsNot Nothing) Then
			jsString &= "ew.Controls['x_ActiveFL'] = '" & control.FindControl("x_ActiveFL").ClientID & "';"
		End If
		If (control.FindControl("x_Component") IsNot Nothing) Then
			jsString &= "ew.Controls['x_Component'] = '" & control.FindControl("x_Component").ClientID & "';"
		End If
		If (control.FindControl("x_FromEmail") IsNot Nothing) Then
			jsString &= "ew.Controls['x_FromEmail'] = '" & control.FindControl("x_FromEmail").ClientID & "';"
		End If
		If (control.FindControl("x_SMTP") IsNot Nothing) Then
			jsString &= "ew.Controls['x_SMTP'] = '" & control.FindControl("x_SMTP").ClientID & "';"
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
			Dim x_CompanyName As TextBox = TryCast(control.FindControl("x_CompanyName"), TextBox)
			If (x_CompanyName IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_CompanyName.Text)) Then
					messageList.Add("Invalid Value (String): Company Name")
				ElseIf (String.IsNullOrEmpty(x_CompanyName.Text)) Then
					messageList.Add("Please enter required field (String): Company Name")
				End If
			End If
			Dim x_SiteTitle As TextBox = TryCast(control.FindControl("x_SiteTitle"), TextBox)
			If (x_SiteTitle IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_SiteTitle.Text)) Then
					messageList.Add("Invalid Value (String): Site Title")
				End If
			End If
			Dim x_SiteTemplate As DropDownList = TryCast(control.FindControl("x_SiteTemplate"), DropDownList)
			If (x_SiteTemplate IsNot Nothing) Then
				If ((x_SiteTemplate.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckString(x_SiteTemplate.SelectedValue))) Then
					messageList.Add("Invalid Value (String): Site Template")
				ElseIf (String.IsNullOrEmpty(x_SiteTemplate.SelectedValue)) Then
					messageList.Add("Please enter required field (String): Site Template")
				End If
			End If
			Dim x_DefaultSiteTemplate As DropDownList = TryCast(control.FindControl("x_DefaultSiteTemplate"), DropDownList)
			If (x_DefaultSiteTemplate IsNot Nothing) Then
				If ((x_DefaultSiteTemplate.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckString(x_DefaultSiteTemplate.SelectedValue))) Then
					messageList.Add("Invalid Value (String): Default Template")
				ElseIf (String.IsNullOrEmpty(x_DefaultSiteTemplate.SelectedValue)) Then
					messageList.Add("Please enter required field (String): Default Template")
				End If
			End If
			Dim x_GalleryFolder As TextBox = TryCast(control.FindControl("x_GalleryFolder"), TextBox)
			If (x_GalleryFolder IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_GalleryFolder.Text)) Then
					messageList.Add("Invalid Value (String): Gallery Folder")
				End If
			End If
			Dim x_SiteURL As TextBox = TryCast(control.FindControl("x_SiteURL"), TextBox)
			If (x_SiteURL IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_SiteURL.Text)) Then
					messageList.Add("Invalid Value (String): Site URL")
				End If
			End If
			Dim x_DefaultPaymentTerms As TextBox = TryCast(control.FindControl("x_DefaultPaymentTerms"), TextBox)
			If (x_DefaultPaymentTerms IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_DefaultPaymentTerms.Text)) Then
					messageList.Add("Invalid Value (String): Site Keywords")
				End If
			End If
			Dim x_DefaultInvoiceDescription As TextBox = TryCast(control.FindControl("x_DefaultInvoiceDescription"), TextBox)
			If (x_DefaultInvoiceDescription IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_DefaultInvoiceDescription.Text)) Then
					messageList.Add("Invalid Value (String): Site Description")
				End If
			End If
			Dim x_DefaultArticleID As DropDownList = TryCast(control.FindControl("x_DefaultArticleID"), DropDownList)
			If (x_DefaultArticleID IsNot Nothing) Then
				If ((x_DefaultArticleID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_DefaultArticleID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Default Article ID")
				End If
			End If
			Dim x_HomePageID As DropDownList = TryCast(control.FindControl("x_HomePageID"), DropDownList)
			If (x_HomePageID IsNot Nothing) Then
				If ((x_HomePageID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_HomePageID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Home Page ID")
				End If
			End If
			Dim x_SiteCategoryTypeID As DropDownList = TryCast(control.FindControl("x_SiteCategoryTypeID"), DropDownList)
			If (x_SiteCategoryTypeID IsNot Nothing) Then
				If ((x_SiteCategoryTypeID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_SiteCategoryTypeID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Site Type")
				End If
			End If
			Dim x_Component As TextBox = TryCast(control.FindControl("x_Component"), TextBox)
			If (x_Component IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_Component.Text)) Then
					messageList.Add("Invalid Value (String): Component")
				End If
			End If
			Dim x_FromEmail As TextBox = TryCast(control.FindControl("x_FromEmail"), TextBox)
			If (x_FromEmail IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_FromEmail.Text)) Then
					messageList.Add("Invalid Value (String): From Email")
				End If
			End If
			Dim x_SMTP As TextBox = TryCast(control.FindControl("x_SMTP"), TextBox)
			If (x_SMTP IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_SMTP.Text)) Then
					messageList.Add("Invalid Value (String): SMTP")
				End If
			End If
		End If
		Return IIf(messageList.Count = 0, Nothing, messageList)
	End Function

	' **********************************************
	' *  Convert Input Control Values to Row Object
	' **********************************************

	Private Sub ControlToRow(ByVal row As Companyrow, ByVal control As WebControl)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		row.SetPageType(Core.PageType.Add) ' set page type = list for update

		' Field CompanyName
		Dim x_CompanyName As TextBox = TryCast(control.FindControl("x_CompanyName"), TextBox)
		If (x_CompanyName.Text <> String.Empty) Then row.CompanyName = x_CompanyName.Text Else row.CompanyName = String.Empty

		' Field SiteTitle
		Dim x_SiteTitle As TextBox = TryCast(control.FindControl("x_SiteTitle"), TextBox)
		If (x_SiteTitle.Text <> String.Empty) Then row.SiteTitle = x_SiteTitle.Text Else row.SiteTitle = Nothing

		' Field SiteTemplate
		Dim x_SiteTemplate As DropDownList = TryCast(control.FindControl("x_SiteTemplate"), DropDownList)
		Dim v_SiteTemplate As String = String.Empty
		If (x_SiteTemplate.SelectedIndex >= 0) Then
			For Each li As ListItem In x_SiteTemplate.Items
				If (li.Selected) Then
					v_SiteTemplate += li.Value
					Exit For
				End If
			Next
		End If
		If (v_SiteTemplate <> String.Empty) Then row.SiteTemplate = v_SiteTemplate Else row.SiteTemplate = String.Empty

		' Field DefaultSiteTemplate
		Dim x_DefaultSiteTemplate As DropDownList = TryCast(control.FindControl("x_DefaultSiteTemplate"), DropDownList)
		Dim v_DefaultSiteTemplate As String = String.Empty
		If (x_DefaultSiteTemplate.SelectedIndex >= 0) Then
			For Each li As ListItem In x_DefaultSiteTemplate.Items
				If (li.Selected) Then
					v_DefaultSiteTemplate += li.Value
					Exit For
				End If
			Next
		End If
		If (v_DefaultSiteTemplate <> String.Empty) Then row.DefaultSiteTemplate = v_DefaultSiteTemplate Else row.DefaultSiteTemplate = String.Empty

		' Field GalleryFolder
		Dim x_GalleryFolder As TextBox = TryCast(control.FindControl("x_GalleryFolder"), TextBox)
		If (x_GalleryFolder.Text <> String.Empty) Then row.GalleryFolder = x_GalleryFolder.Text Else row.GalleryFolder = Nothing

		' Field SiteURL
		Dim x_SiteURL As TextBox = TryCast(control.FindControl("x_SiteURL"), TextBox)
		If (x_SiteURL.Text <> String.Empty) Then row.SiteURL = x_SiteURL.Text Else row.SiteURL = Nothing

		' Field DefaultPaymentTerms
		Dim x_DefaultPaymentTerms As TextBox = TryCast(control.FindControl("x_DefaultPaymentTerms"), TextBox)
		If (x_DefaultPaymentTerms.Text <> String.Empty) Then row.DefaultPaymentTerms = x_DefaultPaymentTerms.Text Else row.DefaultPaymentTerms = Nothing

		' Field DefaultInvoiceDescription
		Dim x_DefaultInvoiceDescription As TextBox = TryCast(control.FindControl("x_DefaultInvoiceDescription"), TextBox)
		If (x_DefaultInvoiceDescription.Text <> String.Empty) Then row.DefaultInvoiceDescription = x_DefaultInvoiceDescription.Text Else row.DefaultInvoiceDescription = Nothing

		' Field DefaultArticleID
		Dim x_DefaultArticleID As DropDownList = TryCast(control.FindControl("x_DefaultArticleID"), DropDownList)
		Dim v_DefaultArticleID As String = String.Empty
		If (x_DefaultArticleID.SelectedIndex >= 0) Then
			For Each li As ListItem In x_DefaultArticleID.Items
				If (li.Selected) Then
					v_DefaultArticleID += li.Value
					Exit For
				End If
			Next
		End If
		If (v_DefaultArticleID <> String.Empty) Then row.DefaultArticleID = Convert.ToInt32(v_DefaultArticleID) Else row.DefaultArticleID = CType(Nothing, Nullable(Of Int32))

		' Field HomePageID
		Dim x_HomePageID As DropDownList = TryCast(control.FindControl("x_HomePageID"), DropDownList)
		Dim v_HomePageID As String = String.Empty
		If (x_HomePageID.SelectedIndex >= 0) Then
			For Each li As ListItem In x_HomePageID.Items
				If (li.Selected) Then
					v_HomePageID += li.Value
					Exit For
				End If
			Next
		End If
		If (v_HomePageID <> String.Empty) Then row.HomePageID = Convert.ToInt32(v_HomePageID) Else row.HomePageID = CType(Nothing, Nullable(Of Int32))

		' Field UseBreadCrumbURL
		Dim x_UseBreadCrumbURL As CheckBox = TryCast(control.FindControl("x_UseBreadCrumbURL"), CheckBox)
		If (x_UseBreadCrumbURL.Checked) Then
			row.UseBreadCrumbURL = True
		Else
			row.UseBreadCrumbURL = False
		End If

		' Field SingleSiteGallery
		Dim x_SingleSiteGallery As CheckBox = TryCast(control.FindControl("x_SingleSiteGallery"), CheckBox)
		If (x_SingleSiteGallery.Checked) Then
			row.SingleSiteGallery = True
		Else
			row.SingleSiteGallery = False
		End If

		' Field SiteCategoryTypeID
		Dim x_SiteCategoryTypeID As DropDownList = TryCast(control.FindControl("x_SiteCategoryTypeID"), DropDownList)
		Dim v_SiteCategoryTypeID As String = String.Empty
		If (x_SiteCategoryTypeID.SelectedIndex >= 0) Then
			For Each li As ListItem In x_SiteCategoryTypeID.Items
				If (li.Selected) Then
					v_SiteCategoryTypeID += li.Value
					Exit For
				End If
			Next
		End If
		If (v_SiteCategoryTypeID <> String.Empty) Then row.SiteCategoryTypeID = Convert.ToInt32(v_SiteCategoryTypeID) Else row.SiteCategoryTypeID = CType(Nothing, Nullable(Of Int32))

		' Field ActiveFL
		Dim x_ActiveFL As CheckBox = TryCast(control.FindControl("x_ActiveFL"), CheckBox)
		If (x_ActiveFL.Checked) Then
			row.ActiveFL = True
		Else
			row.ActiveFL = False
		End If

		' Field Component
		Dim x_Component As TextBox = TryCast(control.FindControl("x_Component"), TextBox)
		If (x_Component.Text <> String.Empty) Then row.Component = x_Component.Text Else row.Component = Nothing

		' Field FromEmail
		Dim x_FromEmail As TextBox = TryCast(control.FindControl("x_FromEmail"), TextBox)
		If (x_FromEmail.Text <> String.Empty) Then row.FromEmail = x_FromEmail.Text Else row.FromEmail = Nothing

		' Field SMTP
		Dim x_SMTP As TextBox = TryCast(control.FindControl("x_SMTP"), TextBox)
		If (x_SMTP.Text <> String.Empty) Then row.SMTP = x_SMTP.Text Else row.SMTP = Nothing
	End Sub

	' ****************************
	' *  Check for duplicate Key
	' ****************************

	Private Function CheckDuplicateKey(ByVal control As WebControl) As ArrayList 
		Dim sWhere As String = String.Empty
		Dim messageList As ArrayList = New ArrayList()
		Dim data As Companydal = New Companydal()
		Dim newkey As Companykey = New Companykey()
		Try

		' Check for duplicate Company Name
			sWhere = "([CompanyName] = '" + Db.AdjustSql((CType(control.FindControl("x_CompanyName"),TextBox)).Text) + "')"
			If (data.LoadList(sWhere) IsNot Nothing) Then
				messageList.Add("Duplicate value: Company Name")
			End If
		data = Nothing
		Catch e As Exception
			messageList.Add(e.Message)
		End Try
		Return IIf(messageList.Count = 0, Nothing, messageList)
	End Function

	' ********************************************
	' *  Convert Row Object Value To Input Control
	' ********************************************

	Private Sub RowToControl(ByVal row As Companyrow, ByVal control As WebControl, ByVal ct As Core.CtrlType)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		If (ct = Core.CtrlType.Edit) Then ' set up edit control

			' Field CompanyName
			Dim x_CompanyName As TextBox = TryCast(control.FindControl("x_CompanyName"), TextBox)
			If (row.CompanyName IsNot Nothing) Then x_CompanyName.Text = row.CompanyName.ToString() Else x_CompanyName.Text = String.Empty

			' Field SiteTitle
			Dim x_SiteTitle As TextBox = TryCast(control.FindControl("x_SiteTitle"), TextBox)
			If (row.SiteTitle IsNot Nothing) Then x_SiteTitle.Text = row.SiteTitle.ToString() Else x_SiteTitle.Text = String.Empty

			' Field SiteTemplate
			Dim x_SiteTemplate As DropDownList = TryCast(control.FindControl("x_SiteTemplate"), DropDownList)
			x_SiteTemplate.DataValueField = "ewValueField"
			x_SiteTemplate.DataTextField = "ewTextField"
			Dim dv_x_SiteTemplate As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
			If (dv_x_SiteTemplate Is Nothing) Then dv_x_SiteTemplate = Companydal.LookUpTable("SiteTemplate")
			x_SiteTemplate.DataSource = dv_x_SiteTemplate
			x_SiteTemplate.DataBind()
			x_SiteTemplate.Items.Insert(0, New ListItem("Please Select", ""))
			Dim v_SiteTemplate As String 
			If (row.SiteTemplate IsNot Nothing) Then  v_SiteTemplate = Convert.ToString(row.SiteTemplate) Else v_SiteTemplate = String.Empty
			x_SiteTemplate.ClearSelection()
			For Each li As ListItem In x_SiteTemplate.Items
				If (li.Value.ToString() = v_SiteTemplate) Then
					li.Selected = True
					Exit For
				End If
			Next

			' Field DefaultSiteTemplate
			Dim x_DefaultSiteTemplate As DropDownList = TryCast(control.FindControl("x_DefaultSiteTemplate"), DropDownList)
			x_DefaultSiteTemplate.DataValueField = "ewValueField"
			x_DefaultSiteTemplate.DataTextField = "ewTextField"
			Dim dv_x_DefaultSiteTemplate As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
			If (dv_x_DefaultSiteTemplate Is Nothing) Then dv_x_DefaultSiteTemplate = Companydal.LookUpTable("DefaultSiteTemplate")
			x_DefaultSiteTemplate.DataSource = dv_x_DefaultSiteTemplate
			x_DefaultSiteTemplate.DataBind()
			x_DefaultSiteTemplate.Items.Insert(0, New ListItem("Please Select", ""))
			Dim v_DefaultSiteTemplate As String 
			If (row.DefaultSiteTemplate IsNot Nothing) Then  v_DefaultSiteTemplate = Convert.ToString(row.DefaultSiteTemplate) Else v_DefaultSiteTemplate = String.Empty
			x_DefaultSiteTemplate.ClearSelection()
			For Each li As ListItem In x_DefaultSiteTemplate.Items
				If (li.Value.ToString() = v_DefaultSiteTemplate) Then
					li.Selected = True
					Exit For
				End If
			Next

			' Field GalleryFolder
			Dim x_GalleryFolder As TextBox = TryCast(control.FindControl("x_GalleryFolder"), TextBox)
			If (row.GalleryFolder IsNot Nothing) Then x_GalleryFolder.Text = row.GalleryFolder.ToString() Else x_GalleryFolder.Text = String.Empty

			' Field SiteURL
			Dim x_SiteURL As TextBox = TryCast(control.FindControl("x_SiteURL"), TextBox)
			If (row.SiteURL IsNot Nothing) Then x_SiteURL.Text = row.SiteURL.ToString() Else x_SiteURL.Text = String.Empty

			' Field DefaultPaymentTerms
			Dim x_DefaultPaymentTerms As TextBox = TryCast(control.FindControl("x_DefaultPaymentTerms"), TextBox)
			If (row.DefaultPaymentTerms IsNot Nothing) Then x_DefaultPaymentTerms.Text = row.DefaultPaymentTerms.ToString() Else x_DefaultPaymentTerms.Text = String.Empty

			' Field DefaultInvoiceDescription
			Dim x_DefaultInvoiceDescription As TextBox = TryCast(control.FindControl("x_DefaultInvoiceDescription"), TextBox)
			If (row.DefaultInvoiceDescription IsNot Nothing) Then x_DefaultInvoiceDescription.Text = row.DefaultInvoiceDescription.ToString() Else x_DefaultInvoiceDescription.Text = String.Empty

			' Field DefaultArticleID
			Dim x_DefaultArticleID As DropDownList = TryCast(control.FindControl("x_DefaultArticleID"), DropDownList)
			x_DefaultArticleID.DataValueField = "ewValueField"
			x_DefaultArticleID.DataTextField = "ewTextField"
			Dim dv_x_DefaultArticleID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
			If (dv_x_DefaultArticleID Is Nothing) Then dv_x_DefaultArticleID = Companydal.LookUpTable("DefaultArticleID")
			x_DefaultArticleID.DataSource = dv_x_DefaultArticleID
			x_DefaultArticleID.DataBind()
			x_DefaultArticleID.Items.Insert(0, New ListItem("Please Select", ""))
			Dim v_DefaultArticleID As String
			If (row.DefaultArticleID.HasValue) Then v_DefaultArticleID = Convert.ToString(row.DefaultArticleID) Else v_DefaultArticleID = String.Empty
			x_DefaultArticleID.ClearSelection()
			For Each li As ListItem In x_DefaultArticleID.Items
				If (li.Value.ToString() = v_DefaultArticleID) Then
					li.Selected = True
					Exit For
				End If
			Next

			' Field HomePageID
			Dim x_HomePageID As DropDownList = TryCast(control.FindControl("x_HomePageID"), DropDownList)
			x_HomePageID.DataValueField = "ewValueField"
			x_HomePageID.DataTextField = "ewTextField"
			Dim dv_x_HomePageID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
			If (dv_x_HomePageID Is Nothing) Then dv_x_HomePageID = Companydal.LookUpTable("HomePageID")
			x_HomePageID.DataSource = dv_x_HomePageID
			x_HomePageID.DataBind()
			x_HomePageID.Items.Insert(0, New ListItem("Please Select", ""))
			Dim v_HomePageID As String
			If (row.HomePageID.HasValue) Then v_HomePageID = Convert.ToString(row.HomePageID) Else v_HomePageID = String.Empty
			x_HomePageID.ClearSelection()
			For Each li As ListItem In x_HomePageID.Items
				If (li.Value.ToString() = v_HomePageID) Then
					li.Selected = True
					Exit For
				End If
			Next

			' Field UseBreadCrumbURL
			Dim x_UseBreadCrumbURL As CheckBox = TryCast(control.FindControl("x_UseBreadCrumbURL"), CheckBox)
			Dim sUseBreadCrumbURL As String = row.UseBreadCrumbURL.ToString()
			If ((sUseBreadCrumbURL IsNot Nothing) AndAlso sUseBreadCrumbURL <> "") Then
				If (Convert.ToBoolean(sUseBreadCrumbURL)) Then
					x_UseBreadCrumbURL.Checked = True
				Else
					x_UseBreadCrumbURL.Checked = False
				End If
			End If

			' Field SingleSiteGallery
			Dim x_SingleSiteGallery As CheckBox = TryCast(control.FindControl("x_SingleSiteGallery"), CheckBox)
			Dim sSingleSiteGallery As String = row.SingleSiteGallery.ToString()
			If ((sSingleSiteGallery IsNot Nothing) AndAlso sSingleSiteGallery <> "") Then
				If (Convert.ToBoolean(sSingleSiteGallery)) Then
					x_SingleSiteGallery.Checked = True
				Else
					x_SingleSiteGallery.Checked = False
				End If
			End If

			' Field SiteCategoryTypeID
			Dim x_SiteCategoryTypeID As DropDownList = TryCast(control.FindControl("x_SiteCategoryTypeID"), DropDownList)
			x_SiteCategoryTypeID.DataValueField = "ewValueField"
			x_SiteCategoryTypeID.DataTextField = "ewTextField"
			Dim dv_x_SiteCategoryTypeID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
			If (dv_x_SiteCategoryTypeID Is Nothing) Then dv_x_SiteCategoryTypeID = Companydal.LookUpTable("SiteCategoryTypeID")
			x_SiteCategoryTypeID.DataSource = dv_x_SiteCategoryTypeID
			x_SiteCategoryTypeID.DataBind()
			x_SiteCategoryTypeID.Items.Insert(0, New ListItem("Please Select", ""))
			Dim v_SiteCategoryTypeID As String
			If (row.SiteCategoryTypeID.HasValue) Then v_SiteCategoryTypeID = Convert.ToString(row.SiteCategoryTypeID) Else v_SiteCategoryTypeID = String.Empty
			x_SiteCategoryTypeID.ClearSelection()
			For Each li As ListItem In x_SiteCategoryTypeID.Items
				If (li.Value.ToString() = v_SiteCategoryTypeID) Then
					li.Selected = True
					Exit For
				End If
			Next

			' Field ActiveFL
			Dim x_ActiveFL As CheckBox = TryCast(control.FindControl("x_ActiveFL"), CheckBox)
			Dim sActiveFL As String = row.ActiveFL.ToString()
			If ((sActiveFL IsNot Nothing) AndAlso sActiveFL <> "") Then
				If (Convert.ToBoolean(sActiveFL)) Then
					x_ActiveFL.Checked = True
				Else
					x_ActiveFL.Checked = False
				End If
			End If

			' Field Component
			Dim x_Component As TextBox = TryCast(control.FindControl("x_Component"), TextBox)
			If (row.Component IsNot Nothing) Then x_Component.Text = row.Component.ToString() Else x_Component.Text = String.Empty

			' Field FromEmail
			Dim x_FromEmail As TextBox = TryCast(control.FindControl("x_FromEmail"), TextBox)
			If (row.FromEmail IsNot Nothing) Then x_FromEmail.Text = row.FromEmail.ToString() Else x_FromEmail.Text = String.Empty

			' Field SMTP
			Dim x_SMTP As TextBox = TryCast(control.FindControl("x_SMTP"), TextBox)
			If (row.SMTP IsNot Nothing) Then x_SMTP.Text = row.SMTP.ToString() Else x_SMTP.Text = String.Empty
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

	Protected Sub CompanyDataSource_Inserting(ByVal sender As Object, ByVal e As ObjectDataSourceMethodEventArgs)
		Dim control As WebControl = CompanyDetailsView

		' Set up row object
		Dim row As Companyrow = TryCast(e.InputParameters(0), Companyrow)
		ControlToRow(row, control)
		If (Companybll.Inserting(row)) Then
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

	Protected Sub CompanyDataSource_Inserted(ByVal sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.Exception IsNot Nothing) Then 
			e.AffectedRows = 0
			e.ExceptionHandled = True
			lblMessage.Text = e.Exception.InnerException.Message
			pnlMessage.Visible = True
		Else
			newrow = TryCast(e.ReturnValue, Companyrow) ' get new row objectinsert method
			Companybll.Inserted(newrow)
		End If
	End Sub

	' *******************
	' *  Get User Filter
	' *******************

	Private Function GetUserFilter() As String 
		Return Companyinf.GetUserFilter()
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
	<p><span class="aspnetmaker">Add to TABLE: Company</span></p>
	<p>
		<asp:Hyperlink id="lnkList" Text="Back to List" NavigateUrl="company_list.aspx" CssClass="aspnetmaker" runat="server" />
		<asp:Label id="lblReturnUrl" Visible="False" CssClass="aspnetmaker" runat="server">company_list.aspx</asp:Label>
	</p>
	<asp:ValidationSummary id="xevs_Company" CssClass="aspnetmaker" runat="server"
	HeaderText=""
	ShowSummary="False"
	ShowMessageBox="True"
	ForeColor="#FF0000" />
	<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
		<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
	</asp:PlaceHolder>
	<!-- Data Source -->
<asp:ObjectDataSource ID="CompanyDataSource"
	TypeName="PMGEN.Companydal"
	DataObjectTypeName="PMGEN.Companyrow"
	InsertMethod="Insert"
	OnInserting="CompanyDataSource_Inserting"
	OnInserted="CompanyDataSource_Inserted"
	runat="server">
</asp:ObjectDataSource>
	<asp:DetailsView ID="CompanyDetailsView"
		DataKeyNames="CompanyID"
		DataSourceID="CompanyDataSource"
		DefaultMode="Insert"
		GridLines="None"
		AutoGenerateRows="False" CssClass="ewDetailTable"
		OnInit="CompanyDetailsView_Init"
		OnDataBound="CompanyDetailsView_DataBound"
		OnItemInserting="CompanyDetailsView_ItemInserting"
		OnItemInserted="CompanyDetailsView_ItemInserted"
		OnUnload="CompanyDetailsView_Unload"
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
					<asp:Label runat="server" id="xs_CompanyName">Company Name<span class='ewmsg'>&nbsp;*</span></asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_CompanyName" Columns="30" MaxLength="50" CssClass="aspnetmaker" runat="server" />
<asp:RequiredFieldValidator ID="vx_CompanyName" CssClass="aspnetmaker" runat="server" ControlToValidate="x_CompanyName" ErrorMessage="Please enter required field - Company Name" ForeColor="Red" Display="None" Text="*" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_SiteTitle">Site Title</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_SiteTitle" Columns="30" MaxLength="255" CssClass="aspnetmaker" runat="server" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_SiteTemplate">Site Template<span class='ewmsg'>&nbsp;*</span></asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:DropDownList ID="x_SiteTemplate" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
<asp:RequiredFieldValidator ID="vx_SiteTemplate" CssClass="aspnetmaker" runat="server" ControlToValidate="x_SiteTemplate" ErrorMessage="Please enter required field - Site Template" ForeColor="Red" Display="None" Text="*" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_DefaultSiteTemplate">Default Template<span class='ewmsg'>&nbsp;*</span></asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:DropDownList ID="x_DefaultSiteTemplate" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
<asp:RequiredFieldValidator ID="vx_DefaultSiteTemplate" CssClass="aspnetmaker" runat="server" ControlToValidate="x_DefaultSiteTemplate" ErrorMessage="Please enter required field - Default Template" ForeColor="Red" Display="None" Text="*" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_GalleryFolder">Gallery Folder</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_GalleryFolder" Columns="30" MaxLength="50" CssClass="aspnetmaker" runat="server" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_SiteURL">Site URL</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_SiteURL" Columns="30" MaxLength="255" CssClass="aspnetmaker" runat="server" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_DefaultPaymentTerms">Site Keywords</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_DefaultPaymentTerms" Columns="30" MaxLength="255" CssClass="aspnetmaker" runat="server" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_DefaultInvoiceDescription">Site Description</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_DefaultInvoiceDescription" TextMode="MultiLine" Rows="4" Columns="35" CssClass="aspnetmaker" runat="server" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_DefaultArticleID">Default Article ID</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:DropDownList ID="x_DefaultArticleID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_HomePageID">Home Page ID</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:DropDownList ID="x_HomePageID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_UseBreadCrumbURL">Use Bread Crumb URL</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:CheckBox ID="x_UseBreadCrumbURL" CssClass="aspnetmakerlist" runat="server" />
<asp:HiddenField ID="hx_UseBreadCrumbURL" Runat="server" Value='<%# Bind("UseBreadCrumbURL") %>' />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_SingleSiteGallery">Single Site Gallery</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:CheckBox ID="x_SingleSiteGallery" CssClass="aspnetmakerlist" runat="server" />
<asp:HiddenField ID="hx_SingleSiteGallery" Runat="server" Value='<%# Bind("SingleSiteGallery") %>' />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_SiteCategoryTypeID">Site Type</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:DropDownList ID="x_SiteCategoryTypeID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_ActiveFL">Active FL</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:CheckBox ID="x_ActiveFL" CssClass="aspnetmakerlist" runat="server" />
<asp:HiddenField ID="hx_ActiveFL" Runat="server" Value='<%# Bind("ActiveFL") %>' />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_Component">Component</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_Component" Columns="30" MaxLength="50" CssClass="aspnetmaker" runat="server" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_FromEmail">From Email</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_FromEmail" Columns="30" MaxLength="50" CssClass="aspnetmaker" runat="server" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_SMTP">SMTP</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_SMTP" Columns="30" MaxLength="50" CssClass="aspnetmaker" runat="server" />
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
					<asp:HiddenField ID="k_CompanyID" Runat="server" Value='<%# Bind("CompanyID") %>' />
				</InsertItemTemplate>
			</asp:TemplateField>
		</Fields>
	</asp:DetailsView>
</asp:Content>
