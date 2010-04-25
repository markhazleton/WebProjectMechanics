<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="PageImage_list.aspx.vb" Inherits="PageImage_list" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If PageImage.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var PageImage_list = new ew_Page("PageImage_list");
// page properties
PageImage_list.PageID = "list"; // page ID
var EW_PAGE_ID = PageImage_list.PageID; // for backward compatibility
// extend page with ValidateForm function
PageImage_list.ValidateForm = function(fobj) {
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
		elm = fobj.elements["x" + infix + "_zPageID"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field - PageName");
		elm = fobj.elements["x" + infix + "_ImageID"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field - ImageName");
		elm = fobj.elements["x" + infix + "_PageImagePosition"];
		if (elm && !ew_CheckInteger(elm.value))
			return ew_OnError(this, elm, "Incorrect integer - Page Image Position");
		} // End Grid Add checking
	}
	if (fobj.a_list && fobj.a_list.value == "gridinsert" && addcnt == 0) { // No row added
		alert("No records to be added");
		return false;
	}
	return true;
}
// Extend page with empty row check
PageImage_list.EmptyRow = function(fobj, infix) {
	if (ew_ValueChanged(fobj, infix, "zPageID")) return false;
	if (ew_ValueChanged(fobj, infix, "ImageID")) return false;
	if (ew_ValueChanged(fobj, infix, "PageImagePosition")) return false;
	return true;
}
<% If EW_CLIENT_VALIDATE Then %>
PageImage_list.ValidateRequired = true; // uses JavaScript validation
<% Else %>
PageImage_list.ValidateRequired = false; // no JavaScript validation
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
<% If PageImage.Export = "" Then %>
<% End If %>
<%
If PageImage.CurrentAction = "gridadd" Then PageImage.CurrentFilter = "0=1"

' Load recordset
Rs = PageImage_list.LoadRecordset()
If PageImage.CurrentAction = "gridadd" Then
	PageImage_list.lStartRec = 1
	If PageImage_list.lDisplayRecs <= 0 Then PageImage_list.lDisplayRecs = 25
	PageImage_list.lTotalRecs = PageImage_list.lDisplayRecs
	PageImage_list.lStopRec = PageImage_list.lDisplayRecs
Else
	PageImage_list.lStartRec = 1
	If PageImage_list.lDisplayRecs <= 0 Then ' Display all records
		PageImage_list.lDisplayRecs = PageImage_list.lTotalRecs
	End If
	If Not (PageImage.ExportAll AndAlso PageImage.Export <> "") Then
		PageImage_list.SetUpStartRec() ' Set up start record position
	End If
End If
%>
<p><span class="aspnetmaker" style="white-space: nowrap;">TABLE: Location Image
<% If PageImage.Export = "" AndAlso PageImage.CurrentAction = "" Then %>
&nbsp;&nbsp;<a href="<%= PageImage_list.PageUrl %>export=excel"><img src='images/exportxls.gif' alt='Export to Excel' title='Export to Excel' width='16' height='16' border='0'></a>
<% End If %>
</span></p>
<% If PageImage.Export = "" AndAlso PageImage.CurrentAction = "" Then %>
<a href="javascript:ew_ToggleSearchPanel(PageImage_list);" style="text-decoration: none;"><img id="PageImage_list_SearchImage" src="images/collapse.gif" alt="" width="9" height="9" border="0"></a><span class="aspnetmaker">&nbsp;Search</span><br>
<div id="PageImage_list_SearchPanel">
<form name="fPageImagelistsrch" id="fPageImagelistsrch" class="ewForm">
<input type="hidden" id="t" name="t" value="PageImage" />
<table class="ewBasicSearch">
	<tr>
		<td><span class="aspnetmaker">
			<a href="<%= PageImage_list.PageUrl %>cmd=reset">Show all</a>&nbsp;
			<a href="PageImage_srch.aspx">Advanced Search</a>&nbsp;
		</span></td>
	</tr>
