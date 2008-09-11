<%@ Page EnableViewState="true" Language="VB" ValidateRequest="False" Debug="True" MasterPageFile="masterpage.master" %>
<%@ MasterType VirtualPath="masterpage.master" %>
<%@ Import Namespace="PMGEN" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="EW.Data" %>
<%@ Import Namespace="EW.Data.Utilities" %>
<%@ Import Namespace="EW.Web" %>
<%@ Register NameSpace="PMGEN" TagPrefix="PMGEN" %>
<script runat="server">
	Dim key As LinkTypekey ' record key
	Dim oldrow As LinkTyperow ' old record
	Dim arrKeys As ArrayList = New ArrayList()
	Dim oldrows As LinkTyperows ' old records
	Dim objProfile As TableProfile ' table profile

	' *************************
	' *  Handler for Page Init
	' *************************

	Protected Sub Page_Init(ByVal s As Object, ByVal e As EventArgs)
		If Not mhUser.IsAdmin() Then
		  Session("Login_Link") = Request.RawUrl
		  Response.Redirect("/mhweb/login/login.aspx")
		End If
		objProfile = CustomProfile.GetTable(Share.ProjectName, LinkTypeinf.TableVar)
	End Sub	

	' *************************
	' *  Handler for Page Load
	' *************************

	Protected Sub Page_Load(ByVal s As Object, ByVal e As EventArgs)
		Response.Cache.SetCacheability(HttpCacheability.NoCache)
		If (Not Page.IsPostBack) Then
		If (Request.QueryString.Count > 0) Then
			key = New LinkTypekey()
			Dim messageList As ArrayList = LinkTypeinf.LoadKey(key)
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
			key = TryCast(ViewState("key"), LinkTypekey)
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

	Protected Sub LinkTypeDetailsView_Init(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			LinkTypedal.OpenConnection()
		Catch err As Exception
			lblMessage.Text = err.Message
			pnlMessage.Visible = True
		End Try
	End Sub

	' *************************************
	' *  Handler for DetailsView DataBound
	' *************************************

	Protected Sub LinkTypeDetailsView_DataBound(ByVal sender As Object, ByVal e As EventArgs)
		Dim source As DetailsView = LinkTypeDetailsView
		Dim row As LinkTyperow = TryCast(LinkTypeDetailsView.DataItem, LinkTyperow) 'get data object
		RowToControl(row, source, Core.CtrlType.View)
	End Sub

	' *********************************
	' *  Handler for DetailsView Unload
	' *********************************

	Protected Sub LinkTypeDetailsView_Unload(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			LinkTypedal.CloseAndDisposeConnection()
		Catch
		End Try
	End Sub

	' ***********************************
	' *  Handler for Delete button click
	' ***********************************

	Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim sDataObjectTypeName As String = String.Empty
		sDataObjectTypeName = LinkTypeDataSource.DataObjectTypeName
		LinkTypeDataSource.DataObjectTypeName = ""
		LinkTypeDataSource.Delete()
		LinkTypeDataSource.DataObjectTypeName = sDataObjectTypeName
	End Sub

	' ********************************************
	' *  Convert Row Object Value To Input Control
	' ********************************************

	Private Sub RowToControl(ByVal row As LinkTyperow, ByVal control As WebControl, ByVal ct As Core.CtrlType)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		If (ct = Core.CtrlType.View) Then ' set up view controls

			' Field LinkTypeCD
			Dim x_LinkTypeCD As Label = TryCast(control.FindControl("x_LinkTypeCD"), Label)
			If (row.LinkTypeCD IsNot Nothing) Then x_LinkTypeCD.Text = row.LinkTypeCD.ToString() Else x_LinkTypeCD.Text = String.Empty

			' Field LinkTypeDesc
			Dim x_LinkTypeDesc As Label = TryCast(control.FindControl("x_LinkTypeDesc"), Label)
			If (row.LinkTypeDesc IsNot Nothing) Then x_LinkTypeDesc.Text = row.LinkTypeDesc.ToString() Else x_LinkTypeDesc.Text = String.Empty

			' Field LinkTypeComment
			Dim x_LinkTypeComment As Label = TryCast(control.FindControl("x_LinkTypeComment"), Label)
			If (row.LinkTypeComment IsNot Nothing) Then x_LinkTypeComment.Text = row.LinkTypeComment.ToString() Else x_LinkTypeComment.Text = String.Empty

			' Field LinkTypeTarget
			Dim x_LinkTypeTarget As Label = TryCast(control.FindControl("x_LinkTypeTarget"), Label)
			If (row.LinkTypeTarget IsNot Nothing) Then x_LinkTypeTarget.Text = row.LinkTypeTarget.ToString() Else x_LinkTypeTarget.Text = String.Empty
		End If
	End Sub

	' ************************************
	' *  Handler for DataSource Selecting
	' ************************************

	Protected Sub LinkTypeDataSource_Selecting(Byval sender As Object, ByVal e As ObjectDataSourceSelectingEventArgs)
		e.InputParameters.Clear()
		e.InputParameters.Add("filter", LinkTypeinf.GetUserFilter())
	End Sub

	' *************************************
	' *  DataSource Selected Event Handler
	' *************************************

	Protected Sub LinkTypeDataSource_Selected(Byval sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.ReturnValue Is Nothing) Then
			If (e.Exception IsNot Nothing) Then
				lblMessage.Text = e.Exception.Message
				pnlMessage.Visible = True
			Else
				lblMessage.Text = "No records found"
				pnlMessage.Visible = True
			End If
		Else
			Dim rows As LinkTyperows = TryCast(e.ReturnValue, LinkTyperows)
			Dim i As Integer = 0
			Do While (rows IsNot Nothing AndAlso i < rows.Count)
				If (key.Equals(rows(i).GetKey())) Then
					LinkTypeDetailsView.PageIndex = i
					Exit Do
				End If
				i += 1
			Loop
		End If
	End Sub

	' ***********************************
	' *  Handler for DataSource Deleting
	' ***********************************

	Protected Sub LinkTypeDataSource_Deleting(ByVal sender As Object, ByVal e As ObjectDataSourceMethodEventArgs)
		Dim data As LinkTypedal = New LinkTypedal()
		Dim sWhere As String = String.Empty
		arrKeys.Add(key)
		oldrows = New LinkTyperows()
		oldrows.Add(data.LoadRow(key, "")) ' load row without filter
		If (LinkTypebll.Deleting(oldrows)) Then
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

	Protected Sub LinkTypeDataSource_Deleted(ByVal sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.Exception IsNot Nothing) Then
			e.ExceptionHandled = True
			lblMessage.Text = e.Exception.InnerException.Message
		Else
			objProfile.Message = "Delete Successful"
			LinkTypebll.Deleted(oldrows)
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
		Dim data As LinkTypedal = New LinkTypedal()
		Dim rows As LinkTyperows = data.LoadList(LinkTypeinf.GetUserFilter())
		If (rows IsNot Nothing AndAlso rows.Count >= e.NewPageIndex) Then
			key = rows(e.NewPageIndex).GetKey()
		End If
	End Sub

	' *******************
	' *  Get User Filter
	' *******************

	Private Function GetUserFilter() As String 
		Return LinkTypeinf.GetUserFilter()
	End Function
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
<p><span class="aspnetmaker">Delete from TABLE: Link Type</span></p>
<p>
	<asp:Hyperlink id="lnkList" Text="Back to List" NavigateUrl="linktype_list.aspx" CssClass="aspnetmaker" runat="server" />
	<asp:Label id="lblReturnUrl" Visible="False" CssClass="aspnetmaker" runat="server">linktype_list.aspx</asp:Label>
</p>
<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
	<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
</asp:PlaceHolder>
	<!-- Data Source -->
<asp:ObjectDataSource ID="LinkTypeDataSource"
	TypeName="PMGEN.LinkTypedal"
	DataObjectTypeName="PMGEN.LinkTypekey"
	SelectMethod="LoadList"
	DeleteMethod="Delete"
	OnSelecting="LinkTypeDataSource_Selecting"
	OnSelected="LinkTypeDataSource_Selected"
	OnDeleting="LinkTypeDataSource_Deleting"
	OnDeleted="LinkTypeDataSource_Deleted"
	runat="server">
</asp:ObjectDataSource>
	<!-- DetailsView -->
<asp:DetailsView ID="LinkTypeDetailsView"
	DataKeyNames="LinkTypeCD"
	DataSourceID="LinkTypeDataSource"
	DefaultMode="ReadOnly"
	GridLines="None"
	AutoGenerateRows="False" CssClass="ewDetailTable"
	OnInit="LinkTypeDetailsView_Init"
	OnDataBound="LinkTypeDetailsView_DataBound"
	OnUnload="LinkTypeDetailsView_Unload"
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
			<asp:Label runat="server" id="xs_LinkTypeCD">Link Type CD</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_LinkTypeCD" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_LinkTypeDesc">Link Type Desc</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_LinkTypeDesc" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_LinkTypeComment">Link Type Comment</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_LinkTypeComment" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_LinkTypeTarget">Link Type Target</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_LinkTypeTarget" CssClass="aspnetmaker" runat="server" />
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
