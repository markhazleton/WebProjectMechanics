<%@ Page EnableViewState="true" Language="VB" ValidateRequest="False" EnableEventValidation="False" Debug="True" MasterPageFile="masterpage.master" %>
<%@ MasterType VirtualPath="masterpage.master" %>
<%@ Import Namespace="PMGEN" %>
<%@ Import Namespace="EW.Data" %>
<%@ Import Namespace="EW.Data.Utilities" %>
<%@ Import Namespace="EW.Web" %>
<%@ Register NameSpace="PMGEN" TagPrefix="PMGEN" %>
<script runat="server">
	Dim key As SiteCategorykey ' record key
	Dim oldrow As SiteCategoryrow ' old record data input by user
	Dim newrow As SiteCategoryrow ' new record data input by user
	Dim objProfile As TableProfile ' table profile

	' *************************
	' *  Handler for Page Init
	' *************************

	Protected Sub Page_Init(ByVal s As Object, ByVal e As System.EventArgs)
		If Not mhUser.IsAdmin() Then
		  Session("Login_Link") = Request.RawUrl
		  Response.Redirect("/mhweb/login/login.aspx")
		End If
		objProfile = CustomProfile.GetTable(Share.ProjectName, SiteCategoryinf.TableVar)
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
				key = New SiteCategorykey()
				Dim messageList As ArrayList = SiteCategoryinf.LoadKey(key) 
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
			key = TryCast(ViewState("key"), SiteCategorykey) ' restore from ViewState for postback
		End If
		If (SiteCategoryDetailsView.Visible) Then
			RegisterClientID("CtrlID", SiteCategoryDetailsView)
		End If
		If (SiteCategoryDetailsView.FindControl("x_SiteCategoryTypeID") IsNot Nothing) Then
			Page.Form.DefaultFocus = SiteCategoryDetailsView.FindControl("x_SiteCategoryTypeID").ClientID
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

	Protected Sub SiteCategoryDetailsView_Init(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			SiteCategorydal.OpenConnection()
		Catch err As Exception
			lblMessage.Text = err.Message
			pnlMessage.Visible = True
		End Try
	End Sub

	' *************************************
	' *  Handler for DetailsView DataBound
	' *************************************

	Protected Sub SiteCategoryDetailsView_DataBound(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim source As DetailsView = SiteCategoryDetailsView
		Dim row As SiteCategoryrow = TryCast(SiteCategoryDetailsView.DataItem, SiteCategoryrow) ' get data object
		If (SiteCategoryDetailsView.CurrentMode = DetailsViewMode.Edit) Then
			If (row IsNot Nothing) Then
				key.SiteCategoryID = Convert.ToInt32(row.SiteCategoryID)
			End If
			RowToControl(row, source, Core.CtrlType.Edit)
		End If
	End Sub

	' ****************************************
	' *  Handler for DetailsView ItemUpdating
	' ****************************************

	Protected Sub SiteCategoryDetailsView_ItemUpdating(ByVal sender As Object, ByVal e As DetailsViewUpdateEventArgs)
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

	Protected Sub SiteCategoryDetailsView_ItemUpdated(ByVal sender As Object, ByVal e As DetailsViewUpdatedEventArgs)

		' Check If update successful 
		If (e.AffectedRows = 0) Then

			' Keep in edit mode 
			e.KeepInEditMode = True

			' Get original values 
			Dim source As WebControl = TryCast(sender, WebControl)
			If (source Is Nothing) Then Return
			Dim oldrow As SiteCategoryrow = New SiteCategoryrow()
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

	Protected Sub SiteCategoryDetailsView_Unload(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			SiteCategorydal.CloseAndDisposeConnection()
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
		If (control.FindControl("x_CategoryName") IsNot Nothing) Then
			jsString &= "ew.Controls['x_CategoryName'] = '" & control.FindControl("x_CategoryName").ClientID & "';"
		End If
		If (control.FindControl("x_CategoryTitle") IsNot Nothing) Then
			jsString &= "ew.Controls['x_CategoryTitle'] = '" & control.FindControl("x_CategoryTitle").ClientID & "';"
		End If
		If (control.FindControl("x_CategoryDescription") IsNot Nothing) Then
			jsString &= "ew.Controls['x_CategoryDescription'] = '" & control.FindControl("x_CategoryDescription").ClientID & "';"
		End If
		If (control.FindControl("x_CategoryKeywords") IsNot Nothing) Then
			jsString &= "ew.Controls['x_CategoryKeywords'] = '" & control.FindControl("x_CategoryKeywords").ClientID & "';"
		End If
		If (control.FindControl("x_SiteCategoryGroupID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_SiteCategoryGroupID'] = '" & control.FindControl("x_SiteCategoryGroupID").ClientID & "';"
		End If
		If (control.FindControl("x_GroupOrder") IsNot Nothing) Then
			jsString &= "ew.Controls['x_GroupOrder'] = '" & control.FindControl("x_GroupOrder").ClientID & "';"
		End If
		If (control.FindControl("x_ParentCategoryID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_ParentCategoryID'] = '" & control.FindControl("x_ParentCategoryID").ClientID & "';"
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
					messageList.Add("Invalid Value (Int32):  Site Type")
				ElseIf (String.IsNullOrEmpty(x_SiteCategoryTypeID.SelectedValue)) Then
					messageList.Add("Please enter required field (Int32):  Site Type")
				End If
			End If
			Dim x_CategoryName As TextBox = TryCast(control.FindControl("x_CategoryName"), TextBox)
			If (x_CategoryName IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_CategoryName.Text)) Then
					messageList.Add("Invalid Value (String): Category Name")
				ElseIf (String.IsNullOrEmpty(x_CategoryName.Text)) Then
					messageList.Add("Please enter required field (String): Category Name")
				End If
			End If
			Dim x_CategoryTitle As TextBox = TryCast(control.FindControl("x_CategoryTitle"), TextBox)
			If (x_CategoryTitle IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_CategoryTitle.Text)) Then
					messageList.Add("Invalid Value (String): Category Title")
				End If
			End If
			Dim x_CategoryDescription As TextBox = TryCast(control.FindControl("x_CategoryDescription"), TextBox)
			If (x_CategoryDescription IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_CategoryDescription.Text)) Then
					messageList.Add("Invalid Value (String): Category Description")
				End If
			End If
			Dim x_CategoryKeywords As TextBox = TryCast(control.FindControl("x_CategoryKeywords"), TextBox)
			If (x_CategoryKeywords IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_CategoryKeywords.Text)) Then
					messageList.Add("Invalid Value (String): Category Keywords")
				End If
			End If
			Dim x_SiteCategoryGroupID As DropDownList = TryCast(control.FindControl("x_SiteCategoryGroupID"), DropDownList)
			If (x_SiteCategoryGroupID IsNot Nothing) Then
				If ((x_SiteCategoryGroupID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_SiteCategoryGroupID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Category Group")
				ElseIf (String.IsNullOrEmpty(x_SiteCategoryGroupID.SelectedValue)) Then
					messageList.Add("Please enter required field (Int32): Category Group")
				End If
			End If
			Dim x_GroupOrder As TextBox = TryCast(control.FindControl("x_GroupOrder"), TextBox)
			If (x_GroupOrder IsNot Nothing) Then
				If (Not DataFormat.CheckDouble(x_GroupOrder.Text)) Then
					messageList.Add("Invalid Value (Double): Order")
				End If
			End If
			Dim x_ParentCategoryID As DropDownList = TryCast(control.FindControl("x_ParentCategoryID"), DropDownList)
			If (x_ParentCategoryID IsNot Nothing) Then
				If ((x_ParentCategoryID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_ParentCategoryID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Parent Category ID")
				End If
			End If
		End If
		Return IIf(messageList.Count = 0, Nothing, messageList)
	End Function

	' **********************************************
	' *  Convert Input Control Values to Row Object
	' **********************************************

	Private Sub ControlToRow(ByVal row As SiteCategoryrow, ByVal control As WebControl)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		row.SetPageType(Core.PageType.Edit) ' set page type = list for update

		' Field SiteCategoryID
		row.Attribute("SiteCategoryID").AllowUpdate = False ' set update to False(read only field)		

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

		' Field CategoryName
		Dim x_CategoryName As TextBox = TryCast(control.FindControl("x_CategoryName"), TextBox)
		If (x_CategoryName.Text <> String.Empty) Then row.CategoryName = x_CategoryName.Text Else row.CategoryName = String.Empty

		' Field CategoryTitle
		Dim x_CategoryTitle As TextBox = TryCast(control.FindControl("x_CategoryTitle"), TextBox)
		If (x_CategoryTitle.Text <> String.Empty) Then row.CategoryTitle = x_CategoryTitle.Text Else row.CategoryTitle = Nothing

		' Field CategoryDescription
		Dim x_CategoryDescription As TextBox = TryCast(control.FindControl("x_CategoryDescription"), TextBox)
		If (x_CategoryDescription.Text <> String.Empty) Then row.CategoryDescription = x_CategoryDescription.Text Else row.CategoryDescription = Nothing

		' Field CategoryKeywords
		Dim x_CategoryKeywords As TextBox = TryCast(control.FindControl("x_CategoryKeywords"), TextBox)
		If (x_CategoryKeywords.Text <> String.Empty) Then row.CategoryKeywords = x_CategoryKeywords.Text Else row.CategoryKeywords = Nothing

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

		' Field GroupOrder
		Dim x_GroupOrder As TextBox = TryCast(control.FindControl("x_GroupOrder"), TextBox)
		If (x_GroupOrder.Text <> String.Empty) Then row.GroupOrder = Convert.ToDouble(x_GroupOrder.Text) Else row.GroupOrder = CType(Nothing, Nullable(Of Double))

		' Field ParentCategoryID
		Dim x_ParentCategoryID As DropDownList = TryCast(control.FindControl("x_ParentCategoryID"), DropDownList)
		Dim v_ParentCategoryID As String = String.Empty
		If (x_ParentCategoryID.SelectedIndex >= 0) Then
			For Each li As ListItem In x_ParentCategoryID.Items
				If (li.Selected) Then
					v_ParentCategoryID += li.Value
					Exit For
				End If
			Next
		End If
		If (v_ParentCategoryID <> String.Empty) Then row.ParentCategoryID = Convert.ToInt32(v_ParentCategoryID) Else row.ParentCategoryID = CType(Nothing, Nullable(Of Int32))
	End Sub

	' ****************************
	' *  Check for duplicate Key
	' ****************************

	Private Function CheckDuplicateKey(ByVal control As WebControl) As ArrayList 
		Dim sWhere As String = String.Empty
		Dim messageList As ArrayList = New ArrayList()
		Dim data As SiteCategorydal = New SiteCategorydal()
		Dim newkey As SiteCategorykey = New SiteCategorykey()
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

	Private Sub RowToControl(ByVal row As SiteCategoryrow, ByVal control As WebControl, ByVal ct As Core.CtrlType)
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

			' Field CategoryName
			Dim x_CategoryName As TextBox = TryCast(control.FindControl("x_CategoryName"), TextBox)
			If (row.CategoryName IsNot Nothing) Then x_CategoryName.Text = row.CategoryName.ToString() Else x_CategoryName.Text = String.Empty

			' Field CategoryTitle
			Dim x_CategoryTitle As TextBox = TryCast(control.FindControl("x_CategoryTitle"), TextBox)
			If (row.CategoryTitle IsNot Nothing) Then x_CategoryTitle.Text = row.CategoryTitle.ToString() Else x_CategoryTitle.Text = String.Empty

			' Field CategoryDescription
			Dim x_CategoryDescription As TextBox = TryCast(control.FindControl("x_CategoryDescription"), TextBox)
			If (row.CategoryDescription IsNot Nothing) Then x_CategoryDescription.Text = row.CategoryDescription.ToString() Else x_CategoryDescription.Text = String.Empty

			' Field CategoryKeywords
			Dim x_CategoryKeywords As TextBox = TryCast(control.FindControl("x_CategoryKeywords"), TextBox)
			If (row.CategoryKeywords IsNot Nothing) Then x_CategoryKeywords.Text = row.CategoryKeywords.ToString() Else x_CategoryKeywords.Text = String.Empty

			' Field SiteCategoryGroupID
			Dim x_SiteCategoryGroupID As DropDownList = TryCast(control.FindControl("x_SiteCategoryGroupID"), DropDownList)
			x_SiteCategoryGroupID.DataValueField = "ewValueField"
			x_SiteCategoryGroupID.DataTextField = "ewTextField"
			Dim dv_x_SiteCategoryGroupID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
			If (dv_x_SiteCategoryGroupID Is Nothing) Then dv_x_SiteCategoryGroupID = SiteCategorydal.LookUpTable("SiteCategoryGroupID")
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

			' Field GroupOrder
			Dim x_GroupOrder As TextBox = TryCast(control.FindControl("x_GroupOrder"), TextBox)
			If (row.GroupOrder.HasValue) Then x_GroupOrder.Text = row.GroupOrder.ToString() Else x_GroupOrder.Text = String.Empty

			' Field ParentCategoryID
			If (control.FindControl("ax_ParentCategoryID") IsNot Nothing) Then
				Dim cdd As CascadingDropDown = TryCast(control.FindControl("ax_ParentCategoryID"), CascadingDropDown)
				Dim v_ParentCategoryID As String
				If (row.ParentCategoryID.HasValue) Then v_ParentCategoryID = Convert.ToString(row.ParentCategoryID) Else v_ParentCategoryID = String.Empty
				cdd.SelectedValue = v_ParentCategoryID
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
	' *  Handler for DataSource Selecting
	' ************************************

	Protected Sub SiteCategoryDataSource_Selecting(Byval sender As Object, ByVal e As ObjectDataSourceSelectingEventArgs)
		e.InputParameters.Clear()
		e.InputParameters.Add("filter", SiteCategoryinf.GetUserFilter())
	End Sub

	' *************************************
	' *  DataSource Selected Event Handler
	' *************************************

	Protected Sub SiteCategoryDataSource_Selected(Byval sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.ReturnValue Is Nothing) Then
			If (e.Exception IsNot Nothing) Then
				lblMessage.Text = e.Exception.Message
				pnlMessage.Visible = True
			Else
				lblMessage.Text = "No records found"
				pnlMessage.Visible = True
			End If
		Else
			Dim rows As SiteCategoryrows = TryCast(e.ReturnValue, SiteCategoryrows)
			Dim i As Integer = 0
			Do While (rows IsNot Nothing AndAlso i < rows.Count)
				If (key.Equals(rows(i).GetKey())) Then
					SiteCategoryDetailsView.PageIndex = i
					Exit Do
				End If
				i += 1
			Loop
		End If
	End Sub

	' ***********************************
	' *  Handler for DataSource Updating
	' ***********************************

	Protected Sub SiteCategoryDataSource_Updating(ByVal sender As Object, ByVal e As ObjectDataSourceMethodEventArgs)
		Dim source As DetailsView = SiteCategoryDetailsView

		' Set up row object
		Dim row As SiteCategoryrow = TryCast(e.InputParameters(0), SiteCategoryrow)
		Dim data As SiteCategorydal = New SiteCategorydal()
		key.SiteCategoryID = Convert.ToInt32(row.SiteCategoryID)
		oldrow = data.LoadRow(key, SiteCategoryinf.GetUserFilter())
		ControlToRow(row, source) ' load values of controls to row
		newrow = row
		If (SiteCategorybll.Updating(oldrow, newrow)) Then
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

	Protected Sub SiteCategoryDataSource_Updated(ByVal sender As Object, ByVal e As ObjectDataSourceStatusEventArgs)
		If (e.Exception IsNot Nothing) Then
			e.AffectedRows = 0
			e.ExceptionHandled = True
			lblMessage.Text = e.Exception.InnerException.Message
		Else
			lblMessage.Text = "Update successful"
			SiteCategorybll.Updated(oldrow, newrow)
		End If
		If (lblMessage.Text <> "") Then pnlMessage.Visible = True
	End Sub

	' ************************************************
	' *  Change the page index for DetailsView paging
	' ************************************************

	Protected Sub ChangePageIndex(ByVal sender As Object, ByVal e As DetailsViewPageEventArgs)
		Dim data As SiteCategorydal = New SiteCategorydal()
		Dim rows As SiteCategoryrows = data.LoadList(SiteCategoryinf.GetUserFilter())
		If (rows IsNot Nothing AndAlso rows.Count >= e.NewPageIndex) Then
			key = rows(e.NewPageIndex).GetKey()
		End If
	End Sub

	' *******************
	' *  Get User Filter
	' *******************

	Private Function GetUserFilter() As String 
		Return SiteCategoryinf.GetUserFilter()
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
	<p><span class="aspnetmaker">Edit TABLE: Site Category</span></p>
	<p>
		<asp:Hyperlink id="lnkList" Text="Back to List" NavigateUrl="sitecategory_list.aspx" CssClass="aspnetmaker" runat="server" />
		<asp:Label id="lblReturnUrl" Visible="False" CssClass="aspnetmaker" runat="server">sitecategory_list.aspx</asp:Label>
	</p>
	<asp:ValidationSummary id="xevs_SiteCategory" CssClass="aspnetmaker" runat="server"
		HeaderText=""
		ShowSummary="False"
		ShowMessageBox="True"
		ForeColor="#FF0000" />
	<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
		<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
	</asp:PlaceHolder>
<!-- Data Source -->
<asp:ObjectDataSource ID="SiteCategoryDataSource"
	TypeName="PMGEN.SiteCategorydal"
	DataObjectTypeName="PMGEN.SiteCategoryrow"
	SelectMethod="LoadList"
	UpdateMethod="Update"
	OnSelecting="SiteCategoryDataSource_Selecting"
	OnSelected="SiteCategoryDataSource_Selected"
	OnUpdating="SiteCategoryDataSource_Updating"
	OnUpdated="SiteCategoryDataSource_Updated"
	runat="server">
</asp:ObjectDataSource>
<!-- DetailsView -->
	<asp:DetailsView ID="SiteCategoryDetailsView"
		DataKeyNames="SiteCategoryID"
		DataSourceID="SiteCategoryDataSource"
		DefaultMode="Edit"
		GridLines="None"
		AutoGenerateRows="False" CssClass="ewDetailTable"
		OnInit="SiteCategoryDetailsView_Init"
		OnDataBound="SiteCategoryDetailsView_DataBound"
		OnItemUpdating="SiteCategoryDetailsView_ItemUpdating"
		OnItemUpdated="SiteCategoryDetailsView_ItemUpdated"
		OnUnload="SiteCategoryDetailsView_Unload"
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
					<asp:Label runat="server" id="xs_SiteCategoryTypeID"> Site Type<span class='ewmsg'>&nbsp;*</span></asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:DropDownList ID="x_SiteCategoryTypeID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
<ew:CascadingDropDown ID="ax_SiteCategoryTypeID" runat="server" EncryptKey="<%$ EWCode: Share.RandomKey %>"
	TargetControlID="x_SiteCategoryTypeID" Category="x_SiteCategoryTypeID" 
	PromptText="Please Select" ServiceMethod="GetDropDownList" ServicePath="ewWebService.asmx" 
	SqlCommand="SELECT [SiteCategoryTypeID], [SiteCategoryTypeNM] FROM [SiteCategoryType] ORDER BY [SiteCategoryTypeNM] Asc"
	DisplayFieldNames="SiteCategoryTypeNM" EnabledDropDown="true" />
<asp:RequiredFieldValidator ID="vx_SiteCategoryTypeID" CssClass="aspnetmaker" runat="server" ControlToValidate="x_SiteCategoryTypeID" ErrorMessage="Please enter required field -  Site Type" ForeColor="Red" Display="None" Text="*" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_CategoryName">Category Name<span class='ewmsg'>&nbsp;*</span></asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_CategoryName" Columns="60" MaxLength="255" CssClass="aspnetmaker" runat="server" />
<asp:RequiredFieldValidator ID="vx_CategoryName" CssClass="aspnetmaker" runat="server" ControlToValidate="x_CategoryName" ErrorMessage="Please enter required field - Category Name" ForeColor="Red" Display="None" Text="*" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_CategoryTitle">Category Title</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_CategoryTitle" Columns="60" MaxLength="255" CssClass="aspnetmaker" runat="server" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_CategoryDescription">Category Description</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_CategoryDescription" TextMode="MultiLine" Rows="5" Columns="70" CssClass="aspnetmaker" runat="server" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_CategoryKeywords">Category Keywords</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_CategoryKeywords" Columns="60" MaxLength="255" CssClass="aspnetmaker" runat="server" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_SiteCategoryGroupID">Category Group<span class='ewmsg'>&nbsp;*</span></asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:DropDownList ID="x_SiteCategoryGroupID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
<asp:RequiredFieldValidator ID="vx_SiteCategoryGroupID" CssClass="aspnetmaker" runat="server" ControlToValidate="x_SiteCategoryGroupID" ErrorMessage="Please enter required field - Category Group" ForeColor="Red" Display="None" Text="*" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_GroupOrder">Order</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_GroupOrder" Columns="30" CssClass="aspnetmaker" runat="server" />
<ew:NumberValidator ID="vc_GroupOrder" ClientValidationFunction="ew_CheckNumber" CssClass="aspnetmaker" runat="server" ControlToValidate="x_GroupOrder" ErrorMessage="Incorrect floating point number - Order" Display="None" ForeColor="Red" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_ParentCategoryID">Parent Category ID</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:DropDownList ID="x_ParentCategoryID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
<ew:CascadingDropDown ID="ax_ParentCategoryID" runat="server" EncryptKey="<%$ EWCode: Share.RandomKey %>"
	TargetControlID="x_ParentCategoryID" Category="x_ParentCategoryID" 
	PromptText="Please Select" ServiceMethod="GetDropDownList" ServicePath="ewWebService.asmx" 
	SqlCommand="SELECT [SiteCategoryID], [CategoryName] FROM [SiteCategory] WHERE ([SiteCategoryTypeID]=@FILTER_VALUE) ORDER BY [CategoryName] Asc"
	ParentCategory="x_SiteCategoryTypeID" ParentControlID="x_SiteCategoryTypeID"
	DisplayFieldNames="CategoryName" EnabledDropDown="true" />
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
				<asp:HiddenField id="x_SiteCategoryID" runat="server" />
				</EditItemTemplate>
			</asp:TemplateField>
		</Fields>
	</asp:DetailsView>
	<br />
	<input type="hidden" id="keyID" runat="server" />
</asp:Content>