</table>
</form>
</div>
<% End If %>
<% PageImage_list.ShowMessage() %>
<br />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<% If PageImage.Export = "" Then %>
<div class="ewGridUpperPanel">
<% If PageImage.CurrentAction <> "gridadd" AndAlso PageImage.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If PageImage_list.Pager Is Nothing Then PageImage_list.Pager = New cPrevNextPager(PageImage_list.lStartRec, PageImage_list.lDisplayRecs, PageImage_list.lTotalRecs) %>
<% If PageImage_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If PageImage_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= PageImage_list.PageUrl %>start=<%= PageImage_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If PageImage_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= PageImage_list.PageUrl %>start=<%= PageImage_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= PageImage_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If PageImage_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= PageImage_list.PageUrl %>start=<%= PageImage_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If PageImage_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= PageImage_list.PageUrl %>start=<%= PageImage_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= PageImage_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= PageImage_list.Pager.FromIndex %> to <%= PageImage_list.Pager.ToIndex %> of <%= PageImage_list.Pager.RecordCount %></span>
<% Else %>
	<% If PageImage_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If PageImage_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="PageImage" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If PageImage_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If PageImage_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If PageImage_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If PageImage.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<div class="aspnetmaker">
<% If PageImage.CurrentAction <> "gridadd" AndAlso PageImage.CurrentAction <> "gridedit" Then ' Not grid add/edit mode %>
<a href="<%= PageImage.AddUrl %>">Add</a>&nbsp;&nbsp;
<a href="<%= PageImage_list.PageUrl %>a=gridadd">Grid Add</a>&nbsp;&nbsp;
<% If PageImage_list.lTotalRecs > 0 Then %>
<a href="<%= PageImage_list.PageUrl %>a=gridedit">Grid Edit</a>&nbsp;&nbsp;
<% End If %>
<% Else ' Grid add/edit mode %>
<% If PageImage.CurrentAction = "gridadd" Then %>
<a href="" onclick="f=ew_GetForm('fPageImagelist');if (PageImage_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/insert.gif' alt='Insert' title='Insert' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<% If PageImage.CurrentAction = "gridedit" Then %>
<a href="" onclick="f=ew_GetForm('fPageImagelist');if (PageImage_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/update.gif' alt='Save' title='Save' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<a href="<%= PageImage_list.PageUrl %>a=cancel"><img src='images/cancel.gif' alt='Cancel' title='Cancel' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
</div>
</div>
<% End If %>
<div class="ewGridMiddlePanel">
<form name="fPageImagelist" id="fPageImagelist" class="ewForm" method="post">
<input type="hidden" name="t" id="t" value="PageImage" />
<% If PageImage_list.lTotalRecs > 0 Then %>
<table cellspacing="0" rowhighlightclass="ewTableHighlightRow" rowselectclass="ewTableSelectRow" roweditclass="ewTableEditRow" class="ewTable ewTableSeparate">
<%
	PageImage_list.lOptionCnt = 0
	PageImage_list.lOptionCnt = PageImage_list.lOptionCnt + 1 ' View
	PageImage_list.lOptionCnt = PageImage_list.lOptionCnt + 1 ' Edit
	PageImage_list.lOptionCnt = PageImage_list.lOptionCnt + 1 ' Copy
	PageImage_list.lOptionCnt = PageImage_list.lOptionCnt + 1 ' Delete
	PageImage_list.lOptionCnt = PageImage_list.lOptionCnt + PageImage_list.ListOptions.Items.Count ' Custom list options
%>
<%= PageImage.TableCustomInnerHTML %>
<thead><!-- Table header -->
	<tr class="ewTableHeader">
<% If PageImage.Export = "" Then %>
<% If PageImage.CurrentAction <> "gridadd" AndAlso PageImage.CurrentAction <> "gridedit" Then %>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<%

' Custom list options
For i As Integer = 0 to PageImage_list.ListOptions.Items.Count -1
	If PageImage_list.ListOptions.Items(i).Visible Then Response.Write(PageImage_list.ListOptions.Items(i).HeaderCellHtml)
