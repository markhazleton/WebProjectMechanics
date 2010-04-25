<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="LinkCategory_list.aspx.vb" Inherits="LinkCategory_list" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If LinkCategory.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var LinkCategory_list = new ew_Page("LinkCategory_list");
// page properties
LinkCategory_list.PageID = "list"; // page ID
var EW_PAGE_ID = LinkCategory_list.PageID; // for backward compatibility
// extend page with ValidateForm function
LinkCategory_list.ValidateForm = function(fobj) {
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
LinkCategory_list.EmptyRow = function(fobj, infix) {
	if (ew_ValueChanged(fobj, infix, "Title")) return false;
	if (ew_ValueChanged(fobj, infix, "ParentID")) return false;
	if (ew_ValueChanged(fobj, infix, "zPageID")) return false;
	return true;
}
<% If EW_CLIENT_VALIDATE Then %>
LinkCategory_list.ValidateRequired = true; // uses JavaScript validation
<% Else %>
LinkCategory_list.ValidateRequired = false; // no JavaScript validation
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
<% If LinkCategory.Export = "" Then %>
<% End If %>
<%
If LinkCategory.CurrentAction = "gridadd" Then LinkCategory.CurrentFilter = "0=1"

' Load recordset
Rs = LinkCategory_list.LoadRecordset()
If LinkCategory.CurrentAction = "gridadd" Then
	LinkCategory_list.lStartRec = 1
	If LinkCategory_list.lDisplayRecs <= 0 Then LinkCategory_list.lDisplayRecs = 25
	LinkCategory_list.lTotalRecs = LinkCategory_list.lDisplayRecs
	LinkCategory_list.lStopRec = LinkCategory_list.lDisplayRecs
Else
	LinkCategory_list.lStartRec = 1
	If LinkCategory_list.lDisplayRecs <= 0 Then ' Display all records
		LinkCategory_list.lDisplayRecs = LinkCategory_list.lTotalRecs
	End If
	If Not (LinkCategory.ExportAll AndAlso LinkCategory.Export <> "") Then
		LinkCategory_list.SetUpStartRec() ' Set up start record position
	End If
End If
%>
<p><span class="aspnetmaker" style="white-space: nowrap;">TABLE: Part Category
<% If LinkCategory.Export = "" AndAlso LinkCategory.CurrentAction = "" Then %>
&nbsp;&nbsp;<a href="<%= LinkCategory_list.PageUrl %>export=excel"><img src='images/exportxls.gif' alt='Export to Excel' title='Export to Excel' width='16' height='16' border='0'></a>
<% End If %>
</span></p>
<% If LinkCategory.Export = "" AndAlso LinkCategory.CurrentAction = "" Then %>
<a href="javascript:ew_ToggleSearchPanel(LinkCategory_list);" style="text-decoration: none;"><img id="LinkCategory_list_SearchImage" src="images/collapse.gif" alt="" width="9" height="9" border="0"></a><span class="aspnetmaker">&nbsp;Search</span><br>
<div id="LinkCategory_list_SearchPanel">
<form name="fLinkCategorylistsrch" id="fLinkCategorylistsrch" class="ewForm">
<input type="hidden" id="t" name="t" value="LinkCategory" />
<table class="ewBasicSearch">
	<tr>
		<td><span class="aspnetmaker">
			<input type="text" name="<%= EW_TABLE_BASIC_SEARCH %>" id="<%= EW_TABLE_BASIC_SEARCH %>" size="20" value="<%= ew_HtmlEncode(LinkCategory.BasicSearchKeyword) %>" />
			<input type="Submit" name="Submit" id="Submit" value="Search (*)" />&nbsp;
			<a href="<%= LinkCategory_list.PageUrl %>cmd=reset">Show all</a>&nbsp;
			<a href="LinkCategory_srch.aspx">Advanced Search</a>&nbsp;
		</span></td>
	</tr>
	<tr>
	<td><span class="aspnetmaker"><label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value=""<% If LinkCategory.BasicSearchType = "" Then %> checked="checked"<% End If %> />Exact phrase</label>&nbsp;&nbsp;<label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value="AND"<% If LinkCategory.BasicSearchType = "AND" Then %> checked="checked"<% End If %> />All words</label>&nbsp;&nbsp;<label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value="OR"<% If LinkCategory.BasicSearchType = "OR" Then %> checked="checked"<% End If %> />Any word</label></span></td>
	</tr>
</table>
</form>
</div>
<% End If %>
<% LinkCategory_list.ShowMessage() %>
<br />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<% If LinkCategory.Export = "" Then %>
<div class="ewGridUpperPanel">
<% If LinkCategory.CurrentAction <> "gridadd" AndAlso LinkCategory.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If LinkCategory_list.Pager Is Nothing Then LinkCategory_list.Pager = New cPrevNextPager(LinkCategory_list.lStartRec, LinkCategory_list.lDisplayRecs, LinkCategory_list.lTotalRecs) %>
<% If LinkCategory_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If LinkCategory_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= LinkCategory_list.PageUrl %>start=<%= LinkCategory_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If LinkCategory_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= LinkCategory_list.PageUrl %>start=<%= LinkCategory_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= LinkCategory_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If LinkCategory_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= LinkCategory_list.PageUrl %>start=<%= LinkCategory_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If LinkCategory_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= LinkCategory_list.PageUrl %>start=<%= LinkCategory_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= LinkCategory_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= LinkCategory_list.Pager.FromIndex %> to <%= LinkCategory_list.Pager.ToIndex %> of <%= LinkCategory_list.Pager.RecordCount %></span>
<% Else %>
	<% If LinkCategory_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If LinkCategory_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="LinkCategory" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If LinkCategory_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If LinkCategory_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If LinkCategory_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If LinkCategory.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<div class="aspnetmaker">
<% If LinkCategory.CurrentAction <> "gridadd" AndAlso LinkCategory.CurrentAction <> "gridedit" Then ' Not grid add/edit mode %>
<a href="<%= LinkCategory.AddUrl %>">Add</a>&nbsp;&nbsp;
<a href="<%= LinkCategory_list.PageUrl %>a=gridadd">Grid Add</a>&nbsp;&nbsp;
<% If LinkCategory_list.lTotalRecs > 0 Then %>
<a href="<%= LinkCategory_list.PageUrl %>a=gridedit">Grid Edit</a>&nbsp;&nbsp;
<% End If %>
<% Else ' Grid add/edit mode %>
<% If LinkCategory.CurrentAction = "gridadd" Then %>
<a href="" onclick="f=ew_GetForm('fLinkCategorylist');if (LinkCategory_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/insert.gif' alt='Insert' title='Insert' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<% If LinkCategory.CurrentAction = "gridedit" Then %>
<a href="" onclick="f=ew_GetForm('fLinkCategorylist');if (LinkCategory_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/update.gif' alt='Save' title='Save' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<a href="<%= LinkCategory_list.PageUrl %>a=cancel"><img src='images/cancel.gif' alt='Cancel' title='Cancel' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
</div>
</div>
<% End If %>
<div class="ewGridMiddlePanel">
<form name="fLinkCategorylist" id="fLinkCategorylist" class="ewForm" method="post">
<input type="hidden" name="t" id="t" value="LinkCategory" />
<% If LinkCategory_list.lTotalRecs > 0 Then %>
<table cellspacing="0" rowhighlightclass="ewTableHighlightRow" rowselectclass="ewTableSelectRow" roweditclass="ewTableEditRow" class="ewTable ewTableSeparate">
<%
	LinkCategory_list.lOptionCnt = 0
	LinkCategory_list.lOptionCnt = LinkCategory_list.lOptionCnt + 1 ' View
	LinkCategory_list.lOptionCnt = LinkCategory_list.lOptionCnt + 1 ' Edit
	LinkCategory_list.lOptionCnt = LinkCategory_list.lOptionCnt + 1 ' Copy
	LinkCategory_list.lOptionCnt = LinkCategory_list.lOptionCnt + 1 ' Delete
	LinkCategory_list.lOptionCnt = LinkCategory_list.lOptionCnt + LinkCategory_list.ListOptions.Items.Count ' Custom list options
%>
<%= LinkCategory.TableCustomInnerHTML %>
<thead><!-- Table header -->
	<tr class="ewTableHeader">
<% If LinkCategory.Export = "" Then %>
<% If LinkCategory.CurrentAction <> "gridadd" AndAlso LinkCategory.CurrentAction <> "gridedit" Then %>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<%

' Custom list options
For i As Integer = 0 to LinkCategory_list.ListOptions.Items.Count -1
	If LinkCategory_list.ListOptions.Items(i).Visible Then Response.Write(LinkCategory_list.ListOptions.Items(i).HeaderCellHtml)
Next
%>
<% End If %>
<% End If %>
<% If LinkCategory.Title.Visible Then ' Title %>
	<% If LinkCategory.SortUrl(LinkCategory.Title) = "" Then %>
		<td style="white-space: nowrap;">Title</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= LinkCategory.SortUrl(LinkCategory.Title) %>',1);" style="white-space: nowrap;">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Title&nbsp;(*)</td><td style="width: 10px;"><% If LinkCategory.Title.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf LinkCategory.Title.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If LinkCategory.ParentID.Visible Then ' ParentID %>
	<% If LinkCategory.SortUrl(LinkCategory.ParentID) = "" Then %>
		<td style="white-space: nowrap;">Parent</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= LinkCategory.SortUrl(LinkCategory.ParentID) %>',1);" style="white-space: nowrap;">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Parent</td><td style="width: 10px;"><% If LinkCategory.ParentID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf LinkCategory.ParentID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If LinkCategory.zPageID.Visible Then ' PageID %>
	<% If LinkCategory.SortUrl(LinkCategory.zPageID) = "" Then %>
		<td style="white-space: nowrap;">Page</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= LinkCategory.SortUrl(LinkCategory.zPageID) %>',1);" style="white-space: nowrap;">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Page</td><td style="width: 10px;"><% If LinkCategory.zPageID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf LinkCategory.zPageID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
	</tr>
</thead>
<tbody><!-- Table body -->
<%
If (LinkCategory.ExportAll AndAlso LinkCategory.Export <> "") Then
	LinkCategory_list.lStopRec = LinkCategory_list.lTotalRecs
Else
	LinkCategory_list.lStopRec = LinkCategory_list.lStartRec + LinkCategory_list.lDisplayRecs - 1 ' Set the last record to display
End If
If LinkCategory.CurrentAction = "gridadd" AndAlso LinkCategory_list.lStopRec = -1 Then
	LinkCategory_list.lStopRec = EW_GRIDADD_ROWS
End If 

' Move to first record
For i As Integer = 1 to LinkCategory_list.lStartRec - 1
	If Rs.Read() Then	LinkCategory_list.lRecCnt = LinkCategory_list.lRecCnt + 1
Next		
LinkCategory_list.lRowCnt = 0
LinkCategory_list.lEditRowCnt = 0
If LinkCategory.CurrentAction = "edit" Then LinkCategory_list.lRowIndex = 1
If LinkCategory.CurrentAction = "gridadd" Then LinkCategory_list.lRowIndex = 0
If LinkCategory.CurrentAction = "gridedit" Then LinkCategory_list.lRowIndex = 0

' Output data rows
Do While (LinkCategory.CurrentAction = "gridadd" OrElse Rs.Read()) AndAlso (LinkCategory_list.lRecCnt < LinkCategory_list.lStopRec)
	LinkCategory_list.lRecCnt = LinkCategory_list.lRecCnt + 1
	If LinkCategory_list.lRecCnt >= LinkCategory_list.lStartRec Then
		LinkCategory_list.lRowCnt = LinkCategory_list.lRowCnt + 1
		If LinkCategory.CurrentAction = "gridadd" OrElse LinkCategory.CurrentAction = "gridedit" Then LinkCategory_list.lRowIndex = LinkCategory_list.lRowIndex + 1
	LinkCategory.CssClass = ""
	LinkCategory.CssStyle = ""
	LinkCategory.RowClientEvents = "onmouseover='ew_MouseOver(event, this);' onmouseout='ew_MouseOut(event, this);' onclick='ew_Click(event, this);'"
	If LinkCategory.CurrentAction = "gridadd" Then
		LinkCategory_list.LoadDefaultValues() ' Load default values
	Else
		LinkCategory_list.LoadRowValues(Rs) ' Load row values
	End If
	LinkCategory.RowType = EW_ROWTYPE_VIEW ' Render view
	If LinkCategory.CurrentAction = "gridadd" Then ' Grid add
		LinkCategory.RowType = EW_ROWTYPE_ADD ' Render add
	End If
	If LinkCategory.CurrentAction = "gridadd" AndAlso LinkCategory.EventCancelled Then ' Insert failed
		LinkCategory_list.RestoreCurrentRowFormValues(LinkCategory_list.lRowIndex) ' Restore form values
	End If
	If LinkCategory.CurrentAction = "edit" Then
		If LinkCategory_list.CheckInlineEditKey() AndAlso LinkCategory_list.lEditRowCnt = 0 Then ' Inline edit
			LinkCategory.RowType = EW_ROWTYPE_EDIT ' Render edit
		End If
	End If
	If LinkCategory.CurrentAction = "gridedit" Then ' Grid edit
		LinkCategory.RowType = EW_ROWTYPE_EDIT ' Render edit
	End If
	If LinkCategory.RowType = EW_ROWTYPE_EDIT AndAlso LinkCategory.EventCancelled Then ' update failed
		If LinkCategory.CurrentAction = "edit" Then
			LinkCategory_list.RestoreFormValues() ' Restore form values
		End If
		If LinkCategory.CurrentAction = "gridedit" Then
			LinkCategory_list.RestoreCurrentRowFormValues(LinkCategory_list.lRowIndex) ' Restore form values
		End If
	End If
	If LinkCategory.RowType = EW_ROWTYPE_EDIT Then ' Edit row
		LinkCategory_list.lEditRowCnt = LinkCategory_list.lEditRowCnt + 1
		LinkCategory.RowClientEvents = "onmouseover='this.edit=true;ew_MouseOver(event, this);' onmouseout='ew_MouseOut(event, this);' onclick='ew_Click(event, this);'"
	End If
	If LinkCategory.RowType = EW_ROWTYPE_ADD OrElse LinkCategory.RowType = EW_ROWTYPE_EDIT Then ' Add / Edit row
		LinkCategory.CssClass = "ewTableEditRow"
	End If

	' Render row
	LinkCategory_list.RenderRow()
%>
	<tr<%= LinkCategory.RowAttributes %>>
<% If LinkCategory.RowType = EW_ROWTYPE_ADD OrElse LinkCategory.RowType = EW_ROWTYPE_EDIT Then %>
<% If LinkCategory.CurrentAction = "edit" Then %>
<td colspan="<%= LinkCategory_list.lOptionCnt %>" align="right"><span class="aspnetmaker">
<a href="" onclick="f=ew_GetForm('fLinkCategorylist');if (LinkCategory_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/update.gif' alt='Update' title='Update' width='16' height='16' border='0'></a>&nbsp;<a href="<%= LinkCategory_list.PageUrl %>a=cancel"><img src='images/cancel.gif' alt='Cancel' title='Cancel' width='16' height='16' border='0'></a>
<input type="hidden" name="a_list" id="a_list" value="update" />
</span></td>
<% End If %>
<%
	If LinkCategory.CurrentAction = "gridedit" Then
		LinkCategory_list.sMultiSelectKey = LinkCategory_list.sMultiSelectKey & "<input type=""hidden"" name=""k" & LinkCategory_list.lRowIndex & "_key"" id=""k" & LinkCategory_list.lRowIndex & "_key"" value=""" & LinkCategory.ID.CurrentValue & """ />"
	End If
%>
<% Else %>
<% If LinkCategory.Export = "" Then %>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= LinkCategory.ViewUrl %>"><img src='images/view.gif' alt='View' title='View' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= LinkCategory.EditUrl %>"><img src='images/edit.gif' alt='Edit' title='Edit' width='16' height='16' border='0'></a><span class="ewSeparator">&nbsp;|&nbsp;</span><a href="<%= LinkCategory.InlineEditUrl %>"><img src='images/inlineedit.gif' alt='Inline Edit' title='Inline Edit' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= LinkCategory.CopyUrl %>"><img src='images/copy.gif' alt='Copy' title='Copy' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= LinkCategory.DeleteUrl %>"><img src='images/delete.gif' alt='Delete' title='Delete' width='16' height='16' border='0'></a>
</span></td>
<%

' Custom list options
For i As Integer = 0 to LinkCategory_list.ListOptions.Items.Count -1
	If LinkCategory_list.ListOptions.Items(i).Visible Then Response.Write(LinkCategory_list.ListOptions.Items(i).BodyCellHtml)
Next
%>
<% End If %>
<% End If %>
	<% If LinkCategory.Title.Visible Then ' Title %>
		<td<%= LinkCategory.Title.CellAttributes %>>
<% If LinkCategory.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<input type="text" name="x<%= LinkCategory_list.lRowIndex %>_Title" id="x<%= LinkCategory_list.lRowIndex %>_Title" size="30" maxlength="50" value="<%= LinkCategory.Title.EditValue %>"<%= LinkCategory.Title.EditAttributes %> />
<input type="hidden" name="o<%= LinkCategory_list.lRowIndex %>_Title" id="o<%= LinkCategory_list.lRowIndex %>_Title" value="<%= ew_HTMLEncode(LinkCategory.Title.OldValue) %>" />
<% End If %>
<% If LinkCategory.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<input type="text" name="x<%= LinkCategory_list.lRowIndex %>_Title" id="x<%= LinkCategory_list.lRowIndex %>_Title" size="30" maxlength="50" value="<%= LinkCategory.Title.EditValue %>"<%= LinkCategory.Title.EditAttributes %> />
<% End If %>
<% If LinkCategory.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= LinkCategory.Title.ViewAttributes %>><%= LinkCategory.Title.ListViewValue %></div>
<% End If %>
<% If LinkCategory.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<input type="hidden" name="o<%= LinkCategory_list.lRowIndex %>_ID" id="o<%= LinkCategory_list.lRowIndex %>_ID" value="<%= ew_HTMLEncode(LinkCategory.ID.OldValue) %>" />
<% End If %>
<% If LinkCategory.RowType = EW_ROWTYPE_EDIT Then %>
<input type="hidden" name="x<%= LinkCategory_list.lRowIndex %>_ID" id="x<%= LinkCategory_list.lRowIndex %>_ID" value="<%= ew_HTMLEncode(LinkCategory.ID.CurrentValue) %>" />
<% End If %>
</td>
	<% End If %>
	<% If LinkCategory.ParentID.Visible Then ' ParentID %>
		<td<%= LinkCategory.ParentID.CellAttributes %>>
<% If LinkCategory.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<select id="x<%= LinkCategory_list.lRowIndex %>_ParentID" name="x<%= LinkCategory_list.lRowIndex %>_ParentID"<%= LinkCategory.ParentID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(LinkCategory.ParentID.EditValue) Then
	arwrk = LinkCategory.ParentID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), LinkCategory.ParentID.CurrentValue) Then
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
If emptywrk Then LinkCategory.ParentID.OldValue = ""
%>
</select>
<input type="hidden" name="o<%= LinkCategory_list.lRowIndex %>_ParentID" id="o<%= LinkCategory_list.lRowIndex %>_ParentID" value="<%= ew_HTMLEncode(LinkCategory.ParentID.OldValue) %>" />
<% End If %>
<% If LinkCategory.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<select id="x<%= LinkCategory_list.lRowIndex %>_ParentID" name="x<%= LinkCategory_list.lRowIndex %>_ParentID"<%= LinkCategory.ParentID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(LinkCategory.ParentID.EditValue) Then
	arwrk = LinkCategory.ParentID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), LinkCategory.ParentID.CurrentValue) Then
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
If emptywrk Then LinkCategory.ParentID.OldValue = ""
%>
</select>
<% End If %>
<% If LinkCategory.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= LinkCategory.ParentID.ViewAttributes %>><%= LinkCategory.ParentID.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	<% If LinkCategory.zPageID.Visible Then ' PageID %>
		<td<%= LinkCategory.zPageID.CellAttributes %>>
<% If LinkCategory.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<select id="x<%= LinkCategory_list.lRowIndex %>_zPageID" name="x<%= LinkCategory_list.lRowIndex %>_zPageID"<%= LinkCategory.zPageID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(LinkCategory.zPageID.EditValue) Then
	arwrk = LinkCategory.zPageID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), LinkCategory.zPageID.CurrentValue) Then
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
If emptywrk Then LinkCategory.zPageID.OldValue = ""
%>
</select>
<input type="hidden" name="o<%= LinkCategory_list.lRowIndex %>_zPageID" id="o<%= LinkCategory_list.lRowIndex %>_zPageID" value="<%= ew_HTMLEncode(LinkCategory.zPageID.OldValue) %>" />
<% End If %>
<% If LinkCategory.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<select id="x<%= LinkCategory_list.lRowIndex %>_zPageID" name="x<%= LinkCategory_list.lRowIndex %>_zPageID"<%= LinkCategory.zPageID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(LinkCategory.zPageID.EditValue) Then
	arwrk = LinkCategory.zPageID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), LinkCategory.zPageID.CurrentValue) Then
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
If emptywrk Then LinkCategory.zPageID.OldValue = ""
%>
</select>
<% End If %>
<% If LinkCategory.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= LinkCategory.zPageID.ViewAttributes %>><%= LinkCategory.zPageID.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	</tr>
<% If LinkCategory.RowType = EW_ROWTYPE_ADD Then %>
<% End If %>
<% If LinkCategory.RowType = EW_ROWTYPE_EDIT Then %>
<% End If %>
<%
	End If
Loop
%>
</tbody>
</table>
<% End If %>
<% If LinkCategory.CurrentAction = "gridadd" Then %>
<input type="hidden" name="a_list" id="a_list" value="gridinsert" />
<input type="hidden" name="key_count" id="key_count" value="<%= LinkCategory_list.lRowIndex %>" />
<% End If %>
<% If LinkCategory.CurrentAction = "edit" Then %>
<input type="hidden" name="key_count" id="key_count" value="<%= LinkCategory_list.lRowIndex %>" />
<% End If %>
<% If LinkCategory.CurrentAction = "gridedit" Then %>
<input type="hidden" name="a_list" id="a_list" value="gridupdate" />
<input type="hidden" name="key_count" id="key_count" value="<%= LinkCategory_list.lRowIndex %>" />
<%= LinkCategory_list.sMultiSelectKey %>
<% End If %>
</form>
<%

' Close recordset
Rs.Close()
Rs.Dispose()
%>
</div>
<% If LinkCategory_list.lTotalRecs > 0 Then %>
<% If LinkCategory.Export = "" Then %>
<div class="ewGridLowerPanel">
<% If LinkCategory.CurrentAction <> "gridadd" AndAlso LinkCategory.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If LinkCategory_list.Pager Is Nothing Then LinkCategory_list.Pager = New cPrevNextPager(LinkCategory_list.lStartRec, LinkCategory_list.lDisplayRecs, LinkCategory_list.lTotalRecs) %>
<% If LinkCategory_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If LinkCategory_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= LinkCategory_list.PageUrl %>start=<%= LinkCategory_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If LinkCategory_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= LinkCategory_list.PageUrl %>start=<%= LinkCategory_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= LinkCategory_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If LinkCategory_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= LinkCategory_list.PageUrl %>start=<%= LinkCategory_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If LinkCategory_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= LinkCategory_list.PageUrl %>start=<%= LinkCategory_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= LinkCategory_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= LinkCategory_list.Pager.FromIndex %> to <%= LinkCategory_list.Pager.ToIndex %> of <%= LinkCategory_list.Pager.RecordCount %></span>
<% Else %>
	<% If LinkCategory_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If LinkCategory_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="LinkCategory" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If LinkCategory_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If LinkCategory_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If LinkCategory_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If LinkCategory.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<% 'If LinkCategory_list.lTotalRecs > 0 Then %>
<div class="aspnetmaker">
<% If LinkCategory.CurrentAction <> "gridadd" AndAlso LinkCategory.CurrentAction <> "gridedit" Then ' Not grid add/edit mode %>
<a href="<%= LinkCategory.AddUrl %>">Add</a>&nbsp;&nbsp;
<a href="<%= LinkCategory_list.PageUrl %>a=gridadd">Grid Add</a>&nbsp;&nbsp;
<% If LinkCategory_list.lTotalRecs > 0 Then %>
<a href="<%= LinkCategory_list.PageUrl %>a=gridedit">Grid Edit</a>&nbsp;&nbsp;
<% End If %>
<% Else ' Grid add/edit mode %>
<% If LinkCategory.CurrentAction = "gridadd" Then %>
<a href="" onclick="f=ew_GetForm('fLinkCategorylist');if (LinkCategory_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/insert.gif' alt='Insert' title='Insert' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<% If LinkCategory.CurrentAction = "gridedit" Then %>
<a href="" onclick="f=ew_GetForm('fLinkCategorylist');if (LinkCategory_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/update.gif' alt='Save' title='Save' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<a href="<%= LinkCategory_list.PageUrl %>a=cancel"><img src='images/cancel.gif' alt='Cancel' title='Cancel' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
</div>
<% 'End If %>
</div>
<% End If %>
<% End If %>
</td></tr></table>
<% If LinkCategory.Export = "" AndAlso LinkCategory.CurrentAction = "" Then %>
<script type="text/javascript">
<!--
//ew_ToggleSearchPanel(LinkCategory_list); // uncomment to init search panel as collapsed
//-->
</script>
<% End If %>
<% If LinkCategory.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
