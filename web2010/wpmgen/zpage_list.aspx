<%@ Page ClassName="zpage_list" Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="zpage_list.aspx.vb" Inherits="zpage_list" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<%@ Register TagPrefix="wpmWebsite" TagName="MasterTable_Company" Src="company_master.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If zPage.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var zPage_list = new ew_Page("zPage_list");
// page properties
zPage_list.PageID = "list"; // page ID
zPage_list.FormID = "fzPagelist"; // form ID 
var EW_PAGE_ID = zPage_list.PageID; // for backward compatibility
// extend page with Form_CustomValidate function
zPage_list.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
zPage_list.ValidateRequired = true; // uses JavaScript validation
<% Else %>
zPage_list.ValidateRequired = false; // no JavaScript validation
<% End If %>
//-->
</script>
<style>
/* main table preview row color *** A8 */
.ewTablePreviewRow {
	background-color: inherit; /* preview row */
}
</style>
<script language="JavaScript" type="text/javascript">
<!--
// PreviewRow extension
var ew_AjaxDetailsTimer = null;
var EW_PREVIEW_SINGLE_ROW = false;
var EW_PREVIEW_IMAGE_CLASSNAME = "ewPreviewRowImage";
var EW_PREVIEW_SHOW_IMAGE = "images/expand.gif";
var EW_PREVIEW_HIDE_IMAGE = "images/collapse.gif";
var EW_PREVIEW_LOADING_IMAGE = "images/loading.gif";
var EW_PREVIEW_LOADING_TEXT = "<%= ew_JsEncode2(Language.Phrase("Loading")) %>"; // lang phrase for loading
// add row

function ew_AddRowToTable(r) {
	var row, cell;
	var tb = ewDom.getAncestorByTagName(r, "TBODY");
	if (EW_PREVIEW_SINGLE_ROW) {
		row = ewDom.getElementBy(function(node) { return ewDom.hasClass(node, EW_TABLE_PREVIEW_ROW_CLASSNAME)}, "TR", tb);
		ew_RemoveRowFromTable(row);
	}
	var sr = ewDom.getNextSiblingBy(r, function(node) { return node.tagName == "TR"});
	if (sr && ewDom.hasClass(sr, EW_TABLE_PREVIEW_ROW_CLASSNAME)) {
		row = sr; // existing sibling row
		if (row && row.cells && row.cells[0])
			cell = row.cells[0];
	} else {
		row = tb.insertRow(r.rowIndex); // new row
		if (row) {
			row.className = EW_TABLE_PREVIEW_ROW_CLASSNAME;
			var cell = row.insertCell(0);
			cell.style.borderRight = "0";
			var colcnt = r.cells.length;
			if (r.cells) {
				var spancnt = 0;
				for (var i = 0; i < colcnt; i++)
					spancnt += r.cells[i].colSpan;
				if (spancnt > 0)
					cell.colSpan = spancnt;
			}
			var pt = ewDom.getAncestorByTagName(row, "TABLE");
			if (pt) ew_SetupTable(pt);
		}
	}
	if (cell)
		cell.innerHTML = "<img src=\"" + EW_PREVIEW_LOADING_IMAGE + "\" style=\"border: 0; vertical-align: middle;\"> " + EW_PREVIEW_LOADING_TEXT;
	return row;
}
// remove row

function ew_RemoveRowFromTable(r) {
	if (r && r.parentNode)
		r.parentNode.removeChild(r);
}
// show results in new table row
var ew_AjaxHandleSuccess2 = function(o) {
	if (o.responseText !== undefined) {
		var row = o.argument.row;
		if (!row || !row.cells || !row.cells[0]) return;
		row.cells[0].innerHTML = o.responseText;
		var ct = ewDom.getElementBy(function(node) { return ewDom.hasClass(node, EW_TABLE_CLASS)}, "TABLE", row);
		if (ct) ew_SetupTable(ct);
		//clearTimeout(ew_AjaxDetailsTimer);
		//setTimeout("alert(ew_AjaxDetailsTimer);", 500);
	}
}
// show error in new table row
var ew_AjaxHandleFailure2 = function(o) {
	var row = o.argument.row;
	if (!row || !row.cells || !row.cells[0]) return;
	row.cells[0].innerHTML = o.responseText;
}
// show detail preview by table row expansion

