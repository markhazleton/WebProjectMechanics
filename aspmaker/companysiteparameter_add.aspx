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
		Page.ClientScript.RegisterClientScriptInclude("ewv", "ewv.js")
		Response.Cache.SetCacheability(HttpCacheability.NoCache)
		If (Not Page.IsPostBack) Then
		End If
		If (CompanySiteParameterDetailsView.Visible) Then
			RegisterClientID("CtrlID", CompanySiteParameterDetailsView)
		End If
			If (CompanySiteParameterDetailsView.FindControl("x_CompanyID") IsNot Nothing) Then
				Page.Form.DefaultFocus = CompanySiteParameterDetailsView.FindControl("x_CompanyID").ClientID
			End If
	End Sub

	' **********************************************
	' *  Load Copy Data & Default Data to Controls
	' **********************************************

	Private Sub LoadData()
		Dim source As WebControl = TryCast(CompanySiteParameterDetailsView, WebControl)
		If (oldrow IsNot Nothing) Then ' old record (from recovery) exists, just restore
			RowToControl(oldrow, source, Core.CtrlType.Edit)
			oldrow = Nothing
		ElseIf (key IsNot Nothing) Then ' key loaded, try to copy record
			Dim data As CompanySiteParameterdal = New CompanySiteParameterdal()
			Dim row As CompanySiteParameterrow = data.LoadRow(key, CompanySiteParameterinf.GetUserFilter())
			If (row Is Nothing) Then
				objProfile.Message = "Record not found"
				Response.Redirect(lblReturnUrl.Text)
			End If
			RowToControl(row, source, Core.CtrlType.Edit)
			row = Nothing
		Else 
			Dim row As CompanySiteParameterrow = New CompanySiteParameterrow()
			row.SortOrder = Convert.ToInt32(999) ' set default value
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
		If (CompanySiteParameterDetailsView.CurrentMode = DetailsViewMode.Insert) Then
			LoadData()
		End If
	End Sub

	' *****************************************
	' *  Handler for DetailsView ItemInserting
	' *****************************************

	Protected Sub CompanySiteParameterDetailsView_ItemInserting(ByVal sender As Object, ByVal e As DetailsViewInsertEventArgs)
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

	Protected Sub CompanySiteParameterDetailsView_ItemInserted(ByVal sender As Object, ByVal e As DetailsViewInsertedEventArgs)
		If (e.AffectedRows = 0) Then

			' Keep in insert mode
			e.KeepInInsertMode = True

			' Get original values
			Dim source As WebControl = TryCast(sender, WebControl)
			If (source Is Nothing) Then Return
			oldrow = New CompanySiteParameterrow()
			ControlToRow(oldrow, source)
		Else
			objProfile.Message = "Add new record successful"
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
		row.SetPageType(Core.PageType.Add) ' set page type = list for update

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
	' *  Handler for DataSource Inserting
	' ************************************

	Protected Sub CompanySiteParameterDataSource_Inserting(ByVal sender As Object, ByVal e As ObjectDataSourceMethodEventArgs)
		Dim control As WebControl = CompanySiteParameterDetailsView

		' Set up row object
		Dim row As CompanySiteParameterrow = TryCast(e.InputParameters(0), CompanySiteParameterrow)
		ControlToRow(row, control)
		If (CompanySiteParameterbll.Inserting(row)) Then
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

	Protected Sub CompanySiteParameterDataSource_Inserted(ByVal sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.Exception IsNot Nothing) Then 
			e.AffectedRows = 0
			e.ExceptionHandled = True
			lblMessage.Text = e.Exception.InnerException.Message
			pnlMessage.Visible = True
		Else
			newrow = TryCast(e.ReturnValue, CompanySiteParameterrow) ' get new row objectinsert method
			CompanySiteParameterbll.Inserted(newrow)
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
	<p><span class="aspnetmaker">Add to TABLE: Site Parameter</span></p>
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
	InsertMethod="Insert"
	OnInserting="CompanySiteParameterDataSource_Inserting"
	OnInserted="CompanySiteParameterDataSource_Inserted"
	runat="server">
</asp:ObjectDataSource>
	<asp:DetailsView ID="CompanySiteParameterDetailsView"
		DataKeyNames="CompanySiteParameterID"
		DataSourceID="CompanySiteParameterDataSource"
		DefaultMode="Insert"
		GridLines="None"
		AutoGenerateRows="False" CssClass="ewDetailTable"
		OnInit="CompanySiteParameterDetailsView_Init"
		OnDataBound="CompanySiteParameterDetailsView_DataBound"
		OnItemInserting="CompanySiteParameterDetailsView_ItemInserting"
		OnItemInserted="CompanySiteParameterDetailsView_ItemInserted"
		OnUnload="CompanySiteParameterDetailsView_Unload"
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
					<asp:Label runat="server" id="xs_CompanyID">Site<span class='ewmsg'>&nbsp;*</span></asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:DropDownList ID="x_CompanyID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
<asp:RequiredFieldValidator ID="vx_CompanyID" CssClass="aspnetmaker" runat="server" ControlToValidate="x_CompanyID" ErrorMessage="Please enter required field - Site" ForeColor="Red" Display="None" Text="*" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_SiteParameterTypeID">Parameter Type<span class='ewmsg'>&nbsp;*</span></asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:DropDownList ID="x_SiteParameterTypeID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
<asp:RequiredFieldValidator ID="vx_SiteParameterTypeID" CssClass="aspnetmaker" runat="server" ControlToValidate="x_SiteParameterTypeID" ErrorMessage="Please enter required field - Parameter Type" ForeColor="Red" Display="None" Text="*" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_SortOrder">Order</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_SortOrder" Columns="30" CssClass="aspnetmaker" runat="server" />
<ew:IntegerValidator ID="vc_SortOrder" ClientValidationFunction="ew_CheckInteger" CssClass="aspnetmaker" runat="server" ControlToValidate="x_SortOrder" ErrorMessage="Incorrect integer - Order" Display="None" ForeColor="Red" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_ParameterValue">Parameter Value</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_ParameterValue" TextMode="MultiLine" Rows="4" Columns="35" CssClass="aspnetmaker" runat="server" />
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
					<asp:HiddenField ID="k_CompanySiteParameterID" Runat="server" Value='<%# Bind("CompanySiteParameterID") %>' />
				</InsertItemTemplate>
			</asp:TemplateField>
		</Fields>
	</asp:DetailsView>
</asp:Content>
