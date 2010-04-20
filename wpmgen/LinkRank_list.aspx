<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="LinkRank_list.aspx.vb" Inherits="LinkRank_list" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If LinkRank.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var LinkRank_list = new ew_Page("LinkRank_list");
// page properties
LinkRank_list.PageID = "list"; // page ID
var EW_PAGE_ID = LinkRank_list.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
LinkRank_list.ValidateRequired = true; // uses JavaScript validation
<% Else %>
LinkRank_list.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker" style="white-space: nowrap;">TABLE: Part Rank
<% If LinkRank.Export = "" AndAlso LinkRank.CurrentAction = "" Then %>
&nbsp;&nbsp;<a href="<%= LinkRank_list.PageUrl %>export=excel"><img src='images/exportxls.gif' alt='Export to Excel' title='Export to Excel' width='16' height='16' border='0'></a>
<% End If %>
</span></p>
<% If LinkRank.Export = "" AndAlso LinkRank.CurrentAction = "" Then %>
<a href="javascript:ew_ToggleSearchPanel(LinkRank_list);" style="text-decoration: none;"><img id="LinkRank_list_SearchImage" src="images/collapse.gif" alt="" width="9" height="9" border="0"></a><span class="aspnetmaker">&nbsp;Search</span><br>
<div id="LinkRank_list_SearchPanel">
<form name="fLinkRanklistsrch" id="fLinkRanklistsrch" class="ewForm">
<input type="hidden" id="t" name="t" value="LinkRank" />
<table class="ewBasicSearch">
	<tr>
		<td><span class="aspnetmaker">
			<input type="text" name="<%= EW_TABLE_BASIC_SEARCH %>" id="<%= EW_TABLE_BASIC_SEARCH %>" size="20" value="<%= ew_HtmlEncode(LinkRank.BasicSearchKeyword) %>" />
			<input type="Submit" name="Submit" id="Submit" value="Search (*)" />&nbsp;
			<a href="<%= LinkRank_list.PageUrl %>cmd=reset">Show all</a>&nbsp;
			<a href="LinkRank_srch.aspx">Advanced Search</a>&nbsp;
		</span></td>
	</tr>
	<tr>
	<td><span class="aspnetmaker"><label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value=""<% If LinkRank.BasicSearchType = "" Then %> checked="checked"<% End If %> />Exact phrase</label>&nbsp;&nbsp;<label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value="AND"<% If LinkRank.BasicSearchType = "AND" Then %> checked="checked"<% End If %> />All words</label>&nbsp;&nbsp;<label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value="OR"<% If LinkRank.BasicSearchType = "OR" Then %> checked="checked"<% End If %> />Any word</label></span></td>
	</tr>
</table>
</form>
</div>
<% End If %>
<% LinkRank_list.ShowMessage() %>
<br />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<% If LinkRank.Export = "" Then %>
<div class="ewGridUpperPanel">
<% If LinkRank.CurrentAction <> "gridadd" AndAlso LinkRank.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If LinkRank_list.Pager Is Nothing Then LinkRank_list.Pager = New cPrevNextPager(LinkRank_list.lStartRec, LinkRank_list.lDisplayRecs, LinkRank_list.lTotalRecs) %>
<% If LinkRank_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If LinkRank_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= LinkRank_list.PageUrl %>start=<%= LinkRank_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If LinkRank_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= LinkRank_list.PageUrl %>start=<%= LinkRank_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= LinkRank_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If LinkRank_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= LinkRank_list.PageUrl %>start=<%= LinkRank_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If LinkRank_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= LinkRank_list.PageUrl %>start=<%= LinkRank_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= LinkRank_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= LinkRank_list.Pager.FromIndex %> to <%= LinkRank_list.Pager.ToIndex %> of <%= LinkRank_list.Pager.RecordCount %></span>
<% Else %>
	<% If LinkRank_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If LinkRank_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="LinkRank" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If LinkRank_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If LinkRank_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If LinkRank_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If LinkRank.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<div class="aspnetmaker">
<a href="<%= LinkRank.AddUrl %>">Add</a>&nbsp;&nbsp;
</div>
</div>
<% End If %>
<div class="ewGridMiddlePanel">
<form name="fLinkRanklist" id="fLinkRanklist" class="ewForm" method="post">
<% If LinkRank_list.lTotalRecs > 0 Then %>
<table cellspacing="0" rowhighlightclass="ewTableHighlightRow" rowselectclass="ewTableSelectRow" roweditclass="ewTableEditRow" class="ewTable ewTableSeparate">
<%
	LinkRank_list.lOptionCnt = 0
	LinkRank_list.lOptionCnt = LinkRank_list.lOptionCnt + 1 ' View
	LinkRank_list.lOptionCnt = LinkRank_list.lOptionCnt + 1 ' Edit
	LinkRank_list.lOptionCnt = LinkRank_list.lOptionCnt + 1 ' Copy
	LinkRank_list.lOptionCnt = LinkRank_list.lOptionCnt + 1 ' Delete
	LinkRank_list.lOptionCnt = LinkRank_list.lOptionCnt + LinkRank_list.ListOptions.Items.Count ' Custom list options
%>
<%= LinkRank.TableCustomInnerHTML %>
<thead><!-- Table header -->
	<tr class="ewTableHeader">
<% If LinkRank.Export = "" Then %>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<%

' Custom list options
For i As Integer = 0 to LinkRank_list.ListOptions.Items.Count -1
	If LinkRank_list.ListOptions.Items(i).Visible Then Response.Write(LinkRank_list.ListOptions.Items(i).HeaderCellHtml)
