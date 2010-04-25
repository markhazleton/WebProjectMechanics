<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="LinkType_list.aspx.vb" Inherits="LinkType_list" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If LinkType.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var LinkType_list = new ew_Page("LinkType_list");
// page properties
LinkType_list.PageID = "list"; // page ID
var EW_PAGE_ID = LinkType_list.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
LinkType_list.ValidateRequired = true; // uses JavaScript validation
<% Else %>
LinkType_list.ValidateRequired = false; // no JavaScript validation
<% End If %>
//-->
</script>
<div id="ewDetailsDiv" name="ewDetailsDivDiv" style="visibility:hidden"></div>
<script language="JavaScript" type="text/javascript">
<!--
// YUI container
var ewDetailsDiv;
var ew_AjaxDetailsTimer = null;
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
<% If LinkType.Export = "" Then %>
<% End If %>
<%

' Load recordset
Rs = LinkType_list.LoadRecordset()
	LinkType_list.lStartRec = 1
	If LinkType_list.lDisplayRecs <= 0 Then ' Display all records
		LinkType_list.lDisplayRecs = LinkType_list.lTotalRecs
	End If
	If Not (LinkType.ExportAll AndAlso LinkType.Export <> "") Then
		LinkType_list.SetUpStartRec() ' Set up start record position
	End If
%>
<p><span class="aspnetmaker" style="white-space: nowrap;">TABLE: Part Type
<% If LinkType.Export = "" AndAlso LinkType.CurrentAction = "" Then %>
&nbsp;&nbsp;<a href="<%= LinkType_list.PageUrl %>export=excel"><img src='images/exportxls.gif' alt='Export to Excel' title='Export to Excel' width='16' height='16' border='0'></a>
<% End If %>
</span></p>
<% If LinkType.Export = "" AndAlso LinkType.CurrentAction = "" Then %>
<a href="javascript:ew_ToggleSearchPanel(LinkType_list);" style="text-decoration: none;"><img id="LinkType_list_SearchImage" src="images/collapse.gif" alt="" width="9" height="9" border="0"></a><span class="aspnetmaker">&nbsp;Search</span><br>
<div id="LinkType_list_SearchPanel">
<form name="fLinkTypelistsrch" id="fLinkTypelistsrch" class="ewForm">
<input type="hidden" id="t" name="t" value="LinkType" />
<table class="ewBasicSearch">
	<tr>
		<td><span class="aspnetmaker">
			<input type="text" name="<%= EW_TABLE_BASIC_SEARCH %>" id="<%= EW_TABLE_BASIC_SEARCH %>" size="20" value="<%= ew_HtmlEncode(LinkType.BasicSearchKeyword) %>" />
			<input type="Submit" name="Submit" id="Submit" value="Search (*)" />&nbsp;
			<a href="<%= LinkType_list.PageUrl %>cmd=reset">Show all</a>&nbsp;
			<a href="LinkType_srch.aspx">Advanced Search</a>&nbsp;
		</span></td>
	</tr>
	<tr>
	<td><span class="aspnetmaker"><label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value=""<% If LinkType.BasicSearchType = "" Then %> checked="checked"<% End If %> />Exact phrase</label>&nbsp;&nbsp;<label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value="AND"<% If LinkType.BasicSearchType = "AND" Then %> checked="checked"<% End If %> />All words</label>&nbsp;&nbsp;<label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value="OR"<% If LinkType.BasicSearchType = "OR" Then %> checked="checked"<% End If %> />Any word</label></span></td>
	</tr>
</table>
</form>
</div>
<% End If %>
<% LinkType_list.ShowMessage() %>
<br />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<% If LinkType.Export = "" Then %>
<div class="ewGridUpperPanel">
<% If LinkType.CurrentAction <> "gridadd" AndAlso LinkType.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If LinkType_list.Pager Is Nothing Then LinkType_list.Pager = New cPrevNextPager(LinkType_list.lStartRec, LinkType_list.lDisplayRecs, LinkType_list.lTotalRecs) %>
<% If LinkType_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If LinkType_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= LinkType_list.PageUrl %>start=<%= LinkType_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If LinkType_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= LinkType_list.PageUrl %>start=<%= LinkType_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= LinkType_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If LinkType_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= LinkType_list.PageUrl %>start=<%= LinkType_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If LinkType_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= LinkType_list.PageUrl %>start=<%= LinkType_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= LinkType_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= LinkType_list.Pager.FromIndex %> to <%= LinkType_list.Pager.ToIndex %> of <%= LinkType_list.Pager.RecordCount %></span>
<% Else %>
	<% If LinkType_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If LinkType_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="LinkType" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If LinkType_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If LinkType_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If LinkType_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If LinkType.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<div class="aspnetmaker">
