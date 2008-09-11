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
	Dim key As Linkkey ' record key
	Dim oldrow As Linkrow ' old record data input by user
	Dim newrow As Linkrow ' new record data input by user
	Dim objProfile As TableProfile ' table profile

	' *************************
	' *  Handler for Page Init
	' *************************

	Protected Sub Page_Init(ByVal s As Object, ByVal e As System.EventArgs)
		If Not mhUser.IsAdmin() Then
		  Session("Login_Link") = Request.RawUrl
		  Response.Redirect("/mhweb/login/login.aspx")
		End If
		objProfile = CustomProfile.GetTable(Share.ProjectName, Linkinf.TableVar)
	End Sub

	' *************************
	' *  Handler for Page Load
	' *************************

	Protected Sub Page_Load(ByVal s As Object, ByVal e As System.EventArgs)
		Page.ClientScript.RegisterClientScriptInclude("ewv", "ewv.js")
		Response.Cache.SetCacheability(HttpCacheability.NoCache)
		If (Not Page.IsPostBack) Then
			If (Request.QueryString.Count > 0) Then
				key = New Linkkey()
				Dim messageList As ArrayList = Linkinf.LoadKey(key)
				If (messageList IsNot Nothing) Then key = Nothing ' error, ignore
			End If
		End If
				Application("FCKeditor:UserFilesPath") = Replace(Request.ApplicationPath & Session("SiteGallery"), "//", "/") ' Set upload path
		If (LinkDetailsView.Visible) Then
			RegisterClientID("CtrlID", LinkDetailsView)
		End If
			If (LinkDetailsView.FindControl("x_Title") IsNot Nothing) Then
				Page.Form.DefaultFocus = LinkDetailsView.FindControl("x_Title").ClientID
			End If
	End Sub

	' **********************************************
	' *  Load Copy Data & Default Data to Controls
	' **********************************************

	Private Sub LoadData()
		Dim source As WebControl = TryCast(LinkDetailsView, WebControl)
		If (oldrow IsNot Nothing) Then ' old record (from recovery) exists, just restore
			RowToControl(oldrow, source, Core.CtrlType.Edit)
			oldrow = Nothing
		ElseIf (key IsNot Nothing) Then ' key loaded, try to copy record
			Dim data As Linkdal = New Linkdal()
			Dim row As Linkrow = data.LoadRow(key, Linkinf.GetUserFilter())
			If (row Is Nothing) Then
				objProfile.Message = "Record not found"
				Response.Redirect(lblReturnUrl.Text)
			End If
			RowToControl(row, source, Core.CtrlType.Edit)
			row = Nothing
		Else 
			Dim row As Linkrow = New Linkrow()
			row.LinkTypeCD = Convert.ToString(0) ' set default value
			row.CategoryID = Convert.ToInt32(0) ' set default value
			row.CompanyID = Convert.ToInt32(val(Session("CompanyID"))) ' set default value
			row.PageID = Convert.ToInt32(val(Session("CurrentPageID"))) ' set default value
			row.DateAdd = Convert.ToDateTime(DateTime.Now) ' set default value
			row.Ranks = Convert.ToInt32(0) ' set default value
			row.Views = Convert.ToBoolean(1) ' set default value
			row.UserID = Convert.ToInt32(val(Session("ContactID"))) ' set default value
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

	Protected Sub LinkDetailsView_Init(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			Linkdal.OpenConnection()
		Catch err As Exception
			lblMessage.Text = err.Message
			pnlMessage.Visible = True
		End Try
	End Sub

	' *************************************
	' *  Handler for DetailsView DataBound
	' *************************************

	Protected Sub LinkDetailsView_DataBound(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim source As DetailsView = LinkDetailsView
		If (LinkDetailsView.CurrentMode = DetailsViewMode.Insert) Then
			LoadData()
		End If
	End Sub

	' ***************************************
	' *  Handler for DetailsView ItemCreated
	' ***************************************

	Protected Sub LinkDetailsView_ItemCreated(ByVal sender As Object, ByVal e As System.EventArgs)
	End Sub

	' *****************************************
	' *  Handler for DetailsView ItemInserting
	' *****************************************

	Protected Sub LinkDetailsView_ItemInserting(ByVal sender As Object, ByVal e As DetailsViewInsertEventArgs)
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

	Protected Sub LinkDetailsView_ItemInserted(ByVal sender As Object, ByVal e As DetailsViewInsertedEventArgs)
		If (e.AffectedRows = 0) Then

			' Keep in insert mode
			e.KeepInInsertMode = True

			' Get original values
			Dim source As WebControl = TryCast(sender, WebControl)
			If (source Is Nothing) Then Return
			oldrow = New Linkrow()
			ControlToRow(oldrow, source)
		Else
			objProfile.Message = "Add new record successful"
			Response.Redirect(lblReturnUrl.Text)
		End If
	End Sub

	' *********************************
	' *  Handler for DetailsView Unload
	' *********************************

	Protected Sub LinkDetailsView_Unload(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			Linkdal.CloseAndDisposeConnection()
		Catch
		End Try
	End Sub

	' ********************************
	' * Register ClientID of Controls
	' ********************************

	Private Sub RegisterClientID(ByVal sBlockName As String, ByVal control As Control)
		Dim jsString As String = String.Empty
		If (control.FindControl("x_Title") IsNot Nothing) Then
			jsString &= "ew.Controls['x_Title'] = '" & control.FindControl("x_Title").ClientID & "';"
		End If
		If (control.FindControl("x_Description") IsNot Nothing) Then
			jsString &= "ew.Controls['x_Description'] = '" & control.FindControl("x_Description").ClientID & "';"
		End If
		If (control.FindControl("x_LinkTypeCD") IsNot Nothing) Then
			jsString &= "ew.Controls['x_LinkTypeCD'] = '" & control.FindControl("x_LinkTypeCD").ClientID & "';"
		End If
		If (control.FindControl("x_CategoryID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_CategoryID'] = '" & control.FindControl("x_CategoryID").ClientID & "';"
		End If
		If (control.FindControl("x_CompanyID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_CompanyID'] = '" & control.FindControl("x_CompanyID").ClientID & "';"
		End If
		If (control.FindControl("x_PageID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_PageID'] = '" & control.FindControl("x_PageID").ClientID & "';"
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
		If (control.FindControl("x_UserID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_UserID'] = '" & control.FindControl("x_UserID").ClientID & "';"
		End If
		If (control.FindControl("x_SiteCategoryGroupID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_SiteCategoryGroupID'] = '" & control.FindControl("x_SiteCategoryGroupID").ClientID & "';"
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
			Dim x_Title As TextBox = TryCast(control.FindControl("x_Title"), TextBox)
			If (x_Title IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_Title.Text)) Then
					messageList.Add("Invalid Value (String): Title")
				End If
			End If
			Dim x_Description As TextBox = TryCast(control.FindControl("x_Description"), TextBox)
			If (x_Description IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_Description.Text)) Then
					messageList.Add("Invalid Value (String): Description")
				End If
			End If
			Dim x_LinkTypeCD As DropDownList = TryCast(control.FindControl("x_LinkTypeCD"), DropDownList)
			If (x_LinkTypeCD IsNot Nothing) Then
				If ((x_LinkTypeCD.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckString(x_LinkTypeCD.SelectedValue))) Then
					messageList.Add("Invalid Value (String): Type")
				End If
			End If
			Dim x_CategoryID As DropDownList = TryCast(control.FindControl("x_CategoryID"), DropDownList)
			If (x_CategoryID IsNot Nothing) Then
				If ((x_CategoryID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_CategoryID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Category")
				End If
			End If
			Dim x_CompanyID As DropDownList = TryCast(control.FindControl("x_CompanyID"), DropDownList)
			If (x_CompanyID IsNot Nothing) Then
				If ((x_CompanyID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_CompanyID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Company")
				ElseIf (String.IsNullOrEmpty(x_CompanyID.SelectedValue)) Then
					messageList.Add("Please enter required field (Int32): Company")
				End If
			End If
			Dim x_PageID As DropDownList = TryCast(control.FindControl("x_PageID"), DropDownList)
			If (x_PageID IsNot Nothing) Then
				If ((x_PageID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_PageID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Page")
				End If
			End If
			Dim x_URL As TextBox = TryCast(control.FindControl("x_URL"), TextBox)
			If (x_URL IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_URL.Text)) Then
					messageList.Add("Invalid Value (String): HTML")
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
					messageList.Add("Invalid Value (Int32): Order")
				End If
			End If
			Dim x_UserID As DropDownList = TryCast(control.FindControl("x_UserID"), DropDownList)
			If (x_UserID IsNot Nothing) Then
				If ((x_UserID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_UserID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): User ID")
				End If
			End If
			Dim x_SiteCategoryGroupID As DropDownList = TryCast(control.FindControl("x_SiteCategoryGroupID"), DropDownList)
			If (x_SiteCategoryGroupID IsNot Nothing) Then
				If ((x_SiteCategoryGroupID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_SiteCategoryGroupID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Site Group")
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

	Private Sub ControlToRow(ByVal row As Linkrow, ByVal control As WebControl)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		row.SetPageType(Core.PageType.Add) ' set page type = list for update

		' Field Title
		Dim x_Title As TextBox = TryCast(control.FindControl("x_Title"), TextBox)
		If (x_Title.Text <> String.Empty) Then row.Title = x_Title.Text Else row.Title = Nothing

		' Field Description
		Dim x_Description As TextBox = TryCast(control.FindControl("x_Description"), TextBox)
		If (x_Description.Text <> String.Empty) Then row.Description = x_Description.Text Else row.Description = Nothing

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

		' Field PageID
		Dim x_PageID As DropDownList = TryCast(control.FindControl("x_PageID"), DropDownList)
		Dim v_PageID As String = String.Empty
		If (x_PageID.SelectedIndex >= 0) Then
			For Each li As ListItem In x_PageID.Items
				If (li.Selected) Then
					v_PageID += li.Value
					Exit For
				End If
			Next
		End If
		If (v_PageID <> String.Empty) Then row.PageID = Convert.ToInt32(v_PageID) Else row.PageID = CType(Nothing, Nullable(Of Int32))

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
		Dim data As Linkdal = New Linkdal()
		Dim newkey As Linkkey = New Linkkey()
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

	Private Sub RowToControl(ByVal row As Linkrow, ByVal control As WebControl, ByVal ct As Core.CtrlType)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		If (ct = Core.CtrlType.Edit) Then ' set up edit control

			' Field Title
			Dim x_Title As TextBox = TryCast(control.FindControl("x_Title"), TextBox)
			If (row.Title IsNot Nothing) Then x_Title.Text = row.Title.ToString() Else x_Title.Text = String.Empty

			' Field Description
			Dim x_Description As TextBox = TryCast(control.FindControl("x_Description"), TextBox)
			If (row.Description IsNot Nothing) Then x_Description.Text = row.Description.ToString() Else x_Description.Text = String.Empty

			' Field LinkTypeCD
			Dim x_LinkTypeCD As DropDownList = TryCast(control.FindControl("x_LinkTypeCD"), DropDownList)
			x_LinkTypeCD.DataValueField = "ewValueField"
			x_LinkTypeCD.DataTextField = "ewTextField"
			Dim dv_x_LinkTypeCD As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
			If (dv_x_LinkTypeCD Is Nothing) Then dv_x_LinkTypeCD = Linkdal.LookUpTable("LinkTypeCD")
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

			' Field CategoryID
			Dim x_CategoryID As DropDownList = TryCast(control.FindControl("x_CategoryID"), DropDownList)
			x_CategoryID.DataValueField = "ewValueField"
			x_CategoryID.DataTextField = "ewTextField"
			Dim dv_x_CategoryID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
			If (dv_x_CategoryID Is Nothing) Then dv_x_CategoryID = Linkdal.LookUpTable("CategoryID")
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

			' Field CompanyID
			If (control.FindControl("ax_CompanyID") IsNot Nothing) Then
				Dim cdd As CascadingDropDown = TryCast(control.FindControl("ax_CompanyID"), CascadingDropDown)
				Dim v_CompanyID As String
				If (row.CompanyID.HasValue) Then v_CompanyID = Convert.ToString(row.CompanyID) Else v_CompanyID = String.Empty
				cdd.SelectedValue = v_CompanyID
			End If

			' Field PageID
			If (control.FindControl("ax_PageID") IsNot Nothing) Then
				Dim cdd As CascadingDropDown = TryCast(control.FindControl("ax_PageID"), CascadingDropDown)
				Dim v_PageID As String
				If (row.PageID.HasValue) Then v_PageID = Convert.ToString(row.PageID) Else v_PageID = String.Empty
				cdd.SelectedValue = v_PageID
			End If

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

			' Field UserID
			If (control.FindControl("ax_UserID") IsNot Nothing) Then
				Dim cdd As CascadingDropDown = TryCast(control.FindControl("ax_UserID"), CascadingDropDown)
				Dim v_UserID As String
				If (row.UserID.HasValue) Then v_UserID = Convert.ToString(row.UserID) Else v_UserID = String.Empty
				cdd.SelectedValue = v_UserID
			End If

			' Field SiteCategoryGroupID
			Dim x_SiteCategoryGroupID As DropDownList = TryCast(control.FindControl("x_SiteCategoryGroupID"), DropDownList)
			x_SiteCategoryGroupID.DataValueField = "ewValueField"
			x_SiteCategoryGroupID.DataTextField = "ewTextField"
			Dim dv_x_SiteCategoryGroupID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
			If (dv_x_SiteCategoryGroupID Is Nothing) Then dv_x_SiteCategoryGroupID = Linkdal.LookUpTable("SiteCategoryGroupID")
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
	' *  Handler for DataSource Inserting
	' ************************************

	Protected Sub LinkDataSource_Inserting(ByVal sender As Object, ByVal e As ObjectDataSourceMethodEventArgs)
		Dim control As WebControl = LinkDetailsView

		' Set up row object
		Dim row As Linkrow = TryCast(e.InputParameters(0), Linkrow)
		ControlToRow(row, control)
		If (Linkbll.Inserting(row)) Then
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

	Protected Sub LinkDataSource_Inserted(ByVal sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.Exception IsNot Nothing) Then 
			e.AffectedRows = 0
			e.ExceptionHandled = True
			lblMessage.Text = e.Exception.InnerException.Message
			pnlMessage.Visible = True
		Else
			newrow = TryCast(e.ReturnValue, Linkrow) ' get new row objectinsert method
			Linkbll.Inserted(newrow)
		End If
	End Sub

	' *******************
	' *  Get User Filter
	' *******************

	Private Function GetUserFilter() As String 
		Return Linkinf.GetUserFilter()
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
	<p><span class="aspnetmaker">Add to TABLE: Link</span></p>
	<p>
		<asp:Hyperlink id="lnkList" Text="Back to List" NavigateUrl="link_list.aspx" CssClass="aspnetmaker" runat="server" />
		<asp:Label id="lblReturnUrl" Visible="False" CssClass="aspnetmaker" runat="server">link_list.aspx</asp:Label>
	</p>
	<asp:ValidationSummary id="xevs_Link" CssClass="aspnetmaker" runat="server"
	HeaderText=""
	ShowSummary="False"
	ShowMessageBox="True"
	ForeColor="#FF0000" />
	<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
		<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
	</asp:PlaceHolder>
	<!-- Data Source -->
<asp:ObjectDataSource ID="LinkDataSource"
	TypeName="PMGEN.Linkdal"
	DataObjectTypeName="PMGEN.Linkrow"
	InsertMethod="Insert"
	OnInserting="LinkDataSource_Inserting"
	OnInserted="LinkDataSource_Inserted"
	runat="server">
</asp:ObjectDataSource>
	<asp:DetailsView ID="LinkDetailsView"
		DataKeyNames="ID"
		DataSourceID="LinkDataSource"
		DefaultMode="Insert"
		GridLines="None"
		AutoGenerateRows="False" CssClass="ewDetailTable"
		OnInit="LinkDetailsView_Init"
		OnDataBound="LinkDetailsView_DataBound"
		OnItemCreated="LinkDetailsView_ItemCreated"
		OnItemInserting="LinkDetailsView_ItemInserting"
		OnItemInserted="LinkDetailsView_ItemInserted"
		OnUnload="LinkDetailsView_Unload"
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
					<asp:Label runat="server" id="xs_Title">Title</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_Title" Columns="30" MaxLength="255" CssClass="aspnetmaker" runat="server" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_Description">Description</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_Description" TextMode="MultiLine" Rows="5" Columns="60" CssClass="aspnetmaker" runat="server" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_LinkTypeCD">Type</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:DropDownList ID="x_LinkTypeCD" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_CategoryID">Category</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:DropDownList ID="x_CategoryID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_CompanyID">Company<span class='ewmsg'>&nbsp;*</span></asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:DropDownList ID="x_CompanyID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
<ew:CascadingDropDown ID="ax_CompanyID" runat="server" EncryptKey="<%$ EWCode: Share.RandomKey %>"
	TargetControlID="x_CompanyID" Category="x_CompanyID" 
	PromptText="Please Select" ServiceMethod="GetDropDownList" ServicePath="ewWebService.asmx" 
	SqlCommand="SELECT [CompanyID], [CompanyName] FROM [Company] ORDER BY [CompanyName] Asc"
	DisplayFieldNames="CompanyName" EnabledDropDown="true" />
<asp:RequiredFieldValidator ID="vx_CompanyID" CssClass="aspnetmaker" runat="server" ControlToValidate="x_CompanyID" ErrorMessage="Company is requried for a link" ForeColor="Red" Display="None" Text="*" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_PageID">Page</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:DropDownList ID="x_PageID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
<ew:CascadingDropDown ID="ax_PageID" runat="server" EncryptKey="<%$ EWCode: Share.RandomKey %>"
	TargetControlID="x_PageID" Category="x_PageID" 
	PromptText="Please Select" ServiceMethod="GetDropDownList" ServicePath="ewWebService.asmx" 
	SqlCommand="SELECT [PageID], [PageName] FROM [Page] WHERE ([CompanyID]=@FILTER_VALUE) ORDER BY [PageName] Asc"
	ParentCategory="x_CompanyID" ParentControlID="x_CompanyID"
	DisplayFieldNames="PageName" EnabledDropDown="true" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_URL">HTML</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_URL" TextMode="MultiLine" Rows="15" Columns="70" CssClass="aspnetmaker" runat="server" />
<script type="text/javascript">
<!--
var editor = new ew_DHTMLEditor(ew.Controls['x_URL']);
editor.create = function() {
	var sBasePath = 'fckeditor/';
	var oFCKeditor = new FCKeditor(ew.Controls['x_URL'], 70*_width_multiplier, 15*_height_multiplier);
	oFCKeditor.BasePath = sBasePath;
	oFCKeditor.ReplaceTextarea();
	this.active = true;
}
ew_DHTMLEditors[ew_DHTMLEditors.length] = editor;
-->
</script>
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_DateAdd">Date Add</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_DateAdd" CssClass="aspnetmaker" runat="server" />
<ew:USDateValidator ID="vc_DateAdd" ClientValidationFunction="ew_CheckUSDate" CssClass="aspnetmaker" runat="server" ControlToValidate="x_DateAdd" ErrorMessage="Incorrect date, format = mm/dd/yyyy - Date Add" Display="None" ForeColor="Red" DateSeparator="/" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_Ranks">Order</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_Ranks" Columns="30" CssClass="aspnetmaker" runat="server" />
<ew:IntegerValidator ID="vc_Ranks" ClientValidationFunction="ew_CheckInteger" CssClass="aspnetmaker" runat="server" ControlToValidate="x_Ranks" ErrorMessage="Incorrect integer - Order" Display="None" ForeColor="Red" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_Views">Views<span class='ewmsg'>&nbsp;*</span></asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:CheckBox ID="x_Views" CssClass="aspnetmakerlist" runat="server" />
<asp:HiddenField ID="hx_Views" Runat="server" Value='<%# Bind("Views") %>' />
<ew:CheckBoxListValidator ID="vx_Views" ClientValidationFunction="ew_CheckBoxListHasValue" CssClass="aspnetmaker" runat="server" ControlToValidate="x_Views" ErrorMessage="Please enter required field - Views" ForeColor="Red"  Display="Dynamic" Text="*"/>
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_UserID">User ID</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:DropDownList ID="x_UserID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
<ew:CascadingDropDown ID="ax_UserID" runat="server" EncryptKey="<%$ EWCode: Share.RandomKey %>"
	TargetControlID="x_UserID" Category="x_UserID" 
	PromptText="Please Select" ServiceMethod="GetDropDownList" ServicePath="ewWebService.asmx" 
	SqlCommand="SELECT [ContactID], [PrimaryContact] FROM [Contact] WHERE ([CompanyID]=@FILTER_VALUE) ORDER BY [PrimaryContact] Asc"
	ParentCategory="x_CompanyID" ParentControlID="x_CompanyID"
	DisplayFieldNames="PrimaryContact" EnabledDropDown="true" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_SiteCategoryGroupID">Site Group</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:DropDownList ID="x_SiteCategoryGroupID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_ASIN">ASIN</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_ASIN" Columns="30" MaxLength="50" CssClass="aspnetmaker" runat="server" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField ShowHeader="False">
				<ItemStyle CssClass="ewTableFooter" />
				<InsertItemTemplate>
					<table border="0">
						<tr>
							<td><asp:LinkButton ID="btnInsert" runat="server" CausesValidation="True" CommandName="Insert" Text="ADD" onClientClick="ew_UpdateTextArea()"></asp:LinkButton></td>
							<td><asp:LinkButton ID="btnCancel" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel" OnClick="btnCancel_Click"></asp:LinkButton></td>
						</tr>
					</table>
					<asp:HiddenField ID="k_ID" Runat="server" Value='<%# Bind("ID") %>' />
				</InsertItemTemplate>
			</asp:TemplateField>
		</Fields>
	</asp:DetailsView>
	<script type="text/javascript">
	<!--
		ew_CreateEditor();  // Create DHTML editor(s)
	//-->
	</script>
</asp:Content>
