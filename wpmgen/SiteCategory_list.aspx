<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="SiteCategory_list.aspx.vb" Inherits="SiteCategory_list" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If SiteCategory.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var SiteCategory_list = new ew_Page("SiteCategory_list");
// page properties
SiteCategory_list.PageID = "list"; // page ID
var EW_PAGE_ID = SiteCategory_list.PageID; // for backward compatibility
// extend page with ValidateForm function
SiteCategory_list.ValidateForm = function(fobj) {
	if (!this.ValidateRequired)
		return true; // ignore validation
	if (fobj.a_confirm && fobj.a_confirm.value == "F")
		return true;
	var i, elm, aelm, infix;
	var rowcnt = (fobj.key_count) ? Number(fobj.key_count.value) : 1;
	var addcnt = 0;
	for (i=0; i<rowcnt; i++) {
		infix = (fobj.key_count) ? String(i+1) : "";
		var chkthisrow = true;
		if (fobj.a_list && fobj.a_list.value == "gridinsert")
			chkthisrow = !(this.EmptyRow(fobj, infix));
		else
			chkthisrow = true;
		if (chkthisrow) {
			addcnt += 1;
		elm = fobj.elements["x" + infix + "_SiteCategoryTypeID"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field - Site Type");
		elm = fobj.elements["x" + infix + "_GroupOrder"];
		if (elm && !ew_CheckNumber(elm.value))
			return ew_OnError(this, elm, "Incorrect floating point number - Order");
		} // End Grid Add checking
	}
	if (fobj.a_list && fobj.a_list.value == "gridinsert" && addcnt == 0) { // No row added
		alert("No records to be added");
		return false;
	}
	return true;
}
// Extend page with empty row check
SiteCategory_list.EmptyRow = function(fobj, infix) {
	if (ew_ValueChanged(fobj, infix, "SiteCategoryTypeID")) return false;
	if (ew_ValueChanged(fobj, infix, "GroupOrder")) return false;
	if (ew_ValueChanged(fobj, infix, "CategoryName")) return false;
	if (ew_ValueChanged(fobj, infix, "ParentCategoryID")) return false;
	if (ew_ValueChanged(fobj, infix, "CategoryFileName")) return false;
	if (ew_ValueChanged(fobj, infix, "SiteCategoryGroupID")) return false;
	return true;
}
<% If EW_CLIENT_VALIDATE Then %>
SiteCategory_list.ValidateRequired = true; // uses JavaScript validation
<% Else %>
SiteCategory_list.ValidateRequired = false; // no JavaScript validation
<% End If %>
//-->
</script>
<div id="ewDetailsDiv" name="ewDetailsDivDiv" style="visibility:hidden"></div>
<script language="JavaScript" type="text/javascript">
<!--
// YUI container
var ewDetailsDiv;
var ew_AjaxDetailsTimer = null;
//-->
</script>
<script type="text/javascript">
<!--
var ew_DHTMLEditors = [];
//-->
</script>
<script language="JavaScript" type="text/javascript">
<!--
// Write your client script here, no need to add script tags.
// To include another .js script, use:
// ew_ClientScriptInclude("my_javascript.js"); 
//-->
</script>
<% End If %>
<% If SiteCategory.Export = "" Then %>
<% End If %>
<%
If SiteCategory.CurrentAction = "gridadd" Then SiteCategory.CurrentFilter = "0=1"

' Load recordset
Rs = SiteCategory_list.LoadRecordset()
If SiteCategory.CurrentAction = "gridadd" Then
	SiteCategory_list.lStartRec = 1
	If SiteCategory_list.lDisplayRecs <= 0 Then SiteCategory_list.lDisplayRecs = 25
	SiteCategory_list.lTotalRecs = SiteCategory_list.lDisplayRecs
	SiteCategory_list.lStopRec = SiteCategory_list.lDisplayRecs
Else
	SiteCategory_list.lStartRec = 1
	If SiteCategory_list.lDisplayRecs <= 0 Then ' Display all records
		SiteCategory_list.lDisplayRecs = SiteCategory_list.lTotalRecs
	End If
	If Not (SiteCategory.ExportAll AndAlso SiteCategory.Export <> "") Then
		SiteCategory_list.SetUpStartRec() ' Set up start record position
	End If
End If
%>
<p><span class="aspnetmaker" style="white-space: nowrap;">TABLE: Site Type Location
<% If SiteCategory.Export = "" AndAlso SiteCategory.CurrentAction = "" Then %>
&nbsp;&nbsp;<a href="<%= SiteCategory_list.PageUrl %>export=excel"><img src='images/exportxls.gif' alt='Export to Excel' title='Export to Excel' width='16' height='16' border='0'></a>
<% End If %>
</span></p>
<% If SiteCategory.Export = "" AndAlso SiteCategory.CurrentAction = "" Then %>
<a href="javascript:ew_ToggleSearchPanel(SiteCategory_list);" style="text-decoration: none;"><img id="SiteCategory_list_SearchImage" src="images/collapse.gif" alt="" width="9" height="9" border="0"></a><span class="aspnetmaker">&nbsp;Search</span><br>
<div id="SiteCategory_list_SearchPanel">
<form name="fSiteCategorylistsrch" id="fSiteCategorylistsrch" class="ewForm">
<input type="hidden" id="t" name="t" value="SiteCategory" />
<table class="ewBasicSearch">
	<tr>
		<td><span class="aspnetmaker">
			<input type="text" name="<%= EW_TABLE_BASIC_SEARCH %>" id="<%= EW_TABLE_BASIC_SEARCH %>" size="20" value="<%= ew_HtmlEncode(SiteCategory.BasicSearchKeyword) %>" />
			<input type="Submit" name="Submit" id="Submit" value="Search (*)" />&nbsp;
			<a href="<%= SiteCategory_list.PageUrl %>cmd=reset">Show all</a>&nbsp;
			<a href="SiteCategory_srch.aspx">Advanced Search</a>&nbsp;
		</span></td>
	</tr>
	<tr>
	<td><span class="aspnetmaker"><label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value=""<% If SiteCategory.BasicSearchType = "" Then %> checked="checked"<% End If %> />Exact phrase</label>&nbsp;&nbsp;<label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value="AND"<% If SiteCategory.BasicSearchType = "AND" Then %> checked="checked"<% End If %> />All words</label>&nbsp;&nbsp;<label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value="OR"<% If SiteCategory.BasicSearchType = "OR" Then %> checked="checked"<% End If %> />Any word</label></span></td>
	</tr>
</table>
</form>
</div>
<% End If %>
<% SiteCategory_list.ShowMessage() %>
<br />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<% If SiteCategory.Export = "" Then %>
<div class="ewGridUpperPanel">
<% If SiteCategory.CurrentAction <> "gridadd" AndAlso SiteCategory.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If SiteCategory_list.Pager Is Nothing Then SiteCategory_list.Pager = New cPrevNextPager(SiteCategory_list.lStartRec, SiteCategory_list.lDisplayRecs, SiteCategory_list.lTotalRecs) %>
<% If SiteCategory_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If SiteCategory_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= SiteCategory_list.PageUrl %>start=<%= SiteCategory_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If SiteCategory_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= SiteCategory_list.PageUrl %>start=<%= SiteCategory_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= SiteCategory_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If SiteCategory_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= SiteCategory_list.PageUrl %>start=<%= SiteCategory_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If SiteCategory_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= SiteCategory_list.PageUrl %>start=<%= SiteCategory_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= SiteCategory_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= SiteCategory_list.Pager.FromIndex %> to <%= SiteCategory_list.Pager.ToIndex %> of <%= SiteCategory_list.Pager.RecordCount %></span>
<% Else %>
	<% If SiteCategory_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If SiteCategory_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="SiteCategory" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If SiteCategory_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If SiteCategory_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If SiteCategory_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If SiteCategory.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<div class="aspnetmaker">
<% If SiteCategory.CurrentAction <> "gridadd" AndAlso SiteCategory.CurrentAction <> "gridedit" Then ' Not grid add/edit mode %>
<a href="<%= SiteCategory.AddUrl %>">Add</a>&nbsp;&nbsp;
<a href="<%= SiteCategory_list.PageUrl %>a=gridadd">Grid Add</a>&nbsp;&nbsp;
<% If SiteCategory_list.lTotalRecs > 0 Then %>
<a href="<%= SiteCategory_list.PageUrl %>a=gridedit">Grid Edit</a>&nbsp;&nbsp;
<% End If %>
<% Else ' Grid add/edit mode %>
<% If SiteCategory.CurrentAction = "gridadd" Then %>
<a href="" onclick="f=ew_GetForm('fSiteCategorylist');if (SiteCategory_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/insert.gif' alt='Insert' title='Insert' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<% If SiteCategory.CurrentAction = "gridedit" Then %>
<a href="" onclick="f=ew_GetForm('fSiteCategorylist');if (SiteCategory_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/update.gif' alt='Save' title='Save' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<a href="<%= SiteCategory_list.PageUrl %>a=cancel"><img src='images/cancel.gif' alt='Cancel' title='Cancel' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
</div>
</div>
<% End If %>
<div class="ewGridMiddlePanel">
<form name="fSiteCategorylist" id="fSiteCategorylist" class="ewForm" method="post">
<input type="hidden" name="t" id="t" value="SiteCategory" />
<% If SiteCategory_list.lTotalRecs > 0 Then %>
<table cellspacing="0" rowhighlightclass="ewTableHighlightRow" rowselectclass="ewTableSelectRow" roweditclass="ewTableEditRow" class="ewTable ewTableSeparate">
<%
	SiteCategory_list.lOptionCnt = 0
	SiteCategory_list.lOptionCnt = SiteCategory_list.lOptionCnt + 1 ' View
	SiteCategory_list.lOptionCnt = SiteCategory_list.lOptionCnt + 1 ' Edit
	SiteCategory_list.lOptionCnt = SiteCategory_list.lOptionCnt + 1 ' Copy
	SiteCategory_list.lOptionCnt = SiteCategory_list.lOptionCnt + 1 ' Delete
	SiteCategory_list.lOptionCnt = SiteCategory_list.lOptionCnt + SiteCategory_list.ListOptions.Items.Count ' Custom list options
%>
<%= SiteCategory.TableCustomInnerHTML %>
<thead><!-- Table header -->
	<tr class="ewTableHeader">
<% If SiteCategory.Export = "" Then %>
<% If SiteCategory.CurrentAction <> "gridadd" AndAlso SiteCategory.CurrentAction <> "gridedit" Then %>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<%

' Custom list options
For i As Integer = 0 to SiteCategory_list.ListOptions.Items.Count -1
	If SiteCategory_list.ListOptions.Items(i).Visible Then Response.Write(SiteCategory_list.ListOptions.Items(i).HeaderCellHtml)
Next
%>
<% End If %>
<% End If %>
<% If SiteCategory.SiteCategoryTypeID.Visible Then ' SiteCategoryTypeID %>
	<% If SiteCategory.SortUrl(SiteCategory.SiteCategoryTypeID) = "" Then %>
		<td style="white-space: nowrap;">Site Type</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteCategory.SortUrl(SiteCategory.SiteCategoryTypeID) %>',1);" style="white-space: nowrap;">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Site Type</td><td style="width: 10px;"><% If SiteCategory.SiteCategoryTypeID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteCategory.SiteCategoryTypeID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If SiteCategory.GroupOrder.Visible Then ' GroupOrder %>
	<% If SiteCategory.SortUrl(SiteCategory.GroupOrder) = "" Then %>
		<td style="white-space: nowrap;">Order</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteCategory.SortUrl(SiteCategory.GroupOrder) %>',1);" style="white-space: nowrap;">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Order</td><td style="width: 10px;"><% If SiteCategory.GroupOrder.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteCategory.GroupOrder.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If SiteCategory.CategoryName.Visible Then ' CategoryName %>
	<% If SiteCategory.SortUrl(SiteCategory.CategoryName) = "" Then %>
		<td style="white-space: nowrap;">Name</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteCategory.SortUrl(SiteCategory.CategoryName) %>',1);" style="white-space: nowrap;">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Name&nbsp;(*)</td><td style="width: 10px;"><% If SiteCategory.CategoryName.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteCategory.CategoryName.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If SiteCategory.ParentCategoryID.Visible Then ' ParentCategoryID %>
	<% If SiteCategory.SortUrl(SiteCategory.ParentCategoryID) = "" Then %>
		<td style="white-space: nowrap;">Parent Category</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteCategory.SortUrl(SiteCategory.ParentCategoryID) %>',1);" style="white-space: nowrap;">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Parent Category</td><td style="width: 10px;"><% If SiteCategory.ParentCategoryID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteCategory.ParentCategoryID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If SiteCategory.CategoryFileName.Visible Then ' CategoryFileName %>
	<% If SiteCategory.SortUrl(SiteCategory.CategoryFileName) = "" Then %>
		<td style="white-space: nowrap;">Category File Name</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteCategory.SortUrl(SiteCategory.CategoryFileName) %>',1);" style="white-space: nowrap;">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Category File Name&nbsp;(*)</td><td style="width: 10px;"><% If SiteCategory.CategoryFileName.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteCategory.CategoryFileName.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If SiteCategory.SiteCategoryGroupID.Visible Then ' SiteCategoryGroupID %>
	<% If SiteCategory.SortUrl(SiteCategory.SiteCategoryGroupID) = "" Then %>
		<td style="white-space: nowrap;">Site Group</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteCategory.SortUrl(SiteCategory.SiteCategoryGroupID) %>',1);" style="white-space: nowrap;">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Site Group</td><td style="width: 10px;"><% If SiteCategory.SiteCategoryGroupID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteCategory.SiteCategoryGroupID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
	</tr>
</thead>
<tbody><!-- Table body -->
<%
If (SiteCategory.ExportAll AndAlso SiteCategory.Export <> "") Then
	SiteCategory_list.lStopRec = SiteCategory_list.lTotalRecs
Else
	SiteCategory_list.lStopRec = SiteCategory_list.lStartRec + SiteCategory_list.lDisplayRecs - 1 ' Set the last record to display
End If
If SiteCategory.CurrentAction = "gridadd" AndAlso SiteCategory_list.lStopRec = -1 Then
	SiteCategory_list.lStopRec = EW_GRIDADD_ROWS
End If 

' Move to first record
For i As Integer = 1 to SiteCategory_list.lStartRec - 1
	If Rs.Read() Then	SiteCategory_list.lRecCnt = SiteCategory_list.lRecCnt + 1
Next		
SiteCategory_list.lRowCnt = 0
If SiteCategory.CurrentAction = "gridadd" Then SiteCategory_list.lRowIndex = 0
If SiteCategory.CurrentAction = "gridedit" Then SiteCategory_list.lRowIndex = 0

' Output data rows
Do While (SiteCategory.CurrentAction = "gridadd" OrElse Rs.Read()) AndAlso (SiteCategory_list.lRecCnt < SiteCategory_list.lStopRec)
	SiteCategory_list.lRecCnt = SiteCategory_list.lRecCnt + 1
	If SiteCategory_list.lRecCnt >= SiteCategory_list.lStartRec Then
		SiteCategory_list.lRowCnt = SiteCategory_list.lRowCnt + 1
		If SiteCategory.CurrentAction = "gridadd" OrElse SiteCategory.CurrentAction = "gridedit" Then SiteCategory_list.lRowIndex = SiteCategory_list.lRowIndex + 1
	SiteCategory.CssClass = ""
	SiteCategory.CssStyle = ""
	SiteCategory.RowClientEvents = "onmouseover='ew_MouseOver(event, this);' onmouseout='ew_MouseOut(event, this);' onclick='ew_Click(event, this);'"
	If SiteCategory.CurrentAction = "gridadd" Then
		SiteCategory_list.LoadDefaultValues() ' Load default values
	Else
		SiteCategory_list.LoadRowValues(Rs) ' Load row values
	End If
	SiteCategory.RowType = EW_ROWTYPE_VIEW ' Render view
	If SiteCategory.CurrentAction = "gridadd" Then ' Grid add
		SiteCategory.RowType = EW_ROWTYPE_ADD ' Render add
	End If
	If SiteCategory.CurrentAction = "gridadd" AndAlso SiteCategory.EventCancelled Then ' Insert failed
		SiteCategory_list.RestoreCurrentRowFormValues(SiteCategory_list.lRowIndex) ' Restore form values
	End If
	If SiteCategory.CurrentAction = "gridedit" Then ' Grid edit
		SiteCategory.RowType = EW_ROWTYPE_EDIT ' Render edit
	End If
	If SiteCategory.RowType = EW_ROWTYPE_EDIT AndAlso SiteCategory.EventCancelled Then ' update failed
		If SiteCategory.CurrentAction = "gridedit" Then
			SiteCategory_list.RestoreCurrentRowFormValues(SiteCategory_list.lRowIndex) ' Restore form values
		End If
	End If
	If SiteCategory.RowType = EW_ROWTYPE_EDIT Then ' Edit row
		SiteCategory_list.lEditRowCnt = SiteCategory_list.lEditRowCnt + 1
		SiteCategory.RowClientEvents = "onmouseover='this.edit=true;ew_MouseOver(event, this);' onmouseout='ew_MouseOut(event, this);' onclick='ew_Click(event, this);'"
	End If
	If SiteCategory.RowType = EW_ROWTYPE_ADD OrElse SiteCategory.RowType = EW_ROWTYPE_EDIT Then ' Add / Edit row
		SiteCategory.CssClass = "ewTableEditRow"
	End If

	' Render row
	SiteCategory_list.RenderRow()
%>
	<tr<%= SiteCategory.RowAttributes %>>
<% If SiteCategory.RowType = EW_ROWTYPE_ADD OrElse SiteCategory.RowType = EW_ROWTYPE_EDIT Then %>
<%
	If SiteCategory.CurrentAction = "gridedit" Then
		SiteCategory_list.sMultiSelectKey = SiteCategory_list.sMultiSelectKey & "<input type=""hidden"" name=""k" & SiteCategory_list.lRowIndex & "_key"" id=""k" & SiteCategory_list.lRowIndex & "_key"" value=""" & SiteCategory.SiteCategoryID.CurrentValue & """ />"
	End If
%>
<% Else %>
<% If SiteCategory.Export = "" Then %>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= SiteCategory.ViewUrl %>"><img src='images/view.gif' alt='View' title='View' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= SiteCategory.EditUrl %>"><img src='images/edit.gif' alt='Edit' title='Edit' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= SiteCategory.CopyUrl %>"><img src='images/copy.gif' alt='Copy' title='Copy' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= SiteCategory.DeleteUrl %>"><img src='images/delete.gif' alt='Delete' title='Delete' width='16' height='16' border='0'></a>
</span></td>
<%

' Custom list options
For i As Integer = 0 to SiteCategory_list.ListOptions.Items.Count -1
	If SiteCategory_list.ListOptions.Items(i).Visible Then Response.Write(SiteCategory_list.ListOptions.Items(i).BodyCellHtml)
Next
%>
<% End If %>
<% End If %>
	<% If SiteCategory.SiteCategoryTypeID.Visible Then ' SiteCategoryTypeID %>
		<td<%= SiteCategory.SiteCategoryTypeID.CellAttributes %>>
<% If SiteCategory.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<select id="x<%= SiteCategory_list.lRowIndex %>_SiteCategoryTypeID" name="x<%= SiteCategory_list.lRowIndex %>_SiteCategoryTypeID" onchange="ew_UpdateOpt('x<%= SiteCategory_list.lRowIndex %>_ParentCategoryID','x<%= SiteCategory_list.lRowIndex %>_SiteCategoryTypeID',SiteCategory_list.ar_x<%= SiteCategory_list.lRowIndex %>_ParentCategoryID);"<%= SiteCategory.SiteCategoryTypeID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(SiteCategory.SiteCategoryTypeID.EditValue) Then
	arwrk = SiteCategory.SiteCategoryTypeID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), SiteCategory.SiteCategoryTypeID.CurrentValue) Then
			selwrk = " selected=""selected"""
			emptywrk = False
		Else
			selwrk = ""
		End If
%>
<option value="<%= ew_HtmlEncode(arwrk(rowcntwrk)(0)) %>"<%= selwrk %>>
<%= arwrk(rowcntwrk)(1) %>
</option>
<%
	Next
End If
If emptywrk Then SiteCategory.SiteCategoryTypeID.OldValue = ""
%>
</select>
<input type="hidden" name="o<%= SiteCategory_list.lRowIndex %>_SiteCategoryTypeID" id="o<%= SiteCategory_list.lRowIndex %>_SiteCategoryTypeID" value="<%= ew_HTMLEncode(SiteCategory.SiteCategoryTypeID.OldValue) %>" />
<% End If %>
<% If SiteCategory.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<select id="x<%= SiteCategory_list.lRowIndex %>_SiteCategoryTypeID" name="x<%= SiteCategory_list.lRowIndex %>_SiteCategoryTypeID" onchange="ew_UpdateOpt('x<%= SiteCategory_list.lRowIndex %>_ParentCategoryID','x<%= SiteCategory_list.lRowIndex %>_SiteCategoryTypeID',SiteCategory_list.ar_x<%= SiteCategory_list.lRowIndex %>_ParentCategoryID);"<%= SiteCategory.SiteCategoryTypeID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(SiteCategory.SiteCategoryTypeID.EditValue) Then
	arwrk = SiteCategory.SiteCategoryTypeID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), SiteCategory.SiteCategoryTypeID.CurrentValue) Then
			selwrk = " selected=""selected"""
			emptywrk = False
		Else
			selwrk = ""
		End If
