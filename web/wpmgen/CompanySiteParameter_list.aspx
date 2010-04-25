<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="CompanySiteParameter_list.aspx.vb" Inherits="CompanySiteParameter_list" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If CompanySiteParameter.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var CompanySiteParameter_list = new ew_Page("CompanySiteParameter_list");
// page properties
CompanySiteParameter_list.PageID = "list"; // page ID
var EW_PAGE_ID = CompanySiteParameter_list.PageID; // for backward compatibility
// extend page with ValidateForm function
CompanySiteParameter_list.ValidateForm = function(fobj) {
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
		elm = fobj.elements["x" + infix + "_CompanyID"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field - Site");
		elm = fobj.elements["x" + infix + "_SiteParameterTypeID"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field -  Parameter");
		elm = fobj.elements["x" + infix + "_SortOrder"];
		if (elm && !ew_CheckInteger(elm.value))
			return ew_OnError(this, elm, "Incorrect integer - Process Order");
		} // End Grid Add checking
	}
	if (fobj.a_list && fobj.a_list.value == "gridinsert" && addcnt == 0) { // No row added
		alert("No records to be added");
		return false;
	}
	return true;
}
// Extend page with empty row check
CompanySiteParameter_list.EmptyRow = function(fobj, infix) {
	if (ew_ValueChanged(fobj, infix, "CompanyID")) return false;
	if (ew_ValueChanged(fobj, infix, "zPageID")) return false;
	if (ew_ValueChanged(fobj, infix, "SiteCategoryGroupID")) return false;
	if (ew_ValueChanged(fobj, infix, "SiteParameterTypeID")) return false;
	if (ew_ValueChanged(fobj, infix, "SortOrder")) return false;
	return true;
}
<% If EW_CLIENT_VALIDATE Then %>
CompanySiteParameter_list.ValidateRequired = true; // uses JavaScript validation
<% Else %>
CompanySiteParameter_list.ValidateRequired = false; // no JavaScript validation
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
<% If CompanySiteParameter.Export = "" Then %>
<% End If %>
<%
If CompanySiteParameter.CurrentAction = "gridadd" Then CompanySiteParameter.CurrentFilter = "0=1"

' Load recordset
Rs = CompanySiteParameter_list.LoadRecordset()
If CompanySiteParameter.CurrentAction = "gridadd" Then
	CompanySiteParameter_list.lStartRec = 1
	If CompanySiteParameter_list.lDisplayRecs <= 0 Then CompanySiteParameter_list.lDisplayRecs = 25
	CompanySiteParameter_list.lTotalRecs = CompanySiteParameter_list.lDisplayRecs
	CompanySiteParameter_list.lStopRec = CompanySiteParameter_list.lDisplayRecs
Else
	CompanySiteParameter_list.lStartRec = 1
	If CompanySiteParameter_list.lDisplayRecs <= 0 Then ' Display all records
		CompanySiteParameter_list.lDisplayRecs = CompanySiteParameter_list.lTotalRecs
	End If
	If Not (CompanySiteParameter.ExportAll AndAlso CompanySiteParameter.Export <> "") Then
		CompanySiteParameter_list.SetUpStartRec() ' Set up start record position
	End If
End If
%>
<p><span class="aspnetmaker" style="white-space: nowrap;">TABLE: Site Parameter
<% If CompanySiteParameter.Export = "" AndAlso CompanySiteParameter.CurrentAction = "" Then %>
&nbsp;&nbsp;<a href="<%= CompanySiteParameter_list.PageUrl %>export=excel"><img src='images/exportxls.gif' alt='Export to Excel' title='Export to Excel' width='16' height='16' border='0'></a>
<% End If %>
</span></p>
<% If CompanySiteParameter.Export = "" AndAlso CompanySiteParameter.CurrentAction = "" Then %>
<a href="javascript:ew_ToggleSearchPanel(CompanySiteParameter_list);" style="text-decoration: none;"><img id="CompanySiteParameter_list_SearchImage" src="images/collapse.gif" alt="" width="9" height="9" border="0"></a><span class="aspnetmaker">&nbsp;Search</span><br>
<div id="CompanySiteParameter_list_SearchPanel">
<form name="fCompanySiteParameterlistsrch" id="fCompanySiteParameterlistsrch" class="ewForm">
<input type="hidden" id="t" name="t" value="CompanySiteParameter" />
<table class="ewBasicSearch">
	<tr>
		<td><span class="aspnetmaker">
			<a href="<%= CompanySiteParameter_list.PageUrl %>cmd=reset">Show all</a>&nbsp;
			<a href="CompanySiteParameter_srch.aspx">Advanced Search</a>&nbsp;
		</span></td>
	</tr>
