<%@ Page EnableViewState="true" Language="VB" ValidateRequest="False" EnableEventValidation="False" Debug="True" MasterPageFile="masterpage.master" %>
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
	Dim key As SiteLinkkey = New SiteLinkkey()  ' record key
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
		objProfile = CustomProfile.GetTable(Share.ProjectName, SiteLinkinf.TableVar)
	End Sub

	' *************************
	' *  Handler for Page Load
	' *************************

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
                Session("ListPageURL") = HttpContext.Current.Request.FilePath()
		lblMessage.Text = String.Empty
		Dim isAuth As Boolean = False
		Page.MaintainScrollPositionOnPostBack = False
		tableId = SiteLinkGridView.ClientID
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
					Dim data As SiteLinkdal = New SiteLinkdal()
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
		If SiteLinkGridView.Rows.Count = 0 Then
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
				Dim data As SiteLinkdal = New SiteLinkdal()
				iRecordCount = data.GetRowsCount(SiteLinkinf.GetUserFilter()) 

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
		Dim data As SiteLinkdal = New SiteLinkdal()
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
		Dim data As SiteLinkdal = New SiteLinkdal()

		' Set up Advanced Search
		If (Request.QueryString.Count > 0) Then
			data.ClearSearch() ' Clear Previous Search

			' Set up search parameters
			Dim vx_SiteCategoryTypeID As String = Request.QueryString("x_SiteCategoryTypeID")
			Dim vz_SiteCategoryTypeID As String = Request.QueryString("z_SiteCategoryTypeID")
			If (Not String.IsNullOrEmpty(vx_SiteCategoryTypeID) AndAlso vz_SiteCategoryTypeID IsNot Nothing) Then
				data.AdvancedSearchParm1.SiteCategoryTypeID = Convert.ToInt32(vx_SiteCategoryTypeID)
				data.AdvancedSearchParm1.Attribute("SiteCategoryTypeID").SearchType = Core.GetAdvancedSearchType(vz_SiteCategoryTypeID)
			End If

			' Set up search parameters
			Dim vx_CategoryID As String = Request.QueryString("x_CategoryID")
			Dim vz_CategoryID As String = Request.QueryString("z_CategoryID")
			If (Not String.IsNullOrEmpty(vx_CategoryID) AndAlso vz_CategoryID IsNot Nothing) Then
				data.AdvancedSearchParm1.CategoryID = Convert.ToInt32(vx_CategoryID)
				data.AdvancedSearchParm1.Attribute("CategoryID").SearchType = Core.GetAdvancedSearchType(vz_CategoryID)
			End If

			' Set up search parameters
			Dim vx_LinkTypeCD As String = Request.QueryString("x_LinkTypeCD")
			Dim vz_LinkTypeCD As String = Request.QueryString("z_LinkTypeCD")
			If (Not String.IsNullOrEmpty(vx_LinkTypeCD) AndAlso vz_LinkTypeCD IsNot Nothing) Then
				data.AdvancedSearchParm1.LinkTypeCD = Convert.ToString(vx_LinkTypeCD)
				data.AdvancedSearchParm1.Attribute("LinkTypeCD").SearchType = Core.GetAdvancedSearchType(vz_LinkTypeCD)
			End If

			' Set up search parameters
			Dim vx_Title As String = Request.QueryString("x_Title")
			Dim vz_Title As String = Request.QueryString("z_Title")
			If (Not String.IsNullOrEmpty(vx_Title) AndAlso vz_Title IsNot Nothing) Then
				data.AdvancedSearchParm1.Title = Convert.ToString(vx_Title)
				data.AdvancedSearchParm1.Attribute("Title").SearchType = Core.GetAdvancedSearchType(vz_Title)
			End If

			' Set up search parameters
			Dim vx_SiteCategoryID As String = Request.QueryString("x_SiteCategoryID")
			Dim vz_SiteCategoryID As String = Request.QueryString("z_SiteCategoryID")
			If (Not String.IsNullOrEmpty(vx_SiteCategoryID) AndAlso vz_SiteCategoryID IsNot Nothing) Then
				data.AdvancedSearchParm1.SiteCategoryID = Convert.ToInt32(vx_SiteCategoryID)
				data.AdvancedSearchParm1.Attribute("SiteCategoryID").SearchType = Core.GetAdvancedSearchType(vz_SiteCategoryID)
			End If

			' Set up search parameters
			Dim vx_SiteCategoryGroupID As String = Request.QueryString("x_SiteCategoryGroupID")
			Dim vz_SiteCategoryGroupID As String = Request.QueryString("z_SiteCategoryGroupID")
			If (Not String.IsNullOrEmpty(vx_SiteCategoryGroupID) AndAlso vz_SiteCategoryGroupID IsNot Nothing) Then
				data.AdvancedSearchParm1.SiteCategoryGroupID = Convert.ToInt32(vx_SiteCategoryGroupID)
				data.AdvancedSearchParm1.Attribute("SiteCategoryGroupID").SearchType = Core.GetAdvancedSearchType(vz_SiteCategoryGroupID)
			End If

			' Set up search parameters
			Dim vx_CompanyID As String = Request.QueryString("x_CompanyID")
			Dim vz_CompanyID As String = Request.QueryString("z_CompanyID")
			If (Not String.IsNullOrEmpty(vx_CompanyID) AndAlso vz_CompanyID IsNot Nothing) Then
				data.AdvancedSearchParm1.CompanyID = Convert.ToInt32(vx_CompanyID)
				data.AdvancedSearchParm1.Attribute("CompanyID").SearchType = Core.GetAdvancedSearchType(vz_CompanyID)
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
		Dim oInfo As SiteLinkinf = New SiteLinkinf()
		Dim sThisSort As String = String.Empty
		Dim sLastSort As String = String.Empty
		Dim sSortField As String = String.Empty
		Dim i As Integer = 1
		Do While (i <= oInfo.TableInfo.Count)
			Dim sSortParm As String = oInfo.TableInfo.Fields(i).SortParm
			Dim fldProfile As FieldProfile = CustomProfile.GetField(Share.ProjectName, SiteLinkinf.TableVar, sSortParm)
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
		Dim oInfo As SiteLinkinf = New SiteLinkinf()
		Dim aryDefaultOrder As String() = oInfo.TableInfo.DefaultOrderBy.Split(","c)
		Dim i As Integer = 1
		Do While (i < oInfo.TableInfo.Count)
			Dim sSortParm As String = oInfo.TableInfo.Fields(i).SortParm
			Dim sFieldName As String = oInfo.TableInfo.Fields(i).FieldName
			Dim fldProfile As FieldProfile = CustomProfile.GetField(Share.ProjectName, SiteLinkinf.TableVar, sSortParm)
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

	Protected Sub SiteLinkGridView_PageIndexChanged(ByVal sender As Object,ByVal e As System.EventArgs)
		objProfile.PageIndex = SiteLinkGridView.PageIndex ' Save page index
	End Sub

	' ***********************************
	' *  Handler for GridView RowCreated
	' ***********************************

	Protected Sub SiteLinkGridView_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
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
						Dim sSort As String = CustomProfile.GetField(Share.ProjectName, SiteLinkinf.TableVar, button.CommandArgument).Sort
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

	' *************************
	' *  Reset Page Properties
	' *************************

	Private Sub ResetPageProperties()

		' Clear Page Index
		objProfile.PageIndex = 0
		SiteLinkGridView.PageIndex = 0
	End Sub

	' ************
	' *  Bind Data
	' ************

	Protected Sub BindData()
		Dim sWhere As String = GetUserFilter()
		Dim sOrderBy As String = String.Empty
		Dim data As SiteLinkdal = New SiteLinkdal()
		If (objProfile.PageIndex < 0) Then
			objProfile.PageIndex = 0
		End If
		SiteLinkGridView.AllowPaging = objProfile.AllowPaging
		If (objProfile.PageSize > 0) Then
			SiteLinkGridView.PageSize = objProfile.PageSize 
		End If
		If (Not objProfile.IsCollapsed) Then

			' Try to restore page index
			Try
				Dim intPageCount As Integer = data.GetPageCount(sWhere, SiteLinkGridView.PageSize)
				If (SiteLinkGridView.PageSize > 0 AndAlso intPageCount < objProfile.PageIndex + 1) Then objProfile.PageIndex = IIf(intPageCount > 0, intPageCount - 1, 0)
				SiteLinkGridView.PageIndex = objProfile.PageIndex
			Catch e As Exception
				ResetPageProperties()
			End Try
		Else
			SiteLinkGridView.PageIndex = 0
		End If
		Try
			SiteLinkGridView.DataBind()
			If (SiteLinkGridView.Rows.Count = 0) Then
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

	Protected Sub SiteLinkGridView_Init(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			SiteLinkdal.OpenConnection()
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

	Protected Sub SiteLinkGridView_Load(ByVal sender As Object, ByVal e As System.EventArgs)
		If (isExport) Then
			SetupControlsVisible(False)
			SiteLinkGridView.PagerSettings.Visible = False
			Return
		End If
	End Sub

	' *************************************
	' *  Handler for GridView Unload
	' *************************************

	Protected Sub SiteLinkGridView_Unload(ByVal sender As Object, ByVal e As System.EventArgs)
		Try
			SiteLinkdal.CloseAndDisposeConnection()
		Catch
		End Try
	End Sub

	' *************************************
	' *  Handler for GridView RowDataBound
	' *************************************

	Protected Sub SiteLinkGridView_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
		tableId = TryCast(sender, GridView).ClientID
		If (SiteLinkGridView.PageCount > 1 AndAlso SiteLinkGridView.PagerSettings.Visible) Then
			If (SiteLinkGridView.PagerSettings.Position = PagerPosition.Top OrElse SiteLinkGridView.PagerSettings.Position = PagerPosition.TopAndBottom) Then
				firstRowOffset = initialFirstRowOffset + 1
			End If
			If (SiteLinkGridView.PagerSettings.Position = PagerPosition.Bottom OrElse SiteLinkGridView.PagerSettings.Position = PagerPosition.TopAndBottom) Then
				lastRowOffset = initialLastRowOffset + 1
			End If
		End If
		If (e.Row.RowType = DataControlRowType.Header) Then
		ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
			Dim row As SiteLinkrow = TryCast(e.Row.DataItem, SiteLinkrow) ' get row object
			Dim control As GridViewRow = e.Row ' get control object
			Dim bNormal As Boolean = ((e.Row.RowState And DataControlRowState.Normal) = DataControlRowState.Normal)
			Dim bAlternate As Boolean = ((e.Row.RowState And DataControlRowState.Alternate) = DataControlRowState.Alternate)
			Dim bEdit As Boolean = ((e.Row.RowState And DataControlRowState.Edit) = DataControlRowState.Edit)
			If ((bNormal OrElse bAlternate) AndAlso (Not bEdit)) Then
				RowToControl(row, TryCast(control, WebControl) , Core.CtrlType.View)
			End If
			Dim ViewLink As HyperLink = TryCast(control.FindControl("ViewLink"), HyperLink)
			If (ViewLink IsNot Nothing) Then _
				ViewLink.NavigateUrl = String.Format("sitelink_view.aspx?ID={0}", Server.UrlEncode(Convert.ToString(row.ID)))
			Dim EditLink As HyperLink = TryCast(control.FindControl("EditLink"), HyperLink)
			If (EditLink IsNot Nothing) Then _
				EditLink.NavigateUrl = String.Format("sitelink_edit.aspx?ID={0}", Server.UrlEncode(Convert.ToString(row.ID)))
			Dim CopyLink As HyperLink = TryCast(control.FindControl("CopyLink"), HyperLink)
			If (CopyLink IsNot Nothing) Then _
				CopyLink.NavigateUrl = String.Format("sitelink_add.aspx?ID={0}", Server.UrlEncode(Convert.ToString(row.ID)))
			Dim DeleteLink As HyperLink = TryCast(control.FindControl("DeleteLink"), HyperLink)
			If (DeleteLink IsNot Nothing) Then _
				DeleteLink.NavigateUrl = String.Format("sitelink_delete.aspx?ID={0}", Server.UrlEncode(Convert.ToString(row.ID)))
		ElseIf (e.Row.RowType = DataControlRowType.Footer) Then
			Dim control As GridViewRow = e.Row ' get control object
		End If
	End Sub

	' **********************************
	' *  Handler for GridView DataBound
	' **********************************

	Protected Sub SiteLinkGridView_DataBound(ByVal sender As Object, ByVal e As System.EventArgs)
	End Sub

	' ***********************************
	' *  Handler for GridView RowCommand
	' ***********************************

	Protected Sub SiteLinkGridView_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)
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
		SiteLinkGridView.Columns(0).Visible = visible
		SiteLinkGridView.Columns(1).Visible = visible
		SiteLinkGridView.Columns(2).Visible = visible
		lnkAdd.Visible = visible
		SiteLinkGridView.Columns(3).Visible = visible
		SiteLinkGridView.AllowSorting = visible
	End Sub

	' ********************************************
	' *  Convert Row Object Value To Input Control
	' ********************************************

	Private Sub RowToControl(ByVal row As SiteLinkrow, ByVal control As WebControl, ByVal ct As Core.CtrlType)
		If (row Is Nothing) Then Return' empty row object
		If (control Is Nothing) Then Return' empty control object
		If (ct = Core.CtrlType.View) Then ' set up view controls

			' Field SiteCategoryTypeID
			Dim x_SiteCategoryTypeID As Label = TryCast(control.FindControl("x_SiteCategoryTypeID"), Label)
			Dim v_SiteCategoryTypeID As String
			If (row.SiteCategoryTypeID.HasValue) Then v_SiteCategoryTypeID = row.SiteCategoryTypeID.ToString() Else v_SiteCategoryTypeID = String.Empty
			x_SiteCategoryTypeID.Text = SiteLinkdal.LookUpTable("SiteCategoryTypeID", v_SiteCategoryTypeID)

			' Field CategoryID
			Dim x_CategoryID As Label = TryCast(control.FindControl("x_CategoryID"), Label)
			Dim v_CategoryID As String
			If (row.CategoryID.HasValue) Then v_CategoryID = row.CategoryID.ToString() Else v_CategoryID = String.Empty
			x_CategoryID.Text = SiteLinkdal.LookUpTable("CategoryID", v_CategoryID)

			' Field LinkTypeCD
			Dim x_LinkTypeCD As Label = TryCast(control.FindControl("x_LinkTypeCD"), Label)
			Dim v_LinkTypeCD As String
			If (row.LinkTypeCD IsNot Nothing) Then v_LinkTypeCD = row.LinkTypeCD.ToString() Else v_LinkTypeCD = String.Empty
			x_LinkTypeCD.Text = SiteLinkdal.LookUpTable("LinkTypeCD", v_LinkTypeCD)

			' Field Title
			Dim x_Title As Label = TryCast(control.FindControl("x_Title"), Label)
			If (row.Title IsNot Nothing) Then x_Title.Text = row.Title.ToString() Else x_Title.Text = String.Empty

			' Field SiteCategoryID
			Dim x_SiteCategoryID As Label = TryCast(control.FindControl("x_SiteCategoryID"), Label)
			Dim v_SiteCategoryID As String
			If (row.SiteCategoryID.HasValue) Then v_SiteCategoryID = row.SiteCategoryID.ToString() Else v_SiteCategoryID = String.Empty
			x_SiteCategoryID.Text = SiteLinkdal.LookUpTable("SiteCategoryID", v_SiteCategoryID)

			' Field SiteCategoryGroupID
			Dim x_SiteCategoryGroupID As Label = TryCast(control.FindControl("x_SiteCategoryGroupID"), Label)
			Dim v_SiteCategoryGroupID As String
			If (row.SiteCategoryGroupID.HasValue) Then v_SiteCategoryGroupID = row.SiteCategoryGroupID.ToString() Else v_SiteCategoryGroupID = String.Empty
			x_SiteCategoryGroupID.Text = SiteLinkdal.LookUpTable("SiteCategoryGroupID", v_SiteCategoryGroupID)

			' Field CompanyID
			Dim x_CompanyID As Label = TryCast(control.FindControl("x_CompanyID"), Label)
			Dim v_CompanyID As String
			If (row.CompanyID.HasValue) Then v_CompanyID = row.CompanyID.ToString() Else v_CompanyID = String.Empty
			x_CompanyID.Text = SiteLinkdal.LookUpTable("CompanyID", v_CompanyID)
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

	Protected Sub SiteLinkDataSource_Selecting(Byval sender As Object, ByVal e As ObjectDataSourceSelectingEventArgs)
		e.InputParameters.Clear()
		e.InputParameters.Add("filter", SiteLinkinf.GetUserFilter())
	End Sub

	' *************************************
	' *  DataSource Selected Event Handler
	' *************************************

	Protected Sub SiteLinkDataSource_Selected(Byval sender As Object, Byval e As ObjectDataSourceStatusEventArgs)
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

	' *****************
	' *  Load Controls 
	' *****************

	Private Sub LoadSearchControls(ByVal control As Control) 
		Dim sSearchType As String = String.Empty
		Dim data As SiteLinkdal = New SiteLinkdal()
	End Sub

	' *******************
	' *  Get User Filter
	' *******************

	Private Function GetUserFilter() As String 
		Return SiteLinkinf.GetUserFilter()
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
				Response.AddHeader("Content-Disposition", "attachment; filename=""SiteLink.xls""")
				Export("excel")
			Case "XML"
				Response.Clear()
				Response.ContentType = "text/xml"
				Response.AddHeader("Content-Disposition", "attachment; filename=""SiteLink.xml""")
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
					SiteLinkGridView.Columns(0).Visible = False
					SiteLinkGridView.Columns(1).Visible = False
					SiteLinkGridView.Columns(2).Visible = False
					lnkAdd.Visible = False
					SiteLinkGridView.Columns(3).Visible = False
					SiteLinkGridView.PagerSettings.Visible = False
					SiteLinkGridView.AllowSorting = False
					BindData()
					PrepareGridViewForExport(SiteLinkGridView)

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
					htw.WriteEncodedText("TABLE: Site Link")
					htw.RenderEndTag()

					' Render Table
					SiteLinkGridView.RenderControl(htw)

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
			Dim dataSet As SiteLinkrows 
			Dim sFldParm As String
			Dim oInfo As SiteLinkinf = New SiteLinkinf()
			Dim nPageSize As Integer = objProfile.PageSize
			Dim nPageIndex As Integer = objProfile.PageIndex
			Dim nStartRec As Integer = nPageSize * nPageIndex
			Dim nStopRec As Integer
			If (nPageSize > 0) Then
				nStopRec = nStartRec + nPageSize
			Else
				nStopRec = Int32.MaxValue ' Show all values
			End If
			Dim data As SiteLinkdal = New SiteLinkdal()
			dataSet = TryCast(data.LoadList(SiteLinkinf.GetUserFilter()), SiteLinkrows)
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
					sFldParm = "SiteCategoryTypeID"
					oXmlField = oXmlDoc.CreateElement(sFldParm)
					If (dataSet(i).SiteCategoryTypeID.HasValue) Then
						oXmlField.InnerText = Convert.ToString(dataSet(i).SiteCategoryTypeID)
					Else
						oXmlField.InnerText = "Null"
					End If
					oXmlRec.AppendChild(oXmlField)
					sFldParm = "CategoryID"
					oXmlField = oXmlDoc.CreateElement(sFldParm)
					If (dataSet(i).CategoryID.HasValue) Then
						oXmlField.InnerText = Convert.ToString(dataSet(i).CategoryID)
					Else
						oXmlField.InnerText = "Null"
					End If
					oXmlRec.AppendChild(oXmlField)
					sFldParm = "LinkTypeCD"
					oXmlField = oXmlDoc.CreateElement(sFldParm)
					If (dataSet(i).LinkTypeCD IsNot Nothing) Then
						oXmlField.InnerText = Convert.ToString(dataSet(i).LinkTypeCD)
					Else
						oXmlField.InnerText = "Null"
					End If
					oXmlRec.AppendChild(oXmlField)
					sFldParm = "Title"
					oXmlField = oXmlDoc.CreateElement(sFldParm)
					If (dataSet(i).Title IsNot Nothing) Then
						oXmlField.InnerText = Convert.ToString(dataSet(i).Title)
					Else
						oXmlField.InnerText = "Null"
					End If
					oXmlRec.AppendChild(oXmlField)
					sFldParm = "SiteCategoryID"
					oXmlField = oXmlDoc.CreateElement(sFldParm)
					If (dataSet(i).SiteCategoryID.HasValue) Then
						oXmlField.InnerText = Convert.ToString(dataSet(i).SiteCategoryID)
					Else
						oXmlField.InnerText = "Null"
					End If
					oXmlRec.AppendChild(oXmlField)
					sFldParm = "SiteCategoryGroupID"
					oXmlField = oXmlDoc.CreateElement(sFldParm)
					If (dataSet(i).SiteCategoryGroupID.HasValue) Then
						oXmlField.InnerText = Convert.ToString(dataSet(i).SiteCategoryGroupID)
					Else
						oXmlField.InnerText = "Null"
					End If
					oXmlRec.AppendChild(oXmlField)
					sFldParm = "CompanyID"
					oXmlField = oXmlDoc.CreateElement(sFldParm)
					If (dataSet(i).CompanyID.HasValue) Then
						oXmlField.InnerText = Convert.ToString(dataSet(i).CompanyID)
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
	<span class="aspnetmaker">Site Link</span>
	<asp:PlaceHolder ID="pnlExport" runat="server">
		<asp:HyperLink CssClass="aspnetmaker" ID="lnkExportExcel" runat="server" NavigateUrl="sitelink_list.aspx?Export=excel">Export to Excel</asp:HyperLink>&nbsp;&nbsp;
		<asp:HyperLink CssClass="aspnetmaker" ID="lnkExprotXML" runat="server" NavigateUrl="sitelink_list.aspx?Export=xml">Export to XML</asp:HyperLink>&nbsp;&nbsp;
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
	<asp:ValidationSummary id="xevs_SiteLink" CssClass="aspnetmaker" runat="server"
		HeaderText=""
		ShowSummary="False"
		ShowMessageBox="True"
		ForeColor="#FF0000" />
	<asp:PlaceHolder ID="pnlSearch" runat="server">
		<asp:Panel runat="server">
			<p />
			<asp:Table runat="server" BorderWidth="0" CellSpacing="1" CellPadding="4" ID="mh_SiteLink">
				<asp:TableRow>
					<asp:TableCell>
						<asp:Table runat="server">
						</asp:Table>
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow>
					<asp:TableCell>
	<asp:LinkButton ID="btnClearSearch" OnClick="btnClearSearch_Click" runat="server" 			CssClass="aspnetmaker" Text="Show all"></asp:LinkButton>&nbsp;<asp:HyperLink ID="lnkSearch" runat="server" CssClass="aspnetmaker" Text="Advanced Search" NavigateUrl="sitelink_search.aspx"></asp:HyperLink>
					</asp:TableCell>
				</asp:TableRow>
			</asp:Table>
		</asp:Panel>
	</asp:PlaceHolder>
	<p><asp:HyperLink ID="lnkAdd" runat="server" CssClass="aspnetmaker" NavigateUrl="sitelink_add.aspx">Add</asp:HyperLink></p>
	<asp:PlaceHolder ID="pnlMessage" runat="server" Visible="false">
		<p><asp:Label id="lblMessage" forecolor="#FF0000" CssClass="aspnetmaker" runat="server" /></p>
	</asp:PlaceHolder>
	<p><asp:Label id="lblReturnUrl" Visible="False" Text="sitelink_list.aspx" CssClass="aspnetmaker" runat="server" />
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
<asp:ObjectDataSource ID="SiteLinkDataSource"
	TypeName="PMGEN.SiteLinkdal"
	SelectMethod="LoadList"
	OnSelecting="SiteLinkDataSource_Selecting"
	OnSelected="SiteLinkDataSource_Selected"
	EnablePaging="True"
	SelectCountMethod="GetRowsCount"
	MaximumRowsParameterName="PageSize"
	StartRowIndexParameterName="StartRow"
	runat="server">
