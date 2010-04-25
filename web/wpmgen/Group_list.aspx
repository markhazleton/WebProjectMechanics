<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="Group_list.aspx.vb" Inherits="Group_list" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If Group.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var Group_list = new ew_Page("Group_list");
// page properties
Group_list.PageID = "list"; // page ID
var EW_PAGE_ID = Group_list.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
Group_list.ValidateRequired = true; // uses JavaScript validation
<% Else %>
Group_list.ValidateRequired = false; // no JavaScript validation
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
<% If Group.Export = "" Then %>
<% End If %>
<%

' Load recordset
Rs = Group_list.LoadRecordset()
	Group_list.lStartRec = 1
	If Group_list.lDisplayRecs <= 0 Then ' Display all records
		Group_list.lDisplayRecs = Group_list.lTotalRecs
	End If
	If Not (Group.ExportAll AndAlso Group.Export <> "") Then
		Group_list.SetUpStartRec() ' Set up start record position
	End If
%>
<p><span class="aspnetmaker" style="white-space: nowrap;">TABLE: Contact Group
<% If Group.Export = "" AndAlso Group.CurrentAction = "" Then %>
&nbsp;&nbsp;<a href="<%= Group_list.PageUrl %>export=excel"><img src='images/exportxls.gif' alt='Export to Excel' title='Export to Excel' width='16' height='16' border='0'></a>
<% End If %>
</span></p>
<% If Group.Export = "" AndAlso Group.CurrentAction = "" Then %>
<a href="javascript:ew_ToggleSearchPanel(Group_list);" style="text-decoration: none;"><img id="Group_list_SearchImage" src="images/collapse.gif" alt="" width="9" height="9" border="0"></a><span class="aspnetmaker">&nbsp;Search</span><br>
<div id="Group_list_SearchPanel">
<form name="fGrouplistsrch" id="fGrouplistsrch" class="ewForm">
<input type="hidden" id="t" name="t" value="Group" />
<table class="ewBasicSearch">
	<tr>
		<td><span class="aspnetmaker">
			<input type="text" name="<%= EW_TABLE_BASIC_SEARCH %>" id="<%= EW_TABLE_BASIC_SEARCH %>" size="20" value="<%= ew_HtmlEncode(Group.BasicSearchKeyword) %>" />
			<input type="Submit" name="Submit" id="Submit" value="Search (*)" />&nbsp;
			<a href="<%= Group_list.PageUrl %>cmd=reset">Show all</a>&nbsp;
			<a href="Group_srch.aspx">Advanced Search</a>&nbsp;
		</span></td>
	</tr>
	<tr>
	<td><span class="aspnetmaker"><label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value=""<% If Group.BasicSearchType = "" Then %> checked="checked"<% End If %> />Exact phrase</label>&nbsp;&nbsp;<label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value="AND"<% If Group.BasicSearchType = "AND" Then %> checked="checked"<% End If %> />All words</label>&nbsp;&nbsp;<label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value="OR"<% If Group.BasicSearchType = "OR" Then %> checked="checked"<% End If %> />Any word</label></span></td>
	</tr>
</table>
</form>
</div>
<% End If %>
<% Group_list.ShowMessage() %>
<br />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<% If Group.Export = "" Then %>
<div class="ewGridUpperPanel">
<% If Group.CurrentAction <> "gridadd" AndAlso Group.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If Group_list.Pager Is Nothing Then Group_list.Pager = New cPrevNextPager(Group_list.lStartRec, Group_list.lDisplayRecs, Group_list.lTotalRecs) %>
<% If Group_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If Group_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= Group_list.PageUrl %>start=<%= Group_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If Group_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= Group_list.PageUrl %>start=<%= Group_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= Group_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If Group_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= Group_list.PageUrl %>start=<%= Group_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If Group_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= Group_list.PageUrl %>start=<%= Group_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= Group_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= Group_list.Pager.FromIndex %> to <%= Group_list.Pager.ToIndex %> of <%= Group_list.Pager.RecordCount %></span>
<% Else %>
	<% If Group_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If Group_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="Group" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If Group_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If Group_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If Group_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If Group.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<div class="aspnetmaker">
<a href="<%= Group.AddUrl %>">Add</a>&nbsp;&nbsp;
</div>
</div>
<% End If %>
<div class="ewGridMiddlePanel">
<form name="fGrouplist" id="fGrouplist" class="ewForm" method="post">
<% If Group_list.lTotalRecs > 0 Then %>
<table cellspacing="0" rowhighlightclass="ewTableHighlightRow" rowselectclass="ewTableSelectRow" roweditclass="ewTableEditRow" class="ewTable ewTableSeparate">
<%
	Group_list.lOptionCnt = 0
	Group_list.lOptionCnt = Group_list.lOptionCnt + 1 ' View
	Group_list.lOptionCnt = Group_list.lOptionCnt + 1 ' Edit
	Group_list.lOptionCnt = Group_list.lOptionCnt + 1 ' Copy
	Group_list.lOptionCnt = Group_list.lOptionCnt + 1 ' Delete
	Group_list.lOptionCnt = Group_list.lOptionCnt + Group_list.ListOptions.Items.Count ' Custom list options
%>
<%= Group.TableCustomInnerHTML %>
<thead><!-- Table header -->
	<tr class="ewTableHeader">
<% If Group.Export = "" Then %>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<%