</table>
</form>
</div>
<% End If %>
<% CompanySiteParameter_list.ShowMessage() %>
<br />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<% If CompanySiteParameter.Export = "" Then %>
<div class="ewGridUpperPanel">
<% If CompanySiteParameter.CurrentAction <> "gridadd" AndAlso CompanySiteParameter.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If CompanySiteParameter_list.Pager Is Nothing Then CompanySiteParameter_list.Pager = New cPrevNextPager(CompanySiteParameter_list.lStartRec, CompanySiteParameter_list.lDisplayRecs, CompanySiteParameter_list.lTotalRecs) %>
<% If CompanySiteParameter_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If CompanySiteParameter_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= CompanySiteParameter_list.PageUrl %>start=<%= CompanySiteParameter_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If CompanySiteParameter_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= CompanySiteParameter_list.PageUrl %>start=<%= CompanySiteParameter_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= CompanySiteParameter_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If CompanySiteParameter_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= CompanySiteParameter_list.PageUrl %>start=<%= CompanySiteParameter_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If CompanySiteParameter_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= CompanySiteParameter_list.PageUrl %>start=<%= CompanySiteParameter_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= CompanySiteParameter_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= CompanySiteParameter_list.Pager.FromIndex %> to <%= CompanySiteParameter_list.Pager.ToIndex %> of <%= CompanySiteParameter_list.Pager.RecordCount %></span>
<% Else %>
	<% If CompanySiteParameter_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If CompanySiteParameter_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="CompanySiteParameter" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If CompanySiteParameter_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If CompanySiteParameter_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If CompanySiteParameter_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If CompanySiteParameter.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<div class="aspnetmaker">
<% If CompanySiteParameter.CurrentAction <> "gridadd" AndAlso CompanySiteParameter.CurrentAction <> "gridedit" Then ' Not grid add/edit mode %>
<a href="<%= CompanySiteParameter.AddUrl %>">Add</a>&nbsp;&nbsp;
<a href="<%= CompanySiteParameter_list.PageUrl %>a=gridadd">Grid Add</a>&nbsp;&nbsp;
<% If CompanySiteParameter_list.lTotalRecs > 0 Then %>
<a href="<%= CompanySiteParameter_list.PageUrl %>a=gridedit">Grid Edit</a>&nbsp;&nbsp;
<% End If %>
<% Else ' Grid add/edit mode %>
<% If CompanySiteParameter.CurrentAction = "gridadd" Then %>
<a href="" onclick="f=ew_GetForm('fCompanySiteParameterlist');if (CompanySiteParameter_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/insert.gif' alt='Insert' title='Insert' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<% If CompanySiteParameter.CurrentAction = "gridedit" Then %>
<a href="" onclick="f=ew_GetForm('fCompanySiteParameterlist');if (CompanySiteParameter_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/update.gif' alt='Save' title='Save' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<a href="<%= CompanySiteParameter_list.PageUrl %>a=cancel"><img src='images/cancel.gif' alt='Cancel' title='Cancel' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
</div>
</div>
<% End If %>
<div class="ewGridMiddlePanel">
<form name="fCompanySiteParameterlist" id="fCompanySiteParameterlist" class="ewForm" method="post">
<input type="hidden" name="t" id="t" value="CompanySiteParameter" />
<% If CompanySiteParameter_list.lTotalRecs > 0 Then %>
<table cellspacing="0" rowhighlightclass="ewTableHighlightRow" rowselectclass="ewTableSelectRow" roweditclass="ewTableEditRow" class="ewTable ewTableSeparate">
<%
	CompanySiteParameter_list.lOptionCnt = 0
	CompanySiteParameter_list.lOptionCnt = CompanySiteParameter_list.lOptionCnt + 1 ' View
	CompanySiteParameter_list.lOptionCnt = CompanySiteParameter_list.lOptionCnt + 1 ' Edit
	CompanySiteParameter_list.lOptionCnt = CompanySiteParameter_list.lOptionCnt + 1 ' Copy
	CompanySiteParameter_list.lOptionCnt = CompanySiteParameter_list.lOptionCnt + 1 ' Delete
	CompanySiteParameter_list.lOptionCnt = CompanySiteParameter_list.lOptionCnt + CompanySiteParameter_list.ListOptions.Items.Count ' Custom list options
%>
<%= CompanySiteParameter.TableCustomInnerHTML %>
<thead><!-- Table header -->
	<tr class="ewTableHeader">
<% If CompanySiteParameter.Export = "" Then %>
<% If CompanySiteParameter.CurrentAction <> "gridadd" AndAlso CompanySiteParameter.CurrentAction <> "gridedit" Then %>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<%

