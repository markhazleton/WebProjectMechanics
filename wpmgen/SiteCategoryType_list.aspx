<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="SiteCategoryType_list.aspx.vb" Inherits="SiteCategoryType_list" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If SiteCategoryType.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var SiteCategoryType_list = new ew_Page("SiteCategoryType_list");
// page properties
SiteCategoryType_list.PageID = "list"; // page ID
var EW_PAGE_ID = SiteCategoryType_list.PageID; // for backward compatibility
// extend page with ValidateForm function
SiteCategoryType_list.ValidateForm = function(fobj) {
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
		} // End Grid Add checking
	}
	if (fobj.a_list && fobj.a_list.value == "gridinsert" && addcnt == 0) { // No row added
		alert("No records to be added");
		return false;
	}
	return true;
}
// Extend page with empty row check
SiteCategoryType_list.EmptyRow = function(fobj, infix) {
	if (ew_ValueChanged(fobj, infix, "SiteCategoryTypeNM")) return false;
	if (ew_ValueChanged(fobj, infix, "SiteCategoryFileName")) return false;
	if (ew_ValueChanged(fobj, infix, "SiteCategoryTransferURL")) return false;
	if (ew_ValueChanged(fobj, infix, "DefaultSiteCategoryID")) return false;
	return true;
}
<% If EW_CLIENT_VALIDATE Then %>
SiteCategoryType_list.ValidateRequired = true; // uses JavaScript validation
<% Else %>
SiteCategoryType_list.ValidateRequired = false; // no JavaScript validation
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
<% If SiteCategoryType.Export = "" Then %>
<% End If %>
<%
If SiteCategoryType.CurrentAction = "gridadd" Then SiteCategoryType.CurrentFilter = "0=1"

' Load recordset
Rs = SiteCategoryType_list.LoadRecordset()
If SiteCategoryType.CurrentAction = "gridadd" Then
	SiteCategoryType_list.lStartRec = 1
	If SiteCategoryType_list.lDisplayRecs <= 0 Then SiteCategoryType_list.lDisplayRecs = 25
	SiteCategoryType_list.lTotalRecs = SiteCategoryType_list.lDisplayRecs
	SiteCategoryType_list.lStopRec = SiteCategoryType_list.lDisplayRecs
Else
	SiteCategoryType_list.lStartRec = 1
	If SiteCategoryType_list.lDisplayRecs <= 0 Then ' Display all records
		SiteCategoryType_list.lDisplayRecs = SiteCategoryType_list.lTotalRecs
	End If
	If Not (SiteCategoryType.ExportAll AndAlso SiteCategoryType.Export <> "") Then
		SiteCategoryType_list.SetUpStartRec() ' Set up start record position
	End If
End If
%>
<p><span class="aspnetmaker" style="white-space: nowrap;">TABLE: Site Type
<% If SiteCategoryType.Export = "" AndAlso SiteCategoryType.CurrentAction = "" Then %>
&nbsp;&nbsp;<a href="<%= SiteCategoryType_list.PageUrl %>export=excel"><img src='images/exportxls.gif' alt='Export to Excel' title='Export to Excel' width='16' height='16' border='0'></a>
<% End If %>
</span></p>
<% If SiteCategoryType.Export = "" AndAlso SiteCategoryType.CurrentAction = "" Then %>
<a href="javascript:ew_ToggleSearchPanel(SiteCategoryType_list);" style="text-decoration: none;"><img id="SiteCategoryType_list_SearchImage" src="images/collapse.gif" alt="" width="9" height="9" border="0"></a><span class="aspnetmaker">&nbsp;Search</span><br>
<div id="SiteCategoryType_list_SearchPanel">
<form name="fSiteCategoryTypelistsrch" id="fSiteCategoryTypelistsrch" class="ewForm">
<input type="hidden" id="t" name="t" value="SiteCategoryType" />
<table class="ewBasicSearch">
	<tr>
		<td><span class="aspnetmaker">
			<input type="text" name="<%= EW_TABLE_BASIC_SEARCH %>" id="<%= EW_TABLE_BASIC_SEARCH %>" size="20" value="<%= ew_HtmlEncode(SiteCategoryType.BasicSearchKeyword) %>" />
			<input type="Submit" name="Submit" id="Submit" value="Search (*)" />&nbsp;
			<a href="<%= SiteCategoryType_list.PageUrl %>cmd=reset">Show all</a>&nbsp;
			<a href="SiteCategoryType_srch.aspx">Advanced Search</a>&nbsp;
		</span></td>
	</tr>
	<tr>
	<td><span class="aspnetmaker"><label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value=""<% If SiteCategoryType.BasicSearchType = "" Then %> checked="checked"<% End If %> />Exact phrase</label>&nbsp;&nbsp;<label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value="AND"<% If SiteCategoryType.BasicSearchType = "AND" Then %> checked="checked"<% End If %> />All words</label>&nbsp;&nbsp;<label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value="OR"<% If SiteCategoryType.BasicSearchType = "OR" Then %> checked="checked"<% End If %> />Any word</label></span></td>
	</tr>
