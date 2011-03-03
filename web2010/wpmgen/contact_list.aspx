<%@ Page ClassName="contact_list" Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="contact_list.aspx.vb" Inherits="contact_list" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If Contact.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var Contact_list = new ew_Page("Contact_list");
// page properties
Contact_list.PageID = "list"; // page ID
Contact_list.FormID = "fContactlist"; // form ID 
var EW_PAGE_ID = Contact_list.PageID; // for backward compatibility
// extend page with Form_CustomValidate function
Contact_list.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
Contact_list.ValidateRequired = true; // uses JavaScript validation
<% Else %>
Contact_list.ValidateRequired = false; // no JavaScript validation
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
<% If Contact.Export = "" Then %>
<% End If %>
<%

' Load recordset
Rs = Contact_list.LoadRecordset()
	Contact_list.lStartRec = 1
	If Contact_list.lDisplayRecs <= 0 Then ' Display all records
		Contact_list.lDisplayRecs = Contact_list.lTotalRecs
	End If
	If Not (Contact.ExportAll AndAlso Contact.Export <> "") Then
		Contact_list.SetUpStartRec() ' Set up start record position
	End If
%>
<p><span class="aspnetmaker" style="white-space: nowrap;"><%= Language.Phrase("TblTypeTABLE") %><%= Contact.TableCaption %>
<% If Contact.Export = "" AndAlso Contact.CurrentAction = "" Then %>
&nbsp;&nbsp;<a href="<%= Contact_list.ExportExcelUrl %>"><img src='images/exportxls.gif' alt='<%= ew_HtmlEncode(Language.Phrase("ExportToExcel")) %>' title='<%= ew_HtmlEncode(Language.Phrase("ExportToExcel")) %>' width='16' height='16' border='0'></a>
<% End If %>
</span></p>
<% If Contact.Export = "" AndAlso Contact.CurrentAction = "" Then %>
<a href="javascript:ew_ToggleSearchPanel(Contact_list);" style="text-decoration: none;"><img id="Contact_list_SearchImage" src="images/collapse.gif" alt="" width="9" height="9" border="0"></a><span class="aspnetmaker">&nbsp;<%= Language.Phrase("Search") %></span><br>
<div id="Contact_list_SearchPanel">
<form name="fContactlistsrch" id="fContactlistsrch" class="ewForm">
<input type="hidden" id="t" name="t" value="Contact" />
<table class="ewBasicSearch">
	<tr>
		<td><span class="aspnetmaker">
			<a href="<%= Contact_list.PageUrl %>cmd=reset"><%= Language.Phrase("ShowAll") %></a>&nbsp;
			<a href="contact_srch.aspx"><%= Language.Phrase("AdvancedSearch") %></a>&nbsp;
		</span></td>
	</tr>
</table>
</form>
</div>
<% End If %>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(Contact_list.DebugMsg) %>
<% Contact_list.ShowMessage() %>
<br />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<form name="fContactlist" id="fContactlist" class="ewForm" method="post">
<div id="gmp_Contact" class="ewGridMiddlePanel">
<% If Contact_list.lTotalRecs > 0 Then %>
<table cellspacing="0" rowhighlightclass="ewTableHighlightRow" rowselectclass="ewTableSelectRow" roweditclass="ewTableEditRow" class="ewTable ewTableSeparate">
<%= Contact.TableCustomInnerHTML %>
<thead><!-- Table header -->
	<tr class="ewTableHeader">
<%

		' Render list options
		Contact_list.RenderListOptions()

		' Render list options (header, left)
		Contact_list.ListOptions.Render("header", "left")
