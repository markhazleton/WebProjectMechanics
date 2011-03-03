<%@ Page ClassName="pagetype_list" Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="pagetype_list.aspx.vb" Inherits="pagetype_list" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If PageType.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var PageType_list = new ew_Page("PageType_list");
// page properties
PageType_list.PageID = "list"; // page ID
PageType_list.FormID = "fPageTypelist"; // form ID 
var EW_PAGE_ID = PageType_list.PageID; // for backward compatibility
// extend page with Form_CustomValidate function
PageType_list.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
PageType_list.ValidateRequired = true; // uses JavaScript validation
<% Else %>
PageType_list.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker" style="white-space: nowrap;"><%= Language.Phrase("TblTypeTABLE") %><%= PageType.TableCaption %>
<% If PageType.Export = "" AndAlso PageType.CurrentAction = "" Then %>
&nbsp;&nbsp;<a href="<%= PageType_list.ExportExcelUrl %>"><img src='images/exportxls.gif' alt='<%= ew_HtmlEncode(Language.Phrase("ExportToExcel")) %>' title='<%= ew_HtmlEncode(Language.Phrase("ExportToExcel")) %>' width='16' height='16' border='0'></a>
<% End If %>
</span></p>
<% If PageType.Export = "" AndAlso PageType.CurrentAction = "" Then %>
<a href="javascript:ew_ToggleSearchPanel(PageType_list);" style="text-decoration: none;"><img id="PageType_list_SearchImage" src="images/collapse.gif" alt="" width="9" height="9" border="0"></a><span class="aspnetmaker">&nbsp;<%= Language.Phrase("Search") %></span><br>
<div id="PageType_list_SearchPanel">
<form name="fPageTypelistsrch" id="fPageTypelistsrch" class="ewForm">
<input type="hidden" id="t" name="t" value="PageType" />
<table class="ewBasicSearch">
	<tr>
		<td><span class="aspnetmaker">
			<a href="<%= PageType_list.PageUrl %>cmd=reset"><%= Language.Phrase("ShowAll") %></a>&nbsp;
			<a href="pagetype_srch.aspx"><%= Language.Phrase("AdvancedSearch") %></a>&nbsp;
		</span></td>
	</tr>
</table>
</form>
</div>
<% End If %>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(PageType_list.DebugMsg) %>
<% PageType_list.ShowMessage() %>
<br />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<form name="fPageTypelist" id="fPageTypelist" class="ewForm" method="post">
<div id="gmp_PageType" class="ewGridMiddlePanel">
<% If PageType_list.lTotalRecs > 0 Then %>
<table cellspacing="0" rowhighlightclass="ewTableHighlightRow" rowselectclass="ewTableSelectRow" roweditclass="ewTableEditRow" class="ewTable ewTableSeparate">
<%= PageType.TableCustomInnerHTML %>
<thead><!-- Table header -->
	<tr class="ewTableHeader">
<%

		' Render list options
		PageType_list.RenderListOptions()

		' Render list options (header, left)
		PageType_list.ListOptions.Render("header", "left")
