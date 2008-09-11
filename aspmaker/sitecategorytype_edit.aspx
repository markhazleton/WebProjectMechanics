<%@ Page EnableViewState="true" Language="VB" ValidateRequest="False" Debug="True" MasterPageFile="masterpage.master" %>
<%@ MasterType VirtualPath="masterpage.master" %>
<%@ Import Namespace="PMGEN" %>
<%@ Import Namespace="EW.Data" %>
<%@ Import Namespace="EW.Data.Utilities" %>
<%@ Import Namespace="EW.Web" %>
<%@ Register NameSpace="PMGEN" TagPrefix="PMGEN" %>
<script runat="server">
	Dim key As SiteCategoryTypekey ' record key
	Dim oldrow As SiteCategoryTyperow ' old record data input by user
	Dim newrow As SiteCategoryTyperow ' new record data input by user
	Dim objProfile As TableProfile ' table profile

	' *************************
	' *  Handler for Page Init
	' *************************

	Protected Sub Page_Init(ByVal s As Object, ByVal e As System.EventArgs)
		If Not mhUser.IsAdmin() Then
		  Session("Login_Link") = Request.RawUrl
		  Response.Redirect("/mhweb/login/login.aspx")
		End If
		objProfile = CustomProfile.GetTable(Share.ProjectName, SiteCategoryTypeinf.TableVar)
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
				key = New SiteCategoryTypekey()
				Dim messageList As ArrayList = SiteCategoryTypeinf.LoadKey(key) 
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
			key = TryCast(ViewState("key"), SiteCategoryTypekey) ' restore from ViewState for postback
		End If
		If (SiteCategoryTypeDetailsView.Visible) Then
			RegisterClientID("CtrlID", SiteCategoryTypeDetailsView)
		End If
		If (SiteCategoryTypeDetailsView.FindControl("x_SiteCategoryTypeNM") IsNot Nothing) Then
			Page.Form.DefaultFocus = SiteCategoryTypeDetailsView.FindControl("x_SiteCategoryTypeNM").ClientID
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

	Protected Sub SiteCategoryTypeDetailsView_Init(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			SiteCategoryTypedal.OpenConnection()
		Catch err As Exception
			lblMessage.Text = err.Message
			pnlMessage.Visible = True
		End Try
	End Sub

	' *************************************
	' *  Handler for DetailsView DataBound
	' *************************************

	Protected Sub SiteCategoryTypeDetailsView_DataBound(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim source As DetailsView = SiteCategoryTypeDetailsView
		Dim row As SiteCategoryTyperow = TryCast(SiteCategoryTypeDetailsView.DataItem, SiteCategoryTyperow) ' get data object
		If (SiteCategoryTypeDetailsView.CurrentMode = DetailsViewMode.Edit) Then
			If (row IsNot Nothing) Then
				key.SiteCategoryTypeID = Convert.ToInt32(row.SiteCategoryTypeID)
			End If
			RowToControl(row, source, Core.CtrlType.Edit)
		End If
	End Sub

	' ****************************************
	' *  Handler for DetailsView ItemUpdating
	' ****************************************

	Protected Sub SiteCategoryTypeDetailsView_ItemUpdating(ByVal sender As Object, ByVal e As DetailsViewUpdateEventArgs)
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

	Protected Sub SiteCategoryTypeDetailsView_ItemUpdated(ByVal sender As Object, ByVal e As DetailsViewUpdatedEventArgs)

		' Check If update successful 
		If (e.AffectedRows = 0) Then

			' Keep in edit mode 
			e.KeepInEditMode = True

			' Get original values 
			Dim source As WebControl = TryCast(sender, WebControl)
			If (source Is Nothing) Then Return
			Dim oldrow As SiteCategoryTyperow = New SiteCategoryTyperow()
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

	Protected Sub SiteCategoryTypeDetailsView_Unload(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			SiteCategoryTypedal.CloseAndDisposeConnection()
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
		If (control.FindControl("x_SiteCategoryTypeNM") IsNot Nothing) Then
			jsString &= "ew.Controls['x_SiteCategoryTypeNM'] = '" & control.FindControl("x_SiteCategoryTypeNM").ClientID & "';"
		End If
		If (control.FindControl("x_SiteCategoryTypeDS") IsNot Nothing) Then
			jsString &= "ew.Controls['x_SiteCategoryTypeDS'] = '" & control.FindControl("x_SiteCategoryTypeDS").ClientID & "';"
		End If
		If (control.FindControl("x_SiteCategoryComment") IsNot Nothing) Then
			jsString &= "ew.Controls['x_SiteCategoryComment'] = '" & control.FindControl("x_SiteCategoryComment").ClientID & "';"
		End If
		If (control.FindControl("x_SiteCategoryFileName") IsNot Nothing) Then
			jsString &= "ew.Controls['x_SiteCategoryFileName'] = '" & control.FindControl("x_SiteCategoryFileName").ClientID & "';"
		End If
		If (control.FindControl("x_SiteCategoryTransferURL") IsNot Nothing) Then
			jsString &= "ew.Controls['x_SiteCategoryTransferURL'] = '" & control.FindControl("x_SiteCategoryTransferURL").ClientID & "';"
		End If
		If (control.FindControl("x_DefaultSiteCategoryID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_DefaultSiteCategoryID'] = '" & control.FindControl("x_DefaultSiteCategoryID").ClientID & "';"
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
			Dim x_SiteCategoryTypeNM As TextBox = TryCast(control.FindControl("x_SiteCategoryTypeNM"), TextBox)
			If (x_SiteCategoryTypeNM IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_SiteCategoryTypeNM.Text)) Then
					messageList.Add("Invalid Value (String): Site Category Type NM")
				End If
			End If
			Dim x_SiteCategoryTypeDS As TextBox = TryCast(control.FindControl("x_SiteCategoryTypeDS"), TextBox)
			If (x_SiteCategoryTypeDS IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_SiteCategoryTypeDS.Text)) Then
					messageList.Add("Invalid Value (String): Site Category Type DS")
				End If
			End If
			Dim x_SiteCategoryComment As TextBox = TryCast(control.FindControl("x_SiteCategoryComment"), TextBox)
			If (x_SiteCategoryComment IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_SiteCategoryComment.Text)) Then
					messageList.Add("Invalid Value (String): Site Category Comment")
				End If
			End If
			Dim x_SiteCategoryFileName As TextBox = TryCast(control.FindControl("x_SiteCategoryFileName"), TextBox)
			If (x_SiteCategoryFileName IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_SiteCategoryFileName.Text)) Then
					messageList.Add("Invalid Value (String): Site Category File Name")
				End If
			End If
			Dim x_SiteCategoryTransferURL As TextBox = TryCast(control.FindControl("x_SiteCategoryTransferURL"), TextBox)
			If (x_SiteCategoryTransferURL IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_SiteCategoryTransferURL.Text)) Then
					messageList.Add("Invalid Value (String): Site Category Transfer URL")
				End If
			End If
			Dim x_DefaultSiteCategoryID As DropDownList = TryCast(control.FindControl("x_DefaultSiteCategoryID"), DropDownList)
			If (x_DefaultSiteCategoryID IsNot Nothing) Then
				If ((x_DefaultSiteCategoryID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_DefaultSiteCategoryID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Default Site Category ID")
				End If
			End If
		End If
		Return IIf(messageList.Count = 0, Nothing, messageList)
	End Function

	' **********************************************
	' *  Convert Input Control Values to Row Object
	' **********************************************

	Private Sub ControlToRow(ByVal row As SiteCategoryTyperow, ByVal control As WebControl)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		row.SetPageType(Core.PageType.Edit) ' set page type = list for update

		' Field SiteCategoryTypeID
		' Field SiteCategoryTypeNM

		Dim x_SiteCategoryTypeNM As TextBox = TryCast(control.FindControl("x_SiteCategoryTypeNM"), TextBox)
		If (x_SiteCategoryTypeNM.Text <> String.Empty) Then row.SiteCategoryTypeNM = x_SiteCategoryTypeNM.Text Else row.SiteCategoryTypeNM = Nothing

		' Field SiteCategoryTypeDS
		Dim x_SiteCategoryTypeDS As TextBox = TryCast(control.FindControl("x_SiteCategoryTypeDS"), TextBox)
		If (x_SiteCategoryTypeDS.Text <> String.Empty) Then row.SiteCategoryTypeDS = x_SiteCategoryTypeDS.Text Else row.SiteCategoryTypeDS = Nothing

		' Field SiteCategoryComment
		Dim x_SiteCategoryComment As TextBox = TryCast(control.FindControl("x_SiteCategoryComment"), TextBox)
		If (x_SiteCategoryComment.Text <> String.Empty) Then row.SiteCategoryComment = x_SiteCategoryComment.Text Else row.SiteCategoryComment = Nothing

		' Field SiteCategoryFileName
		Dim x_SiteCategoryFileName As TextBox = TryCast(control.FindControl("x_SiteCategoryFileName"), TextBox)
		If (x_SiteCategoryFileName.Text <> String.Empty) Then row.SiteCategoryFileName = x_SiteCategoryFileName.Text Else row.SiteCategoryFileName = Nothing

		' Field SiteCategoryTransferURL
		Dim x_SiteCategoryTransferURL As TextBox = TryCast(control.FindControl("x_SiteCategoryTransferURL"), TextBox)
		If (x_SiteCategoryTransferURL.Text <> String.Empty) Then row.SiteCategoryTransferURL = x_SiteCategoryTransferURL.Text Else row.SiteCategoryTransferURL = Nothing

		' Field DefaultSiteCategoryID
		Dim x_DefaultSiteCategoryID As DropDownList = TryCast(control.FindControl("x_DefaultSiteCategoryID"), DropDownList)
		Dim v_DefaultSiteCategoryID As String = String.Empty
		If (x_DefaultSiteCategoryID.SelectedIndex >= 0) Then
			For Each li As ListItem In x_DefaultSiteCategoryID.Items
				If (li.Selected) Then
					v_DefaultSiteCategoryID += li.Value
					Exit For
				End If
			Next
		End If
		If (v_DefaultSiteCategoryID <> String.Empty) Then row.DefaultSiteCategoryID = Convert.ToInt32(v_DefaultSiteCategoryID) Else row.DefaultSiteCategoryID = CType(Nothing, Nullable(Of Int32))
	End Sub

	' ****************************
	' *  Check for duplicate Key
	' ****************************

	Private Function CheckDuplicateKey(ByVal control As WebControl) As ArrayList 
		Dim sWhere As String = String.Empty
		Dim messageList As ArrayList = New ArrayList()
		Dim data As SiteCategoryTypedal = New SiteCategoryTypedal()
		Dim newkey As SiteCategoryTypekey = New SiteCategoryTypekey()
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

	Private Sub RowToControl(ByVal row As SiteCategoryTyperow, ByVal control As WebControl, ByVal ct As Core.CtrlType)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		If (ct = Core.CtrlType.Edit) Then ' set up edit control

			' Field SiteCategoryTypeNM
			Dim x_SiteCategoryTypeNM As TextBox = TryCast(control.FindControl("x_SiteCategoryTypeNM"), TextBox)
			If (row.SiteCategoryTypeNM IsNot Nothing) Then x_SiteCategoryTypeNM.Text = row.SiteCategoryTypeNM.ToString() Else x_SiteCategoryTypeNM.Text = String.Empty

			' Field SiteCategoryTypeDS
			Dim x_SiteCategoryTypeDS As TextBox = TryCast(control.FindControl("x_SiteCategoryTypeDS"), TextBox)
			If (row.SiteCategoryTypeDS IsNot Nothing) Then x_SiteCategoryTypeDS.Text = row.SiteCategoryTypeDS.ToString() Else x_SiteCategoryTypeDS.Text = String.Empty

			' Field SiteCategoryComment
			Dim x_SiteCategoryComment As TextBox = TryCast(control.FindControl("x_SiteCategoryComment"), TextBox)
			If (row.SiteCategoryComment IsNot Nothing) Then x_SiteCategoryComment.Text = row.SiteCategoryComment.ToString() Else x_SiteCategoryComment.Text = String.Empty

			' Field SiteCategoryFileName
			Dim x_SiteCategoryFileName As TextBox = TryCast(control.FindControl("x_SiteCategoryFileName"), TextBox)
			If (row.SiteCategoryFileName IsNot Nothing) Then x_SiteCategoryFileName.Text = row.SiteCategoryFileName.ToString() Else x_SiteCategoryFileName.Text = String.Empty

			' Field SiteCategoryTransferURL
			Dim x_SiteCategoryTransferURL As TextBox = TryCast(control.FindControl("x_SiteCategoryTransferURL"), TextBox)
			If (row.SiteCategoryTransferURL IsNot Nothing) Then x_SiteCategoryTransferURL.Text = row.SiteCategoryTransferURL.ToString() Else x_SiteCategoryTransferURL.Text = String.Empty

			' Field DefaultSiteCategoryID
			Dim x_DefaultSiteCategoryID As DropDownList = TryCast(control.FindControl("x_DefaultSiteCategoryID"), DropDownList)
			x_DefaultSiteCategoryID.DataValueField = "ewValueField"
			x_DefaultSiteCategoryID.DataTextField = "ewTextField"
			Dim dv_x_DefaultSiteCategoryID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
			If (dv_x_DefaultSiteCategoryID Is Nothing) Then dv_x_DefaultSiteCategoryID = SiteCategoryTypedal.LookUpTable("DefaultSiteCategoryID")
			x_DefaultSiteCategoryID.DataSource = dv_x_DefaultSiteCategoryID
			x_DefaultSiteCategoryID.DataBind()
			x_DefaultSiteCategoryID.Items.Insert(0, New ListItem("Please Select", ""))
			Dim v_DefaultSiteCategoryID As String
			If (row.DefaultSiteCategoryID.HasValue) Then v_DefaultSiteCategoryID = Convert.ToString(row.DefaultSiteCategoryID) Else v_DefaultSiteCategoryID = String.Empty
			x_DefaultSiteCategoryID.ClearSelection()
			For Each li As ListItem In x_DefaultSiteCategoryID.Items
				If (li.Value.ToString() = v_DefaultSiteCategoryID) Then
					li.Selected = True
					Exit For
				End If
			Next
		End If
	End Sub

	' ************************************
	' *  Handler for DataSource Selecting
	' ************************************

	Protected Sub SiteCategoryTypeDataSource_Selecting(Byval sender As Object, ByVal e As ObjectDataSourceSelectingEventArgs)
		e.InputParameters.Clear()
		e.InputParameters.Add("filter", SiteCategoryTypeinf.GetUserFilter())
	End Sub

	' *************************************
	' *  DataSource Selected Event Handler
	' *************************************

	Protected Sub SiteCategoryTypeDataSource_Selected(Byval sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.ReturnValue Is Nothing) Then
			If (e.Exception IsNot Nothing) Then
				lblMessage.Text = e.Exception.Message
				pnlMessage.Visible = True
			Else
				lblMessage.Text = "No records found"
				pnlMessage.Visible = True
			End If
		Else
			Dim rows As SiteCategoryTyperows = TryCast(e.ReturnValue, SiteCategoryTyperows)
			Dim i As Integer = 0
			Do While (rows IsNot Nothing AndAlso i < rows.Count)
				If (key.Equals(rows(i).GetKey())) Then
					SiteCategoryTypeDetailsView.PageIndex = i
					Exit Do
				End If
				i += 1
			Loop
		End If
	End Sub

	' ***********************************
	' *  Handler for DataSource Updating
	' ***********************************

	Protected Sub SiteCategoryTypeDataSource_Updating(ByVal sender As Object, ByVal e As ObjectDataSourceMethodEventArgs)
		Dim source As DetailsView = SiteCategoryTypeDetailsView

		' Set up row object
		Dim row As SiteCategoryTyperow = TryCast(e.InputParameters(0), SiteCategoryTyperow)
		Dim data As SiteCategoryTypedal = New SiteCategoryTypedal()
		key.SiteCategoryTypeID = Convert.ToInt32(row.SiteCategoryTypeID)
		oldrow = data.LoadRow(key, SiteCategoryTypeinf.GetUserFilter())
		ControlToRow(row, source) ' load values of controls to row
		newrow = row
		If (SiteCategoryTypebll.Updating(oldrow, newrow)) Then
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

	Protected Sub SiteCategoryTypeDataSource_Updated(ByVal sender As Object, ByVal e As ObjectDataSourceStatusEventArgs)
		If (e.Exception IsNot Nothing) Then
			e.AffectedRows = 0
			e.ExceptionHandled = True
			lblMessage.Text = e.Exception.InnerException.Message
		Else
			lblMessage.Text = "Update successful"
			SiteCategoryTypebll.Updated(oldrow, newrow)
		End If
		If (lblMessage.Text <> "") Then pnlMessage.Visible = True
	End Sub

	' ************************************************
	' *  Change the page index for DetailsView paging
	' ************************************************

	Protected Sub ChangePageIndex(ByVal sender As Object, ByVal e As DetailsViewPageEventArgs)
		Dim data As SiteCategoryTypedal = New SiteCategoryTypedal()
		Dim rows As SiteCategoryTyperows = data.LoadList(SiteCategoryTypeinf.GetUserFilter())
		If (rows IsNot Nothing AndAlso rows.Count >= e.NewPageIndex) Then
			key = rows(e.NewPageIndex).GetKey()
		End If
	End Sub

	' *******************
	' *  Get User Filter
	' *******************

	Private Function GetUserFilter() As String 
		Return SiteCategoryTypeinf.GetUserFilter()
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
	<p><span class="aspnetmaker">Edit TABLE: Site Type</span></p>
	<p>
		<asp:Hyperlink id="lnkList" Text="Back to List" NavigateUrl="sitecategorytype_list.aspx" CssClass="aspnetmaker" runat="server" />
		<asp:Label id="lblReturnUrl" Visible="False" CssClass="aspnetmaker" runat="server">sitecategorytype_list.aspx</asp:Label>
	</p>
	<asp:ValidationSummary id="xevs_SiteCategoryType" CssClass="aspnetmaker" runat="server"
		HeaderText=""
		ShowSummary="False"
		ShowMessageBox="True"
		ForeColor="#FF0000" />
	<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
		<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
	</asp:PlaceHolder>
<!-- Data Source -->
<asp:ObjectDataSource ID="SiteCategoryTypeDataSource"
	TypeName="PMGEN.SiteCategoryTypedal"
	DataObjectTypeName="PMGEN.SiteCategoryTyperow"
	SelectMethod="LoadList"
	UpdateMethod="Update"
	OnSelecting="SiteCategoryTypeDataSource_Selecting"
	OnSelected="SiteCategoryTypeDataSource_Selected"
	OnUpdating="SiteCategoryTypeDataSource_Updating"
	OnUpdated="SiteCategoryTypeDataSource_Updated"
	runat="server">
</asp:ObjectDataSource>
<!-- DetailsView -->
	<asp:DetailsView ID="SiteCategoryTypeDetailsView"
		DataKeyNames="SiteCategoryTypeID"
		DataSourceID="SiteCategoryTypeDataSource"
		DefaultMode="Edit"
		GridLines="None"
		AutoGenerateRows="False" CssClass="ewDetailTable"
		OnInit="SiteCategoryTypeDetailsView_Init"
		OnDataBound="SiteCategoryTypeDetailsView_DataBound"
		OnItemUpdating="SiteCategoryTypeDetailsView_ItemUpdating"
		OnItemUpdated="SiteCategoryTypeDetailsView_ItemUpdated"
		OnUnload="SiteCategoryTypeDetailsView_Unload"
		AllowPaging="True"
		OnPageIndexChanging="ChangePageIndex"
		PagerSettings-Mode="NumericFirstLast"
		PagerSettings-Position="Bottom"
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
					<asp:Label runat="server" id="xs_SiteCategoryTypeID">Site Category Type ID</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:Label id="x_SiteCategoryTypeID" Text='<%# Eval("SiteCategoryTypeID")%>' CssClass="aspnetmaker" runat="server" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_SiteCategoryTypeNM">Site Category Type NM</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_SiteCategoryTypeNM" Columns="30" MaxLength="255" CssClass="aspnetmaker" runat="server" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_SiteCategoryTypeDS">Site Category Type DS</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_SiteCategoryTypeDS" Columns="30" MaxLength="255" CssClass="aspnetmaker" runat="server" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_SiteCategoryComment">Site Category Comment</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_SiteCategoryComment" Columns="30" MaxLength="255" CssClass="aspnetmaker" runat="server" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_SiteCategoryFileName">Site Category File Name</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_SiteCategoryFileName" Columns="30" MaxLength="255" CssClass="aspnetmaker" runat="server" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_SiteCategoryTransferURL">Site Category Transfer URL</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_SiteCategoryTransferURL" Columns="30" MaxLength="255" CssClass="aspnetmaker" runat="server" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_DefaultSiteCategoryID">Default Site Category ID</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:DropDownList ID="x_DefaultSiteCategoryID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
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
				</EditItemTemplate>
			</asp:TemplateField>
		</Fields>
	</asp:DetailsView>
	<br />
	<input type="hidden" id="keyID" runat="server" />
</asp:Content>
