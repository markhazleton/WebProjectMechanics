<%@ Page ClassName="pageimage_list" Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="pageimage_list.aspx.vb" Inherits="pageimage_list" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<%@ Register TagPrefix="wpmWebsite" TagName="MasterTable_zPage" Src="zpage_master.ascx" %>
<%@ Register TagPrefix="wpmWebsite" TagName="MasterTable_Image" Src="image_master.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If PageImage.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var PageImage_list = new ew_Page("PageImage_list");
// page properties
PageImage_list.PageID = "list"; // page ID
PageImage_list.FormID = "fPageImagelist"; // form ID 
var EW_PAGE_ID = PageImage_list.PageID; // for backward compatibility
// extend page with Form_CustomValidate function
PageImage_list.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
PageImage_list.ValidateRequired = true; // uses JavaScript validation
<% Else %>
PageImage_list.ValidateRequired = false; // no JavaScript validation
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
<% If PageImage.Export = "" Then %>
<%
gsMasterReturnUrl = "zpage_list.aspx"
If PageImage_list.sDbMasterFilter <> "" AndAlso PageImage.CurrentMasterTable = "zPage" Then
	If PageImage_list.bMasterRecordExists Then
		If PageImage.CurrentMasterTable = PageImage.TableVar Then gsMasterReturnUrl = gsMasterReturnUrl & "?" & EW_TABLE_SHOW_MASTER & "="
%>
<wpmWebsite:MasterTable_zPage id="MasterTable_zPage" runat="server" />
<%
	End If
End If
%>
<%
gsMasterReturnUrl = "image_list.aspx"
If PageImage_list.sDbMasterFilter <> "" AndAlso PageImage.CurrentMasterTable = "Image" Then
	If PageImage_list.bMasterRecordExists Then
		If PageImage.CurrentMasterTable = PageImage.TableVar Then gsMasterReturnUrl = gsMasterReturnUrl & "?" & EW_TABLE_SHOW_MASTER & "="
%>
<wpmWebsite:MasterTable_Image id="MasterTable_Image" runat="server" />
<%
	End If
End If
%>
<% End If %>
<%

' Load recordset
Rs = PageImage_list.LoadRecordset()
	PageImage_list.lStartRec = 1
	If PageImage_list.lDisplayRecs <= 0 Then ' Display all records
		PageImage_list.lDisplayRecs = PageImage_list.lTotalRecs
	End If
	If Not (PageImage.ExportAll AndAlso PageImage.Export <> "") Then
		PageImage_list.SetUpStartRec() ' Set up start record position
	End If
%>
<p><span class="aspnetmaker" style="white-space: nowrap;"><%= Language.Phrase("TblTypeTABLE") %><%= PageImage.TableCaption %>
<% If PageImage.Export = "" AndAlso PageImage.CurrentAction = "" Then %>
&nbsp;&nbsp;<a href="<%= PageImage_list.ExportExcelUrl %>"><img src='images/exportxls.gif' alt='<%= ew_HtmlEncode(Language.Phrase("ExportToExcel")) %>' title='<%= ew_HtmlEncode(Language.Phrase("ExportToExcel")) %>' width='16' height='16' border='0'></a>
<% End If %>
</span></p>
<% If PageImage.Export = "" AndAlso PageImage.CurrentAction = "" Then %>
<a href="javascript:ew_ToggleSearchPanel(PageImage_list);" style="text-decoration: none;"><img id="PageImage_list_SearchImage" src="images/collapse.gif" alt="" width="9" height="9" border="0"></a><span class="aspnetmaker">&nbsp;<%= Language.Phrase("Search") %></span><br>
<div id="PageImage_list_SearchPanel">
<form name="fPageImagelistsrch" id="fPageImagelistsrch" class="ewForm">
<input type="hidden" id="t" name="t" value="PageImage" />
<table class="ewBasicSearch">
	<tr>
		<td><span class="aspnetmaker">
			<a href="<%= PageImage_list.PageUrl %>cmd=reset"><%= Language.Phrase("ShowAll") %></a>&nbsp;
			<a href="pageimage_srch.aspx"><%= Language.Phrase("AdvancedSearch") %></a>&nbsp;
		</span></td>
	</tr>
