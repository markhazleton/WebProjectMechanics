<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="Contact_list.aspx.vb" Inherits="Contact_list" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If Contact.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var Contact_list = new ew_Page("Contact_list");
// page properties
Contact_list.PageID = "list"; // page ID
var EW_PAGE_ID = Contact_list.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
Contact_list.ValidateRequired = true; // uses JavaScript validation
<% Else %>
Contact_list.ValidateRequired = false; // no JavaScript validation
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
<% If Contact.Export = "" Then %>
<% End If %>
<%

' Load recordset
Rs = Contact_list.LoadRecordset()
	Contact_list.lStartRec = 1
	If Contact_list.lDisplayRecs <= 0 Then ' Display all records
		Contact_list.lDisplayRecs = Contact_list.lTotalRecs
	End If
	If Not (Contact.ExportAll AndAlso Contact.Export <> "") Then
		Contact_list.SetUpStartRec() ' Set up start record position
	End If
%>
<p><span class="aspnetmaker" style="white-space: nowrap;">TABLE: Site Contact
<% If Contact.Export = "" AndAlso Contact.CurrentAction = "" Then %>
&nbsp;&nbsp;<a href="<%= Contact_list.PageUrl %>export=excel"><img src='images/exportxls.gif' alt='Export to Excel' title='Export to Excel' width='16' height='16' border='0'></a>
<% End If %>
</span></p>
<% If Contact.Export = "" AndAlso Contact.CurrentAction = "" Then %>
<a href="javascript:ew_ToggleSearchPanel(Contact_list);" style="text-decoration: none;"><img id="Contact_list_SearchImage" src="images/collapse.gif" alt="" width="9" height="9" border="0"></a><span class="aspnetmaker">&nbsp;Search</span><br>
<div id="Contact_list_SearchPanel">
<form name="fContactlistsrch" id="fContactlistsrch" class="ewForm">
<input type="hidden" id="t" name="t" value="Contact" />
<table class="ewBasicSearch">
	<tr>
		<td><span class="aspnetmaker">
			<a href="<%= Contact_list.PageUrl %>cmd=reset">Show all</a>&nbsp;
			<a href="Contact_srch.aspx">Advanced Search</a>&nbsp;
		</span></td>
	</tr>
</table>
</form>
</div>
<% End If %>
<% Contact_list.ShowMessage() %>
<br />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<% If Contact.Export = "" Then %>
<div class="ewGridUpperPanel">
<% If Contact.CurrentAction <> "gridadd" AndAlso Contact.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If Contact_list.Pager Is Nothing Then Contact_list.Pager = New cPrevNextPager(Contact_list.lStartRec, Contact_list.lDisplayRecs, Contact_list.lTotalRecs) %>
<% If Contact_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If Contact_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= Contact_list.PageUrl %>start=<%= Contact_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If Contact_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= Contact_list.PageUrl %>start=<%= Contact_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= Contact_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If Contact_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= Contact_list.PageUrl %>start=<%= Contact_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If Contact_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= Contact_list.PageUrl %>start=<%= Contact_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= Contact_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= Contact_list.Pager.FromIndex %> to <%= Contact_list.Pager.ToIndex %> of <%= Contact_list.Pager.RecordCount %></span>
<% Else %>
	<% If Contact_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If Contact_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="Contact" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If Contact_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If Contact_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If Contact_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If Contact.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<div class="aspnetmaker">
<a href="<%= Contact.AddUrl %>">Add</a>&nbsp;&nbsp;
</div>
</div>
<% End If %>
<div class="ewGridMiddlePanel">
<form name="fContactlist" id="fContactlist" class="ewForm" method="post">
<% If Contact_list.lTotalRecs > 0 Then %>
<table cellspacing="0" rowhighlightclass="ewTableHighlightRow" rowselectclass="ewTableSelectRow" roweditclass="ewTableEditRow" class="ewTable ewTableSeparate">
<%
	Contact_list.lOptionCnt = 0
	Contact_list.lOptionCnt = Contact_list.lOptionCnt + 1 ' View
	Contact_list.lOptionCnt = Contact_list.lOptionCnt + 1 ' Edit
	Contact_list.lOptionCnt = Contact_list.lOptionCnt + 1 ' Copy
	Contact_list.lOptionCnt = Contact_list.lOptionCnt + 1 ' Delete
	Contact_list.lOptionCnt = Contact_list.lOptionCnt + Contact_list.ListOptions.Items.Count ' Custom list options
%>
<%= Contact.TableCustomInnerHTML %>
<thead><!-- Table header -->
	<tr class="ewTableHeader">
<% If Contact.Export = "" Then %>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<td style="white-space: nowrap;">&nbsp;</td>
<%

' Custom list options
For i As Integer = 0 to Contact_list.ListOptions.Items.Count -1
	If Contact_list.ListOptions.Items(i).Visible Then Response.Write(Contact_list.ListOptions.Items(i).HeaderCellHtml)
