<%@ Page EnableViewState="true" Language="VB" ValidateRequest="False" Debug="True" MasterPageFile="masterpage.master" %>
<%@ MasterType VirtualPath="masterpage.master" %>
<%@ Import Namespace="PMGEN" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="EW.Data" %>
<%@ Import Namespace="EW.Data.Utilities" %>
<%@ Import Namespace="EW.Web" %>
<%@ Register NameSpace="PMGEN" TagPrefix="PMGEN" %>
<script runat="server">
	Dim key As PageImagekey ' record key
	Dim oldrow As PageImagerow ' old record
	Dim arrKeys As ArrayList = New ArrayList()
	Dim oldrows As PageImagerows ' old records
	Dim objProfile As TableProfile ' table profile

	' *************************
	' *  Handler for Page Init
	' *************************

	Protected Sub Page_Init(ByVal s As Object, ByVal e As EventArgs)
		If Not mhUser.IsAdmin() Then
		  Session("Login_Link") = Request.RawUrl
		  Response.Redirect("/mhweb/login/login.aspx")
		End If
		objProfile = CustomProfile.GetTable(Share.ProjectName, PageImageinf.TableVar)
	End Sub	

	' ***********************************
	' *  Handler for Cancel button click
	' ***********************************

	Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim arrkeys As ArrayList = New ArrayList()
		For Each row As GridViewRow In PageImageGridView.Rows
			Dim cell As TableCell = row.Cells(0)
			Dim rowkey As HiddenField = TryCast(cell.FindControl("KeyValues"), HiddenField)
			If (rowkey IsNot Nothing AndAlso rowkey.Value <> "") Then
				Dim arrKey As String() = rowkey.Value.Split(vbCr)
				Dim key As PageImagekey = New PageImagekey()
				key.PageImageID = Convert.ToInt32(arrKey(0))
				arrKeys.Add(key)
			End If
		Next
		Dim data As PageImagedal = New PageImagedal()
		Try
			Dim strFilter As String = data.KeyFilter(arrKeys)
			oldrows = data.LoadList(strFilter)
			If (PageImagebll.Deleting(oldrows)) Then
				data.Delete(arrKeys)
				PageImagebll.Deleted(oldrows)
			End If
			objProfile.Message = "Delete Successful"
			Response.Redirect(lblReturnUrl.Text)
		Catch oErr As Exception
			lblMessage.Text = oErr.Message
			pnlMessage.Visible = True
		End Try
	End Sub

	' *************************
	' *  Handler for Page Load
	' *************************

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
		Response.Cache.SetCacheability(HttpCacheability.NoCache)
		If (Not Page.IsPostBack) Then
			If (PreviousPage Is Nothing) Then Response.Redirect(lblReturnUrl.Text)
			arrKeys = GetKeys()
			If (arrKeys.Count = 0) Then
				Response.Redirect(lblReturnUrl.Text)
			Else
				ViewState("keys") = arrKeys
			End If
		Else
			arrKeys = TryCast(ViewState("keys"), ArrayList)
		End If
	End Sub

	' *************************************
	' *  Get key values from previous page
	' *************************************

	Private Function GetKeys() As ArrayList 
		Dim arrkeys As ArrayList = New ArrayList()
		If (PreviousPage IsNot Nothing) Then
			Dim sKey As String = String.Empty
			Dim arrKey As String()
			Dim ppGridView As GridView
			ppGridView = CType(FindControl(PreviousPage, "PageImageGridView"), GridView)
			If (ppGridView IsNot Nothing) Then ' PreviousPage contains a GridView
				For Each row As GridViewRow In ppGridView.Rows
					If (row.RowType = DataControlRowType.DataRow) Then
						If (row.FindControl("DeleteCheckBox") IsNot Nothing)
							Dim checkbox As HtmlInputCheckBox = TryCast(row.FindControl("DeleteCheckBox"), HtmlInputCheckBox)
							If (checkbox.Checked) Then
								arrKey = checkbox.Value.Split(vbCr)
								Dim key As PageImagekey = New PageImagekey()
								key.PageImageID = Convert.ToInt32(arrKey(0))
								arrkeys.Add(key)
							End If
						End If
					End If
				Next
			End If
		End If
		Return arrkeys
	End Function

	' **********************************************************************
	' * Function For Finding a Control Within a Control.Controls Recursivly
	' **********************************************************************

	Protected Overloads Function FindControl(ByVal obj As Control, ByVal id As String) As Control 
		If (obj.FindControl(id) IsNot Nothing) Then
			Return obj.FindControl(id)
		End If
		If (obj.HasControls()) Then
			For Each ctrl As Control In obj.Controls
				If (FindControl(ctrl, id) IsNot Nothing AndAlso CType(FindControl(ctrl, id),Control).ID = id) Then
					Return FindControl(ctrl, id)
				End If
			Next
		End If
		Return Nothing
	End Function

	' *****************************
	' *  Handler for GridView Init
	' *****************************

	Protected Sub PageImageGridView_Init(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			PageImagedal.OpenConnection()
		Catch err As Exception
			PageImageGridView.Visible = False
			lblMessage.Text = err.Message
			pnlMessage.Visible = True
		End Try
	End Sub

	' *************************************
	' *  Handler for GridView RowDataBound
	' *************************************

	Protected Sub PageImageGridView_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
		Dim oInfo As PageImageinf = New PageImageinf()
		If (e.Row.RowType = DataControlRowType.DataRow) Then
			Dim row As PageImagerow = TryCast(e.Row.DataItem, PageImagerow)
			Dim control As GridViewRow = e.Row
			Dim bNormal As Boolean = ((e.Row.RowState And DataControlRowState.Normal) = DataControlRowState.Normal)
			Dim bAlternate As Boolean = ((e.Row.RowState And DataControlRowState.Alternate) = DataControlRowState.Alternate)
			If (bNormal OrElse bAlternate) Then
				RowToControl(row, TryCast(control, WebControl), Core.CtrlType.View)
			End If
		End If
	End Sub

	' *************************************
	' *  Handler for GridView Unload
	' *************************************

	Protected Sub PageImageGridView_Unload(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			PageImagedal.CloseAndDisposeConnection()
		Catch
		End Try
	End Sub

	' ********************************************
	' *  Convert Row Object Value To Input Control
	' ********************************************

	Private Sub RowToControl(ByVal row As PageImagerow, ByVal control As WebControl, ByVal ct As Core.CtrlType)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		If (ct = Core.CtrlType.View) Then ' set up view controls

			' Field PageID
			Dim x_PageID As Label = TryCast(control.FindControl("x_PageID"), Label)
			Dim v_PageID As String
			If (row.PageID.HasValue) Then v_PageID = row.PageID.ToString() Else v_PageID = String.Empty
			x_PageID.Text = PageImagedal.LookUpTable("PageID", v_PageID)

			' Field ImageID
			Dim x_ImageID As Label = TryCast(control.FindControl("x_ImageID"), Label)
			Dim v_ImageID As String
			If (row.ImageID.HasValue) Then v_ImageID = row.ImageID.ToString() Else v_ImageID = String.Empty
			x_ImageID.Text = PageImagedal.LookUpTable("ImageID", v_ImageID)

			' Field PageImagePosition
			Dim x_PageImagePosition As Label = TryCast(control.FindControl("x_PageImagePosition"), Label)
			If (row.PageImagePosition.HasValue) Then x_PageImagePosition.Text = row.PageImagePosition.ToString() Else x_PageImagePosition.Text = String.Empty
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

	Protected Sub PageImageDataSource_Selecting(Byval sender As Object, ByVal e As ObjectDataSourceSelectingEventArgs)
		e.InputParameters.Clear()
		Dim data As PageImagedal = New PageImagedal()
		Dim sWhere As String = data.KeyFilter(arrKeys)
		e.InputParameters.Add("filter", sWhere)
	End Sub

	' *************************************
	' *  DataSource Selected Event Handler
	' *************************************

	Protected Sub PageImageDataSource_Selected(Byval sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.ReturnValue Is Nothing) Then
			If (e.Exception IsNot Nothing) Then
				lblMessage.Text = e.Exception.Message
				pnlMessage.Visible = True
			Else
				lblMessage.Text = "No records found"
				pnlMessage.Visible = True
			End If
		End If
	End Sub

	' ***********************************
	' *  Handler for DataSource Deleting
	' ***********************************

	Protected Sub PageImageDataSource_Deleting(ByVal sender As Object, ByVal e As ObjectDataSourceMethodEventArgs)
		Dim data As PageImagedal = New PageImagedal()
		Dim sWhere As String = String.Empty
		arrKeys = GetKeys()
		For Each key As PageImagekey In arrKeys
			oldrows.Add(data.LoadRow(key, sWhere))
		Next
		If (PageImagebll.Deleting(oldrows)) Then
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

	Protected Sub PageImageDataSource_Deleted(ByVal sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.Exception IsNot Nothing) Then
			e.ExceptionHandled = True
			lblMessage.Text = e.Exception.InnerException.Message
		Else
			objProfile.Message = "Delete Successful"
			PageImagebll.Deleted(oldrows)
			Response.Redirect(lblReturnUrl.Text)
		End If
		If (lblMessage.Text <> "") Then
			pnlMessage.Visible = True
		End If
	End Sub

	' *******************
	' *  Get User Filter
	' *******************

	Private Function GetUserFilter() As String 
		Return PageImageinf.GetUserFilter()
	End Function
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
<p><span class="aspnetmaker">Delete from TABLE: Page Image</span></p>
<p>
	<asp:Hyperlink id="lnkList" Text="Back to List" NavigateUrl="pageimage_list.aspx" CssClass="aspnetmaker" runat="server" />
	<asp:Label id="lblReturnUrl" Visible="False" CssClass="aspnetmaker" runat="server">pageimage_list.aspx</asp:Label>
</p>
<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
	<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
</asp:PlaceHolder>
	<!-- Data Source -->
<asp:ObjectDataSource ID="PageImageDataSource"
	TypeName="PMGEN.PageImagedal"
	SelectMethod="LoadList"
	DeleteMethod="Delete"
	OnSelecting="PageImageDataSource_Selecting"
	OnSelected="PageImageDataSource_Selected"
	OnDeleting="PageImageDataSource_Deleting"
	OnDeleted="PageImageDataSource_Deleted"
	runat="server">
</asp:ObjectDataSource>
	<!-- GridView -->
<asp:GridView ID="PageImageGridView"
	DataKeyNames="PageImageID"
	DataSourceID="PageImageDataSource"
	GridLines="None"
	AutoGenerateColumns="False" CssClass="ewTable"
	OnInit="PageImageGridView_Init"
	OnRowDataBound="PageImageGridView_RowDataBound"
	OnUnload="PageImageGridView_Unload"
	runat="server">
		<HeaderStyle Wrap="False" CssClass="ewTableHeader" />
		<RowStyle CssClass="ewTableRow" />
		<AlternatingRowStyle CssClass="ewTableAltRow" />
	<Columns>
		<asp:TemplateField>
			<ItemStyle Wrap="False" />
			<HeaderTemplate>
				<asp:Label runat="server"  CssClass="ewTableHeader" id="xs_PageID">Page</asp:Label>
			</HeaderTemplate>
			<ItemTemplate>
				<asp:HiddenField ID="KeyValues" runat="server" Value='<%#Eval("PageImageID")%>' />
<asp:Label id="x_PageID" CssClass="aspnetmaker" runat="server" />
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemStyle Wrap="True" />
			<HeaderTemplate>
				<asp:Label runat="server"  CssClass="ewTableHeader" id="xs_ImageID">Image</asp:Label>
			</HeaderTemplate>
			<ItemTemplate>
<asp:Label id="x_ImageID" CssClass="aspnetmaker" runat="server" />
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemStyle Wrap="False" />
			<HeaderTemplate>
				<asp:Label runat="server"  CssClass="ewTableHeader" id="xs_PageImagePosition">Position</asp:Label>
			</HeaderTemplate>
			<ItemTemplate>
<asp:Label id="x_PageImagePosition" CssClass="aspnetmaker" runat="server" />
			</ItemTemplate>
		</asp:TemplateField>
	</Columns>
</asp:GridView>
<br />
<asp:Button ID="btnDelete" runat="server" Text="CONFIRM DELETE" OnClick="btnDelete_Click"/>
</asp:Content>
