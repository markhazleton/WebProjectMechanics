<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="Article_list.aspx.vb" Inherits="Article_list" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If Article.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var Article_list = new ew_Page("Article_list");
// page properties
Article_list.PageID = "list"; // page ID
var EW_PAGE_ID = Article_list.PageID; // for backward compatibility
Article_list.SelectAllKey = function(elem) {
	ew_SelectAll(elem);
}
<% If EW_CLIENT_VALIDATE Then %>
Article_list.ValidateRequired = true; // uses JavaScript validation
<% Else %>
Article_list.ValidateRequired = false; // no JavaScript validation
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
<link rel="stylesheet" type="text/css" media="all" href="calendar/calendar-win2k-1.css" title="win2k-1" />
<script type="text/javascript" src="calendar/calendar.js"></script>
<script type="text/javascript" src="calendar/lang/calendar-en.js"></script>
<script type="text/javascript" src="calendar/calendar-setup.js"></script>
<script language="JavaScript" type="text/javascript">
<!--
// Write your client script here, no need to add script tags.
// To include another .js script, use:
// ew_ClientScriptInclude("my_javascript.js"); 
//-->
</script>
<% End If %>
<% If Article.Export = "" Then %>
<% End If %>
<%

' Load recordset
Rs = Article_list.LoadRecordset()
	Article_list.lStartRec = 1
	If Article_list.lDisplayRecs <= 0 Then ' Display all records
		Article_list.lDisplayRecs = Article_list.lTotalRecs
	End If
	If Not (Article.ExportAll AndAlso Article.Export <> "") Then
		Article_list.SetUpStartRec() ' Set up start record position
	End If
%>
<p><span class="aspnetmaker" style="white-space: nowrap;">TABLE: Site Article
<% If Article.Export = "" AndAlso Article.CurrentAction = "" Then %>
&nbsp;&nbsp;<a href="<%= Article_list.PageUrl %>export=excel"><img src='images/exportxls.gif' alt='Export to Excel' title='Export to Excel' width='16' height='16' border='0'></a>
<% End If %>
</span></p>
<% If Article.Export = "" AndAlso Article.CurrentAction = "" Then %>
<a href="javascript:ew_ToggleSearchPanel(Article_list);" style="text-decoration: none;"><img id="Article_list_SearchImage" src="images/collapse.gif" alt="" width="9" height="9" border="0"></a><span class="aspnetmaker">&nbsp;Search</span><br>
<div id="Article_list_SearchPanel">
<form name="fArticlelistsrch" id="fArticlelistsrch" class="ewForm">
<input type="hidden" id="t" name="t" value="Article" />
<table class="ewBasicSearch">
	<tr>
		<td><span class="aspnetmaker">
			<input type="text" name="<%= EW_TABLE_BASIC_SEARCH %>" id="<%= EW_TABLE_BASIC_SEARCH %>" size="20" value="<%= ew_HtmlEncode(Article.BasicSearchKeyword) %>" />
			<input type="Submit" name="Submit" id="Submit" value="Search (*)" />&nbsp;
			<a href="<%= Article_list.PageUrl %>cmd=reset">Show all</a>&nbsp;
			<a href="Article_srch.aspx">Advanced Search</a>&nbsp;
		</span></td>
	</tr>
	<tr>
	<td><span class="aspnetmaker"><label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value=""<% If Article.BasicSearchType = "" Then %> checked="checked"<% End If %> />Exact phrase</label>&nbsp;&nbsp;<label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value="AND"<% If Article.BasicSearchType = "AND" Then %> checked="checked"<% End If %> />All words</label>&nbsp;&nbsp;<label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value="OR"<% If Article.BasicSearchType = "OR" Then %> checked="checked"<% End If %> />Any word</label></span></td>
	</tr>
</table>
</form>
</div>
<% End If %>
<% Article_list.ShowMessage() %>
<br />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<% If Article.Export = "" Then %>
<div class="ewGridUpperPanel">
<% If Article.CurrentAction <> "gridadd" AndAlso Article.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If Article_list.Pager Is Nothing Then Article_list.Pager = New cPrevNextPager(Article_list.lStartRec, Article_list.lDisplayRecs, Article_list.lTotalRecs) %>
<% If Article_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If Article_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= Article_list.PageUrl %>start=<%= Article_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If Article_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= Article_list.PageUrl %>start=<%= Article_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= Article_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If Article_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= Article_list.PageUrl %>start=<%= Article_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If Article_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= Article_list.PageUrl %>start=<%= Article_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= Article_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= Article_list.Pager.FromIndex %> to <%= Article_list.Pager.ToIndex %> of <%= Article_list.Pager.RecordCount %></span>
<% Else %>
	<% If Article_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If Article_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="Article" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If Article_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If Article_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If Article_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If Article.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<div class="aspnetmaker">
