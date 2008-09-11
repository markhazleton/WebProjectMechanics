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
			objProfile = CustomProfile.GetTable(Share.ProjectName, SiteParameterTypeinf.TableVar)
		End Sub

		' *************************
		' *  Handler for Page Load
		' *************************

		Protected Sub Page_Load(ByVal s As Object, ByVal e As System.EventArgs)
			Response.Cache.SetCacheability(HttpCacheability.NoCache)
			Page.ClientScript.RegisterClientScriptInclude("ewv", "ewv.js")
			If (Not Page.IsPostBack) Then
				LoadData() ' Initialize Controls
				LoadSearchControls(SiteParameterTypeTable)
			End If
			Dim jsString As String = String.Empty
			jsString &= "if (!ew) { ew = new Object(); ew.Controls = new Array(); }" & vbCrLf
			jsString &= "ew.Controls['SiteParameterType_srch_x_SiteParameterTypeNM'] = '" & x_SiteParameterTypeNM.ClientID & "'" & vbCrLf
			jsString &= "ew.Controls['SiteParameterType_srch_x_SiteParameterTypeDS'] = '" & x_SiteParameterTypeDS.ClientID & "'" & vbCrLf
			jsString &= "ew.Controls['SiteParameterType_srch_x_SiteParameterTypeOrder'] = '" & x_SiteParameterTypeOrder.ClientID & "'" & vbCrLf
			jsString &= "ew.Controls['SiteParameterType_srch_x_SiteParameterTemplate'] = '" & x_SiteParameterTemplate.ClientID & "'" & vbCrLf
			Page.ClientScript.RegisterClientScriptBlock(System.Type.GetType("System.String"), "CtrlID", jsString, True)
			Page.Form.DefaultFocus = x_SiteParameterTypeNM.ClientID
			Try
				SiteParameterTypedal.OpenConnection()
			Catch
			End Try
		End Sub

		' **************************
		' *  Handler for Page Unload
		' **************************

		Protected Sub Page_Unload(ByVal s As Object, ByVal e As System.EventArgs)
			Try
				SiteParameterTypedal.CloseAndDisposeConnection()
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
			Dim messageList As ArrayList = ValidateInputValues(SiteParameterTypeTable, Core.PageType.Search)
			If (messageList IsNot Nothing) Then
				Dim sMsg As String = String.Empty 
				For Each sErrMsg As String In messageList
					sMsg += sErrMsg + "<br>" 
				Next
				lblMessage.Text = sMsg 
				pnlMessage.Visible = True 
				Return 
			End If
			Dim sUrl As String = LoadSearchUrl(SiteParameterTypeTable) 
			If (Not String.IsNullOrEmpty(sUrl)) Then
				objProfile.PageIndex = 0 ' reset page index
				sUrl &= "&SiteParameterType_psearchtype=&SiteParameterType_psearch="
				Response.Redirect (lblReturnUrl.Text + "?" + sUrl) 
			End If
		End Sub

	'*********************************
	'*  Load Default Data to Controls
	'*********************************

	Private Sub LoadData()
		Dim source As WebControl = TryCast(SiteParameterTypeTable, WebControl)
		Dim row As SiteParameterTyperow = New SiteParameterTyperow() ' dummy record for rendering
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
			Dim x_SiteParameterTypeNM As TextBox= TryCast(control.FindControl("x_SiteParameterTypeNM"), TextBox)
			If (x_SiteParameterTypeNM IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_SiteParameterTypeNM.Text)) Then
					messageList.Add("Invalid Value (String): Parameter Type Name")
				End If
			End If
			Dim x_SiteParameterTypeDS As TextBox= TryCast(control.FindControl("x_SiteParameterTypeDS"), TextBox)
			If (x_SiteParameterTypeDS IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_SiteParameterTypeDS.Text)) Then
					messageList.Add("Invalid Value (String): Description")
				End If
			End If
			Dim x_SiteParameterTypeOrder As TextBox= TryCast(control.FindControl("x_SiteParameterTypeOrder"), TextBox)
			If (x_SiteParameterTypeOrder IsNot Nothing) Then
				If (Not DataFormat.CheckInt32(x_SiteParameterTypeOrder.Text)) Then
					messageList.Add("Invalid Value (Int32): Order")
				End If
			End If
			Dim x_SiteParameterTemplate As TextBox= TryCast(control.FindControl("x_SiteParameterTemplate"), TextBox)
			If (x_SiteParameterTemplate IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_SiteParameterTemplate.Text)) Then
					messageList.Add("Invalid Value (String): Template")
				End If
			End If
		End If
		Return IIf(messageList.Count = 0, Nothing, messageList)
	End Function

	' ********************************************
	' *  Convert Row Object Value To Input Control
	' ********************************************

	Private Sub RowToControl(ByVal row As SiteParameterTyperow, ByVal control As WebControl, ByVal ct As Core.CtrlType)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		If (ct = Core.CtrlType.Edit) Then ' set up edit control

			' Field SiteParameterTypeNM
			' Field SiteParameterTypeDS
			' Field SiteParameterTypeOrder
			' Field SiteParameterTemplate

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
		Dim x_SiteParameterTypeNM As TextBox = TryCast(control.FindControl("x_SiteParameterTypeNM"), TextBox)
		Dim z_SiteParameterTypeNM As HiddenField = TryCast(control.FindControl("z_SiteParameterTypeNM"), HiddenField)
		sSrchStr = x_SiteParameterTypeNM.Text
		sSrchOpr = z_SiteParameterTypeNM.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_SiteParameterTypeNM", sSrchStr, "z_SiteParameterTypeNM", sSrchOpr)
		End If
		Dim x_SiteParameterTypeDS As TextBox = TryCast(control.FindControl("x_SiteParameterTypeDS"), TextBox)
		Dim z_SiteParameterTypeDS As HiddenField = TryCast(control.FindControl("z_SiteParameterTypeDS"), HiddenField)
		sSrchStr = x_SiteParameterTypeDS.Text
		sSrchOpr = z_SiteParameterTypeDS.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_SiteParameterTypeDS", sSrchStr, "z_SiteParameterTypeDS", sSrchOpr)
		End If
		Dim x_SiteParameterTypeOrder As TextBox = TryCast(control.FindControl("x_SiteParameterTypeOrder"), TextBox)
		Dim z_SiteParameterTypeOrder As HiddenField = TryCast(control.FindControl("z_SiteParameterTypeOrder"), HiddenField)
		sSrchStr = x_SiteParameterTypeOrder.Text
		sSrchOpr = z_SiteParameterTypeOrder.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_SiteParameterTypeOrder", sSrchStr, "z_SiteParameterTypeOrder", sSrchOpr)
		End If
		Dim x_SiteParameterTemplate As TextBox = TryCast(control.FindControl("x_SiteParameterTemplate"), TextBox)
		Dim z_SiteParameterTemplate As HiddenField = TryCast(control.FindControl("z_SiteParameterTemplate"), HiddenField)
		sSrchStr = x_SiteParameterTemplate.Text
		sSrchOpr = z_SiteParameterTemplate.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_SiteParameterTemplate", sSrchStr, "z_SiteParameterTemplate", sSrchOpr)
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
		Dim data As SiteParameterTypedal = New SiteParameterTypedal()

		' SiteParameterTypeNM
		If (data.AdvancedSearchParm1.SiteParameterTypeNM IsNot Nothing) Then
			Dim x_SiteParameterTypeNM As TextBox = TryCast(control.FindControl("x_SiteParameterTypeNM"), TextBox)
			x_SiteParameterTypeNM.Text = Convert.ToString(data.AdvancedSearchParm1.SiteParameterTypeNM)
		End If

		' SiteParameterTypeDS
		If (data.AdvancedSearchParm1.SiteParameterTypeDS IsNot Nothing) Then
			Dim x_SiteParameterTypeDS As TextBox = TryCast(control.FindControl("x_SiteParameterTypeDS"), TextBox)
			x_SiteParameterTypeDS.Text = Convert.ToString(data.AdvancedSearchParm1.SiteParameterTypeDS)
		End If

		' SiteParameterTypeOrder
		If (data.AdvancedSearchParm1.SiteParameterTypeOrder.HasValue) Then
			Dim x_SiteParameterTypeOrder As TextBox = TryCast(control.FindControl("x_SiteParameterTypeOrder"), TextBox)
			x_SiteParameterTypeOrder.Text = Convert.ToString(data.AdvancedSearchParm1.SiteParameterTypeOrder)
		End If

		' SiteParameterTemplate
		If (data.AdvancedSearchParm1.SiteParameterTemplate IsNot Nothing) Then
			Dim x_SiteParameterTemplate As TextBox = TryCast(control.FindControl("x_SiteParameterTemplate"), TextBox)
			x_SiteParameterTemplate.Text = Convert.ToString(data.AdvancedSearchParm1.SiteParameterTemplate)
		End If
	End Sub

	' *******************
	' *  Get User Filter
	' *******************

	Private Function GetUserFilter() As String 
		Return SiteParameterTypeinf.GetUserFilter()
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
	<p><span class="aspnetmaker">Search TABLE: Site Parameter Type</span></p>
	<p>
		<asp:Hyperlink id="lnkList" Text="Back to List" NavigateUrl="siteparametertype_list.aspx" CssClass="aspnetmaker" runat="server" />
		<asp:Label id="lblReturnUrl" Visible="False" CssClass="aspnetmaker" runat="server">siteparametertype_list.aspx</asp:Label>
	</p>
	<script type='text/javascript'>
	function ew_ClearForm() {
		ew_ResetElement(ew.Controls['SiteParameterType_srch_x_SiteParameterTypeNM']);
		ew_ResetElement(ew.Controls['SiteParameterType_srch_x_SiteParameterTypeDS']);
		ew_ResetElement(ew.Controls['SiteParameterType_srch_x_SiteParameterTypeOrder']);
		ew_ResetElement(ew.Controls['SiteParameterType_srch_x_SiteParameterTemplate']);
	}
	</script>
	<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
	<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
	</asp:PlaceHolder>
	<asp:ValidationSummary id="xevs_SiteParameterType" CssClass="aspnetmaker" runat="server"
		 HeaderText=""
		 ShowSummary="False"
		 ShowMessageBox="True"
		 ForeColor="#FF0000" />
	<asp:Table runat="server" CssClass="ewDetailTable" ID="SiteParameterTypeTable">
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_SiteParameterTypeNM">Parameter Type Name</asp:Label></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">LIKE<asp:HiddenField id="z_SiteParameterTypeNM" value="LIKE,'%,%'" runat="server" /></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">
<asp:TextBox id="x_SiteParameterTypeNM" Columns="30" MaxLength="255" CssClass="aspnetmaker" runat="server" />
</span>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_SiteParameterTypeDS">Description</asp:Label></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">LIKE<asp:HiddenField id="z_SiteParameterTypeDS" value="LIKE,'%,%'" runat="server" /></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">
<asp:TextBox id="x_SiteParameterTypeDS" Columns="30" MaxLength="255" CssClass="aspnetmaker" runat="server" />
</span>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_SiteParameterTypeOrder">Order</asp:Label></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">=<asp:HiddenField id="z_SiteParameterTypeOrder" value="=,," runat="server" /></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">
<asp:TextBox id="x_SiteParameterTypeOrder" Columns="30" CssClass="aspnetmaker" runat="server" />
<ew:IntegerValidator ClientValidationFunction="ew_CheckInteger" CssClass="aspnetmaker" runat="server" ControlToValidate="x_SiteParameterTypeOrder" ErrorMessage="Incorrect integer - Order" Display="None" ForeColor="Red" />
</span>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_SiteParameterTemplate">Template</asp:Label></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">LIKE<asp:HiddenField id="z_SiteParameterTemplate" value="LIKE,'%,%'" runat="server" /></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">
<asp:TextBox id="x_SiteParameterTemplate" TextMode="MultiLine" Rows="4" Columns="35" CssClass="aspnetmaker" runat="server" />
</span>
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
	<br />
	<asp:Button id="btnSearch" Text="Search" OnClick="btnSearch_Click" CssClass="aspnetmaker" runat="server" />
	<input type="button" id="btnReset" onclick="ew_ClearForm();" class="aspnetmaker" value="Reset"/>
</asp:Content>