</table>
</form>
</div>
<% End If %>
<% SiteCategoryType_list.ShowMessage() %>
<br />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<% If SiteCategoryType.Export = "" Then %>
<div class="ewGridUpperPanel">
<% If SiteCategoryType.CurrentAction <> "gridadd" AndAlso SiteCategoryType.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If SiteCategoryType_list.Pager Is Nothing Then SiteCategoryType_list.Pager = New cPrevNextPager(SiteCategoryType_list.lStartRec, SiteCategoryType_list.lDisplayRecs, SiteCategoryType_list.lTotalRecs) %>
<% If SiteCategoryType_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If SiteCategoryType_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= SiteCategoryType_list.PageUrl %>start=<%= SiteCategoryType_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If SiteCategoryType_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= SiteCategoryType_list.PageUrl %>start=<%= SiteCategoryType_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= SiteCategoryType_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If SiteCategoryType_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= SiteCategoryType_list.PageUrl %>start=<%= SiteCategoryType_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If SiteCategoryType_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= SiteCategoryType_list.PageUrl %>start=<%= SiteCategoryType_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= SiteCategoryType_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= SiteCategoryType_list.Pager.FromIndex %> to <%= SiteCategoryType_list.Pager.ToIndex %> of <%= SiteCategoryType_list.Pager.RecordCount %></span>
<% Else %>
	<% If SiteCategoryType_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If SiteCategoryType_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="SiteCategoryType" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If SiteCategoryType_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If SiteCategoryType_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If SiteCategoryType_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If SiteCategoryType.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<div class="aspnetmaker">
<% If SiteCategoryType.CurrentAction <> "gridadd" AndAlso SiteCategoryType.CurrentAction <> "gridedit" Then ' Not grid add/edit mode %>
<a href="<%= SiteCategoryType.AddUrl %>">Add</a>&nbsp;&nbsp;
<a href="<%= SiteCategoryType_list.PageUrl %>a=gridadd">Grid Add</a>&nbsp;&nbsp;
<% If SiteCategoryType_list.lTotalRecs > 0 Then %>
<a href="<%= SiteCategoryType_list.PageUrl %>a=gridedit">Grid Edit</a>&nbsp;&nbsp;
<% End If %>
<% Else ' Grid add/edit mode %>
<% If SiteCategoryType.CurrentAction = "gridadd" Then %>
<a href="" onclick="f=ew_GetForm('fSiteCategoryTypelist');if (SiteCategoryType_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/insert.gif' alt='Insert' title='Insert' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<% If SiteCategoryType.CurrentAction = "gridedit" Then %>
<a href="" onclick="f=ew_GetForm('fSiteCategoryTypelist');if (SiteCategoryType_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/update.gif' alt='Save' title='Save' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<a href="<%= SiteCategoryType_list.PageUrl %>a=cancel"><img src='images/cancel.gif' alt='Cancel' title='Cancel' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
</div>
</div>
<% End If %>
<div class="ewGridMiddlePanel">
<form name="fSiteCategoryTypelist" id="fSiteCategoryTypelist" class="ewForm" method="post">
<input type="hidden" name="t" id="t" value="SiteCategoryType" />
<% If SiteCategoryType_list.lTotalRecs > 0 Then %>
<table cellspacing="0" rowhighlightclass="ewTableHighlightRow" rowselectclass="ewTableSelectRow" roweditclass="ewTableEditRow" class="ewTable ewTableSeparate">
<%
	SiteCategoryType_list.lOptionCnt = 0
	SiteCategoryType_list.lOptionCnt = SiteCategoryType_list.lOptionCnt + 1 ' View
	SiteCategoryType_list.lOptionCnt = SiteCategoryType_list.lOptionCnt + 1 ' Edit
	SiteCategoryType_list.lOptionCnt = SiteCategoryType_list.lOptionCnt + 1 ' Copy
	SiteCategoryType_list.lOptionCnt = SiteCategoryType_list.lOptionCnt + SiteCategoryType_list.ListOptions.Items.Count ' Custom list options
