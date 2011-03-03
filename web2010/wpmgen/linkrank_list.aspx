<%@ Page ClassName="linkrank_list" Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="linkrank_list.aspx.vb" Inherits="linkrank_list" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If LinkRank.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var LinkRank_list = new ew_Page("LinkRank_list");
// page properties
LinkRank_list.PageID = "list"; // page ID
LinkRank_list.FormID = "fLinkRanklist"; // form ID 
var EW_PAGE_ID = LinkRank_list.PageID; // for backward compatibility
// extend page with Form_CustomValidate function
LinkRank_list.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
LinkRank_list.ValidateRequired = true; // uses JavaScript validation
<% Else %>
LinkRank_list.ValidateRequired = false; // no JavaScript validation
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
<% If LinkRank.Export = "" Then %>
<% End If %>
<%

' Load recordset
Rs = LinkRank_list.LoadRecordset()
	LinkRank_list.lStartRec = 1
	If LinkRank_list.lDisplayRecs <= 0 Then ' Display all records
		LinkRank_list.lDisplayRecs = LinkRank_list.lTotalRecs
	End If
	If Not (LinkRank.ExportAll AndAlso LinkRank.Export <> "") Then
		LinkRank_list.SetUpStartRec() ' Set up start record position
	End If
%>
<p><span class="aspnetmaker" style="white-space: nowrap;"><%= Language.Phrase("TblTypeTABLE") %><%= LinkRank.TableCaption %>
<% If LinkRank.Export = "" AndAlso LinkRank.CurrentAction = "" Then %>
&nbsp;&nbsp;<a href="<%= LinkRank_list.ExportExcelUrl %>"><img src='images/exportxls.gif' alt='<%= ew_HtmlEncode(Language.Phrase("ExportToExcel")) %>' title='<%= ew_HtmlEncode(Language.Phrase("ExportToExcel")) %>' width='16' height='16' border='0'></a>
<% End If %>
</span></p>
<% If LinkRank.Export = "" AndAlso LinkRank.CurrentAction = "" Then %>
<a href="javascript:ew_ToggleSearchPanel(LinkRank_list);" style="text-decoration: none;"><img id="LinkRank_list_SearchImage" src="images/collapse.gif" alt="" width="9" height="9" border="0"></a><span class="aspnetmaker">&nbsp;<%= Language.Phrase("Search") %></span><br>
<div id="LinkRank_list_SearchPanel">
<form name="fLinkRanklistsrch" id="fLinkRanklistsrch" class="ewForm">
<input type="hidden" id="t" name="t" value="LinkRank" />
<table class="ewBasicSearch">
	<tr>
		<td><span class="aspnetmaker">
			<a href="<%= LinkRank_list.PageUrl %>cmd=reset"><%= Language.Phrase("ShowAll") %></a>&nbsp;
			<a href="linkrank_srch.aspx"><%= Language.Phrase("AdvancedSearch") %></a>&nbsp;
		</span></td>
	</tr>
</table>
</form>
</div>
<% End If %>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(LinkRank_list.DebugMsg) %>
<% LinkRank_list.ShowMessage() %>
<br />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<form name="fLinkRanklist" id="fLinkRanklist" class="ewForm" method="post">
<div id="gmp_LinkRank" class="ewGridMiddlePanel">
<% If LinkRank_list.lTotalRecs > 0 Then %>
<table cellspacing="0" rowhighlightclass="ewTableHighlightRow" rowselectclass="ewTableSelectRow" roweditclass="ewTableEditRow" class="ewTable ewTableSeparate">
<%= LinkRank.TableCustomInnerHTML %>
<thead><!-- Table header -->
	<tr class="ewTableHeader">
<%

		' Render list options
		LinkRank_list.RenderListOptions()

		' Render list options (header, left)
		LinkRank_list.ListOptions.Render("header", "left")
