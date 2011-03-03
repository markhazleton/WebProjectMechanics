<%@ Page ClassName="sitecategory_list" Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="sitecategory_list.aspx.vb" Inherits="sitecategory_list" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If SiteCategory.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var SiteCategory_list = new ew_Page("SiteCategory_list");
// page properties
SiteCategory_list.PageID = "list"; // page ID
SiteCategory_list.FormID = "fSiteCategorylist"; // form ID 
var EW_PAGE_ID = SiteCategory_list.PageID; // for backward compatibility
// extend page with Form_CustomValidate function
SiteCategory_list.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
SiteCategory_list.ValidateRequired = true; // uses JavaScript validation
<% Else %>
SiteCategory_list.ValidateRequired = false; // no JavaScript validation
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
<% If SiteCategory.Export = "" Then %>
<% End If %>
<%

' Load recordset
Rs = SiteCategory_list.LoadRecordset()
	SiteCategory_list.lStartRec = 1
	If SiteCategory_list.lDisplayRecs <= 0 Then ' Display all records
		SiteCategory_list.lDisplayRecs = SiteCategory_list.lTotalRecs
	End If
	If Not (SiteCategory.ExportAll AndAlso SiteCategory.Export <> "") Then
		SiteCategory_list.SetUpStartRec() ' Set up start record position
	End If
%>
<p><span class="aspnetmaker" style="white-space: nowrap;"><%= Language.Phrase("TblTypeTABLE") %><%= SiteCategory.TableCaption %>
<% If SiteCategory.Export = "" AndAlso SiteCategory.CurrentAction = "" Then %>
&nbsp;&nbsp;<a href="<%= SiteCategory_list.ExportExcelUrl %>"><img src='images/exportxls.gif' alt='<%= ew_HtmlEncode(Language.Phrase("ExportToExcel")) %>' title='<%= ew_HtmlEncode(Language.Phrase("ExportToExcel")) %>' width='16' height='16' border='0'></a>
<% End If %>
</span></p>
<% If SiteCategory.Export = "" AndAlso SiteCategory.CurrentAction = "" Then %>
<a href="javascript:ew_ToggleSearchPanel(SiteCategory_list);" style="text-decoration: none;"><img id="SiteCategory_list_SearchImage" src="images/collapse.gif" alt="" width="9" height="9" border="0"></a><span class="aspnetmaker">&nbsp;<%= Language.Phrase("Search") %></span><br>
<div id="SiteCategory_list_SearchPanel">
<form name="fSiteCategorylistsrch" id="fSiteCategorylistsrch" class="ewForm">
<input type="hidden" id="t" name="t" value="SiteCategory" />
<table class="ewBasicSearch">
	<tr>
		<td><span class="aspnetmaker">
			<a href="<%= SiteCategory_list.PageUrl %>cmd=reset"><%= Language.Phrase("ShowAll") %></a>&nbsp;
			<a href="sitecategory_srch.aspx"><%= Language.Phrase("AdvancedSearch") %></a>&nbsp;
		</span></td>
	</tr>
</table>
</form>
</div>
<% End If %>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(SiteCategory_list.DebugMsg) %>
<% SiteCategory_list.ShowMessage() %>
<br />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<form name="fSiteCategorylist" id="fSiteCategorylist" class="ewForm" method="post">
<div id="gmp_SiteCategory" class="ewGridMiddlePanel">
<% If SiteCategory_list.lTotalRecs > 0 Then %>
<table cellspacing="0" rowhighlightclass="ewTableHighlightRow" rowselectclass="ewTableSelectRow" roweditclass="ewTableEditRow" class="ewTable ewTableSeparate">
<%= SiteCategory.TableCustomInnerHTML %>
<thead><!-- Table header -->
	<tr class="ewTableHeader">
<%

		' Render list options
		SiteCategory_list.RenderListOptions()

		' Render list options (header, left)
		SiteCategory_list.ListOptions.Render("header", "left")
