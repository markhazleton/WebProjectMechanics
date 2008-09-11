<%@ Page EnableViewState="true" Language="VB" ValidateRequest="False" Debug="True" MasterPageFile="masterpage.master" %>
<%@ MasterType VirtualPath="masterpage.master" %>
<%@ Import Namespace="PMGEN" %>
<%@ Import Namespace="EW.Data" %>
<%@ Import Namespace="EW.Security" %>
<%@ Import Namespace="EW.Data.Utilities" %>
<%@ Import Namespace="EW.Web" %>
<%@ Register NameSpace="PMGEN" TagPrefix="PMGEN" %>
<script runat="server">
	Dim sUserFilter As String = string.Empty
	Dim key As Contactkey ' record key
	Dim oldrow As Contactrow ' old record data input by user
	Dim newrow As Contactrow ' new record data input by user
	Dim objProfile As TableProfile ' table profile

	' *************************
	' *  Handler for Page Init
	' *************************

	Protected Sub Page_Init(ByVal s As Object, ByVal e As System.EventArgs)
		If Not mhUser.IsAdmin() Then
		  Session("Login_Link") = Request.RawUrl
		  Response.Redirect("/mhweb/login/login.aspx")
		End If
		objProfile = CustomProfile.GetTable(Share.ProjectName, Contactinf.TableVar)
	End Sub

	' *************************
	' *  Handler for Page Load
	' *************************

	Protected Sub Page_Load(ByVal s As Object, ByVal e As System.EventArgs)
		Page.ClientScript.RegisterClientScriptInclude("ewv", "ewv.js")
		Response.Cache.SetCacheability(HttpCacheability.NoCache)
		If (Not Page.IsPostBack) Then
		End If
		If (ContactDetailsView.Visible) Then
			RegisterClientID("CtrlID", ContactDetailsView)
		End If
			If (ContactDetailsView.FindControl("x_PrimaryContact") IsNot Nothing) Then
				Page.Form.DefaultFocus = ContactDetailsView.FindControl("x_PrimaryContact").ClientID
			End If
	End Sub

	' **********************************************
	' *  Load Copy Data & Default Data to Controls
	' **********************************************

	Private Sub LoadData()
		Dim source As WebControl = TryCast(ContactDetailsView, WebControl)
		If (oldrow IsNot Nothing) Then ' old record (from recovery) exists, just restore
			RowToControl(oldrow, source, Core.CtrlType.Edit)
			oldrow = Nothing
		ElseIf (key IsNot Nothing) Then ' key loaded, try to copy record
			Dim data As Contactdal = New Contactdal()
			Dim row As Contactrow = data.LoadRow(key, Contactinf.GetUserFilter())
			If (row Is Nothing) Then
				objProfile.Message = "Record not found"
				Response.Redirect(lblReturnUrl.Text)
			End If
			RowToControl(row, source, Core.CtrlType.Edit)
			row = Nothing
		Else 
			Dim row As Contactrow = New Contactrow()
			row.CompanyID = Convert.ToInt32(0) ' set default value
			row.Active = Convert.ToBoolean(true) ' set default value
			row.GroupID = Convert.ToInt32(0) ' set default value
			row.CreateDT = Convert.ToDateTime(DateTime.Now) ' set default value
			row.RoleID = Convert.ToInt32(1) ' set default value
			RowToControl(row, source, Core.CtrlType.Edit)
			row = Nothing
		End If
	End Sub

	' *****************************************
	' *  Handler when Cancel button is clicked
	' *****************************************

	Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
		Response.Redirect(lblReturnUrl.Text)
	End Sub

	' ******************************
	' *  Handler for DetailsView Init
	' ******************************

	Protected Sub ContactDetailsView_Init(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			Contactdal.OpenConnection()
		Catch err As Exception
			lblMessage.Text = err.Message
			pnlMessage.Visible = True
		End Try
	End Sub

	' *************************************
	' *  Handler for DetailsView DataBound
	' *************************************

	Protected Sub ContactDetailsView_DataBound(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim source As DetailsView = ContactDetailsView
		If (ContactDetailsView.CurrentMode = DetailsViewMode.Insert) Then
			LoadData()
		End If
	End Sub

	' *****************************************
	' *  Handler for DetailsView ItemInserting
	' *****************************************

	Protected Sub ContactDetailsView_ItemInserting(ByVal sender As Object, ByVal e As DetailsViewInsertEventArgs)
		Dim source As WebControl = TryCast(sender, WebControl)

		' Validate input values
		Dim messageList As ArrayList = ValidateInputValues(CType(sender, Control), Core.PageType.Add)
		If (messageList IsNot Nothing) Then
			lblMessage.Text = String.Empty
			For Each sMsg As String In messageList
				lblMessage.Text += sMsg + "<br>"
			Next
			pnlMessage.Visible = True
			e.Cancel = True
			Return
		End If
		messageList = CheckDuplicateKey(TryCast(sender,WebControl))
		If (messageList IsNot Nothing) Then
			lblMessage.Text = String.Empty
			For Each sMsg As string In messageList
				lblMessage.Text += sMsg + "<br>"
			Next
			pnlMessage.Visible = True
			e.Cancel = True
			Return
		End If
	End Sub

	' ****************************************
	' *  Handler for DetailsView ItemInserted
	' ****************************************

	Protected Sub ContactDetailsView_ItemInserted(ByVal sender As Object, ByVal e As DetailsViewInsertedEventArgs)
		If (e.AffectedRows = 0) Then

			' Keep in insert mode
			e.KeepInInsertMode = True

			' Get original values
			Dim source As WebControl = TryCast(sender, WebControl)
			If (source Is Nothing) Then Return
			oldrow = New Contactrow()
			ControlToRow(oldrow, source)
		Else
			objProfile.Message = "Add new record successful"
			Response.Redirect(lblReturnUrl.Text)
		End If
	End Sub

	' *********************************
	' *  Handler for DetailsView Unload
	' *********************************

	Protected Sub ContactDetailsView_Unload(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			Contactdal.CloseAndDisposeConnection()
		Catch
		End Try
	End Sub

	' ********************************
	' * Register ClientID of Controls
	' ********************************

	Private Sub RegisterClientID(ByVal sBlockName As String, ByVal control As Control)
		Dim jsString As String = String.Empty
		If (control.FindControl("x_PrimaryContact") IsNot Nothing) Then
			jsString &= "ew.Controls['x_PrimaryContact'] = '" & control.FindControl("x_PrimaryContact").ClientID & "';"
		End If
		If (control.FindControl("x_LogonName") IsNot Nothing) Then
			jsString &= "ew.Controls['x_LogonName'] = '" & control.FindControl("x_LogonName").ClientID & "';"
		End If
		If (control.FindControl("x_LogonPassword") IsNot Nothing) Then
			jsString &= "ew.Controls['x_LogonPassword'] = '" & control.FindControl("x_LogonPassword").ClientID & "';"
		End If
		If (control.FindControl("x_EMail") IsNot Nothing) Then
			jsString &= "ew.Controls['x_EMail'] = '" & control.FindControl("x_EMail").ClientID & "';"
		End If
		If (control.FindControl("x_CompanyID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_CompanyID'] = '" & control.FindControl("x_CompanyID").ClientID & "';"
		End If
		If (control.FindControl("x_Active") IsNot Nothing) Then
			jsString &= "ew.Controls['x_Active'] = '" & control.FindControl("x_Active").ClientID & "';"
		End If
		If (control.FindControl("x_GroupID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_GroupID'] = '" & control.FindControl("x_GroupID").ClientID & "';"
		End If
		If (control.FindControl("x_CreateDT") IsNot Nothing) Then
			jsString &= "ew.Controls['x_CreateDT'] = '" & control.FindControl("x_CreateDT").ClientID & "';"
		End If
		If (control.FindControl("x_TemplatePrefix") IsNot Nothing) Then
			jsString &= "ew.Controls['x_TemplatePrefix'] = '" & control.FindControl("x_TemplatePrefix").ClientID & "';"
		End If
		If (control.FindControl("x_RoleID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_RoleID'] = '" & control.FindControl("x_RoleID").ClientID & "';"
		End If
		Page.ClientScript.RegisterClientScriptBlock(System.Type.GetType("System.String"), sBlockName, jsString, True)
	End Sub

	' *****************************************************
	' *  Processing Input Values Before Updating/Inserting
	' *****************************************************

	Private Function ValidateInputValues(ByVal control As Control, ByVal pageType As Core.PageType) As ArrayList
		Dim messageList As ArrayList = New ArrayList()
		DataFormat.SetDateSeparator("/")
		If (pageType <> Core.PageType.Search) Then ' Add/Edit Validation
			Dim x_PrimaryContact As TextBox = TryCast(control.FindControl("x_PrimaryContact"), TextBox)
			If (x_PrimaryContact IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_PrimaryContact.Text)) Then
					messageList.Add("Invalid Value (String): Primary Contact")
				End If
			End If
			Dim x_LogonName As TextBox = TryCast(control.FindControl("x_LogonName"), TextBox)
			If (x_LogonName IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_LogonName.Text)) Then
					messageList.Add("Invalid Value (String): Logon Name")
				ElseIf (String.IsNullOrEmpty(x_LogonName.Text)) Then
					messageList.Add("Please enter required field (String): Logon Name")
				End If
			End If
			Dim x_LogonPassword As TextBox = TryCast(control.FindControl("x_LogonPassword"), TextBox)
			If (x_LogonPassword IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_LogonPassword.Text)) Then
					messageList.Add("Invalid Value (String): Logon Password")
				End If
			End If
			Dim x_EMail As TextBox = TryCast(control.FindControl("x_EMail"), TextBox)
			If (x_EMail IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_EMail.Text)) Then
					messageList.Add("Invalid Value (String): EMail")
				End If
			End If
			Dim x_CompanyID As DropDownList = TryCast(control.FindControl("x_CompanyID"), DropDownList)
			If (x_CompanyID IsNot Nothing) Then
				If ((x_CompanyID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_CompanyID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Company")
				End If
			End If
			Dim x_GroupID As DropDownList = TryCast(control.FindControl("x_GroupID"), DropDownList)
			If (x_GroupID IsNot Nothing) Then
				If ((x_GroupID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_GroupID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Group")
				End If
			End If
			Dim x_CreateDT As TextBox = TryCast(control.FindControl("x_CreateDT"), TextBox)
			If (x_CreateDT IsNot Nothing) Then
				If (Not DataFormat.CheckDateTime(x_CreateDT.Text)) Then
					messageList.Add("Invalid Value (DateTime): Modified")
				End If
			End If
			Dim x_TemplatePrefix As DropDownList = TryCast(control.FindControl("x_TemplatePrefix"), DropDownList)
			If (x_TemplatePrefix IsNot Nothing) Then
				If ((x_TemplatePrefix.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckString(x_TemplatePrefix.SelectedValue))) Then
					messageList.Add("Invalid Value (String): Template")
				End If
			End If
			Dim x_RoleID As DropDownList = TryCast(control.FindControl("x_RoleID"), DropDownList)
			If (x_RoleID IsNot Nothing) Then
				If ((x_RoleID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_RoleID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Role ID")
				End If
			End If
		End If
		Return IIf(messageList.Count = 0, Nothing, messageList)
	End Function

	' **********************************************
	' *  Convert Input Control Values to Row Object
	' **********************************************

	Private Sub ControlToRow(ByVal row As Contactrow, ByVal control As WebControl)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		row.SetPageType(Core.PageType.Add) ' set page type = list for update

		' Field PrimaryContact
		Dim x_PrimaryContact As TextBox = TryCast(control.FindControl("x_PrimaryContact"), TextBox)
		If (x_PrimaryContact.Text <> String.Empty) Then row.PrimaryContact = x_PrimaryContact.Text Else row.PrimaryContact = Nothing

		' Field LogonName
		Dim x_LogonName As TextBox = TryCast(control.FindControl("x_LogonName"), TextBox)
		If (x_LogonName.Text <> String.Empty) Then row.LogonName = x_LogonName.Text Else row.LogonName = String.Empty

		' Field LogonPassword
		Dim x_LogonPassword As TextBox = TryCast(control.FindControl("x_LogonPassword"), TextBox)
		If (x_LogonPassword.Text <> String.Empty) Then row.LogonPassword = x_LogonPassword.Text Else row.LogonPassword = Nothing

		' Field EMail
		Dim x_EMail As TextBox = TryCast(control.FindControl("x_EMail"), TextBox)
		If (x_EMail.Text <> String.Empty) Then row.EMail = x_EMail.Text Else row.EMail = Nothing

		' Field CompanyID
		Dim x_CompanyID As DropDownList = TryCast(control.FindControl("x_CompanyID"), DropDownList)
		Dim v_CompanyID As String = String.Empty
		If (x_CompanyID.SelectedIndex >= 0) Then
			For Each li As ListItem In x_CompanyID.Items
				If (li.Selected) Then
					v_CompanyID += li.Value
					Exit For
				End If
			Next
		End If
		If (v_CompanyID <> String.Empty) Then row.CompanyID = Convert.ToInt32(v_CompanyID) Else row.CompanyID = CType(Nothing, Nullable(Of Int32))

		' Field Active
		Dim x_Active As CheckBox = TryCast(control.FindControl("x_Active"), CheckBox)
		If (x_Active.Checked) Then
			row.Active = True
		Else
			row.Active = False
		End If

		' Field GroupID
		Dim x_GroupID As DropDownList = TryCast(control.FindControl("x_GroupID"), DropDownList)
		Dim v_GroupID As String = String.Empty
		If (x_GroupID.SelectedIndex >= 0) Then
			For Each li As ListItem In x_GroupID.Items
				If (li.Selected) Then
					v_GroupID += li.Value
					Exit For
				End If
			Next
		End If
		If (v_GroupID <> String.Empty) Then row.GroupID = Convert.ToInt32(v_GroupID) Else row.GroupID = CType(Nothing, Nullable(Of Int32))

		' Field CreateDT
		Dim x_CreateDT As TextBox = TryCast(control.FindControl("x_CreateDT"), TextBox)
		If (x_CreateDT.Text <> String.Empty) Then row.CreateDT = Convert.ToDateTime(DataFormat.UnFormatDateTime(x_CreateDT.Text, 6, "/"c)) Else row.CreateDT = CType(Nothing, Nullable(Of DateTime))

		' Field TemplatePrefix
		Dim x_TemplatePrefix As DropDownList = TryCast(control.FindControl("x_TemplatePrefix"), DropDownList)
		Dim v_TemplatePrefix As String = String.Empty
		If (x_TemplatePrefix.SelectedIndex >= 0) Then
			For Each li As ListItem In x_TemplatePrefix.Items
				If (li.Selected) Then
					v_TemplatePrefix += li.Value
					Exit For
				End If
			Next
		End If
		If (v_TemplatePrefix <> String.Empty) Then row.TemplatePrefix = v_TemplatePrefix Else row.TemplatePrefix = String.Empty

		' Field RoleID
		Dim x_RoleID As DropDownList = TryCast(control.FindControl("x_RoleID"), DropDownList)
		Dim v_RoleID As String = String.Empty
		If (x_RoleID.SelectedIndex >= 0) Then
			For Each li As ListItem In x_RoleID.Items
				If (li.Selected) Then
					v_RoleID += li.Value
					Exit For
				End If
			Next
		End If
		If (v_RoleID <> String.Empty) Then row.RoleID = Convert.ToInt32(v_RoleID) Else row.RoleID = CType(Nothing, Nullable(Of Int32))
	End Sub

	' ****************************
	' *  Check for duplicate Key
	' ****************************

	Private Function CheckDuplicateKey(ByVal control As WebControl) As ArrayList 
		Dim sWhere As String = String.Empty
		Dim messageList As ArrayList = New ArrayList()
		Dim data As Contactdal = New Contactdal()
		Dim newkey As Contactkey = New Contactkey()
		Try

		' Check for duplicate Logon Name
			sWhere = "([LogonName] = '" + Db.AdjustSql((CType(control.FindControl("x_LogonName"),TextBox)).Text) + "')"
			If (data.LoadList(sWhere) IsNot Nothing) Then
				messageList.Add("Duplicate value: Logon Name")
			End If
		data = Nothing
		Catch e As Exception
			messageList.Add(e.Message)
		End Try
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
			Dim x_PrimaryContact As TextBox = TryCast(control.FindControl("x_PrimaryContact"), TextBox)
			If (row.PrimaryContact IsNot Nothing) Then x_PrimaryContact.Text = row.PrimaryContact.ToString() Else x_PrimaryContact.Text = String.Empty

			' Field LogonName
			Dim x_LogonName As TextBox = TryCast(control.FindControl("x_LogonName"), TextBox)
			If (row.LogonName IsNot Nothing) Then x_LogonName.Text = row.LogonName.ToString() Else x_LogonName.Text = String.Empty

			' Field LogonPassword
			Dim x_LogonPassword As TextBox = TryCast(control.FindControl("x_LogonPassword"), TextBox)
			If (row.LogonPassword IsNot Nothing) Then x_LogonPassword.Text = row.LogonPassword.ToString() Else x_LogonPassword.Text = String.Empty

			' Field EMail
			Dim x_EMail As TextBox = TryCast(control.FindControl("x_EMail"), TextBox)
			If (row.EMail IsNot Nothing) Then x_EMail.Text = row.EMail.ToString() Else x_EMail.Text = String.Empty

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
			x_CompanyID.ClearSelection()
			For Each li As ListItem In x_CompanyID.Items
				If (li.Value.ToString() = v_CompanyID) Then
					li.Selected = True
					Exit For
				End If
			Next

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
			x_GroupID.ClearSelection()
			For Each li As ListItem In x_GroupID.Items
				If (li.Value.ToString() = v_GroupID) Then
					li.Selected = True
					Exit For
				End If
			Next

			' Field CreateDT
			Dim x_CreateDT As TextBox = TryCast(control.FindControl("x_CreateDT"), TextBox)
			If (row.CreateDT.HasValue) Then x_CreateDT.Text = DataFormat.DateTimeFormat(6, "/", row.CreateDT) Else x_CreateDT.Text = String.Empty

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
			x_TemplatePrefix.ClearSelection()
			For Each li As ListItem In x_TemplatePrefix.Items
				If (li.Value.ToString() = v_TemplatePrefix) Then
					li.Selected = True
					Exit For
				End If
			Next

			' Field RoleID
			Dim x_RoleID As DropDownList = TryCast(control.FindControl("x_RoleID"), DropDownList)
			x_RoleID.DataValueField = "ewValueField"
			x_RoleID.DataTextField = "ewTextField"
			Dim dv_x_RoleID As DataView = Nothing ' NOTE: to improve performance, get lookup value only once
			If (dv_x_RoleID Is Nothing) Then dv_x_RoleID = Contactdal.LookUpTable("RoleID")
			x_RoleID.DataSource = dv_x_RoleID
			x_RoleID.DataBind()
			x_RoleID.Items.Insert(0, New ListItem("Please Select", ""))
			Dim v_RoleID As String
			If (row.RoleID.HasValue) Then v_RoleID = Convert.ToString(row.RoleID) Else v_RoleID = String.Empty
			x_RoleID.ClearSelection()
			For Each li As ListItem In x_RoleID.Items
				If (li.Value.ToString() = v_RoleID) Then
					li.Selected = True
					Exit For
				End If
			Next
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

	' ************************************
	' *  Handler for DataSource Inserting
	' ************************************

	Protected Sub ContactDataSource_Inserting(ByVal sender As Object, ByVal e As ObjectDataSourceMethodEventArgs)
		Dim control As WebControl = ContactDetailsView

		' Set up row object
		Dim row As Contactrow = TryCast(e.InputParameters(0), Contactrow)
		ControlToRow(row, control)
		If (Contactbll.Inserting(row)) Then
		Else
			e.Cancel = True
			If (Not String.IsNullOrEmpty(objProfile.Message)) Then
				lblMessage.Text = objProfile.Message
				pnlMessage.Visible = True
			End If
		End If
	End Sub

	' ***********************************
	' *  Handler for DataSource Inserted
	' ***********************************

	Protected Sub ContactDataSource_Inserted(ByVal sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.Exception IsNot Nothing) Then 
			e.AffectedRows = 0
			e.ExceptionHandled = True
			lblMessage.Text = e.Exception.InnerException.Message
			pnlMessage.Visible = True
		Else
			newrow = TryCast(e.ReturnValue, Contactrow) ' get new row objectinsert method
			Contactbll.Inserted(newrow)
		End If
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
	<p><span class="aspnetmaker">Add to TABLE: Contact</span></p>
	<p>
		<asp:Hyperlink id="lnkList" Text="Back to List" NavigateUrl="contact_list.aspx" CssClass="aspnetmaker" runat="server" />
		<asp:Label id="lblReturnUrl" Visible="False" CssClass="aspnetmaker" runat="server">contact_list.aspx</asp:Label>
	</p>
	<asp:ValidationSummary id="xevs_Contact" CssClass="aspnetmaker" runat="server"
	HeaderText=""
	ShowSummary="False"
	ShowMessageBox="True"
	ForeColor="#FF0000" />
	<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
		<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
	</asp:PlaceHolder>
	<!-- Data Source -->
<asp:ObjectDataSource ID="ContactDataSource"
	TypeName="PMGEN.Contactdal"
	DataObjectTypeName="PMGEN.Contactrow"
	InsertMethod="Insert"
	OnInserting="ContactDataSource_Inserting"
	OnInserted="ContactDataSource_Inserted"
	runat="server">
</asp:ObjectDataSource>
	<asp:DetailsView ID="ContactDetailsView"
		DataKeyNames="ContactID"
		DataSourceID="ContactDataSource"
		DefaultMode="Insert"
		GridLines="None"
		AutoGenerateRows="False" CssClass="ewDetailTable"
		OnInit="ContactDetailsView_Init"
		OnDataBound="ContactDetailsView_DataBound"
		OnItemInserting="ContactDetailsView_ItemInserting"
		OnItemInserted="ContactDetailsView_ItemInserted"
		OnUnload="ContactDetailsView_Unload"
		AllowPaging="True"
		PagerSettings-Mode="NextPreviousFirstLast"
		PagerSettings-Position="Top"
		runat="server">
		<RowStyle CssClass="ewTableRow" />
		<AlternatingRowStyle CssClass="ewTableAltRow" />
		<EditRowStyle />
		<FooterStyle CssClass="ewTableFooter" />
		<PagerStyle CssClass="ewTablePager" />
		<Fields>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_PrimaryContact">Primary Contact</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_PrimaryContact" Columns="30" MaxLength="255" CssClass="aspnetmaker" runat="server" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_LogonName">Logon Name<span class='ewmsg'>&nbsp;*</span></asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_LogonName" Columns="30" MaxLength="50" CssClass="aspnetmaker" runat="server" />
<asp:RequiredFieldValidator ID="vx_LogonName" CssClass="aspnetmaker" runat="server" ControlToValidate="x_LogonName" ErrorMessage="Please enter required field - Logon Name" ForeColor="Red" Display="None" Text="*" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_LogonPassword">Logon Password</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_LogonPassword" Columns="30" MaxLength="50" CssClass="aspnetmaker" runat="server" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_EMail">EMail</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_EMail" Columns="30" MaxLength="50" CssClass="aspnetmaker" runat="server" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_CompanyID">Company</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:DropDownList ID="x_CompanyID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_Active">Active</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:CheckBox ID="x_Active" CssClass="aspnetmakerlist" runat="server" />
<asp:HiddenField ID="hx_Active" Runat="server" Value='<%# Bind("Active") %>' />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_GroupID">Group</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:DropDownList ID="x_GroupID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_CreateDT">Modified</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:TextBox id="x_CreateDT" CssClass="aspnetmaker" runat="server" />
<ew:USDateValidator ID="vc_CreateDT" ClientValidationFunction="ew_CheckUSDate" CssClass="aspnetmaker" runat="server" ControlToValidate="x_CreateDT" ErrorMessage="Incorrect date, format = mm/dd/yyyy - Modified" Display="None" ForeColor="Red" DateSeparator="/" />
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_TemplatePrefix">Template</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:DropDownList ID="x_TemplatePrefix" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle CssClass="ewTableHeader" />
				<HeaderTemplate>
					<asp:Label runat="server" id="xs_RoleID">Role ID</asp:Label>
				</HeaderTemplate>
				<InsertItemTemplate>
<asp:DropDownList ID="x_RoleID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
				</InsertItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField ShowHeader="False">
				<ItemStyle CssClass="ewTableFooter" />
				<InsertItemTemplate>
					<table border="0">
						<tr>
							<td><asp:LinkButton ID="btnInsert" runat="server" CausesValidation="True" CommandName="Insert" Text="ADD"></asp:LinkButton></td>
							<td><asp:LinkButton ID="btnCancel" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel" OnClick="btnCancel_Click"></asp:LinkButton></td>
						</tr>
					</table>
					<asp:HiddenField ID="k_ContactID" Runat="server" Value='<%# Bind("ContactID") %>' />
				</InsertItemTemplate>
			</asp:TemplateField>
		</Fields>
	</asp:DetailsView>
</asp:Content>