Next
%>
<% End If %>
<% End If %>
<% If PageImage.zPageID.Visible Then ' PageID %>
	<% If PageImage.SortUrl(PageImage.zPageID) = "" Then %>
		<td>PageName</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= PageImage.SortUrl(PageImage.zPageID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>PageName</td><td style="width: 10px;"><% If PageImage.zPageID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf PageImage.zPageID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If PageImage.ImageID.Visible Then ' ImageID %>
	<% If PageImage.SortUrl(PageImage.ImageID) = "" Then %>
		<td>ImageName</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= PageImage.SortUrl(PageImage.ImageID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>ImageName</td><td style="width: 10px;"><% If PageImage.ImageID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf PageImage.ImageID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If PageImage.PageImagePosition.Visible Then ' PageImagePosition %>
	<% If PageImage.SortUrl(PageImage.PageImagePosition) = "" Then %>
		<td>Page Image Position</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= PageImage.SortUrl(PageImage.PageImagePosition) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Page Image Position</td><td style="width: 10px;"><% If PageImage.PageImagePosition.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf PageImage.PageImagePosition.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
	</tr>
</thead>
<tbody><!-- Table body -->
<%
If (PageImage.ExportAll AndAlso PageImage.Export <> "") Then
	PageImage_list.lStopRec = PageImage_list.lTotalRecs
Else
	PageImage_list.lStopRec = PageImage_list.lStartRec + PageImage_list.lDisplayRecs - 1 ' Set the last record to display
End If
If PageImage.CurrentAction = "gridadd" AndAlso PageImage_list.lStopRec = -1 Then
	PageImage_list.lStopRec = EW_GRIDADD_ROWS
End If 

' Move to first record
For i As Integer = 1 to PageImage_list.lStartRec - 1
	If Rs.Read() Then	PageImage_list.lRecCnt = PageImage_list.lRecCnt + 1
Next		
PageImage_list.lRowCnt = 0
PageImage_list.lEditRowCnt = 0
If PageImage.CurrentAction = "edit" Then PageImage_list.lRowIndex = 1
If PageImage.CurrentAction = "gridadd" Then PageImage_list.lRowIndex = 0
If PageImage.CurrentAction = "gridedit" Then PageImage_list.lRowIndex = 0

' Output data rows
Do While (PageImage.CurrentAction = "gridadd" OrElse Rs.Read()) AndAlso (PageImage_list.lRecCnt < PageImage_list.lStopRec)
	PageImage_list.lRecCnt = PageImage_list.lRecCnt + 1
	If PageImage_list.lRecCnt >= PageImage_list.lStartRec Then
		PageImage_list.lRowCnt = PageImage_list.lRowCnt + 1
		If PageImage.CurrentAction = "gridadd" OrElse PageImage.CurrentAction = "gridedit" Then PageImage_list.lRowIndex = PageImage_list.lRowIndex + 1
	PageImage.CssClass = ""
	PageImage.CssStyle = ""
	PageImage.RowClientEvents = "onmouseover='ew_MouseOver(event, this);' onmouseout='ew_MouseOut(event, this);' onclick='ew_Click(event, this);'"
	If PageImage.CurrentAction = "gridadd" Then
		PageImage_list.LoadDefaultValues() ' Load default values
	Else
		PageImage_list.LoadRowValues(Rs) ' Load row values
	End If
	PageImage.RowType = EW_ROWTYPE_VIEW ' Render view
	If PageImage.CurrentAction = "gridadd" Then ' Grid add
		PageImage.RowType = EW_ROWTYPE_ADD ' Render add
	End If
	If PageImage.CurrentAction = "gridadd" AndAlso PageImage.EventCancelled Then ' Insert failed
		PageImage_list.RestoreCurrentRowFormValues(PageImage_list.lRowIndex) ' Restore form values
	End If
	If PageImage.CurrentAction = "edit" Then
		If PageImage_list.CheckInlineEditKey() AndAlso PageImage_list.lEditRowCnt = 0 Then ' Inline edit
			PageImage.RowType = EW_ROWTYPE_EDIT ' Render edit
		End If
	End If
	If PageImage.CurrentAction = "gridedit" Then ' Grid edit
		PageImage.RowType = EW_ROWTYPE_EDIT ' Render edit
	End If
	If PageImage.RowType = EW_ROWTYPE_EDIT AndAlso PageImage.EventCancelled Then ' update failed
		If PageImage.CurrentAction = "edit" Then
			PageImage_list.RestoreFormValues() ' Restore form values
		End If
		If PageImage.CurrentAction = "gridedit" Then
			PageImage_list.RestoreCurrentRowFormValues(PageImage_list.lRowIndex) ' Restore form values
		End If
	End If
	If PageImage.RowType = EW_ROWTYPE_EDIT Then ' Edit row
		PageImage_list.lEditRowCnt = PageImage_list.lEditRowCnt + 1
		PageImage.RowClientEvents = "onmouseover='this.edit=true;ew_MouseOver(event, this);' onmouseout='ew_MouseOut(event, this);' onclick='ew_Click(event, this);'"
	End If
	If PageImage.RowType = EW_ROWTYPE_ADD OrElse PageImage.RowType = EW_ROWTYPE_EDIT Then ' Add / Edit row
		PageImage.CssClass = "ewTableEditRow"
	End If

	' Render row
	PageImage_list.RenderRow()
%>
	<tr<%= PageImage.RowAttributes %>>
<% If PageImage.RowType = EW_ROWTYPE_ADD OrElse PageImage.RowType = EW_ROWTYPE_EDIT Then %>
<% If PageImage.CurrentAction = "edit" Then %>
<td colspan="<%= PageImage_list.lOptionCnt %>" align="right"><span class="aspnetmaker">
<a href="" onclick="f=ew_GetForm('fPageImagelist');if (PageImage_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/update.gif' alt='Update' title='Update' width='16' height='16' border='0'></a>&nbsp;<a href="<%= PageImage_list.PageUrl %>a=cancel"><img src='images/cancel.gif' alt='Cancel' title='Cancel' width='16' height='16' border='0'></a>
<input type="hidden" name="a_list" id="a_list" value="update" />
</span></td>
<% End If %>
<%
	If PageImage.CurrentAction = "gridedit" Then
		PageImage_list.sMultiSelectKey = PageImage_list.sMultiSelectKey & "<input type=""hidden"" name=""k" & PageImage_list.lRowIndex & "_key"" id=""k" & PageImage_list.lRowIndex & "_key"" value=""" & PageImage.PageImageID.CurrentValue & """ />"
	End If
%>
<% Else %>
<% If PageImage.Export = "" Then %>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= PageImage.ViewUrl %>"><img src='images/view.gif' alt='View' title='View' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= PageImage.EditUrl %>"><img src='images/edit.gif' alt='Edit' title='Edit' width='16' height='16' border='0'></a><span class="ewSeparator">&nbsp;|&nbsp;</span><a href="<%= PageImage.InlineEditUrl %>"><img src='images/inlineedit.gif' alt='Inline Edit' title='Inline Edit' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= PageImage.CopyUrl %>"><img src='images/copy.gif' alt='Copy' title='Copy' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= PageImage.DeleteUrl %>"><img src='images/delete.gif' alt='Delete' title='Delete' width='16' height='16' border='0'></a>
</span></td>
<%

' Custom list options
For i As Integer = 0 to PageImage_list.ListOptions.Items.Count -1
	If PageImage_list.ListOptions.Items(i).Visible Then Response.Write(PageImage_list.ListOptions.Items(i).BodyCellHtml)
Next
%>
<% End If %>
<% End If %>
	<% If PageImage.zPageID.Visible Then ' PageID %>
		<td<%= PageImage.zPageID.CellAttributes %>>
<% If PageImage.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<select id="x<%= PageImage_list.lRowIndex %>_zPageID" name="x<%= PageImage_list.lRowIndex %>_zPageID"<%= PageImage.zPageID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(PageImage.zPageID.EditValue) Then
	arwrk = PageImage.zPageID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), PageImage.zPageID.CurrentValue) Then
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
If emptywrk Then PageImage.zPageID.OldValue = ""
%>
</select>
<input type="hidden" name="o<%= PageImage_list.lRowIndex %>_zPageID" id="o<%= PageImage_list.lRowIndex %>_zPageID" value="<%= ew_HTMLEncode(PageImage.zPageID.OldValue) %>" />
<% End If %>
<% If PageImage.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<select id="x<%= PageImage_list.lRowIndex %>_zPageID" name="x<%= PageImage_list.lRowIndex %>_zPageID"<%= PageImage.zPageID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(PageImage.zPageID.EditValue) Then
	arwrk = PageImage.zPageID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), PageImage.zPageID.CurrentValue) Then
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
If emptywrk Then PageImage.zPageID.OldValue = ""
%>
</select>
<% End If %>
<% If PageImage.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= PageImage.zPageID.ViewAttributes %>><%= PageImage.zPageID.ListViewValue %></div>
<% End If %>
<% If PageImage.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<input type="hidden" name="o<%= PageImage_list.lRowIndex %>_PageImageID" id="o<%= PageImage_list.lRowIndex %>_PageImageID" value="<%= ew_HTMLEncode(PageImage.PageImageID.OldValue) %>" />
<% End If %>
<% If PageImage.RowType = EW_ROWTYPE_EDIT Then %>
<input type="hidden" name="x<%= PageImage_list.lRowIndex %>_PageImageID" id="x<%= PageImage_list.lRowIndex %>_PageImageID" value="<%= ew_HTMLEncode(PageImage.PageImageID.CurrentValue) %>" />
<% End If %>
</td>
	<% End If %>
	<% If PageImage.ImageID.Visible Then ' ImageID %>
		<td<%= PageImage.ImageID.CellAttributes %>>
<% If PageImage.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<select id="x<%= PageImage_list.lRowIndex %>_ImageID" name="x<%= PageImage_list.lRowIndex %>_ImageID"<%= PageImage.ImageID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(PageImage.ImageID.EditValue) Then
	arwrk = PageImage.ImageID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), PageImage.ImageID.CurrentValue) Then
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
If emptywrk Then PageImage.ImageID.OldValue = ""
%>
</select>
<input type="hidden" name="o<%= PageImage_list.lRowIndex %>_ImageID" id="o<%= PageImage_list.lRowIndex %>_ImageID" value="<%= ew_HTMLEncode(PageImage.ImageID.OldValue) %>" />
<% End If %>
<% If PageImage.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<select id="x<%= PageImage_list.lRowIndex %>_ImageID" name="x<%= PageImage_list.lRowIndex %>_ImageID"<%= PageImage.ImageID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(PageImage.ImageID.EditValue) Then
	arwrk = PageImage.ImageID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), PageImage.ImageID.CurrentValue) Then
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
If emptywrk Then PageImage.ImageID.OldValue = ""
%>
</select>
<% End If %>
<% If PageImage.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= PageImage.ImageID.ViewAttributes %>><%= PageImage.ImageID.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	<% If PageImage.PageImagePosition.Visible Then ' PageImagePosition %>
		<td<%= PageImage.PageImagePosition.CellAttributes %>>
<% If PageImage.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<input type="text" name="x<%= PageImage_list.lRowIndex %>_PageImagePosition" id="x<%= PageImage_list.lRowIndex %>_PageImagePosition" size="30" value="<%= PageImage.PageImagePosition.EditValue %>"<%= PageImage.PageImagePosition.EditAttributes %> />
<input type="hidden" name="o<%= PageImage_list.lRowIndex %>_PageImagePosition" id="o<%= PageImage_list.lRowIndex %>_PageImagePosition" value="<%= ew_HTMLEncode(PageImage.PageImagePosition.OldValue) %>" />
<% End If %>
<% If PageImage.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<input type="text" name="x<%= PageImage_list.lRowIndex %>_PageImagePosition" id="x<%= PageImage_list.lRowIndex %>_PageImagePosition" size="30" value="<%= PageImage.PageImagePosition.EditValue %>"<%= PageImage.PageImagePosition.EditAttributes %> />
<% End If %>
<% If PageImage.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= PageImage.PageImagePosition.ViewAttributes %>><%= PageImage.PageImagePosition.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	</tr>
<% If PageImage.RowType = EW_ROWTYPE_ADD Then %>
<% End If %>
<% If PageImage.RowType = EW_ROWTYPE_EDIT Then %>
<% End If %>
<%
	End If
Loop
%>
</tbody>
</table>
<% End If %>
<% If PageImage.CurrentAction = "gridadd" Then %>
<input type="hidden" name="a_list" id="a_list" value="gridinsert" />
<input type="hidden" name="key_count" id="key_count" value="<%= PageImage_list.lRowIndex %>" />
<% End If %>
<% If PageImage.CurrentAction = "edit" Then %>
<input type="hidden" name="key_count" id="key_count" value="<%= PageImage_list.lRowIndex %>" />
<% End If %>
<% If PageImage.CurrentAction = "gridedit" Then %>
<input type="hidden" name="a_list" id="a_list" value="gridupdate" />
<input type="hidden" name="key_count" id="key_count" value="<%= PageImage_list.lRowIndex %>" />
<%= PageImage_list.sMultiSelectKey %>
<% End If %>
</form>
<%

' Close recordset
Rs.Close()
Rs.Dispose()
%>
</div>
<% If PageImage_list.lTotalRecs > 0 Then %>
<% If PageImage.Export = "" Then %>
<div class="ewGridLowerPanel">
<% If PageImage.CurrentAction <> "gridadd" AndAlso PageImage.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If PageImage_list.Pager Is Nothing Then PageImage_list.Pager = New cPrevNextPager(PageImage_list.lStartRec, PageImage_list.lDisplayRecs, PageImage_list.lTotalRecs) %>
<% If PageImage_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If PageImage_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= PageImage_list.PageUrl %>start=<%= PageImage_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If PageImage_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= PageImage_list.PageUrl %>start=<%= PageImage_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= PageImage_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If PageImage_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= PageImage_list.PageUrl %>start=<%= PageImage_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If PageImage_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= PageImage_list.PageUrl %>start=<%= PageImage_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= PageImage_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= PageImage_list.Pager.FromIndex %> to <%= PageImage_list.Pager.ToIndex %> of <%= PageImage_list.Pager.RecordCount %></span>
<% Else %>
	<% If PageImage_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If PageImage_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="PageImage" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If PageImage_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If PageImage_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If PageImage_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If PageImage.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<% 'If PageImage_list.lTotalRecs > 0 Then %>
<div class="aspnetmaker">
<% If PageImage.CurrentAction <> "gridadd" AndAlso PageImage.CurrentAction <> "gridedit" Then ' Not grid add/edit mode %>
<a href="<%= PageImage.AddUrl %>">Add</a>&nbsp;&nbsp;
<a href="<%= PageImage_list.PageUrl %>a=gridadd">Grid Add</a>&nbsp;&nbsp;
<% If PageImage_list.lTotalRecs > 0 Then %>
<a href="<%= PageImage_list.PageUrl %>a=gridedit">Grid Edit</a>&nbsp;&nbsp;
<% End If %>
<% Else ' Grid add/edit mode %>
<% If PageImage.CurrentAction = "gridadd" Then %>
<a href="" onclick="f=ew_GetForm('fPageImagelist');if (PageImage_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/insert.gif' alt='Insert' title='Insert' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<% If PageImage.CurrentAction = "gridedit" Then %>
<a href="" onclick="f=ew_GetForm('fPageImagelist');if (PageImage_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/update.gif' alt='Save' title='Save' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<a href="<%= PageImage_list.PageUrl %>a=cancel"><img src='images/cancel.gif' alt='Cancel' title='Cancel' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
</div>
<% 'End If %>
</div>
<% End If %>
<% End If %>
</td></tr></table>
<% If PageImage.Export = "" AndAlso PageImage.CurrentAction = "" Then %>
<script type="text/javascript">
<!--
//ew_ToggleSearchPanel(PageImage_list); // uncomment to init search panel as collapsed
//-->
</script>
<% End If %>
<% If PageImage.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
