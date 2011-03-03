<%@ Page ClassName="sitelink_list" Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="sitelink_list.aspx.vb" Inherits="sitelink_list" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If SiteLink.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var SiteLink_list = new ew_Page("SiteLink_list");
// page properties
SiteLink_list.PageID = "list"; // page ID
SiteLink_list.FormID = "fSiteLinklist"; // form ID 
var EW_PAGE_ID = SiteLink_list.PageID; // for backward compatibility
// extend page with Form_CustomValidate function
SiteLink_list.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
SiteLink_list.ValidateRequired = true; // uses JavaScript validation
<% Else %>
SiteLink_list.ValidateRequired = false; // no JavaScript validation
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
<% If SiteLink.Export = "" Then %>
<% End If %>
<%

' Load recordset
Rs = SiteLink_list.LoadRecordset()
	SiteLink_list.lStartRec = 1
	If SiteLink_list.lDisplayRecs <= 0 Then ' Display all records
		SiteLink_list.lDisplayRecs = SiteLink_list.lTotalRecs
	End If
	If Not (SiteLink.ExportAll AndAlso SiteLink.Export <> "") Then
		SiteLink_list.SetUpStartRec() ' Set up start record position
	End If
%>
<p><span class="aspnetmaker" style="white-space: nowrap;"><%= Language.Phrase("TblTypeTABLE") %><%= SiteLink.TableCaption %>
<% If SiteLink.Export = "" AndAlso SiteLink.CurrentAction = "" Then %>
&nbsp;&nbsp;<a href="<%= SiteLink_list.ExportExcelUrl %>"><img src='images/exportxls.gif' alt='<%= ew_HtmlEncode(Language.Phrase("ExportToExcel")) %>' title='<%= ew_HtmlEncode(Language.Phrase("ExportToExcel")) %>' width='16' height='16' border='0'></a>
<% End If %>
</span></p>
<% If SiteLink.Export = "" AndAlso SiteLink.CurrentAction = "" Then %>
<a href="javascript:ew_ToggleSearchPanel(SiteLink_list);" style="text-decoration: none;"><img id="SiteLink_list_SearchImage" src="images/collapse.gif" alt="" width="9" height="9" border="0"></a><span class="aspnetmaker">&nbsp;<%= Language.Phrase("Search") %></span><br>
<div id="SiteLink_list_SearchPanel">
<form name="fSiteLinklistsrch" id="fSiteLinklistsrch" class="ewForm">
<input type="hidden" id="t" name="t" value="SiteLink" />
<table class="ewBasicSearch">
	<tr>
		<td><span class="aspnetmaker">
			<a href="<%= SiteLink_list.PageUrl %>cmd=reset"><%= Language.Phrase("ShowAll") %></a>&nbsp;
			<a href="sitelink_srch.aspx"><%= Language.Phrase("AdvancedSearch") %></a>&nbsp;
		</span></td>
	</tr>
</table>
</form>
</div>
<% End If %>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(SiteLink_list.DebugMsg) %>
<% SiteLink_list.ShowMessage() %>
<br />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<form name="fSiteLinklist" id="fSiteLinklist" class="ewForm" method="post">
<div id="gmp_SiteLink" class="ewGridMiddlePanel">
<% If SiteLink_list.lTotalRecs > 0 Then %>
<table cellspacing="0" rowhighlightclass="ewTableHighlightRow" rowselectclass="ewTableSelectRow" roweditclass="ewTableEditRow" class="ewTable ewTableSeparate">
<%= SiteLink.TableCustomInnerHTML %>
<thead><!-- Table header -->
	<tr class="ewTableHeader">
<%

		' Render list options
		SiteLink_list.RenderListOptions()

		' Render list options (header, left)
		SiteLink_list.ListOptions.Render("header", "left")
