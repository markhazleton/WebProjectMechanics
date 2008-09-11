<%@ Page EnableViewState="true" Language="VB" ValidateRequest="False" EnableEventValidation="False" Debug="True" MasterPageFile="masterpage.master" %>
<%@ MasterType VirtualPath="masterpage.master" %>
<%@ Import Namespace="PMGEN" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="EW.Data" %>
<%@ Import Namespace="EW.Data.Utilities" %>
<%@ Import Namespace="EW.Web" %>
<%@ Register NameSpace="PMGEN" TagPrefix="PMGEN" %>
<script runat="server">
	Dim key As Pagekey ' record key
	Dim oldrow As Pagerow ' old record
	Dim arrKeys As ArrayList = New ArrayList()
	Dim oldrows As Pagerows ' old records
	Dim objProfile As TableProfile ' table profile

	' *************************
	' *  Handler for Page Init
	' *************************

	Protected Sub Page_Init(ByVal s As Object, ByVal e As EventArgs)
		If Not mhUser.IsAdmin() Then
		  Session("Login_Link") = Request.RawUrl
		  Response.Redirect("/mhweb/login/login.aspx")
		End If
		objProfile = CustomProfile.GetTable(Share.ProjectName, Pageinf.TableVar)
	End Sub	

	' *************************
	' *  Handler for Page Load
	' *************************

	Protected Sub Page_Load(ByVal s As Object, ByVal e As EventArgs)
		Response.Cache.SetCacheability(HttpCacheability.NoCache)
		If (Not Page.IsPostBack) Then
		If (Request.QueryString.Count > 0) Then
			key = New Pagekey()
			Dim messageList As ArrayList = Pageinf.LoadKey(key)
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
			key = TryCast(ViewState("key"), Pagekey)
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

	Protected Sub PageDetailsView_Init(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			Pagedal.OpenConnection()
		Catch err As Exception
			lblMessage.Text = err.Message
			pnlMessage.Visible = True
		End Try
	End Sub

	' *************************************
	' *  Handler for DetailsView DataBound
	' *************************************

	Protected Sub PageDetailsView_DataBound(ByVal sender As Object, ByVal e As EventArgs)
		Dim source As DetailsView = PageDetailsView
		Dim row As Pagerow = TryCast(PageDetailsView.DataItem, Pagerow) 'get data object
		RowToControl(row, source, Core.CtrlType.View)
	End Sub

	' *********************************
	' *  Handler for DetailsView Unload
	' *********************************

	Protected Sub PageDetailsView_Unload(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			Pagedal.CloseAndDisposeConnection()
		Catch
		End Try
	End Sub

	' ***********************************
	' *  Handler for Delete button click
	' ***********************************

	Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim sDataObjectTypeName As String = String.Empty
		sDataObjectTypeName = PageDataSource.DataObjectTypeName
		PageDataSource.DataObjectTypeName = ""
		PageDataSource.Delete()
		PageDataSource.DataObjectTypeName = sDataObjectTypeName
	End Sub

	' ********************************************
	' *  Convert Row Object Value To Input Control
	' ********************************************

	Private Sub RowToControl(ByVal row As Pagerow, ByVal control As WebControl, ByVal ct As Core.CtrlType)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		If (ct = Core.CtrlType.View) Then ' set up view controls

			' Field PageOrder
			Dim x_PageOrder As Label = TryCast(control.FindControl("x_PageOrder"), Label)
			If (row.PageOrder.HasValue) Then x_PageOrder.Text = row.PageOrder.ToString() Else x_PageOrder.Text = String.Empty

			' Field PageName
			Dim x_PageName As Label = TryCast(control.FindControl("x_PageName"), Label)
			If (row.PageName IsNot Nothing) Then x_PageName.Text = row.PageName.ToString() Else x_PageName.Text = String.Empty

			' Field PageTitle
			Dim x_PageTitle As Label = TryCast(control.FindControl("x_PageTitle"), Label)
			If (row.PageTitle IsNot Nothing) Then x_PageTitle.Text = row.PageTitle.ToString() Else x_PageTitle.Text = String.Empty

			' Field PageDescription
			Dim x_PageDescription As Label = TryCast(control.FindControl("x_PageDescription"), Label)
			If (row.PageDescription IsNot Nothing) Then x_PageDescription.Text = row.PageDescription.ToString() Else x_PageDescription.Text = String.Empty

			' Field PageKeywords
			Dim x_PageKeywords As Label = TryCast(control.FindControl("x_PageKeywords"), Label)
			If (row.PageKeywords IsNot Nothing) Then x_PageKeywords.Text = row.PageKeywords.ToString() Else x_PageKeywords.Text = String.Empty

			' Field PageTypeID
			Dim x_PageTypeID As Label = TryCast(control.FindControl("x_PageTypeID"), Label)
			Dim v_PageTypeID As String
			If (row.PageTypeID.HasValue) Then v_PageTypeID = row.PageTypeID.ToString() Else v_PageTypeID = String.Empty
			x_PageTypeID.Text = Pagedal.LookUpTable("PageTypeID", v_PageTypeID)

			' Field ImagesPerRow
			Dim x_ImagesPerRow As Label = TryCast(control.FindControl("x_ImagesPerRow"), Label)
			If (row.ImagesPerRow.HasValue) Then x_ImagesPerRow.Text = row.ImagesPerRow.ToString() Else x_ImagesPerRow.Text = String.Empty

			' Field RowsPerPage
			Dim x_RowsPerPage As Label = TryCast(control.FindControl("x_RowsPerPage"), Label)
			If (row.RowsPerPage.HasValue) Then x_RowsPerPage.Text = row.RowsPerPage.ToString() Else x_RowsPerPage.Text = String.Empty

			' Field ParentPageID
			Dim x_ParentPageID As Label = TryCast(control.FindControl("x_ParentPageID"), Label)
			Dim v_ParentPageID As String
			If (row.ParentPageID.HasValue) Then v_ParentPageID = row.ParentPageID.ToString() Else v_ParentPageID = String.Empty
			x_ParentPageID.Text = Pagedal.LookUpTable("ParentPageID", v_ParentPageID)

			' Field Active
			Dim x_Active As CheckBox = TryCast(control.FindControl("x_Active"), CheckBox)
			If (row.Active.HasValue) Then
				x_Active.Checked = IIf(CType(row.Active, Boolean), True, False)
			End If

			' Field CompanyID
			Dim x_CompanyID As Label = TryCast(control.FindControl("x_CompanyID"), Label)
			Dim v_CompanyID As String
			If (row.CompanyID.HasValue) Then v_CompanyID = row.CompanyID.ToString() Else v_CompanyID = String.Empty
			x_CompanyID.Text = Pagedal.LookUpTable("CompanyID", v_CompanyID)

			' Field PageFileName
			Dim x_PageFileName As Label = TryCast(control.FindControl("x_PageFileName"), Label)
			If (row.PageFileName IsNot Nothing) Then x_PageFileName.Text = row.PageFileName.ToString() Else x_PageFileName.Text = String.Empty

			' Field GroupID
			Dim x_GroupID As Label = TryCast(control.FindControl("x_GroupID"), Label)
			Dim v_GroupID As String
			If (row.GroupID.HasValue) Then v_GroupID = row.GroupID.ToString() Else v_GroupID = String.Empty
			x_GroupID.Text = Pagedal.LookUpTable("GroupID", v_GroupID)

			' Field ModifiedDT
			Dim x_ModifiedDT As Label = TryCast(control.FindControl("x_ModifiedDT"), Label)
			If (row.ModifiedDT.HasValue) Then x_ModifiedDT.Text = Convert.ToString(row.ModifiedDT) Else x_ModifiedDT.Text = String.Empty
			x_ModifiedDT.Text = DataFormat.DateTimeFormat(6, "/", x_ModifiedDT.Text)

			' Field VersionNo
			Dim x_VersionNo As Label = TryCast(control.FindControl("x_VersionNo"), Label)
			If (row.VersionNo.HasValue) Then x_VersionNo.Text = row.VersionNo.ToString() Else x_VersionNo.Text = String.Empty

			' Field SiteCategoryID
			Dim x_SiteCategoryID As Label = TryCast(control.FindControl("x_SiteCategoryID"), Label)
			Dim v_SiteCategoryID As String
			If (row.SiteCategoryID.HasValue) Then v_SiteCategoryID = row.SiteCategoryID.ToString() Else v_SiteCategoryID = String.Empty
			x_SiteCategoryID.Text = Pagedal.LookUpTable("SiteCategoryID", v_SiteCategoryID)

			' Field SiteCategoryGroupID
			Dim x_SiteCategoryGroupID As Label = TryCast(control.FindControl("x_SiteCategoryGroupID"), Label)
			Dim v_SiteCategoryGroupID As String
			If (row.SiteCategoryGroupID.HasValue) Then v_SiteCategoryGroupID = row.SiteCategoryGroupID.ToString() Else v_SiteCategoryGroupID = String.Empty
			x_SiteCategoryGroupID.Text = Pagedal.LookUpTable("SiteCategoryGroupID", v_SiteCategoryGroupID)
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

	Protected Sub PageDataSource_Selecting(Byval sender As Object, ByVal e As ObjectDataSourceSelectingEventArgs)
		e.InputParameters.Clear()
		e.InputParameters.Add("filter", Pageinf.GetUserFilter())
	End Sub

	' *************************************
	' *  DataSource Selected Event Handler
	' *************************************

	Protected Sub PageDataSource_Selected(Byval sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.ReturnValue Is Nothing) Then
			If (e.Exception IsNot Nothing) Then
				lblMessage.Text = e.Exception.Message
				pnlMessage.Visible = True
			Else
				lblMessage.Text = "No records found"
				pnlMessage.Visible = True
			End If
		Else
			Dim rows As Pagerows = TryCast(e.ReturnValue, Pagerows)
			Dim i As Integer = 0
			Do While (rows IsNot Nothing AndAlso i < rows.Count)
				If (key.Equals(rows(i).GetKey())) Then
					PageDetailsView.PageIndex = i
					Exit Do
				End If
				i += 1
			Loop
		End If
	End Sub

	' ***********************************
	' *  Handler for DataSource Deleting
	' ***********************************

	Protected Sub PageDataSource_Deleting(ByVal sender As Object, ByVal e As ObjectDataSourceMethodEventArgs)
		Dim data As Pagedal = New Pagedal()
		Dim sWhere As String = String.Empty
		arrKeys.Add(key)
		oldrows = New Pagerows()
		oldrows.Add(data.LoadRow(key, "")) ' load row without filter
		If (Pagebll.Deleting(oldrows)) Then
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

	Protected Sub PageDataSource_Deleted(ByVal sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.Exception IsNot Nothing) Then
			e.ExceptionHandled = True
			lblMessage.Text = e.Exception.InnerException.Message
		Else
			objProfile.Message = "Delete Successful"
			Pagebll.Deleted(oldrows)
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
		Dim data As Pagedal = New Pagedal()
		Dim rows As Pagerows = data.LoadList(Pageinf.GetUserFilter())
		If (rows IsNot Nothing AndAlso rows.Count >= e.NewPageIndex) Then
			key = rows(e.NewPageIndex).GetKey()
		End If
	End Sub

	' *******************
	' *  Get User Filter
	' *******************

	Private Function GetUserFilter() As String 
		Return Pageinf.GetUserFilter()
	End Function
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
<p><span class="aspnetmaker">Delete from TABLE: Page</span></p>
<p>
	<asp:Hyperlink id="lnkList" Text="Back to List" NavigateUrl="page_list.aspx" CssClass="aspnetmaker" runat="server" />
	<asp:Label id="lblReturnUrl" Visible="False" CssClass="aspnetmaker" runat="server">page_list.aspx</asp:Label>
</p>
<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
	<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
</asp:PlaceHolder>
	<!-- Data Source -->
<asp:ObjectDataSource ID="PageDataSource"
	TypeName="PMGEN.Pagedal"
	DataObjectTypeName="PMGEN.Pagekey"
	SelectMethod="LoadList"
	DeleteMethod="Delete"
	OnSelecting="PageDataSource_Selecting"
	OnSelected="PageDataSource_Selected"
	OnDeleting="PageDataSource_Deleting"
	OnDeleted="PageDataSource_Deleted"
	runat="server">
</asp:ObjectDataSource>
	<!-- DetailsView -->
<asp:DetailsView ID="PageDetailsView"
	DataKeyNames="PageID"
	DataSourceID="PageDataSource"
	DefaultMode="ReadOnly"
	GridLines="None"
	AutoGenerateRows="False" CssClass="ewDetailTable"
	OnInit="PageDetailsView_Init"
	OnDataBound="PageDetailsView_DataBound"
	OnUnload="PageDetailsView_Unload"
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
			<asp:Label runat="server" id="xs_PageOrder">Order</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_PageOrder" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_PageName">Name</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_PageName" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_PageTitle">Title</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_PageTitle" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_PageDescription">Description</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_PageDescription" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_PageKeywords">Keywords</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_PageKeywords" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_PageTypeID">Type</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_PageTypeID" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_ImagesPerRow">Images Per Row</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_ImagesPerRow" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_RowsPerPage">Rows Per Page</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_RowsPerPage" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_ParentPageID">Parent</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_ParentPageID" CssClass="aspnetmaker" runat="server" />
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
			<asp:Label runat="server" id="xs_CompanyID">Company ID</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_CompanyID" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_PageFileName">File Name</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_PageFileName" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_GroupID">Security Group</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_GroupID" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_ModifiedDT">Modified DT</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_ModifiedDT" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_VersionNo">Version No</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_VersionNo" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_SiteCategoryID">Site Category</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_SiteCategoryID" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_SiteCategoryGroupID">Site Group </asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_SiteCategoryGroupID" CssClass="aspnetmaker" runat="server" />
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
