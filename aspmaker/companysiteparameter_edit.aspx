<%@ Page EnableViewState="true" Language="VB" ValidateRequest="False" Debug="True" MasterPageFile="masterpage.master" %>
<%@ MasterType VirtualPath="masterpage.master" %>
<%@ Import Namespace="PMGEN" %>
<%@ Import Namespace="EW.Data" %>
<%@ Import Namespace="EW.Data.Utilities" %>
<%@ Import Namespace="EW.Web" %>
<%@ Register NameSpace="PMGEN" TagPrefix="PMGEN" %>
<script runat="server">
	Dim key As CompanySiteParameterkey ' record key
	Dim oldrow As CompanySiteParameterrow ' old record data input by user
	Dim newrow As CompanySiteParameterrow ' new record data input by user
	Dim objProfile As TableProfile ' table profile

	' *************************
	' *  Handler for Page Init
	' *************************

	Protected Sub Page_Init(ByVal s As Object, ByVal e As System.EventArgs)
		If Not mhUser.IsAdmin() Then
		  Session("Login_Link") = Request.RawUrl
		  Response.Redirect("/mhweb/login/login.aspx")
		End If
		objProfile = CustomProfile.GetTable(Share.ProjectName, CompanySiteParameterinf.TableVar)
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
				key = New CompanySiteParameterkey()
				Dim messageList As ArrayList = CompanySiteParameterinf.LoadKey(key) 
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
			key = TryCast(ViewState("key"), CompanySiteParameterkey) ' restore from ViewState for postback
		End If
		If (CompanySiteParameterDetailsView.Visible) Then
			RegisterClientID("CtrlID", CompanySiteParameterDetailsView)
		End If
		If (CompanySiteParameterDetailsView.FindControl("x_CompanyID") IsNot Nothing) Then
			Page.Form.DefaultFocus = CompanySiteParameterDetailsView.FindControl("x_CompanyID").ClientID
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

	Protected Sub CompanySiteParameterDetailsView_Init(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			CompanySiteParameterdal.OpenConnection()
		Catch err As Exception
			lblMessage.Text = err.Message
			pnlMessage.Visible = True
		End Try
	End Sub

	' *************************************
	' *  Handler for DetailsView DataBound
	' *************************************

	Protected Sub CompanySiteParameterDetailsView_DataBound(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim source As DetailsView = CompanySiteParameterDetailsView
		Dim row As CompanySiteParameterrow = TryCast(CompanySiteParameterDetailsView.DataItem, CompanySiteParameterrow) ' get data object
		If (CompanySiteParameterDetailsView.CurrentMode = DetailsViewMode.Edit) Then
			If (row IsNot Nothing) Then
				key.CompanySiteParameterID = Convert.ToInt32(row.CompanySiteParameterID)
			End If
			RowToControl(row, source, Core.CtrlType.Edit)
		End If
	End Sub

	' ****************************************
	' *  Handler for DetailsView ItemUpdating
	' ****************************************

	Protected Sub CompanySiteParameterDetailsView_ItemUpdating(ByVal sender As Object, ByVal e As DetailsViewUpdateEventArgs)
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

	Protected Sub CompanySiteParameterDetailsView_ItemUpdated(ByVal sender As Object, ByVal e As DetailsViewUpdatedEventArgs)

		' Check If update successful 
		If (e.AffectedRows = 0) Then

			' Keep in edit mode 
			e.KeepInEditMode = True

			' Get original values 
			Dim source As WebControl = TryCast(sender, WebControl)
			If (source Is Nothing) Then Return
			Dim oldrow As CompanySiteParameterrow = New CompanySiteParameterrow()
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

	Protected Sub CompanySiteParameterDetailsView_Unload(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			CompanySiteParameterdal.CloseAndDisposeConnection()
		Catch
		End Try
	End Sub

	' ********************************
	' * Register ClientID of Controls
	' ********************************

	Private Sub RegisterClientID(ByVal sBlockName As String, ByVal control As Control)
		Dim jsString As String = String.Empty
		If (control.FindControl("x_CompanyID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_CompanyID'] = '" & control.FindControl("x_CompanyID").ClientID & "';"
		End If
		If (control.FindControl("x_SiteParameterTypeID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_SiteParameterTypeID'] = '" & control.FindControl("x_SiteParameterTypeID").ClientID & "';"
		End If
		If (control.FindControl("x_SortOrder") IsNot Nothing) Then
			jsString &= "ew.Controls['x_SortOrder'] = '" & control.FindControl("x_SortOrder").ClientID & "';"
		End If
		If (control.FindControl("x_ParameterValue") IsNot Nothing) Then
			jsString &= "ew.Controls['x_ParameterValue'] = '" & control.FindControl("x_ParameterValue").ClientID & "';"
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
			Dim x_CompanyID As DropDownList = TryCast(control.FindControl("x_CompanyID"), DropDownList)
			If (x_CompanyID IsNot Nothing) Then
				If ((x_CompanyID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_CompanyID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Site")
				ElseIf (String.IsNullOrEmpty(x_CompanyID.SelectedValue)) Then
					messageList.Add("Please enter required field (Int32): Site")
				End If
			End If
			Dim x_SiteParameterTypeID As DropDownList = TryCast(control.FindControl("x_SiteParameterTypeID"), DropDownList)
			If (x_SiteParameterTypeID IsNot Nothing) Then
				If ((x_SiteParameterTypeID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_SiteParameterTypeID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Parameter Type")
				ElseIf (String.IsNullOrEmpty(x_SiteParameterTypeID.SelectedValue)) Then
					messageList.Add("Please enter required field (Int32): Parameter Type")
				End If
			End If
			Dim x_SortOrder As TextBox = TryCast(control.FindControl("x_SortOrder"), TextBox)
			If (x_SortOrder IsNot Nothing) Then
				If (Not DataFormat.CheckInt32(x_SortOrder.Text)) Then
					messageList.Add("Invalid Value (Int32): Order")
				End If
			End If
			Dim x_ParameterValue As TextBox = TryCast(control.FindControl("x_ParameterValue"), TextBox)
			If (x_ParameterValue IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_ParameterValue.Text)) Then
					messageList.Add("Invalid Value (String): Parameter Value")
				End If
			End If
		End If
		Return IIf(messageList.Count = 0, Nothing, messageList)
	End Function

	' **********************************************
	' *  Convert Input Control Values to Row Object
	' **********************************************

	Private Sub ControlToRow(ByVal row As CompanySiteParameterrow, ByVal control As WebControl)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		row.SetPageType(Core.PageType.Edit) ' set page type = list for update

		' Field CompanySiteParameterID
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

		' Field SiteParameterTypeID
		Dim x_SiteParameterTypeID As DropDownList = TryCast(control.FindControl("x_SiteParameterTypeID"), DropDownList)
		Dim v_SiteParameterTypeID As String = String.Empty
		If (x_SiteParameterTypeID.SelectedIndex >= 0) Then
			For Each li As ListItem In x_SiteParameterTypeID.Items
				If (li.Selected) Then
					v_SiteParameterTypeID += li.Value
					Exit For
				End If
			Next
		End If
		If (v_SiteParameterTypeID <> String.Empty) Then row.SiteParameterTypeID = Convert.ToInt32(v_SiteParameterTypeID) Else row.SiteParameterTypeID = CType(Nothing, Nullable(Of Int32))

		' Field SortOrder
		Dim x_SortOrder As TextBox = TryCast(control.FindControl("x_SortOrder"), TextBox)
		If (x_SortOrder.Text <> String.Empty) Then row.SortOrder = Convert.ToInt32(x_SortOrder.Text) Else row.SortOrder = CType(Nothing, Nullable(Of Int32))

		' Field ParameterValue
		Dim x_ParameterValue As TextBox = TryCast(control.FindControl("x_ParameterValue"), TextBox)
		If (x_ParameterValue.Text <> String.Empty) Then row.ParameterValue = x_ParameterValue.Text Else row.ParameterValue = Nothing
	End Sub

	' ****************************
	' *  Check for duplicate Key
	' ****************************

	Private Function CheckDuplicateKey(ByVal control As WebControl) As ArrayList 
		Dim sWhere As String = String.Empty
		Dim messageList As ArrayList = New ArrayList()
		Dim data As CompanySiteParameterdal = New CompanySiteParameterdal()
		Dim newkey As CompanySiteParameterkey = New CompanySiteParameterkey()
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

	Private Sub RowToControl(ByVal row As CompanySiteParameterrow, ByVal control As WebControl, ByVal ct As Core.CtrlType)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		If (ct = Core.CtrlType.Edit) Then ' set up edit control

			' Field CompanyID
			Dim x_CompanyID As DropDownList = TryCast(control.FindControl("x_CompanyID"), DropDownList)
			x_CompanyID.DataValueField = "ewValueField"
			x_CompanyID.DataTextField = "ewTextField"
			Dim dv_x_CompanyID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
			If (dv_x_CompanyID Is Nothing) Then dv_x_CompanyID = CompanySiteParameterdal.LookUpTable("CompanyID")
			x_CompanyID.DataSource = dv_x_CompanyID
			x_CompanyID.DataBind()
			x_CompanyID.Items.Insert(0, New ListItem("Please Select", ""))
			Dim v_CompanyID As String
			If (row.CompanyID.HasValue) Then v_CompanyID = Convert.ToString(row.CompanyID) Else v_CompanyID = String.Empty
			x_CompanyID.ClearSelection()
			For Each li As ListItem In x_CompanyID.Items
				If (li.Value.ToString() = v_CompanyID) Then
					li.Selected = True
					Exit For
				End If
			Next

			' Field SiteParameterTypeID
			Dim x_SiteParameterTypeID As DropDownList = TryCast(control.FindControl("x_SiteParameterTypeID"), DropDownList)
			x_SiteParameterTypeID.DataValueField = "ewValueField"
			x_SiteParameterTypeID.DataTextField = "ewTextField"
			Dim dv_x_SiteParameterTypeID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
			If (dv_x_SiteParameterTypeID Is Nothing) Then dv_x_SiteParameterTypeID = CompanySiteParameterdal.LookUpTable("SiteParameterTypeID")
			x_SiteParameterTypeID.DataSource = dv_x_SiteParameterTypeID
			x_SiteParameterTypeID.DataBind()
			x_SiteParameterTypeID.Items.Insert(0, New ListItem("Please Select", ""))
			Dim v_SiteParameterTypeID As String
			If (row.SiteParameterTypeID.HasValue) Then v_SiteParameterTypeID = Convert.ToString(row.SiteParameterTypeID) Else v_SiteParameterTypeID = String.Empty
			x_SiteParameterTypeID.ClearSelection()
			For Each li As ListItem In x_SiteParameterTypeID.Items
				If (li.Value.ToString() = v_SiteParameterTypeID) Then
					li.Selected = True
					Exit For
				End If
			Next

			' Field SortOrder
			Dim x_SortOrder As TextBox = TryCast(control.FindControl("x_SortOrder"), TextBox)
			If (row.SortOrder.HasValue) Then x_SortOrder.Text = row.SortOrder.ToString() Else x_SortOrder.Text = String.Empty

			' Field ParameterValue
			Dim x_ParameterValue As TextBox = TryCast(control.FindControl("x_ParameterValue"), TextBox)
			If (row.ParameterValue IsNot Nothing) Then x_ParameterValue.Text = row.ParameterValue.ToString() Else x_ParameterValue.Text = String.Empty
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

	Protected Sub CompanySiteParameterDataSource_Selecting(Byval sender As Object, ByVal e As ObjectDataSourceSelectingEventArgs)
		e.InputParameters.Clear()
		e.InputParameters.Add("filter", CompanySiteParameterinf.GetUserFilter())
	End Sub

	' *************************************
	' *  DataSource Selected Event Handler
	' *************************************

	Protected Sub CompanySiteParameterDataSource_Selected(Byval sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.ReturnValue Is Nothing) Then
			If (e.Exception IsNot Nothing) Then
				lblMessage.Text = e.Exception.Message
				pnlMessage.Visible = True
			Else
				lblMessage.Text = "No records found"
				pnlMessage.Visible = True
			End If
		Else
			Dim rows As CompanySiteParameterrows = TryCast(e.ReturnValue, CompanySiteParameterrows)
			Dim i As Integer = 0
			Do While (rows IsNot Nothing AndAlso i < rows.Count)
				If (key.Equals(rows(i).GetKey())) Then
					CompanySiteParameterDetailsView.PageIndex = i
					Exit Do
				End If
				i += 1
			Loop
		End If
	End Sub

	' ***********************************
	' *  Handler for DataSource Updating
	' ***********************************

	Protected Sub CompanySiteParameterDataSource_Updating(ByVal sender As Object, ByVal e As ObjectDataSourceMethodEventArgs)
		Dim source As DetailsView = CompanySiteParameterDetailsView

		' Set up row object
		Dim row As CompanySiteParameterrow = TryCast(e.InputParameters(0), CompanySiteParameterrow)
		Dim data As CompanySiteParameterdal = New CompanySiteParameterdal()
		key.CompanySiteParameterID = Convert.ToInt32(row.CompanySiteParameterID)
		oldrow = data.LoadRow(key, CompanySiteParameterinf.GetUserFilter())
		ControlToRow(row, source) ' load values of controls to row
		newrow = row
		If (CompanySiteParameterbll.Updating(oldrow, newrow)) Then
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

	Protected Sub CompanySiteParameterDataSource_Updated(ByVal sender As Object, ByVal e As ObjectDataSourceStatusEventArgs)
		If (e.Exception IsNot Nothing) Then
			e.AffectedRows = 0
			e.ExceptionHandled = True
			lblMessage.Text = e.Exception.InnerException.Message
		Else
			lblMessage.Text = "Update successful"
			CompanySiteParameterbll.Updated(oldrow, newrow)
		End If
		If (lblMessage.Text <> "") Then pnlMessage.Visible = True
	End Sub

	' ************************************************
	' *  Change the page index for DetailsView paging
	' ************************************************

	Protected Sub ChangePageIndex(ByVal sender As Object, ByVal e As DetailsViewPageEventArgs)
		Dim data As CompanySiteParameterdal = New CompanySiteParameterdal()
		Dim rows As CompanySiteParameterrows = data.LoadList(CompanySiteParameterinf.GetUserFilter())
		If (rows IsNot Nothing AndAlso rows.Count >= e.NewPageIndex) Then
			key = rows(e.NewPageIndex).GetKey()
		End If
	End Sub

	' *******************
	' *  Get User Filter
	' *******************

	Private Function GetUserFilter() As String 
		Return CompanySiteParameterinf.GetUserFilter()
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
	<p><span class="aspnetmaker">Edit TABLE: Site Parameter</span></p>
	<p>
		<asp:Hyperlink id="lnkList" Text="Back to List" NavigateUrl="companysiteparameter_list.aspx" CssClass="aspnetmaker" runat="server" />
		<asp:Label id="lblReturnUrl" Visible="False" CssClass="aspnetmaker" runat="server">companysiteparameter_list.aspx</asp:Label>
	</p>
	<asp:ValidationSummary id="xevs_CompanySiteParameter" CssClass="aspnetmaker" runat="server"
		HeaderText=""
		ShowSummary="False"
		ShowMessageBox="True"
		ForeColor="#FF0000" />
	<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
		<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
	</asp:PlaceHolder>
<!-- Data Source -->
<asp:ObjectDataSource ID="CompanySiteParameterDataSource"
	TypeName="PMGEN.CompanySiteParameterdal"
	DataObjectTypeName="PMGEN.CompanySiteParameterrow"
	SelectMethod="LoadList"
	UpdateMethod="Update"
	OnSelecting="CompanySiteParameterDataSource_Selecting"
	OnSelected="CompanySiteParameterDataSource_Selected"
	OnUpdating="CompanySiteParameterDataSource_Updating"
	OnUpdated="CompanySiteParameterDataSource_Updated"
	runat="server">
</asp:ObjectDataSource>
<!-- DetailsView -->
	<asp:DetailsView ID="CompanySiteParameterDetailsView"
		DataKeyNames="CompanySiteParameterID"
		DataSourceID="CompanySiteParameterDataSource"
		DefaultMode="Edit"
		GridLines="None"
		AutoGenerateRows="False" CssClass="ewDetailTable"
		OnInit="CompanySiteParameterDetailsView_Init"
		OnDataBound="CompanySiteParameterDetailsView_DataBound"
		OnItemUpdating="CompanySiteParameterDetailsView_ItemUpdating"
		OnItemUpdated="CompanySiteParameterDetailsView_ItemUpdated"
		OnUnload="CompanySiteParameterDetailsView_Unload"
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
					<asp:Label runat="server" id="xs_CompanyID">Site<span class='ewmsg'>&nbsp;*</span></asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:DropDownList ID="x_CompanyID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
<asp:RequiredFieldValidator ID="vx_CompanyID" CssClass="aspnetmaker" runat="server" ControlToValidate="x_CompanyID" ErrorMessage="Please enter required field - Site" ForeColor="Red" Display="None" Text="*" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_SiteParameterTypeID">Parameter Type<span class='ewmsg'>&nbsp;*</span></asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:DropDownList ID="x_SiteParameterTypeID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
<asp:RequiredFieldValidator ID="vx_SiteParameterTypeID" CssClass="aspnetmaker" runat="server" ControlToValidate="x_SiteParameterTypeID" ErrorMessage="Please enter required field - Parameter Type" ForeColor="Red" Display="None" Text="*" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_SortOrder">Order</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_SortOrder" Columns="30" CssClass="aspnetmaker" runat="server" />
<ew:IntegerValidator ID="vc_SortOrder" ClientValidationFunction="ew_CheckInteger" CssClass="aspnetmaker" runat="server" ControlToValidate="x_SortOrder" ErrorMessage="Incorrect integer - Order" Display="None" ForeColor="Red" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_ParameterValue">Parameter Value</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_ParameterValue" TextMode="MultiLine" Rows="4" Columns="35" CssClass="aspnetmaker" runat="server" />
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
				<asp:HiddenField id="x_CompanySiteParameterID" runat="server" />
				</EditItemTemplate>
			</asp:TemplateField>
		</Fields>
	</asp:DetailsView>
	<br />
	<input type="hidden" id="keyID" runat="server" />
</asp:Content>