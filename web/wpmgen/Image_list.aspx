<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="Image_list.aspx.vb" Inherits="Image_list" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If Image.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var Image_list = new ew_Page("Image_list");
// page properties
Image_list.PageID = "list"; // page ID
var EW_PAGE_ID = Image_list.PageID; // for backward compatibility
// extend page with ValidateForm function
Image_list.ValidateForm = function(fobj) {
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
		elm = fobj.elements["x" + infix + "_CompanyID"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field - Company");
		elm = fobj.elements["x" + infix + "_ImageName"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field - Name");
		} // End Grid Add checking
	}
	if (fobj.a_list && fobj.a_list.value == "gridinsert" && addcnt == 0) { // No row added
		alert("No records to be added");
		return false;
	}
	return true;
}
// Extend page with empty row check
Image_list.EmptyRow = function(fobj, infix) {
	if (ew_ValueChanged(fobj, infix, "CompanyID")) return false;
	if (ew_ValueChanged(fobj, infix, "title")) return false;
	if (ew_ValueChanged(fobj, infix, "ImageName")) return false;
	return true;
}
<% If EW_CLIENT_VALIDATE Then %>
Image_list.ValidateRequired = true; // uses JavaScript validation
<% Else %>
Image_list.ValidateRequired = false; // no JavaScript validation
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
<% If Image.Export = "" Then %>
<% End If %>
<%
If Image.CurrentAction = "gridadd" Then Image.CurrentFilter = "0=1"

' Load recordset
Rs = Image_list.LoadRecordset()
If Image.CurrentAction = "gridadd" Then
	Image_list.lStartRec = 1
	If Image_list.lDisplayRecs <= 0 Then Image_list.lDisplayRecs = 25
	Image_list.lTotalRecs = Image_list.lDisplayRecs
	Image_list.lStopRec = Image_list.lDisplayRecs
Else
	Image_list.lStartRec = 1
	If Image_list.lDisplayRecs <= 0 Then ' Display all records
		Image_list.lDisplayRecs = Image_list.lTotalRecs
	End If
	If Not (Image.ExportAll AndAlso Image.Export <> "") Then
		Image_list.SetUpStartRec() ' Set up start record position
	End If
End If
%>
<p><span class="aspnetmaker" style="white-space: nowrap;">TABLE: Site Image
<% If Image.Export = "" AndAlso Image.CurrentAction = "" Then %>
&nbsp;&nbsp;<a href="<%= Image_list.PageUrl %>export=excel"><img src='images/exportxls.gif' alt='Export to Excel' title='Export to Excel' width='16' height='16' border='0'></a>
<% End If %>
</span></p>
<% If Image.Export = "" AndAlso Image.CurrentAction = "" Then %>
<a href="javascript:ew_ToggleSearchPanel(Image_list);" style="text-decoration: none;"><img id="Image_list_SearchImage" src="images/collapse.gif" alt="" width="9" height="9" border="0"></a><span class="aspnetmaker">&nbsp;Search</span><br>
<div id="Image_list_SearchPanel">
<form name="fImagelistsrch" id="fImagelistsrch" class="ewForm">
<input type="hidden" id="t" name="t" value="Image" />
<table class="ewBasicSearch">
	<tr>
		<td><span class="aspnetmaker">
			<input type="text" name="<%= EW_TABLE_BASIC_SEARCH %>" id="<%= EW_TABLE_BASIC_SEARCH %>" size="20" value="<%= ew_HtmlEncode(Image.BasicSearchKeyword) %>" />
			<input type="Submit" name="Submit" id="Submit" value="Search (*)" />&nbsp;
			<a href="<%= Image_list.PageUrl %>cmd=reset">Show all</a>&nbsp;
			<a href="Image_srch.aspx">Advanced Search</a>&nbsp;
		</span></td>
	</tr>
	<tr>
	<td><span class="aspnetmaker"><label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value=""<% If Image.BasicSearchType = "" Then %> checked="checked"<% End If %> />Exact phrase</label>&nbsp;&nbsp;<label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value="AND"<% If Image.BasicSearchType = "AND" Then %> checked="checked"<% End If %> />All words</label>&nbsp;&nbsp;<label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value="OR"<% If Image.BasicSearchType = "OR" Then %> checked="checked"<% End If %> />Any word</label></span></td>
	</tr>
