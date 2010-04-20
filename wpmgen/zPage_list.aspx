<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="zPage_list.aspx.vb" Inherits="zPage_list" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If zPage.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var zPage_list = new ew_Page("zPage_list");
// page properties
zPage_list.PageID = "list"; // page ID
var EW_PAGE_ID = zPage_list.PageID; // for backward compatibility
// extend page with ValidateForm function
zPage_list.ValidateForm = function(fobj) {
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
		elm = fobj.elements["x" + infix + "_PageOrder"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field - Order");
		elm = fobj.elements["x" + infix + "_PageOrder"];
		if (elm && !ew_CheckInteger(elm.value))
			return ew_OnError(this, elm, "Incorrect integer - Order");
		elm = fobj.elements["x" + infix + "_PageTypeID"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field - PageType");
		elm = fobj.elements["x" + infix + "_zPageName"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field - Page Name");
		} // End Grid Add checking
	}
	if (fobj.a_list && fobj.a_list.value == "gridinsert" && addcnt == 0) { // No row added
		alert("No records to be added");
		return false;
	}
	return true;
}
// Extend page with empty row check
zPage_list.EmptyRow = function(fobj, infix) {
	if (ew_ValueChanged(fobj, infix, "PageOrder")) return false;
	if (ew_ValueChanged(fobj, infix, "ParentPageID")) return false;
	if (ew_ValueChanged(fobj, infix, "PageTypeID")) return false;
	if (ew_ValueChanged(fobj, infix, "Active")) return false;
	if (ew_ValueChanged(fobj, infix, "zPageName")) return false;
	if (ew_ValueChanged(fobj, infix, "SiteCategoryID")) return false;
	if (ew_ValueChanged(fobj, infix, "SiteCategoryGroupID")) return false;
	if (ew_ValueChanged(fobj, infix, "ModifiedDT")) return false;
	return true;
}
<% If EW_CLIENT_VALIDATE Then %>
zPage_list.ValidateRequired = true; // uses JavaScript validation
<% Else %>
zPage_list.ValidateRequired = false; // no JavaScript validation
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
<% If zPage.Export = "" Then %>
<% End If %>
<%
If zPage.CurrentAction = "gridadd" Then zPage.CurrentFilter = "0=1"

' Load recordset
Rs = zPage_list.LoadRecordset()
If zPage.CurrentAction = "gridadd" Then
	zPage_list.lStartRec = 1
	If zPage_list.lDisplayRecs <= 0 Then zPage_list.lDisplayRecs = 25
	zPage_list.lTotalRecs = zPage_list.lDisplayRecs
	zPage_list.lStopRec = zPage_list.lDisplayRecs
Else
	zPage_list.lStartRec = 1
	If zPage_list.lDisplayRecs <= 0 Then ' Display all records
		zPage_list.lDisplayRecs = zPage_list.lTotalRecs
	End If
	If Not (zPage.ExportAll AndAlso zPage.Export <> "") Then
		zPage_list.SetUpStartRec() ' Set up start record position
	End If
End If
%>
<p><span class="aspnetmaker" style="white-space: nowrap;">TABLE: Site Location
<% If zPage.Export = "" AndAlso zPage.CurrentAction = "" Then %>
&nbsp;&nbsp;<a href="<%= zPage_list.PageUrl %>export=excel"><img src='images/exportxls.gif' alt='Export to Excel' title='Export to Excel' width='16' height='16' border='0'></a>
<% End If %>
</span></p>
<% If zPage.Export = "" AndAlso zPage.CurrentAction = "" Then %>
<a href="javascript:ew_ToggleSearchPanel(zPage_list);" style="text-decoration: none;"><img id="zPage_list_SearchImage" src="images/collapse.gif" alt="" width="9" height="9" border="0"></a><span class="aspnetmaker">&nbsp;Search</span><br>
<div id="zPage_list_SearchPanel">
<form name="fzPagelistsrch" id="fzPagelistsrch" class="ewForm">
<input type="hidden" id="t" name="t" value="zPage" />
<table class="ewBasicSearch">
	<tr>
		<td><span class="aspnetmaker">
			<input type="text" name="<%= EW_TABLE_BASIC_SEARCH %>" id="<%= EW_TABLE_BASIC_SEARCH %>" size="20" value="<%= ew_HtmlEncode(zPage.BasicSearchKeyword) %>" />
			<input type="Submit" name="Submit" id="Submit" value="Search (*)" />&nbsp;
			<a href="<%= zPage_list.PageUrl %>cmd=reset">Show all</a>&nbsp;
			<a href="zPage_srch.aspx">Advanced Search</a>&nbsp;
		</span></td>
	</tr>
	<tr>
	<td><span class="aspnetmaker"><label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value=""<% If zPage.BasicSearchType = "" Then %> checked="checked"<% End If %> />Exact phrase</label>&nbsp;&nbsp;<label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value="AND"<% If zPage.BasicSearchType = "AND" Then %> checked="checked"<% End If %> />All words</label>&nbsp;&nbsp;<label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value="OR"<% If zPage.BasicSearchType = "OR" Then %> checked="checked"<% End If %> />Any word</label></span></td>
	</tr>
</table>
</form>
</div>
<% End If %>
<% zPage_list.ShowMessage() %>
<br />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<% If zPage.Export = "" Then %>
<div class="ewGridUpperPanel">
<% If zPage.CurrentAction <> "gridadd" AndAlso zPage.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If zPage_list.Pager Is Nothing Then zPage_list.Pager = New cPrevNextPager(zPage_list.lStartRec, zPage_list.lDisplayRecs, zPage_list.lTotalRecs) %>
<% If zPage_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If zPage_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= zPage_list.PageUrl %>start=<%= zPage_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If zPage_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= zPage_list.PageUrl %>start=<%= zPage_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= zPage_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If zPage_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= zPage_list.PageUrl %>start=<%= zPage_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If zPage_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= zPage_list.PageUrl %>start=<%= zPage_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= zPage_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= zPage_list.Pager.FromIndex %> to <%= zPage_list.Pager.ToIndex %> of <%= zPage_list.Pager.RecordCount %></span>
<% Else %>
	<% If zPage_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If zPage_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="zPage" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If zPage_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If zPage_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If zPage_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If zPage.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<div class="aspnetmaker">
<% If zPage.CurrentAction <> "gridadd" AndAlso zPage.CurrentAction <> "gridedit" Then ' Not grid add/edit mode %>
<a href="<%= zPage.AddUrl %>">Add</a>&nbsp;&nbsp;
<a href="<%= zPage_list.PageUrl %>a=gridadd">Grid Add</a>&nbsp;&nbsp;
<% If zPage_list.lTotalRecs > 0 Then %>
<a href="<%= zPage_list.PageUrl %>a=gridedit">Grid Edit</a>&nbsp;&nbsp;
<% End If %>
<% Else ' Grid add/edit mode %>
<% If zPage.CurrentAction = "gridadd" Then %>
<a href="" onclick="f=ew_GetForm('fzPagelist');if (zPage_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/insert.gif' alt='Insert' title='Insert' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<% If zPage.CurrentAction = "gridedit" Then %>
<a href="" onclick="f=ew_GetForm('fzPagelist');if (zPage_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/update.gif' alt='Save' title='Save' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<a href="<%= zPage_list.PageUrl %>a=cancel"><img src='images/cancel.gif' alt='Cancel' title='Cancel' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
</div>
</div>
<% End If %>
<div class="ewGridMiddlePanel">
<form name="fzPagelist" id="fzPagelist" class="ewForm" method="post">
<input type="hidden" name="t" id="t" value="zPage" />
<% If zPage_list.lTotalRecs > 0 Then %>
<table cellspacing="0" rowhighlightclass="ewTableHighlightRow" rowselectclass="ewTableSelectRow" roweditclass="ewTableEditRow" class="ewTable ewTableSeparate">
<%
	zPage_list.lOptionCnt = 0
	zPage_list.lOptionCnt = zPage_list.lOptionCnt + 1 ' View
	zPage_list.lOptionCnt = zPage_list.lOptionCnt + 1 ' Edit
	zPage_list.lOptionCnt = zPage_list.lOptionCnt + 1 ' Copy
	zPage_list.lOptionCnt = zPage_list.lOptionCnt + 1 ' Delete
	zPage_list.lOptionCnt = zPage_list.lOptionCnt + zPage_list.ListOptions.Items.Count ' Custom list options
%>
<%= zPage.TableCustomInnerHTML %>
<thead><!-- Table header -->
	<tr class="ewTableHeader">
<% If zPage.Export = "" Then %>
<% If zPage.CurrentAction <> "gridadd" AndAlso zPage.CurrentAction <> "gridedit" Then %>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<%

' Custom list options
For i As Integer = 0 to zPage_list.ListOptions.Items.Count -1
	If zPage_list.ListOptions.Items(i).Visible Then Response.Write(zPage_list.ListOptions.Items(i).HeaderCellHtml)
Next
%>
<% End If %>
<% End If %>
<% If zPage.PageOrder.Visible Then ' PageOrder %>
	<% If zPage.SortUrl(zPage.PageOrder) = "" Then %>
		<td>Order</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= zPage.SortUrl(zPage.PageOrder) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Order</td><td style="width: 10px;"><% If zPage.PageOrder.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf zPage.PageOrder.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If zPage.ParentPageID.Visible Then ' ParentPageID %>
	<% If zPage.SortUrl(zPage.ParentPageID) = "" Then %>
		<td>Parent Page</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= zPage.SortUrl(zPage.ParentPageID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Parent Page</td><td style="width: 10px;"><% If zPage.ParentPageID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf zPage.ParentPageID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If zPage.PageTypeID.Visible Then ' PageTypeID %>
	<% If zPage.SortUrl(zPage.PageTypeID) = "" Then %>
		<td>PageType</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= zPage.SortUrl(zPage.PageTypeID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>PageType</td><td style="width: 10px;"><% If zPage.PageTypeID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf zPage.PageTypeID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If zPage.Active.Visible Then ' Active %>
	<% If zPage.SortUrl(zPage.Active) = "" Then %>
		<td>Active</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= zPage.SortUrl(zPage.Active) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Active</td><td style="width: 10px;"><% If zPage.Active.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf zPage.Active.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If zPage.zPageName.Visible Then ' PageName %>
	<% If zPage.SortUrl(zPage.zPageName) = "" Then %>
		<td>Page Name</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= zPage.SortUrl(zPage.zPageName) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Page Name&nbsp;(*)</td><td style="width: 10px;"><% If zPage.zPageName.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf zPage.zPageName.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If zPage.SiteCategoryID.Visible Then ' SiteCategoryID %>
	<% If zPage.SortUrl(zPage.SiteCategoryID) = "" Then %>
		<td>Site Category</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= zPage.SortUrl(zPage.SiteCategoryID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Site Category</td><td style="width: 10px;"><% If zPage.SiteCategoryID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf zPage.SiteCategoryID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If zPage.SiteCategoryGroupID.Visible Then ' SiteCategoryGroupID %>
	<% If zPage.SortUrl(zPage.SiteCategoryGroupID) = "" Then %>
		<td>Site Category Group</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= zPage.SortUrl(zPage.SiteCategoryGroupID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Site Category Group</td><td style="width: 10px;"><% If zPage.SiteCategoryGroupID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf zPage.SiteCategoryGroupID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If zPage.ModifiedDT.Visible Then ' ModifiedDT %>
	<% If zPage.SortUrl(zPage.ModifiedDT) = "" Then %>
		<td>Modified DT</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= zPage.SortUrl(zPage.ModifiedDT) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Modified DT</td><td style="width: 10px;"><% If zPage.ModifiedDT.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf zPage.ModifiedDT.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
	</tr>
</thead>
<tbody><!-- Table body -->
<%
If (zPage.ExportAll AndAlso zPage.Export <> "") Then
	zPage_list.lStopRec = zPage_list.lTotalRecs
Else
	zPage_list.lStopRec = zPage_list.lStartRec + zPage_list.lDisplayRecs - 1 ' Set the last record to display
End If
If zPage.CurrentAction = "gridadd" AndAlso zPage_list.lStopRec = -1 Then
	zPage_list.lStopRec = EW_GRIDADD_ROWS
End If 

' Move to first record
For i As Integer = 1 to zPage_list.lStartRec - 1
	If Rs.Read() Then	zPage_list.lRecCnt = zPage_list.lRecCnt + 1
Next		
zPage_list.lRowCnt = 0
zPage_list.lEditRowCnt = 0
If zPage.CurrentAction = "edit" Then zPage_list.lRowIndex = 1
If zPage.CurrentAction = "gridadd" Then zPage_list.lRowIndex = 0
If zPage.CurrentAction = "gridedit" Then zPage_list.lRowIndex = 0

' Output data rows
Do While (zPage.CurrentAction = "gridadd" OrElse Rs.Read()) AndAlso (zPage_list.lRecCnt < zPage_list.lStopRec)
	zPage_list.lRecCnt = zPage_list.lRecCnt + 1
	If zPage_list.lRecCnt >= zPage_list.lStartRec Then
		zPage_list.lRowCnt = zPage_list.lRowCnt + 1
		If zPage.CurrentAction = "gridadd" OrElse zPage.CurrentAction = "gridedit" Then zPage_list.lRowIndex = zPage_list.lRowIndex + 1
	zPage.CssClass = ""
	zPage.CssStyle = ""
	zPage.RowClientEvents = "onmouseover='ew_MouseOver(event, this);' onmouseout='ew_MouseOut(event, this);' onclick='ew_Click(event, this);'"
	If zPage.CurrentAction = "gridadd" Then
		zPage_list.LoadDefaultValues() ' Load default values
	Else
		zPage_list.LoadRowValues(Rs) ' Load row values
	End If
	zPage.RowType = EW_ROWTYPE_VIEW ' Render view
	If zPage.CurrentAction = "gridadd" Then ' Grid add
		zPage.RowType = EW_ROWTYPE_ADD ' Render add
	End If
	If zPage.CurrentAction = "gridadd" AndAlso zPage.EventCancelled Then ' Insert failed
		zPage_list.RestoreCurrentRowFormValues(zPage_list.lRowIndex) ' Restore form values
	End If
	If zPage.CurrentAction = "edit" Then
		If zPage_list.CheckInlineEditKey() AndAlso zPage_list.lEditRowCnt = 0 Then ' Inline edit
			zPage.RowType = EW_ROWTYPE_EDIT ' Render edit
		End If
	End If
	If zPage.CurrentAction = "gridedit" Then ' Grid edit
		zPage.RowType = EW_ROWTYPE_EDIT ' Render edit
	End If
	If zPage.RowType = EW_ROWTYPE_EDIT AndAlso zPage.EventCancelled Then ' update failed
		If zPage.CurrentAction = "edit" Then
			zPage_list.RestoreFormValues() ' Restore form values
		End If
		If zPage.CurrentAction = "gridedit" Then
			zPage_list.RestoreCurrentRowFormValues(zPage_list.lRowIndex) ' Restore form values
		End If
	End If
	If zPage.RowType = EW_ROWTYPE_EDIT Then ' Edit row
		zPage_list.lEditRowCnt = zPage_list.lEditRowCnt + 1
		zPage.RowClientEvents = "onmouseover='this.edit=true;ew_MouseOver(event, this);' onmouseout='ew_MouseOut(event, this);' onclick='ew_Click(event, this);'"
	End If
	If zPage.RowType = EW_ROWTYPE_ADD OrElse zPage.RowType = EW_ROWTYPE_EDIT Then ' Add / Edit row
		zPage.CssClass = "ewTableEditRow"
	End If

	' Render row
	zPage_list.RenderRow()
%>
	<tr<%= zPage.RowAttributes %>>
<% If zPage.RowType = EW_ROWTYPE_ADD OrElse zPage.RowType = EW_ROWTYPE_EDIT Then %>
<% If zPage.CurrentAction = "edit" Then %>
<td colspan="<%= zPage_list.lOptionCnt %>" align="right"><span class="aspnetmaker">
<a href="" onclick="f=ew_GetForm('fzPagelist');if (zPage_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/update.gif' alt='Update' title='Update' width='16' height='16' border='0'></a>&nbsp;<a href="<%= zPage_list.PageUrl %>a=cancel"><img src='images/cancel.gif' alt='Cancel' title='Cancel' width='16' height='16' border='0'></a>
<input type="hidden" name="a_list" id="a_list" value="update" />
</span></td>
<% End If %>
<%
	If zPage.CurrentAction = "gridedit" Then
		zPage_list.sMultiSelectKey = zPage_list.sMultiSelectKey & "<input type=""hidden"" name=""k" & zPage_list.lRowIndex & "_key"" id=""k" & zPage_list.lRowIndex & "_key"" value=""" & zPage.zPageID.CurrentValue & """ />"
	End If
%>
<% Else %>
<% If zPage.Export = "" Then %>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= zPage.ViewUrl %>"><img src='images/view.gif' alt='View' title='View' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= zPage.EditUrl %>"><img src='images/edit.gif' alt='Edit' title='Edit' width='16' height='16' border='0'></a><span class="ewSeparator">&nbsp;|&nbsp;</span><a href="<%= zPage.InlineEditUrl %>"><img src='images/inlineedit.gif' alt='Inline Edit' title='Inline Edit' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= zPage.CopyUrl %>"><img src='images/copy.gif' alt='Copy' title='Copy' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= zPage.DeleteUrl %>"><img src='images/delete.gif' alt='Delete' title='Delete' width='16' height='16' border='0'></a>
</span></td>
<%

' Custom list options
For i As Integer = 0 to zPage_list.ListOptions.Items.Count -1
	If zPage_list.ListOptions.Items(i).Visible Then Response.Write(zPage_list.ListOptions.Items(i).BodyCellHtml)
Next
%>
<% End If %>
<% End If %>
	<% If zPage.PageOrder.Visible Then ' PageOrder %>
		<td<%= zPage.PageOrder.CellAttributes %>>
<% If zPage.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<input type="text" name="x<%= zPage_list.lRowIndex %>_PageOrder" id="x<%= zPage_list.lRowIndex %>_PageOrder" size="10" value="<%= zPage.PageOrder.EditValue %>"<%= zPage.PageOrder.EditAttributes %> />
<input type="hidden" name="o<%= zPage_list.lRowIndex %>_PageOrder" id="o<%= zPage_list.lRowIndex %>_PageOrder" value="<%= ew_HTMLEncode(zPage.PageOrder.OldValue) %>" />
<% End If %>
<% If zPage.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<input type="text" name="x<%= zPage_list.lRowIndex %>_PageOrder" id="x<%= zPage_list.lRowIndex %>_PageOrder" size="10" value="<%= zPage.PageOrder.EditValue %>"<%= zPage.PageOrder.EditAttributes %> />
<% End If %>
<% If zPage.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= zPage.PageOrder.ViewAttributes %>><%= zPage.PageOrder.ListViewValue %></div>
<% End If %>
<% If zPage.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<input type="hidden" name="o<%= zPage_list.lRowIndex %>_zPageID" id="o<%= zPage_list.lRowIndex %>_zPageID" value="<%= ew_HTMLEncode(zPage.zPageID.OldValue) %>" />
<% End If %>
<% If zPage.RowType = EW_ROWTYPE_EDIT Then %>
<input type="hidden" name="x<%= zPage_list.lRowIndex %>_zPageID" id="x<%= zPage_list.lRowIndex %>_zPageID" value="<%= ew_HTMLEncode(zPage.zPageID.CurrentValue) %>" />
<% End If %>
</td>
	<% End If %>
	<% If zPage.ParentPageID.Visible Then ' ParentPageID %>
		<td<%= zPage.ParentPageID.CellAttributes %>>
<% If zPage.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<select id="x<%= zPage_list.lRowIndex %>_ParentPageID" name="x<%= zPage_list.lRowIndex %>_ParentPageID"<%= zPage.ParentPageID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(zPage.ParentPageID.EditValue) Then
	arwrk = zPage.ParentPageID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), zPage.ParentPageID.CurrentValue) Then
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
If emptywrk Then zPage.ParentPageID.OldValue = ""
%>
</select>
<input type="hidden" name="o<%= zPage_list.lRowIndex %>_ParentPageID" id="o<%= zPage_list.lRowIndex %>_ParentPageID" value="<%= ew_HTMLEncode(zPage.ParentPageID.OldValue) %>" />
<% End If %>
<% If zPage.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<select id="x<%= zPage_list.lRowIndex %>_ParentPageID" name="x<%= zPage_list.lRowIndex %>_ParentPageID"<%= zPage.ParentPageID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(zPage.ParentPageID.EditValue) Then
	arwrk = zPage.ParentPageID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), zPage.ParentPageID.CurrentValue) Then
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
If emptywrk Then zPage.ParentPageID.OldValue = ""
%>
</select>
<% End If %>
<% If zPage.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= zPage.ParentPageID.ViewAttributes %>><%= zPage.ParentPageID.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	<% If zPage.PageTypeID.Visible Then ' PageTypeID %>
		<td<%= zPage.PageTypeID.CellAttributes %>>
<% If zPage.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<select id="x<%= zPage_list.lRowIndex %>_PageTypeID" name="x<%= zPage_list.lRowIndex %>_PageTypeID"<%= zPage.PageTypeID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(zPage.PageTypeID.EditValue) Then
	arwrk = zPage.PageTypeID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), zPage.PageTypeID.CurrentValue) Then
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
If emptywrk Then zPage.PageTypeID.OldValue = ""
%>
</select>
<input type="hidden" name="o<%= zPage_list.lRowIndex %>_PageTypeID" id="o<%= zPage_list.lRowIndex %>_PageTypeID" value="<%= ew_HTMLEncode(zPage.PageTypeID.OldValue) %>" />
<% End If %>
<% If zPage.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<select id="x<%= zPage_list.lRowIndex %>_PageTypeID" name="x<%= zPage_list.lRowIndex %>_PageTypeID"<%= zPage.PageTypeID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(zPage.PageTypeID.EditValue) Then
	arwrk = zPage.PageTypeID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), zPage.PageTypeID.CurrentValue) Then
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
If emptywrk Then zPage.PageTypeID.OldValue = ""
%>
</select>
<% End If %>
<% If zPage.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= zPage.PageTypeID.ViewAttributes %>><%= zPage.PageTypeID.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	<% If zPage.Active.Visible Then ' Active %>
		<td<%= zPage.Active.CellAttributes %>>
<% If zPage.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<%
If ew_SameStr(zPage.Active.CurrentValue, "1") Then
	selwrk = " checked=""checked"""
Else
	selwrk = ""
End If
%>
<input type="checkbox" name="x<%= zPage_list.lRowIndex %>_Active" id="x<%= zPage_list.lRowIndex %>_Active" value="1"<%= selwrk %><%= zPage.Active.EditAttributes %> />
<input type="hidden" name="o<%= zPage_list.lRowIndex %>_Active" id="o<%= zPage_list.lRowIndex %>_Active" value="<%= ew_HTMLEncode(zPage.Active.OldValue) %>" />
<% End If %>
<% If zPage.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<%
If ew_SameStr(zPage.Active.CurrentValue, "1") Then
	selwrk = " checked=""checked"""
Else
	selwrk = ""
End If
%>
<input type="checkbox" name="x<%= zPage_list.lRowIndex %>_Active" id="x<%= zPage_list.lRowIndex %>_Active" value="1"<%= selwrk %><%= zPage.Active.EditAttributes %> />
<% End If %>
<% If zPage.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<% If Convert.ToString(zPage.Active.CurrentValue) = "1" Then %>
<input type="checkbox" value="<%= zPage.Active.ListViewValue %>" checked onclick="this.form.reset();" disabled="disabled" />
<% Else %>
<input type="checkbox" value="<%= zPage.Active.ListViewValue %>" onclick="this.form.reset();" disabled="disabled" />
<% End If %>
<% End If %>
</td>
	<% End If %>
	<% If zPage.zPageName.Visible Then ' PageName %>
		<td<%= zPage.zPageName.CellAttributes %>>
<% If zPage.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<input type="text" name="x<%= zPage_list.lRowIndex %>_zPageName" id="x<%= zPage_list.lRowIndex %>_zPageName" size="60" maxlength="60" value="<%= zPage.zPageName.EditValue %>"<%= zPage.zPageName.EditAttributes %> />
<input type="hidden" name="o<%= zPage_list.lRowIndex %>_zPageName" id="o<%= zPage_list.lRowIndex %>_zPageName" value="<%= ew_HTMLEncode(zPage.zPageName.OldValue) %>" />
<% End If %>
<% If zPage.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<input type="text" name="x<%= zPage_list.lRowIndex %>_zPageName" id="x<%= zPage_list.lRowIndex %>_zPageName" size="60" maxlength="60" value="<%= zPage.zPageName.EditValue %>"<%= zPage.zPageName.EditAttributes %> />
<% End If %>
<% If zPage.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= zPage.zPageName.ViewAttributes %>><%= zPage.zPageName.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	<% If zPage.SiteCategoryID.Visible Then ' SiteCategoryID %>
		<td<%= zPage.SiteCategoryID.CellAttributes %>>
<% If zPage.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<select id="x<%= zPage_list.lRowIndex %>_SiteCategoryID" name="x<%= zPage_list.lRowIndex %>_SiteCategoryID"<%= zPage.SiteCategoryID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(zPage.SiteCategoryID.EditValue) Then
	arwrk = zPage.SiteCategoryID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), zPage.SiteCategoryID.CurrentValue) Then
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
If emptywrk Then zPage.SiteCategoryID.OldValue = ""
%>
</select>
<input type="hidden" name="o<%= zPage_list.lRowIndex %>_SiteCategoryID" id="o<%= zPage_list.lRowIndex %>_SiteCategoryID" value="<%= ew_HTMLEncode(zPage.SiteCategoryID.OldValue) %>" />
<% End If %>
<% If zPage.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<select id="x<%= zPage_list.lRowIndex %>_SiteCategoryID" name="x<%= zPage_list.lRowIndex %>_SiteCategoryID"<%= zPage.SiteCategoryID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(zPage.SiteCategoryID.EditValue) Then
	arwrk = zPage.SiteCategoryID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), zPage.SiteCategoryID.CurrentValue) Then
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
If emptywrk Then zPage.SiteCategoryID.OldValue = ""
%>
</select>
<% End If %>
<% If zPage.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= zPage.SiteCategoryID.ViewAttributes %>><%= zPage.SiteCategoryID.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	<% If zPage.SiteCategoryGroupID.Visible Then ' SiteCategoryGroupID %>
		<td<%= zPage.SiteCategoryGroupID.CellAttributes %>>
<% If zPage.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<select id="x<%= zPage_list.lRowIndex %>_SiteCategoryGroupID" name="x<%= zPage_list.lRowIndex %>_SiteCategoryGroupID"<%= zPage.SiteCategoryGroupID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(zPage.SiteCategoryGroupID.EditValue) Then
	arwrk = zPage.SiteCategoryGroupID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), zPage.SiteCategoryGroupID.CurrentValue) Then
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
If emptywrk Then zPage.SiteCategoryGroupID.OldValue = ""
%>
</select>
<input type="hidden" name="o<%= zPage_list.lRowIndex %>_SiteCategoryGroupID" id="o<%= zPage_list.lRowIndex %>_SiteCategoryGroupID" value="<%= ew_HTMLEncode(zPage.SiteCategoryGroupID.OldValue) %>" />
<% End If %>
<% If zPage.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<select id="x<%= zPage_list.lRowIndex %>_SiteCategoryGroupID" name="x<%= zPage_list.lRowIndex %>_SiteCategoryGroupID"<%= zPage.SiteCategoryGroupID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(zPage.SiteCategoryGroupID.EditValue) Then
	arwrk = zPage.SiteCategoryGroupID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), zPage.SiteCategoryGroupID.CurrentValue) Then
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
If emptywrk Then zPage.SiteCategoryGroupID.OldValue = ""
%>
</select>
<% End If %>
<% If zPage.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= zPage.SiteCategoryGroupID.ViewAttributes %>><%= zPage.SiteCategoryGroupID.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	<% If zPage.ModifiedDT.Visible Then ' ModifiedDT %>
		<td<%= zPage.ModifiedDT.CellAttributes %>>