%>
<% If LinkRank.ID.Visible Then ' ID %>
	<% If LinkRank.SortUrl(LinkRank.ID) = "" Then %>
		<td><%= LinkRank.ID.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= LinkRank.SortUrl(LinkRank.ID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= LinkRank.ID.FldCaption %></td><td style="width: 10px;"><% If LinkRank.ID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf LinkRank.ID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If LinkRank.LinkID.Visible Then ' LinkID %>
	<% If LinkRank.SortUrl(LinkRank.LinkID) = "" Then %>
		<td><%= LinkRank.LinkID.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= LinkRank.SortUrl(LinkRank.LinkID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= LinkRank.LinkID.FldCaption %></td><td style="width: 10px;"><% If LinkRank.LinkID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf LinkRank.LinkID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If LinkRank.UserID.Visible Then ' UserID %>
	<% If LinkRank.SortUrl(LinkRank.UserID) = "" Then %>
		<td><%= LinkRank.UserID.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= LinkRank.SortUrl(LinkRank.UserID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= LinkRank.UserID.FldCaption %></td><td style="width: 10px;"><% If LinkRank.UserID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf LinkRank.UserID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If LinkRank.RankNum.Visible Then ' RankNum %>
	<% If LinkRank.SortUrl(LinkRank.RankNum) = "" Then %>
		<td><%= LinkRank.RankNum.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= LinkRank.SortUrl(LinkRank.RankNum) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= LinkRank.RankNum.FldCaption %></td><td style="width: 10px;"><% If LinkRank.RankNum.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf LinkRank.RankNum.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If LinkRank.CateID.Visible Then ' CateID %>
	<% If LinkRank.SortUrl(LinkRank.CateID) = "" Then %>
		<td><%= LinkRank.CateID.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= LinkRank.SortUrl(LinkRank.CateID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= LinkRank.CateID.FldCaption %></td><td style="width: 10px;"><% If LinkRank.CateID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf LinkRank.CateID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If LinkRank.Comment.Visible Then ' Comment %>
	<% If LinkRank.SortUrl(LinkRank.Comment) = "" Then %>
		<td><%= LinkRank.Comment.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= LinkRank.SortUrl(LinkRank.Comment) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= LinkRank.Comment.FldCaption %></td><td style="width: 10px;"><% If LinkRank.Comment.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf LinkRank.Comment.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<%

		' Render list options (header, right)
		LinkRank_list.ListOptions.Render("header", "right")
%>
	</tr>
</thead>
<tbody><!-- Table body -->
<%
If (LinkRank.ExportAll AndAlso LinkRank.Export <> "") Then
	LinkRank_list.lStopRec = LinkRank_list.lTotalRecs
Else
	LinkRank_list.lStopRec = LinkRank_list.lStartRec + LinkRank_list.lDisplayRecs - 1 ' Set the last record to display
End If
If LinkRank.CurrentAction = "gridadd" AndAlso LinkRank_list.lStopRec = -1 Then
	LinkRank_list.lStopRec = EW_GRIDADD_ROWS
End If 

' Move to first record
For i As Integer = 1 to LinkRank_list.lStartRec - 1
	If Rs.Read() Then	LinkRank_list.lRecCnt = LinkRank_list.lRecCnt + 1
Next		

' Initialize Aggregate
LinkRank.RowType = EW_ROWTYPE_AGGREGATEINIT
LinkRank_list.RenderRow()
LinkRank_list.lRowCnt = 0

' Output data rows
Do While (LinkRank.CurrentAction = "gridadd" OrElse Rs.Read()) AndAlso (LinkRank_list.lRecCnt < LinkRank_list.lStopRec)
	LinkRank_list.lRecCnt = LinkRank_list.lRecCnt + 1
	If LinkRank_list.lRecCnt >= LinkRank_list.lStartRec Then
		LinkRank_list.lRowCnt = LinkRank_list.lRowCnt + 1
	LinkRank.CssClass = ""
	LinkRank.CssStyle = ""
	LinkRank.RowAttrs.Clear()
	ew_SetAttr(LinkRank.RowAttrs, New String(){"onmouseover", "onmouseout", "onclick"}, New String(){"ew_MouseOver(event, this);", "ew_MouseOut(event, this);", "ew_Click(event, this);"})
	If LinkRank.CurrentAction = "gridadd" Then
		LinkRank_list.LoadDefaultValues() ' Load default values
	Else
		LinkRank_list.LoadRowValues(Rs) ' Load row values
	End If
	LinkRank.RowType = EW_ROWTYPE_VIEW ' Render view

	' Render row
	LinkRank_list.RenderRow()

	' Render list options
	LinkRank_list.RenderListOptions()
%>
	<tr<%= LinkRank.RowAttributes %>>
<%

		' Render list options (body, left)
		LinkRank_list.ListOptions.Render("body", "left")
%>
	<% If LinkRank.ID.Visible Then ' ID %>
		<td<%= LinkRank.ID.CellAttributes %>>
<div<%= LinkRank.ID.ViewAttributes %>><%= LinkRank.ID.ListViewValue %></div>
</td>
	<% End If %>
	<% If LinkRank.LinkID.Visible Then ' LinkID %>
		<td<%= LinkRank.LinkID.CellAttributes %>>
<div<%= LinkRank.LinkID.ViewAttributes %>><%= LinkRank.LinkID.ListViewValue %></div>
</td>
	<% End If %>
	<% If LinkRank.UserID.Visible Then ' UserID %>
		<td<%= LinkRank.UserID.CellAttributes %>>
<div<%= LinkRank.UserID.ViewAttributes %>><%= LinkRank.UserID.ListViewValue %></div>
</td>
	<% End If %>
	<% If LinkRank.RankNum.Visible Then ' RankNum %>
		<td<%= LinkRank.RankNum.CellAttributes %>>
<div<%= LinkRank.RankNum.ViewAttributes %>><%= LinkRank.RankNum.ListViewValue %></div>
</td>
	<% End If %>
	<% If LinkRank.CateID.Visible Then ' CateID %>
		<td<%= LinkRank.CateID.CellAttributes %>>
<div<%= LinkRank.CateID.ViewAttributes %>><%= LinkRank.CateID.ListViewValue %></div>
</td>
	<% End If %>
	<% If LinkRank.Comment.Visible Then ' Comment %>
		<td<%= LinkRank.Comment.CellAttributes %>>
<div<%= LinkRank.Comment.ViewAttributes %>><%= LinkRank.Comment.ListViewValue %></div>
</td>
	<% End If %>
<%

		' Render list options (body, right)
		LinkRank_list.ListOptions.Render("body", "right")
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
<% If LinkRank.Export = "" Then %>
<div class="ewGridLowerPanel">
<% If LinkRank.CurrentAction <> "gridadd" AndAlso LinkRank.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If LinkRank_list.Pager Is Nothing Then LinkRank_list.Pager = New cPrevNextPager(LinkRank_list.lStartRec, LinkRank_list.lDisplayRecs, LinkRank_list.lTotalRecs) %>
<% If LinkRank_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker"><%= Language.Phrase("Page") %>&nbsp;</span></td>
<!--first page button-->
	<% If LinkRank_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= LinkRank_list.PageUrl %>start=<%= LinkRank_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="<%= Language.Phrase("PagerFirst") %>" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="<%= Language.Phrase("PagerFirst") %>" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If LinkRank_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= LinkRank_list.PageUrl %>start=<%= LinkRank_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="<%= Language.Phrase("PagerPrevious") %>" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="<%= Language.Phrase("PagerPrevious") %>" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= LinkRank_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If LinkRank_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= LinkRank_list.PageUrl %>start=<%= LinkRank_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="<%= Language.Phrase("PagerNext") %>" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="<%= Language.Phrase("PagerNext") %>" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If LinkRank_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= LinkRank_list.PageUrl %>start=<%= LinkRank_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="<%= Language.Phrase("PagerLast") %>" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="<%= Language.Phrase("PagerLast") %>" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;<%= Language.Phrase("Of") %>&nbsp;<%= LinkRank_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker"><%= Language.Phrase("Record") %>&nbsp;<%= LinkRank_list.Pager.FromIndex %>&nbsp;<%= Language.Phrase("To") %>&nbsp;<%= LinkRank_list.Pager.ToIndex %>&nbsp;<%= Language.Phrase("Of") %>&nbsp;<%= LinkRank_list.Pager.RecordCount %></span>
<% Else %>
	<% If LinkRank_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker"><%= Language.Phrase("EnterSearchCriteria") %></span>
	<% Else %>
	<span class="aspnetmaker"><%= Language.Phrase("NoRecord") %></span>
	<% End If %>
<% End If %>
		</td>
<% If LinkRank_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td><%= Language.Phrase("RecordsPerPage") %>&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="LinkRank" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If LinkRank_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="20"<% If LinkRank_list.lDisplayRecs = 20 Then %> selected="selected"<% End If %>>20</option>
<option value="40"<% If LinkRank_list.lDisplayRecs = 40 Then %> selected="selected"<% End If %>>40</option>
<option value="60"<% If LinkRank_list.lDisplayRecs = 60 Then %> selected="selected"<% End If %>>60</option>
<option value="ALL"<% If LinkRank.RecordsPerPage = -1 Then %> selected="selected"<% End If %>><%= Language.Phrase("AllRecords") %></option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<div class="aspnetmaker">
<a href="<%= LinkRank_list.AddUrl %>"><%= Language.Phrase("AddLink") %></a>&nbsp;&nbsp;
</div>
</div>
<% End If %>
</td></tr></table>
<% If LinkRank.Export = "" AndAlso LinkRank.CurrentAction = "" Then %>
<% End If %>
<% If LinkRank.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
