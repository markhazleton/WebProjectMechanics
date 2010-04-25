<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="PageAlias_list.aspx.vb" Inherits="PageAlias_list" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If PageAlias.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var PageAlias_list = new ew_Page("PageAlias_list");
// page properties
PageAlias_list.PageID = "list"; // page ID
var EW_PAGE_ID = PageAlias_list.PageID; // for backward compatibility
// extend page with ValidateForm function
PageAlias_list.ValidateForm = function(fobj) {
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
PageAlias_list.EmptyRow = function(fobj, infix) {
	if (ew_ValueChanged(fobj, infix, "zPageURL")) return false;
	if (ew_ValueChanged(fobj, infix, "TargetURL")) return false;
	if (ew_ValueChanged(fobj, infix, "AliasType")) return false;
	if (ew_ValueChanged(fobj, infix, "CompanyID")) return false;
	return true;
}
<% If EW_CLIENT_VALIDATE Then %>
PageAlias_list.ValidateRequired = true; // uses JavaScript validation
<% Else %>
PageAlias_list.ValidateRequired = false; // no JavaScript validation
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
<% If PageAlias.Export = "" Then %>
<% End If %>
<%
If PageAlias.CurrentAction = "gridadd" Then PageAlias.CurrentFilter = "0=1"

' Load recordset
Rs = PageAlias_list.LoadRecordset()
If PageAlias.CurrentAction = "gridadd" Then
	PageAlias_list.lStartRec = 1
	If PageAlias_list.lDisplayRecs <= 0 Then PageAlias_list.lDisplayRecs = 25
	PageAlias_list.lTotalRecs = PageAlias_list.lDisplayRecs
	PageAlias_list.lStopRec = PageAlias_list.lDisplayRecs
Else
	PageAlias_list.lStartRec = 1
	If PageAlias_list.lDisplayRecs <= 0 Then ' Display all records
		PageAlias_list.lDisplayRecs = PageAlias_list.lTotalRecs
	End If
	If Not (PageAlias.ExportAll AndAlso PageAlias.Export <> "") Then
		PageAlias_list.SetUpStartRec() ' Set up start record position
	End If
End If
%>
<p><span class="aspnetmaker" style="white-space: nowrap;">TABLE: Location Alias
<% If PageAlias.Export = "" AndAlso PageAlias.CurrentAction = "" Then %>
&nbsp;&nbsp;<a href="<%= PageAlias_list.PageUrl %>export=excel"><img src='images/exportxls.gif' alt='Export to Excel' title='Export to Excel' width='16' height='16' border='0'></a>
<% End If %>
</span></p>
<% If PageAlias.Export = "" AndAlso PageAlias.CurrentAction = "" Then %>
<a href="javascript:ew_ToggleSearchPanel(PageAlias_list);" style="text-decoration: none;"><img id="PageAlias_list_SearchImage" src="images/collapse.gif" alt="" width="9" height="9" border="0"></a><span class="aspnetmaker">&nbsp;Search</span><br>
<div id="PageAlias_list_SearchPanel">
<form name="fPageAliaslistsrch" id="fPageAliaslistsrch" class="ewForm">
<input type="hidden" id="t" name="t" value="PageAlias" />
<table class="ewBasicSearch">
	<tr>
		<td><span class="aspnetmaker">
			<input type="text" name="<%= EW_TABLE_BASIC_SEARCH %>" id="<%= EW_TABLE_BASIC_SEARCH %>" size="20" value="<%= ew_HtmlEncode(PageAlias.BasicSearchKeyword) %>" />
			<input type="Submit" name="Submit" id="Submit" value="Search (*)" />&nbsp;
			<a href="<%= PageAlias_list.PageUrl %>cmd=reset">Show all</a>&nbsp;
			<a href="PageAlias_srch.aspx">Advanced Search</a>&nbsp;
		</span></td>
	</tr>
	<tr>
	<td><span class="aspnetmaker"><label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value=""<% If PageAlias.BasicSearchType = "" Then %> checked="checked"<% End If %> />Exact phrase</label>&nbsp;&nbsp;<label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value="AND"<% If PageAlias.BasicSearchType = "AND" Then %> checked="checked"<% End If %> />All words</label>&nbsp;&nbsp;<label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value="OR"<% If PageAlias.BasicSearchType = "OR" Then %> checked="checked"<% End If %> />Any word</label></span></td>
	</tr>
</table>
</form>
</div>
<% End If %>
<% PageAlias_list.ShowMessage() %>
<br />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<% If PageAlias.Export = "" Then %>
<div class="ewGridUpperPanel">
<% If PageAlias.CurrentAction <> "gridadd" AndAlso PageAlias.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If PageAlias_list.Pager Is Nothing Then PageAlias_list.Pager = New cPrevNextPager(PageAlias_list.lStartRec, PageAlias_list.lDisplayRecs, PageAlias_list.lTotalRecs) %>
<% If PageAlias_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If PageAlias_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= PageAlias_list.PageUrl %>start=<%= PageAlias_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If PageAlias_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= PageAlias_list.PageUrl %>start=<%= PageAlias_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= PageAlias_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If PageAlias_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= PageAlias_list.PageUrl %>start=<%= PageAlias_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If PageAlias_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= PageAlias_list.PageUrl %>start=<%= PageAlias_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= PageAlias_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= PageAlias_list.Pager.FromIndex %> to <%= PageAlias_list.Pager.ToIndex %> of <%= PageAlias_list.Pager.RecordCount %></span>
<% Else %>
	<% If PageAlias_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If PageAlias_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="PageAlias" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If PageAlias_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If PageAlias_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If PageAlias_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If PageAlias.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<div class="aspnetmaker">
<% If PageAlias.CurrentAction <> "gridadd" AndAlso PageAlias.CurrentAction <> "gridedit" Then ' Not grid add/edit mode %>
<a href="<%= PageAlias.AddUrl %>">Add</a>&nbsp;&nbsp;
<a href="<%= PageAlias_list.PageUrl %>a=gridadd">Grid Add</a>&nbsp;&nbsp;
<% If PageAlias_list.lTotalRecs > 0 Then %>
<a href="<%= PageAlias_list.PageUrl %>a=gridedit">Grid Edit</a>&nbsp;&nbsp;
<% End If %>
<% Else ' Grid add/edit mode %>
<% If PageAlias.CurrentAction = "gridadd" Then %>
<a href="" onclick="f=ew_GetForm('fPageAliaslist');if (PageAlias_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/insert.gif' alt='Insert' title='Insert' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<% If PageAlias.CurrentAction = "gridedit" Then %>
<a href="" onclick="f=ew_GetForm('fPageAliaslist');if (PageAlias_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/update.gif' alt='Save' title='Save' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<a href="<%= PageAlias_list.PageUrl %>a=cancel"><img src='images/cancel.gif' alt='Cancel' title='Cancel' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
</div>
</div>
<% End If %>
<div class="ewGridMiddlePanel">
<form name="fPageAliaslist" id="fPageAliaslist" class="ewForm" method="post">
<input type="hidden" name="t" id="t" value="PageAlias" />
<% If PageAlias_list.lTotalRecs > 0 Then %>
<table cellspacing="0" rowhighlightclass="ewTableHighlightRow" rowselectclass="ewTableSelectRow" roweditclass="ewTableEditRow" class="ewTable ewTableSeparate">
<%
	PageAlias_list.lOptionCnt = 0
	PageAlias_list.lOptionCnt = PageAlias_list.lOptionCnt + 1 ' View
	PageAlias_list.lOptionCnt = PageAlias_list.lOptionCnt + 1 ' Edit
	PageAlias_list.lOptionCnt = PageAlias_list.lOptionCnt + 1 ' Copy
	PageAlias_list.lOptionCnt = PageAlias_list.lOptionCnt + 1 ' Delete
	PageAlias_list.lOptionCnt = PageAlias_list.lOptionCnt + PageAlias_list.ListOptions.Items.Count ' Custom list options
%>
<%= PageAlias.TableCustomInnerHTML %>
<thead><!-- Table header -->
	<tr class="ewTableHeader">
<% If PageAlias.Export = "" Then %>
<% If PageAlias.CurrentAction <> "gridadd" AndAlso PageAlias.CurrentAction <> "gridedit" Then %>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<%

' Custom list options
For i As Integer = 0 to PageAlias_list.ListOptions.Items.Count -1
	If PageAlias_list.ListOptions.Items(i).Visible Then Response.Write(PageAlias_list.ListOptions.Items(i).HeaderCellHtml)
Next
%>
<% End If %>
<% End If %>
<% If PageAlias.zPageURL.Visible Then ' PageURL %>
	<% If PageAlias.SortUrl(PageAlias.zPageURL) = "" Then %>
		<td>Page URL</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= PageAlias.SortUrl(PageAlias.zPageURL) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Page URL&nbsp;(*)</td><td style="width: 10px;"><% If PageAlias.zPageURL.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf PageAlias.zPageURL.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If PageAlias.TargetURL.Visible Then ' TargetURL %>
	<% If PageAlias.SortUrl(PageAlias.TargetURL) = "" Then %>
		<td>Target URL</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= PageAlias.SortUrl(PageAlias.TargetURL) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Target URL&nbsp;(*)</td><td style="width: 10px;"><% If PageAlias.TargetURL.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf PageAlias.TargetURL.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If PageAlias.AliasType.Visible Then ' AliasType %>
	<% If PageAlias.SortUrl(PageAlias.AliasType) = "" Then %>
		<td>Alias Type</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= PageAlias.SortUrl(PageAlias.AliasType) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Alias Type&nbsp;(*)</td><td style="width: 10px;"><% If PageAlias.AliasType.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf PageAlias.AliasType.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If PageAlias.CompanyID.Visible Then ' CompanyID %>
	<% If PageAlias.SortUrl(PageAlias.CompanyID) = "" Then %>
		<td>Company</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= PageAlias.SortUrl(PageAlias.CompanyID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Company</td><td style="width: 10px;"><% If PageAlias.CompanyID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf PageAlias.CompanyID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
	</tr>
</thead>
<tbody><!-- Table body -->
<%
If (PageAlias.ExportAll AndAlso PageAlias.Export <> "") Then
	PageAlias_list.lStopRec = PageAlias_list.lTotalRecs
Else
	PageAlias_list.lStopRec = PageAlias_list.lStartRec + PageAlias_list.lDisplayRecs - 1 ' Set the last record to display
End If
If PageAlias.CurrentAction = "gridadd" AndAlso PageAlias_list.lStopRec = -1 Then
	PageAlias_list.lStopRec = EW_GRIDADD_ROWS
End If 

' Move to first record
For i As Integer = 1 to PageAlias_list.lStartRec - 1
	If Rs.Read() Then	PageAlias_list.lRecCnt = PageAlias_list.lRecCnt + 1
Next		
PageAlias_list.lRowCnt = 0
PageAlias_list.lEditRowCnt = 0
If PageAlias.CurrentAction = "edit" Then PageAlias_list.lRowIndex = 1
If PageAlias.CurrentAction = "gridadd" Then PageAlias_list.lRowIndex = 0
If PageAlias.CurrentAction = "gridedit" Then PageAlias_list.lRowIndex = 0

' Output data rows
Do While (PageAlias.CurrentAction = "gridadd" OrElse Rs.Read()) AndAlso (PageAlias_list.lRecCnt < PageAlias_list.lStopRec)
	PageAlias_list.lRecCnt = PageAlias_list.lRecCnt + 1
	If PageAlias_list.lRecCnt >= PageAlias_list.lStartRec Then
		PageAlias_list.lRowCnt = PageAlias_list.lRowCnt + 1
		If PageAlias.CurrentAction = "gridadd" OrElse PageAlias.CurrentAction = "gridedit" Then PageAlias_list.lRowIndex = PageAlias_list.lRowIndex + 1
	PageAlias.CssClass = ""
	PageAlias.CssStyle = ""
	PageAlias.RowClientEvents = "onmouseover='ew_MouseOver(event, this);' onmouseout='ew_MouseOut(event, this);' onclick='ew_Click(event, this);'"
	If PageAlias.CurrentAction = "gridadd" Then
		PageAlias_list.LoadDefaultValues() ' Load default values
	Else
		PageAlias_list.LoadRowValues(Rs) ' Load row values
	End If
	PageAlias.RowType = EW_ROWTYPE_VIEW ' Render view
	If PageAlias.CurrentAction = "gridadd" Then ' Grid add
		PageAlias.RowType = EW_ROWTYPE_ADD ' Render add
	End If
	If PageAlias.CurrentAction = "gridadd" AndAlso PageAlias.EventCancelled Then ' Insert failed
		PageAlias_list.RestoreCurrentRowFormValues(PageAlias_list.lRowIndex) ' Restore form values
	End If
	If PageAlias.CurrentAction = "edit" Then
		If PageAlias_list.CheckInlineEditKey() AndAlso PageAlias_list.lEditRowCnt = 0 Then ' Inline edit
			PageAlias.RowType = EW_ROWTYPE_EDIT ' Render edit
		End If
	End If
	If PageAlias.CurrentAction = "gridedit" Then ' Grid edit
		PageAlias.RowType = EW_ROWTYPE_EDIT ' Render edit
	End If
	If PageAlias.RowType = EW_ROWTYPE_EDIT AndAlso PageAlias.EventCancelled Then ' update failed
		If PageAlias.CurrentAction = "edit" Then
			PageAlias_list.RestoreFormValues() ' Restore form values
		End If
		If PageAlias.CurrentAction = "gridedit" Then
			PageAlias_list.RestoreCurrentRowFormValues(PageAlias_list.lRowIndex) ' Restore form values
		End If
	End If
	If PageAlias.RowType = EW_ROWTYPE_EDIT Then ' Edit row
		PageAlias_list.lEditRowCnt = PageAlias_list.lEditRowCnt + 1
		PageAlias.RowClientEvents = "onmouseover='this.edit=true;ew_MouseOver(event, this);' onmouseout='ew_MouseOut(event, this);' onclick='ew_Click(event, this);'"
	End If
	If PageAlias.RowType = EW_ROWTYPE_ADD OrElse PageAlias.RowType = EW_ROWTYPE_EDIT Then ' Add / Edit row
		PageAlias.CssClass = "ewTableEditRow"
	End If

	' Render row
	PageAlias_list.RenderRow()
%>
	<tr<%= PageAlias.RowAttributes %>>
<% If PageAlias.RowType = EW_ROWTYPE_ADD OrElse PageAlias.RowType = EW_ROWTYPE_EDIT Then %>
<% If PageAlias.CurrentAction = "edit" Then %>
<td colspan="<%= PageAlias_list.lOptionCnt %>" align="right"><span class="aspnetmaker">
<a href="" onclick="f=ew_GetForm('fPageAliaslist');if (PageAlias_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/update.gif' alt='Update' title='Update' width='16' height='16' border='0'></a>&nbsp;<a href="<%= PageAlias_list.PageUrl %>a=cancel"><img src='images/cancel.gif' alt='Cancel' title='Cancel' width='16' height='16' border='0'></a>
<input type="hidden" name="a_list" id="a_list" value="update" />
</span></td>
<% End If %>
<%
	If PageAlias.CurrentAction = "gridedit" Then
		PageAlias_list.sMultiSelectKey = PageAlias_list.sMultiSelectKey & "<input type=""hidden"" name=""k" & PageAlias_list.lRowIndex & "_key"" id=""k" & PageAlias_list.lRowIndex & "_key"" value=""" & PageAlias.PageAliasID.CurrentValue & """ />"
	End If
%>
<% Else %>
<% If PageAlias.Export = "" Then %>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= PageAlias.ViewUrl %>"><img src='images/view.gif' alt='View' title='View' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= PageAlias.EditUrl %>"><img src='images/edit.gif' alt='Edit' title='Edit' width='16' height='16' border='0'></a><span class="ewSeparator">&nbsp;|&nbsp;</span><a href="<%= PageAlias.InlineEditUrl %>"><img src='images/inlineedit.gif' alt='Inline Edit' title='Inline Edit' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= PageAlias.CopyUrl %>"><img src='images/copy.gif' alt='Copy' title='Copy' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= PageAlias.DeleteUrl %>"><img src='images/delete.gif' alt='Delete' title='Delete' width='16' height='16' border='0'></a>
</span></td>
<%

' Custom list options
For i As Integer = 0 to PageAlias_list.ListOptions.Items.Count -1
	If PageAlias_list.ListOptions.Items(i).Visible Then Response.Write(PageAlias_list.ListOptions.Items(i).BodyCellHtml)
Next
%>
<% End If %>
<% End If %>
	<% If PageAlias.zPageURL.Visible Then ' PageURL %>
		<td<%= PageAlias.zPageURL.CellAttributes %>>
<% If PageAlias.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<input type="text" name="x<%= PageAlias_list.lRowIndex %>_zPageURL" id="x<%= PageAlias_list.lRowIndex %>_zPageURL" size="30" maxlength="255" value="<%= PageAlias.zPageURL.EditValue %>"<%= PageAlias.zPageURL.EditAttributes %> />
<input type="hidden" name="o<%= PageAlias_list.lRowIndex %>_zPageURL" id="o<%= PageAlias_list.lRowIndex %>_zPageURL" value="<%= ew_HTMLEncode(PageAlias.zPageURL.OldValue) %>" />
<% End If %>
<% If PageAlias.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<input type="text" name="x<%= PageAlias_list.lRowIndex %>_zPageURL" id="x<%= PageAlias_list.lRowIndex %>_zPageURL" size="30" maxlength="255" value="<%= PageAlias.zPageURL.EditValue %>"<%= PageAlias.zPageURL.EditAttributes %> />
<% End If %>
<% If PageAlias.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= PageAlias.zPageURL.ViewAttributes %>><%= PageAlias.zPageURL.ListViewValue %></div>
<% End If %>
<% If PageAlias.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<input type="hidden" name="o<%= PageAlias_list.lRowIndex %>_PageAliasID" id="o<%= PageAlias_list.lRowIndex %>_PageAliasID" value="<%= ew_HTMLEncode(PageAlias.PageAliasID.OldValue) %>" />
<% End If %>
<% If PageAlias.RowType = EW_ROWTYPE_EDIT Then %>
<input type="hidden" name="x<%= PageAlias_list.lRowIndex %>_PageAliasID" id="x<%= PageAlias_list.lRowIndex %>_PageAliasID" value="<%= ew_HTMLEncode(PageAlias.PageAliasID.CurrentValue) %>" />
<% End If %>
</td>
	<% End If %>
	<% If PageAlias.TargetURL.Visible Then ' TargetURL %>
		<td<%= PageAlias.TargetURL.CellAttributes %>>
<% If PageAlias.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<input type="text" name="x<%= PageAlias_list.lRowIndex %>_TargetURL" id="x<%= PageAlias_list.lRowIndex %>_TargetURL" size="30" maxlength="255" value="<%= PageAlias.TargetURL.EditValue %>"<%= PageAlias.TargetURL.EditAttributes %> />
<input type="hidden" name="o<%= PageAlias_list.lRowIndex %>_TargetURL" id="o<%= PageAlias_list.lRowIndex %>_TargetURL" value="<%= ew_HTMLEncode(PageAlias.TargetURL.OldValue) %>" />
<% End If %>
<% If PageAlias.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<input type="text" name="x<%= PageAlias_list.lRowIndex %>_TargetURL" id="x<%= PageAlias_list.lRowIndex %>_TargetURL" size="30" maxlength="255" value="<%= PageAlias.TargetURL.EditValue %>"<%= PageAlias.TargetURL.EditAttributes %> />
<% End If %>
<% If PageAlias.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= PageAlias.TargetURL.ViewAttributes %>><%= PageAlias.TargetURL.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	<% If PageAlias.AliasType.Visible Then ' AliasType %>
		<td<%= PageAlias.AliasType.CellAttributes %>>
<% If PageAlias.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<input type="text" name="x<%= PageAlias_list.lRowIndex %>_AliasType" id="x<%= PageAlias_list.lRowIndex %>_AliasType" size="30" maxlength="10" value="<%= PageAlias.AliasType.EditValue %>"<%= PageAlias.AliasType.EditAttributes %> />
<input type="hidden" name="o<%= PageAlias_list.lRowIndex %>_AliasType" id="o<%= PageAlias_list.lRowIndex %>_AliasType" value="<%= ew_HTMLEncode(PageAlias.AliasType.OldValue) %>" />
<% End If %>
<% If PageAlias.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<input type="text" name="x<%= PageAlias_list.lRowIndex %>_AliasType" id="x<%= PageAlias_list.lRowIndex %>_AliasType" size="30" maxlength="10" value="<%= PageAlias.AliasType.EditValue %>"<%= PageAlias.AliasType.EditAttributes %> />
<% End If %>
<% If PageAlias.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= PageAlias.AliasType.ViewAttributes %>><%= PageAlias.AliasType.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	<% If PageAlias.CompanyID.Visible Then ' CompanyID %>
		<td<%= PageAlias.CompanyID.CellAttributes %>>
<% If PageAlias.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<select id="x<%= PageAlias_list.lRowIndex %>_CompanyID" name="x<%= PageAlias_list.lRowIndex %>_CompanyID"<%= PageAlias.CompanyID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(PageAlias.CompanyID.EditValue) Then
	arwrk = PageAlias.CompanyID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), PageAlias.CompanyID.CurrentValue) Then
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
If emptywrk Then PageAlias.CompanyID.OldValue = ""
%>
</select>
<input type="hidden" name="o<%= PageAlias_list.lRowIndex %>_CompanyID" id="o<%= PageAlias_list.lRowIndex %>_CompanyID" value="<%= ew_HTMLEncode(PageAlias.CompanyID.OldValue) %>" />
<% End If %>
<% If PageAlias.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<select id="x<%= PageAlias_list.lRowIndex %>_CompanyID" name="x<%= PageAlias_list.lRowIndex %>_CompanyID"<%= PageAlias.CompanyID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(PageAlias.CompanyID.EditValue) Then
	arwrk = PageAlias.CompanyID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), PageAlias.CompanyID.CurrentValue) Then
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
If emptywrk Then PageAlias.CompanyID.OldValue = ""
%>
</select>
<% End If %>
<% If PageAlias.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= PageAlias.CompanyID.ViewAttributes %>><%= PageAlias.CompanyID.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	</tr>
<% If PageAlias.RowType = EW_ROWTYPE_ADD Then %>
<% End If %>
<% If PageAlias.RowType = EW_ROWTYPE_EDIT Then %>
<% End If %>
<%
	End If
Loop
%>
</tbody>
</table>
<% End If %>
<% If PageAlias.CurrentAction = "gridadd" Then %>
<input type="hidden" name="a_list" id="a_list" value="gridinsert" />
<input type="hidden" name="key_count" id="key_count" value="<%= PageAlias_list.lRowIndex %>" />
<% End If %>
<% If PageAlias.CurrentAction = "edit" Then %>
<input type="hidden" name="key_count" id="key_count" value="<%= PageAlias_list.lRowIndex %>" />
<% End If %>
<% If PageAlias.CurrentAction = "gridedit" Then %>
<input type="hidden" name="a_list" id="a_list" value="gridupdate" />
<input type="hidden" name="key_count" id="key_count" value="<%= PageAlias_list.lRowIndex %>" />
<%= PageAlias_list.sMultiSelectKey %>
<% End If %>
</form>
<%

' Close recordset
Rs.Close()
Rs.Dispose()
%>
</div>
<% If PageAlias_list.lTotalRecs > 0 Then %>
<% If PageAlias.Export = "" Then %>
<div class="ewGridLowerPanel">
<% If PageAlias.CurrentAction <> "gridadd" AndAlso PageAlias.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If PageAlias_list.Pager Is Nothing Then PageAlias_list.Pager = New cPrevNextPager(PageAlias_list.lStartRec, PageAlias_list.lDisplayRecs, PageAlias_list.lTotalRecs) %>
<% If PageAlias_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If PageAlias_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= PageAlias_list.PageUrl %>start=<%= PageAlias_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If PageAlias_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= PageAlias_list.PageUrl %>start=<%= PageAlias_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= PageAlias_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If PageAlias_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= PageAlias_list.PageUrl %>start=<%= PageAlias_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If PageAlias_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= PageAlias_list.PageUrl %>start=<%= PageAlias_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= PageAlias_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= PageAlias_list.Pager.FromIndex %> to <%= PageAlias_list.Pager.ToIndex %> of <%= PageAlias_list.Pager.RecordCount %></span>
<% Else %>
	<% If PageAlias_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If PageAlias_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="PageAlias" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If PageAlias_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If PageAlias_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If PageAlias_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If PageAlias.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<% 'If PageAlias_list.lTotalRecs > 0 Then %>
<div class="aspnetmaker">
<% If PageAlias.CurrentAction <> "gridadd" AndAlso PageAlias.CurrentAction <> "gridedit" Then ' Not grid add/edit mode %>
<a href="<%= PageAlias.AddUrl %>">Add</a>&nbsp;&nbsp;
<a href="<%= PageAlias_list.PageUrl %>a=gridadd">Grid Add</a>&nbsp;&nbsp;
<% If PageAlias_list.lTotalRecs > 0 Then %>
<a href="<%= PageAlias_list.PageUrl %>a=gridedit">Grid Edit</a>&nbsp;&nbsp;
<% End If %>
<% Else ' Grid add/edit mode %>
<% If PageAlias.CurrentAction = "gridadd" Then %>
<a href="" onclick="f=ew_GetForm('fPageAliaslist');if (PageAlias_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/insert.gif' alt='Insert' title='Insert' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<% If PageAlias.CurrentAction = "gridedit" Then %>
<a href="" onclick="f=ew_GetForm('fPageAliaslist');if (PageAlias_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/update.gif' alt='Save' title='Save' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<a href="<%= PageAlias_list.PageUrl %>a=cancel"><img src='images/cancel.gif' alt='Cancel' title='Cancel' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
</div>
<% 'End If %>
</div>
<% End If %>
<% End If %>
</td></tr></table>
<% If PageAlias.Export = "" AndAlso PageAlias.CurrentAction = "" Then %>
<script type="text/javascript">
<!--
//ew_ToggleSearchPanel(PageAlias_list); // uncomment to init search panel as collapsed
//-->
</script>
<% End If %>
<% If PageAlias.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
