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
	Dim key As SiteTemplatekey ' record key
	Dim oldrow As SiteTemplaterow ' old record data input by user
	Dim newrow As SiteTemplaterow ' new record data input by user
	Dim objProfile As TableProfile ' table profile

	' *************************
	' *  Handler for Page Init
	' *************************

	Protected Sub Page_Init(ByVal s As Object, ByVal e As System.EventArgs)
		If Not mhUser.IsAdmin() Then
		  Session("Login_Link") = Request.RawUrl
		  Response.Redirect("/mhweb/login/login.aspx")
		End If
		objProfile = CustomProfile.GetTable(Share.ProjectName, SiteTemplateinf.TableVar)
	End Sub

	' *************************
	' *  Handler for Page Load
	' *************************

	Protected Sub Page_Load(ByVal s As Object, ByVal e As System.EventArgs)
		Page.ClientScript.RegisterClientScriptInclude("ewv", "ewv.js")
		Response.Cache.SetCacheability(HttpCacheability.NoCache)
		If (Not Page.IsPostBack) Then
			If (Request.QueryString.Count > 0) Then
				key = New SiteTemplatekey()
				Dim messageList As ArrayList = SiteTemplateinf.LoadKey(key)
				If (messageList IsNot Nothing) Then key = Nothing ' error, ignore
			End If
		End If
		If (SiteTemplateDetailsView.Visible) Then
			RegisterClientID("CtrlID", SiteTemplateDetailsView)
		End If
			If (SiteTemplateDetailsView.FindControl("x_TemplatePrefix") IsNot Nothing) Then
				Page.Form.DefaultFocus = SiteTemplateDetailsView.FindControl("x_TemplatePrefix").ClientID
			End If
	End Sub

	' **********************************************
	' *  Load Copy Data & Default Data to Controls
	' **********************************************

	Private Sub LoadData()
		Dim source As WebControl = TryCast(SiteTemplateDetailsView, WebControl)
		If (oldrow IsNot Nothing) Then ' old record (from recovery) exists, just restore
			RowToControl(oldrow, source, Core.CtrlType.Edit)
			oldrow = Nothing
		ElseIf (key IsNot Nothing) Then ' key loaded, try to copy record
			Dim data As SiteTemplatedal = New SiteTemplatedal()
			Dim row As SiteTemplaterow = data.LoadRow(key, SiteTemplateinf.GetUserFilter())
			If (row Is Nothing) Then
				objProfile.Message = "Record not found"
				Response.Redirect(lblReturnUrl.Text)
			End If
			RowToControl(row, source, Core.CtrlType.Edit)
			row = Nothing
		Else 
			Dim row As SiteTemplaterow = New SiteTemplaterow()
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

	Protected Sub SiteTemplateDetailsView_Init(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			SiteTemplatedal.OpenConnection()
		Catch err As Exception
			lblMessage.Text = err.Message
			pnlMessage.Visible = True
		End Try
	End Sub

	' *************************************
	' *  Handler for DetailsView DataBound
	' *************************************

	Protected Sub SiteTemplateDetailsView_DataBound(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim source As DetailsView = SiteTemplateDetailsView
		If (SiteTemplateDetailsView.CurrentMode = DetailsViewMode.Insert) Then
			LoadData()
		End If
	End Sub

	' *****************************************
	' *  Handler for DetailsView ItemInserting
	' *****************************************

	Protected Sub SiteTemplateDetailsView_ItemInserting(ByVal sender As Object, ByVal e As DetailsViewInsertEventArgs)
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

	Protected Sub SiteTemplateDetailsView_ItemInserted(ByVal sender As Object, ByVal e As DetailsViewInsertedEventArgs)
		If (e.AffectedRows = 0) Then

			' Keep in insert mode
			e.KeepInInsertMode = True

			' Get original values
			Dim source As WebControl = TryCast(sender, WebControl)
			If (source Is Nothing) Then Return
			oldrow = New SiteTemplaterow()
			ControlToRow(oldrow, source)
		Else
			objProfile.Message = "Add new record successful"
			Response.Redirect(lblReturnUrl.Text)
		End If
	End Sub

	' *********************************
	' *  Handler for DetailsView Unload
	' *********************************

	Protected Sub SiteTemplateDetailsView_Unload(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			SiteTemplatedal.CloseAndDisposeConnection()
		Catch
		End Try
	End Sub

	' ********************************
	' * Register ClientID of Controls
	' ********************************

	Private Sub RegisterClientID(ByVal sBlockName As String, ByVal control As Control)
		Dim jsString As String = String.Empty
		If (control.FindControl("x_TemplatePrefix") IsNot Nothing) Then
			jsString &= "ew.Controls['x_TemplatePrefix'] = '" & control.FindControl("x_TemplatePrefix").ClientID & "';"
		End If
		If (control.FindControl("x_Name") IsNot Nothing) Then
			jsString &= "ew.Controls['x_Name'] = '" & control.FindControl("x_Name").ClientID & "';"
		End If
		If (control.FindControl("x_Top") IsNot Nothing) Then
			jsString &= "ew.Controls['x_Top'] = '" & control.FindControl("x_Top").ClientID & "';"
		End If
		If (control.FindControl("x_Bottom") IsNot Nothing) Then
			jsString &= "ew.Controls['x_Bottom'] = '" & control.FindControl("x_Bottom").ClientID & "';"
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
			Dim x_TemplatePrefix As TextBox = TryCast(control.FindControl("x_TemplatePrefix"), TextBox)
			If (x_TemplatePrefix IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_TemplatePrefix.Text)) Then
					messageList.Add("Invalid Value (String): Code")
				ElseIf (String.IsNullOrEmpty(x_TemplatePrefix.Text)) Then
					messageList.Add("Please enter required field (String): Code")
				End If
			End If
			Dim x_Name As TextBox = TryCast(control.FindControl("x_Name"), TextBox)
			If (x_Name IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_Name.Text)) Then
					messageList.Add("Invalid Value (String): Name")
				ElseIf (String.IsNullOrEmpty(x_Name.Text)) Then
					messageList.Add("Please enter required field (String): Name")
				End If
			End If
			Dim x_Top As TextBox = TryCast(control.FindControl("x_Top"), TextBox)
			If (x_Top IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_Top.Text)) Then
					messageList.Add("Invalid Value (String): Top")
				End If
			End If
			Dim x_Bottom As TextBox = TryCast(control.FindControl("x_Bottom"), TextBox)
			If (x_Bottom IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_Bottom.Text)) Then
					messageList.Add("Invalid Value (String): Bottom")
				End If
			End If
		End If
		Return IIf(messageList.Count = 0, Nothing, messageList)
	End Function

	' **********************************************
	' *  Convert Input Control Values to Row Object
	' **********************************************

	Private Sub ControlToRow(ByVal row As SiteTemplaterow, ByVal control As WebControl)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		row.SetPageType(Core.PageType.Add) ' set page type = list for update

		' Field TemplatePrefix
		Dim x_TemplatePrefix As TextBox = TryCast(control.FindControl("x_TemplatePrefix"), TextBox)
		If (x_TemplatePrefix.Text <> String.Empty) Then row.TemplatePrefix = x_TemplatePrefix.Text Else row.TemplatePrefix = String.Empty

		' Field Name
		Dim x_Name As TextBox = TryCast(control.FindControl("x_Name"), TextBox)
		If (x_Name.Text <> String.Empty) Then row.Name = x_Name.Text Else row.Name = String.Empty

		' Field Top
		Dim x_Top As TextBox = TryCast(control.FindControl("x_Top"), TextBox)
		If (x_Top.Text <> String.Empty) Then row.Top = x_Top.Text Else row.Top = Nothing

		' Field Bottom
		Dim x_Bottom As TextBox = TryCast(control.FindControl("x_Bottom"), TextBox)
		If (x_Bottom.Text <> String.Empty) Then row.Bottom = x_Bottom.Text Else row.Bottom = Nothing
	End Sub

	' ****************************
	' *  Check for duplicate Key
	' ****************************

	Private Function CheckDuplicateKey(ByVal control As WebControl) As ArrayList 
		Dim sWhere As String = String.Empty
		Dim messageList As ArrayList = New ArrayList()
		Dim data As SiteTemplatedal = New SiteTemplatedal()
		Dim newkey As SiteTemplatekey = New SiteTemplatekey()
		Try
		Dim x_TemplatePrefix As TextBox = TryCast(control.FindControl("x_TemplatePrefix"), TextBox)
		If (x_TemplatePrefix Is Nothing) Then
			Return Nothing
		Else
			newkey.TemplatePrefix = Convert.ToString(x_TemplatePrefix.Text)
		End If
		Dim row As SiteTemplaterow = data.LoadRow(newkey, String.Empty) ' no need to filter
		If (row IsNot Nothing) Then messageList.Add("Duplicate value for primary key")

		' Check for duplicate Code
			sWhere = "([TemplatePrefix] = '" + Db.AdjustSql((CType(control.FindControl("x_TemplatePrefix"),TextBox)).Text) + "')"
			If (data.LoadList(sWhere) IsNot Nothing) Then
				messageList.Add("Duplicate value: Code")
			End If

		' Check for duplicate Name
			sWhere = "([Name] = '" + Db.AdjustSql((CType(control.FindControl("x_Name"),TextBox)).Text) + "')"
			If (data.LoadList(sWhere) IsNot Nothing) Then
				messageList.Add("Duplicate value: Name")
			End If
		row = Nothing
		data = Nothing
		Catch e As Exception
			messageList.Add(e.Message)
		End Try
		Return IIf(messageList.Count = 0, Nothing, messageList)
	End Function

	' ********************************************
	' *  Convert Row Object Value To Input Control
	' ********************************************

	Private Sub RowToControl(ByVal row As SiteTemplaterow, ByVal control As WebControl, ByVal ct As Core.CtrlType)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		If (ct = Core.CtrlType.Edit) Then ' set up edit control

			' Field TemplatePrefix
			Dim x_TemplatePrefix As TextBox = TryCast(control.FindControl("x_TemplatePrefix"), TextBox)
			If (row.TemplatePrefix IsNot Nothing) Then x_TemplatePrefix.Text = row.TemplatePrefix.ToString() Else x_TemplatePrefix.Text = String.Empty

			' Field Name
			Dim x_Name As TextBox = TryCast(control.FindControl("x_Name"), TextBox)
			If (row.Name IsNot Nothing) Then x_Name.Text = row.Name.ToString() Else x_Name.Text = String.Empty

			' Field Top
			Dim x_Top As TextBox = TryCast(control.FindControl("x_Top"), TextBox)
			If (row.Top IsNot Nothing) Then x_Top.Text = row.Top.ToString() Else x_Top.Text = String.Empty

			' Field Bottom
			Dim x_Bottom As TextBox = TryCast(control.FindControl("x_Bottom"), TextBox)
			If (row.Bottom IsNot Nothing) Then x_Bottom.Text = row.Bottom.ToString() Else x_Bottom.Text = String.Empty
		End If
	End Sub

	' ************************************
	' *  Handler for DataSource Inserting
	' ************************************

	Protected Sub SiteTemplateDataSource_Inserting(ByVal sender As Object, ByVal e As ObjectDataSourceMethodEventArgs)
		Dim control As WebControl = SiteTemplateDetailsView

		' Set up row object
		Dim row As SiteTemplaterow = TryCast(e.InputParameters(0), SiteTemplaterow)
		ControlToRow(row, control)
		If (SiteTemplatebll.Inserting(row)) Then
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

	Protected Sub SiteTemplateDataSource_Inserted(ByVal sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.Exception IsNot Nothing) Then 
			e.AffectedRows = 0
			e.ExceptionHandled = True
			lblMessage.Text = e.Exception.InnerException.Message
			pnlMessage.Visible = True
		Else
			newrow = TryCast(e.ReturnValue, SiteTemplaterow) ' get new row objectinsert method
			SiteTemplatebll.Inserted(newrow)
		End If
	End Sub

	' *******************
	' *  Get User Filter
	' *******************

	Private Function GetUserFilter() As String 
		Return SiteTemplateinf.GetUserFilter()
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
	<p><span class="aspnetmaker">Add to TABLE: Site Template</span></p>
	<p>
		<asp:Hyperlink id="lnkList" Text="Back to List" NavigateUrl="sitetemplate_list.aspx" CssClass="aspnetmaker" runat="server" />
		<asp:Label id="lblReturnUrl" Visible="False" CssClass="aspnetmaker" runat="server">sitetemplate_list.aspx</asp:Label>
	</p>
	<asp:ValidationSummary id="xevs_SiteTemplate" CssClass="aspnetmaker" runat="server"
	HeaderText=""
	ShowSummary="False"
	ShowMessageBox="True"
	ForeColor="#FF0000" />
	<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
		<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
	</asp:PlaceHolder>
	<!-- Data Source -->