%>
<option value="<%= ew_HtmlEncode(arwrk(rowcntwrk)(0)) %>"<%= selwrk %>>
<%= arwrk(rowcntwrk)(1) %>
</option>
<%
	Next
End If
If emptywrk Then SiteCategory.SiteCategoryTypeID.OldValue = ""
%>
</select>
<% End If %>
<% If SiteCategory.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= SiteCategory.SiteCategoryTypeID.ViewAttributes %>><%= SiteCategory.SiteCategoryTypeID.ListViewValue %></div>
<% End If %>
<% If SiteCategory.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<input type="hidden" name="o<%= SiteCategory_list.lRowIndex %>_SiteCategoryID" id="o<%= SiteCategory_list.lRowIndex %>_SiteCategoryID" value="<%= ew_HTMLEncode(SiteCategory.SiteCategoryID.OldValue) %>" />
<% End If %>
<% If SiteCategory.RowType = EW_ROWTYPE_EDIT Then %>
<input type="hidden" name="x<%= SiteCategory_list.lRowIndex %>_SiteCategoryID" id="x<%= SiteCategory_list.lRowIndex %>_SiteCategoryID" value="<%= ew_HTMLEncode(SiteCategory.SiteCategoryID.CurrentValue) %>" />
<% End If %>
</td>
	<% End If %>
	<% If SiteCategory.GroupOrder.Visible Then ' GroupOrder %>
		<td<%= SiteCategory.GroupOrder.CellAttributes %>>