%>
<%= SiteCategoryType.TableCustomInnerHTML %>
<thead><!-- Table header -->
	<tr class="ewTableHeader">
<% If SiteCategoryType.Export = "" Then %>
<% If SiteCategoryType.CurrentAction <> "gridadd" AndAlso SiteCategoryType.CurrentAction <> "gridedit" Then %>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<%

' Custom list options
For i As Integer = 0 to SiteCategoryType_list.ListOptions.Items.Count -1
	If SiteCategoryType_list.ListOptions.Items(i).Visible Then Response.Write(SiteCategoryType_list.ListOptions.Items(i).HeaderCellHtml)
Next
%>
<% End If %>
<% End If %>
<% If SiteCategoryType.SiteCategoryTypeID.Visible Then ' SiteCategoryTypeID %>
	<% If SiteCategoryType.SortUrl(SiteCategoryType.SiteCategoryTypeID) = "" Then %>
		<td style="white-space: nowrap;">Site Category Type ID</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteCategoryType.SortUrl(SiteCategoryType.SiteCategoryTypeID) %>',1);" style="white-space: nowrap;">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Site Category Type ID</td><td style="width: 10px;"><% If SiteCategoryType.SiteCategoryTypeID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteCategoryType.SiteCategoryTypeID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If SiteCategoryType.SiteCategoryTypeNM.Visible Then ' SiteCategoryTypeNM %>
	<% If SiteCategoryType.SortUrl(SiteCategoryType.SiteCategoryTypeNM) = "" Then %>
		<td style="white-space: nowrap;">Site Type</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteCategoryType.SortUrl(SiteCategoryType.SiteCategoryTypeNM) %>',1);" style="white-space: nowrap;">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Site Type&nbsp;(*)</td><td style="width: 10px;"><% If SiteCategoryType.SiteCategoryTypeNM.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteCategoryType.SiteCategoryTypeNM.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If SiteCategoryType.SiteCategoryFileName.Visible Then ' SiteCategoryFileName %>
	<% If SiteCategoryType.SortUrl(SiteCategoryType.SiteCategoryFileName) = "" Then %>
		<td style="white-space: nowrap;">File Name</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteCategoryType.SortUrl(SiteCategoryType.SiteCategoryFileName) %>',1);" style="white-space: nowrap;">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>File Name&nbsp;(*)</td><td style="width: 10px;"><% If SiteCategoryType.SiteCategoryFileName.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteCategoryType.SiteCategoryFileName.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If SiteCategoryType.SiteCategoryTransferURL.Visible Then ' SiteCategoryTransferURL %>
	<% If SiteCategoryType.SortUrl(SiteCategoryType.SiteCategoryTransferURL) = "" Then %>
		<td style="white-space: nowrap;">Transfer URL</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteCategoryType.SortUrl(SiteCategoryType.SiteCategoryTransferURL) %>',1);" style="white-space: nowrap;">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Transfer URL&nbsp;(*)</td><td style="width: 10px;"><% If SiteCategoryType.SiteCategoryTransferURL.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteCategoryType.SiteCategoryTransferURL.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If SiteCategoryType.DefaultSiteCategoryID.Visible Then ' DefaultSiteCategoryID %>
	<% If SiteCategoryType.SortUrl(SiteCategoryType.DefaultSiteCategoryID) = "" Then %>
		<td style="white-space: nowrap;">Default Category</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteCategoryType.SortUrl(SiteCategoryType.DefaultSiteCategoryID) %>',1);" style="white-space: nowrap;">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Default Category</td><td style="width: 10px;"><% If SiteCategoryType.DefaultSiteCategoryID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteCategoryType.DefaultSiteCategoryID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
	</tr>
