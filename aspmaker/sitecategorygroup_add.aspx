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
	Dim key As SiteCategoryGroupkey ' record key
	Dim oldrow As SiteCategoryGrouprow ' old record data input by user
	Dim newrow As SiteCategoryGrouprow ' new record data input by user
	Dim objProfile As TableProfile ' table profile

	' *************************
	' *  Handler for Page Init
	' *************************

	Protected Sub Page_Init(ByVal s As Object, ByVal e As System.EventArgs)
		If Not mhUser.IsAdmin() Then
		  Session("Login_Link") = Request.RawUrl
		  Response.Redirect("/mhweb/login/login.aspx")
		End If
		objProfile = CustomProfile.GetTable(Share.ProjectName, SiteCategoryGroupinf.TableVar)
	End Sub

	' *************************
	' *  Handler for Page Load
	' *************************

	Protected Sub Page_Load(ByVal s As Object, ByVal e As System.EventArgs)
		Page.ClientScript.RegisterClientScriptInclude("ewv", "ewv.js")
		Response.Cache.SetCacheability(HttpCacheability.NoCache)
		If (Not Page.IsPostBack) Then
			If (Request.QueryString.Count > 0) Then
				key = New SiteCategoryGroupkey()
				Dim messageList As ArrayList = SiteCategoryGroupinf.LoadKey(key)
				If (messageList IsNot Nothing) Then key = Nothing ' error, ignore
			End If
		End If
		If (SiteCategoryGroupDetailsView.Visible) Then
			RegisterClientID("CtrlID", SiteCategoryGroupDetailsView)
		End If
			If (SiteCategoryGroupDetailsView.FindControl("x_SiteCategoryGroupNM") IsNot Nothing) Then
				Page.Form.DefaultFocus = SiteCategoryGroupDetailsView.FindControl("x_SiteCategoryGroupNM").ClientID
			End If
	End Sub

	' **********************************************
	' *  Load Copy Data & Default Data to Controls
	' **********************************************

	Private Sub LoadData()
		Dim source As WebControl = TryCast(SiteCategoryGroupDetailsView, WebControl)
		If (oldrow IsNot Nothing) Then ' old record (from recovery) exists, just restore
			RowToControl(oldrow, source, Core.CtrlType.Edit)
			oldrow = Nothing
		ElseIf (key IsNot Nothing) Then ' key loaded, try to copy record
			Dim data As SiteCategoryGroupdal = New SiteCategoryGroupdal()
			Dim row As SiteCategoryGrouprow = data.LoadRow(key, SiteCategoryGroupinf.GetUserFilter())
			If (row Is Nothing) Then
				objProfile.Message = "Record not found"
				Response.Redirect(lblReturnUrl.Text)
			End If
			RowToControl(row, source, Core.CtrlType.Edit)
			row = Nothing
		Else 
			Dim row As SiteCategoryGrouprow = New SiteCategoryGrouprow()
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

	Protected Sub SiteCategoryGroupDetailsView_Init(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			SiteCategoryGroupdal.OpenConnection()
		Catch err As Exception
			lblMessage.Text = err.Message
			pnlMessage.Visible = True
		End Try
	End Sub

	' *************************************
	' *  Handler for DetailsView DataBound
	' *************************************

	Protected Sub SiteCategoryGroupDetailsView_DataBound(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim source As DetailsView = SiteCategoryGroupDetailsView
		If (SiteCategoryGroupDetailsView.CurrentMode = DetailsViewMode.Insert) Then
			LoadData()
		End If
	End Sub

	' *****************************************
	' *  Handler for DetailsView ItemInserting
	' *****************************************

	Protected Sub SiteCategoryGroupDetailsView_ItemInserting(ByVal sender As Object, ByVal e As DetailsViewInsertEventArgs)
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

	Protected Sub SiteCategoryGroupDetailsView_ItemInserted(ByVal sender As Object, ByVal e As DetailsViewInsertedEventArgs)
		If (e.AffectedRows = 0) Then

			' Keep in insert mode
			e.KeepInInsertMode = True

			' Get original values
			Dim source As WebControl = TryCast(sender, WebControl)
			If (source Is Nothing) Then Return
			oldrow = New SiteCategoryGrouprow()
			ControlToRow(oldrow, source)
		Else
			objProfile.Message = "Add new record successful"
			Response.Redirect(lblReturnUrl.Text)
		End If
	End Sub

	' *********************************
	' *  Handler for DetailsView Unload
	' *********************************

	Protected Sub SiteCategoryGroupDetailsView_Unload(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			SiteCategoryGroupdal.CloseAndDisposeConnection()
		Catch
		End Try
	End Sub

	' ********************************
	' * Register ClientID of Controls
	' ********************************

	Private Sub RegisterClientID(ByVal sBlockName As String, ByVal control As Control)
		Dim jsString As String = String.Empty
		If (control.FindControl("x_SiteCategoryGroupNM") IsNot Nothing) Then
			jsString &= "ew.Controls['x_SiteCategoryGroupNM'] = '" & control.FindControl("x_SiteCategoryGroupNM").ClientID & "';"
		End If
		If (control.FindControl("x_SiteCategoryGroupDS") IsNot Nothing) Then
			jsString &= "ew.Controls['x_SiteCategoryGroupDS'] = '" & control.FindControl("x_SiteCategoryGroupDS").ClientID & "';"
		End If
		If (control.FindControl("x_SiteCategoryGroupOrder") IsNot Nothing) Then
			jsString &= "ew.Controls['x_SiteCategoryGroupOrder'] = '" & control.FindControl("x_SiteCategoryGroupOrder").ClientID & "';"
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
			Dim x_SiteCategoryGroupNM As TextBox = TryCast(control.FindControl("x_SiteCategoryGroupNM"), TextBox)
			If (x_SiteCategoryGroupNM IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_SiteCategoryGroupNM.Text)) Then
					messageList.Add("Invalid Value (String): Site Category Group NM")
				End If
			End If
			Dim x_SiteCategoryGroupDS As TextBox = TryCast(control.FindControl("x_SiteCategoryGroupDS"), TextBox)
			If (x_SiteCategoryGroupDS IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_SiteCategoryGroupDS.Text)) Then
					messageList.Add("Invalid Value (String): Site Category Group DS")
				End If
			End If
			Dim x_SiteCategoryGroupOrder As TextBox = TryCast(control.FindControl("x_SiteCategoryGroupOrder"), TextBox)
			If (x_SiteCategoryGroupOrder IsNot Nothing) Then
				If (Not DataFormat.CheckInt32(x_SiteCategoryGroupOrder.Text)) Then
					messageList.Add("Invalid Value (Int32): Site Category Group Order")
				End If
			End If
		End If
		Return IIf(messageList.Count = 0, Nothing, messageList)
	End Function

	' **********************************************
	' *  Convert Input Control Values to Row Object
	' **********************************************

	Private Sub ControlToRow(ByVal row As SiteCategoryGrouprow, ByVal control As WebControl)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		row.SetPageType(Core.PageType.Add) ' set page type = list for update

		' Field SiteCategoryGroupNM
		Dim x_SiteCategoryGroupNM As TextBox = TryCast(control.FindControl("x_SiteCategoryGroupNM"), TextBox)
		If (x_SiteCategoryGroupNM.Text <> String.Empty) Then row.SiteCategoryGroupNM = x_SiteCategoryGroupNM.Text Else row.SiteCategoryGroupNM = Nothing

		' Field SiteCategoryGroupDS
		Dim x_SiteCategoryGroupDS As TextBox = TryCast(control.FindControl("x_SiteCategoryGroupDS"), TextBox)
		If (x_SiteCategoryGroupDS.Text <> String.Empty) Then row.SiteCategoryGroupDS = x_SiteCategoryGroupDS.Text Else row.SiteCategoryGroupDS = Nothing

		' Field SiteCategoryGroupOrder
		Dim x_SiteCategoryGroupOrder As TextBox = TryCast(control.FindControl("x_SiteCategoryGroupOrder"), TextBox)
		If (x_SiteCategoryGroupOrder.Text <> String.Empty) Then row.SiteCategoryGroupOrder = Convert.ToInt32(x_SiteCategoryGroupOrder.Text) Else row.SiteCategoryGroupOrder = CType(Nothing, Nullable(Of Int32))
	End Sub

	' ****************************
	' *  Check for duplicate Key
	' ****************************

	Private Function CheckDuplicateKey(ByVal control As WebControl) As ArrayList 
		Dim sWhere As String = String.Empty
		Dim messageList As ArrayList = New ArrayList()
		Dim data As SiteCategoryGroupdal = New SiteCategoryGroupdal()
		Dim newkey As SiteCategoryGroupkey = New SiteCategoryGroupkey()
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

	Private Sub RowToControl(ByVal row As SiteCategoryGrouprow, ByVal control As WebControl, ByVal ct As Core.CtrlType)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		If (ct = Core.CtrlType.Edit) Then ' set up edit control

			' Field SiteCategoryGroupNM
			Dim x_SiteCategoryGroupNM As TextBox = TryCast(control.FindControl("x_SiteCategoryGroupNM"), TextBox)
			If (row.SiteCategoryGroupNM IsNot Nothing) Then x_SiteCategoryGroupNM.Text = row.SiteCategoryGroupNM.ToString() Else x_SiteCategoryGroupNM.Text = String.Empty

			' Field SiteCategoryGroupDS
			Dim x_SiteCategoryGroupDS As TextBox = TryCast(control.FindControl("x_SiteCategoryGroupDS"), TextBox)
			If (row.SiteCategoryGroupDS IsNot Nothing) Then x_SiteCategoryGroupDS.Text = row.SiteCategoryGroupDS.ToString() Else x_SiteCategoryGroupDS.Text = String.Empty

			' Field SiteCategoryGroupOrder
			Dim x_SiteCategoryGroupOrder As TextBox = TryCast(control.FindControl("x_SiteCategoryGroupOrder"), TextBox)
			If (row.SiteCategoryGroupOrder.HasValue) Then x_SiteCategoryGroupOrder.Text = row.SiteCategoryGroupOrder.ToString() Else x_SiteCategoryGroupOrder.Text = String.Empty
		End If
	End Sub

	' ************************************
	' *  Handler for DataSource Inserting
	' ************************************

	Protected Sub SiteCategoryGroupDataSource_Inserting(ByVal sender As Object, ByVal e As ObjectDataSourceMethodEventArgs)
		Dim control As WebControl = SiteCategoryGroupDetailsView

		' Set up row object
		Dim row As SiteCategoryGrouprow = TryCast(e.InputParameters(0), SiteCategoryGrouprow)
		ControlToRow(row, control)
		If (SiteCategoryGroupbll.Inserting(row)) Then
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

	Protected Sub SiteCategoryGroupDataSource_Inserted(ByVal sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.Exception IsNot Nothing) Then 
			e.AffectedRows = 0
			e.ExceptionHandled = True
			lblMessage.Text = e.Exception.InnerException.Message
			pnlMessage.Visible = True
		Else
			newrow = TryCast(e.ReturnValue, SiteCategoryGrouprow) ' get new row objectinsert method
			SiteCategoryGroupbll.Inserted(newrow)
		End If
	End Sub

	' *******************
	' *  Get User Filter
	' *******************

	Private Function GetUserFilter() As String 
		Return SiteCategoryGroupinf.GetUserFilter()
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
	<p><span class="aspnetmaker">Add to TABLE: Site Category Group</span></p>
	<p>
		<asp:Hyperlink id="lnkList" Text="Back to List" NavigateUrl="sitecategorygroup_list.aspx" CssClass="aspnetmaker" runat="server" />
		<asp:Label id="lblReturnUrl" Visible="False" CssClass="aspnetmaker" runat="server">sitecategorygroup_list.aspx</asp:Label>
	</p>
	<asp:ValidationSummary id="xevs_SiteCategoryGroup" CssClass="aspnetmaker" runat="server"
	HeaderText=""
	ShowSummary="False"
	ShowMessageBox="True"
	ForeColor="#FF0000" />
	<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
		<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
	</asp:PlaceHolder>
	<!-- Data Source -->
<asp:ObjectDataSource ID="SiteCategoryGroupDataSource"
	TypeName="PMGEN.SiteCategoryGroupdal"
	DataObjectTypeName="PMGEN.SiteCategoryGrouprow"
	InsertMethod="Insert"
	OnInserting="SiteCategoryGroupDataSource_Inserting"
	OnInserted="SiteCategoryGroupDataSource_Inserted"
	runat="server">
</asp:ObjectDataSource>
	<asp:DetailsView ID="SiteCategoryGroupDetailsView"
		DataKeyNames="SiteCategoryGroupID"
		DataSourceID="SiteCategoryGroupDataSource"
		DefaultMode="Insert"
		GridLines="None"
		AutoGenerateRows="False" CssClass="ewDetailTable"
		OnInit="SiteCategoryGroupDetailsView_Init"
		OnDataBound="SiteCategoryGroupDetailsView_DataBound"
		OnItemInserting="SiteCategoryGroupDetailsView_ItemInserting"
		OnItemInserted="SiteCategoryGroupDetailsView_ItemInserted"
		OnUnload="SiteCategoryGroupDetailsView_Unload"
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
					<asp:Label runat="server" id="xs_SiteCategoryGroupNM">Site Category Group NM</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_SiteCategoryGroupNM" Columns="30" MaxLength="255" CssClass="aspnetmaker" runat="server" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_SiteCategoryGroupDS">Site Category Group DS</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_SiteCategoryGroupDS" Columns="30" MaxLength="255" CssClass="aspnetmaker" runat="server" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_SiteCategoryGroupOrder">Site Category Group Order</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_SiteCategoryGroupOrder" Columns="30" CssClass="aspnetmaker" runat="server" />
<ew:IntegerValidator ID="vc_SiteCategoryGroupOrder" ClientValidationFunction="ew_CheckInteger" CssClass="aspnetmaker" runat="server" ControlToValidate="x_SiteCategoryGroupOrder" ErrorMessage="Incorrect integer - Site Category Group Order" Display="None" ForeColor="Red" />
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
					<asp:HiddenField ID="k_SiteCategoryGroupID" Runat="server" Value='<%# Bind("SiteCategoryGroupID") %>' />
				</InsertItemTemplate>
			</asp:TemplateField>
		</Fields>
	</asp:DetailsView>
</asp:Content>
