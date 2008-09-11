<%@ Page EnableViewState="true" Language="VB" ValidateRequest="False" Debug="True" MasterPageFile="masterpage.master" %>
<%@ MasterType VirtualPath="masterpage.master" %>
<%@ Import Namespace="PMGEN" %>
<%@ Import Namespace="EW.Data" %>
<%@ Import Namespace="EW.Data.Utilities" %>
<%@ Import Namespace="EW.Web" %>
<%@ Register NameSpace="PMGEN" TagPrefix="PMGEN" %>
<script runat="server">
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
		If Not mhUser.IsAdmin() Then
		  Session("Login_Link") = Request.RawUrl
                  Response.Redirect("/mhweb/login/login.aspx")
		End If
		Page.ClientScript.RegisterClientScriptInclude("ewv", "ewv.js")
		Response.Cache.SetCacheability(HttpCacheability.NoCache)
		If (Not Page.IsPostBack) Then
			lblReturnURL.Text = Session("ListPageURL")
			If (Request.QueryString.Count > 0) Then
				key = New Companykey()
				Dim messageList As ArrayList = Companyinf.LoadKey(key) 
				If (messageList IsNot Nothing) Then
					objProfile.Message = String.Empty
					For Each sMsg As String In messageList
						objProfile.Message &= sMsg & "<br>" 
					Next
					Response.Redirect(lblReturnUrl.Text) 
				End If
				ViewState("key") = key
			Else
				Response.Redirect(lblReturnUrl.Text)
			End If
		Else
			key = TryCast(ViewState("key"), Companykey) ' restore from ViewState for postback
		End If
		If (CompanyDetailsView.Visible) Then
			RegisterClientID("CtrlID", CompanyDetailsView)
		End If
		If (CompanyDetailsView.FindControl("x_CompanyName") IsNot Nothing) Then
			Page.Form.DefaultFocus = CompanyDetailsView.FindControl("x_CompanyName").ClientID
		End If
	End Sub

	' ***********************************
	' *  Handler for Cancel button click
	' ***********************************

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
		Dim row As Companyrow = TryCast(CompanyDetailsView.DataItem, Companyrow) ' get data object
		If (CompanyDetailsView.CurrentMode = DetailsViewMode.Edit) Then
			If (row IsNot Nothing) Then
				key.CompanyID = Convert.ToInt32(row.CompanyID)
			End If
			RowToControl(row, source, Core.CtrlType.Edit)
		End If
	End Sub

	' ****************************************
	' *  Handler for DetailsView ItemUpdating
	' ****************************************

	Protected Sub CompanyDetailsView_ItemUpdating(ByVal sender As Object, ByVal e As DetailsViewUpdateEventArgs)
		Dim source As WebControl = TryCast(sender, WebControl)

		' Validate input values
		Dim messageList As ArrayList = ValidateInputValues(CType(sender, Control), Core.PageType.Edit)
		If (messageList IsNot Nothing) Then
			lblMessage.Text = String.Empty
			For Each sMsg As String In messageList
				lblMessage.Text += sMsg + "<br>"
			Next
			pnlMessage.Visible = True
			e.Cancel = True
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

	' ***************************************
	' *  Handler for DetailsView ItemUpdated
	' ***************************************

	Protected Sub CompanyDetailsView_ItemUpdated(ByVal sender As Object, ByVal e As DetailsViewUpdatedEventArgs)

		' Check If update successful 
		If (e.AffectedRows = 0) Then

			' Keep in edit mode 
			e.KeepInEditMode = True

			' Get original values 
			Dim source As WebControl = TryCast(sender, WebControl)
			If (source Is Nothing) Then Return
			Dim oldrow As Companyrow = New Companyrow()
			ControlToRow(oldrow, source)

			' synchronize to database 
			Try
				CType(sender, DetailsView).DataBind()
			Catch err As Exception
				lblMessage.Text += "<br>" + err.Message
				pnlMessage.Visible = True
			End Try

			' Re-populate with values entered by user 
			source = TryCast(sender, WebControl) ' must get correct object again
			If (source Is Nothing) Then Return
			RowToControl(oldrow, source, Core.CtrlType.Edit)
			oldrow = Nothing
		Else
			objProfile.Message = "Update successful"
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
		If (control.FindControl("x_City") IsNot Nothing) Then
			jsString &= "ew.Controls['x_City'] = '" & control.FindControl("x_City").ClientID & "';"
		End If
		If (control.FindControl("x_StateOrProvince") IsNot Nothing) Then
			jsString &= "ew.Controls['x_StateOrProvince'] = '" & control.FindControl("x_StateOrProvince").ClientID & "';"
		End If
		If (control.FindControl("x_Country") IsNot Nothing) Then
			jsString &= "ew.Controls['x_Country'] = '" & control.FindControl("x_Country").ClientID & "';"
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
			Dim x_City As TextBox = TryCast(control.FindControl("x_City"), TextBox)
			If (x_City IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_City.Text)) Then
					messageList.Add("Invalid Value (String): City")
				End If
			End If
			Dim x_StateOrProvince As TextBox = TryCast(control.FindControl("x_StateOrProvince"), TextBox)
			If (x_StateOrProvince IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_StateOrProvince.Text)) Then
					messageList.Add("Invalid Value (String): State/Province")
				End If
			End If
			Dim x_Country As TextBox = TryCast(control.FindControl("x_Country"), TextBox)
			If (x_Country IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_Country.Text)) Then
					messageList.Add("Invalid Value (String): Country")
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
		row.SetPageType(Core.PageType.Edit) ' set page type = list for update

		' Field CompanyID
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

		' Field City
		Dim x_City As TextBox = TryCast(control.FindControl("x_City"), TextBox)
		If (x_City.Text <> String.Empty) Then row.City = x_City.Text Else row.City = Nothing

		' Field StateOrProvince
		Dim x_StateOrProvince As TextBox = TryCast(control.FindControl("x_StateOrProvince"), TextBox)
		If (x_StateOrProvince.Text <> String.Empty) Then row.StateOrProvince = x_StateOrProvince.Text Else row.StateOrProvince = Nothing

		' Field Country
		Dim x_Country As TextBox = TryCast(control.FindControl("x_Country"), TextBox)
		If (x_Country.Text <> String.Empty) Then row.Country = x_Country.Text Else row.Country = Nothing

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
			Dim sKeyWhere As string = data.KeyFilter(key)

		' Check for duplicate Company Name
			sWhere = "([CompanyName] = '" + Db.AdjustSql((CType(control.FindControl("x_CompanyName"),TextBox)).Text) + "' AND NOT (" & sKeyWhere & "))"
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

			' Field City
			Dim x_City As TextBox = TryCast(control.FindControl("x_City"), TextBox)
			If (row.City IsNot Nothing) Then x_City.Text = row.City.ToString() Else x_City.Text = String.Empty

			' Field StateOrProvince
			Dim x_StateOrProvince As TextBox = TryCast(control.FindControl("x_StateOrProvince"), TextBox)
			If (row.StateOrProvince IsNot Nothing) Then x_StateOrProvince.Text = row.StateOrProvince.ToString() Else x_StateOrProvince.Text = String.Empty

			' Field Country
			Dim x_Country As TextBox = TryCast(control.FindControl("x_Country"), TextBox)
			If (row.Country IsNot Nothing) Then x_Country.Text = row.Country.ToString() Else x_Country.Text = String.Empty

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
	' *  Handler for DataSource Selecting
	' ************************************

	Protected Sub CompanyDataSource_Selecting(Byval sender As Object, ByVal e As ObjectDataSourceSelectingEventArgs)
		e.InputParameters.Clear()
		e.InputParameters.Add("filter", Companyinf.GetUserFilter())
	End Sub

	' *************************************
	' *  DataSource Selected Event Handler
	' *************************************

	Protected Sub CompanyDataSource_Selected(Byval sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.ReturnValue Is Nothing) Then
			If (e.Exception IsNot Nothing) Then
				lblMessage.Text = e.Exception.Message
				pnlMessage.Visible = True
			Else
				lblMessage.Text = "No records found"
				pnlMessage.Visible = True
			End If
		Else
			Dim rows As Companyrows = TryCast(e.ReturnValue, Companyrows)
			Dim i As Integer = 0
			Do While (rows IsNot Nothing AndAlso i < rows.Count)
				If (key.Equals(rows(i).GetKey())) Then
					CompanyDetailsView.PageIndex = i
					Exit Do
				End If
				i += 1
			Loop
		End If
	End Sub

	' ***********************************
	' *  Handler for DataSource Updating
	' ***********************************

	Protected Sub CompanyDataSource_Updating(ByVal sender As Object, ByVal e As ObjectDataSourceMethodEventArgs)
		Dim source As DetailsView = CompanyDetailsView

		' Set up row object
		Dim row As Companyrow = TryCast(e.InputParameters(0), Companyrow)
		Dim data As Companydal = New Companydal()
		key.CompanyID = Convert.ToInt32(row.CompanyID)
		oldrow = data.LoadRow(key, Companyinf.GetUserFilter())
		ControlToRow(row, source) ' load values of controls to row
		newrow = row
		If (Companybll.Updating(oldrow, newrow)) Then
		Else
			e.Cancel = True
			If (Not String.IsNullOrEmpty(objProfile.Message)) Then
				lblMessage.Text = objProfile.Message
				pnlMessage.Visible = True
			End If
		End If
	End Sub

	' **********************************
	' *  Handler for DataSource Updated
	' **********************************

	Protected Sub CompanyDataSource_Updated(ByVal sender As Object, ByVal e As ObjectDataSourceStatusEventArgs)
		If (e.Exception IsNot Nothing) Then
			e.AffectedRows = 0
			e.ExceptionHandled = True
			lblMessage.Text = e.Exception.InnerException.Message
		Else
			lblMessage.Text = "Update successful"
			Companybll.Updated(oldrow, newrow)
		End If
		If (lblMessage.Text <> "") Then pnlMessage.Visible = True
	End Sub

	' ************************************************
	' *  Change the page index for DetailsView paging
	' ************************************************

	Protected Sub ChangePageIndex(ByVal sender As Object, ByVal e As DetailsViewPageEventArgs)
		Dim data As Companydal = New Companydal()
		Dim rows As Companyrows = data.LoadList(Companyinf.GetUserFilter())
		If (rows IsNot Nothing AndAlso rows.Count >= e.NewPageIndex) Then
			key = rows(e.NewPageIndex).GetKey()
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
	<p><span class="aspnetmaker">Edit TABLE: Company</span></p>
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
	SelectMethod="LoadList"
	UpdateMethod="Update"
	OnSelecting="CompanyDataSource_Selecting"
	OnSelected="CompanyDataSource_Selected"
	OnUpdating="CompanyDataSource_Updating"
	OnUpdated="CompanyDataSource_Updated"
	runat="server">