<% If SiteCategory.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<input type="text" name="x<%= SiteCategory_list.lRowIndex %>_GroupOrder" id="x<%= SiteCategory_list.lRowIndex %>_GroupOrder" size="30" value="<%= SiteCategory.GroupOrder.EditValue %>"<%= SiteCategory.GroupOrder.EditAttributes %> />
<input type="hidden" name="o<%= SiteCategory_list.lRowIndex %>_GroupOrder" id="o<%= SiteCategory_list.lRowIndex %>_GroupOrder" value="<%= ew_HTMLEncode(SiteCategory.GroupOrder.OldValue) %>" />
<% End If %>
<% If SiteCategory.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<input type="text" name="x<%= SiteCategory_list.lRowIndex %>_GroupOrder" id="x<%= SiteCategory_list.lRowIndex %>_GroupOrder" size="30" value="<%= SiteCategory.GroupOrder.EditValue %>"<%= SiteCategory.GroupOrder.EditAttributes %> />
<% End If %>
<% If SiteCategory.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= SiteCategory.GroupOrder.ViewAttributes %>><%= SiteCategory.GroupOrder.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	<% If SiteCategory.CategoryName.Visible Then ' CategoryName %>
		<td<%= SiteCategory.CategoryName.CellAttributes %>>
<% If SiteCategory.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<input type="text" name="x<%= SiteCategory_list.lRowIndex %>_CategoryName" id="x<%= SiteCategory_list.lRowIndex %>_CategoryName" size="50" maxlength="255" value="<%= SiteCategory.CategoryName.EditValue %>"<%= SiteCategory.CategoryName.EditAttributes %> />
<input type="hidden" name="o<%= SiteCategory_list.lRowIndex %>_CategoryName" id="o<%= SiteCategory_list.lRowIndex %>_CategoryName" value="<%= ew_HTMLEncode(SiteCategory.CategoryName.OldValue) %>" />
<% End If %>
<% If SiteCategory.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<input type="text" name="x<%= SiteCategory_list.lRowIndex %>_CategoryName" id="x<%= SiteCategory_list.lRowIndex %>_CategoryName" size="50" maxlength="255" value="<%= SiteCategory.CategoryName.EditValue %>"<%= SiteCategory.CategoryName.EditAttributes %> />
<% End If %>
<% If SiteCategory.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= SiteCategory.CategoryName.ViewAttributes %>><%= SiteCategory.CategoryName.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	<% If SiteCategory.ParentCategoryID.Visible Then ' ParentCategoryID %>
		<td<%= SiteCategory.ParentCategoryID.CellAttributes %>>