%>
<% If SiteLink.ID.Visible Then ' ID %>
	<% If SiteLink.SortUrl(SiteLink.ID) = "" Then %>
		<td><%= SiteLink.ID.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteLink.SortUrl(SiteLink.ID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= SiteLink.ID.FldCaption %></td><td style="width: 10px;"><% If SiteLink.ID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteLink.ID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If SiteLink.CompanyID.Visible Then ' CompanyID %>
	<% If SiteLink.SortUrl(SiteLink.CompanyID) = "" Then %>
		<td><%= SiteLink.CompanyID.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteLink.SortUrl(SiteLink.CompanyID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= SiteLink.CompanyID.FldCaption %></td><td style="width: 10px;"><% If SiteLink.CompanyID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteLink.CompanyID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If SiteLink.LinkTypeCD.Visible Then ' LinkTypeCD %>
	<% If SiteLink.SortUrl(SiteLink.LinkTypeCD) = "" Then %>
		<td><%= SiteLink.LinkTypeCD.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteLink.SortUrl(SiteLink.LinkTypeCD) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= SiteLink.LinkTypeCD.FldCaption %></td><td style="width: 10px;"><% If SiteLink.LinkTypeCD.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteLink.LinkTypeCD.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If SiteLink.Title.Visible Then ' Title %>
	<% If SiteLink.SortUrl(SiteLink.Title) = "" Then %>
		<td><%= SiteLink.Title.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteLink.SortUrl(SiteLink.Title) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= SiteLink.Title.FldCaption %></td><td style="width: 10px;"><% If SiteLink.Title.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteLink.Title.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If SiteLink.DateAdd.Visible Then ' DateAdd %>
	<% If SiteLink.SortUrl(SiteLink.DateAdd) = "" Then %>
		<td><%= SiteLink.DateAdd.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteLink.SortUrl(SiteLink.DateAdd) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= SiteLink.DateAdd.FldCaption %></td><td style="width: 10px;"><% If SiteLink.DateAdd.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteLink.DateAdd.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If SiteLink.Ranks.Visible Then ' Ranks %>
	<% If SiteLink.SortUrl(SiteLink.Ranks) = "" Then %>
		<td><%= SiteLink.Ranks.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteLink.SortUrl(SiteLink.Ranks) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= SiteLink.Ranks.FldCaption %></td><td style="width: 10px;"><% If SiteLink.Ranks.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteLink.Ranks.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If SiteLink.Views.Visible Then ' Views %>
	<% If SiteLink.SortUrl(SiteLink.Views) = "" Then %>
		<td><%= SiteLink.Views.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteLink.SortUrl(SiteLink.Views) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= SiteLink.Views.FldCaption %></td><td style="width: 10px;"><% If SiteLink.Views.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteLink.Views.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If SiteLink.UserName.Visible Then ' UserName %>
	<% If SiteLink.SortUrl(SiteLink.UserName) = "" Then %>
		<td><%= SiteLink.UserName.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteLink.SortUrl(SiteLink.UserName) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= SiteLink.UserName.FldCaption %></td><td style="width: 10px;"><% If SiteLink.UserName.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteLink.UserName.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If SiteLink.UserID.Visible Then ' UserID %>
	<% If SiteLink.SortUrl(SiteLink.UserID) = "" Then %>
		<td><%= SiteLink.UserID.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteLink.SortUrl(SiteLink.UserID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= SiteLink.UserID.FldCaption %></td><td style="width: 10px;"><% If SiteLink.UserID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteLink.UserID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If SiteLink.ASIN.Visible Then ' ASIN %>
	<% If SiteLink.SortUrl(SiteLink.ASIN) = "" Then %>
		<td><%= SiteLink.ASIN.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteLink.SortUrl(SiteLink.ASIN) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= SiteLink.ASIN.FldCaption %></td><td style="width: 10px;"><% If SiteLink.ASIN.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteLink.ASIN.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If SiteLink.CategoryID.Visible Then ' CategoryID %>
	<% If SiteLink.SortUrl(SiteLink.CategoryID) = "" Then %>
		<td><%= SiteLink.CategoryID.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteLink.SortUrl(SiteLink.CategoryID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= SiteLink.CategoryID.FldCaption %></td><td style="width: 10px;"><% If SiteLink.CategoryID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteLink.CategoryID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If SiteLink.SiteCategoryID.Visible Then ' SiteCategoryID %>
	<% If SiteLink.SortUrl(SiteLink.SiteCategoryID) = "" Then %>
		<td><%= SiteLink.SiteCategoryID.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteLink.SortUrl(SiteLink.SiteCategoryID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= SiteLink.SiteCategoryID.FldCaption %></td><td style="width: 10px;"><% If SiteLink.SiteCategoryID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteLink.SiteCategoryID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If SiteLink.SiteCategoryTypeID.Visible Then ' SiteCategoryTypeID %>
	<% If SiteLink.SortUrl(SiteLink.SiteCategoryTypeID) = "" Then %>
		<td><%= SiteLink.SiteCategoryTypeID.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteLink.SortUrl(SiteLink.SiteCategoryTypeID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= SiteLink.SiteCategoryTypeID.FldCaption %></td><td style="width: 10px;"><% If SiteLink.SiteCategoryTypeID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteLink.SiteCategoryTypeID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If SiteLink.SiteCategoryGroupID.Visible Then ' SiteCategoryGroupID %>
	<% If SiteLink.SortUrl(SiteLink.SiteCategoryGroupID) = "" Then %>
		<td><%= SiteLink.SiteCategoryGroupID.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteLink.SortUrl(SiteLink.SiteCategoryGroupID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= SiteLink.SiteCategoryGroupID.FldCaption %></td><td style="width: 10px;"><% If SiteLink.SiteCategoryGroupID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteLink.SiteCategoryGroupID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<%

		' Render list options (header, right)
		SiteLink_list.ListOptions.Render("header", "right")
%>
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

' Initialize Aggregate
SiteLink.RowType = EW_ROWTYPE_AGGREGATEINIT
SiteLink_list.RenderRow()
SiteLink_list.lRowCnt = 0

' Output data rows
Do While (SiteLink.CurrentAction = "gridadd" OrElse Rs.Read()) AndAlso (SiteLink_list.lRecCnt < SiteLink_list.lStopRec)
	SiteLink_list.lRecCnt = SiteLink_list.lRecCnt + 1
	If SiteLink_list.lRecCnt >= SiteLink_list.lStartRec Then
		SiteLink_list.lRowCnt = SiteLink_list.lRowCnt + 1
	SiteLink.CssClass = ""
	SiteLink.CssStyle = ""
	SiteLink.RowAttrs.Clear()
	ew_SetAttr(SiteLink.RowAttrs, New String(){"onmouseover", "onmouseout", "onclick"}, New String(){"ew_MouseOver(event, this);", "ew_MouseOut(event, this);", "ew_Click(event, this);"})
	If SiteLink.CurrentAction = "gridadd" Then
		SiteLink_list.LoadDefaultValues() ' Load default values
	Else
		SiteLink_list.LoadRowValues(Rs) ' Load row values
	End If
	SiteLink.RowType = EW_ROWTYPE_VIEW ' Render view

	' Render row
	SiteLink_list.RenderRow()

	' Render list options
	SiteLink_list.RenderListOptions()
%>
	<tr<%= SiteLink.RowAttributes %>>
<%

		' Render list options (body, left)
		SiteLink_list.ListOptions.Render("body", "left")
%>
	<% If SiteLink.ID.Visible Then ' ID %>
		<td<%= SiteLink.ID.CellAttributes %>>
<div<%= SiteLink.ID.ViewAttributes %>><%= SiteLink.ID.ListViewValue %></div>
</td>
	<% End If %>
	<% If SiteLink.CompanyID.Visible Then ' CompanyID %>
		<td<%= SiteLink.CompanyID.CellAttributes %>>
<div<%= SiteLink.CompanyID.ViewAttributes %>><%= SiteLink.CompanyID.ListViewValue %></div>
</td>
	<% End If %>
	<% If SiteLink.LinkTypeCD.Visible Then ' LinkTypeCD %>
		<td<%= SiteLink.LinkTypeCD.CellAttributes %>>
<div<%= SiteLink.LinkTypeCD.ViewAttributes %>><%= SiteLink.LinkTypeCD.ListViewValue %></div>
</td>
	<% End If %>
	<% If SiteLink.Title.Visible Then ' Title %>
		<td<%= SiteLink.Title.CellAttributes %>>
<div<%= SiteLink.Title.ViewAttributes %>><%= SiteLink.Title.ListViewValue %></div>
</td>
	<% End If %>
	<% If SiteLink.DateAdd.Visible Then ' DateAdd %>
		<td<%= SiteLink.DateAdd.CellAttributes %>>
<div<%= SiteLink.DateAdd.ViewAttributes %>><%= SiteLink.DateAdd.ListViewValue %></div>
</td>
	<% End If %>
	<% If SiteLink.Ranks.Visible Then ' Ranks %>
		<td<%= SiteLink.Ranks.CellAttributes %>>
<div<%= SiteLink.Ranks.ViewAttributes %>><%= SiteLink.Ranks.ListViewValue %></div>
</td>
	<% End If %>
	<% If SiteLink.Views.Visible Then ' Views %>
		<td<%= SiteLink.Views.CellAttributes %>>
<% If Convert.ToString(SiteLink.Views.CurrentValue) = "1" Then %>
<input type="checkbox" value="<%= SiteLink.Views.ListViewValue %>" checked onclick="this.form.reset();" disabled="disabled" />
<% Else %>
<input type="checkbox" value="<%= SiteLink.Views.ListViewValue %>" onclick="this.form.reset();" disabled="disabled" />
<% End If %>
</td>
	<% End If %>
	<% If SiteLink.UserName.Visible Then ' UserName %>
		<td<%= SiteLink.UserName.CellAttributes %>>
<div<%= SiteLink.UserName.ViewAttributes %>><%= SiteLink.UserName.ListViewValue %></div>
</td>
	<% End If %>
	<% If SiteLink.UserID.Visible Then ' UserID %>
		<td<%= SiteLink.UserID.CellAttributes %>>
<div<%= SiteLink.UserID.ViewAttributes %>><%= SiteLink.UserID.ListViewValue %></div>
</td>
	<% End If %>
	<% If SiteLink.ASIN.Visible Then ' ASIN %>
		<td<%= SiteLink.ASIN.CellAttributes %>>
<div<%= SiteLink.ASIN.ViewAttributes %>><%= SiteLink.ASIN.ListViewValue %></div>
</td>
	<% End If %>
	<% If SiteLink.CategoryID.Visible Then ' CategoryID %>
		<td<%= SiteLink.CategoryID.CellAttributes %>>
<div<%= SiteLink.CategoryID.ViewAttributes %>><%= SiteLink.CategoryID.ListViewValue %></div>
</td>
	<% End If %>
	<% If SiteLink.SiteCategoryID.Visible Then ' SiteCategoryID %>
		<td<%= SiteLink.SiteCategoryID.CellAttributes %>>
<div<%= SiteLink.SiteCategoryID.ViewAttributes %>><%= SiteLink.SiteCategoryID.ListViewValue %></div>
</td>
	<% End If %>
	<% If SiteLink.SiteCategoryTypeID.Visible Then ' SiteCategoryTypeID %>
		<td<%= SiteLink.SiteCategoryTypeID.CellAttributes %>>
<div<%= SiteLink.SiteCategoryTypeID.ViewAttributes %>><%= SiteLink.SiteCategoryTypeID.ListViewValue %></div>
</td>
	<% End If %>
	<% If SiteLink.SiteCategoryGroupID.Visible Then ' SiteCategoryGroupID %>
		<td<%= SiteLink.SiteCategoryGroupID.CellAttributes %>>
<div<%= SiteLink.SiteCategoryGroupID.ViewAttributes %>><%= SiteLink.SiteCategoryGroupID.ListViewValue %></div>
</td>
	<% End If %>
<%

		' Render list options (body, right)
		SiteLink_list.ListOptions.Render("body", "right")
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
<% If SiteLink.Export = "" Then %>
<div class="ewGridLowerPanel">
<% If SiteLink.CurrentAction <> "gridadd" AndAlso SiteLink.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If SiteLink_list.Pager Is Nothing Then SiteLink_list.Pager = New cPrevNextPager(SiteLink_list.lStartRec, SiteLink_list.lDisplayRecs, SiteLink_list.lTotalRecs) %>
<% If SiteLink_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker"><%= Language.Phrase("Page") %>&nbsp;</span></td>
<!--first page button-->
	<% If SiteLink_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= SiteLink_list.PageUrl %>start=<%= SiteLink_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="<%= Language.Phrase("PagerFirst") %>" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="<%= Language.Phrase("PagerFirst") %>" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If SiteLink_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= SiteLink_list.PageUrl %>start=<%= SiteLink_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="<%= Language.Phrase("PagerPrevious") %>" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="<%= Language.Phrase("PagerPrevious") %>" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= SiteLink_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If SiteLink_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= SiteLink_list.PageUrl %>start=<%= SiteLink_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="<%= Language.Phrase("PagerNext") %>" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="<%= Language.Phrase("PagerNext") %>" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If SiteLink_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= SiteLink_list.PageUrl %>start=<%= SiteLink_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="<%= Language.Phrase("PagerLast") %>" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="<%= Language.Phrase("PagerLast") %>" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;<%= Language.Phrase("Of") %>&nbsp;<%= SiteLink_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker"><%= Language.Phrase("Record") %>&nbsp;<%= SiteLink_list.Pager.FromIndex %>&nbsp;<%= Language.Phrase("To") %>&nbsp;<%= SiteLink_list.Pager.ToIndex %>&nbsp;<%= Language.Phrase("Of") %>&nbsp;<%= SiteLink_list.Pager.RecordCount %></span>
<% Else %>
	<% If SiteLink_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker"><%= Language.Phrase("EnterSearchCriteria") %></span>
	<% Else %>
	<span class="aspnetmaker"><%= Language.Phrase("NoRecord") %></span>
	<% End If %>
<% End If %>
		</td>
<% If SiteLink_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td><%= Language.Phrase("RecordsPerPage") %>&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="SiteLink" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If SiteLink_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="20"<% If SiteLink_list.lDisplayRecs = 20 Then %> selected="selected"<% End If %>>20</option>
<option value="40"<% If SiteLink_list.lDisplayRecs = 40 Then %> selected="selected"<% End If %>>40</option>
<option value="60"<% If SiteLink_list.lDisplayRecs = 60 Then %> selected="selected"<% End If %>>60</option>
<option value="ALL"<% If SiteLink.RecordsPerPage = -1 Then %> selected="selected"<% End If %>><%= Language.Phrase("AllRecords") %></option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<div class="aspnetmaker">
<a href="<%= SiteLink_list.AddUrl %>"><%= Language.Phrase("AddLink") %></a>&nbsp;&nbsp;
</div>
</div>
<% End If %>
</td></tr></table>
<% If SiteLink.Export = "" AndAlso SiteLink.CurrentAction = "" Then %>
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
