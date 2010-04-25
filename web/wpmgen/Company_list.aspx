<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="Company_list.aspx.vb" Inherits="Company_list" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If Company.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var Company_list = new ew_Page("Company_list");
// page properties
Company_list.PageID = "list"; // page ID
var EW_PAGE_ID = Company_list.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
Company_list.ValidateRequired = true; // uses JavaScript validation
<% Else %>
Company_list.ValidateRequired = false; // no JavaScript validation
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
<% If Company.Export = "" Then %>
<% End If %>
<%

' Load recordset
Rs = Company_list.LoadRecordset()
	Company_list.lStartRec = 1
	If Company_list.lDisplayRecs <= 0 Then ' Display all records
		Company_list.lDisplayRecs = Company_list.lTotalRecs
	End If
	If Not (Company.ExportAll AndAlso Company.Export <> "") Then
		Company_list.SetUpStartRec() ' Set up start record position
	End If
%>
<p><span class="aspnetmaker" style="white-space: nowrap;">TABLE: Site
<% If Company.Export = "" AndAlso Company.CurrentAction = "" Then %>
&nbsp;&nbsp;<a href="<%= Company_list.PageUrl %>export=excel"><img src='images/exportxls.gif' alt='Export to Excel' title='Export to Excel' width='16' height='16' border='0'></a>
<% End If %>
</span></p>
<% If Company.Export = "" AndAlso Company.CurrentAction = "" Then %>
<a href="javascript:ew_ToggleSearchPanel(Company_list);" style="text-decoration: none;"><img id="Company_list_SearchImage" src="images/collapse.gif" alt="" width="9" height="9" border="0"></a><span class="aspnetmaker">&nbsp;Search</span><br>
<div id="Company_list_SearchPanel">
<form name="fCompanylistsrch" id="fCompanylistsrch" class="ewForm">
<input type="hidden" id="t" name="t" value="Company" />
<table class="ewBasicSearch">
	<tr>
		<td><span class="aspnetmaker">
			<input type="text" name="<%= EW_TABLE_BASIC_SEARCH %>" id="<%= EW_TABLE_BASIC_SEARCH %>" size="20" value="<%= ew_HtmlEncode(Company.BasicSearchKeyword) %>" />
			<input type="Submit" name="Submit" id="Submit" value="Search (*)" />&nbsp;
			<a href="<%= Company_list.PageUrl %>cmd=reset">Show all</a>&nbsp;
		</span></td>
	</tr>
	<tr>
	<td><span class="aspnetmaker"><label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value=""<% If Company.BasicSearchType = "" Then %> checked="checked"<% End If %> />Exact phrase</label>&nbsp;&nbsp;<label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value="AND"<% If Company.BasicSearchType = "AND" Then %> checked="checked"<% End If %> />All words</label>&nbsp;&nbsp;<label><input type="radio" name="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" id="<%= EW_TABLE_BASIC_SEARCH_TYPE %>" value="OR"<% If Company.BasicSearchType = "OR" Then %> checked="checked"<% End If %> />Any word</label></span></td>
	</tr>
</table>
</form>
</div>
<% End If %>
<% Company_list.ShowMessage() %>
<br />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<% If Company.Export = "" Then %>
<div class="ewGridUpperPanel">
<% If Company.CurrentAction <> "gridadd" AndAlso Company.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If Company_list.Pager Is Nothing Then Company_list.Pager = New cPrevNextPager(Company_list.lStartRec, Company_list.lDisplayRecs, Company_list.lTotalRecs) %>
<% If Company_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If Company_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= Company_list.PageUrl %>start=<%= Company_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If Company_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= Company_list.PageUrl %>start=<%= Company_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= Company_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If Company_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= Company_list.PageUrl %>start=<%= Company_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If Company_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= Company_list.PageUrl %>start=<%= Company_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= Company_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= Company_list.Pager.FromIndex %> to <%= Company_list.Pager.ToIndex %> of <%= Company_list.Pager.RecordCount %></span>
<% Else %>
	<% If Company_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If Company_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="Company" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If Company_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If Company_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If Company_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If Company.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<div class="aspnetmaker">
