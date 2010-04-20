<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="SiteCategoryGroup_list.aspx.vb" Inherits="SiteCategoryGroup_list" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If SiteCategoryGroup.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var SiteCategoryGroup_list = new ew_Page("SiteCategoryGroup_list");
// page properties
SiteCategoryGroup_list.PageID = "list"; // page ID
var EW_PAGE_ID = SiteCategoryGroup_list.PageID; // for backward compatibility
// extend page with ValidateForm function
SiteCategoryGroup_list.ValidateForm = function(fobj) {
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
		elm = fobj.elements["x" + infix + "_SiteCategoryGroupOrder"];
		if (elm && !ew_CheckInteger(elm.value))
			return ew_OnError(this, elm, "Incorrect integer - Site Category Group Order");
		} // End Grid Add checking
	}
	if (fobj.a_list && fobj.a_list.value == "gridinsert" && addcnt == 0) { // No row added
		alert("No records to be added");
		return false;
	}
	return true;
}
// Extend page with empty row check
SiteCategoryGroup_list.EmptyRow = function(fobj, infix) {
	if (ew_ValueChanged(fobj, infix, "SiteCategoryGroupNM")) return false;
	if (ew_ValueChanged(fobj, infix, "SiteCategoryGroupDS")) return false;
	if (ew_ValueChanged(fobj, infix, "SiteCategoryGroupOrder")) return false;
	return true;
}
<% If EW_CLIENT_VALIDATE Then %>
SiteCategoryGroup_list.ValidateRequired = true; // uses JavaScript validation
<% Else %>
SiteCategoryGroup_list.ValidateRequired = false; // no JavaScript validation
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
<% If SiteCategoryGroup.Export = "" Then %>
<% End If %>
<%
If SiteCategoryGroup.CurrentAction = "gridadd" Then SiteCategoryGroup.CurrentFilter = "0=1"

' Load recordset
Rs = SiteCategoryGroup_list.LoadRecordset()
If SiteCategoryGroup.CurrentAction = "gridadd" Then
	SiteCategoryGroup_list.lStartRec = 1
	If SiteCategoryGroup_list.lDisplayRecs <= 0 Then SiteCategoryGroup_list.lDisplayRecs = 25
	SiteCategoryGroup_list.lTotalRecs = SiteCategoryGroup_list.lDisplayRecs
	SiteCategoryGroup_list.lStopRec = SiteCategoryGroup_list.lDisplayRecs
Else
	SiteCategoryGroup_list.lStartRec = 1
	If SiteCategoryGroup_list.lDisplayRecs <= 0 Then ' Display all records
		SiteCategoryGroup_list.lDisplayRecs = SiteCategoryGroup_list.lTotalRecs
	End If
	If Not (SiteCategoryGroup.ExportAll AndAlso SiteCategoryGroup.Export <> "") Then
		SiteCategoryGroup_list.SetUpStartRec() ' Set up start record position
	End If
End If
%>
<p><span class="aspnetmaker" style="white-space: nowrap;">TABLE: Location Group
<% If SiteCategoryGroup.Export = "" AndAlso SiteCategoryGroup.CurrentAction = "" Then %>
&nbsp;&nbsp;<a href="<%= SiteCategoryGroup_list.PageUrl %>export=excel"><img src='images/exportxls.gif' alt='Export to Excel' title='Export to Excel' width='16' height='16' border='0'></a>
<% End If %>
</span></p>
<% If SiteCategoryGroup.Export = "" AndAlso SiteCategoryGroup.CurrentAction = "" Then %>
<a href="javascript:ew_ToggleSearchPanel(SiteCategoryGroup_list);" style="text-decoration: none;"><img id="SiteCategoryGroup_list_SearchImage" src="images/collapse.gif" alt="" width="9" height="9" border="0"></a><span class="aspnetmaker">&nbsp;Search</span><br>
<div id="SiteCategoryGroup_list_SearchPanel">
<form name="fSiteCategoryGrouplistsrch" id="fSiteCategoryGrouplistsrch" class="ewForm">
<input type="hidden" id="t" name="t" value="SiteCategoryGroup" />
<table class="ewBasicSearch">
	<tr>
		<td><span class="aspnetmaker">
			<input type="text" name="<%= EW_TABLE_BASIC_SEARCH %>" id="<%= EW_TABLE_BASIC_SEARCH %>" size="20" value="<%= ew_HtmlEncode(SiteCategoryGroup.BasicSearchKeyword) %>" />
			<input type="Submit" name="Submit" id="Submit" value="Search (*)" />&nbsp;
			<a href="<%= SiteCategoryGroup_list.PageUrl %>cmd=reset">Show all</a>&nbsp;
			<a href="SiteCategoryGroup_srch.aspx">Advanced Search</a>&nbsp;
		</span></td>
	</tr>
	<tr>
	<td><span class="aspnetmaker"><label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value=""<% If SiteCategoryGroup.BasicSearchType = "" Then %> checked="checked"<% End If %> />Exact phrase</label>&nbsp;&nbsp;<label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value="AND"<% If SiteCategoryGroup.BasicSearchType = "AND" Then %> checked="checked"<% End If %> />All words</label>&nbsp;&nbsp;<label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value="OR"<% If SiteCategoryGroup.BasicSearchType = "OR" Then %> checked="checked"<% End If %> />Any word</label></span></td>
	</tr>
