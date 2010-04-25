<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="role_view.aspx.vb" Inherits="role_view" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If role.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var role_view = new ew_Page("role_view");
// page properties
role_view.PageID = "view"; // page ID
var EW_PAGE_ID = role_view.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
role_view.ValidateRequired = true; // uses JavaScript validation
<% Else %>
role_view.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">View TABLE: Contact Role
<br /><br />
<% If role.Export = "" Then %>
<a href="role_list.aspx">Back to List</a>&nbsp;
<a href="<%= role.AddUrl %>">Add</a>&nbsp;
<a href="<%= role.EditUrl %>">Edit</a>&nbsp;
<a href="<%= role.CopyUrl %>">Copy</a>&nbsp;
<a href="<%= role.DeleteUrl %>">Delete</a>&nbsp;
<% End If %>
</span></p>
<% role_view.ShowMessage() %>
<p />
<% If role.Export = "" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If role_view.Pager Is Nothing Then role_view.Pager = New cPrevNextPager(role_view.lStartRec, role_view.lDisplayRecs, role_view.lTotalRecs) %>
<% If role_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If role_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= role_view.PageUrl %>start=<%= role_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If role_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= role_view.PageUrl %>start=<%= role_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= role_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If role_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= role_view.PageUrl %>start=<%= role_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If role_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= role_view.PageUrl %>start=<%= role_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= role_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If role_view.sSrchWhere = "0=101" Then %>
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
<% If role.RoleID.Visible Then ' RoleID %>
	<tr<%= role.RoleID.RowAttributes %>>
		<td class="ewTableHeader">Role ID</td>
		<td<%= role.RoleID.CellAttributes %>>
<div<%= role.RoleID.ViewAttributes %>><%= role.RoleID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If role.RoleName.Visible Then ' RoleName %>
	<tr<%= role.RoleName.RowAttributes %>>
		<td class="ewTableHeader">Role Name</td>
		<td<%= role.RoleName.CellAttributes %>>
<div<%= role.RoleName.ViewAttributes %>><%= role.RoleName.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If role.RoleTitle.Visible Then ' RoleTitle %>
	<tr<%= role.RoleTitle.RowAttributes %>>
		<td class="ewTableHeader">Role Title</td>
		<td<%= role.RoleTitle.CellAttributes %>>
<div<%= role.RoleTitle.ViewAttributes %>><%= role.RoleTitle.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If role.RoleComment.Visible Then ' RoleComment %>
	<tr<%= role.RoleComment.RowAttributes %>>
		<td class="ewTableHeader">Role Comment</td>
		<td<%= role.RoleComment.CellAttributes %>>
<div<%= role.RoleComment.ViewAttributes %>><%= role.RoleComment.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If role.FilterMenu.Visible Then ' FilterMenu %>
	<tr<%= role.FilterMenu.RowAttributes %>>
		<td class="ewTableHeader">Filter Menu</td>
		<td<%= role.FilterMenu.CellAttributes %>>
<% If Convert.ToString(role.FilterMenu.CurrentValue) = "1" Then %>
<input type="checkbox" value="<%= role.FilterMenu.ViewValue %>" checked onclick="this.form.reset();" disabled="disabled" />
<% Else %>
<input type="checkbox" value="<%= role.FilterMenu.ViewValue %>" onclick="this.form.reset();" disabled="disabled" />
<% End If %></td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<% If role.Export = "" Then %>
<br />
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If role_view.Pager Is Nothing Then role_view.Pager = New cPrevNextPager(role_view.lStartRec, role_view.lDisplayRecs, role_view.lTotalRecs) %>
<% If role_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If role_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= role_view.PageUrl %>start=<%= role_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If role_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= role_view.PageUrl %>start=<%= role_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= role_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If role_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= role_view.PageUrl %>start=<%= role_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If role_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= role_view.PageUrl %>start=<%= role_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= role_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If role_view.sSrchWhere = "0=101" Then %>
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
<% If role.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