<a href="<%= Company.AddUrl %>">Add</a>&nbsp;&nbsp;
</div>
</div>
<% End If %>
<div class="ewGridMiddlePanel">
<form name="fCompanylist" id="fCompanylist" class="ewForm" method="post">
<% If Company_list.lTotalRecs > 0 Then %>
<table cellspacing="0" rowhighlightclass="ewTableHighlightRow" rowselectclass="ewTableSelectRow" roweditclass="ewTableEditRow" class="ewTable ewTableSeparate">
<%
	Company_list.lOptionCnt = 0
	Company_list.lOptionCnt = Company_list.lOptionCnt + 1 ' View
	Company_list.lOptionCnt = Company_list.lOptionCnt + 1 ' Edit
	Company_list.lOptionCnt = Company_list.lOptionCnt + 1 ' Copy
	Company_list.lOptionCnt = Company_list.lOptionCnt + Company_list.ListOptions.Items.Count ' Custom list options
%>
<%= Company.TableCustomInnerHTML %>
<thead><!-- Table header -->
	<tr class="ewTableHeader">
<% If Company.Export = "" Then %>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<%

' Custom list options
For i As Integer = 0 to Company_list.ListOptions.Items.Count -1
	If Company_list.ListOptions.Items(i).Visible Then Response.Write(Company_list.ListOptions.Items(i).HeaderCellHtml)
Next
%>
<% End If %>
<% If Company.CompanyName.Visible Then ' CompanyName %>
	<% If Company.SortUrl(Company.CompanyName) = "" Then %>
		<td>Company Name</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= Company.SortUrl(Company.CompanyName) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Company Name&nbsp;(*)</td><td style="width: 10px;"><% If Company.CompanyName.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Company.CompanyName.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If Company.SiteTitle.Visible Then ' SiteTitle %>
	<% If Company.SortUrl(Company.SiteTitle) = "" Then %>
		<td>Site Title</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= Company.SortUrl(Company.SiteTitle) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Site Title&nbsp;(*)</td><td style="width: 10px;"><% If Company.SiteTitle.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Company.SiteTitle.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If Company.SiteURL.Visible Then ' SiteURL %>
	<% If Company.SortUrl(Company.SiteURL) = "" Then %>
		<td>Site URL</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= Company.SortUrl(Company.SiteURL) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Site URL&nbsp;(*)</td><td style="width: 10px;"><% If Company.SiteURL.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Company.SiteURL.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If Company.SiteTemplate.Visible Then ' SiteTemplate %>
	<% If Company.SortUrl(Company.SiteTemplate) = "" Then %>
		<td>Template</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= Company.SortUrl(Company.SiteTemplate) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Template</td><td style="width: 10px;"><% If Company.SiteTemplate.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Company.SiteTemplate.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If Company.DefaultSiteTemplate.Visible Then ' DefaultSiteTemplate %>
	<% If Company.SortUrl(Company.DefaultSiteTemplate) = "" Then %>
		<td>Default Template</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= Company.SortUrl(Company.DefaultSiteTemplate) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Default Template</td><td style="width: 10px;"><% If Company.DefaultSiteTemplate.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Company.DefaultSiteTemplate.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If Company.ActiveFL.Visible Then ' ActiveFL %>
	<% If Company.SortUrl(Company.ActiveFL) = "" Then %>
		<td>Active FL</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= Company.SortUrl(Company.ActiveFL) %>',1);">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Active FL</td><td style="width: 10px;"><% If Company.ActiveFL.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Company.ActiveFL.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
	</tr>
</thead>
<tbody><!-- Table body -->
<%
If (Company.ExportAll AndAlso Company.Export <> "") Then
	Company_list.lStopRec = Company_list.lTotalRecs
Else
	Company_list.lStopRec = Company_list.lStartRec + Company_list.lDisplayRecs - 1 ' Set the last record to display
End If
If Company.CurrentAction = "gridadd" AndAlso Company_list.lStopRec = -1 Then
	Company_list.lStopRec = EW_GRIDADD_ROWS
End If 

' Move to first record
For i As Integer = 1 to Company_list.lStartRec - 1
	If Rs.Read() Then	Company_list.lRecCnt = Company_list.lRecCnt + 1
Next		
Company_list.lRowCnt = 0

' Output data rows
Do While (Company.CurrentAction = "gridadd" OrElse Rs.Read()) AndAlso (Company_list.lRecCnt < Company_list.lStopRec)
	Company_list.lRecCnt = Company_list.lRecCnt + 1
	If Company_list.lRecCnt >= Company_list.lStartRec Then
		Company_list.lRowCnt = Company_list.lRowCnt + 1
	Company.CssClass = ""
	Company.CssStyle = ""
	Company.RowClientEvents = "onmouseover='ew_MouseOver(event, this);' onmouseout='ew_MouseOut(event, this);' onclick='ew_Click(event, this);'"
	If Company.CurrentAction = "gridadd" Then
		Company_list.LoadDefaultValues() ' Load default values
	Else
		Company_list.LoadRowValues(Rs) ' Load row values
	End If
	Company.RowType = EW_ROWTYPE_VIEW ' Render view

	' Render row
	Company_list.RenderRow()
