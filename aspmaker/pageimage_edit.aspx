<%@ Page EnableViewState="true" Language="VB" ValidateRequest="False" Debug="True" MasterPageFile="masterpage.master" %>
<%@ MasterType VirtualPath="masterpage.master" %>
<%@ Import Namespace="PMGEN" %>
<%@ Import Namespace="EW.Data" %>
<%@ Import Namespace="EW.Data.Utilities" %>
<%@ Import Namespace="EW.Web" %>
<%@ Register NameSpace="PMGEN" TagPrefix="PMGEN" %>
<script runat="server">
	Dim key As PageImagekey ' record key
	Dim oldrow As PageImagerow ' old record data input by user
	Dim newrow As PageImagerow ' new record data input by user
	Dim objProfile As TableProfile ' table profile

	' *************************
	' *  Handler for Page Init
	' *************************

	Protected Sub Page_Init(ByVal s As Object, ByVal e As System.EventArgs)
		If Not mhUser.IsAdmin() Then
		  Session("Login_Link") = Request.RawUrl
		  Response.Redirect("/mhweb/login/login.aspx")
		End If
		objProfile = CustomProfile.GetTable(Share.ProjectName, PageImageinf.TableVar)
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
				key = New PageImagekey()
				Dim messageList As ArrayList = PageImageinf.LoadKey(key) 
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
			key = TryCast(ViewState("key"), PageImagekey) ' restore from ViewState for postback
		End If
		If (PageImageDetailsView.Visible) Then
			RegisterClientID("CtrlID", PageImageDetailsView)
		End If
		If (PageImageDetailsView.FindControl("x_PageID") IsNot Nothing) Then
			Page.Form.DefaultFocus = PageImageDetailsView.FindControl("x_PageID").ClientID
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

	Protected Sub PageImageDetailsView_Init(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			PageImagedal.OpenConnection()
		Catch err As Exception
			lblMessage.Text = err.Message
			pnlMessage.Visible = True
		End Try
	End Sub

	' *************************************
	' *  Handler for DetailsView DataBound
	' *************************************

	Protected Sub PageImageDetailsView_DataBound(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim source As DetailsView = PageImageDetailsView
		Dim row As PageImagerow = TryCast(PageImageDetailsView.DataItem, PageImagerow) ' get data object
		If (PageImageDetailsView.CurrentMode = DetailsViewMode.Edit) Then
			If (row IsNot Nothing) Then
				key.PageImageID = Convert.ToInt32(row.PageImageID)
			End If
			RowToControl(row, source, Core.CtrlType.Edit)
		End If
	End Sub

	' ****************************************
	' *  Handler for DetailsView ItemUpdating
	' ****************************************

	Protected Sub PageImageDetailsView_ItemUpdating(ByVal sender As Object, ByVal e As DetailsViewUpdateEventArgs)
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

	Protected Sub PageImageDetailsView_ItemUpdated(ByVal sender As Object, ByVal e As DetailsViewUpdatedEventArgs)

		' Check If update successful 
		If (e.AffectedRows = 0) Then

			' Keep in edit mode 
			e.KeepInEditMode = True

			' Get original values 
			Dim source As WebControl = TryCast(sender, WebControl)
			If (source Is Nothing) Then Return
			Dim oldrow As PageImagerow = New PageImagerow()
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

	Protected Sub PageImageDetailsView_Unload(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			PageImagedal.CloseAndDisposeConnection()
		Catch
		End Try
	End Sub

	' ********************************
	' * Register ClientID of Controls
	' ********************************

	Private Sub RegisterClientID(ByVal sBlockName As String, ByVal control As Control)
		Dim jsString As String = String.Empty
		If (control.FindControl("x_PageID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_PageID'] = '" & control.FindControl("x_PageID").ClientID & "';"
		End If
		If (control.FindControl("x_ImageID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_ImageID'] = '" & control.FindControl("x_ImageID").ClientID & "';"
		End If
		If (control.FindControl("x_PageImagePosition") IsNot Nothing) Then
			jsString &= "ew.Controls['x_PageImagePosition'] = '" & control.FindControl("x_PageImagePosition").ClientID & "';"
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
			Dim x_PageID As DropDownList = TryCast(control.FindControl("x_PageID"), DropDownList)
			If (x_PageID IsNot Nothing) Then
				If ((x_PageID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_PageID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Page")
				ElseIf (String.IsNullOrEmpty(x_PageID.SelectedValue)) Then
					messageList.Add("Please enter required field (Int32): Page")
				End If
			End If
			Dim x_ImageID As DropDownList = TryCast(control.FindControl("x_ImageID"), DropDownList)
			If (x_ImageID IsNot Nothing) Then
				If ((x_ImageID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_ImageID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Image")
				ElseIf (String.IsNullOrEmpty(x_ImageID.SelectedValue)) Then
					messageList.Add("Please enter required field (Int32): Image")
				End If
			End If
			Dim x_PageImagePosition As TextBox = TryCast(control.FindControl("x_PageImagePosition"), TextBox)
			If (x_PageImagePosition IsNot Nothing) Then
				If (Not DataFormat.CheckInt32(x_PageImagePosition.Text)) Then
					messageList.Add("Invalid Value (Int32): Position")
				End If
			End If
		End If
		Return IIf(messageList.Count = 0, Nothing, messageList)
	End Function

	' **********************************************
	' *  Convert Input Control Values to Row Object
	' **********************************************

	Private Sub ControlToRow(ByVal row As PageImagerow, ByVal control As WebControl)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		row.SetPageType(Core.PageType.Edit) ' set page type = list for update

		' Field PageImageID
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

		' Field ImageID
		Dim x_ImageID As DropDownList = TryCast(control.FindControl("x_ImageID"), DropDownList)
		Dim v_ImageID As String = String.Empty
		If (x_ImageID.SelectedIndex >= 0) Then
			For Each li As ListItem In x_ImageID.Items
				If (li.Selected) Then
					v_ImageID += li.Value
					Exit For
				End If
			Next
		End If
		If (v_ImageID <> String.Empty) Then row.ImageID = Convert.ToInt32(v_ImageID) Else row.ImageID = CType(Nothing, Nullable(Of Int32))

		' Field PageImagePosition
		Dim x_PageImagePosition As TextBox = TryCast(control.FindControl("x_PageImagePosition"), TextBox)
		If (x_PageImagePosition.Text <> String.Empty) Then row.PageImagePosition = Convert.ToInt32(x_PageImagePosition.Text) Else row.PageImagePosition = CType(Nothing, Nullable(Of Int32))
	End Sub

	' ****************************
	' *  Check for duplicate Key
	' ****************************

	Private Function CheckDuplicateKey(ByVal control As WebControl) As ArrayList 
		Dim sWhere As String = String.Empty
		Dim messageList As ArrayList = New ArrayList()
		Dim data As PageImagedal = New PageImagedal()
		Dim newkey As PageImagekey = New PageImagekey()
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

	Private Sub RowToControl(ByVal row As PageImagerow, ByVal control As WebControl, ByVal ct As Core.CtrlType)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		If (ct = Core.CtrlType.Edit) Then ' set up edit control

			' Field PageID
			Dim x_PageID As DropDownList = TryCast(control.FindControl("x_PageID"), DropDownList)
			x_PageID.DataValueField = "ewValueField"
			x_PageID.DataTextField = "ewTextField"
			Dim dv_x_PageID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
			If (dv_x_PageID Is Nothing) Then dv_x_PageID = PageImagedal.LookUpTable("PageID")
			x_PageID.DataSource = dv_x_PageID
			x_PageID.DataBind()
			x_PageID.Items.Insert(0, New ListItem("Please Select", ""))
			Dim v_PageID As String
			If (row.PageID.HasValue) Then v_PageID = Convert.ToString(row.PageID) Else v_PageID = String.Empty
			x_PageID.ClearSelection()
			For Each li As ListItem In x_PageID.Items
				If (li.Value.ToString() = v_PageID) Then
					li.Selected = True
					Exit For
				End If
			Next

			' Field ImageID
			Dim x_ImageID As DropDownList = TryCast(control.FindControl("x_ImageID"), DropDownList)
			x_ImageID.DataValueField = "ewValueField"
			x_ImageID.DataTextField = "ewTextField"
			Dim dv_x_ImageID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
			If (dv_x_ImageID Is Nothing) Then dv_x_ImageID = PageImagedal.LookUpTable("ImageID")
			x_ImageID.DataSource = dv_x_ImageID
			x_ImageID.DataBind()
			x_ImageID.Items.Insert(0, New ListItem("Please Select", ""))
			Dim v_ImageID As String
			If (row.ImageID.HasValue) Then v_ImageID = Convert.ToString(row.ImageID) Else v_ImageID = String.Empty
			x_ImageID.ClearSelection()
			For Each li As ListItem In x_ImageID.Items
				If (li.Value.ToString() = v_ImageID) Then
					li.Selected = True
					Exit For
				End If
			Next

			' Field PageImagePosition
			Dim x_PageImagePosition As TextBox = TryCast(control.FindControl("x_PageImagePosition"), TextBox)
			If (row.PageImagePosition.HasValue) Then x_PageImagePosition.Text = row.PageImagePosition.ToString() Else x_PageImagePosition.Text = String.Empty
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

	Protected Sub PageImageDataSource_Selecting(Byval sender As Object, ByVal e As ObjectDataSourceSelectingEventArgs)
		e.InputParameters.Clear()
		e.InputParameters.Add("filter", PageImageinf.GetUserFilter())
	End Sub

	' *************************************
	' *  DataSource Selected Event Handler
	' *************************************

	Protected Sub PageImageDataSource_Selected(Byval sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.ReturnValue Is Nothing) Then
			If (e.Exception IsNot Nothing) Then
				lblMessage.Text = e.Exception.Message
				pnlMessage.Visible = True
			Else
				lblMessage.Text = "No records found"
				pnlMessage.Visible = True
			End If
		Else
			Dim rows As PageImagerows = TryCast(e.ReturnValue, PageImagerows)
			Dim i As Integer = 0
			Do While (rows IsNot Nothing AndAlso i < rows.Count)
				If (key.Equals(rows(i).GetKey())) Then
					PageImageDetailsView.PageIndex = i
					Exit Do
				End If
				i += 1
			Loop
		End If
	End Sub

	' ***********************************
	' *  Handler for DataSource Updating
	' ***********************************

	Protected Sub PageImageDataSource_Updating(ByVal sender As Object, ByVal e As ObjectDataSourceMethodEventArgs)
		Dim source As DetailsView = PageImageDetailsView

		' Set up row object
		Dim row As PageImagerow = TryCast(e.InputParameters(0), PageImagerow)
		Dim data As PageImagedal = New PageImagedal()
		key.PageImageID = Convert.ToInt32(row.PageImageID)
		oldrow = data.LoadRow(key, PageImageinf.GetUserFilter())
		ControlToRow(row, source) ' load values of controls to row
		newrow = row
		If (PageImagebll.Updating(oldrow, newrow)) Then
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

	Protected Sub PageImageDataSource_Updated(ByVal sender As Object, ByVal e As ObjectDataSourceStatusEventArgs)
		If (e.Exception IsNot Nothing) Then
			e.AffectedRows = 0
			e.ExceptionHandled = True
			lblMessage.Text = e.Exception.InnerException.Message
		Else
			lblMessage.Text = "Update successful"
			PageImagebll.Updated(oldrow, newrow)
		End If
		If (lblMessage.Text <> "") Then pnlMessage.Visible = True
	End Sub

	' ************************************************
	' *  Change the page index for DetailsView paging
	' ************************************************

	Protected Sub ChangePageIndex(ByVal sender As Object, ByVal e As DetailsViewPageEventArgs)
		Dim data As PageImagedal = New PageImagedal()
		Dim rows As PageImagerows = data.LoadList(PageImageinf.GetUserFilter())
		If (rows IsNot Nothing AndAlso rows.Count >= e.NewPageIndex) Then
			key = rows(e.NewPageIndex).GetKey()
		End If
	End Sub

	' *******************
	' *  Get User Filter
	' *******************

	Private Function GetUserFilter() As String 
		Return PageImageinf.GetUserFilter()
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
	<p><span class="aspnetmaker">Edit TABLE: Page Image</span></p>
	<p>
		<asp:Hyperlink id="lnkList" Text="Back to List" NavigateUrl="pageimage_list.aspx" CssClass="aspnetmaker" runat="server" />
		<asp:Label id="lblReturnUrl" Visible="False" CssClass="aspnetmaker" runat="server">pageimage_list.aspx</asp:Label>
	</p>
	<asp:ValidationSummary id="xevs_PageImage" CssClass="aspnetmaker" runat="server"
		HeaderText=""
		ShowSummary="False"
		ShowMessageBox="True"
		ForeColor="#FF0000" />
	<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
		<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
	</asp:PlaceHolder>
<!-- Data Source -->
<asp:ObjectDataSource ID="PageImageDataSource"
	TypeName="PMGEN.PageImagedal"
	DataObjectTypeName="PMGEN.PageImagerow"
	SelectMethod="LoadList"
	UpdateMethod="Update"
	OnSelecting="PageImageDataSource_Selecting"
	OnSelected="PageImageDataSource_Selected"
	OnUpdating="PageImageDataSource_Updating"
	OnUpdated="PageImageDataSource_Updated"
	runat="server">
</asp:ObjectDataSource>
<!-- DetailsView -->
	<asp:DetailsView ID="PageImageDetailsView"
		DataKeyNames="PageImageID"
		DataSourceID="PageImageDataSource"
		DefaultMode="Edit"
		GridLines="None"
		AutoGenerateRows="False" CssClass="ewDetailTable"
		OnInit="PageImageDetailsView_Init"
		OnDataBound="PageImageDetailsView_DataBound"
		OnItemUpdating="PageImageDetailsView_ItemUpdating"
		OnItemUpdated="PageImageDetailsView_ItemUpdated"
		OnUnload="PageImageDetailsView_Unload"
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
					<asp:Label runat="server" id="xs_PageID">Page<span class='ewmsg'>&nbsp;*</span></asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:DropDownList ID="x_PageID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
<asp:RequiredFieldValidator ID="vx_PageID" CssClass="aspnetmaker" runat="server" ControlToValidate="x_PageID" ErrorMessage="Please enter required field - Page" ForeColor="Red" Display="None" Text="*" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_ImageID">Image<span class='ewmsg'>&nbsp;*</span></asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:DropDownList ID="x_ImageID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
<asp:RequiredFieldValidator ID="vx_ImageID" CssClass="aspnetmaker" runat="server" ControlToValidate="x_ImageID" ErrorMessage="Please enter required field - Image" ForeColor="Red" Display="None" Text="*" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_PageImagePosition">Position</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_PageImagePosition" Columns="30" CssClass="aspnetmaker" runat="server" />
<ew:IntegerValidator ID="vc_PageImagePosition" ClientValidationFunction="ew_CheckInteger" CssClass="aspnetmaker" runat="server" ControlToValidate="x_PageImagePosition" ErrorMessage="Incorrect integer - Position" Display="None" ForeColor="Red" />
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
				<asp:HiddenField id="x_PageImageID" runat="server" />
				</EditItemTemplate>
			</asp:TemplateField>
		</Fields>
	</asp:DetailsView>
	<br />
	<input type="hidden" id="keyID" runat="server" />
</asp:Content>