<% If SiteCategory.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<select id="x<%= SiteCategory_list.lRowIndex %>_ParentCategoryID" name="x<%= SiteCategory_list.lRowIndex %>_ParentCategoryID"<%= SiteCategory.ParentCategoryID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(SiteCategory.ParentCategoryID.EditValue) Then
	arwrk = SiteCategory.ParentCategoryID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), SiteCategory.ParentCategoryID.CurrentValue) Then
			selwrk = " selected=""selected"""
			emptywrk = False
		Else
			selwrk = ""
		End If
%>
<option value="<%= ew_HtmlEncode(arwrk(rowcntwrk)(0)) %>"<%= selwrk %>>
<%= arwrk(rowcntwrk)(1) %>
</option>
<%
	Next
End If
If emptywrk Then SiteCategory.ParentCategoryID.OldValue = ""
%>
</select>
<%
jswrk = "" ' Initialise
If ew_IsArrayList(SiteCategory.ParentCategoryID.EditValue) Then
	arwrk = SiteCategory.ParentCategoryID.EditValue
	For rowcntwrk As Integer = 1 To arwrk.Count - 1
		If jswrk <> "" Then jswrk = jswrk & ","
		jswrk = jswrk & "['" & ew_JsEncode(arwrk(rowcntwrk)(0)) & "'," ' Value
		jswrk = jswrk & "'" & ew_JsEncode(arwrk(rowcntwrk)(1)) & "'," ' Display field 1
		jswrk = jswrk & "'" & ew_JsEncode(arwrk(rowcntwrk)(2)) & "'," ' Display field 2
		jswrk = jswrk & "'" & ew_JsEncode(arwrk(rowcntwrk)(3)) & "']" ' Filter field
	Next
