<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="PageRole_list.aspx.vb" Inherits="PageRole_list" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If PageRole.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var PageRole_list = new ew_Page("PageRole_list");
// page properties
PageRole_list.PageID = "list"; // page ID
var EW_PAGE_ID = PageRole_list.PageID; // for backward compatibility
// extend page with ValidateForm function
PageRole_list.ValidateForm = function(fobj) {
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
		elm = fobj.elements["x" + infix + "_RoleID"];
		if (elm && !ew_CheckInteger(elm.value))
			return ew_OnError(this, elm, "Incorrect integer - Role ID");
		elm = fobj.elements["x" + infix + "_zPageID"];
		if (elm && !ew_CheckInteger(elm.value))
			return ew_OnError(this, elm, "Incorrect integer - Page ID");
		elm = fobj.elements["x" + infix + "_CompanyID"];
		if (elm && !ew_CheckInteger(elm.value))
			return ew_OnError(this, elm, "Incorrect integer - Company ID");
		} // End Grid Add checking
	}
	if (fobj.a_list && fobj.a_list.value == "gridinsert" && addcnt == 0) { // No row added
		alert("No records to be added");
		return false;
	}
	return true;
}
// Extend page with empty row check
PageRole_list.EmptyRow = function(fobj, infix) {
	if (ew_ValueChanged(fobj, infix, "RoleID")) return false;
	if (ew_ValueChanged(fobj, infix, "zPageID")) return false;
	if (ew_ValueChanged(fobj, infix, "CompanyID")) return false;
	return true;
}
<% If EW_CLIENT_VALIDATE Then %>
PageRole_list.ValidateRequired = true; // uses JavaScript validation
<% Else %>
PageRole_list.ValidateRequired = false; // no JavaScript validation
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
<% If PageRole.Export = "" Then %>
<% End If %>
<%
If PageRole.CurrentAction = "gridadd" Then PageRole.CurrentFilter = "0=1"

' Load recordset
Rs = PageRole_list.LoadRecordset()
If PageRole.CurrentAction = "gridadd" Then
	PageRole_list.lStartRec = 1
	If PageRole_list.lDisplayRecs <= 0 Then PageRole_list.lDisplayRecs = 25
	PageRole_list.lTotalRecs = PageRole_list.lDisplayRecs
	PageRole_list.lStopRec = PageRole_list.lDisplayRecs
Else
	PageRole_list.lStartRec = 1
	If PageRole_list.lDisplayRecs <= 0 Then ' Display all records
		PageRole_list.lDisplayRecs = PageRole_list.lTotalRecs
	End If
	If Not (PageRole.ExportAll AndAlso PageRole.Export <> "") Then
		PageRole_list.SetUpStartRec() ' Set up start record position
	End If
End If
%>
<p><span class="aspnetmaker" style="white-space: nowrap;">TABLE: Location Role
<% If PageRole.Export = "" AndAlso PageRole.CurrentAction = "" Then %>
&nbsp;&nbsp;<a href="<%= PageRole_list.PageUrl %>export=excel"><img src='images/exportxls.gif' alt='Export to Excel' title='Export to Excel' width='16' height='16' border='0'></a>
<% End If %>
</span></p>
<% If PageRole.Export = "" AndAlso PageRole.CurrentAction = "" Then %>
<a href="javascript:ew_ToggleSearchPanel(PageRole_list);" style="text-decoration: none;"><img id="PageRole_list_SearchImage" src="images/collapse.gif" alt="" width="9" height="9" border="0"></a><span class="aspnetmaker">&nbsp;Search</span><br>
<div id="PageRole_list_SearchPanel">
<form name="fPageRolelistsrch" id="fPageRolelistsrch" class="ewForm">
<input type="hidden" id="t" name="t" value="PageRole" />
<table class="ewBasicSearch">
	<tr>
		<td><span class="aspnetmaker">
			<a href="<%= PageRole_list.PageUrl %>cmd=reset">Show all</a>&nbsp;
			<a href="PageRole_srch.aspx">Advanced Search</a>&nbsp;
		</span></td>
	</tr>
