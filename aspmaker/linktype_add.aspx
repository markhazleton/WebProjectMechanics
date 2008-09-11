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
	Dim key As LinkTypekey ' record key
	Dim oldrow As LinkTyperow ' old record data input by user
	Dim newrow As LinkTyperow ' new record data input by user
	Dim objProfile As TableProfile ' table profile

	' *************************
	' *  Handler for Page Init
	' *************************

	Protected Sub Page_Init(ByVal s As Object, ByVal e As System.EventArgs)
		If Not mhUser.IsAdmin() Then
		  Session("Login_Link") = Request.RawUrl
		  Response.Redirect("/mhweb/login/login.aspx")
		End If
		objProfile = CustomProfile.GetTable(Share.ProjectName, LinkTypeinf.TableVar)
	End Sub

	' *************************
	' *  Handler for Page Load
	' *************************

	Protected Sub Page_Load(ByVal s As Object, ByVal e As System.EventArgs)
		Page.ClientScript.RegisterClientScriptInclude("ewv", "ewv.js")
		Response.Cache.SetCacheability(HttpCacheability.NoCache)
		If (Not Page.IsPostBack) Then
		End If
		If (LinkTypeDetailsView.Visible) Then
			RegisterClientID("CtrlID", LinkTypeDetailsView)
		End If
			If (LinkTypeDetailsView.FindControl("x_LinkTypeCD") IsNot Nothing) Then
				Page.Form.DefaultFocus = LinkTypeDetailsView.FindControl("x_LinkTypeCD").ClientID
			End If
	End Sub

	' **********************************************
	' *  Load Copy Data & Default Data to Controls
	' **********************************************

	Private Sub LoadData()
		Dim source As WebControl = TryCast(LinkTypeDetailsView, WebControl)
		If (oldrow IsNot Nothing) Then ' old record (from recovery) exists, just restore
			RowToControl(oldrow, source, Core.CtrlType.Edit)
			oldrow = Nothing
		ElseIf (key IsNot Nothing) Then ' key loaded, try to copy record
			Dim data As LinkTypedal = New LinkTypedal()
			Dim row As LinkTyperow = data.LoadRow(key, LinkTypeinf.GetUserFilter())
			If (row Is Nothing) Then
				objProfile.Message = "Record not found"
				Response.Redirect(lblReturnUrl.Text)
			End If
			RowToControl(row, source, Core.CtrlType.Edit)
			row = Nothing
		Else 
			Dim row As LinkTyperow = New LinkTyperow()
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

	Protected Sub LinkTypeDetailsView_Init(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			LinkTypedal.OpenConnection()
		Catch err As Exception
			lblMessage.Text = err.Message
			pnlMessage.Visible = True
		End Try
	End Sub

	' *************************************
	' *  Handler for DetailsView DataBound
	' *************************************

	Protected Sub LinkTypeDetailsView_DataBound(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim source As DetailsView = LinkTypeDetailsView
		If (LinkTypeDetailsView.CurrentMode = DetailsViewMode.Insert) Then
			LoadData()
		End If
	End Sub

	' *****************************************
	' *  Handler for DetailsView ItemInserting
	' *****************************************

	Protected Sub LinkTypeDetailsView_ItemInserting(ByVal sender As Object, ByVal e As DetailsViewInsertEventArgs)
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

	Protected Sub LinkTypeDetailsView_ItemInserted(ByVal sender As Object, ByVal e As DetailsViewInsertedEventArgs)
		If (e.AffectedRows = 0) Then

			' Keep in insert mode
			e.KeepInInsertMode = True

			' Get original values
			Dim source As WebControl = TryCast(sender, WebControl)
			If (source Is Nothing) Then Return
			oldrow = New LinkTyperow()
			ControlToRow(oldrow, source)
		Else
			objProfile.Message = "Add new record successful"
			Response.Redirect(lblReturnUrl.Text)
		End If
	End Sub

	' *********************************
	' *  Handler for DetailsView Unload
	' *********************************

	Protected Sub LinkTypeDetailsView_Unload(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			LinkTypedal.CloseAndDisposeConnection()
		Catch
		End Try
	End Sub

	' ********************************
	' * Register ClientID of Controls
	' ********************************

	Private Sub RegisterClientID(ByVal sBlockName As String, ByVal control As Control)
		Dim jsString As String = String.Empty
		If (control.FindControl("x_LinkTypeCD") IsNot Nothing) Then
			jsString &= "ew.Controls['x_LinkTypeCD'] = '" & control.FindControl("x_LinkTypeCD").ClientID & "';"
		End If
		If (control.FindControl("x_LinkTypeDesc") IsNot Nothing) Then
			jsString &= "ew.Controls['x_LinkTypeDesc'] = '" & control.FindControl("x_LinkTypeDesc").ClientID & "';"
		End If
		If (control.FindControl("x_LinkTypeComment") IsNot Nothing) Then
			jsString &= "ew.Controls['x_LinkTypeComment'] = '" & control.FindControl("x_LinkTypeComment").ClientID & "';"
		End If
		If (control.FindControl("x_LinkTypeTarget") IsNot Nothing) Then
			jsString &= "ew.Controls['x_LinkTypeTarget'] = '" & control.FindControl("x_LinkTypeTarget").ClientID & "';"
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
			Dim x_LinkTypeCD As TextBox = TryCast(control.FindControl("x_LinkTypeCD"), TextBox)
			If (x_LinkTypeCD IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_LinkTypeCD.Text)) Then
					messageList.Add("Invalid Value (String): Link Type CD")
				ElseIf (String.IsNullOrEmpty(x_LinkTypeCD.Text)) Then
					messageList.Add("Please enter required field (String): Link Type CD")
				End If
			End If
			Dim x_LinkTypeDesc As TextBox = TryCast(control.FindControl("x_LinkTypeDesc"), TextBox)
			If (x_LinkTypeDesc IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_LinkTypeDesc.Text)) Then
					messageList.Add("Invalid Value (String): Link Type Desc")
				End If
			End If
			Dim x_LinkTypeComment As TextBox = TryCast(control.FindControl("x_LinkTypeComment"), TextBox)
			If (x_LinkTypeComment IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_LinkTypeComment.Text)) Then
					messageList.Add("Invalid Value (String): Link Type Comment")
				End If
			End If
			Dim x_LinkTypeTarget As TextBox = TryCast(control.FindControl("x_LinkTypeTarget"), TextBox)
			If (x_LinkTypeTarget IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_LinkTypeTarget.Text)) Then
					messageList.Add("Invalid Value (String): Link Type Target")
				End If
			End If
		End If
		Return IIf(messageList.Count = 0, Nothing, messageList)
	End Function

	' **********************************************
	' *  Convert Input Control Values to Row Object
	' **********************************************

	Private Sub ControlToRow(ByVal row As LinkTyperow, ByVal control As WebControl)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		row.SetPageType(Core.PageType.Add) ' set page type = list for update

		' Field LinkTypeCD
		Dim x_LinkTypeCD As TextBox = TryCast(control.FindControl("x_LinkTypeCD"), TextBox)
		If (x_LinkTypeCD.Text <> String.Empty) Then row.LinkTypeCD = x_LinkTypeCD.Text Else row.LinkTypeCD = Nothing

		' Field LinkTypeDesc
		Dim x_LinkTypeDesc As TextBox = TryCast(control.FindControl("x_LinkTypeDesc"), TextBox)
		If (x_LinkTypeDesc.Text <> String.Empty) Then row.LinkTypeDesc = x_LinkTypeDesc.Text Else row.LinkTypeDesc = Nothing

		' Field LinkTypeComment
		Dim x_LinkTypeComment As TextBox = TryCast(control.FindControl("x_LinkTypeComment"), TextBox)
		If (x_LinkTypeComment.Text <> String.Empty) Then row.LinkTypeComment = x_LinkTypeComment.Text Else row.LinkTypeComment = Nothing

		' Field LinkTypeTarget
		Dim x_LinkTypeTarget As TextBox = TryCast(control.FindControl("x_LinkTypeTarget"), TextBox)
		If (x_LinkTypeTarget.Text <> String.Empty) Then row.LinkTypeTarget = x_LinkTypeTarget.Text Else row.LinkTypeTarget = Nothing
	End Sub

	' ****************************
	' *  Check for duplicate Key
	' ****************************

	Private Function CheckDuplicateKey(ByVal control As WebControl) As ArrayList 
		Dim sWhere As String = String.Empty
		Dim messageList As ArrayList = New ArrayList()
		Dim data As LinkTypedal = New LinkTypedal()
		Dim newkey As LinkTypekey = New LinkTypekey()
		Try
		Dim x_LinkTypeCD As TextBox = TryCast(control.FindControl("x_LinkTypeCD"), TextBox)
		If (x_LinkTypeCD Is Nothing) Then
			Return Nothing
		Else
			newkey.LinkTypeCD = Convert.ToString(x_LinkTypeCD.Text)
		End If
		Dim row As LinkTyperow = data.LoadRow(newkey, String.Empty) ' no need to filter
		If (row IsNot Nothing) Then messageList.Add("Duplicate value for primary key")

		' Check for duplicate Link Type CD
			sWhere = "([LinkTypeCD] = '" + Db.AdjustSql((CType(control.FindControl("x_LinkTypeCD"),TextBox)).Text) + "')"
			If (data.LoadList(sWhere) IsNot Nothing) Then
				messageList.Add("Duplicate value: Link Type CD")
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

	Private Sub RowToControl(ByVal row As LinkTyperow, ByVal control As WebControl, ByVal ct As Core.CtrlType)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		If (ct = Core.CtrlType.Edit) Then ' set up edit control

			' Field LinkTypeCD
			Dim x_LinkTypeCD As TextBox = TryCast(control.FindControl("x_LinkTypeCD"), TextBox)
			If (row.LinkTypeCD IsNot Nothing) Then x_LinkTypeCD.Text = row.LinkTypeCD.ToString() Else x_LinkTypeCD.Text = String.Empty

			' Field LinkTypeDesc
			Dim x_LinkTypeDesc As TextBox = TryCast(control.FindControl("x_LinkTypeDesc"), TextBox)
			If (row.LinkTypeDesc IsNot Nothing) Then x_LinkTypeDesc.Text = row.LinkTypeDesc.ToString() Else x_LinkTypeDesc.Text = String.Empty

			' Field LinkTypeComment
			Dim x_LinkTypeComment As TextBox = TryCast(control.FindControl("x_LinkTypeComment"), TextBox)
			If (row.LinkTypeComment IsNot Nothing) Then x_LinkTypeComment.Text = row.LinkTypeComment.ToString() Else x_LinkTypeComment.Text = String.Empty

			' Field LinkTypeTarget
			Dim x_LinkTypeTarget As TextBox = TryCast(control.FindControl("x_LinkTypeTarget"), TextBox)
			If (row.LinkTypeTarget IsNot Nothing) Then x_LinkTypeTarget.Text = row.LinkTypeTarget.ToString() Else x_LinkTypeTarget.Text = String.Empty
		End If
	End Sub

	' ************************************
	' *  Handler for DataSource Inserting
	' ************************************

	Protected Sub LinkTypeDataSource_Inserting(ByVal sender As Object, ByVal e As ObjectDataSourceMethodEventArgs)
		Dim control As WebControl = LinkTypeDetailsView

		' Set up row object
		Dim row As LinkTyperow = TryCast(e.InputParameters(0), LinkTyperow)
		ControlToRow(row, control)
		If (LinkTypebll.Inserting(row)) Then
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

	Protected Sub LinkTypeDataSource_Inserted(ByVal sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.Exception IsNot Nothing) Then 
			e.AffectedRows = 0
			e.ExceptionHandled = True
			lblMessage.Text = e.Exception.InnerException.Message
			pnlMessage.Visible = True
		Else
			newrow = TryCast(e.ReturnValue, LinkTyperow) ' get new row objectinsert method
			LinkTypebll.Inserted(newrow)
		End If
	End Sub

	' *******************
	' *  Get User Filter
	' *******************

	Private Function GetUserFilter() As String 
		Return LinkTypeinf.GetUserFilter()
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
	<p><span class="aspnetmaker">Add to TABLE: Link Type</span></p>
	<p>
		<asp:Hyperlink id="lnkList" Text="Back to List" NavigateUrl="linktype_list.aspx" CssClass="aspnetmaker" runat="server" />
		<asp:Label id="lblReturnUrl" Visible="False" CssClass="aspnetmaker" runat="server">linktype_list.aspx</asp:Label>
	</p>
	<asp:ValidationSummary id="xevs_LinkType" CssClass="aspnetmaker" runat="server"
	HeaderText=""
	ShowSummary="False"
	ShowMessageBox="True"
	ForeColor="#FF0000" />
	<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
		<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
	</asp:PlaceHolder>
	<!-- Data Source -->