<a href="<%= LinkType.AddUrl %>">Add</a>&nbsp;&nbsp;
</div>
</div>
<% End If %>
<div class="ewGridMiddlePanel">
<form name="fLinkTypelist" id="fLinkTypelist" class="ewForm" method="post">
<% If LinkType_list.lTotalRecs > 0 Then %>
<table cellspacing="0" rowhighlightclass="ewTableHighlightRow" rowselectclass="ewTableSelectRow" roweditclass="ewTableEditRow" class="ewTable ewTableSeparate">
<%
	LinkType_list.lOptionCnt = 0
	LinkType_list.lOptionCnt = LinkType_list.lOptionCnt + 1 ' View
	LinkType_list.lOptionCnt = LinkType_list.lOptionCnt + 1 ' Edit
	LinkType_list.lOptionCnt = LinkType_list.lOptionCnt + 1 ' Copy
	LinkType_list.lOptionCnt = LinkType_list.lOptionCnt + 1 ' Delete
	LinkType_list.lOptionCnt = LinkType_list.lOptionCnt + LinkType_list.ListOptions.Items.Count ' Custom list options
%>
<%= LinkType.TableCustomInnerHTML %>
<thead><!-- Table header -->
	<tr class="ewTableHeader">
<% If LinkType.Export = "" Then %>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<%

' Custom list options
For i As Integer = 0 to LinkType_list.ListOptions.Items.Count -1
	If LinkType_list.ListOptions.Items(i).Visible Then Response.Write(LinkType_list.ListOptions.Items(i).HeaderCellHtml)
Next
%>
<% End If %>
<% If LinkType.LinkTypeCD.Visible Then ' LinkTypeCD %>
	<% If LinkType.SortUrl(LinkType.LinkTypeCD) = "" Then %>
		<td>Link Type CD</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= LinkType.SortUrl(LinkType.LinkTypeCD) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Link Type CD&nbsp;(*)</td><td style="width: 10px;"><% If LinkType.LinkTypeCD.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf LinkType.LinkTypeCD.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If LinkType.LinkTypeDesc.Visible Then ' LinkTypeDesc %>
	<% If LinkType.SortUrl(LinkType.LinkTypeDesc) = "" Then %>
		<td>Link Type Desc</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= LinkType.SortUrl(LinkType.LinkTypeDesc) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Link Type Desc&nbsp;(*)</td><td style="width: 10px;"><% If LinkType.LinkTypeDesc.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf LinkType.LinkTypeDesc.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If LinkType.LinkTypeTarget.Visible Then ' LinkTypeTarget %>
	<% If LinkType.SortUrl(LinkType.LinkTypeTarget) = "" Then %>
		<td>Link Type Target</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= LinkType.SortUrl(LinkType.LinkTypeTarget) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Link Type Target&nbsp;(*)</td><td style="width: 10px;"><% If LinkType.LinkTypeTarget.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf LinkType.LinkTypeTarget.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
	</tr>
</thead>
<tbody><!-- Table body -->
<%
If (LinkType.ExportAll AndAlso LinkType.Export <> "") Then
	LinkType_list.lStopRec = LinkType_list.lTotalRecs
Else
	LinkType_list.lStopRec = LinkType_list.lStartRec + LinkType_list.lDisplayRecs - 1 ' Set the last record to display
End If
If LinkType.CurrentAction = "gridadd" AndAlso LinkType_list.lStopRec = -1 Then
	LinkType_list.lStopRec = EW_GRIDADD_ROWS
End If 

' Move to first record
For i As Integer = 1 to LinkType_list.lStartRec - 1
	If Rs.Read() Then	LinkType_list.lRecCnt = LinkType_list.lRecCnt + 1
Next		
LinkType_list.lRowCnt = 0