Next
%>
<% End If %>
<% If Contact.LogonName.Visible Then ' LogonName %>
	<% If Contact.SortUrl(Contact.LogonName) = "" Then %>
		<td style="white-space: nowrap;">Logon Name</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= Contact.SortUrl(Contact.LogonName) %>',1);" style="white-space: nowrap;">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Logon Name</td><td style="width: 10px;"><% If Contact.LogonName.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Contact.LogonName.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If Contact.PrimaryContact.Visible Then ' PrimaryContact %>
	<% If Contact.SortUrl(Contact.PrimaryContact) = "" Then %>
		<td style="white-space: nowrap;">Full Name</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= Contact.SortUrl(Contact.PrimaryContact) %>',1);" style="white-space: nowrap;">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Full Name</td><td style="width: 10px;"><% If Contact.PrimaryContact.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Contact.PrimaryContact.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If Contact.Active.Visible Then ' Active %>
	<% If Contact.SortUrl(Contact.Active) = "" Then %>
		<td style="white-space: nowrap;">Active</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= Contact.SortUrl(Contact.Active) %>',1);" style="white-space: nowrap;">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Active</td><td style="width: 10px;"><% If Contact.Active.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Contact.Active.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If Contact.CompanyID.Visible Then ' CompanyID %>
	<% If Contact.SortUrl(Contact.CompanyID) = "" Then %>
		<td style="white-space: nowrap;">Company</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= Contact.SortUrl(Contact.CompanyID) %>',1);" style="white-space: nowrap;">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Company</td><td style="width: 10px;"><% If Contact.CompanyID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Contact.CompanyID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
<% If Contact.GroupID.Visible Then ' GroupID %>
	<% If Contact.SortUrl(Contact.GroupID) = "" Then %>
		<td style="white-space: nowrap;">Group</td>
	<% Else %>
		<td class="ewPointer" onmousedown="ew_Sort(event,'<%= Contact.SortUrl(Contact.GroupID) %>',1);" style="white-space: nowrap;">
			<table cellspacing="0" class="ewTableHeaderBtn"><tr><td>Group</td><td style="width: 10px;"><% If Contact.GroupID.Sort = "ASC" Then %><img src="images/sortup.gif" width="10" height="9" border="0"><% ElseIf Contact.GroupID.Sort = "DESC" Then %><img src="images/sortdown.gif" width="10" height="9" border="0"><% End If %></td></tr></table>
		</td>
	<% End If %>
<% End If %>		
	</tr>
</thead>
<tbody><!-- Table body -->
<%
If (Contact.ExportAll AndAlso Contact.Export <> "") Then
	Contact_list.lStopRec = Contact_list.lTotalRecs
Else
	Contact_list.lStopRec = Contact_list.lStartRec + Contact_list.lDisplayRecs - 1 ' Set the last record to display
End If
If Contact.CurrentAction = "gridadd" AndAlso Contact_list.lStopRec = -1 Then
	Contact_list.lStopRec = EW_GRIDADD_ROWS
End If 

' Move to first record
For i As Integer = 1 to Contact_list.lStartRec - 1
	If Rs.Read() Then	Contact_list.lRecCnt = Contact_list.lRecCnt + 1
Next		
Contact_list.lRowCnt = 0

' Output data rows
Do While (Contact.CurrentAction = "gridadd" OrElse Rs.Read()) AndAlso (Contact_list.lRecCnt < Contact_list.lStopRec)
	Contact_list.lRecCnt = Contact_list.lRecCnt + 1
	If Contact_list.lRecCnt >= Contact_list.lStartRec Then
		Contact_list.lRowCnt = Contact_list.lRowCnt + 1
	Contact.CssClass = ""
	Contact.CssStyle = ""
	Contact.RowClientEvents = "onmouseover='ew_MouseOver(event, this);' onmouseout='ew_MouseOut(event, this);' onclick='ew_Click(event, this);'"
	If Contact.CurrentAction = "gridadd" Then
		Contact_list.LoadDefaultValues() ' Load default values
	Else
		Contact_list.LoadRowValues(Rs) ' Load row values
	End If
	Contact.RowType = EW_ROWTYPE_VIEW ' Render view

	' Render row
	Contact_list.RenderRow()
%>
	<tr<%= Contact.RowAttributes %>>
<% If Contact.Export = "" Then %>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= Contact.ViewUrl %>"><img src='images/view.gif' alt='View' title='View' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= Contact.EditUrl %>"><img src='images/edit.gif' alt='Edit' title='Edit' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= Contact.CopyUrl %>"><img src='images/copy.gif' alt='Copy' title='Copy' width='16' height='16' border='0'></a>
</span></td>
<td style="white-space: nowrap;"><span class="aspnetmaker">
<a href="<%= Contact.DeleteUrl %>"><img src='images/delete.gif' alt='Delete' title='Delete' width='16' height='16' border='0'></a>
</span></td>
<%

