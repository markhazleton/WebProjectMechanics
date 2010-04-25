<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="SiteLink_list.aspx.vb" Inherits="SiteLink_list" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If SiteLink.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var SiteLink_list = new ew_Page("SiteLink_list");
// page properties
SiteLink_list.PageID = "list"; // page ID
var EW_PAGE_ID = SiteLink_list.PageID; // for backward compatibility
// extend page with ValidateForm function
SiteLink_list.ValidateForm = function(fobj) {
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
		elm = fobj.elements["x" + infix + "_Title"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field - Title");
		elm = fobj.elements["x" + infix + "_LinkTypeCD"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field - Link Type");
		elm = fobj.elements["x" + infix + "_CategoryID"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field - Link Category");
		elm = fobj.elements["x" + infix + "_Ranks"];
		if (elm && !ew_CheckInteger(elm.value))
			return ew_OnError(this, elm, "Incorrect integer - Ranks");
		elm = fobj.elements["x" + infix + "_Views"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field - Active/Visible");
		} // End Grid Add checking
	}
	if (fobj.a_list && fobj.a_list.value == "gridinsert" && addcnt == 0) { // No row added
		alert("No records to be added");
		return false;
	}
	return true;
}
// Extend page with empty row check
SiteLink_list.EmptyRow = function(fobj, infix) {
	if (ew_ValueChanged(fobj, infix, "SiteCategoryTypeID")) return false;
	if (ew_ValueChanged(fobj, infix, "Title")) return false;
	if (ew_ValueChanged(fobj, infix, "LinkTypeCD")) return false;
	if (ew_ValueChanged(fobj, infix, "CategoryID")) return false;
	if (ew_ValueChanged(fobj, infix, "CompanyID")) return false;
	if (ew_ValueChanged(fobj, infix, "SiteCategoryID")) return false;
	if (ew_ValueChanged(fobj, infix, "SiteCategoryGroupID")) return false;
	if (ew_ValueChanged(fobj, infix, "Ranks")) return false;
	if (ew_ValueChanged(fobj, infix, "Views")) return false;
	return true;
}
<% If EW_CLIENT_VALIDATE Then %>
SiteLink_list.ValidateRequired = true; // uses JavaScript validation
<% Else %>
SiteLink_list.ValidateRequired = false; // no JavaScript validation
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
<% If SiteLink.Export = "" Then %>
<% End If %>
<%
If SiteLink.CurrentAction = "gridadd" Then SiteLink.CurrentFilter = "0=1"

' Load recordset
Rs = SiteLink_list.LoadRecordset()
If SiteLink.CurrentAction = "gridadd" Then
	SiteLink_list.lStartRec = 1
	If SiteLink_list.lDisplayRecs <= 0 Then SiteLink_list.lDisplayRecs = 25
	SiteLink_list.lTotalRecs = SiteLink_list.lDisplayRecs
	SiteLink_list.lStopRec = SiteLink_list.lDisplayRecs
Else
	SiteLink_list.lStartRec = 1
	If SiteLink_list.lDisplayRecs <= 0 Then ' Display all records
		SiteLink_list.lDisplayRecs = SiteLink_list.lTotalRecs
	End If
	If Not (SiteLink.ExportAll AndAlso SiteLink.Export <> "") Then
		SiteLink_list.SetUpStartRec() ' Set up start record position
	End If
End If
%>
<p><span class="aspnetmaker" style="white-space: nowrap;">TABLE: Site Type Parts
<% If SiteLink.Export = "" AndAlso SiteLink.CurrentAction = "" Then %>
&nbsp;&nbsp;<a href="<%= SiteLink_list.PageUrl %>export=excel"><img src='images/exportxls.gif' alt='Export to Excel' title='Export to Excel' width='16' height='16' border='0'></a>
<% End If %>
</span></p>
<% If SiteLink.Export = "" AndAlso SiteLink.CurrentAction = "" Then %>
<a href="javascript:ew_ToggleSearchPanel(SiteLink_list);" style="text-decoration: none;"><img id="SiteLink_list_SearchImage" src="images/collapse.gif" alt="" width="9" height="9" border="0"></a><span class="aspnetmaker">&nbsp;Search</span><br>
<div id="SiteLink_list_SearchPanel">
<form name="fSiteLinklistsrch" id="fSiteLinklistsrch" class="ewForm">
<input type="hidden" id="t" name="t" value="SiteLink" />
<table class="ewBasicSearch">
	<tr>
		<td><span class="aspnetmaker">
			<input type="text" name="<%= EW_TABLE_BASIC_SEARCH %>" id="<%= EW_TABLE_BASIC_SEARCH %>" size="20" value="<%= ew_HtmlEncode(SiteLink.BasicSearchKeyword) %>" />
			<input type="Submit" name="Submit" id="Submit" value="Search (*)" />&nbsp;
			<a href="<%= SiteLink_list.PageUrl %>cmd=reset">Show all</a>&nbsp;
			<a href="SiteLink_srch.aspx">Advanced Search</a>&nbsp;
		</span></td>
	</tr>
	<tr>
	<td><span class="aspnetmaker"><label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value=""<% If SiteLink.BasicSearchType = "" Then %> checked="checked"<% End If %> />Exact phrase</label>&nbsp;&nbsp;<label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value="AND"<% If SiteLink.BasicSearchType = "AND" Then %> checked="checked"<% End If %> />All words</label>&nbsp;&nbsp;<label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value="OR"<% If SiteLink.BasicSearchType = "OR" Then %> checked="checked"<% End If %> />Any word</label></span></td>
	</tr>
</table>
</form>
</div>
<% End If %>
<% SiteLink_list.ShowMessage() %>
<br />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<% If SiteLink.Export = "" Then %>
<div class="ewGridUpperPanel">
<% If SiteLink.CurrentAction <> "gridadd" AndAlso SiteLink.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If SiteLink_list.Pager Is Nothing Then SiteLink_list.Pager = New cPrevNextPager(SiteLink_list.lStartRec, SiteLink_list.lDisplayRecs, SiteLink_list.lTotalRecs) %>
<% If SiteLink_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If SiteLink_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= SiteLink_list.PageUrl %>start=<%= SiteLink_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If SiteLink_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= SiteLink_list.PageUrl %>start=<%= SiteLink_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= SiteLink_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If SiteLink_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= SiteLink_list.PageUrl %>start=<%= SiteLink_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If SiteLink_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= SiteLink_list.PageUrl %>start=<%= SiteLink_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= SiteLink_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= SiteLink_list.Pager.FromIndex %> to <%= SiteLink_list.Pager.ToIndex %> of <%= SiteLink_list.Pager.RecordCount %></span>
<% Else %>
	<% If SiteLink_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If SiteLink_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="SiteLink" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If SiteLink_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If SiteLink_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If SiteLink_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If SiteLink.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<div class="aspnetmaker">
<% If SiteLink.CurrentAction <> "gridadd" AndAlso SiteLink.CurrentAction <> "gridedit" Then ' Not grid add/edit mode %>
<a href="<%= SiteLink.AddUrl %>">Add</a>&nbsp;&nbsp;
<a href="<%= SiteLink_list.PageUrl %>a=gridadd">Grid Add</a>&nbsp;&nbsp;
<% If SiteLink_list.lTotalRecs > 0 Then %>
<a href="<%= SiteLink_list.PageUrl %>a=gridedit">Grid Edit</a>&nbsp;&nbsp;
<% End If %>
<% Else ' Grid add/edit mode %>
<% If SiteLink.CurrentAction = "gridadd" Then %>
<a href="" onclick="f=ew_GetForm('fSiteLinklist');if (SiteLink_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/insert.gif' alt='Insert' title='Insert' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<% If SiteLink.CurrentAction = "gridedit" Then %>
<a href="" onclick="f=ew_GetForm('fSiteLinklist');if (SiteLink_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/update.gif' alt='Save' title='Save' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<a href="<%= SiteLink_list.PageUrl %>a=cancel"><img src='images/cancel.gif' alt='Cancel' title='Cancel' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
</div>
</div>
<% End If %>
<div class="ewGridMiddlePanel">
<form name="fSiteLinklist" id="fSiteLinklist" class="ewForm" method="post">
<input type="hidden" name="t" id="t" value="SiteLink" />
<% If SiteLink_list.lTotalRecs > 0 Then %>
<table cellspacing="0" rowhighlightclass="ewTableHighlightRow" rowselectclass="ewTableSelectRow" roweditclass="ewTableEditRow" class="ewTable ewTableSeparate">
<%
	SiteLink_list.lOptionCnt = 0
	SiteLink_list.lOptionCnt = SiteLink_list.lOptionCnt + 1 ' View
	SiteLink_list.lOptionCnt = SiteLink_list.lOptionCnt + 1 ' Edit
	SiteLink_list.lOptionCnt = SiteLink_list.lOptionCnt + 1 ' Copy
	SiteLink_list.lOptionCnt = SiteLink_list.lOptionCnt + 1 ' Delete
	SiteLink_list.lOptionCnt = SiteLink_list.lOptionCnt + SiteLink_list.ListOptions.Items.Count ' Custom list options
%>
<%= SiteLink.TableCustomInnerHTML %>
<thead><!-- Table header -->
	<tr class="ewTableHeader">
<% If SiteLink.Export = "" Then %>
<% If SiteLink.CurrentAction <> "gridadd" AndAlso SiteLink.CurrentAction <> "gridedit" Then %>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<%

' Custom list options
For i As Integer = 0 to SiteLink_list.ListOptions.Items.Count -1
	If SiteLink_list.ListOptions.Items(i).Visible Then Response.Write(SiteLink_list.ListOptions.Items(i).HeaderCellHtml)
Next
%>
<% End If %>
<% End If %>
<% If SiteLink.SiteCategoryTypeID.Visible Then ' SiteCategoryTypeID %>
	<% If SiteLink.SortUrl(SiteLink.SiteCategoryTypeID) = "" Then %>
		<td>Site Type</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteLink.SortUrl(SiteLink.SiteCategoryTypeID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Site Type</td><td style="width: 10px;"><% If SiteLink.SiteCategoryTypeID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteLink.SiteCategoryTypeID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If SiteLink.Title.Visible Then ' Title %>
	<% If SiteLink.SortUrl(SiteLink.Title) = "" Then %>
		<td>Title</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteLink.SortUrl(SiteLink.Title) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Title&nbsp;(*)</td><td style="width: 10px;"><% If SiteLink.Title.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteLink.Title.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If SiteLink.LinkTypeCD.Visible Then ' LinkTypeCD %>
	<% If SiteLink.SortUrl(SiteLink.LinkTypeCD) = "" Then %>
		<td>Link Type</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteLink.SortUrl(SiteLink.LinkTypeCD) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Link Type</td><td style="width: 10px;"><% If SiteLink.LinkTypeCD.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteLink.LinkTypeCD.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If SiteLink.CategoryID.Visible Then ' CategoryID %>
	<% If SiteLink.SortUrl(SiteLink.CategoryID) = "" Then %>
		<td>Link Category</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteLink.SortUrl(SiteLink.CategoryID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Link Category</td><td style="width: 10px;"><% If SiteLink.CategoryID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteLink.CategoryID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If SiteLink.CompanyID.Visible Then ' CompanyID %>
	<% If SiteLink.SortUrl(SiteLink.CompanyID) = "" Then %>
		<td>Company</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteLink.SortUrl(SiteLink.CompanyID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Company</td><td style="width: 10px;"><% If SiteLink.CompanyID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteLink.CompanyID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If SiteLink.SiteCategoryID.Visible Then ' SiteCategoryID %>
	<% If SiteLink.SortUrl(SiteLink.SiteCategoryID) = "" Then %>
		<td>Site Category</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteLink.SortUrl(SiteLink.SiteCategoryID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Site Category</td><td style="width: 10px;"><% If SiteLink.SiteCategoryID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteLink.SiteCategoryID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If SiteLink.SiteCategoryGroupID.Visible Then ' SiteCategoryGroupID %>
	<% If SiteLink.SortUrl(SiteLink.SiteCategoryGroupID) = "" Then %>
		<td>Category Group</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteLink.SortUrl(SiteLink.SiteCategoryGroupID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Category Group</td><td style="width: 10px;"><% If SiteLink.SiteCategoryGroupID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteLink.SiteCategoryGroupID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If SiteLink.Ranks.Visible Then ' Ranks %>
	<% If SiteLink.SortUrl(SiteLink.Ranks) = "" Then %>
		<td>Ranks</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteLink.SortUrl(SiteLink.Ranks) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Ranks</td><td style="width: 10px;"><% If SiteLink.Ranks.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteLink.Ranks.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If SiteLink.Views.Visible Then ' Views %>
	<% If SiteLink.SortUrl(SiteLink.Views) = "" Then %>
		<td>Active/Visible</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteLink.SortUrl(SiteLink.Views) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Active/Visible</td><td style="width: 10px;"><% If SiteLink.Views.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteLink.Views.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
	</tr>
</thead>
<tbody><!-- Table body -->
<%
If (SiteLink.ExportAll AndAlso SiteLink.Export <> "") Then
	SiteLink_list.lStopRec = SiteLink_list.lTotalRecs
Else
	SiteLink_list.lStopRec = SiteLink_list.lStartRec + SiteLink_list.lDisplayRecs - 1 ' Set the last record to display
End If
If SiteLink.CurrentAction = "gridadd" AndAlso SiteLink_list.lStopRec = -1 Then
	SiteLink_list.lStopRec = EW_GRIDADD_ROWS
End If 

' Move to first record
For i As Integer = 1 to SiteLink_list.lStartRec - 1
	If Rs.Read() Then	SiteLink_list.lRecCnt = SiteLink_list.lRecCnt + 1
Next		
SiteLink_list.lRowCnt = 0
If SiteLink.CurrentAction = "gridadd" Then SiteLink_list.lRowIndex = 0
If SiteLink.CurrentAction = "gridedit" Then SiteLink_list.lRowIndex = 0

' Output data rows
Do While (SiteLink.CurrentAction = "gridadd" OrElse Rs.Read()) AndAlso (SiteLink_list.lRecCnt < SiteLink_list.lStopRec)
	SiteLink_list.lRecCnt = SiteLink_list.lRecCnt + 1
	If SiteLink_list.lRecCnt >= SiteLink_list.lStartRec Then
		SiteLink_list.lRowCnt = SiteLink_list.lRowCnt + 1
		If SiteLink.CurrentAction = "gridadd" OrElse SiteLink.CurrentAction = "gridedit" Then SiteLink_list.lRowIndex = SiteLink_list.lRowIndex + 1
	SiteLink.CssClass = ""
	SiteLink.CssStyle = ""
	SiteLink.RowClientEvents = "onmouseover='ew_MouseOver(event, this);' onmouseout='ew_MouseOut(event, this);' onclick='ew_Click(event, this);'"
	If SiteLink.CurrentAction = "gridadd" Then
		SiteLink_list.LoadDefaultValues() ' Load default values
	Else
		SiteLink_list.LoadRowValues(Rs) ' Load row values
	End If
	SiteLink.RowType = EW_ROWTYPE_VIEW ' Render view
	If SiteLink.CurrentAction = "gridadd" Then ' Grid add
		SiteLink.RowType = EW_ROWTYPE_ADD ' Render add
	End If
	If SiteLink.CurrentAction = "gridadd" AndAlso SiteLink.EventCancelled Then ' Insert failed
		SiteLink_list.RestoreCurrentRowFormValues(SiteLink_list.lRowIndex) ' Restore form values
	End If
	If SiteLink.CurrentAction = "gridedit" Then ' Grid edit
		SiteLink.RowType = EW_ROWTYPE_EDIT ' Render edit
	End If
	If SiteLink.RowType = EW_ROWTYPE_EDIT AndAlso SiteLink.EventCancelled Then ' update failed
		If SiteLink.CurrentAction = "gridedit" Then
			SiteLink_list.RestoreCurrentRowFormValues(SiteLink_list.lRowIndex) ' Restore form values
		End If
	End If
	If SiteLink.RowType = EW_ROWTYPE_EDIT Then ' Edit row
		SiteLink_list.lEditRowCnt = SiteLink_list.lEditRowCnt + 1
		SiteLink.RowClientEvents = "onmouseover='this.edit=true;ew_MouseOver(event, this);' onmouseout='ew_MouseOut(event, this);' onclick='ew_Click(event, this);'"
	End If
	If SiteLink.RowType = EW_ROWTYPE_ADD OrElse SiteLink.RowType = EW_ROWTYPE_EDIT Then ' Add / Edit row
		SiteLink.CssClass = "ewTableEditRow"
	End If

	' Render row
	SiteLink_list.RenderRow()
%>
	<tr<%= SiteLink.RowAttributes %>>
<% If SiteLink.RowType = EW_ROWTYPE_ADD OrElse SiteLink.RowType = EW_ROWTYPE_EDIT Then %>
<%
	If SiteLink.CurrentAction = "gridedit" Then
		SiteLink_list.sMultiSelectKey = SiteLink_list.sMultiSelectKey & "<input type=""hidden"" name=""k" & SiteLink_list.lRowIndex & "_key"" id=""k" & SiteLink_list.lRowIndex & "_key"" value=""" & SiteLink.ID.CurrentValue & """ />"
	End If
%>
<% Else %>
<% If SiteLink.Export = "" Then %>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= SiteLink.ViewUrl %>"><img src='images/view.gif' alt='View' title='View' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= SiteLink.EditUrl %>"><img src='images/edit.gif' alt='Edit' title='Edit' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= SiteLink.CopyUrl %>"><img src='images/copy.gif' alt='Copy' title='Copy' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= SiteLink.DeleteUrl %>"><img src='images/delete.gif' alt='Delete' title='Delete' width='16' height='16' border='0'></a>
</span></td>
<%

' Custom list options
For i As Integer = 0 to SiteLink_list.ListOptions.Items.Count -1
	If SiteLink_list.ListOptions.Items(i).Visible Then Response.Write(SiteLink_list.ListOptions.Items(i).BodyCellHtml)
Next
%>
<% End If %>
<% End If %>
	<% If SiteLink.SiteCategoryTypeID.Visible Then ' SiteCategoryTypeID %>
		<td<%= SiteLink.SiteCategoryTypeID.CellAttributes %>>
<% If SiteLink.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<select id="x<%= SiteLink_list.lRowIndex %>_SiteCategoryTypeID" name="x<%= SiteLink_list.lRowIndex %>_SiteCategoryTypeID" onchange="ew_UpdateOpt('x<%= SiteLink_list.lRowIndex %>_SiteCategoryID','x<%= SiteLink_list.lRowIndex %>_SiteCategoryTypeID',SiteLink_list.ar_x<%= SiteLink_list.lRowIndex %>_SiteCategoryID);"<%= SiteLink.SiteCategoryTypeID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(SiteLink.SiteCategoryTypeID.EditValue) Then
	arwrk = SiteLink.SiteCategoryTypeID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), SiteLink.SiteCategoryTypeID.CurrentValue) Then
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
If emptywrk Then SiteLink.SiteCategoryTypeID.OldValue = ""
%>
</select>
<input type="hidden" name="o<%= SiteLink_list.lRowIndex %>_SiteCategoryTypeID" id="o<%= SiteLink_list.lRowIndex %>_SiteCategoryTypeID" value="<%= ew_HTMLEncode(SiteLink.SiteCategoryTypeID.OldValue) %>" />
<% End If %>
<% If SiteLink.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<select id="x<%= SiteLink_list.lRowIndex %>_SiteCategoryTypeID" name="x<%= SiteLink_list.lRowIndex %>_SiteCategoryTypeID" onchange="ew_UpdateOpt('x<%= SiteLink_list.lRowIndex %>_SiteCategoryID','x<%= SiteLink_list.lRowIndex %>_SiteCategoryTypeID',SiteLink_list.ar_x<%= SiteLink_list.lRowIndex %>_SiteCategoryID);"<%= SiteLink.SiteCategoryTypeID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(SiteLink.SiteCategoryTypeID.EditValue) Then
	arwrk = SiteLink.SiteCategoryTypeID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), SiteLink.SiteCategoryTypeID.CurrentValue) Then
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
If emptywrk Then SiteLink.SiteCategoryTypeID.OldValue = ""
%>
</select>
<% End If %>
<% If SiteLink.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= SiteLink.SiteCategoryTypeID.ViewAttributes %>><%= SiteLink.SiteCategoryTypeID.ListViewValue %></div>
<% End If %>
<% If SiteLink.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<input type="hidden" name="o<%= SiteLink_list.lRowIndex %>_ID" id="o<%= SiteLink_list.lRowIndex %>_ID" value="<%= ew_HTMLEncode(SiteLink.ID.OldValue) %>" />
<% End If %>
<% If SiteLink.RowType = EW_ROWTYPE_EDIT Then %>
<input type="hidden" name="x<%= SiteLink_list.lRowIndex %>_ID" id="x<%= SiteLink_list.lRowIndex %>_ID" value="<%= ew_HTMLEncode(SiteLink.ID.CurrentValue) %>" />
<% End If %>
</td>
	<% End If %>
	<% If SiteLink.Title.Visible Then ' Title %>
		<td<%= SiteLink.Title.CellAttributes %>>
<% If SiteLink.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<input type="text" name="x<%= SiteLink_list.lRowIndex %>_Title" id="x<%= SiteLink_list.lRowIndex %>_Title" size="30" maxlength="255" value="<%= SiteLink.Title.EditValue %>"<%= SiteLink.Title.EditAttributes %> />
<input type="hidden" name="o<%= SiteLink_list.lRowIndex %>_Title" id="o<%= SiteLink_list.lRowIndex %>_Title" value="<%= ew_HTMLEncode(SiteLink.Title.OldValue) %>" />
<% End If %>
<% If SiteLink.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<input type="text" name="x<%= SiteLink_list.lRowIndex %>_Title" id="x<%= SiteLink_list.lRowIndex %>_Title" size="30" maxlength="255" value="<%= SiteLink.Title.EditValue %>"<%= SiteLink.Title.EditAttributes %> />
<% End If %>
<% If SiteLink.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= SiteLink.Title.ViewAttributes %>><%= SiteLink.Title.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	<% If SiteLink.LinkTypeCD.Visible Then ' LinkTypeCD %>
		<td<%= SiteLink.LinkTypeCD.CellAttributes %>>
<% If SiteLink.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<select id="x<%= SiteLink_list.lRowIndex %>_LinkTypeCD" name="x<%= SiteLink_list.lRowIndex %>_LinkTypeCD"<%= SiteLink.LinkTypeCD.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(SiteLink.LinkTypeCD.EditValue) Then
	arwrk = SiteLink.LinkTypeCD.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), SiteLink.LinkTypeCD.CurrentValue) Then
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
If emptywrk Then SiteLink.LinkTypeCD.OldValue = ""
%>
</select>
<input type="hidden" name="o<%= SiteLink_list.lRowIndex %>_LinkTypeCD" id="o<%= SiteLink_list.lRowIndex %>_LinkTypeCD" value="<%= ew_HTMLEncode(SiteLink.LinkTypeCD.OldValue) %>" />
<% End If %>
<% If SiteLink.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<select id="x<%= SiteLink_list.lRowIndex %>_LinkTypeCD" name="x<%= SiteLink_list.lRowIndex %>_LinkTypeCD"<%= SiteLink.LinkTypeCD.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(SiteLink.LinkTypeCD.EditValue) Then
	arwrk = SiteLink.LinkTypeCD.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), SiteLink.LinkTypeCD.CurrentValue) Then
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
If emptywrk Then SiteLink.LinkTypeCD.OldValue = ""
%>
</select>
<% End If %>
<% If SiteLink.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= SiteLink.LinkTypeCD.ViewAttributes %>><%= SiteLink.LinkTypeCD.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	<% If SiteLink.CategoryID.Visible Then ' CategoryID %>
		<td<%= SiteLink.CategoryID.CellAttributes %>>
<% If SiteLink.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<select id="x<%= SiteLink_list.lRowIndex %>_CategoryID" name="x<%= SiteLink_list.lRowIndex %>_CategoryID"<%= SiteLink.CategoryID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(SiteLink.CategoryID.EditValue) Then
	arwrk = SiteLink.CategoryID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), SiteLink.CategoryID.CurrentValue) Then
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
If emptywrk Then SiteLink.CategoryID.OldValue = ""
%>
</select>
<input type="hidden" name="o<%= SiteLink_list.lRowIndex %>_CategoryID" id="o<%= SiteLink_list.lRowIndex %>_CategoryID" value="<%= ew_HTMLEncode(SiteLink.CategoryID.OldValue) %>" />
<% End If %>
<% If SiteLink.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<select id="x<%= SiteLink_list.lRowIndex %>_CategoryID" name="x<%= SiteLink_list.lRowIndex %>_CategoryID"<%= SiteLink.CategoryID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(SiteLink.CategoryID.EditValue) Then
	arwrk = SiteLink.CategoryID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), SiteLink.CategoryID.CurrentValue) Then
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
If emptywrk Then SiteLink.CategoryID.OldValue = ""
%>
</select>
<% End If %>
<% If SiteLink.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= SiteLink.CategoryID.ViewAttributes %>><%= SiteLink.CategoryID.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	<% If SiteLink.CompanyID.Visible Then ' CompanyID %>
		<td<%= SiteLink.CompanyID.CellAttributes %>>
<% If SiteLink.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<select id="x<%= SiteLink_list.lRowIndex %>_CompanyID" name="x<%= SiteLink_list.lRowIndex %>_CompanyID"<%= SiteLink.CompanyID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(SiteLink.CompanyID.EditValue) Then
	arwrk = SiteLink.CompanyID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), SiteLink.CompanyID.CurrentValue) Then
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
If emptywrk Then SiteLink.CompanyID.OldValue = ""
%>
</select>
<input type="hidden" name="o<%= SiteLink_list.lRowIndex %>_CompanyID" id="o<%= SiteLink_list.lRowIndex %>_CompanyID" value="<%= ew_HTMLEncode(SiteLink.CompanyID.OldValue) %>" />
<% End If %>
<% If SiteLink.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<select id="x<%= SiteLink_list.lRowIndex %>_CompanyID" name="x<%= SiteLink_list.lRowIndex %>_CompanyID"<%= SiteLink.CompanyID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(SiteLink.CompanyID.EditValue) Then
	arwrk = SiteLink.CompanyID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), SiteLink.CompanyID.CurrentValue) Then
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
If emptywrk Then SiteLink.CompanyID.OldValue = ""
%>
</select>
<% End If %>
<% If SiteLink.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= SiteLink.CompanyID.ViewAttributes %>><%= SiteLink.CompanyID.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	<% If SiteLink.SiteCategoryID.Visible Then ' SiteCategoryID %>
		<td<%= SiteLink.SiteCategoryID.CellAttributes %>>
<% If SiteLink.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<select id="x<%= SiteLink_list.lRowIndex %>_SiteCategoryID" name="x<%= SiteLink_list.lRowIndex %>_SiteCategoryID"<%= SiteLink.SiteCategoryID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(SiteLink.SiteCategoryID.EditValue) Then
	arwrk = SiteLink.SiteCategoryID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), SiteLink.SiteCategoryID.CurrentValue) Then
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
If emptywrk Then SiteLink.SiteCategoryID.OldValue = ""
%>
</select>
<%
jswrk = "" ' Initialise
If ew_IsArrayList(SiteLink.SiteCategoryID.EditValue) Then
	arwrk = SiteLink.SiteCategoryID.EditValue
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
SiteLink_list.ar_x<%= SiteLink_list.lRowIndex %>_SiteCategoryID = [<%= jswrk %>];
//-->
</script>
<input type="hidden" name="o<%= SiteLink_list.lRowIndex %>_SiteCategoryID" id="o<%= SiteLink_list.lRowIndex %>_SiteCategoryID" value="<%= ew_HTMLEncode(SiteLink.SiteCategoryID.OldValue) %>" />
<% End If %>
<% If SiteLink.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<select id="x<%= SiteLink_list.lRowIndex %>_SiteCategoryID" name="x<%= SiteLink_list.lRowIndex %>_SiteCategoryID"<%= SiteLink.SiteCategoryID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(SiteLink.SiteCategoryID.EditValue) Then
	arwrk = SiteLink.SiteCategoryID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), SiteLink.SiteCategoryID.CurrentValue) Then
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
If emptywrk Then SiteLink.SiteCategoryID.OldValue = ""
%>
</select>
<%
jswrk = "" ' Initialise
If ew_IsArrayList(SiteLink.SiteCategoryID.EditValue) Then
	arwrk = SiteLink.SiteCategoryID.EditValue
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
SiteLink_list.ar_x<%= SiteLink_list.lRowIndex %>_SiteCategoryID = [<%= jswrk %>];
//-->
</script>
<% End If %>
<% If SiteLink.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= SiteLink.SiteCategoryID.ViewAttributes %>><%= SiteLink.SiteCategoryID.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	<% If SiteLink.SiteCategoryGroupID.Visible Then ' SiteCategoryGroupID %>
		<td<%= SiteLink.SiteCategoryGroupID.CellAttributes %>>
<% If SiteLink.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<select id="x<%= SiteLink_list.lRowIndex %>_SiteCategoryGroupID" name="x<%= SiteLink_list.lRowIndex %>_SiteCategoryGroupID"<%= SiteLink.SiteCategoryGroupID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(SiteLink.SiteCategoryGroupID.EditValue) Then
	arwrk = SiteLink.SiteCategoryGroupID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), SiteLink.SiteCategoryGroupID.CurrentValue) Then
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
If emptywrk Then SiteLink.SiteCategoryGroupID.OldValue = ""
%>
</select>
<input type="hidden" name="o<%= SiteLink_list.lRowIndex %>_SiteCategoryGroupID" id="o<%= SiteLink_list.lRowIndex %>_SiteCategoryGroupID" value="<%= ew_HTMLEncode(SiteLink.SiteCategoryGroupID.OldValue) %>" />
<% End If %>
<% If SiteLink.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<select id="x<%= SiteLink_list.lRowIndex %>_SiteCategoryGroupID" name="x<%= SiteLink_list.lRowIndex %>_SiteCategoryGroupID"<%= SiteLink.SiteCategoryGroupID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(SiteLink.SiteCategoryGroupID.EditValue) Then
	arwrk = SiteLink.SiteCategoryGroupID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), SiteLink.SiteCategoryGroupID.CurrentValue) Then
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
If emptywrk Then SiteLink.SiteCategoryGroupID.OldValue = ""
%>
</select>
<% End If %>
<% If SiteLink.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= SiteLink.SiteCategoryGroupID.ViewAttributes %>><%= SiteLink.SiteCategoryGroupID.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	<% If SiteLink.Ranks.Visible Then ' Ranks %>
		<td<%= SiteLink.Ranks.CellAttributes %>>
<% If SiteLink.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<input type="text" name="x<%= SiteLink_list.lRowIndex %>_Ranks" id="x<%= SiteLink_list.lRowIndex %>_Ranks" size="30" value="<%= SiteLink.Ranks.EditValue %>"<%= SiteLink.Ranks.EditAttributes %> />
<input type="hidden" name="o<%= SiteLink_list.lRowIndex %>_Ranks" id="o<%= SiteLink_list.lRowIndex %>_Ranks" value="<%= ew_HTMLEncode(SiteLink.Ranks.OldValue) %>" />
<% End If %>
<% If SiteLink.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<input type="text" name="x<%= SiteLink_list.lRowIndex %>_Ranks" id="x<%= SiteLink_list.lRowIndex %>_Ranks" size="30" value="<%= SiteLink.Ranks.EditValue %>"<%= SiteLink.Ranks.EditAttributes %> />
<% End If %>
<% If SiteLink.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= SiteLink.Ranks.ViewAttributes %>><%= SiteLink.Ranks.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	<% If SiteLink.Views.Visible Then ' Views %>
		<td<%= SiteLink.Views.CellAttributes %>>
<% If SiteLink.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<%
If ew_SameStr(SiteLink.Views.CurrentValue, "1") Then
	selwrk = " checked=""checked"""
Else
	selwrk = ""
End If
%>
<input type="checkbox" name="x<%= SiteLink_list.lRowIndex %>_Views" id="x<%= SiteLink_list.lRowIndex %>_Views" value="1"<%= selwrk %><%= SiteLink.Views.EditAttributes %> />
<input type="hidden" name="o<%= SiteLink_list.lRowIndex %>_Views" id="o<%= SiteLink_list.lRowIndex %>_Views" value="<%= ew_HTMLEncode(SiteLink.Views.OldValue) %>" />
<% End If %>
<% If SiteLink.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<%
If ew_SameStr(SiteLink.Views.CurrentValue, "1") Then
	selwrk = " checked=""checked"""
Else
	selwrk = ""
End If
%>
<input type="checkbox" name="x<%= SiteLink_list.lRowIndex %>_Views" id="x<%= SiteLink_list.lRowIndex %>_Views" value="1"<%= selwrk %><%= SiteLink.Views.EditAttributes %> />
<% End If %>
<% If SiteLink.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<% If Convert.ToString(SiteLink.Views.CurrentValue) = "1" Then %>
<input type="checkbox" value="<%= SiteLink.Views.ListViewValue %>" checked onclick="this.form.reset();" disabled="disabled" />
<% Else %>
<input type="checkbox" value="<%= SiteLink.Views.ListViewValue %>" onclick="this.form.reset();" disabled="disabled" />
<% End If %>
<% End If %>
</td>
	<% End If %>
	</tr>
<% If SiteLink.RowType = EW_ROWTYPE_ADD Then %>
<script language="JavaScript">
<!--
ew_UpdateOpts([['x<%= SiteLink_list.lRowIndex %>_SiteCategoryID','x<%= SiteLink_list.lRowIndex %>_SiteCategoryTypeID',SiteLink_list.ar_x<%= SiteLink_list.lRowIndex %>_SiteCategoryID]]);
//-->
</script>
<% End If %>
<% If SiteLink.RowType = EW_ROWTYPE_EDIT Then %>
<script language="JavaScript">
<!--
ew_UpdateOpts([['x<%= SiteLink_list.lRowIndex %>_SiteCategoryID','x<%= SiteLink_list.lRowIndex %>_SiteCategoryTypeID',SiteLink_list.ar_x<%= SiteLink_list.lRowIndex %>_SiteCategoryID]]);
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
<% If SiteLink.CurrentAction = "gridadd" Then %>
<input type="hidden" name="a_list" id="a_list" value="gridinsert" />
<input type="hidden" name="key_count" id="key_count" value="<%= SiteLink_list.lRowIndex %>" />
<% End If %>
<% If SiteLink.CurrentAction = "gridedit" Then %>
<input type="hidden" name="a_list" id="a_list" value="gridupdate" />
<input type="hidden" name="key_count" id="key_count" value="<%= SiteLink_list.lRowIndex %>" />
<%= SiteLink_list.sMultiSelectKey %>
<% End If %>
</form>
<%

' Close recordset
Rs.Close()
Rs.Dispose()
%>
</div>
<% If SiteLink_list.lTotalRecs > 0 Then %>
<% If SiteLink.Export = "" Then %>
<div class="ewGridLowerPanel">
<% If SiteLink.CurrentAction <> "gridadd" AndAlso SiteLink.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If SiteLink_list.Pager Is Nothing Then SiteLink_list.Pager = New cPrevNextPager(SiteLink_list.lStartRec, SiteLink_list.lDisplayRecs, SiteLink_list.lTotalRecs) %>
<% If SiteLink_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If SiteLink_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= SiteLink_list.PageUrl %>start=<%= SiteLink_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If SiteLink_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= SiteLink_list.PageUrl %>start=<%= SiteLink_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= SiteLink_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If SiteLink_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= SiteLink_list.PageUrl %>start=<%= SiteLink_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If SiteLink_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= SiteLink_list.PageUrl %>start=<%= SiteLink_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= SiteLink_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= SiteLink_list.Pager.FromIndex %> to <%= SiteLink_list.Pager.ToIndex %> of <%= SiteLink_list.Pager.RecordCount %></span>
<% Else %>
	<% If SiteLink_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If SiteLink_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="SiteLink" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If SiteLink_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If SiteLink_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If SiteLink_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If SiteLink.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<% 'If SiteLink_list.lTotalRecs > 0 Then %>
<div class="aspnetmaker">
<% If SiteLink.CurrentAction <> "gridadd" AndAlso SiteLink.CurrentAction <> "gridedit" Then ' Not grid add/edit mode %>
<a href="<%= SiteLink.AddUrl %>">Add</a>&nbsp;&nbsp;
<a href="<%= SiteLink_list.PageUrl %>a=gridadd">Grid Add</a>&nbsp;&nbsp;
<% If SiteLink_list.lTotalRecs > 0 Then %>
<a href="<%= SiteLink_list.PageUrl %>a=gridedit">Grid Edit</a>&nbsp;&nbsp;
<% End If %>
<% Else ' Grid add/edit mode %>
<% If SiteLink.CurrentAction = "gridadd" Then %>
<a href="" onclick="f=ew_GetForm('fSiteLinklist');if (SiteLink_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/insert.gif' alt='Insert' title='Insert' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<% If SiteLink.CurrentAction = "gridedit" Then %>
<a href="" onclick="f=ew_GetForm('fSiteLinklist');if (SiteLink_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/update.gif' alt='Save' title='Save' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<a href="<%= SiteLink_list.PageUrl %>a=cancel"><img src='images/cancel.gif' alt='Cancel' title='Cancel' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
</div>
<% 'End If %>
</div>
<% End If %>
<% End If %>
</td></tr></table>
<% If SiteLink.Export = "" AndAlso SiteLink.CurrentAction = "" Then %>
<script type="text/javascript">
<!--
//ew_ToggleSearchPanel(SiteLink_list); // uncomment to init search panel as collapsed
//-->
</script>
<% End If %>
<% If SiteLink.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
