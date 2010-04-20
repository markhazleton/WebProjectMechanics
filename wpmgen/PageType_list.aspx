<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="PageType_list.aspx.vb" Inherits="PageType_list" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If PageType.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var PageType_list = new ew_Page("PageType_list");
// page properties
PageType_list.PageID = "list"; // page ID
var EW_PAGE_ID = PageType_list.PageID; // for backward compatibility
// extend page with ValidateForm function
PageType_list.ValidateForm = function(fobj) {
	if (!this.ValidateRequired)
		return true; // ignore validation
	if (fobj.a_confirm && fobj.a_confirm.value == "F")
		return true;
	var i, elm, aelm, infix;
	var rowcnt = (fobj.key_count) ? Number(fobj.key_count.value) : 1;
	for (i=0; i<rowcnt; i++) {
		infix = (fobj.key_count) ? String(i+1) : "";
	}
	return true;
}
<% If EW_CLIENT_VALIDATE Then %>
PageType_list.ValidateRequired = true; // uses JavaScript validation
<% Else %>
PageType_list.ValidateRequired = false; // no JavaScript validation
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
<% If PageType.Export = "" Then %>
<% End If %>
<%

' Load recordset
Rs = PageType_list.LoadRecordset()
	PageType_list.lStartRec = 1
	If PageType_list.lDisplayRecs <= 0 Then ' Display all records
		PageType_list.lDisplayRecs = PageType_list.lTotalRecs
	End If
	If Not (PageType.ExportAll AndAlso PageType.Export <> "") Then
		PageType_list.SetUpStartRec() ' Set up start record position
	End If
%>
<p><span class="aspnetmaker" style="white-space: nowrap;">TABLE: Location Type
<% If PageType.Export = "" AndAlso PageType.CurrentAction = "" Then %>
&nbsp;&nbsp;<a href="<%= PageType_list.PageUrl %>export=excel"><img src='images/exportxls.gif' alt='Export to Excel' title='Export to Excel' width='16' height='16' border='0'></a>
<% End If %>
</span></p>
<% If PageType.Export = "" AndAlso PageType.CurrentAction = "" Then %>
<a href="javascript:ew_ToggleSearchPanel(PageType_list);" style="text-decoration: none;"><img id="PageType_list_SearchImage" src="images/collapse.gif" alt="" width="9" height="9" border="0"></a><span class="aspnetmaker">&nbsp;Search</span><br>
<div id="PageType_list_SearchPanel">
<form name="fPageTypelistsrch" id="fPageTypelistsrch" class="ewForm">
<input type="hidden" id="t" name="t" value="PageType" />
<table class="ewBasicSearch">
	<tr>
		<td><span class="aspnetmaker">
			<input type="text" name="<%= EW_TABLE_BASIC_SEARCH %>" id="<%= EW_TABLE_BASIC_SEARCH %>" size="20" value="<%= ew_HtmlEncode(PageType.BasicSearchKeyword) %>" />
			<input type="Submit" name="Submit" id="Submit" value="Search (*)" />&nbsp;
			<a href="<%= PageType_list.PageUrl %>cmd=reset">Show all</a>&nbsp;
			<a href="PageType_srch.aspx">Advanced Search</a>&nbsp;
		</span></td>
	</tr>
	<tr>
	<td><span class="aspnetmaker"><label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value=""<% If PageType.BasicSearchType = "" Then %> checked="checked"<% End If %> />Exact phrase</label>&nbsp;&nbsp;<label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value="AND"<% If PageType.BasicSearchType = "AND" Then %> checked="checked"<% End If %> />All words</label>&nbsp;&nbsp;<label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value="OR"<% If PageType.BasicSearchType = "OR" Then %> checked="checked"<% End If %> />Any word</label></span></td>
	</tr>
</table>
</form>
</div>
<% End If %>
<% PageType_list.ShowMessage() %>
<br />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<% If PageType.Export = "" Then %>
<div class="ewGridUpperPanel">
<% If PageType.CurrentAction <> "gridadd" AndAlso PageType.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If PageType_list.Pager Is Nothing Then PageType_list.Pager = New cPrevNextPager(PageType_list.lStartRec, PageType_list.lDisplayRecs, PageType_list.lTotalRecs) %>
<% If PageType_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If PageType_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= PageType_list.PageUrl %>start=<%= PageType_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If PageType_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= PageType_list.PageUrl %>start=<%= PageType_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= PageType_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If PageType_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= PageType_list.PageUrl %>start=<%= PageType_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If PageType_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= PageType_list.PageUrl %>start=<%= PageType_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= PageType_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= PageType_list.Pager.FromIndex %> to <%= PageType_list.Pager.ToIndex %> of <%= PageType_list.Pager.RecordCount %></span>
<% Else %>
	<% If PageType_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If PageType_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="PageType" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If PageType_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If PageType_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If PageType_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If PageType.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<div class="aspnetmaker">
