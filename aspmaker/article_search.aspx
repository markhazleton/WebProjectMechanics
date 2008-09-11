<%@ Page EnableViewState="true" Language="VB" ValidateRequest="False" EnableEventValidation="False" Debug="True" MasterPageFile="masterpage.master" %>
<%@ MasterType VirtualPath="masterpage.master" %>
<%@ Import Namespace="PMGEN" %>
<%@ Import Namespace="EW.Data" %>
<%@ Import Namespace="EW.Data.Utilities" %>
<%@ Import Namespace="EW.Web" %>
<%@ Register NameSpace="PMGEN" TagPrefix="PMGEN" %>
<script runat="server">
	Dim searchParms As String 
		Private objProfile As TableProfile ' table profile

		' *************************
		' *  Handler for Page Init
		' *************************

		Protected Sub Page_Init(ByVal s As Object, ByVal e As System.EventArgs)
			objProfile = CustomProfile.GetTable(Share.ProjectName, Articleinf.TableVar)
		End Sub

		' *************************
		' *  Handler for Page Load
		' *************************

		Protected Sub Page_Load(ByVal s As Object, ByVal e As System.EventArgs)
			Response.Cache.SetCacheability(HttpCacheability.NoCache)
			Page.ClientScript.RegisterClientScriptInclude("ewv", "ewv.js")
			If (Not Page.IsPostBack) Then
				LoadData() ' Initialize Controls
				LoadSearchControls(ArticleTable)
			End If
			Dim jsString As String = String.Empty
			jsString &= "if (!ew) { ew = new Object(); ew.Controls = new Array(); }" & vbCrLf
			jsString &= "ew.Controls['Article_srch_x_Title'] = '" & x_Title.ClientID & "'" & vbCrLf
			jsString &= "ew.Controls['Article_srch_x_Description'] = '" & x_Description.ClientID & "'" & vbCrLf
			jsString &= "ew.Controls['Article_srch_x_ArticleSummary'] = '" & x_ArticleSummary.ClientID & "'" & vbCrLf
			jsString &= "ew.Controls['Article_srch_x_ArticleBody'] = '" & x_ArticleBody.ClientID & "'" & vbCrLf
			jsString &= "ew.Controls['Article_srch_x_CompanyID'] = '" & x_CompanyID.ClientID & "'" & vbCrLf
			jsString &= "ew.Controls['Article_srch_x_PageID'] = '" & x_PageID.ClientID & "'" & vbCrLf
			jsString &= "ew.Controls['Article_srch_x_Active'] = '" & x_Active.UniqueID & "'" & vbCrLf
			jsString &= "ew.Controls['Article_srch_x_ContactID'] = '" & x_ContactID.ClientID & "'" & vbCrLf
			Page.ClientScript.RegisterClientScriptBlock(System.Type.GetType("System.String"), "CtrlID", jsString, True)
			Page.Form.DefaultFocus = x_Title.ClientID
					Application("FCKeditor:UserFilesPath") = Replace(Request.ApplicationPath & Session("SiteGallery"), "//", "/") ' Set upload path
			Try
				Articledal.OpenConnection()
			Catch
			End Try
		End Sub

		' **************************
		' *  Handler for Page Unload
		' **************************

		Protected Sub Page_Unload(ByVal s As Object, ByVal e As System.EventArgs)
			Try
				Articledal.CloseAndDisposeConnection()
			Catch
			End Try
		End Sub

		' ******************
		' *  Search Handler
		' ******************

		Protected Sub btnSearch_Click(ByVal s As Object, ByVal e As System.EventArgs)
			Page.Validate()
			If (Not Page.IsValid) Then
				Dim sMsg As String = String.Empty
				For Each oValidator As IValidator In Validators
					If (Not oValidator.IsValid) Then
						sMsg += oValidator.ErrorMessage + "<br />" 
					End If
				Next
				lblMessage.Text = sMsg
				pnlMessage.Visible = True
				Return
			End If
			Dim messageList As ArrayList = ValidateInputValues(ArticleTable, Core.PageType.Search)
			If (messageList IsNot Nothing) Then
				Dim sMsg As String = String.Empty 
				For Each sErrMsg As String In messageList
					sMsg += sErrMsg + "<br>" 
				Next
				lblMessage.Text = sMsg 
				pnlMessage.Visible = True 
				Return 
			End If
			Dim sUrl As String = LoadSearchUrl(ArticleTable) 
			If (Not String.IsNullOrEmpty(sUrl)) Then
				objProfile.PageIndex = 0 ' reset page index
				sUrl &= "&Article_psearchtype=&Article_psearch="
				Response.Redirect (lblReturnUrl.Text + "?" + sUrl) 
			End If
		End Sub

	'*********************************
	'*  Load Default Data to Controls
	'*********************************

	Private Sub LoadData()
		Dim source As WebControl = TryCast(ArticleTable, WebControl)
		Dim row As Articlerow = New Articlerow() ' dummy record for rendering
		RowToControl(row, source, Core.CtrlType.Edit)
		row = Nothing
	End Sub

	' ********************************
	' * Register ClientID of Controls
	' ********************************

	Private Sub RegisterClientID(ByVal sBlockName As String, ByVal control As Control)
		Dim jsString As String = String.Empty
		Page.ClientScript.RegisterClientScriptBlock(System.Type.GetType("System.String"), sBlockName, jsString, True)
	End Sub

	' *****************************************************
	' *  Processing Input Values Before Updating/Inserting
	' *****************************************************

	Private Function ValidateInputValues(ByVal control As Control, ByVal pageType As Core.PageType) As ArrayList
		Dim messageList As ArrayList = New ArrayList()
		DataFormat.SetDateSeparator("/")
		If (pageType = Core.PageType.Search) Then ' Search Validation
			Dim x_Title As TextBox= TryCast(control.FindControl("x_Title"), TextBox)
			If (x_Title IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_Title.Text)) Then
					messageList.Add("Invalid Value (String): Title")
				End If
			End If
			Dim x_Description As TextBox= TryCast(control.FindControl("x_Description"), TextBox)
			If (x_Description IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_Description.Text)) Then
					messageList.Add("Invalid Value (String): Description")
				End If
			End If
			Dim x_ArticleSummary As TextBox= TryCast(control.FindControl("x_ArticleSummary"), TextBox)
			If (x_ArticleSummary IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_ArticleSummary.Text)) Then
					messageList.Add("Invalid Value (String): Article Summary")
				End If
			End If
			Dim x_ArticleBody As TextBox= TryCast(control.FindControl("x_ArticleBody"), TextBox)
			If (x_ArticleBody IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_ArticleBody.Text)) Then
					messageList.Add("Invalid Value (String): Article Body")
				End If
			End If
			Dim x_CompanyID As DropDownList= TryCast(control.FindControl("x_CompanyID"), DropDownList)
			If (x_CompanyID IsNot Nothing) Then
				If ((x_CompanyID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_CompanyID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Company")
				End If
			End If
			Dim x_PageID As DropDownList= TryCast(control.FindControl("x_PageID"), DropDownList)
			If (x_PageID IsNot Nothing) Then
				If ((x_PageID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_PageID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Page")
				End If
			End If
			Dim x_ContactID As DropDownList= TryCast(control.FindControl("x_ContactID"), DropDownList)
			If (x_ContactID IsNot Nothing) Then
				If ((x_ContactID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_ContactID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Contact")
				End If
			End If
		End If
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
			' Field Description
			' Field ArticleSummary
			' Field ArticleBody
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

	' ****************************
	' *  Set up Search Parameters
	' ****************************

	Private Function LoadSearchUrl(ByVal control As Control) As String 
		Dim sTmp As String = String.Empty
		Dim sSrchOpr As String = String.Empty
		Dim sSrchStr As String = String.Empty
		Dim sUrl As String = String.Empty

		' Construct Search Parameters
		Dim x_Title As TextBox = TryCast(control.FindControl("x_Title"), TextBox)
		Dim z_Title As HiddenField = TryCast(control.FindControl("z_Title"), HiddenField)
		sSrchStr = x_Title.Text
		sSrchOpr = z_Title.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_Title", sSrchStr, "z_Title", sSrchOpr)
		End If
		Dim x_Description As TextBox = TryCast(control.FindControl("x_Description"), TextBox)
		Dim z_Description As HiddenField = TryCast(control.FindControl("z_Description"), HiddenField)
		sSrchStr = x_Description.Text
		sSrchOpr = z_Description.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_Description", sSrchStr, "z_Description", sSrchOpr)
		End If
		Dim x_ArticleSummary As TextBox = TryCast(control.FindControl("x_ArticleSummary"), TextBox)
		Dim z_ArticleSummary As HiddenField = TryCast(control.FindControl("z_ArticleSummary"), HiddenField)
		sSrchStr = x_ArticleSummary.Text
		sSrchOpr = z_ArticleSummary.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_ArticleSummary", sSrchStr, "z_ArticleSummary", sSrchOpr)
		End If
		Dim x_ArticleBody As TextBox = TryCast(control.FindControl("x_ArticleBody"), TextBox)
		Dim z_ArticleBody As HiddenField = TryCast(control.FindControl("z_ArticleBody"), HiddenField)
		sSrchStr = x_ArticleBody.Text
		sSrchOpr = z_ArticleBody.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_ArticleBody", sSrchStr, "z_ArticleBody", sSrchOpr)
		End If
		Dim x_CompanyID As DropDownList = TryCast(control.FindControl("x_CompanyID"), DropDownList)
		Dim z_CompanyID As HiddenField = TryCast(control.FindControl("z_CompanyID"), HiddenField)
		sSrchStr = x_CompanyID.SelectedValue
		sSrchOpr = z_CompanyID.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_CompanyID", sSrchStr, "z_CompanyID", sSrchOpr)
		End If
		Dim x_PageID As DropDownList = TryCast(control.FindControl("x_PageID"), DropDownList)
		Dim z_PageID As HiddenField = TryCast(control.FindControl("z_PageID"), HiddenField)
		sSrchStr = x_PageID.SelectedValue
		sSrchOpr = z_PageID.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_PageID", sSrchStr, "z_PageID", sSrchOpr)
		End If
		Dim x_Active As CheckBox = TryCast(control.FindControl("x_Active"), CheckBox)
		Dim z_Active As HiddenField = TryCast(control.FindControl("z_Active"), HiddenField)
		sSrchStr = IIf(x_Active.Checked, "True", String.Empty)
		sSrchOpr = z_Active.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_Active", sSrchStr, "z_Active", sSrchOpr)
		End If
		Dim x_ContactID As DropDownList = TryCast(control.FindControl("x_ContactID"), DropDownList)
		Dim z_ContactID As HiddenField = TryCast(control.FindControl("z_ContactID"), HiddenField)
		sSrchStr = x_ContactID.SelectedValue
		sSrchOpr = z_ContactID.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_ContactID", sSrchStr, "z_ContactID", sSrchOpr)
		End If

		' Remove First "&" character
		If (sUrl.Length > 1) Then sUrl = sUrl.SubString(1)
		Return sUrl
	End Function

	' *****************
	' *  Load Controls 
	' *****************

	Private Sub LoadSearchControls(ByVal control As Control) 
		Dim sSearchType As String = String.Empty
		Dim data As Articledal = New Articledal()

		' Title
		If (data.AdvancedSearchParm1.Title IsNot Nothing) Then
			Dim x_Title As TextBox = TryCast(control.FindControl("x_Title"), TextBox)
			x_Title.Text = Convert.ToString(data.AdvancedSearchParm1.Title)
		End If

		' Description
		If (data.AdvancedSearchParm1.Description IsNot Nothing) Then
			Dim x_Description As TextBox = TryCast(control.FindControl("x_Description"), TextBox)
			x_Description.Text = Convert.ToString(data.AdvancedSearchParm1.Description)
		End If

		' ArticleSummary
		If (data.AdvancedSearchParm1.ArticleSummary IsNot Nothing) Then
			Dim x_ArticleSummary As TextBox = TryCast(control.FindControl("x_ArticleSummary"), TextBox)
			x_ArticleSummary.Text = Convert.ToString(data.AdvancedSearchParm1.ArticleSummary)
		End If

		' ArticleBody
		If (data.AdvancedSearchParm1.ArticleBody IsNot Nothing) Then
			Dim x_ArticleBody As TextBox = TryCast(control.FindControl("x_ArticleBody"), TextBox)
			x_ArticleBody.Text = Convert.ToString(data.AdvancedSearchParm1.ArticleBody)
		End If

		' CompanyID
		If (control.FindControl("ax_CompanyID") IsNot Nothing) Then
			Dim cdd As CascadingDropDown = TryCast(control.FindControl("ax_CompanyID"), CascadingDropDown)
			Dim v_CompanyID As String = Convert.ToString(data.AdvancedSearchParm1.CompanyID)
			cdd.SelectedValue = v_CompanyID
		End If

		' PageID
		If (control.FindControl("ax_PageID") IsNot Nothing) Then
			Dim cdd As CascadingDropDown = TryCast(control.FindControl("ax_PageID"), CascadingDropDown)
			Dim v_PageID As String = Convert.ToString(data.AdvancedSearchParm1.PageID)
			cdd.SelectedValue = v_PageID
		End If

		' Active
		Dim x_Active As CheckBox= TryCast(control.FindControl("x_Active"), CheckBox)
		Dim v_Active As String = Convert.ToString(data.AdvancedSearchParm1.Active)
		x_Active.Checked = (Not String.IsNullOrEmpty(v_Active) AndAlso Convert.ToBoolean(v_Active))

		' ContactID
		If (control.FindControl("ax_ContactID") IsNot Nothing) Then
			Dim cdd As CascadingDropDown = TryCast(control.FindControl("ax_ContactID"), CascadingDropDown)
			Dim v_ContactID As String = Convert.ToString(data.AdvancedSearchParm1.ContactID)
			cdd.SelectedValue = v_ContactID
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
	<p><span class="aspnetmaker">Search TABLE: Article</span></p>
	<p>
		<asp:Hyperlink id="lnkList" Text="Back to List" NavigateUrl="article_list.aspx" CssClass="aspnetmaker" runat="server" />
		<asp:Label id="lblReturnUrl" Visible="False" CssClass="aspnetmaker" runat="server">article_list.aspx</asp:Label>
	</p>
	<script type='text/javascript'>
	function ew_ClearForm() {
		ew_ResetElement(ew.Controls['Article_srch_x_Title']);
		ew_ResetElement(ew.Controls['Article_srch_x_Description']);
		ew_ResetElement(ew.Controls['Article_srch_x_ArticleSummary']);
		ew_ResetTextArea(ew.Controls['Article_srch_x_ArticleBody']);
		ew_ResetDropDownList(ew.Controls['Article_srch_x_CompanyID']);
		ew_ResetDropDownList(ew.Controls['Article_srch_x_PageID']);
		ew_ResetCheckBox(ew.Controls['Article_srch_x_Active']);
		ew_ResetDropDownList(ew.Controls['Article_srch_x_ContactID']);
	}
	</script>
	<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
	<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
	</asp:PlaceHolder>
	<asp:ValidationSummary id="xevs_Article" CssClass="aspnetmaker" runat="server"
		 HeaderText=""
		 ShowSummary="False"
		 ShowMessageBox="True"
		 ForeColor="#FF0000" />
	<asp:Table runat="server" CssClass="ewDetailTable" ID="ArticleTable">
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_Title">Title</asp:Label></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">LIKE<asp:HiddenField id="z_Title" value="LIKE,'%,%'" runat="server" /></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">
<asp:TextBox id="x_Title" Columns="80" MaxLength="255" CssClass="aspnetmaker" runat="server" />
</span>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_Description">Description</asp:Label></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">LIKE<asp:HiddenField id="z_Description" value="LIKE,'%,%'" runat="server" /></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">
<asp:TextBox id="x_Description" Columns="30" MaxLength="255" CssClass="aspnetmaker" runat="server" />
</span>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_ArticleSummary">Article Summary</asp:Label></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">LIKE<asp:HiddenField id="z_ArticleSummary" value="LIKE,'%,%'" runat="server" /></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">
<asp:TextBox id="x_ArticleSummary" TextMode="MultiLine" Rows="3" Columns="60" CssClass="aspnetmaker" runat="server" />
</span>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_ArticleBody">Article Body</asp:Label></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">LIKE<asp:HiddenField id="z_ArticleBody" value="LIKE,'%,%'" runat="server" /></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">
<asp:TextBox id="x_ArticleBody" TextMode="MultiLine" Rows="15" Columns="45" CssClass="aspnetmaker" runat="server" />
<script type="text/javascript">
<!--
var editor = new ew_DHTMLEditor(ew.Controls['Article_srch_x_ArticleBody']);
editor.create = function() {
	var sBasePath = 'fckeditor/';
	var oFCKeditor = new FCKeditor(ew.Controls['Article_srch_x_ArticleBody'], 45*_width_multiplier, 15*_height_multiplier);
	oFCKeditor.BasePath = sBasePath;
	oFCKeditor.ReplaceTextarea();
	this.active = true;
}
ew_DHTMLEditors[ew_DHTMLEditors.length] = editor;
-->
</script>
</span>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_CompanyID">Company</asp:Label></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">=<asp:HiddenField id="z_CompanyID" value="=,," runat="server" /></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">
<asp:DropDownList ID="x_CompanyID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
<ew:CascadingDropDown ID="ax_CompanyID" runat="server" EncryptKey="<%$ EWCode: Share.RandomKey %>"
	TargetControlID="x_CompanyID" Category="x_CompanyID" 
	PromptText="Please Select" ServiceMethod="GetDropDownList" ServicePath="ewWebService.asmx" 
	SqlCommand="SELECT [CompanyID], [CompanyName] FROM [Company] ORDER BY [CompanyName] Asc"
	DisplayFieldNames="CompanyName" EnabledDropDown="true" />
</span>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_PageID">Page</asp:Label></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">=<asp:HiddenField id="z_PageID" value="=,," runat="server" /></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">
<asp:DropDownList ID="x_PageID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
<ew:CascadingDropDown ID="ax_PageID" runat="server" EncryptKey="<%$ EWCode: Share.RandomKey %>"
	TargetControlID="x_PageID" Category="x_PageID" 
	PromptText="Please Select" ServiceMethod="GetDropDownList" ServicePath="ewWebService.asmx" 
	SqlCommand="SELECT [PageID], [PageName] FROM [Page] WHERE ([CompanyID]=@FILTER_VALUE) ORDER BY [PageName] Asc"
	ParentCategory="x_CompanyID" ParentControlID="x_CompanyID"
	DisplayFieldNames="PageName" EnabledDropDown="true" />
</span>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_Active">Active</asp:Label></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">=<asp:HiddenField id="z_Active" value="=,," runat="server" /></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">
<asp:CheckBox ID="x_Active" CssClass="aspnetmakerlist" runat="server" />
</span>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_ContactID">Contact</asp:Label></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">=<asp:HiddenField id="z_ContactID" value="=,," runat="server" /></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">
<asp:DropDownList ID="x_ContactID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
<ew:CascadingDropDown ID="ax_ContactID" runat="server" EncryptKey="<%$ EWCode: Share.RandomKey %>"
	TargetControlID="x_ContactID" Category="x_ContactID" 
	PromptText="Please Select" ServiceMethod="GetDropDownList" ServicePath="ewWebService.asmx" 
	SqlCommand="SELECT [ContactID], [PrimaryContact] FROM [Contact] WHERE ([CompanyID]=@FILTER_VALUE) ORDER BY [PrimaryContact] Asc"
	ParentCategory="x_CompanyID" ParentControlID="x_CompanyID"
	DisplayFieldNames="PrimaryContact" EnabledDropDown="true" />
</span>
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
	<br />
	<script type="text/javascript">
	<!--
	ew_CreateEditor();  // Create DHTML editor(s)
	//-->
	</script>
	<asp:Button id="btnSearch" Text="Search" OnClick="btnSearch_Click" CssClass="aspnetmaker" runat="server" />
	<input type="button" id="btnReset" onclick="ew_ClearForm();" class="aspnetmaker" value="Reset"/>
</asp:Content>
