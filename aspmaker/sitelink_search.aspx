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
			objProfile = CustomProfile.GetTable(Share.ProjectName, SiteLinkinf.TableVar)
		End Sub

		' *************************
		' *  Handler for Page Load
		' *************************

		Protected Sub Page_Load(ByVal s As Object, ByVal e As System.EventArgs)
			Response.Cache.SetCacheability(HttpCacheability.NoCache)
			Page.ClientScript.RegisterClientScriptInclude("ewv", "ewv.js")
			If (Not Page.IsPostBack) Then
				LoadData() ' Initialize Controls
				LoadSearchControls(SiteLinkTable)
			End If
			Dim jsString As String = String.Empty
			jsString &= "if (!ew) { ew = new Object(); ew.Controls = new Array(); }" & vbCrLf
			jsString &= "ew.Controls['SiteLink_srch_x_SiteCategoryTypeID'] = '" & x_SiteCategoryTypeID.ClientID & "'" & vbCrLf
			jsString &= "ew.Controls['SiteLink_srch_x_CategoryID'] = '" & x_CategoryID.ClientID & "'" & vbCrLf
			jsString &= "ew.Controls['SiteLink_srch_x_LinkTypeCD'] = '" & x_LinkTypeCD.ClientID & "'" & vbCrLf
			jsString &= "ew.Controls['SiteLink_srch_x_Title'] = '" & x_Title.ClientID & "'" & vbCrLf
			jsString &= "ew.Controls['SiteLink_srch_x_SiteCategoryID'] = '" & x_SiteCategoryID.ClientID & "'" & vbCrLf
			jsString &= "ew.Controls['SiteLink_srch_x_SiteCategoryGroupID'] = '" & x_SiteCategoryGroupID.ClientID & "'" & vbCrLf
			jsString &= "ew.Controls['SiteLink_srch_x_CompanyID'] = '" & x_CompanyID.ClientID & "'" & vbCrLf
			Page.ClientScript.RegisterClientScriptBlock(System.Type.GetType("System.String"), "CtrlID", jsString, True)
			Page.Form.DefaultFocus = x_SiteCategoryTypeID.ClientID
			Try
				SiteLinkdal.OpenConnection()
			Catch
			End Try
		End Sub

		' **************************
		' *  Handler for Page Unload
		' **************************

		Protected Sub Page_Unload(ByVal s As Object, ByVal e As System.EventArgs)
			Try
				SiteLinkdal.CloseAndDisposeConnection()
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
			Dim messageList As ArrayList = ValidateInputValues(SiteLinkTable, Core.PageType.Search)
			If (messageList IsNot Nothing) Then
				Dim sMsg As String = String.Empty 
				For Each sErrMsg As String In messageList
					sMsg += sErrMsg + "<br>" 
				Next
				lblMessage.Text = sMsg 
				pnlMessage.Visible = True 
				Return 
			End If
			Dim sUrl As String = LoadSearchUrl(SiteLinkTable) 
			If (Not String.IsNullOrEmpty(sUrl)) Then
				objProfile.PageIndex = 0 ' reset page index
				sUrl &= "&SiteLink_psearchtype=&SiteLink_psearch="
				Response.Redirect (lblReturnUrl.Text + "?" + sUrl) 
			End If
		End Sub

	'*********************************
	'*  Load Default Data to Controls
	'*********************************

	Private Sub LoadData()
		Dim source As WebControl = TryCast(SiteLinkTable, WebControl)
		Dim row As SiteLinkrow = New SiteLinkrow() ' dummy record for rendering
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
					messageList.Add("Invalid Value (Int32): Site Type")
				End If
			End If
			Dim x_CategoryID As DropDownList= TryCast(control.FindControl("x_CategoryID"), DropDownList)
			If (x_CategoryID IsNot Nothing) Then
				If ((x_CategoryID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_CategoryID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Link Category")
				End If
			End If
			Dim x_LinkTypeCD As DropDownList= TryCast(control.FindControl("x_LinkTypeCD"), DropDownList)
			If (x_LinkTypeCD IsNot Nothing) Then
				If ((x_LinkTypeCD.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckString(x_LinkTypeCD.SelectedValue))) Then
					messageList.Add("Invalid Value (String): Link Type")
				End If
			End If
			Dim x_Title As TextBox= TryCast(control.FindControl("x_Title"), TextBox)
			If (x_Title IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_Title.Text)) Then
					messageList.Add("Invalid Value (String): Title")
				End If
			End If
			Dim x_SiteCategoryID As DropDownList= TryCast(control.FindControl("x_SiteCategoryID"), DropDownList)
			If (x_SiteCategoryID IsNot Nothing) Then
				If ((x_SiteCategoryID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_SiteCategoryID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Site Category")
				End If
			End If
			Dim x_SiteCategoryGroupID As DropDownList= TryCast(control.FindControl("x_SiteCategoryGroupID"), DropDownList)
			If (x_SiteCategoryGroupID IsNot Nothing) Then
				If ((x_SiteCategoryGroupID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_SiteCategoryGroupID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Site Group")
				End If
			End If
			Dim x_CompanyID As DropDownList= TryCast(control.FindControl("x_CompanyID"), DropDownList)
			If (x_CompanyID IsNot Nothing) Then
				If ((x_CompanyID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_CompanyID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Company")
				End If
			End If
		End If
		Return IIf(messageList.Count = 0, Nothing, messageList)
	End Function

	' ********************************************
	' *  Convert Row Object Value To Input Control
	' ********************************************

	Private Sub RowToControl(ByVal row As SiteLinkrow, ByVal control As WebControl, ByVal ct As Core.CtrlType)
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

			' Field CategoryID
			Dim x_CategoryID As DropDownList = TryCast(control.FindControl("x_CategoryID"), DropDownList)
			x_CategoryID.DataValueField = "ewValueField"
			x_CategoryID.DataTextField = "ewTextField"
			Dim dv_x_CategoryID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
			If (dv_x_CategoryID Is Nothing) Then dv_x_CategoryID = SiteLinkdal.LookUpTable("CategoryID")
			x_CategoryID.DataSource = dv_x_CategoryID
			x_CategoryID.DataBind()
			x_CategoryID.Items.Insert(0, New ListItem("Please Select", ""))
			Dim v_CategoryID As String
			If (row.CategoryID.HasValue) Then v_CategoryID = Convert.ToString(row.CategoryID) Else v_CategoryID = String.Empty

			' Field LinkTypeCD
			Dim x_LinkTypeCD As DropDownList = TryCast(control.FindControl("x_LinkTypeCD"), DropDownList)
			x_LinkTypeCD.DataValueField = "ewValueField"
			x_LinkTypeCD.DataTextField = "ewTextField"
			Dim dv_x_LinkTypeCD As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
			If (dv_x_LinkTypeCD Is Nothing) Then dv_x_LinkTypeCD = SiteLinkdal.LookUpTable("LinkTypeCD")
			x_LinkTypeCD.DataSource = dv_x_LinkTypeCD
			x_LinkTypeCD.DataBind()
			x_LinkTypeCD.Items.Insert(0, New ListItem("Please Select", ""))
			Dim v_LinkTypeCD As String 
			If (row.LinkTypeCD IsNot Nothing) Then  v_LinkTypeCD = Convert.ToString(row.LinkTypeCD) Else v_LinkTypeCD = String.Empty

			' Field Title
			' Field SiteCategoryID

			If (control.FindControl("ax_SiteCategoryID") IsNot Nothing) Then
				Dim cdd As CascadingDropDown = TryCast(control.FindControl("ax_SiteCategoryID"), CascadingDropDown)
				Dim v_SiteCategoryID As String
				If (row.SiteCategoryID.HasValue) Then v_SiteCategoryID = Convert.ToString(row.SiteCategoryID) Else v_SiteCategoryID = String.Empty
				cdd.SelectedValue = v_SiteCategoryID
			End If

			' Field SiteCategoryGroupID
			Dim x_SiteCategoryGroupID As DropDownList = TryCast(control.FindControl("x_SiteCategoryGroupID"), DropDownList)
			x_SiteCategoryGroupID.DataValueField = "ewValueField"
			x_SiteCategoryGroupID.DataTextField = "ewTextField"
			Dim dv_x_SiteCategoryGroupID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
			If (dv_x_SiteCategoryGroupID Is Nothing) Then dv_x_SiteCategoryGroupID = SiteLinkdal.LookUpTable("SiteCategoryGroupID")
			x_SiteCategoryGroupID.DataSource = dv_x_SiteCategoryGroupID
			x_SiteCategoryGroupID.DataBind()
			x_SiteCategoryGroupID.Items.Insert(0, New ListItem("Please Select", ""))
			Dim v_SiteCategoryGroupID As String
			If (row.SiteCategoryGroupID.HasValue) Then v_SiteCategoryGroupID = Convert.ToString(row.SiteCategoryGroupID) Else v_SiteCategoryGroupID = String.Empty

			' Field CompanyID
			If (control.FindControl("ax_CompanyID") IsNot Nothing) Then
				Dim cdd As CascadingDropDown = TryCast(control.FindControl("ax_CompanyID"), CascadingDropDown)
				Dim v_CompanyID As String
				If (row.CompanyID.HasValue) Then v_CompanyID = Convert.ToString(row.CompanyID) Else v_CompanyID = String.Empty
				cdd.SelectedValue = v_CompanyID
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
		Dim x_CategoryID As DropDownList = TryCast(control.FindControl("x_CategoryID"), DropDownList)
		Dim z_CategoryID As HiddenField = TryCast(control.FindControl("z_CategoryID"), HiddenField)
		sSrchStr = x_CategoryID.SelectedValue
		sSrchOpr = z_CategoryID.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_CategoryID", sSrchStr, "z_CategoryID", sSrchOpr)
		End If
		Dim x_LinkTypeCD As DropDownList = TryCast(control.FindControl("x_LinkTypeCD"), DropDownList)
		Dim z_LinkTypeCD As HiddenField = TryCast(control.FindControl("z_LinkTypeCD"), HiddenField)
		sSrchStr = x_LinkTypeCD.SelectedValue
		sSrchOpr = z_LinkTypeCD.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_LinkTypeCD", sSrchStr, "z_LinkTypeCD", sSrchOpr)
		End If
		Dim x_Title As TextBox = TryCast(control.FindControl("x_Title"), TextBox)
		Dim z_Title As HiddenField = TryCast(control.FindControl("z_Title"), HiddenField)
		sSrchStr = x_Title.Text
		sSrchOpr = z_Title.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_Title", sSrchStr, "z_Title", sSrchOpr)
		End If
		Dim x_SiteCategoryID As DropDownList = TryCast(control.FindControl("x_SiteCategoryID"), DropDownList)
		Dim z_SiteCategoryID As HiddenField = TryCast(control.FindControl("z_SiteCategoryID"), HiddenField)
		sSrchStr = x_SiteCategoryID.SelectedValue
		sSrchOpr = z_SiteCategoryID.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_SiteCategoryID", sSrchStr, "z_SiteCategoryID", sSrchOpr)
		End If
		Dim x_SiteCategoryGroupID As DropDownList = TryCast(control.FindControl("x_SiteCategoryGroupID"), DropDownList)
		Dim z_SiteCategoryGroupID As HiddenField = TryCast(control.FindControl("z_SiteCategoryGroupID"), HiddenField)
		sSrchStr = x_SiteCategoryGroupID.SelectedValue
		sSrchOpr = z_SiteCategoryGroupID.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_SiteCategoryGroupID", sSrchStr, "z_SiteCategoryGroupID", sSrchOpr)
		End If
		Dim x_CompanyID As DropDownList = TryCast(control.FindControl("x_CompanyID"), DropDownList)
		Dim z_CompanyID As HiddenField = TryCast(control.FindControl("z_CompanyID"), HiddenField)
		sSrchStr = x_CompanyID.SelectedValue
		sSrchOpr = z_CompanyID.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_CompanyID", sSrchStr, "z_CompanyID", sSrchOpr)
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
		Dim data As SiteLinkdal = New SiteLinkdal()

		' SiteCategoryTypeID
		If (control.FindControl("ax_SiteCategoryTypeID") IsNot Nothing) Then
			Dim cdd As CascadingDropDown = TryCast(control.FindControl("ax_SiteCategoryTypeID"), CascadingDropDown)
			Dim v_SiteCategoryTypeID As String = Convert.ToString(data.AdvancedSearchParm1.SiteCategoryTypeID)
			cdd.SelectedValue = v_SiteCategoryTypeID
		End If

		' CategoryID
		Dim x_CategoryID As DropDownList = TryCast(control.FindControl("x_CategoryID"), DropDownList)
		x_CategoryID.DataValueField = "ewValueField"
		x_CategoryID.DataTextField = "ewTextField"
		Dim dv_x_CategoryID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
		if (dv_x_CategoryID Is Nothing) Then dv_x_CategoryID = SiteLinkdal.LookUpTable("CategoryID")
		x_CategoryID.DataSource = dv_x_CategoryID
		x_CategoryID.DataBind()
		x_CategoryID.Items.Insert(0, new ListItem("Please Select", ""))
		Dim v_CategoryID As String = Convert.ToString(data.AdvancedSearchParm1.CategoryID)
		x_CategoryID.ClearSelection()
		For Each li As ListItem In x_CategoryID.Items
			If (li.Value.ToString() = v_CategoryID) Then
				li.Selected = True
				Exit For
			End If
		Next

		' LinkTypeCD
		Dim x_LinkTypeCD As DropDownList = TryCast(control.FindControl("x_LinkTypeCD"), DropDownList)
		x_LinkTypeCD.DataValueField = "ewValueField"
		x_LinkTypeCD.DataTextField = "ewTextField"
		Dim dv_x_LinkTypeCD As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
		if (dv_x_LinkTypeCD Is Nothing) Then dv_x_LinkTypeCD = SiteLinkdal.LookUpTable("LinkTypeCD")
		x_LinkTypeCD.DataSource = dv_x_LinkTypeCD
		x_LinkTypeCD.DataBind()
		x_LinkTypeCD.Items.Insert(0, new ListItem("Please Select", ""))
		Dim v_LinkTypeCD As String = data.AdvancedSearchParm1.LinkTypeCD
		x_LinkTypeCD.ClearSelection()
		For Each li As ListItem In x_LinkTypeCD.Items
			If (li.Value.ToString() = v_LinkTypeCD) Then
				li.Selected = True
				Exit For
			End If
		Next

		' Title
		If (data.AdvancedSearchParm1.Title IsNot Nothing) Then
			Dim x_Title As TextBox = TryCast(control.FindControl("x_Title"), TextBox)
			x_Title.Text = Convert.ToString(data.AdvancedSearchParm1.Title)
		End If

		' SiteCategoryID
		If (control.FindControl("ax_SiteCategoryID") IsNot Nothing) Then
			Dim cdd As CascadingDropDown = TryCast(control.FindControl("ax_SiteCategoryID"), CascadingDropDown)
			Dim v_SiteCategoryID As String = Convert.ToString(data.AdvancedSearchParm1.SiteCategoryID)
			cdd.SelectedValue = v_SiteCategoryID
		End If

		' SiteCategoryGroupID
		Dim x_SiteCategoryGroupID As DropDownList = TryCast(control.FindControl("x_SiteCategoryGroupID"), DropDownList)
		x_SiteCategoryGroupID.DataValueField = "ewValueField"
		x_SiteCategoryGroupID.DataTextField = "ewTextField"
		Dim dv_x_SiteCategoryGroupID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
		if (dv_x_SiteCategoryGroupID Is Nothing) Then dv_x_SiteCategoryGroupID = SiteLinkdal.LookUpTable("SiteCategoryGroupID")
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

		' CompanyID
		If (control.FindControl("ax_CompanyID") IsNot Nothing) Then
			Dim cdd As CascadingDropDown = TryCast(control.FindControl("ax_CompanyID"), CascadingDropDown)
			Dim v_CompanyID As String = Convert.ToString(data.AdvancedSearchParm1.CompanyID)
			cdd.SelectedValue = v_CompanyID
		End If
	End Sub

	' *******************
	' *  Get User Filter
	' *******************

	Private Function GetUserFilter() As String 
		Return SiteLinkinf.GetUserFilter()
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
	<p><span class="aspnetmaker">Search TABLE: Site Link</span></p>
	<p>
		<asp:Hyperlink id="lnkList" Text="Back to List" NavigateUrl="sitelink_list.aspx" CssClass="aspnetmaker" runat="server" />
		<asp:Label id="lblReturnUrl" Visible="False" CssClass="aspnetmaker" runat="server">sitelink_list.aspx</asp:Label>
	</p>
	<script type='text/javascript'>
	function ew_ClearForm() {
		ew_ResetDropDownList(ew.Controls['SiteLink_srch_x_SiteCategoryTypeID']);
		ew_ResetDropDownList(ew.Controls['SiteLink_srch_x_CategoryID']);
		ew_ResetDropDownList(ew.Controls['SiteLink_srch_x_LinkTypeCD']);
		ew_ResetElement(ew.Controls['SiteLink_srch_x_Title']);
		ew_ResetDropDownList(ew.Controls['SiteLink_srch_x_SiteCategoryID']);
		ew_ResetDropDownList(ew.Controls['SiteLink_srch_x_SiteCategoryGroupID']);
		ew_ResetDropDownList(ew.Controls['SiteLink_srch_x_CompanyID']);
	}
	</script>
	<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
	<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
	</asp:PlaceHolder>
	<asp:ValidationSummary id="xevs_SiteLink" CssClass="aspnetmaker" runat="server"
		 HeaderText=""
		 ShowSummary="False"
		 ShowMessageBox="True"
		 ForeColor="#FF0000" />
	<asp:Table runat="server" CssClass="ewDetailTable" ID="SiteLinkTable">
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_SiteCategoryTypeID">Site Type</asp:Label></span>
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
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_CategoryID">Link Category</asp:Label></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">=<asp:HiddenField id="z_CategoryID" value="=,," runat="server" /></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">
<asp:DropDownList ID="x_CategoryID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
</span>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_LinkTypeCD">Link Type</asp:Label></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">LIKE<asp:HiddenField id="z_LinkTypeCD" value="LIKE,'%,%'" runat="server" /></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">
<asp:DropDownList ID="x_LinkTypeCD" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
</span>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_Title">Title</asp:Label></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">LIKE<asp:HiddenField id="z_Title" value="LIKE,'%,%'" runat="server" /></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">
<asp:TextBox id="x_Title" Columns="30" MaxLength="255" CssClass="aspnetmaker" runat="server" />
</span>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_SiteCategoryID">Site Category</asp:Label></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">=<asp:HiddenField id="z_SiteCategoryID" value="=,," runat="server" /></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">
<asp:DropDownList ID="x_SiteCategoryID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
<ew:CascadingDropDown ID="ax_SiteCategoryID" runat="server" EncryptKey="<%$ EWCode: Share.RandomKey %>"
	TargetControlID="x_SiteCategoryID" Category="x_SiteCategoryID" 
	PromptText="Please Select" ServiceMethod="GetDropDownList" ServicePath="ewWebService.asmx" 
	SqlCommand="SELECT [SiteCategoryID], [CategoryName] FROM [SiteCategory] WHERE ([SiteCategoryTypeID]=@FILTER_VALUE) ORDER BY [CategoryName] Asc"
	ParentCategory="x_SiteCategoryTypeID" ParentControlID="x_SiteCategoryTypeID"
	DisplayFieldNames="CategoryName" EnabledDropDown="true" />
</span>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_SiteCategoryGroupID">Site Group</asp:Label></span>
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
	SqlCommand="SELECT [CompanyID], [CompanyName] FROM [Company] WHERE ([SiteCategoryTypeID]=@FILTER_VALUE) ORDER BY [CompanyName] Asc"
	ParentCategory="x_SiteCategoryTypeID" ParentControlID="x_SiteCategoryTypeID"
	DisplayFieldNames="CompanyName" EnabledDropDown="true" />
</span>
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
	<br />
	<asp:Button id="btnSearch" Text="Search" OnClick="btnSearch_Click" CssClass="aspnetmaker" runat="server" />
	<input type="button" id="btnReset" onclick="ew_ClearForm();" class="aspnetmaker" value="Reset"/>
</asp:Content>