End If
%>
<script type="text/javascript">
<!--
SiteCategory_list.ar_x<%= SiteCategory_list.lRowIndex %>_ParentCategoryID = [<%= jswrk %>];
//-->
</script>
<input type="hidden" name="o<%= SiteCategory_list.lRowIndex %>_ParentCategoryID" id="o<%= SiteCategory_list.lRowIndex %>_ParentCategoryID" value="<%= ew_HTMLEncode(SiteCategory.ParentCategoryID.OldValue) %>" />
<% End If %>
<% If SiteCategory.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<select id="x<%= SiteCategory_list.lRowIndex %>_ParentCategoryID" name="x<%= SiteCategory_list.lRowIndex %>_ParentCategoryID"<%= SiteCategory.ParentCategoryID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(SiteCategory.ParentCategoryID.EditValue) Then
	arwrk = SiteCategory.ParentCategoryID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), SiteCategory.ParentCategoryID.CurrentValue) Then
			selwrk = " selected=""selected"""
			emptywrk = False
		Else
			selwrk = ""
		End If
%>
<option value="<%= ew_HtmlEncode(arwrk(rowcntwrk)(0)) %>"<%= selwrk %>>
<%= arwrk(rowcntwrk)(1) %>
</option>
<%
	Next
End If
If emptywrk Then SiteCategory.ParentCategoryID.OldValue = ""
%>
</select>
<%
jswrk = "" ' Initialise
If ew_IsArrayList(SiteCategory.ParentCategoryID.EditValue) Then
	arwrk = SiteCategory.ParentCategoryID.EditValue
	For rowcntwrk As Integer = 1 To arwrk.Count - 1
		If jswrk <> "" Then jswrk = jswrk & ","
		jswrk = jswrk & "['" & ew_JsEncode(arwrk(rowcntwrk)(0)) & "'," ' Value
		jswrk = jswrk & "'" & ew_JsEncode(arwrk(rowcntwrk)(1)) & "'," ' Display field 1
		jswrk = jswrk & "'" & ew_JsEncode(arwrk(rowcntwrk)(2)) & "'," ' Display field 2
		jswrk = jswrk & "'" & ew_JsEncode(arwrk(rowcntwrk)(3)) & "']" ' Filter field
	Next
