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
		Page.ClientScript.RegisterClientScriptInclude("ewv", "ewv.js")
		Response.Cache.SetCacheability(HttpCacheability.NoCache)
		If (Not Page.IsPostBack) Then
		End If
		If (SiteCategoryTypeDetailsView.Visible) Then
			RegisterClientID("CtrlID", SiteCategoryTypeDetailsView)
		End If
			If (SiteCategoryTypeDetailsView.FindControl("x_SiteCategoryTypeNM") IsNot Nothing) Then
				Page.Form.DefaultFocus = SiteCategoryTypeDetailsView.FindControl("x_SiteCategoryTypeNM").ClientID
			End If
	End Sub

	' **********************************************
	' *  Load Copy Data & Default Data to Controls
	' **********************************************

	Private Sub LoadData()
		Dim source As WebControl = TryCast(SiteCategoryTypeDetailsView, WebControl)
		If (oldrow IsNot Nothing) Then ' old record (from recovery) exists, just restore
			RowToControl(oldrow, source, Core.CtrlType.Edit)
			oldrow = Nothing
		ElseIf (key IsNot Nothing) Then ' key loaded, try to copy record
			Dim data As SiteCategoryTypedal = New SiteCategoryTypedal()
			Dim row As SiteCategoryTyperow = data.LoadRow(key, SiteCategoryTypeinf.GetUserFilter())
			If (row Is Nothing) Then
				objProfile.Message = "Record not found"
				Response.Redirect(lblReturnUrl.Text)
			End If
			RowToControl(row, source, Core.CtrlType.Edit)
			row = Nothing
		Else 
			Dim row As SiteCategoryTyperow = New SiteCategoryTyperow()
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
		If (SiteCategoryTypeDetailsView.CurrentMode = DetailsViewMode.Insert) Then
			LoadData()
		End If
	End Sub

	' *****************************************
	' *  Handler for DetailsView ItemInserting
	' *****************************************

	Protected Sub SiteCategoryTypeDetailsView_ItemInserting(ByVal sender As Object, ByVal e As DetailsViewInsertEventArgs)
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

	Protected Sub SiteCategoryTypeDetailsView_ItemInserted(ByVal sender As Object, ByVal e As DetailsViewInsertedEventArgs)
		If (e.AffectedRows = 0) Then

			' Keep in insert mode
			e.KeepInInsertMode = True

			' Get original values
			Dim source As WebControl = TryCast(sender, WebControl)
			If (source Is Nothing) Then Return
			oldrow = New SiteCategoryTyperow()
			ControlToRow(oldrow, source)
		Else
			objProfile.Message = "Add new record successful"
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
		row.SetPageType(Core.PageType.Add) ' set page type = list for update

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
	' *  Handler for DataSource Inserting
	' ************************************

	Protected Sub SiteCategoryTypeDataSource_Inserting(ByVal sender As Object, ByVal e As ObjectDataSourceMethodEventArgs)
		Dim control As WebControl = SiteCategoryTypeDetailsView

		' Set up row object
		Dim row As SiteCategoryTyperow = TryCast(e.InputParameters(0), SiteCategoryTyperow)
		ControlToRow(row, control)
		If (SiteCategoryTypebll.Inserting(row)) Then
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

	Protected Sub SiteCategoryTypeDataSource_Inserted(ByVal sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.Exception IsNot Nothing) Then 
			e.AffectedRows = 0
			e.ExceptionHandled = True
			lblMessage.Text = e.Exception.InnerException.Message
			pnlMessage.Visible = True
		Else
			newrow = TryCast(e.ReturnValue, SiteCategoryTyperow) ' get new row objectinsert method
			SiteCategoryTypebll.Inserted(newrow)
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
	<p><span class="aspnetmaker">Add to TABLE: Site Type</span></p>
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
	InsertMethod="Insert"
	OnInserting="SiteCategoryTypeDataSource_Inserting"
	OnInserted="SiteCategoryTypeDataSource_Inserted"
	runat="server">
</asp:ObjectDataSource>
	<asp:DetailsView ID="SiteCategoryTypeDetailsView"
		DataKeyNames="SiteCategoryTypeID"
		DataSourceID="SiteCategoryTypeDataSource"
		DefaultMode="Insert"
		GridLines="None"
		AutoGenerateRows="False" CssClass="ewDetailTable"
		OnInit="SiteCategoryTypeDetailsView_Init"
		OnDataBound="SiteCategoryTypeDetailsView_DataBound"
		OnItemInserting="SiteCategoryTypeDetailsView_ItemInserting"
		OnItemInserted="SiteCategoryTypeDetailsView_ItemInserted"
		OnUnload="SiteCategoryTypeDetailsView_Unload"
		AllowPaging="True"
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
					<asp:Label runat="server" id="xs_SiteCategoryTypeNM">Site Category Type NM</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_SiteCategoryTypeNM" Columns="30" MaxLength="255" CssClass="aspnetmaker" runat="server" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_SiteCategoryTypeDS">Site Category Type DS</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_SiteCategoryTypeDS" Columns="30" MaxLength="255" CssClass="aspnetmaker" runat="server" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_SiteCategoryComment">Site Category Comment</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_SiteCategoryComment" Columns="30" MaxLength="255" CssClass="aspnetmaker" runat="server" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_SiteCategoryFileName">Site Category File Name</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_SiteCategoryFileName" Columns="30" MaxLength="255" CssClass="aspnetmaker" runat="server" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_SiteCategoryTransferURL">Site Category Transfer URL</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_SiteCategoryTransferURL" Columns="30" MaxLength="255" CssClass="aspnetmaker" runat="server" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_DefaultSiteCategoryID">Default Site Category ID</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:DropDownList ID="x_DefaultSiteCategoryID" CssClass="aspnetmaker" runat="server" >
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
					<asp:HiddenField ID="k_SiteCategoryTypeID" Runat="server" Value='<%# Bind("SiteCategoryTypeID") %>' />
				</InsertItemTemplate>
			</asp:TemplateField>
		</Fields>
	</asp:DetailsView>
</asp:Content>