%>
<% If Contact.LogonName.Visible Then ' LogonName %>
	<% If Contact.SortUrl(Contact.LogonName) = "" Then %>
		<td style="white-space: nowrap;"><%= Contact.LogonName.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= Contact.SortUrl(Contact.LogonName) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn" style="white-space: nowrap;"><thead><tr><td><%= Contact.LogonName.FldCaption %></td><td style="width: 10px;"><% If Contact.LogonName.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Contact.LogonName.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If Contact.CompanyID.Visible Then ' CompanyID %>
	<% If Contact.SortUrl(Contact.CompanyID) = "" Then %>
		<td style="white-space: nowrap;"><%= Contact.CompanyID.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= Contact.SortUrl(Contact.CompanyID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn" style="white-space: nowrap;"><thead><tr><td><%= Contact.CompanyID.FldCaption %></td><td style="width: 10px;"><% If Contact.CompanyID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Contact.CompanyID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If Contact.zEMail.Visible Then ' EMail %>
	<% If Contact.SortUrl(Contact.zEMail) = "" Then %>
		<td style="white-space: nowrap;"><%= Contact.zEMail.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= Contact.SortUrl(Contact.zEMail) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn" style="white-space: nowrap;"><thead><tr><td><%= Contact.zEMail.FldCaption %></td><td style="width: 10px;"><% If Contact.zEMail.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Contact.zEMail.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If Contact.PrimaryContact.Visible Then ' PrimaryContact %>
	<% If Contact.SortUrl(Contact.PrimaryContact) = "" Then %>
		<td style="white-space: nowrap;"><%= Contact.PrimaryContact.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= Contact.SortUrl(Contact.PrimaryContact) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn" style="white-space: nowrap;"><thead><tr><td><%= Contact.PrimaryContact.FldCaption %></td><td style="width: 10px;"><% If Contact.PrimaryContact.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Contact.PrimaryContact.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<%

		' Render list options (header, right)
		Contact_list.ListOptions.Render("header", "right")
%>
	</tr>
</thead>
<tbody><!-- Table body -->
<%
If (Contact.ExportAll AndAlso Contact.Export <> "") Then
	Contact_list.lStopRec = Contact_list.lTotalRecs
Else
	Contact_list.lStopRec = Contact_list.lStartRec + Contact_list.lDisplayRecs - 1 ' Set the last record to display
End If
If Contact.CurrentAction = "gridadd" AndAlso Contact_list.lStopRec = -1 Then
	Contact_list.lStopRec = EW_GRIDADD_ROWS
End If 

' Move to first record
For i As Integer = 1 to Contact_list.lStartRec - 1
	If Rs.Read() Then	Contact_list.lRecCnt = Contact_list.lRecCnt + 1
Next		

' Initialize Aggregate
Contact.RowType = EW_ROWTYPE_AGGREGATEINIT
Contact_list.RenderRow()
Contact_list.lRowCnt = 0

' Output data rows
Do While (Contact.CurrentAction = "gridadd" OrElse Rs.Read()) AndAlso (Contact_list.lRecCnt < Contact_list.lStopRec)
	Contact_list.lRecCnt = Contact_list.lRecCnt + 1
	If Contact_list.lRecCnt >= Contact_list.lStartRec Then
		Contact_list.lRowCnt = Contact_list.lRowCnt + 1
	Contact.CssClass = ""
	Contact.CssStyle = ""
	Contact.RowAttrs.Clear()
	ew_SetAttr(Contact.RowAttrs, New String(){"onmouseover", "onmouseout", "onclick"}, New String(){"ew_MouseOver(event, this);", "ew_MouseOut(event, this);", "ew_Click(event, this);"})
	If Contact.CurrentAction = "gridadd" Then
		Contact_list.LoadDefaultValues() ' Load default values
	Else
		Contact_list.LoadRowValues(Rs) ' Load row values
	End If
	Contact.RowType = EW_ROWTYPE_VIEW ' Render view

	' Render row
	Contact_list.RenderRow()

	' Render list options
	Contact_list.RenderListOptions()
%>
	<tr<%= Contact.RowAttributes %>>
<%

		' Render list options (body, left)
		Contact_list.ListOptions.Render("body", "left")
%>
	<% If Contact.LogonName.Visible Then ' LogonName %>
		<td<%= Contact.LogonName.CellAttributes %>>
<div<%= Contact.LogonName.ViewAttributes %>><%= Contact.LogonName.ListViewValue %></div>
</td>
	<% End If %>
	<% If Contact.CompanyID.Visible Then ' CompanyID %>
		<td<%= Contact.CompanyID.CellAttributes %>>
<div<%= Contact.CompanyID.ViewAttributes %>><%= Contact.CompanyID.ListViewValue %></div>
</td>
	<% End If %>
	<% If Contact.zEMail.Visible Then ' EMail %>
		<td<%= Contact.zEMail.CellAttributes %>>
<div<%= Contact.zEMail.ViewAttributes %>><%= Contact.zEMail.ListViewValue %></div>
</td>
	<% End If %>
	<% If Contact.PrimaryContact.Visible Then ' PrimaryContact %>
		<td<%= Contact.PrimaryContact.CellAttributes %>>
<div<%= Contact.PrimaryContact.ViewAttributes %>><%= Contact.PrimaryContact.ListViewValue %></div>
</td>
	<% End If %>
<%

		' Render list options (body, right)
		Contact_list.ListOptions.Render("body", "right")
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
<% If Contact.Export = "" Then %>
<div class="ewGridLowerPanel">
<% If Contact.CurrentAction <> "gridadd" AndAlso Contact.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If Contact_list.Pager Is Nothing Then Contact_list.Pager = New cPrevNextPager(Contact_list.lStartRec, Contact_list.lDisplayRecs, Contact_list.lTotalRecs) %>
<% If Contact_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker"><%= Language.Phrase("Page") %>&nbsp;</span></td>
<!--first page button-->
	<% If Contact_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= Contact_list.PageUrl %>start=<%= Contact_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="<%= Language.Phrase("PagerFirst") %>" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="<%= Language.Phrase("PagerFirst") %>" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If Contact_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= Contact_list.PageUrl %>start=<%= Contact_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="<%= Language.Phrase("PagerPrevious") %>" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="<%= Language.Phrase("PagerPrevious") %>" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= Contact_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If Contact_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= Contact_list.PageUrl %>start=<%= Contact_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="<%= Language.Phrase("PagerNext") %>" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="<%= Language.Phrase("PagerNext") %>" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If Contact_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= Contact_list.PageUrl %>start=<%= Contact_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="<%= Language.Phrase("PagerLast") %>" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="<%= Language.Phrase("PagerLast") %>" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;<%= Language.Phrase("Of") %>&nbsp;<%= Contact_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker"><%= Language.Phrase("Record") %>&nbsp;<%= Contact_list.Pager.FromIndex %>&nbsp;<%= Language.Phrase("To") %>&nbsp;<%= Contact_list.Pager.ToIndex %>&nbsp;<%= Language.Phrase("Of") %>&nbsp;<%= Contact_list.Pager.RecordCount %></span>
<% Else %>
	<% If Contact_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker"><%= Language.Phrase("EnterSearchCriteria") %></span>
	<% Else %>
	<span class="aspnetmaker"><%= Language.Phrase("NoRecord") %></span>
	<% End If %>
<% End If %>
		</td>
<% If Contact_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td><%= Language.Phrase("RecordsPerPage") %>&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="Contact" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If Contact_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="20"<% If Contact_list.lDisplayRecs = 20 Then %> selected="selected"<% End If %>>20</option>
<option value="40"<% If Contact_list.lDisplayRecs = 40 Then %> selected="selected"<% End If %>>40</option>
<option value="60"<% If Contact_list.lDisplayRecs = 60 Then %> selected="selected"<% End If %>>60</option>
<option value="ALL"<% If Contact.RecordsPerPage = -1 Then %> selected="selected"<% End If %>><%= Language.Phrase("AllRecords") %></option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<div class="aspnetmaker">
<a href="<%= Contact_list.AddUrl %>"><%= Language.Phrase("AddLink") %></a>&nbsp;&nbsp;
</div>
</div>
<% End If %>
</td></tr></table>
<% If Contact.Export = "" AndAlso Contact.CurrentAction = "" Then %>
<% End If %>
<% If Contact.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
