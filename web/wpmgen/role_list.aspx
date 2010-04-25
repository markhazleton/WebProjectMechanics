<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="role_list.aspx.vb" Inherits="role_list" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If role.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var role_list = new ew_Page("role_list");
// page properties
role_list.PageID = "list"; // page ID
var EW_PAGE_ID = role_list.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
role_list.ValidateRequired = true; // uses JavaScript validation
<% Else %>
role_list.ValidateRequired = false; // no JavaScript validation
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
<% If role.Export = "" Then %>
<% End If %>
<%

' Load recordset
Rs = role_list.LoadRecordset()
	role_list.lStartRec = 1
	If role_list.lDisplayRecs <= 0 Then ' Display all records
		role_list.lDisplayRecs = role_list.lTotalRecs
	End If
	If Not (role.ExportAll AndAlso role.Export <> "") Then
		role_list.SetUpStartRec() ' Set up start record position
	End If
%>
<p><span class="aspnetmaker" style="white-space: nowrap;">TABLE: Contact Role
<% If role.Export = "" AndAlso role.CurrentAction = "" Then %>
&nbsp;&nbsp;<a href="<%= role_list.PageUrl %>export=excel"><img src='images/exportxls.gif' alt='Export to Excel' title='Export to Excel' width='16' height='16' border='0'></a>
<% End If %>
</span></p>
<% If role.Export = "" AndAlso role.CurrentAction = "" Then %>
<a href="javascript:ew_ToggleSearchPanel(role_list);" style="text-decoration: none;"><img id="role_list_SearchImage" src="images/collapse.gif" alt="" width="9" height="9" border="0"></a><span class="aspnetmaker">&nbsp;Search</span><br>
<div id="role_list_SearchPanel">
<form name="frolelistsrch" id="frolelistsrch" class="ewForm">
<input type="hidden" id="t" name="t" value="role" />
<table class="ewBasicSearch">
	<tr>
		<td><span class="aspnetmaker">
			<input type="text" name="<%= EW_TABLE_BASIC_SEARCH %>" id="<%= EW_TABLE_BASIC_SEARCH %>" size="20" value="<%= ew_HtmlEncode(role.BasicSearchKeyword) %>" />
			<input type="Submit" name="Submit" id="Submit" value="Search (*)" />&nbsp;
			<a href="<%= role_list.PageUrl %>cmd=reset">Show all</a>&nbsp;
			<a href="role_srch.aspx">Advanced Search</a>&nbsp;
		</span></td>
	</tr>
	<tr>
	<td><span class="aspnetmaker"><label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value=""<% If role.BasicSearchType = "" Then %> checked="checked"<% End If %> />Exact phrase</label>&nbsp;&nbsp;<label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value="AND"<% If role.BasicSearchType = "AND" Then %> checked="checked"<% End If %> />All words</label>&nbsp;&nbsp;<label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value="OR"<% If role.BasicSearchType = "OR" Then %> checked="checked"<% End If %> />Any word</label></span></td>
	</tr>
</table>
</form>
</div>
<% End If %>
<% role_list.ShowMessage() %>
<br />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<% If role.Export = "" Then %>
<div class="ewGridUpperPanel">
<% If role.CurrentAction <> "gridadd" AndAlso role.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If role_list.Pager Is Nothing Then role_list.Pager = New cPrevNextPager(role_list.lStartRec, role_list.lDisplayRecs, role_list.lTotalRecs) %>
<% If role_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If role_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= role_list.PageUrl %>start=<%= role_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If role_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= role_list.PageUrl %>start=<%= role_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= role_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If role_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= role_list.PageUrl %>start=<%= role_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If role_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= role_list.PageUrl %>start=<%= role_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= role_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= role_list.Pager.FromIndex %> to <%= role_list.Pager.ToIndex %> of <%= role_list.Pager.RecordCount %></span>
<% Else %>
	<% If role_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If role_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="role" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If role_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If role_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If role_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If role.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<div class="aspnetmaker">