</table>
</form>
</div>
<% End If %>
<% PageRole_list.ShowMessage() %>
<br />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<% If PageRole.Export = "" Then %>
<div class="ewGridUpperPanel">
<% If PageRole.CurrentAction <> "gridadd" AndAlso PageRole.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If PageRole_list.Pager Is Nothing Then PageRole_list.Pager = New cPrevNextPager(PageRole_list.lStartRec, PageRole_list.lDisplayRecs, PageRole_list.lTotalRecs) %>
<% If PageRole_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If PageRole_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= PageRole_list.PageUrl %>start=<%= PageRole_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If PageRole_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= PageRole_list.PageUrl %>start=<%= PageRole_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= PageRole_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If PageRole_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= PageRole_list.PageUrl %>start=<%= PageRole_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If PageRole_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= PageRole_list.PageUrl %>start=<%= PageRole_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= PageRole_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= PageRole_list.Pager.FromIndex %> to <%= PageRole_list.Pager.ToIndex %> of <%= PageRole_list.Pager.RecordCount %></span>
<% Else %>
	<% If PageRole_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If PageRole_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="PageRole" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If PageRole_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If PageRole_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If PageRole_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If PageRole.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<div class="aspnetmaker">
<% If PageRole.CurrentAction <> "gridadd" AndAlso PageRole.CurrentAction <> "gridedit" Then ' Not grid add/edit mode %>
<a href="<%= PageRole.AddUrl %>">Add</a>&nbsp;&nbsp;
<a href="<%= PageRole_list.PageUrl %>a=gridadd">Grid Add</a>&nbsp;&nbsp;
<% Else ' Grid add/edit mode %>
<% If PageRole.CurrentAction = "gridadd" Then %>
<a href="" onclick="f=ew_GetForm('fPageRolelist');if (PageRole_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/insert.gif' alt='Insert' title='Insert' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<a href="<%= PageRole_list.PageUrl %>a=cancel"><img src='images/cancel.gif' alt='Cancel' title='Cancel' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
</div>
</div>
<% End If %>
<div class="ewGridMiddlePanel">
<form name="fPageRolelist" id="fPageRolelist" class="ewForm" method="post">
<input type="hidden" name="t" id="t" value="PageRole" />
<% If PageRole_list.lTotalRecs > 0 Then %>
<table cellspacing="0" rowhighlightclass="ewTableHighlightRow" rowselectclass="ewTableSelectRow" roweditclass="ewTableEditRow" class="ewTable ewTableSeparate">
<%
	PageRole_list.lOptionCnt = 0
	PageRole_list.lOptionCnt = PageRole_list.lOptionCnt + 1 ' View
	PageRole_list.lOptionCnt = PageRole_list.lOptionCnt + 1 ' Edit
	PageRole_list.lOptionCnt = PageRole_list.lOptionCnt + 1 ' Copy
	PageRole_list.lOptionCnt = PageRole_list.lOptionCnt + 1 ' Delete
	PageRole_list.lOptionCnt = PageRole_list.lOptionCnt + PageRole_list.ListOptions.Items.Count ' Custom list options
%>
<%= PageRole.TableCustomInnerHTML %>
<thead><!-- Table header -->
	<tr class="ewTableHeader">
<% If PageRole.Export = "" Then %>
<% If PageRole.CurrentAction <> "gridadd" AndAlso PageRole.CurrentAction <> "gridedit" Then %>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<%

' Custom list options
For i As Integer = 0 to PageRole_list.ListOptions.Items.Count -1
	If PageRole_list.ListOptions.Items(i).Visible Then Response.Write(PageRole_list.ListOptions.Items(i).HeaderCellHtml)
