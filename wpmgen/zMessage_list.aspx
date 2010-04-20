<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="zMessage_list.aspx.vb" Inherits="zMessage_list" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If zMessage.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var zMessage_list = new ew_Page("zMessage_list");
// page properties
zMessage_list.PageID = "list"; // page ID
var EW_PAGE_ID = zMessage_list.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
zMessage_list.ValidateRequired = true; // uses JavaScript validation
<% Else %>
zMessage_list.ValidateRequired = false; // no JavaScript validation
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
<% If zMessage.Export = "" Then %>
<% End If %>
<%

' Load recordset
Rs = zMessage_list.LoadRecordset()
	zMessage_list.lStartRec = 1
	If zMessage_list.lDisplayRecs <= 0 Then ' Display all records
		zMessage_list.lDisplayRecs = zMessage_list.lTotalRecs
	End If
	If Not (zMessage.ExportAll AndAlso zMessage.Export <> "") Then
		zMessage_list.SetUpStartRec() ' Set up start record position
	End If
%>
<p><span class="aspnetmaker" style="white-space: nowrap;">TABLE: Message
<% If zMessage.Export = "" AndAlso zMessage.CurrentAction = "" Then %>
&nbsp;&nbsp;<a href="<%= zMessage_list.PageUrl %>export=excel"><img src='images/exportxls.gif' alt='Export to Excel' title='Export to Excel' width='16' height='16' border='0'></a>
<% End If %>
</span></p>
<% If zMessage.Export = "" AndAlso zMessage.CurrentAction = "" Then %>
<a href="javascript:ew_ToggleSearchPanel(zMessage_list);" style="text-decoration: none;"><img id="zMessage_list_SearchImage" src="images/collapse.gif" alt="" width="9" height="9" border="0"></a><span class="aspnetmaker">&nbsp;Search</span><br>
<div id="zMessage_list_SearchPanel">
<form name="fzMessagelistsrch" id="fzMessagelistsrch" class="ewForm">
<input type="hidden" id="t" name="t" value="zMessage" />
<table class="ewBasicSearch">
	<tr>
		<td><span class="aspnetmaker">
			<input type="text" name="<%= EW_TABLE_BASIC_SEARCH %>" id="<%= EW_TABLE_BASIC_SEARCH %>" size="20" value="<%= ew_HtmlEncode(zMessage.BasicSearchKeyword) %>" />
			<input type="Submit" name="Submit" id="Submit" value="Search (*)" />&nbsp;
			<a href="<%= zMessage_list.PageUrl %>cmd=reset">Show all</a>&nbsp;
			<a href="zMessage_srch.aspx">Advanced Search</a>&nbsp;
		</span></td>
	</tr>
	<tr>
	<td><span class="aspnetmaker"><label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value=""<% If zMessage.BasicSearchType = "" Then %> checked="checked"<% End If %> />Exact phrase</label>&nbsp;&nbsp;<label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value="AND"<% If zMessage.BasicSearchType = "AND" Then %> checked="checked"<% End If %> />All words</label>&nbsp;&nbsp;<label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value="OR"<% If zMessage.BasicSearchType = "OR" Then %> checked="checked"<% End If %> />Any word</label></span></td>
	</tr>
</table>
</form>
</div>
<% End If %>
<% zMessage_list.ShowMessage() %>
<br />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<% If zMessage.Export = "" Then %>
<div class="ewGridUpperPanel">
<% If zMessage.CurrentAction <> "gridadd" AndAlso zMessage.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If zMessage_list.Pager Is Nothing Then zMessage_list.Pager = New cPrevNextPager(zMessage_list.lStartRec, zMessage_list.lDisplayRecs, zMessage_list.lTotalRecs) %>
<% If zMessage_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If zMessage_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= zMessage_list.PageUrl %>start=<%= zMessage_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If zMessage_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= zMessage_list.PageUrl %>start=<%= zMessage_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= zMessage_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If zMessage_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= zMessage_list.PageUrl %>start=<%= zMessage_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If zMessage_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= zMessage_list.PageUrl %>start=<%= zMessage_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= zMessage_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= zMessage_list.Pager.FromIndex %> to <%= zMessage_list.Pager.ToIndex %> of <%= zMessage_list.Pager.RecordCount %></span>
<% Else %>
	<% If zMessage_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If zMessage_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="zMessage" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If zMessage_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If zMessage_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If zMessage_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If zMessage.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<div class="aspnetmaker">