</thead>
<tbody><!-- Table body -->
<%
If (SiteCategoryType.ExportAll AndAlso SiteCategoryType.Export <> "") Then
	SiteCategoryType_list.lStopRec = SiteCategoryType_list.lTotalRecs
Else
	SiteCategoryType_list.lStopRec = SiteCategoryType_list.lStartRec + SiteCategoryType_list.lDisplayRecs - 1 ' Set the last record to display
End If
If SiteCategoryType.CurrentAction = "gridadd" AndAlso SiteCategoryType_list.lStopRec = -1 Then
	SiteCategoryType_list.lStopRec = EW_GRIDADD_ROWS
End If 

' Move to first record
For i As Integer = 1 to SiteCategoryType_list.lStartRec - 1
	If Rs.Read() Then	SiteCategoryType_list.lRecCnt = SiteCategoryType_list.lRecCnt + 1
Next		
SiteCategoryType_list.lRowCnt = 0
If SiteCategoryType.CurrentAction = "gridadd" Then SiteCategoryType_list.lRowIndex = 0
If SiteCategoryType.CurrentAction = "gridedit" Then SiteCategoryType_list.lRowIndex = 0

' Output data rows
Do While (SiteCategoryType.CurrentAction = "gridadd" OrElse Rs.Read()) AndAlso (SiteCategoryType_list.lRecCnt < SiteCategoryType_list.lStopRec)
	SiteCategoryType_list.lRecCnt = SiteCategoryType_list.lRecCnt + 1
	If SiteCategoryType_list.lRecCnt >= SiteCategoryType_list.lStartRec Then
		SiteCategoryType_list.lRowCnt = SiteCategoryType_list.lRowCnt + 1
		If SiteCategoryType.CurrentAction = "gridadd" OrElse SiteCategoryType.CurrentAction = "gridedit" Then SiteCategoryType_list.lRowIndex = SiteCategoryType_list.lRowIndex + 1
	SiteCategoryType.CssClass = ""
	SiteCategoryType.CssStyle = ""
	SiteCategoryType.RowClientEvents = "onmouseover='ew_MouseOver(event, this);' onmouseout='ew_MouseOut(event, this);' onclick='ew_Click(event, this);'"
	If SiteCategoryType.CurrentAction = "gridadd" Then
		SiteCategoryType_list.LoadDefaultValues() ' Load default values
	Else
		SiteCategoryType_list.LoadRowValues(Rs) ' Load row values
	End If
	SiteCategoryType.RowType = EW_ROWTYPE_VIEW ' Render view
	If SiteCategoryType.CurrentAction = "gridadd" Then ' Grid add
		SiteCategoryType.RowType = EW_ROWTYPE_ADD ' Render add
	End If
	If SiteCategoryType.CurrentAction = "gridadd" AndAlso SiteCategoryType.EventCancelled Then ' Insert failed
		SiteCategoryType_list.RestoreCurrentRowFormValues(SiteCategoryType_list.lRowIndex) ' Restore form values
	End If
	If SiteCategoryType.CurrentAction = "gridedit" Then ' Grid edit
		SiteCategoryType.RowType = EW_ROWTYPE_EDIT ' Render edit
	End If
	If SiteCategoryType.RowType = EW_ROWTYPE_EDIT AndAlso SiteCategoryType.EventCancelled Then ' update failed
		If SiteCategoryType.CurrentAction = "gridedit" Then
			SiteCategoryType_list.RestoreCurrentRowFormValues(SiteCategoryType_list.lRowIndex) ' Restore form values
		End If
	End If
	If SiteCategoryType.RowType = EW_ROWTYPE_EDIT Then ' Edit row
		SiteCategoryType_list.lEditRowCnt = SiteCategoryType_list.lEditRowCnt + 1
		SiteCategoryType.RowClientEvents = "onmouseover='this.edit=true;ew_MouseOver(event, this);' onmouseout='ew_MouseOut(event, this);' onclick='ew_Click(event, this);'"
	End If
	If SiteCategoryType.RowType = EW_ROWTYPE_ADD OrElse SiteCategoryType.RowType = EW_ROWTYPE_EDIT Then ' Add / Edit row
		SiteCategoryType.CssClass = "ewTableEditRow"
	End If

	' Render row
	SiteCategoryType_list.RenderRow()
%>
	<tr<%= SiteCategoryType.RowAttributes %>>