' Output data rows
Do While (LinkType.CurrentAction = "gridadd" OrElse Rs.Read()) AndAlso (LinkType_list.lRecCnt < LinkType_list.lStopRec)
	LinkType_list.lRecCnt = LinkType_list.lRecCnt + 1
	If LinkType_list.lRecCnt >= LinkType_list.lStartRec Then
		LinkType_list.lRowCnt = LinkType_list.lRowCnt + 1
	LinkType.CssClass = ""
	LinkType.CssStyle = ""
	LinkType.RowClientEvents = "onmouseover='ew_MouseOver(event, this);' onmouseout='ew_MouseOut(event, this);' onclick='ew_Click(event, this);'"
	If LinkType.CurrentAction = "gridadd" Then
		LinkType_list.LoadDefaultValues() ' Load default values
	Else
		LinkType_list.LoadRowValues(Rs) ' Load row values
	End If
	LinkType.RowType = EW_ROWTYPE_VIEW ' Render view

	' Render row
	LinkType_list.RenderRow()
%>
	<tr<%= LinkType.RowAttributes %>>
<% If LinkType.Export = "" Then %>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= LinkType.ViewUrl %>"><img src='images/view.gif' alt='View' title='View' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= LinkType.EditUrl %>"><img src='images/edit.gif' alt='Edit' title='Edit' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= LinkType.CopyUrl %>"><img src='images/copy.gif' alt='Copy' title='Copy' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= LinkType.DeleteUrl %>"><img src='images/delete.gif' alt='Delete' title='Delete' width='16' height='16' border='0'></a>
</span></td>
<%

' Custom list options
For i As Integer = 0 to LinkType_list.ListOptions.Items.Count -1
	If LinkType_list.ListOptions.Items(i).Visible Then Response.Write(LinkType_list.ListOptions.Items(i).BodyCellHtml)
Next
%>
<% End If %>
	<% If LinkType.LinkTypeCD.Visible Then ' LinkTypeCD %>
		<td<%= LinkType.LinkTypeCD.CellAttributes %>>
<div<%= LinkType.LinkTypeCD.ViewAttributes %>><%= LinkType.LinkTypeCD.ListViewValue %></div>
</td>
	<% End If %>
	<% If LinkType.LinkTypeDesc.Visible Then ' LinkTypeDesc %>
		<td<%= LinkType.LinkTypeDesc.CellAttributes %>>
<div<%= LinkType.LinkTypeDesc.ViewAttributes %>><%= LinkType.LinkTypeDesc.ListViewValue %></div>
</td>
	<% End If %>
	<% If LinkType.LinkTypeTarget.Visible Then ' LinkTypeTarget %>
		<td<%= LinkType.LinkTypeTarget.CellAttributes %>>
<div<%= LinkType.LinkTypeTarget.ViewAttributes %>><%= LinkType.LinkTypeTarget.ListViewValue %></div>
</td>
	<% End If %>
	</tr>
<%
	End If
Loop
%>
</tbody>
</table>
<% End If %>
</form>
<%

' Close recordset
Rs.Close()
Rs.Dispose()
%>
</div>
<% If LinkType_list.lTotalRecs > 0 Then %>
<% If LinkType.Export = "" Then %>
<div class="ewGridLowerPanel">
<% If LinkType.CurrentAction <> "gridadd" AndAlso LinkType.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If LinkType_list.Pager Is Nothing Then LinkType_list.Pager = New cPrevNextPager(LinkType_list.lStartRec, LinkType_list.lDisplayRecs, LinkType_list.lTotalRecs) %>
<% If LinkType_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If LinkType_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= LinkType_list.PageUrl %>start=<%= LinkType_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If LinkType_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= LinkType_list.PageUrl %>start=<%= LinkType_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= LinkType_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If LinkType_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= LinkType_list.PageUrl %>start=<%= LinkType_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If LinkType_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= LinkType_list.PageUrl %>start=<%= LinkType_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= LinkType_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= LinkType_list.Pager.FromIndex %> to <%= LinkType_list.Pager.ToIndex %> of <%= LinkType_list.Pager.RecordCount %></span>
<% Else %>
	<% If LinkType_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If LinkType_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="LinkType" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If LinkType_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If LinkType_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If LinkType_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If LinkType.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<% 'If LinkType_list.lTotalRecs > 0 Then %>
<div class="aspnetmaker">
<a href="<%= LinkType.AddUrl %>">Add</a>&nbsp;&nbsp;
</div>
<% 'End If %>
</div>
<% End If %>
<% End If %>
</td></tr></table>
<% If LinkType.Export = "" AndAlso LinkType.CurrentAction = "" Then %>
<script type="text/javascript">
<!--
//ew_ToggleSearchPanel(LinkType_list); // uncomment to init search panel as collapsed
//-->
</script>
<% End If %>
<% If LinkType.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
