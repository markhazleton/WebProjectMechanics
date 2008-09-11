<%@ Page EnableViewState="true" Language="VB" ValidateRequest="False" Debug="True" MasterPageFile="masterpage.master" %>
<%@ MasterType VirtualPath="masterpage.master" %>
<%@ Import Namespace="PMGEN" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="EW.Data" %>
<%@ Import Namespace="EW.Data.Utilities" %>
<%@ Import Namespace="EW.Web" %>
<%@ Register NameSpace="PMGEN" TagPrefix="PMGEN" %>
<script runat="server">
	Dim key As Contactkey ' record key
	Dim oldrow As Contactrow ' old record
	Dim arrKeys As ArrayList = New ArrayList()
	Dim oldrows As Contactrows ' old records
	Dim objProfile As TableProfile ' table profile

	' *************************
	' *  Handler for Page Init
	' *************************

	Protected Sub Page_Init(ByVal s As Object, ByVal e As EventArgs)
		If Not mhUser.IsAdmin() Then
		  Session("Login_Link") = Request.RawUrl
		  Response.Redirect("/mhweb/login/login.aspx")
		End If
		objProfile = CustomProfile.GetTable(Share.ProjectName, Contactinf.TableVar)
	End Sub	

	' *************************
	' *  Handler for Page Load
	' *************************

	Protected Sub Page_Load(ByVal s As Object, ByVal e As EventArgs)
		Response.Cache.SetCacheability(HttpCacheability.NoCache)
		If (Not Page.IsPostBack) Then
		If (Request.QueryString.Count > 0) Then
			key = New Contactkey()
			Dim messageList As ArrayList = Contactinf.LoadKey(key)
			If (messageList IsNot Nothing) Then
				objProfile.Message = String.Empty
				For Each sMsg As string In messageList
					objProfile.Message &= sMsg & "<br>"
				Next 
				Response.Redirect(lblReturnUrl.Text)
			End If
			ViewState("key") = key
		Else
			Response.Redirect(lblReturnUrl.Text)
		End If
		Else
			key = TryCast(ViewState("key"), Contactkey)
		End If
		Page.ClientScript.RegisterClientScriptInclude("ewv", "ewv.js")
	End Sub

	' ***********************************
	' *  Handler for Cancel button click
	' ***********************************

	Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs)
		Response.Redirect(lblReturnUrl.Text)
	End Sub

	' ******************************
	' *  Handler for DetailsView Init
	' ******************************

	Protected Sub ContactDetailsView_Init(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			Contactdal.OpenConnection()
		Catch err As Exception
			lblMessage.Text = err.Message
			pnlMessage.Visible = True
		End Try
	End Sub

	' *************************************
	' *  Handler for DetailsView DataBound
	' *************************************

	Protected Sub ContactDetailsView_DataBound(ByVal sender As Object, ByVal e As EventArgs)
		Dim source As DetailsView = ContactDetailsView
		Dim row As Contactrow = TryCast(ContactDetailsView.DataItem, Contactrow) 'get data object
		RowToControl(row, source, Core.CtrlType.View)
	End Sub

	' *********************************
	' *  Handler for DetailsView Unload
	' *********************************

	Protected Sub ContactDetailsView_Unload(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			Contactdal.CloseAndDisposeConnection()
		Catch
		End Try
	End Sub

	' ***********************************
	' *  Handler for Delete button click
	' ***********************************

	Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim sDataObjectTypeName As String = String.Empty
		sDataObjectTypeName = ContactDataSource.DataObjectTypeName
		ContactDataSource.DataObjectTypeName = ""
		ContactDataSource.Delete()
		ContactDataSource.DataObjectTypeName = sDataObjectTypeName
	End Sub

	' ********************************************
	' *  Convert Row Object Value To Input Control
	' ********************************************

	Private Sub RowToControl(ByVal row As Contactrow, ByVal control As WebControl, ByVal ct As Core.CtrlType)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		If (ct = Core.CtrlType.View) Then ' set up view controls

			' Field PrimaryContact
			Dim x_PrimaryContact As Label = TryCast(control.FindControl("x_PrimaryContact"), Label)
			If (row.PrimaryContact IsNot Nothing) Then x_PrimaryContact.Text = row.PrimaryContact.ToString() Else x_PrimaryContact.Text = String.Empty

			' Field LogonName
			Dim x_LogonName As Label = TryCast(control.FindControl("x_LogonName"), Label)
			If (row.LogonName IsNot Nothing) Then x_LogonName.Text = row.LogonName.ToString() Else x_LogonName.Text = String.Empty

			' Field LogonPassword
			Dim x_LogonPassword As Label = TryCast(control.FindControl("x_LogonPassword"), Label)
			If (row.LogonPassword IsNot Nothing) Then x_LogonPassword.Text = row.LogonPassword.ToString() Else x_LogonPassword.Text = String.Empty

			' Field EMail
			Dim x_EMail As Label = TryCast(control.FindControl("x_EMail"), Label)
			If (row.EMail IsNot Nothing) Then x_EMail.Text = row.EMail.ToString() Else x_EMail.Text = String.Empty

			' Field CompanyID
			Dim x_CompanyID As Label = TryCast(control.FindControl("x_CompanyID"), Label)
			Dim v_CompanyID As String
			If (row.CompanyID.HasValue) Then v_CompanyID = row.CompanyID.ToString() Else v_CompanyID = String.Empty
			x_CompanyID.Text = Contactdal.LookUpTable("CompanyID", v_CompanyID)

			' Field Active
			Dim x_Active As CheckBox = TryCast(control.FindControl("x_Active"), CheckBox)
			If (row.Active.HasValue) Then
				x_Active.Checked = IIf(CType(row.Active, Boolean), True, False)
			End If

			' Field GroupID
			Dim x_GroupID As Label = TryCast(control.FindControl("x_GroupID"), Label)
			Dim v_GroupID As String
			If (row.GroupID.HasValue) Then v_GroupID = row.GroupID.ToString() Else v_GroupID = String.Empty
			x_GroupID.Text = Contactdal.LookUpTable("GroupID", v_GroupID)

			' Field CreateDT
			Dim x_CreateDT As Label = TryCast(control.FindControl("x_CreateDT"), Label)
			If (row.CreateDT.HasValue) Then x_CreateDT.Text = Convert.ToString(row.CreateDT) Else x_CreateDT.Text = String.Empty
			x_CreateDT.Text = DataFormat.DateTimeFormat(6, "/", x_CreateDT.Text)

			' Field TemplatePrefix
			Dim x_TemplatePrefix As Label = TryCast(control.FindControl("x_TemplatePrefix"), Label)
			Dim v_TemplatePrefix As String
			If (row.TemplatePrefix IsNot Nothing) Then v_TemplatePrefix = row.TemplatePrefix.ToString() Else v_TemplatePrefix = String.Empty
			x_TemplatePrefix.Text = Contactdal.LookUpTable("TemplatePrefix", v_TemplatePrefix)

			' Field RoleID
			Dim x_RoleID As Label = TryCast(control.FindControl("x_RoleID"), Label)
			Dim v_RoleID As String
			If (row.RoleID.HasValue) Then v_RoleID = row.RoleID.ToString() Else v_RoleID = String.Empty
			x_RoleID.Text = Contactdal.LookUpTable("RoleID", v_RoleID)
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

	Protected Sub ContactDataSource_Selecting(Byval sender As Object, ByVal e As ObjectDataSourceSelectingEventArgs)
		e.InputParameters.Clear()
		e.InputParameters.Add("filter", Contactinf.GetUserFilter())
	End Sub

	' *************************************
	' *  DataSource Selected Event Handler
	' *************************************

	Protected Sub ContactDataSource_Selected(Byval sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.ReturnValue Is Nothing) Then
			If (e.Exception IsNot Nothing) Then
				lblMessage.Text = e.Exception.Message
				pnlMessage.Visible = True
			Else
				lblMessage.Text = "No records found"
				pnlMessage.Visible = True
			End If
		Else
			Dim rows As Contactrows = TryCast(e.ReturnValue, Contactrows)
			Dim i As Integer = 0
			Do While (rows IsNot Nothing AndAlso i < rows.Count)
				If (key.Equals(rows(i).GetKey())) Then
					ContactDetailsView.PageIndex = i
					Exit Do
				End If
				i += 1
			Loop
		End If
	End Sub

	' ***********************************
	' *  Handler for DataSource Deleting
	' ***********************************

	Protected Sub ContactDataSource_Deleting(ByVal sender As Object, ByVal e As ObjectDataSourceMethodEventArgs)
		Dim data As Contactdal = New Contactdal()
		Dim sWhere As String = String.Empty
		arrKeys.Add(key)
		oldrows = New Contactrows()
		oldrows.Add(data.LoadRow(key, "")) ' load row without filter
		If (Contactbll.Deleting(oldrows)) Then
			e.InputParameters.Clear()
			e.InputParameters.Add("keys", arrKeys)
		Else
			e.Cancel = True
			If (Not String.IsNullOrEmpty(objProfile.Message)) Then
				lblMessage.Text = objProfile.Message
				pnlMessage.Visible = True
			End If
		End If
	End Sub

	' **********************************
	' *  Handler for DataSource Deleted
	' **********************************

	Protected Sub ContactDataSource_Deleted(ByVal sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.Exception IsNot Nothing) Then
			e.ExceptionHandled = True
			lblMessage.Text = e.Exception.InnerException.Message
		Else
			objProfile.Message = "Delete Successful"
			Contactbll.Deleted(oldrows)
			Response.Redirect(lblReturnUrl.Text)
		End If
		If (lblMessage.Text <> "") Then
			pnlMessage.Visible = True
		End If
	End Sub

	' ************************************************
	' *  Change the page index for DetailsView paging
	' ************************************************

	Protected Sub ChangePageIndex(ByVal sender As Object, ByVal e As DetailsViewPageEventArgs)
		Dim data As Contactdal = New Contactdal()
		Dim rows As Contactrows = data.LoadList(Contactinf.GetUserFilter())
		If (rows IsNot Nothing AndAlso rows.Count >= e.NewPageIndex) Then
			key = rows(e.NewPageIndex).GetKey()
		End If
	End Sub

	' *******************
	' *  Get User Filter
	' *******************

	Private Function GetUserFilter() As String 
		Return Contactinf.GetUserFilter()
	End Function
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
<p><span class="aspnetmaker">Delete from TABLE: Contact</span></p>
<p>
	<asp:Hyperlink id="lnkList" Text="Back to List" NavigateUrl="contact_list.aspx" CssClass="aspnetmaker" runat="server" />
	<asp:Label id="lblReturnUrl" Visible="False" CssClass="aspnetmaker" runat="server">contact_list.aspx</asp:Label>
</p>
<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
	<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
</asp:PlaceHolder>
	<!-- Data Source -->
<asp:ObjectDataSource ID="ContactDataSource"
	TypeName="PMGEN.Contactdal"
	DataObjectTypeName="PMGEN.Contactkey"
	SelectMethod="LoadList"
	DeleteMethod="Delete"
	OnSelecting="ContactDataSource_Selecting"
	OnSelected="ContactDataSource_Selected"
	OnDeleting="ContactDataSource_Deleting"
	OnDeleted="ContactDataSource_Deleted"
	runat="server">
</asp:ObjectDataSource>
	<!-- DetailsView -->
<asp:DetailsView ID="ContactDetailsView"
	DataKeyNames="ContactID"
	DataSourceID="ContactDataSource"
	DefaultMode="ReadOnly"
	GridLines="None"
	AutoGenerateRows="False" CssClass="ewDetailTable"
	OnInit="ContactDetailsView_Init"
	OnDataBound="ContactDetailsView_DataBound"
	OnUnload="ContactDetailsView_Unload"
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
			<asp:Label runat="server" id="xs_PrimaryContact">Primary Contact</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_PrimaryContact" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_LogonName">Logon Name</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_LogonName" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_LogonPassword">Logon Password</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_LogonPassword" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_EMail">EMail</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_EMail" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_CompanyID">Company</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_CompanyID" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_Active">Active</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:CheckBox id="x_Active" Enabled="False"  CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_GroupID">Group</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_GroupID" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_CreateDT">Modified</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_CreateDT" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_TemplatePrefix">Template</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_TemplatePrefix" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_RoleID">Role ID</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_RoleID" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField ShowHeader="False">
		<ItemStyle CssClass="ewTableFooter" />
		<ItemTemplate>
		<table border="0">
		<td><asp:LinkButton ID="btnDelete" runat="server" CausesValidation="False" Text="Delete"  OnClick="btnDelete_Click"></asp:LinkButton></td>
		<td><asp:LinkButton ID="btnCancel" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel" OnClick="btnCancel_Click"></asp:LinkButton></td>
		</table>
		</ItemTemplate>
	</asp:TemplateField>
	</Fields>
</asp:DetailsView>
<br />
</asp:Content>