<% If SiteCategoryType.RowType = EW_ROWTYPE_ADD OrElse SiteCategoryType.RowType = EW_ROWTYPE_EDIT Then %>
<%
	If SiteCategoryType.CurrentAction = "gridedit" Then
		SiteCategoryType_list.sMultiSelectKey = SiteCategoryType_list.sMultiSelectKey & "<input type=""hidden"" name=""k" & SiteCategoryType_list.lRowIndex & "_key"" id=""k" & SiteCategoryType_list.lRowIndex & "_key"" value=""" & SiteCategoryType.SiteCategoryTypeID.CurrentValue & """ />"
	End If
%>
<% Else %>
<% If SiteCategoryType.Export = "" Then %>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= SiteCategoryType.ViewUrl %>"><img src='images/view.gif' alt='View' title='View' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= SiteCategoryType.EditUrl %>"><img src='images/edit.gif' alt='Edit' title='Edit' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= SiteCategoryType.CopyUrl %>"><img src='images/copy.gif' alt='Copy' title='Copy' width='16' height='16' border='0'></a>
</span></td>
<%

' Custom list options
For i As Integer = 0 to SiteCategoryType_list.ListOptions.Items.Count -1
	If SiteCategoryType_list.ListOptions.Items(i).Visible Then Response.Write(SiteCategoryType_list.ListOptions.Items(i).BodyCellHtml)
Next
%>
<% End If %>
<% End If %>
	<% If SiteCategoryType.SiteCategoryTypeID.Visible Then ' SiteCategoryTypeID %>
		<td<%= SiteCategoryType.SiteCategoryTypeID.CellAttributes %>>
<% If SiteCategoryType.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<input type="hidden" name="o<%= SiteCategoryType_list.lRowIndex %>_SiteCategoryTypeID" id="o<%= SiteCategoryType_list.lRowIndex %>_SiteCategoryTypeID" value="<%= ew_HTMLEncode(SiteCategoryType.SiteCategoryTypeID.OldValue) %>" />
<% End If %>
<% If SiteCategoryType.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<input type="hidden" name="x<%= SiteCategoryType_list.lRowIndex %>_SiteCategoryTypeID" id="x<%= SiteCategoryType_list.lRowIndex %>_SiteCategoryTypeID" value="<%= ew_HTMLEncode(SiteCategoryType.SiteCategoryTypeID.CurrentValue) %>" />
<% End If %>
<% If SiteCategoryType.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= SiteCategoryType.SiteCategoryTypeID.ViewAttributes %>><%= SiteCategoryType.SiteCategoryTypeID.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	<% If SiteCategoryType.SiteCategoryTypeNM.Visible Then ' SiteCategoryTypeNM %>
		<td<%= SiteCategoryType.SiteCategoryTypeNM.CellAttributes %>>
<% If SiteCategoryType.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<input type="text" name="x<%= SiteCategoryType_list.lRowIndex %>_SiteCategoryTypeNM" id="x<%= SiteCategoryType_list.lRowIndex %>_SiteCategoryTypeNM" size="30" maxlength="255" value="<%= SiteCategoryType.SiteCategoryTypeNM.EditValue %>"<%= SiteCategoryType.SiteCategoryTypeNM.EditAttributes %> />
<input type="hidden" name="o<%= SiteCategoryType_list.lRowIndex %>_SiteCategoryTypeNM" id="o<%= SiteCategoryType_list.lRowIndex %>_SiteCategoryTypeNM" value="<%= ew_HTMLEncode(SiteCategoryType.SiteCategoryTypeNM.OldValue) %>" />
<% End If %>
<% If SiteCategoryType.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<input type="text" name="x<%= SiteCategoryType_list.lRowIndex %>_SiteCategoryTypeNM" id="x<%= SiteCategoryType_list.lRowIndex %>_SiteCategoryTypeNM" size="30" maxlength="255" value="<%= SiteCategoryType.SiteCategoryTypeNM.EditValue %>"<%= SiteCategoryType.SiteCategoryTypeNM.EditAttributes %> />
<% End If %>
<% If SiteCategoryType.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= SiteCategoryType.SiteCategoryTypeNM.ViewAttributes %>><%= SiteCategoryType.SiteCategoryTypeNM.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	<% If SiteCategoryType.SiteCategoryFileName.Visible Then ' SiteCategoryFileName %>
		<td<%= SiteCategoryType.SiteCategoryFileName.CellAttributes %>>
<% If SiteCategoryType.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<input type="text" name="x<%= SiteCategoryType_list.lRowIndex %>_SiteCategoryFileName" id="x<%= SiteCategoryType_list.lRowIndex %>_SiteCategoryFileName" size="30" maxlength="255" value="<%= SiteCategoryType.SiteCategoryFileName.EditValue %>"<%= SiteCategoryType.SiteCategoryFileName.EditAttributes %> />
<input type="hidden" name="o<%= SiteCategoryType_list.lRowIndex %>_SiteCategoryFileName" id="o<%= SiteCategoryType_list.lRowIndex %>_SiteCategoryFileName" value="<%= ew_HTMLEncode(SiteCategoryType.SiteCategoryFileName.OldValue) %>" />
<% End If %>
<% If SiteCategoryType.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<input type="text" name="x<%= SiteCategoryType_list.lRowIndex %>_SiteCategoryFileName" id="x<%= SiteCategoryType_list.lRowIndex %>_SiteCategoryFileName" size="30" maxlength="255" value="<%= SiteCategoryType.SiteCategoryFileName.EditValue %>"<%= SiteCategoryType.SiteCategoryFileName.EditAttributes %> />
<% End If %>
<% If SiteCategoryType.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= SiteCategoryType.SiteCategoryFileName.ViewAttributes %>><%= SiteCategoryType.SiteCategoryFileName.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	<% If SiteCategoryType.SiteCategoryTransferURL.Visible Then ' SiteCategoryTransferURL %>
		<td<%= SiteCategoryType.SiteCategoryTransferURL.CellAttributes %>>
<% If SiteCategoryType.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<input type="text" name="x<%= SiteCategoryType_list.lRowIndex %>_SiteCategoryTransferURL" id="x<%= SiteCategoryType_list.lRowIndex %>_SiteCategoryTransferURL" size="30" maxlength="255" value="<%= SiteCategoryType.SiteCategoryTransferURL.EditValue %>"<%= SiteCategoryType.SiteCategoryTransferURL.EditAttributes %> />
<input type="hidden" name="o<%= SiteCategoryType_list.lRowIndex %>_SiteCategoryTransferURL" id="o<%= SiteCategoryType_list.lRowIndex %>_SiteCategoryTransferURL" value="<%= ew_HTMLEncode(SiteCategoryType.SiteCategoryTransferURL.OldValue) %>" />
<% End If %>
<% If SiteCategoryType.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<input type="text" name="x<%= SiteCategoryType_list.lRowIndex %>_SiteCategoryTransferURL" id="x<%= SiteCategoryType_list.lRowIndex %>_SiteCategoryTransferURL" size="30" maxlength="255" value="<%= SiteCategoryType.SiteCategoryTransferURL.EditValue %>"<%= SiteCategoryType.SiteCategoryTransferURL.EditAttributes %> />
<% End If %>
<% If SiteCategoryType.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= SiteCategoryType.SiteCategoryTransferURL.ViewAttributes %>><%= SiteCategoryType.SiteCategoryTransferURL.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	<% If SiteCategoryType.DefaultSiteCategoryID.Visible Then ' DefaultSiteCategoryID %>
		<td<%= SiteCategoryType.DefaultSiteCategoryID.CellAttributes %>>
<% If SiteCategoryType.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<select id="x<%= SiteCategoryType_list.lRowIndex %>_DefaultSiteCategoryID" name="x<%= SiteCategoryType_list.lRowIndex %>_DefaultSiteCategoryID"<%= SiteCategoryType.DefaultSiteCategoryID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(SiteCategoryType.DefaultSiteCategoryID.EditValue) Then
	arwrk = SiteCategoryType.DefaultSiteCategoryID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), SiteCategoryType.DefaultSiteCategoryID.CurrentValue) Then
			selwrk = " selected=""selected"""
			emptywrk = False
		Else
			selwrk = ""
		End If