<a href="<%= Article.AddUrl %>">Add</a>&nbsp;&nbsp;
<% If Article_list.lTotalRecs > 0 Then %>
<a href="" onclick="f=ew_GetForm('fArticlelist');if (!ew_KeySelected(f)) alert('No records selected'); else if (ew_Confirm('<%= Article_list.sDeleteConfirmMsg %>')) {f.action='Article_delete.aspx';f.encoding='application/x-www-form-urlencoded';f.submit();};return false;">Delete Selected Records</a>&nbsp;&nbsp;
<% End If %>
</div>
</div>
<% End If %>
<div class="ewGridMiddlePanel">
<form name="fArticlelist" id="fArticlelist" class="ewForm" method="post">
<% If Article_list.lTotalRecs > 0 Then %>
<table cellspacing="0" rowhighlightclass="ewTableHighlightRow" rowselectclass="ewTableSelectRow" roweditclass="ewTableEditRow" class="ewTable ewTableSeparate">
<%
	Article_list.lOptionCnt = 0
	Article_list.lOptionCnt = Article_list.lOptionCnt + 1 ' View
	Article_list.lOptionCnt = Article_list.lOptionCnt + 1 ' Edit
	Article_list.lOptionCnt = Article_list.lOptionCnt + 1 ' Copy
	Article_list.lOptionCnt = Article_list.lOptionCnt + 1 ' Multi-select
	Article_list.lOptionCnt = Article_list.lOptionCnt + Article_list.ListOptions.Items.Count ' Custom list options
%>
<%= Article.TableCustomInnerHTML %>
<thead><!-- Table header -->
	<tr class="ewTableHeader">
<% If Article.Export = "" Then %>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">
<input type="checkbox" name="key" id="key" class="aspnetmaker" onclick="Article_list.SelectAllKey(this);" />
</td>
<%

' Custom list options
For i As Integer = 0 to Article_list.ListOptions.Items.Count -1
	If Article_list.ListOptions.Items(i).Visible Then Response.Write(Article_list.ListOptions.Items(i).HeaderCellHtml)
Next
%>
<% End If %>
<% If Article.Active.Visible Then ' Active %>
	<% If Article.SortUrl(Article.Active) = "" Then %>
		<td style="white-space: nowrap;">Active</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= Article.SortUrl(Article.Active) %>',1);" style="white-space: nowrap;">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Active</td><td style="width: 10px;"><% If Article.Active.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Article.Active.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If Article.StartDT.Visible Then ' StartDT %>
	<% If Article.SortUrl(Article.StartDT) = "" Then %>
		<td style="white-space: nowrap;">Start DT</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= Article.SortUrl(Article.StartDT) %>',1);" style="white-space: nowrap;">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Start DT</td><td style="width: 10px;"><% If Article.StartDT.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Article.StartDT.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If Article.Title.Visible Then ' Title %>
	<% If Article.SortUrl(Article.Title) = "" Then %>
		<td style="white-space: nowrap;">Title</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= Article.SortUrl(Article.Title) %>',1);" style="white-space: nowrap;">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Title&nbsp;(*)</td><td style="width: 10px;"><% If Article.Title.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Article.Title.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If Article.zPageID.Visible Then ' PageID %>
	<% If Article.SortUrl(Article.zPageID) = "" Then %>
		<td style="white-space: nowrap;">Location</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= Article.SortUrl(Article.zPageID) %>',1);" style="white-space: nowrap;">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Location</td><td style="width: 10px;"><% If Article.zPageID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Article.zPageID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
	</tr>
</thead>
<tbody><!-- Table body -->
<%
If (Article.ExportAll AndAlso Article.Export <> "") Then
	Article_list.lStopRec = Article_list.lTotalRecs
Else
	Article_list.lStopRec = Article_list.lStartRec + Article_list.lDisplayRecs - 1 ' Set the last record to display
End If
If Article.CurrentAction = "gridadd" AndAlso Article_list.lStopRec = -1 Then
	Article_list.lStopRec = EW_GRIDADD_ROWS
End If 

' Move to first record
For i As Integer = 1 to Article_list.lStartRec - 1
	If Rs.Read() Then	Article_list.lRecCnt = Article_list.lRecCnt + 1
Next		
Article_list.lRowCnt = 0

