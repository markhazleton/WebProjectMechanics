<%@ Page EnableViewState="true" Language="VB" ValidateRequest="False" EnableEventValidation="False" Debug="True" MasterPageFile="masterpage.master" %>
<%@ MasterType VirtualPath="masterpage.master" %>
<%@ Import Namespace="PMGEN" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="EW.Data" %>
<%@ Import Namespace="EW.Data.Utilities" %>
<%@ Import Namespace="EW.Web" %>
<%@ Register NameSpace="PMGEN" TagPrefix="PMGEN" %>
<script runat="server">
	Dim key As Articlekey ' record key
	Dim oldrow As Articlerow ' old record
	Dim arrKeys As ArrayList = New ArrayList()
	Dim oldrows As Articlerows ' old records
	Dim objProfile As TableProfile ' table profile

	' *************************
	' *  Handler for Page Init
	' *************************

	Protected Sub Page_Init(ByVal s As Object, ByVal e As EventArgs)
		If Not mhUser.IsAdmin() Then
		  Session("Login_Link") = Request.RawUrl
		  Response.Redirect("/mhweb/login/login.aspx")
		End If
		objProfile = CustomProfile.GetTable(Share.ProjectName, Articleinf.TableVar)
	End Sub	

	' *************************
	' *  Handler for Page Load
	' *************************

	Protected Sub Page_Load(ByVal s As Object, ByVal e As EventArgs)
		Response.Cache.SetCacheability(HttpCacheability.NoCache)
		If (Not Page.IsPostBack) Then
		If (Request.QueryString.Count > 0) Then
			key = New Articlekey()
			Dim messageList As ArrayList = Articleinf.LoadKey(key)
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
			key = TryCast(ViewState("key"), Articlekey)
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

	Protected Sub ArticleDetailsView_Init(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			Articledal.OpenConnection()
		Catch err As Exception
			lblMessage.Text = err.Message
			pnlMessage.Visible = True
		End Try
	End Sub

	' *************************************
	' *  Handler for DetailsView DataBound
	' *************************************

	Protected Sub ArticleDetailsView_DataBound(ByVal sender As Object, ByVal e As EventArgs)
		Dim source As DetailsView = ArticleDetailsView
		Dim row As Articlerow = TryCast(ArticleDetailsView.DataItem, Articlerow) 'get data object
		RowToControl(row, source, Core.CtrlType.View)
	End Sub

	' *********************************
	' *  Handler for DetailsView Unload
	' *********************************

	Protected Sub ArticleDetailsView_Unload(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			Articledal.CloseAndDisposeConnection()
		Catch
		End Try
	End Sub

	' ***********************************
	' *  Handler for Delete button click
	' ***********************************

	Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim sDataObjectTypeName As String = String.Empty
		sDataObjectTypeName = ArticleDataSource.DataObjectTypeName
		ArticleDataSource.DataObjectTypeName = ""
		ArticleDataSource.Delete()
		ArticleDataSource.DataObjectTypeName = sDataObjectTypeName
	End Sub

	' ********************************************
	' *  Convert Row Object Value To Input Control
	' ********************************************

	Private Sub RowToControl(ByVal row As Articlerow, ByVal control As WebControl, ByVal ct As Core.CtrlType)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		If (ct = Core.CtrlType.View) Then ' set up view controls

			' Field Title
			Dim x_Title As Label = TryCast(control.FindControl("x_Title"), Label)
			If (row.Title IsNot Nothing) Then x_Title.Text = row.Title.ToString() Else x_Title.Text = String.Empty

			' Field Description
			Dim x_Description As Label = TryCast(control.FindControl("x_Description"), Label)
			If (row.Description IsNot Nothing) Then x_Description.Text = row.Description.ToString() Else x_Description.Text = String.Empty

			' Field ArticleSummary
			Dim x_ArticleSummary As Label = TryCast(control.FindControl("x_ArticleSummary"), Label)
			If (row.ArticleSummary IsNot Nothing) Then x_ArticleSummary.Text = row.ArticleSummary.ToString() Else x_ArticleSummary.Text = String.Empty

			' Field ArticleBody
			Dim x_ArticleBody As Label = TryCast(control.FindControl("x_ArticleBody"), Label)
			If (row.ArticleBody IsNot Nothing) Then x_ArticleBody.Text = row.ArticleBody.ToString() Else x_ArticleBody.Text = String.Empty

			' Field CompanyID
			Dim x_CompanyID As Label = TryCast(control.FindControl("x_CompanyID"), Label)
			Dim v_CompanyID As String
			If (row.CompanyID.HasValue) Then v_CompanyID = row.CompanyID.ToString() Else v_CompanyID = String.Empty
			x_CompanyID.Text = Articledal.LookUpTable("CompanyID", v_CompanyID)

			' Field PageID
			Dim x_PageID As Label = TryCast(control.FindControl("x_PageID"), Label)
			Dim v_PageID As String
			If (row.PageID.HasValue) Then v_PageID = row.PageID.ToString() Else v_PageID = String.Empty
			x_PageID.Text = Articledal.LookUpTable("PageID", v_PageID)

			' Field Active
			Dim x_Active As CheckBox = TryCast(control.FindControl("x_Active"), CheckBox)
			If (row.Active.HasValue) Then
				x_Active.Checked = IIf(CType(row.Active, Boolean), True, False)
			End If

			' Field StartDT
			Dim x_StartDT As Label = TryCast(control.FindControl("x_StartDT"), Label)
			If (row.StartDT.HasValue) Then x_StartDT.Text = Convert.ToString(row.StartDT) Else x_StartDT.Text = String.Empty
			x_StartDT.Text = DataFormat.DateTimeFormat(6, "/", x_StartDT.Text)

			' Field ContactID
			Dim x_ContactID As Label = TryCast(control.FindControl("x_ContactID"), Label)
			Dim v_ContactID As String
			If (row.ContactID.HasValue) Then v_ContactID = row.ContactID.ToString() Else v_ContactID = String.Empty
			x_ContactID.Text = Articledal.LookUpTable("ContactID", v_ContactID)

			' Field userID
			Dim x_userID As Label = TryCast(control.FindControl("x_userID"), Label)
			If (row.userID.HasValue) Then x_userID.Text = row.userID.ToString() Else x_userID.Text = String.Empty
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

	Protected Sub ArticleDataSource_Selecting(Byval sender As Object, ByVal e As ObjectDataSourceSelectingEventArgs)
		e.InputParameters.Clear()
		e.InputParameters.Add("filter", Articleinf.GetUserFilter())
	End Sub

	' *************************************
	' *  DataSource Selected Event Handler
	' *************************************

	Protected Sub ArticleDataSource_Selected(Byval sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.ReturnValue Is Nothing) Then
			If (e.Exception IsNot Nothing) Then
				lblMessage.Text = e.Exception.Message
				pnlMessage.Visible = True
			Else
				lblMessage.Text = "No records found"
				pnlMessage.Visible = True
			End If
		Else
			Dim rows As Articlerows = TryCast(e.ReturnValue, Articlerows)
			Dim i As Integer = 0
			Do While (rows IsNot Nothing AndAlso i < rows.Count)
				If (key.Equals(rows(i).GetKey())) Then
					ArticleDetailsView.PageIndex = i
					Exit Do
				End If
				i += 1
			Loop
		End If
	End Sub

	' ***********************************
	' *  Handler for DataSource Deleting
	' ***********************************

	Protected Sub ArticleDataSource_Deleting(ByVal sender As Object, ByVal e As ObjectDataSourceMethodEventArgs)
		Dim data As Articledal = New Articledal()
		Dim sWhere As String = String.Empty
		arrKeys.Add(key)
		oldrows = New Articlerows()
		oldrows.Add(data.LoadRow(key, "")) ' load row without filter
		If (Articlebll.Deleting(oldrows)) Then
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

	Protected Sub ArticleDataSource_Deleted(ByVal sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.Exception IsNot Nothing) Then
			e.ExceptionHandled = True
			lblMessage.Text = e.Exception.InnerException.Message
		Else
			objProfile.Message = "Delete Successful"
			Articlebll.Deleted(oldrows)
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
		Dim data As Articledal = New Articledal()
		Dim rows As Articlerows = data.LoadList(Articleinf.GetUserFilter())
		If (rows IsNot Nothing AndAlso rows.Count >= e.NewPageIndex) Then
			key = rows(e.NewPageIndex).GetKey()
		End If
	End Sub

	' *******************
	' *  Get User Filter
	' *******************

	Private Function GetUserFilter() As String 
		Return Articleinf.GetUserFilter()
	End Function
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
<p><span class="aspnetmaker">Delete from TABLE: Article</span></p>
<p>
	<asp:Hyperlink id="lnkList" Text="Back to List" NavigateUrl="article_list.aspx" CssClass="aspnetmaker" runat="server" />
	<asp:Label id="lblReturnUrl" Visible="False" CssClass="aspnetmaker" runat="server">article_list.aspx</asp:Label>
</p>
<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
	<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
</asp:PlaceHolder>
	<!-- Data Source -->
<asp:ObjectDataSource ID="ArticleDataSource"
	TypeName="PMGEN.Articledal"
	DataObjectTypeName="PMGEN.Articlekey"
	SelectMethod="LoadList"
	DeleteMethod="Delete"
	OnSelecting="ArticleDataSource_Selecting"
	OnSelected="ArticleDataSource_Selected"
	OnDeleting="ArticleDataSource_Deleting"
	OnDeleted="ArticleDataSource_Deleted"
	runat="server">
</asp:ObjectDataSource>
	<!-- DetailsView -->
<asp:DetailsView ID="ArticleDetailsView"
	DataKeyNames="ArticleID"
	DataSourceID="ArticleDataSource"
	DefaultMode="ReadOnly"
	GridLines="None"
	AutoGenerateRows="False" CssClass="ewDetailTable"
	OnInit="ArticleDetailsView_Init"
	OnDataBound="ArticleDetailsView_DataBound"
	OnUnload="ArticleDetailsView_Unload"
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
			<asp:Label runat="server" id="xs_ArticleSummary">Article Summary</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_ArticleSummary" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_ArticleBody">Article Body</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_ArticleBody" CssClass="aspnetmaker" runat="server" />
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
			<asp:Label runat="server" id="xs_Active">Active</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:CheckBox id="x_Active" Enabled="False"  CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_StartDT">Start DT</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_StartDT" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_ContactID">Contact</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_ContactID" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_userID">userName</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_userID" CssClass="aspnetmaker" runat="server" />
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