Next
%>
<% End If %>
<% End If %>
<% If PageRole.PageRoleID.Visible Then ' PageRoleID %>
	<% If PageRole.SortUrl(PageRole.PageRoleID) = "" Then %>
		<td>Page Role ID</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= PageRole.SortUrl(PageRole.PageRoleID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Page Role ID</td><td style="width: 10px;"><% If PageRole.PageRoleID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf PageRole.PageRoleID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If PageRole.RoleID.Visible Then ' RoleID %>
	<% If PageRole.SortUrl(PageRole.RoleID) = "" Then %>
		<td>Role ID</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= PageRole.SortUrl(PageRole.RoleID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Role ID</td><td style="width: 10px;"><% If PageRole.RoleID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf PageRole.RoleID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If PageRole.zPageID.Visible Then ' PageID %>
	<% If PageRole.SortUrl(PageRole.zPageID) = "" Then %>
		<td>Page ID</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= PageRole.SortUrl(PageRole.zPageID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Page ID</td><td style="width: 10px;"><% If PageRole.zPageID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf PageRole.zPageID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If PageRole.CompanyID.Visible Then ' CompanyID %>
	<% If PageRole.SortUrl(PageRole.CompanyID) = "" Then %>
		<td>Company ID</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= PageRole.SortUrl(PageRole.CompanyID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Company ID</td><td style="width: 10px;"><% If PageRole.CompanyID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf PageRole.CompanyID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
	</tr>
</thead>
<tbody><!-- Table body -->
<%
If (PageRole.ExportAll AndAlso PageRole.Export <> "") Then
	PageRole_list.lStopRec = PageRole_list.lTotalRecs
Else
	PageRole_list.lStopRec = PageRole_list.lStartRec + PageRole_list.lDisplayRecs - 1 ' Set the last record to display
End If
If PageRole.CurrentAction = "gridadd" AndAlso PageRole_list.lStopRec = -1 Then
	PageRole_list.lStopRec = EW_GRIDADD_ROWS
End If 

' Move to first record
For i As Integer = 1 to PageRole_list.lStartRec - 1
	If Rs.Read() Then	PageRole_list.lRecCnt = PageRole_list.lRecCnt + 1
Next		
PageRole_list.lRowCnt = 0
PageRole_list.lEditRowCnt = 0
If PageRole.CurrentAction = "edit" Then PageRole_list.lRowIndex = 1
If PageRole.CurrentAction = "gridadd" Then PageRole_list.lRowIndex = 0

' Output data rows
Do While (PageRole.CurrentAction = "gridadd" OrElse Rs.Read()) AndAlso (PageRole_list.lRecCnt < PageRole_list.lStopRec)
	PageRole_list.lRecCnt = PageRole_list.lRecCnt + 1
	If PageRole_list.lRecCnt >= PageRole_list.lStartRec Then
		PageRole_list.lRowCnt = PageRole_list.lRowCnt + 1
		If PageRole.CurrentAction = "gridadd" OrElse PageRole.CurrentAction = "gridedit" Then PageRole_list.lRowIndex = PageRole_list.lRowIndex + 1
	PageRole.CssClass = ""
	PageRole.CssStyle = ""
	PageRole.RowClientEvents = "onmouseover='ew_MouseOver(event, this);' onmouseout='ew_MouseOut(event, this);' onclick='ew_Click(event, this);'"
	If PageRole.CurrentAction = "gridadd" Then
		PageRole_list.LoadDefaultValues() ' Load default values
	Else
		PageRole_list.LoadRowValues(Rs) ' Load row values
	End If
	PageRole.RowType = EW_ROWTYPE_VIEW ' Render view
	If PageRole.CurrentAction = "gridadd" Then ' Grid add
		PageRole.RowType = EW_ROWTYPE_ADD ' Render add
	End If
	If PageRole.CurrentAction = "gridadd" AndAlso PageRole.EventCancelled Then ' Insert failed
		PageRole_list.RestoreCurrentRowFormValues(PageRole_list.lRowIndex) ' Restore form values
	End If
	If PageRole.CurrentAction = "edit" Then
		If PageRole_list.CheckInlineEditKey() AndAlso PageRole_list.lEditRowCnt = 0 Then ' Inline edit
			PageRole.RowType = EW_ROWTYPE_EDIT ' Render edit
		End If
	End If
	If PageRole.RowType = EW_ROWTYPE_EDIT AndAlso PageRole.EventCancelled Then ' update failed
		If PageRole.CurrentAction = "edit" Then
			PageRole_list.RestoreFormValues() ' Restore form values
		End If
	End If
	If PageRole.RowType = EW_ROWTYPE_EDIT Then ' Edit row
		PageRole_list.lEditRowCnt = PageRole_list.lEditRowCnt + 1
		PageRole.RowClientEvents = "onmouseover='this.edit=true;ew_MouseOver(event, this);' onmouseout='ew_MouseOut(event, this);' onclick='ew_Click(event, this);'"
	End If
	If PageRole.RowType = EW_ROWTYPE_ADD OrElse PageRole.RowType = EW_ROWTYPE_EDIT Then ' Add / Edit row
		PageRole.CssClass = "ewTableEditRow"
	End If

	' Render row
	PageRole_list.RenderRow()
%>
	<tr<%= PageRole.RowAttributes %>>
<% If PageRole.RowType = EW_ROWTYPE_ADD OrElse PageRole.RowType = EW_ROWTYPE_EDIT Then %>
<% If PageRole.CurrentAction = "edit" Then %>
<td colspan="<%= PageRole_list.lOptionCnt %>" align="right"><span class="aspnetmaker">
<a href="" onclick="f=ew_GetForm('fPageRolelist');if (PageRole_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/update.gif' alt='Update' title='Update' width='16' height='16' border='0'></a>&nbsp;<a href="<%= PageRole_list.PageUrl %>a=cancel"><img src='images/cancel.gif' alt='Cancel' title='Cancel' width='16' height='16' border='0'></a>
<input type="hidden" name="a_list" id="a_list" value="update" />
</span></td>
<% End If %>
<% Else %>
<% If PageRole.Export = "" Then %>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= PageRole.ViewUrl %>"><img src='images/view.gif' alt='View' title='View' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= PageRole.EditUrl %>"><img src='images/edit.gif' alt='Edit' title='Edit' width='16' height='16' border='0'></a><span class="ewSeparator">&nbsp;|&nbsp;</span><a href="<%= PageRole.InlineEditUrl %>"><img src='images/inlineedit.gif' alt='Inline Edit' title='Inline Edit' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= PageRole.CopyUrl %>"><img src='images/copy.gif' alt='Copy' title='Copy' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= PageRole.DeleteUrl %>"><img src='images/delete.gif' alt='Delete' title='Delete' width='16' height='16' border='0'></a>
</span></td>
<%

' Custom list options
For i As Integer = 0 to PageRole_list.ListOptions.Items.Count -1
	If PageRole_list.ListOptions.Items(i).Visible Then Response.Write(PageRole_list.ListOptions.Items(i).BodyCellHtml)
Next
%>
<% End If %>
<% End If %>
	<% If PageRole.PageRoleID.Visible Then ' PageRoleID %>
		<td<%= PageRole.PageRoleID.CellAttributes %>>
<% If PageRole.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<input type="hidden" name="o<%= PageRole_list.lRowIndex %>_PageRoleID" id="o<%= PageRole_list.lRowIndex %>_PageRoleID" value="<%= ew_HTMLEncode(PageRole.PageRoleID.OldValue) %>" />
<% End If %>
<% If PageRole.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<div<%= PageRole.PageRoleID.ViewAttributes %>><%= PageRole.PageRoleID.EditValue %></div>
<input type="hidden" name="x<%= PageRole_list.lRowIndex %>_PageRoleID" id="x<%= PageRole_list.lRowIndex %>_PageRoleID" value="<%= ew_HTMLEncode(PageRole.PageRoleID.CurrentValue) %>" />
<% End If %>
<% If PageRole.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= PageRole.PageRoleID.ViewAttributes %>><%= PageRole.PageRoleID.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	<% If PageRole.RoleID.Visible Then ' RoleID %>
		<td<%= PageRole.RoleID.CellAttributes %>>
<% If PageRole.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<input type="text" name="x<%= PageRole_list.lRowIndex %>_RoleID" id="x<%= PageRole_list.lRowIndex %>_RoleID" size="30" value="<%= PageRole.RoleID.EditValue %>"<%= PageRole.RoleID.EditAttributes %> />
<input type="hidden" name="o<%= PageRole_list.lRowIndex %>_RoleID" id="o<%= PageRole_list.lRowIndex %>_RoleID" value="<%= ew_HTMLEncode(PageRole.RoleID.OldValue) %>" />
<% End If %>
<% If PageRole.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<input type="text" name="x<%= PageRole_list.lRowIndex %>_RoleID" id="x<%= PageRole_list.lRowIndex %>_RoleID" size="30" value="<%= PageRole.RoleID.EditValue %>"<%= PageRole.RoleID.EditAttributes %> />
<% End If %>
<% If PageRole.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= PageRole.RoleID.ViewAttributes %>><%= PageRole.RoleID.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	<% If PageRole.zPageID.Visible Then ' PageID %>
		<td<%= PageRole.zPageID.CellAttributes %>>
<% If PageRole.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<input type="text" name="x<%= PageRole_list.lRowIndex %>_zPageID" id="x<%= PageRole_list.lRowIndex %>_zPageID" size="30" value="<%= PageRole.zPageID.EditValue %>"<%= PageRole.zPageID.EditAttributes %> />
<input type="hidden" name="o<%= PageRole_list.lRowIndex %>_zPageID" id="o<%= PageRole_list.lRowIndex %>_zPageID" value="<%= ew_HTMLEncode(PageRole.zPageID.OldValue) %>" />
<% End If %>
<% If PageRole.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<input type="text" name="x<%= PageRole_list.lRowIndex %>_zPageID" id="x<%= PageRole_list.lRowIndex %>_zPageID" size="30" value="<%= PageRole.zPageID.EditValue %>"<%= PageRole.zPageID.EditAttributes %> />
<% End If %>
<% If PageRole.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= PageRole.zPageID.ViewAttributes %>><%= PageRole.zPageID.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	<% If PageRole.CompanyID.Visible Then ' CompanyID %>
		<td<%= PageRole.CompanyID.CellAttributes %>>
<% If PageRole.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<input type="text" name="x<%= PageRole_list.lRowIndex %>_CompanyID" id="x<%= PageRole_list.lRowIndex %>_CompanyID" size="30" value="<%= PageRole.CompanyID.EditValue %>"<%= PageRole.CompanyID.EditAttributes %> />
<input type="hidden" name="o<%= PageRole_list.lRowIndex %>_CompanyID" id="o<%= PageRole_list.lRowIndex %>_CompanyID" value="<%= ew_HTMLEncode(PageRole.CompanyID.OldValue) %>" />
<% End If %>
<% If PageRole.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<input type="text" name="x<%= PageRole_list.lRowIndex %>_CompanyID" id="x<%= PageRole_list.lRowIndex %>_CompanyID" size="30" value="<%= PageRole.CompanyID.EditValue %>"<%= PageRole.CompanyID.EditAttributes %> />
<% End If %>
<% If PageRole.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= PageRole.CompanyID.ViewAttributes %>><%= PageRole.CompanyID.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	</tr>
<% If PageRole.RowType = EW_ROWTYPE_ADD Then %>
<% End If %>
<% If PageRole.RowType = EW_ROWTYPE_EDIT Then %>
<% End If %>
<%
	End If
Loop
%>
</tbody>
</table>
<% End If %>
<% If PageRole.CurrentAction = "gridadd" Then %>
<input type="hidden" name="a_list" id="a_list" value="gridinsert" />
<input type="hidden" name="key_count" id="key_count" value="<%= PageRole_list.lRowIndex %>" />
<% End If %>
<% If PageRole.CurrentAction = "edit" Then %>
<input type="hidden" name="key_count" id="key_count" value="<%= PageRole_list.lRowIndex %>" />
<% End If %>
</form>
<%

' Close recordset
Rs.Close()
Rs.Dispose()
%>
</div>
<% If PageRole_list.lTotalRecs > 0 Then %>
<% If PageRole.Export = "" Then %>
<div class="ewGridLowerPanel">
<% If PageRole.CurrentAction <> "gridadd" AndAlso PageRole.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If PageRole_list.Pager Is Nothing Then PageRole_list.Pager = New cPrevNextPager(PageRole_list.lStartRec, PageRole_list.lDisplayRecs, PageRole_list.lTotalRecs) %>
<% If PageRole_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If PageRole_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= PageRole_list.PageUrl %>start=<%= PageRole_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If PageRole_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= PageRole_list.PageUrl %>start=<%= PageRole_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= PageRole_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If PageRole_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= PageRole_list.PageUrl %>start=<%= PageRole_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If PageRole_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= PageRole_list.PageUrl %>start=<%= PageRole_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= PageRole_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= PageRole_list.Pager.FromIndex %> to <%= PageRole_list.Pager.ToIndex %> of <%= PageRole_list.Pager.RecordCount %></span>
<% Else %>
	<% If PageRole_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If PageRole_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="PageRole" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If PageRole_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If PageRole_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If PageRole_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If PageRole.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<% 'If PageRole_list.lTotalRecs > 0 Then %>
<div class="aspnetmaker">
<% If PageRole.CurrentAction <> "gridadd" AndAlso PageRole.CurrentAction <> "gridedit" Then ' Not grid add/edit mode %>
<a href="<%= PageRole.AddUrl %>">Add</a>&nbsp;&nbsp;
<a href="<%= PageRole_list.PageUrl %>a=gridadd">Grid Add</a>&nbsp;&nbsp;
<% Else ' Grid add/edit mode %>
<% If PageRole.CurrentAction = "gridadd" Then %>
<a href="" onclick="f=ew_GetForm('fPageRolelist');if (PageRole_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/insert.gif' alt='Insert' title='Insert' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<a href="<%= PageRole_list.PageUrl %>a=cancel"><img src='images/cancel.gif' alt='Cancel' title='Cancel' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
</div>
<% 'End If %>
</div>
<% End If %>
<% End If %>
</td></tr></table>
<% If PageRole.Export = "" AndAlso PageRole.CurrentAction = "" Then %>
<script type="text/javascript">
<!--
//ew_ToggleSearchPanel(PageRole_list); // uncomment to init search panel as collapsed
//-->
</script>
<% End If %>
<% If PageRole.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