End If
%>
<script type="text/javascript">
<!--
SiteCategory_list.ar_x<%= SiteCategory_list.lRowIndex %>_ParentCategoryID = [<%= jswrk %>];
//-->
</script>
<% End If %>
<% If SiteCategory.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= SiteCategory.ParentCategoryID.ViewAttributes %>><%= SiteCategory.ParentCategoryID.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	<% If SiteCategory.CategoryFileName.Visible Then ' CategoryFileName %>
		<td<%= SiteCategory.CategoryFileName.CellAttributes %>>
<% If SiteCategory.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<input type="text" name="x<%= SiteCategory_list.lRowIndex %>_CategoryFileName" id="x<%= SiteCategory_list.lRowIndex %>_CategoryFileName" size="30" maxlength="255" value="<%= SiteCategory.CategoryFileName.EditValue %>"<%= SiteCategory.CategoryFileName.EditAttributes %> />
<input type="hidden" name="o<%= SiteCategory_list.lRowIndex %>_CategoryFileName" id="o<%= SiteCategory_list.lRowIndex %>_CategoryFileName" value="<%= ew_HTMLEncode(SiteCategory.CategoryFileName.OldValue) %>" />
<% End If %>
<% If SiteCategory.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<input type="text" name="x<%= SiteCategory_list.lRowIndex %>_CategoryFileName" id="x<%= SiteCategory_list.lRowIndex %>_CategoryFileName" size="30" maxlength="255" value="<%= SiteCategory.CategoryFileName.EditValue %>"<%= SiteCategory.CategoryFileName.EditAttributes %> />
<% End If %>
<% If SiteCategory.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= SiteCategory.CategoryFileName.ViewAttributes %>><%= SiteCategory.CategoryFileName.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	<% If SiteCategory.SiteCategoryGroupID.Visible Then ' SiteCategoryGroupID %>
		<td<%= SiteCategory.SiteCategoryGroupID.CellAttributes %>>
