<%@ Page ClassName="image_list" Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="image_list.aspx.vb" Inherits="image_list" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<%@ Register TagPrefix="wpmWebsite" TagName="MasterTable_Company" Src="company_master.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If Image.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var Image_list = new ew_Page("Image_list");
// page properties
Image_list.PageID = "list"; // page ID
Image_list.FormID = "fImagelist"; // form ID 
var EW_PAGE_ID = Image_list.PageID; // for backward compatibility
// extend page with Form_CustomValidate function
Image_list.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
Image_list.ValidateRequired = true; // uses JavaScript validation
<% Else %>
Image_list.ValidateRequired = false; // no JavaScript validation
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
<% If Image.Export = "" Then %>
<%
gsMasterReturnUrl = "company_list.aspx"
If Image_list.sDbMasterFilter <> "" AndAlso Image.CurrentMasterTable = "Company" Then
	If Image_list.bMasterRecordExists Then
		If Image.CurrentMasterTable = Image.TableVar Then gsMasterReturnUrl = gsMasterReturnUrl & "?" & EW_TABLE_SHOW_MASTER & "="
%>
<wpmWebsite:MasterTable_Company id="MasterTable_Company" runat="server" />
<%
	End If
End If
%>
<% End If %>
<%

' Load recordset
Rs = Image_list.LoadRecordset()
	Image_list.lStartRec = 1
	If Image_list.lDisplayRecs <= 0 Then ' Display all records
		Image_list.lDisplayRecs = Image_list.lTotalRecs
	End If
	If Not (Image.ExportAll AndAlso Image.Export <> "") Then
		Image_list.SetUpStartRec() ' Set up start record position
	End If
%>
<p><span class="aspnetmaker" style="white-space: nowrap;"><%= Language.Phrase("TblTypeTABLE") %><%= Image.TableCaption %>
<% If Image.Export = "" AndAlso Image.CurrentAction = "" Then %>
&nbsp;&nbsp;<a href="<%= Image_list.ExportExcelUrl %>"><img src='images/exportxls.gif' alt='<%= ew_HtmlEncode(Language.Phrase("ExportToExcel")) %>' title='<%= ew_HtmlEncode(Language.Phrase("ExportToExcel")) %>' width='16' height='16' border='0'></a>
<% End If %>
</span></p>
<% If Image.Export = "" AndAlso Image.CurrentAction = "" Then %>
<a href="javascript:ew_ToggleSearchPanel(Image_list);" style="text-decoration: none;"><img id="Image_list_SearchImage" src="images/collapse.gif" alt="" width="9" height="9" border="0"></a><span class="aspnetmaker">&nbsp;<%= Language.Phrase("Search") %></span><br>
<div id="Image_list_SearchPanel">
<form name="fImagelistsrch" id="fImagelistsrch" class="ewForm">
<input type="hidden" id="t" name="t" value="Image" />
<table class="ewBasicSearch">
	<tr>
		<td><span class="aspnetmaker">
			<a href="<%= Image_list.PageUrl %>cmd=reset"><%= Language.Phrase("ShowAll") %></a>&nbsp;
			<a href="image_srch.aspx"><%= Language.Phrase("AdvancedSearch") %></a>&nbsp;
		</span></td>
	</tr>
</table>
</form>
</div>
<% End If %>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(Image_list.DebugMsg) %>
<% Image_list.ShowMessage() %>
<br />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<form name="fImagelist" id="fImagelist" class="ewForm" method="post">
<div id="gmp_Image" class="ewGridMiddlePanel">
<% If Image_list.lTotalRecs > 0 Then %>
<table cellspacing="0" rowhighlightclass="ewTableHighlightRow" rowselectclass="ewTableSelectRow" roweditclass="ewTableEditRow" class="ewTable ewTableSeparate">
<%= Image.TableCustomInnerHTML %>
<thead><!-- Table header -->
	<tr class="ewTableHeader">