</table>
</form>
</div>
<% End If %>
<% SiteCategoryGroup_list.ShowMessage() %>
<br />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<% If SiteCategoryGroup.Export = "" Then %>
<div class="ewGridUpperPanel">
<% If SiteCategoryGroup.CurrentAction <> "gridadd" AndAlso SiteCategoryGroup.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If SiteCategoryGroup_list.Pager Is Nothing Then SiteCategoryGroup_list.Pager = New cPrevNextPager(SiteCategoryGroup_list.lStartRec, SiteCategoryGroup_list.lDisplayRecs, SiteCategoryGroup_list.lTotalRecs) %>
<% If SiteCategoryGroup_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If SiteCategoryGroup_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= SiteCategoryGroup_list.PageUrl %>start=<%= SiteCategoryGroup_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If SiteCategoryGroup_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= SiteCategoryGroup_list.PageUrl %>start=<%= SiteCategoryGroup_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= SiteCategoryGroup_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If SiteCategoryGroup_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= SiteCategoryGroup_list.PageUrl %>start=<%= SiteCategoryGroup_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If SiteCategoryGroup_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= SiteCategoryGroup_list.PageUrl %>start=<%= SiteCategoryGroup_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= SiteCategoryGroup_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= SiteCategoryGroup_list.Pager.FromIndex %> to <%= SiteCategoryGroup_list.Pager.ToIndex %> of <%= SiteCategoryGroup_list.Pager.RecordCount %></span>
<% Else %>
	<% If SiteCategoryGroup_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If SiteCategoryGroup_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="SiteCategoryGroup" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If SiteCategoryGroup_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If SiteCategoryGroup_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If SiteCategoryGroup_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If SiteCategoryGroup.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<div class="aspnetmaker">
<% If SiteCategoryGroup.CurrentAction <> "gridadd" AndAlso SiteCategoryGroup.CurrentAction <> "gridedit" Then ' Not grid add/edit mode %>
<a href="<%= SiteCategoryGroup.AddUrl %>">Add</a>&nbsp;&nbsp;
<a href="<%= SiteCategoryGroup_list.PageUrl %>a=gridadd">Grid Add</a>&nbsp;&nbsp;
<% If SiteCategoryGroup_list.lTotalRecs > 0 Then %>
<a href="<%= SiteCategoryGroup_list.PageUrl %>a=gridedit">Grid Edit</a>&nbsp;&nbsp;
<% End If %>
<% Else ' Grid add/edit mode %>
<% If SiteCategoryGroup.CurrentAction = "gridadd" Then %>
<a href="" onclick="f=ew_GetForm('fSiteCategoryGrouplist');if (SiteCategoryGroup_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/insert.gif' alt='Insert' title='Insert' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<% If SiteCategoryGroup.CurrentAction = "gridedit" Then %>
<a href="" onclick="f=ew_GetForm('fSiteCategoryGrouplist');if (SiteCategoryGroup_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/update.gif' alt='Save' title='Save' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<a href="<%= SiteCategoryGroup_list.PageUrl %>a=cancel"><img src='images/cancel.gif' alt='Cancel' title='Cancel' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
</div>
</div>
<% End If %>
<div class="ewGridMiddlePanel">
<form name="fSiteCategoryGrouplist" id="fSiteCategoryGrouplist" class="ewForm" method="post">
<input type="hidden" name="t" id="t" value="SiteCategoryGroup" />
<% If SiteCategoryGroup_list.lTotalRecs > 0 Then %>
<table cellspacing="0" rowhighlightclass="ewTableHighlightRow" rowselectclass="ewTableSelectRow" roweditclass="ewTableEditRow" class="ewTable ewTableSeparate">
<%
	SiteCategoryGroup_list.lOptionCnt = 0
	SiteCategoryGroup_list.lOptionCnt = SiteCategoryGroup_list.lOptionCnt + 1 ' View
	SiteCategoryGroup_list.lOptionCnt = SiteCategoryGroup_list.lOptionCnt + 1 ' Edit
	SiteCategoryGroup_list.lOptionCnt = SiteCategoryGroup_list.lOptionCnt + 1 ' Copy
	SiteCategoryGroup_list.lOptionCnt = SiteCategoryGroup_list.lOptionCnt + 1 ' Delete
	SiteCategoryGroup_list.lOptionCnt = SiteCategoryGroup_list.lOptionCnt + SiteCategoryGroup_list.ListOptions.Items.Count ' Custom list options