%>
<option value="<%= ew_HtmlEncode(arwrk(rowcntwrk)(0)) %>"<%= selwrk %>>
<%= arwrk(rowcntwrk)(1) %>
<% If ew_NotEmpty(arwrk(rowcntwrk)(2)) Then %>
<%= ew_ValueSeparator(rowcntwrk) %><%= arwrk(rowcntwrk)(2) %>
<% End If %>
</option>
<%
	Next
End If
If emptywrk Then SiteCategoryType.DefaultSiteCategoryID.OldValue = ""
%>
</select>
<%
sSqlWrk = "SELECT [SiteCategoryID], [CategoryName], [SiteCategoryTypeID] FROM [SiteCategory]"
sWhereWrk = ""
If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryTypeID] Asc"
sSqlWrk = cTEA.Encrypt(sSqlWrk, EW_RANDOM_KEY)
%>
<input type="hidden" name="s_x<%= SiteCategoryType_list.lRowIndex %>_DefaultSiteCategoryID" id="s_x<%= SiteCategoryType_list.lRowIndex %>_DefaultSiteCategoryID" value="<%= sSqlWrk %>">
<input type="hidden" name="lft_x<%= SiteCategoryType_list.lRowIndex %>_DefaultSiteCategoryID" id="lft_x<%= SiteCategoryType_list.lRowIndex %>_DefaultSiteCategoryID" value="">
<input type="hidden" name="o<%= SiteCategoryType_list.lRowIndex %>_DefaultSiteCategoryID" id="o<%= SiteCategoryType_list.lRowIndex %>_DefaultSiteCategoryID" value="<%= ew_HTMLEncode(SiteCategoryType.DefaultSiteCategoryID.OldValue) %>" />
<% End If %>
<% If SiteCategoryType.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<select id="x<%= SiteCategoryType_list.lRowIndex %>_DefaultSiteCategoryID" name="x<%= SiteCategoryType_list.lRowIndex %>_DefaultSiteCategoryID"<%= SiteCategoryType.DefaultSiteCategoryID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(SiteCategoryType.DefaultSiteCategoryID.EditValue) Then
	arwrk = SiteCategoryType.DefaultSiteCategoryID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), SiteCategoryType.DefaultSiteCategoryID.CurrentValue) Then
			selwrk = " selected=""selected"""
			emptywrk = False
		Else
			selwrk = ""
		End If