%>
<% If PageType.PageTypeID.Visible Then ' PageTypeID %>
	<% If PageType.SortUrl(PageType.PageTypeID) = "" Then %>
		<td><%= PageType.PageTypeID.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= PageType.SortUrl(PageType.PageTypeID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= PageType.PageTypeID.FldCaption %></td><td style="width: 10px;"><% If PageType.PageTypeID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf PageType.PageTypeID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If PageType.PageTypeCD.Visible Then ' PageTypeCD %>
	<% If PageType.SortUrl(PageType.PageTypeCD) = "" Then %>
		<td><%= PageType.PageTypeCD.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= PageType.SortUrl(PageType.PageTypeCD) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= PageType.PageTypeCD.FldCaption %></td><td style="width: 10px;"><% If PageType.PageTypeCD.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf PageType.PageTypeCD.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If PageType.PageTypeDesc.Visible Then ' PageTypeDesc %>
	<% If PageType.SortUrl(PageType.PageTypeDesc) = "" Then %>
		<td><%= PageType.PageTypeDesc.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= PageType.SortUrl(PageType.PageTypeDesc) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= PageType.PageTypeDesc.FldCaption %></td><td style="width: 10px;"><% If PageType.PageTypeDesc.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf PageType.PageTypeDesc.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If PageType.PageFileName.Visible Then ' PageFileName %>
	<% If PageType.SortUrl(PageType.PageFileName) = "" Then %>
		<td><%= PageType.PageFileName.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= PageType.SortUrl(PageType.PageFileName) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= PageType.PageFileName.FldCaption %></td><td style="width: 10px;"><% If PageType.PageFileName.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf PageType.PageFileName.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<%

		' Render list options (header, right)
		PageType_list.ListOptions.Render("header", "right")
%>
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

' Initialize Aggregate
PageType.RowType = EW_ROWTYPE_AGGREGATEINIT
PageType_list.RenderRow()
PageType_list.lRowCnt = 0

' Output data rows
Do While (PageType.CurrentAction = "gridadd" OrElse Rs.Read()) AndAlso (PageType_list.lRecCnt < PageType_list.lStopRec)
	PageType_list.lRecCnt = PageType_list.lRecCnt + 1
	If PageType_list.lRecCnt >= PageType_list.lStartRec Then
		PageType_list.lRowCnt = PageType_list.lRowCnt + 1
	PageType.CssClass = ""
	PageType.CssStyle = ""
	PageType.RowAttrs.Clear()
	ew_SetAttr(PageType.RowAttrs, New String(){"onmouseover", "onmouseout", "onclick"}, New String(){"ew_MouseOver(event, this);", "ew_MouseOut(event, this);", "ew_Click(event, this);"})
	If PageType.CurrentAction = "gridadd" Then
		PageType_list.LoadDefaultValues() ' Load default values
	Else
		PageType_list.LoadRowValues(Rs) ' Load row values
	End If
	PageType.RowType = EW_ROWTYPE_VIEW ' Render view

	' Render row
	PageType_list.RenderRow()

	' Render list options
	PageType_list.RenderListOptions()
%>
	<tr<%= PageType.RowAttributes %>>
<%

		' Render list options (body, left)
		PageType_list.ListOptions.Render("body", "left")
%>
	<% If PageType.PageTypeID.Visible Then ' PageTypeID %>
		<td<%= PageType.PageTypeID.CellAttributes %>>
<div<%= PageType.PageTypeID.ViewAttributes %>><%= PageType.PageTypeID.ListViewValue %></div>
</td>
	<% End If %>
	<% If PageType.PageTypeCD.Visible Then ' PageTypeCD %>
		<td<%= PageType.PageTypeCD.CellAttributes %>>
<div<%= PageType.PageTypeCD.ViewAttributes %>><%= PageType.PageTypeCD.ListViewValue %></div>
</td>
	<% End If %>
	<% If PageType.PageTypeDesc.Visible Then ' PageTypeDesc %>
		<td<%= PageType.PageTypeDesc.CellAttributes %>>
<div<%= PageType.PageTypeDesc.ViewAttributes %>><%= PageType.PageTypeDesc.ListViewValue %></div>
</td>
	<% End If %>
	<% If PageType.PageFileName.Visible Then ' PageFileName %>
		<td<%= PageType.PageFileName.CellAttributes %>>
<div<%= PageType.PageFileName.ViewAttributes %>><%= PageType.PageFileName.ListViewValue %></div>
</td>
	<% End If %>
<%

		' Render list options (body, right)
		PageType_list.ListOptions.Render("body", "right")
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
<% If PageType.Export = "" Then %>
<div class="ewGridLowerPanel">
<% If PageType.CurrentAction <> "gridadd" AndAlso PageType.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If PageType_list.Pager Is Nothing Then PageType_list.Pager = New cPrevNextPager(PageType_list.lStartRec, PageType_list.lDisplayRecs, PageType_list.lTotalRecs) %>
<% If PageType_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker"><%= Language.Phrase("Page") %>&nbsp;</span></td>
<!--first page button-->
	<% If PageType_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= PageType_list.PageUrl %>start=<%= PageType_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="<%= Language.Phrase("PagerFirst") %>" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="<%= Language.Phrase("PagerFirst") %>" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If PageType_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= PageType_list.PageUrl %>start=<%= PageType_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="<%= Language.Phrase("PagerPrevious") %>" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="<%= Language.Phrase("PagerPrevious") %>" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= PageType_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If PageType_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= PageType_list.PageUrl %>start=<%= PageType_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="<%= Language.Phrase("PagerNext") %>" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="<%= Language.Phrase("PagerNext") %>" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If PageType_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= PageType_list.PageUrl %>start=<%= PageType_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="<%= Language.Phrase("PagerLast") %>" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="<%= Language.Phrase("PagerLast") %>" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;<%= Language.Phrase("Of") %>&nbsp;<%= PageType_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker"><%= Language.Phrase("Record") %>&nbsp;<%= PageType_list.Pager.FromIndex %>&nbsp;<%= Language.Phrase("To") %>&nbsp;<%= PageType_list.Pager.ToIndex %>&nbsp;<%= Language.Phrase("Of") %>&nbsp;<%= PageType_list.Pager.RecordCount %></span>
<% Else %>
	<% If PageType_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker"><%= Language.Phrase("EnterSearchCriteria") %></span>
	<% Else %>
	<span class="aspnetmaker"><%= Language.Phrase("NoRecord") %></span>
	<% End If %>
<% End If %>
		</td>
<% If PageType_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td><%= Language.Phrase("RecordsPerPage") %>&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="PageType" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If PageType_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="20"<% If PageType_list.lDisplayRecs = 20 Then %> selected="selected"<% End If %>>20</option>
<option value="40"<% If PageType_list.lDisplayRecs = 40 Then %> selected="selected"<% End If %>>40</option>
<option value="60"<% If PageType_list.lDisplayRecs = 60 Then %> selected="selected"<% End If %>>60</option>
<option value="ALL"<% If PageType.RecordsPerPage = -1 Then %> selected="selected"<% End If %>><%= Language.Phrase("AllRecords") %></option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<div class="aspnetmaker">
<a href="<%= PageType_list.AddUrl %>"><%= Language.Phrase("AddLink") %></a>&nbsp;&nbsp;
</div>
</div>
<% End If %>
</td></tr></table>
<% If PageType.Export = "" AndAlso PageType.CurrentAction = "" Then %>
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