function ew_AjaxShowDetails2(ev, link, url) {
	var img = ewDom.getElementBy(function(node) { return true; }, "IMG", link);
	var r = ewDom.getAncestorByTagName(link, "TR");
	if (!img || !r)
		return;
	var show = (img.src.substr(img.src.length - EW_PREVIEW_SHOW_IMAGE.length) == EW_PREVIEW_SHOW_IMAGE);
	if (show) {
		if (ew_AjaxDetailsTimer)
			clearTimeout(ew_AjaxDetailsTimer);		
		var row = ew_AddRowToTable(r);
		ew_AjaxDetailsTimer = setTimeout(function() { ewConnect.asyncRequest('GET', url, {success: ew_AjaxHandleSuccess2, failure: ew_AjaxHandleFailure2, argument:{id: link, row: row}}) }, 200);
		ewDom.getElementsByClassName(EW_PREVIEW_IMAGE_CLASSNAME, "IMG", r, function(node) {node.src = EW_PREVIEW_SHOW_IMAGE});
		img.src = EW_PREVIEW_HIDE_IMAGE;
	} else {	 
		var sr = ewDom.getNextSiblingBy(r, function(node) { return node.tagName == "TR"});
		if (sr && ewDom.hasClass(sr, EW_TABLE_PREVIEW_ROW_CLASSNAME))
			ew_RemoveRowFromTable(sr);
		img.src = EW_PREVIEW_SHOW_IMAGE;
	}
}
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
// To include another .js script, you can use, e.g.
// ew_ClientScriptInclude("my_javascript.js"); or
// ew_ClientScriptInclude("my_javascript.js", {onSuccess: function(o) { // your code here }});
//-->
</script>
<% End If %>
<% If zPage.Export = "" Then %>
<%
gsMasterReturnUrl = "company_list.aspx"
If zPage_list.sDbMasterFilter <> "" AndAlso zPage.CurrentMasterTable = "Company" Then
	If zPage_list.bMasterRecordExists Then
		If zPage.CurrentMasterTable = zPage.TableVar Then gsMasterReturnUrl = gsMasterReturnUrl & "?" & EW_TABLE_SHOW_MASTER & "="
%>
<wpmWebsite:MasterTable_Company id="MasterTable_Company" runat="server" />
<%
	End If
End If
%>
<% End If %>
<%

' Load recordset
Rs = zPage_list.LoadRecordset()
	zPage_list.lStartRec = 1
	If zPage_list.lDisplayRecs <= 0 Then ' Display all records
		zPage_list.lDisplayRecs = zPage_list.lTotalRecs
	End If
	If Not (zPage.ExportAll AndAlso zPage.Export <> "") Then
		zPage_list.SetUpStartRec() ' Set up start record position
	End If
%>
<p><span class="aspnetmaker" style="white-space: nowrap;"><%= Language.Phrase("TblTypeTABLE") %><%= zPage.TableCaption %>
<% If zPage.Export = "" AndAlso zPage.CurrentAction = "" Then %>
&nbsp;&nbsp;<a href="<%= zPage_list.ExportExcelUrl %>"><img src='images/exportxls.gif' alt='<%= ew_HtmlEncode(Language.Phrase("ExportToExcel")) %>' title='<%= ew_HtmlEncode(Language.Phrase("ExportToExcel")) %>' width='16' height='16' border='0'></a>
<% End If %>
</span></p>
<% If zPage.Export = "" AndAlso zPage.CurrentAction = "" Then %>
<a href="javascript:ew_ToggleSearchPanel(zPage_list);" style="text-decoration: none;"><img id="zPage_list_SearchImage" src="images/collapse.gif" alt="" width="9" height="9" border="0"></a><span class="aspnetmaker">&nbsp;<%= Language.Phrase("Search") %></span><br>
<div id="zPage_list_SearchPanel">
<form name="fzPagelistsrch" id="fzPagelistsrch" class="ewForm">
<input type="hidden" id="t" name="t" value="zPage" />
<table class="ewBasicSearch">
	<tr>
		<td><span class="aspnetmaker">
			<a href="<%= zPage_list.PageUrl %>cmd=reset"><%= Language.Phrase("ShowAll") %></a>&nbsp;
			<a href="zpage_srch.aspx"><%= Language.Phrase("AdvancedSearch") %></a>&nbsp;
		</span></td>
	</tr>