%>
<%= SiteCategoryGroup.TableCustomInnerHTML %>
<thead><!-- Table header -->
	<tr class="ewTableHeader">
<% If SiteCategoryGroup.Export = "" Then %>
<% If SiteCategoryGroup.CurrentAction <> "gridadd" AndAlso SiteCategoryGroup.CurrentAction <> "gridedit" Then %>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<%

' Custom list options
For i As Integer = 0 to SiteCategoryGroup_list.ListOptions.Items.Count -1
	If SiteCategoryGroup_list.ListOptions.Items(i).Visible Then Response.Write(SiteCategoryGroup_list.ListOptions.Items(i).HeaderCellHtml)
Next
%>
<% End If %>
<% End If %>
<% If SiteCategoryGroup.SiteCategoryGroupID.Visible Then ' SiteCategoryGroupID %>
	<% If SiteCategoryGroup.SortUrl(SiteCategoryGroup.SiteCategoryGroupID) = "" Then %>
		<td>Site Category Group ID</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteCategoryGroup.SortUrl(SiteCategoryGroup.SiteCategoryGroupID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Site Category Group ID</td><td style="width: 10px;"><% If SiteCategoryGroup.SiteCategoryGroupID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteCategoryGroup.SiteCategoryGroupID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If SiteCategoryGroup.SiteCategoryGroupNM.Visible Then ' SiteCategoryGroupNM %>
	<% If SiteCategoryGroup.SortUrl(SiteCategoryGroup.SiteCategoryGroupNM) = "" Then %>
		<td>Site Category Group NM</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteCategoryGroup.SortUrl(SiteCategoryGroup.SiteCategoryGroupNM) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Site Category Group NM&nbsp;(*)</td><td style="width: 10px;"><% If SiteCategoryGroup.SiteCategoryGroupNM.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteCategoryGroup.SiteCategoryGroupNM.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If SiteCategoryGroup.SiteCategoryGroupDS.Visible Then ' SiteCategoryGroupDS %>
	<% If SiteCategoryGroup.SortUrl(SiteCategoryGroup.SiteCategoryGroupDS) = "" Then %>
		<td>Site Category Group DS</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteCategoryGroup.SortUrl(SiteCategoryGroup.SiteCategoryGroupDS) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Site Category Group DS&nbsp;(*)</td><td style="width: 10px;"><% If SiteCategoryGroup.SiteCategoryGroupDS.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteCategoryGroup.SiteCategoryGroupDS.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If SiteCategoryGroup.SiteCategoryGroupOrder.Visible Then ' SiteCategoryGroupOrder %>
	<% If SiteCategoryGroup.SortUrl(SiteCategoryGroup.SiteCategoryGroupOrder) = "" Then %>
		<td>Site Category Group Order</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteCategoryGroup.SortUrl(SiteCategoryGroup.SiteCategoryGroupOrder) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Site Category Group Order</td><td style="width: 10px;"><% If SiteCategoryGroup.SiteCategoryGroupOrder.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteCategoryGroup.SiteCategoryGroupOrder.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
	</tr>
</thead>
<tbody><!-- Table body -->
<%
If (SiteCategoryGroup.ExportAll AndAlso SiteCategoryGroup.Export <> "") Then
	SiteCategoryGroup_list.lStopRec = SiteCategoryGroup_list.lTotalRecs
Else
	SiteCategoryGroup_list.lStopRec = SiteCategoryGroup_list.lStartRec + SiteCategoryGroup_list.lDisplayRecs - 1 ' Set the last record to display
End If
If SiteCategoryGroup.CurrentAction = "gridadd" AndAlso SiteCategoryGroup_list.lStopRec = -1 Then
	SiteCategoryGroup_list.lStopRec = EW_GRIDADD_ROWS
End If 

' Move to first record
For i As Integer = 1 to SiteCategoryGroup_list.lStartRec - 1
	If Rs.Read() Then	SiteCategoryGroup_list.lRecCnt = SiteCategoryGroup_list.lRecCnt + 1
Next		
SiteCategoryGroup_list.lRowCnt = 0
If SiteCategoryGroup.CurrentAction = "gridadd" Then SiteCategoryGroup_list.lRowIndex = 0
If SiteCategoryGroup.CurrentAction = "gridedit" Then SiteCategoryGroup_list.lRowIndex = 0

' Output data rows
Do While (SiteCategoryGroup.CurrentAction = "gridadd" OrElse Rs.Read()) AndAlso (SiteCategoryGroup_list.lRecCnt < SiteCategoryGroup_list.lStopRec)
	SiteCategoryGroup_list.lRecCnt = SiteCategoryGroup_list.lRecCnt + 1
	If SiteCategoryGroup_list.lRecCnt >= SiteCategoryGroup_list.lStartRec Then
		SiteCategoryGroup_list.lRowCnt = SiteCategoryGroup_list.lRowCnt + 1
		If SiteCategoryGroup.CurrentAction = "gridadd" OrElse SiteCategoryGroup.CurrentAction = "gridedit" Then SiteCategoryGroup_list.lRowIndex = SiteCategoryGroup_list.lRowIndex + 1
	SiteCategoryGroup.CssClass = ""
	SiteCategoryGroup.CssStyle = ""
	SiteCategoryGroup.RowClientEvents = "onmouseover='ew_MouseOver(event, this);' onmouseout='ew_MouseOut(event, this);' onclick='ew_Click(event, this);'"
	If SiteCategoryGroup.CurrentAction = "gridadd" Then
		SiteCategoryGroup_list.LoadDefaultValues() ' Load default values
	Else
		SiteCategoryGroup_list.LoadRowValues(Rs) ' Load row values
	End If
	SiteCategoryGroup.RowType = EW_ROWTYPE_VIEW ' Render view
	If SiteCategoryGroup.CurrentAction = "gridadd" Then ' Grid add
		SiteCategoryGroup.RowType = EW_ROWTYPE_ADD ' Render add
	End If
	If SiteCategoryGroup.CurrentAction = "gridadd" AndAlso SiteCategoryGroup.EventCancelled Then ' Insert failed
		SiteCategoryGroup_list.RestoreCurrentRowFormValues(SiteCategoryGroup_list.lRowIndex) ' Restore form values
	End If
	If SiteCategoryGroup.CurrentAction = "gridedit" Then ' Grid edit
		SiteCategoryGroup.RowType = EW_ROWTYPE_EDIT ' Render edit
	End If
	If SiteCategoryGroup.RowType = EW_ROWTYPE_EDIT AndAlso SiteCategoryGroup.EventCancelled Then ' update failed
		If SiteCategoryGroup.CurrentAction = "gridedit" Then
			SiteCategoryGroup_list.RestoreCurrentRowFormValues(SiteCategoryGroup_list.lRowIndex) ' Restore form values
		End If
	End If
	If SiteCategoryGroup.RowType = EW_ROWTYPE_EDIT Then ' Edit row
		SiteCategoryGroup_list.lEditRowCnt = SiteCategoryGroup_list.lEditRowCnt + 1
		SiteCategoryGroup.RowClientEvents = "onmouseover='this.edit=true;ew_MouseOver(event, this);' onmouseout='ew_MouseOut(event, this);' onclick='ew_Click(event, this);'"
	End If
	If SiteCategoryGroup.RowType = EW_ROWTYPE_ADD OrElse SiteCategoryGroup.RowType = EW_ROWTYPE_EDIT Then ' Add / Edit row
		SiteCategoryGroup.CssClass = "ewTableEditRow"
	End If

	' Render row
	SiteCategoryGroup_list.RenderRow()
%>
	<tr<%= SiteCategoryGroup.RowAttributes %>>
<% If SiteCategoryGroup.RowType = EW_ROWTYPE_ADD OrElse SiteCategoryGroup.RowType = EW_ROWTYPE_EDIT Then %>
<%
	If SiteCategoryGroup.CurrentAction = "gridedit" Then
		SiteCategoryGroup_list.sMultiSelectKey = SiteCategoryGroup_list.sMultiSelectKey & "<input type=""hidden"" name=""k" & SiteCategoryGroup_list.lRowIndex & "_key"" id=""k" & SiteCategoryGroup_list.lRowIndex & "_key"" value=""" & SiteCategoryGroup.SiteCategoryGroupID.CurrentValue & """ />"
	End If
%>
<% Else %>
<% If SiteCategoryGroup.Export = "" Then %>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= SiteCategoryGroup.ViewUrl %>"><img src='images/view.gif' alt='View' title='View' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= SiteCategoryGroup.EditUrl %>"><img src='images/edit.gif' alt='Edit' title='Edit' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= SiteCategoryGroup.CopyUrl %>"><img src='images/copy.gif' alt='Copy' title='Copy' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= SiteCategoryGroup.DeleteUrl %>"><img src='images/delete.gif' alt='Delete' title='Delete' width='16' height='16' border='0'></a>
</span></td>
<%

' Custom list options
For i As Integer = 0 to SiteCategoryGroup_list.ListOptions.Items.Count -1
	If SiteCategoryGroup_list.ListOptions.Items(i).Visible Then Response.Write(SiteCategoryGroup_list.ListOptions.Items(i).BodyCellHtml)
Next
%>
<% End If %>
<% End If %>
	<% If SiteCategoryGroup.SiteCategoryGroupID.Visible Then ' SiteCategoryGroupID %>
		<td<%= SiteCategoryGroup.SiteCategoryGroupID.CellAttributes %>>
<% If SiteCategoryGroup.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<input type="hidden" name="o<%= SiteCategoryGroup_list.lRowIndex %>_SiteCategoryGroupID" id="o<%= SiteCategoryGroup_list.lRowIndex %>_SiteCategoryGroupID" value="<%= ew_HTMLEncode(SiteCategoryGroup.SiteCategoryGroupID.OldValue) %>" />
<% End If %>
<% If SiteCategoryGroup.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<div<%= SiteCategoryGroup.SiteCategoryGroupID.ViewAttributes %>><%= SiteCategoryGroup.SiteCategoryGroupID.EditValue %></div>
<input type="hidden" name="x<%= SiteCategoryGroup_list.lRowIndex %>_SiteCategoryGroupID" id="x<%= SiteCategoryGroup_list.lRowIndex %>_SiteCategoryGroupID" value="<%= ew_HTMLEncode(SiteCategoryGroup.SiteCategoryGroupID.CurrentValue) %>" />
<% End If %>
<% If SiteCategoryGroup.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= SiteCategoryGroup.SiteCategoryGroupID.ViewAttributes %>><%= SiteCategoryGroup.SiteCategoryGroupID.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	<% If SiteCategoryGroup.SiteCategoryGroupNM.Visible Then ' SiteCategoryGroupNM %>
		<td<%= SiteCategoryGroup.SiteCategoryGroupNM.CellAttributes %>>
<% If SiteCategoryGroup.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<input type="text" name="x<%= SiteCategoryGroup_list.lRowIndex %>_SiteCategoryGroupNM" id="x<%= SiteCategoryGroup_list.lRowIndex %>_SiteCategoryGroupNM" size="30" maxlength="255" value="<%= SiteCategoryGroup.SiteCategoryGroupNM.EditValue %>"<%= SiteCategoryGroup.SiteCategoryGroupNM.EditAttributes %> />
<input type="hidden" name="o<%= SiteCategoryGroup_list.lRowIndex %>_SiteCategoryGroupNM" id="o<%= SiteCategoryGroup_list.lRowIndex %>_SiteCategoryGroupNM" value="<%= ew_HTMLEncode(SiteCategoryGroup.SiteCategoryGroupNM.OldValue) %>" />
<% End If %>
<% If SiteCategoryGroup.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<input type="text" name="x<%= SiteCategoryGroup_list.lRowIndex %>_SiteCategoryGroupNM" id="x<%= SiteCategoryGroup_list.lRowIndex %>_SiteCategoryGroupNM" size="30" maxlength="255" value="<%= SiteCategoryGroup.SiteCategoryGroupNM.EditValue %>"<%= SiteCategoryGroup.SiteCategoryGroupNM.EditAttributes %> />
<% End If %>
<% If SiteCategoryGroup.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= SiteCategoryGroup.SiteCategoryGroupNM.ViewAttributes %>><%= SiteCategoryGroup.SiteCategoryGroupNM.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	<% If SiteCategoryGroup.SiteCategoryGroupDS.Visible Then ' SiteCategoryGroupDS %>
		<td<%= SiteCategoryGroup.SiteCategoryGroupDS.CellAttributes %>>
<% If SiteCategoryGroup.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<input type="text" name="x<%= SiteCategoryGroup_list.lRowIndex %>_SiteCategoryGroupDS" id="x<%= SiteCategoryGroup_list.lRowIndex %>_SiteCategoryGroupDS" size="30" maxlength="255" value="<%= SiteCategoryGroup.SiteCategoryGroupDS.EditValue %>"<%= SiteCategoryGroup.SiteCategoryGroupDS.EditAttributes %> />
<input type="hidden" name="o<%= SiteCategoryGroup_list.lRowIndex %>_SiteCategoryGroupDS" id="o<%= SiteCategoryGroup_list.lRowIndex %>_SiteCategoryGroupDS" value="<%= ew_HTMLEncode(SiteCategoryGroup.SiteCategoryGroupDS.OldValue) %>" />
<% End If %>
<% If SiteCategoryGroup.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<input type="text" name="x<%= SiteCategoryGroup_list.lRowIndex %>_SiteCategoryGroupDS" id="x<%= SiteCategoryGroup_list.lRowIndex %>_SiteCategoryGroupDS" size="30" maxlength="255" value="<%= SiteCategoryGroup.SiteCategoryGroupDS.EditValue %>"<%= SiteCategoryGroup.SiteCategoryGroupDS.EditAttributes %> />
<% End If %>
<% If SiteCategoryGroup.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= SiteCategoryGroup.SiteCategoryGroupDS.ViewAttributes %>><%= SiteCategoryGroup.SiteCategoryGroupDS.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	<% If SiteCategoryGroup.SiteCategoryGroupOrder.Visible Then ' SiteCategoryGroupOrder %>
		<td<%= SiteCategoryGroup.SiteCategoryGroupOrder.CellAttributes %>>
<% If SiteCategoryGroup.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<input type="text" name="x<%= SiteCategoryGroup_list.lRowIndex %>_SiteCategoryGroupOrder" id="x<%= SiteCategoryGroup_list.lRowIndex %>_SiteCategoryGroupOrder" size="30" value="<%= SiteCategoryGroup.SiteCategoryGroupOrder.EditValue %>"<%= SiteCategoryGroup.SiteCategoryGroupOrder.EditAttributes %> />
<input type="hidden" name="o<%= SiteCategoryGroup_list.lRowIndex %>_SiteCategoryGroupOrder" id="o<%= SiteCategoryGroup_list.lRowIndex %>_SiteCategoryGroupOrder" value="<%= ew_HTMLEncode(SiteCategoryGroup.SiteCategoryGroupOrder.OldValue) %>" />
<% End If %>
<% If SiteCategoryGroup.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<input type="text" name="x<%= SiteCategoryGroup_list.lRowIndex %>_SiteCategoryGroupOrder" id="x<%= SiteCategoryGroup_list.lRowIndex %>_SiteCategoryGroupOrder" size="30" value="<%= SiteCategoryGroup.SiteCategoryGroupOrder.EditValue %>"<%= SiteCategoryGroup.SiteCategoryGroupOrder.EditAttributes %> />
<% End If %>
<% If SiteCategoryGroup.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= SiteCategoryGroup.SiteCategoryGroupOrder.ViewAttributes %>><%= SiteCategoryGroup.SiteCategoryGroupOrder.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	</tr>
<% If SiteCategoryGroup.RowType = EW_ROWTYPE_ADD Then %>
<% End If %>
<% If SiteCategoryGroup.RowType = EW_ROWTYPE_EDIT Then %>
<% End If %>
<%
	End If
Loop
%>
</tbody>
</table>
<% End If %>
<% If SiteCategoryGroup.CurrentAction = "gridadd" Then %>
<input type="hidden" name="a_list" id="a_list" value="gridinsert" />
<input type="hidden" name="key_count" id="key_count" value="<%= SiteCategoryGroup_list.lRowIndex %>" />
<% End If %>
<% If SiteCategoryGroup.CurrentAction = "gridedit" Then %>
<input type="hidden" name="a_list" id="a_list" value="gridupdate" />
<input type="hidden" name="key_count" id="key_count" value="<%= SiteCategoryGroup_list.lRowIndex %>" />
<%= SiteCategoryGroup_list.sMultiSelectKey %>
<% End If %>
</form>
<%

' Close recordset
Rs.Close()
Rs.Dispose()
%>
</div>
<% If SiteCategoryGroup_list.lTotalRecs > 0 Then %>
<% If SiteCategoryGroup.Export = "" Then %>
<div class="ewGridLowerPanel">
<% If SiteCategoryGroup.CurrentAction <> "gridadd" AndAlso SiteCategoryGroup.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If SiteCategoryGroup_list.Pager Is Nothing Then SiteCategoryGroup_list.Pager = New cPrevNextPager(SiteCategoryGroup_list.lStartRec, SiteCategoryGroup_list.lDisplayRecs, SiteCategoryGroup_list.lTotalRecs) %>
<% If SiteCategoryGroup_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If SiteCategoryGroup_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= SiteCategoryGroup_list.PageUrl %>start=<%= SiteCategoryGroup_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If SiteCategoryGroup_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= SiteCategoryGroup_list.PageUrl %>start=<%= SiteCategoryGroup_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= SiteCategoryGroup_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If SiteCategoryGroup_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= SiteCategoryGroup_list.PageUrl %>start=<%= SiteCategoryGroup_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If SiteCategoryGroup_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= SiteCategoryGroup_list.PageUrl %>start=<%= SiteCategoryGroup_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= SiteCategoryGroup_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= SiteCategoryGroup_list.Pager.FromIndex %> to <%= SiteCategoryGroup_list.Pager.ToIndex %> of <%= SiteCategoryGroup_list.Pager.RecordCount %></span>
<% Else %>
	<% If SiteCategoryGroup_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If SiteCategoryGroup_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="SiteCategoryGroup" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If SiteCategoryGroup_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If SiteCategoryGroup_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If SiteCategoryGroup_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If SiteCategoryGroup.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<% 'If SiteCategoryGroup_list.lTotalRecs > 0 Then %>
<div class="aspnetmaker">
<% If SiteCategoryGroup.CurrentAction <> "gridadd" AndAlso SiteCategoryGroup.CurrentAction <> "gridedit" Then ' Not grid add/edit mode %>
<a href="<%= SiteCategoryGroup.AddUrl %>">Add</a>&nbsp;&nbsp;
<a href="<%= SiteCategoryGroup_list.PageUrl %>a=gridadd">Grid Add</a>&nbsp;&nbsp;
<% If SiteCategoryGroup_list.lTotalRecs > 0 Then %>
<a href="<%= SiteCategoryGroup_list.PageUrl %>a=gridedit">Grid Edit</a>&nbsp;&nbsp;
<% End If %>
<% Else ' Grid add/edit mode %>
<% If SiteCategoryGroup.CurrentAction = "gridadd" Then %>
<a href="" onclick="f=ew_GetForm('fSiteCategoryGrouplist');if (SiteCategoryGroup_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/insert.gif' alt='Insert' title='Insert' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<% If SiteCategoryGroup.CurrentAction = "gridedit" Then %>
<a href="" onclick="f=ew_GetForm('fSiteCategoryGrouplist');if (SiteCategoryGroup_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/update.gif' alt='Save' title='Save' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<a href="<%= SiteCategoryGroup_list.PageUrl %>a=cancel"><img src='images/cancel.gif' alt='Cancel' title='Cancel' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
</div>
<% 'End If %>
</div>
<% End If %>
<% End If %>
</td></tr></table>
<% If SiteCategoryGroup.Export = "" AndAlso SiteCategoryGroup.CurrentAction = "" Then %>
<script type="text/javascript">
<!--
//ew_ToggleSearchPanel(SiteCategoryGroup_list); // uncomment to init search panel as collapsed
//-->
</script>
<% End If %>
<% If SiteCategoryGroup.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
