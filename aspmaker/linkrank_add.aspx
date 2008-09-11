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
	Dim key As LinkRankkey ' record key
	Dim oldrow As LinkRankrow ' old record data input by user
	Dim newrow As LinkRankrow ' new record data input by user
	Dim objProfile As TableProfile ' table profile

	' *************************
	' *  Handler for Page Init
	' *************************

	Protected Sub Page_Init(ByVal s As Object, ByVal e As System.EventArgs)
		If Not mhUser.IsAdmin() Then
		  Session("Login_Link") = Request.RawUrl
		  Response.Redirect("/mhweb/login/login.aspx")
		End If
		objProfile = CustomProfile.GetTable(Share.ProjectName, LinkRankinf.TableVar)
	End Sub

	' *************************
	' *  Handler for Page Load
	' *************************

	Protected Sub Page_Load(ByVal s As Object, ByVal e As System.EventArgs)
		Page.ClientScript.RegisterClientScriptInclude("ewv", "ewv.js")
		Response.Cache.SetCacheability(HttpCacheability.NoCache)
		If (Not Page.IsPostBack) Then
		End If
		If (LinkRankDetailsView.Visible) Then
			RegisterClientID("CtrlID", LinkRankDetailsView)
		End If
			If (LinkRankDetailsView.FindControl("x_LinkID") IsNot Nothing) Then
				Page.Form.DefaultFocus = LinkRankDetailsView.FindControl("x_LinkID").ClientID
			End If
	End Sub

	' **********************************************
	' *  Load Copy Data & Default Data to Controls
	' **********************************************

	Private Sub LoadData()
		Dim source As WebControl = TryCast(LinkRankDetailsView, WebControl)
		If (oldrow IsNot Nothing) Then ' old record (from recovery) exists, just restore
			RowToControl(oldrow, source, Core.CtrlType.Edit)
			oldrow = Nothing
		ElseIf (key IsNot Nothing) Then ' key loaded, try to copy record
			Dim data As LinkRankdal = New LinkRankdal()
			Dim row As LinkRankrow = data.LoadRow(key, LinkRankinf.GetUserFilter())
			If (row Is Nothing) Then
				objProfile.Message = "Record not found"
				Response.Redirect(lblReturnUrl.Text)
			End If
			RowToControl(row, source, Core.CtrlType.Edit)
			row = Nothing
		Else 
			Dim row As LinkRankrow = New LinkRankrow()
			row.LinkID = Convert.ToInt32(0) ' set default value
			row.UserID = Convert.ToInt32(0) ' set default value
			row.RankNum = Convert.ToInt32(0) ' set default value
			row.CateID = Convert.ToInt32(0) ' set default value
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

	Protected Sub LinkRankDetailsView_Init(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			LinkRankdal.OpenConnection()
		Catch err As Exception
			lblMessage.Text = err.Message
			pnlMessage.Visible = True
		End Try
	End Sub

	' *************************************
	' *  Handler for DetailsView DataBound
	' *************************************

	Protected Sub LinkRankDetailsView_DataBound(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim source As DetailsView = LinkRankDetailsView
		If (LinkRankDetailsView.CurrentMode = DetailsViewMode.Insert) Then
			LoadData()
		End If
	End Sub

	' *****************************************
	' *  Handler for DetailsView ItemInserting
	' *****************************************

	Protected Sub LinkRankDetailsView_ItemInserting(ByVal sender As Object, ByVal e As DetailsViewInsertEventArgs)
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

	Protected Sub LinkRankDetailsView_ItemInserted(ByVal sender As Object, ByVal e As DetailsViewInsertedEventArgs)
		If (e.AffectedRows = 0) Then

			' Keep in insert mode
			e.KeepInInsertMode = True

			' Get original values
			Dim source As WebControl = TryCast(sender, WebControl)
			If (source Is Nothing) Then Return
			oldrow = New LinkRankrow()
			ControlToRow(oldrow, source)
		Else
			objProfile.Message = "Add new record successful"
			Response.Redirect(lblReturnUrl.Text)
		End If
	End Sub

	' *********************************
	' *  Handler for DetailsView Unload
	' *********************************

	Protected Sub LinkRankDetailsView_Unload(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			LinkRankdal.CloseAndDisposeConnection()
		Catch
		End Try
	End Sub

	' ********************************
	' * Register ClientID of Controls
	' ********************************

	Private Sub RegisterClientID(ByVal sBlockName As String, ByVal control As Control)
		Dim jsString As String = String.Empty
		If (control.FindControl("x_LinkID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_LinkID'] = '" & control.FindControl("x_LinkID").ClientID & "';"
		End If
		If (control.FindControl("x_UserID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_UserID'] = '" & control.FindControl("x_UserID").ClientID & "';"
		End If
		If (control.FindControl("x_RankNum") IsNot Nothing) Then
			jsString &= "ew.Controls['x_RankNum'] = '" & control.FindControl("x_RankNum").ClientID & "';"
		End If
		If (control.FindControl("x_CateID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_CateID'] = '" & control.FindControl("x_CateID").ClientID & "';"
		End If
		If (control.FindControl("x_Comment") IsNot Nothing) Then
			jsString &= "ew.Controls['x_Comment'] = '" & control.FindControl("x_Comment").ClientID & "';"
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
			Dim x_LinkID As TextBox = TryCast(control.FindControl("x_LinkID"), TextBox)
			If (x_LinkID IsNot Nothing) Then
				If (Not DataFormat.CheckInt32(x_LinkID.Text)) Then
					messageList.Add("Invalid Value (Int32): Link ID")
				End If
			End If
			Dim x_UserID As TextBox = TryCast(control.FindControl("x_UserID"), TextBox)
			If (x_UserID IsNot Nothing) Then
				If (Not DataFormat.CheckInt32(x_UserID.Text)) Then
					messageList.Add("Invalid Value (Int32): User ID")
				End If
			End If
			Dim x_RankNum As TextBox = TryCast(control.FindControl("x_RankNum"), TextBox)
			If (x_RankNum IsNot Nothing) Then
				If (Not DataFormat.CheckInt32(x_RankNum.Text)) Then
					messageList.Add("Invalid Value (Int32): Rank Num")
				End If
			End If
			Dim x_CateID As TextBox = TryCast(control.FindControl("x_CateID"), TextBox)
			If (x_CateID IsNot Nothing) Then
				If (Not DataFormat.CheckInt32(x_CateID.Text)) Then
					messageList.Add("Invalid Value (Int32): Cate ID")
				End If
			End If
			Dim x_Comment As TextBox = TryCast(control.FindControl("x_Comment"), TextBox)
			If (x_Comment IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_Comment.Text)) Then
					messageList.Add("Invalid Value (String): Comment")
				End If
			End If
		End If
		Return IIf(messageList.Count = 0, Nothing, messageList)
	End Function

	' **********************************************
	' *  Convert Input Control Values to Row Object
	' **********************************************

	Private Sub ControlToRow(ByVal row As LinkRankrow, ByVal control As WebControl)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		row.SetPageType(Core.PageType.Add) ' set page type = list for update

		' Field LinkID
		Dim x_LinkID As TextBox = TryCast(control.FindControl("x_LinkID"), TextBox)
		If (x_LinkID.Text <> String.Empty) Then row.LinkID = Convert.ToInt32(x_LinkID.Text) Else row.LinkID = CType(Nothing, Nullable(Of Int32))

		' Field UserID
		Dim x_UserID As TextBox = TryCast(control.FindControl("x_UserID"), TextBox)
		If (x_UserID.Text <> String.Empty) Then row.UserID = Convert.ToInt32(x_UserID.Text) Else row.UserID = CType(Nothing, Nullable(Of Int32))

		' Field RankNum
		Dim x_RankNum As TextBox = TryCast(control.FindControl("x_RankNum"), TextBox)
		If (x_RankNum.Text <> String.Empty) Then row.RankNum = Convert.ToInt32(x_RankNum.Text) Else row.RankNum = CType(Nothing, Nullable(Of Int32))

		' Field CateID
		Dim x_CateID As TextBox = TryCast(control.FindControl("x_CateID"), TextBox)
		If (x_CateID.Text <> String.Empty) Then row.CateID = Convert.ToInt32(x_CateID.Text) Else row.CateID = CType(Nothing, Nullable(Of Int32))

		' Field Comment
		Dim x_Comment As TextBox = TryCast(control.FindControl("x_Comment"), TextBox)
		If (x_Comment.Text <> String.Empty) Then row.Comment = x_Comment.Text Else row.Comment = Nothing
	End Sub

	' ****************************
	' *  Check for duplicate Key
	' ****************************

	Private Function CheckDuplicateKey(ByVal control As WebControl) As ArrayList 
		Dim sWhere As String = String.Empty
		Dim messageList As ArrayList = New ArrayList()
		Dim data As LinkRankdal = New LinkRankdal()
		Dim newkey As LinkRankkey = New LinkRankkey()
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

	Private Sub RowToControl(ByVal row As LinkRankrow, ByVal control As WebControl, ByVal ct As Core.CtrlType)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		If (ct = Core.CtrlType.Edit) Then ' set up edit control

			' Field LinkID
			Dim x_LinkID As TextBox = TryCast(control.FindControl("x_LinkID"), TextBox)
			If (row.LinkID.HasValue) Then x_LinkID.Text = row.LinkID.ToString() Else x_LinkID.Text = String.Empty

			' Field UserID
			Dim x_UserID As TextBox = TryCast(control.FindControl("x_UserID"), TextBox)
			If (row.UserID.HasValue) Then x_UserID.Text = row.UserID.ToString() Else x_UserID.Text = String.Empty

			' Field RankNum
			Dim x_RankNum As TextBox = TryCast(control.FindControl("x_RankNum"), TextBox)
			If (row.RankNum.HasValue) Then x_RankNum.Text = row.RankNum.ToString() Else x_RankNum.Text = String.Empty

			' Field CateID
			Dim x_CateID As TextBox = TryCast(control.FindControl("x_CateID"), TextBox)
			If (row.CateID.HasValue) Then x_CateID.Text = row.CateID.ToString() Else x_CateID.Text = String.Empty

			' Field Comment
			Dim x_Comment As TextBox = TryCast(control.FindControl("x_Comment"), TextBox)
			If (row.Comment IsNot Nothing) Then x_Comment.Text = row.Comment.ToString() Else x_Comment.Text = String.Empty
		End If
	End Sub

	' ************************************
	' *  Handler for DataSource Inserting
	' ************************************

	Protected Sub LinkRankDataSource_Inserting(ByVal sender As Object, ByVal e As ObjectDataSourceMethodEventArgs)
		Dim control As WebControl = LinkRankDetailsView

		' Set up row object
		Dim row As LinkRankrow = TryCast(e.InputParameters(0), LinkRankrow)
		ControlToRow(row, control)
		If (LinkRankbll.Inserting(row)) Then
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

	Protected Sub LinkRankDataSource_Inserted(ByVal sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.Exception IsNot Nothing) Then 
			e.AffectedRows = 0
			e.ExceptionHandled = True
			lblMessage.Text = e.Exception.InnerException.Message
			pnlMessage.Visible = True
		Else
			newrow = TryCast(e.ReturnValue, LinkRankrow) ' get new row objectinsert method
			LinkRankbll.Inserted(newrow)
		End If
	End Sub

	' *******************
	' *  Get User Filter
	' *******************

	Private Function GetUserFilter() As String 
		Return LinkRankinf.GetUserFilter()
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
	<p><span class="aspnetmaker">Add to TABLE: Link Rank</span></p>
	<p>
		<asp:Hyperlink id="lnkList" Text="Back to List" NavigateUrl="linkrank_list.aspx" CssClass="aspnetmaker" runat="server" />
		<asp:Label id="lblReturnUrl" Visible="False" CssClass="aspnetmaker" runat="server">linkrank_list.aspx</asp:Label>
	</p>
	<asp:ValidationSummary id="xevs_LinkRank" CssClass="aspnetmaker" runat="server"
	HeaderText=""
	ShowSummary="False"
	ShowMessageBox="True"
	ForeColor="#FF0000" />
	<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
		<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
	</asp:PlaceHolder>
	<!-- Data Source -->
<asp:ObjectDataSource ID="LinkRankDataSource"
	TypeName="PMGEN.LinkRankdal"
	DataObjectTypeName="PMGEN.LinkRankrow"
	InsertMethod="Insert"
	OnInserting="LinkRankDataSource_Inserting"
	OnInserted="LinkRankDataSource_Inserted"
	runat="server">
</asp:ObjectDataSource>
	<asp:DetailsView ID="LinkRankDetailsView"
		DataKeyNames="ID"
		DataSourceID="LinkRankDataSource"
		DefaultMode="Insert"
		GridLines="None"
		AutoGenerateRows="False" CssClass="ewDetailTable"
		OnInit="LinkRankDetailsView_Init"
		OnDataBound="LinkRankDetailsView_DataBound"
		OnItemInserting="LinkRankDetailsView_ItemInserting"
		OnItemInserted="LinkRankDetailsView_ItemInserted"
		OnUnload="LinkRankDetailsView_Unload"
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
					<asp:Label runat="server" id="xs_LinkID">Link ID</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_LinkID" Columns="30" CssClass="aspnetmaker" runat="server" />
<ew:IntegerValidator ID="vc_LinkID" ClientValidationFunction="ew_CheckInteger" CssClass="aspnetmaker" runat="server" ControlToValidate="x_LinkID" ErrorMessage="Incorrect integer - Link ID" Display="None" ForeColor="Red" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_UserID">User ID</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_UserID" Columns="30" CssClass="aspnetmaker" runat="server" />
<ew:IntegerValidator ID="vc_UserID" ClientValidationFunction="ew_CheckInteger" CssClass="aspnetmaker" runat="server" ControlToValidate="x_UserID" ErrorMessage="Incorrect integer - User ID" Display="None" ForeColor="Red" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_RankNum">Rank Num</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_RankNum" Columns="30" CssClass="aspnetmaker" runat="server" />
<ew:IntegerValidator ID="vc_RankNum" ClientValidationFunction="ew_CheckInteger" CssClass="aspnetmaker" runat="server" ControlToValidate="x_RankNum" ErrorMessage="Incorrect integer - Rank Num" Display="None" ForeColor="Red" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_CateID">Cate ID</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_CateID" Columns="30" CssClass="aspnetmaker" runat="server" />
<ew:IntegerValidator ID="vc_CateID" ClientValidationFunction="ew_CheckInteger" CssClass="aspnetmaker" runat="server" ControlToValidate="x_CateID" ErrorMessage="Incorrect integer - Cate ID" Display="None" ForeColor="Red" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_Comment">Comment</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_Comment" Columns="30" MaxLength="255" CssClass="aspnetmaker" runat="server" />
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
					<asp:HiddenField ID="k_ID" Runat="server" Value='<%# Bind("ID") %>' />
				</InsertItemTemplate>
			</asp:TemplateField>
		</Fields>
	</asp:DetailsView>
</asp:Content>