</asp:ObjectDataSource>
	<asp:GridView ID="SiteLinkGridView"
		PageSize="50"
		DataKeyNames="ID"
		DataSourceID="SiteLinkDataSource"
		GridLines="None"
		AutoGenerateColumns="False" CssClass="ewTable"
		AllowSorting="True" AllowPaging="True"
		OnInit="SiteLinkGridView_Init"
		OnDataBound="SiteLinkGridView_DataBound"
		OnRowCommand="SiteLinkGridView_RowCommand"
		OnRowDataBound="SiteLinkGridView_RowDataBound"
		OnSorting="Sorting"
		OnRowCreated="SiteLinkGridView_RowCreated"
		OnPageIndexChanged="SiteLinkGridView_PageIndexChanged"
		OnLoad="SiteLinkGridView_Load"
		OnUnload="SiteLinkGridView_Unload"
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
				<asp:HyperLink ID="ViewLink" CssClass="aspnetmaker" runat="server"  Text="View" />
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
				<asp:HyperLink ID="EditLink" CssClass="aspnetmaker" runat="server"  Text="Edit" />
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
				<asp:HyperLink ID="CopyLink" CssClass="aspnetmaker" runat="server"  Text="Copy" />
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
				<asp:HyperLink ID="DeleteLink" CssClass="aspnetmaker" runat="server" Text="Delete" />
			</ItemTemplate>
		</asp:TemplateField>
			<asp:TemplateField>
				<ItemStyle Wrap="True" />
				<HeaderTemplate>
					<asp:LinkButton runat='server' id="xs_SiteCategoryTypeID"  CssClass="ewTableHeader" CommandName="Sort" CommandArgument="SiteCategoryTypeID">Site Type </asp:LinkButton>
				</HeaderTemplate>
				<ItemTemplate>
