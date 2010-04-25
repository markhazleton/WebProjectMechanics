<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="SiteTemplate_list.aspx.vb" Inherits="SiteTemplate_list" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If SiteTemplate.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var SiteTemplate_list = new ew_Page("SiteTemplate_list");
// page properties
SiteTemplate_list.PageID = "list"; // page ID
var EW_PAGE_ID = SiteTemplate_list.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
SiteTemplate_list.ValidateRequired = true; // uses JavaScript validation
<% Else %>
SiteTemplate_list.ValidateRequired = false; // no JavaScript validation
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
<% If SiteTemplate.Export = "" Then %>
<% End If %>
<%

' Load recordset
Rs = SiteTemplate_list.LoadRecordset()
	SiteTemplate_list.lStartRec = 1
	If SiteTemplate_list.lDisplayRecs <= 0 Then ' Display all records
		SiteTemplate_list.lDisplayRecs = SiteTemplate_list.lTotalRecs
	End If
	If Not (SiteTemplate.ExportAll AndAlso SiteTemplate.Export <> "") Then
		SiteTemplate_list.SetUpStartRec() ' Set up start record position
	End If
%>
<p><span class="aspnetmaker" style="white-space: nowrap;">TABLE: Presentation Template (skin)
<% If SiteTemplate.Export = "" AndAlso SiteTemplate.CurrentAction = "" Then %>
&nbsp;&nbsp;<a href="<%= SiteTemplate_list.PageUrl %>export=excel"><img src='images/exportxls.gif' alt='Export to Excel' title='Export to Excel' width='16' height='16' border='0'></a>
<% End If %>
</span></p>
<% If SiteTemplate.Export = "" AndAlso SiteTemplate.CurrentAction = "" Then %>
<a href="javascript:ew_ToggleSearchPanel(SiteTemplate_list);" style="text-decoration: none;"><img id="SiteTemplate_list_SearchImage" src="images/collapse.gif" alt="" width="9" height="9" border="0"></a><span class="aspnetmaker">&nbsp;Search</span><br>
<div id="SiteTemplate_list_SearchPanel">
<form name="fSiteTemplatelistsrch" id="fSiteTemplatelistsrch" class="ewForm">
<input type="hidden" id="t" name="t" value="SiteTemplate" />
<table class="ewBasicSearch">
	<tr>
		<td><span class="aspnetmaker">
			<input type="text" name="<%= EW_TABLE_BASIC_SEARCH %>" id="<%= EW_TABLE_BASIC_SEARCH %>" size="20" value="<%= ew_HtmlEncode(SiteTemplate.BasicSearchKeyword) %>" />
			<input type="Submit" name="Submit" id="Submit" value="Search (*)" />&nbsp;
			<a href="<%= SiteTemplate_list.PageUrl %>cmd=reset">Show all</a>&nbsp;
		</span></td>
	</tr>
	<tr>
	<td><span class="aspnetmaker"><label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value=""<% If SiteTemplate.BasicSearchType = "" Then %> checked="checked"<% End If %> />Exact phrase</label>&nbsp;&nbsp;<label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value="AND"<% If SiteTemplate.BasicSearchType = "AND" Then %> checked="checked"<% End If %> />All words</label>&nbsp;&nbsp;<label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value="OR"<% If SiteTemplate.BasicSearchType = "OR" Then %> checked="checked"<% End If %> />Any word</label></span></td>
	</tr>
</table>
</form>
</div>
<% End If %>
<% SiteTemplate_list.ShowMessage() %>
<br />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<% If SiteTemplate.Export = "" Then %>
<div class="ewGridUpperPanel">
<% If SiteTemplate.CurrentAction <> "gridadd" AndAlso SiteTemplate.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If SiteTemplate_list.Pager Is Nothing Then SiteTemplate_list.Pager = New cPrevNextPager(SiteTemplate_list.lStartRec, SiteTemplate_list.lDisplayRecs, SiteTemplate_list.lTotalRecs) %>
<% If SiteTemplate_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If SiteTemplate_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= SiteTemplate_list.PageUrl %>start=<%= SiteTemplate_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If SiteTemplate_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= SiteTemplate_list.PageUrl %>start=<%= SiteTemplate_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= SiteTemplate_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If SiteTemplate_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= SiteTemplate_list.PageUrl %>start=<%= SiteTemplate_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If SiteTemplate_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= SiteTemplate_list.PageUrl %>start=<%= SiteTemplate_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= SiteTemplate_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= SiteTemplate_list.Pager.FromIndex %> to <%= SiteTemplate_list.Pager.ToIndex %> of <%= SiteTemplate_list.Pager.RecordCount %></span>
<% Else %>
	<% If SiteTemplate_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If SiteTemplate_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="SiteTemplate" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If SiteTemplate_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If SiteTemplate_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If SiteTemplate_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If SiteTemplate.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<div class="aspnetmaker">