<a href="<%= PageType.AddUrl %>">Add</a>&nbsp;&nbsp;
</div>
</div>
<% End If %>
<div class="ewGridMiddlePanel">
<form name="fPageTypelist" id="fPageTypelist" class="ewForm" method="post">
<input type="hidden" name="t" id="t" value="PageType" />
<% If PageType_list.lTotalRecs > 0 Then %>
<table cellspacing="0" rowhighlightclass="ewTableHighlightRow" rowselectclass="ewTableSelectRow" roweditclass="ewTableEditRow" class="ewTable ewTableSeparate">
<%
	PageType_list.lOptionCnt = 0
	PageType_list.lOptionCnt = PageType_list.lOptionCnt + 1 ' View
	PageType_list.lOptionCnt = PageType_list.lOptionCnt + 1 ' Edit
	PageType_list.lOptionCnt = PageType_list.lOptionCnt + 1 ' Copy
	PageType_list.lOptionCnt = PageType_list.lOptionCnt + 1 ' Delete
	PageType_list.lOptionCnt = PageType_list.lOptionCnt + PageType_list.ListOptions.Items.Count ' Custom list options
%>
<%= PageType.TableCustomInnerHTML %>
<thead><!-- Table header -->
	<tr class="ewTableHeader">
<% If PageType.Export = "" Then %>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<%

' Custom list options
For i As Integer = 0 to PageType_list.ListOptions.Items.Count -1
	If PageType_list.ListOptions.Items(i).Visible Then Response.Write(PageType_list.ListOptions.Items(i).HeaderCellHtml)
