<%@ Page EnableViewState="true" Language="VB" ValidateRequest="False" Debug="True" MasterPageFile="masterpage.master" %>
<%@ MasterType VirtualPath="masterpage.master" %>
<%@ Import Namespace="PMGEN" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="EW.Data" %>
<%@ Import Namespace="EW.Data.Utilities" %>
<%@ Import Namespace="EW.Web" %>
<%@ Register NameSpace="PMGEN" TagPrefix="PMGEN" %>
<script runat="server">
	Dim key As SiteCategoryTypekey ' record key
	Dim oldrow As SiteCategoryTyperow ' old record
	Dim arrKeys As ArrayList = New ArrayList()
	Dim oldrows As SiteCategoryTyperows ' old records
	Dim objProfile As TableProfile ' table profile

	' *************************
	' *  Handler for Page Init
	' *************************

	Protected Sub Page_Init(ByVal s As Object, ByVal e As EventArgs)
		If Not mhUser.IsAdmin() Then
		  Session("Login_Link") = Request.RawUrl
		  Response.Redirect("/mhweb/login/login.aspx")
		End If
		objProfile = CustomProfile.GetTable(Share.ProjectName, SiteCategoryTypeinf.TableVar)
	End Sub	

	' *************************
	' *  Handler for Page Load
	' *************************

	Protected Sub Page_Load(ByVal s As Object, ByVal e As EventArgs)
		Response.Cache.SetCacheability(HttpCacheability.NoCache)
		If (Not Page.IsPostBack) Then
		If (Request.QueryString.Count > 0) Then
			key = New SiteCategoryTypekey()
			Dim messageList As ArrayList = SiteCategoryTypeinf.LoadKey(key)
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
			key = TryCast(ViewState("key"), SiteCategoryTypekey)
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

	Protected Sub SiteCategoryTypeDetailsView_Init(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			SiteCategoryTypedal.OpenConnection()
		Catch err As Exception
			lblMessage.Text = err.Message
			pnlMessage.Visible = True
		End Try
	End Sub

	' *************************************
	' *  Handler for DetailsView DataBound
	' *************************************

	Protected Sub SiteCategoryTypeDetailsView_DataBound(ByVal sender As Object, ByVal e As EventArgs)
		Dim source As DetailsView = SiteCategoryTypeDetailsView
		Dim row As SiteCategoryTyperow = TryCast(SiteCategoryTypeDetailsView.DataItem, SiteCategoryTyperow) 'get data object
		RowToControl(row, source, Core.CtrlType.View)
	End Sub

	' *********************************
	' *  Handler for DetailsView Unload
	' *********************************

	Protected Sub SiteCategoryTypeDetailsView_Unload(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			SiteCategoryTypedal.CloseAndDisposeConnection()
		Catch
		End Try
	End Sub

	' ***********************************
	' *  Handler for Delete button click
	' ***********************************

	Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim sDataObjectTypeName As String = String.Empty
		sDataObjectTypeName = SiteCategoryTypeDataSource.DataObjectTypeName
		SiteCategoryTypeDataSource.DataObjectTypeName = ""
		SiteCategoryTypeDataSource.Delete()
		SiteCategoryTypeDataSource.DataObjectTypeName = sDataObjectTypeName
	End Sub

	' ********************************************
	' *  Convert Row Object Value To Input Control
	' ********************************************

	Private Sub RowToControl(ByVal row As SiteCategoryTyperow, ByVal control As WebControl, ByVal ct As Core.CtrlType)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		If (ct = Core.CtrlType.View) Then ' set up view controls

			' Field SiteCategoryTypeNM
			Dim x_SiteCategoryTypeNM As Label = TryCast(control.FindControl("x_SiteCategoryTypeNM"), Label)
			If (row.SiteCategoryTypeNM IsNot Nothing) Then x_SiteCategoryTypeNM.Text = row.SiteCategoryTypeNM.ToString() Else x_SiteCategoryTypeNM.Text = String.Empty

			' Field SiteCategoryTypeDS
			Dim x_SiteCategoryTypeDS As Label = TryCast(control.FindControl("x_SiteCategoryTypeDS"), Label)
			If (row.SiteCategoryTypeDS IsNot Nothing) Then x_SiteCategoryTypeDS.Text = row.SiteCategoryTypeDS.ToString() Else x_SiteCategoryTypeDS.Text = String.Empty

			' Field SiteCategoryComment
			Dim x_SiteCategoryComment As Label = TryCast(control.FindControl("x_SiteCategoryComment"), Label)
			If (row.SiteCategoryComment IsNot Nothing) Then x_SiteCategoryComment.Text = row.SiteCategoryComment.ToString() Else x_SiteCategoryComment.Text = String.Empty

			' Field SiteCategoryFileName
			Dim x_SiteCategoryFileName As Label = TryCast(control.FindControl("x_SiteCategoryFileName"), Label)
			If (row.SiteCategoryFileName IsNot Nothing) Then x_SiteCategoryFileName.Text = row.SiteCategoryFileName.ToString() Else x_SiteCategoryFileName.Text = String.Empty

			' Field SiteCategoryTransferURL
			Dim x_SiteCategoryTransferURL As Label = TryCast(control.FindControl("x_SiteCategoryTransferURL"), Label)
			If (row.SiteCategoryTransferURL IsNot Nothing) Then x_SiteCategoryTransferURL.Text = row.SiteCategoryTransferURL.ToString() Else x_SiteCategoryTransferURL.Text = String.Empty

			' Field DefaultSiteCategoryID
			Dim x_DefaultSiteCategoryID As Label = TryCast(control.FindControl("x_DefaultSiteCategoryID"), Label)
			Dim v_DefaultSiteCategoryID As String
			If (row.DefaultSiteCategoryID.HasValue) Then v_DefaultSiteCategoryID = row.DefaultSiteCategoryID.ToString() Else v_DefaultSiteCategoryID = String.Empty
			x_DefaultSiteCategoryID.Text = SiteCategoryTypedal.LookUpTable("DefaultSiteCategoryID", v_DefaultSiteCategoryID)
		End If
	End Sub

	' ************************************
	' *  Handler for DataSource Selecting
	' ************************************

	Protected Sub SiteCategoryTypeDataSource_Selecting(Byval sender As Object, ByVal e As ObjectDataSourceSelectingEventArgs)
		e.InputParameters.Clear()
		e.InputParameters.Add("filter", SiteCategoryTypeinf.GetUserFilter())
	End Sub

	' *************************************
	' *  DataSource Selected Event Handler
	' *************************************

	Protected Sub SiteCategoryTypeDataSource_Selected(Byval sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.ReturnValue Is Nothing) Then
			If (e.Exception IsNot Nothing) Then
				lblMessage.Text = e.Exception.Message
				pnlMessage.Visible = True
			Else
				lblMessage.Text = "No records found"
				pnlMessage.Visible = True
			End If
		Else
			Dim rows As SiteCategoryTyperows = TryCast(e.ReturnValue, SiteCategoryTyperows)
			Dim i As Integer = 0
			Do While (rows IsNot Nothing AndAlso i < rows.Count)
				If (key.Equals(rows(i).GetKey())) Then
					SiteCategoryTypeDetailsView.PageIndex = i
					Exit Do
				End If
				i += 1
			Loop
		End If
	End Sub

	' ***********************************
	' *  Handler for DataSource Deleting
	' ***********************************

	Protected Sub SiteCategoryTypeDataSource_Deleting(ByVal sender As Object, ByVal e As ObjectDataSourceMethodEventArgs)
		Dim data As SiteCategoryTypedal = New SiteCategoryTypedal()
		Dim sWhere As String = String.Empty
		arrKeys.Add(key)
		oldrows = New SiteCategoryTyperows()
		oldrows.Add(data.LoadRow(key, "")) ' load row without filter
		If (SiteCategoryTypebll.Deleting(oldrows)) Then
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

	Protected Sub SiteCategoryTypeDataSource_Deleted(ByVal sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.Exception IsNot Nothing) Then
			e.ExceptionHandled = True
			lblMessage.Text = e.Exception.InnerException.Message
		Else
			objProfile.Message = "Delete Successful"
			SiteCategoryTypebll.Deleted(oldrows)
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
		Dim data As SiteCategoryTypedal = New SiteCategoryTypedal()
		Dim rows As SiteCategoryTyperows = data.LoadList(SiteCategoryTypeinf.GetUserFilter())
		If (rows IsNot Nothing AndAlso rows.Count >= e.NewPageIndex) Then
			key = rows(e.NewPageIndex).GetKey()
		End If
	End Sub

	' *******************
	' *  Get User Filter
	' *******************

	Private Function GetUserFilter() As String 
		Return SiteCategoryTypeinf.GetUserFilter()
	End Function
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
<p><span class="aspnetmaker">Delete from TABLE: Site Type</span></p>
<p>
	<asp:Hyperlink id="lnkList" Text="Back to List" NavigateUrl="sitecategorytype_list.aspx" CssClass="aspnetmaker" runat="server" />
	<asp:Label id="lblReturnUrl" Visible="False" CssClass="aspnetmaker" runat="server">sitecategorytype_list.aspx</asp:Label>
</p>
<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
	<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
</asp:PlaceHolder>
	<!-- Data Source -->
<asp:ObjectDataSource ID="SiteCategoryTypeDataSource"
	TypeName="PMGEN.SiteCategoryTypedal"
	DataObjectTypeName="PMGEN.SiteCategoryTypekey"
	SelectMethod="LoadList"
	DeleteMethod="Delete"
	OnSelecting="SiteCategoryTypeDataSource_Selecting"
	OnSelected="SiteCategoryTypeDataSource_Selected"
	OnDeleting="SiteCategoryTypeDataSource_Deleting"
	OnDeleted="SiteCategoryTypeDataSource_Deleted"
	runat="server">
</asp:ObjectDataSource>
	<!-- DetailsView -->
<asp:DetailsView ID="SiteCategoryTypeDetailsView"
	DataKeyNames="SiteCategoryTypeID"
	DataSourceID="SiteCategoryTypeDataSource"
	DefaultMode="ReadOnly"
	GridLines="None"
	AutoGenerateRows="False" CssClass="ewDetailTable"
	OnInit="SiteCategoryTypeDetailsView_Init"
	OnDataBound="SiteCategoryTypeDetailsView_DataBound"
	OnUnload="SiteCategoryTypeDetailsView_Unload"
	AllowPaging="True"
	OnPageIndexChanging="ChangePageIndex"
	PagerSettings-Mode="NumericFirstLast"
	PagerSettings-Position="Bottom"
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
			<asp:Label runat="server" id="xs_SiteCategoryTypeNM">Site Category Type NM</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_SiteCategoryTypeNM" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_SiteCategoryTypeDS">Site Category Type DS</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_SiteCategoryTypeDS" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_SiteCategoryComment">Site Category Comment</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_SiteCategoryComment" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_SiteCategoryFileName">Site Category File Name</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_SiteCategoryFileName" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_SiteCategoryTransferURL">Site Category Transfer URL</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_SiteCategoryTransferURL" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_DefaultSiteCategoryID">Default Site Category ID</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_DefaultSiteCategoryID" CssClass="aspnetmaker" runat="server" />
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
