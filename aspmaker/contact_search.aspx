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
			objProfile = CustomProfile.GetTable(Share.ProjectName, Contactinf.TableVar)
		End Sub

		' *************************
		' *  Handler for Page Load
		' *************************

		Protected Sub Page_Load(ByVal s As Object, ByVal e As System.EventArgs)
			Response.Cache.SetCacheability(HttpCacheability.NoCache)
			Page.ClientScript.RegisterClientScriptInclude("ewv", "ewv.js")
			If (Not Page.IsPostBack) Then
				LoadData() ' Initialize Controls
				LoadSearchControls(ContactTable)
			End If
			Dim jsString As String = String.Empty
			jsString &= "if (!ew) { ew = new Object(); ew.Controls = new Array(); }" & vbCrLf
			jsString &= "ew.Controls['Contact_srch_x_PrimaryContact'] = '" & x_PrimaryContact.ClientID & "'" & vbCrLf
			jsString &= "ew.Controls['Contact_srch_x_LogonName'] = '" & x_LogonName.ClientID & "'" & vbCrLf
			jsString &= "ew.Controls['Contact_srch_x_CompanyID'] = '" & x_CompanyID.ClientID & "'" & vbCrLf
			jsString &= "ew.Controls['Contact_srch_x_GroupID'] = '" & x_GroupID.ClientID & "'" & vbCrLf
			jsString &= "ew.Controls['Contact_srch_x_CreateDT'] = '" & x_CreateDT.ClientID & "'" & vbCrLf
			jsString &= "ew.Controls['Contact_srch_x_TemplatePrefix'] = '" & x_TemplatePrefix.ClientID & "'" & vbCrLf
			Page.ClientScript.RegisterClientScriptBlock(System.Type.GetType("System.String"), "CtrlID", jsString, True)
			Page.Form.DefaultFocus = x_PrimaryContact.ClientID
			Try
				Contactdal.OpenConnection()
			Catch
			End Try
		End Sub

		' **************************
		' *  Handler for Page Unload
		' **************************

		Protected Sub Page_Unload(ByVal s As Object, ByVal e As System.EventArgs)
			Try
				Contactdal.CloseAndDisposeConnection()
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
			Dim messageList As ArrayList = ValidateInputValues(ContactTable, Core.PageType.Search)
			If (messageList IsNot Nothing) Then
				Dim sMsg As String = String.Empty 
				For Each sErrMsg As String In messageList
					sMsg += sErrMsg + "<br>" 
				Next
				lblMessage.Text = sMsg 
				pnlMessage.Visible = True 
				Return 
			End If
			Dim sUrl As String = LoadSearchUrl(ContactTable) 
			If (Not String.IsNullOrEmpty(sUrl)) Then
				objProfile.PageIndex = 0 ' reset page index
				sUrl &= "&Contact_psearchtype=&Contact_psearch="
				Response.Redirect (lblReturnUrl.Text + "?" + sUrl) 
			End If
		End Sub

	'*********************************
	'*  Load Default Data to Controls
	'*********************************

	Private Sub LoadData()
		Dim source As WebControl = TryCast(ContactTable, WebControl)
		Dim row As Contactrow = New Contactrow() ' dummy record for rendering
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
			Dim x_PrimaryContact As TextBox= TryCast(control.FindControl("x_PrimaryContact"), TextBox)
			If (x_PrimaryContact IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_PrimaryContact.Text)) Then
					messageList.Add("Invalid Value (String): Primary Contact")
				End If
			End If
			Dim x_LogonName As TextBox= TryCast(control.FindControl("x_LogonName"), TextBox)
			If (x_LogonName IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_LogonName.Text)) Then
					messageList.Add("Invalid Value (String): Logon Name")
				End If
			End If
			Dim x_CompanyID As DropDownList= TryCast(control.FindControl("x_CompanyID"), DropDownList)
			If (x_CompanyID IsNot Nothing) Then
				If ((x_CompanyID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_CompanyID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Company")
				End If
			End If
			Dim x_GroupID As DropDownList= TryCast(control.FindControl("x_GroupID"), DropDownList)
			If (x_GroupID IsNot Nothing) Then
				If ((x_GroupID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_GroupID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Group")
				End If
			End If
			Dim x_CreateDT As TextBox= TryCast(control.FindControl("x_CreateDT"), TextBox)
			If (x_CreateDT IsNot Nothing) Then
				If (Not DataFormat.CheckDateTime(x_CreateDT.Text)) Then
					messageList.Add("Invalid Value (DateTime): Modified")
				End If
			End If
			Dim x_TemplatePrefix As DropDownList= TryCast(control.FindControl("x_TemplatePrefix"), DropDownList)
			If (x_TemplatePrefix IsNot Nothing) Then
				If ((x_TemplatePrefix.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckString(x_TemplatePrefix.SelectedValue))) Then
					messageList.Add("Invalid Value (String): Template")
				End If
			End If
		End If
		Return IIf(messageList.Count = 0, Nothing, messageList)
	End Function

	' ********************************************
	' *  Convert Row Object Value To Input Control
	' ********************************************

	Private Sub RowToControl(ByVal row As Contactrow, ByVal control As WebControl, ByVal ct As Core.CtrlType)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		If (ct = Core.CtrlType.Edit) Then ' set up edit control

			' Field PrimaryContact
			' Field LogonName
			' Field CompanyID

			Dim x_CompanyID As DropDownList = TryCast(control.FindControl("x_CompanyID"), DropDownList)
			x_CompanyID.DataValueField = "ewValueField"
			x_CompanyID.DataTextField = "ewTextField"
			Dim dv_x_CompanyID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
			If (dv_x_CompanyID Is Nothing) Then dv_x_CompanyID = Contactdal.LookUpTable("CompanyID")
			x_CompanyID.DataSource = dv_x_CompanyID
			x_CompanyID.DataBind()
			x_CompanyID.Items.Insert(0, New ListItem("Please Select", ""))
			Dim v_CompanyID As String
			If (row.CompanyID.HasValue) Then v_CompanyID = Convert.ToString(row.CompanyID) Else v_CompanyID = String.Empty

			' Field GroupID
			Dim x_GroupID As DropDownList = TryCast(control.FindControl("x_GroupID"), DropDownList)
			x_GroupID.DataValueField = "ewValueField"
			x_GroupID.DataTextField = "ewTextField"
			Dim dv_x_GroupID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
			If (dv_x_GroupID Is Nothing) Then dv_x_GroupID = Contactdal.LookUpTable("GroupID")
			x_GroupID.DataSource = dv_x_GroupID
			x_GroupID.DataBind()
			x_GroupID.Items.Insert(0, New ListItem("Please Select", ""))
			Dim v_GroupID As String
			If (row.GroupID.HasValue) Then v_GroupID = Convert.ToString(row.GroupID) Else v_GroupID = String.Empty

			' Field CreateDT
			' Field TemplatePrefix

			Dim x_TemplatePrefix As DropDownList = TryCast(control.FindControl("x_TemplatePrefix"), DropDownList)
			x_TemplatePrefix.DataValueField = "ewValueField"
			x_TemplatePrefix.DataTextField = "ewTextField"
			Dim dv_x_TemplatePrefix As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
			If (dv_x_TemplatePrefix Is Nothing) Then dv_x_TemplatePrefix = Contactdal.LookUpTable("TemplatePrefix")
			x_TemplatePrefix.DataSource = dv_x_TemplatePrefix
			x_TemplatePrefix.DataBind()
			x_TemplatePrefix.Items.Insert(0, New ListItem("Please Select", ""))
			Dim v_TemplatePrefix As String 
			If (row.TemplatePrefix IsNot Nothing) Then  v_TemplatePrefix = Convert.ToString(row.TemplatePrefix) Else v_TemplatePrefix = String.Empty
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
		Dim x_PrimaryContact As TextBox = TryCast(control.FindControl("x_PrimaryContact"), TextBox)
		Dim z_PrimaryContact As HiddenField = TryCast(control.FindControl("z_PrimaryContact"), HiddenField)
		sSrchStr = x_PrimaryContact.Text
		sSrchOpr = z_PrimaryContact.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_PrimaryContact", sSrchStr, "z_PrimaryContact", sSrchOpr)
		End If
		Dim x_LogonName As TextBox = TryCast(control.FindControl("x_LogonName"), TextBox)
		Dim z_LogonName As HiddenField = TryCast(control.FindControl("z_LogonName"), HiddenField)
		sSrchStr = x_LogonName.Text
		sSrchOpr = z_LogonName.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_LogonName", sSrchStr, "z_LogonName", sSrchOpr)
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
		Dim x_CreateDT As TextBox = TryCast(control.FindControl("x_CreateDT"), TextBox)
		Dim z_CreateDT As HiddenField = TryCast(control.FindControl("z_CreateDT"), HiddenField)
		sSrchStr = DataFormat.UnFormatDateTime(x_CreateDT.Text, 6, "/"c)
		sSrchOpr = z_CreateDT.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_CreateDT", sSrchStr, "z_CreateDT", sSrchOpr)
		End If
		Dim x_TemplatePrefix As DropDownList = TryCast(control.FindControl("x_TemplatePrefix"), DropDownList)
		Dim z_TemplatePrefix As HiddenField = TryCast(control.FindControl("z_TemplatePrefix"), HiddenField)
		sSrchStr = x_TemplatePrefix.SelectedValue
		sSrchOpr = z_TemplatePrefix.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_TemplatePrefix", sSrchStr, "z_TemplatePrefix", sSrchOpr)
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
		Dim data As Contactdal = New Contactdal()

		' PrimaryContact
		If (data.AdvancedSearchParm1.PrimaryContact IsNot Nothing) Then
			Dim x_PrimaryContact As TextBox = TryCast(control.FindControl("x_PrimaryContact"), TextBox)
			x_PrimaryContact.Text = Convert.ToString(data.AdvancedSearchParm1.PrimaryContact)
		End If

		' LogonName
		If (data.AdvancedSearchParm1.LogonName IsNot Nothing) Then
			Dim x_LogonName As TextBox = TryCast(control.FindControl("x_LogonName"), TextBox)
			x_LogonName.Text = Convert.ToString(data.AdvancedSearchParm1.LogonName)
		End If

		' CompanyID
		Dim x_CompanyID As DropDownList = TryCast(control.FindControl("x_CompanyID"), DropDownList)
		x_CompanyID.DataValueField = "ewValueField"
		x_CompanyID.DataTextField = "ewTextField"
		Dim dv_x_CompanyID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
		if (dv_x_CompanyID Is Nothing) Then dv_x_CompanyID = Contactdal.LookUpTable("CompanyID")
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

		' GroupID
		Dim x_GroupID As DropDownList = TryCast(control.FindControl("x_GroupID"), DropDownList)
		x_GroupID.DataValueField = "ewValueField"
		x_GroupID.DataTextField = "ewTextField"
		Dim dv_x_GroupID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
		if (dv_x_GroupID Is Nothing) Then dv_x_GroupID = Contactdal.LookUpTable("GroupID")
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

		' CreateDT
		If (data.AdvancedSearchParm1.CreateDT.HasValue) Then
			Dim x_CreateDT As TextBox = TryCast(control.FindControl("x_CreateDT"), TextBox)
			x_CreateDT.Text = IIf(data.AdvancedSearchParm1.CreateDT.HasValue, DataFormat.DateTimeFormat(6, "/", data.AdvancedSearchParm1.CreateDT), String.Empty)
		End If

		' TemplatePrefix
		Dim x_TemplatePrefix As DropDownList = TryCast(control.FindControl("x_TemplatePrefix"), DropDownList)
		x_TemplatePrefix.DataValueField = "ewValueField"
		x_TemplatePrefix.DataTextField = "ewTextField"
		Dim dv_x_TemplatePrefix As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
		if (dv_x_TemplatePrefix Is Nothing) Then dv_x_TemplatePrefix = Contactdal.LookUpTable("TemplatePrefix")
		x_TemplatePrefix.DataSource = dv_x_TemplatePrefix
		x_TemplatePrefix.DataBind()
		x_TemplatePrefix.Items.Insert(0, new ListItem("Please Select", ""))
		Dim v_TemplatePrefix As String = data.AdvancedSearchParm1.TemplatePrefix
		x_TemplatePrefix.ClearSelection()
		For Each li As ListItem In x_TemplatePrefix.Items
			If (li.Value.ToString() = v_TemplatePrefix) Then
				li.Selected = True
				Exit For
			End If
		Next
	End Sub

	' *******************
	' *  Get User Filter
	' *******************

	Private Function GetUserFilter() As String 
		Return Contactinf.GetUserFilter()
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
	<p><span class="aspnetmaker">Search TABLE: Contact</span></p>
	<p>
		<asp:Hyperlink id="lnkList" Text="Back to List" NavigateUrl="contact_list.aspx" CssClass="aspnetmaker" runat="server" />
		<asp:Label id="lblReturnUrl" Visible="False" CssClass="aspnetmaker" runat="server">contact_list.aspx</asp:Label>
	</p>
	<script type='text/javascript'>
	function ew_ClearForm() {
		ew_ResetElement(ew.Controls['Contact_srch_x_PrimaryContact']);
		ew_ResetElement(ew.Controls['Contact_srch_x_LogonName']);
		ew_ResetDropDownList(ew.Controls['Contact_srch_x_CompanyID']);
		ew_ResetDropDownList(ew.Controls['Contact_srch_x_GroupID']);
		ew_ResetElement(ew.Controls['Contact_srch_x_CreateDT']);
		ew_ResetDropDownList(ew.Controls['Contact_srch_x_TemplatePrefix']);
	}
	</script>
	<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
	<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
	</asp:PlaceHolder>
	<asp:ValidationSummary id="xevs_Contact" CssClass="aspnetmaker" runat="server"
		 HeaderText=""
		 ShowSummary="False"
		 ShowMessageBox="True"
		 ForeColor="#FF0000" />
	<asp:Table runat="server" CssClass="ewDetailTable" ID="ContactTable">
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_PrimaryContact">Primary Contact</asp:Label></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">LIKE<asp:HiddenField id="z_PrimaryContact" value="LIKE,'%,%'" runat="server" /></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">
<asp:TextBox id="x_PrimaryContact" Columns="30" MaxLength="255" CssClass="aspnetmaker" runat="server" />
</span>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_LogonName">Logon Name</asp:Label></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">LIKE<asp:HiddenField id="z_LogonName" value="LIKE,'%,%'" runat="server" /></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">
<asp:TextBox id="x_LogonName" Columns="30" MaxLength="50" CssClass="aspnetmaker" runat="server" />
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
</span>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_GroupID">Group</asp:Label></span>
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
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_CreateDT">Modified</asp:Label></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">=<asp:HiddenField id="z_CreateDT" value="=,#,#" runat="server" /></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">
<asp:TextBox id="x_CreateDT" CssClass="aspnetmaker" runat="server" />
<ew:USDateValidator ClientValidationFunction="ew_CheckUSDate" CssClass="aspnetmaker" runat="server" ControlToValidate="x_CreateDT" ErrorMessage="Incorrect date, format = mm/dd/yyyy - Modified" Display="None" ForeColor="Red" DateSeparator="/" />
</span>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_TemplatePrefix">Template</asp:Label></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">=<asp:HiddenField id="z_TemplatePrefix" value="=,','" runat="server" /></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">
<asp:DropDownList ID="x_TemplatePrefix" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
</span>
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
	<br />
	<asp:Button id="btnSearch" Text="Search" OnClick="btnSearch_Click" CssClass="aspnetmaker" runat="server" />
	<input type="button" id="btnReset" onclick="ew_ClearForm();" class="aspnetmaker" value="Reset"/>
</asp:Content>
