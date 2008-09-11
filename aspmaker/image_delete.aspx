<%@ Page EnableViewState="true" Language="VB" ValidateRequest="False" EnableEventValidation="False" Debug="True" MasterPageFile="masterpage.master" %>
<%@ MasterType VirtualPath="masterpage.master" %>
<%@ Import Namespace="PMGEN" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="EW.Data" %>
<%@ Import Namespace="EW.Data.Utilities" %>
<%@ Import Namespace="EW.Web" %>
<%@ Register NameSpace="PMGEN" TagPrefix="PMGEN" %>
<script runat="server">
	Dim key As Imagekey ' record key
	Dim oldrow As Imagerow ' old record
	Dim arrKeys As ArrayList = New ArrayList()
	Dim oldrows As Imagerows ' old records
	Dim objProfile As TableProfile ' table profile

	' *************************
	' *  Handler for Page Init
	' *************************

	Protected Sub Page_Init(ByVal s As Object, ByVal e As EventArgs)
		If Not mhUser.IsAdmin() Then
		  Session("Login_Link") = Request.RawUrl
		  Response.Redirect("/mhweb/login/login.aspx")
		End If
		objProfile = CustomProfile.GetTable(Share.ProjectName, Imageinf.TableVar)
	End Sub	

	' *************************
	' *  Handler for Page Load
	' *************************

	Protected Sub Page_Load(ByVal s As Object, ByVal e As EventArgs)
		Response.Cache.SetCacheability(HttpCacheability.NoCache)
		If (Not Page.IsPostBack) Then
		If (Request.QueryString.Count > 0) Then
			key = New Imagekey()
			Dim messageList As ArrayList = Imageinf.LoadKey(key)
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
			key = TryCast(ViewState("key"), Imagekey)
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

	Protected Sub ImageDetailsView_Init(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			Imagedal.OpenConnection()
		Catch err As Exception
			lblMessage.Text = err.Message
			pnlMessage.Visible = True
		End Try
	End Sub

	' *************************************
	' *  Handler for DetailsView DataBound
	' *************************************

	Protected Sub ImageDetailsView_DataBound(ByVal sender As Object, ByVal e As EventArgs)
		Dim source As DetailsView = ImageDetailsView
		Dim row As Imagerow = TryCast(ImageDetailsView.DataItem, Imagerow) 'get data object
		RowToControl(row, source, Core.CtrlType.View)
	End Sub

	' *********************************
	' *  Handler for DetailsView Unload
	' *********************************

	Protected Sub ImageDetailsView_Unload(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			Imagedal.CloseAndDisposeConnection()
		Catch
		End Try
	End Sub

	' ***********************************
	' *  Handler for Delete button click
	' ***********************************

	Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim sDataObjectTypeName As String = String.Empty
		sDataObjectTypeName = ImageDataSource.DataObjectTypeName
		ImageDataSource.DataObjectTypeName = ""
		ImageDataSource.Delete()
		ImageDataSource.DataObjectTypeName = sDataObjectTypeName
	End Sub

	' ********************************************
	' *  Convert Row Object Value To Input Control
	' ********************************************

	Private Sub RowToControl(ByVal row As Imagerow, ByVal control As WebControl, ByVal ct As Core.CtrlType)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		If (ct = Core.CtrlType.View) Then ' set up view controls

			' Field ImageID
			Dim x_ImageID As Label = TryCast(control.FindControl("x_ImageID"), Label)
			If (row.ImageID.HasValue) Then x_ImageID.Text = row.ImageID.ToString() Else x_ImageID.Text = String.Empty

			' Field ImageName
			Dim x_ImageName As Label = TryCast(control.FindControl("x_ImageName"), Label)
			If (row.ImageName IsNot Nothing) Then x_ImageName.Text = row.ImageName.ToString() Else x_ImageName.Text = String.Empty

			' Field Active
			Dim x_Active As CheckBox = TryCast(control.FindControl("x_Active"), CheckBox)
			If (row.Active.HasValue) Then
				x_Active.Checked = IIf(CType(row.Active, Boolean), True, False)
			End If

			' Field ImageDescription
			Dim x_ImageDescription As Label = TryCast(control.FindControl("x_ImageDescription"), Label)
			If (row.ImageDescription IsNot Nothing) Then x_ImageDescription.Text = row.ImageDescription.ToString() Else x_ImageDescription.Text = String.Empty

			' Field ImageComment
			Dim x_ImageComment As Label = TryCast(control.FindControl("x_ImageComment"), Label)
			If (row.ImageComment IsNot Nothing) Then x_ImageComment.Text = row.ImageComment.ToString() Else x_ImageComment.Text = String.Empty

			' Field ImageFileName
			Dim x_ImageFileName As Label = TryCast(control.FindControl("x_ImageFileName"), Label)
			If (row.ImageFileName IsNot Nothing) Then x_ImageFileName.Text = row.ImageFileName.ToString() Else x_ImageFileName.Text = String.Empty

			' Field ImageThumbFileName
			Dim x_ImageThumbFileName As Label = TryCast(control.FindControl("x_ImageThumbFileName"), Label)
			If (row.ImageThumbFileName IsNot Nothing) Then x_ImageThumbFileName.Text = row.ImageThumbFileName.ToString() Else x_ImageThumbFileName.Text = String.Empty

			' Field ImageDate
			Dim x_ImageDate As Label = TryCast(control.FindControl("x_ImageDate"), Label)
			If (row.ImageDate.HasValue) Then x_ImageDate.Text = Convert.ToString(row.ImageDate) Else x_ImageDate.Text = String.Empty
			x_ImageDate.Text = DataFormat.DateTimeFormat(6, "/", x_ImageDate.Text)

			' Field ModifiedDT
			Dim x_ModifiedDT As Label = TryCast(control.FindControl("x_ModifiedDT"), Label)
			If (row.ModifiedDT.HasValue) Then x_ModifiedDT.Text = Convert.ToString(row.ModifiedDT) Else x_ModifiedDT.Text = String.Empty
			x_ModifiedDT.Text = DataFormat.DateTimeFormat(6, "/", x_ModifiedDT.Text)

			' Field VersionNo
			Dim x_VersionNo As Label = TryCast(control.FindControl("x_VersionNo"), Label)
			If (row.VersionNo.HasValue) Then x_VersionNo.Text = row.VersionNo.ToString() Else x_VersionNo.Text = String.Empty

			' Field ContactID
			Dim x_ContactID As Label = TryCast(control.FindControl("x_ContactID"), Label)
			Dim v_ContactID As String
			If (row.ContactID.HasValue) Then v_ContactID = row.ContactID.ToString() Else v_ContactID = String.Empty
			x_ContactID.Text = Imagedal.LookUpTable("ContactID", v_ContactID)

			' Field CompanyID
			Dim x_CompanyID As Label = TryCast(control.FindControl("x_CompanyID"), Label)
			Dim v_CompanyID As String
			If (row.CompanyID.HasValue) Then v_CompanyID = row.CompanyID.ToString() Else v_CompanyID = String.Empty
			x_CompanyID.Text = Imagedal.LookUpTable("CompanyID", v_CompanyID)

			' Field title
			Dim x_title As Label = TryCast(control.FindControl("x_title"), Label)
			If (row.title IsNot Nothing) Then x_title.Text = row.title.ToString() Else x_title.Text = String.Empty

			' Field medium
			Dim x_medium As Label = TryCast(control.FindControl("x_medium"), Label)
			If (row.medium IsNot Nothing) Then x_medium.Text = row.medium.ToString() Else x_medium.Text = String.Empty

			' Field size
			Dim x_size As Label = TryCast(control.FindControl("x_size"), Label)
			If (row.size IsNot Nothing) Then x_size.Text = row.size.ToString() Else x_size.Text = String.Empty

			' Field price
			Dim x_price As Label = TryCast(control.FindControl("x_price"), Label)
			If (row.price IsNot Nothing) Then x_price.Text = row.price.ToString() Else x_price.Text = String.Empty

			' Field color
			Dim x_color As Label = TryCast(control.FindControl("x_color"), Label)
			If (row.color IsNot Nothing) Then x_color.Text = row.color.ToString() Else x_color.Text = String.Empty

			' Field subject
			Dim x_subject As Label = TryCast(control.FindControl("x_subject"), Label)
			If (row.subject IsNot Nothing) Then x_subject.Text = row.subject.ToString() Else x_subject.Text = String.Empty

			' Field sold
			Dim x_sold As CheckBox = TryCast(control.FindControl("x_sold"), CheckBox)
			If (row.sold.HasValue) Then
				x_sold.Checked = IIf(CType(row.sold, Boolean), True, False)
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

	Protected Sub ImageDataSource_Selecting(Byval sender As Object, ByVal e As ObjectDataSourceSelectingEventArgs)
		e.InputParameters.Clear()
		e.InputParameters.Add("filter", Imageinf.GetUserFilter())
	End Sub

	' *************************************
	' *  DataSource Selected Event Handler
	' *************************************

	Protected Sub ImageDataSource_Selected(Byval sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.ReturnValue Is Nothing) Then
			If (e.Exception IsNot Nothing) Then
				lblMessage.Text = e.Exception.Message
				pnlMessage.Visible = True
			Else
				lblMessage.Text = "No records found"
				pnlMessage.Visible = True
			End If
		Else
			Dim rows As Imagerows = TryCast(e.ReturnValue, Imagerows)
			Dim i As Integer = 0
			Do While (rows IsNot Nothing AndAlso i < rows.Count)
				If (key.Equals(rows(i).GetKey())) Then
					ImageDetailsView.PageIndex = i
					Exit Do
				End If
				i += 1
			Loop
		End If
	End Sub

	' ***********************************
	' *  Handler for DataSource Deleting
	' ***********************************

	Protected Sub ImageDataSource_Deleting(ByVal sender As Object, ByVal e As ObjectDataSourceMethodEventArgs)
		Dim data As Imagedal = New Imagedal()
		Dim sWhere As String = String.Empty
		arrKeys.Add(key)
		oldrows = New Imagerows()
		oldrows.Add(data.LoadRow(key, "")) ' load row without filter
		If (Imagebll.Deleting(oldrows)) Then
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

	Protected Sub ImageDataSource_Deleted(ByVal sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.Exception IsNot Nothing) Then
			e.ExceptionHandled = True
			lblMessage.Text = e.Exception.InnerException.Message
		Else
			objProfile.Message = "Delete Successful"
			Imagebll.Deleted(oldrows)
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
		Dim data As Imagedal = New Imagedal()
		Dim rows As Imagerows = data.LoadList(Imageinf.GetUserFilter())
		If (rows IsNot Nothing AndAlso rows.Count >= e.NewPageIndex) Then
			key = rows(e.NewPageIndex).GetKey()
		End If
	End Sub

	' *******************
	' *  Get User Filter
	' *******************

	Private Function GetUserFilter() As String 
		Return Imageinf.GetUserFilter()
	End Function
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
<p><span class="aspnetmaker">Delete from TABLE: Image</span></p>
<p>
	<asp:Hyperlink id="lnkList" Text="Back to List" NavigateUrl="image_list.aspx" CssClass="aspnetmaker" runat="server" />
	<asp:Label id="lblReturnUrl" Visible="False" CssClass="aspnetmaker" runat="server">image_list.aspx</asp:Label>
</p>
<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
	<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
</asp:PlaceHolder>
	<!-- Data Source -->
<asp:ObjectDataSource ID="ImageDataSource"
	TypeName="PMGEN.Imagedal"
	DataObjectTypeName="PMGEN.Imagekey"
	SelectMethod="LoadList"
	DeleteMethod="Delete"
	OnSelecting="ImageDataSource_Selecting"
	OnSelected="ImageDataSource_Selected"
	OnDeleting="ImageDataSource_Deleting"
	OnDeleted="ImageDataSource_Deleted"
	runat="server">
</asp:ObjectDataSource>
	<!-- DetailsView -->
<asp:DetailsView ID="ImageDetailsView"
	DataKeyNames="ImageID"
	DataSourceID="ImageDataSource"
	DefaultMode="ReadOnly"
	GridLines="None"
	AutoGenerateRows="False" CssClass="ewDetailTable"
	OnInit="ImageDetailsView_Init"
	OnDataBound="ImageDetailsView_DataBound"
	OnUnload="ImageDetailsView_Unload"
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
			<asp:Label runat="server" id="xs_ImageID">Image ID</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_ImageID" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_ImageName">Name</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_ImageName" CssClass="aspnetmaker" runat="server" />
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
			<asp:Label runat="server" id="xs_ImageDescription">Description</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_ImageDescription" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_ImageComment">Comment</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_ImageComment" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_ImageFileName">File URL</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_ImageFileName" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_ImageThumbFileName">Thumbnail URL</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_ImageThumbFileName" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_ImageDate">Created</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_ImageDate" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_ModifiedDT">Modified</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_ModifiedDT" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_VersionNo">Version</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_VersionNo" CssClass="aspnetmaker" runat="server" />
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
			<asp:Label runat="server" id="xs_CompanyID">Company</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_CompanyID" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_title">Title</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_title" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_medium">Medium</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_medium" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_size">Size</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_size" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_price">Price</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_price" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_color">Color</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_color" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_subject">Subject</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:Label id="x_subject" CssClass="aspnetmaker" runat="server" />
		</ItemTemplate>
	</asp:TemplateField>
	<asp:TemplateField>
		<HeaderStyle CssClass="ewTableHeader" />
		<HeaderTemplate>
			<asp:Label runat="server" id="xs_sold">Sold</asp:Label>
		</HeaderTemplate>
		<ItemTemplate>
<asp:CheckBox id="x_sold" Enabled="False"  CssClass="aspnetmaker" runat="server" />
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