%>
<option value="<%= ew_HtmlEncode(arwrk(rowcntwrk)(0)) %>"<%= selwrk %>>
<%= arwrk(rowcntwrk)(1) %>
<% If ew_NotEmpty(arwrk(rowcntwrk)(2)) Then %>
<%= ew_ValueSeparator(rowcntwrk) %><%= arwrk(rowcntwrk)(2) %>
<% End If %>
</option>
<%
	Next
End If
If emptywrk Then SiteCategoryType.DefaultSiteCategoryID.OldValue = ""
%>
</select>
<%
sSqlWrk = "SELECT [SiteCategoryID], [CategoryName], [SiteCategoryTypeID] FROM [SiteCategory]"
sWhereWrk = ""
If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryTypeID] Asc"
sSqlWrk = cTEA.Encrypt(sSqlWrk, EW_RANDOM_KEY)
%>
<input type="hidden" name="s_x<%= SiteCategoryType_list.lRowIndex %>_DefaultSiteCategoryID" id="s_x<%= SiteCategoryType_list.lRowIndex %>_DefaultSiteCategoryID" value="<%= sSqlWrk %>">
<input type="hidden" name="lft_x<%= SiteCategoryType_list.lRowIndex %>_DefaultSiteCategoryID" id="lft_x<%= SiteCategoryType_list.lRowIndex %>_DefaultSiteCategoryID" value="">
<% End If %>
<% If SiteCategoryType.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= SiteCategoryType.DefaultSiteCategoryID.ViewAttributes %>><%= SiteCategoryType.DefaultSiteCategoryID.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	</tr>
<% If SiteCategoryType.RowType = EW_ROWTYPE_ADD Then %>
<script language="JavaScript">
<!--
ew_UpdateOpts([['x<%= SiteCategoryType_list.lRowIndex %>_DefaultSiteCategoryID','x<%= SiteCategoryType_list.lRowIndex %>_DefaultSiteCategoryID',false]]);
//-->
</script>
<% End If %>
<% If SiteCategoryType.RowType = EW_ROWTYPE_EDIT Then %>
<script language="JavaScript">
<!--
ew_UpdateOpts([['x<%= SiteCategoryType_list.lRowIndex %>_DefaultSiteCategoryID','x<%= SiteCategoryType_list.lRowIndex %>_DefaultSiteCategoryID',false]]);
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
<% If SiteCategoryType.CurrentAction = "gridadd" Then %>
<input type="hidden" name="a_list" id="a_list" value="gridinsert" />
<input type="hidden" name="key_count" id="key_count" value="<%= SiteCategoryType_list.lRowIndex %>" />
<% End If %>
<% If SiteCategoryType.CurrentAction = "gridedit" Then %>
<input type="hidden" name="a_list" id="a_list" value="gridupdate" />
<input type="hidden" name="key_count" id="key_count" value="<%= SiteCategoryType_list.lRowIndex %>" />
<%= SiteCategoryType_list.sMultiSelectKey %>
<% End If %>
</form>
<%