<% If SiteCategory.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<select id="x<%= SiteCategory_list.lRowIndex %>_SiteCategoryGroupID" name="x<%= SiteCategory_list.lRowIndex %>_SiteCategoryGroupID"<%= SiteCategory.SiteCategoryGroupID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(SiteCategory.SiteCategoryGroupID.EditValue) Then
	arwrk = SiteCategory.SiteCategoryGroupID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), SiteCategory.SiteCategoryGroupID.CurrentValue) Then
			selwrk = " selected=""selected"""
			emptywrk = False
		Else
			selwrk = ""
		End If
%>
<option value="<%= ew_HtmlEncode(arwrk(rowcntwrk)(0)) %>"<%= selwrk %>>
<%= arwrk(rowcntwrk)(1) %>
</option>
<%
	Next
End If
If emptywrk Then SiteCategory.SiteCategoryGroupID.OldValue = ""
%>
</select>
<input type="hidden" name="o<%= SiteCategory_list.lRowIndex %>_SiteCategoryGroupID" id="o<%= SiteCategory_list.lRowIndex %>_SiteCategoryGroupID" value="<%= ew_HTMLEncode(SiteCategory.SiteCategoryGroupID.OldValue) %>" />
<% End If %>
<% If SiteCategory.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<select id="x<%= SiteCategory_list.lRowIndex %>_SiteCategoryGroupID" name="x<%= SiteCategory_list.lRowIndex %>_SiteCategoryGroupID"<%= SiteCategory.SiteCategoryGroupID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(SiteCategory.SiteCategoryGroupID.EditValue) Then
	arwrk = SiteCategory.SiteCategoryGroupID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), SiteCategory.SiteCategoryGroupID.CurrentValue) Then
			selwrk = " selected=""selected"""
			emptywrk = False
		Else
			selwrk = ""
		End If
%>
<option value="<%= ew_HtmlEncode(arwrk(rowcntwrk)(0)) %>"<%= selwrk %>>
<%= arwrk(rowcntwrk)(1) %>
</option>
<%
	Next