<a href="<%= SiteTemplate.AddUrl %>">Add</a>&nbsp;&nbsp;
</div>
</div>
<% End If %>
<div class="ewGridMiddlePanel">
<form name="fSiteTemplatelist" id="fSiteTemplatelist" class="ewForm" method="post">
<% If SiteTemplate_list.lTotalRecs > 0 Then %>
<table cellspacing="0" rowhighlightclass="ewTableHighlightRow" rowselectclass="ewTableSelectRow" roweditclass="ewTableEditRow" class="ewTable ewTableSeparate">
<%
	SiteTemplate_list.lOptionCnt = 0
	SiteTemplate_list.lOptionCnt = SiteTemplate_list.lOptionCnt + 1 ' View
	SiteTemplate_list.lOptionCnt = SiteTemplate_list.lOptionCnt + 1 ' Edit
	SiteTemplate_list.lOptionCnt = SiteTemplate_list.lOptionCnt + 1 ' Copy
	SiteTemplate_list.lOptionCnt = SiteTemplate_list.lOptionCnt + 1 ' Delete
	SiteTemplate_list.lOptionCnt = SiteTemplate_list.lOptionCnt + SiteTemplate_list.ListOptions.Items.Count ' Custom list options
%>
<%= SiteTemplate.TableCustomInnerHTML %>
<thead><!-- Table header -->
	<tr class="ewTableHeader">
<% If SiteTemplate.Export = "" Then %>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<%

' Custom list options
For i As Integer = 0 to SiteTemplate_list.ListOptions.Items.Count -1
	If SiteTemplate_list.ListOptions.Items(i).Visible Then Response.Write(SiteTemplate_list.ListOptions.Items(i).HeaderCellHtml)
Next
%>
<% End If %>
<% If SiteTemplate.TemplatePrefix.Visible Then ' TemplatePrefix %>
	<% If SiteTemplate.SortUrl(SiteTemplate.TemplatePrefix) = "" Then %>
		<td>Template Prefix</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteTemplate.SortUrl(SiteTemplate.TemplatePrefix) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Template Prefix&nbsp;(*)</td><td style="width: 10px;"><% If SiteTemplate.TemplatePrefix.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteTemplate.TemplatePrefix.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If SiteTemplate.zName.Visible Then ' Name %>
	<% If SiteTemplate.SortUrl(SiteTemplate.zName) = "" Then %>
		<td>Name</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= SiteTemplate.SortUrl(SiteTemplate.zName) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Name&nbsp;(*)</td><td style="width: 10px;"><% If SiteTemplate.zName.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf SiteTemplate.zName.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
	</tr>
</thead>
<tbody><!-- Table body -->
<%
If (SiteTemplate.ExportAll AndAlso SiteTemplate.Export <> "") Then
	SiteTemplate_list.lStopRec = SiteTemplate_list.lTotalRecs
Else
	SiteTemplate_list.lStopRec = SiteTemplate_list.lStartRec + SiteTemplate_list.lDisplayRecs - 1 ' Set the last record to display
End If
If SiteTemplate.CurrentAction = "gridadd" AndAlso SiteTemplate_list.lStopRec = -1 Then
	SiteTemplate_list.lStopRec = EW_GRIDADD_ROWS
End If 

' Move to first record
For i As Integer = 1 to SiteTemplate_list.lStartRec - 1
	If Rs.Read() Then	SiteTemplate_list.lRecCnt = SiteTemplate_list.lRecCnt + 1
Next		
SiteTemplate_list.lRowCnt = 0