<% If zPage.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<input type="hidden" name="o<%= zPage_list.lRowIndex %>_ModifiedDT" id="o<%= zPage_list.lRowIndex %>_ModifiedDT" value="<%= ew_HTMLEncode(zPage.ModifiedDT.OldValue) %>" />
<% End If %>
<% If zPage.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<% End If %>
<% If zPage.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= zPage.ModifiedDT.ViewAttributes %>><%= zPage.ModifiedDT.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	</tr>
<% If zPage.RowType = EW_ROWTYPE_ADD Then %>
<% End If %>
<% If zPage.RowType = EW_ROWTYPE_EDIT Then %>
<% End If %>
<%
	End If
Loop
%>
</tbody>
</table>
<% End If %>
<% If zPage.CurrentAction = "gridadd" Then %>
<input type="hidden" name="a_list" id="a_list" value="gridinsert" />
<input type="hidden" name="key_count" id="key_count" value="<%= zPage_list.lRowIndex %>" />
<% End If %>
<% If zPage.CurrentAction = "edit" Then %>
<input type="hidden" name="key_count" id="key_count" value="<%= zPage_list.lRowIndex %>" />
<% End If %>
<% If zPage.CurrentAction = "gridedit" Then %>
<input type="hidden" name="a_list" id="a_list" value="gridupdate" />
<input type="hidden" name="key_count" id="key_count" value="<%= zPage_list.lRowIndex %>" />
<%= zPage_list.sMultiSelectKey %>
<% End If %>
</form>
<%