<%

		' Render list options
		Image_list.RenderListOptions()

		' Render list options (header, left)
		Image_list.ListOptions.Render("header", "left")
%>
<% If Image.ImageID.Visible Then ' ImageID %>
	<% If Image.SortUrl(Image.ImageID) = "" Then %>
		<td><%= Image.ImageID.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= Image.SortUrl(Image.ImageID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= Image.ImageID.FldCaption %></td><td style="width: 10px;"><% If Image.ImageID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Image.ImageID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If Image.ImageName.Visible Then ' ImageName %>
	<% If Image.SortUrl(Image.ImageName) = "" Then %>
		<td><%= Image.ImageName.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= Image.SortUrl(Image.ImageName) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= Image.ImageName.FldCaption %></td><td style="width: 10px;"><% If Image.ImageName.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Image.ImageName.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If Image.ImageFileName.Visible Then ' ImageFileName %>
	<% If Image.SortUrl(Image.ImageFileName) = "" Then %>
		<td><%= Image.ImageFileName.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= Image.SortUrl(Image.ImageFileName) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= Image.ImageFileName.FldCaption %></td><td style="width: 10px;"><% If Image.ImageFileName.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Image.ImageFileName.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If Image.ImageThumbFileName.Visible Then ' ImageThumbFileName %>
	<% If Image.SortUrl(Image.ImageThumbFileName) = "" Then %>
		<td><%= Image.ImageThumbFileName.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= Image.SortUrl(Image.ImageThumbFileName) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= Image.ImageThumbFileName.FldCaption %></td><td style="width: 10px;"><% If Image.ImageThumbFileName.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Image.ImageThumbFileName.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If Image.ImageDate.Visible Then ' ImageDate %>
	<% If Image.SortUrl(Image.ImageDate) = "" Then %>
		<td><%= Image.ImageDate.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= Image.SortUrl(Image.ImageDate) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= Image.ImageDate.FldCaption %></td><td style="width: 10px;"><% If Image.ImageDate.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Image.ImageDate.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If Image.Active.Visible Then ' Active %>
	<% If Image.SortUrl(Image.Active) = "" Then %>
		<td><%= Image.Active.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= Image.SortUrl(Image.Active) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= Image.Active.FldCaption %></td><td style="width: 10px;"><% If Image.Active.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Image.Active.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If Image.ModifiedDT.Visible Then ' ModifiedDT %>
	<% If Image.SortUrl(Image.ModifiedDT) = "" Then %>
		<td><%= Image.ModifiedDT.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= Image.SortUrl(Image.ModifiedDT) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= Image.ModifiedDT.FldCaption %></td><td style="width: 10px;"><% If Image.ModifiedDT.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Image.ModifiedDT.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If Image.VersionNo.Visible Then ' VersionNo %>
	<% If Image.SortUrl(Image.VersionNo) = "" Then %>
		<td><%= Image.VersionNo.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= Image.SortUrl(Image.VersionNo) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= Image.VersionNo.FldCaption %></td><td style="width: 10px;"><% If Image.VersionNo.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Image.VersionNo.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If Image.ContactID.Visible Then ' ContactID %>
	<% If Image.SortUrl(Image.ContactID) = "" Then %>
		<td><%= Image.ContactID.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= Image.SortUrl(Image.ContactID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= Image.ContactID.FldCaption %></td><td style="width: 10px;"><% If Image.ContactID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Image.ContactID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If Image.CompanyID.Visible Then ' CompanyID %>
	<% If Image.SortUrl(Image.CompanyID) = "" Then %>
		<td><%= Image.CompanyID.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= Image.SortUrl(Image.CompanyID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= Image.CompanyID.FldCaption %></td><td style="width: 10px;"><% If Image.CompanyID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Image.CompanyID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If Image.title.Visible Then ' title %>
	<% If Image.SortUrl(Image.title) = "" Then %>
		<td><%= Image.title.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= Image.SortUrl(Image.title) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= Image.title.FldCaption %></td><td style="width: 10px;"><% If Image.title.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Image.title.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If Image.medium.Visible Then ' medium %>
	<% If Image.SortUrl(Image.medium) = "" Then %>
		<td><%= Image.medium.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= Image.SortUrl(Image.medium) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= Image.medium.FldCaption %></td><td style="width: 10px;"><% If Image.medium.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Image.medium.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If Image.size.Visible Then ' size %>
	<% If Image.SortUrl(Image.size) = "" Then %>
		<td><%= Image.size.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= Image.SortUrl(Image.size) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= Image.size.FldCaption %></td><td style="width: 10px;"><% If Image.size.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Image.size.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If Image.price.Visible Then ' price %>
	<% If Image.SortUrl(Image.price) = "" Then %>
		<td><%= Image.price.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= Image.SortUrl(Image.price) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= Image.price.FldCaption %></td><td style="width: 10px;"><% If Image.price.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Image.price.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If Image.color.Visible Then ' color %>
	<% If Image.SortUrl(Image.color) = "" Then %>
		<td><%= Image.color.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= Image.SortUrl(Image.color) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= Image.color.FldCaption %></td><td style="width: 10px;"><% If Image.color.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Image.color.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If Image.subject.Visible Then ' subject %>
	<% If Image.SortUrl(Image.subject) = "" Then %>
		<td><%= Image.subject.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= Image.SortUrl(Image.subject) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= Image.subject.FldCaption %></td><td style="width: 10px;"><% If Image.subject.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Image.subject.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<% If Image.sold.Visible Then ' sold %>
	<% If Image.SortUrl(Image.sold) = "" Then %>
		<td><%= Image.sold.FldCaption %></td>
	<% Else %>
		<td><div class="ewPointer" onmousedown="ew_Sort(event,'<%= Image.SortUrl(Image.sold) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><thead><tr><td><%= Image.sold.FldCaption %></td><td style="width: 10px;"><% If Image.sold.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Image.sold.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></thead></table>
		</div></td>
	<% End If %>
<% End If %>		
<%

		' Render list options (header, right)
		Image_list.ListOptions.Render("header", "right")
%>
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

' Initialize Aggregate
Image.RowType = EW_ROWTYPE_AGGREGATEINIT
Image_list.RenderRow()
Image_list.lRowCnt = 0

' Output data rows
Do While (Image.CurrentAction = "gridadd" OrElse Rs.Read()) AndAlso (Image_list.lRecCnt < Image_list.lStopRec)
	Image_list.lRecCnt = Image_list.lRecCnt + 1
	If Image_list.lRecCnt >= Image_list.lStartRec Then
		Image_list.lRowCnt = Image_list.lRowCnt + 1
	Image.CssClass = ""
	Image.CssStyle = ""
	Image.RowAttrs.Clear()
	ew_SetAttr(Image.RowAttrs, New String(){"onmouseover", "onmouseout", "onclick"}, New String(){"ew_MouseOver(event, this);", "ew_MouseOut(event, this);", "ew_Click(event, this);"})
	If Image.CurrentAction = "gridadd" Then
		Image_list.LoadDefaultValues() ' Load default values
	Else
		Image_list.LoadRowValues(Rs) ' Load row values
	End If
	Image.RowType = EW_ROWTYPE_VIEW ' Render view

	' Render row
	Image_list.RenderRow()

	' Render list options
	Image_list.RenderListOptions()
%>
	<tr<%= Image.RowAttributes %>>
<%

		' Render list options (body, left)
		Image_list.ListOptions.Render("body", "left")
%>
	<% If Image.ImageID.Visible Then ' ImageID %>
		<td<%= Image.ImageID.CellAttributes %>>
<div<%= Image.ImageID.ViewAttributes %>><%= Image.ImageID.ListViewValue %></div>
</td>
	<% End If %>
	<% If Image.ImageName.Visible Then ' ImageName %>
		<td<%= Image.ImageName.CellAttributes %>>
<div<%= Image.ImageName.ViewAttributes %>><%= Image.ImageName.ListViewValue %></div>
</td>
	<% End If %>
	<% If Image.ImageFileName.Visible Then ' ImageFileName %>
		<td<%= Image.ImageFileName.CellAttributes %>>
<div<%= Image.ImageFileName.ViewAttributes %>><%= Image.ImageFileName.ListViewValue %></div>
</td>
	<% End If %>
	<% If Image.ImageThumbFileName.Visible Then ' ImageThumbFileName %>
		<td<%= Image.ImageThumbFileName.CellAttributes %>>
<div<%= Image.ImageThumbFileName.ViewAttributes %>><%= Image.ImageThumbFileName.ListViewValue %></div>
</td>
	<% End If %>
	<% If Image.ImageDate.Visible Then ' ImageDate %>
		<td<%= Image.ImageDate.CellAttributes %>>
<div<%= Image.ImageDate.ViewAttributes %>><%= Image.ImageDate.ListViewValue %></div>
</td>
	<% End If %>
	<% If Image.Active.Visible Then ' Active %>
		<td<%= Image.Active.CellAttributes %>>
<% If Convert.ToString(Image.Active.CurrentValue) = "1" Then %>
<input type="checkbox" value="<%= Image.Active.ListViewValue %>" checked onclick="this.form.reset();" disabled="disabled" />
<% Else %>
<input type="checkbox" value="<%= Image.Active.ListViewValue %>" onclick="this.form.reset();" disabled="disabled" />
<% End If %>
</td>
	<% End If %>
	<% If Image.ModifiedDT.Visible Then ' ModifiedDT %>
		<td<%= Image.ModifiedDT.CellAttributes %>>
<div<%= Image.ModifiedDT.ViewAttributes %>><%= Image.ModifiedDT.ListViewValue %></div>
</td>
	<% End If %>
	<% If Image.VersionNo.Visible Then ' VersionNo %>
		<td<%= Image.VersionNo.CellAttributes %>>
<div<%= Image.VersionNo.ViewAttributes %>><%= Image.VersionNo.ListViewValue %></div>
</td>
	<% End If %>
	<% If Image.ContactID.Visible Then ' ContactID %>
		<td<%= Image.ContactID.CellAttributes %>>
<div<%= Image.ContactID.ViewAttributes %>><%= Image.ContactID.ListViewValue %></div>
</td>
	<% End If %>
	<% If Image.CompanyID.Visible Then ' CompanyID %>
		<td<%= Image.CompanyID.CellAttributes %>>
<div<%= Image.CompanyID.ViewAttributes %>><%= Image.CompanyID.ListViewValue %></div>
</td>
	<% End If %>
	<% If Image.title.Visible Then ' title %>
		<td<%= Image.title.CellAttributes %>>
<div<%= Image.title.ViewAttributes %>><%= Image.title.ListViewValue %></div>
</td>
	<% End If %>
	<% If Image.medium.Visible Then ' medium %>
		<td<%= Image.medium.CellAttributes %>>
<div<%= Image.medium.ViewAttributes %>><%= Image.medium.ListViewValue %></div>
</td>
	<% End If %>
	<% If Image.size.Visible Then ' size %>
		<td<%= Image.size.CellAttributes %>>
<div<%= Image.size.ViewAttributes %>><%= Image.size.ListViewValue %></div>
</td>
	<% End If %>
	<% If Image.price.Visible Then ' price %>
		<td<%= Image.price.CellAttributes %>>
<div<%= Image.price.ViewAttributes %>><%= Image.price.ListViewValue %></div>
</td>
	<% End If %>
	<% If Image.color.Visible Then ' color %>
		<td<%= Image.color.CellAttributes %>>
<div<%= Image.color.ViewAttributes %>><%= Image.color.ListViewValue %></div>
</td>
	<% End If %>
	<% If Image.subject.Visible Then ' subject %>
		<td<%= Image.subject.CellAttributes %>>
<div<%= Image.subject.ViewAttributes %>><%= Image.subject.ListViewValue %></div>
</td>
	<% End If %>
	<% If Image.sold.Visible Then ' sold %>
		<td<%= Image.sold.CellAttributes %>>
<% If Convert.ToString(Image.sold.CurrentValue) = "1" Then %>
<input type="checkbox" value="<%= Image.sold.ListViewValue %>" checked onclick="this.form.reset();" disabled="disabled" />
<% Else %>
<input type="checkbox" value="<%= Image.sold.ListViewValue %>" onclick="this.form.reset();" disabled="disabled" />
<% End If %>
</td>
	<% End If %>
<%

		' Render list options (body, right)
		Image_list.ListOptions.Render("body", "right")
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
<% If Image.Export = "" Then %>
<div class="ewGridLowerPanel">
<% If Image.CurrentAction <> "gridadd" AndAlso Image.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If Image_list.Pager Is Nothing Then Image_list.Pager = New cPrevNextPager(Image_list.lStartRec, Image_list.lDisplayRecs, Image_list.lTotalRecs) %>
<% If Image_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker"><%= Language.Phrase("Page") %>&nbsp;</span></td>
<!--first page button-->
	<% If Image_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= Image_list.PageUrl %>start=<%= Image_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="<%= Language.Phrase("PagerFirst") %>" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="<%= Language.Phrase("PagerFirst") %>" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If Image_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= Image_list.PageUrl %>start=<%= Image_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="<%= Language.Phrase("PagerPrevious") %>" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="<%= Language.Phrase("PagerPrevious") %>" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= Image_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If Image_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= Image_list.PageUrl %>start=<%= Image_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="<%= Language.Phrase("PagerNext") %>" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="<%= Language.Phrase("PagerNext") %>" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If Image_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= Image_list.PageUrl %>start=<%= Image_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="<%= Language.Phrase("PagerLast") %>" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="<%= Language.Phrase("PagerLast") %>" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;<%= Language.Phrase("Of") %>&nbsp;<%= Image_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker"><%= Language.Phrase("Record") %>&nbsp;<%= Image_list.Pager.FromIndex %>&nbsp;<%= Language.Phrase("To") %>&nbsp;<%= Image_list.Pager.ToIndex %>&nbsp;<%= Language.Phrase("Of") %>&nbsp;<%= Image_list.Pager.RecordCount %></span>
<% Else %>
	<% If Image_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker"><%= Language.Phrase("EnterSearchCriteria") %></span>
	<% Else %>
	<span class="aspnetmaker"><%= Language.Phrase("NoRecord") %></span>
	<% End If %>
<% End If %>
		</td>
<% If Image_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td><%= Language.Phrase("RecordsPerPage") %>&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="Image" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If Image_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="20"<% If Image_list.lDisplayRecs = 20 Then %> selected="selected"<% End If %>>20</option>
<option value="40"<% If Image_list.lDisplayRecs = 40 Then %> selected="selected"<% End If %>>40</option>
<option value="60"<% If Image_list.lDisplayRecs = 60 Then %> selected="selected"<% End If %>>60</option>
<option value="ALL"<% If Image.RecordsPerPage = -1 Then %> selected="selected"<% End If %>><%= Language.Phrase("AllRecords") %></option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<div class="aspnetmaker">
<a href="<%= Image_list.AddUrl %>"><%= Language.Phrase("AddLink") %></a>&nbsp;&nbsp;
</div>
</div>
<% End If %>
</td></tr></table>
<% If Image.Export = "" AndAlso Image.CurrentAction = "" Then %>
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