' Output data rows
Do While (Article.CurrentAction = "gridadd" OrElse Rs.Read()) AndAlso (Article_list.lRecCnt < Article_list.lStopRec)
	Article_list.lRecCnt = Article_list.lRecCnt + 1
	If Article_list.lRecCnt >= Article_list.lStartRec Then
		Article_list.lRowCnt = Article_list.lRowCnt + 1
	Article.CssClass = ""
	Article.CssStyle = ""
	Article.RowClientEvents = "onmouseover='ew_MouseOver(event, this);' onmouseout='ew_MouseOut(event, this);' onclick='ew_Click(event, this);'"
	If Article.CurrentAction = "gridadd" Then
		Article_list.LoadDefaultValues() ' Load default values
	Else
		Article_list.LoadRowValues(Rs) ' Load row values
	End If
	Article.RowType = EW_ROWTYPE_VIEW ' Render view

	' Render row
	Article_list.RenderRow()
%>
	<tr<%= Article.RowAttributes %>>
<% If Article.Export = "" Then %>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= Article.ViewUrl %>"><img src='images/view.gif' alt='View' title='View' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= Article.EditUrl %>"><img src='images/edit.gif' alt='Edit' title='Edit' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= Article.CopyUrl %>"><img src='images/copy.gif' alt='Copy' title='Copy' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<input type="checkbox" name="key_m" id="key_m" value="<%= Server.HtmlEncode(Convert.ToString(Article.ArticleID.CurrentValue)) %>" class="aspnetmaker" onclick='ew_ClickMultiCheckbox(this);' />
</span></td>
<%

' Custom list options
For i As Integer = 0 to Article_list.ListOptions.Items.Count -1
	If Article_list.ListOptions.Items(i).Visible Then Response.Write(Article_list.ListOptions.Items(i).BodyCellHtml)
Next
%>
<% End If %>
	<% If Article.Active.Visible Then ' Active %>
		<td<%= Article.Active.CellAttributes %>>
<% If Convert.ToString(Article.Active.CurrentValue) = "1" Then %>
<input type="checkbox" value="<%= Article.Active.ListViewValue %>" checked onclick="this.form.reset();" disabled="disabled" />
<% Else %>
<input type="checkbox" value="<%= Article.Active.ListViewValue %>" onclick="this.form.reset();" disabled="disabled" />
<% End If %>
</td>
	<% End If %>
	<% If Article.StartDT.Visible Then ' StartDT %>
		<td<%= Article.StartDT.CellAttributes %>>
<div<%= Article.StartDT.ViewAttributes %>><%= Article.StartDT.ListViewValue %></div>
</td>
	<% End If %>
	<% If Article.Title.Visible Then ' Title %>
		<td<%= Article.Title.CellAttributes %>>
<div<%= Article.Title.ViewAttributes %>><%= Article.Title.ListViewValue %></div>
</td>
	<% End If %>
	<% If Article.zPageID.Visible Then ' PageID %>
		<td<%= Article.zPageID.CellAttributes %>>
<div<%= Article.zPageID.ViewAttributes %>><%= Article.zPageID.ListViewValue %></div>
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
<% If Article_list.lTotalRecs > 0 Then %>
<% If Article.Export = "" Then %>
<div class="ewGridLowerPanel">
<% If Article.CurrentAction <> "gridadd" AndAlso Article.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If Article_list.Pager Is Nothing Then Article_list.Pager = New cPrevNextPager(Article_list.lStartRec, Article_list.lDisplayRecs, Article_list.lTotalRecs) %>
<% If Article_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If Article_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= Article_list.PageUrl %>start=<%= Article_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If Article_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= Article_list.PageUrl %>start=<%= Article_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= Article_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If Article_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= Article_list.PageUrl %>start=<%= Article_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If Article_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= Article_list.PageUrl %>start=<%= Article_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= Article_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= Article_list.Pager.FromIndex %> to <%= Article_list.Pager.ToIndex %> of <%= Article_list.Pager.RecordCount %></span>
<% Else %>
	<% If Article_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If Article_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="Article" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If Article_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If Article_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If Article_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If Article.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<% 'If Article_list.lTotalRecs > 0 Then %>
<div class="aspnetmaker">
<a href="<%= Article.AddUrl %>">Add</a>&nbsp;&nbsp;
<% If Article_list.lTotalRecs > 0 Then %>
<a href="" onclick="f=ew_GetForm('fArticlelist');if (!ew_KeySelected(f)) alert('No records selected'); else if (ew_Confirm('<%= Article_list.sDeleteConfirmMsg %>')) {f.action='Article_delete.aspx';f.encoding='application/x-www-form-urlencoded';f.submit();};return false;">Delete Selected Records</a>&nbsp;&nbsp;
<% End If %>
</div>
<% 'End If %>
</div>
<% End If %>
<% End If %>
</td></tr></table>
<% If Article.Export = "" AndAlso Article.CurrentAction = "" Then %>
<script type="text/javascript">
<!--
//ew_ToggleSearchPanel(Article_list); // uncomment to init search panel as collapsed
//-->
</script>
<% End If %>
<% If Article.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