Next
%>
<% End If %>
<% If PageType.PageTypeID.Visible Then ' PageTypeID %>
	<% If PageType.SortUrl(PageType.PageTypeID) = "" Then %>
		<td>Page Type ID</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= PageType.SortUrl(PageType.PageTypeID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Page Type ID</td><td style="width: 10px;"><% If PageType.PageTypeID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf PageType.PageTypeID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If PageType.PageTypeCD.Visible Then ' PageTypeCD %>
	<% If PageType.SortUrl(PageType.PageTypeCD) = "" Then %>
		<td>Page Type CD</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= PageType.SortUrl(PageType.PageTypeCD) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Page Type CD&nbsp;(*)</td><td style="width: 10px;"><% If PageType.PageTypeCD.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf PageType.PageTypeCD.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If PageType.PageTypeDesc.Visible Then ' PageTypeDesc %>
	<% If PageType.SortUrl(PageType.PageTypeDesc) = "" Then %>
		<td>Page Type Desc</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= PageType.SortUrl(PageType.PageTypeDesc) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Page Type Desc&nbsp;(*)</td><td style="width: 10px;"><% If PageType.PageTypeDesc.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf PageType.PageTypeDesc.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If PageType.PageFileName.Visible Then ' PageFileName %>
	<% If PageType.SortUrl(PageType.PageFileName) = "" Then %>
		<td>Page File Name</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= PageType.SortUrl(PageType.PageFileName) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Page File Name&nbsp;(*)</td><td style="width: 10px;"><% If PageType.PageFileName.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf PageType.PageFileName.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
	</tr>
</thead>
<tbody><!-- Table body -->
<%
If (PageType.ExportAll AndAlso PageType.Export <> "") Then
	PageType_list.lStopRec = PageType_list.lTotalRecs
Else
	PageType_list.lStopRec = PageType_list.lStartRec + PageType_list.lDisplayRecs - 1 ' Set the last record to display
End If
If PageType.CurrentAction = "gridadd" AndAlso PageType_list.lStopRec = -1 Then
	PageType_list.lStopRec = EW_GRIDADD_ROWS
End If 

' Move to first record
For i As Integer = 1 to PageType_list.lStartRec - 1
	If Rs.Read() Then	PageType_list.lRecCnt = PageType_list.lRecCnt + 1
Next		
PageType_list.lRowCnt = 0
PageType_list.lEditRowCnt = 0
If PageType.CurrentAction = "edit" Then PageType_list.lRowIndex = 1

' Output data rows
Do While (PageType.CurrentAction = "gridadd" OrElse Rs.Read()) AndAlso (PageType_list.lRecCnt < PageType_list.lStopRec)
	PageType_list.lRecCnt = PageType_list.lRecCnt + 1
	If PageType_list.lRecCnt >= PageType_list.lStartRec Then
		PageType_list.lRowCnt = PageType_list.lRowCnt + 1
	PageType.CssClass = ""
	PageType.CssStyle = ""
	PageType.RowClientEvents = "onmouseover='ew_MouseOver(event, this);' onmouseout='ew_MouseOut(event, this);' onclick='ew_Click(event, this);'"
	If PageType.CurrentAction = "gridadd" Then
		PageType_list.LoadDefaultValues() ' Load default values
	Else
		PageType_list.LoadRowValues(Rs) ' Load row values
	End If
	PageType.RowType = EW_ROWTYPE_VIEW ' Render view
	If PageType.CurrentAction = "edit" Then
		If PageType_list.CheckInlineEditKey() AndAlso PageType_list.lEditRowCnt = 0 Then ' Inline edit
			PageType.RowType = EW_ROWTYPE_EDIT ' Render edit
		End If
	End If
	If PageType.RowType = EW_ROWTYPE_EDIT AndAlso PageType.EventCancelled Then ' update failed
		If PageType.CurrentAction = "edit" Then
			PageType_list.RestoreFormValues() ' Restore form values
		End If
	End If
	If PageType.RowType = EW_ROWTYPE_EDIT Then ' Edit row
		PageType_list.lEditRowCnt = PageType_list.lEditRowCnt + 1
		PageType.RowClientEvents = "onmouseover='this.edit=true;ew_MouseOver(event, this);' onmouseout='ew_MouseOut(event, this);' onclick='ew_Click(event, this);'"
	End If
	If PageType.RowType = EW_ROWTYPE_ADD OrElse PageType.RowType = EW_ROWTYPE_EDIT Then ' Add / Edit row
		PageType.CssClass = "ewTableEditRow"
	End If

	' Render row
	PageType_list.RenderRow()
%>
	<tr<%= PageType.RowAttributes %>>
<% If PageType.RowType = EW_ROWTYPE_ADD OrElse PageType.RowType = EW_ROWTYPE_EDIT Then %>
<% If PageType.CurrentAction = "edit" Then %>
<td colspan="<%= PageType_list.lOptionCnt %>" align="right"><span class="aspnetmaker">
<a href="" onclick="f=ew_GetForm('fPageTypelist');if (PageType_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/update.gif' alt='Update' title='Update' width='16' height='16' border='0'></a>&nbsp;<a href="<%= PageType_list.PageUrl %>a=cancel"><img src='images/cancel.gif' alt='Cancel' title='Cancel' width='16' height='16' border='0'></a>
<input type="hidden" name="a_list" id="a_list" value="update" />
</span></td>
<% End If %>
<% Else %>
<% If PageType.Export = "" Then %>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= PageType.ViewUrl %>"><img src='images/view.gif' alt='View' title='View' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= PageType.EditUrl %>"><img src='images/edit.gif' alt='Edit' title='Edit' width='16' height='16' border='0'></a><span class="ewSeparator">&nbsp;|&nbsp;</span><a href="<%= PageType.InlineEditUrl %>"><img src='images/inlineedit.gif' alt='Inline Edit' title='Inline Edit' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= PageType.CopyUrl %>"><img src='images/copy.gif' alt='Copy' title='Copy' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= PageType.DeleteUrl %>"><img src='images/delete.gif' alt='Delete' title='Delete' width='16' height='16' border='0'></a>
</span></td>
<%

' Custom list options
For i As Integer = 0 to PageType_list.ListOptions.Items.Count -1
	If PageType_list.ListOptions.Items(i).Visible Then Response.Write(PageType_list.ListOptions.Items(i).BodyCellHtml)
Next
%>
<% End If %>
<% End If %>
	<% If PageType.PageTypeID.Visible Then ' PageTypeID %>
		<td<%= PageType.PageTypeID.CellAttributes %>>
<% If PageType.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<div<%= PageType.PageTypeID.ViewAttributes %>><%= PageType.PageTypeID.EditValue %></div>
<input type="hidden" name="x<%= PageType_list.lRowIndex %>_PageTypeID" id="x<%= PageType_list.lRowIndex %>_PageTypeID" value="<%= ew_HTMLEncode(PageType.PageTypeID.CurrentValue) %>" />
<% End If %>
<% If PageType.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= PageType.PageTypeID.ViewAttributes %>><%= PageType.PageTypeID.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	<% If PageType.PageTypeCD.Visible Then ' PageTypeCD %>
		<td<%= PageType.PageTypeCD.CellAttributes %>>
<% If PageType.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<input type="text" name="x<%= PageType_list.lRowIndex %>_PageTypeCD" id="x<%= PageType_list.lRowIndex %>_PageTypeCD" size="30" maxlength="50" value="<%= PageType.PageTypeCD.EditValue %>"<%= PageType.PageTypeCD.EditAttributes %> />
<% End If %>
<% If PageType.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= PageType.PageTypeCD.ViewAttributes %>><%= PageType.PageTypeCD.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	<% If PageType.PageTypeDesc.Visible Then ' PageTypeDesc %>
		<td<%= PageType.PageTypeDesc.CellAttributes %>>
<% If PageType.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<input type="text" name="x<%= PageType_list.lRowIndex %>_PageTypeDesc" id="x<%= PageType_list.lRowIndex %>_PageTypeDesc" size="30" maxlength="50" value="<%= PageType.PageTypeDesc.EditValue %>"<%= PageType.PageTypeDesc.EditAttributes %> />
<% End If %>
<% If PageType.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= PageType.PageTypeDesc.ViewAttributes %>><%= PageType.PageTypeDesc.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	<% If PageType.PageFileName.Visible Then ' PageFileName %>
		<td<%= PageType.PageFileName.CellAttributes %>>
<% If PageType.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<input type="text" name="x<%= PageType_list.lRowIndex %>_PageFileName" id="x<%= PageType_list.lRowIndex %>_PageFileName" size="30" maxlength="50" value="<%= PageType.PageFileName.EditValue %>"<%= PageType.PageFileName.EditAttributes %> />
<% End If %>
<% If PageType.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= PageType.PageFileName.ViewAttributes %>><%= PageType.PageFileName.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	</tr>
<% If PageType.RowType = EW_ROWTYPE_EDIT Then %>
<% End If %>
<%
	End If
Loop
%>
</tbody>
</table>
<% End If %>
<% If PageType.CurrentAction = "edit" Then %>
<input type="hidden" name="key_count" id="key_count" value="<%= PageType_list.lRowIndex %>" />
<% End If %>
</form>
<%

' Close recordset
Rs.Close()
Rs.Dispose()
%>
</div>
<% If PageType_list.lTotalRecs > 0 Then %>
<% If PageType.Export = "" Then %>
<div class="ewGridLowerPanel">
<% If PageType.CurrentAction <> "gridadd" AndAlso PageType.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If PageType_list.Pager Is Nothing Then PageType_list.Pager = New cPrevNextPager(PageType_list.lStartRec, PageType_list.lDisplayRecs, PageType_list.lTotalRecs) %>
<% If PageType_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If PageType_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= PageType_list.PageUrl %>start=<%= PageType_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If PageType_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= PageType_list.PageUrl %>start=<%= PageType_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= PageType_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If PageType_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= PageType_list.PageUrl %>start=<%= PageType_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If PageType_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= PageType_list.PageUrl %>start=<%= PageType_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= PageType_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= PageType_list.Pager.FromIndex %> to <%= PageType_list.Pager.ToIndex %> of <%= PageType_list.Pager.RecordCount %></span>
<% Else %>
	<% If PageType_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If PageType_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="PageType" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If PageType_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If PageType_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If PageType_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If PageType.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<% 'If PageType_list.lTotalRecs > 0 Then %>
<div class="aspnetmaker">
<a href="<%= PageType.AddUrl %>">Add</a>&nbsp;&nbsp;
</div>
<% 'End If %>
</div>
<% End If %>
<% End If %>
</td></tr></table>
<% If PageType.Export = "" AndAlso PageType.CurrentAction = "" Then %>
<script type="text/javascript">
<!--
//ew_ToggleSearchPanel(PageType_list); // uncomment to init search panel as collapsed
//-->
</script>
<% End If %>
<% If PageType.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
