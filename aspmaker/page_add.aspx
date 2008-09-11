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
	Dim key As Pagekey ' record key
	Dim oldrow As Pagerow ' old record data input by user
	Dim newrow As Pagerow ' new record data input by user
	Dim objProfile As TableProfile ' table profile

	' *************************
	' *  Handler for Page Init
	' *************************

	Protected Sub Page_Init(ByVal s As Object, ByVal e As System.EventArgs)
		If Not mhUser.IsAdmin() Then
		  Session("Login_Link") = Request.RawUrl
		  Response.Redirect("/mhweb/login/login.aspx")
		End If
		objProfile = CustomProfile.GetTable(Share.ProjectName, Pageinf.TableVar)
	End Sub

	' *************************
	' *  Handler for Page Load
	' *************************

	Protected Sub Page_Load(ByVal s As Object, ByVal e As System.EventArgs)
		Page.ClientScript.RegisterClientScriptInclude("ewv", "ewv.js")
		Response.Cache.SetCacheability(HttpCacheability.NoCache)
		If (Not Page.IsPostBack) Then
			If (Request.QueryString.Count > 0) Then
				key = New Pagekey()
				Dim messageList As ArrayList = Pageinf.LoadKey(key)
				If (messageList IsNot Nothing) Then key = Nothing ' error, ignore
			End If
		End If
		If (PageDetailsView.Visible) Then
			RegisterClientID("CtrlID", PageDetailsView)
		End If
			If (PageDetailsView.FindControl("x_PageOrder") IsNot Nothing) Then
				Page.Form.DefaultFocus = PageDetailsView.FindControl("x_PageOrder").ClientID
			End If
	End Sub

	' **********************************************
	' *  Load Copy Data & Default Data to Controls
	' **********************************************

	Private Sub LoadData()
		Dim source As WebControl = TryCast(PageDetailsView, WebControl)
		If (oldrow IsNot Nothing) Then ' old record (from recovery) exists, just restore
			RowToControl(oldrow, source, Core.CtrlType.Edit)
			oldrow = Nothing
		ElseIf (key IsNot Nothing) Then ' key loaded, try to copy record
			Dim data As Pagedal = New Pagedal()
			Dim row As Pagerow = data.LoadRow(key, Pageinf.GetUserFilter())
			If (row Is Nothing) Then
				objProfile.Message = "Record not found"
				Response.Redirect(lblReturnUrl.Text)
			End If
			RowToControl(row, source, Core.CtrlType.Edit)
			row = Nothing
		Else 
			Dim row As Pagerow = New Pagerow()
			row.PageOrder = Convert.ToInt32(0) ' set default value
			row.PageTypeID = Convert.ToInt32(0) ' set default value
			row.ImagesPerRow = Convert.ToInt32(0) ' set default value
			row.RowsPerPage = Convert.ToInt32(0) ' set default value
			row.ParentPageID = Convert.ToInt32(0) ' set default value
			row.CompanyID = Convert.ToInt32(0) ' set default value
			row.GroupID = Convert.ToInt32(4) ' set default value
			row.VersionNo = Convert.ToInt32(0) ' set default value
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

	Protected Sub PageDetailsView_Init(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			Pagedal.OpenConnection()
		Catch err As Exception
			lblMessage.Text = err.Message
			pnlMessage.Visible = True
		End Try
	End Sub

	' *************************************
	' *  Handler for DetailsView DataBound
	' *************************************

	Protected Sub PageDetailsView_DataBound(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim source As DetailsView = PageDetailsView
		If (PageDetailsView.CurrentMode = DetailsViewMode.Insert) Then
			LoadData()
		End If
	End Sub

	' *****************************************
	' *  Handler for DetailsView ItemInserting
	' *****************************************

	Protected Sub PageDetailsView_ItemInserting(ByVal sender As Object, ByVal e As DetailsViewInsertEventArgs)
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

	Protected Sub PageDetailsView_ItemInserted(ByVal sender As Object, ByVal e As DetailsViewInsertedEventArgs)
		If (e.AffectedRows = 0) Then

			' Keep in insert mode
			e.KeepInInsertMode = True

			' Get original values
			Dim source As WebControl = TryCast(sender, WebControl)
			If (source Is Nothing) Then Return
			oldrow = New Pagerow()
			ControlToRow(oldrow, source)
		Else
			objProfile.Message = "Add new record successful"
			Response.Redirect(lblReturnUrl.Text)
		End If
	End Sub

	' *********************************
	' *  Handler for DetailsView Unload
	' *********************************

	Protected Sub PageDetailsView_Unload(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			Pagedal.CloseAndDisposeConnection()
		Catch
		End Try
	End Sub

	' ********************************
	' * Register ClientID of Controls
	' ********************************

	Private Sub RegisterClientID(ByVal sBlockName As String, ByVal control As Control)
		Dim jsString As String = String.Empty
		If (control.FindControl("x_PageOrder") IsNot Nothing) Then
			jsString &= "ew.Controls['x_PageOrder'] = '" & control.FindControl("x_PageOrder").ClientID & "';"
		End If
		If (control.FindControl("x_PageName") IsNot Nothing) Then
			jsString &= "ew.Controls['x_PageName'] = '" & control.FindControl("x_PageName").ClientID & "';"
		End If
		If (control.FindControl("x_PageTitle") IsNot Nothing) Then
			jsString &= "ew.Controls['x_PageTitle'] = '" & control.FindControl("x_PageTitle").ClientID & "';"
		End If
		If (control.FindControl("x_PageDescription") IsNot Nothing) Then
			jsString &= "ew.Controls['x_PageDescription'] = '" & control.FindControl("x_PageDescription").ClientID & "';"
		End If
		If (control.FindControl("x_PageKeywords") IsNot Nothing) Then
			jsString &= "ew.Controls['x_PageKeywords'] = '" & control.FindControl("x_PageKeywords").ClientID & "';"
		End If
		If (control.FindControl("x_PageTypeID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_PageTypeID'] = '" & control.FindControl("x_PageTypeID").ClientID & "';"
		End If
		If (control.FindControl("x_ImagesPerRow") IsNot Nothing) Then
			jsString &= "ew.Controls['x_ImagesPerRow'] = '" & control.FindControl("x_ImagesPerRow").ClientID & "';"
		End If
		If (control.FindControl("x_RowsPerPage") IsNot Nothing) Then
			jsString &= "ew.Controls['x_RowsPerPage'] = '" & control.FindControl("x_RowsPerPage").ClientID & "';"
		End If
		If (control.FindControl("x_ParentPageID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_ParentPageID'] = '" & control.FindControl("x_ParentPageID").ClientID & "';"
		End If
		If (control.FindControl("x_Active") IsNot Nothing) Then
			jsString &= "ew.Controls['x_Active'] = '" & control.FindControl("x_Active").ClientID & "';"
		End If
		If (control.FindControl("x_CompanyID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_CompanyID'] = '" & control.FindControl("x_CompanyID").ClientID & "';"
		End If
		If (control.FindControl("x_PageFileName") IsNot Nothing) Then
			jsString &= "ew.Controls['x_PageFileName'] = '" & control.FindControl("x_PageFileName").ClientID & "';"
		End If
		If (control.FindControl("x_GroupID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_GroupID'] = '" & control.FindControl("x_GroupID").ClientID & "';"
		End If
		If (control.FindControl("x_ModifiedDT") IsNot Nothing) Then
			jsString &= "ew.Controls['x_ModifiedDT'] = '" & control.FindControl("x_ModifiedDT").ClientID & "';"
		End If
		If (control.FindControl("x_VersionNo") IsNot Nothing) Then
			jsString &= "ew.Controls['x_VersionNo'] = '" & control.FindControl("x_VersionNo").ClientID & "';"
		End If
		If (control.FindControl("x_SiteCategoryID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_SiteCategoryID'] = '" & control.FindControl("x_SiteCategoryID").ClientID & "';"
		End If
		If (control.FindControl("x_SiteCategoryGroupID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_SiteCategoryGroupID'] = '" & control.FindControl("x_SiteCategoryGroupID").ClientID & "';"
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
			Dim x_PageOrder As TextBox = TryCast(control.FindControl("x_PageOrder"), TextBox)
			If (x_PageOrder IsNot Nothing) Then
				If (Not DataFormat.CheckInt32(x_PageOrder.Text)) Then
					messageList.Add("Invalid Value (Int32): Order")
				ElseIf (String.IsNullOrEmpty(x_PageOrder.Text)) Then
					messageList.Add("Please enter required field (Int32): Order")
				End If
			End If
			Dim x_PageName As TextBox = TryCast(control.FindControl("x_PageName"), TextBox)
			If (x_PageName IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_PageName.Text)) Then
					messageList.Add("Invalid Value (String): Name")
				ElseIf (String.IsNullOrEmpty(x_PageName.Text)) Then
					messageList.Add("Please enter required field (String): Name")
				End If
			End If
			Dim x_PageTitle As TextBox = TryCast(control.FindControl("x_PageTitle"), TextBox)
			If (x_PageTitle IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_PageTitle.Text)) Then
					messageList.Add("Invalid Value (String): Title")
				End If
			End If
			Dim x_PageDescription As TextBox = TryCast(control.FindControl("x_PageDescription"), TextBox)
			If (x_PageDescription IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_PageDescription.Text)) Then
					messageList.Add("Invalid Value (String): Description")
				End If
			End If
			Dim x_PageKeywords As TextBox = TryCast(control.FindControl("x_PageKeywords"), TextBox)
			If (x_PageKeywords IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_PageKeywords.Text)) Then
					messageList.Add("Invalid Value (String): Keywords")
				End If
			End If
			Dim x_PageTypeID As DropDownList = TryCast(control.FindControl("x_PageTypeID"), DropDownList)
			If (x_PageTypeID IsNot Nothing) Then
				If ((x_PageTypeID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_PageTypeID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Type")
				ElseIf (String.IsNullOrEmpty(x_PageTypeID.SelectedValue)) Then
					messageList.Add("Please enter required field (Int32): Type")
				End If
			End If
			Dim x_ImagesPerRow As TextBox = TryCast(control.FindControl("x_ImagesPerRow"), TextBox)
			If (x_ImagesPerRow IsNot Nothing) Then
				If (Not DataFormat.CheckInt32(x_ImagesPerRow.Text)) Then
					messageList.Add("Invalid Value (Int32): Images Per Row")
				End If
			End If
			Dim x_RowsPerPage As TextBox = TryCast(control.FindControl("x_RowsPerPage"), TextBox)
			If (x_RowsPerPage IsNot Nothing) Then
				If (Not DataFormat.CheckInt32(x_RowsPerPage.Text)) Then
					messageList.Add("Invalid Value (Int32): Rows Per Page")
				End If
			End If
			Dim x_ParentPageID As DropDownList = TryCast(control.FindControl("x_ParentPageID"), DropDownList)
			If (x_ParentPageID IsNot Nothing) Then
				If ((x_ParentPageID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_ParentPageID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Parent")
				End If
			End If
			Dim x_CompanyID As DropDownList = TryCast(control.FindControl("x_CompanyID"), DropDownList)
			If (x_CompanyID IsNot Nothing) Then
				If ((x_CompanyID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_CompanyID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Company ID")
				End If
			End If
			Dim x_PageFileName As TextBox = TryCast(control.FindControl("x_PageFileName"), TextBox)
			If (x_PageFileName IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_PageFileName.Text)) Then
					messageList.Add("Invalid Value (String): File Name")
				End If
			End If
			Dim x_GroupID As DropDownList = TryCast(control.FindControl("x_GroupID"), DropDownList)
			If (x_GroupID IsNot Nothing) Then
				If ((x_GroupID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_GroupID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Security Group")
				End If
			End If
			Dim x_ModifiedDT As TextBox = TryCast(control.FindControl("x_ModifiedDT"), TextBox)
			If (x_ModifiedDT IsNot Nothing) Then
				If (Not DataFormat.CheckDateTime(x_ModifiedDT.Text)) Then
					messageList.Add("Invalid Value (DateTime): Modified DT")
				End If
			End If
			Dim x_VersionNo As TextBox = TryCast(control.FindControl("x_VersionNo"), TextBox)
			If (x_VersionNo IsNot Nothing) Then
				If (Not DataFormat.CheckInt32(x_VersionNo.Text)) Then
					messageList.Add("Invalid Value (Int32): Version No")
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
					messageList.Add("Invalid Value (Int32): Site Group ")
				End If
			End If
		End If
		Return IIf(messageList.Count = 0, Nothing, messageList)
	End Function

	' **********************************************
	' *  Convert Input Control Values to Row Object
	' **********************************************

	Private Sub ControlToRow(ByVal row As Pagerow, ByVal control As WebControl)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		row.SetPageType(Core.PageType.Add) ' set page type = list for update

		' Field PageOrder
		Dim x_PageOrder As TextBox = TryCast(control.FindControl("x_PageOrder"), TextBox)
		If (x_PageOrder.Text <> String.Empty) Then row.PageOrder = Convert.ToInt32(x_PageOrder.Text) Else row.PageOrder = CType(Nothing, Nullable(Of Int32))

		' Field PageName
		Dim x_PageName As TextBox = TryCast(control.FindControl("x_PageName"), TextBox)
		If (x_PageName.Text <> String.Empty) Then row.PageName = x_PageName.Text Else row.PageName = String.Empty

		' Field PageTitle
		Dim x_PageTitle As TextBox = TryCast(control.FindControl("x_PageTitle"), TextBox)
		If (x_PageTitle.Text <> String.Empty) Then row.PageTitle = x_PageTitle.Text Else row.PageTitle = Nothing

		' Field PageDescription
		Dim x_PageDescription As TextBox = TryCast(control.FindControl("x_PageDescription"), TextBox)
		If (x_PageDescription.Text <> String.Empty) Then row.PageDescription = x_PageDescription.Text Else row.PageDescription = Nothing

		' Field PageKeywords
		Dim x_PageKeywords As TextBox = TryCast(control.FindControl("x_PageKeywords"), TextBox)
		If (x_PageKeywords.Text <> String.Empty) Then row.PageKeywords = x_PageKeywords.Text Else row.PageKeywords = Nothing

		' Field PageTypeID
		Dim x_PageTypeID As DropDownList = TryCast(control.FindControl("x_PageTypeID"), DropDownList)
		Dim v_PageTypeID As String = String.Empty
		If (x_PageTypeID.SelectedIndex >= 0) Then
			For Each li As ListItem In x_PageTypeID.Items
				If (li.Selected) Then
					v_PageTypeID += li.Value
					Exit For
				End If
			Next
		End If
		If (v_PageTypeID <> String.Empty) Then row.PageTypeID = Convert.ToInt32(v_PageTypeID) Else row.PageTypeID = CType(Nothing, Nullable(Of Int32))

		' Field ImagesPerRow
		Dim x_ImagesPerRow As TextBox = TryCast(control.FindControl("x_ImagesPerRow"), TextBox)
		If (x_ImagesPerRow.Text <> String.Empty) Then row.ImagesPerRow = Convert.ToInt32(x_ImagesPerRow.Text) Else row.ImagesPerRow = CType(Nothing, Nullable(Of Int32))

		' Field RowsPerPage
		Dim x_RowsPerPage As TextBox = TryCast(control.FindControl("x_RowsPerPage"), TextBox)
		If (x_RowsPerPage.Text <> String.Empty) Then row.RowsPerPage = Convert.ToInt32(x_RowsPerPage.Text) Else row.RowsPerPage = CType(Nothing, Nullable(Of Int32))

		' Field ParentPageID
		Dim x_ParentPageID As DropDownList = TryCast(control.FindControl("x_ParentPageID"), DropDownList)
		Dim v_ParentPageID As String = String.Empty
		If (x_ParentPageID.SelectedIndex >= 0) Then
			For Each li As ListItem In x_ParentPageID.Items
				If (li.Selected) Then
					v_ParentPageID += li.Value
					Exit For
				End If
			Next
		End If
		If (v_ParentPageID <> String.Empty) Then row.ParentPageID = Convert.ToInt32(v_ParentPageID) Else row.ParentPageID = CType(Nothing, Nullable(Of Int32))

		' Field Active
		Dim x_Active As CheckBox = TryCast(control.FindControl("x_Active"), CheckBox)
		If (x_Active.Checked) Then
			row.Active = True
		Else
			row.Active = False
		End If

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

		' Field PageFileName
		Dim x_PageFileName As TextBox = TryCast(control.FindControl("x_PageFileName"), TextBox)
		If (x_PageFileName.Text <> String.Empty) Then row.PageFileName = x_PageFileName.Text Else row.PageFileName = Nothing

		' Field GroupID
		Dim x_GroupID As DropDownList = TryCast(control.FindControl("x_GroupID"), DropDownList)
		Dim v_GroupID As String = String.Empty
		If (x_GroupID.SelectedIndex >= 0) Then
			For Each li As ListItem In x_GroupID.Items
				If (li.Selected) Then
					v_GroupID += li.Value
					Exit For
				End If
			Next
		End If
		If (v_GroupID <> String.Empty) Then row.GroupID = Convert.ToInt32(v_GroupID) Else row.GroupID = CType(Nothing, Nullable(Of Int32))

		' Field ModifiedDT
		Dim x_ModifiedDT As TextBox = TryCast(control.FindControl("x_ModifiedDT"), TextBox)
		If (x_ModifiedDT.Text <> String.Empty) Then row.ModifiedDT = Convert.ToDateTime(DataFormat.UnFormatDateTime(x_ModifiedDT.Text, 6, "/"c)) Else row.ModifiedDT = CType(Nothing, Nullable(Of DateTime))

		' Field VersionNo
		Dim x_VersionNo As TextBox = TryCast(control.FindControl("x_VersionNo"), TextBox)
		If (x_VersionNo.Text <> String.Empty) Then row.VersionNo = Convert.ToInt32(x_VersionNo.Text) Else row.VersionNo = CType(Nothing, Nullable(Of Int32))

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
	End Sub

	' ****************************
	' *  Check for duplicate Key
	' ****************************

	Private Function CheckDuplicateKey(ByVal control As WebControl) As ArrayList 
		Dim sWhere As String = String.Empty
		Dim messageList As ArrayList = New ArrayList()
		Dim data As Pagedal = New Pagedal()
		Dim newkey As Pagekey = New Pagekey()
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

	Private Sub RowToControl(ByVal row As Pagerow, ByVal control As WebControl, ByVal ct As Core.CtrlType)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		If (ct = Core.CtrlType.Edit) Then ' set up edit control

			' Field PageOrder
			Dim x_PageOrder As TextBox = TryCast(control.FindControl("x_PageOrder"), TextBox)
			If (row.PageOrder.HasValue) Then x_PageOrder.Text = row.PageOrder.ToString() Else x_PageOrder.Text = String.Empty

			' Field PageName
			Dim x_PageName As TextBox = TryCast(control.FindControl("x_PageName"), TextBox)
			If (row.PageName IsNot Nothing) Then x_PageName.Text = row.PageName.ToString() Else x_PageName.Text = String.Empty

			' Field PageTitle
			Dim x_PageTitle As TextBox = TryCast(control.FindControl("x_PageTitle"), TextBox)
			If (row.PageTitle IsNot Nothing) Then x_PageTitle.Text = row.PageTitle.ToString() Else x_PageTitle.Text = String.Empty

			' Field PageDescription
			Dim x_PageDescription As TextBox = TryCast(control.FindControl("x_PageDescription"), TextBox)
			If (row.PageDescription IsNot Nothing) Then x_PageDescription.Text = row.PageDescription.ToString() Else x_PageDescription.Text = String.Empty

			' Field PageKeywords
			Dim x_PageKeywords As TextBox = TryCast(control.FindControl("x_PageKeywords"), TextBox)
			If (row.PageKeywords IsNot Nothing) Then x_PageKeywords.Text = row.PageKeywords.ToString() Else x_PageKeywords.Text = String.Empty

			' Field PageTypeID
			Dim x_PageTypeID As DropDownList = TryCast(control.FindControl("x_PageTypeID"), DropDownList)
			x_PageTypeID.DataValueField = "ewValueField"
			x_PageTypeID.DataTextField = "ewTextField"
			Dim dv_x_PageTypeID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
			If (dv_x_PageTypeID Is Nothing) Then dv_x_PageTypeID = Pagedal.LookUpTable("PageTypeID")
			x_PageTypeID.DataSource = dv_x_PageTypeID
			x_PageTypeID.DataBind()
			x_PageTypeID.Items.Insert(0, New ListItem("Please Select", ""))
			Dim v_PageTypeID As String
			If (row.PageTypeID.HasValue) Then v_PageTypeID = Convert.ToString(row.PageTypeID) Else v_PageTypeID = String.Empty
			x_PageTypeID.ClearSelection()
			For Each li As ListItem In x_PageTypeID.Items
				If (li.Value.ToString() = v_PageTypeID) Then
					li.Selected = True
					Exit For
				End If
			Next

			' Field ImagesPerRow
			Dim x_ImagesPerRow As TextBox = TryCast(control.FindControl("x_ImagesPerRow"), TextBox)
			If (row.ImagesPerRow.HasValue) Then x_ImagesPerRow.Text = row.ImagesPerRow.ToString() Else x_ImagesPerRow.Text = String.Empty

			' Field RowsPerPage
			Dim x_RowsPerPage As TextBox = TryCast(control.FindControl("x_RowsPerPage"), TextBox)
			If (row.RowsPerPage.HasValue) Then x_RowsPerPage.Text = row.RowsPerPage.ToString() Else x_RowsPerPage.Text = String.Empty

			' Field ParentPageID
			If (control.FindControl("ax_ParentPageID") IsNot Nothing) Then
				Dim cdd As CascadingDropDown = TryCast(control.FindControl("ax_ParentPageID"), CascadingDropDown)
				Dim v_ParentPageID As String
				If (row.ParentPageID.HasValue) Then v_ParentPageID = Convert.ToString(row.ParentPageID) Else v_ParentPageID = String.Empty
				cdd.SelectedValue = v_ParentPageID
			End If

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

			' Field CompanyID
			If (control.FindControl("ax_CompanyID") IsNot Nothing) Then
				Dim cdd As CascadingDropDown = TryCast(control.FindControl("ax_CompanyID"), CascadingDropDown)
				Dim v_CompanyID As String
				If (row.CompanyID.HasValue) Then v_CompanyID = Convert.ToString(row.CompanyID) Else v_CompanyID = String.Empty
				cdd.SelectedValue = v_CompanyID
			End If

			' Field PageFileName
			Dim x_PageFileName As TextBox = TryCast(control.FindControl("x_PageFileName"), TextBox)
			If (row.PageFileName IsNot Nothing) Then x_PageFileName.Text = row.PageFileName.ToString() Else x_PageFileName.Text = String.Empty

			' Field GroupID
			Dim x_GroupID As DropDownList = TryCast(control.FindControl("x_GroupID"), DropDownList)
			x_GroupID.DataValueField = "ewValueField"
			x_GroupID.DataTextField = "ewTextField"
			Dim dv_x_GroupID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
			If (dv_x_GroupID Is Nothing) Then dv_x_GroupID = Pagedal.LookUpTable("GroupID")
			x_GroupID.DataSource = dv_x_GroupID
			x_GroupID.DataBind()
			x_GroupID.Items.Insert(0, New ListItem("Please Select", ""))
			Dim v_GroupID As String
			If (row.GroupID.HasValue) Then v_GroupID = Convert.ToString(row.GroupID) Else v_GroupID = String.Empty
			x_GroupID.ClearSelection()
			For Each li As ListItem In x_GroupID.Items
				If (li.Value.ToString() = v_GroupID) Then
					li.Selected = True
					Exit For
				End If
			Next

			' Field ModifiedDT
			Dim x_ModifiedDT As TextBox = TryCast(control.FindControl("x_ModifiedDT"), TextBox)
			If (row.ModifiedDT.HasValue) Then x_ModifiedDT.Text = DataFormat.DateTimeFormat(6, "/", row.ModifiedDT) Else x_ModifiedDT.Text = String.Empty

			' Field VersionNo
			Dim x_VersionNo As TextBox = TryCast(control.FindControl("x_VersionNo"), TextBox)
			If (row.VersionNo.HasValue) Then x_VersionNo.Text = row.VersionNo.ToString() Else x_VersionNo.Text = String.Empty

			' Field SiteCategoryID
			Dim x_SiteCategoryID As DropDownList = TryCast(control.FindControl("x_SiteCategoryID"), DropDownList)
			x_SiteCategoryID.DataValueField = "ewValueField"
			x_SiteCategoryID.DataTextField = "ewTextField"
			Dim dv_x_SiteCategoryID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
			If (dv_x_SiteCategoryID Is Nothing) Then dv_x_SiteCategoryID = Pagedal.LookUpTable("SiteCategoryID")
			x_SiteCategoryID.DataSource = dv_x_SiteCategoryID
			x_SiteCategoryID.DataBind()
			x_SiteCategoryID.Items.Insert(0, New ListItem("Please Select", ""))
			Dim v_SiteCategoryID As String
			If (row.SiteCategoryID.HasValue) Then v_SiteCategoryID = Convert.ToString(row.SiteCategoryID) Else v_SiteCategoryID = String.Empty
			x_SiteCategoryID.ClearSelection()
			For Each li As ListItem In x_SiteCategoryID.Items
				If (li.Value.ToString() = v_SiteCategoryID) Then
					li.Selected = True
					Exit For
				End If
			Next

			' Field SiteCategoryGroupID
			Dim x_SiteCategoryGroupID As DropDownList = TryCast(control.FindControl("x_SiteCategoryGroupID"), DropDownList)
			x_SiteCategoryGroupID.DataValueField = "ewValueField"
			x_SiteCategoryGroupID.DataTextField = "ewTextField"
			Dim dv_x_SiteCategoryGroupID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
			If (dv_x_SiteCategoryGroupID Is Nothing) Then dv_x_SiteCategoryGroupID = Pagedal.LookUpTable("SiteCategoryGroupID")
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

	Protected Sub PageDataSource_Inserting(ByVal sender As Object, ByVal e As ObjectDataSourceMethodEventArgs)
		Dim control As WebControl = PageDetailsView

		' Set up row object
		Dim row As Pagerow = TryCast(e.InputParameters(0), Pagerow)
		ControlToRow(row, control)
		If (Pagebll.Inserting(row)) Then
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

	Protected Sub PageDataSource_Inserted(ByVal sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.Exception IsNot Nothing) Then 
			e.AffectedRows = 0
			e.ExceptionHandled = True
			lblMessage.Text = e.Exception.InnerException.Message
			pnlMessage.Visible = True
		Else
			newrow = TryCast(e.ReturnValue, Pagerow) ' get new row objectinsert method
			Pagebll.Inserted(newrow)
		End If
	End Sub

	' *******************
	' *  Get User Filter
	' *******************

	Private Function GetUserFilter() As String 
		Return Pageinf.GetUserFilter()
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
	<p><span class="aspnetmaker">Add to TABLE: Page</span></p>
	<p>
		<asp:Hyperlink id="lnkList" Text="Back to List" NavigateUrl="page_list.aspx" CssClass="aspnetmaker" runat="server" />
		<asp:Label id="lblReturnUrl" Visible="False" CssClass="aspnetmaker" runat="server">page_list.aspx</asp:Label>
	</p>
	<asp:ValidationSummary id="xevs_Page" CssClass="aspnetmaker" runat="server"
	HeaderText=""
	ShowSummary="False"
	ShowMessageBox="True"
	ForeColor="#FF0000" />
	<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
		<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
	</asp:PlaceHolder>
	<!-- Data Source -->
<asp:ObjectDataSource ID="PageDataSource"
	TypeName="PMGEN.Pagedal"
	DataObjectTypeName="PMGEN.Pagerow"
	InsertMethod="Insert"
	OnInserting="PageDataSource_Inserting"
	OnInserted="PageDataSource_Inserted"
	runat="server">
</asp:ObjectDataSource>
	<asp:DetailsView ID="PageDetailsView"
		DataKeyNames="PageID"
		DataSourceID="PageDataSource"
		DefaultMode="Insert"
		GridLines="None"
		AutoGenerateRows="False" CssClass="ewDetailTable"
		OnInit="PageDetailsView_Init"
		OnDataBound="PageDetailsView_DataBound"
		OnItemInserting="PageDetailsView_ItemInserting"
		OnItemInserted="PageDetailsView_ItemInserted"
		OnUnload="PageDetailsView_Unload"
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
					<asp:Label runat="server" id="xs_PageOrder">Order<span class='ewmsg'>&nbsp;*</span></asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_PageOrder" Columns="30" CssClass="aspnetmaker" runat="server" />
<asp:RequiredFieldValidator ID="vx_PageOrder" CssClass="aspnetmaker" runat="server" ControlToValidate="x_PageOrder" ErrorMessage="Please enter required field - Order" ForeColor="Red" Display="None" Text="*" />
<ew:IntegerValidator ID="vc_PageOrder" ClientValidationFunction="ew_CheckInteger" CssClass="aspnetmaker" runat="server" ControlToValidate="x_PageOrder" ErrorMessage="Incorrect integer - Order" Display="None" ForeColor="Red" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_PageName">Name<span class='ewmsg'>&nbsp;*</span></asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_PageName" Columns="80" MaxLength="50" CssClass="aspnetmaker" runat="server" />
<asp:RequiredFieldValidator ID="vx_PageName" CssClass="aspnetmaker" runat="server" ControlToValidate="x_PageName" ErrorMessage="Please enter required field - Name" ForeColor="Red" Display="None" Text="*" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_PageTitle">Title</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_PageTitle" Columns="80" MaxLength="255" CssClass="aspnetmaker" runat="server" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_PageDescription">Description</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_PageDescription" TextMode="MultiLine" Rows="5" Columns="80" CssClass="aspnetmaker" runat="server" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_PageKeywords">Keywords</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_PageKeywords" TextMode="MultiLine" Rows="3" Columns="80" CssClass="aspnetmaker" runat="server" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_PageTypeID">Type<span class='ewmsg'>&nbsp;*</span></asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:DropDownList ID="x_PageTypeID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
<asp:RequiredFieldValidator ID="vx_PageTypeID" CssClass="aspnetmaker" runat="server" ControlToValidate="x_PageTypeID" ErrorMessage="Page Type is a required field" ForeColor="Red" Display="None" Text="*" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_ImagesPerRow">Images Per Row</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_ImagesPerRow" Columns="30" CssClass="aspnetmaker" runat="server" />
<ew:IntegerValidator ID="vc_ImagesPerRow" ClientValidationFunction="ew_CheckInteger" CssClass="aspnetmaker" runat="server" ControlToValidate="x_ImagesPerRow" ErrorMessage="Incorrect integer - Images Per Row" Display="None" ForeColor="Red" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_RowsPerPage">Rows Per Page</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_RowsPerPage" Columns="30" CssClass="aspnetmaker" runat="server" />
<ew:IntegerValidator ID="vc_RowsPerPage" ClientValidationFunction="ew_CheckInteger" CssClass="aspnetmaker" runat="server" ControlToValidate="x_RowsPerPage" ErrorMessage="Incorrect integer - Rows Per Page" Display="None" ForeColor="Red" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_ParentPageID">Parent</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:DropDownList ID="x_ParentPageID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
<ew:CascadingDropDown ID="ax_ParentPageID" runat="server" EncryptKey="<%$ EWCode: Share.RandomKey %>"
	TargetControlID="x_ParentPageID" Category="x_ParentPageID" 
	PromptText="Please Select" ServiceMethod="GetDropDownList" ServicePath="ewWebService.asmx" 
	SqlCommand="SELECT [PageID], [PageName] FROM [Page] WHERE ([CompanyID]=@FILTER_VALUE) ORDER BY [PageName] Asc"
	ParentCategory="x_CompanyID" ParentControlID="x_CompanyID"
	DisplayFieldNames="PageName" EnabledDropDown="true" />
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
					<asp:Label runat="server" id="xs_CompanyID">Company ID</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:DropDownList ID="x_CompanyID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
<ew:CascadingDropDown ID="ax_CompanyID" runat="server" EncryptKey="<%$ EWCode: Share.RandomKey %>"
	TargetControlID="x_CompanyID" Category="x_CompanyID" 
	PromptText="Please Select" ServiceMethod="GetDropDownList" ServicePath="ewWebService.asmx" 
	SqlCommand="SELECT [CompanyID], [CompanyName] FROM [Company] ORDER BY [CompanyName]"
	DisplayFieldNames="CompanyName" EnabledDropDown="true" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_PageFileName">File Name</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_PageFileName" Columns="50" MaxLength="255" CssClass="aspnetmaker" runat="server" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_GroupID">Security Group</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:DropDownList ID="x_GroupID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_ModifiedDT">Modified DT</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_ModifiedDT" CssClass="aspnetmaker" runat="server" />
<ew:USDateValidator ID="vc_ModifiedDT" ClientValidationFunction="ew_CheckUSDate" CssClass="aspnetmaker" runat="server" ControlToValidate="x_ModifiedDT" ErrorMessage="Incorrect date, format = mm/dd/yyyy - Modified DT" Display="None" ForeColor="Red" DateSeparator="/" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_VersionNo">Version No</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_VersionNo" Columns="30" CssClass="aspnetmaker" runat="server" />
<ew:IntegerValidator ID="vc_VersionNo" ClientValidationFunction="ew_CheckInteger" CssClass="aspnetmaker" runat="server" ControlToValidate="x_VersionNo" ErrorMessage="Incorrect integer - Version No" Display="None" ForeColor="Red" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_SiteCategoryID">Site Category</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:DropDownList ID="x_SiteCategoryID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_SiteCategoryGroupID">Site Group </asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:DropDownList ID="x_SiteCategoryGroupID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
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
					<asp:HiddenField ID="k_PageID" Runat="server" Value='<%# Bind("PageID") %>' />
				</InsertItemTemplate>
			</asp:TemplateField>
		</Fields>
	</asp:DetailsView>
</asp:Content>
