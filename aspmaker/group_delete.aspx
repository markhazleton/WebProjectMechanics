<%@ Page EnableViewState="true" Language="VB" ValidateRequest="False" Debug="True" MasterPageFile="masterpage.master" %>
<%@ MasterType VirtualPath="masterpage.master" %>
<%@ Import Namespace="PMGEN" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="EW.Data" %>
<%@ Import Namespace="EW.Data.Utilities" %>
<%@ Import Namespace="EW.Web" %>
<%@ Register NameSpace="PMGEN" TagPrefix="PMGEN" %>
<script runat="server">
	Dim key As Groupkey ' record key
	Dim oldrow As Grouprow ' old record
	Dim arrKeys As ArrayList = New ArrayList()
	Dim oldrows As Grouprows ' old records
	Dim objProfile As TableProfile ' table profile

	' *************************
	' *  Handler for Page Init
	' *************************

	Protected Sub Page_Init(ByVal s As Object, ByVal e As EventArgs)
		If Not mhUser.IsAdmin() Then
		  Session("Login_Link") = Request.RawUrl
		  Response.Redirect("/mhweb/login/login.aspx")
		End If
		objProfile = CustomProfile.GetTable(Share.ProjectName, Groupinf.TableVar)
	End Sub	

	' *************************
	' *  Handler for Page Load
	' *************************

	Protected Sub Page_Load(ByVal s As Object, ByVal e As EventArgs)
		Response.Cache.SetCacheability(HttpCacheability.NoCache)
		If (Not Page.IsPostBack) Then
		If (Request.QueryString.Count > 0) Then
			key = New Groupkey()
			Dim messageList As ArrayList = Groupinf.LoadKey(key)
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
			key = TryCast(ViewState("key"), Groupkey)
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

	Protected Sub GroupDetailsView_Init(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			Groupdal.OpenConnection()
		Catch err As Exception
			lblMessage.Text = err.Message
			pnlMessage.Visible = True
		End Try
	End Sub

	' *************************************
	' *  Handler for DetailsView DataBound
	' *************************************

	Protected Sub GroupDetailsView_DataBound(ByVal sender As Object, ByVal e As EventArgs)
		Dim source As DetailsView = GroupDetailsView
		Dim row As Grouprow = TryCast(GroupDetailsView.DataItem, Grouprow) 'get data object
		RowToControl(row, source, Core.CtrlType.View)
	End Sub

	' *********************************
	' *  Handler for DetailsView Unload
	' *********************************

	Protected Sub GroupDetailsView_Unload(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			Groupdal.CloseAndDisposeConnection()
		Catch
		End Try
	End Sub

	' ***********************************
	' *  Handler for Delete button click
	' ***********************************

	Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim sDataObjectTypeName As String = String.Empty
		sDataObjectTypeName = GroupDataSource.DataObjectTypeName
		GroupDataSource.DataObjectTypeName = ""
		GroupDataSource.Delete()
		GroupDataSource.DataObjectTypeName = sDataObjectTypeName
	End Sub

	' ********************************************
	' *  Convert Row Object Value To Input Control
	' ********************************************

	Private Sub RowToControl(ByVal row As Grouprow, ByVal control As WebControl, ByVal ct As Core.CtrlType)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		If (ct = Core.CtrlType.View) Then ' set up view controls

			' Field GroupID
			Dim x_GroupID As Label = TryCast(control.FindControl("x_GroupID"), Label)
			If (row.GroupID.HasValue) Then x_GroupID.Text = row.GroupID.ToString() Else x_GroupID.Text = String.Empty

			' Field GroupName
			Dim x_GroupName As Label = TryCast(control.FindControl("x_GroupName"), Label)
			If (row.GroupName IsNot Nothing) Then x_GroupName.Text = row.GroupName.ToString() Else x_GroupName.Text = String.Empty

			' Field GroupComment
			Dim x_GroupComment As Label = TryCast(control.FindControl("x_GroupComment"), Label)
			If (row.GroupComment IsNot Nothing) Then x_GroupComment.Text = row.GroupComment.ToString() Else x_GroupComment.Text = String.Empty
		End If
	End Sub

	' ************************************
	' *  Handler for DataSource Selecting
	' ************************************

	Protected Sub GroupDataSource_Selecting(Byval sender As Object, ByVal e As ObjectDataSourceSelectingEventArgs)
		e.InputParameters.Clear()
		e.InputParameters.Add("filter", Groupinf.GetUserFilter())
	End Sub

	' *************************************
	' *  DataSource Selected Event Handler
	' *************************************

	Protected Sub GroupDataSource_Selected(Byval sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.ReturnValue Is Nothing) Then
			If (e.Exception IsNot Nothing) Then
				lblMessage.Text = e.Exception.Message
				pnlMessage.Visible = True
			Else
				lblMessage.Text = "No records found"
				pnlMessage.Visible = True
			End If
		Else
			Dim rows As Grouprows = TryCast(e.ReturnValue, Grouprows)
			Dim i As Integer = 0
			Do While (rows IsNot Nothing AndAlso i < rows.Count)
				If (key.Equals(rows(i).GetKey())) Then
					GroupDetailsView.PageIndex = i
					Exit Do
				End If
				i += 1
			Loop
		End If
	End Sub

	' ***********************************
	' *  Handler for DataSource Deleting
	' ***********************************

	Protected Sub GroupDataSource_Deleting(ByVal sender As Object, ByVal e As ObjectDataSourceMethodEventArgs)
		Dim data As Groupdal = New Groupdal()
		Dim sWhere As String = String.Empty
		arrKeys.Add(key)
		oldrows = New Grouprows()
		oldrows.Add(data.LoadRow(key, "")) ' load row without filter
		If (Groupbll.Deleting(oldrows)) Then
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

	Protected Sub GroupDataSource_Deleted(ByVal sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.Exception IsNot Nothing) Then
			e.ExceptionHandled = True
			lblMessage.Text = e.Exception.InnerException.Message
		Else
			objProfile.Message = "Delete Successful"
			Groupbll.Deleted(oldrows)
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
		Dim data As Groupdal = New Groupdal()
		Dim rows As Grouprows = data.LoadList(Groupinf.GetUserFilter())
		If (rows IsNot Nothing AndAlso rows.Count >= e.NewPageIndex) Then
			key = rows(e.NewPageIndex).GetKey()
		End If
	End Sub

	' *******************
	' *  Get User Filter
	' *******************

	Private Function GetUserFilter() As String 
		Return Groupinf.GetUserFilter()
	End Function
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
<p><span class="aspnetmaker">Delete from TABLE: Group</span></p>
<p>
	<asp:Hyperlink id="lnkList" Text="Back to List" NavigateUrl="group_list.aspx" CssClass="aspnetmaker" runat="server" />
	<asp:Label id="lblReturnUrl" Visible="False" CssClass="aspnetmaker" runat="server">group_list.aspx</asp:Label>
</p>
<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
	<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
</asp:PlaceHolder>
	<!-- Data Source -->
<asp:ObjectDataSource ID="GroupDataSource"
	TypeName="PMGEN.Groupdal"
	DataObjectTypeName="PMGEN.Groupkey"
	SelectMethod="LoadList"
	DeleteMethod="Delete"
	OnSelecting="GroupDataSource_Selecting"
	OnSelected="GroupDataSource_Selected"
	OnDeleting="GroupDataSource_Deleting"
	OnDeleted="GroupDataSource_Deleted"
	runat="server">
</asp:ObjectDataSource>
	<!-- DetailsView -->
<asp:DetailsView ID="GroupDetailsView"
	DataKeyNames="GroupID"
	DataSourceID="GroupDataSource"
	DefaultMode="ReadOnly"
	GridLines="None"
	AutoGenerateRows="False" CssClass="ewDetailTable"
	OnInit="GroupDetailsView_Init"
	OnDataBound="GroupDetailsView_DataBound"
	OnUnload="GroupDetailsView_Unload"
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
			<asp:Label runat="server" id="xs_GroupID">Group</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_GroupID" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_GroupName">Name</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_GroupName" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_GroupComment">Comment</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_GroupComment" CssClass="aspnetmaker" runat="server" />
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
