<%@ Page EnableViewState="true" Language="VB" ValidateRequest="False" EnableEventValidation="False" Debug="True" MasterPageFile="masterpage.master" %>
<%@ MasterType VirtualPath="masterpage.master" %>
<%@ Import Namespace="PMGEN" %>
<%@ Import Namespace="EW.Data" %>
<%@ Import Namespace="EW.Data.Utilities" %>
<%@ Import Namespace="EW.Web" %>
<%@ Register NameSpace="PMGEN" TagPrefix="PMGEN" %>
<script runat="server">
	Dim key As SiteLinkkey ' record key
	Dim oldrow As SiteLinkrow ' old record data input by user
	Dim newrow As SiteLinkrow ' new record data input by user
	Dim objProfile As TableProfile ' table profile

	' *************************
	' *  Handler for Page Init
	' *************************

	Protected Sub Page_Init(ByVal s As Object, ByVal e As System.EventArgs)
		If Not mhUser.IsAdmin() Then
		  Session("Login_Link") = Request.RawUrl
		  Response.Redirect("/mhweb/login/login.aspx")
		End If
		objProfile = CustomProfile.GetTable(Share.ProjectName, SiteLinkinf.TableVar)
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
				key = New SiteLinkkey()
				Dim messageList As ArrayList = SiteLinkinf.LoadKey(key) 
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
			key = TryCast(ViewState("key"), SiteLinkkey) ' restore from ViewState for postback
		End If
				Application("FCKeditor:UserFilesPath") = Replace(Request.ApplicationPath & Session("SiteGallery"), "//", "/") ' Set upload path
		If (SiteLinkDetailsView.Visible) Then
			RegisterClientID("CtrlID", SiteLinkDetailsView)
		End If
		If (SiteLinkDetailsView.FindControl("x_SiteCategoryTypeID") IsNot Nothing) Then
			Page.Form.DefaultFocus = SiteLinkDetailsView.FindControl("x_SiteCategoryTypeID").ClientID
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

	Protected Sub SiteLinkDetailsView_Init(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			SiteLinkdal.OpenConnection()
		Catch err As Exception
			lblMessage.Text = err.Message
			pnlMessage.Visible = True
		End Try
	End Sub

	' *************************************
	' *  Handler for DetailsView DataBound
	' *************************************

	Protected Sub SiteLinkDetailsView_DataBound(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim source As DetailsView = SiteLinkDetailsView
		Dim row As SiteLinkrow = TryCast(SiteLinkDetailsView.DataItem, SiteLinkrow) ' get data object
		If (SiteLinkDetailsView.CurrentMode = DetailsViewMode.Edit) Then
			If (row IsNot Nothing) Then
				key.ID = Convert.ToInt32(row.ID)
			End If
			RowToControl(row, source, Core.CtrlType.Edit)
		End If
	End Sub

	' ***********************************************
	' *  Generate Clientside Javascript for controls
	' ***********************************************

	Protected Sub SiteLinkDetailsView_ItemCreated(ByVal sender As Object, ByVal e As System.EventArgs)
		If (SiteLinkDetailsView.CurrentMode = DetailsViewMode.Edit) Then
		End If
	End Sub

	' ****************************************
	' *  Handler for DetailsView ItemUpdating
	' ****************************************

	Protected Sub SiteLinkDetailsView_ItemUpdating(ByVal sender As Object, ByVal e As DetailsViewUpdateEventArgs)
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

	Protected Sub SiteLinkDetailsView_ItemUpdated(ByVal sender As Object, ByVal e As DetailsViewUpdatedEventArgs)

		' Check If update successful 
		If (e.AffectedRows = 0) Then

			' Keep in edit mode 
			e.KeepInEditMode = True

			' Get original values 
			Dim source As WebControl = TryCast(sender, WebControl)
			If (source Is Nothing) Then Return
			Dim oldrow As SiteLinkrow = New SiteLinkrow()
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

	Protected Sub SiteLinkDetailsView_Unload(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			SiteLinkdal.CloseAndDisposeConnection()
		Catch
		End Try
	End Sub

	' ********************************
	' * Register ClientID of Controls
	' ********************************

	Private Sub RegisterClientID(ByVal sBlockName As String, ByVal control As Control)
		Dim jsString As String = String.Empty
		If (control.FindControl("x_SiteCategoryTypeID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_SiteCategoryTypeID'] = '" & control.FindControl("x_SiteCategoryTypeID").ClientID & "';"
		End If
		If (control.FindControl("x_CategoryID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_CategoryID'] = '" & control.FindControl("x_CategoryID").ClientID & "';"
		End If
		If (control.FindControl("x_LinkTypeCD") IsNot Nothing) Then
			jsString &= "ew.Controls['x_LinkTypeCD'] = '" & control.FindControl("x_LinkTypeCD").ClientID & "';"
		End If
		If (control.FindControl("x_Title") IsNot Nothing) Then
			jsString &= "ew.Controls['x_Title'] = '" & control.FindControl("x_Title").ClientID & "';"
		End If
		If (control.FindControl("x_SiteCategoryID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_SiteCategoryID'] = '" & control.FindControl("x_SiteCategoryID").ClientID & "';"
		End If
		If (control.FindControl("x_SiteCategoryGroupID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_SiteCategoryGroupID'] = '" & control.FindControl("x_SiteCategoryGroupID").ClientID & "';"
		End If
		If (control.FindControl("x_CompanyID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_CompanyID'] = '" & control.FindControl("x_CompanyID").ClientID & "';"
		End If
		If (control.FindControl("x_Description") IsNot Nothing) Then
			jsString &= "ew.Controls['x_Description'] = '" & control.FindControl("x_Description").ClientID & "';"
		End If
		If (control.FindControl("x_URL") IsNot Nothing) Then
			jsString &= "ew.Controls['x_URL'] = '" & control.FindControl("x_URL").ClientID & "';"
		End If
		If (control.FindControl("x_DateAdd") IsNot Nothing) Then
			jsString &= "ew.Controls['x_DateAdd'] = '" & control.FindControl("x_DateAdd").ClientID & "';"
		End If
		If (control.FindControl("x_Ranks") IsNot Nothing) Then
			jsString &= "ew.Controls['x_Ranks'] = '" & control.FindControl("x_Ranks").ClientID & "';"
		End If
		If (control.FindControl("x_Views") IsNot Nothing) Then
			jsString &= "ew.Controls['x_Views'] = '" & control.FindControl("x_Views").ClientID & "';"
		End If
		If (control.FindControl("x_UserName") IsNot Nothing) Then
			jsString &= "ew.Controls['x_UserName'] = '" & control.FindControl("x_UserName").ClientID & "';"
		End If
		If (control.FindControl("x_UserID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_UserID'] = '" & control.FindControl("x_UserID").ClientID & "';"
		End If
		If (control.FindControl("x_ASIN") IsNot Nothing) Then
			jsString &= "ew.Controls['x_ASIN'] = '" & control.FindControl("x_ASIN").ClientID & "';"
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
			Dim x_SiteCategoryTypeID As DropDownList = TryCast(control.FindControl("x_SiteCategoryTypeID"), DropDownList)
			If (x_SiteCategoryTypeID IsNot Nothing) Then
				If ((x_SiteCategoryTypeID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_SiteCategoryTypeID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Site Type")
				ElseIf (String.IsNullOrEmpty(x_SiteCategoryTypeID.SelectedValue)) Then
					messageList.Add("Please enter required field (Int32): Site Type")
				End If
			End If
			Dim x_CategoryID As DropDownList = TryCast(control.FindControl("x_CategoryID"), DropDownList)
			If (x_CategoryID IsNot Nothing) Then
				If ((x_CategoryID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_CategoryID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Link Category")
				End If
			End If
			Dim x_LinkTypeCD As DropDownList = TryCast(control.FindControl("x_LinkTypeCD"), DropDownList)
			If (x_LinkTypeCD IsNot Nothing) Then
				If ((x_LinkTypeCD.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckString(x_LinkTypeCD.SelectedValue))) Then
					messageList.Add("Invalid Value (String): Link Type")
				ElseIf (String.IsNullOrEmpty(x_LinkTypeCD.SelectedValue)) Then
					messageList.Add("Please enter required field (String): Link Type")
				End If
			End If
			Dim x_Title As TextBox = TryCast(control.FindControl("x_Title"), TextBox)
			If (x_Title IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_Title.Text)) Then
					messageList.Add("Invalid Value (String): Title")
				ElseIf (String.IsNullOrEmpty(x_Title.Text)) Then
					messageList.Add("Please enter required field (String): Title")
				End If
			End If
			Dim x_SiteCategoryID As DropDownList = TryCast(control.FindControl("x_SiteCategoryID"), DropDownList)
			If (x_SiteCategoryID IsNot Nothing) Then
				If ((x_SiteCategoryID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_SiteCategoryID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Site Category")
				End If
			End If
			Dim x_SiteCategoryGroupID As DropDownList = TryCast(control.FindControl("x_SiteCategoryGroupID"), DropDownList)
			If (x_SiteCategoryGroupID IsNot Nothing) Then
				If ((x_SiteCategoryGroupID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_SiteCategoryGroupID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Site Group")
				End If
			End If
			Dim x_CompanyID As DropDownList = TryCast(control.FindControl("x_CompanyID"), DropDownList)
			If (x_CompanyID IsNot Nothing) Then
				If ((x_CompanyID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_CompanyID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Company")
				End If
			End If
			Dim x_Description As TextBox = TryCast(control.FindControl("x_Description"), TextBox)
			If (x_Description IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_Description.Text)) Then
					messageList.Add("Invalid Value (String): Description")
				End If
			End If
			Dim x_URL As TextBox = TryCast(control.FindControl("x_URL"), TextBox)
			If (x_URL IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_URL.Text)) Then
					messageList.Add("Invalid Value (String): URL")
				End If
			End If
			Dim x_DateAdd As TextBox = TryCast(control.FindControl("x_DateAdd"), TextBox)
			If (x_DateAdd IsNot Nothing) Then
				If (Not DataFormat.CheckDateTime(x_DateAdd.Text)) Then
					messageList.Add("Invalid Value (DateTime): Date Add")
				End If
			End If
			Dim x_Ranks As TextBox = TryCast(control.FindControl("x_Ranks"), TextBox)
			If (x_Ranks IsNot Nothing) Then
				If (Not DataFormat.CheckInt32(x_Ranks.Text)) Then
					messageList.Add("Invalid Value (Int32): Ranks")
				End If
			End If
			Dim x_UserName As TextBox = TryCast(control.FindControl("x_UserName"), TextBox)
			If (x_UserName IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_UserName.Text)) Then
					messageList.Add("Invalid Value (String): User Name")
				End If
			End If
			Dim x_UserID As DropDownList = TryCast(control.FindControl("x_UserID"), DropDownList)
			If (x_UserID IsNot Nothing) Then
				If ((x_UserID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_UserID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): User ID")
				End If
			End If
			Dim x_ASIN As TextBox = TryCast(control.FindControl("x_ASIN"), TextBox)
			If (x_ASIN IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_ASIN.Text)) Then
					messageList.Add("Invalid Value (String): ASIN")
				End If
			End If
		End If
		Return IIf(messageList.Count = 0, Nothing, messageList)
	End Function

	' **********************************************
	' *  Convert Input Control Values to Row Object
	' **********************************************

	Private Sub ControlToRow(ByVal row As SiteLinkrow, ByVal control As WebControl)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		row.SetPageType(Core.PageType.Edit) ' set page type = list for update

		' Field ID
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

		' Field CategoryID
		Dim x_CategoryID As DropDownList = TryCast(control.FindControl("x_CategoryID"), DropDownList)
		Dim v_CategoryID As String = String.Empty
		If (x_CategoryID.SelectedIndex >= 0) Then
			For Each li As ListItem In x_CategoryID.Items
				If (li.Selected) Then
					v_CategoryID += li.Value
					Exit For
				End If
			Next
		End If
		If (v_CategoryID <> String.Empty) Then row.CategoryID = Convert.ToInt32(v_CategoryID) Else row.CategoryID = CType(Nothing, Nullable(Of Int32))

		' Field LinkTypeCD
		Dim x_LinkTypeCD As DropDownList = TryCast(control.FindControl("x_LinkTypeCD"), DropDownList)
		Dim v_LinkTypeCD As String = String.Empty
		If (x_LinkTypeCD.SelectedIndex >= 0) Then
			For Each li As ListItem In x_LinkTypeCD.Items
				If (li.Selected) Then
					v_LinkTypeCD += li.Value
					Exit For
				End If
			Next
		End If
		If (v_LinkTypeCD <> String.Empty) Then row.LinkTypeCD = v_LinkTypeCD Else row.LinkTypeCD = String.Empty

		' Field Title
		Dim x_Title As TextBox = TryCast(control.FindControl("x_Title"), TextBox)
		If (x_Title.Text <> String.Empty) Then row.Title = x_Title.Text Else row.Title = String.Empty

		' Field SiteCategoryID
		Dim x_SiteCategoryID As DropDownList = TryCast(control.FindControl("x_SiteCategoryID"), DropDownList)
		Dim v_SiteCategoryID As String = String.Empty
		If (x_SiteCategoryID.SelectedIndex >= 0) Then
			For Each li As ListItem In x_SiteCategoryID.Items
				If (li.Selected) Then
					v_SiteCategoryID += li.Value
					Exit For
				End If
			Next
		End If
		If (v_SiteCategoryID <> String.Empty) Then row.SiteCategoryID = Convert.ToInt32(v_SiteCategoryID) Else row.SiteCategoryID = CType(Nothing, Nullable(Of Int32))

		' Field SiteCategoryGroupID
		Dim x_SiteCategoryGroupID As DropDownList = TryCast(control.FindControl("x_SiteCategoryGroupID"), DropDownList)
		Dim v_SiteCategoryGroupID As String = String.Empty
		If (x_SiteCategoryGroupID.SelectedIndex >= 0) Then
			For Each li As ListItem In x_SiteCategoryGroupID.Items
				If (li.Selected) Then
					v_SiteCategoryGroupID += li.Value
					Exit For
				End If
			Next
		End If
		If (v_SiteCategoryGroupID <> String.Empty) Then row.SiteCategoryGroupID = Convert.ToInt32(v_SiteCategoryGroupID) Else row.SiteCategoryGroupID = CType(Nothing, Nullable(Of Int32))

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

		' Field Description
		Dim x_Description As TextBox = TryCast(control.FindControl("x_Description"), TextBox)
		If (x_Description.Text <> String.Empty) Then row.Description = x_Description.Text Else row.Description = Nothing

		' Field URL
		Dim x_URL As TextBox = TryCast(control.FindControl("x_URL"), TextBox)
		If (x_URL.Text <> String.Empty) Then row.URL = x_URL.Text Else row.URL = Nothing

		' Field DateAdd
		Dim x_DateAdd As TextBox = TryCast(control.FindControl("x_DateAdd"), TextBox)
		If (x_DateAdd.Text <> String.Empty) Then row.DateAdd = Convert.ToDateTime(DataFormat.UnFormatDateTime(x_DateAdd.Text, 6, "/"c)) Else row.DateAdd = CType(Nothing, Nullable(Of DateTime))

		' Field Ranks
		Dim x_Ranks As TextBox = TryCast(control.FindControl("x_Ranks"), TextBox)
		If (x_Ranks.Text <> String.Empty) Then row.Ranks = Convert.ToInt32(x_Ranks.Text) Else row.Ranks = CType(Nothing, Nullable(Of Int32))

		' Field Views
		Dim x_Views As CheckBox = TryCast(control.FindControl("x_Views"), CheckBox)
		If (x_Views.Checked) Then
			row.Views = True
		Else
			row.Views = False
		End If

		' Field UserName
		Dim x_UserName As TextBox = TryCast(control.FindControl("x_UserName"), TextBox)
		If (x_UserName.Text <> String.Empty) Then row.UserName = x_UserName.Text Else row.UserName = Nothing

		' Field UserID
		Dim x_UserID As DropDownList = TryCast(control.FindControl("x_UserID"), DropDownList)
		Dim v_UserID As String = String.Empty
		If (x_UserID.SelectedIndex >= 0) Then
			For Each li As ListItem In x_UserID.Items
				If (li.Selected) Then
					v_UserID += li.Value
					Exit For
				End If
			Next
		End If
		If (v_UserID <> String.Empty) Then row.UserID = Convert.ToInt32(v_UserID) Else row.UserID = CType(Nothing, Nullable(Of Int32))

		' Field ASIN
		Dim x_ASIN As TextBox = TryCast(control.FindControl("x_ASIN"), TextBox)
		If (x_ASIN.Text <> String.Empty) Then row.ASIN = x_ASIN.Text Else row.ASIN = Nothing
	End Sub

	' ****************************
	' *  Check for duplicate Key
	' ****************************

	Private Function CheckDuplicateKey(ByVal control As WebControl) As ArrayList 
		Dim sWhere As String = String.Empty
		Dim messageList As ArrayList = New ArrayList()
		Dim data As SiteLinkdal = New SiteLinkdal()
		Dim newkey As SiteLinkkey = New SiteLinkkey()
		Try
			Dim sKeyWhere As string = data.KeyFilter(key)
		data = Nothing
		Catch e As Exception
			messageList.Add(e.Message)
		End Try
		Return IIf(messageList.Count = 0, Nothing, messageList)
	End Function

	' ********************************************
	' *  Convert Row Object Value To Input Control
	' ********************************************

	Private Sub RowToControl(ByVal row As SiteLinkrow, ByVal control As WebControl, ByVal ct As Core.CtrlType)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		If (ct = Core.CtrlType.Edit) Then ' set up edit control

			' Field SiteCategoryTypeID
			If (control.FindControl("ax_SiteCategoryTypeID") IsNot Nothing) Then
				Dim cdd As CascadingDropDown = TryCast(control.FindControl("ax_SiteCategoryTypeID"), CascadingDropDown)
				Dim v_SiteCategoryTypeID As String
				If (row.SiteCategoryTypeID.HasValue) Then v_SiteCategoryTypeID = Convert.ToString(row.SiteCategoryTypeID) Else v_SiteCategoryTypeID = String.Empty
				cdd.SelectedValue = v_SiteCategoryTypeID
			End If

			' Field CategoryID
			Dim x_CategoryID As DropDownList = TryCast(control.FindControl("x_CategoryID"), DropDownList)
			x_CategoryID.DataValueField = "ewValueField"
			x_CategoryID.DataTextField = "ewTextField"
			Dim dv_x_CategoryID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
			If (dv_x_CategoryID Is Nothing) Then dv_x_CategoryID = SiteLinkdal.LookUpTable("CategoryID")
			x_CategoryID.DataSource = dv_x_CategoryID
			x_CategoryID.DataBind()
			x_CategoryID.Items.Insert(0, New ListItem("Please Select", ""))
			Dim v_CategoryID As String
			If (row.CategoryID.HasValue) Then v_CategoryID = Convert.ToString(row.CategoryID) Else v_CategoryID = String.Empty
			x_CategoryID.ClearSelection()
			For Each li As ListItem In x_CategoryID.Items
				If (li.Value.ToString() = v_CategoryID) Then
					li.Selected = True
					Exit For
				End If
			Next

			' Field LinkTypeCD
			Dim x_LinkTypeCD As DropDownList = TryCast(control.FindControl("x_LinkTypeCD"), DropDownList)
			x_LinkTypeCD.DataValueField = "ewValueField"
			x_LinkTypeCD.DataTextField = "ewTextField"
			Dim dv_x_LinkTypeCD As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
			If (dv_x_LinkTypeCD Is Nothing) Then dv_x_LinkTypeCD = SiteLinkdal.LookUpTable("LinkTypeCD")
			x_LinkTypeCD.DataSource = dv_x_LinkTypeCD
			x_LinkTypeCD.DataBind()
			x_LinkTypeCD.Items.Insert(0, New ListItem("Please Select", ""))
			Dim v_LinkTypeCD As String 
			If (row.LinkTypeCD IsNot Nothing) Then  v_LinkTypeCD = Convert.ToString(row.LinkTypeCD) Else v_LinkTypeCD = String.Empty
			x_LinkTypeCD.ClearSelection()
			For Each li As ListItem In x_LinkTypeCD.Items
				If (li.Value.ToString() = v_LinkTypeCD) Then
					li.Selected = True
					Exit For
				End If
			Next

			' Field Title
			Dim x_Title As TextBox = TryCast(control.FindControl("x_Title"), TextBox)
			If (row.Title IsNot Nothing) Then x_Title.Text = row.Title.ToString() Else x_Title.Text = String.Empty

			' Field SiteCategoryID
			If (control.FindControl("ax_SiteCategoryID") IsNot Nothing) Then
				Dim cdd As CascadingDropDown = TryCast(control.FindControl("ax_SiteCategoryID"), CascadingDropDown)
				Dim v_SiteCategoryID As String
				If (row.SiteCategoryID.HasValue) Then v_SiteCategoryID = Convert.ToString(row.SiteCategoryID) Else v_SiteCategoryID = String.Empty
				cdd.SelectedValue = v_SiteCategoryID
			End If

			' Field SiteCategoryGroupID
			Dim x_SiteCategoryGroupID As DropDownList = TryCast(control.FindControl("x_SiteCategoryGroupID"), DropDownList)
			x_SiteCategoryGroupID.DataValueField = "ewValueField"
			x_SiteCategoryGroupID.DataTextField = "ewTextField"
			Dim dv_x_SiteCategoryGroupID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
			If (dv_x_SiteCategoryGroupID Is Nothing) Then dv_x_SiteCategoryGroupID = SiteLinkdal.LookUpTable("SiteCategoryGroupID")
			x_SiteCategoryGroupID.DataSource = dv_x_SiteCategoryGroupID
			x_SiteCategoryGroupID.DataBind()
			x_SiteCategoryGroupID.Items.Insert(0, New ListItem("Please Select", ""))
			Dim v_SiteCategoryGroupID As String
			If (row.SiteCategoryGroupID.HasValue) Then v_SiteCategoryGroupID = Convert.ToString(row.SiteCategoryGroupID) Else v_SiteCategoryGroupID = String.Empty
			x_SiteCategoryGroupID.ClearSelection()
			For Each li As ListItem In x_SiteCategoryGroupID.Items
				If (li.Value.ToString() = v_SiteCategoryGroupID) Then
					li.Selected = True
					Exit For
				End If
			Next

			' Field CompanyID
			If (control.FindControl("ax_CompanyID") IsNot Nothing) Then
				Dim cdd As CascadingDropDown = TryCast(control.FindControl("ax_CompanyID"), CascadingDropDown)
				Dim v_CompanyID As String
				If (row.CompanyID.HasValue) Then v_CompanyID = Convert.ToString(row.CompanyID) Else v_CompanyID = String.Empty
				cdd.SelectedValue = v_CompanyID
			End If

			' Field Description
			Dim x_Description As TextBox = TryCast(control.FindControl("x_Description"), TextBox)
			If (row.Description IsNot Nothing) Then x_Description.Text = row.Description.ToString() Else x_Description.Text = String.Empty

			' Field URL
			Dim x_URL As TextBox = TryCast(control.FindControl("x_URL"), TextBox)
			If (row.URL IsNot Nothing) Then x_URL.Text = row.URL.ToString() Else x_URL.Text = String.Empty

			' Field DateAdd
			Dim x_DateAdd As TextBox = TryCast(control.FindControl("x_DateAdd"), TextBox)
			If (row.DateAdd.HasValue) Then x_DateAdd.Text = DataFormat.DateTimeFormat(6, "/", row.DateAdd) Else x_DateAdd.Text = String.Empty

			' Field Ranks
			Dim x_Ranks As TextBox = TryCast(control.FindControl("x_Ranks"), TextBox)
			If (row.Ranks.HasValue) Then x_Ranks.Text = row.Ranks.ToString() Else x_Ranks.Text = String.Empty

			' Field Views
			Dim x_Views As CheckBox = TryCast(control.FindControl("x_Views"), CheckBox)
			Dim sViews As String = row.Views.ToString()
			If ((sViews IsNot Nothing) AndAlso sViews <> "") Then
				If (Convert.ToBoolean(sViews)) Then
					x_Views.Checked = True
				Else
					x_Views.Checked = False
				End If
			End If

			' Field UserName
			Dim x_UserName As TextBox = TryCast(control.FindControl("x_UserName"), TextBox)
			If (row.UserName IsNot Nothing) Then x_UserName.Text = row.UserName.ToString() Else x_UserName.Text = String.Empty

			' Field UserID
			Dim x_UserID As DropDownList = TryCast(control.FindControl("x_UserID"), DropDownList)
			x_UserID.DataValueField = "ewValueField"
			x_UserID.DataTextField = "ewTextField"
			Dim dv_x_UserID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
			If (dv_x_UserID Is Nothing) Then dv_x_UserID = SiteLinkdal.LookUpTable("UserID")
			x_UserID.DataSource = dv_x_UserID
			x_UserID.DataBind()
			x_UserID.Items.Insert(0, New ListItem("Please Select", ""))
			Dim v_UserID As String
			If (row.UserID.HasValue) Then v_UserID = Convert.ToString(row.UserID) Else v_UserID = String.Empty
			x_UserID.ClearSelection()
			For Each li As ListItem In x_UserID.Items
				If (li.Value.ToString() = v_UserID) Then
					li.Selected = True
					Exit For
				End If
			Next

			' Field ASIN
			Dim x_ASIN As TextBox = TryCast(control.FindControl("x_ASIN"), TextBox)
			If (row.ASIN IsNot Nothing) Then x_ASIN.Text = row.ASIN.ToString() Else x_ASIN.Text = String.Empty
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

	Protected Sub SiteLinkDataSource_Selecting(Byval sender As Object, ByVal e As ObjectDataSourceSelectingEventArgs)
		e.InputParameters.Clear()
		e.InputParameters.Add("filter", SiteLinkinf.GetUserFilter())
	End Sub

	' *************************************
	' *  DataSource Selected Event Handler
	' *************************************

	Protected Sub SiteLinkDataSource_Selected(Byval sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.ReturnValue Is Nothing) Then
			If (e.Exception IsNot Nothing) Then
				lblMessage.Text = e.Exception.Message
				pnlMessage.Visible = True
			Else
				lblMessage.Text = "No records found"
				pnlMessage.Visible = True
			End If
		Else
			Dim rows As SiteLinkrows = TryCast(e.ReturnValue, SiteLinkrows)
			Dim i As Integer = 0
			Do While (rows IsNot Nothing AndAlso i < rows.Count)
				If (key.Equals(rows(i).GetKey())) Then
					SiteLinkDetailsView.PageIndex = i
					Exit Do
				End If
				i += 1
			Loop
		End If
	End Sub

	' ***********************************
	' *  Handler for DataSource Updating
	' ***********************************

	Protected Sub SiteLinkDataSource_Updating(ByVal sender As Object, ByVal e As ObjectDataSourceMethodEventArgs)
		Dim source As DetailsView = SiteLinkDetailsView

		' Set up row object
		Dim row As SiteLinkrow = TryCast(e.InputParameters(0), SiteLinkrow)
		Dim data As SiteLinkdal = New SiteLinkdal()
		key.ID = Convert.ToInt32(row.ID)
		oldrow = data.LoadRow(key, SiteLinkinf.GetUserFilter())
		ControlToRow(row, source) ' load values of controls to row
		newrow = row
		If (SiteLinkbll.Updating(oldrow, newrow)) Then
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

	Protected Sub SiteLinkDataSource_Updated(ByVal sender As Object, ByVal e As ObjectDataSourceStatusEventArgs)
		If (e.Exception IsNot Nothing) Then
			e.AffectedRows = 0
			e.ExceptionHandled = True
			lblMessage.Text = e.Exception.InnerException.Message
		Else
			lblMessage.Text = "Update successful"
			SiteLinkbll.Updated(oldrow, newrow)
		End If
		If (lblMessage.Text <> "") Then pnlMessage.Visible = True
	End Sub

	' ************************************************
	' *  Change the page index for DetailsView paging
	' ************************************************

	Protected Sub ChangePageIndex(ByVal sender As Object, ByVal e As DetailsViewPageEventArgs)
		Dim data As SiteLinkdal = New SiteLinkdal()
		Dim rows As SiteLinkrows = data.LoadList(SiteLinkinf.GetUserFilter())
		If (rows IsNot Nothing AndAlso rows.Count >= e.NewPageIndex) Then
			key = rows(e.NewPageIndex).GetKey()
		End If
	End Sub

	' *******************
	' *  Get User Filter
	' *******************

	Private Function GetUserFilter() As String 
		Return SiteLinkinf.GetUserFilter()
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
<script type="text/javascript" src="fckeditor/fckeditor.js"></script>
<script type="text/javascript">
<!--
    _width_multiplier = 16;
    _height_multiplier = 60;
    var ew_DHTMLEditors = [];
    function ew_UpdateTextArea() {
    	if (typeof ew_DHTMLEditors != 'undefined' &&
    		typeof FCKeditorAPI != 'undefined') {			
    			var inst;			
    			for (inst in FCKeditorAPI.__Instances) {
    			    with (FCKeditorAPI.__Instances[inst]) {
        				UpdateLinkedField();
        				LinkedField.value = ew_RemoveSpaces(LinkedField.value); // add and modify ew_RemoveSpaces() 
    			    }
    			}
    	}
    }
    function ew_ResetTextArea(elem) {
    	if (typeof ew_DHTMLEditors != 'undefined' &&
    		typeof FCKeditorAPI != 'undefined') {
    		    var oEditor = FCKeditorAPI.GetInstance(elem) ;// Get the editor instance that we want to interact with.
    		    oEditor.EditorDocument.body.innerHTML = '';
    	}
    }
//-->
</script>
	<p><span class="aspnetmaker">Edit TABLE: Site Link</span></p>
	<p>
		<asp:Hyperlink id="lnkList" Text="Back to List" NavigateUrl="sitelink_list.aspx" CssClass="aspnetmaker" runat="server" />
		<asp:Label id="lblReturnUrl" Visible="False" CssClass="aspnetmaker" runat="server">sitelink_list.aspx</asp:Label>
	</p>
	<asp:ValidationSummary id="xevs_SiteLink" CssClass="aspnetmaker" runat="server"
		HeaderText=""
		ShowSummary="False"
		ShowMessageBox="True"
		ForeColor="#FF0000" />
	<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
		<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
	</asp:PlaceHolder>
<!-- Data Source -->
<asp:ObjectDataSource ID="SiteLinkDataSource"
	TypeName="PMGEN.SiteLinkdal"
	DataObjectTypeName="PMGEN.SiteLinkrow"
	SelectMethod="LoadList"
	UpdateMethod="Update"
	OnSelecting="SiteLinkDataSource_Selecting"
	OnSelected="SiteLinkDataSource_Selected"
	OnUpdating="SiteLinkDataSource_Updating"
	OnUpdated="SiteLinkDataSource_Updated"
	runat="server">
</asp:ObjectDataSource>
<!-- DetailsView -->
	<asp:DetailsView ID="SiteLinkDetailsView"
		DataKeyNames="ID"
		DataSourceID="SiteLinkDataSource"
		DefaultMode="Edit"
		GridLines="None"
		AutoGenerateRows="False" CssClass="ewDetailTable"
		OnInit="SiteLinkDetailsView_Init"
		OnDataBound="SiteLinkDetailsView_DataBound"
		OnItemCreated="SiteLinkDetailsView_ItemCreated"
		OnItemUpdating="SiteLinkDetailsView_ItemUpdating"
		OnItemUpdated="SiteLinkDetailsView_ItemUpdated"
		OnUnload="SiteLinkDetailsView_Unload"
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
					<asp:Label runat="server" id="xs_SiteCategoryTypeID">Site Type<span class='ewmsg'>&nbsp;*</span></asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:DropDownList ID="x_SiteCategoryTypeID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
<ew:CascadingDropDown ID="ax_SiteCategoryTypeID" runat="server" EncryptKey="<%$ EWCode: Share.RandomKey %>"
	TargetControlID="x_SiteCategoryTypeID" Category="x_SiteCategoryTypeID" 
	PromptText="Please Select" ServiceMethod="GetDropDownList" ServicePath="ewWebService.asmx" 
	SqlCommand="SELECT [SiteCategoryTypeID], [SiteCategoryTypeNM] FROM [SiteCategoryType] ORDER BY [SiteCategoryTypeNM] Asc"
	DisplayFieldNames="SiteCategoryTypeNM" EnabledDropDown="true" />
<asp:RequiredFieldValidator ID="vx_SiteCategoryTypeID" CssClass="aspnetmaker" runat="server" ControlToValidate="x_SiteCategoryTypeID" ErrorMessage="Site Type Is Required" ForeColor="Red" Display="None" Text="*" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_CategoryID">Link Category</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:DropDownList ID="x_CategoryID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_LinkTypeCD">Link Type<span class='ewmsg'>&nbsp;*</span></asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:DropDownList ID="x_LinkTypeCD" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
<asp:RequiredFieldValidator ID="vx_LinkTypeCD" CssClass="aspnetmaker" runat="server" ControlToValidate="x_LinkTypeCD" ErrorMessage="Please enter required field - Link Type" ForeColor="Red" Display="None" Text="*" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_Title">Title<span class='ewmsg'>&nbsp;*</span></asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_Title" Columns="30" MaxLength="255" CssClass="aspnetmaker" runat="server" />
<asp:RequiredFieldValidator ID="vx_Title" CssClass="aspnetmaker" runat="server" ControlToValidate="x_Title" ErrorMessage="Please enter required field - Title" ForeColor="Red" Display="None" Text="*" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_SiteCategoryID">Site Category</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:DropDownList ID="x_SiteCategoryID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
<ew:CascadingDropDown ID="ax_SiteCategoryID" runat="server" EncryptKey="<%$ EWCode: Share.RandomKey %>"
	TargetControlID="x_SiteCategoryID" Category="x_SiteCategoryID" 
	PromptText="Please Select" ServiceMethod="GetDropDownList" ServicePath="ewWebService.asmx" 
	SqlCommand="SELECT [SiteCategoryID], [CategoryName] FROM [SiteCategory] WHERE ([SiteCategoryTypeID]=@FILTER_VALUE) ORDER BY [CategoryName] Asc"
	ParentCategory="x_SiteCategoryTypeID" ParentControlID="x_SiteCategoryTypeID"
	DisplayFieldNames="CategoryName" EnabledDropDown="true" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_SiteCategoryGroupID">Site Group</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:DropDownList ID="x_SiteCategoryGroupID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_CompanyID">Company</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:DropDownList ID="x_CompanyID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
<ew:CascadingDropDown ID="ax_CompanyID" runat="server" EncryptKey="<%$ EWCode: Share.RandomKey %>"
	TargetControlID="x_CompanyID" Category="x_CompanyID" 
	PromptText="Please Select" ServiceMethod="GetDropDownList" ServicePath="ewWebService.asmx" 
	SqlCommand="SELECT [CompanyID], [CompanyName] FROM [Company] WHERE ([SiteCategoryTypeID]=@FILTER_VALUE) ORDER BY [CompanyName] Asc"
	ParentCategory="x_SiteCategoryTypeID" ParentControlID="x_SiteCategoryTypeID"
	DisplayFieldNames="CompanyName" EnabledDropDown="true" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_Description">Description</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_Description" TextMode="MultiLine" Rows="5" Columns="90" CssClass="aspnetmaker" runat="server" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_URL">URL</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_URL" TextMode="MultiLine" Rows="15" Columns="50" CssClass="aspnetmaker" runat="server" />
<script type="text/javascript">
<!--
var editor = new ew_DHTMLEditor(ew.Controls['x_URL']);
editor.create = function() {
	var sBasePath = 'fckeditor/';
	var oFCKeditor = new FCKeditor(ew.Controls['x_URL'], 50*_width_multiplier, 15*_height_multiplier);
	oFCKeditor.BasePath = sBasePath;
	oFCKeditor.ReplaceTextarea();
	this.active = true;
}
ew_DHTMLEditors[ew_DHTMLEditors.length] = editor;
-->
</script>
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_DateAdd">Date Add</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_DateAdd" CssClass="aspnetmaker" runat="server" />
<ew:USDateValidator ID="vc_DateAdd" ClientValidationFunction="ew_CheckUSDate" CssClass="aspnetmaker" runat="server" ControlToValidate="x_DateAdd" ErrorMessage="Incorrect date, format = mm/dd/yyyy - Date Add" Display="None" ForeColor="Red" DateSeparator="/" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_Ranks">Ranks</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_Ranks" Columns="30" CssClass="aspnetmaker" runat="server" />
<ew:IntegerValidator ID="vc_Ranks" ClientValidationFunction="ew_CheckInteger" CssClass="aspnetmaker" runat="server" ControlToValidate="x_Ranks" ErrorMessage="Incorrect integer - Ranks" Display="None" ForeColor="Red" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_Views">Views<span class='ewmsg'>&nbsp;*</span></asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:CheckBox ID="x_Views" CssClass="aspnetmakerlist" runat="server" />
<asp:HiddenField ID="hx_Views" Runat="server" Value='<%# Bind("Views") %>' />
<ew:CheckBoxListValidator ID="vx_Views" ClientValidationFunction="ew_CheckBoxListHasValue" CssClass="aspnetmaker" runat="server" ControlToValidate="x_Views" ErrorMessage="Please enter required field - Views" ForeColor="Red"  Display="Dynamic" Text="*"/>
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_UserName">User Name</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_UserName" Columns="30" MaxLength="50" CssClass="aspnetmaker" runat="server" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_UserID">User ID</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:DropDownList ID="x_UserID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_ASIN">ASIN</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_ASIN" Columns="30" MaxLength="50" CssClass="aspnetmaker" runat="server" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField ShowHeader="False">
				<ItemStyle CssClass="ewTableFooter" />
				<EditItemTemplate>
				<table border="0">
					<tr>
						<td><asp:LinkButton ID="btnEdit" CssClass="aspnetmaker" runat="server" CausesValidation="True" CommandName="Update" Text="Update" onClientClick="ew_UpdateTextArea()"></asp:LinkButton></td>
						<td><asp:LinkButton ID="btnCancel" CssClass="aspnetmaker" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel" OnClick="btnCancel_Click"></asp:LinkButton></td>
					</tr>
				</table>
				<asp:HiddenField id="x_ID" runat="server" />
				</EditItemTemplate>
			</asp:TemplateField>
		</Fields>
	</asp:DetailsView>
	<script type="text/javascript">
	<!--
		ew_CreateEditor();  // Create DHTML editor(s)
	//-->
	</script>
	<br />
	<input type="hidden" id="keyID" runat="server" />
</asp:Content>