<asp:ObjectDataSource ID="LinkTypeDataSource"
	TypeName="PMGEN.LinkTypedal"
	DataObjectTypeName="PMGEN.LinkTyperow"
	InsertMethod="Insert"
	OnInserting="LinkTypeDataSource_Inserting"
	OnInserted="LinkTypeDataSource_Inserted"
	runat="server">
</asp:ObjectDataSource>
	<asp:DetailsView ID="LinkTypeDetailsView"
		DataKeyNames="LinkTypeCD"
		DataSourceID="LinkTypeDataSource"
		DefaultMode="Insert"
		GridLines="None"
		AutoGenerateRows="False" CssClass="ewDetailTable"
		OnInit="LinkTypeDetailsView_Init"
		OnDataBound="LinkTypeDetailsView_DataBound"
		OnItemInserting="LinkTypeDetailsView_ItemInserting"
		OnItemInserted="LinkTypeDetailsView_ItemInserted"
		OnUnload="LinkTypeDetailsView_Unload"
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
					<asp:Label runat="server" id="xs_LinkTypeCD">Link Type CD</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_LinkTypeCD" Columns="30" MaxLength="50" CssClass="aspnetmaker" runat="server" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_LinkTypeDesc">Link Type Desc</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_LinkTypeDesc" Columns="30" MaxLength="255" CssClass="aspnetmaker" runat="server" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_LinkTypeComment">Link Type Comment</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_LinkTypeComment" TextMode="MultiLine" Rows="4" Columns="35" CssClass="aspnetmaker" runat="server" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_LinkTypeTarget">Link Type Target</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_LinkTypeTarget" Columns="30" MaxLength="50" CssClass="aspnetmaker" runat="server" />
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
					<asp:HiddenField ID="k_LinkTypeCD" Runat="server" Value='<%# Bind("LinkTypeCD") %>' />
				</InsertItemTemplate>
			</asp:TemplateField>
		</Fields>
	</asp:DetailsView>
</asp:Content>
