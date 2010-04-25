<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="PageRole_view.aspx.vb" Inherits="PageRole_view" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If PageRole.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var PageRole_view = new ew_Page("PageRole_view");
// page properties
PageRole_view.PageID = "view"; // page ID
var EW_PAGE_ID = PageRole_view.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
PageRole_view.ValidateRequired = true; // uses JavaScript validation
<% Else %>
PageRole_view.ValidateRequired = false; // no JavaScript validation
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
<script language="JavaScript" type="text/javascript">
<!--
// Write your client script here, no need to add script tags.
// To include another .js script, use:
// ew_ClientScriptInclude("my_javascript.js"); 
//-->
</script>
<% End If %>
<p><span class="aspnetmaker">View TABLE: Location Role
<br /><br />
<% If PageRole.Export = "" Then %>
<a href="PageRole_list.aspx">Back to List</a>&nbsp;
<a href="<%= PageRole.AddUrl %>">Add</a>&nbsp;
<a href="<%= PageRole.EditUrl %>">Edit</a>&nbsp;
<a href="<%= PageRole.CopyUrl %>">Copy</a>&nbsp;
<a href="<%= PageRole.DeleteUrl %>">Delete</a>&nbsp;
<% End If %>
</span></p>
<% PageRole_view.ShowMessage() %>
<p />
<% If PageRole.Export = "" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If PageRole_view.Pager Is Nothing Then PageRole_view.Pager = New cPrevNextPager(PageRole_view.lStartRec, PageRole_view.lDisplayRecs, PageRole_view.lTotalRecs) %>
<% If PageRole_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If PageRole_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= PageRole_view.PageUrl %>start=<%= PageRole_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If PageRole_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= PageRole_view.PageUrl %>start=<%= PageRole_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= PageRole_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If PageRole_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= PageRole_view.PageUrl %>start=<%= PageRole_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If PageRole_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= PageRole_view.PageUrl %>start=<%= PageRole_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= PageRole_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If PageRole_view.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
	</tr>
</table>
</form>
<br />
<% End If %>
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
<% If PageRole.PageRoleID.Visible Then ' PageRoleID %>
	<tr<%= PageRole.PageRoleID.RowAttributes %>>
		<td class="ewTableHeader">Page Role ID</td>
		<td<%= PageRole.PageRoleID.CellAttributes %>>
<div<%= PageRole.PageRoleID.ViewAttributes %>><%= PageRole.PageRoleID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If PageRole.RoleID.Visible Then ' RoleID %>
	<tr<%= PageRole.RoleID.RowAttributes %>>
		<td class="ewTableHeader">Role ID</td>
		<td<%= PageRole.RoleID.CellAttributes %>>
<div<%= PageRole.RoleID.ViewAttributes %>><%= PageRole.RoleID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If PageRole.zPageID.Visible Then ' PageID %>
	<tr<%= PageRole.zPageID.RowAttributes %>>
		<td class="ewTableHeader">Page ID</td>
		<td<%= PageRole.zPageID.CellAttributes %>>
<div<%= PageRole.zPageID.ViewAttributes %>><%= PageRole.zPageID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If PageRole.CompanyID.Visible Then ' CompanyID %>
	<tr<%= PageRole.CompanyID.RowAttributes %>>
		<td class="ewTableHeader">Company ID</td>
		<td<%= PageRole.CompanyID.CellAttributes %>>
<div<%= PageRole.CompanyID.ViewAttributes %>><%= PageRole.CompanyID.ViewValue %></div>
</td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<% If PageRole.Export = "" Then %>
<br />
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If PageRole_view.Pager Is Nothing Then PageRole_view.Pager = New cPrevNextPager(PageRole_view.lStartRec, PageRole_view.lDisplayRecs, PageRole_view.lTotalRecs) %>
<% If PageRole_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If PageRole_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= PageRole_view.PageUrl %>start=<%= PageRole_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If PageRole_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= PageRole_view.PageUrl %>start=<%= PageRole_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= PageRole_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If PageRole_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= PageRole_view.PageUrl %>start=<%= PageRole_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If PageRole_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= PageRole_view.PageUrl %>start=<%= PageRole_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= PageRole_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If PageRole_view.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
	</tr>
</table>
</form>
<% End If %>
<p />
<% If PageRole.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
