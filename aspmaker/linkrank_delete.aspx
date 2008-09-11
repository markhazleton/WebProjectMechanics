<%@ Page EnableViewState="true" Language="VB" ValidateRequest="False" Debug="True" MasterPageFile="masterpage.master" %>
<%@ MasterType VirtualPath="masterpage.master" %>
<%@ Import Namespace="PMGEN" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="EW.Data" %>
<%@ Import Namespace="EW.Data.Utilities" %>
<%@ Import Namespace="EW.Web" %>
<%@ Register NameSpace="PMGEN" TagPrefix="PMGEN" %>
<script runat="server">
	Dim key As LinkRankkey ' record key
	Dim oldrow As LinkRankrow ' old record
	Dim arrKeys As ArrayList = New ArrayList()
	Dim oldrows As LinkRankrows ' old records
	Dim objProfile As TableProfile ' table profile

	' *************************
	' *  Handler for Page Init
	' *************************

	Protected Sub Page_Init(ByVal s As Object, ByVal e As EventArgs)
		If Not mhUser.IsAdmin() Then
		  Session("Login_Link") = Request.RawUrl
		  Response.Redirect("/mhweb/login/login.aspx")
		End If
		objProfile = CustomProfile.GetTable(Share.ProjectName, LinkRankinf.TableVar)
	End Sub	

	' *************************
	' *  Handler for Page Load
	' *************************

	Protected Sub Page_Load(ByVal s As Object, ByVal e As EventArgs)
		Response.Cache.SetCacheability(HttpCacheability.NoCache)
		If (Not Page.IsPostBack) Then
		If (Request.QueryString.Count > 0) Then
			key = New LinkRankkey()
			Dim messageList As ArrayList = LinkRankinf.LoadKey(key)
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
			key = TryCast(ViewState("key"), LinkRankkey)
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

	Protected Sub LinkRankDetailsView_Init(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			LinkRankdal.OpenConnection()
		Catch err As Exception
			lblMessage.Text = err.Message
			pnlMessage.Visible = True
		End Try
	End Sub

	' *************************************
	' *  Handler for DetailsView DataBound
	' *************************************

	Protected Sub LinkRankDetailsView_DataBound(ByVal sender As Object, ByVal e As EventArgs)
		Dim source As DetailsView = LinkRankDetailsView
		Dim row As LinkRankrow = TryCast(LinkRankDetailsView.DataItem, LinkRankrow) 'get data object
		RowToControl(row, source, Core.CtrlType.View)
	End Sub

	' *********************************
	' *  Handler for DetailsView Unload
	' *********************************

	Protected Sub LinkRankDetailsView_Unload(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			LinkRankdal.CloseAndDisposeConnection()
		Catch
		End Try
	End Sub

	' ***********************************
	' *  Handler for Delete button click
	' ***********************************

	Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim sDataObjectTypeName As String = String.Empty
		sDataObjectTypeName = LinkRankDataSource.DataObjectTypeName
		LinkRankDataSource.DataObjectTypeName = ""
		LinkRankDataSource.Delete()
		LinkRankDataSource.DataObjectTypeName = sDataObjectTypeName
	End Sub

	' ********************************************
	' *  Convert Row Object Value To Input Control
	' ********************************************

	Private Sub RowToControl(ByVal row As LinkRankrow, ByVal control As WebControl, ByVal ct As Core.CtrlType)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		If (ct = Core.CtrlType.View) Then ' set up view controls

			' Field ID
			Dim x_ID As Label = TryCast(control.FindControl("x_ID"), Label)
			If (row.ID.HasValue) Then x_ID.Text = row.ID.ToString() Else x_ID.Text = String.Empty

			' Field LinkID
			Dim x_LinkID As Label = TryCast(control.FindControl("x_LinkID"), Label)
			If (row.LinkID.HasValue) Then x_LinkID.Text = row.LinkID.ToString() Else x_LinkID.Text = String.Empty

			' Field UserID
			Dim x_UserID As Label = TryCast(control.FindControl("x_UserID"), Label)
			If (row.UserID.HasValue) Then x_UserID.Text = row.UserID.ToString() Else x_UserID.Text = String.Empty

			' Field RankNum
			Dim x_RankNum As Label = TryCast(control.FindControl("x_RankNum"), Label)
			If (row.RankNum.HasValue) Then x_RankNum.Text = row.RankNum.ToString() Else x_RankNum.Text = String.Empty

			' Field CateID
			Dim x_CateID As Label = TryCast(control.FindControl("x_CateID"), Label)
			If (row.CateID.HasValue) Then x_CateID.Text = row.CateID.ToString() Else x_CateID.Text = String.Empty

			' Field Comment
			Dim x_Comment As Label = TryCast(control.FindControl("x_Comment"), Label)
			If (row.Comment IsNot Nothing) Then x_Comment.Text = row.Comment.ToString() Else x_Comment.Text = String.Empty
		End If
	End Sub

	' ************************************
	' *  Handler for DataSource Selecting
	' ************************************

	Protected Sub LinkRankDataSource_Selecting(Byval sender As Object, ByVal e As ObjectDataSourceSelectingEventArgs)
		e.InputParameters.Clear()
		e.InputParameters.Add("filter", LinkRankinf.GetUserFilter())
	End Sub

	' *************************************
	' *  DataSource Selected Event Handler
	' *************************************

	Protected Sub LinkRankDataSource_Selected(Byval sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.ReturnValue Is Nothing) Then
			If (e.Exception IsNot Nothing) Then
				lblMessage.Text = e.Exception.Message
				pnlMessage.Visible = True
			Else
				lblMessage.Text = "No records found"
				pnlMessage.Visible = True
			End If
		Else
			Dim rows As LinkRankrows = TryCast(e.ReturnValue, LinkRankrows)
			Dim i As Integer = 0
			Do While (rows IsNot Nothing AndAlso i < rows.Count)
				If (key.Equals(rows(i).GetKey())) Then
					LinkRankDetailsView.PageIndex = i
					Exit Do
				End If
				i += 1
			Loop
		End If
	End Sub

	' ***********************************
	' *  Handler for DataSource Deleting
	' ***********************************

	Protected Sub LinkRankDataSource_Deleting(ByVal sender As Object, ByVal e As ObjectDataSourceMethodEventArgs)
		Dim data As LinkRankdal = New LinkRankdal()
		Dim sWhere As String = String.Empty
		arrKeys.Add(key)
		oldrows = New LinkRankrows()
		oldrows.Add(data.LoadRow(key, "")) ' load row without filter
		If (LinkRankbll.Deleting(oldrows)) Then
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

	Protected Sub LinkRankDataSource_Deleted(ByVal sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.Exception IsNot Nothing) Then
			e.ExceptionHandled = True
			lblMessage.Text = e.Exception.InnerException.Message
		Else
			objProfile.Message = "Delete Successful"
			LinkRankbll.Deleted(oldrows)
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
		Dim data As LinkRankdal = New LinkRankdal()
		Dim rows As LinkRankrows = data.LoadList(LinkRankinf.GetUserFilter())
		If (rows IsNot Nothing AndAlso rows.Count >= e.NewPageIndex) Then
			key = rows(e.NewPageIndex).GetKey()
		End If
	End Sub

	' *******************
	' *  Get User Filter
	' *******************

	Private Function GetUserFilter() As String 
		Return LinkRankinf.GetUserFilter()
	End Function
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
<p><span class="aspnetmaker">Delete from TABLE: Link Rank</span></p>
<p>
	<asp:Hyperlink id="lnkList" Text="Back to List" NavigateUrl="linkrank_list.aspx" CssClass="aspnetmaker" runat="server" />
	<asp:Label id="lblReturnUrl" Visible="False" CssClass="aspnetmaker" runat="server">linkrank_list.aspx</asp:Label>
</p>
<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
	<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
</asp:PlaceHolder>
	<!-- Data Source -->
<asp:ObjectDataSource ID="LinkRankDataSource"
	TypeName="PMGEN.LinkRankdal"
	DataObjectTypeName="PMGEN.LinkRankkey"
	SelectMethod="LoadList"
	DeleteMethod="Delete"
	OnSelecting="LinkRankDataSource_Selecting"
	OnSelected="LinkRankDataSource_Selected"
	OnDeleting="LinkRankDataSource_Deleting"
	OnDeleted="LinkRankDataSource_Deleted"
	runat="server">
</asp:ObjectDataSource>
	<!-- DetailsView -->
<asp:DetailsView ID="LinkRankDetailsView"
	DataKeyNames="ID"
	DataSourceID="LinkRankDataSource"
	DefaultMode="ReadOnly"
	GridLines="None"
	AutoGenerateRows="False" CssClass="ewDetailTable"
	OnInit="LinkRankDetailsView_Init"
	OnDataBound="LinkRankDetailsView_DataBound"
	OnUnload="LinkRankDetailsView_Unload"
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
			<asp:Label runat="server" id="xs_ID">ID</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_ID" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_LinkID">Link ID</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_LinkID" CssClass="aspnetmaker" runat="server" />
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
			<asp:Label runat="server" id="xs_RankNum">Rank Num</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_RankNum" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_CateID">Cate ID</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_CateID" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_Comment">Comment</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_Comment" CssClass="aspnetmaker" runat="server" />
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