%>
<% If SiteCategory.SiteCategoryID.Visible Then ' SiteCategoryID %>
	<% If SiteCategory.SortUrl(SiteCategory.SiteCategoryID) = "" Then %>
		<td><%= SiteCategory.SiteCategoryID.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteCategory.SortUrl(SiteCategory.SiteCategoryID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= SiteCategory.SiteCategoryID.FldCaption %></td><td style="width: 10px;"><% If SiteCategory.SiteCategoryID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteCategory.SiteCategoryID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If SiteCategory.CategoryKeywords.Visible Then ' CategoryKeywords %>
	<% If SiteCategory.SortUrl(SiteCategory.CategoryKeywords) = "" Then %>
		<td><%= SiteCategory.CategoryKeywords.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteCategory.SortUrl(SiteCategory.CategoryKeywords) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= SiteCategory.CategoryKeywords.FldCaption %></td><td style="width: 10px;"><% If SiteCategory.CategoryKeywords.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteCategory.CategoryKeywords.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If SiteCategory.CategoryName.Visible Then ' CategoryName %>
	<% If SiteCategory.SortUrl(SiteCategory.CategoryName) = "" Then %>
		<td><%= SiteCategory.CategoryName.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteCategory.SortUrl(SiteCategory.CategoryName) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= SiteCategory.CategoryName.FldCaption %></td><td style="width: 10px;"><% If SiteCategory.CategoryName.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteCategory.CategoryName.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If SiteCategory.CategoryTitle.Visible Then ' CategoryTitle %>
	<% If SiteCategory.SortUrl(SiteCategory.CategoryTitle) = "" Then %>
		<td><%= SiteCategory.CategoryTitle.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteCategory.SortUrl(SiteCategory.CategoryTitle) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= SiteCategory.CategoryTitle.FldCaption %></td><td style="width: 10px;"><% If SiteCategory.CategoryTitle.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteCategory.CategoryTitle.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If SiteCategory.CategoryDescription.Visible Then ' CategoryDescription %>
	<% If SiteCategory.SortUrl(SiteCategory.CategoryDescription) = "" Then %>
		<td><%= SiteCategory.CategoryDescription.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteCategory.SortUrl(SiteCategory.CategoryDescription) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= SiteCategory.CategoryDescription.FldCaption %></td><td style="width: 10px;"><% If SiteCategory.CategoryDescription.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteCategory.CategoryDescription.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If SiteCategory.GroupOrder.Visible Then ' GroupOrder %>
	<% If SiteCategory.SortUrl(SiteCategory.GroupOrder) = "" Then %>
		<td><%= SiteCategory.GroupOrder.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteCategory.SortUrl(SiteCategory.GroupOrder) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= SiteCategory.GroupOrder.FldCaption %></td><td style="width: 10px;"><% If SiteCategory.GroupOrder.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteCategory.GroupOrder.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If SiteCategory.ParentCategoryID.Visible Then ' ParentCategoryID %>
	<% If SiteCategory.SortUrl(SiteCategory.ParentCategoryID) = "" Then %>
		<td><%= SiteCategory.ParentCategoryID.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteCategory.SortUrl(SiteCategory.ParentCategoryID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= SiteCategory.ParentCategoryID.FldCaption %></td><td style="width: 10px;"><% If SiteCategory.ParentCategoryID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteCategory.ParentCategoryID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If SiteCategory.CategoryFileName.Visible Then ' CategoryFileName %>
	<% If SiteCategory.SortUrl(SiteCategory.CategoryFileName) = "" Then %>
		<td><%= SiteCategory.CategoryFileName.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteCategory.SortUrl(SiteCategory.CategoryFileName) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= SiteCategory.CategoryFileName.FldCaption %></td><td style="width: 10px;"><% If SiteCategory.CategoryFileName.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteCategory.CategoryFileName.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If SiteCategory.SiteCategoryTypeID.Visible Then ' SiteCategoryTypeID %>
	<% If SiteCategory.SortUrl(SiteCategory.SiteCategoryTypeID) = "" Then %>
		<td><%= SiteCategory.SiteCategoryTypeID.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteCategory.SortUrl(SiteCategory.SiteCategoryTypeID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= SiteCategory.SiteCategoryTypeID.FldCaption %></td><td style="width: 10px;"><% If SiteCategory.SiteCategoryTypeID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteCategory.SiteCategoryTypeID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If SiteCategory.SiteCategoryGroupID.Visible Then ' SiteCategoryGroupID %>
	<% If SiteCategory.SortUrl(SiteCategory.SiteCategoryGroupID) = "" Then %>
		<td><%= SiteCategory.SiteCategoryGroupID.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteCategory.SortUrl(SiteCategory.SiteCategoryGroupID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= SiteCategory.SiteCategoryGroupID.FldCaption %></td><td style="width: 10px;"><% If SiteCategory.SiteCategoryGroupID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteCategory.SiteCategoryGroupID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<%

		' Render list options (header, right)
		SiteCategory_list.ListOptions.Render("header", "right")
%>
	</tr>
</thead>
<tbody><!-- Table body -->
<%
If (SiteCategory.ExportAll AndAlso SiteCategory.Export <> "") Then
	SiteCategory_list.lStopRec = SiteCategory_list.lTotalRecs
Else
	SiteCategory_list.lStopRec = SiteCategory_list.lStartRec + SiteCategory_list.lDisplayRecs - 1 ' Set the last record to display
End If
If SiteCategory.CurrentAction = "gridadd" AndAlso SiteCategory_list.lStopRec = -1 Then
	SiteCategory_list.lStopRec = EW_GRIDADD_ROWS
End If 

' Move to first record
For i As Integer = 1 to SiteCategory_list.lStartRec - 1
	If Rs.Read() Then	SiteCategory_list.lRecCnt = SiteCategory_list.lRecCnt + 1
Next		

' Initialize Aggregate
SiteCategory.RowType = EW_ROWTYPE_AGGREGATEINIT
SiteCategory_list.RenderRow()
SiteCategory_list.lRowCnt = 0

' Output data rows
Do While (SiteCategory.CurrentAction = "gridadd" OrElse Rs.Read()) AndAlso (SiteCategory_list.lRecCnt < SiteCategory_list.lStopRec)
	SiteCategory_list.lRecCnt = SiteCategory_list.lRecCnt + 1
	If SiteCategory_list.lRecCnt >= SiteCategory_list.lStartRec Then
		SiteCategory_list.lRowCnt = SiteCategory_list.lRowCnt + 1
	SiteCategory.CssClass = ""
	SiteCategory.CssStyle = ""
	SiteCategory.RowAttrs.Clear()
	ew_SetAttr(SiteCategory.RowAttrs, New String(){"onmouseover", "onmouseout", "onclick"}, New String(){"ew_MouseOver(event, this);", "ew_MouseOut(event, this);", "ew_Click(event, this);"})
	If SiteCategory.CurrentAction = "gridadd" Then
		SiteCategory_list.LoadDefaultValues() ' Load default values
	Else
		SiteCategory_list.LoadRowValues(Rs) ' Load row values
	End If
	SiteCategory.RowType = EW_ROWTYPE_VIEW ' Render view

	' Render row
	SiteCategory_list.RenderRow()

	' Render list options
	SiteCategory_list.RenderListOptions()
%>
	<tr<%= SiteCategory.RowAttributes %>>
<%

		' Render list options (body, left)
		SiteCategory_list.ListOptions.Render("body", "left")
%>
	<% If SiteCategory.SiteCategoryID.Visible Then ' SiteCategoryID %>
		<td<%= SiteCategory.SiteCategoryID.CellAttributes %>>
<div<%= SiteCategory.SiteCategoryID.ViewAttributes %>><%= SiteCategory.SiteCategoryID.ListViewValue %></div>
</td>
	<% End If %>
	<% If SiteCategory.CategoryKeywords.Visible Then ' CategoryKeywords %>
		<td<%= SiteCategory.CategoryKeywords.CellAttributes %>>
<div<%= SiteCategory.CategoryKeywords.ViewAttributes %>><%= SiteCategory.CategoryKeywords.ListViewValue %></div>
</td>
	<% End If %>
	<% If SiteCategory.CategoryName.Visible Then ' CategoryName %>
		<td<%= SiteCategory.CategoryName.CellAttributes %>>
<div<%= SiteCategory.CategoryName.ViewAttributes %>><%= SiteCategory.CategoryName.ListViewValue %></div>
</td>
	<% End If %>
	<% If SiteCategory.CategoryTitle.Visible Then ' CategoryTitle %>
		<td<%= SiteCategory.CategoryTitle.CellAttributes %>>
<div<%= SiteCategory.CategoryTitle.ViewAttributes %>><%= SiteCategory.CategoryTitle.ListViewValue %></div>
</td>
	<% End If %>
	<% If SiteCategory.CategoryDescription.Visible Then ' CategoryDescription %>
		<td<%= SiteCategory.CategoryDescription.CellAttributes %>>
<div<%= SiteCategory.CategoryDescription.ViewAttributes %>><%= SiteCategory.CategoryDescription.ListViewValue %></div>
</td>
	<% End If %>
	<% If SiteCategory.GroupOrder.Visible Then ' GroupOrder %>
		<td<%= SiteCategory.GroupOrder.CellAttributes %>>
<div<%= SiteCategory.GroupOrder.ViewAttributes %>><%= SiteCategory.GroupOrder.ListViewValue %></div>
</td>
	<% End If %>
	<% If SiteCategory.ParentCategoryID.Visible Then ' ParentCategoryID %>
		<td<%= SiteCategory.ParentCategoryID.CellAttributes %>>
<div<%= SiteCategory.ParentCategoryID.ViewAttributes %>><%= SiteCategory.ParentCategoryID.ListViewValue %></div>
</td>
	<% End If %>
	<% If SiteCategory.CategoryFileName.Visible Then ' CategoryFileName %>
		<td<%= SiteCategory.CategoryFileName.CellAttributes %>>
<div<%= SiteCategory.CategoryFileName.ViewAttributes %>><%= SiteCategory.CategoryFileName.ListViewValue %></div>
</td>
	<% End If %>
	<% If SiteCategory.SiteCategoryTypeID.Visible Then ' SiteCategoryTypeID %>
		<td<%= SiteCategory.SiteCategoryTypeID.CellAttributes %>>
<div<%= SiteCategory.SiteCategoryTypeID.ViewAttributes %>><%= SiteCategory.SiteCategoryTypeID.ListViewValue %></div>
</td>
	<% End If %>
	<% If SiteCategory.SiteCategoryGroupID.Visible Then ' SiteCategoryGroupID %>
		<td<%= SiteCategory.SiteCategoryGroupID.CellAttributes %>>
<div<%= SiteCategory.SiteCategoryGroupID.ViewAttributes %>><%= SiteCategory.SiteCategoryGroupID.ListViewValue %></div>
</td>
	<% End If %>
<%

		' Render list options (body, right)
		SiteCategory_list.ListOptions.Render("body", "right")
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
<% If SiteCategory.Export = "" Then %>
<div class="ewGridLowerPanel">
<% If SiteCategory.CurrentAction <> "gridadd" AndAlso SiteCategory.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If SiteCategory_list.Pager Is Nothing Then SiteCategory_list.Pager = New cPrevNextPager(SiteCategory_list.lStartRec, SiteCategory_list.lDisplayRecs, SiteCategory_list.lTotalRecs) %>
<% If SiteCategory_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker"><%= Language.Phrase("Page") %>&nbsp;</span></td>
<!--first page button-->
	<% If SiteCategory_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= SiteCategory_list.PageUrl %>start=<%= SiteCategory_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="<%= Language.Phrase("PagerFirst") %>" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="<%= Language.Phrase("PagerFirst") %>" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If SiteCategory_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= SiteCategory_list.PageUrl %>start=<%= SiteCategory_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="<%= Language.Phrase("PagerPrevious") %>" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="<%= Language.Phrase("PagerPrevious") %>" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= SiteCategory_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If SiteCategory_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= SiteCategory_list.PageUrl %>start=<%= SiteCategory_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="<%= Language.Phrase("PagerNext") %>" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="<%= Language.Phrase("PagerNext") %>" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If SiteCategory_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= SiteCategory_list.PageUrl %>start=<%= SiteCategory_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="<%= Language.Phrase("PagerLast") %>" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="<%= Language.Phrase("PagerLast") %>" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;<%= Language.Phrase("Of") %>&nbsp;<%= SiteCategory_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker"><%= Language.Phrase("Record") %>&nbsp;<%= SiteCategory_list.Pager.FromIndex %>&nbsp;<%= Language.Phrase("To") %>&nbsp;<%= SiteCategory_list.Pager.ToIndex %>&nbsp;<%= Language.Phrase("Of") %>&nbsp;<%= SiteCategory_list.Pager.RecordCount %></span>
<% Else %>
	<% If SiteCategory_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker"><%= Language.Phrase("EnterSearchCriteria") %></span>
	<% Else %>
	<span class="aspnetmaker"><%= Language.Phrase("NoRecord") %></span>
	<% End If %>
<% End If %>
		</td>
<% If SiteCategory_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td><%= Language.Phrase("RecordsPerPage") %>&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="SiteCategory" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If SiteCategory_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="20"<% If SiteCategory_list.lDisplayRecs = 20 Then %> selected="selected"<% End If %>>20</option>
<option value="40"<% If SiteCategory_list.lDisplayRecs = 40 Then %> selected="selected"<% End If %>>40</option>
<option value="60"<% If SiteCategory_list.lDisplayRecs = 60 Then %> selected="selected"<% End If %>>60</option>
<option value="ALL"<% If SiteCategory.RecordsPerPage = -1 Then %> selected="selected"<% End If %>><%= Language.Phrase("AllRecords") %></option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<div class="aspnetmaker">
<a href="<%= SiteCategory_list.AddUrl %>"><%= Language.Phrase("AddLink") %></a>&nbsp;&nbsp;
</div>
</div>
<% End If %>
</td></tr></table>
<% If SiteCategory.Export = "" AndAlso SiteCategory.CurrentAction = "" Then %>
<% End If %>
<% If SiteCategory.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