</table>
</form>
</div>
<% End If %>
<% Image_list.ShowMessage() %>
<br />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<% If Image.Export = "" Then %>
<div class="ewGridUpperPanel">
<% If Image.CurrentAction <> "gridadd" AndAlso Image.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If Image_list.Pager Is Nothing Then Image_list.Pager = New cPrevNextPager(Image_list.lStartRec, Image_list.lDisplayRecs, Image_list.lTotalRecs) %>
<% If Image_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If Image_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= Image_list.PageUrl %>start=<%= Image_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If Image_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= Image_list.PageUrl %>start=<%= Image_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= Image_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If Image_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= Image_list.PageUrl %>start=<%= Image_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If Image_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= Image_list.PageUrl %>start=<%= Image_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= Image_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= Image_list.Pager.FromIndex %> to <%= Image_list.Pager.ToIndex %> of <%= Image_list.Pager.RecordCount %></span>
<% Else %>
	<% If Image_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If Image_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="Image" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If Image_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If Image_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If Image_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If Image.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<div class="aspnetmaker">
<% If Image.CurrentAction <> "gridadd" AndAlso Image.CurrentAction <> "gridedit" Then ' Not grid add/edit mode %>
<a href="<%= Image.AddUrl %>">Add</a>&nbsp;&nbsp;
<a href="<%= Image_list.PageUrl %>a=gridadd">Grid Add</a>&nbsp;&nbsp;
<% If Image_list.lTotalRecs > 0 Then %>
<a href="<%= Image_list.PageUrl %>a=gridedit">Grid Edit</a>&nbsp;&nbsp;
<% End If %>
<% Else ' Grid add/edit mode %>
<% If Image.CurrentAction = "gridadd" Then %>
<a href="" onclick="f=ew_GetForm('fImagelist');if (Image_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/insert.gif' alt='Insert' title='Insert' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<% If Image.CurrentAction = "gridedit" Then %>
<a href="" onclick="f=ew_GetForm('fImagelist');if (Image_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/update.gif' alt='Save' title='Save' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<a href="<%= Image_list.PageUrl %>a=cancel"><img src='images/cancel.gif' alt='Cancel' title='Cancel' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
</div>
</div>
<% End If %>
<div class="ewGridMiddlePanel">
<form name="fImagelist" id="fImagelist" class="ewForm" method="post">
<input type="hidden" name="t" id="t" value="Image" />
<% If Image_list.lTotalRecs > 0 Then %>
<table cellspacing="0" rowhighlightclass="ewTableHighlightRow" rowselectclass="ewTableSelectRow" roweditclass="ewTableEditRow" class="ewTable ewTableSeparate">
<%
	Image_list.lOptionCnt = 0
	Image_list.lOptionCnt = Image_list.lOptionCnt + 1 ' View
	Image_list.lOptionCnt = Image_list.lOptionCnt + 1 ' Edit
	Image_list.lOptionCnt = Image_list.lOptionCnt + 1 ' Copy
	Image_list.lOptionCnt = Image_list.lOptionCnt + 1 ' Delete
	Image_list.lOptionCnt = Image_list.lOptionCnt + Image_list.ListOptions.Items.Count ' Custom list options
%>
<%= Image.TableCustomInnerHTML %>
<thead><!-- Table header -->
	<tr class="ewTableHeader">
<% If Image.Export = "" Then %>
<% If Image.CurrentAction <> "gridadd" AndAlso Image.CurrentAction <> "gridedit" Then %>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<%

' Custom list options
For i As Integer = 0 to Image_list.ListOptions.Items.Count -1
	If Image_list.ListOptions.Items(i).Visible Then Response.Write(Image_list.ListOptions.Items(i).HeaderCellHtml)
Next
%>
<% End If %>
<% End If %>
<% If Image.CompanyID.Visible Then ' CompanyID %>
	<% If Image.SortUrl(Image.CompanyID) = "" Then %>
		<td style="white-space: nowrap;">Company</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= Image.SortUrl(Image.CompanyID) %>',1);" style="white-space: nowrap;">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Company</td><td style="width: 10px;"><% If Image.CompanyID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Image.CompanyID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If Image.title.Visible Then ' title %>
	<% If Image.SortUrl(Image.title) = "" Then %>
		<td style="white-space: nowrap;">Title</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= Image.SortUrl(Image.title) %>',1);" style="white-space: nowrap;">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Title&nbsp;(*)</td><td style="width: 10px;"><% If Image.title.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Image.title.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If Image.ImageName.Visible Then ' ImageName %>
	<% If Image.SortUrl(Image.ImageName) = "" Then %>
		<td style="white-space: nowrap;">Name</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= Image.SortUrl(Image.ImageName) %>',1);" style="white-space: nowrap;">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Name&nbsp;(*)</td><td style="width: 10px;"><% If Image.ImageName.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Image.ImageName.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
	</tr>
