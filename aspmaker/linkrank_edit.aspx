<%@ Page EnableViewState="true" Language="VB" ValidateRequest="False" Debug="True" MasterPageFile="masterpage.master" %>
<%@ MasterType VirtualPath="masterpage.master" %>
<%@ Import Namespace="PMGEN" %>
<%@ Import Namespace="EW.Data" %>
<%@ Import Namespace="EW.Data.Utilities" %>
<%@ Import Namespace="EW.Web" %>
<%@ Register NameSpace="PMGEN" TagPrefix="PMGEN" %>
<script runat="server">
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
		If Not mhUser.IsAdmin() Then
		  Session("Login_Link") = Request.RawUrl
                  Response.Redirect("/mhweb/login/login.aspx")
		End If
		Page.ClientScript.RegisterClientScriptInclude("ewv", "ewv.js")
		Response.Cache.SetCacheability(HttpCacheability.NoCache)
		If (Not Page.IsPostBack) Then
			lblReturnURL.Text = Session("ListPageURL")
			If (Request.QueryString.Count > 0) Then
				key = New LinkRankkey()
				Dim messageList As ArrayList = LinkRankinf.LoadKey(key) 
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
			key = TryCast(ViewState("key"), LinkRankkey) ' restore from ViewState for postback
		End If
		If (LinkRankDetailsView.Visible) Then
			RegisterClientID("CtrlID", LinkRankDetailsView)
		End If
		If (LinkRankDetailsView.FindControl("x_LinkID") IsNot Nothing) Then
			Page.Form.DefaultFocus = LinkRankDetailsView.FindControl("x_LinkID").ClientID
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
		Dim row As LinkRankrow = TryCast(LinkRankDetailsView.DataItem, LinkRankrow) ' get data object
		If (LinkRankDetailsView.CurrentMode = DetailsViewMode.Edit) Then
			If (row IsNot Nothing) Then
				key.ID = Convert.ToInt32(row.ID)
			End If
			RowToControl(row, source, Core.CtrlType.Edit)
		End If
	End Sub

	' ****************************************
	' *  Handler for DetailsView ItemUpdating
	' ****************************************

	Protected Sub LinkRankDetailsView_ItemUpdating(ByVal sender As Object, ByVal e As DetailsViewUpdateEventArgs)
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

	Protected Sub LinkRankDetailsView_ItemUpdated(ByVal sender As Object, ByVal e As DetailsViewUpdatedEventArgs)

		' Check If update successful 
		If (e.AffectedRows = 0) Then

			' Keep in edit mode 
			e.KeepInEditMode = True

			' Get original values 
			Dim source As WebControl = TryCast(sender, WebControl)
			If (source Is Nothing) Then Return
			Dim oldrow As LinkRankrow = New LinkRankrow()
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
		If (control.FindControl("x_ID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_ID'] = '" & control.FindControl("x_ID").ClientID & "';"
		End If
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
		row.SetPageType(Core.PageType.Edit) ' set page type = list for update

		' Field ID
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
	' *  Handler for DataSource Selecting
	' ************************************

	Protected Sub LinkRankDataSource_Selecting(Byval sender As Object, ByVal e As ObjectDataSourceSelectingEventArgs)
		e.InputParameters.Clear()
		e.InputParameters.Add("filter", LinkRankinf.GetUserFilter())
	End Sub

	' *************************************
	' *  DataSource Selected Event Handler
	' *************************************

	Protected Sub LinkRankDataSource_Selected(Byval sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.ReturnValue Is Nothing) Then
			If (e.Exception IsNot Nothing) Then
				lblMessage.Text = e.Exception.Message
				pnlMessage.Visible = True
			Else
				lblMessage.Text = "No records found"
				pnlMessage.Visible = True
			End If
		Else
			Dim rows As LinkRankrows = TryCast(e.ReturnValue, LinkRankrows)
			Dim i As Integer = 0
			Do While (rows IsNot Nothing AndAlso i < rows.Count)
				If (key.Equals(rows(i).GetKey())) Then
					LinkRankDetailsView.PageIndex = i
					Exit Do
				End If
				i += 1
			Loop
		End If
	End Sub

	' ***********************************
	' *  Handler for DataSource Updating
	' ***********************************

	Protected Sub LinkRankDataSource_Updating(ByVal sender As Object, ByVal e As ObjectDataSourceMethodEventArgs)
		Dim source As DetailsView = LinkRankDetailsView

		' Set up row object
		Dim row As LinkRankrow = TryCast(e.InputParameters(0), LinkRankrow)
		Dim data As LinkRankdal = New LinkRankdal()
		key.ID = Convert.ToInt32(row.ID)
		oldrow = data.LoadRow(key, LinkRankinf.GetUserFilter())
		ControlToRow(row, source) ' load values of controls to row
		newrow = row
		If (LinkRankbll.Updating(oldrow, newrow)) Then
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

	Protected Sub LinkRankDataSource_Updated(ByVal sender As Object, ByVal e As ObjectDataSourceStatusEventArgs)
		If (e.Exception IsNot Nothing) Then
			e.AffectedRows = 0
			e.ExceptionHandled = True
			lblMessage.Text = e.Exception.InnerException.Message
		Else
			lblMessage.Text = "Update successful"
			LinkRankbll.Updated(oldrow, newrow)
		End If
		If (lblMessage.Text <> "") Then pnlMessage.Visible = True
	End Sub

	' ************************************************
	' *  Change the page index for DetailsView paging
	' ************************************************

	Protected Sub ChangePageIndex(ByVal sender As Object, ByVal e As DetailsViewPageEventArgs)
		Dim data As LinkRankdal = New LinkRankdal()
		Dim rows As LinkRankrows = data.LoadList(LinkRankinf.GetUserFilter())
		If (rows IsNot Nothing AndAlso rows.Count >= e.NewPageIndex) Then
			key = rows(e.NewPageIndex).GetKey()
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
	<p><span class="aspnetmaker">Edit TABLE: Link Rank</span></p>
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
	SelectMethod="LoadList"
	UpdateMethod="Update"
	OnSelecting="LinkRankDataSource_Selecting"
	OnSelected="LinkRankDataSource_Selected"
	OnUpdating="LinkRankDataSource_Updating"
	OnUpdated="LinkRankDataSource_Updated"
	runat="server">
</asp:ObjectDataSource>
<!-- DetailsView -->
	<asp:DetailsView ID="LinkRankDetailsView"
		DataKeyNames="ID"
		DataSourceID="LinkRankDataSource"
		DefaultMode="Edit"
		GridLines="None"
		AutoGenerateRows="False" CssClass="ewDetailTable"
		OnInit="LinkRankDetailsView_Init"
		OnDataBound="LinkRankDetailsView_DataBound"
		OnItemUpdating="LinkRankDetailsView_ItemUpdating"
		OnItemUpdated="LinkRankDetailsView_ItemUpdated"
		OnUnload="LinkRankDetailsView_Unload"
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
					<asp:Label runat="server" id="xs_ID">ID</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:Label id="x_ID" Text='<%# Eval("ID")%>' CssClass="aspnetmaker" runat="server" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_LinkID">Link ID</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_LinkID" Columns="30" CssClass="aspnetmaker" runat="server" />
<ew:IntegerValidator ID="vc_LinkID" ClientValidationFunction="ew_CheckInteger" CssClass="aspnetmaker" runat="server" ControlToValidate="x_LinkID" ErrorMessage="Incorrect integer - Link ID" Display="None" ForeColor="Red" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_UserID">User ID</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_UserID" Columns="30" CssClass="aspnetmaker" runat="server" />
<ew:IntegerValidator ID="vc_UserID" ClientValidationFunction="ew_CheckInteger" CssClass="aspnetmaker" runat="server" ControlToValidate="x_UserID" ErrorMessage="Incorrect integer - User ID" Display="None" ForeColor="Red" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_RankNum">Rank Num</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_RankNum" Columns="30" CssClass="aspnetmaker" runat="server" />
<ew:IntegerValidator ID="vc_RankNum" ClientValidationFunction="ew_CheckInteger" CssClass="aspnetmaker" runat="server" ControlToValidate="x_RankNum" ErrorMessage="Incorrect integer - Rank Num" Display="None" ForeColor="Red" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_CateID">Cate ID</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_CateID" Columns="30" CssClass="aspnetmaker" runat="server" />
<ew:IntegerValidator ID="vc_CateID" ClientValidationFunction="ew_CheckInteger" CssClass="aspnetmaker" runat="server" ControlToValidate="x_CateID" ErrorMessage="Incorrect integer - Cate ID" Display="None" ForeColor="Red" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_Comment">Comment</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_Comment" Columns="30" MaxLength="255" CssClass="aspnetmaker" runat="server" />
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
				</EditItemTemplate>
			</asp:TemplateField>
		</Fields>
	</asp:DetailsView>
	<br />
	<input type="hidden" id="keyID" runat="server" />
</asp:Content>