' Custom list options
For i As Integer = 0 to Group_list.ListOptions.Items.Count -1
	If Group_list.ListOptions.Items(i).Visible Then Response.Write(Group_list.ListOptions.Items(i).HeaderCellHtml)
Next
%>
<% End If %>
<% If Group.GroupName.Visible Then ' GroupName %>
	<% If Group.SortUrl(Group.GroupName) = "" Then %>
		<td>Group Name</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= Group.SortUrl(Group.GroupName) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Group Name&nbsp;(*)</td><td style="width: 10px;"><% If Group.GroupName.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Group.GroupName.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If Group.GroupComment.Visible Then ' GroupComment %>
	<% If Group.SortUrl(Group.GroupComment) = "" Then %>
		<td>Group Comment</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= Group.SortUrl(Group.GroupComment) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Group Comment&nbsp;(*)</td><td style="width: 10px;"><% If Group.GroupComment.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Group.GroupComment.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
	</tr>
</thead>
<tbody><!-- Table body -->
<%
If (Group.ExportAll AndAlso Group.Export <> "") Then
	Group_list.lStopRec = Group_list.lTotalRecs
Else
	Group_list.lStopRec = Group_list.lStartRec + Group_list.lDisplayRecs - 1 ' Set the last record to display
End If
If Group.CurrentAction = "gridadd" AndAlso Group_list.lStopRec = -1 Then
	Group_list.lStopRec = EW_GRIDADD_ROWS
End If 

' Move to first record
For i As Integer = 1 to Group_list.lStartRec - 1
	If Rs.Read() Then	Group_list.lRecCnt = Group_list.lRecCnt + 1
Next		
Group_list.lRowCnt = 0

' Output data rows
Do While (Group.CurrentAction = "gridadd" OrElse Rs.Read()) AndAlso (Group_list.lRecCnt < Group_list.lStopRec)
	Group_list.lRecCnt = Group_list.lRecCnt + 1
	If Group_list.lRecCnt >= Group_list.lStartRec Then
		Group_list.lRowCnt = Group_list.lRowCnt + 1
	Group.CssClass = ""
	Group.CssStyle = ""
	Group.RowClientEvents = "onmouseover='ew_MouseOver(event, this);' onmouseout='ew_MouseOut(event, this);' onclick='ew_Click(event, this);'"
	If Group.CurrentAction = "gridadd" Then
		Group_list.LoadDefaultValues() ' Load default values
	Else
		Group_list.LoadRowValues(Rs) ' Load row values
	End If
	Group.RowType = EW_ROWTYPE_VIEW ' Render view

	' Render row
	Group_list.RenderRow()
%>
	<tr<%= Group.RowAttributes %>>
<% If Group.Export = "" Then %>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= Group.ViewUrl %>"><img src='images/view.gif' alt='View' title='View' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= Group.EditUrl %>"><img src='images/edit.gif' alt='Edit' title='Edit' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= Group.CopyUrl %>"><img src='images/copy.gif' alt='Copy' title='Copy' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= Group.DeleteUrl %>"><img src='images/delete.gif' alt='Delete' title='Delete' width='16' height='16' border='0'></a>
</span></td>
<%

' Custom list options
For i As Integer = 0 to Group_list.ListOptions.Items.Count -1
	If Group_list.ListOptions.Items(i).Visible Then Response.Write(Group_list.ListOptions.Items(i).BodyCellHtml)
Next
%>
<% End If %>
	<% If Group.GroupName.Visible Then ' GroupName %>
		<td<%= Group.GroupName.CellAttributes %>>
<div<%= Group.GroupName.ViewAttributes %>><%= Group.GroupName.ListViewValue %></div>
</td>
	<% End If %>
	<% If Group.GroupComment.Visible Then ' GroupComment %>
		<td<%= Group.GroupComment.CellAttributes %>>
<div<%= Group.GroupComment.ViewAttributes %>><%= Group.GroupComment.ListViewValue %></div>
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
<% If Group_list.lTotalRecs > 0 Then %>
<% If Group.Export = "" Then %>
<div class="ewGridLowerPanel">
<% If Group.CurrentAction <> "gridadd" AndAlso Group.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If Group_list.Pager Is Nothing Then Group_list.Pager = New cPrevNextPager(Group_list.lStartRec, Group_list.lDisplayRecs, Group_list.lTotalRecs) %>
<% If Group_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If Group_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= Group_list.PageUrl %>start=<%= Group_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If Group_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= Group_list.PageUrl %>start=<%= Group_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= Group_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If Group_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= Group_list.PageUrl %>start=<%= Group_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If Group_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= Group_list.PageUrl %>start=<%= Group_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= Group_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= Group_list.Pager.FromIndex %> to <%= Group_list.Pager.ToIndex %> of <%= Group_list.Pager.RecordCount %></span>
<% Else %>
	<% If Group_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If Group_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="Group" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If Group_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If Group_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If Group_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If Group.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<% 'If Group_list.lTotalRecs > 0 Then %>
<div class="aspnetmaker">
<a href="<%= Group.AddUrl %>">Add</a>&nbsp;&nbsp;
</div>
<% 'End If %>
</div>
<% End If %>
<% End If %>
</td></tr></table>
<% If Group.Export = "" AndAlso Group.CurrentAction = "" Then %>
<script type="text/javascript">
<!--
//ew_ToggleSearchPanel(Group_list); // uncomment to init search panel as collapsed
//-->
</script>
<% End If %>
<% If Group.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
