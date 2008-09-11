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
			objProfile = CustomProfile.GetTable(Share.ProjectName, Pageinf.TableVar)
		End Sub

		' *************************
		' *  Handler for Page Load
		' *************************

		Protected Sub Page_Load(ByVal s As Object, ByVal e As System.EventArgs)
			Response.Cache.SetCacheability(HttpCacheability.NoCache)
			Page.ClientScript.RegisterClientScriptInclude("ewv", "ewv.js")
			If (Not Page.IsPostBack) Then
				LoadData() ' Initialize Controls
				LoadSearchControls(PageTable)
			End If
			Dim jsString As String = String.Empty
			jsString &= "if (!ew) { ew = new Object(); ew.Controls = new Array(); }" & vbCrLf
			jsString &= "ew.Controls['Page_srch_x_PageName'] = '" & x_PageName.ClientID & "'" & vbCrLf
			jsString &= "ew.Controls['Page_srch_x_PageTypeID'] = '" & x_PageTypeID.ClientID & "'" & vbCrLf
			jsString &= "ew.Controls['Page_srch_x_ParentPageID'] = '" & x_ParentPageID.ClientID & "'" & vbCrLf
			jsString &= "ew.Controls['Page_srch_x_CompanyID'] = '" & x_CompanyID.ClientID & "'" & vbCrLf
			jsString &= "ew.Controls['Page_srch_x_GroupID'] = '" & x_GroupID.ClientID & "'" & vbCrLf
			jsString &= "ew.Controls['Page_srch_x_SiteCategoryID'] = '" & x_SiteCategoryID.ClientID & "'" & vbCrLf
			jsString &= "ew.Controls['Page_srch_x_SiteCategoryGroupID'] = '" & x_SiteCategoryGroupID.ClientID & "'" & vbCrLf
			Page.ClientScript.RegisterClientScriptBlock(System.Type.GetType("System.String"), "CtrlID", jsString, True)
			Page.Form.DefaultFocus = x_PageName.ClientID
			Try
				Pagedal.OpenConnection()
			Catch
			End Try
		End Sub

		' **************************
		' *  Handler for Page Unload
		' **************************

		Protected Sub Page_Unload(ByVal s As Object, ByVal e As System.EventArgs)
			Try
				Pagedal.CloseAndDisposeConnection()
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
			Dim messageList As ArrayList = ValidateInputValues(PageTable, Core.PageType.Search)
			If (messageList IsNot Nothing) Then
				Dim sMsg As String = String.Empty 
				For Each sErrMsg As String In messageList
					sMsg += sErrMsg + "<br>" 
				Next
				lblMessage.Text = sMsg 
				pnlMessage.Visible = True 
				Return 
			End If
			Dim sUrl As String = LoadSearchUrl(PageTable) 
			If (Not String.IsNullOrEmpty(sUrl)) Then
				objProfile.PageIndex = 0 ' reset page index
				sUrl &= "&Page_psearchtype=&Page_psearch="
				Response.Redirect (lblReturnUrl.Text + "?" + sUrl) 
			End If
		End Sub

	'*********************************
	'*  Load Default Data to Controls
	'*********************************

	Private Sub LoadData()
		Dim source As WebControl = TryCast(PageTable, WebControl)
		Dim row As Pagerow = New Pagerow() ' dummy record for rendering
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
			Dim x_PageName As TextBox= TryCast(control.FindControl("x_PageName"), TextBox)
			If (x_PageName IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_PageName.Text)) Then
					messageList.Add("Invalid Value (String): Name")
				End If
			End If
			Dim x_PageTypeID As DropDownList= TryCast(control.FindControl("x_PageTypeID"), DropDownList)
			If (x_PageTypeID IsNot Nothing) Then
				If ((x_PageTypeID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_PageTypeID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Type")
				End If
			End If
			Dim x_ParentPageID As DropDownList= TryCast(control.FindControl("x_ParentPageID"), DropDownList)
			If (x_ParentPageID IsNot Nothing) Then
				If ((x_ParentPageID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_ParentPageID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Parent")
				End If
			End If
			Dim x_CompanyID As DropDownList= TryCast(control.FindControl("x_CompanyID"), DropDownList)
			If (x_CompanyID IsNot Nothing) Then
				If ((x_CompanyID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_CompanyID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Company ID")
				End If
			End If
			Dim x_GroupID As DropDownList= TryCast(control.FindControl("x_GroupID"), DropDownList)
			If (x_GroupID IsNot Nothing) Then
				If ((x_GroupID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_GroupID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Security Group")
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
					messageList.Add("Invalid Value (Int32): Site Group ")
				End If
			End If
		End If
		Return IIf(messageList.Count = 0, Nothing, messageList)
	End Function

	' ********************************************
	' *  Convert Row Object Value To Input Control
	' ********************************************

	Private Sub RowToControl(ByVal row As Pagerow, ByVal control As WebControl, ByVal ct As Core.CtrlType)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		If (ct = Core.CtrlType.Edit) Then ' set up edit control

			' Field PageName
			' Field PageTypeID

			Dim x_PageTypeID As DropDownList = TryCast(control.FindControl("x_PageTypeID"), DropDownList)
			x_PageTypeID.DataValueField = "ewValueField"
			x_PageTypeID.DataTextField = "ewTextField"
			Dim dv_x_PageTypeID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
			If (dv_x_PageTypeID Is Nothing) Then dv_x_PageTypeID = Pagedal.LookUpTable("PageTypeID")
			x_PageTypeID.DataSource = dv_x_PageTypeID
			x_PageTypeID.DataBind()
			x_PageTypeID.Items.Insert(0, New ListItem("Please Select", ""))
			Dim v_PageTypeID As String
			If (row.PageTypeID.HasValue) Then v_PageTypeID = Convert.ToString(row.PageTypeID) Else v_PageTypeID = String.Empty

			' Field ParentPageID
			If (control.FindControl("ax_ParentPageID") IsNot Nothing) Then
				Dim cdd As CascadingDropDown = TryCast(control.FindControl("ax_ParentPageID"), CascadingDropDown)
				Dim v_ParentPageID As String
				If (row.ParentPageID.HasValue) Then v_ParentPageID = Convert.ToString(row.ParentPageID) Else v_ParentPageID = String.Empty
				cdd.SelectedValue = v_ParentPageID
			End If

			' Field CompanyID
			If (control.FindControl("ax_CompanyID") IsNot Nothing) Then
				Dim cdd As CascadingDropDown = TryCast(control.FindControl("ax_CompanyID"), CascadingDropDown)
				Dim v_CompanyID As String
				If (row.CompanyID.HasValue) Then v_CompanyID = Convert.ToString(row.CompanyID) Else v_CompanyID = String.Empty
				cdd.SelectedValue = v_CompanyID
			End If

			' Field GroupID
			Dim x_GroupID As DropDownList = TryCast(control.FindControl("x_GroupID"), DropDownList)
			x_GroupID.DataValueField = "ewValueField"
			x_GroupID.DataTextField = "ewTextField"
			Dim dv_x_GroupID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
			If (dv_x_GroupID Is Nothing) Then dv_x_GroupID = Pagedal.LookUpTable("GroupID")
			x_GroupID.DataSource = dv_x_GroupID
			x_GroupID.DataBind()
			x_GroupID.Items.Insert(0, New ListItem("Please Select", ""))
			Dim v_GroupID As String
			If (row.GroupID.HasValue) Then v_GroupID = Convert.ToString(row.GroupID) Else v_GroupID = String.Empty

			' Field SiteCategoryID
			Dim x_SiteCategoryID As DropDownList = TryCast(control.FindControl("x_SiteCategoryID"), DropDownList)
			x_SiteCategoryID.DataValueField = "ewValueField"
			x_SiteCategoryID.DataTextField = "ewTextField"
			Dim dv_x_SiteCategoryID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
			If (dv_x_SiteCategoryID Is Nothing) Then dv_x_SiteCategoryID = Pagedal.LookUpTable("SiteCategoryID")
			x_SiteCategoryID.DataSource = dv_x_SiteCategoryID
			x_SiteCategoryID.DataBind()
			x_SiteCategoryID.Items.Insert(0, New ListItem("Please Select", ""))
			Dim v_SiteCategoryID As String
			If (row.SiteCategoryID.HasValue) Then v_SiteCategoryID = Convert.ToString(row.SiteCategoryID) Else v_SiteCategoryID = String.Empty

			' Field SiteCategoryGroupID
			Dim x_SiteCategoryGroupID As DropDownList = TryCast(control.FindControl("x_SiteCategoryGroupID"), DropDownList)
			x_SiteCategoryGroupID.DataValueField = "ewValueField"
			x_SiteCategoryGroupID.DataTextField = "ewTextField"
			Dim dv_x_SiteCategoryGroupID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
			If (dv_x_SiteCategoryGroupID Is Nothing) Then dv_x_SiteCategoryGroupID = Pagedal.LookUpTable("SiteCategoryGroupID")
			x_SiteCategoryGroupID.DataSource = dv_x_SiteCategoryGroupID
			x_SiteCategoryGroupID.DataBind()
			x_SiteCategoryGroupID.Items.Insert(0, New ListItem("Please Select", ""))
			Dim v_SiteCategoryGroupID As String
			If (row.SiteCategoryGroupID.HasValue) Then v_SiteCategoryGroupID = Convert.ToString(row.SiteCategoryGroupID) Else v_SiteCategoryGroupID = String.Empty
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
		Dim x_PageName As TextBox = TryCast(control.FindControl("x_PageName"), TextBox)
		Dim z_PageName As HiddenField = TryCast(control.FindControl("z_PageName"), HiddenField)
		sSrchStr = x_PageName.Text
		sSrchOpr = z_PageName.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_PageName", sSrchStr, "z_PageName", sSrchOpr)
		End If
		Dim x_PageTypeID As DropDownList = TryCast(control.FindControl("x_PageTypeID"), DropDownList)
		Dim z_PageTypeID As HiddenField = TryCast(control.FindControl("z_PageTypeID"), HiddenField)
		sSrchStr = x_PageTypeID.SelectedValue
		sSrchOpr = z_PageTypeID.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_PageTypeID", sSrchStr, "z_PageTypeID", sSrchOpr)
		End If
		Dim x_ParentPageID As DropDownList = TryCast(control.FindControl("x_ParentPageID"), DropDownList)
		Dim z_ParentPageID As HiddenField = TryCast(control.FindControl("z_ParentPageID"), HiddenField)
		sSrchStr = x_ParentPageID.SelectedValue
		sSrchOpr = z_ParentPageID.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_ParentPageID", sSrchStr, "z_ParentPageID", sSrchOpr)
		End If
		Dim x_CompanyID As DropDownList = TryCast(control.FindControl("x_CompanyID"), DropDownList)
		Dim z_CompanyID As HiddenField = TryCast(control.FindControl("z_CompanyID"), HiddenField)
		sSrchStr = x_CompanyID.SelectedValue
		sSrchOpr = z_CompanyID.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_CompanyID", sSrchStr, "z_CompanyID", sSrchOpr)
		End If
		Dim x_GroupID As DropDownList = TryCast(control.FindControl("x_GroupID"), DropDownList)
		Dim z_GroupID As HiddenField = TryCast(control.FindControl("z_GroupID"), HiddenField)
		sSrchStr = x_GroupID.SelectedValue
		sSrchOpr = z_GroupID.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_GroupID", sSrchStr, "z_GroupID", sSrchOpr)
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

		' Remove First "&" character
		If (sUrl.Length > 1) Then sUrl = sUrl.SubString(1)
		Return sUrl
	End Function

	' *****************
	' *  Load Controls 
	' *****************

	Private Sub LoadSearchControls(ByVal control As Control) 
		Dim sSearchType As String = String.Empty
		Dim data As Pagedal = New Pagedal()

		' PageName
		If (data.AdvancedSearchParm1.PageName IsNot Nothing) Then
			Dim x_PageName As TextBox = TryCast(control.FindControl("x_PageName"), TextBox)
			x_PageName.Text = Convert.ToString(data.AdvancedSearchParm1.PageName)
		End If

		' PageTypeID
		Dim x_PageTypeID As DropDownList = TryCast(control.FindControl("x_PageTypeID"), DropDownList)
		x_PageTypeID.DataValueField = "ewValueField"
		x_PageTypeID.DataTextField = "ewTextField"
		Dim dv_x_PageTypeID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
		if (dv_x_PageTypeID Is Nothing) Then dv_x_PageTypeID = Pagedal.LookUpTable("PageTypeID")
		x_PageTypeID.DataSource = dv_x_PageTypeID
		x_PageTypeID.DataBind()
		x_PageTypeID.Items.Insert(0, new ListItem("Please Select", ""))
		Dim v_PageTypeID As String = Convert.ToString(data.AdvancedSearchParm1.PageTypeID)
		x_PageTypeID.ClearSelection()
		For Each li As ListItem In x_PageTypeID.Items
			If (li.Value.ToString() = v_PageTypeID) Then
				li.Selected = True
				Exit For
			End If
		Next

		' ParentPageID
		If (control.FindControl("ax_ParentPageID") IsNot Nothing) Then
			Dim cdd As CascadingDropDown = TryCast(control.FindControl("ax_ParentPageID"), CascadingDropDown)
			Dim v_ParentPageID As String = Convert.ToString(data.AdvancedSearchParm1.ParentPageID)
			cdd.SelectedValue = v_ParentPageID
		End If

		' CompanyID
		If (control.FindControl("ax_CompanyID") IsNot Nothing) Then
			Dim cdd As CascadingDropDown = TryCast(control.FindControl("ax_CompanyID"), CascadingDropDown)
			Dim v_CompanyID As String = Convert.ToString(data.AdvancedSearchParm1.CompanyID)
			cdd.SelectedValue = v_CompanyID
		End If

		' GroupID
		Dim x_GroupID As DropDownList = TryCast(control.FindControl("x_GroupID"), DropDownList)
		x_GroupID.DataValueField = "ewValueField"
		x_GroupID.DataTextField = "ewTextField"
		Dim dv_x_GroupID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
		if (dv_x_GroupID Is Nothing) Then dv_x_GroupID = Pagedal.LookUpTable("GroupID")
		x_GroupID.DataSource = dv_x_GroupID
		x_GroupID.DataBind()
		x_GroupID.Items.Insert(0, new ListItem("Please Select", ""))
		Dim v_GroupID As String = Convert.ToString(data.AdvancedSearchParm1.GroupID)
		x_GroupID.ClearSelection()
		For Each li As ListItem In x_GroupID.Items
			If (li.Value.ToString() = v_GroupID) Then
				li.Selected = True
				Exit For
			End If
		Next

		' SiteCategoryID
		Dim x_SiteCategoryID As DropDownList = TryCast(control.FindControl("x_SiteCategoryID"), DropDownList)
		x_SiteCategoryID.DataValueField = "ewValueField"
		x_SiteCategoryID.DataTextField = "ewTextField"
		Dim dv_x_SiteCategoryID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
		if (dv_x_SiteCategoryID Is Nothing) Then dv_x_SiteCategoryID = Pagedal.LookUpTable("SiteCategoryID")
		x_SiteCategoryID.DataSource = dv_x_SiteCategoryID
		x_SiteCategoryID.DataBind()
		x_SiteCategoryID.Items.Insert(0, new ListItem("Please Select", ""))
		Dim v_SiteCategoryID As String = Convert.ToString(data.AdvancedSearchParm1.SiteCategoryID)
		x_SiteCategoryID.ClearSelection()
		For Each li As ListItem In x_SiteCategoryID.Items
			If (li.Value.ToString() = v_SiteCategoryID) Then
				li.Selected = True
				Exit For
			End If
		Next

		' SiteCategoryGroupID
		Dim x_SiteCategoryGroupID As DropDownList = TryCast(control.FindControl("x_SiteCategoryGroupID"), DropDownList)
		x_SiteCategoryGroupID.DataValueField = "ewValueField"
		x_SiteCategoryGroupID.DataTextField = "ewTextField"
		Dim dv_x_SiteCategoryGroupID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
		if (dv_x_SiteCategoryGroupID Is Nothing) Then dv_x_SiteCategoryGroupID = Pagedal.LookUpTable("SiteCategoryGroupID")
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
	End Sub

	' *******************
	' *  Get User Filter
	' *******************

	Private Function GetUserFilter() As String 
		Return Pageinf.GetUserFilter()
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
	<p><span class="aspnetmaker">Search TABLE: Page</span></p>
	<p>
		<asp:Hyperlink id="lnkList" Text="Back to List" NavigateUrl="page_list.aspx" CssClass="aspnetmaker" runat="server" />
		<asp:Label id="lblReturnUrl" Visible="False" CssClass="aspnetmaker" runat="server">page_list.aspx</asp:Label>
	</p>
	<script type='text/javascript'>
	function ew_ClearForm() {
		ew_ResetElement(ew.Controls['Page_srch_x_PageName']);
		ew_ResetDropDownList(ew.Controls['Page_srch_x_PageTypeID']);
		ew_ResetDropDownList(ew.Controls['Page_srch_x_ParentPageID']);
		ew_ResetDropDownList(ew.Controls['Page_srch_x_CompanyID']);
		ew_ResetDropDownList(ew.Controls['Page_srch_x_GroupID']);
		ew_ResetDropDownList(ew.Controls['Page_srch_x_SiteCategoryID']);
		ew_ResetDropDownList(ew.Controls['Page_srch_x_SiteCategoryGroupID']);
	}
	</script>
	<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
	<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
	</asp:PlaceHolder>
	<asp:ValidationSummary id="xevs_Page" CssClass="aspnetmaker" runat="server"
		 HeaderText=""
		 ShowSummary="False"
		 ShowMessageBox="True"
		 ForeColor="#FF0000" />
	<asp:Table runat="server" CssClass="ewDetailTable" ID="PageTable">
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_PageName">Name</asp:Label></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">LIKE<asp:HiddenField id="z_PageName" value="LIKE,'%,%'" runat="server" /></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">
<asp:TextBox id="x_PageName" Columns="80" MaxLength="50" CssClass="aspnetmaker" runat="server" />
</span>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_PageTypeID">Type</asp:Label></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">=<asp:HiddenField id="z_PageTypeID" value="=,," runat="server" /></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">
<asp:DropDownList ID="x_PageTypeID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
</span>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_ParentPageID">Parent</asp:Label></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">=<asp:HiddenField id="z_ParentPageID" value="=,," runat="server" /></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">
<asp:DropDownList ID="x_ParentPageID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
<ew:CascadingDropDown ID="ax_ParentPageID" runat="server" EncryptKey="<%$ EWCode: Share.RandomKey %>"
	TargetControlID="x_ParentPageID" Category="x_ParentPageID" 
	PromptText="Please Select" ServiceMethod="GetDropDownList" ServicePath="ewWebService.asmx" 
	SqlCommand="SELECT [PageID], [PageName] FROM [Page] WHERE ([CompanyID]=@FILTER_VALUE) ORDER BY [PageName] Asc"
	ParentCategory="x_CompanyID" ParentControlID="x_CompanyID"
	DisplayFieldNames="PageName" EnabledDropDown="true" />
</span>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_CompanyID">Company ID</asp:Label></span>
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
	SqlCommand="SELECT [CompanyID], [CompanyName] FROM [Company] ORDER BY [CompanyName]"
	DisplayFieldNames="CompanyName" EnabledDropDown="true" />
</span>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_GroupID">Security Group</asp:Label></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">=<asp:HiddenField id="z_GroupID" value="=,," runat="server" /></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">
<asp:DropDownList ID="x_GroupID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
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
</span>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_SiteCategoryGroupID">Site Group </asp:Label></span>
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
	</asp:Table>
	<br />
	<asp:Button id="btnSearch" Text="Search" OnClick="btnSearch_Click" CssClass="aspnetmaker" runat="server" />
	<input type="button" id="btnReset" onclick="ew_ClearForm();" class="aspnetmaker" value="Reset"/>
</asp:Content>
