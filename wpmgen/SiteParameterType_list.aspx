<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="SiteParameterType_list.aspx.vb" Inherits="SiteParameterType_list" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If SiteParameterType.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var SiteParameterType_list = new ew_Page("SiteParameterType_list");
// page properties
SiteParameterType_list.PageID = "list"; // page ID
var EW_PAGE_ID = SiteParameterType_list.PageID; // for backward compatibility
// extend page with ValidateForm function
SiteParameterType_list.ValidateForm = function(fobj) {
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
		elm = fobj.elements["x" + infix + "_SiteParameterTypeOrder"];
		if (elm && !ew_CheckInteger(elm.value))
			return ew_OnError(this, elm, "Incorrect integer - Parameter Order");
		} // End Grid Add checking
	}
	if (fobj.a_list && fobj.a_list.value == "gridinsert" && addcnt == 0) { // No row added
		alert("No records to be added");
		return false;
	}
	return true;
}
// Extend page with empty row check
SiteParameterType_list.EmptyRow = function(fobj, infix) {
	if (ew_ValueChanged(fobj, infix, "SiteParameterTypeNM")) return false;
	if (ew_ValueChanged(fobj, infix, "SiteParameterTypeDS")) return false;
	if (ew_ValueChanged(fobj, infix, "SiteParameterTypeOrder")) return false;
	return true;
}
<% If EW_CLIENT_VALIDATE Then %>
SiteParameterType_list.ValidateRequired = true; // uses JavaScript validation
<% Else %>
SiteParameterType_list.ValidateRequired = false; // no JavaScript validation
<% End If %>
//-->
</script>
<style>
	/* styles for details panel */
	.yui-overlay { position:absolute;background:#fff;border:2px solid orange;padding:4px;margin:10px; }
	.yui-overlay .hd { border:1px solid red;padding:5px; }
	.yui-overlay .bd { border:0px solid green;padding:5px; }
	.yui-overlay .ft { border:1px solid blue;padding:5px; }
</style>
<div id="ewDetailsDiv" name="ewDetailsDivDiv" style="visibility:hidden"></div>
<script language="JavaScript" type="text/javascript">
<!--
// YUI container
var ewDetailsDiv;
var ew_AjaxDetailsTimer = null;
// init details div

function ew_InitDetailsDiv() {
	ewDetailsDiv = new YAHOO.widget.Overlay("ewDetailsDiv", { context:null, visible:false} );
	ewDetailsDiv.beforeMoveEvent.subscribe(ew_EnforceConstraints, ewDetailsDiv, true);
	ewDetailsDiv.render();
}
// init details div on window.load
YAHOO.util.Event.addListener(window, "load", ew_InitDetailsDiv);
// show results in details div
var ew_AjaxHandleSuccess = function(o) {
	if (ewDetailsDiv && o.responseText !== undefined) {
		ewDetailsDiv.cfg.applyConfig({context:[o.argument.id,o.argument.elcorner,o.argument.ctxcorner], visible:false}, true);
		ewDetailsDiv.setBody(o.responseText);
		ewDetailsDiv.render();
		ew_SetupTable(document.getElementById("ewDetailsPreviewTable"));
		ewDetailsDiv.show();
	}
}
// show error in details div
var ew_AjaxHandleFailure = function(o) {
	if (ewDetailsDiv && o.responseText != "") {
		ewDetailsDiv.cfg.applyConfig({context:[o.argument.id,o.argument.elcorner,o.argument.ctxcorner], visible:false, constraintoviewport:true}, true);
		ewDetailsDiv.setBody(o.responseText);
		ewDetailsDiv.render();
		ewDetailsDiv.show();
	}
}
// show details div

function ew_AjaxShowDetails(obj, url) {
	if (ew_AjaxDetailsTimer)
		clearTimeout(ew_AjaxDetailsTimer);
	ew_AjaxDetailsTimer = setTimeout(function() { YAHOO.util.Connect.asyncRequest('GET', url, {success: ew_AjaxHandleSuccess , failure: ew_AjaxHandleFailure, argument:{id: obj.id, elcorner: "tl", ctxcorner: "tr"}}) }, 200);
}
// hide details div

function ew_AjaxHideDetails(obj) {
	if (ew_AjaxDetailsTimer)
		clearTimeout(ew_AjaxDetailsTimer);
	if (ewDetailsDiv)
		ewDetailsDiv.hide();
}
// move details div
ew_EnforceConstraints = function(type, args, obj) {
	var pos = args[0];
	var x = pos[0];
	var y = pos[1];
	var offsetHeight = this.element.offsetHeight;
	var offsetWidth = this.element.offsetWidth;
	var viewPortWidth = YAHOO.util.Dom.getViewportWidth();
	var viewPortHeight = YAHOO.util.Dom.getViewportHeight();
	var scrollX = document.documentElement.scrollLeft || document.body.scrollLeft;
	var scrollY = document.documentElement.scrollTop || document.body.scrollTop;
	var topConstraint = scrollY + 10;
	var leftConstraint = scrollX + 10;
	var bottomConstraint = scrollY + viewPortHeight - offsetHeight - 10;
	var rightConstraint = scrollX + viewPortWidth - offsetWidth - 10;
// if (x < leftConstraint) {
// x = leftConstraint;
// } else if (x > rightConstraint) {
// x = rightConstraint;
// }
	if (y < topConstraint) {
		y = topConstraint;
	} else if (y > bottomConstraint) {
		y = (bottomConstraint < topConstraint) ? topConstraint : bottomConstraint;
	}
// this.cfg.setProperty("x", x, true);
	this.cfg.setProperty("y", y, true);
	this.cfg.setProperty("xy", [x,y], true);
};
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
<% If SiteParameterType.Export = "" Then %>
<% End If %>
<%
If SiteParameterType.CurrentAction = "gridadd" Then SiteParameterType.CurrentFilter = "0=1"

' Load recordset
Rs = SiteParameterType_list.LoadRecordset()
If SiteParameterType.CurrentAction = "gridadd" Then
	SiteParameterType_list.lStartRec = 1
	If SiteParameterType_list.lDisplayRecs <= 0 Then SiteParameterType_list.lDisplayRecs = 25
	SiteParameterType_list.lTotalRecs = SiteParameterType_list.lDisplayRecs
	SiteParameterType_list.lStopRec = SiteParameterType_list.lDisplayRecs
Else
	SiteParameterType_list.lStartRec = 1
	If SiteParameterType_list.lDisplayRecs <= 0 Then ' Display all records
		SiteParameterType_list.lDisplayRecs = SiteParameterType_list.lTotalRecs
	End If
	If Not (SiteParameterType.ExportAll AndAlso SiteParameterType.Export <> "") Then
		SiteParameterType_list.SetUpStartRec() ' Set up start record position
	End If
End If
%>
<p><span class="aspnetmaker" style="white-space: nowrap;">TABLE: Parameter Type
<% If SiteParameterType.Export = "" AndAlso SiteParameterType.CurrentAction = "" Then %>
&nbsp;&nbsp;<a href="<%= SiteParameterType_list.PageUrl %>export=excel"><img src='images/exportxls.gif' alt='Export to Excel' title='Export to Excel' width='16' height='16' border='0'></a>
<% End If %>
</span></p>
<% If SiteParameterType.Export = "" AndAlso SiteParameterType.CurrentAction = "" Then %>
<a href="javascript:ew_ToggleSearchPanel(SiteParameterType_list);" style="text-decoration: none;"><img id="SiteParameterType_list_SearchImage" src="images/collapse.gif" alt="" width="9" height="9" border="0"></a><span class="aspnetmaker">&nbsp;Search</span><br>
<div id="SiteParameterType_list_SearchPanel">
<form name="fSiteParameterTypelistsrch" id="fSiteParameterTypelistsrch" class="ewForm">
<input type="hidden" id="t" name="t" value="SiteParameterType" />
<table class="ewBasicSearch">
	<tr>
		<td><span class="aspnetmaker">
			<input type="text" name="<%= EW_TABLE_BASIC_SEARCH %>" id="<%= EW_TABLE_BASIC_SEARCH %>" size="20" value="<%= ew_HtmlEncode(SiteParameterType.BasicSearchKeyword) %>" />
			<input type="Submit" name="Submit" id="Submit" value="Search (*)" />&nbsp;
			<a href="<%= SiteParameterType_list.PageUrl %>cmd=reset">Show all</a>&nbsp;
			<a href="SiteParameterType_srch.aspx">Advanced Search</a>&nbsp;
		</span></td>
	</tr>
	<tr>
	<td><span class="aspnetmaker"><label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value=""<% If SiteParameterType.BasicSearchType = "" Then %> checked="checked"<% End If %> />Exact phrase</label>&nbsp;&nbsp;<label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value="AND"<% If SiteParameterType.BasicSearchType = "AND" Then %> checked="checked"<% End If %> />All words</label>&nbsp;&nbsp;<label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value="OR"<% If SiteParameterType.BasicSearchType = "OR" Then %> checked="checked"<% End If %> />Any word</label></span></td>
	</tr>
</table>
</form>
</div>
<% End If %>
<% SiteParameterType_list.ShowMessage() %>
<br />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<% If SiteParameterType.Export = "" Then %>
<div class="ewGridUpperPanel">
<% If SiteParameterType.CurrentAction <> "gridadd" AndAlso SiteParameterType.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If SiteParameterType_list.Pager Is Nothing Then SiteParameterType_list.Pager = New cPrevNextPager(SiteParameterType_list.lStartRec, SiteParameterType_list.lDisplayRecs, SiteParameterType_list.lTotalRecs) %>
<% If SiteParameterType_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If SiteParameterType_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= SiteParameterType_list.PageUrl %>start=<%= SiteParameterType_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If SiteParameterType_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= SiteParameterType_list.PageUrl %>start=<%= SiteParameterType_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= SiteParameterType_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If SiteParameterType_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= SiteParameterType_list.PageUrl %>start=<%= SiteParameterType_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If SiteParameterType_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= SiteParameterType_list.PageUrl %>start=<%= SiteParameterType_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= SiteParameterType_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= SiteParameterType_list.Pager.FromIndex %> to <%= SiteParameterType_list.Pager.ToIndex %> of <%= SiteParameterType_list.Pager.RecordCount %></span>
<% Else %>
	<% If SiteParameterType_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If SiteParameterType_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="SiteParameterType" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If SiteParameterType_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If SiteParameterType_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If SiteParameterType_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If SiteParameterType.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<div class="aspnetmaker">
<% If SiteParameterType.CurrentAction <> "gridadd" AndAlso SiteParameterType.CurrentAction <> "gridedit" Then ' Not grid add/edit mode %>
<a href="<%= SiteParameterType.AddUrl %>">Add</a>&nbsp;&nbsp;
<a href="<%= SiteParameterType_list.PageUrl %>a=gridadd">Grid Add</a>&nbsp;&nbsp;
<% If SiteParameterType_list.lTotalRecs > 0 Then %>
<a href="<%= SiteParameterType_list.PageUrl %>a=gridedit">Grid Edit</a>&nbsp;&nbsp;
<% End If %>
<% Else ' Grid add/edit mode %>
<% If SiteParameterType.CurrentAction = "gridadd" Then %>
<a href="" onclick="f=ew_GetForm('fSiteParameterTypelist');if (SiteParameterType_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/insert.gif' alt='Insert' title='Insert' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<% If SiteParameterType.CurrentAction = "gridedit" Then %>
<a href="" onclick="f=ew_GetForm('fSiteParameterTypelist');if (SiteParameterType_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/update.gif' alt='Save' title='Save' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<a href="<%= SiteParameterType_list.PageUrl %>a=cancel"><img src='images/cancel.gif' alt='Cancel' title='Cancel' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
</div>
</div>
<% End If %>
<div class="ewGridMiddlePanel">
<form name="fSiteParameterTypelist" id="fSiteParameterTypelist" class="ewForm" method="post">
<input type="hidden" name="t" id="t" value="SiteParameterType" />
<% If SiteParameterType_list.lTotalRecs > 0 Then %>
<table cellspacing="0" rowhighlightclass="ewTableHighlightRow" rowselectclass="ewTableSelectRow" roweditclass="ewTableEditRow" class="ewTable ewTableSeparate">
<%
	SiteParameterType_list.lOptionCnt = 0
	SiteParameterType_list.lOptionCnt = SiteParameterType_list.lOptionCnt + 1 ' View
	SiteParameterType_list.lOptionCnt = SiteParameterType_list.lOptionCnt + 1 ' Edit
	SiteParameterType_list.lOptionCnt = SiteParameterType_list.lOptionCnt + 1 ' Copy
	SiteParameterType_list.lOptionCnt = SiteParameterType_list.lOptionCnt + 1 ' Delete
	SiteParameterType_list.lOptionCnt = SiteParameterType_list.lOptionCnt + 1 ' Detail
	SiteParameterType_list.lOptionCnt = SiteParameterType_list.lOptionCnt + 1 ' Detail
	SiteParameterType_list.lOptionCnt = SiteParameterType_list.lOptionCnt + SiteParameterType_list.ListOptions.Items.Count ' Custom list options
%>
<%= SiteParameterType.TableCustomInnerHTML %>
<thead><!-- Table header -->
	<tr class="ewTableHeader">
<% If SiteParameterType.Export = "" Then %>
<% If SiteParameterType.CurrentAction <> "gridadd" AndAlso SiteParameterType.CurrentAction <> "gridedit" Then %>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<%

' Custom list options
For i As Integer = 0 to SiteParameterType_list.ListOptions.Items.Count -1
	If SiteParameterType_list.ListOptions.Items(i).Visible Then Response.Write(SiteParameterType_list.ListOptions.Items(i).HeaderCellHtml)
Next
%>
<% End If %>
<% End If %>
<% If SiteParameterType.SiteParameterTypeNM.Visible Then ' SiteParameterTypeNM %>
	<% If SiteParameterType.SortUrl(SiteParameterType.SiteParameterTypeNM) = "" Then %>
		<td>Parameter Name</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteParameterType.SortUrl(SiteParameterType.SiteParameterTypeNM) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Parameter Name&nbsp;(*)</td><td style="width: 10px;"><% If SiteParameterType.SiteParameterTypeNM.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteParameterType.SiteParameterTypeNM.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If SiteParameterType.SiteParameterTypeDS.Visible Then ' SiteParameterTypeDS %>
	<% If SiteParameterType.SortUrl(SiteParameterType.SiteParameterTypeDS) = "" Then %>
		<td>Parameter Description</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteParameterType.SortUrl(SiteParameterType.SiteParameterTypeDS) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Parameter Description&nbsp;(*)</td><td style="width: 10px;"><% If SiteParameterType.SiteParameterTypeDS.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteParameterType.SiteParameterTypeDS.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If SiteParameterType.SiteParameterTypeOrder.Visible Then ' SiteParameterTypeOrder %>
	<% If SiteParameterType.SortUrl(SiteParameterType.SiteParameterTypeOrder) = "" Then %>
		<td>Parameter Order</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteParameterType.SortUrl(SiteParameterType.SiteParameterTypeOrder) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Parameter Order</td><td style="width: 10px;"><% If SiteParameterType.SiteParameterTypeOrder.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteParameterType.SiteParameterTypeOrder.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
	</tr>
</thead>
<tbody><!-- Table body -->
<%
If (SiteParameterType.ExportAll AndAlso SiteParameterType.Export <> "") Then
	SiteParameterType_list.lStopRec = SiteParameterType_list.lTotalRecs
Else
	SiteParameterType_list.lStopRec = SiteParameterType_list.lStartRec + SiteParameterType_list.lDisplayRecs - 1 ' Set the last record to display
End If
If SiteParameterType.CurrentAction = "gridadd" AndAlso SiteParameterType_list.lStopRec = -1 Then
	SiteParameterType_list.lStopRec = EW_GRIDADD_ROWS
End If 

' Move to first record
For i As Integer = 1 to SiteParameterType_list.lStartRec - 1
	If Rs.Read() Then	SiteParameterType_list.lRecCnt = SiteParameterType_list.lRecCnt + 1
Next		
SiteParameterType_list.lRowCnt = 0
If SiteParameterType.CurrentAction = "gridadd" Then SiteParameterType_list.lRowIndex = 0
If SiteParameterType.CurrentAction = "gridedit" Then SiteParameterType_list.lRowIndex = 0

' Output data rows
Do While (SiteParameterType.CurrentAction = "gridadd" OrElse Rs.Read()) AndAlso (SiteParameterType_list.lRecCnt < SiteParameterType_list.lStopRec)
	SiteParameterType_list.lRecCnt = SiteParameterType_list.lRecCnt + 1
	If SiteParameterType_list.lRecCnt >= SiteParameterType_list.lStartRec Then
		SiteParameterType_list.lRowCnt = SiteParameterType_list.lRowCnt + 1
		If SiteParameterType.CurrentAction = "gridadd" OrElse SiteParameterType.CurrentAction = "gridedit" Then SiteParameterType_list.lRowIndex = SiteParameterType_list.lRowIndex + 1
	SiteParameterType.CssClass = ""
	SiteParameterType.CssStyle = ""
	SiteParameterType.RowClientEvents = "onmouseover='ew_MouseOver(event, this);' onmouseout='ew_MouseOut(event, this);' onclick='ew_Click(event, this);'"
	If SiteParameterType.CurrentAction = "gridadd" Then
		SiteParameterType_list.LoadDefaultValues() ' Load default values
	Else
		SiteParameterType_list.LoadRowValues(Rs) ' Load row values
	End If
	SiteParameterType.RowType = EW_ROWTYPE_VIEW ' Render view
	If SiteParameterType.CurrentAction = "gridadd" Then ' Grid add
		SiteParameterType.RowType = EW_ROWTYPE_ADD ' Render add
	End If
	If SiteParameterType.CurrentAction = "gridadd" AndAlso SiteParameterType.EventCancelled Then ' Insert failed
		SiteParameterType_list.RestoreCurrentRowFormValues(SiteParameterType_list.lRowIndex) ' Restore form values
	End If
	If SiteParameterType.CurrentAction = "gridedit" Then ' Grid edit
		SiteParameterType.RowType = EW_ROWTYPE_EDIT ' Render edit
	End If
	If SiteParameterType.RowType = EW_ROWTYPE_EDIT AndAlso SiteParameterType.EventCancelled Then ' update failed
		If SiteParameterType.CurrentAction = "gridedit" Then
			SiteParameterType_list.RestoreCurrentRowFormValues(SiteParameterType_list.lRowIndex) ' Restore form values
		End If
	End If
	If SiteParameterType.RowType = EW_ROWTYPE_EDIT Then ' Edit row
		SiteParameterType_list.lEditRowCnt = SiteParameterType_list.lEditRowCnt + 1
		SiteParameterType.RowClientEvents = "onmouseover='this.edit=true;ew_MouseOver(event, this);' onmouseout='ew_MouseOut(event, this);' onclick='ew_Click(event, this);'"
	End If
	If SiteParameterType.RowType = EW_ROWTYPE_ADD OrElse SiteParameterType.RowType = EW_ROWTYPE_EDIT Then ' Add / Edit row
		SiteParameterType.CssClass = "ewTableEditRow"
	End If

	' Render row
	SiteParameterType_list.RenderRow()
%>
	<tr<%= SiteParameterType.RowAttributes %>>
<% If SiteParameterType.RowType = EW_ROWTYPE_ADD OrElse SiteParameterType.RowType = EW_ROWTYPE_EDIT Then %>
<%
	If SiteParameterType.CurrentAction = "gridedit" Then
		SiteParameterType_list.sMultiSelectKey = SiteParameterType_list.sMultiSelectKey & "<input type=""hidden"" name=""k" & SiteParameterType_list.lRowIndex & "_key"" id=""k" & SiteParameterType_list.lRowIndex & "_key"" value=""" & SiteParameterType.SiteParameterTypeID.CurrentValue & """ />"
	End If
%>
<% Else %>
<% If SiteParameterType.Export = "" Then %>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= SiteParameterType.ViewUrl %>"><img src='images/view.gif' alt='View' title='View' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= SiteParameterType.EditUrl %>"><img src='images/edit.gif' alt='Edit' title='Edit' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= SiteParameterType.CopyUrl %>"><img src='images/copy.gif' alt='Copy' title='Copy' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= SiteParameterType.DeleteUrl %>"><img src='images/delete.gif' alt='Delete' title='Delete' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<%
sSqlWrk = "[SiteParameterTypeID]=" & ew_AdjustSql(SiteParameterType.SiteParameterTypeID.CurrentValue) & ""
sSqlWrk = cTEA.Encrypt(sSqlWrk, EW_RANDOM_KEY)
sSqlWrk = sSqlWrk.Replace("'", "\'")
%>
<a name="ew_SiteParameterType_CompanySiteParameter_DetailLink<%= SiteParameterType_list.lRowCnt %>" id="ew_SiteParameterType_CompanySiteParameter_DetailLink<%= SiteParameterType_list.lRowCnt %>" href="CompanySiteParameter_list.aspx?<%= EW_TABLE_SHOW_MASTER %>=SiteParameterType&SiteParameterTypeID=<%= Server.UrlEncode(Convert.ToString(SiteParameterType.SiteParameterTypeID.CurrentValue)) %>" onmouseover="ew_AjaxShowDetails(this, 'CompanySiteParameter_preview.aspx?f=<%= sSqlWrk %>')" onmouseout="ew_AjaxHideDetails(this);">Site Parameter<img src='images/detail.gif' alt='Details' title='Details' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<%
sSqlWrk = "[SiteCategoryTypeID]=" & ew_AdjustSql(SiteParameterType.SiteParameterTypeID.CurrentValue) & ""
sSqlWrk = cTEA.Encrypt(sSqlWrk, EW_RANDOM_KEY)
sSqlWrk = sSqlWrk.Replace("'", "\'")
%>
<a name="ew_SiteParameterType_CompanySiteTypeParameter_DetailLink<%= SiteParameterType_list.lRowCnt %>" id="ew_SiteParameterType_CompanySiteTypeParameter_DetailLink<%= SiteParameterType_list.lRowCnt %>" href="CompanySiteTypeParameter_list.aspx?<%= EW_TABLE_SHOW_MASTER %>=SiteParameterType&SiteParameterTypeID=<%= Server.UrlEncode(Convert.ToString(SiteParameterType.SiteParameterTypeID.CurrentValue)) %>" onmouseover="ew_AjaxShowDetails(this, 'CompanySiteTypeParameter_preview.aspx?f=<%= sSqlWrk %>')" onmouseout="ew_AjaxHideDetails(this);">Site Type Parameter<img src='images/detail.gif' alt='Details' title='Details' width='16' height='16' border='0'></a>
</span></td>
<%

' Custom list options
For i As Integer = 0 to SiteParameterType_list.ListOptions.Items.Count -1
	If SiteParameterType_list.ListOptions.Items(i).Visible Then Response.Write(SiteParameterType_list.ListOptions.Items(i).BodyCellHtml)
Next
%>
<% End If %>
<% End If %>
	<% If SiteParameterType.SiteParameterTypeNM.Visible Then ' SiteParameterTypeNM %>
		<td<%= SiteParameterType.SiteParameterTypeNM.CellAttributes %>>
<% If SiteParameterType.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<input type="text" name="x<%= SiteParameterType_list.lRowIndex %>_SiteParameterTypeNM" id="x<%= SiteParameterType_list.lRowIndex %>_SiteParameterTypeNM" size="50" maxlength="255" value="<%= SiteParameterType.SiteParameterTypeNM.EditValue %>"<%= SiteParameterType.SiteParameterTypeNM.EditAttributes %> />
<input type="hidden" name="o<%= SiteParameterType_list.lRowIndex %>_SiteParameterTypeNM" id="o<%= SiteParameterType_list.lRowIndex %>_SiteParameterTypeNM" value="<%= ew_HTMLEncode(SiteParameterType.SiteParameterTypeNM.OldValue) %>" />
<% End If %>
<% If SiteParameterType.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<input type="text" name="x<%= SiteParameterType_list.lRowIndex %>_SiteParameterTypeNM" id="x<%= SiteParameterType_list.lRowIndex %>_SiteParameterTypeNM" size="50" maxlength="255" value="<%= SiteParameterType.SiteParameterTypeNM.EditValue %>"<%= SiteParameterType.SiteParameterTypeNM.EditAttributes %> />
<% End If %>
<% If SiteParameterType.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= SiteParameterType.SiteParameterTypeNM.ViewAttributes %>><%= SiteParameterType.SiteParameterTypeNM.ListViewValue %></div>
<% End If %>
<% If SiteParameterType.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<input type="hidden" name="o<%= SiteParameterType_list.lRowIndex %>_SiteParameterTypeID" id="o<%= SiteParameterType_list.lRowIndex %>_SiteParameterTypeID" value="<%= ew_HTMLEncode(SiteParameterType.SiteParameterTypeID.OldValue) %>" />
<% End If %>
<% If SiteParameterType.RowType = EW_ROWTYPE_EDIT Then %>
<input type="hidden" name="x<%= SiteParameterType_list.lRowIndex %>_SiteParameterTypeID" id="x<%= SiteParameterType_list.lRowIndex %>_SiteParameterTypeID" value="<%= ew_HTMLEncode(SiteParameterType.SiteParameterTypeID.CurrentValue) %>" />
<% End If %>
</td>
	<% End If %>
	<% If SiteParameterType.SiteParameterTypeDS.Visible Then ' SiteParameterTypeDS %>
		<td<%= SiteParameterType.SiteParameterTypeDS.CellAttributes %>>
<% If SiteParameterType.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<input type="text" name="x<%= SiteParameterType_list.lRowIndex %>_SiteParameterTypeDS" id="x<%= SiteParameterType_list.lRowIndex %>_SiteParameterTypeDS" size="50" maxlength="255" value="<%= SiteParameterType.SiteParameterTypeDS.EditValue %>"<%= SiteParameterType.SiteParameterTypeDS.EditAttributes %> />
<input type="hidden" name="o<%= SiteParameterType_list.lRowIndex %>_SiteParameterTypeDS" id="o<%= SiteParameterType_list.lRowIndex %>_SiteParameterTypeDS" value="<%= ew_HTMLEncode(SiteParameterType.SiteParameterTypeDS.OldValue) %>" />
<% End If %>
<% If SiteParameterType.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<input type="text" name="x<%= SiteParameterType_list.lRowIndex %>_SiteParameterTypeDS" id="x<%= SiteParameterType_list.lRowIndex %>_SiteParameterTypeDS" size="50" maxlength="255" value="<%= SiteParameterType.SiteParameterTypeDS.EditValue %>"<%= SiteParameterType.SiteParameterTypeDS.EditAttributes %> />
<% End If %>
<% If SiteParameterType.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= SiteParameterType.SiteParameterTypeDS.ViewAttributes %>><%= SiteParameterType.SiteParameterTypeDS.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	<% If SiteParameterType.SiteParameterTypeOrder.Visible Then ' SiteParameterTypeOrder %>
		<td<%= SiteParameterType.SiteParameterTypeOrder.CellAttributes %>>
<% If SiteParameterType.RowType = EW_ROWTYPE_ADD Then ' Add Record %>
<input type="text" name="x<%= SiteParameterType_list.lRowIndex %>_SiteParameterTypeOrder" id="x<%= SiteParameterType_list.lRowIndex %>_SiteParameterTypeOrder" size="30" value="<%= SiteParameterType.SiteParameterTypeOrder.EditValue %>"<%= SiteParameterType.SiteParameterTypeOrder.EditAttributes %> />
<input type="hidden" name="o<%= SiteParameterType_list.lRowIndex %>_SiteParameterTypeOrder" id="o<%= SiteParameterType_list.lRowIndex %>_SiteParameterTypeOrder" value="<%= ew_HTMLEncode(SiteParameterType.SiteParameterTypeOrder.OldValue) %>" />
<% End If %>
<% If SiteParameterType.RowType = EW_ROWTYPE_EDIT Then ' Edit Record %>
<input type="text" name="x<%= SiteParameterType_list.lRowIndex %>_SiteParameterTypeOrder" id="x<%= SiteParameterType_list.lRowIndex %>_SiteParameterTypeOrder" size="30" value="<%= SiteParameterType.SiteParameterTypeOrder.EditValue %>"<%= SiteParameterType.SiteParameterTypeOrder.EditAttributes %> />
<% End If %>
<% If SiteParameterType.RowType = EW_ROWTYPE_VIEW Then ' View Record %>
<div<%= SiteParameterType.SiteParameterTypeOrder.ViewAttributes %>><%= SiteParameterType.SiteParameterTypeOrder.ListViewValue %></div>
<% End If %>
</td>
	<% End If %>
	</tr>
<% If SiteParameterType.RowType = EW_ROWTYPE_ADD Then %>
<% End If %>
<% If SiteParameterType.RowType = EW_ROWTYPE_EDIT Then %>
<% End If %>
<%
	End If
Loop
%>
</tbody>
</table>
<% End If %>
<% If SiteParameterType.CurrentAction = "gridadd" Then %>
<input type="hidden" name="a_list" id="a_list" value="gridinsert" />
<input type="hidden" name="key_count" id="key_count" value="<%= SiteParameterType_list.lRowIndex %>" />
<% End If %>
<% If SiteParameterType.CurrentAction = "gridedit" Then %>
<input type="hidden" name="a_list" id="a_list" value="gridupdate" />
<input type="hidden" name="key_count" id="key_count" value="<%= SiteParameterType_list.lRowIndex %>" />
<%= SiteParameterType_list.sMultiSelectKey %>
<% End If %>
</form>
<%

' Close recordset
Rs.Close()
Rs.Dispose()
%>
</div>
<% If SiteParameterType_list.lTotalRecs > 0 Then %>
<% If SiteParameterType.Export = "" Then %>
<div class="ewGridLowerPanel">
<% If SiteParameterType.CurrentAction <> "gridadd" AndAlso SiteParameterType.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If SiteParameterType_list.Pager Is Nothing Then SiteParameterType_list.Pager = New cPrevNextPager(SiteParameterType_list.lStartRec, SiteParameterType_list.lDisplayRecs, SiteParameterType_list.lTotalRecs) %>
<% If SiteParameterType_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If SiteParameterType_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= SiteParameterType_list.PageUrl %>start=<%= SiteParameterType_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If SiteParameterType_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= SiteParameterType_list.PageUrl %>start=<%= SiteParameterType_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= SiteParameterType_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If SiteParameterType_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= SiteParameterType_list.PageUrl %>start=<%= SiteParameterType_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If SiteParameterType_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= SiteParameterType_list.PageUrl %>start=<%= SiteParameterType_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= SiteParameterType_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= SiteParameterType_list.Pager.FromIndex %> to <%= SiteParameterType_list.Pager.ToIndex %> of <%= SiteParameterType_list.Pager.RecordCount %></span>
<% Else %>
	<% If SiteParameterType_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If SiteParameterType_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="SiteParameterType" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If SiteParameterType_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If SiteParameterType_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If SiteParameterType_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If SiteParameterType.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<% 'If SiteParameterType_list.lTotalRecs > 0 Then %>
<div class="aspnetmaker">
<% If SiteParameterType.CurrentAction <> "gridadd" AndAlso SiteParameterType.CurrentAction <> "gridedit" Then ' Not grid add/edit mode %>
<a href="<%= SiteParameterType.AddUrl %>">Add</a>&nbsp;&nbsp;
<a href="<%= SiteParameterType_list.PageUrl %>a=gridadd">Grid Add</a>&nbsp;&nbsp;
<% If SiteParameterType_list.lTotalRecs > 0 Then %>
<a href="<%= SiteParameterType_list.PageUrl %>a=gridedit">Grid Edit</a>&nbsp;&nbsp;
<% End If %>
<% Else ' Grid add/edit mode %>
<% If SiteParameterType.CurrentAction = "gridadd" Then %>
<a href="" onclick="f=ew_GetForm('fSiteParameterTypelist');if (SiteParameterType_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/insert.gif' alt='Insert' title='Insert' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<% If SiteParameterType.CurrentAction = "gridedit" Then %>
<a href="" onclick="f=ew_GetForm('fSiteParameterTypelist');if (SiteParameterType_list.ValidateForm(f)) { f.action=location.pathname;f.submit(); }return false;"><img src='images/update.gif' alt='Save' title='Save' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
<a href="<%= SiteParameterType_list.PageUrl %>a=cancel"><img src='images/cancel.gif' alt='Cancel' title='Cancel' width='16' height='16' border='0'></a>&nbsp;&nbsp;
<% End If %>
</div>
<% 'End If %>
</div>
<% End If %>
<% End If %>
</td></tr></table>
<% If SiteParameterType.Export = "" AndAlso SiteParameterType.CurrentAction = "" Then %>
<script type="text/javascript">
<!--
//ew_ToggleSearchPanel(SiteParameterType_list); // uncomment to init search panel as collapsed
//-->
</script>
<% End If %>
<% If SiteParameterType.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
