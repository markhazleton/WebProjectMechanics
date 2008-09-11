<%@ Page EnableViewState="true" Language="VB" ValidateRequest="False" Debug="True" MasterPageFile="masterpage.master" %>
<%@ MasterType VirtualPath="masterpage.master" %>
<%@ Import Namespace="PMGEN" %>
<%@ Import Namespace="EW.Data" %>
<%@ Import Namespace="EW.Data.Utilities" %>
<%@ Import Namespace="EW.Web" %>
<%@ Register NameSpace="PMGEN" TagPrefix="PMGEN" %>
<script runat="server">
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
		If Not mhUser.IsAdmin() Then
		  Session("Login_Link") = Request.RawUrl
                  Response.Redirect("/mhweb/login/login.aspx")
		End If
		Page.ClientScript.RegisterClientScriptInclude("ewv", "ewv.js")
		Response.Cache.SetCacheability(HttpCacheability.NoCache)
		If (Not Page.IsPostBack) Then
			lblReturnURL.Text = Session("ListPageURL")
			If (Request.QueryString.Count > 0) Then
				key = New SiteTemplatekey()
				Dim messageList As ArrayList = SiteTemplateinf.LoadKey(key) 
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
			key = TryCast(ViewState("key"), SiteTemplatekey) ' restore from ViewState for postback
		End If
		If (SiteTemplateDetailsView.Visible) Then
			RegisterClientID("CtrlID", SiteTemplateDetailsView)
		End If
		If (SiteTemplateDetailsView.FindControl("x_TemplatePrefix") IsNot Nothing) Then
			Page.Form.DefaultFocus = SiteTemplateDetailsView.FindControl("x_TemplatePrefix").ClientID
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
		Dim row As SiteTemplaterow = TryCast(SiteTemplateDetailsView.DataItem, SiteTemplaterow) ' get data object
		If (SiteTemplateDetailsView.CurrentMode = DetailsViewMode.Edit) Then
			If (row IsNot Nothing) Then
				key.TemplatePrefix = row.TemplatePrefix
			End If
			RowToControl(row, source, Core.CtrlType.Edit)
		End If
	End Sub

	' ****************************************
	' *  Handler for DetailsView ItemUpdating
	' ****************************************

	Protected Sub SiteTemplateDetailsView_ItemUpdating(ByVal sender As Object, ByVal e As DetailsViewUpdateEventArgs)
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

	Protected Sub SiteTemplateDetailsView_ItemUpdated(ByVal sender As Object, ByVal e As DetailsViewUpdatedEventArgs)

		' Check If update successful 
		If (e.AffectedRows = 0) Then

			' Keep in edit mode 
			e.KeepInEditMode = True

			' Get original values 
			Dim source As WebControl = TryCast(sender, WebControl)
			If (source Is Nothing) Then Return
			Dim oldrow As SiteTemplaterow = New SiteTemplaterow()
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
		row.SetPageType(Core.PageType.Edit) ' set page type = list for update

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
			Dim sKeyWhere As string = data.KeyFilter(key)

		' Check for duplicate Code
			sWhere = "([TemplatePrefix] = '" + Db.AdjustSql((CType(control.FindControl("x_TemplatePrefix"),TextBox)).Text) + "' AND NOT (" & sKeyWhere & "))"
			If (data.LoadList(sWhere) IsNot Nothing) Then
				messageList.Add("Duplicate value: Code")
			End If

		' Check for duplicate Name
			sWhere = "([Name] = '" + Db.AdjustSql((CType(control.FindControl("x_Name"),TextBox)).Text) + "' AND NOT (" & sKeyWhere & "))"
			If (data.LoadList(sWhere) IsNot Nothing) Then
				messageList.Add("Duplicate value: Name")
			End If
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
			TryCast(control.FindControl("x_TemplatePrefix"), TextBox).Enabled = False

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
	' *  Handler for DataSource Selecting
	' ************************************

	Protected Sub SiteTemplateDataSource_Selecting(Byval sender As Object, ByVal e As ObjectDataSourceSelectingEventArgs)
		e.InputParameters.Clear()
		e.InputParameters.Add("filter", SiteTemplateinf.GetUserFilter())
	End Sub

	' *************************************
	' *  DataSource Selected Event Handler
	' *************************************

	Protected Sub SiteTemplateDataSource_Selected(Byval sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.ReturnValue Is Nothing) Then
			If (e.Exception IsNot Nothing) Then
				lblMessage.Text = e.Exception.Message
				pnlMessage.Visible = True
			Else
				lblMessage.Text = "No records found"
				pnlMessage.Visible = True
			End If
		Else
			Dim rows As SiteTemplaterows = TryCast(e.ReturnValue, SiteTemplaterows)
			Dim i As Integer = 0
			Do While (rows IsNot Nothing AndAlso i < rows.Count)
				If (key.Equals(rows(i).GetKey())) Then
					SiteTemplateDetailsView.PageIndex = i
					Exit Do
				End If
				i += 1
			Loop
		End If
	End Sub

	' ***********************************
	' *  Handler for DataSource Updating
	' ***********************************

	Protected Sub SiteTemplateDataSource_Updating(ByVal sender As Object, ByVal e As ObjectDataSourceMethodEventArgs)
		Dim source As DetailsView = SiteTemplateDetailsView

		' Set up row object
		Dim row As SiteTemplaterow = TryCast(e.InputParameters(0), SiteTemplaterow)
		Dim data As SiteTemplatedal = New SiteTemplatedal()
		key.TemplatePrefix = Convert.ToString(row.TemplatePrefix)
		oldrow = data.LoadRow(key, SiteTemplateinf.GetUserFilter())
		ControlToRow(row, source) ' load values of controls to row
		newrow = row
		If (SiteTemplatebll.Updating(oldrow, newrow)) Then
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

	Protected Sub SiteTemplateDataSource_Updated(ByVal sender As Object, ByVal e As ObjectDataSourceStatusEventArgs)
		If (e.Exception IsNot Nothing) Then
			e.AffectedRows = 0
			e.ExceptionHandled = True
			lblMessage.Text = e.Exception.InnerException.Message
		Else
			lblMessage.Text = "Update successful"
			SiteTemplatebll.Updated(oldrow, newrow)
		End If
		If (lblMessage.Text <> "") Then pnlMessage.Visible = True
	End Sub

	' ************************************************
	' *  Change the page index for DetailsView paging
	' ************************************************

	Protected Sub ChangePageIndex(ByVal sender As Object, ByVal e As DetailsViewPageEventArgs)
		Dim data As SiteTemplatedal = New SiteTemplatedal()
		Dim rows As SiteTemplaterows = data.LoadList(SiteTemplateinf.GetUserFilter())
		If (rows IsNot Nothing AndAlso rows.Count >= e.NewPageIndex) Then
			key = rows(e.NewPageIndex).GetKey()
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
	<p><span class="aspnetmaker">Edit TABLE: Site Template</span></p>
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
	SelectMethod="LoadList"
	UpdateMethod="Update"
	OnSelecting="SiteTemplateDataSource_Selecting"
	OnSelected="SiteTemplateDataSource_Selected"
	OnUpdating="SiteTemplateDataSource_Updating"
	OnUpdated="SiteTemplateDataSource_Updated"
	runat="server">
