<%@ Page EnableViewState="true" Language="VB" ValidateRequest="False" Debug="True" MasterPageFile="masterpage.master" %>
<%@ MasterType VirtualPath="masterpage.master" %>
<%@ Import Namespace="PMGEN" %>
<%@ Import Namespace="EW.Data" %>
<%@ Import Namespace="EW.Security" %>
<%@ Import Namespace="EW.Data.Utilities" %>
<%@ Import Namespace="EW.Web" %>
<%@ Register NameSpace="PMGEN" TagPrefix="PMGEN" %>
<script runat="server">
	Dim sUserFilter As String = string.Empty
	Dim key As PageImagekey ' record key
	Dim oldrow As PageImagerow ' old record data input by user
	Dim newrow As PageImagerow ' new record data input by user
	Dim objProfile As TableProfile ' table profile

	' *************************
	' *  Handler for Page Init
	' *************************

	Protected Sub Page_Init(ByVal s As Object, ByVal e As System.EventArgs)
		If Not mhUser.IsAdmin() Then
		  Session("Login_Link") = Request.RawUrl
		  Response.Redirect("/mhweb/login/login.aspx")
		End If
		objProfile = CustomProfile.GetTable(Share.ProjectName, PageImageinf.TableVar)
	End Sub

	' *************************
	' *  Handler for Page Load
	' *************************

	Protected Sub Page_Load(ByVal s As Object, ByVal e As System.EventArgs)
		Page.ClientScript.RegisterClientScriptInclude("ewv", "ewv.js")
		Response.Cache.SetCacheability(HttpCacheability.NoCache)
		If (Not Page.IsPostBack) Then
		End If
		If (PageImageDetailsView.Visible) Then
			RegisterClientID("CtrlID", PageImageDetailsView)
		End If
			If (PageImageDetailsView.FindControl("x_PageID") IsNot Nothing) Then
				Page.Form.DefaultFocus = PageImageDetailsView.FindControl("x_PageID").ClientID
			End If
	End Sub

	' **********************************************
	' *  Load Copy Data & Default Data to Controls
	' **********************************************

	Private Sub LoadData()
		Dim source As WebControl = TryCast(PageImageDetailsView, WebControl)
		If (oldrow IsNot Nothing) Then ' old record (from recovery) exists, just restore
			RowToControl(oldrow, source, Core.CtrlType.Edit)
			oldrow = Nothing
		ElseIf (key IsNot Nothing) Then ' key loaded, try to copy record
			Dim data As PageImagedal = New PageImagedal()
			Dim row As PageImagerow = data.LoadRow(key, PageImageinf.GetUserFilter())
			If (row Is Nothing) Then
				objProfile.Message = "Record not found"
				Response.Redirect(lblReturnUrl.Text)
			End If
			RowToControl(row, source, Core.CtrlType.Edit)
			row = Nothing
		Else 
			Dim row As PageImagerow = New PageImagerow()
			row.PageID = Convert.ToInt32(0) ' set default value
			row.ImageID = Convert.ToInt32(0) ' set default value
			row.PageImagePosition = Convert.ToInt32(0) ' set default value
			RowToControl(row, source, Core.CtrlType.Edit)
			row = Nothing
		End If
	End Sub

	' *****************************************
	' *  Handler when Cancel button is clicked
	' *****************************************

	Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
		Response.Redirect(lblReturnUrl.Text)
	End Sub

	' ******************************
	' *  Handler for DetailsView Init
	' ******************************

	Protected Sub PageImageDetailsView_Init(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			PageImagedal.OpenConnection()
		Catch err As Exception
			lblMessage.Text = err.Message
			pnlMessage.Visible = True
		End Try
	End Sub

	' *************************************
	' *  Handler for DetailsView DataBound
	' *************************************

	Protected Sub PageImageDetailsView_DataBound(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim source As DetailsView = PageImageDetailsView
		If (PageImageDetailsView.CurrentMode = DetailsViewMode.Insert) Then
			LoadData()
		End If
	End Sub

	' *****************************************
	' *  Handler for DetailsView ItemInserting
	' *****************************************

	Protected Sub PageImageDetailsView_ItemInserting(ByVal sender As Object, ByVal e As DetailsViewInsertEventArgs)
		Dim source As WebControl = TryCast(sender, WebControl)

		' Validate input values
		Dim messageList As ArrayList = ValidateInputValues(CType(sender, Control), Core.PageType.Add)
		If (messageList IsNot Nothing) Then
			lblMessage.Text = String.Empty
			For Each sMsg As String In messageList
				lblMessage.Text += sMsg + "<br>"
			Next
			pnlMessage.Visible = True
			e.Cancel = True
			Return
		End If
		messageList = CheckDuplicateKey(TryCast(sender,WebControl))
		If (messageList IsNot Nothing) Then
			lblMessage.Text = String.Empty
			For Each sMsg As string In messageList
				lblMessage.Text += sMsg + "<br>"
			Next
			pnlMessage.Visible = True
			e.Cancel = True
			Return
		End If
	End Sub

	' ****************************************
	' *  Handler for DetailsView ItemInserted
	' ****************************************

	Protected Sub PageImageDetailsView_ItemInserted(ByVal sender As Object, ByVal e As DetailsViewInsertedEventArgs)
		If (e.AffectedRows = 0) Then

			' Keep in insert mode
			e.KeepInInsertMode = True

			' Get original values
			Dim source As WebControl = TryCast(sender, WebControl)
			If (source Is Nothing) Then Return
			oldrow = New PageImagerow()
			ControlToRow(oldrow, source)
		Else
			objProfile.Message = "Add new record successful"
			Response.Redirect(lblReturnUrl.Text)
		End If
	End Sub

	' *********************************
	' *  Handler for DetailsView Unload
	' *********************************

	Protected Sub PageImageDetailsView_Unload(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			PageImagedal.CloseAndDisposeConnection()
		Catch
		End Try
	End Sub

	' ********************************
	' * Register ClientID of Controls
	' ********************************

	Private Sub RegisterClientID(ByVal sBlockName As String, ByVal control As Control)
		Dim jsString As String = String.Empty
		If (control.FindControl("x_PageID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_PageID'] = '" & control.FindControl("x_PageID").ClientID & "';"
		End If
		If (control.FindControl("x_ImageID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_ImageID'] = '" & control.FindControl("x_ImageID").ClientID & "';"
		End If
		If (control.FindControl("x_PageImagePosition") IsNot Nothing) Then
			jsString &= "ew.Controls['x_PageImagePosition'] = '" & control.FindControl("x_PageImagePosition").ClientID & "';"
		End If
		Page.ClientScript.RegisterClientScriptBlock(System.Type.GetType("System.String"), sBlockName, jsString, True)
	End Sub

	' *****************************************************
	' *  Processing Input Values Before Updating/Inserting
	' *****************************************************

	Private Function ValidateInputValues(ByVal control As Control, ByVal pageType As Core.PageType) As ArrayList
		Dim messageList As ArrayList = New ArrayList()
		DataFormat.SetDateSeparator("/")
		If (pageType <> Core.PageType.Search) Then ' Add/Edit Validation
			Dim x_PageID As DropDownList = TryCast(control.FindControl("x_PageID"), DropDownList)
			If (x_PageID IsNot Nothing) Then
				If ((x_PageID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_PageID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Page")
				ElseIf (String.IsNullOrEmpty(x_PageID.SelectedValue)) Then
					messageList.Add("Please enter required field (Int32): Page")
				End If
			End If
			Dim x_ImageID As DropDownList = TryCast(control.FindControl("x_ImageID"), DropDownList)
			If (x_ImageID IsNot Nothing) Then
				If ((x_ImageID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_ImageID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Image")
				ElseIf (String.IsNullOrEmpty(x_ImageID.SelectedValue)) Then
					messageList.Add("Please enter required field (Int32): Image")
				End If
			End If
			Dim x_PageImagePosition As TextBox = TryCast(control.FindControl("x_PageImagePosition"), TextBox)
			If (x_PageImagePosition IsNot Nothing) Then
				If (Not DataFormat.CheckInt32(x_PageImagePosition.Text)) Then
					messageList.Add("Invalid Value (Int32): Position")
				End If
			End If
		End If
		Return IIf(messageList.Count = 0, Nothing, messageList)
	End Function

	' **********************************************
	' *  Convert Input Control Values to Row Object
	' **********************************************

	Private Sub ControlToRow(ByVal row As PageImagerow, ByVal control As WebControl)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		row.SetPageType(Core.PageType.Add) ' set page type = list for update

		' Field PageID
		Dim x_PageID As DropDownList = TryCast(control.FindControl("x_PageID"), DropDownList)
		Dim v_PageID As String = String.Empty
		If (x_PageID.SelectedIndex >= 0) Then
			For Each li As ListItem In x_PageID.Items
				If (li.Selected) Then
					v_PageID += li.Value
					Exit For
				End If
			Next
		End If
		If (v_PageID <> String.Empty) Then row.PageID = Convert.ToInt32(v_PageID) Else row.PageID = CType(Nothing, Nullable(Of Int32))

		' Field ImageID
		Dim x_ImageID As DropDownList = TryCast(control.FindControl("x_ImageID"), DropDownList)
		Dim v_ImageID As String = String.Empty
		If (x_ImageID.SelectedIndex >= 0) Then
			For Each li As ListItem In x_ImageID.Items
				If (li.Selected) Then
					v_ImageID += li.Value
					Exit For
				End If
			Next
		End If
		If (v_ImageID <> String.Empty) Then row.ImageID = Convert.ToInt32(v_ImageID) Else row.ImageID = CType(Nothing, Nullable(Of Int32))

		' Field PageImagePosition
		Dim x_PageImagePosition As TextBox = TryCast(control.FindControl("x_PageImagePosition"), TextBox)
		If (x_PageImagePosition.Text <> String.Empty) Then row.PageImagePosition = Convert.ToInt32(x_PageImagePosition.Text) Else row.PageImagePosition = CType(Nothing, Nullable(Of Int32))
	End Sub

	' ****************************
	' *  Check for duplicate Key
	' ****************************

	Private Function CheckDuplicateKey(ByVal control As WebControl) As ArrayList 
		Dim sWhere As String = String.Empty
		Dim messageList As ArrayList = New ArrayList()
		Dim data As PageImagedal = New PageImagedal()
		Dim newkey As PageImagekey = New PageImagekey()
		Try
		data = Nothing
		Catch e As Exception
			messageList.Add(e.Message)
		End Try
		Return IIf(messageList.Count = 0, Nothing, messageList)
	End Function

	' ********************************************
	' *  Convert Row Object Value To Input Control
	' ********************************************

	Private Sub RowToControl(ByVal row As PageImagerow, ByVal control As WebControl, ByVal ct As Core.CtrlType)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		If (ct = Core.CtrlType.Edit) Then ' set up edit control

			' Field PageID
			Dim x_PageID As DropDownList = TryCast(control.FindControl("x_PageID"), DropDownList)
			x_PageID.DataValueField = "ewValueField"
			x_PageID.DataTextField = "ewTextField"
			Dim dv_x_PageID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
			If (dv_x_PageID Is Nothing) Then dv_x_PageID = PageImagedal.LookUpTable("PageID")
			x_PageID.DataSource = dv_x_PageID
			x_PageID.DataBind()
			x_PageID.Items.Insert(0, New ListItem("Please Select", ""))
			Dim v_PageID As String
			If (row.PageID.HasValue) Then v_PageID = Convert.ToString(row.PageID) Else v_PageID = String.Empty
			x_PageID.ClearSelection()
			For Each li As ListItem In x_PageID.Items
				If (li.Value.ToString() = v_PageID) Then
					li.Selected = True
					Exit For
				End If
			Next

			' Field ImageID
			Dim x_ImageID As DropDownList = TryCast(control.FindControl("x_ImageID"), DropDownList)
			x_ImageID.DataValueField = "ewValueField"
			x_ImageID.DataTextField = "ewTextField"
			Dim dv_x_ImageID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
			If (dv_x_ImageID Is Nothing) Then dv_x_ImageID = PageImagedal.LookUpTable("ImageID")
			x_ImageID.DataSource = dv_x_ImageID
			x_ImageID.DataBind()
			x_ImageID.Items.Insert(0, New ListItem("Please Select", ""))
			Dim v_ImageID As String
			If (row.ImageID.HasValue) Then v_ImageID = Convert.ToString(row.ImageID) Else v_ImageID = String.Empty
			x_ImageID.ClearSelection()
			For Each li As ListItem In x_ImageID.Items
				If (li.Value.ToString() = v_ImageID) Then
					li.Selected = True
					Exit For
				End If
			Next

			' Field PageImagePosition
			Dim x_PageImagePosition As TextBox = TryCast(control.FindControl("x_PageImagePosition"), TextBox)
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
	' *  Handler for DataSource Inserting
	' ************************************

	Protected Sub PageImageDataSource_Inserting(ByVal sender As Object, ByVal e As ObjectDataSourceMethodEventArgs)
		Dim control As WebControl = PageImageDetailsView

		' Set up row object
		Dim row As PageImagerow = TryCast(e.InputParameters(0), PageImagerow)
		ControlToRow(row, control)
		If (PageImagebll.Inserting(row)) Then
		Else
			e.Cancel = True
			If (Not String.IsNullOrEmpty(objProfile.Message)) Then
				lblMessage.Text = objProfile.Message
				pnlMessage.Visible = True
			End If
		End If
	End Sub

	' ***********************************
	' *  Handler for DataSource Inserted
	' ***********************************

	Protected Sub PageImageDataSource_Inserted(ByVal sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.Exception IsNot Nothing) Then 
			e.AffectedRows = 0
			e.ExceptionHandled = True
			lblMessage.Text = e.Exception.InnerException.Message
			pnlMessage.Visible = True
		Else
			newrow = TryCast(e.ReturnValue, PageImagerow) ' get new row objectinsert method
			PageImagebll.Inserted(newrow)
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
<asp:ScriptManager ID="ScriptManager1" runat="server" />
<script type="text/javascript">
<!--
var ew_DateSep = "/"; // set date separator
var ew_FieldSep = "<%= Share.ValueSeparator(0) %>"; // set value separator
var ew_CurrencyDecimalSeparator = "<%=System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator %>";
//-->
</script>
<script type="text/javascript">
<!--
    var ew_DHTMLEditors = [];
//-->
</script>
	<p><span class="aspnetmaker">Add to TABLE: Page Image</span></p>
	<p>
		<asp:Hyperlink id="lnkList" Text="Back to List" NavigateUrl="pageimage_list.aspx" CssClass="aspnetmaker" runat="server" />
		<asp:Label id="lblReturnUrl" Visible="False" CssClass="aspnetmaker" runat="server">pageimage_list.aspx</asp:Label>
	</p>
	<asp:ValidationSummary id="xevs_PageImage" CssClass="aspnetmaker" runat="server"
	HeaderText=""
	ShowSummary="False"
	ShowMessageBox="True"
	ForeColor="#FF0000" />
	<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
		<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
	</asp:PlaceHolder>
	<!-- Data Source -->
<asp:ObjectDataSource ID="PageImageDataSource"
	TypeName="PMGEN.PageImagedal"
	DataObjectTypeName="PMGEN.PageImagerow"
	InsertMethod="Insert"
	OnInserting="PageImageDataSource_Inserting"
	OnInserted="PageImageDataSource_Inserted"
	runat="server">
</asp:ObjectDataSource>
	<asp:DetailsView ID="PageImageDetailsView"
		DataKeyNames="PageImageID"
		DataSourceID="PageImageDataSource"
		DefaultMode="Insert"
		GridLines="None"
		AutoGenerateRows="False" CssClass="ewDetailTable"
		OnInit="PageImageDetailsView_Init"
		OnDataBound="PageImageDetailsView_DataBound"
		OnItemInserting="PageImageDetailsView_ItemInserting"
		OnItemInserted="PageImageDetailsView_ItemInserted"
		OnUnload="PageImageDetailsView_Unload"
		AllowPaging="True"
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
					<asp:Label runat="server" id="xs_PageID">Page<span class='ewmsg'>&nbsp;*</span></asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:DropDownList ID="x_PageID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
<asp:RequiredFieldValidator ID="vx_PageID" CssClass="aspnetmaker" runat="server" ControlToValidate="x_PageID" ErrorMessage="Please enter required field - Page" ForeColor="Red" Display="None" Text="*" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_ImageID">Image<span class='ewmsg'>&nbsp;*</span></asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:DropDownList ID="x_ImageID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
<asp:RequiredFieldValidator ID="vx_ImageID" CssClass="aspnetmaker" runat="server" ControlToValidate="x_ImageID" ErrorMessage="Please enter required field - Image" ForeColor="Red" Display="None" Text="*" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_PageImagePosition">Position</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_PageImagePosition" Columns="30" CssClass="aspnetmaker" runat="server" />
<ew:IntegerValidator ID="vc_PageImagePosition" ClientValidationFunction="ew_CheckInteger" CssClass="aspnetmaker" runat="server" ControlToValidate="x_PageImagePosition" ErrorMessage="Incorrect integer - Position" Display="None" ForeColor="Red" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField ShowHeader="False">
				<ItemStyle CssClass="ewTableFooter" />
				<InsertItemTemplate>
					<table border="0">
						<tr>
							<td><asp:LinkButton ID="btnInsert" runat="server" CausesValidation="True" CommandName="Insert" Text="ADD"></asp:LinkButton></td>
							<td><asp:LinkButton ID="btnCancel" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel" OnClick="btnCancel_Click"></asp:LinkButton></td>
						</tr>
					</table>
					<asp:HiddenField ID="k_PageImageID" Runat="server" Value='<%# Bind("PageImageID") %>' />
				</InsertItemTemplate>
			</asp:TemplateField>
		</Fields>
	</asp:DetailsView>
</asp:Content>
