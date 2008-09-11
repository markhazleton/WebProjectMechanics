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
			objProfile = CustomProfile.GetTable(Share.ProjectName, SiteCategoryinf.TableVar)
		End Sub

		' *************************
		' *  Handler for Page Load
		' *************************

		Protected Sub Page_Load(ByVal s As Object, ByVal e As System.EventArgs)
			Response.Cache.SetCacheability(HttpCacheability.NoCache)
			Page.ClientScript.RegisterClientScriptInclude("ewv", "ewv.js")
			If (Not Page.IsPostBack) Then
				LoadData() ' Initialize Controls
				LoadSearchControls(SiteCategoryTable)
			End If
			Dim jsString As String = String.Empty
			jsString &= "if (!ew) { ew = new Object(); ew.Controls = new Array(); }" & vbCrLf
			jsString &= "ew.Controls['SiteCategory_srch_x_SiteCategoryTypeID'] = '" & x_SiteCategoryTypeID.ClientID & "'" & vbCrLf
			jsString &= "ew.Controls['SiteCategory_srch_x_CategoryName'] = '" & x_CategoryName.ClientID & "'" & vbCrLf
			jsString &= "ew.Controls['SiteCategory_srch_x_CategoryTitle'] = '" & x_CategoryTitle.ClientID & "'" & vbCrLf
			jsString &= "ew.Controls['SiteCategory_srch_x_SiteCategoryGroupID'] = '" & x_SiteCategoryGroupID.ClientID & "'" & vbCrLf
			jsString &= "ew.Controls['SiteCategory_srch_x_ParentCategoryID'] = '" & x_ParentCategoryID.ClientID & "'" & vbCrLf
			Page.ClientScript.RegisterClientScriptBlock(System.Type.GetType("System.String"), "CtrlID", jsString, True)
			Page.Form.DefaultFocus = x_SiteCategoryTypeID.ClientID
			Try
				SiteCategorydal.OpenConnection()
			Catch
			End Try
		End Sub

		' **************************
		' *  Handler for Page Unload
		' **************************

		Protected Sub Page_Unload(ByVal s As Object, ByVal e As System.EventArgs)
			Try
				SiteCategorydal.CloseAndDisposeConnection()
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
			Dim messageList As ArrayList = ValidateInputValues(SiteCategoryTable, Core.PageType.Search)
			If (messageList IsNot Nothing) Then
				Dim sMsg As String = String.Empty 
				For Each sErrMsg As String In messageList
					sMsg += sErrMsg + "<br>" 
				Next
				lblMessage.Text = sMsg 
				pnlMessage.Visible = True 
				Return 
			End If
			Dim sUrl As String = LoadSearchUrl(SiteCategoryTable) 
			If (Not String.IsNullOrEmpty(sUrl)) Then
				objProfile.PageIndex = 0 ' reset page index
				sUrl &= "&SiteCategory_psearchtype=&SiteCategory_psearch="
				Response.Redirect (lblReturnUrl.Text + "?" + sUrl) 
			End If
		End Sub

	'*********************************
	'*  Load Default Data to Controls
	'*********************************

	Private Sub LoadData()
		Dim source As WebControl = TryCast(SiteCategoryTable, WebControl)
		Dim row As SiteCategoryrow = New SiteCategoryrow() ' dummy record for rendering
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
			Dim x_SiteCategoryTypeID As DropDownList= TryCast(control.FindControl("x_SiteCategoryTypeID"), DropDownList)
			If (x_SiteCategoryTypeID IsNot Nothing) Then
				If ((x_SiteCategoryTypeID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_SiteCategoryTypeID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32):  Site Type")
				End If
			End If
			Dim x_CategoryName As TextBox= TryCast(control.FindControl("x_CategoryName"), TextBox)
			If (x_CategoryName IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_CategoryName.Text)) Then
					messageList.Add("Invalid Value (String): Category Name")
				End If
			End If
			Dim x_CategoryTitle As TextBox= TryCast(control.FindControl("x_CategoryTitle"), TextBox)
			If (x_CategoryTitle IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_CategoryTitle.Text)) Then
					messageList.Add("Invalid Value (String): Category Title")
				End If
			End If
			Dim x_SiteCategoryGroupID As DropDownList= TryCast(control.FindControl("x_SiteCategoryGroupID"), DropDownList)
			If (x_SiteCategoryGroupID IsNot Nothing) Then
				If ((x_SiteCategoryGroupID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_SiteCategoryGroupID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Category Group")
				End If
			End If
			Dim x_ParentCategoryID As DropDownList= TryCast(control.FindControl("x_ParentCategoryID"), DropDownList)
			If (x_ParentCategoryID IsNot Nothing) Then
				If ((x_ParentCategoryID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_ParentCategoryID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Parent Category ID")
				End If
			End If
		End If
		Return IIf(messageList.Count = 0, Nothing, messageList)
	End Function

	' ********************************************
	' *  Convert Row Object Value To Input Control
	' ********************************************

	Private Sub RowToControl(ByVal row As SiteCategoryrow, ByVal control As WebControl, ByVal ct As Core.CtrlType)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		If (ct = Core.CtrlType.Edit) Then ' set up edit control

			' Field SiteCategoryTypeID
			If (control.FindControl("ax_SiteCategoryTypeID") IsNot Nothing) Then
				Dim cdd As CascadingDropDown = TryCast(control.FindControl("ax_SiteCategoryTypeID"), CascadingDropDown)
				Dim v_SiteCategoryTypeID As String
				If (row.SiteCategoryTypeID.HasValue) Then v_SiteCategoryTypeID = Convert.ToString(row.SiteCategoryTypeID) Else v_SiteCategoryTypeID = String.Empty
				cdd.SelectedValue = v_SiteCategoryTypeID
			End If

			' Field CategoryName
			' Field CategoryTitle
			' Field SiteCategoryGroupID

			Dim x_SiteCategoryGroupID As DropDownList = TryCast(control.FindControl("x_SiteCategoryGroupID"), DropDownList)
			x_SiteCategoryGroupID.DataValueField = "ewValueField"
			x_SiteCategoryGroupID.DataTextField = "ewTextField"
			Dim dv_x_SiteCategoryGroupID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
			If (dv_x_SiteCategoryGroupID Is Nothing) Then dv_x_SiteCategoryGroupID = SiteCategorydal.LookUpTable("SiteCategoryGroupID")
			x_SiteCategoryGroupID.DataSource = dv_x_SiteCategoryGroupID
			x_SiteCategoryGroupID.DataBind()
			x_SiteCategoryGroupID.Items.Insert(0, New ListItem("Please Select", ""))
			Dim v_SiteCategoryGroupID As String
			If (row.SiteCategoryGroupID.HasValue) Then v_SiteCategoryGroupID = Convert.ToString(row.SiteCategoryGroupID) Else v_SiteCategoryGroupID = String.Empty

			' Field ParentCategoryID
			If (control.FindControl("ax_ParentCategoryID") IsNot Nothing) Then
				Dim cdd As CascadingDropDown = TryCast(control.FindControl("ax_ParentCategoryID"), CascadingDropDown)
				Dim v_ParentCategoryID As String
				If (row.ParentCategoryID.HasValue) Then v_ParentCategoryID = Convert.ToString(row.ParentCategoryID) Else v_ParentCategoryID = String.Empty
				cdd.SelectedValue = v_ParentCategoryID
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
		Dim x_SiteCategoryTypeID As DropDownList = TryCast(control.FindControl("x_SiteCategoryTypeID"), DropDownList)
		Dim z_SiteCategoryTypeID As HiddenField = TryCast(control.FindControl("z_SiteCategoryTypeID"), HiddenField)
		sSrchStr = x_SiteCategoryTypeID.SelectedValue
		sSrchOpr = z_SiteCategoryTypeID.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_SiteCategoryTypeID", sSrchStr, "z_SiteCategoryTypeID", sSrchOpr)
		End If
		Dim x_CategoryName As TextBox = TryCast(control.FindControl("x_CategoryName"), TextBox)
		Dim z_CategoryName As HiddenField = TryCast(control.FindControl("z_CategoryName"), HiddenField)
		sSrchStr = x_CategoryName.Text
		sSrchOpr = z_CategoryName.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_CategoryName", sSrchStr, "z_CategoryName", sSrchOpr)
		End If
		Dim x_CategoryTitle As TextBox = TryCast(control.FindControl("x_CategoryTitle"), TextBox)
		Dim z_CategoryTitle As HiddenField = TryCast(control.FindControl("z_CategoryTitle"), HiddenField)
		sSrchStr = x_CategoryTitle.Text
		sSrchOpr = z_CategoryTitle.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_CategoryTitle", sSrchStr, "z_CategoryTitle", sSrchOpr)
		End If
		Dim x_SiteCategoryGroupID As DropDownList = TryCast(control.FindControl("x_SiteCategoryGroupID"), DropDownList)
		Dim z_SiteCategoryGroupID As HiddenField = TryCast(control.FindControl("z_SiteCategoryGroupID"), HiddenField)
		sSrchStr = x_SiteCategoryGroupID.SelectedValue
		sSrchOpr = z_SiteCategoryGroupID.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_SiteCategoryGroupID", sSrchStr, "z_SiteCategoryGroupID", sSrchOpr)
		End If
		Dim x_ParentCategoryID As DropDownList = TryCast(control.FindControl("x_ParentCategoryID"), DropDownList)
		Dim z_ParentCategoryID As HiddenField = TryCast(control.FindControl("z_ParentCategoryID"), HiddenField)
		sSrchStr = x_ParentCategoryID.SelectedValue
		sSrchOpr = z_ParentCategoryID.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_ParentCategoryID", sSrchStr, "z_ParentCategoryID", sSrchOpr)
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
		Dim data As SiteCategorydal = New SiteCategorydal()

		' SiteCategoryTypeID
		If (control.FindControl("ax_SiteCategoryTypeID") IsNot Nothing) Then
			Dim cdd As CascadingDropDown = TryCast(control.FindControl("ax_SiteCategoryTypeID"), CascadingDropDown)
			Dim v_SiteCategoryTypeID As String = Convert.ToString(data.AdvancedSearchParm1.SiteCategoryTypeID)
			cdd.SelectedValue = v_SiteCategoryTypeID
		End If

		' CategoryName
		If (data.AdvancedSearchParm1.CategoryName IsNot Nothing) Then
			Dim x_CategoryName As TextBox = TryCast(control.FindControl("x_CategoryName"), TextBox)
			x_CategoryName.Text = Convert.ToString(data.AdvancedSearchParm1.CategoryName)
		End If

		' CategoryTitle
		If (data.AdvancedSearchParm1.CategoryTitle IsNot Nothing) Then
			Dim x_CategoryTitle As TextBox = TryCast(control.FindControl("x_CategoryTitle"), TextBox)
			x_CategoryTitle.Text = Convert.ToString(data.AdvancedSearchParm1.CategoryTitle)
		End If

		' SiteCategoryGroupID
		Dim x_SiteCategoryGroupID As DropDownList = TryCast(control.FindControl("x_SiteCategoryGroupID"), DropDownList)
		x_SiteCategoryGroupID.DataValueField = "ewValueField"
		x_SiteCategoryGroupID.DataTextField = "ewTextField"
		Dim dv_x_SiteCategoryGroupID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
		if (dv_x_SiteCategoryGroupID Is Nothing) Then dv_x_SiteCategoryGroupID = SiteCategorydal.LookUpTable("SiteCategoryGroupID")
		x_SiteCategoryGroupID.DataSource = dv_x_SiteCategoryGroupID
		x_SiteCategoryGroupID.DataBind()
		x_SiteCategoryGroupID.Items.Insert(0, new ListItem("Please Select", ""))
		Dim v_SiteCategoryGroupID As String = Convert.ToString(data.AdvancedSearchParm1.SiteCategoryGroupID)
		x_SiteCategoryGroupID.ClearSelection()
		For Each li As ListItem In x_SiteCategoryGroupID.Items
			If (li.Value.ToString() = v_SiteCategoryGroupID) Then
				li.Selected = True
				Exit For
			End If
		Next

		' ParentCategoryID
		If (control.FindControl("ax_ParentCategoryID") IsNot Nothing) Then
			Dim cdd As CascadingDropDown = TryCast(control.FindControl("ax_ParentCategoryID"), CascadingDropDown)
			Dim v_ParentCategoryID As String = Convert.ToString(data.AdvancedSearchParm1.ParentCategoryID)
			cdd.SelectedValue = v_ParentCategoryID
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
	<p><span class="aspnetmaker">Search TABLE: Site Category</span></p>
	<p>
		<asp:Hyperlink id="lnkList" Text="Back to List" NavigateUrl="sitecategory_list.aspx" CssClass="aspnetmaker" runat="server" />
		<asp:Label id="lblReturnUrl" Visible="False" CssClass="aspnetmaker" runat="server">sitecategory_list.aspx</asp:Label>
	</p>
	<script type='text/javascript'>
	function ew_ClearForm() {
		ew_ResetDropDownList(ew.Controls['SiteCategory_srch_x_SiteCategoryTypeID']);
		ew_ResetElement(ew.Controls['SiteCategory_srch_x_CategoryName']);
		ew_ResetElement(ew.Controls['SiteCategory_srch_x_CategoryTitle']);
		ew_ResetDropDownList(ew.Controls['SiteCategory_srch_x_SiteCategoryGroupID']);
		ew_ResetDropDownList(ew.Controls['SiteCategory_srch_x_ParentCategoryID']);
	}
	</script>
	<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
	<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
	</asp:PlaceHolder>
	<asp:ValidationSummary id="xevs_SiteCategory" CssClass="aspnetmaker" runat="server"
		 HeaderText=""
		 ShowSummary="False"
		 ShowMessageBox="True"
		 ForeColor="#FF0000" />
	<asp:Table runat="server" CssClass="ewDetailTable" ID="SiteCategoryTable">
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_SiteCategoryTypeID"> Site Type</asp:Label></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">=<asp:HiddenField id="z_SiteCategoryTypeID" value="=,," runat="server" /></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">
<asp:DropDownList ID="x_SiteCategoryTypeID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
<ew:CascadingDropDown ID="ax_SiteCategoryTypeID" runat="server" EncryptKey="<%$ EWCode: Share.RandomKey %>"
	TargetControlID="x_SiteCategoryTypeID" Category="x_SiteCategoryTypeID" 
	PromptText="Please Select" ServiceMethod="GetDropDownList" ServicePath="ewWebService.asmx" 
	SqlCommand="SELECT [SiteCategoryTypeID], [SiteCategoryTypeNM] FROM [SiteCategoryType] ORDER BY [SiteCategoryTypeNM] Asc"
	DisplayFieldNames="SiteCategoryTypeNM" EnabledDropDown="true" />
</span>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_CategoryName">Category Name</asp:Label></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">LIKE<asp:HiddenField id="z_CategoryName" value="LIKE,'%,%'" runat="server" /></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">
<asp:TextBox id="x_CategoryName" Columns="60" MaxLength="255" CssClass="aspnetmaker" runat="server" />
</span>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_CategoryTitle">Category Title</asp:Label></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">LIKE<asp:HiddenField id="z_CategoryTitle" value="LIKE,'%,%'" runat="server" /></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">
<asp:TextBox id="x_CategoryTitle" Columns="60" MaxLength="255" CssClass="aspnetmaker" runat="server" />
</span>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_SiteCategoryGroupID">Category Group</asp:Label></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">=<asp:HiddenField id="z_SiteCategoryGroupID" value="=,," runat="server" /></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">
<asp:DropDownList ID="x_SiteCategoryGroupID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
</span>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_ParentCategoryID">Parent Category ID</asp:Label></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">=<asp:HiddenField id="z_ParentCategoryID" value="=,," runat="server" /></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">
<asp:DropDownList ID="x_ParentCategoryID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
<ew:CascadingDropDown ID="ax_ParentCategoryID" runat="server" EncryptKey="<%$ EWCode: Share.RandomKey %>"
	TargetControlID="x_ParentCategoryID" Category="x_ParentCategoryID" 
	PromptText="Please Select" ServiceMethod="GetDropDownList" ServicePath="ewWebService.asmx" 
	SqlCommand="SELECT [SiteCategoryID], [CategoryName] FROM [SiteCategory] WHERE ([SiteCategoryTypeID]=@FILTER_VALUE) ORDER BY [CategoryName] Asc"
	ParentCategory="x_SiteCategoryTypeID" ParentControlID="x_SiteCategoryTypeID"
	DisplayFieldNames="CategoryName" EnabledDropDown="true" />
</span>
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
	<br />
	<asp:Button id="btnSearch" Text="Search" OnClick="btnSearch_Click" CssClass="aspnetmaker" runat="server" />
	<input type="button" id="btnReset" onclick="ew_ClearForm();" class="aspnetmaker" value="Reset"/>
</asp:Content>