<asp:ObjectDataSource ID="SiteTemplateDataSource"
	TypeName="PMGEN.SiteTemplatedal"
	DataObjectTypeName="PMGEN.SiteTemplaterow"
	InsertMethod="Insert"
	OnInserting="SiteTemplateDataSource_Inserting"
	OnInserted="SiteTemplateDataSource_Inserted"
	runat="server">
</asp:ObjectDataSource>
	<asp:DetailsView ID="SiteTemplateDetailsView"
		DataKeyNames="TemplatePrefix"
		DataSourceID="SiteTemplateDataSource"
		DefaultMode="Insert"
		GridLines="None"
		AutoGenerateRows="False" CssClass="ewDetailTable"
		OnInit="SiteTemplateDetailsView_Init"
		OnDataBound="SiteTemplateDetailsView_DataBound"
		OnItemInserting="SiteTemplateDetailsView_ItemInserting"
		OnItemInserted="SiteTemplateDetailsView_ItemInserted"
		OnUnload="SiteTemplateDetailsView_Unload"
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
					<asp:Label runat="server" id="xs_TemplatePrefix">Code<span class='ewmsg'>&nbsp;*</span></asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_TemplatePrefix" Columns="30" MaxLength="10" CssClass="aspnetmaker" runat="server" />
<asp:RequiredFieldValidator ID="vx_TemplatePrefix" CssClass="aspnetmaker" runat="server" ControlToValidate="x_TemplatePrefix" ErrorMessage="Please enter required field - Code" ForeColor="Red" Display="None" Text="*" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_Name">Name<span class='ewmsg'>&nbsp;*</span></asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_Name" Columns="30" MaxLength="50" CssClass="aspnetmaker" runat="server" />
<asp:RequiredFieldValidator ID="vx_Name" CssClass="aspnetmaker" runat="server" ControlToValidate="x_Name" ErrorMessage="Please enter required field - Name" ForeColor="Red" Display="None" Text="*" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_Top">Top</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_Top" TextMode="MultiLine" Rows="20" Columns="80" CssClass="aspnetmaker" runat="server" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_Bottom">Bottom</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_Bottom" TextMode="MultiLine" Rows="20" Columns="80" CssClass="aspnetmaker" runat="server" />
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
					<asp:HiddenField ID="k_TemplatePrefix" Runat="server" Value='<%# Bind("TemplatePrefix") %>' />
				</InsertItemTemplate>
			</asp:TemplateField>
		</Fields>
	</asp:DetailsView>
</asp:Content>
