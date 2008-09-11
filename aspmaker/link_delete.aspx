<%@ Page EnableViewState="true" Language="VB" ValidateRequest="False" EnableEventValidation="False" Debug="True" MasterPageFile="masterpage.master" %>
<%@ MasterType VirtualPath="masterpage.master" %>
<%@ Import Namespace="PMGEN" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="EW.Data" %>
<%@ Import Namespace="EW.Data.Utilities" %>
<%@ Import Namespace="EW.Web" %>
<%@ Register NameSpace="PMGEN" TagPrefix="PMGEN" %>
<script runat="server">
	Dim key As Linkkey ' record key
	Dim oldrow As Linkrow ' old record
	Dim arrKeys As ArrayList = New ArrayList()
	Dim oldrows As Linkrows ' old records
	Dim objProfile As TableProfile ' table profile

	' *************************
	' *  Handler for Page Init
	' *************************

	Protected Sub Page_Init(ByVal s As Object, ByVal e As EventArgs)
		If Not mhUser.IsAdmin() Then
		  Session("Login_Link") = Request.RawUrl
		  Response.Redirect("/mhweb/login/login.aspx")
		End If
		objProfile = CustomProfile.GetTable(Share.ProjectName, Linkinf.TableVar)
	End Sub	

	' *************************
	' *  Handler for Page Load
	' *************************

	Protected Sub Page_Load(ByVal s As Object, ByVal e As EventArgs)
		Response.Cache.SetCacheability(HttpCacheability.NoCache)
		If (Not Page.IsPostBack) Then
		If (Request.QueryString.Count > 0) Then
			key = New Linkkey()
			Dim messageList As ArrayList = Linkinf.LoadKey(key)
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
			key = TryCast(ViewState("key"), Linkkey)
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

	Protected Sub LinkDetailsView_Init(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			Linkdal.OpenConnection()
		Catch err As Exception
			lblMessage.Text = err.Message
			pnlMessage.Visible = True
		End Try
	End Sub

	' *************************************
	' *  Handler for DetailsView DataBound
	' *************************************

	Protected Sub LinkDetailsView_DataBound(ByVal sender As Object, ByVal e As EventArgs)
		Dim source As DetailsView = LinkDetailsView
		Dim row As Linkrow = TryCast(LinkDetailsView.DataItem, Linkrow) 'get data object
		RowToControl(row, source, Core.CtrlType.View)
	End Sub

	' *********************************
	' *  Handler for DetailsView Unload
	' *********************************

	Protected Sub LinkDetailsView_Unload(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			Linkdal.CloseAndDisposeConnection()
		Catch
		End Try
	End Sub

	' ***********************************
	' *  Handler for Delete button click
	' ***********************************

	Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim sDataObjectTypeName As String = String.Empty
		sDataObjectTypeName = LinkDataSource.DataObjectTypeName
		LinkDataSource.DataObjectTypeName = ""
		LinkDataSource.Delete()
		LinkDataSource.DataObjectTypeName = sDataObjectTypeName
	End Sub

	' ********************************************
	' *  Convert Row Object Value To Input Control
	' ********************************************

	Private Sub RowToControl(ByVal row As Linkrow, ByVal control As WebControl, ByVal ct As Core.CtrlType)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		If (ct = Core.CtrlType.View) Then ' set up view controls

			' Field Title
			Dim x_Title As Label = TryCast(control.FindControl("x_Title"), Label)
			If (row.Title IsNot Nothing) Then x_Title.Text = row.Title.ToString() Else x_Title.Text = String.Empty

			' Field Description
			Dim x_Description As Label = TryCast(control.FindControl("x_Description"), Label)
			If (row.Description IsNot Nothing) Then x_Description.Text = row.Description.ToString() Else x_Description.Text = String.Empty

			' Field LinkTypeCD
			Dim x_LinkTypeCD As Label = TryCast(control.FindControl("x_LinkTypeCD"), Label)
			Dim v_LinkTypeCD As String
			If (row.LinkTypeCD IsNot Nothing) Then v_LinkTypeCD = row.LinkTypeCD.ToString() Else v_LinkTypeCD = String.Empty
			x_LinkTypeCD.Text = Linkdal.LookUpTable("LinkTypeCD", v_LinkTypeCD)

			' Field CategoryID
			Dim x_CategoryID As Label = TryCast(control.FindControl("x_CategoryID"), Label)
			Dim v_CategoryID As String
			If (row.CategoryID.HasValue) Then v_CategoryID = row.CategoryID.ToString() Else v_CategoryID = String.Empty
			x_CategoryID.Text = Linkdal.LookUpTable("CategoryID", v_CategoryID)

			' Field CompanyID
			Dim x_CompanyID As Label = TryCast(control.FindControl("x_CompanyID"), Label)
			Dim v_CompanyID As String
			If (row.CompanyID.HasValue) Then v_CompanyID = row.CompanyID.ToString() Else v_CompanyID = String.Empty
			x_CompanyID.Text = Linkdal.LookUpTable("CompanyID", v_CompanyID)

			' Field PageID
			Dim x_PageID As Label = TryCast(control.FindControl("x_PageID"), Label)
			Dim v_PageID As String
			If (row.PageID.HasValue) Then v_PageID = row.PageID.ToString() Else v_PageID = String.Empty
			x_PageID.Text = Linkdal.LookUpTable("PageID", v_PageID)

			' Field URL
			Dim x_URL As Label = TryCast(control.FindControl("x_URL"), Label)
			If (row.URL IsNot Nothing) Then x_URL.Text = row.URL.ToString() Else x_URL.Text = String.Empty

			' Field DateAdd
			Dim x_DateAdd As Label = TryCast(control.FindControl("x_DateAdd"), Label)
			If (row.DateAdd.HasValue) Then x_DateAdd.Text = Convert.ToString(row.DateAdd) Else x_DateAdd.Text = String.Empty
			x_DateAdd.Text = DataFormat.DateTimeFormat(6, "/", x_DateAdd.Text)

			' Field Ranks
			Dim x_Ranks As Label = TryCast(control.FindControl("x_Ranks"), Label)
			If (row.Ranks.HasValue) Then x_Ranks.Text = row.Ranks.ToString() Else x_Ranks.Text = String.Empty

			' Field Views
			Dim x_Views As CheckBox = TryCast(control.FindControl("x_Views"), CheckBox)
			If (row.Views.HasValue) Then
				x_Views.Checked = IIf(CType(row.Views, Boolean), True, False)
			End If

			' Field UserID
			Dim x_UserID As Label = TryCast(control.FindControl("x_UserID"), Label)
			Dim v_UserID As String
			If (row.UserID.HasValue) Then v_UserID = row.UserID.ToString() Else v_UserID = String.Empty
			x_UserID.Text = Linkdal.LookUpTable("UserID", v_UserID)

			' Field SiteCategoryGroupID
			Dim x_SiteCategoryGroupID As Label = TryCast(control.FindControl("x_SiteCategoryGroupID"), Label)
			Dim v_SiteCategoryGroupID As String
			If (row.SiteCategoryGroupID.HasValue) Then v_SiteCategoryGroupID = row.SiteCategoryGroupID.ToString() Else v_SiteCategoryGroupID = String.Empty
			x_SiteCategoryGroupID.Text = Linkdal.LookUpTable("SiteCategoryGroupID", v_SiteCategoryGroupID)

			' Field ASIN
			Dim x_ASIN As Label = TryCast(control.FindControl("x_ASIN"), Label)
			If (row.ASIN IsNot Nothing) Then x_ASIN.Text = row.ASIN.ToString() Else x_ASIN.Text = String.Empty
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

	Protected Sub LinkDataSource_Selecting(Byval sender As Object, ByVal e As ObjectDataSourceSelectingEventArgs)
		e.InputParameters.Clear()
		e.InputParameters.Add("filter", Linkinf.GetUserFilter())
	End Sub

	' *************************************
	' *  DataSource Selected Event Handler
	' *************************************

	Protected Sub LinkDataSource_Selected(Byval sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.ReturnValue Is Nothing) Then
			If (e.Exception IsNot Nothing) Then
				lblMessage.Text = e.Exception.Message
				pnlMessage.Visible = True
			Else
				lblMessage.Text = "No records found"
				pnlMessage.Visible = True
			End If
		Else
			Dim rows As Linkrows = TryCast(e.ReturnValue, Linkrows)
			Dim i As Integer = 0
			Do While (rows IsNot Nothing AndAlso i < rows.Count)
				If (key.Equals(rows(i).GetKey())) Then
					LinkDetailsView.PageIndex = i
					Exit Do
				End If
				i += 1
			Loop
		End If
	End Sub

	' ***********************************
	' *  Handler for DataSource Deleting
	' ***********************************

	Protected Sub LinkDataSource_Deleting(ByVal sender As Object, ByVal e As ObjectDataSourceMethodEventArgs)
		Dim data As Linkdal = New Linkdal()
		Dim sWhere As String = String.Empty
		arrKeys.Add(key)
		oldrows = New Linkrows()
		oldrows.Add(data.LoadRow(key, "")) ' load row without filter
		If (Linkbll.Deleting(oldrows)) Then
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

	Protected Sub LinkDataSource_Deleted(ByVal sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.Exception IsNot Nothing) Then
			e.ExceptionHandled = True
			lblMessage.Text = e.Exception.InnerException.Message
		Else
			objProfile.Message = "Delete Successful"
			Linkbll.Deleted(oldrows)
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
		Dim data As Linkdal = New Linkdal()
		Dim rows As Linkrows = data.LoadList(Linkinf.GetUserFilter())
		If (rows IsNot Nothing AndAlso rows.Count >= e.NewPageIndex) Then
			key = rows(e.NewPageIndex).GetKey()
		End If
	End Sub

	' *******************
	' *  Get User Filter
	' *******************

	Private Function GetUserFilter() As String 
		Return Linkinf.GetUserFilter()
	End Function
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
<p><span class="aspnetmaker">Delete from TABLE: Link</span></p>
<p>
	<asp:Hyperlink id="lnkList" Text="Back to List" NavigateUrl="link_list.aspx" CssClass="aspnetmaker" runat="server" />
	<asp:Label id="lblReturnUrl" Visible="False" CssClass="aspnetmaker" runat="server">link_list.aspx</asp:Label>
</p>
<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
	<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
</asp:PlaceHolder>
	<!-- Data Source -->
<asp:ObjectDataSource ID="LinkDataSource"
	TypeName="PMGEN.Linkdal"
	DataObjectTypeName="PMGEN.Linkkey"
	SelectMethod="LoadList"
	DeleteMethod="Delete"
	OnSelecting="LinkDataSource_Selecting"
	OnSelected="LinkDataSource_Selected"
	OnDeleting="LinkDataSource_Deleting"
	OnDeleted="LinkDataSource_Deleted"
	runat="server">
</asp:ObjectDataSource>
	<!-- DetailsView -->
<asp:DetailsView ID="LinkDetailsView"
	DataKeyNames="ID"
	DataSourceID="LinkDataSource"
	DefaultMode="ReadOnly"
	GridLines="None"
	AutoGenerateRows="False" CssClass="ewDetailTable"
	OnInit="LinkDetailsView_Init"
	OnDataBound="LinkDetailsView_DataBound"
	OnUnload="LinkDetailsView_Unload"
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
			<asp:Label runat="server" id="xs_Title">Title</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_Title" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_Description">Description</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_Description" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_LinkTypeCD">Type</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_LinkTypeCD" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_CategoryID">Category</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_CategoryID" CssClass="aspnetmaker" runat="server" />
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
			<asp:Label runat="server" id="xs_PageID">Page</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_PageID" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_URL">HTML</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_URL" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_DateAdd">Date Add</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_DateAdd" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_Ranks">Order</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_Ranks" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_Views">Views</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:CheckBox id="x_Views" Enabled="False"  CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_UserID">User ID</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_UserID" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_SiteCategoryGroupID">Site Group</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_SiteCategoryGroupID" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_ASIN">ASIN</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_ASIN" CssClass="aspnetmaker" runat="server" />
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
