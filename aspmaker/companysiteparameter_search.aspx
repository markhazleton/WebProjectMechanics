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
			objProfile = CustomProfile.GetTable(Share.ProjectName, CompanySiteParameterinf.TableVar)
		End Sub

		' *************************
		' *  Handler for Page Load
		' *************************

		Protected Sub Page_Load(ByVal s As Object, ByVal e As System.EventArgs)
			Response.Cache.SetCacheability(HttpCacheability.NoCache)
			Page.ClientScript.RegisterClientScriptInclude("ewv", "ewv.js")
			If (Not Page.IsPostBack) Then
				LoadData() ' Initialize Controls
				LoadSearchControls(CompanySiteParameterTable)
			End If
			Dim jsString As String = String.Empty
			jsString &= "if (!ew) { ew = new Object(); ew.Controls = new Array(); }" & vbCrLf
			jsString &= "ew.Controls['CompanySiteParameter_srch_x_CompanyID'] = '" & x_CompanyID.ClientID & "'" & vbCrLf
			jsString &= "ew.Controls['CompanySiteParameter_srch_x_SiteParameterTypeID'] = '" & x_SiteParameterTypeID.ClientID & "'" & vbCrLf
			jsString &= "ew.Controls['CompanySiteParameter_srch_x_SortOrder'] = '" & x_SortOrder.ClientID & "'" & vbCrLf
			jsString &= "ew.Controls['CompanySiteParameter_srch_x_ParameterValue'] = '" & x_ParameterValue.ClientID & "'" & vbCrLf
			Page.ClientScript.RegisterClientScriptBlock(System.Type.GetType("System.String"), "CtrlID", jsString, True)
			Page.Form.DefaultFocus = x_CompanyID.ClientID
			Try
				CompanySiteParameterdal.OpenConnection()
			Catch
			End Try
		End Sub

		' **************************
		' *  Handler for Page Unload
		' **************************

		Protected Sub Page_Unload(ByVal s As Object, ByVal e As System.EventArgs)
			Try
				CompanySiteParameterdal.CloseAndDisposeConnection()
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
			Dim messageList As ArrayList = ValidateInputValues(CompanySiteParameterTable, Core.PageType.Search)
			If (messageList IsNot Nothing) Then
				Dim sMsg As String = String.Empty 
				For Each sErrMsg As String In messageList
					sMsg += sErrMsg + "<br>" 
				Next
				lblMessage.Text = sMsg 
				pnlMessage.Visible = True 
				Return 
			End If
			Dim sUrl As String = LoadSearchUrl(CompanySiteParameterTable) 
			If (Not String.IsNullOrEmpty(sUrl)) Then
				objProfile.PageIndex = 0 ' reset page index
				sUrl &= "&CompanySiteParameter_psearchtype=&CompanySiteParameter_psearch="
				Response.Redirect (lblReturnUrl.Text + "?" + sUrl) 
			End If
		End Sub

	'*********************************
	'*  Load Default Data to Controls
	'*********************************

	Private Sub LoadData()
		Dim source As WebControl = TryCast(CompanySiteParameterTable, WebControl)
		Dim row As CompanySiteParameterrow = New CompanySiteParameterrow() ' dummy record for rendering
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
			Dim x_CompanyID As DropDownList= TryCast(control.FindControl("x_CompanyID"), DropDownList)
			If (x_CompanyID IsNot Nothing) Then
				If ((x_CompanyID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_CompanyID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Site")
				End If
			End If
			Dim x_SiteParameterTypeID As DropDownList= TryCast(control.FindControl("x_SiteParameterTypeID"), DropDownList)
			If (x_SiteParameterTypeID IsNot Nothing) Then
				If ((x_SiteParameterTypeID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_SiteParameterTypeID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Parameter Type")
				End If
			End If
			Dim x_SortOrder As TextBox= TryCast(control.FindControl("x_SortOrder"), TextBox)
			If (x_SortOrder IsNot Nothing) Then
				If (Not DataFormat.CheckInt32(x_SortOrder.Text)) Then
					messageList.Add("Invalid Value (Int32): Order")
				End If
			End If
			Dim x_ParameterValue As TextBox= TryCast(control.FindControl("x_ParameterValue"), TextBox)
			If (x_ParameterValue IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_ParameterValue.Text)) Then
					messageList.Add("Invalid Value (String): Parameter Value")
				End If
			End If
		End If
		Return IIf(messageList.Count = 0, Nothing, messageList)
	End Function

	' ********************************************
	' *  Convert Row Object Value To Input Control
	' ********************************************

	Private Sub RowToControl(ByVal row As CompanySiteParameterrow, ByVal control As WebControl, ByVal ct As Core.CtrlType)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		If (ct = Core.CtrlType.Edit) Then ' set up edit control

			' Field CompanyID
			Dim x_CompanyID As DropDownList = TryCast(control.FindControl("x_CompanyID"), DropDownList)
			x_CompanyID.DataValueField = "ewValueField"
			x_CompanyID.DataTextField = "ewTextField"
			Dim dv_x_CompanyID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
			If (dv_x_CompanyID Is Nothing) Then dv_x_CompanyID = CompanySiteParameterdal.LookUpTable("CompanyID")
			x_CompanyID.DataSource = dv_x_CompanyID
			x_CompanyID.DataBind()
			x_CompanyID.Items.Insert(0, New ListItem("Please Select", ""))
			Dim v_CompanyID As String
			If (row.CompanyID.HasValue) Then v_CompanyID = Convert.ToString(row.CompanyID) Else v_CompanyID = String.Empty

			' Field SiteParameterTypeID
			Dim x_SiteParameterTypeID As DropDownList = TryCast(control.FindControl("x_SiteParameterTypeID"), DropDownList)
			x_SiteParameterTypeID.DataValueField = "ewValueField"
			x_SiteParameterTypeID.DataTextField = "ewTextField"
			Dim dv_x_SiteParameterTypeID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
			If (dv_x_SiteParameterTypeID Is Nothing) Then dv_x_SiteParameterTypeID = CompanySiteParameterdal.LookUpTable("SiteParameterTypeID")
			x_SiteParameterTypeID.DataSource = dv_x_SiteParameterTypeID
			x_SiteParameterTypeID.DataBind()
			x_SiteParameterTypeID.Items.Insert(0, New ListItem("Please Select", ""))
			Dim v_SiteParameterTypeID As String
			If (row.SiteParameterTypeID.HasValue) Then v_SiteParameterTypeID = Convert.ToString(row.SiteParameterTypeID) Else v_SiteParameterTypeID = String.Empty

			' Field SortOrder
			' Field ParameterValue

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
		Dim x_CompanyID As DropDownList = TryCast(control.FindControl("x_CompanyID"), DropDownList)
		Dim z_CompanyID As HiddenField = TryCast(control.FindControl("z_CompanyID"), HiddenField)
		sSrchStr = x_CompanyID.SelectedValue
		sSrchOpr = z_CompanyID.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_CompanyID", sSrchStr, "z_CompanyID", sSrchOpr)
		End If
		Dim x_SiteParameterTypeID As DropDownList = TryCast(control.FindControl("x_SiteParameterTypeID"), DropDownList)
		Dim z_SiteParameterTypeID As HiddenField = TryCast(control.FindControl("z_SiteParameterTypeID"), HiddenField)
		sSrchStr = x_SiteParameterTypeID.SelectedValue
		sSrchOpr = z_SiteParameterTypeID.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_SiteParameterTypeID", sSrchStr, "z_SiteParameterTypeID", sSrchOpr)
		End If
		Dim x_SortOrder As TextBox = TryCast(control.FindControl("x_SortOrder"), TextBox)
		Dim z_SortOrder As HiddenField = TryCast(control.FindControl("z_SortOrder"), HiddenField)
		sSrchStr = x_SortOrder.Text
		sSrchOpr = z_SortOrder.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_SortOrder", sSrchStr, "z_SortOrder", sSrchOpr)
		End If
		Dim x_ParameterValue As TextBox = TryCast(control.FindControl("x_ParameterValue"), TextBox)
		Dim z_ParameterValue As HiddenField = TryCast(control.FindControl("z_ParameterValue"), HiddenField)
		sSrchStr = x_ParameterValue.Text
		sSrchOpr = z_ParameterValue.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_ParameterValue", sSrchStr, "z_ParameterValue", sSrchOpr)
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
		Dim data As CompanySiteParameterdal = New CompanySiteParameterdal()

		' CompanyID
		Dim x_CompanyID As DropDownList = TryCast(control.FindControl("x_CompanyID"), DropDownList)
		x_CompanyID.DataValueField = "ewValueField"
		x_CompanyID.DataTextField = "ewTextField"
		Dim dv_x_CompanyID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
		if (dv_x_CompanyID Is Nothing) Then dv_x_CompanyID = CompanySiteParameterdal.LookUpTable("CompanyID")
		x_CompanyID.DataSource = dv_x_CompanyID
		x_CompanyID.DataBind()
		x_CompanyID.Items.Insert(0, new ListItem("Please Select", ""))
		Dim v_CompanyID As String = Convert.ToString(data.AdvancedSearchParm1.CompanyID)
		x_CompanyID.ClearSelection()
		For Each li As ListItem In x_CompanyID.Items
			If (li.Value.ToString() = v_CompanyID) Then
				li.Selected = True
				Exit For
			End If
		Next

		' SiteParameterTypeID
		Dim x_SiteParameterTypeID As DropDownList = TryCast(control.FindControl("x_SiteParameterTypeID"), DropDownList)
		x_SiteParameterTypeID.DataValueField = "ewValueField"
		x_SiteParameterTypeID.DataTextField = "ewTextField"
		Dim dv_x_SiteParameterTypeID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
		if (dv_x_SiteParameterTypeID Is Nothing) Then dv_x_SiteParameterTypeID = CompanySiteParameterdal.LookUpTable("SiteParameterTypeID")
		x_SiteParameterTypeID.DataSource = dv_x_SiteParameterTypeID
		x_SiteParameterTypeID.DataBind()
		x_SiteParameterTypeID.Items.Insert(0, new ListItem("Please Select", ""))
		Dim v_SiteParameterTypeID As String = Convert.ToString(data.AdvancedSearchParm1.SiteParameterTypeID)
		x_SiteParameterTypeID.ClearSelection()
		For Each li As ListItem In x_SiteParameterTypeID.Items
			If (li.Value.ToString() = v_SiteParameterTypeID) Then
				li.Selected = True
				Exit For
			End If
		Next

		' SortOrder
		If (data.AdvancedSearchParm1.SortOrder.HasValue) Then
			Dim x_SortOrder As TextBox = TryCast(control.FindControl("x_SortOrder"), TextBox)
			x_SortOrder.Text = Convert.ToString(data.AdvancedSearchParm1.SortOrder)
		End If

		' ParameterValue
		If (data.AdvancedSearchParm1.ParameterValue IsNot Nothing) Then
			Dim x_ParameterValue As TextBox = TryCast(control.FindControl("x_ParameterValue"), TextBox)
			x_ParameterValue.Text = Convert.ToString(data.AdvancedSearchParm1.ParameterValue)
		End If
	End Sub

	' *******************
	' *  Get User Filter
	' *******************

	Private Function GetUserFilter() As String 
		Return CompanySiteParameterinf.GetUserFilter()
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
	<p><span class="aspnetmaker">Search TABLE: Site Parameter</span></p>
	<p>
		<asp:Hyperlink id="lnkList" Text="Back to List" NavigateUrl="companysiteparameter_list.aspx" CssClass="aspnetmaker" runat="server" />
		<asp:Label id="lblReturnUrl" Visible="False" CssClass="aspnetmaker" runat="server">companysiteparameter_list.aspx</asp:Label>
	</p>
	<script type='text/javascript'>
	function ew_ClearForm() {
		ew_ResetDropDownList(ew.Controls['CompanySiteParameter_srch_x_CompanyID']);
		ew_ResetDropDownList(ew.Controls['CompanySiteParameter_srch_x_SiteParameterTypeID']);
		ew_ResetElement(ew.Controls['CompanySiteParameter_srch_x_SortOrder']);
		ew_ResetElement(ew.Controls['CompanySiteParameter_srch_x_ParameterValue']);
	}
	</script>
	<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
	<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
	</asp:PlaceHolder>
	<asp:ValidationSummary id="xevs_CompanySiteParameter" CssClass="aspnetmaker" runat="server"
		 HeaderText=""
		 ShowSummary="False"
		 ShowMessageBox="True"
		 ForeColor="#FF0000" />
	<asp:Table runat="server" CssClass="ewDetailTable" ID="CompanySiteParameterTable">
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_CompanyID">Site</asp:Label></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">=<asp:HiddenField id="z_CompanyID" value="=,," runat="server" /></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">
<asp:DropDownList ID="x_CompanyID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
</span>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_SiteParameterTypeID">Parameter Type</asp:Label></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">=<asp:HiddenField id="z_SiteParameterTypeID" value="=,," runat="server" /></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">
<asp:DropDownList ID="x_SiteParameterTypeID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
</span>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_SortOrder">Order</asp:Label></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">=<asp:HiddenField id="z_SortOrder" value="=,," runat="server" /></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">
<asp:TextBox id="x_SortOrder" Columns="30" CssClass="aspnetmaker" runat="server" />
<ew:IntegerValidator ClientValidationFunction="ew_CheckInteger" CssClass="aspnetmaker" runat="server" ControlToValidate="x_SortOrder" ErrorMessage="Incorrect integer - Order" Display="None" ForeColor="Red" />
</span>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_ParameterValue">Parameter Value</asp:Label></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">LIKE<asp:HiddenField id="z_ParameterValue" value="LIKE,'%,%'" runat="server" /></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">
<asp:TextBox id="x_ParameterValue" TextMode="MultiLine" Rows="4" Columns="35" CssClass="aspnetmaker" runat="server" />
</span>
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
	<br />
	<asp:Button id="btnSearch" Text="Search" OnClick="btnSearch_Click" CssClass="aspnetmaker" runat="server" />
	<input type="button" id="btnReset" onclick="ew_ClearForm();" class="aspnetmaker" value="Reset"/>
</asp:Content>
