<%@ Page EnableViewState="true" Language="VB" ValidateRequest="False" Debug="True" MasterPageFile="masterpage.master" %>
<%@ MasterType VirtualPath="masterpage.master" %>
<%@ Import Namespace="PMGEN" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="EW.Data" %>
<%@ Import Namespace="EW.Data.Utilities" %>
<%@ Import Namespace="EW.Web" %>
<%@ Register NameSpace="PMGEN" TagPrefix="PMGEN" %>
<script runat="server">
	Dim key As Companykey ' record key
	Dim oldrow As Companyrow ' old record
	Dim arrKeys As ArrayList = New ArrayList()
	Dim oldrows As Companyrows ' old records
	Dim objProfile As TableProfile ' table profile

	' *************************
	' *  Handler for Page Init
	' *************************

	Protected Sub Page_Init(ByVal s As Object, ByVal e As EventArgs)
		If Not mhUser.IsAdmin() Then
		  Session("Login_Link") = Request.RawUrl
		  Response.Redirect("/mhweb/login/login.aspx")
		End If
		objProfile = CustomProfile.GetTable(Share.ProjectName, Companyinf.TableVar)
	End Sub	

	' *************************
	' *  Handler for Page Load
	' *************************

	Protected Sub Page_Load(ByVal s As Object, ByVal e As EventArgs)
		Response.Cache.SetCacheability(HttpCacheability.NoCache)
		If (Not Page.IsPostBack) Then
		If (Request.QueryString.Count > 0) Then
			key = New Companykey()
			Dim messageList As ArrayList = Companyinf.LoadKey(key)
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
			key = TryCast(ViewState("key"), Companykey)
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

	Protected Sub CompanyDetailsView_Init(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			Companydal.OpenConnection()
		Catch err As Exception
			lblMessage.Text = err.Message
			pnlMessage.Visible = True
		End Try
	End Sub

	' *************************************
	' *  Handler for DetailsView DataBound
	' *************************************

	Protected Sub CompanyDetailsView_DataBound(ByVal sender As Object, ByVal e As EventArgs)
		Dim source As DetailsView = CompanyDetailsView
		Dim row As Companyrow = TryCast(CompanyDetailsView.DataItem, Companyrow) 'get data object
		RowToControl(row, source, Core.CtrlType.View)
	End Sub

	' *********************************
	' *  Handler for DetailsView Unload
	' *********************************

	Protected Sub CompanyDetailsView_Unload(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			Companydal.CloseAndDisposeConnection()
		Catch
		End Try
	End Sub

	' ***********************************
	' *  Handler for Delete button click
	' ***********************************

	Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim sDataObjectTypeName As String = String.Empty
		sDataObjectTypeName = CompanyDataSource.DataObjectTypeName
		CompanyDataSource.DataObjectTypeName = ""
		CompanyDataSource.Delete()
		CompanyDataSource.DataObjectTypeName = sDataObjectTypeName
	End Sub

	' ********************************************
	' *  Convert Row Object Value To Input Control
	' ********************************************

	Private Sub RowToControl(ByVal row As Companyrow, ByVal control As WebControl, ByVal ct As Core.CtrlType)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		If (ct = Core.CtrlType.View) Then ' set up view controls

			' Field CompanyName
			Dim x_CompanyName As Label = TryCast(control.FindControl("x_CompanyName"), Label)
			If (row.CompanyName IsNot Nothing) Then x_CompanyName.Text = row.CompanyName.ToString() Else x_CompanyName.Text = String.Empty

			' Field SiteTitle
			Dim x_SiteTitle As Label = TryCast(control.FindControl("x_SiteTitle"), Label)
			If (row.SiteTitle IsNot Nothing) Then x_SiteTitle.Text = row.SiteTitle.ToString() Else x_SiteTitle.Text = String.Empty

			' Field SiteTemplate
			Dim x_SiteTemplate As Label = TryCast(control.FindControl("x_SiteTemplate"), Label)
			Dim v_SiteTemplate As String
			If (row.SiteTemplate IsNot Nothing) Then v_SiteTemplate = row.SiteTemplate.ToString() Else v_SiteTemplate = String.Empty
			x_SiteTemplate.Text = Companydal.LookUpTable("SiteTemplate", v_SiteTemplate)

			' Field DefaultSiteTemplate
			Dim x_DefaultSiteTemplate As Label = TryCast(control.FindControl("x_DefaultSiteTemplate"), Label)
			Dim v_DefaultSiteTemplate As String
			If (row.DefaultSiteTemplate IsNot Nothing) Then v_DefaultSiteTemplate = row.DefaultSiteTemplate.ToString() Else v_DefaultSiteTemplate = String.Empty
			x_DefaultSiteTemplate.Text = Companydal.LookUpTable("DefaultSiteTemplate", v_DefaultSiteTemplate)

			' Field GalleryFolder
			Dim x_GalleryFolder As Label = TryCast(control.FindControl("x_GalleryFolder"), Label)
			If (row.GalleryFolder IsNot Nothing) Then x_GalleryFolder.Text = row.GalleryFolder.ToString() Else x_GalleryFolder.Text = String.Empty

			' Field SiteURL
			Dim x_SiteURL As Label = TryCast(control.FindControl("x_SiteURL"), Label)
			If (row.SiteURL IsNot Nothing) Then x_SiteURL.Text = row.SiteURL.ToString() Else x_SiteURL.Text = String.Empty

			' Field City
			Dim x_City As Label = TryCast(control.FindControl("x_City"), Label)
			If (row.City IsNot Nothing) Then x_City.Text = row.City.ToString() Else x_City.Text = String.Empty

			' Field StateOrProvince
			Dim x_StateOrProvince As Label = TryCast(control.FindControl("x_StateOrProvince"), Label)
			If (row.StateOrProvince IsNot Nothing) Then x_StateOrProvince.Text = row.StateOrProvince.ToString() Else x_StateOrProvince.Text = String.Empty

			' Field Country
			Dim x_Country As Label = TryCast(control.FindControl("x_Country"), Label)
			If (row.Country IsNot Nothing) Then x_Country.Text = row.Country.ToString() Else x_Country.Text = String.Empty

			' Field DefaultPaymentTerms
			Dim x_DefaultPaymentTerms As Label = TryCast(control.FindControl("x_DefaultPaymentTerms"), Label)
			If (row.DefaultPaymentTerms IsNot Nothing) Then x_DefaultPaymentTerms.Text = row.DefaultPaymentTerms.ToString() Else x_DefaultPaymentTerms.Text = String.Empty

			' Field DefaultInvoiceDescription
			Dim x_DefaultInvoiceDescription As Label = TryCast(control.FindControl("x_DefaultInvoiceDescription"), Label)
			If (row.DefaultInvoiceDescription IsNot Nothing) Then x_DefaultInvoiceDescription.Text = row.DefaultInvoiceDescription.ToString() Else x_DefaultInvoiceDescription.Text = String.Empty

			' Field DefaultArticleID
			Dim x_DefaultArticleID As Label = TryCast(control.FindControl("x_DefaultArticleID"), Label)
			Dim v_DefaultArticleID As String
			If (row.DefaultArticleID.HasValue) Then v_DefaultArticleID = row.DefaultArticleID.ToString() Else v_DefaultArticleID = String.Empty
			x_DefaultArticleID.Text = Companydal.LookUpTable("DefaultArticleID", v_DefaultArticleID)

			' Field HomePageID
			Dim x_HomePageID As Label = TryCast(control.FindControl("x_HomePageID"), Label)
			Dim v_HomePageID As String
			If (row.HomePageID.HasValue) Then v_HomePageID = row.HomePageID.ToString() Else v_HomePageID = String.Empty
			x_HomePageID.Text = Companydal.LookUpTable("HomePageID", v_HomePageID)

			' Field UseBreadCrumbURL
			Dim x_UseBreadCrumbURL As CheckBox = TryCast(control.FindControl("x_UseBreadCrumbURL"), CheckBox)
			If (row.UseBreadCrumbURL.HasValue) Then
				x_UseBreadCrumbURL.Checked = IIf(CType(row.UseBreadCrumbURL, Boolean), True, False)
			End If

			' Field SingleSiteGallery
			Dim x_SingleSiteGallery As CheckBox = TryCast(control.FindControl("x_SingleSiteGallery"), CheckBox)
			If (row.SingleSiteGallery.HasValue) Then
				x_SingleSiteGallery.Checked = IIf(CType(row.SingleSiteGallery, Boolean), True, False)
			End If

			' Field SiteCategoryTypeID
			Dim x_SiteCategoryTypeID As Label = TryCast(control.FindControl("x_SiteCategoryTypeID"), Label)
			Dim v_SiteCategoryTypeID As String
			If (row.SiteCategoryTypeID.HasValue) Then v_SiteCategoryTypeID = row.SiteCategoryTypeID.ToString() Else v_SiteCategoryTypeID = String.Empty
			x_SiteCategoryTypeID.Text = Companydal.LookUpTable("SiteCategoryTypeID", v_SiteCategoryTypeID)

			' Field ActiveFL
			Dim x_ActiveFL As CheckBox = TryCast(control.FindControl("x_ActiveFL"), CheckBox)
			If (row.ActiveFL.HasValue) Then
				x_ActiveFL.Checked = IIf(CType(row.ActiveFL, Boolean), True, False)
			End If
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

	Protected Sub CompanyDataSource_Selecting(Byval sender As Object, ByVal e As ObjectDataSourceSelectingEventArgs)
		e.InputParameters.Clear()
		e.InputParameters.Add("filter", Companyinf.GetUserFilter())
	End Sub

	' *************************************
	' *  DataSource Selected Event Handler
	' *************************************

	Protected Sub CompanyDataSource_Selected(Byval sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.ReturnValue Is Nothing) Then
			If (e.Exception IsNot Nothing) Then
				lblMessage.Text = e.Exception.Message
				pnlMessage.Visible = True
			Else
				lblMessage.Text = "No records found"
				pnlMessage.Visible = True
			End If
		Else
			Dim rows As Companyrows = TryCast(e.ReturnValue, Companyrows)
			Dim i As Integer = 0
			Do While (rows IsNot Nothing AndAlso i < rows.Count)
				If (key.Equals(rows(i).GetKey())) Then
					CompanyDetailsView.PageIndex = i
					Exit Do
				End If
				i += 1
			Loop
		End If
	End Sub

	' ***********************************
	' *  Handler for DataSource Deleting
	' ***********************************

	Protected Sub CompanyDataSource_Deleting(ByVal sender As Object, ByVal e As ObjectDataSourceMethodEventArgs)
		Dim data As Companydal = New Companydal()
		Dim sWhere As String = String.Empty
		arrKeys.Add(key)
		oldrows = New Companyrows()
		oldrows.Add(data.LoadRow(key, "")) ' load row without filter
		If (Companybll.Deleting(oldrows)) Then
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

	Protected Sub CompanyDataSource_Deleted(ByVal sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.Exception IsNot Nothing) Then
			e.ExceptionHandled = True
			lblMessage.Text = e.Exception.InnerException.Message
		Else
			objProfile.Message = "Delete Successful"
			Companybll.Deleted(oldrows)
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
		Dim data As Companydal = New Companydal()
		Dim rows As Companyrows = data.LoadList(Companyinf.GetUserFilter())
		If (rows IsNot Nothing AndAlso rows.Count >= e.NewPageIndex) Then
			key = rows(e.NewPageIndex).GetKey()
		End If
	End Sub

	' *******************
	' *  Get User Filter
	' *******************

	Private Function GetUserFilter() As String 
		Return Companyinf.GetUserFilter()
	End Function
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
<p><span class="aspnetmaker">Delete from TABLE: Company</span></p>
<p>
	<asp:Hyperlink id="lnkList" Text="Back to List" NavigateUrl="company_list.aspx" CssClass="aspnetmaker" runat="server" />
	<asp:Label id="lblReturnUrl" Visible="False" CssClass="aspnetmaker" runat="server">company_list.aspx</asp:Label>
</p>
<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
	<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
</asp:PlaceHolder>
	<!-- Data Source -->
<asp:ObjectDataSource ID="CompanyDataSource"
	TypeName="PMGEN.Companydal"
	DataObjectTypeName="PMGEN.Companykey"
	SelectMethod="LoadList"
	DeleteMethod="Delete"
	OnSelecting="CompanyDataSource_Selecting"
	OnSelected="CompanyDataSource_Selected"
	OnDeleting="CompanyDataSource_Deleting"
	OnDeleted="CompanyDataSource_Deleted"
	runat="server">
</asp:ObjectDataSource>
	<!-- DetailsView -->
<asp:DetailsView ID="CompanyDetailsView"
	DataKeyNames="CompanyID"
	DataSourceID="CompanyDataSource"
	DefaultMode="ReadOnly"
	GridLines="None"
	AutoGenerateRows="False" CssClass="ewDetailTable"
	OnInit="CompanyDetailsView_Init"
	OnDataBound="CompanyDetailsView_DataBound"
	OnUnload="CompanyDetailsView_Unload"
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
			<asp:Label runat="server" id="xs_CompanyName">Company Name</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_CompanyName" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_SiteTitle">Site Title</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_SiteTitle" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_SiteTemplate">Site Template</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_SiteTemplate" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_DefaultSiteTemplate">Default Template</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_DefaultSiteTemplate" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_GalleryFolder">Gallery Folder</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_GalleryFolder" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_SiteURL">Site URL</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_SiteURL" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_City">City</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_City" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_StateOrProvince">State/Province</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_StateOrProvince" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_Country">Country</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_Country" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_DefaultPaymentTerms">Site Keywords</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_DefaultPaymentTerms" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_DefaultInvoiceDescription">Site Description</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_DefaultInvoiceDescription" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_DefaultArticleID">Default Article ID</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_DefaultArticleID" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_HomePageID">Home Page ID</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_HomePageID" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_UseBreadCrumbURL">Use Bread Crumb URL</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:CheckBox id="x_UseBreadCrumbURL" Enabled="False"  CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_SingleSiteGallery">Single Site Gallery</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:CheckBox id="x_SingleSiteGallery" Enabled="False"  CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_SiteCategoryTypeID">Site Type</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_SiteCategoryTypeID" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_ActiveFL">Active FL</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:CheckBox id="x_ActiveFL" Enabled="False"  CssClass="aspnetmaker" runat="server" />
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
