<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="Link_list.aspx.vb" Inherits="Link_list" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If Link.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var Link_list = new ew_Page("Link_list");
// page properties
Link_list.PageID = "list"; // page ID
var EW_PAGE_ID = Link_list.PageID; // for backward compatibility
// extend page with ValidateForm function
Link_list.ValidateForm = function(fobj) {
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
		elm = fobj.elements["x" + infix + "_Title"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field - Title");
		elm = fobj.elements["x" + infix + "_LinkTypeCD"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field - Part Type");
		elm = fobj.elements["x" + infix + "_CategoryID"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field - Part Category");
		elm = fobj.elements["x" + infix + "_Ranks"];
		if (elm && !ew_CheckInteger(elm.value))
			return ew_OnError(this, elm, "Incorrect integer - Ranks");
		elm = fobj.elements["x" + infix + "_DateAdd"];
		if (elm && !ew_CheckUSDate(elm.value))
			return ew_OnError(this, elm, "Incorrect date, format = mm/dd/yyyy - Date Add");
		} // End Grid Add checking
	}
	if (fobj.a_list && fobj.a_list.value == "gridinsert" && addcnt == 0) { // No row added
		alert("No records to be added");
		return false;
	}
	return true;
}
// Extend page with empty row check
Link_list.EmptyRow = function(fobj, infix) {
	if (ew_ValueChanged(fobj, infix, "Title")) return false;
	if (ew_ValueChanged(fobj, infix, "LinkTypeCD")) return false;
	if (ew_ValueChanged(fobj, infix, "CategoryID")) return false;
	if (ew_ValueChanged(fobj, infix, "SiteCategoryGroupID")) return false;
	if (ew_ValueChanged(fobj, infix, "zPageID")) return false;
	if (ew_ValueChanged(fobj, infix, "Views")) return false;
	if (ew_ValueChanged(fobj, infix, "Ranks")) return false;
	if (ew_ValueChanged(fobj, infix, "DateAdd")) return false;
	return true;
}
<% If EW_CLIENT_VALIDATE Then %>
Link_list.ValidateRequired = true; // uses JavaScript validation
<% Else %>
Link_list.ValidateRequired = false; // no JavaScript validation
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
<link rel="stylesheet" type="text/css" media="all" href="calendar/calendar-win2k-1.css" title="win2k-1" />
<script type="text/javascript" src="calendar/calendar.js"></script>
<script type="text/javascript" src="calendar/lang/calendar-en.js"></script>
<script type="text/javascript" src="calendar/calendar-setup.js"></script>
<script language="JavaScript" type="text/javascript">
<!--
// Write your client script here, no need to add script tags.
// To include another .js script, use:
// ew_ClientScriptInclude("my_javascript.js"); 
//-->
</script>
<% End If %>
<% If Link.Export = "" Then %>
<% End If %>
<%
If Link.CurrentAction = "gridadd" Then Link.CurrentFilter = "0=1"

' Load recordset
Rs = Link_list.LoadRecordset()
If Link.CurrentAction = "gridadd" Then
	Link_list.lStartRec = 1
	If Link_list.lDisplayRecs <= 0 Then Link_list.lDisplayRecs = 25
	Link_list.lTotalRecs = Link_list.lDisplayRecs
	Link_list.lStopRec = Link_list.lDisplayRecs
Else
	Link_list.lStartRec = 1
	If Link_list.lDisplayRecs <= 0 Then ' Display all records
		Link_list.lDisplayRecs = Link_list.lTotalRecs
	End If
	If Not (Link.ExportAll AndAlso Link.Export <> "") Then
		Link_list.SetUpStartRec() ' Set up start record position
	End If
End If
%>
<p><span class="aspnetmaker" style="white-space: nowrap;">TABLE: Site Parts
<% If Link.Export = "" AndAlso Link.CurrentAction = "" Then %>
&nbsp;&nbsp;<a href="<%= Link_list.PageUrl %>export=excel"><img src='images/exportxls.gif' alt='Export to Excel' title='Export to Excel' width='16' height='16' border='0'></a>
<% End If %>
</span></p>
<% If Link.Export = "" AndAlso Link.CurrentAction = "" Then %>
<a href="javascript:ew_ToggleSearchPanel(Link_list);" style="text-decoration: none;"><img id="Link_list_SearchImage" src="images/collapse.gif" alt="" width="9" height="9" border="0"></a><span class="aspnetmaker">&nbsp;Search</span><br>
<div id="Link_list_SearchPanel">
<form name="fLinklistsrch" id="fLinklistsrch" class="ewForm">
<input type="hidden" id="t" name="t" value="Link" />
<table class="ewBasicSearch">
	<tr>
		<td><span class="aspnetmaker">
			<input type="text" name="<%= EW_TABLE_BASIC_SEARCH %>" id="<%= EW_TABLE_BASIC_SEARCH %>" size="20" value="<%= ew_HtmlEncode(Link.BasicSearchKeyword) %>" />
			<input type="Submit" name="Submit" id="Submit" value="Search (*)" />&nbsp;
			<a href="<%= Link_list.PageUrl %>cmd=reset">Show all</a>&nbsp;
			<a href="Link_srch.aspx">Advanced Search</a>&nbsp;
		</span></td>
	</tr>
	<tr>
	<td><span class="aspnetmaker"><label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value=""<% If Link.BasicSearchType = "" Then %> checked="checked"<% End If %> />Exact phrase</label>&nbsp;&nbsp;<label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value="AND"<% If Link.BasicSearchType = "AND" Then %> checked="checked"<% End If %> />All words</label>&nbsp;&nbsp;<label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value="OR"<% If Link.BasicSearchType = "OR" Then %> checked="checked"<% End If %> />Any word</label></span></td>
	</tr>
</table>
</form>
</div>
<% End If %>
<% Link_list.ShowMessage() %>
<br />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<% If Link.Export = "" Then %>
<div class="ewGridUpperPanel">
<% If Link.CurrentAction <> "gridadd" AndAlso Link.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If Link_list.Pager Is Nothing Then Link_list.Pager = New cPrevNextPager(Link_list.lStartRec, Link_list.lDisplayRecs, Link_list.lTotalRecs) %>
<% If Link_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If Link_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= Link_list.PageUrl %>start=<%= Link_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If Link_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= Link_list.PageUrl %>start=<%= Link_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= Link_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If Link_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= Link_list.PageUrl %>start=<%= Link_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If Link_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= Link_list.PageUrl %>start=<%= Link_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= Link_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= Link_list.Pager.FromIndex %> to <%= Link_list.Pager.ToIndex %> of <%= Link_list.Pager.RecordCount %></span>
<% Else %>
	<% If Link_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If Link_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="Link" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If Link_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If Link_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If Link_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If Link.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<div class="aspnetmaker">
<% If Link.CurrentAction <> "gridadd" AndAlso Link.CurrentAction <> "gridedit" Then ' Not grid add/edit mode %>
<a href="<%= Link.AddUrl %>">Add</a>&nbsp;&nbsp;
<a href="<%= Link_list.PageUrl %>a=gridadd">Grid Add</a>&nbsp;&nbsp;
<% If Link_list.lTotalRecs > 0 Then %>
<a href="<%= Link_list.PageUrl %>a=gridedit">Grid Edit</a>&nbsp;&nbsp;
<% End If %>
<% Else ' Grid add/edit mode %>
<% If Link.CurrentAction = "gridadd" Then %>
<a href="" onclick="f=ew_GetForm('fLinklist');if (Link_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/insert.gif' alt='Insert' title='Insert' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<% If Link.CurrentAction = "gridedit" Then %>
<a href="" onclick="f=ew_GetForm('fLinklist');if (Link_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/update.gif' alt='Save' title='Save' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<a href="<%= Link_list.PageUrl %>a=cancel"><img src='images/cancel.gif' alt='Cancel' title='Cancel' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
</div>
</div>
<% End If %>
<div class="ewGridMiddlePanel">
<form name="fLinklist" id="fLinklist" class="ewForm" method="post">
<input type="hidden" name="t" id="t" value="Link" />
<% If Link_list.lTotalRecs > 0 Then %>
<table cellspacing="0" rowhighlightclass="ewTableHighlightRow" rowselectclass="ewTableSelectRow" roweditclass="ewTableEditRow" class="ewTable ewTableSeparate">
<%
	Link_list.lOptionCnt = 0
	Link_list.lOptionCnt = Link_list.lOptionCnt + 1 ' View
	Link_list.lOptionCnt = Link_list.lOptionCnt + 1 ' Edit
	Link_list.lOptionCnt = Link_list.lOptionCnt + 1 ' Copy
	Link_list.lOptionCnt = Link_list.lOptionCnt + 1 ' Delete
	Link_list.lOptionCnt = Link_list.lOptionCnt + Link_list.ListOptions.Items.Count ' Custom list options
%>
<%= Link.TableCustomInnerHTML %>
<thead><!-- Table header -->
	<tr class="ewTableHeader">
<% If Link.Export = "" Then %>
<% If Link.CurrentAction <> "gridadd" AndAlso Link.CurrentAction <> "gridedit" Then %>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<%

' Custom list options
For i As Integer = 0 to Link_list.ListOptions.Items.Count -1
	If Link_list.ListOptions.Items(i).Visible Then Response.Write(Link_list.ListOptions.Items(i).HeaderCellHtml)
Next
%>
<% End If %>
<% End If %>
<% If Link.Title.Visible Then ' Title %>
	<% If Link.SortUrl(Link.Title) = "" Then %>
		<td>Title</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= Link.SortUrl(Link.Title) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Title&nbsp;(*)</td><td style="width: 10px;"><% If Link.Title.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Link.Title.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If Link.LinkTypeCD.Visible Then ' LinkTypeCD %>
	<% If Link.SortUrl(Link.LinkTypeCD) = "" Then %>
		<td>Part Type</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= Link.SortUrl(Link.LinkTypeCD) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Part Type</td><td style="width: 10px;"><% If Link.LinkTypeCD.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Link.LinkTypeCD.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If Link.CategoryID.Visible Then ' CategoryID %>
	<% If Link.SortUrl(Link.CategoryID) = "" Then %>
		<td>Part Category</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= Link.SortUrl(Link.CategoryID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Part Category</td><td style="width: 10px;"><% If Link.CategoryID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Link.CategoryID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If Link.SiteCategoryGroupID.Visible Then ' SiteCategoryGroupID %>
	<% If Link.SortUrl(Link.SiteCategoryGroupID) = "" Then %>
		<td>Location Group</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= Link.SortUrl(Link.SiteCategoryGroupID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Location Group</td><td style="width: 10px;"><% If Link.SiteCategoryGroupID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Link.SiteCategoryGroupID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If Link.zPageID.Visible Then ' PageID %>
	<% If Link.SortUrl(Link.zPageID) = "" Then %>
		<td>Location</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= Link.SortUrl(Link.zPageID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Location</td><td style="width: 10px;"><% If Link.zPageID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Link.zPageID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If Link.Views.Visible Then ' Views %>
	<% If Link.SortUrl(Link.Views) = "" Then %>
		<td>Visible/Active</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= Link.SortUrl(Link.Views) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Visible/Active</td><td style="width: 10px;"><% If Link.Views.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Link.Views.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If Link.Ranks.Visible Then ' Ranks %>
	<% If Link.SortUrl(Link.Ranks) = "" Then %>
		<td>Ranks</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= Link.SortUrl(Link.Ranks) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Ranks</td><td style="width: 10px;"><% If Link.Ranks.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Link.Ranks.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If Link.DateAdd.Visible Then ' DateAdd %>
	<% If Link.SortUrl(Link.DateAdd) = "" Then %>
		<td>Date Add</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= Link.SortUrl(Link.DateAdd) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Date Add</td><td style="width: 10px;"><% If Link.DateAdd.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Link.DateAdd.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
	</tr>
</thead>
<tbody><!-- Table body -->
<%
If (Link.ExportAll AndAlso Link.Export <> "") Then
	Link_list.lStopRec = Link_list.lTotalRecs
Else
	Link_list.lStopRec = Link_list.lStartRec + Link_list.lDisplayRecs - 1 ' Set the last record to display
End If
If Link.CurrentAction = "gridadd" AndAlso Link_list.lStopRec = -1 Then
	Link_list.lStopRec = EW_GRIDADD_ROWS
End If 

' Move to first record
For i As Integer = 1 to Link_list.lStartRec - 1
	If Rs.Read() Then	Link_list.lRecCnt = Link_list.lRecCnt + 1
Next		
Link_list.lRowCnt = 0
Link_list.lEditRowCnt = 0
If Link.CurrentAction = "edit" Then Link_list.lRowIndex = 1
If Link.CurrentAction = "gridadd" Then Link_list.lRowIndex = 0
If Link.CurrentAction = "gridedit" Then Link_list.lRowIndex = 0

' Output data rows
Do While (Link.CurrentAction = "gridadd" OrElse Rs.Read()) AndAlso (Link_list.lRecCnt < Link_list.lStopRec)
	Link_list.lRecCnt = Link_list.lRecCnt + 1
	If Link_list.lRecCnt >= Link_list.lStartRec Then
		Link_list.lRowCnt = Link_list.lRowCnt + 1
		If Link.CurrentAction = "gridadd" OrElse Link.CurrentAction = "gridedit" Then Link_list.lRowIndex = Link_list.lRowIndex + 1
	Link.CssClass = ""
	Link.CssStyle = ""
	Link.RowClientEvents = "onmouseover='ew_MouseOver(event, this);' onmouseout='ew_MouseOut(event, this);' onclick='ew_Click(event, this);'"
	If Link.CurrentAction = "gridadd" Then
		Link_list.LoadDefaultValues() ' Load default values
	Else
		Link_list.LoadRowValues(Rs) ' Load row values
	End If
	Link.RowType = EW_ROWTYPE_VIEW ' Render view
	If Link.CurrentAction = "gridadd" Then ' Grid add
		Link.RowType = EW_ROWTYPE_ADD ' Render add
	End If
	If Link.CurrentAction = "gridadd" AndAlso Link.EventCancelled Then ' Insert failed
		Link_list.RestoreCurrentRowFormValues(Link_list.lRowIndex) ' Restore form values
	End If
	If Link.CurrentAction = "edit" Then
		If Link_list.CheckInlineEditKey() AndAlso Link_list.lEditRowCnt = 0 Then ' Inline edit
			Link.RowType = EW_ROWTYPE_EDIT ' Render edit
		End If
	End If
	If Link.CurrentAction = "gridedit" Then ' Grid edit
		Link.RowType = EW_ROWTYPE_EDIT ' Render edit
	End If
	If Link.RowType = EW_ROWTYPE_EDIT AndAlso Link.EventCancelled Then ' update failed
		If Link.CurrentAction = "edit" Then
			Link_list.RestoreFormValues() ' Restore form values
		End If
		If Link.CurrentAction = "gridedit" Then
			Link_list.RestoreCurrentRowFormValues(Link_list.lRowIndex) ' Restore form values
		End If
	End If
	If Link.RowType = EW_ROWTYPE_EDIT Then ' Edit row
		Link_list.lEditRowCnt = Link_list.lEditRowCnt + 1
		Link.RowClientEvents = "onmouseover='this.edit=true;ew_MouseOver(event, this);' onmouseout='ew_MouseOut(event, this);' onclick='ew_Click(event, this);'"
	End If
	If Link.RowType = EW_ROWTYPE_ADD OrElse Link.RowType = EW_ROWTYPE_EDIT Then ' Add / Edit row
		Link.CssClass = "ewTableEditRow"
	End If

	' Render row
	Link_list.RenderRow()
%>
	<tr<%= Link.RowAttributes %>>
<% If Link.RowType = EW_ROWTYPE_ADD OrElse Link.RowType = EW_ROWTYPE_EDIT Then %>
<% If Link.CurrentAction = "edit" Then %>
<td colspan="<%= Link_list.lOptionCnt %>" align="right"><span class="aspnetmaker">
<a href="" onclick="f=ew_GetForm('fLinklist');if (Link_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/update.gif' alt='Update' title='Update' width='16' height='16' border='0'></a>&nbsp;<a href="<%= Link_list.PageUrl %>a=cancel"><img src='images/cancel.gif' alt='Cancel' title='Cancel' width='16' height='16' border='0'></a>
<input type="hidden" name="a_list" id="a_list" value="update" />
</span></td>
<% End If %>
<%
	If Link.CurrentAction = "gridedit" Then
		Link_list.sMultiSelectKey = Link_list.sMultiSelectKey & "<input type=""hidden"" name=""k" & Link_list.lRowIndex & "_key"" id=""k" & Link_list.lRowIndex & "_key"" value=""" & Link.ID.CurrentValue & """ />"
	End If
%>
<% Else %>
<% If Link.Export = "" Then %>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= Link.ViewUrl %>"><img src='images/view.gif' alt='View' title='View' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= Link.EditUrl %>"><img src='images/edit.gif' alt='Edit' title='Edit' width='16' height='16' border='0'></a><span class="ewSeparator">&nbsp;|&nbsp;</span><a href="<%= Link.InlineEditUrl %>"><img src='images/inlineedit.gif' alt='Inline Edit' title='Inline Edit' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= Link.CopyUrl %>"><img src='images/copy.gif' alt='Copy' title='Copy' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= Link.DeleteUrl %>"><img src='images/delete.gif' alt='Delete' title='Delete' width='16' height='16' border='0'></a>
</span></td>
<%

' Custom list options
For i As Integer = 0 to Link_list.ListOptions.Items.Count -1
	If Link_list.ListOptions.Items(i).Visible Then Response.Write(Link_list.ListOptions.Items(i).BodyCellHtml)
Next
%>
<% End If %>
<% End If %>
	<% If Link.Title.Visible Then ' Title %>
		<td<%= Link.Title.CellAttributes %>>
<% If Link.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<input type="text" name="x<%= Link_list.lRowIndex %>_Title" id="x<%= Link_list.lRowIndex %>_Title" size="30" maxlength="255" value="<%= Link.Title.EditValue %>"<%= Link.Title.EditAttributes %> />
<input type="hidden" name="o<%= Link_list.lRowIndex %>_Title" id="o<%= Link_list.lRowIndex %>_Title" value="<%= ew_HTMLEncode(Link.Title.OldValue) %>" />
<% End If %>
<% If Link.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<input type="text" name="x<%= Link_list.lRowIndex %>_Title" id="x<%= Link_list.lRowIndex %>_Title" size="30" maxlength="255" value="<%= Link.Title.EditValue %>"<%= Link.Title.EditAttributes %> />
<% End If %>
<% If Link.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= Link.Title.ViewAttributes %>><%= Link.Title.ListViewValue %></div>
<% End If %>
<% If Link.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<input type="hidden" name="o<%= Link_list.lRowIndex %>_ID" id="o<%= Link_list.lRowIndex %>_ID" value="<%= ew_HTMLEncode(Link.ID.OldValue) %>" />
<% End If %>
<% If Link.RowType = EW_ROWTYPE_EDIT Then %>
<input type="hidden" name="x<%= Link_list.lRowIndex %>_ID" id="x<%= Link_list.lRowIndex %>_ID" value="<%= ew_HTMLEncode(Link.ID.CurrentValue) %>" />
<% End If %>
</td>
	<% End If %>
	<% If Link.LinkTypeCD.Visible Then ' LinkTypeCD %>
		<td<%= Link.LinkTypeCD.CellAttributes %>>
<% If Link.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<select id="x<%= Link_list.lRowIndex %>_LinkTypeCD" name="x<%= Link_list.lRowIndex %>_LinkTypeCD"<%= Link.LinkTypeCD.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Link.LinkTypeCD.EditValue) Then
	arwrk = Link.LinkTypeCD.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Link.LinkTypeCD.CurrentValue) Then
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
If emptywrk Then Link.LinkTypeCD.OldValue = ""
%>
</select>
<input type="hidden" name="o<%= Link_list.lRowIndex %>_LinkTypeCD" id="o<%= Link_list.lRowIndex %>_LinkTypeCD" value="<%= ew_HTMLEncode(Link.LinkTypeCD.OldValue) %>" />
<% End If %>
<% If Link.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<select id="x<%= Link_list.lRowIndex %>_LinkTypeCD" name="x<%= Link_list.lRowIndex %>_LinkTypeCD"<%= Link.LinkTypeCD.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Link.LinkTypeCD.EditValue) Then
	arwrk = Link.LinkTypeCD.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Link.LinkTypeCD.CurrentValue) Then
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
If emptywrk Then Link.LinkTypeCD.OldValue = ""
%>
</select>
<% End If %>
<% If Link.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= Link.LinkTypeCD.ViewAttributes %>><%= Link.LinkTypeCD.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	<% If Link.CategoryID.Visible Then ' CategoryID %>
		<td<%= Link.CategoryID.CellAttributes %>>
<% If Link.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<select id="x<%= Link_list.lRowIndex %>_CategoryID" name="x<%= Link_list.lRowIndex %>_CategoryID"<%= Link.CategoryID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Link.CategoryID.EditValue) Then
	arwrk = Link.CategoryID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Link.CategoryID.CurrentValue) Then
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
If emptywrk Then Link.CategoryID.OldValue = ""
%>
</select>
<input type="hidden" name="o<%= Link_list.lRowIndex %>_CategoryID" id="o<%= Link_list.lRowIndex %>_CategoryID" value="<%= ew_HTMLEncode(Link.CategoryID.OldValue) %>" />
<% End If %>
<% If Link.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<select id="x<%= Link_list.lRowIndex %>_CategoryID" name="x<%= Link_list.lRowIndex %>_CategoryID"<%= Link.CategoryID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Link.CategoryID.EditValue) Then
	arwrk = Link.CategoryID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Link.CategoryID.CurrentValue) Then
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
If emptywrk Then Link.CategoryID.OldValue = ""
%>
</select>
<% End If %>
<% If Link.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= Link.CategoryID.ViewAttributes %>><%= Link.CategoryID.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	<% If Link.SiteCategoryGroupID.Visible Then ' SiteCategoryGroupID %>
		<td<%= Link.SiteCategoryGroupID.CellAttributes %>>
<% If Link.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<select id="x<%= Link_list.lRowIndex %>_SiteCategoryGroupID" name="x<%= Link_list.lRowIndex %>_SiteCategoryGroupID"<%= Link.SiteCategoryGroupID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Link.SiteCategoryGroupID.EditValue) Then
	arwrk = Link.SiteCategoryGroupID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Link.SiteCategoryGroupID.CurrentValue) Then
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
If emptywrk Then Link.SiteCategoryGroupID.OldValue = ""
%>
</select>
<input type="hidden" name="o<%= Link_list.lRowIndex %>_SiteCategoryGroupID" id="o<%= Link_list.lRowIndex %>_SiteCategoryGroupID" value="<%= ew_HTMLEncode(Link.SiteCategoryGroupID.OldValue) %>" />
<% End If %>
<% If Link.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<select id="x<%= Link_list.lRowIndex %>_SiteCategoryGroupID" name="x<%= Link_list.lRowIndex %>_SiteCategoryGroupID"<%= Link.SiteCategoryGroupID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Link.SiteCategoryGroupID.EditValue) Then
	arwrk = Link.SiteCategoryGroupID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Link.SiteCategoryGroupID.CurrentValue) Then
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
If emptywrk Then Link.SiteCategoryGroupID.OldValue = ""
%>
</select>
<% End If %>
<% If Link.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= Link.SiteCategoryGroupID.ViewAttributes %>><%= Link.SiteCategoryGroupID.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	<% If Link.zPageID.Visible Then ' PageID %>
		<td<%= Link.zPageID.CellAttributes %>>
<% If Link.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<select id="x<%= Link_list.lRowIndex %>_zPageID" name="x<%= Link_list.lRowIndex %>_zPageID"<%= Link.zPageID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Link.zPageID.EditValue) Then
	arwrk = Link.zPageID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Link.zPageID.CurrentValue) Then
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
If emptywrk Then Link.zPageID.OldValue = ""
%>
</select>
<input type="hidden" name="o<%= Link_list.lRowIndex %>_zPageID" id="o<%= Link_list.lRowIndex %>_zPageID" value="<%= ew_HTMLEncode(Link.zPageID.OldValue) %>" />
<% End If %>
<% If Link.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<select id="x<%= Link_list.lRowIndex %>_zPageID" name="x<%= Link_list.lRowIndex %>_zPageID"<%= Link.zPageID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Link.zPageID.EditValue) Then
	arwrk = Link.zPageID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Link.zPageID.CurrentValue) Then
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
If emptywrk Then Link.zPageID.OldValue = ""
%>
</select>
<% End If %>
<% If Link.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= Link.zPageID.ViewAttributes %>><%= Link.zPageID.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	<% If Link.Views.Visible Then ' Views %>
		<td<%= Link.Views.CellAttributes %>>
<% If Link.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<%
If ew_SameStr(Link.Views.CurrentValue, "1") Then
	selwrk = " checked=""checked"""
Else
	selwrk = ""
End If
%>
<input type="checkbox" name="x<%= Link_list.lRowIndex %>_Views" id="x<%= Link_list.lRowIndex %>_Views" value="1"<%= selwrk %><%= Link.Views.EditAttributes %> />
<input type="hidden" name="o<%= Link_list.lRowIndex %>_Views" id="o<%= Link_list.lRowIndex %>_Views" value="<%= ew_HTMLEncode(Link.Views.OldValue) %>" />
<% End If %>
<% If Link.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<%
If ew_SameStr(Link.Views.CurrentValue, "1") Then
	selwrk = " checked=""checked"""
Else
	selwrk = ""
End If
%>
<input type="checkbox" name="x<%= Link_list.lRowIndex %>_Views" id="x<%= Link_list.lRowIndex %>_Views" value="1"<%= selwrk %><%= Link.Views.EditAttributes %> />
<% End If %>
<% If Link.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<% If Convert.ToString(Link.Views.CurrentValue) = "1" Then %>
<input type="checkbox" value="<%= Link.Views.ListViewValue %>" checked onclick="this.form.reset();" disabled="disabled" />
<% Else %>
<input type="checkbox" value="<%= Link.Views.ListViewValue %>" onclick="this.form.reset();" disabled="disabled" />
<% End If %>
<% End If %>
</td>
	<% End If %>
	<% If Link.Ranks.Visible Then ' Ranks %>
		<td<%= Link.Ranks.CellAttributes %>>
<% If Link.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<input type="text" name="x<%= Link_list.lRowIndex %>_Ranks" id="x<%= Link_list.lRowIndex %>_Ranks" size="30" value="<%= Link.Ranks.EditValue %>"<%= Link.Ranks.EditAttributes %> />
<input type="hidden" name="o<%= Link_list.lRowIndex %>_Ranks" id="o<%= Link_list.lRowIndex %>_Ranks" value="<%= ew_HTMLEncode(Link.Ranks.OldValue) %>" />
<% End If %>
<% If Link.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<input type="text" name="x<%= Link_list.lRowIndex %>_Ranks" id="x<%= Link_list.lRowIndex %>_Ranks" size="30" value="<%= Link.Ranks.EditValue %>"<%= Link.Ranks.EditAttributes %> />
<% End If %>
<% If Link.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= Link.Ranks.ViewAttributes %>><%= Link.Ranks.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	<% If Link.DateAdd.Visible Then ' DateAdd %>
		<td<%= Link.DateAdd.CellAttributes %>>
<% If Link.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<input type="text" name="x<%= Link_list.lRowIndex %>_DateAdd" id="x<%= Link_list.lRowIndex %>_DateAdd" value="<%= Link.DateAdd.EditValue %>"<%= Link.DateAdd.EditAttributes %> />
&nbsp;<img src="images/calendar.png" id="cal_x<%= Link_list.lRowIndex %>_DateAdd" name="cal_x<%= Link_list.lRowIndex %>_DateAdd" alt="Pick a date" style="cursor:pointer;cursor:hand;" />
<script type="text/javascript">
Calendar.setup({
	inputField : "x<%= Link_list.lRowIndex %>_DateAdd", // ID of the input field
	ifFormat : "%m/%d/%Y", // the date format
	button : "cal_x<%= Link_list.lRowIndex %>_DateAdd" // ID of the button
});
</script>
<input type="hidden" name="o<%= Link_list.lRowIndex %>_DateAdd" id="o<%= Link_list.lRowIndex %>_DateAdd" value="<%= ew_HTMLEncode(Link.DateAdd.OldValue) %>" />
<% End If %>
<% If Link.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<input type="text" name="x<%= Link_list.lRowIndex %>_DateAdd" id="x<%= Link_list.lRowIndex %>_DateAdd" value="<%= Link.DateAdd.EditValue %>"<%= Link.DateAdd.EditAttributes %> />
&nbsp;<img src="images/calendar.png" id="cal_x<%= Link_list.lRowIndex %>_DateAdd" name="cal_x<%= Link_list.lRowIndex %>_DateAdd" alt="Pick a date" style="cursor:pointer;cursor:hand;" />
<script type="text/javascript">
Calendar.setup({
	inputField : "x<%= Link_list.lRowIndex %>_DateAdd", // ID of the input field
	ifFormat : "%m/%d/%Y", // the date format
	button : "cal_x<%= Link_list.lRowIndex %>_DateAdd" // ID of the button
});
</script>
<% End If %>
<% If Link.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= Link.DateAdd.ViewAttributes %>><%= Link.DateAdd.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	</tr>
<% If Link.RowType = EW_ROWTYPE_ADD Then %>
<% End If %>
<% If Link.RowType = EW_ROWTYPE_EDIT Then %>
<% End If %>
<%
	End If
Loop
%>
</tbody>
</table>
<% End If %>
<% If Link.CurrentAction = "gridadd" Then %>
<input type="hidden" name="a_list" id="a_list" value="gridinsert" />
<input type="hidden" name="key_count" id="key_count" value="<%= Link_list.lRowIndex %>" />
<% End If %>
<% If Link.CurrentAction = "edit" Then %>
<input type="hidden" name="key_count" id="key_count" value="<%= Link_list.lRowIndex %>" />
<% End If %>
<% If Link.CurrentAction = "gridedit" Then %>
<input type="hidden" name="a_list" id="a_list" value="gridupdate" />
<input type="hidden" name="key_count" id="key_count" value="<%= Link_list.lRowIndex %>" />
<%= Link_list.sMultiSelectKey %>
<% End If %>
</form>
<%

' Close recordset
Rs.Close()
Rs.Dispose()
%>
</div>
<% If Link_list.lTotalRecs > 0 Then %>
<% If Link.Export = "" Then %>
<div class="ewGridLowerPanel">
<% If Link.CurrentAction <> "gridadd" AndAlso Link.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If Link_list.Pager Is Nothing Then Link_list.Pager = New cPrevNextPager(Link_list.lStartRec, Link_list.lDisplayRecs, Link_list.lTotalRecs) %>
<% If Link_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If Link_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= Link_list.PageUrl %>start=<%= Link_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If Link_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= Link_list.PageUrl %>start=<%= Link_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= Link_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If Link_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= Link_list.PageUrl %>start=<%= Link_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If Link_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= Link_list.PageUrl %>start=<%= Link_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= Link_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= Link_list.Pager.FromIndex %> to <%= Link_list.Pager.ToIndex %> of <%= Link_list.Pager.RecordCount %></span>
<% Else %>
	<% If Link_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If Link_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="Link" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If Link_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If Link_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If Link_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If Link.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<% 'If Link_list.lTotalRecs > 0 Then %>
<div class="aspnetmaker">
<% If Link.CurrentAction <> "gridadd" AndAlso Link.CurrentAction <> "gridedit" Then ' Not grid add/edit mode %>
<a href="<%= Link.AddUrl %>">Add</a>&nbsp;&nbsp;
<a href="<%= Link_list.PageUrl %>a=gridadd">Grid Add</a>&nbsp;&nbsp;
<% If Link_list.lTotalRecs > 0 Then %>
<a href="<%= Link_list.PageUrl %>a=gridedit">Grid Edit</a>&nbsp;&nbsp;
<% End If %>
<% Else ' Grid add/edit mode %>
<% If Link.CurrentAction = "gridadd" Then %>
<a href="" onclick="f=ew_GetForm('fLinklist');if (Link_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/insert.gif' alt='Insert' title='Insert' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<% If Link.CurrentAction = "gridedit" Then %>
<a href="" onclick="f=ew_GetForm('fLinklist');if (Link_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/update.gif' alt='Save' title='Save' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<a href="<%= Link_list.PageUrl %>a=cancel"><img src='images/cancel.gif' alt='Cancel' title='Cancel' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
</div>
<% 'End If %>
</div>
<% End If %>
<% End If %>
</td></tr></table>
<% If Link.Export = "" AndAlso Link.CurrentAction = "" Then %>
<script type="text/javascript">
<!--
//ew_ToggleSearchPanel(Link_list); // uncomment to init search panel as collapsed
//-->
</script>
<% End If %>
<% If Link.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