End If
If emptywrk Then SiteCategory.SiteCategoryGroupID.OldValue = ""
%>
</select>
<% End If %>
<% If SiteCategory.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= SiteCategory.SiteCategoryGroupID.ViewAttributes %>><%= SiteCategory.SiteCategoryGroupID.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	</tr>
<% If SiteCategory.RowType = EW_ROWTYPE_ADD Then %>
<script language="JavaScript">
<!--
ew_UpdateOpts([['x<%= SiteCategory_list.lRowIndex %>_ParentCategoryID','x<%= SiteCategory_list.lRowIndex %>_SiteCategoryTypeID',SiteCategory_list.ar_x<%= SiteCategory_list.lRowIndex %>_ParentCategoryID]]);
//-->
</script>
<% End If %>
<% If SiteCategory.RowType = EW_ROWTYPE_EDIT Then %>
<script language="JavaScript">
<!--
ew_UpdateOpts([['x<%= SiteCategory_list.lRowIndex %>_ParentCategoryID','x<%= SiteCategory_list.lRowIndex %>_SiteCategoryTypeID',SiteCategory_list.ar_x<%= SiteCategory_list.lRowIndex %>_ParentCategoryID]]);
//-->
</script>
<% End If %>
<%
	End If
Loop
%>
</tbody>
</table>
<% End If %>
<% If SiteCategory.CurrentAction = "gridadd" Then %>
<input type="hidden" name="a_list" id="a_list" value="gridinsert" />
<input type="hidden" name="key_count" id="key_count" value="<%= SiteCategory_list.lRowIndex %>" />
<% End If %>
<% If SiteCategory.CurrentAction = "gridedit" Then %>
<input type="hidden" name="a_list" id="a_list" value="gridupdate" />
<input type="hidden" name="key_count" id="key_count" value="<%= SiteCategory_list.lRowIndex %>" />
<%= SiteCategory_list.sMultiSelectKey %>
<% End If %>
</form>
<%

' Close recordset
Rs.Close()
Rs.Dispose()
%>
</div>
<% If SiteCategory_list.lTotalRecs > 0 Then %>
<% If SiteCategory.Export = "" Then %>
<div class="ewGridLowerPanel">
<% If SiteCategory.CurrentAction <> "gridadd" AndAlso SiteCategory.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If SiteCategory_list.Pager Is Nothing Then SiteCategory_list.Pager = New cPrevNextPager(SiteCategory_list.lStartRec, SiteCategory_list.lDisplayRecs, SiteCategory_list.lTotalRecs) %>
<% If SiteCategory_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If SiteCategory_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= SiteCategory_list.PageUrl %>start=<%= SiteCategory_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If SiteCategory_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= SiteCategory_list.PageUrl %>start=<%= SiteCategory_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= SiteCategory_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If SiteCategory_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= SiteCategory_list.PageUrl %>start=<%= SiteCategory_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If SiteCategory_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= SiteCategory_list.PageUrl %>start=<%= SiteCategory_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= SiteCategory_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= SiteCategory_list.Pager.FromIndex %> to <%= SiteCategory_list.Pager.ToIndex %> of <%= SiteCategory_list.Pager.RecordCount %></span>
<% Else %>
	<% If SiteCategory_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If SiteCategory_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="SiteCategory" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If SiteCategory_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If SiteCategory_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If SiteCategory_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If SiteCategory.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<% 'If SiteCategory_list.lTotalRecs > 0 Then %>
<div class="aspnetmaker">
<% If SiteCategory.CurrentAction <> "gridadd" AndAlso SiteCategory.CurrentAction <> "gridedit" Then ' Not grid add/edit mode %>
<a href="<%= SiteCategory.AddUrl %>">Add</a>&nbsp;&nbsp;
<a href="<%= SiteCategory_list.PageUrl %>a=gridadd">Grid Add</a>&nbsp;&nbsp;
<% If SiteCategory_list.lTotalRecs > 0 Then %>
<a href="<%= SiteCategory_list.PageUrl %>a=gridedit">Grid Edit</a>&nbsp;&nbsp;
<% End If %>
<% Else ' Grid add/edit mode %>
<% If SiteCategory.CurrentAction = "gridadd" Then %>
<a href="" onclick="f=ew_GetForm('fSiteCategorylist');if (SiteCategory_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/insert.gif' alt='Insert' title='Insert' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<% If SiteCategory.CurrentAction = "gridedit" Then %>
<a href="" onclick="f=ew_GetForm('fSiteCategorylist');if (SiteCategory_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/update.gif' alt='Save' title='Save' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<a href="<%= SiteCategory_list.PageUrl %>a=cancel"><img src='images/cancel.gif' alt='Cancel' title='Cancel' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
</div>
<% 'End If %>
</div>
<% End If %>
<% End If %>
</td></tr></table>
<% If SiteCategory.Export = "" AndAlso SiteCategory.CurrentAction = "" Then %>
<script type="text/javascript">
<!--
//ew_ToggleSearchPanel(SiteCategory_list); // uncomment to init search panel as collapsed
//-->
</script>
<% End If %>
<% If SiteCategory.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
