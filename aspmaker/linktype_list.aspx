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
	Dim key As LinkTypekey = New LinkTypekey()  ' record key
	Dim oldrow As LinkTyperow ' old record data input by user
	Dim newrow As LinkTyperow ' new record data input by user
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
		objProfile = CustomProfile.GetTable(Share.ProjectName, LinkTypeinf.TableVar)
	End Sub

	' *************************
	' *  Handler for Page Load
	' *************************

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
                Session("ListPageURL") = HttpContext.Current.Request.FilePath()
		lblMessage.Text = String.Empty
		Dim isAuth As Boolean = False
		Page.MaintainScrollPositionOnPostBack = False
		tableId = LinkTypeGridView.ClientID
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
				Dim sCmd As String = Request.QueryString("cmd")
				If (sCmd = "resetall") Then
					objProfile.AllowSorting = True
					Dim data As LinkTypedal = New LinkTypedal()
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
		If LinkTypeGridView.Rows.Count = 0 Then
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
				Dim data As LinkTypedal = New LinkTypedal()
				iRecordCount = data.GetRowsCount(LinkTypeinf.GetUserFilter()) 

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

	' ******************
	' *  Handle sorting
	' ******************

	Protected Sub Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
		Dim sOrderBy As String = e.SortExpression
		Dim oInfo As LinkTypeinf = New LinkTypeinf()
		Dim sThisSort As String = String.Empty
		Dim sLastSort As String = String.Empty
		Dim sSortField As String = String.Empty
		Dim i As Integer = 1
		Do While (i <= oInfo.TableInfo.Count)
			Dim sSortParm As String = oInfo.TableInfo.Fields(i).SortParm
			Dim fldProfile As FieldProfile = CustomProfile.GetField(Share.ProjectName, LinkTypeinf.TableVar, sSortParm)
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
		Dim oInfo As LinkTypeinf = New LinkTypeinf()
		Dim aryDefaultOrder As String() = oInfo.TableInfo.DefaultOrderBy.Split(","c)
		Dim i As Integer = 1
		Do While (i < oInfo.TableInfo.Count)
			Dim sSortParm As String = oInfo.TableInfo.Fields(i).SortParm
			Dim sFieldName As String = oInfo.TableInfo.Fields(i).FieldName
			Dim fldProfile As FieldProfile = CustomProfile.GetField(Share.ProjectName, LinkTypeinf.TableVar, sSortParm)
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

	Protected Sub LinkTypeGridView_PageIndexChanged(ByVal sender As Object,ByVal e As System.EventArgs)
		objProfile.PageIndex = LinkTypeGridView.PageIndex ' Save page index
	End Sub

	' ***********************************
	' *  Handler for GridView RowCreated
	' ***********************************

	Protected Sub LinkTypeGridView_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
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
						Dim sSort As String = CustomProfile.GetField(Share.ProjectName, LinkTypeinf.TableVar, button.CommandArgument).Sort
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

	Protected Sub LinkTypeGridView_RowUpdating(ByVal sender As Object, Byval e As GridViewUpdateEventArgs)
		Dim source As WebControl = EW.Data.Core.GetGridViewEditRow(CType(sender, GridView))
		key.LinkTypeCD = Convert.ToString(e.Keys("LinkTypeCD")) ' setup key value
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

	Protected Sub LinkTypeGridView_RowUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdatedEventArgs)
		If (e.AffectedRows = 0) Then

			' Keep in edit mode
			e.KeepInEditMode = True

			' Get original values
			Dim source As WebControl = EW.Data.Core.GetGridViewEditRow(TryCast(sender, GridView))
			If (source Is Nothing) Then Return
			Dim oldrow As LinkTyperow = New LinkTyperow()
			ControlToRow(oldrow, source)

			' Synchronize to database 
			Try
				LinkTypeGridView.DataBind()
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
		LinkTypeGridView.EditIndex = -1
	End Sub

	' *************************
	' *  Reset Page Properties
	' *************************

	Private Sub ResetPageProperties()

		' Clear Page Index
		objProfile.PageIndex = 0
		LinkTypeGridView.PageIndex = 0
	End Sub

	' ************
	' *  Bind Data
	' ************

	Protected Sub BindData()
		Dim sWhere As String = GetUserFilter()
		Dim sOrderBy As String = String.Empty
		Dim data As LinkTypedal = New LinkTypedal()
		If (objProfile.PageIndex < 0) Then
			objProfile.PageIndex = 0
		End If
		LinkTypeGridView.AllowPaging = objProfile.AllowPaging
		If (objProfile.PageSize > 0) Then
			LinkTypeGridView.PageSize = objProfile.PageSize 
		End If
		If (Not objProfile.IsCollapsed) Then

			' Try to restore page index
			Try
				Dim intPageCount As Integer = data.GetPageCount(sWhere, LinkTypeGridView.PageSize)
				If (LinkTypeGridView.PageSize > 0 AndAlso intPageCount < objProfile.PageIndex + 1) Then objProfile.PageIndex = IIf(intPageCount > 0, intPageCount - 1, 0)
				LinkTypeGridView.PageIndex = objProfile.PageIndex
			Catch e As Exception
				ResetPageProperties()
			End Try
		Else
			LinkTypeGridView.PageIndex = 0
		End If
		Try
			LinkTypeGridView.DataBind()
			If (LinkTypeGridView.Rows.Count = 0) Then
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

	Protected Sub LinkTypeGridView_Init(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			LinkTypedal.OpenConnection()
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

	Protected Sub LinkTypeGridView_Load(ByVal sender As Object, ByVal e As System.EventArgs)
		If (isExport) Then
			SetupControlsVisible(False)
			LinkTypeGridView.PagerSettings.Visible = False
			Return
		End If
	End Sub

	' *************************************
	' *  Handler for GridView Unload
	' *************************************

	Protected Sub LinkTypeGridView_Unload(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			LinkTypedal.CloseAndDisposeConnection()
		Catch
		End Try
	End Sub

	' *************************************
	' *  Handler for GridView RowDataBound
	' *************************************

	Protected Sub LinkTypeGridView_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
		tableId = TryCast(sender, GridView).ClientID
		If (LinkTypeGridView.PageCount > 1 AndAlso LinkTypeGridView.PagerSettings.Visible) Then
			If (LinkTypeGridView.PagerSettings.Position = PagerPosition.Top OrElse LinkTypeGridView.PagerSettings.Position = PagerPosition.TopAndBottom) Then
				firstRowOffset = initialFirstRowOffset + 1
			End If
			If (LinkTypeGridView.PagerSettings.Position = PagerPosition.Bottom OrElse LinkTypeGridView.PagerSettings.Position = PagerPosition.TopAndBottom) Then
				lastRowOffset = initialLastRowOffset + 1
			End If
		End If
		If (e.Row.RowType = DataControlRowType.Header) Then
		ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
			Dim row As LinkTyperow = TryCast(e.Row.DataItem, LinkTyperow) ' get row object
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
			Dim DeleteLink As HyperLink = TryCast(control.FindControl("DeleteLink"), HyperLink)
			If (DeleteLink IsNot Nothing) Then _
				DeleteLink.NavigateUrl = String.Format("linktype_delete.aspx?LinkTypeCD={0}", Server.UrlEncode(Convert.ToString(row.LinkTypeCD)))
		ElseIf (e.Row.RowType = DataControlRowType.Footer) Then
			Dim control As GridViewRow = e.Row ' get control object
		End If
	End Sub

	' **********************************
	' *  Handler for GridView DataBound
	' **********************************

	Protected Sub LinkTypeGridView_DataBound(ByVal sender As Object, ByVal e As System.EventArgs)
	End Sub

	' ***********************************
	' *  Handler for GridView RowCommand
	' ***********************************

	Protected Sub LinkTypeGridView_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)
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
		LinkTypeGridView.Columns(0).Visible = visible
		lnkAdd.Visible = visible
		LinkTypeGridView.Columns(1).Visible = visible
		LinkTypeGridView.AllowSorting = visible
	End Sub

	' ********************************
	' * Register ClientID of Controls
	' ********************************

	Private Sub RegisterClientID(ByVal sBlockName As String, ByVal control As Control)
		Dim jsString As String = String.Empty
		If (control.FindControl("x_LinkTypeCD") IsNot Nothing) Then
			jsString &= "ew.Controls['x_LinkTypeCD'] = '" & control.FindControl("x_LinkTypeCD").ClientID & "';"
		End If
		If (control.FindControl("x_LinkTypeDesc") IsNot Nothing) Then
			jsString &= "ew.Controls['x_LinkTypeDesc'] = '" & control.FindControl("x_LinkTypeDesc").ClientID & "';"
		End If
		If (control.FindControl("x_LinkTypeTarget") IsNot Nothing) Then
			jsString &= "ew.Controls['x_LinkTypeTarget'] = '" & control.FindControl("x_LinkTypeTarget").ClientID & "';"
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
			Dim x_LinkTypeDesc As TextBox = TryCast(control.FindControl("x_LinkTypeDesc"), TextBox)
			If (x_LinkTypeDesc IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_LinkTypeDesc.Text)) Then
					messageList.Add("Invalid Value (String): Link Type Desc")
				End If
			End If
			Dim x_LinkTypeTarget As TextBox = TryCast(control.FindControl("x_LinkTypeTarget"), TextBox)
			If (x_LinkTypeTarget IsNot Nothing) Then
				If (Not DataFormat.CheckString(x_LinkTypeTarget.Text)) Then
					messageList.Add("Invalid Value (String): Link Type Target")
				End If
			End If
		End If
		Return IIf(messageList.Count = 0, Nothing, messageList)
	End Function

	' **********************************************
	' *  Convert Input Control Values to Row Object
	' **********************************************

	Private Sub ControlToRow(ByVal row As LinkTyperow, ByVal control As WebControl)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		row.SetPageType(Core.PageType.List) ' set page type = list for update

		' Field LinkTypeCD
		Dim x_LinkTypeCD As TextBox = TryCast(control.FindControl("x_LinkTypeCD"), TextBox)
		If (x_LinkTypeCD.Text <> String.Empty) Then row.LinkTypeCD = x_LinkTypeCD.Text Else row.LinkTypeCD = Nothing

		' Field LinkTypeDesc
		Dim x_LinkTypeDesc As TextBox = TryCast(control.FindControl("x_LinkTypeDesc"), TextBox)
		If (x_LinkTypeDesc.Text <> String.Empty) Then row.LinkTypeDesc = x_LinkTypeDesc.Text Else row.LinkTypeDesc = Nothing

		' Field LinkTypeTarget
		Dim x_LinkTypeTarget As TextBox = TryCast(control.FindControl("x_LinkTypeTarget"), TextBox)
		If (x_LinkTypeTarget.Text <> String.Empty) Then row.LinkTypeTarget = x_LinkTypeTarget.Text Else row.LinkTypeTarget = Nothing
	End Sub

	' ****************************
	' *  Check for duplicate Key
	' ****************************

	Private Function CheckDuplicateKey(ByVal control As WebControl) As ArrayList 
		Dim sWhere As String = String.Empty
		Dim messageList As ArrayList = New ArrayList()
		Dim data As LinkTypedal = New LinkTypedal()
		Dim newkey As LinkTypekey = New LinkTypekey()
		Try
			Dim sKeyWhere As string = data.KeyFilter(key)

		' Check for duplicate Link Type CD
			sWhere = "([LinkTypeCD] = '" + Db.AdjustSql((CType(control.FindControl("x_LinkTypeCD"),TextBox)).Text) + "' AND NOT (" & sKeyWhere & "))"
			If (data.LoadList(sWhere) IsNot Nothing) Then
				messageList.Add("Duplicate value: Link Type CD")
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

	Private Sub RowToControl(ByVal row As LinkTyperow, ByVal control As WebControl, ByVal ct As Core.CtrlType)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		If (ct = Core.CtrlType.View) Then ' set up view controls

			' Field LinkTypeCD
			Dim x_LinkTypeCD As Label = TryCast(control.FindControl("x_LinkTypeCD"), Label)
			If (row.LinkTypeCD IsNot Nothing) Then x_LinkTypeCD.Text = row.LinkTypeCD.ToString() Else x_LinkTypeCD.Text = String.Empty

			' Field LinkTypeDesc
			Dim x_LinkTypeDesc As Label = TryCast(control.FindControl("x_LinkTypeDesc"), Label)
			If (row.LinkTypeDesc IsNot Nothing) Then x_LinkTypeDesc.Text = row.LinkTypeDesc.ToString() Else x_LinkTypeDesc.Text = String.Empty

			' Field LinkTypeTarget
			Dim x_LinkTypeTarget As Label = TryCast(control.FindControl("x_LinkTypeTarget"), Label)
			If (row.LinkTypeTarget IsNot Nothing) Then x_LinkTypeTarget.Text = row.LinkTypeTarget.ToString() Else x_LinkTypeTarget.Text = String.Empty
		End If
		If (ct = Core.CtrlType.Edit) Then ' set up edit control

			' Field LinkTypeCD
			Dim x_LinkTypeCD As TextBox = TryCast(control.FindControl("x_LinkTypeCD"), TextBox)
			If (row.LinkTypeCD IsNot Nothing) Then x_LinkTypeCD.Text = row.LinkTypeCD.ToString() Else x_LinkTypeCD.Text = String.Empty
			TryCast(control.FindControl("x_LinkTypeCD"), TextBox).Enabled = False

			' Field LinkTypeDesc
			Dim x_LinkTypeDesc As TextBox = TryCast(control.FindControl("x_LinkTypeDesc"), TextBox)
			If (row.LinkTypeDesc IsNot Nothing) Then x_LinkTypeDesc.Text = row.LinkTypeDesc.ToString() Else x_LinkTypeDesc.Text = String.Empty

			' Field LinkTypeTarget
			Dim x_LinkTypeTarget As TextBox = TryCast(control.FindControl("x_LinkTypeTarget"), TextBox)
			If (row.LinkTypeTarget IsNot Nothing) Then x_LinkTypeTarget.Text = row.LinkTypeTarget.ToString() Else x_LinkTypeTarget.Text = String.Empty
			Page.Form.DefaultFocus = control.FindControl("x_LinkTypeDesc").ClientID
		End If
	End Sub

	' ************************************
	' *  Handler for DataSource Selecting
	' ************************************

	Protected Sub LinkTypeDataSource_Selecting(Byval sender As Object, ByVal e As ObjectDataSourceSelectingEventArgs)
		e.InputParameters.Clear()
		e.InputParameters.Add("filter", LinkTypeinf.GetUserFilter())
	End Sub

	' *************************************
	' *  DataSource Selected Event Handler
	' *************************************

	Protected Sub LinkTypeDataSource_Selected(Byval sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
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

	Protected Sub LinkTypeDataSource_Updating(ByVal sender As Object, ByVal e As ObjectDataSourceMethodEventArgs)
		Dim source As WebControl = EW.Data.Core.GetGridViewEditRow(LinkTypeGridView)
		If (source Is Nothing) Then
			e.Cancel = True
			Return
		End If
		Dim sTmp As String = String.Empty

		' Set up row object
		Dim row As LinkTyperow = TryCast(e.InputParameters(0), LinkTyperow)
		Dim data As LinkTypedal = New LinkTypedal()
		oldrow = data.LoadRow(key, LinkTypeinf.GetUserFilter())
		ControlToRow(row, source) ' load values of controls to row
		newrow = row
		If (LinkTypebll.Updating(oldrow, newrow)) Then
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

	Protected Sub LinkTypeDataSource_Updated(ByVal sender As Object, ByVal e As ObjectDataSourceStatusEventArgs)
		If (e.Exception IsNot Nothing) Then
			e.AffectedRows = 0
			e.ExceptionHandled = True
			lblMessage.Text = e.Exception.InnerException.Message
		Else
			lblMessage.Text = "Update successful"
			LinkTypebll.Updated(oldrow, newrow)
		End If
		If (lblMessage.Text <> "") Then pnlMessage.Visible = True
	End Sub

	' *******************
	' *  Get User Filter
	' *******************

	Private Function GetUserFilter() As String 
		Return LinkTypeinf.GetUserFilter()
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
				Response.AddHeader("Content-Disposition", "attachment; filename=""LinkType.xls""")
				Export("excel")
			Case "XML"
				Response.Clear()
				Response.ContentType = "text/xml"
				Response.AddHeader("Content-Disposition", "attachment; filename=""LinkType.xml""")
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
					LinkTypeGridView.Columns(0).Visible = False
					lnkAdd.Visible = False
					LinkTypeGridView.Columns(1).Visible = False
					LinkTypeGridView.PagerSettings.Visible = False
					LinkTypeGridView.AllowSorting = False
					BindData()
					PrepareGridViewForExport(LinkTypeGridView)

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
					htw.WriteEncodedText("TABLE: Link Type")
					htw.RenderEndTag()

					' Render Table
					LinkTypeGridView.RenderControl(htw)

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
			Dim dataSet As LinkTyperows 
			Dim sFldParm As String
			Dim oInfo As LinkTypeinf = New LinkTypeinf()
			Dim nPageSize As Integer = objProfile.PageSize
			Dim nPageIndex As Integer = objProfile.PageIndex
			Dim nStartRec As Integer = nPageSize * nPageIndex
			Dim nStopRec As Integer
			If (nPageSize > 0) Then
				nStopRec = nStartRec + nPageSize
			Else
				nStopRec = Int32.MaxValue ' Show all values
			End If
			Dim data As LinkTypedal = New LinkTypedal()
			dataSet = TryCast(data.LoadList(LinkTypeinf.GetUserFilter()), LinkTyperows)
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
					sFldParm = "LinkTypeCD"
					oXmlField = oXmlDoc.CreateElement(sFldParm)
					If (dataSet(i).LinkTypeCD IsNot Nothing) Then
						oXmlField.InnerText = Convert.ToString(dataSet(i).LinkTypeCD)
					Else
						oXmlField.InnerText = "Null"
					End If
					oXmlRec.AppendChild(oXmlField)
					sFldParm = "LinkTypeDesc"
					oXmlField = oXmlDoc.CreateElement(sFldParm)
					If (dataSet(i).LinkTypeDesc IsNot Nothing) Then
						oXmlField.InnerText = Convert.ToString(dataSet(i).LinkTypeDesc)
					Else
						oXmlField.InnerText = "Null"
					End If
					oXmlRec.AppendChild(oXmlField)
					sFldParm = "LinkTypeTarget"
					oXmlField = oXmlDoc.CreateElement(sFldParm)
					If (dataSet(i).LinkTypeTarget IsNot Nothing) Then
						oXmlField.InnerText = Convert.ToString(dataSet(i).LinkTypeTarget)
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
	<span class="aspnetmaker">Link Type</span>
	<asp:PlaceHolder ID="pnlExport" runat="server">
		<asp:HyperLink CssClass="aspnetmaker" ID="lnkExportExcel" runat="server" NavigateUrl="linktype_list.aspx?Export=excel">Export to Excel</asp:HyperLink>&nbsp;&nbsp;
		<asp:HyperLink CssClass="aspnetmaker" ID="lnkExprotXML" runat="server" NavigateUrl="linktype_list.aspx?Export=xml">Export to XML</asp:HyperLink>&nbsp;&nbsp;
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
	<asp:ValidationSummary id="xevs_LinkType" CssClass="aspnetmaker" runat="server"
		HeaderText=""
		ShowSummary="False"
		ShowMessageBox="True"
		ForeColor="#FF0000" />
	<p><asp:HyperLink ID="lnkAdd" runat="server" CssClass="aspnetmaker" NavigateUrl="linktype_add.aspx">Add</asp:HyperLink></p>
	<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
		<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
	</asp:PlaceHolder>
	<p><asp:Label id="lblReturnUrl" Visible="False" Text="linktype_list.aspx" CssClass="aspnetmaker" runat="server" />
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
<asp:ObjectDataSource ID="LinkTypeDataSource"
	TypeName="PMGEN.LinkTypedal"
	DataObjectTypeName="PMGEN.LinkTyperow"
	SelectMethod="LoadList"
	UpdateMethod="Update"
	OnSelecting="LinkTypeDataSource_Selecting"
	OnSelected="LinkTypeDataSource_Selected"
	OnUpdating="LinkTypeDataSource_Updating"
	OnUpdated="LinkTypeDataSource_Updated"
	EnablePaging="True"
	SelectCountMethod="GetRowsCount"
	MaximumRowsParameterName="PageSize"
	StartRowIndexParameterName="StartRow"
	runat="server">
</asp:ObjectDataSource>
	<asp:GridView ID="LinkTypeGridView"
		PageSize="50"
		DataKeyNames="LinkTypeCD"
		DataSourceID="LinkTypeDataSource"
		GridLines="None"
		AutoGenerateColumns="False" CssClass="ewTable"
		AllowSorting="True" AllowPaging="True"
		OnInit="LinkTypeGridView_Init"
		OnDataBound="LinkTypeGridView_DataBound"
		OnRowCommand="LinkTypeGridView_RowCommand"
		OnRowUpdating="LinkTypeGridView_RowUpdating"
		OnRowUpdated="LinkTypeGridView_RowUpdated"
		OnRowDataBound="LinkTypeGridView_RowDataBound"
		OnSorting="Sorting"
		OnRowCreated="LinkTypeGridView_RowCreated"
		OnPageIndexChanged="LinkTypeGridView_PageIndexChanged"
		OnLoad="LinkTypeGridView_Load"
		OnUnload="LinkTypeGridView_Unload"
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
				<asp:HyperLink ID="DeleteLink" CssClass="aspnetmaker" runat="server" Text="Delete" />
			</ItemTemplate>
		</asp:TemplateField>
			<asp:TemplateField>
				<ItemStyle Wrap="True" />
				<HeaderTemplate>
					<asp:LinkButton runat='server' id="xs_LinkTypeCD"  CssClass="ewTableHeader" CommandName="Sort" CommandArgument="LinkTypeCD">Link Type CD </asp:LinkButton>
				</HeaderTemplate>
				<ItemTemplate>
<asp:Label id="x_LinkTypeCD" CssClass="aspnetmaker" runat="server" />
				</ItemTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_LinkTypeCD" Columns="30" MaxLength="50" CssClass="aspnetmaker" runat="server" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<ItemStyle Wrap="True" />
				<HeaderTemplate>
					<asp:LinkButton runat='server' id="xs_LinkTypeDesc"  CssClass="ewTableHeader" CommandName="Sort" CommandArgument="LinkTypeDesc">Link Type Desc </asp:LinkButton>
				</HeaderTemplate>
				<ItemTemplate>
<asp:Label id="x_LinkTypeDesc" CssClass="aspnetmaker" runat="server" />
				</ItemTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_LinkTypeDesc" Columns="30" MaxLength="255" CssClass="aspnetmaker" runat="server" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<ItemStyle Wrap="True" />
				<HeaderTemplate>
					<asp:LinkButton runat='server' id="xs_LinkTypeTarget"  CssClass="ewTableHeader" CommandName="Sort" CommandArgument="LinkTypeTarget">Link Type Target </asp:LinkButton>
				</HeaderTemplate>
				<ItemTemplate>
<asp:Label id="x_LinkTypeTarget" CssClass="aspnetmaker" runat="server" />
				</ItemTemplate>
				<EditItemTemplate>
<asp:TextBox id="x_LinkTypeTarget" Columns="30" MaxLength="50" CssClass="aspnetmaker" runat="server" />
				</EditItemTemplate>
			</asp:TemplateField>
		</Columns>
	</asp:GridView>
</asp:Content>