%>
	<tr<%= Company.RowAttributes %>>
<% If Company.Export = "" Then %>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= Company.ViewUrl %>"><img src='images/view.gif' alt='View' title='View' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= Company.EditUrl %>"><img src='images/edit.gif' alt='Edit' title='Edit' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= Company.CopyUrl %>"><img src='images/copy.gif' alt='Copy' title='Copy' width='16' height='16' border='0'></a>
</span></td>
<%

' Custom list options
For i As Integer = 0 to Company_list.ListOptions.Items.Count -1
	If Company_list.ListOptions.Items(i).Visible Then Response.Write(Company_list.ListOptions.Items(i).BodyCellHtml)
Next
%>
<% End If %>
	<% If Company.CompanyName.Visible Then ' CompanyName %>
		<td<%= Company.CompanyName.CellAttributes %>>
<div<%= Company.CompanyName.ViewAttributes %>><%= Company.CompanyName.ListViewValue %></div>
</td>
	<% End If %>
	<% If Company.SiteTitle.Visible Then ' SiteTitle %>
		<td<%= Company.SiteTitle.CellAttributes %>>
<div<%= Company.SiteTitle.ViewAttributes %>><%= Company.SiteTitle.ListViewValue %></div>
</td>
	<% End If %>
	<% If Company.SiteURL.Visible Then ' SiteURL %>
		<td<%= Company.SiteURL.CellAttributes %>>
<div<%= Company.SiteURL.ViewAttributes %>><%= Company.SiteURL.ListViewValue %></div>
</td>
	<% End If %>
	<% If Company.SiteTemplate.Visible Then ' SiteTemplate %>
		<td<%= Company.SiteTemplate.CellAttributes %>>
<div<%= Company.SiteTemplate.ViewAttributes %>><%= Company.SiteTemplate.ListViewValue %></div>
</td>
	<% End If %>
	<% If Company.DefaultSiteTemplate.Visible Then ' DefaultSiteTemplate %>
		<td<%= Company.DefaultSiteTemplate.CellAttributes %>>
<div<%= Company.DefaultSiteTemplate.ViewAttributes %>><%= Company.DefaultSiteTemplate.ListViewValue %></div>
</td>
	<% End If %>
	<% If Company.ActiveFL.Visible Then ' ActiveFL %>
		<td<%= Company.ActiveFL.CellAttributes %>>
<% If Convert.ToString(Company.ActiveFL.CurrentValue) = "1" Then %>
<input type="checkbox" value="<%= Company.ActiveFL.ListViewValue %>" checked onclick="this.form.reset();" disabled="disabled" />
<% Else %>
<input type="checkbox" value="<%= Company.ActiveFL.ListViewValue %>" onclick="this.form.reset();" disabled="disabled" />
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
<% If Company_list.lTotalRecs > 0 Then %>
<% If Company.Export = "" Then %>
<div class="ewGridLowerPanel">
<% If Company.CurrentAction <> "gridadd" AndAlso Company.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If Company_list.Pager Is Nothing Then Company_list.Pager = New cPrevNextPager(Company_list.lStartRec, Company_list.lDisplayRecs, Company_list.lTotalRecs) %>
<% If Company_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If Company_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= Company_list.PageUrl %>start=<%= Company_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If Company_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= Company_list.PageUrl %>start=<%= Company_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= Company_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If Company_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= Company_list.PageUrl %>start=<%= Company_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If Company_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= Company_list.PageUrl %>start=<%= Company_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= Company_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= Company_list.Pager.FromIndex %> to <%= Company_list.Pager.ToIndex %> of <%= Company_list.Pager.RecordCount %></span>
<% Else %>
	<% If Company_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If Company_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="Company" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If Company_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If Company_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If Company_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If Company.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<% 'If Company_list.lTotalRecs > 0 Then %>
<div class="aspnetmaker">
<a href="<%= Company.AddUrl %>">Add</a>&nbsp;&nbsp;
</div>
<% 'End If %>
</div>
<% End If %>
<% End If %>
</td></tr></table>
<% If Company.Export = "" AndAlso Company.CurrentAction = "" Then %>
<script type="text/javascript">
<!--
//ew_ToggleSearchPanel(Company_list); // uncomment to init search panel as collapsed
//-->
</script>
<% End If %>
<% If Company.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