' Custom list options
For i As Integer = 0 to CompanySiteParameter_list.ListOptions.Items.Count -1
	If CompanySiteParameter_list.ListOptions.Items(i).Visible Then Response.Write(CompanySiteParameter_list.ListOptions.Items(i).HeaderCellHtml)
Next
%>
<% End If %>
<% End If %>
<% If CompanySiteParameter.CompanyID.Visible Then ' CompanyID %>
	<% If CompanySiteParameter.SortUrl(CompanySiteParameter.CompanyID) = "" Then %>
		<td>Site</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= CompanySiteParameter.SortUrl(CompanySiteParameter.CompanyID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Site</td><td style="width: 10px;"><% If CompanySiteParameter.CompanyID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf CompanySiteParameter.CompanyID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If CompanySiteParameter.zPageID.Visible Then ' PageID %>
	<% If CompanySiteParameter.SortUrl(CompanySiteParameter.zPageID) = "" Then %>
		<td>Page</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= CompanySiteParameter.SortUrl(CompanySiteParameter.zPageID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Page</td><td style="width: 10px;"><% If CompanySiteParameter.zPageID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf CompanySiteParameter.zPageID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If CompanySiteParameter.SiteCategoryGroupID.Visible Then ' SiteCategoryGroupID %>
	<% If CompanySiteParameter.SortUrl(CompanySiteParameter.SiteCategoryGroupID) = "" Then %>
		<td>Category Group</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= CompanySiteParameter.SortUrl(CompanySiteParameter.SiteCategoryGroupID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Category Group</td><td style="width: 10px;"><% If CompanySiteParameter.SiteCategoryGroupID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf CompanySiteParameter.SiteCategoryGroupID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If CompanySiteParameter.SiteParameterTypeID.Visible Then ' SiteParameterTypeID %>
	<% If CompanySiteParameter.SortUrl(CompanySiteParameter.SiteParameterTypeID) = "" Then %>
		<td> Parameter</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= CompanySiteParameter.SortUrl(CompanySiteParameter.SiteParameterTypeID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td> Parameter</td><td style="width: 10px;"><% If CompanySiteParameter.SiteParameterTypeID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf CompanySiteParameter.SiteParameterTypeID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If CompanySiteParameter.SortOrder.Visible Then ' SortOrder %>
	<% If CompanySiteParameter.SortUrl(CompanySiteParameter.SortOrder) = "" Then %>
		<td>Process Order</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= CompanySiteParameter.SortUrl(CompanySiteParameter.SortOrder) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Process Order</td><td style="width: 10px;"><% If CompanySiteParameter.SortOrder.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf CompanySiteParameter.SortOrder.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
	</tr>
</thead>
<tbody><!-- Table body -->
<%
If (CompanySiteParameter.ExportAll AndAlso CompanySiteParameter.Export <> "") Then
	CompanySiteParameter_list.lStopRec = CompanySiteParameter_list.lTotalRecs
Else
	CompanySiteParameter_list.lStopRec = CompanySiteParameter_list.lStartRec + CompanySiteParameter_list.lDisplayRecs - 1 ' Set the last record to display
End If
If CompanySiteParameter.CurrentAction = "gridadd" AndAlso CompanySiteParameter_list.lStopRec = -1 Then
	CompanySiteParameter_list.lStopRec = EW_GRIDADD_ROWS
End If 

' Move to first record
For i As Integer = 1 to CompanySiteParameter_list.lStartRec - 1
	If Rs.Read() Then	CompanySiteParameter_list.lRecCnt = CompanySiteParameter_list.lRecCnt + 1
Next		
CompanySiteParameter_list.lRowCnt = 0
CompanySiteParameter_list.lEditRowCnt = 0
If CompanySiteParameter.CurrentAction = "edit" Then CompanySiteParameter_list.lRowIndex = 1
If CompanySiteParameter.CurrentAction = "gridadd" Then CompanySiteParameter_list.lRowIndex = 0
If CompanySiteParameter.CurrentAction = "gridedit" Then CompanySiteParameter_list.lRowIndex = 0

' Output data rows
Do While (CompanySiteParameter.CurrentAction = "gridadd" OrElse Rs.Read()) AndAlso (CompanySiteParameter_list.lRecCnt < CompanySiteParameter_list.lStopRec)
	CompanySiteParameter_list.lRecCnt = CompanySiteParameter_list.lRecCnt + 1
	If CompanySiteParameter_list.lRecCnt >= CompanySiteParameter_list.lStartRec Then
		CompanySiteParameter_list.lRowCnt = CompanySiteParameter_list.lRowCnt + 1
		If CompanySiteParameter.CurrentAction = "gridadd" OrElse CompanySiteParameter.CurrentAction = "gridedit" Then CompanySiteParameter_list.lRowIndex = CompanySiteParameter_list.lRowIndex + 1
	CompanySiteParameter.CssClass = ""
	CompanySiteParameter.CssStyle = ""
	CompanySiteParameter.RowClientEvents = "onmouseover='ew_MouseOver(event, this);' onmouseout='ew_MouseOut(event, this);' onclick='ew_Click(event, this);'"
	If CompanySiteParameter.CurrentAction = "gridadd" Then
		CompanySiteParameter_list.LoadDefaultValues() ' Load default values
	Else
		CompanySiteParameter_list.LoadRowValues(Rs) ' Load row values
	End If
	CompanySiteParameter.RowType = EW_ROWTYPE_VIEW ' Render view
	If CompanySiteParameter.CurrentAction = "gridadd" Then ' Grid add
		CompanySiteParameter.RowType = EW_ROWTYPE_ADD ' Render add
	End If
	If CompanySiteParameter.CurrentAction = "gridadd" AndAlso CompanySiteParameter.EventCancelled Then ' Insert failed
		CompanySiteParameter_list.RestoreCurrentRowFormValues(CompanySiteParameter_list.lRowIndex) ' Restore form values
	End If
	If CompanySiteParameter.CurrentAction = "edit" Then
		If CompanySiteParameter_list.CheckInlineEditKey() AndAlso CompanySiteParameter_list.lEditRowCnt = 0 Then ' Inline edit
			CompanySiteParameter.RowType = EW_ROWTYPE_EDIT ' Render edit
		End If
	End If
	If CompanySiteParameter.CurrentAction = "gridedit" Then ' Grid edit
		CompanySiteParameter.RowType = EW_ROWTYPE_EDIT ' Render edit
	End If
	If CompanySiteParameter.RowType = EW_ROWTYPE_EDIT AndAlso CompanySiteParameter.EventCancelled Then ' update failed
		If CompanySiteParameter.CurrentAction = "edit" Then
			CompanySiteParameter_list.RestoreFormValues() ' Restore form values
		End If
		If CompanySiteParameter.CurrentAction = "gridedit" Then
			CompanySiteParameter_list.RestoreCurrentRowFormValues(CompanySiteParameter_list.lRowIndex) ' Restore form values
		End If
	End If
	If CompanySiteParameter.RowType = EW_ROWTYPE_EDIT Then ' Edit row
		CompanySiteParameter_list.lEditRowCnt = CompanySiteParameter_list.lEditRowCnt + 1
		CompanySiteParameter.RowClientEvents = "onmouseover='this.edit=true;ew_MouseOver(event, this);' onmouseout='ew_MouseOut(event, this);' onclick='ew_Click(event, this);'"
	End If
	If CompanySiteParameter.RowType = EW_ROWTYPE_ADD OrElse CompanySiteParameter.RowType = EW_ROWTYPE_EDIT Then ' Add / Edit row
		CompanySiteParameter.CssClass = "ewTableEditRow"
	End If

	' Render row
	CompanySiteParameter_list.RenderRow()
%>
	<tr<%= CompanySiteParameter.RowAttributes %>>
<% If CompanySiteParameter.RowType = EW_ROWTYPE_ADD OrElse CompanySiteParameter.RowType = EW_ROWTYPE_EDIT Then %>
<% If CompanySiteParameter.CurrentAction = "edit" Then %>
<td colspan="<%= CompanySiteParameter_list.lOptionCnt %>" align="right"><span class="aspnetmaker">
<a href="" onclick="f=ew_GetForm('fCompanySiteParameterlist');if (CompanySiteParameter_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/update.gif' alt='Update' title='Update' width='16' height='16' border='0'></a>&nbsp;<a href="<%= CompanySiteParameter_list.PageUrl %>a=cancel"><img src='images/cancel.gif' alt='Cancel' title='Cancel' width='16' height='16' border='0'></a>
<input type="hidden" name="a_list" id="a_list" value="update" />
</span></td>
<% End If %>
<%
	If CompanySiteParameter.CurrentAction = "gridedit" Then
		CompanySiteParameter_list.sMultiSelectKey = CompanySiteParameter_list.sMultiSelectKey & "<input type=""hidden"" name=""k" & CompanySiteParameter_list.lRowIndex & "_key"" id=""k" & CompanySiteParameter_list.lRowIndex & "_key"" value=""" & CompanySiteParameter.CompanySiteParameterID.CurrentValue & """ />"
	End If
%>
<% Else %>
<% If CompanySiteParameter.Export = "" Then %>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= CompanySiteParameter.ViewUrl %>"><img src='images/view.gif' alt='View' title='View' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= CompanySiteParameter.EditUrl %>"><img src='images/edit.gif' alt='Edit' title='Edit' width='16' height='16' border='0'></a><span class="ewSeparator">&nbsp;|&nbsp;</span><a href="<%= CompanySiteParameter.InlineEditUrl %>"><img src='images/inlineedit.gif' alt='Inline Edit' title='Inline Edit' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= CompanySiteParameter.CopyUrl %>"><img src='images/copy.gif' alt='Copy' title='Copy' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= CompanySiteParameter.DeleteUrl %>"><img src='images/delete.gif' alt='Delete' title='Delete' width='16' height='16' border='0'></a>
</span></td>
<%

' Custom list options
For i As Integer = 0 to CompanySiteParameter_list.ListOptions.Items.Count -1
	If CompanySiteParameter_list.ListOptions.Items(i).Visible Then Response.Write(CompanySiteParameter_list.ListOptions.Items(i).BodyCellHtml)
Next
%>
<% End If %>
<% End If %>
	<% If CompanySiteParameter.CompanyID.Visible Then ' CompanyID %>
		<td<%= CompanySiteParameter.CompanyID.CellAttributes %>>
<% If CompanySiteParameter.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<select id="x<%= CompanySiteParameter_list.lRowIndex %>_CompanyID" name="x<%= CompanySiteParameter_list.lRowIndex %>_CompanyID" onchange="ew_UpdateOpt('x<%= CompanySiteParameter_list.lRowIndex %>_zPageID','x<%= CompanySiteParameter_list.lRowIndex %>_CompanyID',CompanySiteParameter_list.ar_x<%= CompanySiteParameter_list.lRowIndex %>_zPageID);"<%= CompanySiteParameter.CompanyID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(CompanySiteParameter.CompanyID.EditValue) Then
	arwrk = CompanySiteParameter.CompanyID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), CompanySiteParameter.CompanyID.CurrentValue) Then
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
If emptywrk Then CompanySiteParameter.CompanyID.OldValue = ""
%>
</select>
<input type="hidden" name="o<%= CompanySiteParameter_list.lRowIndex %>_CompanyID" id="o<%= CompanySiteParameter_list.lRowIndex %>_CompanyID" value="<%= ew_HTMLEncode(CompanySiteParameter.CompanyID.OldValue) %>" />
<% End If %>
<% If CompanySiteParameter.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<select id="x<%= CompanySiteParameter_list.lRowIndex %>_CompanyID" name="x<%= CompanySiteParameter_list.lRowIndex %>_CompanyID" onchange="ew_UpdateOpt('x<%= CompanySiteParameter_list.lRowIndex %>_zPageID','x<%= CompanySiteParameter_list.lRowIndex %>_CompanyID',CompanySiteParameter_list.ar_x<%= CompanySiteParameter_list.lRowIndex %>_zPageID);"<%= CompanySiteParameter.CompanyID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(CompanySiteParameter.CompanyID.EditValue) Then
	arwrk = CompanySiteParameter.CompanyID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), CompanySiteParameter.CompanyID.CurrentValue) Then
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
If emptywrk Then CompanySiteParameter.CompanyID.OldValue = ""
%>
</select>
<% End If %>
<% If CompanySiteParameter.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= CompanySiteParameter.CompanyID.ViewAttributes %>><%= CompanySiteParameter.CompanyID.ListViewValue %></div>
<% End If %>
<% If CompanySiteParameter.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<input type="hidden" name="o<%= CompanySiteParameter_list.lRowIndex %>_CompanySiteParameterID" id="o<%= CompanySiteParameter_list.lRowIndex %>_CompanySiteParameterID" value="<%= ew_HTMLEncode(CompanySiteParameter.CompanySiteParameterID.OldValue) %>" />
<% End If %>
<% If CompanySiteParameter.RowType = EW_ROWTYPE_EDIT Then %>
<input type="hidden" name="x<%= CompanySiteParameter_list.lRowIndex %>_CompanySiteParameterID" id="x<%= CompanySiteParameter_list.lRowIndex %>_CompanySiteParameterID" value="<%= ew_HTMLEncode(CompanySiteParameter.CompanySiteParameterID.CurrentValue) %>" />
<% End If %>
</td>
	<% End If %>
	<% If CompanySiteParameter.zPageID.Visible Then ' PageID %>
		<td<%= CompanySiteParameter.zPageID.CellAttributes %>>
<% If CompanySiteParameter.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<select id="x<%= CompanySiteParameter_list.lRowIndex %>_zPageID" name="x<%= CompanySiteParameter_list.lRowIndex %>_zPageID"<%= CompanySiteParameter.zPageID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(CompanySiteParameter.zPageID.EditValue) Then
	arwrk = CompanySiteParameter.zPageID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), CompanySiteParameter.zPageID.CurrentValue) Then
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
If emptywrk Then CompanySiteParameter.zPageID.OldValue = ""
%>
</select>
<%
jswrk = "" ' Initialise
If ew_IsArrayList(CompanySiteParameter.zPageID.EditValue) Then
	arwrk = CompanySiteParameter.zPageID.EditValue
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
CompanySiteParameter_list.ar_x<%= CompanySiteParameter_list.lRowIndex %>_zPageID = [<%= jswrk %>];
//-->
</script>
<input type="hidden" name="o<%= CompanySiteParameter_list.lRowIndex %>_zPageID" id="o<%= CompanySiteParameter_list.lRowIndex %>_zPageID" value="<%= ew_HTMLEncode(CompanySiteParameter.zPageID.OldValue) %>" />
<% End If %>
<% If CompanySiteParameter.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<select id="x<%= CompanySiteParameter_list.lRowIndex %>_zPageID" name="x<%= CompanySiteParameter_list.lRowIndex %>_zPageID"<%= CompanySiteParameter.zPageID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(CompanySiteParameter.zPageID.EditValue) Then
	arwrk = CompanySiteParameter.zPageID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), CompanySiteParameter.zPageID.CurrentValue) Then
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
If emptywrk Then CompanySiteParameter.zPageID.OldValue = ""
%>
</select>
<%
jswrk = "" ' Initialise
If ew_IsArrayList(CompanySiteParameter.zPageID.EditValue) Then
	arwrk = CompanySiteParameter.zPageID.EditValue
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
CompanySiteParameter_list.ar_x<%= CompanySiteParameter_list.lRowIndex %>_zPageID = [<%= jswrk %>];
//-->
</script>
<% End If %>
<% If CompanySiteParameter.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= CompanySiteParameter.zPageID.ViewAttributes %>><%= CompanySiteParameter.zPageID.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	<% If CompanySiteParameter.SiteCategoryGroupID.Visible Then ' SiteCategoryGroupID %>
		<td<%= CompanySiteParameter.SiteCategoryGroupID.CellAttributes %>>
<% If CompanySiteParameter.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<select id="x<%= CompanySiteParameter_list.lRowIndex %>_SiteCategoryGroupID" name="x<%= CompanySiteParameter_list.lRowIndex %>_SiteCategoryGroupID"<%= CompanySiteParameter.SiteCategoryGroupID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(CompanySiteParameter.SiteCategoryGroupID.EditValue) Then
	arwrk = CompanySiteParameter.SiteCategoryGroupID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), CompanySiteParameter.SiteCategoryGroupID.CurrentValue) Then
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
If emptywrk Then CompanySiteParameter.SiteCategoryGroupID.OldValue = ""
%>
</select>
<input type="hidden" name="o<%= CompanySiteParameter_list.lRowIndex %>_SiteCategoryGroupID" id="o<%= CompanySiteParameter_list.lRowIndex %>_SiteCategoryGroupID" value="<%= ew_HTMLEncode(CompanySiteParameter.SiteCategoryGroupID.OldValue) %>" />
<% End If %>
<% If CompanySiteParameter.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<select id="x<%= CompanySiteParameter_list.lRowIndex %>_SiteCategoryGroupID" name="x<%= CompanySiteParameter_list.lRowIndex %>_SiteCategoryGroupID"<%= CompanySiteParameter.SiteCategoryGroupID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(CompanySiteParameter.SiteCategoryGroupID.EditValue) Then
	arwrk = CompanySiteParameter.SiteCategoryGroupID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), CompanySiteParameter.SiteCategoryGroupID.CurrentValue) Then
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
If emptywrk Then CompanySiteParameter.SiteCategoryGroupID.OldValue = ""
%>
</select>
<% End If %>
<% If CompanySiteParameter.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= CompanySiteParameter.SiteCategoryGroupID.ViewAttributes %>><%= CompanySiteParameter.SiteCategoryGroupID.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	<% If CompanySiteParameter.SiteParameterTypeID.Visible Then ' SiteParameterTypeID %>
		<td<%= CompanySiteParameter.SiteParameterTypeID.CellAttributes %>>
<% If CompanySiteParameter.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<% If CompanySiteParameter.SiteParameterTypeID.SessionValue <> "" Then %>
<div<%= CompanySiteParameter.SiteParameterTypeID.ViewAttributes %>><%= CompanySiteParameter.SiteParameterTypeID.ListViewValue %></div>
<input type="hidden" id="x<%= CompanySiteParameter_list.lRowIndex %>_SiteParameterTypeID" name="x<%= CompanySiteParameter_list.lRowIndex %>_SiteParameterTypeID" value="<%= ew_HtmlEncode(CompanySiteParameter.SiteParameterTypeID.CurrentValue) %>">
<% Else %>
<select id="x<%= CompanySiteParameter_list.lRowIndex %>_SiteParameterTypeID" name="x<%= CompanySiteParameter_list.lRowIndex %>_SiteParameterTypeID"<%= CompanySiteParameter.SiteParameterTypeID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(CompanySiteParameter.SiteParameterTypeID.EditValue) Then
	arwrk = CompanySiteParameter.SiteParameterTypeID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), CompanySiteParameter.SiteParameterTypeID.CurrentValue) Then
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
If emptywrk Then CompanySiteParameter.SiteParameterTypeID.OldValue = ""
%>
</select>
<% End If %>
<input type="hidden" name="o<%= CompanySiteParameter_list.lRowIndex %>_SiteParameterTypeID" id="o<%= CompanySiteParameter_list.lRowIndex %>_SiteParameterTypeID" value="<%= ew_HTMLEncode(CompanySiteParameter.SiteParameterTypeID.OldValue) %>" />
<% End If %>
<% If CompanySiteParameter.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<% If CompanySiteParameter.SiteParameterTypeID.SessionValue <> "" Then %>
<div<%= CompanySiteParameter.SiteParameterTypeID.ViewAttributes %>><%= CompanySiteParameter.SiteParameterTypeID.ListViewValue %></div>
<input type="hidden" id="x<%= CompanySiteParameter_list.lRowIndex %>_SiteParameterTypeID" name="x<%= CompanySiteParameter_list.lRowIndex %>_SiteParameterTypeID" value="<%= ew_HtmlEncode(CompanySiteParameter.SiteParameterTypeID.CurrentValue) %>">
<% Else %>
<select id="x<%= CompanySiteParameter_list.lRowIndex %>_SiteParameterTypeID" name="x<%= CompanySiteParameter_list.lRowIndex %>_SiteParameterTypeID"<%= CompanySiteParameter.SiteParameterTypeID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(CompanySiteParameter.SiteParameterTypeID.EditValue) Then
	arwrk = CompanySiteParameter.SiteParameterTypeID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), CompanySiteParameter.SiteParameterTypeID.CurrentValue) Then
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
If emptywrk Then CompanySiteParameter.SiteParameterTypeID.OldValue = ""
%>
</select>
<% End If %>
<% End If %>
<% If CompanySiteParameter.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= CompanySiteParameter.SiteParameterTypeID.ViewAttributes %>><%= CompanySiteParameter.SiteParameterTypeID.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	<% If CompanySiteParameter.SortOrder.Visible Then ' SortOrder %>
		<td<%= CompanySiteParameter.SortOrder.CellAttributes %>>
<% If CompanySiteParameter.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<input type="text" name="x<%= CompanySiteParameter_list.lRowIndex %>_SortOrder" id="x<%= CompanySiteParameter_list.lRowIndex %>_SortOrder" size="30" value="<%= CompanySiteParameter.SortOrder.EditValue %>"<%= CompanySiteParameter.SortOrder.EditAttributes %> />
<input type="hidden" name="o<%= CompanySiteParameter_list.lRowIndex %>_SortOrder" id="o<%= CompanySiteParameter_list.lRowIndex %>_SortOrder" value="<%= ew_HTMLEncode(CompanySiteParameter.SortOrder.OldValue) %>" />
<% End If %>
<% If CompanySiteParameter.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<input type="text" name="x<%= CompanySiteParameter_list.lRowIndex %>_SortOrder" id="x<%= CompanySiteParameter_list.lRowIndex %>_SortOrder" size="30" value="<%= CompanySiteParameter.SortOrder.EditValue %>"<%= CompanySiteParameter.SortOrder.EditAttributes %> />
<% End If %>
<% If CompanySiteParameter.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= CompanySiteParameter.SortOrder.ViewAttributes %>><%= CompanySiteParameter.SortOrder.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	</tr>
<% If CompanySiteParameter.RowType = EW_ROWTYPE_ADD Then %>
<script language="JavaScript">
<!--
ew_UpdateOpts([['x<%= CompanySiteParameter_list.lRowIndex %>_zPageID','x<%= CompanySiteParameter_list.lRowIndex %>_CompanyID',CompanySiteParameter_list.ar_x<%= CompanySiteParameter_list.lRowIndex %>_zPageID]]);
//-->
</script>
<% End If %>
<% If CompanySiteParameter.RowType = EW_ROWTYPE_EDIT Then %>
<script language="JavaScript">
<!--
ew_UpdateOpts([['x<%= CompanySiteParameter_list.lRowIndex %>_zPageID','x<%= CompanySiteParameter_list.lRowIndex %>_CompanyID',CompanySiteParameter_list.ar_x<%= CompanySiteParameter_list.lRowIndex %>_zPageID]]);
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
<% If CompanySiteParameter.CurrentAction = "gridadd" Then %>
<input type="hidden" name="a_list" id="a_list" value="gridinsert" />
<input type="hidden" name="key_count" id="key_count" value="<%= CompanySiteParameter_list.lRowIndex %>" />
<% End If %>
<% If CompanySiteParameter.CurrentAction = "edit" Then %>
<input type="hidden" name="key_count" id="key_count" value="<%= CompanySiteParameter_list.lRowIndex %>" />
<% End If %>
<% If CompanySiteParameter.CurrentAction = "gridedit" Then %>
<input type="hidden" name="a_list" id="a_list" value="gridupdate" />
<input type="hidden" name="key_count" id="key_count" value="<%= CompanySiteParameter_list.lRowIndex %>" />
<%= CompanySiteParameter_list.sMultiSelectKey %>
<% End If %>
</form>
<%

' Close recordset
Rs.Close()
Rs.Dispose()
%>
</div>
<% If CompanySiteParameter_list.lTotalRecs > 0 Then %>
<% If CompanySiteParameter.Export = "" Then %>
<div class="ewGridLowerPanel">
<% If CompanySiteParameter.CurrentAction <> "gridadd" AndAlso CompanySiteParameter.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If CompanySiteParameter_list.Pager Is Nothing Then CompanySiteParameter_list.Pager = New cPrevNextPager(CompanySiteParameter_list.lStartRec, CompanySiteParameter_list.lDisplayRecs, CompanySiteParameter_list.lTotalRecs) %>
<% If CompanySiteParameter_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If CompanySiteParameter_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= CompanySiteParameter_list.PageUrl %>start=<%= CompanySiteParameter_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If CompanySiteParameter_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= CompanySiteParameter_list.PageUrl %>start=<%= CompanySiteParameter_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= CompanySiteParameter_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If CompanySiteParameter_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= CompanySiteParameter_list.PageUrl %>start=<%= CompanySiteParameter_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If CompanySiteParameter_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= CompanySiteParameter_list.PageUrl %>start=<%= CompanySiteParameter_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= CompanySiteParameter_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= CompanySiteParameter_list.Pager.FromIndex %> to <%= CompanySiteParameter_list.Pager.ToIndex %> of <%= CompanySiteParameter_list.Pager.RecordCount %></span>
<% Else %>
	<% If CompanySiteParameter_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If CompanySiteParameter_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="CompanySiteParameter" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If CompanySiteParameter_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If CompanySiteParameter_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If CompanySiteParameter_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If CompanySiteParameter.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<% 'If CompanySiteParameter_list.lTotalRecs > 0 Then %>
<div class="aspnetmaker">
<% If CompanySiteParameter.CurrentAction <> "gridadd" AndAlso CompanySiteParameter.CurrentAction <> "gridedit" Then ' Not grid add/edit mode %>
<a href="<%= CompanySiteParameter.AddUrl %>">Add</a>&nbsp;&nbsp;
<a href="<%= CompanySiteParameter_list.PageUrl %>a=gridadd">Grid Add</a>&nbsp;&nbsp;
<% If CompanySiteParameter_list.lTotalRecs > 0 Then %>
<a href="<%= CompanySiteParameter_list.PageUrl %>a=gridedit">Grid Edit</a>&nbsp;&nbsp;
<% End If %>
<% Else ' Grid add/edit mode %>
<% If CompanySiteParameter.CurrentAction = "gridadd" Then %>
<a href="" onclick="f=ew_GetForm('fCompanySiteParameterlist');if (CompanySiteParameter_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/insert.gif' alt='Insert' title='Insert' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<% If CompanySiteParameter.CurrentAction = "gridedit" Then %>
<a href="" onclick="f=ew_GetForm('fCompanySiteParameterlist');if (CompanySiteParameter_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/update.gif' alt='Save' title='Save' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<a href="<%= CompanySiteParameter_list.PageUrl %>a=cancel"><img src='images/cancel.gif' alt='Cancel' title='Cancel' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
</div>
<% 'End If %>
</div>
<% End If %>
<% End If %>
</td></tr></table>
<% If CompanySiteParameter.Export = "" AndAlso CompanySiteParameter.CurrentAction = "" Then %>
<script type="text/javascript">
<!--
//ew_ToggleSearchPanel(CompanySiteParameter_list); // uncomment to init search panel as collapsed
//-->
</script>
<% End If %>
<% If CompanySiteParameter.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
