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
	Dim key As SiteParameterTypekey ' record key
	Dim oldrow As SiteParameterTyperow ' old record data input by user
	Dim newrow As SiteParameterTyperow ' new record data input by user
	Dim objProfile As TableProfile ' table profile

	' *************************
	' *  Handler for Page Init
	' *************************

	Protected Sub Page_Init(ByVal s As Object, ByVal e As System.EventArgs)
		If Not mhUser.IsAdmin() Then
		  Session("Login_Link") = Request.RawUrl
		  Response.Redirect("/mhweb/login/login.aspx")
		End If
		objProfile = CustomProfile.GetTable(Share.ProjectName, SiteParameterTypeinf.TableVar)
	End Sub

	' *************************
	' *  Handler for Page Load
	' *************************

	Protected Sub Page_Load(ByVal s As Object, ByVal e As System.EventArgs)
		Page.ClientScript.RegisterClientScriptInclude("ewv", "ewv.js")
		Response.Cache.SetCacheability(HttpCacheability.NoCache)
		If (Not Page.IsPostBack) Then
		End If
		If (SiteParameterTypeDetailsView.Visible) Then
			RegisterClientID("CtrlID", SiteParameterTypeDetailsView)
		End If
			If (SiteParameterTypeDetailsView.FindControl("x_SiteParameterTypeNM") IsNot Nothing) Then
				Page.Form.DefaultFocus = SiteParameterTypeDetailsView.FindControl("x_SiteParameterTypeNM").ClientID
			End If
	End Sub

	' **********************************************
	' *  Load Copy Data & Default Data to Controls
	' **********************************************

	Private Sub LoadData()
		Dim source As WebControl = TryCast(SiteParameterTypeDetailsView, WebControl)
		If (oldrow IsNot Nothing) Then ' old record (from recovery) exists, just restore
			RowToControl(oldrow, source, Core.CtrlType.Edit)
			oldrow = Nothing
		ElseIf (key IsNot Nothing) Then ' key loaded, try to copy record
			Dim data As SiteParameterTypedal = New SiteParameterTypedal()
			Dim row As SiteParameterTyperow = data.LoadRow(key, SiteParameterTypeinf.GetUserFilter())
			If (row Is Nothing) Then
				objProfile.Message = "Record not found"
				Response.Redirect(lblReturnUrl.Text)
			End If
			RowToControl(row, source, Core.CtrlType.Edit)
			row = Nothing
		Else 
			Dim row As SiteParameterTyperow = New SiteParameterTyperow()
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

	Protected Sub SiteParameterTypeDetailsView_Init(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			SiteParameterTypedal.OpenConnection()
		Catch err As Exception
			lblMessage.Text = err.Message
			pnlMessage.Visible = True
		End Try
	End Sub

	' *************************************
	' *  Handler for DetailsView DataBound
	' *************************************

	Protected Sub SiteParameterTypeDetailsView_DataBound(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim source As DetailsView = SiteParameterTypeDetailsView
		If (SiteParameterTypeDetailsView.CurrentMode = DetailsViewMode.Insert) Then
			LoadData()
		End If
	End Sub

	' *****************************************
	' *  Handler for DetailsView ItemInserting
	' *****************************************

	Protected Sub SiteParameterTypeDetailsView_ItemInserting(ByVal sender As Object, ByVal e As DetailsViewInsertEventArgs)
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

	Protected Sub SiteParameterTypeDetailsView_ItemInserted(ByVal sender As Object, ByVal e As DetailsViewInsertedEventArgs)
		If (e.AffectedRows = 0) Then

			' Keep in insert mode
			e.KeepInInsertMode = True

			' Get original values
			Dim source As WebControl = TryCast(sender, WebControl)
			If (source Is Nothing) Then Return
			oldrow = New SiteParameterTyperow()
			ControlToRow(oldrow, source)
		Else
			objProfile.Message = "Add new record successful"
			Response.Redirect(lblReturnUrl.Text)
		End If
	End Sub

	' *********************************
	' *  Handler for DetailsView Unload
	' *********************************

	Protected Sub SiteParameterTypeDetailsView_Unload(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			SiteParameterTypedal.CloseAndDisposeConnection()
		Catch
		End Try
	End Sub

	' ********************************
	' * Register ClientID of Controls
	' ********************************

	Private Sub RegisterClientID(ByVal sBlockName As String, ByVal control As Control)
		Dim jsString As String = String.Empty
		If (control.FindControl("x_SiteParameterTypeNM") IsNot Nothing) Then
			jsString &= "ew.Controls['x_SiteParameterTypeNM'] = '" & control.FindControl("x_SiteParameterTypeNM").ClientID & "';"
		End If
		If (control.FindControl("x_SiteParameterTypeDS") IsNot Nothing) Then
			jsString &= "ew.Controls['x_SiteParameterTypeDS'] = '" & control.FindControl("x_SiteParameterTypeDS").ClientID & "';"
		End If
		If (control.FindControl("x_SiteParameterTypeOrder") IsNot Nothing) Then
			jsString &= "ew.Controls['x_SiteParameterTypeOrder'] = '" & control.FindControl("x_SiteParameterTypeOrder").ClientID & "';"
		End If
		If (control.FindControl("x_SiteParameterTemplate") IsNot Nothing) Then
			jsString &= "ew.Controls['x_SiteParameterTemplate'] = '" & control.FindControl("x_SiteParameterTemplate").ClientID & "';"
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
			Dim x_SiteParameterTypeNM As TextBox = TryCast(control.FindControl("x_SiteParameterTypeNM"), TextBox)
			If (x_SiteParameterTypeNM IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_SiteParameterTypeNM.Text)) Then
					messageList.Add("Invalid Value (String): Parameter Type Name")
				End If
			End If
			Dim x_SiteParameterTypeDS As TextBox = TryCast(control.FindControl("x_SiteParameterTypeDS"), TextBox)
			If (x_SiteParameterTypeDS IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_SiteParameterTypeDS.Text)) Then
					messageList.Add("Invalid Value (String): Description")
				End If
			End If
			Dim x_SiteParameterTypeOrder As TextBox = TryCast(control.FindControl("x_SiteParameterTypeOrder"), TextBox)
			If (x_SiteParameterTypeOrder IsNot Nothing) Then
				If (Not DataFormat.CheckInt32(x_SiteParameterTypeOrder.Text)) Then
					messageList.Add("Invalid Value (Int32): Order")
				End If
			End If
			Dim x_SiteParameterTemplate As TextBox = TryCast(control.FindControl("x_SiteParameterTemplate"), TextBox)
			If (x_SiteParameterTemplate IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_SiteParameterTemplate.Text)) Then
					messageList.Add("Invalid Value (String): Template")
				End If
			End If
		End If
		Return IIf(messageList.Count = 0, Nothing, messageList)
	End Function

	' **********************************************
	' *  Convert Input Control Values to Row Object
	' **********************************************

	Private Sub ControlToRow(ByVal row As SiteParameterTyperow, ByVal control As WebControl)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		row.SetPageType(Core.PageType.Add) ' set page type = list for update

		' Field SiteParameterTypeNM
		Dim x_SiteParameterTypeNM As TextBox = TryCast(control.FindControl("x_SiteParameterTypeNM"), TextBox)
		If (x_SiteParameterTypeNM.Text <> String.Empty) Then row.SiteParameterTypeNM = x_SiteParameterTypeNM.Text Else row.SiteParameterTypeNM = Nothing

		' Field SiteParameterTypeDS
		Dim x_SiteParameterTypeDS As TextBox = TryCast(control.FindControl("x_SiteParameterTypeDS"), TextBox)
		If (x_SiteParameterTypeDS.Text <> String.Empty) Then row.SiteParameterTypeDS = x_SiteParameterTypeDS.Text Else row.SiteParameterTypeDS = Nothing

		' Field SiteParameterTypeOrder
		Dim x_SiteParameterTypeOrder As TextBox = TryCast(control.FindControl("x_SiteParameterTypeOrder"), TextBox)
		If (x_SiteParameterTypeOrder.Text <> String.Empty) Then row.SiteParameterTypeOrder = Convert.ToInt32(x_SiteParameterTypeOrder.Text) Else row.SiteParameterTypeOrder = CType(Nothing, Nullable(Of Int32))

		' Field SiteParameterTemplate
		Dim x_SiteParameterTemplate As TextBox = TryCast(control.FindControl("x_SiteParameterTemplate"), TextBox)
		If (x_SiteParameterTemplate.Text <> String.Empty) Then row.SiteParameterTemplate = x_SiteParameterTemplate.Text Else row.SiteParameterTemplate = Nothing
	End Sub

	' ****************************
	' *  Check for duplicate Key
	' ****************************

	Private Function CheckDuplicateKey(ByVal control As WebControl) As ArrayList 
		Dim sWhere As String = String.Empty
		Dim messageList As ArrayList = New ArrayList()
		Dim data As SiteParameterTypedal = New SiteParameterTypedal()
		Dim newkey As SiteParameterTypekey = New SiteParameterTypekey()
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

	Private Sub RowToControl(ByVal row As SiteParameterTyperow, ByVal control As WebControl, ByVal ct As Core.CtrlType)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		If (ct = Core.CtrlType.Edit) Then ' set up edit control

			' Field SiteParameterTypeNM
			Dim x_SiteParameterTypeNM As TextBox = TryCast(control.FindControl("x_SiteParameterTypeNM"), TextBox)
			If (row.SiteParameterTypeNM IsNot Nothing) Then x_SiteParameterTypeNM.Text = row.SiteParameterTypeNM.ToString() Else x_SiteParameterTypeNM.Text = String.Empty

			' Field SiteParameterTypeDS
			Dim x_SiteParameterTypeDS As TextBox = TryCast(control.FindControl("x_SiteParameterTypeDS"), TextBox)
			If (row.SiteParameterTypeDS IsNot Nothing) Then x_SiteParameterTypeDS.Text = row.SiteParameterTypeDS.ToString() Else x_SiteParameterTypeDS.Text = String.Empty

			' Field SiteParameterTypeOrder
			Dim x_SiteParameterTypeOrder As TextBox = TryCast(control.FindControl("x_SiteParameterTypeOrder"), TextBox)
			If (row.SiteParameterTypeOrder.HasValue) Then x_SiteParameterTypeOrder.Text = row.SiteParameterTypeOrder.ToString() Else x_SiteParameterTypeOrder.Text = String.Empty

			' Field SiteParameterTemplate
			Dim x_SiteParameterTemplate As TextBox = TryCast(control.FindControl("x_SiteParameterTemplate"), TextBox)
			If (row.SiteParameterTemplate IsNot Nothing) Then x_SiteParameterTemplate.Text = row.SiteParameterTemplate.ToString() Else x_SiteParameterTemplate.Text = String.Empty
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

	Protected Sub SiteParameterTypeDataSource_Inserting(ByVal sender As Object, ByVal e As ObjectDataSourceMethodEventArgs)
		Dim control As WebControl = SiteParameterTypeDetailsView

		' Set up row object
		Dim row As SiteParameterTyperow = TryCast(e.InputParameters(0), SiteParameterTyperow)
		ControlToRow(row, control)
		If (SiteParameterTypebll.Inserting(row)) Then
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

	Protected Sub SiteParameterTypeDataSource_Inserted(ByVal sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.Exception IsNot Nothing) Then 
			e.AffectedRows = 0
			e.ExceptionHandled = True
			lblMessage.Text = e.Exception.InnerException.Message
			pnlMessage.Visible = True
		Else
			newrow = TryCast(e.ReturnValue, SiteParameterTyperow) ' get new row objectinsert method
			SiteParameterTypebll.Inserted(newrow)
		End If
	End Sub

	' *******************
	' *  Get User Filter
	' *******************

	Private Function GetUserFilter() As String 
		Return SiteParameterTypeinf.GetUserFilter()
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
	<p><span class="aspnetmaker">Add to TABLE: Site Parameter Type</span></p>
	<p>
		<asp:Hyperlink id="lnkList" Text="Back to List" NavigateUrl="siteparametertype_list.aspx" CssClass="aspnetmaker" runat="server" />
		<asp:Label id="lblReturnUrl" Visible="False" CssClass="aspnetmaker" runat="server">siteparametertype_list.aspx</asp:Label>
	</p>
	<asp:ValidationSummary id="xevs_SiteParameterType" CssClass="aspnetmaker" runat="server"
	HeaderText=""
	ShowSummary="False"
	ShowMessageBox="True"
	ForeColor="#FF0000" />
	<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
		<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
	</asp:PlaceHolder>
	<!-- Data Source -->
