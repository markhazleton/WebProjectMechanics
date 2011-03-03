<%@ Page ClassName="link_list" Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="link_list.aspx.vb" Inherits="link_list" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<%@ Register TagPrefix="wpmWebsite" TagName="MasterTable_Company" Src="company_master.ascx" %>
<%@ Register TagPrefix="wpmWebsite" TagName="MasterTable_LinkType" Src="linktype_master.ascx" %>
<%@ Register TagPrefix="wpmWebsite" TagName="MasterTable_LinkCategory" Src="linkcategory_master.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If Link.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var Link_list = new ew_Page("Link_list");
// page properties
Link_list.PageID = "list"; // page ID
Link_list.FormID = "fLinklist"; // form ID 
var EW_PAGE_ID = Link_list.PageID; // for backward compatibility
// extend page with Form_CustomValidate function
Link_list.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
Link_list.ValidateRequired = true; // uses JavaScript validation
<% Else %>
Link_list.ValidateRequired = false; // no JavaScript validation
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
<% If Link.Export = "" Then %>
<%
gsMasterReturnUrl = "company_list.aspx"
If Link_list.sDbMasterFilter <> "" AndAlso Link.CurrentMasterTable = "Company" Then
	If Link_list.bMasterRecordExists Then
		If Link.CurrentMasterTable = Link.TableVar Then gsMasterReturnUrl = gsMasterReturnUrl & "?" & EW_TABLE_SHOW_MASTER & "="
%>
<wpmWebsite:MasterTable_Company id="MasterTable_Company" runat="server" />
<%
	End If
End If
%>
<%
gsMasterReturnUrl = "linktype_list.aspx"
If Link_list.sDbMasterFilter <> "" AndAlso Link.CurrentMasterTable = "LinkType" Then
	If Link_list.bMasterRecordExists Then
		If Link.CurrentMasterTable = Link.TableVar Then gsMasterReturnUrl = gsMasterReturnUrl & "?" & EW_TABLE_SHOW_MASTER & "="
%>
<wpmWebsite:MasterTable_LinkType id="MasterTable_LinkType" runat="server" />
<%
	End If
End If
%>
<%
gsMasterReturnUrl = "linkcategory_list.aspx"
If Link_list.sDbMasterFilter <> "" AndAlso Link.CurrentMasterTable = "LinkCategory" Then
	If Link_list.bMasterRecordExists Then
		If Link.CurrentMasterTable = Link.TableVar Then gsMasterReturnUrl = gsMasterReturnUrl & "?" & EW_TABLE_SHOW_MASTER & "="
%>
<wpmWebsite:MasterTable_LinkCategory id="MasterTable_LinkCategory" runat="server" />
<%
	End If
End If
%>
<% End If %>
<%

' Load recordset
Rs = Link_list.LoadRecordset()
	Link_list.lStartRec = 1
	If Link_list.lDisplayRecs <= 0 Then ' Display all records
		Link_list.lDisplayRecs = Link_list.lTotalRecs
	End If
	If Not (Link.ExportAll AndAlso Link.Export <> "") Then
		Link_list.SetUpStartRec() ' Set up start record position
	End If
%>
<p><span class="aspnetmaker" style="white-space: nowrap;"><%= Language.Phrase("TblTypeTABLE") %><%= Link.TableCaption %>
<% If Link.Export = "" AndAlso Link.CurrentAction = "" Then %>
&nbsp;&nbsp;<a href="<%= Link_list.ExportExcelUrl %>"><img src='images/exportxls.gif' alt='<%= ew_HtmlEncode(Language.Phrase("ExportToExcel")) %>' title='<%= ew_HtmlEncode(Language.Phrase("ExportToExcel")) %>' width='16' height='16' border='0'></a>
<% End If %>
</span></p>
<% If Link.Export = "" AndAlso Link.CurrentAction = "" Then %>
<a href="javascript:ew_ToggleSearchPanel(Link_list);" style="text-decoration: none;"><img id="Link_list_SearchImage" src="images/collapse.gif" alt="" width="9" height="9" border="0"></a><span class="aspnetmaker">&nbsp;<%= Language.Phrase("Search") %></span><br>
<div id="Link_list_SearchPanel">
<form name="fLinklistsrch" id="fLinklistsrch" class="ewForm">
<input type="hidden" id="t" name="t" value="Link" />
<table class="ewBasicSearch">
	<tr>
		<td><span class="aspnetmaker">
			<a href="<%= Link_list.PageUrl %>cmd=reset"><%= Language.Phrase("ShowAll") %></a>&nbsp;
			<a href="link_srch.aspx"><%= Language.Phrase("AdvancedSearch") %></a>&nbsp;
		</span></td>
	</tr>
