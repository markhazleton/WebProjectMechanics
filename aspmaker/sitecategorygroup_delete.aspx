<%@ Page EnableViewState="true" Language="VB" ValidateRequest="False" Debug="True" MasterPageFile="masterpage.master" %>
<%@ MasterType VirtualPath="masterpage.master" %>
<%@ Import Namespace="PMGEN" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="EW.Data" %>
<%@ Import Namespace="EW.Data.Utilities" %>
<%@ Import Namespace="EW.Web" %>
<%@ Register NameSpace="PMGEN" TagPrefix="PMGEN" %>
<script runat="server">
	Dim key As SiteCategoryGroupkey ' record key
	Dim oldrow As SiteCategoryGrouprow ' old record
	Dim arrKeys As ArrayList = New ArrayList()
	Dim oldrows As SiteCategoryGrouprows ' old records
	Dim objProfile As TableProfile ' table profile

	' *************************
	' *  Handler for Page Init
	' *************************

	Protected Sub Page_Init(ByVal s As Object, ByVal e As EventArgs)
		If Not mhUser.IsAdmin() Then
		  Session("Login_Link") = Request.RawUrl
		  Response.Redirect("/mhweb/login/login.aspx")
		End If
		objProfile = CustomProfile.GetTable(Share.ProjectName, SiteCategoryGroupinf.TableVar)
	End Sub	

	' *************************
	' *  Handler for Page Load
	' *************************

	Protected Sub Page_Load(ByVal s As Object, ByVal e As EventArgs)
		Response.Cache.SetCacheability(HttpCacheability.NoCache)
		If (Not Page.IsPostBack) Then
		If (Request.QueryString.Count > 0) Then
			key = New SiteCategoryGroupkey()
			Dim messageList As ArrayList = SiteCategoryGroupinf.LoadKey(key)
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
			key = TryCast(ViewState("key"), SiteCategoryGroupkey)
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

	Protected Sub SiteCategoryGroupDetailsView_DataBound(ByVal sender As Object, ByVal e As EventArgs)
		Dim source As DetailsView = SiteCategoryGroupDetailsView
		Dim row As SiteCategoryGrouprow = TryCast(SiteCategoryGroupDetailsView.DataItem, SiteCategoryGrouprow) 'get data object
		RowToControl(row, source, Core.CtrlType.View)
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

	' ***********************************
	' *  Handler for Delete button click
	' ***********************************

	Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim sDataObjectTypeName As String = String.Empty
		sDataObjectTypeName = SiteCategoryGroupDataSource.DataObjectTypeName
		SiteCategoryGroupDataSource.DataObjectTypeName = ""
		SiteCategoryGroupDataSource.Delete()
		SiteCategoryGroupDataSource.DataObjectTypeName = sDataObjectTypeName
	End Sub

	' ********************************************
	' *  Convert Row Object Value To Input Control
	' ********************************************

	Private Sub RowToControl(ByVal row As SiteCategoryGrouprow, ByVal control As WebControl, ByVal ct As Core.CtrlType)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		If (ct = Core.CtrlType.View) Then ' set up view controls

			' Field SiteCategoryGroupNM
			Dim x_SiteCategoryGroupNM As Label = TryCast(control.FindControl("x_SiteCategoryGroupNM"), Label)
			If (row.SiteCategoryGroupNM IsNot Nothing) Then x_SiteCategoryGroupNM.Text = row.SiteCategoryGroupNM.ToString() Else x_SiteCategoryGroupNM.Text = String.Empty

			' Field SiteCategoryGroupDS
			Dim x_SiteCategoryGroupDS As Label = TryCast(control.FindControl("x_SiteCategoryGroupDS"), Label)
			If (row.SiteCategoryGroupDS IsNot Nothing) Then x_SiteCategoryGroupDS.Text = row.SiteCategoryGroupDS.ToString() Else x_SiteCategoryGroupDS.Text = String.Empty

			' Field SiteCategoryGroupOrder
			Dim x_SiteCategoryGroupOrder As Label = TryCast(control.FindControl("x_SiteCategoryGroupOrder"), Label)
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
	' *  Handler for DataSource Deleting
	' ***********************************

	Protected Sub SiteCategoryGroupDataSource_Deleting(ByVal sender As Object, ByVal e As ObjectDataSourceMethodEventArgs)
		Dim data As SiteCategoryGroupdal = New SiteCategoryGroupdal()
		Dim sWhere As String = String.Empty
		arrKeys.Add(key)
		oldrows = New SiteCategoryGrouprows()
		oldrows.Add(data.LoadRow(key, "")) ' load row without filter
		If (SiteCategoryGroupbll.Deleting(oldrows)) Then
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

	Protected Sub SiteCategoryGroupDataSource_Deleted(ByVal sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.Exception IsNot Nothing) Then
			e.ExceptionHandled = True
			lblMessage.Text = e.Exception.InnerException.Message
		Else
			objProfile.Message = "Delete Successful"
			SiteCategoryGroupbll.Deleted(oldrows)
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
<p><span class="aspnetmaker">Delete from TABLE: Site Category Group</span></p>
<p>
	<asp:Hyperlink id="lnkList" Text="Back to List" NavigateUrl="sitecategorygroup_list.aspx" CssClass="aspnetmaker" runat="server" />
	<asp:Label id="lblReturnUrl" Visible="False" CssClass="aspnetmaker" runat="server">sitecategorygroup_list.aspx</asp:Label>
</p>
<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
	<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
</asp:PlaceHolder>
	<!-- Data Source -->
<asp:ObjectDataSource ID="SiteCategoryGroupDataSource"
	TypeName="PMGEN.SiteCategoryGroupdal"
	DataObjectTypeName="PMGEN.SiteCategoryGroupkey"
	SelectMethod="LoadList"
	DeleteMethod="Delete"
	OnSelecting="SiteCategoryGroupDataSource_Selecting"
	OnSelected="SiteCategoryGroupDataSource_Selected"
	OnDeleting="SiteCategoryGroupDataSource_Deleting"
	OnDeleted="SiteCategoryGroupDataSource_Deleted"
	runat="server">
</asp:ObjectDataSource>
	<!-- DetailsView -->
<asp:DetailsView ID="SiteCategoryGroupDetailsView"
	DataKeyNames="SiteCategoryGroupID"
	DataSourceID="SiteCategoryGroupDataSource"
	DefaultMode="ReadOnly"
	GridLines="None"
	AutoGenerateRows="False" CssClass="ewDetailTable"
	OnInit="SiteCategoryGroupDetailsView_Init"
	OnDataBound="SiteCategoryGroupDetailsView_DataBound"
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
			<asp:Label runat="server" id="xs_SiteCategoryGroupNM">Site Category Group NM</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_SiteCategoryGroupNM" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_SiteCategoryGroupDS">Site Category Group DS</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_SiteCategoryGroupDS" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_SiteCategoryGroupOrder">Site Category Group Order</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_SiteCategoryGroupOrder" CssClass="aspnetmaker" runat="server" />
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
