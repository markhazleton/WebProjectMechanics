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
	Dim key As PageImagekey = New PageImagekey()  ' record key
	Dim oldrow As PageImagerow ' old record data input by user
	Dim newrow As PageImagerow ' new record data input by user
	Dim oldrows As PageImagerows = New PageImagerows() ' old records (inline delete)
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
		objProfile = CustomProfile.GetTable(Share.ProjectName, PageImageinf.TableVar)
	End Sub

	' *************************
	' *  Handler for Page Load
	' *************************

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
                Session("ListPageURL") = HttpContext.Current.Request.FilePath()
		lblMessage.Text = String.Empty
		Dim isAuth As Boolean = False
		Page.MaintainScrollPositionOnPostBack = False
		tableId = PageImageGridView.ClientID
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
					Dim data As PageImagedal = New PageImagedal()
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
				btnDelete.Attributes.Add("onclick", "if (!ew_ConfirmMultiDelete('Do you want to delete the selected records?','" & btnDelete.UniqueID & "', '" & tableId & "', " & Convert.ToString(firstRowOffset) & ", " & Convert.ToString(lastRowOffset) & ")) return false;")
			End If
		End If
	End Sub

	' ******************************
	' *  Handler for Page PreRender
	' ******************************

	Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs)
		If PageImageGridView.Rows.Count = 0 Then
			If lblMessage.Text = "" Then
				lblMessage.Text = "No records found"
				pnlMessage.Visible = True
			End If
		End If
	End Sub

	' ***********************************
	' *  Handler for delete button click
	' ***********************************

	Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim sDataObjectTypeName As String = PageImageDataSource.DataObjectTypeName
		PageImageDataSource.DataObjectTypeName = ""
		PageImageDataSource.Delete() ' delete records
		PageImageDataSource.DataObjectTypeName = sDataObjectTypeName
	End Sub

	' Dim iRecordCount As Integer = 0
	' ********************
	' *  Get record count
	' ********************

	Private Function ewRecordCount() As Integer
		Dim iRecordCount As Integer = 0
		Try

			'If (iRecordCount = 0)
				Dim data As PageImagedal = New PageImagedal()
				iRecordCount = data.GetRowsCount(PageImageinf.GetUserFilter()) 

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
		Dim data As PageImagedal = New PageImagedal()
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
		Dim data As PageImagedal = New PageImagedal()

		' Set up Advanced Search
		If (Request.QueryString.Count > 0) Then
			data.ClearSearch() ' Clear Previous Search

			' Set up search parameters
			Dim vx_PageID As String = Request.QueryString("x_PageID")
			Dim vz_PageID As String = Request.QueryString("z_PageID")
			If (Not String.IsNullOrEmpty(vx_PageID) AndAlso vz_PageID IsNot Nothing) Then
				data.AdvancedSearchParm1.PageID = Convert.ToInt32(vx_PageID)
				data.AdvancedSearchParm1.Attribute("PageID").SearchType = Core.GetAdvancedSearchType(vz_PageID)
			End If

			' Set up search parameters
			Dim vx_ImageID As String = Request.QueryString("x_ImageID")
			Dim vz_ImageID As String = Request.QueryString("z_ImageID")
			If (Not String.IsNullOrEmpty(vx_ImageID) AndAlso vz_ImageID IsNot Nothing) Then
				data.AdvancedSearchParm1.ImageID = Convert.ToInt32(vx_ImageID)
				data.AdvancedSearchParm1.Attribute("ImageID").SearchType = Core.GetAdvancedSearchType(vz_ImageID)
			End If

			' Set up search parameters
			Dim vx_PageImagePosition As String = Request.QueryString("x_PageImagePosition")
			Dim vz_PageImagePosition As String = Request.QueryString("z_PageImagePosition")
			If (Not String.IsNullOrEmpty(vx_PageImagePosition) AndAlso vz_PageImagePosition IsNot Nothing) Then
				data.AdvancedSearchParm1.PageImagePosition = Convert.ToInt32(vx_PageImagePosition)
				data.AdvancedSearchParm1.Attribute("PageImagePosition").SearchType = Core.GetAdvancedSearchType(vz_PageImagePosition)
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
		Dim oInfo As PageImageinf = New PageImageinf()
		Dim sThisSort As String = String.Empty
		Dim sLastSort As String = String.Empty
		Dim sSortField As String = String.Empty
		Dim i As Integer = 1
		Do While (i <= oInfo.TableInfo.Count)
			Dim sSortParm As String = oInfo.TableInfo.Fields(i).SortParm
			Dim fldProfile As FieldProfile = CustomProfile.GetField(Share.ProjectName, PageImageinf.TableVar, sSortParm)
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
		If (String.IsNullOrEmpty(sOrderBy)) Then
			SetupDefaultOrderBy()
		End If
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
		Dim oInfo As PageImageinf = New PageImageinf()
		Dim aryDefaultOrder As String() = oInfo.TableInfo.DefaultOrderBy.Split(","c)
		Dim i As Integer = 1
		Do While (i < oInfo.TableInfo.Count)
			Dim sSortParm As String = oInfo.TableInfo.Fields(i).SortParm
			Dim sFieldName As String = oInfo.TableInfo.Fields(i).FieldName
			Dim fldProfile As FieldProfile = CustomProfile.GetField(Share.ProjectName, PageImageinf.TableVar, sSortParm)
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

	Protected Sub PageImageGridView_PageIndexChanged(ByVal sender As Object,ByVal e As System.EventArgs)
		objProfile.PageIndex = PageImageGridView.PageIndex ' Save page index
	End Sub

	' ***********************************
	' *  Handler for GridView RowCreated
	' ***********************************

	Protected Sub PageImageGridView_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
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
						Dim sSort As String = CustomProfile.GetField(Share.ProjectName, PageImageinf.TableVar, button.CommandArgument).Sort
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

	Protected Sub PageImageGridView_RowUpdating(ByVal sender As Object, Byval e As GridViewUpdateEventArgs)
		Dim source As WebControl = EW.Data.Core.GetGridViewEditRow(CType(sender, GridView))
		key.PageImageID = Convert.ToInt32(e.Keys("PageImageID")) ' setup key value
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

	Protected Sub PageImageGridView_RowUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdatedEventArgs)
		If (e.AffectedRows = 0) Then

			' Keep in edit mode
			e.KeepInEditMode = True

			' Get original values
			Dim source As WebControl = EW.Data.Core.GetGridViewEditRow(TryCast(sender, GridView))
			If (source Is Nothing) Then Return
			Dim oldrow As PageImagerow = New PageImagerow()
			ControlToRow(oldrow, source)

			' Synchronize to database 
			Try
				PageImageGridView.DataBind()
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
		PageImageGridView.EditIndex = -1
	End Sub

	' *************************
	' *  Reset Page Properties
	' *************************

	Private Sub ResetPageProperties()

		' Clear Page Index
		objProfile.PageIndex = 0
		PageImageGridView.PageIndex = 0
	End Sub

	' ************
	' *  Bind Data
	' ************

	Protected Sub BindData()
		Dim sWhere As String = GetUserFilter()
		Dim sOrderBy As String = String.Empty
		Dim data As PageImagedal = New PageImagedal()
		If (objProfile.PageIndex < 0) Then
			objProfile.PageIndex = 0
		End If
		PageImageGridView.AllowPaging = objProfile.AllowPaging
		If (objProfile.PageSize > 0) Then
			PageImageGridView.PageSize = objProfile.PageSize 
		End If
		If (Not objProfile.IsCollapsed) Then

			' Try to restore page index
			Try
				Dim intPageCount As Integer = data.GetPageCount(sWhere, PageImageGridView.PageSize)
				If (PageImageGridView.PageSize > 0 AndAlso intPageCount < objProfile.PageIndex + 1) Then objProfile.PageIndex = IIf(intPageCount > 0, intPageCount - 1, 0)
				PageImageGridView.PageIndex = objProfile.PageIndex
			Catch e As Exception
				ResetPageProperties()
			End Try
		Else
			PageImageGridView.PageIndex = 0
		End If
		Try
			PageImageGridView.DataBind()
			If (PageImageGridView.Rows.Count = 0) Then
				pnlPager.Visible = False ' Hide Pager
				btnDelete.Visible = False ' Hide Delete Button
				btnExpand.Visible = False ' Hide Expand Button
			Else
			If (Not isExport) Then
				Dim isCollapse As Boolean= objProfile.IsCollapsed
				SetupControlsVisible(Not isCollapse)
				pnlPager.Visible = Not isCollapse ' Show Pager
				btnDelete.Visible = Not isCollapse ' Show Delete Button
				btnExpand.Visible = isCollapse
			End If
			End If
		Catch e As Exception
			pnlPager.Visible = False ' Hide Pager
			btnDelete.Visible = False ' Hide Delete Button
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

	Protected Sub PageImageGridView_Init(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			PageImagedal.OpenConnection()
		Catch err As Exception
			pnlPager.Visible = False ' Hide Pager
			btnDelete.Visible = false ' Hide Delete Button
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

	Protected Sub PageImageGridView_Load(ByVal sender As Object, ByVal e As System.EventArgs)
		If (isExport) Then
			SetupControlsVisible(False)
			PageImageGridView.PagerSettings.Visible = False
			Return
		End If
	End Sub

	' *************************************
	' *  Handler for GridView Unload
	' *************************************

	Protected Sub PageImageGridView_Unload(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			PageImagedal.CloseAndDisposeConnection()
		Catch
		End Try
	End Sub

	' *************************************
	' *  Handler for GridView RowDataBound
	' *************************************

	Protected Sub PageImageGridView_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
		tableId = TryCast(sender, GridView).ClientID
		If (PageImageGridView.PageCount > 1 AndAlso PageImageGridView.PagerSettings.Visible) Then
			If (PageImageGridView.PagerSettings.Position = PagerPosition.Top OrElse PageImageGridView.PagerSettings.Position = PagerPosition.TopAndBottom) Then
				firstRowOffset = initialFirstRowOffset + 1
			End If
			If (PageImageGridView.PagerSettings.Position = PagerPosition.Bottom OrElse PageImageGridView.PagerSettings.Position = PagerPosition.TopAndBottom) Then
				lastRowOffset = initialLastRowOffset + 1
			End If
		End If
		If (e.Row.RowType = DataControlRowType.Header) Then
			For Each cell As TableCell In e.Row.Cells
				If (cell.HasControls() AndAlso cell.FindControl("DeleteAll") IsNot Nothing) Then
					Dim ctrl As HtmlInputCheckBox = TryCast(cell.FindControl("DeleteAll"),HtmlInputCheckBox)
					ctrl.Attributes.Add("onclick", "ew_SelectAll('$Delete', this, '" & tableId & "', " & firstRowOffset & ", " & lastRowOffset & ", false);")
					Exit For
				End If
			Next
		ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
			Dim row As PageImagerow = TryCast(e.Row.DataItem, PageImagerow) ' get row object
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
				EditLink.NavigateUrl = String.Format("pageimage_edit.aspx?PageImageID={0}", Server.UrlEncode(Convert.ToString(row.PageImageID)))
			Dim DeleteCheckBox As HtmlInputCheckBox = TryCast(control.FindControl("DeleteCheckBox"), HtmlInputCheckBox)
			If (DeleteCheckBox IsNot Nothing) Then _
				DeleteCheckBox.Value = Convert.ToString(row.PageImageID)
		ElseIf (e.Row.RowType = DataControlRowType.Footer) Then
			Dim control As GridViewRow = e.Row ' get control object
		End If
	End Sub

	' **********************************
	' *  Handler for GridView DataBound
	' **********************************

	Protected Sub PageImageGridView_DataBound(ByVal sender As Object, ByVal e As System.EventArgs)
	End Sub

	' ***********************************
	' *  Handler for GridView RowCommand
	' ***********************************

	Protected Sub PageImageGridView_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)
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
		PageImageGridView.Columns(1).Visible = visible
		PageImageGridView.Columns(0).Visible = visible
		lnkAdd.Visible = visible
		PageImageGridView.Columns(2).Visible = visible
		btnDelete.Visible = visible
		PageImageGridView.AllowSorting = visible
	End Sub

	' ********************************
	' * Register ClientID of Controls
	' ********************************

	Private Sub RegisterClientID(ByVal sBlockName As String, ByVal control As Control)
		Dim jsString As String = String.Empty
		If (control.FindControl("x_PageID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_PageID'] = '" & control.FindControl("x_PageID").ClientID & "';"
		End If
		If (control.FindControl("x_ImageID") IsNot Nothing) Then
			jsString &= "ew.Controls['x_ImageID'] = '" & control.FindControl("x_ImageID").ClientID & "';"
		End If
		If (control.FindControl("x_PageImagePosition") IsNot Nothing) Then
			jsString &= "ew.Controls['x_PageImagePosition'] = '" & control.FindControl("x_PageImagePosition").ClientID & "';"
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
			Dim x_PageID As DropDownList = TryCast(control.FindControl("x_PageID"), DropDownList)
			If (x_PageID IsNot Nothing) Then
				If ((x_PageID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_PageID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Page")
				ElseIf (String.IsNullOrEmpty(x_PageID.SelectedValue)) Then
					messageList.Add("Please enter required field (Int32): Page")
				End If
			End If
			Dim x_ImageID As DropDownList = TryCast(control.FindControl("x_ImageID"), DropDownList)
			If (x_ImageID IsNot Nothing) Then
				If ((x_ImageID.SelectedIndex <= 0) AndAlso (Not DataFormat.CheckInt32(x_ImageID.SelectedValue))) Then
					messageList.Add("Invalid Value (Int32): Image")
				ElseIf (String.IsNullOrEmpty(x_ImageID.SelectedValue)) Then
					messageList.Add("Please enter required field (Int32): Image")
				End If
			End If
			Dim x_PageImagePosition As TextBox = TryCast(control.FindControl("x_PageImagePosition"), TextBox)
			If (x_PageImagePosition IsNot Nothing) Then
				If (Not DataFormat.CheckInt32(x_PageImagePosition.Text)) Then
					messageList.Add("Invalid Value (Int32): Position")
				End If
			End If
		End If
		Return IIf(messageList.Count = 0, Nothing, messageList)
	End Function

	' **********************************************
	' *  Convert Input Control Values to Row Object
	' **********************************************

	Private Sub ControlToRow(ByVal row As PageImagerow, ByVal control As WebControl)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		row.SetPageType(Core.PageType.List) ' set page type = list for update

		' Field PageID
		Dim x_PageID As DropDownList = TryCast(control.FindControl("x_PageID"), DropDownList)
		Dim v_PageID As String = String.Empty
		If (x_PageID.SelectedIndex >= 0) Then
			For Each li As ListItem In x_PageID.Items
				If (li.Selected) Then
					v_PageID += li.Value
					Exit For
				End If
			Next
		End If
		If (v_PageID <> String.Empty) Then row.PageID = Convert.ToInt32(v_PageID) Else row.PageID = CType(Nothing, Nullable(Of Int32))

		' Field ImageID
		Dim x_ImageID As DropDownList = TryCast(control.FindControl("x_ImageID"), DropDownList)
		Dim v_ImageID As String = String.Empty
		If (x_ImageID.SelectedIndex >= 0) Then
			For Each li As ListItem In x_ImageID.Items
				If (li.Selected) Then
					v_ImageID += li.Value
					Exit For
				End If
			Next
		End If
		If (v_ImageID <> String.Empty) Then row.ImageID = Convert.ToInt32(v_ImageID) Else row.ImageID = CType(Nothing, Nullable(Of Int32))

		' Field PageImagePosition
		Dim x_PageImagePosition As TextBox = TryCast(control.FindControl("x_PageImagePosition"), TextBox)
		If (x_PageImagePosition.Text <> String.Empty) Then row.PageImagePosition = Convert.ToInt32(x_PageImagePosition.Text) Else row.PageImagePosition = CType(Nothing, Nullable(Of Int32))
	End Sub

	' ****************************
	' *  Check for duplicate Key
	' ****************************

	Private Function CheckDuplicateKey(ByVal control As WebControl) As ArrayList 
		Dim sWhere As String = String.Empty
		Dim messageList As ArrayList = New ArrayList()
		Dim data As PageImagedal = New PageImagedal()
		Dim newkey As PageImagekey = New PageImagekey()
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

	Private Sub RowToControl(ByVal row As PageImagerow, ByVal control As WebControl, ByVal ct As Core.CtrlType)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		If (ct = Core.CtrlType.View) Then ' set up view controls

			' Field PageID
			Dim x_PageID As Label = TryCast(control.FindControl("x_PageID"), Label)
			Dim v_PageID As String
			If (row.PageID.HasValue) Then v_PageID = row.PageID.ToString() Else v_PageID = String.Empty
			x_PageID.Text = PageImagedal.LookUpTable("PageID", v_PageID)

			' Field ImageID
			Dim x_ImageID As Label = TryCast(control.FindControl("x_ImageID"), Label)
			Dim v_ImageID As String
			If (row.ImageID.HasValue) Then v_ImageID = row.ImageID.ToString() Else v_ImageID = String.Empty
			x_ImageID.Text = PageImagedal.LookUpTable("ImageID", v_ImageID)

			' Field PageImagePosition
			Dim x_PageImagePosition As Label = TryCast(control.FindControl("x_PageImagePosition"), Label)
			If (row.PageImagePosition.HasValue) Then x_PageImagePosition.Text = row.PageImagePosition.ToString() Else x_PageImagePosition.Text = String.Empty
		End If
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
			x_PageID.ClearSelection()
			For Each li As ListItem In x_PageID.Items
				If (li.Value.ToString() = v_PageID) Then
					li.Selected = True
					Exit For
				End If
			Next

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
			x_ImageID.ClearSelection()
			For Each li As ListItem In x_ImageID.Items
				If (li.Value.ToString() = v_ImageID) Then
					li.Selected = True
					Exit For
				End If
			Next

			' Field PageImagePosition
			Dim x_PageImagePosition As TextBox = TryCast(control.FindControl("x_PageImagePosition"), TextBox)
			If (row.PageImagePosition.HasValue) Then x_PageImagePosition.Text = row.PageImagePosition.ToString() Else x_PageImagePosition.Text = String.Empty
			Page.Form.DefaultFocus = control.FindControl("x_PageID").ClientID
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

	Protected Sub PageImageDataSource_Selecting(Byval sender As Object, ByVal e As ObjectDataSourceSelectingEventArgs)
		e.InputParameters.Clear()
		e.InputParameters.Add("filter", PageImageinf.GetUserFilter())
	End Sub

	' *************************************
	' *  DataSource Selected Event Handler
	' *************************************

	Protected Sub PageImageDataSource_Selected(Byval sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
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

	Protected Sub PageImageDataSource_Updating(ByVal sender As Object, ByVal e As ObjectDataSourceMethodEventArgs)
		Dim source As WebControl = EW.Data.Core.GetGridViewEditRow(PageImageGridView)
		If (source Is Nothing) Then
			e.Cancel = True
			Return
		End If
		Dim sTmp As String = String.Empty

		' Set up row object
		Dim row As PageImagerow = TryCast(e.InputParameters(0), PageImagerow)
		Dim data As PageImagedal = New PageImagedal()
		oldrow = data.LoadRow(key, PageImageinf.GetUserFilter())
		ControlToRow(row, source) ' load values of controls to row
		newrow = row
		If (PageImagebll.Updating(oldrow, newrow)) Then
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

	Protected Sub PageImageDataSource_Updated(ByVal sender As Object, ByVal e As ObjectDataSourceStatusEventArgs)
		If (e.Exception IsNot Nothing) Then
			e.AffectedRows = 0
			e.ExceptionHandled = True
			lblMessage.Text = e.Exception.InnerException.Message
		Else
			lblMessage.Text = "Update successful"
			PageImagebll.Updated(oldrow, newrow)
		End If
		If (lblMessage.Text <> "") Then pnlMessage.Visible = True
	End Sub

	' ***********************************
	' *  Handler for DataSource Deleting
	' ***********************************

	Protected Sub PageImageDataSource_Deleting(ByVal sender As Object, ByVal e As ObjectDataSourceMethodEventArgs)
		Dim data As PageImagedal = New PageImagedal()
		Dim sWhere As String = String.Empty
		For Each row As GridViewRow In PageImageGridView.Rows

			' Get the HtmlInputChecklBox control from the cells control collection
			Dim checkBox As HtmlInputCheckBox = TryCast(row.FindControl("DeleteCheckBox"), HtmlInputCheckBox)
			If (checkBox IsNot Nothing AndAlso checkBox.Checked) Then
				Dim arrKey As String() = checkBox.Value.Split(vbCr)
				Dim key As PageImagekey = New PageImagekey()
				key.PageImageID = Convert.ToInt32(arrKey(0))
				arrKeys.Add(key)
				oldrows.Add(data.LoadRow(key, sWhere))
			End If
		Next
		If (PageImagebll.Deleting(oldrows)) Then
			e.InputParameters.Clear()
			e.InputParameters.Add("keys", arrKeys)
		Else
			e.Cancel = True
			If (Not String.IsNullOrEmpty(objProfile.Message)) Then
				lblMessage.Text = objProfile.Message
				pnlMessage.Visible = True
			End If
		End If
	End Sub

	' **********************************
	' *  Handler for DataSource Deleted
	' **********************************

	Protected Sub PageImageDataSource_Deleted(ByVal sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
		If (e.Exception IsNot Nothing) Then
			e.ExceptionHandled = True
			lblMessage.Text = e.Exception.InnerException.Message
		Else
			objProfile.Message = "Delete Successful"
			PageImagebll.Deleted(oldrows)
			lblMessage.Text = objProfile.Message
			BindData()
		End If
		If (lblMessage.Text <> "") Then
			pnlMessage.Visible = True
		End If
	End Sub

	' *****************
	' *  Load Controls 
	' *****************

	Private Sub LoadSearchControls(ByVal control As Control) 
		Dim sSearchType As String = String.Empty
		Dim data As PageImagedal = New PageImagedal()
	End Sub

	' *******************
	' *  Get User Filter
	' *******************

	Private Function GetUserFilter() As String 
		Return PageImageinf.GetUserFilter()
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
				Response.AddHeader("Content-Disposition", "attachment; filename=""PageImage.xls""")
				Export("excel")
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
					PageImageGridView.Columns(1).Visible = False
					PageImageGridView.Columns(0).Visible = False
					lnkAdd.Visible = False
					PageImageGridView.Columns(2).Visible = False
					btnDelete.Visible = False
					PageImageGridView.PagerSettings.Visible = False
					PageImageGridView.AllowSorting = False
					BindData()
					PrepareGridViewForExport(PageImageGridView)

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
					htw.WriteEncodedText("TABLE: Page Image")
					htw.RenderEndTag()

					' Render Table
					PageImageGridView.RenderControl(htw)

					' Render Body End Tag
					htw.RenderEndTag()

					' Render Html End Tag
					htw.RenderEndTag()
					Response.Write(sw.ToString())
				End Using
			End Using
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
	<span class="aspnetmaker">Page Image</span>
	<asp:PlaceHolder ID="pnlExport" runat="server">
		<asp:HyperLink CssClass="aspnetmaker" ID="lnkExportExcel" runat="server" NavigateUrl="pageimage_list.aspx?Export=excel">Export to Excel</asp:HyperLink>&nbsp;&nbsp;
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
	<asp:ValidationSummary id="xevs_PageImage" CssClass="aspnetmaker" runat="server"
		HeaderText=""
		ShowSummary="False"
		ShowMessageBox="True"
		ForeColor="#FF0000" />
	<asp:PlaceHolder ID="pnlSearch" runat="server">
		<asp:Panel runat="server">
			<p />
			<asp:Table runat="server" BorderWidth="0" CellSpacing="1" CellPadding="4" ID="mh_PageImage">
				<asp:TableRow>
					<asp:TableCell>
						<asp:Table runat="server">
						</asp:Table>
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow>
					<asp:TableCell>
	<asp:LinkButton ID="btnClearSearch" OnClick="btnClearSearch_Click" runat="server" 			CssClass="aspnetmaker" Text="Show all"></asp:LinkButton>&nbsp;<asp:HyperLink ID="lnkSearch" runat="server" CssClass="aspnetmaker" Text="Advanced Search" NavigateUrl="pageimage_search.aspx"></asp:HyperLink>
					</asp:TableCell>
				</asp:TableRow>
			</asp:Table>
		</asp:Panel>
	</asp:PlaceHolder>
	<p><asp:HyperLink ID="lnkAdd" runat="server" CssClass="aspnetmaker" NavigateUrl="pageimage_add.aspx">Add</asp:HyperLink></p>
	<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
		<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
	</asp:PlaceHolder>
	<p><asp:Label id="lblReturnUrl" Visible="False" Text="pageimage_list.aspx" CssClass="aspnetmaker" runat="server" />
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
<asp:ObjectDataSource ID="PageImageDataSource"
	TypeName="PMGEN.PageImagedal"
	DataObjectTypeName="PMGEN.PageImagerow"
	SelectMethod="LoadList"
	UpdateMethod="Update"
	DeleteMethod="Delete"
	OnSelecting="PageImageDataSource_Selecting"
	OnSelected="PageImageDataSource_Selected"
	OnUpdating="PageImageDataSource_Updating"
	OnUpdated="PageImageDataSource_Updated"
	OnDeleting="PageImageDataSource_Deleting"
	OnDeleted="PageImageDataSource_Deleted"
	EnablePaging="True"
	SelectCountMethod="GetRowsCount"
	MaximumRowsParameterName="PageSize"
	StartRowIndexParameterName="StartRow"
	runat="server">
</asp:ObjectDataSource>
	<asp:GridView ID="PageImageGridView"
		PageSize="50"
		DataKeyNames="PageImageID"
		DataSourceID="PageImageDataSource"
		GridLines="None"
		AutoGenerateColumns="False" CssClass="ewTable"
		AllowSorting="True" AllowPaging="True"
		OnInit="PageImageGridView_Init"
		OnDataBound="PageImageGridView_DataBound"
		OnRowCommand="PageImageGridView_RowCommand"
		OnRowUpdating="PageImageGridView_RowUpdating"
		OnRowUpdated="PageImageGridView_RowUpdated"
		OnRowDataBound="PageImageGridView_RowDataBound"
		OnSorting="Sorting"
		OnRowCreated="PageImageGridView_RowCreated"
		OnPageIndexChanged="PageImageGridView_PageIndexChanged"
		OnLoad="PageImageGridView_Load"
		OnUnload="PageImageGridView_Unload"
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
			<HeaderTemplate>
				<input id="DeleteAll" type="checkbox" runat="server" />
			</HeaderTemplate>
			<ItemTemplate>
				<input id="DeleteCheckBox" name="key_d" type="checkbox" runat="server" /><span class="aspnetmaker">Delete</span>
			</ItemTemplate>
		</asp:TemplateField>
			<asp:TemplateField>
				<ItemStyle Wrap="False" />
				<HeaderTemplate>
					<asp:LinkButton runat='server' id="xs_PageID"  CssClass="ewTableHeader" CommandName="Sort" CommandArgument="PageID">Page </asp:LinkButton>
				</HeaderTemplate>
				<ItemTemplate>
<asp:Label id="x_PageID" CssClass="aspnetmaker" runat="server" />
				</ItemTemplate>
				<EditItemTemplate>
<asp:DropDownList ID="x_PageID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
<asp:RequiredFieldValidator ID="vx_PageID" CssClass="aspnetmaker" runat="server" ControlToValidate="x_PageID" ErrorMessage="Please enter required field - Page" ForeColor="Red" Display="None" Text="*" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<ItemStyle Wrap="True" />
				<HeaderTemplate>
					<asp:LinkButton runat='server' id="xs_ImageID"  CssClass="ewTableHeader" CommandName="Sort" CommandArgument="ImageID">Image </asp:LinkButton>
				</HeaderTemplate>
				<ItemTemplate>
<asp:Label id="x_ImageID" CssClass="aspnetmaker" runat="server" />
				</ItemTemplate>
				<EditItemTemplate>
<asp:DropDownList ID="x_ImageID" CssClass="aspnetmaker" runat="server" >
</asp:DropDownList>
<asp:RequiredFieldValidator ID="vx_ImageID" CssClass="aspnetmaker" runat="server" ControlToValidate="x_ImageID" ErrorMessage="Please enter required field - Image" ForeColor="Red" Display="None" Text="*" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<ItemStyle Wrap="False" />
				<HeaderTemplate>
					<asp:LinkButton runat='server' id="xs_PageImagePosition"  CssClass="ewTableHeader" CommandName="Sort" CommandArgument="PageImagePosition">Position </asp:LinkButton>
				</HeaderTemplate>
				<ItemTemplate>
<asp:Label id="x_PageImagePosition" CssClass="aspnetmaker" runat="server" />
				</ItemTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_PageImagePosition" Columns="30" CssClass="aspnetmaker" runat="server" />
<ew:IntegerValidator ID="vc_PageImagePosition" ClientValidationFunction="ew_CheckInteger" CssClass="aspnetmaker" runat="server" ControlToValidate="x_PageImagePosition" ErrorMessage="Incorrect integer - Position" Display="None" ForeColor="Red" />
				</EditItemTemplate>
			</asp:TemplateField>
		</Columns>
	</asp:GridView>
	<br />
	<asp:Button ID="btnDelete" Runat="server" Text="DELETE SELECTED" OnClick="btnDelete_Click" UseSubmitBehavior="false" />
</asp:Content>