Next
%>
<% End If %>
<% If LinkRank.ID.Visible Then ' ID %>
	<% If LinkRank.SortUrl(LinkRank.ID) = "" Then %>
		<td>ID</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= LinkRank.SortUrl(LinkRank.ID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>ID</td><td style="width: 10px;"><% If LinkRank.ID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf LinkRank.ID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If LinkRank.LinkID.Visible Then ' LinkID %>
	<% If LinkRank.SortUrl(LinkRank.LinkID) = "" Then %>
		<td>Link ID</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= LinkRank.SortUrl(LinkRank.LinkID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Link ID</td><td style="width: 10px;"><% If LinkRank.LinkID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf LinkRank.LinkID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If LinkRank.UserID.Visible Then ' UserID %>
	<% If LinkRank.SortUrl(LinkRank.UserID) = "" Then %>
		<td>User ID</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= LinkRank.SortUrl(LinkRank.UserID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>User ID</td><td style="width: 10px;"><% If LinkRank.UserID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf LinkRank.UserID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If LinkRank.RankNum.Visible Then ' RankNum %>
	<% If LinkRank.SortUrl(LinkRank.RankNum) = "" Then %>
		<td>Rank Num</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= LinkRank.SortUrl(LinkRank.RankNum) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Rank Num</td><td style="width: 10px;"><% If LinkRank.RankNum.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf LinkRank.RankNum.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If LinkRank.CateID.Visible Then ' CateID %>
	<% If LinkRank.SortUrl(LinkRank.CateID) = "" Then %>
		<td>Cate ID</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= LinkRank.SortUrl(LinkRank.CateID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Cate ID</td><td style="width: 10px;"><% If LinkRank.CateID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf LinkRank.CateID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If LinkRank.Comment.Visible Then ' Comment %>
	<% If LinkRank.SortUrl(LinkRank.Comment) = "" Then %>
		<td>Comment</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= LinkRank.SortUrl(LinkRank.Comment) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Comment&nbsp;(*)</td><td style="width: 10px;"><% If LinkRank.Comment.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf LinkRank.Comment.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
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
LinkRank_list.lRowCnt = 0

' Output data rows
Do While (LinkRank.CurrentAction = "gridadd" OrElse Rs.Read()) AndAlso (LinkRank_list.lRecCnt < LinkRank_list.lStopRec)
	LinkRank_list.lRecCnt = LinkRank_list.lRecCnt + 1
	If LinkRank_list.lRecCnt >= LinkRank_list.lStartRec Then
		LinkRank_list.lRowCnt = LinkRank_list.lRowCnt + 1
	LinkRank.CssClass = ""
	LinkRank.CssStyle = ""
	LinkRank.RowClientEvents = "onmouseover='ew_MouseOver(event, this);' onmouseout='ew_MouseOut(event, this);' onclick='ew_Click(event, this);'"
	If LinkRank.CurrentAction = "gridadd" Then
		LinkRank_list.LoadDefaultValues() ' Load default values
	Else
		LinkRank_list.LoadRowValues(Rs) ' Load row values
	End If
	LinkRank.RowType = EW_ROWTYPE_VIEW ' Render view

	' Render row
	LinkRank_list.RenderRow()
%>
	<tr<%= LinkRank.RowAttributes %>>
<% If LinkRank.Export = "" Then %>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= LinkRank.ViewUrl %>"><img src='images/view.gif' alt='View' title='View' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= LinkRank.EditUrl %>"><img src='images/edit.gif' alt='Edit' title='Edit' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= LinkRank.CopyUrl %>"><img src='images/copy.gif' alt='Copy' title='Copy' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= LinkRank.DeleteUrl %>"><img src='images/delete.gif' alt='Delete' title='Delete' width='16' height='16' border='0'></a>
</span></td>
<%

' Custom list options
For i As Integer = 0 to LinkRank_list.ListOptions.Items.Count -1
	If LinkRank_list.ListOptions.Items(i).Visible Then Response.Write(LinkRank_list.ListOptions.Items(i).BodyCellHtml)
Next
%>
<% End If %>
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
<% If LinkRank_list.lTotalRecs > 0 Then %>
<% If LinkRank.Export = "" Then %>
<div class="ewGridLowerPanel">
<% If LinkRank.CurrentAction <> "gridadd" AndAlso LinkRank.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If LinkRank_list.Pager Is Nothing Then LinkRank_list.Pager = New cPrevNextPager(LinkRank_list.lStartRec, LinkRank_list.lDisplayRecs, LinkRank_list.lTotalRecs) %>
<% If LinkRank_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If LinkRank_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= LinkRank_list.PageUrl %>start=<%= LinkRank_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If LinkRank_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= LinkRank_list.PageUrl %>start=<%= LinkRank_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= LinkRank_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If LinkRank_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= LinkRank_list.PageUrl %>start=<%= LinkRank_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If LinkRank_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= LinkRank_list.PageUrl %>start=<%= LinkRank_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= LinkRank_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= LinkRank_list.Pager.FromIndex %> to <%= LinkRank_list.Pager.ToIndex %> of <%= LinkRank_list.Pager.RecordCount %></span>
<% Else %>
	<% If LinkRank_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If LinkRank_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="LinkRank" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If LinkRank_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If LinkRank_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If LinkRank_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If LinkRank.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<% 'If LinkRank_list.lTotalRecs > 0 Then %>
<div class="aspnetmaker">
<a href="<%= LinkRank.AddUrl %>">Add</a>&nbsp;&nbsp;
</div>
<% 'End If %>
</div>
<% End If %>
<% End If %>
</td></tr></table>
<% If LinkRank.Export = "" AndAlso LinkRank.CurrentAction = "" Then %>
<script type="text/javascript">
<!--
//ew_ToggleSearchPanel(LinkRank_list); // uncomment to init search panel as collapsed
//-->
</script>
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