' Output data rows
Do While (SiteTemplate.CurrentAction = "gridadd" OrElse Rs.Read()) AndAlso (SiteTemplate_list.lRecCnt < SiteTemplate_list.lStopRec)
	SiteTemplate_list.lRecCnt = SiteTemplate_list.lRecCnt + 1
	If SiteTemplate_list.lRecCnt >= SiteTemplate_list.lStartRec Then
		SiteTemplate_list.lRowCnt = SiteTemplate_list.lRowCnt + 1
	SiteTemplate.CssClass = ""
	SiteTemplate.CssStyle = ""
	SiteTemplate.RowClientEvents = "onmouseover='ew_MouseOver(event, this);' onmouseout='ew_MouseOut(event, this);' onclick='ew_Click(event, this);'"
	If SiteTemplate.CurrentAction = "gridadd" Then
		SiteTemplate_list.LoadDefaultValues() ' Load default values
	Else
		SiteTemplate_list.LoadRowValues(Rs) ' Load row values
	End If
	SiteTemplate.RowType = EW_ROWTYPE_VIEW ' Render view

	' Render row
	SiteTemplate_list.RenderRow()
%>
	<tr<%= SiteTemplate.RowAttributes %>>
<% If SiteTemplate.Export = "" Then %>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= SiteTemplate.ViewUrl %>"><img src='images/view.gif' alt='View' title='View' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= SiteTemplate.EditUrl %>"><img src='images/edit.gif' alt='Edit' title='Edit' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= SiteTemplate.CopyUrl %>"><img src='images/copy.gif' alt='Copy' title='Copy' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= SiteTemplate.DeleteUrl %>"><img src='images/delete.gif' alt='Delete' title='Delete' width='16' height='16' border='0'></a>
</span></td>
<%

' Custom list options
For i As Integer = 0 to SiteTemplate_list.ListOptions.Items.Count -1
	If SiteTemplate_list.ListOptions.Items(i).Visible Then Response.Write(SiteTemplate_list.ListOptions.Items(i).BodyCellHtml)
Next
%>
<% End If %>
	<% If SiteTemplate.TemplatePrefix.Visible Then ' TemplatePrefix %>
		<td<%= SiteTemplate.TemplatePrefix.CellAttributes %>>
<div<%= SiteTemplate.TemplatePrefix.ViewAttributes %>><%= SiteTemplate.TemplatePrefix.ListViewValue %></div>
</td>
	<% End If %>
	<% If SiteTemplate.zName.Visible Then ' Name %>
		<td<%= SiteTemplate.zName.CellAttributes %>>
<div<%= SiteTemplate.zName.ViewAttributes %>><%= SiteTemplate.zName.ListViewValue %></div>
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
<% If SiteTemplate_list.lTotalRecs > 0 Then %>
<% If SiteTemplate.Export = "" Then %>
<div class="ewGridLowerPanel">
<% If SiteTemplate.CurrentAction <> "gridadd" AndAlso SiteTemplate.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If SiteTemplate_list.Pager Is Nothing Then SiteTemplate_list.Pager = New cPrevNextPager(SiteTemplate_list.lStartRec, SiteTemplate_list.lDisplayRecs, SiteTemplate_list.lTotalRecs) %>
<% If SiteTemplate_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If SiteTemplate_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= SiteTemplate_list.PageUrl %>start=<%= SiteTemplate_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If SiteTemplate_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= SiteTemplate_list.PageUrl %>start=<%= SiteTemplate_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= SiteTemplate_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If SiteTemplate_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= SiteTemplate_list.PageUrl %>start=<%= SiteTemplate_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If SiteTemplate_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= SiteTemplate_list.PageUrl %>start=<%= SiteTemplate_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= SiteTemplate_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= SiteTemplate_list.Pager.FromIndex %> to <%= SiteTemplate_list.Pager.ToIndex %> of <%= SiteTemplate_list.Pager.RecordCount %></span>
<% Else %>
	<% If SiteTemplate_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If SiteTemplate_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="SiteTemplate" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If SiteTemplate_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If SiteTemplate_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If SiteTemplate_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If SiteTemplate.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<% 'If SiteTemplate_list.lTotalRecs > 0 Then %>
<div class="aspnetmaker">
<a href="<%= SiteTemplate.AddUrl %>">Add</a>&nbsp;&nbsp;
</div>
<% 'End If %>
</div>
<% End If %>
<% End If %>
</td></tr></table>
<% If SiteTemplate.Export = "" AndAlso SiteTemplate.CurrentAction = "" Then %>
<script type="text/javascript">
<!--
//ew_ToggleSearchPanel(SiteTemplate_list); // uncomment to init search panel as collapsed
//-->
</script>
<% End If %>
<% If SiteTemplate.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