<a href="<%= role.AddUrl %>">Add</a>&nbsp;&nbsp;
</div>
</div>
<% End If %>
<div class="ewGridMiddlePanel">
<form name="frolelist" id="frolelist" class="ewForm" method="post">
<% If role_list.lTotalRecs > 0 Then %>
<table cellspacing="0" rowhighlightclass="ewTableHighlightRow" rowselectclass="ewTableSelectRow" roweditclass="ewTableEditRow" class="ewTable ewTableSeparate">
<%
	role_list.lOptionCnt = 0
	role_list.lOptionCnt = role_list.lOptionCnt + 1 ' View
	role_list.lOptionCnt = role_list.lOptionCnt + 1 ' Edit
	role_list.lOptionCnt = role_list.lOptionCnt + 1 ' Copy
	role_list.lOptionCnt = role_list.lOptionCnt + 1 ' Delete
	role_list.lOptionCnt = role_list.lOptionCnt + role_list.ListOptions.Items.Count ' Custom list options
%>
<%= role.TableCustomInnerHTML %>
<thead><!-- Table header -->
	<tr class="ewTableHeader">
<% If role.Export = "" Then %>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<%

' Custom list options
For i As Integer = 0 to role_list.ListOptions.Items.Count -1
	If role_list.ListOptions.Items(i).Visible Then Response.Write(role_list.ListOptions.Items(i).HeaderCellHtml)
Next
%>
<% End If %>
<% If role.RoleID.Visible Then ' RoleID %>
	<% If role.SortUrl(role.RoleID) = "" Then %>
		<td>Role ID</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= role.SortUrl(role.RoleID) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Role ID</td><td style="width: 10px;"><% If role.RoleID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf role.RoleID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If role.RoleName.Visible Then ' RoleName %>
	<% If role.SortUrl(role.RoleName) = "" Then %>
		<td>Role Name</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= role.SortUrl(role.RoleName) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Role Name&nbsp;(*)</td><td style="width: 10px;"><% If role.RoleName.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf role.RoleName.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If role.RoleTitle.Visible Then ' RoleTitle %>
	<% If role.SortUrl(role.RoleTitle) = "" Then %>
		<td>Role Title</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= role.SortUrl(role.RoleTitle) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Role Title&nbsp;(*)</td><td style="width: 10px;"><% If role.RoleTitle.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf role.RoleTitle.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If role.RoleComment.Visible Then ' RoleComment %>
	<% If role.SortUrl(role.RoleComment) = "" Then %>
		<td>Role Comment</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= role.SortUrl(role.RoleComment) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Role Comment&nbsp;(*)</td><td style="width: 10px;"><% If role.RoleComment.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf role.RoleComment.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If role.FilterMenu.Visible Then ' FilterMenu %>
	<% If role.SortUrl(role.FilterMenu) = "" Then %>
		<td>Filter Menu</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= role.SortUrl(role.FilterMenu) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Filter Menu</td><td style="width: 10px;"><% If role.FilterMenu.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf role.FilterMenu.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
	</tr>
</thead>
<tbody><!-- Table body -->
<%
If (role.ExportAll AndAlso role.Export <> "") Then
	role_list.lStopRec = role_list.lTotalRecs
Else
	role_list.lStopRec = role_list.lStartRec + role_list.lDisplayRecs - 1 ' Set the last record to display
End If
If role.CurrentAction = "gridadd" AndAlso role_list.lStopRec = -1 Then
	role_list.lStopRec = EW_GRIDADD_ROWS
End If 

' Move to first record
For i As Integer = 1 to role_list.lStartRec - 1
	If Rs.Read() Then	role_list.lRecCnt = role_list.lRecCnt + 1
Next		
role_list.lRowCnt = 0

' Output data rows
Do While (role.CurrentAction = "gridadd" OrElse Rs.Read()) AndAlso (role_list.lRecCnt < role_list.lStopRec)
	role_list.lRecCnt = role_list.lRecCnt + 1
	If role_list.lRecCnt >= role_list.lStartRec Then
		role_list.lRowCnt = role_list.lRowCnt + 1
	role.CssClass = ""
	role.CssStyle = ""
	role.RowClientEvents = "onmouseover='ew_MouseOver(event, this);' onmouseout='ew_MouseOut(event, this);' onclick='ew_Click(event, this);'"
	If role.CurrentAction = "gridadd" Then
		role_list.LoadDefaultValues() ' Load default values
	Else
		role_list.LoadRowValues(Rs) ' Load row values
	End If
	role.RowType = EW_ROWTYPE_VIEW ' Render view

	' Render row
	role_list.RenderRow()
