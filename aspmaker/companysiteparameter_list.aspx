<%@ Page EnableViewState="true" Language="VB" ValidateRequest="False" Debug="True" MasterPageFile="masterpage.master" %>
<%@ MasterType VirtualPath="masterpage.master" %>
<%@ Import Namespace="PMGEN" %>
<%@ Register NameSpace="PMGEN" TagPrefix="PMGEN" %>
<script runat="server">

	' ****************************
	' *  Handler for Page PreInit
	' ****************************

	Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim sExport As String = Request.QueryString("Export")
		Dim bDisableTheme As Boolean = (Not String.IsNullOrEmpty(sExport)) AndAlso (sExport <> "html")
		Master.SetExport(Not String.IsNullOrEmpty(sExport))
		If (Not Page.IsPostBack) Then
			If (bDisableTheme) Then
				Page.Theme = String.Empty
			Else
				Response.Cache.SetCacheability(HttpCacheability.NoCache)
			End If
		Else
			Response.Cache.SetCacheability(HttpCacheability.NoCache)
		End If
	End Sub
	Private sKey As String = String.Empty 'Inline edit key String
	Dim key As CompanySiteParameterkey = New CompanySiteParameterkey()  ' record key
	Dim oldrow As CompanySiteParameterrow ' old record data input by user
	Dim newrow As CompanySiteParameterrow ' new record data input by user
	Dim objProfile As TableProfile = Nothing ' table profile
	Dim arrKeys As ArrayList = New ArrayList()
	Protected initialFirstRowOffset As Integer = 1
	Protected initialLastRowOffset As Integer = 0
	Protected firstRowOffset As Integer = 1
	Protected lastRowOffset As Integer = 0
	Protected tableId As String = String.Empty

	' *************************
	' *  Handler for Page Init
	' *************************

	Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs)
		objProfile = CustomProfile.GetTable(Share.ProjectName, CompanySiteParameterinf.TableVar)
	End Sub

	' *************************
	' *  Handler for Page Load
	' *************************

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
                Session("ListPageURL") = HttpContext.Current.Request.FilePath()
		lblMessage.Text = String.Empty
		Dim isAuth As Boolean = False
		Page.MaintainScrollPositionOnPostBack = False
		tableId = CompanySiteParameterGridView.ClientID
		If (objProfile.Message <> String.Empty) Then
			lblMessage.Text = objProfile.Message 
			pnlMessage.Visible = True
			objProfile.Message = String.Empty 
		End If
		Page.ClientScript.RegisterClientScriptInclude("ewv", "ewv.js")
		If (Not Page.IsPostBack) Then
			Dim sExport As String = Request.QueryString("Export")
			If (Not String.IsNullOrEmpty(sExport)) Then
				Export_Command(sExport)
			Else
				AdvancedSearchRtn()
				Dim sCmd As String = Request.QueryString("cmd")
				If (sCmd = "resetall") Then
					objProfile.AllowSorting = True
					Dim data As CompanySiteParameterdal = New CompanySiteParameterdal()
					data.ClearSearch()
					objProfile.PageIndex = 0
					objProfile.PageSize = 50

					' Detail GridView Default Visibility
					objProfile.MasterKey = Nothing
					objProfile.IsCollapsed = False
				End If
				If (sCmd = "resetsort") Then
					ClearSort()
				End If
				If (objProfile.PageSize < 0) Then
					objProfile.PageSize = 50
				End If
				objProfile.AllowPaging = (objProfile.PageSize <> 0 AndAlso True)
				If (objProfile.OrderBy = String.Empty) Then
					SetupDefaultOrderBy()
				End If
				SelectedItem((TryCast(pnlPager.FindControl("RecPerPage"),DropDownList)), objProfile.PageSize)
				BindData()
			End If
		End If
	End Sub

	' ******************************
	' *  Handler for Page PreRender
	' ******************************

	Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs)
		If CompanySiteParameterGridView.Rows.Count = 0 Then
			If lblMessage.Text = "" Then
				lblMessage.Text = "No records found"
				pnlMessage.Visible = True
			End If
		End If
	End Sub

	' Dim iRecordCount As Integer = 0
	' ********************
	' *  Get record count
	' ********************

	Private Function ewRecordCount() As Integer
		Dim iRecordCount As Integer = 0
		Try

			'If (iRecordCount = 0)
				Dim data As CompanySiteParameterdal = New CompanySiteParameterdal()
				iRecordCount = data.GetRowsCount(CompanySiteParameterinf.GetUserFilter()) 

			'End If
		Catch e As Exception
			lblMessage.Text = e.Message
			pnlMessage.Visible = True
		End Try
		Return iRecordCount
	End Function

	' ********************************************
	' *  Handler for Selectable page size changed
	' ********************************************

	Protected Sub RecPerPage_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim dropDownList As DropDownList = TryCast(sender, DropDownList)
		Dim iPageSize As Integer = Convert.ToInt32(dropDownList.SelectedValue)
		If (iPageSize > 0) Then
			objProfile.AllowPaging = True
			objProfile.PageSize = iPageSize
			objProfile.PageIndex = 0
		Else
			objProfile.AllowPaging = False
			objProfile.PageSize = iPageSize
		End If
		SelectedItem(dropDownList, iPageSize)
		BindData()
	End Sub

	' ******************************
	' *  Process selected page size
	' ******************************

	Private Sub SelectedItem(ByVal dropDownList As DropDownList, ByVal iPageSize As Integer)
		Dim i As Integer = 0
		Do While (i < dropDownList.Items.Count)
			If (dropDownList.Items(i).Value = iPageSize.ToString()) Then
				dropDownList.Items(i).Selected = True
				Exit Do
			End If
			i += 1
		Loop
	End Sub

	' **********************************
	' *  Handler for Clear Search click
	' **********************************

	Protected Sub btnClearSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim data As CompanySiteParameterdal = New CompanySiteParameterdal()
		data.ClearSearch()
		objProfile.PageIndex = 0 'reset page index

		' Clear query String
		If (Not String.IsNullOrEmpty(objProfile.MasterPageUrl)) Then
			lblReturnUrl.Text = objProfile.MasterPageUrl
		End If
		Response.Redirect(lblReturnUrl.Text + "?")
	End Sub

	' ***************************
	' *  Advanced Search Routine
	' ***************************

	Private Sub AdvancedSearchRtn()
		Dim data As CompanySiteParameterdal = New CompanySiteParameterdal()

		' Set up Advanced Search
		If (Request.QueryString.Count > 0) Then
			data.ClearSearch() ' Clear Previous Search

			' Set up search parameters
			Dim vx_CompanyID As String = Request.QueryString("x_CompanyID")
			Dim vz_CompanyID As String = Request.QueryString("z_CompanyID")
			If (Not String.IsNullOrEmpty(vx_CompanyID) AndAlso vz_CompanyID IsNot Nothing) Then
				data.AdvancedSearchParm1.CompanyID = Convert.ToInt32(vx_CompanyID)
				data.AdvancedSearchParm1.Attribute("CompanyID").SearchType = Core.GetAdvancedSearchType(vz_CompanyID)
			End If

			' Set up search parameters
			Dim vx_SiteParameterTypeID As String = Request.QueryString("x_SiteParameterTypeID")
			Dim vz_SiteParameterTypeID As String = Request.QueryString("z_SiteParameterTypeID")
			If (Not String.IsNullOrEmpty(vx_SiteParameterTypeID) AndAlso vz_SiteParameterTypeID IsNot Nothing) Then
				data.AdvancedSearchParm1.SiteParameterTypeID = Convert.ToInt32(vx_SiteParameterTypeID)
				data.AdvancedSearchParm1.Attribute("SiteParameterTypeID").SearchType = Core.GetAdvancedSearchType(vz_SiteParameterTypeID)
			End If

			' Set up search parameters
			Dim vx_SortOrder As String = Request.QueryString("x_SortOrder")
			Dim vz_SortOrder As String = Request.QueryString("z_SortOrder")
			If (Not String.IsNullOrEmpty(vx_SortOrder) AndAlso vz_SortOrder IsNot Nothing) Then
				data.AdvancedSearchParm1.SortOrder = Convert.ToInt32(vx_SortOrder)
				data.AdvancedSearchParm1.Attribute("SortOrder").SearchType = Core.GetAdvancedSearchType(vz_SortOrder)
			End If

			' Set up search parameters
			Dim vx_ParameterValue As String = Request.QueryString("x_ParameterValue")
			Dim vz_ParameterValue As String = Request.QueryString("z_ParameterValue")
			If (Not String.IsNullOrEmpty(vx_ParameterValue) AndAlso vz_ParameterValue IsNot Nothing) Then
				data.AdvancedSearchParm1.ParameterValue = Convert.ToString(vx_ParameterValue)
				data.AdvancedSearchParm1.Attribute("ParameterValue").SearchType = Core.GetAdvancedSearchType(vz_ParameterValue)
			End If
		End If
		If (Request.QueryString.Count > 0) Then

			'BindData()
		End If
	End Sub

	' ******************
	' *  Handle sorting
	' ******************

	Protected Sub Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
		Dim sOrderBy As String = e.SortExpression
		Dim oInfo As CompanySiteParameterinf = New CompanySiteParameterinf()
		Dim sThisSort As String = String.Empty
		Dim sLastSort As String = String.Empty
		Dim sSortField As String = String.Empty
		Dim i As Integer = 1
		Do While (i <= oInfo.TableInfo.Count)
			Dim sSortParm As String = oInfo.TableInfo.Fields(i).SortParm
			Dim fldProfile As FieldProfile = CustomProfile.GetField(Share.ProjectName, CompanySiteParameterinf.TableVar, sSortParm)
			If (sOrderBy = sSortParm) Then
				sSortField = oInfo.TableInfo.Fields(i).FieldName
				sLastSort = fldProfile.Sort
				sThisSort = IIf(sLastSort = "ASC", "DESC", "ASC")
				fldProfile.Sort = sThisSort
			Else
				If (Not String.IsNullOrEmpty(fldProfile.Sort)) Then
					fldProfile.Sort = String.Empty
				End If
			End If
			i += 1
		Loop
		objProfile.OrderBy = sSortField & " " & sThisSort
		objProfile.PageIndex = 0
		sOrderBy = objProfile.OrderBy
		BindData() ' Bind to Data Control
		e.Cancel = True ' skip built-in sort
	End Sub

	' *****************
	' *  Clear Sorting
	' *****************

	Private Sub ClearSort()
		objProfile.OrderBy = String.Empty
	End Sub

	' ***************************
	' *  Set up Default Order By
	' ***************************

	Private Sub SetupDefaultOrderBy()
		Dim oInfo As CompanySiteParameterinf = New CompanySiteParameterinf()
		Dim aryDefaultOrder As String() = oInfo.TableInfo.DefaultOrderBy.Split(","c)
		Dim i As Integer = 1
		Do While (i < oInfo.TableInfo.Count)
			Dim sSortParm As String = oInfo.TableInfo.Fields(i).SortParm
			Dim sFieldName As String = oInfo.TableInfo.Fields(i).FieldName
			Dim fldProfile As FieldProfile = CustomProfile.GetField(Share.ProjectName, CompanySiteParameterinf.TableVar, sSortParm)
			fldProfile.Sort = String.Empty 
			Dim j As Integer = 0
			Do While (j < aryDefaultOrder.Length)
				If (aryDefaultOrder(j).IndexOf(sFieldName) >= 0) Then
					fldProfile.Sort = IIf(aryDefaultOrder(j).Trim().ToUpper().EndsWith(" DESC"), "DESC", "ASC")
					Exit Do
				End If
				j += 1
			Loop
			i += 1
		Loop
		objProfile.OrderBy = oInfo.TableInfo.DefaultOrderBy
	End Sub

	' *****************************************
	' *  Handler for GridView PageIndexChanged
	' *****************************************

	Protected Sub CompanySiteParameterGridView_PageIndexChanged(ByVal sender As Object,ByVal e As System.EventArgs)
		objProfile.PageIndex = CompanySiteParameterGridView.PageIndex ' Save page index
	End Sub

	' ***********************************
	' *  Handler for GridView RowCreated
	' ***********************************

	Protected Sub CompanySiteParameterGridView_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
		Dim button As LinkButton 
		If (e IsNot Nothing AndAlso e.Row.RowType = DataControlRowType.Header) Then
			For each cell As TableCell In e.Row.Cells
				If (cell.HasControls()) Then
					If (cell.Controls.Count > 2 AndAlso (TypeOf cell.Controls(1) is LinkButton)) Then
						button = TryCast(cell.Controls(1), LinkButton)
						If (isExport) Then
							button.Visible = False
							Dim label As Label = New Label()
							label.Text = button.Text.Replace("(*)", "")
							cell.Controls.Add(label)
						ElseIf (Not objProfile.AllowSorting) Then
							button.Visible = False
							Dim lbl As Label = New Label()
							lbl.Text = button.Text
							cell.Controls.Add(lbl)
						End If
						Dim image As System.Web.UI.WebControls.Image = New System.Web.UI.WebControls.Image()
						Dim sSort As String = CustomProfile.GetField(Share.ProjectName, CompanySiteParameterinf.TableVar, button.CommandArgument).Sort
						If (String.Compare(sSort,"ASC", True) = 0) Then
							image.ImageUrl = "images/sortup.gIf"
						ElseIf (String.Compare(sSort,"DESC", True) = 0) Then
							image.ImageUrl = "images/sortdown.gIf"
						End If
						If (image.ImageUrl.Length > 0) Then
							cell.Controls.Add(image)
						End If
					End If
				End If
			Next 
		End If
	End Sub

	' ************************************
	' *  Handler for GridView RowUpdating
	' ************************************

	Protected Sub CompanySiteParameterGridView_RowUpdating(ByVal sender As Object, Byval e As GridViewUpdateEventArgs)
		Dim source As WebControl = EW.Data.Core.GetGridViewEditRow(CType(sender, GridView))
		key.CompanySiteParameterID = Convert.ToInt32(e.Keys("CompanySiteParameterID")) ' setup key value
		If (source Is Nothing) Then Return
		Dim messageList As ArrayList = ValidateInputValues(source, Core.PageType.Edit) 
		If (messageList IsNot Nothing) Then
			lblMessage.Text = String.Empty 
			For Each sMsg As String In messageList
				lblMessage.Text &= sMsg & "<br>" 
			Next
			pnlMessage.Visible = True 
			RegisterClientID("InlineEditCtrlID", TryCast(source, Control))
			e.Cancel = True
			Return
		End If
		messageList = CheckDuplicateKey(TryCast(source,Control))
		If (messageList IsNot Nothing) Then
			lblMessage.Text = String.Empty
			For Each sMsg As string In messageList
				lblMessage.Text &= sMsg & "<br>"
			Next
			pnlMessage.Visible = True
			RegisterClientID("InlineEditCtrlID", TryCast(source, Control))
			e.Cancel = True
			Return
		End If
	End Sub

	' ***********************************
	' *  Handler for GridView RowUpdated
	' ***********************************

	Protected Sub CompanySiteParameterGridView_RowUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdatedEventArgs)
		If (e.AffectedRows = 0) Then

			' Keep in edit mode
			e.KeepInEditMode = True

			' Get original values
			Dim source As WebControl = EW.Data.Core.GetGridViewEditRow(TryCast(sender, GridView))
			If (source Is Nothing) Then Return
			Dim oldrow As CompanySiteParameterrow = New CompanySiteParameterrow()
			ControlToRow(oldrow, source)

			' Synchronize to database 
			Try
				CompanySiteParameterGridView.DataBind()
			Catch err As Exception
				lblMessage.Text &= "<br>" & err.Message
				pnlMessage.Visible = True
			End Try

			' Re-populate with values entered by user
			source = EW.Data.Core.GetGridViewEditRow(TryCast(sender, GridView)) ' must get correct object again
			If (source Is Nothing) Then Return
			RowToControl(oldrow, source, Core.CtrlType.Edit)
			oldrow = Nothing
		Else
			BindData()
		End if
	End Sub

	' ********************
	' *  Reset Edit Index
	' ********************

	Private Sub ResetEditIndex()
		CompanySiteParameterGridView.EditIndex = -1
	End Sub

	' *************************
	' *  Reset Page Properties
	' *************************

	Private Sub ResetPageProperties()

		' Clear Page Index
		objProfile.PageIndex = 0
		CompanySiteParameterGridView.PageIndex = 0
	End Sub

	' ************
	' *  Bind Data
	' ************

	Protected Sub BindData()
		Dim sWhere As String = GetUserFilter()
		Dim sOrderBy As String = String.Empty
		Dim data As CompanySiteParameterdal = New CompanySiteParameterdal()
		If (objProfile.PageIndex < 0) Then
			objProfile.PageIndex = 0
		End If
		CompanySiteParameterGridView.AllowPaging = objProfile.AllowPaging
		If (objProfile.PageSize > 0) Then
			CompanySiteParameterGridView.PageSize = objProfile.PageSize 
		End If
		If (Not objProfile.IsCollapsed) Then

			' Try to restore page index
			Try
				Dim intPageCount As Integer = data.GetPageCount(sWhere, CompanySiteParameterGridView.PageSize)
				If (CompanySiteParameterGridView.PageSize > 0 AndAlso intPageCount < objProfile.PageIndex + 1) Then objProfile.PageIndex = IIf(intPageCount > 0, intPageCount - 1, 0)
				CompanySiteParameterGridView.PageIndex = objProfile.PageIndex
			Catch e As Exception
				ResetPageProperties()
			End Try
		Else
			CompanySiteParameterGridView.PageIndex = 0
		End If
		Try
			CompanySiteParameterGridView.DataBind()
			If (CompanySiteParameterGridView.Rows.Count = 0) Then
				pnlPager.Visible = False ' Hide Pager
				btnExpand.Visible = False ' Hide Expand Button
			Else
			If (Not isExport) Then
				Dim isCollapse As Boolean= objProfile.IsCollapsed
				SetupControlsVisible(Not isCollapse)
				pnlPager.Visible = Not isCollapse ' Show Pager
				btnExpand.Visible = isCollapse
			End If
			End If
		Catch e As Exception
			pnlPager.Visible = False ' Hide Pager
				btnExpand.Visible = False ' Hide Expand Button
			If (e.InnerException IsNot Nothing) Then
				lblMessage.Text = "<br>" & e.InnerException.Message
			Else
				lblMessage.Text = "<br>" & e.Message
			End If
			pnlMessage.Visible = True
		End Try
	End Sub

	' *****************************
	' *  Handler for GridView Init
	' *****************************

	Protected Sub CompanySiteParameterGridView_Init(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			CompanySiteParameterdal.OpenConnection()
		Catch err As Exception
			pnlPager.Visible = False ' Hide Pager
				btnExpand.Visible = False ' Hide Expand Button
			If (err.InnerException IsNot Nothing) Then
				lblMessage.Text = "<br>" & err.InnerException.Message
			Else
				lblMessage.Text = "<br>" & err.Message
			End If
			pnlMessage.Visible = True
		End Try
	End Sub

	' *****************************
	' *  Handler for GridView Load
	' *****************************

	Protected Sub CompanySiteParameterGridView_Load(ByVal sender As Object, ByVal e As System.EventArgs)
		If (isExport) Then
			SetupControlsVisible(False)
			CompanySiteParameterGridView.PagerSettings.Visible = False
			Return
		End If
	End Sub

	' *************************************
	' *  Handler for GridView Unload
	' *************************************

	Protected Sub CompanySiteParameterGridView_Unload(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			CompanySiteParameterdal.CloseAndDisposeConnection()
		Catch
		End Try
	End Sub

	' *************************************
	' *  Handler for GridView RowDataBound
	' *************************************

	Protected Sub CompanySiteParameterGridView_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
		tableId = TryCast(sender, GridView).ClientID
		If (CompanySiteParameterGridView.PageCount > 1 AndAlso CompanySiteParameterGridView.PagerSettings.Visible) Then
			If (CompanySiteParameterGridView.PagerSettings.Position = PagerPosition.Top OrElse CompanySiteParameterGridView.PagerSettings.Position = PagerPosition.TopAndBottom) Then
				firstRowOffset = initialFirstRowOffset + 1
			End If
			If (CompanySiteParameterGridView.PagerSettings.Position = PagerPosition.Bottom OrElse CompanySiteParameterGridView.PagerSettings.Position = PagerPosition.TopAndBottom) Then
				lastRowOffset = initialLastRowOffset + 1
			End If
		End If
		If (e.Row.RowType = DataControlRowType.Header) Then
		ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
			Dim row As CompanySiteParameterrow = TryCast(e.Row.DataItem, CompanySiteParameterrow) ' get row object
			Dim control As GridViewRow = e.Row ' get control object
			Dim bNormal As Boolean = ((e.Row.RowState And DataControlRowState.Normal) = DataControlRowState.Normal)
			Dim bAlternate As Boolean = ((e.Row.RowState And DataControlRowState.Alternate) = DataControlRowState.Alternate)
			Dim bEdit As Boolean = ((e.Row.RowState And DataControlRowState.Edit) = DataControlRowState.Edit)
			If ((bNormal OrElse bAlternate) AndAlso (Not bEdit)) Then
				RowToControl(row, TryCast(control, WebControl) , Core.CtrlType.View)
			ElseIf (bEdit) Then
				RowToControl(row, TryCast(control, WebControl), Core.CtrlType.Edit)
				RegisterClientID("InlineEditCtrlID", TryCast(control, Control))
			End If
			Dim EditLink As HyperLink = TryCast(control.FindControl("EditLink"), HyperLink)
			If (EditLink IsNot Nothing) Then _
				EditLink.NavigateUrl = String.Format("companysiteparameter_edit.aspx?CompanySiteParameterID={0}", Server.UrlEncode(Convert.ToString(row.CompanySiteParameterID)))
			Dim DeleteLink As HyperLink = TryCast(control.FindControl("DeleteLink"), HyperLink)
			If (DeleteLink IsNot Nothing) Then _
				DeleteLink.NavigateUrl = String.Format("companysiteparameter_delete.aspx?CompanySiteParameterID={0}", Server.UrlEncode(Convert.ToString(row.CompanySiteParameterID)))
		ElseIf (e.Row.RowType = DataControlRowType.Footer) Then
			Dim control As GridViewRow = e.Row ' get control object
		End If
	End Sub

	' **********************************
	' *  Handler for GridView DataBound
	' **********************************

	Protected Sub CompanySiteParameterGridView_DataBound(ByVal sender As Object, ByVal e As System.EventArgs)
	End Sub

	' ***********************************
	' *  Handler for GridView RowCommand
	' ***********************************

	Protected Sub CompanySiteParameterGridView_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)
	End Sub

	' ***********************************
	' *  Handler for Expand Click
	' ***********************************	

	Protected Sub btnExpand_Click(ByVal sender As Object, ByVal e As System.EventArgs) 
		objProfile.IsCollapsed = False
		objProfile.AllowSorting = True
		BindData()
	End Sub

	' ***************************************************
	' *  Setup Controls Visible
	' ***************************************************

	Private Sub SetupControlsVisible(ByVal visible As Boolean)
		CompanySiteParameterGridView.Columns(1).Visible = visible
		CompanySiteParameterGridView.Columns(0).Visible = visible
		lnkAdd.Visible = visible
		CompanySiteParameterGridView.Columns(2).Visible = visible
		CompanySiteParameterGridView.AllowSorting = visible
	End Sub

	' ********************************
	' * Register ClientID of Controls
	' ********************************

	Private Sub RegisterClientID(ByVal sBlockName As String, ByVal control As Control)
		Dim jsString As String = String.Empty
		If (control.FindControl("x_CompanyID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_CompanyID'] = '" & control.FindControl("x_CompanyID").ClientID & "';"
		End If
		If (control.FindControl("x_SiteParameterTypeID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_SiteParameterTypeID'] = '" & control.FindControl("x_SiteParameterTypeID").ClientID & "';"
		End If
		If (control.FindControl("x_SortOrder") IsNot Nothing) Then
			jsString &= "ew.Controls['x_SortOrder'] = '" & control.FindControl("x_SortOrder").ClientID & "';"
		End If
		If (control.FindControl("x_ParameterValue") IsNot Nothing) Then
			jsString &= "ew.Controls['x_ParameterValue'] = '" & control.FindControl("x_ParameterValue").ClientID & "';"
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
			Dim x_CompanyID As DropDownList = TryCast(control.FindControl("x_CompanyID"), DropDownList)
			If (x_CompanyID IsNot Nothing) Then
				If ((x_CompanyID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_CompanyID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Site")
				ElseIf (String.IsNullOrEmpty(x_CompanyID.SelectedValue)) Then
					messageList.Add("Please enter required field (Int32): Site")
				End If
			End If
			Dim x_SiteParameterTypeID As DropDownList = TryCast(control.FindControl("x_SiteParameterTypeID"), DropDownList)
			If (x_SiteParameterTypeID IsNot Nothing) Then
				If ((x_SiteParameterTypeID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_SiteParameterTypeID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Parameter Type")
				ElseIf (String.IsNullOrEmpty(x_SiteParameterTypeID.SelectedValue)) Then
					messageList.Add("Please enter required field (Int32): Parameter Type")
				End If
			End If
			Dim x_SortOrder As TextBox = TryCast(control.FindControl("x_SortOrder"), TextBox)
			If (x_SortOrder IsNot Nothing) Then
				If (Not DataFormat.CheckInt32(x_SortOrder.Text)) Then
					messageList.Add("Invalid Value (Int32): Order")
				End If
			End If
			Dim x_ParameterValue As TextBox = TryCast(control.FindControl("x_ParameterValue"), TextBox)
			If (x_ParameterValue IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_ParameterValue.Text)) Then
					messageList.Add("Invalid Value (String): Parameter Value")
				End If
			End If
		End If
		Return IIf(messageList.Count = 0, Nothing, messageList)
	End Function

	' **********************************************
	' *  Convert Input Control Values to Row Object
	' **********************************************

	Private Sub ControlToRow(ByVal row As CompanySiteParameterrow, ByVal control As WebControl)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		row.SetPageType(Core.PageType.List) ' set page type = list for update

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

		' Field SiteParameterTypeID
		Dim x_SiteParameterTypeID As DropDownList = TryCast(control.FindControl("x_SiteParameterTypeID"), DropDownList)
		Dim v_SiteParameterTypeID As String = String.Empty
		If (x_SiteParameterTypeID.SelectedIndex >= 0) Then
			For Each li As ListItem In x_SiteParameterTypeID.Items
				If (li.Selected) Then
					v_SiteParameterTypeID += li.Value
					Exit For
				End If
			Next
		End If
		If (v_SiteParameterTypeID <> String.Empty) Then row.SiteParameterTypeID = Convert.ToInt32(v_SiteParameterTypeID) Else row.SiteParameterTypeID = CType(Nothing, Nullable(Of Int32))

		' Field SortOrder
		Dim x_SortOrder As TextBox = TryCast(control.FindControl("x_SortOrder"), TextBox)
		If (x_SortOrder.Text <> String.Empty) Then row.SortOrder = Convert.ToInt32(x_SortOrder.Text) Else row.SortOrder = CType(Nothing, Nullable(Of Int32))

		' Field ParameterValue
		Dim x_ParameterValue As TextBox = TryCast(control.FindControl("x_ParameterValue"), TextBox)
		If (x_ParameterValue.Text <> String.Empty) Then row.ParameterValue = x_ParameterValue.Text Else row.ParameterValue = Nothing
	End Sub

	' ****************************
	' *  Check for duplicate Key
	' ****************************

	Private Function CheckDuplicateKey(ByVal control As WebControl) As ArrayList 
		Dim sWhere As String = String.Empty
		Dim messageList As ArrayList = New ArrayList()
		Dim data As CompanySiteParameterdal = New CompanySiteParameterdal()
		Dim newkey As CompanySiteParameterkey = New CompanySiteParameterkey()
		Try
			Dim sKeyWhere As string = data.KeyFilter(key)
		data = Nothing
		Catch e As Exception
			messageList.Add(e.Message)
		End Try
		Return IIf(messageList.Count = 0, Nothing, messageList)
	End Function

	' ********************************************
	' *  Convert Row Object Value To Input Control
	' ********************************************

	Private Sub RowToControl(ByVal row As CompanySiteParameterrow, ByVal control As WebControl, ByVal ct As Core.CtrlType)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		If (ct = Core.CtrlType.View) Then ' set up view controls

			' Field CompanyID
			Dim x_CompanyID As Label = TryCast(control.FindControl("x_CompanyID"), Label)
			Dim v_CompanyID As String
			If (row.CompanyID.HasValue) Then v_CompanyID = row.CompanyID.ToString() Else v_CompanyID = String.Empty
			x_CompanyID.Text = CompanySiteParameterdal.LookUpTable("CompanyID", v_CompanyID)

			' Field SiteParameterTypeID
			Dim x_SiteParameterTypeID As Label = TryCast(control.FindControl("x_SiteParameterTypeID"), Label)
			Dim v_SiteParameterTypeID As String
			If (row.SiteParameterTypeID.HasValue) Then v_SiteParameterTypeID = row.SiteParameterTypeID.ToString() Else v_SiteParameterTypeID = String.Empty
			x_SiteParameterTypeID.Text = CompanySiteParameterdal.LookUpTable("SiteParameterTypeID", v_SiteParameterTypeID)

			' Field SortOrder
			Dim x_SortOrder As Label = TryCast(control.FindControl("x_SortOrder"), Label)
			If (row.SortOrder.HasValue) Then x_SortOrder.Text = row.SortOrder.ToString() Else x_SortOrder.Text = String.Empty

			' Field ParameterValue
			Dim x_ParameterValue As Label = TryCast(control.FindControl("x_ParameterValue"), Label)
			If (row.ParameterValue IsNot Nothing) Then x_ParameterValue.Text = row.ParameterValue.ToString() Else x_ParameterValue.Text = String.Empty
		End If
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
			x_CompanyID.ClearSelection()
			For Each li As ListItem In x_CompanyID.Items
				If (li.Value.ToString() = v_CompanyID) Then
					li.Selected = True
					Exit For
				End If
			Next

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
			x_SiteParameterTypeID.ClearSelection()
			For Each li As ListItem In x_SiteParameterTypeID.Items
				If (li.Value.ToString() = v_SiteParameterTypeID) Then
					li.Selected = True
					Exit For
				End If
			Next

			' Field SortOrder
			Dim x_SortOrder As TextBox = TryCast(control.FindControl("x_SortOrder"), TextBox)
			If (row.SortOrder.HasValue) Then x_SortOrder.Text = row.SortOrder.ToString() Else x_SortOrder.Text = String.Empty

			' Field ParameterValue
			Dim x_ParameterValue As TextBox = TryCast(control.FindControl("x_ParameterValue"), TextBox)
			If (row.ParameterValue IsNot Nothing) Then x_ParameterValue.Text = row.ParameterValue.ToString() Else x_ParameterValue.Text = String.Empty
			Page.Form.DefaultFocus = control.FindControl("x_CompanyID").ClientID
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
	' *  Handler for DataSource Selecting
	' ************************************

	Protected Sub CompanySiteParameterDataSource_Selecting(Byval sender As Object, ByVal e As ObjectDataSourceSelectingEventArgs)
		e.InputParameters.Clear()
		e.InputParameters.Add("filter", CompanySiteParameterinf.GetUserFilter())
	End Sub

	' *************************************
	' *  DataSource Selected Event Handler
	' *************************************

	Protected Sub CompanySiteParameterDataSource_Selected(Byval sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.ReturnValue Is Nothing) Then
			If (e.Exception IsNot Nothing) Then
				lblMessage.Text = e.Exception.Message
				pnlMessage.Visible = True
			Else
				lblMessage.Text = "No records found"
				pnlMessage.Visible = True
			End If
		End If
	End Sub

	' ***********************************
	' *  Handler for DataSource Updating
	' ***********************************

	Protected Sub CompanySiteParameterDataSource_Updating(ByVal sender As Object, ByVal e As ObjectDataSourceMethodEventArgs)
		Dim source As WebControl = EW.Data.Core.GetGridViewEditRow(CompanySiteParameterGridView)
		If (source Is Nothing) Then
			e.Cancel = True
			Return
		End If
		Dim sTmp As String = String.Empty

		' Set up row object
		Dim row As CompanySiteParameterrow = TryCast(e.InputParameters(0), CompanySiteParameterrow)
		Dim data As CompanySiteParameterdal = New CompanySiteParameterdal()
		oldrow = data.LoadRow(key, CompanySiteParameterinf.GetUserFilter())
		ControlToRow(row, source) ' load values of controls to row
		newrow = row
		If (CompanySiteParameterbll.Updating(oldrow, newrow)) Then
		Else
			e.Cancel = True
			If (Not String.IsNullOrEmpty(objProfile.Message)) Then
				lblMessage.Text = objProfile.Message
				pnlMessage.Visible = True
			End If
		End If
	End Sub

	' **********************************
	' *  Handler for DataSource Updated
	' **********************************

	Protected Sub CompanySiteParameterDataSource_Updated(ByVal sender As Object, ByVal e As ObjectDataSourceStatusEventArgs)
		If (e.Exception IsNot Nothing) Then
			e.AffectedRows = 0
			e.ExceptionHandled = True
			lblMessage.Text = e.Exception.InnerException.Message
		Else
			lblMessage.Text = "Update successful"
			CompanySiteParameterbll.Updated(oldrow, newrow)
		End If
		If (lblMessage.Text <> "") Then pnlMessage.Visible = True
	End Sub

	' *****************
	' *  Load Controls 
	' *****************

	Private Sub LoadSearchControls(ByVal control As Control) 
		Dim sSearchType As String = String.Empty
		Dim data As CompanySiteParameterdal = New CompanySiteParameterdal()
	End Sub

	' *******************
	' *  Get User Filter
	' *******************

	Private Function GetUserFilter() As String 
		Return CompanySiteParameterinf.GetUserFilter()
	End Function
	Dim isExport As Boolean = False

	' *******************************
	' *  Change Web Control to Literal
	' *******************************

    Private Sub PrepareGridViewForExport(ByVal gv As Control)
        Dim l As Literal = New Literal
        Dim name As String = String.Empty
        Dim i As Integer = 0
        Do While (i < gv.Controls.Count)
            If gv.Controls(i).Visible Then
                If (gv.Controls(i).GetType.Equals(GetType(LinkButton))) Then
                    l.Text = CType(gv.Controls(i),LinkButton).Text
                    gv.Controls.Remove(gv.Controls(i))
                    gv.Controls.AddAt(i, l)
                ElseIf (gv.Controls(i).GetType.Equals(GetType(DropDownList))) Then
                    l.Text = CType(gv.Controls(i),DropDownList).SelectedItem.Text
                    gv.Controls.Remove(gv.Controls(i))
                    gv.Controls.AddAt(i, l)
                ElseIf (gv.Controls(i).GetType.Equals(GetType(CheckBox))) Then
                    l.Text = IIf(CType(gv.Controls(i),CheckBox).Checked, "True", "False")
                    gv.Controls.Remove(gv.Controls(i))
                    gv.Controls.AddAt(i, l)
                ElseIf (gv.Controls(i).GetType.Equals(GetType(ImageHyperLink))) Then
                ElseIf (gv.Controls(i).GetType.Equals(GetType(HyperLink))) Then
                    If (gv.Controls(i).Controls.Count = 0) Then
                        l.Text = CType(gv.Controls(i),HyperLink).Text
                        gv.Controls.Remove(gv.Controls(i))
                        gv.Controls.AddAt(i, l)
                    End If
                ElseIf (gv.Controls(i).GetType.Equals(GetType(Image))) Then
                    If (gv.Controls(i).Parent.GetType.Equals(GetType(DataControlFieldHeaderCell))) Then
                        gv.Controls.Remove(gv.Controls(i))
                    End If
                End If
                If ((i < gv.Controls.Count) AndAlso gv.Controls(i).HasControls) Then
                    PrepareGridViewForExport(gv.Controls(i))
                End If
            End If
            i = (i + 1)
        Loop
    End Sub

    ' ********************************************************
    ' *  Override VerifyRenderingInServerForm to suppress error
    ' ********************************************************

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
    End Sub

	' **********************
	' *  Handler for Export
	' **********************

	Public Sub Export_Command(Byval exportType As String)
		isExport = True
		pnlExport.Visible = False
		Select Case exportType.ToUpper()
			Case "EXCEL"
				Response.Clear()
				Response.ContentType = "application/vnd.ms-excel"
				Response.AddHeader("Content-Disposition", "attachment; filename=""CompanySiteParameter.xls""")
				Export("excel")
			Case "XML"
				Response.Clear()
				Response.ContentType = "text/xml"
				Response.AddHeader("Content-Disposition", "attachment; filename=""CompanySiteParameter.xml""")
				Export("xml")
		End Select
	End Sub

	' ************************************
	' * Export to Word/Excel/CSV/XML/HTML
	' ************************************

	Private Sub Export(ByVal sExport As String)
		Dim isExportWord As Boolean = (String.Compare(sExport, "word", true) = 0)
		Dim isExportExcel As Boolean = (String.Compare(sExport, "excel", true) = 0)
		Dim isExportHtml As Boolean = (String.Compare(sExport, "html", true) = 0)
		Dim isExportCsv As Boolean = (String.Compare(sExport, "csv", true) = 0)
		Dim isExportXml As Boolean = (String.Compare(sExport, "xml", true) = 0)
		Dim isEndResponse As Boolean = (isExportWord OrElse isExportExcel OrElse isExportCsv OrElse isExportXml)

		' *********************************************************************
		' * Uncomment following lines if want to export all records
		' *********************************************************************
		'Dim nCurrentPageSize As Integer = objProfile.PageSize
		'objProfile.PageSize = 0
		'objProfile.AllowPaging = False

		If (isExportWord OrElse isExportExcel) Then
			Using sw As StringWriter = New StringWriter()
				Using htw As HtmlTextWriter = New HtmlTextWriter(sw)
					CompanySiteParameterGridView.Columns(1).Visible = False
					CompanySiteParameterGridView.Columns(0).Visible = False
					lnkAdd.Visible = False
					CompanySiteParameterGridView.Columns(2).Visible = False
					CompanySiteParameterGridView.PagerSettings.Visible = False
					CompanySiteParameterGridView.AllowSorting = False
					BindData()
					PrepareGridViewForExport(CompanySiteParameterGridView)

					' Start Render
					htw.RenderBeginTag(HtmlTextWriterTag.Html)

					' Render Head
					htw.RenderBeginTag(HtmlTextWriterTag.Head)
					htw.RenderEndTag()
					htw.WriteLineNoTabs("")

					' Render Body
					htw.RenderBeginTag(HtmlTextWriterTag.Body)

					' Render Table Name
					htw.RenderBeginTag(HtmlTextWriterTag.P)
					htw.WriteEncodedText("TABLE: Site Parameter")
					htw.RenderEndTag()

					' Render Table
					CompanySiteParameterGridView.RenderControl(htw)

					' Render Body End Tag
					htw.RenderEndTag()

					' Render Html End Tag
					htw.RenderEndTag()
					Response.Write(sw.ToString())
				End Using
			End Using
		End If 
		If (isExportCsv OrElse isExportXml) Then
			Response.Buffer = True
			Dim ioout As TextWriter = Response.Output
			Dim dataSet As CompanySiteParameterrows 
			Dim sFldParm As String
			Dim oInfo As CompanySiteParameterinf = New CompanySiteParameterinf()
			Dim nPageSize As Integer = objProfile.PageSize
			Dim nPageIndex As Integer = objProfile.PageIndex
			Dim nStartRec As Integer = nPageSize * nPageIndex
			Dim nStopRec As Integer
			If (nPageSize > 0) Then
				nStopRec = nStartRec + nPageSize
			Else
				nStopRec = Int32.MaxValue ' Show all values
			End If
			Dim data As CompanySiteParameterdal = New CompanySiteParameterdal()
			dataSet = TryCast(data.LoadList(CompanySiteParameterinf.GetUserFilter()), CompanySiteParameterrows)
			Dim sExportContent As System.Text.StringBuilder = New StringBuilder()
			If (isExportXml) Then
				ioout.WriteLine("<?xml version=""1.0"" standalone=""yes""?>")
				Dim oXmlDoc As System.Xml.XmlDocument = New System.Xml.XmlDocument()
				Dim oXmlRoot As System.Xml.XmlElement = oXmlDoc.CreateElement("root")
				Dim oXmlTbl As System.Xml.XmlElement = oXmlDoc.CreateElement("table")
				Dim xmlWriter As System.Xml.XmlTextWriter = New System.Xml.XmlTextWriter(ioout)
				Dim i As Integer = nStartRec
				Do While (dataSet IsNot Nothing AndAlso i < dataSet.Count AndAlso (nStartRec < nStopRec))
					Dim oXmlRec As System.Xml.XmlElement = oXmlDoc.CreateElement("record")
					Dim oXmlField As System.Xml.XmlElement 
					sFldParm = "CompanyID"
					oXmlField = oXmlDoc.CreateElement(sFldParm)
					If (dataSet(i).CompanyID.HasValue) Then
						oXmlField.InnerText = Convert.ToString(dataSet(i).CompanyID)
					Else
						oXmlField.InnerText = "Null"
					End If
					oXmlRec.AppendChild(oXmlField)
					sFldParm = "SiteParameterTypeID"
					oXmlField = oXmlDoc.CreateElement(sFldParm)
					If (dataSet(i).SiteParameterTypeID.HasValue) Then
						oXmlField.InnerText = Convert.ToString(dataSet(i).SiteParameterTypeID)
					Else
						oXmlField.InnerText = "Null"
					End If
					oXmlRec.AppendChild(oXmlField)
					sFldParm = "SortOrder"
					oXmlField = oXmlDoc.CreateElement(sFldParm)
					If (dataSet(i).SortOrder.HasValue) Then
						oXmlField.InnerText = Convert.ToString(dataSet(i).SortOrder)
					Else
						oXmlField.InnerText = "Null"
					End If
					oXmlRec.AppendChild(oXmlField)
					sFldParm = "ParameterValue"
					oXmlField = oXmlDoc.CreateElement(sFldParm)
					If (dataSet(i).ParameterValue IsNot Nothing) Then
						oXmlField.InnerText = Convert.ToString(dataSet(i).ParameterValue)
					Else
						oXmlField.InnerText = "Null"
					End If
					oXmlRec.AppendChild(oXmlField)
					oXmlTbl.AppendChild(oXmlRec)
					i += 1
					nStartRec += 1
				Loop
				oXmlRoot.AppendChild(oXmlTbl)
				oXmlRoot.WriteContentTo(xmlWriter)
			End If
			dataSet = Nothing
		End If

		' **********************************************************************
		' * Uncomment following lines if want to export all records: restore page size
		' ***********************************************************************
		'objProfile.PageSize = nCurrentPageSize
		'objProfile.AllowPaging = (nCurrentPageSize <> 0)

		If (isEndResponse) Then Response.End()
	End Sub
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server" />
	<span class="aspnetmaker">Site Parameter</span>
	<asp:PlaceHolder ID="pnlExport" runat="server">
		<asp:HyperLink CssClass="aspnetmaker" ID="lnkExportExcel" runat="server" NavigateUrl="companysiteparameter_list.aspx?Export=excel">Export to Excel</asp:HyperLink>&nbsp;&nbsp;
		<asp:HyperLink CssClass="aspnetmaker" ID="lnkExprotXML" runat="server" NavigateUrl="companysiteparameter_list.aspx?Export=xml">Export to XML</asp:HyperLink>&nbsp;&nbsp;
	</asp:PlaceHolder>
<script type="text/javascript">
<!--
var usecss = true; // use css
//var usecss = false; // not use css
var rowclass = 'ewTableRow'; // row class
var rowaltclass = 'ewTableAltRow'; // row alternate class
var rowmoverclass = 'ewTableHighlightRow'; // row mouse over class
var rowselectedclass = 'ewTableSelectRow'; // row selected class
var roweditclass = 'ewTableEditRow'; // row edit class
var rowmasterclass = 'ewTableSelectRow'; // row master class
var rowcolor = '#FFFFFF'; // row color
var rowaltcolor = '#F5F5F5'; // row alternate color
var rowmovercolor = ''; // row mouse over color
var rowselectedcolor = ''; // row selected color
var roweditcolor = '#FFFF99'; // row edit color
var rowmastercolor = '#E6E6FA'; // row master color
//-->
</script>
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
	<asp:ValidationSummary id="xevs_CompanySiteParameter" CssClass="aspnetmaker" runat="server"
		HeaderText=""
		ShowSummary="False"
		ShowMessageBox="True"
		ForeColor="#FF0000" />
	<asp:PlaceHolder ID="pnlSearch" runat="server">
		<asp:Panel runat="server">
			<p />
			<asp:Table runat="server" BorderWidth="0" CellSpacing="1" CellPadding="4" ID="mh_CompanySiteParameter">
				<asp:TableRow>
					<asp:TableCell>
						<asp:Table runat="server">
						</asp:Table>
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow>
					<asp:TableCell>
	<asp:LinkButton ID="btnClearSearch" OnClick="btnClearSearch_Click" runat="server" 			CssClass="aspnetmaker" Text="Show all"></asp:LinkButton>&nbsp;<asp:HyperLink ID="lnkSearch" runat="server" CssClass="aspnetmaker" Text="Advanced Search" NavigateUrl="companysiteparameter_search.aspx"></asp:HyperLink>
					</asp:TableCell>
				</asp:TableRow>
			</asp:Table>
		</asp:Panel>
	</asp:PlaceHolder>
	<p><asp:HyperLink ID="lnkAdd" runat="server" CssClass="aspnetmaker" NavigateUrl="companysiteparameter_add.aspx">Add</asp:HyperLink></p>
	<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
		<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
	</asp:PlaceHolder>
	<p><asp:Label id="lblReturnUrl" Visible="False" Text="companysiteparameter_list.aspx" CssClass="aspnetmaker" runat="server" />
    <asp:Button ID="btnExpand" runat="server" Text="Expand" Visible="False" OnClick="btnExpand_Click" />
	</p>
	<asp:PlaceHolder runat="server" ID="pnlPager">
		<table cellspacing="0" cellpadding="0">
			<tr>
				<td nowrap="nowrap">
					<span class="aspnetmaker">Records Per Page&nbsp;
						<asp:DropDownList ID="RecPerPage" runat="server" OnSelectedIndexChanged="RecPerPage_SelectedIndexChanged" AutoPostBack="True" CssClass="aspnetmaker">
							<asp:ListItem Value="1">1</asp:ListItem>
							<asp:ListItem Value="10">10</asp:ListItem>
							<asp:ListItem Value="20">20</asp:ListItem>
							<asp:ListItem Value="50">50</asp:ListItem>
							<asp:ListItem Value="0">All Records</asp:ListItem>	
						</asp:DropDownList>
					</span>
				</td>
			</tr>
		</table>
	</asp:PlaceHolder>
	<br />
	<!-- Data Source -->
<asp:ObjectDataSource ID="CompanySiteParameterDataSource"
	TypeName="PMGEN.CompanySiteParameterdal"
	DataObjectTypeName="PMGEN.CompanySiteParameterrow"
	SelectMethod="LoadList"
	UpdateMethod="Update"
	OnSelecting="CompanySiteParameterDataSource_Selecting"
	OnSelected="CompanySiteParameterDataSource_Selected"
	OnUpdating="CompanySiteParameterDataSource_Updating"
	OnUpdated="CompanySiteParameterDataSource_Updated"
	EnablePaging="True"
	SelectCountMethod="GetRowsCount"
	MaximumRowsParameterName="PageSize"
	StartRowIndexParameterName="StartRow"
	runat="server">
</asp:ObjectDataSource>
	<asp:GridView ID="CompanySiteParameterGridView"
		PageSize="50"
		DataKeyNames="CompanySiteParameterID"
		DataSourceID="CompanySiteParameterDataSource"
		GridLines="None"
		AutoGenerateColumns="False" CssClass="ewTable"
		AllowSorting="True" AllowPaging="True"
		OnInit="CompanySiteParameterGridView_Init"
		OnDataBound="CompanySiteParameterGridView_DataBound"
		OnRowCommand="CompanySiteParameterGridView_RowCommand"
		OnRowUpdating="CompanySiteParameterGridView_RowUpdating"
		OnRowUpdated="CompanySiteParameterGridView_RowUpdated"
		OnRowDataBound="CompanySiteParameterGridView_RowDataBound"
		OnSorting="Sorting"
		OnRowCreated="CompanySiteParameterGridView_RowCreated"
		OnPageIndexChanged="CompanySiteParameterGridView_PageIndexChanged"
		OnLoad="CompanySiteParameterGridView_Load"
		OnUnload="CompanySiteParameterGridView_Unload"
		PagerSettings-Mode="NextPreviousFirstLast"
		PagerSettings-Position="Top"
		runat="server">
		<HeaderStyle Wrap="False" CssClass="ewTableHeader" />
		<RowStyle CssClass="ewTableRow" />
		<AlternatingRowStyle CssClass="ewTableAltRow" />
		<EditRowStyle CssClass="ewTableEditRow" />
		<FooterStyle CssClass="ewTableFooter" />
		<SelectedRowStyle CssClass="ewTableSelectRow" />
		<PagerStyle CssClass="ewTablePager" />
		<Columns>
		<asp:TemplateField>
			<ItemTemplate>
				<asp:LinkButton ID="InlineEdit" runat="server" CssClass="aspnetmaker" CommandName="Edit" Text="Inline Edit" />
			</ItemTemplate>
			<EditItemTemplate>
				<asp:LinkButton ID="Cancel" runat="server" CssClass="aspnetmaker" CommandName="Cancel" Text="Cancel" CausesValidation="false" />
				<asp:LinkButton ID="Update" runat="server" CssClass="aspnetmaker" CommandName="Update" Text="Update" />
			</EditItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
				<asp:HyperLink ID="EditLink" CssClass="aspnetmaker" runat="server"  Text="Edit" />
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
				<asp:HyperLink ID="DeleteLink" CssClass="aspnetmaker" runat="server" Text="Delete" />
			</ItemTemplate>
		</asp:TemplateField>
			<asp:TemplateField>
				<ItemStyle Wrap="False" />
				<HeaderTemplate>
					<asp:LinkButton runat='server' id="xs_CompanyID"  CssClass="ewTableHeader" CommandName="Sort" CommandArgument="CompanyID">Site </asp:LinkButton>
				</HeaderTemplate>
				<ItemTemplate>
<asp:Label id="x_CompanyID" CssClass="aspnetmaker" runat="server" />
				</ItemTemplate>
				<EditItemTemplate>
<asp:DropDownList ID="x_CompanyID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
<asp:RequiredFieldValidator ID="vx_CompanyID" CssClass="aspnetmaker" runat="server" ControlToValidate="x_CompanyID" ErrorMessage="Please enter required field - Site" ForeColor="Red" Display="None" Text="*" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<ItemStyle Wrap="False" />
				<HeaderTemplate>
					<asp:LinkButton runat='server' id="xs_SiteParameterTypeID"  CssClass="ewTableHeader" CommandName="Sort" CommandArgument="SiteParameterTypeID">Parameter Type </asp:LinkButton>
				</HeaderTemplate>
				<ItemTemplate>
<asp:Label id="x_SiteParameterTypeID" CssClass="aspnetmaker" runat="server" />
				</ItemTemplate>
				<EditItemTemplate>
<asp:DropDownList ID="x_SiteParameterTypeID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
<asp:RequiredFieldValidator ID="vx_SiteParameterTypeID" CssClass="aspnetmaker" runat="server" ControlToValidate="x_SiteParameterTypeID" ErrorMessage="Please enter required field - Parameter Type" ForeColor="Red" Display="None" Text="*" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<ItemStyle Wrap="False" />
				<HeaderTemplate>
					<asp:LinkButton runat='server' id="xs_SortOrder"  CssClass="ewTableHeader" CommandName="Sort" CommandArgument="SortOrder">Order </asp:LinkButton>
				</HeaderTemplate>
				<ItemTemplate>
<asp:Label id="x_SortOrder" CssClass="aspnetmaker" runat="server" />
				</ItemTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_SortOrder" Columns="30" CssClass="aspnetmaker" runat="server" />
<ew:IntegerValidator ID="vc_SortOrder" ClientValidationFunction="ew_CheckInteger" CssClass="aspnetmaker" runat="server" ControlToValidate="x_SortOrder" ErrorMessage="Incorrect integer - Order" Display="None" ForeColor="Red" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<ItemStyle Wrap="True" />
				<HeaderTemplate>
					<asp:Label runat='server' id="s_ParameterValue"  CssClass="ewTableHeader">Parameter Value</asp:Label>
				</HeaderTemplate>
				<ItemTemplate>
<asp:Label id="x_ParameterValue" CssClass="aspnetmaker" runat="server" />
				</ItemTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_ParameterValue" TextMode="MultiLine" Rows="4" Columns="35" CssClass="aspnetmaker" runat="server" />
				</EditItemTemplate>
			</asp:TemplateField>
		</Columns>
	</asp:GridView>
</asp:Content>