</asp:ObjectDataSource>
<!-- DetailsView -->
	<asp:DetailsView ID="SiteTemplateDetailsView"
		DataKeyNames="TemplatePrefix"
		DataSourceID="SiteTemplateDataSource"
		DefaultMode="Edit"
		GridLines="None"
		AutoGenerateRows="False" CssClass="ewDetailTable"
		OnInit="SiteTemplateDetailsView_Init"
		OnDataBound="SiteTemplateDetailsView_DataBound"
		OnItemUpdating="SiteTemplateDetailsView_ItemUpdating"
		OnItemUpdated="SiteTemplateDetailsView_ItemUpdated"
		OnUnload="SiteTemplateDetailsView_Unload"
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
					<asp:Label runat="server" id="xs_TemplatePrefix">Code<span class='ewmsg'>&nbsp;*</span></asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_TemplatePrefix" Columns="30" MaxLength="10" CssClass="aspnetmaker" runat="server" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_Name">Name<span class='ewmsg'>&nbsp;*</span></asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_Name" Columns="30" MaxLength="50" CssClass="aspnetmaker" runat="server" />
<asp:RequiredFieldValidator ID="vx_Name" CssClass="aspnetmaker" runat="server" ControlToValidate="x_Name" ErrorMessage="Please enter required field - Name" ForeColor="Red" Display="None" Text="*" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_Top">Top</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_Top" TextMode="MultiLine" Rows="20" Columns="80" CssClass="aspnetmaker" runat="server" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_Bottom">Bottom</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_Bottom" TextMode="MultiLine" Rows="20" Columns="80" CssClass="aspnetmaker" runat="server" />
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