' Custom list options
For i As Integer = 0 to Contact_list.ListOptions.Items.Count -1
	If Contact_list.ListOptions.Items(i).Visible Then Response.Write(Contact_list.ListOptions.Items(i).BodyCellHtml)
Next
%>
<% End If %>
	<% If Contact.LogonName.Visible Then ' LogonName %>
		<td<%= Contact.LogonName.CellAttributes %>>
<div<%= Contact.LogonName.ViewAttributes %>><%= Contact.LogonName.ListViewValue %></div>
</td>
	<% End If %>
	<% If Contact.PrimaryContact.Visible Then ' PrimaryContact %>
		<td<%= Contact.PrimaryContact.CellAttributes %>>
<div<%= Contact.PrimaryContact.ViewAttributes %>><%= Contact.PrimaryContact.ListViewValue %></div>
</td>
	<% End If %>
	<% If Contact.Active.Visible Then ' Active %>
		<td<%= Contact.Active.CellAttributes %>>
<% If Convert.ToString(Contact.Active.CurrentValue) = "1" Then %>
<input type="checkbox" value="<%= Contact.Active.ListViewValue %>" checked onclick="this.form.reset();" disabled="disabled" />
<% Else %>
<input type="checkbox" value="<%= Contact.Active.ListViewValue %>" onclick="this.form.reset();" disabled="disabled" />
<% End If %>
</td>
	<% End If %>
	<% If Contact.CompanyID.Visible Then ' CompanyID %>
		<td<%= Contact.CompanyID.CellAttributes %>>
<div<%= Contact.CompanyID.ViewAttributes %>><%= Contact.CompanyID.ListViewValue %></div>
</td>
	<% End If %>
	<% If Contact.GroupID.Visible Then ' GroupID %>
		<td<%= Contact.GroupID.CellAttributes %>>
<div<%= Contact.GroupID.ViewAttributes %>><%= Contact.GroupID.ListViewValue %></div>
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
<% If Contact_list.lTotalRecs > 0 Then %>
<% If Contact.Export = "" Then %>
<div class="ewGridLowerPanel">
<% If Contact.CurrentAction <> "gridadd" AndAlso Contact.CurrentAction <> "gridedit" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If Contact_list.Pager Is Nothing Then Contact_list.Pager = New cPrevNextPager(Contact_list.lStartRec, Contact_list.lDisplayRecs, Contact_list.lTotalRecs) %>
<% If Contact_list.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If Contact_list.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= Contact_list.PageUrl %>start=<%= Contact_list.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If Contact_list.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= Contact_list.PageUrl %>start=<%= Contact_list.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= Contact_list.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If Contact_list.Pager.NextButton.Enabled Then %>
	<td><a href="<%= Contact_list.PageUrl %>start=<%= Contact_list.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If Contact_list.Pager.LastButton.Enabled Then %>
	<td><a href="<%= Contact_list.PageUrl %>start=<%= Contact_list.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= Contact_list.Pager.PageCount %></span></td>
	</tr></table>
	</td>	
	<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
	<td>
	<span class="aspnetmaker">Records <%= Contact_list.Pager.FromIndex %> to <%= Contact_list.Pager.ToIndex %> of <%= Contact_list.Pager.RecordCount %></span>
<% Else %>
	<% If Contact_list.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
<% If Contact_list.lTotalRecs > 0 Then %>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><table border="0" cellspacing="0" cellpadding="0"><tr><td>Page Size&nbsp;</td><td>
<input type="hidden" id="t" name="t" value="Contact" />
<select name="<%= EW_TABLE_REC_PER_PAGE %>" id="<%= EW_TABLE_REC_PER_PAGE %>" onchange="this.form.submit();" class="aspnetmaker">
<option value="10"<% If Contact_list.lDisplayRecs = 10 Then %> selected="selected"<% End If %>>10</option>
<option value="25"<% If Contact_list.lDisplayRecs = 25 Then %> selected="selected"<% End If %>>25</option>
<option value="50"<% If Contact_list.lDisplayRecs = 50 Then %> selected="selected"<% End If %>>50</option>
<option value="ALL"<% If Contact.RecordsPerPage = -1 Then %> selected="selected"<% End If %>>All</option>
</select></td></tr></table>
		</td>
<% End If %>
	</tr>
</table>
</form>
<% End If %>
<% 'If Contact_list.lTotalRecs > 0 Then %>
<div class="aspnetmaker">
<a href="<%= Contact.AddUrl %>">Add</a>&nbsp;&nbsp;
</div>
<% 'End If %>
</div>
<% End If %>
<% End If %>
</td></tr></table>
<% If Contact.Export = "" AndAlso Contact.CurrentAction = "" Then %>
<script type="text/javascript">
<!--
//ew_ToggleSearchPanel(Contact_list); // uncomment to init search panel as collapsed
//-->
</script>
<% End If %>
<% If Contact.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
