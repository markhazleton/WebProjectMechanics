<%@ Page EnableViewState="true" Language="VB" ValidateRequest="False" Debug="True" MasterPageFile="masterpage.master" %>
<%@ MasterType VirtualPath="masterpage.master" %>
<%@ Import Namespace="PMGEN" %>
<%@ Import Namespace="EW.Data" %>
<%@ Import Namespace="EW.Data.Utilities" %>
<%@ Import Namespace="EW.Web" %>
<%@ Register NameSpace="PMGEN" TagPrefix="PMGEN" %>
<script runat="server">
	Dim key As PageTypekey ' record key
	Dim oldrow As PageTyperow ' old record data input by user
	Dim newrow As PageTyperow ' new record data input by user
	Dim objProfile As TableProfile ' table profile

	' *************************
	' *  Handler for Page Init
	' *************************

	Protected Sub Page_Init(ByVal s As Object, ByVal e As System.EventArgs)
		If Not mhUser.IsAdmin() Then
		  Session("Login_Link") = Request.RawUrl
		  Response.Redirect("/mhweb/login/login.aspx")
		End If
		objProfile = CustomProfile.GetTable(Share.ProjectName, PageTypeinf.TableVar)
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
				key = New PageTypekey()
				Dim messageList As ArrayList = PageTypeinf.LoadKey(key) 
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
			key = TryCast(ViewState("key"), PageTypekey) ' restore from ViewState for postback
		End If
		If (PageTypeDetailsView.Visible) Then
			RegisterClientID("CtrlID", PageTypeDetailsView)
		End If
		If (PageTypeDetailsView.FindControl("x_PageTypeCD") IsNot Nothing) Then
			Page.Form.DefaultFocus = PageTypeDetailsView.FindControl("x_PageTypeCD").ClientID
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

	Protected Sub PageTypeDetailsView_Init(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			PageTypedal.OpenConnection()
		Catch err As Exception
			lblMessage.Text = err.Message
			pnlMessage.Visible = True
		End Try
	End Sub

	' *************************************
	' *  Handler for DetailsView DataBound
	' *************************************

	Protected Sub PageTypeDetailsView_DataBound(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim source As DetailsView = PageTypeDetailsView
		Dim row As PageTyperow = TryCast(PageTypeDetailsView.DataItem, PageTyperow) ' get data object
		If (PageTypeDetailsView.CurrentMode = DetailsViewMode.Edit) Then
			If (row IsNot Nothing) Then
				key.PageTypeID = Convert.ToInt32(row.PageTypeID)
			End If
			RowToControl(row, source, Core.CtrlType.Edit)
		End If
	End Sub

	' ****************************************
	' *  Handler for DetailsView ItemUpdating
	' ****************************************

	Protected Sub PageTypeDetailsView_ItemUpdating(ByVal sender As Object, ByVal e As DetailsViewUpdateEventArgs)
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

	Protected Sub PageTypeDetailsView_ItemUpdated(ByVal sender As Object, ByVal e As DetailsViewUpdatedEventArgs)

		' Check If update successful 
		If (e.AffectedRows = 0) Then

			' Keep in edit mode 
			e.KeepInEditMode = True

			' Get original values 
			Dim source As WebControl = TryCast(sender, WebControl)
			If (source Is Nothing) Then Return
			Dim oldrow As PageTyperow = New PageTyperow()
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

	Protected Sub PageTypeDetailsView_Unload(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			PageTypedal.CloseAndDisposeConnection()
		Catch
		End Try
	End Sub

	' ********************************
	' * Register ClientID of Controls
	' ********************************

	Private Sub RegisterClientID(ByVal sBlockName As String, ByVal control As Control)
		Dim jsString As String = String.Empty
		If (control.FindControl("x_PageTypeID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_PageTypeID'] = '" & control.FindControl("x_PageTypeID").ClientID & "';"
		End If
		If (control.FindControl("x_PageTypeCD") IsNot Nothing) Then
			jsString &= "ew.Controls['x_PageTypeCD'] = '" & control.FindControl("x_PageTypeCD").ClientID & "';"
		End If
		If (control.FindControl("x_PageTypeDesc") IsNot Nothing) Then
			jsString &= "ew.Controls['x_PageTypeDesc'] = '" & control.FindControl("x_PageTypeDesc").ClientID & "';"
		End If
		If (control.FindControl("x_PageFileName") IsNot Nothing) Then
			jsString &= "ew.Controls['x_PageFileName'] = '" & control.FindControl("x_PageFileName").ClientID & "';"
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
			Dim x_PageTypeCD As TextBox = TryCast(control.FindControl("x_PageTypeCD"), TextBox)
			If (x_PageTypeCD IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_PageTypeCD.Text)) Then
					messageList.Add("Invalid Value (String): Page Type CD")
				End If
			End If
			Dim x_PageTypeDesc As TextBox = TryCast(control.FindControl("x_PageTypeDesc"), TextBox)
			If (x_PageTypeDesc IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_PageTypeDesc.Text)) Then
					messageList.Add("Invalid Value (String): Page Type Desc")
				End If
			End If
			Dim x_PageFileName As TextBox = TryCast(control.FindControl("x_PageFileName"), TextBox)
			If (x_PageFileName IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_PageFileName.Text)) Then
					messageList.Add("Invalid Value (String): Page File Name")
				End If
			End If
		End If
		Return IIf(messageList.Count = 0, Nothing, messageList)
	End Function

	' **********************************************
	' *  Convert Input Control Values to Row Object
	' **********************************************

	Private Sub ControlToRow(ByVal row As PageTyperow, ByVal control As WebControl)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		row.SetPageType(Core.PageType.Edit) ' set page type = list for update

		' Field PageTypeID
		' Field PageTypeCD

		Dim x_PageTypeCD As TextBox = TryCast(control.FindControl("x_PageTypeCD"), TextBox)
		If (x_PageTypeCD.Text <> String.Empty) Then row.PageTypeCD = x_PageTypeCD.Text Else row.PageTypeCD = Nothing

		' Field PageTypeDesc
		Dim x_PageTypeDesc As TextBox = TryCast(control.FindControl("x_PageTypeDesc"), TextBox)
		If (x_PageTypeDesc.Text <> String.Empty) Then row.PageTypeDesc = x_PageTypeDesc.Text Else row.PageTypeDesc = Nothing

		' Field PageFileName
		Dim x_PageFileName As TextBox = TryCast(control.FindControl("x_PageFileName"), TextBox)
		If (x_PageFileName.Text <> String.Empty) Then row.PageFileName = x_PageFileName.Text Else row.PageFileName = Nothing
	End Sub

	' ****************************
	' *  Check for duplicate Key
	' ****************************

	Private Function CheckDuplicateKey(ByVal control As WebControl) As ArrayList 
		Dim sWhere As String = String.Empty
		Dim messageList As ArrayList = New ArrayList()
		Dim data As PageTypedal = New PageTypedal()
		Dim newkey As PageTypekey = New PageTypekey()
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

	Private Sub RowToControl(ByVal row As PageTyperow, ByVal control As WebControl, ByVal ct As Core.CtrlType)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		If (ct = Core.CtrlType.Edit) Then ' set up edit control

			' Field PageTypeCD
			Dim x_PageTypeCD As TextBox = TryCast(control.FindControl("x_PageTypeCD"), TextBox)
			If (row.PageTypeCD IsNot Nothing) Then x_PageTypeCD.Text = row.PageTypeCD.ToString() Else x_PageTypeCD.Text = String.Empty

			' Field PageTypeDesc
			Dim x_PageTypeDesc As TextBox = TryCast(control.FindControl("x_PageTypeDesc"), TextBox)
			If (row.PageTypeDesc IsNot Nothing) Then x_PageTypeDesc.Text = row.PageTypeDesc.ToString() Else x_PageTypeDesc.Text = String.Empty

			' Field PageFileName
			Dim x_PageFileName As TextBox = TryCast(control.FindControl("x_PageFileName"), TextBox)
			If (row.PageFileName IsNot Nothing) Then x_PageFileName.Text = row.PageFileName.ToString() Else x_PageFileName.Text = String.Empty
		End If
	End Sub

	' ************************************
	' *  Handler for DataSource Selecting
	' ************************************

	Protected Sub PageTypeDataSource_Selecting(Byval sender As Object, ByVal e As ObjectDataSourceSelectingEventArgs)
		e.InputParameters.Clear()
		e.InputParameters.Add("filter", PageTypeinf.GetUserFilter())
	End Sub

	' *************************************
	' *  DataSource Selected Event Handler
	' *************************************

	Protected Sub PageTypeDataSource_Selected(Byval sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.ReturnValue Is Nothing) Then
			If (e.Exception IsNot Nothing) Then
				lblMessage.Text = e.Exception.Message
				pnlMessage.Visible = True
			Else
				lblMessage.Text = "No records found"
				pnlMessage.Visible = True
			End If
		Else
			Dim rows As PageTyperows = TryCast(e.ReturnValue, PageTyperows)
			Dim i As Integer = 0
			Do While (rows IsNot Nothing AndAlso i < rows.Count)
				If (key.Equals(rows(i).GetKey())) Then
					PageTypeDetailsView.PageIndex = i
					Exit Do
				End If
				i += 1
			Loop
		End If
	End Sub

	' ***********************************
	' *  Handler for DataSource Updating
	' ***********************************

	Protected Sub PageTypeDataSource_Updating(ByVal sender As Object, ByVal e As ObjectDataSourceMethodEventArgs)
		Dim source As DetailsView = PageTypeDetailsView

		' Set up row object
		Dim row As PageTyperow = TryCast(e.InputParameters(0), PageTyperow)
		Dim data As PageTypedal = New PageTypedal()
		key.PageTypeID = Convert.ToInt32(row.PageTypeID)
		oldrow = data.LoadRow(key, PageTypeinf.GetUserFilter())
		ControlToRow(row, source) ' load values of controls to row
		newrow = row
		If (PageTypebll.Updating(oldrow, newrow)) Then
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

	Protected Sub PageTypeDataSource_Updated(ByVal sender As Object, ByVal e As ObjectDataSourceStatusEventArgs)
		If (e.Exception IsNot Nothing) Then
			e.AffectedRows = 0
			e.ExceptionHandled = True
			lblMessage.Text = e.Exception.InnerException.Message
		Else
			lblMessage.Text = "Update successful"
			PageTypebll.Updated(oldrow, newrow)
		End If
		If (lblMessage.Text <> "") Then pnlMessage.Visible = True
	End Sub

	' ************************************************
	' *  Change the page index for DetailsView paging
	' ************************************************

	Protected Sub ChangePageIndex(ByVal sender As Object, ByVal e As DetailsViewPageEventArgs)
		Dim data As PageTypedal = New PageTypedal()
		Dim rows As PageTyperows = data.LoadList(PageTypeinf.GetUserFilter())
		If (rows IsNot Nothing AndAlso rows.Count >= e.NewPageIndex) Then
			key = rows(e.NewPageIndex).GetKey()
		End If
	End Sub

	' *******************
	' *  Get User Filter
	' *******************

	Private Function GetUserFilter() As String 
		Return PageTypeinf.GetUserFilter()
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
	<p><span class="aspnetmaker">Edit TABLE: Page Type</span></p>
	<p>
		<asp:Hyperlink id="lnkList" Text="Back to List" NavigateUrl="pagetype_list.aspx" CssClass="aspnetmaker" runat="server" />
		<asp:Label id="lblReturnUrl" Visible="False" CssClass="aspnetmaker" runat="server">pagetype_list.aspx</asp:Label>
	</p>
	<asp:ValidationSummary id="xevs_PageType" CssClass="aspnetmaker" runat="server"
		HeaderText=""
		ShowSummary="False"
		ShowMessageBox="True"
		ForeColor="#FF0000" />
	<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
		<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
	</asp:PlaceHolder>
<!-- Data Source -->
<asp:ObjectDataSource ID="PageTypeDataSource"
	TypeName="PMGEN.PageTypedal"
	DataObjectTypeName="PMGEN.PageTyperow"
	SelectMethod="LoadList"
	UpdateMethod="Update"
	OnSelecting="PageTypeDataSource_Selecting"
	OnSelected="PageTypeDataSource_Selected"
	OnUpdating="PageTypeDataSource_Updating"
	OnUpdated="PageTypeDataSource_Updated"
	runat="server">
</asp:ObjectDataSource>
<!-- DetailsView -->
	<asp:DetailsView ID="PageTypeDetailsView"
		DataKeyNames="PageTypeID"
		DataSourceID="PageTypeDataSource"
		DefaultMode="Edit"
		GridLines="None"
		AutoGenerateRows="False" CssClass="ewDetailTable"
		OnInit="PageTypeDetailsView_Init"
		OnDataBound="PageTypeDetailsView_DataBound"
		OnItemUpdating="PageTypeDetailsView_ItemUpdating"
		OnItemUpdated="PageTypeDetailsView_ItemUpdated"
		OnUnload="PageTypeDetailsView_Unload"
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
					<asp:Label runat="server" id="xs_PageTypeID">Page Type ID</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:Label id="x_PageTypeID" Text='<%# Eval("PageTypeID")%>' CssClass="aspnetmaker" runat="server" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_PageTypeCD">Page Type CD</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_PageTypeCD" Columns="30" MaxLength="50" CssClass="aspnetmaker" runat="server" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_PageTypeDesc">Page Type Desc</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_PageTypeDesc" Columns="30" MaxLength="50" CssClass="aspnetmaker" runat="server" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_PageFileName">Page File Name</asp:Label>
				</HeaderTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_PageFileName" Columns="30" MaxLength="50" CssClass="aspnetmaker" runat="server" />
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
