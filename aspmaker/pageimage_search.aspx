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
			objProfile = CustomProfile.GetTable(Share.ProjectName, PageImageinf.TableVar)
		End Sub

		' *************************
		' *  Handler for Page Load
		' *************************

		Protected Sub Page_Load(ByVal s As Object, ByVal e As System.EventArgs)
			Response.Cache.SetCacheability(HttpCacheability.NoCache)
			Page.ClientScript.RegisterClientScriptInclude("ewv", "ewv.js")
			If (Not Page.IsPostBack) Then
				LoadData() ' Initialize Controls
				LoadSearchControls(PageImageTable)
			End If
			Dim jsString As String = String.Empty
			jsString &= "if (!ew) { ew = new Object(); ew.Controls = new Array(); }" & vbCrLf
			jsString &= "ew.Controls['PageImage_srch_x_PageID'] = '" & x_PageID.ClientID & "'" & vbCrLf
			jsString &= "ew.Controls['PageImage_srch_x_ImageID'] = '" & x_ImageID.ClientID & "'" & vbCrLf
			jsString &= "ew.Controls['PageImage_srch_x_PageImagePosition'] = '" & x_PageImagePosition.ClientID & "'" & vbCrLf
			Page.ClientScript.RegisterClientScriptBlock(System.Type.GetType("System.String"), "CtrlID", jsString, True)
			Page.Form.DefaultFocus = x_PageID.ClientID
			Try
				PageImagedal.OpenConnection()
			Catch
			End Try
		End Sub

		' **************************
		' *  Handler for Page Unload
		' **************************

		Protected Sub Page_Unload(ByVal s As Object, ByVal e As System.EventArgs)
			Try
				PageImagedal.CloseAndDisposeConnection()
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
			Dim messageList As ArrayList = ValidateInputValues(PageImageTable, Core.PageType.Search)
			If (messageList IsNot Nothing) Then
				Dim sMsg As String = String.Empty 
				For Each sErrMsg As String In messageList
					sMsg += sErrMsg + "<br>" 
				Next
				lblMessage.Text = sMsg 
				pnlMessage.Visible = True 
				Return 
			End If
			Dim sUrl As String = LoadSearchUrl(PageImageTable) 
			If (Not String.IsNullOrEmpty(sUrl)) Then
				objProfile.PageIndex = 0 ' reset page index
				sUrl &= "&PageImage_psearchtype=&PageImage_psearch="
				Response.Redirect (lblReturnUrl.Text + "?" + sUrl) 
			End If
		End Sub

	'*********************************
	'*  Load Default Data to Controls
	'*********************************

	Private Sub LoadData()
		Dim source As WebControl = TryCast(PageImageTable, WebControl)
		Dim row As PageImagerow = New PageImagerow() ' dummy record for rendering
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
			Dim x_PageID As DropDownList= TryCast(control.FindControl("x_PageID"), DropDownList)
			If (x_PageID IsNot Nothing) Then
				If ((x_PageID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_PageID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Page")
				End If
			End If
			Dim x_ImageID As DropDownList= TryCast(control.FindControl("x_ImageID"), DropDownList)
			If (x_ImageID IsNot Nothing) Then
				If ((x_ImageID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_ImageID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Image")
				End If
			End If
			Dim x_PageImagePosition As TextBox= TryCast(control.FindControl("x_PageImagePosition"), TextBox)
			If (x_PageImagePosition IsNot Nothing) Then
				If (Not DataFormat.CheckInt32(x_PageImagePosition.Text)) Then
					messageList.Add("Invalid Value (Int32): Position")
				End If
			End If
		End If
		Return IIf(messageList.Count = 0, Nothing, messageList)
	End Function

	' ********************************************
	' *  Convert Row Object Value To Input Control
	' ********************************************

	Private Sub RowToControl(ByVal row As PageImagerow, ByVal control As WebControl, ByVal ct As Core.CtrlType)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		If (ct = Core.CtrlType.Edit) Then ' set up edit control

			' Field PageID
			Dim x_PageID As DropDownList = TryCast(control.FindControl("x_PageID"), DropDownList)
			x_PageID.DataValueField = "ewValueField"
			x_PageID.DataTextField = "ewTextField"
			Dim dv_x_PageID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
			If (dv_x_PageID Is Nothing) Then dv_x_PageID = PageImagedal.LookUpTable("PageID")
			x_PageID.DataSource = dv_x_PageID
			x_PageID.DataBind()
			x_PageID.Items.Insert(0, New ListItem("Please Select", ""))
			Dim v_PageID As String
			If (row.PageID.HasValue) Then v_PageID = Convert.ToString(row.PageID) Else v_PageID = String.Empty

			' Field ImageID
			Dim x_ImageID As DropDownList = TryCast(control.FindControl("x_ImageID"), DropDownList)
			x_ImageID.DataValueField = "ewValueField"
			x_ImageID.DataTextField = "ewTextField"
			Dim dv_x_ImageID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
			If (dv_x_ImageID Is Nothing) Then dv_x_ImageID = PageImagedal.LookUpTable("ImageID")
			x_ImageID.DataSource = dv_x_ImageID
			x_ImageID.DataBind()
			x_ImageID.Items.Insert(0, New ListItem("Please Select", ""))
			Dim v_ImageID As String
			If (row.ImageID.HasValue) Then v_ImageID = Convert.ToString(row.ImageID) Else v_ImageID = String.Empty

			' Field PageImagePosition
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
		Dim x_PageID As DropDownList = TryCast(control.FindControl("x_PageID"), DropDownList)
		Dim z_PageID As HiddenField = TryCast(control.FindControl("z_PageID"), HiddenField)
		sSrchStr = x_PageID.SelectedValue
		sSrchOpr = z_PageID.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_PageID", sSrchStr, "z_PageID", sSrchOpr)
		End If
		Dim x_ImageID As DropDownList = TryCast(control.FindControl("x_ImageID"), DropDownList)
		Dim z_ImageID As HiddenField = TryCast(control.FindControl("z_ImageID"), HiddenField)
		sSrchStr = x_ImageID.SelectedValue
		sSrchOpr = z_ImageID.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_ImageID", sSrchStr, "z_ImageID", sSrchOpr)
		End If
		Dim x_PageImagePosition As TextBox = TryCast(control.FindControl("x_PageImagePosition"), TextBox)
		Dim z_PageImagePosition As HiddenField = TryCast(control.FindControl("z_PageImagePosition"), HiddenField)
		sSrchStr = x_PageImagePosition.Text
		sSrchOpr = z_PageImagePosition.Value
		If (Not String.IsNullOrEmpty(sSrchStr)) Then
			sUrl += BuildSearchParm("x_PageImagePosition", sSrchStr, "z_PageImagePosition", sSrchOpr)
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
		Dim data As PageImagedal = New PageImagedal()

		' PageID
		Dim x_PageID As DropDownList = TryCast(control.FindControl("x_PageID"), DropDownList)
		x_PageID.DataValueField = "ewValueField"
		x_PageID.DataTextField = "ewTextField"
		Dim dv_x_PageID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
		if (dv_x_PageID Is Nothing) Then dv_x_PageID = PageImagedal.LookUpTable("PageID")
		x_PageID.DataSource = dv_x_PageID
		x_PageID.DataBind()
		x_PageID.Items.Insert(0, new ListItem("Please Select", ""))
		Dim v_PageID As String = Convert.ToString(data.AdvancedSearchParm1.PageID)
		x_PageID.ClearSelection()
		For Each li As ListItem In x_PageID.Items
			If (li.Value.ToString() = v_PageID) Then
				li.Selected = True
				Exit For
			End If
		Next

		' ImageID
		Dim x_ImageID As DropDownList = TryCast(control.FindControl("x_ImageID"), DropDownList)
		x_ImageID.DataValueField = "ewValueField"
		x_ImageID.DataTextField = "ewTextField"
		Dim dv_x_ImageID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
		if (dv_x_ImageID Is Nothing) Then dv_x_ImageID = PageImagedal.LookUpTable("ImageID")
		x_ImageID.DataSource = dv_x_ImageID
		x_ImageID.DataBind()
		x_ImageID.Items.Insert(0, new ListItem("Please Select", ""))
		Dim v_ImageID As String = Convert.ToString(data.AdvancedSearchParm1.ImageID)
		x_ImageID.ClearSelection()
		For Each li As ListItem In x_ImageID.Items
			If (li.Value.ToString() = v_ImageID) Then
				li.Selected = True
				Exit For
			End If
		Next

		' PageImagePosition
		If (data.AdvancedSearchParm1.PageImagePosition.HasValue) Then
			Dim x_PageImagePosition As TextBox = TryCast(control.FindControl("x_PageImagePosition"), TextBox)
			x_PageImagePosition.Text = Convert.ToString(data.AdvancedSearchParm1.PageImagePosition)
		End If
	End Sub

	' *******************
	' *  Get User Filter
	' *******************

	Private Function GetUserFilter() As String 
		Return PageImageinf.GetUserFilter()
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
	<p><span class="aspnetmaker">Search TABLE: Page Image</span></p>
	<p>
		<asp:Hyperlink id="lnkList" Text="Back to List" NavigateUrl="pageimage_list.aspx" CssClass="aspnetmaker" runat="server" />
		<asp:Label id="lblReturnUrl" Visible="False" CssClass="aspnetmaker" runat="server">pageimage_list.aspx</asp:Label>
	</p>
	<script type='text/javascript'>
	function ew_ClearForm() {
		ew_ResetDropDownList(ew.Controls['PageImage_srch_x_PageID']);
		ew_ResetDropDownList(ew.Controls['PageImage_srch_x_ImageID']);
		ew_ResetElement(ew.Controls['PageImage_srch_x_PageImagePosition']);
	}
	</script>
	<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
	<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
	</asp:PlaceHolder>
	<asp:ValidationSummary id="xevs_PageImage" CssClass="aspnetmaker" runat="server"
		 HeaderText=""
		 ShowSummary="False"
		 ShowMessageBox="True"
		 ForeColor="#FF0000" />
	<asp:Table runat="server" CssClass="ewDetailTable" ID="PageImageTable">
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
</span>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_ImageID">Image</asp:Label></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">=<asp:HiddenField id="z_ImageID" value="=,," runat="server" /></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">
<asp:DropDownList ID="x_ImageID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
</span>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="ewTableHeader">
			<span class="aspnetmaker" style="color: #FFFFFF;"><asp:Label runat="server" id="xss_PageImagePosition">Position</asp:Label></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">=<asp:HiddenField id="z_PageImagePosition" value="=,," runat="server" /></span>
			</asp:TableCell>
			<asp:TableCell CssClass="ewTableRow">
			<span class="aspnetmaker">
<asp:TextBox id="x_PageImagePosition" Columns="30" CssClass="aspnetmaker" runat="server" />
<ew:IntegerValidator ClientValidationFunction="ew_CheckInteger" CssClass="aspnetmaker" runat="server" ControlToValidate="x_PageImagePosition" ErrorMessage="Incorrect integer - Position" Display="None" ForeColor="Red" />
</span>
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
	<br />
	<asp:Button id="btnSearch" Text="Search" OnClick="btnSearch_Click" CssClass="aspnetmaker" runat="server" />
	<input type="button" id="btnReset" onclick="ew_ClearForm();" class="aspnetmaker" value="Reset"/>
</asp:Content>
