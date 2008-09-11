<%@ Page EnableViewState="true" Language="VB" ValidateRequest="False" Debug="True" MasterPageFile="masterpage.master" %>
<%@ MasterType VirtualPath="masterpage.master" %>
<%@ Import Namespace="PMGEN" %>
<%@ Import Namespace="EW.Data" %>
<%@ Import Namespace="EW.Data.Utilities" %>
<%@ Import Namespace="EW.Web" %>
<%@ Register NameSpace="PMGEN" TagPrefix="PMGEN" %>
<script runat="server">
	Dim key As PageAliaskey ' record key
	Dim oldrow As PageAliasrow ' old record data input by user
	Dim newrow As PageAliasrow ' new record data input by user
	Dim objProfile As TableProfile ' table profile

	' *************************
	' *  Handler for Page Init
	' *************************

	Protected Sub Page_Init(ByVal s As Object, ByVal e As System.EventArgs)
		If Not mhUser.IsAdmin() Then
		  Session("Login_Link") = Request.RawUrl
		  Response.Redirect("/mhweb/login/login.aspx")
		End If
		objProfile = CustomProfile.GetTable(Share.ProjectName, PageAliasinf.TableVar)
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
				key = New PageAliaskey()
				Dim messageList As ArrayList = PageAliasinf.LoadKey(key) 
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
			key = TryCast(ViewState("key"), PageAliaskey) ' restore from ViewState for postback
		End If
		If (PageAliasDetailsView.Visible) Then
			RegisterClientID("CtrlID", PageAliasDetailsView)
		End If
		If (PageAliasDetailsView.FindControl("x_PageURL") IsNot Nothing) Then
			Page.Form.DefaultFocus = PageAliasDetailsView.FindControl("x_PageURL").ClientID
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

	Protected Sub PageAliasDetailsView_Init(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			PageAliasdal.OpenConnection()
		Catch err As Exception
			lblMessage.Text = err.Message
			pnlMessage.Visible = True
		End Try
	End Sub

	' *************************************
	' *  Handler for DetailsView DataBound
	' *************************************

	Protected Sub PageAliasDetailsView_DataBound(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim source As DetailsView = PageAliasDetailsView
		Dim row As PageAliasrow = TryCast(PageAliasDetailsView.DataItem, PageAliasrow) ' get data object
		If (PageAliasDetailsView.CurrentMode = DetailsViewMode.Edit) Then
			If (row IsNot Nothing) Then
				key.PageAliasID = Convert.ToInt32(row.PageAliasID)
			End If
			RowToControl(row, source, Core.CtrlType.Edit)
		End If
	End Sub

	' ****************************************
	' *  Handler for DetailsView ItemUpdating
	' ****************************************

	Protected Sub PageAliasDetailsView_ItemUpdating(ByVal sender As Object, ByVal e As DetailsViewUpdateEventArgs)
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

	Protected Sub PageAliasDetailsView_ItemUpdated(ByVal sender As Object, ByVal e As DetailsViewUpdatedEventArgs)

		' Check If update successful 
		If (e.AffectedRows = 0) Then

			' Keep in edit mode 
			e.KeepInEditMode = True

			' Get original values 
			Dim source As WebControl = TryCast(sender, WebControl)
			If (source Is Nothing) Then Return
			Dim oldrow As PageAliasrow = New PageAliasrow()
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

	Protected Sub PageAliasDetailsView_Unload(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			PageAliasdal.CloseAndDisposeConnection()
		Catch
		End Try
	End Sub

	' ********************************
	' * Register ClientID of Controls
	' ********************************

	Private Sub RegisterClientID(ByVal sBlockName As String, ByVal control As Control)
		Dim jsString As String = String.Empty
		If (control.FindControl("x_PageAliasID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_PageAliasID'] = '" & control.FindControl("x_PageAliasID").ClientID & "';"
		End If
		If (control.FindControl("x_PageURL") IsNot Nothing) Then
			jsString &= "ew.Controls['x_PageURL'] = '" & control.FindControl("x_PageURL").ClientID & "';"
		End If
		If (control.FindControl("x_TargetURL") IsNot Nothing) Then
			jsString &= "ew.Controls['x_TargetURL'] = '" & control.FindControl("x_TargetURL").ClientID & "';"
		End If
		If (control.FindControl("x_AliasType") IsNot Nothing) Then
			jsString &= "ew.Controls['x_AliasType'] = '" & control.FindControl("x_AliasType").ClientID & "';"
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
			Dim x_PageURL As TextBox = TryCast(control.FindControl("x_PageURL"), TextBox)
			If (x_PageURL IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_PageURL.Text)) Then
					messageList.Add("Invalid Value (String): Page URL")
				End If
			End If
			Dim x_TargetURL As TextBox = TryCast(control.FindControl("x_TargetURL"), TextBox)
			If (x_TargetURL IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_TargetURL.Text)) Then
					messageList.Add("Invalid Value (String): Target URL")
				End If
			End If
			Dim x_AliasType As TextBox = TryCast(control.FindControl("x_AliasType"), TextBox)
			If (x_AliasType IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_AliasType.Text)) Then
					messageList.Add("Invalid Value (String): Alias Type")
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

	Private Sub ControlToRow(ByVal row As PageAliasrow, ByVal control As WebControl)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		row.SetPageType(Core.PageType.Edit) ' set page type = list for update

		' Field PageAliasID
		' Field PageURL

		Dim x_PageURL As TextBox = TryCast(control.FindControl("x_PageURL"), TextBox)
		If (x_PageURL.Text <> String.Empty) Then row.PageURL = x_PageURL.Text Else row.PageURL = Nothing

		' Field TargetURL
		Dim x_TargetURL As TextBox = TryCast(control.FindControl("x_TargetURL"), TextBox)
		If (x_TargetURL.Text <> String.Empty) Then row.TargetURL = x_TargetURL.Text Else row.TargetURL = Nothing

		' Field AliasType
		Dim x_AliasType As TextBox = TryCast(control.FindControl("x_AliasType"), TextBox)
		If (x_AliasType.Text <> String.Empty) Then row.AliasType = x_AliasType.Text Else row.AliasType = Nothing

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
		Dim data As PageAliasdal = New PageAliasdal()
		Dim newkey As PageAliaskey = New PageAliaskey()
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

	Private Sub RowToControl(ByVal row As PageAliasrow, ByVal control As WebControl, ByVal ct As Core.CtrlType)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		If (ct = Core.CtrlType.Edit) Then ' set up edit control

			' Field PageURL
			Dim x_PageURL As TextBox = TryCast(control.FindControl("x_PageURL"), TextBox)
			If (row.PageURL IsNot Nothing) Then x_PageURL.Text = row.PageURL.ToString() Else x_PageURL.Text = String.Empty

			' Field TargetURL
			Dim x_TargetURL As TextBox = TryCast(control.FindControl("x_TargetURL"), TextBox)
			If (row.TargetURL IsNot Nothing) Then x_TargetURL.Text = row.TargetURL.ToString() Else x_TargetURL.Text = String.Empty

			' Field AliasType
			Dim x_AliasType As TextBox = TryCast(control.FindControl("x_AliasType"), TextBox)
			If (row.AliasType IsNot Nothing) Then x_AliasType.Text = row.AliasType.ToString() Else x_AliasType.Text = String.Empty

			' Field CompanyID
			Dim x_CompanyID As TextBox = TryCast(control.FindControl("x_CompanyID"), TextBox)
			If (row.CompanyID.HasValue) Then x_CompanyID.Text = row.CompanyID.ToString() Else x_CompanyID.Text = String.Empty
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
	' *  Handler for DataSource Selecting
	' ************************************

	Protected Sub PageAliasDataSource_Selecting(Byval sender As Object, ByVal e As ObjectDataSourceSelectingEventArgs)
		e.InputParameters.Clear()
		e.InputParameters.Add("filter", PageAliasinf.GetUserFilter())
	End Sub

	' *************************************
	' *  DataSource Selected Event Handler
	' *************************************

	Protected Sub PageAliasDataSource_Selected(Byval sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.ReturnValue Is Nothing) Then
			If (e.Exception IsNot Nothing) Then
				lblMessage.Text = e.Exception.Message
				pnlMessage.Visible = True
			Else
				lblMessage.Text = "No records found"
				pnlMessage.Visible = True
			End If
		Else
			Dim rows As PageAliasrows = TryCast(e.ReturnValue, PageAliasrows)
			Dim i As Integer = 0
			Do While (rows IsNot Nothing AndAlso i < rows.Count)
				If (key.Equals(rows(i).GetKey())) Then
					PageAliasDetailsView.PageIndex = i
					Exit Do
				End If
				i += 1
			Loop
		End If
	End Sub

	' ***********************************
	' *  Handler for DataSource Updating
	' ***********************************

	Protected Sub PageAliasDataSource_Updating(ByVal sender As Object, ByVal e As ObjectDataSourceMethodEventArgs)
		Dim source As DetailsView = PageAliasDetailsView

		' Set up row object
		Dim row As PageAliasrow = TryCast(e.InputParameters(0), PageAliasrow)
		Dim data As PageAliasdal = New PageAliasdal()
		key.PageAliasID = Convert.ToInt32(row.PageAliasID)
		oldrow = data.LoadRow(key, PageAliasinf.GetUserFilter())
		ControlToRow(row, source) ' load values of controls to row
		newrow = row
		If (PageAliasbll.Updating(oldrow, newrow)) Then
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

	Protected Sub PageAliasDataSource_Updated(ByVal sender As Object, ByVal e As ObjectDataSourceStatusEventArgs)
		If (e.Exception IsNot Nothing) Then
			e.AffectedRows = 0
			e.ExceptionHandled = True
			lblMessage.Text = e.Exception.InnerException.Message
		Else
			lblMessage.Text = "Update successful"
			PageAliasbll.Updated(oldrow, newrow)
		End If
		If (lblMessage.Text <> "") Then pnlMessage.Visible = True
	End Sub

	' ************************************************
	' *  Change the page index for DetailsView paging
	' ************************************************

	Protected Sub ChangePageIndex(ByVal sender As Object, ByVal e As DetailsViewPageEventArgs)
		Dim data As PageAliasdal = New PageAliasdal()
		Dim rows As PageAliasrows = data.LoadList(PageAliasinf.GetUserFilter())
		If (rows IsNot Nothing AndAlso rows.Count >= e.NewPageIndex) Then
			key = rows(e.NewPageIndex).GetKey()
		End If
	End Sub

	' *******************
	' *  Get User Filter
	' *******************

	Private Function GetUserFilter() As String 
		Return PageAliasinf.GetUserFilter()
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
	<p><span class="aspnetmaker">Edit TABLE: Page Alias</span></p>
	<p>
		<asp:Hyperlink id="lnkList" Text="Back to List" NavigateUrl="pagealias_list.aspx" CssClass="aspnetmaker" runat="server" />
		<asp:Label id="lblReturnUrl" Visible="False" CssClass="aspnetmaker" runat="server">pagealias_list.aspx</asp:Label>
	</p>
	<asp:ValidationSummary id="xevs_PageAlias" CssClass="aspnetmaker" runat="server"
		HeaderText=""
		ShowSummary="False"
		ShowMessageBox="True"
		ForeColor="#FF0000" />
	<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
		<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
	</asp:PlaceHolder>
<!-- Data Source -->
<asp:ObjectDataSource ID="PageAliasDataSource"
	TypeName="PMGEN.PageAliasdal"
	DataObjectTypeName="PMGEN.PageAliasrow"
	SelectMethod="LoadList"
	UpdateMethod="Update"
	OnSelecting="PageAliasDataSource_Selecting"
	OnSelected="PageAliasDataSource_Selected"
	OnUpdating="PageAliasDataSource_Updating"
	OnUpdated="PageAliasDataSource_Updated"
	runat="server">
</asp:ObjectDataSource>
<!-- DetailsView -->
	<asp:DetailsView ID="PageAliasDetailsView"
		DataKeyNames="PageAliasID"
		DataSourceID="PageAliasDataSource"
		DefaultMode="Edit"
		GridLines="None"
		AutoGenerateRows="False" CssClass="ewDetailTable"
		OnInit="PageAliasDetailsView_Init"
		OnDataBound="PageAliasDetailsView_DataBound"
		OnItemUpdating="PageAliasDetailsView_ItemUpdating"
		OnItemUpdated="PageAliasDetailsView_ItemUpdated"
		OnUnload="PageAliasDetailsView_Unload"
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
					<asp:Label runat="server" id="xs_PageAliasID">Page Alias ID</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:Label id="x_PageAliasID" Text='<%# Eval("PageAliasID")%>' CssClass="aspnetmaker" runat="server" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_PageURL">Page URL</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_PageURL" Columns="30" MaxLength="255" CssClass="aspnetmaker" runat="server" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_TargetURL">Target URL</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_TargetURL" Columns="30" MaxLength="255" CssClass="aspnetmaker" runat="server" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_AliasType">Alias Type</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_AliasType" Columns="30" MaxLength="10" CssClass="aspnetmaker" runat="server" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_CompanyID">Company ID</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_CompanyID" Columns="30" CssClass="aspnetmaker" runat="server" />
<ew:IntegerValidator ID="vc_CompanyID" ClientValidationFunction="ew_CheckInteger" CssClass="aspnetmaker" runat="server" ControlToValidate="x_CompanyID" ErrorMessage="Incorrect integer - Company ID" Display="None" ForeColor="Red" />
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
