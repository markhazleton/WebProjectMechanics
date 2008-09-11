<%@ Page EnableViewState="true" Language="VB" ValidateRequest="False" Debug="True" MasterPageFile="masterpage.master" %>
<%@ MasterType VirtualPath="masterpage.master" %>
<%@ Import Namespace="PMGEN" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="EW.Data" %>
<%@ Import Namespace="EW.Data.Utilities" %>
<%@ Import Namespace="EW.Web" %>
<%@ Register NameSpace="PMGEN" TagPrefix="PMGEN" %>
<script runat="server">
	Dim key As PageAliaskey ' record key
	Dim oldrow As PageAliasrow ' old record
	Dim arrKeys As ArrayList = New ArrayList()
	Dim oldrows As PageAliasrows ' old records
	Dim objProfile As TableProfile ' table profile

	' *************************
	' *  Handler for Page Init
	' *************************

	Protected Sub Page_Init(ByVal s As Object, ByVal e As EventArgs)
		If Not mhUser.IsAdmin() Then
		  Session("Login_Link") = Request.RawUrl
		  Response.Redirect("/mhweb/login/login.aspx")
		End If
		objProfile = CustomProfile.GetTable(Share.ProjectName, PageAliasinf.TableVar)
	End Sub	

	' *************************
	' *  Handler for Page Load
	' *************************

	Protected Sub Page_Load(ByVal s As Object, ByVal e As EventArgs)
		Response.Cache.SetCacheability(HttpCacheability.NoCache)
		If (Not Page.IsPostBack) Then
		If (Request.QueryString.Count > 0) Then
			key = New PageAliaskey()
			Dim messageList As ArrayList = PageAliasinf.LoadKey(key)
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
			key = TryCast(ViewState("key"), PageAliaskey)
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

	Protected Sub PageAliasDetailsView_DataBound(ByVal sender As Object, ByVal e As EventArgs)
		Dim source As DetailsView = PageAliasDetailsView
		Dim row As PageAliasrow = TryCast(PageAliasDetailsView.DataItem, PageAliasrow) 'get data object
		RowToControl(row, source, Core.CtrlType.View)
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

	' ***********************************
	' *  Handler for Delete button click
	' ***********************************

	Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim sDataObjectTypeName As String = String.Empty
		sDataObjectTypeName = PageAliasDataSource.DataObjectTypeName
		PageAliasDataSource.DataObjectTypeName = ""
		PageAliasDataSource.Delete()
		PageAliasDataSource.DataObjectTypeName = sDataObjectTypeName
	End Sub

	' ********************************************
	' *  Convert Row Object Value To Input Control
	' ********************************************

	Private Sub RowToControl(ByVal row As PageAliasrow, ByVal control As WebControl, ByVal ct As Core.CtrlType)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		If (ct = Core.CtrlType.View) Then ' set up view controls

			' Field PageAliasID
			Dim x_PageAliasID As Label = TryCast(control.FindControl("x_PageAliasID"), Label)
			If (row.PageAliasID.HasValue) Then x_PageAliasID.Text = row.PageAliasID.ToString() Else x_PageAliasID.Text = String.Empty

			' Field PageURL
			Dim x_PageURL As Label = TryCast(control.FindControl("x_PageURL"), Label)
			If (row.PageURL IsNot Nothing) Then x_PageURL.Text = row.PageURL.ToString() Else x_PageURL.Text = String.Empty

			' Field TargetURL
			Dim x_TargetURL As Label = TryCast(control.FindControl("x_TargetURL"), Label)
			If (row.TargetURL IsNot Nothing) Then x_TargetURL.Text = row.TargetURL.ToString() Else x_TargetURL.Text = String.Empty

			' Field AliasType
			Dim x_AliasType As Label = TryCast(control.FindControl("x_AliasType"), Label)
			If (row.AliasType IsNot Nothing) Then x_AliasType.Text = row.AliasType.ToString() Else x_AliasType.Text = String.Empty

			' Field CompanyID
			Dim x_CompanyID As Label = TryCast(control.FindControl("x_CompanyID"), Label)
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
	' *  Handler for DataSource Deleting
	' ***********************************

	Protected Sub PageAliasDataSource_Deleting(ByVal sender As Object, ByVal e As ObjectDataSourceMethodEventArgs)
		Dim data As PageAliasdal = New PageAliasdal()
		Dim sWhere As String = String.Empty
		arrKeys.Add(key)
		oldrows = New PageAliasrows()
		oldrows.Add(data.LoadRow(key, "")) ' load row without filter
		If (PageAliasbll.Deleting(oldrows)) Then
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

	Protected Sub PageAliasDataSource_Deleted(ByVal sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.Exception IsNot Nothing) Then
			e.ExceptionHandled = True
			lblMessage.Text = e.Exception.InnerException.Message
		Else
			objProfile.Message = "Delete Successful"
			PageAliasbll.Deleted(oldrows)
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
<p><span class="aspnetmaker">Delete from TABLE: Page Alias</span></p>
<p>
	<asp:Hyperlink id="lnkList" Text="Back to List" NavigateUrl="pagealias_list.aspx" CssClass="aspnetmaker" runat="server" />
	<asp:Label id="lblReturnUrl" Visible="False" CssClass="aspnetmaker" runat="server">pagealias_list.aspx</asp:Label>
</p>
<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
	<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
</asp:PlaceHolder>
	<!-- Data Source -->
<asp:ObjectDataSource ID="PageAliasDataSource"
	TypeName="PMGEN.PageAliasdal"
	DataObjectTypeName="PMGEN.PageAliaskey"
	SelectMethod="LoadList"
	DeleteMethod="Delete"
	OnSelecting="PageAliasDataSource_Selecting"
	OnSelected="PageAliasDataSource_Selected"
	OnDeleting="PageAliasDataSource_Deleting"
	OnDeleted="PageAliasDataSource_Deleted"
	runat="server">
</asp:ObjectDataSource>
	<!-- DetailsView -->
<asp:DetailsView ID="PageAliasDetailsView"
	DataKeyNames="PageAliasID"
	DataSourceID="PageAliasDataSource"
	DefaultMode="ReadOnly"
	GridLines="None"
	AutoGenerateRows="False" CssClass="ewDetailTable"
	OnInit="PageAliasDetailsView_Init"
	OnDataBound="PageAliasDetailsView_DataBound"
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
		<ItemTemplate>
<asp:Label id="x_PageAliasID" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_PageURL">Page URL</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_PageURL" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_TargetURL">Target URL</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_TargetURL" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_AliasType">Alias Type</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_AliasType" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_CompanyID">Company ID</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_CompanyID" CssClass="aspnetmaker" runat="server" />
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
