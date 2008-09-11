<%@ Page EnableViewState="true" Language="VB" ValidateRequest="False" EnableEventValidation="False" Debug="True" MasterPageFile="masterpage.master" %>
<%@ MasterType VirtualPath="masterpage.master" %>
<%@ Import Namespace="PMGEN" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="EW.Data" %>
<%@ Import Namespace="EW.Data.Utilities" %>
<%@ Import Namespace="EW.Web" %>
<%@ Register NameSpace="PMGEN" TagPrefix="PMGEN" %>
<script runat="server">
	Dim key As SiteLinkkey ' record key
	Dim oldrow As SiteLinkrow ' old record
	Dim arrKeys As ArrayList = New ArrayList()
	Dim oldrows As SiteLinkrows ' old records
	Dim objProfile As TableProfile ' table profile

	' *************************
	' *  Handler for Page Init
	' *************************

	Protected Sub Page_Init(ByVal s As Object, ByVal e As EventArgs)
		If Not mhUser.IsAdmin() Then
		  Session("Login_Link") = Request.RawUrl
		  Response.Redirect("/mhweb/login/login.aspx")
		End If
		objProfile = CustomProfile.GetTable(Share.ProjectName, SiteLinkinf.TableVar)
	End Sub	

	' *************************
	' *  Handler for Page Load
	' *************************

	Protected Sub Page_Load(ByVal s As Object, ByVal e As EventArgs)
		Response.Cache.SetCacheability(HttpCacheability.NoCache)
		If (Not Page.IsPostBack) Then
		If (Request.QueryString.Count > 0) Then
			key = New SiteLinkkey()
			Dim messageList As ArrayList = SiteLinkinf.LoadKey(key)
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
			key = TryCast(ViewState("key"), SiteLinkkey)
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

	Protected Sub SiteLinkDetailsView_Init(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			SiteLinkdal.OpenConnection()
		Catch err As Exception
			lblMessage.Text = err.Message
			pnlMessage.Visible = True
		End Try
	End Sub

	' *************************************
	' *  Handler for DetailsView DataBound
	' *************************************

	Protected Sub SiteLinkDetailsView_DataBound(ByVal sender As Object, ByVal e As EventArgs)
		Dim source As DetailsView = SiteLinkDetailsView
		Dim row As SiteLinkrow = TryCast(SiteLinkDetailsView.DataItem, SiteLinkrow) 'get data object
		RowToControl(row, source, Core.CtrlType.View)
	End Sub

	' *********************************
	' *  Handler for DetailsView Unload
	' *********************************

	Protected Sub SiteLinkDetailsView_Unload(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			SiteLinkdal.CloseAndDisposeConnection()
		Catch
		End Try
	End Sub

	' ***********************************
	' *  Handler for Delete button click
	' ***********************************

	Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim sDataObjectTypeName As String = String.Empty
		sDataObjectTypeName = SiteLinkDataSource.DataObjectTypeName
		SiteLinkDataSource.DataObjectTypeName = ""
		SiteLinkDataSource.Delete()
		SiteLinkDataSource.DataObjectTypeName = sDataObjectTypeName
	End Sub

	' ********************************************
	' *  Convert Row Object Value To Input Control
	' ********************************************

	Private Sub RowToControl(ByVal row As SiteLinkrow, ByVal control As WebControl, ByVal ct As Core.CtrlType)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		If (ct = Core.CtrlType.View) Then ' set up view controls

			' Field SiteCategoryTypeID
			Dim x_SiteCategoryTypeID As Label = TryCast(control.FindControl("x_SiteCategoryTypeID"), Label)
			Dim v_SiteCategoryTypeID As String
			If (row.SiteCategoryTypeID.HasValue) Then v_SiteCategoryTypeID = row.SiteCategoryTypeID.ToString() Else v_SiteCategoryTypeID = String.Empty
			x_SiteCategoryTypeID.Text = SiteLinkdal.LookUpTable("SiteCategoryTypeID", v_SiteCategoryTypeID)

			' Field CategoryID
			Dim x_CategoryID As Label = TryCast(control.FindControl("x_CategoryID"), Label)
			Dim v_CategoryID As String
			If (row.CategoryID.HasValue) Then v_CategoryID = row.CategoryID.ToString() Else v_CategoryID = String.Empty
			x_CategoryID.Text = SiteLinkdal.LookUpTable("CategoryID", v_CategoryID)

			' Field LinkTypeCD
			Dim x_LinkTypeCD As Label = TryCast(control.FindControl("x_LinkTypeCD"), Label)
			Dim v_LinkTypeCD As String
			If (row.LinkTypeCD IsNot Nothing) Then v_LinkTypeCD = row.LinkTypeCD.ToString() Else v_LinkTypeCD = String.Empty
			x_LinkTypeCD.Text = SiteLinkdal.LookUpTable("LinkTypeCD", v_LinkTypeCD)

			' Field Title
			Dim x_Title As Label = TryCast(control.FindControl("x_Title"), Label)
			If (row.Title IsNot Nothing) Then x_Title.Text = row.Title.ToString() Else x_Title.Text = String.Empty

			' Field SiteCategoryID
			Dim x_SiteCategoryID As Label = TryCast(control.FindControl("x_SiteCategoryID"), Label)
			Dim v_SiteCategoryID As String
			If (row.SiteCategoryID.HasValue) Then v_SiteCategoryID = row.SiteCategoryID.ToString() Else v_SiteCategoryID = String.Empty
			x_SiteCategoryID.Text = SiteLinkdal.LookUpTable("SiteCategoryID", v_SiteCategoryID)

			' Field SiteCategoryGroupID
			Dim x_SiteCategoryGroupID As Label = TryCast(control.FindControl("x_SiteCategoryGroupID"), Label)
			Dim v_SiteCategoryGroupID As String
			If (row.SiteCategoryGroupID.HasValue) Then v_SiteCategoryGroupID = row.SiteCategoryGroupID.ToString() Else v_SiteCategoryGroupID = String.Empty
			x_SiteCategoryGroupID.Text = SiteLinkdal.LookUpTable("SiteCategoryGroupID", v_SiteCategoryGroupID)

			' Field CompanyID
			Dim x_CompanyID As Label = TryCast(control.FindControl("x_CompanyID"), Label)
			Dim v_CompanyID As String
			If (row.CompanyID.HasValue) Then v_CompanyID = row.CompanyID.ToString() Else v_CompanyID = String.Empty
			x_CompanyID.Text = SiteLinkdal.LookUpTable("CompanyID", v_CompanyID)

			' Field Description
			Dim x_Description As Label = TryCast(control.FindControl("x_Description"), Label)
			If (row.Description IsNot Nothing) Then x_Description.Text = row.Description.ToString() Else x_Description.Text = String.Empty

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

			' Field UserName
			Dim x_UserName As Label = TryCast(control.FindControl("x_UserName"), Label)
			If (row.UserName IsNot Nothing) Then x_UserName.Text = row.UserName.ToString() Else x_UserName.Text = String.Empty

			' Field UserID
			Dim x_UserID As Label = TryCast(control.FindControl("x_UserID"), Label)
			Dim v_UserID As String
			If (row.UserID.HasValue) Then v_UserID = row.UserID.ToString() Else v_UserID = String.Empty
			x_UserID.Text = SiteLinkdal.LookUpTable("UserID", v_UserID)

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

	Protected Sub SiteLinkDataSource_Selecting(Byval sender As Object, ByVal e As ObjectDataSourceSelectingEventArgs)
		e.InputParameters.Clear()
		e.InputParameters.Add("filter", SiteLinkinf.GetUserFilter())
	End Sub

	' *************************************
	' *  DataSource Selected Event Handler
	' *************************************

	Protected Sub SiteLinkDataSource_Selected(Byval sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.ReturnValue Is Nothing) Then
			If (e.Exception IsNot Nothing) Then
				lblMessage.Text = e.Exception.Message
				pnlMessage.Visible = True
			Else
				lblMessage.Text = "No records found"
				pnlMessage.Visible = True
			End If
		Else
			Dim rows As SiteLinkrows = TryCast(e.ReturnValue, SiteLinkrows)
			Dim i As Integer = 0
			Do While (rows IsNot Nothing AndAlso i < rows.Count)
				If (key.Equals(rows(i).GetKey())) Then
					SiteLinkDetailsView.PageIndex = i
					Exit Do
				End If
				i += 1
			Loop
		End If
	End Sub

	' ***********************************
	' *  Handler for DataSource Deleting
	' ***********************************

	Protected Sub SiteLinkDataSource_Deleting(ByVal sender As Object, ByVal e As ObjectDataSourceMethodEventArgs)
		Dim data As SiteLinkdal = New SiteLinkdal()
		Dim sWhere As String = String.Empty
		arrKeys.Add(key)
		oldrows = New SiteLinkrows()
		oldrows.Add(data.LoadRow(key, "")) ' load row without filter
		If (SiteLinkbll.Deleting(oldrows)) Then
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

	Protected Sub SiteLinkDataSource_Deleted(ByVal sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.Exception IsNot Nothing) Then
			e.ExceptionHandled = True
			lblMessage.Text = e.Exception.InnerException.Message
		Else
			objProfile.Message = "Delete Successful"
			SiteLinkbll.Deleted(oldrows)
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
		Dim data As SiteLinkdal = New SiteLinkdal()
		Dim rows As SiteLinkrows = data.LoadList(SiteLinkinf.GetUserFilter())
		If (rows IsNot Nothing AndAlso rows.Count >= e.NewPageIndex) Then
			key = rows(e.NewPageIndex).GetKey()
		End If
	End Sub

	' *******************
	' *  Get User Filter
	' *******************

	Private Function GetUserFilter() As String 
		Return SiteLinkinf.GetUserFilter()
	End Function
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
<p><span class="aspnetmaker">Delete from TABLE: Site Link</span></p>
<p>
	<asp:Hyperlink id="lnkList" Text="Back to List" NavigateUrl="sitelink_list.aspx" CssClass="aspnetmaker" runat="server" />
	<asp:Label id="lblReturnUrl" Visible="False" CssClass="aspnetmaker" runat="server">sitelink_list.aspx</asp:Label>
</p>
<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
	<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
</asp:PlaceHolder>
	<!-- Data Source -->
<asp:ObjectDataSource ID="SiteLinkDataSource"
	TypeName="PMGEN.SiteLinkdal"
	DataObjectTypeName="PMGEN.SiteLinkkey"
	SelectMethod="LoadList"
	DeleteMethod="Delete"
	OnSelecting="SiteLinkDataSource_Selecting"
	OnSelected="SiteLinkDataSource_Selected"
	OnDeleting="SiteLinkDataSource_Deleting"
	OnDeleted="SiteLinkDataSource_Deleted"
	runat="server">
</asp:ObjectDataSource>
	<!-- DetailsView -->
<asp:DetailsView ID="SiteLinkDetailsView"
	DataKeyNames="ID"
	DataSourceID="SiteLinkDataSource"
	DefaultMode="ReadOnly"
	GridLines="None"
	AutoGenerateRows="False" CssClass="ewDetailTable"
	OnInit="SiteLinkDetailsView_Init"
	OnDataBound="SiteLinkDetailsView_DataBound"
	OnUnload="SiteLinkDetailsView_Unload"
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
			<asp:Label runat="server" id="xs_SiteCategoryTypeID">Site Type</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_SiteCategoryTypeID" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_CategoryID">Link Category</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_CategoryID" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_LinkTypeCD">Link Type</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_LinkTypeCD" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
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
			<asp:Label runat="server" id="xs_SiteCategoryID">Site Category</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_SiteCategoryID" CssClass="aspnetmaker" runat="server" />
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
			<asp:Label runat="server" id="xs_CompanyID">Company</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_CompanyID" CssClass="aspnetmaker" runat="server" />
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
			<asp:Label runat="server" id="xs_URL">URL</asp:Label>
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
			<asp:Label runat="server" id="xs_Ranks">Ranks</asp:Label>
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
			<asp:Label runat="server" id="xs_UserName">User Name</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_UserName" CssClass="aspnetmaker" runat="server" />
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
