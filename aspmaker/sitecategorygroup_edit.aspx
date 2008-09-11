<%@ Page EnableViewState="true" Language="VB" ValidateRequest="False" Debug="True" MasterPageFile="masterpage.master" %>
<%@ MasterType VirtualPath="masterpage.master" %>
<%@ Import Namespace="PMGEN" %>
<%@ Import Namespace="EW.Data" %>
<%@ Import Namespace="EW.Data.Utilities" %>
<%@ Import Namespace="EW.Web" %>
<%@ Register NameSpace="PMGEN" TagPrefix="PMGEN" %>
<script runat="server">
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
		If Not mhUser.IsAdmin() Then
		  Session("Login_Link") = Request.RawUrl
                  Response.Redirect("/mhweb/login/login.aspx")
		End If
		Page.ClientScript.RegisterClientScriptInclude("ewv", "ewv.js")
		Response.Cache.SetCacheability(HttpCacheability.NoCache)
		If (Not Page.IsPostBack) Then
			lblReturnURL.Text = Session("ListPageURL")
			If (Request.QueryString.Count > 0) Then
				key = New SiteCategoryGroupkey()
				Dim messageList As ArrayList = SiteCategoryGroupinf.LoadKey(key) 
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
			key = TryCast(ViewState("key"), SiteCategoryGroupkey) ' restore from ViewState for postback
		End If
		If (SiteCategoryGroupDetailsView.Visible) Then
			RegisterClientID("CtrlID", SiteCategoryGroupDetailsView)
		End If
		If (SiteCategoryGroupDetailsView.FindControl("x_SiteCategoryGroupNM") IsNot Nothing) Then
			Page.Form.DefaultFocus = SiteCategoryGroupDetailsView.FindControl("x_SiteCategoryGroupNM").ClientID
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
		Dim row As SiteCategoryGrouprow = TryCast(SiteCategoryGroupDetailsView.DataItem, SiteCategoryGrouprow) ' get data object
		If (SiteCategoryGroupDetailsView.CurrentMode = DetailsViewMode.Edit) Then
			If (row IsNot Nothing) Then
				key.SiteCategoryGroupID = Convert.ToInt32(row.SiteCategoryGroupID)
			End If
			RowToControl(row, source, Core.CtrlType.Edit)
		End If
	End Sub

	' ****************************************
	' *  Handler for DetailsView ItemUpdating
	' ****************************************

	Protected Sub SiteCategoryGroupDetailsView_ItemUpdating(ByVal sender As Object, ByVal e As DetailsViewUpdateEventArgs)
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

	Protected Sub SiteCategoryGroupDetailsView_ItemUpdated(ByVal sender As Object, ByVal e As DetailsViewUpdatedEventArgs)

		' Check If update successful 
		If (e.AffectedRows = 0) Then

			' Keep in edit mode 
			e.KeepInEditMode = True

			' Get original values 
			Dim source As WebControl = TryCast(sender, WebControl)
			If (source Is Nothing) Then Return
			Dim oldrow As SiteCategoryGrouprow = New SiteCategoryGrouprow()
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
		If (control.FindControl("x_SiteCategoryGroupID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_SiteCategoryGroupID'] = '" & control.FindControl("x_SiteCategoryGroupID").ClientID & "';"
		End If
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
		row.SetPageType(Core.PageType.Edit) ' set page type = list for update

		' Field SiteCategoryGroupID
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
	' *  Handler for DataSource Selecting
	' ************************************

	Protected Sub SiteCategoryGroupDataSource_Selecting(Byval sender As Object, ByVal e As ObjectDataSourceSelectingEventArgs)
		e.InputParameters.Clear()
		e.InputParameters.Add("filter", SiteCategoryGroupinf.GetUserFilter())
	End Sub

	' *************************************
	' *  DataSource Selected Event Handler
	' *************************************

	Protected Sub SiteCategoryGroupDataSource_Selected(Byval sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.ReturnValue Is Nothing) Then
			If (e.Exception IsNot Nothing) Then
				lblMessage.Text = e.Exception.Message
				pnlMessage.Visible = True
			Else
				lblMessage.Text = "No records found"
				pnlMessage.Visible = True
			End If
		Else
			Dim rows As SiteCategoryGrouprows = TryCast(e.ReturnValue, SiteCategoryGrouprows)
			Dim i As Integer = 0
			Do While (rows IsNot Nothing AndAlso i < rows.Count)
				If (key.Equals(rows(i).GetKey())) Then
					SiteCategoryGroupDetailsView.PageIndex = i
					Exit Do
				End If
				i += 1
			Loop
		End If
	End Sub

	' ***********************************
	' *  Handler for DataSource Updating
	' ***********************************

	Protected Sub SiteCategoryGroupDataSource_Updating(ByVal sender As Object, ByVal e As ObjectDataSourceMethodEventArgs)
		Dim source As DetailsView = SiteCategoryGroupDetailsView

		' Set up row object
		Dim row As SiteCategoryGrouprow = TryCast(e.InputParameters(0), SiteCategoryGrouprow)
		Dim data As SiteCategoryGroupdal = New SiteCategoryGroupdal()
		key.SiteCategoryGroupID = Convert.ToInt32(row.SiteCategoryGroupID)
		oldrow = data.LoadRow(key, SiteCategoryGroupinf.GetUserFilter())
		ControlToRow(row, source) ' load values of controls to row
		newrow = row
		If (SiteCategoryGroupbll.Updating(oldrow, newrow)) Then
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

	Protected Sub SiteCategoryGroupDataSource_Updated(ByVal sender As Object, ByVal e As ObjectDataSourceStatusEventArgs)
		If (e.Exception IsNot Nothing) Then
			e.AffectedRows = 0
			e.ExceptionHandled = True
			lblMessage.Text = e.Exception.InnerException.Message
		Else
			lblMessage.Text = "Update successful"
			SiteCategoryGroupbll.Updated(oldrow, newrow)
		End If
		If (lblMessage.Text <> "") Then pnlMessage.Visible = True
	End Sub

	' ************************************************
	' *  Change the page index for DetailsView paging
	' ************************************************

	Protected Sub ChangePageIndex(ByVal sender As Object, ByVal e As DetailsViewPageEventArgs)
		Dim data As SiteCategoryGroupdal = New SiteCategoryGroupdal()
		Dim rows As SiteCategoryGrouprows = data.LoadList(SiteCategoryGroupinf.GetUserFilter())
		If (rows IsNot Nothing AndAlso rows.Count >= e.NewPageIndex) Then
			key = rows(e.NewPageIndex).GetKey()
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
	<p><span class="aspnetmaker">Edit TABLE: Site Category Group</span></p>
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
	SelectMethod="LoadList"
	UpdateMethod="Update"
	OnSelecting="SiteCategoryGroupDataSource_Selecting"
	OnSelected="SiteCategoryGroupDataSource_Selected"
	OnUpdating="SiteCategoryGroupDataSource_Updating"
	OnUpdated="SiteCategoryGroupDataSource_Updated"
	runat="server">
</asp:ObjectDataSource>
<!-- DetailsView -->
	<asp:DetailsView ID="SiteCategoryGroupDetailsView"
		DataKeyNames="SiteCategoryGroupID"
		DataSourceID="SiteCategoryGroupDataSource"
		DefaultMode="Edit"
		GridLines="None"
		AutoGenerateRows="False" CssClass="ewDetailTable"
		OnInit="SiteCategoryGroupDetailsView_Init"
		OnDataBound="SiteCategoryGroupDetailsView_DataBound"
		OnItemUpdating="SiteCategoryGroupDetailsView_ItemUpdating"
		OnItemUpdated="SiteCategoryGroupDetailsView_ItemUpdated"
		OnUnload="SiteCategoryGroupDetailsView_Unload"
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
					<asp:Label runat="server" id="xs_SiteCategoryGroupID">Site Category Group ID</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:Label id="x_SiteCategoryGroupID" Text='<%# Eval("SiteCategoryGroupID")%>' CssClass="aspnetmaker" runat="server" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_SiteCategoryGroupNM">Site Category Group NM</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_SiteCategoryGroupNM" Columns="30" MaxLength="255" CssClass="aspnetmaker" runat="server" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_SiteCategoryGroupDS">Site Category Group DS</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_SiteCategoryGroupDS" Columns="30" MaxLength="255" CssClass="aspnetmaker" runat="server" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_SiteCategoryGroupOrder">Site Category Group Order</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_SiteCategoryGroupOrder" Columns="30" CssClass="aspnetmaker" runat="server" />
<ew:IntegerValidator ID="vc_SiteCategoryGroupOrder" ClientValidationFunction="ew_CheckInteger" CssClass="aspnetmaker" runat="server" ControlToValidate="x_SiteCategoryGroupOrder" ErrorMessage="Incorrect integer - Site Category Group Order" Display="None" ForeColor="Red" />
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