' Close recordset
Rs.Close()
Rs.Dispose()
%>
</div>
<% If SiteCategoryType_list.lTotalRecs > 0 Then %>
<% If SiteCategoryType.Export = "" Then %>
<div class="ewGridLowerPanel">
<% If SiteCategoryType.CurrentAction <> "gridadd" AndAlso SiteCategoryType.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If SiteCategoryType_list.Pager Is Nothing Then SiteCategoryType_list.Pager = New cPrevNextPager(SiteCategoryType_list.lStartRec, SiteCategoryType_list.lDisplayRecs, SiteCategoryType_list.lTotalRecs) %>
<% If SiteCategoryType_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If SiteCategoryType_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= SiteCategoryType_list.PageUrl %>start=<%= SiteCategoryType_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If SiteCategoryType_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= SiteCategoryType_list.PageUrl %>start=<%= SiteCategoryType_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= SiteCategoryType_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If SiteCategoryType_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= SiteCategoryType_list.PageUrl %>start=<%= SiteCategoryType_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If SiteCategoryType_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= SiteCategoryType_list.PageUrl %>start=<%= SiteCategoryType_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= SiteCategoryType_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= SiteCategoryType_list.Pager.FromIndex %> to <%= SiteCategoryType_list.Pager.ToIndex %> of <%= SiteCategoryType_list.Pager.RecordCount %></span>
<% Else %>
	<% If SiteCategoryType_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If SiteCategoryType_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="SiteCategoryType" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If SiteCategoryType_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If SiteCategoryType_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If SiteCategoryType_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If SiteCategoryType.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<% 'If SiteCategoryType_list.lTotalRecs > 0 Then %>
<div class="aspnetmaker">
<% If SiteCategoryType.CurrentAction <> "gridadd" AndAlso SiteCategoryType.CurrentAction <> "gridedit" Then ' Not grid add/edit mode %>
<a href="<%= SiteCategoryType.AddUrl %>">Add</a>&nbsp;&nbsp;
<a href="<%= SiteCategoryType_list.PageUrl %>a=gridadd">Grid Add</a>&nbsp;&nbsp;
<% If SiteCategoryType_list.lTotalRecs > 0 Then %>
<a href="<%= SiteCategoryType_list.PageUrl %>a=gridedit">Grid Edit</a>&nbsp;&nbsp;
<% End If %>
<% Else ' Grid add/edit mode %>
<% If SiteCategoryType.CurrentAction = "gridadd" Then %>
<a href="" onclick="f=ew_GetForm('fSiteCategoryTypelist');if (SiteCategoryType_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/insert.gif' alt='Insert' title='Insert' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<% If SiteCategoryType.CurrentAction = "gridedit" Then %>
<a href="" onclick="f=ew_GetForm('fSiteCategoryTypelist');if (SiteCategoryType_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/update.gif' alt='Save' title='Save' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<a href="<%= SiteCategoryType_list.PageUrl %>a=cancel"><img src='images/cancel.gif' alt='Cancel' title='Cancel' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
</div>
<% 'End If %>
</div>
<% End If %>
<% End If %>
</td></tr></table>
<% If SiteCategoryType.Export = "" AndAlso SiteCategoryType.CurrentAction = "" Then %>
<script type="text/javascript">
<!--
//ew_ToggleSearchPanel(SiteCategoryType_list); // uncomment to init search panel as collapsed
//-->
</script>
<% End If %>
<% If SiteCategoryType.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