%>
	<tr<%= role.RowAttributes %>>
<% If role.Export = "" Then %>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= role.ViewUrl %>"><img src='images/view.gif' alt='View' title='View' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= role.EditUrl %>"><img src='images/edit.gif' alt='Edit' title='Edit' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= role.CopyUrl %>"><img src='images/copy.gif' alt='Copy' title='Copy' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= role.DeleteUrl %>"><img src='images/delete.gif' alt='Delete' title='Delete' width='16' height='16' border='0'></a>
</span></td>
<%

' Custom list options
For i As Integer = 0 to role_list.ListOptions.Items.Count -1
	If role_list.ListOptions.Items(i).Visible Then Response.Write(role_list.ListOptions.Items(i).BodyCellHtml)
Next
%>
<% End If %>
	<% If role.RoleID.Visible Then ' RoleID %>
		<td<%= role.RoleID.CellAttributes %>>
<div<%= role.RoleID.ViewAttributes %>><%= role.RoleID.ListViewValue %></div>
</td>
	<% End If %>
	<% If role.RoleName.Visible Then ' RoleName %>
		<td<%= role.RoleName.CellAttributes %>>
<div<%= role.RoleName.ViewAttributes %>><%= role.RoleName.ListViewValue %></div>
</td>
	<% End If %>
	<% If role.RoleTitle.Visible Then ' RoleTitle %>
		<td<%= role.RoleTitle.CellAttributes %>>
<div<%= role.RoleTitle.ViewAttributes %>><%= role.RoleTitle.ListViewValue %></div>
</td>
	<% End If %>
	<% If role.RoleComment.Visible Then ' RoleComment %>
		<td<%= role.RoleComment.CellAttributes %>>
<div<%= role.RoleComment.ViewAttributes %>><%= role.RoleComment.ListViewValue %></div>
</td>
	<% End If %>
	<% If role.FilterMenu.Visible Then ' FilterMenu %>
		<td<%= role.FilterMenu.CellAttributes %>>
<% If Convert.ToString(role.FilterMenu.CurrentValue) = "1" Then %>
<input type="checkbox" value="<%= role.FilterMenu.ListViewValue %>" checked onclick="this.form.reset();" disabled="disabled" />
<% Else %>
<input type="checkbox" value="<%= role.FilterMenu.ListViewValue %>" onclick="this.form.reset();" disabled="disabled" />
<% End If %>
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
<% If role_list.lTotalRecs > 0 Then %>
<% If role.Export = "" Then %>
<div class="ewGridLowerPanel">
<% If role.CurrentAction <> "gridadd" AndAlso role.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If role_list.Pager Is Nothing Then role_list.Pager = New cPrevNextPager(role_list.lStartRec, role_list.lDisplayRecs, role_list.lTotalRecs) %>
<% If role_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If role_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= role_list.PageUrl %>start=<%= role_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If role_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= role_list.PageUrl %>start=<%= role_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= role_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If role_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= role_list.PageUrl %>start=<%= role_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If role_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= role_list.PageUrl %>start=<%= role_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= role_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= role_list.Pager.FromIndex %> to <%= role_list.Pager.ToIndex %> of <%= role_list.Pager.RecordCount %></span>
<% Else %>
	<% If role_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If role_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="role" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If role_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If role_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If role_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If role.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<% 'If role_list.lTotalRecs > 0 Then %>
<div class="aspnetmaker">
<a href="<%= role.AddUrl %>">Add</a>&nbsp;&nbsp;
</div>
<% 'End If %>
</div>
<% End If %>
<% End If %>
</td></tr></table>
<% If role.Export = "" AndAlso role.CurrentAction = "" Then %>
<script type="text/javascript">
<!--
//ew_ToggleSearchPanel(role_list); // uncomment to init search panel as collapsed
//-->
</script>
<% End If %>
<% If role.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
