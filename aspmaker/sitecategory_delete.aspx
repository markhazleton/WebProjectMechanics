<%@ Page EnableViewState="true" Language="VB" ValidateRequest="False" EnableEventValidation="False" Debug="True" MasterPageFile="masterpage.master" %>
<%@ MasterType VirtualPath="masterpage.master" %>
<%@ Import Namespace="PMGEN" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="EW.Data" %>
<%@ Import Namespace="EW.Data.Utilities" %>
<%@ Import Namespace="EW.Web" %>
<%@ Register NameSpace="PMGEN" TagPrefix="PMGEN" %>
<script runat="server">
	Dim key As SiteCategorykey ' record key
	Dim oldrow As SiteCategoryrow ' old record
	Dim arrKeys As ArrayList = New ArrayList()
	Dim oldrows As SiteCategoryrows ' old records
	Dim objProfile As TableProfile ' table profile

	' *************************
	' *  Handler for Page Init
	' *************************

	Protected Sub Page_Init(ByVal s As Object, ByVal e As EventArgs)
		If Not mhUser.IsAdmin() Then
		  Session("Login_Link") = Request.RawUrl
		  Response.Redirect("/mhweb/login/login.aspx")
		End If
		objProfile = CustomProfile.GetTable(Share.ProjectName, SiteCategoryinf.TableVar)
	End Sub	

	' *************************
	' *  Handler for Page Load
	' *************************

	Protected Sub Page_Load(ByVal s As Object, ByVal e As EventArgs)
		Response.Cache.SetCacheability(HttpCacheability.NoCache)
		If (Not Page.IsPostBack) Then
		If (Request.QueryString.Count > 0) Then
			key = New SiteCategorykey()
			Dim messageList As ArrayList = SiteCategoryinf.LoadKey(key)
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
			key = TryCast(ViewState("key"), SiteCategorykey)
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

	Protected Sub SiteCategoryDetailsView_Init(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			SiteCategorydal.OpenConnection()
		Catch err As Exception
			lblMessage.Text = err.Message
			pnlMessage.Visible = True
		End Try
	End Sub

	' *************************************
	' *  Handler for DetailsView DataBound
	' *************************************

	Protected Sub SiteCategoryDetailsView_DataBound(ByVal sender As Object, ByVal e As EventArgs)
		Dim source As DetailsView = SiteCategoryDetailsView
		Dim row As SiteCategoryrow = TryCast(SiteCategoryDetailsView.DataItem, SiteCategoryrow) 'get data object
		RowToControl(row, source, Core.CtrlType.View)
	End Sub

	' *********************************
	' *  Handler for DetailsView Unload
	' *********************************

	Protected Sub SiteCategoryDetailsView_Unload(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			SiteCategorydal.CloseAndDisposeConnection()
		Catch
		End Try
	End Sub

	' ***********************************
	' *  Handler for Delete button click
	' ***********************************

	Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim sDataObjectTypeName As String = String.Empty
		sDataObjectTypeName = SiteCategoryDataSource.DataObjectTypeName
		SiteCategoryDataSource.DataObjectTypeName = ""
		SiteCategoryDataSource.Delete()
		SiteCategoryDataSource.DataObjectTypeName = sDataObjectTypeName
	End Sub

	' ********************************************
	' *  Convert Row Object Value To Input Control
	' ********************************************

	Private Sub RowToControl(ByVal row As SiteCategoryrow, ByVal control As WebControl, ByVal ct As Core.CtrlType)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		If (ct = Core.CtrlType.View) Then ' set up view controls

			' Field SiteCategoryTypeID
			Dim x_SiteCategoryTypeID As Label = TryCast(control.FindControl("x_SiteCategoryTypeID"), Label)
			Dim v_SiteCategoryTypeID As String
			If (row.SiteCategoryTypeID.HasValue) Then v_SiteCategoryTypeID = row.SiteCategoryTypeID.ToString() Else v_SiteCategoryTypeID = String.Empty
			x_SiteCategoryTypeID.Text = SiteCategorydal.LookUpTable("SiteCategoryTypeID", v_SiteCategoryTypeID)

			' Field CategoryName
			Dim x_CategoryName As Label = TryCast(control.FindControl("x_CategoryName"), Label)
			If (row.CategoryName IsNot Nothing) Then x_CategoryName.Text = row.CategoryName.ToString() Else x_CategoryName.Text = String.Empty

			' Field CategoryTitle
			Dim x_CategoryTitle As Label = TryCast(control.FindControl("x_CategoryTitle"), Label)
			If (row.CategoryTitle IsNot Nothing) Then x_CategoryTitle.Text = row.CategoryTitle.ToString() Else x_CategoryTitle.Text = String.Empty

			' Field CategoryDescription
			Dim x_CategoryDescription As Label = TryCast(control.FindControl("x_CategoryDescription"), Label)
			If (row.CategoryDescription IsNot Nothing) Then x_CategoryDescription.Text = row.CategoryDescription.ToString() Else x_CategoryDescription.Text = String.Empty

			' Field CategoryKeywords
			Dim x_CategoryKeywords As Label = TryCast(control.FindControl("x_CategoryKeywords"), Label)
			If (row.CategoryKeywords IsNot Nothing) Then x_CategoryKeywords.Text = row.CategoryKeywords.ToString() Else x_CategoryKeywords.Text = String.Empty

			' Field SiteCategoryGroupID
			Dim x_SiteCategoryGroupID As Label = TryCast(control.FindControl("x_SiteCategoryGroupID"), Label)
			Dim v_SiteCategoryGroupID As String
			If (row.SiteCategoryGroupID.HasValue) Then v_SiteCategoryGroupID = row.SiteCategoryGroupID.ToString() Else v_SiteCategoryGroupID = String.Empty
			x_SiteCategoryGroupID.Text = SiteCategorydal.LookUpTable("SiteCategoryGroupID", v_SiteCategoryGroupID)

			' Field GroupOrder
			Dim x_GroupOrder As Label = TryCast(control.FindControl("x_GroupOrder"), Label)
			If (row.GroupOrder.HasValue) Then x_GroupOrder.Text = row.GroupOrder.ToString() Else x_GroupOrder.Text = String.Empty

			' Field ParentCategoryID
			Dim x_ParentCategoryID As Label = TryCast(control.FindControl("x_ParentCategoryID"), Label)
			Dim v_ParentCategoryID As String
			If (row.ParentCategoryID.HasValue) Then v_ParentCategoryID = row.ParentCategoryID.ToString() Else v_ParentCategoryID = String.Empty
			x_ParentCategoryID.Text = SiteCategorydal.LookUpTable("ParentCategoryID", v_ParentCategoryID)
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

	Protected Sub SiteCategoryDataSource_Selecting(Byval sender As Object, ByVal e As ObjectDataSourceSelectingEventArgs)
		e.InputParameters.Clear()
		e.InputParameters.Add("filter", SiteCategoryinf.GetUserFilter())
	End Sub

	' *************************************
	' *  DataSource Selected Event Handler
	' *************************************

	Protected Sub SiteCategoryDataSource_Selected(Byval sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.ReturnValue Is Nothing) Then
			If (e.Exception IsNot Nothing) Then
				lblMessage.Text = e.Exception.Message
				pnlMessage.Visible = True
			Else
				lblMessage.Text = "No records found"
				pnlMessage.Visible = True
			End If
		Else
			Dim rows As SiteCategoryrows = TryCast(e.ReturnValue, SiteCategoryrows)
			Dim i As Integer = 0
			Do While (rows IsNot Nothing AndAlso i < rows.Count)
				If (key.Equals(rows(i).GetKey())) Then
					SiteCategoryDetailsView.PageIndex = i
					Exit Do
				End If
				i += 1
			Loop
		End If
	End Sub

	' ***********************************
	' *  Handler for DataSource Deleting
	' ***********************************

	Protected Sub SiteCategoryDataSource_Deleting(ByVal sender As Object, ByVal e As ObjectDataSourceMethodEventArgs)
		Dim data As SiteCategorydal = New SiteCategorydal()
		Dim sWhere As String = String.Empty
		arrKeys.Add(key)
		oldrows = New SiteCategoryrows()
		oldrows.Add(data.LoadRow(key, "")) ' load row without filter
		If (SiteCategorybll.Deleting(oldrows)) Then
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

	Protected Sub SiteCategoryDataSource_Deleted(ByVal sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.Exception IsNot Nothing) Then
			e.ExceptionHandled = True
			lblMessage.Text = e.Exception.InnerException.Message
		Else
			objProfile.Message = "Delete Successful"
			SiteCategorybll.Deleted(oldrows)
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
		Dim data As SiteCategorydal = New SiteCategorydal()
		Dim rows As SiteCategoryrows = data.LoadList(SiteCategoryinf.GetUserFilter())
		If (rows IsNot Nothing AndAlso rows.Count >= e.NewPageIndex) Then
			key = rows(e.NewPageIndex).GetKey()
		End If
	End Sub

	' *******************
	' *  Get User Filter
	' *******************

	Private Function GetUserFilter() As String 
		Return SiteCategoryinf.GetUserFilter()
	End Function
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
<p><span class="aspnetmaker">Delete from TABLE: Site Category</span></p>
<p>
	<asp:Hyperlink id="lnkList" Text="Back to List" NavigateUrl="sitecategory_list.aspx" CssClass="aspnetmaker" runat="server" />
	<asp:Label id="lblReturnUrl" Visible="False" CssClass="aspnetmaker" runat="server">sitecategory_list.aspx</asp:Label>
</p>
<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
	<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
</asp:PlaceHolder>
	<!-- Data Source -->
<asp:ObjectDataSource ID="SiteCategoryDataSource"
	TypeName="PMGEN.SiteCategorydal"
	DataObjectTypeName="PMGEN.SiteCategorykey"
	SelectMethod="LoadList"
	DeleteMethod="Delete"
	OnSelecting="SiteCategoryDataSource_Selecting"
	OnSelected="SiteCategoryDataSource_Selected"
	OnDeleting="SiteCategoryDataSource_Deleting"
	OnDeleted="SiteCategoryDataSource_Deleted"
	runat="server">
</asp:ObjectDataSource>
	<!-- DetailsView -->
<asp:DetailsView ID="SiteCategoryDetailsView"
	DataKeyNames="SiteCategoryID"
	DataSourceID="SiteCategoryDataSource"
	DefaultMode="ReadOnly"
	GridLines="None"
	AutoGenerateRows="False" CssClass="ewDetailTable"
	OnInit="SiteCategoryDetailsView_Init"
	OnDataBound="SiteCategoryDetailsView_DataBound"
	OnUnload="SiteCategoryDetailsView_Unload"
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
			<asp:Label runat="server" id="xs_SiteCategoryTypeID"> Site Type</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_SiteCategoryTypeID" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_CategoryName">Category Name</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_CategoryName" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_CategoryTitle">Category Title</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_CategoryTitle" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_CategoryDescription">Category Description</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_CategoryDescription" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_CategoryKeywords">Category Keywords</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_CategoryKeywords" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_SiteCategoryGroupID">Category Group</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_SiteCategoryGroupID" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_GroupOrder">Order</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_GroupOrder" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_ParentCategoryID">Parent Category ID</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_ParentCategoryID" CssClass="aspnetmaker" runat="server" />
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