</table>
</form>
</div>
<% End If %>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(Link_list.DebugMsg) %>
<% Link_list.ShowMessage() %>
<br />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<form name="fLinklist" id="fLinklist" class="ewForm" method="post">
<div id="gmp_Link" class="ewGridMiddlePanel">
<% If Link_list.lTotalRecs > 0 Then %>
<table cellspacing="0" rowhighlightclass="ewTableHighlightRow" rowselectclass="ewTableSelectRow" roweditclass="ewTableEditRow" class="ewTable ewTableSeparate">
<%= Link.TableCustomInnerHTML %>
<thead><!-- Table header -->
	<tr class="ewTableHeader">
<%

		' Render list options
		Link_list.RenderListOptions()

		' Render list options (header, left)
		Link_list.ListOptions.Render("header", "left")
%>
<% If Link.Title.Visible Then ' Title %>
	<% If Link.SortUrl(Link.Title) = "" Then %>
		<td style="white-space: nowrap;"><%= Link.Title.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= Link.SortUrl(Link.Title) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn" style="white-space: nowrap;"><thead><tr><td><%= Link.Title.FldCaption %></td><td style="width: 10px;"><% If Link.Title.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Link.Title.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If Link.LinkTypeCD.Visible Then ' LinkTypeCD %>
	<% If Link.SortUrl(Link.LinkTypeCD) = "" Then %>
		<td style="white-space: nowrap;"><%= Link.LinkTypeCD.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= Link.SortUrl(Link.LinkTypeCD) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn" style="white-space: nowrap;"><thead><tr><td><%= Link.LinkTypeCD.FldCaption %></td><td style="width: 10px;"><% If Link.LinkTypeCD.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Link.LinkTypeCD.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If Link.CategoryID.Visible Then ' CategoryID %>
	<% If Link.SortUrl(Link.CategoryID) = "" Then %>
		<td style="white-space: nowrap;"><%= Link.CategoryID.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= Link.SortUrl(Link.CategoryID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn" style="white-space: nowrap;"><thead><tr><td><%= Link.CategoryID.FldCaption %></td><td style="width: 10px;"><% If Link.CategoryID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Link.CategoryID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If Link.CompanyID.Visible Then ' CompanyID %>
	<% If Link.SortUrl(Link.CompanyID) = "" Then %>
		<td style="white-space: nowrap;"><%= Link.CompanyID.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= Link.SortUrl(Link.CompanyID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn" style="white-space: nowrap;"><thead><tr><td><%= Link.CompanyID.FldCaption %></td><td style="width: 10px;"><% If Link.CompanyID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Link.CompanyID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If Link.SiteCategoryGroupID.Visible Then ' SiteCategoryGroupID %>
	<% If Link.SortUrl(Link.SiteCategoryGroupID) = "" Then %>
		<td style="white-space: nowrap;"><%= Link.SiteCategoryGroupID.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= Link.SortUrl(Link.SiteCategoryGroupID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn" style="white-space: nowrap;"><thead><tr><td><%= Link.SiteCategoryGroupID.FldCaption %></td><td style="width: 10px;"><% If Link.SiteCategoryGroupID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Link.SiteCategoryGroupID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If Link.zPageID.Visible Then ' PageID %>
	<% If Link.SortUrl(Link.zPageID) = "" Then %>
		<td style="white-space: nowrap;"><%= Link.zPageID.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= Link.SortUrl(Link.zPageID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn" style="white-space: nowrap;"><thead><tr><td><%= Link.zPageID.FldCaption %></td><td style="width: 10px;"><% If Link.zPageID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Link.zPageID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If Link.Views.Visible Then ' Views %>
	<% If Link.SortUrl(Link.Views) = "" Then %>
		<td style="white-space: nowrap;"><%= Link.Views.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= Link.SortUrl(Link.Views) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn" style="white-space: nowrap;"><thead><tr><td><%= Link.Views.FldCaption %></td><td style="width: 10px;"><% If Link.Views.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Link.Views.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If Link.Ranks.Visible Then ' Ranks %>
	<% If Link.SortUrl(Link.Ranks) = "" Then %>
		<td style="white-space: nowrap;"><%= Link.Ranks.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= Link.SortUrl(Link.Ranks) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn" style="white-space: nowrap;"><thead><tr><td><%= Link.Ranks.FldCaption %></td><td style="width: 10px;"><% If Link.Ranks.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Link.Ranks.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If Link.UserID.Visible Then ' UserID %>
	<% If Link.SortUrl(Link.UserID) = "" Then %>
		<td style="white-space: nowrap;"><%= Link.UserID.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= Link.SortUrl(Link.UserID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn" style="white-space: nowrap;"><thead><tr><td><%= Link.UserID.FldCaption %></td><td style="width: 10px;"><% If Link.UserID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Link.UserID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If Link.ASIN.Visible Then ' ASIN %>
	<% If Link.SortUrl(Link.ASIN) = "" Then %>
		<td style="white-space: nowrap;"><%= Link.ASIN.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= Link.SortUrl(Link.ASIN) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn" style="white-space: nowrap;"><thead><tr><td><%= Link.ASIN.FldCaption %></td><td style="width: 10px;"><% If Link.ASIN.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Link.ASIN.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If Link.DateAdd.Visible Then ' DateAdd %>
	<% If Link.SortUrl(Link.DateAdd) = "" Then %>
		<td style="white-space: nowrap;"><%= Link.DateAdd.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= Link.SortUrl(Link.DateAdd) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn" style="white-space: nowrap;"><thead><tr><td><%= Link.DateAdd.FldCaption %></td><td style="width: 10px;"><% If Link.DateAdd.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Link.DateAdd.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<%

		' Render list options (header, right)
		Link_list.ListOptions.Render("header", "right")
%>
	</tr>
</thead>
<tbody><!-- Table body -->
<%
If (Link.ExportAll AndAlso Link.Export <> "") Then
	Link_list.lStopRec = Link_list.lTotalRecs
Else
	Link_list.lStopRec = Link_list.lStartRec + Link_list.lDisplayRecs - 1 ' Set the last record to display
End If
If Link.CurrentAction = "gridadd" AndAlso Link_list.lStopRec = -1 Then
	Link_list.lStopRec = EW_GRIDADD_ROWS
End If 

' Move to first record
For i As Integer = 1 to Link_list.lStartRec - 1
	If Rs.Read() Then	Link_list.lRecCnt = Link_list.lRecCnt + 1
Next		

' Initialize Aggregate
Link.RowType = EW_ROWTYPE_AGGREGATEINIT
Link_list.RenderRow()
Link_list.lRowCnt = 0

' Output data rows
Do While (Link.CurrentAction = "gridadd" OrElse Rs.Read()) AndAlso (Link_list.lRecCnt < Link_list.lStopRec)
	Link_list.lRecCnt = Link_list.lRecCnt + 1
	If Link_list.lRecCnt >= Link_list.lStartRec Then
		Link_list.lRowCnt = Link_list.lRowCnt + 1
	Link.CssClass = ""
	Link.CssStyle = ""
	Link.RowAttrs.Clear()
	ew_SetAttr(Link.RowAttrs, New String(){"onmouseover", "onmouseout", "onclick"}, New String(){"ew_MouseOver(event, this);", "ew_MouseOut(event, this);", "ew_Click(event, this);"})
	If Link.CurrentAction = "gridadd" Then
		Link_list.LoadDefaultValues() ' Load default values
	Else
		Link_list.LoadRowValues(Rs) ' Load row values
	End If
	Link.RowType = EW_ROWTYPE_VIEW ' Render view

	' Render row
	Link_list.RenderRow()

	' Render list options
	Link_list.RenderListOptions()
%>
	<tr<%= Link.RowAttributes %>>
<%

		' Render list options (body, left)
		Link_list.ListOptions.Render("body", "left")
%>
	<% If Link.Title.Visible Then ' Title %>
		<td<%= Link.Title.CellAttributes %>>
<div<%= Link.Title.ViewAttributes %>><%= Link.Title.ListViewValue %></div>
</td>
	<% End If %>
	<% If Link.LinkTypeCD.Visible Then ' LinkTypeCD %>
		<td<%= Link.LinkTypeCD.CellAttributes %>>
<div<%= Link.LinkTypeCD.ViewAttributes %>><%= Link.LinkTypeCD.ListViewValue %></div>
</td>
	<% End If %>
	<% If Link.CategoryID.Visible Then ' CategoryID %>
		<td<%= Link.CategoryID.CellAttributes %>>
<div<%= Link.CategoryID.ViewAttributes %>><%= Link.CategoryID.ListViewValue %></div>
</td>
	<% End If %>
	<% If Link.CompanyID.Visible Then ' CompanyID %>
		<td<%= Link.CompanyID.CellAttributes %>>
<div<%= Link.CompanyID.ViewAttributes %>><%= Link.CompanyID.ListViewValue %></div>
</td>
	<% End If %>
	<% If Link.SiteCategoryGroupID.Visible Then ' SiteCategoryGroupID %>
		<td<%= Link.SiteCategoryGroupID.CellAttributes %>>
<div<%= Link.SiteCategoryGroupID.ViewAttributes %>><%= Link.SiteCategoryGroupID.ListViewValue %></div>
</td>
	<% End If %>
	<% If Link.zPageID.Visible Then ' PageID %>
		<td<%= Link.zPageID.CellAttributes %>>
<div<%= Link.zPageID.ViewAttributes %>><%= Link.zPageID.ListViewValue %></div>
</td>
	<% End If %>
	<% If Link.Views.Visible Then ' Views %>
		<td<%= Link.Views.CellAttributes %>>
<% If Convert.ToString(Link.Views.CurrentValue) = "1" Then %>
<input type="checkbox" value="<%= Link.Views.ListViewValue %>" checked onclick="this.form.reset();" disabled="disabled" />
<% Else %>
<input type="checkbox" value="<%= Link.Views.ListViewValue %>" onclick="this.form.reset();" disabled="disabled" />
<% End If %>
</td>
	<% End If %>
	<% If Link.Ranks.Visible Then ' Ranks %>
		<td<%= Link.Ranks.CellAttributes %>>
<div<%= Link.Ranks.ViewAttributes %>><%= Link.Ranks.ListViewValue %></div>
</td>
	<% End If %>
	<% If Link.UserID.Visible Then ' UserID %>
		<td<%= Link.UserID.CellAttributes %>>
<div<%= Link.UserID.ViewAttributes %>><%= Link.UserID.ListViewValue %></div>
</td>
	<% End If %>
	<% If Link.ASIN.Visible Then ' ASIN %>
		<td<%= Link.ASIN.CellAttributes %>>
<div<%= Link.ASIN.ViewAttributes %>><%= Link.ASIN.ListViewValue %></div>
</td>
	<% End If %>
	<% If Link.DateAdd.Visible Then ' DateAdd %>
		<td<%= Link.DateAdd.CellAttributes %>>
<div<%= Link.DateAdd.ViewAttributes %>><%= Link.DateAdd.ListViewValue %></div>
</td>
	<% End If %>
<%

		' Render list options (body, right)
		Link_list.ListOptions.Render("body", "right")
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
<% If Link.Export = "" Then %>
<div class="ewGridLowerPanel">
<% If Link.CurrentAction <> "gridadd" AndAlso Link.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If Link_list.Pager Is Nothing Then Link_list.Pager = New cPrevNextPager(Link_list.lStartRec, Link_list.lDisplayRecs, Link_list.lTotalRecs) %>
<% If Link_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker"><%= Language.Phrase("Page") %>&nbsp;</span></td>
<!--first page button-->
	<% If Link_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= Link_list.PageUrl %>start=<%= Link_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="<%= Language.Phrase("PagerFirst") %>" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="<%= Language.Phrase("PagerFirst") %>" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If Link_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= Link_list.PageUrl %>start=<%= Link_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="<%= Language.Phrase("PagerPrevious") %>" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="<%= Language.Phrase("PagerPrevious") %>" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= Link_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If Link_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= Link_list.PageUrl %>start=<%= Link_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="<%= Language.Phrase("PagerNext") %>" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="<%= Language.Phrase("PagerNext") %>" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If Link_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= Link_list.PageUrl %>start=<%= Link_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="<%= Language.Phrase("PagerLast") %>" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="<%= Language.Phrase("PagerLast") %>" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;<%= Language.Phrase("Of") %>&nbsp;<%= Link_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker"><%= Language.Phrase("Record") %>&nbsp;<%= Link_list.Pager.FromIndex %>&nbsp;<%= Language.Phrase("To") %>&nbsp;<%= Link_list.Pager.ToIndex %>&nbsp;<%= Language.Phrase("Of") %>&nbsp;<%= Link_list.Pager.RecordCount %></span>
<% Else %>
	<% If Link_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker"><%= Language.Phrase("EnterSearchCriteria") %></span>
	<% Else %>
	<span class="aspnetmaker"><%= Language.Phrase("NoRecord") %></span>
	<% End If %>
<% End If %>
		</td>
<% If Link_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td><%= Language.Phrase("RecordsPerPage") %>&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="Link" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If Link_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="20"<% If Link_list.lDisplayRecs = 20 Then %> selected="selected"<% End If %>>20</option>
<option value="40"<% If Link_list.lDisplayRecs = 40 Then %> selected="selected"<% End If %>>40</option>
<option value="60"<% If Link_list.lDisplayRecs = 60 Then %> selected="selected"<% End If %>>60</option>
<option value="ALL"<% If Link.RecordsPerPage = -1 Then %> selected="selected"<% End If %>><%= Language.Phrase("AllRecords") %></option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<div class="aspnetmaker">
<a href="<%= Link_list.AddUrl %>"><%= Language.Phrase("AddLink") %></a>&nbsp;&nbsp;
</div>
</div>
<% End If %>
</td></tr></table>
<% If Link.Export = "" AndAlso Link.CurrentAction = "" Then %>
<% End If %>
<% If Link.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