<a href="<%= zMessage.AddUrl %>">Add</a>&nbsp;&nbsp;
</div>
</div>
<% End If %>
<div class="ewGridMiddlePanel">
<form name="fzMessagelist" id="fzMessagelist" class="ewForm" method="post">
<% If zMessage_list.lTotalRecs > 0 Then %>
<table cellspacing="0" rowhighlightclass="ewTableHighlightRow" rowselectclass="ewTableSelectRow" roweditclass="ewTableEditRow" class="ewTable ewTableSeparate">
<%
	zMessage_list.lOptionCnt = 0
	zMessage_list.lOptionCnt = zMessage_list.lOptionCnt + 1 ' View
	zMessage_list.lOptionCnt = zMessage_list.lOptionCnt + 1 ' Edit
	zMessage_list.lOptionCnt = zMessage_list.lOptionCnt + 1 ' Copy
	zMessage_list.lOptionCnt = zMessage_list.lOptionCnt + 1 ' Delete
	zMessage_list.lOptionCnt = zMessage_list.lOptionCnt + zMessage_list.ListOptions.Items.Count ' Custom list options
%>
<%= zMessage.TableCustomInnerHTML %>
<thead><!-- Table header -->
	<tr class="ewTableHeader">
<% If zMessage.Export = "" Then %>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<%

' Custom list options
For i As Integer = 0 to zMessage_list.ListOptions.Items.Count -1
	If zMessage_list.ListOptions.Items(i).Visible Then Response.Write(zMessage_list.ListOptions.Items(i).HeaderCellHtml)