</table>
</form>
</div>
<% End If %>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(PageImage_list.DebugMsg) %>
<% PageImage_list.ShowMessage() %>
<br />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<form name="fPageImagelist" id="fPageImagelist" class="ewForm" method="post">
<div id="gmp_PageImage" class="ewGridMiddlePanel">
<% If PageImage_list.lTotalRecs > 0 Then %>
<table cellspacing="0" rowhighlightclass="ewTableHighlightRow" rowselectclass="ewTableSelectRow" roweditclass="ewTableEditRow" class="ewTable ewTableSeparate">
<%= PageImage.TableCustomInnerHTML %>
<thead><!-- Table header -->
	<tr class="ewTableHeader">
<%

		' Render list options
		PageImage_list.RenderListOptions()

		' Render list options (header, left)
		PageImage_list.ListOptions.Render("header", "left")
%>
<% If PageImage.zPageID.Visible Then ' PageID %>
	<% If PageImage.SortUrl(PageImage.zPageID) = "" Then %>
		<td style="white-space: nowrap;"><%= PageImage.zPageID.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= PageImage.SortUrl(PageImage.zPageID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn" style="white-space: nowrap;"><thead><tr><td><%= PageImage.zPageID.FldCaption %></td><td style="width: 10px;"><% If PageImage.zPageID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf PageImage.zPageID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If PageImage.ImageID.Visible Then ' ImageID %>
	<% If PageImage.SortUrl(PageImage.ImageID) = "" Then %>
		<td style="white-space: nowrap;"><%= PageImage.ImageID.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= PageImage.SortUrl(PageImage.ImageID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn" style="white-space: nowrap;"><thead><tr><td><%= PageImage.ImageID.FldCaption %></td><td style="width: 10px;"><% If PageImage.ImageID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf PageImage.ImageID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If PageImage.PageImagePosition.Visible Then ' PageImagePosition %>
	<% If PageImage.SortUrl(PageImage.PageImagePosition) = "" Then %>
		<td style="white-space: nowrap;"><%= PageImage.PageImagePosition.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= PageImage.SortUrl(PageImage.PageImagePosition) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn" style="white-space: nowrap;"><thead><tr><td><%= PageImage.PageImagePosition.FldCaption %></td><td style="width: 10px;"><% If PageImage.PageImagePosition.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf PageImage.PageImagePosition.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<%

		' Render list options (header, right)
		PageImage_list.ListOptions.Render("header", "right")
%>
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

' Initialize Aggregate
PageImage.RowType = EW_ROWTYPE_AGGREGATEINIT
PageImage_list.RenderRow()
PageImage_list.lRowCnt = 0

' Output data rows
Do While (PageImage.CurrentAction = "gridadd" OrElse Rs.Read()) AndAlso (PageImage_list.lRecCnt < PageImage_list.lStopRec)
	PageImage_list.lRecCnt = PageImage_list.lRecCnt + 1
	If PageImage_list.lRecCnt >= PageImage_list.lStartRec Then
		PageImage_list.lRowCnt = PageImage_list.lRowCnt + 1
	PageImage.CssClass = ""
	PageImage.CssStyle = ""
	PageImage.RowAttrs.Clear()
	ew_SetAttr(PageImage.RowAttrs, New String(){"onmouseover", "onmouseout", "onclick"}, New String(){"ew_MouseOver(event, this);", "ew_MouseOut(event, this);", "ew_Click(event, this);"})
	If PageImage.CurrentAction = "gridadd" Then
		PageImage_list.LoadDefaultValues() ' Load default values
	Else
		PageImage_list.LoadRowValues(Rs) ' Load row values
	End If
	PageImage.RowType = EW_ROWTYPE_VIEW ' Render view

	' Render row
	PageImage_list.RenderRow()

	' Render list options
	PageImage_list.RenderListOptions()
%>
	<tr<%= PageImage.RowAttributes %>>
<%

		' Render list options (body, left)
		PageImage_list.ListOptions.Render("body", "left")
%>
	<% If PageImage.zPageID.Visible Then ' PageID %>
		<td<%= PageImage.zPageID.CellAttributes %>>
<div<%= PageImage.zPageID.ViewAttributes %>><%= PageImage.zPageID.ListViewValue %></div>
</td>
	<% End If %>
	<% If PageImage.ImageID.Visible Then ' ImageID %>
		<td<%= PageImage.ImageID.CellAttributes %>>
<div<%= PageImage.ImageID.ViewAttributes %>><%= PageImage.ImageID.ListViewValue %></div>
</td>
	<% End If %>
	<% If PageImage.PageImagePosition.Visible Then ' PageImagePosition %>
		<td<%= PageImage.PageImagePosition.CellAttributes %>>
<div<%= PageImage.PageImagePosition.ViewAttributes %>><%= PageImage.PageImagePosition.ListViewValue %></div>
</td>
	<% End If %>
<%

		' Render list options (body, right)
		PageImage_list.ListOptions.Render("body", "right")
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
<% If PageImage.Export = "" Then %>
<div class="ewGridLowerPanel">
<% If PageImage.CurrentAction <> "gridadd" AndAlso PageImage.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If PageImage_list.Pager Is Nothing Then PageImage_list.Pager = New cPrevNextPager(PageImage_list.lStartRec, PageImage_list.lDisplayRecs, PageImage_list.lTotalRecs) %>
<% If PageImage_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker"><%= Language.Phrase("Page") %>&nbsp;</span></td>
<!--first page button-->
	<% If PageImage_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= PageImage_list.PageUrl %>start=<%= PageImage_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="<%= Language.Phrase("PagerFirst") %>" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="<%= Language.Phrase("PagerFirst") %>" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If PageImage_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= PageImage_list.PageUrl %>start=<%= PageImage_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="<%= Language.Phrase("PagerPrevious") %>" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="<%= Language.Phrase("PagerPrevious") %>" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= PageImage_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If PageImage_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= PageImage_list.PageUrl %>start=<%= PageImage_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="<%= Language.Phrase("PagerNext") %>" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="<%= Language.Phrase("PagerNext") %>" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If PageImage_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= PageImage_list.PageUrl %>start=<%= PageImage_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="<%= Language.Phrase("PagerLast") %>" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="<%= Language.Phrase("PagerLast") %>" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;<%= Language.Phrase("Of") %>&nbsp;<%= PageImage_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker"><%= Language.Phrase("Record") %>&nbsp;<%= PageImage_list.Pager.FromIndex %>&nbsp;<%= Language.Phrase("To") %>&nbsp;<%= PageImage_list.Pager.ToIndex %>&nbsp;<%= Language.Phrase("Of") %>&nbsp;<%= PageImage_list.Pager.RecordCount %></span>
<% Else %>
	<% If PageImage_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker"><%= Language.Phrase("EnterSearchCriteria") %></span>
	<% Else %>
	<span class="aspnetmaker"><%= Language.Phrase("NoRecord") %></span>
	<% End If %>
<% End If %>
		</td>
<% If PageImage_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td><%= Language.Phrase("RecordsPerPage") %>&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="PageImage" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If PageImage_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="20"<% If PageImage_list.lDisplayRecs = 20 Then %> selected="selected"<% End If %>>20</option>
<option value="40"<% If PageImage_list.lDisplayRecs = 40 Then %> selected="selected"<% End If %>>40</option>
<option value="60"<% If PageImage_list.lDisplayRecs = 60 Then %> selected="selected"<% End If %>>60</option>
<option value="ALL"<% If PageImage.RecordsPerPage = -1 Then %> selected="selected"<% End If %>><%= Language.Phrase("AllRecords") %></option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<div class="aspnetmaker">
<a href="<%= PageImage_list.AddUrl %>"><%= Language.Phrase("AddLink") %></a>&nbsp;&nbsp;
</div>
</div>
<% End If %>
</td></tr></table>
<% If PageImage.Export = "" AndAlso PageImage.CurrentAction = "" Then %>
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
