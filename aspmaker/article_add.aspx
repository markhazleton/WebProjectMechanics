<%@ Page EnableViewState="true" Language="VB" ValidateRequest="False" EnableEventValidation="False" Debug="True" MasterPageFile="masterpage.master" %>
<%@ MasterType VirtualPath="masterpage.master" %>
<%@ Import Namespace="PMGEN" %>
<%@ Import Namespace="EW.Data" %>
<%@ Import Namespace="EW.Security" %>
<%@ Import Namespace="EW.Data.Utilities" %>
<%@ Import Namespace="EW.Web" %>
<%@ Register NameSpace="PMGEN" TagPrefix="PMGEN" %>
<script runat="server">
	Dim sUserFilter As String = string.Empty
	Dim key As Articlekey ' record key
	Dim oldrow As Articlerow ' old record data input by user
	Dim newrow As Articlerow ' new record data input by user
	Dim objProfile As TableProfile ' table profile

	' *************************
	' *  Handler for Page Init
	' *************************

	Protected Sub Page_Init(ByVal s As Object, ByVal e As System.EventArgs)
		If Not mhUser.IsAdmin() Then
		  Session("Login_Link") = Request.RawUrl
		  Response.Redirect("/mhweb/login/login.aspx")
		End If
		objProfile = CustomProfile.GetTable(Share.ProjectName, Articleinf.TableVar)
	End Sub

	' *************************
	' *  Handler for Page Load
	' *************************

	Protected Sub Page_Load(ByVal s As Object, ByVal e As System.EventArgs)
		Page.ClientScript.RegisterClientScriptInclude("ewv", "ewv.js")
		Response.Cache.SetCacheability(HttpCacheability.NoCache)
		If (Not Page.IsPostBack) Then
			If (Request.QueryString.Count > 0) Then
				key = New Articlekey()
				Dim messageList As ArrayList = Articleinf.LoadKey(key)
				If (messageList IsNot Nothing) Then key = Nothing ' error, ignore
			End If
		End If
				Application("FCKeditor:UserFilesPath") = Replace(Request.ApplicationPath & Session("SiteGallery"), "//", "/") ' Set upload path
		If (ArticleDetailsView.Visible) Then
			RegisterClientID("CtrlID", ArticleDetailsView)
		End If
			If (ArticleDetailsView.FindControl("x_Title") IsNot Nothing) Then
				Page.Form.DefaultFocus = ArticleDetailsView.FindControl("x_Title").ClientID
			End If
	End Sub

	' **********************************************
	' *  Load Copy Data & Default Data to Controls
	' **********************************************

	Private Sub LoadData()
		Dim source As WebControl = TryCast(ArticleDetailsView, WebControl)
		If (oldrow IsNot Nothing) Then ' old record (from recovery) exists, just restore
			RowToControl(oldrow, source, Core.CtrlType.Edit)
			oldrow = Nothing
		ElseIf (key IsNot Nothing) Then ' key loaded, try to copy record
			Dim data As Articledal = New Articledal()
			Dim row As Articlerow = data.LoadRow(key, Articleinf.GetUserFilter())
			If (row Is Nothing) Then
				objProfile.Message = "Record not found"
				Response.Redirect(lblReturnUrl.Text)
			End If
			RowToControl(row, source, Core.CtrlType.Edit)
			row = Nothing
		Else 
			Dim row As Articlerow = New Articlerow()
			row.CompanyID = Convert.ToInt32(val(session("CompanyID"))) ' set default value
			row.PageID = Convert.ToInt32(val(session("CurrentPageID"))) ' set default value
			row.Active = Convert.ToBoolean(1) ' set default value
			row.StartDT = Convert.ToDateTime(DateTime.Now) ' set default value
			row.ContactID = Convert.ToInt32(val(session("ContactID"))) ' set default value
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

	Protected Sub ArticleDetailsView_DataBound(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim source As DetailsView = ArticleDetailsView
		If (ArticleDetailsView.CurrentMode = DetailsViewMode.Insert) Then
			LoadData()
		End If
	End Sub

	' ***************************************
	' *  Handler for DetailsView ItemCreated
	' ***************************************

	Protected Sub ArticleDetailsView_ItemCreated(ByVal sender As Object, ByVal e As System.EventArgs)
	End Sub

	' *****************************************
	' *  Handler for DetailsView ItemInserting
	' *****************************************

	Protected Sub ArticleDetailsView_ItemInserting(ByVal sender As Object, ByVal e As DetailsViewInsertEventArgs)
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

	Protected Sub ArticleDetailsView_ItemInserted(ByVal sender As Object, ByVal e As DetailsViewInsertedEventArgs)
		If (e.AffectedRows = 0) Then

			' Keep in insert mode
			e.KeepInInsertMode = True

			' Get original values
			Dim source As WebControl = TryCast(sender, WebControl)
			If (source Is Nothing) Then Return
			oldrow = New Articlerow()
			ControlToRow(oldrow, source)
		Else
			objProfile.Message = "Add new record successful"
			Response.Redirect(lblReturnUrl.Text)
		End If
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

	' ********************************
	' * Register ClientID of Controls
	' ********************************

	Private Sub RegisterClientID(ByVal sBlockName As String, ByVal control As Control)
		Dim jsString As String = String.Empty
		If (control.FindControl("x_Title") IsNot Nothing) Then
			jsString &= "ew.Controls['x_Title'] = '" & control.FindControl("x_Title").ClientID & "';"
		End If
		If (control.FindControl("x_Description") IsNot Nothing) Then
			jsString &= "ew.Controls['x_Description'] = '" & control.FindControl("x_Description").ClientID & "';"
		End If
		If (control.FindControl("x_ArticleSummary") IsNot Nothing) Then
			jsString &= "ew.Controls['x_ArticleSummary'] = '" & control.FindControl("x_ArticleSummary").ClientID & "';"
		End If
		If (control.FindControl("x_ArticleBody") IsNot Nothing) Then
			jsString &= "ew.Controls['x_ArticleBody'] = '" & control.FindControl("x_ArticleBody").ClientID & "';"
		End If
		If (control.FindControl("x_CompanyID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_CompanyID'] = '" & control.FindControl("x_CompanyID").ClientID & "';"
		End If
		If (control.FindControl("x_PageID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_PageID'] = '" & control.FindControl("x_PageID").ClientID & "';"
		End If
		If (control.FindControl("x_Active") IsNot Nothing) Then
			jsString &= "ew.Controls['x_Active'] = '" & control.FindControl("x_Active").ClientID & "';"
		End If
		If (control.FindControl("x_StartDT") IsNot Nothing) Then
			jsString &= "ew.Controls['x_StartDT'] = '" & control.FindControl("x_StartDT").ClientID & "';"
		End If
		If (control.FindControl("x_ContactID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_ContactID'] = '" & control.FindControl("x_ContactID").ClientID & "';"
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
			Dim x_Title As TextBox = TryCast(control.FindControl("x_Title"), TextBox)
			If (x_Title IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_Title.Text)) Then
					messageList.Add("Invalid Value (String): Title")
				ElseIf (String.IsNullOrEmpty(x_Title.Text)) Then
					messageList.Add("Please enter required field (String): Title")
				End If
			End If
			Dim x_Description As TextBox = TryCast(control.FindControl("x_Description"), TextBox)
			If (x_Description IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_Description.Text)) Then
					messageList.Add("Invalid Value (String): Description")
				End If
			End If
			Dim x_ArticleSummary As TextBox = TryCast(control.FindControl("x_ArticleSummary"), TextBox)
			If (x_ArticleSummary IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_ArticleSummary.Text)) Then
					messageList.Add("Invalid Value (String): Article Summary")
				End If
			End If
			Dim x_ArticleBody As TextBox = TryCast(control.FindControl("x_ArticleBody"), TextBox)
			If (x_ArticleBody IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_ArticleBody.Text)) Then
					messageList.Add("Invalid Value (String): Article Body")
				End If
			End If
			Dim x_CompanyID As DropDownList = TryCast(control.FindControl("x_CompanyID"), DropDownList)
			If (x_CompanyID IsNot Nothing) Then
				If ((x_CompanyID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_CompanyID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Company")
				ElseIf (String.IsNullOrEmpty(x_CompanyID.SelectedValue)) Then
					messageList.Add("Please enter required field (Int32): Company")
				End If
			End If
			Dim x_PageID As DropDownList = TryCast(control.FindControl("x_PageID"), DropDownList)
			If (x_PageID IsNot Nothing) Then
				If ((x_PageID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_PageID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Page")
				End If
			End If
			Dim x_StartDT As TextBox = TryCast(control.FindControl("x_StartDT"), TextBox)
			If (x_StartDT IsNot Nothing) Then
				If (Not DataFormat.CheckDateTime(x_StartDT.Text)) Then
					messageList.Add("Invalid Value (DateTime): Start DT")
				ElseIf (String.IsNullOrEmpty(x_StartDT.Text)) Then
					messageList.Add("Please enter required field (DateTime): Start DT")
				End If
			End If
			Dim x_ContactID As DropDownList = TryCast(control.FindControl("x_ContactID"), DropDownList)
			If (x_ContactID IsNot Nothing) Then
				If ((x_ContactID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_ContactID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Contact")
				End If
			End If
		End If
		Return IIf(messageList.Count = 0, Nothing, messageList)
	End Function

	' **********************************************
	' *  Convert Input Control Values to Row Object
	' **********************************************

	Private Sub ControlToRow(ByVal row As Articlerow, ByVal control As WebControl)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		row.SetPageType(Core.PageType.Add) ' set page type = list for update

		' Field Title
		Dim x_Title As TextBox = TryCast(control.FindControl("x_Title"), TextBox)
		If (x_Title.Text <> String.Empty) Then row.Title = x_Title.Text Else row.Title = String.Empty

		' Field Description
		Dim x_Description As TextBox = TryCast(control.FindControl("x_Description"), TextBox)
		If (x_Description.Text <> String.Empty) Then row.Description = x_Description.Text Else row.Description = Nothing

		' Field ArticleSummary
		Dim x_ArticleSummary As TextBox = TryCast(control.FindControl("x_ArticleSummary"), TextBox)
		If (x_ArticleSummary.Text <> String.Empty) Then row.ArticleSummary = x_ArticleSummary.Text Else row.ArticleSummary = Nothing

		' Field ArticleBody
		Dim x_ArticleBody As TextBox = TryCast(control.FindControl("x_ArticleBody"), TextBox)
		If (x_ArticleBody.Text <> String.Empty) Then row.ArticleBody = x_ArticleBody.Text Else row.ArticleBody = Nothing

		' Field CompanyID
		Dim x_CompanyID As DropDownList = TryCast(control.FindControl("x_CompanyID"), DropDownList)
		Dim v_CompanyID As String = String.Empty
		If (x_CompanyID.SelectedIndex >= 0) Then
			For Each li As ListItem In x_CompanyID.Items
				If (li.Selected) Then
					v_CompanyID += li.Value
					Exit For
				End If
			Next
		End If
		If (v_CompanyID <> String.Empty) Then row.CompanyID = Convert.ToInt32(v_CompanyID) Else row.CompanyID = CType(Nothing, Nullable(Of Int32))

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

		' Field Active
		Dim x_Active As CheckBox = TryCast(control.FindControl("x_Active"), CheckBox)
		If (x_Active.Checked) Then
			row.Active = True
		Else
			row.Active = False
		End If

		' Field StartDT
		Dim x_StartDT As TextBox = TryCast(control.FindControl("x_StartDT"), TextBox)
		If (x_StartDT.Text <> String.Empty) Then row.StartDT = Convert.ToDateTime(DataFormat.UnFormatDateTime(x_StartDT.Text, 6, "/"c)) Else row.StartDT = CType(Nothing, Nullable(Of DateTime))

		' Field ContactID
		Dim x_ContactID As DropDownList = TryCast(control.FindControl("x_ContactID"), DropDownList)
		Dim v_ContactID As String = String.Empty
		If (x_ContactID.SelectedIndex >= 0) Then
			For Each li As ListItem In x_ContactID.Items
				If (li.Selected) Then
					v_ContactID += li.Value
					Exit For
				End If
			Next
		End If
		If (v_ContactID <> String.Empty) Then row.ContactID = Convert.ToInt32(v_ContactID) Else row.ContactID = CType(Nothing, Nullable(Of Int32))
	End Sub

	' ****************************
	' *  Check for duplicate Key
	' ****************************

	Private Function CheckDuplicateKey(ByVal control As WebControl) As ArrayList 
		Dim sWhere As String = String.Empty
		Dim messageList As ArrayList = New ArrayList()
		Dim data As Articledal = New Articledal()
		Dim newkey As Articlekey = New Articlekey()
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

	Private Sub RowToControl(ByVal row As Articlerow, ByVal control As WebControl, ByVal ct As Core.CtrlType)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		If (ct = Core.CtrlType.Edit) Then ' set up edit control

			' Field Title
			Dim x_Title As TextBox = TryCast(control.FindControl("x_Title"), TextBox)
			If (row.Title IsNot Nothing) Then x_Title.Text = row.Title.ToString() Else x_Title.Text = String.Empty

			' Field Description
			Dim x_Description As TextBox = TryCast(control.FindControl("x_Description"), TextBox)
			If (row.Description IsNot Nothing) Then x_Description.Text = row.Description.ToString() Else x_Description.Text = String.Empty

			' Field ArticleSummary
			Dim x_ArticleSummary As TextBox = TryCast(control.FindControl("x_ArticleSummary"), TextBox)
			If (row.ArticleSummary IsNot Nothing) Then x_ArticleSummary.Text = row.ArticleSummary.ToString() Else x_ArticleSummary.Text = String.Empty

			' Field ArticleBody
			Dim x_ArticleBody As TextBox = TryCast(control.FindControl("x_ArticleBody"), TextBox)
			If (row.ArticleBody IsNot Nothing) Then x_ArticleBody.Text = row.ArticleBody.ToString() Else x_ArticleBody.Text = String.Empty

			' Field CompanyID
			If (control.FindControl("ax_CompanyID") IsNot Nothing) Then
				Dim cdd As CascadingDropDown = TryCast(control.FindControl("ax_CompanyID"), CascadingDropDown)
				Dim v_CompanyID As String
				If (row.CompanyID.HasValue) Then v_CompanyID = Convert.ToString(row.CompanyID) Else v_CompanyID = String.Empty
				cdd.SelectedValue = v_CompanyID
			End If

			' Field PageID
			If (control.FindControl("ax_PageID") IsNot Nothing) Then
				Dim cdd As CascadingDropDown = TryCast(control.FindControl("ax_PageID"), CascadingDropDown)
				Dim v_PageID As String
				If (row.PageID.HasValue) Then v_PageID = Convert.ToString(row.PageID) Else v_PageID = String.Empty
				cdd.SelectedValue = v_PageID
			End If

			' Field Active
			Dim x_Active As CheckBox = TryCast(control.FindControl("x_Active"), CheckBox)
			Dim sActive As String = row.Active.ToString()
			If ((sActive IsNot Nothing) AndAlso sActive <> "") Then
				If (Convert.ToBoolean(sActive)) Then
					x_Active.Checked = True
				Else
					x_Active.Checked = False
				End If
			End If

			' Field StartDT
			Dim x_StartDT As TextBox = TryCast(control.FindControl("x_StartDT"), TextBox)
			If (row.StartDT.HasValue) Then x_StartDT.Text = DataFormat.DateTimeFormat(6, "/", row.StartDT) Else x_StartDT.Text = String.Empty

			' Field ContactID
			If (control.FindControl("ax_ContactID") IsNot Nothing) Then
				Dim cdd As CascadingDropDown = TryCast(control.FindControl("ax_ContactID"), CascadingDropDown)
				Dim v_ContactID As String
				If (row.ContactID.HasValue) Then v_ContactID = Convert.ToString(row.ContactID) Else v_ContactID = String.Empty
				cdd.SelectedValue = v_ContactID
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
	' *  Handler for DataSource Inserting
	' ************************************

	Protected Sub ArticleDataSource_Inserting(ByVal sender As Object, ByVal e As ObjectDataSourceMethodEventArgs)
		Dim control As WebControl = ArticleDetailsView

		' Set up row object
		Dim row As Articlerow = TryCast(e.InputParameters(0), Articlerow)
		ControlToRow(row, control)
		If (Articlebll.Inserting(row)) Then
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

	Protected Sub ArticleDataSource_Inserted(ByVal sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.Exception IsNot Nothing) Then 
			e.AffectedRows = 0
			e.ExceptionHandled = True
			lblMessage.Text = e.Exception.InnerException.Message
			pnlMessage.Visible = True
		Else
			newrow = TryCast(e.ReturnValue, Articlerow) ' get new row objectinsert method
			Articlebll.Inserted(newrow)
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
<asp:ScriptManager ID="ScriptManager1" runat="server" />
<script type="text/javascript">
<!--
var ew_DateSep = "/"; // set date separator
var ew_FieldSep = "<%= Share.ValueSeparator(0) %>"; // set value separator
var ew_CurrencyDecimalSeparator = "<%=System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator %>";
//-->
</script>
<script type="text/javascript" src="fckeditor/fckeditor.js"></script>
<script type="text/javascript">
<!--
    _width_multiplier = 16;
    _height_multiplier = 60;
    var ew_DHTMLEditors = [];
    function ew_UpdateTextArea() {
    	if (typeof ew_DHTMLEditors != 'undefined' &&
    		typeof FCKeditorAPI != 'undefined') {			
    			var inst;			
    			for (inst in FCKeditorAPI.__Instances) {
    			    with (FCKeditorAPI.__Instances[inst]) {
        				UpdateLinkedField();
        				LinkedField.value = ew_RemoveSpaces(LinkedField.value); // add and modify ew_RemoveSpaces() 
    			    }
    			}
    	}
    }
    function ew_ResetTextArea(elem) {
    	if (typeof ew_DHTMLEditors != 'undefined' &&
    		typeof FCKeditorAPI != 'undefined') {
    		    var oEditor = FCKeditorAPI.GetInstance(elem) ;// Get the editor instance that we want to interact with.
    		    oEditor.EditorDocument.body.innerHTML = '';
    	}
    }
//-->
</script>
	<p><span class="aspnetmaker">Add to TABLE: Article</span></p>
	<p>
		<asp:Hyperlink id="lnkList" Text="Back to List" NavigateUrl="article_list.aspx" CssClass="aspnetmaker" runat="server" />
		<asp:Label id="lblReturnUrl" Visible="False" CssClass="aspnetmaker" runat="server">article_list.aspx</asp:Label>
	</p>
	<asp:ValidationSummary id="xevs_Article" CssClass="aspnetmaker" runat="server"
	HeaderText=""
	ShowSummary="False"
	ShowMessageBox="True"
	ForeColor="#FF0000" />
	<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
		<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
	</asp:PlaceHolder>
	<!-- Data Source -->
<asp:ObjectDataSource ID="ArticleDataSource"
	TypeName="PMGEN.Articledal"
	DataObjectTypeName="PMGEN.Articlerow"
	InsertMethod="Insert"
	OnInserting="ArticleDataSource_Inserting"
	OnInserted="ArticleDataSource_Inserted"
	runat="server">
</asp:ObjectDataSource>
	<asp:DetailsView ID="ArticleDetailsView"
		DataKeyNames="ArticleID"
		DataSourceID="ArticleDataSource"
		DefaultMode="Insert"
		GridLines="None"
		AutoGenerateRows="False" CssClass="ewDetailTable"
		OnInit="ArticleDetailsView_Init"
		OnDataBound="ArticleDetailsView_DataBound"
		OnItemCreated="ArticleDetailsView_ItemCreated"
		OnItemInserting="ArticleDetailsView_ItemInserting"
		OnItemInserted="ArticleDetailsView_ItemInserted"
		OnUnload="ArticleDetailsView_Unload"
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
					<asp:Label runat="server" id="xs_Title">Title<span class='ewmsg'>&nbsp;*</span></asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_Title" Columns="80" MaxLength="255" CssClass="aspnetmaker" runat="server" />
<asp:RequiredFieldValidator ID="vx_Title" CssClass="aspnetmaker" runat="server" ControlToValidate="x_Title" ErrorMessage="Article title is required and must be unique" ForeColor="Red" Display="None" Text="*" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_Description">Description</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_Description" Columns="30" MaxLength="255" CssClass="aspnetmaker" runat="server" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_ArticleSummary">Article Summary</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_ArticleSummary" TextMode="MultiLine" Rows="3" Columns="60" CssClass="aspnetmaker" runat="server" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_ArticleBody">Article Body</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_ArticleBody" TextMode="MultiLine" Rows="15" Columns="45" CssClass="aspnetmaker" runat="server" />
<script type="text/javascript">
<!--
var editor = new ew_DHTMLEditor(ew.Controls['x_ArticleBody']);
editor.create = function() {
	var sBasePath = 'fckeditor/';
	var oFCKeditor = new FCKeditor(ew.Controls['x_ArticleBody'], 45*_width_multiplier, 15*_height_multiplier);
	oFCKeditor.BasePath = sBasePath;
	oFCKeditor.ReplaceTextarea();
	this.active = true;
}
ew_DHTMLEditors[ew_DHTMLEditors.length] = editor;
-->
</script>
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_CompanyID">Company<span class='ewmsg'>&nbsp;*</span></asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:DropDownList ID="x_CompanyID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
<ew:CascadingDropDown ID="ax_CompanyID" runat="server" EncryptKey="<%$ EWCode: Share.RandomKey %>"
	TargetControlID="x_CompanyID" Category="x_CompanyID" 
	PromptText="Please Select" ServiceMethod="GetDropDownList" ServicePath="ewWebService.asmx" 
	SqlCommand="SELECT [CompanyID], [CompanyName] FROM [Company] ORDER BY [CompanyName] Asc"
	DisplayFieldNames="CompanyName" EnabledDropDown="true" />
<asp:RequiredFieldValidator ID="vx_CompanyID" CssClass="aspnetmaker" runat="server" ControlToValidate="x_CompanyID" ErrorMessage="Please enter required field - Company" ForeColor="Red" Display="None" Text="*" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_PageID">Page</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:DropDownList ID="x_PageID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
<ew:CascadingDropDown ID="ax_PageID" runat="server" EncryptKey="<%$ EWCode: Share.RandomKey %>"
	TargetControlID="x_PageID" Category="x_PageID" 
	PromptText="Please Select" ServiceMethod="GetDropDownList" ServicePath="ewWebService.asmx" 
	SqlCommand="SELECT [PageID], [PageName] FROM [Page] WHERE ([CompanyID]=@FILTER_VALUE) ORDER BY [PageName] Asc"
	ParentCategory="x_CompanyID" ParentControlID="x_CompanyID"
	DisplayFieldNames="PageName" EnabledDropDown="true" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_Active">Active</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:CheckBox ID="x_Active" CssClass="aspnetmakerlist" runat="server" />
<asp:HiddenField ID="hx_Active" Runat="server" Value='<%# Bind("Active") %>' />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_StartDT">Start DT<span class='ewmsg'>&nbsp;*</span></asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_StartDT" CssClass="aspnetmaker" runat="server" />
<asp:RequiredFieldValidator ID="vx_StartDT" CssClass="aspnetmaker" runat="server" ControlToValidate="x_StartDT" ErrorMessage="Start Date is required for all Articles" ForeColor="Red" Display="None" Text="*" />
<ew:USDateValidator ID="vc_StartDT" ClientValidationFunction="ew_CheckUSDate" CssClass="aspnetmaker" runat="server" ControlToValidate="x_StartDT" ErrorMessage="Start Date is required for all Articles" Display="None" ForeColor="Red" DateSeparator="/" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_ContactID">Contact</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:DropDownList ID="x_ContactID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
<ew:CascadingDropDown ID="ax_ContactID" runat="server" EncryptKey="<%$ EWCode: Share.RandomKey %>"
	TargetControlID="x_ContactID" Category="x_ContactID" 
	PromptText="Please Select" ServiceMethod="GetDropDownList" ServicePath="ewWebService.asmx" 
	SqlCommand="SELECT [ContactID], [PrimaryContact] FROM [Contact] WHERE ([CompanyID]=@FILTER_VALUE) ORDER BY [PrimaryContact] Asc"
	ParentCategory="x_CompanyID" ParentControlID="x_CompanyID"
	DisplayFieldNames="PrimaryContact" EnabledDropDown="true" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField ShowHeader="False">
				<ItemStyle CssClass="ewTableFooter" />
				<InsertItemTemplate>
					<table border="0">
						<tr>
							<td><asp:LinkButton ID="btnInsert" runat="server" CausesValidation="True" CommandName="Insert" Text="ADD" onClientClick="ew_UpdateTextArea()"></asp:LinkButton></td>
							<td><asp:LinkButton ID="btnCancel" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel" OnClick="btnCancel_Click"></asp:LinkButton></td>
						</tr>
					</table>
					<asp:HiddenField ID="k_ArticleID" Runat="server" Value='<%# Bind("ArticleID") %>' />
				</InsertItemTemplate>
			</asp:TemplateField>
		</Fields>
	</asp:DetailsView>
	<script type="text/javascript">
	<!--
		ew_CreateEditor();  // Create DHTML editor(s)
	//-->
	</script>
</asp:Content>
