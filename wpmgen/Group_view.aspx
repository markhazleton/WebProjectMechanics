<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="Group_view.aspx.vb" Inherits="Group_view" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If Group.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var Group_view = new ew_Page("Group_view");
// page properties
Group_view.PageID = "view"; // page ID
var EW_PAGE_ID = Group_view.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
Group_view.ValidateRequired = true; // uses JavaScript validation
<% Else %>
Group_view.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">View TABLE: Contact Group
<br /><br />
<% If Group.Export = "" Then %>
<a href="Group_list.aspx">Back to List</a>&nbsp;
<a href="<%= Group.AddUrl %>">Add</a>&nbsp;
<a href="<%= Group.EditUrl %>">Edit</a>&nbsp;
<a href="<%= Group.CopyUrl %>">Copy</a>&nbsp;
<a href="<%= Group.DeleteUrl %>">Delete</a>&nbsp;
<% End If %>
</span></p>
<% Group_view.ShowMessage() %>
<p />
<% If Group.Export = "" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If Group_view.Pager Is Nothing Then Group_view.Pager = New cPrevNextPager(Group_view.lStartRec, Group_view.lDisplayRecs, Group_view.lTotalRecs) %>
<% If Group_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If Group_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= Group_view.PageUrl %>start=<%= Group_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If Group_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= Group_view.PageUrl %>start=<%= Group_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= Group_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If Group_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= Group_view.PageUrl %>start=<%= Group_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If Group_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= Group_view.PageUrl %>start=<%= Group_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= Group_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If Group_view.sSrchWhere = "0=101" Then %>
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
<% If Group.GroupName.Visible Then ' GroupName %>
	<tr<%= Group.GroupName.RowAttributes %>>
		<td class="ewTableHeader">Group Name</td>
		<td<%= Group.GroupName.CellAttributes %>>
<div<%= Group.GroupName.ViewAttributes %>><%= Group.GroupName.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Group.GroupComment.Visible Then ' GroupComment %>
	<tr<%= Group.GroupComment.RowAttributes %>>
		<td class="ewTableHeader">Group Comment</td>
		<td<%= Group.GroupComment.CellAttributes %>>
<div<%= Group.GroupComment.ViewAttributes %>><%= Group.GroupComment.ViewValue %></div>
</td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<% If Group.Export = "" Then %>
<br />
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If Group_view.Pager Is Nothing Then Group_view.Pager = New cPrevNextPager(Group_view.lStartRec, Group_view.lDisplayRecs, Group_view.lTotalRecs) %>
<% If Group_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If Group_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= Group_view.PageUrl %>start=<%= Group_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If Group_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= Group_view.PageUrl %>start=<%= Group_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= Group_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If Group_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= Group_view.PageUrl %>start=<%= Group_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If Group_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= Group_view.PageUrl %>start=<%= Group_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= Group_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If Group_view.sSrchWhere = "0=101" Then %>
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
<% If Group.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