<asp:Label id="x_SiteCategoryTypeID" CssClass="aspnetmaker" runat="server" />
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<ItemStyle Wrap="True" />
				<HeaderTemplate>
					<asp:LinkButton runat='server' id="xs_CategoryID"  CssClass="ewTableHeader" CommandName="Sort" CommandArgument="CategoryID">Link Category </asp:LinkButton>
				</HeaderTemplate>
				<ItemTemplate>
<asp:Label id="x_CategoryID" CssClass="aspnetmaker" runat="server" />
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<ItemStyle Wrap="True" />
				<HeaderTemplate>
					<asp:LinkButton runat='server' id="xs_LinkTypeCD"  CssClass="ewTableHeader" CommandName="Sort" CommandArgument="LinkTypeCD">Link Type </asp:LinkButton>
				</HeaderTemplate>
				<ItemTemplate>
<asp:Label id="x_LinkTypeCD" CssClass="aspnetmaker" runat="server" />
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<ItemStyle Wrap="True" />
				<HeaderTemplate>
					<asp:LinkButton runat='server' id="xs_Title"  CssClass="ewTableHeader" CommandName="Sort" CommandArgument="Title">Title </asp:LinkButton>
				</HeaderTemplate>
				<ItemTemplate>
<asp:Label id="x_Title" CssClass="aspnetmaker" runat="server" />
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<ItemStyle Wrap="True" />
				<HeaderTemplate>
					<asp:LinkButton runat='server' id="xs_SiteCategoryID"  CssClass="ewTableHeader" CommandName="Sort" CommandArgument="SiteCategoryID">Site Category </asp:LinkButton>
				</HeaderTemplate>
				<ItemTemplate>
<asp:Label id="x_SiteCategoryID" CssClass="aspnetmaker" runat="server" />
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<ItemStyle Wrap="True" />
				<HeaderTemplate>
					<asp:LinkButton runat='server' id="xs_SiteCategoryGroupID"  CssClass="ewTableHeader" CommandName="Sort" CommandArgument="SiteCategoryGroupID">Site Group </asp:LinkButton>
				</HeaderTemplate>
				<ItemTemplate>
<asp:Label id="x_SiteCategoryGroupID" CssClass="aspnetmaker" runat="server" />
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<ItemStyle Wrap="True" />
				<HeaderTemplate>
					<asp:LinkButton runat='server' id="xs_CompanyID"  CssClass="ewTableHeader" CommandName="Sort" CommandArgument="CompanyID">Company </asp:LinkButton>
				</HeaderTemplate>
				<ItemTemplate>
<asp:Label id="x_CompanyID" CssClass="aspnetmaker" runat="server" />
				</ItemTemplate>
			</asp:TemplateField>
		</Columns>
	</asp:GridView>
</asp:Content>