</thead>
<tbody><!-- Table body -->
<%
If (Image.ExportAll AndAlso Image.Export <> "") Then
	Image_list.lStopRec = Image_list.lTotalRecs
Else
	Image_list.lStopRec = Image_list.lStartRec + Image_list.lDisplayRecs - 1 ' Set the last record to display
End If
If Image.CurrentAction = "gridadd" AndAlso Image_list.lStopRec = -1 Then
	Image_list.lStopRec = EW_GRIDADD_ROWS
End If 

' Move to first record
For i As Integer = 1 to Image_list.lStartRec - 1
	If Rs.Read() Then	Image_list.lRecCnt = Image_list.lRecCnt + 1
Next		
Image_list.lRowCnt = 0
Image_list.lEditRowCnt = 0
If Image.CurrentAction = "edit" Then Image_list.lRowIndex = 1
If Image.CurrentAction = "gridadd" Then Image_list.lRowIndex = 0
If Image.CurrentAction = "gridedit" Then Image_list.lRowIndex = 0

' Output data rows
Do While (Image.CurrentAction = "gridadd" OrElse Rs.Read()) AndAlso (Image_list.lRecCnt < Image_list.lStopRec)
	Image_list.lRecCnt = Image_list.lRecCnt + 1
	If Image_list.lRecCnt >= Image_list.lStartRec Then
		Image_list.lRowCnt = Image_list.lRowCnt + 1
		If Image.CurrentAction = "gridadd" OrElse Image.CurrentAction = "gridedit" Then Image_list.lRowIndex = Image_list.lRowIndex + 1
	Image.CssClass = ""
	Image.CssStyle = ""
	Image.RowClientEvents = "onmouseover='ew_MouseOver(event, this);' onmouseout='ew_MouseOut(event, this);' onclick='ew_Click(event, this);'"
	If Image.CurrentAction = "gridadd" Then
		Image_list.LoadDefaultValues() ' Load default values
	Else
		Image_list.LoadRowValues(Rs) ' Load row values
	End If
	Image.RowType = EW_ROWTYPE_VIEW ' Render view
	If Image.CurrentAction = "gridadd" Then ' Grid add
		Image.RowType = EW_ROWTYPE_ADD ' Render add
	End If
	If Image.CurrentAction = "gridadd" AndAlso Image.EventCancelled Then ' Insert failed
		Image_list.RestoreCurrentRowFormValues(Image_list.lRowIndex) ' Restore form values
	End If
	If Image.CurrentAction = "edit" Then
		If Image_list.CheckInlineEditKey() AndAlso Image_list.lEditRowCnt = 0 Then ' Inline edit
			Image.RowType = EW_ROWTYPE_EDIT ' Render edit
		End If
	End If
	If Image.CurrentAction = "gridedit" Then ' Grid edit
		Image.RowType = EW_ROWTYPE_EDIT ' Render edit
	End If
	If Image.RowType = EW_ROWTYPE_EDIT AndAlso Image.EventCancelled Then ' update failed
		If Image.CurrentAction = "edit" Then
			Image_list.RestoreFormValues() ' Restore form values
		End If
		If Image.CurrentAction = "gridedit" Then
			Image_list.RestoreCurrentRowFormValues(Image_list.lRowIndex) ' Restore form values
		End If
	End If
	If Image.RowType = EW_ROWTYPE_EDIT Then ' Edit row
		Image_list.lEditRowCnt = Image_list.lEditRowCnt + 1
		Image.RowClientEvents = "onmouseover='this.edit=true;ew_MouseOver(event, this);' onmouseout='ew_MouseOut(event, this);' onclick='ew_Click(event, this);'"
	End If
	If Image.RowType = EW_ROWTYPE_ADD OrElse Image.RowType = EW_ROWTYPE_EDIT Then ' Add / Edit row
		Image.CssClass = "ewTableEditRow"
	End If

	' Render row
	Image_list.RenderRow()
%>
	<tr<%= Image.RowAttributes %>>