' Close recordset
Rs.Close()
Rs.Dispose()
%>
</div>
<% If zPage_list.lTotalRecs > 0 Then %>
<% If zPage.Export = "" Then %>
<div class="ewGridLowerPanel">
<% If zPage.CurrentAction <> "gridadd" AndAlso zPage.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If zPage_list.Pager Is Nothing Then zPage_list.Pager = New cPrevNextPager(zPage_list.lStartRec, zPage_list.lDisplayRecs, zPage_list.lTotalRecs) %>
<% If zPage_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If zPage_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= zPage_list.PageUrl %>start=<%= zPage_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If zPage_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= zPage_list.PageUrl %>start=<%= zPage_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= zPage_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If zPage_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= zPage_list.PageUrl %>start=<%= zPage_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If zPage_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= zPage_list.PageUrl %>start=<%= zPage_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= zPage_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= zPage_list.Pager.FromIndex %> to <%= zPage_list.Pager.ToIndex %> of <%= zPage_list.Pager.RecordCount %></span>
<% Else %>
	<% If zPage_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If zPage_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="zPage" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If zPage_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If zPage_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If zPage_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If zPage.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<% 'If zPage_list.lTotalRecs > 0 Then %>
<div class="aspnetmaker">
<% If zPage.CurrentAction <> "gridadd" AndAlso zPage.CurrentAction <> "gridedit" Then ' Not grid add/edit mode %>
<a href="<%= zPage.AddUrl %>">Add</a>&nbsp;&nbsp;
<a href="<%= zPage_list.PageUrl %>a=gridadd">Grid Add</a>&nbsp;&nbsp;
<% If zPage_list.lTotalRecs > 0 Then %>
<a href="<%= zPage_list.PageUrl %>a=gridedit">Grid Edit</a>&nbsp;&nbsp;
<% End If %>
<% Else ' Grid add/edit mode %>
<% If zPage.CurrentAction = "gridadd" Then %>
<a href="" onclick="f=ew_GetForm('fzPagelist');if (zPage_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/insert.gif' alt='Insert' title='Insert' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<% If zPage.CurrentAction = "gridedit" Then %>
<a href="" onclick="f=ew_GetForm('fzPagelist');if (zPage_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/update.gif' alt='Save' title='Save' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<a href="<%= zPage_list.PageUrl %>a=cancel"><img src='images/cancel.gif' alt='Cancel' title='Cancel' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
</div>
<% 'End If %>
</div>
<% End If %>
<% End If %>
</td></tr></table>
<% If zPage.Export = "" AndAlso zPage.CurrentAction = "" Then %>
<script type="text/javascript">
<!--
//ew_ToggleSearchPanel(zPage_list); // uncomment to init search panel as collapsed
//-->
</script>
<% End If %>
<% If zPage.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
