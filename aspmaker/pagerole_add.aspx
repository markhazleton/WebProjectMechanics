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
	Dim key As PageRolekey ' record key
	Dim oldrow As PageRolerow ' old record data input by user
	Dim newrow As PageRolerow ' new record data input by user
	Dim objProfile As TableProfile ' table profile

	' *************************
	' *  Handler for Page Init
	' *************************

	Protected Sub Page_Init(ByVal s As Object, ByVal e As System.EventArgs)
		If Not mhUser.IsAdmin() Then
		  Session("Login_Link") = Request.RawUrl
		  Response.Redirect("/mhweb/login/login.aspx")
		End If
		objProfile = CustomProfile.GetTable(Share.ProjectName, PageRoleinf.TableVar)
	End Sub

	' *************************
	' *  Handler for Page Load
	' *************************

	Protected Sub Page_Load(ByVal s As Object, ByVal e As System.EventArgs)
		Page.ClientScript.RegisterClientScriptInclude("ewv", "ewv.js")
		Response.Cache.SetCacheability(HttpCacheability.NoCache)
		If (Not Page.IsPostBack) Then
		End If
		If (PageRoleDetailsView.Visible) Then
			RegisterClientID("CtrlID", PageRoleDetailsView)
		End If
			If (PageRoleDetailsView.FindControl("x_RoleID") IsNot Nothing) Then
				Page.Form.DefaultFocus = PageRoleDetailsView.FindControl("x_RoleID").ClientID
			End If
	End Sub

	' **********************************************
	' *  Load Copy Data & Default Data to Controls
	' **********************************************

	Private Sub LoadData()
		Dim source As WebControl = TryCast(PageRoleDetailsView, WebControl)
		If (oldrow IsNot Nothing) Then ' old record (from recovery) exists, just restore
			RowToControl(oldrow, source, Core.CtrlType.Edit)
			oldrow = Nothing
		ElseIf (key IsNot Nothing) Then ' key loaded, try to copy record
			Dim data As PageRoledal = New PageRoledal()
			Dim row As PageRolerow = data.LoadRow(key, PageRoleinf.GetUserFilter())
			If (row Is Nothing) Then
				objProfile.Message = "Record not found"
				Response.Redirect(lblReturnUrl.Text)
			End If
			RowToControl(row, source, Core.CtrlType.Edit)
			row = Nothing
		Else 
			Dim row As PageRolerow = New PageRolerow()
			row.RoleID = Convert.ToInt32(0) ' set default value
			row.PageID = Convert.ToInt32(0) ' set default value
			row.CompanyID = Convert.ToInt32(0) ' set default value
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

	Protected Sub PageRoleDetailsView_Init(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			PageRoledal.OpenConnection()
		Catch err As Exception
			lblMessage.Text = err.Message
			pnlMessage.Visible = True
		End Try
	End Sub

	' *************************************
	' *  Handler for DetailsView DataBound
	' *************************************

	Protected Sub PageRoleDetailsView_DataBound(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim source As DetailsView = PageRoleDetailsView
		If (PageRoleDetailsView.CurrentMode = DetailsViewMode.Insert) Then
			LoadData()
		End If
	End Sub

	' *****************************************
	' *  Handler for DetailsView ItemInserting
	' *****************************************

	Protected Sub PageRoleDetailsView_ItemInserting(ByVal sender As Object, ByVal e As DetailsViewInsertEventArgs)
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

	Protected Sub PageRoleDetailsView_ItemInserted(ByVal sender As Object, ByVal e As DetailsViewInsertedEventArgs)
		If (e.AffectedRows = 0) Then

			' Keep in insert mode
			e.KeepInInsertMode = True

			' Get original values
			Dim source As WebControl = TryCast(sender, WebControl)
			If (source Is Nothing) Then Return
			oldrow = New PageRolerow()
			ControlToRow(oldrow, source)
		Else
			objProfile.Message = "Add new record successful"
			Response.Redirect(lblReturnUrl.Text)
		End If
	End Sub

	' *********************************
	' *  Handler for DetailsView Unload
	' *********************************

	Protected Sub PageRoleDetailsView_Unload(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			PageRoledal.CloseAndDisposeConnection()
		Catch
		End Try
	End Sub

	' ********************************
	' * Register ClientID of Controls
	' ********************************

	Private Sub RegisterClientID(ByVal sBlockName As String, ByVal control As Control)
		Dim jsString As String = String.Empty
		If (control.FindControl("x_RoleID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_RoleID'] = '" & control.FindControl("x_RoleID").ClientID & "';"
		End If
		If (control.FindControl("x_PageID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_PageID'] = '" & control.FindControl("x_PageID").ClientID & "';"
		End If
		If (control.FindControl("x_CompanyID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_CompanyID'] = '" & control.FindControl("x_CompanyID").ClientID & "';"
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
			Dim x_RoleID As TextBox = TryCast(control.FindControl("x_RoleID"), TextBox)
			If (x_RoleID IsNot Nothing) Then
				If (Not DataFormat.CheckInt32(x_RoleID.Text)) Then
					messageList.Add("Invalid Value (Int32): Role ID")
				End If
			End If
			Dim x_PageID As TextBox = TryCast(control.FindControl("x_PageID"), TextBox)
			If (x_PageID IsNot Nothing) Then
				If (Not DataFormat.CheckInt32(x_PageID.Text)) Then
					messageList.Add("Invalid Value (Int32): Page ID")
				End If
			End If
			Dim x_CompanyID As TextBox = TryCast(control.FindControl("x_CompanyID"), TextBox)
			If (x_CompanyID IsNot Nothing) Then
				If (Not DataFormat.CheckInt32(x_CompanyID.Text)) Then
					messageList.Add("Invalid Value (Int32): Company ID")
				End If
			End If
		End If
		Return IIf(messageList.Count = 0, Nothing, messageList)
	End Function

	' **********************************************
	' *  Convert Input Control Values to Row Object
	' **********************************************

	Private Sub ControlToRow(ByVal row As PageRolerow, ByVal control As WebControl)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		row.SetPageType(Core.PageType.Add) ' set page type = list for update

		' Field RoleID
		Dim x_RoleID As TextBox = TryCast(control.FindControl("x_RoleID"), TextBox)
		If (x_RoleID.Text <> String.Empty) Then row.RoleID = Convert.ToInt32(x_RoleID.Text) Else row.RoleID = CType(Nothing, Nullable(Of Int32))

		' Field PageID
		Dim x_PageID As TextBox = TryCast(control.FindControl("x_PageID"), TextBox)
		If (x_PageID.Text <> String.Empty) Then row.PageID = Convert.ToInt32(x_PageID.Text) Else row.PageID = CType(Nothing, Nullable(Of Int32))

		' Field CompanyID
		Dim x_CompanyID As TextBox = TryCast(control.FindControl("x_CompanyID"), TextBox)
		If (x_CompanyID.Text <> String.Empty) Then row.CompanyID = Convert.ToInt32(x_CompanyID.Text) Else row.CompanyID = CType(Nothing, Nullable(Of Int32))
	End Sub

	' ****************************
	' *  Check for duplicate Key
	' ****************************

	Private Function CheckDuplicateKey(ByVal control As WebControl) As ArrayList 
		Dim sWhere As String = String.Empty
		Dim messageList As ArrayList = New ArrayList()
		Dim data As PageRoledal = New PageRoledal()
		Dim newkey As PageRolekey = New PageRolekey()
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

	Private Sub RowToControl(ByVal row As PageRolerow, ByVal control As WebControl, ByVal ct As Core.CtrlType)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		If (ct = Core.CtrlType.Edit) Then ' set up edit control

			' Field RoleID
			Dim x_RoleID As TextBox = TryCast(control.FindControl("x_RoleID"), TextBox)
			If (row.RoleID.HasValue) Then x_RoleID.Text = row.RoleID.ToString() Else x_RoleID.Text = String.Empty

			' Field PageID
			Dim x_PageID As TextBox = TryCast(control.FindControl("x_PageID"), TextBox)
			If (row.PageID.HasValue) Then x_PageID.Text = row.PageID.ToString() Else x_PageID.Text = String.Empty

			' Field CompanyID
			Dim x_CompanyID As TextBox = TryCast(control.FindControl("x_CompanyID"), TextBox)
			If (row.CompanyID.HasValue) Then x_CompanyID.Text = row.CompanyID.ToString() Else x_CompanyID.Text = String.Empty
		End If
	End Sub

	' ************************************
	' *  Handler for DataSource Inserting
	' ************************************

	Protected Sub PageRoleDataSource_Inserting(ByVal sender As Object, ByVal e As ObjectDataSourceMethodEventArgs)
		Dim control As WebControl = PageRoleDetailsView

		' Set up row object
		Dim row As PageRolerow = TryCast(e.InputParameters(0), PageRolerow)
		ControlToRow(row, control)
		If (PageRolebll.Inserting(row)) Then
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

	Protected Sub PageRoleDataSource_Inserted(ByVal sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.Exception IsNot Nothing) Then 
			e.AffectedRows = 0
			e.ExceptionHandled = True
			lblMessage.Text = e.Exception.InnerException.Message
			pnlMessage.Visible = True
		Else
			newrow = TryCast(e.ReturnValue, PageRolerow) ' get new row objectinsert method
			PageRolebll.Inserted(newrow)
		End If
	End Sub

	' *******************
	' *  Get User Filter
	' *******************

	Private Function GetUserFilter() As String 
		Return PageRoleinf.GetUserFilter()
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
	<p><span class="aspnetmaker">Add to TABLE: Page Role</span></p>
	<p>
		<asp:Hyperlink id="lnkList" Text="Back to List" NavigateUrl="pagerole_list.aspx" CssClass="aspnetmaker" runat="server" />
		<asp:Label id="lblReturnUrl" Visible="False" CssClass="aspnetmaker" runat="server">pagerole_list.aspx</asp:Label>
	</p>
	<asp:ValidationSummary id="xevs_PageRole" CssClass="aspnetmaker" runat="server"
	HeaderText=""
	ShowSummary="False"
	ShowMessageBox="True"
	ForeColor="#FF0000" />
	<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
		<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
	</asp:PlaceHolder>
	<!-- Data Source -->
<asp:ObjectDataSource ID="PageRoleDataSource"
	TypeName="PMGEN.PageRoledal"
	DataObjectTypeName="PMGEN.PageRolerow"
	InsertMethod="Insert"
	OnInserting="PageRoleDataSource_Inserting"
	OnInserted="PageRoleDataSource_Inserted"
	runat="server">
</asp:ObjectDataSource>
	<asp:DetailsView ID="PageRoleDetailsView"
		DataKeyNames="PageRoleID"
		DataSourceID="PageRoleDataSource"
		DefaultMode="Insert"
		GridLines="None"
		AutoGenerateRows="False" CssClass="ewDetailTable"
		OnInit="PageRoleDetailsView_Init"
		OnDataBound="PageRoleDetailsView_DataBound"
		OnItemInserting="PageRoleDetailsView_ItemInserting"
		OnItemInserted="PageRoleDetailsView_ItemInserted"
		OnUnload="PageRoleDetailsView_Unload"
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
					<asp:Label runat="server" id="xs_RoleID">Role ID</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_RoleID" Columns="30" CssClass="aspnetmaker" runat="server" />
<ew:IntegerValidator ID="vc_RoleID" ClientValidationFunction="ew_CheckInteger" CssClass="aspnetmaker" runat="server" ControlToValidate="x_RoleID" ErrorMessage="Incorrect integer - Role ID" Display="None" ForeColor="Red" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_PageID">Page ID</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_PageID" Columns="30" CssClass="aspnetmaker" runat="server" />
<ew:IntegerValidator ID="vc_PageID" ClientValidationFunction="ew_CheckInteger" CssClass="aspnetmaker" runat="server" ControlToValidate="x_PageID" ErrorMessage="Incorrect integer - Page ID" Display="None" ForeColor="Red" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_CompanyID">Company ID</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_CompanyID" Columns="30" CssClass="aspnetmaker" runat="server" />
<ew:IntegerValidator ID="vc_CompanyID" ClientValidationFunction="ew_CheckInteger" CssClass="aspnetmaker" runat="server" ControlToValidate="x_CompanyID" ErrorMessage="Incorrect integer - Company ID" Display="None" ForeColor="Red" />
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
					<asp:HiddenField ID="k_PageRoleID" Runat="server" Value='<%# Bind("PageRoleID") %>' />
				</InsertItemTemplate>
			</asp:TemplateField>
		</Fields>
	</asp:DetailsView>
</asp:Content>
