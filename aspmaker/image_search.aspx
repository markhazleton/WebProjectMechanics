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
			objProfile = CustomProfile.GetTable(Share.ProjectName, Imageinf.TableVar)
		End Sub

		' *************************
		' *  Handler for Page Load
		' *************************

		Protected Sub Page_Load(ByVal s As Object, ByVal e As System.EventArgs)
			Response.Cache.SetCacheability(HttpCacheability.NoCache)
			Page.ClientScript.RegisterClientScriptInclude("ewv", "ewv.js")
			If (Not Page.IsPostBack) Then
				LoadData() ' Initialize Controls
				LoadSearchControls(ImageTable)
			End If
			Dim jsString As String = String.Empty
			jsString &= "if (!ew) { ew = new Object(); ew.Controls = new Array(); }" & vbCrLf
			jsString &= "ew.Controls['Image_srch_x_ImageName'] = '" & x_ImageName.ClientID & "'" & vbCrLf
			jsString &= "ew.Controls['Image_srch_x_ContactID'] = '" & x_ContactID.ClientID & "'" & vbCrLf
			jsString &= "ew.Controls['Image_srch_x_CompanyID'] = '" & x_CompanyID.ClientID & "'" & vbCrLf
			jsString &= "ew.Controls['Image_srch_x_title'] = '" & x_title.ClientID & "'" & vbCrLf
			jsString &= "ew.Controls['Image_srch_x_medium'] = '" & x_medium.ClientID & "'" & vbCrLf
			jsString &= "ew.Controls['Image_srch_x_size'] = '" & x_size.ClientID & "'" & vbCrLf
			jsString &= "ew.Controls['Image_srch_x_price'] = '" & x_price.ClientID & "'" & vbCrLf
			jsString &= "ew.Controls['Image_srch_x_color'] = '" & x_color.ClientID & "'" & vbCrLf
			jsString &= "ew.Controls['Image_srch_x_subject'] = '" & x_subject.ClientID & "'" & vbCrLf
			jsString &= "ew.Controls['Image_srch_x_sold'] = '" & x_sold.UniqueID & "'" & vbCrLf
			Page.ClientScript.RegisterClientScriptBlock(System.Type.GetType("System.String"), "CtrlID", jsString, True)
			Page.Form.DefaultFocus = x_ImageName.ClientID
			Try
				Imagedal.OpenConnection()
			Catch
			End Try
		End Sub

		' **************************
		' *  Handler for Page Unload
		' **************************

		Protected Sub Page_Unload(ByVal s As Object, ByVal e As System.EventArgs)
			Try
				Imagedal.CloseAndDisposeConnection()
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
			Dim messageList As ArrayList = ValidateInputValues(ImageTable, Core.PageType.Search)
			If (messageList IsNot Nothing) Then
				Dim sMsg As String = String.Empty 
				For Each sErrMsg As String In messageList
					sMsg += sErrMsg + "<br>" 
				Next
				lblMessage.Text = sMsg 
				pnlMessage.Visible = True 
				Return 
			End If
			Dim sUrl As String = LoadSearchUrl(ImageTable) 
			If (Not String.IsNullOrEmpty(sUrl)) Then
				objProfile.PageIndex = 0 ' reset page index
				sUrl &= "&Image_psearchtype=&Image_psearch="
				Response.Redirect (lblReturnUrl.Text + "?" + sUrl) 
			End If
		End Sub

	'*********************************
	'*  Load Default Data to Controls
	'*********************************

	Private Sub LoadData()
		Dim source As WebControl = TryCast(ImageTable, WebControl)
		Dim row As Imagerow = New Imagerow() ' dummy record for rendering
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
			Dim x_ImageName As TextBox= TryCast(control.FindControl("x_ImageName"), TextBox)
			If (x_ImageName IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_ImageName.Text)) Then
					messageList.Add("Invalid Value (String): Name")
				End If
			End If
			Dim x_ContactID As DropDownList= TryCast(control.FindControl("x_ContactID"), DropDownList)
			If (x_ContactID IsNot Nothing) Then
				If ((x_ContactID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_ContactID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Contact")
				End If
			End If
			Dim x_CompanyID As DropDownList= TryCast(control.FindControl("x_CompanyID"), DropDownList)
			If (x_CompanyID IsNot Nothing) Then
				If ((x_CompanyID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_CompanyID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Company")
				End If
			End If
			Dim x_title As TextBox= TryCast(control.FindControl("x_title"), TextBox)
			If (x_title IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_title.Text)) Then
					messageList.Add("Invalid Value (String): Title")
				End If
			End If
			Dim x_medium As TextBox= TryCast(control.FindControl("x_medium"), TextBox)
			If (x_medium IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_medium.Text)) Then
					messageList.Add("Invalid Value (String): Medium")
				End If
			End If
			Dim x_size As TextBox= TryCast(control.FindControl("x_size"), TextBox)
			If (x_size IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_size.Text)) Then
					messageList.Add("Invalid Value (String): Size")
				End If
			End If
			Dim x_price As TextBox= TryCast(control.FindControl("x_price"), TextBox)
			If (x_price IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_price.Text)) Then
					messageList.Add("Invalid Value (String): Price")
				End If
			End If
			Dim x_color As TextBox= TryCast(control.FindControl("x_color"), TextBox)
			If (x_color IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_color.Text)) Then
					messageList.Add("Invalid Value (String): Color")
				End If
			End If
			Dim x_subject As TextBox= TryCast(control.FindControl("x_subject"), TextBox)
			If (x_subject IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_subject.Text)) Then
					messageList.Add("Invalid Value (String): Subject")
				End If
			End If
		End If
		Return IIf(messageList.Count = 0, Nothing, messageList)
	End Function

	' ********************************************
	' *  Convert Row Object Value To Input Control
	' ********************************************

	Private Sub RowToControl(ByVal row As Imagerow, ByVal control As WebControl, ByVal ct As Core.CtrlType)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		If (ct = Core.CtrlType.Edit) Then ' set up edit control

			' Field ImageName
			' Field ContactID

			If (control.FindControl("ax_ContactID") IsNot Nothing) Then
				Dim cdd As CascadingDropDown = TryCast(control.FindControl("ax_ContactID"), CascadingDropDown)
				Dim v_ContactID As String
				If (row.ContactID.HasValue) Then v_ContactID = Convert.ToString(row.ContactID) Else v_ContactID = String.Empty
				cdd.SelectedValue = v_ContactID
			End If

			' Field CompanyID
			If (control.FindControl("ax_CompanyID") IsNot Nothing) Then
				Dim cdd As CascadingDropDown = TryCast(control.FindControl("ax_CompanyID"), CascadingDropDown)
				Dim v_CompanyID As String
				If (row.CompanyID.HasValue) Then v_CompanyID = Convert.ToString(row.CompanyID) Else v_CompanyID = String.Empty
				cdd.SelectedValue = v_CompanyID
			End If

			' Field title
			' Field medium
			' Field size
			' Field price
			' Field color
			' Field subject
			' Field sold

			Dim x_sold As CheckBox = TryCast(control.FindControl("x_sold"), CheckBox)
			Dim ssold As String = row.sold.ToString()
			If ((ssold IsNot Nothing) AndAlso ssold <> "") Then
				If (Convert.ToBoolean(ssold)) Then
					x_sold.Checked = True
				Else
					x_sold.Checked = False
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
		Dim x_ImageName As TextBox = TryCast(control.FindControl("x_ImageName"), TextBox)
		Dim z_ImageName As HiddenField = TryCast(control.FindControl("z_ImageName"), HiddenField)
		sSrchStr = x_ImageName.Text
		sSrchOpr = z_ImageName.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_ImageName", sSrchStr, "z_ImageName", sSrchOpr)
		End If
		Dim x_ContactID As DropDownList = TryCast(control.FindControl("x_ContactID"), DropDownList)
		Dim z_ContactID As HiddenField = TryCast(control.FindControl("z_ContactID"), HiddenField)
		sSrchStr = x_ContactID.SelectedValue
		sSrchOpr = z_ContactID.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_ContactID", sSrchStr, "z_ContactID", sSrchOpr)
		End If
		Dim x_CompanyID As DropDownList = TryCast(control.FindControl("x_CompanyID"), DropDownList)
		Dim z_CompanyID As HiddenField = TryCast(control.FindControl("z_CompanyID"), HiddenField)
		sSrchStr = x_CompanyID.SelectedValue
		sSrchOpr = z_CompanyID.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_CompanyID", sSrchStr, "z_CompanyID", sSrchOpr)
		End If
		Dim x_title As TextBox = TryCast(control.FindControl("x_title"), TextBox)
		Dim z_title As HiddenField = TryCast(control.FindControl("z_title"), HiddenField)
		sSrchStr = x_title.Text
		sSrchOpr = z_title.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_title", sSrchStr, "z_title", sSrchOpr)
		End If
		Dim x_medium As TextBox = TryCast(control.FindControl("x_medium"), TextBox)
		Dim z_medium As HiddenField = TryCast(control.FindControl("z_medium"), HiddenField)
		sSrchStr = x_medium.Text
		sSrchOpr = z_medium.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_medium", sSrchStr, "z_medium", sSrchOpr)
		End If
		Dim x_size As TextBox = TryCast(control.FindControl("x_size"), TextBox)
		Dim z_size As HiddenField = TryCast(control.FindControl("z_size"), HiddenField)
		sSrchStr = x_size.Text
		sSrchOpr = z_size.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_size", sSrchStr, "z_size", sSrchOpr)
		End If
		Dim x_price As TextBox = TryCast(control.FindControl("x_price"), TextBox)
		Dim z_price As HiddenField = TryCast(control.FindControl("z_price"), HiddenField)
		sSrchStr = x_price.Text
		sSrchOpr = z_price.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_price", sSrchStr, "z_price", sSrchOpr)
		End If
		Dim x_color As TextBox = TryCast(control.FindControl("x_color"), TextBox)
		Dim z_color As HiddenField = TryCast(control.FindControl("z_color"), HiddenField)
		sSrchStr = x_color.Text
		sSrchOpr = z_color.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_color", sSrchStr, "z_color", sSrchOpr)
		End If
		Dim x_subject As TextBox = TryCast(control.FindControl("x_subject"), TextBox)
		Dim z_subject As HiddenField = TryCast(control.FindControl("z_subject"), HiddenField)
		sSrchStr = x_subject.Text
		sSrchOpr = z_subject.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_subject", sSrchStr, "z_subject", sSrchOpr)
		End If
		Dim x_sold As CheckBox = TryCast(control.FindControl("x_sold"), CheckBox)
		Dim z_sold As HiddenField = TryCast(control.FindControl("z_sold"), HiddenField)
		sSrchStr = IIf(x_sold.Checked, "True", String.Empty)
		sSrchOpr = z_sold.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_sold", sSrchStr, "z_sold", sSrchOpr)
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
		Dim data As Imagedal = New Imagedal()

		' ImageName
		If (data.AdvancedSearchParm1.ImageName IsNot Nothing) Then
			Dim x_ImageName As TextBox = TryCast(control.FindControl("x_ImageName"), TextBox)
			x_ImageName.Text = Convert.ToString(data.AdvancedSearchParm1.ImageName)
		End If

		' ContactID
		If (control.FindControl("ax_ContactID") IsNot Nothing) Then
			Dim cdd As CascadingDropDown = TryCast(control.FindControl("ax_ContactID"), CascadingDropDown)
			Dim v_ContactID As String = Convert.ToString(data.AdvancedSearchParm1.ContactID)
			cdd.SelectedValue = v_ContactID
		End If

		' CompanyID
		If (control.FindControl("ax_CompanyID") IsNot Nothing) Then
			Dim cdd As CascadingDropDown = TryCast(control.FindControl("ax_CompanyID"), CascadingDropDown)
			Dim v_CompanyID As String = Convert.ToString(data.AdvancedSearchParm1.CompanyID)
			cdd.SelectedValue = v_CompanyID
		End If

		' title
		If (data.AdvancedSearchParm1.title IsNot Nothing) Then
			Dim x_title As TextBox = TryCast(control.FindControl("x_title"), TextBox)
			x_title.Text = Convert.ToString(data.AdvancedSearchParm1.title)
		End If

		' medium
		If (data.AdvancedSearchParm1.medium IsNot Nothing) Then
			Dim x_medium As TextBox = TryCast(control.FindControl("x_medium"), TextBox)
			x_medium.Text = Convert.ToString(data.AdvancedSearchParm1.medium)
		End If

		' size
		If (data.AdvancedSearchParm1.size IsNot Nothing) Then
			Dim x_size As TextBox = TryCast(control.FindControl("x_size"), TextBox)
			x_size.Text = Convert.ToString(data.AdvancedSearchParm1.size)
		End If

		' price
		If (data.AdvancedSearchParm1.price IsNot Nothing) Then
			Dim x_price As TextBox = TryCast(control.FindControl("x_price"), TextBox)
			x_price.Text = Convert.ToString(data.AdvancedSearchParm1.price)
		End If

		' color
		If (data.AdvancedSearchParm1.color IsNot Nothing) Then
			Dim x_color As TextBox = TryCast(control.FindControl("x_color"), TextBox)
			x_color.Text = Convert.ToString(data.AdvancedSearchParm1.color)
		End If

		' subject
		If (data.AdvancedSearchParm1.subject IsNot Nothing) Then
			Dim x_subject As TextBox = TryCast(control.FindControl("x_subject"), TextBox)
			x_subject.Text = Convert.ToString(data.AdvancedSearchParm1.subject)
		End If

		' sold
		Dim x_sold As CheckBox= TryCast(control.FindControl("x_sold"), CheckBox)
		Dim v_sold As String = Convert.ToString(data.AdvancedSearchParm1.sold)
		x_sold.Checked = (Not String.IsNullOrEmpty(v_sold) AndAlso Convert.ToBoolean(v_sold))
	End Sub

	' *******************
	' *  Get User Filter
	' *******************

	Private Function GetUserFilter() As String 
		Return Imageinf.GetUserFilter()
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
	<p><span class="aspnetmaker">Search TABLE: Image</span></p>
	<p>
		<asp:Hyperlink id="lnkList" Text="Back to List" NavigateUrl="image_list.aspx" CssClass="aspnetmaker" runat="server" />
		<asp:Label id="lblReturnUrl" Visible="False" CssClass="aspnetmaker" runat="server">image_list.aspx</asp:Label>
	</p>
	<script type='text/javascript'>
	function ew_ClearForm() {
		ew_ResetElement(ew.Controls['Image_srch_x_ImageName']);
		ew_ResetDropDownList(ew.Controls['Image_srch_x_ContactID']);
		ew_ResetDropDownList(ew.Controls['Image_srch_x_CompanyID']);
		ew_ResetElement(ew.Controls['Image_srch_x_title']);
		ew_ResetElement(ew.Controls['Image_srch_x_medium']);
		ew_ResetElement(ew.Controls['Image_srch_x_size']);
		ew_ResetElement(ew.Controls['Image_srch_x_price']);
		ew_ResetElement(ew.Controls['Image_srch_x_color']);
		ew_ResetElement(ew.Controls['Image_srch_x_subject']);
		ew_ResetCheckBox(ew.Controls['Image_srch_x_sold']);
	}
	</script>
	<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
	<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
	</asp:PlaceHolder>
	<asp:ValidationSummary id="xevs_Image" CssClass="aspnetmaker" runat="server"
		 HeaderText=""
		 ShowSummary="False"
		 ShowMessageBox="True"
		 ForeColor="#FF0000" />
	<asp:Table runat="server" CssClass="ewDetailTable" ID="ImageTable">
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_ImageName">Name</asp:Label></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">LIKE<asp:HiddenField id="z_ImageName" value="LIKE,'%,%'" runat="server" /></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">
<asp:TextBox id="x_ImageName" Columns="30" MaxLength="50" CssClass="aspnetmaker" runat="server" />
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
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_title">Title</asp:Label></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">LIKE<asp:HiddenField id="z_title" value="LIKE,'%,%'" runat="server" /></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">
<asp:TextBox id="x_title" Columns="30" MaxLength="50" CssClass="aspnetmaker" runat="server" />
</span>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_medium">Medium</asp:Label></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">LIKE<asp:HiddenField id="z_medium" value="LIKE,'%,%'" runat="server" /></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">
<asp:TextBox id="x_medium" Columns="30" MaxLength="50" CssClass="aspnetmaker" runat="server" />
</span>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_size">Size</asp:Label></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">LIKE<asp:HiddenField id="z_size" value="LIKE,'%,%'" runat="server" /></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">
<asp:TextBox id="x_size" Columns="30" MaxLength="50" CssClass="aspnetmaker" runat="server" />
</span>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_price">Price</asp:Label></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">LIKE<asp:HiddenField id="z_price" value="LIKE,'%,%'" runat="server" /></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">
<asp:TextBox id="x_price" Columns="30" MaxLength="255" CssClass="aspnetmaker" runat="server" />
</span>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_color">Color</asp:Label></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">LIKE<asp:HiddenField id="z_color" value="LIKE,'%,%'" runat="server" /></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">
<asp:TextBox id="x_color" Columns="30" MaxLength="255" CssClass="aspnetmaker" runat="server" />
</span>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_subject">Subject</asp:Label></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">LIKE<asp:HiddenField id="z_subject" value="LIKE,'%,%'" runat="server" /></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">
<asp:TextBox id="x_subject" Columns="30" MaxLength="255" CssClass="aspnetmaker" runat="server" />
</span>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_sold">Sold</asp:Label></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">=<asp:HiddenField id="z_sold" value="=,," runat="server" /></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">
<asp:CheckBox ID="x_sold" CssClass="aspnetmakerlist" runat="server" />
</span>
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
	<br />
	<asp:Button id="btnSearch" Text="Search" OnClick="btnSearch_Click" CssClass="aspnetmaker" runat="server" />
	<input type="button" id="btnReset" onclick="ew_ClearForm();" class="aspnetmaker" value="Reset"/>
</asp:Content>
