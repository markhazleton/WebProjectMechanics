<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="CompanySiteTypeParameter_list.aspx.vb" Inherits="CompanySiteTypeParameter_list" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<%@ Register TagPrefix="WPMGen" TagName="MasterTable_SiteParameterType" Src="SiteParameterType_master.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If CompanySiteTypeParameter.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var CompanySiteTypeParameter_list = new ew_Page("CompanySiteTypeParameter_list");
// page properties
CompanySiteTypeParameter_list.PageID = "list"; // page ID
var EW_PAGE_ID = CompanySiteTypeParameter_list.PageID; // for backward compatibility
// extend page with ValidateForm function
CompanySiteTypeParameter_list.ValidateForm = function(fobj) {
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
		elm = fobj.elements["x" + infix + "_SiteParameterTypeID"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field - Parameter");
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
CompanySiteTypeParameter_list.EmptyRow = function(fobj, infix) {
	if (ew_ValueChanged(fobj, infix, "SiteParameterTypeID")) return false;
	if (ew_ValueChanged(fobj, infix, "CompanyID")) return false;
	if (ew_ValueChanged(fobj, infix, "SiteCategoryTypeID")) return false;
	if (ew_ValueChanged(fobj, infix, "SiteCategoryGroupID")) return false;
	if (ew_ValueChanged(fobj, infix, "SiteCategoryID")) return false;
	if (ew_ValueChanged(fobj, infix, "SortOrder")) return false;
	return true;
}
<% If EW_CLIENT_VALIDATE Then %>
CompanySiteTypeParameter_list.ValidateRequired = true; // uses JavaScript validation
<% Else %>
CompanySiteTypeParameter_list.ValidateRequired = false; // no JavaScript validation
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
<% If CompanySiteTypeParameter.Export = "" Then %>
<%
gsMasterReturnUrl = "SiteParameterType_list.aspx"
If CompanySiteTypeParameter_list.sDbMasterFilter <> "" AndAlso CompanySiteTypeParameter.CurrentMasterTable = "SiteParameterType" Then
	If CompanySiteTypeParameter_list.bMasterRecordExists Then
		If CompanySiteTypeParameter.CurrentMasterTable = CompanySiteTypeParameter.TableVar Then gsMasterReturnUrl = gsMasterReturnUrl & "?" & EW_TABLE_SHOW_MASTER & "="
%>
<WPMGen:MasterTable_SiteParameterType id="MasterTable_SiteParameterType" runat="server" />
<%
	End If
End If
%>
<% End If %>
<%
If CompanySiteTypeParameter.CurrentAction = "gridadd" Then CompanySiteTypeParameter.CurrentFilter = "0=1"

' Load recordset
Rs = CompanySiteTypeParameter_list.LoadRecordset()
If CompanySiteTypeParameter.CurrentAction = "gridadd" Then
	CompanySiteTypeParameter_list.lStartRec = 1
	If CompanySiteTypeParameter_list.lDisplayRecs <= 0 Then CompanySiteTypeParameter_list.lDisplayRecs = 25
	CompanySiteTypeParameter_list.lTotalRecs = CompanySiteTypeParameter_list.lDisplayRecs
	CompanySiteTypeParameter_list.lStopRec = CompanySiteTypeParameter_list.lDisplayRecs
Else
	CompanySiteTypeParameter_list.lStartRec = 1
	If CompanySiteTypeParameter_list.lDisplayRecs <= 0 Then ' Display all records
		CompanySiteTypeParameter_list.lDisplayRecs = CompanySiteTypeParameter_list.lTotalRecs
	End If
	If Not (CompanySiteTypeParameter.ExportAll AndAlso CompanySiteTypeParameter.Export <> "") Then
		CompanySiteTypeParameter_list.SetUpStartRec() ' Set up start record position
	End If
End If
%>
<p><span class="aspnetmaker" style="white-space: nowrap;">TABLE: Site Type Parameter
<% If CompanySiteTypeParameter.Export = "" AndAlso CompanySiteTypeParameter.CurrentAction = "" Then %>
&nbsp;&nbsp;<a href="<%= CompanySiteTypeParameter_list.PageUrl %>export=excel"><img src='images/exportxls.gif' alt='Export to Excel' title='Export to Excel' width='16' height='16' border='0'></a>
<% End If %>
</span></p>
<% If CompanySiteTypeParameter.Export = "" AndAlso CompanySiteTypeParameter.CurrentAction = "" Then %>
<a href="javascript:ew_ToggleSearchPanel(CompanySiteTypeParameter_list);" style="text-decoration: none;"><img id="CompanySiteTypeParameter_list_SearchImage" src="images/collapse.gif" alt="" width="9" height="9" border="0"></a><span class="aspnetmaker">&nbsp;Search</span><br>
<div id="CompanySiteTypeParameter_list_SearchPanel">
<form name="fCompanySiteTypeParameterlistsrch" id="fCompanySiteTypeParameterlistsrch" class="ewForm">
<input type="hidden" id="t" name="t" value="CompanySiteTypeParameter" />
<table class="ewBasicSearch">
	<tr>
		<td><span class="aspnetmaker">
			<a href="<%= CompanySiteTypeParameter_list.PageUrl %>cmd=reset">Show all</a>&nbsp;
			<a href="CompanySiteTypeParameter_srch.aspx">Advanced Search</a>&nbsp;
		</span></td>
	</tr>
</table>
</form>
</div>
<% End If %>
<% CompanySiteTypeParameter_list.ShowMessage() %>
<br />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<% If CompanySiteTypeParameter.Export = "" Then %>
<div class="ewGridUpperPanel">
<% If CompanySiteTypeParameter.CurrentAction <> "gridadd" AndAlso CompanySiteTypeParameter.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If CompanySiteTypeParameter_list.Pager Is Nothing Then CompanySiteTypeParameter_list.Pager = New cPrevNextPager(CompanySiteTypeParameter_list.lStartRec, CompanySiteTypeParameter_list.lDisplayRecs, CompanySiteTypeParameter_list.lTotalRecs) %>
<% If CompanySiteTypeParameter_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If CompanySiteTypeParameter_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= CompanySiteTypeParameter_list.PageUrl %>start=<%= CompanySiteTypeParameter_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If CompanySiteTypeParameter_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= CompanySiteTypeParameter_list.PageUrl %>start=<%= CompanySiteTypeParameter_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= CompanySiteTypeParameter_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If CompanySiteTypeParameter_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= CompanySiteTypeParameter_list.PageUrl %>start=<%= CompanySiteTypeParameter_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If CompanySiteTypeParameter_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= CompanySiteTypeParameter_list.PageUrl %>start=<%= CompanySiteTypeParameter_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= CompanySiteTypeParameter_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= CompanySiteTypeParameter_list.Pager.FromIndex %> to <%= CompanySiteTypeParameter_list.Pager.ToIndex %> of <%= CompanySiteTypeParameter_list.Pager.RecordCount %></span>
<% Else %>
	<% If CompanySiteTypeParameter_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If CompanySiteTypeParameter_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="CompanySiteTypeParameter" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If CompanySiteTypeParameter_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If CompanySiteTypeParameter_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If CompanySiteTypeParameter_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If CompanySiteTypeParameter.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<div class="aspnetmaker">
<% If CompanySiteTypeParameter.CurrentAction <> "gridadd" AndAlso CompanySiteTypeParameter.CurrentAction <> "gridedit" Then ' Not grid add/edit mode %>
<a href="<%= CompanySiteTypeParameter.AddUrl %>">Add</a>&nbsp;&nbsp;
<a href="<%= CompanySiteTypeParameter_list.PageUrl %>a=gridadd">Grid Add</a>&nbsp;&nbsp;
<% If CompanySiteTypeParameter_list.lTotalRecs > 0 Then %>
<a href="<%= CompanySiteTypeParameter_list.PageUrl %>a=gridedit">Grid Edit</a>&nbsp;&nbsp;
<% End If %>
<% Else ' Grid add/edit mode %>
<% If CompanySiteTypeParameter.CurrentAction = "gridadd" Then %>
<a href="" onclick="f=ew_GetForm('fCompanySiteTypeParameterlist');if (CompanySiteTypeParameter_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/insert.gif' alt='Insert' title='Insert' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<% If CompanySiteTypeParameter.CurrentAction = "gridedit" Then %>
<a href="" onclick="f=ew_GetForm('fCompanySiteTypeParameterlist');if (CompanySiteTypeParameter_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/update.gif' alt='Save' title='Save' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<a href="<%= CompanySiteTypeParameter_list.PageUrl %>a=cancel"><img src='images/cancel.gif' alt='Cancel' title='Cancel' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
</div>
</div>
<% End If %>
<div class="ewGridMiddlePanel">
<form name="fCompanySiteTypeParameterlist" id="fCompanySiteTypeParameterlist" class="ewForm" method="post">
<input type="hidden" name="t" id="t" value="CompanySiteTypeParameter" />
<% If CompanySiteTypeParameter_list.lTotalRecs > 0 Then %>
<table cellspacing="0" rowhighlightclass="ewTableHighlightRow" rowselectclass="ewTableSelectRow" roweditclass="ewTableEditRow" class="ewTable ewTableSeparate">
<%
	CompanySiteTypeParameter_list.lOptionCnt = 0
	CompanySiteTypeParameter_list.lOptionCnt = CompanySiteTypeParameter_list.lOptionCnt + 1 ' View
	CompanySiteTypeParameter_list.lOptionCnt = CompanySiteTypeParameter_list.lOptionCnt + 1 ' Edit
	CompanySiteTypeParameter_list.lOptionCnt = CompanySiteTypeParameter_list.lOptionCnt + 1 ' Copy
	CompanySiteTypeParameter_list.lOptionCnt = CompanySiteTypeParameter_list.lOptionCnt + 1 ' Delete
	CompanySiteTypeParameter_list.lOptionCnt = CompanySiteTypeParameter_list.lOptionCnt + CompanySiteTypeParameter_list.ListOptions.Items.Count ' Custom list options
%>
<%= CompanySiteTypeParameter.TableCustomInnerHTML %>
<thead><!-- Table header -->
	<tr class="ewTableHeader">
<% If CompanySiteTypeParameter.Export = "" Then %>
<% If CompanySiteTypeParameter.CurrentAction <> "gridadd" AndAlso CompanySiteTypeParameter.CurrentAction <> "gridedit" Then %>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<%

' Custom list options
For i As Integer = 0 to CompanySiteTypeParameter_list.ListOptions.Items.Count -1
	If CompanySiteTypeParameter_list.ListOptions.Items(i).Visible Then Response.Write(CompanySiteTypeParameter_list.ListOptions.Items(i).HeaderCellHtml)
Next
%>
<% End If %>
<% End If %>
<% If CompanySiteTypeParameter.SiteParameterTypeID.Visible Then ' SiteParameterTypeID %>
	<% If CompanySiteTypeParameter.SortUrl(CompanySiteTypeParameter.SiteParameterTypeID) = "" Then %>
		<td>Parameter</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= CompanySiteTypeParameter.SortUrl(CompanySiteTypeParameter.SiteParameterTypeID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Parameter</td><td style="width: 10px;"><% If CompanySiteTypeParameter.SiteParameterTypeID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf CompanySiteTypeParameter.SiteParameterTypeID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If CompanySiteTypeParameter.CompanyID.Visible Then ' CompanyID %>
	<% If CompanySiteTypeParameter.SortUrl(CompanySiteTypeParameter.CompanyID) = "" Then %>
		<td>Site</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= CompanySiteTypeParameter.SortUrl(CompanySiteTypeParameter.CompanyID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Site</td><td style="width: 10px;"><% If CompanySiteTypeParameter.CompanyID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf CompanySiteTypeParameter.CompanyID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If CompanySiteTypeParameter.SiteCategoryTypeID.Visible Then ' SiteCategoryTypeID %>
	<% If CompanySiteTypeParameter.SortUrl(CompanySiteTypeParameter.SiteCategoryTypeID) = "" Then %>
		<td>Site Type</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= CompanySiteTypeParameter.SortUrl(CompanySiteTypeParameter.SiteCategoryTypeID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Site Type</td><td style="width: 10px;"><% If CompanySiteTypeParameter.SiteCategoryTypeID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf CompanySiteTypeParameter.SiteCategoryTypeID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If CompanySiteTypeParameter.SiteCategoryGroupID.Visible Then ' SiteCategoryGroupID %>
	<% If CompanySiteTypeParameter.SortUrl(CompanySiteTypeParameter.SiteCategoryGroupID) = "" Then %>
		<td>Site Group</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= CompanySiteTypeParameter.SortUrl(CompanySiteTypeParameter.SiteCategoryGroupID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Site Group</td><td style="width: 10px;"><% If CompanySiteTypeParameter.SiteCategoryGroupID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf CompanySiteTypeParameter.SiteCategoryGroupID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If CompanySiteTypeParameter.SiteCategoryID.Visible Then ' SiteCategoryID %>
	<% If CompanySiteTypeParameter.SortUrl(CompanySiteTypeParameter.SiteCategoryID) = "" Then %>
		<td>Site Category</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= CompanySiteTypeParameter.SortUrl(CompanySiteTypeParameter.SiteCategoryID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Site Category</td><td style="width: 10px;"><% If CompanySiteTypeParameter.SiteCategoryID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf CompanySiteTypeParameter.SiteCategoryID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If CompanySiteTypeParameter.SortOrder.Visible Then ' SortOrder %>
	<% If CompanySiteTypeParameter.SortUrl(CompanySiteTypeParameter.SortOrder) = "" Then %>
		<td>Process Order</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= CompanySiteTypeParameter.SortUrl(CompanySiteTypeParameter.SortOrder) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Process Order</td><td style="width: 10px;"><% If CompanySiteTypeParameter.SortOrder.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf CompanySiteTypeParameter.SortOrder.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
	</tr>
</thead>
<tbody><!-- Table body -->
<%
If (CompanySiteTypeParameter.ExportAll AndAlso CompanySiteTypeParameter.Export <> "") Then
	CompanySiteTypeParameter_list.lStopRec = CompanySiteTypeParameter_list.lTotalRecs
Else
	CompanySiteTypeParameter_list.lStopRec = CompanySiteTypeParameter_list.lStartRec + CompanySiteTypeParameter_list.lDisplayRecs - 1 ' Set the last record to display
End If
If CompanySiteTypeParameter.CurrentAction = "gridadd" AndAlso CompanySiteTypeParameter_list.lStopRec = -1 Then
	CompanySiteTypeParameter_list.lStopRec = EW_GRIDADD_ROWS
End If 

' Move to first record
For i As Integer = 1 to CompanySiteTypeParameter_list.lStartRec - 1
	If Rs.Read() Then	CompanySiteTypeParameter_list.lRecCnt = CompanySiteTypeParameter_list.lRecCnt + 1
Next		
CompanySiteTypeParameter_list.lRowCnt = 0
CompanySiteTypeParameter_list.lEditRowCnt = 0
If CompanySiteTypeParameter.CurrentAction = "edit" Then CompanySiteTypeParameter_list.lRowIndex = 1
If CompanySiteTypeParameter.CurrentAction = "gridadd" Then CompanySiteTypeParameter_list.lRowIndex = 0
If CompanySiteTypeParameter.CurrentAction = "gridedit" Then CompanySiteTypeParameter_list.lRowIndex = 0

' Output data rows
Do While (CompanySiteTypeParameter.CurrentAction = "gridadd" OrElse Rs.Read()) AndAlso (CompanySiteTypeParameter_list.lRecCnt < CompanySiteTypeParameter_list.lStopRec)
	CompanySiteTypeParameter_list.lRecCnt = CompanySiteTypeParameter_list.lRecCnt + 1
	If CompanySiteTypeParameter_list.lRecCnt >= CompanySiteTypeParameter_list.lStartRec Then
		CompanySiteTypeParameter_list.lRowCnt = CompanySiteTypeParameter_list.lRowCnt + 1
		If CompanySiteTypeParameter.CurrentAction = "gridadd" OrElse CompanySiteTypeParameter.CurrentAction = "gridedit" Then CompanySiteTypeParameter_list.lRowIndex = CompanySiteTypeParameter_list.lRowIndex + 1
	CompanySiteTypeParameter.CssClass = ""
	CompanySiteTypeParameter.CssStyle = ""
	CompanySiteTypeParameter.RowClientEvents = "onmouseover='ew_MouseOver(event, this);' onmouseout='ew_MouseOut(event, this);' onclick='ew_Click(event, this);'"
	If CompanySiteTypeParameter.CurrentAction = "gridadd" Then
		CompanySiteTypeParameter_list.LoadDefaultValues() ' Load default values
	Else
		CompanySiteTypeParameter_list.LoadRowValues(Rs) ' Load row values
	End If
	CompanySiteTypeParameter.RowType = EW_ROWTYPE_VIEW ' Render view
	If CompanySiteTypeParameter.CurrentAction = "gridadd" Then ' Grid add
		CompanySiteTypeParameter.RowType = EW_ROWTYPE_ADD ' Render add
	End If
	If CompanySiteTypeParameter.CurrentAction = "gridadd" AndAlso CompanySiteTypeParameter.EventCancelled Then ' Insert failed
		CompanySiteTypeParameter_list.RestoreCurrentRowFormValues(CompanySiteTypeParameter_list.lRowIndex) ' Restore form values
	End If
	If CompanySiteTypeParameter.CurrentAction = "edit" Then
		If CompanySiteTypeParameter_list.CheckInlineEditKey() AndAlso CompanySiteTypeParameter_list.lEditRowCnt = 0 Then ' Inline edit
			CompanySiteTypeParameter.RowType = EW_ROWTYPE_EDIT ' Render edit
		End If
	End If
	If CompanySiteTypeParameter.CurrentAction = "gridedit" Then ' Grid edit
		CompanySiteTypeParameter.RowType = EW_ROWTYPE_EDIT ' Render edit
	End If
	If CompanySiteTypeParameter.RowType = EW_ROWTYPE_EDIT AndAlso CompanySiteTypeParameter.EventCancelled Then ' update failed
		If CompanySiteTypeParameter.CurrentAction = "edit" Then
			CompanySiteTypeParameter_list.RestoreFormValues() ' Restore form values
		End If
		If CompanySiteTypeParameter.CurrentAction = "gridedit" Then
			CompanySiteTypeParameter_list.RestoreCurrentRowFormValues(CompanySiteTypeParameter_list.lRowIndex) ' Restore form values
		End If
	End If
	If CompanySiteTypeParameter.RowType = EW_ROWTYPE_EDIT Then ' Edit row
		CompanySiteTypeParameter_list.lEditRowCnt = CompanySiteTypeParameter_list.lEditRowCnt + 1
		CompanySiteTypeParameter.RowClientEvents = "onmouseover='this.edit=true;ew_MouseOver(event, this);' onmouseout='ew_MouseOut(event, this);' onclick='ew_Click(event, this);'"
	End If
	If CompanySiteTypeParameter.RowType = EW_ROWTYPE_ADD OrElse CompanySiteTypeParameter.RowType = EW_ROWTYPE_EDIT Then ' Add / Edit row
		CompanySiteTypeParameter.CssClass = "ewTableEditRow"
	End If

	' Render row
	CompanySiteTypeParameter_list.RenderRow()
%>
	<tr<%= CompanySiteTypeParameter.RowAttributes %>>
<% If CompanySiteTypeParameter.RowType = EW_ROWTYPE_ADD OrElse CompanySiteTypeParameter.RowType = EW_ROWTYPE_EDIT Then %>
<% If CompanySiteTypeParameter.CurrentAction = "edit" Then %>
<td colspan="<%= CompanySiteTypeParameter_list.lOptionCnt %>" align="right"><span class="aspnetmaker">
<a href="" onclick="f=ew_GetForm('fCompanySiteTypeParameterlist');if (CompanySiteTypeParameter_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/update.gif' alt='Update' title='Update' width='16' height='16' border='0'></a>&nbsp;<a href="<%= CompanySiteTypeParameter_list.PageUrl %>a=cancel"><img src='images/cancel.gif' alt='Cancel' title='Cancel' width='16' height='16' border='0'></a>
<input type="hidden" name="a_list" id="a_list" value="update" />
</span></td>
<% End If %>
<%
	If CompanySiteTypeParameter.CurrentAction = "gridedit" Then
		CompanySiteTypeParameter_list.sMultiSelectKey = CompanySiteTypeParameter_list.sMultiSelectKey & "<input type=""hidden"" name=""k" & CompanySiteTypeParameter_list.lRowIndex & "_key"" id=""k" & CompanySiteTypeParameter_list.lRowIndex & "_key"" value=""" & CompanySiteTypeParameter.CompanySiteTypeParameterID.CurrentValue & """ />"
	End If
%>
<% Else %>
<% If CompanySiteTypeParameter.Export = "" Then %>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= CompanySiteTypeParameter.ViewUrl %>"><img src='images/view.gif' alt='View' title='View' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= CompanySiteTypeParameter.EditUrl %>"><img src='images/edit.gif' alt='Edit' title='Edit' width='16' height='16' border='0'></a><span class="ewSeparator">&nbsp;|&nbsp;</span><a href="<%= CompanySiteTypeParameter.InlineEditUrl %>"><img src='images/inlineedit.gif' alt='Inline Edit' title='Inline Edit' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= CompanySiteTypeParameter.CopyUrl %>"><img src='images/copy.gif' alt='Copy' title='Copy' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= CompanySiteTypeParameter.DeleteUrl %>"><img src='images/delete.gif' alt='Delete' title='Delete' width='16' height='16' border='0'></a>
</span></td>
<%

' Custom list options
For i As Integer = 0 to CompanySiteTypeParameter_list.ListOptions.Items.Count -1
	If CompanySiteTypeParameter_list.ListOptions.Items(i).Visible Then Response.Write(CompanySiteTypeParameter_list.ListOptions.Items(i).BodyCellHtml)
Next
%>
<% End If %>
<% End If %>
	<% If CompanySiteTypeParameter.SiteParameterTypeID.Visible Then ' SiteParameterTypeID %>
		<td<%= CompanySiteTypeParameter.SiteParameterTypeID.CellAttributes %>>
<% If CompanySiteTypeParameter.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<select id="x<%= CompanySiteTypeParameter_list.lRowIndex %>_SiteParameterTypeID" name="x<%= CompanySiteTypeParameter_list.lRowIndex %>_SiteParameterTypeID"<%= CompanySiteTypeParameter.SiteParameterTypeID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(CompanySiteTypeParameter.SiteParameterTypeID.EditValue) Then
	arwrk = CompanySiteTypeParameter.SiteParameterTypeID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), CompanySiteTypeParameter.SiteParameterTypeID.CurrentValue) Then
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
If emptywrk Then CompanySiteTypeParameter.SiteParameterTypeID.OldValue = ""
%>
</select>
<input type="hidden" name="o<%= CompanySiteTypeParameter_list.lRowIndex %>_SiteParameterTypeID" id="o<%= CompanySiteTypeParameter_list.lRowIndex %>_SiteParameterTypeID" value="<%= ew_HTMLEncode(CompanySiteTypeParameter.SiteParameterTypeID.OldValue) %>" />
<% End If %>
<% If CompanySiteTypeParameter.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<select id="x<%= CompanySiteTypeParameter_list.lRowIndex %>_SiteParameterTypeID" name="x<%= CompanySiteTypeParameter_list.lRowIndex %>_SiteParameterTypeID"<%= CompanySiteTypeParameter.SiteParameterTypeID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(CompanySiteTypeParameter.SiteParameterTypeID.EditValue) Then
	arwrk = CompanySiteTypeParameter.SiteParameterTypeID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), CompanySiteTypeParameter.SiteParameterTypeID.CurrentValue) Then
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
If emptywrk Then CompanySiteTypeParameter.SiteParameterTypeID.OldValue = ""
%>
</select>
<% End If %>
<% If CompanySiteTypeParameter.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= CompanySiteTypeParameter.SiteParameterTypeID.ViewAttributes %>><%= CompanySiteTypeParameter.SiteParameterTypeID.ListViewValue %></div>
<% End If %>
<% If CompanySiteTypeParameter.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<input type="hidden" name="o<%= CompanySiteTypeParameter_list.lRowIndex %>_CompanySiteTypeParameterID" id="o<%= CompanySiteTypeParameter_list.lRowIndex %>_CompanySiteTypeParameterID" value="<%= ew_HTMLEncode(CompanySiteTypeParameter.CompanySiteTypeParameterID.OldValue) %>" />
<% End If %>
<% If CompanySiteTypeParameter.RowType = EW_ROWTYPE_EDIT Then %>
<input type="hidden" name="x<%= CompanySiteTypeParameter_list.lRowIndex %>_CompanySiteTypeParameterID" id="x<%= CompanySiteTypeParameter_list.lRowIndex %>_CompanySiteTypeParameterID" value="<%= ew_HTMLEncode(CompanySiteTypeParameter.CompanySiteTypeParameterID.CurrentValue) %>" />
<% End If %>
</td>
	<% End If %>
	<% If CompanySiteTypeParameter.CompanyID.Visible Then ' CompanyID %>
		<td<%= CompanySiteTypeParameter.CompanyID.CellAttributes %>>
<% If CompanySiteTypeParameter.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<select id="x<%= CompanySiteTypeParameter_list.lRowIndex %>_CompanyID" name="x<%= CompanySiteTypeParameter_list.lRowIndex %>_CompanyID"<%= CompanySiteTypeParameter.CompanyID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(CompanySiteTypeParameter.CompanyID.EditValue) Then
	arwrk = CompanySiteTypeParameter.CompanyID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), CompanySiteTypeParameter.CompanyID.CurrentValue) Then
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
If emptywrk Then CompanySiteTypeParameter.CompanyID.OldValue = ""
%>
</select>
<input type="hidden" name="o<%= CompanySiteTypeParameter_list.lRowIndex %>_CompanyID" id="o<%= CompanySiteTypeParameter_list.lRowIndex %>_CompanyID" value="<%= ew_HTMLEncode(CompanySiteTypeParameter.CompanyID.OldValue) %>" />
<% End If %>
<% If CompanySiteTypeParameter.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<select id="x<%= CompanySiteTypeParameter_list.lRowIndex %>_CompanyID" name="x<%= CompanySiteTypeParameter_list.lRowIndex %>_CompanyID"<%= CompanySiteTypeParameter.CompanyID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(CompanySiteTypeParameter.CompanyID.EditValue) Then
	arwrk = CompanySiteTypeParameter.CompanyID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), CompanySiteTypeParameter.CompanyID.CurrentValue) Then
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
If emptywrk Then CompanySiteTypeParameter.CompanyID.OldValue = ""
%>
</select>
<% End If %>
<% If CompanySiteTypeParameter.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= CompanySiteTypeParameter.CompanyID.ViewAttributes %>><%= CompanySiteTypeParameter.CompanyID.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	<% If CompanySiteTypeParameter.SiteCategoryTypeID.Visible Then ' SiteCategoryTypeID %>
		<td<%= CompanySiteTypeParameter.SiteCategoryTypeID.CellAttributes %>>
<% If CompanySiteTypeParameter.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<% If CompanySiteTypeParameter.SiteCategoryTypeID.SessionValue <> "" Then %>
<div<%= CompanySiteTypeParameter.SiteCategoryTypeID.ViewAttributes %>><%= CompanySiteTypeParameter.SiteCategoryTypeID.ListViewValue %></div>
<input type="hidden" id="x<%= CompanySiteTypeParameter_list.lRowIndex %>_SiteCategoryTypeID" name="x<%= CompanySiteTypeParameter_list.lRowIndex %>_SiteCategoryTypeID" value="<%= ew_HtmlEncode(CompanySiteTypeParameter.SiteCategoryTypeID.CurrentValue) %>">
<% Else %>
<select id="x<%= CompanySiteTypeParameter_list.lRowIndex %>_SiteCategoryTypeID" name="x<%= CompanySiteTypeParameter_list.lRowIndex %>_SiteCategoryTypeID" onchange="ew_UpdateOpt('x<%= CompanySiteTypeParameter_list.lRowIndex %>_SiteCategoryID','x<%= CompanySiteTypeParameter_list.lRowIndex %>_SiteCategoryTypeID',CompanySiteTypeParameter_list.ar_x<%= CompanySiteTypeParameter_list.lRowIndex %>_SiteCategoryID);"<%= CompanySiteTypeParameter.SiteCategoryTypeID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(CompanySiteTypeParameter.SiteCategoryTypeID.EditValue) Then
	arwrk = CompanySiteTypeParameter.SiteCategoryTypeID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), CompanySiteTypeParameter.SiteCategoryTypeID.CurrentValue) Then
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
If emptywrk Then CompanySiteTypeParameter.SiteCategoryTypeID.OldValue = ""
%>
</select>
<% End If %>
<input type="hidden" name="o<%= CompanySiteTypeParameter_list.lRowIndex %>_SiteCategoryTypeID" id="o<%= CompanySiteTypeParameter_list.lRowIndex %>_SiteCategoryTypeID" value="<%= ew_HTMLEncode(CompanySiteTypeParameter.SiteCategoryTypeID.OldValue) %>" />
<% End If %>
<% If CompanySiteTypeParameter.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<% If CompanySiteTypeParameter.SiteCategoryTypeID.SessionValue <> "" Then %>
<div<%= CompanySiteTypeParameter.SiteCategoryTypeID.ViewAttributes %>><%= CompanySiteTypeParameter.SiteCategoryTypeID.ListViewValue %></div>
<input type="hidden" id="x<%= CompanySiteTypeParameter_list.lRowIndex %>_SiteCategoryTypeID" name="x<%= CompanySiteTypeParameter_list.lRowIndex %>_SiteCategoryTypeID" value="<%= ew_HtmlEncode(CompanySiteTypeParameter.SiteCategoryTypeID.CurrentValue) %>">
<% Else %>
<select id="x<%= CompanySiteTypeParameter_list.lRowIndex %>_SiteCategoryTypeID" name="x<%= CompanySiteTypeParameter_list.lRowIndex %>_SiteCategoryTypeID" onchange="ew_UpdateOpt('x<%= CompanySiteTypeParameter_list.lRowIndex %>_SiteCategoryID','x<%= CompanySiteTypeParameter_list.lRowIndex %>_SiteCategoryTypeID',CompanySiteTypeParameter_list.ar_x<%= CompanySiteTypeParameter_list.lRowIndex %>_SiteCategoryID);"<%= CompanySiteTypeParameter.SiteCategoryTypeID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(CompanySiteTypeParameter.SiteCategoryTypeID.EditValue) Then
	arwrk = CompanySiteTypeParameter.SiteCategoryTypeID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), CompanySiteTypeParameter.SiteCategoryTypeID.CurrentValue) Then
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
If emptywrk Then CompanySiteTypeParameter.SiteCategoryTypeID.OldValue = ""
%>
</select>
<% End If %>
<% End If %>
<% If CompanySiteTypeParameter.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= CompanySiteTypeParameter.SiteCategoryTypeID.ViewAttributes %>><%= CompanySiteTypeParameter.SiteCategoryTypeID.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	<% If CompanySiteTypeParameter.SiteCategoryGroupID.Visible Then ' SiteCategoryGroupID %>
		<td<%= CompanySiteTypeParameter.SiteCategoryGroupID.CellAttributes %>>
<% If CompanySiteTypeParameter.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<select id="x<%= CompanySiteTypeParameter_list.lRowIndex %>_SiteCategoryGroupID" name="x<%= CompanySiteTypeParameter_list.lRowIndex %>_SiteCategoryGroupID"<%= CompanySiteTypeParameter.SiteCategoryGroupID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(CompanySiteTypeParameter.SiteCategoryGroupID.EditValue) Then
	arwrk = CompanySiteTypeParameter.SiteCategoryGroupID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), CompanySiteTypeParameter.SiteCategoryGroupID.CurrentValue) Then
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
If emptywrk Then CompanySiteTypeParameter.SiteCategoryGroupID.OldValue = ""
%>
</select>
<input type="hidden" name="o<%= CompanySiteTypeParameter_list.lRowIndex %>_SiteCategoryGroupID" id="o<%= CompanySiteTypeParameter_list.lRowIndex %>_SiteCategoryGroupID" value="<%= ew_HTMLEncode(CompanySiteTypeParameter.SiteCategoryGroupID.OldValue) %>" />
<% End If %>
<% If CompanySiteTypeParameter.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<select id="x<%= CompanySiteTypeParameter_list.lRowIndex %>_SiteCategoryGroupID" name="x<%= CompanySiteTypeParameter_list.lRowIndex %>_SiteCategoryGroupID"<%= CompanySiteTypeParameter.SiteCategoryGroupID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(CompanySiteTypeParameter.SiteCategoryGroupID.EditValue) Then
	arwrk = CompanySiteTypeParameter.SiteCategoryGroupID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), CompanySiteTypeParameter.SiteCategoryGroupID.CurrentValue) Then
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
If emptywrk Then CompanySiteTypeParameter.SiteCategoryGroupID.OldValue = ""
%>
</select>
<% End If %>
<% If CompanySiteTypeParameter.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= CompanySiteTypeParameter.SiteCategoryGroupID.ViewAttributes %>><%= CompanySiteTypeParameter.SiteCategoryGroupID.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	<% If CompanySiteTypeParameter.SiteCategoryID.Visible Then ' SiteCategoryID %>
		<td<%= CompanySiteTypeParameter.SiteCategoryID.CellAttributes %>>
<% If CompanySiteTypeParameter.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<select id="x<%= CompanySiteTypeParameter_list.lRowIndex %>_SiteCategoryID" name="x<%= CompanySiteTypeParameter_list.lRowIndex %>_SiteCategoryID"<%= CompanySiteTypeParameter.SiteCategoryID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(CompanySiteTypeParameter.SiteCategoryID.EditValue) Then
	arwrk = CompanySiteTypeParameter.SiteCategoryID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), CompanySiteTypeParameter.SiteCategoryID.CurrentValue) Then
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
If emptywrk Then CompanySiteTypeParameter.SiteCategoryID.OldValue = ""
%>
</select>
<%
jswrk = "" ' Initialise
If ew_IsArrayList(CompanySiteTypeParameter.SiteCategoryID.EditValue) Then
	arwrk = CompanySiteTypeParameter.SiteCategoryID.EditValue
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
CompanySiteTypeParameter_list.ar_x<%= CompanySiteTypeParameter_list.lRowIndex %>_SiteCategoryID = [<%= jswrk %>];
//-->
</script>
<input type="hidden" name="o<%= CompanySiteTypeParameter_list.lRowIndex %>_SiteCategoryID" id="o<%= CompanySiteTypeParameter_list.lRowIndex %>_SiteCategoryID" value="<%= ew_HTMLEncode(CompanySiteTypeParameter.SiteCategoryID.OldValue) %>" />
<% End If %>
<% If CompanySiteTypeParameter.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<select id="x<%= CompanySiteTypeParameter_list.lRowIndex %>_SiteCategoryID" name="x<%= CompanySiteTypeParameter_list.lRowIndex %>_SiteCategoryID"<%= CompanySiteTypeParameter.SiteCategoryID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(CompanySiteTypeParameter.SiteCategoryID.EditValue) Then
	arwrk = CompanySiteTypeParameter.SiteCategoryID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), CompanySiteTypeParameter.SiteCategoryID.CurrentValue) Then
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
If emptywrk Then CompanySiteTypeParameter.SiteCategoryID.OldValue = ""
%>
</select>
<%
jswrk = "" ' Initialise
If ew_IsArrayList(CompanySiteTypeParameter.SiteCategoryID.EditValue) Then
	arwrk = CompanySiteTypeParameter.SiteCategoryID.EditValue
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
CompanySiteTypeParameter_list.ar_x<%= CompanySiteTypeParameter_list.lRowIndex %>_SiteCategoryID = [<%= jswrk %>];
//-->
</script>
<% End If %>
<% If CompanySiteTypeParameter.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= CompanySiteTypeParameter.SiteCategoryID.ViewAttributes %>><%= CompanySiteTypeParameter.SiteCategoryID.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	<% If CompanySiteTypeParameter.SortOrder.Visible Then ' SortOrder %>
		<td<%= CompanySiteTypeParameter.SortOrder.CellAttributes %>>
<% If CompanySiteTypeParameter.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<input type="text" name="x<%= CompanySiteTypeParameter_list.lRowIndex %>_SortOrder" id="x<%= CompanySiteTypeParameter_list.lRowIndex %>_SortOrder" size="30" value="<%= CompanySiteTypeParameter.SortOrder.EditValue %>"<%= CompanySiteTypeParameter.SortOrder.EditAttributes %> />
<input type="hidden" name="o<%= CompanySiteTypeParameter_list.lRowIndex %>_SortOrder" id="o<%= CompanySiteTypeParameter_list.lRowIndex %>_SortOrder" value="<%= ew_HTMLEncode(CompanySiteTypeParameter.SortOrder.OldValue) %>" />
<% End If %>
<% If CompanySiteTypeParameter.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<input type="text" name="x<%= CompanySiteTypeParameter_list.lRowIndex %>_SortOrder" id="x<%= CompanySiteTypeParameter_list.lRowIndex %>_SortOrder" size="30" value="<%= CompanySiteTypeParameter.SortOrder.EditValue %>"<%= CompanySiteTypeParameter.SortOrder.EditAttributes %> />
<% End If %>
<% If CompanySiteTypeParameter.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= CompanySiteTypeParameter.SortOrder.ViewAttributes %>><%= CompanySiteTypeParameter.SortOrder.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	</tr>
<% If CompanySiteTypeParameter.RowType = EW_ROWTYPE_ADD Then %>
<script language="JavaScript">
<!--
ew_UpdateOpts([['x<%= CompanySiteTypeParameter_list.lRowIndex %>_SiteCategoryID','x<%= CompanySiteTypeParameter_list.lRowIndex %>_SiteCategoryTypeID',CompanySiteTypeParameter_list.ar_x<%= CompanySiteTypeParameter_list.lRowIndex %>_SiteCategoryID]]);
//-->
</script>
<% End If %>
<% If CompanySiteTypeParameter.RowType = EW_ROWTYPE_EDIT Then %>
<script language="JavaScript">
<!--
ew_UpdateOpts([['x<%= CompanySiteTypeParameter_list.lRowIndex %>_SiteCategoryID','x<%= CompanySiteTypeParameter_list.lRowIndex %>_SiteCategoryTypeID',CompanySiteTypeParameter_list.ar_x<%= CompanySiteTypeParameter_list.lRowIndex %>_SiteCategoryID]]);
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
<% If CompanySiteTypeParameter.CurrentAction = "gridadd" Then %>
<input type="hidden" name="a_list" id="a_list" value="gridinsert" />
<input type="hidden" name="key_count" id="key_count" value="<%= CompanySiteTypeParameter_list.lRowIndex %>" />
<% End If %>
<% If CompanySiteTypeParameter.CurrentAction = "edit" Then %>
<input type="hidden" name="key_count" id="key_count" value="<%= CompanySiteTypeParameter_list.lRowIndex %>" />
<% End If %>
<% If CompanySiteTypeParameter.CurrentAction = "gridedit" Then %>
<input type="hidden" name="a_list" id="a_list" value="gridupdate" />
<input type="hidden" name="key_count" id="key_count" value="<%= CompanySiteTypeParameter_list.lRowIndex %>" />
<%= CompanySiteTypeParameter_list.sMultiSelectKey %>
<% End If %>
</form>
<%

' Close recordset
Rs.Close()
Rs.Dispose()
%>
</div>
<% If CompanySiteTypeParameter_list.lTotalRecs > 0 Then %>
<% If CompanySiteTypeParameter.Export = "" Then %>
<div class="ewGridLowerPanel">
<% If CompanySiteTypeParameter.CurrentAction <> "gridadd" AndAlso CompanySiteTypeParameter.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If CompanySiteTypeParameter_list.Pager Is Nothing Then CompanySiteTypeParameter_list.Pager = New cPrevNextPager(CompanySiteTypeParameter_list.lStartRec, CompanySiteTypeParameter_list.lDisplayRecs, CompanySiteTypeParameter_list.lTotalRecs) %>
<% If CompanySiteTypeParameter_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If CompanySiteTypeParameter_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= CompanySiteTypeParameter_list.PageUrl %>start=<%= CompanySiteTypeParameter_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If CompanySiteTypeParameter_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= CompanySiteTypeParameter_list.PageUrl %>start=<%= CompanySiteTypeParameter_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= CompanySiteTypeParameter_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If CompanySiteTypeParameter_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= CompanySiteTypeParameter_list.PageUrl %>start=<%= CompanySiteTypeParameter_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If CompanySiteTypeParameter_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= CompanySiteTypeParameter_list.PageUrl %>start=<%= CompanySiteTypeParameter_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= CompanySiteTypeParameter_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= CompanySiteTypeParameter_list.Pager.FromIndex %> to <%= CompanySiteTypeParameter_list.Pager.ToIndex %> of <%= CompanySiteTypeParameter_list.Pager.RecordCount %></span>
<% Else %>
	<% If CompanySiteTypeParameter_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If CompanySiteTypeParameter_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="CompanySiteTypeParameter" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If CompanySiteTypeParameter_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If CompanySiteTypeParameter_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If CompanySiteTypeParameter_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If CompanySiteTypeParameter.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<% 'If CompanySiteTypeParameter_list.lTotalRecs > 0 Then %>
<div class="aspnetmaker">
<% If CompanySiteTypeParameter.CurrentAction <> "gridadd" AndAlso CompanySiteTypeParameter.CurrentAction <> "gridedit" Then ' Not grid add/edit mode %>
<a href="<%= CompanySiteTypeParameter.AddUrl %>">Add</a>&nbsp;&nbsp;
<a href="<%= CompanySiteTypeParameter_list.PageUrl %>a=gridadd">Grid Add</a>&nbsp;&nbsp;
<% If CompanySiteTypeParameter_list.lTotalRecs > 0 Then %>
<a href="<%= CompanySiteTypeParameter_list.PageUrl %>a=gridedit">Grid Edit</a>&nbsp;&nbsp;
<% End If %>
<% Else ' Grid add/edit mode %>
<% If CompanySiteTypeParameter.CurrentAction = "gridadd" Then %>
<a href="" onclick="f=ew_GetForm('fCompanySiteTypeParameterlist');if (CompanySiteTypeParameter_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/insert.gif' alt='Insert' title='Insert' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<% If CompanySiteTypeParameter.CurrentAction = "gridedit" Then %>
<a href="" onclick="f=ew_GetForm('fCompanySiteTypeParameterlist');if (CompanySiteTypeParameter_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/update.gif' alt='Save' title='Save' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<a href="<%= CompanySiteTypeParameter_list.PageUrl %>a=cancel"><img src='images/cancel.gif' alt='Cancel' title='Cancel' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
</div>
<% 'End If %>
</div>
<% End If %>
<% End If %>
</td></tr></table>
<% If CompanySiteTypeParameter.Export = "" AndAlso CompanySiteTypeParameter.CurrentAction = "" Then %>
<script type="text/javascript">
<!--
//ew_ToggleSearchPanel(CompanySiteTypeParameter_list); // uncomment to init search panel as collapsed
//-->
</script>
<% End If %>
<% If CompanySiteTypeParameter.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