<% If Image.RowType = EW_ROWTYPE_ADD OrElse Image.RowType = EW_ROWTYPE_EDIT Then %>
<% If Image.CurrentAction = "edit" Then %>
<td colspan="<%= Image_list.lOptionCnt %>" align="right"><span class="aspnetmaker">
<a href="" onclick="f=ew_GetForm('fImagelist');if (Image_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/update.gif' alt='Update' title='Update' width='16' height='16' border='0'></a>&nbsp;<a href="<%= Image_list.PageUrl %>a=cancel"><img src='images/cancel.gif' alt='Cancel' title='Cancel' width='16' height='16' border='0'></a>
<input type="hidden" name="a_list" id="a_list" value="update" />
</span></td>
<% End If %>
<%
	If Image.CurrentAction = "gridedit" Then
		Image_list.sMultiSelectKey = Image_list.sMultiSelectKey & "<input type=""hidden"" name=""k" & Image_list.lRowIndex & "_key"" id=""k" & Image_list.lRowIndex & "_key"" value=""" & Image.ImageID.CurrentValue & """ />"
	End If
%>
<% Else %>
<% If Image.Export = "" Then %>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= Image.ViewUrl %>"><img src='images/view.gif' alt='View' title='View' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= Image.EditUrl %>"><img src='images/edit.gif' alt='Edit' title='Edit' width='16' height='16' border='0'></a><span class="ewSeparator">&nbsp;|&nbsp;</span><a href="<%= Image.InlineEditUrl %>"><img src='images/inlineedit.gif' alt='Inline Edit' title='Inline Edit' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= Image.CopyUrl %>"><img src='images/copy.gif' alt='Copy' title='Copy' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= Image.DeleteUrl %>"><img src='images/delete.gif' alt='Delete' title='Delete' width='16' height='16' border='0'></a>
</span></td>
<%

' Custom list options
For i As Integer = 0 to Image_list.ListOptions.Items.Count -1
	If Image_list.ListOptions.Items(i).Visible Then Response.Write(Image_list.ListOptions.Items(i).BodyCellHtml)
Next
%>
<% End If %>
<% End If %>
	<% If Image.CompanyID.Visible Then ' CompanyID %>
		<td<%= Image.CompanyID.CellAttributes %>>
<% If Image.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<select id="x<%= Image_list.lRowIndex %>_CompanyID" name="x<%= Image_list.lRowIndex %>_CompanyID"<%= Image.CompanyID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Image.CompanyID.EditValue) Then
	arwrk = Image.CompanyID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Image.CompanyID.CurrentValue) Then
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
If emptywrk Then Image.CompanyID.OldValue = ""
%>
</select>
<input type="hidden" name="o<%= Image_list.lRowIndex %>_CompanyID" id="o<%= Image_list.lRowIndex %>_CompanyID" value="<%= ew_HTMLEncode(Image.CompanyID.OldValue) %>" />
<% End If %>
<% If Image.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<select id="x<%= Image_list.lRowIndex %>_CompanyID" name="x<%= Image_list.lRowIndex %>_CompanyID"<%= Image.CompanyID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Image.CompanyID.EditValue) Then
	arwrk = Image.CompanyID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Image.CompanyID.CurrentValue) Then
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
If emptywrk Then Image.CompanyID.OldValue = ""
%>
</select>
<% End If %>
<% If Image.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= Image.CompanyID.ViewAttributes %>><%= Image.CompanyID.ListViewValue %></div>
<% End If %>
<% If Image.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<input type="hidden" name="o<%= Image_list.lRowIndex %>_ImageID" id="o<%= Image_list.lRowIndex %>_ImageID" value="<%= ew_HTMLEncode(Image.ImageID.OldValue) %>" />
<% End If %>
<% If Image.RowType = EW_ROWTYPE_EDIT Then %>
<input type="hidden" name="x<%= Image_list.lRowIndex %>_ImageID" id="x<%= Image_list.lRowIndex %>_ImageID" value="<%= ew_HTMLEncode(Image.ImageID.CurrentValue) %>" />
<% End If %>
</td>
	<% End If %>
	<% If Image.title.Visible Then ' title %>
		<td<%= Image.title.CellAttributes %>>
<% If Image.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<input type="text" name="x<%= Image_list.lRowIndex %>_title" id="x<%= Image_list.lRowIndex %>_title" size="30" maxlength="50" value="<%= Image.title.EditValue %>"<%= Image.title.EditAttributes %> />
<input type="hidden" name="o<%= Image_list.lRowIndex %>_title" id="o<%= Image_list.lRowIndex %>_title" value="<%= ew_HTMLEncode(Image.title.OldValue) %>" />
<% End If %>
<% If Image.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<input type="text" name="x<%= Image_list.lRowIndex %>_title" id="x<%= Image_list.lRowIndex %>_title" size="30" maxlength="50" value="<%= Image.title.EditValue %>"<%= Image.title.EditAttributes %> />
<% End If %>
<% If Image.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= Image.title.ViewAttributes %>><%= Image.title.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	<% If Image.ImageName.Visible Then ' ImageName %>
		<td<%= Image.ImageName.CellAttributes %>>
<% If Image.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<input type="text" name="x<%= Image_list.lRowIndex %>_ImageName" id="x<%= Image_list.lRowIndex %>_ImageName" size="30" maxlength="50" value="<%= Image.ImageName.EditValue %>"<%= Image.ImageName.EditAttributes %> />
<input type="hidden" name="o<%= Image_list.lRowIndex %>_ImageName" id="o<%= Image_list.lRowIndex %>_ImageName" value="<%= ew_HTMLEncode(Image.ImageName.OldValue) %>" />
<% End If %>
<% If Image.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<input type="text" name="x<%= Image_list.lRowIndex %>_ImageName" id="x<%= Image_list.lRowIndex %>_ImageName" size="30" maxlength="50" value="<%= Image.ImageName.EditValue %>"<%= Image.ImageName.EditAttributes %> />
<% End If %>
<% If Image.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= Image.ImageName.ViewAttributes %>><%= Image.ImageName.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	</tr>
<% If Image.RowType = EW_ROWTYPE_ADD Then %>
<% End If %>
<% If Image.RowType = EW_ROWTYPE_EDIT Then %>
<% End If %>
<%
	End If
Loop
%>
</tbody>
</table>
<% End If %>
<% If Image.CurrentAction = "gridadd" Then %>
<input type="hidden" name="a_list" id="a_list" value="gridinsert" />
<input type="hidden" name="key_count" id="key_count" value="<%= Image_list.lRowIndex %>" />
<% End If %>
<% If Image.CurrentAction = "edit" Then %>
<input type="hidden" name="key_count" id="key_count" value="<%= Image_list.lRowIndex %>" />
<% End If %>
<% If Image.CurrentAction = "gridedit" Then %>
<input type="hidden" name="a_list" id="a_list" value="gridupdate" />
<input type="hidden" name="key_count" id="key_count" value="<%= Image_list.lRowIndex %>" />
<%= Image_list.sMultiSelectKey %>
<% End If %>
</form>
<%

' Close recordset
Rs.Close()
Rs.Dispose()
%>
</div>
<% If Image_list.lTotalRecs > 0 Then %>
<% If Image.Export = "" Then %>
<div class="ewGridLowerPanel">
<% If Image.CurrentAction <> "gridadd" AndAlso Image.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If Image_list.Pager Is Nothing Then Image_list.Pager = New cPrevNextPager(Image_list.lStartRec, Image_list.lDisplayRecs, Image_list.lTotalRecs) %>
<% If Image_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If Image_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= Image_list.PageUrl %>start=<%= Image_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If Image_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= Image_list.PageUrl %>start=<%= Image_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= Image_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If Image_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= Image_list.PageUrl %>start=<%= Image_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If Image_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= Image_list.PageUrl %>start=<%= Image_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= Image_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= Image_list.Pager.FromIndex %> to <%= Image_list.Pager.ToIndex %> of <%= Image_list.Pager.RecordCount %></span>
<% Else %>
	<% If Image_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If Image_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="Image" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If Image_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If Image_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If Image_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If Image.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<% 'If Image_list.lTotalRecs > 0 Then %>
<div class="aspnetmaker">
<% If Image.CurrentAction <> "gridadd" AndAlso Image.CurrentAction <> "gridedit" Then ' Not grid add/edit mode %>
<a href="<%= Image.AddUrl %>">Add</a>&nbsp;&nbsp;
<a href="<%= Image_list.PageUrl %>a=gridadd">Grid Add</a>&nbsp;&nbsp;
<% If Image_list.lTotalRecs > 0 Then %>
<a href="<%= Image_list.PageUrl %>a=gridedit">Grid Edit</a>&nbsp;&nbsp;
<% End If %>
<% Else ' Grid add/edit mode %>
<% If Image.CurrentAction = "gridadd" Then %>
<a href="" onclick="f=ew_GetForm('fImagelist');if (Image_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/insert.gif' alt='Insert' title='Insert' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<% If Image.CurrentAction = "gridedit" Then %>
<a href="" onclick="f=ew_GetForm('fImagelist');if (Image_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/update.gif' alt='Save' title='Save' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<a href="<%= Image_list.PageUrl %>a=cancel"><img src='images/cancel.gif' alt='Cancel' title='Cancel' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
</div>
<% 'End If %>
</div>
<% End If %>
<% End If %>
</td></tr></table>
<% If Image.Export = "" AndAlso Image.CurrentAction = "" Then %>
<script type="text/javascript">
<!--
//ew_ToggleSearchPanel(Image_list); // uncomment to init search panel as collapsed
//-->
</script>
<% End If %>
<% If Image.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