</table>
</form>
</div>
<% End If %>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(zPage_list.DebugMsg) %>
<% zPage_list.ShowMessage() %>
<br />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<form name="fzPagelist" id="fzPagelist" class="ewForm" method="post">
<div id="gmp_zPage" class="ewGridMiddlePanel">
<% If zPage_list.lTotalRecs > 0 Then %>
<table cellspacing="0" rowhighlightclass="ewTableHighlightRow" rowselectclass="ewTableSelectRow" roweditclass="ewTableEditRow" class="ewTable ewTableSeparate">
<%= zPage.TableCustomInnerHTML %>
<thead><!-- Table header -->
	<tr class="ewTableHeader">
<%

		' Render list options
		zPage_list.RenderListOptions()

		' Render list options (header, left)
		zPage_list.ListOptions.Render("header", "left")
%>
<% If zPage.ParentPageID.Visible Then ' ParentPageID %>
	<% If zPage.SortUrl(zPage.ParentPageID) = "" Then %>
		<td style="white-space: nowrap;"><%= zPage.ParentPageID.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= zPage.SortUrl(zPage.ParentPageID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn" style="white-space: nowrap;"><thead><tr><td><%= zPage.ParentPageID.FldCaption %></td><td style="width: 10px;"><% If zPage.ParentPageID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf zPage.ParentPageID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If zPage.zPageName.Visible Then ' PageName %>
	<% If zPage.SortUrl(zPage.zPageName) = "" Then %>
		<td style="white-space: nowrap;"><%= zPage.zPageName.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= zPage.SortUrl(zPage.zPageName) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn" style="white-space: nowrap;"><thead><tr><td><%= zPage.zPageName.FldCaption %></td><td style="width: 10px;"><% If zPage.zPageName.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf zPage.zPageName.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If zPage.PageTitle.Visible Then ' PageTitle %>
	<% If zPage.SortUrl(zPage.PageTitle) = "" Then %>
		<td style="white-space: nowrap;"><%= zPage.PageTitle.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= zPage.SortUrl(zPage.PageTitle) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn" style="white-space: nowrap;"><thead><tr><td><%= zPage.PageTitle.FldCaption %></td><td style="width: 10px;"><% If zPage.PageTitle.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf zPage.PageTitle.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If zPage.PageTypeID.Visible Then ' PageTypeID %>
	<% If zPage.SortUrl(zPage.PageTypeID) = "" Then %>
		<td style="white-space: nowrap;"><%= zPage.PageTypeID.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= zPage.SortUrl(zPage.PageTypeID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn" style="white-space: nowrap;"><thead><tr><td><%= zPage.PageTypeID.FldCaption %></td><td style="width: 10px;"><% If zPage.PageTypeID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf zPage.PageTypeID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If zPage.GroupID.Visible Then ' GroupID %>
	<% If zPage.SortUrl(zPage.GroupID) = "" Then %>
		<td style="white-space: nowrap;"><%= zPage.GroupID.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= zPage.SortUrl(zPage.GroupID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn" style="white-space: nowrap;"><thead><tr><td><%= zPage.GroupID.FldCaption %></td><td style="width: 10px;"><% If zPage.GroupID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf zPage.GroupID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If zPage.Active.Visible Then ' Active %>
	<% If zPage.SortUrl(zPage.Active) = "" Then %>
		<td style="white-space: nowrap;"><%= zPage.Active.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= zPage.SortUrl(zPage.Active) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn" style="white-space: nowrap;"><thead><tr><td><%= zPage.Active.FldCaption %></td><td style="width: 10px;"><% If zPage.Active.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf zPage.Active.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If zPage.PageOrder.Visible Then ' PageOrder %>
	<% If zPage.SortUrl(zPage.PageOrder) = "" Then %>
		<td style="white-space: nowrap;"><%= zPage.PageOrder.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= zPage.SortUrl(zPage.PageOrder) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn" style="white-space: nowrap;"><thead><tr><td><%= zPage.PageOrder.FldCaption %></td><td style="width: 10px;"><% If zPage.PageOrder.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf zPage.PageOrder.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<%

		' Render list options (header, right)
		zPage_list.ListOptions.Render("header", "right")
%>
	</tr>
</thead>
<tbody><!-- Table body -->
<%
If (zPage.ExportAll AndAlso zPage.Export <> "") Then
	zPage_list.lStopRec = zPage_list.lTotalRecs
Else
	zPage_list.lStopRec = zPage_list.lStartRec + zPage_list.lDisplayRecs - 1 ' Set the last record to display
End If
If zPage.CurrentAction = "gridadd" AndAlso zPage_list.lStopRec = -1 Then
	zPage_list.lStopRec = EW_GRIDADD_ROWS
End If 

' Move to first record
For i As Integer = 1 to zPage_list.lStartRec - 1
	If Rs.Read() Then	zPage_list.lRecCnt = zPage_list.lRecCnt + 1
Next		

' Initialize Aggregate
zPage.RowType = EW_ROWTYPE_AGGREGATEINIT
zPage_list.RenderRow()
zPage_list.lRowCnt = 0

' Output data rows
Do While (zPage.CurrentAction = "gridadd" OrElse Rs.Read()) AndAlso (zPage_list.lRecCnt < zPage_list.lStopRec)
	zPage_list.lRecCnt = zPage_list.lRecCnt + 1
	If zPage_list.lRecCnt >= zPage_list.lStartRec Then
		zPage_list.lRowCnt = zPage_list.lRowCnt + 1
	zPage.CssClass = ""
	zPage.CssStyle = ""
	zPage.RowAttrs.Clear()
	ew_SetAttr(zPage.RowAttrs, New String(){"onmouseover", "onmouseout", "onclick"}, New String(){"ew_MouseOver(event, this);", "ew_MouseOut(event, this);", "ew_Click(event, this);"})
	If zPage.CurrentAction = "gridadd" Then
		zPage_list.LoadDefaultValues() ' Load default values
	Else
		zPage_list.LoadRowValues(Rs) ' Load row values
	End If
	zPage.RowType = EW_ROWTYPE_VIEW ' Render view

	' Render row
	zPage_list.RenderRow()

	' Render list options
	zPage_list.RenderListOptions()
%>
	<tr<%= zPage.RowAttributes %>>
<%

		' Render list options (body, left)
		zPage_list.ListOptions.Render("body", "left")
%>
	<% If zPage.ParentPageID.Visible Then ' ParentPageID %>
		<td<%= zPage.ParentPageID.CellAttributes %>>
<div<%= zPage.ParentPageID.ViewAttributes %>><%= zPage.ParentPageID.ListViewValue %></div>
</td>
	<% End If %>
	<% If zPage.zPageName.Visible Then ' PageName %>
		<td<%= zPage.zPageName.CellAttributes %>>
<div<%= zPage.zPageName.ViewAttributes %>><%= zPage.zPageName.ListViewValue %></div>
</td>
	<% End If %>
	<% If zPage.PageTitle.Visible Then ' PageTitle %>
		<td<%= zPage.PageTitle.CellAttributes %>>
<div<%= zPage.PageTitle.ViewAttributes %>><%= zPage.PageTitle.ListViewValue %></div>
</td>
	<% End If %>
	<% If zPage.PageTypeID.Visible Then ' PageTypeID %>
		<td<%= zPage.PageTypeID.CellAttributes %>>
<div<%= zPage.PageTypeID.ViewAttributes %>><%= zPage.PageTypeID.ListViewValue %></div>
</td>
	<% End If %>
	<% If zPage.GroupID.Visible Then ' GroupID %>
		<td<%= zPage.GroupID.CellAttributes %>>
<div<%= zPage.GroupID.ViewAttributes %>><%= zPage.GroupID.ListViewValue %></div>
</td>
	<% End If %>
	<% If zPage.Active.Visible Then ' Active %>
		<td<%= zPage.Active.CellAttributes %>>
<% If Convert.ToString(zPage.Active.CurrentValue) = "1" Then %>
<input type="checkbox" value="<%= zPage.Active.ListViewValue %>" checked onclick="this.form.reset();" disabled="disabled" />
<% Else %>
<input type="checkbox" value="<%= zPage.Active.ListViewValue %>" onclick="this.form.reset();" disabled="disabled" />
<% End If %>
</td>
	<% End If %>
	<% If zPage.PageOrder.Visible Then ' PageOrder %>
		<td<%= zPage.PageOrder.CellAttributes %>>
<div<%= zPage.PageOrder.ViewAttributes %>><%= zPage.PageOrder.ListViewValue %></div>
</td>
	<% End If %>
<%

		' Render list options (body, right)
		zPage_list.ListOptions.Render("body", "right")
%>
	</tr>
<%
	End If
Loop
%>
</tbody>
</table>
<% End If %>
</div>
</form>
<%

' Close recordset
If Rs IsNot Nothing Then
	Rs.Close()
	Rs.Dispose()
End If
%>
<% If zPage.Export = "" Then %>
<div class="ewGridLowerPanel">
<% If zPage.CurrentAction <> "gridadd" AndAlso zPage.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If zPage_list.Pager Is Nothing Then zPage_list.Pager = New cPrevNextPager(zPage_list.lStartRec, zPage_list.lDisplayRecs, zPage_list.lTotalRecs) %>
<% If zPage_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker"><%= Language.Phrase("Page") %>&nbsp;</span></td>
<!--first page button-->
	<% If zPage_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= zPage_list.PageUrl %>start=<%= zPage_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="<%= Language.Phrase("PagerFirst") %>" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="<%= Language.Phrase("PagerFirst") %>" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If zPage_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= zPage_list.PageUrl %>start=<%= zPage_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="<%= Language.Phrase("PagerPrevious") %>" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="<%= Language.Phrase("PagerPrevious") %>" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= zPage_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If zPage_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= zPage_list.PageUrl %>start=<%= zPage_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="<%= Language.Phrase("PagerNext") %>" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="<%= Language.Phrase("PagerNext") %>" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If zPage_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= zPage_list.PageUrl %>start=<%= zPage_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="<%= Language.Phrase("PagerLast") %>" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="<%= Language.Phrase("PagerLast") %>" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;<%= Language.Phrase("Of") %>&nbsp;<%= zPage_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker"><%= Language.Phrase("Record") %>&nbsp;<%= zPage_list.Pager.FromIndex %>&nbsp;<%= Language.Phrase("To") %>&nbsp;<%= zPage_list.Pager.ToIndex %>&nbsp;<%= Language.Phrase("Of") %>&nbsp;<%= zPage_list.Pager.RecordCount %></span>
<% Else %>
	<% If zPage_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker"><%= Language.Phrase("EnterSearchCriteria") %></span>
	<% Else %>
	<span class="aspnetmaker"><%= Language.Phrase("NoRecord") %></span>
	<% End If %>
<% End If %>
		</td>
<% If zPage_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td><%= Language.Phrase("RecordsPerPage") %>&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="zPage" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If zPage_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="20"<% If zPage_list.lDisplayRecs = 20 Then %> selected="selected"<% End If %>>20</option>
<option value="40"<% If zPage_list.lDisplayRecs = 40 Then %> selected="selected"<% End If %>>40</option>
<option value="60"<% If zPage_list.lDisplayRecs = 60 Then %> selected="selected"<% End If %>>60</option>
<option value="ALL"<% If zPage.RecordsPerPage = -1 Then %> selected="selected"<% End If %>><%= Language.Phrase("AllRecords") %></option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<div class="aspnetmaker">
<a href="<%= zPage_list.AddUrl %>"><%= Language.Phrase("AddLink") %></a>&nbsp;&nbsp;
</div>
</div>
<% End If %>
</td></tr></table>
<% If zPage.Export = "" AndAlso zPage.CurrentAction = "" Then %>
<% End If %>
<% If zPage.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