Next
%>
<% End If %>
<% If zMessage.MessageID.Visible Then ' MessageID %>
	<% If zMessage.SortUrl(zMessage.MessageID) = "" Then %>
		<td>Message ID</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= zMessage.SortUrl(zMessage.MessageID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Message ID</td><td style="width: 10px;"><% If zMessage.MessageID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf zMessage.MessageID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If zMessage.zPageID.Visible Then ' PageID %>
	<% If zMessage.SortUrl(zMessage.zPageID) = "" Then %>
		<td>Page ID</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= zMessage.SortUrl(zMessage.zPageID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Page ID</td><td style="width: 10px;"><% If zMessage.zPageID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf zMessage.zPageID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If zMessage.ParentMessageID.Visible Then ' ParentMessageID %>
	<% If zMessage.SortUrl(zMessage.ParentMessageID) = "" Then %>
		<td>Parent Message ID</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= zMessage.SortUrl(zMessage.ParentMessageID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Parent Message ID</td><td style="width: 10px;"><% If zMessage.ParentMessageID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf zMessage.ParentMessageID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If zMessage.Subject.Visible Then ' Subject %>
	<% If zMessage.SortUrl(zMessage.Subject) = "" Then %>
		<td>Subject</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= zMessage.SortUrl(zMessage.Subject) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Subject&nbsp;(*)</td><td style="width: 10px;"><% If zMessage.Subject.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf zMessage.Subject.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If zMessage.Author.Visible Then ' Author %>
	<% If zMessage.SortUrl(zMessage.Author) = "" Then %>
		<td>Author</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= zMessage.SortUrl(zMessage.Author) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Author&nbsp;(*)</td><td style="width: 10px;"><% If zMessage.Author.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf zMessage.Author.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If zMessage.zEmail.Visible Then ' Email %>
	<% If zMessage.SortUrl(zMessage.zEmail) = "" Then %>
		<td>Email</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= zMessage.SortUrl(zMessage.zEmail) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Email&nbsp;(*)</td><td style="width: 10px;"><% If zMessage.zEmail.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf zMessage.zEmail.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If zMessage.City.Visible Then ' City %>
	<% If zMessage.SortUrl(zMessage.City) = "" Then %>
		<td>City</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= zMessage.SortUrl(zMessage.City) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>City&nbsp;(*)</td><td style="width: 10px;"><% If zMessage.City.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf zMessage.City.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If zMessage.URL.Visible Then ' URL %>
	<% If zMessage.SortUrl(zMessage.URL) = "" Then %>
		<td>URL</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= zMessage.SortUrl(zMessage.URL) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>URL&nbsp;(*)</td><td style="width: 10px;"><% If zMessage.URL.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf zMessage.URL.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If zMessage.MessageDate.Visible Then ' MessageDate %>
	<% If zMessage.SortUrl(zMessage.MessageDate) = "" Then %>
		<td>Message Date</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= zMessage.SortUrl(zMessage.MessageDate) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Message Date</td><td style="width: 10px;"><% If zMessage.MessageDate.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf zMessage.MessageDate.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
	</tr>
</thead>
<tbody><!-- Table body -->
<%
If (zMessage.ExportAll AndAlso zMessage.Export <> "") Then
	zMessage_list.lStopRec = zMessage_list.lTotalRecs
Else
	zMessage_list.lStopRec = zMessage_list.lStartRec + zMessage_list.lDisplayRecs - 1 ' Set the last record to display
End If
If zMessage.CurrentAction = "gridadd" AndAlso zMessage_list.lStopRec = -1 Then
	zMessage_list.lStopRec = EW_GRIDADD_ROWS
End If 

' Move to first record
For i As Integer = 1 to zMessage_list.lStartRec - 1
	If Rs.Read() Then	zMessage_list.lRecCnt = zMessage_list.lRecCnt + 1
Next		
zMessage_list.lRowCnt = 0

' Output data rows
Do While (zMessage.CurrentAction = "gridadd" OrElse Rs.Read()) AndAlso (zMessage_list.lRecCnt < zMessage_list.lStopRec)
	zMessage_list.lRecCnt = zMessage_list.lRecCnt + 1
	If zMessage_list.lRecCnt >= zMessage_list.lStartRec Then
		zMessage_list.lRowCnt = zMessage_list.lRowCnt + 1
	zMessage.CssClass = ""
	zMessage.CssStyle = ""
	zMessage.RowClientEvents = "onmouseover='ew_MouseOver(event, this);' onmouseout='ew_MouseOut(event, this);' onclick='ew_Click(event, this);'"
	If zMessage.CurrentAction = "gridadd" Then
		zMessage_list.LoadDefaultValues() ' Load default values
	Else
		zMessage_list.LoadRowValues(Rs) ' Load row values
	End If
	zMessage.RowType = EW_ROWTYPE_VIEW ' Render view

	' Render row
	zMessage_list.RenderRow()
%>
	<tr<%= zMessage.RowAttributes %>>
<% If zMessage.Export = "" Then %>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= zMessage.ViewUrl %>"><img src='images/view.gif' alt='View' title='View' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= zMessage.EditUrl %>"><img src='images/edit.gif' alt='Edit' title='Edit' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= zMessage.CopyUrl %>"><img src='images/copy.gif' alt='Copy' title='Copy' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= zMessage.DeleteUrl %>"><img src='images/delete.gif' alt='Delete' title='Delete' width='16' height='16' border='0'></a>
</span></td>
<%

' Custom list options
For i As Integer = 0 to zMessage_list.ListOptions.Items.Count -1
	If zMessage_list.ListOptions.Items(i).Visible Then Response.Write(zMessage_list.ListOptions.Items(i).BodyCellHtml)
Next
%>
<% End If %>
	<% If zMessage.MessageID.Visible Then ' MessageID %>
		<td<%= zMessage.MessageID.CellAttributes %>>
<div<%= zMessage.MessageID.ViewAttributes %>><%= zMessage.MessageID.ListViewValue %></div>
</td>
	<% End If %>
	<% If zMessage.zPageID.Visible Then ' PageID %>
		<td<%= zMessage.zPageID.CellAttributes %>>
<div<%= zMessage.zPageID.ViewAttributes %>><%= zMessage.zPageID.ListViewValue %></div>
</td>
	<% End If %>
	<% If zMessage.ParentMessageID.Visible Then ' ParentMessageID %>
		<td<%= zMessage.ParentMessageID.CellAttributes %>>
<div<%= zMessage.ParentMessageID.ViewAttributes %>><%= zMessage.ParentMessageID.ListViewValue %></div>
</td>
	<% End If %>
	<% If zMessage.Subject.Visible Then ' Subject %>
		<td<%= zMessage.Subject.CellAttributes %>>
<div<%= zMessage.Subject.ViewAttributes %>><%= zMessage.Subject.ListViewValue %></div>
</td>
	<% End If %>
	<% If zMessage.Author.Visible Then ' Author %>
		<td<%= zMessage.Author.CellAttributes %>>
<div<%= zMessage.Author.ViewAttributes %>><%= zMessage.Author.ListViewValue %></div>
</td>
	<% End If %>
	<% If zMessage.zEmail.Visible Then ' Email %>
		<td<%= zMessage.zEmail.CellAttributes %>>
<div<%= zMessage.zEmail.ViewAttributes %>><%= zMessage.zEmail.ListViewValue %></div>
</td>
	<% End If %>
	<% If zMessage.City.Visible Then ' City %>
		<td<%= zMessage.City.CellAttributes %>>
<div<%= zMessage.City.ViewAttributes %>><%= zMessage.City.ListViewValue %></div>
</td>
	<% End If %>
	<% If zMessage.URL.Visible Then ' URL %>
		<td<%= zMessage.URL.CellAttributes %>>
<div<%= zMessage.URL.ViewAttributes %>><%= zMessage.URL.ListViewValue %></div>
</td>
	<% End If %>
	<% If zMessage.MessageDate.Visible Then ' MessageDate %>
		<td<%= zMessage.MessageDate.CellAttributes %>>
<div<%= zMessage.MessageDate.ViewAttributes %>><%= zMessage.MessageDate.ListViewValue %></div>
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
<% If zMessage_list.lTotalRecs > 0 Then %>
<% If zMessage.Export = "" Then %>
<div class="ewGridLowerPanel">
<% If zMessage.CurrentAction <> "gridadd" AndAlso zMessage.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If zMessage_list.Pager Is Nothing Then zMessage_list.Pager = New cPrevNextPager(zMessage_list.lStartRec, zMessage_list.lDisplayRecs, zMessage_list.lTotalRecs) %>
<% If zMessage_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If zMessage_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= zMessage_list.PageUrl %>start=<%= zMessage_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If zMessage_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= zMessage_list.PageUrl %>start=<%= zMessage_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= zMessage_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If zMessage_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= zMessage_list.PageUrl %>start=<%= zMessage_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If zMessage_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= zMessage_list.PageUrl %>start=<%= zMessage_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= zMessage_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= zMessage_list.Pager.FromIndex %> to <%= zMessage_list.Pager.ToIndex %> of <%= zMessage_list.Pager.RecordCount %></span>
<% Else %>
	<% If zMessage_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If zMessage_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="zMessage" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If zMessage_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If zMessage_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If zMessage_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If zMessage.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<% 'If zMessage_list.lTotalRecs > 0 Then %>
<div class="aspnetmaker">
<a href="<%= zMessage.AddUrl %>">Add</a>&nbsp;&nbsp;
</div>
<% 'End If %>
</div>
<% End If %>
<% End If %>
</td></tr></table>
<% If zMessage.Export = "" AndAlso zMessage.CurrentAction = "" Then %>
<script type="text/javascript">
<!--
//ew_ToggleSearchPanel(zMessage_list); // uncomment to init search panel as collapsed
//-->
</script>
<% End If %>
<% If zMessage.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