<asp:ObjectDataSource ID="SiteParameterTypeDataSource"
	TypeName="PMGEN.SiteParameterTypedal"
	DataObjectTypeName="PMGEN.SiteParameterTyperow"
	InsertMethod="Insert"
	OnInserting="SiteParameterTypeDataSource_Inserting"
	OnInserted="SiteParameterTypeDataSource_Inserted"
	runat="server">
</asp:ObjectDataSource>
	<asp:DetailsView ID="SiteParameterTypeDetailsView"
		DataKeyNames="SiteParameterTypeID"
		DataSourceID="SiteParameterTypeDataSource"
		DefaultMode="Insert"
		GridLines="None"
		AutoGenerateRows="False" CssClass="ewDetailTable"
		OnInit="SiteParameterTypeDetailsView_Init"
		OnDataBound="SiteParameterTypeDetailsView_DataBound"
		OnItemInserting="SiteParameterTypeDetailsView_ItemInserting"
		OnItemInserted="SiteParameterTypeDetailsView_ItemInserted"
		OnUnload="SiteParameterTypeDetailsView_Unload"
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
					<asp:Label runat="server" id="xs_SiteParameterTypeNM">Parameter Type Name</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_SiteParameterTypeNM" Columns="30" MaxLength="255" CssClass="aspnetmaker" runat="server" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_SiteParameterTypeDS">Description</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_SiteParameterTypeDS" Columns="30" MaxLength="255" CssClass="aspnetmaker" runat="server" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_SiteParameterTypeOrder">Order</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_SiteParameterTypeOrder" Columns="30" CssClass="aspnetmaker" runat="server" />
<ew:IntegerValidator ID="vc_SiteParameterTypeOrder" ClientValidationFunction="ew_CheckInteger" CssClass="aspnetmaker" runat="server" ControlToValidate="x_SiteParameterTypeOrder" ErrorMessage="Incorrect integer - Order" Display="None" ForeColor="Red" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_SiteParameterTemplate">Template</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_SiteParameterTemplate" TextMode="MultiLine" Rows="4" Columns="35" CssClass="aspnetmaker" runat="server" />
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
					<asp:HiddenField ID="k_SiteParameterTypeID" Runat="server" Value='<%# Bind("SiteParameterTypeID") %>' />
				</InsertItemTemplate>
			</asp:TemplateField>
		</Fields>
	</asp:DetailsView>
</asp:Content>