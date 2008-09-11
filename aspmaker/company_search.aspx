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
			objProfile = CustomProfile.GetTable(Share.ProjectName, Companyinf.TableVar)
		End Sub

		' *************************
		' *  Handler for Page Load
		' *************************

		Protected Sub Page_Load(ByVal s As Object, ByVal e As System.EventArgs)
			Response.Cache.SetCacheability(HttpCacheability.NoCache)
			Page.ClientScript.RegisterClientScriptInclude("ewv", "ewv.js")
			If (Not Page.IsPostBack) Then
				LoadData() ' Initialize Controls
				LoadSearchControls(CompanyTable)
			End If
			Dim jsString As String = String.Empty
			jsString &= "if (!ew) { ew = new Object(); ew.Controls = new Array(); }" & vbCrLf
			jsString &= "ew.Controls['Company_srch_x_UseBreadCrumbURL'] = '" & x_UseBreadCrumbURL.UniqueID & "'" & vbCrLf
			jsString &= "ew.Controls['Company_srch_x_SingleSiteGallery'] = '" & x_SingleSiteGallery.UniqueID & "'" & vbCrLf
			jsString &= "ew.Controls['Company_srch_x_SiteCategoryTypeID'] = '" & x_SiteCategoryTypeID.ClientID & "'" & vbCrLf
			jsString &= "ew.Controls['Company_srch_x_ActiveFL'] = '" & x_ActiveFL.UniqueID & "'" & vbCrLf
			Page.ClientScript.RegisterClientScriptBlock(System.Type.GetType("System.String"), "CtrlID", jsString, True)
			Page.Form.DefaultFocus = x_UseBreadCrumbURL.ClientID
			Try
				Companydal.OpenConnection()
			Catch
			End Try
		End Sub

		' **************************
		' *  Handler for Page Unload
		' **************************

		Protected Sub Page_Unload(ByVal s As Object, ByVal e As System.EventArgs)
			Try
				Companydal.CloseAndDisposeConnection()
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
			Dim messageList As ArrayList = ValidateInputValues(CompanyTable, Core.PageType.Search)
			If (messageList IsNot Nothing) Then
				Dim sMsg As String = String.Empty 
				For Each sErrMsg As String In messageList
					sMsg += sErrMsg + "<br>" 
				Next
				lblMessage.Text = sMsg 
				pnlMessage.Visible = True 
				Return 
			End If
			Dim sUrl As String = LoadSearchUrl(CompanyTable) 
			If (Not String.IsNullOrEmpty(sUrl)) Then
				objProfile.PageIndex = 0 ' reset page index
				sUrl &= "&Company_psearchtype=&Company_psearch="
				Response.Redirect (lblReturnUrl.Text + "?" + sUrl) 
			End If
		End Sub

	'*********************************
	'*  Load Default Data to Controls
	'*********************************

	Private Sub LoadData()
		Dim source As WebControl = TryCast(CompanyTable, WebControl)
		Dim row As Companyrow = New Companyrow() ' dummy record for rendering
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
		End If
		Return IIf(messageList.Count = 0, Nothing, messageList)
	End Function

	' ********************************************
	' *  Convert Row Object Value To Input Control
	' ********************************************

	Private Sub RowToControl(ByVal row As Companyrow, ByVal control As WebControl, ByVal ct As Core.CtrlType)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		If (ct = Core.CtrlType.Edit) Then ' set up edit control

			' Field UseBreadCrumbURL
			Dim x_UseBreadCrumbURL As CheckBox = TryCast(control.FindControl("x_UseBreadCrumbURL"), CheckBox)
			Dim sUseBreadCrumbURL As String = row.UseBreadCrumbURL.ToString()
			If ((sUseBreadCrumbURL IsNot Nothing) AndAlso sUseBreadCrumbURL <> "") Then
				If (Convert.ToBoolean(sUseBreadCrumbURL)) Then
					x_UseBreadCrumbURL.Checked = True
				Else
					x_UseBreadCrumbURL.Checked = False
				End If
			End If

			' Field SingleSiteGallery
			Dim x_SingleSiteGallery As CheckBox = TryCast(control.FindControl("x_SingleSiteGallery"), CheckBox)
			Dim sSingleSiteGallery As String = row.SingleSiteGallery.ToString()
			If ((sSingleSiteGallery IsNot Nothing) AndAlso sSingleSiteGallery <> "") Then
				If (Convert.ToBoolean(sSingleSiteGallery)) Then
					x_SingleSiteGallery.Checked = True
				Else
					x_SingleSiteGallery.Checked = False
				End If
			End If

			' Field SiteCategoryTypeID
			Dim x_SiteCategoryTypeID As DropDownList = TryCast(control.FindControl("x_SiteCategoryTypeID"), DropDownList)
			x_SiteCategoryTypeID.DataValueField = "ewValueField"
			x_SiteCategoryTypeID.DataTextField = "ewTextField"
			Dim dv_x_SiteCategoryTypeID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
			If (dv_x_SiteCategoryTypeID Is Nothing) Then dv_x_SiteCategoryTypeID = Companydal.LookUpTable("SiteCategoryTypeID")
			x_SiteCategoryTypeID.DataSource = dv_x_SiteCategoryTypeID
			x_SiteCategoryTypeID.DataBind()
			x_SiteCategoryTypeID.Items.Insert(0, New ListItem("Please Select", ""))
			Dim v_SiteCategoryTypeID As String
			If (row.SiteCategoryTypeID.HasValue) Then v_SiteCategoryTypeID = Convert.ToString(row.SiteCategoryTypeID) Else v_SiteCategoryTypeID = String.Empty

			' Field ActiveFL
			Dim x_ActiveFL As CheckBox = TryCast(control.FindControl("x_ActiveFL"), CheckBox)
			Dim sActiveFL As String = row.ActiveFL.ToString()
			If ((sActiveFL IsNot Nothing) AndAlso sActiveFL <> "") Then
				If (Convert.ToBoolean(sActiveFL)) Then
					x_ActiveFL.Checked = True
				Else
					x_ActiveFL.Checked = False
				End If
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
		Dim x_UseBreadCrumbURL As CheckBox = TryCast(control.FindControl("x_UseBreadCrumbURL"), CheckBox)
		Dim z_UseBreadCrumbURL As HiddenField = TryCast(control.FindControl("z_UseBreadCrumbURL"), HiddenField)
		sSrchStr = IIf(x_UseBreadCrumbURL.Checked, "True", String.Empty)
		sSrchOpr = z_UseBreadCrumbURL.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_UseBreadCrumbURL", sSrchStr, "z_UseBreadCrumbURL", sSrchOpr)
		End If
		Dim x_SingleSiteGallery As CheckBox = TryCast(control.FindControl("x_SingleSiteGallery"), CheckBox)
		Dim z_SingleSiteGallery As HiddenField = TryCast(control.FindControl("z_SingleSiteGallery"), HiddenField)
		sSrchStr = IIf(x_SingleSiteGallery.Checked, "True", String.Empty)
		sSrchOpr = z_SingleSiteGallery.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_SingleSiteGallery", sSrchStr, "z_SingleSiteGallery", sSrchOpr)
		End If
		Dim x_SiteCategoryTypeID As DropDownList = TryCast(control.FindControl("x_SiteCategoryTypeID"), DropDownList)
		Dim z_SiteCategoryTypeID As HiddenField = TryCast(control.FindControl("z_SiteCategoryTypeID"), HiddenField)
		sSrchStr = x_SiteCategoryTypeID.SelectedValue
		sSrchOpr = z_SiteCategoryTypeID.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_SiteCategoryTypeID", sSrchStr, "z_SiteCategoryTypeID", sSrchOpr)
		End If
		Dim x_ActiveFL As CheckBox = TryCast(control.FindControl("x_ActiveFL"), CheckBox)
		Dim z_ActiveFL As HiddenField = TryCast(control.FindControl("z_ActiveFL"), HiddenField)
		sSrchStr = IIf(x_ActiveFL.Checked, "True", String.Empty)
		sSrchOpr = z_ActiveFL.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_ActiveFL", sSrchStr, "z_ActiveFL", sSrchOpr)
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
		Dim data As Companydal = New Companydal()

		' UseBreadCrumbURL
		Dim x_UseBreadCrumbURL As CheckBox= TryCast(control.FindControl("x_UseBreadCrumbURL"), CheckBox)
		Dim v_UseBreadCrumbURL As String = Convert.ToString(data.AdvancedSearchParm1.UseBreadCrumbURL)
		x_UseBreadCrumbURL.Checked = (Not String.IsNullOrEmpty(v_UseBreadCrumbURL) AndAlso Convert.ToBoolean(v_UseBreadCrumbURL))

		' SingleSiteGallery
		Dim x_SingleSiteGallery As CheckBox= TryCast(control.FindControl("x_SingleSiteGallery"), CheckBox)
		Dim v_SingleSiteGallery As String = Convert.ToString(data.AdvancedSearchParm1.SingleSiteGallery)
		x_SingleSiteGallery.Checked = (Not String.IsNullOrEmpty(v_SingleSiteGallery) AndAlso Convert.ToBoolean(v_SingleSiteGallery))

		' SiteCategoryTypeID
		Dim x_SiteCategoryTypeID As DropDownList = TryCast(control.FindControl("x_SiteCategoryTypeID"), DropDownList)
		x_SiteCategoryTypeID.DataValueField = "ewValueField"
		x_SiteCategoryTypeID.DataTextField = "ewTextField"
		Dim dv_x_SiteCategoryTypeID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
		if (dv_x_SiteCategoryTypeID Is Nothing) Then dv_x_SiteCategoryTypeID = Companydal.LookUpTable("SiteCategoryTypeID")
		x_SiteCategoryTypeID.DataSource = dv_x_SiteCategoryTypeID
		x_SiteCategoryTypeID.DataBind()
		x_SiteCategoryTypeID.Items.Insert(0, new ListItem("Please Select", ""))
		Dim v_SiteCategoryTypeID As String = Convert.ToString(data.AdvancedSearchParm1.SiteCategoryTypeID)
		x_SiteCategoryTypeID.ClearSelection()
		For Each li As ListItem In x_SiteCategoryTypeID.Items
			If (li.Value.ToString() = v_SiteCategoryTypeID) Then
				li.Selected = True
				Exit For
			End If
		Next

		' ActiveFL
		Dim x_ActiveFL As CheckBox= TryCast(control.FindControl("x_ActiveFL"), CheckBox)
		Dim v_ActiveFL As String = Convert.ToString(data.AdvancedSearchParm1.ActiveFL)
		x_ActiveFL.Checked = (Not String.IsNullOrEmpty(v_ActiveFL) AndAlso Convert.ToBoolean(v_ActiveFL))
	End Sub

	' *******************
	' *  Get User Filter
	' *******************

	Private Function GetUserFilter() As String 
		Return Companyinf.GetUserFilter()
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
	<p><span class="aspnetmaker">Search TABLE: Company</span></p>
	<p>
		<asp:Hyperlink id="lnkList" Text="Back to List" NavigateUrl="company_list.aspx" CssClass="aspnetmaker" runat="server" />
		<asp:Label id="lblReturnUrl" Visible="False" CssClass="aspnetmaker" runat="server">company_list.aspx</asp:Label>
	</p>
	<script type='text/javascript'>
	function ew_ClearForm() {
		ew_ResetCheckBox(ew.Controls['Company_srch_x_UseBreadCrumbURL']);
		ew_ResetCheckBox(ew.Controls['Company_srch_x_SingleSiteGallery']);
		ew_ResetDropDownList(ew.Controls['Company_srch_x_SiteCategoryTypeID']);
		ew_ResetCheckBox(ew.Controls['Company_srch_x_ActiveFL']);
	}
	</script>
	<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
	<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
	</asp:PlaceHolder>
	<asp:ValidationSummary id="xevs_Company" CssClass="aspnetmaker" runat="server"
		 HeaderText=""
		 ShowSummary="False"
		 ShowMessageBox="True"
		 ForeColor="#FF0000" />
	<asp:Table runat="server" CssClass="ewDetailTable" ID="CompanyTable">
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_UseBreadCrumbURL">Use Bread Crumb URL</asp:Label></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">=<asp:HiddenField id="z_UseBreadCrumbURL" value="=,," runat="server" /></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">
<asp:CheckBox ID="x_UseBreadCrumbURL" CssClass="aspnetmakerlist" runat="server" />
</span>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_SingleSiteGallery">Single Site Gallery</asp:Label></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">=<asp:HiddenField id="z_SingleSiteGallery" value="=,," runat="server" /></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">
<asp:CheckBox ID="x_SingleSiteGallery" CssClass="aspnetmakerlist" runat="server" />
</span>
			</asp:TableCell>
		</asp:TableRow>
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
</span>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_ActiveFL">Active FL</asp:Label></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">=<asp:HiddenField id="z_ActiveFL" value="=,," runat="server" /></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">
<asp:CheckBox ID="x_ActiveFL" CssClass="aspnetmakerlist" runat="server" />
</span>
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
	<br />
	<asp:Button id="btnSearch" Text="Search" OnClick="btnSearch_Click" CssClass="aspnetmaker" runat="server" />
	<input type="button" id="btnReset" onclick="ew_ClearForm();" class="aspnetmaker" value="Reset"/>
</asp:Content>