</asp:ObjectDataSource>
<!-- DetailsView -->
	<asp:DetailsView ID="CompanyDetailsView"
		DataKeyNames="CompanyID"
		DataSourceID="CompanyDataSource"
		DefaultMode="Edit"
		GridLines="None"
		AutoGenerateRows="False" CssClass="ewDetailTable"
		OnInit="CompanyDetailsView_Init"
		OnDataBound="CompanyDetailsView_DataBound"
		OnItemUpdating="CompanyDetailsView_ItemUpdating"
		OnItemUpdated="CompanyDetailsView_ItemUpdated"
		OnUnload="CompanyDetailsView_Unload"
		AllowPaging="True"
		OnPageIndexChanging="ChangePageIndex"
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
				<EditItemTemplate>
<asp:TextBox id="x_CompanyName" Columns="30" MaxLength="50" CssClass="aspnetmaker" runat="server" />
<asp:RequiredFieldValidator ID="vx_CompanyName" CssClass="aspnetmaker" runat="server" ControlToValidate="x_CompanyName" ErrorMessage="Please enter required field - Company Name" ForeColor="Red" Display="None" Text="*" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_SiteTitle">Site Title</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_SiteTitle" Columns="30" MaxLength="255" CssClass="aspnetmaker" runat="server" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_SiteTemplate">Site Template<span class='ewmsg'>&nbsp;*</span></asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:DropDownList ID="x_SiteTemplate" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
<asp:RequiredFieldValidator ID="vx_SiteTemplate" CssClass="aspnetmaker" runat="server" ControlToValidate="x_SiteTemplate" ErrorMessage="Please enter required field - Site Template" ForeColor="Red" Display="None" Text="*" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_DefaultSiteTemplate">Default Template<span class='ewmsg'>&nbsp;*</span></asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:DropDownList ID="x_DefaultSiteTemplate" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
<asp:RequiredFieldValidator ID="vx_DefaultSiteTemplate" CssClass="aspnetmaker" runat="server" ControlToValidate="x_DefaultSiteTemplate" ErrorMessage="Please enter required field - Default Template" ForeColor="Red" Display="None" Text="*" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_GalleryFolder">Gallery Folder</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_GalleryFolder" Columns="30" MaxLength="50" CssClass="aspnetmaker" runat="server" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_SiteURL">Site URL</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_SiteURL" Columns="30" MaxLength="255" CssClass="aspnetmaker" runat="server" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_City">City</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_City" Columns="30" MaxLength="50" CssClass="aspnetmaker" runat="server" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_StateOrProvince">State/Province</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_StateOrProvince" Columns="30" MaxLength="20" CssClass="aspnetmaker" runat="server" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_Country">Country</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_Country" Columns="30" MaxLength="50" CssClass="aspnetmaker" runat="server" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_DefaultPaymentTerms">Site Keywords</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_DefaultPaymentTerms" Columns="30" MaxLength="255" CssClass="aspnetmaker" runat="server" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_DefaultInvoiceDescription">Site Description</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_DefaultInvoiceDescription" TextMode="MultiLine" Rows="4" Columns="35" CssClass="aspnetmaker" runat="server" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_DefaultArticleID">Default Article ID</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:DropDownList ID="x_DefaultArticleID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_HomePageID">Home Page ID</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:DropDownList ID="x_HomePageID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_UseBreadCrumbURL">Use Bread Crumb URL</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:CheckBox ID="x_UseBreadCrumbURL" CssClass="aspnetmakerlist" runat="server" />
<asp:HiddenField ID="hx_UseBreadCrumbURL" Runat="server" Value='<%# Bind("UseBreadCrumbURL") %>' />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_SingleSiteGallery">Single Site Gallery</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:CheckBox ID="x_SingleSiteGallery" CssClass="aspnetmakerlist" runat="server" />
<asp:HiddenField ID="hx_SingleSiteGallery" Runat="server" Value='<%# Bind("SingleSiteGallery") %>' />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_SiteCategoryTypeID">Site Type</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:DropDownList ID="x_SiteCategoryTypeID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_ActiveFL">Active FL</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:CheckBox ID="x_ActiveFL" CssClass="aspnetmakerlist" runat="server" />
<asp:HiddenField ID="hx_ActiveFL" Runat="server" Value='<%# Bind("ActiveFL") %>' />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_Component">Component</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_Component" Columns="30" MaxLength="50" CssClass="aspnetmaker" runat="server" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_FromEmail">From Email</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_FromEmail" Columns="30" MaxLength="50" CssClass="aspnetmaker" runat="server" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_SMTP">SMTP</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_SMTP" Columns="30" MaxLength="50" CssClass="aspnetmaker" runat="server" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField ShowHeader="False">
				<ItemStyle CssClass="ewTableFooter" />
				<EditItemTemplate>
				<table border="0">
					<tr>
						<td><asp:LinkButton ID="btnEdit" CssClass="aspnetmaker" runat="server" CausesValidation="True" CommandName="Update" Text="Update"></asp:LinkButton></td>
						<td><asp:LinkButton ID="btnCancel" CssClass="aspnetmaker" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel" OnClick="btnCancel_Click"></asp:LinkButton></td>
					</tr>
				</table>
				<asp:HiddenField id="x_CompanyID" runat="server" />
				</EditItemTemplate>
			</asp:TemplateField>
		</Fields>
	</asp:DetailsView>
	<br />
	<input type="hidden" id="keyID" runat="server" />
</asp:Content>
